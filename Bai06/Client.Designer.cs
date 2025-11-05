namespace Bai06
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
            this.usernameBox = new System.Windows.Forms.TextBox();
            this.usernameLabel = new System.Windows.Forms.Label();
            this.messageLabel = new System.Windows.Forms.Label();
            this.connectButton = new System.Windows.Forms.Button();
            this.disconnectButton = new System.Windows.Forms.Button();
            this.sendMsgBox = new System.Windows.Forms.RichTextBox();
            this.sendButton = new System.Windows.Forms.Button();
            this.msgBox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // usernameBox
            // 
            this.usernameBox.Location = new System.Drawing.Point(88, 28);
            this.usernameBox.Name = "usernameBox";
            this.usernameBox.Size = new System.Drawing.Size(393, 22);
            this.usernameBox.TabIndex = 1;
            // 
            // usernameLabel
            // 
            this.usernameLabel.AutoSize = true;
            this.usernameLabel.Location = new System.Drawing.Point(12, 31);
            this.usernameLabel.Name = "usernameLabel";
            this.usernameLabel.Size = new System.Drawing.Size(70, 16);
            this.usernameLabel.TabIndex = 3;
            this.usernameLabel.Text = "Username";
            // 
            // messageLabel
            // 
            this.messageLabel.AutoSize = true;
            this.messageLabel.Location = new System.Drawing.Point(12, 354);
            this.messageLabel.Name = "messageLabel";
            this.messageLabel.Size = new System.Drawing.Size(64, 16);
            this.messageLabel.TabIndex = 4;
            this.messageLabel.Text = "Message";
            // 
            // connectButton
            // 
            this.connectButton.Location = new System.Drawing.Point(512, 23);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(107, 32);
            this.connectButton.TabIndex = 5;
            this.connectButton.Text = "Connect";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // disconnectButton
            // 
            this.disconnectButton.Location = new System.Drawing.Point(650, 23);
            this.disconnectButton.Name = "disconnectButton";
            this.disconnectButton.Size = new System.Drawing.Size(107, 32);
            this.disconnectButton.TabIndex = 6;
            this.disconnectButton.Text = "Disconnect";
            this.disconnectButton.UseVisualStyleBackColor = true;
            this.disconnectButton.Click += new System.EventHandler(this.disconnectButton_Click);
            // 
            // sendMsgBox
            // 
            this.sendMsgBox.Location = new System.Drawing.Point(15, 374);
            this.sendMsgBox.Name = "sendMsgBox";
            this.sendMsgBox.Size = new System.Drawing.Size(615, 42);
            this.sendMsgBox.TabIndex = 8;
            this.sendMsgBox.Text = "";
            // 
            // sendButton
            // 
            this.sendButton.Location = new System.Drawing.Point(659, 374);
            this.sendButton.Name = "sendButton";
            this.sendButton.Size = new System.Drawing.Size(98, 42);
            this.sendButton.TabIndex = 9;
            this.sendButton.Text = "Send";
            this.sendButton.UseVisualStyleBackColor = true;
            this.sendButton.Click += new System.EventHandler(this.sendButton_Click);
            // 
            // msgBox
            // 
            this.msgBox.Location = new System.Drawing.Point(15, 61);
            this.msgBox.Name = "msgBox";
            this.msgBox.Size = new System.Drawing.Size(773, 271);
            this.msgBox.TabIndex = 10;
            this.msgBox.Text = "";
            // 
            // Client
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.msgBox);
            this.Controls.Add(this.sendButton);
            this.Controls.Add(this.sendMsgBox);
            this.Controls.Add(this.disconnectButton);
            this.Controls.Add(this.connectButton);
            this.Controls.Add(this.messageLabel);
            this.Controls.Add(this.usernameLabel);
            this.Controls.Add(this.usernameBox);
            this.Name = "Client";
            this.Text = "Bai4_Client";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox usernameBox;
        private System.Windows.Forms.Label usernameLabel;
        private System.Windows.Forms.Label messageLabel;
        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.Button disconnectButton;
        private System.Windows.Forms.RichTextBox sendMsgBox;
        private System.Windows.Forms.Button sendButton;
        private System.Windows.Forms.RichTextBox msgBox;
    }

}