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

namespace DVLD_Project.Users.Contols
{
    public partial class controlUserInformation : UserControl
    {

        public clsUsers user { set; get; }

        public controlUserInformation()
        {
            InitializeComponent();
        }

        public void LoadUserInfo(int UserID)
        {
            user = clsUsers.Find(UserID);
            if(user == null)
            {
                resetUserInfo();
                MessageBox.Show("Person With ID :" + UserID + " is not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            showPersonInfoControl1.LoadPersonInfo(user.PersonID);
            lblUserID.Text = user.UserID.ToString();
            lblUserName.Text = user.UserName;
            lblIsActive.Text = (user.isActive) ? "Yes" : "No";

        }

        private void resetUserInfo()
        {
            lblUserID.Text = "N/A";
            lblUserName.Text = "N/A";
            lblIsActive.Text = "N/A";
        }

       
    }
}
