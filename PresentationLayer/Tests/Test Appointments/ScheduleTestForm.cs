using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project.Tests.Test_Appointments
{
    public partial class ScheduleTestForm : Form
    {
        enum enMode { AddNew , Update}
        enMode mode;
        public ScheduleTestForm(int TestAppointmentID)
        {
            InitializeComponent();
            mode = enMode.Update;
            scheduleTestControl1.LoadControlInfo(TestAppointmentID);
        }

        public ScheduleTestForm(int LocalDrivingLicenseApplicationID, int TestTypeID, int Trial)
        {
            InitializeComponent();
            mode = enMode.AddNew;
            scheduleTestControl1.LoadControlInfo(LocalDrivingLicenseApplicationID  , TestTypeID , Trial);
            scheduleTestControl1.dataBack += setUpdateMode;
        }

        private void setUpdateMode()
        {
            mode = enMode.Update;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            
            this.Close();
        }
    }
}
