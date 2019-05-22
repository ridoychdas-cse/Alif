using System;
using System.Data;
using System.Configuration;
//using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
//using System.Xml.Linq;
using System.Data.SqlClient;

/// <summary>
/// Summary description for clsPaymentInfoManager
/// </summary>
/// 
namespace KHSC
{
    public class clsPaymentInfoManager
    {
        public static DataTable GetPayments()
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = @"select t1.pay_id,t1.pay_head_id as pay_Name_Id,t2.Head_Name as pay_head_id,t1.pay_type,convert(varchar,t1.pay_class) pay_class,(select class_name from class_info where class_id=pay_class )class,t1.for_all_std,convert(varchar,t1.pay_amt) pay_amt,t1.discount,t1.[version] as[version],v.version_name as[version_name], CASE WHEN GroupID=1 THEN 'Science' WHEN GroupID=2 THEN 'Commerce' WHEN GroupID=3 THEN 'Humanities' else '' end AS[GroupName],GroupID AS [GroupID]  from payment_info t1 left join version_info v on v.id=t1.[version] inner join payment_head t2 on t2.ID=t1.pay_head_id  order by convert(int,pay_id) asc";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Payments");
            return dt;
        }

        public static DataTable GetAdditionalPayments()
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = "select * from payment_info where for_all_std='N' and pay_id not in (select pay_id from std_special_pay)";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Payments");
            return dt;
        }

        public static void UpdatePayment(clsPaymentInfo pay)
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = "update payment_info set pay_head_id='" + pay.PayName + "',pay_type='" + pay.PayType + "',pay_class=convert(numeric,nullif('" + pay.PayClass + "','')),for_all_std='" + pay.ForAllStd + "' ,pay_amt=convert(decimal(13,2),nullif('" + pay.PayAmt + "','')), discount='" + pay.Discount + "',version='" + pay.Version + "',GroupID='" + pay.GroupID + "' where pay_id='" + pay.PayId + "'";
            DataManager.ExecuteNonQuery(connectionString, query);
        }

        public static void DeletePayment(string pay)
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = "delete from payment_info where pay_id='" + pay + "'";
            DataManager.ExecuteNonQuery(connectionString, query);
        }

        public static void CreatePayment(clsPaymentInfo pay)
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = "insert into payment_info (pay_id, pay_head_id,pay_type,pay_class,for_all_std,pay_amt,discount,version,GroupID) values ( '" + pay.PayId + "', '" + pay.PayName + "', '" + pay.PayType + "',convert(numeric,nullif('" + pay.PayClass + "','')),'" + pay.ForAllStd + "', convert(decimal(13,2),nullif('" + pay.PayAmt + "','')),'" + pay.Discount + "','" + pay.Version + "','" + pay.GroupID + "')";
            DataManager.ExecuteNonQuery(connectionString, query);
        }

        public static clsPaymentInfo getPayment(string pay)
        {
            String connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);

            string query = "select pay_id,pay_head_id,pay_type,convert(varchar,pay_class) pay_class,for_all_std,convert(varchar,pay_amt) pay_amt,discount,version,GroupID from payment_info where pay_id='" + pay + "'";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Payment");
            if (dt.Rows.Count == 0)
            {
                return null;
            }
            return new clsPaymentInfo(dt.Rows[0]);
        }

        public static string getPayAmount(string cls, string pay)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            string Query = " select pay_amt from payment_info where coalesce(pay_class,'" + cls + "')='" + cls + "' and pay_id='" + pay + "'";
            myConnection.Open();
            SqlCommand myCommand = new SqlCommand(Query, myConnection);
            object maxValue = myCommand.ExecuteScalar();
            myConnection.Close();
            string a = "";
            if (maxValue != null)
            {
                a = maxValue.ToString();
            }
            if (a == "")
            {
                a = "0";
            }
            return a;
        }

        public static DataTable GetPaymentsOnSearchClassAndVerson(string Class, string Verson)
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = "";
            if (Class != "" && Verson == "")
            {
                query = "select t1.pay_id,t1.pay_head_id as pay_Name_Id,t2.Head_Name as pay_head_id,t1.pay_type,convert(varchar,t1.pay_class) pay_class,t5.class_name as class,t1.for_all_std,convert(varchar,t1.pay_amt) pay_amt,t1.discount,t1.[version] as[version],v.version_name as[version_name] , CASE WHEN GroupID=1 THEN 'Science' WHEN GroupID=2 THEN 'Commerce' WHEN GroupID=3 THEN 'Humanities' else '' end AS[GroupName],GroupID AS [GroupID] from payment_info t1 inner join class_info t5 on t5.class_id=t1.pay_class  inner join version_info v on v.id=t1.[version] inner join payment_head t2 on t2.ID=t1.pay_head_id where t1.pay_class='" + Class + "' order by convert(int,pay_id) asc";
            }
            else if (Class != "" && Verson != "")
            {
                query = "select t1.pay_id,t1.pay_head_id as pay_Name_Id,t2.Head_Name as pay_head_id,t1.pay_type,convert(varchar,t1.pay_class) pay_class,t5.class_name as class,t1.for_all_std,convert(varchar,t1.pay_amt) pay_amt,t1.discount,t1.[version] as[version],v.version_name as[version_name] , CASE WHEN GroupID=1 THEN 'Science' WHEN GroupID=2 THEN 'Commerce' WHEN GroupID=3 THEN 'Humanities' else '' end AS[GroupName],GroupID AS [GroupID] from payment_info t1 inner join class_info t5 on t5.class_id=t1.pay_class  inner join version_info v on v.id=t1.[version] inner join payment_head t2 on t2.ID=t1.pay_head_id where t1.pay_class='" + Class + "' and t1.version='" + Verson + "'  order by convert(int,pay_id) asc";
            }
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Payments");
            return dt;
        }

        public static DataTable GetShowAllPayInfo(string ClassID, string VersonID)
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = @"SELECT t1.[pay_id] AS pay_id  ,t2.Head_Name AS pay_head_id  
            FROM [payment_info] t1 INNER JOIN payment_head t2 on t2.ID=t1.pay_head_id where t1.pay_class='" + ClassID + "' and t1.[version]='" + VersonID + "' order by CONVERT(int, t1.pay_id) asc";            
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Payments");
            return dt;
        }
    }
}