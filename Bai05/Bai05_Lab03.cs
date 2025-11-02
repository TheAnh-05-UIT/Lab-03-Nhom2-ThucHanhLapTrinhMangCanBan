using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.Json;

namespace Bai05
{
    public partial class Bai05_Lab03 : Form
    {
        public Bai05_Lab03()
        {
            InitializeComponent();
        }

        private void Bai05_Lab03_Load(object sender, EventArgs e)
        {

        }
        private JsonDocument SendRequest(JsonElement requestJson)
        {
            string server = txtServerIP.Text.Trim();
            int port = int.TryParse(txtPort.Text.Trim(), out int p) ? p : 3000;
            string reqStr = JsonSerializer.Serialize(requestJson);

            try
            {
                using (TcpClient client = new TcpClient())
                {
                    client.Connect(server, port);
                    using (var ns = client.GetStream())
                    {
                        byte[] outBuf = Encoding.UTF8.GetBytes(reqStr);
                        ns.Write(outBuf, 0, outBuf.Length);

                        // read response (simple approach)
                        byte[] inBuf = new byte[8192];
                        int bytes = ns.Read(inBuf, 0, inBuf.Length);
                        string resp = Encoding.UTF8.GetString(inBuf, 0, bytes);
                        var doc = JsonDocument.Parse(resp);
                        return doc;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error connecting server: " + ex.Message);
                return null;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string user = txtUsername.Text.Trim();
            string dish = txtNew.Text.Trim();
            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(dish))
            {
                MessageBox.Show("Enter username and dish");
                return;
            }

            var json = new Dictionary<string, object>()
            {
                ["action"] = "ADD",
                ["username"] = user,
                ["dish"] = dish
            };
            var reqJson = JsonSerializer.SerializeToElement(json);
            var resp = SendRequest(reqJson);
            if (resp != null)
            {
                var root = resp.RootElement;
                string status = root.GetProperty("status").GetString();
                if (status == "OK") MessageBox.Show("Thêm món thành công", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else MessageBox.Show("Error: " + root.GetProperty("message").GetString());
            }
        }

        private void btnListMy_Click(object sender, EventArgs e)
        {
            string user = txtUsername.Text.Trim();
            if (string.IsNullOrEmpty(user)) { MessageBox.Show("Enter username"); return; }
            var json = new Dictionary<string, object>() { ["action"] = "LIST", ["username"] = user };
            var reqJson = JsonSerializer.SerializeToElement(json);
            var resp = SendRequest(reqJson);
            lbOutput.Items.Clear();
            if (resp != null)
            {
                var root = resp.RootElement;
                if (root.GetProperty("status").GetString() == "OK")
                {
                    foreach (var it in root.GetProperty("items").EnumerateArray())
                        lbOutput.Items.Add(it.GetString());
                }
                else lbOutput.Items.Add("Error: " + root.GetProperty("message").GetString());
            }
        }

        private void btnRandomMy_Click(object sender, EventArgs e)
        {
            string user = txtUsername.Text.Trim();
            if (string.IsNullOrEmpty(user)) { MessageBox.Show("Enter username"); return; }
            var json = new Dictionary<string, object>() { ["action"] = "RANDOM", ["username"] = user };
            var reqJson = JsonSerializer.SerializeToElement(json);
            var resp = SendRequest(reqJson);
            if (resp != null)
            {
                var root = resp.RootElement;
                if (root.GetProperty("status").GetString() == "OK")
                    MessageBox.Show("Random (you): " + root.GetProperty("result").GetString(), "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else MessageBox.Show("Error: " + root.GetProperty("message").GetString());
            }
        }

        private void btnListAll_Click(object sender, EventArgs e)
        {
            var json = new Dictionary<string, object>() { ["action"] = "LISTALL" };
            var reqJson = JsonSerializer.SerializeToElement(json);
            var resp = SendRequest(reqJson);
            lbOutput.Items.Clear();
            if (resp != null)
            {
                var root = resp.RootElement;
                if (root.GetProperty("status").GetString() == "OK")
                {
                    foreach (var it in root.GetProperty("items").EnumerateArray())
                        lbOutput.Items.Add(it.GetString());
                }
                else lbOutput.Items.Add("Error: " + root.GetProperty("message").GetString());
            }
        }

        private void btnRandomAll_Click(object sender, EventArgs e)
        {
            var json = new Dictionary<string, object>() { ["action"] = "RANDOMALL" };
            var reqJson = JsonSerializer.SerializeToElement(json);
            var resp = SendRequest(reqJson);
            if (resp != null)
            {
                var root = resp.RootElement;
                if (root.GetProperty("status").GetString() == "OK")
                {
                    string result = root.GetProperty("result").GetString();
                    string by = root.GetProperty("by").GetString();
                    MessageBox.Show($"Random (community): {result} (by {by})", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else MessageBox.Show("Error: " + root.GetProperty("message").GetString());
            }
        }
    }
}
