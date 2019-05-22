using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using KHSC;

/// <summary>
/// Summary description for CourseRegManager
/// </summary>
public class CourseRegManager
{
    public void SaveCourseRegistrationInformtion(clsCourseRegistration CourseObj)
    {
        string connectionString = DataManager.OraConnString();

        string insertQuery = @"INSERT INTO [tbl_Course_Name]
           ([CourseID] ,[CourseName] ,[TracID] ,[CourseFee] ,[Doscount],[EntryBy] ,[EntryDate])
     VALUES('" + CourseObj.CourseId + "','" + CourseObj.CoursesName + "','" + CourseObj.CourseTrac + "','" + CourseObj.Fee + "','" + CourseObj.Discount + "','" + CourseObj.LoginBy + "',GETDATE())";
        DataManager.ExecuteNonQuery(connectionString, insertQuery);
    }

    public void UpdateCourseRegistrationInformtion(clsCourseRegistration CourseObj)
    {
        string connectionString = DataManager.OraConnString();

        string updateQuery = @"UPDATE [tbl_Course_Name]
   SET [CourseID] = '" + CourseObj.CourseId + "',[CourseName] = '" + CourseObj.CoursesName + "',[TracID] = '" + CourseObj.CourseTrac + "',[CourseFee]='" + CourseObj.Fee + "',[Doscount]='" + CourseObj.Discount + "' WHERE [ID] ='" + CourseObj.ID + "'";
        DataManager.ExecuteNonQuery(connectionString, updateQuery);
    }

    public void DeleteCourseInformtion(clsCourseRegistration CourseObj)
    {
        string connectionString = DataManager.OraConnString();

        string deleteQuery = @"UPDATE [tbl_Course_Name]
   SET [DeleteBy]='" + CourseObj.LoginBy + "',[DeleteDate]=GETDATE()  WHERE ID='" + CourseObj.ID + "' ";    
        DataManager.ExecuteNonQuery(connectionString, deleteQuery);
    }

    public DataTable GetCourselInfo(string CourseID)
    {

        string connectionString = DataManager.OraConnString();
        string Parameter = "";
        if (!string.IsNullOrEmpty(CourseID)) 
        { Parameter = "and cn.ID='" + CourseID + "'"; }
        string SelectQuery = @"SELECT cn.[ID] ,[CourseID] ,[CourseName],ct.TracName,cn.[TracID] ,convert(decimal(18,1),[CourseFee])as Fees ,convert(decimal(18,1),[Doscount])as Discount ,[Description] FROM [tbl_Course_Name] cn inner join CourseTrac ct on  ct.id=cn.TracID where cn.DeleteBy is null " + Parameter;
        DataTable dt = DataManager.ExecuteQuery(connectionString, SelectQuery, "tbl_Course_Name");
        return dt;
    }

    public DataTable GetClassDetailsInfo(string Course)
    {
        string connectionString = DataManager.OraConnString();
        string Par = "";
        if (!string.IsNullOrEmpty(Course))
        {
            Par = "and [ID]='" + Course + "'";
        }
        string SelectQuery = @"SELECT [ID] ,[CourseID] ,[CourseName] ,[TracID] ,convert(decimal(18,1),[CourseFee])as Fees ,convert(decimal(18,1),[Doscount]) as Discount ,[Description] FROM  [tbl_Course_Name] where DeleteBy is null " + Par + " ";
        DataTable dt = DataManager.ExecuteQuery(connectionString, SelectQuery, "tbl_Course_Name");
        return dt;
    }

    SqlConnection connection = new SqlConnection(DataManager.OraConnString());
    public string GetAutoId()
    {
        connection.Open();
        try
        {
            string selectQuery = @"SELECT RIGHT('CUR'+'00'+CONVERT(VARCHAR,ISNULL(MAX(CONVERT(INTEGER,RIGHT([CourseID],2))),0)+1),2) FROM [tbl_Course_Name]";
            //  string selectQuery = @"SELECT '00' + RIGHT('000000'+CONVERT(VARCHAR,ISNULL(MAX(CONVERT(INTEGER,RIGHT([class_id],6))),0)+1),6) FROM [tbl_Course_Name]";
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

    public DataTable GetCourseTracInfo(string Trac)
    {
        string connectionString = DataManager.OraConnString();
        string Par = "";
        if (!string.IsNullOrEmpty(Trac))
        {
            Par = "and [id]='" + Trac + "'";
        }
        string SelectQuery = @"SELECT [id] ,[TracId] ,[TracName] FROM [dbo].[CourseTrac]" + Par + " ";
        DataTable dt = DataManager.ExecuteQuery(connectionString, SelectQuery, "CourseTrac");
        return dt;
    }

    public static DataTable GetClassDetailsInfo()
    {
        string connectionString = DataManager.OraConnString();        
        string SelectQuery = @"SELECT [id] ,[TracId] ,[TracName] FROM [dbo].[CourseTrac]";
        DataTable dt = DataManager.ExecuteQuery(connectionString, SelectQuery, "CourseTrac");
        return dt;
    }

    public static DataTable GetCourseDetailsInfo(string Course)
    {
        string connectionString = DataManager.OraConnString();
        string Par = "";
        if (!string.IsNullOrEmpty(Course))
        {
            Par = "and [ID]='" + Course + "'";
        }
        string SelectQuery = @"SELECT [ID] ,[CourseID] ,[CourseName] ,[TracID] ,convert(decimal(18,1),[CourseFee])as Fees ,convert(decimal(18,1),[Doscount]) as Discount ,[Description] FROM  [tbl_Course_Name] where DeleteBy is null " + Par + " ";
        DataTable dt = DataManager.ExecuteQuery(connectionString, SelectQuery, "tbl_Course_Name");
        return dt;
    }

    public static DataTable GetFAcultyNAme(string Faculty)
    {
        string connectionString = DataManager.OraConnString();
        string SelectQuery = @"select FacultyId+'-'+FacultyName as search,ID,FacultyId,FacultyName from Faculty_Info where (FacultyId+'-'+FacultyName)='" + Faculty + "' ";
        DataTable dt = DataManager.ExecuteQuery(connectionString, SelectQuery, "tbl_Course_Name");
        return dt;
    }

    public static DataTable GetAllDaysInfor(string Days)
    {
        string Par = "";
        if (!string.IsNullOrEmpty(Days))
        {
            Par = " where [ID]='" + Days + "'";
        }
        string connectionString = DataManager.OraConnString();
        string SelectQuery = @"select ID,Days_name from tbl_days_info "+Par+" ";
        DataTable dt = DataManager.ExecuteQuery(connectionString, SelectQuery, "tbl_Course_Name");
        return dt;
    }

    public static DataTable GetCourseDetailsOfCourseShedule(string MSTID)
    {
        string Par = "";
        if (!string.IsNullOrEmpty(MSTID))
        {
            Par = " where [MSTID]='" + MSTID + "'";
        }
        string connectionString = DataManager.OraConnString();
        string SelectQuery = @"SELECT sd.[ID]
      ,[MSTID]
      ,[Days]
      ,convert(char(5),[StartTime],108)as [sttime]     
      ,SUBSTRING(CONVERT(varchar(20), StartTime, 22), 18, 3)as StAPM
      ,[StartAmPm]
      ,convert(char(5),[EndtTime],108)as [endtime]
      ,SUBSTRING(CONVERT(varchar(20), EndtTime, 22), 18, 3)as EndAPM
      ,[EndAmPm]
      ,[RoomNo]
      ,[Flag]
      ,dt.Days_name
  FROM [TBL_SHEDULE_ENTRY_DTL] sd
  inner join Tbl_Days_Info dt on dt.ID=sd.Days  " + Par + " ";
        DataTable dt = DataManager.ExecuteQuery(connectionString, SelectQuery, "TBL_SHEDULE_ENTRY_DTL");
        return dt;
    }

    public static DataTable GetCourseSheduleInfo(string RFID)
    {
        string Par = "";
        if (!string.IsNullOrEmpty(RFID))
        {
            Par = " where sm.[ID]='" + RFID + "'";
        }
        string connectionString = DataManager.OraConnString();
        string SelectQuery = @"SELECT sm.[ID]
      ,[TracID]
      ,sm.[CourseID]
      ,sm.[FacultyID]
      ,[BatchNo]
      ,sm.[Status]
      ,[Flag]
      ,convert(nvarchar,[SheduleStartDate],103)as dttST
      ,convert(nvarchar,[SheduleEndDate],103)as dttEnd
      ,sm.[Year]
      ,sm.[EntryBy]
      ,sm.[EntryDate]
      ,fi.FacultyName     
  FROM [TBL_SHEDULE_ENTRY_MST] sm
  inner join Faculty_Info fi on fi.ID=sm.FacultyID " + Par + " ";
        DataTable dt = DataManager.ExecuteQuery(connectionString, SelectQuery, "TBL_SHEDULE_ENTRY_MST");
        return dt;
    }
}