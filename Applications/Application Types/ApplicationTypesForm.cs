using BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project.Applications.Application_Types
{
    public partial class ApplicationTypesForm : Form
    {
        DataTable tableOfApplicationTypes;
        DataView viewOfApplicationTypes;
        public ApplicationTypesForm()
        {
            InitializeComponent();
            fillApplicationTypesDataGridView();
        }

        public void fillApplicationTypesDataGridView()
        {
            tableOfApplicationTypes = clsApplicationTypes.getAllApplicationTypes();
            viewOfApplicationTypes = tableOfApplicationTypes.DefaultView;
            dgvApplicationTypes.DataSource = viewOfApplicationTypes;
            lblRecordsNum.Text = dgvApplicationTypes.RowCount.ToString();

            if (dgvApplicationTypes.Rows.Count > 0)
            {

                dgvApplicationTypes.Columns[0].HeaderText = "Application Type ID";
                dgvApplicationTypes.Columns[1].HeaderText = "Application Type Title";
                dgvApplicationTypes.Columns[2].HeaderText = "Application Type Fees";

            }
        }

        private void editApplicationTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvApplicationTypes.CurrentRow == null || dgvApplicationTypes.CurrentRow.Index < 0) return;

            

            EditApplicationType form = new EditApplicationType((int)dgvApplicationTypes.CurrentRow.Cells[0].Value);
            form.ShowDialog();
            fillApplicationTypesDataGridView();

        }

        private void ApplicationTypesForm_Load(object sender, EventArgs e)
        {
            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
