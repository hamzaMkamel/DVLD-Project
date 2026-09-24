using BusinessLayer;
using DVLD_Project.Global_Classes;
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
    public partial class IssueDriverLicenseForm : Form
    {
        clsLocalDrivingLicenseApplication localApplication;
        public IssueDriverLicenseForm(int LocalApplication)
        {
            InitializeComponent();
            LoadLocalApplicaitonInfo(LocalApplication);

        }
        clsLicenseClasses licenseClass;

        private void LoadLocalApplicaitonInfo(int LocalApplication)
        {
            if (localApplication == null)
            {
                MessageBox.Show($"Application With ID {LocalApplication} Not Found ", "Note :", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
                return;
            }

            showDrivingLicenseApplicationInfoControl1.LoadLocalApplicationInfo(LocalApplication);
            localApplication = showDrivingLicenseApplicationInfoControl1.localApplication;
             licenseClass = clsLicenseClasses.Find(localApplication.LicenseClassID);
            lblClassFees.Text = licenseClass.ClassFees.ToString();

        }
        //this method should be more handled this code should be extracted to clsLicense to method issuelicensefirstTime
        private void btnSave_Click(object sender, EventArgs e)
        {
            

            int _LicenseID = localApplication.IssueLicenseForFirstTime(textBoxNotes.Text, GlobalClass.CurrentUser.UserID);

            if(_LicenseID != -1)
            {
                MessageBox.Show($"License Issued Successfully With ID {_LicenseID} ", "Note :", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
                MessageBox.Show($"License Not Issued! Error Happened ", "Note :", MessageBoxButtons.OK, MessageBoxIcon.Error);



        }

        
    }
}
