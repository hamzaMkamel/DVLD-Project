using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public static class DetainedLicensesDataAccess
    {
        public static DataTable getAllDetainedLicenses()
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"Select DetainID , DetainedLicenses.LicenseID , DetainDate , FineFees ,IsReleased , ReleaseDate , People.NationalNo
                            ,FullName = People.FirstName + ' ' + 
                            People.SecondName + ' ' + Concat(People.ThirdName , '') + ' ' + People.LastName , DetainedLicenses.ReleaseApplicationID
                            from DetainedLicenses inner join Licenses On Licenses.LicenseID = DetainedLicenses.LicenseID inner join Drivers ON
                            Drivers.DriverID = Licenses.DriverID inner join People On People.PersonID = Drivers.PersonID";
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

        public static int addNewDetainedLicense(int LicenseID, DateTime DetainDate, decimal FineFees
           , int CreatedByUserID)
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"INSERT INTO DetainedLicenses( LicenseID , DetainDate , FineFees , CreatedByUserID , IsReleased)
                        Values
                         (@LicenseID , @DetainDate , @FineFees , @CreatedByUserID ,0);
                        Select Scope_IDENTITY()";
            SqlCommand command = new SqlCommand(query, connection);
            
            command.Parameters.AddWithValue("@LicenseID", LicenseID);
            command.Parameters.AddWithValue("@DetainDate", DetainDate);
            command.Parameters.AddWithValue("@FineFees", FineFees);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            int newDetainID = -1;
            try
            {
                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int ID))
                {
                    newDetainID = ID;
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

            return newDetainID;
        }

        public static bool ReleaseDetainedLicense(int DetainID, DateTime ReleaseDate, int ReleasedByUserID, int ReleaseApplicationID)
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"Update DetainedLicenses Set ReleaseDate = @ReleaseDate , IsReleased = 1 , ReleasedByUserID = @ReleasedByUserID , ReleaseApplicationID = @ReleaseApplicationID
                             Where DetainID = @DetainID;";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@DetainID", DetainID);

            command.Parameters.AddWithValue("@ReleaseDate", ReleaseDate);
            command.Parameters.AddWithValue("@ReleasedByUserID", ReleasedByUserID);
            command.Parameters.AddWithValue("@ReleaseApplicationID", ReleaseApplicationID);
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

        public static bool getDetainedLicense(int DetainID, ref int LicenseID, ref DateTime DetainDate, ref decimal FineFees
           , ref int CreatedByUserID, ref bool IsReleased, ref DateTime ReleaseDate, ref int ReleasedByUserID, ref int ReleaseApplicationID)
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"Select * from DetainedLicenses Where DetainID = @DetainID;";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@DetainID", DetainID);
            bool isfound = false;
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    LicenseID = (int)reader["LicenseID"];
                    DetainDate = (DateTime)reader["DetainDate"];
                    FineFees = (decimal)reader["FineFees"];
                    CreatedByUserID = (int)reader["CreatedByUserID"];
                    IsReleased = (bool)reader["IsReleased"];
                    if (reader["ReleaseDate"] != System.DBNull.Value)
                        ReleaseDate = (DateTime)reader["ReleaseDate"];
                    if (reader["ReleasedByUserID"] == System.DBNull.Value)
                        ReleasedByUserID = -1;
                    else
                        ReleasedByUserID = (int)reader["ReleasedByUserID"];
                    if (reader["ReleaseApplicationID"] == System.DBNull.Value)
                        ReleasedByUserID = -1;
                    else
                        ReleasedByUserID = (int)reader["ReleaseApplicationID"];



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
    
        
        public static bool isLicenseDetained(int LicenseID)
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"Select result = 1 From DetainedLicenses Where LicenseID = @LicenseID and IsReleased = 0";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LicenseID", LicenseID);
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
        
        public static int getDetainIDByLicenseID(int LicenseID)
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"Select DetainID From DetainedLicenses Where LicenseID = @LicenseID and IsReleased = 0";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LicenseID", LicenseID);
            object result = -1;
            try
            {
                connection.Open();

                result = command.ExecuteScalar();




            }
            catch (Exception e)
            {
                DataAccessSettings.SaveLogToFileLog(e.Message);
            }
            finally
            {
                connection.Close();
            }
            return (int)result;
        }
    }
}
