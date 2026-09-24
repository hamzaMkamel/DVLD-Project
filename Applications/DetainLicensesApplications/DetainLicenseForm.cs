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
    public partial class DetainLicenseForm : Form
    {
        clsLicenses licenseInfo;
        public DetainLicenseForm()
        {
            InitializeComponent();
            fillGroupboxWithInfo();
        }

        private void fillGroupboxWithInfo()
        {
            lblDetainDate.Text = DateTime.Now.ToShortDateString();
            lblCreatedByUser.Text = GlobalClass.CurrentUser.UserName;
            llShowLicenseHistory.Enabled = false;
            btnDetain.Enabled = false;
            llShowLicenseInfo.Enabled = false;
            textBoxFineFees.Focus();



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
            
            if (clsDetainLicenses.isLicenseDetained(LicenseID))
            {
                MessageBox.Show("This License is Already Detained , Pick another one.",
                    "Error :", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                btnDetain.Enabled = false;
                return;
            }
            lblLicenseID.Text = licenseInfo.LicenseID.ToString();
            btnDetain.Enabled = true;
            textBoxFineFees.Focus();
        }

        private void textBoxFineFees_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void btnDetain_Click(object sender, EventArgs e)
        {

            if(!string.IsNullOrEmpty(errorProvider1.GetError(textBoxFineFees)))
            {
                MessageBox.Show("Please Check The Fees Field And try Again!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show("Are You Sure to Detain this license ? ", "Take a Desision :", MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                DialogResult.No)
                return;


            if (licenseInfo.Detain(Convert.ToDecimal(textBoxFineFees.Text), GlobalClass.CurrentUser.UserID))
            {

                lblDetainID.Text = licenseInfo.detainedLicenseInfo.DetainID.ToString();

                MessageBox.Show("License Detained Successfully!",
                    "Note :", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnDetain.Enabled = false;
                showDriverLicenseWithFilterControl1.FilterEnabled = false;
                
            }
            else
            {
                MessageBox.Show("Error Happened While Detaining the License .. Please Contact Adminstrator",
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

        private void textBoxFineFees_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxFineFees.Text))
                errorProvider1.SetError(textBoxFineFees, "This Field Can't Be Empty!");
            else
                errorProvider1.SetError(textBoxFineFees, null);
        }
    }
}
