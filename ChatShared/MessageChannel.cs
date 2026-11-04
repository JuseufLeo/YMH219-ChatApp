using System.Net.Sockets;
using System.Text;

namespace ChatShared
{
    /// <summary>
    /// Bir TCP bağlantısı üzerinden mesaj gönderip almayı sağlar.
    /// Her mesaj bir satır (JSON + yeni satır) olarak yazılır; StreamReader satır satır okur.
    /// Hem sunucu hem istemci bu sınıfı kullanır.
    /// </summary>
    public class MessageChannel : IDisposable
    {
        private readonly TcpClient client;
        private readonly StreamReader reader;
        private readonly StreamWriter writer;
        private readonly SemaphoreSlim writeLock = new SemaphoreSlim(1, 1);   // aynı anda iki yazma olmasın

        public MessageChannel(TcpClient client)
        {
            this.client = client;
            NetworkStream stream = client.GetStream();
            var utf8 = new UTF8Encoding(false);
            reader = new StreamReader(stream, utf8);
            writer = new StreamWriter(stream, utf8) { AutoFlush = true };
        }

        public string RemoteEndPoint => client.Client?.RemoteEndPoint?.ToString() ?? "?";

        public async Task SendAsync(ChatMessage message)
        {
            await writeLock.WaitAsync();
            try
            {
                await writer.WriteLineAsync(message.ToJson());
            }
            finally
            {
                writeLock.Release();
            }
        }

        /// <summary>Bir sonraki mesajı bekler. Bağlantı kapanırsa null döner.</summary>
        public async Task<ChatMessage> ReceiveAsync()
        {
            while (true)
            {
                string line = await reader.ReadLineAsync();
                if (line == null)
                    return null;                       // karşı taraf bağlantıyı kapattı
                ChatMessage message = ChatMessage.FromJson(line);
                if (message != null)
                    return message;                    // bozuk satırları atla
            }
        }

        public void Dispose()
        {
            try { writer.Dispose(); } catch (IOException) { } catch (ObjectDisposedException) { }
            try { reader.Dispose(); } catch (IOException) { }
            client.Dispose();
        }
    }
}
