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

namespace DVLD_Project.Licenses.LocalLicenses.Controls
{
    public partial class ShowDriverLicenseWithFilterControl : UserControl
    {
        public int LicenseID;
        public clsLicenses licenseInfo
        {
            get
            {
                return driverLicenseInfoControl1.license;
            }
        }

        public event Action<int> OnLicenseSelected;

        protected virtual void LicenseSelected(int LicenseID)
        {
            Action<int> handler = OnLicenseSelected;
            if (handler != null)
            {
                OnLicenseSelected(LicenseID);
            }


        }
        public ShowDriverLicenseWithFilterControl()
        {
            InitializeComponent();

        }
        public bool FilterEnabled {
            set
            {
                groupBoxFilter.Enabled = value;
            }
            get
            {
                return groupBoxFilter.Enabled;
            }
            }

        public void LoadLicenseInfo(int LicenseID)
        {
            this.LicenseID = LicenseID;
            driverLicenseInfoControl1.LoadDriverLicenseInfo(LicenseID);
            FilterEnabled = false;
        }

        private void FindNow()
        {
            

            if(int.TryParse(textBoxFilter.Text , out int id))
            {
                driverLicenseInfoControl1.LoadDriverLicenseInfo(id);
                LicenseID = id;
            }
            else
            {
                MessageBox.Show("Please Enter a valid ID Number.", "Note : ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (FilterEnabled && driverLicenseInfoControl1.license != null)
                if(OnLicenseSelected != null)
                    OnLicenseSelected(driverLicenseInfoControl1.license.LicenseID);

            
        }

        private void textBoxFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);

            if (e.KeyChar == (char)13)
            {
                btnSearch.PerformClick();
            }

            
            
            
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            
                FindNow();
            
        }

        
    }
}
