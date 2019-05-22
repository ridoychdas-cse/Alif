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
using KHSC;
//using CrystalDecisions.CrystalReports.Engine;
//using CrystalDecisions.Shared;

public partial class TrainReport : System.Web.UI.Page
{
    //public static ReportDocument rpt;
    //private static Permis per;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
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
            ddlBranchId.Items.Clear();
            string queryBranch = "select '' branch_code, '' branch_name union select branch_code,dbo.initcap(branch_name) branch_name from pmis_branch order by 2 desc";
            util.PopulationDropDownList(ddlBranchId, "Branch", queryBranch, "branch_name", "branch_code");
        }
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {

    }
    protected void btnShow_Click(object sender, EventArgs e)
    {
        string connectionString = DataManager.OraConnString();
        SqlConnection conn = new SqlConnection();
        conn.ConnectionString = connectionString;
        if (rdoSelectCriteria.SelectedValue == "YEAR")
        {
            if (rdoReportType.SelectedValue == "S")
            {
                string query = "SELECT x.emp_no, dbo.initcap(x.F_NAME||' '||M_NAME||' '||L_NAME) AS Name, dbo.initcap(y.desig_name)desig_name,dbo.initcap(t.branch_name)branch_name, "+
                " nullif(dbo.initcap(TRAIN_TITLE),'NOT MENTIONED')AS TITLE, dbo.initcap(PLACE)place, dbo.initcap(COUNTRY)country, dbo.initcap(FINAN)finan, convert(AMOUNT)amount, "+
                " convert(YEAR)year, convert(nullif(DU_YEAR,0))du_year, convert(nullif(DU_MONTH,0))du_month, convert(nullif(DU_DAY,0))du_day "+
                " FROM PMIS_PERSONNEL x,(SELECT emp_no,joining_desig,joining_date,UPPER(desig_name) desig_name FROM PMIS_PROMOTION a,PMIS_DESIG_CODE b  "+
                " WHERE a.joining_desig=b.Desig_Code(+) AND (emp_no,joining_date) IN (SELECT emp_no,MAX(joining_date)join_date FROM PMIS_PROMOTION "+
                " GROUP BY emp_no) GROUP BY emp_no,joining_desig,desig_name,joining_date) y, PMIS_TRAIN_DTL z,PMIS_BRANCH t "+
                " WHERE x.emp_no=y.emp_no(+) AND x.emp_no=z.emp_no AND x.branch_code=t.branch_code(+) and convert(year) like nullif('"+txtYear.Text+"','%') order by t.branch_code,train_title ";
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataSet Ds = new DataSet();
                adapter.Fill(Ds, "dtTrainReport");
                if (Ds.Tables[0].Rows.Count == 0)
                {
                    return;
                }
                //rpt = new ReportDocument();
                //rpt.Load(Server.MapPath("rptTrainings.rpt"));
                //rpt.SetDatabaseLogon("pp", "pp");
                //rpt.PrintOptions.PaperSize = PaperSize.PaperA4;
                //rpt.PrintOptions.PaperOrientation = PaperOrientation.Landscape;
                //rpt.SetDataSource(Ds);
                //CrystalReportViewer1.ReportSource = rpt;
                //CrystalReportViewer1.DisplayGroupTree = false;
                //CrystalReportViewer1.DisplayToolbar = false;
                //rpt.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Response, true, "");
            }
        }
    }
    protected void rdoSelectCriteria_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdoSelectCriteria.SelectedValue == "YEAR")
        {
            txtYear.Enabled = true;
            txtYear.Focus();
            txtEmployeeId.Enabled = false;
            txtEmployeeName.Enabled = false;
            txtTrainTitle.Enabled = false;
            ddlBranchId.Enabled = false;
        }
        else if (rdoSelectCriteria.SelectedValue == "BRANCH")
        {
            txtYear.Enabled = false;
            txtEmployeeId.Enabled = false;
            txtEmployeeName.Enabled = false;
            txtTrainTitle.Enabled = false;
            ddlBranchId.Enabled = true;
            ddlBranchId.Focus();
        }
        else if (rdoSelectCriteria.SelectedValue == "TITLE")
        {
            txtYear.Enabled = false;
            txtEmployeeId.Enabled = false;
            txtEmployeeName.Enabled = false;
            txtTrainTitle.Enabled = true;
            txtTrainTitle.Focus();
            ddlBranchId.Enabled = false;            
        }
        else if (rdoSelectCriteria.SelectedValue == "EMP")
        {
            txtYear.Enabled = false;
            txtEmployeeId.Enabled = true;
            txtEmployeeId.Focus();
            txtEmployeeName.Enabled = false;
            txtTrainTitle.Enabled = false;
            ddlBranchId.Enabled = false;
        }
        else if (rdoSelectCriteria.SelectedValue == "ALL")
        {
            txtYear.Enabled = true;
            txtEmployeeId.Enabled = true;
            txtEmployeeName.Enabled = false;
            txtTrainTitle.Enabled = true;
            ddlBranchId.Enabled = true;
        }
        else 
        {
            txtYear.Enabled = false;
            txtEmployeeId.Enabled = false;
            txtEmployeeName.Enabled = false;
            txtTrainTitle.Enabled = false;
            ddlBranchId.Enabled = false;
        }
    }
    protected void txtEmployeeId_TextChanged(object sender, EventArgs e)
    {
        string connectionString = DataManager.OraConnString();
        SqlDataReader dReader;
        SqlCommand cmd;
        SqlConnection conn = new SqlConnection();
        conn.ConnectionString = connectionString;
        conn.Open();
        cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = "Select employee_id,name from pay_employee a where employee_id= '" + txtEmployeeId.Text + "'";
        dReader = cmd.ExecuteReader();
        if (dReader.HasRows == true)
        {
            while (dReader.Read())
            {
                txtEmployeeName.Text = dReader["name"].ToString();
            }
        }
    }
}
