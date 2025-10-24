using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bai03
{
    public partial class Client : Form
    {
        private TcpClient client;
        private NetworkStream stream;
        private Thread receiveThread;

        private const string Serverip = "127.0.0.1";
        private const int port = 8000;
        public Client()
        {
            InitializeComponent();
            btn_Send.Enabled = false;
        }

        private void btn_Connect_Click(object sender, EventArgs e)
        {
            try
            {
                client = new TcpClient();
                client.Connect(Serverip, port);

                stream = client.GetStream();

                receiveThread = new Thread(new ThreadStart(ReceiveMessages));
                receiveThread.IsBackground = true;
                receiveThread.Start();

                UpdateChatLog("Đã kết nối thành công đến Server!");
                btn_Connect.Enabled = false;
                btn_Send.Enabled = true;
            }
            catch (Exception ex) 
            {
                UpdateChatLog("Lỗi kết nối Server: " + ex.Message);
                btn_Connect.Enabled = true;
            }
        }

        private void btn_Send_Click(object sender, EventArgs e)
        {
            string Message = txtMessage.Text;
            if (string.IsNullOrWhiteSpace(Message) || stream == null) return;

            try
            {
                byte[] data = Encoding.ASCII.GetBytes(Message);

                stream.Write(data, 0, data.Length);

                UpdateChatLog($"[You]: {Message}");

                txtMessage.Clear();
            }
            catch (Exception ex)
            {
                UpdateChatLog("Lỗi gửi tin nhắn: " + ex.Message);
                Disconnect();
            }
        }

        private void UpdateChatLog(string message)
        {
            if (this.txtChatLog.InvokeRequired)
            {
                this.Invoke(new Action<string>(UpdateChatLog), new object[] { message });
            }
            else
            {
                txtChatLog.AppendText(message + Environment.NewLine);
            }
        }

        private void Disconnect()
        {
            if (client != null)
            {
                stream?.Close();
                client.Close();
                stream = null;
                client = null;
                UpdateChatLog("Đã ngắt kết nối.");

                
                this.Invoke(new Action(() => {
                    btn_Connect.Enabled = true;
                    btn_Send.Enabled = false;
                }));
            }
            
            if (receiveThread != null && receiveThread.IsAlive)
            {
                receiveThread.Abort();
            }
        }

        private void ReceiveMessages()
        {
            byte[] buffer = new byte[1024];
            int bytesRead;

            try
            {
                while (client.Connected)
                {
                    bytesRead = stream.Read(buffer, 0, buffer.Length);

                    if (bytesRead == 0)
                    {
                        UpdateChatLog("Server đã ngắt kết nối!");
                        break;
                    }

                    string response = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    UpdateChatLog($"[Server]: {response}");
                }
            }

            catch (Exception ex)
            {
                UpdateChatLog("Lỗi nhận tin nhắn hoặc kết nối thất bại" + ex.Message);
            }
            finally
            {
                Disconnect();
            }
        }
        private void ClientForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Disconnect();
        }

        private void Client_FormClosing(object sender, FormClosingEventArgs e)
        {
            Disconnect();
        }
    }
}
