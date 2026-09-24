using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project
{
    public partial class ManagePeopleForm : Form
    {


        private DataTable tableOfPeople;
        private DataView tableView = new DataView();
        public ManagePeopleForm()
        {
            InitializeComponent();
            fillPeopleTable();
            fillComboBoxFilterBy();
        }

        private void fillComboBoxFilterBy()
        {
            DataTable table = tableOfPeople;
            foreach (DataColumn column in table.Columns)
            {

                comboBoxFilterBy.Items.Add(column.ToString());

            }
            comboBoxFilterBy.SelectedItem = "None";
            comboBoxFilterBy.Items.Remove("DateOfBirth");

        }

        private void fillPeopleTable()
        {

            tableOfPeople = clsPeople.getAllPeople();
            tableView = tableOfPeople.DefaultView;
            dgvPeople.DataSource = tableView;
            lblRecordsNum.Text = dgvPeople.RowCount.ToString();

            if (dgvPeople.Rows.Count > 0)
            {

                dgvPeople.Columns[0].HeaderText = "Person ID";
                

                dgvPeople.Columns[1].HeaderText = "National No.";

                dgvPeople.Columns[2].HeaderText = "First Name";
                

                dgvPeople.Columns[3].HeaderText = "Second Name";

                dgvPeople.Columns[4].HeaderText = "Third Name";
                

                dgvPeople.Columns[5].HeaderText = "Last Name";
                

                dgvPeople.Columns[6].HeaderText = "Gendor";
                
                dgvPeople.Columns[7].HeaderText = "Date Of Birth";
                
                dgvPeople.Columns[8].HeaderText = "Nationality";

                dgvPeople.Columns[9].HeaderText = "Phone";

                dgvPeople.Columns[10].HeaderText = "Email";
                
            }



        }

        private void ManagePeopleForm_Load(object sender, EventArgs e)
        {
            
        }

        private void btnToolStripAddNewPerson_Click(object sender, EventArgs e)
        {
            formAddNewUpdatePerson form = new formAddNewUpdatePerson(-1);
            //Delegate Implementaion :
            form.callaMethod += fillPeopleTable;
            form.ShowDialog();

            
            

        }

        private void btnToolStripEdit_Click(object sender, EventArgs e)
        {
            if (dgvPeople.CurrentRow == null || dgvPeople.CurrentRow.Index < 0) return;
            formAddNewUpdatePerson form = new formAddNewUpdatePerson((int)dgvPeople.CurrentRow.Cells[0].Value);
            //Delegate Implementaion :
            form.callaMethod += fillPeopleTable;
            form.ShowDialog();
            
        }

        private void txtBFilterBy_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = comboBoxFilterBy.SelectedItem.ToString();
            //Map Selected Filter to real Column name 
            if (string.IsNullOrWhiteSpace(txtBFilterBy.Text)) {
                tableView.RowFilter = "";
                dgvPeople.DataSource = tableView;
                lblRecordsNum.Text = dgvPeople.RowCount.ToString();
                return;
            }
            


            DataView view = tableOfPeople.DefaultView;
            
            

            if (FilterColumn == "PersonID")
                //in this case we deal with integer not string.

                view.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txtBFilterBy.Text.Trim());
            else
                view.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, txtBFilterBy.Text.Trim());

            dgvPeople.DataSource = view;
            lblRecordsNum.Text = dgvPeople.RowCount.ToString();


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

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolStripbtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvPeople.CurrentRow == null || dgvPeople.CurrentRow.Index < 0) return;

            if (MessageBox.Show("This person will be Deleted Forever! , Are you Sure?", "Take A Decision :", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation)
                == DialogResult.Yes)
            {
                if (clsPeople.DeletePerson((int)dgvPeople.CurrentRow.Cells[0].Value))
                {
                    MessageBox.Show("Person Deleted Successfully!", "Note :", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    fillPeopleTable();
                }
                else
                    MessageBox.Show("This Person Can't Be Deleted!", "Error :", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void sendEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Will be added soon...", "Note");
        }

        private void phoneCallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Will be added soon...", "Note");
        }

        private void personDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvPeople.CurrentRow == null || dgvPeople.CurrentRow.Index < 0) return;

            formShowPersonInfo form = new formShowPersonInfo((int)dgvPeople.CurrentRow.Cells[0].Value);
            form.ShowDialog();
        }

        private void txtBFilterBy_KeyPress(object sender, KeyPressEventArgs e)
        {
            //we allow number incase person id is selected.
            if (comboBoxFilterBy.Text == "PersonID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
    }
}
