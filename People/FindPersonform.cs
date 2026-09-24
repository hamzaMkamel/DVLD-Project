using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project.People
{
    public partial class FindPersonform : Form
    {

        public delegate Action<int> DataBackHandler(object sender , int PersonID);
        public event DataBackHandler dataBack;
        public FindPersonform()
        {
            InitializeComponent();
        }

        private void FindPersonform_FormClosing(object sender, FormClosingEventArgs e)
        {
            dataBack?.Invoke(this, showPersonInfoWithFilterControl1.SelectedPersonInfo.PersonID);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
