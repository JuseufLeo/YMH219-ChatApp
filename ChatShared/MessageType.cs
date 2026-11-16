namespace ChatShared
{
    /// <summary>Sunucu ile istemci arasında gidip gelen mesajların türü.</summary>
    public enum MessageType
    {
        Join,       // istemci kullanıcı adıyla katılmak istiyor
        Chat,       // normal sohbet mesajı (herkese)
        Private,    // özel mesaj: sadece Receiver'a (ve gönderene) iletilir
        Leave,      // istemci ayrılıyor
        System,     // sunucunun bilgilendirme mesajı (ör. "Ali katıldı")
        UserList,   // çevrimiçi kullanıcıların listesi
        Error       // hata (ör. kullanıcı adı zaten alınmış)
    }
}
