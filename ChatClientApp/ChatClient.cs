using System.Net.Sockets;
using ChatShared;

namespace ChatClientApp
{
    /// <summary>
    /// Sunucuya TcpClient ile bağlanan, mesaj gönderen ve gelen mesajları dinleyen istemci.
    /// Arayüzden bağımsızdır; gelen mesajları ve bağlantı kopmasını olaylarla bildirir.
    /// </summary>
    public class ChatClient
    {
        private MessageChannel channel;
        private bool userDisconnected;

        public bool IsConnected { get; private set; }
        public string UserName { get; private set; }

        /// <summary>Sunucudan bir mesaj geldiğinde.</summary>
        public event EventHandler<ChatMessage> MessageReceived;
        /// <summary>Bağlantı kapandığında (sebep metniyle).</summary>
        public event EventHandler<string> Disconnected;

        /// <summary>Sunucuya bağlanır ve kullanıcı adıyla katılma mesajı gönderir.</summary>
        public async Task ConnectAsync(string host, int port, string userName)
        {
            var tcp = new TcpClient();
            using (var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(5)))
            {
                try
                {
                    await tcp.ConnectAsync(host, port, timeout.Token);
                }
                catch (OperationCanceledException)
                {
                    tcp.Dispose();
                    throw new TimeoutException("Sunucu 5 saniye içinde yanıt vermedi.");
                }
                catch
                {
                    tcp.Dispose();
                    throw;
                }
            }

            channel = new MessageChannel(tcp);
            UserName = userName;
            userDisconnected = false;
            await channel.SendAsync(new ChatMessage(MessageType.Join, userName, ""));
            IsConnected = true;
            _ = ReceiveLoopAsync();
        }

        public async Task SendAsync(string text)
        {
            if (!IsConnected)
                throw new InvalidOperationException("Sunucuya bağlı değilsiniz.");
            await channel.SendAsync(new ChatMessage(MessageType.Chat, UserName, text));
        }

        /// <summary>Kullanıcının isteğiyle bağlantıyı kapatır.</summary>
        public async Task DisconnectAsync()
        {
            if (!IsConnected)
                return;
            userDisconnected = true;
            try
            {
                await channel.SendAsync(new ChatMessage(MessageType.Leave, UserName, ""));
            }
            catch (Exception ex) when (ex is IOException || ex is ObjectDisposedException) { }
            CloseConnection("Bağlantı kesildi.");
        }

        /// <summary>Sunucudan gelen mesajları bağlantı kapanana kadar dinler.</summary>
        private async Task ReceiveLoopAsync()
        {
            string reason = "Sunucu bağlantısı koptu.";
            try
            {
                while (true)
                {
                    ChatMessage msg = await channel.ReceiveAsync();
                    if (msg == null)
                        break;
                    MessageReceived?.Invoke(this, msg);
                    if (msg.Type == MessageType.Error)
                    {
                        reason = "Sunucu bağlantıyı reddetti.";
                        break;
                    }
                }
            }
            catch (Exception ex) when (ex is IOException || ex is ObjectDisposedException || ex is SocketException)
            {
                // bağlantı koptu veya kullanıcı kendisi kapattı
            }

            if (!userDisconnected)
                CloseConnection(reason);
        }

        private void CloseConnection(string reason)
        {
            if (!IsConnected && channel == null)
                return;
            IsConnected = false;
            channel?.Dispose();
            channel = null;
            Disconnected?.Invoke(this, reason);
        }
    }
}
