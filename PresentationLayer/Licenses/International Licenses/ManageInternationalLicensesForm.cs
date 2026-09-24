using BusinessLayer;
using DVLD_Project.Applications.International_Applications;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project.Licenses.International_Licenses
{
    public partial class ManageInternationalLicensesForm : Form
    {
        DataTable tableOfInternationalApplications;
        DataView view;
        public ManageInternationalLicensesForm()
        {
            InitializeComponent();
            fillInternationalApplicationsTable();
        }

        private void fillInternationalApplicationsTable()
        {

            tableOfInternationalApplications = clsInternationalLicenses.getAllInternationalLicenses();
            view = tableOfInternationalApplications.DefaultView;
            dgvInternationalApplications.DataSource = view;
            lblRecordsNum.Text = dgvInternationalApplications.RowCount.ToString();
            comboBoxFilterBy.SelectedIndex = 0;

            if (dgvInternationalApplications.Rows.Count > 0)
            {





                dgvInternationalApplications.Columns[0].HeaderText = "Int.License ID";


                dgvInternationalApplications.Columns[1].HeaderText = "Application ID";

                dgvInternationalApplications.Columns[2].HeaderText = "Driver ID";


                dgvInternationalApplications.Columns[3].HeaderText = "L.License ID";

                dgvInternationalApplications.Columns[4].HeaderText = "Issue Date";


                dgvInternationalApplications.Columns[5].HeaderText = "Expiration Date";


                dgvInternationalApplications.Columns[6].HeaderText = "Is Active";




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
                this.view.RowFilter = "";
                dgvInternationalApplications.DataSource = this.view;
                lblRecordsNum.Text = dgvInternationalApplications.RowCount.ToString();
                return;
            }



            DataView view = tableOfInternationalApplications.DefaultView;



            

                view.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txtBFilterBy.Text.Trim());
            

            dgvInternationalApplications.DataSource = view;
            lblRecordsNum.Text = dgvInternationalApplications.RowCount.ToString();
        }

        private void txtBFilterBy_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAddNewInternationalLicenseApplication_Click(object sender, EventArgs e)
        {
            AddInternationalLicenseFrom form = new AddInternationalLicenseFrom();
            form.ShowDialog();
            fillInternationalApplicationsTable();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (dgvInternationalApplications.CurrentRow == null || dgvInternationalApplications.CurrentRow.Index < 0)
            {
                e.Cancel = true;
                return;
            }
            else e.Cancel = false;
        }

        private void showPersonDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clsInternationalLicenses license = clsInternationalLicenses.Find((int)dgvInternationalApplications.CurrentRow.Cells[0].Value);
            formShowPersonInfo form = new formShowPersonInfo(license.applicationInfo.ApplicantPersonID);
            form.ShowDialog();
        }

        private void showLicenseDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowInternationalLicenseInfoForm form = new ShowInternationalLicenseInfoForm((int)dgvInternationalApplications.CurrentRow.Cells[0].Value);
            form.ShowDialog();
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clsInternationalLicenses license = clsInternationalLicenses.Find((int)dgvInternationalApplications.CurrentRow.Cells[0].Value);
            ShowLicenseHistoryForm form = new ShowLicenseHistoryForm(license.applicationInfo.ApplicantPersonID);
            form.ShowDialog();
        }
    }
}
