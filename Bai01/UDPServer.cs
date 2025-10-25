using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Bai01
{
    public partial class UDPServer : Form
    {
        private Thread listenerThread;
        private UdpClient listener;
        private volatile bool listening = false;

        public UDPServer()
        {
            InitializeComponent();
        }
        private void InfoMessage(string message)
        {
            try
            {
                if (ltvReceived.InvokeRequired)
                {
                    ltvReceived.Invoke(new Action(() => InfoMessage(message)));
                    return;
                }

                if (!ltvReceived.IsDisposed)
                {
                    ltvReceived.Items.Add(message);
                    ltvReceived.EnsureVisible(ltvReceived.Items.Count - 1);
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show("Kết nối đến server đã bị mất.");
            }
            catch (ObjectDisposedException ex)
            {
                MessageBox.Show("Socket đã bị đóng.");
            }
            catch (SocketException ex)
            {
                MessageBox.Show("Không thể kết nối đến server.");
            }
        }


        private void btnListen_Click(object sender, EventArgs e)
        {
            if (listening)
            {
                MessageBox.Show("Đã đang lắng nghe rồi.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!int.TryParse(txtPort.Text.Trim(), out int port))
            {
                MessageBox.Show("Port không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            listening = true;
            btnListen.Enabled = false; 
            listenerThread = new Thread(() => ListenLoop(port));
            listenerThread.IsBackground = true;
            listenerThread.SetApartmentState(ApartmentState.STA);
            listenerThread.Start();
        }

        private void ListenLoop(int port)
        {
            try
            {
                listener = new UdpClient(port);
                IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, port);              
                while (listening)
                {
                    byte[] receivedBytes = listener.Receive(ref groupEP);
                    string returnData = Encoding.UTF8.GetString(receivedBytes);
                    string mess = $"{groupEP.Address}: {returnData}";
                    InfoMessage(mess);
                }
            }
            catch (ObjectDisposedException)
            {
               
            }
            catch (SocketException ex)
            {
                InfoMessage($"SocketException: {ex.Message}");
            }
            catch (Exception ex)
            {
                InfoMessage($"Exception: {ex.Message}");
            }
            finally
            {
                listener?.Close();
                listener = null;
                listening = false;
                if (this.IsHandleCreated)
                {
                    this.BeginInvoke(new Action(() => btnListen.Enabled = true));
                }
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            listening = false;
            try 
            { 
                listener?.Close(); 
            } 
            catch 
            { 
            }
            if (listenerThread != null && listenerThread.IsAlive)
            {
                listenerThread.Join(500);
            }

            base.OnFormClosing(e);
        }

        private void ltvReceived_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void UDPServer_Load(object sender, EventArgs e)
        {
            
        }
    }
}
