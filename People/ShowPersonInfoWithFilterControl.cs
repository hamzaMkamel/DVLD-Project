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
    public partial class ShowPersonInfoWithFilterControl : UserControl
    {
        public event Action<int> OnPersonSelected;

        protected virtual void PersonSelected(int personID)
        {
            Action<int> handler = OnPersonSelected;
            if (handler != null)
            {
                OnPersonSelected(showPersonInfoControl1.Person.PersonID);
            }


        }

        
        public int PersonID { get { return showPersonInfoControl1.Person.PersonID; } }

        public clsPeople SelectedPersonInfo {get { return showPersonInfoControl1.Person; } }

        bool _ShowAddPerson = true;
        public bool ShowAddPerson
        {
            set
            {
                _ShowAddPerson = value;
                btnAdd.Visible = value;
            }
            get { return _ShowAddPerson; }
        }
        bool _FilterEnabled = true;
        public bool FilterEnabled
        {
            set
            {
                _FilterEnabled = value;
                groupBox1.Enabled = value;
            }
            get { return _FilterEnabled; }
        }

        public ShowPersonInfoWithFilterControl()
        {
            
            InitializeComponent();
            comboBoxFilter.SelectedIndex = 0;
            maskedTextBox1.Focus();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(maskedTextBox1.Text))
                FindNow();
        }

        

        private void btnAdd_Click(object sender, EventArgs e)
        {
            formAddNewUpdatePerson form = new formAddNewUpdatePerson(-1);
            form.onPersonSaved += LoadPersonInfo;
            form.ShowDialog();
        }

        public void LoadPersonInfo(int PersonID)
        {
            comboBoxFilter.SelectedIndex = 0;
            maskedTextBox1.Text = PersonID.ToString();
            FindNow();
        }

        public void LoadPersonInfo(string NationalNo)
        {
            comboBoxFilter.SelectedIndex = 1;
            maskedTextBox1.Text = NationalNo;
            FindNow();
        }

        private void FindNow()
        {
            switch(comboBoxFilter.Text)
            {
                case "Person ID":
                    {
                        if (int.TryParse(maskedTextBox1.Text, out int ID))
                        {
                            showPersonInfoControl1.LoadPersonInfo(ID);
                        }
                        else
                        {
                            MessageBox.Show("Please Enter a valid ID.", "Note : ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                            break;
                    }
                case "National No":
                    {
                        showPersonInfoControl1.LoadPersonInfo(maskedTextBox1.Text);
                        break;
                    }
                default: break;
            }

            if(FilterEnabled && showPersonInfoControl1.Person != null)
            {
                if(OnPersonSelected != null)
                    OnPersonSelected(PersonID);
            }
        }

        private void maskedTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)13)
            {
                btnSearch.PerformClick();
            }

            //this will allow only digits if person id is selected
            if (maskedTextBox1.Text == "Person ID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
    }
}
