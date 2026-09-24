using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class clsApplicationTypes
    {
        public int ApplicationTypeID { get; }
        public string ApplicationTypeName { set; get; }
        public decimal ApplicationTypeFees { set; get; }

        private clsApplicationTypes(int ApplicationTypeID , string ApplicationTypeName , decimal ApplicationTypeFees)
        {
            this.ApplicationTypeID = ApplicationTypeID;
            this.ApplicationTypeName = ApplicationTypeName;
            this.ApplicationTypeFees = ApplicationTypeFees;
        }

        public static DataTable getAllApplicationTypes()
        {
            return ApplicationTypesDataAccess.getAllApplicationTypes();
        }

        public  bool updateApplicationType()
        {
            return ApplicationTypesDataAccess.UpdateApplicationTypeInfo(ApplicationTypeID, ApplicationTypeName, ApplicationTypeFees);
        }

        public static  clsApplicationTypes getApplicationTypeByID(int ApplicationTypeID)

        {
            string AppName = ""; decimal AppFees = 0;
            if(ApplicationTypesDataAccess.getApplicationTypeInfo(ApplicationTypeID , ref AppName , ref AppFees))
            {
                return new clsApplicationTypes(ApplicationTypeID, AppName, AppFees);
            }
            return null;

        }

        public static int getApplicationTypeIDByName(string ApplicationTypeName)
        {
            int appID = -1;

            ApplicationTypesDataAccess.getApplicationTypeIDByName(ApplicationTypeName, ref appID);
            return appID;
        }

    }
}
