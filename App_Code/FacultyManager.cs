using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KHSC;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Summary description for FacultyManager
/// </summary>
public class FacultyManager
{
	    public static void SaveFacultyInformation(clsFaculty aFaculty)
    {
        string connectionString = DataManager.OraConnString();
        string insertQuery = @"INSERT INTO [Faculty_Info]
           ([FacultyId]
           ,[FacultyName]
           ,[CourseId]
           ,[Type]
           ,[Status]
           ,[DesignationID]
           ,[EntryBy]
           ,[EntryDate]))
     VALUES('" + aFaculty.FacultyId + "','" + aFaculty.FacultyName + "','" + aFaculty.CourseName + "','" + aFaculty.Type + "','" + aFaculty.Status + "','" + aFaculty.Desognation + "','" + aFaculty.LoginBy+"',GETDATE())";
        DataManager.ExecuteNonQuery(connectionString, insertQuery);
    }

        public static void UpdateFacultyInformation(clsFaculty aFaculty)
        {
            string connectionString = DataManager.OraConnString();
            string updateQuery = @"UPDATE [Faculty_Info]
   SET [FacultyId] = '" + aFaculty.FacultyId + "',[FacultyName] = '" + aFaculty.FacultyName + "',[CourseId]='" + aFaculty.CourseName + "',[Type]='" + aFaculty.Type + "',[Status]='" + aFaculty.Status + "',[DesignationID]='" + aFaculty.Desognation + "',[UpdateBy]='" + aFaculty.LoginBy + "',[UpdateDate]=GETDATE()) WHERE [ID]='" +aFaculty.ID + "'";
            DataManager.ExecuteNonQuery(connectionString, updateQuery);
        }
        public DataTable GetFacultyInformationAll(string Faculty)
        {
            string connectionString = DataManager.OraConnString();
            string Parameter = "";
            if (!string.IsNullOrEmpty(Faculty))
            { Parameter = "and fi.ID='" + Faculty + "'"; }           
            string SelectQuery = @"SELECT TOP(20) fi.[ID] ,[FacultyId] ,[FacultyName]  ,fi.[CourseId], cn.CourseName ,[Type]  ,[Status] ,di.desig_name,[DesignationID] FROM  [Faculty_Info] fi inner join tbl_designation_information di on di.ID=fi.DesignationID inner join tbl_Course_Name cn on cn.ID=fi.CourseId where fi.DeleteBy is null
" + Parameter;
            DataTable dt = DataManager.ExecuteQuery(connectionString, SelectQuery, "Faculty_Info");
            return dt;
        }
        public string GetDepartmentAutoId()
        {
            SqlConnection connection = new SqlConnection(DataManager.OraConnString());
            connection.Open();
            try
            {
                string selectQuery = @"SELECT RIGHT(''+CONVERT(VARCHAR,ISNULL(MAX(CONVERT(INTEGER,RIGHT([FacultyId],2))),0)+1),2) FROM [Faculty_Info]";
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

        public void DeleteFacultyInformation(clsFaculty Faculty)       
         {
        string connectionString = DataManager.OraConnString();

        string deleteQuery = @"Update  [Faculty_Info] set [DeleteBy]='"+Faculty.LoginBy+"' ,[DeleteDate]=GETDATE()  WHERE [ID]='" +Faculty.ID+ "' ";
        DataManager.ExecuteNonQuery(connectionString, deleteQuery);
         }


        public static DataTable GetDesignation(string Deg)
        {
            string connectionString = DataManager.OraConnString();
            string Parameter = "";
            if (!string.IsNullOrEmpty(Deg)) 
            { Parameter = "and ID='" + Deg + "'"; }
            string SelectQuery = @"SELECT [ID] ,[desig_id]  ,[desig_name] FROM  [tbl_designation_information]
" + Parameter;
            DataTable dt = DataManager.ExecuteQuery(connectionString, SelectQuery, "tbl_designation_information");
            return dt;
        }

        public DataTable GetCourseInfo(string Course)
        {
            string connectionString = DataManager.OraConnString();
            string Parameter = "";
            if (!string.IsNullOrEmpty(Course)) 
            { Parameter = "and ID='" + Course + "'"; }            
            string SelectQuery = @"SELECT [ID] ,[CourseID] ,[CourseName] FROM [tbl_Course_Name]
" + Parameter;
            DataTable dt = DataManager.ExecuteQuery(connectionString, SelectQuery, "tbl_designation_information");
            return dt;
        }
}