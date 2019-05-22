using System;
using System.Data;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using KHSC;

/// <summary>
/// Summary description for clsSubOfferManager
/// </summary>
/// 
namespace autouniv
{
    public class clsSubOfferManager
    {
        public static void CreateOfferMst(clsSubOfferMst mst, DataTable ddtDtl, DataTable dtStd, DataTable dtShedule,
            string OfferDepartmentID)
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
                string FieldDeptTypeID = "",
                    ValueDeptTypeID = "",
                    FieldDeptID = "",
                    ValueDeptID = "",
                    FieldOfferDept = "",
                    ValueOfferDept = "";

                if (!string.IsNullOrEmpty(mst.DeptTypeID))
                {
                    FieldDeptTypeID = " , DeptTypeID";
                    ValueDeptTypeID = " , " + mst.DeptTypeID;
                }

                if (!string.IsNullOrEmpty(mst.DeptId))
                {
                    FieldDeptID = " , dept_id";
                    ValueDeptID = " , " + mst.DeptId;
                }
                if (!string.IsNullOrEmpty(OfferDepartmentID))
                {
                    FieldOfferDept = " , OfferDept";
                    ValueOfferDept = " , '" + OfferDepartmentID + "' ";
                }
                command.CommandText =
                    @"insert into sub_offer_mst (offer_id,offer_date,sessions,study_year,batch_no,sections ,[AddBy] ,[AddDate] " +
                    FieldDeptTypeID + " " + FieldDeptID + " " + FieldOfferDept + ") values ('" + mst.OfferId + "', " +
                    " convert(datetime,nullif('" + mst.OfferDate + "',''),103), " + " '" + mst.Sessions + "','" +
                    mst.StudyYear + "','" + mst.BatchNo + "','" + mst.Sections + "','" +
                    mst.LoginBy + "',GETDATE() " + ValueDeptTypeID + " " + ValueDeptID + " " + ValueOfferDept + ")";
                command.ExecuteNonQuery();

                foreach (DataRow dr in ddtDtl.Rows)
                {
                    if (dr["check"].ToString() == "1")
                    {
                        command.CommandText =
                            @"insert into sub_offer_dtl (offer_id,subject_id,FacultyID,Section) values ('" + mst.OfferId +
                            "', '" + dr["subject_id"].ToString() + "' ,'" + dr["FacultyID"].ToString() + "','" +
                            dr["Section"].ToString() + "') ";
                        command.ExecuteNonQuery();

                        command1.CommandText = @"SELECT top(1)[ID]  FROM [sub_offer_dtl] ORDER BY ID DESC";
                        int DtlID = Convert.ToInt32(command1.ExecuteScalar());

                        if (dtStd != null)
                        {
                            foreach (DataRow drr in dtStd.Rows)
                            {
                                command.CommandText = @"insert into std_subject(student_id,sessions,Batch,study_year,subject_id,UpdateBy,UpdateDate,FacultyID,Dtl_ID,CheckTime,Improve) 
                   values ('" + drr["ID"].ToString() + "', '" + mst.Sessions + "','" + mst.BatchNo.Replace("''", "'0'") +
                                                      "','" + mst.StudyYear + "','" + dr["subject_id"].ToString() +
                                                      "','" + mst.LoginBy + "',GETDATE(),'" + dr["FacultyID"].ToString() +
                                                      "','" + DtlID + "','0','N')";
                                command.ExecuteNonQuery();
                            }
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
        public static void UpdateOfferMst(clsSubOfferMst mst, DataTable dtSubjectOfferDtl, DataTable dtStd, DataTable dtShedule,
            DataTable dtOldSub, string OfferDepartmentID)
        {
            SqlConnection connection = new SqlConnection(DataManager.OraConnString());
            SqlTransaction transection;
            try
            {
                //****************** Old Subject Offer Details **********//
                DataTable dtOfferDept = IdManager.GetShowDataTable("select * from SUB_OFFER_DTL WHERE OFFER_ID='" + mst.OfferId + "' ");
                //**
                connection.Open();
                transection = connection.BeginTransaction();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transection;

                SqlCommand command1 = new SqlCommand();
                command1.Connection = connection;
                command1.Transaction = transection;

                SqlCommand command2 = new SqlCommand();
                command2.Connection = connection;
                command2.Transaction = transection;

                //********** Suject Offer MSt ******//

                string DeptValue = "", OfferDepartmentValue = "";
                if (!string.IsNullOrEmpty(mst.DeptId))
                {
                    DeptValue = " , dept_id='" + mst.DeptId + "' ";
                }
                if (!string.IsNullOrEmpty(OfferDepartmentID))
                {
                    OfferDepartmentValue = " , OfferDept ='" + OfferDepartmentID + "' ";
                }
                else
                {
                    OfferDepartmentValue = " , OfferDept = NULL ";
                }
                command.CommandText = @"update sub_offer_mst set offer_date= convert(datetime,nullif('" + mst.OfferDate +
                                      "',''),103), sessions= '" + mst.Sessions + "', study_year='" + mst.StudyYear +
                                      "',batch_no='" + mst.BatchNo + "',sections= '" + mst.Sections + "' ,[UpdateBy]='" +
                                      mst.LoginBy + "',[UpdateDate]=GETDATE() " + DeptValue + " " + OfferDepartmentValue +
                                      " where offer_id= '" + mst.OfferId + "' ";
                command.ExecuteNonQuery();
                
                //***** Insert  & Update sub_offer_dtl & std_subject **********//

                foreach (DataRow dr in dtSubjectOfferDtl.Rows)
                {
                    int DtlID = 0;
                    //string OfferDept ="", OfferDeptTypeID= "";
                    //************* Subnject Offer Dtl *********//
                    string FieldOfferDept = "", FieldOfferDeptTypeID = "", ValOfferDept = "", ValOfferDeptTypeID="",UpdateDept="",UpdateDeptType="";
                    if (dr["check"].ToString().Equals("1"))
                    {
                        DataRow[] rows = dtOfferDept.Select("ID ='" + dr["ID"].ToString().Trim() + "'");
                        FieldOfferDept = ",OfferDept";
                        FieldOfferDeptTypeID = ",OfferDeptTypeID";
                        if (rows.Length > 0)
                        {
                            if (!string.IsNullOrEmpty(rows[0]["OfferDept"].ToString()))
                            {
                                ValOfferDept = ",'" + rows[0]["OfferDept"].ToString() + "' ";
                                UpdateDept = ",OfferDept='" + rows[0]["OfferDept"].ToString() + "' ";
                            }
                            else
                            {
                                ValOfferDept = ", NULL";
                            }

                            if (!string.IsNullOrEmpty(rows[0]["OfferDeptTypeID"].ToString()))
                            {
                                ValOfferDeptTypeID = ", '" + rows[0]["OfferDeptTypeID"].ToString() + "' ";
                                UpdateDeptType = ",OfferDeptTypeID='" + rows[0]["OfferDeptTypeID"].ToString() + "'";
                            }
                            else
                            {
                                ValOfferDeptTypeID = ", NULL";
                            }
                        }
                        else
                        {
                            ValOfferDept = ", NULL";
                            ValOfferDeptTypeID = ", NULL";
                        }

                        command1.CommandText = @"select COUNT(*) from SUB_OFFER_DTL t1 where t1.ID=" +
                                               dr["ID"].ToString();
                        int CheckSubDtl = Convert.ToInt32(command1.ExecuteScalar());
                        if (CheckSubDtl > 0)
                        {
                            command2.CommandText = @"update SUB_OFFER_DTL set FacultyID='" +
                                                   dr["FacultyID"].ToString() + "',Section='" +
                                                   dr["Section"].ToString() + "' " + UpdateDept + " " + UpdateDeptType +
                                                   " where ID='" +
                                                   dr["ID"].ToString() + "'";
                            command2.ExecuteNonQuery();
                            command1.CommandText = @"SELECT [ID] FROM [sub_offer_dtl] where ID=" + dr["ID"].ToString();
                            DtlID = Convert.ToInt32(command1.ExecuteScalar());
                        }
                        else
                        {

                            command.CommandText =
                                @"insert into sub_offer_dtl (offer_id,subject_id,FacultyID,Section " + FieldOfferDept +
                                " " + FieldOfferDeptTypeID + ") values ('" + mst.OfferId + "', '" +
                                dr["subject_id"].ToString() + "' ,'" + dr["FacultyID"].ToString() + "','" +
                                dr["Section"].ToString() + "' " + ValOfferDept + " " + ValOfferDeptTypeID + ") ";
                            command.ExecuteNonQuery();

                            command1.CommandText = @"SELECT top(1)[ID]  FROM [sub_offer_dtl] ORDER BY ID DESC";
                            DtlID = Convert.ToInt32(command1.ExecuteScalar());
                        }
                    }
                    else
                    {
                        command1.CommandText = @"SELECT [ID] FROM [sub_offer_dtl] where ID=" + dr["ID"].ToString();
                        DtlID = Convert.ToInt32(command1.ExecuteScalar());
                        command.CommandText = @"delete from sub_offer_dtl where ID=" + dr["ID"].ToString();
                        command.ExecuteNonQuery();
                    }

                    // ****** Update Student Subject Choice Tbable ******//
                    //if (dtStd == null)
                    //{
                        command1.CommandText = @"SELECT COUNT(*) FROM [STD_SUBJECT] t1 where t1.[Dtl_ID]='" +
                                               dr["ID"].ToString() + "' AND t1.DeleteBy IS NULL";
                        int CountStudentSubject = Convert.ToInt32(command1.ExecuteScalar());
                        if (CountStudentSubject > 0)
                        {
                            if (dr["check"].ToString() == "1")
                            {
                                // ****************** Update Offer Dtl_ID STD_SUBJECT  ****************//
                                command.CommandText = @"UPDATE [STD_SUBJECT] SET FacultyID='" +
                                                      dr["FacultyID"].ToString() + "',[Dtl_ID]='" + DtlID +
                                                      "' where [Dtl_ID]='" +
                                                      dr["ID"].ToString() + "' AND DeleteBy IS NULL";
                                command.ExecuteNonQuery();
                            }
                            else
                            {
                                command.CommandText = @"UPDATE [STD_SUBJECT] SET DeleteBy='" + mst.LoginBy +
                                                      "',DeleteDate=GETDATE() where [Dtl_ID]='" +
                                                      dr["ID"].ToString() +
                                                      "' AND DeleteBy IS NULL";
                                command.ExecuteNonQuery();
                            }
                        }
                    //}
                    
                    //********************* Update Shedule Table ******//           
                    if (dtShedule != null)
                    {
                        command.CommandText = @"select COUNT(*) from [SUB_Offer_Schedule] where OfferMstID='" +
                                              mst.OfferId + "' and OfferDtl_ID='" + dr["ID"].ToString() + "'";
                        int Count = Convert.ToInt32(command.ExecuteScalar());
                        if (Count > 0)
                        {
                            if (dr["check"].ToString() == "1")
                            {
                                // ****************** STD_SUBJECT  && SUB_Offer_Schedule ****************//
                                command.CommandText = @"UPDATE [SUB_Offer_Schedule] SET OfferDtl_ID='" +
                                                      DtlID + "' where OfferMstID='" + mst.OfferId +
                                                      "' and OfferDtl_ID='" + dr["ID"].ToString() + "' ";
                                command.ExecuteNonQuery();
                            }
                            else
                            {
                                command.CommandText = @"delete from [SUB_Offer_Schedule] where OfferMstID='" +
                                                      mst.OfferId + "' and OfferDtl_ID='" + dr["ID"].ToString() + "'";
                                command.ExecuteNonQuery();
                            }
                        }
                    }
                    //********* Batch Wise Fix student **************//
//                    if (dtStd != null)
//                    {
//                        //command.CommandText = @"delete from sub_offer_dtl where offer_id='" + mst.OfferId + "'";
//                        //command.ExecuteNonQuery();
//                        foreach (DataRow drr in dtStd.Rows)
//                        {
//                            string DeptVal = "", DeptTypeVal = "", BatchVal = "";
//                            if (!string.IsNullOrEmpty(mst.DeptId))
//                            {
//                                DeptVal = " AND E.DEPT_ID='" + mst.DeptId + "' ";
//                            }
//                            if (!string.IsNullOrEmpty(mst.DeptTypeID))
//                            {
//                                DeptTypeVal = " AND E.DeptTypeID='" + mst.DeptTypeID + "' ";
//                            }
//                            if (!string.IsNullOrEmpty(mst.BatchNo))
//                            {
//                                BatchVal = " AND E.BATCH_NO='" + mst.BatchNo + "' ";
//                            }
//                            command2.CommandText =
//                                @"DELETE w FROM STD_SUBJECT w INNER JOIN STUDENT_INFO E  ON E.ID=w.STUDENT_ID AND E.DeleteBy IS NULL WHERE w.STUDENT_ID='" +
//                                drr["ID"].ToString() + "' AND w.[SESSIONS]='" + mst.Sessions + "' AND w.Dtl_ID='" +
//                                dr["ID"].ToString() + "' AND w.STUDY_YEAR='" + mst.StudyYear +
//                                "' AND w.DeleteBy IS NULL " + DeptVal + "  " + DeptTypeVal + " " + BatchVal + " ";
//                            command2.ExecuteNonQuery();

//                            command.CommandText = @"insert into std_subject                                                                                                              (student_id,sessions,Batch,study_year,subject_id,UpdateBy,UpdateDate,FacultyID,Dtl_ID,CheckTime,Improve) 
//                                 values
//                            ('" + drr["ID"].ToString() + "', '" + mst.Sessions + "','" +
//                                                  mst.BatchNo.Replace("''", "'0'") +
//                                                  "','" + mst.StudyYear + "','" + dr["subject_id"].ToString() + "','" +
//                                                  mst.LoginBy + "',GETDATE(),'" + dr["FacultyID"].ToString() + "','" +
//                                                  DtlID + "','0','N')";
//                            command.ExecuteNonQuery();
                             
//                        }
//                    }
                }
                transection.Commit();
                connection.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public static void DeleteOfferMst(clsSubOfferMst mst, DataTable dtStd)
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

                command.CommandText = "update sub_offer_mst set [DeleteBy]='" + mst.LoginBy +
                                      "',[DeleteDate]=GETDATE() where offer_id= convert(nvarchar,'" + mst.OfferId +
                                      "') ";
                command.ExecuteNonQuery();

                command.CommandText = @"update SUB_OFFER_DTL set  [DeleteBy]='" + mst.LoginBy +
                                      "',[DeleteDate]=GETDATE() where OFFER_ID= '" + mst.OfferId + "'  ";
                command.ExecuteNonQuery();

                command.CommandText = @"DELETE FROM [SUB_Offer_Schedule] WHERE [OfferDtl_ID]='" + mst.OfferId + "' ";
                command.ExecuteNonQuery();

                foreach (DataRow dr in dtStd.Rows)
                {
                    string DeptVal = "", DeptTypeVal = "", BatchVal = "";
                    if (!string.IsNullOrEmpty(mst.DeptId))
                    {
                        DeptVal = " AND E.DEPT_ID='" + mst.DeptId + "' ";
                    }
                    if (!string.IsNullOrEmpty(mst.DeptTypeID))
                    {
                        DeptTypeVal = " AND E.DeptTypeID='" + mst.DeptTypeID + "' ";
                    }
                    if (!string.IsNullOrEmpty(mst.BatchNo))
                    {
                        BatchVal = " AND E.BATCH_NO='" + mst.BatchNo + "' ";
                    }

                    command.CommandText = @"UPDATE w
  SET w.DeleteBy = '" + mst.LoginBy + "',w.DeleteDate=GETDATE()  FROM STD_SUBJECT AS w INNER JOIN dbo.STUDENT_INFO AS E ON w.STUDENT_ID = E.ID WHERE w.STUDENT_ID='" + dr["ID"].ToString() + "' AND w.[SESSIONS]='" + mst.Sessions + "' AND w.STUDY_YEAR='" + mst.StudyYear + "' " + DeptVal + "  " + DeptTypeVal + " " + BatchVal + " ";
                    command.ExecuteNonQuery();
                }

                transection.Commit();
                connection.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public static clsSubOfferMst getOfferMst(string mst)
        {
            String connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);

            string query =
                "select offer_id,convert(nvarchar,offer_date,103)offer_date,sessions,study_year,dept_id,batch_no,sections from sub_offer_mst where offer_id='" +
                mst + "'";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "OfferMst");
            if (dt.Rows.Count == 0)
            {
                return null;
            }
            return new clsSubOfferMst(dt.Rows[0]);
        }

        public static clsSubOfferMst getOfferMaster(string dept, string ses, string yr, string bat, string sec,
            string DeptType)
        {
            String connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string batch_no = "", sections = "", ParameterDeptType = "", ParameterDeptID = "";
            if (!string.IsNullOrEmpty(bat))
            {
                batch_no = "and IsNull(batch_no,'99') like IsNull(IsNull('" + bat + "','99'),'%')";
            }
            if (!string.IsNullOrEmpty(sec))
            {
                sections = "and IsNull(sections,'A') like IsNull(IsNull('" + sec + "','A'),'%')";
            }
            if (!string.IsNullOrEmpty(DeptType))
            {
                ParameterDeptType = " and DeptTypeID='" + DeptType + "' ";
            }

            if (!string.IsNullOrEmpty(dept))
            {
                ParameterDeptID = " and dept_id='" + dept + "' ";
            }
            else
            {
                ParameterDeptID = " and dept_id IS NULL ";
            }

            string query =
                "select offer_id,convert(nvarchar,offer_date,103)offer_date,[sessions],study_year,dept_id,batch_no,sections,DeptTypeID,OfferDept from sub_offer_mst where [DeleteBy] IS NULL " +
                ParameterDeptID + " and " +
                " [sessions]='" + ses + "' and convert(varchar,study_year)='" + yr + "' " + ParameterDeptType + " " +
                batch_no + " " + sections + " ";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "OfferMst");
            if (dt.Rows.Count == 0)
            {
                return null;
            }
            return new clsSubOfferMst(dt.Rows[0]);
        }

        public static DataTable getOfferMsts(string criteria)
        {
            String connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query =
                "select offer_id,convert(date,offer_date,103)offer_date,sessions,study_year,dept_id,batch_no,sections from sub_offer_mst";
            if (criteria != "")
            {
                query = query + " where " + criteria;
            }
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "OfferMst");
            return dt;
        }

        public static void CreateOfferDtl(clsSubOfferDtl dtl)
        {
            String connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = "insert into sub_offer_dtl (offer_id,subject_id) values ('" + dtl.OfferId + "', '" +
                           dtl.SubjectId + "' ) ";
            DataManager.ExecuteNonQuery(connectionString, query);
        }

        public static void DeleteOfferDtl(string dtl)
        {
            String connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = "delete from sub_offer_dtl where offer_id=convert(nvarchar,'" + dtl + "') ";
            DataManager.ExecuteNonQuery(connectionString, query);
        }

        public static DataTable getOfferDtls(string offer)
        {
            String connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = "select offer_id,subject_id from sub_offer_dtl where offer_id=convert(nvarchar,'" + offer +
                           "')";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "OfferDtl");
            return dt;
        }

        public static DataTable getOfferDtlSubjects(string offer, string dept, string FacultyID)
        {
            String connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string Parameter = "", parDeptID = "";
            if (!string.IsNullOrEmpty(FacultyID))
            {
                Parameter = " AND  a.FacultyID='" + FacultyID + "' ";
            }
            if (!string.IsNullOrEmpty(dept))
            {
                parDeptID = " AND b.dept_id='" + dept + "'";
            }
            else
            {
                parDeptID = " AND b.dept_id IS NULL ";
            }
            string query =
                @"select '1' AS [check],a.ID,a.offer_id,a.subject_id AS [subject_id],b.SUBJECT_ID AS[subjectCode],b.sub_name AS [SubjectName],b.sub_credit AS[sub_credit],a.FacultyID ,dbo.InitCap(tt.F_NAME+' '+tt.M_NAME+' '+tt.L_NAME) AS [FacultyName],'O' AS Flag,a.[Section] AS [Section] FROM sub_offer_dtl a inner join subject_info b on b.ID=a.SUBJECT_ID AND a.DeleteBy IS NULL " +
                Parameter +
                " Left JOIN PMIS_PERSONNEL tt on tt.EMP_NO=a.FacultyID where b.DeleteBy IS NULL AND offer_id=convert(nvarchar,'" +
                offer + "') " + parDeptID + " ORDER BY b.SUBJECT_ID ASC ";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "OfferDtl");
            return dt;
        }

        public static DataTable getOfferDtlsSubjects(string dept, string ses, string yr)
        {
            String connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);

            string query =
                @"select a.ID AS[dtlID],offer_id,a.subject_id AS [SubID],t1.SUBJECT_ID,b.sub_name,b.sub_credit,a.FacultyID,(tt.F_NAME+' '+tt.M_NAME+' '+tt.L_NAME) AS FacultyName,'' AS [dtlID] from sub_offer_dtl a LEFT join  subject_info b on   a.subject_id=b.ID inner join SUBJECT_INFO t1 on t1.ID=a.SUBJECT_ID Left Join [PMIS_PERSONNEL] tt on tt.EMP_NO=a.FacultyID where offer_id=(select offer_id from sub_offer_mst where dept_id='" +
                dept + "'  and [sessions]='" + ses + "' and convert(nvarchar,study_year)='" + yr + "') and b.dept_id='" +
                dept + "' ";

            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "OfferDtl");
            return dt;
        }

        public static DataTable GetShowOfferMst(string DepartmentID, string Year, string SemesterID)
        {
            String connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string Parameter = "";
            if (DepartmentID != "" && Year == "" && SemesterID == "")
            {
                Parameter = " WHERE t1.DEPT_ID='" + DepartmentID + "' order by t1.[SESSIONS],t1.STUDY_YEAR ";
            }
            else if (DepartmentID == "" && Year != "" && SemesterID == "")
            {
                Parameter = " WHERE t1.STUDY_YEAR='" + Year + "' order by t1.[SESSIONS],t1.STUDY_YEAR ";
            }
            else if (DepartmentID == "" && Year == "" && SemesterID != "")
            {
                Parameter = " WHERE t1.SECTIONS='" + SemesterID + "' order by t1.[SESSIONS],t1.STUDY_YEAR ";
            }
            else if (DepartmentID != "" && Year != "" && SemesterID == "")
            {
                Parameter = " WHERE t1.DEPT_ID='" + DepartmentID + "' AND t1.STUDY_YEAR='" + Year +
                            "' order by t1.[SESSIONS],t1.STUDY_YEAR ";
            }
            else if (DepartmentID != "" && Year != "" && SemesterID != "")
            {
                Parameter = " WHERE t1.DEPT_ID='" + DepartmentID + "' AND t1.STUDY_YEAR='" + Year +
                            "' AND t1.[SESSIONS]='" + SemesterID + "' order by t1.[SESSIONS],t1.STUDY_YEAR ";
            }

            string query =
                @"SELECT t1.[OFFER_ID] ,CONVERT(NVARCHAR,t1.[OFFER_DATE],103) AS [OFFER_DATE]
      ,CASE WHEN t1.[SESSIONS]=1 THEN 'Spring' WHEN t1.[SESSIONS]=2 THEN 'Summer' WHEN t1.[SESSIONS]=3 THEN 'Fall' ELSE '' END AS[SESSIONS] ,t1.[STUDY_YEAR] ,t2.DEPT_NAME AS DEPT_NAME,t3.DEPT_NAME AS [DepartmentType],t1.[BATCH_NO],t1.[SECTIONS]  FROM [SUB_OFFER_MST] t1 INNER JOIN Department t2 on t2.ID=t1.DEPT_ID Left Join DEPT_INFO t3 on t3.DEPT_ID=t1.DeptTypeID " +
                Parameter;
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "OfferDtl");
            return dt;
        }

        public static DataTable GetShowOfferDetails(string OfferID)
        {
            String connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string Parameter = "";
            string query =
                @"SELECT t1.SUBJECT_ID,t2.SUBJECT_ID AS [Sub_Code]
      ,t2.SUB_NAME  
      ,t2.SUB_CREDIT  
      ,t3.F_NAME+' '+t3.M_NAME+' '+t3.L_NAME AS[Faculty] 
  FROM [SUB_OFFER_DTL] t1 INNER JOIN SUBJECT_INFO t2 on t2.ID=t1.SUBJECT_ID Left JOIN PMIS_PERSONNEL t3 on t3.EMP_NO=t1.FacultyID WHERE t1.[OFFER_ID]='" +
                OfferID + "' order by t2.SUBJECT_ID ";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "OfferDtl");
            return dt;
        }

        public static DataTable GetFacultyName(string SubjectId)
        {
            String connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);

            string query = @"Select distinct t1.FacultyID,(pm.F_NAME+' '+pm.M_NAME+' '+pm.L_NAME) as FacultyName from STD_SUBJECT t1
Inner join PMIS_PERSONNEL PM on PM.EMP_NO=t1.FacultyID where t1.SUBJECT_ID ='" + SubjectId + "'";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "STD_SUBJECT");
            return dt;
        }

        //public static DataTable GetAllDays()
        //{
        //    String connectionString = DataManager.OraConnString();
        //    SqlConnection sqlCon = new SqlConnection(connectionString);

        //    string query = @"SELECT [ID] ,[Days] FROM [Days]";
        //    DataTable dt = DataManager.ExecuteQuery(connectionString, query, "Days");
        //    return dt;
        //}

        public static DataTable GetShowSheduleInSubject(string OfferDtlID, string OfferMSTID)
        {
            String connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);

            string query =
                @"select se.ID,dt.ID as DaysID,LTRIM(RTRIM(substring(convert(nvarchar,RIGHT(CONVERT(VARCHAR,sd.[StartTime], 100),8)),0,7))) AS [StartTime],sd.StartAmPm AS [StartAmPm],LTRIM(RTRIM(substring(convert(nvarchar,RIGHT(CONVERT(VARCHAR,[EndtTime], 100),8)),0,7))) AS [EndtTime],sd.EndAmPm AS [EndAmPm],sd.RoomNo from dbo.TBL_SHEDULE_ENTRY_MST se
left join dbo.TBL_SHEDULE_ENTRY_DTL sd on sd.MSTID=se.ID
inner join dbo.Tbl_Days_Info dt on dt.ID=sd.[Days]  WHERE se.BatchNo='" +
                OfferDtlID + "' AND sd.RoomNo='" + OfferMSTID + "' ";
            DataTable dt = DataManager.ExecuteQuery(connectionString, query, "TBL_SHEDULE_ENTRY_DTL");
            return dt;
        }

        //********************** New Code ***********************//

        //************** Get Show Subject Info In Department Wise And Check In Year And Semester in Offer Subject **********************//

        public static DataTable getShowSubjectInfo(string DeptID, string Session, string Year)
        {
            String connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string Parameter = "";
            if (!string.IsNullOrEmpty(DeptID))
            {
                Parameter = "t1.[DEPT_ID]='" + DeptID + "'  AND ";
            }
            else
            {
                Parameter = "t1.[DEPT_ID] IS NULL  AND ";
            }
            string Query = @"SELECT t1.[ID],t1.[SUBJECT_ID],REPLACE(t1.[SUB_NAME],'&',' and ') AS[SUB_NAME] ,t1.[SUB_CREDIT],t1.[Mid_Marks],t1.[Final_Marks],t1.[DEPT_ID]
  FROM [SUBJECT_INFO] t1 where " + Parameter + " Active=1 and t1.DeleteBy IS NULL ORDER BY t1.[SUBJECT_ID] ASC ";
            DataTable dt = DataManager.ExecuteQuery(connectionString, Query, "SUBJECT_INFO");
            return dt;
        }

        public static DataTable GetShowCourceRegistrationReport(string ReportType, string DeptID, string SessionID,
            string Year,string ID,string DeptTypeID,string Batch)
        {
            String connectionString = DataManager.OraConnString();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            DataTable dt = null;
            string Parameter = "", Parameter1 = "", Parameter2 = "", Parameter3 = "", Parameter4 = "", Parameter5 = "";
            if (ReportType == "CRL")
            {
                if (!string.IsNullOrEmpty(ID) && string.IsNullOrEmpty(DeptTypeID) && string.IsNullOrEmpty(Batch))
                {
                    Parameter = " AND t1.STUDENT_ID=" + ID + " ";
                }
                else if (!string.IsNullOrEmpty(ID) && !string.IsNullOrEmpty(DeptTypeID) && string.IsNullOrEmpty(Batch))
                {
                    Parameter1 = "and t3.DeptTypeID='" + DeptTypeID + "'";
                    Parameter2 = "AND t2.DeptTypeID='" + DeptTypeID + "'";
                    Parameter = " AND t1.STUDENT_ID=" + ID + " ";
                }
                else if (string.IsNullOrEmpty(ID) && string.IsNullOrEmpty(DeptTypeID) && !string.IsNullOrEmpty(Batch))
                {
                    Parameter3 = "AND t2.BATCH_NO='" + Batch + "'";
                    Parameter4 = "AND t3.BATCH_NO='" + Batch + "'";
                    Parameter5 = "AND t1.BATCH_NO='" + Batch + "'";
                }
                else if (string.IsNullOrEmpty(ID) && !string.IsNullOrEmpty(DeptTypeID) && string.IsNullOrEmpty(Batch))
                {
                    Parameter1 = " AND t3.DeptTypeID='" + DeptTypeID + "'";
                    Parameter2 = "AND t2.DeptTypeID='" + DeptTypeID + "'";
                }
                else if (string.IsNullOrEmpty(ID) && !string.IsNullOrEmpty(DeptTypeID) && !string.IsNullOrEmpty(Batch))
                {
                    Parameter1 = " AND t3.DeptTypeID='" + DeptTypeID + "'";
                    Parameter2 = "AND t2.DeptTypeID='" + DeptTypeID + "'";

                    Parameter3 = "AND t2.BATCH_NO='" + Batch + "'";
                    Parameter4 = "AND t3.BATCH_NO='" + Batch + "'";
                    Parameter5 = "AND t1.BATCH_NO='" + Batch + "'";
                }
                string Query =
                    @"SELECT DISTINCT t1.[STUDENT_ID] AS[ID],t2.STUDENT_ID,dbo.InitCap(ISNULL(t2.F_NAME,'')+''+ISNULL(t2.M_NAME,'')+''+ISNULL(t2.L_NAME,'')) Name,t2.BATCH_NO,t2.MOBILE_NO,totCredit,t5.ABVR FROM [STD_SUBJECT] t1 inner join STUDENT_INFO t2 on t2.ID=t1.STUDENT_ID AND t2.DeleteBy IS NULL inner join DEPT_INFO t5 on t5.DEPT_ID= t2.DeptTypeID and t5.DeleteBy IS Null INNER JOIN (select t1.STUDENT_ID AS[std_ID],t3.STUDENT_ID,SUM(ISNULL(t2.SUB_CREDIT,0))AS totCredit from [STD_SUBJECT] t1 INNER join SUBJECT_INFO t2 on t2.ID=t1.SUBJECT_ID AND t2.DeleteBy IS NULL inner join STUDENT_INFO t3 on t3.ID=t1.STUDENT_ID AND t3.DeleteBy IS NULL " + Parameter + " where t1.[SESSIONS]='" + SessionID + "' AND t1.[STUDY_YEAR]='" + Year + "'  AND (t2.DEPT_ID=" + DeptID + " OR t2.DEPT_ID IS NULL) " + Parameter1 + " " + Parameter4 + "  and t1.DeleteBy IS NULL GROUP BY t1.STUDENT_ID,t3.STUDENT_ID) tt on tt.std_ID=t1.STUDENT_ID  WHERE t1.[SESSIONS]='" + SessionID + "' AND t1.[STUDY_YEAR]='" + Year + "' AND t2.DEPT_ID=" + DeptID + " AND t1.DeleteBy IS NULL " + Parameter + " " + Parameter2 + " " + Parameter3 + " ORDER BY  t2.STUDENT_ID ASC";
                dt = DataManager.ExecuteQuery(connectionString, Query, "SUBJECT_INFO");

            }
            else if (ReportType == "CURL")
            {
                if (!string.IsNullOrEmpty(ID) && string.IsNullOrEmpty(DeptTypeID) && string.IsNullOrEmpty(Batch))
                {
                    Parameter = " AND t1.STUDENT_ID=" + ID + " ";
                }
                else if (!string.IsNullOrEmpty(ID) && !string.IsNullOrEmpty(DeptTypeID) && string.IsNullOrEmpty(Batch))
                {
                    Parameter1 = "and t1.DeptTypeID='" + DeptTypeID + "'";
                    Parameter2 = "AND t2.DeptTypeID='" + DeptTypeID + "'";
                    Parameter = " AND t1.STUDENT_ID=" + ID + " ";
                }
                else if (string.IsNullOrEmpty(ID) && string.IsNullOrEmpty(DeptTypeID) && !string.IsNullOrEmpty(Batch))
                {
                    Parameter3 = "AND t2.BATCH_NO='" + Batch + "'";                   
                    Parameter5 = "AND t1.BATCH_NO='" + Batch + "'";
                }
                else if (string.IsNullOrEmpty(ID) && !string.IsNullOrEmpty(DeptTypeID) && string.IsNullOrEmpty(Batch))
                {
                    Parameter1 = " AND t1.DeptTypeID='" + DeptTypeID + "'";
                    Parameter2 = "AND t2.DeptTypeID='" + DeptTypeID + "'";
                }
                else if (string.IsNullOrEmpty(ID) && !string.IsNullOrEmpty(DeptTypeID) && !string.IsNullOrEmpty(Batch))
                {
                    Parameter1 = " AND t1.DeptTypeID='" + DeptTypeID + "'";
                    Parameter2 = "AND t2.DeptTypeID='" + DeptTypeID + "'";                   
                    Parameter5 = "AND t1.BATCH_NO='" + Batch + "'";
                }
                string Query =
                    @"select t1.STUDENT_ID,dbo.InitCap(ISNULL(t1.F_NAME,'')+''+ISNULL(t1.M_NAME,'')+''+ISNULL(t1.L_NAME,'')) Name,t1.BATCH_NO,t1.MOBILE_NO,'' AS[totCredit] from STUDENT_INFO t1 where  " + Parameter + " t1.DeleteBy IS NULL AND t1.ID  not in (SELECT DISTINCT t1.[STUDENT_ID] FROM [STD_SUBJECT] t1 inner join STUDENT_INFO t2 on t2.ID=t1.STUDENT_ID AND t2.DeleteBy IS NULL WHERE t1.[SESSIONS]='" +
                    SessionID + "' AND t1.[STUDY_YEAR]='" + Year + "' AND t2.DEPT_ID='" + DeptID +
                    "' " + Parameter2 + " " + Parameter3 + " AND t1.DeleteBy IS NULL ) " + Parameter5 + " AND t1.DEPT_ID='" + DeptID +
                    "' " + Parameter1 + " " + Parameter5 + "  order by t1.BATCH_NO desc,dbo.InitCap(ISNULL(t1.F_NAME,'')+''+ISNULL(t1.M_NAME,'')+''+ISNULL(t1.L_NAME,'')) asc ";
                dt = DataManager.ExecuteQuery(connectionString, Query, "SUBJECT_INFO");
            }
            return dt;
        }
        public static DataTable getOfferDtlSubjectsWithFaculty(string FacultyID, string SessionID, string Year)
        {
            String connectionString = DataManager.OraConnString();
            string Query =
                @"SELECT DISTINCT t1.ID,t1.[SUBJECT_ID],t2.SUBJECT_ID ,ISNULL(t1.Section,'')+' - '+ ISNULL(t2.SUBJECT_ID,'')+' - '+ ISNULL(t2.SUB_NAME,'') AS SUB_NAME    
  FROM [dbo].[SUB_OFFER_DTL] t1 INNER JOIN [dbo].[SUB_OFFER_MST] t4 on t4.OFFER_ID=t1.OFFER_ID AND t4.DeleteBy IS NULL inner join SUBJECT_INFO t2 on t2.ID=t1.SUBJECT_ID AND t2.DeleteBy IS NULL LEFT JOIN [PMIS_PERSONNEL] t3 on t3.EMP_NO=t1.FacultyID  where t1.FacultyID='" +
                FacultyID + "' AND t4.[SESSIONS]='" + SessionID + "' AND t4.STUDY_YEAR='" + Year +
                "' AND t1.DeleteBy IS NULL ORDER BY t2.SUBJECT_ID";
           DataTable dt = DataManager.ExecuteQuery(connectionString, Query, "SUBJECT_INFO");
            return dt;
        }
        //****************** save shedule in pop up from ****************//
        public static void SaveSheduleInSubject(DataTable dt, string OffferID, string OfferDtlID, string LoginBy)
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

                command.CommandText = "select COUNT(*) from [SUB_Offer_Schedule] where OfferMstID='" + OffferID + "' and OfferDtl_ID='" + OfferDtlID + "'";
                int Count = Convert.ToInt32(command.ExecuteScalar());
                if (Count > 0)
                {
                    command.CommandText = @"delete from [SUB_Offer_Schedule] where OfferMstID='" + OffferID + "' and OfferDtl_ID='" + OfferDtlID + "'";
                    command.ExecuteNonQuery();

                    foreach (DataRow drsh in dt.Rows)
                    {
                        if (drsh["ID"].ToString() != "")
                        {
                            string StartDateTime = DateTime.Now.ToString("MM/dd/yyyy") + " " +
                                                   drsh["StartTime"].ToString() + " " +
                                                   drsh["StartAmPm"].ToString();
                            string EndDateTime = DateTime.Now.ToString("MM/dd/yyyy") + " " +
                                                 drsh["EndtTime"].ToString() + " " +
                                                 drsh["EndAmPm"].ToString();
                            command.CommandText = @"INSERT INTO [SUB_Offer_Schedule]
                            ([OfferMstID],[OfferDtl_ID],[DayID],[StartTime],[EndTime],[AddBy],[AddDate],RoomNo)
                            VALUES
                        ('" + OffferID + "','" + OfferDtlID + "','" + drsh["DaysID"].ToString() + "','" + StartDateTime +
                                                  "','" + EndDateTime + "','" + LoginBy +
                                                  "',GETDATE(),'" +
                                                  drsh["RoomNo"].ToString() + "')";
                            command.ExecuteNonQuery();

                        }
                    }
                }
                else
                {
                    foreach (DataRow drsh in dt.Rows)
                    {
                        if (drsh["ID"].ToString() != "")
                        {
                            string StartDateTime = DateTime.Now.ToString("MM/dd/yyyy") + " " +
                                                   drsh["StartTime"].ToString() + " " +
                                                   drsh["StartAmPm"].ToString();
                            string EndDateTime = DateTime.Now.ToString("MM/dd/yyyy") + " " +
                                                 drsh["EndtTime"].ToString() + " " +
                                                 drsh["EndAmPm"].ToString();
                            command.CommandText = @"INSERT INTO [SUB_Offer_Schedule]
                            ([OfferMstID],[OfferDtl_ID],[DayID],[StartTime],[EndTime],[AddBy],[AddDate],RoomNo)
                            VALUES
                        ('" + OffferID + "','" + OfferDtlID + "','" + drsh["DaysID"].ToString() + "','" + StartDateTime +
                                                  "','" + EndDateTime + "','" + LoginBy +
                                                  "',GETDATE(),'" +
                                                  drsh["RoomNo"].ToString() + "')";
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
        public static void SaveDepartmentWithSubject(string AllDept, string AllDeptType, string DtlID, string MstID)
        {
            String connectionString = DataManager.OraConnString();
            string query = @"UPDATE [SUB_OFFER_DTL]  SET OfferDept='" + AllDept + "',OfferDeptTypeID='" + AllDeptType + "' WHERE [OFFER_ID] ='" + MstID + "' AND ID=" + DtlID + " ";
            DataManager.ExecuteNonQuery(connectionString, query);
        }

        public static DataTable GetShowSubjectStudentRegistrationList(string Parameter)
        {
            String connectionString = DataManager.OraConnString();
            string Query =
                @"SELECT ISNULL(t2.STUDENT_ID,'')+' - '+dbo.InitCap(ISNULL(t2.F_NAME,'')+' '+ISNULL(t2.M_NAME,'')+' '+ISNULL(t2.L_NAME,''))Name,t3.SUBJECT_ID,t3.SUB_NAME,t4.DEPT_NAME,t2.MOBILE_NO FROM [dbo].[STD_SUBJECT] t1 inner join STUDENT_INFO t2 on t2.ID=t1.STUDENT_ID AND t2.DeleteBy IS NULL left join SUBJECT_INFO t3 on t3.ID=t1.SUBJECT_ID AND t3.DeleteBy IS NULL inner join Department t4 on t4.ID=t2.DEPT_ID AND t4.DeleteBy IS NULL " + Parameter + " ORDER BY t2.STUDENT_ID ASC";
            DataTable dt = DataManager.ExecuteQuery(connectionString, Query, "SUBJECT_INFO");
            return dt;
        }

        //********************* Faculty Change Show Subject *********************//

        public static DataTable GetShowAllSubject(string Parameter)
        {
            String connectionString = DataManager.OraConnString();
            string Query =
                @"SELECT DISTINCT t1.ID,t1.[SUBJECT_ID],t2.SUBJECT_ID ,ISNULL(t1.Section,'')+' - '+ ISNULL(t2.SUBJECT_ID,'')+' - '+ ISNULL(t2.SUB_NAME,'') AS SUB_NAME, ISNULL(t3.F_NAME,'')+' '+ISNULL(t3.M_NAME,'')+' '+ISNULL(t3.L_NAME,'') AS[Faculty],t3.EMP_NO AS[FacultyID]
  FROM [dbo].[SUB_OFFER_DTL] t1 INNER JOIN [dbo].[SUB_OFFER_MST] t4 on t4.OFFER_ID=t1.OFFER_ID AND t4.DeleteBy IS NULL inner join SUBJECT_INFO t2 on t2.ID=t1.SUBJECT_ID AND t2.DeleteBy IS NULL LEFT JOIN [PMIS_PERSONNEL] t3 on t3.EMP_NO=t1.FacultyID " + Parameter + " ORDER BY t2.SUBJECT_ID ";
            DataTable dt = DataManager.ExecuteQuery(connectionString, Query, "SUBJECT_INFO");
            return dt;
        }

        public static void GetChangeFaculty(string offerDtl_ID, string FacultyID,string oldFacultyID,string Session,string Year,string LoginBy)
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

                command.CommandText = @"INSERT INTO [STD_ChangeFaculty]
           ([Session],[StudyYear],[Subject],[CurFaculty],[NewFaculty],[AddBy],[AddDate])
     VALUES
           ('" + Session + "','" + Year + "','" + offerDtl_ID + "','" + oldFacultyID + "','" + FacultyID + "','" + LoginBy + "',GETDATE())";
                command.ExecuteNonQuery();

                command1.CommandText = @"UPDATE [SUB_OFFER_DTL]  SET [FacultyID] ='" + FacultyID + "' WHERE ID='" + offerDtl_ID + "'";
                command1.ExecuteNonQuery();

                command.CommandText = @"UPDATE [STD_ResultMST]  SET [FacultyID] ='" + FacultyID + "'  WHERE [SUBJECT_ID]='" + offerDtl_ID + "' ";
                command.ExecuteNonQuery();

                command1.CommandText = @"UPDATE [STD_SUBJECT]  SET [FacultyID] ='" + FacultyID + "',UpdateBy='" + LoginBy + "',UpdateDate=GETDATE() WHERE [FacultyID]='" + oldFacultyID + "' AND [Dtl_ID]='" + offerDtl_ID + "' AND DeleteBy IS NULL ";
                command1.ExecuteNonQuery();

                transection.Commit();
            }
            catch (Exception ex)
            {
                // transection.Rollback();
                throw new Exception(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
    }
}