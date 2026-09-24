using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DataAccessLayer
{
    public static class TestAppointmentsDataAccess
    {
        public static int addNewAppointment(int TestTypeID , int LocalDrivingLicenseApplicationID , DateTime AppointmentDate,
                            decimal PaidFees , int CreatedByUserID , int RetakeTestApplicationID)
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"INSERT INTO TestAppointments (TestTypeID , LocalDrivingLicenseApplicationID ,
                        AppointmentDate , PaidFees , CreatedByUserID , IsLocked , RetakeTestApplicationID) Values (@TestTypeID , @LocalDrivingLicenseApplicationID , @AppointmentDate
                        ,@PaidFees , @CreatedByUserID , 0 , @RetakeTestApplicationID);
                        Select Scope_IDENTITY();";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            command.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);
            command.Parameters.AddWithValue("@PaidFees", PaidFees);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            if(RetakeTestApplicationID == -1)
            {
                command.Parameters.AddWithValue("@RetakeTestApplicationID", System.DBNull.Value);
            }
            else
                command.Parameters.AddWithValue("@RetakeTestApplicationID", RetakeTestApplicationID);



            int newAppointmentID = -1;
            try
            {
                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int ID))
                {
                    newAppointmentID = ID;
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

            return newAppointmentID;
        }

        public static bool setAppointmentLocked(int TestAppointmentID)
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"Update TestAppointments Set IsLocked = 1
                            Where TestAppointmentID = @TestAppointmentID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
            
            int rowAffected = 0;
            try
            {
                connection.Open();
                rowAffected = command.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                DataAccessSettings.SaveLogToFileLog(e.Message);
            }
            finally
            {
                connection.Close();
            }
            return (rowAffected > 0);
        }

        public static bool updateAppointmentDate(int TestAppointmentID,DateTime AppointmentDate)
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"Update TestAppointments Set AppointmentDate = @AppointmentDate
                            Where TestAppointmentID = @TestAppointmentID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
            command.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);

            int rowAffected = 0;
            try
            {
                connection.Open();
                rowAffected = command.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                DataAccessSettings.SaveLogToFileLog(e.Message);
            }
            finally
            {
                connection.Close();
            }
            return (rowAffected > 0);
        }

        public static bool isPersonTakeTestBefore(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"Select Found = 1 From TestAppointments Where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID and TestTypeID = @TestTypeID";
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

        


        public static DataTable getAssociatedTestAppointments(int LocalDrivingLicenseApplicationID , int TestTypeID)
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"Select TestAppointmentID , AppointmentDate , PaidFees , IsLocked from TestAppointments
                            Where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID and TestTypeID = @TestTypeID;";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
            DataTable dt = new DataTable();
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                    dt.Load(reader);
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

            return dt;
        }

        /*There is 3 Setps to let the Applicant to add an appointment
         * 1 - he is not passed in this test before
         * 2-there is no appointment is not locked (that 's mean he already has a open appointment)
         * 3- if he is failed before we should take care of this situation and apply a new application (retake test)
         */
        //2-
        public static bool isThereNotLockedAppointment(int LocalDrivingLicenseApplicationID, int TestTypeID) 
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"Select Found = 1 From TestAppointments Where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID and TestTypeID = @TestTypeID and IsLocked = 0;";
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

        public static bool getTestAppointmentInfo(int TestAppointmentID, ref int TestTypeID, ref int LocalDrivingLicenseApplicationID, ref DateTime AppointmentDate,
                            ref decimal PaidFees, ref int CreatedByUserID, ref bool IsLocked , ref int RetakeTestApplicationID)
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"Select * From TestAppointments Where TestAppointmentID = @TestAppointmentID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
            bool isfound = false;
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    TestTypeID = (int)reader["TestTypeID"];
                    LocalDrivingLicenseApplicationID = (int)reader["LocalDrivingLicenseApplicationID"];
                    AppointmentDate = (DateTime)reader["AppointmentDate"];
                    PaidFees = (decimal)reader["PaidFees"];
                    CreatedByUserID = (int)reader["CreatedByUserID"];
                    IsLocked = (bool)reader["IsLocked"];
                    

                    if (reader["RetakeTestApplicationID"] == System.DBNull.Value)
                        RetakeTestApplicationID = -1;
                    else RetakeTestApplicationID = (int)reader["RetakeTestApplicationID"];
                    


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

        public static bool isTestAppointmentTaken(int TestAppointmentID)
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"Select Found = 1 From TestAppointments Where TestAppointmentID = @TestAppointmentID and IsLocked = 1;";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);

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
    }
}
