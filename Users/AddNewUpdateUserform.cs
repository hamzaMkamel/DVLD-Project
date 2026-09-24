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
    public partial class AddNewUpdateUserform : Form
    {
        clsUsers user;
        clsUsers.enMode mode;
        public delegate void OnUserSaved();
        public event OnUserSaved onUserSaved;
        public AddNewUpdateUserform(int UserID)
        {
            InitializeComponent();
            if(UserID != -1)
            {
                filltabControlWithUserInfo(UserID);
                mode = clsUsers.enMode.UpdateUser;
                labelMode.Text = "Update User";
                btnNext.Enabled = true;
                DisableViabilityOfPasswordFields();

            }
            else
            {
                mode = clsUsers.enMode.AddNewUser;

            }
        }


        private void filltabControlWithUserInfo(int UserID)
        {
            user = clsUsers.Find(UserID);
            if(user == null)
            {
                MessageBox.Show("User Not Found..", "Error", MessageBoxButtons.OK , MessageBoxIcon.Error);
                return;
            }
            showPersonInfoWithFilterControl1.FilterEnabled = false;
            showPersonInfoWithFilterControl1.LoadPersonInfo(user.PersonID);
            
            lblUserID.Text = user.UserID.ToString();
            txtBUserName.Text = user.UserName;
            checkBoxIsActive.Checked = user.isActive;
            btnSave.Enabled = false;

        }

        private void showPersonInfoWithFilterControl1_OnPersonSelected(int obj)
        {
            if(clsUsers.isUserExists(obj))
            {
                MessageBox.Show("This Person You are Selecting is already an User!.", "Take Caution :", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            user = new clsUsers();
            user.PersonID = obj;
            btnNext.Enabled = true;

        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (user == null)
            {
                e.Cancel = true;
                MessageBox.Show("Please Select A Person First.", "Note :", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else e.Cancel = false;
        }

        private void txtBUserName_Validating(object sender, CancelEventArgs e)
        {
            if(string.IsNullOrEmpty(txtBUserName.Text))
            {
                errorProvider1.SetError(txtBUserName, "This Field Can't be Empty.");
            }
            else if(clsUsers.isUserNameExists(txtBUserName.Text))
            {
                errorProvider1.SetError(txtBUserName, "The UserName is already exists please choose another one");
            }
            else
            {
                errorProvider1.SetError(txtBUserName, null);
            }
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
            if(!txtBConfirmPassword.Text.Equals(txtBPassword.Text))
            {
                errorProvider1.SetError(txtBConfirmPassword, "Password Doesn't Match.");
            }
            else
                errorProvider1.SetError(txtBConfirmPassword, null);

        }

        void DisableViabilityOfPasswordFields()
        {
            txtBPassword.Enabled = false;
            txtBConfirmPassword.Enabled = false;
            errorProvider1.SetError(txtBConfirmPassword, null);
            errorProvider1.SetError(txtBPassword, null);
            linkLabel1.Visible = true;
            lblNote.Visible = true;
        }

        bool checkForErrors()
        {
            if (!string.IsNullOrEmpty(errorProvider1.GetError(txtBUserName)))
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
            user.UserName = txtBUserName.Text;
            user.isActive = checkBoxIsActive.Checked;
            if (mode == clsUsers.enMode.AddNewUser)
            {
                
                if(user.Save(txtBConfirmPassword.Text))
                {
                    lblUserID.Text = user.UserID.ToString();
                    MessageBox.Show("User Added Successfully with ID : " + user.UserID, "Note :", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    labelMode.Text = "Update User";
                    mode = clsUsers.enMode.UpdateUser;
                    DisableViabilityOfPasswordFields();
                    onUserSaved?.Invoke();
                }
                else
                {
                    MessageBox.Show("Error Happend While Adding User.", "Note : ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            else
            {
                if(user.Save(""))
                {
                    MessageBox.Show("User Updated Successfully!", "Note : ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    onUserSaved?.Invoke();
                }
                else
                {
                    MessageBox.Show("Error Happend While Updating User.", "Note : ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtBUserName_TextChanged(object sender, EventArgs e)
        {
            btnSave.Enabled = true;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ChangePasswordForm form = new ChangePasswordForm(user.UserID);
            form.ShowDialog();
        }
    }
}
