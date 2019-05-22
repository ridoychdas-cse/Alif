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
using KHSC;

/// <summary>
/// Summary description for DivDisThanaManager
/// </summary>
/// 
namespace KHSC
{
    public class DivDisThanaManager
    {
        public static DataTable getDivision()
        {
            String connectionString = DataManager.OraConnString();
            string query = "select division_code,initcap(division_name)division_name from pmis_division_code";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Division");
            return dt;
        }
        public static DataTable getDistrict(string criteria)
        {
            String connectionString = DataManager.OraConnString();
            string query = "select initcap(b.division_name)division_name,a.district_code,initcap(a.district_name)district_name from pmis_district_code a,pmis_division_code b "+
                " where a.division_code=b.division_code";
            if (criteria != "")
            {
                query = query + " and " + criteria;
            }
            query = query + " order by convert(a.division_code),convert(a.district_code)";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Division");
            return dt;
        }
        public static DataTable getThana(string criteria)
        {
            String connectionString = DataManager.OraConnString();
            string query = "select b.division_code,initcap(c.division_name)division_name,a.district_code,initcap(b.district_name)district_name,a.thana_code,initcap(a.thana_name)thana_name "+
            " from pmis_thana_code a,pmis_district_code b,pmis_division_code c "+
            " where a.district_code=b.district_code and b.division_code=c.division_code";
            if (criteria != String.Empty)
            {
                query = query + " and " + criteria;
            }
            query = query + " order by convert(b.division_code),convert(a.district_code),convert(a.thana_code)";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Division");
            return dt;
        }
        public static DataTable getBranch()
        {
            String connectionString = DataManager.OraConnString();
            string query = "select branch_code,dbo.initcap(branch_name)branch_name from pmis_branch order by convert(numeric,branch_code)";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Branch");
            return dt;
        }
        public static DataTable getDesignation()
        {
            String connectionString = DataManager.OraConnString();
            string query = "select desig_code,dbo.initcap(desig_name)desig_name from pmis_desig_code";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Designation");
            return dt;
        }

         
    }
}