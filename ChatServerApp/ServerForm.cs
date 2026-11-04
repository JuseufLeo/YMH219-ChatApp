using System.Net;
using System.Net.Sockets;

namespace ChatServerApp
{
    /// <summary>
    /// Sunucu penceresi: sunucuyu başlatır/durdurur, bağlı istemcileri ve sunucu günlüğünü gösterir.
    /// </summary>
    public partial class ServerForm : Form
    {
        private readonly ChatServer server = new ChatServer();

        public ServerForm()
        {
            InitializeComponent();
            server.Log += (s, line) => RunOnUi(() => AddLog(line));
            server.ClientsChanged += (s, e) => RunOnUi(RefreshClients);
            UpdateStatus();
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            try
            {
                server.Start((int)numPort.Value);
            }
            catch (SocketException ex)
            {
                MessageBox.Show($"Sunucu başlatılamadı. Port başka bir program tarafından kullanılıyor olabilir.\n\n{ex.Message}",
                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            UpdateStatus();
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            server.Stop();
            UpdateStatus();
        }

        private void ServerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            server.Stop();
        }

        // ---------- Arayüz güncelleme ----------

        private void UpdateStatus()
        {
            bool running = server.IsRunning;
            lblStatus.Text = running ? $"● ÇALIŞIYOR (:{server.Port})" : "● Durduruldu";
            lblStatus.BackColor = running ? Color.FromArgb(46, 125, 50) : Color.FromArgb(158, 158, 158);
            btnStart.Enabled = !running;
            btnStop.Enabled = running;
            btnStart.BackColor = running ? Color.Silver : Color.FromArgb(46, 125, 50);
            btnStop.BackColor = running ? Color.FromArgb(198, 40, 40) : Color.Silver;
            numPort.Enabled = !running;
            lblAddress.Text = running
                ? $"İstemciler bu bilgisayardan 127.0.0.1, yerel ağdan {LocalIPv4()} adresine, {server.Port} portuna bağlanabilir."
                : "Sunucu kapalı. Başlatmak için port seçip 'Sunucuyu Başlat'a basın.";
        }

        private void RefreshClients()
        {
            List<ClientConnection> clients = server.Clients;
            lvClients.Items.Clear();
            foreach (ClientConnection c in clients)
            {
                var item = new ListViewItem(c.UserName);
                item.SubItems.Add(c.RemoteEndPoint);
                item.SubItems.Add(c.ConnectedAt.ToString("dd.MM.yyyy HH:mm:ss"));
                lvClients.Items.Add(item);
            }
            lblClients.Text = $"Bağlı istemciler ({clients.Count})";
        }

        private void AddLog(string line)
        {
            lstLog.Items.Add(line);
            lstLog.TopIndex = lstLog.Items.Count - 1;
        }

        /// <summary>Kod başka bir iş parçacığından çağrılırsa arayüz iş parçacığına aktarır.</summary>
        private void RunOnUi(Action action)
        {
            if (IsDisposed) return;
            if (InvokeRequired) BeginInvoke(action);
            else action();
        }

        /// <summary>Bu bilgisayarın yerel ağdaki IPv4 adresi (yoksa 127.0.0.1).</summary>
        private static string LocalIPv4()
        {
            try
            {
                foreach (IPAddress ip in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
                    if (ip.AddressFamily == AddressFamily.InterNetwork && !IPAddress.IsLoopback(ip))
                        return ip.ToString();
            }
            catch (SocketException) { }
            return "127.0.0.1";
        }
    }
}
