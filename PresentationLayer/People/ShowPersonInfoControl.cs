using BusinessLayer;
using DVLD_Project.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project
{
    public partial class ShowPersonInfoControl : UserControl
    {
        private clsPeople _person;

        public clsPeople Person
        {
            set { _person = value; }
            get { return _person; }
        }
        int personID = -1;
        

        
        
        void resetDefalutImage()
        {
            if (_person.Gendor == 0)
                pictureBox1.Image = Resources.Male_512;
            else
                pictureBox1.Image = Resources.Female_512;
        }

        public ShowPersonInfoControl()
        {
            InitializeComponent();
        }

        public void LoadPersonInfo(int personID)
        {
            _person = clsPeople.Find(personID);
            if (_person == null)
            {
                ResetPersonInfo();
                MessageBox.Show("Person With ID :" + personID + " is not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            fillControlWithInfo();
        }
        public void LoadPersonInfo(string NationalNo)
        {
            _person = clsPeople.Find(NationalNo);
            if (_person == null)
            {
                ResetPersonInfo();
                MessageBox.Show("Person With ID :" + personID + " is not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            fillControlWithInfo();
        }

        private void ResetPersonInfo()
        {
            lblID.Text = "N/A";
            lblName.Text = "N/A";
            lblNationalNo.Text = "N/A";
            lblGendor.Text = "N/A";
            lblEmail.Text = "N/A";
            lblAddress.Text = "N/A";
            lblDateOfBirth.Text = "N/A";
            lblPhone.Text = "N/A"; ;
            lblCountry.Text = "N/A";
            pictureBox1.Image = Resources.Male_512;
            linkLabel1.Visible = false;
        }
        private void fillControlWithInfo()
        {
            lblID.Text = _person.PersonID.ToString();
            lblName.Text = $"{_person.FirstName} {_person.SecondName} {_person.LastName}";
            lblNationalNo.Text = _person.NationalNo;
            lblGendor.Text = (_person.Gendor == 0) ? "Male" : "Female";
            lblEmail.Text = _person.Email;
            lblAddress.Text = _person.Address;
            lblDateOfBirth.Text = _person.DateOfBirth.ToShortDateString();
            lblPhone.Text = _person.Phone;
            lblCountry.Text = _person.CountryInformation.CountryName;
            if (string.IsNullOrEmpty(_person.ImagePath))
                resetDefalutImage();
            else
            {
                if (File.Exists(_person.ImagePath))
                {
                    pictureBox1.ImageLocation = _person.ImagePath;
                }
                else
                {
                    MessageBox.Show("Can't Load Image, Image not found :", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    resetDefalutImage();
                }

            }
                

            linkLabel1.Visible = true;


        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            formAddNewUpdatePerson form = new formAddNewUpdatePerson(_person.PersonID);
            form.ShowDialog();
            LoadPersonInfo(_person.PersonID); // refresh
        }
    }
}
