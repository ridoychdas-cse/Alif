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
using System.Collections.Generic;

/// <summary>
/// Summary description for clsPaymentManager
/// </summary>
/// 
namespace KHSC
{
    public class clsPaymentManager
    {
        SqlConnection connection = new SqlConnection(DataManager.OraConnString());
        SqlTransaction transaction;
        public static void CreatePaymentMst(clsPaymentMst pay)
        {
            String connectionString = DataManager.OraConnString();
            string query = " insert into payment_mst(payment_id,student_id,class_id,version,shift,pay_year,pay_date,pay_mode,cheque_no,cheque_date,cheque_amt,bank_no,ref_no,entry_user,sect,entry_date,Pay_Amount,Total_Paid_Amt,DiscountAmt) values (" +
                "  '" + pay.PaymentId + "', '" + pay.StudentId + "', convert(nvarchar,nullif('" + pay.ClassId + "','')),convert(nvarchar,nullif('" + pay.Version + "','')),convert(nvarchar,nullif('" + pay.Shift + "','')), convert(nvarchar,nullif('" + pay.PayYear + "','')), convert(datetime,nullif('" + pay.PayDate + "',''),103), '" + pay.PayMode + "', " +
             "  '" + pay.ChequeNo + "', convert(datetime,nullif('" + pay.ChequeDate + "',''),103), convert(numeric,nullif('" + pay.ChequeAmt + "','')), '" + pay.BankNo + "', '" + pay.RefNo + "', '" + pay.EntryUser + "','" + pay.Section + "', convert(datetime,nullif('" + pay.EntryDate + "',''),103),convert(decimal,nullif('" + pay.PayAmt + "','')),convert(decimal,nullif('" + pay.TotalPaidAmt + "','')),convert(decimal,nullif('" + pay.TotalDiscountAmt + "','')))";
            DataManager.ExecuteNonQuery(connectionString, query);
        }
        public static void UpdatePaymentMst(clsPaymentMst pay)
        {
            String connectionString = DataManager.OraConnString();
            string query = "update payment_mst set student_id= '" + pay.StudentId + "',class_id=convert(nvarchar,nullif('" + pay.ClassId + "','')),shift=convert(nvarchar,nullif('" + pay.Shift + "','')),version=convert(nvarchar,nullif('" + pay.Version + "','')), pay_year= convert(nvarchar,nullif('" + pay.PayYear + "','')),pay_date= convert(datetime,nullif('" + pay.PayDate + "',''),103),pay_mode= '" + pay.PayMode + "', " +
             " cheque_no= '" + pay.ChequeNo + "',cheque_date= convert(datetime,nullif('" + pay.ChequeDate + "',''),103),cheque_amt= convert(decimal(13,2),nullif('" + pay.ChequeAmt + "','')),bank_no='" + pay.BankNo + "', ref_no='" + pay.RefNo + "',sect='" + pay.Section + "', " +
             " update_user= '" + pay.UpdateUser + "',update_date= convert(datetime,nullif('" + pay.UpdateDate + "',''),103), Pay_Amount=convert(decimal(13,2),nullif( '" + pay.PayAmt + "','')), Total_Paid_Amt=convert(decimal(13,2),nullif( '" + pay.TotalPaidAmt + "','')),DiscountAmt=convert(decimal(13,2),nullif( '" + pay.TotalDiscountAmt + "','')) where payment_id='" + pay.PaymentId + "' ";
            DataManager.ExecuteNonQuery(connectionString, query);
        }
        
        public static clsPaymentMst getPaymentMst(string pay)
        {
            String connectionString = DataManager.OraConnString();
            //string query = "select  b.payment_id,student_id,b.class_id,b.Pay_Amount,b.Total_Paid_Amt,b.sect,convert(varchar,b.pay_year)pay_year,convert(varchar,b.pay_date,103)pay_date,b.pay_mode,b.cheque_no,b.cheque_date, " +
            //    " b.cheque_amt,b.bank_no,b.ref_no,b.entry_user,convert(varchar,b.entry_date,103)entry_date,b.update_user,convert(varchar,b.update_date,103)update_date,t2.class_name,t3.sec_name from payment_mst b inner join class_info t2 on t2.class_id=b.class_id inner join section_info t3 on t3.sec_id=b.sect where payment_id='" + pay + "'";
            string query = @"select  b.ID,b.payment_id,b.student_id,b.class_id,b.[version],b.sect,b.shift,t6.std_roll,b.Pay_Amount,b.Total_Paid_Amt,b.DiscountAmt,convert(varchar,b.pay_year)pay_year
,convert(varchar,b.pay_date,103)pay_date,b.pay_mode,b.cheque_no,b.cheque_date
,b.cheque_amt,b.bank_no,b.ref_no,b.entry_user,convert(varchar,b.entry_date,103)entry_date,b.update_user
,convert(varchar,b.update_date,103)update_date,t2.class_name,t5.version_name,t3.sec_name,t4.shift_name
from payment_mst b 
inner join class_info t2 on t2.class_id=b.class_id  
inner join section_info t3 on t3.sec_id=b.sect 
inner join shift_info t4 on t4.id=b.shift 
inner join std_current_status t6 on t6.student_id=b.STUDENT_ID
inner join version_info t5 on t5.id=b.[version] where payment_id = '" + pay + "'";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PaymentMst");
            if (dt.Rows.Count == 0)
            {
                return null;
            }
            return new clsPaymentMst(dt.Rows[0]);
        }
        public static DataTable getPaymentMsts(string pay, string std, string user, string superUser, string year)
        {
            string parametter = "";
            if (superUser != "4")
            {
                parametter = "and ENTRY_USER='" + user + "'";
            }
            String connectionString = DataManager.OraConnString();
            string query = @"select payment_id,student_id,(select dbo.initcap(rtrim(ltrim(f_name+' '+m_name+' '+l_name))) from student_info where student_id=a.student_id)name,(select class_name from class_info where class_id=a.class_id) class_id,convert(varchar,pay_year)pay_year,convert(varchar,pay_date,103)pay_date,cheque_no,case when pay_mode='C' then 'Cash' else 'Cheque' end as [pay_mode],(select SUM(PAY_AMT) from PAYMENT_DTL d where d.PAYMENT_ID=a.PAYMENT_ID ) total_amount,cheque_date,cheque_amt,bank_no,(select bank_name from bank_info where bank_id=a.bank_no)bank_name,ref_no from payment_mst a where payment_id like '%" + pay + "%' and student_id like '%" + std + "%' " + parametter + " and a.pay_year='" + year + "' order by convert(int,payment_id) desc ";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PaymentMsts");
            return dt;
            //String connectionString = DataManager.OraConnString();
            //string query = "select payment_id,student_id,(select dbo.initcap(rtrim(ltrim(f_name+' '+m_name+' '+l_name))) from student_info where student_id=a.student_id)name,(select class_name from class_info where class_id=a.class_id) class_id,convert(varchar,pay_year)pay_year,convert(varchar,pay_date,103)pay_date,pay_mode,cheque_no,cheque_date,cheque_amt,bank_no, " +
            //    " (select bank_name from bank_info where bank_id=a.bank_no)bank_name,ref_no from payment_mst a where payment_id like '%" + pay + "%' and student_id like '%" + std + "%' order by convert(datetime,pay_date,103) desc ";
            //DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PaymentMsts");
            //return dt;
        }
        public static DataTable getPaymentMsts(string pay,string std)
        {

            String connectionString = DataManager.OraConnString();
            string query = @"select payment_id,student_id,(select dbo.initcap(rtrim(ltrim(f_name+' '+m_name+' '+l_name))) from student_info where student_id=a.student_id)name,(select class_name from class_info where class_id=a.class_id) class_id,convert(varchar,pay_year)pay_year,convert(varchar,pay_date,103)pay_date,cheque_no,case when pay_mode='C' then 'Cash' else 'Cheque' end as [pay_mode],(select SUM(PAY_AMT) from PAYMENT_DTL d where d.PAYMENT_ID=a.PAYMENT_ID ) total_amount,cheque_date,cheque_amt,bank_no,(select bank_name from bank_info where bank_id=a.bank_no)bank_name,ref_no from payment_mst a where payment_id like '%" + pay + "%' and student_id like '%" + std + "%' order by payment_id desc ";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PaymentMsts");
            return dt;
            //String connectionString = DataManager.OraConnString();
            //string query = "select payment_id,student_id,(select dbo.initcap(rtrim(ltrim(f_name+' '+m_name+' '+l_name))) from student_info where student_id=a.student_id)name,(select class_name from class_info where class_id=a.class_id) class_id,convert(varchar,pay_year)pay_year,convert(varchar,pay_date,103)pay_date,pay_mode,cheque_no,cheque_date,cheque_amt,bank_no, " +
            //    " (select bank_name from bank_info where bank_id=a.bank_no)bank_name,ref_no from payment_mst a where payment_id like '%" + pay + "%' and student_id like '%" + std + "%' order by convert(datetime,pay_date,103) desc ";
            //DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PaymentMsts");
            //return dt;
        }

        public static void SaveStudentTransportInformation(List<StudentTransportMst> TransportStudentlist)
        {
            String connectionString = DataManager.OraConnString();
             foreach (StudentTransportMst TransportList in TransportStudentlist)
            {
            string query = @"";             
            DataManager.ExecuteNonQuery(connectionString, query);
             }            
        }
        public static void UpdateStudentTransportInformation(List<StudentTransportMst> TransportStudentlist)
        {
            String connectionString = DataManager.OraConnString();
            foreach (StudentTransportMst TransportList in TransportStudentlist)
            {
                string query = @"";

                DataManager.ExecuteNonQuery(connectionString, query);
            }


        }
        public static void CreatePaymentDtl(clsPaymentDtl pay)
        {
            String connectionString = DataManager.OraConnString();
            string query = " insert into payment_dtl(payment_id,pay_id,pay_amt,Discount_AMT) values (" +
                "  '" + pay.PaymentId + "', '" + pay.PayId + "', convert(decimal(13,2),nullif('" + pay.PayAmt + "','')),convert(decimal(13,2),nullif('" + pay.Discount_AMT + "','')))";
            DataManager.ExecuteNonQuery(connectionString, query);
        }
        
        public  void DeletePaymentDtls(string pay)
        {
            string connectionString = DataManager.OraConnString();
            try
            {
                connection.Open();

                transaction = connection.BeginTransaction();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                SqlCommand command1 = new SqlCommand();
                command1.Connection = connection;
                command1.Transaction = transaction;

                SqlCommand command2 = new SqlCommand();
                command2.Connection = connection;
                command2.Transaction = transaction;

                SqlCommand command3 = new SqlCommand();
                command3.Connection = connection;
                command3.Transaction = transaction;

                SqlCommand command4 = new SqlCommand();
                command4.Connection = connection;
                command4.Transaction = transaction;               

                command4.CommandText = @"SELECT COUNT(*) FROM [GL_TRANS_MST] where [SERIAL_NO]='" + pay + "' and PAYEE='ST_Pay' ";
                int AdCount = Convert.ToInt32(command4.ExecuteScalar());
                if (AdCount > 0)
                {
                    string Query = @"SELECT [VCH_SYS_NO] FROM [GL_TRANS_MST] where [SERIAL_NO]='" + pay + "' and PAYEE='ST_Pay' ";
                    DataTable table = DataManager.ExecuteQuery(connectionString, Query, "GL_TRANS_MST");
                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow dr in table.Rows)
                        {
                            command.CommandText = @"DELETE FROM [GL_TRANS_DTL] WHERE VCH_SYS_NO='" + dr["VCH_SYS_NO"] + "'";
                            command.ExecuteNonQuery();
                        }
                    }
                    command1.CommandText = @"DELETE FROM [GL_TRANS_MST]  WHERE  [SERIAL_NO]='" + pay + "' ";
                    command1.ExecuteNonQuery();
                }

                command2.CommandText = @"delete from payment_dtl where payment_id='" + pay + "'";
                command2.ExecuteNonQuery();

                command3.CommandText = @"delete from payment_mst where payment_id='" + pay + "'";
                command3.ExecuteNonQuery();

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }        
           
        }
        public static clsPaymentMst getPaymentDtl(string paymnt,string payid)
        {
            String connectionString = DataManager.OraConnString();
            string query = "select payment_id,pay_id,convert(varchar,pay_amt)pay_amt from payment_mst where payment_id='" + paymnt + "' and pay_id='"+payid+"'";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PaymentDtl");
            if (dt.Rows.Count == 0)
            {
                return null;
            }
            return new clsPaymentMst(dt.Rows[0]);
        }
        public static DataTable getPaymentDtlStd(string std)
        {
            String connectionString = DataManager.OraConnString();
            string query = "select  a.pay_date,a.payment_id,b.pay_id,(select pay_head_id from payment_info where pay_id=b.pay_id)pay_head_id, " +
            " b.pay_amt from payment_mst a, payment_dtl b where a.payment_id=b.payment_id " +
            " and a.student_id='" + std + "' order by a.pay_date";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PaymentDtls");
            return dt;
        }

        //public static DataTable getStudentDtl(string std, string year)
        //{
        //    String connectionString = DataManager.OraConnString();
        //    string query = @"select  c.student_id,a.pay_date,d.f_name,e.class_name,f.sec_name,a.PAY_YEAR,c.std_roll, a.payment_id,b.pay_id,(select pay_head_id from payment_info where pay_id=b.pay_id)pay_head_id, b.pay_amt from payment_mst a, payment_dtl b ,std_current_status c,student_info d,class_info e,section_info f where a.payment_id=b.payment_id  and c.student_id=a.STUDENT_ID and d.student_id=c.student_id and e.class_id=c.class_id and f.class_id=e.class_id and a.student_id='" + std + "' and a.PAY_YEAR='" + year + "' order by a.pay_date";
        //    DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PaymentDtls");
        //    return dt;
        //}

        public static DataTable getPaymentDtlStd(string std, string year)
        {
            String connectionString = DataManager.OraConnString();
            string query = @"select  a.student_id,Convert(varchar,a.pay_date,103) as pay_date,d.f_name,e.class_name,f.sec_name,a.PAY_YEAR,c.std_roll, a.payment_id,b.pay_id,
(select pay_head_id from payment_info where pay_id=b.pay_id)pay_head_id, 
b.pay_amt 
from payment_mst a 
inner join payment_dtl b on b.PAYMENT_ID=a.PAYMENT_ID
inner join std_current_status c on c.student_id=a.STUDENT_ID
inner join student_info d on d.student_id=c.student_id
inner join class_info e on e.class_id=c.class_id
inner join section_info f on  c.sect=f.sec_id
where  a.student_id='" +std+"' and a.PAY_YEAR='"+year+"' order by a.pay_date";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PaymentDtls");
            return dt;
        }
        
        public static DataTable getPaymentDtls(string payID,string StudentId,string Year)
        {
            String connectionString = DataManager.OraConnString();

            string query1 = @"select a.payment_id,a.pay_id,t1.Head_Name as pay_head_id,a.pay_amt,a. Discount_AMT ,t3.pay_type as[OtCharge_flag],case when t3.pay_type='E' then a.PAY_AMT else case when t3.pay_type='M' then t3.pay_amt * (DATEDIFF(MONTH,CONVERT(date,si.class_start,103),CONVERT(date,pms.PAY_DATE,103))+1) else t3.pay_amt end  end as[Fix_Amt],ISNULL(pay.paid_amt,0) as Previous_Pay,ISNULL(pay.Discount_AMT,0) Prv_Discount_AMT
 from payment_dtl a inner join PAYMENT_MST pms on pms.PAYMENT_ID=a.PAYMENT_ID  inner join std_current_status si on si.student_id=pms.STUDENT_ID  inner join payment_info t3 on t3.pay_id=a.PAY_ID  and a.PAYMENT_ID= " + payID + " inner join payment_head t1 on t1.ID=t3.pay_head_id    left join (select pd.pay_id,sum(pay_amt) paid_amt,sum(pd.Discount_AMT) Discount_AMT from payment_mst pm inner join payment_dtl pd  on (pm.payment_id=pd.payment_id and student_id='" + StudentId + "' and pm.PAY_YEAR='" + Year + "'  and pm.PAYMENT_ID< " + payID + " ) group by class_id,pay_year,pd.pay_id) pay on  a.pay_id=pay.pay_id";

            //            string query = @"select a.payment_id,a.pay_id,t1.Head_Name as pay_head_id,a.pay_amt,a. Discount_AMT +ISNULL(pay.Discount_AMT,0) Discount_AMT,t3.pay_type as[OtCharge_flag],case when t3.pay_type='E' then a.PAY_AMT else t3.pay_amt  end as[Fix_Amt],ISNULL(pay.paid_amt,0) as Previous_Pay
            // from payment_dtl a inner join payment_info t3 on t3.pay_id=a.PAY_ID  and a.PAYMENT_ID=" + payID + " inner join payment_head t1 on t1.ID=t3.pay_head_id left join (select pd.pay_id,sum(pay_amt) paid_amt,sum(pd.Discount_AMT) Discount_AMT from payment_mst pm inner join payment_dtl pd  on (pm.payment_id=pd.payment_id and student_id='" + StudentId + "' and pm.PAY_YEAR='" + Year + "'  and pm.PAYMENT_ID<" + payID + ") group by class_id,pay_year,pd.pay_id) pay on  a.pay_id=pay.pay_id";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query1, "PaymentDtls");
            return dt;
        }
        public static DataTable getStdTotalPayment(string clsid)
        {
            //String connectionString = DataManager.OraConnString();
            //string query = "select b.student_id,dbo.initcap(rtrim(ltrim(f_name+' '+m_name+' '+l_name))) sname,b.class_id,d.shift,d.sect,sum(pay_amt) pay_amt " +
            //" from  payment_dtl a, payment_mst b,student_info c,std_current_status d where a.payment_id=b.payment_id and b.student_id=c.student_id " +
            //" and b.student_id=d.student_id and b.class_id=d.class_id and b.class_id='" + clsid + "' " +
            //" group by b.student_id,dbo.initcap(rtrim(ltrim(f_name+' '+m_name+' '+l_name))),b.class_id,d.shift,d.sect " +
            //" order by 1,2";
            //DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PaymentDtls");
            //return dt;

            String connectionString = DataManager.OraConnString();
            string query = "select b.student_id,dbo.initcap(rtrim(ltrim(f_name+' '+m_name+' '+l_name))) sname,(select class_name from class_info cl where cl.class_id= b.class_id)class_id,d.shift,(select sec_name from section_info se where se.sec_id=d.sect)sect,sum(pay_amt) pay_amt " +
            " from  payment_dtl a, payment_mst b,student_info c,std_current_status d where a.payment_id=b.payment_id and b.student_id=c.student_id " +
            " and b.student_id=d.student_id and b.class_id=d.class_id and b.class_id='" + clsid + "' " +
            " group by b.student_id,dbo.initcap(rtrim(ltrim(f_name+' '+m_name+' '+l_name))), b.class_id,d.shift,d.sect " +
            " order by 1,2";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PaymentDtls");
            return dt;
        }

        public static DataTable getStdClassPaymentDtls(string std,string clsid,string yr)
        {
            String connectionString = DataManager.OraConnString();
            string query = "select a.payment_id,convert(varchar,pay_date,103)pay_date,sum(pay_amt)pay_amt from payment_mst a,payment_dtl b where a.payment_id=b.payment_id " +
            " and student_id='" + std + "' and convert(varchar,class_id)='" + clsid + "' and convert(varchar,pay_year)='" + yr + "' group by a.payment_id,convert(varchar,pay_date,103) order by convert(varchar,pay_date,103)";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PaymentDtls");
            return dt;
        }
        public static DataTable getStdPaymentDtls(string std,string clsid)
        {
            String connectionString = DataManager.OraConnString();
            string query = "select a.payment_id,a.REF_NO,convert(varchar,pay_date,103)pay_date,sum(pay_amt)pay_amt from payment_mst a,payment_dtl b where a.payment_id=b.payment_id " +
            " and student_id='" + std + "' and convert(varchar,class_id)='" + clsid + "' group by a.payment_id,a.REF_NO,convert(varchar,pay_date,103) order by convert(varchar,pay_date,103)";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PaymentDtls");
            return dt;
        }

        public static DataTable getPaymentMstStds(string std)
        {
            String connectionString = DataManager.OraConnString();
            string query = "select distinct payment_id,pay_date,class_id,pay_year from payment_mst a where payment_id in (select payment_id from payment_dtl where student_id ='" + std + "') order by pay_date ";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PaymentDtls");
            return dt;
        }
        public static DataTable getPaymentMstStd(string std,string cls)
        {
            String connectionString = DataManager.OraConnString();
            string query = "select distinct payment_id,pay_date,(select b.class_name from class_info b where b.class_id=a.class_id)class_id,pay_year from payment_mst a where class_id='" + cls + "' and payment_id in (select payment_id from payment_dtl where student_id ='" + std + "') order by pay_date ";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PaymentDtls");
            return dt;
        }
        public static DataTable getPaymentDtlStds(string pay)
        {
            String connectionString = DataManager.OraConnString();
            string query = "select payment_id,(select convert(varchar,pay_date,103) from payment_mst where payment_id=a.payment_id)pay_date,pay_id,(select pay_head_id from payment_info where pay_id=a.pay_id)pay_head_id,convert(varchar,pay_amt)pay_amt " +
                " from payment_dtl a where payment_id ='" + pay + "' ";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PaymentDtls");
            return dt;
        }
        public static DataTable getStudentInfo(string std)
        {
            String connectionString = DataManager.OraConnString();
//            string query = @"select student_id,name,  class_id,class_year,sect,std_roll,[version],shift,class_name,sec_name,shift_name,version_name,group_name
//from (select a.student_id,dbo.initcap(rtrim(ltrim(f_name+' '+m_name+' '+l_name))) name
//,  max(a.class_id)class_id,a.sect,a.std_roll, max(a.class_year)class_year,a.[version],a.shift,c.class_name,sc.sec_name,sh.shift_name,vv.version_name,a.group_name
//from std_current_status a inner join student_info b on a.student_id=b.student_id inner join class_info c on c.class_id=a.class_id left join section_info sc on sc.sec_id=a.sect inner join shift_info sh on sh.shift_id=a.shift left join version_info vv on vv.id=a.[version] where  a.student_id='" + std + "' and b.status=1 group by a.student_id,a.sect,a.std_roll,a.[version],a.shift,dbo.initcap(rtrim(ltrim(f_name+' '+m_name+'  '+l_name))),c.class_name,sc.sec_name,sh.shift_name,vv.version_name,a.group_name) x";
            string query = @"select student_id,name,  class_id,class_year,sect,std_roll,[version],shift,class_name,sec_name,shift_name,version_name,group_name
from (select a.student_id,dbo.initcap(rtrim(ltrim(f_name+' '+m_name+' '+l_name))) name
,  max(a.class_id)class_id,a.sect,a.std_roll, max(a.class_year)class_year,a.[version],a.shift,c.class_name,sc.sec_name,sh.shift_name,vv.version_name,a.group_name
from std_current_status a inner join student_info b on a.student_id=b.student_id inner join class_info c on c.class_id=a.class_id left join section_info sc on sc.sec_id=a.sect left join shift_info sh on sh.shift_id=a.shift left join version_info vv on vv.id=a.[version] where  a.student_id='" + std + "' group by a.student_id,a.sect,a.std_roll,a.[version],a.shift,dbo.initcap(rtrim(ltrim(f_name+' '+m_name+' '+l_name))),c.class_name,sc.sec_name,sh.shift_name,vv.version_name,a.group_name) x";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PaymentDtls");
            return dt;
        }
        public static DataTable getDailyCollection(string sd,string ed,string usr)
        {
            String connectionString = DataManager.OraConnString();
            string query = "select a.payment_id,convert(varchar,pay_date,103) pay_date,student_id, " +
            " (select dbo.initcap(rtrim(ltrim(f_name+' '+m_name+' '+l_name))) from student_info where student_id=a.student_id) std_name,(select c.class_name  from class_info c  where c.class_id=a.class_id) std_class, " +
            " (select pay_head_id from payment_info where pay_id=b.pay_id) pay_head_id, " +
            " round(b.pay_amt,2) pay_amt from payment_mst a, payment_dtl b where " +
            " a.payment_id=b.payment_id and upper(a.entry_user) like upper('%" + usr + "%') and pay_date between convert(datetime,'" + sd + "',103) and convert(datetime,'" + ed + "',103) " +
            " order by pay_date desc, payment_id desc";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PaymentDtls");
            return dt;
        }

        public static DataTable getDailyCollectionSummary(string sd, string ed)
        {
//            String connectionString = DataManager.OraConnString();
//            string query = @"  with tt as (
// SELECT  st.std_roll, b.student_id,si.f_name as std_name,ci.class_id,ci.class_name as std_class,se.sec_name,sh.shift_name,vi.version_name,pay.paid_amt as[Prv_Amt],
//(select isnull(SUM(pay_amt),0) from payment_info where (pay_class is null OR pay_class=b.class_id) and (pay_type='Y' or (for_all_std='Y' and pay_type = case when st.previous_last_class='' OR st.previous_last_class IS NULL  then 'N' else 'O'end  )))+((select isnull(SUM(pay_amt),0) from payment_info where(pay_class is null OR pay_class=b.class_id) and pay_type='M') *  case when (DATEDIFF(MONTH,CONVERT(date,st.class_start,103),CONVERT(date,GETDATE(),103))+1)<0 then 0 else  (DATEDIFF(MONTH,CONVERT(date,st.class_start,103),CONVERT(date,GETDATE(),103))+1) end ) pay_amt,
//sum(isnull(a.Discount_AMT,0))+isnull(pay.Discount_AMT,0) Discount_AMT,sum(round(a.pay_amt,2)) Paid  ,
//
//Isnull(((select isnull( ss.WAIVE_PCT,0) * case when  ( DATEDIFF(MONTH, case when CONVERT(date,ss.EXC_FROM,103) > CONVERT(date,st.class_start,103) then CONVERT(date,ss.EXC_FROM,103) else  CONVERT(date,st.class_start,103) end, case when CONVERT(date,ss.EXC_TO,103) < CONVERT(date,GETDATE(),103) then CONVERT(date,ss.EXC_TO,103) else CONVERT(date,GETDATE(),103) end )+1) <0 then 0 else  ( DATEDIFF(MONTH, case when CONVERT(date,ss.EXC_FROM,103) > CONVERT(date,st.class_start,103) then CONVERT(date,ss.EXC_FROM,103) else  CONVERT(date,st.class_start,103) end, case when CONVERT(date,ss.EXC_TO,103) < CONVERT(date,GETDATE(),103) then CONVERT(date,ss.EXC_TO,103) else CONVERT(date,GETDATE(),103) end )+1) end 
//from STD_WAIVER ss where( ss.STUDENT_ID=st.student_id) and ss.WAIVE_YEAR=st.class_year)),0) WV_Amt
//     from PAYMENT_DTL a           
//    inner join PAYMENT_MST b on b.PAYMENT_ID=a.PAYMENT_ID and b.PAY_DATE   between convert(datetime,'" + sd + "',103) and     convert(datetime,'" + ed + "',103) and upper(b.entry_user) like upper('%" + usr + "%') inner join class_info ci on ci.class_id =b.class_id    inner join student_info si on si.student_id=b.STUDENT_ID inner join std_current_status st on st.student_id=si.student_id   inner join payment_info pf on pf.pay_id=a.PAY_ID  inner join section_info se on se.sec_id=b.sect   inner join shift_info sh on sh.shift_id=b.shift  inner join version_info vi on vi.version_id=b.[version]  left join (select student_id,sum(pay_amt) paid_amt,sum(pd.Discount_AMT) Discount_AMT,pm.PAY_YEAR from payment_mst pm inner join payment_dtl pd  on pm.PAYMENT_ID=pd.PAYMENT_ID  and pm.PAY_DATE<convert(datetime,'" + sd + "',103) group by student_id,pm.PAY_YEAR) pay on pay.STUDENT_ID=b.STUDENT_ID and b.PAY_YEAR=pay.PAY_YEAR        group by st.std_roll,st.class_year, b.student_id,si.f_name,ci.class_name, b.class_id,st.previous_last_class,se.sec_name,st.class_start,sh.shift_name,st.student_id,ci.class_id, vi.version_name,pay.paid_amt,pay.Discount_AMT) select tt.std_roll, tt.STUDENT_ID as[student_id],tt.std_name as [std_name],tt.std_class as std_class,tt.class_id,tt.WV_Amt, tt.sec_name as sec_name,tt.shift_name as shift_name,tt.version_name as version_name,tt.pay_amt as pay_amt,isnull(tt.Discount_AMT,0) as Discount_AMT,isnull(tt.Prv_Amt,0) Prv_Amt,isnull(tt.pay_amt,0)-isnull(tt.Discount_AMT,0)-isnull(tt.Prv_Amt,0) as[GrandTotal],tt.Paid,(isnull(tt.pay_amt,0)-isnull(tt.Discount_AMT,0))-isnull(tt.Paid,0)-isnull(tt.Prv_Amt,0)-ISNULL(tt.WV_Amt,0) as[DueAmt] from tt order by  Convert(int,tt.class_id),Convert(int,tt.std_roll)";
//            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PaymentDtls");
//            return dt;

            SqlConnection conn = new SqlConnection(DataManager.OraConnString());
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = new SqlCommand("SP_AccStudentTransactionDateWise", conn);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;


            da.SelectCommand.Parameters.AddWithValue("@StartDate", sd);
            da.SelectCommand.Parameters.AddWithValue("@EndDate", ed);
            //if (year != "")
            //{
            //    da.SelectCommand.Parameters.AddWithValue("@Year", year);
            //}


            //if (Shift != "")
            //{
            //    da.SelectCommand.Parameters.AddWithValue("@Shift", Shift);
            //}
            //if (Version != "")
            //{
            //    da.SelectCommand.Parameters.AddWithValue("@verson", Version);
            //}

            DataSet ds = new DataSet();
            da.Fill(ds, "SP_AccStudentTransactionDateWise");
            ds.Tables[0].TableName = "SP_AccStudentTransactionDateWise";
            return ds.Tables[0];
        }

        public static DataTable getDailyCollectionClassWiseSummary(string sd, string ed, string year, string ClassId)
        {
//            String connectionString = DataManager.OraConnString();
//            string query = @"  with tt as (
// SELECT  st.std_roll, b.student_id,ci.class_id,si.f_name as std_name,ci.class_name as std_class,se.sec_name,sh.shift_name,vi.version_name,pay.paid_amt as[Prv_Amt],
//(select isnull(SUM(pay_amt),0) from payment_info where (pay_class is null OR pay_class=b.class_id) and (pay_type='Y' or (for_all_std='Y' and pay_type = case when st.previous_last_class='' OR st.previous_last_class IS NULL  then 'N' else 'O'end  )))+((select isnull(SUM(pay_amt),0) from payment_info where(pay_class is null OR pay_class=b.class_id) and pay_type='M') *  case when (DATEDIFF(MONTH,CONVERT(date,st.class_start,103),CONVERT(date,GETDATE(),103))+1)<0 then 0 else  (DATEDIFF(MONTH,CONVERT(date,st.class_start,103),CONVERT(date,GETDATE(),103))+1) end ) pay_amt,
//
//sum(isnull(a.Discount_AMT,0))+isnull(pay.Discount_AMT,0) Discount_AMT,sum(round(a.pay_amt,2)) Paid  ,
//
//Isnull(((select isnull( ss.WAIVE_PCT,0) * case when  ( DATEDIFF(MONTH, case when CONVERT(date,ss.EXC_FROM,103) > CONVERT(date,st.class_start,103) then CONVERT(date,ss.EXC_FROM,103) else  CONVERT(date,st.class_start,103) end, case when CONVERT(date,ss.EXC_TO,103) < CONVERT(date,GETDATE(),103) then CONVERT(date,ss.EXC_TO,103) else CONVERT(date,GETDATE(),103) end )+1) <0 then 0 else  ( DATEDIFF(MONTH, case when CONVERT(date,ss.EXC_FROM,103) > CONVERT(date,st.class_start,103) then CONVERT(date,ss.EXC_FROM,103) else  CONVERT(date,st.class_start,103) end, case when CONVERT(date,ss.EXC_TO,103) < CONVERT(date,GETDATE(),103) then CONVERT(date,ss.EXC_TO,103) else CONVERT(date,GETDATE(),103) end )+1) end 
//from STD_WAIVER ss where( ss.STUDENT_ID=st.student_id) and ss.WAIVE_YEAR=st.class_year) ),0) WV_Amt
//     from PAYMENT_DTL a           
//    inner join PAYMENT_MST b on b.PAYMENT_ID=a.PAYMENT_ID and b.PAY_DATE   between convert(datetime,'" + sd + "',103) and     convert(datetime,'" + ed + "',103) and upper(b.entry_user) like upper('%" + usr + "%') inner join class_info ci on ci.class_id =b.class_id and b.class_id='" + ClassId + "'  inner join student_info si on si.student_id=b.STUDENT_ID inner join std_current_status st on st.student_id=si.student_id   inner join payment_info pf on pf.pay_id=a.PAY_ID  inner join section_info se on se.sec_id=b.sect   inner join shift_info sh on sh.shift_id=b.shift  inner join version_info vi on vi.version_id=b.[version]  left join (select student_id,sum(pay_amt) paid_amt,sum(pd.Discount_AMT) Discount_AMT,pm.PAY_YEAR from payment_mst pm inner join payment_dtl pd  on pm.PAYMENT_ID=pd.PAYMENT_ID  and pm.PAY_DATE<convert(datetime,'" + sd + "',103) group by student_id,pm.PAY_YEAR) pay on pay.STUDENT_ID=b.STUDENT_ID and b.PAY_YEAR=pay.PAY_YEAR        group by st.std_roll,st.class_year, b.student_id,si.f_name,ci.class_name, b.class_id,ci.class_id,st.previous_last_class,se.sec_name,st.class_start,sh.shift_name,st.student_id, vi.version_name,pay.paid_amt,pay.Discount_AMT) select tt.std_roll, tt.STUDENT_ID as[student_id],tt.std_name as [std_name],tt.std_class as std_class,tt.WV_Amt, tt.sec_name as sec_name,tt.shift_name as shift_name,tt.version_name as version_name,tt.class_id,tt.pay_amt as pay_amt,isnull(tt.Discount_AMT,0) as Discount_AMT,isnull(tt.Prv_Amt,0) Prv_Amt,isnull(tt.pay_amt,0)-isnull(tt.Discount_AMT,0)-isnull(tt.Prv_Amt,0) as[GrandTotal],tt.Paid,(isnull(tt.pay_amt,0)-isnull(tt.Discount_AMT,0))-isnull(tt.Paid,0)-isnull(tt.Prv_Amt,0)-ISNULL(tt.WV_Amt,0) as[DueAmt] from tt order by  Convert(int,tt.class_id),Convert(int,tt.std_roll)";
//            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PaymentDtls");
//            return dt;

            SqlConnection conn = new SqlConnection(DataManager.OraConnString());
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = new SqlCommand("SP_AccStudentTransactionDateWise", conn);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;


            da.SelectCommand.Parameters.AddWithValue("@StartDate", sd);
            da.SelectCommand.Parameters.AddWithValue("@EndDate", ed);
            if (year != "")
            {
                da.SelectCommand.Parameters.AddWithValue("@Year", year);
            }
            if (ClassId != "")
            {
                da.SelectCommand.Parameters.AddWithValue("@class", ClassId);
            }
            //if (Shift != "")
            //{
            //    da.SelectCommand.Parameters.AddWithValue("@Shift", Shift);
            //}
            //if (Version != "")
            //{
            //    da.SelectCommand.Parameters.AddWithValue("@verson", Version);
            //}

            DataSet ds = new DataSet();
            da.Fill(ds, "SP_AccStudentTransactionDateWise");
            ds.Tables[0].TableName = "SP_AccStudentTransactionDateWise";
            return ds.Tables[0];
        }

        public static DataTable getDailyCollectionClassAndSectionWiseSummary(string sd, string ed, string year, string ClassId, string Sec)
        {
//            String connectionString = DataManager.OraConnString();
//            string query = @"  with tt as (
// SELECT  st.std_roll, b.student_id,ci.class_id,si.f_name as std_name,ci.class_name as std_class,se.sec_name,sh.shift_name,vi.version_name,pay.paid_amt as[Prv_Amt],
//(select isnull(SUM(pay_amt),0) from payment_info where (pay_class is null OR pay_class=b.class_id) and (pay_type='Y' or (for_all_std='Y' and pay_type = case when st.previous_last_class='' OR st.previous_last_class IS NULL  then 'N' else 'O'end  )))+((select isnull(SUM(pay_amt),0) from payment_info where(pay_class is null OR pay_class=b.class_id) and pay_type='M') *  case when (DATEDIFF(MONTH,CONVERT(date,st.class_start,103),CONVERT(date,GETDATE(),103))+1)<0 then 0 else  (DATEDIFF(MONTH,CONVERT(date,st.class_start,103),CONVERT(date,GETDATE(),103))+1) end ) pay_amt,
//sum(isnull(a.Discount_AMT,0))+isnull(pay.Discount_AMT,0) Discount_AMT,sum(round(a.pay_amt,2)) Paid  ,
//
//Isnull(((select isnull( ss.WAIVE_PCT,0) * case when  ( DATEDIFF(MONTH, case when CONVERT(date,ss.EXC_FROM,103) > CONVERT(date,st.class_start,103) then CONVERT(date,ss.EXC_FROM,103) else  CONVERT(date,st.class_start,103) end, case when CONVERT(date,ss.EXC_TO,103) < CONVERT(date,GETDATE(),103) then CONVERT(date,ss.EXC_TO,103) else CONVERT(date,GETDATE(),103) end )+1) <0 then 0 else  ( DATEDIFF(MONTH, case when CONVERT(date,ss.EXC_FROM,103) > CONVERT(date,st.class_start,103) then CONVERT(date,ss.EXC_FROM,103) else  CONVERT(date,st.class_start,103) end, case when CONVERT(date,ss.EXC_TO,103) < CONVERT(date,GETDATE(),103) then CONVERT(date,ss.EXC_TO,103) else CONVERT(date,GETDATE(),103) end )+1) end 
//from STD_WAIVER ss where( ss.STUDENT_ID=st.student_id) and ss.WAIVE_YEAR=st.class_year) ),0) WV_Amt
//     from PAYMENT_DTL a           
//    inner join PAYMENT_MST b on b.PAYMENT_ID=a.PAYMENT_ID and b.PAY_DATE   between convert(datetime,'" + sd + "',103) and     convert(datetime,'" + ed + "',103) and upper(b.entry_user) like upper('%" + usr + "%') inner join class_info ci on ci.class_id =b.class_id and b.class_id='" + ClassId + "'  inner join student_info si on si.student_id=b.STUDENT_ID inner join std_current_status st on st.student_id=si.student_id   inner join payment_info pf on pf.pay_id=a.PAY_ID  inner join section_info se on se.sec_id=b.sect and b.sect='" + Sec + "'  inner join shift_info sh on sh.shift_id=b.shift  inner join version_info vi on vi.version_id=b.[version]  left join (select student_id,sum(pay_amt) paid_amt,sum(pd.Discount_AMT) Discount_AMT,pm.PAY_YEAR from payment_mst pm inner join payment_dtl pd  on pm.PAYMENT_ID=pd.PAYMENT_ID  and pm.PAY_DATE<convert(datetime,'" + sd + "',103) group by student_id,pm.PAY_YEAR) pay on pay.STUDENT_ID=b.STUDENT_ID and b.PAY_YEAR=pay.PAY_YEAR        group by st.std_roll,st.class_year, b.student_id,si.f_name,ci.class_id,ci.class_name, b.class_id,st.previous_last_class,se.sec_name,st.class_start,sh.shift_name,st.student_id, vi.version_name,pay.paid_amt,pay.Discount_AMT) select tt.std_roll, tt.STUDENT_ID as[student_id],tt.std_name as [std_name],tt.std_class as std_class,tt.WV_Amt, tt.sec_name as sec_name,tt.shift_name as shift_name,tt.version_name as version_name,tt.class_id,tt.pay_amt as pay_amt,isnull(tt.Discount_AMT,0) as Discount_AMT,isnull(tt.Prv_Amt,0) Prv_Amt,isnull(tt.pay_amt,0)-isnull(tt.Discount_AMT,0)-isnull(tt.Prv_Amt,0) as[GrandTotal],tt.Paid,(isnull(tt.pay_amt,0)-isnull(tt.Discount_AMT,0))-isnull(tt.Paid,0)-isnull(tt.Prv_Amt,0)-ISNULL(tt.WV_Amt,0) as[DueAmt] from tt order by  Convert(int,tt.class_id),Convert(int,tt.std_roll)";
//            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PaymentDtls");
//            return dt;
            SqlConnection conn = new SqlConnection(DataManager.OraConnString());
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = new SqlCommand("SP_AccStudentTransactionDateWise", conn);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;


            da.SelectCommand.Parameters.AddWithValue("@StartDate", sd);
            da.SelectCommand.Parameters.AddWithValue("@EndDate", ed);
            if (year != "")
            {
                da.SelectCommand.Parameters.AddWithValue("@Year", year);
            }
            if (ClassId != "")
            {
                da.SelectCommand.Parameters.AddWithValue("@class", ClassId);
            }
            if (Sec != "")
            {
                da.SelectCommand.Parameters.AddWithValue("@section", Sec);
            }
            //if (Shift != "")
            //{
            //    da.SelectCommand.Parameters.AddWithValue("@Shift", Shift);
            //}
            //if (Version != "")
            //{
            //    da.SelectCommand.Parameters.AddWithValue("@verson", Version);
            //}

            DataSet ds = new DataSet();
            da.Fill(ds, "SP_AccStudentTransactionDateWise");
            ds.Tables[0].TableName = "SP_AccStudentTransactionDateWise";
            return ds.Tables[0];
        }
        //********************* Change Query**************************//
        public static DataTable getDailyByDateCollection(string sd, string ed)
        {
            String connectionString = DataManager.OraConnString();
            string query = @"SELECT a.payment_id ,convert (nvarchar,b.PAY_DATE ,103)PAY_DATE,b.student_id,si.f_name as std_name,ci.class_name as std_class,pf.pay_head_id, se.sec_name,  
    round(pf.pay_amt,2) pay_amt ,a.Discount_AMT ,(pf.pay_amt-a.Discount_AMT)Total_Pay_amt from PAYMENT_DTL a    
    inner join PAYMENT_MST b on b.PAYMENT_ID=a.PAYMENT_ID
    inner join class_info ci on ci.class_id =b.class_id
    inner join student_info si on si.student_id=b.STUDENT_ID  
    inner join std_current_status st on st.student_id=si.student_id
    inner join payment_info pf on pf.pay_id=a.PAY_ID 
inner join section_info se on se.sec_id=b.sect    
    where b.PAY_DATE   between convert(datetime,'" + sd + "',103) and convert(datetime,'" + ed + "',103)  order by (select b.PAY_DATE from  PAYMENT_MST b where b.PAYMENT_ID=a.PAYMENT_ID) desc";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PaymentDtls");
            return dt;
        }
        //******************** Change By Shakhawat *******************//

        public static DataTable getDailyByDateCollectionSummery(string sd, string ed, string year)
        {
//            String connectionString = DataManager.OraConnString();
//            string query = @"with tt as (
// SELECT  st.std_roll, b.student_id, ci.class_id,si.f_name as std_name,ci.class_name as std_class,se.sec_name,sh.shift_name,vi.version_name,pay.paid_amt as[Prv_Amt],
//(select isnull(SUM(pay_amt),0) from payment_info where (pay_class is null OR pay_class=b.class_id) and (pay_type='Y' or (for_all_std='Y' and pay_type = case when st.previous_last_class='' OR st.previous_last_class IS NULL  then 'N' else 'O'end  )))+((select isnull(SUM(pay_amt),0) from payment_info where(pay_class is null OR pay_class=b.class_id) and pay_type='M') *  case when (DATEDIFF(MONTH,CONVERT(date,st.class_start,103),CONVERT(date,GETDATE(),103))+1)<0 then 0 else  (DATEDIFF(MONTH,CONVERT(date,st.class_start,103),CONVERT(date,GETDATE(),103))+1) end ) pay_amt,
//sum(isnull(a.Discount_AMT,0))+isnull(pay.Discount_AMT,0) Discount_AMT,sum(round(a.pay_amt,2)) Paid  ,
//
//Isnull(((select isnull( ss.WAIVE_PCT,0) * case when  ( DATEDIFF(MONTH, case when CONVERT(date,ss.EXC_FROM,103) > CONVERT(date,st.class_start,103) then CONVERT(date,ss.EXC_FROM,103) else  CONVERT(date,st.class_start,103) end, case when CONVERT(date,ss.EXC_TO,103) < CONVERT(date,GETDATE(),103) then CONVERT(date,ss.EXC_TO,103) else CONVERT(date,GETDATE(),103) end )+1) <0 then 0 else  ( DATEDIFF(MONTH, case when CONVERT(date,ss.EXC_FROM,103) > CONVERT(date,st.class_start,103) then CONVERT(date,ss.EXC_FROM,103) else  CONVERT(date,st.class_start,103) end, case when CONVERT(date,ss.EXC_TO,103) < CONVERT(date,GETDATE(),103) then CONVERT(date,ss.EXC_TO,103) else CONVERT(date,GETDATE(),103) end )+1) end 
//from STD_WAIVER ss where( ss.STUDENT_ID=st.student_id) and ss.WAIVE_YEAR=st.class_year) ),0) WV_Amt
//     from PAYMENT_DTL a           
//    inner join PAYMENT_MST b on b.PAYMENT_ID=a.PAYMENT_ID and b.PAY_DATE   between convert(datetime,'" + sd + "',103) and     convert(datetime,'" + ed + "',103) inner join class_info ci on ci.class_id =b.class_id  inner join student_info si on si.student_id=b.STUDENT_ID inner join std_current_status st on st.student_id=si.student_id   inner join payment_info pf on pf.pay_id=a.PAY_ID  inner join section_info se on se.sec_id=b.sect  inner join shift_info sh on sh.shift_id=b.shift  inner join version_info vi on vi.version_id=b.[version]  left join (select student_id,sum(pay_amt) paid_amt,sum(pd.Discount_AMT) Discount_AMT,pm.PAY_YEAR from payment_mst pm inner join payment_dtl pd  on pm.PAYMENT_ID=pd.PAYMENT_ID  and pm.PAY_DATE<convert(datetime,'" + sd + "',103) group by student_id,pm.PAY_YEAR) pay on pay.STUDENT_ID=b.STUDENT_ID and b.PAY_YEAR=pay.PAY_YEAR        group by st.std_roll,st.class_year, b.student_id,si.f_name,ci.class_name, b.class_id,st.previous_last_class,se.sec_name,st.class_start,sh.shift_name,st.student_id,ci.class_id, vi.version_name,pay.paid_amt,pay.Discount_AMT) select tt.std_roll, tt.STUDENT_ID as[student_id],tt.std_name as [std_name], tt.class_id,tt.std_class as std_class,tt.WV_Amt, tt.sec_name as sec_name,tt.shift_name as shift_name,tt.version_name as version_name,tt.pay_amt as pay_amt,isnull(tt.Discount_AMT,0) as Discount_AMT,isnull(tt.Prv_Amt,0) Prv_Amt,isnull(tt.pay_amt,0)-isnull(tt.Discount_AMT,0)-isnull(tt.Prv_Amt,0) as[GrandTotal],tt.Paid,(isnull(tt.pay_amt,0)-isnull(tt.Discount_AMT,0))-isnull(tt.Paid,0)-isnull(tt.Prv_Amt,0)-ISNULL(tt.WV_Amt,0) as[DueAmt] from tt order by  Convert(int,tt.class_id),Convert(int,tt.std_roll)";
//            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PaymentDtls");
//            return dt;

            SqlConnection conn = new SqlConnection(DataManager.OraConnString());
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = new SqlCommand("SP_AccStudentTransactionDateWise", conn);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;



            //if (Sec != "")
            //{
            //    da.SelectCommand.Parameters.AddWithValue("@section", Sec);
            //}
            //if (Shift != "")
            //{
            //    da.SelectCommand.Parameters.AddWithValue("@Shift", Shift);
            //}
            //if (Version != "")
            //{
            //    da.SelectCommand.Parameters.AddWithValue("@verson", Version);
            //}
            da.SelectCommand.Parameters.AddWithValue("@StartDate", sd);
            da.SelectCommand.Parameters.AddWithValue("@EndDate", ed);
            da.SelectCommand.Parameters.AddWithValue("@Year", year);
            DataSet ds = new DataSet();
            da.Fill(ds, "SP_AccStudentTransactionDateWise");
            ds.Tables[0].TableName = "SP_AccStudentTransactionDateWise";
            return ds.Tables[0];
        }

        public static DataTable getDailyByDateAndClassCollectionSummery(string sd, string ed,string year,string Class)
        {
//            String connectionString = DataManager.OraConnString();
//            string query = @" with tt as (
// SELECT  st.std_roll, b.student_id, ci.class_id,si.f_name as std_name,ci.class_name as std_class,se.sec_name,sh.shift_name,vi.version_name,pay.paid_amt as[Prv_Amt],
//(select isnull(SUM(pay_amt),0) from payment_info where (pay_class is null OR pay_class=b.class_id) and (pay_type='Y' or (for_all_std='Y' and pay_type = case when st.previous_last_class='' OR st.previous_last_class IS NULL  then 'N' else 'O'end  )))+((select isnull(SUM(pay_amt),0) from payment_info where(pay_class is null OR pay_class=b.class_id) and pay_type='M') *  case when (DATEDIFF(MONTH,CONVERT(date,st.class_start,103),CONVERT(date,GETDATE(),103))+1)<0 then 0 else  (DATEDIFF(MONTH,CONVERT(date,st.class_start,103),CONVERT(date,GETDATE(),103))+1) end ) pay_amt,
//sum(isnull(a.Discount_AMT,0))+isnull(pay.Discount_AMT,0) Discount_AMT,sum(round(a.pay_amt,2)) Paid  ,
//
//Isnull(((select isnull( ss.WAIVE_PCT,0) * case when  ( DATEDIFF(MONTH, case when CONVERT(date,ss.EXC_FROM,103) > CONVERT(date,st.class_start,103) then CONVERT(date,ss.EXC_FROM,103) else  CONVERT(date,st.class_start,103) end, case when CONVERT(date,ss.EXC_TO,103) < CONVERT(date,GETDATE(),103) then CONVERT(date,ss.EXC_TO,103) else CONVERT(date,GETDATE(),103) end )+1) <0 then 0 else  ( DATEDIFF(MONTH, case when CONVERT(date,ss.EXC_FROM,103) > CONVERT(date,st.class_start,103) then CONVERT(date,ss.EXC_FROM,103) else  CONVERT(date,st.class_start,103) end, case when CONVERT(date,ss.EXC_TO,103) < CONVERT(date,GETDATE(),103) then CONVERT(date,ss.EXC_TO,103) else CONVERT(date,GETDATE(),103) end )+1) end 
//from STD_WAIVER ss where( ss.STUDENT_ID=st.student_id) and ss.WAIVE_YEAR=st.class_year) ),0) WV_Amt
//     from PAYMENT_DTL a           
//    inner join PAYMENT_MST b on b.PAYMENT_ID=a.PAYMENT_ID and b.PAY_DATE   between convert(datetime,'" + sd + "',103) and     convert(datetime,'" + ed + "',103) inner join class_info ci on ci.class_id =b.class_id and b.class_id='" + Class + "'  inner join student_info si on si.student_id=b.STUDENT_ID inner join std_current_status st on st.student_id=si.student_id   inner join payment_info pf on pf.pay_id=a.PAY_ID  inner join section_info se on se.sec_id=b.sect  inner join shift_info sh on sh.shift_id=b.shift  inner join version_info vi on vi.version_id=b.[version]  left join (select student_id,sum(pay_amt) paid_amt,sum(pd.Discount_AMT) Discount_AMT,pm.PAY_YEAR from payment_mst pm inner join payment_dtl pd  on pm.PAYMENT_ID=pd.PAYMENT_ID  and pm.PAY_DATE<convert(datetime,'" + sd + "',103) group by student_id,pm.PAY_YEAR) pay on pay.STUDENT_ID=b.STUDENT_ID and b.PAY_YEAR=pay.PAY_YEAR        group by st.std_roll,st.class_year, b.student_id,si.f_name,ci.class_name, b.class_id,st.previous_last_class,se.sec_name,st.class_start,sh.shift_name, ci.class_id,st.student_id, vi.version_name,pay.paid_amt,pay.Discount_AMT) select tt.std_roll, tt.STUDENT_ID as[student_id],tt.std_name as [std_name],tt.class_id, tt.std_class as std_class,tt.WV_Amt, tt.sec_name as sec_name,tt.shift_name as shift_name,tt.version_name as version_name,tt.pay_amt as pay_amt,isnull(tt.Discount_AMT,0) as Discount_AMT,isnull(tt.Prv_Amt,0) Prv_Amt,isnull(tt.pay_amt,0)-isnull(tt.Discount_AMT,0)-isnull(tt.Prv_Amt,0) as[GrandTotal],tt.Paid,(isnull(tt.pay_amt,0)-isnull(tt.Discount_AMT,0))-isnull(tt.Paid,0)-isnull(tt.Prv_Amt,0)-ISNULL(tt.WV_Amt,0) as[DueAmt] from tt order by  Convert(int,tt.class_id),Convert(int,tt.std_roll)";
//            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PaymentDtls");
//            return dt;

            SqlConnection conn = new SqlConnection(DataManager.OraConnString());
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = new SqlCommand("SP_AccStudentTransactionDateWise", conn);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;

            da.SelectCommand.Parameters.AddWithValue("@StartDate", sd);
            da.SelectCommand.Parameters.AddWithValue("@EndDate", ed);
            if (year != "")
            {
                da.SelectCommand.Parameters.AddWithValue("@Year", year);
            }
            if (Class != "")
            {
                da.SelectCommand.Parameters.AddWithValue("@class", Class);
            }
            //if (Shift != "")
            //{
            //    da.SelectCommand.Parameters.AddWithValue("@Shift", Shift);
            //}
            //if (Version != "")
            //{
            //    da.SelectCommand.Parameters.AddWithValue("@verson", Version);
            //}

            DataSet ds = new DataSet();
            da.Fill(ds, "SP_AccStudentTransactionDateWise");
            ds.Tables[0].TableName = "SP_AccStudentTransactionDateWise";
            return ds.Tables[0];
        }


        public static DataTable getDailyByDateAndClassAndSecCollectionSummery(string sd, string ed, string year, string Class, string Shif, string Sec)
        {
//            String connectionString = DataManager.OraConnString();
//            string query = @"  with tt as (
// SELECT  st.std_roll, b.student_id,si.f_name as std_name,ci.class_id,ci.class_name as std_class,se.sec_name,sh.shift_name,vi.version_name,pay.paid_amt as[Prv_Amt],
//(select isnull(SUM(pay_amt),0) from payment_info where (pay_class is null OR pay_class=b.class_id) and (pay_type='Y' or (for_all_std='Y' and pay_type = case when st.previous_last_class='' OR st.previous_last_class IS NULL  then 'N' else 'O'end  )))+((select isnull(SUM(pay_amt),0) from payment_info where(pay_class is null OR pay_class=b.class_id) and pay_type='M') *  case when (DATEDIFF(MONTH,CONVERT(date,st.class_start,103),CONVERT(date,GETDATE(),103))+1)<0 then 0 else  (DATEDIFF(MONTH,CONVERT(date,st.class_start,103),CONVERT(date,GETDATE(),103))+1) end ) pay_amt,
//sum(isnull(a.Discount_AMT,0))+isnull(pay.Discount_AMT,0) Discount_AMT,sum(round(a.pay_amt,2)) Paid  ,
//
//Isnull(((select isnull( ss.WAIVE_PCT,0) * case when  ( DATEDIFF(MONTH, case when CONVERT(date,ss.EXC_FROM,103) > CONVERT(date,st.class_start,103) then CONVERT(date,ss.EXC_FROM,103) else  CONVERT(date,st.class_start,103) end, case when CONVERT(date,ss.EXC_TO,103) < CONVERT(date,GETDATE(),103) then CONVERT(date,ss.EXC_TO,103) else CONVERT(date,GETDATE(),103) end )+1) <0 then 0 else  ( DATEDIFF(MONTH, case when CONVERT(date,ss.EXC_FROM,103) > CONVERT(date,st.class_start,103) then CONVERT(date,ss.EXC_FROM,103) else  CONVERT(date,st.class_start,103) end, case when CONVERT(date,ss.EXC_TO,103) < CONVERT(date,GETDATE(),103) then CONVERT(date,ss.EXC_TO,103) else CONVERT(date,GETDATE(),103) end )+1) end 
//from STD_WAIVER ss where( ss.STUDENT_ID=st.student_id) and ss.WAIVE_YEAR=st.class_year) ),0) WV_Amt
//     from PAYMENT_DTL a           
//    inner join PAYMENT_MST b on b.PAYMENT_ID=a.PAYMENT_ID and b.PAY_DATE   between convert(datetime,'" + sd + "',103) and     convert(datetime,'" + ed + "',103) inner join class_info ci on ci.class_id =b.class_id and b.class_id='" + Class + "'  inner join student_info si on si.student_id=b.STUDENT_ID inner join std_current_status st on st.student_id=si.student_id   inner join payment_info pf on pf.pay_id=a.PAY_ID  inner join section_info se on se.sec_id=b.sect and b.sect='" + Sec + "'  inner join shift_info sh on sh.shift_id=b.shift  inner join version_info vi on vi.version_id=b.[version]  left join (select student_id,sum(pay_amt) paid_amt,sum(pd.Discount_AMT) Discount_AMT,pm.PAY_YEAR from payment_mst pm inner join payment_dtl pd  on pm.PAYMENT_ID=pd.PAYMENT_ID  and pm.PAY_DATE<convert(datetime,'" + sd + "',103) group by student_id,pm.PAY_YEAR) pay on pay.STUDENT_ID=b.STUDENT_ID and b.PAY_YEAR=pay.PAY_YEAR        group by st.std_roll,st.class_year, b.student_id,si.f_name,ci.class_name, b.class_id,st.previous_last_class,se.sec_name,st.class_start,sh.shift_name,ci.class_id,st.student_id, vi.version_name,pay.paid_amt,pay.Discount_AMT) select tt.std_roll, tt.STUDENT_ID as[student_id],tt.std_name as [std_name], tt.class_id,tt.std_class as std_class,tt.WV_Amt, tt.sec_name as sec_name,tt.shift_name as shift_name,tt.version_name as version_name,tt.pay_amt as pay_amt,isnull(tt.Discount_AMT,0) as Discount_AMT,isnull(tt.Prv_Amt,0) Prv_Amt,isnull(tt.pay_amt,0)-isnull(tt.Discount_AMT,0)-isnull(tt.Prv_Amt,0) as[GrandTotal],tt.Paid,(isnull(tt.pay_amt,0)-isnull(tt.Discount_AMT,0))-isnull(tt.Paid,0)-isnull(tt.Prv_Amt,0)-ISNULL(tt.WV_Amt,0) as[DueAmt] from tt order by  Convert(int,tt.class_id),Convert(int,tt.std_roll)";
//            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PaymentDtls");
//            return dt;

            SqlConnection conn = new SqlConnection(DataManager.OraConnString());
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = new SqlCommand("SP_AccStudentTransactionDateWise", conn);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;


            da.SelectCommand.Parameters.AddWithValue("@StartDate", sd);
            da.SelectCommand.Parameters.AddWithValue("@EndDate", ed);
            if (year != "")
            {
                da.SelectCommand.Parameters.AddWithValue("@Year", year);
            }

            if (Class != "")
            {
                da.SelectCommand.Parameters.AddWithValue("@class", Class);
            }
            if (Shif != "")
            {
                da.SelectCommand.Parameters.AddWithValue("@Shift", Shif);
            }
            if (Sec != "")
            {
                da.SelectCommand.Parameters.AddWithValue("@section", Sec);
            }
            //if (Shift != "")
            //{
            //    da.SelectCommand.Parameters.AddWithValue("@Shift", Shift);
            //}
            //if (Version != "")
            //{
            //    da.SelectCommand.Parameters.AddWithValue("@verson", Version);
            //}

            DataSet ds = new DataSet();
            da.Fill(ds, "SP_AccStudentTransactionDateWise");
            ds.Tables[0].TableName = "SP_AccStudentTransactionDateWise";
            return ds.Tables[0];
        }

//        public static DataTable getDailyByDateandClassCollectionSummery(string sd, string ed , string classId)
//        {
//            String connectionString = DataManager.OraConnString();
//            string query = @"select  a.payment_id,b.student_id,ci.class_name as std_class,si.f_name as std_name,Convert(varchar,b.PAY_DATE,103) PAY_DATE, sum(a.PAY_AMT) PAY_AMT  from PAYMENT_DTL a
//  inner join PAYMENT_MST b on b.PAYMENT_ID=a.PAYMENT_ID and b.PAY_DATE  between convert(date,'" +sd+"',103) and convert(date,'"+ed+"',103)  inner join student_info si on b.STUDENT_ID=si.student_id  inner join class_info ci on b.class_id=ci.class_id and b.class_id='"+classId+"'  group by ci.class_name ,si.f_name,b.PAY_DATE,a.payment_id,b.student_id order by a.PAYMENT_ID ";
//            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PaymentDtls");
//            return dt;
//        }



         
        //************************Change Query*****************//
        public static DataTable getDailyByDateAndStIdCollection(string sd, string ed, string id)
        {
            String connectionString = DataManager.OraConnString();
            string query = "SELECT a.payment_id ,convert(nvarchar,(select b.PAY_DATE from  PAYMENT_MST b where b.PAYMENT_ID=a.PAYMENT_ID),103)pay_date," +
            " (select b.STUDENT_ID from  PAYMENT_MST b where b.PAYMENT_ID=a.PAYMENT_ID) student_id,(select dbo.initcap(rtrim(ltrim(f_name+' '+m_name+' '+l_name))) from student_info where student_id=(select b.STUDENT_ID from  PAYMENT_MST b where b.PAYMENT_ID=a.PAYMENT_ID)) std_name,(select class_name from class_info where class_id=(select b.class_id from  PAYMENT_MST b where b.PAYMENT_ID=a.PAYMENT_ID)) std_class, " +
            " (select pay_head_id from payment_info where pay_id=a.PAY_ID) pay_head_id," +
            " round(a.pay_amt,2) pay_amt from PAYMENT_DTL a  where" +
            " (select b.STUDENT_ID from  PAYMENT_MST b where b.PAYMENT_ID=a.PAYMENT_ID) like upper('%" + id + "%') and (select b.PAY_DATE from  PAYMENT_MST b where b.PAYMENT_ID=a.PAYMENT_ID) between convert(datetime,'" + sd + "',103) and convert(datetime,'" + ed + "',103) " +
            " order by (select b.PAY_DATE from  PAYMENT_MST b where b.PAYMENT_ID=a.PAYMENT_ID) desc, payment_id desc";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PaymentDtls");
            return dt;
        }


        public static DataTable getDailyByDateAndClassCollection(string sd, string ed, string id)
        {
            String connectionString = DataManager.OraConnString();
            string query = @"SELECT a.payment_id ,convert (nvarchar,b.PAY_DATE ,103)PAY_DATE,b.student_id,si.f_name as std_name,ci.class_name as std_class,pf.pay_head_id,se.sec_name,   
    round(a.pay_amt,2) pay_amt ,a.Discount_AMT ,(a.pay_amt-a.Discount_AMT)Total_Pay_amt from PAYMENT_DTL a    
    inner join PAYMENT_MST b on b.PAYMENT_ID=a.PAYMENT_ID
    inner join class_info ci on ci.class_id =b.class_id and b.class_id='" + id + "'  inner join student_info si on si.student_id=b.STUDENT_ID    inner join std_current_status st on st.student_id=si.student_id   inner join payment_info pf on pf.pay_id=a.PAY_ID inner join section_info se on se.sec_id=b.sect      where b.PAY_DATE   between convert(datetime,'" + sd + "',103) and convert(datetime,'" + ed + "',103)  order by (select b.PAY_DATE from  PAYMENT_MST b where b.PAYMENT_ID=a.PAYMENT_ID) desc";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PaymentDtls");
            return dt;
        }

        public static DataTable getDailyByDateAndClassSectionCollection(string sd, string ed, string id,string sec)
        {
            String connectionString = DataManager.OraConnString();
            string query = @"SELECT a.payment_id ,convert (nvarchar,b.PAY_DATE ,103)PAY_DATE,b.student_id,si.f_name as std_name,ci.class_name as std_class,pf.pay_head_id,se.sec_name,     
    round(a.pay_amt,2) pay_amt ,a.Discount_AMT ,(a.pay_amt-a.Discount_AMT)Total_Pay_amt from PAYMENT_DTL a    
    inner join PAYMENT_MST b on b.PAYMENT_ID=a.PAYMENT_ID
    inner join class_info ci on ci.class_id =b.class_id and b.class_id='" + id + "'  inner join student_info si on si.student_id=b.STUDENT_ID    inner join std_current_status st on st.student_id=si.student_id  inner join payment_info pf on pf.pay_id=a.PAY_ID      inner join section_info se on se.sec_id=b.sect and b.sect='" + sec + "'    where b.PAY_DATE   between convert(datetime,'" + sd + "',103) and convert(datetime,'" + ed + "',103)  order by (select b.PAY_DATE from  PAYMENT_MST b where b.PAYMENT_ID=a.PAYMENT_ID) desc";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PaymentDtls");
            return dt;
        }



        //public static DataTable getDailyByDateAndStIdCollection(string sd, string ed, string id)
        //{
        //    String connectionString = DataManager.OraConnString();
        //    string query = "select a.payment_id,convert(varchar,pay_date,103) pay_date,student_id, " +
        //    " (select dbo.initcap(rtrim(ltrim(f_name+' '+m_name+' '+l_name))) from student_info where student_id=a.student_id) std_name,(select c.class_name  from class_info c  where c.class_id=a.class_id) std_class, " +
        //    " (select pay_head_id from payment_info where pay_id=b.pay_id) pay_head_id, " +
        //    " round(b.pay_amt,2) pay_amt from payment_mst a, payment_dtl b where " +
        //    " student_id like upper('%" + id + "%') and pay_date between convert(datetime,'" + sd + "',103) and convert(datetime,'" + ed + "',103) " +
        //    " order by pay_date desc, payment_id desc";
        //    DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PaymentDtls");
        //    return dt;
        //}


        public static DataTable getDailyCollectionUsr(string sd, string ed)
        {
            String connectionString = DataManager.OraConnString();
            string query = "select upper(c.description) user_name,sum(b.pay_amt) pay_amt from utl_userinfo c,payment_mst a, payment_dtl b where a.payment_id=b.payment_id "+
            " and upper(a.entry_user)=upper(c.user_name) and a.pay_date between convert('" + sd + "',103) and convert('" + ed + "',103) " +
            " group by upper(c.description) order by 1";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PaymentDtls");
            return dt;
        }
        public static DataTable getDueList(string std ,string yr)
        {


            //String connectionString = DataManager.OraConnString();
            //string query = " select tot.*,paid_amt-pay_amt bal from ( "+
            //" select x.student_id,dbo.initcap(rtrim(ltrim(f_name+' '+m_name+' '+l_name))) name,x.class_id,x.class_year, "+
            //" (select isnull(SUM(pay_amt),0) from payment_info where coalesce(pay_class,x.class_id)=x.class_id )+ " +
            //" ((select isnull(SUM(pay_amt),0) from payment_info where coalesce(pay_class,x.class_id)=x.class_id and pay_type='M')* " +
            //" case when ((MONTH(convert(datetime,nullif('" + yr + "',''),103))-1)-(MONTH(convert(datetime,nullif(x.class_start,''),103))))<0 then 0 else "+
            //" ((MONTH(convert(datetime,nullif(Getdate(),''),103))-1)-(MONTH(convert(datetime,nullif(x.class_start,''),103)))) end ) pay_amt, " +
            //" (select isnull(SUM(pay_amt),0) from PAYMENT_DTL a,PAYMENT_MST b where a.PAYMENT_ID=b.PAYMENT_ID and b.STUDENT_ID=x.student_id "+
            //" and b.class_id=x.class_id and b.PAY_YEAR=x.class_year) paid_amt from "+
            //" (select student_id,max(class_start) class_start,MAX(class_id) class_id,MAX(class_year) class_year from std_current_status group by student_id) x, " +
            //" student_info y where x.student_id=y.student_id) tot where paid_amt-pay_amt<-100 order by paid_amt-pay_amt desc";
            //DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PaymentDtls");
            //return dt;

            //****************************** Change This Due List All Student ****************************//




            String connectionString = DataManager.OraConnString();
            string query = @"select tot.*,pay_amt-WV_Amt-paid_amt-Discount_AMT bal from (select x.student_id,dbo.initcap(rtrim(ltrim(f_name+' '+m_name+' '+l_name))) name,(select class_name from class_info z where z.class_id= x.class_id)class_id,x.class_year, (select isnull(SUM(pay_amt),0) from payment_info where (pay_class is null OR pay_class=x.class_id) and (pay_type='Y' or (for_all_std='Y' and pay_type = case when x.previous_last_class='' OR x.previous_last_class IS NULL  then 'N' else 'O'end  )))+((select isnull(SUM(pay_amt),0) from payment_info where(pay_class is null OR pay_class=x.class_id) and pay_type='M') * 
case when (DATEDIFF(MONTH,CONVERT(date,x.class_start,103),CONVERT(date,GETDATE(),103))+1)<0 then 0 else  (DATEDIFF(MONTH,CONVERT(date,x.class_start,103),CONVERT(date,GETDATE(),103))+1) end ) pay_amt,(select isnull(SUM(Discount_AMT),0)from PAYMENT_DTL a,PAYMENT_MST b where a.PAYMENT_ID=b.PAYMENT_ID and b.STUDENT_ID=x.student_id  and b.class_id=x.class_id and b.PAY_YEAR=x.class_year) Discount_AMT,isnull(((select isnull( ss.WAIVE_PCT,0) * case when  ( DATEDIFF(MONTH, case when CONVERT(date,ss.EXC_FROM,103) > CONVERT(date,x.class_start,103) then CONVERT(date,ss.EXC_FROM,103) else  CONVERT(date,x.class_start,103) end, case when CONVERT(date,ss.EXC_TO,103) < CONVERT(date,GETDATE(),103) then CONVERT(date,ss.EXC_TO,103) else CONVERT(date,GETDATE(),103) end )+1) <0 then 0 else  ( DATEDIFF(MONTH, case when CONVERT(date,ss.EXC_FROM,103) > CONVERT(date,x.class_start,103) then CONVERT(date,ss.EXC_FROM,103) else  CONVERT(date,x.class_start,103) end, case when CONVERT(date,ss.EXC_TO,103) < CONVERT(date,GETDATE(),103) then CONVERT(date,ss.EXC_TO,103) else CONVERT(date,GETDATE(),103) end )+1) end
from STD_WAIVER ss where( ss.STUDENT_ID=x.student_id) and ss.WAIVE_YEAR=x.class_year) ),0) WV_Amt,  (select isnull(SUM(pay_amt),0)from PAYMENT_DTL a,PAYMENT_MST b where a.PAYMENT_ID=b.PAYMENT_ID and b.STUDENT_ID=x.student_id  and b.class_id=x.class_id and b.PAY_YEAR=x.class_year)paid_amt,(select std_roll from std_current_status k where x.student_id=k.student_id)Roll,(select sec_name from section_info where CONVERT(int,sec_id)=(select sect from std_current_status k where k.student_id=x.student_id))section from  (select student_id, class_start, CONVERT(int,class_id)  class_id, class_year ,previous_last_class  from std_current_status group by student_id, class_start,class_id,class_year,previous_last_class )x,  student_info y where x.student_id=y.student_id)tot where student_id='" + std + "' and paid_amt-pay_amt<-100 order by tot.class_id,tot.section, CONVERT(int, Roll) asc";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PaymentDtls");
            return dt;
            
            

        }
        //********************************** Due List *******************************************************//
        public static DataTable getDueListAllStudent(int Year)
        {
            String connectionString = DataManager.OraConnString();
//            string query = @"select tot.*,pay_amt-WV_Amt-paid_amt-Discount_AMT bal from (select x.student_id,dbo.initcap(rtrim(ltrim(f_name+' '+m_name+' '+l_name))) name,(select class_name from class_info z where z.class_id= x.class_id)class_id,x.class_year, (select isnull(SUM(pay_amt),0) from payment_info where (pay_class is null OR pay_class=x.class_id) and (pay_type='Y' or (for_all_std='Y' and pay_type = case when x.previous_last_class='' OR x.previous_last_class IS NULL  then 'N' else 'O'end  )))+((select isnull(SUM(pay_amt),0) from payment_info where(pay_class is null OR pay_class=x.class_id) and pay_type='M') * 
//case when (DATEDIFF(MONTH,CONVERT(date,x.class_start,103),CONVERT(date,GETDATE(),103))+1)<0 then 0 else  (DATEDIFF(MONTH,CONVERT(date,x.class_start,103),CONVERT(date,GETDATE(),103))+1) end ) pay_amt,(select isnull(SUM(Discount_AMT),0)from PAYMENT_DTL a,PAYMENT_MST b where a.PAYMENT_ID=b.PAYMENT_ID and b.STUDENT_ID=x.student_id  and b.class_id=x.class_id and b.PAY_YEAR=x.class_year) Discount_AMT,isnull(((select isnull( ss.WAIVE_PCT,0) * case when  ( DATEDIFF(MONTH, case when CONVERT(date,ss.EXC_FROM,103) > CONVERT(date,x.class_start,103) then CONVERT(date,ss.EXC_FROM,103) else  CONVERT(date,x.class_start,103) end, case when CONVERT(date,ss.EXC_TO,103) < CONVERT(date,GETDATE(),103) then CONVERT(date,ss.EXC_TO,103) else CONVERT(date,GETDATE(),103) end )+1) <0 then 0 else  ( DATEDIFF(MONTH, case when CONVERT(date,ss.EXC_FROM,103) > CONVERT(date,x.class_start,103) then CONVERT(date,ss.EXC_FROM,103) else  CONVERT(date,x.class_start,103) end, case when CONVERT(date,ss.EXC_TO,103) < CONVERT(date,GETDATE(),103) then CONVERT(date,ss.EXC_TO,103) else CONVERT(date,GETDATE(),103) end )+1) end
//from STD_WAIVER ss where( ss.STUDENT_ID=x.student_id) and ss.WAIVE_YEAR=x.class_year) ),0) WV_Amt,  (select isnull(SUM(pay_amt),0)from PAYMENT_DTL a,PAYMENT_MST b where a.PAYMENT_ID=b.PAYMENT_ID and b.STUDENT_ID=x.student_id  and b.class_id=x.class_id and b.PAY_YEAR=x.class_year)paid_amt,(select std_roll from std_current_status k where x.student_id=k.student_id)Roll,(select sec_name from section_info where CONVERT(int,sec_id)=(select sect from std_current_status k where k.student_id=x.student_id))section from  (select student_id, class_start, CONVERT(int,class_id)  class_id, class_year ,previous_last_class  from std_current_status group by student_id, class_start,class_id,class_year,previous_last_class )x,  student_info y where x.student_id=y.student_id)tot where pay_amt-paid_amt<=0 and  paid_amt>0 order by tot.class_id,tot.section, CONVERT(int, Roll) asc";
            SqlConnection conn = new SqlConnection(DataManager.OraConnString());
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = new SqlCommand("SP_AccStudentTransaction", conn);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.Parameters.AddWithValue("@Year", Year);
            DataSet ds = new DataSet();
            da.Fill(ds, "SP_AccStudentTransaction");
            ds.Tables[0].TableName = "SP_AccStudentTransaction";
            return ds.Tables[0];
        }
        public static DataTable getDueListAllStudentClassWise(string Class)
        {
            String connectionString = DataManager.OraConnString();
            string query = @"select tot.*,pay_amt-WV_Amt-paid_amt-Discount_AMT bal from (select x.student_id,dbo.initcap(rtrim(ltrim(f_name+' '+m_name+' '+l_name))) name,(select class_name from class_info z where z.class_id= x.class_id)class_id,x.class_year, (select isnull(SUM(pay_amt),0) from payment_info where (pay_class is null OR pay_class=x.class_id) and (pay_type='Y' or (for_all_std='Y' and pay_type = case when x.previous_last_class='' OR x.previous_last_class IS NULL  then 'N' else 'O'end  )))+((select isnull(SUM(pay_amt),0) from payment_info where(pay_class is null OR pay_class=x.class_id) and pay_type='M') * case when (DATEDIFF(MONTH,CONVERT(date,x.class_start,103),CONVERT(date,GETDATE(),103))+1)<0 then 0 else  (DATEDIFF(MONTH,CONVERT(date,x.class_start,103),CONVERT(date,GETDATE(),103))+1) end ) pay_amt,isnull(((select isnull( ss.WAIVE_PCT,0) * case when  ( DATEDIFF(MONTH, case when CONVERT(date,ss.EXC_FROM,103) > CONVERT(date,x.class_start,103) then CONVERT(date,ss.EXC_FROM,103) else  CONVERT(date,x.class_start,103) end, case when CONVERT(date,ss.EXC_TO,103) < CONVERT(date,GETDATE(),103) then CONVERT(date,ss.EXC_TO,103) else CONVERT(date,GETDATE(),103) end )+1) <0 then 0 else  ( DATEDIFF(MONTH, case when CONVERT(date,ss.EXC_FROM,103) > CONVERT(date,x.class_start,103) then CONVERT(date,ss.EXC_FROM,103) else  CONVERT(date,x.class_start,103) end, case when CONVERT(date,ss.EXC_TO,103) < CONVERT(date,GETDATE(),103) then CONVERT(date,ss.EXC_TO,103) else CONVERT(date,GETDATE(),103) end )+1) end
from STD_WAIVER ss where( ss.STUDENT_ID=x.student_id) and ss.WAIVE_YEAR=x.class_year) ),0) WV_Amt, (select isnull(SUM(Discount_AMT),0)from PAYMENT_DTL a,PAYMENT_MST b where a.PAYMENT_ID=b.PAYMENT_ID and b.STUDENT_ID=x.student_id  and b.class_id=x.class_id and b.PAY_YEAR=x.class_year) Discount_AMT, (select isnull(SUM(pay_amt),0)from PAYMENT_DTL a,PAYMENT_MST b where a.PAYMENT_ID=b.PAYMENT_ID and b.STUDENT_ID=x.student_id  and b.class_id=x.class_id and b.PAY_YEAR=x.class_year)paid_amt,(select std_roll from std_current_status k where x.student_id=k.student_id)Roll,(select sec_name from section_info where CONVERT(int,sec_id)=(select sect from std_current_status k where k.student_id=x.student_id))section from  (select student_id, class_start, CONVERT(int,class_id)  class_id, class_year ,previous_last_class  from std_current_status group by student_id, class_start,class_id,class_year,previous_last_class  )x,  student_info y where x.student_id=y.student_id and x.class_id='" + Class + "')tot where paid_amt-pay_amt<-100 order by tot.class_id,tot.section, CONVERT(int, Roll) asc";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PaymentDtls");
            return dt;
        }

        public static DataTable AllStudentPaymentPaidDueListClassWise(string Class, int Year)
        {
            //String connectionString = DataManager.OraConnString();
//            string query = @" select tot.*,pay_amt-WV_Amt-paid_amt-Discount_AMT bal from (select x.student_id,dbo.initcap(rtrim(ltrim(f_name+' '+m_name+' '+l_name))) name,(select class_name from class_info z where z.class_id= x.class_id)class_id,x.class_year, (select isnull(SUM(pay_amt),0) from payment_info where (pay_class is null OR pay_class=x.class_id) and (pay_type='Y' or (for_all_std='Y' and pay_type = case when x.previous_last_class='' OR x.previous_last_class IS NULL  then 'N' else 'O'end  )))+((select isnull(SUM(pay_amt),0) from payment_info where(pay_class is null OR pay_class=x.class_id) and pay_type='M') * case when (DATEDIFF(MONTH,CONVERT(date,x.class_start,103),CONVERT(date,GETDATE(),103))+1)<0 then 0 else  (DATEDIFF(MONTH,CONVERT(date,x.class_start,103),CONVERT(date,GETDATE(),103))+1) end ) pay_amt,isnull(((select isnull( ss.WAIVE_PCT,0) * case when  ( DATEDIFF(MONTH, case when CONVERT(date,ss.EXC_FROM,103) > CONVERT(date,x.class_start,103) then CONVERT(date,ss.EXC_FROM,103) else  CONVERT(date,x.class_start,103) end, case when CONVERT(date,ss.EXC_TO,103) < CONVERT(date,GETDATE(),103) then CONVERT(date,ss.EXC_TO,103) else CONVERT(date,GETDATE(),103) end )+1) <0 then 0 else  ( DATEDIFF(MONTH, case when CONVERT(date,ss.EXC_FROM,103) > CONVERT(date,x.class_start,103) then CONVERT(date,ss.EXC_FROM,103) else  CONVERT(date,x.class_start,103) end, case when CONVERT(date,ss.EXC_TO,103) < CONVERT(date,GETDATE(),103) then CONVERT(date,ss.EXC_TO,103) else CONVERT(date,GETDATE(),103) end )+1) end
//from STD_WAIVER ss where( ss.STUDENT_ID=x.student_id) and ss.WAIVE_YEAR=x.class_year) ),0) WV_Amt,(select isnull(SUM(Discount_AMT),0)from PAYMENT_DTL a,PAYMENT_MST b where a.PAYMENT_ID=b.PAYMENT_ID and b.STUDENT_ID=x.student_id  and b.class_id=x.class_id and b.PAY_YEAR=x.class_year) Discount_AMT,  (select isnull(SUM(pay_amt),0)from PAYMENT_DTL a,PAYMENT_MST b where a.PAYMENT_ID=b.PAYMENT_ID and b.STUDENT_ID=x.student_id  and b.class_id=x.class_id and b.PAY_YEAR=x.class_year)paid_amt,(select std_roll from std_current_status k where x.student_id=k.student_id)Roll,(select sec_name from section_info where CONVERT(int,sec_id)=(select sect from std_current_status k where k.student_id=x.student_id))section from  (select student_id, class_start, CONVERT(int,class_id)  class_id, class_year ,previous_last_class  from std_current_status group by student_id, class_start,class_id,class_year,previous_last_class  )x,  student_info y where x.student_id=y.student_id and x.class_id='" + Class + "')tot where paid_amt-pay_amt<-100 order by tot.class_id,tot.section, CONVERT(int, Roll) asc";
//            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PaymentDtls");
//            return dt;
            SqlConnection conn = new SqlConnection(DataManager.OraConnString());
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = new SqlCommand("SP_AccStudentTransaction", conn);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            if (Class != "Select Class")
            {
                da.SelectCommand.Parameters.AddWithValue("@class", Class);
            }
            da.SelectCommand.Parameters.AddWithValue("@Year", Year);
            DataSet ds = new DataSet();
            da.Fill(ds, "SP_AccStudentTransaction");
            ds.Tables[0].TableName = "SP_AccStudentTransaction";
            return ds.Tables[0];
        }

        public static DataTable AllStudentPaymentPaidDueList(int Year)
        {
            //String connectionString = DataManager.OraConnString();
//            string query = @"select tot.*,pay_amt-WV_Amt-paid_amt-Discount_AMT bal from (select x.student_id,dbo.initcap(rtrim(ltrim(f_name+' '+m_name+' '+l_name))) name,(select class_name from class_info z where z.class_id= x.class_id)class_id,x.class_year, (select isnull(SUM(pay_amt),0) from payment_info where (pay_class is null OR pay_class=x.class_id) and (pay_type='Y' or (for_all_std='Y' and pay_type = case when x.previous_last_class='' OR x.previous_last_class IS NULL  then 'N' else 'O'end  )))+((select isnull(SUM(pay_amt),0) from payment_info where(pay_class is null OR pay_class=x.class_id) and pay_type='M') * case when (DATEDIFF(MONTH,CONVERT(date,x.class_start,103),CONVERT(date,GETDATE(),103))+1)<0 then 0 else  (DATEDIFF(MONTH,CONVERT(date,x.class_start,103),CONVERT(date,GETDATE(),103))+1) end ) pay_amt,isnull(((select isnull( ss.WAIVE_PCT,0) * case when  ( DATEDIFF(MONTH, case when CONVERT(date,ss.EXC_FROM,103) > CONVERT(date,x.class_start,103) then CONVERT(date,ss.EXC_FROM,103) else  CONVERT(date,x.class_start,103) end, case when CONVERT(date,ss.EXC_TO,103) < CONVERT(date,GETDATE(),103) then CONVERT(date,ss.EXC_TO,103) else CONVERT(date,GETDATE(),103) end )+1) <0 then 0 else  ( DATEDIFF(MONTH, case when CONVERT(date,ss.EXC_FROM,103) > CONVERT(date,x.class_start,103) then CONVERT(date,ss.EXC_FROM,103) else  CONVERT(date,x.class_start,103) end, case when CONVERT(date,ss.EXC_TO,103) < CONVERT(date,GETDATE(),103) then CONVERT(date,ss.EXC_TO,103) else CONVERT(date,GETDATE(),103) end )+1) end
//from STD_WAIVER ss where( ss.STUDENT_ID=x.student_id) and ss.WAIVE_YEAR=x.class_year) ),0) WV_Amt,(select isnull(SUM(Discount_AMT),0)from PAYMENT_DTL a,PAYMENT_MST b where a.PAYMENT_ID=b.PAYMENT_ID and b.STUDENT_ID=x.student_id  and b.class_id=x.class_id and b.PAY_YEAR=x.class_year) Discount_AMT,  (select isnull(SUM(pay_amt),0)from PAYMENT_DTL a,PAYMENT_MST b where a.PAYMENT_ID=b.PAYMENT_ID and b.STUDENT_ID=x.student_id  and b.class_id=x.class_id and b.PAY_YEAR=x.class_year)paid_amt,(select std_roll from std_current_status k where x.student_id=k.student_id)Roll,(select sec_name from section_info where CONVERT(int,sec_id)=(select sect from std_current_status k where k.student_id=x.student_id))section from  (select student_id, class_start, CONVERT(int,class_id)  class_id, class_year ,previous_last_class  from std_current_status group by student_id, class_start,class_id,class_year,previous_last_class  )x,  student_info y where x.student_id=y.student_id )tot  order by tot.class_id,tot.section, CONVERT(int, Roll) asc";
//            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PaymentDtls");
//            return dt;


            SqlConnection conn = new SqlConnection(DataManager.OraConnString());
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = new SqlCommand("SP_AccStudentTransaction", conn);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;

            da.SelectCommand.Parameters.AddWithValue("@Year", Year);
            DataSet ds = new DataSet();
            da.Fill(ds, "SP_AccStudentTransaction");
            ds.Tables[0].TableName = "SP_AccStudentTransaction";
            return ds.Tables[0];
        }

        public static DataTable ClassAndSectionPaymentPaidDueList(string ClassId, string Sec, int Year)
        {
//            String connectionString = DataManager.OraConnString();
//            string query = @" select tot.*,pay_amt-WV_Amt-paid_amt-Discount_AMT bal from (select x.student_id,dbo.initcap(rtrim(ltrim(f_name+' '+m_name+' '+l_name))) name,(select class_name from class_info z where z.class_id= x.class_id)class_id,x.class_year, (select isnull(SUM(pay_amt),0) from payment_info where (pay_class is null OR pay_class=x.class_id) and (pay_type='Y' or (for_all_std='Y' and pay_type = case when x.previous_last_class='' OR x.previous_last_class IS NULL  then 'N' else 'O'end  )))+((select isnull(SUM(pay_amt),0) from payment_info where(pay_class is null OR pay_class=x.class_id) and pay_type='M') * case when (DATEDIFF(MONTH,CONVERT(date,x.class_start,103),CONVERT(date,GETDATE(),103))+1)<0 then 0 else  (DATEDIFF(MONTH,CONVERT(date,x.class_start,103),CONVERT(date,GETDATE(),103))+1) end ) pay_amt,isnull(((select isnull( ss.WAIVE_PCT,0) * case when  ( DATEDIFF(MONTH, case when CONVERT(date,ss.EXC_FROM,103) > CONVERT(date,x.class_start,103) then CONVERT(date,ss.EXC_FROM,103) else  CONVERT(date,x.class_start,103) end, case when CONVERT(date,ss.EXC_TO,103) < CONVERT(date,GETDATE(),103) then CONVERT(date,ss.EXC_TO,103) else CONVERT(date,GETDATE(),103) end )+1) <0 then 0 else  ( DATEDIFF(MONTH, case when CONVERT(date,ss.EXC_FROM,103) > CONVERT(date,x.class_start,103) then CONVERT(date,ss.EXC_FROM,103) else  CONVERT(date,x.class_start,103) end, case when CONVERT(date,ss.EXC_TO,103) < CONVERT(date,GETDATE(),103) then CONVERT(date,ss.EXC_TO,103) else CONVERT(date,GETDATE(),103) end )+1) end
//from STD_WAIVER ss where( ss.STUDENT_ID=x.student_id) and ss.WAIVE_YEAR=x.class_year) ),0) WV_Amt, (select isnull(SUM(Discount_AMT),0)from PAYMENT_DTL a,PAYMENT_MST b where a.PAYMENT_ID=b.PAYMENT_ID and b.STUDENT_ID=x.student_id  and b.class_id=x.class_id and b.PAY_YEAR=x.class_year) Discount_AMT, (select isnull(SUM(pay_amt),0)from PAYMENT_DTL a,PAYMENT_MST b where a.PAYMENT_ID=b.PAYMENT_ID and b.STUDENT_ID=x.student_id  and b.class_id=x.class_id and b.PAY_YEAR=x.class_year)paid_amt,(select std_roll from std_current_status k where x.student_id=k.student_id)Roll,(select sec_name from section_info where CONVERT(int,sec_id)=(select sect from std_current_status k where k.student_id=x.student_id))section from  (select student_id, class_start, CONVERT(int,class_id)  class_id, class_year ,previous_last_class  from std_current_status group by student_id, class_start,class_id,class_year,previous_last_class  )x,  student_info y where x.student_id=y.student_id and x.class_id='" + ClassId + "' and x.sect='" + Sec + "')tot   order by tot.class_id,tot.section, CONVERT(int, Roll) asc ";
//            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PaymentDtls");
//            return dt;

            SqlConnection conn = new SqlConnection(DataManager.OraConnString());
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = new SqlCommand("SP_AccStudentTransaction", conn);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            if (ClassId != "Select Class")
            {
                da.SelectCommand.Parameters.AddWithValue("@class", ClassId);
            }
            if (Sec != "")
            {
                da.SelectCommand.Parameters.AddWithValue("@section", Sec);
            }
            //if (Shift != "")
            //{
            //    da.SelectCommand.Parameters.AddWithValue("@Shift", Shift);
            //}
            //if (Version != "")
            //{
            //    da.SelectCommand.Parameters.AddWithValue("@verson", Version);
            //}
            da.SelectCommand.Parameters.AddWithValue("@Year", Year);
            DataSet ds = new DataSet();
            da.Fill(ds, "SP_AccStudentTransaction");
            ds.Tables[0].TableName = "SP_AccStudentTransaction";
            return ds.Tables[0];
        }
        public static DataTable ClassWisePaymentPaidDueList(string ClassId,int Year)
       {
//            String connectionString = DataManager.OraConnString();
//            string query = @" select tot.*,pay_amt-WV_Amt-paid_amt-Discount_AMT bal from (select x.student_id,dbo.initcap(rtrim(ltrim(f_name+' '+m_name+' '+l_name))) name,(select class_name from class_info z where z.class_id= x.class_id)class_id,x.class_year, (select isnull(SUM(pay_amt),0) from payment_info where (pay_class is null OR pay_class=x.class_id) and (pay_type='Y' or (for_all_std='Y' and pay_type = case when x.previous_last_class='' OR x.previous_last_class IS NULL  then 'N' else 'O'end  )))+((select isnull(SUM(pay_amt),0) from payment_info where(pay_class is null OR pay_class=x.class_id) and pay_type='M') * case when (DATEDIFF(MONTH,CONVERT(date,x.class_start,103),CONVERT(date,GETDATE(),103))+1)<0 then 0 else  (DATEDIFF(MONTH,CONVERT(date,x.class_start,103),CONVERT(date,GETDATE(),103))+1) end ) pay_amt,isnull(((select isnull( ss.WAIVE_PCT,0) * case when  ( DATEDIFF(MONTH, case when CONVERT(date,ss.EXC_FROM,103) > CONVERT(date,x.class_start,103) then CONVERT(date,ss.EXC_FROM,103) else  CONVERT(date,x.class_start,103) end, case when CONVERT(date,ss.EXC_TO,103) < CONVERT(date,GETDATE(),103) then CONVERT(date,ss.EXC_TO,103) else CONVERT(date,GETDATE(),103) end )+1) <0 then 0 else  ( DATEDIFF(MONTH, case when CONVERT(date,ss.EXC_FROM,103) > CONVERT(date,x.class_start,103) then CONVERT(date,ss.EXC_FROM,103) else  CONVERT(date,x.class_start,103) end, case when CONVERT(date,ss.EXC_TO,103) < CONVERT(date,GETDATE(),103) then CONVERT(date,ss.EXC_TO,103) else CONVERT(date,GETDATE(),103) end )+1) end
//from STD_WAIVER ss where( ss.STUDENT_ID=x.student_id) and ss.WAIVE_YEAR=x.class_year) ),0) WV_Amt,(select isnull(SUM(Discount_AMT),0)from PAYMENT_DTL a,PAYMENT_MST b where a.PAYMENT_ID=b.PAYMENT_ID and b.STUDENT_ID=x.student_id  and b.class_id=x.class_id and b.PAY_YEAR=x.class_year) Discount_AMT,  (select isnull(SUM(pay_amt),0)from PAYMENT_DTL a,PAYMENT_MST b where a.PAYMENT_ID=b.PAYMENT_ID and b.STUDENT_ID=x.student_id  and b.class_id=x.class_id and b.PAY_YEAR=x.class_year)paid_amt,(select std_roll from std_current_status k where x.student_id=k.student_id)Roll,(select sec_name from section_info where CONVERT(int,sec_id)=(select sect from std_current_status k where k.student_id=x.student_id))section from  (select student_id, class_start, CONVERT(int,class_id)  class_id, class_year ,previous_last_class  from std_current_status group by student_id, class_start,class_id,class_year,previous_last_class  )x,  student_info y where x.student_id=y.student_id and x.class_id='" + ClassId + "')tot where paid_amt-pay_amt<-100 order by tot.class_id,tot.section, CONVERT(int, Roll) asc";
//            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PaymentDtls");
//            return dt;

            SqlConnection conn = new SqlConnection(DataManager.OraConnString());
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = new SqlCommand("SP_AccStudentTransaction", conn);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            if (ClassId != "Select Class")
            {
                da.SelectCommand.Parameters.AddWithValue("@class", ClassId);
            }

    da.SelectCommand.Parameters.AddWithValue("@Year", Year);
            DataSet ds = new DataSet();
            da.Fill(ds, "SP_AccStudentTransaction");
            ds.Tables[0].TableName = "SP_AccStudentTransaction";
            return ds.Tables[0];
        }

        public static DataTable AllStudentClassWiseAdmissionFeePaymentPaidDueList(string ClassId)
        {
            String connectionString = DataManager.OraConnString();
            string query = @" select tot.*,pay_amt-WV_Amt-paid_amt-Discount_AMT bal from (select x.student_id,dbo.initcap(rtrim(ltrim(f_name+' '+m_name+' '+l_name))) name,(select class_name from class_info z where z.class_id= x.class_id)class_id,x.class_year, (select isnull(SUM(pay_amt),0) from payment_info where (pay_class is null OR pay_class=x.class_id) and (pay_type='Y' or (for_all_std='Y' and pay_type = case when x.previous_last_class='' OR x.previous_last_class IS NULL  then 'N' else 'O'end  )))+((select isnull(SUM(pay_amt),0) from payment_info where(pay_class is null OR pay_class=x.class_id) and pay_type='M') * case when (DATEDIFF(MONTH,CONVERT(date,x.class_start,103),CONVERT(date,GETDATE(),103))+1)<0 then 0 else  (DATEDIFF(MONTH,CONVERT(date,x.class_start,103),CONVERT(date,GETDATE(),103))+1) end ) pay_amt,isnull(((select isnull( ss.WAIVE_PCT,0) * case when  ( DATEDIFF(MONTH, case when CONVERT(date,ss.EXC_FROM,103) > CONVERT(date,x.class_start,103) then CONVERT(date,ss.EXC_FROM,103) else  CONVERT(date,x.class_start,103) end, case when CONVERT(date,ss.EXC_TO,103) < CONVERT(date,GETDATE(),103) then CONVERT(date,ss.EXC_TO,103) else CONVERT(date,GETDATE(),103) end )+1) <0 then 0 else  ( DATEDIFF(MONTH, case when CONVERT(date,ss.EXC_FROM,103) > CONVERT(date,x.class_start,103) then CONVERT(date,ss.EXC_FROM,103) else  CONVERT(date,x.class_start,103) end, case when CONVERT(date,ss.EXC_TO,103) < CONVERT(date,GETDATE(),103) then CONVERT(date,ss.EXC_TO,103) else CONVERT(date,GETDATE(),103) end )+1) end
from STD_WAIVER ss where( ss.STUDENT_ID=x.student_id) and ss.WAIVE_YEAR=x.class_year) ),0) WV_Amt,(select isnull(SUM(Discount_AMT),0)from PAYMENT_DTL a,PAYMENT_MST b where a.PAYMENT_ID=b.PAYMENT_ID and b.STUDENT_ID=x.student_id  and b.class_id=x.class_id and b.PAY_YEAR=x.class_year) Discount_AMT,  (select isnull(SUM(pay_amt),0)from PAYMENT_DTL a,PAYMENT_MST b where a.PAYMENT_ID=b.PAYMENT_ID and b.STUDENT_ID=x.student_id  and b.class_id=x.class_id and b.PAY_YEAR=x.class_year)paid_amt,(select std_roll from std_current_status k where x.student_id=k.student_id)Roll,(select sec_name from section_info where CONVERT(int,sec_id)=(select sect from std_current_status k where k.student_id=x.student_id))section from  (select student_id, class_start, CONVERT(int,class_id)  class_id, class_year ,previous_last_class  from std_current_status group by student_id, class_start,class_id,class_year,previous_last_class  )x,  student_info y where x.student_id=y.student_id and x.class_id='" + ClassId + "')tot   order by tot.class_id,tot.section, CONVERT(int, Roll) asc";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PaymentDtls");
            return dt;
        }

        //--------------------------------------------------------*****************************************************

        public static DataTable ClassWiseAdmissionFeePaymentPaidDueList(string ClassId)
        {
            String connectionString = DataManager.OraConnString();
            string query = @"select tot.*,pay_amt-WV_Amt-paid_amt-Discount_AMT bal from (select x.student_id,dbo.initcap(rtrim(ltrim(f_name+' '+m_name+' '+l_name))) name,(select class_name from class_info z where  z.class_id=  x.class_id)class_id,x.class_year,

(select isnull(SUM(pay_amt),0) from payment_info where (pay_class is null OR pay_class=x.class_id) and (pay_type='Y' or (for_all_std='Y' and pay_type = case when x.previous_last_class='' OR x.previous_last_class IS NULL  then 'N' else 'O'end  ))) pay_amt,

 (select isnull(SUM(a.pay_amt),0)from PAYMENT_DTL a,PAYMENT_MST b,payment_info c where a.PAYMENT_ID=b.PAYMENT_ID and a.PAY_ID=c.pay_id and (pay_class is null OR pay_class=x.class_id) and (pay_type='Y' or (for_all_std='Y' and pay_type = case when x.previous_last_class='' OR x.previous_last_class IS NULL  then 'N' else 'O'end))   and b.STUDENT_ID=x.student_id  and  b.class_id= x.class_id and b.PAY_YEAR=x.class_year) paid_amt, (select isnull(SUM(Discount_AMT),0)from PAYMENT_DTL a,PAYMENT_MST b where a.PAYMENT_ID=b.PAYMENT_ID and b.STUDENT_ID=x.student_id  and b.class_id=x.class_id and b.PAY_YEAR=x.class_year) Discount_AMT,isnull(((select isnull( ss.WAIVE_PCT,0) * case when  ( DATEDIFF(MONTH, case when CONVERT(date,ss.EXC_FROM,103) > CONVERT(date,x.class_start,103) then CONVERT(date,ss.EXC_FROM,103) else  CONVERT(date,x.class_start,103) end, case when CONVERT(date,ss.EXC_TO,103) < CONVERT(date,GETDATE(),103) then CONVERT(date,ss.EXC_TO,103) else CONVERT(date,GETDATE(),103) end )+1) <0 then 0 else  ( DATEDIFF(MONTH, case when CONVERT(date,ss.EXC_FROM,103) > CONVERT(date,x.class_start,103) then CONVERT(date,ss.EXC_FROM,103) else  CONVERT(date,x.class_start,103) end, case when CONVERT(date,ss.EXC_TO,103) < CONVERT(date,GETDATE(),103) then CONVERT(date,ss.EXC_TO,103) else CONVERT(date,GETDATE(),103) end )+1) end
from STD_WAIVER ss where( ss.STUDENT_ID=x.student_id) and ss.WAIVE_YEAR=x.class_year) ),0) WV_Amt,
 
 (select std_roll from std_current_status k where x.student_id=k.student_id)Roll,(select sec_name from section_info where sec_id=(select sect from std_current_status k where k.student_id=x.student_id))section from  (select student_id, class_start,sect, class_id,class_year,previous_last_class from std_current_status ) x,  student_info y where x.student_id=y.student_id  and x.class_id='" + ClassId + "')tot     order by  tot.class_id, tot.section, CONVERT(int, Roll) asc";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PaymentDtls");
            return dt;
        }

        public static DataTable UnAdmittedStudentList(string ClassId)
        {
            String connectionString = DataManager.OraConnString();
            string query = @"select tot.*,pay_amt-WV_Amt-paid_amt-Discount_AMT bal from (select x.student_id,dbo.initcap(rtrim(ltrim(f_name+' '+m_name+' '+l_name))) name,(select class_name from class_info z where z.class_id= x.class_id)class_id,x.class_year, (select isnull(SUM(pay_amt),0) from payment_info where (pay_class is null OR pay_class=x.class_id) and (pay_type='Y' or (for_all_std='Y' and pay_type = case when x.previous_last_class='' OR x.previous_last_class IS NULL  then 'N' else 'O'end  )))+((select isnull(SUM(pay_amt),0) from payment_info where(pay_class is null OR pay_class=x.class_id) and pay_type='M') * case when (DATEDIFF(MONTH,CONVERT(date,x.class_start,103),CONVERT(date,GETDATE(),103))+1)<0 then 0 else  (DATEDIFF(MONTH,CONVERT(date,x.class_start,103),CONVERT(date,GETDATE(),103))+1) end ) pay_amt, (select isnull(SUM(Discount_AMT),0)from PAYMENT_DTL a,PAYMENT_MST b where a.PAYMENT_ID=b.PAYMENT_ID and b.STUDENT_ID=x.student_id  and b.class_id=x.class_id and b.PAY_YEAR=x.class_year) Discount_AMT,  (select isnull(SUM(pay_amt),0)from PAYMENT_DTL a,PAYMENT_MST b where a.PAYMENT_ID=b.PAYMENT_ID and b.STUDENT_ID=x.student_id  and b.class_id=x.class_id and b.PAY_YEAR=x.class_year)paid_amt,isnull(((select isnull( ss.WAIVE_PCT,0) * case when  ( DATEDIFF(MONTH, case when CONVERT(date,ss.EXC_FROM,103) > CONVERT(date,x.class_start,103) then CONVERT(date,ss.EXC_FROM,103) else  CONVERT(date,x.class_start,103) end, case when CONVERT(date,ss.EXC_TO,103) < CONVERT(date,GETDATE(),103) then CONVERT(date,ss.EXC_TO,103) else CONVERT(date,GETDATE(),103) end )+1) <0 then 0 else  ( DATEDIFF(MONTH, case when CONVERT(date,ss.EXC_FROM,103) > CONVERT(date,x.class_start,103) then CONVERT(date,ss.EXC_FROM,103) else  CONVERT(date,x.class_start,103) end, case when CONVERT(date,ss.EXC_TO,103) < CONVERT(date,GETDATE(),103) then CONVERT(date,ss.EXC_TO,103) else CONVERT(date,GETDATE(),103) end )+1) end
from STD_WAIVER ss where( ss.STUDENT_ID=x.student_id) and ss.WAIVE_YEAR=x.class_year) ),0) WV_Amt,(select std_roll from std_current_status k where x.student_id=k.student_id)Roll,(select sec_name from section_info where CONVERT(int,sec_id)=(select sect from std_current_status k where k.student_id=x.student_id))section from  (select student_id, class_start, CONVERT(int,class_id)  class_id, class_year ,previous_last_class  from std_current_status group by student_id, class_start,class_id,class_year,previous_last_class )x,  student_info y where x.student_id=y.student_id and x.class_id='" + ClassId + "' )tot where paid_amt=0 order by tot.class_id,tot.section, CONVERT(int, Roll) asc";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PaymentDtls");
            return dt;
        }

        public static DataTable ClassAndSectionWiseAdmissionFeePaymentPaidDueList(string ClassId,string Sec)
        {
            String connectionString = DataManager.OraConnString();
            string query = @"select tot.*,pay_amt-WV_Amt-paid_amt-Discount_AMT bal from (select x.student_id,dbo.initcap(rtrim(ltrim(f_name+' '+m_name+' '+l_name))) name,(select class_name from class_info z where  z.class_id=  x.class_id)class_id,x.class_year,

(select isnull(SUM(pay_amt),0) from payment_info where (pay_class is null OR pay_class=x.class_id) and (pay_type='Y' or (for_all_std='Y' and pay_type = case when x.previous_last_class='' OR x.previous_last_class IS NULL  then 'N' else 'O'end  ))) pay_amt,

 (select isnull(SUM(a.pay_amt),0)from PAYMENT_DTL a,PAYMENT_MST b,payment_info c where a.PAYMENT_ID=b.PAYMENT_ID and a.PAY_ID=c.pay_id and (pay_class is null OR pay_class=x.class_id) and (pay_type='Y' or (for_all_std='Y' and pay_type = case when x.previous_last_class='' OR x.previous_last_class IS NULL  then 'N' else 'O'end))   and b.STUDENT_ID=x.student_id  and  b.class_id= x.class_id and b.PAY_YEAR=x.class_year) paid_amt, (select isnull(SUM(Discount_AMT),0)from PAYMENT_DTL a,PAYMENT_MST b where a.PAYMENT_ID=b.PAYMENT_ID and b.STUDENT_ID=x.student_id  and b.class_id=x.class_id and b.PAY_YEAR=x.class_year) Discount_AMT,isnull(((select isnull( ss.WAIVE_PCT,0) * case when  ( DATEDIFF(MONTH, case when CONVERT(date,ss.EXC_FROM,103) > CONVERT(date,x.class_start,103) then CONVERT(date,ss.EXC_FROM,103) else  CONVERT(date,x.class_start,103) end, case when CONVERT(date,ss.EXC_TO,103) < CONVERT(date,GETDATE(),103) then CONVERT(date,ss.EXC_TO,103) else CONVERT(date,GETDATE(),103) end )+1) <0 then 0 else  ( DATEDIFF(MONTH, case when CONVERT(date,ss.EXC_FROM,103) > CONVERT(date,x.class_start,103) then CONVERT(date,ss.EXC_FROM,103) else  CONVERT(date,x.class_start,103) end, case when CONVERT(date,ss.EXC_TO,103) < CONVERT(date,GETDATE(),103) then CONVERT(date,ss.EXC_TO,103) else CONVERT(date,GETDATE(),103) end )+1) end
from STD_WAIVER ss where( ss.STUDENT_ID=x.student_id) and ss.WAIVE_YEAR=x.class_year) ),0) WV_Amt,
 
 (select std_roll from std_current_status k where x.student_id=k.student_id)Roll,(select sec_name from section_info where sec_id=(select sect from std_current_status k where k.student_id=x.student_id))section from  (select student_id, class_start,sect, class_id,class_year,previous_last_class from std_current_status ) x,  student_info y where x.student_id=y.student_id  and x.class_id='" + ClassId + "' and x.sect='" + Sec + "'  )tot     order by  tot.class_id, tot.section, CONVERT(int, Roll) asc";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PaymentDtls");
            return dt;
        }

        public static DataTable ClassWiseTutionFeePaymentPaidDueList(string ClassId)
        {
            String connectionString = DataManager.OraConnString();
            string query = @"select tot.*,pay_amt-WV_Amt-paid_amt  bal from (select x.student_id,dbo.initcap(rtrim(ltrim(f_name+' '+m_name+' '+l_name))) name,(select class_name from class_info z where  z.class_id=  x.class_id)class_id,x.class_year,

(select isnull(SUM(pay_amt),0) from payment_info where(pay_class is null OR pay_class= x.class_id) and pay_type='M') MonthlyFee,
(case when (DATEDIFF(MONTH,CONVERT(date,x.class_start,103),CONVERT(date,GETDATE(),103))+1)<0 then 0 else  (DATEDIFF(MONTH,CONVERT(date,x.class_start,103),CONVERT(date,GETDATE(),103))+1) end) TotalMonth,

((select isnull(SUM(pay_amt),0) from payment_info where(pay_class is null OR pay_class= x.class_id) and pay_type='M') 
* case when (DATEDIFF(MONTH,CONVERT(date,x.class_start,103),CONVERT(date,GETDATE(),103))+1)<0 then 0 else  (DATEDIFF(MONTH,CONVERT(date,x.class_start,103),CONVERT(date,GETDATE(),103))+1) end ) pay_amt, 

 (select isnull(SUM(a.pay_amt),0)from PAYMENT_DTL a,PAYMENT_MST b,payment_info c where a.PAYMENT_ID=b.PAYMENT_ID and a.PAY_ID=c.pay_id and (pay_class is null OR pay_class=x.class_id) and c.pay_type='M' and b.STUDENT_ID=x.student_id  and  b.class_id= x.class_id and b.PAY_YEAR=x.class_year) paid_amt,
 
  isnull(((select isnull( ss.WAIVE_PCT,0) * case when  ( DATEDIFF(MONTH, case when CONVERT(date,ss.EXC_FROM,103) > CONVERT(date,x.class_start,103) then CONVERT(date,ss.EXC_FROM,103) else  CONVERT(date,x.class_start,103) end, case when CONVERT(date,ss.EXC_TO,103) < CONVERT(date,GETDATE(),103) then CONVERT(date,ss.EXC_TO,103) else CONVERT(date,GETDATE(),103) end )+1) <0 then 0 else  ( DATEDIFF(MONTH, case when CONVERT(date,ss.EXC_FROM,103) > CONVERT(date,x.class_start,103) then CONVERT(date,ss.EXC_FROM,103) else  CONVERT(date,x.class_start,103) end, case when CONVERT(date,ss.EXC_TO,103) < CONVERT(date,GETDATE(),103) then CONVERT(date,ss.EXC_TO,103) else CONVERT(date,GETDATE(),103) end )+1) end
from STD_WAIVER ss where( ss.STUDENT_ID=x.student_id) and ss.WAIVE_YEAR=x.class_year) ),0) WV_Amt,
 
 (select std_roll from std_current_status k where x.student_id=k.student_id)Roll,(select sec_name from section_info where sec_id=(select sect from std_current_status k where k.student_id=x.student_id))section from  (select student_id, class_start,sect, class_id,class_year,previous_last_class from std_current_status ) x,  student_info y where x.student_id=y.student_id and x.class_id='" + ClassId + "' )tot     order by  tot.class_id, tot.section, CONVERT(int, Roll) asc";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PaymentDtls");
            return dt;
        }

        public static DataTable ClassAndSecWiseTutionFeePaymentPaidDueList(string ClassId,string Sec)
        {
            String connectionString = DataManager.OraConnString();
            string query = @"select tot.*,pay_amt-WV_Amt-paid_amt  bal from (select x.student_id,dbo.initcap(rtrim(ltrim(f_name+' '+m_name+' '+l_name))) name,(select class_name from class_info z where  z.class_id=  x.class_id)class_id,x.class_year,

(select isnull(SUM(pay_amt),0) from payment_info where(pay_class is null OR pay_class= x.class_id) and pay_type='M') MonthlyFee,
(case when (DATEDIFF(MONTH,CONVERT(date,x.class_start,103),CONVERT(date,GETDATE(),103))+1)<0 then 0 else  (DATEDIFF(MONTH,CONVERT(date,x.class_start,103),CONVERT(date,GETDATE(),103))+1) end) TotalMonth,

((select isnull(SUM(pay_amt),0) from payment_info where(pay_class is null OR pay_class= x.class_id) and pay_type='M') 
* case when (DATEDIFF(MONTH,CONVERT(date,x.class_start,103),CONVERT(date,GETDATE(),103))+1)<0 then 0 else  (DATEDIFF(MONTH,CONVERT(date,x.class_start,103),CONVERT(date,GETDATE(),103))+1) end ) pay_amt, 

 (select isnull(SUM(a.pay_amt),0)from PAYMENT_DTL a,PAYMENT_MST b,payment_info c where a.PAYMENT_ID=b.PAYMENT_ID and a.PAY_ID=c.pay_id and (pay_class is null OR pay_class=x.class_id) and c.pay_type='M' and b.STUDENT_ID=x.student_id  and  b.class_id= x.class_id and b.PAY_YEAR=x.class_year) paid_amt,
 
  isnull(((select isnull( ss.WAIVE_PCT,0) * case when  ( DATEDIFF(MONTH, case when CONVERT(date,ss.EXC_FROM,103) > CONVERT(date,x.class_start,103) then CONVERT(date,ss.EXC_FROM,103) else  CONVERT(date,x.class_start,103) end, case when CONVERT(date,ss.EXC_TO,103) < CONVERT(date,GETDATE(),103) then CONVERT(date,ss.EXC_TO,103) else CONVERT(date,GETDATE(),103) end )+1) <0 then 0 else  ( DATEDIFF(MONTH, case when CONVERT(date,ss.EXC_FROM,103) > CONVERT(date,x.class_start,103) then CONVERT(date,ss.EXC_FROM,103) else  CONVERT(date,x.class_start,103) end, case when CONVERT(date,ss.EXC_TO,103) < CONVERT(date,GETDATE(),103) then CONVERT(date,ss.EXC_TO,103) else CONVERT(date,GETDATE(),103) end )+1) end
from STD_WAIVER ss where( ss.STUDENT_ID=x.student_id) and ss.WAIVE_YEAR=x.class_year) ),0) WV_Amt,
 
 (select std_roll from std_current_status k where x.student_id=k.student_id)Roll,(select sec_name from section_info where sec_id=(select sect from std_current_status k where k.student_id=x.student_id))section from  (select student_id, class_start,sect, class_id,class_year,previous_last_class from std_current_status ) x,  student_info y where x.student_id=y.student_id and x.class_id='" + ClassId + "' and x.sect='" + Sec + "'  )tot     order by  tot.class_id, tot.section, CONVERT(int, Roll) asc";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PaymentDtls");
            return dt;
        }

        public static DataTable getDueListAllStudentClassAndSection(string Class, string Sec, string Shift, string Version, int Year)
        {
            String connectionString = DataManager.OraConnString();
//            string query = @" select tot.*,pay_amt-WV_Amt-paid_amt-Discount_AMT bal from (select x.student_id,dbo.initcap(rtrim(ltrim(f_name+' '+m_name+' '+l_name))) name,(select class_name from class_info z where z.class_id= x.class_id)class_id,x.class_year, (select isnull(SUM(pay_amt),0) from payment_info where (pay_class is null OR pay_class=x.class_id) and (pay_type='Y' or (for_all_std='Y' and pay_type = case when x.previous_last_class='' OR x.previous_last_class IS NULL  then 'N' else 'O'end  )))+((select isnull(SUM(pay_amt),0) from payment_info where(pay_class is null OR pay_class=x.class_id) and pay_type='M') * case when (DATEDIFF(MONTH,CONVERT(date,x.class_start,103),CONVERT(date,GETDATE(),103))+1)<0 then 0 else  (DATEDIFF(MONTH,CONVERT(date,x.class_start,103),CONVERT(date,GETDATE(),103))+1) end ) pay_amt, (select isnull(SUM(Discount_AMT),0)from PAYMENT_DTL a,PAYMENT_MST b where a.PAYMENT_ID=b.PAYMENT_ID and b.STUDENT_ID=x.student_id  and b.class_id=x.class_id and b.PAY_YEAR=x.class_year) Discount_AMT,  (select isnull(SUM(pay_amt),0)from PAYMENT_DTL a,PAYMENT_MST b where a.PAYMENT_ID=b.PAYMENT_ID and b.STUDENT_ID=x.student_id  and b.class_id=x.class_id and b.PAY_YEAR=x.class_year)paid_amt,isnull(((select isnull( ss.WAIVE_PCT,0) * case when  ( DATEDIFF(MONTH, case when CONVERT(date,ss.EXC_FROM,103) > CONVERT(date,x.class_start,103) then CONVERT(date,ss.EXC_FROM,103) else  CONVERT(date,x.class_start,103) end, case when CONVERT(date,ss.EXC_TO,103) < CONVERT(date,GETDATE(),103) then CONVERT(date,ss.EXC_TO,103) else CONVERT(date,GETDATE(),103) end )+1) <0 then 0 else  ( DATEDIFF(MONTH, case when CONVERT(date,ss.EXC_FROM,103) > CONVERT(date,x.class_start,103) then CONVERT(date,ss.EXC_FROM,103) else  CONVERT(date,x.class_start,103) end, case when CONVERT(date,ss.EXC_TO,103) < CONVERT(date,GETDATE(),103) then CONVERT(date,ss.EXC_TO,103) else CONVERT(date,GETDATE(),103) end )+1) end
//from STD_WAIVER ss where( ss.STUDENT_ID=x.student_id) and ss.WAIVE_YEAR=x.class_year) ),0) WV_Amt,(select std_roll from std_current_status k where x.student_id=k.student_id)Roll,(select sec_name from section_info where CONVERT(int,sec_id)=(select sect from std_current_status k where k.student_id=x.student_id))section from  (select student_id, class_start, CONVERT(int,class_id)  class_id, class_year ,previous_last_class  from std_current_status group by student_id, class_start,class_id,class_year,previous_last_class )x,  student_info y where x.student_id=y.student_id and x.class_id='" + Class + "' and (select sect from std_current_status k where k.student_id=x.student_id)='" + Sec + "')tot where paid_amt-pay_amt<-100 order by tot.class_id,tot.section, CONVERT(int, Roll) asc ";
            SqlConnection conn = new SqlConnection(DataManager.OraConnString());
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = new SqlCommand("SP_AccStudentTransaction", conn);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            if (Class != "Select Class")
            {
                da.SelectCommand.Parameters.AddWithValue("@class", Class);
            }
            if (Sec != "")
            {
                da.SelectCommand.Parameters.AddWithValue("@section", Sec);
            }
            if (Shift != "")
            {
                da.SelectCommand.Parameters.AddWithValue("@Shift", Shift);
            }
            if (Version != "")
            {
                da.SelectCommand.Parameters.AddWithValue("@verson", Version);
            }

            da.SelectCommand.Parameters.AddWithValue("@Year", Year);
            DataSet ds = new DataSet();
            da.Fill(ds, "SP_AccStudentTransaction");
            ds.Tables[0].TableName = "SP_AccStudentTransaction";
            return ds.Tables[0];
        }
        public static DataTable getDueListAllStudentId(string Id)
        {
            String connectionString = DataManager.OraConnString();
            string query = @"select tot.*,pay_amt-WV_Amt-paid_amt-Discount_AMT bal from (select x.student_id,dbo.initcap(rtrim(ltrim(f_name+' '+m_name+' '+l_name))) name,(select class_name from class_info z where z.class_id= x.class_id)class_id,x.class_year,(select isnull(SUM(pay_amt),0) from payment_info where (pay_class is null OR pay_class=x.class_id) and (pay_type='Y' or  pay_type IN ('O','N')))+((select isnull(SUM(pay_amt),0) from payment_info where(pay_class is null OR pay_class=x.class_id) and pay_type='M') *case when (DATEDIFF(MONTH,CONVERT(date,x.class_start,103),CONVERT(date,GETDATE(),103))+1)<0 then 0 else  (DATEDIFF(MONTH,CONVERT(date,x.class_start,103),CONVERT(date,GETDATE(),103))+1) end ) pay_amt,isnull(((select isnull( ss.WAIVE_PCT,0) * case when  ( DATEDIFF(MONTH, case when CONVERT(date,ss.EXC_FROM,103) > CONVERT(date,x.class_start,103) then CONVERT(date,ss.EXC_FROM,103) else  CONVERT(date,x.class_start,103) end, case when CONVERT(date,ss.EXC_TO,103) < CONVERT(date,GETDATE(),103) then CONVERT(date,ss.EXC_TO,103) else CONVERT(date,GETDATE(),103) end )+1) <0 then 0 else  ( DATEDIFF(MONTH, case when CONVERT(date,ss.EXC_FROM,103) > CONVERT(date,x.class_start,103) then CONVERT(date,ss.EXC_FROM,103) else  CONVERT(date,x.class_start,103) end, case when CONVERT(date,ss.EXC_TO,103) < CONVERT(date,GETDATE(),103) then CONVERT(date,ss.EXC_TO,103) else CONVERT(date,GETDATE(),103) end )+1) end
from STD_WAIVER ss where( ss.STUDENT_ID=x.student_id) and ss.WAIVE_YEAR=x.class_year) ),0) WV_Amt,  (select isnull(SUM(pay_amt),0)from PAYMENT_DTL a,PAYMENT_MST b where a.PAYMENT_ID=b.PAYMENT_ID and b.STUDENT_ID=x.student_id  and b.class_id=x.class_id and b.PAY_YEAR=x.class_year)paid_amt, (select isnull(SUM(Discount_AMT),0)from PAYMENT_DTL a,PAYMENT_MST b where a.PAYMENT_ID=b.PAYMENT_ID and b.STUDENT_ID=x.student_id  and b.class_id=x.class_id and b.PAY_YEAR=x.class_year) Discount_AMT,(select std_roll from std_current_status k where x.student_id=k.student_id)Roll,(select sec_name from section_info where sec_id=(select sect from std_current_status k where k.student_id=x.student_id))section from  (select student_id,max(class_start) class_start,MAX(class_id) class_id,MAX(class_year)class_year from std_current_status group by student_id) x,  student_info y where x.student_id=y.student_id and x.student_id='" + Id + "')tot where paid_amt-pay_amt<-100 order by CONVERT(int, Roll) asc";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PaymentDtls");
            return dt;
        }
        //******************************************** Paid List ***********************************************//
        public static DataTable getPaidListAllStudent(int Year)
        {
//            String connectionString = DataManager.OraConnString();
//            string query = @"select tot.*,pay_amt-WV_Amt-paid_amt-Discount_AMT bal from (select x.student_id,dbo.initcap(rtrim(ltrim(f_name+' '+m_name+' '+l_name))) name,(select class_name from class_info z where z.class_id= x.class_id)class_id,x.class_year, (select isnull(SUM(pay_amt),0) from payment_info where (pay_class is null OR pay_class=x.class_id) and (pay_type='Y' or (for_all_std='Y' and pay_type = case when x.previous_last_class='' OR x.previous_last_class IS NULL  then 'N' else 'O'end  )))+((select isnull(SUM(pay_amt),0) from payment_info where(pay_class is null OR pay_class=x.class_id) and pay_type='M') * case when (DATEDIFF(MONTH,CONVERT(date,x.class_start,103),CONVERT(date,GETDATE(),103))+1)<0 then 0 else  (DATEDIFF(MONTH,CONVERT(date,x.class_start,103),CONVERT(date,GETDATE(),103))+1) end ) pay_amt,isnull(((select isnull( ss.WAIVE_PCT,0) * case when  ( DATEDIFF(MONTH, case when CONVERT(date,ss.EXC_FROM,103) > CONVERT(date,x.class_start,103) then CONVERT(date,ss.EXC_FROM,103) else  CONVERT(date,x.class_start,103) end, case when CONVERT(date,ss.EXC_TO,103) < CONVERT(date,GETDATE(),103) then CONVERT(date,ss.EXC_TO,103) else CONVERT(date,GETDATE(),103) end )+1) <0 then 0 else  ( DATEDIFF(MONTH, case when CONVERT(date,ss.EXC_FROM,103) > CONVERT(date,x.class_start,103) then CONVERT(date,ss.EXC_FROM,103) else  CONVERT(date,x.class_start,103) end, case when CONVERT(date,ss.EXC_TO,103) < CONVERT(date,GETDATE(),103) then CONVERT(date,ss.EXC_TO,103) else CONVERT(date,GETDATE(),103) end )+1) end
//from STD_WAIVER ss where( ss.STUDENT_ID=x.student_id) and ss.WAIVE_YEAR=x.class_year) ),0) WV_Amt,(select isnull(SUM(Discount_AMT),0)from PAYMENT_DTL a,PAYMENT_MST b where a.PAYMENT_ID=b.PAYMENT_ID and b.STUDENT_ID=x.student_id  and b.class_id=x.class_id and b.PAY_YEAR=x.class_year) Discount_AMT,  (select isnull(SUM(pay_amt),0)from PAYMENT_DTL a,PAYMENT_MST b where a.PAYMENT_ID=b.PAYMENT_ID and b.STUDENT_ID=x.student_id  and b.class_id=x.class_id and b.PAY_YEAR=x.class_year)paid_amt,(select std_roll from std_current_status k where x.student_id=k.student_id)Roll,(select sec_name from section_info where CONVERT(int,sec_id)=(select sect from std_current_status k where k.student_id=x.student_id))section from  (select student_id, class_start, CONVERT(int,class_id)  class_id, class_year ,previous_last_class  from std_current_status group by student_id, class_start,class_id,class_year,previous_last_class )x,  student_info y where x.student_id=y.student_id  )tot where pay_amt-paid_amt<=0 and  paid_amt>0 order by tot.class_id,tot.section, CONVERT(int, Roll) asc";
//            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PaymentDtls");
//            return dt;

            SqlConnection conn = new SqlConnection(DataManager.OraConnString());
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = new SqlCommand("SP_AccStudentTransaction", conn);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            //if (Class != "Select Class")
            //{
            //    da.SelectCommand.Parameters.AddWithValue("@class", Class);
            //}
            //if (Sec != "")
            //{
            //    da.SelectCommand.Parameters.AddWithValue("@section", Sec);
            //}
            //if (Shift != "")
            //{
            //    da.SelectCommand.Parameters.AddWithValue("@Shift", Shift);
            //}
            //if (Version != "")
            //{
            //    da.SelectCommand.Parameters.AddWithValue("@verson", Version);
            //}
            da.SelectCommand.Parameters.AddWithValue("@Year", Year);
            DataSet ds = new DataSet();
            da.Fill(ds, "SP_AccStudentTransaction");
            ds.Tables[0].TableName = "SP_AccStudentTransaction";
            return ds.Tables[0];
        }
        public static DataTable getPaidListAllClassViseStudent(string Class)
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = @"select tot.*,pay_amt-WV_Amt-paid_amt-Discount_AMT bal from (select x.student_id,dbo.initcap(rtrim(ltrim(f_name+' '+m_name+' '+l_name))) name,(select class_name from class_info z where z.class_id= x.class_id)class_id,x.class_year,(select isnull(SUM(pay_amt),0) from payment_info where (pay_class is null OR pay_class=x.class_id) and (pay_type='Y' or  pay_type IN ('O','N')))+((select isnull(SUM(pay_amt),0) from payment_info where(pay_class is null OR pay_class=x.class_id) and pay_type='M') * case when (DATEDIFF(MONTH,CONVERT(date,x.class_start,103),CONVERT(date,GETDATE(),103))+1)<0 then 0 else  (DATEDIFF(MONTH,CONVERT(date,x.class_start,103),CONVERT(date,GETDATE(),103))+1) end ) pay_amt, (select isnull(SUM(Discount_AMT),0)from PAYMENT_DTL a,PAYMENT_MST b where a.PAYMENT_ID=b.PAYMENT_ID and b.STUDENT_ID=x.student_id  and b.class_id=x.class_id and b.PAY_YEAR=x.class_year) Discount_AMT,isnull(((select isnull( ss.WAIVE_PCT,0) * case when  ( DATEDIFF(MONTH, case when CONVERT(date,ss.EXC_FROM,103) > CONVERT(date,x.class_start,103) then CONVERT(date,ss.EXC_FROM,103) else  CONVERT(date,x.class_start,103) end, case when CONVERT(date,ss.EXC_TO,103) < CONVERT(date,GETDATE(),103) then CONVERT(date,ss.EXC_TO,103) else CONVERT(date,GETDATE(),103) end )+1) <0 then 0 else  ( DATEDIFF(MONTH, case when CONVERT(date,ss.EXC_FROM,103) > CONVERT(date,x.class_start,103) then CONVERT(date,ss.EXC_FROM,103) else  CONVERT(date,x.class_start,103) end, case when CONVERT(date,ss.EXC_TO,103) < CONVERT(date,GETDATE(),103) then CONVERT(date,ss.EXC_TO,103) else CONVERT(date,GETDATE(),103) end )+1) end
from STD_WAIVER ss where( ss.STUDENT_ID=x.student_id) and ss.WAIVE_YEAR=x.class_year) ),0) WV_Amt, (select isnull(SUM(pay_amt),0)from PAYMENT_DTL a,PAYMENT_MST b where a.PAYMENT_ID=b.PAYMENT_ID and b.STUDENT_ID=x.student_id  and b.class_id=x.class_id and b.PAY_YEAR=x.class_year)paid_amt,(select std_roll from std_current_status k where x.student_id=k.student_id)Roll,(select sec_name from section_info where Convert(int,sec_id)=(select sect from std_current_status k where k.student_id=x.student_id))section from  (select student_id,max(class_start) class_start,MAX(Convert(int,class_id)) class_id,MAX(class_year)class_year from std_current_status group by student_id) x,  student_info y where x.student_id=y.student_id and x.class_id='" + Class + "')tot where paid_amt-pay_amt>-100 and paid_amt>0 order by tot.class_id,tot.section, CONVERT(int, Roll) asc ";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Payments");
            return dt;
        }
        // paid list class wise and section
        public static DataTable getPaidListAllStudentClassAndSection(string Class, string Sec)
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = @" select tot.*,pay_amt-WV_Amt-paid_amt-Discount_AMT bal from (select x.student_id,dbo.initcap(rtrim(ltrim(f_name+' '+m_name+' '+l_name))) name,(select class_name from class_info z where z.class_id= x.class_id)class_id,x.class_year, (select isnull(SUM(pay_amt),0) from payment_info where (pay_class is null OR pay_class=x.class_id) and (pay_type='Y' or (for_all_std='Y' and pay_type = case when x.previous_last_class='' OR x.previous_last_class IS NULL  then 'N' else 'O'end  )))+((select isnull(SUM(pay_amt),0) from payment_info where(pay_class is null OR pay_class=x.class_id) and pay_type='M') * case when (DATEDIFF(MONTH,CONVERT(date,x.class_start,103),CONVERT(date,GETDATE(),103))+1)<0 then 0 else  (DATEDIFF(MONTH,CONVERT(date,x.class_start,103),CONVERT(date,GETDATE(),103))+1) end ) pay_amt,isnull(((select isnull( ss.WAIVE_PCT,0) * case when  ( DATEDIFF(MONTH, case when CONVERT(date,ss.EXC_FROM,103) > CONVERT(date,x.class_start,103) then CONVERT(date,ss.EXC_FROM,103) else  CONVERT(date,x.class_start,103) end, case when CONVERT(date,ss.EXC_TO,103) < CONVERT(date,GETDATE(),103) then CONVERT(date,ss.EXC_TO,103) else CONVERT(date,GETDATE(),103) end )+1) <0 then 0 else  ( DATEDIFF(MONTH, case when CONVERT(date,ss.EXC_FROM,103) > CONVERT(date,x.class_start,103) then CONVERT(date,ss.EXC_FROM,103) else  CONVERT(date,x.class_start,103) end, case when CONVERT(date,ss.EXC_TO,103) < CONVERT(date,GETDATE(),103) then CONVERT(date,ss.EXC_TO,103) else CONVERT(date,GETDATE(),103) end )+1) end
from STD_WAIVER ss where( ss.STUDENT_ID=x.student_id) and ss.WAIVE_YEAR=x.class_year) ),0) WV_Amt,  (select isnull(SUM(pay_amt),0)from PAYMENT_DTL a,PAYMENT_MST b where a.PAYMENT_ID=b.PAYMENT_ID and b.STUDENT_ID=x.student_id  and b.class_id=x.class_id and b.PAY_YEAR=x.class_year)paid_amt, (select isnull(SUM(Discount_AMT),0)from PAYMENT_DTL a,PAYMENT_MST b where a.PAYMENT_ID=b.PAYMENT_ID and b.STUDENT_ID=x.student_id  and b.class_id=x.class_id and b.PAY_YEAR=x.class_year) Discount_AMT,(select std_roll from std_current_status k where x.student_id=k.student_id)Roll,(select sec_name from section_info where CONVERT(int,sec_id)=(select sect from std_current_status k where k.student_id=x.student_id))section from  (select student_id, class_start, CONVERT(int,class_id)  class_id, class_year ,previous_last_class  from std_current_status group by student_id, class_start,class_id,class_year,previous_last_class )x,  student_info y where x.student_id=y.student_id  and x.class_id='" + Class + "' and (select sect from std_current_status k where k.student_id=x.student_id)='" + Sec + "')tot where pay_amt-paid_amt<=0 and  paid_amt>0 order by tot.class_id,tot.section, CONVERT(int, Roll) asc ";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Payments");
            return dt;
        }
        // paid list id
        public static DataTable getPaidListAllStudentStudentId(string Id)
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = @"  select tot.*,pay_amt-WV_Amt-paid_amt-Discount_AMT bal from (select x.student_id,dbo.initcap(rtrim(ltrim(f_name+' '+m_name+' '+l_name))) name,(select class_name from class_info z where z.class_id= x.class_id)class_id,x.class_year,(select isnull(SUM(pay_amt),0) from payment_info where (pay_class is null OR pay_class=x.class_id) and (pay_type='Y' or  pay_type IN ('O','N')))+((select isnull(SUM(pay_amt),0) from payment_info where(pay_class is null OR pay_class=x.class_id) and pay_type='M') * case when (DATEDIFF(MONTH,CONVERT(date,x.class_start,103),CONVERT(date,GETDATE(),103))+1)<0 then 0 else  (DATEDIFF(MONTH,CONVERT(date,x.class_start,103),CONVERT(date,GETDATE(),103))+1) end ) pay_amt,isnull(((select isnull( ss.WAIVE_PCT,0) * case when  ( DATEDIFF(MONTH, case when CONVERT(date,ss.EXC_FROM,103) > CONVERT(date,x.class_start,103) then CONVERT(date,ss.EXC_FROM,103) else  CONVERT(date,x.class_start,103) end, case when CONVERT(date,ss.EXC_TO,103) < CONVERT(date,GETDATE(),103) then CONVERT(date,ss.EXC_TO,103) else CONVERT(date,GETDATE(),103) end )+1) <0 then 0 else  ( DATEDIFF(MONTH, case when CONVERT(date,ss.EXC_FROM,103) > CONVERT(date,x.class_start,103) then CONVERT(date,ss.EXC_FROM,103) else  CONVERT(date,x.class_start,103) end, case when CONVERT(date,ss.EXC_TO,103) < CONVERT(date,GETDATE(),103) then CONVERT(date,ss.EXC_TO,103) else CONVERT(date,GETDATE(),103) end )+1) end
from STD_WAIVER ss where( ss.STUDENT_ID=x.student_id) and ss.WAIVE_YEAR=x.class_year) ),0) WV_Amt, (select isnull(SUM(Discount_AMT),0)from PAYMENT_DTL a,PAYMENT_MST b where a.PAYMENT_ID=b.PAYMENT_ID and b.STUDENT_ID=x.student_id  and b.class_id=x.class_id and b.PAY_YEAR=x.class_year) Discount_AMT,  (select isnull(SUM(pay_amt),0)from PAYMENT_DTL a,PAYMENT_MST b where a.PAYMENT_ID=b.PAYMENT_ID and b.STUDENT_ID=x.student_id  and b.class_id=x.class_id and b.PAY_YEAR=x.class_year)paid_amt,(select std_roll from std_current_status k where x.student_id=k.student_id)Roll,(select sec_name from section_info where sec_id=(select sect from std_current_status k where k.student_id=x.student_id))section from  (select student_id,max(class_start) class_start,MAX(class_id) class_id,MAX(class_year)class_year from std_current_status group by student_id) x,  student_info y where x.student_id=y.student_id and x.student_id='" + Id + "' )tot where paid_amt-pay_amt>-100 and paid_amt>0 order by CONVERT(int, Roll) asc";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Payments");
            return dt;
        }
        //**************************************************** Payment List ***************************************//
        public static DataTable getPaymetListAllStudent()
        {
            String connectionString = DataManager.OraConnString();
            string query = @"  select tot.*,pay_amt-WV_Amt-paid_amt-Discount_AMT bal from (select x.student_id,dbo.initcap(rtrim(ltrim(f_name+' '+m_name+' '+l_name))) name,(select class_name from class_info z where z.class_id= x.class_id)class_id,x.class_year,(select isnull(SUM(pay_amt),0) from payment_info where (pay_class is null OR pay_class=x.class_id) and (pay_type='Y' or  pay_type IN ('O','N')))+((select isnull(SUM(pay_amt),0) from payment_info where(pay_class is null OR pay_class=x.class_id) and pay_type='M') *case when (DATEDIFF(MONTH,CONVERT(date,x.class_start,103),CONVERT(date,GETDATE(),103))+1)<0 then 0 else  (DATEDIFF(MONTH,CONVERT(date,x.class_start,103),CONVERT(date,GETDATE(),103))+1) end ) pay_amt,isnull(((select isnull( ss.WAIVE_PCT,0) * case when  ( DATEDIFF(MONTH, case when CONVERT(date,ss.EXC_FROM,103) > CONVERT(date,x.class_start,103) then CONVERT(date,ss.EXC_FROM,103) else  CONVERT(date,x.class_start,103) end, case when CONVERT(date,ss.EXC_TO,103) < CONVERT(date,GETDATE(),103) then CONVERT(date,ss.EXC_TO,103) else CONVERT(date,GETDATE(),103) end )+1) <0 then 0 else  ( DATEDIFF(MONTH, case when CONVERT(date,ss.EXC_FROM,103) > CONVERT(date,x.class_start,103) then CONVERT(date,ss.EXC_FROM,103) else  CONVERT(date,x.class_start,103) end, case when CONVERT(date,ss.EXC_TO,103) < CONVERT(date,GETDATE(),103) then CONVERT(date,ss.EXC_TO,103) else CONVERT(date,GETDATE(),103) end )+1) end
from STD_WAIVER ss where( ss.STUDENT_ID=x.student_id) and ss.WAIVE_YEAR=x.class_year) ),0) WV_Amt,(select isnull(SUM(Discount_AMT),0)from PAYMENT_DTL a,PAYMENT_MST b where a.PAYMENT_ID=b.PAYMENT_ID and b.STUDENT_ID=x.student_id  and b.class_id=x.class_id and b.PAY_YEAR=x.class_year) Discount_AMT,  (select isnull(SUM(pay_amt),0)from PAYMENT_DTL a,PAYMENT_MST b where a.PAYMENT_ID=b.PAYMENT_ID and b.STUDENT_ID=x.student_id  and b.class_id=x.class_id and b.PAY_YEAR=x.class_year)paid_amt,(select std_roll from std_current_status k where x.student_id=k.student_id)Roll,(select sec_name from section_info where sec_id=(select sect from std_current_status k where k.student_id=x.student_id))section from  (select student_id,max(class_start) class_start,MAX(class_id) class_id,MAX(class_year)class_year from std_current_status group by student_id) x,  student_info y where x.student_id=y.student_id)tot where paid_amt-pay_amt<-100 order by paid_amt-pay_amt desc";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PaymentDtls");
            return dt;
        }
        public static DataTable getPaymetListAllStudentClassWise(string Class)
        {
            String connectionString = DataManager.OraConnString();
            string query = @" select tot.*,pay_amt-WV_Amt-paid_amt-Discount_AMT bal from (select x.student_id,dbo.initcap(rtrim(ltrim(f_name+' '+m_name+' '+l_name))) name,(select class_name from class_info z where z.class_id= x.class_id)class_id,x.class_year,(select isnull(SUM(pay_amt),0) from payment_info where (pay_class is null OR pay_class=x.class_id) and (pay_type='Y' or  pay_type IN ('O','N')))+((select isnull(SUM(pay_amt),0) from payment_info where(pay_class is null OR pay_class=x.class_id) and pay_type='M') * case when (DATEDIFF(MONTH,CONVERT(date,x.class_start,103),CONVERT(date,GETDATE(),103))+1)<0 then 0 else  (DATEDIFF(MONTH,CONVERT(date,x.class_start,103),CONVERT(date,GETDATE(),103))+1) end ) pay_amt,isnull(((select isnull( ss.WAIVE_PCT,0) * case when  ( DATEDIFF(MONTH, case when CONVERT(date,ss.EXC_FROM,103) > CONVERT(date,x.class_start,103) then CONVERT(date,ss.EXC_FROM,103) else  CONVERT(date,x.class_start,103) end, case when CONVERT(date,ss.EXC_TO,103) < CONVERT(date,GETDATE(),103) then CONVERT(date,ss.EXC_TO,103) else CONVERT(date,GETDATE(),103) end )+1) <0 then 0 else  ( DATEDIFF(MONTH, case when CONVERT(date,ss.EXC_FROM,103) > CONVERT(date,x.class_start,103) then CONVERT(date,ss.EXC_FROM,103) else  CONVERT(date,x.class_start,103) end, case when CONVERT(date,ss.EXC_TO,103) < CONVERT(date,GETDATE(),103) then CONVERT(date,ss.EXC_TO,103) else CONVERT(date,GETDATE(),103) end )+1) end
from STD_WAIVER ss where( ss.STUDENT_ID=x.student_id) and ss.WAIVE_YEAR=x.class_year) ),0) WV_Amt, (select isnull(SUM(Discount_AMT),0)from PAYMENT_DTL a,PAYMENT_MST b where a.PAYMENT_ID=b.PAYMENT_ID and b.STUDENT_ID=x.student_id  and b.class_id=x.class_id and b.PAY_YEAR=x.class_year) Discount_AMT, (select isnull(SUM(pay_amt),0)from PAYMENT_DTL a,PAYMENT_MST b where a.PAYMENT_ID=b.PAYMENT_ID and b.STUDENT_ID=x.student_id  and b.class_id=x.class_id and b.PAY_YEAR=x.class_year)paid_amt,(select std_roll from std_current_status k where x.student_id=k.student_id)Roll,(select sec_name from section_info where sec_id=(select sect from std_current_status k where k.student_id=x.student_id))section from  (select student_id,max(class_start) class_start,MAX(class_id) class_id,MAX(class_year)class_year from std_current_status group by student_id) x,  student_info y where x.student_id=y.student_id and x.class_id='" + Class + "')tot order by CONVERT(int, Roll) asc";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PaymentDtls");
            return dt;
        }
     
        public static DataTable getPaymetListAllStudentClassAndSec(string Class, string sec)
        {
            String connectionString = DataManager.OraConnString();
            string query = @" select tot.*,pay_amt-WV_Amt-paid_amt-Discount_AMT bal from (select x.student_id,dbo.initcap(rtrim(ltrim(f_name+' '+m_name+' '+l_name))) name,(select class_name from class_info z where z.class_id= x.class_id)class_id,x.class_year,(select isnull(SUM(pay_amt),0) from payment_info where (pay_class is null OR pay_class=x.class_id) and (pay_type='Y' or  pay_type IN ('O','N')))+((select isnull(SUM(pay_amt),0) from payment_info where(pay_class is null OR pay_class=x.class_id) and pay_type='M') * case when (DATEDIFF(MONTH,CONVERT(date,x.class_start,103),CONVERT(date,GETDATE(),103))+1)<0 then 0 else  (DATEDIFF(MONTH,CONVERT(date,x.class_start,103),CONVERT(date,GETDATE(),103))+1) end ) pay_amt,isnull(((select isnull( ss.WAIVE_PCT,0) * case when  ( DATEDIFF(MONTH, case when CONVERT(date,ss.EXC_FROM,103) > CONVERT(date,x.class_start,103) then CONVERT(date,ss.EXC_FROM,103) else  CONVERT(date,x.class_start,103) end, case when CONVERT(date,ss.EXC_TO,103) < CONVERT(date,GETDATE(),103) then CONVERT(date,ss.EXC_TO,103) else CONVERT(date,GETDATE(),103) end )+1) <0 then 0 else  ( DATEDIFF(MONTH, case when CONVERT(date,ss.EXC_FROM,103) > CONVERT(date,x.class_start,103) then CONVERT(date,ss.EXC_FROM,103) else  CONVERT(date,x.class_start,103) end, case when CONVERT(date,ss.EXC_TO,103) < CONVERT(date,GETDATE(),103) then CONVERT(date,ss.EXC_TO,103) else CONVERT(date,GETDATE(),103) end )+1) end
from STD_WAIVER ss where( ss.STUDENT_ID=x.student_id) and ss.WAIVE_YEAR=x.class_year) ),0) WV_Amt, (select isnull(SUM(Discount_AMT),0)from PAYMENT_DTL a,PAYMENT_MST b where a.PAYMENT_ID=b.PAYMENT_ID and b.STUDENT_ID=x.student_id  and b.class_id=x.class_id and b.PAY_YEAR=x.class_year) Discount_AMT, (select isnull(SUM(pay_amt),0)from PAYMENT_DTL a,PAYMENT_MST b where a.PAYMENT_ID=b.PAYMENT_ID and b.STUDENT_ID=x.student_id  and b.class_id=x.class_id and b.PAY_YEAR=x.class_year)paid_amt,(select std_roll from std_current_status k where x.student_id=k.student_id)Roll,(select sec_name from section_info where sec_id=(select sect from std_current_status k where k.student_id=x.student_id))section from  (select student_id,max(class_start) class_start,MAX(class_id) class_id,MAX(class_year)class_year from std_current_status group by student_id) x,  student_info y where x.student_id=y.student_id and x.class_id='" + Class + "' and (select sect from std_current_status k where k.student_id=x.student_id)='" + sec + "')tot  order by CONVERT(int, Roll) asc ";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PaymentDtls");
            return dt;
        }
        public static DataTable getPaymetListAllStudentId(string id)
        {
            String connectionString = DataManager.OraConnString();
            string query = @" select tot.*,pay_amt-WV_Amt-paid_amt-Discount_AMT bal from (select x.student_id,dbo.initcap(rtrim(ltrim(f_name+' '+m_name+' '+l_name))) name,(select class_name from class_info z where z.class_id= x.class_id)class_id,x.class_year,(select isnull(SUM(pay_amt),0) from payment_info where (pay_class is null OR pay_class=x.class_id) and (pay_type='Y' or  pay_type IN ('O','N')))+((select isnull(SUM(pay_amt),0) from payment_info where(pay_class is null OR pay_class=x.class_id) and pay_type='M') * case when (DATEDIFF(MONTH,CONVERT(date,x.class_start,103),CONVERT(date,GETDATE(),103))+1)<0 then 0 else  (DATEDIFF(MONTH,CONVERT(date,x.class_start,103),CONVERT(date,GETDATE(),103))+1) end ) pay_amt, (select isnull(SUM(Discount_AMT),0)from PAYMENT_DTL a,PAYMENT_MST b where a.PAYMENT_ID=b.PAYMENT_ID and b.STUDENT_ID=x.student_id  and b.class_id=x.class_id and b.PAY_YEAR=x.class_year) Discount_AMT,isnull(((select isnull( ss.WAIVE_PCT,0) * case when  ( DATEDIFF(MONTH, case when CONVERT(date,ss.EXC_FROM,103) > CONVERT(date,x.class_start,103) then CONVERT(date,ss.EXC_FROM,103) else  CONVERT(date,x.class_start,103) end, case when CONVERT(date,ss.EXC_TO,103) < CONVERT(date,GETDATE(),103) then CONVERT(date,ss.EXC_TO,103) else CONVERT(date,GETDATE(),103) end )+1) <0 then 0 else  ( DATEDIFF(MONTH, case when CONVERT(date,ss.EXC_FROM,103) > CONVERT(date,x.class_start,103) then CONVERT(date,ss.EXC_FROM,103) else  CONVERT(date,x.class_start,103) end, case when CONVERT(date,ss.EXC_TO,103) < CONVERT(date,GETDATE(),103) then CONVERT(date,ss.EXC_TO,103) else CONVERT(date,GETDATE(),103) end )+1) end
from STD_WAIVER ss where( ss.STUDENT_ID=x.student_id) and ss.WAIVE_YEAR=x.class_year) ),0) WV_Amt, (select isnull(SUM(pay_amt),0)from PAYMENT_DTL a,PAYMENT_MST b where a.PAYMENT_ID=b.PAYMENT_ID and b.STUDENT_ID=x.student_id  and b.class_id=x.class_id and b.PAY_YEAR=x.class_year)paid_amt,(select std_roll from std_current_status k where x.student_id=k.student_id)Roll,(select sec_name from section_info where sec_id=(select sect from std_current_status k where k.student_id=x.student_id))section from  (select student_id,max(class_start) class_start,MAX(class_id) class_id,MAX(class_year)class_year from std_current_status group by student_id) x,  student_info y where x.student_id=y.student_id and x.student_id='" + id + "')tot where paid_amt-pay_amt<-100 order by CONVERT(int, Roll) asc ";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PaymentDtls");
            return dt;
        }
        //**********************************************  getTutionFee ***********************************************//
        public static string getTutionFee(string std,string yr)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            string Query = "select tot.pay_amt from ( " +
            " select x.student_id,dbo.initcap(rtrim(ltrim(f_name+' '+m_name+' '+l_name))) name,x.class_id,x.class_year, " +
            " (select isnull(SUM(pay_amt),0) from payment_info where pay_class=x.class_id and (pay_type='Y' or  pay_type IN ('O','N')))+ " +
            " ((select isnull(SUM(pay_amt),0) from payment_info where pay_class=x.class_id and pay_type='M')* " +
            " case when ((MONTH(convert(datetime,nullif('" + yr + "',''),103))-1)-(MONTH(convert(datetime,nullif(x.class_start,''),103))))<0 then 0 else " +
            " ((MONTH(convert(datetime,nullif('" + yr + "',''),103))-1)-(MONTH(convert(datetime,nullif(x.class_start,''),103)))) end ) pay_amt " +
            " from (select student_id,max(class_start) class_start,MAX(class_id) class_id,MAX(class_year) class_year from std_current_status group by student_id) x, " +
            " student_info y where x.student_id=y.student_id) tot where student_id='" + std + "' ";

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
        
        public static string getPaidAmount(string std,string yr)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            string Query = " select tot.paid_amt from ( " +
            " select x.student_id,dbo.initcap(rtrim(ltrim(f_name+' '+m_name+' '+l_name))) name,x.class_id,x.class_year, " +
            " (select isnull(SUM(pay_amt),0) from payment_info where pay_class=x.class_id and (pay_type='Y' or  pay_type IN ('O','N')))+ " +
            " ((select isnull(SUM(pay_amt),0) from payment_info where pay_class=x.class_id and pay_type='M')* " +
            " case when ((MONTH(convert(datetime,nullif('" + yr + "',''),103))-1)-(MONTH(convert(datetime,nullif(x.class_start,''),103))))<0 then 0 else " +
            " ((MONTH(convert(datetime,nullif('" + yr + "',''),103))-1)-(MONTH(convert(datetime,nullif(x.class_start,''),103)))) end ) pay_amt, " +
            " (select isnull(SUM(pay_amt),0) from PAYMENT_DTL a,PAYMENT_MST b where a.PAYMENT_ID=b.PAYMENT_ID and b.STUDENT_ID=x.student_id " +
            " and b.class_id=x.class_id and b.PAY_YEAR=x.class_year) paid_amt from " +
            " (select student_id,max(class_start) class_start,MAX(class_id) class_id,MAX(class_year) class_year from std_current_status group by student_id) x, " +
            " student_info y where x.student_id=y.student_id) tot where student_id='" + std + "' and SUBSTRING (class_year,0,5)=Year(convert(datetime,nullif('" + yr + "',''),103))";
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
        public static DataTable GetStdPaymentInfos(string std, string yr, string pdt)
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = "select pay_id,pay_head_id,pay_amt,coalesce(paid_amt,0) paid_amt,coalesce(pay_amt,0)-coalesce(paid_amt,0) due_amt from (select pay_id, case when pay_type='M' then pay_head_id + ' x ' + (select convert(varchar,datediff(month,class_start,convert(datetime,'" + pdt + "',103))) " +
            " from std_current_status where student_id='" + std + "' and class_year='" + yr + "') + ' month(s)' else pay_head_id end pay_head_id, " +
            " case when pay_type='M' then convert(decimal(13,2),pay_amt*(select datediff(month,class_start,convert(datetime,'" + pdt + "',103)) from std_current_status " +
            " where student_id='" + std + "' and class_year='" + yr + "')+ coalesce(pay_amt*(select datediff(month,class_start,convert(datetime,'" + pdt + "',103)) from std_current_status " +
            " where student_id='" + std + "' and class_year='" + yr + "')*(select waive_pct/100 from std_waiver where student_id='" + std + "' and waive_year='" + yr + "'),0)) else pay_amt end pay_amt, " +
            " (select sum(pay_amt) from payment_mst a inner join payment_dtl b on (a.payment_id=b.payment_id and a.student_id='" + std + "' and pay_year='" + yr + "' and b.pay_id=pay.pay_id)) paid_amt from payment_info pay " +
            " where coalesce(pay_class,(select class_id from std_current_status where student_id='" + std + "' " +
            " and class_year='" + yr + "')) = (select class_id from std_current_status where student_id='" + std + "' and class_year='" + yr + "') and for_all_std='Y') as stdpay union " +
            " select pay_id,pay_head_id,coalesce(pay_amt,0) pay_amt,coalesce(paid_amt,0) paid_amt,coalesce(pay_amt,0)-coalesce(paid_amt,0) due_amt from ( select a.pay_id,case when b.pay_type='M' then b.pay_head_id +' x ' + " +
            " (select convert(varchar,datediff(month,class_start,convert(datetime,'" + pdt + "',103))) from std_current_status  where student_id='" + std + "' and class_year='" + yr + "') + ' month(s)' " +
            " else b.pay_head_id end pay_head_id, case when b.pay_type='M' then a.pay_amt *  (select datediff(month,class_start,convert(datetime,'" + pdt + "',103)) from std_current_status  where student_id='" + std + "' and class_year='" + yr + "') " +
            " else a.pay_amt end pay_amt, (select sum(pay_amt) from payment_mst pm inner join payment_dtl pd on (pm.payment_id=pd.payment_id and pm.student_id='" + std + "' and pay_year='" + yr + "' and pd.pay_id=a.pay_id)) paid_amt " +
            " from std_special_pay a,payment_info b  where a.pay_id=b.pay_id and student_id='" + std + "' and class_year='" + yr + "') as tot1";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Payments");
            return dt;            
        }

        
        public static DataTable GetStdCommonPayments(string std,string year,string Version,string PayID)
        {
            string asdsd = "";
            if (PayID != "")
            {
                asdsd = "and pm.PAYMENT_ID<" + PayID;
            }
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = @" with tot as(select pin.pay_id,hd.Head_Name as pay_head_id,case when pay_type IN ('O','N') then 'Y' else pay_type end as pay_type,discount,case when pay_type != 'E' then ISNULL(pay_amt,0) when (pay_type = 'E' and pay_amt=0) then  ISNULL(pay.paid_amt,0) else ISNULL(pay_amt,0) end as[pay_amt] ,ISNULL(pay.paid_amt,0) paid_amt, ISNULL(pay.Discount_AMT,0) Discount_AMT,ISNULL((pay_amt-pay.Discount_AMT-pay.paid_amt),0) Totalpaid_amt, convert(varchar,cur.class_start,103) class_start
from payment_info pin inner join payment_head hd on hd.ID=pin.pay_head_id
left join  (select  class_id, class_year, [version],class_start ,previous_last_class,group_name
from std_current_status  
where student_id='" + std + "') cur on coalesce(pin.pay_class,cur.class_id)=cur.class_id and for_all_std='Y' and pin.GroupID in (cur.group_name,'',NULL) and pin.[version]='" + Version + "' and pay_type != case when cur.previous_last_class='' OR cur.previous_last_class IS NULL  then 'O' else 'N'end  left join (select class_id,pay_year,pd.pay_id,sum(pay_amt) paid_amt,sum(pd.Discount_AMT) Discount_AMT from payment_mst pm inner join payment_dtl pd  on (pm.payment_id=pd.payment_id and student_id='" + std + "' and pm.PAY_YEAR='" + year + "' " + asdsd + " ) group by class_id,pay_year,pd.pay_id) pay on (cur.class_id=pay.class_id and pin.pay_id=pay.pay_id) UNION ALL   select T5.pay_id,'Transport Fee','Y','',T5.TotalAmount,paid_amt, Discount_AMT,(paid_amt-Discount_AMT) Totalpaid_amt,  convert(varchar,class_start,103) class_start   from (SELECT T3.TMST_Std_Id,199 AS pay_id, SUM(T3.TotalAmount) AS TotalAmount  FROM (select T2.TMST_Std_Id , case when t1.To_Date<GETDATE() then DATEDIFF(MONTH, t1.From_Date, t1.To_Date)+1 else DATEDIFF(MONTH, t1.From_Date, GETDATE())+1 END * t1.T_Pay_amt as TotalAmount from dbo.TransportDtl t1  inner join TransportMst t2 on t2.TMST_Id=t1.Mst_Id and t1.From_Date<GETDATE() AND T1.[year]='" + year + "' AND T2.TMST_Std_Id= '" + std + "') T3  GROUP BY T3.TMST_Std_Id) T5   inner join  (select student_id,class_id,class_year, class_start from std_current_status  where student_id='" + std + "') cur on CUR.student_id=T5.TMST_Std_Id   left join (select class_id,pay_year,pd.pay_id,sum(pay_amt) paid_amt,sum(pd.Discount_AMT) Discount_AMT from payment_mst pm   inner join payment_dtl pd  on (pm.payment_id=pd.payment_id and student_id='" + std + "' and pm.PAY_YEAR='" + year + "' AND PD.PAY_ID=1999" + asdsd + " ) group by class_id,pay_year,pd.pay_id) pay on (cur.class_id=pay.class_id and T5.pay_id=pay.pay_id)) select * from tot where tot.pay_amt>0 ";
            // where tot.pay_amt>0
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "payment_info");
            return dt;

            //string connectionString = DataManager.OraConnString();
            //SqlConnection sqlCon = new SqlConnection(connectionString);

            //sqlCon.Open();
            //SqlDataAdapter da = new SqlDataAdapter();
            //da.SelectCommand = new SqlCommand("SP_AccStudentPaymentInfo", sqlCon);
            //da.SelectCommand.CommandType = CommandType.StoredProcedure;
            //da.SelectCommand.Parameters.AddWithValue("@StudentID", std);
            //da.SelectCommand.Parameters.AddWithValue("@Year", year);
            //if (PayID == "") { da.SelectCommand.Parameters.AddWithValue("@PayId", null); }
            //else { da.SelectCommand.Parameters.AddWithValue("@PayId", PayID); }
            //da.SelectCommand.Parameters.AddWithValue("@version", Version);

            //DataSet ds = new DataSet();
            //da.Fill(ds, "tableName");
            //DataTable dt = ds.Tables["tableName"];
            //return dt;
        }
        public static DataTable GetStdSpecialPayments(string std,string pdt)
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = "select pin.pay_id,case when mon>1 then pay_head_id+ ' x '+convert(varchar,mon) + ' month(s)' else pay_head_id end pay_head_id,spay.pay_amt,pay.paid_amt,coalesce(spay.pay_amt,0)-coalesce(pay.paid_amt,0) due_amt from payment_info pin " +
            " inner join (select max(class_id) class_id,max(class_year) class_year, max(class_start) class_start from std_current_status " +
            " where student_id='" + std + "') cur on (coalesce(pin.pay_class,cur.class_id)=cur.class_id and for_all_std='N') " +
            " inner join (select class_id,class_year,pay_id,sum(pay_amt*mon) pay_amt,sum(mon) mon from ( " +
            " select a.*, case when pay_type='M' then case when month(to_dt) <= month(getdate()) then datediff(month,from_dt,dateadd(day,1,to_dt)) " +
            " when month(to_dt) > month(getdate()) then datediff(month,from_dt,dateadd(day,1,DATEADD(s,-1,DATEADD(mm, DATEDIFF(m,0,convert(datetime,'" + pdt + "',103))+1,0)))) end else 1 end mon " +
            " from std_special_pay a,payment_info b where a.pay_id=b.pay_id and student_id='" + std + "' ) as xx  group by class_id,class_year,pay_id) spay " +
            " on (coalesce(pin.pay_class,spay.class_id)=spay.class_id and pin.pay_id=spay.pay_id) " +
            " left join (select class_id,pay_year,pd.pay_id,sum(pay_amt) paid_amt from payment_mst pm inner join payment_dtl pd " +
            " on (pm.payment_id=pd.payment_id and student_id='" + std + "') group by class_id,pay_year,pd.pay_id) pay" +
            " on (cur.class_id=pay.class_id and pin.pay_id=pay.pay_id)";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Payments");
            return dt;
        }
        //******************************** Due Not Admit Card Report ********************************//
        public static DataTable getStudentAdmitCardInformation()
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = @"select tot.*,paid_amt-pay_amt bal from (select x.student_id,dbo.initcap(rtrim(ltrim(f_name+' '+m_name+' '+l_name))) name,(select class_name from class_info z where z.class_id= x.class_id)class_id,x.class_year,(select isnull(SUM(pay_amt),0) from payment_info where (pay_class is null OR pay_class=x.class_id) and pay_type='Y')+((select isnull(SUM(pay_amt),0) from payment_info where(pay_class is null OR pay_class=x.class_id) and pay_type='M') * case when (DATEDIFF(MONTH,CONVERT(date,x.class_start,103),CONVERT(date,GETDATE(),103))+1)<0 then 0 else  (DATEDIFF(MONTH,CONVERT(date,x.class_start,103),CONVERT(date,GETDATE(),103))+1) end) pay_amt,isnull(((select isnull( ss.WAIVE_PCT,0) * case when  ( DATEDIFF(MONTH, case when CONVERT(date,ss.EXC_FROM,103) > CONVERT(date,x.class_start,103) then CONVERT(date,ss.EXC_FROM,103) else  CONVERT(date,x.class_start,103) end, case when CONVERT(date,ss.EXC_TO,103) < CONVERT(date,GETDATE(),103) then CONVERT(date,ss.EXC_TO,103) else CONVERT(date,GETDATE(),103) end )+1) <0 then 0 else  ( DATEDIFF(MONTH, case when CONVERT(date,ss.EXC_FROM,103) > CONVERT(date,x.class_start,103) then CONVERT(date,ss.EXC_FROM,103) else  CONVERT(date,x.class_start,103) end, case when CONVERT(date,ss.EXC_TO,103) < CONVERT(date,GETDATE(),103) then CONVERT(date,ss.EXC_TO,103) else CONVERT(date,GETDATE(),103) end )+1) end
from STD_WAIVER ss where( ss.STUDENT_ID=x.student_id) and ss.WAIVE_YEAR=x.class_year) ),0) WV_Amt, (select isnull(SUM(Discount_AMT),0)from PAYMENT_DTL a,PAYMENT_MST b where a.PAYMENT_ID=b.PAYMENT_ID and b.STUDENT_ID=x.student_id  and b.class_id=x.class_id and b.PAY_YEAR=x.class_year) Discount_AMT, (select isnull(SUM(pay_amt),0)from PAYMENT_DTL a,PAYMENT_MST b where a.PAYMENT_ID=b.PAYMENT_ID and b.STUDENT_ID=x.student_id  and b.class_id=x.class_id and b.PAY_YEAR=x.class_year)paid_amt,(select std_roll from std_current_status k where x.student_id=k.student_id)Roll,(select sec_name from section_info where sec_id=(select sect from std_current_status k where k.student_id=x.student_id))section from  (select student_id,max(class_start) class_start,MAX(class_id) class_id,MAX(class_year)class_year from std_current_status group by student_id) x,  student_info y where x.student_id=y.student_id)tot where paid_amt-pay_amt>-100 order by paid_amt-pay_amt desc";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Payments");
            return dt;
        }

        public static DataTable getStudentClassAndSectionWiseInfo(string ClassId, string SectionId, string Roll, string Shift, string Version,string year, string StudentID)
        {
            String connectionString = DataManager.OraConnString();
            string Parameter = "";
            if (StudentID != "") { Parameter = " and a.student_id='" + StudentID + "' "; }
            string query = @"select student_id,name, class_id,class_year,sect,std_roll 
from (select a.student_id,dbo.initcap(rtrim(ltrim(f_name+' '+m_name+' '+l_name))) name, max(a.class_id)class_id,sect,std_roll, max(a.class_year)class_year from std_current_status a,student_info b  where a.student_id=b.student_id and a.class_id='" + ClassId + "' and a.sect='" + SectionId + "' and a.class_year='" + year + "' and a.std_roll='" + Roll + "' and a.shift='" + Shift + "' and a.[version]='" + Version + "' " + Parameter + "  group by a.student_id,sect,std_roll,dbo.initcap(rtrim(ltrim(f_name+' '+m_name+' '+l_name)))) x";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PaymentDtls");
            return dt;
        }

        public static DataTable GetShowStudentCurrentInformation( string Year ,string ClassId, string SectionId)
        {
            String connectionString = DataManager.OraConnString();
            string query = @"SELECT [student_id]
,(Select b.f_name+' '+b.m_name+' '+b.l_name FROM student_info b Where b.student_id=a.student_id)As Student_Name
  ,Convert (int,std_roll) std_roll,'Transport Fee' Transport,case WHEN  c.Std_Id IS null THEN 'FALSE' else 'TRUE' END  as ckeck,c.Pay_Amt
   FROM std_current_status a  
left join Student_Transport c on a.student_id=c.Std_Id
WHERE  class_year='" + Year + "' AND class_id='" + ClassId + "' AND sect='" + SectionId + "' order by Convert (int,std_roll) ";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PaymentDtls");
            return dt;
        }


        public static DataTable GetShowStudentSelectInformation(string Year, string ClassId, string SectionId)
        {
            String connectionString = DataManager.OraConnString();
            string query = @"SELECT a.Std_Class,a.Std_Section, ci.class_name,si.sec_name, a.Std_Year,a.[Start_Date],a.[End_Date]  
FROM  Student_Transport a
inner join class_info ci on ci.class_id=a.Std_Class
inner join section_info si on si.sec_id=a.Std_Section
where a.Std_Year='" + Year + "'  and a.Std_Section='" + SectionId + "' and a.Std_Class='" + ClassId + "'   group by a.Std_Class,a.Std_Section,ci.class_name,si.sec_name,a.Std_Year,a.[Start_Date],a.[End_Date]";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PaymentDtls");
            return dt;
        }

        //public static StudentTransportMst GetShowStudentSelectChangeInformation(string ClassId, string SectionId, string Year)
        //{
//            String connectionString = DataManager.OraConnString();
//            string query = @" SELECT a.Std_Class,a.Std_Section, ci.class_name,si.sec_name, a.Std_Year,a.Pay_Amt,a.[Start_Date],a.[End_Date]  
//FROM  Student_Transport a
//inner join class_info ci on ci.class_id=a.Std_Class
//inner join section_info si on si.sec_id=a.Std_Section
//where   a.Std_Class='" + ClassId + "' and a.Std_Section='" + SectionId + "' and a.Std_Year='" + Year + "'  ";
//            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PaymentDtls");

//            if (dt.Rows.Count == 0)
//            {
//                return null;
//            }
//            return new StudentTransportMst(dt.Rows[0]);
            
        //}


        public decimal getShowPaymetHeadAmount(string HeadId)
        {
            SqlConnection connection = new SqlConnection(DataManager.OraConnString());
            connection.Open();
            string query = @"SELECT isnull([pay_amt],0) FROM [payment_info] where [pay_id]='" + HeadId + "'";
            SqlCommand command = new SqlCommand(query, connection);
            //connection.Close();
            return Convert.ToDecimal(command.ExecuteScalar());
           
        }

        public static DataTable getWaiverInformation(string StudentId, string ClassId, string Year)
        {
            String connectionString = DataManager.OraConnString();
            string query = @"SELECT [WAIVE_PCT],convert(nvarchar,[EXC_FROM],103)EXC_FROM,convert(nvarchar,[EXC_TO],103) EXC_TO     
  FROM [STD_WAIVER] where [STUDENT_ID]='" + StudentId + "' and [class_id]='" + ClassId + "' and [WAIVE_YEAR]='" + Year + "'";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PaymentDtls");
            return dt;
        }

        public static DataTable getSpecialPayment(string StudentId, string ClassId, string Year)
        {
            String connectionString = DataManager.OraConnString();
            string query = @"SELECT t1.pay_id 
      ,t2.pay_head_id
      ,t1.[pay_amt]
      ,t1.[from_dt]
      ,t1.[to_dt]
      ,t1.[serial_no]
  FROM [std_special_pay] t1
  inner join payment_info t2 on t2.pay_id=t1.pay_id where t1.[student_id]='" + StudentId + "' and t1.[class_id]='" + ClassId + "' and t1.[class_year]='" + Year + "'";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "std_special_pay");
            return dt;
        }

        public static DataTable getDueListAllStudentClassName()
        {
            String connectionString = DataManager.OraConnString();
            string query = @"select class_id,class_name from class_info";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PaymentDtls");
            return dt;
        }

        public static double GetShowTitionFees(string p)
        {
            SqlConnection connection = new SqlConnection(DataManager.OraConnString());
            connection.Open();
            string selectQuery=@"SELECT [pay_amt] FROM [payment_info] where [pay_type]='M' and [pay_class]='"+p+"'";
            SqlCommand command = new SqlCommand(selectQuery,connection);
            return Convert.ToDouble(command.ExecuteScalar());

        }

        public static string GetShowExtraChareFlag(string p)
        {
            SqlConnection connection = new SqlConnection(DataManager.OraConnString());
            connection.Open();
            string selectQuery = @"SELECT [pay_type] FROM [payment_info] where [pay_id]='" + p + "'";
            SqlCommand command = new SqlCommand(selectQuery, connection);
            return command.ExecuteScalar().ToString();
        }

        public static object GetShowAllHead(string Class, string Verson, string Shift, string section)
        {
            String connectionString = DataManager.OraConnString();
            string query = @"select class_id,class_name from class_info";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PaymentDtls");
            return dt;
        }

        public static DataTable getStudentInfoSerachOption(string strID)
        {
            String connectionString = DataManager.OraConnString();
            string query = @"select student_id,name,  class_id,class_year,sect,std_roll,[version],shift,class_name,sec_name,shift_name,version_name
from (select a.student_id,dbo.initcap(rtrim(ltrim(f_name+' '+m_name+' '+l_name))) name
,  max(a.class_id)class_id,a.sect,a.std_roll, max(a.class_year)class_year,a.[version],a.shift,c.class_name,sc.sec_name,sh.shift_name,vv.version_name
from std_current_status a inner join student_info b on a.student_id=b.student_id inner join class_info c on c.class_id=a.class_id left join section_info sc on sc.sec_id=a.sect inner join shift_info sh on sh.shift_id=a.shift left join version_info vv on vv.id=a.[version] where upper(b.[student_id]+' - '+b.[f_name]+' - '+c.class_name+' - '+sc.sec_name+' - '+vv.version_name+' - '+a.std_roll+' - '+b.mobile_no) = upper('" + strID + "') group by a.student_id,a.sect,a.std_roll,a.[version],a.shift,dbo.initcap(rtrim(ltrim(f_name+' '+m_name+' '+l_name))),c.class_name,sc.sec_name,sh.shift_name,vv.version_name) x";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PaymentDtls");
            return dt;
        }

        public static DataTable GetShowMonthlyPaymentHeadReport(string StrDate, string EndDate,string ClassID,string SectionID)
        {
            String connectionString = DataManager.OraConnString();
            string Parameter = "";
            if (ClassID != "0" && SectionID == "") { Parameter = " AND pm.class_id='" + ClassID + "'"; }
            else if (ClassID == "0" && SectionID != "") { Parameter = " and sf.shift_name='" + SectionID + "' "; }
            else if (ClassID != "0" && SectionID != "") { Parameter = " AND pm.class_id='" + ClassID + "' and sf.shift_name='" + SectionID + "'"; }
            
            string query = @"Select distinct  ph.ID,  ph.Head_Name    
from PAYMENT_DTL pd
inner join PAYMENT_MST pm on pm.PAYMENT_ID=pd.PAYMENT_ID 
inner join payment_info pf on pf.pay_id=pd.PAY_ID
inner join payment_head ph on ph.ID=pf.pay_head_id 
inner join shift_info sf on sf.shift_id=pm.shift
where CONVERT(date,pm.PAY_DATE,103) between CONVERT(date,'" + StrDate + "',103) and CONVERT(date,'" + EndDate + "',103) " + Parameter + " order by ph.ID   ";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PaymentDtls");
            return dt;
        }

        public static DataTable GetShowMonthlyPaymentHeadDetailsReport(string StrDate, string EndDate, string ClassID, string SectionID)
        {
            String connectionString = DataManager.OraConnString();

            string Parameter = "";
            if (ClassID != "0" && SectionID == "") { Parameter = " AND pm.class_id='" + ClassID + "'"; }
            else if (ClassID == "0" && SectionID != "") { Parameter = " and sf.shift_name='" + SectionID + "' "; }
            else if (ClassID != "0" && SectionID != "") { Parameter = " AND pm.class_id='" + ClassID + "' and sf.shift_name='" + SectionID + "'"; }

            string query = @"Select ph.ID, ph.Head_Name,DATENAME(month,pm.PAY_DATE)as monName,DATEPART(MM,pm.PAY_DATE)as mon,SUM(pd.PAY_AMT)Payment  
from PAYMENT_DTL pd
inner join PAYMENT_MST pm on pm.PAYMENT_ID=pd.PAYMENT_ID 
inner join payment_info pf on pf.pay_id=pd.PAY_ID inner join payment_head ph on ph.ID=pf.pay_head_id  inner join shift_info sf on sf.shift_id=pm.shift
where CONVERT(date,pm.PAY_DATE,103) between CONVERT(date,'" + StrDate + "',103) and CONVERT(date,'" + EndDate + "',103)  "+Parameter+"  group by  ph.ID, ph.Head_Name ,DATENAME(month,pm.PAY_DATE),DatePart(MM,pm.PAY_DATE) order by ph.ID  ";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PaymentDtls");
            return dt;
        }

        public static DataTable GetShowTotPayment(string StrDate, string EndDate, string ClassID, string SectionID)
        {
            String connectionString = DataManager.OraConnString();
            string Parameter = "";
            if (ClassID != "0" && SectionID == "") { Parameter = " AND pm.class_id='" + ClassID + "'"; }
            else if (ClassID == "0" && SectionID != "") { Parameter = " and sf.shift_name='" + SectionID + "' "; }
            else if (ClassID != "0" && SectionID != "") { Parameter = " AND pm.class_id='" + ClassID + "' and sf.shift_name='" + SectionID + "'"; }
            string query = @"Select SUM(pd.PAY_AMT)Payment
from PAYMENT_DTL pd
inner join PAYMENT_MST pm on pm.PAYMENT_ID=pd.PAYMENT_ID 
inner join payment_info pf on pf.pay_id=pd.PAY_ID inner join payment_head ph on ph.ID=pf.pay_head_id inner join shift_info sf on sf.shift_id=pm.shift where CONVERT(date,pm.PAY_DATE,103) between CONVERT(date,'" + StrDate + "',103) and CONVERT(date,'" + EndDate + "',103) " + Parameter + " group by ph.ID,ph.Head_Name order by ph.ID ";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PAYMENT_DTL");
            return dt;
        }
        //******************** Change By Shakhawat *******************//

//        public static DataTable getDailyByDateCollectionSummery(string sd, string ed, string shift,string Class)
//        {
////            String connectionString = DataManager.OraConnString();
////            string Parameter = "";
////            if (Class == "Select Class" || Class == "") { } else { Parameter = "AND b.class_id='" + Class + "'"; }
////            string query = @"with tt as (
//// SELECT  st.std_roll, b.student_id, ci.class_id,si.f_name as std_name,ci.class_name as std_class,se.sec_name,sh.shift_name,vi.version_name,pay.paid_amt as[Prv_Amt],
////(select isnull(SUM(pay_amt),0) from payment_info where (pay_class is null OR pay_class=b.class_id) and (pay_type='Y' or (for_all_std='Y' and pay_type = case when st.previous_last_class='' OR st.previous_last_class IS NULL  then 'N' else 'O'end  )))+((select isnull(SUM(pay_amt),0) from payment_info where(pay_class is null OR pay_class=b.class_id) and pay_type='M') *  case when (DATEDIFF(MONTH,CONVERT(date,st.class_start,103),CONVERT(date,GETDATE(),103))+1)<0 then 0 else  (DATEDIFF(MONTH,CONVERT(date,st.class_start,103),CONVERT(date,GETDATE(),103))+1) end ) pay_amt,
////sum(isnull(a.Discount_AMT,0))+isnull(pay.Discount_AMT,0) Discount_AMT,sum(round(a.pay_amt,2)) Paid  ,
////
////Isnull(((select isnull( ss.WAIVE_PCT,0) * case when  ( DATEDIFF(MONTH, case when CONVERT(date,ss.EXC_FROM,103) > CONVERT(date,st.class_start,103) then CONVERT(date,ss.EXC_FROM,103) else  CONVERT(date,st.class_start,103) end, case when CONVERT(date,ss.EXC_TO,103) < CONVERT(date,GETDATE(),103) then CONVERT(date,ss.EXC_TO,103) else CONVERT(date,GETDATE(),103) end )+1) <0 then 0 else  ( DATEDIFF(MONTH, case when CONVERT(date,ss.EXC_FROM,103) > CONVERT(date,st.class_start,103) then CONVERT(date,ss.EXC_FROM,103) else  CONVERT(date,st.class_start,103) end, case when CONVERT(date,ss.EXC_TO,103) < CONVERT(date,GETDATE(),103) then CONVERT(date,ss.EXC_TO,103) else CONVERT(date,GETDATE(),103) end )+1) end 
////from STD_WAIVER ss where( ss.STUDENT_ID=st.student_id) and ss.WAIVE_YEAR=st.class_year) ),0) WV_Amt
////     from PAYMENT_DTL a           
////    inner join PAYMENT_MST b on b.PAYMENT_ID=a.PAYMENT_ID and b.PAY_DATE   between convert(datetime,'" + sd + "',103) and     convert(datetime,'" + ed + "',103) inner join class_info ci on ci.class_id =b.class_id " + Parameter + " inner join student_info si on si.student_id=b.STUDENT_ID inner join std_current_status st on st.student_id=si.student_id   inner join payment_info pf on pf.pay_id=a.PAY_ID  inner join section_info se on se.sec_id=b.sect  inner join shift_info sh on sh.shift_id=b.shift AND sh.shift_name='" + shift + "' inner join version_info vi on vi.version_id=b.[version]  left join (select student_id,sum(pay_amt) paid_amt,sum(pd.Discount_AMT) Discount_AMT,pm.PAY_YEAR from payment_mst pm inner join payment_dtl pd  on pm.PAYMENT_ID=pd.PAYMENT_ID  and pm.PAY_DATE<convert(datetime,'" + sd + "',103) group by student_id,pm.PAY_YEAR) pay on pay.STUDENT_ID=b.STUDENT_ID and b.PAY_YEAR=pay.PAY_YEAR        group by st.std_roll,st.class_year, b.student_id,si.f_name,ci.class_name, b.class_id,st.previous_last_class,se.sec_name,st.class_start,sh.shift_name,st.student_id,ci.class_id, vi.version_name,pay.paid_amt,pay.Discount_AMT) select tt.std_roll, tt.STUDENT_ID as[student_id],tt.std_name as [std_name], tt.class_id,tt.std_class as std_class,tt.WV_Amt, tt.sec_name as sec_name,tt.shift_name as shift_name,tt.version_name as version_name,tt.pay_amt as pay_amt,isnull(tt.Discount_AMT,0) as Discount_AMT,isnull(tt.Prv_Amt,0) Prv_Amt,isnull(tt.pay_amt,0)-isnull(tt.Discount_AMT,0)-isnull(tt.Prv_Amt,0) as[GrandTotal],tt.Paid,(isnull(tt.pay_amt,0)-isnull(tt.Discount_AMT,0))-isnull(tt.Paid,0)-isnull(tt.Prv_Amt,0)-ISNULL(tt.WV_Amt,0) as[DueAmt] from tt order by  Convert(int,tt.class_id),Convert(int,tt.std_roll)";
////            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PaymentDtls");
////            return dt;
//        }

        public static DataTable getStdTotalPaymentShiftWise(string ClassID, string Shift, string Year)
        {
            SqlConnection connection = new SqlConnection(DataManager.OraConnString());
            connection.Open();
            try
            {               
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = new SqlCommand("SP_AccStudentTransaction_Shift", connection);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.AddWithValue("@class", ClassID);
                da.SelectCommand.Parameters.AddWithValue("@section", null);
                da.SelectCommand.Parameters.AddWithValue("@verson", null);
                da.SelectCommand.Parameters.AddWithValue("@Shift", null);
                da.SelectCommand.Parameters.AddWithValue("@StudentID", null);
                da.SelectCommand.Parameters.AddWithValue("@Year", Year);
                da.SelectCommand.Parameters.AddWithValue("@ShiftName", Shift);
                DataSet ds = new DataSet();
                da.Fill(ds, "tableName");
                DataTable tab = ds.Tables[0];
                return tab;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
        public static DataTable GetShowMonthlyPaymentHeadReport(string StrDate, string EndDate,string Shift)
        {
            String connectionString = DataManager.OraConnString();
            string Parameter = "";
            if (Shift == "") 
            { Parameter = "where CONVERT(date,pm.PAY_DATE,103) between CONVERT(date,'" + StrDate + "',103) and CONVERT(date,'" + EndDate + "',103) and sf.shift_name='" + Shift + "' "; }
            else
            { Parameter = "where CONVERT(date,pm.PAY_DATE,103) between CONVERT(date,'" + StrDate + "',103) and CONVERT(date,'" + EndDate + "',103) "; }
            string query = @"Select distinct  ph.ID,  ph.Head_Name    
from PAYMENT_DTL pd
inner join PAYMENT_MST pm on pm.PAYMENT_ID=pd.PAYMENT_ID 
inner join student_info stu on pm.STUDENT_ID=stu.student_id
inner join std_current_status cur on cur.student_id=stu.student_id
inner join payment_info pf on pf.pay_id=pd.PAY_ID
inner join payment_head ph on ph.ID=pf.pay_head_id 
inner join shift_info sf on sf.shift_id=cur.shift " + Parameter;
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PaymentDtls");
            return dt;
        }
        public static DataTable ShowAllStudentLedgerDetails(string Class, string Section, string StudentID, string Year, string StarDate, string EndDate,string Shift,string ClassID)
        {
            String connectionString = DataManager.OraConnString();
            string Parameter = "";
            if (ClassID!="" && Shift == "")
            { Parameter = "where cur.class_id='" + ClassID + "' and CONVERT(date,pm.PAY_DATE,103) between CONVERT(DATE,'" + StarDate + "',103) and CONVERT(DATE,'" + EndDate + "',103)  and  pd.PAY_AMT >0"; }
            else
            { Parameter = "where CONVERT(date,pm.PAY_DATE,103) between CONVERT(DATE,'" + StarDate + "',103) and CONVERT(DATE,'" + EndDate + "',103) and cur.class_id='" + ClassID + "' and sf.shift_name='" + Shift + "' and  pd.PAY_AMT >0"; }
            string query = @"Select DISTINCT stu.student_id,stu.f_name,case when '" + Year + "' = '' then cur.class_year else '" + Year + "' end as Cur_year from PAYMENT_DTL pd inner join PAYMENT_MST pm on pm.PAYMENT_ID=pd.PAYMENT_ID inner join student_info stu on pm.STUDENT_ID=stu.student_id inner join std_current_status cur on cur.student_id=stu.student_id  left join shift_info sf on sf.shift_id=cur.shift inner join payment_info pf on pf.pay_id=pd.PAY_ID inner join payment_head ph on ph.ID=pf.pay_head_id  " + Parameter;
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PAYMENT_DTL");
            return dt;
        }

        public static DataTable ShowAllStudentLedger(string StudentID, string Year, string StarDate, string EndDate, string Shift,string ClassID)
        {
            String connectionString = DataManager.OraConnString();
            string query = @"Select DISTINCT pm.PAYMENT_ID,Convert(nvarchar,pm.PAY_DATE,103) AS PAY_DATE
from PAYMENT_DTL pd
inner join PAYMENT_MST pm on pm.PAYMENT_ID=pd.PAYMENT_ID and pm.PAY_YEAR ='" + Year + "' inner join student_info stu on pm.STUDENT_ID=stu.student_id inner join payment_info pf on pf.pay_id=pd.PAY_ID inner join payment_head ph on ph.ID=pf.pay_head_id where stu.student_id='" + StudentID + "' AND  CONVERT(date,pm.PAY_DATE,103) between CONVERT(DATE,'" + StarDate + "',103) and CONVERT(DATE,'" + EndDate + "',103) ";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PAYMENT_DTL");
            return dt;
        }

        public static DataTable GetShowAllPaymentinPayIdAndStudentID(string StudentID, string PayID)
        {
            String connectionString = DataManager.OraConnString();
            string query = @"Select stu.f_name,pm.PAYMENT_ID,pm.PAY_DATE,ph.ID, ph.Head_Name,pd.PAY_AMT,cur.student_id
from PAYMENT_DTL pd
inner join PAYMENT_MST pm on pm.PAYMENT_ID=pd.PAYMENT_ID 
inner join student_info stu on pm.STUDENT_ID=stu.student_id
inner join std_current_status cur on cur.student_id=stu.student_id
inner join payment_info pf on pf.pay_id=pd.PAY_ID 
inner join payment_head ph on ph.ID=pf.pay_head_id where stu.student_id='" + StudentID + "'  and pm.PAYMENT_ID='" + PayID + "' and  pd.PAY_AMT >0 order by  stu.f_name,pm.PAYMENT_ID,pm.PAY_DATE";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PAYMENT_DTL");
            return dt;
        }

        public static DataTable GetShowTotPaymentOnAll(string StrDate, string EndDate,string Shift,string ClassID)
        {
            String connectionString = DataManager.OraConnString();
            string Parameter = "";
            if (ClassID!="" && Shift == "")
            { Parameter = "where pm.class_id='" + ClassID + "' and CONVERT(date,pm.PAY_DATE,103) between CONVERT(date,'" + StrDate + "',103) and CONVERT(date,'" + EndDate + "',103) group by ph.ID,ph.Head_Name order by ph.ID"; }
            if (ClassID != "" && Shift != "")
            { Parameter = "where pm.class_id='" + ClassID + "' and sf.shift_name='" + Shift + "' and CONVERT(date,pm.PAY_DATE,103) between CONVERT(date,'" + StrDate + "',103) and CONVERT(date,'" + EndDate + "',103) group by ph.ID,ph.Head_Name order by ph.ID"; }
            else
            { Parameter = "where CONVERT(date,pm.PAY_DATE,103) between CONVERT(date,'" + StrDate + "',103) and CONVERT(date,'" + EndDate + "',103) group by ph.ID,ph.Head_Name order by ph.ID"; }
            string query = @"Select SUM(pd.PAY_AMT)Payment
from PAYMENT_DTL pd
inner join PAYMENT_MST pm on pm.PAYMENT_ID=pd.PAYMENT_ID 
inner join student_info stu on pm.STUDENT_ID=stu.student_id
inner join std_current_status cur on cur.student_id=stu.student_id
inner join payment_info pf on pf.pay_id=pd.PAY_ID 
inner join payment_head ph on ph.ID=pf.pay_head_id
inner join shift_info sf on sf.shift_id=cur.shift  " + Parameter;
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PAYMENT_DTL");
            return dt;
        }

        public static DataTable GetShowMonthlyPaymentHeadReportDateWise(string StrDate, string EndDate, string ClassID, string Shift)
        {
            String connectionString = DataManager.OraConnString();
            string Parameter = "";
            if (ClassID != "" && Shift=="")
            { Parameter = "where cur.class_id='" + ClassID + "' and CONVERT(date,pm.PAY_DATE,103) between CONVERT(date,'" + StrDate + "',103) and CONVERT(date,'" + EndDate + "',103)"; }
            else if (ClassID != "" && Shift != "")
            { Parameter = "where cur.class_id='" + ClassID + "' and sf.shift_name='" + Shift + "' and  CONVERT(date,pm.PAY_DATE,103) between CONVERT(date,'" + StrDate + "',103) and CONVERT(date,'" + EndDate + "',103) "; }
            else
            { Parameter = "where CONVERT(date,pm.PAY_DATE,103) between CONVERT(date,'" + StrDate + "',103) and CONVERT(date,'" + EndDate + "',103) "; }
            string query = @"Select distinct  ph.ID,  ph.Head_Name    
from PAYMENT_DTL pd
inner join PAYMENT_MST pm on pm.PAYMENT_ID=pd.PAYMENT_ID 
inner join student_info stu on pm.STUDENT_ID=stu.student_id
inner join std_current_status cur on cur.student_id=stu.student_id
inner join payment_info pf on pf.pay_id=pd.PAY_ID
left join shift_info sf on sf.shift_id=cur.shift
inner join payment_head ph on ph.ID=pf.pay_head_id " + Parameter;
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PaymentDtls");
            return dt;
        }

        public static DataTable GetShowPaymentDropdown(string StudentID, string Year, string PayID)
        {
            String connectionString = DataManager.OraConnString();
            string query = @"select a.payment_id,a.pay_id,t1.Head_Name as pay_head_id,a.pay_amt,a. Discount_AMT ,t3.pay_type as[OtCharge_flag],case when t3.pay_type='E' then a.PAY_AMT else case when t3.pay_type='M' then t3.pay_amt * (DATEDIFF(MONTH,CONVERT(date,si.class_start,103),CONVERT(date,pms.PAY_DATE,103))+1) else t3.pay_amt end  end as[Fix_Amt],ISNULL(pay.paid_amt,0) as Previous_Pay,ISNULL(pay.Discount_AMT,0) Prv_Discount_AMT
 from payment_dtl a inner join PAYMENT_MST pms on pms.PAYMENT_ID=a.PAYMENT_ID  inner join std_current_status si on si.student_id=pms.STUDENT_ID  inner join payment_info t3 on t3.pay_id=a.PAY_ID  and a.PAYMENT_ID= 5786 inner join payment_head t1 on t1.ID=t3.pay_head_id    left join (select pd.pay_id,sum(pay_amt) paid_amt,sum(pd.Discount_AMT) Discount_AMT from payment_mst pm inner join payment_dtl pd  on (pm.payment_id=pd.payment_id and student_id='" + StudentID + "' and pm.PAY_YEAR='" + Year + "') group by class_id,pay_year,pd.pay_id) pay on  a.pay_id=pay.pay_id where pay.PAY_ID='" + PayID + "' ";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PAYMENT_DTL");
            return dt;
        }

        public static DataTable getCollectUserWisePayment(string StrDate, string EndDate, string user, string year)
        {
            String connectionString = DataManager.OraConnString();
            string query = @"select pm.STUDENT_ID,(t2.f_name+' '+t2.m_name+' '+t2.l_name) as std_name,t1.std_roll,t3.class_name,t4.shift_name,sum(pm.Total_Paid_Amt) as Total_Paid_Amt  from PAYMENT_MST pm 
inner join std_current_status t1 on t1.student_id=pm.STUDENT_ID
inner join student_info t2 on t1.student_id=t2.student_id
inner join class_info t3 on pm.class_id=t3.class_id
inner join shift_info t4 on t1.shift=t4.shift_id
where (convert(date,pm.PAY_DATE,103) between CONVERT(date,'" + StrDate + "',103) and CONVERT(date,'" + EndDate + "',103)) and pm.ENTRY_USER='" + user + "' and PAY_YEAR='" + year + "' group by pm.STUDENT_ID,(t2.f_name+' '+t2.m_name+' '+t2.l_name),t1.std_roll,t3.class_name,t4.shift_name";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "PaymentDtls");
            return dt;
        }

        public static DataTable getDailyByDateWithoutYearCollectionSummery(string sd, string ed, string Class, string Shif, string Sec)
        {
            SqlConnection conn = new SqlConnection(DataManager.OraConnString());
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = new SqlCommand("SP_AccStudentTransactionDateWise", conn);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;

            da.SelectCommand.Parameters.AddWithValue("@StartDate", sd);
            da.SelectCommand.Parameters.AddWithValue("@EndDate", ed);
            if (Sec != "")
            {
                da.SelectCommand.Parameters.AddWithValue("@section", Sec);
            }
            if (Class != "")
            {
                da.SelectCommand.Parameters.AddWithValue("@class", Class);
            }
            if (Shif != "")
            {
                da.SelectCommand.Parameters.AddWithValue("@Shift", Shif);
            }
            //if (Version != "")
            //{
            //    da.SelectCommand.Parameters.AddWithValue("@verson", Version);
            //}
            DataSet ds = new DataSet();
            da.Fill(ds, "SP_AccStudentTransactionDateWise");
            ds.Tables[0].TableName = "SP_AccStudentTransactionDateWise";
            return ds.Tables[0];
        }
    }
}