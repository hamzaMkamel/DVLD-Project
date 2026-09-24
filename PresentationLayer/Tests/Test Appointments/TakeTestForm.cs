using BusinessLayer;
using DVLD_Project.Global_Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project.Tests.Test_Appointments
{
    public partial class TakeTestForm : Form
    {
        int AppointmentID;
        clsTests TestInfo;
        public TakeTestForm(int AppointmentID)
        {
            InitializeComponent();
            this.AppointmentID = AppointmentID;
        }



        private void TakeTestForm_Load(object sender, EventArgs e)
        {
            // if test already exists it show only a not editable form showing data
            TestInfo = clsTests.FindByAppointmentID(AppointmentID);
            scheduledTestControl1.LoadControlInfo(AppointmentID);
            if(TestInfo!= null)
            {
                if(TestInfo.TestResult) { rBPass.Checked = true; } else { rBFail.Checked = true; }
                textBoxNotes.Text = TestInfo.Notes;
                rBPass.Enabled = false;
                rBFail.Enabled = false;
                textBoxNotes.Enabled = false;
                btnSave.Enabled = false;
            }

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            TestInfo = new clsTests();
            TestInfo.TestAppointmentID = AppointmentID;
            TestInfo.TestResult = (rBPass.Checked) ? true : false;
            TestInfo.Notes = textBoxNotes.Text;
            TestInfo.CreatedByUserID = GlobalClass.CurrentUser.UserID;

            if(TestInfo.Save())
            {
                MessageBox.Show("Test Saved Successfully!", "Note :", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Close();
                
            }
            else
            {
                MessageBox.Show("Test is not saved..Error Happened!", "Note :", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
