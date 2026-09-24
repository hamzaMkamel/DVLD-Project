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

namespace DVLD_Project.Licenses.LocalLicenses
{
    public partial class ShowDriverLicenseInfoForm : Form
    {
        int applicationID;
        public ShowDriverLicenseInfoForm()
        {
            InitializeComponent();
            
            
        }

        public void loadDriverLicenseInfoByApplciationID(int applciationID)
        {

            driverLicenseInfoControl1.LoadDriverLicenseInfo(clsLicenses.getLicenseIDByApplicationID(applciationID));
        }

        public void loadDriverLicenseInfoByLicenseID(int LicenseID)
        {

            driverLicenseInfoControl1.LoadDriverLicenseInfo(LicenseID);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
