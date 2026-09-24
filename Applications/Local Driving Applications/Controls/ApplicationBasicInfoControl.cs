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

namespace DVLD_Project.Applications.Controls
{
    public partial class ApplicationBasicInfoControl : UserControl
    {

        clsApplications applicaiton;
        public ApplicationBasicInfoControl()
        {
            InitializeComponent();
        }

        public void LoadApplicationInfo(int ApplicationID)
        {
            applicaiton = clsApplications.FindBaseApplication(ApplicationID);
            if(applicaiton == null)
            {
                MessageBox.Show("Application With ID :" + ApplicationID + " is not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                resetControl();
                return;
            }
            lblApplicationID.Text = applicaiton.ApplicationID.ToString();
            lblApplicant.Text = clsPeople.getPersonNameByID( applicaiton.ApplicantPersonID);
            lblCreatedByUser.Text = clsUsers.getUserName(applicaiton.CreatedByUserID);
            lblDate.Text = applicaiton.ApplicationDate.ToShortDateString();
            lblFees.Text = applicaiton.PaidFees.ToString();
            lblStatusDate.Text = applicaiton.LastStatusDate.ToShortDateString();
            lblType.Text = clsApplicationTypes.getApplicationTypeByID(applicaiton.ApplicationTypeID).ApplicationTypeName;

            if (applicaiton.ApplicationStatus == 1)
                lblStatus.Text = "New";
            else if (applicaiton.ApplicationStatus == 2)
                lblStatus.Text = "Canceled";
            else
                lblStatus.Text = "Completed";

            llViewPersonInfo.Visible = true;

        }

        public void resetControl()
        {
            lblApplicationID.Text = "N/A";
            lblApplicant.Text = "N/A";
            lblCreatedByUser.Text = "N/A";
            lblDate.Text = "N/A";
            lblFees.Text = "N/A";
            lblStatusDate.Text = "N/A";
            lblType.Text = "N/A";
            lblStatus.Text = "N/A";
            llViewPersonInfo.Visible = false;
        }

        private void llViewPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            formShowPersonInfo form = new formShowPersonInfo(applicaiton.ApplicantPersonID);
            form.ShowDialog();
        }
    }
}
