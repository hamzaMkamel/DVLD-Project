using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public static class InternationalLicensesDataAccess
    {
        public static DataTable getAllLicensesToSpecificDriver(int DriverID)
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"Select InternationalLicenseID , ApplicationID  , IssuedUsingLocalLicenseID ,IssueDate
                            ,IsActive from InternationalLicenses Where DriverID = @DriverID;";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@DriverID", DriverID);
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

        public static DataTable getAllInternationalLicenses()
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"Select InternationalLicenseID , ApplicationID , DriverID , IssuedUsingLocalLicenseID ,IssueDate, ExpirationDate
                            ,IsActive from InternationalLicenses;";
            SqlCommand command = new SqlCommand(query, connection);
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

        public static int AddNewLicense(int ApplicationID, int DriverID, int IssuedUsingLocalLicenseID, DateTime IssueDate, DateTime ExpirationDate,
            bool IsActive, int CreatedByUserID)
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"
                             Update InternationalLicenses set  IsActive = 0 Where DriverID = @DriverID;
                        INSERT INTO InternationalLicenses Values
                         (@ApplicationID , @DriverID , @IssuedUsingLocalLicenseID , @IssueDate ,@ExpirationDate ,1 , @CreatedByUserID );
                        Select Scope_IDENTITY()";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            command.Parameters.AddWithValue("@IssuedUsingLocalLicenseID", IssuedUsingLocalLicenseID);
            command.Parameters.AddWithValue("@DriverID", DriverID);
            command.Parameters.AddWithValue("@IssueDate", IssueDate);
            command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);



            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

            int newLicenseID = -1;
            try
            {
                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int ID))
                {
                    newLicenseID = ID;
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

            return newLicenseID;
        }


        public static bool setInternationalLicenseActiveStatus(int InternationalLicenseID, int IsActive)
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"Update InternationalLicenses Set IsActive = @IsActive
                             Where InternationalLicenseID = @InternationalLicenseID;";
            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@InternationalLicenseID", InternationalLicenseID);

            command.Parameters.AddWithValue("@IsActive", IsActive);
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

        public static bool getInternationalLicenseStatus(int InternationalLicenseID)
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"Select IsActive From InternationalLicenses Where InternationalLicenseID = @InternationalLicenseID;";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@InternationalLicenseID", InternationalLicenseID);
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
            return (bool)result;
        }

        public static bool DeactivateInternationalLicense(int InternationalLicenseID)
        {

            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);

            string query = @"UPDATE InternationalLicenses
                           SET 
                              IsActive = 0
                             
                         WHERE InternationalLicenseID=@InternationalLicenseID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@InternationalLicenseID", InternationalLicenseID);


            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                DataAccessSettings.SaveLogToFileLog(ex.Message);
                return false;
            }

            finally
            {
                connection.Close();
            }

            return (rowsAffected > 0);
        }


        public static bool isLicenseLinkedToInternationalLicense(int IssuedUsingLocalLicenseID) 
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"Select Found = 1 from InternationalLicenses where IssuedUsingLocalLicenseID = @IssuedUsingLocalLicenseID and IsActive = 1;";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@IssuedUsingLocalLicenseID", IssuedUsingLocalLicenseID);
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

        public static bool getLicenseInfoByLicenseID(int InternationalLicenseID, ref int ApplicationID, ref int DriverID,ref int IssuedUsingLocalLicenseID,ref DateTime IssueDate,ref DateTime ExpirationDate,
           ref bool IsActive,ref int CreatedByUserID)
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"Select * from InternationalLicenses Where InternationalLicenseID = @InternationalLicenseID;";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@InternationalLicenseID", InternationalLicenseID);
            bool isfound = false;
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    ApplicationID = (int)reader["ApplicationID"];
                    DriverID = (int)reader["DriverID"];
                    IssuedUsingLocalLicenseID = (int)reader["IssuedUsingLocalLicenseID"];
                    IssueDate = (DateTime)reader["IssueDate"];
                    ExpirationDate = (DateTime)reader["ExpirationDate"];
                    

                    
                    IsActive = (bool)reader["IsActive"];
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
    }
}
