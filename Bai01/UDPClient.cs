using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
namespace Bai01
{
    public partial class UDPClient : Form
    {
        public UDPClient()
        {
            InitializeComponent();
        }

        private void txtIPHost_TextChanged(object sender, EventArgs e)
        {

        }
        private void txtMessage_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            UdpClient udpclient = null;
            try
            {
                 udpclient = new UdpClient();
                Byte[] sendBytes = Encoding.UTF8.GetBytes(txtMessage.Text);
                if(!int.TryParse(txtPort.Text.Trim(), out int port ))
                {
                    MessageBox.Show("Port không hợp lệ", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                udpclient.Send(sendBytes, sendBytes.Length, txtIPHost.Text, int.Parse(txtPort.Text));
            }
            catch(SocketException E)
            {
                MessageBox.Show("Lỗi: " + E.ToString(), "Thông báo!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch(Exception E)
            {
                MessageBox.Show("Lỗi: " + E.ToString(), "Thông báo!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
               udpclient.Close();
            }
        }

        private void txtPort_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
