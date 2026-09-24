using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class clsLicenses
    {
        public int LicenseID { set; get; }
        public int ApplicationID { set; get; }
        public int DriverID { set; get; }
        public int LicenseClass { set; get; }
        public DateTime IssueDate { set; get; }

        public DateTime ExpirationDate { set; get; }

        public string Notes { set; get; }
        public enum enIssueReason { FirstTime = 1, Renew = 2,  LostReplacement = 3 ,DamagedReplacement = 4 };
        public decimal PaidFees { set; get; }
        public bool IsActive { set; get; }
        public int IssueReason { set; get; }
        public int CreatedByUserID { set; get; }
        public clsApplications applicationInfo;
        public clsDetainLicenses detainedLicenseInfo;
        public clsLicenses()
        {
            LicenseID = -1;
            ApplicationID = -1;
            DriverID = -1;
            LicenseClass = -1;
            IssueDate = DateTime.Now;
            ExpirationDate = DateTime.Now;
            Notes = "";
            PaidFees = -1;
            IsActive = false;
            IssueReason = -1;
            CreatedByUserID = -1;
        }

        clsLicenses(int LicenseID, int ApplicationID, int DriverID, int LicenseClass,
            DateTime IssueDate, DateTime ExpirationDate, string Notes, decimal PaidFees, bool IsActive
            , int IssueReason, int CreatedByUserID)
        {
            this.LicenseID = LicenseID;
            this.ApplicationID = ApplicationID;
            this.DriverID = DriverID;
            this.LicenseClass = LicenseClass;
            this.IssueDate = IssueDate;
            this.ExpirationDate = ExpirationDate;
            this.Notes = Notes;
            this.PaidFees = PaidFees;
            this.IsActive = IsActive;
            this.IssueReason = IssueReason;
            this.CreatedByUserID = CreatedByUserID;
            applicationInfo = clsApplications.FindBaseApplication(ApplicationID);

        }

        public static clsLicenses FindByApplicationID(int ApplicationID)
        {
            int licenseID = -1;
            if (LicensesDataAccess.getLicenseIDByApplicationID(ApplicationID, ref licenseID))
            {
                return clsLicenses.Find(licenseID);
            }
            return null;
        }

        public static int getLicenseIDByApplicationID(int ApplicationID)
        {
            int licenseID = -1;
            LicensesDataAccess.getLicenseIDByApplicationID(ApplicationID, ref licenseID);
            return licenseID;
        }


        public static clsLicenses Find(int LicenseID)
        {
            int ApplicationID = -1;
            int DriverID = -1;
            int LicenseClass = -1;
            DateTime IssueDate = DateTime.Now;
            DateTime ExpirationDate = DateTime.Now;
            string Notes = "";
            decimal PaidFees = -1;
            bool IsActive = false;
            int IssueReason = -1;
            int CreatedByUserID = -1;

            if (LicensesDataAccess.getLicenseInfoByLicenseID(LicenseID, ref ApplicationID, ref DriverID, ref LicenseClass, ref IssueDate
                , ref ExpirationDate, ref Notes, ref PaidFees, ref IsActive, ref IssueReason, ref CreatedByUserID))
            {
                return new clsLicenses(LicenseID, ApplicationID, DriverID, LicenseClass, IssueDate, ExpirationDate, Notes, PaidFees, IsActive, IssueReason, CreatedByUserID);
            }
            else return null;
        }

        public bool SaveNewLicense()
        {
            this.LicenseID = LicensesDataAccess.AddNewLicense(ApplicationID, DriverID, LicenseClass, IssueDate, ExpirationDate, Notes, PaidFees, IssueReason, CreatedByUserID);
            return LicenseID != -1;
        }

        public static DataTable getAllLicensesToSpecificDriver(int DriverID)
        {
            return LicensesDataAccess.getAllLicensesToSpecificDriver(DriverID);
        }

        public bool DeactivateCurrentLicense()
        {
            return (LicensesDataAccess.DeactivateLicense(this.LicenseID));
        }
        public Boolean IsLicenseExpired()
        {

            return (this.ExpirationDate < DateTime.Now);

        }

        public static bool isLicenseValidToBeAnInternationalLicense(int LicenseID)
        {
            return LicensesDataAccess.isLicenseValidToBeAnInternationalLicense(LicenseID);
        }

        public clsLicenses RenewLicense(string Notes , int CurrentUserID)
        {
           return PerformLicenseApplications(Notes, CurrentUserID, enIssueReason.Renew);
        }

        public clsLicenses ReplaceLicenseForLost(int CurrentUserID)
        {
            return PerformLicenseApplications(this.Notes, CurrentUserID, enIssueReason.LostReplacement);
        }

        public clsLicenses ReplaceLicenseForDamaged(int CurrentUserID)
        {
            return PerformLicenseApplications(this.Notes, CurrentUserID, enIssueReason.DamagedReplacement);
        }

        private clsLicenses PerformLicenseApplications(string Notes , int CurrentUserID , enIssueReason issueReason)
        {
            clsLicenses newLicense = new clsLicenses();
            newLicense.applicationInfo = createApplication(CurrentUserID , (int)issueReason);
            if(newLicense.applicationInfo != null)
            {
                newLicense.ApplicationID = newLicense.applicationInfo.ApplicationID;
                newLicense.CreatedByUserID = CurrentUserID;
                newLicense.DriverID = this.DriverID;
                newLicense.IsActive = true;
                newLicense.IssueDate = DateTime.Now;
                newLicense.IssueReason = (int)issueReason;
                newLicense.LicenseClass = this.LicenseClass;
                newLicense.Notes = Notes;
                newLicense.PaidFees = clsLicenseClasses.Find(this.LicenseClass).ClassFees;
                newLicense.ExpirationDate = DateTime.Now.AddYears(clsLicenseClasses.Find(this.LicenseClass).DefaultValidityLength);

                if(newLicense.SaveNewLicense())
                {
                    DeactivateCurrentLicense();
                    return newLicense;
                }
                clsApplications.DeleteApplication(newLicense.applicationInfo.ApplicationID); // if the save of license faild its crucial to delete the saved application
                return null;
            }
            return null;
        }

        private clsApplications createApplication(int CurrentUserID , int applicationTypeID)
        {
            clsApplications application = new clsApplications();
            application.ApplicantPersonID = this.applicationInfo.ApplicantPersonID;
            application.ApplicationDate = DateTime.Now;
            application.ApplicationStatus = 3; // When saving application the license is newed so the status is completed directrly
            application.ApplicationTypeID = applicationTypeID; // from table its renew license
            application.CreatedByUserID = CurrentUserID;
            application.PaidFees = clsApplicationTypes.getApplicationTypeByID(applicationTypeID).ApplicationTypeFees;

            if (application.Save())
            {
                return application;
            }
            return null;
        }
    
        public bool Detain(decimal FineFees , int CreatedByUserID)
        {
            detainedLicenseInfo = new clsDetainLicenses();
            detainedLicenseInfo.LicenseID = this.LicenseID;
            detainedLicenseInfo.DetainDate = DateTime.Now;
            detainedLicenseInfo.FineFees = FineFees;
            detainedLicenseInfo.CreatedByUserID = CreatedByUserID;
            if (detainedLicenseInfo.addNewDetainLicense())
                return true;
            else return false;
        }

        public bool Release(int ReleasedByUserID)
        {
            clsApplications releaseApplication = createApplication(ReleasedByUserID, 5);
            
            if(releaseApplication != null)
            {
                detainedLicenseInfo = clsDetainLicenses.FindByLicenseID(LicenseID);
                if(detainedLicenseInfo != null)
                {
                    detainedLicenseInfo.ReleaseDate = DateTime.Now;
                    detainedLicenseInfo.ReleasedByUserID = ReleasedByUserID;
                    detainedLicenseInfo.ReleaseApplicationID = releaseApplication.ApplicationID;
                    if(detainedLicenseInfo.ReleaseDetainedLicense())
                    {
                        return true;
                    }
                    return false;
                }
                return false;
            }
            return false;
        }
    }
}
