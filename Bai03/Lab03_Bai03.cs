using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bai03
{
    public partial class Lab03_Bai03 : Form
    {
        public Lab03_Bai03()
        {
            InitializeComponent();
        }

        private void btn_Server_Click(object sender, EventArgs e)
        {
            Server f = new Server();
            f.Show();
            
        }

        private void btn_Client_Click(object sender, EventArgs e)
        {
            Client f = new Client();
            f.Show();
            
        }

        private void Lab03_Bai03_Load(object sender, EventArgs e)
        {

        }
    }
}
