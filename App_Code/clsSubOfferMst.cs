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
/// Summary description for clsSubOfferMst
/// </summary>
public class clsSubOfferMst
{
    public string OfferId;
    public string OfferDate;
    public string Sessions;
    public string StudyYear;
    public string DeptId;
    public string BatchNo;
    public string Sections;

    public clsSubOfferMst()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public clsSubOfferMst(DataRow dr)
    {
        if (dr["offer_id"].ToString() != string.Empty)
        {
            this.OfferId = dr["offer_id"].ToString();
        }
        if (dr["offer_date"].ToString() != string.Empty)
        {
            this.OfferDate = dr["offer_date"].ToString();
        }
        if (dr["sessions"].ToString() != string.Empty)
        {
            this.Sessions = dr["sessions"].ToString();
        }
        if (dr["study_year"].ToString() != string.Empty)
        {
            this.StudyYear = dr["study_year"].ToString();
        }
        if (dr["dept_id"].ToString() != string.Empty)
        {
            this.DeptId = dr["dept_id"].ToString();
        }
        if (dr["batch_no"].ToString() != string.Empty)
        {
            this.BatchNo = dr["batch_no"].ToString();
        }
        if (dr["sections"].ToString() != string.Empty)
        {
            this.Sections = dr["sections"].ToString();
        }
    }

    public string LoginBy { get; set; }

    public string DeptTypeID { get; set; }
}
