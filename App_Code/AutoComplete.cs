using System;
using System.Collections.Generic;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using KHSC;


[WebService(Namespace = "http://simran.com/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[System.Web.Script.Services.ScriptService]

public class AutoComplete : WebService
{
    public AutoComplete()
    {

    }
    
    //_______________________________________________________________________________

    [WebMethod]
    public string[] GetCompletionStudentId(string prefixText, int count)
    {
        if (count == 0)
        {
            count = 10;
        }
        DataTable dt = GetStudent(prefixText);

        List<string> items = new List<string>(count);
        for (int i = 0; i < dt.Rows.Count; i++)
        {

            string str = dt.Rows[i][0].ToString();

            items.Add(str);
        }

        return items.ToArray();
    }

    public DataTable GetStudent(string strName)
    {
        string strConn = DataManager.OraConnString();
        SqlConnection sqlCon =new SqlConnection(strConn);
        string query = @"select (a.[student_id]+' - '+a.[f_name]+' - '+b.BatchNo+' - '+cn.CourseName+' - '+a.mobile_no)
from student_info a
inner join dbo.std_current_status b on b.student_id=a.ID
inner join tbl_Course_Name cn on cn.ID=b.CourseId where upper(a.[student_id]+' - '+a.[f_name]+' - '+b.BatchNo+' - '+cn.CourseName+' - '+a.mobile_no) LIKE upper('%" + strName + "%') and a.status=1";

        DataTable dt = DataManager.ExecuteQuery(strConn, query, "autoname");
        return dt;
    }
    //_______________________________________________________________________________
    [WebMethod]
    public string[] GetCompletionList(string prefixText, int count)
    {
        if (count == 0)
        {
            count = 10;
        }
        DataTable dt = GetItems(prefixText);
       
        List<string> items = new List<string>(count);
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            
            string str = dt.Rows[i][0].ToString();
           
            items.Add(str);
        }

        return items.ToArray();
    }
    [WebMethod]
    public string[] GetCompletionListSeg(string prefixText, int count)
    {
        if (count == 0)
        {
            count = 10;
        }
        DataTable dt = GetSegDesc(prefixText);

        List<string> items = new List<string>(count);
        for (int i = 0; i < dt.Rows.Count; i++)
        {

            string str = dt.Rows[i][0].ToString();

            items.Add(str);
        }

        return items.ToArray();
    }
    [WebMethod]
    public string[] GetCompletionListCost(string prefixText, int count)
    {
        if (count == 0)
        {
            count = 10;
        }
        DataTable dt = GetCostDesc(prefixText);

        List<string> items = new List<string>(count);
        for (int i = 0; i < dt.Rows.Count; i++)
        {

            string str = dt.Rows[i][0].ToString();

            items.Add(str);
        }

        return items.ToArray();
    }
    
    public DataTable GetItems(string strName)
    {
        string strConn = DataManager.OraConnString();
        SqlConnection sqlCon = new SqlConnection(strConn);
        string query = "select coa_desc from gl_coa where upper(coa_desc) like upper('%" + strName + "%') ";

        DataTable dt = DataManager.ExecuteQuery(strConn, query, "autoname");
        return dt;
    }
    public DataTable GetSegDesc(string strName)
    {
        string strConn = DataManager.OraConnString();
        SqlConnection sqlCon = new SqlConnection(strConn);
        string query = "select seg_coa_desc from gl_seg_coa where lvl_code='02' and upper(seg_coa_desc) like upper('%" + strName + "%') ";

        DataTable dt = DataManager.ExecuteQuery(strConn, query, "autoname");
        return dt;
    }
    public DataTable GetCostDesc(string strName)
    {
        string strConn = DataManager.OraConnString();
        SqlConnection sqlCon = new SqlConnection(strConn);
        string query = "select seg_coa_desc from gl_seg_coa where lvl_code='03' and upper(seg_coa_desc) like upper('%" + strName + "%') ";

        DataTable dt = DataManager.ExecuteQuery(strConn, query, "autoname");
        return dt;
    }
    [WebMethod]
    public string[] GetExeistCourse(string prefixText, int count)
    {
        if (count == 0)
        {
            count = 10;
        }
        string strConn = DataManager.OraConnString();
        SqlConnection sqlCon = new SqlConnection(strConn);
        string query = @"select FacultyId+'-'+FacultyName as search from Faculty_Info where (FacultyId+'-'+FacultyName) LIKE('%" + prefixText + "%')";
        DataTable dt = DataManager.ExecuteQuery(strConn, query, "autoname");
        List<string> items = new List<string>(count);
        for (int i = 0; i < dt.Rows.Count; i++)
        {

            string str = dt.Rows[i][0].ToString();

            items.Add(str);
        }

        return items.ToArray();
    }

    [WebMethod]
    public string[] GetStudentInfo(string prefixText, int count)
    {
        if (count == 0)
        {
            count = 10;
        }
        string strConn = DataManager.OraConnString();
        SqlConnection sqlCon = new SqlConnection(strConn);
        string query = @"select (Sl_Id +'-'+ NID+'-'+StdName+'-'+StdPhoneNo+'-'+FthName+'-'+MthName+'-'+CourseName) as Search from dbo.StudetnInfoMst t1 inner join  dbo.StudentInfoDtl t2 on t1.Id=t2.MstId inner join  dbo.Day_Information t3 on t1.Id=t3.StudentID where (Sl_Id +'-'+ NID+'-'+StdName+'-'+StdPhoneNo+'-'+FthName+'-'+MthName+'-'+CourseName) Like('%"+prefixText+"%') and t1.DeleteDate IS NULL";
        DataTable dt = DataManager.ExecuteQuery(strConn, query, "autoname");
        List<string> items = new List<string>(count);
        for (int i = 0; i < dt.Rows.Count; i++)
        {

            string str = dt.Rows[i][0].ToString();

            items.Add(str);
        }

        return items.ToArray();
    } 
}
