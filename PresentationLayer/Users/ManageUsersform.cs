using BusinessLayer;
using DVLD_Project.Global_Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project.Users
{
    public partial class ManageUsersform : Form
    {
        DataTable tableOfUsers;
        DataView viewOfUsers;
        public ManageUsersform()
        {
            InitializeComponent();
            fillUsersDataGridView();
            fillCombobox();
        }

        private void fillCombobox()
        {
            
            foreach (DataColumn column in tableOfUsers.Columns)
            {

                comboBoxFilterBy.Items.Add(column.ToString());

            }
            comboBoxFilterBy.SelectedItem = "None";
        }

        public void fillUsersDataGridView()
        {
            tableOfUsers = clsUsers.getAllUsers();
            viewOfUsers = tableOfUsers.DefaultView;
            dgvUsers.DataSource = viewOfUsers;
            lblRecordsNum.Text = dgvUsers.RowCount.ToString();

            if (dgvUsers.Rows.Count > 0)
            {

                dgvUsers.Columns[0].HeaderText = "User ID";


                dgvUsers.Columns[1].HeaderText = "Person ID";

                dgvUsers.Columns[2].HeaderText = "Full Name";
                dgvUsers.Columns[3].HeaderText = "User Name";
                dgvUsers.Columns[4].HeaderText = "Is Active";


            }
        }

        private void btnToolStripEdit_Click(object sender, EventArgs e)
        {
            if (dgvUsers.CurrentRow == null || dgvUsers.CurrentRow.Index < 0) return;

            AddNewUpdateUserform form = new AddNewUpdateUserform((int)dgvUsers.CurrentRow.Cells[0].Value);
            form.onUserSaved += fillUsersDataGridView;
            form.ShowDialog();

        }

        private void btnToolStripAddNewUser_Click(object sender, EventArgs e)
        {
            AddNewUpdateUserform form = new AddNewUpdateUserform(-1);
            form.onUserSaved += fillUsersDataGridView; // or to update the form just call the load method using (null , null) parameters;
            form.ShowDialog();
        }

        private void userDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvUsers.CurrentRow == null || dgvUsers.CurrentRow.Index < 0) return;
            ShowUserInfoForm form = new ShowUserInfoForm((int)dgvUsers.CurrentRow.Cells[0].Value);
            form.ShowDialog();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (dgvUsers.CurrentRow == null || dgvUsers.CurrentRow.Index < 0) return;
            ChangePasswordForm form = new ChangePasswordForm((int)dgvUsers.CurrentRow.Cells[0].Value);
            form.ShowDialog();
        }

        private void toolStripbtnDelete_Click(object sender, EventArgs e)
        {

            if (dgvUsers.CurrentRow == null || dgvUsers.CurrentRow.Index < 0) return;

            if ((int)dgvUsers.CurrentRow.Cells[0].Value == GlobalClass.CurrentUser.UserID)
            {
                MessageBox.Show("This User can't be deleted now because he is signed to the system!", "Error :", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (MessageBox.Show("This User will be Deleted.. , Are you Sure?", "Take A Decision :", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation)
                == DialogResult.Yes)
            {
                if (clsUsers.deleteUser((int)dgvUsers.CurrentRow.Cells[0].Value))
                {
                    MessageBox.Show("User Deleted Successfully!", "Note :", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    fillUsersDataGridView();
                }
                else
                    MessageBox.Show("This User Can't Be Deleted!", "Error :", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtBFilterBy_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = comboBoxFilterBy.SelectedItem.ToString();
            //Map Selected Filter to real Column name 
            if (string.IsNullOrWhiteSpace(txtBFilterBy.Text))
            {
                viewOfUsers.RowFilter = "";
                dgvUsers.DataSource = viewOfUsers;
                lblRecordsNum.Text = dgvUsers.RowCount.ToString();
                return;
            }



            DataView view = tableOfUsers.DefaultView;



            if (FilterColumn == "PersonID" || FilterColumn == "UserID")
                //in this case we deal with integer not string.

                view.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txtBFilterBy.Text.Trim());
            else
                view.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, txtBFilterBy.Text.Trim());

            dgvUsers.DataSource = view;
            lblRecordsNum.Text = dgvUsers.RowCount.ToString();
        }

        private void comboBoxFilterByActivation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxFilterByActivation.SelectedItem.ToString().Equals("All"))
            {
                viewOfUsers.RowFilter = "";
                dgvUsers.DataSource = viewOfUsers;
                lblRecordsNum.Text = dgvUsers.RowCount.ToString();
                return;
            }
            else 
            {
                DataView view = tableOfUsers.DefaultView;
                if (comboBoxFilterByActivation.SelectedItem.ToString().Equals("Yes"))
                {
                    view.RowFilter = string.Format("[IsActive] = 1");
                }
                else
                {
                    view.RowFilter = string.Format("[IsActive] = 0");
                }

                dgvUsers.DataSource = view;
                lblRecordsNum.Text = dgvUsers.RowCount.ToString();
            }
        }

        private void comboBoxFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxFilterBy.SelectedItem.ToString().Equals("None"))
            {
                txtBFilterBy.Visible = false;
                txtBFilterBy.Text = "";
                comboBoxFilterByActivation.Visible = false;
                comboBoxFilterByActivation.SelectedIndex = 0;

            }
            else if(comboBoxFilterBy.SelectedItem.ToString().Equals("IsActive"))
            {
                comboBoxFilterByActivation.Visible = true;
                txtBFilterBy.Visible = false;
                comboBoxFilterByActivation.Focus();
            }
            else
            {
                txtBFilterBy.Visible = true;
                comboBoxFilterByActivation.Visible = false;
                txtBFilterBy.Focus();
            }
        }

        private void txtBFilterBy_KeyPress(object sender, KeyPressEventArgs e)
        {
            //we allow number incase person id is selected.
            if (comboBoxFilterBy.Text == "PersonID" || comboBoxFilterBy.Text == "UserID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }


}
