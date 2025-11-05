using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bai06
{
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        private void btnClient_Click(object sender, EventArgs e)
        {
            Client clientForm = new Client();
            clientForm.Show();
        }

        private void btnServer_Click(object sender, EventArgs e)
        {
            Server serverForm = new Server();
            serverForm.Show();
        }


    }
}
