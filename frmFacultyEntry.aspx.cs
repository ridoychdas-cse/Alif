using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using KHSC;
using System.Data.SqlClient;

public partial class frmFacultyEntry : System.Web.UI.Page
{
    FacultyManager aFacultyManager = new FacultyManager();    
    private static Permis per;
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["user"] == null)
        {
            if (Session.SessionID != "" | Session.SessionID != null)
            {
                clsSession ses = clsSessionManager.getSession(Session.SessionID);
                if (ses != null)
                {
                    Session["user"] = ses.UserId; Session["wnote"] = UsersManager.getUserName(ses.UserId);
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
                        }
                        Session["wnote"] = wnot;
                        Session["USER_ID"] = dReader["ID"].ToString();
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
        if (!IsPostBack)
        {
            RefreshAll();

        }
    }

    protected void DeleteButton_Click(object sender, EventArgs e)
    {
        try
        {
            clsFaculty aFaculty = new clsFaculty();
           
            aFaculty.LoginBy = Session["USER_ID"].ToString();
            aFaculty.ID = lblID.Text;
            aFacultyManager.DeleteFacultyInformation(aFaculty);
            RefreshAll();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ale", "alert('Record(s) delete suceessfullly...!!');", true);
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
    protected void CloseButton_Click(object sender, EventArgs e)
    {
        RefreshAll();
        ClearBoxes();
    }
    private void RefreshAll()
    {
     
        Session["Faculty"] = null;

        RefreshDropList();

        DataTable dt = aFacultyManager.GetFacultyInformationAll("");
        dgFaculty.DataSource = dt;
        Session["Faculty"] = dt;
        dgFaculty.DataBind();

        //txtFacultyId.Text = aFacultyManager.GetDepartmentAutoId();
        UpdateButton.Visible = false;
        DeleteButton.Visible = false;
        SaveButton.Visible = true;
        //UpdatePanelMST.Update();
        //UpdatePanelGride.Update();
        txtFacultyName.Focus();
    }

    private void RefreshDropList()
    {
        ddlCourseName.DataSource = aFacultyManager.GetCourseInfo("");
        ddlCourseName.DataTextField = "CourseName";
        ddlCourseName.DataValueField = "ID";
        ddlCourseName.DataBind();
        ddlCourseName.Items.Insert(0, "");

        ddlDesignation.DataSource = FacultyManager.GetDesignation("");
        ddlDesignation.DataTextField = "desig_name";
        ddlDesignation.DataValueField = "ID";
        ddlDesignation.DataBind();
        ddlDesignation.Items.Insert(0, "");
    }

    private void ClearBoxes()
    {
        txtFacultyId.Text = "";
        txtFacultyName.Text = "";
        ddlStatus.SelectedIndex = -1;
        ddlFacultyType.SelectedIndex = -1;
        ddlDesignation.SelectedIndex = -1;
        ddlCourseName.SelectedIndex = -1;
    }
    protected void SaveButton_Click(object sender, EventArgs e)
    {
        try
        {
            clsFaculty aFaculty = new clsFaculty();
            txtFacultyId.Text = aFacultyManager.GetDepartmentAutoId();
            aFaculty.FacultyId = txtFacultyId.Text.Trim();
            aFaculty.FacultyName = txtFacultyName.Text.Trim();
            aFaculty.CourseName = ddlCourseName.SelectedValue;
            aFaculty.Type = ddlFacultyType.SelectedValue;
            aFaculty.Status = ddlStatus.SelectedValue;
            aFaculty.Desognation = ddlDesignation.SelectedValue;
            aFaculty.LoginBy = Session["USER_ID"].ToString();
            FacultyManager.SaveFacultyInformation(aFaculty);
            RefreshAll();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ale", "alert('Record(s) is/are created suceessfullly...!!');", true);
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
    protected void UpdateButton_Click(object sender, EventArgs e)
    {
        try
        {
            clsFaculty aFaculty = new clsFaculty();
            txtFacultyId.Text = aFacultyManager.GetDepartmentAutoId();
            aFaculty.FacultyId = txtFacultyId.Text.Trim();
            aFaculty.FacultyName = txtFacultyName.Text.Trim();
            aFaculty.CourseName = ddlCourseName.SelectedValue;
            aFaculty.Type = ddlFacultyType.SelectedValue;
            aFaculty.Status = ddlStatus.SelectedValue;
            aFaculty.Desognation = ddlDesignation.SelectedValue;
            aFaculty.LoginBy = Session["USER_ID"].ToString();
            aFaculty.ID = lblID.Text;
            FacultyManager.UpdateFacultyInformation(aFaculty);
            RefreshAll();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ale", "alert('Record(s) update suceessfullly...!!');", true);
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



    protected void dgFaculty_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dgFaculty.DataSource = Session["Faculty"];
        dgFaculty.PageIndex = e.NewPageIndex;
        dgFaculty.DataBind();
    }
    protected void dgFaculty_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].Attributes.Add("style", "display:none");
                //e.Row.Cells[3].Attributes.Add("style", "display:none");
            }
            else if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[1].Attributes.Add("style", "display:none");
                //e.Row.Cells[3].Attributes.Add("style", "display:none");
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[1].Attributes.Add("style", "display:none");
                //e.Row.Cells[3].Attributes.Add("style", "display:none");
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[1].Attributes.Add("style", "display:none");
                //e.Row.Cells[3].Attributes.Add("style", "display:none");
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
    protected void dgFaculty_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = aFacultyManager.GetFacultyInformationAll(dgFaculty.SelectedRow.Cells[1].Text);
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                lblID.Text = row["ID"].ToString();
                txtFacultyId.Text = row["FacultyId"].ToString();
                txtFacultyName.Text = row["FacultyName"].ToString(); 
                ddlCourseName.SelectedValue = row["CourseID"].ToString();
                ddlDesignation.SelectedValue = row["DesignationID"].ToString();
                ddlFacultyType.SelectedValue = row["Type"].ToString();
                ddlStatus.SelectedValue = row["Status"].ToString();
                SaveButton.Visible = false;
                UpdateButton.Visible = true;
                DeleteButton.Visible = true;
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
    
}