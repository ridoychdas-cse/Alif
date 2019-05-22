using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using KHSC;

/// <summary>
/// Summary description for AccountSetupGateway
/// </summary>
public class AccountSetupGateway
{
    SqlConnection connection = new SqlConnection(DataManager.OraConnString());

    internal void GetSaveAccountSetupInformation(AccountSetup accountSetupObj)
    {
        try
        {
            connection.Open();
            string selectQuery = @"INSERT INTO  [tbl_acc_setup]
           ([accs_id],[accs_head_id],[accs_head_nature],[accs_class_id],[accs_amount],[accs_transport_area])
     VALUES
           ('" + accountSetupObj.Id + "','" + accountSetupObj.AccountHead + "','" + accountSetupObj.Nature + "','" +accountSetupObj.Class+ "','" + accountSetupObj.Amount + "','" + accountSetupObj.TransportArea + "')";
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

    internal string GetAccountSetupAutoId()
    {
        try
        {
            connection.Open();
            string selectQuery = @"SELECT 'ACCS-' + RIGHT('000000'+CONVERT(VARCHAR,ISNULL(MAX(CONVERT(INTEGER,RIGHT([accs_id],6))),0)+1),6) FROM [tbl_acc_setup]";
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

    internal void DeleteAccountSetupInformation(AccountSetup accountSetupObj)
    {
        try
        {
            connection.Open();
            string selectQuery = @"DELETE  FROM [tbl_acc_setup] WHERE [accs_id] ='" + accountSetupObj.Id + "'  ";
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

    internal void UpdateAccountSetupInformation(AccountSetup accountSetupObj)
    {
        try
        {
            connection.Open();
            string selectQuery = @"UPDATE [tbl_acc_setup]
   SET[accs_transport_area] ='" + accountSetupObj.TransportArea + "',[accs_amount]='" + accountSetupObj.Amount + "',[accs_class_id]='"+accountSetupObj.Class+"',[accs_head_nature]='" + accountSetupObj.Nature + "',[accs_head_id]='" + accountSetupObj.AccountHead + "' WHERE [accs_id] ='" + accountSetupObj.Id + "'  ";
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

    internal DataTable GetAllAccountsHeadInformation()
    {
        try
        {
            connection.Open();
            string selectQuery = @"SELECT [accs_id]
      ,[accs_transport_area],[accs_amount],[accs_class_id],[accs_head_nature],[accs_head_id]
  FROM [tbl_acc_setup]";
            SqlDataAdapter da = new SqlDataAdapter(selectQuery, connection);
            DataSet ds = new DataSet();
            da.Fill(ds, "tbl_acc_setup");
            DataTable table = ds.Tables["tbl_acc_setup"];
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
}