using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class clsInternationalLicenses
    {
        public int InternationalLicenseID { set; get; }
        public int ApplicationID { set; get; }
        public int DriverID { set; get; }
        public int IssuedUsingLocalLicenseID { set; get; }
        public DateTime IssueDate { set; get; }
        public DateTime ExpirationDate { set; get; }
        public bool IsActive { set; get; }
        public int CreatedByUserID { set; get; }
        public clsDrivers DriverInfo;
        public clsApplications applicationInfo;
        public clsLicenses localLicense;

        public clsInternationalLicenses()
        {
            this.InternationalLicenseID = -1;
            this.ApplicationID = -1;
            this.DriverID = -1;
            this.IssuedUsingLocalLicenseID = -1;
            this.IssueDate = DateTime.Now;
            this.ExpirationDate = DateTime.Now;
            this.IsActive = false;
            this.CreatedByUserID = -1;
        }

        clsInternationalLicenses(int InternationalLicenseID,int ApplicationID, int DriverID, int IssuedUsingLocalLicenseID, DateTime IssueDate, DateTime ExpirationDate,
            bool IsActive, int CreatedByUserID)
        {
            this.InternationalLicenseID = InternationalLicenseID;
            this.ApplicationID = ApplicationID;
            this.DriverID = DriverID;
            this.IssuedUsingLocalLicenseID = IssuedUsingLocalLicenseID;
            this.IssueDate = IssueDate;
            this.ExpirationDate = ExpirationDate;
            this.IsActive = IsActive;
            this.CreatedByUserID = CreatedByUserID;
            this.DriverInfo = clsDrivers.Find(DriverID);
            this.applicationInfo = clsApplications.FindBaseApplication(ApplicationID);
        }

        private bool SaveNewInternationalLicense()
        {

            this.InternationalLicenseID = InternationalLicensesDataAccess.AddNewLicense(ApplicationID, DriverID, IssuedUsingLocalLicenseID
                , IssueDate, ExpirationDate, IsActive, CreatedByUserID);
            
            this.DriverInfo = clsDrivers.Find(DriverID);
            if (InternationalLicenseID != -1)
            {
                applicationInfo.ApplicationStatus = 3; // setting application to completed
                applicationInfo.LastStatusDate = DateTime.Now;
                applicationInfo.Save();
            }
            return this.InternationalLicenseID != -1;
        }

        public static clsInternationalLicenses Find(int InternationalLicenseID)
        {
            int ApplicationID = -1;
            int DriverID = -1;
            int IssuedUsingLocalLicenseID = -1;
            DateTime IssueDate = DateTime.Now;
            DateTime ExpirationDate = DateTime.Now;
            bool IsActive = false;
            int CreatedByUserID = -1;
            

            if (InternationalLicensesDataAccess.getLicenseInfoByLicenseID(InternationalLicenseID, ref ApplicationID, ref DriverID, ref IssuedUsingLocalLicenseID
                , ref IssueDate, ref ExpirationDate, ref IsActive, ref CreatedByUserID))
                return new clsInternationalLicenses(InternationalLicenseID, ApplicationID, DriverID, IssuedUsingLocalLicenseID
                , IssueDate, ExpirationDate, IsActive, CreatedByUserID);
            return null;
        }

        public static DataTable getAllLicensesToSpecificDriver(int DriverID)
        {
            return InternationalLicensesDataAccess.getAllLicensesToSpecificDriver(DriverID);
        }
        public static DataTable getAllInternationalLicenses()
        {
            return InternationalLicensesDataAccess.getAllInternationalLicenses();
        }

        public static bool setInternationalLicenseActiveStatus(int InternationalLicenseID, int IsActive)
        {
            return InternationalLicensesDataAccess.setInternationalLicenseActiveStatus(InternationalLicenseID, IsActive);
        }

        public static bool getInternationalLicenseStatus(int InternationalLicenseID)
        {
            return InternationalLicensesDataAccess.getInternationalLicenseStatus(InternationalLicenseID);
        }
        public static bool DeactivateInternationalLicense(int InternationalLicenseID)
        {
            return InternationalLicensesDataAccess.DeactivateInternationalLicense(InternationalLicenseID);
        }

        public static bool isLicenseLinkedToInternationalLicense(int LocalLicenseID)
        {
            return InternationalLicensesDataAccess.isLicenseLinkedToInternationalLicense(LocalLicenseID);
        }

        public bool IssueInternationalLicense(int LocalLicenseID , int CurrentUserID)
        {
            localLicense = clsLicenses.Find(LocalLicenseID);
            if(localLicense != null)
            {
                applicationInfo = createNewInternationalLicenseApplicaiton(CurrentUserID);
                if(applicationInfo != null)
                {
                    this.ApplicationID = applicationInfo.ApplicationID;
                    this.CreatedByUserID = CurrentUserID;
                    this.DriverID = localLicense.DriverID;
                    this.IssuedUsingLocalLicenseID = localLicense.LicenseID;
                    this.IssueDate = DateTime.Now;
                    this.ExpirationDate = DateTime.Now.AddYears(clsLicenseClasses.Find(localLicense.LicenseClass).DefaultValidityLength);
                    this.IsActive = true;
                    if(SaveNewInternationalLicense())
                    {
                        return true;
                    }
                    return false ;


                }
            }
            return false ;
        }

        private clsApplications createNewInternationalLicenseApplicaiton(int CurrentUserID)
        {
            clsApplications application = new clsApplications();
            application.ApplicantPersonID = localLicense.applicationInfo.ApplicantPersonID;
            application.ApplicationDate = DateTime.Now;
            application.ApplicationStatus = 1;
            application.ApplicationTypeID = 6;
            application.CreatedByUserID = CurrentUserID;
            application.PaidFees = clsApplicationTypes.getApplicationTypeByID(6).ApplicationTypeFees;

            if(application.Save())
            {
                return application;
            }
            return null;
            
        }
    }
}
