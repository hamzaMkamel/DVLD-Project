using BusinessLayer;
using DVLD_Project.Global_Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void txtBox_Validating(object sender, CancelEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if(string.IsNullOrEmpty(txt.Text))
            {
                errorProvider1.SetError(txt, "This Filed Can't Be Empty !");
            }
            else
                errorProvider1.SetError(txt, null);

        }

        public bool checkForErrors()
        {
            if(string.IsNullOrEmpty(errorProvider1.GetError(txtBUserName)) || string.IsNullOrEmpty(errorProvider1.GetError(txtBPassword)))
            {
                return false;
            }
            return true;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if(checkForErrors())
            {
                MessageBox.Show("Some fields contains invalid data, please review them and try again.",
                    "Error :", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }

            if(clsUsers.VerifyUserNameAndPassword(txtBUserName.Text , txtBPassword.Text))
            {
                clsUsers user = clsUsers.Find(txtBUserName.Text);
                if (user != null && user.isActive)
                {
                    GlobalClass.CurrentUser = user;
                    checkRememberMeOption();
                    MainForm form = new MainForm(this);
                    this.Hide();
                    form.Show();
                    
                }
                else
                {
                    MessageBox.Show("Sorry , this user is not active , Contact Admin.",
                    "Error :", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return;
                }
            }
            else
            {
                MessageBox.Show("Invalid UserName or Password.",
                    "Wrong Credential :", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void checkRememberMeOption()
        {
            if(checkBoxRememberMe.Checked)
            {
                clsUsers.StoreRememberMeInfo(txtBUserName.Text, txtBPassword.Text);
            }
            else
            {
                clsUsers.ClearRememberMeInfo();
            }

        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            string userName = "", password = "";
            clsUsers.ReadRememberMeInfo(ref userName, ref password);
            txtBUserName.Text = userName;
            txtBPassword.Text = password;
            if (string.IsNullOrEmpty(userName))
                checkBoxRememberMe.Checked = false;
        }
    }
}
