using BusinessLayer;
using DVLD_Project.Licenses;
using DVLD_Project.Licenses.LocalLicenses;
using DVLD_Project.Tests.Test_Appointments;
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

namespace DVLD_Project.Applications.Local_Driving_Applications
{
    public partial class ManageLocalDrivingApplicationsForm : Form
    {
        private DataTable tableOfLocalApplications;
        private DataView tableView = new DataView();
        public ManageLocalDrivingApplicationsForm()
        {
            InitializeComponent();
            fillLocalApplicationsTable();
        }

       

        private void fillLocalApplicationsTable()
        {

            tableOfLocalApplications = clsLocalDrivingLicenseApplication.getAllLocalDrivingLicenseApplications();
            tableView = tableOfLocalApplications.DefaultView;
            dgvLocalApplications.DataSource = tableView;
            lblRecordsNum.Text = dgvLocalApplications.RowCount.ToString();
            comboBoxFilterBy.SelectedIndex =0 ;

            if (dgvLocalApplications.Rows.Count > 0)
            {





                dgvLocalApplications.Columns[0].HeaderText = "L.D.L.App ID";


                dgvLocalApplications.Columns[1].HeaderText = "Driving Class";

                dgvLocalApplications.Columns[2].HeaderText = "National No.";


                dgvLocalApplications.Columns[3].HeaderText = "Full Name";

                dgvLocalApplications.Columns[4].HeaderText = "Application Date";


                dgvLocalApplications.Columns[5].HeaderText = "Passed Tests";


                dgvLocalApplications.Columns[6].HeaderText = "Status";

                


            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void editApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            

            AddNewUpdateLocalDrivingLicenseApplication form = new AddNewUpdateLocalDrivingLicenseApplication((int)dgvLocalApplications.CurrentRow.Cells[0].Value);
            form.ShowDialog();
            fillLocalApplicationsTable();
        }

        private void btnAddNewLocalApplication_Click(object sender, EventArgs e)
        {
            AddNewUpdateLocalDrivingLicenseApplication form = new AddNewUpdateLocalDrivingLicenseApplication(-1);
            form.ShowDialog();
            fillLocalApplicationsTable();
        }

        private void comboBoxFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxFilterBy.SelectedItem.ToString().Equals("None"))
            {
                txtBFilterBy.Visible = false;
                txtBFilterBy.Text = "";

            }
            else
            {
                txtBFilterBy.Visible = true;
                txtBFilterBy.Focus();
            }
        }

        private void txtBFilterBy_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = comboBoxFilterBy.SelectedItem.ToString();
            
            if (string.IsNullOrWhiteSpace(txtBFilterBy.Text))
            {
                tableView.RowFilter = "";
                dgvLocalApplications.DataSource = tableView;
                lblRecordsNum.Text = dgvLocalApplications.RowCount.ToString();
                return;
            }



            DataView view = tableOfLocalApplications.DefaultView;



            if (FilterColumn == "LocalDrivingLicenseApplicationID")
                //in this case we deal with integer not string.

                view.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txtBFilterBy.Text.Trim());
            else
                view.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, txtBFilterBy.Text.Trim());

            dgvLocalApplications.DataSource = view;
            lblRecordsNum.Text = dgvLocalApplications.RowCount.ToString();

        }

        private void txtBFilterBy_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (comboBoxFilterBy.Text == "L.D.L.AppID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void cancelButtontoolStripMenuItem1_Click(object sender, EventArgs e)
        {
            
            if(clsLocalDrivingLicenseApplication.CancelLocalApplication((int)dgvLocalApplications.CurrentRow.Cells[0].Value))
            {
                MessageBox.Show("Application Canceled Successfully!", "Note :", MessageBoxButtons.OK, MessageBoxIcon.Information);
                fillLocalApplicationsTable();
            }
            else
                MessageBox.Show("Error Happend please Call the adminstrator!", "Error :", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void DeleteApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("This Application will be Deleted..! , Are you Sure?", "Take A Decision :", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation)
                == DialogResult.Yes)
            {
                if (clsLocalDrivingLicenseApplication.DeleteLocalDrivingLicenseApplication((int)dgvLocalApplications.CurrentRow.Cells[0].Value))
                {
                    MessageBox.Show("Application Deleted Successfully!", "Note :", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    fillLocalApplicationsTable();
                }
                else
                    MessageBox.Show("This Application Can't Be Deleted ,it has relations in the system!", "Error :", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void schduleVisionTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
           

            ManageTestAppointmentsform form = new ManageTestAppointmentsform((int)dgvLocalApplications.CurrentRow.Cells[0].Value, 1);
            form.ShowDialog();
            fillLocalApplicationsTable();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (dgvLocalApplications.CurrentRow == null || dgvLocalApplications.CurrentRow.Index < 0)
            {
                e.Cancel = true;
                return;
            }
            else e.Cancel = false;
                checkContextMenuStripItems();
        }

        private void setAllContextMenuItemsDisabled() // we need to set all the items disable till improve it will be enabled
        {
            editApplicationToolStripMenuItem.Enabled = false;
            cancelButtontoolStripMenuItem1.Enabled = false;
            DeleteApplicationToolStripMenuItem.Enabled = false;
            schduleTestsToolStripMenuItem.Enabled = false;
            issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = false;
            showLicenseToolStripMenuItem.Enabled = false;
            schduleVisionTestToolStripMenuItem.Enabled = false;
            schduleStreetTestToolStripMenuItem.Enabled = false;
            schduleWrittenTestToolStripMenuItem.Enabled = false;

        }

        private void checkContextMenuStripItems() // crucial method which insure the menu's Items when opened to be enabled
                                                    //or disabled base on the cases
        {
            setAllContextMenuItemsDisabled();
            int LocalApplicationID = (int)dgvLocalApplications.CurrentRow.Cells[0].Value;
            clsLocalDrivingLicenseApplication localApplicationInfo = clsLocalDrivingLicenseApplication.Find(LocalApplicationID);
            int ApplicationID = localApplicationInfo.ApplicationID;
            if (clsApplications.IsApplicationCanceled(ApplicationID)) return;

            if(clsApplications.IsApplicationCompleted(ApplicationID))
            {
                showLicenseToolStripMenuItem.Enabled = true;
                return;
            }
            if(clsTests.getPassedTestsByLocalDrivingLicenseApplicationID(LocalApplicationID) == 3)
            {
                issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = true;
                return;
            }
            else
            {
                schduleTestsToolStripMenuItem.Enabled = true;
                editApplicationToolStripMenuItem.Enabled = true;
                DeleteApplicationToolStripMenuItem.Enabled = true;
                cancelButtontoolStripMenuItem1.Enabled = true;
            }
        }

        private void schduleTestsToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            int LocalApplicationID = (int)dgvLocalApplications.CurrentRow.Cells[0].Value;
            int passedTests = clsTests.getPassedTestsByLocalDrivingLicenseApplicationID(LocalApplicationID);
            if(passedTests == 2)
            {
                schduleStreetTestToolStripMenuItem.Enabled = true;
                return;
            }
            if(passedTests == 1)
            {
                schduleWrittenTestToolStripMenuItem.Enabled = true;
                return;
            }
            schduleVisionTestToolStripMenuItem.Enabled = true;
        }

        private void schduleWrittenTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManageTestAppointmentsform form = new ManageTestAppointmentsform((int)dgvLocalApplications.CurrentRow.Cells[0].Value, 2);
            form.ShowDialog();
            fillLocalApplicationsTable();
        }

        private void schduleStreetTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManageTestAppointmentsform form = new ManageTestAppointmentsform((int)dgvLocalApplications.CurrentRow.Cells[0].Value, 3);
            form.ShowDialog();
            fillLocalApplicationsTable();
        }

        private void showLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clsLocalDrivingLicenseApplication localApplication = clsLocalDrivingLicenseApplication.Find((int)dgvLocalApplications.CurrentRow.Cells[0].Value);
            ShowDriverLicenseInfoForm form = new ShowDriverLicenseInfoForm();
            form.loadDriverLicenseInfoByApplciationID(localApplication.ApplicationID);
            form.ShowDialog();
        }

        private void issueDrivingLicenseFirstTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IssueDriverLicenseForm form = new IssueDriverLicenseForm((int)dgvLocalApplications.CurrentRow.Cells[0].Value);
            form.ShowDialog();
            

        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int PersonID = clsLocalDrivingLicenseApplication.Find((int)dgvLocalApplications.CurrentRow.Cells[0].Value).applicationInfo.ApplicantPersonID;
            if (clsDrivers.isPersonADriver(PersonID))
            {
                ShowLicenseHistoryForm form = new ShowLicenseHistoryForm(PersonID);
                form.ShowDialog();
            }
            else
            {
                MessageBox.Show("This Person doesn't has any licenses yet!", "Note", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void showApplicationDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowLocalDrivingLicenseApplicationInfoForm form = new ShowLocalDrivingLicenseApplicationInfoForm((int)dgvLocalApplications.CurrentRow.Cells[0].Value);
            form.ShowDialog();
        }
    }
}
