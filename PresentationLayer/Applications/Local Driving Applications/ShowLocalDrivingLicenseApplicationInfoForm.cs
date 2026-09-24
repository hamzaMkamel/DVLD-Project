using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project.Applications.Local_Driving_Applications
{
    public partial class ShowLocalDrivingLicenseApplicationInfoForm : Form
    {
        int LocalDrivingLicenseApplicationID;
        public ShowLocalDrivingLicenseApplicationInfoForm(int LocalDrivingLicenseApplicationID)
        {
            InitializeComponent();
            this.LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            LoadFormInfo();
        }

        private void LoadFormInfo()
        {
            showDrivingLicenseApplicationInfoControl1.LoadLocalApplicationInfo(LocalDrivingLicenseApplicationID);

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
