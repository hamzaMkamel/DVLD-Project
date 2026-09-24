using BusinessLayer;
using DVLD_Project.Licenses;
using DVLD_Project.Licenses.International_Licenses;
using DVLD_Project.Licenses.LocalLicenses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project.Applications.DetainLicensesApplications
{
    public partial class ManageDetainedLicensesForm : Form
    {
        private DataTable tableOfDetainedLicenses;
        private DataView tableView = new DataView();
        public ManageDetainedLicensesForm()
        {
            InitializeComponent();
            FillDetainedLicensesTable();
        }

        private void FillDetainedLicensesTable()
        {

            tableOfDetainedLicenses = clsDetainLicenses.getAllDetainedLicenses();
            tableView = tableOfDetainedLicenses.DefaultView;
            dgvDetainedLicenses.DataSource = tableView;
            lblRecordsNum.Text = dgvDetainedLicenses.RowCount.ToString();
            comboBoxFilterBy.SelectedIndex = 0;

            if (dgvDetainedLicenses.Rows.Count > 0)
            {





                dgvDetainedLicenses.Columns[0].HeaderText = "D.ID";


                dgvDetainedLicenses.Columns[1].HeaderText = "L.ID";
                dgvDetainedLicenses.Columns[2].HeaderText = "D.Date";
                dgvDetainedLicenses.Columns[3].HeaderText = "Fine Fees";
                dgvDetainedLicenses.Columns[4].HeaderText = "Is Released";
                dgvDetainedLicenses.Columns[5].HeaderText = "Release Date";
                dgvDetainedLicenses.Columns[6].HeaderText = "National No.";


                dgvDetainedLicenses.Columns[7].HeaderText = "Full Name";

                dgvDetainedLicenses.Columns[8].HeaderText = "Release App ID";


                




            }
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
                dgvDetainedLicenses.DataSource = tableView;
                lblRecordsNum.Text = dgvDetainedLicenses.RowCount.ToString();
                return;
            }



            DataView view = tableOfDetainedLicenses.DefaultView;



            if (FilterColumn == "LicenseID")
                //in this case we deal with integer not string.

                view.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txtBFilterBy.Text.Trim());
            else
                view.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, txtBFilterBy.Text.Trim());

            dgvDetainedLicenses.DataSource = view;
            lblRecordsNum.Text = dgvDetainedLicenses.RowCount.ToString();
        }

        private void txtBFilterBy_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (comboBoxFilterBy.Text == "LicenseID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (dgvDetainedLicenses.CurrentRow == null || dgvDetainedLicenses.CurrentRow.Index < 0)
            {
                e.Cancel = true;
                return;
            }
            else e.Cancel = false;

            if (!clsDetainLicenses.isLicenseDetained((int)dgvDetainedLicenses.CurrentRow.Cells[1].Value))
                releaseDetainedLicenseToolStripMenuItem.Enabled = false;


        }

        private void showPersonDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clsLicenses license = clsLicenses.Find((int)dgvDetainedLicenses.CurrentRow.Cells[1].Value);
            formShowPersonInfo form = new formShowPersonInfo(license.applicationInfo.ApplicantPersonID);
            form.ShowDialog();
        }

        private void showLicenseInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowDriverLicenseInfoForm form = new ShowDriverLicenseInfoForm();
            form.loadDriverLicenseInfoByLicenseID((int)dgvDetainedLicenses.CurrentRow.Cells[1].Value);
            form.ShowDialog();
        }

        private void showPersonsLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clsLicenses license = clsLicenses.Find((int)dgvDetainedLicenses.CurrentRow.Cells[1].Value);
            ShowLicenseHistoryForm form = new ShowLicenseHistoryForm(license.applicationInfo.ApplicantPersonID);
            form.ShowDialog();
        }

        private void releaseDetainedLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReleaseDetainedLicenseForm form = new ReleaseDetainedLicenseForm();
            form.LoadLicenseInfo((int)dgvDetainedLicenses.CurrentRow.Cells[1].Value);
            form.ShowDialog();
            FillDetainedLicensesTable();
        }

        private void btnDetain_Click(object sender, EventArgs e)
        {
            DetainLicenseForm form = new DetainLicenseForm();
            form.ShowDialog();
            FillDetainedLicensesTable();
        }

        private void btnRelease_Click(object sender, EventArgs e)
        {
            ReleaseDetainedLicenseForm form = new ReleaseDetainedLicenseForm();
            form.ShowDialog();
            FillDetainedLicensesTable();
        }
    }
}
