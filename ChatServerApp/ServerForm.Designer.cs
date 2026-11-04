namespace ChatServerApp
{
    partial class ServerForm
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
            lblPort = new Label();
            numPort = new NumericUpDown();
            btnStart = new Button();
            btnStop = new Button();
            lblStatus = new Label();
            lblAddress = new Label();
            lblClients = new Label();
            lvClients = new ListView();
            colName = new ColumnHeader();
            colEndPoint = new ColumnHeader();
            colTime = new ColumnHeader();
            lblLog = new Label();
            lstLog = new ListBox();
            ((System.ComponentModel.ISupportInitialize)numPort).BeginInit();
            SuspendLayout();
            //
            // lblPort
            //
            lblPort.AutoSize = true;
            lblPort.Location = new Point(12, 20);
            lblPort.Name = "lblPort";
            lblPort.Text = "Port:";
            //
            // numPort
            //
            numPort.Location = new Point(52, 16);
            numPort.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            numPort.Minimum = new decimal(new int[] { 1024, 0, 0, 0 });
            numPort.Name = "numPort";
            numPort.Size = new Size(80, 23);
            numPort.Value = new decimal(new int[] { 5000, 0, 0, 0 });
            //
            // btnStart
            //
            btnStart.BackColor = Color.FromArgb(46, 125, 50);
            btnStart.FlatStyle = FlatStyle.Flat;
            btnStart.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            btnStart.ForeColor = Color.White;
            btnStart.Location = new Point(146, 11);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(140, 32);
            btnStart.Text = "Sunucuyu Başlat";
            btnStart.UseVisualStyleBackColor = false;
            btnStart.Click += BtnStart_Click;
            //
            // btnStop
            //
            btnStop.BackColor = Color.FromArgb(198, 40, 40);
            btnStop.FlatStyle = FlatStyle.Flat;
            btnStop.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            btnStop.ForeColor = Color.White;
            btnStop.Location = new Point(296, 11);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(140, 32);
            btnStop.Text = "Sunucuyu Durdur";
            btnStop.UseVisualStyleBackColor = false;
            btnStop.Click += BtnStop_Click;
            //
            // lblStatus
            //
            lblStatus.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblStatus.ForeColor = Color.White;
            lblStatus.Location = new Point(530, 11);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(198, 32);
            lblStatus.TextAlign = ContentAlignment.MiddleCenter;
            //
            // lblAddress
            //
            lblAddress.ForeColor = Color.DimGray;
            lblAddress.Location = new Point(12, 52);
            lblAddress.Name = "lblAddress";
            lblAddress.Size = new Size(716, 20);
            //
            // lblClients
            //
            lblClients.AutoSize = true;
            lblClients.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            lblClients.Location = new Point(12, 80);
            lblClients.Name = "lblClients";
            lblClients.Text = "Bağlı istemciler (0)";
            //
            // lvClients
            //
            lvClients.Columns.AddRange(new ColumnHeader[] { colName, colEndPoint, colTime });
            lvClients.FullRowSelect = true;
            lvClients.GridLines = true;
            lvClients.Location = new Point(12, 102);
            lvClients.Name = "lvClients";
            lvClients.Size = new Size(716, 130);
            lvClients.View = View.Details;
            //
            // colName
            //
            colName.Text = "Kullanıcı adı";
            colName.Width = 200;
            //
            // colEndPoint
            //
            colEndPoint.Text = "Adres (IP:Port)";
            colEndPoint.Width = 260;
            //
            // colTime
            //
            colTime.Text = "Bağlanma zamanı";
            colTime.Width = 230;
            //
            // lblLog
            //
            lblLog.AutoSize = true;
            lblLog.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            lblLog.Location = new Point(12, 244);
            lblLog.Name = "lblLog";
            lblLog.Text = "Sunucu günlüğü";
            //
            // lstLog
            //
            lstLog.Font = new Font("Consolas", 9.75F);
            lstLog.HorizontalScrollbar = true;
            lstLog.IntegralHeight = false;
            lstLog.Location = new Point(12, 266);
            lstLog.Name = "lstLog";
            lstLog.Size = new Size(716, 250);
            //
            // ServerForm
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(740, 528);
            Controls.Add(lblPort);
            Controls.Add(numPort);
            Controls.Add(btnStart);
            Controls.Add(btnStop);
            Controls.Add(lblStatus);
            Controls.Add(lblAddress);
            Controls.Add(lblClients);
            Controls.Add(lvClients);
            Controls.Add(lblLog);
            Controls.Add(lstLog);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "ServerForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Chat Sunucusu - YMH219";
            FormClosing += ServerForm_FormClosing;
            ((System.ComponentModel.ISupportInitialize)numPort).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private Label lblPort;
        private NumericUpDown numPort;
        private Button btnStart;
        private Button btnStop;
        private Label lblStatus;
        private Label lblAddress;
        private Label lblClients;
        private ListView lvClients;
        private ColumnHeader colName;
        private ColumnHeader colEndPoint;
        private ColumnHeader colTime;
        private Label lblLog;
        private ListBox lstLog;
    }
}
