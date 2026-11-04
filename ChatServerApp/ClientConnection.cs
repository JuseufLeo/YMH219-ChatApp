using ChatShared;

namespace ChatServerApp
{
    /// <summary>Sunucuya bağlı tek bir istemcinin bilgileri.</summary>
    public class ClientConnection
    {
        public MessageChannel Channel { get; }
        public string UserName { get; set; }
        public string RemoteEndPoint { get; }
        public DateTime ConnectedAt { get; }

        public ClientConnection(MessageChannel channel)
        {
            Channel = channel;
            RemoteEndPoint = channel.RemoteEndPoint;
            ConnectedAt = DateTime.Now;
        }
    }
}
