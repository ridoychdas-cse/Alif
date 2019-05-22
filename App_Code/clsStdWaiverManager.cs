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
/// Summary description for clsStdWaiverManager
/// </summary>
/// 
namespace KHSC
{
    public class clsStdWaiverManager
    {
        public static DataTable GetStdWaivers(string criteria)
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = "select student_id,class_id, "+
                " convert(varchar,waive_year)waive_year, convert(varchar,waive_pct)waive_pct, convert(varchar,exc_from,103)exc_from,convert(varchar,exc_to,103)exc_to,convert(varchar,waive_sl)waive_sl  from std_waiver a ";
            if (criteria != "")
            {
                query = query + " where " + criteria;
            }
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Waivers");
            return dt;
        }
        public static DataTable getStdClasses()
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = "select distinct student_id,(select f_name+' '+m_name+' '+l_name from student_info where student_id=a.student_id)name,(select t1.class_name from class_info t1 where t1.class_id= a.class_id)class_id,convert(varchar,waive_year)waive_year, convert(varchar,waive_pct)waive_pct,convert(varchar,exc_from,103)exc_from,convert(varchar,exc_to,103)exc_to,convert(varchar,waive_sl)waive_sl from std_waiver a order by waive_year desc,class_id desc";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Waivers");
            return dt;
        }
        public static clsStdWaiver getStdWaiver(string sl)
        {
            String connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);

            string query = "select student_id,class_id,convert(varchar,waive_year)waive_year,convert(varchar,waive_pct)waive_pct,convert(varchar,exc_from,103)exc_from,convert(varchar,exc_to,103)exc_to,convert(varchar,waive_sl)waive_sl,remarks " +
                " from std_waiver a where convert(varchar,waive_sl)='" + sl + "' ";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Waiver");
            if (dt.Rows.Count == 0)
            {
                return null;
            }
            return new clsStdWaiver(dt.Rows[0]);
        }

        public static DataTable getStdWaiverInfo(string p,string flag)
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string Type="";
            if (flag == "0") { Type = "convert(varchar,a.waive_sl)"; } else { Type = "a.STUDENT_ID"; }
            string query = @"select dbo.initcap(t.f_name+' '+t.m_name+' '+t.l_name) name,a.student_id,a.class_id,convert(varchar,waive_year)waive_year,convert(varchar,waive_pct)waive_pct,convert(varchar,exc_from,103)exc_from
,convert(varchar,exc_to,103)exc_to,convert(varchar,waive_sl)waive_sl,remarks,b.[version],b.sect,b.shift,b.std_roll,c.class_name,sc.sec_name,sh.shift_name,vv.version_name
  from std_waiver a 
  inner join student_info t on t.student_id=a.STUDENT_ID
  inner join std_current_status b on b.student_id=a.STUDENT_ID
  inner join class_info c on c.class_id=b.class_id left join section_info sc on sc.sec_id=b.sect inner join shift_info sh on sh.shift_id=b.shift left join version_info vv on vv.id=b.[version]
  where upper(" + Type + ")=upper('" + p + "') ";

            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "std_waiver");
            return dt;
        }
        public static DataTable getStdWaivers(string std)
        {
            String connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);

            string query = "select student_id,(select f_name+' '+m_name+' '+l_name from student_info where student_id=a.student_id)name, "+
                " class_id,convert(varchar,waive_year)waive_year,convert(varchar,waive_pct)waive_pct, "+
                " convert(varchar,exc_from,103)exc_from,convert(varchar,exc_to,103)exc_to,convert(varchar,waive_sl)waive_sl from std_waiver a where student_id='" + std + "' ";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Waiver");
            
            return dt;
        }
        public static clsStdWaiver getStdWaiverClass(string std,string yr,string clsid,string versionId,string shiftId,string sectionId)
        {
            String connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            if (yr == "")
            {
                yr = "5";
            }
            string query = "select student_id,class_id,convert(varchar,waive_year)waive_year,convert(varchar,waive_pct)waive_pct, " +
                " convert(varchar,exc_from,103)exc_from,convert(varchar,exc_to,103)exc_to,convert(varchar,waive_sl)waive_sl from std_waiver a where student_id='" + std + "' and isnull(convert(varchar,waive_year),'5')=isnull('" + yr + "','5') " +
                " and class_id ='" + clsid + "' and version='" + versionId + "' and shift='" + shiftId + "' and section='" + sectionId + "' ";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Waiver");
            if (dt.Rows.Count == 0)
            {
                return null;
            }
            return new clsStdWaiver(dt.Rows[0]);
        }
        public static void CreateStdWaiver(clsStdWaiver std)
        {
            String connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);

            string query = "insert into std_waiver(student_id,class_id,waive_year,waive_pct,exc_from,exc_to,waive_sl,remarks) values ('" + std.StudentId + "', '" + std.ClassId + "','" + std.WaiveYear + "', " +
                "convert(numeric(5,2),'" + std.WaivePct + "'), convert(datetime,'" + std.ExcFrom + "',103), convert(datetime,'" + std.ExcTo + "',103), convert(numeric(18,0),'"+std.WaiveSl+"'),'"+std.Remarks+"') ";
            DataManager.ExecuteNonQuery(connectionString, query);
        }
        public static void UpdateStdWaiver(clsStdWaiver std)
        {
            String connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);

            string query = "update std_waiver set student_id='" + std.StudentId + "',class_id='" + std.ClassId + "',remarks='" + std.Remarks + "',waive_year='" + std.WaiveYear + "', " +
                " waive_pct='" + std.WaivePct + "',exc_from= convert(datetime,nullif('" + std.ExcFrom + "',''),103),exc_to= convert(datetime,nullif('" + std.ExcTo + "',''),103) " +
                " where waive_sl= '" + std.WaiveSl + "'";
            DataManager.ExecuteNonQuery(connectionString, query);
        }
        public static void DeleteStdWaiver(clsStdWaiver std)
        {
            String connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);

            string query = "delete from std_waiver where waive_sl= '" + std.WaiveSl + "' ";
            DataManager.ExecuteNonQuery(connectionString, query);
        }
        public static string getStudentWaiverPct(string std, string yr, string clsid)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            string Query = "select waive_pct from std_waiver where student_id='" + std + "' and class_id= '" + clsid + "' " +
                " and waive_year='" + yr + "'  ";
            myConnection.Open();
            SqlCommand myCommand = new SqlCommand(Query, myConnection);
            object maxValue = myCommand.ExecuteScalar();
            myConnection.Close();
            if (maxValue == null)
            {
                maxValue = "0";
            }
            return maxValue.ToString();
        }

        public static int GetCountSameMonthYear(string StudentId,string year)
        {
            String ConnectionString = DataManager.OraConnString();
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            string Query = @"SELECT COUNT(*) FROM [STD_WAIVER] where [STUDENT_ID]='" + StudentId + "' and [WAIVE_YEAR]='" + year + "'";
            myConnection.Open();
            SqlCommand myCommand = new SqlCommand(Query, myConnection);
            object maxValue = myCommand.ExecuteScalar();
            myConnection.Close();
            if (maxValue == null)
            {
                maxValue = "0";
            }
            return Convert.ToInt32(maxValue);
        }        
    }
}