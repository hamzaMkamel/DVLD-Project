using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public static  class ApplicationTypesDataAccess
    {
        public static DataTable getAllApplicationTypes()
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"Select * from ApplicationTypes;";
        
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
        public static bool UpdateApplicationTypeInfo(int AppTypeID , string AppTypeName , decimal AppTypeFees)
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"Update ApplicationTypes 
                Set ApplicationFees = @AppTypeFees , ApplicationTypeTitle = @AppTypeName Where ApplicationTypeID = @AppTypeID;";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@AppTypeName", AppTypeName);
            command.Parameters.AddWithValue("@AppTypeFees", AppTypeFees);
            command.Parameters.AddWithValue("@AppTypeID", AppTypeID);
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

        public static bool getApplicationTypeInfo(int AppTypeID, ref string AppTypeName, ref decimal AppTypeFees)
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"Select * from ApplicationTypes Where ApplicationTypeID = @AppTypeID;";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@AppTypeID", AppTypeID);
            bool isfound = false;
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    
                    AppTypeName = (string)reader["ApplicationTypeTitle"];
                    AppTypeFees = (decimal)reader["ApplicationFees"];
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

        public static bool getApplicationTypeFees(int ApplicationTypeID , ref decimal fees)
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"Select ApplicationFees From ApplicationTypes Where ApplicationTypeID = @ApplicationTypeID;";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
            bool isdone = false;
            try
            {
                object result = (decimal)command.ExecuteScalar();
                if(result != null )
                {
                    fees = (decimal)result;
                    isdone = true;
                }

            }
            catch(Exception e)
            {
                DataAccessSettings.SaveLogToFileLog(e.Message);
            }
            finally
            {
                connection.Close();
            }
            return isdone;
        }

        public static bool getApplicationTypeIDByName(string ApplicationTypeTitle, ref int ApplicationID)
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"Select ApplicationTypeID from ApplicationTypes Where ApplicationTypeTitle = @ApplicationTypeTitle;;";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ApplicationTypeTitle", ApplicationTypeTitle);
            bool isdone = false;
            try
            {
                object result = (decimal)command.ExecuteScalar();
                if (result != null)
                {
                    ApplicationID = (int)result;
                    isdone = true;
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
            return isdone;
        }

        public static bool getApplicationTypeNameByID(int ApplicationID ,ref string ApplicationTypeTitle)
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"Select ApplicationTypeTitle from ApplicationTypes Where ApplicationID = @ApplicationID;;";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            bool isdone = false;
            try
            {
                object result = (decimal)command.ExecuteScalar();
                if (result != null)
                {
                    ApplicationTypeTitle = (string)result;
                    isdone = true;
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
            return isdone;
        }



    }
}
