using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Data;
using KHSC;
using KHSC.DAO.Others;


namespace KHSC.Gateway.Others
{
public class PromoteGateway

{
    SqlConnection connection = new SqlConnection(DataManager.OraConnString());
    SqlTransaction transaction;


    internal DataTable GetShowStudentCurrentInformation(string Year, string Class, string Section, string Version, string Shift)
    {
        try
        {
            connection.Open();
            string selectQuery = @"SELECT [student_id]
,(Select b.f_name+' '+b.m_name+' '+b.l_name FROM student_info b Where b.student_id=a.student_id)As [Student_Name]
  ,a.std_roll
   FROM std_current_status a  WHERE a.class_id='" + Class + "' AND a.sect='" + Section + "' AND a.class_year='" + Year + "'  AND a.[version]='" + Version + "' AND a.shift='" + Shift + "' order by convert(int, a.std_roll )  ";
            SqlDataAdapter da = new SqlDataAdapter(selectQuery, connection);
            DataSet ds = new DataSet();
            da.Fill(ds, "std_current_status");
            DataTable table = ds.Tables["std_current_status"];
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

    internal void ArchiveStudentPromoteInformation(PromoteStudent aPromoteStudentobj, List<PromoteStudent> PromoteStudentlist)
    {
        try
        {
            connection.Open();


            foreach (PromoteStudent PrestPromor in PromoteStudentlist)
            {

                string selectQuery = @"INSERT INTO [tbl_student_previous_history]
           ([spi_student_id]
           ,[spi_previous_roll]
           ,[spi_class]
           ,[spi_year]
           ,[spi_section]
           ,[spi_version]
           ,[spi_pomot_date]
           ,[spi_pomot_by]
           ,[Tc_flag]
           ,[shift])
     VALUES
           ('" + PrestPromor.StudentId + "','" + PrestPromor.PreviousRoll + "','" + aPromoteStudentobj.PreviousClass + "','" + aPromoteStudentobj.PreviousYear + "','" + aPromoteStudentobj.PreviousSection + "','" + aPromoteStudentobj.PreviousVersion + "',GETDATE(),'" + PrestPromor.User + "','0','" + aPromoteStudentobj.PreviousShift + "')";

                SqlCommand command = new SqlCommand(selectQuery, connection);
                command.ExecuteNonQuery();

                string SeclectQuery1 = @" DELETE FROM  [std_current_status] WHERE [student_id] = '" + PrestPromor.StudentId + "'";
  

                SqlCommand command1 = new SqlCommand(SeclectQuery1, connection);
                command1.ExecuteNonQuery();

            }


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

    internal void SaveStudentPromoteInformation(PromoteStudent aPromoteStudentobj, List<PromoteStudent> PromoteStudentlist)
    {
        try
        {
            connection.Open();


            foreach (PromoteStudent PrestPromor in PromoteStudentlist)
            {

                string selectQuery = @"INSERT INTO [tbl_student_previous_history]
           ([spi_student_id]
           ,[spi_previous_roll]
           ,[spi_class]
           ,[spi_year]
           ,[spi_section]
           ,[spi_version]
           ,[spi_pomot_date]
           ,[spi_pomot_by]
           ,[Tc_flag]
           ,[shift])
     VALUES
           ('" + PrestPromor.StudentId + "','" + PrestPromor.PreviousRoll + "','" + aPromoteStudentobj.PreviousClass + "','" + aPromoteStudentobj.PreviousYear + "','" + aPromoteStudentobj.PreviousSection + "','" + aPromoteStudentobj.PreviousVersion + "',GETDATE(),'" + PrestPromor.User + "','0','" + aPromoteStudentobj.PreviousShift + "')";

                SqlCommand command = new SqlCommand(selectQuery, connection);
                command.ExecuteNonQuery();

                string SeclectQuery1 = @"UPDATE [std_current_status]
   SET  [class_id] = '" + aPromoteStudentobj.CurrentClass + "',[class_year] = '" + aPromoteStudentobj.CurrentYear + "',[shift] = '" + aPromoteStudentobj.CurrentShift + "' ,[sect] = '" + aPromoteStudentobj.CurrentSection + "',[version] ='" + aPromoteStudentobj.CurrentVersion + "' ,[class_start]=convert(datetime,nullif('" + aPromoteStudentobj.CurrentClassStartDate + "',''),103) ,[std_roll] ='" + PrestPromor.CurrrentRoll + "',[std_admission_date] =convert(datetime,nullif( Getdate(),''),103) ,[previous_last_class] = '" + aPromoteStudentobj.PreviousClass + "' ,[group_name] ='' WHERE [student_id] = '" + PrestPromor.StudentId + "'";

                SqlCommand command1 = new SqlCommand(SeclectQuery1, connection);
                command1.ExecuteNonQuery();
            }


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

    internal void SaveStudentGroupPromoteInformation(PromoteStudent aPromoteStudentobj, List<PromoteStudent> PromoteStudentlist)
    {
        try
        {
            connection.Open();


            foreach (PromoteStudent PrestPromor in PromoteStudentlist)
            {

                string selectQuery = @"INSERT INTO [tbl_student_previous_history]
           ([spi_student_id]
           ,[spi_previous_roll]
           ,[spi_class]
           ,[spi_year]
           ,[spi_section]
           ,[spi_version]
           ,[spi_pomot_date]
           ,[spi_pomot_by]
           ,[Tc_flag]
           ,[shift]
           ,[group_name])
     VALUES
           ('" + PrestPromor.StudentId + "','" + PrestPromor.PreviousRoll + "','" + aPromoteStudentobj.PreviousClass + "','" + aPromoteStudentobj.PreviousYear + "','" + aPromoteStudentobj.PreviousSection + "','" + aPromoteStudentobj.PreviousVersion + "',GETDATE(),'" + PrestPromor.User + "','0','" + aPromoteStudentobj.PreviousShift + "','" + aPromoteStudentobj.PreviousGroup + "')";

                SqlCommand command = new SqlCommand(selectQuery, connection);
                command.ExecuteNonQuery();

                string SeclectQuery1 = @"UPDATE [std_current_status]
   SET  [class_id] = '" + aPromoteStudentobj.CurrentClass + "',[class_year] = '" + aPromoteStudentobj.CurrentYear + "',[shift] = '" + aPromoteStudentobj.CurrentShift + "' ,[sect] = '" + aPromoteStudentobj.CurrentSection + "',[version] ='" + aPromoteStudentobj.CurrentVersion + "' ,[class_start]=convert(datetime,nullif('" + aPromoteStudentobj.CurrentClassStartDate + "',''),103) ,[std_roll] ='" + PrestPromor.CurrrentRoll + "',[std_admission_date] =convert(datetime,nullif( Getdate(),''),103) ,[previous_last_class] = '" + aPromoteStudentobj.PreviousClass + "' ,[group_name] ='" + aPromoteStudentobj.CurrentGroup + "' WHERE [student_id] = '" + PrestPromor.StudentId + "'";

                SqlCommand command1 = new SqlCommand(SeclectQuery1, connection);
                command1.ExecuteNonQuery();
            }


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

    internal void SaveRetrieveStudentPromoteInformation(PromoteStudent aPromoteStudentobj, List<PromoteStudent> PromoteStudentlist)
    {
        try
        {
            connection.Open();


            foreach (PromoteStudent PrestPromor in PromoteStudentlist)
            {

                string selectQuery = @"INSERT INTO [std_current_status]
           ([student_id]
           ,[class_id]
           ,[class_year]
           ,[shift]
           ,[sect]
           ,[version]
           ,[class_start]
           ,[std_roll]
           ,[std_admission_date]
           ,[previous_last_class]
            )
     VALUES
           ('" + PrestPromor.StudentId + "','" + aPromoteStudentobj.CurrentClass + "','" + aPromoteStudentobj.CurrentYear + "','"+aPromoteStudentobj.CurrentShift+"','" + aPromoteStudentobj.CurrentSection + "','"+aPromoteStudentobj.CurrentVersion+"',convert(datetime,nullif('" + aPromoteStudentobj.CurrentClassStartDate + "',''),103),'" + PrestPromor.CurrrentRoll + "',GETDATE(),'" + aPromoteStudentobj.PreviousClass + "')";

                SqlCommand command = new SqlCommand(selectQuery, connection);
                command.ExecuteNonQuery();

                string SeclectQuery1 = @" DELETE FROM  [tbl_student_previous_history] WHERE [spi_student_id] = '" + PrestPromor.StudentId + "'";

                SqlCommand command1 = new SqlCommand(SeclectQuery1, connection);
                command1.ExecuteNonQuery();
            }


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

    internal DataTable GetShowStudentArchiveInformation(string Year, string Class, string Section, string Version, string Shift)
    {
        try
        {
            connection.Open();
            string selectQuery = @"SELECT a.spi_student_id
,(Select b.f_name+' '+b.m_name+' '+b.l_name FROM student_info b Where b.student_id=a.spi_student_id)As [Student_Name]
  ,a.spi_previous_roll
   FROM tbl_student_previous_history a  WHERE  a.spi_class='" + Class + "' AND a.spi_section ='" + Section + "' AND a.spi_year='" + Year + "' AND a.spi_version='" + Version + "' AND a.shift='" + Shift + "'  order by  CONVERT(int, a.spi_previous_roll) asc";
            SqlDataAdapter da = new SqlDataAdapter(selectQuery, connection);
            DataSet ds = new DataSet();
            da.Fill(ds, "tbl_student_previous_history");
            DataTable table = ds.Tables["tbl_student_previous_history"];
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

    internal void SaveRetrieveStudentGroupPromoteInformation(PromoteStudent aPromoteStudentobj, List<PromoteStudent> PromoteStudentlist)
    {
        try
        {
            connection.Open();


            foreach (PromoteStudent PrestPromor in PromoteStudentlist)
            {

                string selectQuery = @"INSERT INTO [std_current_status]
           ([student_id]
           ,[class_id]
           ,[class_year]
           ,[shift]
           ,[sect]
           ,[version]
           ,[class_start]
           ,[std_roll]
           ,[std_admission_date]
           ,[previous_last_class]
           ,[group_name])
     VALUES
           ('" + PrestPromor.StudentId + "','" + aPromoteStudentobj.CurrentClass + "','" + aPromoteStudentobj.CurrentYear + "','" + aPromoteStudentobj.CurrentShift + "','" + aPromoteStudentobj.CurrentSection + "','" + aPromoteStudentobj.CurrentVersion + "',convert(datetime,nullif('" + aPromoteStudentobj.CurrentClassStartDate + "',''),103),'" + PrestPromor.CurrrentRoll + "',GETDATE(),'" + aPromoteStudentobj.PreviousClass + "','" + aPromoteStudentobj.CurrentGroup + "')";

                SqlCommand command = new SqlCommand(selectQuery, connection);
                command.ExecuteNonQuery();

                string SeclectQuery1 = @" DELETE FROM  [tbl_student_previous_history] WHERE [spi_student_id] = '" + PrestPromor.StudentId + "'";

                SqlCommand command1 = new SqlCommand(SeclectQuery1, connection);
                command1.ExecuteNonQuery();
            }


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
}
}