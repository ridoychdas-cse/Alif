using System;
using System.Collections;
using System.Configuration;
using System.Data;
//using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
//using System.Xml.Linq;
using System.Data.SqlClient;
using KHSC;
//using DBSC;


public partial class frmPaymentInfo : System.Web.UI.Page
{
    ClassManeger aClassManagerObj = new ClassManeger();
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
                            wnot = "DBSC Mr. " + dReader["description"].ToString();
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
            //ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('" + Session.SessionID + "');", true);
            string pageName = DataManager.GetCurrentPageName();
            string modid = PermisManager.getModuleId(pageName);
            per = PermisManager.getUsrPermis(Session["user"].ToString().Trim().ToUpper(), modid);
            if (per != null & per.AllowView == "Y")
            {
                ((Label)Page.Master.FindControl("lblLogin")).Text = Session["wnote"].ToString();
                ((LinkButton)Page.Master.FindControl("lbLogout")).Visible = true;
            }
            else
            {
                Response.Redirect("Home.aspx?sid=sam");
            }
        }
        catch
        {
            Response.Redirect("Default.aspx?sid=sam");
        }
        if (!Page.IsPostBack)
        {
            try
            {
                DataTable dt = clsPaymentInfoManager.GetPayments();
                if (dt.Rows.Count > 0)
                {
                    dgPay.DataSource = dt;
                    Session["Payment"] = dt;
                    dgPay.DataBind();
                }
                else
                {

                    getEmptyGrid();
                }
                RefreshPayHead();
                ddlClass.DataSource = aClassManagerObj.GetClassInfo("");
                ddlClass.DataTextField = "class_name";
                ddlClass.DataValueField = "class_id";
                ddlClass.DataBind();
                ddlClass.Items.Insert(0,"");
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
        DataTable dt = new DataTable();
        dt.Columns.Add("pay_id", typeof(string));
        dt.Columns.Add("pay_head_id", typeof(string));
        dt.Columns.Add("pay_Name_Id", typeof(string));
        dt.Columns.Add("pay_type", typeof(string));
        dt.Columns.Add("pay_class", typeof(string));
        dt.Columns.Add("class", typeof(string));
        dt.Columns.Add("pay_class_id", typeof(string));
        dt.Columns.Add("version", typeof(string));
        dt.Columns.Add("version_name", typeof(string));
        dt.Columns.Add("GroupID", typeof(string));
        dt.Columns.Add("GroupName", typeof(string));
        dt.Columns.Add("for_all_std", typeof(string));
        dt.Columns.Add("pay_amt", typeof(string));
        dt.Columns.Add("discount", typeof(string));
        DataRow dr = dt.NewRow();
        dt.Rows.Add(dr);
        dgPay.DataSource = dt;
        dgPay.DataBind();
        dgPay.ShowFooter = true;
        dgPay.FooterRow.Visible = true;
        ((TextBox)dgPay.FooterRow.FindControl("txtPayId")).Text = IdManager.GetNextID("payment_info","pay_id").ToString();

        string queryDist = "select '' class_id, 'Select Class' class_name  union select class_id,class_name from class_info order by 1";
        util.PopulationDropDownList(((DropDownList)dgPay.FooterRow.FindControl("ddlPayClass")), "Class", queryDist, "class_name", "class_id");
        ((DropDownList)dgPay.FooterRow.FindControl("ddlVersion")).Items.Clear();

        //string queryVersion = "select '' id, 'Select Version' version_name  union select id,version_name from version_info order by 1";
        //util.PopulationDropDownList(((DropDownList)dgPay.FooterRow.FindControl("ddlVersion")), "Verson", queryVersion, "version_name", "id");

        string queryHead = "select '' id, 'Select Head' Head_Name  union select id,Head_Name from payment_head order by 1";
        util.PopulationDropDownList(((DropDownList)dgPay.FooterRow.FindControl("ddlPayHead")), "PayHead", queryHead, "Head_Name", "id");
    }
    protected void dgPay_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)Session["Payment"];
            if (dt.Rows.Count > 0)
            {
                dgPay.DataSource = dt;
                dgPay.EditIndex = -1;
                dgPay.DataBind();
                dgPay.ShowFooter = false;
                dgPay.FooterRow.Visible = false;
            }
            else
            {
                getEmptyGrid();
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
    protected void dgPay_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "AddNew")
            {
                //DataTable dt = clsPaymentInfoManager.GetPayments();
                DataTable dt = (DataTable)Session["Payment"];
                dgPay.DataSource = dt;
                dgPay.DataBind();
                dgPay.ShowFooter = true;
                dgPay.FooterRow.Visible = true;
                ((TextBox)dgPay.FooterRow.FindControl("txtPayId")).Text = IdManager.GetNextID("payment_info", "Pay_id").ToString();
                //((TextBox)dgPay.FooterRow.FindControl("txtPayName")).Focus();
                ((TextBox)dgPay.FooterRow.FindControl("txtPayAmt")).Text = "0";
                ((DropDownList)dgPay.FooterRow.FindControl("ddlPayClass")).Items.Clear();

                string queryDist = "select class_id,class_name from class_info order by 1";
                util.PopulationDropDownList(((DropDownList)dgPay.FooterRow.FindControl("ddlPayClass")), "Class", queryDist, "class_name", "class_id");
                ((DropDownList)dgPay.FooterRow.FindControl("ddlPayClass")).Items.Insert(0, "");

                ((DropDownList)dgPay.FooterRow.FindControl("ddlVersion")).Items.Clear();
                //string queryVersion = "select '' id, 'Select Version' version_name  union select id,version_name from version_info order by 1";
                //util.PopulationDropDownList(((DropDownList)dgPay.FooterRow.FindControl("ddlVersion")), "Verson", queryVersion, "version_name", "id");
                string queryHead = "select '' id, 'Select Head' Head_Name  union select id,Head_Name from payment_head order by 1";
                util.PopulationDropDownList(((DropDownList)dgPay.FooterRow.FindControl("ddlPayHead")), "PayHead", queryHead, "Head_Name", "id");
            }
            else if (e.CommandName == "Insert")
            {
                clsPaymentInfo pay = new clsPaymentInfo();
                pay.PayId = ((TextBox)dgPay.FooterRow.FindControl("txtPayId")).Text.Trim();
                pay.PayName = ((DropDownList)dgPay.FooterRow.FindControl("ddlPayHead")).SelectedValue.Trim();
                pay.PayType = ((DropDownList)dgPay.FooterRow.FindControl("ddlPayType")).SelectedValue.Trim();
                pay.PayClass = ((DropDownList)dgPay.FooterRow.FindControl("ddlPayClass")).SelectedValue.Trim();
                pay.ForAllStd = ((DropDownList)dgPay.FooterRow.FindControl("ddlForAllStd")).SelectedValue.Trim();
                pay.PayAmt = ((TextBox)dgPay.FooterRow.FindControl("txtPayAmt")).Text.Trim();
                pay.Discount = ((DropDownList)dgPay.FooterRow.FindControl("ddlDiscount")).SelectedValue.Trim();
                pay.Version = ((DropDownList)dgPay.FooterRow.FindControl("ddlVersion")).SelectedValue.Trim();
                pay.GroupID = ((DropDownList)dgPay.FooterRow.FindControl("ddlGroup")).SelectedValue.Trim();
                if (per.AllowAdd == "Y")
                {
                    clsPaymentInfoManager.CreatePayment(pay);
                }
                //DataTable dt = clsPaymentInfoManager.GetPayments();
                DataTable dt = (DataTable)Session["Payment"];
                if (dt.Rows.Count > 0)
                {
                    dgPay.DataSource = dt;
                    dgPay.EditIndex = -1;
                    dgPay.DataBind();
                    dgPay.FooterRow.Visible = false;
                    dgPay.ShowFooter = false;
                }
                else
                {
                    getEmptyGrid();
                }
                ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Record created successfully!!');", true);
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
            if (per.AllowDelete == "Y")
            {
                clsPaymentInfoManager.DeletePayment(((Label)dgPay.Rows[e.RowIndex].FindControl("lblPayId")).Text);
                DataTable dt =(DataTable)Session["Payment"];
                if (dt.Rows.Count > 0)
                {
                    dgPay.DataSource = dt;
                    dgPay.EditIndex = -1;
                    dgPay.DataBind();
                    dgPay.ShowFooter = false;
                    dgPay.FooterRow.Visible = false;
                }
                else
                {
                    getEmptyGrid();
                }
                ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Record deleted successfully!!');", true);
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
    protected void dgPay_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            string paytyp = ((Label)dgPay.Rows[e.NewEditIndex].FindControl("lblPayType")).Text;
            string paycls = ((Label)dgPay.Rows[e.NewEditIndex].FindControl("lblPayClass")).Text;
            string forall = ((Label)dgPay.Rows[e.NewEditIndex].FindControl("lblForAllStd")).Text;
            string discount = ((Label)dgPay.Rows[e.NewEditIndex].FindControl("lblDiscount")).Text;
            string Version = ((Label)dgPay.Rows[e.NewEditIndex].FindControl("lblVersion")).Text;
            string GroupID = ((Label)dgPay.Rows[e.NewEditIndex].FindControl("lblGroupID")).Text;
            string PayId = ((Label)dgPay.Rows[e.NewEditIndex].FindControl("lblPayNameID")).Text;
            DataTable dt = (DataTable)Session["Payment"];
            dgPay.DataSource = dt;
            dgPay.EditIndex = e.NewEditIndex;
            dgPay.DataBind();
            ((DropDownList)dgPay.Rows[e.NewEditIndex].FindControl("ddlPayType")).SelectedIndex = ((DropDownList)dgPay.Rows[e.NewEditIndex].FindControl("ddlPayType")).Items.IndexOf(((DropDownList)dgPay.Rows[e.NewEditIndex].FindControl("ddlPayType")).Items.FindByValue(paytyp));
            ((DropDownList)dgPay.Rows[e.NewEditIndex].FindControl("ddlPayClass")).Items.Clear();
            string queryDist = "select class_id,class_name from class_info order by 1";
            util.PopulationDropDownList(((DropDownList)dgPay.Rows[e.NewEditIndex].FindControl("ddlPayClass")), "Class", queryDist, "class_name", "class_id");
            ((DropDownList)dgPay.Rows[e.NewEditIndex].FindControl("ddlPayClass")).Items.Insert(0,"");

            ((DropDownList)dgPay.Rows[e.NewEditIndex].FindControl("ddlPayClass")).SelectedIndex = ((DropDownList)dgPay.Rows[e.NewEditIndex].FindControl("ddlPayClass")).Items.IndexOf(((DropDownList)dgPay.Rows[e.NewEditIndex].FindControl("ddlPayClass")).Items.FindByValue(paycls));
            ((DropDownList)dgPay.Rows[e.NewEditIndex].FindControl("ddlForAllStd")).SelectedValue = forall;
            ((DropDownList)dgPay.Rows[e.NewEditIndex].FindControl("ddlDiscount")).SelectedValue = discount;

            ((DropDownList)dgPay.FooterRow.FindControl("ddlVersion")).Items.Clear();
            string queryVersion = "select id,version_name from version_info where class_id='" + ((DropDownList)dgPay.Rows[e.NewEditIndex].FindControl("ddlPayClass")).SelectedValue + "' order by 1";
            util.PopulationDropDownList(((DropDownList)dgPay.Rows[e.NewEditIndex].FindControl("ddlVersion")), "Version", queryVersion, "version_name", "id");
            ((DropDownList)dgPay.Rows[e.NewEditIndex].FindControl("ddlVersion")).Items.Insert(0, "");
             ((DropDownList)dgPay.Rows[e.NewEditIndex].FindControl("ddlVersion")).SelectedIndex = ((DropDownList)dgPay.Rows[e.NewEditIndex].FindControl("ddlVersion")).Items.IndexOf(((DropDownList)dgPay.Rows[e.NewEditIndex].FindControl("ddlVersion")).Items.FindByValue(Version));

             string queryHead = "select '' id, 'Select Head' Head_Name  union select id,Head_Name from payment_head order by 1";
             util.PopulationDropDownList(((DropDownList)dgPay.Rows[e.NewEditIndex].FindControl("ddlPayHead")), "PayHead", queryHead, "Head_Name", "id");
             ((DropDownList)dgPay.Rows[e.NewEditIndex].FindControl("ddlPayHead")).SelectedValue = PayId;
             ((DropDownList)dgPay.Rows[e.NewEditIndex].FindControl("ddlGroup")).SelectedValue = GroupID;
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
    protected void dgPay_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            if (per.AllowEdit == "Y")
            {
                clsPaymentInfo pay = clsPaymentInfoManager.getPayment(((TextBox)dgPay.Rows[e.RowIndex].FindControl("txtPayId")).Text);
                if (pay != null)
                {
                    pay.PayName = ((DropDownList)dgPay.Rows[e.RowIndex].FindControl("ddlPayHead")).SelectedValue.Trim();
                    pay.PayType = ((DropDownList)dgPay.Rows[e.RowIndex].FindControl("ddlPayType")).SelectedValue.Trim();
                    pay.ForAllStd = ((DropDownList)dgPay.Rows[e.RowIndex].FindControl("ddlForAllStd")).SelectedValue.Trim();
                    pay.PayClass = ((DropDownList)dgPay.Rows[e.RowIndex].FindControl("ddlPayClass")).SelectedValue.Trim();
                    pay.Discount = ((DropDownList)dgPay.Rows[e.RowIndex].FindControl("ddlDiscount")).SelectedValue.Trim();
                    pay.PayAmt = ((TextBox)dgPay.Rows[e.RowIndex].FindControl("txtPayAmt")).Text;
                    pay.Version = ((DropDownList)dgPay.Rows[e.RowIndex].FindControl("ddlVersion")).SelectedValue.Trim();
                    pay.GroupID = ((DropDownList)dgPay.Rows[e.RowIndex].FindControl("ddlGroup")).SelectedValue.Trim();
                    clsPaymentInfoManager.UpdatePayment(pay);
                }
                DataTable dt = (DataTable)Session["Payment"];
                if (dt.Rows.Count > 0)
                {
                    dgPay.DataSource = dt;
                    dgPay.EditIndex = -1;
                    dgPay.DataBind();
                }
                else
                {
                    getEmptyGrid();
                }
                ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Record udpated successfully!!');", true);
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
    protected void dgPay_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (((DataRowView)e.Row.DataItem)[1].ToString() == String.Empty)
                {
                    e.Row.Visible = false;
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
    protected void dgPay_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            dgPay.PageIndex = e.NewPageIndex;
            DataTable dt = (DataTable)Session["Payment"];
            dgPay.DataSource = dt;
            dgPay.DataBind();
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
    protected void ddlClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow gvr = (GridViewRow)((DropDownList)sender).NamingContainer;
        ((DropDownList)gvr.FindControl("ddlVersion")).Items.Clear();
        string queryVersion = "select id,version_name from version_info where class_id='" + ((DropDownList)gvr.FindControl("ddlPayClass")).SelectedValue + "' order by 1";
        util.PopulationDropDownList(((DropDownList)gvr.FindControl("ddlVersion")), "Class", queryVersion, "version_name", "id");
        ((DropDownList)gvr.FindControl("ddlVersion")).Items.Insert(0,"");
    }


    //******************************************** Pay Head ********************************//
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            PayHead aPayHead = new PayHead();
            aPayHead.ID = txtId.Text.Trim();
            aPayHead.PayHeadName = txtHeadName.Text.Trim();

            PayHeadManager.SavePayHeadSetting(aPayHead);
            RefreshPayHead();
            ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Record are/is saved sucessfully....!!!');", true);
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
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            PayHead aPayHead = new PayHead();
            aPayHead.ID = txtId.Text.Trim();
            aPayHead.PayHeadName = txtHeadName.Text.Trim();

            PayHeadManager.UpdatePayHeadSetting(aPayHead);
            RefreshPayHead();
            ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Record are/is update sucessfully....!!!');", true);
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
            PayHead aPayHead = new PayHead();
            aPayHead.ID = txtId.Text.Trim();
            aPayHead.PayHeadName = txtHeadName.Text.Trim();

            PayHeadManager.DeletePayHeadSetting(aPayHead);
            RefreshPayHead();
            ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Record are/is delete sucessfully....!!!');", true);
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
    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            GridView1.PageIndex = e.NewPageIndex;
            GridView1.DataSource = PayHeadManager.GetShowPayheadInfo();
            GridView1.DataBind();
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
            RefreshPayHead();
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
    private void RefreshPayHead()
    {
        GridView1.DataSource = PayHeadManager.GetShowPayheadInfo();
        GridView1.DataBind();
        btnSave.Visible = GridView1.Visible = true;
        btnUpdate.Visible =btnDelete.Visible= false;
        txtId.Text = txtHeadName.Text = "";
        txtHeadName.Focus();
    }  
    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtId.Text = GridView1.SelectedRow.Cells[1].Text;
        txtHeadName.Text = GridView1.SelectedRow.Cells[2].Text;
        btnSave.Visible =GridView1.Visible= false;
        btnUpdate.Visible = btnDelete.Visible = true;
    }
    protected void ddlClass_SelectedIndexChanged1(object sender, EventArgs e)
    {
        ddlVersion.DataSource = ClsVersionManager.GetVersionDetailsInformation(ddlClass.SelectedValue);
        ddlVersion.DataTextField = "version_name";
        ddlVersion.DataValueField = "id";
        ddlVersion.DataBind();
        ddlVersion.Items.Insert(0, "");

        UpdatePanel2.Update();
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (ddlClass.SelectedItem.Text != "")
        {
            DataTable dt = clsPaymentInfoManager.GetPaymentsOnSearchClassAndVerson(ddlClass.SelectedValue, ddlVersion.SelectedValue);
            Session["Payment"] = dt;
            dgPay.DataSource = dt;
            dgPay.DataBind();
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Select Class First....!!');", true);
        }
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        //DataTable dt = clsPaymentInfoManager.GetPayments();
        //Session["Payment"] = dt;
        //dgPay.DataSource = dt;
        //dgPay.DataBind();
        //ddlClass.SelectedIndex = -1;
        //ddlVersion.SelectedIndex = -1;
        //UpdatePanel1.Update();
        //UpdatePanel2.Update();
        Response.Redirect("frmPaymentInfo.aspx?mno=0.1");
        
    }
}
