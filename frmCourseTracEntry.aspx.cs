using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using KHSC;
using System.Data.SqlClient;

public partial class frmCourseTracEntry : System.Web.UI.Page
{
    CourseTracManager CourseTracManager = new CourseTracManager();
    clsCourseTrac CourseTrac = new clsCourseTrac();
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

    protected void dgCourseTrac_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = CourseTracManager.GetCourseTracDetailsInfo(dgCourseTrac.SelectedRow.Cells[1].Text);
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                lblID.Text = row["id"].ToString();
                txtColurseTracId.Text = row["TracId"].ToString();
                txtCourseTracName.Text = row["TracName"].ToString();
                btnSave.Visible = false;
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
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            
            //txtCollegeId.Text = CourseTrac.GetAutoId();
            CourseTrac.CourseTracId = txtColurseTracId.Text.Trim();
            CourseTrac.CourseTraceName = txtCourseTracName.Text.Trim();
            CourseTracManager.SaveCourseTracInformtion(CourseTrac);
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
    protected void CloseButton_Click(object sender, EventArgs e)
    {
        RefreshAll();
    }
    protected void UpdateButton_Click(object sender, EventArgs e)
    {
        try
        {            
            CourseTrac.CourseTracId = txtColurseTracId.Text.Trim();
            CourseTrac.CourseTraceName = txtCourseTracName.Text.Trim();
            CourseTrac.ID = lblID.Text;
            CourseTracManager.UpdateCoursetracInformtion(CourseTrac);
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
    protected void DeleteButton_Click(object sender, EventArgs e)
    {
        try
        {          
            CourseTrac.CourseTracId = txtColurseTracId.Text.Trim();          
            CourseTracManager.DeleteCourseTracInformtion(CourseTrac);
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
    private void RefreshAll()
    {
        txtColurseTracId.Text = "";
        txtCourseTracName.Text = "";
        dgCourseTrac.DataSource = CourseTracManager.GetCourseTracDetailsInfo("");
        dgCourseTrac.DataBind();
        //txtColurseTracId.Text = CourseTracManager.GetAutoId();
        UpdateButton.Visible = false;
        DeleteButton.Visible = false;
        btnSave.Visible = true;
        txtCourseTracName.Focus();
    }
    protected void dgCourseTrac_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].Attributes.Add("style", "display:none");         
            }
            else if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[1].Attributes.Add("style", "display:none");               
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[1].Attributes.Add("style", "display:none");              
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
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
}