using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bai01
{
    public partial class frm_Bai01 : Form
    {
        public frm_Bai01()
        {
            InitializeComponent();
        }

        private void btnClient_Click(object sender, EventArgs e)
        {
            UDPClient f = new UDPClient();
            //this.Hide();
            f.Show();
            //this.Show();
        }

        private void btnServer_Click(object sender, EventArgs e)
        {
            UDPServer f = new UDPServer();
            //this.Hide();
            f.Show();
            //this.Show();
        }
    }
}
