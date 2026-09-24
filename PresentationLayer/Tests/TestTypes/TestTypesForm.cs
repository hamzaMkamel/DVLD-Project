using BusinessLayer;
using DVLD_Project.Applications.Application_Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project.Applications.TestTypes
{
    
    public partial class TestTypesForm : Form
    {
        DataTable tableOfTestTypes;
        DataView viewOfTestTypes;

        public TestTypesForm()
        {
            InitializeComponent();
            fillTestTypesDataGridView();
        }

        public void fillTestTypesDataGridView()
        {
            tableOfTestTypes = clsTestTypes.getAllTestTypes();
            viewOfTestTypes = tableOfTestTypes.DefaultView;
            dgvTestTypes.DataSource = viewOfTestTypes;
            lblRecordsNum.Text = dgvTestTypes.RowCount.ToString();

            if (dgvTestTypes.Rows.Count > 0)
            {

                dgvTestTypes.Columns[0].HeaderText = "Test Type ID";
                dgvTestTypes.Columns[1].HeaderText = "Application Type Title";
                dgvTestTypes.Columns[2].HeaderText = "Test Type Description";
                dgvTestTypes.Columns[3].HeaderText = "Test Type Fees";

            }
        }

        private void editTestTypeInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvTestTypes.CurrentRow == null || dgvTestTypes.CurrentRow.Index < 0) return;
            EditTestTypeForm form = new EditTestTypeForm((int)dgvTestTypes.CurrentRow.Cells[0].Value);
            form.ShowDialog();
            fillTestTypesDataGridView();
            

            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
