namespace ChatClientApp
{
    partial class ClientForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblHost = new Label();
            txtHost = new TextBox();
            lblPort = new Label();
            numPort = new NumericUpDown();
            lblUser = new Label();
            txtUser = new TextBox();
            btnConnect = new Button();
            btnDisconnect = new Button();
            lblStatus = new Label();
            lstMessages = new ListBox();
            lblUsers = new Label();
            lstUsers = new ListBox();
            txtMessage = new TextBox();
            btnSend = new Button();
            btnSaveHistory = new Button();
            saveFileDialog = new SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)numPort).BeginInit();
            SuspendLayout();
            //
            // lblHost
            //
            lblHost.AutoSize = true;
            lblHost.Location = new Point(12, 19);
            lblHost.Name = "lblHost";
            lblHost.Text = "Sunucu:";
            //
            // txtHost
            //
            txtHost.Location = new Point(66, 15);
            txtHost.Name = "txtHost";
            txtHost.Size = new Size(110, 23);
            txtHost.Text = "127.0.0.1";
            //
            // lblPort
            //
            lblPort.AutoSize = true;
            lblPort.Location = new Point(184, 19);
            lblPort.Name = "lblPort";
            lblPort.Text = "Port:";
            //
            // numPort
            //
            numPort.Location = new Point(220, 15);
            numPort.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            numPort.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numPort.Name = "numPort";
            numPort.Size = new Size(70, 23);
            numPort.Value = new decimal(new int[] { 5000, 0, 0, 0 });
            //
            // lblUser
            //
            lblUser.AutoSize = true;
            lblUser.Location = new Point(298, 19);
            lblUser.Name = "lblUser";
            lblUser.Text = "Kullanıcı adı:";
            //
            // txtUser
            //
            txtUser.Location = new Point(378, 15);
            txtUser.MaxLength = 20;
            txtUser.Name = "txtUser";
            txtUser.Size = new Size(110, 23);
            //
            // btnConnect
            //
            btnConnect.BackColor = Color.FromArgb(46, 125, 50);
            btnConnect.FlatStyle = FlatStyle.Flat;
            btnConnect.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnConnect.ForeColor = Color.White;
            btnConnect.Location = new Point(496, 11);
            btnConnect.Name = "btnConnect";
            btnConnect.Size = new Size(80, 30);
            btnConnect.Text = "Bağlan";
            btnConnect.UseVisualStyleBackColor = false;
            btnConnect.Click += BtnConnect_Click;
            //
            // btnDisconnect
            //
            btnDisconnect.BackColor = Color.FromArgb(198, 40, 40);
            btnDisconnect.FlatStyle = FlatStyle.Flat;
            btnDisconnect.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnDisconnect.ForeColor = Color.White;
            btnDisconnect.Location = new Point(582, 11);
            btnDisconnect.Name = "btnDisconnect";
            btnDisconnect.Size = new Size(120, 30);
            btnDisconnect.Text = "Bağlantıyı Kes";
            btnDisconnect.UseVisualStyleBackColor = false;
            btnDisconnect.Click += BtnDisconnect_Click;
            //
            // lblStatus
            //
            lblStatus.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            lblStatus.ForeColor = Color.White;
            lblStatus.Location = new Point(12, 50);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(736, 26);
            lblStatus.TextAlign = ContentAlignment.MiddleLeft;
            //
            // lstMessages
            //
            lstMessages.Font = new Font("Segoe UI", 10F);
            lstMessages.HorizontalScrollbar = true;
            lstMessages.IntegralHeight = false;
            lstMessages.Location = new Point(12, 86);
            lstMessages.Name = "lstMessages";
            lstMessages.Size = new Size(540, 380);
            //
            // lblUsers
            //
            lblUsers.AutoSize = true;
            lblUsers.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            lblUsers.Location = new Point(564, 86);
            lblUsers.Name = "lblUsers";
            lblUsers.Text = "Çevrimiçi (0)";
            //
            // lstUsers
            //
            lstUsers.Font = new Font("Segoe UI", 10F);
            lstUsers.IntegralHeight = false;
            lstUsers.Location = new Point(564, 108);
            lstUsers.Name = "lstUsers";
            lstUsers.Size = new Size(184, 358);
            //
            // txtMessage
            //
            txtMessage.Font = new Font("Segoe UI", 10.5F);
            txtMessage.Location = new Point(12, 478);
            txtMessage.MaxLength = 500;
            txtMessage.Name = "txtMessage";
            txtMessage.PlaceholderText = "Mesajınızı yazın ve Enter'a basın...";
            txtMessage.Size = new Size(440, 26);
            //
            // btnSend
            //
            btnSend.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            btnSend.Location = new Point(460, 476);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(92, 30);
            btnSend.Text = "Gönder";
            btnSend.Click += BtnSend_Click;
            //
            // btnSaveHistory
            //
            btnSaveHistory.Location = new Point(564, 476);
            btnSaveHistory.Name = "btnSaveHistory";
            btnSaveHistory.Size = new Size(184, 30);
            btnSaveHistory.Text = "Sohbet Geçmişini Kaydet";
            btnSaveHistory.Click += BtnSaveHistory_Click;
            //
            // saveFileDialog
            //
            saveFileDialog.DefaultExt = "txt";
            saveFileDialog.Filter = "Metin dosyası (*.txt)|*.txt";
            saveFileDialog.Title = "Sohbet geçmişini kaydet";
            //
            // ClientForm
            //
            AcceptButton = btnSend;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(760, 518);
            Controls.Add(lblHost);
            Controls.Add(txtHost);
            Controls.Add(lblPort);
            Controls.Add(numPort);
            Controls.Add(lblUser);
            Controls.Add(txtUser);
            Controls.Add(btnConnect);
            Controls.Add(btnDisconnect);
            Controls.Add(lblStatus);
            Controls.Add(lstMessages);
            Controls.Add(lblUsers);
            Controls.Add(lstUsers);
            Controls.Add(txtMessage);
            Controls.Add(btnSend);
            Controls.Add(btnSaveHistory);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "ClientForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Chat İstemcisi - YMH219";
            FormClosing += ClientForm_FormClosing;
            ((System.ComponentModel.ISupportInitialize)numPort).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private Label lblHost;
        private TextBox txtHost;
        private Label lblPort;
        private NumericUpDown numPort;
        private Label lblUser;
        private TextBox txtUser;
        private Button btnConnect;
        private Button btnDisconnect;
        private Label lblStatus;
        private ListBox lstMessages;
        private Label lblUsers;
        private ListBox lstUsers;
        private TextBox txtMessage;
        private Button btnSend;
        private Button btnSaveHistory;
        private SaveFileDialog saveFileDialog;
    }
}
