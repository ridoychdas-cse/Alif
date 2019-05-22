using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using KHSC;


public partial class UserInfo : System.Web.UI.Page
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
                            wnot = "KHSC Mr. " + dReader["description"].ToString();
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
            //string queryBank = "select '' BANK_ID, '' BANK_NAME  union select BANK_ID,BANK_NAME from BANK_INFO order by 1";
            //util.PopulationDropDownList(ddlBankNo, "Bank", queryBank, "BANK_NAME", "BANK_ID"); 
        }
    }
    private void clearFields()
    {
        txtUserId.Text = "";
        txtPassword.Text = "";
        txtDescription.Text = "";
        ddlUsrGrp.SelectedIndex = -1;
        ddlStatus.SelectedIndex = -1;
        txtEmpNo.Text = "";
        lblTranStatus.Visible = false;
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        clearFields();

    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (txtUserId.Text.ToString().Trim() != String.Empty)
        {
            Users usr = UsersManager.getUser(txtUserId.Text.ToString().ToUpper());
            if (usr != null)
            {
                usr.Description = txtDescription.Text;
                usr.Password = txtPassword.Text;
                usr.UserGrp = ddlUsrGrp.SelectedValue;
                usr.Status = ddlStatus.SelectedValue;
                usr.EmpNo = txtEmpNo.Text;
                UsersManager.UpdateUser(usr);
                lblTranStatus.Visible = true;
                lblTranStatus.Text = "User updated successfully!!";
                lblTranStatus.ForeColor = System.Drawing.Color.Green;
            }
            else
            {
                usr = new Users();
                usr.UserName = txtUserId.Text;
                usr.Description = txtDescription.Text;
                usr.Password = txtPassword.Text;
                usr.UserGrp = ddlUsrGrp.SelectedValue;
                usr.Status = ddlStatus.SelectedValue;
                usr.EmpNo = txtEmpNo.Text;
                UsersManager.CreateUser(usr);
                lblTranStatus.Visible = true;
                lblTranStatus.Text = "User created successfully!!";
                lblTranStatus.ForeColor = System.Drawing.Color.Green;
            }
        }
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        if (txtUserId.Text.ToString().Trim() != String.Empty)
        {
            Users usr = UsersManager.getUser(txtUserId.Text.ToString().ToUpper());
            if (usr != null)
            {
                UsersManager.DeleteUser(usr);
                lblTranStatus.Visible = true;
                lblTranStatus.Text = "User deleted successfully!!";
                lblTranStatus.ForeColor = System.Drawing.Color.Green;
            }
        }
    }
    protected void btnFind_Click(object sender, EventArgs e)
    {
        if (txtUserId.Text.ToString().Trim() != String.Empty)
        {
            Users usr = UsersManager.getUser(txtUserId.Text.ToString().ToUpper());
            if (usr != null)
            {
                txtUserId.Text = usr.UserName;
                txtDescription.Text = usr.Description;
                txtPassword.Text = usr.Password;
                ddlUsrGrp.SelectedValue = usr.UserGrp;
                ddlStatus.SelectedValue = usr.Status;
                txtEmpNo.Text = usr.EmpNo;
            }
            else
            {
                lblTranStatus.Visible = true;
                lblTranStatus.Text = "No such user exists!!";
                lblTranStatus.ForeColor = System.Drawing.Color.Red;
            }
        }
    }
}
