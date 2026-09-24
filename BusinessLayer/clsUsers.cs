using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;
using DataAccessLayer;
namespace BusinessLayer
{
    public class clsUsers
    {
        public int PersonID { get; set; }
        public int UserID { get; private set; }
        public string UserName { set; get; }
        public bool isActive { set; get; }
        public clsPeople personInfo;
        public enum enMode { AddNewUser , UpdateUser};
        enMode mode;
        public clsUsers()
        {
            PersonID = -1;
            UserID = -1;
            UserName = string.Empty;
            isActive = false ;
            mode = enMode.AddNewUser;
        }

        clsUsers(int PersonID , int UserID , string UserName , bool isActive)
        {
            this.PersonID = PersonID;
            this.UserID = UserID;
            this.UserName = UserName;
            personInfo = clsPeople.Find(PersonID);
            this.isActive = isActive;
            mode = enMode.UpdateUser;
        }

        public bool Save(string password)

        { 
            if(mode== enMode.AddNewUser)
            {
                password = ModifyPasswordToHashedPassword(password);
                this.UserID = UsersDataAccess.AddNewUser(PersonID, UserName, password, isActive);
                mode = enMode.UpdateUser;
                return this.UserID != -1;
            }
            else
            {
                return UsersDataAccess.UpdateUser(UserID, UserName, isActive);
            }
        }

        public static bool ChangeUserPassword(string UserName, string newPassword)
        {
            newPassword = ModifyPasswordToHashedPassword(newPassword);
            return UsersDataAccess.ChangeUserPassWord(UserName, newPassword);
        }

        public static clsUsers Find(int UserID)
        {
            int PersonID = -1;bool isActive = false ;
            string UserName = "";
            if (UsersDataAccess.getUserInfoByUserID(UserID, ref PersonID, ref UserName, ref isActive))
            {
                return new clsUsers(PersonID, UserID, UserName, isActive);
            }
            else return null;

        }

        public static clsUsers Find(string UserName)
        {
            int PersonID = -1; bool isActive = false;
            int UserID = -1;
            if (UsersDataAccess.getUserByUserName(UserName, ref PersonID, ref UserID, ref isActive))
            {
                return new clsUsers(PersonID, UserID, UserName, isActive);
            }
            else return null;
        }
        public static string getUserName(int UserID)
        {
            return UsersDataAccess.getUserName(UserID);
        }

        public static bool VerifyUserNameAndPassword(string UserName , string userAttemptPassword)
        {
            string hashedPassword = "";
            if (UsersDataAccess.getPasswordForSpecificUserName(UserName, ref hashedPassword))
            {
                return BCrypt.Net.BCrypt.Verify(userAttemptPassword, hashedPassword);
            }
            else
                return false;
        }
        public static bool isUserNameExists(string UserName)
        {
            return UsersDataAccess.isUserNameExists(UserName);
        }

        public static DataTable getAllUsers()
        {
            return UsersDataAccess.getAllUsers();
        }


        private static  string ModifyPasswordToHashedPassword(string password) // simply converts the normal string to hashed string using BCrypt library
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public static bool isUserExists(int PersonID)
        {
            return UsersDataAccess.isUserExists(PersonID);
        }

        public static bool deleteUser(int UserID)
        {
        
            return UsersDataAccess.DeleteUser(UserID);
        }

        public static bool StoreRememberMeInfo(string UserName , string Password)
        {
            try
            {
                string fullRecord = UserName + "|" + Password;
                string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string fullDirectory = Path.Combine(appDirectory, "AppLoginInfo.txt");
                File.WriteAllText(fullDirectory, fullRecord);
            }
            catch (Exception e)
            {
                DataAccessSettings.SaveLogToFileLog(e.Message);
                return false;
            }
            return true;
        }
        public static bool ClearRememberMeInfo()
        {
            try
            {
                
                string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string fullDirectory = Path.Combine(appDirectory, "AppLoginInfo.Txt");
                File.Delete(fullDirectory);
            }
            catch (Exception e)
            {
                DataAccessSettings.SaveLogToFileLog(e.Message);
                return false;
            }
            return true;
            
        }
        public static bool ReadRememberMeInfo(ref string UserName , ref string Password)
        {
            try
            {

                string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string fullDirectory = Path.Combine(appDirectory, "AppLoginInfo.txt");
                string fullRecord = File.ReadAllText(fullDirectory);
                string[] fullSeparatedRecord = fullRecord.Split('|');
                UserName = fullSeparatedRecord[0];
                Password = fullSeparatedRecord[1];

            }
            catch (Exception e)
            {
                DataAccessSettings.SaveLogToFileLog(e.Message);
                return false;
            }
            return true;
        }


    }
}
