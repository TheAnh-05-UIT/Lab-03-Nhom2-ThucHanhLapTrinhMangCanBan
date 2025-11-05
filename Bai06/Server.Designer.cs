namespace Bai06
{
    partial class Server
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
            this.listenButton = new System.Windows.Forms.Button();
            this.logMsgBox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // listenButton
            // 
            this.listenButton.Location = new System.Drawing.Point(404, 40);
            this.listenButton.Name = "listenButton";
            this.listenButton.Size = new System.Drawing.Size(97, 31);
            this.listenButton.TabIndex = 0;
            this.listenButton.Text = "Listen";
            this.listenButton.UseVisualStyleBackColor = true;
            this.listenButton.Click += new System.EventHandler(this.listenButton_Click);
            // 
            // logMsgBox
            // 
            this.logMsgBox.Location = new System.Drawing.Point(12, 77);
            this.logMsgBox.Name = "logMsgBox";
            this.logMsgBox.Size = new System.Drawing.Size(489, 361);
            this.logMsgBox.TabIndex = 1;
            this.logMsgBox.Text = "";
            // 
            // Bai4_Server
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(513, 450);
            this.Controls.Add(this.logMsgBox);
            this.Controls.Add(this.listenButton);
            this.Name = "Bai4_Server";
            this.Text = "Bai4_Server";
            this.ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.Button listenButton;
        private System.Windows.Forms.RichTextBox logMsgBox;
    }
}