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

namespace DVLD_Project.Licenses
{
    public partial class ShowLicenseHistoryForm : Form
    {
        int PersonID;
        public ShowLicenseHistoryForm(int PersonID)
        {
            InitializeComponent();
            this.PersonID = PersonID;
            LoadFormInfo();
        }

        private void LoadFormInfo()
        {
            clsPeople person = clsPeople.Find(PersonID);
            if(person == null)
            {
                MessageBox.Show("Perrson Not found ..! Error !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            showPersonInfoWithFilterControl1.LoadPersonInfo(PersonID);
            showPersonInfoWithFilterControl1.FilterEnabled = false;
            driverLicensesControl1.LoadLicenses(clsDrivers.getDriverIDByPersonID(PersonID));
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
