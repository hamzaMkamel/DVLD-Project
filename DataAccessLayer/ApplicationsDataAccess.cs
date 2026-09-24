using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public static class ApplicationsDataAccess
    {
        public static int addNewApplication(int ApplicantPersonID , DateTime ApplicationDate , int ApplicationTypeID , int ApplicationStatus ,
                         DateTime LastStatusDate , decimal PaidFees , int CreatedByUserID)
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"INSERT INTO Applications Values
                         (@ApplicantPersonID , @ApplicationDate , @ApplicationTypeID , @ApplicationStatus , 
                         @LastStatusDate , @PaidFees , @CreatedByUserID);
                        Select Scope_IDENTITY();";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ApplicantPersonID", ApplicantPersonID);
            command.Parameters.AddWithValue("@ApplicationDate", ApplicationDate);
            command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
            command.Parameters.AddWithValue("@ApplicationStatus", ApplicationStatus);
            command.Parameters.AddWithValue("@LastStatusDate", LastStatusDate);
            command.Parameters.AddWithValue("@PaidFees", PaidFees);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            int newApplicationID = -1;
            try
            {
                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int ID))
                {
                    newApplicationID = ID;
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

            return newApplicationID;
        }

        public static bool UpdateApplication(int ApplicationID ,  int ApplicantPersonID,  DateTime ApplicationDate, int ApplicationTypeID,  int ApplicationStatus,
                          DateTime LastStatusDate,  decimal PaidFees, int CreatedByUserID)

        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"Update Applications Set ApplicantPersonID = @ApplicantPersonID , ApplicationDate = @ApplicationDate , ApplicationTypeID = @ApplicationTypeID
            , ApplicationStatus = @ApplicationStatus , LastStatusDate = @LastStatusDate , PaidFees = @PaidFees , CreatedByUserID = @CreatedByUserID Where ApplicationID = @ApplicationID;";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ApplicantPersonID", ApplicantPersonID);

            command.Parameters.AddWithValue("@ApplicationDate", ApplicationDate);
            command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
            command.Parameters.AddWithValue("@ApplicationStatus", ApplicationStatus);
            command.Parameters.AddWithValue("@LastStatusDate", LastStatusDate);
            command.Parameters.AddWithValue("@PaidFees", PaidFees);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            bool doneSuccessfully = false;
            try
            {
                connection.Open();
                int result = command.ExecuteNonQuery();

                if (result > 0) doneSuccessfully = true;
            }
            catch (Exception e)
            {
                DataAccessSettings.SaveLogToFileLog(e.Message);
            }
            finally
            {
                connection.Close();
            }

            return doneSuccessfully;
        }

        public static bool CancelApplication(int ApplicationID)
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"Update Applications Set ApplicationStatus = 2 , LastStatusDate = @LastStatusDate Where ApplicationID = @ApplicationID;";
            SqlCommand command = new SqlCommand(query, connection);
            
            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            command.Parameters.AddWithValue("@LastStatusDate", DateTime.Now);
            bool doneSuccessfully = false;
            try
            {
                connection.Open();
                int result = command.ExecuteNonQuery();

                if (result > 0) doneSuccessfully = true;
            }
            catch (Exception e)
            {
                DataAccessSettings.SaveLogToFileLog(e.Message);
            }
            finally
            {
                connection.Close();
            }

            return doneSuccessfully;
        }

        public static bool setCompletedApplication(int ApplicationID)
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"Update Applications Set ApplicationStatus = 3 ,  LastStatusDate = @LastStatusDate  Where ApplicationID = @ApplicationID;";
            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            command.Parameters.AddWithValue("@LastStatusDate", DateTime.Now);
            bool doneSuccessfully = false;
            try
            {
                connection.Open();
                int result = command.ExecuteNonQuery();

                if (result > 0) doneSuccessfully = true;
            }
            catch (Exception e)
            {
                DataAccessSettings.SaveLogToFileLog(e.Message);
            }
            finally
            {
                connection.Close();
            }

            return doneSuccessfully;
        }

        public static bool DeleteApplication(int ApplicationID)
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"Delete From Applications Where ApplicationID = @ApplicationID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
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

        public static bool isApplicationStatusCanceledOrFinished(int ApplicationID)

        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"Select Found = 1 from Applications Where ApplicationStatus != 1 and ApplicationID = @ApplicationID;";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            
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

        public static bool isApplicationCompleted(int ApplicationID)
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"Select Found = 1 from Applications Where ApplicationStatus = 3 and ApplicationID = @ApplicationID;";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

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
        public static bool isApplicationCanceled(int ApplicationID)
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"Select Found = 1 from Applications Where ApplicationStatus = 2 and ApplicationID = @ApplicationID;";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

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

        public static bool getApplicationInfoByID(int ApplicationID, ref int ApplicantPersonID, ref DateTime ApplicationDate, ref int ApplicationTypeID, ref byte ApplicationStatus,
                         ref DateTime LastStatusDate, ref decimal PaidFees, ref int CreatedByUserID)
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"Select * from Applications Where ApplicationID = @ApplicationID;";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            bool isfound = false;
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    ApplicantPersonID = (int)reader["ApplicantPersonID"];
                    ApplicationDate = (DateTime)reader["ApplicationDate"];
                    ApplicationTypeID = (int)reader["ApplicationTypeID"];
                    ApplicationStatus = (byte)reader["ApplicationStatus"];
                    LastStatusDate = (DateTime)reader["LastStatusDate"];
                    PaidFees = (decimal)reader["PaidFees"];
                    CreatedByUserID = (int)reader["CreatedByUserID"];
                    isfound = true;

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


        public static bool IsApplicationExist(int ApplicationID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);

            string query = "SELECT Found=1 FROM Applications WHERE ApplicationID = @ApplicationID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                isFound = reader.HasRows;

                reader.Close();
            }
            catch (Exception ex)
            {
                DataAccessSettings.SaveLogToFileLog(ex.Message);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static bool DoesPersonHaveActiveApplication(int PersonID, int ApplicationTypeID)
        {

            //incase the ActiveApplication ID !=-1 return true.
            return (GetActiveApplicationID(PersonID, ApplicationTypeID) != -1);
        }

        public static int GetActiveApplicationID(int PersonID, int ApplicationTypeID)
        {
            int ActiveApplicationID = -1;

            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);

            string query = "SELECT ActiveApplicationID=ApplicationID FROM Applications WHERE ApplicantPersonID = @ApplicantPersonID and ApplicationTypeID=@ApplicationTypeID and ApplicationStatus=1";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicantPersonID", PersonID);
            command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();


                if (result != null && int.TryParse(result.ToString(), out int AppID))
                {
                    ActiveApplicationID = AppID;
                }
            }
            catch (Exception ex)
            {
                DataAccessSettings.SaveLogToFileLog(ex.Message);
                return ActiveApplicationID;
            }
            finally
            {
                connection.Close();
            }

            return ActiveApplicationID;
        }

        


    }
}
