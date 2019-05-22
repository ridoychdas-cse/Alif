using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class VersionEntryUI : System.Web.UI.Page
{
    ClsVersionManager aClsVersionManagerObj = new ClsVersionManager();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
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
    protected void SaveButton_Click(object sender, EventArgs e)
    {
        try
        {
            ClsVersion aClsVersionObj = new ClsVersion();
            VersionIdTextBox.Text = aClsVersionManagerObj.AutoId();
            aClsVersionObj.VersionId = VersionIdTextBox.Text.Trim();
            aClsVersionObj.VersionName = VersionNameTextBox.Text.Trim();
            aClsVersionObj.ClassId = ddlClassName.SelectedValue;

            aClsVersionManagerObj.SaveVersionInfo(aClsVersionObj);
            RefreshAll();
            ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Information save successfully');", true);
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
            ClsVersion aClsVersionObj = new ClsVersion();
            aClsVersionObj.VersionId = VersionIdTextBox.Text.Trim();
            aClsVersionObj.VersionName = VersionNameTextBox.Text.Trim();
            aClsVersionObj.ClassId = ddlClassName.SelectedValue;

            aClsVersionManagerObj.UpdateVersionInfo(aClsVersionObj);
            RefreshAll();
            ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Information update successfully');", true);
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
            ClsVersion aClsVersionObj = new ClsVersion();
            aClsVersionObj.VersionId = VersionIdTextBox.Text.Trim();
            aClsVersionObj.VersionName = VersionNameTextBox.Text.Trim();
            aClsVersionObj.ClassId = ddlClassName.SelectedValue;

            aClsVersionManagerObj.DeleteVersionInfo(aClsVersionObj);
            RefreshAll();
            ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Information delete successfully');", true);
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

    private void RefreshAll()
    {
        VersionIdTextBox.Text = "";
        VersionNameTextBox.Text = "";
        ViewState["Version"] = null;

        DataTable dt = aClsVersionManagerObj.GetVersionInfo("");
        VersionGridview.DataSource = dt;
        ViewState["Version"] = dt;
        VersionGridview.DataBind();

        ddlClassName.DataSource = aClsVersionManagerObj.GetClassddlInfo();
        ddlClassName.DataTextField = "class_name";
        ddlClassName.DataValueField = "class_id";
        ddlClassName.DataBind();
        ddlClassName.Items.Insert(0, "");
        VersionIdTextBox.Text = aClsVersionManagerObj.AutoId();
        UpdateButton.Visible = false;
        DeleteButton.Visible = false;
        SaveButton.Visible = true;
        VersionNameTextBox.Focus();
    }
    protected void ShiftGridview_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = ClsVersionManager.GetVersionDetailsInfo(VersionGridview.SelectedRow.Cells[1].Text);
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                VersionIdTextBox.Text = row["version_id"].ToString();
                VersionNameTextBox.Text = row["version_name"].ToString();

                ddlClassName.DataSource = aClsVersionManagerObj.GetClassddlInfo();
                ddlClassName.DataTextField = "class_name";
                ddlClassName.DataValueField = "class_id";
                ddlClassName.DataBind();
                ddlClassName.Items.Insert(0, "");

                ddlClassName.SelectedValue = row["class_id"].ToString();

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
    protected void ddlClassName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["Version"] = null;
        DataTable dt = aClsVersionManagerObj.GetVersionInfo(ddlClassName.SelectedValue);
        VersionGridview.DataSource = dt;
        ViewState["Version"] = dt;
        VersionGridview.DataBind();
    }
    protected void VersionGridview_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        VersionGridview.DataSource = ViewState["Version"];
        VersionGridview.PageIndex = e.NewPageIndex;
        VersionGridview.DataBind();
    }
    protected void VersionGridview_RowDataBound(object sender, GridViewRowEventArgs e)
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
}