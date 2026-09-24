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
    public partial class EditApplicationType : Form
    {
        clsApplicationTypes appType;
        public EditApplicationType(int AppID)
        {
            InitializeComponent();
            fillEditForm(AppID);
        }

        private void fillEditForm(int AppID)
        {
            appType = clsApplicationTypes.getApplicationTypeByID(AppID);
            if(appType != null)
            {
                lblID.Text = appType.ApplicationTypeID.ToString();
                txtBTitle.Text = appType.ApplicationTypeName;
                txtBFees.Text = appType.ApplicationTypeFees.ToString();
            }
            else
            {
                MessageBox.Show("Error This Application Type Not Found!", "Error :", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void TextBoxValidating(object sender, CancelEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if(string.IsNullOrEmpty(txt.Text))
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

            appType.ApplicationTypeName = txtBTitle.Text;
            appType.ApplicationTypeFees = decimal.Parse(txtBFees.Text);

            if(appType.updateApplicationType())
            {

                MessageBox.Show("Application Type updated Successfully.",
                        "Note :", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Somthing happend while updating Application Type..(Please Call the adminstrator)",
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
