using System.Net;
using System.Net.Sockets;
using ChatShared;

namespace ChatServerApp
{
    /// <summary>
    /// TcpListener ile istemcileri kabul eden, gelen mesajları tüm istemcilere dağıtan sohbet sunucusu.
    /// Arayüzden bağımsızdır; olup biteni olaylar (event) ile forma bildirir.
    /// </summary>
    public class ChatServer
    {
        private TcpListener listener;
        private readonly List<ClientConnection> clients = new List<ClientConnection>();
        private readonly object clientsLock = new object();

        public bool IsRunning { get; private set; }
        public int Port { get; private set; }

        /// <summary>Sunucu günlüğüne yazılacak bir satır oluştuğunda.</summary>
        public event EventHandler<string> Log;
        /// <summary>Bağlı istemci listesi değiştiğinde.</summary>
        public event EventHandler ClientsChanged;

        /// <summary>Bağlı istemcilerin anlık bir kopyası.</summary>
        public List<ClientConnection> Clients
        {
            get { lock (clientsLock) return clients.ToList(); }
        }

        /// <summary>Sunucuyu başlatır. Port kullanımdaysa SocketException fırlatır.</summary>
        public void Start(int port)
        {
            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            Port = port;
            IsRunning = true;
            WriteLog($"Sunucu {port} portunda başlatıldı.");
            _ = AcceptLoopAsync();
        }

        public void Stop()
        {
            if (!IsRunning)
                return;
            IsRunning = false;
            listener.Stop();
            foreach (ClientConnection c in Clients)
                c.Channel.Dispose();     // açık bağlantıları kapat
            lock (clientsLock) clients.Clear();
            ClientsChanged?.Invoke(this, EventArgs.Empty);
            WriteLog("Sunucu durduruldu.");
        }

        /// <summary>Yeni bağlantıları sürekli bekler.</summary>
        private async Task AcceptLoopAsync()
        {
            while (IsRunning)
            {
                TcpClient tcp;
                try
                {
                    tcp = await listener.AcceptTcpClientAsync();
                }
                catch (Exception ex) when (ex is SocketException || ex is ObjectDisposedException)
                {
                    break;   // sunucu durduruldu
                }
                _ = HandleClientAsync(new ClientConnection(new MessageChannel(tcp)));
            }
        }

        /// <summary>Tek bir istemciyle konuşur: önce katılma mesajını, sonra sohbet mesajlarını işler.</summary>
        private async Task HandleClientAsync(ClientConnection client)
        {
            bool joined = false;
            try
            {
                // İlk mesaj kullanıcı adını içeren Join olmalı
                ChatMessage first = await client.Channel.ReceiveAsync();
                if (first == null || first.Type != MessageType.Join)
                    return;

                string name = first.Sender?.Trim();
                if (string.IsNullOrEmpty(name))
                {
                    await client.Channel.SendAsync(new ChatMessage(MessageType.Error, "Sunucu", "Kullanıcı adı boş olamaz."));
                    return;
                }
                if (Clients.Any(c => c.UserName.Equals(name, StringComparison.OrdinalIgnoreCase)))
                {
                    await client.Channel.SendAsync(new ChatMessage(MessageType.Error, "Sunucu", $"'{name}' kullanıcı adı zaten kullanılıyor."));
                    return;
                }

                client.UserName = name;
                lock (clientsLock) clients.Add(client);
                joined = true;
                ClientsChanged?.Invoke(this, EventArgs.Empty);
                WriteLog($"{name} bağlandı ({client.RemoteEndPoint}).");
                await BroadcastAsync(new ChatMessage(MessageType.System, "Sunucu", $"{name} sohbete katıldı."));
                await BroadcastUserListAsync();

                // Mesaj döngüsü
                while (IsRunning)
                {
                    ChatMessage msg = await client.Channel.ReceiveAsync();
                    if (msg == null || msg.Type == MessageType.Leave)
                        break;
                    if (string.IsNullOrWhiteSpace(msg.Text))
                        continue;
                    if (msg.Type == MessageType.Private)
                    {
                        await SendPrivateAsync(client, msg.Receiver, msg.Text.Trim());
                        continue;
                    }
                    if (msg.Type != MessageType.Chat)
                        continue;

                    // Gönderen adı ve zaman sunucu tarafından belirlenir
                    var chat = new ChatMessage(MessageType.Chat, client.UserName, msg.Text.Trim());
                    WriteLog($"{chat.Sender}: {chat.Text}");
                    await BroadcastAsync(chat);
                }
            }
            catch (Exception ex) when (ex is IOException || ex is ObjectDisposedException || ex is SocketException)
            {
                // Bağlantı beklenmedik şekilde koptu; aşağıda temizlenecek
            }
            finally
            {
                client.Channel.Dispose();
                if (joined)
                    await RemoveClientAsync(client);
            }
        }

        /// <summary>
        /// Özel mesajı sadece alıcıya ve (görsün diye) gönderene iletir; diğer kullanıcılar görmez.
        /// Alıcı çevrimiçi değilse gönderene bilgi mesajı döner.
        /// </summary>
        private async Task SendPrivateAsync(ClientConnection sender, string receiverName, string text)
        {
            receiverName = receiverName?.Trim() ?? "";
            ClientConnection receiver = Clients.FirstOrDefault(c =>
                c.UserName.Equals(receiverName, StringComparison.OrdinalIgnoreCase));

            if (receiver == null)
            {
                await TrySendAsync(sender, new ChatMessage(MessageType.System, "Sunucu",
                    $"'{receiverName}' çevrimiçi değil, özel mesaj iletilemedi."));
                return;
            }
            if (receiver == sender)
            {
                await TrySendAsync(sender, new ChatMessage(MessageType.System, "Sunucu",
                    "Kendinize özel mesaj gönderemezsiniz."));
                return;
            }

            var pm = new ChatMessage(MessageType.Private, sender.UserName, text) { Receiver = receiver.UserName };
            // Sunucu günlüğüne özel mesajın içeriği yazılmaz, sadece kimden kime gittiği
            WriteLog($"{sender.UserName} → {receiver.UserName}: [özel mesaj]");
            await TrySendAsync(receiver, pm);
            await TrySendAsync(sender, pm);
        }

        /// <summary>Tek bir istemciye gönderir; bağlantı kopmuşsa sessizce geçer.</summary>
        private async Task TrySendAsync(ClientConnection c, ChatMessage message)
        {
            try
            {
                await c.Channel.SendAsync(message);
            }
            catch (Exception ex) when (ex is IOException || ex is ObjectDisposedException || ex is SocketException)
            {
            }
        }

        private async Task RemoveClientAsync(ClientConnection client)
        {
            bool removed;
            lock (clientsLock) removed = clients.Remove(client);
            if (!removed || !IsRunning)
                return;
            ClientsChanged?.Invoke(this, EventArgs.Empty);
            WriteLog($"{client.UserName} ayrıldı.");
            await BroadcastAsync(new ChatMessage(MessageType.System, "Sunucu", $"{client.UserName} sohbetten ayrıldı."));
            await BroadcastUserListAsync();
        }

        /// <summary>Mesajı bağlı tüm istemcilere gönderir. Gönderilemeyenleri atlar.</summary>
        private async Task BroadcastAsync(ChatMessage message)
        {
            foreach (ClientConnection c in Clients)
            {
                try
                {
                    await c.Channel.SendAsync(message);
                }
                catch (Exception ex) when (ex is IOException || ex is ObjectDisposedException || ex is SocketException)
                {
                    // Bu istemcinin bağlantısı kopmuş; kendi döngüsü onu listeden çıkaracak
                }
            }
        }

        private Task BroadcastUserListAsync()
        {
            var msg = new ChatMessage(MessageType.UserList, "Sunucu", "")
            {
                Users = Clients.Select(c => c.UserName).OrderBy(n => n).ToList()
            };
            return BroadcastAsync(msg);
        }

        private void WriteLog(string text) => Log?.Invoke(this, $"[{DateTime.Now:HH:mm:ss}] {text}");
    }
}
