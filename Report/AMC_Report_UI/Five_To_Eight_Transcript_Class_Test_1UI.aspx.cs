using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using KHSC;
using System.Data;
using CrystalDecisions.CrystalReports.Engine;

public partial class Report_AMC_Report_UI_Five_To_Eight_Transcript_Class_Test_1UI : System.Web.UI.Page
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
                da.SelectCommand = new SqlCommand("SP_ResultCalculation1st_Five_ModelTest", conn);
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
                da.Fill(ds, "SP_ResultCalculation1st_Five_ModelTest");
                ds.Tables[0].TableName = "SP_ResultCalculation1st_Five_ModelTest";

                SqlDataAdapter da1 = new SqlDataAdapter();
                da1.SelectCommand = new SqlCommand("SP_ResultCalculation1st_Five_to_Eight_Model_Ma", conn);
                da1.SelectCommand.CommandType = CommandType.StoredProcedure;

                da1.SelectCommand.Parameters.AddWithValue("@class_year", Session["Year"]);
                da1.SelectCommand.Parameters.AddWithValue("@class", Session["Class_ID"]);
                da1.SelectCommand.Parameters.AddWithValue("@section", Session["Scetion"]);
                da1.SelectCommand.Parameters.AddWithValue("@ExamTitle", Session["Exam_Title"]);
                da1.SelectCommand.Parameters.AddWithValue("@verson", Session["version"]);
                da1.SelectCommand.Parameters.AddWithValue("@Shift", Session["Shift"]);
                da1.SelectCommand.Parameters.AddWithValue("@student_id", Session["Student_ID"]);


                DataSet ds1 = new DataSet();
                da1.Fill(ds1, "SP_ResultCalculation1st_Five_to_Eight_Model_Ma");
                //  DataTable table = ds1.Tables["tableName"];
                ds1.Tables[0].TableName = "SP_ResultCalculation1st_Five_to_Eight_Model_Ma";




                ReportDocument crystalReport = new ReportDocument(); // creating object of crystal report

                string reportPath = Server.MapPath("~/Report/AMC_Report_UI/Five_Model_Test_Transcript.rpt");
                crystalReport.Load(reportPath);
                crystalReport.SetDataSource(ds.Tables[0]);
                crystalReport.Subreports[0].SetDataSource(ds1.Tables[0]);
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
                da.SelectCommand = new SqlCommand("SP_ResultCalculation1st_Five_ModelTest", conn);
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
                da.Fill(ds, "SP_ResultCalculation1st_Five_ModelTest");
                ds.Tables[0].TableName = "SP_ResultCalculation1st_Five_ModelTest";

                SqlDataAdapter da1 = new SqlDataAdapter();
                da1.SelectCommand = new SqlCommand("SP_ResultCalculation1st_Five_to_Eight_Model_Ma", conn);
                da1.SelectCommand.CommandType = CommandType.StoredProcedure;

                da1.SelectCommand.Parameters.AddWithValue("@class_year", Session["Year"]);
                da1.SelectCommand.Parameters.AddWithValue("@class", Session["Class_ID"]);
                da1.SelectCommand.Parameters.AddWithValue("@section", Session["Scetion"]);
                da1.SelectCommand.Parameters.AddWithValue("@ExamTitle", Session["Exam_Title"]);
                da1.SelectCommand.Parameters.AddWithValue("@verson", Session["version"]);
                da1.SelectCommand.Parameters.AddWithValue("@Shift", Session["Shift"]);
                da1.SelectCommand.Parameters.AddWithValue("@student_id", null);


                DataSet ds1 = new DataSet();
                da1.Fill(ds1, "SP_ResultCalculation1st_Five_to_Eight_Model_Ma");
                //  DataTable table = ds1.Tables["tableName"];
                ds1.Tables[0].TableName = "SP_ResultCalculation1st_Five_to_Eight_Model_Ma";

                 



                ReportDocument crystalReport = new ReportDocument(); // creating object of crystal report

                string reportPath = Server.MapPath("~/Report/AMC_Report_UI/Five_Model_Test_Transcript.rpt");
                crystalReport.Load(reportPath);
                crystalReport.SetDataSource(ds.Tables[0]);
                crystalReport.Subreports[0].SetDataSource(ds1.Tables[0]);
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
