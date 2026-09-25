using DVLD_Project.Applications.Application_Types;
using DVLD_Project.Applications.DetainLicensesApplications;
using DVLD_Project.Applications.International_Applications;
using DVLD_Project.Applications.Local_Driving_Applications;
using DVLD_Project.Applications.RenewApplication;
using DVLD_Project.Applications.RepleaceForDamagedOrLostApplication;
using DVLD_Project.Applications.TestTypes;
using DVLD_Project.Drivers;
using DVLD_Project.Global_Classes;
using DVLD_Project.Licenses.International_Licenses;
using DVLD_Project.Properties;
using DVLD_Project.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project
{
    public partial class MainForm : Form
    {

        private LoginForm _loginForm;
        public MainForm(LoginForm form)
        {
            InitializeComponent();
            _loginForm = form;

        }



        private void MainForm_Load(object sender, EventArgs e)
        {
            
            
        }

        private void tSButtonManagePeople_Click(object sender, EventArgs e)
        {
            ManagePeopleForm form = new ManagePeopleForm();
           
            form.ShowDialog();

        }

        private void toolStripButtonUsers_Click(object sender, EventArgs e)
        {
            ManageUsersform form = new ManageUsersform();
            
            form.ShowDialog();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            GlobalClass.CurrentUser = null;

            _loginForm.Show();
        }

        private void currentUserInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowUserInfoForm form = new ShowUserInfoForm(GlobalClass.CurrentUser.UserID);
            form.ShowDialog();
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangePasswordForm form = new ChangePasswordForm(GlobalClass.CurrentUser.UserID);
            form.ShowDialog();
        }

        private void signOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GlobalClass.CurrentUser = null;
            this.Close();
            _loginForm.Show();
        }

        private void manageApplicationTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ApplicationTypesForm form = new ApplicationTypesForm();
            
            form.ShowDialog();
        }

        private void toolStripSplitButton1_Click(object sender, EventArgs e)
        {

        }

        private void manageTestTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TestTypesForm form = new TestTypesForm();
            
            form.ShowDialog();
        }

        private void localDrivingLicenseApplicationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManageLocalDrivingApplicationsForm form = new ManageLocalDrivingApplicationsForm();
            
            form.ShowDialog();
        }

        private void localLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddNewUpdateLocalDrivingLicenseApplication form = new AddNewUpdateLocalDrivingLicenseApplication(-1);
            
            form.ShowDialog();
        }

        private void toolStripButtonDrivers_Click(object sender, EventArgs e)
        {
            ShowDriversForm form = new ShowDriversForm();
            
            form.ShowDialog();
        }

        private void retakeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManageLocalDrivingApplicationsForm form = new ManageLocalDrivingApplicationsForm();
            
            form.ShowDialog();
        }

        private void internationalLicesneApplicationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManageInternationalLicensesForm form = new ManageInternationalLicensesForm();
            
            form.ShowDialog();
        }

        private void internationalLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddInternationalLicenseFrom form = new AddInternationalLicenseFrom();
            
            form.ShowDialog();
        }

        private void renewDrivingLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RenewLicenseApplicationForm form = new RenewLicenseApplicationForm();
            form.ShowDialog();
        }

        private void replacementForLostOrDamagedLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReplacementFormLostOrDamagedLicenseApplicationForm form = new ReplacementFormLostOrDamagedLicenseApplicationForm();
            form.ShowDialog();
        }

        private void manageDetainedLicensesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManageDetainedLicensesForm form = new ManageDetainedLicensesForm();
            
            form.ShowDialog();
        }

        private void detainLicenseToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DetainLicenseForm form = new DetainLicenseForm();
            form.ShowDialog();
        }

        private void releaseDetainLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReleaseDetainedLicenseForm form = new ReleaseDetainedLicenseForm();
            form.ShowDialog();
        }

        private void releaseDetainedDrivingLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReleaseDetainedLicenseForm form = new ReleaseDetainedLicenseForm();
            form.ShowDialog();
        }
    }
}
