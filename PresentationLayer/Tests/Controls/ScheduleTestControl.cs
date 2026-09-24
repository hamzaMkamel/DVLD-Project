using BusinessLayer;
using DVLD_Project.Global_Classes;
using DVLD_Project.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project.Tests.Controls
{
    public partial class ScheduleTestControl : UserControl
    {

        int LocalDrivingLicenseApplicationID, TestTypeID, Trial;
        
        clsLocalDrivingLicenseApplication Localapplication;
        clsApplications retakeTestApplication = null;
        clsTestAppointments appointment;
        enum enMode { addNew , Update , View}
        enMode mode;

        //Declare  a Delegate :
        public delegate void OnSavedClicked();
        //Initialize an item // event is a protection layer
        public event OnSavedClicked dataBack;



        public ScheduleTestControl()
        {
            InitializeComponent();
        }

        private void setTextAndImageToTest(int TestTypeID)
        {
            switch(TestTypeID)
            {
                case 1:
                    {
                        pictureBoxICon.Image = Resources.Vision_512;
                        groupBoxTest.Text = "Vision Test";
                        break;
                    }
                case 2:
                    {
                        pictureBoxICon.Image = Resources.Written_Test_512;
                        groupBoxTest.Text = "Written Test";
                        break;
                    }
                case 3:
                    {
                        pictureBoxICon.Image = Resources.driving_test_512;
                        groupBoxTest.Text = "Street Test";
                        break;
                    }
            }
        }

        private bool handleTakenTestAppointmentCase(int TestAppointmentID)
        {
            if(clsTestAppointments.isTestAppointmentTaken(TestAppointmentID))
            {
                labelHeader.Text = "View Scheduled Test :";
                dateTimePicker1.Enabled = false;
                btnSave.Enabled = false;
                mode = enMode.View;
                return true;
            }
            return false;
        }

        public void LoadControlInfo(int LocalDrivingLicenseApplicationID, int TestTypeID, int Trial)
        {
            setTextAndImageToTest(TestTypeID);

            this.LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            this.TestTypeID = TestTypeID;
            this.Trial = Trial;
            mode = enMode.addNew;

            lblApplicationID.Text = LocalDrivingLicenseApplicationID.ToString();
            Localapplication = clsLocalDrivingLicenseApplication.Find(LocalDrivingLicenseApplicationID);
            lblApplicant.Text = clsPeople.getPersonNameByID(Localapplication.applicationInfo.ApplicantPersonID);
            lblFees.Text = clsTestTypes.getTestTypeByID(1).TestTypeFees.ToString();
            dateTimePicker1.MinDate = DateTime.Now;
            dateTimePicker1.Value = DateTime.Now;
            lblTrial.Text = Localapplication.TotalTrialsPerTest(TestTypeID).ToString();
            lblClassName.Text = clsLicenseClasses.getLicenseClassNameByID(Localapplication.LicenseClassID);

            if (handleRetakeTest())
            {
                lblRetakeFees.Text = retakeTestApplication.PaidFees.ToString();
                lblTotalFees.Text = (retakeTestApplication.PaidFees + Convert.ToDecimal(lblFees.Text)).ToString();
                groupBoxRetakeTest.Enabled = true;
            }
            else
                lblTotalFees.Text = lblFees.Text;

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(mode == enMode.addNew)
            {
                clsTestAppointments appointment = new clsTestAppointments();
                appointment.TestTypeID = this.TestTypeID;
                appointment.LocalDrivingLicenseApplicationID = this.LocalDrivingLicenseApplicationID;
                appointment.AppointmentDate = dateTimePicker1.Value;
                appointment.CreatedByUserID = GlobalClass.CurrentUser.UserID;
                appointment.PaidFees = Convert.ToDecimal(lblFees.Text);
                appointment.RetakeTestApplicationInfo = retakeTestApplication;
                appointment.RetakeTestApplicationID = (retakeTestApplication != null) ? retakeTestApplication.ApplicationID : -1;
                if(appointment.Save())
                {
                    dataBack?.Invoke();
                    MessageBox.Show("Appointment Saved Successfully", "Note", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    MessageBox.Show("Appointment doesn't saved!..Error Happened !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                appointment.AppointmentDate = dateTimePicker1.Value;
                if(appointment.Save())
                {
                    dataBack?.Invoke();
                    MessageBox.Show(" Appointment Saved Successfully", "Note", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    MessageBox.Show("Appointment doesn't saved!..Error Happened !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

            }
        }

        public void LoadControlInfo(int TestAppointmentID)
        {
            if(!handleTakenTestAppointmentCase(TestAppointmentID))
            {
                mode = enMode.Update;
            }

                appointment = clsTestAppointments.Find(TestAppointmentID);

            if(appointment != null)
            {
                setTextAndImageToTest(appointment.TestTypeID);
                lblApplicationID.Text = appointment.LocalDrivingLicenseApplicationID.ToString();
                Localapplication = clsLocalDrivingLicenseApplication.Find(appointment.LocalDrivingLicenseApplicationID);
                lblApplicant.Text = clsPeople.getPersonNameByID(Localapplication.applicationInfo.ApplicantPersonID);
                lblFees.Text = appointment.PaidFees.ToString();
                lblTrial.Text = Localapplication.TotalTrialsPerTest(TestTypeID).ToString();

                if (appointment.AppointmentDate < DateTime.Now)
                    dateTimePicker1.MinDate = appointment.AppointmentDate;
                else
                    dateTimePicker1.MinDate = DateTime.Now;
                dateTimePicker1.Value = appointment.AppointmentDate;
                
                lblClassName.Text = clsLicenseClasses.getLicenseClassNameByID(Localapplication.LicenseClassID);

                if(appointment.RetakeTestApplicationInfo != null)
                {
                    lblRetakeFees.Text = appointment.RetakeTestApplicationInfo.PaidFees.ToString();
                    lblTotalFees.Text = (appointment.RetakeTestApplicationInfo.PaidFees + Convert.ToDecimal(lblFees.Text)).ToString();
                    lblRetakeID.Text = appointment.RetakeTestApplicationInfo.ApplicationID.ToString();
                    groupBoxRetakeTest.Enabled = true;
                }
                else
                    lblTotalFees.Text = lblFees.Text;
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            
        }

        private bool handleRetakeTest()
        {
            if (clsTestAppointments.isPersonTakeTestBefore(this.LocalDrivingLicenseApplicationID, this.TestTypeID))
            {
                retakeTestApplication = new clsApplications();
                int personID = -1;

                
                personID = Localapplication.applicationInfo.ApplicantPersonID;

                retakeTestApplication.ApplicantPersonID = personID;
                retakeTestApplication.ApplicationDate = DateTime.Now;
                retakeTestApplication.ApplicationStatus = 1;
                retakeTestApplication.ApplicationTypeID = 7;
                retakeTestApplication.CreatedByUserID = GlobalClass.CurrentUser.UserID ;
                retakeTestApplication.PaidFees = clsApplicationTypes.getApplicationTypeByID(7).ApplicationTypeFees;

                return true;
                 

            }

            return false;
        }
        
        public void DeleteApplicationIfExists() // the application is auto generated when the control is created so it should be deleted if the user
                                                //doesn't continue the proccess
        {
            if(retakeTestApplication != null)
            {
                clsApplications.DeleteApplication(retakeTestApplication.ApplicationID);
                // no need to handle the appointment attributes cuz it is not saved in the database yet
            }
        }
       
    }
}
