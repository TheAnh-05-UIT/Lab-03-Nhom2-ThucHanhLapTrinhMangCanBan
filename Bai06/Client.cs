using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Bai06
{
    public partial class Client : Form
    {
        public Client()
        {
            InitializeComponent();
        }

        private TcpClient tcpClient;
        private string data;
        private const int serverPort = 8080;
        private const string Serverip = "127.0.0.1";
        private Thread clientThread;
        private StreamReader sReader;
        private StreamWriter sWriter;
        private bool stoptcpClient = true;

        private delegate void SafeCallDelegate(string text);
        private void UpdateChatHistorySafeCall(string text)
        {
            if (msgBox.InvokeRequired)
            {
                var d = new SafeCallDelegate(UpdateChatHistorySafeCall);
                msgBox.Invoke(d, new object[] { text });
            }
            else
            {
                msgBox.Text += text + "\n";
            }
        }
        private void ClientReceive()
        {
            sReader = new StreamReader(tcpClient.GetStream(), Encoding.UTF8);
            try
            {
                while (!stoptcpClient && tcpClient.Connected)
                {
                    string rcvdata;
                    try
                    {
                        rcvdata = sReader.ReadLine();
                    }
                    catch (IOException)
                    {
                        break;
                    }
                    if (rcvdata == null) break;
                    if (rcvdata.StartsWith("FILE|"))
                    {
                        var parts = rcvdata.Split(new char[] { '|' }, 5);
                        if (parts.Length == 5)
                        {
                            string senderName = parts[1];
                            string fileName = parts[2];
                            string mime = parts[3];
                            string b64 = parts[4];
                            try
                            {
                                byte[] bytes = Convert.FromBase64String(b64);
                                string safeName = string.Concat(fileName.Split(Path.GetInvalidFileNameChars()));
                                string savePath = Path.Combine(Path.GetTempPath(), $"{DateTime.Now:yyyyMMdd_HHmmss}_{safeName}");
                                File.WriteAllBytes(savePath, bytes);
                                UpdateChatHistorySafeCall($"[FILE] {fileName} luu: {savePath}");
                                try
                                {
                                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                                    {
                                        FileName = savePath,
                                        UseShellExecute = true
                                    });
                                }
                                catch { }
                            }
                            catch (Exception ex)
                            {
                                UpdateChatHistorySafeCall("Khong the luu file: " + ex.Message);
                            }
                            continue;
                        }
                    }
                    UpdateChatHistorySafeCall(rcvdata);
                }
            }
            finally
            {
                try { sReader?.Dispose(); } catch { }
            }
        }
        private void connectButton_Click(object sender, EventArgs e)
        {
            try
            {
                stoptcpClient = false;
                tcpClient = new TcpClient();
                tcpClient.Connect(Serverip, serverPort);
                sWriter = new StreamWriter(tcpClient.GetStream(), Encoding.UTF8) { AutoFlush = true };
                sWriter.WriteLine(usernameBox.Text.Trim());
                clientThread = new Thread(ClientReceive) { IsBackground = true };
                clientThread.Start();
                SetConnectedState(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void sendButton_Click(object sender, EventArgs e)
        {
            try
            {
                data = sendMsgBox.Text;
                sWriter.WriteLine(data);

                sendMsgBox.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void disconnectButton_Click(object sender, EventArgs e)
        {
            stoptcpClient = true;
            clientThread = null;
            tcpClient.Close();
            SetConnectedState(false);
            MessageBox.Show("Disconnected.");
        }
        private void SetConnectedState(bool connected)
        {
            connectButton.Visible = !connected;
            disconnectButton.Visible = connected;
            sendButton.Enabled = connected;
            sendMsgBox.Enabled = connected;
        }
        private static string GuessMime(string fileName)
        {
            string ext = Path.GetExtension(fileName).ToLowerInvariant();
            if (ext == ".png") return "image/png";
            if (ext == ".jpg") return "image/jpeg";
            if (ext == ".jpeg") return "image/jpeg";
            if (ext == ".txt") return "text/plain";
            return "other";
        }
        private void SendFileCore(string path)
        {
            var fi = new FileInfo(path);
            string fileName = fi.Name;
            string mime = GuessMime(fileName);
            string b64 = Convert.ToBase64String(File.ReadAllBytes(path));
            sWriter.WriteLine($"FILE|{fileName}|{mime}|{b64}");
            UpdateChatHistorySafeCall($"Ban da gui: {fileName} ({mime})");
        }
        private void sendFileButton_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog { Filter = "Files|*.png;*.jpg;*.jpeg;*.txt" } )
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    SendFileCore(ofd.FileName);
                }
            }
        }
    }
}
