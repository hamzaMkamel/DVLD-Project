using BusinessLayer;
using DVLD_Project.Properties;
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
    public partial class formAddNewUpdatePerson : Form
    {
        clsPeople.enMode mode;
        clsPeople person;

        public delegate void callAMethod();
        public event callAMethod callaMethod;
        public delegate void OnPersonSaved(int personID);
        public event OnPersonSaved onPersonSaved;
        public formAddNewUpdatePerson(int PersonID)
        {
            InitializeComponent();
            loadCountriesInCombobox();
            dateTimePicker1.MaxDate = DateTime.Now.AddYears(-18);
            if (PersonID != -1) {
                mode = clsPeople.enMode.Update;
                fillFormWithPersonInfo(PersonID);
            }
            else
            {
                mode = clsPeople.enMode.addNew;
                this.person = new clsPeople();
                
            }

            
        }

        private void restDefaultImage()
        {
            if (rBMale.Checked)
                pictureBoxPhoto.Image = Resources.Male_512;
            else
                pictureBoxPhoto.Image = Resources.Female_512;
        }

        private void loadCountriesInCombobox()
        {
            DataTable dt = clsCountries.getAllCountries();

            foreach (DataRow row in dt.Rows)
            {

                comboBoxCountry.Items.Add(row["CountryName"]);

            }

            comboBoxCountry.SelectedIndex = comboBoxCountry.Items.IndexOf("Syria");
        }

        private void fillFormWithPersonInfo(int PersonID)
        {
            person = clsPeople.Find(PersonID);

            if (person == null)
            {
                MessageBox.Show("Error Person not Found !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            labelMode.Text = "Update Person";
            labelID.Text = person.PersonID.ToString();
            txtBFirstName.Text = person.FirstName;
            txtBLastName.Text = person.LastName;
            txtBSecondName.Text = person.SecondName;
            txtBThirdName.Text = person.ThirdName;
            txtBNationalNo.Text = person.NationalNo;
            dateTimePicker1.Value = person.DateOfBirth;
            txtBPhone.Text = person.Phone;
            txtBEmail.Text = person.Email;
            txtBAddress.Text = person.Address;
            
            if (person.Gendor == 0) rBMale.Select(); else rBFemale.Select();
            comboBoxCountry.SelectedItem = person.CountryInformation.CountryName;
            if (string.IsNullOrEmpty(person.ImagePath))
            {
                restDefaultImage();
                
            }
            else
            {
                pictureBoxPhoto.ImageLocation = person.ImagePath;

                
            }
            linklabelRemoveImage.Visible = (!string.IsNullOrEmpty(person.ImagePath));
        }

        private void linklabelSetImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                person.ImagePath = openFileDialog1.FileName;
                pictureBoxPhoto.Image = Image.FromFile(openFileDialog1.FileName);
                linklabelRemoveImage.Visible = true;
            }

        }

        private void linklabelRemoveImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pictureBoxPhoto.ImageLocation = null;
            restDefaultImage();
            person.ImagePath = "";
            linklabelRemoveImage.Visible = false;
            restDefaultImage();
            
        }

        private void rBMaleOrFemaleChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(person.ImagePath)) return;
            restDefaultImage();

        }

       
        private void txtBValidating(object sender, CancelEventArgs e)
        {
            //to check if all (required) textboxes is fill with info
            TextBox txtB1 = (TextBox)sender;
            if(string.IsNullOrEmpty(txtB1.Text))
            {
                errorProvider1.SetError(txtB1, "This Field Can't Be Empty!");
                
                
            }
            else
            {
                errorProvider1.SetError(txtB1, null);
                
            }
        }

        private bool checkForAnyErrors()
        {
            foreach (Control c in groupBox1.Controls)
            {
                if (c is TextBox)
                {
                    TextBox txtB = (TextBox)c;
                    if (!string.IsNullOrWhiteSpace(errorProvider1.GetError(txtB)))
                        return true;
                }
            }
            return false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (checkForAnyErrors()) //!this.ValidateChildren() you cant implement this
            {
                MessageBox.Show("Some fields contains invalid data, please review them and try again.",
                    "Error :", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            person.FirstName = txtBFirstName.Text;
            person.SecondName = txtBSecondName.Text;
            person.ThirdName = txtBThirdName.Text;
            person.LastName = txtBLastName.Text;
            person.Email = txtBEmail.Text;
            person.Phone = txtBPhone.Text;
            person.Address = txtBAddress.Text;
            person.NationalNo = txtBNationalNo.Text;
            person.DateOfBirth = dateTimePicker1.Value;
            person.NationalityCountryID = clsCountries.Find(comboBoxCountry.SelectedItem.ToString()).CountryID;

            if(mode == clsPeople.enMode.Update)
            {
                if (person.Save())
                {
                    MessageBox.Show("Person Updated Successfully!", "Note : ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    onPersonSaved?.Invoke(person.PersonID);
                }
                else
                {
                    MessageBox.Show("Error Happend While Updating Person.", "Note : ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                if (person.Save())
                {
                    labelID.Text = person.PersonID.ToString();
                    MessageBox.Show("Person Added Successfully with ID : " + person.PersonID, "Note :", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    labelMode.Text = "Update Person";
                    mode = clsPeople.enMode.Update;
                    onPersonSaved?.Invoke(person.PersonID);
                }
                else
                {
                    MessageBox.Show("Error Happend While Adding Person.", "Note : ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }

            
            

        }

        private void txtBEmailValidating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtBEmail.Text))
            {
                errorProvider1.SetError(txtBEmail, null);
            }
            else if (!clsValidation.ValidateEmail(txtBEmail.Text))
                errorProvider1.SetError(txtBEmail, "The email's format is not set properly!");
            else
                errorProvider1.SetError(txtBEmail, null);
        }

        private void formAddNewUpdatePerson_FormClosing(object sender, FormClosingEventArgs e)
        {
            callaMethod?.Invoke();
            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtBNationalNoValidating(object sender, CancelEventArgs e)
        {
            if(string.IsNullOrEmpty(txtBNationalNo.Text))
            {
                errorProvider1.SetError(txtBNationalNo, "This Field Can't Be Empty!");
            }
            else if(clsPeople.IsPersonExists(txtBNationalNo.Text))
            {
                errorProvider1.SetError(txtBNationalNo, "This National No is Already exists!");
            }
            else
            {
                errorProvider1.SetError(txtBNationalNo, null);
            }
        }
    }
}

