namespace Bai03
{
    partial class Client
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
            this.txtChatLog = new System.Windows.Forms.TextBox();
            this.btn_Connect = new System.Windows.Forms.Button();
            this.btn_Send = new System.Windows.Forms.Button();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtChatLog
            // 
            this.txtChatLog.Location = new System.Drawing.Point(38, 79);
            this.txtChatLog.Multiline = true;
            this.txtChatLog.Name = "txtChatLog";
            this.txtChatLog.ReadOnly = true;
            this.txtChatLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtChatLog.Size = new System.Drawing.Size(620, 236);
            this.txtChatLog.TabIndex = 0;
            // 
            // btn_Connect
            // 
            this.btn_Connect.Location = new System.Drawing.Point(722, 79);
            this.btn_Connect.Name = "btn_Connect";
            this.btn_Connect.Size = new System.Drawing.Size(166, 50);
            this.btn_Connect.TabIndex = 1;
            this.btn_Connect.Text = "Connect";
            this.btn_Connect.UseVisualStyleBackColor = true;
            this.btn_Connect.Click += new System.EventHandler(this.btn_Connect_Click);
            // 
            // btn_Send
            // 
            this.btn_Send.Location = new System.Drawing.Point(722, 367);
            this.btn_Send.Name = "btn_Send";
            this.btn_Send.Size = new System.Drawing.Size(166, 50);
            this.btn_Send.TabIndex = 2;
            this.btn_Send.Text = "Send";
            this.btn_Send.UseVisualStyleBackColor = true;
            this.btn_Send.Click += new System.EventHandler(this.btn_Send_Click);
            // 
            // txtMessage
            // 
            this.txtMessage.Location = new System.Drawing.Point(38, 377);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(620, 31);
            this.txtMessage.TabIndex = 4;
            // 
            // Client
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(928, 481);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.btn_Send);
            this.Controls.Add(this.btn_Connect);
            this.Controls.Add(this.txtChatLog);
            this.Name = "Client";
            this.Text = "Client";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Client_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtChatLog;
        private System.Windows.Forms.Button btn_Connect;
        private System.Windows.Forms.Button btn_Send;
        private System.Windows.Forms.TextBox txtMessage;
    }
}

