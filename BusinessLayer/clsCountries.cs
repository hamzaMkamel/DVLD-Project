using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public  class clsCountries
    {

        public int CountryID { set; get; }
        public string CountryName { set; get; }

        public clsCountries() { }
        private clsCountries(int ID , string CountryName)
        {
            CountryID = ID;
            this.CountryName = CountryName;
        }

        public static DataTable getAllCountries()
        {
            return DataAccessLayer.CountriesAccessData.getAllCountries();
        }
        public static clsCountries Find(string CountryName)
        {
            int ID = -1;
            if (CountriesAccessData.getCountryInfoByName(CountryName, ref ID))
            {
                return new clsCountries(ID, CountryName);
            }
            else return null;
        }
        public static clsCountries Find(int CountryID)
        {
            string CountryName = "";
            if (CountriesAccessData.GetCountryInfoByID(CountryID , ref CountryName))
            {
                return new clsCountries(CountryID, CountryName);
            }
            else return null;
        }
    }
}
