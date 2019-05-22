using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Xml.Linq;
using System.Data.SqlClient;
using KHSC;

/// <summary>
/// Summary description for PermisManager
/// </summary>
/// 
namespace KHSC
{
    public class PermisManager
    {
        public static void CreatePermis(Permis per)
        {
            String connectionString = DataManager.OraConnString();
            string query = " insert into utl_usergrant (user_name,mod_id,allow_add,allow_edit,allow_view, " +
                   " allow_delete,allow_print,allow_autho) values ( '" + per.UserName + "', "+ 
                   " '" + per.ModId + "', '" + per.AllowAdd + "',  '" + per.AllowEdit + "', "+
                   " '" + per.AllowView + "', '" + per.AllowDelete + "',  '" + per.AllowPrint + "', '" + per.AllowAutho + "')";

            DataManager.ExecuteNonQuery(connectionString, query);
        }
        public static void UpdatePermis(Permis per)
        {
            String connectionString = DataManager.OraConnString();
            string query = " update utl_usergrant set allow_add= '" + per.AllowAdd + "',  allow_edit= '" + per.AllowEdit + "', " +
                   " allow_view= '" + per.AllowView + "', allow_delete= '" + per.AllowDelete + "',  allow_print= '" + per.AllowPrint + "', "+
                   " allow_autho= '" + per.AllowAutho + "' where upper(user_name)='"+per.UserName.ToUpper()+"' and mod_id='"+per.ModId+"'";

            DataManager.ExecuteNonQuery(connectionString, query);
        }
        public static DataTable GetPermiss(string user)
        {
            string connectionString = DataManager.OraConnString();
            string query = "select distinct a.user_name,a.mod_id,b.description mod_name,a.allow_add,a.allow_edit,a.allow_view, " +
            " a.allow_delete,a.allow_print,a.allow_autho from utl_usergrant a, utl_modules b where a.mod_id=b.mod_id " +
            " and a.user_name='" + user + "' order by a.mod_id";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Permiss");
            return dt;
        }
        public static Permis getPermis(string user, string modid)
        {
            String connectionString = DataManager.OraConnString();
            string query = "select * from utl_usergrant where upper(user_name)='" + user.ToUpper() + "' and mod_id='"+modid+"'";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Permis");
            if (dt.Rows.Count == 0)
            {
                return null;
            }
            return new Permis(dt.Rows[0]);
        }
        public static DataTable getModules()
        {
            string connectionString = DataManager.OraConnString();
            string query = "select * from utl_modules";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Modules");
            return dt;
        }
        public static DataTable getModulesGrid()
        {
            string connectionString = DataManager.OraConnString();
            string query = "select mod_id,description mod_desc from utl_modules where need_auth='Y'";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Modules");
            return dt;
        }
        public static DataTable getModulesUser(string user)
        {
            string connectionString = DataManager.OraConnString();
            string query = "select * from utl_modules where mod_id not in (select mod_id from utl_usergrant where upper(user_name)=upper('"+user+"')) order by mod_id";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Modules");
            return dt;
        }
        public static Permis getUsrPermis(string user, string modname)
        {
            String connectionString = DataManager.OraConnString();
            string query = "select distinct user_name,a.mod_id,a.allow_add,a.allow_edit,a.allow_view,a.allow_delete,a.allow_print,a.allow_autho "+
            " from utl_usergrant a, utl_modules b where a.mod_id=b.mod_id and upper(a.user_name)=upper('" + user + "') and a.mod_id='" + modname + "'";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Permis");
            if (dt.Rows.Count == 0)
            {
                return null;
            }
            return new Permis(dt.Rows[0]);
        }
        public static string getModuleId(string modname)
        {
            string modName = "";
            string connectionString = DataManager.OraConnString();
            SqlDataReader dReader;
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = connectionString;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "Select mod_id from utl_modules where rtrim(upper(mod_name))=rtrim(upper('" +modname + "'))";
            conn.Open();
            try
            {
                dReader = cmd.ExecuteReader();
                if (dReader.HasRows == true)
                {
                    while (dReader.Read())
                        modName = dReader["mod_id"].ToString();
                }
            }
            finally
            {
                conn.Close();

            }
            return modName;
        }
    }
}