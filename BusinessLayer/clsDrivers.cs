using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class clsDrivers
    {
        public int DriverID { set; get; }
        public int PersonID { set; get; }
        public int CreatedByUserID { set; get; }
        public DateTime CreatedDate { set; get; }

        public clsDrivers()
        {
            DriverID = -1;
            PersonID = -1;
            CreatedByUserID = -1;
            CreatedDate = DateTime.Now;
        }
        clsDrivers(int DriverID , int PersonID , int CreatedByUserID , DateTime CreatedDate)
        {
            this.DriverID = DriverID;
            this.PersonID = PersonID;
            this.CreatedByUserID = CreatedByUserID;
            this.CreatedDate = CreatedDate;
        }

        public static clsDrivers Find(int DriverID)
        {
            
            int PersonID = -1;
            int CreatedByUserID = -1;
            DateTime CreatedDate = DateTime.Now;

            if (DriversDataAccess.getDriverInfoByDriverID(DriverID, ref PersonID, ref CreatedByUserID, ref CreatedDate))
            {
                return new clsDrivers(DriverID, PersonID, CreatedByUserID, CreatedDate);
            }
            else return null;
        }
        public static clsDrivers FindByPersonID(int PersonID)
        {
            int driverID = -1;
            DriversDataAccess.getDriverIDByPersonID(PersonID , ref driverID);
            return (driverID != -1) ? clsDrivers.Find(driverID) : null;
        }
        public bool SaveNewDriver()
        {
            this.DriverID = DriversDataAccess.AddNewDriver(PersonID, CreatedByUserID, CreatedDate);
            return DriverID != -1;
        }

        public static DataTable getAllDriversTable()
        {
            return DriversDataAccess.getAllDriversTable();
        }

        public static bool isPersonADriver(int PersonID)
        {
            return DriversDataAccess.isPersonADriver(PersonID);
        }

        public static int getDriverIDByPersonID(int PersonID)
        {
            int driverID = -1;
            DriversDataAccess.getDriverIDByPersonID(PersonID, ref driverID);
            return driverID;
        }

        public static clsDrivers createOrGetDriver(int PersonID , int CurrentUserID)
        {
            clsDrivers driver = clsDrivers.FindByPersonID(PersonID);
            if (driver != null)
            {
                return driver;
            }
            driver = new clsDrivers();
            driver.PersonID = PersonID;
            driver.CreatedByUserID = CurrentUserID;
            driver.CreatedDate = DateTime.Now;

            if (!driver.SaveNewDriver())
            {
                

                return null;
            }
            return driver;
        }


    }
}
