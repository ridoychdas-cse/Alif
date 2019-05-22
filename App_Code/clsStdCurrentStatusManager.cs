using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Summary description for clsStdCurrentStatusManager
/// </summary>
/// 
namespace KHSC
{
    public class clsStdCurrentStatusManager
    {
        public static DataTable GetStdCurrentStatuss()
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = "select student_id,max(class_id) class_id,max(class_year) class_year,shift,sect,version,convert(varchar,max(class_start),103) class_start from std_current_status group by student_id,shift,sect order by 3 desc,2 desc";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "CountryInfo");
            return dt;
        }
        public static DataTable GetCourseInfo(string CourseID)
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);


            string query = @"Select ci.CourseName,ci.ID,ci.CourseFee,ci.Doscount as Discount,convert(nvarchar,sm.SheduleStartDate,103)as SheduleStartDate,sm.ID as SheduleID,ci.ID as CourseID,fc.ID as FacultyID,fc.FacultyName,sm.BatchNo,sm.Year from tbl_Course_Name  ci
 inner join CourseTrac ct on ct.id=ci.TracID 
 inner join TBL_SHEDULE_ENTRY_MST sm on sm.CourseID=ci.ID
 inner join Faculty_Info fc on fc.ID=sm.FacultyID  where sm.CourseID='" + CourseID + "'";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "tbl_Course_Name");
            return dt;
        }
        public static DataTable GetStdCurrentStatusByYearByClass(string cls,string year,string shft,string sect)
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = "select a.student_id,dbo.initcap(rtrim(ltrim(f_name+' '+m_name+' '+l_name))) std_name, a.class_id, a.class_year,'' std_marks,shift,sect,convert(varchar,class_start,103) class_start from std_current_status a,student_info b " +
                " where a.student_id=b.subject_id and a.class_year=convert(numeric,nullif('" + year + "','')) and a.class_id=convert(numeric,nullif('" + cls + "','')) " +
                " and a.shift='" + shft + "' and a.sect='" + sect + "' " +
                " and a.student_id not in (select student_id from std_current_status where class_id=convert(numeric,nullif('" + cls + "',''))+1 and class_year=convert(numeric,nullif('" + year + "',''))+1 and shift='" + shft + "' and sect='" + sect + "') " +
                " order by 3 desc,2 desc";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "CountryInfo");
            return dt;
        }
        public static object GetCourseName(string TracID)
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string per = "";
            if (!string.IsNullOrEmpty(TracID))
            {
                per = " where ct.ID='" + TracID + "'";
            }

            string query = @"Select ci.CourseName,ci.ID,ci.CourseFee,ci.Doscount as Discount from tbl_Course_Name  ci inner join CourseTrac ct on ct.id=ci.TracID  " + per + " ";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "tbl_Course_Name");
            return dt;

        }
        public static void UpdateCurrentStatus(clsStdCurrentStatus st)
        {
            String connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string selectQuery = @"UPDATE [std_current_status]
   SET  [class_id] ='" + st.ClassId + "',[class_year] ='" + st.ClassYear + "',[sect] ='" + st.Sect + "',[DeptId] ='" + st.DeptId + "',[version] ='" + st.version + "',shift='" + st.Shift + "',[group_name] = '" + st.Group + "',[class_start]= convert(datetime,nullif('" + st.ClassStart + "',''),103),[std_roll] ='" + st.Roll + "',[std_admission_date]=convert(datetime,nullif('" + st.AdmissionDt + "',''),103),PayAmount='" + st.TotalAmount + "',PaidAmount='" + st.PayAmount + "',DueAmount='" + st.DueAmount + "',SheduleStart=convert(date,'" + st.ScheduleTime + "',103),ShehuleEnd=convert(date,'" + st.ScheduleTimeEnd + "',103),ClassTime='" + st.ClassTime + "'  WHERE [student_id] ='" + st.StudentId + "'";
             DataManager.ExecuteNonQuery(connectionString, selectQuery);
            sqlCon.Close();
        }

        public static void CreatePreviousStatus(clsStdCurrentStatus st)
        {
            String connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string selectQuery = @"INSERT INTO [tbl_student_previous_history]
           ([spi_student_id]
           ,[spi_previous_roll]
           ,[spi_class]
           ,[spi_year]
           ,[spi_section]
           ,[spi_version]
           ,[spi_pomot_date]
           ,[spi_pomot_by],Tc_flag)
     VALUES
           ('" + st.StudentId + "','" + st.Roll + "','" + st.ClassId + "','" + st.ClassYear + "','" + st.Sect + "','" + st.version + "',GETDATE(),'Admin','1')";
            DataManager.ExecuteNonQuery(connectionString, selectQuery);


            string selectQuery1 = @"INSERT INTO [TC_Info]
           ([Student_Id])
            
     VALUES
           ('" + st.StudentId + "')";
            DataManager.ExecuteNonQuery(connectionString, selectQuery1);

            string query = " delete from std_current_status where student_id='" +st.StudentId + "' ";

            DataManager.ExecuteNonQuery(connectionString, query);

            sqlCon.Close();
        }
        
        public static void CreateCurrentStatus(clsStdCurrentStatus cnt)
        {
            //String connectionString = DataManager.OraConnString();
            //SqlConnection sqlCon = new SqlConnection(connectionString);

            //string query = "insert into std_current_status(student_id,class_id,class_year,shift,DeptId,sect,version,group_name,class_start,std_roll,std_admission_date) values ('" + cnt.StudentId + "','" + cnt.ClassId + "','" + cnt.ClassYear + "','" + cnt.Shift + "','" + cnt.DeptId + "','" + cnt.Sect + "','" + cnt.version + "','" + cnt.Group + "', convert(datetime,nullif('" + cnt.ClassStart + "',''),103),'" + cnt.Roll + "', convert(datetime,nullif('" + cnt.AdmissionDt + "',''),103))";
           
            //DataManager.ExecuteNonQuery(connectionString, query);
            //sqlCon.Close();
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

                command.CommandText = @"SELECT TOP(1)[ID] FROM [student_info] ORDER BY ID DESC";
                int ID = Convert.ToInt32(command.ExecuteScalar());


                string ClassTime = cnt.ClassTime + ":" + cnt.APMTime;

                command1.CommandText = @"INSERT INTO [std_current_status]
           ([student_id],[tracId],[AddmisionYear],[CourseId],[CourseFee],[ScheduleId],[BatchNo],[std_admission_date],[PreviousCourseID],[EntryBy],[EntryDate],[TrainerName],[Discount],[Waiver],[PayAmount],[PaidAmount],[DueAmount],Pay_Date,SheduleStart,ShehuleEnd,ClassTime,CertificateDate)
values
('" + ID + "','" + cnt.TracID + "','" + cnt.AddmissionYear + "','" + cnt.CourseID + "','" + cnt.CourseFee + "','" + cnt.ScheduleId + "','" + cnt.BatchNo + "',convert(date,'" + cnt.AddmissionDate + "',103),'" + cnt.PreviousCourseID + "','" + cnt.LogineBy + "',GETDATE(),'" + cnt.FacultyID + "','" + cnt.Discount + "','" + cnt.Waiver + "','" + cnt.TotalAmount + "','" + cnt.PayAmount + "','" + cnt.DueAmount + "',GETDATE(),convert(date,'" + cnt.ScheduleTime + "',103),convert(date,'" + cnt.ScheduleTimeEnd + "',103),'" + ClassTime + "',convert(date,'" + cnt.CertificateDate + "',103) )";

                command1.ExecuteNonQuery();

                command.CommandText = @"INSERT INTO [Day_Information]
           ([StudentID]
           ,[StarDay]
           ,[Sunday]
           ,[MonDay]
           ,[TusesDay]
           ,[WednessDay]
           ,[ThusDay]
           ,[Friday])
     VALUES
           ('" + ID + "','" + cnt.SatDay + "','" + cnt.SunDay + "','" + cnt.MonDay + "','" + cnt.TuesDay + "','" + cnt.WednessDay + "','" + cnt.ThusDay + "','" + cnt.FriDay + "')";
                command.ExecuteNonQuery();

                transection.Commit();
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
        public static void CreateCurrentStatusUpdate(clsStdCurrentStatus st)
        {
            String connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);

            string query = " Update std_current_status set PaidAmount='" + st.PaidAmount + "',Discount='" + st.TotalDiscount + "',DueAmount='" + st.CurrentDue + "',UpdateBy='" + st.LogineBy + "',Pay_Date=convert(date,'" + st.DateUpdate + "',103),UpdateDate=GETDATE() where student_id='" + st.StudentId + "'";

            DataManager.ExecuteNonQuery(connectionString, query);
            sqlCon.Close();
        }
        public static void DeleteCurrentStatus(clsStdCurrentStatus std)
        {
            String connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);

            string query = " Update std_current_status set DeleteBy='" + std.LogineBy + "',DeleteDate=GETDATE() where student_id='" + std.StudentId + "'";

            DataManager.ExecuteNonQuery(connectionString, query);
            sqlCon.Close();
        }

       
        //public static void DeleteCurrentStatus(string StudentId)
        //{
        //    String connectionString = DataManager.OraConnString();
        //    SqlConnection sqlCon = new SqlConnection(connectionString);

        //    string query = " delete from std_current_status where student_id='" + StudentId + "'";

        //    DataManager.ExecuteNonQuery(connectionString, query);
        //    sqlCon.Close();
        //}

        public static clsStdCurrentStatus GetStdCurrentStatus(string si)
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = @"select sc.student_id,st.f_name as name,sc.std_roll,max(sc.class_id) class_id,max(sc.class_year) class_year,sc.shift,sc.sect,sc.[version],sc.group_name,ci.class_name,si.sec_name,sh.shift_name,vi.version_name, convert(varchar,max(sc.class_start),103) class_start from std_current_status sc
inner join class_info ci on sc.class_id=ci.class_id
inner join section_info si on sc.sect=si.sec_id
inner join shift_info sh on sc.shift=sh.shift_id
inner join version_info vi on sc.[version]=vi.version_id
inner join student_info st on sc.student_id=st.student_id
where sc.student_id='"+si+"' group by sc.student_id,st.f_name,sc.std_roll,sc.shift,sc.sect,sc.[version],sc.group_name,ci.class_name,si.sec_name,sh.shift_name,vi.version_name";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Country");
            sqlCon.Close();
            if (dt.Rows.Count == 0)
            {
                return null;
            }
            return new clsStdCurrentStatus(dt.Rows[0]);
        }
        public static clsStdCurrentStatus GetStdCurrentSts(string si)
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = "select student_id,max(class_id) class_id,max(class_year) class_year,shift,sect,convert(varchar,max(class_start),103) class_start from std_current_status where student_id='" + si + "' group by student_id,shift,sect ";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Country");
            sqlCon.Close();
            if (dt.Rows.Count == 0)
            {
                return null;
            }
            return new clsStdCurrentStatus(dt.Rows[0]);
        }

        public static clsStdCurrentStatus GetAllStdCurrentStatusForSpecificStd(string studentId)
        {
             string connectionString = DataManager.OraConnString();
                SqlConnection sqlCon = new SqlConnection(connectionString);
            try
            {
                sqlCon.Open();
                string selectQuery = @"SELECT [tracId]
      ,[AddmisionYear]
      ,[CourseId]
      ,[CourseFee]
      ,[ScheduleId]
      ,[BatchNo]
      ,a.[std_admission_date]
      ,[PreviousCourseID]     
      ,[TrainerName]
      ,[Discount]
      ,[Waiver]
      ,[PayAmount]
      ,[PaidAmount]
      ,[DueAmount]
  FROM [std_current_status] a 
  inner join student_info si on si.ID=a.student_id WHERE si.[student_id]='" + studentId + "'";
                SqlCommand cmd = new SqlCommand(selectQuery,sqlCon);
                SqlDataReader reader = cmd.ExecuteReader();
                clsStdCurrentStatus aclsStdCurrentStatusObj = new clsStdCurrentStatus();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        aclsStdCurrentStatusObj.tracId = reader[0].ToString();
                        aclsStdCurrentStatusObj.AddmissionYear = reader[1].ToString();
                        aclsStdCurrentStatusObj.CourseID = reader[2].ToString();
                        aclsStdCurrentStatusObj.CourseFee = reader[3].ToString();
                        aclsStdCurrentStatusObj.ScheduleId = reader[4].ToString();
                        aclsStdCurrentStatusObj.BatchNo = reader[5].ToString();
                        aclsStdCurrentStatusObj.AddmissionDate = reader[6].ToString();
                        aclsStdCurrentStatusObj.PreviousCourseID = reader[7].ToString();
                        aclsStdCurrentStatusObj.FacultyID = reader[8].ToString();
                        aclsStdCurrentStatusObj.Discount = reader[9].ToString();
                        aclsStdCurrentStatusObj.Waiver = reader[10].ToString();
                        aclsStdCurrentStatusObj.TotalAmount = reader[11].ToString();
                        aclsStdCurrentStatusObj.PayAmount = reader[12].ToString();
                        aclsStdCurrentStatusObj.DueAmount = reader[13].ToString();

                    }
                }
                return aclsStdCurrentStatusObj;
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

        public static StudentFee GetAllCurrentStatusForSpecificStudent(string studentId)
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            try
            {
                sqlCon.Open();
                string selectQuery = @"SELECT  [f_name]+' '+[m_name]+' '+[l_name] As Student_Name
      ,(SELECT RTRIM(LTRIM(Replace([class_id],' ',''))) FROM dbo.std_current_status WHERE [student_id]=[student_id]) AS Class_Name
      ,(SELECT [class_year] FROM dbo.std_current_status WHERE [student_id]=[student_id]) AS Class_year
      ,(SELECT [sect] FROM dbo.std_current_status WHERE [student_id]=[student_id]) AS Class_Section
      ,(SELECT [std_roll] FROM dbo.std_current_status WHERE [student_id]=[student_id]) AS Class_Roll
  FROM [student_info] where [student_id]='" + studentId + "'";
                SqlCommand cmd = new SqlCommand(selectQuery, sqlCon);
                SqlDataReader reader = cmd.ExecuteReader();
                StudentFee aStudentFeeObj = new StudentFee();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        aStudentFeeObj.Name = reader[0].ToString();
                        aStudentFeeObj.ClassId = reader[1].ToString();
                        aStudentFeeObj.ClassYear = reader[2].ToString();
                        aStudentFeeObj.Sect = reader[3].ToString();
                        aStudentFeeObj.Roll = reader[4].ToString();

                        
                    }
                }
                return aStudentFeeObj;
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

        public static DataTable GetStudentCurrentInfoSecAndRoll(string ClassId, string Section, string Roll, string Verson, string Shift)
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = @"select  a.student_id,dbo.initcap(a.f_name+' '+a.m_name+' '+a.l_name) name, b.class_id,b.sect,b.shift,b.class_year,b.student_id  
from student_info a
inner join std_current_status b on a.student_id=b.student_id where a.student_id =b.student_id and b.class_id='" + ClassId + "' and b.sect='" + Section + "' and b.std_roll='" + Roll + "' and b.[version]='" + Verson + "' and b.shift='" + Shift + "' ";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "student_info");
            return dt;
        }

        public static DataTable GetStudentCurrentInfoById(string StudentId)
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = @"select student_id,name,  class_id,class_year,sect,std_roll,[version],shift,class_name,sec_name,shift_name,version_name
from (select a.student_id,dbo.initcap(rtrim(ltrim(f_name+' '+m_name+' '+l_name))) name
,  max(a.class_id)class_id,a.sect,a.std_roll, max(a.class_year)class_year,a.[version],a.shift,c.class_name,sc.sec_name,sh.shift_name,vv.version_name
from std_current_status a inner join student_info b on a.student_id=b.student_id inner join class_info c on c.class_id=a.class_id left join section_info sc on sc.sec_id=a.sect inner join shift_info sh on sh.shift_id=a.shift left join version_info vv on vv.id=a.[version] where  a.student_id='" + StudentId + "' group by a.student_id,a.sect,a.std_roll,a.[version],a.shift,dbo.initcap(rtrim(ltrim(f_name+' '+m_name+' '+l_name))),c.class_name,sc.sec_name,sh.shift_name,vv.version_name) x";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "student_info");
            return dt;
        }

        public static DataTable GetStudentCurrentInfoSearch(string p)
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = @"select  a.student_id,dbo.initcap(a.f_name+' '+a.m_name+' '+a.l_name) name, b.class_id,b.sect,b.shift,b.class_year,b.[version],b.std_roll,c.class_name,sc.sec_name,sh.shift_name,vv.version_name
from student_info a
inner join std_current_status b on a.student_id=b.student_id inner join class_info c on c.class_id=b.class_id left join section_info sc on sc.sec_id=b.sect inner join shift_info sh on sh.shift_id=b.shift left join version_info vv on vv.id=b.[version] where upper(a.[student_id]+' - '+a.[f_name]+' - '+c.class_name+' - '+sc.sec_name+' - '+vv.version_name+' - '+b.std_roll) = upper('"+p+"')";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "student_info");
            return dt;
        }        
        public static DataTable GetShowStudentGroupWise(string StdID, string ClassID, string SecID, string ShiftID, string Version)
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string Parameter = "";
            if (StdID != "") { Parameter = "where a.student_id='" + StdID + "'"; }
            else { Parameter = "where b.class_id='" + ClassID + "' and b.sect='" + SecID + "' and b.shift='" + ShiftID + "' and b.[version]='" + Version + "'"; }
            string query = @"select  a.student_id,dbo.initcap(a.f_name+' '+a.m_name+' '+a.l_name) name, b.class_id,b.sect,b.shift,b.class_year,b.[version],b.std_roll,c.class_name,sc.sec_name,sh.shift_name,vv.version_name,b.group_name
from student_info a
inner join std_current_status b on a.student_id=b.student_id inner join class_info c on c.class_id=b.class_id left join section_info sc on sc.sec_id=b.sect inner join shift_info sh on sh.shift_id=b.shift left join version_info vv on vv.id=b.[version] " + Parameter;
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "student_info");
            return dt;
        }
        public static DataTable getOldStudentInfoStudentInfo(string StudentId, string ClassId, string SectID, string ShiftID, string VersionID, string YearID)
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string Parameter = "";
            if (StudentId != "") { Parameter = "where  a.spi_student_id='" + StudentId + "' and a.spi_class='" + ClassId + "' and a.spi_section='" + SectID + "' and a.shift='" + ShiftID + "' and a.spi_version='" + VersionID + "' and a.spi_year='" + YearID + "'"; }
            else { Parameter = "where a.spi_class='" + ClassId + "' and a.spi_section='" + SectID + "' and a.shift='" + ShiftID + "' and a.spi_version='" + VersionID + "' and a.spi_year='" + YearID + "'"; }
            string query = @"select a.spi_student_id as [Id],dbo.initcap(rtrim(ltrim(f_name+' '+m_name+' '+l_name))) AS [Name]
,  a.spi_class AS [ClassID],a.spi_section AS SectionID,sc.sec_name AS Section,a.spi_previous_roll AS [STUDENT ROLL], a.spi_year AS class_year,a.spi_version AS [VersionID],a.shift,c.class_name AS[CLASS NAME],sc.sec_name,sh.shift_name AS shift_name,vv.version_name AS [Version],a.group_name AS[groupID],case when a.group_name=1 then 'Science' when a.group_name=2 then 'Business Studies' when a.group_name=3 then  'Humanities' end as[Grup]
from tbl_student_previous_history a inner join student_info b on a.spi_student_id=b.student_id inner join class_info c on c.class_id=a.spi_class left join section_info sc on sc.sec_id=a.spi_section inner join shift_info sh on sh.shift_id=a.shift left join version_info vv on vv.id=a.spi_version " + Parameter;
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "student_info");
            return dt;
        }

        public static object GetCourseNameAssaign(string TracID)
        {
            string connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string per = "";
            if (!string.IsNullOrEmpty(TracID))
            {
                per = " where ct.ID='" + TracID + "'";
            }

            string query = @"Select ci.CourseName,ci.ID,ci.CourseFee,ci.Doscount as Discount from tbl_Course_Name  ci
 inner join CourseTrac ct on ct.id=ci.TracID 
 inner join TBL_SHEDULE_ENTRY_MST sm on sm.CourseID=ci.ID " + per + " ";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "tbl_Course_Name");
            return dt;
        }
    }

}