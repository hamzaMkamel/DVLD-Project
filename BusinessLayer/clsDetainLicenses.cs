using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class clsDetainLicenses
    {
        public int DetainID {set; get;}
        public int LicenseID {set; get;}
        public DateTime DetainDate {set; get;}
        public decimal FineFees {set; get;}
        public int CreatedByUserID {set; get;}
        public bool IsReleased {set; get;}
        public DateTime ReleaseDate {set; get;}
        public int ReleasedByUserID {set; get;}
        public int ReleaseApplicationID { set; get; }

        public clsApplications applicationInfo;
        public clsDetainLicenses()
        {
            this.DetainID = -1;
            this.LicenseID = -1;
            this.DetainDate = DateTime.Now;
            this.FineFees = -1;
            this.CreatedByUserID = -1;
            this.IsReleased = false;
            this.ReleaseDate = DateTime.Now;
            this.ReleasedByUserID = -1;
            this.ReleaseApplicationID = -1;
        }

        clsDetainLicenses(int DetainID,  int LicenseID,  DateTime DetainDate,  decimal FineFees
           , int CreatedByUserID, bool IsReleased, DateTime ReleaseDate,int ReleasedByUserID, int ReleaseApplicationID)
        {
            this.DetainID = DetainID;
            this.LicenseID = LicenseID;
            this.DetainDate = DetainDate;
            this.FineFees = FineFees;
            this.CreatedByUserID = CreatedByUserID;
            this.IsReleased = IsReleased;
            this.ReleaseDate = ReleaseDate;
            this.ReleasedByUserID = ReleasedByUserID;
            this.ReleaseApplicationID = ReleaseApplicationID;
            if (ReleaseApplicationID != -1)
                applicationInfo = clsApplications.FindBaseApplication(ReleaseApplicationID);
            
        }

        public bool addNewDetainLicense()
        {
            this.DetainID = DetainedLicensesDataAccess.addNewDetainedLicense(LicenseID, DetainDate, FineFees, CreatedByUserID);
            return DetainID != -1;
        }
        public static DataTable getAllDetainedLicenses()
        {
            return DetainedLicensesDataAccess.getAllDetainedLicenses();
        }

        public bool ReleaseDetainedLicense()
        {
            return DetainedLicensesDataAccess.ReleaseDetainedLicense(DetainID, DateTime.Now, ReleasedByUserID, ReleaseApplicationID);
        }

        public static int getDetainIDByLicenseID(int LicenseID)
        {
            return DetainedLicensesDataAccess.getDetainIDByLicenseID(LicenseID);
        }
        public static bool isLicenseDetained(int LicenseID)
        {
            return DetainedLicensesDataAccess.isLicenseDetained(LicenseID);
        }

        public static clsDetainLicenses Find(int DetainID)
        {
            
            int LicenseID = -1;
            DateTime DetainDate = DateTime.Now;
            decimal FineFees = -1;
            int CreatedByUserID = -1;
            bool IsReleased = false;
            DateTime ReleaseDate = DateTime.Now;
            int ReleasedByUserID = -1;
            int ReleaseApplicationID = -1;

            if (DetainedLicensesDataAccess.getDetainedLicense(DetainID, ref LicenseID, ref DetainDate, ref FineFees, ref CreatedByUserID
                , ref IsReleased, ref ReleaseDate, ref ReleasedByUserID, ref ReleaseApplicationID))
                return new clsDetainLicenses(DetainID, LicenseID, DetainDate, FineFees, CreatedByUserID
                , IsReleased, ReleaseDate, ReleasedByUserID, ReleaseApplicationID);

            return null;
        }
        public static clsDetainLicenses FindByLicenseID(int LicenseID)
        {
            int detainID = DetainedLicensesDataAccess.getDetainIDByLicenseID(LicenseID);
            if (detainID == -1) return null;
            return clsDetainLicenses.Find(detainID);
        }


    }
}
