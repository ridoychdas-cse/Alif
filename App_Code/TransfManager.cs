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
/// Summary description for TransfManager
/// </summary>
/// 
namespace KHSC
{
    public class TransfManager
    {
        public static void CreateTransf(Transf trn)
        {
            String connectionString = DataManager.OraConnString();
            string query = " insert into pmis_transfer (emp_no,order_no,trans_date,trans_prom,branch_code,desig_code) values (" +
                " " + " '" + trn.EmpNo + "'," + " '" + trn.OrderNo + "'," + " convert('" + trn.TransDate + "','dd/mm/rrrr')," + " '" + trn.TransProm + "', " +
             " " + " '" + trn.BranchCode + "'," + " '" + trn.DesigCode + "')";
            DataManager.ExecuteNonQuery(connectionString, query);
        }
        public static void UpdateTransf(Transf trn)
        {
            String connectionString = DataManager.OraConnString();
            string query = " update pmis_transfer set order_no = '" + trn.OrderNo + "',trans_date= convert('" + trn.TransDate + "','dd/mm/rrrr'),trans_prom= '" + trn.TransProm + "', " +
             " branch_code= '" + trn.BranchCode + "', desig_code = '" + trn.DesigCode + "' where emp_no='" + trn.EmpNo + "' and order_no='" + trn.OrderNo + "'";
            DataManager.ExecuteNonQuery(connectionString, query);
        }
        public static void DeleteTransf(string emp)
        {
            String connectionString = DataManager.OraConnString();
            string query = " delete from pmis_transfer  where emp_no='" + emp + "' ";
            DataManager.ExecuteNonQuery(connectionString, query);
        }
        public static void DeleteTransfer(string emp, string ord)
        {
            String connectionString = DataManager.OraConnString();
            string query = " delete from pmis_transfer  where emp_no='" + emp + "' and order_no='"+ord+"' ";
            DataManager.ExecuteNonQuery(connectionString, query);
        }
        public static Transf getTransf(string empno, string trn)
        {
            String connectionString = DataManager.OraConnString();
            string query = "select EMP_NO, convert(TRANS_DATE,'dd/MM/rrrr')trans_date, TRANS_PROM, BRANCH_CODE, DESIG_CODE, ORDER_NO from pmis_transfer where emp_no='" + empno + "' and rtrim(order_no)= rtrim('" + trn + "') ";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Transfer");
            if (dt.Rows.Count == 0)
            {
                return null;
            }
            return new Transf(dt.Rows[0]);
        }
        public static DataTable getTransfs(string empno)
        {
            String connectionString = DataManager.OraConnString();
            string query = "select EMP_NO, convert(TRANS_DATE,'dd/MM/rrrr')trans_date, TRANS_PROM, BRANCH_CODE, DESIG_CODE, ORDER_NO from pmis_transfer where emp_no='" + empno + "' ";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Transfer");
            return dt;
        }
        public static DataTable getTransfer(string empno)
        {
            String connectionString = DataManager.OraConnString();
            string query = "select EMP_NO, convert(TRANS_DATE,'dd/MM/rrrr')trans_date, decode(TRANS_PROM,'Y','Yes','N','No')trans_prom, "+
                " (select dbo.initcap(branch_name) from pmis_branch where branch_code=a.branch_code)branch_code, "+
                " (select dbo.initcap(desig_name) from pmis_desig_code where desig_code=a.desig_code)desig_code, ORDER_NO from pmis_transfer a where emp_no='" + empno + "' ";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Transfer");
            return dt;
        }
        public static DataTable getTransferRpt(string criteria)
        {
            String connectionString = DataManager.OraConnString();
            string query = "select EMP_NO, convert(TRANS_DATE,'dd/MM/rrrr')trans_date, decode(TRANS_PROM,'Y','Yes','N','No')trans_prom, "+
                " (select dbo.initcap(branch_name) from pmis_branch where branch_code=a.branch_code)branch_code, "+
                " (select dbo.initcap(desig_name) from pmis_desig_code where desig_code=a.desig_code)desig_code, ORDER_NO from pmis_transfer a ";
            if (criteria.Length > 0)
            {
                query += " where " + criteria;
            }
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Transfer");
            return dt;
        }
        public static DataTable getTransRpt(string empno)
        {
            String connectionString = DataManager.OraConnString();
            string query = "select ORDER_NO,convert(TRANS_DATE,'dd/MM/rrrr')trans_date, decode(TRANS_PROM,'Y','Yes','N','No')trans_prom, " +
                " (select dbo.initcap(branch_name) from pmis_branch where branch_code=a.branch_code)branch_code, " +
                " (select dbo.initcap(desig_name) from pmis_desig_code where desig_code=a.desig_code)desig_code from pmis_transfer a where emp_no='" + empno + "' ";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Transfer");
            return dt;
        }
    }
}
