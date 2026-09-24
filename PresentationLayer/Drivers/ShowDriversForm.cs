using BusinessLayer;
using DVLD_Project.Licenses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project.Drivers
{
    public partial class ShowDriversForm : Form
    {
        DataTable tableOfDrivers;
        DataView tableView;
        public ShowDriversForm()
        {
            InitializeComponent();
            fillDriversTable();
        }

        private void fillDriversTable()
        {
            tableOfDrivers = clsDrivers.getAllDriversTable();
            tableView = tableOfDrivers.DefaultView;
            dgvDrivers.DataSource = tableView;
            lblRecordsNum.Text = dgvDrivers.RowCount.ToString();
            comboBoxFilterBy.SelectedIndex = 0;

            if (dgvDrivers.Rows.Count > 0)
            {





                dgvDrivers.Columns[0].HeaderText = "Driver ID";


                dgvDrivers.Columns[1].HeaderText = "Person ID";

                dgvDrivers.Columns[2].HeaderText = "National No.";


                dgvDrivers.Columns[3].HeaderText = "Full Name";

                dgvDrivers.Columns[4].HeaderText = "Date";


                dgvDrivers.Columns[5].HeaderText = "Active Licenses";







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

        private void txtBFilterBy_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (comboBoxFilterBy.Text == "DriverID" || comboBoxFilterBy.Text == "PersonID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void txtBFilterBy_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = comboBoxFilterBy.SelectedItem.ToString();

            if (string.IsNullOrWhiteSpace(txtBFilterBy.Text))
            {
                tableView.RowFilter = "";
                dgvDrivers.DataSource = tableView;
                lblRecordsNum.Text = dgvDrivers.RowCount.ToString();
                return;
            }



            DataView view = tableOfDrivers.DefaultView;



            if (FilterColumn == "DriverID" || FilterColumn == "PersonID")
                //in this case we deal with integer not string.

                view.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txtBFilterBy.Text.Trim());
            else
                view.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, txtBFilterBy.Text.Trim());

            dgvDrivers.DataSource = view;
            lblRecordsNum.Text = dgvDrivers.RowCount.ToString();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void showPersonInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int DriverID = (int)dgvDrivers.CurrentRow.Cells[0].Value;
            clsDrivers driver = clsDrivers.Find(DriverID);
            if (driver == null)
            {
                MessageBox.Show("Perrson Not found ..! Error !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
            formShowPersonInfo form = new formShowPersonInfo(driver.PersonID);
            form.ShowDialog();
        }

        private void showPersonLicensesHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int DriverID = (int)dgvDrivers.CurrentRow.Cells[0].Value;
            clsDrivers driver = clsDrivers.Find(DriverID);
            if (driver == null)
            {
                MessageBox.Show("Perrson Not found ..! Error !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
            ShowLicenseHistoryForm form = new ShowLicenseHistoryForm(driver.PersonID);
            form.ShowDialog();
        }
    }
}
    

