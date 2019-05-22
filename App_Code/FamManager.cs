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
/// Summary description for FamManager
/// </summary>
/// 
namespace KHSC
{
    public class FamManager
    {
        public static void CreateFam(Fam fam)
        {
            String connectionString = DataManager.OraConnString();
            string query = " insert into family_dtl (sfml_student_id,rel_name,relation,birth_dt,age,occupation) values (" +
                "  '" + fam.StudentId + "', '" + fam.RelName + "', '" + fam.Relation + "','" + fam.BirthDt + "', " +
             "'" + fam.Age + "', '" + fam.Occupation + "')";
            DataManager.ExecuteNonQuery(connectionString, query);
        }
        public static void UpdateFam(Fam fam)
        {
            String connectionString = DataManager.OraConnString();
            string query = " update family_dtl set rel_name= '" + fam.RelName + "',relation= '" + fam.Relation + "',birth_dt= convert(datetime,nullif('" + fam.BirthDt + "',''),103), " +
             " age= convert(numeric,nullif('" + fam.Age + "','')),occupation= '" + fam.Occupation + "' where sfml_student_id='" + fam.StudentId + "' and rtrim(rel_name)=rtrim('" + fam.RelName + "')";
            DataManager.ExecuteNonQuery(connectionString, query);
        }
        public static void DeleteFam(string emp)
        {
            String connectionString = DataManager.OraConnString();
            string query = " delete from family_dtl where sfml_student_id='" + emp + "' ";
            DataManager.ExecuteNonQuery(connectionString, query);
        }
        public static Fam getFam(string stdid, string rel)
        {
            String connectionString = DataManager.OraConnString();
            string query = "select sfml_student_id, REL_NAME, RELATION, convert(varchar,BIRTH_DT,103)birth_dt, convert(varchar,AGE)age, OCCUPATION from family_dtl where sfml_student_id='" + stdid + "' and rtrim(rel_name)=rtrim('" + rel + "') ";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Family");
            if (dt.Rows.Count == 0)
            {
                return null;
            }
            return new Fam(dt.Rows[0]);
        }
        public static DataTable getFams(string stdid)
        {
            String connectionString = DataManager.OraConnString();
            string query = "select sfml_student_id, REL_NAME, RELATION, convert(varchar,BIRTH_DT,103)birth_dt, convert(varchar,AGE)age, OCCUPATION from family_dtl where sfml_student_id='" + stdid + "' ";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Family");
            return dt;
        }
        public static DataTable getFamily(string stdid)
        {
            String connectionString = DataManager.OraConnString();
            string query = "select sfml_student_id, initcap(REL_NAME)rel_name, RELATION, convert(varchar,BIRTH_DT,103)birth_dt, convert(varchar,AGE)age, dbo.initcap(OCCUPATION)occupation from family_dtl where sfml_student_id='" + stdid + "' ";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Family");
            return dt;
        }
        public static DataTable getFamilyRpt(string criteria)
        {
            String connectionString = DataManager.OraConnString();
            string query = "select sfml_student_id, dbo.initcap(REL_NAME)rel_name, RELATION, convert(varchar,BIRTH_DT,103)birth_dt, convert(varchar,AGE)age, dbo.initcap(OCCUPATION)occupation from family_dtl ";
            if (criteria.Length > 0)
            {
                query += " where " + criteria;
            }
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Family");
            return dt;
        }
        public static DataTable getFamRpt(string stdid)
        {
            String connectionString = DataManager.OraConnString();
            string query = "select dbo.initcap(REL_NAME)rel_name, RELATION, convert(varchar,BIRTH_DT,103)birth_dt, convert(varchar,AGE)age, dbo.initcap(OCCUPATION)occupation from family_dtl where sfml_student_id='" + stdid + "' ";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Family");
            return dt;
        }

        public static DataTable GetStdFamInformationForSpecificStudent(string studentId)
        {
            String connectionString = DataManager.OraConnString();
            string query = "select dbo.initcap(REL_NAME)rel_name, RELATION, convert(varchar,BIRTH_DT,103)birth_dt, convert(varchar,AGE)age, dbo.initcap(OCCUPATION)occupation from family_dtl where sfml_student_id='" + studentId + "'";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Family");
            return dt;
        }
    }
}