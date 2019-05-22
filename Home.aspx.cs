using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using KHSC;

public partial class Home : System.Web.UI.Page
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
                    Session["user"] = ses.UserId;  Session["wnote"] = "KHSC Mr. " + UsersManager.getUserName(ses.UserId);
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
        

        if (!IsPostBack)
        {
            try
            {
                if (int.Parse(Session["userlevel"].ToString()) > 0)
                {
                    pnlTask.Visible = true;
                    lbAuthList.Visible = true;
                    dgVoucher.Visible = true;
                    dgVoucher.DataBind();
                    lbAuthList.Text = "Show: Authorization List";                    
                }
                else
                {
                    pnlTask.Visible = true;
                    lbAuthList.Visible = false;
                }
                ((Label)Page.Master.FindControl("lblLogin")).Text = Session["wnote"].ToString();
                ((LinkButton)Page.Master.FindControl("lbLogout")).Visible = true;
                lbAuthList.Visible = false;
            }
            catch
            {
                Session["user"] = "";
                Session["pass"] = "";
                pnlTask.Visible = false;
                Response.Redirect("Default.aspx?sid=sam");
            }
            pnlChangePass.Visible = false;
        } 
    }
    protected void dgVoucher_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Session["vchno"] = dgVoucher.SelectedRow.Cells[1].Text;
        Response.Redirect("Voucher.aspx?vchno=" + dgVoucher.SelectedRow.Cells[1].Text + "&mno=0.0");
    }
    protected void lbAuthList_Click(object sender, EventArgs e)
    {
        pnlChangePass.Visible = false;
        if (lbAuthList.Text == "Show: Authorization List")
        {
            //dgVoucher.Visible = true;
            string criteria = "(convert(nullif(autho_user_type,1))+1) = " + Session["userlevel"].ToString() + " order by value_date desc, vch_sys_no desc";
            DataTable dtTable = VouchManager.GetVouchMstAuth(criteria);
            dgVoucher.DataSource = dtTable;
            if (dtTable.Rows.Count > 0)
            {
                pnlTask.Visible = true;
                dgVoucher.Visible = true;
                lbAuthList.Text = "Hide: Authorization List";
            }
            dgVoucher.DataBind();
        }
        else
        {
            //dgVoucher.Visible = false;
            pnlTask.Visible = true;
            dgVoucher.Visible = false;
            lbAuthList.Text = "Show: Authorization List";
        }
    }
    protected void dgVoucher_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        pnlTask.Visible = true;
        dgVoucher.Visible = true;
        string userLvl = Session["userlevel"].ToString();
        string criteria = "(convert(nullif(autho_user_type,1))+1) = " + userLvl + " order by value_date desc";
        DataTable dtTable = VouchManager.GetVouchMstAuth(criteria);
        dgVoucher.DataSource = dtTable;
        dgVoucher.PageIndex = e.NewPageIndex;
        dgVoucher.DataBind();
    }
    protected void lbChangePassword_click(object sender, EventArgs e)
    {
        if (txtCpNewPass.Text != txtCpConfPass.Text)
        {
            lblTranStatus.Text = "New Password & Confirm Password are not same!!";
            lblTranStatus.Visible = true;
            lblTranStatus.ForeColor = System.Drawing.Color.Red;
        }
        else if (txtCpCurPass.Text == String.Empty)
        {
            lblTranStatus.Text = "Please provide current password!!";
            lblTranStatus.Visible = true;
            lblTranStatus.ForeColor = System.Drawing.Color.Red;
        }
        else if (txtCpNewPass.Text == String.Empty | txtCpConfPass.Text == String.Empty)
        {
            lblTranStatus.Text = "New password cannot be null!!";
            lblTranStatus.Visible = true;
            lblTranStatus.ForeColor = System.Drawing.Color.Red;
        }
        else if (txtCpNewPass.Text != String.Empty & txtCpConfPass.Text != String.Empty)
        {
            Users usr = UsersManager.getUser(txtCpUserName.Text.ToString().ToUpper());
            if (usr != null & usr.Password == txtCpCurPass.Text)
            {
                usr.Password = txtCpNewPass.Text;
                UsersManager.UpdateUser(usr);
                lblTranStatus.Text = "Password has changed!!";
                lblTranStatus.Visible = true;
                lblTranStatus.ForeColor = System.Drawing.Color.Green;
            }
            else
            {
                lblTranStatus.Text = "Old password is not correct!!";
                lblTranStatus.Visible = true;
                lblTranStatus.ForeColor = System.Drawing.Color.Red;
            }
        }
    }
    protected void lbCancel_Click(object sender, EventArgs e)
    {
        pnlChangePass.Visible = false;
    }
    protected void lbChangePass_Click(object sender, EventArgs e)
    {
        pnlTask.Visible = true;
        dgVoucher.DataBind();
        pnlChangePass.Visible = true;
        txtCpUserName.Text = Session["user"].ToString();
        lblTranStatus.Visible = false;
    }

}