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

/// <summary>
/// Summary description for SuspManager
/// </summary>
/// 
namespace KHSC
{
    public class SuspManager
    {
        public static void CreateSusp(Susp susp)
        {
            String connectionString = DataManager.OraConnString();
            string query = " insert into pmis_suspension (emp_no,off_order_no,suspen_date,suspen_clause,withdraw_order_no,with_date,punishment) values (" +
                " " + " '" + susp.EmpNo + "'," + " '" + susp.OffOrderNo + "'," + " convert('" + susp.SuspenDate + "','dd/mm/rrrr')," + " '" + susp.SuspenClause + "', " +
                " " + " '" + susp.WithdrawOrderNo + "'," + " convert('" + susp.WithDate + "','dd/mm/rrrr'), " + " '" + susp.Punishment + "')";
            DataManager.ExecuteNonQuery(connectionString, query);
        }
        public static void UpdateSusp(Susp susp)
        {
            String connectionString = DataManager.OraConnString();
            string query = " update pmis_suspension set off_order_no= '" + susp.OffOrderNo + "',suspen_date= convert('" + susp.SuspenDate + "','dd/mm/rrrr'),suspen_clause= '" + susp.SuspenClause + "', " +
                " withdraw_order_no= '" + susp.WithdrawOrderNo + "',with_date= convert('" + susp.WithDate + "','dd/mm/rrrr'), punishment= '" + susp.Punishment + "' where emp_no='" + susp.EmpNo + "' and off_order_no='" + susp.OffOrderNo + "'";
            DataManager.ExecuteNonQuery(connectionString, query);
        }
        public static void DeleteSusp(string emp)
        {
            String connectionString = DataManager.OraConnString();
            string query = " delete from pmis_suspension where emp_no='" + emp + "' ";
            DataManager.ExecuteNonQuery(connectionString, query);
        }
        public static Susp getSusp(string empno, string susp)
        {
            String connectionString = DataManager.OraConnString();
            string query = "select emp_no, OFF_ORDER_NO, convert(SUSPEN_DATE,'dd/mm/rrrr')suspen_date, SUSPEN_CLAUSE, WITHDRAW_ORDER_NO, convert(WITH_DATE,'dd/mm/rrrr')with_date, PUNISHMENT from pmis_suspension where emp_no='" + empno + "' and rtrim(off_order_no)= rtrim('" + susp + "') ";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Suspension");
            if (dt.Rows.Count == 0)
            {
                return null;
            }
            return new Susp(dt.Rows[0]);
        }
        public static DataTable getSusps(string empno)
        {
            String connectionString = DataManager.OraConnString();
            string query = "select emp_no, OFF_ORDER_NO, convert(SUSPEN_DATE,'dd/mm/rrrr')suspen_date, SUSPEN_CLAUSE, WITHDRAW_ORDER_NO, convert(WITH_DATE,'dd/mm/rrrr')with_date, PUNISHMENT from pmis_suspension where emp_no='" + empno + "' ";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Suspension");
            return dt;
        }
        public static DataTable getSuspsRpt(string criteria)
        {
            String connectionString = DataManager.OraConnString();
            string query = "select emp_no, OFF_ORDER_NO, convert(SUSPEN_DATE,'dd/mm/rrrr')suspen_date, SUSPEN_CLAUSE, WITHDRAW_ORDER_NO, convert(WITH_DATE,'dd/mm/rrrr')with_date, PUNISHMENT from pmis_suspension ";
            if (criteria.Length > 0)
            {
                query += " where " + criteria;
            }
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Suspension");
            return dt;
        }
        public static DataTable getSuspRpt(string empno)
        {
            String connectionString = DataManager.OraConnString();
            string query = "select OFF_ORDER_NO, convert(SUSPEN_DATE,'dd/mm/rrrr')suspen_date, SUSPEN_CLAUSE, WITHDRAW_ORDER_NO, convert(WITH_DATE,'dd/mm/rrrr')with_date, PUNISHMENT from pmis_suspension where emp_no='" + empno + "' ";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Suspension");
            return dt;
        }
    }
    
}