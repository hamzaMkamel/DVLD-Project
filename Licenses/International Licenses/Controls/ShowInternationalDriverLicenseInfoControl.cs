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

namespace DVLD_Project.Licenses.International_Licenses.Controls
{
    public partial class ShowInternationalDriverLicenseInfoControl : UserControl
    {
        public clsInternationalLicenses International_Licenses;
        public ShowInternationalDriverLicenseInfoControl()
        {
            InitializeComponent();
        }

        public void LoadDriverLicenseInfo(int internationalLicenseID)
        {
            International_Licenses = clsInternationalLicenses.Find(internationalLicenseID);

            if (International_Licenses == null)
            {
                resetControlInfo();
                MessageBox.Show("License With ID :" + internationalLicenseID + " is not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }
            fillLicenseControlInfo();
        }

        private void fillLicenseControlInfo()
        {
            
            
            
            lblName.Text = clsPeople.getPersonNameByID(International_Licenses.applicationInfo.ApplicantPersonID);
            lblLicenseID.Text = International_Licenses.IssuedUsingLocalLicenseID.ToString();
            lblNationalNo.Text = International_Licenses.applicationInfo.personInfo.NationalNo;
            lblGendor.Text = (International_Licenses.applicationInfo.personInfo.Gendor == 0) ? "Male" : "Female";
            lblIssueDate.Text = International_Licenses.IssueDate.ToShortDateString();
            
            
            lblIsActive.Text = (International_Licenses.IsActive) ? "Yes" : "No";
            lblDateOfBirth.Text = International_Licenses.applicationInfo.personInfo.DateOfBirth.ToShortDateString();
            lblDriverID.Text = International_Licenses.DriverID.ToString();
            lblExpirationDate.Text = International_Licenses.ExpirationDate.ToShortDateString();
            lblInternationalLicenseID.Text = International_Licenses.InternationalLicenseID.ToString();
            lblApplicationID.Text = International_Licenses.ApplicationID.ToString();
            
            if (string.IsNullOrEmpty(International_Licenses.applicationInfo.personInfo.ImagePath))
                setDefaultImage(International_Licenses.applicationInfo.personInfo.Gendor);
            else
            {
                if (File.Exists(International_Licenses.applicationInfo.personInfo.ImagePath))
                    pictureBoxPhoto.ImageLocation = International_Licenses.applicationInfo.personInfo.ImagePath;
                else
                    setDefaultImage(International_Licenses.applicationInfo.personInfo.Gendor);
            }
        }

        private void setDefaultImage(int Gendor)
        {
            if (Gendor == 1)
                pictureBoxPhoto.Image = Resources.Female_512;
            else
                pictureBoxPhoto.Image = Resources.Male_512;
        }

        private void resetControlInfo()
        {

            lblInternationalLicenseID.Text = "N/A";
            lblName.Text = "N/A";
            lblLicenseID.Text = "N/A";
            lblNationalNo.Text = "N/A";
            lblGendor.Text = "N/A";
            lblIssueDate.Text = "N/A";
            lblApplicationID.Text = "N/A";
            
            lblIsActive.Text = "N/A";
            lblDateOfBirth.Text = "N/A";
            lblDriverID.Text = "N/A";
            lblExpirationDate.Text = "N/A";
            
            pictureBoxPhoto.Image = Resources.Male_512;
        }
    }
}
