using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;

public partial class TestUI : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    private void CreateGridView()
    {
        int numbers = int.Parse(this.txtNumbsers.Text.Trim());
        DataTable dt = new DataTable();
        //you can add as many rows you want
        dt.Columns.Add("ID", typeof(string));
        dt.Columns.Add("Name", typeof(string));
        dt.Columns.Add("Number", typeof(int));
        for (int i = 0; i < numbers; i++)
        {
            //dont forget to add null values in each column
            dt.Rows.Add("", "", null);
        }
        GridView1.DataSource = dt;
        GridView1.DataBind();

    }
    protected void GenerateRows(object sender, EventArgs e)
    {
        this.CreateGridView();
        int numbers = int.Parse(this.txtNumbsers.Text.Trim());
        string serialNumber = this.txtSerials.Text.Trim();
        string newSerialNumber = serialNumber.Substring(0, serialNumber.Length - 2);
        //Here i am adding label if you dont want it then comment till end of for loop
        int cellCount = this.GridView1.Rows[0].Cells.Count;
        int rowsCount = this.GridView1.Rows.Count;
        foreach (GridViewRow row in this.GridView1.Rows)
        {
            Label label = new Label();
            label.ID = newSerialNumber + (Convert.ToInt32(row.RowIndex + 1)).ToString();
            label.Text = newSerialNumber + (Convert.ToInt32(row.RowIndex + 1)).ToString();
            label.Attributes.Add("runat", "server");
            label.CssClass = "Color";
            row.Cells[0].Controls.Add(label);
        }
        foreach (GridViewRow row in this.GridView1.Rows)
        {
            TextBox textBox = new TextBox();
            textBox.ID = newSerialNumber + (Convert.ToInt32(row.RowIndex + 1)).ToString();
            // textBox.Text = newSerialNumber + (Convert.ToInt32(row.RowIndex + 1)).ToString();
            textBox.Attributes.Add("runat", "server");
            textBox.BackColor = Color.Pink;
            row.Cells[1].Controls.Add(textBox);
        }
        foreach (GridViewRow row in this.GridView1.Rows)
        {
            TextBox textBox = new TextBox();
            textBox.ID = newSerialNumber + (Convert.ToInt32(row.RowIndex + 1)).ToString();
            // textBox.Text = newSerialNumber + (Convert.ToInt32(row.RowIndex + 1)).ToString();
            textBox.Attributes.Add("runat", "server");
            textBox.BackColor = Color.Pink;
            row.Cells[2].Controls.Add(textBox);
        }
    }
}