using BusinessLayer;
using DVLD_Project.Global_Classes;
using DVLD_Project.Licenses;
using DVLD_Project.Licenses.LocalLicenses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project.Applications.RenewApplication
{
    public partial class RenewLicenseApplicationForm : Form
    {
        clsLicenses oldLicense;
        clsLicenses newLicense;
        public RenewLicenseApplicationForm()
        {
            InitializeComponent();
            fillApplicationGroupboxWithInfo();
        }

        private void fillApplicationGroupboxWithInfo()
        {
            lblApplicationDate.Text = DateTime.Now.ToShortDateString();
            lblIssueDate.Text = DateTime.Now.ToShortDateString();
            lblApplicationFees.Text = clsApplicationTypes.getApplicationTypeByID(2).ApplicationTypeFees.ToString();
            
            
            lblCreatedByUser.Text = GlobalClass.CurrentUser.UserName;
            llShowNewLicenseInfo.Enabled = false;
            btnRenew.Enabled = false;
            llShowLicenseHistory.Enabled = false;



        }

        private void showDriverLicenseWithFilterControl1_OnLicenseSelected(int LicenseID)
        {
            oldLicense = clsLicenses.Find(LicenseID);
            if(oldLicense == null)
            {
                return;
            }
            llShowLicenseHistory.Enabled = true;
            if(!oldLicense.IsActive)
            {
                MessageBox.Show("This License is not active please choose an active one.",
                    "Error :", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                btnRenew.Enabled = false;
                return;
            }
            if (!oldLicense.IsLicenseExpired())
            {
                MessageBox.Show($"This License is Not Expired Yet , it will be Expired on {oldLicense.ExpirationDate.ToShortDateString()}.",
                    "Error :", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                btnRenew.Enabled = false;
                return;
            }
            lblOldLicenseID.Text = oldLicense.LicenseID.ToString();
            lblExpirationDate.Text = DateTime.Now.AddYears(clsLicenseClasses.Find(oldLicense.LicenseClass).DefaultValidityLength).ToShortDateString();
            lblLicenseFees.Text = clsLicenseClasses.Find(oldLicense.LicenseClass).ClassFees.ToString();
            lblTotalFees.Text = (Convert.ToDecimal(lblApplicationFees.Text) + Convert.ToDecimal(lblLicenseFees.Text)).ToString();
            btnRenew.Enabled = true;

        }

        private void btnRenew_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are You Sure to issue this license ? ", "Take a Desision :", MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                DialogResult.No)
                return;

            newLicense = oldLicense.RenewLicense(textBoxNotes.Text, GlobalClass.CurrentUser.UserID);
            if(newLicense != null)
            {
                lblRenewedLicenseID.Text = newLicense.LicenseID.ToString();
                lblRenewLicenseApplicationID.Text = newLicense.ApplicationID.ToString();


                MessageBox.Show("License Issued Successfully!",
                    "Note :", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnRenew.Enabled = false;
                showDriverLicenseWithFilterControl1.FilterEnabled = false;
                llShowNewLicenseInfo.Enabled = true;
            }
            else
            {
                MessageBox.Show("Error Happened While Renewing the License .. Please Contact Adminstrator",
                    "Error :", MessageBoxButtons.OK, MessageBoxIcon.Hand);

                return;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void llShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ShowLicenseHistoryForm form = new ShowLicenseHistoryForm(showDriverLicenseWithFilterControl1.licenseInfo.applicationInfo.ApplicantPersonID);

            form.ShowDialog();
        }

        private void llShowNewLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ShowDriverLicenseInfoForm form = new ShowDriverLicenseInfoForm();
            form.loadDriverLicenseInfoByLicenseID(newLicense.LicenseID);
            form.ShowDialog();
        }
    }
}
