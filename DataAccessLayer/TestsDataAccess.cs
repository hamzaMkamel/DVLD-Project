using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DataAccessLayer
{
    public static class TestsDataAccess
    {

        /*There is 3 Setps to let the Applicant to add an appointment
 * 1 - he is not passed in this test before
 * 2-there is no appointment is not locked (that 's mean he already has a open appointment)
 * 3- if he is failed before we should take care of this situation and apply a new application (retake test)
 */
        //1-
        public static bool isApplicantPassedThisTestBefore(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"Select Top 1 Found = 1 From Tests Inner join TestAppointments On TestAppointments.TestAppointmentID = Tests.TestAppointmentID
                inner join LocalDrivingLicenseApplications on LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID
                = TestAppointments.LocalDrivingLicenseApplicationID
                Where Tests.TestResult = 1 and TestAppointments.TestTypeID = @TestTypeID and LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID
                Order By TestAppointments.AppointmentDate Desc;";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
            object result = 0;
            try
            {
                connection.Open();

                result = command.ExecuteScalar();
                if (result == null) result = 0;



            }
            catch (Exception e)
            {
                DataAccessSettings.SaveLogToFileLog(e.Message);
            }
            finally
            {
                connection.Close();
            }
            return ((int)result == 1);
        }
        //3-
        public static bool isApplicantFailedThisTestBefore(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"Select Top 1 Found = 1 From Tests Inner join TestAppointments On TestAppointments.TestAppointmentID = Tests.TestAppointmentID
                inner join LocalDrivingLicenseApplications on LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID
                = TestAppointments.LocalDrivingLicenseApplicationID
                Where Tests.TestResult = 0 and TestAppointments.TestTypeID = @TestTypeID and LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID
                Order By TestAppointments.AppointmentDate Desc;";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
            object result = 0;
            try
            {
                connection.Open();

                result = command.ExecuteScalar();
                if (result == null) result = 0;



            }
            catch (Exception e)
            {
                DataAccessSettings.SaveLogToFileLog(e.Message);
            }
            finally
            {
                connection.Close();
            }
            return ((int)result == 1);
        }

        public static int setNewTest(int TestAppointmentID , bool TestResult , string Notes , int CreatedByUserID)
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"INSERT INTO Tests Values
                         (@TestAppointmentID , @TestResult , @Notes , @CreatedByUserID);
                        Select Scope_IDENTITY();";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
            command.Parameters.AddWithValue("@TestResult", TestResult);
            if(string.IsNullOrEmpty(Notes))
            {
                command.Parameters.AddWithValue("@Notes", System.DBNull.Value);
            }
            else
                command.Parameters.AddWithValue("@Notes", Notes);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);


            int newTestID = -1;
            try
            {
                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int ID))
                {
                    newTestID = ID;
                }
            }
            catch (Exception e)
            {
                DataAccessSettings.SaveLogToFileLog(e.Message);
            }
            finally
            {
                connection.Close();
            }

            return newTestID;
        }

        public static bool getTestInfoByID(int TestID , ref int TestAppointmentID,ref  bool TestResult,ref  string Notes, ref int CreatedByUserID)
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"Select * From Tests Where TestID = @TestID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestID", TestID);
            bool isfound = false;
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    TestAppointmentID = (int)reader["TestAppointmentID"];
                    TestResult = (bool)reader["TestResult"];
                    
                    if (reader["Notes"] == System.DBNull.Value)
                        Notes = "";
                    else Notes = (string)reader["Notes"];
                    CreatedByUserID = (int)reader["CreatedByUserID"];
                    

                    isfound = true;
                }
                else
                {
                    isfound = false;
                }
                reader.Close();
            }
            catch (Exception e)
            {
                DataAccessSettings.SaveLogToFileLog(e.Message);
            }
            finally
            {
                connection.Close();
            }
            return isfound;
        }

        public static int getPassedTestsByLocalDrivingLicenseApplicationID(int LocalDrivingLicenseApplicationID)
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"Select 
                PassedTests = count (Tests.TestResult) From Tests inner join TestAppointments  ON Tests.TestAppointmentID = TestAppointments.TestAppointmentID
                inner join LocalDrivingLicenseApplications ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID
                and TestResult = 1  Where LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            
            


            int passedTests = -1;
            try
            {
                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int num))
                {
                    passedTests = num;
                }
            }
            catch (Exception e)
            {
                DataAccessSettings.SaveLogToFileLog(e.Message);
            }
            finally
            {
                connection.Close();
            }

            return passedTests;
        }

        public static bool getTestIDByAppointmentID(int TestAppointmentID, ref int TestID)
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"Select TestID From Tests Where TestAppointmentID = @TestAppointmentID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
            
            object result = -1;
            try
            {
                connection.Open();

                result = command.ExecuteScalar();
                if (result != null) TestID = (int) result;



            }
            catch (Exception e)
            {
                DataAccessSettings.SaveLogToFileLog(e.Message);
            }
            finally
            {
                connection.Close();
            }
            return ((int)TestID != -1);
        }

        
    }
}
