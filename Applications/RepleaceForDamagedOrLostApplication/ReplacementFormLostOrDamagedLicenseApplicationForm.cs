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
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project.Applications.RepleaceForDamagedOrLostApplication
{
    public partial class ReplacementFormLostOrDamagedLicenseApplicationForm : Form
    {
        clsLicenses oldLicense;
        clsLicenses newLicense;
        public ReplacementFormLostOrDamagedLicenseApplicationForm()
        {
            InitializeComponent();
            fillApplicationGroupboxWithInfo();
        }

        private void fillApplicationGroupboxWithInfo()
        {
            lblApplicationDate.Text = DateTime.Now.ToShortDateString();
            
            lblApplicationFees.Text = clsApplicationTypes.getApplicationTypeByID(4).ApplicationTypeFees.ToString();


            lblCreatedByUser.Text = GlobalClass.CurrentUser.UserName;
            llShowNewLicenseInfo.Enabled = false;
            btnReplace.Enabled = false;
            llShowLicenseHistory.Enabled = false;



        }

        private void rbDamaged_CheckedChanged(object sender, EventArgs e)
        {
            if(rbDamaged.Checked)
            {
                lblApplicationFees.Text = clsApplicationTypes.getApplicationTypeByID(4).ApplicationTypeFees.ToString();
            }
            else
            {
                lblApplicationFees.Text = clsApplicationTypes.getApplicationTypeByID(3).ApplicationTypeFees.ToString();
            }
        }

        private void showDriverLicenseWithFilterControl1_OnLicenseSelected(int LicenseID)
        {
            oldLicense = clsLicenses.Find(LicenseID);
            if (oldLicense == null)
            {
                return;
            }
            llShowLicenseHistory.Enabled = true;
            if (!oldLicense.IsActive)
            {
                MessageBox.Show("This License is not active please Pick an active one.",
                    "Error :", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                btnReplace.Enabled = false;
                return;
            }
            
            lblOldLicenseID.Text = oldLicense.LicenseID.ToString();
            btnReplace.Enabled = true;
        }

        private void btnReplace_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("Are You Sure to issue this license ? ", "Take a Desision :", MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                DialogResult.No)
                return;

            newLicense = (rbDamaged.Checked ? oldLicense.ReplaceLicenseForDamaged(GlobalClass.CurrentUser.UserID) : 
                oldLicense.ReplaceLicenseForLost(GlobalClass.CurrentUser.UserID));
                
            if (newLicense != null)
            {
                lblRenewedLicenseID.Text = newLicense.LicenseID.ToString();
                lblReplacementLicenseApplicationID.Text = newLicense.ApplicationID.ToString();


                MessageBox.Show("License Issued Successfully!",
                    "Note :", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnReplace.Enabled = false;
                showDriverLicenseWithFilterControl1.FilterEnabled = false;
                llShowNewLicenseInfo.Enabled = true;
                groupBoxReplacementFor.Enabled = false;
            }
            else
            {
                MessageBox.Show("Error Happened While Issueing the License .. Please Contact Adminstrator",
                    "Error :", MessageBoxButtons.OK, MessageBoxIcon.Hand);

                return;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void llShowNewLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ShowDriverLicenseInfoForm form = new ShowDriverLicenseInfoForm();
            form.loadDriverLicenseInfoByLicenseID(newLicense.LicenseID);
            form.ShowDialog();
        }

        private void llShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ShowLicenseHistoryForm form = new ShowLicenseHistoryForm(showDriverLicenseWithFilterControl1.licenseInfo.applicationInfo.ApplicantPersonID);

            form.ShowDialog();
        }
    }
}
