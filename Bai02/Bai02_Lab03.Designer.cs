namespace Bai02
{
    partial class Bai02_Lab03
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
            this.btn_Listen = new System.Windows.Forms.Button();
            this.txt_Listen = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btn_Listen
            // 
            this.btn_Listen.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Listen.Location = new System.Drawing.Point(576, 61);
            this.btn_Listen.Name = "btn_Listen";
            this.btn_Listen.Size = new System.Drawing.Size(132, 45);
            this.btn_Listen.TabIndex = 0;
            this.btn_Listen.Text = "Listen";
            this.btn_Listen.UseVisualStyleBackColor = true;
            this.btn_Listen.Click += new System.EventHandler(this.btn_Listen_Click);
            // 
            // txt_Listen
            // 
            this.txt_Listen.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_Listen.Location = new System.Drawing.Point(108, 123);
            this.txt_Listen.Multiline = true;
            this.txt_Listen.Name = "txt_Listen";
            this.txt_Listen.Size = new System.Drawing.Size(600, 274);
            this.txt_Listen.TabIndex = 1;
            // 
            // Bai02_Lab03
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.txt_Listen);
            this.Controls.Add(this.btn_Listen);
            this.Name = "Bai02_Lab03";
            this.Text = "Bai02_Lab03";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_Listen;
        private System.Windows.Forms.TextBox txt_Listen;
    }
}

