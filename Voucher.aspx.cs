using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html;
using iTextSharp.text.pdf.draw;
using System.Reflection;
using System.IO;
using KHSC;


public partial class Voucher : System.Web.UI.Page
{
    
    public static Permis per;
    //public static ReportDocument rpt;
    //public static decimal priceDr = 0;
    //public static decimal priceCr = 0;
    private int dtlRecordNum = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["user"] == null)
        {
            if (Session.SessionID != "" | Session.SessionID != null)
            {
                clsSession ses = clsSessionManager.getSession(Session.SessionID);
                if (ses != null)
                {
                    Session["user"] = ses.UserId; Session["book"] = "AMB"; 
                    
                    string connectionString = DataManager.OraConnString();
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        string query = "Select user_grp,description from utl_userinfo where upper(user_name)=upper('" + Session["user"].ToString().ToUpper() + "') and status='A'";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.CommandType = CommandType.Text;
                            conn.Open();
                            using (SqlDataReader dreader = cmd.ExecuteReader())
                            {
                                if (dreader.HasRows == true)
                                {
                                    while (dreader.Read())
                                    {
                                        Session["userlevel"] = int.Parse(dreader["user_grp"].ToString());
                                        Session["wnote"] = "Welcome Mr. " + dreader["description"].ToString();
                                    }
                                }
                            }
                        }
                    }
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        string query = "Select book_desc,company_address1,company_address2,separator_type from gl_set_of_books where book_name='" + Session["book"].ToString() + "' ";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.CommandType = CommandType.Text;
                            conn.Open();
                            using (SqlDataReader dreader = cmd.ExecuteReader())
                            {
                                if (dreader.HasRows == true)
                                {
                                    while (dreader.Read())
                                    {
                                        Session["septype"] = dreader["separator_type"].ToString();
                                        Session["org"] = dreader["book_desc"].ToString();
                                        Session["add1"] = dreader["company_address1"].ToString();
                                        Session["add2"] = dreader["company_address2"].ToString();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        try
        {
            //ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('" + ViewState.ViewStateID + "');", true);
            string pageName = DataManager.GetCurrentPageName();
            string modid = PermisManager.getModuleId(pageName);
            per = PermisManager.getUsrPermis(Session["user"].ToString().Trim().ToUpper(), modid);
            if (per != null && per.AllowView == "Y")
            {
                ((Label)Page.Master.FindControl("lblLogin")).Text = Session["wnote"].ToString();
                ((LinkButton)Page.Master.FindControl("lbLogout")).Visible = true;
                if (int.Parse(Session["userlevel"].ToString()) > 1)
                {
                    lbAuth.Visible = true;
                }
                else
                {
                    lbAuth.Visible = false;
                }
            }
            else
            {
                Response.Redirect("Home.aspx?sid=sam");
            }
        }
        catch
        {
            //ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('"+ex.Message+"!!');", true);
            Response.Redirect("Default.aspx?sid=sam");
        }

        if (!Page.IsPostBack)
        {
            dgVoucher.Visible = true;
            dgVoucherDtl.Visible = true;            
            lbAuth.Visible = false;
            btnCheqPrint.Visible = false;
            btnNew_Click(sender, e);
            LoadGrid();
            BindGrid();
            txtValueDate.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
        }

        txtValueDate.Attributes.Add("onBlur", "formatdate('" + txtValueDate.ClientID + "')");
        txtCheqDate.Attributes.Add("onBlur", "formatdate('" + txtCheqDate.ClientID + "')");
        txtMoneyRptDate.Attributes.Add("onBlur", "formatdate('" + txtMoneyRptDate.ClientID + "')");
        txtControlAmt.Attributes.Add("onBlur", "setDecimal('" + txtControlAmt.ClientID + "')");
        txtCheqAmnt.Attributes.Add("onBlur", "setDecimal('" + txtCheqAmnt.ClientID + "')");
        if (Request.QueryString["vchno"] != null )
        {
            if (Request.QueryString["vchno"].ToString() != "")
            {
                txtVchSysNo.Text = Request.QueryString["vchno"].ToString();
                dgVoucherDtl.Columns[0].Visible = true;
                FindForAuth();
                VEUnauth();
                //dgVoucher.Visible = false;
                ShowFooterTotal();
            }
        }
    }
    
    private void RefreshDropDown()
    {
        //txtVchCode.Items.Clear();
        //util.PopulateCombo(txtVchCode, "gl_voucher_type", "vch_desc", "vch_code");

        ddlFinMon.Items.Clear();
        util.PopulateCombo1(ddlFinMon, "gl_fin_month", "fin_mon", "fin_mon");
    }
    private void ClearField()
    {
        txtVchSysNo.Text = String.Empty;
        RefreshDropDown();
        txtValueDate.Text = String.Empty;
        txtVchRefNo.Text = String.Empty;
        txtRefFileNo.Text = String.Empty;
        txtVolumeNo.Text = String.Empty;
        txtSerialNo.Text = String.Empty;
        ddlTransType.SelectedIndex = -1;
        txtVchCode.SelectedIndex = -1;
        txtParticulars.Text = String.Empty;
        txtPayee.Text = String.Empty;
        txtCheckNo.Text = String.Empty;
        txtCheqDate.Text = String.Empty;
        txtCheqAmnt.Text = String.Empty;
        txtMoneyRptDate.Text = String.Empty;
        txtMoneyRptNo.Text = String.Empty;
        txtControlAmt.Text = String.Empty;
        txtStatus.Text = "U";
        loginId.Text = "";
        pwd.Text = "";
        ddlFinMon.SelectedIndex = -1;
        ddlTransType.SelectedIndex = -1;
        txtVchCode.SelectedIndex = -1;
        lbSetNew.Visible = false;
    }
    private void LoadGrid()
    {
        DataTable dtTable;
        if (txtRefFileNo.Text == String.Empty)
        {

            dtTable = VouchManager.GetVouchMasters(Session["userlevel"].ToString());
        }
        else
        {
            dtTable = VouchManager.GetVouchMstFind(txtVchSysNo.Text, txtRefFileNo.Text, txtVolumeNo.Text);
        }
        dgVoucher.DataSource = dtTable;
        //dgVoucher.DataBind();
    }
    private void BindGrid()
    {
        dgVoucher.DataBind();
    }
    protected void Clear_Click(object sender, EventArgs e)
    {
        ViewState.Remove("vouchdtl");
        Response.Redirect("~/Voucher.aspx?mno=0.0");
    }
    protected void dgVoucher_SelectedIndexChanged(object sender, EventArgs e)
    {
        RefreshDropDown();
        ClearField();
        txtVchSysNo.Text = dgVoucher.SelectedRow.Cells[1].Text.Trim();
        VouchMst vchmst = VouchManager.GetVouchMaster(txtVchSysNo.Text, (Session["userlevel"].ToString()).ToString());
        ddlFinMon.Items.Add(vchmst.FinMon);
        ddlFinMon.SelectedValue = vchmst.FinMon;
        txtValueDate.Text = vchmst.ValueDate;        
        txtVchRefNo.Text = vchmst.VchRefNo;
        txtRefFileNo.Text = vchmst.RefFileNo;
        txtVolumeNo.Text = vchmst.VolumeNo;
        txtSerialNo.Text = vchmst.SerialNo;
        txtVchCode.SelectedValue = vchmst.VchCode;
        ddlTransType.SelectedValue = vchmst.TransType;
        txtParticulars.Text = vchmst.Particulars;
        txtPayee.Text = vchmst.Payee;
        txtCheckNo.Text = vchmst.CheckNo;
        txtCheqDate.Text = vchmst.CheqDate;
        txtCheqAmnt.Text = vchmst.CheqAmnt;        
        txtMoneyRptNo.Text = vchmst.MoneyRptNo;
        txtMoneyRptDate.Text = vchmst.MoneyRptDate;
        txtControlAmt.Text = decimal.Parse(vchmst.ControlAmt).ToString("N2");
        txtStatus.Text = vchmst.Status;
       
        //dgVoucher.Visible = false;
        //dgVoucherDtl.Visible = true;
        ShowChild_Click(sender, e);
        if (!string.IsNullOrEmpty(vchmst.AuthoUserType))
        {
            if ((vchmst.Status == "A") && (int.Parse(vchmst.AuthoUserType) >= int.Parse(Session["userlevel"].ToString())))
            {
                lbAuth.Visible = false;
                VEAuth();
            }
            else
            {
                lbAuth.Visible = true;
                VEUnauth();
                lbSetNew.Visible = true;
            }
        }
        else
        {
            lbAuth.Visible = true;
            VEUnauth();
            lbSetNew.Visible = true;
        }

        //if (vchmst.Status == "U")
        //{
        //    lbAuth.Visible = true;
        //    VEUnauth();
        //    lbSetNew.Visible = true;
        //}
        //else
        //{
        //    lbAuth.Visible = false;
        //    VEAuth();
        //}
        ShowFooterTotal();
        btnPrint.Visible = true;
        detailsupdatepanal.Update();
    }
    protected void lbSetNew_Click(object sender, EventArgs e)
    {
        if (txtVchSysNo.Text != String.Empty)
        {
            VouchMst vchmst = VouchManager.GetVouchMst(txtVchSysNo.Text);
            if (vchmst != null)
            {
                ClearField();
                ddlFinMon.SelectedIndex = -1;
                txtRefFileNo.Text = vchmst.RefFileNo;
                txtVolumeNo.Text = vchmst.VolumeNo;
                txtSerialNo.Text = vchmst.SerialNo;
                txtVchCode.SelectedValue = vchmst.VchCode.ToString();
                ddlTransType.SelectedValue = vchmst.TransType;
                txtParticulars.Text = vchmst.Particulars;
                txtPayee.Text = vchmst.Payee;
                txtCheqAmnt.Text = "";
                txtMoneyRptNo.Text = vchmst.MoneyRptNo;
                txtControlAmt.Text = "0";
                txtStatus.Text = "U";
                DataTable dt = new DataTable();
                if (dt.Columns.Count == 0)
                {
                    dt.Columns.Add("line_no", typeof(string));
                    dt.Columns.Add("gl_coa_code", typeof(string));
                    dt.Columns.Add("particulars", typeof(string));
                    dt.Columns.Add("amount_dr", typeof(string));
                    dt.Columns.Add("amount_cr", typeof(string));
                }
                DataRow dr;
                foreach (GridViewRow gvr in dgVoucherDtl.Rows)
                {
                    /*
                    if (gvr.RowType == DataControlRowType.DataRow)
                    {
                        dr = dt.NewRow();
                        if ((gvr.RowState & DataControlRowState.Edit) > 0)
                        {
                            dr["line_no"] = ((TextBox)gvr.FindControl("txtLineNo")).Text.ToString();
                            dr["gl_coa_code"] = ((TextBox)gvr.FindControl("txtGlCoaCode")).Text.ToString();
                            dr["particulars"] = ((TextBox)gvr.FindControl("txtCoaDesc")).Text.ToString();
                        }
                        else
                        {
                            dr["line_no"] = ((Label)gvr.FindControl("lblLineNo")).Text.ToString();
                            dr["gl_coa_code"] = ((Label)gvr.FindControl("lblGlCoaCode")).Text;
                            dr["particulars"] = ((Label)gvr.FindControl("lblCoaDesc")).Text;
                        }
                        dr["amount_dr"] = "0";
                        dr["amount_cr"] = "0";
                        dt.Rows.Add(dr);
                    }
                    */
                    dr = dt.NewRow();
                    dr["line_no"] = ((TextBox)gvr.FindControl("txtLineNo")).Text.ToString();
                    dr["gl_coa_code"] = ((TextBox)gvr.FindControl("txtGlCoaCode")).Text.ToString();
                    dr["particulars"] = ((TextBox)gvr.FindControl("txtCoaDesc")).Text.ToString();
                    dr["amount_dr"] = "0";
                    dr["amount_cr"] = "0";
                    dt.Rows.Add(dr);
                }
                dgVoucherDtl.DataSource = dt;
                dgVoucherDtl.DataBind();
                ShowFooterTotal();
                txtValueDate.Focus();
                dgVoucherDtl.Columns[0].Visible = true;
                ViewState["vouchdtl"] = dt;
            }
        }
    }
    protected void Find_Click(object sender, EventArgs e)
    {
        if (txtStatus.Text == "A")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Clear the form first and try again!!');", true);
            ShowFooterTotal();
            return;
        }
        RefreshDropDown();
        DataTable vchM = new DataTable();
        if (txtVchSysNo.Text != "" | txtRefFileNo.Text != "" | txtVolumeNo.Text != "")
        {
            vchM = VouchManager.GetVouchMstFind(txtVchSysNo.Text, txtRefFileNo.Text, txtVolumeNo.Text);
        }
        else if (txtVchSysNo.Text == "" && txtRefFileNo.Text == "" && txtVolumeNo.Text == "" && txtValueDate.Text != "")
        {
            vchM = VouchManager.GetVouchMstByDate(txtValueDate.Text,"DV");
        }
        if (vchM.Rows.Count == 1)
        {
            VouchMst vchmst = VouchManager.GetVouchMaster(vchM.Rows[0]["vch_sys_no"].ToString(), Session["userlevel"].ToString());
            if (vchmst != null)
            {
                txtVchSysNo.Text = vchmst.VchSysNo.ToString();
                ddlFinMon.Items.Add(vchmst.FinMon);
                ddlFinMon.SelectedValue = vchmst.FinMon;
                txtValueDate.Text = vchmst.ValueDate;
                txtVchRefNo.Text = vchmst.VchRefNo;
                txtRefFileNo.Text = vchmst.RefFileNo;
                txtVolumeNo.Text = vchmst.VolumeNo;
                txtSerialNo.Text = vchmst.SerialNo;
                txtVchCode.SelectedValue = vchmst.VchCode;
                ddlTransType.SelectedValue = vchmst.TransType;
                txtParticulars.Text = vchmst.Particulars;
                txtPayee.Text = vchmst.Payee;
                txtCheckNo.Text = vchmst.CheckNo;
                txtCheqDate.Text = vchmst.CheqDate;
                txtCheqAmnt.Text = vchmst.CheqAmnt;
                txtMoneyRptNo.Text = vchmst.MoneyRptNo;
                txtMoneyRptDate.Text = vchmst.MoneyRptDate;
                txtControlAmt.Text = decimal.Parse(vchmst.ControlAmt).ToString("N2");
                txtStatus.Text = vchmst.Status;
                dgVoucherDtl.Visible = true;
                dgVoucher.Visible = false;
                ShowChild_Click(sender, e);

                /* if the current user's rating is equal or higher than actual rating of voucher authorization, then the voucher will be disabled for edit */
                if (vchmst.AuthoUserType != "")
                {
                    if ((vchmst.Status == "A") && (int.Parse(vchmst.AuthoUserType) >= int.Parse(Session["userlevel"].ToString())))
                    {
                        lbAuth.Visible = false;
                        VEAuth();
                    }
                }
                else
                {
                    lbAuth.Visible = true;
                    VEUnauth();
                    lbSetNew.Visible = true;
                }
                //if (vchmst.Status == "U")
                //{
                //    lbAuth.Visible = true;
                //    lbSetNew.Visible = true;
                //    VEUnauth();
                //}
                //else
                //{
                //    lbAuth.Visible = false;
                //    VEAuth();
                //}
                ShowFooterTotal();
                lblTranStatus.Visible = false;
            }            
        }
        else if (vchM.Rows.Count > 1)
        {
            dgVoucher.Visible = true;
            dgVoucherDtl.Visible = false;
            dgVoucher.DataBind();
            dgVoucher.DataSource = vchM;
            dgVoucher.DataBind();
        }
        else if (vchM.Rows.Count == 0)
        {
            lbSetNew.Visible = false;
            ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('No Such Voucher Exist!!');", true);
        }
    }

    public void FindForAuth()
    {        
        RefreshDropDown();        
        VouchMst vchmst = VouchManager.GetVouchMst(txtVchSysNo.Text.Trim());
        if (vchmst != null)
        {
            ddlFinMon.Items.Add(vchmst.FinMon);
            ddlFinMon.SelectedValue = vchmst.FinMon;
            txtValueDate.Text = vchmst.ValueDate;
            txtVchRefNo.Text = vchmst.VchRefNo;
            txtRefFileNo.Text = vchmst.RefFileNo;
            txtVolumeNo.Text = vchmst.VolumeNo;
            txtSerialNo.Text = vchmst.SerialNo;
            txtVchCode.SelectedValue = vchmst.VchCode.Trim();
            ddlTransType.SelectedValue = vchmst.TransType.ToString();
            txtParticulars.Text = vchmst.Particulars;
            txtPayee.Text = vchmst.Payee;
            txtCheckNo.Text = vchmst.CheckNo;
            txtCheqDate.Text = vchmst.CheqDate;
            txtCheqAmnt.Text = vchmst.CheqAmnt;
            txtMoneyRptNo.Text = vchmst.MoneyRptNo;
            txtMoneyRptDate.Text = vchmst.MoneyRptDate;
            txtControlAmt.Text = vchmst.ControlAmt;
            txtStatus.Text = "U";
            
            DataTable dt = VouchManager.GetVouchDtl(txtVchSysNo.Text);
            dgVoucherDtl.DataSource = dt;
            ViewState["vouchdtl"] = dt;
            dgVoucherDtl.DataBind();            
        }
    }
    protected void Delete_Click(object sender, EventArgs e)
    {
        if (per.AllowDelete == "Y")
        {
            if (txtVchSysNo.Text.Trim() == String.Empty)
            {
                lbAuth.Visible = false;
                ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Select a voucher first!!');", true);
                txtVchSysNo.Focus();
                return;
            }
            else if (txtValueDate.Text == "" && txtControlAmt.Text == "" && dgVoucherDtl.Visible == false)
            {
                lbAuth.Visible = false;
                ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Select a voucher first!!');", true);
            }
            else if (txtStatus.Text == "A")
            {
                lbAuth.Visible = false;
                ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('You cannot delete an authorized voucher!!');", true);
                ShowFooterTotal();
            }
            else
            {
                VouchManager.DeleteVouchMst(txtVchSysNo.Text);
                VouchManager.DeleteVouchDtl(txtVchSysNo.Text);
                ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Record Successfully deleted!!');", true);
                //VouchMst VchMst = VouchManager.GetVouchMst(txtVchSysNo.Text);
                //if (VchMst != null)
                //{
                //    DataTable vchdtl = VouchManager.GetVouchDtl(txtVchSysNo.Text);
                //    if (vchdtl.Rows.Count != 0)
                //    {
                //        ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('you can not delete master data whille child record exist!!');", true);
                //    }
                //    else
                //    {
                //        VouchManager.DeleteVouchMst(VchMst);
                //        ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Successfully deleted!!');", true);

                //        LoadGrid();
                //        BindGrid();
                //        ClearField();
                //    }
                //}
            }
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('You have no permission to delete this voucher!!');", true);
        }
    }

    private void LoadDetailGrid()
    {
        DataTable dtlTable = VouchManager.GetVouchDtl(txtVchSysNo.Text);
        dgVoucherDtl.DataSource = dtlTable;
    }
    private void DetailGridBind()
    {
        dgVoucherDtl.DataBind();
        tabVch.ActiveTabIndex = 0;
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        if (per.AllowPrint == "Y")
        {
            if (txtVchSysNo.Text != "")
            {
                Session.Remove("rptbyte");
                getVoucherPdf();
                string strJS = ("<script type='text/javascript'>window.open('Default4.aspx','_blank');</script>");
                Page.ClientScript.RegisterStartupScript(this.GetType(), "strJSAlert", strJS);
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Please select voucher first!!');", true);
            }
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('You have no permission to print this voucher!!');", true);
        }
    }

    private void getVoucherPdf()
    {
        VouchMst vmst = VouchManager.GetVouchMst(txtVchSysNo.Text);
        Response.Clear();
        Response.ContentType = "application/pdf";
        Response.AddHeader("content-disposition", "attachment; filename='Voucher-Information'.pdf");
        Document document = new Document();
        document = new Document(PageSize.A4);
        MemoryStream ms = new MemoryStream();
        PdfWriter writer = PdfWriter.GetInstance(document, Response.OutputStream);
        pdfPage page = new pdfPage();
        writer.PageEvent = page;
        document.Open();
        //Rectangle page = document.PageSize;
        //PdfPTable head = new PdfPTable(1);
        //head.TotalWidth = page.Width - 20;
        //Phrase phrase = new Phrase(DateTime.Now.ToString("dd/MM/yyyy"), new Font(Font.FontFamily.TIMES_ROMAN, 8));
        //PdfPCell c = new PdfPCell(phrase);
        //c.Border = Rectangle.NO_BORDER;
        //c.VerticalAlignment = Element.ALIGN_BOTTOM;
        //c.HorizontalAlignment = Element.ALIGN_RIGHT;
        //head.AddCell(c);
        //head.WriteSelectedRows(0, -1, 0, page.Height - document.TopMargin + head.TotalHeight + 20, writer.DirectContent);

        PdfPCell cell;
        if (vmst != null)
        {
            byte[] logo = GlBookManager.GetGlLogo(Session["book"].ToString());
            iTextSharp.text.Image gif = iTextSharp.text.Image.GetInstance(logo);
            gif.Alignment = iTextSharp.text.Image.MIDDLE_ALIGN;
            gif.ScalePercent(30f);

            float[] titwidth = new float[2] { 10, 200 };
            PdfPTable dth = new PdfPTable(titwidth);
            dth.WidthPercentage = 100;

            cell = new PdfPCell(gif);
            cell.HorizontalAlignment = 1;
            cell.VerticalAlignment = 1;
            cell.Rowspan = 4;
            cell.BorderWidth = 0f;
            cell.FixedHeight = 80f;
            dth.AddCell(cell);
            cell = new PdfPCell(new Phrase(Session["org"].ToString(), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12, iTextSharp.text.Font.BOLD)));
            cell.HorizontalAlignment = 1;
            cell.VerticalAlignment = 1;
            cell.BorderWidth = 0f;
            cell.FixedHeight = 20f;
            dth.AddCell(cell);
            cell = new PdfPCell(new Phrase(Session["add1"].ToString(), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)));
            cell.HorizontalAlignment = 1;
            cell.VerticalAlignment = 1;
            cell.BorderWidth = 0f;
            cell.FixedHeight = 20f;
            dth.AddCell(cell);
            cell = new PdfPCell(new Phrase(Session["add2"].ToString(), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)));
            cell.HorizontalAlignment = 1;
            cell.VerticalAlignment = 1;
            cell.BorderWidth = 0f;
            cell.FixedHeight = 20f;
            dth.AddCell(cell);
            string vchtype = "";
            if (vmst.VchCode == "02") { vchtype = "Credit Voucher"; }
            else if (vmst.VchCode == "01") { vchtype = "Debit Voucher"; }
            else if (vmst.VchCode == "03") { vchtype = "Journal Voucher"; }

            cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)));
            cell.HorizontalAlignment = 1;
            cell.VerticalAlignment = 1;
            cell.BorderWidth = 0f;
            cell.FixedHeight = 20f;
            dth.AddCell(cell);
            document.Add(dth);
            LineSeparator line = new LineSeparator(1f, 100, null, Element.ALIGN_CENTER, -2);
            document.Add(line);
            PdfPTable dtempty = new PdfPTable(1);
            cell = new PdfPCell(FormatHeaderPhrase(""));
            cell.BorderWidth = 0f;
            cell.FixedHeight = 10f;
            dtempty.AddCell(cell);
            document.Add(dtempty);

            float[] widthmst = new float[5] { 20, 20, 20, 20, 20 };
            PdfPTable pdtMst = new PdfPTable(widthmst);
            pdtMst.WidthPercentage = 100;
            cell = new PdfPCell(FormatPhrase("Voucher No"));
            cell.HorizontalAlignment = 0;
            cell.VerticalAlignment = 1;
            cell.BorderWidth = 0;
            cell.FixedHeight = 18f;
            cell.BorderColor = BaseColor.LIGHT_GRAY;
            pdtMst.AddCell(cell);
            cell = new PdfPCell(FormatPhrase(vmst.VchSysNo));
            cell.HorizontalAlignment = 0;
            cell.VerticalAlignment = 1;
            cell.FixedHeight = 18f;
            cell.BorderColor = BaseColor.LIGHT_GRAY;
            pdtMst.AddCell(cell);
            cell = new PdfPCell(FormatPhrase(""));
            cell.HorizontalAlignment = 1;
            cell.VerticalAlignment = 1;
            cell.BorderWidth = 0;
            cell.FixedHeight = 18f;
            cell.BorderColor = BaseColor.LIGHT_GRAY;
            pdtMst.AddCell(cell);
            cell = new PdfPCell(FormatPhrase("Voucher Date"));
            cell.HorizontalAlignment = 0;
            cell.VerticalAlignment = 1;
            cell.BorderWidth = 0;
            cell.FixedHeight = 18f;
            cell.BorderColor = BaseColor.LIGHT_GRAY;
            pdtMst.AddCell(cell);
            cell = new PdfPCell(FormatPhrase(vmst.ValueDate));
            cell.HorizontalAlignment = 0;
            cell.VerticalAlignment = 1;
            cell.FixedHeight = 18f;
            cell.BorderColor = BaseColor.LIGHT_GRAY;
            pdtMst.AddCell(cell);
            cell = new PdfPCell(FormatPhrase("Reference No"));
            cell.HorizontalAlignment = 0;
            cell.VerticalAlignment = 1;
            cell.BorderWidth = 0;
            cell.FixedHeight = 18f;
            cell.BorderColor = BaseColor.LIGHT_GRAY;
            pdtMst.AddCell(cell);
            cell = new PdfPCell(FormatPhrase(vmst.RefFileNo));
            cell.HorizontalAlignment = 0;
            cell.VerticalAlignment = 1;
            cell.FixedHeight = 18f;
            cell.BorderColor = BaseColor.LIGHT_GRAY;
            pdtMst.AddCell(cell);
            cell = new PdfPCell(FormatPhrase(""));
            cell.HorizontalAlignment = 0;
            cell.VerticalAlignment = 1;
            cell.BorderWidth = 0;
            cell.FixedHeight = 18f;
            cell.BorderColor = BaseColor.LIGHT_GRAY;
            pdtMst.AddCell(cell);
            cell = new PdfPCell(FormatPhrase("Revenue/Project"));
            cell.HorizontalAlignment = 0;
            cell.VerticalAlignment = 1;
            cell.BorderWidth = 0;
            cell.FixedHeight = 18f;
            cell.BorderColor = BaseColor.LIGHT_GRAY;
            pdtMst.AddCell(cell);
            string rev = "";
            if (vmst.TransType == "P") { rev = "Project"; }
            else if (vmst.TransType == "R") { rev = "Revenue"; }
            cell = new PdfPCell(FormatPhrase(rev));
            cell.HorizontalAlignment = 0;
            cell.VerticalAlignment = 1;
            cell.FixedHeight = 18f;
            cell.BorderColor = BaseColor.LIGHT_GRAY;
            pdtMst.AddCell(cell);
            cell = new PdfPCell(FormatPhrase("Volume/Serial No"));
            cell.HorizontalAlignment = 0;
            cell.VerticalAlignment = 1;
            cell.BorderWidth = 0;
            cell.FixedHeight = 18f;
            cell.BorderColor = BaseColor.LIGHT_GRAY;
            pdtMst.AddCell(cell);
            cell = new PdfPCell(FormatPhrase("Vol#" + vmst.VolumeNo + ", Sl#" + vmst.SerialNo));
            cell.HorizontalAlignment = 0;
            cell.VerticalAlignment = 1;
            cell.FixedHeight = 18f;
            cell.BorderColor = BaseColor.LIGHT_GRAY;
            pdtMst.AddCell(cell);
            cell = new PdfPCell(FormatPhrase(""));
            cell.HorizontalAlignment = 1;
            cell.VerticalAlignment = 1;
            cell.BorderWidth = 0;
            cell.FixedHeight = 18f;
            cell.BorderColor = BaseColor.LIGHT_GRAY;
            pdtMst.AddCell(cell);
            cell = new PdfPCell(FormatPhrase("Cheque No"));
            cell.HorizontalAlignment = 0;
            cell.VerticalAlignment = 1;
            cell.BorderWidth = 0;
            cell.FixedHeight = 18f;
            cell.BorderColor = BaseColor.LIGHT_GRAY;
            pdtMst.AddCell(cell);
            cell = new PdfPCell(FormatPhrase(vmst.CheckNo));
            cell.HorizontalAlignment = 0;
            cell.VerticalAlignment = 1;
            cell.FixedHeight = 18f;
            cell.BorderColor = BaseColor.LIGHT_GRAY;
            pdtMst.AddCell(cell);
            cell = new PdfPCell(FormatPhrase("Cheque Date"));
            cell.HorizontalAlignment = 0;
            cell.VerticalAlignment = 1;
            cell.BorderWidth = 0;
            cell.FixedHeight = 18f;
            cell.BorderColor = BaseColor.LIGHT_GRAY;
            pdtMst.AddCell(cell);
            cell = new PdfPCell(FormatPhrase(vmst.CheqDate));
            cell.HorizontalAlignment = 0;
            cell.VerticalAlignment = 1;
            cell.FixedHeight = 18f;
            cell.BorderColor = BaseColor.LIGHT_GRAY;
            pdtMst.AddCell(cell);
            cell = new PdfPCell(FormatPhrase(""));
            cell.HorizontalAlignment = 1;
            cell.VerticalAlignment = 1;
            cell.BorderWidth = 0;
            cell.FixedHeight = 18f;
            cell.BorderColor = BaseColor.LIGHT_GRAY;
            pdtMst.AddCell(cell);
            cell = new PdfPCell(FormatPhrase("Cheque Amount"));
            cell.HorizontalAlignment = 0;
            cell.VerticalAlignment = 1;
            cell.BorderWidth = 0;
            cell.FixedHeight = 18f;
            cell.BorderColor = BaseColor.LIGHT_GRAY;
            pdtMst.AddCell(cell);
            cell = new PdfPCell(FormatPhrase(vmst.CheqAmnt));
            cell.HorizontalAlignment = 0;
            cell.VerticalAlignment = 1;
            cell.FixedHeight = 18f;
            cell.BorderColor = BaseColor.LIGHT_GRAY;
            pdtMst.AddCell(cell);
            cell = new PdfPCell(FormatPhrase("Name of Payee"));
            cell.HorizontalAlignment = 0;
            cell.VerticalAlignment = 1;
            cell.BorderWidth = 0;
            cell.FixedHeight = 18f;
            cell.BorderColor = BaseColor.LIGHT_GRAY;
            pdtMst.AddCell(cell);
            cell = new PdfPCell(FormatPhrase(vmst.Payee));
            cell.HorizontalAlignment = 0;
            cell.VerticalAlignment = 1;
            cell.Colspan = 4;
            cell.FixedHeight = 18f;
            cell.BorderColor = BaseColor.LIGHT_GRAY;
            pdtMst.AddCell(cell);
            cell = new PdfPCell(FormatPhrase("Particulars"));
            cell.HorizontalAlignment = 0;
            cell.VerticalAlignment = 1;
            cell.BorderWidth = 0;
            cell.FixedHeight = 18f;
            cell.BorderColor = BaseColor.LIGHT_GRAY;
            pdtMst.AddCell(cell);
            cell = new PdfPCell(FormatPhrase(vmst.Particulars));
            cell.HorizontalAlignment = 0;
            cell.VerticalAlignment = 1;
            cell.Colspan = 4;
            cell.FixedHeight = 60f;
            cell.BorderColor = BaseColor.LIGHT_GRAY;
            pdtMst.AddCell(cell);
            document.Add(pdtMst);

            DataTable dtdtl = VouchManager.GetVouchDtlRpt(vmst.VchSysNo);
            float[] widthdtl = new float[3] {  50, 20, 20 };
            PdfPTable pdtDtl = new PdfPTable(widthdtl);
            pdtDtl.WidthPercentage = 100;
            decimal ctot = decimal.Zero;
            decimal dtot = decimal.Zero;
            //foreach (DataRow dr in dtdtl.Rows)
            //{
            //    if (dr["amount_cr"].ToString() != "")
            //    {
            //        ctot += decimal.Parse(dr["amount_cr"].ToString());
            //    }
            //    if (dr["amount_dr"].ToString() != "")
            //    {
            //        dtot += decimal.Parse(dr["amount_dr"].ToString());
            //    }
            //}
            //DataRow drd = dtdtl.NewRow();
            //drd["particulars"] = "Total";
            //drd["amount_cr"] = ctot.ToString("N2");
            //drd["amount_dr"] = dtot.ToString("N2");
            //dtdtl.Rows.Add(drd);
            //cell = new PdfPCell(FormatHeaderPhrase("Account Code"));
            //cell.HorizontalAlignment = 1;
            //cell.VerticalAlignment = 1;
            //cell.MinimumHeight = 18f;
            //cell.BorderColor = BaseColor.LIGHT_GRAY;
            //pdtDtl.AddCell(cell);
            cell = new PdfPCell(FormatHeaderPhrase("Account Description"));
            cell.HorizontalAlignment = 1;
            cell.VerticalAlignment = 1;
            cell.FixedHeight = 18f;
            cell.BorderColor = BaseColor.LIGHT_GRAY;
            pdtDtl.AddCell(cell);
            cell = new PdfPCell(FormatHeaderPhrase("Debit Taka"));
            cell.HorizontalAlignment = 1;
            cell.VerticalAlignment = 1;
            cell.FixedHeight = 18f;
            cell.BorderColor = BaseColor.LIGHT_GRAY;
            pdtDtl.AddCell(cell);
            cell = new PdfPCell(FormatHeaderPhrase("Credit Taka"));
            cell.HorizontalAlignment = 1;
            cell.VerticalAlignment = 1;
            cell.FixedHeight = 18f;
            cell.BorderColor = BaseColor.LIGHT_GRAY;
            pdtDtl.AddCell(cell);

            foreach (DataRow row in dtdtl.Rows)
            {
                if (row["particulars"].ToString() != "")
                {
                    cell = new PdfPCell(FormatPhrase(row["particulars"].ToString()));
                    cell.HorizontalAlignment = 0;
                    cell.VerticalAlignment = 1;
                    cell.FixedHeight = 20f;
                    cell.BorderColor = BaseColor.LIGHT_GRAY;
                    pdtDtl.AddCell(cell);
                    cell = new PdfPCell(FormatPhrase(row["amount_dr"].ToString()));
                    cell.HorizontalAlignment = 2;
                    cell.VerticalAlignment = 1;
                    cell.FixedHeight = 20f;
                    cell.BorderColor = BaseColor.LIGHT_GRAY;
                    pdtDtl.AddCell(cell);
                    cell = new PdfPCell(FormatPhrase(row["amount_cr"].ToString()));
                    cell.HorizontalAlignment = 2;
                    cell.VerticalAlignment = 1;
                    cell.FixedHeight = 20f;
                    cell.BorderColor = BaseColor.LIGHT_GRAY;
                    pdtDtl.AddCell(cell);

                    if (row["amount_cr"].ToString() != "")
                    {
                        ctot += decimal.Parse(row["amount_cr"].ToString());
                    }
                    if (row["amount_dr"].ToString() != "")
                    {
                        dtot += decimal.Parse(row["amount_dr"].ToString());
                    }
                }
            }
            decimal Total = (ctot + dtot);
            cell = new PdfPCell(FormatHeaderPhrase("Total"));
            cell.HorizontalAlignment = 2;
            cell.VerticalAlignment = 1;
            cell.FixedHeight = 18f;
            cell.BorderColor = BaseColor.LIGHT_GRAY;
            pdtDtl.AddCell(cell);
            cell = new PdfPCell(FormatPhrase(dtot.ToString("N2")));
            cell.HorizontalAlignment = 2;
            cell.VerticalAlignment = 1;
            cell.FixedHeight = 20f;
            cell.BorderColor = BaseColor.LIGHT_GRAY;
            pdtDtl.AddCell(cell);
            cell = new PdfPCell(FormatPhrase(ctot.ToString("N2")));
            cell.HorizontalAlignment = 2;
            cell.VerticalAlignment = 1;
            cell.FixedHeight = 20f;
            cell.BorderColor = BaseColor.LIGHT_GRAY;
            pdtDtl.AddCell(cell);
            //for (int i = 0; i < dtdtl.Rows.Count; i++)
            //{
            //    for (int j = 0; j < dtdtl.Columns.Count; j++)
            //    {
            //        string val = "";
            //        if (j > 1)
            //        {
            //            if (((DataRow)dtdtl.Rows[i])[j].ToString() != "")
            //            {
            //                val = decimal.Parse(((DataRow)dtdtl.Rows[i])[j].ToString()).ToString("N2");
            //            }
            //        }
            //        else
            //        {
            //            val = ((DataRow)dtdtl.Rows[i])[j].ToString();
            //        }
            //        cell = new PdfPCell(FormatPhrase(val));
            //        if (j == 1)
            //        {
            //            cell.HorizontalAlignment = 0;
            //        }
            //        else if (j > 1)
            //        {
            //            cell.HorizontalAlignment = 2;
            //        }
            //        else
            //        {
            //            cell.HorizontalAlignment = 1;
            //        }
            //        cell.VerticalAlignment = 1;
            //        cell.FixedHeight = 18f;
            //        cell.BorderColor = BaseColor.LIGHT_GRAY;
            //        pdtDtl.AddCell(cell);
            //    }
            //}
            document.Add(pdtDtl);

            PdfPTable dtempty1 = new PdfPTable(1);
            dtempty1.WidthPercentage = 100;
            cell = new PdfPCell(FormatPhrase("In word: " + DataManager.GetLiteralAmt(dtot.ToString()).Replace("  ", " ").Replace("  ", " ")));
            cell.VerticalAlignment = 0;
            cell.HorizontalAlignment = 0;
            cell.BorderWidth = 0f;
            cell.FixedHeight = 60f;
            dtempty1.AddCell(cell);
            document.Add(dtempty1);

            float[] widthsig = new float[] { 20, 5, 20, 5, 20 };
            PdfPTable pdtsig = new PdfPTable(widthsig);
            pdtsig.WidthPercentage = 100;
            cell = new PdfPCell(FormatPhrase("Accountant"));
            cell.HorizontalAlignment = 1;
            cell.VerticalAlignment = 1;
            cell.Border = 1;
            cell.FixedHeight = 18f;
            cell.BorderColor = BaseColor.LIGHT_GRAY;
            pdtsig.AddCell(cell);
            cell = new PdfPCell(FormatPhrase(""));
            cell.HorizontalAlignment = 0;
            cell.VerticalAlignment = 1;
            cell.Border = 0;
            cell.FixedHeight = 18f;
            cell.BorderColor = BaseColor.LIGHT_GRAY;
            pdtsig.AddCell(cell);
            cell = new PdfPCell(FormatPhrase("Accounts Officer"));
            cell.HorizontalAlignment = 1;
            cell.VerticalAlignment = 1;
            cell.Border = 1;
            cell.FixedHeight = 18f;
            cell.BorderColor = BaseColor.LIGHT_GRAY;
            pdtsig.AddCell(cell);
            cell = new PdfPCell(FormatPhrase(""));
            cell.HorizontalAlignment = 0;
            cell.VerticalAlignment = 1;
            cell.Border = 0;
            cell.FixedHeight = 18f;
            cell.BorderColor = BaseColor.LIGHT_GRAY;
            pdtsig.AddCell(cell);
            cell = new PdfPCell(FormatPhrase("Chief Accounts Officer"));
            cell.HorizontalAlignment = 1;
            cell.VerticalAlignment = 1;
            cell.Border = 1;
            cell.FixedHeight = 18f;
            cell.BorderColor = BaseColor.LIGHT_GRAY;
            pdtsig.AddCell(cell);
            document.Add(pdtsig);
        }

        document.Close();
        Response.Flush();
        Response.End();
        //byte[] byt = ms.GetBuffer();
        //if (Session["rptbyte"] != null) { byte[] rptbyt = (byte[])Session["rptbyte"]; rptbyt = byt; } else { Session["rptbyte"] = byt; }
    }
    
    protected void ShowChild_Click(object sender, EventArgs e)
    {
        if (txtVchSysNo.Text.Trim() == String.Empty)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Input voucher No First!!');", true);
            txtVchSysNo.Focus();
            return;
        }
        DataTable dt = VouchManager.GetVouchDtl(txtVchSysNo.Text);        
        if (dt.Rows.Count > 0)
        {
            dgVoucherDtl.DataSource = dt;
            DetailGridBind();
            ViewState["vouchdtl"] = dt;            
            //ShowFooterTotal();
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('No detail record!!');", true);
            getVoucherDtlGrid();
        }
    }
    
    protected void DeleteChild_Click(object sender, EventArgs e)
    {
        if (txtVchSysNo.Text.Trim() == String.Empty)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Input voucher no first!!');", true);
            txtVchSysNo.Focus();
            return;
        }
        VouchManager.DeleteVouchDtl(txtVchSysNo.Text);
        ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Detail record deleted successfully!!');", true);
        ShowChild_Click(sender, e);
        ShowFooterTotal();
    }

    protected void dgVoucherDtl_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string dr = "0";
            string cr = "0";
            if (((DataRowView)e.Row.DataItem)["amount_dr"].ToString() != "")
            {
                dr = ((DataRowView)e.Row.DataItem)["amount_dr"].ToString();
            }
            if (((DataRowView)e.Row.DataItem)["amount_cr"].ToString() != "")
            {
                cr = ((DataRowView)e.Row.DataItem)["amount_cr"].ToString();
            }
            ((TextBox)e.Row.FindControl("txtDebit")).Text = decimal.Parse(dr).ToString("N2");
            ((TextBox)e.Row.FindControl("txtCredit")).Text = decimal.Parse(cr).ToString("N2");
        }
    }
    protected void dgVoucher_PageIndexChanging1(object sender, GridViewPageEventArgs e)
    {
        dgVoucher.PageIndex = e.NewPageIndex;
        LoadGrid();
        dgVoucher.DataBind();
    }


    protected void dgVoucherDtl_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        if (Session["vouchdtl"] != null)
        {
            DataTable dt = (DataTable)Session["vouchdtl"];
            dt.Rows.RemoveAt(dgVoucherDtl.Rows[e.RowIndex].DataItemIndex);
            string found = "";
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["gl_coa_code"].ToString() == "" && dr["particulars"].ToString() == "")
                {
                    found = "Y";
                }
            }
            if (found == "")
            {
                DataRow dr = dt.NewRow();
                dr["line_no"] = (dt.Rows.Count + 1).ToString();
                dt.Rows.Add(dr);
            }
            if (dt.Rows.Count > 0)
            {
                int x = 1;
                foreach (DataRow drdt in dt.Rows)
                {
                    drdt.BeginEdit();
                    drdt["line_no"] = x;
                    drdt.AcceptChanges();
                    x = x + 1;
                }
                dgVoucherDtl.DataSource = dt;
                DetailGridBind();
            }
            else
            {
                getVoucherDtlGrid();
            }
            ShowFooterTotal();
            ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Record/s is/are in Delete queue. To Permanently delete click on Update Voucher!!');", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Your session is over. Please do these again!!');", true);
        }
    }

    protected void txtDebit_TextChanged(object sender, EventArgs e)
    {
        TextBox tb = (TextBox)sender;
        GridViewRow row = (GridViewRow)tb.NamingContainer;
        addVouchDtl(row, "Y", "amount_dr");
        ShowFooterTotal();
        ((TextBox)dgVoucherDtl.Rows[dgVoucherDtl.Rows.Count - 1].FindControl("txtCoaDesc")).Focus();

    }
    protected void txtCredit_TextChanged(object sender, EventArgs e)
    {
        TextBox tb = (TextBox)sender;
        GridViewRow row = (GridViewRow)tb.NamingContainer;
        addVouchDtl(row, "Y", "amount_cr");
        ShowFooterTotal();
        ((TextBox)dgVoucherDtl.Rows[dgVoucherDtl.Rows.Count - 1].FindControl("txtCoaDesc")).Focus();

    }
    protected void txtGlCode_TextChanged(object sender, EventArgs e)
    {
        TextBox tb = (TextBox)sender;
        GridViewRow row = (GridViewRow)tb.NamingContainer;
        addVouchDtl(row, "N", "gl_coa_code");
        ShowFooterTotal();
        ((TextBox)dgVoucherDtl.Rows[dgVoucherDtl.Rows.Count - 1].FindControl("txtCoaDesc")).Focus();
    }

    protected void txtCoaDesc_TextChanged(object sender, EventArgs e)
    {
        GridViewRow row = (GridViewRow)((TextBox)sender).NamingContainer;
        addVouchDtl(row, "N", "coa_desc");
        ShowFooterTotal();
        //((TextBox)row.FindControl("txtDebit")).Attributes.Add("onBlur", "setDecimal('" + ((TextBox)row.FindControl("txtDebit")).ClientID + "')");
        //((TextBox)row.FindControl("txtCredit")).Attributes.Add("onBlur", "setDecimal('" + ((TextBox)row.FindControl("txtCredit")).ClientID + "')");
        ((TextBox)dgVoucherDtl.Rows[dgVoucherDtl.Rows.Count - 1].FindControl("txtCoaDesc")).Focus();
    }
    private void addVouchDtl(GridViewRow row, string ownvalue, string colname)
    {
        DataTable dt = new DataTable();
        string param1 = "";
        string param2 = "";
        param1 = ((TextBox)row.FindControl("txtGlCoaCode")).Text;
        param2 = ((TextBox)row.FindControl("txtCoaDesc")).Text;
        if (colname != "coa_desc")
        {
            dt = VouchManager.getCoaCode(((TextBox)row.FindControl("txtGlCoaCode")).Text, "");
        }
        else
        {
            dt = VouchManager.getCoaCode("", ((TextBox)row.FindControl("txtCoaDesc")).Text);
        }
        decimal ctlamt = Decimal.Zero;
        string glcode = "";
        string gldesc = "";
        if (txtControlAmt.Text != "")
        {
            ctlamt = decimal.Parse(txtControlAmt.Text);
        }
        if (dt.Rows.Count > 0)
        {
            glcode = ((DataRow)dt.Rows[0])[0].ToString();
            gldesc = ((DataRow)dt.Rows[0])[1].ToString();
        }
        dt.Dispose();

        string found = "";
        int fndline = 0;
        if (glcode != "")
        {
            DataTable dtv = (DataTable)ViewState["vouchdtl"];
            for (int i = 0; i < dtv.Rows.Count; i++)
            {
                if (i != row.DataItemIndex)
                {
                    if (glcode == ((DataRow)dtv.Rows[i])["gl_coa_code"].ToString())
                    {
                        found = "Y";
                        fndline = i + 1;
                    }
                }
            }

            if (found == "Y")
            {
                ((TextBox)row.FindControl("txtGlCoaCode")).Text = "";
                ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('You have already use this COA at line " + fndline.ToString() + " !!');", true);
                return;
            }
            else
            {
                dtv.Rows.RemoveAt(row.DataItemIndex);
                DataRow dr = dtv.NewRow();
                dr["line_no"] = ((TextBox)row.FindControl("txtLineNo")).Text;
                dr["gl_coa_code"] = glcode;
                dr["particulars"] = gldesc;

                if (ownvalue == "N")
                {

                    if (decimal.Parse(((TextBox)row.FindControl("txtDebit")).Text) > 0 | decimal.Parse(((TextBox)row.FindControl("txtCredit")).Text) > 0)
                    {
                        dr["amount_dr"] = ((TextBox)row.FindControl("txtDebit")).Text;
                        dr["amount_cr"] = ((TextBox)row.FindControl("txtCredit")).Text;
                    }
                    else
                    {
                        decimal pdr = GetTotal("amount_dr");
                        decimal pcr = GetTotal("amount_cr");
                        if (pdr < decimal.Parse(txtControlAmt.Text))
                        {
                            dr["amount_dr"] = (ctlamt - pdr).ToString("N2");
                            dr["amount_cr"] = "0";
                        }
                        else
                        {
                            if (pcr < decimal.Parse(txtControlAmt.Text))
                            {
                                dr["amount_cr"] = (ctlamt - pcr).ToString("N2");
                                dr["amount_dr"] = "0";
                            }
                        }
                    }
                }
                else if (ownvalue == "Y")
                {
                    dr["amount_dr"] = ((TextBox)row.FindControl("txtDebit")).Text;
                    dr["amount_cr"] = ((TextBox)row.FindControl("txtCredit")).Text;
                }
                dtv.Rows.InsertAt(dr, row.DataItemIndex);
                string nrow = "";
                for (int x = 0; x < dtv.Rows.Count; x++)
                {
                    if (((DataRow)dtv.Rows[x])["gl_coa_code"].ToString() == "" && ((DataRow)dtv.Rows[x])["particulars"].ToString() == "")
                    {
                        nrow = "N";
                    }
                }
                if (nrow == "")
                {
                    dr = dtv.NewRow();
                    dr["line_no"] = (dtv.Rows.Count + 1).ToString();
                    dtv.Rows.Add(dr);
                }

                ViewState["vouchdtl"] = dtv;
                dgVoucherDtl.DataSource = dtv;
                dgVoucherDtl.DataBind();
            }
        }
    }
    private void ShowFooterTotal()
    {
        if (dgVoucherDtl.Rows.Count>0)
        {
            GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Footer, DataControlRowState.Normal);
            TableCell cell;
            int j;
            if (dgVoucherDtl.Columns[0].Visible == true)
            {
                j = dgVoucherDtl.Columns.Count - 3;
            }
            else
            {
                j = dgVoucherDtl.Columns.Count - 4;
            }

            for (int i = 0; i < j; i++)
            {
                cell = new TableCell();
                cell.Text = "";
                row.Cells.Add(cell);
            }
            cell = new TableCell();
            cell.Text = "Total";
            row.Cells.Add(cell);
            cell = new TableCell();
            decimal priceCr = decimal.Zero;
            decimal priceDr = decimal.Zero;            
            priceDr = GetTotal("amount_dr");
            cell.Text = priceDr.ToString("N2");
            cell.HorizontalAlign = HorizontalAlign.Right;
            row.Cells.Add(cell);
            cell = new TableCell();            
            priceCr = GetTotal("amount_cr");
            cell.Text = priceCr.ToString("N2");
            cell.HorizontalAlign = HorizontalAlign.Right;
            row.Cells.Add(cell);
            row.Font.Bold = true;
            //dgVoucherDtl.Visible = true;
            dgVoucherDtl.Controls[0].Controls.Add(row);
            ViewState["pricecr"] = priceCr.ToString();
            ViewState["pricedr"] = priceDr.ToString();
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        decimal priceCr = decimal.Zero;
        decimal priceDr = decimal.Zero;
        if (ViewState["pricecr"] != null && ViewState["pricedr"] != null)
        {
            priceCr = decimal.Parse(ViewState["pricecr"].ToString());
            priceDr = decimal.Parse(ViewState["pricedr"].ToString());
        }
        if (txtValueDate.Text=="" && txtControlAmt.Text == "" && dgVoucherDtl.Visible==false)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Please click on New button for new voucher entry!!');", true);
        }
        else if (txtValueDate.Text == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Voucher date cannot be null!!');", true);
        }
        else if (ddlFinMon.SelectedItem.Text != FinYearManager.getFinMonthByDate(txtValueDate.Text))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Voucher date does not comply with the financial month!!');", true);
        }
        else if (txtControlAmt.Text == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Voucher Amount Can Not Be Blank!!');", true);
        }
        else if (priceCr != priceDr | decimal.Parse(txtControlAmt.Text) != priceCr)
        {
            txtCheckNo.Text = priceCr.ToString() + "-" + priceDr.ToString() + "-" + txtControlAmt.Text;
            ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Debit And Credit or Control Amount are Not Equal or No Details!!');", true);
        }
        else if (txtStatus.Text == "A")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('You cannot change already authorized record!!');", true);
        }
        else
        {
            VouchMst vchmst;
            if (txtVchSysNo.Text == "")
            {
                if (per.AllowAdd == "Y")
                {
                    vchmst = new VouchMst();
                    vchmst.FinMon = ddlFinMon.SelectedItem.Text;
                    vchmst.ValueDate = txtValueDate.Text;
                    vchmst.RefFileNo = txtRefFileNo.Text.ToString();
                    vchmst.VolumeNo = txtVolumeNo.Text.ToString();
                    vchmst.SerialNo = txtSerialNo.Text.ToString();
                    vchmst.VchCode = txtVchCode.SelectedItem.Value;
                    vchmst.TransType = ddlTransType.SelectedValue;
                    vchmst.Particulars = txtParticulars.Text.Replace("'", "''");
                    vchmst.Payee = txtPayee.Text.Replace("'", "''");
                    vchmst.CheckNo = txtCheckNo.Text;
                    vchmst.CheqDate = txtCheqDate.Text;
                    vchmst.CheqAmnt = txtCheqAmnt.Text;
                    vchmst.MoneyRptNo = txtMoneyRptNo.Text;
                    vchmst.MoneyRptDate = txtMoneyRptDate.Text;
                    vchmst.ControlAmt = txtControlAmt.Text;
                    vchmst.Status = txtStatus.Text;
                    vchmst.BookName = Session["book"].ToString();
                    vchmst.EntryUser = Session["user"].ToString().ToUpper();
                    vchmst.AuthoUserType = Session["userlevel"].ToString();
                    vchmst.EntryDate = System.DateTime.Now.ToString("dd/MM/yyyy");
                    vchmst.VchSysNo = IdManager.GetNextID("gl_trans_mst", "vch_sys_no").ToString();
                    if (txtVchCode.SelectedValue == "01")
                    {
                        vchmst.VchRefNo = "DV-" + vchmst.VchSysNo.ToString().PadLeft(10, '0');
                        txtVchRefNo.Text = "DV-" + vchmst.VchSysNo.ToString().PadLeft(10, '0');
                    }
                    else if (txtVchCode.SelectedValue == "02")
                    {
                        vchmst.VchRefNo = "CV-" + vchmst.VchSysNo.ToString().PadLeft(10, '0');
                        txtVchRefNo.Text = "CV-" + vchmst.VchSysNo.ToString().PadLeft(10, '0');
                    }
                    else if (txtVchCode.SelectedValue == "03")
                    {
                        vchmst.VchRefNo = "JV-" + vchmst.VchSysNo.ToString().PadLeft(10, '0');
                        txtVchRefNo.Text = "JV-" + vchmst.VchSysNo.ToString().PadLeft(10, '0');
                    }
                    txtVchSysNo.Text = vchmst.VchSysNo;
                    if (per.AllowAdd == "Y")
                    {
                        VouchManager.CreateVouchMst(vchmst);
                    }

                    DataTable dt = (DataTable)ViewState["vouchdtl"];
                    VouchDtl VchDtl;
                    foreach (DataRow drdt in dt.Rows)
                    {
                        if (drdt["line_no"].ToString() != "" && drdt["gl_coa_code"].ToString() != "")
                        {
                            VchDtl = new VouchDtl();
                            VchDtl.VchSysNo = txtVchSysNo.Text;
                            VchDtl.ValueDate = txtValueDate.Text;
                            VchDtl.LineNo = drdt["line_no"].ToString();
                            VchDtl.GlCoaCode = drdt["gl_coa_code"].ToString();
                            VchDtl.Particulars = drdt["particulars"].ToString();
                            VchDtl.AccType = VouchManager.getAccType(drdt["gl_coa_code"].ToString());
                            VchDtl.AmountDr = drdt["amount_dr"].ToString();
                            VchDtl.AmountCr = drdt["amount_cr"].ToString();
                            VchDtl.Status = "U";
                            VchDtl.BookName = Session["book"].ToString();
                            VouchManager.CreateVouchDtl(VchDtl);
                        }
                    }
                    ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Saved Successfully!!');", true);
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('You have no permission to add any datam in this form!!');", true);
                }
            }
            else
            {
                if (per.AllowEdit == "Y")
                {
                    vchmst = VouchManager.GetVouchMst(txtVchSysNo.Text.ToString());
                    if (vchmst != null)
                    {
                        if (vchmst.FinMon != ddlFinMon.SelectedItem.Text | vchmst.ValueDate != txtValueDate.Text |
                            vchmst.VchRefNo != txtVchRefNo.Text | vchmst.RefFileNo != txtRefFileNo.Text |
                             vchmst.VolumeNo != txtVolumeNo.Text | vchmst.SerialNo != txtSerialNo.Text |
                            vchmst.VchCode != txtVchCode.SelectedValue | vchmst.Particulars != txtParticulars.Text |
                            vchmst.Payee != txtPayee.Text | vchmst.CheckNo != txtCheckNo.Text |
                            vchmst.CheqDate != txtCheqDate.Text | vchmst.ControlAmt != txtCheqAmnt.Text |
                            vchmst.MoneyRptNo != txtMoneyRptNo.Text | vchmst.MoneyRptDate != txtMoneyRptDate.Text |
                            vchmst.ControlAmt != txtControlAmt.Text | vchmst.Status != txtStatus.Text)
                        {
                            vchmst.FinMon = ddlFinMon.SelectedItem.Text;
                            vchmst.ValueDate = txtValueDate.Text;
                            vchmst.VchRefNo = txtVchRefNo.Text;
                            vchmst.RefFileNo = txtRefFileNo.Text;
                            vchmst.VolumeNo = txtVolumeNo.Text;
                            vchmst.SerialNo = txtSerialNo.Text;
                            vchmst.VchCode = txtVchCode.SelectedItem.Value;
                            vchmst.TransType = ddlTransType.SelectedValue;
                            vchmst.Particulars = txtParticulars.Text.Replace("'", "''");
                            vchmst.Payee = txtPayee.Text.Replace("'", "''");
                            vchmst.CheckNo = txtCheckNo.Text;
                            vchmst.CheqDate = txtCheqDate.Text;
                            vchmst.CheqAmnt = txtCheqAmnt.Text;
                            vchmst.MoneyRptNo = txtMoneyRptNo.Text;
                            vchmst.MoneyRptDate = txtMoneyRptDate.Text;
                            vchmst.ControlAmt = txtControlAmt.Text;
                            vchmst.Status = txtStatus.Text;
                            vchmst.UpdateUser = Session["user"].ToString().ToUpper();
                            vchmst.UpdateDate = System.DateTime.Now.ToString("dd/MM/yyyy");
                            VouchManager.UpdateVouchMst(vchmst);
                        }
                        VouchManager.DeleteVouchDtl(vchmst.VchSysNo);

                        DataTable dt = (DataTable)ViewState["vouchdtl"];
                        VouchDtl VchDtl;
                        foreach (DataRow drdt in dt.Rows)
                        {
                            if (drdt["line_no"].ToString() != "" && drdt["gl_coa_code"].ToString()!="")
                            {
                                VchDtl = new VouchDtl();
                                VchDtl.VchSysNo = txtVchSysNo.Text;
                                VchDtl.ValueDate = txtValueDate.Text;
                                VchDtl.LineNo = drdt["line_no"].ToString();
                                VchDtl.GlCoaCode = drdt["gl_coa_code"].ToString();
                                VchDtl.Particulars = drdt["particulars"].ToString();
                                VchDtl.AccType = VouchManager.getAccType(drdt["gl_coa_code"].ToString());
                                VchDtl.AmountDr = drdt["amount_dr"].ToString();
                                VchDtl.AmountCr = drdt["amount_cr"].ToString();
                                VchDtl.Status = "U";
                                VchDtl.BookName = Session["book"].ToString();
                                VouchManager.CreateVouchDtl(VchDtl);
                            }
                        } 
                        ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Updated Successfully!!');", true);
                        dgVoucherDtl.ShowFooter = false;
                        LoadDetailGrid();
                        DetailGridBind();
                        if (int.Parse(Session["userlevel"].ToString()) > 1)
                        {
                            lbAuth.Visible = true;
                        }
                        else
                        {
                            lbAuth.Visible = false;
                        }
                    }
                }
                else
                {
                    //lblTranStatus.Visible = true;
                    //lblTranStatus.ForeColor = System.Drawing.Color.Red;
                    //lblTranStatus.Text = "*** You have no permission to edit/update these data!";
                    ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('You have no permission to edit/update these data!!');", true);
                }
            }
        }
        ShowFooterTotal();
    }
    private void getVoucherDtlGrid()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("line_no", typeof(string));
        dt.Columns.Add("gl_coa_code", typeof(string));
        dt.Columns.Add("particulars", typeof(string));
        dt.Columns.Add("amount_dr", typeof(string));
        dt.Columns.Add("amount_cr", typeof(string));
        DataRow dr = dt.NewRow();
        dr["line_no"] = "1";
        dt.Rows.Add(dr);
        dgVoucherDtl.DataSource = dt;
        dgVoucherDtl.DataBind();
        ViewState["vouchdtl"] = dt;
    }
    private decimal GetTotal(string ctrl)
    {
        decimal drt = 0;
        DataTable dt = (DataTable)ViewState["vouchdtl"];

        foreach (DataRow rowst in dt.Rows)
        {
            drt += decimal.Parse(string.IsNullOrEmpty(rowst[ctrl].ToString()) ? "0" : rowst[ctrl].ToString());
        }
        return drt;
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        ViewState.Remove("vouchdtl");
        ClearField();        
        VEUnauth();
        lbAuth.Visible = false;
        btnCheqPrint.Visible = false;
        RefreshDropDown();
        btnCheqPrint.Visible = true;
        txtStatus.Text = "U";
        txtVchSysNo.Enabled = false;
        txtVchRefNo.Enabled = false;
        ddlTransType.SelectedValue = "R";
        txtVchCode.SelectedValue = "01";        
        DataTable dt= new DataTable();
        dt.Columns.Add("line_no", typeof(string));
        dt.Columns.Add("gl_coa_code", typeof(string));
        dt.Columns.Add("particulars", typeof(string));
        dt.Columns.Add("amount_dr", typeof(string));
        dt.Columns.Add("amount_cr", typeof(string));
        DataRow dr = dt.NewRow();
        dr["line_no"] = "1";
        dt.Rows.Add(dr);
        dgVoucherDtl.DataSource = dt;
        dgVoucherDtl.DataBind();
        txtControlAmt.Text = "0";
        //dgVoucher.Visible = false;
        //dgVoucherDtl.Visible = true;
        ViewState["vouchdtl"] = dt;
        ShowFooterTotal();
        txtValueDate.Focus();
    }

    protected void LoginBtn_Click(object sender, EventArgs e)
    {
        if (per.AllowAutho == "Y")
        {
            string connectionString = DataManager.OraConnString();
            SqlDataReader dReader;
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = connectionString;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "Select password,user_grp from utl_userinfo where upper(user_name)=upper('" + loginId.Text + "')";
            conn.Open();
            dReader = cmd.ExecuteReader();
            if (dReader.HasRows == true)
            {
                while (dReader.Read())

                    if (pwd.Text.Trim() == dReader["password"].ToString())
                    {
                        if (int.Parse(dReader["user_grp"].ToString()) > 1)
                        {
                            VouchMst vchmst = VouchManager.GetVouchMst(txtVchSysNo.Text.ToString());
                            vchmst.Status = "A";
                            vchmst.AuthoUser = loginId.Text.ToString().ToUpper();
                            vchmst.AuthoDate = System.DateTime.Now.ToString("dd/MM/yyyy");
                            vchmst.AuthoUserType = dReader["user_grp"].ToString();
                            VouchManager.UpdateVouchMst(vchmst);
                            //lblTranStatus.Visible = true;
                            //lblTranStatus.ForeColor = System.Drawing.Color.Green;
                            //lblTranStatus.Text = "Authorized successfully.!";
                            ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Authorized successfully!!');", true);
                            txtStatus.Text = "A";
                            lbAuth.Visible = false;
                            pwd.Text = "";
                            VEAuth();
                        }
                        else
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('You have no enough permission to authorize!!');", true);
                            pwd.Text = "";
                        }
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Password Incorrect, Authentication Failed...!!');", true);
                        pwd.Text = "";
                    }
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('No Such User!!');", true);
            }
            dReader.Close();
            conn.Close();
            ShowFooterTotal();
        }
    }

    protected void CancelBtn_Click(object sender, EventArgs e)
    {
        ShowFooterTotal();
    }

    private void VEAuth()
    {
        txtVchSysNo.Enabled = false;
        txtValueDate.Enabled = false;
        txtVchCode.Enabled = false;
        txtRefFileNo.Enabled = false;
        txtVchRefNo.Enabled = false;
        ddlFinMon.Enabled = false;
        txtPayee.Enabled = false;
        txtCheckNo.Enabled = false;
        txtCheqAmnt.Enabled = false;
        txtCheqDate.Enabled = false;
        txtControlAmt.Enabled = false;
        txtMoneyRptDate.Enabled = false;
        txtMoneyRptNo.Enabled = false;
        txtParticulars.Enabled = false;
        dgVoucherDtl.Columns[0].Visible = false;
        lbAuth.Visible = false;
        btnCheqPrint.Visible = false;
        txtVolumeNo.Enabled = false;
        txtSerialNo.Enabled = false;
        ddlTransType.Enabled = false;
        lbSetNew.Visible = true;
        /*
        foreach (GridViewRow gvr in dgVoucherDtl.Rows)
        {

        }
        */
    }
    private void VEUnauth()
    {
        txtVchSysNo.Enabled = true;
        txtValueDate.Enabled = true;
        txtVchCode.Enabled = true;
        txtRefFileNo.Enabled = true;
        txtVchRefNo.Enabled = true;
        ddlFinMon.Enabled = true;
        txtPayee.Enabled = true;
        txtCheckNo.Enabled = true;
        txtCheqAmnt.Enabled = true;
        txtCheqDate.Enabled = true;
        txtControlAmt.Enabled = true;
        txtMoneyRptDate.Enabled = true;
        txtMoneyRptNo.Enabled = true;
        txtParticulars.Enabled = true;
        dgVoucherDtl.Columns[0].Visible = true;
        if (int.Parse(Session["userlevel"].ToString()) > 1)
        {
            lbAuth.Visible = true;
        }
        else
        {
            lbAuth.Visible = false;
        }
        btnCheqPrint.Visible = true;
        txtVolumeNo.Enabled = true;
        txtSerialNo.Enabled = true;
        ddlTransType.Enabled = true;
    }
    protected void Autho_Click(object sender, EventArgs e)
    {
        if (txtVchSysNo.Text.Trim() == String.Empty)
        {
            //lblTranStatus.Visible = true;
            //lblTranStatus.ForeColor = System.Drawing.Color.Red;
            //lblTranStatus.Text = "Enter voucher no first!";
            ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Input Voucher No First!!');", true);
            txtVchSysNo.Focus();
            return;
        }
    }    
    protected void btnCheqPrint_Click(object sender, EventArgs e)
    {
        ShowFooterTotal();
    }
    public override void VerifyRenderingInServerForm(Control control)
    {

    }
    private static Phrase FormatPhrase(string value)
    {
        return new Phrase(value, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9));
    }

    private static Phrase FormatHeaderPhrase(string value)
    {
        return new Phrase(value, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD));
    }
    
}
