using System;
using System.Data;
using System.Configuration;
//using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
//using System.Xml.Linq;

/// <summary>
/// Summary description for clsStdWaiver
/// </summary>
public class clsStdWaiver
{
    public string StudentId;
    public string ClassId;    
    public string WaiveYear;
    public string WaivePct;
    public string ExcFrom;
    public string ExcTo;
    public string WaiveSl;
    public string Remarks;

    public clsStdWaiver()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public clsStdWaiver(DataRow dr)
    {
        if (dr["student_id"].ToString() != string.Empty)
        {
            this.StudentId = dr["student_id"].ToString();
        }
        if (dr["class_id"].ToString() != string.Empty)
        {
            this.ClassId = dr["class_id"].ToString();
        }       
        if (dr["waive_year"].ToString() != string.Empty)
        {
            this.WaiveYear = dr["waive_year"].ToString();
        }
        if (dr["waive_pct"].ToString() != string.Empty)
        {
            this.WaivePct = dr["waive_pct"].ToString();
        }
        if (dr["exc_from"].ToString() != string.Empty)
        {
            this.ExcFrom = dr["exc_from"].ToString();
        }
        if (dr["exc_to"].ToString() != string.Empty)
        {
            this.ExcTo = dr["exc_to"].ToString();
        }
        if (dr["waive_sl"].ToString() != string.Empty)
        {
            this.WaiveSl = dr["waive_sl"].ToString();
        }
        if (dr["remarks"].ToString() != string.Empty)
        {
            this.Remarks = dr["remarks"].ToString();
        }
    }
    
}
