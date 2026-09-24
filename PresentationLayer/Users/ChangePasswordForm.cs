using BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project.Users
{
    public partial class ChangePasswordForm : Form
    {
        public ChangePasswordForm(int UserID)
        {
            InitializeComponent();
            controlUserInformation1.LoadUserInfo(UserID);
        }

        private void txtBPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtBPassword.Text))
            {
                errorProvider1.SetError(txtBPassword, "This Field Can't be Empty.");
            }

            else
                errorProvider1.SetError(txtBPassword, null);
        }

        private void txtBConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            if (!txtBConfirmPassword.Text.Equals(txtBPassword.Text))
            {
                errorProvider1.SetError(txtBConfirmPassword, "Password Doesn't Match.");
            }
            else
                errorProvider1.SetError(txtBConfirmPassword, null);
        }

        private void txtBCurrentPassword_Validating(object sender, CancelEventArgs e)
        {

            if (string.IsNullOrEmpty(txtBCurrentPassword.Text))
            {
                errorProvider1.SetError(txtBPassword, "This Field Can't be Empty.");
                return;
            }
            bool isCurrentPasswordTrue = clsUsers.VerifyUserNameAndPassword(controlUserInformation1.user.UserName, txtBCurrentPassword.Text);
            if(isCurrentPasswordTrue)
            {
                errorProvider1.SetError(txtBCurrentPassword, null);
            }
            else
            {
                errorProvider1.SetError(txtBCurrentPassword, "Password Is not true");
            }
        }

        bool checkForErrors()
        {
            if (!string.IsNullOrEmpty(errorProvider1.GetError(txtBCurrentPassword)))
                return true;
            else if (!string.IsNullOrEmpty(errorProvider1.GetError(txtBPassword)))
                return true;
            else if (!string.IsNullOrEmpty(errorProvider1.GetError(txtBConfirmPassword)))
                return true;
            return false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(checkForErrors())
            {
                MessageBox.Show("Some fields contains invalid data, please review them and try again.",
                    "Error :", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }

            if(clsUsers.ChangeUserPassword(controlUserInformation1.user.UserName , txtBConfirmPassword.Text))
            {
                MessageBox.Show("Password Changed Successfully.",
                    "Note :", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Somthing happend while changing password..(Please Call the adminstrator)",
                    "Error :", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
