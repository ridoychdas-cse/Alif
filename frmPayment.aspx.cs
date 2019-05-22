using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using KHSC;
using System.Data.SqlClient;
using iTextSharp.text;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;

public partial class frmPayment : System.Web.UI.Page
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
                            wnot =  dReader["description"].ToString();
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
    protected void txtSearchStudent_TextChanged(object sender, EventArgs e)
    {
        DataTable dt = StudentManager.GetStudentAllCurrentStatus(txtSearchStudent.Text.Trim(),"","","");
        if (dt.Rows.Count > 0)
        {
            lblName.Text = dt.Rows[0]["f_name"].ToString();
            lblBatch.Text = dt.Rows[0]["BatchNo"].ToString();
            lblSTDID.Text = dt.Rows[0]["student_id"].ToString();
            RealID.Text = dt.Rows[0]["ID"].ToString();
            lblCourseName.Text = dt.Rows[0]["CourseName"].ToString();
            lblDate.Text = dt.Rows[0]["std_admission_date"].ToString();
            lblYear.Text = dt.Rows[0]["AddmisionYear"].ToString();
            txtStudentId.Text = dt.Rows[0]["student_id"].ToString();
            txtCourseFees.Text = dt.Rows[0]["CourseFee"].ToString();
            txtPrevDiscount.Text = dt.Rows[0]["Discount"].ToString();
            txtPaidAmount.Text = dt.Rows[0]["PaidAmount"].ToString();
            txtTotalPayable.Text = dt.Rows[0]["PayAmount"].ToString();            
            txtPrevWaiver.Text = dt.Rows[0]["Waiver"].ToString();
            txtPrevDueAmount.Text = dt.Rows[0]["DueAmount"].ToString();
        }
        UpdatePanelDetails.Update();
        UpdatePanelMST.Update();
    }
    protected void txtStudentId_TextChanged(object sender, EventArgs e)
    {
        DataTable dt = StudentManager.GetStudentAllCurrentStatusss(txtStudentId.Text.Trim());
        if (dt.Rows.Count > 0)
        {
            lblName.Text = dt.Rows[0]["f_name"].ToString();
            lblBatch.Text = dt.Rows[0]["BatchNo"].ToString();
            lblSTDID.Text = dt.Rows[0]["student_id"].ToString();
            RealID.Text = dt.Rows[0]["ID"].ToString();
            lblCourseName.Text = dt.Rows[0]["CourseName"].ToString();
            lblDate.Text = dt.Rows[0]["std_admission_date"].ToString();
            lblYear.Text = dt.Rows[0]["AddmisionYear"].ToString();
            txtStudentId.Text = dt.Rows[0]["student_id"].ToString();
            txtCourseFees.Text = dt.Rows[0]["CourseFee"].ToString();
            txtPrevDiscount.Text = dt.Rows[0]["Discount"].ToString();
            txtPaidAmount.Text = dt.Rows[0]["PaidAmount"].ToString();
            txtTotalPayable.Text = dt.Rows[0]["PayAmount"].ToString();
            txtPrevWaiver.Text = dt.Rows[0]["Waiver"].ToString();
            txtPrevDueAmount.Text = dt.Rows[0]["DueAmount"].ToString();

        }
        UpdatePanelDetails.Update();
        UpdatePanelMST.Update();
    }
    private void Clear()
    {
        RealID.Text = "";
        TotalDiscount.Text = "";
        lblTotalPaid.Text = "";
    }
    private void Summation()
    {
        if (string.IsNullOrEmpty(txtCourseFees.Text))
        { txtCourseFees.Text = "0"; }
        if (string.IsNullOrEmpty(txtDiscount.Text))
        { txtDiscount.Text = "0"; }
        if (string.IsNullOrEmpty(txtPrevWaiver.Text))
        { txtPrevWaiver.Text = "0"; }
        if (string.IsNullOrEmpty(txtPaidAmount.Text))
        { txtPaidAmount.Text = "0"; }
        if (string.IsNullOrEmpty(txtPayment.Text))
        { txtPayment.Text = "0"; }
        if (string.IsNullOrEmpty(txtPrevDiscount.Text))
        { txtPrevDiscount.Text = "0"; }
        if (string.IsNullOrEmpty(txtPrevDueAmount.Text))
        { txtPrevDueAmount.Text = "0"; }
        decimal TotaDis = (Convert.ToDecimal(txtPrevDiscount.Text) + Convert.ToDecimal(txtDiscount.Text) + Convert.ToDecimal(txtPrevWaiver.Text) + Convert.ToDecimal(txtPaidAmount.Text) + Convert.ToDecimal(txtPayment.Text));
        decimal TotalPAid = Convert.ToDecimal(txtCourseFees.Text)-TotaDis;   
        txtCurrDue.Text = TotalPAid.ToString("N2");
        TotalDiscount.Text = (Convert.ToDecimal(txtPrevDiscount.Text) + Convert.ToDecimal(txtDiscount.Text)).ToString("N2");
        lblTotalPaid.Text = (Convert.ToDecimal(txtPayment.Text) + Convert.ToDecimal(txtPaidAmount.Text)).ToString("N2");
        
        //lblDueAmount.Text = (Convert.ToDecimal(txtPayment.Text) + Convert.ToDecimal(txtPaidAmount.Text)).ToString("N2");
    }
    protected void txtPayment_TextChanged(object sender, EventArgs e)
    {
        Summation();
        UpdatePanelDetails.Update();
        UpdatePanelMST.Update();
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {

    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try{
            if (txtChequeDate.Text == "")
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Please input date...!!');", true);
                return;
            }
            else
            {
                clsStdCurrentStatus st = new clsStdCurrentStatus();
                st.StudentId = RealID.Text;
                st.PaidAmount = lblTotalPaid.Text;
                st.TotalDiscount = TotalDiscount.Text;
                st.CurrentDue = txtCurrDue.Text;
                st.DateUpdate = txtChequeDate.Text;
                if (per.AllowAdd == "Y")
                {
                    clsStdCurrentStatusManager.CreateCurrentStatusUpdate(st);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ale", "alert('Record(s) is/are created suceessfullly...!!');", true);
                    //Clear();
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
    protected void btnFind_Click(object sender, EventArgs e)
    {

    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/frmPayment.aspx?mno=0.0");
    }
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        PrintMoneyRecieptNew();
    }
    protected void txtDiscount_TextChanged(object sender, EventArgs e)
    {
        Summation();
        UpdatePanelDetails.Update();
        UpdatePanelMST.Update();
    }
    private void PrintMoneyRecieptNew()
    {
        try
        {

            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment; filename='StudentPaymentReport'.pdf");
            Student std = StudentManager.getStd(lblSTDID.Text);
            //clsPaymentMst paymst = clsPaymentManager.getPaymentMst(txtPaymentId.Text);
           
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
            string a = lblSTDID.Text.Trim();    
           
                if (txtStudentId.Text != "")
                {
                    byte[] logo = GlBookManager.GetGlLogo(Session["book"].ToString());
                    iTextSharp.text.Image gif = iTextSharp.text.Image.GetInstance(logo);
                    gif.Alignment = iTextSharp.text.Image.MIDDLE_ALIGN;
                    gif.ScalePercent(25f);
                    float[] titwidth = new float[2] { 5, 200 };
                    float[] swidth1 = new float[2] { 130, 70 };
                    float[] twidth = new float[3] { 100, 5, 100 };
                    float[] widFooter = new float[2] { 55, 45 };
                  
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

                            float[] width = new float[7] { 20, 5, 25, 10, 20, 5, 35 };
                            PdfPTable dtm = new PdfPTable(width);
                            dtm.WidthPercentage = 100;

                              DataTable dt = StudentManager.GetStudentAllCurrentStatusss(txtStudentId.Text.Trim());
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
                            cell = new PdfPCell(new Phrase(dt.Rows[0]["ID"].ToString(), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
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
                            cell = new PdfPCell(new Phrase(txtChequeDate.Text, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
                            cell.HorizontalAlignment = 3;
                            cell.VerticalAlignment = 1;
                            cell.BorderWidth = 0f;
                            //cell.FixedHeight = 15f;
                            dtm.AddCell(cell);
                            cell = new PdfPCell(new Phrase("Student ID", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
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
                            cell = new PdfPCell(new Phrase(txtStudentId.Text, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 7, iTextSharp.text.Font.NORMAL)));
                            cell.HorizontalAlignment = 0;
                            cell.VerticalAlignment = 1;
                            cell.BorderWidth = 0f;
                            dtm.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
                            cell.HorizontalAlignment = 3;
                            cell.VerticalAlignment = 1;
                            cell.BorderWidth = 0f;
                            //cell.FixedHeight = 15f;
                            dtm.AddCell(cell);
                            cell = new PdfPCell(new Phrase("Student Name", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
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
                            cell = new PdfPCell(new Phrase(lblName.Text, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 7, iTextSharp.text.Font.NORMAL)));
                            cell.HorizontalAlignment = 0;
                            cell.VerticalAlignment = 1;
                            cell.BorderWidth = 0f;
                            dtm.AddCell(cell);
                            cell = new PdfPCell(new Phrase("Course Name", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
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
                            cell = new PdfPCell(new Phrase(lblCourseName.Text, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
                            cell.HorizontalAlignment = 3;
                            cell.VerticalAlignment = 1;
                            cell.BorderWidth = 0f;
                            cell.Colspan = 5;
                            cell.FixedHeight = 15f;
                            dtm.AddCell(cell);
                
                            cell = new PdfPCell(new Phrase("Batch No", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
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
                            cell = new PdfPCell(new Phrase(lblBatch.Text, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
                            cell.HorizontalAlignment = 3;
                            cell.VerticalAlignment = 1;
                            cell.BorderWidth = 0f;
                            cell.FixedHeight = 15f;
                            dtm.AddCell(cell);
                            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
                            cell.HorizontalAlignment = 3;
                            cell.VerticalAlignment = 1;
                            cell.BorderWidth = 0f;
                            //cell.FixedHeight = 15f;
                            dtm.AddCell(cell);
                            cell = new PdfPCell(new Phrase("Year", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
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
                            cell = new PdfPCell(new Phrase(lblYear.Text, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
                            cell.HorizontalAlignment = 3;
                            cell.VerticalAlignment = 1;
                            cell.BorderWidth = 0f;
                            cell.FixedHeight = 15f;
                            dtm.AddCell(cell);    

                            dtm.AddCell(cell);
                            document.Add(dtm);
                            document.Add(dtempty);

                            float[] swidth = new float[2] { 60, 20 };
                            PdfPTable pdtPay = new PdfPTable(swidth);
                            pdtPay.WidthPercentage = 100;

                            cell = new PdfPCell(new Phrase("Fee. Particulars", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.BOLD)));
                            cell.HorizontalAlignment = 3;
                            cell.VerticalAlignment = 1;
                            //cell.BorderColor = BaseColor.LIGHT_GRAY;
                            pdtPay.AddCell(cell);
                          
                            cell = new PdfPCell(new Phrase("Paid Amount", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.BOLD)));
                            cell.HorizontalAlignment = 2;
                            cell.VerticalAlignment = 1;
                            //cell.BorderColor = BaseColor.LIGHT_GRAY;
                            pdtPay.AddCell(cell);                         
                            if (i == 2)
                            {
                                string PaidAmount = lblTotalPaid.Text;
                                string DueAmount = txtCurrDue.Text;

                                cell = new PdfPCell(new Phrase("Total Amount", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.BOLD)));
                                cell.HorizontalAlignment = 2;
                                cell.VerticalAlignment = 1;
                                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                pdtPay.AddCell(cell);

                                cell = new PdfPCell(new Phrase(txtTotalPayable.Text, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.BOLD)));
                                cell.HorizontalAlignment = 2;
                                cell.VerticalAlignment = 1;
                                cell.Colspan = 2;
                                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                pdtPay.AddCell(cell);
                                

                                cell = new PdfPCell(new Phrase("Discount Amount", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.BOLD)));
                                cell.HorizontalAlignment = 2;
                                cell.VerticalAlignment = 1;
                                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                pdtPay.AddCell(cell);

                                cell = new PdfPCell(new Phrase(TotalDiscount.Text, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.BOLD)));
                                cell.HorizontalAlignment = 2;
                                cell.VerticalAlignment = 1;
                                cell.Colspan = 2;
                                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                pdtPay.AddCell(cell);

                                cell = new PdfPCell(new Phrase("Waiver Amount", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.BOLD)));
                                cell.HorizontalAlignment = 2;
                                cell.VerticalAlignment = 1;
                                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                pdtPay.AddCell(cell);

                                cell = new PdfPCell(new Phrase(txtPrevWaiver.Text, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.BOLD)));
                                cell.HorizontalAlignment = 2;
                                cell.VerticalAlignment = 1;
                                cell.Colspan = 2;
                                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                pdtPay.AddCell(cell);

                                cell = new PdfPCell(new Phrase("Previous Paid", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.BOLD)));
                                cell.HorizontalAlignment = 2;
                                cell.VerticalAlignment = 1;
                                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                pdtPay.AddCell(cell);

                                cell = new PdfPCell(new Phrase(txtPaidAmount.Text, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.BOLD)));
                                cell.HorizontalAlignment = 2;
                                cell.VerticalAlignment = 1;
                                cell.Colspan = 2;
                                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                pdtPay.AddCell(cell);

                                cell = new PdfPCell(new Phrase("Current Paid", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.BOLD)));
                                cell.HorizontalAlignment = 2;
                                cell.VerticalAlignment = 1;
                                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                pdtPay.AddCell(cell);

                                cell = new PdfPCell(new Phrase(txtPayment.Text, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.BOLD)));
                                cell.HorizontalAlignment = 2;
                                cell.VerticalAlignment = 1;
                                cell.Colspan = 2;
                                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                pdtPay.AddCell(cell);

                                cell = new PdfPCell(new Phrase("Current Due", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.BOLD)));
                                cell.HorizontalAlignment = 2;
                                cell.VerticalAlignment = 1;
                                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                pdtPay.AddCell(cell);

                                cell = new PdfPCell(new Phrase(txtCurrDue.Text, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.BOLD)));
                                cell.HorizontalAlignment = 2;
                                cell.VerticalAlignment = 1;
                                cell.Colspan = 2;
                                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                pdtPay.AddCell(cell);

                                cell = new PdfPCell(new Phrase(lblTotalPaid.Text, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.BOLD)));
                                cell.HorizontalAlignment = 2;
                                cell.VerticalAlignment = 1;
                                cell.Colspan = 2;
                                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                pdtPay.AddCell(cell);
                                //cell = new PdfPCell(new Phrase("In word: " +lblTotalPaid.Text, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)));
                                decimal tott2 = Convert.ToDecimal(lblTotalPaid.Text);
                                cell = new PdfPCell(new Phrase("In word: " + DataManager.GetLiteralAmt(tott2.ToString()).Replace("  ", " "), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)));
                                cell.HorizontalAlignment = 3;
                                cell.VerticalAlignment = 2;
                                cell.Border = 0;
                                cell.Colspan = 2;
                                pdtPay.AddCell(cell);                              

                                document.Add(pdtPay);
                                document.Add(dtempty);                       

                                PdfPTable dtempty1 = new PdfPTable(1);
                                dtempty1.WidthPercentage = 100;
                                cell = new PdfPCell(FormatPhrase(""));
                                cell.VerticalAlignment = 0;
                                cell.HorizontalAlignment = 0;
                                cell.BorderWidth = 0f;
                                cell.FixedHeight = 30f;
                                dtempty1.AddCell(cell);
                                document.Add(dtempty1);

                                float[] widthsig = new float[] { 20, 20 };
                                PdfPTable pdtsig = new PdfPTable(widthsig);
                                pdtsig.WidthPercentage = 100;
                                //cell = new PdfPCell(FormatPhrase("Student/Gurdian Signature"));
                                //cell.HorizontalAlignment = 1;
                                //cell.VerticalAlignment = 1;
                                //cell.Border = 1;
                                ////cell.FixedHeight = 18f;
                                ////cell.BorderColor = BaseColor.LIGHT_GRAY;
                                //pdtsig.AddCell(cell);
                                //cell = new PdfPCell(FormatPhrase(""));
                                //cell.HorizontalAlignment = 0;
                                //cell.VerticalAlignment = 1;
                                //cell.Border = 0;
                                ////cell.FixedHeight = 18f;
                                ////cell.BorderColor = BaseColor.LIGHT_GRAY;
                                //pdtsig.AddCell(cell);
                                //cell = new PdfPCell(FormatPhrase("HeadMaster Signature"));
                                //cell.HorizontalAlignment = 1;
                                //cell.VerticalAlignment = 1;
                                //cell.Border = 1;
                                //cell.FixedHeight = 18f;
                                ////cell.BorderColor = BaseColor.LIGHT_GRAY;
                                //pdtsig.AddCell(cell);
                                cell = new PdfPCell(FormatPhrase(""));
                                cell.HorizontalAlignment = 0;
                                cell.VerticalAlignment = 1;
                                cell.Border = 0;
                                //cell.FixedHeight = 18f;
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
                                string PaidAmount = lblTotalPaid.Text;
                                string DueAmount = txtCurrDue.Text;

                                cell = new PdfPCell(new Phrase("Total Amount", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.BOLD)));
                                cell.HorizontalAlignment = 2;
                                cell.VerticalAlignment = 1;
                                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                pdtPay.AddCell(cell);

                                cell = new PdfPCell(new Phrase(txtTotalPayable.Text, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.BOLD)));
                                cell.HorizontalAlignment = 2;
                                cell.VerticalAlignment = 1;
                                cell.Colspan = 2;
                                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                pdtPay.AddCell(cell);


                                cell = new PdfPCell(new Phrase("Discount Amount", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.BOLD)));
                                cell.HorizontalAlignment = 2;
                                cell.VerticalAlignment = 1;
                                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                pdtPay.AddCell(cell);

                                cell = new PdfPCell(new Phrase(TotalDiscount.Text, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.BOLD)));
                                cell.HorizontalAlignment = 2;
                                cell.VerticalAlignment = 1;
                                cell.Colspan = 2;
                                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                pdtPay.AddCell(cell);

                                cell = new PdfPCell(new Phrase("Waiver Amount", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.BOLD)));
                                cell.HorizontalAlignment = 2;
                                cell.VerticalAlignment = 1;
                                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                pdtPay.AddCell(cell);

                                cell = new PdfPCell(new Phrase(txtPrevWaiver.Text, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.BOLD)));
                                cell.HorizontalAlignment = 2;
                                cell.VerticalAlignment = 1;
                                cell.Colspan = 2;
                                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                pdtPay.AddCell(cell);

                                cell = new PdfPCell(new Phrase("Previous Paid", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.BOLD)));
                                cell.HorizontalAlignment = 2;
                                cell.VerticalAlignment = 1;
                                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                pdtPay.AddCell(cell);

                                cell = new PdfPCell(new Phrase(txtPaidAmount.Text, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.BOLD)));
                                cell.HorizontalAlignment = 2;
                                cell.VerticalAlignment = 1;
                                cell.Colspan = 2;
                                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                pdtPay.AddCell(cell);

                                cell = new PdfPCell(new Phrase("Current Paid", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.BOLD)));
                                cell.HorizontalAlignment = 2;
                                cell.VerticalAlignment = 1;
                                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                pdtPay.AddCell(cell);

                                cell = new PdfPCell(new Phrase(txtPayment.Text, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.BOLD)));
                                cell.HorizontalAlignment = 2;
                                cell.VerticalAlignment = 1;
                                cell.Colspan = 2;
                                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                pdtPay.AddCell(cell);

                                cell = new PdfPCell(new Phrase("Current Due", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.BOLD)));
                                cell.HorizontalAlignment = 2;
                                cell.VerticalAlignment = 1;
                                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                pdtPay.AddCell(cell);

                                cell = new PdfPCell(new Phrase(txtCurrDue.Text, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.BOLD)));
                                cell.HorizontalAlignment = 2;
                                cell.VerticalAlignment = 1;
                                cell.Colspan = 2;
                                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                pdtPay.AddCell(cell);

                                cell = new PdfPCell(new Phrase(lblTotalPaid.Text, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.BOLD)));
                                cell.HorizontalAlignment = 2;
                                cell.VerticalAlignment = 1;
                                cell.Colspan = 2;
                                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                                pdtPay.AddCell(cell);
                                //cell = new PdfPCell(new Phrase("In word: " + lblTotalPaid.Text, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)));
                                decimal tott2 = Convert.ToDecimal(lblTotalPaid.Text);
                                cell = new PdfPCell(new Phrase("In word: " + DataManager.GetLiteralAmt(tott2.ToString()).Replace("  ", " "), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)));
                                cell.HorizontalAlignment = 3;
                                cell.VerticalAlignment = 2;
                                cell.Border = 0;
                                cell.Colspan = 2;
                                pdtPay.AddCell(cell);

                                document.Add(pdtPay);
                                document.Add(dtempty);

                                PdfPTable dtempty1 = new PdfPTable(1);
                                dtempty1.WidthPercentage = 100;
                                cell = new PdfPCell(FormatPhrase(""));
                                cell.VerticalAlignment = 0;
                                cell.HorizontalAlignment = 0;
                                cell.BorderWidth = 0f;
                                cell.FixedHeight = 30f;
                                dtempty1.AddCell(cell);
                                document.Add(dtempty1);

                                float[] widthsig = new float[] { 20, 20 };
                                PdfPTable pdtsig = new PdfPTable(widthsig);
                                pdtsig.WidthPercentage = 100;
                                //cell = new PdfPCell(FormatPhrase("Student/Gurdian Signature"));
                                //cell.HorizontalAlignment = 1;
                                //cell.VerticalAlignment = 1;
                                //cell.Border = 1;
                                ////cell.FixedHeight = 18f;
                                ////cell.BorderColor = BaseColor.LIGHT_GRAY;
                                //pdtsig.AddCell(cell);
                                //cell = new PdfPCell(FormatPhrase(""));
                                //cell.HorizontalAlignment = 0;
                                //cell.VerticalAlignment = 1;
                                //cell.Border = 0;
                                ////cell.FixedHeight = 18f;
                                ////cell.BorderColor = BaseColor.LIGHT_GRAY;
                                //pdtsig.AddCell(cell);
                                //cell = new PdfPCell(FormatPhrase("HeadMaster Signature"));
                                //cell.HorizontalAlignment = 1;
                                //cell.VerticalAlignment = 1;
                                //cell.Border = 1;
                                //cell.FixedHeight = 18f;
                                ////cell.BorderColor = BaseColor.LIGHT_GRAY;
                                //pdtsig.AddCell(cell);
                                cell = new PdfPCell(FormatPhrase(""));
                                cell.HorizontalAlignment = 0;
                                cell.VerticalAlignment = 1;
                                cell.Border = 0;
                                //cell.FixedHeight = 18f;
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
                        }

                    }

                    document.Close();
                    Response.Flush();
                    Response.End();

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
    private static Phrase FormatPhrase(string value)
    {
        return new Phrase(value, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10));
    }
    private static Phrase FormatHeaderPhrase(string value)
    {
        return new Phrase(value, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12, iTextSharp.text.Font.BOLD));
    }
}