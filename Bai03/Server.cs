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

namespace Bai03
{
    public partial class Server : Form
    {
        private TcpListener listener;
        private Thread listenerThread;
        private TcpClient client;
        public Server()
        {
            InitializeComponent();
        }

        private void btn_Listen_Click(object sender, EventArgs e)
        {
            try
            {
                IPAddress ip = IPAddress.Parse("127.0.0.1");
                int port = 8000;

                listener = new TcpListener(ip, port);

                listenerThread = new Thread(new ThreadStart(StartListening));
                listenerThread.IsBackground = true;
                listenerThread.Start();

                rtxt_Show.AppendText("Connection accepted from 127.0.0.1:8000" + Environment.NewLine);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khởi động server: " + ex.Message);
            }
        }

        private void StartListening()
        {
            try
            {
                listener.Start();
                while (true) 
                {
                    client = listener.AcceptTcpClient();

                    rtxt_Show.AppendText("Server started!" + Environment.NewLine);

                    Thread clientThread = new Thread(() => HandleClient(client));
                    clientThread.IsBackground=true;
                    clientThread.Start();
                }
            }
            catch (SocketException e)
            {
                MessageBox.Show("Lỗi Socket: " + e.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi trong StarListening: " + ex.Message );
            }
        }

        private void HandleClient(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];
            int bytesRead;

            try
            {
                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    string data = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    rtxt_Show.AppendText($"[Client]: {data}" + Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                rtxt_Show.AppendText("Client đã ngắt kết nối hoặc lỗi!");
            }
            finally
            {
                client.Close();
            }
        }

        private void Server_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (listener != null)
            {
                listener.Stop();
            }
            if (listenerThread != null)
            {
                listenerThread.Abort();
            }
            if (client != null)
            {
                client.Close();
            }
        }
    }
}
