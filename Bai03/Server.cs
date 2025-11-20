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
        private volatile bool isRunning = false;

        public Server()
        {
            InitializeComponent();
        }

        private void btn_Listen_Click(object sender, EventArgs e)
        {
            try
            {
                if (isRunning) return;

                listener = new TcpListener(IPAddress.Any, 8000);
                listener.Start();
                isRunning = true;

                listenerThread = new Thread(ListenLoop);
                listenerThread.IsBackground = true;
                listenerThread.Start();

                AppendText("Server started on port 8000...");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ListenLoop()
        {
            try
            {
                while (isRunning)
                {
                    TcpClient client;

                    try
                    {
                        client = listener.AcceptTcpClient();
                    }
                    catch (SocketException)
                    {
                        if (!isRunning) break;
                        throw;
                    }
                    catch (ObjectDisposedException)
                    {
                        break;
                    }

                    AppendText("Client connected: " +
                               client.Client.RemoteEndPoint);

                    Thread t = new Thread(() => HandleClient(client));
                    t.IsBackground = true;
                    t.Start();
                }
            }
            catch (Exception ex)
            {
                AppendText("ListenLoop error: " + ex.Message);
            }
        }

        private void HandleClient(TcpClient client)
        {
            try
            {
                NetworkStream stream = client.GetStream();
                byte[] buffer = new byte[1024];
                int bytes;

                while ((bytes = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    string msg = Encoding.UTF8.GetString(buffer, 0, bytes);
                    AppendText("[Client]: " + msg);
                }
            }
            catch (Exception)
            {
                AppendText("Client disconnected.");
            }
            finally
            {
                client.Close();
            }
        }

        private void AppendText(string msg)
        {
            if (!IsHandleCreated || IsDisposed) return;

            if (rtxt_Show.InvokeRequired)
            {
                rtxt_Show.Invoke(new Action<string>(AppendText), msg);
            }
            else
            {
                rtxt_Show.AppendText(msg + Environment.NewLine);
            }
        }

        private void Server_FormClosing(object sender, FormClosingEventArgs e)
        {
            isRunning = false;
            listener?.Stop();
            if (listenerThread != null && listenerThread.IsAlive)
            {
                listenerThread.Join(500);
            }
        }
    }
}
