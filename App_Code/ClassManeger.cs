using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.SqlClient;
using System.Data;
using KHSC;
//using DBSC;

/// <summary>
/// Summary description for ClassManeger
/// </summary>
public class ClassManeger
{
	public ClassManeger()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public void SaveClassInformtion(ClassEnty aClassEntryObj)
    {
        string connectionString = DataManager.OraConnString();

        string insertQuery = @"INSERT INTO [class_info]
           (id,[class_id]
           ,[class_name],CollegeId,SemisterId)
     VALUES('" + aClassEntryObj.ClassId + "','" + aClassEntryObj.ClassId + "','" + aClassEntryObj.ClassName + "','" + aClassEntryObj.CollegeName + "','" + aClassEntryObj.SemisterName + "')";
        DataManager.ExecuteNonQuery(connectionString, insertQuery);
    }

    public void UpdateClassInformtion(ClassEnty aClassEntryObj)
    {
        string connectionString = DataManager.OraConnString();

        string updateQuery = @"UPDATE [class_info]
   SET [class_name] = '" + aClassEntryObj.ClassName + "',[CollegeId] = '" + aClassEntryObj.CollegeName + "',[SemisterID] = '" + aClassEntryObj.SemisterName + "' WHERE [class_id] ='" + aClassEntryObj.ClassId + "'";
        DataManager.ExecuteNonQuery(connectionString, updateQuery);
    }

    public void DeleteClassInformtion(ClassEnty aClassEntryObj)
    {
        string connectionString = DataManager.OraConnString();

        string deleteQuery = @"DELETE FROM [class_info]
      WHERE id='" + aClassEntryObj.ClassId + "' ";
        DataManager.ExecuteNonQuery(connectionString, deleteQuery);
    }

    public DataTable GetClassddlInfo()
    {
        string connectionString = DataManager.OraConnString();

        string SelectQuery = @"SELECT [CollegeId]
      ,[CollegeName]
  FROM [College_Info]";
        DataTable dt = DataManager.ExecuteQuery(connectionString, SelectQuery, "College_Info");
        return dt;
    }

    public DataTable GetClassInfo(string ClassID)
    {

        string connectionString = DataManager.OraConnString();
        string Parameter = "";
        if (ClassID == "") { Parameter = ""; } else { Parameter = "where t1.CollegeId='" + ClassID + "'"; }
        string SelectQuery = @"SELECT t1.[class_id] 
      ,t1.[class_name] 
      ,t1.[CollegeId]
      ,t2.CollegeName
,t3.SemisterId,t3.SemisterName
  FROM [class_info] t1 inner join College_Info t2 on t2.id=t1.CollegeId left join SemisterInfo t3 on t3.SemisterId=t1.SemisterId  " + Parameter;
        DataTable dt = DataManager.ExecuteQuery(connectionString, SelectQuery, "class_info");
        return dt;

//        string connectionString = DataManager.OraConnString();
//        string Parameter = "";
//        if (ClassID == "")
//        { Parameter = "order by ID asc"; }
//        else { Parameter = " WHERE class_id ='" + ClassID + "' order by ID asc"; }
//        string SelectQuery = @"SELECT [class_id] 
//      ,[class_name],CollegeId,SemisterId 
//  FROM [class_info] " + Parameter;
//        DataTable dt = DataManager.ExecuteQuery(connectionString, SelectQuery, "class_info");
//        return dt;
    }

    public DataTable GetClassDetailsInfo(string p)
    {
        string connectionString = DataManager.OraConnString();

        string SelectQuery = @"SELECT [class_id]
      ,[class_name]
,CollegeId,SemisterId
  FROM [class_info] where [class_id]='" + p + "'";
        DataTable dt = DataManager.ExecuteQuery(connectionString, SelectQuery, "class_info");
        return dt;
    }

    SqlConnection connection = new SqlConnection(DataManager.OraConnString());
    public string GetAutoId()
    {
        connection.Open();
        try
        {
            string selectQuery = @"SELECT RIGHT('00'+CONVERT(VARCHAR,ISNULL(MAX(CONVERT(INTEGER,RIGHT([class_id],2))),0)+1),2) FROM [class_info]";
            //  string selectQuery = @"SELECT '00' + RIGHT('000000'+CONVERT(VARCHAR,ISNULL(MAX(CONVERT(INTEGER,RIGHT([class_id],6))),0)+1),6) FROM [class_info]";
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
}