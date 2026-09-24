using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public static  class DriversDataAccess
    {

        public static DataTable getAllDriversTable()
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"Select Drivers.DriverID , People.PersonID , People.NationalNo , FullName = People.FirstName + ' ' + 
                            People.SecondName + ' ' + Concat(People.ThirdName , '') + ' ' + People.LastName , Drivers.CreatedDate ,
                            ActiveLicenses = Case when #R1.ActiveLicenses is null Then '0'
                            Else  #R1.ActiveLicenses
                            End From Drivers
                            Inner join People On Drivers.PersonID = People.PersonID
                            left join 
                            (Select DriverID , ActiveLicenses = Count(IsActive) from Licenses
                            Where IsActive = 1
                            Group By DriverID) #R1 ON #R1.DriverID = Drivers.DriverID;";
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

        public static int AddNewDriver(int PersonID, int CreatedByUserID, DateTime CreatedDate)
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"INSERT INTO Drivers Values
                         (@PersonID , @CreatedByUserID , @CreatedDate);
                        Select Scope_IDENTITY()";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            command.Parameters.AddWithValue("@CreatedDate", CreatedDate);
            
            int newDriverID = -1;
            try
            {
                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int ID))
                {
                    newDriverID = ID;
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

            return newDriverID;
        }

        public static bool getDriverInfoByDriverID(int DriverID, ref int PersonID,
            ref int CreatedByUserID, ref DateTime CreatedDate)
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"Select * from Drivers Where DriverID = @DriverID;";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@DriverID", DriverID);
            bool isfound = false;
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    PersonID = (int)reader["PersonID"];
                    CreatedByUserID = (int)reader["CreatedByUserID"];
                    CreatedDate = (DateTime)reader["CreatedDate"];
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

        public static bool isPersonADriver(int PersonID)
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"Select Found = 1 From Drivers Where PersonID = @PersonID;";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PersonID", PersonID);
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

        public static bool getDriverIDByPersonID(int PersonID , ref int DriverID)
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"Select DriverID From Drivers Where PersonID = @PersonID;";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PersonID", PersonID);
            object result = 0;
            try
            {
                connection.Open();

                result = command.ExecuteScalar();
                if (result == null)
                { result = 0; }
                else
                    DriverID = (int)result;



            }
            catch (Exception e)
            {
                DataAccessSettings.SaveLogToFileLog(e.Message);
            }
            finally
            {
                connection.Close();
            }
            return ((int)result != 0);
        }
    }
}
