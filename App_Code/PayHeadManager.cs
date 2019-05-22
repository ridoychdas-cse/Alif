using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KHSC;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Summary description for PayHeadManager
/// </summary>
public class PayHeadManager
{
	public PayHeadManager()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public static void SavePayHeadSetting(PayHead aPayHead)
    {
        String connectionString = DataManager.OraConnString();
        string query = @"INSERT INTO [payment_head] ([Head_Name]) VALUES ('"+aPayHead.PayHeadName+"')";
        DataManager.ExecuteNonQuery(connectionString, query);
    }

    public static void UpdatePayHeadSetting(PayHead aPayHead)
    {
        String connectionString = DataManager.OraConnString();
        string query = @"UPDATE [payment_head] SET [Head_Name] ='" + aPayHead.PayHeadName + "'  WHERE ID='" + aPayHead.ID + "'";
        DataManager.ExecuteNonQuery(connectionString, query);
    }

    public static void DeletePayHeadSetting(PayHead aPayHead)
    {
        String connectionString = DataManager.OraConnString();
        string query = @"DELETE FROM [payment_head] WHERE ID='"+aPayHead.ID+"'";
        DataManager.ExecuteNonQuery(connectionString, query);
    }

    public static DataTable GetShowPayheadInfo()
    {
        string connectionString = DataManager.OraConnString();
        SqlConnection sqlCon = new SqlConnection(connectionString);
        string query = @"SELECT [ID]
      ,[Head_Name]
  FROM [payment_head] order by ID asc ";
        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "payment_head");
        return dt;
    }
}