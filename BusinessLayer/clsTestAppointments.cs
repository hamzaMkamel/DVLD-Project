using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace BusinessLayer
{
    public class clsTestAppointments
    {
        public enum enMode { AddNew, Update };
        public int TestAppointmentID { set; get; }
        public int TestTypeID { set; get; }
        public int LocalDrivingLicenseApplicationID { set; get; }
        public DateTime AppointmentDate { set; get; }
        public decimal PaidFees { set; get; }
        public int CreatedByUserID { set; get; }
        public bool IsLocked { set; get; }
        public int RetakeTestApplicationID { set; get; }
        public clsApplications RetakeTestApplicationInfo { set; get; }
        public enMode mode;

        public clsTestAppointments()
        {
            this.TestAppointmentID = -1;
            this.TestTypeID = -1;
            this.LocalDrivingLicenseApplicationID = -1;
            this.AppointmentDate = DateTime.Now;
            this.PaidFees = -1;
            this.CreatedByUserID = -1;
            this.IsLocked = false;
            this.RetakeTestApplicationID = -1;
            mode = enMode.AddNew;

        }

        clsTestAppointments(int TestAppointmentID, int TestTypeID, int LocalDrivingLicenseApplicationID, DateTime AppointmentDate,
                            decimal PaidFees, int CreatedByUserID, bool IsLocked, int RetakeTestApplicationID)
        {

            this.TestAppointmentID = TestAppointmentID;
            this.TestTypeID = TestTypeID;
            this.LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            this.AppointmentDate = AppointmentDate;
            this.PaidFees = PaidFees;
            this.CreatedByUserID = CreatedByUserID;
            this.IsLocked = IsLocked;
            this.RetakeTestApplicationID = RetakeTestApplicationID;
            if (this.RetakeTestApplicationID != -1)
                RetakeTestApplicationInfo = clsApplications.FindBaseApplication(RetakeTestApplicationID);
            mode = enMode.Update;
        }

        public bool Save()
        {
            if (mode == enMode.AddNew)
            {



                if (RetakeTestApplicationInfo != null)
                {
                    RetakeTestApplicationInfo.Save();
                RetakeTestApplicationID = RetakeTestApplicationInfo.ApplicationID;
                }
                this.TestAppointmentID = TestAppointmentsDataAccess.addNewAppointment(TestTypeID, LocalDrivingLicenseApplicationID,
                    AppointmentDate, PaidFees, CreatedByUserID, RetakeTestApplicationID);
                mode = enMode.Update;
                return this.TestAppointmentID != -1;
            }
            else
            {
                return TestAppointmentsDataAccess.updateAppointmentDate(TestAppointmentID, AppointmentDate);
            }
        }



        public static clsTestAppointments Find(int TestAppointmentID)
        {
            int TestTypeID = -1, LocalDrivingLicenseApplicationID = -1;
            DateTime AppointmentDate = DateTime.Now;
            decimal PaidFees = -1;
            int CreatedByUserID = -1;
            bool IsLocked = false;
            int RetakeTestApplicationID = -1;
            if (TestAppointmentsDataAccess.getTestAppointmentInfo(TestAppointmentID, ref TestTypeID, ref LocalDrivingLicenseApplicationID
                , ref AppointmentDate, ref PaidFees, ref CreatedByUserID, ref IsLocked, ref RetakeTestApplicationID))
            {

                return new clsTestAppointments(TestAppointmentID, TestTypeID, LocalDrivingLicenseApplicationID, AppointmentDate, PaidFees, CreatedByUserID, IsLocked
                    , RetakeTestApplicationID);
            }
            else return null;
        }

        public static void setAppointmentLocked(int TestAppointmentID)
        {
            TestAppointmentsDataAccess.setAppointmentLocked(TestAppointmentID);
        }

        public static DataTable getAssociatedTestAppointmentset(int LocalDrivingLicenseApplicationID, int TestType)
        {
            return TestAppointmentsDataAccess.getAssociatedTestAppointments(LocalDrivingLicenseApplicationID, TestType);
        }
        public static bool isThereNotLockedAppointment(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            return TestAppointmentsDataAccess.isThereNotLockedAppointment(LocalDrivingLicenseApplicationID, TestTypeID);
        }

        public static bool isPersonTakeTestBefore(int LocalDrivingLicenseApplicationID , int TestTypeID)
        {
            return TestAppointmentsDataAccess.isPersonTakeTestBefore(LocalDrivingLicenseApplicationID, TestTypeID);
        }

        public static bool isTestAppointmentTaken(int TestAppointmentID)
        {
            return TestAppointmentsDataAccess.isTestAppointmentTaken(TestAppointmentID);
        }

        

    }
}

     

