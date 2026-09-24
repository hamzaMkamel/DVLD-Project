using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;


public static class PersonAccessData
{
    public static int AddNewPerson(string NationalNo , string FirstName , string SecondName ,string ThirdName ,string LastName,
                    DateTime DateOfBirth , int Gendor , string Address , string Phone , string Email , int NationalityCountryID , string ImagePath )
    {
        SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
        string query = @"INSERT INTO People Values
                         (@NationalNo , @FirstName , @SecondName , @ThirdName , @LastName , @DateOfBirth , @Gendor ,
                            @Address , @Phone , @Email , @NationalityCountryID , @ImagePath);
                        Select Scope_IDENTITY()";
        int newID = -1; // to store & retrieve the newAddPersonID
        SqlCommand command = new SqlCommand(query, connection);

        command.Parameters.AddWithValue("@NationalNo", NationalNo);
        command.Parameters.AddWithValue("@FirstName", FirstName);
        command.Parameters.AddWithValue("@SecondName", SecondName);
        if(string.IsNullOrEmpty(ThirdName))
        {
            command.Parameters.AddWithValue("@ThirdName", System.DBNull.Value);
        }
        else command.Parameters.AddWithValue("@ThirdName", ThirdName);

        
        command.Parameters.AddWithValue("@LastName", LastName);
        command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
        command.Parameters.AddWithValue("@Gendor", Gendor);
        command.Parameters.AddWithValue("@Address", Address);
        command.Parameters.AddWithValue("@Phone", Phone);
        if (string.IsNullOrEmpty(Email))
        {
            command.Parameters.AddWithValue("@Email", System.DBNull.Value);
        }
        else command.Parameters.AddWithValue("@Email", Email);
        command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);

        if (string.IsNullOrEmpty(ImagePath))
        {
            command.Parameters.AddWithValue("@ImagePath", System.DBNull.Value);
        }
        else command.Parameters.AddWithValue("@ImagePath", ImagePath);


        try
        {
            connection.Open();
            object result = command.ExecuteScalar();

            if(result != null && int.TryParse(result.ToString() , out int ID))
            {
                newID = ID;
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

        return newID;
        }

    public static bool UpdatePerson(int PersonID , string NationalNo, string FirstName, string SecondName, string ThirdName, string LastName,
                    DateTime DateOfBirth, int Gendor, string Address, string Phone, string Email, int NationalityCountryID, string ImagePath)
    {
        SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
        string query = @"Update People Set NationalNo = @NationalNo , FirstName = @FirstName , SecondName = @SecondName ,
                        ThirdName = @ThirdName , LastName = @LastName , DateOfBirth = @DateOfBirth , Gendor = @Gendor,
                        Address = @Address , Phone = @Phone , Email = @Email , NationalityCountryID = @NationalityCountryID ,
                        ImagePath = @ImagePath Where PersonID = @PersonID";

        bool doneSuccessfully = false; // to ensure the process is done successfully

        SqlCommand command = new SqlCommand(query, connection);

        command.Parameters.AddWithValue("@PersonID", PersonID);
        command.Parameters.AddWithValue("@NationalNo", NationalNo);
        command.Parameters.AddWithValue("@FirstName", FirstName);
        command.Parameters.AddWithValue("@SecondName", SecondName);
        if (string.IsNullOrEmpty(ThirdName))
        {
            command.Parameters.AddWithValue("@ThirdName", System.DBNull.Value);
        }
        else command.Parameters.AddWithValue("@ThirdName", ThirdName);


        command.Parameters.AddWithValue("@LastName", LastName);
        command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
        command.Parameters.AddWithValue("@Gendor", Gendor);
        command.Parameters.AddWithValue("@Address", Address);
        command.Parameters.AddWithValue("@Phone", Phone);
        if (string.IsNullOrEmpty(Email))
        {
            command.Parameters.AddWithValue("@Email", System.DBNull.Value);
        }
        else command.Parameters.AddWithValue("@Email", Email);
        command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);

        if (string.IsNullOrEmpty(ImagePath))
        {
            command.Parameters.AddWithValue("@ImagePath", System.DBNull.Value);
        }
        else command.Parameters.AddWithValue("@ImagePath", ImagePath);

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

    public static bool getPersonInfoByID(int PersonID, ref string  NationalNo, ref string FirstName, ref string SecondName, ref string ThirdName, ref string LastName,
                    ref DateTime DateOfBirth, ref int Gendor, ref string Address, ref string Phone, ref string Email, ref int NationalityCountryID, ref string ImagePath)
    {
        SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
        string query = @"Select * From People Where PersonID = @PersonID";
        SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@PersonID", PersonID);
        bool isfound = false;
        try
        {
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            if(reader.Read())
            {
                NationalNo =(string) reader["NationalNo"];
                FirstName = (string)reader["FirstName"];
                SecondName = (string)reader["SecondName"];
                if (reader["ThirdName"] == System.DBNull.Value)
                    ThirdName = "";
                else ThirdName = (string)reader["ThirdName"];
                LastName = (string)reader["LastName"];
                DateOfBirth = (DateTime)reader["DateOfBirth"];
                Gendor = int.Parse(reader["Gendor"].ToString());
                Address = (string)reader["Address"];
                Phone = (string)reader["Phone"];
                if (reader["Email"] == System.DBNull.Value)
                    Email = "";
                else Email = (string)reader["Email"];
                NationalityCountryID = (int)reader["NationalityCountryID"];
                if (reader["ImagePath"] == System.DBNull.Value)
                    ImagePath = "";
                else ImagePath = (string)reader["ImagePath"];

                isfound = true;
            }
            else
            {
                isfound = false;
            }
            reader.Close();
        }
        catch(Exception e)
        {
            DataAccessSettings.SaveLogToFileLog(e.Message);
        }
        finally
        {
            connection.Close();
        }
        return isfound;
    }
    public static bool getPersonInfoByNationalNo(string NationalNo ,ref int PersonID, ref string FirstName, ref string SecondName, ref string ThirdName, ref string LastName,
                    ref DateTime DateOfBirth, ref int Gendor, ref string Address, ref string Phone, ref string Email, ref int NationalityCountryID, ref string ImagePath)
    {
        SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
        string query = @"Select * From People Where NationalNo = @NationalNo";
        SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@NationalNo", NationalNo);
        bool isfound = false;
        try
        {
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                PersonID = (int)reader["PersonID"];
                FirstName = (string)reader["FirstName"];
                SecondName = (string)reader["SecondName"];
                if (reader["ThirdName"] == System.DBNull.Value)
                    ThirdName = "";
                else ThirdName = (string)reader["ThirdName"];
                LastName = (string)reader["LastName"];
                DateOfBirth = (DateTime)reader["DateOfBirth"];
                Gendor = int.Parse(reader["Gendor"].ToString());
                Address = (string)reader["Address"];
                Phone = (string)reader["Phone"];
                if (reader["Email"] == System.DBNull.Value)
                    Email = "";
                else Email = (string)reader["Email"];
                NationalityCountryID = (int)reader["NationalityCountryID"];
                if (reader["ImagePath"] == System.DBNull.Value)
                    ImagePath = "";
                else ImagePath = (string)reader["ImagePath"];

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
    public static DataTable getAllPeople()
    {
        SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
        string query = @"Select PersonID ,NationalNo, FirstName ,SecondName ,ThirdName , LastName , Gendor = 
        Case when Gendor = 0 Then 'Male' Else 'Female' End , DateOfBirth , Nationality =  Countries.CountryName
        , Phone , Email From People
        inner join Countries On Countries.CountryID = People.NationalityCountryID;
        ";
        SqlCommand command = new SqlCommand(query, connection);
        DataTable dt = new DataTable();
        try
        {
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            if(reader.HasRows)
                dt.Load(reader);
            reader.Close();
        }
        catch(Exception e)
        {
            DataAccessSettings.SaveLogToFileLog(e.Message);
        }
        finally
        {
            connection.Close();
        }

        return dt;
    }

    public static bool getPersonNameByID(int PersonID , ref string PersonName)
    {
        SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
        string query = @"Select FirstName + ' ' + SecondName + ' ' + LastName From People Where PersonID = @PersonID;";
        SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@PersonID", PersonID);
        object result = 0;
        try
        {
            connection.Open();

            result = command.ExecuteScalar();
            if (result == null)  result= 0;
            else
            {
                PersonName = (string)result;
                result = 1;
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
        return ((int)result == 1);
    }

    public static bool DeletePerson(int PersonID)
    {
        SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
        string query = @"Delete From People Where PersonID = @PersonID";
        SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@PersonID", PersonID);
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
    public static bool IsPersonExists(string NationalNo)
    {
        SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
        string query = @"Select result = 1 From People Where NationalNo = @NationalNo";
        SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@NationalNo", NationalNo);
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

    public static bool IsPersonExists(int PersonID)
    {
        SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
        string query = @"Select result = 1 From People Where PersonID = @PersonID";
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

}


