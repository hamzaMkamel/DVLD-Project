using BusinessLayer;
using DVLD_Project.Licenses.International_Licenses;
using DVLD_Project.Licenses.LocalLicenses;
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

namespace DVLD_Project.Licenses.Controls
{
    public partial class DriverLicensesControl : UserControl
    {
        int driverID;
        clsDrivers driverInfo;
        public DriverLicensesControl()
        {
            InitializeComponent();
        }

        public void LoadLicenses(int DriverID)
        {
            driverID = DriverID;
            driverInfo = clsDrivers.Find(driverID);

            if(driverInfo == null)
            {
                MessageBox.Show($"Driver With ID : {driverID} Not found ..! Error !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            LoadLocalLicenses();
            LoadInternationalLicenses();
            // later on we should handle international one
        }

        private void LoadLocalLicenses()
        {
            DataTable table = clsLicenses.getAllLicensesToSpecificDriver(driverID);
            dgvLocalLicenses.DataSource = table.DefaultView;
            if (dgvLocalLicenses.RowCount == 0)
            {
                labelLocalNote.Visible = true;
            }
            lblLocalRecords.Text = dgvLocalLicenses.RowCount.ToString();
        }
        private void LoadInternationalLicenses()
        {
            DataTable table = clsInternationalLicenses.getAllLicensesToSpecificDriver(driverID);
            dgvInternationalLicenses.DataSource = table.DefaultView;
            if (dgvInternationalLicenses.RowCount == 0)
            {
                labelInternationalNote.Visible = true;
            }
            lblLocalRecords.Text = dgvLocalLicenses.RowCount.ToString();

        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (dgvLocalLicenses.RowCount == 0)
            {
                e.Cancel = true;

            }
            else e.Cancel = false;

        }

        public void Clear()
        {
            
        }

        private void showLicenseNToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowDriverLicenseInfoForm form = new ShowDriverLicenseInfoForm();
            form.loadDriverLicenseInfoByLicenseID((int)dgvLocalLicenses.CurrentRow.Cells[0].Value);
            form.ShowDialog();
        }

        private void toolStripMenuShowInternationalLicenseInfo_Click(object sender, EventArgs e)
        {
            ShowInternationalLicenseInfoForm form = new ShowInternationalLicenseInfoForm((int)dgvInternationalLicenses.CurrentRow.Cells[0].Value);
            form.ShowDialog();
        }
    }
}
