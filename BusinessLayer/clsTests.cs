using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class clsTests
    {
        public int TestID { set; get; }
        public int TestAppointmentID { set; get; }
        public bool TestResult { set; get; }
        public string Notes { set; get; }
        public int CreatedByUserID { set; get;}

        public clsTests()
        {
            this.TestID = -1;
            this.TestAppointmentID = -1;
            this.TestResult = false;
            this.Notes = "";
            this.CreatedByUserID = -1;

        }

        clsTests (int TestID,int TestAppointmentID, bool TestResult, string Notes, int CreatedByUserID)
        {
            this.TestID = TestID;
            this.TestAppointmentID = TestAppointmentID;
            this.TestResult = TestResult;
            this.Notes = Notes;
            this.CreatedByUserID = CreatedByUserID;
        }

        public bool Save()
        {
            this.TestID = TestsDataAccess.setNewTest(TestAppointmentID, TestResult, Notes, CreatedByUserID);
            clsTestAppointments.setAppointmentLocked(TestAppointmentID); // this is critical to ensure that the user can't take the test of this appointment again
            return TestID != -1;
        }

        public static bool isApplicantPassedThisTestBefore(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            return TestsDataAccess.isApplicantPassedThisTestBefore(LocalDrivingLicenseApplicationID, TestTypeID);
        }

        public static bool isApplicantFailedThisTestBefore(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            return TestsDataAccess.isApplicantFailedThisTestBefore(LocalDrivingLicenseApplicationID, TestTypeID);
        }
        public static int getPassedTestsByLocalDrivingLicenseApplicationID(int LocalDrivingLicenseApplicationID)
        {
            return TestsDataAccess.getPassedTestsByLocalDrivingLicenseApplicationID(LocalDrivingLicenseApplicationID);
        }

        public static clsTests FindByAppointmentID(int AppointmentID)
        {
            int TestID = -1, TestAppointmentID = -1, CreatedByUserID = -1;
            bool TestResult = false ;
            string Notes = "";
            TestsDataAccess.getTestIDByAppointmentID(AppointmentID , ref TestID);
            if (TestID != -1) 
            {
                TestsDataAccess.getTestInfoByID(TestID, ref TestAppointmentID, ref TestResult, ref Notes, ref CreatedByUserID);
                return new clsTests(TestID, TestAppointmentID, TestResult, Notes, CreatedByUserID);
            }

            return null;

        }

    }
}
