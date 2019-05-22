using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using KHSC;
using System.Data;

/// <summary>
/// Summary description for ClassEntryGateway
/// </summary>
public class ClassEntryGateway
{
    SqlConnection connection = new SqlConnection(DataManager.OraConnString());

    internal string GetClassAutoId()
    {
        try
        {
            connection.Open();
            string selectQuery = @"SELECT 'CLS-' + RIGHT('000000'+CONVERT(VARCHAR,ISNULL(MAX(CONVERT(INTEGER,RIGHT([cls_id],6))),0)+1),6) FROM [tbl_class_entry_information]";
            SqlCommand command = new SqlCommand(selectQuery, connection);
            return command.ExecuteScalar().ToString();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        finally
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }
    }

    internal DataTable GetAllClassInformation()
    {
        try
        {
            connection.Open();
            string selectQuery = @"SELECT [cls_id]
      ,[cls_name]
  FROM [tbl_class_entry_information]";
            SqlDataAdapter da = new SqlDataAdapter(selectQuery, connection);
            DataSet ds = new DataSet();
            da.Fill(ds, "tbl_class_entry_information");
            DataTable table = ds.Tables["tbl_class_entry_information"];
            return table;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);

        }
        finally
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();

            }
        }
    }

    internal void SaveTheClassInformation(ClassItem aClassitemObj)
    {
        try
        {
            connection.Open();
            string selectQuery = @"INSERT INTO [tbl_class_entry_information]
           ([cls_id]
           ,[cls_name])
     VALUES
           ('" + aClassitemObj.Id + "','" + aClassitemObj.ClassName + "')";
            SqlCommand command = new SqlCommand(selectQuery, connection);
            command.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        finally
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }
    }

    internal void UpdateTheClassInformation(ClassItem aClassitemObj)
    {
        try
        {
            connection.Open();
            string selectQuery = @"UPDATE  [tbl_class_entry_information]
   SET[cls_name] ='" + aClassitemObj.ClassName + "' WHERE [cls_id] ='" + aClassitemObj.Id + "'  ";
            SqlCommand command = new SqlCommand(selectQuery, connection);
            command.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        finally
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }
    }

    internal void DeleteTheClassInformation(ClassItem aClassitemObj)
    {
        try
        {
            connection.Open();
            string selectQuery = @"DELETE  FROM [tbl_class_entry_information] WHERE [cls_id] ='" + aClassitemObj.Id + "'  ";
            SqlCommand command = new SqlCommand(selectQuery, connection);
            command.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        finally
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }
    }
}