using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using KHSC;
using System.Data.SqlClient;
using autouniv;

public partial class frmCourseAndSheduleAssiagn : System.Web.UI.Page
{
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
                    cmd.CommandText = "Select ID,user_grp,description from utl_userinfo where upper(user_name)=upper('" + Session["user"].ToString().ToUpper() + "') and status='A'";
                    conn.Open();
                    dReader = cmd.ExecuteReader();
                    string wnot = "";
                    if (dReader.HasRows == true)
                    {
                        while (dReader.Read())
                        {
                            Session["userlevel"] = int.Parse(dReader["user_grp"].ToString());
                            wnot = dReader["description"].ToString();
                            Session["userID"] = dReader["ID"].ToString();
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
        if (!IsPostBack)
        {
            Refresh();            
            //lblDepartment.Text = Request.QueryString["DepartmentName"];
            //lblFaculty.Text = Request.QueryString["Faculty"];
            //lblSubjectName.Text = Request.QueryString["SubjectName"];
            //lblYear.Text = Request.QueryString["StudyYear"];
            ////lblID.Text = Request.QueryString["ID"].Trim().Replace(" ","");
            //lblMstId.Text = Request.QueryString["OfferMstID"];
            //lblFlag.Text = Request.QueryString["Flag"];
            //lblUserID.Text = Request.QueryString["UserID"];
            //DateTime dtt = DateTime.Now;
            //DataTable dtt = clsSubOfferManager.GetShowAllSubjectSedule(lblMstId.Text);
            if (lblFlag.Text == "O")
            {
                if (Session["ShList"] != null)
                {
                    DataTable dtt = (DataTable)Session["ShList"];
                    DataRow[] rows = dtt.Select("ID ='" + lblID.Text + "'");
                    if (rows.Length <= 0)
                    {
                        DataTable dt = clsSubOfferManager.GetShowSheduleInSubject(txtBatchNo.Text, txtBatchNo.Text);
                        if (dt.Rows.Count > 0)
                        {
                            Session["ShList"] = dt;
                            ViewState["Flag"] = 1;
                        }
                    }
                }
                else
                {
                    DataTable dt = clsSubOfferManager.GetShowSheduleInSubject(txtBatchNo.Text, txtBatchNo.Text);
                    if (dt.Rows.Count > 0)
                    {
                        Session["ShList"] = dt;
                        ViewState["Flag"] = 1;
                    }
                }
            }

            if (Session["ShList"] != null)
            {
                DataTable dt = (DataTable)Session["ShList"];
                if (dt.Rows.Count > 0)
                {
                    DataRow[] rows = dt.Select("ID ='" + lblID.Text + "'");
                    if (rows.Length > 0)
                    {
                        DataTable dtResult = dt.Select("ID='" + lblID.Text + "'").CopyToDataTable();
                        if (dtResult.Rows.Count > 0)
                        {
                            ViewState["itemdtl"] = dtResult;
                            DataRow dr = dtResult.NewRow();
                            dtResult.Rows.Add(dr);
                            dgSubSedule.DataSource = dtResult;
                            dgSubSedule.DataBind();
                            ViewState["Flag"] = 1;
                        }
                    }
                    else
                    {
                        getEmptyDtl();
                        ViewState["Flag"] = "0";
                    }
                }
                else
                {
                    getEmptyDtl();
                }
            }
            else
            {
                getEmptyDtl();
                ViewState["Flag"] = "0";
            }
        }

        
    }
    private void Refresh()
    {
        DataTable dt = CourseRegManager.GetClassDetailsInfo();
        ddlCourseTrac.DataSource = dt;
        ddlCourseTrac.DataValueField = "id";
        ddlCourseTrac.DataTextField = "TracName";
        ddlCourseTrac.DataBind();
        ddlCourseTrac.Items.Insert(0, new System.Web.UI.WebControls.ListItem(""));

        DataTable dts = CourseRegManager.GetAllDaysInfor("");
        //ddlDays.DataSource = dts;
        //ddlDays.DataValueField = "id";
        //ddlDays.DataTextField = "TracName";
        //ddlDays.DataBind();
        //ddlDays.Items.Insert(0, new System.Web.UI.WebControls.ListItem(""));
        DataTable dtst = StudentManager.GetAllStudentSearch("", "", "", "");
        HistoryGridView.DataSource = dtst;
        HistoryGridView.DataBind();
        dgSubSedule.DataSource = null;
        dgSubSedule.DataBind();

        hfID.Value = "";

      
    }
    private void getEmptyDtl()
    {
        DataTable dtDtlGrid = new DataTable();
        dtDtlGrid.Columns.Add("ID", typeof(string));
        dtDtlGrid.Columns.Add("DaysID", typeof(string));
        dtDtlGrid.Columns.Add("StartTime", typeof(string));
        dtDtlGrid.Columns.Add("StartAmPm", typeof(string));
        dtDtlGrid.Columns.Add("EndtTime", typeof(string));
        dtDtlGrid.Columns.Add("EndAmPm", typeof(string));
        dtDtlGrid.Columns.Add("RoomNo", typeof(string));
        DataRow dr = dtDtlGrid.NewRow();
        dr["StartAmPm"] = "AM";
        dr["EndAmPm"] = "AM";
        dtDtlGrid.Rows.Add(dr);
        dgSubSedule.DataSource = dtDtlGrid;
        ViewState["itemdtl"] = dtDtlGrid;
        dgSubSedule.DataBind();
    }
    public DataTable PopulateItem()
    {
        DataTable dtitm = CourseRegManager.GetAllDaysInfor("");
        DataRow dr = dtitm.NewRow();
        dtitm.Rows.InsertAt(dr, 0);
        return dtitm;
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        if (ViewState["Flag"] == "0")
        {
            getEmptyDtl();
        }
    }
    protected void dgSubSedule_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ((TextBox)e.Row.FindControl("txtStartTime")).Attributes.Add("onBlur", "formatTime('" + ((TextBox)e.Row.FindControl("txtStartTime")).ClientID + "')");
            ((TextBox)e.Row.FindControl("txtEndTime")).Attributes.Add("onBlur", "formatTime('" + ((TextBox)e.Row.FindControl("txtEndTime")).ClientID + "')");
        }
    }
    protected void ddlDays_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["itemdtl"] != null)
            {
                GridViewRow gvr = (GridViewRow)((DropDownList)sender).NamingContainer;
                DataTable dtdtl = (DataTable)ViewState["itemdtl"];
                DataRow dr = dtdtl.Rows[gvr.DataItemIndex];

                dr["ID"] = lblID.Text;
                dr["DaysID"] = ((DropDownList)gvr.FindControl("ddlDays")).SelectedValue;
                dr["StartTime"] = ((TextBox)gvr.FindControl("txtStartTime")).Text;
                dr["StartAmPm"] = ((RadioButtonList)gvr.FindControl("rbStartAmPm")).SelectedValue;
                dr["EndtTime"] = ((TextBox)gvr.FindControl("txtEndTime")).Text;
                dr["EndAmPm"] = ((RadioButtonList)gvr.FindControl("rbEndAmPm")).SelectedValue;
                dr["RoomNo"] = ((TextBox)gvr.FindControl("txtRoomNo")).Text;
                string found = "";
                foreach (DataRow drf in dtdtl.Rows)
                {
                    if (drf["DaysID"].ToString() == "" && drf["StartTime"].ToString() == "")
                    {
                        found = "Y";
                    }
                }
                if (found == "")
                {
                    DataRow dr1 = dtdtl.NewRow();
                    dr1["StartAmPm"] = "AM";//dr1["EndAmPm"] = "AM";
                    dtdtl.Rows.Add(dr1);
                }
                dgSubSedule.DataSource = dtdtl;
                dgSubSedule.DataBind();
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
    protected void rbStartAmPm_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["itemdtl"] != null)
            {
                GridViewRow gvr = (GridViewRow)((RadioButtonList)sender).NamingContainer;
                DataTable dtdtl = (DataTable)ViewState["itemdtl"];
                DataRow dr = dtdtl.Rows[gvr.DataItemIndex];
                dr["ID"] = lblID.Text;
                dr["DaysID"] = ((DropDownList)gvr.FindControl("ddlDays")).SelectedValue;
                if (string.IsNullOrEmpty(((DropDownList)gvr.FindControl("ddlDays")).SelectedItem.Text))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Select Days First....!!!!');", true);
                    ((RadioButtonList)gvr.FindControl("rbStartAmPm")).SelectedIndex = -1;
                    ((RadioButtonList)gvr.FindControl("rbEndAmPm")).SelectedIndex = -1;
                    return;
                }
                if (string.IsNullOrEmpty(((TextBox)gvr.FindControl("txtStartTime")).Text))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Select Start Time First....!!!!');", true);
                    ((RadioButtonList)gvr.FindControl("rbStartAmPm")).SelectedIndex = -1;
                    ((RadioButtonList)gvr.FindControl("rbEndAmPm")).SelectedIndex = -1;
                    return;
                }
                dr["StartTime"] = ((TextBox)gvr.FindControl("txtStartTime")).Text;
                dr["StartAmPm"] = ((RadioButtonList)gvr.FindControl("rbStartAmPm")).SelectedValue;
                dr["EndtTime"] = ((TextBox)gvr.FindControl("txtEndTime")).Text;
                dr["EndAmPm"] = ((RadioButtonList)gvr.FindControl("rbEndAmPm")).SelectedValue;
                dr["RoomNo"] = ((TextBox)gvr.FindControl("txtRoomNo")).Text;
                string found = "";
                foreach (DataRow drf in dtdtl.Rows)
                {
                    if (drf["DaysID"].ToString() == "" && drf["StartTime"].ToString() == "")
                    {
                        found = "Y";
                    }
                }
                if (found == "")
                {
                    DataRow dr1 = dtdtl.NewRow();
                    dtdtl.Rows.Add(dr1);
                }
                dgSubSedule.DataSource = dtdtl;
                dgSubSedule.DataBind();
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
    protected void rbEndAmPm_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["itemdtl"] != null)
            {
                GridViewRow gvr = (GridViewRow)((RadioButtonList)sender).NamingContainer;
                DataTable dtdtl = (DataTable)ViewState["itemdtl"];
                DataRow dr = dtdtl.Rows[gvr.DataItemIndex];
                dr["ID"] = lblID.Text;
                dr["DaysID"] = ((DropDownList)gvr.FindControl("ddlDays")).SelectedValue;
                dr["StartTime"] = ((TextBox)gvr.FindControl("txtStartTime")).Text;
                dr["StartAmPm"] = ((RadioButtonList)gvr.FindControl("rbStartAmPm")).SelectedValue;
                if (string.IsNullOrEmpty(((DropDownList)gvr.FindControl("ddlDays")).SelectedItem.Text))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Select Days First....!!!!');", true);
                    ((RadioButtonList)gvr.FindControl("rbEndAmPm")).SelectedIndex = -1;
                    return;
                }
                if (string.IsNullOrEmpty(((TextBox)gvr.FindControl("txtStartTime")).Text))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Select Start Time First....!!!!');", true);
                    ((RadioButtonList)gvr.FindControl("rbEndAmPm")).SelectedIndex = -1;
                    return;
                }
                if (string.IsNullOrEmpty(((TextBox)gvr.FindControl("txtEndTime")).Text))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Select End Time ....!!!!');", true);
                    ((RadioButtonList)gvr.FindControl("rbEndAmPm")).SelectedIndex = -1;
                    return;
                }

                dr["EndtTime"] = ((TextBox)gvr.FindControl("txtEndTime")).Text;
                dr["EndAmPm"] = ((RadioButtonList)gvr.FindControl("rbEndAmPm")).SelectedValue;
                dr["RoomNo"] = ((TextBox)gvr.FindControl("txtRoomNo")).Text;
                string found = "";
                foreach (DataRow drf in dtdtl.Rows)
                {
                    if (drf["DaysID"].ToString() == "" && drf["StartTime"].ToString() == "")
                    {
                        found = "Y";
                    }
                }
                if (found == "")
                {
                    DataRow dr1 = dtdtl.NewRow();
                    dtdtl.Rows.Add(dr1);
                }
                dgSubSedule.DataSource = dtdtl;
                dgSubSedule.DataBind();
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
    protected void txtRoomNo_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["itemdtl"] != null)
            {
                GridViewRow gvr = (GridViewRow)((TextBox)sender).NamingContainer;
                DataTable dtdtl = (DataTable)ViewState["itemdtl"];
                DataRow dr = dtdtl.Rows[gvr.DataItemIndex];
                dr["ID"] = lblID.Text;
                dr["DaysID"] = ((DropDownList)gvr.FindControl("ddlDays")).SelectedValue;
                dr["StartTime"] = ((TextBox)gvr.FindControl("txtStartTime")).Text;
                dr["StartAmPm"] = ((RadioButtonList)gvr.FindControl("rbStartAmPm")).SelectedValue;
                dr["EndtTime"] = ((TextBox)gvr.FindControl("txtEndTime")).Text;
                dr["EndAmPm"] = ((RadioButtonList)gvr.FindControl("rbEndAmPm")).SelectedValue;
                if (string.IsNullOrEmpty(((DropDownList)gvr.FindControl("ddlDays")).SelectedItem.Text))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Select Days First. .!!!!');", true);
                    return;
                }
                if (string.IsNullOrEmpty(((TextBox)gvr.FindControl("txtStartTime")).Text))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Select Start Time First. .!!!!');", true);
                    return;
                }
                if (string.IsNullOrEmpty(((TextBox)gvr.FindControl("txtEndTime")).Text))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Select End Time. .!!!!');", true);
                    return;
                }
                if (string.IsNullOrEmpty(((TextBox)gvr.FindControl("txtRoomNo")).Text))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Select Room No. .!!!!');", true);
                    return;
                }

                dr["RoomNo"] = ((TextBox)gvr.FindControl("txtRoomNo")).Text;
                string found = "";
                foreach (DataRow drf in dtdtl.Rows)
                {
                    if (drf["DaysID"].ToString() == "" && drf["StartTime"].ToString() == "")
                    {
                        found = "Y";
                    }
                }
                if (found == "")
                {
                    DataRow dr1 = dtdtl.NewRow();
                    dtdtl.Rows.Add(dr1);
                }
                dgSubSedule.DataSource = dtdtl;
                dgSubSedule.DataBind();
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
            if (!string.IsNullOrEmpty(hfID.Value))
            {
               
            }
            else {
                if (ddlCourseName.SelectedValue == "")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Please Select course name...!!');", true);
                    return;
                }
                if (string.IsNullOrEmpty(lblFacultyID.Text))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Input faculty Name...!!');", true);
                    return;
                }
                if (string.IsNullOrEmpty(txtBatchNo.Text))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Input batch no...!!');", true);
                    return;
                }

                if (string.IsNullOrEmpty(txtStartDate.Text))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Please Select start date...!!');", true);
                    return;
                }
                else
                {

                    clsCourseSheduleAssign sheduleObj = new clsCourseSheduleAssign();
                    sheduleObj.TracID = ddlCourseTrac.SelectedValue.Replace("'", "");
                    sheduleObj.CourseID = ddlCourseName.SelectedValue.Replace("'", "");
                    sheduleObj.FacultyID = lblFacultyID.Text.Replace("'", "");
                    sheduleObj.BatchNo = txtBatchNo.Text.Replace("'", "");
                    sheduleObj.StartDate = DataManager.DateEncode(txtStartDate.Text.Replace("'", "")).ToString();
                    sheduleObj.EndDate = DataManager.DateEncode(txtEndDate.Text.Replace("'", "")).ToString();
                    sheduleObj.Status = ddlStatus.SelectedValue.Replace("'", "");
                    sheduleObj.Year = txtYear.Text.Replace("'", "");
                    sheduleObj.LoginBy = Session["USER_ID"].ToString();

                    DataTable dt = (DataTable)ViewState["itemdtl"];
                    DataTable NewDt = new DataTable();
                    NewDt.Columns.Add("ID", typeof(string));
                    NewDt.Columns.Add("DaysID", typeof(string));
                    NewDt.Columns.Add("StartTime", typeof(string));
                    NewDt.Columns.Add("StartAmPm", typeof(string));
                    NewDt.Columns.Add("EndtTime", typeof(string));
                    NewDt.Columns.Add("EndAmPm", typeof(string));
                    NewDt.Columns.Add("RoomNo", typeof(string));
                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            DataTable dtResult = (DataTable)Session["ShList"];
                            if (dtResult != null)
                            {
                                //dtResult.AsEnumerable()
                                //     .Where(r => r.Field<string>("ID") == lblID.Text.Trim() || r.Field<string>("ID") == "")
                                //     .ToList()
                                //     .ForEach(row => row.Delete());

                                foreach (DataRow dr1 in dtResult.Rows)
                                {
                                    if (dr1["ID"].ToString().Trim() == lblID.Text.Trim() || dr1["ID"].ToString().Trim() == "")
                                    { }
                                    else
                                    { NewDt.Rows.Add(dr1["ID"].ToString(), dr1["DaysID"].ToString(), dr1["StartTime"].ToString(), dr1["StartAmPm"].ToString(), dr1["EndtTime"].ToString(), dr1["EndAmPm"].ToString(), dr1["RoomNo"].ToString()); }
                                }
                                foreach (DataRow dr in dt.Rows)
                                {
                                    if (dr["ID"].ToString() != "")
                                    {
                                        NewDt.Rows.Add(dr["ID"].ToString(), dr["DaysID"].ToString(), dr["StartTime"].ToString(), dr["StartAmPm"].ToString(), dr["EndtTime"].ToString(), dr["EndAmPm"].ToString(), dr["RoomNo"].ToString());
                                    }
                                }

                                Session["ShList"] = NewDt;
                            }
                            else
                            {
                                Session["ShList"] = dt;
                            }
                        }
                        CourseSheduleAssaignManager.SaveSheduleCourseBatch(dt, sheduleObj);
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ale", "alert('Record(s) is/are created suceessfullly...!!');", true);
                        Refresh();
                    }
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
    protected void ddlCourseName_SelectedIndexChanged(object sender, EventArgs e)
    {
       
    }
    protected void txtFAcultySearch_TextChanged(object sender, EventArgs e)
    {
        DataTable dt = CourseRegManager.GetFAcultyNAme(txtFAcultySearch.Text);
        txtFAcultySearch.Text = dt.Rows[0]["FacultyName"].ToString();
        lblFacultyID.Text = dt.Rows[0]["ID"].ToString();
        UP1.Update();
    }
    protected void ddlCourseTrac_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable dts = CourseRegManager.GetCourseDetailsInfo(ddlCourseTrac.SelectedValue);
        ddlCourseName.DataSource = dts;
        ddlCourseName.DataValueField = "id";
        ddlCourseName.DataTextField = "CourseName";
        ddlCourseName.DataBind();
        ddlCourseName.Items.Insert(0, new System.Web.UI.WebControls.ListItem(""));
    } 
    
    protected void HistoryGridView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[6].Attributes.Add("style", "display:none");
            e.Row.Cells[7].Attributes.Add("style", "display:none");
          
        }
        else if (e.Row.RowType == DataControlRowType.Header)
        {           
            e.Row.Cells[6].Attributes.Add("style", "display:none");
            e.Row.Cells[7].Attributes.Add("style", "display:none");
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {          
            e.Row.Cells[6].Attributes.Add("style", "display:none");
            e.Row.Cells[7].Attributes.Add("style", "display:none");
        }
    }
    protected void HistoryGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

    }
    protected void HistoryGridView_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {            
            DataTable dt = CourseRegManager.GetCourseSheduleInfo(HistoryGridView.SelectedRow.Cells[7].Text);
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                lblID.Text = row["ID"].ToString();
                ddlCourseTrac.SelectedValue = row["TracID"].ToString();
                ddlCourseName.SelectedValue = row["CourseID"].ToString();
                txtFAcultySearch.Text = row["FacultyName"].ToString();
                txtBatchNo.Text = row["BatchNo"].ToString();
                txtYear.Text = row["Year"].ToString();
                txtStartDate.Text = row["dttST"].ToString();
                txtEndDate.Text = row["dttEnd"].ToString();
                hfID.Value = row["ID"].ToString();                
            }
            DataTable dts = CourseRegManager.GetCourseDetailsOfCourseShedule(hfID.Value);
           // ViewState["SelectDtl"] = dts;
            //GridViewRow gvr = (GridViewRow)((DropDownList)sender).NamingContainer;
           // DataTable dtdtl = (DataTable)ViewState["SelectDtl"];
           // DataRow dr = dtdtl.Rows[gvr.DataItemIndex];
           // //dr["ID"] = lblID.Text;
           // ((DropDownList)gvr.FindControl("ddlDays")).SelectedValue= dr["Days"].ToString();
           //((TextBox)gvr.FindControl("txtStartTime")).Text = dr["sttime"].ToString();
           //((RadioButtonList)gvr.FindControl("rbStartAmPm")).SelectedValue=dr["StAPM"].ToString();
           //((TextBox)gvr.FindControl("txtEndTime")).Text = dr["endtime"].ToString();
           //((RadioButtonList)gvr.FindControl("rbEndAmPm")).SelectedValue = dr["EndAPM"].ToString();
           //((TextBox)gvr.FindControl("txtRoomNo")).Text = dr["RoomNo"].ToString();
            //var query = from c in dts.AsEnumerable() where c.Field<string>("ID") == ((DropDownList)gvr.FindControl("ddlDays")).SelectedValue.ToString() select s;
            //dgSubSedule.DataSource = query;
            //dgSubSedule.DataBind();
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