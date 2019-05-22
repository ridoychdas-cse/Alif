﻿using System;
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
/// Summary description for clsDesigManager
/// </summary>
/// 
namespace KHSC
{
    public class clsDesigManager
    {
        public static void CreateDesig(clsDesig des)
        {
            String connectionString = DataManager.OraConnString();
            string query = " insert into pmis_desig_code (desig_code,desig_name,desig_abb,mgr_code,grade_code,tech_ntech,class,officer_staff) values (" +
                " " + " '" + des.DesigCode + "'," + " '" + des.DesigName + "'," + " '" + des.DesigAbb + "'," + " '" + des.MgrCode + "', "+
                " " + " '" + des.GradeCode + "'," + " '" + des.TechNtech + "'," + " '" + des.Class + "'," + " '" + des.OfficerStaff + "' )";
            DataManager.ExecuteNonQuery(connectionString, query);
        }
        public static void UpdateDesig(clsDesig des)
        {
            String connectionString = DataManager.OraConnString();
            string query = " update pmis_desig_code set desig_name= '" + des.DesigName + "',desig_abb= '" + des.DesigAbb + "',mgr_code= '" + des.MgrCode + "', " +
                " grade_code= '" + des.GradeCode + "',tech_ntech= '" + des.TechNtech + "',class= '" + des.Class + "',officer_staff= '" + des.OfficerStaff + "' " +
                " where desig_code= '" + des.DesigCode + "'";
            DataManager.ExecuteNonQuery(connectionString, query);
        }
        public static void DeleteDesig(string desigcode)
        {
            String connectionString = DataManager.OraConnString();
            string query = " delete from pmis_desig_code where desig_code= '" + desigcode + "'";
            DataManager.ExecuteNonQuery(connectionString, query);
        }
        public static DataTable getDesigs(string designame)
        {
            String connectionString = DataManager.OraConnString();
            string query = "select desig_code,dbo.initcap(desig_name)desig_name,dbo.initcap(desig_abb)desig_abb,mgr_code,grade_code,class,tech_ntech,officer_staff from pmis_desig_code where lower(desig_name) like nullif('%"+designame+"%','%') order by convert(desig_code)";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Designations");
            return dt;
        }
        /*public static DataTable getDesigDetails(string designame)
        {
            String connectionString = DataManager.OraConnString();
            string query = "select desig_code,dbo.initcap(desig_name)desig_name,dbo.initcap(desig_abb)desig_abb,(select dbo.initcap(desig_name) from pmis_desig_code where desig_code=a.mgr_code) mgr_code, "+
            " (select scale from v_scale where scale_detail_id=a.grade_code) grade_code, "+
            " (select class_name from pmis_class where class_id=a.class) class,case when tech_ntech='T' then 'Technical' when tech_ntech='N' then 'Non-technical' end tech_ntech, "+
            " case when officer_staff='O' then 'Officer' when officer_staff='S' then 'Staff' end officer_staff from pmis_desig_code a where lower(desig_name) like nullif('%" + designame + "%','%') order by convert(varchar,desig_code)";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Designations");
            return dt;
        }*/
        public static DataTable getDesigDetails(string designame)
        {
            String connectionString = DataManager.OraConnString();
            string query = "select desig_code,dbo.initcap(desig_name)desig_name,dbo.initcap(desig_abb)desig_abb,(select dbo.initcap(desig_name) from pmis_desig_code where desig_code=a.mgr_code) mgr_code, " +
            " (select class_name from pmis_class where class_id=a.class) class,case when tech_ntech='T' then 'Technical' when tech_ntech='N' then 'Non-technical' end tech_ntech, " +
            " case when officer_staff='O' then 'Officer' when officer_staff='S' then 'Staff' end officer_staff from pmis_desig_code a where lower(desig_name) like nullif('%" + designame + "%','%') order by convert(varchar,desig_code)";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Designations");
            return dt;
        }
        public static clsDesig getDesig(string desigcode)
        {
            String connectionString = DataManager.OraConnString();
            string query = "select desig_code,dbo.initcap(desig_name)desig_name,desig_abb,mgr_code,grade_code,class,tech_ntech,officer_staff from pmis_desig_code where desig_code='" + desigcode + "'";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Designation");
            if (dt.Rows.Count == 0)
            {
                return null;
            }
            return new clsDesig (dt.Rows[0]);
        }
    }
}