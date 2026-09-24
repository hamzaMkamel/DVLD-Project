using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project.Licenses.International_Licenses
{
    public partial class ShowInternationalLicenseInfoForm : Form
    {
        public ShowInternationalLicenseInfoForm(int LicenseID)
        {
            InitializeComponent();
            showInternationalDriverLicenseInfoControl1.LoadDriverLicenseInfo(LicenseID);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
