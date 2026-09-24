using BusinessLayer;
using DVLD_Project.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project.Licenses.LocalLicenses.Controls
{
    public partial class DriverLicenseInfoControl : UserControl
    {
        public clsLicenses license;
        //enum IssueReason { FirstTime = 1, Renew = 2, ReplacementForDamaged = 4, ReplacementForLost = 3 };
        public DriverLicenseInfoControl()
        {
            InitializeComponent();
        }

        public void LoadDriverLicenseInfo(int LicenseID)
        {
            license = clsLicenses.Find(LicenseID);

            if (license == null)
            {
                resetControlInfo();
                MessageBox.Show("License With ID :" + LicenseID + " is not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }
            fillLicenseControlInfo();
        }

        private void fillLicenseControlInfo()
        {
            clsDrivers driver = clsDrivers.Find(license.DriverID);
            clsPeople person = clsPeople.Find(driver.PersonID);
            lblClassName.Text = clsLicenseClasses.getLicenseClassNameByID(license.LicenseClass);
            lblName.Text = clsPeople.getPersonNameByID(person.PersonID);
            lblLicenseID.Text = license.LicenseID.ToString();
            lblNationalNo.Text = person.NationalNo;
            lblGendor.Text = (person.Gendor == 0) ? "Male" : "Female";
            lblIssueDate.Text = license.IssueDate.ToShortDateString();
            lblIssueReason.Text = setIssueReasonString(license.IssueReason);
            lblNotes.Text = (string.IsNullOrEmpty(license.Notes)) ? "No Notes" : license.Notes;
            lblIsActive.Text = (license.IsActive) ? "Yes" : "No";
            lblDateOfBirth.Text = person.DateOfBirth.ToShortDateString();
            lblDriverID.Text = license.DriverID.ToString();
            lblExpirationDate.Text = license.ExpirationDate.ToShortDateString();
            lblIsDetained.Text = clsDetainLicenses.isLicenseDetained(license.LicenseID) ? "Yes" : "No";
            if (string.IsNullOrEmpty(person.ImagePath))
                setDefaultImage(person.Gendor);
            else
            {
                if (File.Exists(person.ImagePath))
                    pictureBoxPhoto.ImageLocation = person.ImagePath;
                else
                    setDefaultImage(person.Gendor);
            }
        }

        private string setIssueReasonString(int IssueReason)
        {
            switch (IssueReason)
            {
                case 1:
                    return "First Time";
                case 2:
                    return "Renew";
                case 3:
                    return "Replacement For Lost";
                case 4:
                    return "Replacement For Damaged";
                
                default:
                    return "none..";
            }
        }

        private void resetControlInfo()
        {

            lblClassName.Text = "N/A";
            lblName.Text = "N/A";
            lblLicenseID.Text = "N/A";
            lblNationalNo.Text = "N/A";
            lblGendor.Text = "N/A";
            lblIssueDate.Text = "N/A";
            lblIssueReason.Text = "N/A";
            lblNotes.Text = "N/A";
            lblIsActive.Text = "N/A";
            lblDateOfBirth.Text = "N/A";
            lblDriverID.Text = "N/A";
            lblExpirationDate.Text = "N/A";
            lblIsDetained.Text = "N/A";
            pictureBoxPhoto.Image = Resources.Male_512;
        }
        private void setDefaultImage(int Gendor)
        {
            if (Gendor == 1)
                pictureBoxPhoto.Image = Resources.Female_512;
            else
                pictureBoxPhoto.Image = Resources.Male_512;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}