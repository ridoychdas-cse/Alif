using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using KHSC;

/// <summary>
/// Summary description for clsCourseRegManager
/// </summary>
public class clsCourseRegManager
{
	public clsCourseRegManager()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public static DataTable GetAllStudent(string StdID, string Name)
    {
        String connectionString = DataManager.OraConnString();
        SqlConnection sqlCon = new SqlConnection(connectionString);

        string query = "Select a.ID,a.student_id,a.f_name,a.mobile_no,a.email from student_info a";

        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "student_info");
        return dt;
    }
}