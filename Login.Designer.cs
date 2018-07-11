namespace FileManager
{
    partial class Login
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            this.textBoxInfo1 = new System.Windows.Forms.TextBox();
            this.TextBoxInfo2 = new System.Windows.Forms.TextBox();
            this.textBoxInfo3 = new System.Windows.Forms.TextBox();
            this.textBoxLogin = new System.Windows.Forms.TextBox();
            this.textBoxPasswors = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBoxInfo1
            // 
            this.textBoxInfo1.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.textBoxInfo1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxInfo1.Enabled = false;
            this.textBoxInfo1.Font = new System.Drawing.Font("Comic Sans MS", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxInfo1.Location = new System.Drawing.Point(12, 12);
            this.textBoxInfo1.Multiline = true;
            this.textBoxInfo1.Name = "textBoxInfo1";
            this.textBoxInfo1.ReadOnly = true;
            this.textBoxInfo1.Size = new System.Drawing.Size(337, 43);
            this.textBoxInfo1.TabIndex = 0;
            this.textBoxInfo1.Text = "Hellow! To continue using the product, enter the username and password!";
            this.textBoxInfo1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TextBoxInfo2
            // 
            this.TextBoxInfo2.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.TextBoxInfo2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TextBoxInfo2.Enabled = false;
            this.TextBoxInfo2.Font = new System.Drawing.Font("Comic Sans MS", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.TextBoxInfo2.Location = new System.Drawing.Point(12, 61);
            this.TextBoxInfo2.Name = "TextBoxInfo2";
            this.TextBoxInfo2.ReadOnly = true;
            this.TextBoxInfo2.Size = new System.Drawing.Size(118, 19);
            this.TextBoxInfo2.TabIndex = 1;
            this.TextBoxInfo2.Text = "User name:";
            this.TextBoxInfo2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxInfo3
            // 
            this.textBoxInfo3.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.textBoxInfo3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxInfo3.Enabled = false;
            this.textBoxInfo3.Font = new System.Drawing.Font("Comic Sans MS", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxInfo3.Location = new System.Drawing.Point(12, 111);
            this.textBoxInfo3.Name = "textBoxInfo3";
            this.textBoxInfo3.ReadOnly = true;
            this.textBoxInfo3.Size = new System.Drawing.Size(118, 19);
            this.textBoxInfo3.TabIndex = 2;
            this.textBoxInfo3.Text = "Password:";
            this.textBoxInfo3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxLogin
            // 
            this.textBoxLogin.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.textBoxLogin.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxLogin.Font = new System.Drawing.Font("Comic Sans MS", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxLogin.Location = new System.Drawing.Point(12, 86);
            this.textBoxLogin.Name = "textBoxLogin";
            this.textBoxLogin.Size = new System.Drawing.Size(337, 19);
            this.textBoxLogin.TabIndex = 3;
            this.textBoxLogin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxPasswors
            // 
            this.textBoxPasswors.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.textBoxPasswors.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxPasswors.Font = new System.Drawing.Font("Comic Sans MS", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxPasswors.Location = new System.Drawing.Point(12, 136);
            this.textBoxPasswors.Name = "textBoxPasswors";
            this.textBoxPasswors.Size = new System.Drawing.Size(337, 19);
            this.textBoxPasswors.TabIndex = 4;
            this.textBoxPasswors.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Comic Sans MS", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button1.Location = new System.Drawing.Point(274, 161);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "Ok!";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(361, 196);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBoxPasswors);
            this.Controls.Add(this.textBoxLogin);
            this.Controls.Add(this.textBoxInfo3);
            this.Controls.Add(this.TextBoxInfo2);
            this.Controls.Add(this.textBoxInfo1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Login";
            this.Text = "Login";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxInfo1;
        private System.Windows.Forms.TextBox TextBoxInfo2;
        private System.Windows.Forms.TextBox textBoxInfo3;
        private System.Windows.Forms.TextBox textBoxLogin;
        private System.Windows.Forms.TextBox textBoxPasswors;
        private System.Windows.Forms.Button button1;
    }
}