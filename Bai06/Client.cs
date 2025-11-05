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
    }
}
