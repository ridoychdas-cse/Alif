using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using KHSC;
using System.Data;

/// <summary>
/// Summary description for InstallmentFeeManager
/// </summary>
public class InstallmentFeeManager
{
    SqlConnection connection = new SqlConnection(DataManager.OraConnString());
    SqlTransaction transaction;

	public InstallmentFeeManager()
	{
		//
		// TODO: Add constructor logic here
		//
	}



    public void CreateInstallment(DataTable dt1,InstallmentFee Ins)
    {
       

        try
        {
            connection.Open();
            transaction = connection.BeginTransaction();
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.Transaction = transaction;

            SqlCommand command1 = new SqlCommand();
            command1.Connection = connection;
            command1.Transaction = transaction;

            command1.CommandText = @"SELECT COUNT(*)  FROM [InstallmentMst] where [StudentId]='" + Ins.StudentId + "'";
            int COUNT = Convert.ToInt32(command1.ExecuteScalar());
            if (COUNT > 0)
            {
                command.CommandText = @"UPDATE  [InstallmentMst]
   SET [Serial] ='"+Ins.Id+"'  ,[StudentId] = '"+Ins.StudentId+"' ,[TotalAmt] = '"+Ins.TotalAmt+"' ,[PayDate] = convert(date,'" + Ins.PayDate + "',103),[MonthInterval] = '"+Ins.MonthInterval+"' ,[InsQty] ='"+Ins.InsQty+"'  ,[AdmissionFee] = '"+Ins.AdmissionFee+"' ,[UpdateDate] =GETDATE(),[UpdateUser] = '"+Ins.Loginby+"'  WHERE [StudentId] = '"+Ins.StudentId+"'";
                command.ExecuteNonQuery();

                command.CommandText = @"SELECT ID  FROM [InstallmentMst] where  StudentId='"+Ins.StudentId+"'";
                int ID = Convert.ToInt32(command.ExecuteScalar());
                command1.CommandText = @"DELETE FROM [InstallmentDtl] WHERE MstId='" + ID + "'";
                command1.ExecuteNonQuery();

                foreach (DataRow dr in dt1.Rows)
                {
                     
                        command1.CommandText = @"INSERT INTO [InstallmentDtl]
               (InstallmentId,[MstId] ,[InstallAmt] ,[InstallDate])
         VALUES ('" + dr["Installment_Serial"].ToString() + "','" + ID + "','" + Convert.ToDouble(dr["Installment_Amount"].ToString()) + "',convert(date,'" + dr["Installment_Date"].ToString() + "',103))";
                        command1.ExecuteNonQuery();
 
                }

            }
            else
            {

                command1.CommandText = @"INSERT INTO [InstallmentMst]
           (Serial,[StudentId],[TotalAmt],[PayDate],[MonthInterval],[InsQty],[AdmissionFee],[EntryUser],[EntryDate])
     VALUES ((SELECT isnull(max([Serial]),0) FROM [InstallmentMst])+1,'" + Ins.StudentId + "','" + Ins.TotalAmt + "',convert(date,'" + Ins.PayDate + "',103),'" + Ins.MonthInterval + "','" + Ins.InsQty + "','" + Ins.AdmissionFee + "','" + Ins.Loginby + "',GETDATE())";
                command1.ExecuteNonQuery();

                command.CommandText = @"SELECT [Id]  FROM [InstallmentMst] order by [Id] desc";
                int MstID = Convert.ToInt32(command.ExecuteScalar());

                foreach (DataRow dr in dt1.Rows)
                {
                     
                        command1.CommandText = @"INSERT INTO [InstallmentDtl]
               (InstallmentId,[MstId] ,[InstallAmt] ,[InstallDate])
         VALUES ('" + dr["Installment_Serial"].ToString() + "','" + MstID + "','" + Convert.ToDouble(dr["Installment_Amount"].ToString()) + "',convert(date,'" + dr["Installment_Date"].ToString() + "',103))";
                        command1.ExecuteNonQuery();
                     
                }
            }
            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
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

    public string GetAutoId()
    {
        connection.Open();
        try
        {
            string selectQuery = @"SELECT RIGHT(''+CONVERT(VARCHAR,ISNULL(MAX(CONVERT(INTEGER,RIGHT([Serial],2))),0)+1),2) FROM [InstallmentMst]";             
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

    public static InstallmentFee GetInstallmentFeeInfo(string studentId)
    {
        string connectionString = DataManager.OraConnString();
        SqlConnection sqlCon = new SqlConnection(connectionString);
        try
        {
            sqlCon.Open();
            string selectQuery = @"SELECT [Serial],[StudentId],[TotalAmt],convert(varchar,PayDate,103) [PayDate],[MonthInterval],[InsQty],[AdmissionFee]  FROM  [InstallmentMst] WHERE [StudentId]='" + studentId + "'";
            SqlCommand cmd = new SqlCommand(selectQuery, sqlCon);
            SqlDataReader reader = cmd.ExecuteReader();
            InstallmentFee aInstallmentFeeObj = new InstallmentFee();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    aInstallmentFeeObj.Id = reader[0].ToString();
                    aInstallmentFeeObj.StudentId = reader[1].ToString();
                    aInstallmentFeeObj.TotalAmt = reader[2].ToString();
                    aInstallmentFeeObj.PayDate = reader[3].ToString();
                    aInstallmentFeeObj.MonthInterval = reader[4].ToString();
                    aInstallmentFeeObj.InsQty = reader[5].ToString();
                    aInstallmentFeeObj.AdmissionFee = reader[6].ToString();
                    

                }
            }
            return aInstallmentFeeObj;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        finally
        {
            if (sqlCon.State == ConnectionState.Open)
                sqlCon.Close();
        }
    }

    public static DataTable GetInstallmentFeeDtls(string p)
    {

        string connectionString = DataManager.OraConnString();
        SqlConnection oracon = new SqlConnection(connectionString);
        string query = @"SELECT t1.[InstallmentId] as Installment_Serial,t1.[InstallAmt] as Installment_Amount ,convert(varchar,t1.[InstallDate],103) as Installment_Date FROM  [InstallmentDtl] t1
inner join InstallmentMst t2 on t1.MstId=t2.Id where t2.StudentId='" + p + "'";
        DataTable dt = DataManager.ExecuteQuery(connectionString, query, "InstallmentMst");
        return dt;

         
         
    }
}