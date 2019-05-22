using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KHSC;
using System.Data;

/// <summary>
/// Summary description for CourseTracManager
/// </summary>
public class CourseTracManager
{
	public CourseTracManager()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public System.Data.DataTable GetCourseTracDetailsInfo(string Trac)
    {
        string connectionString = DataManager.OraConnString();
        string Parameter = "";
        if (!string.IsNullOrEmpty(Trac))
        { Parameter = " WHERE id ='" + Trac + "' order by id asc"; }        
        string SelectQuery = @"SELECT [id] ,[TracId] ,[TracName]  FROM  [CourseTrac]" + Parameter;
        DataTable dt = DataManager.ExecuteQuery(connectionString, SelectQuery, "CourseTrac");
        return dt;
    }

    public void SaveCourseTracInformtion(clsCourseTrac Trac)
    {
        string connectionString = DataManager.OraConnString();

        string insertQuery = @"INSERT INTO [CourseTrac]
           ([TracId] ,[TracName])
     VALUES('" + Trac.CourseTracId + "','" + Trac.CourseTraceName + "')";
        DataManager.ExecuteNonQuery(connectionString, insertQuery);
    }
    public static void UpdateCoursetracInformtion(clsCourseTrac CourseTrac)
    {
        string connectionString = DataManager.OraConnString();

        string updateQuery = @"UPDATE [CourseTrac]
   SET [TracId] = '" + CourseTrac.CourseTracId + "' WHERE [TracName] ='" +CourseTrac.CourseTraceName + "'";
        DataManager.ExecuteNonQuery(connectionString, updateQuery);
    }

    public static void DeleteCourseTracInformtion(clsCourseTrac CourseTrac)
    {
        string connectionString = DataManager.OraConnString();

        string deleteQuery = @"DELETE FROM [CourseTrac]
      WHERE id='" + CourseTrac.ID + "' ";
        DataManager.ExecuteNonQuery(connectionString, deleteQuery);
    }
}