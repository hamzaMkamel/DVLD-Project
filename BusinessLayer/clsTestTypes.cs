using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public  class clsTestTypes
    {
        public int TestTypeID { get; }
        public string TestTypeTitle { set; get; }
        public string TestTypeDescription { set; get; }
        public decimal TestTypeFees { set; get; }

        private clsTestTypes(int TestTypeID, string TestTypeTitle,string TestTypeDescription , decimal TestTypeFees)
        {
            this.TestTypeID = TestTypeID;
            this.TestTypeTitle = TestTypeTitle;
            this.TestTypeFees = TestTypeFees;
            this.TestTypeDescription = TestTypeDescription;
        }

        public static DataTable getAllTestTypes()
        {
            return TestTypesData.getAllTestTypes();
        }

        public bool updateTestTypeInfo()
        {
            return TestTypesData.UpdateTestTypeInfo(TestTypeID , TestTypeTitle , TestTypeDescription , TestTypeFees);
        }

        public static clsTestTypes getTestTypeByID(int TestTypeID)

        {
            string TestTitle = "", TestDescription = ""; decimal TestFees = 0;
            if (TestTypesData.getTestTypeInfo(TestTypeID, ref TestTitle, ref TestDescription,ref TestFees))
            {
                return new clsTestTypes(TestTypeID, TestTitle, TestDescription, TestFees);
            }
            return null;

        }
    }
}
