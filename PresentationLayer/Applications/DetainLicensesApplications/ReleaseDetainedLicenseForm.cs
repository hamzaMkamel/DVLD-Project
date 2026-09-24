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

namespace DVLD_Project.Applications.DetainLicensesApplications
{
    
    public partial class ReleaseDetainedLicenseForm : Form
    {
        clsLicenses licenseInfo;
        clsDetainLicenses detainLicenseInfo;
        public ReleaseDetainedLicenseForm()
        {
            InitializeComponent();
            fillGroupboxWithInfo();
        }

        private void fillGroupboxWithInfo()
        {
            
            lblCreatedByUser.Text = GlobalClass.CurrentUser.UserName;
            lblApplicationFees.Text = clsApplicationTypes.getApplicationTypeByID(5).ApplicationTypeFees.ToString();
            llShowLicenseHistory.Enabled = false;
            btnRelease.Enabled = false;
            llShowLicenseInfo.Enabled = false;



        }

        public void LoadLicenseInfo(int LicenseID)
        {
            showDriverLicenseWithFilterControl1.LoadLicenseInfo(LicenseID);
            showDriverLicenseWithFilterControl1.FilterEnabled = false;
            showDriverLicenseWithFilterControl1_OnLicenseSelected(LicenseID);
        }

        private void showDriverLicenseWithFilterControl1_OnLicenseSelected(int LicenseID)
        {
            licenseInfo = clsLicenses.Find(LicenseID);

            if (licenseInfo == null)
            {
                return;
            }
            llShowLicenseHistory.Enabled = true;
            llShowLicenseInfo.Enabled = true;

            if (!clsDetainLicenses.isLicenseDetained(LicenseID))
            {
                MessageBox.Show("This License is Not Detained , Pick another one.",
                    "Error :", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                btnRelease.Enabled = false;
                return;
            }
            lblLicenseID.Text = licenseInfo.LicenseID.ToString();
            detainLicenseInfo = clsDetainLicenses.FindByLicenseID(LicenseID);
            if(detainLicenseInfo != null)
            {
                lblDetainID.Text = detainLicenseInfo.DetainID.ToString();
                lblDetainDate.Text = detainLicenseInfo.DetainDate.ToShortDateString();
                lblFineFees.Text = detainLicenseInfo.FineFees.ToString();
                lblTotalFees.Text = (Convert.ToDecimal(lblApplicationFees.Text) + Convert.ToDecimal(lblFineFees.Text)).ToString();
            }
            btnRelease.Enabled = true;
        }

        private void btnRelease_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are You Sure to Release this license ? ", "Take a Desision :", MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                DialogResult.No)
                return;


            if (licenseInfo.Release(GlobalClass.CurrentUser.UserID))
            {

                

                MessageBox.Show("License Released Successfully!",
                    "Note :", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnRelease.Enabled = false;
                showDriverLicenseWithFilterControl1.FilterEnabled = false;
                lblReleaseApplicationID.Text = licenseInfo.detainedLicenseInfo.ReleaseApplicationID.ToString();

            }
            else
            {
                MessageBox.Show("Error Happened While Releasing the License .. Please Contact Adminstrator",
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
            ShowDriverLicenseInfoForm form = new ShowDriverLicenseInfoForm();
            form.loadDriverLicenseInfoByLicenseID(licenseInfo.LicenseID);
            form.ShowDialog();
        }

        private void llShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ShowLicenseHistoryForm form = new ShowLicenseHistoryForm(showDriverLicenseWithFilterControl1.licenseInfo.applicationInfo.ApplicantPersonID);

            form.ShowDialog();
        }
    }
}
