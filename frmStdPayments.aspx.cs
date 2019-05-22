using System;
using System.Collections;
using System.Configuration;
using System.Data;

using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using KHSC;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html;
using iTextSharp.text.pdf.draw;
using System.Reflection;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Globalization;
using System.Drawing;
using System.Drawing.Printing;
public partial class frmStdPayments : System.Web.UI.Page
{
    public static Permis per;
    clsPaymentManager aclsPaymentManagerObj = new clsPaymentManager();
    
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
                            wnot = "Mems Mr. " + dReader["description"].ToString();
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
                
                txtPayDate.Attributes.Add("onBlur", "formatdate('" + txtPayDate.ClientID + "')");
                txtChequeDate.Attributes.Add("onBlur", "formatdate('" + txtChequeDate.ClientID + "')");
                getEmptyGrid();
                //txtPayDate.Focus();
                clearFields();
                ddlBankNo.Items.Clear();
                string queryBank = "select '' BANK_ID, '' BANK_NAME  union select BANK_ID,BANK_NAME from BANK_INFO order by 1";
                util.PopulationDropDownList(ddlBankNo, "Bank", queryBank, "BANK_NAME", "BANK_ID");                
                txtPayDate.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
                ddlPayMode.SelectedValue = "C";
                if (Session["BankID"] != null)
                {
                    ddlBankNo.SelectedValue = Session["BankID"].ToString();
                    ddlPayMode.SelectedValue = Session["PayType"].ToString();
                    ddlPayMode_SelectedIndexChanged(sender, e);
                }
                txtStudentId.Focus();
                //lblShiftNew.Text = "";
                btnSave.Enabled = true;
                Session["Flg"] = "0";
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
    private void getEmptyGrid()
    {
        dgStd.Visible = false;
        pnlPay.Visible = true;
        DataTable dt = new DataTable();
        dt.Columns.Add("pay_id", typeof(string));
        dt.Columns.Add("pay_head_id", typeof(string));
        dt.Columns.Add("pay_amt", typeof(string));
        dt.Columns.Add("Discount_AMT", typeof(string));
        dt.Columns.Add("OtCharge_flag", typeof(string));
        dt.Columns.Add("Fix_Amt", typeof(string));
        dt.Columns.Add("Previous_Pay", typeof(string));
        dt.Columns.Add("Prv_Discount_AMT", typeof(string));
        DataRow dr = dt.NewRow();
        dr["Discount_AMT"] = "0";
        dt.Rows.Add(dr);
        ViewState["paydtl"] = dt;
        dgPay.DataSource = dt;
        dgPay.DataBind();
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/frmStdPayments.aspx?mno=0.0");
    }
    protected void btnPrint_Click(object sender, EventArgs e)
    {

        //Session.Remove("rptbyte");
        PrintMoneyReciept();

        string strJS = ("<script type='text/javascript'>window.open('Default4.aspx','_blank');</script>");
        Page.ClientScript.RegisterStartupScript(this.GetType(), "strJSAlert", strJS);
    }

    private void PrintMoneyReciept()
    {
        try
        {

            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment; filename='StudentPaymentReport'.pdf");
            Student std = StudentManager.getStd(txtStudentId.Text);
            clsPaymentMst paymst = clsPaymentManager.getPaymentMst(txtPaymentId.Text);
            Document document = new Document();
            //float pwidth = (float)(25.7 / 2.54) * 72;
            //float pheight = (float)(22.7 / 2.54) * 72; 
            float pwidth = (float)(14 / 2.54) * 72;
            float pheight = (float)(20 / 2.54) * 72;
            document = new Document(new iTextSharp.text.Rectangle(pwidth, pheight));
            //document = new Document(PageSize.A4);
            //document = new Document(new Rectangle.A4.Rotate(), 90f, 90f, 35f, 20f);
            PdfWriter writer = PdfWriter.GetInstance(document, Response.OutputStream);
            MemoryStream ms = new MemoryStream();
            //pdfPage page1 = new pdfPage();
            // writer.PageEvent = page1;
            document.Open();
            string a = txtStudentId.Text.Trim();
            string b = txtPaymentId.Text.Trim();
            if (b != "")
            {
                if (txtStudentId.Text != "")
                {
                    byte[] logo = GlBookManager.GetGlLogo(Session["book"].ToString());
                    iTextSharp.text.Image gif = iTextSharp.text.Image.GetInstance(logo);
                    gif.Alignment = iTextSharp.text.Image.MIDDLE_ALIGN;
                    gif.ScalePercent(25f);
                    float[] titwidth = new float[2] { 5, 200 };
                    float[] width = new float[7] { 60, 5, 105, 10, 25, 5, 35 };
                    float[] swidth = new float[5] { 60, 20, 20, 20, 20 };
                    float[] swidth1 = new float[2] { 130, 70 };
                    float[] twidth = new float[3] { 100, 5, 100 };
                    float[] widFooter = new float[2] { 55, 45 };

                    decimal tot = decimal.Zero;
                    decimal tot3 = decimal.Zero;
                    decimal tot1 = decimal.Zero;
                    decimal tot2 = decimal.Zero;
                    decimal tott = decimal.Zero;
                    decimal tott3 = decimal.Zero;
                    decimal tott1 = decimal.Zero;
                    decimal tott2 = decimal.Zero;
                    decimal tot4 = decimal.Zero;
                    //  decimal totttt4 = decimal.Zero;
                    if (std != null)
                    {
                        for (int i = 1; i <= 2; i++)
                        {
                            PdfPCell cell;
                            PdfPTable dth = new PdfPTable(titwidth);
                            dth.WidthPercentage = 100;
                            cell = new PdfPCell(gif);
                            cell.HorizontalAlignment = 1;
                            cell.VerticalAlignment = 1;
                            cell.Rowspan = 3;
                            cell.BorderWidth = 0f;
                            dth.AddCell(cell);
                            cell = new PdfPCell(new Phrase(Session["org"].ToString(), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12, iTextSharp.text.Font.BOLD)));
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
                            //cell = new PdfPCell(new Phrase(Session["add2"].ToString(), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL)));
                            //cell.HorizontalAlignment = 1;
                            //cell.VerticalAlignment = 1;
                            //cell.BorderWidth = 0f;
                            ////cell.FixedHeight = 20f;
                            //dth.AddCell(cell);
                            cell = new PdfPCell(new Phrase("Student Payment Reciept", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 11, iTextSharp.text.Font.BOLD)));
                            cell.HorizontalAlignment = 1;
                            cell.VerticalAlignment = 1;
                            cell.BorderWidth = 0f;
                            cell.FixedHeight = 20f;
                            dth.AddCell(cell);

                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 11, iTextSharp.text.Font.BOLD)));
                            cell.HorizontalAlignment = 1;
                            cell.VerticalAlignment = 1;
                            cell.BorderWidth = 0f;
                            cell.FixedHeight = 20f;
                            dth.AddCell(cell);
                            if (i == 2)
                            {
                                cell = new PdfPCell(new Phrase("Student Copy", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 11, iTextSharp.text.Font.BOLD)));
                                cell.HorizontalAlignment = 1;
                                cell.VerticalAlignment = 1;
                                cell.BorderWidth = 0f;
                                cell.FixedHeight = 20f;
                                dth.AddCell(cell);

                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 11, iTextSharp.text.Font.BOLD)));
                                cell.HorizontalAlignment = 1;
                                cell.VerticalAlignment = 1;
                                cell.BorderWidth = 0f;
                                cell.FixedHeight = 20f;
                                dth.AddCell(cell);


                            }
                            else
                            {
                                cell = new PdfPCell(new Phrase("Office Copy", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 11, iTextSharp.text.Font.BOLD)));
                                cell.HorizontalAlignment = 1;
                                cell.VerticalAlignment = 1;
                                cell.BorderWidth = 0f;
                                cell.FixedHeight = 20f;
                                dth.AddCell(cell);

                                cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 11, iTextSharp.text.Font.BOLD)));
                                cell.HorizontalAlignment = 1;
                                cell.VerticalAlignment = 1;
                                cell.BorderWidth = 0f;
                                cell.FixedHeight = 20f;
                                dth.AddCell(cell);
                            }
                            document.Add(dth);
                            LineSeparator line = new LineSeparator(0f, 100, null, Element.ALIGN_CENTER, -2);
                            document.Add(line);
                            PdfPTable dtempty = new PdfPTable(1);
                            cell = new PdfPCell(FormatHeaderPhrase(""));
                            cell.BorderWidth = 0f;
                            cell.FixedHeight = 5f;
                            dtempty.AddCell(cell);
                            document.Add(dtempty);

                            PdfPTable dtm = new PdfPTable(width);
                            dtm.WidthPercentage = 100;
                            cell = new PdfPCell(new Phrase("MR No", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
                            cell.HorizontalAlignment = 3;
                            cell.VerticalAlignment = 1;
                            cell.BorderWidth = 0f;
                            cell.FixedHeight = 15f;
                            dtm.AddCell(cell);
                            cell = new PdfPCell(new Phrase(":", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
                            cell.HorizontalAlignment = 3;
                            cell.VerticalAlignment = 1;
                            cell.BorderWidth = 0f;
                            cell.FixedHeight = 15f;
                            dtm.AddCell(cell);
                            cell = new PdfPCell(new Phrase(txtPaymentId.Text, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
                            cell.HorizontalAlignment = 3;
                            cell.VerticalAlignment = 1;
                            cell.BorderWidth = 0f;
                            //cell.FixedHeight = 15f;
                            //cell.Colspan = 3;
                            dtm.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
                            cell.HorizontalAlignment = 3;
                            cell.VerticalAlignment = 1;
                            cell.BorderWidth = 0f;
                            //cell.FixedHeight = 15f;
                            dtm.AddCell(cell);
                            cell = new PdfPCell(new Phrase("Date", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
                            cell.HorizontalAlignment = 3;
                            cell.VerticalAlignment = 1;
                            cell.BorderWidth = 0f;
                            cell.FixedHeight = 15f;
                            dtm.AddCell(cell);
                            cell = new PdfPCell(new Phrase(":", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
                            cell.HorizontalAlignment = 3;
                            cell.VerticalAlignment = 1;
                            cell.BorderWidth = 0f;
                            //cell.FixedHeight = 15f;
                            dtm.AddCell(cell);
                            cell = new PdfPCell(new Phrase(txtPayDate.Text, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
                            cell.HorizontalAlignment = 3;
                            cell.VerticalAlignment = 1;
                            cell.BorderWidth = 0f;
                            //cell.FixedHeight = 15f;
                            dtm.AddCell(cell);
                            cell = new PdfPCell(new Phrase("Student ID & Name", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
                            cell.HorizontalAlignment = 0;
                            cell.VerticalAlignment = 1;
                            cell.BorderWidth = 0f;
                            cell.FixedHeight = 15f;
                            dtm.AddCell(cell);
                            cell = new PdfPCell(new Phrase(":", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
                            cell.HorizontalAlignment = 3;
                            cell.VerticalAlignment = 1;
                            cell.BorderWidth = 0f;
                            cell.FixedHeight = 15f;
                            dtm.AddCell(cell);
                            cell = new PdfPCell(new Phrase(std.StudentId + "-" + StudentManager.getStudentName(std.StudentId), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 7, iTextSharp.text.Font.NORMAL)));
                            cell.HorizontalAlignment = 0;
                            cell.VerticalAlignment = 1;
                            cell.BorderWidth = 0f;

                            dtm.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
                            cell.HorizontalAlignment = 3;
                            cell.VerticalAlignment = 1;
                            cell.BorderWidth = 0f;
                            cell.FixedHeight = 15f;
                            dtm.AddCell(cell);
                            cell = new PdfPCell(new Phrase("Class", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
                            cell.HorizontalAlignment = 3;
                            cell.VerticalAlignment = 1;
                            cell.BorderWidth = 0f;
                            cell.FixedHeight = 15f;
                            dtm.AddCell(cell);
                            cell = new PdfPCell(new Phrase(":", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
                            cell.HorizontalAlignment = 3;
                            cell.VerticalAlignment = 1;
                            cell.BorderWidth = 0f;
                            cell.FixedHeight = 15f;
                            dtm.AddCell(cell);
                            cell = new PdfPCell(new Phrase(paymst.ClassName, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
                            cell.HorizontalAlignment = 3;
                            cell.VerticalAlignment = 1;
                            cell.BorderWidth = 0f;
                            cell.FixedHeight = 15f;
                            dtm.AddCell(cell);

                            cell = new PdfPCell(new Phrase("Section", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
                            cell.HorizontalAlignment = 3;
                            cell.VerticalAlignment = 1;
                            cell.BorderWidth = 0f;
                            cell.FixedHeight = 15f;
                            dtm.AddCell(cell);
                            cell = new PdfPCell(new Phrase(":", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
                            cell.HorizontalAlignment = 3;
                            cell.VerticalAlignment = 1;
                            cell.BorderWidth = 0f;
                            cell.FixedHeight = 15f;
                            dtm.AddCell(cell);
                            cell = new PdfPCell(new Phrase(paymst.SectionName, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
                            cell.HorizontalAlignment = 3;
                            cell.VerticalAlignment = 1;
                            cell.BorderWidth = 0f;
                            cell.FixedHeight = 15f;
                            dtm.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
                            cell.HorizontalAlignment = 3;
                            cell.VerticalAlignment = 1;
                            cell.BorderWidth = 0f;
                            cell.FixedHeight = 15f;
                            dtm.AddCell(cell);
                            cell = new PdfPCell(new Phrase("Roll No.", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
                            cell.HorizontalAlignment = 3;
                            cell.VerticalAlignment = 1;
                            cell.BorderWidth = 0f;
                            cell.FixedHeight = 15f;
                            dtm.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
                            cell.HorizontalAlignment = 3;
                            cell.VerticalAlignment = 1;
                            cell.BorderWidth = 0f;
                            cell.FixedHeight = 15f;
                            dtm.AddCell(cell);
                            cell = new PdfPCell(new Phrase(lblRollName.Text, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
                            cell.HorizontalAlignment = 3;
                            cell.VerticalAlignment = 1;
                            cell.BorderWidth = 0f;
                            cell.FixedHeight = 15f;

                            dtm.AddCell(cell);
                            document.Add(dtm);
                            document.Add(dtempty);

                            PdfPTable pdtPay = new PdfPTable(swidth);
                            pdtPay.WidthPercentage = 100;

                            cell = new PdfPCell(new Phrase("Fee. Particulars", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.BOLD)));
                            cell.HorizontalAlignment = 3;
                            cell.VerticalAlignment = 1;
                            //cell.BorderColor = BaseColor.LIGHT_GRAY;
                            pdtPay.AddCell(cell);
                            cell = new PdfPCell(new Phrase("Payable Amount", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.BOLD)));
                            cell.HorizontalAlignment = 2;
                            cell.VerticalAlignment = 1;
                            //cell.BorderColor = BaseColor.LIGHT_GRAY;
                            pdtPay.AddCell(cell);
                            cell = new PdfPCell(new Phrase("Previous Paid", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.BOLD)));
                            cell.HorizontalAlignment = 2;
                            cell.VerticalAlignment = 1;
                            //cell.BorderColor = BaseColor.LIGHT_GRAY;
                            pdtPay.AddCell(cell);
                            cell = new PdfPCell(new Phrase("Discount Amount", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.BOLD)));
                            cell.HorizontalAlignment = 2;
                            cell.VerticalAlignment = 1;
                            //cell.BorderColor = BaseColor.LIGHT_GRAY;
                            pdtPay.AddCell(cell);
                            cell = new PdfPCell(new Phrase("Paid Amount", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.BOLD)));
                            cell.HorizontalAlignment = 2;
                            cell.VerticalAlignment = 1;
                            //cell.BorderColor = BaseColor.LIGHT_GRAY;
                            pdtPay.AddCell(cell);

                            DataTable dtPay = (DataTable)ViewState["paydtl"];
                            foreach (DataRow dr in dtPay.Rows)
                            {
                                double mon = 0;
                                double amt = 0;
                                string MonName = "";
                                if (dr["pay_amt"].ToString() != "")
                                {
                                    if (dr["OtCharge_flag"].ToString() == "M")
                                    {
                                        amt = IdManager.GetShowSingleValueCurrency("pay_amt", "pay_id", "payment_info", dr["pay_id"].ToString());
                                        mon = (Convert.ToDouble(dr["pay_amt"].ToString()) + Convert.ToDouble(dr["Previous_Pay"].ToString()) + Convert.ToDouble(dr["Discount_AMT"].ToString())) / amt;
                                        if (Math.Round(mon) == 0) { MonName = "Jan"; }
                                        else if (Math.Round(mon) == 1) { MonName = "Jan - Jan"; }
                                        else if (Math.Round(mon) == 2) { MonName = "Jan - Feb"; }
                                        else if (Math.Round(mon) == 3) { MonName = "Jan - Mar"; }
                                        else if (Math.Round(mon) == 4) { MonName = "Jan - Apr"; }
                                        else if (Math.Round(mon) == 5) { MonName = "Jan - May"; }
                                        else if (Math.Round(mon) == 6) { MonName = "Jan - Jun"; }
                                        else if (Math.Round(mon) == 7) { MonName = "Jan - Jul"; }
                                        else if (Math.Round(mon) == 8) { MonName = "Jan - Aug"; }
                                        else if (Math.Round(mon) == 9) { MonName = "Jan - Sep"; }
                                        else if (Math.Round(mon) == 10) { MonName = "Jan - Oct"; }
                                        else if (Math.Round(mon) == 11) { MonName = "Jan - Nov"; }
                                        else if (Math.Round(mon) == 12) { MonName = "Jan - Dec"; }
                                        cell = new PdfPCell(new Phrase(dr["pay_head_id"].ToString() + " : " + MonName, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)));
                                        cell.HorizontalAlignment = 3;
                                        cell.VerticalAlignment = 1;
                                        //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                        pdtPay.AddCell(cell);
                                    }
                                    else
                                    {
                                        cell = new PdfPCell(new Phrase(dr["pay_head_id"].ToString(), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)));
                                        cell.HorizontalAlignment = 3;
                                        cell.VerticalAlignment = 1;
                                        //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                        pdtPay.AddCell(cell);
                                    }
                                    decimal Payment = decimal.Parse(dr["Fix_Amt"].ToString());
                                    decimal PreviousAmt = decimal.Parse(dr["Previous_Pay"].ToString());
                                    decimal Discount = decimal.Parse(dr["Discount_AMT"].ToString());
                                    decimal totalamount = decimal.Parse(dr["pay_amt"].ToString());

                                    cell = new PdfPCell(new Phrase(decimal.Parse(dr["Fix_Amt"].ToString().Replace(",", "")).ToString("N2"), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL)));
                                    cell.HorizontalAlignment = 2;
                                    cell.VerticalAlignment = 1;
                                    //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                    pdtPay.AddCell(cell);

                                    cell = new PdfPCell(new Phrase(decimal.Parse(dr["Previous_Pay"].ToString().Replace(",", "")).ToString("N2"), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)));
                                    cell.HorizontalAlignment = 2;
                                    cell.VerticalAlignment = 1;
                                    //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                    pdtPay.AddCell(cell);

                                    // double prvDiscount=IdManager.GetShowSingleValueCurrency(
                                    cell = new PdfPCell(new Phrase(decimal.Parse(dr["Discount_AMT"].ToString().Replace(",", "")).ToString("N2"), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL)));
                                    cell.HorizontalAlignment = 2;
                                    cell.VerticalAlignment = 1;
                                    //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                    pdtPay.AddCell(cell);

                                    cell = new PdfPCell(new Phrase(decimal.Parse(dr["pay_amt"].ToString().Replace(",", "")).ToString("N2"), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)));
                                    cell.HorizontalAlignment = 2;
                                    cell.VerticalAlignment = 1;
                                    //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                    pdtPay.AddCell(cell);

                                    if (i == 2)
                                    {
                                        tott += decimal.Parse(dr["Fix_Amt"].ToString().Replace(",", ""));
                                        tott3 += decimal.Parse(dr["Previous_Pay"].ToString().Replace(",", ""));
                                        tott1 += decimal.Parse(dr["Discount_AMT"].ToString().Replace(",", ""));
                                        tott2 += decimal.Parse(dr["pay_amt"].ToString().Replace(",", ""));
                                    }
                                    else
                                    {
                                        tot += decimal.Parse(dr["Fix_Amt"].ToString().Replace(",", ""));
                                        tot3 += decimal.Parse(dr["Previous_Pay"].ToString().Replace(",", ""));
                                        tot1 += decimal.Parse(dr["Discount_AMT"].ToString().Replace(",", ""));
                                        tot2 += decimal.Parse(dr["pay_amt"].ToString().Replace(",", ""));
                                    }
                                    if (dr["Prv_Discount_AMT"].ToString() != "")
                                    {
                                        tot4 += decimal.Parse(dr["Prv_Discount_AMT"].ToString().Replace(",", ""));
                                    }
                                }
                            }
                            if (tot4 > 0)
                            {
                                ViewState["Discount"] = tot4;
                            }
                            if (i == 2)
                            {
                                string PaidAmount = (Convert.ToDecimal(lblTotalAmount.Text) - Convert.ToDecimal(lblCurrentDiscount.Text)).ToString("N2");
                                string DueAmount = (Convert.ToDecimal(lblTotalAmount.Text) - (Convert.ToDecimal(lblTotalPaid.Text) + Convert.ToDecimal(lblCurrentDiscount.Text))).ToString("N2");

                                cell = new PdfPCell(new Phrase("Total", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.BOLD)));
                                cell.HorizontalAlignment = 2;
                                cell.VerticalAlignment = 1;
                                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                pdtPay.AddCell(cell);
                                cell = new PdfPCell(new Phrase((tott).ToString("N2"), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.BOLD)));
                                cell.HorizontalAlignment = 2;
                                cell.VerticalAlignment = 1;
                                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                pdtPay.AddCell(cell);
                                cell = new PdfPCell(new Phrase((tott3).ToString("N2"), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.BOLD)));
                                cell.HorizontalAlignment = 2;
                                cell.VerticalAlignment = 1;
                                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                pdtPay.AddCell(cell);
                                cell = new PdfPCell(new Phrase((tott1).ToString("N2"), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.BOLD)));
                                cell.HorizontalAlignment = 2;
                                cell.VerticalAlignment = 1;
                                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                pdtPay.AddCell(cell);
                                cell = new PdfPCell(new Phrase((tott2).ToString("N2"), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.BOLD)));
                                cell.HorizontalAlignment = 2;
                                cell.VerticalAlignment = 1;
                                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                pdtPay.AddCell(cell);
                                cell = new PdfPCell(new Phrase("In word: " + DataManager.GetLiteralAmt(tott2.ToString()).Replace("  ", " "), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)));
                                cell.HorizontalAlignment = 3;
                                cell.VerticalAlignment = 2;
                                cell.Border = 0;
                                cell.Colspan = 5;
                                pdtPay.AddCell(cell);

                                PdfPTable pdtTot = new PdfPTable(swidth1);
                                pdtTot.WidthPercentage = 100;
                                pdtTot.HorizontalAlignment = 0;
                                cell = new PdfPCell(new Phrase("Payment Status", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.BOLD)));
                                cell.HorizontalAlignment = 3;
                                cell.VerticalAlignment = 1;
                                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                cell.Colspan = 2;
                                pdtTot.AddCell(cell);
                                cell = new PdfPCell(new Phrase("Total Amount to Pay", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)));
                                cell.HorizontalAlignment = 3;
                                cell.VerticalAlignment = 1;
                                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                pdtTot.AddCell(cell);
                                //lblTotalAmount.Text disabled because the student payed more than due
                                cell = new PdfPCell(new Phrase(decimal.Parse((tott).ToString()).ToString("N2"), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL)));
                                cell.HorizontalAlignment = 2;
                                cell.VerticalAlignment = 1;
                                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                pdtTot.AddCell(cell);

                                cell = new PdfPCell(new Phrase("Previous Discount", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)));
                                cell.HorizontalAlignment = 3;
                                cell.VerticalAlignment = 1;
                                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                pdtTot.AddCell(cell);

                                //lblTotalAmount.Text disabled because the student payed more than due
                                cell = new PdfPCell(new Phrase(decimal.Parse((ViewState["Discount"]).ToString()).ToString("N2"), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL)));
                                cell.HorizontalAlignment = 2;
                                cell.VerticalAlignment = 1;
                                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                pdtTot.AddCell(cell);

                                cell = new PdfPCell(new Phrase("Total Previous Paid", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)));
                                cell.HorizontalAlignment = 3;
                                cell.VerticalAlignment = 1;
                                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                pdtTot.AddCell(cell);


                                //lblTotalAmount.Text disabled because the student payed more than due
                                cell = new PdfPCell(new Phrase(decimal.Parse((tott3).ToString()).ToString("N2"), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL)));
                                cell.HorizontalAlignment = 2;
                                cell.VerticalAlignment = 1;
                                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                pdtTot.AddCell(cell);

                                cell = new PdfPCell(new Phrase("Discount Amount", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)));
                                cell.HorizontalAlignment = 3;
                                cell.VerticalAlignment = 1;
                                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                pdtTot.AddCell(cell);

                                cell = new PdfPCell(new Phrase((tott1).ToString("N2"), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)));
                                cell.HorizontalAlignment = 2;
                                cell.VerticalAlignment = 1;
                                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                pdtTot.AddCell(cell);

                                cell = new PdfPCell(new Phrase("Now Pay Amount", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)));
                                cell.HorizontalAlignment = 3;
                                cell.VerticalAlignment = 1;
                                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                pdtTot.AddCell(cell);
                                cell = new PdfPCell(new Phrase(((tott - (tott3 + Convert.ToDecimal(ViewState["Discount"])))).ToString("N2"), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)));
                                cell.HorizontalAlignment = 2;
                                cell.VerticalAlignment = 1;
                                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                pdtTot.AddCell(cell);

                                cell = new PdfPCell(new Phrase("Total Paid Amount", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)));
                                cell.HorizontalAlignment = 3;
                                cell.VerticalAlignment = 1;
                                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                pdtTot.AddCell(cell);

                                cell = new PdfPCell(new Phrase((Convert.ToDecimal(tott2)).ToString("N2"), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)));
                                cell.HorizontalAlignment = 2;
                                cell.VerticalAlignment = 1;
                                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                pdtTot.AddCell(cell);

                                cell = new PdfPCell(new Phrase("Total Due", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)));
                                cell.HorizontalAlignment = 3;
                                cell.VerticalAlignment = 1;
                                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                pdtTot.AddCell(cell);
                                //lblTotalDue.Text disabled because the student payed more than due
                                cell = new PdfPCell(new Phrase((tott - (tott3 + tott1 + tott2 + Convert.ToDecimal(ViewState["Discount"]))).ToString("N2"), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)));
                                cell.HorizontalAlignment = 2;
                                cell.VerticalAlignment = 1;
                                pdtTot.AddCell(cell);
                                //cell.BorderColor = BaseColor.LIGHT_GRAY;

                                PdfPTable pdtTotDis = new PdfPTable(swidth1);
                                pdtTotDis.WidthPercentage = 100;
                                cell = new PdfPCell(new Phrase("   Overall Discount : " + (Convert.ToDecimal(lblPreviousDiscount.Text) + Convert.ToDecimal(lblCurrentDiscount.Text)) + " Taka.", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.BOLD)));
                                cell.HorizontalAlignment = 3;
                                cell.VerticalAlignment = 1;
                                cell.BorderWidth = 0f;
                                cell.Colspan = 2;
                                pdtTotDis.AddCell(cell);

                                document.Add(pdtPay);
                                document.Add(dtempty);

                                PdfPTable pdtFooter = new PdfPTable(widFooter);
                                pdtFooter.WidthPercentage = 100;
                                cell = new PdfPCell(pdtTot);
                                cell.BorderWidth = 0f;
                                pdtFooter.AddCell(cell);

                                cell = new PdfPCell(pdtTotDis);
                                cell.BorderWidth = 0f;
                                pdtFooter.AddCell(cell);
                                document.Add(pdtFooter);
                                //document.Add(pdtTot);

                                PdfPTable dtempty1 = new PdfPTable(1);
                                dtempty1.WidthPercentage = 100;
                                cell = new PdfPCell(FormatPhrase(""));
                                cell.VerticalAlignment = 0;
                                cell.HorizontalAlignment = 0;
                                cell.BorderWidth = 0f;
                                cell.FixedHeight = 30f;
                                dtempty1.AddCell(cell);
                                document.Add(dtempty1);

                                float[] widthsig = new float[] { 20, 5, 20, 5, 20 };
                                PdfPTable pdtsig = new PdfPTable(widthsig);
                                pdtsig.WidthPercentage = 100;
                                cell = new PdfPCell(FormatPhrase("Student/Gurdian Signature"));
                                cell.HorizontalAlignment = 1;
                                cell.VerticalAlignment = 1;
                                cell.Border = 1;
                                //cell.FixedHeight = 18f;
                                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                pdtsig.AddCell(cell);
                                cell = new PdfPCell(FormatPhrase(""));
                                cell.HorizontalAlignment = 0;
                                cell.VerticalAlignment = 1;
                                cell.Border = 0;
                                //cell.FixedHeight = 18f;
                                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                pdtsig.AddCell(cell);
                                cell = new PdfPCell(FormatPhrase("HeadMaster Signature"));
                                cell.HorizontalAlignment = 1;
                                cell.VerticalAlignment = 1;
                                cell.Border = 1;
                                cell.FixedHeight = 18f;
                                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                pdtsig.AddCell(cell);
                                cell = new PdfPCell(FormatPhrase(""));
                                cell.HorizontalAlignment = 0;
                                cell.VerticalAlignment = 1;
                                cell.Border = 0;
                                cell.FixedHeight = 18f;
                                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                pdtsig.AddCell(cell);
                                cell = new PdfPCell(FormatPhrase("Accounts Officer"));
                                cell.HorizontalAlignment = 1;
                                cell.VerticalAlignment = 1;
                                cell.Border = 1;
                                //cell.FixedHeight = 18f;
                                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                pdtsig.AddCell(cell);
                                document.Add(pdtsig);

                                document.NewPage();
                            }
                            else
                            {
                                string PaidAmount = (Convert.ToDecimal(lblTotalAmount.Text) - Convert.ToDecimal(lblCurrentDiscount.Text)).ToString("N2");
                                string DueAmount = (Convert.ToDecimal(lblTotalAmount.Text) - (Convert.ToDecimal(lblTotalPaid.Text) + Convert.ToDecimal(lblCurrentDiscount.Text))).ToString("N2");

                                cell = new PdfPCell(new Phrase("Total", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.BOLD)));
                                cell.HorizontalAlignment = 2;
                                cell.VerticalAlignment = 1;
                                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                pdtPay.AddCell(cell);
                                cell = new PdfPCell(new Phrase(tot.ToString("N2"), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.BOLD)));
                                cell.HorizontalAlignment = 2;
                                cell.VerticalAlignment = 1;
                                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                pdtPay.AddCell(cell);

                                cell = new PdfPCell(new Phrase(tot3.ToString("N2"), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.BOLD)));
                                cell.HorizontalAlignment = 2;
                                cell.VerticalAlignment = 1;
                                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                pdtPay.AddCell(cell);
                                cell = new PdfPCell(new Phrase(tot1.ToString("N2"), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.BOLD)));
                                cell.HorizontalAlignment = 2;
                                cell.VerticalAlignment = 1;
                                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                pdtPay.AddCell(cell);
                                cell = new PdfPCell(new Phrase(tot2.ToString("N2"), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.BOLD)));
                                cell.HorizontalAlignment = 2;
                                cell.VerticalAlignment = 1;
                                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                pdtPay.AddCell(cell);
                                cell = new PdfPCell(new Phrase("In word: " + DataManager.GetLiteralAmt(tot2.ToString()).Replace("  ", " "), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.BOLD)));
                                string Name = DataManager.GetLiteralAmt(tot.ToString()).Replace("  ", " ");
                                cell.HorizontalAlignment = 3;
                                cell.VerticalAlignment = 2;
                                cell.Border = 0;
                                cell.Colspan = 5;
                                pdtPay.AddCell(cell);

                                PdfPTable pdtTot = new PdfPTable(swidth1);
                                pdtTot.WidthPercentage = 50;
                                pdtTot.HorizontalAlignment = 0;
                                cell = new PdfPCell(new Phrase("Payment Status", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.BOLD)));
                                cell.HorizontalAlignment = 3;
                                cell.VerticalAlignment = 1;
                                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                cell.Colspan = 2;
                                pdtTot.AddCell(cell);
                                cell = new PdfPCell(new Phrase("Total Amount to Pay", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)));
                                cell.HorizontalAlignment = 3;
                                cell.VerticalAlignment = 1;
                                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                pdtTot.AddCell(cell);

                                //lblTotalAmount.Text disabled because the student payed more than due
                                cell = new PdfPCell(new Phrase((decimal.Parse((tot).ToString())).ToString("N2"), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL)));
                                cell.HorizontalAlignment = 2;
                                cell.VerticalAlignment = 1;
                                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                pdtTot.AddCell(cell);

                                cell = new PdfPCell(new Phrase("Previous Discount", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)));
                                cell.HorizontalAlignment = 3;
                                cell.VerticalAlignment = 1;
                                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                pdtTot.AddCell(cell);

                                //lblTotalAmount.Text disabled because the student payed more than due
                                cell = new PdfPCell(new Phrase(decimal.Parse((ViewState["Discount"]).ToString()).ToString("N2"), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL)));
                                cell.HorizontalAlignment = 2;
                                cell.VerticalAlignment = 1;
                                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                pdtTot.AddCell(cell);

                                cell = new PdfPCell(new Phrase("Total Previous Paid", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)));
                                cell.HorizontalAlignment = 3;
                                cell.VerticalAlignment = 1;
                                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                pdtTot.AddCell(cell);

                                //lblTotalAmount.Text disabled because the student payed more than due
                                cell = new PdfPCell(new Phrase(decimal.Parse((tot3).ToString()).ToString("N2"), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL)));
                                cell.HorizontalAlignment = 2;
                                cell.VerticalAlignment = 1;
                                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                pdtTot.AddCell(cell);

                                cell = new PdfPCell(new Phrase("Discount Amount", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)));
                                cell.HorizontalAlignment = 3;
                                cell.VerticalAlignment = 1;
                                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                pdtTot.AddCell(cell);

                                cell = new PdfPCell(new Phrase((tot1).ToString("N2"), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)));
                                cell.HorizontalAlignment = 2;
                                cell.VerticalAlignment = 1;
                                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                pdtTot.AddCell(cell);

                                cell = new PdfPCell(new Phrase("Now Pay Amount", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)));
                                cell.HorizontalAlignment = 3;
                                cell.VerticalAlignment = 1;
                                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                pdtTot.AddCell(cell);
                                cell = new PdfPCell(new Phrase(((tot - (tot3 + Convert.ToDecimal(ViewState["Discount"])))).ToString("N2"), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)));
                                cell.HorizontalAlignment = 2;
                                cell.VerticalAlignment = 1;
                                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                pdtTot.AddCell(cell);

                                cell = new PdfPCell(new Phrase("Total Paid Amount", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)));
                                cell.HorizontalAlignment = 3;
                                cell.VerticalAlignment = 1;
                                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                pdtTot.AddCell(cell);
                                cell = new PdfPCell(new Phrase((Convert.ToDecimal(tot2)).ToString("N2"), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)));
                                cell.HorizontalAlignment = 2;
                                cell.VerticalAlignment = 1;
                                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                pdtTot.AddCell(cell);

                                cell = new PdfPCell(new Phrase("Total Due", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)));
                                cell.HorizontalAlignment = 3;
                                cell.VerticalAlignment = 1;
                                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                pdtTot.AddCell(cell);
                                //lblTotalDue.Text disabled because the student payed more than due

                                cell = new PdfPCell(new Phrase((Convert.ToDecimal(tot - (tot3 + tot1 + tot2 + Convert.ToDecimal(ViewState["Discount"])))).ToString("N2"), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)));
                                cell.HorizontalAlignment = 2;
                                cell.VerticalAlignment = 1;
                                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                pdtTot.AddCell(cell);
                                document.Add(pdtPay);
                                document.Add(dtempty);

                                PdfPTable pdtTotDis = new PdfPTable(swidth1);
                                pdtTotDis.WidthPercentage = 100;
                                cell = new PdfPCell(new Phrase("   Overall Discount : " + (Convert.ToDecimal(lblPreviousDiscount.Text) + Convert.ToDecimal(lblCurrentDiscount.Text)) + " Taka.", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.BOLD)));
                                cell.HorizontalAlignment = 3;
                                cell.VerticalAlignment = 1;
                                cell.BorderWidth = 0f;
                                cell.Colspan = 2;
                                pdtTotDis.AddCell(cell);

                                PdfPTable pdtFooter = new PdfPTable(widFooter);
                                pdtFooter.WidthPercentage = 100;
                                cell = new PdfPCell(pdtTot);
                                cell.BorderWidth = 0f;
                                pdtFooter.AddCell(cell);

                                cell = new PdfPCell(pdtTotDis);
                                cell.BorderWidth = 0f;
                                pdtFooter.AddCell(cell);
                                document.Add(pdtFooter);
                                //document.Add(pdtTot);

                                PdfPTable dtempty1 = new PdfPTable(1);
                                dtempty1.WidthPercentage = 100;
                                cell = new PdfPCell(FormatPhrase(""));
                                cell.VerticalAlignment = 0;
                                cell.HorizontalAlignment = 0;
                                cell.BorderWidth = 0f;
                                cell.FixedHeight = 30f;
                                dtempty1.AddCell(cell);
                                document.Add(dtempty1);

                                float[] widthsig = new float[] { 20, 5, 20, 5, 20 };
                                PdfPTable pdtsig = new PdfPTable(widthsig);
                                pdtsig.WidthPercentage = 100;
                                cell = new PdfPCell(FormatPhrase("Student/Gurdian Signature"));
                                cell.HorizontalAlignment = 1;
                                cell.VerticalAlignment = 1;
                                cell.Border = 1;
                                //cell.FixedHeight = 18f;
                                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                pdtsig.AddCell(cell);
                                cell = new PdfPCell(FormatPhrase(""));
                                cell.HorizontalAlignment = 0;
                                cell.VerticalAlignment = 1;
                                cell.Border = 0;
                                //cell.FixedHeight = 18f;
                                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                pdtsig.AddCell(cell);
                                cell = new PdfPCell(FormatPhrase("Headmaster Signature"));
                                cell.HorizontalAlignment = 1;
                                cell.VerticalAlignment = 1;
                                cell.Border = 1;
                                cell.FixedHeight = 18f;
                                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                pdtsig.AddCell(cell);
                                cell = new PdfPCell(FormatPhrase(""));
                                cell.HorizontalAlignment = 0;
                                cell.VerticalAlignment = 1;
                                cell.Border = 0;
                                cell.FixedHeight = 18f;
                                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                pdtsig.AddCell(cell);
                                cell = new PdfPCell(FormatPhrase("Accounts Officer"));
                                cell.HorizontalAlignment = 1;
                                cell.VerticalAlignment = 1;
                                cell.Border = 1;
                                //cell.FixedHeight = 18f;
                                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                pdtsig.AddCell(cell);
                                document.Add(pdtsig);
                                document.NewPage();
                            }
                            //if (i == 1)
                            //{
                            //    document.Add(new Paragraph(string.Format("", i)));
                            //}
                        }

                    }

                    document.Close();
                    Response.Flush();
                    Response.End();

                }
            }
            else
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "ale", "alert('Please select a Payment first!!');", true);
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
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {

            if (ViewState["paydtl"] != null)
            {
                DataTable dtDtl = (DataTable)ViewState["paydtl"];
                if (dtDtl.Rows.Count < 1)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Please input at least one payment type!!');", true);
                    return;
                }
                else if (dtDtl.Rows.Count == 1 && ((DataRow)dtDtl.Rows[0])["pay_id"].ToString() == "")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Please input at least one payment type!!');", true);
                    return;
                }
            }
            clsPaymentMst paymst = clsPaymentManager.getPaymentMst(txtPaymentId.Text);
            if (paymst != null)
            {
                if (per.AllowEdit == "Y")
                {
                    //    paymst.PayDate = txtPayDate.Text;
                    //    paymst.PayMode = ddlPayMode.SelectedValue;
                    //    paymst.StudentId = txtStudentId.Text;
                    //    paymst.ClassId = ddlClassId.SelectedValue;
                    //    paymst.Section = ddlSect.SelectedValue;
                    //    paymst.Version = ddlVersion.SelectedValue;
                    //    paymst.Shift = ddlShift.SelectedValue;
                    //    paymst.PayYear = txtYear.Text;
                    //    paymst.ChequeNo = txtChequeNo.Text;
                    //    paymst.ChequeDate = txtChequeDate.Text;
                    //    paymst.ChequeAmt = txtChequeAmt.Text;
                    //    paymst.BankNo = ddlBankNo.SelectedValue;
                    //    paymst.RefNo = txtRefNo.Text;
                    //    paymst.PayAmt = lblTotalAmount.Text.Replace(",", "");
                    //    paymst.TotalPaidAmt = GetTotal().ToString();
                    //    paymst.TotalDiscountAmt = lblCurrentDiscount.Text.Replace(",", "");
                    //    paymst.UpdateUser = Session["user"].ToString();
                    //    paymst.UpdateDate = DateTime.Parse(System.DateTime.Now.ToString()).ToString("dd/MM/yyyy");
                    //    clsPaymentManager.UpdatePaymentMst(paymst);
                    //    VouchMst vmst = VouchManager.GetVouchMstByRefsl(paymst.PaymentId);
                    //    if (vmst != null)
                    //    {
                    //        vmst.FinMon = FinYearManager.getFinMonthByDate(txtPayDate.Text);
                    //        vmst.ValueDate = txtPayDate.Text;
                    //        vmst.VchCode = "02";
                    //        vmst.RefFileNo = "";
                    //        vmst.VolumeNo = "";
                    //        vmst.SerialNo = paymst.PaymentId;
                    //        vmst.Particulars = "Received student fees from " + StudentManager.getStudentName(paymst.StudentId) + ":ID No." + paymst.StudentId + " ";
                    //        vmst.ControlAmt = GetTotal().ToString();
                    //        vmst.Payee = "ST_Pay";
                    //        vmst.CheckNo = "";
                    //        vmst.CheqDate = "";
                    //        vmst.CheqAmnt = "";
                    //        vmst.MoneyRptNo = "";
                    //        vmst.MoneyRptDate = "";
                    //        vmst.TransType = "R";
                    //        vmst.BookName = Session["book"].ToString();
                    //        vmst.EntryUser = Session["user"].ToString();
                    //        vmst.EntryDate = DateTime.Parse(DateTime.Now.ToString()).ToString("dd/MM/yyyy");
                    //        vmst.Status = "U";
                    //        VouchManager.UpdateVouchMst(vmst);
                    //        VouchManager.DeleteVouchDtl(vmst.VchSysNo);
                    //    }
                    //    clsPaymentManager.DeletePaymentDtls(paymst.PaymentId);

                    //    clsPaymentDtl paydtl;
                    //    VouchDtl vdtl;
                    //    vdtl = new VouchDtl();
                    //    vdtl.VchSysNo = vmst.VchSysNo;
                    //    vdtl.ValueDate = txtPayDate.Text;
                    //    vdtl.LineNo = "1";
                    //    if (ddlPayMode.SelectedValue == "Q")
                    //    {
                    //        string B_Cood = IdManager.GetShowSingleValueString("gl_coa_code", "bank_id", "bank_info", ddlPayMode.SelectedValue);
                    //        vdtl.GlCoaCode = "1-" + B_Cood;
                    //        vdtl.Particulars = "Cash in Hand: Student Fee Receive Desk";
                    //        vdtl.AccType = VouchManager.getAccType("1-" + B_Cood);
                    //    }
                    //    else
                    //    {

                    //        string C_Code = IdManager.GetShowSingleValueString("CASH_CODE", "BOOK_NAME", "GL_SET_OF_BOOKS", "AMB");
                    //        vdtl.GlCoaCode = "1-" + C_Code;
                    //        vdtl.Particulars = "Cash at Bank: Student Fee Receive Desk";
                    //        vdtl.AccType = VouchManager.getAccType("1-" + C_Code);
                    //    }
                    //    vdtl.AmountDr = GetTotal().ToString();
                    //    vdtl.AmountCr = "0";
                    //    vdtl.Status = "U";
                    //    vdtl.BookName = Session["book"].ToString();
                    //    VouchManager.CreateVouchDtl(vdtl);

                    //    int i = 2;
                    //    decimal TotalAmount = 0;
                    //    decimal TotalDiscount = 0;
                    //    DataTable dtpaydtl = (DataTable)ViewState["paydtl"];
                    //    foreach (DataRow drp in dtpaydtl.Rows)
                    //    {
                    //        if (!string.IsNullOrEmpty(drp["pay_amt"].ToString()))
                    //        {
                    //            paydtl = new clsPaymentDtl();
                    //            paydtl.PaymentId = paymst.PaymentId;
                    //            paydtl.PayId = drp["pay_id"].ToString();
                    //            paydtl.PayAmt = drp["pay_amt"].ToString().Replace(",", "");
                    //            paydtl.Discount_AMT = drp["Discount_Amt"].ToString().Replace(",", "");
                    //            TotalAmount += Convert.ToDecimal(drp["pay_amt"]);
                    //            TotalDiscount += Convert.ToDecimal(drp["Discount_Amt"]);
                    //            clsPaymentManager.CreatePaymentDtl(paydtl);
                    //            //                            1. Admission Fee .2 Tution Fee.	3. Session Charge  4.  Development Fee.	5. Magazine.  6. B.N.C.C.   7. Baz/Diary. 8.  Multimedia and Computer. 9	Graciuate .   10 .Water And Electricity.   11. Semister Fee.	12. Baz 13. Diary  14.	Registation Fees   15.	IT Fees  16.  ID Card
                    //            //17.  Certificate Fees	18.  Testomonial Fees  19. Laboratory Fees   20.  Exam Fees

                    //            if (vmst != null)
                    //            {
                    //                string GlcoaCode = "";
                    //                if (ddlShift.SelectedItem.Text == "Morning")
                    //                {
                    //                    int headId = IdManager.GetShowSingleValueInt("pay_head_id", "pay_id", "payment_info", drp["pay_id"].ToString());
                    //                    if (headId == 1) { GlcoaCode = "5010110"; } else if (headId == 2) { GlcoaCode = "5010130"; } else if (headId == 4) { GlcoaCode = "5010120"; } else if (headId == 15) { GlcoaCode = "5010140"; } else if (headId == 16) { GlcoaCode = "5010150"; } else if (headId == 17) { GlcoaCode = "5010160"; } else if (headId == 18) { GlcoaCode = "5010170"; } else if (headId == 19) { GlcoaCode = "5010180"; } else if (headId == 20) { GlcoaCode = "5010113"; } else { GlcoaCode = "5030000"; }
                    //                }
                    //                else if (ddlShift.SelectedItem.Text == "Day")
                    //                {
                    //                    int headId = IdManager.GetShowSingleValueInt("pay_head_id", "pay_id", "payment_info", drp["pay_id"].ToString());
                    //                    if (headId == 1) { GlcoaCode = "5010210"; } else if (headId == 2) { GlcoaCode = "5010230"; } else if (headId == 4) { GlcoaCode = "5010220"; } else if (headId == 15) { GlcoaCode = "5010240"; } else if (headId == 16) { GlcoaCode = "5010250"; } else if (headId == 17) { GlcoaCode = "5010260"; } else if (headId == 18) { GlcoaCode = "5010270"; } else if (headId == 19) { GlcoaCode = "5010280"; } else if (headId == 20) { GlcoaCode = "5010213"; } else { GlcoaCode = "5030000"; }
                    //                }

                    //                vdtl = new VouchDtl();
                    //                vdtl.VchSysNo = vmst.VchSysNo;
                    //                vdtl.ValueDate = txtPayDate.Text;
                    //                vdtl.LineNo = i.ToString();
                    //                vdtl.GlCoaCode = "1-" + GlcoaCode;
                    //                vdtl.Particulars = drp["pay_head_id"].ToString();
                    //                vdtl.AccType = VouchManager.getAccType("1-" + GlcoaCode);
                    //                vdtl.AmountDr = "0";
                    //                vdtl.AmountCr = drp["pay_amt"].ToString().Replace(",", "");
                    //                vdtl.Status = "U";
                    //                vdtl.BookName = Session["book"].ToString();
                    //                VouchManager.CreateVouchDtl(vdtl);
                    //                i++;
                    //            }
                    //        }
                    //    }
                    //    //lblTotalPaid.Text =(Convert.ToDecimal(lblTotalDue.Text)+(TotalAmount)).ToString("N2");
                    //    //lblTotalDue.Text = ((Convert.ToDecimal(lblTotalAmount.Text)) - (Convert.ToDecimal(lblTotalPaid.Text))).ToString("N2");
                    //    ShowFooterTotal();
                    //    //txtStudentId_TextChanged(sender, e);
                    //    ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Record updated successfully!!');", true);
                    //}
                    //else
                    //{
                    //    ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('You have not enough permissoin to update this record!!');", true);
                    //}
                }
            }

            else
            {

                if (per.AllowAdd == "Y")
                {

                        paymst = new clsPaymentMst();
                        paymst.PayDate = txtPayDate.Text;
                        paymst.PayMode = ddlPayMode.SelectedValue;
                        paymst.StudentId = txtStudentId.Text;
                        paymst.ClassId = lblClassId.Text;
                        paymst.Section = lblSectionId.Text;
                        paymst.Version = lblVersonId.Text;
                        paymst.Shift = lblShiftId.Text;
                        //paymst.PayYear = lblYear.Text;
                        paymst.ChequeNo = txtChequeNo.Text;
                        paymst.ChequeDate = txtChequeDate.Text;
                        paymst.ChequeAmt = txtChequeAmt.Text;
                        paymst.BankNo = ddlBankNo.SelectedValue;
                        paymst.RefNo = txtRefNo.Text;
                        paymst.EntryUser = Session["user"].ToString();
                        paymst.EntryDate = DateTime.Parse(System.DateTime.Now.ToString()).ToString("dd/MM/yyyy");
                        paymst.UpdateUser = "";
                        paymst.UpdateDate = "";
                        paymst.PayAmt = lblTotalAmount.Text.Replace(",", "");
                        paymst.TotalPaidAmt = GetTotal().ToString();
                        paymst.TotalDiscountAmt = lblCurrentDiscount.Text.Replace(",", "");
                        //***Disabled for previous entry. Will be corrected again after finishing all previous entries
                        paymst.PaymentId = IdManager.GetNextID("payment_mst", "payment_id").ToString();
                        txtPaymentId.Text = paymst.PaymentId;
                        //paymst.PaymentId = txtPaymentId.Text;
                        txtPaymentId.Text = paymst.PaymentId;
                        clsPaymentManager.CreatePaymentMst(paymst);

                        decimal TotalAmount = 0;
                        decimal TotalDiscount = 0;
                        VouchMst vmst = new VouchMst();

                        vmst.FinMon = FinYearManager.getFinMonthByDate(txtPayDate.Text);
                        vmst.ValueDate = txtPayDate.Text;
                        vmst.VchCode = "02";
                        vmst.RefFileNo = "";
                        vmst.VolumeNo = "";
                        vmst.SerialNo = paymst.PaymentId;
                        vmst.Particulars = "Student fees received from " + StudentManager.getStudentName(paymst.StudentId) + ":ID No." + paymst.StudentId + " ";
                        vmst.ControlAmt = GetTotal().ToString();
                        vmst.Payee = "ST_Pay";
                        vmst.CheckNo = "";
                        vmst.CheqDate = "";
                        vmst.CheqAmnt = "";
                        vmst.MoneyRptNo = "";
                        vmst.MoneyRptDate = "";
                        vmst.TransType = "R";
                        vmst.BookName = Session["book"].ToString();
                        vmst.EntryUser = Session["user"].ToString();
                        vmst.EntryDate = DateTime.Parse(DateTime.Now.ToString()).ToString("dd/MM/yyyy");
                        vmst.VchSysNo = IdManager.GetNextID("gl_trans_mst", "vch_sys_no").ToString();
                        vmst.VchRefNo = "CV-" + vmst.VchSysNo.ToString().PadLeft(10, '0');
                        vmst.AuthoUserType = "4";
                        vmst.Status = "A";
                        VouchManager.CreateVouchMst(vmst);

                        clsPaymentDtl paydtl;
                        VouchDtl vdtl;
                        vdtl = new VouchDtl();
                        vdtl.VchSysNo = vmst.VchSysNo;
                        vdtl.ValueDate = txtPayDate.Text;
                        vdtl.LineNo = "1";
                        if (ddlPayMode.SelectedValue == "Q")
                        {
                           // string B_Cood = IdManager.GetShowSingleValueString("gl_coa_code", "bank_id", "bank_info", ddlPayMode.SelectedValue);
                            vdtl.GlCoaCode = "1-" + ddlBankNo.SelectedValue;
                            vdtl.Particulars = "Cash at Bank: Student Fee Receive Desk";
                            vdtl.AccType = VouchManager.getAccType("1-" + ddlBankNo.SelectedValue);
                            if (vdtl.AccType == "")
                            {
                                string C_Code = IdManager.GetShowSingleValueString("CASH_CODE", "BOOK_NAME", "GL_SET_OF_BOOKS", "AMB");
                                vdtl.GlCoaCode = "1-" + C_Code;
                                vdtl.Particulars = "Cash in Hand: Student Fee Receive Desk";
                                vdtl.AccType = VouchManager.getAccType("1-" + C_Code);
                            }
                        }
                        else
                        {

                            string C_Code = IdManager.GetShowSingleValueString("CASH_CODE", "BOOK_NAME", "GL_SET_OF_BOOKS", "AMB");
                            vdtl.GlCoaCode = "1-" + C_Code;
                            vdtl.Particulars =  "Cash in Hand: Student Fee Receive Desk";
                            vdtl.AccType = VouchManager.getAccType("1-" + C_Code);
                            
                        }
                        vdtl.AmountDr = GetTotal().ToString();
                        vdtl.AmountCr = "0";
                        vdtl.Status = "A";
                        vdtl.BookName = Session["book"].ToString();
                        VouchManager.CreateVouchDtl(vdtl);

                        int i = 2;
                        DataTable dtpaydtl = (DataTable)ViewState["paydtl"];
                        foreach (DataRow drp in dtpaydtl.Rows)
                        {
                            if (!string.IsNullOrEmpty(drp["pay_amt"].ToString()))
                            {
                                paydtl = new clsPaymentDtl();
                                paydtl.PaymentId = paymst.PaymentId;
                                paydtl.PayId = drp["pay_id"].ToString();
                                paydtl.PayAmt = drp["pay_amt"].ToString().Replace(",", "");
                                paydtl.Discount_AMT = drp["Discount_Amt"].ToString().Replace(",", "");
                                TotalAmount += Convert.ToDecimal(drp["pay_amt"]);
                                TotalDiscount += Convert.ToDecimal(drp["Discount_Amt"]);
                                clsPaymentManager.CreatePaymentDtl(paydtl);
                                if (vmst != null)
                                {
                                     if (lblClassId.Text == "12" || lblClassId.Text == "13")
                                    {
                                        string GlcoaCode = "";
                                   
                                        int headId = IdManager.GetShowSingleValueInt("pay_head_id", "pay_id", "payment_info", drp["pay_id"].ToString());
                                        if (headId == 1) { GlcoaCode = "5010310"; }
                                        else if (headId == 2) { GlcoaCode = "5010320"; }
                                        else if (headId == 3) { GlcoaCode = "5010330"; }
                                        else if (headId == 4) { GlcoaCode = "5011312"; }
                                        else if (headId == 25) { GlcoaCode = "5010355"; }
                                        else if (headId == 26) { GlcoaCode = "5019312"; }
                                        else if (headId == 27) { GlcoaCode = "5010366"; }
                                        else if (headId == 28) { GlcoaCode = "5010377"; }
                                        else if (headId == 32) { GlcoaCode = "5014316"; }
                                        else if (headId == 33) { GlcoaCode = "5010388"; }
                                        else if (headId == 35) { GlcoaCode = "5010399"; }
                                        else if (headId == 38) { GlcoaCode = "5010321"; }
                                        else if (headId == 5) { GlcoaCode = "5012312"; }
                                        else if (headId == 6) { GlcoaCode = "5013312"; }
                                        else if (headId == 8) { GlcoaCode = "5015312"; }
                                        else if (headId == 9) { GlcoaCode = "5016312"; }
                                        else if (headId == 10) { GlcoaCode = "5017312"; }
                                        else if (headId == 11) { GlcoaCode = "5018312"; }
                                        else if (headId == 15) { GlcoaCode = "5013390"; }
                                        else if (headId == 16) { GlcoaCode = "5010360"; }
                                        else if (headId == 17) { GlcoaCode = "5010370"; }
                                        else if (headId == 18) { GlcoaCode = "5010380"; }
                                        else if (headId == 19) { GlcoaCode = "5010350"; }
                                        else if (headId == 20) { GlcoaCode = "5010340"; }
                                        else if (headId == 24) { GlcoaCode = "5013311"; }
                                        else if (headId == 39) { GlcoaCode = "5010322"; }
                                        else if (headId == 7) { GlcoaCode = "5020312"; }
                                        else if (headId == 40) { GlcoaCode = "5014315"; }
                                        else if (headId == 41) { GlcoaCode = "5014314"; }

                                        else { GlcoaCode = "5030000"; }           
                                    
                                  

                                    vdtl = new VouchDtl();
                                    vdtl.VchSysNo = vmst.VchSysNo;
                                    vdtl.ValueDate = txtPayDate.Text;
                                    vdtl.LineNo = i.ToString();
                                    vdtl.GlCoaCode = "1-" + GlcoaCode;
                                    vdtl.Particulars = drp["pay_head_id"].ToString();
                                    vdtl.AccType = VouchManager.getAccType("1-" + GlcoaCode);
                                    vdtl.AmountDr = "0";
                                    vdtl.AmountCr = drp["pay_amt"].ToString().Replace(",", "");
                                    vdtl.Status = "A";
                                    vdtl.BookName = Session["book"].ToString();
                                    VouchManager.CreateVouchDtl(vdtl);
                                    i++;
                                    }
                                    else
                                    {
                                    string GlcoaCode = "";
                                    if (lblShiftNew.Text == "Day")
                                    {
                                        int headId = IdManager.GetShowSingleValueInt("pay_head_id", "pay_id", "payment_info", drp["pay_id"].ToString());
                                        if (headId == 1) { GlcoaCode = "5010110"; } else if (headId == 2) { GlcoaCode = "5010120"; } else if (headId == 3) { GlcoaCode = "5010130"; } else if (headId == 4) { GlcoaCode = "5011112"; } else if (headId == 5) { GlcoaCode = "5012112"; } else if (headId == 6) { GlcoaCode = "5013112"; } else if (headId == 7) { GlcoaCode = "5014112"; } else if (headId == 8) { GlcoaCode = "5015112"; } else if (headId == 9) { GlcoaCode = "5016112"; } else if (headId == 10) { GlcoaCode = "5017112"; } else if (headId == 11) { GlcoaCode = "5018112"; } else if (headId == 12) { GlcoaCode = "5014190"; } else if (headId == 13) { GlcoaCode = "5011190"; } else if (headId == 14) { GlcoaCode = "5012190"; } else if (headId == 15) { GlcoaCode = "5013190"; } else if (headId == 16) { GlcoaCode = "5010160"; } else if (headId == 17) { GlcoaCode = "5010170"; } else if (headId == 18) { GlcoaCode = "5010180"; } else if (headId == 19) { GlcoaCode = "5010150"; } else if (headId == 20) { GlcoaCode = "5010140"; } else if (headId == 21) { GlcoaCode = "5014111"; } else if (headId == 22) { GlcoaCode = "5011111"; } else if (headId == 23) { GlcoaCode = "5012111"; } else if (headId == 24) { GlcoaCode = "5013111"; } else if (headId == 25) { GlcoaCode = "5010185"; } else if (headId == 26) { GlcoaCode = "5019112"; } else if (headId == 27) { GlcoaCode = "5010184"; } else if (headId == 28) { GlcoaCode = "5010183"; } else if (headId == 29) { GlcoaCode = "5010213"; } else if (headId == 30) { GlcoaCode = "5010313"; } else if (headId == 31) { GlcoaCode = "5010413"; } else if (headId == 32) { GlcoaCode = "5010513"; } else if (headId == 33) { GlcoaCode = "5010182"; } else if (headId == 35) { GlcoaCode = "5010181"; } else if (headId == 36) { GlcoaCode = "5010186"; } else if (headId == 37) { GlcoaCode = "5010187"; } else if (headId == 38) { GlcoaCode = "5010188"; } else if (headId == 39) { GlcoaCode = "5010190"; } else if (headId == 40) { GlcoaCode = "5010191"; } else if (headId == 41) { GlcoaCode = "5010192"; } else { GlcoaCode = "5030000"; }
                                    }
                                    else if (lblShiftNew.Text == "Morning")
                                    {
                                        int headId = IdManager.GetShowSingleValueInt("pay_head_id", "pay_id", "payment_info", drp["pay_id"].ToString());
                                        if (headId == 1) { GlcoaCode = "5010210"; }
                                        else if (headId == 2) { GlcoaCode = "5010220"; }
                                        else if (headId == 3) { GlcoaCode = "5010230"; }
                                        else if (headId == 4) { GlcoaCode = "5011212"; }
                                        else if (headId == 5) { GlcoaCode = "5012212"; }
                                        else if (headId == 6) { GlcoaCode = "5013212"; }
                                        else if (headId == 7) { GlcoaCode = "5014212"; }
                                        else if (headId == 8) { GlcoaCode = "5015212"; }
                                        else if (headId == 9) { GlcoaCode = "5016212"; }
                                        else if (headId == 10) { GlcoaCode = "5017212"; }
                                        else if (headId == 11) { GlcoaCode = "5018212"; }
                                        else if (headId == 12) { GlcoaCode = "5014290"; }
                                        else if (headId == 13) { GlcoaCode = "5011290"; }
                                        else if (headId == 14) { GlcoaCode = "5012290"; }
                                        else if (headId == 15) { GlcoaCode = "5013290"; } else if (headId == 16) { GlcoaCode = "5010260"; } else if (headId == 17) { GlcoaCode = "5010270"; } else if (headId == 18) { GlcoaCode = "5010280"; } else if (headId == 19) { GlcoaCode = "5010250"; } else if (headId == 20) { GlcoaCode = "5010240"; } else if (headId == 21) { GlcoaCode = "5014211"; } else if (headId == 22) { GlcoaCode = "5011211"; } else if (headId == 23) { GlcoaCode = "5012211"; } else if (headId == 24) { GlcoaCode = "5013211"; } else if (headId == 25) { GlcoaCode = "5015280"; } else if (headId == 26) { GlcoaCode = "5019212"; } else if (headId == 27) { GlcoaCode = "5014280"; } else if (headId == 28) { GlcoaCode = "5013280"; } else if (headId == 29) { GlcoaCode = "5012223"; } else if (headId == 30) { GlcoaCode = "5012233"; } else if (headId == 31) { GlcoaCode = "5012253"; } else if (headId == 32) { GlcoaCode = "5012243"; } else if (headId == 33) { GlcoaCode = "5012280"; } else if (headId == 35) { GlcoaCode = "5011280"; } else if (headId == 36) { GlcoaCode = "5015281"; } else if (headId == 37) { GlcoaCode = "5015282"; } else if (headId == 38) { GlcoaCode = "5015283"; } else if (headId == 39) { GlcoaCode = "5015285"; } else if (headId == 40) { GlcoaCode = "5015286"; } else if (headId == 41) { GlcoaCode = "5015287"; }

else { GlcoaCode = "5030000"; }
                                    }

                                    vdtl = new VouchDtl();
                                    vdtl.VchSysNo = vmst.VchSysNo;
                                    vdtl.ValueDate = txtPayDate.Text;
                                    vdtl.LineNo = i.ToString();
                                    vdtl.GlCoaCode = "1-" + GlcoaCode;
                                    vdtl.Particulars = drp["pay_head_id"].ToString();
                                    vdtl.AccType = VouchManager.getAccType("1-" + GlcoaCode);
                                    vdtl.AmountDr = "0";
                                    vdtl.AmountCr = drp["pay_amt"].ToString().Replace(",", "");
                                    vdtl.Status = "A";
                                    vdtl.BookName = Session["book"].ToString();
                                    VouchManager.CreateVouchDtl(vdtl);
                                    i++;

                                    }
                                }
                            }
                        }                                       
                    lblCurrentDiscount.Text = (TotalDiscount).ToString("N2");
                    lblTotalPaid.Text = (GetTotal()).ToString("N2");
                    lblTotalDue.Text = (Convert.ToDecimal(lblTotalAmount.Text) - (Convert.ToDecimal(lblTotalPaid.Text) + Convert.ToDecimal(lblCurrentDiscount.Text))).ToString("N2");

                    ShowFooterTotal();
                    ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Record created successfully!!');", true);
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('You have not enough permissoin to create this record!!');", true);
                }
            }
            btnSave.Enabled = false;
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

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtPaymentId.Text != "" && txtStudentId.Text != "")
            {
                if (per.AllowDelete == "Y")
                {
                    aclsPaymentManagerObj.DeletePaymentDtls(txtPaymentId.Text);
                    //clsPaymentManager.DeletePaymentMst(txtPaymentId.Text);
                    clearFields();
                    ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Record deleted successfully!!');", true);
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('You have not enough permissoin to delete this record!!');", true);
                }
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Please select a transaction first!!');", true);
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
    protected void btnFind_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtPaymentId.Text != "")
            {
                clsPaymentMst paymst = clsPaymentManager.getPaymentMst(txtPaymentId.Text);
                if (paymst != null)
                {
                    txtPayDate.Text = paymst.PayDate;
                    ddlPayMode.SelectedValue = paymst.PayMode;
                    txtStudentId.Text = paymst.StudentId;
                    txtStudentId_TextChanged(sender, e);
                    lblName.Text = StudentManager.getStudentName(paymst.StudentId);
                    lblClassId.Text = paymst.ClassId;
                    lblYear.Text = paymst.PayYear;
                    lblYear0.Text = paymst.PayYear;
                    txtChequeNo.Text = paymst.ChequeNo;
                    txtChequeDate.Text = paymst.ChequeDate;
                    txtChequeAmt.Text = paymst.ChequeAmt;
                    ddlBankNo.SelectedValue = paymst.BankNo;
                    txtRefNo.Text = paymst.RefNo;
                    txtWaiverPct.Text = clsStdWaiverManager.getStudentWaiverPct(paymst.StudentId, paymst.PayYear, paymst.ClassId);
                    //DataTable dtDtl = clsPaymentManager.getPaymentDtls(txtPaymentId.Text);
                    //if (dtDtl.Rows.Count > 0)
                    //{
                    //    ViewState["paydtl"] = dtDtl;
                    //    dgPay.DataSource = dtDtl;
                    //    DataRow dr = dtDtl.NewRow();
                    //    dtDtl.Rows.Add(dr);
                    //    dgPay.DataBind();
                    //    ShowFooterTotal();
                    //}
                    //else
                    //{
                    //    getEmptyGrid();
                    //}
                    dgStd.Visible = false;
                    pnlPay.Visible = true;
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('No payment is made yet for this student!!');", true);
                }
            }
            else if (txtPaymentId.Text == "" && txtStudentId.Text != "")
            {
                /******************** Ekhane user id dhore jate data dekhte pare gridview te ************************/
                //DataTable dt = clsPaymentManager.getPaymentMsts(txtPaymentId.Text, txtStudentId.Text);
                DataTable dt = clsPaymentManager.getPaymentMsts(txtPaymentId.Text, txtStudentId.Text, Session["user"].ToString(), Session["userlevel"].ToString(), lblPaymentYear.Text);
                if (dt.Rows.Count > 0)
                {
                    dgStd.Visible = true;
                    pnlPay.Visible = false;
                    dgStd.DataSource = dt;
                    dgStd.DataBind();
                    Session["Payment"] = dt;
                    dgStdShowFooterTotal();
                }
                //else if (dt.Rows.Count == 1)
                //{
                //    clsPaymentMst paymst = clsPaymentManager.getPaymentMst(((DataRow)dt.Rows[0])["payment_id"].ToString());
                //    if (paymst != null)
                //    {
                //        txtPayDate.Text = paymst.PayDate;
                //        ddlPayMode.SelectedValue = paymst.PayMode;
                //        txtStudentId.Text = paymst.StudentId;
                //        txtStudentName.Text = StudentManager.getStudentName(paymst.StudentId);
                //        ddlClassId.SelectedValue = paymst.ClassId;
                //        txtYear.Text = paymst.PayYear;
                //        txtChequeNo.Text = paymst.ChequeNo;
                //        txtChequeDate.Text = paymst.ChequeDate;
                //        txtChequeAmt.Text = paymst.ChequeAmt;
                //        ddlBankNo.SelectedValue = paymst.BankNo;
                //        txtPaymentId.Text = paymst.PaymentId;
                //        txtRefNo.Text = paymst.RefNo;
                //        txtWaiverPct.Text = clsStdWaiverManager.getStudentWaiverPct(paymst.StudentId, paymst.PayYear, paymst.ClassId);


                //        //lblTotalAmount.Text = decimal.Parse(clsPaymentManager.getTutionFee(txtStudentId.Text, txtPayDate.Text)).ToString("N2");
                //        //lblTotalPaid.Text = decimal.Parse(clsPaymentManager.getPaidAmount(txtStudentId.Text, txtPayDate.Text)).ToString("N2");
                //        //lblTotalDue.Text = (decimal.Parse(lblTotalPaid.Text) - decimal.Parse(lblTotalAmount.Text)).ToString("N2");

                //        DataTable dtDtl = clsPaymentManager.getPaymentDtls(txtPaymentId.Text);
                //        if (dtDtl.Rows.Count > 0)
                //        {
                //            dgPay.DataSource = dtDtl;
                //            dgPay.DataBind();
                //            dgPay.FooterRow.Visible = false;
                //            dgPay.ShowFooter = false;
                //            ViewState["paydtl"] = dtDtl;
                //            ShowFooterTotal();
                //        }
                //        else
                //        {
                //            getEmptyGrid();
                //        }
                //        dgStd.Visible = false;
                //        pnlPay.Visible = true;
                //    }
                //    else
                //    {
                //        ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('No payment is made yet for this student!!');", true);
                //    }
                //}
                else
                {
                    dgStd.Visible = false;
                    pnlPay.Visible = true;
                }
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

    protected void txtStudentId_TextChanged(object sender, EventArgs e)
    {
        try
        {

            DataTable dt = clsPaymentManager.getStudentInfo(txtStudentId.Text);
            if (dt.Rows.Count > 0)
            {
                lblName.Text = ": " + ((DataRow)dt.Rows[0])["name"].ToString();
                lblClassId.Text = ((DataRow)dt.Rows[0])["class_id"].ToString();
                lblSectionId.Text = ((DataRow)dt.Rows[0])["sect"].ToString();
                lblVersonId.Text = ((DataRow)dt.Rows[0])["version"].ToString();
                lblShiftId.Text = ((DataRow)dt.Rows[0])["shift"].ToString();
                lblRoll.Text = ((DataRow)dt.Rows[0])["std_roll"].ToString();

                lblClass.Text = ": " + ((DataRow)dt.Rows[0])["class_name"].ToString();
                lblSection.Text = ": " + ((DataRow)dt.Rows[0])["sec_name"].ToString();
                lblRollName.Text = ": " + ((DataRow)dt.Rows[0])["std_roll"].ToString();
                lblShift.Text = ": " + ((DataRow)dt.Rows[0])["shift_name"].ToString();
                lblShiftNew.Text = ((DataRow)dt.Rows[0])["shift_name"].ToString();
                //lblVersion.Text = ": " + ((DataRow)dt.Rows[0])["version_name"].ToString();
                lblRollName.Text = ": " + ((DataRow)dt.Rows[0])["std_roll"].ToString();
                lblYear.Text = ((DataRow)dt.Rows[0])["class_year"].ToString();
                lblYear0.Text = ": " + ((DataRow)dt.Rows[0])["class_year"].ToString();
                lblPaymentYear.Text = ((DataRow)dt.Rows[0])["class_year"].ToString();
                string waiver = clsStdWaiverManager.getStudentWaiverPct(txtStudentId.Text, ((DataRow)dt.Rows[0])["class_year"].ToString(), ((DataRow)dt.Rows[0])["class_id"].ToString());
                txtWaiverPct.Text = waiver;
                txtChequeNo.Focus();

                Payment("");

                UpdatePanelDetails.Update();
                ddlPayMode.Focus();
                UpdatePanelMST.Update();
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", "alert('No Search Student..!!');", true);
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

    protected void dgPay_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            if (ViewState["paydtl"] != null)
            {
                DataTable dt = (DataTable)ViewState["paydtl"];
                DataRow dr1 = dt.Rows[dgPay.Rows[e.RowIndex].DataItemIndex];
                if (dr1["OtCharge_flag"].ToString() == "E")
                {
                    lblTotalAmount.Text = (Convert.ToDouble(lblTotalAmount.Text) - Convert.ToDouble(dr1["pay_amt"])).ToString("N2");
                }
                dt.Rows.Remove(dr1);
                string found = "";
                foreach (DataRow dr in dt.Rows)
                {
                    if (string.IsNullOrEmpty(dr["pay_amt"].ToString()))
                    {
                        found = "Y";
                    }
                }
                if (found == "")
                {
                    DataRow drd = dt.NewRow();
                    dt.Rows.Add(drd);
                }



                dgPay.DataSource = dt;
                dgPay.DataBind();
                ShowFooterTotal();
                //upPayment.Update();
                //UpdatePanel1.Update();
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

        decimal dd = GetTotalDiscount();
        lblCurrentDiscount.Text = dd.ToString("N2");
        decimal ff = GetTotal();
        lblTotalPaid.Text = (Convert.ToDecimal(ff)).ToString("N2");
        lblTotalDue.Text = (Convert.ToDecimal(lblTotalAmount.Text) - (Convert.ToDecimal(ff) + Convert.ToDecimal(lblCurrentDiscount.Text))).ToString();
        upPayment.Update();
        UpdatePanel1.Update();
    }
    public DataTable PopulatePayType()
    {
        DataTable dt = clsPaymentInfoManager.GetShowAllPayInfo(lblClassId.Text, lblVersonId.Text);

        DataRow dr = dt.NewRow();
        dt.Rows.InsertAt(dr, 0);
        //if (ViewState["paydtl"] != null)
        //{
        //    DataTable dtDtl = (DataTable)ViewState["paydtl"];
        //    foreach (GridViewRow row in dgPayType.Rows)
        //    {
        //        double amt = double.Parse(string.IsNullOrEmpty(row.Cells[3].Text.Replace(",", "")) ? "0" : row.Cells[3].Text.Replace(",", ""));
        //        //double amt = double.Parse(string.IsNullOrEmpty(row.Cells[3].Text.Replace(",", "")) ? "0" : row.Cells[3].Text.Replace(",", ""));
        //        //if (amt > 0)
        //        //{
        //        string pname = row.Cells[1].Text.ToString();
        //        if (pname.Contains(" x "))
        //        {
        //            int pos = pname.IndexOf(" x ");
        //            pname = pname.Substring(0, pos);
        //        }
        //        dr = dt.NewRow();
        //        dr[0] = row.Cells[0].Text;
        //        dr[1] = pname;
        //        dt.Rows.Add(dr);
        //        //}
        //    }
        //    DataTable dt1 = clsPaymentInfoManager.GetAdditionalPayments();
        //    foreach (DataRow dr1 in dt1.Rows)
        //    {
        //        dr = dt.NewRow();
        //        dr[0] = dr1["pay_id"].ToString();
        //        dr[1] = dr1["pay_head_id"].ToString();
        //        dt.Rows.Add(dr);
        //    }
        //}
        return dt;
    }

    protected void dgPay_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Attributes.Add("style", "display:none");
                e.Row.Cells[1].Attributes.Add("style", "display:none");
            }
            else if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Attributes.Add("style", "display:none");
                e.Row.Cells[1].Attributes.Add("style", "display:none");
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Attributes.Add("style", "display:none");
                e.Row.Cells[1].Attributes.Add("style", "display:none");
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Attributes.Add("style", "display:none");
                e.Row.Cells[1].Attributes.Add("style", "display:none");
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
    private decimal GetTotal()
    {
        decimal ctot = decimal.Zero;
        if (ViewState["paydtl"] != null)
        {
            DataTable dt = (DataTable)ViewState["paydtl"];
            foreach (DataRow drp in dt.Rows)
            {
                if (drp["pay_head_id"].ToString() != "")
                {
                    ctot += decimal.Parse(drp["pay_amt"].ToString());

                }
            }
            Session["Total"] = ctot;
        }
        if (ddlPayMode.SelectedValue == "Q")
        {
            txtChequeAmt.Text = ctot.ToString("N2");
            UpdatePanelMST.Update();
        }
        else { txtChequeAmt.Text = "0"; }
        return ctot;
    }
    private decimal GetTotalDiscount()
    {
        decimal ctot = decimal.Zero;
        if (ViewState["paydtl"] != null)
        {
            DataTable dt = (DataTable)ViewState["paydtl"];
            foreach (DataRow drp in dt.Rows)
            {
                if (drp["pay_head_id"].ToString() != "")
                {
                    ctot += decimal.Parse(drp["Discount_AMT"].ToString());

                }
            }
            Session["Total"] = ctot;
        }

        return ctot;
    }

    private void ShowFooterTotal()
    {
        ViewState["FoterTotal"] = GetTotal();

        ViewState["FoterTotalDiscount"] = GetTotalDiscount();

        GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Footer, DataControlRowState.Normal);
        TableCell cell = new TableCell();
        cell.Text = "Total";
        row.Cells.Add(cell);
        //cell = new TableCell();
        //cell.Text = "Total";
        //row.Cells.Add(cell);
        cell = new TableCell();
        cell.Text = GetTotal().ToString("N2");
        cell.HorizontalAlign = HorizontalAlign.Right;
        row.Cells.Add(cell);
        cell = new TableCell();
        cell.Text = GetTotalDiscount().ToString("N2");
        cell.HorizontalAlign = HorizontalAlign.Right;
        row.Cells.Add(cell);
        row.Font.Bold = true;
        if (dgPay.Rows.Count > 0)
        {
            dgPay.Controls[0].Controls.Add(row);
        }
        //while using Total field separately
        //row.Attributes.Add("style", "display:none");
    }
    private void dgStdShowFooterTotal()
    {
        decimal ctot = decimal.Zero;
        if (Session["Payment"] != null)
        {
            DataTable dt = (DataTable)Session["Payment"];
            foreach (DataRow drp in dt.Rows)
            {
                if (drp["total_amount"].ToString() != "")
                {
                    ctot += decimal.Parse(drp["total_amount"].ToString());

                }
            }

        }
        GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Footer, DataControlRowState.Normal);
        TableCell cell = new TableCell();
        cell.Text = "";
        row.Cells.Add(cell);
        cell = new TableCell();
        cell.Text = "";
        row.Cells.Add(cell);
        cell = new TableCell();
        cell.Text = "";
        row.Cells.Add(cell);
        cell = new TableCell();
        cell.Text = "";
        row.Cells.Add(cell);
        cell = new TableCell();
        cell.Text = "";
        row.Cells.Add(cell);
        cell = new TableCell();
        cell.Text = "Total";
        row.Cells.Add(cell);
        cell = new TableCell();
        cell.Text = ctot.ToString("N2");
        cell.HorizontalAlign = HorizontalAlign.Right;
        row.Cells.Add(cell);
        row.Font.Bold = true;
        if (dgStd.Rows.Count > 0)
        {
            dgStd.Controls[0].Controls.Add(row);
        }
        //while using Total field separately
        //row.Attributes.Add("style", "display:none");
    }

    protected void dgStd_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dgStd.PageIndex = e.NewPageIndex;
        dgStd.DataSource = Session["Payment"];
        dgStd.DataBind();
    }

    //*************************** ddlPayName_SelectedIndexChanged ***************************************************************************************?????

    protected void ddlPayName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            if (ViewState["paydtl"] != null)
            {
                DropDownList dl = (DropDownList)sender;
                GridViewRow gvr = (GridViewRow)dl.NamingContainer;
                DataTable dt = (DataTable)ViewState["paydtl"];
                DataRow dr1 = dt.Rows[gvr.DataItemIndex];
                bool isCheck = false;
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["pay_id"].ToString() == ((DropDownList)gvr.FindControl("ddlPayName")).SelectedValue)
                    {
                        isCheck = true;
                        break;
                    }
                }
                if (isCheck == true)
                {
                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", "alert('This payment Head Alrady add this list...!!!');", true);
                    ((DropDownList)gvr.FindControl("ddlPayName")).SelectedIndex = -1;
                    //((TextBox)gvr.FindControl("txtPayAmt")).Text = "0";
                    ShowFooterTotal();
                    return;
                }
                string EC = clsPaymentManager.GetShowExtraChareFlag(((DropDownList)gvr.FindControl("ddlPayName")).SelectedValue);
                if (EC == "Y" || EC == "O" || EC == "N")
                {
                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", "alert('This Yearly Charge Cannot Taken One More Time......!!!!');", true);
                    ((DropDownList)gvr.FindControl("ddlPayName")).SelectedIndex = -1;
                    ShowFooterTotal();
                    return;
                }
                dr1.BeginEdit();
                dr1["pay_id"] = ((DropDownList)gvr.FindControl("ddlPayName")).SelectedValue;
                dr1["pay_head_id"] = ((DropDownList)gvr.FindControl("ddlPayName")).SelectedItem.Text;
                //dr1["Previous_Pay"] = "0";
                //****************** Show Hader Amount *******************//
                decimal PayheadNo = aclsPaymentManagerObj.getShowPaymetHeadAmount(((DropDownList)gvr.FindControl("ddlPayName")).SelectedValue);
                ((TextBox)gvr.FindControl("txtPayAmt")).Text = PayheadNo.ToString("N2");
                dr1["pay_amt"] = ((TextBox)gvr.FindControl("txtPayAmt")).Text;
                //***** Get Show Extra Chage ******//

                if (EC == "E")
                {
                    dr1["OtCharge_flag"] = EC;
                    // dr1["Fix_Amt"] = "0";
                    dr1["Fix_Amt"] = ((TextBox)gvr.FindControl("txtPayAmt")).Text;
                    lblTotalAmount.Text = (Convert.ToDouble(lblTotalAmount.Text) + Convert.ToDouble(((TextBox)gvr.FindControl("txtPayAmt")).Text)).ToString("N2");
                    lblGrandTotal.Text = (Convert.ToDouble(lblGrandTotal.Text) + Convert.ToDouble(((TextBox)gvr.FindControl("txtPayAmt")).Text)).ToString("N2");

                    ((TextBox)gvr.FindControl("txtDiscountAmt")).Enabled = false;
                }
                else
                {
                    dr1["OtCharge_flag"] = EC;
                    ((TextBox)gvr.FindControl("txtDiscountAmt")).Enabled = true;
                }
                DataTable dtt = clsPaymentManager.GetShowPaymentDropdown(txtStudentId.Text, lblYear.Text, ((DropDownList)
gvr.FindControl("ddlPayName")).SelectedValue);
                if (dtt.Rows.Count > 0)
                {
                    dr1["Fix_Amt"] = dtt.Rows[0][6].ToString();
                    dr1["Previous_Pay"] = dtt.Rows[0][7].ToString();
                    dr1["Discount_AMT"] = dtt.Rows[0][4].ToString();
                    dr1["Prv_Discount_AMT"] = dtt.Rows[0][8].ToString();
                }
                else
                {
                    dr1["Fix_Amt"] = 0;
                    dr1["Previous_Pay"] = 0;
                    dr1["Discount_AMT"] = 0;
                    dr1["Prv_Discount_AMT"] = 0;
                }                
                string found = "";                
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr1["pay_id"].ToString() == ((DropDownList)gvr.FindControl("ddlPayName")).SelectedValue)
                    {
                        isCheck = true;
                        break;
                    }
                    if (string.IsNullOrEmpty(dr["pay_amt"].ToString()))
                    {
                        found = "Y";

                    }
                }               
                if (found == "")
                {
                    DataRow drd = dt.NewRow();
                    dt.Rows.Add(drd);
                }
                dr1.EndEdit();
                dr1.AcceptChanges();
                dgPay.DataSource = dt;
                dgPay.DataBind();
                ShowFooterTotal();

                upPayment.Update();
                UpdatePanel1.Update();
            }

            decimal dd = GetTotalDiscount();
            lblCurrentDiscount.Text = dd.ToString("N2");
            decimal ff = GetTotal();
            lblTotalPaid.Text = (ff - dd).ToString("N2");
            lblTotalDue.Text = (Convert.ToDecimal(lblTotalAmount.Text) - ff).ToString();
            upPayment.Update();
            UpdatePanel1.Update();
        }
        catch (FormatException fex)
        {
            ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", "alert('" + fex.Message + "');", true);
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("Database"))
                ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", "alert('Database Maintain Error. Contact to the Software Provider..!!');", true);
            else
                ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", "alert('There is some problem to do the task. Try again properly.!!');", true);
        }
    }
   //Session["Amt"] = 0;

    protected void txtPayAmt_TextChanged(object sender, EventArgs e)
    {
        try
        {

            if (ViewState["paydtl"] != null)
            {
                TextBox dl = (TextBox)sender;
                GridViewRow gvr = (GridViewRow)dl.NamingContainer;
                DataTable dt = (DataTable)ViewState["paydtl"];
                DataRow dr1 = dt.Rows[gvr.DataItemIndex];
                dr1.BeginEdit();
                dr1["pay_id"] = ((DropDownList)gvr.FindControl("ddlPayName")).SelectedValue;
                dr1["pay_head_id"] = ((DropDownList)gvr.FindControl("ddlPayName")).SelectedItem.Text;
                Session["Amt"] = dr1["Discount_AMT"];
                ViewState["FixAmt"] = dr1["Fix_Amt"];
                if (Convert.ToDouble(((TextBox)gvr.FindControl("txtDiscountAmt")).Text) > 0)
                {
                    dr1["Discount_AMT"] = ((TextBox)gvr.FindControl("txtDiscountAmt")).Text;
                    dr1["pay_amt"] = (Convert.ToDouble(((TextBox)gvr.FindControl("txtPayAmt")).Text) + Convert.ToDouble(Session["Amt"])) - Convert.ToDouble(((TextBox)gvr.FindControl("txtDiscountAmt")).Text);
                }
                else
                {
                    dr1["Discount_AMT"] = "0";
                    dr1["pay_amt"] = Convert.ToDouble(((TextBox)gvr.FindControl("txtPayAmt")).Text) + Convert.ToDouble(Session["Amt"]);
                }

                dr1.EndEdit();
                dr1.AcceptChanges();
                string found = "";
                foreach (DataRow dr in dt.Rows)
                {
                    if (string.IsNullOrEmpty(dr["pay_amt"].ToString()))
                    {
                        found = "Y";
                    }
                }
                if (found == "")
                {
                    DataRow drd = dt.NewRow();
                    dt.Rows.Add(drd);
                }
                dgPay.DataSource = dt;
                dgPay.DataBind();
                double Amt = IdManager.GetShowSingleValueCurrency("isnull([pay_amt],0)", "pay_id", "payment_info", dr1["pay_id"].ToString());
                if (dr1["OtCharge_flag"].ToString() == "E" && Amt == 0)
                {
                    dr1["Fix_Amt"] = (Convert.ToDouble(ViewState["FixAmt"]) + Convert.ToDouble(((TextBox)gvr.FindControl("txtPayAmt")).Text)) - Convert.ToDouble(ViewState["FixAmt"]);
                    lblTotalAmount.Text = ((Convert.ToDouble(lblTotalAmount.Text) + Convert.ToDouble(((TextBox)gvr.FindControl("txtPayAmt")).Text)) - Convert.ToDouble(ViewState["FixAmt"])).ToString("N2");
                    lblGrandTotal.Text = ((Convert.ToDouble(lblGrandTotal.Text) + Convert.ToDouble(((TextBox)gvr.FindControl("txtPayAmt")).Text)) - Convert.ToDouble(ViewState["FixAmt"])).ToString("N2");
                }
                ShowFooterTotal();
                //GTotal = GetTotal();
                //lblTotalDue.Text = (Convert.ToDecimal(lblGrandTotal.Text) - GTotal).ToString("N2");

            }
            ViewState["FoterTotal"] = GetTotal();
            ViewState["FoterTotalDiscount"] = GetTotalDiscount();
            lblCurrentDiscount.Text = Convert.ToDecimal(ViewState["FoterTotalDiscount"]).ToString("N2");
            lblTotalPaid.Text = ((Convert.ToDecimal(ViewState["FoterTotal"]))).ToString("N2");
            lblTotalDue.Text = (Convert.ToDecimal(lblTotalAmount.Text) - (Convert.ToDecimal(ViewState["FoterTotal"]) + Convert.ToDecimal(ViewState["FoterTotalDiscount"]))).ToString("N2");
            upPayment.Update();
            UpdatePanel1.Update();
            //lblTotalPaid.Text = ((Convert.ToDouble(lblTotalPaid.Text) + Convert.ToDouble(ViewState["FoterTotal"]))).ToString("N2");

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

    protected void dgStd_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clsPaymentMst paymst = clsPaymentManager.getPaymentMst(dgStd.SelectedRow.Cells[1].Text);
            txtPaymentId.Text = paymst.PaymentId;


            dgStd.Visible = false;
            pnlPay.Visible = true;
            Payment(dgStd.SelectedRow.Cells[1].Text);

            // DateTime date1=DataManager.DateEncode(dgStd.SelectedRow.Cells[2].Text);
            DataTable dtDtl = clsPaymentManager.getPaymentDtls(dgStd.SelectedRow.Cells[1].Text, txtStudentId.Text, lblYear.Text);
            if (dtDtl.Rows.Count > 0)
            {
                DataRow dr = dtDtl.NewRow();
                dtDtl.Rows.Add(dr);
                ViewState["paydtl"] = dtDtl;
                dgPay.DataSource = dtDtl;
                dgPay.DataBind();
                ShowFooterTotal();
            }
            else
            {
                getEmptyGrid();
            }

            lblTotalAmount.Text = Convert.ToDecimal(paymst.PayAmt).ToString("N2");
            lblCurrentDiscount.Text = Convert.ToDecimal(paymst.TotalDiscountAmt).ToString("N2");
            //lblTotalPaid.Text = (Convert.ToDecimal(paymst.TotalPaidAmt)).ToString("N2");
            lblTotalPaid.Text = ViewState["FoterTotal"].ToString();
            lblTotalDue.Text = (Convert.ToDecimal(paymst.PayAmt) - (Convert.ToDecimal(paymst.TotalPaidAmt) + Convert.ToDecimal(paymst.TotalDiscountAmt))).ToString("N2");
            txtSearchStudent.Enabled = txtStudentId.Enabled = false;
            btnPrint.Enabled = true;
            btnSave.Enabled = true;
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

    private void clearFields()
    {
        txtPaymentId.Text = "";
        txtPayDate.Text = "";
        ddlPayMode.SelectedIndex = -1;
        txtStudentId.Text = "";
        lblName.Text = "";
        lblClass.Text = "";
        lblClassId.Text = "";
        lblYear.Text = "";
        txtChequeNo.Text = "";
        txtChequeDate.Text = "";
        txtChequeAmt.Text = "";
        ddlBankNo.SelectedIndex = -1;
        txtRefNo.Text = "";
        dgPay.DataBind();
        getEmptyGrid();
    }
    private static Phrase FormatPhrase(string value)
    {
        return new Phrase(value, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10));
    }
    private static Phrase FormatHeaderPhrase(string value)
    {
        return new Phrase(value, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12, iTextSharp.text.Font.BOLD));
    }
    protected void dgPayType_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[0].Attributes.Add("style", "display:none");
    }

    private void Payment(string PaymentID)
    {

        DataTable dt = clsPaymentManager.getStudentClassAndSectionWiseInfo(lblClassId.Text, lblSectionId.Text, lblRoll.Text, lblShiftId.Text, lblVersonId.Text, lblYear.Text, txtStudentId.Text);
        if (dt.Rows.Count > 0)
        {
            lblGrandTotal.Text = lblPreviousDiscount.Text = lblPreviousAmount.Text = lblTotalAmount.Text = lblCurrentDiscount.Text = lblTotalPaid.Text = lblTotalDue.Text = "0";

            txtStudentId.Text = ((DataRow)dt.Rows[0])["student_id"].ToString();
            //lblName.Text = ((DataRow)dt.Rows[0])["name"].ToString();
            lblClassId.Text = ((DataRow)dt.Rows[0])["class_id"].ToString();
            lblYear.Text = ((DataRow)dt.Rows[0])["class_year"].ToString();
            string waiver = clsStdWaiverManager.getStudentWaiverPct(txtStudentId.Text, ((DataRow)dt.Rows[0])["class_year"].ToString(), ((DataRow)dt.Rows[0])["class_id"].ToString());
            txtWaiverPct.Text = waiver;

            lblTotalAmount.Text = decimal.Parse(clsPaymentManager.getTutionFee(txtStudentId.Text, txtPayDate.Text)).ToString("N2");
            lblTotalPaid.Text = (decimal.Parse(clsPaymentManager.getPaidAmount(txtStudentId.Text, txtPayDate.Text))).ToString("N2");
            lblTotalDue.Text = (decimal.Parse(lblTotalPaid.Text) - (decimal.Parse(lblTotalAmount.Text) + decimal.Parse(lblPreviousDiscount.Text))).ToString("N2");
            /*
            DataTable dtpay = clsPaymentManager.GetStdPaymentInfos(txtStudentId.Text, txtYear.Text,txtPayDate.Text);
            */
            //double waive = double.Parse(string.IsNullOrEmpty(waiver) ? "0" : waiver) / 100;

            DataTable dtpay = new DataTable();
            dtpay.Columns.Add("pay_id", typeof(string));
            dtpay.Columns.Add("pay_head_id", typeof(string));
            dtpay.Columns.Add("pay_amt", typeof(string));
            dtpay.Columns.Add("Discount_Amt", typeof(string));
            dtpay.Columns.Add("paid_amt", typeof(string));
            dtpay.Columns.Add("due_amt", typeof(string));
            dtpay.Columns.Add("Fix_Amt", typeof(string));
            dtpay.Columns.Add("OtCharge_flag", typeof(string));
            // dtpay.Columns.Add("Previous_Pay", typeof(string));

            //*********** get Show All  Payment Information ***************//

            DataTable dtpay1 = clsPaymentManager.GetStdCommonPayments(txtStudentId.Text, lblYear.Text, lblVersonId.Text, PaymentID);
            DataRow drpay;
            //DataTable dtSpecial = clsPaymentManager.getSpecialPayment(txtStudentId.Text, ddlClassId.SelectedValue, txtYear.Text);
            foreach (DataRow drt in dtpay1.Rows)
            {
                drpay = dtpay.NewRow();
                drpay["pay_id"] = drt["pay_id"].ToString();
                double payamt = 0;
                double amt = double.Parse(string.IsNullOrEmpty(drt["pay_amt"].ToString()) ? "0" : drt["pay_amt"].ToString());

                if (drt["pay_type"].ToString().Equals("L"))
                {

                    if (drt["discount"].ToString().Equals("Y"))
                    {
                        payamt = Math.Ceiling(amt - (amt * Convert.ToDouble(waiver)));
                        drpay["pay_amt"] = payamt.ToString();
                    }
                    else
                    {
                        double TutionFees = clsPaymentManager.GetShowTitionFees(lblClassId.Text);
                        double TotalLateFees = 0;
                        if (Convert.ToDouble(Session["DueT"]) > 0)
                        {
                            //double totDays = Convert.ToDouble(Session["DueT"]) / TutionFees;
                            var totDays = (int)(Math.Ceiling(Convert.ToDouble(Session["DueT"]) / TutionFees));

                            if (totDays == 1)
                            {
                                if (DateTime.Now.Day > 15 && DateTime.Now.Day <= 30)
                                {
                                    TotalLateFees = totDays * 150;
                                }
                            }
                            else
                            {
                                TotalLateFees += totDays * 200;
                            }
                        }

                        payamt = Convert.ToDouble(drt["paid_amt"]) + TotalLateFees;
                        drpay["pay_amt"] = Convert.ToDouble(drt["paid_amt"]) + TotalLateFees;
                    }
                    drpay["pay_head_id"] = drt["pay_head_id"].ToString();

                }

                if (drt["pay_type"].ToString().Equals("Y"))
                {
                    if (drt["discount"].ToString().Equals("Y"))
                    {
                        payamt = Math.Ceiling(amt - (amt * Convert.ToDouble(waiver)));
                        drpay["pay_amt"] = payamt.ToString();
                    }
                    else
                    {
                        payamt = amt;
                        drpay["pay_amt"] = payamt.ToString();
                    }
                    drpay["pay_head_id"] = drt["pay_head_id"].ToString();
                }
                if (drt["pay_type"].ToString().Equals("M"))
                {
                    // int mon = (DataManager.DateEncode(txtPayDate.Text).Month) - (DataManager.DateEncode(drt["class_start"].ToString()).Month);
                    // int mon = Convert.ToInt32(Convert.ToDateTime(txtPayDate.Text).Subtract(Convert.ToDateTime(drt["class_start"])).Days / (365.25 / 12));
                    //int mon = Convert.ToInt32((Convert.ToDateTime(txtPayDate.Text)- Convert.ToDateTime(drt["class_start"])).to);
                    //int mon1 = DataManager.DateEncode(txtPayDate.Text).Year;
                    //int mon2 = DataManager.DateEncode(txtPayDate.Text).Month;
                    //int mon3 = DataManager.DateEncode(drt["class_start"].ToString()).Year;
                    //int mon4 = DataManager.DateEncode(drt["class_start"].ToString()).Month;

                    //********************** Waiver Date ************************//
                    int month = 0;
                    string StartDate;
                    string EndDate;
                    DataTable dtWaiver = clsPaymentManager.getWaiverInformation(txtStudentId.Text, lblClassId.Text, lblYear.Text);
                    if (dtWaiver.Rows.Count > 0)
                    {
                        DataRow rowWaber = dtWaiver.Rows[0];
                        if ((DataManager.DateEncode(drt["class_start"].ToString()) > DataManager.DateEncode(rowWaber["EXC_FROM"].ToString()) && DataManager.DateEncode(drt["class_start"].ToString()) > DataManager.DateEncode(rowWaber["EXC_TO"].ToString())) || (DataManager.DateEncode(txtPayDate.Text)) < DataManager.DateEncode(rowWaber["EXC_FROM"].ToString()) && DataManager.DateEncode(txtPayDate.Text) < DataManager.DateEncode(rowWaber["EXC_TO"].ToString()))
                        {
                            month = 0;
                        }
                        else
                        {
                            if (DataManager.DateEncode(rowWaber["EXC_FROM"].ToString()) < DataManager.DateEncode(drt["class_start"].ToString()))
                            {
                                StartDate = drt["class_start"].ToString();
                            }
                            else
                            {
                                StartDate = rowWaber["EXC_FROM"].ToString();
                            }

                            if (DataManager.DateEncode(rowWaber["EXC_TO"].ToString()) > DataManager.DateEncode(txtPayDate.Text))
                            {
                                EndDate = txtPayDate.Text;
                            }
                            else
                            {
                                EndDate = rowWaber["EXC_TO"].ToString();
                            }

                            month = (((DataManager.DateEncode(EndDate).Year - (DataManager.DateEncode(StartDate).Year)) * 12) + DataManager.DateEncode(EndDate).Month - DataManager.DateEncode(StartDate).Month) + 1;
                        }

                    }
                    ////int mon = (((DataManager.DateEncode(txtPayDate.Text).Year - (DataManager.DateEncode(drt["class_start"].ToString()).Year)) * 12) + DataManager.DateEncode(txtPayDate.Text).Month - DataManager.DateEncode(drt["class_start"].ToString()).Month) + 1;
                    //if (drt["discount"].ToString().Equals("Y"))
                    //{
                    //    payamt = Math.Ceiling((amt * mon) - (Convert.ToDouble(waiver) * month));
                    //    drpay["pay_amt"] = payamt.ToString();
                    //}
                    //else
                    //{
                    //    payamt = (amt * mon);
                    //    drpay["pay_amt"] = payamt.ToString();
                    //}
                    //drpay["pay_head_id"] = drt["pay_head_id"].ToString() + " x " + mon.ToString() + " month(s)";
                    ////drpay["pay_head_id"] = drt["pay_head_id"].ToString();
                }

                //******************** Extra Charge *************//

                if (drt["pay_type"].ToString().Equals("E"))
                {
                    drpay["pay_head_id"] = drt["pay_head_id"].ToString();
                    //drpay["pay_amt"] = drt["paid_amt"].ToString();
                    drpay["Fix_Amt"] = drt["pay_amt"].ToString();
                    double tt = Convert.ToDouble(drt["pay_amt"]);
                    if (tt < 0)
                    { drpay["pay_amt"] = drt["paid_amt"].ToString(); payamt = Convert.ToDouble(drt["paid_amt"].ToString()); }
                    else if (tt == 0)
                    { drpay["pay_amt"] = drt["paid_amt"].ToString(); payamt = Convert.ToDouble(drt["paid_amt"].ToString()); }
                    else
                    { drpay["pay_amt"] = drt["pay_amt"].ToString(); payamt = Convert.ToDouble(drt["pay_amt"].ToString()); }

                    //if (drt["paid_amt"].ToString() != "")
                    //{
                    //    payamt = Convert.ToDouble(drt["paid_amt"].ToString());
                    //}
                }
                //***************** Transport Fees *******************//

                if (drt["pay_type"].ToString().Trim().Equals("TR"))
                {
                    drpay["pay_head_id"] = drt["pay_head_id"].ToString();
                    drpay["Fix_Amt"] = drt["pay_amt"].ToString();
                    drpay["pay_amt"] = drt["pay_amt"].ToString();
                    payamt = Convert.ToDouble(drt["pay_amt"].ToString());
                }
                double paidamt = double.Parse(string.IsNullOrEmpty(drt["paid_amt"].ToString()) ? "0" : drt["paid_amt"].ToString());
                double Discountamt = double.Parse(string.IsNullOrEmpty(drt["Discount_AMT"].ToString()) ? "0" : drt["Discount_AMT"].ToString());

                drpay["Fix_Amt"] = drt["pay_amt"].ToString();
                drpay["OtCharge_flag"] = drt["pay_type"].ToString();
                drpay["Discount_AMT"] = Discountamt.ToString();
                drpay["paid_amt"] = paidamt.ToString();
                drpay["due_amt"] = (payamt - (paidamt + Discountamt)).ToString();
                dtpay.Rows.Add(drpay);
            }
            DataTable dtpay2 = clsPaymentManager.GetStdSpecialPayments(txtStudentId.Text, txtPayDate.Text);
            foreach (DataRow drt in dtpay2.Rows)
            {
                drpay = dtpay.NewRow();
                drpay["pay_id"] = drt["pay_id"].ToString();
                drpay["pay_head_id"] = drt["pay_head_id"].ToString();
                drpay["Discount_AMT"] = drt["Discount_AMT"].ToString();
                drpay["pay_amt"] = drt["pay_amt"].ToString();
                drpay["paid_amt"] = drt["paid_amt"].ToString();
                drpay["due_amt"] = drt["due_amt"].ToString();
                dtpay.Rows.Add(drpay);
            }
            dgPayType.DataSource = dtpay;
            dgPayType.DataBind();
            double tot = 0;
            double Discount = 0;
            double paid = 0;
            double due = 0;
            double PrvDiscount = 0;
            DataTable paydtl = (DataTable)ViewState["paydtl"];
            paydtl.Rows.Clear();
            DataRow drp;
            foreach (DataRow dr in dtpay.Rows)
            {
                tot += double.Parse(string.IsNullOrEmpty(dr["pay_amt"].ToString()) ? "0" : dr["pay_amt"].ToString());
                Discount += double.Parse(string.IsNullOrEmpty(dr["Discount_AMT"].ToString()) ? "0" : dr["Discount_AMT"].ToString());
                paid += double.Parse(string.IsNullOrEmpty(dr["paid_amt"].ToString()) ? "0" : dr["paid_amt"].ToString());
                due += double.Parse(string.IsNullOrEmpty(dr["due_amt"].ToString()) ? "0" : dr["due_amt"].ToString());
                if (PaymentID == "")
                {
                    if (double.Parse(string.IsNullOrEmpty(dr["due_amt"].ToString()) ? "0" : dr["due_amt"].ToString()) > 0)
                    {
                        drp = paydtl.NewRow();
                        drp[0] = dr["pay_id"].ToString();
                        drp[1] = dr["pay_head_id"].ToString();
                        drp[2] = dr["due_amt"].ToString();
                        //drp[3] = dr["Discount_AMT"].ToString();
                        drp[3] = "0";
                        drp[5] = dr["pay_amt"].ToString();
                        //if (Convert.ToDouble(dr["pay_amt"]) > 0)
                        //{
                        //    drp[6] = (Convert.ToDouble(dr["Fix_Amt"]) - Convert.ToDouble(dr["due_amt"])).ToString("N0");
                        drp[6] = dr["paid_amt"].ToString();
                        drp[4] = dr["OtCharge_flag"].ToString();
                        PrvDiscount += Convert.ToDouble(dr["Discount_AMT"].ToString());
                        //}
                        //else
                        //{
                        //    drp[6] = "0";
                        //}
                        paydtl.Rows.Add(drp);
                    }
                }
            }
            ViewState["Discount"] = PrvDiscount;
            drp = paydtl.NewRow();
            paydtl.Rows.Add(drp);
            dgPay.DataSource = paydtl;
            // need check.....................
            dgPay.DataBind();
            ShowFooterTotal();


            lblGrandTotal.Text = tot.ToString("N2");
            //decimal paidamount=decimal.Parse(clsPaymentManager.getPaidAmount(txtStudentId.Text, txtPayDate.Text));
            if (paid > 0)
            {
                lblTotalAmount.Text = due.ToString("N2");
                lblCurrentDiscount.Text = GetTotalDiscount().ToString("N2");
                lblPreviousDiscount.Text = Discount.ToString("N2");
                lblPreviousAmount.Text = (paid).ToString("N2");
                lblTotalDue.Text = ((Convert.ToDecimal(lblGrandTotal.Text) - (Convert.ToDecimal(lblPreviousAmount.Text) + Convert.ToDecimal(lblPreviousDiscount.Text)))).ToString("N2");
            }
            else
            {
                lblTotalAmount.Text = tot.ToString("N2");
                lblCurrentDiscount.Text = (paid - Discount).ToString("N2");
                lblTotalDue.Text = ((Convert.ToDecimal(lblGrandTotal.Text) - (Convert.ToDecimal(lblPreviousAmount.Text) + Convert.ToDecimal(lblTotalAmount.Text)))).ToString("N2");
            }
            lblPreviousDiscount.Text = Discount.ToString("N2");
            lblTotalPaid.Text = due.ToString("N2");



            upPayment.Update();
            UpdatePanel1.Update();
        }
        else
        {
            Student std = StudentManager.getStd(txtStudentId.Text);
            if (std == null)
            {
                lblName.Text = "";
                lblClassId.Text = "";
                lblYear.Text = "";
                lblTotalAmount.Text = "";
                lblTotalPaid.Text = "";
                lblTotalDue.Text = "";
                lblCurrentDiscount.Text = "";
                lblPreviousDiscount.Text = "";

                // upPayment.Update();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ale", "alert('No Such Student!!');", true);
            }
            else
            {
                lblName.Text = StudentManager.getStudentName(std.StudentId);
                lblClassId.Text = "";
                lblYear.Text = "";
                // upPayment.Update();
            }
        }
        //DataTable Hdt = StudentManager.GetShowPaymentInformation(txtStudentId.Text, ddlClassId.SelectedValue, txtYear.Text);
        //dgPaymentHistory.DataSource = Hdt;
        //dgPaymentHistory.DataBind();

        upPayment.Update();
        UpdatePanel1.Update();
    }    
    protected void btnPPos_Click(object sender, EventArgs e)
    {

       // PrintDocument pd = new PrintDocument();
       // pd.PrintPage += PrintPage;
       // //here to select the printer attached to user PC
       // //PrintDialog printDialog1 = new PrintDialog();
       //// printDialog1.Document = pd;
       //// DialogResult result = printDialog1.ShowDialog();
       // //if (result == DialogResult.OK)
       // //{
       //     pd.Print();//this will trigger the Print Event handeler PrintPage
      //  }

        PrintDocument printDocument = new PrintDocument();
        printDocument.PrintPage += new PrintPageEventHandler(pdoc_PrintPage);
        printDocument.Print();   

        //Student std = StudentManager.getStd(txtStudentId.Text);
        //clsPaymentMst paymst = clsPaymentManager.getPaymentMst(txtPaymentId.Text);

        ////  DataRow row = dt.Rows[0];
        //Session.Remove("rptbyte");
        //Response.Clear();
        //Document document = new Document();
        //float pheight = (float)(14.8 / 2.54) * 72;
        //float pwidth = (float)(8.1 / 2.54) * 72;
        //document = new Document(new iTextSharp.text.Rectangle(pwidth, pheight), 10f, 10f, 10f, 10f);
        //MemoryStream ms = new MemoryStream();
        //PdfWriter writer = PdfWriter.GetInstance(document, ms);
        ////pdfPage page = new pdfPage();
        ////writer.PageEvent = page;
        //document.Open();

        //PdfPCell cell;
        //float[] titwidth = new float[2] { 30, 200 };
        //PdfPTable dth = new PdfPTable(titwidth);
        //dth.WidthPercentage = 100;

        //byte[] logo = GlBookManager.GetGlLogo(Session["book"].ToString());
        //iTextSharp.text.Image gif = iTextSharp.text.Image.GetInstance(logo);
        //gif.Alignment = iTextSharp.text.Image.MIDDLE_ALIGN;
        //gif.ScalePercent(15f);
        //cell = new PdfPCell(gif);
        //cell.HorizontalAlignment = 1;
        //cell.VerticalAlignment = 1;
        //cell.Rowspan = 3;
        //cell.BorderWidth = 0f;
        //dth.AddCell(cell);

        //cell = new PdfPCell(new Phrase(Session["org"].ToString(), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12, iTextSharp.text.Font.BOLD)));
        //cell.HorizontalAlignment = 1;
        //cell.VerticalAlignment = 1;
        //cell.BorderWidth = 0f;
        ////cell.FixedHeight = 20f;
        //dth.AddCell(cell);
        //cell = new PdfPCell(new Phrase(Session["add1"].ToString(), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL)));
        //cell.HorizontalAlignment = 1;
        //cell.VerticalAlignment = 1;
        //cell.BorderWidth = 0f;
        ////cell.FixedHeight = 20f;
        //dth.AddCell(cell);
        //cell = new PdfPCell(new Phrase("Student Payment Reciept", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 11, iTextSharp.text.Font.BOLD)));
        //cell.HorizontalAlignment = 1;
        //cell.VerticalAlignment = 1;
        //cell.BorderWidth = 0f;
        //dth.AddCell(cell);

        //document.Add(dth);
        //LineSeparator line = new LineSeparator(0f, 100, null, Element.ALIGN_CENTER, -2);
        //document.Add(line);
        //PdfPTable dtempty = new PdfPTable(1);
        //cell = new PdfPCell(FormatHeaderPhrase(""));
        //cell.BorderWidth = 0f;
        //cell.FixedHeight = 5f;
        //dtempty.AddCell(cell);
        //document.Add(dtempty);

        //float[] width = new float[7] { 50, 5, 60, 5, 20, 5, 40 };
        //PdfPTable dtm = new PdfPTable(width);
        //dtm.WidthPercentage = 100;
        //cell = new PdfPCell(new Phrase("MR No", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
        //cell.HorizontalAlignment = 3;
        //cell.VerticalAlignment = 1;
        //cell.BorderWidth = 0f;
        ////cell.FixedHeight = 15f;
        //dtm.AddCell(cell);
        //cell = new PdfPCell(new Phrase(":", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
        //cell.HorizontalAlignment = 3;
        //cell.VerticalAlignment = 1;
        //cell.BorderWidth = 0f;
        //// cell.FixedHeight = 15f;
        //dtm.AddCell(cell);
        //cell = new PdfPCell(new Phrase(txtPaymentId.Text, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
        //cell.HorizontalAlignment = 3;
        //cell.VerticalAlignment = 1;
        //cell.BorderWidth = 0f;
        ////cell.FixedHeight = 15f;
        ////cell.Colspan = 3;
        //dtm.AddCell(cell);
        //cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
        //cell.HorizontalAlignment = 3;
        //cell.VerticalAlignment = 1;
        //cell.BorderWidth = 0f;
        ////cell.FixedHeight = 15f;
        //dtm.AddCell(cell);
        //cell = new PdfPCell(new Phrase("Date", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
        //cell.HorizontalAlignment = 3;
        //cell.VerticalAlignment = 1;
        //cell.BorderWidth = 0f;
        ////cell.FixedHeight = 15f;
        //dtm.AddCell(cell);
        //cell = new PdfPCell(new Phrase(":", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
        //cell.HorizontalAlignment = 3;
        //cell.VerticalAlignment = 1;
        //cell.BorderWidth = 0f;
        ////cell.FixedHeight = 15f;
        //dtm.AddCell(cell);
        //cell = new PdfPCell(new Phrase(txtPayDate.Text, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
        //cell.HorizontalAlignment = 3;
        //cell.VerticalAlignment = 1;
        //cell.BorderWidth = 0f;
        ////cell.FixedHeight = 15f;
        //dtm.AddCell(cell);

        //cell = new PdfPCell(new Phrase("Student ID", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
        //cell.HorizontalAlignment = 0;
        //cell.VerticalAlignment = 1;
        //cell.BorderWidth = 0f;
        ////cell.FixedHeight = 15f;
        //dtm.AddCell(cell);
        //cell = new PdfPCell(new Phrase(":", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
        //cell.HorizontalAlignment = 3;
        //cell.VerticalAlignment = 1;
        //cell.BorderWidth = 0f;
        //// cell.FixedHeight = 15f;
        //dtm.AddCell(cell);
        //cell = new PdfPCell(new Phrase(std.StudentId, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 7, iTextSharp.text.Font.NORMAL)));
        //cell.HorizontalAlignment = 0;
        //cell.VerticalAlignment = 1;
        //cell.BorderWidth = 0f;

        //dtm.AddCell(cell);
        //cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
        //cell.HorizontalAlignment = 3;
        //cell.VerticalAlignment = 1;
        //cell.BorderWidth = 0f;
        //// cell.FixedHeight = 15f;
        //dtm.AddCell(cell);
        //cell = new PdfPCell(new Phrase("Class", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
        //cell.HorizontalAlignment = 3;
        //cell.VerticalAlignment = 1;
        //cell.BorderWidth = 0f;
        //// cell.FixedHeight = 15f;
        //dtm.AddCell(cell);
        //cell = new PdfPCell(new Phrase(":", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
        //cell.HorizontalAlignment = 3;
        //cell.VerticalAlignment = 1;
        //cell.BorderWidth = 0f;
        //// cell.FixedHeight = 15f;
        //dtm.AddCell(cell);
        //cell = new PdfPCell(new Phrase(paymst.ClassName, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
        //cell.HorizontalAlignment = 3;
        //cell.VerticalAlignment = 1;
        //cell.BorderWidth = 0f;
        //// cell.FixedHeight = 15f;
        //dtm.AddCell(cell);

        //cell = new PdfPCell(new Phrase("Student Name", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
        //cell.HorizontalAlignment = 0;
        //cell.VerticalAlignment = 1;
        //cell.BorderWidth = 0f;
        //// cell.FixedHeight = 15f;
        //dtm.AddCell(cell);
        //cell = new PdfPCell(new Phrase(":", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
        //cell.HorizontalAlignment = 3;
        //cell.VerticalAlignment = 1;
        //cell.BorderWidth = 0f;
        ////cell.FixedHeight = 15f;
        //dtm.AddCell(cell);

        //cell = new PdfPCell(new Phrase(StudentManager.getStudentName(std.StudentId), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 7, iTextSharp.text.Font.NORMAL)));
        //cell.HorizontalAlignment = 3;
        //cell.VerticalAlignment = 1;
        //cell.BorderWidth = 0f;
        //// cell.FixedHeight = 15f;
        //cell.Colspan = 5;
        //dtm.AddCell(cell);

        //cell = new PdfPCell(new Phrase("Section", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
        //cell.HorizontalAlignment = 3;
        //cell.VerticalAlignment = 1;
        //cell.BorderWidth = 0f;
        ////cell.FixedHeight = 15f;
        //dtm.AddCell(cell);
        //cell = new PdfPCell(new Phrase(":", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
        //cell.HorizontalAlignment = 3;
        //cell.VerticalAlignment = 1;
        //cell.BorderWidth = 0f;
        ////  cell.FixedHeight = 15f;
        //dtm.AddCell(cell);
        //cell = new PdfPCell(new Phrase(paymst.SectionName, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
        //cell.HorizontalAlignment = 3;
        //cell.VerticalAlignment = 1;
        //cell.BorderWidth = 0f;
        ////  cell.FixedHeight = 15f;
        //dtm.AddCell(cell);
        //cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
        //cell.HorizontalAlignment = 3;
        //cell.VerticalAlignment = 1;
        //cell.BorderWidth = 0f;
        ////cell.FixedHeight = 15f;
        //dtm.AddCell(cell);
        //cell = new PdfPCell(new Phrase("Roll", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
        //cell.HorizontalAlignment = 3;
        //cell.VerticalAlignment = 1;
        //cell.BorderWidth = 0f;
        //// cell.FixedHeight = 15f;
        //dtm.AddCell(cell);
        //cell = new PdfPCell(new Phrase(":", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
        //cell.HorizontalAlignment = 3;
        //cell.VerticalAlignment = 1;
        //cell.BorderWidth = 0f;
        //// cell.FixedHeight = 15f;
        //dtm.AddCell(cell);
        //cell = new PdfPCell(new Phrase(ddlRollNo.SelectedItem.Text, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
        //cell.HorizontalAlignment = 3;
        //cell.VerticalAlignment = 1;
        //cell.BorderWidth = 0f;
        ////cell.FixedHeight = 15f;
        //dtm.AddCell(cell);
        //document.Add(dtm);

        //decimal tot = decimal.Zero;
        //decimal tot3 = decimal.Zero;
        //decimal tot1 = decimal.Zero;
        //decimal tot2 = decimal.Zero;

        //float[] swidth = new float[2] { 60, 30 };
        //PdfPTable pdtPay = new PdfPTable(swidth);
        //pdtPay.WidthPercentage = 100;

        //cell = new PdfPCell(new Phrase("Fee. Particulars", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.BOLD)));
        //cell.HorizontalAlignment = 3;
        //cell.VerticalAlignment = 1;
        ////cell.BorderColor = BaseColor.LIGHT_GRAY;
        //pdtPay.AddCell(cell);

        //cell = new PdfPCell(new Phrase("Paid Amount", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.BOLD)));
        //cell.HorizontalAlignment = 2;
        //cell.VerticalAlignment = 1;
        ////cell.BorderColor = BaseColor.LIGHT_GRAY;
        //pdtPay.AddCell(cell);

        //DataTable dtPay = (DataTable)ViewState["paydtl"];

        //foreach (DataRow dr in dtPay.Rows)
        //{
        //    if (dr["pay_amt"].ToString() != "")
        //    {
        //        cell = new PdfPCell(new Phrase(dr["pay_head_id"].ToString(), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)));
        //        cell.HorizontalAlignment = 3;
        //        cell.VerticalAlignment = 1;
        //        //cell.BorderColor = BaseColor.LIGHT_GRAY;
        //        pdtPay.AddCell(cell);

        //        //decimal Payment = decimal.Parse(dr["Fix_Amt"].ToString());
        //        //decimal PreviousAmt = decimal.Parse(dr["Previous_Pay"].ToString());
        //        //decimal Discount = decimal.Parse(dr["Discount_AMT"].ToString());
        //        //decimal totalamount = decimal.Parse(dr["pay_amt"].ToString());   


        //        cell = new PdfPCell(new Phrase(decimal.Parse(dr["pay_amt"].ToString().Replace(",", "")).ToString("N2"), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)));
        //        cell.HorizontalAlignment = 2;
        //        cell.VerticalAlignment = 1;
        //        //cell.BorderColor = BaseColor.LIGHT_GRAY;
        //        pdtPay.AddCell(cell);

        //        tot += decimal.Parse(dr["Fix_Amt"].ToString().Replace(",", ""));
        //        tot3 += decimal.Parse(dr["Previous_Pay"].ToString().Replace(",", ""));
        //        tot1 += decimal.Parse(dr["Discount_AMT"].ToString().Replace(",", ""));
        //        tot2 += decimal.Parse(dr["pay_amt"].ToString().Replace(",", ""));

        //    }
        //}
        //string DueAmount = (Convert.ToDecimal(lblTotalAmount.Text) - (Convert.ToDecimal(lblTotalPaid.Text) + Convert.ToDecimal(lblCurrentDiscount.Text))).ToString("N2");
        //float[] swidth1 = new float[2] { 180, 90 };
        //PdfPTable pdtTot = new PdfPTable(swidth1);
        //pdtTot.WidthPercentage = 100;
        ////pdtTot.HorizontalAlignment = 0;
        //cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.BOLD)));
        //cell.HorizontalAlignment = 3;
        //cell.VerticalAlignment = 1;
        ////cell.BorderColor = BaseColor.LIGHT_GRAY;
        //cell.Colspan = 2;
        //cell.Border = 0;
        //pdtTot.AddCell(cell);

        //cell = new PdfPCell(new Phrase("Payment Status", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.BOLD)));
        //cell.HorizontalAlignment = 3;
        //cell.VerticalAlignment = 1;
        ////cell.BorderColor = BaseColor.LIGHT_GRAY;
        //cell.Colspan = 2;
        //pdtTot.AddCell(cell);
        //cell = new PdfPCell(new Phrase("Total Amount to Pay", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)));
        //cell.HorizontalAlignment = 3;
        //cell.VerticalAlignment = 1;
        ////cell.BorderColor = BaseColor.LIGHT_GRAY;
        //pdtTot.AddCell(cell);

        ////lblTotalAmount.Text disabled because the student payed more than due
        //cell = new PdfPCell(new Phrase(decimal.Parse((tot).ToString()).ToString("N2"), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL)));
        //cell.HorizontalAlignment = 2;
        //cell.VerticalAlignment = 1;
        ////cell.BorderColor = BaseColor.LIGHT_GRAY;
        //pdtTot.AddCell(cell);

        //cell = new PdfPCell(new Phrase("Now Paid Amount", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)));
        //cell.HorizontalAlignment = 3;
        //cell.VerticalAlignment = 1;
        ////cell.BorderColor = BaseColor.LIGHT_GRAY;
        //pdtTot.AddCell(cell);
        //cell = new PdfPCell(new Phrase((tot2).ToString("N2"), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)));
        //cell.HorizontalAlignment = 2;
        //cell.VerticalAlignment = 1;
        ////cell.BorderColor = BaseColor.LIGHT_GRAY;
        //pdtTot.AddCell(cell);

        //cell = new PdfPCell(new Phrase("Discount Amount", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)));
        //cell.HorizontalAlignment = 3;
        //cell.VerticalAlignment = 1;
        ////cell.BorderColor = BaseColor.LIGHT_GRAY;
        //pdtTot.AddCell(cell);

        //cell = new PdfPCell(new Phrase((tot1).ToString("N2"), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)));
        //cell.HorizontalAlignment = 2;
        //cell.VerticalAlignment = 1;
        ////cell.BorderColor = BaseColor.LIGHT_GRAY;
        //pdtTot.AddCell(cell);

        //cell = new PdfPCell(new Phrase("Previous Amount", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)));
        //cell.HorizontalAlignment = 3;
        //cell.VerticalAlignment = 1;
        ////cell.BorderColor = BaseColor.LIGHT_GRAY;
        //pdtTot.AddCell(cell);

        //cell = new PdfPCell(new Phrase((tot3).ToString("N2"), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)));
        //cell.HorizontalAlignment = 2;
        //cell.VerticalAlignment = 1;
        ////cell.BorderColor = BaseColor.LIGHT_GRAY;
        //pdtTot.AddCell(cell);

        //cell = new PdfPCell(new Phrase("Total Paid Amount", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)));
        //cell.HorizontalAlignment = 3;
        //cell.VerticalAlignment = 1;
        ////cell.BorderColor = BaseColor.LIGHT_GRAY;
        //pdtTot.AddCell(cell);

        //cell = new PdfPCell(new Phrase((Convert.ToDecimal(tot2)).ToString("N2"), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)));
        //cell.HorizontalAlignment = 2;
        //cell.VerticalAlignment = 1;
        ////cell.BorderColor = BaseColor.LIGHT_GRAY;
        //pdtTot.AddCell(cell);

        //cell = new PdfPCell(new Phrase("Total Due", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)));
        //cell.HorizontalAlignment = 3;
        //cell.VerticalAlignment = 1;
        ////cell.BorderColor = BaseColor.LIGHT_GRAY;
        //pdtTot.AddCell(cell);
        ////lblTotalDue.Text disabled because the student payed more than due

        //cell = new PdfPCell(new Phrase((DueAmount), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)));
        //cell.HorizontalAlignment = 2;
        //cell.VerticalAlignment = 1;
        ////cell.BorderColor = BaseColor.LIGHT_GRAY;
        //pdtTot.AddCell(cell);

        //document.Add(pdtPay);
        ////document.Add(dtempty);      

        //document.Add(pdtTot);


        //document.Close();
        //byte[] byt = ms.GetBuffer();
        //if (Session["rptbyte"] != null) { byte[] rptbyt = (byte[])Session["rptbyte"]; rptbyt = byt; } else { Session["rptbyte"] = byt; }

        //string strJS = ("<script type='text/javascript'>window.open('Default4.aspx','_blank');</script>");
        //Page.ClientScript.RegisterStartupScript(this.GetType(), "strJSAlert", strJS);


    }
    
    private void pdoc_PrintPage(object sender, PrintPageEventArgs e)
    {
        Student std = StudentManager.getStd(txtStudentId.Text);
        clsPaymentMst paymst = clsPaymentManager.getPaymentMst(txtPaymentId.Text);
        decimal tot = decimal.Zero;
        decimal tot3 = decimal.Zero;
        decimal tot1 = decimal.Zero;
        decimal tot2 = decimal.Zero;
        DataTable dtPay = (DataTable)ViewState["paydtl"];
        if (dtPay.Rows.Count > 0)
        {           
            //for (int i = 0; i < dtPay.Rows.Count; i++)
            //{
              
            //}
            string PaidAmount = (Convert.ToDecimal(lblTotalAmount.Text) - Convert.ToDecimal(lblCurrentDiscount.Text)).ToString("N2");
            string DueAmount = (Convert.ToDecimal(lblTotalAmount.Text) - (Convert.ToDecimal(lblTotalPaid.Text) + Convert.ToDecimal(lblCurrentDiscount.Text))).ToString("N2");

            System.Drawing.Graphics graphics = e.Graphics;
            System.Drawing.Font fontSize7 = new System.Drawing.Font("Courier New", 7);
            System.Drawing.Font fontSize7Bold = new System.Drawing.Font("Courier New", 7, FontStyle.Bold);
            float fontHeight = fontSize7.GetHeight();
            int startX = 10, startY = 25, offset = 20;
            //string Path = ConfigurationManager.AppSettings["FilePath"];

            byte[] logo = GlBookManager.GetGlLogo(Session["book"].ToString());
            MemoryStream ms = new MemoryStream(logo);
            //Image returnImage = Image.FromStream(ms);
            System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
            //gif.Alignment = iTextSharp.text.Image.MIDDLE_ALIGN;
            //gif.ScalePercent(35f);

            //System.Drawing.Image img = System.Drawing.Image.FromFile(@"D:\KHSC(24-01-2016)\KHSC\img\AMB.jpg");

            //Adjust the size of the image to the page to print the full image without loosing any part of it
            //System.Drawing.Rectangle m = e.MarginBounds;          
            //m.Height = 20;            
            //m.Width = 20;      
            e.Graphics.DrawImage(img, 90, 5,40,40);

            graphics.DrawString("Kurmitola High School & College (KHSC)", new System.Drawing.Font("Courier New", 7, FontStyle.Bold),
                                new SolidBrush(Color.Black), startX, startY + offset);

            offset = offset + 15;

            graphics.DrawString("Khilkhet-2,Plot Area,Dhaka-1216", fontSize7,
                                new SolidBrush(Color.Black), startX + 20, startY + offset);
            offset = offset + 15;

            graphics.DrawString("Student Payment Reciept", new System.Drawing.Font("Courier New", 7, FontStyle.Bold),
                                new SolidBrush(Color.Black), startX + 30, startY + offset);
           
            offset = offset + 12;
            graphics.DrawLine(new Pen(Color.Black, Convert.ToSingle(0.5)), startX, startY + offset, startX + 230, startY + offset); 

            Pen pen1 = new Pen(Color.Black, Convert.ToSingle(0.5));
            pen1.DashPattern = new float[] { 2f, 1f };

            graphics.DrawString("MR No: ", fontSize7,
                   new SolidBrush(Color.Black), startX, startY + offset);

            graphics.DrawString(txtPaymentId.Text, fontSize7,
                    new SolidBrush(Color.Black), startX + 40, startY + offset);

            //graphics.DrawString("Date " + ":" + dr["In_DateTime"].ToString(), fontSize7,
            //         new SolidBrush(Color.Black), startX + 140, startY + offset);
            graphics.DrawString("Date " + ":" + txtPayDate.Text, fontSize7,
                         new SolidBrush(Color.Black), startX + 95, startY + offset);
            offset = offset + 12;

            //graphics.DrawString("Date :", fontSize7,new SolidBrush(Color.Black), startX, startY + offset);

            //graphics.DrawString(txtPayDate.Text, fontSize7,new SolidBrush(Color.Black), startX + 80, startY + offset);

            //graphics.DrawString("", fontSize7,
            //     new SolidBrush(Color.Black), startX, startY + offset);

            //offset = offset + 12;
            graphics.DrawString("Student ID: " + std.StudentId, fontSize7,
                 new SolidBrush(Color.Black), startX, startY + offset);

            graphics.DrawString("", fontSize7,
                    new SolidBrush(Color.Black), startX + 40, startY + offset);

            //graphics.DrawString("Date " + ":" + dr["In_DateTime"].ToString(), fontSize7,
            //         new SolidBrush(Color.Black), startX + 140, startY + offset);
            graphics.DrawString("", fontSize7,
                         new SolidBrush(Color.Black), startX + 95, startY + offset);
            offset = offset + 12;

            graphics.DrawString("Student Name: " + lblName.Text, fontSize7,
                new SolidBrush(Color.Black), startX, startY + offset);

            graphics.DrawString("", fontSize7,
                    new SolidBrush(Color.Black), startX + 40, startY + offset);

            //graphics.DrawString("Date " + ":" + dr["In_DateTime"].ToString(), fontSize7,
            //         new SolidBrush(Color.Black), startX + 140, startY + offset);
            graphics.DrawString("", fontSize7,
                         new SolidBrush(Color.Black), startX + 95, startY + offset);
            offset = offset + 12;

            graphics.DrawString("Class : " + lblClass.Text, fontSize7,
                new SolidBrush(Color.Black), startX, startY + offset);

            graphics.DrawString("", fontSize7,
                    new SolidBrush(Color.Black), startX + 40, startY + offset);

            //graphics.DrawString("Date " + ":" + dr["In_DateTime"].ToString(), fontSize7,
            //         new SolidBrush(Color.Black), startX + 140, startY + offset);
            graphics.DrawString("", fontSize7,
                         new SolidBrush(Color.Black), startX + 95, startY + offset);
            offset = offset + 12;

            graphics.DrawString("Roll No. : " + lblRollName.Text, fontSize7,
                   new SolidBrush(Color.Black), startX, startY + offset);

            graphics.DrawString("", fontSize7,
                    new SolidBrush(Color.Black), startX + 40, startY + offset);

            //graphics.DrawString("Date " + ":" + dr["In_DateTime"].ToString(), fontSize7,
            //         new SolidBrush(Color.Black), startX + 140, startY + offset);
            graphics.DrawString("Section" + " : " + lblSection.Text, fontSize7,
                         new SolidBrush(Color.Black), startX + 95, startY + offset);
            offset = offset + 12;

            Pen pen = new Pen(Color.Black, Convert.ToSingle(0.5));
            pen.DashPattern = new float[] { 2f, 1f };

           // offset = offset + 12;

            graphics.DrawLine(pen, startX, startY + offset, startX + 230, startY + offset);

            // offset = offset + 5;
            graphics.DrawString("Fee. Particulars", fontSize7,
                     new SolidBrush(Color.Black), startX, startY + offset);
            

            graphics.DrawString("Paid Amount".PadLeft(11), fontSize7,
                     new SolidBrush(Color.Black), startX + 160, startY + offset);

            offset = offset + 12;
            graphics.DrawLine(pen, startX, startY + offset, startX + 230, startY + offset);
            var format = new StringFormat() { Alignment = StringAlignment.Far };
            foreach (DataRow dr1 in dtPay.Rows)
            {
                if (dr1["pay_head_id"].ToString() != "")
                {
                    if (Convert.ToDouble(dr1["pay_amt"]) > 0)
                    {
                        graphics.DrawString(dr1["pay_head_id"].ToString(), fontSize7,
                         new SolidBrush(Color.Black), startX, startY + offset);


                        graphics.DrawString(dr1["pay_amt"].ToString(), fontSize7,
                                 new SolidBrush(Color.Black), startX + 222, startY + offset, format);

                        offset = offset + 12;
                    }
                    tot += decimal.Parse(dr1["Fix_Amt"].ToString().Replace(",", ""));
                    tot3 += decimal.Parse(dr1["Previous_Pay"].ToString().Replace(",", ""));
                    tot1 += decimal.Parse(dr1["Discount_AMT"].ToString().Replace(",", ""));
                    tot2 += decimal.Parse(dr1["pay_amt"].ToString().Replace(",", ""));
                }
            }
            
            Pen pen5 = new Pen(Color.Black, Convert.ToSingle(0.5));
            pen5.DashPattern = new float[] { 2f, 1f };

            graphics.DrawLine(pen, startX, startY + offset, startX + 230, startY + offset);

            // offset = offset + 5;
           
            graphics.DrawString("Total", fontSize7,
                     new SolidBrush(Color.Black), startX, startY + offset);        

            graphics.DrawString(tot2.ToString("N2").PadLeft(11), fontSize7,
                     new SolidBrush(Color.Black), startX + 153, startY + offset);

            offset = offset + 12;
            graphics.DrawLine(pen, startX, startY + offset, startX + 230, startY + offset);

            offset = offset + 12;


            Pen pen6 = new Pen(Color.Black, Convert.ToSingle(0.5));
            pen6.DashPattern = new float[] { 2f, 1f };

            graphics.DrawLine(new Pen(Color.Black, Convert.ToSingle(0.5)), startX, startY + offset, startX + 205, startY + offset);  

            // offset = offset + 5;
            //startX =+15;
            graphics.DrawString("Payment Status",new System.Drawing.Font("Courier New", 7, FontStyle.Bold),
                                new SolidBrush(Color.Black), startX, startY + offset);

            graphics.DrawString("", fontSize7,
                     new SolidBrush(Color.Black), startX + 75, startY + offset);       

            offset = offset + 12;
            graphics.DrawLine(new Pen(Color.Black, Convert.ToSingle(0.5)), startX, startY + offset, startX + 205, startY + offset);


            Pen pen10 = new Pen(Color.Black, Convert.ToSingle(0.5));
            pen10.DashPattern = new float[] { 2f, 1f };          

            // offset = offset + 5;
            graphics.DrawString("Total Amount to Pay :", fontSize7,
                     new SolidBrush(Color.Black), startX, startY + offset);

            graphics.DrawString(tot.ToString("N2"), fontSize7,
                     new SolidBrush(Color.Black), startX + 200, startY + offset, format);           

            offset = offset + 12;

            graphics.DrawLine(pen, startX, startY + offset, startX + 205, startY + offset);

            graphics.DrawString("Now Paid Amount     :", fontSize7,
                    new SolidBrush(Color.Black), startX, startY + offset);

            graphics.DrawString(tot2.ToString("N2").PadLeft(11), fontSize7,
                     new SolidBrush(Color.Black), startX + 200, startY + offset, format);

            offset = offset + 12;

            graphics.DrawLine(pen, startX, startY + offset, startX + 205, startY + offset);

            graphics.DrawString("Previous Amount     :", fontSize7,
                    new SolidBrush(Color.Black), startX, startY + offset);

            graphics.DrawString(tot3.ToString("N2").PadLeft(11), fontSize7,
                     new SolidBrush(Color.Black), startX + 200, startY + offset, format);

            offset = offset + 12;

            graphics.DrawLine(pen, startX, startY + offset, startX + 205, startY + offset);

            graphics.DrawString("Discount Amount     :", fontSize7,
                   new SolidBrush(Color.Black), startX, startY + offset);

            graphics.DrawString(tot1.ToString("N2").PadLeft(11), fontSize7,
                     new SolidBrush(Color.Black), startX + 200, startY + offset, format);

            offset = offset + 12;

            graphics.DrawLine(pen, startX, startY + offset, startX + 205, startY + offset);

            graphics.DrawString("Total Paid Amount   :", fontSize7,
                 new SolidBrush(Color.Black), startX, startY + offset);

            graphics.DrawString(PaidAmount.PadLeft(11), fontSize7,
                     new SolidBrush(Color.Black), startX + 200, startY + offset, format);

            offset = offset + 12;

            graphics.DrawLine(pen, startX, startY + offset, startX + 205, startY + offset);

            graphics.DrawString("Total Due           :", fontSize7,
                 new SolidBrush(Color.Black), startX, startY + offset);

            graphics.DrawString(DueAmount.PadLeft(11), fontSize7,
                     new SolidBrush(Color.Black), startX + 200, startY + offset,format);

            offset = offset + 12;

            graphics.DrawLine(new Pen(Color.Black, Convert.ToSingle(0.5)), startX, startY + offset, startX + 205, startY + offset); 
           // graphics.DrawLine(new Pen(Color.Black, Convert.ToSingle(0.5)), startX, startY + offset, startX + 257, startY + offset);          

            offset = offset + 25;
            graphics.DrawString("Keep rocking", fontSize7Bold,
                     new SolidBrush(Color.Black), startX, startY + offset);
        }
    }
    protected void txtSearchStudent_TextChanged(object sender, EventArgs e)
    {
        DataTable dt = clsPaymentManager.getStudentInfoSerachOption(txtSearchStudent.Text);
        if (dt.Rows.Count > 0)
        {
            lblName.Text = ": " + ((DataRow)dt.Rows[0])["name"].ToString();
            lblClassId.Text = ((DataRow)dt.Rows[0])["class_id"].ToString();
            lblSectionId.Text = ((DataRow)dt.Rows[0])["sect"].ToString();
            lblVersonId.Text = ((DataRow)dt.Rows[0])["version"].ToString();
            lblShiftId.Text = ((DataRow)dt.Rows[0])["shift"].ToString();
            lblRoll.Text = ((DataRow)dt.Rows[0])["std_roll"].ToString();

            lblClass.Text = ": " + ((DataRow)dt.Rows[0])["class_name"].ToString();
            lblSection.Text = ": " + ((DataRow)dt.Rows[0])["sec_name"].ToString();
            lblRollName.Text = ": " + ((DataRow)dt.Rows[0])["std_roll"].ToString();
            lblShift.Text = ": " + ((DataRow)dt.Rows[0])["shift_name"].ToString();
            lblShiftNew.Text = ((DataRow)dt.Rows[0])["shift_name"].ToString();
            //lblVersion.Text = ": " + ((DataRow)dt.Rows[0])["version_name"].ToString();
            lblRollName.Text = ": " + ((DataRow)dt.Rows[0])["std_roll"].ToString();
            lblYear.Text = ((DataRow)dt.Rows[0])["class_year"].ToString();
            lblYear0.Text = ": " + ((DataRow)dt.Rows[0])["class_year"].ToString();
            //string waiver = clsStdWaiverManager.getStudentWaiverPct(txtStudentId.Text, ((DataRow)dt.Rows[0])["class_year"].ToString(), ((DataRow)dt.Rows[0])["class_id"].ToString());
            //txtWaiverPct.Text = waiver;
            txtChequeNo.Focus();
            Payment("");
            txtSearchStudent.Text = "";

            UpdatePanelDetails.Update();
            ddlPayMode.Focus();
            UpdatePanelMST.Update();
        }
    }
    protected void ddlPayMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlPayMode.SelectedValue == "C")
        {
            lblCheckNo.Visible = txtChequeNo.Visible = lblChkPoint.Visible = lblCheckDate.Visible = txtChequeDate.Visible = false;
            lblBankName.Visible = ddlBankNo.Visible = lblBankPoint.Visible = lblChkAmount.Visible = txtChequeAmt.Visible = false;
            UpdatePanelMST.Update();
        }
        else if (ddlPayMode.SelectedValue == "Q")
        {
            lblCheckNo.Visible = txtChequeNo.Visible = lblChkPoint.Visible = lblCheckDate.Visible = txtChequeDate.Visible = true;
            lblBankName.Visible = ddlBankNo.Visible = lblBankPoint.Visible = lblChkAmount.Visible = txtChequeAmt.Visible = true;
            Session["BankID"] = ddlBankNo.SelectedValue;
            Session["PayType"] = ddlPayMode.SelectedValue;
            GetTotal();
            UpdatePanelMST.Update();
        }
    }

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        Session["BankID"] = null;
        Session["PayType"] = null;
        ddlPayMode.SelectedValue = "C";
        ddlPayMode_SelectedIndexChanged(sender, e);
    }
    protected void ddlBankNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["BankID"] = ddlBankNo.SelectedValue;
    }
}
