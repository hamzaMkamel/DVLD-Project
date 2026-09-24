using BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project.Applications.TestTypes
{
    public partial class EditTestTypeForm : Form
    {
        clsTestTypes testType;
        public EditTestTypeForm(int TestID)
        {
            InitializeComponent();
            fillEditTestTypeForm(TestID);
        }

        private void fillEditTestTypeForm(int TestID)
        {
            testType = clsTestTypes.getTestTypeByID(TestID);
            if (testType != null)
            {
                lblID.Text = testType.TestTypeID.ToString();
                txtBTitle.Text = testType.TestTypeTitle;
                txtBDescription.Text = testType.TestTypeDescription;
                txtBFees.Text = testType.TestTypeFees.ToString();
            }
            else
            {
                MessageBox.Show("Error This Test Type Not Found!", "Error :", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void TextboxValidating(object sender, CancelEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if (string.IsNullOrEmpty(txt.Text))
            {
                errorProvider1.SetError(txt, "This Field Can't Be Empty!");
            }
            else
                errorProvider1.SetError(txt, null);
        }
        bool checkForErrors()
        {
            if (!string.IsNullOrEmpty(errorProvider1.GetError(txtBTitle)))
                return true;
            else if (!string.IsNullOrEmpty(errorProvider1.GetError(txtBFees)))
                return true;
            else if (!string.IsNullOrEmpty(errorProvider1.GetError(txtBDescription)))
                return true;

            return false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (checkForErrors())
            {
                MessageBox.Show("Some fields contains invalid data, please review them and try again.",
                    "Error :", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }

            testType.TestTypeTitle = txtBTitle.Text;
            testType.TestTypeDescription = txtBDescription.Text;
            testType.TestTypeFees = decimal.Parse(txtBFees.Text);

            if (testType.updateTestTypeInfo())
            {

                MessageBox.Show("Test Type updated Successfully.",
                        "Note :", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Somthing happend while updating Test Type..(Please Call the adminstrator)",
                    "Error :", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void txtBFees_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
    }

