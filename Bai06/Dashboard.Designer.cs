using System.Drawing;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace Bai06
{
    partial class Dashboard
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnClient = new Button();
            btnServer = new Button();
            SuspendLayout();
            // 
            // btnClient
            // 
            btnClient.Location = new Point(149, 139);
            btnClient.Name = "btnClient";
            btnClient.Size = new Size(150, 46);
            btnClient.TabIndex = 0;
            btnClient.Text = "Client";
            btnClient.UseVisualStyleBackColor = true;
            btnClient.Click += btnClient_Click;
            // 
            // btnServer
            // 
            btnServer.Location = new Point(473, 139);
            btnServer.Name = "btnServer";
            btnServer.Size = new Size(150, 46);
            btnServer.TabIndex = 1;
            btnServer.Text = "Server";
            btnServer.UseVisualStyleBackColor = true;
            btnServer.Click += btnServer_Click;
            // 
            // Dashboard
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnServer);
            Controls.Add(btnClient);
            Name = "Dashboard";
            Text = "Dashboard";
            ResumeLayout(false);
        }

        #endregion

        private Button btnClient;
        private Button btnServer;
    }
}