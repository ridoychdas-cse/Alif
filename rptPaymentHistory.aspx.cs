using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KHSC;
using System.Data.SqlClient;
using System.Data;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using iTextSharp.text.pdf.draw;

public partial class rptPaymentHistory : System.Web.UI.Page
{
    public static Permis per;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["user"] == null)
        {
            if (Session.SessionID != "" | Session.SessionID != null)
            {
                clsSession ses = clsSessionManager.getSession(Session.SessionID);
                if (ses != null)
                {
                    Session["user"] = ses.UserId;
                    Session["book"] = "AMB";
                    string connectionString = DataManager.OraConnString();
                    SqlDataReader dReader;
                    SqlConnection conn = new SqlConnection();
                    conn.ConnectionString = connectionString;
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "Select user_grp,description from utl_userinfo where upper(user_name)=upper('" + Session["user"].ToString().ToUpper() + "') and status='A'";
                    conn.Open();
                    dReader = cmd.ExecuteReader();
                    string wnot = "";
                    if (dReader.HasRows == true)
                    {
                        while (dReader.Read())
                        {
                            Session["userlevel"] = int.Parse(dReader["user_grp"].ToString());
                            wnot = dReader["description"].ToString();
                        }
                        Session["wnote"] = wnot;

                        cmd = new SqlCommand();
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "Select book_desc,company_address1,company_address2,separator_type from gl_set_of_books where book_name='" + Session["book"].ToString() + "' ";
                        if (dReader.IsClosed == false)
                        {
                            dReader.Close();
                        }
                        dReader = cmd.ExecuteReader();
                        if (dReader.HasRows == true)
                        {
                            while (dReader.Read())
                            {
                                Session["septype"] = dReader["separator_type"].ToString();
                                Session["org"] = dReader["book_desc"].ToString();
                                Session["add1"] = dReader["company_address1"].ToString();
                                Session["add2"] = dReader["company_address2"].ToString();
                            }
                        }
                    }
                    dReader.Close();
                    conn.Close();
                }
            }
        }
        try
        {
            string pageName = DataManager.GetCurrentPageName();
            string modid = PermisManager.getModuleId(pageName);
            per = PermisManager.getUsrPermis(Session["user"].ToString().Trim().ToUpper(), modid);
            if (per != null && per.AllowView == "Y")
            {
                ((Label)Page.Master.FindControl("lblLogin")).Text = Session["wnote"].ToString();
                ((LinkButton)Page.Master.FindControl("lbLogout")).Visible = true;
            }
            else
            {
                Response.Redirect("Default.aspx?sid=sam");
            }
        }
        catch
        {
            Response.Redirect("Default.aspx?sid=sam");
        }
        if (!IsPostBack)
        {
            try
            {
                DataTable dt = StudentManager.GetStudentAllCurrentStatus("","","","");
                dgPayHistory.DataSource = dt;
                dgPayHistory.DataBind();

            }
            catch (FormatException fex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('" + fex.Message + "');", true);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Database"))
                    ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Database Maintain Error. Contact to the Software Provider..!!');", true);
                else
                    ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('There is some problem to do the task. Try again properly.!!');", true);
            }
        }
    }
    protected void btnRunReport_Click(object sender, EventArgs e)
    {
        try{

       

        if (ddlReportType.SelectedValue == "CR")
        {
            CollectionReport();
        }
        else
        {
            RunReport();
        }

        }
        catch (FormatException fex)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('" + fex.Message + "');", true);
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("Database"))
                ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Database Maintain Error. Contact to the Software Provider..!!');", true);
            else
                ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('There is some problem to do the task. Try again properly.!!');", true);
        }
    }

    private void RunReport()
    {
        DataTable dt = StudentManager.GetStudentAllCurrentStatus("",txtSearchBatch.Text,"","");
        Response.Clear();
        Response.ContentType = "application/pdf";
        Response.AddHeader("content-disposition", "attachment; filename=studentpaymenthistory.pdf");
        Document document = new Document();
        document = new Document(PageSize.A4, 50f, 50f, 20f, 20f);

        MemoryStream ms = new MemoryStream();
        PdfWriter writer = PdfWriter.GetInstance(document, Response.OutputStream);
        pdfPage page = new pdfPage();
        writer.PageEvent = page;
        document.Open();

        byte[] logo = GlBookManager.GetGlLogo(Session["book"].ToString());
        iTextSharp.text.Image gif = iTextSharp.text.Image.GetInstance(logo);
        gif.Alignment = iTextSharp.text.Image.MIDDLE_ALIGN;
        gif.ScalePercent(35f);
        float[] titwidth = new float[2] { 5, 200 };

        PdfPCell cell;
        PdfPTable dth = new PdfPTable(titwidth);
        dth.WidthPercentage = 100;
        cell = new PdfPCell(gif);
        cell.HorizontalAlignment = 1;
        cell.VerticalAlignment = 1;
        cell.Rowspan = 4;
        cell.BorderWidth = 0f;
        dth.AddCell(cell);
        cell =
            new PdfPCell(new Phrase(Session["org"].ToString(), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12, iTextSharp.text.Font.BOLD)));
        cell.HorizontalAlignment = 1;
        cell.VerticalAlignment = 1;
        cell.BorderWidth = 0f;
        //cell.FixedHeight = 20f;
        dth.AddCell(cell);
        cell = new PdfPCell(new Phrase(Session["add1"].ToString(), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL)));
        cell.HorizontalAlignment = 1;
        cell.VerticalAlignment = 1;
        cell.BorderWidth = 0f;
        //cell.FixedHeight = 20f;
        dth.AddCell(cell);
        cell = new PdfPCell(new Phrase(Session["add2"].ToString(), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL)));
        cell.HorizontalAlignment = 1;
        cell.VerticalAlignment = 1;
        cell.BorderWidth = 0f;
        //cell.FixedHeight = 20f;
        dth.AddCell(cell);
        cell = new PdfPCell(new Phrase("Report of Student Payment Collection From " + txtStartDate.Text + "-" + txtEndDate.Text, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 11, iTextSharp.text.Font.BOLD)));
        cell.HorizontalAlignment = 1;
        cell.VerticalAlignment = 1;
        cell.BorderWidth = 0f;

        dth.AddCell(cell);
        document.Add(dth);
        //cell = new PdfPCell(dth);
        //cell.BorderWidth = 0f;
        //pdt.AddCell(cell);
        //LineSeparator line = new LineSeparator(0f, 100, null, Element.ALIGN_CENTER, -2);
        //cell = new PdfPCell(line);
        //pdt.AddCell(cell);
        //document.Add(line);
        PdfPTable dtempty = new PdfPTable(1);
        cell = new PdfPCell(FormatHeaderPhrase(""));
        cell.BorderWidth = 0f;
        cell.FixedHeight = 15f;
        dtempty.AddCell(cell);
        document.Add(dtempty);
        PdfPTable pdtn = new PdfPTable(2);
        pdtn.WidthPercentage = 100;

        float[] width = new float[10] { 4, 15, 25, 10, 8, 8, 8, 9, 8, 8 };
        PdfPTable pdtc = new PdfPTable(width);
        pdtc.WidthPercentage = 100;
        cell = new PdfPCell(new Phrase("SL", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL)));
        cell.HorizontalAlignment = 1;
        cell.VerticalAlignment = 1;
        pdtc.AddCell(cell);
        cell = new PdfPCell(new Phrase("Student Id", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL)));
        cell.HorizontalAlignment = 1;
        cell.VerticalAlignment = 1;
        pdtc.AddCell(cell);
        cell = new PdfPCell(new Phrase("Name", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL)));
        cell.HorizontalAlignment = 1;
        cell.VerticalAlignment = 1;
        pdtc.AddCell(cell);
        cell = new PdfPCell(new Phrase("Date", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL)));
        cell.HorizontalAlignment = 1;
        cell.VerticalAlignment = 1;
        pdtc.AddCell(cell);
        cell = new PdfPCell(new Phrase("Batch", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL)));
        cell.HorizontalAlignment = 1;
        cell.VerticalAlignment = 1;
        pdtc.AddCell(cell);
        cell = new PdfPCell(new Phrase("Pay Amount", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL)));
        cell.HorizontalAlignment = 1;
        cell.VerticalAlignment = 1;
        pdtc.AddCell(cell);

        cell = new PdfPCell(new Phrase("Waiver", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL)));
        cell.HorizontalAlignment = 1;
        cell.VerticalAlignment = 1;
        pdtc.AddCell(cell);

        cell = new PdfPCell(new Phrase("Discount", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL)));
        cell.HorizontalAlignment = 1;
        cell.VerticalAlignment = 1;
        pdtc.AddCell(cell);

        cell = new PdfPCell(new Phrase("Paid Amount", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL)));
        cell.HorizontalAlignment = 1;
        cell.VerticalAlignment = 1;
        pdtc.AddCell(cell);
        cell = new PdfPCell(new Phrase("Due Amount", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL)));
        cell.HorizontalAlignment = 1;
        cell.VerticalAlignment = 1;
        pdtc.AddCell(cell);


        for (int i = 0; i < dt.Rows.Count; i++)
        {
            cell = new PdfPCell(new Phrase((i + 1).ToString(), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
            cell.HorizontalAlignment = 1;
            cell.VerticalAlignment = 1;
            pdtc.AddCell(cell);
            cell = new PdfPCell(new Phrase(((DataRow)dt.Rows[i])["student_id"].ToString(), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
            cell.HorizontalAlignment = 1;
            cell.VerticalAlignment = 1;
            pdtc.AddCell(cell);

            cell = new PdfPCell(new Phrase(((DataRow)dt.Rows[i])["f_name"].ToString(), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
            cell.HorizontalAlignment = 1;
            cell.VerticalAlignment = 1;
            pdtc.AddCell(cell);
            cell = new PdfPCell(new Phrase(((DataRow)dt.Rows[i])["pay_date"].ToString(), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
            cell.HorizontalAlignment = 1;
            cell.VerticalAlignment = 1;
            pdtc.AddCell(cell);
            cell = new PdfPCell(new Phrase(((DataRow)dt.Rows[i])["BatchNo"].ToString(), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
            cell.HorizontalAlignment = 1;
            cell.VerticalAlignment = 1;
            pdtc.AddCell(cell);
            cell = new PdfPCell(new Phrase(((DataRow)dt.Rows[i])["PaidAmount"].ToString(), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
            cell.HorizontalAlignment = 2;
            cell.VerticalAlignment = 1;
            pdtc.AddCell(cell);
            cell = new PdfPCell(new Phrase(((DataRow)dt.Rows[i])["Waiver"].ToString(), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
            cell.HorizontalAlignment = 2;
            cell.VerticalAlignment = 1;
            pdtc.AddCell(cell);
            cell = new PdfPCell(new Phrase(((DataRow)dt.Rows[i])["Discount"].ToString(), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
            cell.HorizontalAlignment = 2;
            cell.VerticalAlignment = 1;
            pdtc.AddCell(cell);
            cell = new PdfPCell(new Phrase(((DataRow)dt.Rows[i])["PayAmount"].ToString(), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
            cell.HorizontalAlignment = 2;
            cell.VerticalAlignment = 1;
            pdtc.AddCell(cell);
            cell = new PdfPCell(new Phrase(((DataRow)dt.Rows[i])["DueAmount"].ToString(), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
            cell.HorizontalAlignment = 2;
            cell.VerticalAlignment = 1;
            pdtc.AddCell(cell);

        }


        document.Add(pdtc);
        document.Close();
    }
    private void CollectionReport()
    {
        DataTable dt = StudentManager.GetStudentAllCurrentStatus("", txtSearchBatch.Text, "", "");
        Response.Clear();
        Response.ContentType = "application/pdf";
        Response.AddHeader("content-disposition", "attachment; filename=studentpaymenthistory.pdf");
        Document document = new Document();
        document = new Document(PageSize.A4, 50f, 50f, 20f, 20f);

        MemoryStream ms = new MemoryStream();
        PdfWriter writer = PdfWriter.GetInstance(document, Response.OutputStream);
        pdfPage page = new pdfPage();
        writer.PageEvent = page;
        document.Open();

        byte[] logo = GlBookManager.GetGlLogo(Session["book"].ToString());
        iTextSharp.text.Image gif = iTextSharp.text.Image.GetInstance(logo);
        gif.Alignment = iTextSharp.text.Image.MIDDLE_ALIGN;
        gif.ScalePercent(35f);
        float[] titwidth = new float[2] { 5, 200 };

        PdfPCell cell;
        PdfPTable dth = new PdfPTable(titwidth);
        dth.WidthPercentage = 100;
        cell = new PdfPCell(gif);
        cell.HorizontalAlignment = 1;
        cell.VerticalAlignment = 1;
        cell.Rowspan = 4;
        cell.BorderWidth = 0f;
        dth.AddCell(cell);
        cell =
            new PdfPCell(new Phrase(Session["org"].ToString(), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12, iTextSharp.text.Font.BOLD)));
        cell.HorizontalAlignment = 1;
        cell.VerticalAlignment = 1;
        cell.BorderWidth = 0f;
        //cell.FixedHeight = 20f;
        dth.AddCell(cell);
        cell = new PdfPCell(new Phrase(Session["add1"].ToString(), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL)));
        cell.HorizontalAlignment = 1;
        cell.VerticalAlignment = 1;
        cell.BorderWidth = 0f;
        //cell.FixedHeight = 20f;
        dth.AddCell(cell);
        cell = new PdfPCell(new Phrase(Session["add2"].ToString(), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL)));
        cell.HorizontalAlignment = 1;
        cell.VerticalAlignment = 1;
        cell.BorderWidth = 0f;
        //cell.FixedHeight = 20f;
        dth.AddCell(cell);
        cell = new PdfPCell(new Phrase("Report of Student Payment Collection From " + txtStartDate.Text + "-" + txtEndDate.Text, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 11, iTextSharp.text.Font.BOLD)));
        cell.HorizontalAlignment = 1;
        cell.VerticalAlignment = 1;
        cell.BorderWidth = 0f;

        dth.AddCell(cell);
        document.Add(dth);
        //cell = new PdfPCell(dth);
        //cell.BorderWidth = 0f;
        //pdt.AddCell(cell);
        //LineSeparator line = new LineSeparator(0f, 100, null, Element.ALIGN_CENTER, -2);
        //cell = new PdfPCell(line);
        //pdt.AddCell(cell);
        //document.Add(line);
        PdfPTable dtempty = new PdfPTable(1);
        cell = new PdfPCell(FormatHeaderPhrase(""));
        cell.BorderWidth = 0f;
        cell.FixedHeight = 15f;
        dtempty.AddCell(cell);
        document.Add(dtempty);
        PdfPTable pdtn = new PdfPTable(2);
        pdtn.WidthPercentage = 100;

        float[] width = new float[10] { 4, 15, 25, 10, 8, 8, 8, 9, 8, 8 };
        PdfPTable pdtc = new PdfPTable(width);
        pdtc.WidthPercentage = 100;
        cell = new PdfPCell(new Phrase("SL", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL)));
        cell.HorizontalAlignment = 1;
        cell.VerticalAlignment = 1;
        pdtc.AddCell(cell);
        cell = new PdfPCell(new Phrase("Student Id", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL)));
        cell.HorizontalAlignment = 1;
        cell.VerticalAlignment = 1;
        pdtc.AddCell(cell);
        cell = new PdfPCell(new Phrase("Name", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL)));
        cell.HorizontalAlignment = 1;
        cell.VerticalAlignment = 1;
        pdtc.AddCell(cell);
        cell = new PdfPCell(new Phrase("Date", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL)));
        cell.HorizontalAlignment = 1;
        cell.VerticalAlignment = 1;
        pdtc.AddCell(cell);
        cell = new PdfPCell(new Phrase("Batch", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL)));
        cell.HorizontalAlignment = 1;
        cell.VerticalAlignment = 1;
        pdtc.AddCell(cell);
        cell = new PdfPCell(new Phrase("Pay Amount", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL)));
        cell.HorizontalAlignment = 1;
        cell.VerticalAlignment = 1;
        pdtc.AddCell(cell);

        cell = new PdfPCell(new Phrase("Waiver", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL)));
        cell.HorizontalAlignment = 1;
        cell.VerticalAlignment = 1;
        pdtc.AddCell(cell);

        cell = new PdfPCell(new Phrase("Discount", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL)));
        cell.HorizontalAlignment = 1;
        cell.VerticalAlignment = 1;
        pdtc.AddCell(cell);

        cell = new PdfPCell(new Phrase("Acctual Paid Amount", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL)));
        cell.HorizontalAlignment = 1;
        cell.VerticalAlignment = 1;
        pdtc.AddCell(cell);
        cell = new PdfPCell(new Phrase("Due Amount", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL)));
        cell.HorizontalAlignment = 1;
        cell.VerticalAlignment = 1;
        pdtc.AddCell(cell);


        for (int i = 0; i < dt.Rows.Count; i++)
        {
            cell = new PdfPCell(new Phrase((i + 1).ToString(), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
            cell.HorizontalAlignment = 1;
            cell.VerticalAlignment = 1;
            pdtc.AddCell(cell);
            cell = new PdfPCell(new Phrase(((DataRow)dt.Rows[i])["student_id"].ToString(), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
            cell.HorizontalAlignment = 1;
            cell.VerticalAlignment = 1;
            pdtc.AddCell(cell);

            cell = new PdfPCell(new Phrase(((DataRow)dt.Rows[i])["f_name"].ToString(), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
            cell.HorizontalAlignment = 1;
            cell.VerticalAlignment = 1;
            pdtc.AddCell(cell);
            cell = new PdfPCell(new Phrase(((DataRow)dt.Rows[i])["pay_date"].ToString(), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
            cell.HorizontalAlignment = 1;
            cell.VerticalAlignment = 1;
            pdtc.AddCell(cell);
            cell = new PdfPCell(new Phrase(((DataRow)dt.Rows[i])["BatchNo"].ToString(), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
            cell.HorizontalAlignment = 1;
            cell.VerticalAlignment = 1;
            pdtc.AddCell(cell);
            cell = new PdfPCell(new Phrase(((DataRow)dt.Rows[i])["PaidAmount"].ToString(), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
            cell.HorizontalAlignment = 2;
            cell.VerticalAlignment = 1;
            pdtc.AddCell(cell);
            cell = new PdfPCell(new Phrase(((DataRow)dt.Rows[i])["Waiver"].ToString(), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
            cell.HorizontalAlignment = 2;
            cell.VerticalAlignment = 1;
            pdtc.AddCell(cell);
            cell = new PdfPCell(new Phrase(((DataRow)dt.Rows[i])["Discount"].ToString(), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
            cell.HorizontalAlignment = 2;
            cell.VerticalAlignment = 1;
            pdtc.AddCell(cell);
            cell = new PdfPCell(new Phrase(((DataRow)dt.Rows[i])["PayAmount"].ToString(), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
            cell.HorizontalAlignment = 2;
            cell.VerticalAlignment = 1;
            pdtc.AddCell(cell);
            cell = new PdfPCell(new Phrase(((DataRow)dt.Rows[i])["DueAmount"].ToString(), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
            cell.HorizontalAlignment = 2;
            cell.VerticalAlignment = 1;
            pdtc.AddCell(cell);

        }
        decimal tot = decimal.Zero;
        decimal tot1 = decimal.Zero;
        decimal tot2 = decimal.Zero;
        decimal tot3 = decimal.Zero;
        decimal tot5 = decimal.Zero;

        foreach (DataRow dr in dt.Rows)
        {
            
                tot += decimal.Parse(dr["PaidAmount"].ToString());

                tot1 += decimal.Parse(dr["Waiver"].ToString());
                tot5 += decimal.Parse(dr["Discount"].ToString());
                tot2 += decimal.Parse(dr["PayAmount"].ToString());
                tot3 += decimal.Parse(dr["DueAmount"].ToString());              
            
        }
        cell = new PdfPCell(new Phrase("Total", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.BOLD)));
        cell.HorizontalAlignment = 2;
        cell.VerticalAlignment = 1;
        cell.Colspan = 5;
        pdtc.AddCell(cell);
        cell = new PdfPCell(new Phrase(tot.ToString("N2"), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.BOLD)));
        cell.HorizontalAlignment = 2;
        cell.VerticalAlignment = 1;
        pdtc.AddCell(cell);

        cell = new PdfPCell(new Phrase(tot1.ToString("N2"), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.BOLD)));
        cell.HorizontalAlignment = 2;
        cell.VerticalAlignment = 1;
        pdtc.AddCell(cell);

        cell = new PdfPCell(new Phrase(tot5.ToString("N2"), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.BOLD)));
        cell.HorizontalAlignment = 2;
        cell.VerticalAlignment = 1;
        pdtc.AddCell(cell);

        cell = new PdfPCell(new Phrase(tot2.ToString("N2"), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.BOLD)));
        cell.HorizontalAlignment = 2;
        cell.VerticalAlignment = 1;
        pdtc.AddCell(cell);

        cell = new PdfPCell(new Phrase(tot3.ToString("N2"), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.BOLD)));
        cell.HorizontalAlignment = 2;
        cell.VerticalAlignment = 1;
        pdtc.AddCell(cell);    

        document.Add(pdtc);
        document.Close();
    }

    private static Phrase FormatPhrase(string value)
    {
        return new Phrase(value, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10));
    }
    private static Phrase FormatHeaderPhrase(string value)
    {
        return new Phrase(value, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12, iTextSharp.text.Font.BOLD));
    }
    protected void btnShowReport_Click(object sender, EventArgs e)
    {
        try{
        if (ddlReportType.SelectedValue == "")
        {

        }

        DataTable dt = StudentManager.GetStudentAllCurrentStatus("",txtSearchBatch.Text,"","");
        dgPayHistory.DataSource = dt;
        dgPayHistory.DataBind();
        Up1.Update();

        }
        catch (FormatException fex)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('" + fex.Message + "');", true);
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("Database"))
                ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Database Maintain Error. Contact to the Software Provider..!!');", true);
            else
                ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('There is some problem to do the task. Try again properly.!!');", true);
        }
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
        DataTable dt = StudentManager.GetStudentAllCurrentStatus("", "","","");
        dgPayHistory.DataSource = dt;
        dgPayHistory.DataBind();
        Refresh();
        }
        catch (FormatException fex)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('" + fex.Message + "');", true);
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("Database"))
                ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Database Maintain Error. Contact to the Software Provider..!!');", true);
            else
                ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('There is some problem to do the task. Try again properly.!!');", true);
        }
    }
    protected void ddlReportType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlReportType.SelectedValue == "DWR")
        {
            txtEndDate.Visible = true;
            txtStartDate.Visible = true;
            Label2.Visible = true;
            Label3.Visible = true;
            txtSearchBatch.Visible = false;
            Label2.Text = "Start Date";
        }
        if (ddlReportType.SelectedValue == "BWR")
        {
            txtEndDate.Visible = false;
            txtStartDate.Visible = false;
            Label2.Visible = true;
            Label3.Visible = false;
            txtSearchBatch.Visible = true;
            Label2.Text = "Batch No";
        }
        if(ddlReportType.SelectedValue=="DWEL")
        {
            txtEndDate.Visible = true;
            txtStartDate.Visible = true;
            Label2.Visible = true;
            Label3.Visible = true;
            txtSearchBatch.Visible = false;
            Label2.Text = "Start Date";
        }
        if (ddlReportType.SelectedValue == "CR")
        {
            txtEndDate.Visible = true;
            txtStartDate.Visible = true;
            Label2.Visible = true;
            Label3.Visible = true;
            txtSearchBatch.Visible = false;
            Label2.Text = "Start Date";
        }
        else
        { }
        Up1.Update();
    }

    private void Refresh()
    {
        txtEndDate.Visible = false;
        txtStartDate.Visible = false;
        Label2.Visible = false;
        Label3.Visible = false;
        txtSearchBatch.Visible = false;
        ddlReportType.SelectedIndex = -1;
        txtSearchBatch.Text = "";
        txtEndDate.Text = "";
        txtStartDate.Text = "";
    }
}