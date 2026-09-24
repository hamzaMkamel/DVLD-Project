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

namespace DVLD_Project.Applications.Local_Driving_Applications
{
    public partial class AddNewUpdateLocalDrivingLicenseApplication : Form
    {
        clsLocalDrivingLicenseApplication localApplication;
        int PersonID = -1;
        clsLocalDrivingLicenseApplication.enMode mode;
        int LocalApplicationID;

        public AddNewUpdateLocalDrivingLicenseApplication(int LocalApplicationID)
        {
            InitializeComponent();
            this.LocalApplicationID = LocalApplicationID;

            
            
        }

        private bool isApplicationEditable(int LocalApplicationID)
        {
            localApplication = clsLocalDrivingLicenseApplication.Find(LocalApplicationID);
            if (clsApplications.isApplicationStatusCancelOrFinished(localApplication.applicationInfo.ApplicationID))
            {
                this.Close();
                MessageBox.Show("Only New Application Status Can Be Updated", "Note", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            return true;
        }

        public void fillCompoBoxLicenseClasses()
        {
            DataTable LicenseClasses = clsLicenseClasses.getAllLicenseClasses();
            foreach (DataRow row in LicenseClasses.Rows)
            {
                comboBoxLicsenseClasses.Items.Add(row["ClassName"]);
            }

            comboBoxLicsenseClasses.SelectedIndex = 2;
        }

        private void fillformWithApplicationInfo(int LocalApplicationID)

        {
            
            //localApplication = clsLocalDrivingLicenseApplication.Find(LocalApplicationID); no need for it 
            showPersonInfoWithFilterControl1.LoadPersonInfo(localApplication.applicationInfo.ApplicantPersonID);
            if (localApplication == null)
            {
                MessageBox.Show("Application Not Found..", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            lblApplicationID.Text = localApplication.ApplicationID.ToString();
            lblDate.Text = localApplication.applicationInfo.ApplicationDate.ToShortDateString();
            lblApplicationFees.Text = localApplication.applicationInfo.PaidFees.ToString();
            lblCreatedBy.Text = clsUsers.getUserName( localApplication.applicationInfo.CreatedByUserID);
            mode = clsLocalDrivingLicenseApplication.enMode.Update;
            labelMode.Text = "Update Local Driving License Application : ";
            showPersonInfoWithFilterControl1.FilterEnabled = false;

        }

        private void fillformWithApplicationInfo()
        {
            mode = clsLocalDrivingLicenseApplication.enMode.AddNew;
            lblDate.Text = DateTime.Now.ToShortDateString();
            showPersonInfoWithFilterControl1.FilterEnabled = true;
            localApplication = new clsLocalDrivingLicenseApplication();
            lblApplicationFees.Text = clsApplicationTypes.getApplicationTypeByID(1).ApplicationTypeFees.ToString();
            lblCreatedBy.Text = GlobalClass.CurrentUser.UserName;

        }

        private void showPersonInfoWithFilterControl1_OnPersonSelected(int obj)
        {
            PersonID = obj;
            btnNext.Enabled = true;
            btnSave.Enabled = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(clsLocalDrivingLicenseApplication.isThisPeronAppliedForThisClass(PersonID , comboBoxLicsenseClasses.SelectedItem.ToString()))
            {
                MessageBox.Show("This Person is already applied for this class..", "Notice :", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if(mode == clsLocalDrivingLicenseApplication.enMode.AddNew)
            {
                // AS ANY NORMAL APPLICAION;
                localApplication.applicationInfo.ApplicantPersonID = PersonID;
                localApplication.applicationInfo.ApplicationDate = DateTime.Now;
                localApplication.applicationInfo.ApplicationStatus = 1;
                localApplication.applicationInfo.ApplicationTypeID = 1;// represents add new local driving license
                localApplication.applicationInfo.CreatedByUserID = GlobalClass.CurrentUser.UserID;
                localApplication.applicationInfo.PaidFees = decimal.Parse(lblApplicationFees.Text);
                //Here what makes local application is local which is class name :
                localApplication.LicenseClassID = clsLicenseClasses.getLicenseClassIDByName(comboBoxLicsenseClasses.SelectedItem.ToString());
                showPersonInfoWithFilterControl1.FilterEnabled = false;
                
                if (localApplication.Save())
                {
                    MessageBox.Show("The Application Added Successfully..", "Note :", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    mode = clsLocalDrivingLicenseApplication.enMode.Update;
                    lblApplicationID.Text = localApplication.LocalDrivingLicenseApplicationID.ToString();
                    labelMode.Text = "Update Local Driving License Application :";
                }
                else
                {
                    MessageBox.Show("The Process Faild (please contact the adminstrator)..", "Notice :", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else
            {
                localApplication.LicenseClassID = clsLicenseClasses.getLicenseClassIDByName(comboBoxLicsenseClasses.SelectedItem.ToString());
                if (localApplication.Save())
                {
                    MessageBox.Show("The Application Updated Successfully..", "Note :", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    mode = clsLocalDrivingLicenseApplication.enMode.Update;
                }
                else
                {
                    MessageBox.Show("The Process Faild (please contact the adminstrator)..", "Notice :", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (PersonID == -1)
            {
                e.Cancel = true;
                MessageBox.Show("Please Select A Person First.", "Note :", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else e.Cancel = false;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AddNewUpdateLocalDrivingLicenseApplication_Load(object sender, EventArgs e)
        {
            
            
                fillCompoBoxLicenseClasses();
                if (LocalApplicationID != -1)
                {
                    if (isApplicationEditable(LocalApplicationID))
                        fillformWithApplicationInfo(LocalApplicationID);


                }
                else
                {

                    fillformWithApplicationInfo();

                }

            
        }
    }
}
