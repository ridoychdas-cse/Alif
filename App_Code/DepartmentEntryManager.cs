using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using KHSC;

/// <summary>
/// Summary description for DepartmentEntryManager
/// </summary>
public class DepartmentEntryManager
{
	public DepartmentEntryManager()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public void SaveDepartmentInformation(DepartmentEntry aDepartmentEntry)
    {
        string connectionString = DataManager.OraConnString();
        string insertQuery = @"INSERT INTO [Department_info]
           ( [DeptId]
           ,[DeptName]
           ,[CollegeId])
     VALUES('" + aDepartmentEntry.DeptId + "','" + aDepartmentEntry.DeptName + "','" + aDepartmentEntry.CollegeName + "')";
        DataManager.ExecuteNonQuery(connectionString, insertQuery);
    }

    public void UpdateDepartmentInformation(DepartmentEntry aDepartmentEntry)
    {
        string connectionString = DataManager.OraConnString();
        string updateQuery = @"UPDATE [Department_Info]
   SET [DeptName] = '" + aDepartmentEntry.DeptName + "',[CollegeId] = '" + aDepartmentEntry.CollegeName + "' WHERE [DeptId]='" + aDepartmentEntry.DeptId + "'";
        DataManager.ExecuteNonQuery(connectionString, updateQuery);
    }

    public void DeleteDepartmentInformation(DepartmentEntry aDepartmentEntry)
    {
        string connectionString = DataManager.OraConnString();

        string deleteQuery = @"DELETE FROM [Department_Info]
      WHERE [DeptId]='" + aDepartmentEntry.DeptId + "' ";
        DataManager.ExecuteNonQuery(connectionString, deleteQuery);
    }



    public DataTable GetFacultyInformationAll(string ClassId)
    {
        string connectionString = DataManager.OraConnString();
        string Parameter = "";
        if (ClassId == "") { Parameter = ""; } else { Parameter = "and t1.CollegeId='" + ClassId + "'"; }
        string SelectQuery = @"SELECT fi.[ID] ,[FacultyId] ,[FacultyName]  ,fi.[CourseId], cn.CourseName ,[Type]  ,[Status] ,di.desig_name,[DesignationID] FROM  [Faculty_Info] fi inner join tbl_designation_information di on di.ID=fi.DesignationID inner join tbl_Course_Name cn on cn.ID=fi.CourseId where fi.DeleteBy is null
" + Parameter;
        DataTable dt = DataManager.ExecuteQuery(connectionString, SelectQuery, "Department_Info");
        return dt;
    }

    public static DataTable GetDepartmentDetailsInfo(string SectionId)
    {
        string connectionString = DataManager.OraConnString();

        string selectQuery = @"SELECT [DeptId]
      ,[DeptName]
      ,[CollegeName]
  FROM [Department_Info] where [DeptId]='" + SectionId + "'";
        DataTable dt = DataManager.ExecuteQuery(connectionString, selectQuery, "Department_Info");
        return dt;
    }

    SqlConnection connection = new SqlConnection(DataManager.OraConnString());
    public string GetDepartmentAutoId()
    {
        connection.Open();
        try
        {
            string selectQuery = @"SELECT RIGHT(''+CONVERT(VARCHAR,ISNULL(MAX(CONVERT(INTEGER,RIGHT([DeptId],2))),0)+1),2) FROM [Department_Info]";
            SqlCommand command = new SqlCommand(selectQuery, connection);
            return command.ExecuteScalar().ToString();
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

//    public DataTable GetClassddlInfo()
//    {
//        string connectionString = DataManager.OraConnString();

//        string SelectQuery = @"SELECT [CollegeId]
//      ,[CollegeName]
//  FROM [College_Info]";
//        DataTable dt = DataManager.ExecuteQuery(connectionString, SelectQuery, "College_Info");
//        return dt;
//    }



    public static DataTable GetShowDepartmentInformation()
    {
        String ConnectionString = DataManager.OraConnString();
        SqlConnection myConnection = new SqlConnection(ConnectionString);
        try
        {
            myConnection.Open();
            string Query = @"SELECT * FROM Department_Info";
            DataTable dt = DataManager.ExecuteQuery(ConnectionString, Query, "Department_Info");
            return dt;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        finally
        {
            if (myConnection.State == ConnectionState.Open)
                myConnection.Close();
        }
    }
}