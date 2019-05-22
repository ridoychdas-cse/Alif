using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class DepartmentEntryUI : System.Web.UI.Page
{
    DepartmentEntryManager aDepartmentEntryManager = new DepartmentEntryManager();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack != true)
        {
            try
            {
                RefreshAll();
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
    
    protected void DeleteButton_Click(object sender, EventArgs e)
    {
        try
        {
            DepartmentEntry aDepartmentEntry = new DepartmentEntry();
            aDepartmentEntry.DeptId = txtFacultyId.Text.Trim();
            aDepartmentEntry.DeptName = txtFacultyName.Text.Trim();
            aDepartmentEntry.CollegeName = ddlCourseName.SelectedValue;
            aDepartmentEntryManager.DeleteDepartmentInformation(aDepartmentEntry);
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
    }
    private void RefreshAll()
    {
        txtFacultyId.Text = "";
        txtFacultyName.Text = "";
        Session["Department"] = null;

        //ddlCourseName.DataSource = aDepartmentEntryManager.GetClassddlInfo();
        ddlCourseName.DataTextField = "CollegeName";
        ddlCourseName.DataValueField = "CollegeId";
        ddlCourseName.DataBind();
        ddlCourseName.Items.Insert(0, "");

        //DataTable dt = aDepartmentEntryManager.GetShowDepartmentInformationAll("");
        //dgFaculty.DataSource = dt;
        //Session["Department"] = dt;
        //dgFaculty.DataBind();

        txtFacultyId.Text = aDepartmentEntryManager.GetDepartmentAutoId();
        UpdateButton.Visible = false;
        DeleteButton.Visible = false;
        SectionSaveButton.Visible = true;
        //UpdatePanelMST.Update();
        //UpdatePanelGride.Update();
        txtFacultyName.Focus();
    }
    protected void SectionSaveButton_Click(object sender, EventArgs e)
    {
        try
        {
            DepartmentEntry aDepartmentEntry = new DepartmentEntry();
            txtFacultyId.Text = aDepartmentEntryManager.GetDepartmentAutoId();
            aDepartmentEntry.DeptId = txtFacultyId.Text.Trim();
            aDepartmentEntry.DeptName = txtFacultyName.Text.Trim();
            aDepartmentEntry.CollegeName = ddlCourseName.SelectedValue;
            aDepartmentEntryManager.SaveDepartmentInformation(aDepartmentEntry);
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
            DepartmentEntry aDepartmentEntry = new DepartmentEntry();
            aDepartmentEntry.DeptId = txtFacultyId.Text.Trim();
            aDepartmentEntry.DeptName = txtFacultyName.Text.Trim();
            aDepartmentEntry.CollegeName = ddlCourseName.SelectedValue;
            aDepartmentEntryManager.UpdateDepartmentInformation(aDepartmentEntry);
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
    
    
    
    protected void dgDepartment_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dgFaculty.DataSource = Session["Department"];
        dgFaculty.PageIndex = e.NewPageIndex;
        dgFaculty.DataBind();
    }
    protected void dgDepartment_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].Attributes.Add("style", "display:none");
                e.Row.Cells[3].Attributes.Add("style", "display:none");
            }
            else if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[1].Attributes.Add("style", "display:none");
                e.Row.Cells[3].Attributes.Add("style", "display:none");
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[1].Attributes.Add("style", "display:none");
                e.Row.Cells[3].Attributes.Add("style", "display:none");
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[1].Attributes.Add("style", "display:none");
                e.Row.Cells[3].Attributes.Add("style", "display:none");
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
    protected void dgDepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = DepartmentEntryManager.GetDepartmentDetailsInfo(dgFaculty.SelectedRow.Cells[1].Text);
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                txtFacultyId.Text = row["DeptId"].ToString();
                txtFacultyName.Text = row["DeptName"].ToString();

                //ddlCourseName.DataSource = aDepartmentEntryManager.GetClassddlInfo();
                ddlCourseName.DataTextField = "CollegeName";
                ddlCourseName.DataValueField = "CollegeId";
                ddlCourseName.DataBind();
                ddlCourseName.Items.Insert(0, "");

                ddlCourseName.SelectedValue = row["CollegeId"].ToString();

                SectionSaveButton.Visible = false;
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
    protected void ddlCollegeName_SelectedIndexChanged(object sender, EventArgs e)
    {
        //DataTable dt = aDepartmentEntryManager.GetShowDepartmentInformationAll(ddlCourseName.SelectedValue);
        //dgFaculty.DataSource = dt;
        //Session["Department"] = dt;
        //dgFaculty.DataBind();
    }
}