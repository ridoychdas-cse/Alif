using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using KHSC;


/// <summary>
/// Summary description for clsDeptManager
/// </summary>
/// 
namespace autouniv
{
    public class clsDeptManager
    {
        public static DataTable GetDepts()
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            //string query = "select dept_id,dept_name,abvr,convert(decimal,costs)costs,credit from dept_info order by dept_id";
            string query =
                @"select dept_id,dept_name,abvr,convert(decimal,costs)costs,credit,CONVERT(decimal,[SEMISTERFEE]) SEMISTERFEE,CONVERT(decimal,[ADMISSIONFEE]) ADMISSIONFEE
      ,CONVERT(int,[NOOFSEMI]) NOOFSEMI,CONVERT(decimal,[SEMIHIGHCREDIT]) SEMIHIGHCREDIT,CONVERT(decimal,[PERCREDITCOST]) PERCREDITCOST from dept_info order by dept_id";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Depts");
            return dt;
        }

        public static void UpdateDept(clsDept dept)
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = @"update dept_info set dept_name='" + dept.DeptName + "',abvr='" + dept.Abvr +
                           "', credit=convert(decimal,'" + dept.Credit + "'),costs=convert(decimal,'" + dept.Costs +
                           "'),[SEMISTERFEE] = convert(decimal,'" + dept.SEMISTERFEE +
                           "'),[ADMISSIONFEE] = convert(decimal,'" + dept.ADMISSIONFEE + "'),[NOOFSEMI] = convert(int,'" +
                           dept.NOOFSEMI + "'),[SEMIHIGHCREDIT] = convert(decimal,'" + dept.SEMIHIGHCREDIT +
                           "'),[PERCREDITCOST] = convert(decimal,'" + dept.PERCREDITCOST + "') where dept_id='" +
                           dept.DeptId + "'";
            DataManager.ExecuteNonQuery(connectionString, query);
        }

        public static void DeleteDept(string dept)
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = "delete from dept_info where dept_id='" + dept + "'";
            DataManager.ExecuteNonQuery(connectionString, query);
        }

        public static void CreateDept(clsDept dept)
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = @"insert into dept_info (dept_id, dept_name,abvr,costs,credit,[SEMISTERFEE]
           ,[ADMISSIONFEE]
           ,[NOOFSEMI]
           ,[SEMIHIGHCREDIT]
           ,[PERCREDITCOST]) values ('" + dept.DeptId + "','" + dept.DeptName + "','" + dept.Abvr +
                           "',convert(decimal,'" + dept.Costs + "'),'" + dept.Credit + "','" + dept.SEMISTERFEE + "','" +
                           dept.ADMISSIONFEE + "','" + dept.NOOFSEMI + "','" + dept.SEMIHIGHCREDIT + "','" +
                           dept.PERCREDITCOST + "')";
            DataManager.ExecuteNonQuery(connectionString, query);
        }

        public static clsDept getDept(string dept)
        {
            String connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);

            //string query = "select dept_id,dept_name,abvr,convert(decimal,costs)costs,credit from dept_info where dept_id='" + dept + "'";
            string query =
                @"select dept_id,dept_name,abvr,convert(decimal,costs)costs,credit,CONVERT(decimal,[SEMISTERFEE]) SEMISTERFEE,CONVERT(decimal,[ADMISSIONFEE]) ADMISSIONFEE
      ,CONVERT(int,[NOOFSEMI]) NOOFSEMI,CONVERT(decimal,[SEMIHIGHCREDIT]) SEMIHIGHCREDIT,CONVERT(decimal,[PERCREDITCOST]) PERCREDITCOST from dept_info where dept_id='" +
                dept + "'";

            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Dept");
            if (dt.Rows.Count == 0)
            {
                return null;
            }
            return new clsDept(dt.Rows[0]);
        }

        public static string getDeptName(string dept)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            string Query =
                "select isnull(dept_name, '')+ ' ('+isnull(abvr, '')+')' dept_name from dept_info where dept_id='" +
                dept + "'";
            myConnection.Open();
            SqlCommand myCommand = new SqlCommand(Query, myConnection);
            object maxValue = myCommand.ExecuteScalar();
            myConnection.Close();
            string a = "";
            if (maxValue != null)
            {
                a = maxValue.ToString();
            }
            return a;
        }

        public static string getDeptCost(string dept)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            string Query = "select convert(decimal,costs)costs from dept_info where dept_id='" + dept + "'";
            myConnection.Open();
            SqlCommand myCommand = new SqlCommand(Query, myConnection);
            object maxValue = myCommand.ExecuteScalar();
            myConnection.Close();
            return maxValue.ToString();
        }

        public static string getPerCreditCost(string dept)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            string Query = "select IsNull(round(costs/credit,2),0)per_credit from dept_info where dept_id='" + dept +
                           "'";
            myConnection.Open();
            SqlCommand myCommand = new SqlCommand(Query, myConnection);
            object maxValue = myCommand.ExecuteScalar();
            myConnection.Close();
            return maxValue.ToString();
        }

        public void SaveInfo(clsDept aDept)
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = @"insert into dept_info (dept_name,abvr,costs,credit,[SEMISTERFEE]
           ,[ADMISSIONFEE]
           ,[NOOFSEMI]
           ,[SEMIHIGHCREDIT]
           ,[PERCREDITCOST]
           ,[AddBy]
           ,[AddDate],Dept_MstID) values ('" + aDept.DeptName + "','" + aDept.Abvr + "','" +
                           aDept.Costs.Replace(",", "") + "','" + aDept.Credit + "','" + aDept.SEMISTERFEE + "','" +
                           aDept.ADMISSIONFEE + "','" + aDept.NOOFSEMI + "','" + aDept.SEMIHIGHCREDIT + "','" +
                           aDept.PERCREDITCOST + "','" + aDept.LoginBy + "',GetDate(),'" + aDept.DeptId + "')";
            DataManager.ExecuteNonQuery(connectionString, query);
        }

        public void UpdateInfo(clsDept aDept)
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = @"update dept_info set dept_name='" + aDept.DeptName + "',abvr='" + aDept.Abvr +
                           "', credit='" + aDept.Credit.Replace(",", "") + "',costs='" + aDept.Costs.Replace(",", "") +
                           "',[SEMISTERFEE] = '" + aDept.SEMISTERFEE.Replace(",", "") + "',[ADMISSIONFEE] = '" +
                           aDept.ADMISSIONFEE.Replace(",", "") + "',[NOOFSEMI] = convert(int,'" + aDept.NOOFSEMI +
                           "'),[SEMIHIGHCREDIT] = '" + aDept.SEMIHIGHCREDIT.Replace(",", "") + "',[PERCREDITCOST] = '" +
                           aDept.PERCREDITCOST.Replace(",", "") + "',[UpdateBy]='" + aDept.LoginBy +
                           "',[UpdateDate]=GetDate(),Dept_MstID='" + aDept.DeptId + "' where dept_id='" + aDept.ID + "'";
            DataManager.ExecuteNonQuery(connectionString, query);
        }

        public DataTable GetDepartmentInfo(string DeptId, string Flag)
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string Parameter = "";
            DataTable dt = null;
            string query = "";
            if (Flag == "DE")
            {
                if (DeptId == "0" || string.IsNullOrEmpty(DeptId))
                {
                    Parameter = "where [DeleteBy] IS NULL order by DEPT_NAME";
                }
                else
                {
                    Parameter = "where [DeleteBy] IS NULL  and ID='" + DeptId + "' order by DEPT_NAME ";
                }
                //string query = "select dept_id,dept_name,abvr,convert(decimal,costs)costs,credit from dept_info order by dept_id";
                query = @"SELECT [ID],[DEPT_CODE] ,[DEPT_NAME] ,[ABVR]  FROM [Department]" + Parameter;
                dt = DataManager.ExecuteQuery(connectionString, query, "Department");
                return dt;
            }
            else
            {
                if (Flag == "SearchDept")
                {
                    Parameter = "where DeleteBy IS NULL  and Dept_MstID='" + DeptId + "' order by dept_name";
                }
                else
                {
                    if (DeptId == "0" || string.IsNullOrEmpty(DeptId))
                    {
                        Parameter = "where DeleteBy IS NULL order by dept_name";
                    }
                    else
                    {
                        Parameter = "where DeleteBy IS NULL  and dept_id='" + DeptId + "' order by dept_name";
                    }
                }
                //string query = "select dept_id,dept_name,abvr,convert(decimal,costs)costs,credit from dept_info order by dept_id";
                query =
                    @"select dept_id,dept_name,abvr,convert(decimal,costs)costs,credit,CONVERT(decimal,[SEMISTERFEE]) SEMISTERFEE,CONVERT(decimal,[ADMISSIONFEE]) ADMISSIONFEE
      ,CONVERT(int,[NOOFSEMI]) NOOFSEMI,CONVERT(decimal,[SEMIHIGHCREDIT]) SEMIHIGHCREDIT,CONVERT(decimal,[PERCREDITCOST]) PERCREDITCOST,Dept_MstID from dept_info " +
                    Parameter;
                
            }
            dt = DataManager.ExecuteQuery(connectionString, query, "Depts");
            return dt;
        }
        public clsDept GetSelectDeptname(string deptId)
        {
            String connectionString = DataManager.OraConnString();
            string query = @"SELECT [DEPT_ID]
      ,[DEPT_NAME]
      ,[ABVR]
      ,[COSTS]
      ,[CREDIT]
      ,[SEMISTERFEE]
      ,[ADMISSIONFEE]
      ,[NOOFSEMI]
      ,[SEMIHIGHCREDIT]
      ,[PERCREDITCOST]
      ,[AddBy]
      ,[AddDate]
      ,[UpdateBy]
      ,[UpdateDate]
      ,[DeleteBy]
      ,[DeleteDate],Dept_MstID
  FROM [DEPT_INFO] where [DEPT_ID]='" + deptId + "' and DeleteBy IS NULL";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "clsDept");
            if (dt.Rows.Count == 0)
            {
                return null;
            }
            return new clsDept(dt.Rows[0]);
        }

        public void DeleteInfo(clsDept dept)
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = @"update dept_info set [DeleteBy]='" + dept.LoginBy +
                           "',[DeleteDate]=GetDate() where [DEPT_ID]='" + dept.DeptId + "'";
            DataManager.ExecuteNonQuery(connectionString, query);
        }


        public DataTable GetDeptCount()
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = @"select COUNT(DEPT_ID) TotalDept from dept_info where DeleteBy IS NULL";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "dept_info");
            return dt;
        }

        //***************************** Insert Department ************************//

        public void SaveDepartment(clsDept aclsDept)
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = @"INSERT INTO [Department]
           ([DEPT_CODE]
           ,[DEPT_NAME]
           ,[ABVR],[AddBy],[AddDate])
               VALUES
           ('" + aclsDept.DeptId + "','" + aclsDept.DeptName + "','" + aclsDept.Abvr + "','" + aclsDept.LoginBy +
                           "',GETDATE())";
            DataManager.ExecuteNonQuery(connectionString, query);
        }

        public void UpdateDepartment(clsDept aclsDept)
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = @"UPDATE [Department]
   SET [DEPT_CODE] ='" + aclsDept.DeptId + "' ,[DEPT_NAME] ='" + aclsDept.DeptName + "',[ABVR] ='" + aclsDept.Abvr +
                           "' ,[UpdateBy] ='" + aclsDept.LoginBy + "' ,[UpdateDate] =GETDATE() WHERE ID='" + aclsDept.ID +
                           "'";
            DataManager.ExecuteNonQuery(connectionString, query);
        }

        public void DeleteDeptname(string ID, string UserID)
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = @"UPDATE [Department]
   SET [DeleteBy] ='" + UserID + "' ,[DeleteDate] =GETDATE() WHERE ID='" + ID + "'";
            DataManager.ExecuteNonQuery(connectionString, query);
        }
    }
}
