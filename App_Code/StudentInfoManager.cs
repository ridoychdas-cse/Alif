using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using KHSC;



using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Web.DynamicData;
using System.Web.Management;

/// <summary>
/// Summary description for StudentInfoManager
/// </summary>
public class StudentInfoManager
{
	public StudentInfoManager()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public int MstId()
    {
        var connection=new SqlConnection(DataManager.OraConnString());
        connection.Open();
        string query = @"SELECT TOP(1)[ID] FROM StudetnInfoMst ORDER BY ID DESC";
        var command=new SqlCommand(query,connection);
        int Id = Convert.ToInt32(command.ExecuteScalar());
        return Id;
    }

    public int SaveMst(StudentInfoModel student)
    {
        var connection =new SqlConnection(DataManager.OraConnString());
        connection.Open();
       // string variables =
        //    "Sl_Id,NID,StdName,StdPhoneNo,FthName,FthPhoneNo,MthName,MthPhoneNo,DBO,Email,Nationality,Religion,MaritalStatus,LastEducation,Bord,Result,PresentAddress,ParVill,ParPO,ParPS,ParDistrict,AddDate";
        string variables = "Sl_Id,NID,StdName,StdPhoneNo,FthName,FthPhoneNo,MthName,MthPhoneNo,DBO,Email,Nationality,Religion,MaritalStatus,LastEducation,Bord,Result,PresentAddress,ParVill,ParPO,ParPS,ParDistrict,AddDate";

        
        
        string valus = "  '" + student.SlNo + "','" + student.NID + "','" + student.Name + "','" +
                       student.StdPhone + "','" + student.FthName + "','" + student.FthPhone + "','" +
                       student.MthName + "','" + student.MthPhone + "',convert(datetime,nullif('" + student.DBO + "',''),103),'" + student.Email +
                       "','" + student.Nationality + "','" + student.Religion + "','" + student.MaritalStatus +
                       "','" + student.LastEducation + "','"+student.Bord+"','"+student.Result+"','" + student.PresentAddress + "','" + student.ParVill +
                       "','" + student.ParPO + "','" + student.ParPS + "','" + student.ParDis + "',  GETDate() ";
        string query = "";
        if (student.StdPhoto != null)
        {
            if (student.StdPhoto.Length > 0)
            {
                variables = variables + ",StdPhoto";
                valus = valus + ",@img";
            }
        }

        query = "Insert Into StudetnInfoMst (" + variables + ") Values (" + valus + ")";
        var command=new SqlCommand(query,connection);
        SqlParameter img = new SqlParameter();
        img.SqlDbType = SqlDbType.VarBinary;
        img.ParameterName = "img";
        img.Value = student.StdPhoto;
        command.Parameters.Add(img);
        if (student.StdPhoto == null)
        {
            command.Parameters.Remove(img);
        }
        else
        {
            if (student.StdPhoto.Length == 0)
            {
                command.Parameters.Remove(img);
            }
        }

   return  command.ExecuteNonQuery();
    }


    public int Save(StudentInfoModel student)
    {
       var connection=new SqlConnection(DataManager.OraConnString());
       SqlTransaction transaction;
        try
        {
            connection.Open();
            transaction = connection.BeginTransaction();
            var command = new SqlCommand();
            command.Connection = connection;
            command.Transaction = transaction;
            var command1=new SqlCommand();
            command1.Connection = connection;
            command1.Transaction = transaction;
            var Command2=new SqlCommand();
            Command2.Connection = connection;
            Command2.Transaction = transaction;

      command.CommandText = @"SELECT TOP(1)[ID] FROM StudetnInfoMst ORDER BY ID DESC";
      int ID = Convert.ToInt32(command.ExecuteScalar());
      command.CommandText=" INSERT INTO dbo.StudentInfoDtl (MstId,CourseName,TrainerName,BatchNo,ClassTime,APM,AdmissionDate,CourseFee,Waiver,DisCount,TotalAmount,PayAmount,AddmissionYear,CertificationDate) VALUES('"+ID+"','"+student.CourseName+"','"+student.TrainerName+"','"+student.BatchNo+"','"+student.ClassTime+"','"+student.APM+"', convert(datetime,nullif('"+student.AdmissionDate+"',''),103),'"+student.CourseFee+"','"+student.Waiver+"','"+student.DisCount+"','"+student.TotalAmount+"','"+student.PayAmount+"','"+student.AddmissionYear+"',convert(datetime,nullif('"+student.CertificationDate+"',''),103) ) ";
      command.ExecuteNonQuery();
      command1.CommandText =
          "insert into Day_Information(StudentID,StarDay,Sunday,MonDay,TusesDay,WednessDay,ThusDay,Friday) values('" +
          ID + "','" + student.SatDay + "','" + student.SunDay + "','" + student.MonDay + "','" + student.TuesDay +
          "','" + student.WednessDay + "','" + student.ThusDay + "','" + student.FriDay + "')";
     
       command1.ExecuteNonQuery();
      Command2.CommandText =
          "Insert Into StdPaymentDtls (StudentId,CourseFee,Waiver,Discount,TotalAmount,PaidAmount) values('" + ID + "','" + student.CourseFee + "','" + student.Waiver + "','" + student.DisCount + "','" + student.TotalAmount + "','" + student.PayAmount + "')";
      int success = Command2.ExecuteNonQuery();
    transaction.Commit();
    return success;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        finally
        {
            if (connection.State == ConnectionState.Open)
                connection.Close();
        }

    }

    public DataTable CheckId(string Id)
    {
        var connectionstring = DataManager.OraConnString();
        string query = "select Id from StudetnInfoMst where ID='" + Id + "' and DeleteDate Is null";
        var data = DataManager.ExecuteQuery(connectionstring, query, "StudetnInfoMst");
        return data;
    }

    public DataTable SearchId(string item)
    {
        var ConnectionString = DataManager.OraConnString();
        string query = "select t1.Id, Sl_Id,NID,StdName,StdPhoneNo,FthName,FthPhoneNo,FthPhoneNo,MthName, MthPhoneNo,CONVERT(nvarchar,nullif(DBO,''),103)as DBO ,Email,Nationality,Religion,MaritalStatus,LastEducation,Bord,Result,PresentAddress,ParVill,ParPO, ParPS,ParDistrict,StdPhoto,CourseName,TrainerName,BatchNo,ClassTime, APM,convert(nvarchar,nullif(AdmissionDate,''),103) AS AdmissionDate,CourseFee, Waiver,DisCount,TotalAmount,PayAmount,AddmissionYear,Convert(nvarchar,nullif(CertificationDate,''),103) AS CertificationDate,StarDay,Sunday ,MonDay,TusesDay,WednessDay,ThusDay,Friday    from dbo.StudetnInfoMst t1 inner join  dbo.StudentInfoDtl t2 on t1.Id=t2.MstId inner join  dbo.Day_Information t3 on t1.Id=t3.StudentID where upper(Sl_Id +'-'+ NID+'-'+StdName+'-'+StdPhoneNo+'-'+FthName+'-'+MthName+'-'+CourseName)  = upper('" + item + "') and t1.DeleteDate IS NULL";
        var data = DataManager.ExecuteQuery(ConnectionString, query, "StudetnInfoMst");
        return data;

        //string query =
        //    "select t1.Id, Sl_Id,NID,StdName,StdPhoneNo,FthName,FthPhoneNo,FthPhoneNo,MthName, MthPhoneNo,CONVERT(nvarchar,nullif(DBO,''),103)as DBO ,Email,Nationality,Religion,MaritalStatus,LastEducation,Bord,Result,PresentAddress,ParVill,ParPO, ParPS,ParDistrict,StdPhoto,CourseName,TrainerName,BatchNo,ClassTime, APM,convert(nvarchar,nullif(AdmissionDate,''),103) AS AdmissionDate,t4.CourseFee, t4.Waiver,t4.DisCount,t4.TotalAmount,t4.PayAmount,AddmissionYear,Convert(nvarchar,nullif(CertificationDate,''),103) AS CertificationDate,StarDay,Sunday ,MonDay,TusesDay,WednessDay,ThusDay,Friday    from dbo.StudetnInfoMst t1 inner join  dbo.StudentInfoDtl t2 on t1.Id=t2.MstId inner join  dbo.Day_Information t3 on t1.Id=t3.StudentID inner join (select StudentId,CourseFee,Waiver,sum(Discount) as DisCount,((CourseFee-(CourseFee*(Waiver/100)))-(sum(Discount))) as TotalAmount,((CourseFee-(sum(Discount)))-(Sum(PaidAmount))) as Due,Sum(PaidAmount) as PayAmount from StdPaymentDtls where StudentId=71 group by StudentId,CourseFee,Waiver) t4 on  t1.Id= t4.StudentId where upper(Sl_Id +'-'+ NID+'-'+StdName+'-'+StdPhoneNo+'-'+FthName+'-'+MthName+'-'+CourseName)  = upper('02-6454564651-Mr.s-0147442331-Mr.A-SDJKLF-CSE') and t1.DeleteDate IS NULL";

    }
    public DataTable Search(string Id, string Item)
    {
        var ConnectionString = DataManager.OraConnString();
        string query =
            "select t1.Id, Sl_Id,NID,StdName,StdPhoneNo,FthName,FthPhoneNo,FthPhoneNo,MthName, MthPhoneNo,CONVERT(nvarchar,nullif(DBO,''),103)as DBO ,Email,Nationality,Religion,MaritalStatus,LastEducation,Bord,Result,PresentAddress,ParVill,ParPO, ParPS,ParDistrict,StdPhoto,CourseName,TrainerName,BatchNo,ClassTime, APM,convert(nvarchar,nullif(AdmissionDate,''),103) AS AdmissionDate,t4.CourseFee, t4.Waiver,t4.DisCount,t4.TotalAmount,t4.PayAmount,t4.Due,AddmissionYear,Convert(nvarchar,nullif(CertificationDate,''),103) AS CertificationDate,StarDay,Sunday ,MonDay,TusesDay,WednessDay,ThusDay,Friday    from dbo.StudetnInfoMst t1 inner join  dbo.StudentInfoDtl t2 on t1.Id=t2.MstId inner join  dbo.Day_Information t3 on t1.Id=t3.StudentID inner join (select StudentId,CourseFee,Waiver,sum(Discount) as DisCount,((CourseFee-(CourseFee*(Waiver/100)))-(sum(Discount))) as TotalAmount,((CourseFee-(sum(Discount)))-(Sum(PaidAmount))) as Due,Sum(PaidAmount) as PayAmount from StdPaymentDtls where StudentId='" +
            Id +
            "' group by StudentId,CourseFee,Waiver) t4 on  t1.Id= t4.StudentId where upper(Sl_Id +'-'+ NID+'-'+StdName+'-'+StdPhoneNo+'-'+FthName+'-'+MthName+'-'+CourseName)  = upper('" +
            Item + "') and t1.DeleteDate IS NULL";
           

        var data = DataManager.ExecuteQuery(ConnectionString, query, "StudetnInfoMst");
        return data;


    }


    public byte[] GetStudentPhoto(string p)
    {
        try
        {
          var connection=new SqlConnection(DataManager.OraConnString());
            connection.Open();
            string selectQuery = @"select ISNULL(StdPhoto,0) from StudetnInfoMst where Sl_ID='" + p + "'";
            SqlCommand command = new SqlCommand(selectQuery, connection);
            return (byte[])command.ExecuteScalar();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
       
    }

    public int UpdateMst(StudentInfoModel student, string MstId)
    {
        var connection = new SqlConnection(DataManager.OraConnString());
        connection.Open();
        string variables =
            "Update StudetnInfoMst set NID ='" + student.NID + "',StdName ='" + student.Name + "',StdPhoneNo ='" + student.StdPhone + "',FthName ='" + student.FthName + "',FthPhoneNo ='" + student.FthPhone + "',MthName ='" + student.MthName + "',MthPhoneNo ='" + student.MthPhone + "',DBO = convert(datetime,nullif('" + student.DBO + "',''),103),Email ='" + student.Email + "',Nationality ='" + student.Nationality + "',Religion ='" + student.Religion + "',MaritalStatus ='" + student.MaritalStatus + "',LastEducation ='" + student.LastEducation + "',Bord='"+student.Bord+"',Result='"+student.Result+"',PresentAddress ='" + student.PresentAddress + "',ParVill ='" + student.ParVill + "',ParPO ='" + student.ParPO + "',ParPS ='" + student.ParPS + "',ParDistrict ='" + student.ParDis + "',UpdateDate=GETDATE()";
        
        string query = "";
        if (student.StdPhoto != null)
        {
            if (student.StdPhoto.Length > 0)
            {
                variables = variables + ",StdPhoto = @img";
              
            }
        }

        string WHERE = "Where Id='" + MstId + "'";

        query = "" + variables + " "+WHERE+"";
        var command = new SqlCommand(query, connection);
        SqlParameter img = new SqlParameter();
        img.SqlDbType = SqlDbType.VarBinary;
        img.ParameterName = "img";
        img.Value = student.StdPhoto;
        command.Parameters.Add(img);
        if (student.StdPhoto == null)
        {
            command.Parameters.Remove(img);
        }
        else
        {
            if (student.StdPhoto.Length == 0)
            {
                command.Parameters.Remove(img);
            }
        }

        return command.ExecuteNonQuery();
    }



    public int Update(StudentInfoModel student, string MstId)
    {
        
    var connection=new SqlConnection(DataManager.OraConnString());
       SqlTransaction transaction;
        try
        {
            connection.Open();
            transaction = connection.BeginTransaction();
            var command = new SqlCommand();
            command.Connection = connection;
            command.Transaction = transaction;
            var command1=new SqlCommand();
            command1.Connection = connection;
            command1.Transaction = transaction;

            command.CommandText = " update StudentInfoDtl set CourseName ='"+student.CourseName+"',TrainerName ='"+student.TrainerName+"',BatchNo ='"+student.BatchNo+"',ClassTime ='"+student.ClassTime+"',APM ='"+student.APM+"',AdmissionDate =convert(datetime,nullif('"+student.AdmissionDate+"',''),103),CourseFee ='"+student.CourseFee+"',Waiver ='"+student.Waiver+"',DisCount = '"+student.DisCount+"',TotalAmount ='"+student.TotalAmount+"',AddmissionYear ='"+student.AddmissionYear+"',CertificationDate =convert(datetime,nullif('"+student.CertificationDate+"',''),103) where MstId ='"+MstId+"' ";
      command.ExecuteNonQuery();
      command1.CommandText =
          "update Day_Information set StarDay ='"+student.SatDay+"',Sunday ='"+student.SunDay+"',MonDay ='"+student.MonDay+"',TusesDay ='"+student.TuesDay+"',WednessDay ='"+student.WednessDay+"',ThusDay ='"+student.ThusDay+"',Friday ='"+student.FriDay+"' where StudentID ='"+MstId+"'";

      int success = command1.ExecuteNonQuery();
    transaction.Commit();
      return success;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        finally
        {
            if (connection.State == ConnectionState.Open)
                connection.Close();
        }

    
    }

    public int DeleteMst(string MstId)
    {
        var connection = new SqlConnection(DataManager.OraConnString());
        SqlTransaction transaction;
        try
        {
            connection.Open();
            transaction = connection.BeginTransaction();
            var command = new SqlCommand();
            command.Connection = connection;
            command.Transaction = transaction;
            var command1 = new SqlCommand();
            command1.Connection = connection;
            command1.Transaction = transaction;
            var command0 = new SqlCommand();
            command0.Connection = connection;
            command0.Transaction = transaction;

            command0.CommandText = "Update StudetnInfoMst set DeleteDate=GETDATE() where Id='" + MstId + "'";
            command0.ExecuteNonQuery();
            command.CommandText = " update StudentInfoDtl set DeleteDate=Getdate() where MstId ='" + MstId + "' ";
            command.ExecuteNonQuery();
            command1.CommandText =  "update Day_Information set DeleteDate=GetDate() where StudentID ='" + MstId + "'";

            int success = command1.ExecuteNonQuery();
            transaction.Commit();
            return success;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        finally
        {
            if (connection.State == ConnectionState.Open)
                connection.Close();
        }
      
    }

   
}