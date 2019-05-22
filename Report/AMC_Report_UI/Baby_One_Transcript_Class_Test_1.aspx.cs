using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using System.Data;
using System.Data.SqlClient;
using KHSC.Manager.Others;
using KHSC;

public partial class Report_AMC_Report_UI_Baby_One_Transcript_Class_Test_1 : System.Web.UI.Page
{

    SqlConnection conn = new SqlConnection(DataManager.OraConnString());
    
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void CrystalReportViewer1_Load(object sender, EventArgs e)
    {
        if (Session["Student_ID"] != "")
        {
            try
            {
                conn.Open();

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = new SqlCommand("SP_MT_PG", conn);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                string year = Session["Year"].ToString();
                da.SelectCommand.Parameters.AddWithValue("@class_year", Session["Year"]);
                da.SelectCommand.Parameters.AddWithValue("@class", Session["Class_ID"]);
                da.SelectCommand.Parameters.AddWithValue("@section", Session["Scetion"]);
                da.SelectCommand.Parameters.AddWithValue("@ExamTitle", Session["Exam_Title"]);
                da.SelectCommand.Parameters.AddWithValue("@verson", Session["version"]);
                da.SelectCommand.Parameters.AddWithValue("@Shift", Session["Shift"]);
                da.SelectCommand.Parameters.AddWithValue("@student_id", Session["Student_ID"]);
                DataSet ds = new DataSet();
                da.Fill(ds, "SP_MT_PG");
                ds.Tables[0].TableName = "SP_MT_PG";

                ReportDocument crystalReport = new ReportDocument(); // creating object of crystal report
                string reportPath = Server.MapPath("~/Report/AMC_Report_UI/Baby_One_Transcript_Class_Test_1.rpt");
                crystalReport.Load(reportPath);
                crystalReport.SetDataSource(ds.Tables[0]);
                crystalReport.Subreports[0].SetDataSource(ds.Tables[0]);
               // binding datatable
                //crystalReport.Subreports[1].SetDataSource(ds2.Tables[0]);
                crystalReport.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Response, true, "Final Accademic Report");

            }
            catch (Exception ex)
            {
                //throw new Exception(ex.Message);
            }
            finally
            {
                conn.Close();

            }
        }
        else
        {
            try
            {
                conn.Open();

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = new SqlCommand("SP_MT_PG", conn);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                string year = Session["Year"].ToString();
                da.SelectCommand.Parameters.AddWithValue("@class_year", Session["Year"]);
                da.SelectCommand.Parameters.AddWithValue("@class", Session["Class_ID"]);
                da.SelectCommand.Parameters.AddWithValue("@verson", Session["version"]);
                da.SelectCommand.Parameters.AddWithValue("@section", Session["Scetion"]);
                da.SelectCommand.Parameters.AddWithValue("@ExamTitle", Session["Exam_Title"]);
                da.SelectCommand.Parameters.AddWithValue("@Shift", Session["Shift"]);
                da.SelectCommand.Parameters.AddWithValue("@student_id", null);


                DataSet ds = new DataSet();
                da.Fill(ds, "SP_MT_PG");
                ds.Tables[0].TableName = "SP_MT_PG";

                 



                ReportDocument crystalReport = new ReportDocument(); // creating object of crystal report

                string reportPath = Server.MapPath("~/Report/AMC_Report_UI/Baby_One_Transcript_Class_Test_1.rpt");
                crystalReport.Load(reportPath);
                crystalReport.SetDataSource(ds.Tables[0]);
                crystalReport.Subreports[0].SetDataSource(ds.Tables[0]);
                 // binding datatable
                //crystalReport.Subreports[1].SetDataSource(ds2.Tables[0]);

                crystalReport.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Response, true, "Final Accademic Report");

            }
            catch (Exception ex)
            {
                //throw new Exception(ex.Message);
            }
            finally
            {
                conn.Close();

            }
        }
    
    }
}