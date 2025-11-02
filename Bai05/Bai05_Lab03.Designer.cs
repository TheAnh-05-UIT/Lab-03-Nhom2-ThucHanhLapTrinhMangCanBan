namespace Bai05
{
    partial class Bai05_Lab03
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
            this.lblIP = new System.Windows.Forms.Label();
            this.lblPort = new System.Windows.Forms.Label();
            this.lblUser = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnListMy = new System.Windows.Forms.Button();
            this.btnListAll = new System.Windows.Forms.Button();
            this.lbOutput = new System.Windows.Forms.ListBox();
            this.txtServerIP = new System.Windows.Forms.TextBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtNew = new System.Windows.Forms.TextBox();
            this.btnRandomMy = new System.Windows.Forms.Button();
            this.btnRandomAll = new System.Windows.Forms.Button();
            this.lblNew = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblIP
            // 
            this.lblIP.AutoSize = true;
            this.lblIP.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIP.Location = new System.Drawing.Point(46, 31);
            this.lblIP.Name = "lblIP";
            this.lblIP.Size = new System.Drawing.Size(30, 25);
            this.lblIP.TabIndex = 0;
            this.lblIP.Text = "IP";
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPort.Location = new System.Drawing.Point(451, 33);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(47, 25);
            this.lblPort.TabIndex = 1;
            this.lblPort.Text = "Port";
            // 
            // lblUser
            // 
            this.lblUser.AutoSize = true;
            this.lblUser.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUser.Location = new System.Drawing.Point(21, 89);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(102, 25);
            this.lblUser.TabIndex = 2;
            this.lblUser.Text = "Username";
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(568, 124);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(148, 48);
            this.btnAdd.TabIndex = 6;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnListMy
            // 
            this.btnListMy.Location = new System.Drawing.Point(201, 204);
            this.btnListMy.Name = "btnListMy";
            this.btnListMy.Size = new System.Drawing.Size(132, 54);
            this.btnListMy.TabIndex = 7;
            this.btnListMy.Text = "List My";
            this.btnListMy.UseVisualStyleBackColor = true;
            this.btnListMy.Click += new System.EventHandler(this.btnListMy_Click);
            // 
            // btnListAll
            // 
            this.btnListAll.Location = new System.Drawing.Point(201, 328);
            this.btnListAll.Name = "btnListAll";
            this.btnListAll.Size = new System.Drawing.Size(132, 54);
            this.btnListAll.TabIndex = 8;
            this.btnListAll.Text = "List All";
            this.btnListAll.UseVisualStyleBackColor = true;
            this.btnListAll.Click += new System.EventHandler(this.btnListAll_Click);
            // 
            // lbOutput
            // 
            this.lbOutput.FormattingEnabled = true;
            this.lbOutput.ItemHeight = 20;
            this.lbOutput.Location = new System.Drawing.Point(377, 204);
            this.lbOutput.Name = "lbOutput";
            this.lbOutput.Size = new System.Drawing.Size(409, 224);
            this.lbOutput.TabIndex = 9;
            // 
            // txtServerIP
            // 
            this.txtServerIP.Location = new System.Drawing.Point(137, 34);
            this.txtServerIP.Name = "txtServerIP";
            this.txtServerIP.Size = new System.Drawing.Size(231, 26);
            this.txtServerIP.TabIndex = 10;
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(525, 32);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(231, 26);
            this.txtPort.TabIndex = 11;
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(137, 90);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(231, 26);
            this.txtUsername.TabIndex = 12;
            // 
            // txtNew
            // 
            this.txtNew.Location = new System.Drawing.Point(525, 92);
            this.txtNew.Name = "txtNew";
            this.txtNew.Size = new System.Drawing.Size(231, 26);
            this.txtNew.TabIndex = 14;
            // 
            // btnRandomMy
            // 
            this.btnRandomMy.Location = new System.Drawing.Point(26, 204);
            this.btnRandomMy.Name = "btnRandomMy";
            this.btnRandomMy.Size = new System.Drawing.Size(132, 54);
            this.btnRandomMy.TabIndex = 15;
            this.btnRandomMy.Text = "Random My";
            this.btnRandomMy.UseVisualStyleBackColor = true;
            this.btnRandomMy.Click += new System.EventHandler(this.btnRandomMy_Click);
            // 
            // btnRandomAll
            // 
            this.btnRandomAll.Location = new System.Drawing.Point(26, 328);
            this.btnRandomAll.Name = "btnRandomAll";
            this.btnRandomAll.Size = new System.Drawing.Size(132, 54);
            this.btnRandomAll.TabIndex = 16;
            this.btnRandomAll.Text = "Random All";
            this.btnRandomAll.UseVisualStyleBackColor = true;
            this.btnRandomAll.Click += new System.EventHandler(this.btnRandomAll_Click);
            // 
            // lblNew
            // 
            this.lblNew.AutoSize = true;
            this.lblNew.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNew.Location = new System.Drawing.Point(402, 91);
            this.lblNew.Name = "lblNew";
            this.lblNew.Size = new System.Drawing.Size(107, 25);
            this.lblNew.TabIndex = 17;
            this.lblNew.Text = "Thêm Món";
            // 
            // Bai05_Lab03
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lblNew);
            this.Controls.Add(this.btnRandomAll);
            this.Controls.Add(this.btnRandomMy);
            this.Controls.Add(this.txtNew);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.txtServerIP);
            this.Controls.Add(this.lbOutput);
            this.Controls.Add(this.btnListAll);
            this.Controls.Add(this.btnListMy);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.lblUser);
            this.Controls.Add(this.lblPort);
            this.Controls.Add(this.lblIP);
            this.Name = "Bai05_Lab03";
            this.Text = "Bai05_Lab03";
            this.Load += new System.EventHandler(this.Bai05_Lab03_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblIP;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnListMy;
        private System.Windows.Forms.Button btnListAll;
        private System.Windows.Forms.ListBox lbOutput;
        private System.Windows.Forms.TextBox txtServerIP;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtNew;
        private System.Windows.Forms.Button btnRandomMy;
        private System.Windows.Forms.Button btnRandomAll;
        private System.Windows.Forms.Label lblNew;
    }
}

