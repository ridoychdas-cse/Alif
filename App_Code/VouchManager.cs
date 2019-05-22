using System;
using System.Data;
using System.Configuration;
using System.Linq;

using System.Xml.Linq;
using System.Data.SqlClient;

/// <summary>
/// Summary description for VouchManager
/// </summary>
/// 
namespace KHSC
{
    public class VouchManager
    {
        public static void DeleteVouchMst(string vouchmst)
        {
            String connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = "delete from gl_trans_mst where vch_sys_no=" + vouchmst;
            DataManager.ExecuteNonQuery(connectionString, query);

           
        }

        public static void CreateVouchMst(VouchMst vouchmst)
        {
            String connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            
            string query= " insert into gl_trans_mst(vch_sys_no,fin_mon,value_date,vch_ref_no,"+
            " ref_file_no,volume_no,serial_no,vch_code,trans_type,particulars,control_amt,book_name,payee,check_no,cheq_date,"+
            " cheq_amnt,money_rpt_no,money_rpt_date,status,entry_user,entry_date,AUTHO_USER_TYPE) values (convert(numeric, case '" + vouchmst.VchSysNo + "' when '' then null else '" + vouchmst.VchSysNo + "' end), " +
            "  '" + vouchmst.FinMon + "', convert(datetime, nullif( '" + vouchmst.ValueDate + "',''), 103),'" + vouchmst.VchRefNo + "', " +
            "  '" + vouchmst.RefFileNo + "', '" + vouchmst.VolumeNo + "', '" + vouchmst.SerialNo + "','" + vouchmst.VchCode + "', "+
            " '" + vouchmst.TransType + "','" + vouchmst.Particulars + "', " +
            "  convert(decimal(13,2),nullif('" + vouchmst.ControlAmt.Replace(",", "") + "','') ) ,'" + vouchmst.BookName + "','" + vouchmst.Payee + "', " +
            "  '" + vouchmst.CheckNo + "', convert(datetime,nullif( '" + vouchmst.CheqDate + "',''),103), convert(decimal(13,2),nullif('" + vouchmst.CheqAmnt.Replace(",", "") + "','')), " +
            "  '" + vouchmst.MoneyRptNo + "', convert(datetime,nullif( '" + vouchmst.MoneyRptDate + "',''),103),'" + vouchmst.Status + "', " +
            "  '" + vouchmst.EntryUser.ToUpper() + "', convert (datetime, nullif('" + vouchmst.EntryDate + "',''),103),'"+vouchmst.AuthoUserType+"')";
                      

            DataManager.ExecuteNonQuery(connectionString, query);

        }
        public static DataTable getCoaCode(string coadesc)
        {
            string connectionString = DataManager.OraConnString();

            string query = "select a.gl_coa_code,b.seg_coa_desc from gl_coa a, gl_seg_coa b where a.coa_natural_code=b.seg_coa_code and coa_desc='" + coadesc + "'";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "CoaCode");
            return dt;
        }
        public static DataTable getCoaCode(string glcode, string coadesc)
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = "select a.gl_coa_code,b.seg_coa_desc from gl_coa a, gl_seg_coa b where a.coa_natural_code=b.seg_coa_code and (a.gl_coa_code='" + glcode + "' or coa_desc='" + coadesc + "')";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "CoaCode");
            return dt;
        }

        //public static DataTable getCoaCode(string coadesc)
        //{
        //    string connectionString = DataManager.OraConnString();

        //    string query = "select a.gl_coa_code,b.seg_coa_desc from gl_coa a, gl_seg_coa b where a.coa_natural_code=b.seg_coa_code and coa_desc='" + coadesc + "'";
        //    DataTable dt = DataManager.ExecuteQuery(connectionString, query, "CoaCode");
        //    return dt;
        //}
        public static string getCoaCodeByName(string coadesc)
        {
            string val;
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = "select a.gl_coa_code from gl_coa a, gl_seg_coa b where a.coa_natural_code=b.seg_coa_code and coa_desc='" + coadesc + "'";
            sqlCon.Open();
            SqlCommand myCommand = new SqlCommand(query, sqlCon);
            object maxValue = myCommand.ExecuteScalar();
            sqlCon.Close();
            if (maxValue == DBNull.Value) return "";
            val = maxValue.ToString();
            return val;
        }

        public static string getCoaDescByName(string coadesc)
        {
            string val;
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = "select b.seg_coa_desc from gl_coa a, gl_seg_coa b where a.coa_natural_code=b.seg_coa_code and coa_desc='" + coadesc + "'";
            sqlCon.Open();
            SqlCommand myCommand = new SqlCommand(query, sqlCon);
            object maxValue = myCommand.ExecuteScalar();
            sqlCon.Close();
            if (maxValue == DBNull.Value) return "";
            val = maxValue.ToString();
            return val;
        }

        public static DataTable getCoaDesc(string coa)
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = "select a.gl_coa_code,b.seg_coa_desc from gl_coa a, gl_seg_coa b where a.coa_natural_code=b.seg_coa_code and a.gl_coa_code='" + coa.ToString().Trim() + "'";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "CoaCode");
            return dt;
        }
        public static void UpdateVouchMst(VouchMst vouchmst)
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = " update gl_trans_mst set fin_mon='" + vouchmst.FinMon + "',value_date=convert(datetime,nullif('" + vouchmst.ValueDate + "',''),103), " +
                          " vch_ref_no='" + vouchmst.VchRefNo + "',ref_file_no='" + vouchmst.RefFileNo + "', " +
                          " volume_no= '" + vouchmst.VolumeNo + "',serial_no= '" + vouchmst.SerialNo + "', "+
                          " trans_type='" + vouchmst.TransType + "',vch_code='" + vouchmst.VchCode + "',particulars='" + vouchmst.Particulars + "', " +
                          " control_amt=convert(decimal(13,2),nullif('" + vouchmst.ControlAmt.Replace(",", "") + "','')),book_name='" + vouchmst.BookName + "', " +
                          " payee='" + vouchmst.Payee + "',check_no='" + vouchmst.CheckNo + "', " +
                          " cheq_date=convert(datetime,nullif('" + vouchmst.CheqDate + "',''),103),cheq_amnt=convert(decimal(13,2),nullif('" + vouchmst.CheqAmnt + "','')), " +
                          " money_rpt_no='" + vouchmst.MoneyRptNo + "',money_rpt_date=convert(datetime, nullif('" + vouchmst.MoneyRptDate + "',''),103), " +
                          " status='" + vouchmst.Status + "',update_user='" + vouchmst.UpdateUser + "', update_date=convert(datetime, nullif('" + vouchmst.UpdateDate + "',''),103), " +
                          " autho_user='" + vouchmst.AuthoUser + "', autho_date=convert(datetime,nullif('" + vouchmst.AuthoDate + "',''),103),autho_user_type='" + vouchmst.AuthoUserType + "' " +
                          " where vch_sys_no=convert(numeric,nullif('" + vouchmst.VchSysNo+"',''))";
            DataManager.ExecuteNonQuery(connectionString, query);
        }
        public static VouchMst GetVouchMst(string vchno)
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = "select convert(varchar, vch_sys_no) vch_sys_no,fin_mon,convert(varchar,value_date,103)value_date,vch_ref_no," +
            " ref_file_no,volume_no,serial_no,vch_code,trans_type,particulars,convert(varchar,control_amt) control_amt,book_name,payee,check_no,convert(varchar,cheq_date,103)cheq_date," +
            " convert(varchar,cheq_amnt) cheq_amnt,money_rpt_no,convert(varchar,money_rpt_date,103) money_rpt_date,entry_user,convert(varchar,entry_date,103) entry_date,update_user,convert(varchar,update_date,103) update_date, " +
                           " status,autho_user,convert(varchar,autho_date,103) autho_date,autho_user_type from gl_trans_mst where vch_sys_no= convert(numeric, case '" + vchno +"' when ''  then null else '" + vchno +"' end)";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "VoucherMst");
            if (dt.Rows.Count == 0)
            {
                return null;
            }
            return new VouchMst(dt.Rows[0]);
        }
        public static VouchMst GetVouchMstByRefsl(string sl)
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = "select convert(varchar, vch_sys_no) vch_sys_no,fin_mon,convert(varchar,value_date,103)value_date,vch_ref_no," +
            " ref_file_no,volume_no,serial_no,vch_code,trans_type,particulars,convert(varchar,control_amt) control_amt,book_name,payee,check_no,convert(varchar,cheq_date,103)cheq_date," +
            " convert(varchar,cheq_amnt) cheq_amnt,money_rpt_no,convert(varchar,money_rpt_date,103) money_rpt_date,entry_user,convert(varchar,entry_date,103) entry_date,update_user,convert(varchar,update_date,103) update_date, " +
                           " status,autho_user,convert(varchar,autho_date,103) autho_date,autho_user_type from gl_trans_mst where serial_no='" + sl + "'";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "VoucherMst");
            if (dt.Rows.Count == 0)
            {
                return null;
            }
            return new VouchMst(dt.Rows[0]);
        }
        public static VouchMst GetVouchMstByVolSl(string vol,string sl)
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = "select convert(varchar, vch_sys_no) vch_sys_no,fin_mon,convert(varchar,value_date,103)value_date,vch_ref_no," +
            " ref_file_no,volume_no,serial_no,vch_code,trans_type,particulars,convert(varchar,control_amt) control_amt,book_name,payee,check_no,convert(varchar,cheq_date,103)cheq_date," +
            " convert(varchar,cheq_amnt) cheq_amnt,money_rpt_no,convert(varchar,money_rpt_date,103) money_rpt_date,entry_user,convert(varchar,entry_date,103) entry_date,update_user,convert(varchar,update_date,103) update_date, " +
                           " status,autho_user,convert(varchar,autho_date,103) autho_date,autho_user_type from gl_trans_mst where volume_no='"+vol+"' and serial_no='" + sl + "'";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "VoucherMst");
            if (dt.Rows.Count == 0)
            {
                return null;
            }
            return new VouchMst(dt.Rows[0]);
        }
        public static VouchMst GetVouchMstByRefNo(string sl)
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = "select convert(varchar, vch_sys_no) vch_sys_no,fin_mon,convert(varchar,value_date,103)value_date,vch_ref_no," +
            " ref_file_no,volume_no,serial_no,vch_code,trans_type,particulars,convert(varchar,control_amt) control_amt,book_name,payee,check_no,convert(varchar,cheq_date,103)cheq_date," +
            " convert(varchar,cheq_amnt) cheq_amnt,money_rpt_no,convert(varchar,money_rpt_date,103) money_rpt_date,entry_user,convert(varchar,entry_date,103) entry_date,update_user,convert(varchar,update_date,103) update_date, " +
                           " status,autho_user,convert(varchar,autho_date,103) autho_date,autho_user_type from gl_trans_mst where ref_file_no='" + sl + "'";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "VoucherMst");
            if (dt.Rows.Count == 0)
            {
                return null;
            }
            return new VouchMst(dt.Rows[0]);
        }
        public static VouchMst GetVouchMstF(string vchno, string reffile, string vol)
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = "select convert(varchar, vch_sys_no) vch_sys_no,fin_mon,convert(varchar,value_date,103)value_date,vch_ref_no," +
            " ref_file_no,volume_no,serial_no,vch_code,trans_type,particulars,convert(varchar,control_amt) control_amt,book_name,payee,check_no,convert(varchar,cheq_date,103)cheq_date," +
            " convert(varchar,cheq_amnt) cheq_amnt,money_rpt_no,convert(varchar,money_rpt_date,103) money_rpt_date,entry_user,convert(varchar,entry_date,103) entry_date,update_user,convert(varchar,update_date,103) update_date, " +
                           " status,autho_user,convert(varchar,autho_date,103) autho_date,autho_user_type from gl_trans_mst where vch_sys_no= convert(numeric, case '" + vchno +"' when ''  then null else '" + vchno +"' end) or (upper(ref_file_no)=upper('" + reffile + "') and upper(volume_no) like upper('%" + vol + "%'))";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "VoucherMst");
            if (dt.Rows.Count == 0)
            {
                return null;
            }
            return new VouchMst(dt.Rows[0]);
        }
        public static DataTable GetVouchType()
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = "select vch_code,vch_desc from gl_voucher_type order by vch_code";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "VoucherType");
            return dt;
        }
        public static DataTable GetVouchMstFind(string vchno, string reffile, string vol)
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = "select convert(varchar, vch_sys_no) vch_sys_no,fin_mon,convert(varchar,value_date,103)value_date,vch_ref_no," +
            " ref_file_no,volume_no,serial_no,vch_code,trans_type,particulars,convert(varchar,control_amt) control_amt,book_name,payee,check_no,convert(varchar,cheq_date,103)cheq_date," +
            " convert(varchar,cheq_amnt) cheq_amnt,money_rpt_no,convert(varchar,money_rpt_date,103) money_rpt_date,entry_user,convert(varchar,entry_date,103) entry_date,update_user,convert(varchar,update_date,103) update_date, " +
                           " status,autho_user,convert(varchar,autho_date,103) autho_date,autho_user_type,VCH_REF_NO from gl_trans_mst where vch_sys_no= convert(numeric, case '" + vchno + "' when ''  then null else '" + vchno + "' end) and (upper(ref_file_no) like upper('%" + reffile + "%') and upper(volume_no) like upper('%" + vol + "%')) order by value_date desc";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "VoucherMst");
            return dt;
        }
        public static DataTable GetVouchMstByDate(string date,string VoucherType)
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = "select convert(varchar, vch_sys_no) vch_sys_no,fin_mon,convert(varchar,value_date,103)value_date,vch_ref_no," +
            " ref_file_no,volume_no,serial_no,vch_code,trans_type,particulars,convert(varchar,control_amt) control_amt,book_name,payee,check_no,convert(varchar,cheq_date,103)cheq_date," +
            " convert(varchar,cheq_amnt) cheq_amnt,money_rpt_no,convert(varchar,money_rpt_date,103) money_rpt_date,entry_user,convert(varchar,entry_date,103) entry_date,update_user,convert(varchar,update_date,103) update_date, " +
                           " status,autho_user,convert(varchar,autho_date,103) autho_date,autho_user_type from gl_trans_mst where convert(varchar,value_date,103)='" + date + "' and substring(VCH_REF_NO,1,2)='" + VoucherType + "' ";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "VoucherMst");
            return dt;
        }
        public static VouchMst GetVouchMaster(string vchno, string usr)
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = "select convert(varchar, vch_sys_no) vch_sys_no,fin_mon,convert(varchar,value_date,103)value_date,vch_ref_no," +
            " ref_file_no,volume_no,serial_no,vch_code,trans_type,particulars,convert(varchar,control_amt) control_amt,book_name,payee,check_no,convert(varchar,cheq_date,103)cheq_date," +
            " convert(varchar,cheq_amnt) cheq_amnt,money_rpt_no,convert(varchar,money_rpt_date,103) money_rpt_date,case when status='A' and convert(numeric,autho_user_type)>=convert(numeric, case '" + usr + "' when '' then null else '" + usr + "' end) then 'A' " +
                           " else 'U' end status,entry_user,convert(varchar,entry_date,103) entry_date,update_user,convert(varchar,update_date,103) update_date, " +
                           " status,autho_user,convert(varchar,autho_date,103) autho_date,autho_user_type from gl_trans_mst where vch_sys_no= convert(numeric, case '" + vchno +"' when ''  then null else '" + vchno +"' end)";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "VoucherMst");
            if (dt.Rows.Count == 0)
            {
                return null;
            }
            return new VouchMst(dt.Rows[0]);
        }
        public static DataTable GetVouchMsts(string criteria)
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = "select top 100 convert(varchar, vch_sys_no) vch_sys_no,fin_mon,convert(varchar,value_date,103)value_date,vch_ref_no," +
            " ref_file_no,volume_no,serial_no,vch_code,trans_type,particulars,convert(varchar,control_amt) control_amt,book_name,payee,check_no,convert(varchar,cheq_date,103)cheq_date," +
            " convert(varchar,cheq_amnt) cheq_amnt,money_rpt_no,convert(varchar,money_rpt_date,103) money_rpt_date,entry_user,convert(varchar,entry_date,103) entry_date,update_user,convert(varchar,update_date,103) update_date, " +
                           " status,autho_user,convert(varchar,autho_date,103) autho_date,autho_user_type from gl_trans_mst order by value_date desc ";
            if (criteria != "")
            {
                query = query + " where " + criteria;
            }
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "VoucherMst");
            return dt;            
        }
        public static DataTable GetVouchMasters(string usr)
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = " select top 1000 convert(varchar,vch_sys_no) vch_sys_no,convert(varchar,value_date,103) value_date,particulars,convert(varchar,control_amt) control_amt,case when status='A' and autho_user_type>="+usr+" then 'A' "+
                           " else 'U' end status from gl_trans_mst where isnull(autho_user_type,1) between " + usr + "-1 and " + usr + "+1 order by convert(datetime,value_date,103) desc ";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "VoucherMst");
            return dt;            
        }
        
        public static DataTable GetVouchMstAuth(string criteria)
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = "select convert(varchar,vch_sys_no) vch_sys_no,convert(varchar,value_date,103) value_date,particulars,convert(varchar,control_amt) control_amt from gl_trans_mst";
            if (criteria != "")
            {
                query = query + " where [STATUS]='U' AND " + criteria;
            }
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "VoucherMst");
            return dt;

        }
        public static DataTable GetVouch()
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = "select top 100 convert(varchar, vch_sys_no) vch_sys_no,fin_mon,convert(varchar,value_date,103)value_date,vch_ref_no," +
            " ref_file_no,volume_no,serial_no,vch_code,trans_type,particulars,convert(varchar,control_amt) control_amt,book_name,payee,check_no,convert(varchar,cheq_date,103)cheq_date," +
            " convert(varchar,cheq_amnt) cheq_amnt,money_rpt_no,convert(varchar,money_rpt_date,103) money_rpt_date,entry_user,convert(varchar,entry_date,103) entry_date,update_user,convert(varchar,update_date,103) update_date, " +
                           " status,autho_user,convert(varchar,autho_date,103) autho_date,autho_user_type from gl_trans_mst order by value_date desc ";
            
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "VoucherMst");
            return dt;

        }

        public static void DeleteVouchDtl(string vouchid)
        {
            String connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = "delete from gl_trans_dtl where vch_sys_no=convert(numeric,'" + vouchid+"')";
            DataManager.ExecuteNonQuery(connectionString, query);
        }

        public static void DeleteVouchDtlRecord(string vouchid, string glcode)
        {
            String connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = "delete from gl_trans_dtl where vch_sys_no=convert(numeric,'" + vouchid + "') and gl_coa_code='" +glcode+"'";
            DataManager.ExecuteNonQuery(connectionString, query);
        }
        public static void CreateVouchDtl(VouchDtl vouchdtl)
        {
            String connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = " insert into gl_trans_dtl(vch_sys_no,line_no,gl_coa_code,value_date,particulars, " +
                          " acc_type,amount_dr,amount_cr,status,book_name) values (convert(numeric,'" + vouchdtl.VchSysNo + "'), " +
                          "  '" + vouchdtl.LineNo + "',  '" + vouchdtl.GlCoaCode + "', convert(datetime,nullif('" + vouchdtl.ValueDate + "',''),103), " +
                          "  '" + vouchdtl.Particulars + "', '" + vouchdtl.AccType + "', convert(decimal(13,2),nullif('" + vouchdtl.AmountDr.Replace(",", "") + "','')), " +
                          "  convert(decimal(13,2),nullif('" + vouchdtl.AmountCr.Replace(",", "") + "','')), '" + vouchdtl.Status + "',  '" + vouchdtl.BookName + "')";
            DataManager.ExecuteNonQuery(connectionString, query);
        }

        public static void UpdateVouchDtl(VouchDtl vouchdtl)
        {
            String connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query =" update gl_trans_dtl set line_no='" + vouchdtl.LineNo + "',gl_coa_code='" + vouchdtl.GlCoaCode + "', " +
                          " value_date=convert(datetime,case '" + vouchdtl.ValueDate + "' when '' then null else '" + vouchdtl.ValueDate + "' end ,103),particulars='" + vouchdtl.Particulars + "', " +
                          " acc_type='" + vouchdtl.AccType + "',amount_dr=" + vouchdtl.AmountDr.Replace(",", "") + ", " +
                          " amount_cr=" + vouchdtl.AmountCr.Replace(",", "") + ",book_name='" + vouchdtl.BookName + "' where " +
                          " vch_sys_no= convert(numeric, case '" + vouchdtl.VchSysNo+"' when ''  then null else '" + vouchdtl.VchSysNo+"' end)";
            DataManager.ExecuteNonQuery(connectionString, query);
            
        }

        public static DataTable GetVouchDtl(string vouchno)
        {
            String connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = "select convert(varchar,vch_sys_no) vch_sys_no,line_no,gl_coa_code,convert(varchar,value_date,103) value_date, "+
                " particulars,acc_type,convert(varchar,amount_dr) amount_dr,convert(varchar,amount_cr) amount_cr,status,book_name from gl_trans_dtl where vch_sys_no=convert(numeric,'" + vouchno + "') order by convert(numeric,line_no)" ;
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "VoucherDtl");
            return dt;
        }
        public static DataTable GetVouchDtlRpt(string vouchno)
        {
            String connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = "select gl_coa_code,particulars,convert(varchar,amount_dr) amount_dr,convert(varchar,amount_cr) amount_cr from gl_trans_dtl where vch_sys_no=convert(numeric,'" + vouchno + "') order by convert(numeric,line_no)";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "VoucherDtl");
            return dt;
        }
        public static DataTable GetVouchDtlForTotal(string ed,string status,string book)
        {
            String connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = @"SELECT convert(date,ta.VALUE_DATE,103) VALUE_DATE,ta.GL_COA_CODE,ta.ACC_TYPE,ta.STATUS,ta.amount_dr,ta.amount_cr FROM ( select b.value_date,a.gl_coa_code,a.acc_type,b.status,isnull(amount_dr,0) amount_dr,isnull(amount_cr,0) amount_cr from gl_trans_dtl a, gl_trans_mst b where a.vch_sys_no=b.vch_sys_no and " +
                " a.book_name=b.book_name and convert(datetime,b.value_date,103) <= convert(datetime,'" + ed + "',103) " +
                " and b.book_name='" + book + "' union all SELECT '01/01/2012',t1.GL_COA_CODE,t1.ACC_TYPE,'A',case when t1.opening_balance  >= 0 then t1.opening_balance else 0 end , case when t1.opening_balance  <= 0 then t1.opening_balance else 0 end FROM GL_COA T1 where t1.opening_balance !=0 and t1.status='" + status + "' and t1.book_name='" + book + "') Ta   order by ta.VALUE_DATE,ta.GL_COA_CODE";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "VoucherDtl");
            return dt;
        }
        public static VouchDtl GetVouchDtlRecord(string vchno,string glcode)
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = "select convert(varchar,vch_sys_no) vch_sys_no,line_no,gl_coa_code,convert(varchar,value_date,103) value_date, " +
                " particulars,acc_type,convert(varchar,amount_dr) amount_dr,convert(varchar,amount_cr) amount_cr,status,book_name from gl_trans_dtl where vch_sys_no= convert(numeric, case '" + vchno +"' when ''  then null else '" + vchno +"' end) and gl_coa_code='" + glcode + "'";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "VoucherDtl");
            if (dt.Rows.Count == 0)
            {
                return null;
            }
            return new VouchDtl(dt.Rows[0]);
        }
        public static decimal getRemainDebitValue(string vchno)
        {
            decimal val;
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = "select isnull(sum(amount_dr),0) from gl_trans_dtl where vch_sys_no=convert(numeric, case '" + vchno+"' when ''  then null else '" + vchno+"' end)";
            sqlCon.Open();
            SqlCommand myCommand = new SqlCommand(query, sqlCon);
            object maxValue = myCommand.ExecuteScalar();
            sqlCon.Close();
            if (maxValue == DBNull.Value) return 1;
            val = decimal.Parse( maxValue.ToString());
            return val ;
        }
        public static decimal getRemainCreditValue(string vchno)
        {
            decimal val;
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = "select isnull(sum(amount_cr),0) from gl_trans_dtl where vch_sys_no=convert(numeric, case '" + vchno+"' when ''  then null else '" + vchno+"' end)";
            sqlCon.Open();
            SqlCommand myCommand = new SqlCommand(query, sqlCon);
            object maxValue = myCommand.ExecuteScalar();
            sqlCon.Close();
            if (maxValue == DBNull.Value) return 1;
            val = decimal.Parse(maxValue.ToString());
            return val;
        }

        public static decimal getBal(string glcode,string sd, string ed,string costc,string book)
        {
            decimal val;
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = "select gl_balc('"+book+"','"+glcode+"','"+sd+"','"+ed+"','A','"+costc+"') ";
            sqlCon.Open();
            SqlCommand myCommand = new SqlCommand(query, sqlCon);
            object maxValue = myCommand.ExecuteScalar();
            sqlCon.Close();
            if (maxValue == DBNull.Value) return 0;
            val = decimal.Parse(maxValue.ToString());
            return val;
        }

        public static decimal getBudAmnt(string glcode,string vdate)
        {
            decimal val;
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = "select sum(bud_amnt+bud_tol_amnt) bud_amnt from sgl_budget_amnt where gl_coa_code='"+glcode+"' "+
            " and bud_sys_id in (select bud_sys_id from sgl_budget where convert(datetime,'" + vdate + "',103) between fin_start_dt and fin_end_dt) ";
            sqlCon.Open();
            SqlCommand myCommand = new SqlCommand(query, sqlCon);
            object maxValue = myCommand.ExecuteScalar();
            sqlCon.Close();
            if (maxValue == DBNull.Value) return 0;
            val = decimal.Parse(maxValue.ToString());
            return val;
        }
        public static string getBudStatus(string glcode, string vdate)
        {
            string val;
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = "select max('Y') bud_en from sgl_budget_amnt where gl_coa_code='" + glcode + "' " +
            " and bud_sys_id in (select bud_sys_id from sgl_budget where convert(datetime,'" + vdate + "',103) between fin_start_dt and fin_end_dt) ";
            sqlCon.Open();
            SqlCommand myCommand = new SqlCommand(query, sqlCon);
            object maxValue = myCommand.ExecuteScalar();
            sqlCon.Close();
            if (maxValue == DBNull.Value) return "N";
            val = maxValue.ToString();
            return val;
        }
        public static string getBudYn(string finyr)
        {
            string val;
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = "select max('Y') bud_en from sgl_budget where fin_year= '"+finyr+"'";
            sqlCon.Open();
            SqlCommand myCommand = new SqlCommand(query, sqlCon);
            object maxValue = myCommand.ExecuteScalar();
            sqlCon.Close();
            if (maxValue == DBNull.Value) return "N";
            val = maxValue.ToString();
            return val;
        }

        public static string getAccType(string glcode)
        {
            string val;
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = "select max(acc_type) acc_type from gl_coa where gl_coa_code= '" + glcode + "'";
            sqlCon.Open();
            SqlCommand myCommand = new SqlCommand(query, sqlCon);
            object maxValue = myCommand.ExecuteScalar();
            sqlCon.Close();
            if (maxValue == DBNull.Value) return "";
            val = maxValue.ToString();
            return val;
        }

        public static DataTable getDayBook(string sd, string ed, string vchcode, string book)
        {
            //String connectionString = DataManager.OraConnString();
            //SqlConnection sqlCon = new SqlConnection(connectionString);
            //string query = "select a.vch_sys_no,vch_ref_no,ltrim('File#'+ref_file_no+' Vol#'+volume_no+' sl#'+serial_no) vch_manual_no,convert(varchar,b.value_date,103) value_date,substring(replace(b.particulars,'  ',' '),1,100) particulars,amount_dr,amount_cr " +
            //" from gl_trans_dtl a, gl_trans_mst b where b.book_name='" + book + "' and a.vch_sys_no=b.vch_sys_no and b.vch_code='" + vchcode + "'  " +
            //" and b.value_date between convert(datetime,'" + sd + "',103) and convert(datetime,'" + ed + "',103) and a.gl_coa_code in (select gl_coa_code from gl_coa where acc_type in('" + vchcode + "','01','E','02','I'))";
            //DataTable dt = DataManager.ExecuteQuery(connectionString, query, "VoucherDtl");//substring was wrong and decode not work this replace withb in()
            //return dt;

            //String connectionString = DataManager.OraConnString();
            //SqlConnection sqlCon = new SqlConnection(connectionString);
            //string query = "select a.vch_sys_no,vch_ref_no,ltrim('File#'+ref_file_no+' Vol#'+volume_no+' sl#'+serial_no) vch_manual_no,convert(varchar,b.value_date,103) value_date,substring(replace(a.particulars,'  ',' '),1,100) particulars,case when b.VCH_CODE='01' then 'Debit Voucher' when b.VCH_CODE='02' then 'Cradit Voucher' when b.VCH_CODE='03' then 'Journal Voucher' else 'Contra Voucher' end as VCH_CODE,amount_dr,amount_cr " +
            //" from gl_trans_dtl a, gl_trans_mst b where b.book_name='" + book + "' and a.vch_sys_no=b.vch_sys_no  " +
            //" and b.value_date between convert(datetime,'" + sd + "',103) and convert(datetime,'" + ed + "',103) and a.gl_coa_code in (select gl_coa_code from gl_coa where acc_type in('" + vchcode + "','01','E','02','I'))";//substring was wrong and decode not work this replace withb in()
            //DataTable dt = DataManager.ExecuteQuery(connectionString, query, "VoucherDtl");
            //return dt;
            String connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = "select a.vch_sys_no,vch_ref_no,convert(varchar,b.value_date,103) value_date,ltrim('File#'+ref_file_no+' Vol#'+volume_no+' sl#'+serial_no) Ref#,b.PARTICULARS Descriptions,substring(replace(+a.GL_COA_CODE+' - '+a.particulars,'  ',' '),1,100) particulars,case when b.VCH_CODE='01' then 'Debit Voucher' when b.VCH_CODE='02' then 'Cradit Voucher' when b.VCH_CODE='03' then 'Journal Voucher' else 'Contra Voucher' end as VCH_CODE,amount_dr,amount_cr " +
            " from gl_trans_dtl a, gl_trans_mst b where b.book_name='" + book + "' and a.vch_sys_no=b.vch_sys_no and b.VCH_CODE LIKE '%" + vchcode + "%' " +
            " and b.value_date between convert(datetime,'" + sd + "',103) and convert(datetime,'" + ed + "',103)";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "VoucherDtl");
            return dt;
        }
        public static DataTable getDayIncomeBook(string sd, string ed, string book)
        {
            //String connectionString = DataManager.OraConnString();
            //SqlConnection sqlCon = new SqlConnection(connectionString);
            //string query = "select particulars,convert(varchar,sum(amount_cr)) amount_cr from gl_trans_dtl where substring(gl_coa_code,3) like '7%' and book_name='" + book + "' and" +
            //" value_date between convert(datetime,'" + sd + "',103) and convert(datetime,'" + ed + "',103) group by particulars ";
            //DataTable dt = DataManager.ExecuteQuery(connectionString, query, "VoucherDtl");
            //return dt;

            String connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = "select particulars,isnull(sum(amount_cr),0) amount_cr  from gl_trans_dtl where SUBSTRING(GL_COA_CODE,3,5) like'6%' and book_name='" + book + "' and" +
            " value_date between convert(datetime,'" + sd + "',103) and convert(datetime,'" + ed + "',103) group by particulars ";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "VoucherDtl");
            return dt;
        }

        public static DataTable getCoa()
        {
            
            String connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = "select c.GL_COA_CODE,c.COA_DESC from GL_COA c";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "VoucherDtl");
            return dt;
        }

        public static string GetshowBankGlCoaCode(string p)
        {
            SqlConnection connection = new SqlConnection(DataManager.OraConnString());
            connection.Open();
            string SelectQuery = @"SELECT [gl_coa_code]  FROM [bank_info] where [bank_id]='"+p+"'";
            SqlCommand command = new SqlCommand(SelectQuery,connection);
            return command.ExecuteScalar().ToString();
        }

        public static DataTable GetshowGlCoa(string p)
        {
            String connectionString = DataManager.OraConnString(); 
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = @"SELECT GL_COA_CODE ,COA_DESC FROM GL_COA where COA_DESC='" + p + "'"; 
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "VoucherDtl"); 
            return dt;
        }

        public static object GetShowTotalMonthOnShow()
        {
            String connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = @"SELECT [FIN_MON]  FROM [GL_FIN_MONTH] where [YEAR_FLAG] ='O'";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "VoucherDtl");
            return dt;
        }

        public static DataTable GetVouchMastersOnDevidVoucher(string DebitVoucher)
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = " select top 1000 convert(varchar,vch_sys_no) vch_sys_no,convert(varchar,value_date,103) value_date,particulars,convert(varchar,control_amt) control_amt,case when status='A' and VCH_CODE='01' and autho_user_type>=" + DebitVoucher + " then 'A' " +
                           " else 'U' end status,VCH_REF_NO from gl_trans_mst where VCH_CODE='01' and isnull(autho_user_type,1) between " + DebitVoucher + "-1 and " + DebitVoucher + "+1 order by convert(datetime,value_date,103) desc ";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "VoucherMst");
            return dt;  
        }

        public static DataTable GetVouchMastersContraVoucher(string CONV)
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = " select top 1000 convert(varchar,vch_sys_no) vch_sys_no,convert(varchar,value_date,103) value_date,particulars,convert(varchar,control_amt) control_amt,case when status='A' and VCH_CODE='01' and autho_user_type>=" + CONV + " then 'A' " +
                           " else 'U' end status from gl_trans_mst where VCH_CODE='04' and isnull(autho_user_type,1) between " + CONV + "-1 and " + CONV + "+1 order by convert(datetime,value_date,103) desc ";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "VoucherMst");
            return dt;
        }

        public static DataTable GetVouchMastersCreditVoucher(string CRV)
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = " select top 1000 convert(varchar,vch_sys_no) vch_sys_no,convert(varchar,value_date,103) value_date,particulars,convert(varchar,control_amt) control_amt,case when status='A' and VCH_CODE='01' and autho_user_type>=" + CRV + " then 'A' " +
                           " else 'U' end status,VCH_REF_NO from gl_trans_mst where VCH_CODE='02' and isnull(autho_user_type,1) between " + CRV + "-1 and " + CRV + "+1 order by convert(datetime,value_date,103) desc ";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "VoucherMst");
            return dt;
        }

        public static DataTable GetVouchMastersJurnalVoucher(string JRNV)
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = " select top 1000 convert(varchar,vch_sys_no) vch_sys_no,convert(varchar,value_date,103) value_date,particulars,convert(varchar,control_amt) control_amt,case when status='A' and VCH_CODE='01' and autho_user_type>=" + JRNV + " then 'A' " +
                           " else 'U' end status ,VCH_REF_NO from gl_trans_mst where VCH_CODE='03' and isnull(autho_user_type,1) between " + JRNV + "-1 and " + JRNV + "+1 order by convert(datetime,value_date,103) desc ";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "VoucherMst");
            return dt;
        }
        public static int GetshowCoaCheck(string p)
        {
            if (p == "")
            {
                return 0;
            }
            else
            {
                SqlConnection connection = new SqlConnection(DataManager.OraConnString());
                //connection.Open();
                string selectQuery = @"SELECT COUNT(*)  FROM [GL_TRANS_MST] where [CHECK_NO]='" + p + "'";
                SqlCommand command = new SqlCommand(selectQuery, connection);
                connection.Open();
                object maxValue = command.ExecuteScalar();
                connection.Close();
                return Convert.ToInt32(maxValue);
            }

            
        }

        public static DataTable GetVouchMstByVoucherNo(string VoucherNo)
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = "select convert(varchar, vch_sys_no) vch_sys_no,fin_mon,convert(varchar,value_date,103)value_date,vch_ref_no," +
            " ref_file_no,volume_no,serial_no,vch_code,trans_type,particulars,convert(varchar,control_amt) control_amt,book_name,payee,check_no,convert(varchar,cheq_date,103)cheq_date," +
            " convert(varchar,cheq_amnt) cheq_amnt,money_rpt_no,convert(varchar,money_rpt_date,103) money_rpt_date,entry_user,convert(varchar,entry_date,103) entry_date,update_user,convert(varchar,update_date,103) update_date, " +
                           " status,autho_user,convert(varchar,autho_date,103) autho_date,autho_user_type from gl_trans_mst where vch_sys_no='" + VoucherNo + "'";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "VoucherMst");
            return dt;
        }

        public static VouchMst GetVouchMstByRefslDrCrAndJV(string sl, string VoucherType)
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = "select convert(varchar, vch_sys_no) vch_sys_no,fin_mon,convert(varchar,value_date,103)value_date,vch_ref_no," +
            " ref_file_no,volume_no,serial_no,vch_code,trans_type,particulars,convert(varchar,control_amt) control_amt,book_name,payee,check_no,convert(varchar,cheq_date,103)cheq_date," +
            " convert(varchar,cheq_amnt) cheq_amnt,money_rpt_no,convert(varchar,money_rpt_date,103) money_rpt_date,entry_user,convert(varchar,entry_date,103) entry_date,update_user,convert(varchar,update_date,103) update_date, " +
                           " status,autho_user,convert(varchar,autho_date,103) autho_date,autho_user_type from gl_trans_mst where serial_no='" + sl + "' and SUBSTRING(vch_ref_no,0,3)='" + VoucherType + "' ";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "VoucherMst");
            if (dt.Rows.Count == 0)
            {
                return null;
            }
            return new VouchMst(dt.Rows[0]);
        }

        public static DataTable GetShowAllVoucherWithParameter(string p)
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string Parameter = "";
            if (p == "CV")
            { Parameter = "AND PAYEE!='ST_Pay'"; }
            string query = @"select top(300) convert(varchar,vch_sys_no) vch_sys_no,convert(varchar,value_date,103) value_date,particulars,convert(varchar,control_amt) control_amt,status,VCH_REF_NO from gl_trans_mst where substring(VCH_REF_NO,1,2)='" + p + "' "+Parameter+" order by convert(int,vch_sys_no) desc ";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "VoucherMst");
            return dt;
        }
    }
}