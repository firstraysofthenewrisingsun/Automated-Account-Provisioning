namespace WinHRTool
{
    partial class InitialSetup
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
            this.initTbEmail = new System.Windows.Forms.TextBox();
            this.btnProtect = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.initTbEmailPass = new System.Windows.Forms.TextBox();
            this.initTbADUser = new System.Windows.Forms.TextBox();
            this.initTbADPath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.initTbADDomain = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.initTbADPass = new System.Windows.Forms.TextBox();
            this.submitEncBut = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.initTbADDC = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // initTbEmail
            // 
            this.initTbEmail.Location = new System.Drawing.Point(153, 111);
            this.initTbEmail.Name = "initTbEmail";
            this.initTbEmail.Size = new System.Drawing.Size(332, 26);
            this.initTbEmail.TabIndex = 0;
            // 
            // btnProtect
            // 
            this.btnProtect.Location = new System.Drawing.Point(257, 237);
            this.btnProtect.Name = "btnProtect";
            this.btnProtect.Size = new System.Drawing.Size(129, 46);
            this.btnProtect.TabIndex = 1;
            this.btnProtect.Text = "Encrypt Email";
            this.btnProtect.UseVisualStyleBackColor = true;
            this.btnProtect.Click += new System.EventHandler(this.encryptEmail_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(257, 657);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(129, 46);
            this.button2.TabIndex = 2;
            this.button2.Text = "Encrypt AD";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.encryptAD_Click);
            // 
            // initTbEmailPass
            // 
            this.initTbEmailPass.Location = new System.Drawing.Point(153, 181);
            this.initTbEmailPass.Name = "initTbEmailPass";
            this.initTbEmailPass.Size = new System.Drawing.Size(332, 26);
            this.initTbEmailPass.TabIndex = 3;
            // 
            // initTbADUser
            // 
            this.initTbADUser.Location = new System.Drawing.Point(149, 538);
            this.initTbADUser.Name = "initTbADUser";
            this.initTbADUser.Size = new System.Drawing.Size(332, 26);
            this.initTbADUser.TabIndex = 4;
            // 
            // initTbADPath
            // 
            this.initTbADPath.Location = new System.Drawing.Point(149, 467);
            this.initTbADPath.Name = "initTbADPath";
            this.initTbADPath.Size = new System.Drawing.Size(332, 26);
            this.initTbADPath.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(153, 88);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 20);
            this.label1.TabIndex = 6;
            this.label1.Text = "Enter Email:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(145, 444);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(176, 20);
            this.label2.TabIndex = 7;
            this.label2.Text = "Enter OU Domain Path:";
            // 
            // initTbADDomain
            // 
            this.initTbADDomain.Location = new System.Drawing.Point(149, 399);
            this.initTbADDomain.Name = "initTbADDomain";
            this.initTbADDomain.Size = new System.Drawing.Size(332, 26);
            this.initTbADDomain.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(149, 512);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(238, 20);
            this.label4.TabIndex = 10;
            this.label4.Text = "Enter Domain Admin Username:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(149, 373);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(111, 20);
            this.label5.TabIndex = 11;
            this.label5.Text = "Enter Domain:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(149, 158);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(168, 20);
            this.label6.TabIndex = 12;
            this.label6.Text = "Enter Email Password:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(149, 586);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(233, 20);
            this.label7.TabIndex = 14;
            this.label7.Text = "Enter Domain Admin Password:";
            // 
            // initTbADPass
            // 
            this.initTbADPass.Location = new System.Drawing.Point(149, 612);
            this.initTbADPass.Name = "initTbADPass";
            this.initTbADPass.Size = new System.Drawing.Size(332, 26);
            this.initTbADPass.TabIndex = 13;
            // 
            // submitEncBut
            // 
            this.submitEncBut.Location = new System.Drawing.Point(207, 732);
            this.submitEncBut.Name = "submitEncBut";
            this.submitEncBut.Size = new System.Drawing.Size(234, 68);
            this.submitEncBut.TabIndex = 15;
            this.submitEncBut.Text = "Next";
            this.submitEncBut.UseVisualStyleBackColor = true;
            this.submitEncBut.Click += new System.EventHandler(this.submitOnClick);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(293, 29);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 20);
            this.label3.TabIndex = 16;
            this.label3.Text = "label3";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(149, 304);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(135, 20);
            this.label8.TabIndex = 18;
            this.label8.Text = "Enter PDC Name:";
            // 
            // initTbADDC
            // 
            this.initTbADDC.Location = new System.Drawing.Point(153, 327);
            this.initTbADDC.Name = "initTbADDC";
            this.initTbADDC.Size = new System.Drawing.Size(332, 26);
            this.initTbADDC.TabIndex = 17;
            // 
            // InitialSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(650, 812);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.initTbADDC);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.submitEncBut);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.initTbADPass);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.initTbADDomain);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.initTbADPath);
            this.Controls.Add(this.initTbADUser);
            this.Controls.Add(this.initTbEmailPass);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btnProtect);
            this.Controls.Add(this.initTbEmail);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "InitialSetup";
            this.Text = "InitialSetup";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox initTbEmail;
        private System.Windows.Forms.Button btnProtect;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox initTbEmailPass;
        private System.Windows.Forms.TextBox initTbADUser;
        private System.Windows.Forms.TextBox initTbADPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox initTbADDomain;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox initTbADPass;
        private System.Windows.Forms.Button submitEncBut;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox initTbADDC;
    }
}