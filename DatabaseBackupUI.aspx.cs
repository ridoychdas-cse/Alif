using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using KHSC;
using System.Data;
using System.Configuration;

public partial class DatabaseBackupUI : System.Web.UI.Page
{
    public static Permis per;
    SqlConnection con = new SqlConnection(DataManager.OraConnString());
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
                
            }
            else
            {
                Response.Redirect("Default.aspx?sid=sam");
            }
        }
        catch
        {
            //ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('"+ex.Message+"!!');", true);
            Response.Redirect("Default.aspx?sid=sam");
        }
        if (!IsPostBack)
        {

        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        try
        {
            SqlConnection con = new SqlConnection(DataManager.OraConnString());
            SqlCommand sqlcmd = new SqlCommand();
            string Drive = ConfigurationManager.AppSettings["BackupDriver"];
            string backupDIR = "" + Drive + ":\\BackUpDataBase";
            if (!System.IO.Directory.Exists(backupDIR))
            {
                System.IO.Directory.CreateDirectory(backupDIR);
            }

            con.Open();
            sqlcmd.CommandTimeout = 500;
            sqlcmd = new SqlCommand("backup database " + con.Database + " to disk='" + backupDIR + "\\" + con.Database + DateTime.Now.ToString("dd-MM-yyyy(hh_mmtt)") + ".Bak'", con);
            sqlcmd.ExecuteNonQuery();
            con.Close();
            ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Database Backup successfully..!!');", true);


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
    protected void LinkButton1_Click(object sender, EventArgs e)
    {

    }
  
}