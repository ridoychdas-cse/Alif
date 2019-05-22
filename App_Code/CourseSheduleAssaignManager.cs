using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using KHSC;
using System.Data;

/// <summary>
/// Summary description for CourseSheduleAssaignManager
/// </summary>
public class CourseSheduleAssaignManager
{
	public CourseSheduleAssaignManager()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public static void SaveSheduleCourseBatch(System.Data.DataTable dt, clsCourseSheduleAssign sheduleObj)
    {
        SqlConnection connection = new SqlConnection(DataManager.OraConnString());
        SqlTransaction transection;
        try
        {
            connection.Open();
            transection = connection.BeginTransaction();
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.Transaction = transection;
            SqlCommand command1 = new SqlCommand();
            command1.Connection = connection;
            command1.Transaction = transection;

            command.CommandText = "Select ID,CourseID,FacultyID,BatchNo,Status,Flag,SheduleStartDate,SheduleEndDate from TBL_SHEDULE_ENTRY_MST where BatchNo='" + sheduleObj.BatchNo + "' and CourseID='" + sheduleObj.CourseID + "'";
            int Count = Convert.ToInt32(command.ExecuteScalar());
            if (Count > 0)
            {
                command.CommandText = @"delete from [TBL_SHEDULE_ENTRY_MST] where BatchNo='" + sheduleObj.BatchNo + "' and CourseID='" + sheduleObj.CourseID + "'";
                command.ExecuteNonQuery();

                command1.CommandText = @"INSERT INTO [TBL_SHEDULE_ENTRY_MST]
           ([TracID],[CourseID],[FacultyID],[BatchNo],[Status],[Flag],[SheduleStartDate],[SheduleEndDate],[EntryBy],[EntryDate])
            VALUES
           ('" + sheduleObj.TracID + "','" + sheduleObj.CourseID + "','" + sheduleObj.FacultyID + "','" + sheduleObj.BatchNo + "','" + sheduleObj.Status + "',1,convert(date,'" + sheduleObj.StartDate + "',103),convert(date,'" + sheduleObj.EndDate + "',103),'" + sheduleObj.LoginBy + "',GETDATE() )";
                command1.ExecuteNonQuery();

                command.CommandText = @"SELECT TOP(1)[ID] FROM [TBL_SHEDULE_ENTRY_MST] ORDER BY ID DESC";
                int ID = Convert.ToInt32(command.ExecuteScalar());

                foreach (DataRow drsh in dt.Rows)
                {
                    if (drsh["DaysID"].ToString() != "")
                    {
                        string StartDateTime =
                                   //DateTime.Now.ToString("MM/dd/yyyy") + " " +
                                               drsh["StartTime"].ToString() + " " +
                                               drsh["StartAmPm"].ToString();
                        string EndDateTime = 
                            //DateTime.Now.ToString("MM/dd/yyyy") + " " +
                                             drsh["EndtTime"].ToString() + " " +
                                             drsh["EndAmPm"].ToString();
                        command.CommandText = @"INSERT INTO [TBL_SHEDULE_ENTRY_DTL]
           ([MSTID],[Days],[StartTime],[EndTime],[RoomNo],[Flag])
                            VALUES
                        ('" + ID + "','" + drsh["DaysID"].ToString() + "',convert(datetime,'" + drsh["StartTime"].ToString() +
                                              "',103),convert(datetime,'" + drsh["EndtTime"].ToString() + "',103),'" +
                                              drsh["RoomNo"].ToString() + "','1')";
                        command.ExecuteNonQuery();

                    }
                }
            }
            else
            {
                command1.CommandText = @"INSERT INTO [TBL_SHEDULE_ENTRY_MST]
           ([TracID],[CourseID],[FacultyID],[BatchNo],[Status],[Flag],[SheduleStartDate],[SheduleEndDate],[EntryBy],[EntryDate])
            VALUES
           ('" + sheduleObj.TracID + "','" + sheduleObj.CourseID + "','" + sheduleObj.FacultyID + "','" + sheduleObj.BatchNo + "','" + sheduleObj.Status + "',1,convert(datetime,'" + sheduleObj.StartDate + "',103),convert(datetime,'" + sheduleObj.EndDate + "',103),'" + sheduleObj.LoginBy + "',GETDATE() )";
                command1.ExecuteNonQuery();

                command.CommandText = @"SELECT TOP(1)[ID] FROM [TBL_SHEDULE_ENTRY_MST] ORDER BY ID DESC";
                int ID = Convert.ToInt32(command.ExecuteScalar());

                foreach (DataRow drsh in dt.Rows)
                {
                    if (drsh["DaysID"].ToString() != "")
                    {
                    string StartDateTime =
                        //DateTime.Now.ToString("MM/dd/yyyy")
                        //+ " " + 
                        drsh["StartTime"].ToString()
                    +" " + drsh["StartAmPm"].ToString();
                        string EndDateTime = 
                            //DateTime.Now.ToString("MM/dd/yyyy")
                            //+ " " + 
                    drsh["EndtTime"].ToString()
                        +" " + drsh["EndAmPm"].ToString();
                        command.CommandText = @"INSERT INTO [TBL_SHEDULE_ENTRY_DTL]
           ([MSTID],[Days],[StartTime],[EndtTime],[RoomNo],[Flag])
                            VALUES
                        ('" + ID + "','" + drsh["DaysID"].ToString() + "',convert(datetime,'" + StartDateTime +
                                              "',103),convert(datetime,'" + EndDateTime + "',103),'" +
                                              drsh["RoomNo"].ToString() + "','1')";
                        command.ExecuteNonQuery();

                    }
                }
            }
            transection.Commit();
            connection.Close();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}