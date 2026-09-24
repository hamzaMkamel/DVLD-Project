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

namespace DVLD_Project.Tests.Test_Appointments
{
    public partial class ManageTestAppointmentsform : Form
    {

        int  LocalDrivingLicenseApplicationID ,TestTypeID;
        DataTable tableOfAppointments;
        private void ManageTestAppointmentsform_Load(object sender, EventArgs e)
        {
            showDrivingLicenseApplicationInfoControl1.LoadLocalApplicationInfo(LocalDrivingLicenseApplicationID);
            setTextAndImageToTestAppointmentform(TestTypeID);

            fillAppointmentsTable();
        }
        private void setTextAndImageToTestAppointmentform(int TestTypeID)
        {
            switch (TestTypeID)
            {
                case 1:
                    {
                        pictureBoxICon.Image = Resources.Vision_512;
                        this.Text = "Vision Test Appointments";
                        break;
                    }
                case 2:
                    {
                        pictureBoxICon.Image = Resources.Written_Test_512;
                        this.Text = "Written Test Appointments";
                        break;
                    }
                case 3:
                    {
                        pictureBoxICon.Image = Resources.driving_test_512;
                        this.Text = "Street Test Appointments";
                        break;
                    }
            }
        }
        public ManageTestAppointmentsform(int LocalDrivingLicenseApplicationID , int TestTypeID)
        {
            InitializeComponent();

            this.LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            this.TestTypeID = TestTypeID;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            /*we should check for two conditions 
            *  1 -is he passed the test ?
            *  2  is there any open appointment ?


             */

            if(clsTests.isApplicantPassedThisTestBefore(LocalDrivingLicenseApplicationID , TestTypeID))
            {
                MessageBox.Show("Applicant already Passed this Test, you can't add any new appointments!", "Note :", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            if(clsTestAppointments.isThereNotLockedAppointment(LocalDrivingLicenseApplicationID , TestTypeID))
            {
                MessageBox.Show("The applicant already has an open appointment, you can't add any new appointments in this situation!", "Note :", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }

            ScheduleTestForm form = new ScheduleTestForm(LocalDrivingLicenseApplicationID, TestTypeID, dgvAppointments.RowCount);
            form.ShowDialog();
            fillAppointmentsTable();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvAppointments.CurrentRow == null || dgvAppointments.CurrentRow.Index < 0) return;
            ScheduleTestForm form = new ScheduleTestForm((int)dgvAppointments.CurrentRow.Cells[0].Value);
            form.ShowDialog();
            fillAppointmentsTable();
        }

        private void takeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvAppointments.CurrentRow == null || dgvAppointments.CurrentRow.Index < 0) return;
            TakeTestForm form = new TakeTestForm((int)dgvAppointments.CurrentRow.Cells[0].Value);
            form.ShowDialog();
            fillAppointmentsTable();
        }

        private void fillAppointmentsTable()
        {
            tableOfAppointments = clsTestAppointments.getAssociatedTestAppointmentset(LocalDrivingLicenseApplicationID, TestTypeID);
            lblRecords.Text = dgvAppointments.RowCount.ToString();
            dgvAppointments.DataSource = tableOfAppointments.DefaultView;

            if (dgvAppointments.Rows.Count > 0)
            {





                dgvAppointments.Columns[0].HeaderText = "Appointment ID";


                dgvAppointments.Columns[1].HeaderText = "Appointment Date";

                dgvAppointments.Columns[2].HeaderText = "Paid Fees";


                dgvAppointments.Columns[3].HeaderText = "Is Locked";

                



            }
        }
        
    }
}
