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
using System.IO;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
//using KHSC;
using RIITS_FES_Accounts_Apps.Manager;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using KHSC;
using System.Drawing;
//using iTextSharp.text;
//using iTextSharp.text.pdf;
//using iTextSharp.text.pdf.draw;


public partial class StudentInfo : System.Web.UI.Page
{
    private static DataTable dtSegParent = new DataTable();
    private static DataTable dtSegChild = new DataTable();
    public static string FamSt = "N";
    public static Permis per;
    Student aStudent = new Student();
    ControlManager aControlManagerObj = new ControlManager();
    StudentAccountManager aStudentAccountsManagerObj=new StudentAccountManager();
    StudentManager aStudentManager=new StudentManager();
    Decimal InstallmentAmount;
    InstallmentFeeManager aInstallmentFeeManager = new InstallmentFeeManager();
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
                            wnot =  dReader["description"].ToString();
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
            Response.Redirect("Default.aspx?sid=sam");
        }
        if (!IsPostBack)
        {
            try
            {
                RefreshDropDown();
                getEmptyFam();
                imgStd.ImageUrl = "~/tmpHandler.ashx?filename=img/noimage.jpg";
                //imgFth.ImageUrl = "~/tmpHandler.ashx?filename=img/noimage.jpg";
                //imgMth.ImageUrl = "~/tmpHandler.ashx?filename=img/noimage.jpg";
                //imgGuard1.ImageUrl = "~/tmpHandler.ashx?filename=img/noimage.jpg";
                //imgGuard2.ImageUrl = "~/tmpHandler.ashx?filename=img/noimage.jpg";
                //imgGuard3.ImageUrl = "~/tmpHandler.ashx?filename=img/noimage.jpg";
                //imgGuard4.ImageUrl = "~/tmpHandler.ashx?filename=img/noimage.jpg";
                txtBirthDt.Attributes.Add("onBlur", "formatdate('" + txtBirthDt.ClientID + "')");

                //ddlCurClass.DataSource = StudentManager.GetAllStudentClassInformation();
                //ddlCurClass.DataValueField = "class_id";
                //ddlCurClass.DataTextField = "class_name";
                //ddlCurClass.DataBind();
                //ddlCurClass.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select Class"));

                //ClassSelectDropDownList.DataSource = StudentManager.GetAllStudentClassInformation();
                //ClassSelectDropDownList.DataValueField = "class_id";
                //ClassSelectDropDownList.DataTextField = "class_name";
                //ClassSelectDropDownList.DataBind();
                //ClassSelectDropDownList.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select Class"));

                ddlCourseName.DataSource = clsStdCurrentStatusManager.GetCourseName("");
                ddlCourseName.DataValueField = "ID";
                ddlCourseName.DataTextField = "CourseName";
                ddlCourseName.DataBind();
                ddlCourseName.Items.Insert(0, "");
                //ddlTracName.DataSource = StudentManager.GetAllTracName();
                //ddlTracName.DataValueField = "id";
                //ddlTracName.DataTextField = "TracName";
                //ddlTracName.DataBind();
                //ddlTracName.Items.Insert(0, "");

                //ddlTracID.DataSource = StudentManager.GetAllTracName();
                //ddlTracID.DataValueField = "id";
                //ddlTracID.DataTextField = "TracName";
                //ddlTracID.DataBind();
                //ddlTracID.Items.Insert(0, "");
                //VersionSelectDropdownList.DataSource = StudentManager.GetAllCollegeInformation();
                //VersionSelectDropdownList.DataValueField = "ID";
                //VersionSelectDropdownList.DataTextField = "CourseName";
                //VersionSelectDropdownList.DataBind();
                //VersionSelectDropdownList.Items.Insert(0, "");

                


                //********************* History Gride View **********************//

                //DataTable dt = StudentManager.GetAllStudentInformation();
                DataTable dt = StudentManager.GetAllStudentSearch("","","","");
                HistoryGridView.DataSource = dt;
                HistoryGridView.DataBind();

                CreateGridView();
               // txtInstallmentName.Text = aInstallmentFeeManager.GetAutoId();

               
                //ddlCurClass.Visible = false;
                //ddlSect.Visible = false;
                //ddlDepartment.Visible = false;
                //ddlSemister.Visible = false;
                //lbl2.Visible = false;
                //lbl3.Visible = false;
                //lbl4.Visible = false;
                //lbl7.Visible = false;
                
                //lblSection.Visible = false;
                //lblclassName.Visible = false;
                //lblDept.Visible = false;
                //lblSemister.Visible = false;

                //lblSection.Visible = ddlSect.Visible = false;
                //lblDept.Visible = ddlDepartment.Visible =  false;

                //lblGroupName.Visible = false;
                //ddlgroup.Visible = false;
                 


                //ArchiveLabel.Visible = false;
                //txtInactiveDate.Visible = false;
                //txtInactiveDate.Text = DateTime.Now.ToString("dd/MM/yyyy");

                

                btnUpdate.Visible = false;
                BtnSave.Visible = true;
                BtnSave.Enabled = true;
                btnUpdate.Enabled = true;
                txtStudentId.Enabled = true;
              //  StdTabContainer.ActiveTabIndex = 7;
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
    private void getEmptyFam()
    {
        //DataTable dt = new DataTable();
        //dt.Columns.Add("rel_name", typeof(string));
        //dt.Columns.Add("relation", typeof(string));
        //dt.Columns.Add("birth_dt", typeof(string));
        //dt.Columns.Add("age", typeof(string));
        //dt.Columns.Add("occupation", typeof(string));
        //DataRow dr = dt.NewRow();
        //dt.Rows.Add(dr);
        //dgEmpFam.DataSource = dt;
        //ViewState["dtfam"] = dt;
        //dgEmpFam.EditIndex = -1;
        //dgEmpFam.ShowFooter = true;
        //dgEmpFam.DataBind();
        //((TextBox)dgEmpFam.FooterRow.FindControl("txtBirthDt")).Attributes.Add("onBlur", "formatdate('" + ((TextBox)dgEmpFam.FooterRow.FindControl("txtBirthDt")).ClientID + "')");
    }
    //private void Refresh()
    //{
 
    //}
    private void RefreshDropDown()
    {
        ddlPerDistCode.Items.Clear();
        ddlPerDistCode.DataSource = StudentManager.GetAllDistrictInformation();
        ddlPerDistCode.DataValueField = "DISTRICT_CODE";
        ddlPerDistCode.DataTextField = "DISTRICT_NAME";
        ddlPerDistCode.DataBind();
        ddlPerDistCode.Items.Insert(0,"");
        //string queryDist = "select '' dist_code, '' dist_name  union select district_code,district_name from district_code";
        //util.PopulationDropDownList(ddlPerDistCode, "District", queryDist, "dist_name", "dist_code");

        ddlMailDistCode.Items.Clear();
        ddlMailDistCode.DataSource = StudentManager.GetAllDistrictInformation();
        ddlMailDistCode.DataValueField = "DISTRICT_CODE";
        ddlMailDistCode.DataTextField = "DISTRICT_NAME";
        ddlMailDistCode.DataBind();
        ddlMailDistCode.Items.Insert(0,"");
        //util.PopulationDropDownList(ddlMailDistCode, "District", queryDist, "dist_name", "dist_code");

        //lblClass.Items.Clear();
        //string queryCls = "select '' class_id, '' class_name union select class_id,class_name from class_info";
        //util.PopulationDropDownList(lblClass, "Class", queryCls, "class_name", "class_id");
    }
    protected void ddlPerDistCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlPerThanaCode.Items.Clear();
        ddlPerThanaCode.DataSource = StudentManager.GetAllThanaForSpecificDistric(ddlPerDistCode.SelectedValue);
        ddlPerThanaCode.DataTextField = "THANA_NAME";
        ddlPerThanaCode.DataValueField = "THANA_CODE";
        ddlPerThanaCode.DataBind();
        ddlPerThanaCode.Items.Insert(0, "");
        //string queryThana = "select '' th_code, '' th_name union select thana_code, thana_name from thana_code where district_code like nullif('" + ddlPerDistCode.SelectedValue + "','%') order by 2 desc";
        //util.PopulationDropDownList(ddlPerThanaCode, "Thana", queryThana, "th_name", "th_code");
    }
    protected void ddlMailDistCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        
        //string queryThana = "select '' th_code, '' th_name union select thana_code, thana_name from thana_code where district_code like nullif('" + ddlMailDistCode.SelectedValue + "','%') order by 2 desc";
        //util.PopulationDropDownList(ddlMailThanaCode, "Thana", queryThana, "th_name", "th_code");
        ddlMailThanaCode.Items.Clear();
        ddlMailThanaCode.DataSource = StudentManager.GetAllThanaForSpecificDistric(ddlMailDistCode.SelectedValue);
        ddlMailThanaCode.DataTextField = "THANA_NAME";
        ddlMailThanaCode.DataValueField = "THANA_CODE";
        ddlMailThanaCode.DataBind();
        ddlMailThanaCode.Items.Insert(0, "");
    }
    protected void ddlCurClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlMailThanaCode.Items.Clear();
       // string queryThana = "select '' sec_id, '' sec_name union select sec_id, sec_name from section_info where class_id='" + ddlCurClass.SelectedValue + "' order by 2 desc";
        //util.PopulationDropDownList(ddlSect, "Thana", queryThana, "sec_name", "sec_id");
    }
    //******************* Check Istudent Id ***********************//

    protected void txtStudentId_TextChanged(object sender, EventArgs e)
    {
        DataTable dts = StudentManager.getCheckDuplicateStudentId(txtStudentId.Text);
        if (dts.Rows.Count > 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('This student Id alrady save on database..!!');", true);
            txtStudentId.Text = "";
            txtStudentId.Focus();
        }
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {       
            DataTable dts = StudentManager.getCheckDuplicateStudentId(txtStudentId.Text.Trim());
            try
            {
                if (per.AllowAdd == "Y")
                {

                    if (dts.Rows.Count > 0)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('This student Id alrady save on database..!!');", true);
                        txtStudentId.Text = "";
                        txtStudentId.Focus();
                    }
                    else if (txtStudentId.Text.Trim() == "")
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Enter student ID........!!');", true);
                        txtStudentId.Focus();
                    }
                    else if (txtFName.Text == "")
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Enter first name........!!');", true);
                        txtFName.Focus();
                    }

                    else if (ddlCourseName.SelectedValue == null)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Please Enter Course name.........!!Then Saved');", true);
                        ddlCourseName.Focus();
                    }
                    else if (txtSheduleTime.Text == null)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Please input Schedule start date.........!!Then Saved');", true);
                        txtSheduleTime.Focus();
                    }
                    else if (txtEndSheduleTime.Text == "")
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('please input  Schedulde End Date.........!!');", true);
                        txtEndSheduleTime.Focus();
                    }
                    else if (txtBatch.Text == "")
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Enter batch no.........!!');", true);
                        txtBatch.Focus();
                    }
                    else
                    {
                        Student std = new Student();
                        std.StudentId = txtStudentId.Text.Trim();
                        std.FName = txtFName.Text;
                        //std.MName = txtMName.Text;
                        std.LName = txtTrainerName.Text;
                        std.BirthDt = txtBirthDt.Text;
                        std.Religion = ddlReligion.SelectedValue;
                        std.Sex = ddlSex.SelectedValue;
                        std.Status = ddlStatus.SelectedValue;
                        //if (txtInactiveDate.Visible == false)
                        //{
                        //    std.InactiveDate = "01/01/1990";
                        //}
                        //else
                        //{
                        //    std.InactiveDate = txtInactiveDate.Text;
                        //}
                        std.PerLoc = txtPerLoc.Text;
                        std.PerDistCode = ddlPerDistCode.SelectedValue;
                        std.PerThanaCode = ddlPerThanaCode.SelectedValue;
                        std.PerZipCode = txtPerZipCode.Text;
                        std.MailLoc = txtMailLoc.Text;
                        std.MailDistCode = ddlMailDistCode.SelectedValue;
                        std.MailThanaCode = ddlMailThanaCode.SelectedValue;
                        std.MailZipCode = txtMailZipCode.Text;
                        std.Email = txtEmail.Text;
                        std.SpousName = txtSpousName.Text;
                        std.StdPhoto = (byte[])Session["stdPhoto"];
                        //std.StdCurPhoto = (byte[])Session["std_cur_photo"];
                        //std.FthPhoto = (byte[])Session["fthPhoto"];
                        //std.MthPhoto = (byte[])Session["mthPhoto"];
                        //std.GuardPhoto1 = (byte[])Session["guard1Photo"];
                        //std.GuardPhoto2 = (byte[])Session["guard2Photo"];
                        //std.GuardPhoto3 = (byte[])Session["guard3Photo"];
                        //std.GuardPhoto4 = (byte[])Session["guard4Photo"];
                        std.MobileNo = txtMobileNo.Text;
                        
                        //std.ContPerson = txtContPerson.Text;
                        //std.ContRelate = ddlContRelate.SelectedValue;
                        //std.ContPhone = txtContPhone.Text;
                        //std.ContMobile = txtContMobile.Text;
                        //std.ContAddress = txtContAddress.Text;
                        std.FthName = txtFthName.Text;
                        //std.FthEdu = txtFthEdu.Text;
                        //std.FthOccup = ddlFthOccup.SelectedValue;
                        //std.FthOrg = txtFthOrg.Text;
                        std.FthTel = txtFthTel.Text;
                        //std.FthOthAct = txtFthOthAct.Text;
                        std.MthName = txtMthName.Text;
                       
                        std.MthTel = txtMthTel.Text;                
                        std.BloodGroup = ddlBloodGroup.SelectedValue;
                        std.Note = txtNote.Text;
                        //std.CollegeId = ddlCollegeName.SelectedValue;
                        std.EntryUser = "";
                        std.EntryDate = System.DateTime.Now.ToString("dd/MM/yyyy");
                        byte[] image = null;
                        if (ViewState["stdPhoto"] != " ")
                        {
                            std.StdPhoto = (byte[])ViewState["stdPhoto"];
                            image = (byte[])ViewState["stdPhoto"];

                        }

                        if (ViewState["std_cur_photo"] != " ")
                        {
                            std.StdCurPhoto = (byte[])ViewState["std_cur_photo"];
                            image = (byte[])ViewState["std_cur_photo"];

                        }
                       
                        StudentManager.CreateStd(std);
                        if (FamSt == "O")
                        {
                            //dgEmpFam.ShowFooter = false;
                            //dgEmpFam.EditIndex = -1;
                            FamManager.DeleteFam(std.StudentId);
                            Fam fam;
                            DataTable dt = (DataTable)ViewState["dtfam"];
                            foreach (DataRow drfam in dt.Rows)
                            {
                                if (drfam["rel_name"].ToString() != "")
                                {
                                    fam = new Fam();
                                    fam.StudentId = std.StudentId;
                                    fam.RelName = drfam["rel_name"].ToString();
                                    fam.Relation = drfam["relation"].ToString();
                                    fam.BirthDt = drfam["birth_dt"].ToString();
                                    fam.Age = drfam["age"].ToString();
                                    fam.Occupation = drfam["occupation"].ToString();
                                    FamManager.CreateFam(fam);
                                }
                            }
                        }

                        if (pnlCurStEdit.Visible)
                        {
                            InstallmentFee Ins = new InstallmentFee();
                            //Ins.Id = txtInstallmentName.Text;
                            //Ins.StudentId = txtStudentId.Text;
                            //Ins.TotalAmt = txtTotalAmt.Text;
                            //Ins.PayDate = txtDate.Text;
                            //Ins.MonthInterval = txtMonthInterval.Text;
                            //Ins.InsQty = txtInstallQnty.Text;
                            //Ins.AdmissionFee = txtAdmissionFee.Text;

                            //DataTable dt1 = (DataTable)ViewState["dt"];
                            //if (txtTotalAmt.Text != "")
                            //{
                            //    aInstallmentFeeManager.CreateInstallment(dt1, Ins);
                            //}
                        }
                        if (pnlCurStEdit.Visible)
                        {
                            clsStdCurrentStatus st = new clsStdCurrentStatus();
                            st.StudentId = std.StudentId.Trim();                             
                            st.AddmissionYear = txtAddmissionYear.Text;
                            //st.TracID = ddlTracName.SelectedValue;
                            st.CourseID = ddlCourseName.SelectedValue;
                            st.CourseFee = txtCourseFee.Text;
                            st.Waiver = txtWaiver.Text;
                            st.Discount = txtDiscountTaka.Text;
                            st.ScheduleTime = txtSheduleTime.Text;
                            st.BatchNo = txtBatch.Text;
                            st.CertificateDate = txtCertificationDate.Text;
                            //st.FacultyID = lblFAcultyID.Text;
                            st.TotalAmount = txtTotalAMount.Text;
                            st.PayAmount = txtPayAmount.Text;
                            st.DueAmount = txtDueAmount.Text;
                            st.LogineBy = "";
                            st.TrainerName = txtTrainerName.Text;
                            st.ScheduleTimeEnd = txtEndSheduleTime.Text;
                            st.ClassTime = txtClassTime.Text;
                            st.APMTime = lblTimeAM.Text;

                            if (SatCheckBox.Checked == true)
                            {st.SatDay = SatCheckBox.Text;}
                            else
                            { st.SatDay = "";}
                            if (SunCheckBox.Checked == true)
                            {st.SunDay = SatCheckBox.Text;}
                            else
                            { st.SunDay = "";}
                            if (MonCheckBox.Checked == true)
                            {st.MonDay = SatCheckBox.Text; }
                            else
                            { st.MonDay = "";}
                            if (TueCheckBox.Checked == true)
                            { st.TuesDay = SatCheckBox.Text; }
                            else
                            { st.TuesDay = "";}
                            if (WedCheckBox.Checked ==true)
                            {st.WednessDay = SatCheckBox.Text;}
                            else
                            { st.WednessDay = "";}
                            if (ThusCheckBox.Checked == true)
                            { st.ThusDay = SatCheckBox.Text;}
                            else
                            {st.ThusDay = "";}
                            if (FriCheckBox.Checked == true)
                            { st.FriDay = SatCheckBox.Text; }
                            else
                            { st.FriDay = "";}

                            clsStdCurrentStatusManager.CreateCurrentStatus(st);
                        }                        
                        ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Record(s) is/are created suceessfullly!!');", true);
                        BtnSave.Enabled = false;
                    }
                   
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('You have not enough permissoin to create this record!!');", true);
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
    private void Summation()
    {
        if (string.IsNullOrEmpty(txtCourseFee.Text))
        { txtCourseFee.Text = "0"; }
        if (string.IsNullOrEmpty(txtDiscountTaka.Text))
        { txtDiscountTaka.Text = "0"; }
        if (string.IsNullOrEmpty(txtWaiver.Text))
        { txtWaiver.Text = "0"; }
        if (string.IsNullOrEmpty(txtTotalAMount.Text))
        { txtTotalAMount.Text = "0"; }
        if (string.IsNullOrEmpty(txtPayAmount.Text))
        { txtPayAmount.Text = "0"; }
        //if (string.IsNullOrEmpty(txtDueAmount.Text))
        //{ txtDueAmount.Text = "0"; }
        decimal Quan = Convert.ToDecimal(txtDiscountTaka.Text);
        decimal UnitPrice = Convert.ToDecimal(txtWaiver.Text);
        decimal Total = Quan + UnitPrice;
        decimal paidamnt = Convert.ToDecimal(txtCourseFee.Text);
        decimal dueAmnt = paidamnt - Total;
        txtTotalAMount.Text = dueAmnt.ToString("N2");
        txtDueAmount.Text = (dueAmnt - Convert.ToDecimal(txtPayAmount.Text)).ToString("N2");
        //txtGrandTotal.Text = Total.ToString();
    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        Student std = StudentManager.getStd(txtStudentId.Text.Trim());        
        try
        {
            if (std != null)
            {
                if (per.AllowEdit == "Y")
                {
                    std = new Student();
                    std.StudentId = txtStudentId.Text.Trim();
                    std.FName = txtFName.Text;
                    //std.MName = txtMName.Text;
                    std.LName = txtMobileNo.Text;
                    std.BirthDt = txtBirthDt.Text;
                    std.Religion = ddlReligion.SelectedValue;
                    std.Sex = ddlSex.SelectedValue;
                    std.Status = ddlStatus.SelectedValue;
                    //if (txtInactiveDate.Visible == false)
                    //{
                    //    std.InactiveDate = "01/01/1990";
                    //}
                    //else
                    //{
                    //    std.InactiveDate = txtInactiveDate.Text;
                    //}
                    std.PerLoc = txtPerLoc.Text;
                    std.PerDistCode = ddlPerDistCode.SelectedValue;
                    std.PerThanaCode = ddlPerThanaCode.SelectedValue;
                    std.PerZipCode = txtPerZipCode.Text;
                    std.MailLoc = txtMailLoc.Text;
                    std.MailDistCode = ddlMailDistCode.SelectedValue;
                    std.MailThanaCode = ddlMailThanaCode.SelectedValue;
                    std.MailZipCode = txtMailZipCode.Text;
                    std.Email = txtEmail.Text;
                    std.TelNo = txtSpousName.Text;
                    std.MobileNo = txtMobileNo.Text;
                     
                    //std.ContPerson = txtContPerson.Text;
                    //std.ContRelate = ddlContRelate.SelectedValue;
                    //std.ContPhone = txtContPhone.Text;
                    //std.ContMobile = txtContMobile.Text;
                    //std.ContAddress = txtContAddress.Text;
                    std.FthName = txtFthName.Text;
                    //std.FthEdu = txtFthEdu.Text;
                    //std.FthOccup = ddlFthOccup.SelectedValue;
                    //std.FthOrg = txtFthOrg.Text;
                    std.FthTel = txtFthTel.Text;
                    //std.FthOthAct = txtFthOthAct.Text;
                    std.MthName = txtMthName.Text;
                    //std.MthEdu = txtMthEdu.Text;
                    //std.MthOccup = ddlMthOccup.SelectedValue;
                    //std.MthOrg = txtMthOrg.Text;
                    std.MthTel = txtMthTel.Text;
                    //std.MthOthAct = txtMthOthAct.Text;
                    //std.PrevSch = txtPrevSch.Text;
                    //std.PrevAdd = txtPrevAdd.Text;
                    //std.LastClass = txtLastClass.Text;
                    std.ClassYear = txtAddmissionYear.Text;
                    //std.ReasonLeave = txtReasonLeave.Text;
                    //std.PhysicProb = txtPhysicProb.Text;
                    //std.Allergic = txtAllergic.Text;
                    //std.ChildRcv1 = txtChildRcv1.Text;
                    //std.RelRcv1 = ddlRelRcv1.SelectedValue;
                    //std.ChildRcv2 = txtChildRcv2.Text;
                    //std.RelRcv2 = ddlRelRcv2.SelectedValue;
                    //std.ChildRcv3 = txtChildRcv3.Text;
                    //std.RelRcv3 = ddlRelRcv3.SelectedValue;
                    //std.ChildRcv4 = txtChildRcv4.Text;
                    //std.RelRcv4 = ddlRelRcv4.SelectedValue;
                    std.BloodGroup = ddlBloodGroup.SelectedValue;
                    //std.CollegeId = ddlCollegeName.SelectedValue;
                    std.EntryUser = "";
                    std.EntryDate = DateTime.Now.ToString("dd/MM/yyyy");
                    byte[] image = null;
                    if (ViewState["stdPhoto"] != " ")
                    {
                        std.StdPhoto = (byte[])ViewState["stdPhoto"];
                        image = (byte[])ViewState["stdPhoto"];

                    }
                    if (ViewState["std_cur_photo"] != " ")
                    {
                        std.StdCurPhoto = (byte[])ViewState["std_cur_photo"];
                        image = (byte[])ViewState["std_cur_photo"];

                    }

                    if (ViewState["fthPhoto"] != " ")
                    {
                        std.FthPhoto = (byte[])ViewState["fthPhoto"];
                        image = (byte[])ViewState["fthPhoto"];

                    }

                    if (ViewState["mthPhoto"] != " ")
                    {
                        std.MthPhoto = (byte[])ViewState["mthPhoto"];
                        image = (byte[])ViewState["mthPhoto"];

                    }

                    if (ViewState["guard1Photo"] != " ")
                    {
                        std.GuardPhoto1 = (byte[])ViewState["guard1Photo"];
                        image = (byte[])ViewState["guard1Photo"];

                    }

                    if (ViewState["guard2Photo"] != " ")
                    {
                        std.GuardPhoto2 = (byte[])ViewState["guard2Photo"];
                        image = (byte[])ViewState["guard2Photo"];

                    }

                    if (ViewState["guard3Photo"] != " ")
                    {
                        std.GuardPhoto3 = (byte[])ViewState["guard3Photo"];
                        image = (byte[])ViewState["guard3Photo"];

                    }

                    if (ViewState["guard4Photo"] != " ")
                    {
                        std.GuardPhoto4 = (byte[])ViewState["guard4Photo"];
                        image = (byte[])ViewState["guard4Photo"];

                    }
                    
                    StudentManager.UpdateStd(std);// TO Update Student Information
                    if (FamSt == "O")
                    {
                        //dgEmpFam.ShowFooter = false;
                        //dgEmpFam.EditIndex = -1;
                        FamManager.DeleteFam(std.StudentId);
                        Fam fam;
                        DataTable dt = (DataTable)ViewState["dtfam"];
                        foreach (DataRow drfam in dt.Rows)
                        {
                            if (drfam["rel_name"].ToString() != "")
                            {
                                fam = new Fam();
                                fam.StudentId = std.StudentId;
                                fam.RelName = drfam["rel_name"].ToString();
                                fam.Relation = drfam["relation"].ToString();
                                fam.BirthDt = drfam["birth_dt"].ToString();
                                fam.Age = drfam["age"].ToString();
                                fam.Occupation = drfam["occupation"].ToString();
                                FamManager.CreateFam(fam);
                            }
                        }
                    }
                    if (pnlCurStEdit.Visible)
                    {
                        //InstallmentFee Ins = new InstallmentFee();
                        //Ins.Id = txtInstallmentName.Text;
                        //Ins.StudentId = txtStudentId.Text;
                        //Ins.TotalAmt = txtTotalAmt.Text;
                        //Ins.PayDate = txtDate.Text;
                        //Ins.MonthInterval = txtMonthInterval.Text;
                        //Ins.InsQty = txtInstallQnty.Text;
                        //Ins.AdmissionFee = txtAdmissionFee.Text;

                        //DataTable dt1 = (DataTable)ViewState["dt"];
                        //if (txtTotalAmt.Text != "")
                        //{
                        //    aInstallmentFeeManager.CreateInstallment(dt1, Ins);
                        //}
                    }
                    if (pnlCurStEdit.Visible)
                    {
                        clsStdCurrentStatus st = new clsStdCurrentStatus();
                        st.StudentId = std.StudentId.Trim();
                        st.AddmissionYear = txtAddmissionYear.Text;
                        //st.TracID = ddlTracName.SelectedValue;
                        st.CourseID = lblCourseID.Text;
                        st.CourseFee = txtCourseFee.Text;
                        st.Waiver = txtWaiver.Text;
                        st.Discount = txtDiscountTaka.Text;
                        st.ScheduleTime = txtSheduleTime.Text;
                        st.BatchNo = txtBatch.Text;
                        //st.AddmissionDate = TxtAdmissionDate.Text;
                        st.FacultyID = lblFAcultyID.Text;
                        st.TotalAmount = txtTotalAMount.Text;
                        st.PayAmount = txtPayAmount.Text;
                        st.DueAmount = txtDueAmount.Text;
                        st.TrainerName = txtTrainerName.Text;
                        st.ScheduleTimeEnd = txtEndSheduleTime.Text;
                        st.ClassTime = txtClassTime.Text;
                        st.LogineBy = "";
                        if (ddlStatus.SelectedValue == "3")
                        {
                            clsStdCurrentStatusManager.CreatePreviousStatus(st);
                        }
                        else
                        {
                            clsStdCurrentStatusManager.UpdateCurrentStatus(st);
                        }
                    }
                     RefreshAll();
                    ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Record(s) is/are updated suceessfullly...!!');", true);
                    //Response.Write("<script>alert('Record(s) is/are updated suceessfullly..!!');</script>");
                    
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('You have not enough permissoin to update this record!!');", true);
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

    protected void BtnFind_Click(object sender, EventArgs e)
    {
        try
        {
            clearFields();
            RefreshDropDown();
            //txtStudentId.Text = HistoryGridView.SelectedRow.Cells[1].Text;           
            
            Student aStudentObj = StudentManager.GetAllStudentInformationForSpecificStudent(txtStudentId.Text.Trim());
            txtFName.Text = aStudentObj.FName.Trim();
            //txtMName.Text = aStudentObj.MName.Trim();

          
           
            //ddlSex.SelectedIndex = aControlManagerObj.IndexCalCulation(ddlSex, aStudentObj.Sex);
            //ddlSex.SelectedValue = aStudentObj.Sex;
            if (aStudentObj.Sex.Trim() != null && aStudentObj.Sex.Trim() != "")
            {
                ddlSex.SelectedValue = aStudentObj.Sex.Trim();
                //ddlSex.SelectedValue = aStudentObj.Sex;
            }
            if (aStudentObj.Religion.Trim() != null && aStudentObj.Religion.Trim() != "")
            {
                ddlReligion.SelectedValue = aStudentObj.Religion.Trim();
                //ddlReligion.SelectedValue = aStudent.Religion;
            }
            txtBirthDt.Text = aStudentObj.BirthDt.Trim();
            txtPerLoc.Text = aStudentObj.PerLoc.Trim();
            if (aStudentObj.PerDistCode.Trim() != "" && aStudentObj.PerDistCode.Trim() != null)
            {
                ddlPerDistCode.SelectedValue = aStudentObj.PerDistCode.Trim();
            }
            if (aStudentObj.PerThanaCode != "" && aStudentObj.PerThanaCode.Trim() != null)
            {                
                ddlPerThanaCode.Items.Clear();
                ddlPerThanaCode.DataSource = StudentManager.GetAllThanaForSpecificDistric(ddlPerDistCode.SelectedValue.Trim());
                ddlPerThanaCode.DataTextField = "THANA_NAME";
                ddlPerThanaCode.DataValueField = "THANA_CODE";
                ddlPerThanaCode.DataBind();
                ddlPerThanaCode.SelectedValue = aStudentObj.PerThanaCode.Trim();

            }
            txtPerZipCode.Text = aStudentObj.PerZipCode.Trim();
            txtMailLoc.Text = aStudentObj.MailLoc.Trim();
            if (aStudentObj.MailDistCode.Trim() != "" && aStudentObj.MailDistCode.Trim() != null)
            {
                ddlMailDistCode.SelectedValue = aStudentObj.MailDistCode.Trim();
            }
            if (aStudentObj.MailThanaCode.Trim() != "" && aStudentObj.MailThanaCode.Trim() != null)
            {                
                ddlMailThanaCode.Items.Clear();
                ddlMailThanaCode.DataSource = StudentManager.GetAllThanaForSpecificDistric(ddlMailDistCode.SelectedValue.Trim());
                ddlMailThanaCode.DataTextField = "THANA_NAME";
                ddlMailThanaCode.DataValueField = "THANA_CODE";
                ddlMailThanaCode.DataBind();

                ddlMailThanaCode.SelectedValue = aStudentObj.MailThanaCode.Trim();
            }
         
            //string mdistrict = StudentManager.GetDistrict(aStudentObj.MailDistCode);
            //string mthana = StudentManager.GetThana(aStudentObj.MailThanaCode);
            txtMailZipCode.Text = aStudentObj.MailZipCode.Trim();
            txtEmail.Text = aStudentObj.Email.Trim();
            txtSpousName.Text = aStudentObj.TelNo.Trim();
            txtMobileNo.Text = aStudentObj.MobileNo.Trim();
            txtFthName.Text = aStudentObj.FthName.Trim();
            //txtFthEdu.Text = aStudentObj.FthEdu.Trim();
            ddlStatus.SelectedValue = aStudentObj.Status.Trim();
            //ddlFthOccup.SelectedValue = aStudentObj.FthOccup.Trim();
            //txtFthOrg.Text = aStudentObj.FthOrg.Trim();
            txtFthTel.Text = aStudentObj.FthTel.Trim();
            //txtFthOthAct.Text = aStudentObj.FthOthAct.Trim();
            txtMthName.Text = aStudentObj.MthName.Trim();
            //txtMthEdu.Text = aStudentObj.MthEdu.Trim();

            //ddlMthOccup.SelectedValue = aStudentObj.MthOccup.Trim();
            //txtMthOrg.Text = aStudentObj.MthOrg.Trim();
            txtMthTel.Text = aStudentObj.MthTel.Trim();
            //txtMthOthAct.Text = aStudentObj.MthOthAct.Trim();

            
             
           // txtContPerson.Text = aStudentObj.ContPerson.Trim();
            //txtContAddress.Text = aStudentObj.ContAddress.Trim();
            //txtContPhone.Text = aStudentObj.ContPhone.Trim();
            //txtContMobile.Text = aStudentObj.ContMobile.Trim();

            //ddlContRelate.SelectedValue = aStudentObj.ContRelate.Trim();
            //txtChildRcv1.Text = aStudentObj.ChildRcv1.Trim();
            //txtChildRcv2.Text = aStudentObj.ChildRcv2.Trim();
            //txtChildRcv3.Text = aStudentObj.ChildRcv3.Trim();
            //txtChildRcv4.Text = aStudentObj.ChildRcv4.Trim();

            //ddlRelRcv1.SelectedValue = aStudentObj.RelRcv1.Trim();
            //ddlRelRcv2.SelectedValue = aStudentObj.RelRcv2.Trim();
            //ddlRelRcv3.SelectedValue = aStudentObj.RelRcv3.Trim();
            //ddlRelRcv4.SelectedValue = aStudentObj.RelRcv4.Trim();
            //txtPrevSch.Text = aStudentObj.PrevSch.Trim();
            //txtPrevAdd.Text = aStudentObj.PrevAdd.Trim();
            //txtLastClass.Text = aStudentObj.LastClass.Trim();
            //txtClassYear.Text = aStudentObj.ClassYear.Trim();
            //txtClassPos.Text = aStudentObj.ClassPos.Trim();
            //txtReasonLeave.Text = aStudentObj.ReasonLeave.Trim();
            //txtPhysicProb.Text = aStudentObj.PhysicProb.Trim();
            //txtAllergic.Text = aStudentObj.Allergic.Trim();
            
            //TxtAdmissionDate.Text = aStudentObj.AdmissionDt.Trim();
           
            if (aStudentObj.StdPhoto.Length > 4)
            {
                byte[] studentImage = aStudentObj.StdPhoto;
                ViewState["stdPhoto"] = studentImage;
                MemoryStream ms = new MemoryStream(studentImage);
                System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
                Session["byt"] = studentImage;
                string base64String = Convert.ToBase64String(studentImage, 0, studentImage.Length);
                imgStd.ImageUrl = "data:image/jpeg;base64," + base64String;
            }

            //if (aStudentObj.StdCurPhoto.Length > 4)
            //{
            //    byte[] stdCurrentImage = aStudentObj.StdCurPhoto;
            //    ViewState["std_cur_photo"] = stdCurrentImage;
            //    MemoryStream ms1 = new MemoryStream(stdCurrentImage);
            //    System.Drawing.Image img1 = System.Drawing.Image.FromStream(ms1);
            //    string base64String1 = Convert.ToBase64String(stdCurrentImage, 0, stdCurrentImage.Length);
            //    imgStdCur.ImageUrl = "data:image/jpeg;base64," + base64String1;

            //}

            //if (aStudentObj.FthPhoto.Length > 4)
            //{
            //    byte[] fthImage = aStudentObj.FthPhoto;
            //    ViewState["fthPhoto"] = fthImage;
            //    MemoryStream ms2 = new MemoryStream(fthImage);
            //    System.Drawing.Image img2 = System.Drawing.Image.FromStream(ms2);
            //    string base64String2 = Convert.ToBase64String(fthImage, 0, fthImage.Length);
            //    imgFth.ImageUrl = "data:image/jpeg;base64," + base64String2;

            //}

            //if (aStudentObj.MthPhoto.Length > 4)
            //{
            //    byte[] mthImage = aStudentObj.MthPhoto;
            //    ViewState["mthPhoto"] = mthImage;
            //    MemoryStream ms3 = new MemoryStream(mthImage);
            //    System.Drawing.Image img3 = System.Drawing.Image.FromStream(ms3);
            //    string base64String3 = Convert.ToBase64String(mthImage, 0, mthImage.Length);
            //    imgMth.ImageUrl = "data:image/jpeg;base64," + base64String3;

            //}

            //if (aStudentObj.GuardPhoto1.Length > 4)
            //{
            //    byte[] grdImage = aStudentObj.GuardPhoto1;
            //    ViewState["guard1Photo"] = grdImage;
            //    MemoryStream ms4 = new MemoryStream(grdImage);
            //    System.Drawing.Image img4 = System.Drawing.Image.FromStream(ms4);
            //    string base64String4 = Convert.ToBase64String(grdImage, 0, grdImage.Length);
            //    imgGuard1.ImageUrl = "data:image/jpeg;base64," + base64String4;
            //}

            //if (aStudentObj.GuardPhoto2.Length > 4)
            //{
            //    byte[] grdImage1 = aStudentObj.GuardPhoto2;
            //    ViewState["guard2Photo"] = grdImage1;
            //    MemoryStream ms5 = new MemoryStream(grdImage1);
            //    System.Drawing.Image img5 = System.Drawing.Image.FromStream(ms5);
            //    string base64String5 = Convert.ToBase64String(grdImage1, 0, grdImage1.Length);
            //    imgGuard2.ImageUrl = "data:image/jpeg;base64," + base64String5;
            //}

            //if (aStudentObj.GuardPhoto3.Length > 4)
            //{
            //    byte[] grdImage2 = aStudentObj.GuardPhoto3;
            //    ViewState["guard3Photo"] = grdImage2;
            //    MemoryStream ms6 = new MemoryStream(grdImage2);
            //    System.Drawing.Image img6 = System.Drawing.Image.FromStream(ms6);
            //    string base64String6 = Convert.ToBase64String(grdImage2, 0, grdImage2.Length);
            //    imgGuard3.ImageUrl = "data:image/jpeg;base64," + base64String6;
            //}

            //if (aStudentObj.GuardPhoto4.Length > 4)
            //{
            //    byte[] grdImage3 = aStudentObj.GuardPhoto4;
            //    ViewState["guard4Photo"] = grdImage3;
            //    MemoryStream ms7 = new MemoryStream(grdImage3);
            //    System.Drawing.Image img7 = System.Drawing.Image.FromStream(ms7);
            //    string base64String7 = Convert.ToBase64String(grdImage3, 0, grdImage3.Length);
            //    imgGuard4.ImageUrl = "data:image/jpeg;base64," + base64String7;
            //}
            //byte[] currentStdPhoto = aStudentObj.StdCurPhoto;
            //ViewState["std_cur_photo"] = currentStdPhoto;
            //byte[] fatherPhoto = aStudentObj.FthPhoto;
            //ViewState["fthPhoto"] = aStudentObj.FthPhoto;
            //byte[] motherPhoto = aStudentObj.MthPhoto;
            //ViewState["mthPhoto"] = aStudentObj.MthPhoto;
            //byte[] gurdian1 = aStudentObj.GuardPhoto1;
            //ViewState["guard1Photo"] = aStudentObj.GuardPhoto1;
            //byte[] gurdian2 = aStudentObj.GuardPhoto2;
            //ViewState["guard2Photo"] = aStudentObj.GuardPhoto2;
            //byte[] gurdian3 = aStudentObj.GuardPhoto3;
            //ViewState["guard3Photo"] = aStudentObj.GuardPhoto3;
            //byte[] gurdian4 = aStudentObj.GuardPhoto4;
            //ViewState["guard4Photo"] = aStudentObj.GuardPhoto4;
            //ShowImage(studentImage, imgStd);
            //ShowImage(currentStdPhoto, imgStdCur);
            //ShowImage(fatherPhoto, imgFth);
            //ShowImage(motherPhoto, imgMth);
            //ShowImage(gurdian1, imgGuard1);
            //ShowImage(gurdian2, imgGuard2);
            //ShowImage(gurdian3, imgGuard3);
            //ShowImage(gurdian4, imgGuard4);
            DataTable familtyDetails = FamManager.GetStdFamInformationForSpecificStudent(txtStudentId.Text.Trim());
            //dgEmpFam.DataSource = familtyDetails;
            //dgEmpFam.DataBind();
            ViewState["dtfam"] = familtyDetails;

            clsStdCurrentStatus stdObj = clsStdCurrentStatusManager.GetAllStdCurrentStatusForSpecificStd(txtStudentId.Text.Trim());
            //****************** Current Ststus*****************************************
            //txtAddmissionYear.Text = stdObj.AddmissionYear.Trim();
            //ddlTracName.SelectedValue = stdObj.tracId.Trim();
          
            //txtSheduleTime.Text = stdObj.ScheduleId.Trim();
            //txtCourseFee.Text = stdObj.CourseFee.Trim();
            //txtBatch.Text = stdObj.BatchNo.Trim();
            //txtDiscountTaka.Text = stdObj.Discount.Trim();
            //txtWaiver.Text = stdObj.Waiver.Trim();
            //txtTrainerName.Text = stdObj.TrainerName.Trim();
            //TxtAdmissionDate.Text = stdObj.AddmissionDate.Trim();
            //InstallmentFee InsObj = InstallmentFeeManager.GetInstallmentFeeInfo(txtStudentId.Text.Trim());
            DataTable dt = StudentManager.GetStudentCurrentStatus(txtStudentId.Text.Trim());
            //DataTable dts = clsStdCurrentStatusManager.GetCourseInfo(dt.Rows[0]["CourseId"].ToString());           

            txtCourseFee.Text = dt.Rows[0]["CourseFee"].ToString();
            txtDiscountTaka.Text = dt.Rows[0]["Discount"].ToString();
            txtSheduleTime.Text = dt.Rows[0]["std_admission_date"].ToString();
            txtAddmissionYear.Text = dt.Rows[0]["AddmisionYear"].ToString();
            txtBatch.Text = dt.Rows[0]["BatchNo"].ToString();
            //txtTrainerName.Text = dt.Rows[0]["FacultyName"].ToString();
            lblCourseID.Text = dt.Rows[0]["CourseId"].ToString();
            ddlCourseName.SelectedValue=dt.Rows[0]["CourseId"].ToString();
            txtTotalAMount.Text = dt.Rows[0]["PayAmount"].ToString();
            txtPayAmount.Text = dt.Rows[0]["PaidAmount"].ToString();
            txtDueAmount.Text = dt.Rows[0]["DueAmount"].ToString();
            //lblFAcultyID.Text = dt.Rows[0]["FacultyID"].ToString();
            txtTrainerName.Text = dt.Rows[0]["TrainerName"].ToString();
            //lblSheduleID.Text = dt.Rows[0]["SheduleID"].ToString();
            txtWaiver.Text = dt.Rows[0]["Waiver"].ToString();
            lblTracID.Text = dt.Rows[0]["tracId"].ToString();
            //ddlTracName.SelectedValue = dt.Rows[0]["tracId"].ToString();
            txtEndSheduleTime.Text = dt.Rows[0]["ShehuleEnd"].ToString();
            txtSheduleTime.Text = dt.Rows[0]["SheduleStart"].ToString();
            txtClassTime.Text = dt.Rows[0]["ClassTime"].ToString();
            ddlCourseName.DataSource = clsStdCurrentStatusManager.GetCourseNameAssaign(lblTracID.Text);
            ddlCourseName.DataValueField = "ID";
            ddlCourseName.DataTextField = "CourseName";
            ddlCourseName.DataBind();
            ddlCourseName.Items.Insert(0, "Select Course");

            if (stdObj.CourseID.Trim() != "" && stdObj.CourseID.Trim() != null)
            {
                          
                util.PopulationDropDownList(ddlCourseName, "tbl_Course_Name", "Select ci.CourseName,ci.ID,ci.CourseFee,ci.Doscount as Discount from tbl_Course_Name  ci inner join CourseTrac ct on ct.id=ci.TracID where ct.ID='" + stdObj.tracId.Trim()+ "'", "CourseName", "ID");
                ddlCourseName.SelectedValue = stdObj.CourseID.Trim();     

                //ddlCurClass.DataValueField = "class_id";
                //ddlCurClass.DataTextField = "class_name";
                //ddlCurClass.DataBind();
                //ddlCurClass.Items.Insert(0, new System.Web.UI.WebControls.ListItem(" "));

                //ddlCurClass.SelectedValue = stdObj.ClassId.Trim();
            }

            //if (stdObj.DeptId.Trim() != "" && stdObj.DeptId.Trim() != null)
            //{
            //    ddlDepartment.DataSource = StudentManager.GetAllStudentDeptInformation(ddlCollegeName.SelectedValue);
            //    ddlDepartment.DataValueField = "DeptId";
            //    ddlDepartment.DataTextField = "DeptName";
            //    ddlDepartment.DataBind();
            //    ddlDepartment.Items.Insert(0, new System.Web.UI.WebControls.ListItem(" "));

            //    ddlDepartment.SelectedValue = stdObj.DeptId.Trim();
            //}
            //ddlSect.DataSource = StudentManager.GetShowSectionOnClass(ddlCurClass.SelectedValue.Trim());
            //ddlSect.DataTextField = "sec_name";
            //ddlSect.DataValueField = "sec_id";
            //ddlSect.DataBind();
            //ddlSect.Items.Insert(0, new System.Web.UI.WebControls.ListItem(""));
            //ddlSect.SelectedValue = stdObj.Sect.Trim();

            //ddlVersion.DataSource = StudentManager.GetShowVersionOnClass(ddlCurClass.SelectedValue.Trim());
            //ddlVersion.DataTextField = "version_name";
            //ddlVersion.DataValueField = "version_id";
            //ddlVersion.DataBind();
            //ddlVersion.Items.Insert(0, new System.Web.UI.WebControls.ListItem(""));
            //ddlVersion.SelectedValue = stdObj.version.Trim();

            //ddlShift.DataSource = StudentManager.GetShowShiftOnClass(ddlCurClass.SelectedValue.Trim());
            //ddlShift.DataTextField = "shift_name";
            //ddlShift.DataValueField = "shift_id";
            //ddlShift.DataBind();
            //ddlShift.Items.Insert(0, new System.Web.UI.WebControls.ListItem(""));
            //ddlShift.SelectedValue = stdObj.Shift.Trim();
            //txtCurYear.Text = stdObj.ClassYear.Trim();

            //if (ddlCollegeName.SelectedValue == "2")
            //{
            //    if (stdObj.DeptId.Trim() != "")
            //    {
            //        ddlDepartment.SelectedValue = stdObj.DeptId.Trim();
            //    }
            //    lblDept.Visible = ddlDepartment.Visible =  true;
            //}
            //else
            //{ lblDept.Visible = ddlDepartment.Visible =  false; }


            //if (ddlCurClass.SelectedValue == "13" || ddlCurClass.SelectedValue == "14")
            //{
            //    if (stdObj.Group.Trim() != "")
            //    {
            //        ddlgroup.SelectedValue = stdObj.Group.Trim();
            //    }
            //    lblGroupName.Visible = ddlgroup.Visible =  true;
            //}
            //else
            //{ lblGroupName.Visible = ddlgroup.Visible =   false; }

            
            ////if (stdObj.Group.Trim() != "")
            ////{
            ////    ddlgroup.Visible = lblGroupName.Visible = true;
            ////    ddlgroup.SelectedValue = stdObj.Group.Trim();
            ////}
            ////else
            ////{ ddlgroup.Visible = lblGroupName.Visible = false; }
            //txtAdmissionFee.Text = InsObj.AdmissionFee;
            //txtTotalAmt.Text = InsObj.TotalAmt;
            //txtInstallQnty.Text = InsObj.InsQty;
            //txtMonthInterval.Text = InsObj.MonthInterval;
            //txtDate.Text = InsObj.PayDate;
            //txtInstallmentName.Text = InsObj.Id;
            //txtRoll.Text = stdObj.Roll.Trim();
            //txtClassStart.Text = stdObj.ClassStart.Trim();
            //TxtDoa.Text = stdObj.AdmissionDt.Trim();
            //if (aStudentObj.BloodGroup.Trim() != "")
            //{
            //    ddlBloodGroup.SelectedValue = aStudentObj.BloodGroup.Trim();
            //}

            //DataTable Insdt = InstallmentFeeManager.GetInstallmentFeeDtls(txtStudentId.Text.Trim());
            //dgInstallment.DataSource = Insdt;
            //ViewState["dt"] = Insdt;
            //dgInstallment.DataBind();

            btnUpdate.Visible = true;
            BtnSave.Visible = false;
            txtStudentId.Enabled = false;
            //StdTabContainer.ActiveTabIndex = 5;
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
    protected void BtnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtStudentId.Text != "")
            {
                Student aStudent = new Student();
                clsStdCurrentStatus st = new clsStdCurrentStatus();
                st.StudentId = txtStudentId.Text;
                //st.ClassId = ddlCurClass.SelectedValue;
                //st.ClassYear = txtCurYear.Text;
                ////st.Shift = ddlShift.SelectedValue;
                //st.Sect = ddlSect.SelectedValue;
                //st.ClassStart = txtClassStart.Text;
                //st.Roll = txtRoll.Text;
                //st.AdmissionDt = TxtDoa.Text;
                st.LogineBy = Session["user"].ToString();
                //clsStdEduManager.DeleteEdu(std.StudentId);
                //ClsStudentSubjectSetManager.DeleteStudentSubject(std.StudentId);
                aStudent.EntryUser = Session["user"].ToString();
                aStudent.StudentId = txtStudentId.Text.Trim();
                clsStdCurrentStatusManager.DeleteCurrentStatus(st);
                FamManager.DeleteFam(txtStudentId.Text);
                StudentManager.DeleteStd(aStudent);
                clearFields();
                ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Records are deleted suceessfullly!!');", true);
                   
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('You have not enough permissoin to delete this record!!');", true);
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
    protected void BtnReset_Click(object sender, EventArgs e)
    {
        try
        {

           RefreshAll();
            Response.Redirect("~/StudentInfo.aspx?mno=2.9");
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
        //DataTable dt = StudentManager.GetAllStudentInformation();
        //HistoryGridView.DataSource = dt;
        //HistoryGridView.DataBind();
        clearFields();
        getEmptyFam();
        RefreshDropDown();
        ddlPerThanaCode.Items.Clear();
        ddlMailThanaCode.Items.Clear();
        ViewState.Remove("stdPhoto");
        ViewState.Remove("fthPhoto");
        ViewState.Remove("std_cur_photo");
        ViewState.Remove("mthPhoto");
        ViewState.Remove("guard1Photo");
        ViewState.Remove("guard2Photo");
        ViewState.Remove("guard3Photo");
        ViewState.Remove("guard4Photo");
        //Response.Redirect("~/StudentInfo.aspx");        
        txtStudentId.Text = "";
        txtFName.Text = "";
        //txtMName.Text = "";
        txtTrainerName.Text = "";
        txtBirthDt.Text = "";
        txtPerLoc.Text = "";
        txtPerZipCode.Text = "";
        txtMailLoc.Text = "";
        txtMailZipCode.Text = "";
        txtEmail.Text = "";
        txtSpousName.Text = "";
        txtMobileNo.Text = "";
        txtFthName.Text = "";
        //txtFthEdu.Text = "";
        //txtFthOrg.Text = "";
        txtFthTel.Text = "";
        //txtFthOthAct.Text = "";
        txtMthName.Text = "";
        //txtMthEdu.Text = "";
        //txtMthOrg.Text = "";
        //txtMthOthAct.Text = "";
        lblCourseID.Text = lblFAcultyID.Text = lblSheduleID.Text = "";
        //txtContPerson.Text = "";
        //txtContAddress.Text = "";
        //txtContPhone.Text = "";
        //txtContMobile.Text = "";
        //txtChildRcv1.Text = "";
        //txtChildRcv2.Text = "";
        //txtChildRcv3.Text = "";
        //txtChildRcv4.Text = "";
        //txtPrevSch.Text = "";
        //txtPrevAdd.Text = "";
        //txtLastClass.Text = "";
        //txtClassYear.Text = "";
        //txtClassPos.Text = "";
        //txtReasonLeave.Text = "";
        //txtPhysicProb.Text = "";
        //txtAllergic.Text = "";
        
        //lblClass.Text = "";
        //lblShift.Text = "";
        //lblSect.Text = "";
        //txtCourseFee.Text = "";
        //lblRoll.Text = "";
        //lblClassStart.Text = "";
        //lblDoa.Text = "";
        //txtCurYear.Text = "";
        //txtRoll.Text = "";
        //ddlCurClass.SelectedIndex = -1;
        //ddlCollegeName.SelectedIndex = -1;
        //ddlDepartment.SelectedIndex = -1;
        //ddlSect.SelectedIndex= - 1;
        //ddlVersion.SelectedIndex = -1;
        //ddlgroup.SelectedIndex = -1;
        //ddlSex.SelectedIndex = -1;
        //ddlShift.SelectedIndex = -1;
        ddlReligion.SelectedIndex = -1;
        ddlBloodGroup.SelectedIndex = -1;

        //txtClassStart.Text = "";
        //TxtDoa.Text = "";
        //ArchiveLabel.Visible = false;
        //txtInactiveDate.Visible = false;
        txtCourseFee.Text = "";
        txtDiscountTaka.Text = "";
        txtAddmissionYear.Text = "";
        txtTrainerName.Text = "";
        ddlCourseName.SelectedIndex = -1;
        txtBatch.Text = "";
        btnUpdate.Visible = false;
        BtnSave.Visible = true;
        BtnSave.Enabled = true;
        txtStudentId.Enabled = true;
        txtSheduleTime.Text = "";
        txtEndSheduleTime.Text = "";
        lblTimeAM.Text= "";
        //StdTabContainer.ActiveTabIndex = 7;
        txtStudentId.Focus();
    }
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            clearFields();
            RefreshDropDown();
            btnUpdate.Visible = true;
            BtnSave.Visible = false;
            //txtStudentId.Text = HistoryGridView.SelectedRow.Cells[1].Text;
            Student aStudentObj = StudentManager.GetAllStudentInformationForSpecificStudent(txtStudentId.Text);
            if (aStudentObj.StudentId == "" || aStudentObj.StudentId == null)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Not Fount This Student.!!');", true);
            }
            else
            {
                txtFName.Text = aStudentObj.FName;
                //txtMName.Text = aStudentObj.MName;
                txtTrainerName.Text = aStudentObj.LName;
                //ddlSex.SelectedIndex = aControlManagerObj.IndexCalCulation(ddlSex, aStudentObj.Sex);
                //ddlSex.SelectedValue = aStudentObj.Sex;
                if (aStudentObj.Sex != null && aStudentObj.Sex != "")
                {
                    ddlSex.SelectedValue = aStudentObj.Sex;
                    //ddlSex.SelectedValue = aStudentObj.Sex;
                }
                if (aStudentObj.Religion != null && aStudentObj.Religion != "")
                {
                    ddlReligion.SelectedValue = aStudentObj.Religion;
                    //ddlReligion.SelectedValue = aStudent.Religion;
                }
                txtBirthDt.Text = aStudentObj.BirthDt;
                txtPerLoc.Text = aStudentObj.PerLoc;
                if (aStudentObj.PerDistCode != "" && aStudentObj.PerDistCode != null)
                {
                    ddlPerDistCode.SelectedValue = aStudentObj.PerDistCode;
                }
                if (aStudentObj.PerThanaCode != "" && aStudentObj.PerThanaCode != null)
                {
                    ddlPerThanaCode.SelectedValue = aStudentObj.PerThanaCode;
                    ddlPerThanaCode.Items.Clear();
                    ddlPerThanaCode.DataSource = StudentManager.GetAllThanaForSpecificDistric(ddlPerDistCode.SelectedValue);
                    ddlPerThanaCode.DataTextField = "THANA_NAME";
                    ddlPerThanaCode.DataValueField = "THANA_CODE";
                    ddlPerThanaCode.DataBind();

                }
                txtPerZipCode.Text = aStudentObj.PerZipCode;
                txtMailLoc.Text = aStudentObj.MailLoc;
                if (aStudentObj.MailDistCode != "" && aStudentObj.MailDistCode != null)
                {
                    ddlMailDistCode.SelectedValue = aStudentObj.MailDistCode;
                }
                if (aStudentObj.MailThanaCode != "" && aStudentObj.MailThanaCode != null)
                {
                    ddlMailThanaCode.SelectedValue = aStudentObj.MailThanaCode;
                    ddlMailThanaCode.Items.Clear();
                    ddlMailThanaCode.DataSource = StudentManager.GetAllThanaForSpecificDistric(ddlMailDistCode.SelectedValue);
                    ddlMailThanaCode.DataTextField = "THANA_NAME";
                    ddlMailThanaCode.DataValueField = "THANA_CODE";
                    ddlMailThanaCode.DataBind();
                }
                //string mdistrict = StudentManager.GetDistrict(aStudentObj.MailDistCode);
                //string mthana = StudentManager.GetThana(aStudentObj.MailThanaCode);

                txtMailZipCode.Text = aStudentObj.MailZipCode;
                txtEmail.Text = aStudentObj.Email;
                txtSpousName.Text = aStudentObj.TelNo;
                txtMobileNo.Text = aStudentObj.MobileNo;
                txtFthName.Text = aStudentObj.FthName;
                //txtFthEdu.Text = aStudentObj.FthEdu;

                //ddlFthOccup.SelectedValue = aStudentObj.FthOccup;
                //txtFthOrg.Text = aStudentObj.FthOrg;
                txtFthTel.Text = aStudentObj.FthTel;
                //txtFthOthAct.Text = aStudentObj.FthOthAct;
                txtMthName.Text = aStudentObj.MthName;
                //txtMthEdu.Text = aStudentObj.MthEdu;

                //ddlMthOccup.SelectedValue = aStudentObj.MthOccup;
                //txtMthOrg.Text = aStudentObj.MthOrg;
                txtMthTel.Text = aStudentObj.MthTel;
                //txtMthOthAct.Text = aStudentObj.MthOthAct;
                //txtContPerson.Text = aStudentObj.ContPerson;
                //txtContAddress.Text = aStudentObj.ContAddress;
                //txtContPhone.Text = aStudentObj.ContPhone;
                //txtContMobile.Text = aStudentObj.ContMobile;

                //ddlContRelate.SelectedValue = aStudentObj.ContRelate;
                //txtChildRcv1.Text = aStudentObj.ChildRcv1;
                //txtChildRcv2.Text = aStudentObj.ChildRcv2;
                //txtChildRcv3.Text = aStudentObj.ChildRcv3;
                //txtChildRcv4.Text = aStudentObj.ChildRcv4;

                //ddlRelRcv1.SelectedValue = aStudentObj.RelRcv1;
                //ddlRelRcv2.SelectedValue = aStudentObj.RelRcv2;
                //ddlRelRcv3.SelectedValue = aStudentObj.RelRcv3;
                //ddlRelRcv4.SelectedValue = aStudentObj.RelRcv4;
                //txtPrevSch.Text = aStudentObj.PrevSch;
                //txtPrevAdd.Text = aStudentObj.PrevAdd;
                //txtLastClass.Text = aStudentObj.LastClass;
                //txtClassYear.Text = aStudentObj.ClassYear;
                //txtClassPos.Text = aStudentObj.ClassPos;
                //txtReasonLeave.Text = aStudentObj.ReasonLeave;
                //txtPhysicProb.Text = aStudentObj.PhysicProb;
                //txtAllergic.Text = aStudentObj.Allergic;
                
                ddlStatus.SelectedValue = aStudentObj.Status;
                //TxtAdmissionDate.Text = aStudentObj.AdmissionDt;
                //if (aStudentObj.InactiveDate == "" || aStudentObj.InactiveDate == null)
                //{
                //    ArchiveLabel.Visible = false;
                //    txtInactiveDate.Visible = false;
                //}
                //else
                //{
                //    ArchiveLabel.Visible = true;
                //    txtInactiveDate.Visible = true;
                //    txtInactiveDate.Text = aStudentObj.InactiveDate;                    
                //}
                if (aStudentObj.StdPhoto.Length > 4)
                {
                    byte[] studentImage = aStudentObj.StdPhoto;
                    ViewState["stdPhoto"] = studentImage;
                    MemoryStream ms = new MemoryStream(studentImage);
                    System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
                    Session["byt"] = studentImage;
                    string base64String = Convert.ToBase64String(studentImage, 0, studentImage.Length);
                    imgStd.ImageUrl = "data:image/jpeg;base64," + base64String;
                }

                //if (aStudentObj.StdCurPhoto.Length > 4)
                //{
                //    byte[] stdCurrentImage = aStudentObj.StdCurPhoto;
                //    ViewState["std_cur_photo"] = stdCurrentImage;
                //    MemoryStream ms1 = new MemoryStream(stdCurrentImage);
                //    System.Drawing.Image img1 = System.Drawing.Image.FromStream(ms1);
                //    string base64String1 = Convert.ToBase64String(stdCurrentImage, 0, stdCurrentImage.Length);
                //    imgStdCur.ImageUrl = "data:image/jpeg;base64," + base64String1;

                //}

                //if (aStudentObj.FthPhoto.Length > 4)
                //{
                //    byte[] fthImage = aStudentObj.FthPhoto;
                //    ViewState["fthPhoto"] = fthImage;
                //    MemoryStream ms2 = new MemoryStream(fthImage);
                //    System.Drawing.Image img2 = System.Drawing.Image.FromStream(ms2);
                //    string base64String2 = Convert.ToBase64String(fthImage, 0, fthImage.Length);
                //    imgFth.ImageUrl = "data:image/jpeg;base64," + base64String2;

                //}

                //if (aStudentObj.MthPhoto.Length > 4)
                //{
                //    byte[] mthImage = aStudentObj.MthPhoto;
                //    ViewState["mthPhoto"] = mthImage;
                //    MemoryStream ms3 = new MemoryStream(mthImage);
                //    System.Drawing.Image img3 = System.Drawing.Image.FromStream(ms3);
                //    string base64String3 = Convert.ToBase64String(mthImage, 0, mthImage.Length);
                //    imgMth.ImageUrl = "data:image/jpeg;base64," + base64String3;

                //}

                //if (aStudentObj.GuardPhoto1.Length > 4)
                //{
                //    byte[] grdImage = aStudentObj.GuardPhoto1;
                //    ViewState["guard1Photo"] = grdImage;
                //    MemoryStream ms4 = new MemoryStream(grdImage);
                //    System.Drawing.Image img4 = System.Drawing.Image.FromStream(ms4);
                //    string base64String4 = Convert.ToBase64String(grdImage, 0, grdImage.Length);
                //    imgGuard1.ImageUrl = "data:image/jpeg;base64," + base64String4;
                //}

                //if (aStudentObj.GuardPhoto2.Length > 4)
                //{
                //    byte[] grdImage1 = aStudentObj.GuardPhoto2;
                //    ViewState["guard2Photo"] = grdImage1;
                //    MemoryStream ms5 = new MemoryStream(grdImage1);
                //    System.Drawing.Image img5 = System.Drawing.Image.FromStream(ms5);
                //    string base64String5 = Convert.ToBase64String(grdImage1, 0, grdImage1.Length);
                //    imgGuard2.ImageUrl = "data:image/jpeg;base64," + base64String5;
                //}

                //if (aStudentObj.GuardPhoto3.Length > 4)
                //{
                //    byte[] grdImage2 = aStudentObj.GuardPhoto3;
                //    ViewState["guard3Photo"] = grdImage2;
                //    MemoryStream ms6 = new MemoryStream(grdImage2);
                //    System.Drawing.Image img6 = System.Drawing.Image.FromStream(ms6);
                //    string base64String6 = Convert.ToBase64String(grdImage2, 0, grdImage2.Length);
                //    imgGuard3.ImageUrl = "data:image/jpeg;base64," + base64String6;
                //}

                //if (aStudentObj.GuardPhoto4.Length > 4)
                //{
                //    byte[] grdImage3 = aStudentObj.GuardPhoto4;
                //    ViewState["guard4Photo"] = grdImage3;
                //    MemoryStream ms7 = new MemoryStream(grdImage3);
                //    System.Drawing.Image img7 = System.Drawing.Image.FromStream(ms7);
                //    string base64String7 = Convert.ToBase64String(grdImage3, 0, grdImage3.Length);
                //    imgGuard4.ImageUrl = "data:image/jpeg;base64," + base64String7;
                //}

                //byte[] currentStdPhoto = aStudentObj.StdCurPhoto;
                //ViewState["std_cur_photo"] = currentStdPhoto;
                //byte[] fatherPhoto = aStudentObj.FthPhoto;
                //ViewState["fthPhoto"] = aStudentObj.FthPhoto;
                //byte[] motherPhoto = aStudentObj.MthPhoto;
                //ViewState["mthPhoto"] = aStudentObj.MthPhoto;
                //byte[] gurdian1 = aStudentObj.GuardPhoto1;
                //ViewState["guard1Photo"] = aStudentObj.GuardPhoto1;
                //byte[] gurdian2 = aStudentObj.GuardPhoto2;
                //ViewState["guard2Photo"] = aStudentObj.GuardPhoto2;
                //byte[] gurdian3 = aStudentObj.GuardPhoto3;
                //ViewState["guard3Photo"] = aStudentObj.GuardPhoto3;
                //byte[] gurdian4 = aStudentObj.GuardPhoto4;
                //ViewState["guard4Photo"] = aStudentObj.GuardPhoto4;



                //ShowImage(studentImage, imgStd);
                //ShowImage(currentStdPhoto, imgStdCur);
                //ShowImage(fatherPhoto, imgFth);
                //ShowImage(motherPhoto, imgMth);
                //ShowImage(gurdian1, imgGuard1);
                //ShowImage(gurdian2, imgGuard2);
                //ShowImage(gurdian3, imgGuard3);
                //ShowImage(gurdian4, imgGuard4);

                DataTable familtyDetails = FamManager.GetStdFamInformationForSpecificStudent(txtStudentId.Text);
                //dgEmpFam.DataSource = familtyDetails;
                //dgEmpFam.DataBind();
                ViewState["dtfam"] = familtyDetails;
                clsStdCurrentStatus stdObj = clsStdCurrentStatusManager.GetAllStdCurrentStatusForSpecificStd(txtStudentId.Text);
                //if (stdObj.ClassId != "" && stdObj.ClassId != null)
                //{
                //    ddlCurClass.SelectedValue = stdObj.ClassId;
                //}
                //ddlSect.DataSource = StudentManager.GetShowSectionOnClass(ddlCurClass.SelectedValue);
                //ddlSect.DataTextField = "sec_name";
                //ddlSect.DataValueField = "sec_id";
                //ddlSect.DataBind();
                //ddlSect.Items.Insert(0, new System.Web.UI.WebControls.ListItem(""));
                //ddlSect.SelectedValue = stdObj.Sect;

                //ddlVersion.DataSource = StudentManager.GetShowVersionOnClass(ddlCurClass.SelectedValue);
                //ddlVersion.DataTextField = "version_name";
                //ddlVersion.DataValueField = "version_id";
                //ddlVersion.DataBind();
                //ddlVersion.Items.Insert(0, new System.Web.UI.WebControls.ListItem(""));
                //ddlVersion.SelectedValue = stdObj.version;

                //ddlShift.DataSource = StudentManager.GetShowShiftOnClass(ddlCurClass.SelectedValue);
                //ddlShift.DataTextField = "shift_name";
                //ddlShift.DataValueField = "shift_id";
                //ddlShift.DataBind();
                //ddlShift.Items.Insert(0, new System.Web.UI.WebControls.ListItem(""));
                //ddlShift.SelectedValue = stdObj.Shift;

                ////ddlCurClass.SelectedValue = stdObj.ClassId;

                //ddlBloodGroup.SelectedValue = aStudentObj.BloodGroup;              
                //txtCurYear.Text = stdObj.ClassYear;
                //txtRoll.Text = stdObj.Roll;
                //txtClassStart.Text = stdObj.ClassStart;
                //TxtDoa.Text = stdObj.AdmissionDt;
                
            }
            txtStudentId.Enabled = false;
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
    protected void dgStd_PageIndexChanging(object sender, GridViewPageEventArgs e)   
    {
        try
        {
           
                DataTable dt = StudentManager.getStds(txtStudentId.Text, (txtFName.Text).ToString().Trim(), txtBirthDt.Text);
                
                HistoryGridView.PageIndex = e.NewPageIndex;
                HistoryGridView.DataSource = dt;
                HistoryGridView.DataBind();
         
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
    protected void dgStd_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Student std = StudentManager.getStd(HistoryGridView.SelectedRow.Cells[1].Text.Trim());
            if (std != null)
            {
                //stdPhoto = (byte[])std.StdPhoto;
                //if (stdPhoto.Length > 0)
                //{
                //    imgStd.ImageUrl = "~/getImage.ashx?studentid='" + std.StudentId + "'";
                //    stdPhoto = (byte[])std.StdPhoto;
                //}
                //else
                //{
                //    imgStd.ImageUrl = "~/tmpHandler.ashx?filename=img/noimage.jpg";
                //}
                //ddlPerDistCode.SelectedValue = std.PerDistCode;
                //ddlPerThanaCode.Items.Clear();
                //string queryPThana = "select '' th_code, '' th_name  union select thana_code, thana_name from thana_code where district_code like '" + ddlPerDistCode.SelectedValue + "' order by 2 desc";
                //util.PopulationDropDownList(ddlPerThanaCode, "Thana", queryPThana, "th_name", "th_code");
                //ddlPerThanaCode.SelectedValue = std.PerThanaCode;

                //ddlMailDistCode.SelectedValue = std.MailDistCode;
                //ddlMailThanaCode.Items.Clear();
                //string queryMThana = "select '' th_code, '' th_name  union select thana_code, thana_name from thana_code where district_code like '" + ddlMailDistCode.SelectedValue + "' order by 2 desc";
                //util.PopulationDropDownList(ddlMailThanaCode, "Thana", queryMThana, "th_name", "th_code");
                //ddlMailThanaCode.SelectedValue = std.MailThanaCode;
                clearFields();
                txtStudentId.Text = std.StudentId;
                BtnFind_Click(sender, e);

                HistoryGridView.Visible = false;
                StdTabContainer.Visible = true;

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
    //protected void dgEmpFam_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    //{
    //    try
    //    {
    //        if (ViewState["dtfam"] != null)
    //        {
    //            FamSt = "O";
    //            DataTable dt = (DataTable)ViewState["dtfam"];
    //            if (dt.Rows.Count > 1)
    //            {
    //                dgEmpFam.DataSource = dt;
    //                dgEmpFam.EditIndex = -1;
    //                dgEmpFam.ShowFooter = false;
    //                dgEmpFam.DataBind();
    //            }
    //            else if (dt.Rows.Count == 1 && ((DataRow)dt.Rows[0])["rel_name"].ToString() != "")
    //            {
    //                dgEmpFam.DataSource = dt;
    //                dgEmpFam.EditIndex = -1;
    //                dgEmpFam.ShowFooter = false;
    //                dgEmpFam.DataBind();
    //            }
    //            else
    //            {
    //                getEmptyFam();
    //            }
    //        }
    //    }
    //    catch (FormatException fex)
    //    {
    //        ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('" + fex.Message + "');", true);
    //    }
    //    catch (Exception ex)
    //    {
    //        if (ex.Message.Contains("Database"))
    //            ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Database Maintain Error. Contact to the Software Provider..!!');", true);
    //        else
    //            ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('There is some problem to do the task. Try again properly.!!');", true);
    //    }
    //}
    //protected void dgEmpFam_RowDeleting(object sender, GridViewDeleteEventArgs e)
    //{
    //    try
    //    {
    //        if (ViewState["dtfam"] != null)
    //        {
    //            FamSt = "O";
    //            DataTable dt = (DataTable)ViewState["dtfam"];
    //            DataRow dr = dt.Rows[dgEmpFam.Rows[e.RowIndex].DataItemIndex];
    //            dt.Rows.Remove(dr);
    //            if (dt.Rows.Count > 1)
    //            {
    //                dgEmpFam.DataSource = dt;
    //                dgEmpFam.EditIndex = -1;
    //                dgEmpFam.ShowFooter = false;
    //                dgEmpFam.DataBind();
    //            }
    //            else if (dt.Rows.Count == 1 && ((DataRow)dt.Rows[0])["rel_name"].ToString() != "")
    //            {
    //                dgEmpFam.DataSource = dt;
    //                dgEmpFam.EditIndex = -1;
    //                dgEmpFam.ShowFooter = false;
    //                dgEmpFam.DataBind();
    //            }
    //            else
    //            {
    //                getEmptyFam();
    //            }
    //        }
    //    }
    //    catch (FormatException fex)
    //    {
    //        ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('" + fex.Message + "');", true);
    //    }
    //    catch (Exception ex)
    //    {
    //        if (ex.Message.Contains("Database"))
    //            ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Database Maintain Error. Contact to the Software Provider..!!');", true);
    //        else
    //            ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('There is some problem to do the task. Try again properly.!!');", true);
    //    }
    //    //updatepanelEmpNo.Update();
    //    //ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Record is deleted provisionally, to delete from database permanently please click on Save Link!!');", true);
    //}
    //protected void dgEmpFam_RowEditing(object sender, GridViewEditEventArgs e)
    //{
    //    try
    //    {
    //        if (ViewState["dtfam"] != null)
    //        {
    //            FamSt = "O";
    //            DataTable dt = (DataTable)ViewState["dtfam"];
    //            string name = ((Label)dgEmpFam.Rows[e.NewEditIndex].FindControl("lblRelName")).Text;
    //            string rel = ((Label)dgEmpFam.Rows[e.NewEditIndex].FindControl("lblRelation")).Text;
    //            string dob = ((Label)dgEmpFam.Rows[e.NewEditIndex].FindControl("lblBirthDt")).Text;
    //            string age = ((Label)dgEmpFam.Rows[e.NewEditIndex].FindControl("lblAge")).Text;
    //            string occu = ((Label)dgEmpFam.Rows[e.NewEditIndex].FindControl("lblOccupation")).Text;
    //            dgEmpFam.DataSource = dt;
    //            dgEmpFam.EditIndex = e.NewEditIndex;
    //            dgEmpFam.DataBind();
    //            ((TextBox)dgEmpFam.Rows[e.NewEditIndex].FindControl("txtRelName")).Text = name;
    //            ((DropDownList)dgEmpFam.Rows[e.NewEditIndex].FindControl("ddlRelation")).SelectedItem.Text = rel;
    //            ((TextBox)dgEmpFam.Rows[e.NewEditIndex].FindControl("txtBirthDt")).Text = dob;
    //            ((TextBox)dgEmpFam.Rows[e.NewEditIndex].FindControl("txtAge")).Text = age;
    //            ((TextBox)dgEmpFam.Rows[e.NewEditIndex].FindControl("txtOccupation")).Text = occu;
    //            ((TextBox)dgEmpFam.Rows[e.NewEditIndex].FindControl("txtBirthDt")).Attributes.Add("onBlur", "formatdate('" + ((TextBox)dgEmpFam.Rows[e.NewEditIndex].FindControl("txtBirthDt")).ClientID + "')");
    //        }
    //    }
    //    catch (FormatException fex)
    //    {
    //        ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('" + fex.Message + "');", true);
    //    }
    //    catch (Exception ex)
    //    {
    //        if (ex.Message.Contains("Database"))
    //            ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Database Maintain Error. Contact to the Software Provider..!!');", true);
    //        else
    //            ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('There is some problem to do the task. Try again properly.!!');", true);
    //    }
    //}
    //protected void dgEmpFam_RowUpdating(object sender, GridViewUpdateEventArgs e)
    //{
    //    try
    //    {
    //        if (ViewState["dtfam"] != null)
    //        {
    //            FamSt = "O";
    //            GridViewRow gvr = dgEmpFam.Rows[e.RowIndex];
    //            DataTable dt = (DataTable)ViewState["dtfam"];
    //            DataRow dr = dt.Rows[gvr.DataItemIndex];
    //            dr["rel_name"] = ((TextBox)gvr.FindControl("txtRelName")).Text;
    //            dr["relation"] = ((DropDownList)gvr.FindControl("ddlRelation")).SelectedItem.Text;
    //            dr["birth_dt"] = ((TextBox)gvr.FindControl("txtBirthDt")).Text;
    //            dr["age"] = ((TextBox)gvr.FindControl("txtAge")).Text;
    //            dr["occupation"] = ((TextBox)gvr.FindControl("txtOccupation")).Text;
    //            dgEmpFam.DataSource = dt;
    //            dgEmpFam.EditIndex = -1;
    //            dgEmpFam.DataBind();
    //            //updatepanelEmpNo.Update();
    //            //ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Record is updated provisionally, to update in database permanently please click on Save Link!!');", true);
    //        }
    //    }
    //    catch (FormatException fex)
    //    {
    //        ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('" + fex.Message + "');", true);
    //    }
    //    catch (Exception ex)
    //    {
    //        if (ex.Message.Contains("Database"))
    //            ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Database Maintain Error. Contact to the Software Provider..!!');", true);
    //        else
    //            ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('There is some problem to do the task. Try again properly.!!');", true);
    //    }
    //}
    //protected void dgEmpFam_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    try
    //    {
    //        if (e.Row.RowType == DataControlRowType.DataRow)
    //        {
    //            if (((DataRowView)e.Row.DataItem)["rel_name"].ToString() == "")
    //            {
    //                e.Row.Visible = false;
    //            }
    //        }
    //    }
    //    catch (FormatException fex)
    //    {
    //        ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('" + fex.Message + "');", true);
    //    }
    //    catch (Exception ex)
    //    {
    //        if (ex.Message.Contains("Database"))
    //            ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Database Maintain Error. Contact to the Software Provider..!!');", true);
    //        else
    //            ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('There is some problem to do the task. Try again properly.!!');", true);
    //    }
    //}
    //protected void dgEmpFam_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    try
    //    {
    //        if (ViewState["dtfam"] != null)
    //        {
    //            FamSt = "O";
    //            DataTable dt = (DataTable)ViewState["dtfam"];
    //            if (e.CommandName.Equals("New"))
    //            {
    //                dgEmpFam.DataSource = dt;
    //                dgEmpFam.ShowFooter = true;
    //                dgEmpFam.FooterRow.Visible = true;
    //                dgEmpFam.DataBind();
    //                ((TextBox)dgEmpFam.FooterRow.FindControl("txtBirthDt")).Attributes.Add("onBlur", "formatdate('" + ((TextBox)dgEmpFam.FooterRow.FindControl("txtBirthDt")).ClientID + "')");
    //            }
    //            else if (e.CommandName.Equals("Insert"))
    //            {
    //                DataRow dr = dt.NewRow();
    //                dr["rel_name"] = ((TextBox)dgEmpFam.FooterRow.FindControl("txtRelName")).Text;
    //                dr["relation"] = ((DropDownList)dgEmpFam.FooterRow.FindControl("ddlRelation")).SelectedItem.Text;
    //                dr["birth_dt"] = ((TextBox)dgEmpFam.FooterRow.FindControl("txtBirthDt")).Text;
    //                dr["age"] = ((TextBox)dgEmpFam.FooterRow.FindControl("txtAge")).Text;
    //                dr["occupation"] = ((TextBox)dgEmpFam.FooterRow.FindControl("txtOccupation")).Text;
    //                dt.Rows.Add(dr);
    //                dgEmpFam.DataSource = dt;
    //                dgEmpFam.ShowFooter = false;
    //                dgEmpFam.DataBind();
    //                //updatepanelEmpNo.Update();
    //                //ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Record is inserted provisionally, to save in database permanently please click on Save Link!!');", true);
    //            }
    //        }
    //    }
    //    catch (FormatException fex)
    //    {
    //        ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('" + fex.Message + "');", true);
    //    }
    //    catch (Exception ex)
    //    {
    //        if (ex.Message.Contains("Database"))
    //            ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Database Maintain Error. Contact to the Software Provider..!!');", true);
    //        else
    //            ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('There is some problem to do the task. Try again properly.!!');", true);
    //    }
    //}
    
   
    protected void lbImgUpload_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtFName.Text != "" && imgUpload.HasFile)
            {
                int width = 145;
                int height = 165;
                byte[] stdphoto;
                byte[] std_cur_photo;
                using (System.Drawing.Bitmap img = StudentManager.ResizeImage(new System.Drawing.Bitmap(imgUpload.PostedFile.InputStream), width, height, StudentManager.ResizeOptions.ExactWidthAndHeight))
                {
                    imgUpload.PostedFile.InputStream.Close();
                    stdphoto = StudentManager.ConvertImageToByteArray(img, System.Drawing.Imaging.ImageFormat.Png);
                    std_cur_photo = StudentManager.ConvertImageToByteArray(img, System.Drawing.Imaging.ImageFormat.Png);
                    ViewState["stdPhoto"] = stdphoto;
                    ViewState["std_cur_photo"] = std_cur_photo;
                    img.Dispose();
                }
                string base64String = Convert.ToBase64String(stdphoto, 0, stdphoto.Length);
                imgStd.ImageUrl = "data:image/png;base64," + base64String;

                string base64String1 = Convert.ToBase64String(std_cur_photo, 0, std_cur_photo.Length);
                //imgStdCur.ImageUrl = "data:image/png;base64," + base64String1;
            }
           
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Please input student campus, semester, admission date and then browse a photograph image!!');", true);
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
    //protected void lbImgUploadFth_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        //if (imgUploadFth.HasFile)
    //        //{
    //        //    int width = 145;
    //        //    int height = 165;
    //        //    byte[] fthPhoto;
    //        //    using (System.Drawing.Bitmap img = StudentManager.ResizeImage(new System.Drawing.Bitmap(imgUploadFth.PostedFile.InputStream), width, height, StudentManager.ResizeOptions.ExactWidthAndHeight))
    //        //    {
    //        //        imgUploadFth.PostedFile.InputStream.Close();
    //        //        fthPhoto = StudentManager.ConvertImageToByteArray(img, System.Drawing.Imaging.ImageFormat.Png);
    //        //        ViewState["fthPhoto"] = fthPhoto;
    //        //        img.Dispose();
    //        //    }
    //        //    string base64String = Convert.ToBase64String(fthPhoto, 0, fthPhoto.Length);
    //        //    imgFth.ImageUrl = "data:image/png;base64," + base64String;

    //        //}
    //        //else
    //        //{
    //        //    ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Please input student admission date and then browse a photograph image!!');", true);
    //        //}
    //    }
    //    catch (FormatException fex)
    //    {
    //        ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('" + fex.Message + "');", true);
    //    }
    //    catch (Exception ex)
    //    {
    //        if (ex.Message.Contains("Database"))
    //            ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Database Maintain Error. Contact to the Software Provider..!!');", true);
    //        else
    //            ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('There is some problem to do the task. Try again properly.!!');", true);
    //    }
    //}
    //protected void lbImgUploadStdCur_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        //if (imgUploadStdCur.HasFile)
    //        //{
    //        //    int width = 145;
    //        //    int height = 165;
    //        //    byte[] std_cur_photo;
    //        //    using (System.Drawing.Bitmap img = StudentManager.ResizeImage(new System.Drawing.Bitmap(imgUploadStdCur.PostedFile.InputStream), width, height, StudentManager.ResizeOptions.ExactWidthAndHeight))
    //        //    {
    //        //        imgUploadStdCur.PostedFile.InputStream.Close();
    //        //        std_cur_photo = StudentManager.ConvertImageToByteArray(img, System.Drawing.Imaging.ImageFormat.Png);
    //        //        ViewState["std_cur_photo"] = std_cur_photo;
    //        //        img.Dispose();
    //        //    }
    //        //    string base64String = Convert.ToBase64String(std_cur_photo, 0, std_cur_photo.Length);
    //        //    imgStdCur.ImageUrl = "data:image/png;base64," + base64String;
    //        //}
    //        //else
    //        //{
    //        //    ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Please input student admission date and then browse a photograph image!!');", true);
    //        //}
    //    }
    //    catch (FormatException fex)
    //    {
    //        ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('" + fex.Message + "');", true);
    //    }
    //    catch (Exception ex)
    //    {
    //        if (ex.Message.Contains("Database"))
    //            ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Database Maintain Error. Contact to the Software Provider..!!');", true);
    //        else
    //            ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('There is some problem to do the task. Try again properly.!!');", true);
    //    }
    //}
    //protected void lbImgUploadMth_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        //if (imgUploadMth.HasFile)
    //        //{
    //        //    int width = 145;
    //        //    int height = 165;
    //        //    byte[] mthPhoto;
    //        //    using (System.Drawing.Bitmap img = StudentManager.ResizeImage(new System.Drawing.Bitmap(imgUploadMth.PostedFile.InputStream), width, height, StudentManager.ResizeOptions.ExactWidthAndHeight))
    //        //    {
    //        //        imgUploadMth.PostedFile.InputStream.Close();
    //        //        mthPhoto = StudentManager.ConvertImageToByteArray(img, System.Drawing.Imaging.ImageFormat.Png);
    //        //        ViewState["mthPhoto"] = mthPhoto;
    //        //        img.Dispose();
    //        //    }
    //        //    string base64String = Convert.ToBase64String(mthPhoto, 0, mthPhoto.Length);
    //        //    imgMth.ImageUrl = "data:image/png;base64," + base64String;
    //        //}


    //        //else
    //        //{
    //        //    ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Please input student admission date and then browse a photograph image!!');", true);
    //        //}
    //    }
    //    catch (FormatException fex)
    //    {
    //        ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('" + fex.Message + "');", true);
    //    }
    //    catch (Exception ex)
    //    {
    //        if (ex.Message.Contains("Database"))
    //            ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Database Maintain Error. Contact to the Software Provider..!!');", true);
    //        else
    //            ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('There is some problem to do the task. Try again properly.!!');", true);
    //    }
    //}
    //protected void lbImgUploadGuard1_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        //if (imgUploadGuard1.HasFile)
    //        //{
    //        //    int width = 145;
    //        //    int height = 165;
    //        //    byte[] guard1Photo;
    //        //    using (System.Drawing.Bitmap img = StudentManager.ResizeImage(new System.Drawing.Bitmap(imgUploadGuard1.PostedFile.InputStream), width, height, StudentManager.ResizeOptions.ExactWidthAndHeight))
    //        //    {
    //        //        imgUploadGuard1.PostedFile.InputStream.Close();
    //        //        guard1Photo = StudentManager.ConvertImageToByteArray(img, System.Drawing.Imaging.ImageFormat.Png);
    //        //        ViewState["guard1Photo"] = guard1Photo;
    //        //        img.Dispose();
    //        //    }
    //        //    string base64String = Convert.ToBase64String(guard1Photo, 0, guard1Photo.Length);
    //        //    imgGuard1.ImageUrl = "data:image/png;base64," + base64String;
    //        //}


    //        //else
    //        //{
    //        //    ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Please input student admission date and then browse a photograph image!!');", true);
    //        //}
    //    }
    //    catch (FormatException fex)
    //    {
    //        ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('" + fex.Message + "');", true);
    //    }
    //    catch (Exception ex)
    //    {
    //        if (ex.Message.Contains("Database"))
    //            ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Database Maintain Error. Contact to the Software Provider..!!');", true);
    //        else
    //            ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('There is some problem to do the task. Try again properly.!!');", true);
    //    }
    //}
    //protected void lbImgUploadGuard2_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        //if (imgUploadGuard2.HasFile)
    //        //{
    //        //    int width = 145;
    //        //    int height = 165;
    //        //    byte[] guard2Photo;
    //        //    using (System.Drawing.Bitmap img = StudentManager.ResizeImage(new System.Drawing.Bitmap(imgUploadGuard2.PostedFile.InputStream), width, height, StudentManager.ResizeOptions.ExactWidthAndHeight))
    //        //    {
    //        //        imgUploadGuard2.PostedFile.InputStream.Close();
    //        //        guard2Photo = StudentManager.ConvertImageToByteArray(img, System.Drawing.Imaging.ImageFormat.Png);
    //        //        ViewState["guard2Photo"] = guard2Photo;
    //        //        img.Dispose();
    //        //    }
    //        //    string base64String = Convert.ToBase64String(guard2Photo, 0, guard2Photo.Length);
    //        //    imgGuard2.ImageUrl = "data:image/png;base64," + base64String;
    //        //}

    //        //else
    //        //{
    //        //    ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Please input student admission date and then browse a photograph image!!');", true);
    //        //}
    //    }
    //    catch (FormatException fex)
    //    {
    //        ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('" + fex.Message + "');", true);
    //    }
    //    catch (Exception ex)
    //    {
    //        if (ex.Message.Contains("Database"))
    //            ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Database Maintain Error. Contact to the Software Provider..!!');", true);
    //        else
    //            ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('There is some problem to do the task. Try again properly.!!');", true);
    //    }
    //}
    //protected void lbImgUploadGuard3_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        //if (imgUploadGuard3.HasFile)
    //        //{
    //        //    int width = 145;
    //        //    int height = 165;
    //        //    byte[] guard3Photo;
    //        //    using (System.Drawing.Bitmap img = StudentManager.ResizeImage(new System.Drawing.Bitmap(imgUploadGuard3.PostedFile.InputStream), width, height, StudentManager.ResizeOptions.ExactWidthAndHeight))
    //        //    {
    //        //        imgUploadGuard3.PostedFile.InputStream.Close();
    //        //        guard3Photo = StudentManager.ConvertImageToByteArray(img, System.Drawing.Imaging.ImageFormat.Png);
    //        //        ViewState["guard3Photo"] = guard3Photo;
    //        //        img.Dispose();
    //        //    }
    //        //    string base64String = Convert.ToBase64String(guard3Photo, 0, guard3Photo.Length);
    //        //    imgGuard3.ImageUrl = "data:image/png;base64," + base64String;
    //        //}

    //        //else
    //        //{
    //        //    ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Please input student admission date and then browse a photograph image!!');", true);
    //        //}
    //    }
    //    catch (FormatException fex)
    //    {
    //        ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('" + fex.Message + "');", true);
    //    }
    //    catch (Exception ex)
    //    {
    //        if (ex.Message.Contains("Database"))
    //            ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Database Maintain Error. Contact to the Software Provider..!!');", true);
    //        else
    //            ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('There is some problem to do the task. Try again properly.!!');", true);
    //    }
    //}
    //protected void lbImgUploadGuard4_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        //if (imgUploadGuard4.HasFile)
    //        //{
    //        //    int width = 145;
    //        //    int height = 165;
    //        //    byte[] guard4Photo;
    //        //    using (System.Drawing.Bitmap img = StudentManager.ResizeImage(new System.Drawing.Bitmap(imgUploadGuard4.PostedFile.InputStream), width, height, StudentManager.ResizeOptions.ExactWidthAndHeight))
    //        //    {
    //        //        imgUploadGuard4.PostedFile.InputStream.Close();
    //        //        guard4Photo = StudentManager.ConvertImageToByteArray(img, System.Drawing.Imaging.ImageFormat.Png);
    //        //        ViewState["guard4Photo"] = guard4Photo;
    //        //        img.Dispose();
    //        //    }
    //        //    string base64String = Convert.ToBase64String(guard4Photo, 0, guard4Photo.Length);
    //        //    imgGuard4.ImageUrl = "data:image/png;base64," + base64String;
    //        //}


    //        //else
    //        //{
    //        //    ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Please input student admission date and then browse a photograph image!!');", true);
    //        //}
    //    }
    //    catch (FormatException fex)
    //    {
    //        ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('" + fex.Message + "');", true);
    //    }
    //    catch (Exception ex)
    //    {
    //        if (ex.Message.Contains("Database"))
    //            ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Database Maintain Error. Contact to the Software Provider..!!');", true);
    //        else
    //            ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('There is some problem to do the task. Try again properly.!!');", true);
    //    }
    //}
    private string getId(string cam,string sem,string adm)
    {
       
        string sl = IdManager.GetNextSlStd().ToString().PadLeft(5, '0');
        //return cam + sem + DateTime.Parse(adm).Year.ToString().Substring(2) + sl;
        return txtStudentId.Text;
        
    }
    private void clearFields()
    {
        //imgStd.ImageUrl = "~/tmpHandler.ashx?filename=img/noimage.jpg";
        //imgStdCur.ImageUrl = "~/tmpHandler.ashx?filename=img/noimage.jpg";
        //imgFth.ImageUrl = "~/tmpHandler.ashx?filename=img/noimage.jpg";
        //imgMth.ImageUrl = "~/tmpHandler.ashx?filename=img/noimage.jpg";
        //imgGuard1.ImageUrl = "~/tmpHandler.ashx?filename=img/noimage.jpg";
        //imgGuard2.ImageUrl = "~/tmpHandler.ashx?filename=img/noimage.jpg";
        //imgGuard3.ImageUrl = "~/tmpHandler.ashx?filename=img/noimage.jpg";
        //imgGuard4.ImageUrl = "~/tmpHandler.ashx?filename=img/noimage.jpg";
    }

    protected void HistoryGridView_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            RefreshAll();
            Student std = StudentManager.getStd(HistoryGridView.SelectedRow.Cells[1].Text.Trim());
            if (std != null)
            {

                txtStudentId.Text = std.StudentId.Trim();
                BtnFind_Click(sender, e);
                
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

   

    private void ShowImage(byte[] studentImage,System.Web.UI.WebControls.Image imgStd1)
    {
        try
        {
            if (studentImage != null)
            {
                MemoryStream ms = new MemoryStream(studentImage);
                System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
                Session["byt"] = studentImage;
                string base64String = Convert.ToBase64String(studentImage, 0, studentImage.Length);
                imgStd1.ImageUrl = "data:image/jpeg;base64," + base64String;
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
    protected void HistoryGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            if (ddlCourseName.SelectedValue == "")
            {
                DataTable dt = StudentManager.GetAllStudentSearchBySessionYear(ddlCourseName.SelectedValue);
                HistoryGridView.DataSource = Session["Class"];
                //HistoryGridView.DataSource = dt;
                HistoryGridView.PageIndex = e.NewPageIndex;
                HistoryGridView.DataBind();
            }
            else
            {
                DataTable dt = StudentManager.GetAllStudentInformation();
                HistoryGridView.DataSource = dt;
                HistoryGridView.PageIndex = e.NewPageIndex;
                HistoryGridView.DataBind();
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
    protected void ClassSelectDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlCourseName.DataSource = StudentManager.GetVersionOnClass("");
            ddlCourseName.DataTextField = "version_name";
            ddlCourseName.DataValueField = "id";
            ddlCourseName.DataBind();
            //VersionSelectDropdownList.Items.Insert(0, new System.Web.UI.WebControls.ListItem(""));

            //ShiftSelectDropdownLift.DataSource = StudentManager.GetShiftOnClass(ClassSelectDropDownList.SelectedValue);
            //ShiftSelectDropdownLift.DataTextField = "shift_name";
            //ShiftSelectDropdownLift.DataValueField = "id";
            //ShiftSelectDropdownLift.DataBind();
            //ShiftSelectDropdownLift.Items.Insert(0, new System.Web.UI.WebControls.ListItem(""));

            //SectionSelectDropDownList.DataSource = StudentManager.GetShowSectionOnClass(ClassSelectDropDownList.SelectedValue);
            //SectionSelectDropDownList.DataTextField = "sec_name";
            //SectionSelectDropDownList.DataValueField = "sec_id";
            //SectionSelectDropDownList.DataBind();
            //SectionSelectDropDownList.Items.Insert(0, new System.Web.UI.WebControls.ListItem(""));
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
    

    protected void BtnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = StudentManager.GetAllStudentSearch(ddlCourseID.SelectedValue,txtBatchNo.Text,"",ddlTracID.SelectedValue);
            Session["Class"] = dt;
            HistoryGridView.DataSource = dt;
            HistoryGridView.DataBind();
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
    protected void Refresh_Click(object sender, EventArgs e)
    {
        try
        {
            //ClassSelectDropDownList.SelectedIndex = -1;
            //SectionSelectDropDownList.SelectedIndex = -1;
            ddlTracID.SelectedIndex = -1;
            ddlCourseID.SelectedIndex = -1;
            txtBatchNo.Text = "";
            //ShiftSelectDropdownLift.SelectedIndex = -1;
            //DataTable dt = StudentManager.GetAllStudentInformation();
            DataTable dt = StudentManager.GetAllStudentSearch("", "", "", "");
            HistoryGridView.DataSource = dt;
            //HistoryGridView.PageIndex = e.NewPageIndex;
            HistoryGridView.DataBind();
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
    //************************** Class Selection IndexChange On Section Name Show ****************************//

    //protected void ddlCurClass_SelectedIndexChanged1(object sender, EventArgs e)
    //{

    //    ddlDepartment.DataSource = StudentManager.GetAllDepartmentInformation(ddlCollegeName.SelectedValue);
    //    ddlDepartment.DataValueField = "DeptId";
    //    ddlDepartment.DataTextField = "DeptName";
    //    ddlDepartment.DataBind();
    //    ddlDepartment.Items.Insert(0, new System.Web.UI.WebControls.ListItem(""));

    //    ddlVersion.DataSource = StudentManager.GetShowVersionOnClass(ddlCurClass.SelectedValue);
    //    ddlVersion.DataTextField = "version_name";
    //    ddlVersion.DataValueField = "id";
    //    ddlVersion.DataBind();
    //    ddlVersion.Items.Insert(0, new System.Web.UI.WebControls.ListItem(""));

    //    ddlShift.DataSource = StudentManager.GetShowShiftOnClass(ddlCurClass.SelectedValue);
    //    ddlShift.DataTextField = "shift_name";
    //    ddlShift.DataValueField = "id";
    //    ddlShift.DataBind();
    //    ddlShift.Items.Insert(0, new System.Web.UI.WebControls.ListItem(""));

    //    ddlSect.DataSource = StudentManager.GetShowSectionOnClass(ddlCurClass.SelectedValue);
    //    ddlSect.DataTextField = "sec_name";
    //    ddlSect.DataValueField = "sec_id";
    //    ddlSect.DataBind();
    //    ddlSect.Items.Insert(0, new System.Web.UI.WebControls.ListItem(""));


    //    if (ddlCurClass.SelectedValue == "13" || ddlCurClass.SelectedValue == "14" )
    //    {  lblGroupName.Visible = ddlgroup.Visible =   true;  }
    //    else
    //    { lblGroupName.Visible = ddlgroup.Visible =  false;  }

    //    //if (ddlCurClass.SelectedValue == "07" || ddlCurClass.SelectedValue == "08" || ddlCurClass.SelectedValue == "09" || ddlCurClass.SelectedValue == "10" || ddlCurClass.SelectedValue == "11" || ddlCurClass.SelectedValue == "12")
    //    //{ lblSection.Visible = ddlSect.Visible = lbl13.Visible = true; }
    //    //else
    //    //{ lblSection.Visible = ddlSect.Visible = lbl13.Visible = false; }


    //    lblSection.Visible = ddlSect.Visible =   false;


        
    //}
    protected void PrintButton_Click(object sender, EventArgs e)
    {
        Response.Clear();
        Response.ContentType = "application/pdf";
        Response.AddHeader("content-disposition", "attachment; filename='StudentInformation'.pdf");
        Document document = new Document();
        document = new Document(PageSize.A4.Rotate());
        MemoryStream ms = new MemoryStream();
        PdfWriter writer = PdfWriter.GetInstance(document, Response.OutputStream);
        pdfPage page = new pdfPage();
        writer.PageEvent = page;
        document.Open();

        byte[] logo = GlBookManager.GetGlLogo(Session["book"].ToString());
        iTextSharp.text.Image gif = iTextSharp.text.Image.GetInstance(logo);
        gif.Alignment = iTextSharp.text.Image.MIDDLE_ALIGN;
        gif.ScalePercent(35f);
        float[] titwidth = new float[2] { 5, 200 };
        PdfPCell cell;
        PdfPTable dth = new PdfPTable(titwidth);
        dth.WidthPercentage = 100;
        cell = new PdfPCell(gif);
        cell.HorizontalAlignment = 1;
        cell.VerticalAlignment = 1;
        cell.Rowspan = 4;
        cell.BorderWidth = 0f;
        dth.AddCell(cell);
        cell = new PdfPCell(new Phrase(Session["org"].ToString(), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12, iTextSharp.text.Font.BOLD)));
        cell.HorizontalAlignment = 1;
        cell.VerticalAlignment = 1;
        cell.BorderWidth = 0f;
        //cell.FixedHeight = 20f;
        dth.AddCell(cell);
        cell = new PdfPCell(new Phrase(Session["add1"].ToString(), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL)));
        cell.HorizontalAlignment = 1;
        cell.VerticalAlignment = 1;
        cell.BorderWidth = 0f;
        //cell.FixedHeight = 20f;
        dth.AddCell(cell);
        cell = new PdfPCell(new Phrase(Session["add2"].ToString(), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL)));
        cell.HorizontalAlignment = 1;
        cell.VerticalAlignment = 1;
        cell.BorderWidth = 0f;
        //cell.FixedHeight = 20f;
        dth.AddCell(cell);

        string Head = "";
        DataTable st = StudentManager.getStudentCount("","",ddlCourseName.SelectedValue,txtBatch.Text);
        DataRow row1 = st.Rows[0];

        if (txtBatch.Text == "" && ddlCourseName.SelectedValue == "")
        {}
        else if ( txtBatch.Text == "" && ddlCourseName.SelectedValue != "")
        {  }
        else if ( txtBatch.Text != "" && ddlCourseName.SelectedValue == "")
        { }
        else if ( txtBatch.Text != "" && ddlCourseName.SelectedValue != "")
        { Head = " Version : " + ddlCourseName.SelectedItem.Text + " , Shift : " + txtBatch.Text + " "; }
        else if (  txtBatch.Text != "" && ddlCourseName.SelectedValue != "")
        {   }
        cell = new PdfPCell(new Phrase(Head + " Total Student :" + row1["TotalStd"], FontFactory.GetFont(FontFactory.TIMES_ROMAN, 11, iTextSharp.text.Font.BOLD)));
        cell.HorizontalAlignment = 1;
        cell.VerticalAlignment = 1;
        cell.BorderWidth = 0f;
        dth.AddCell(cell);
        document.Add(dth);

        LineSeparator line = new LineSeparator(0f, 100, null, Element.ALIGN_CENTER, -2);
        document.Add(line);
        PdfPTable dtempty = new PdfPTable(1);
        cell.BorderWidth = 0f;
        cell.FixedHeight = 5f;
        dtempty.AddCell(cell);
        document.Add(dtempty);

        float[] width = new float[9] { 5, 17, 30, 30, 8, 25, 11,15, 15 };
        PdfPTable pdtc = new PdfPTable(width);
        pdtc.WidthPercentage = 100;
        //pdtc.HeaderRows =3;       
        pdtc.HeaderRows = 1;
        //cell = new PdfPCell(FormatHeaderPhrase(""));
        //cell.HorizontalAlignment = 1;
        //cell.VerticalAlignment = 1;
        //cell.FixedHeight = 20f;
        //cell.Colspan = 9;
        //pdtc.AddCell(cell);

            cell = new PdfPCell(FormatHeaderPhrase("SL."));
            cell.HorizontalAlignment = 1;
            cell.VerticalAlignment = 1;
            pdtc.AddCell(cell);
            cell = new PdfPCell(FormatHeaderPhrase("Student ID"));
            cell.HorizontalAlignment = 1;
            cell.VerticalAlignment = 1;
            pdtc.AddCell(cell);
            cell = new PdfPCell(FormatHeaderPhrase("Student Name"));
            cell.HorizontalAlignment = 1;
            cell.VerticalAlignment = 1;
            pdtc.AddCell(cell);
            cell = new PdfPCell(FormatHeaderPhrase("Course Name"));
            cell.HorizontalAlignment = 1;
            cell.VerticalAlignment = 1;
            pdtc.AddCell(cell);
            cell = new PdfPCell(FormatHeaderPhrase("Batch No"));
            cell.HorizontalAlignment = 1;
            cell.VerticalAlignment = 1;
            pdtc.AddCell(cell);
            cell = new PdfPCell(FormatHeaderPhrase("Traineer Name"));
            cell.HorizontalAlignment = 1;
            cell.VerticalAlignment = 1;
            pdtc.AddCell(cell);
            cell = new PdfPCell(FormatHeaderPhrase("Mobile No"));
            cell.HorizontalAlignment = 1;
            cell.VerticalAlignment = 1;
            pdtc.AddCell(cell);
            cell = new PdfPCell(FormatHeaderPhrase("Email"));
            cell.HorizontalAlignment = 1;
            cell.VerticalAlignment = 1;
            pdtc.AddCell(cell);
            //cell = new PdfPCell(FormatHeaderPhrase("Blood Group"));
            //cell.HorizontalAlignment = 1;
            //cell.VerticalAlignment = 1;
            //pdtc.AddCell(cell);
            cell = new PdfPCell(FormatHeaderPhrase("Remarks"));
            cell.HorizontalAlignment = 1;
            cell.VerticalAlignment = 1;
            pdtc.AddCell(cell);
            //DataTable dt = StudentManager.GetShowReportOnStudentInfoClassAndSec(VersionSelectDropdownList.SelectedValue, txtBatchNo.Text,"","");
            DataTable dt = StudentManager.GetAllStudentSearch(ddlCourseName.SelectedValue, txtBatch.Text, "", "");
            int a = 1;

            foreach (DataRow row in dt.Rows)
            {

                cell = new PdfPCell(FormatPhrase(a.ToString()));
                cell.HorizontalAlignment = 1;
                cell.VerticalAlignment = 1;
                //cell.FixedHeight = 20f;
                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                pdtc.AddCell(cell);
                cell = new PdfPCell(FormatPhrase(row["student_id"].ToString()));
                cell.HorizontalAlignment = 1;
                cell.VerticalAlignment = 1;
                //cell.FixedHeight = 20f;
                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                pdtc.AddCell(cell);
                cell = new PdfPCell(FormatPhrase(row["f_name"].ToString()));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = 1;
                //cell.FixedHeight = 20f;
                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                pdtc.AddCell(cell);

                cell = new PdfPCell(FormatPhrase(row["CourseName"].ToString()));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = 1;
                //cell.FixedHeight = 20f;
                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                pdtc.AddCell(cell);

                cell = new PdfPCell(FormatPhrase(row["BatchNo"].ToString()));
                cell.HorizontalAlignment = 1;
                cell.VerticalAlignment = 1;
                //cell.FixedHeight = 20f;
                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                pdtc.AddCell(cell);

                cell = new PdfPCell(FormatPhrase(row["TrainerName"].ToString()));
                cell.HorizontalAlignment = 1;
                cell.VerticalAlignment = 1;
                //cell.FixedHeight = 20f;
                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                pdtc.AddCell(cell);

                cell = new PdfPCell(FormatPhrase(row["mobile_no"].ToString()));
                cell.HorizontalAlignment = 1;
                cell.VerticalAlignment = 1;
                //cell.FixedHeight = 20f;
                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                pdtc.AddCell(cell);
                cell = new PdfPCell(FormatPhrase(row["email"].ToString()));
                cell.HorizontalAlignment = 1;
                cell.VerticalAlignment = 1;
                //cell.FixedHeight = 20f;
                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                //pdtc.AddCell(cell);
                //cell = new PdfPCell(FormatPhrase(row["blood_group"].ToString()));
                //cell.HorizontalAlignment = 1;
                //cell.VerticalAlignment = 1;
                //cell.FixedHeight = 20f;
                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                pdtc.AddCell(cell);
                cell = new PdfPCell(FormatPhrase(""));
                cell.HorizontalAlignment = 1;
                cell.VerticalAlignment = 1;
                //cell.FixedHeight = 20f;
                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                pdtc.AddCell(cell);

                a++;

            }        
        document.Add(pdtc);
        document.Close();
        Response.Flush();
        Response.End();
    }
    private static Phrase FormatPhrase(string value)
    {
        return new Phrase(value, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9));
    }

    private static Phrase FormatHeaderPhrase(string value)
    {
        return new Phrase(value, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD));
    }
    protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
       // if (ddlStatus.SelectedValue == "2")
       // {
       //     ArchiveLabel.Visible = true;
       //     txtInactiveDate.Visible = true;
       // }
       //else if (ddlStatus.SelectedValue == "3")
       // {
       //     ArchiveLabel.Visible = true;
       //     txtInactiveDate.Visible = true;
       // }
       // else
       // {
       //     ArchiveLabel.Visible = false;
       //     txtInactiveDate.Visible = false;
       // }
    }

    protected void TCPrintButton_Click(object sender, EventArgs e)
    {
        Session["TC"] =txtStudentId.Text;
        Response.Redirect("~/Report/UI/TransferCertificateUI.aspx");
    }
    protected void Principalbtn_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtStudentId.Text.Trim()))
        {
            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment; filename='StudentInformation'.pdf");
            Document document = new Document();
            document = new Document(PageSize.A4);
            MemoryStream ms = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(document, Response.OutputStream);
            pdfPage page = new pdfPage();
            writer.PageEvent = page;
            document.Open();

            byte[] logo = GlBookManager.GetGlLogo(Session["book"].ToString());
            iTextSharp.text.Image gif = iTextSharp.text.Image.GetInstance(logo);
            gif.Alignment = iTextSharp.text.Image.MIDDLE_ALIGN;
            gif.ScalePercent(35f);
            float[] titwidth = new float[2] { 5, 200 };
            PdfPCell cell;
            PdfPTable dth = new PdfPTable(titwidth);
            dth.WidthPercentage = 100;
            cell = new PdfPCell(gif);
            cell.HorizontalAlignment = 1;
            cell.VerticalAlignment = 1;
            cell.Rowspan = 4;
            cell.BorderWidth = 0f;
            dth.AddCell(cell);
            cell = new PdfPCell(new Phrase(Session["org"].ToString(), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12, iTextSharp.text.Font.BOLD)));
            cell.HorizontalAlignment = 1;
            cell.VerticalAlignment = 1;
            cell.BorderWidth = 0f;
            //cell.FixedHeight = 20f;
            dth.AddCell(cell);
            cell = new PdfPCell(new Phrase(Session["add1"].ToString(), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL)));
            cell.HorizontalAlignment = 1;
            cell.VerticalAlignment = 1;
            cell.BorderWidth = 0f;
            //cell.FixedHeight = 20f;
            dth.AddCell(cell);
            cell = new PdfPCell(new Phrase(Session["add2"].ToString(), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL)));
            cell.HorizontalAlignment = 1;
            cell.VerticalAlignment = 1;
            cell.BorderWidth = 0f;
            //cell.FixedHeight = 20f;
            dth.AddCell(cell);

            cell = new PdfPCell(new Phrase("Indivisual Student Inforation", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 11, iTextSharp.text.Font.BOLD)));
            cell.HorizontalAlignment = 1;
            cell.VerticalAlignment = 1;
            cell.BorderWidth = 0f;

            dth.AddCell(cell);
            document.Add(dth);

            LineSeparator line = new LineSeparator(0f, 100, null, Element.ALIGN_CENTER, -2);
            document.Add(line);
            PdfPTable dtempty = new PdfPTable(1);
            cell.BorderWidth = 0f;
            cell.FixedHeight = 5f;
            dtempty.AddCell(cell);
            document.Add(dtempty);

            float[] width = new float[5] { 14, 20, 20, 35, 11 };
            PdfPTable pdtc = new PdfPTable(width);
            pdtc.WidthPercentage = 100;
            //pdtc.HeaderRows = 1;
            //cell = new PdfPCell(FormatHeaderPhrase(""));
            //cell.HorizontalAlignment = 1;
            //cell.VerticalAlignment = 1;
            //cell.Colspan = 5;
            //cell.Border = 0;
            //pdtc.AddCell(cell);
            cell = new PdfPCell(FormatHeaderPhrase("Student ID"));
            cell.HorizontalAlignment = 1;
            cell.VerticalAlignment = 1;
            pdtc.AddCell(cell);
            cell = new PdfPCell(FormatHeaderPhrase("Name"));
            cell.HorizontalAlignment = 1;
            cell.VerticalAlignment = 1;
            pdtc.AddCell(cell);
            cell = new PdfPCell(FormatHeaderPhrase("Fathers Name"));
            cell.HorizontalAlignment = 1;
            cell.VerticalAlignment = 1;
            pdtc.AddCell(cell);
            cell = new PdfPCell(FormatHeaderPhrase("Present Address"));
            cell.HorizontalAlignment = 1;
            cell.VerticalAlignment = 1;
            pdtc.AddCell(cell);
            cell = new PdfPCell(FormatHeaderPhrase("Mobile No"));
            cell.HorizontalAlignment = 1;
            cell.VerticalAlignment = 1;
            pdtc.AddCell(cell);


            DataTable dt = StudentManager.GetShowReportOnStudentInfo(txtStudentId.Text);
            foreach (DataRow row in dt.Rows)
            {
                cell = new PdfPCell(FormatPhrase(row["student_id"].ToString()));
                cell.HorizontalAlignment = 1;
                cell.VerticalAlignment = 1;
                //cell.FixedHeight = 20f;
                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                pdtc.AddCell(cell);
                cell = new PdfPCell(FormatPhrase(row["student_name"].ToString()));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = 1;
                //cell.FixedHeight = 20f;
                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                pdtc.AddCell(cell);
                cell = new PdfPCell(FormatPhrase(row["fth_name"].ToString()));
                cell.HorizontalAlignment = 1;
                cell.VerticalAlignment = 1;
                //cell.FixedHeight = 20f;
                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                pdtc.AddCell(cell);
                cell = new PdfPCell(FormatPhrase(row["per_loc"].ToString()));
                cell.HorizontalAlignment = 1;
                cell.VerticalAlignment = 1;
                //cell.FixedHeight = 20f;
                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                pdtc.AddCell(cell);
                cell = new PdfPCell(FormatPhrase(row["mobile_no"].ToString()));
                cell.HorizontalAlignment = 1;
                cell.VerticalAlignment = 1;
                //cell.FixedHeight = 20f;
                //cell.BorderColor = BaseColor.LIGHT_GRAY;
                pdtc.AddCell(cell);

            }

            cell = new PdfPCell(FormatHeaderPhrase(""));
            cell.HorizontalAlignment = 1;
            cell.VerticalAlignment = 1;
            cell.Colspan = 5;
            cell.Border = 0;
            cell.FixedHeight = 20f;
            pdtc.AddCell(cell);

            float[] widtht = new float[2] { 25, 30 };
            PdfPTable pdtct = new PdfPTable(widtht);
            pdtct.WidthPercentage = 60;
            cell.VerticalAlignment = 3;
            DataTable dts = StudentManager.GetShowReportOnStudentInfoCurrent(txtStudentId.Text);

            cell = new PdfPCell(FormatHeaderPhrase("Course Name"));
            cell.HorizontalAlignment = 1;
            cell.VerticalAlignment = 1;
            pdtct.AddCell(cell);

            cell = new PdfPCell(FormatPhrase(dts.Rows[0]["CourseName"].ToString()));
            cell.HorizontalAlignment = 1;
            cell.VerticalAlignment = 1;
            pdtct.AddCell(cell);

            cell = new PdfPCell(FormatHeaderPhrase("Batch No"));
            cell.HorizontalAlignment = 1;
            cell.VerticalAlignment = 1;
            pdtct.AddCell(cell);

            cell = new PdfPCell(FormatPhrase(dts.Rows[0]["BatchNo"].ToString()));
            cell.HorizontalAlignment = 1;
            cell.VerticalAlignment = 1;
            pdtct.AddCell(cell);

            cell = new PdfPCell(FormatHeaderPhrase("Shedule"));
            cell.HorizontalAlignment = 1;
            cell.VerticalAlignment = 1;
            pdtct.AddCell(cell);

            cell = new PdfPCell(FormatPhrase(dts.Rows[0]["std_admission_date"].ToString()));
            cell.HorizontalAlignment = 1;
            cell.VerticalAlignment = 1;
            pdtct.AddCell(cell);

            cell = new PdfPCell(FormatHeaderPhrase("Course Fee"));
            cell.HorizontalAlignment = 1;
            cell.VerticalAlignment = 1;
            pdtct.AddCell(cell);

            cell = new PdfPCell(FormatPhrase(dts.Rows[0]["CourseFee"].ToString()));
            cell.HorizontalAlignment = 1;
            cell.VerticalAlignment = 1;
            pdtct.AddCell(cell);

            cell = new PdfPCell(FormatHeaderPhrase("Paid Amount"));
            cell.HorizontalAlignment = 1;
            cell.VerticalAlignment = 1;
            pdtct.AddCell(cell);

            cell = new PdfPCell(FormatPhrase(dts.Rows[0]["PayAmount"].ToString()));
            cell.HorizontalAlignment = 1;
            cell.VerticalAlignment = 1;
            pdtct.AddCell(cell);

            cell = new PdfPCell(FormatHeaderPhrase("Due Amount"));
            cell.HorizontalAlignment = 1;
            cell.VerticalAlignment = 1;
            pdtct.AddCell(cell);

            cell = new PdfPCell(FormatPhrase(dts.Rows[0]["DueAmount"].ToString()));
            cell.HorizontalAlignment = 1;
            cell.VerticalAlignment = 1;
            pdtct.AddCell(cell);


            PdfPCell cells = new PdfPCell();
            iTextSharp.text.Rectangle page1 = document.PageSize;
            float[] FootWth = new float[] { 5, 30, 10, 30, 10, 30 };
            PdfPTable Fdth = new PdfPTable(FootWth);
            Fdth.TotalWidth = page1.Width - 10;
            Fdth.HorizontalAlignment = Element.ALIGN_CENTER;
            cells = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)));
            cells.HorizontalAlignment = 1;
            cells.Border = 0;
            cells.VerticalAlignment = 1;
            Fdth.AddCell(cells);
            cells = new PdfPCell(new Phrase("Student/Gurdian Signature", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL)));
            cells.HorizontalAlignment = 1;
            cells.Border = 1;
            cells.VerticalAlignment = 1;
            Fdth.AddCell(cells);
            cells = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)));
            cells.HorizontalAlignment = 1;
            cells.VerticalAlignment = 1;
            cells.Border = 0;
            Fdth.AddCell(cells);
            cells = new PdfPCell(new Phrase("Co-ordinator Signature", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL)));
            cells.HorizontalAlignment = 1;
            cells.Border = 1;
            cells.VerticalAlignment = 1;
            Fdth.AddCell(cells);
            cells = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)));
            cells.HorizontalAlignment = 1;
            cells.VerticalAlignment = 1;
            cells.Border = 0;
            Fdth.AddCell(cells);
            cells = new PdfPCell(new Phrase("Accounts Officer", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL)));
            cells.HorizontalAlignment = 1;
            cells.Border = 1;
            cells.VerticalAlignment = 1;
            Fdth.AddCell(cells);
            Fdth.WriteSelectedRows(0, 5, 0, 30, writer.DirectContent);
            document.Add(pdtc);
            document.Add(pdtct);
            document.Close();
            Response.Flush();
            Response.End();
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ale", "alert('Please select student Id or input...!!');", true);
        }
    }
    
    private void CreateGridView()
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("Installment_Serial", typeof(string));
        dt.Columns.Add("Installment_Amount", typeof(string));
        dt.Columns.Add("Installment_Date", typeof(string));

        ViewState["dt"] = dt;

    }
    int serial = 1;
    //protected void GenerateRows(object sender, ImageClickEventArgs e)
    //{

    //    var InstallQnty = int.Parse(txtInstallQnty.Text.Trim());
    //    var interval = int.Parse(txtMonthInterval.Text);

    //    string startdate = txtDate.Text;
    //    var tamount = decimal.Parse(txtTotalAmt.Text);
    //    decimal admisionfee = decimal.Parse(txtAdmissionFee.Text);
    //    decimal remainingAmount = tamount - admisionfee;
    //    decimal rowamount = Math.Ceiling(remainingAmount / InstallQnty);

    //    int numbers = int.Parse(this.txtInstallQnty.Text.Trim());
    //    DataTable dt = (DataTable)ViewState["dt"];
    //    dt.Rows.Clear();
    //    ViewState["dt"] = dt;
    //    string runningdate = "";
    //    for (int i = 0; i < numbers; i++)
    //    {
    //        if (runningdate == "")
    //        {
    //            runningdate = txtDate.Text;
    //        }
    //        else
    //        {
    //            runningdate = DataManager.DateEncode(runningdate).AddMonths(Convert.ToInt32(txtMonthInterval.Text)).ToString("dd/MM/yyy");
    //        }

    //        dt.NewRow();
    //        dt.Rows.Add(serial, rowamount, runningdate);
    //        serial++;
    //    }
    //    dgInstallment.DataSource = dt;
    //    ViewState["dt"] = dt;
    //    dgInstallment.DataBind();
    //}


     
    
    protected void btnInsFeePrint_Click(object sender, EventArgs e)
    {
        PrintInstallmentReciept();

        string strJS = ("<script type='text/javascript'>window.open('Default4.aspx','_blank');</script>");
        Page.ClientScript.RegisterStartupScript(this.GetType(), "strJSAlert", strJS);
    }

    private void PrintInstallmentReciept()
    {
    //    try
    //    {

    //        Response.Clear();
    //        Response.ContentType = "application/pdf";
    //        Response.AddHeader("content-disposition", "attachment; filename='StudentPaymentReport'.pdf");
    //        Student std = StudentManager.getStd(txtStudentId.Text);

    //       // DataTable paymst = StudentManager.getInstallmentDetails(txtInstallmentName.Text);

    //        Document document = new Document();
    //        //float pwidth = (float)(25.7 / 2.54) * 72;
    //        //float pheight = (float)(22.7 / 2.54) * 72; 
    //        float pwidth = (float)(14 / 2.54) * 72;
    //        float pheight = (float)(20 / 2.54) * 72;
    //        document = new Document(new iTextSharp.text.Rectangle(pwidth, pheight));
    //        //document = new Document(PageSize.A4);
    //        //document = new Document(new Rectangle.A4.Rotate(), 90f, 90f, 35f, 20f);
    //        PdfWriter writer = PdfWriter.GetInstance(document, Response.OutputStream);
    //        MemoryStream ms = new MemoryStream();
    //        //pdfPage page1 = new pdfPage();
    //        // writer.PageEvent = page1;
    //        document.Open();
    //        string a = txtStudentId.Text.Trim();


    //        if (txtStudentId.Text != "")
    //        {
    //            //byte[] logo = GlBookManager.GetGlLogo(Session["book"].ToString());
    //            //iTextSharp.text.Image gif = iTextSharp.text.Image.GetInstance(logo);
    //            //gif.Alignment = iTextSharp.text.Image.MIDDLE_ALIGN;
    //            //gif.ScalePercent(25f);
    //            float[] titwidth = new float[2] { 5, 200 };
    //            float[] width = new float[7] { 50, 5, 90, 7, 40, 5, 45 };
    //            float[] swidth = new float[3] { 20, 30, 20 };
    //            float[] swidth1 = new float[2] { 130, 70 };
    //            float[] twidth = new float[3] { 100, 5, 100 };
    //            float[] widFooter = new float[2] { 55, 45 };

    //            decimal tott = decimal.Zero;
    //            decimal tot = decimal.Zero;

    //           // if (paymst.Rows.Count > 0)
    //            //{
    //             //   DataRow row = paymst.Rows[0];
    //            //  decimal totttt4 = decimal.Zero;
    //            if (std != null)
    //            {
    //                for (int i = 1; i <= 2; i++)
    //                {
    //                    PdfPCell cell;
    //                    PdfPTable dth = new PdfPTable(titwidth);
    //                    dth.WidthPercentage = 100;
    //                    cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12, iTextSharp.text.Font.BOLD)));
    //                    cell.HorizontalAlignment = 1;
    //                    cell.VerticalAlignment = 1;
    //                    cell.Rowspan = 4;
    //                    cell.BorderWidth = 0f;
    //                    dth.AddCell(cell);
    //                    cell = new PdfPCell(new Phrase(ddlCourseName.SelectedItem.Text, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12, iTextSharp.text.Font.BOLD)));
    //                    cell.HorizontalAlignment = 1;
    //                    cell.VerticalAlignment = 1;
    //                    cell.BorderWidth = 0f;
    //                    //cell.FixedHeight = 20f;
    //                    dth.AddCell(cell);
    //                    cell = new PdfPCell(new Phrase(Session["add1"].ToString(), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL)));
    //                    cell.HorizontalAlignment = 1;
    //                    cell.VerticalAlignment = 1;
    //                    cell.BorderWidth = 0f;
    //                    //cell.FixedHeight = 20f;
    //                    dth.AddCell(cell);
    //                    cell = new PdfPCell(new Phrase(Session["add2"].ToString(), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL)));
    //                    cell.HorizontalAlignment = 1;
    //                    cell.VerticalAlignment = 1;
    //                    cell.BorderWidth = 0f;
    //                    //cell.FixedHeight = 20f;
    //                    dth.AddCell(cell);
    //                    cell = new PdfPCell(new Phrase("Student Installment Reciept", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 11, iTextSharp.text.Font.BOLD)));
    //                    cell.HorizontalAlignment = 1;
    //                    cell.VerticalAlignment = 1;
    //                    cell.BorderWidth = 0f;
    //                    cell.FixedHeight = 20f;
    //                    dth.AddCell(cell);

    //                    cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 11, iTextSharp.text.Font.BOLD)));
    //                    cell.HorizontalAlignment = 1;
    //                    cell.VerticalAlignment = 1;
    //                    cell.BorderWidth = 0f;
    //                    cell.FixedHeight = 20f;
    //                    dth.AddCell(cell);
    //                    if (i == 2)
    //                    {
    //                        cell = new PdfPCell(new Phrase("Student Copy", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 11, iTextSharp.text.Font.BOLD)));
    //                        cell.HorizontalAlignment = 1;
    //                        cell.VerticalAlignment = 1;
    //                        cell.BorderWidth = 0f;
    //                        cell.FixedHeight = 20f;
    //                        dth.AddCell(cell);

    //                        cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 11, iTextSharp.text.Font.BOLD)));
    //                        cell.HorizontalAlignment = 1;
    //                        cell.VerticalAlignment = 1;
    //                        cell.BorderWidth = 0f;
    //                        cell.FixedHeight = 20f;
    //                        dth.AddCell(cell);


    //                    }
    //                    else
    //                    {
    //                        cell = new PdfPCell(new Phrase("Office Copy", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 11, iTextSharp.text.Font.BOLD)));
    //                        cell.HorizontalAlignment = 1;
    //                        cell.VerticalAlignment = 1;
    //                        cell.BorderWidth = 0f;
    //                        cell.FixedHeight = 20f;
    //                        dth.AddCell(cell);

    //                        cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 11, iTextSharp.text.Font.BOLD)));
    //                        cell.HorizontalAlignment = 1;
    //                        cell.VerticalAlignment = 1;
    //                        cell.BorderWidth = 0f;
    //                        cell.FixedHeight = 20f;
    //                        dth.AddCell(cell);
    //                    }
    //                    document.Add(dth);
    //                    LineSeparator line = new LineSeparator(0f, 100, null, Element.ALIGN_CENTER, -2);
    //                    document.Add(line);
    //                    PdfPTable dtempty = new PdfPTable(1);
    //                    cell = new PdfPCell(FormatHeaderPhrase(""));
    //                    cell.BorderWidth = 0f;
    //                    cell.FixedHeight = 5f;
    //                    dtempty.AddCell(cell);
    //                    document.Add(dtempty);

    //                    PdfPTable dtm = new PdfPTable(width);
    //                    dtm.WidthPercentage = 100;
    //                    cell = new PdfPCell(new Phrase("Receipt No", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
    //                    cell.HorizontalAlignment = 3;
    //                    cell.VerticalAlignment = 1;
    //                    cell.BorderWidth = 0f;
    //                    cell.FixedHeight = 15f;
    //                    dtm.AddCell(cell);
    //                    cell = new PdfPCell(new Phrase(":", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
    //                    cell.HorizontalAlignment = 3;
    //                    cell.VerticalAlignment = 1;
    //                    cell.BorderWidth = 0f;
    //                    cell.FixedHeight = 15f;
    //                    dtm.AddCell(cell);
    //                    cell = new PdfPCell(new Phrase(txtInstallmentName.Text, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
    //                    cell.HorizontalAlignment = 3;
    //                    cell.VerticalAlignment = 1;
    //                    cell.BorderWidth = 0f;
    //                    //cell.FixedHeight = 15f;
    //                    //cell.Colspan = 3;
    //                    dtm.AddCell(cell);
    //                    cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
    //                    cell.HorizontalAlignment = 3;
    //                    cell.VerticalAlignment = 1;
    //                    cell.BorderWidth = 0f;
    //                    //cell.FixedHeight = 15f;
    //                    dtm.AddCell(cell);
    //                    cell = new PdfPCell(new Phrase("Date", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
    //                    cell.HorizontalAlignment = 3;
    //                    cell.VerticalAlignment = 1;
    //                    cell.BorderWidth = 0f;
    //                    cell.FixedHeight = 15f;
    //                    dtm.AddCell(cell);
    //                    cell = new PdfPCell(new Phrase(":", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
    //                    cell.HorizontalAlignment = 3;
    //                    cell.VerticalAlignment = 1;
    //                    cell.BorderWidth = 0f;
    //                    //cell.FixedHeight = 15f;
    //                    dtm.AddCell(cell);
    //                    cell = new PdfPCell(new Phrase(txtDate.Text, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
    //                    cell.HorizontalAlignment = 3;
    //                    cell.VerticalAlignment = 1;
    //                    cell.BorderWidth = 0f;
    //                    //cell.FixedHeight = 15f;
    //                    dtm.AddCell(cell);
    //                    cell = new PdfPCell(new Phrase("Student ID & Name", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
    //                    cell.HorizontalAlignment = 0;
    //                    cell.VerticalAlignment = 1;
    //                    cell.BorderWidth = 0f;
    //                    cell.FixedHeight = 15f;
    //                    dtm.AddCell(cell);
    //                    cell = new PdfPCell(new Phrase(":", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
    //                    cell.HorizontalAlignment = 3;
    //                    cell.VerticalAlignment = 1;
    //                    cell.BorderWidth = 0f;
    //                    cell.FixedHeight = 15f;
    //                    dtm.AddCell(cell);
    //                    cell = new PdfPCell(new Phrase(std.StudentId + "-" + StudentManager.getStudentName(std.StudentId), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 7, iTextSharp.text.Font.NORMAL)));
    //                    cell.HorizontalAlignment = 0;
    //                    cell.VerticalAlignment = 1;
    //                    cell.BorderWidth = 0f;
    //                    dtm.AddCell(cell);
    //                    cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
    //                    cell.HorizontalAlignment = 3;
    //                    cell.VerticalAlignment = 1;
    //                    cell.BorderWidth = 0f;
    //                    cell.FixedHeight = 15f;
    //                    dtm.AddCell(cell);
    //                    cell = new PdfPCell(new Phrase(" ", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
    //                    cell.HorizontalAlignment = 3;
    //                    cell.VerticalAlignment = 1;
    //                    cell.BorderWidth = 0f;
    //                    cell.FixedHeight = 15f;
    //                    dtm.AddCell(cell);
    //                    cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
    //                    cell.HorizontalAlignment = 3;
    //                    cell.VerticalAlignment = 1;
    //                    cell.BorderWidth = 0f;
    //                    cell.FixedHeight = 15f;
    //                    dtm.AddCell(cell);
    //                    cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
    //                    cell.HorizontalAlignment = 3;
    //                    cell.VerticalAlignment = 1;
    //                    cell.BorderWidth = 0f;
    //                    cell.FixedHeight = 15f;
    //                    dtm.AddCell(cell);

    //                    cell = new PdfPCell(new Phrase("Roll No.", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
    //                    cell.HorizontalAlignment = 3;
    //                    cell.VerticalAlignment = 1;
    //                    cell.BorderWidth = 0f;
    //                    cell.FixedHeight = 15f;
    //                    dtm.AddCell(cell);
    //                    cell = new PdfPCell(new Phrase(":", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
    //                    cell.HorizontalAlignment = 3;
    //                    cell.VerticalAlignment = 1;
    //                    cell.BorderWidth = 0f;
    //                    cell.FixedHeight = 15f;
    //                    dtm.AddCell(cell);
    //                    cell = new PdfPCell(new Phrase(txtRoll.Text, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
    //                    cell.HorizontalAlignment = 3;
    //                    cell.VerticalAlignment = 1;
    //                    cell.BorderWidth = 0f;
    //                    cell.FixedHeight = 15f;
    //                    dtm.AddCell(cell);
    //                    cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
    //                    cell.HorizontalAlignment = 3;
    //                    cell.VerticalAlignment = 1;
    //                    cell.BorderWidth = 0f;
    //                    cell.FixedHeight = 15f;
    //                    dtm.AddCell(cell);
    //                    cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
    //                    cell.HorizontalAlignment = 3;
    //                    cell.VerticalAlignment = 1;
    //                    cell.BorderWidth = 0f;
    //                    cell.FixedHeight = 15f;
    //                    dtm.AddCell(cell);
    //                    cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
    //                    cell.HorizontalAlignment = 3;
    //                    cell.VerticalAlignment = 1;
    //                    cell.BorderWidth = 0f;
    //                    cell.FixedHeight = 15f;
    //                    dtm.AddCell(cell);
    //                    cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
    //                    cell.HorizontalAlignment = 3;
    //                    cell.VerticalAlignment = 1;
    //                    cell.BorderWidth = 0f;
    //                    cell.FixedHeight = 15f;

    //                    dtm.AddCell(cell);

    //                    cell = new PdfPCell(new Phrase("Total Amount", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
    //                    cell.HorizontalAlignment = 3;
    //                    cell.VerticalAlignment = 1;
    //                    cell.BorderWidth = 0f;
    //                    cell.FixedHeight = 15f;
    //                    dtm.AddCell(cell);
    //                    cell = new PdfPCell(new Phrase(":", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
    //                    cell.HorizontalAlignment = 3;
    //                    cell.VerticalAlignment = 1;
    //                    cell.BorderWidth = 0f;
    //                    cell.FixedHeight = 15f;
    //                    dtm.AddCell(cell);
    //                    cell = new PdfPCell(new Phrase(row["TotalAmt"].ToString(), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)));
    //                    cell.HorizontalAlignment = 3;
    //                    cell.VerticalAlignment = 1;
    //                    cell.BorderWidth = 0f;
    //                    cell.FixedHeight = 15f;
    //                    dtm.AddCell(cell);
    //                    cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
    //                    cell.HorizontalAlignment = 3;
    //                    cell.VerticalAlignment = 1;
    //                    cell.BorderWidth = 0f;
    //                    cell.FixedHeight = 15f;
    //                    dtm.AddCell(cell);
    //                    cell = new PdfPCell(new Phrase("Admission Fee", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
    //                    cell.HorizontalAlignment = 3;
    //                    cell.VerticalAlignment = 1;
    //                    cell.BorderWidth = 0f;
    //                    cell.FixedHeight = 15f;
    //                    dtm.AddCell(cell);
    //                    cell = new PdfPCell(new Phrase(":", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL)));
    //                    cell.HorizontalAlignment = 3;
    //                    cell.VerticalAlignment = 1;
    //                    cell.BorderWidth = 0f;
    //                    cell.FixedHeight = 15f;
    //                    dtm.AddCell(cell);
    //                    cell = new PdfPCell(new Phrase(row["AdmissionFee"].ToString(), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)));
    //                    cell.HorizontalAlignment = 3;
    //                    cell.VerticalAlignment = 1;
    //                    cell.BorderWidth = 0f;
    //                    cell.FixedHeight = 15f;

    //                    dtm.AddCell(cell);
    //                    document.Add(dtm);
    //                    document.Add(dtempty);


    //                    PdfPTable pdtPay = new PdfPTable(swidth);
    //                    pdtPay.WidthPercentage = 100;

    //                    cell = new PdfPCell(new Phrase("Installment", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.BOLD)));
    //                    cell.HorizontalAlignment = 1;
    //                    cell.VerticalAlignment = 1;
    //                    //cell.BorderColor = BaseColor.LIGHT_GRAY;
    //                    pdtPay.AddCell(cell);
    //                    cell = new PdfPCell(new Phrase("Installment Date", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.BOLD)));
    //                    cell.HorizontalAlignment = 1;
    //                    cell.VerticalAlignment = 1;
    //                    //cell.BorderColor = BaseColor.LIGHT_GRAY;
    //                    pdtPay.AddCell(cell);
    //                    cell = new PdfPCell(new Phrase("Installment Amount", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.BOLD)));
    //                    cell.HorizontalAlignment = 1;
    //                    cell.VerticalAlignment = 1;
    //                    //cell.BorderColor = BaseColor.LIGHT_GRAY;
    //                    pdtPay.AddCell(cell);

    //                     //DataTable dtPay = (DataTable)ViewState["paydtl"];
    //                     foreach (DataRow dr in paymst.Rows)
    //                     {
    //                         cell = new PdfPCell(new Phrase(dr["InstallmentId"].ToString(), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL)));
    //                         cell.HorizontalAlignment = 1;
    //                         cell.VerticalAlignment = 1;
    //                         //cell.BorderColor = BaseColor.LIGHT_GRAY;
    //                         pdtPay.AddCell(cell);

    //                         cell = new PdfPCell(new Phrase(dr["InstallDate"].ToString(), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)));
    //                         cell.HorizontalAlignment = 1;
    //                         cell.VerticalAlignment = 1;
    //                         //cell.BorderColor = BaseColor.LIGHT_GRAY;
    //                         pdtPay.AddCell(cell);

    //                         // double prvDiscount=IdManager.GetShowSingleValueCurrency(
    //                         cell = new PdfPCell(new Phrase(decimal.Parse(dr["InstallAmt"].ToString().Replace(",", "")).ToString("N2"), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL)));
    //                         cell.HorizontalAlignment = 2;
    //                         cell.VerticalAlignment = 1;
    //                         //cell.BorderColor = BaseColor.LIGHT_GRAY;
    //                         pdtPay.AddCell(cell);

    //                         if (i == 2)
    //                         {
    //                             tott += decimal.Parse(dr["InstallAmt"].ToString().Replace(",", ""));
    //                         }
    //                         else
    //                         {
    //                             tot += decimal.Parse(dr["InstallAmt"].ToString().Replace(",", ""));

    //                         }
    //                     }



    //                     if (i == 2)
    //                     {
    //                         cell = new PdfPCell(new Phrase("Total Amount of Installment", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.BOLD)));
    //                         cell.HorizontalAlignment = 2;
    //                         cell.VerticalAlignment = 1;
    //                         //cell.BorderColor = BaseColor.LIGHT_GRAY;
    //                         cell.Colspan = 2;
    //                         pdtPay.AddCell(cell);
                             

    //                         cell = new PdfPCell(new Phrase((tott).ToString("N2"), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.BOLD)));
    //                         cell.HorizontalAlignment = 2;
    //                         cell.VerticalAlignment = 1;
    //                         //cell.BorderColor = BaseColor.LIGHT_GRAY;                            
    //                         pdtPay.AddCell(cell);

    //                         cell = new PdfPCell(new Phrase("In word: " + DataManager.GetLiteralAmt(tott.ToString()).Replace("  ", " "), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)));
    //                         cell.HorizontalAlignment = 3;
    //                         cell.VerticalAlignment = 2;
    //                         cell.Border = 0;
    //                         cell.Colspan = 3;
    //                         pdtPay.AddCell(cell);
    //                         document.Add(pdtPay);

                            
    //                     }
    //                     else
    //                     {
    //                         cell = new PdfPCell(new Phrase("Total Amount of Installment", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.BOLD)));
    //                         cell.HorizontalAlignment = 2;
    //                         cell.VerticalAlignment = 1;
    //                         //cell.BorderColor = BaseColor.LIGHT_GRAY;
    //                         cell.Colspan = 2;
    //                         pdtPay.AddCell(cell);
                             
    //                         cell = new PdfPCell(new Phrase(tot.ToString("N2"), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.BOLD)));
    //                         cell.HorizontalAlignment = 2;
    //                         cell.VerticalAlignment = 1;
    //                         //cell.BorderColor = BaseColor.LIGHT_GRAY;
    //                         pdtPay.AddCell(cell);

    //                         cell = new PdfPCell(new Phrase("In word: " + DataManager.GetLiteralAmt(tot.ToString()).Replace("  ", " "), FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)));
    //                         cell.HorizontalAlignment = 3;
    //                         cell.VerticalAlignment = 2;
    //                         cell.Border = 0;
    //                         cell.Colspan = 3;
    //                         pdtPay.AddCell(cell);
    //                         document.Add(pdtPay);

    //                     }


    //                    if (i == 2)
    //                    {


    //                        PdfPTable dtempty1 = new PdfPTable(1);
    //                        dtempty1.WidthPercentage = 100;
    //                        cell = new PdfPCell(FormatPhrase(""));
    //                        cell.VerticalAlignment = 0;
    //                        cell.HorizontalAlignment = 0;
    //                        cell.BorderWidth = 0f;
    //                        cell.FixedHeight = 30f;
    //                        dtempty1.AddCell(cell);
    //                        document.Add(dtempty1);

    //                        float[] widthsig = new float[] { 20, 5, 20, 5, 20 };
    //                        PdfPTable pdtsig = new PdfPTable(widthsig);
    //                        pdtsig.WidthPercentage = 100;
    //                        cell = new PdfPCell(FormatPhrase("Student/Gurdian Signature"));
    //                        cell.HorizontalAlignment = 1;
    //                        cell.VerticalAlignment = 1;
    //                        cell.Border = 1;
    //                        //cell.FixedHeight = 18f;
    //                        //cell.BorderColor = BaseColor.LIGHT_GRAY;
    //                        pdtsig.AddCell(cell);
    //                        cell = new PdfPCell(FormatPhrase(""));
    //                        cell.HorizontalAlignment = 0;
    //                        cell.VerticalAlignment = 1;
    //                        cell.Border = 0;
    //                        //cell.FixedHeight = 18f;
    //                        //cell.BorderColor = BaseColor.LIGHT_GRAY;
    //                        pdtsig.AddCell(cell);
    //                        cell = new PdfPCell(FormatPhrase("Principal Signature"));
    //                        cell.HorizontalAlignment = 1;
    //                        cell.VerticalAlignment = 1;
    //                        cell.Border = 1;
    //                        cell.FixedHeight = 18f;
    //                        //cell.BorderColor = BaseColor.LIGHT_GRAY;
    //                        pdtsig.AddCell(cell);
    //                        cell = new PdfPCell(FormatPhrase(""));
    //                        cell.HorizontalAlignment = 0;
    //                        cell.VerticalAlignment = 1;
    //                        cell.Border = 0;
    //                        cell.FixedHeight = 18f;
    //                        //cell.BorderColor = BaseColor.LIGHT_GRAY;
    //                        pdtsig.AddCell(cell);
    //                        cell = new PdfPCell(FormatPhrase("Accounts Officer"));
    //                        cell.HorizontalAlignment = 1;
    //                        cell.VerticalAlignment = 1;
    //                        cell.Border = 1;
    //                        //cell.FixedHeight = 18f;
    //                        //cell.BorderColor = BaseColor.LIGHT_GRAY;
    //                        pdtsig.AddCell(cell);
    //                        document.Add(pdtsig);

    //                        document.NewPage();
    //                    }
    //                    else
    //                    {

    //                        //document.Add(pdtTot);

    //                        PdfPTable dtempty1 = new PdfPTable(1);
    //                        dtempty1.WidthPercentage = 100;
    //                        cell = new PdfPCell(FormatPhrase(""));
    //                        cell.VerticalAlignment = 0;
    //                        cell.HorizontalAlignment = 0;
    //                        cell.BorderWidth = 0f;
    //                        cell.FixedHeight = 30f;
    //                        dtempty1.AddCell(cell);
    //                        document.Add(dtempty1);

    //                        float[] widthsig = new float[] { 20, 5, 20, 5, 20 };
    //                        PdfPTable pdtsig = new PdfPTable(widthsig);
    //                        pdtsig.WidthPercentage = 100;
    //                        cell = new PdfPCell(FormatPhrase("Student/Gurdian Signature"));
    //                        cell.HorizontalAlignment = 1;
    //                        cell.VerticalAlignment = 1;
    //                        cell.Border = 1;
    //                        //cell.FixedHeight = 18f;
    //                        //cell.BorderColor = BaseColor.LIGHT_GRAY;
    //                        pdtsig.AddCell(cell);
    //                        cell = new PdfPCell(FormatPhrase(""));
    //                        cell.HorizontalAlignment = 0;
    //                        cell.VerticalAlignment = 1;
    //                        cell.Border = 0;
    //                        //cell.FixedHeight = 18f;
    //                        //cell.BorderColor = BaseColor.LIGHT_GRAY;
    //                        pdtsig.AddCell(cell);
    //                        cell = new PdfPCell(FormatPhrase("Principal Signature"));
    //                        cell.HorizontalAlignment = 1;
    //                        cell.VerticalAlignment = 1;
    //                        cell.Border = 1;
    //                        cell.FixedHeight = 18f;
    //                        //cell.BorderColor = BaseColor.LIGHT_GRAY;
    //                        pdtsig.AddCell(cell);
    //                        cell = new PdfPCell(FormatPhrase(""));
    //                        cell.HorizontalAlignment = 0;
    //                        cell.VerticalAlignment = 1;
    //                        cell.Border = 0;
    //                        cell.FixedHeight = 18f;
    //                        //cell.BorderColor = BaseColor.LIGHT_GRAY;
    //                        pdtsig.AddCell(cell);
    //                        cell = new PdfPCell(FormatPhrase("Accounts Officer"));
    //                        cell.HorizontalAlignment = 1;
    //                        cell.VerticalAlignment = 1;
    //                        cell.Border = 1;
    //                        //cell.FixedHeight = 18f;
    //                        //cell.BorderColor = BaseColor.LIGHT_GRAY;
    //                        pdtsig.AddCell(cell);
    //                        document.Add(pdtsig);
    //                        document.NewPage();
    //                    }
    //                    //if (i == 1)
    //                    //{
    //                    //    document.Add(new Paragraph(string.Format("", i)));
    //                    //}
    //                }

    //            }
    //            }

    //            document.Close();
    //            Response.Flush();
    //            Response.End();

            

    //    }
    //    catch (FormatException fex)
    //    {
    //        ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('" + fex.Message + "');", true);
    //    }
    //    catch (Exception ex)
    //    {
    //        if (ex.Message.Contains("Database"))
    //            ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Database Maintain Error. Contact to the Software Provider..!!');", true);
    //        else
    //            ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('There is some problem to do the task. Try again properly.!!');", true);
    //    }
    }


    protected void ddlTracName_SelectedIndexChanged(object sender, EventArgs e)
    {

        //util.PopulationDropDownList(ddlCourseName, "tbl_Course_Name", "Select ci.CourseName,ci.ID,ci.CourseFee,ci.Doscount as Discount from tbl_Course_Name  ci inner join CourseTrac ct on ct.id=ci.TracID where ct.ID='" + ddlTracName.SelectedValue + "'", "CourseName", "ID");        


        //ddlCourseName.DataSource = clsStdCurrentStatusManager.GetCourseName(ddlTracName.SelectedValue);
        ddlCourseName.DataSource = clsStdCurrentStatusManager.GetCourseNameAssaign("");
        ddlCourseName.DataValueField = "ID";
        ddlCourseName.DataTextField = "CourseName";
        ddlCourseName.DataBind();
        ddlCourseName.Items.Insert(0, "Select Course");
        
    }
    protected void ddlCourseName_SelectedIndexChanged(object sender, EventArgs e)
    {
        //DataTable dt = clsStdCurrentStatusManager.GetCourseInfo(ddlCourseName.SelectedValue);
        //txtCourseFee.Text = dt.Rows[0]["CourseFee"].ToString();
        //txtDiscountTaka.Text = dt.Rows[0]["Discount"].ToString();
        //txtSheduleTime.Text = dt.Rows[0]["SheduleStartDate"].ToString();
        //txtAddmissionYear.Text = dt.Rows[0]["Year"].ToString();
        //txtBatch.Text = dt.Rows[0]["BatchNo"].ToString();
        //txtTrainerName.Text = dt.Rows[0]["FacultyName"].ToString();
        //lblCourseID.Text = dt.Rows[0]["CourseID"].ToString();
        //lblFAcultyID.Text = dt.Rows[0]["FacultyID"].ToString();
        //lblSheduleID.Text = dt.Rows[0]["SheduleID"].ToString();
    }
    protected void ddlTracID_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlCourseID.DataSource = clsStdCurrentStatusManager.GetCourseName("");

        ddlCourseID.DataValueField = "ID";
        ddlCourseID.DataTextField = "CourseName";
        ddlCourseID.DataBind();
        ddlCourseID.Items.Insert(0, "");
    }
    protected void txtPayAmount_TextChanged(object sender, EventArgs e)
    {
        Summation();
    }
    protected void txtDiscountTaka_TextChanged(object sender, EventArgs e)
    {
        Summation();
    }
    protected void txtWaiver_TextChanged(object sender, EventArgs e)
    {
        Summation();
    }
    protected void rbStartAmPm_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rbStartAmPm.SelectedValue == "AM")
        {
            lblTimeAM.Text = "AM";
        }
        if (rbStartAmPm.SelectedValue == "PM")
        {
            lblTimeAM.Text = "PM";
        }
        else
        {
            lblTimeAM.Text = "";
        }
    }
    protected void SatCheckBox_CheckedChanged(object sender, EventArgs e)
    {
        if (SatCheckBox.Checked == true)
        {
            SatCheckBox.Text = "1";
        }
        else
        { SatCheckBox.Text = ""; }

    }
    protected void SunCheckBox_CheckedChanged(object sender, EventArgs e)
    {
        if (SatCheckBox.Checked == true)
        {
            SatCheckBox.Text = "2";
        }
        else
        { SatCheckBox.Text = ""; }
    }
    protected void MonCheckBox_CheckedChanged(object sender, EventArgs e)
    {
        if (SatCheckBox.Checked == true)
        {
            SatCheckBox.Text = "3";
        }
        else
        { SatCheckBox.Text = ""; }
    }
    protected void TueCheckBox_CheckedChanged(object sender, EventArgs e)
    {
        if (SatCheckBox.Checked == true)
        {
            SatCheckBox.Text = "4";
        }
        else
        { SatCheckBox.Text = ""; }
    }
    protected void WedCheckBox_CheckedChanged(object sender, EventArgs e)
    {
        if (SatCheckBox.Checked == true)
        {
            SatCheckBox.Text = "5";
        }
        else
        { SatCheckBox.Text = ""; }
    }
    protected void ThusCheckBox_CheckedChanged(object sender, EventArgs e)
    {
        if (SatCheckBox.Checked == true)
        {
            SatCheckBox.Text = "6";
        }
        else
        { SatCheckBox.Text = ""; }
    }
    protected void FriCheckBox_CheckedChanged(object sender, EventArgs e)
    {
        if (SatCheckBox.Checked == true)
        {
            SatCheckBox.Text = "7";
        }
        else
        { SatCheckBox.Text = ""; }
    }
    protected void txtMailLoc_TextChanged(object sender, EventArgs e)
    {

    }
}
