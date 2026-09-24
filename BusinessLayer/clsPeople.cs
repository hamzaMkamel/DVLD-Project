using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using System.Data;
using BusinessLayer;



public class clsPeople
{
    public enum enMode { addNew = 0, Update = 1 };
    enMode mode;
    public int PersonID { get; private set; }
    public string NationalNo { get; set; }
    public string FirstName { get; set; }
    public string SecondName { get; set; }
    public string ThirdName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public int Gendor { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public int NationalityCountryID {

        set;get;
    }
    private string _ImagePath;

    public clsCountries CountryInformation { get; set; }

    public string ImagePath
    {
        get { return _ImagePath; }
        set
        {
            if(string.IsNullOrEmpty(value) && File.Exists(_ImagePath))
            {
                
                File.Delete(_ImagePath);
                _ImagePath = value;
            }
            else if (!string.IsNullOrEmpty(value))
            {
                if(!string.IsNullOrEmpty(_ImagePath))
                {
                    File.Copy(value, _ImagePath, true);
                }
                else
                {
                    FileInfo fi = new FileInfo(value);
                    string ex = fi.Extension;
                    string appDirectory = AppDomain.CurrentDomain.BaseDirectory + "\\Photos";
                    string fullDirectory = Path.Combine(appDirectory, Guid.NewGuid().ToString() + ex );
                    File.Copy(value, fullDirectory);
                    _ImagePath = fullDirectory;
                }
            }
        }
    }
    
     
    
        



    public clsPeople()
    {
        PersonID = -1;
        NationalNo = "";
        FirstName = ""; SecondName = ""; ThirdName = ""; LastName = "";
        DateOfBirth = DateTime.Now.AddYears(-18);
        Gendor = 0;
        Address = "";
        Phone = "";
        Email = "";
        NationalityCountryID = 1;
        CountryInformation = null;
        ImagePath = "";
        mode = enMode.addNew;
    }

    clsPeople(int PersonID, string NationalNo, string FirstName, string SecondName, string ThirdName, string LastName,
                    DateTime DateOfBirth, int Gendor, string Address, string Phone, string Email, int NationalityCountryID, string ImagePath)
    {
        this.PersonID = PersonID;
        this.NationalNo = NationalNo;
        this.FirstName = FirstName; this.SecondName = SecondName; this.ThirdName = ThirdName; this.LastName = LastName;
        this.DateOfBirth = DateOfBirth;
        this.Gendor = Gendor;
        this.Address = Address;
        this.Phone = Phone;
        this.Email = Email;
        this.NationalityCountryID = NationalityCountryID;
        this._ImagePath = ImagePath;
        this.CountryInformation = clsCountries.Find(NationalityCountryID);
        this.mode = enMode.Update;
    }

    public bool Save()
    {
        if (mode == enMode.addNew)
        {
            this.PersonID = PersonAccessData.AddNewPerson(NationalNo, FirstName, SecondName, ThirdName, LastName
                , DateOfBirth, Gendor, Address, Phone, Email, NationalityCountryID, ImagePath);
            mode = enMode.Update;
            CountryInformation = clsCountries.Find(NationalityCountryID); // to update the current Country Properity
            return this.PersonID != -1;
        }
        else
        {
            bool result = PersonAccessData.UpdatePerson(PersonID, NationalNo, FirstName, SecondName, ThirdName, LastName
                , DateOfBirth, Gendor, Address, Phone, Email, NationalityCountryID, ImagePath);
            CountryInformation = clsCountries.Find(NationalityCountryID); // to update the current Country Properity
            return result;
        }
            
    }

    public static clsPeople Find(int PersonID)
    {
        
        string NationalNo = "";
        string FirstName = ""; string SecondName = ""; string ThirdName = ""; string LastName = "";
        DateTime DateOfBirth = new DateTime();
        int Gendor = 0;
        string Address = "";
        string Phone = "";
        string Email = "";
        int NationalityCountryID = 1;
        string ImagePath = "";

        if (PersonAccessData.getPersonInfoByID(PersonID, ref NationalNo, ref FirstName, ref SecondName, ref ThirdName
            , ref LastName, ref DateOfBirth, ref Gendor, ref Address, ref Phone, ref Email, ref NationalityCountryID, ref ImagePath))
            return new clsPeople(PersonID ,NationalNo, FirstName, SecondName, ThirdName, LastName
                , DateOfBirth, Gendor, Address, Phone, Email, NationalityCountryID, ImagePath);
        return null;
        
    }
    public static clsPeople Find(string NationalNo)
    {

        int PersonID = -1;
        string FirstName = ""; string SecondName = ""; string ThirdName = ""; string LastName = "";
        DateTime DateOfBirth = new DateTime();
        int Gendor = 0;
        string Address = "";
        string Phone = "";
        string Email = "";
        int NationalityCountryID = 1;
        string ImagePath = "";

        if (PersonAccessData.getPersonInfoByNationalNo(NationalNo, ref PersonID, ref FirstName, ref SecondName, ref ThirdName
            , ref LastName, ref DateOfBirth, ref Gendor, ref Address, ref Phone, ref Email, ref NationalityCountryID, ref ImagePath))
            return new clsPeople(PersonID, NationalNo, FirstName, SecondName, ThirdName, LastName
                , DateOfBirth, Gendor, Address, Phone, Email, NationalityCountryID, ImagePath);
        return null;

    }

    public static bool DeletePerson(int PersonID)
    {
        return PersonAccessData.DeletePerson(PersonID);
    }

    public static DataTable getAllPeople()
    {
        return PersonAccessData.getAllPeople();
    }

    public static bool IsPersonExists(string NationalNo)
    {
        return PersonAccessData.IsPersonExists(NationalNo);
    }

    public static bool IsPersonExists(int PersonID)
    {
        return PersonAccessData.IsPersonExists(PersonID);
    }

    public static string getPersonNameByID(int PersonID)
    {
        string personName = "";
        PersonAccessData.getPersonNameByID(PersonID, ref personName);
        return personName;
    }


}

