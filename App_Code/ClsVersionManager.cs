using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

using System.Data.SqlClient;
using KHSC;
//using DBSC;

/// <summary>
/// Summary description for ClsVersionManager
/// </summary>
public class ClsVersionManager
{
	public ClsVersionManager()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public void SaveVersionInfo(ClsVersion aClsVersionObj)
    {
        string connectionString = DataManager.OraConnString();
        string insert = @"INSERT INTO [version_info]
           ([id]
           ,[version_id]
           ,[version_name]
           ,[class_id])
        VALUES('" + aClsVersionObj.VersionId + "','" + aClsVersionObj.VersionId + "','" + aClsVersionObj.VersionName + "','" + aClsVersionObj.ClassId + "')";

        DataManager.ExecuteNonQuery(connectionString, insert);
    }

    public void UpdateVersionInfo(ClsVersion aClsVersionObj)
    {
        string connectionString = DataManager.OraConnString();
        string update = @"UPDATE [version_info]
   SET [version_name] = '" + aClsVersionObj.VersionName + "',[class_id] = '" + aClsVersionObj.ClassId + "' WHERE [version_id] = '" + aClsVersionObj.VersionId + "'";

        DataManager.ExecuteNonQuery(connectionString, update);
    }

    public void DeleteVersionInfo(ClsVersion aClsVersionObj)
    {
        string connectionString = DataManager.OraConnString();
        string delete = @"DELETE FROM [version_info]
      WHERE [version_id] = '" + aClsVersionObj.VersionId + "'";

        DataManager.ExecuteNonQuery(connectionString, delete);
    }

    public DataTable GetVersionInfo(string Class_Id)
    {
        string connectionString = DataManager.OraConnString();
        string Found = "";
        if (Class_Id != "") { Found = "where t1.[class_id] ='" + Class_Id + "' order by t2.class_name"; } else { Found = "order by t2.class_name"; }
        string select = @"SELECT t1.[version_id]
      ,t1.[version_name]
      ,t1.[class_id]
      ,t2.class_name
  FROM [version_info] t1 inner join class_info t2 on t2.class_id=t1.[class_id] " + Found;
        DataTable dt = DataManager.ExecuteQuery(connectionString, select, "version_info");
        return dt;
    }

    public DataTable GetClassddlInfo()
    {
        string connectionString = DataManager.OraConnString();

        string SelectQuery = @"SELECT [class_id]
      ,[class_name]
  FROM [class_info]";
        DataTable dt = DataManager.ExecuteQuery(connectionString, SelectQuery, "class_info");
        return dt;
    }

    SqlConnection connection = new SqlConnection(DataManager.OraConnString());
    public string AutoId()
    {
       try
       {
           connection.Open();
        string selectQuery = @"SELECT RIGHT('00'+CONVERT(VARCHAR,ISNULL(MAX(CONVERT(INTEGER,RIGHT([version_id],2))),0)+1),2) FROM [version_info]";
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

    public static DataTable GetVersionDetailsInfo(string p)
    {
        string connectionString = DataManager.OraConnString();

        string SelectQuery = @"SELECT [id]
      ,[version_id]
      ,[version_name]
      ,[class_id]
  FROM [version_info] where [version_id] = '" + p + "'";
        DataTable dt = DataManager.ExecuteQuery(connectionString, SelectQuery, "version_info");
        return dt;
    }

    public static object GetVersionDetailsInformation(string p)
    {
        string connectionString = DataManager.OraConnString();

        string SelectQuery = @"SELECT [version_name],id      
  FROM [version_info]  WHERE [class_id] ='" + p + "'";
        DataTable dt = DataManager.ExecuteQuery(connectionString, SelectQuery, "version_info");
        return dt;
    }

    public static DataTable GetVersionInformation(string classId)
    {
        String ConnectionString = DataManager.OraConnString();
        SqlConnection myConnection = new SqlConnection(ConnectionString);
        try
        {
            myConnection.Open();
            string Query = @"SELECT * FROM version_info where class_id='" + classId + "'";
            DataTable dt = DataManager.ExecuteQuery(ConnectionString, Query, "section_info");
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

    public static DataTable GetVersionInformation()
    {
        String ConnectionString = DataManager.OraConnString();
        SqlConnection myConnection = new SqlConnection(ConnectionString);
        try
        {
            myConnection.Open();
            string Query = @"SELECT * FROM version_info";
            DataTable dt = DataManager.ExecuteQuery(ConnectionString, Query, "section_info");
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