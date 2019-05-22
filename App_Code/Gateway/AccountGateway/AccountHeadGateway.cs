using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using KHSC;

/// <summary>
/// Summary description for AccountHeadGateway
/// </summary>
public class AccountHeadGateway
{
    SqlConnection connection = new SqlConnection(DataManager.OraConnString());

    internal string GetAccountHeadAutoId()
    {

            try
            {
                connection.Open();
                string selectQuery = @"SELECT 'ACC-' + RIGHT('000000'+CONVERT(VARCHAR,ISNULL(MAX(CONVERT(INTEGER,RIGHT([acch_id],6))),0)+1),6) FROM [tbl_acc_head]";
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

    internal void SaveTheAccountHeadInformation(AccountHead accountHeadObj)
    {
        try
        {
            connection.Open();
            string selectQuery = @"INSERT INTO  [tbl_acc_head]
           ([acch_id]
           ,[acch_name])
     VALUES
           ('" + accountHeadObj.Id + "','" + accountHeadObj.AccountName + "')";
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
    //********************** Head Information *********************//
    internal DataTable GetAllAccountsHeadInformation()
    {
        try
        {
            connection.Open();
            string selectQuery = @"SELECT [acch_id]
      ,[acch_name]
  FROM [tbl_acc_head]";
            SqlDataAdapter da = new SqlDataAdapter(selectQuery, connection);
            DataSet ds = new DataSet();
            da.Fill(ds, "tbl_acc_head");
            DataTable table = ds.Tables["tbl_acc_head"];
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

    internal void DeleteTheAcccountInformation(AccountHead accountHeadObj)
    {
        try
        {
            connection.Open();
            string selectQuery = @"DELETE  FROM [tbl_acc_head] WHERE [acch_id] ='" + accountHeadObj.Id + "'  ";
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

    internal void UpdateTheAcccountInformation(AccountHead accountHeadObj)
    {
        try
        {
            connection.Open();
            string selectQuery = @"UPDATE [tbl_acc_head]
   SET[acch_name] ='" + accountHeadObj.AccountName + "' WHERE [acch_id] ='" + accountHeadObj.Id + "'  ";
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