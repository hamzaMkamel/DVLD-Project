using BusinessLayer;
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
    public partial class ScheduledTestControl : UserControl
    {
        clsTestAppointments appointment;
        clsLocalDrivingLicenseApplication LocalApplication;
        public ScheduledTestControl()
        {
            InitializeComponent();
        }

        private void setTextAndImageToTest(int TestTypeID)
        {
            switch (TestTypeID)
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

        public void LoadControlInfo(int TestAppointmentID)
        {
            

            appointment = clsTestAppointments.Find(TestAppointmentID);

            if (appointment != null)
            {
                setTextAndImageToTest(appointment.TestTypeID);
                lblApplicationID.Text = appointment.LocalDrivingLicenseApplicationID.ToString();
                LocalApplication = clsLocalDrivingLicenseApplication.Find(appointment.LocalDrivingLicenseApplicationID);
                lblApplicant.Text = clsPeople.getPersonNameByID(LocalApplication.applicationInfo.ApplicantPersonID);
                lblFees.Text = appointment.PaidFees.ToString();
                
                lblDate.Text = appointment.AppointmentDate.ToShortDateString();

                lblClassName.Text = clsLicenseClasses.getLicenseClassNameByID(LocalApplication.LicenseClassID);

                
            }
        }
    }
}
