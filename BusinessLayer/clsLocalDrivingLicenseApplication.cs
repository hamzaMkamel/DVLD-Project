using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class clsLocalDrivingLicenseApplication
    {
        public int LocalDrivingLicenseApplicationID { set; get; }
        public int ApplicationID { set; get; }
        public int LicenseClassID { set; get; }
        public clsApplications applicationInfo;
        public enum enMode  {AddNew , Update};
        clsLicenseClasses licenseClassInfo;
        enMode mode;


        public clsLocalDrivingLicenseApplication()
        {
            LocalDrivingLicenseApplicationID = -1;
            ApplicationID = -1;
            LicenseClassID = -1;
            applicationInfo = new clsApplications();
            mode = enMode.AddNew;
        }

        private clsLocalDrivingLicenseApplication(int localdrivinglicenseID , int applicationID , int licenseClassId , clsApplications application)
        {
            this.LocalDrivingLicenseApplicationID = localdrivinglicenseID;
            this.ApplicationID = applicationID;
            this.LicenseClassID = licenseClassId;
            this.applicationInfo = application;
            mode = enMode.Update;

            this.licenseClassInfo = clsLicenseClasses.Find(LicenseClassID);
        }

        public static clsLocalDrivingLicenseApplication Find(int LocalDrivingLicenseApplicaitonID)
        {
            int applicationID = -1 ,  licenseClassID = -1;
            clsApplications application;
            if(LocalLicenseApplicationDataAccess.getLocalLicenseApplicationInfoByID(LocalDrivingLicenseApplicaitonID , ref applicationID ,ref licenseClassID))
            {
                application = clsApplications.FindBaseApplication(applicationID);
                return new clsLocalDrivingLicenseApplication(LocalDrivingLicenseApplicaitonID, applicationID, licenseClassID , application);
            }
            else return null;
        }

        public static DataTable getAllLocalDrivingLicenseApplications()
        {
            return LocalLicenseApplicationDataAccess.getAllLocalDrivingLicenseApplications();
        }

        public bool Save()
        {
            if (mode == enMode.Update)
            {
                return DataAccessLayer.LocalLicenseApplicationDataAccess.updateLocalDrivingLicenseApplicationClass(this.LocalDrivingLicenseApplicationID
                    , this.LicenseClassID);
            }
            else
            {
                bool isDone = applicationInfo.Save();
                this.ApplicationID = applicationInfo.ApplicationID;
                mode = enMode.Update;
                this.LocalDrivingLicenseApplicationID = LocalLicenseApplicationDataAccess.addApplicationToLocalDrivingLicenseApplication(this.ApplicationID, this.LicenseClassID);
                return isDone && LocalDrivingLicenseApplicationID != -1;
            }
               
        }

        public static bool isThisPeronAppliedForThisClass(int PersonID , string className)
        {
            return LocalLicenseApplicationDataAccess.isPersonAppliedForThisClass(PersonID, className);
        }

        public static bool CancelLocalApplication(int LocalDrivingAppID)
        {

            clsLocalDrivingLicenseApplication app = clsLocalDrivingLicenseApplication.Find(LocalDrivingAppID);
            
            if (app != null)
            {
                return clsApplications.CancelApplication(app.applicationInfo.ApplicationID);
            }
            return false;
        }
        public static bool DeleteLocalDrivingLicenseApplication(int LocalDrivingAppID)
        {
            clsLocalDrivingLicenseApplication app = clsLocalDrivingLicenseApplication.Find(LocalDrivingAppID);
            bool isdone = false;
            if(app!= null)
            {
                isdone = DataAccessLayer.LocalLicenseApplicationDataAccess.DeleteLocalApplication(LocalDrivingAppID);
            }

            return isdone && clsApplications.DeleteApplication(app.applicationInfo.ApplicationID);
        }

        public byte TotalTrialsPerTest(int TestTypeID)
        {
            return LocalLicenseApplicationDataAccess.TotalTrialsPerTest(this.LocalDrivingLicenseApplicationID, TestTypeID);
        }

        public int IssueLicenseForFirstTime(string Notes,int CurrentUserID)
        {
            clsDrivers driver = clsDrivers.createOrGetDriver(applicationInfo.ApplicantPersonID , CurrentUserID);
            if (driver == null) return -1 ; //  if an error happened while saving driver no need to initialize an license
            clsLicenses license = new clsLicenses();
            license.ApplicationID = this.ApplicationID;
            license.DriverID = driver.DriverID;
            license.CreatedByUserID = CurrentUserID;
            license.LicenseClass = this.LicenseClassID;
            license.IssueDate = DateTime.Now;

            license.ExpirationDate = DateTime.Now.AddYears(licenseClassInfo.DefaultValidityLength);
            license.Notes = Notes;
            license.IssueReason = 1; // which will be handled later on to be dynamic
            license.PaidFees = licenseClassInfo.ClassFees;

            if (license.SaveNewLicense())
            {

                clsApplications.setCompletedApplication(applicationInfo.ApplicationID);
                return license.LicenseID;
            }
            else
                return -1;
                
        }
    }
}
