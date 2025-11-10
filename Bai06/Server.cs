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

namespace Bai06
{
    public partial class Server : Form
    {
        public Server()
        {
            InitializeComponent();
        }
        private Thread listenThread;
        private TcpListener tcpListener;
        private bool stopChatServer = true;
        private readonly int _serverPort = 8080;
        private Dictionary<string, TcpClient> dict = new Dictionary<string, TcpClient>();

        public void Listen()
        {
            try
            {
                tcpListener = new TcpListener(IPAddress.Any, _serverPort);
                tcpListener.Start();
                UpdateChatHistorySafeCall($"[i] Listening on 0.0.0.0:{_serverPort}");

                while (!stopChatServer)
                {
                    TcpClient tcpClient = tcpListener.AcceptTcpClient();

                    var ns = tcpClient.GetStream();
                    var sReader = new StreamReader(ns, Encoding.UTF8);
                    var sWriter = new StreamWriter(ns, Encoding.UTF8) { AutoFlush = true };

                    string username = sReader.ReadLine();

                    if (string.IsNullOrWhiteSpace(username))
                    {
                        sWriter.WriteLine("Ten khong hop le");
                        tcpClient.Close();
                        continue;
                    }
                    if (dict.ContainsKey(username))
                    {
                        sWriter.WriteLine("Ten dang nhap da ton tai");
                        tcpClient.Close();
                    }
                    else
                    {
                        dict.Add(username, tcpClient);
                        UpdateChatHistorySafeCall($"Nguoi dung moi tham gia: {username}");
                        var th = new Thread(() => ClientRecv(username, tcpClient));
                        th.IsBackground = true;
                        th.Start();
                    }
                }
            }
            catch (SocketException sockEx)
            {
                MessageBox.Show(sockEx.Message);
            }
        }

        private delegate void SafeCallDelegate(string text);

        public void UpdateChatHistorySafeCall(string text)
        {
            if (logMsgBox.InvokeRequired)
            {
                var d = new SafeCallDelegate(UpdateChatHistorySafeCall);
                logMsgBox.Invoke(d, new object[] { text });
            }
            else
            {
                if (text[text.Length - 1] == '\n')
                {
                    logMsgBox.Text += text;
                }
                else
                {
                    logMsgBox.Text += text + "\n";
                }
            }
        }
        public void ClientRecv(string username, TcpClient tcpClient)
        {
            var sReader = new StreamReader(tcpClient.GetStream(), Encoding.UTF8);

            while (!stopChatServer)
            {
                try
                {
                    string line = sReader.ReadLine();
                    if (line == null) break;
                    if (line.StartsWith("FILE|"))
                    {
                        string payload = "FILE|" + username + "|" + line.Substring("FILE|".Length);
                        foreach (TcpClient other in dict.Values)
                        {
                            try
                            {
                                var sw = new StreamWriter(other.GetStream(), Encoding.UTF8) { AutoFlush = true };
                                sw.WriteLine(payload);
                            }
                            catch { }
                        }
                        try
                        {
                            var fname = line.Split('|')[1];
                            UpdateChatHistorySafeCall($"[FILE] {username} -> ALL: {fname}");
                        }
                        catch { }
                    }
                    else
                    {
                        foreach (TcpClient other in dict.Values)
                        {
                            try
                            {
                                var sw = new StreamWriter(other.GetStream(), Encoding.UTF8) { AutoFlush = true };
                                sw.WriteLine($"{username}: {line}");
                            }
                            catch { }
                        }
                        UpdateChatHistorySafeCall($"{username}: {line}");
                    }
                }
                catch
                {
                    break;
                }
            }
            if (dict.ContainsKey(username))
            {
                dict.Remove(username);
                UpdateChatHistorySafeCall($"{username} roi chat.");
            }
            try { tcpClient.Close(); } catch { }
        }

        private void listenButton_Click(object sender, EventArgs e)
        {
            if (stopChatServer)
            {
                stopChatServer = false;
                listenThread = new Thread(new ThreadStart(Listen));
                listenThread.IsBackground = true;
                listenThread.Start();
                listenButton.Text = @"Stop";
            }
            else
            {
                stopChatServer = true;
                tcpListener.Stop();
                listenThread = null;
                listenButton.Text = @"Listen";
            }
        }
    }
}

