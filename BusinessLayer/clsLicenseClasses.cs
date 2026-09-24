using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class clsLicenseClasses
    {

        public int LicenseClassID { set; get; }
        public string ClassName { set; get; }
        public string ClassDescription { set; get; }

        public byte MinimumAllowedAge { set; get; }
        public byte DefaultValidityLength { set; get; }
        public decimal ClassFees { set; get; }


        private clsLicenseClasses()
        {

        }
        private clsLicenseClasses(int LicenseClassID,  string ClassName,  string ClassDescription,  byte MinimumAllowedAge,  byte DefaultValidityLength,  decimal ClassFees)
        {
            this.LicenseClassID = LicenseClassID;
            this.ClassName = ClassName;
            this.ClassDescription = ClassDescription;
            this.MinimumAllowedAge = MinimumAllowedAge;
            this.DefaultValidityLength = DefaultValidityLength;
            this.ClassFees = ClassFees;
        }

        public static DataTable getAllLicenseClasses()
        {
            return LicenseClassesDataAccess.getAllLicenseClassesName();
        }

        public static int getLicenseClassIDByName(string className)
        {
            int id = -1;
            LicenseClassesDataAccess.getLicenceClassIDByName(className, ref id);
            return id;
        }

        public static string getLicenseClassNameByID(int id)
        {
            string className = "";
            LicenseClassesDataAccess.getLicenceClassNameByID(id , ref className);
            return className;

        }
        public static clsLicenseClasses Find(int LicenseClassID)
        {
            
            string ClassName = "";
            string ClassDescription = "";
            byte MinimumAllowedAge = 0;
            byte DefaultValidityLength = 0;
            decimal ClassFees = -1;
            if(LicenseClassesDataAccess.getLicenseClassInfoByID(LicenseClassID , ref ClassName , ref ClassDescription , ref MinimumAllowedAge , ref DefaultValidityLength ,ref ClassFees))
            {
                return new clsLicenseClasses(LicenseClassID, ClassName, ClassDescription, MinimumAllowedAge, DefaultValidityLength, ClassFees);
            }
            return null;
        }
        
    }
}
