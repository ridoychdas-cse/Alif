using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Summary description for clsStdSpecialPayManager
/// </summary>
/// 
namespace KHSC
{
    public class clsStdSpecialPayManager
    {
        public static void CreateStdSpecialPay(clsStdSpecialPay pay)
        {
            String connectionString = DataManager.OraConnString();
            string query = " insert into std_special_pay(student_id, class_id, class_year, pay_id, pay_amt, from_dt, to_dt,serial_no) values (" +
                " '" + pay.StudentId + "', '" + pay.ClassId + "', '" + pay.ClassYear + "', " +
            " '" + pay.PayId + "','" + pay.PayAmt + "',convert(datetime,'" + pay.FromDt + "',103),convert(datetime,'" + pay.ToDt + "',103),'" + pay.SerialNo + "')";
            DataManager.ExecuteNonQuery(connectionString, query);
        }
        public static void UpdateStdSpecialPay(clsStdSpecialPay pay)
        {
            String connectionString = DataManager.OraConnString();
            string query = " update std_special_pay set pay_amt= '" + pay.PayAmt + "', from_dt=convert(datetime, '" + pay.FromDt + "',103), to_dt= convert(datetime,'" + pay.ToDt + "',103) " +
                " where serial_no='" + pay.SerialNo + "'";
            DataManager.ExecuteNonQuery(connectionString, query);
        }
        public static void DeleteStdSpecialPay(clsStdSpecialPay pay)
        {
            String connectionString = DataManager.OraConnString();
            string query = " delete from std_special_pay where serial_no='" + pay.SerialNo + "'";
            DataManager.ExecuteNonQuery(connectionString, query);
        }
        public static clsStdSpecialPay getStdSpecialPay(string sl)
        {
            String connectionString = DataManager.OraConnString();
            string query = " select student_id, class_id, class_year, pay_id, convert(varchar,pay_amt) pay_amt, convert(varchar,from_dt,103) from_dt, convert(varchar,to_dt,103) to_dt,serial_no from std_special_pay "+
                " where serial_no=convert(numeric,nullif('" + sl + "','')) ";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "BankMaster");
            if (dt.Rows.Count == 0)
            {
                return null;
            }
            return new clsStdSpecialPay(dt.Rows[0]);
        }
        public static DataTable getStdSpecialPays(string std, string cid, string yr)
        {
            String connectionString = DataManager.OraConnString();
            string query = "select student_id, class_id, class_year, pay_id, convert(varchar,pay_amt) pay_amt, convert(varchar,from_dt,103) from_dt, convert(varchar,to_dt,103) to_dt,serial_no from std_special_pay " +
                " where student_id='" + std + "' and class_id= convert(numeric,nullif('" + cid + "','')) and class_year= convert(numeric,nullif('" + yr + "','')) ";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PaymentDtls");
            return dt;
        }
        public static DataTable getStdSpecialPaysGrid(string std)
        {
            String connectionString = DataManager.OraConnString();
            string query = "select a.student_id, f_name+' '+m_name+' '+l_name name, c.class_name, a.class_year, d.pay_head_id, convert(varchar,a.pay_amt) pay_amt, convert(varchar,from_dt,103) from_dt, convert(varchar,to_dt,103) to_dt,serial_no from std_special_pay a, student_info b, class_info c, payment_info d " +
                " where a.student_id=b.student_id and a.class_id=c.class_id and a.pay_id=d.pay_id and a.student_id like '%" + std + "%'  ";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PaymentDtls");
            return dt;
        }
    }
}