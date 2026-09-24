using BusinessLayer;
using DVLD_Project.Global_Classes;
using DVLD_Project.Licenses;
using DVLD_Project.Licenses.International_Licenses;
using DVLD_Project.Licenses.International_Licenses.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project.Applications.International_Applications
{
    public partial class AddInternationalLicenseFrom : Form
    {
        clsInternationalLicenses internationalLicense;
        int localLicenseID;
        public AddInternationalLicenseFrom()
        {
            InitializeComponent();
            fillApplicationGroupboxWithInfo();
        }

        private void fillApplicationGroupboxWithInfo()
        {
            lblApplicationDate.Text = DateTime.Now.ToShortDateString();
            lblIssueDate.Text = DateTime.Now.ToShortDateString();
            lblExpirationDate.Text = DateTime.Now.AddYears(1).ToShortDateString();
            lblFees.Text = clsApplicationTypes.getApplicationTypeByID(6).ApplicationTypeFees.ToString();
            lblCreatedByUser.Text = GlobalClass.CurrentUser.UserName;
            llShowLicenseInfo.Enabled = false;
            btnIssue.Enabled = false;
            llShowLicenseHistory.Enabled = false;



        }

        private void showDriverLicenseWithFilterControl1_OnLicenseSelected(int LicenseID)
        {
            llShowLicenseHistory.Enabled = true;

            if(clsInternationalLicenses.isLicenseLinkedToInternationalLicense(LicenseID))
            {
                MessageBox.Show("This License already linked to active international license please pick another one.",
                    "Error :", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                btnIssue.Enabled = false;
                return;
            }

            if(!clsLicenses.isLicenseValidToBeAnInternationalLicense(LicenseID))
            {
                MessageBox.Show("This License Can't be used to issue an international license due to its class",
                    "Error :", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                btnIssue.Enabled = false;
                return;
            }

            lblLocalLicenseID.Text = LicenseID.ToString();
            btnIssue.Enabled = true;
            localLicenseID = LicenseID;

        }

        private void btnIssue_Click(object sender, EventArgs e)
        {
            internationalLicense = new clsInternationalLicenses();
            if(internationalLicense.IssueInternationalLicense(localLicenseID, GlobalClass.CurrentUser.UserID))
            {
                lblInternationalLicenseID.Text = internationalLicense.InternationalLicenseID.ToString();
                lblInternationalApplicationID.Text = internationalLicense.ApplicationID.ToString();
                llShowLicenseInfo.Enabled = true;

                MessageBox.Show("License Issued Successfully!",
                    "Note :", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnIssue.Enabled = false;
                showDriverLicenseWithFilterControl1.FilterEnabled = false;
            }
            else
            {
                MessageBox.Show("Error Happened While Issuing the License .. Please Contact Adminstrator",
                    "Error :", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                
                return;
            }

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void llShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ShowInternationalLicenseInfoForm form = new ShowInternationalLicenseInfoForm(localLicenseID);
            form.ShowDialog();
        }

        private void llShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ShowLicenseHistoryForm form = new ShowLicenseHistoryForm(showDriverLicenseWithFilterControl1.licenseInfo.applicationInfo.ApplicantPersonID);

            form.ShowDialog();
        }
    }
}
