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
using static System.Net.Mime.MediaTypeNames;

namespace DVLD_Project.Tests.Controls
{
    public partial class ShowDrivingLicenseApplicationInfoControl : UserControl
    {

        public clsLocalDrivingLicenseApplication localApplication;
        public ShowDrivingLicenseApplicationInfoControl()
        {
            InitializeComponent();
        }

        public bool ShowLicenseInfoEnabled
        {
            set
            {
                linkLabel1.Enabled = value;
            }
            get
            {
                return linkLabel1.Enabled;
            }
        }

        public void LoadLocalApplicationInfo(int LocalApplicationID)
        {
            localApplication = clsLocalDrivingLicenseApplication.Find(LocalApplicationID);
            if(localApplication == null)
            {
                MessageBox.Show("Application With ID :" + LocalApplicationID + " is not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                resetControl();
                return;
            }

            lblID.Text = localApplication.LocalDrivingLicenseApplicationID.ToString();
            lblLicenseClass.Text = clsLicenseClasses.getLicenseClassNameByID(localApplication.LicenseClassID);
            lblPassedTests.Text = $"{clsTests.getPassedTestsByLocalDrivingLicenseApplicationID(localApplication.LocalDrivingLicenseApplicationID)} / 3";
            applicationBasicInfoControl1.LoadApplicationInfo(localApplication.ApplicationID);
        }

        public void resetControl()
        {

            lblID.Text = "N/A";
            lblLicenseClass.Text = "N/A";
            lblPassedTests.Text = "N/A";
            linkLabel1.Enabled = false;

        }
        //you should implement showing license info ;; later on
    }
}
