using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using CrystalDecisions.CrystalReports.Engine;
using KHSC;
using KHSC.Manager.Others;
 

public partial class Report_UI_ModelTestExamReport_Eight : System.Web.UI.Page
{
    SqlConnection conn = new SqlConnection(DataManager.OraConnString());
    ExamTitleManager aExamTitleManager = new ExamTitleManager();
    public static string FamSt = "N";
    public static Permis per;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CrystalReportViewer1.RefreshReport();
            if (IdTextTextBox.Text != "" && YearTextBox.Text == "")
            {
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = new SqlCommand("SP_ResultCalculation1st_EightModelTest_TransCript", conn);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.AddWithValue("@student_id", IdTextTextBox.Text);
                da.SelectCommand.Parameters.AddWithValue("@class_year", YearTextBox.Text);

                DataSet ds = new DataSet();
                da.Fill(ds, "tableName");
                ds.Tables[0].TableName = "tableName";
                // return ds.Tables[0];
                ReportDocument crystalReport = new ReportDocument(); // creating object of crystal report

                string reportPath = Server.MapPath("~/Report/AMC_Report_UI/ModelTestExamReportForTranscript.rpt");
                crystalReport.Load(reportPath);
                crystalReport.SetDataSource(ds.Tables[0]); // binding datatable
                crystalReport.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Response, true, "Accademic Report");
                conn.Close();

            }
        }
    }
    protected void CrystalReportViewer1_Load(object sender, EventArgs e)
    {
        try
        {
            conn.Open();

            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = new SqlCommand("SP_ResultCalculation1st_EightModelTest_TransCript", conn);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            string year = Session["Year"].ToString();
            da.SelectCommand.Parameters.AddWithValue("@class_year", Session["Year"]);
            da.SelectCommand.Parameters.AddWithValue("@class", Session["Class_ID"]);
            da.SelectCommand.Parameters.AddWithValue("@section", Session["Scetion"]);
            da.SelectCommand.Parameters.AddWithValue("@ExamTitle", Session["Exam_Title"]);
            da.SelectCommand.Parameters.AddWithValue("@verson", Session["version"]);
            da.SelectCommand.Parameters.AddWithValue("@Shift", Session["shift"]);
            da.SelectCommand.Parameters.AddWithValue("@student_id", null);


            DataSet ds = new DataSet();
            da.Fill(ds, "SP_ResultCalculation1st_EightModelTest_TransCript");
            ds.Tables[0].TableName = "SP_ResultCalculation1st_EightModelTest_TransCript";

            SqlDataAdapter da1 = new SqlDataAdapter();
            da1.SelectCommand = new SqlCommand("SP_ResultCalculation1st_EightModelTest_TransCript_Ma", conn);
            da1.SelectCommand.CommandType = CommandType.StoredProcedure;

            da1.SelectCommand.Parameters.AddWithValue("@class_year", Session["Year"]);
            da1.SelectCommand.Parameters.AddWithValue("@class", Session["Class_ID"]);
            da1.SelectCommand.Parameters.AddWithValue("@section", Session["Scetion"]);
            da1.SelectCommand.Parameters.AddWithValue("@ExamTitle", Session["Exam_Title"]);
            da1.SelectCommand.Parameters.AddWithValue("@verson", Session["version"]);
            da1.SelectCommand.Parameters.AddWithValue("@Shift", Session["Shift"]);
            da1.SelectCommand.Parameters.AddWithValue("@student_id", null);


            DataSet ds1 = new DataSet();
            da1.Fill(ds1, "SP_ResultCalculation1st_EightModelTest_TransCript_Ma");
            //  DataTable table = ds1.Tables["tableName"];
            ds1.Tables[0].TableName = "SP_ResultCalculation1st_EightModelTest_TransCript_Ma";

            

            ReportDocument crystalReport = new ReportDocument(); // creating object of crystal report

            string reportPath = Server.MapPath("~/Report/AMC_Report_UI/ModelTestExamReportForTranscript.rpt");
            crystalReport.Load(reportPath);
            crystalReport.SetDataSource(ds.Tables[0]);
            crystalReport.Subreports[0].SetDataSource(ds1.Tables[0]); 
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
    protected void refreshButton_Click(object sender, EventArgs e)
    {
        YearTextBox.Text = DateTime.Now.Year.ToString() + '-';
        IdTextTextBox.Text = "";
        CrystalReportViewer1.RefreshReport();
    }
}