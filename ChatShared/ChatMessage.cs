using System.Text.Json;
using System.Text.Json.Serialization;

namespace ChatShared
{
    /// <summary>
    /// Ağ üzerinden gönderilen tek bir mesaj. Her mesaj JSON'a çevrilip tek satır olarak gönderilir.
    /// </summary>
    public class ChatMessage
    {
        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter() }
        };

        public MessageType Type { get; set; }
        public string Sender { get; set; } = "";
        public string Text { get; set; } = "";
        /// <summary>Özel mesajın alıcısı. Herkese giden mesajlarda boştur.</summary>
        public string Receiver { get; set; } = "";
        public DateTime Time { get; set; } = DateTime.Now;
        public List<string> Users { get; set; }

        public ChatMessage() { }

        public ChatMessage(MessageType type, string sender, string text)
        {
            Type = type;
            Sender = sender;
            Text = text;
            Time = DateTime.Now;
        }

        public string ToJson() => JsonSerializer.Serialize(this, JsonOptions);

        /// <summary>JSON satırını mesaja çevirir; bozuksa null döner.</summary>
        public static ChatMessage FromJson(string json)
        {
            try
            {
                return JsonSerializer.Deserialize<ChatMessage>(json, JsonOptions);
            }
            catch (JsonException)
            {
                return null;
            }
        }

        /// <summary>Ekranda gösterilecek hâli, örn. "[14:05:12] Ayşe: Merhaba".</summary>
        public string ToDisplayString() => Type switch
        {
            MessageType.Chat => $"[{Time:HH:mm:ss}] {Sender}: {Text}",
            MessageType.Private => $"[{Time:HH:mm:ss}] (özel) {Sender} → {Receiver}: {Text}",
            MessageType.System => $"[{Time:HH:mm:ss}] *** {Text}",
            MessageType.Error => $"[{Time:HH:mm:ss}] HATA: {Text}",
            _ => $"[{Time:HH:mm:ss}] {Text}"
        };
    }
}
