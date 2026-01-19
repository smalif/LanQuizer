// Your code is mostly correct. Only minor fixes applied.

using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace LanQuizer
{
    public class QuizHostForm : Form
    {
        Label lblIP, lblPort, lblStatus;
        Button btnStart, btnStop;

        Process webServerProcess;
        string lanIP;
        int port;
        string serverUrl;

        public QuizHostForm()
        {
            this.Text = "Quiz Host";
            this.Size = new Size(600, 350);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            lblIP = new Label() { Left = 40, Top = 40, Width = 500, Font = new Font("Segoe UI", 11, FontStyle.Bold) };
            lblPort = new Label() { Left = 40, Top = 80, Width = 500, Font = new Font("Segoe UI", 11) };
            lblStatus = new Label() { Left = 40, Top = 170, Width = 500, ForeColor = Color.DarkGreen, Font = new Font("Segoe UI", 10, FontStyle.Bold) };

            btnStart = new Button() { Text = "Start Quiz Hosting", Left = 40, Top = 120, Width = 200, Height = 40, BackColor = Color.FromArgb(33, 150, 243), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
            btnStop = new Button() { Text = "Stop Quiz", Left = 260, Top = 120, Width = 200, Height = 40, BackColor = Color.Red, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand, Enabled = false };

            btnStart.Click += BtnStart_Click;
            btnStop.Click += BtnStop_Click;

            this.Controls.Add(lblIP);
            this.Controls.Add(lblPort);
            this.Controls.Add(lblStatus);
            this.Controls.Add(btnStart);
            this.Controls.Add(btnStop);

            lanIP = GetLocalIPAddress();
            port = GetFreePort();
            serverUrl = $"http://{lanIP}:5000/Login";


            lblIP.Text = "Local IP Address: " + lanIP;
            lblPort.Text = "Port: " + port;
            lblStatus.Text = "Status: Not started";
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            try
            {
                // Kill previous instances
                foreach (var p in Process.GetProcessesByName("LanQuizWeb"))
                {
                    try { p.Kill(); p.WaitForExit(); } catch { }
                }

                string exePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LanQuizWeb.exe");
                if (!File.Exists(exePath))
                {
                    string devPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "LanQuizWeb", "bin", "Debug", "net8.0", "LanQuizWeb.exe"));
                    exePath = devPath;
                }

                if (!File.Exists(exePath))
                {
                    MessageBox.Show($"Server executable not found at:\n{exePath}");
                    return;
                }

                webServerProcess = new Process();
                webServerProcess.StartInfo.FileName = exePath;
                webServerProcess.StartInfo.Arguments = $"--urls http://0.0.0.0:{port}";
                webServerProcess.StartInfo.UseShellExecute = false;
                webServerProcess.StartInfo.CreateNoWindow = true;
                webServerProcess.StartInfo.WorkingDirectory = Path.GetDirectoryName(exePath);
                webServerProcess.Start();

                lblStatus.Text = $"Status: Quiz LIVE at {serverUrl}";
                btnStart.Enabled = false;
                btnStop.Enabled = true;

                // Wait a bit for server to bind
                Thread.Sleep(1500);

                try
                {
                    Process.Start(new ProcessStartInfo { FileName = serverUrl, UseShellExecute = true });
                }
                catch { /* ignore browser launch errors */ }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error starting server: " + ex.Message);
            }
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            try
            {
                if (webServerProcess != null && !webServerProcess.HasExited)
                {
                    webServerProcess.Kill();
                    webServerProcess.WaitForExit();
                    webServerProcess = null;
                }

                lblStatus.Text = "Status: Quiz stopped";
                btnStart.Enabled = true;
                btnStop.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to stop server: " + ex.Message);
            }
        }

        private string GetLocalIPAddress()
        {
            foreach (var ip in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
                if (ip.AddressFamily == AddressFamily.InterNetwork && ip.ToString().StartsWith("192.168."))
                    return ip.ToString();
            return "127.0.0.1";
        }

        private int GetFreePort()
        {
            TcpListener l = new TcpListener(IPAddress.Loopback, 0);
            l.Start();
            int p = ((IPEndPoint)l.LocalEndpoint).Port;
            l.Stop();
            return p;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            try
            {
                if (webServerProcess != null && !webServerProcess.HasExited)
                {
                    webServerProcess.Kill();
                    webServerProcess.WaitForExit();
                }
            }
            catch { }
            base.OnFormClosing(e);
        }
    }
}
