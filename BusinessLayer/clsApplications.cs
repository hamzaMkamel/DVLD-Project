using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class clsApplications
    {
        enum enMode { AddNew , Update};
        enMode mode;
        public enum enApplicationType
        {
            NewDrivingLicense = 1, RenewDrivingLicense = 2, ReplaceLostDrivingLicense = 3,
            ReplaceDamagedDrivingLicense = 4, ReleaseDetainedDrivingLicsense = 5, NewInternationalLicense = 6, RetakeTest = 7
        };
        public int ApplicationID {private set; get;}
        public int ApplicantPersonID { set; get; }
        public DateTime ApplicationDate { set; get; }
        
        public int ApplicationTypeID { set; get; }
        public byte ApplicationStatus { set; get; }
        public DateTime LastStatusDate { set; get; }
        public decimal PaidFees { set; get; }
        public int CreatedByUserID { set; get; }
        public clsUsers UserInfo;
        public clsPeople personInfo;



        public clsApplications()
        {
            this.ApplicationID = -1;
            this.ApplicantPersonID = -1;
            this.ApplicationDate = DateTime.Now;
            this.ApplicationTypeID = -1;
            this.ApplicationStatus = 0;
            this.LastStatusDate = DateTime.Now;
            this.PaidFees = -1;
            this.CreatedByUserID = -1;
            mode = enMode.AddNew;

        }

        private clsApplications(int ApplicationID , int ApplicantPersonID , DateTime ApplicationDate , int ApplicationTypeID , byte ApplicationStatus
            , DateTime LastStatusDate , decimal PaidFees , int CreatedByUserID)
        {
            this.ApplicationID = ApplicationID;
            this.ApplicantPersonID = ApplicantPersonID;
            this.ApplicationDate = ApplicationDate;
            this.ApplicationTypeID = ApplicationTypeID;
            this.ApplicationStatus = ApplicationStatus;
            this.LastStatusDate = LastStatusDate;
            this.PaidFees = PaidFees;
            this.CreatedByUserID = CreatedByUserID;
            this.UserInfo = clsUsers.Find(this.CreatedByUserID);
            this.personInfo = clsPeople.Find(this.ApplicantPersonID);
            mode = enMode.Update;

        }

        public bool Save()
        {
            if (mode == enMode.AddNew)
            {
                this.ApplicationID = ApplicationsDataAccess.addNewApplication(ApplicantPersonID, ApplicationDate, ApplicationTypeID, ApplicationStatus, LastStatusDate,
                    PaidFees, CreatedByUserID);
                return this.ApplicationID != -1;
            }
            else
            {
                return ApplicationsDataAccess.UpdateApplication(this.ApplicationID, ApplicantPersonID, ApplicationDate, ApplicationTypeID, ApplicationStatus, LastStatusDate,
                    PaidFees, CreatedByUserID);
            }

            
        }
        public static clsApplications FindBaseApplication(int ApplicationID)
        {
            
            int ApplicantPersonID = -1;
            
            DateTime ApplicationDate = DateTime.Now;
            int ApplicationTypeID = -1;
            byte ApplicationStatus = 0;
            DateTime LastStatusDate = DateTime.Now;
            decimal PaidFees = -1;
            int CreatedByUserID = -1;
            if(ApplicationsDataAccess.getApplicationInfoByID(ApplicationID ,ref ApplicantPersonID , ref ApplicationDate , ref ApplicationTypeID ,ref ApplicationStatus
                ,ref LastStatusDate , ref PaidFees , ref CreatedByUserID))
            {
                return new clsApplications(ApplicationID, ApplicantPersonID, ApplicationDate, ApplicationTypeID, ApplicationStatus
                , LastStatusDate, PaidFees, CreatedByUserID);
            }
            return null;
        }

        public static bool isApplicationStatusCancelOrFinished(int appID)
        {
            return DataAccessLayer.ApplicationsDataAccess.isApplicationStatusCanceledOrFinished(appID);
        }

        public static bool CancelApplication(int ApplicationID)
        {
            return ApplicationsDataAccess.CancelApplication(ApplicationID);
        }

        public static bool DeleteApplication(int ApplicationID)
        {
            return ApplicationsDataAccess.DeleteApplication(ApplicationID);
        }
        public static bool IsApplicationCompleted(int ApplicationID)
        {
            return ApplicationsDataAccess.isApplicationCompleted(ApplicationID);
        }

        public static bool IsApplicationCanceled(int ApplicationID)
        {
            return ApplicationsDataAccess.isApplicationCanceled(ApplicationID);
        }

        public static bool setCompletedApplication(int ApplicationID)
        {
            return ApplicationsDataAccess.setCompletedApplication(ApplicationID);
        }

        public static bool DoesPersonHaveActiveApplication(int PersonID, int ApplicationTypeID)
        {
            return ApplicationsDataAccess.DoesPersonHaveActiveApplication(PersonID, ApplicationTypeID);
        }
        public static int GetActiveApplicationID(int PersonID, clsApplications.enApplicationType ApplicationTypeID)
        {
            return ApplicationsDataAccess.GetActiveApplicationID(PersonID, (int)ApplicationTypeID);
        }
    }
}
