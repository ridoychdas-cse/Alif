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
/// Summary description for classAssoManager
/// </summary>
/// 
namespace KHSC
{
    public class classAssoManager
    {
        public static void CreateAsso(classAsso asso)
        {
            String connectionString = DataManager.OraConnString();
            string query = " insert into association_info (asso_id,asso_name,asso_abvr) values (" +
                " " + " convert(numeric,'" + asso.AssoId + "')," + " '" + asso.AssoName + "'," + " '" + asso.AssoAbvr + "')";
            DataManager.ExecuteNonQuery(connectionString, query);
        }
        public static void UpdateAsso(classAsso asso)
        {
            String connectionString = DataManager.OraConnString();
            string query = " update association_info set asso_name= '" + asso.AssoName + "',asso_abvr= '" + asso.AssoAbvr + "' where asso_id=convert(numeric,'" + asso.AssoId + "')";
            DataManager.ExecuteNonQuery(connectionString, query);
        }
        public static void DeleteAsso(string assoid)
        {
            String connectionString = DataManager.OraConnString();
            string query = " delete from association_info  where asso_id=convert(numeric,'" + assoid + "')";
            DataManager.ExecuteNonQuery(connectionString, query);
        }
        public static classAsso getAsso(string assoid)
        {
            String connectionString = DataManager.OraConnString();
            string query = " select convert(varchar,asso_id)asso_id,asso_name,asso_abvr from association_info  where asso_id=convert(numeric,'" + assoid + "')";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Association");
            if (dt.Rows.Count == 0)
            {
                return null;
            }
            return new classAsso(dt.Rows[0]);
        }
        public static DataTable getAssos(string assoid)
        {
            String connectionString = DataManager.OraConnString();
            string query = " select convert(varchar,asso_id)asso_id,asso_name,asso_abvr from association_info  where convert(varchar,asso_id) like nullif('" + assoid + "','%') order by convert(numeric,asso_id)";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Associations");
            return dt;
        }
        public static DataTable getAssos()
        {
            String connectionString = DataManager.OraConnString();
            string query = " select asso_id,asso_name+' ('+asso_abvr+')' asso_name from association_info   order by convert(numeric,asso_id)";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Associations");
            return dt;
        }
    }
}