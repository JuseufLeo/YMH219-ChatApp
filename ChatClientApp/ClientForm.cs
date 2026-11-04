using System.Net.Sockets;
using ChatShared;

namespace ChatClientApp
{
    /// <summary>
    /// İstemci penceresi: sunucuya bağlanma, mesaj gönderme, gelen mesajları ve çevrimiçi kullanıcıları gösterme.
    /// </summary>
    public partial class ClientForm : Form
    {
        private readonly ChatClient client = new ChatClient();

        public ClientForm()
        {
            InitializeComponent();
            client.MessageReceived += (s, msg) => RunOnUi(() => ShowMessage(msg));
            client.Disconnected += (s, reason) => RunOnUi(() => OnDisconnected(reason));
            UpdateStatus();
        }

        // ---------- Butonlar ----------

        private async void BtnConnect_Click(object sender, EventArgs e)
        {
            string host = txtHost.Text.Trim();
            string user = txtUser.Text.Trim();
            if (host.Length == 0 || user.Length == 0)
            {
                MessageBox.Show("Sunucu adresi ve kullanıcı adı boş olamaz.", "Eksik bilgi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnConnect.Enabled = false;
            lblStatus.Text = "  ● Bağlanılıyor...";
            lblStatus.BackColor = Color.FromArgb(245, 124, 0);
            AddLine($"[{DateTime.Now:HH:mm:ss}] *** {host}:{numPort.Value} sunucusuna bağlanılıyor...");
            try
            {
                await client.ConnectAsync(host, (int)numPort.Value, user);
                txtMessage.Focus();
            }
            catch (Exception ex) when (ex is SocketException || ex is TimeoutException || ex is IOException)
            {
                MessageBox.Show($"Sunucuya bağlanılamadı.\nSunucunun açık olduğundan ve adres/port bilgisinin doğru olduğundan emin olun.\n\n{ex.Message}",
                    "Bağlantı hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            UpdateStatus();
        }

        private async void BtnDisconnect_Click(object sender, EventArgs e)
        {
            await client.DisconnectAsync();
        }

        private async void BtnSend_Click(object sender, EventArgs e)
        {
            string text = txtMessage.Text.Trim();
            if (text.Length == 0 || !client.IsConnected)
                return;
            try
            {
                await client.SendAsync(text);
                txtMessage.Clear();
            }
            catch (Exception ex) when (ex is IOException || ex is ObjectDisposedException || ex is InvalidOperationException)
            {
                MessageBox.Show("Mesaj gönderilemedi: " + ex.Message, "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            txtMessage.Focus();
        }

        private void BtnSaveHistory_Click(object sender, EventArgs e)
        {
            if (lstMessages.Items.Count == 0)
            {
                MessageBox.Show("Kaydedilecek mesaj yok.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            saveFileDialog.FileName = $"sohbet_gecmisi_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
            if (saveFileDialog.ShowDialog(this) != DialogResult.OK)
                return;
            try
            {
                File.WriteAllLines(saveFileDialog.FileName, lstMessages.Items.Cast<string>());
                MessageBox.Show("Sohbet geçmişi kaydedildi.", "Kaydedildi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
            {
                MessageBox.Show("Dosya kaydedilemedi: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void ClientForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (client.IsConnected)
                await client.DisconnectAsync();
        }

        // ---------- Gelen olaylar ----------

        private void ShowMessage(ChatMessage msg)
        {
            if (msg.Type == MessageType.UserList)
            {
                lstUsers.Items.Clear();
                foreach (string u in msg.Users ?? new List<string>())
                    lstUsers.Items.Add(u == client.UserName ? u + " (sen)" : u);
                lblUsers.Text = $"Çevrimiçi ({lstUsers.Items.Count})";
                return;
            }
            AddLine(msg.ToDisplayString());
        }

        private void OnDisconnected(string reason)
        {
            AddLine($"[{DateTime.Now:HH:mm:ss}] *** {reason}");
            lstUsers.Items.Clear();
            lblUsers.Text = "Çevrimiçi (0)";
            UpdateStatus();
        }

        // ---------- Yardımcılar ----------

        private void AddLine(string line)
        {
            lstMessages.Items.Add(line);
            lstMessages.TopIndex = lstMessages.Items.Count - 1;
        }

        private void UpdateStatus()
        {
            bool connected = client.IsConnected;
            lblStatus.Text = connected ? $"  ● Bağlı — {client.UserName}" : "  ● Bağlı değil";
            lblStatus.BackColor = connected ? Color.FromArgb(46, 125, 50) : Color.FromArgb(158, 158, 158);
            btnConnect.Enabled = !connected;
            btnDisconnect.Enabled = connected;
            btnConnect.BackColor = connected ? Color.Silver : Color.FromArgb(46, 125, 50);
            btnDisconnect.BackColor = connected ? Color.FromArgb(198, 40, 40) : Color.Silver;
            txtHost.Enabled = txtUser.Enabled = numPort.Enabled = !connected;
            txtMessage.Enabled = btnSend.Enabled = connected;
        }

        private void RunOnUi(Action action)
        {
            if (IsDisposed) return;
            if (InvokeRequired) BeginInvoke(action);
            else action();
        }
    }
}
