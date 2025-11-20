using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bai02
{
    public partial class Bai02_Lab03 : Form
    {
        public Bai02_Lab03()
        {
            InitializeComponent();
        }

        private void btn_Listen_Click(object sender, EventArgs e)
        {
            void StartUnsafeThread()
            {
                int bytesReceived = 0;
                byte[] arr = new byte[1];
                Socket clientSocket;
                Socket listenerSocket = new Socket(
                    AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080);
                //Socket listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                listenerSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                listenerSocket.Bind(new IPEndPoint(IPAddress.Any, 8080));
                listenerSocket.Listen(-1);
                clientSocket = listenerSocket.Accept();
                txt_Listen.AppendText("New client connected" + Environment.NewLine);
                while (clientSocket.Connected)
                {
                    try
                    {
                        string text = "";
                        do
                        {
                            try
                            {
                                bytesReceived = clientSocket.Receive(arr);
                            }
                            catch (SocketException ex)
                            {
                                MessageBox.Show("Mất kết nối đến server: " + ex.Message);
                            }

                            string txt = System.Text.Encoding.UTF8.GetString(arr, 0, bytesReceived);
                            text += txt;

                        } while (text.Length > 0 && text[text.Length - 1] != '\n');

                        if (!txt_Listen.IsDisposed)
                        {
                            try
                            {
                                txt_Listen.Invoke(new Action(() => txt_Listen.AppendText(text)));
                            }
                            catch (ObjectDisposedException ex)
                            {
                                this.Invoke(new Action(() =>
                                {
                                    MessageBox.Show("Mất kết nối đến server: " + ex.Message);
                                }));
                            }
                        }
                    }
                    catch (SocketException ex)
                    {
                        MessageBox.Show("Lỗi kết nối đến server: " + ex.Message);
                    }
                    catch (ObjectDisposedException ex)
                    {
                        MessageBox.Show("Socket hoặc control đã bị dispose.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("dịch vụ telnet đã bị đóng: " + ex.Message);
                    }
                }
                txt_Listen.AppendText(Environment.NewLine);
                listenerSocket.Close();
            }

            CheckForIllegalCrossThreadCalls = false;
            Thread serverThread = new Thread(new ThreadStart(StartUnsafeThread));
            serverThread.IsBackground = true;
            serverThread.Start();
        }
    }
}
