using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileManager
{
    public partial class Login : Form
    {
        Form1 ParentForm;
        string username, password;

        public Login(string newUsername, string newPassword, Form1 newParentForm)
        {
            InitializeComponent();
            username = newUsername;
            password = newPassword;
            ParentForm = newParentForm;

            textBoxLogin.SelectionStart = textBoxLogin.Text.Length;
            textBoxLogin.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBoxLogin.Text == username && textBoxPasswors.Text == password)
                ParentForm.LoginAndPassOk();
            else
                MessageBox.Show("Login or password are incorrect!");
        }
    }
}
