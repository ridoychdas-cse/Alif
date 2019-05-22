using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using KHSC;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public partial class _Default : System.Web.UI.Page
{
    public int userLvl = 0;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["sid"] != null)
        {
            string RepType = Request.QueryString["sid"].ToString();
            if (RepType != "sam")
            {
                Response.Redirect("Default.aspx?sid=sam");
                //ClientScript.RegisterStartupScript(this.GetType(), "ale", "closeWindowNoPrompt();", true);
            }
        }
        else
        {
            Response.Redirect("Default.aspx?sid=sam");
            //ClientScript.RegisterStartupScript(this.GetType(), "ale", "closeWindowNoPrompt();", true);
        }
        if (!Page.IsPostBack)
        {
            ddlBook.Items.Clear();
            string queryBook = "select 'AMB' book_name, '' book_desc   union select '*' book_name, 'New GL' book_desc  union select book_name,book_name book_desc from gl_set_of_books where book_status='A' order by 2 desc";
            util.PopulationDropDownList(ddlBook, "level", queryBook, "book_desc", "book_name");
            ddlBook.SelectedValue = "AMB";
            ddlBook.Visible = false;
            //lbClose.Attributes.Add("onclick", "opener=self;window.close()");
            System.Type oType = System.Type.GetTypeFromProgID("InternetExplorer.Application");
            txtUserName.Focus();
        }
    }
    protected void LoginBtn_Click(object sender, EventArgs e)
    {
        System.Threading.Thread.Sleep(3000);
        if (ddlBook.SelectedValue != "")
        {
            string connectionString = DataManager.OraConnString();
            SqlDataReader dReader;
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = connectionString;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "Select ID,USER_NAME,password,user_grp,description from utl_userinfo where upper(user_name)=upper('" + txtUserName.Text.Trim() + "') and status='A'";
            conn.Open();
            dReader = cmd.ExecuteReader();
            if (dReader.HasRows == true)
            {
                while (dReader.Read())

                    if (txtPassword.Text != "" && txtPassword.Text.Trim() == (dReader["password"].ToString()))
                    {
                        Session["user"] = txtUserName.Text;
                        Session["pass"] = txtPassword.Text;
                        Session["USER_ID"] = dReader["ID"].ToString();
                        Session["user_grp"] = dReader["user_grp"].ToString();
                        userLvl = int.Parse(dReader["user_grp"].ToString());
                        Session["userlevel"] = userLvl.ToString();
                        Session["book"] = ddlBook.SelectedValue;
                        //Session["dept"] = dReader["dept"].ToString();
                        string wnote = "Welcome Mr. " + dReader["description"].ToString();
                        Session["wnote"] = wnote;
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

                        clsSession ses = clsSessionManager.getLoginSession(txtUserName.Text.ToUpper(), "");
                        if (ses == null)
                        {
                            ses = new clsSession();
                            ses.UserId = txtUserName.Text.ToUpper();
                            ses.SessionTime = System.DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");
                            ses.SessionId = Session.SessionID;

                            ses.Mac = Server.HtmlEncode(Request.UserHostAddress);
                            //clsSessionManager.CreateSession(ses);
                        }
                        Response.Redirect("~/Home.aspx");

                    }
            }
            else
            {
                Session["user"] = "";
                Session["pass"] = "";
                txtUserName.Focus();
            }
        }
        else
        {
            Session["user"] = "";
            Session["pass"] = "";
            txtUserName.Focus();
        }

    }
    //private string Decrypt(string cipherText)
    //{
    //    //string EncryptionKey = "MAKV2SPBNI99212";
    //    //byte[] cipherBytes = Convert.FromBase64String(cipherText);
    //    //using (Aes encryptor = Aes.Create())
    //    //{
    //    //    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
    //    //    encryptor.Key = pdb.GetBytes(32);
    //    //    encryptor.IV = pdb.GetBytes(16);
    //    //    using (MemoryStream ms = new MemoryStream())
    //    //    {
    //    //        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
    //    //        {
    //    //            cs.Write(cipherBytes, 0, cipherBytes.Length);
    //    //            cs.Close();
    //    //        }
    //    //        cipherText = Encoding.Unicode.GetString(ms.ToArray());
    //    //    }
    //    //}
    //    //return cipherText;
    //}
}