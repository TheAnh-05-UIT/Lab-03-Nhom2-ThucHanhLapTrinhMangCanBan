namespace Bai01
{
    partial class UDPServer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnListen = new System.Windows.Forms.Button();
            this.lblPort = new System.Windows.Forms.Label();
            this.lblReceived = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.ltvReceived = new System.Windows.Forms.ListView();
            this.colReceived = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // btnListen
            // 
            this.btnListen.Location = new System.Drawing.Point(403, 29);
            this.btnListen.Name = "btnListen";
            this.btnListen.Size = new System.Drawing.Size(75, 23);
            this.btnListen.TabIndex = 0;
            this.btnListen.Text = "Listen";
            this.btnListen.UseVisualStyleBackColor = true;
            this.btnListen.Click += new System.EventHandler(this.btnListen_Click);
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(40, 29);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(26, 13);
            this.lblPort.TabIndex = 1;
            this.lblPort.Text = "Port";
            // 
            // lblReceived
            // 
            this.lblReceived.AutoSize = true;
            this.lblReceived.Location = new System.Drawing.Point(40, 61);
            this.lblReceived.Name = "lblReceived";
            this.lblReceived.Size = new System.Drawing.Size(103, 13);
            this.lblReceived.TabIndex = 2;
            this.lblReceived.Text = "Received messages";
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(81, 26);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(121, 20);
            this.txtPort.TabIndex = 3;
            // 
            // ltvReceived
            // 
            this.ltvReceived.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colReceived});
            this.ltvReceived.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ltvReceived.FullRowSelect = true;
            this.ltvReceived.GridLines = true;
            this.ltvReceived.HideSelection = false;
            this.ltvReceived.Location = new System.Drawing.Point(43, 77);
            this.ltvReceived.Name = "ltvReceived";
            this.ltvReceived.Size = new System.Drawing.Size(435, 200);
            this.ltvReceived.TabIndex = 4;
            this.ltvReceived.UseCompatibleStateImageBehavior = false;
            this.ltvReceived.View = System.Windows.Forms.View.Details;
            this.ltvReceived.SelectedIndexChanged += new System.EventHandler(this.ltvReceived_SelectedIndexChanged);
            // 
            // colReceived
            // 
            this.colReceived.Text = "";
            this.colReceived.Width = 400;
            // 
            // UDPServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(525, 300);
            this.Controls.Add(this.ltvReceived);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.lblReceived);
            this.Controls.Add(this.lblPort);
            this.Controls.Add(this.btnListen);
            this.Name = "UDPServer";
            this.Text = "UDPServer";
            this.Load += new System.EventHandler(this.UDPServer_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnListen;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.Label lblReceived;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.ListView ltvReceived;
        private System.Windows.Forms.ColumnHeader colReceived;
    }
}