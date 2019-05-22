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
/// Summary description for clsSubOfferDtl
/// </summary>
public class clsSubOfferDtl
{
    public string OfferId;
    public string SubjectId;
    
    public clsSubOfferDtl()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public clsSubOfferDtl(DataRow dr)
    {
        if (dr["offer_id"].ToString() != string.Empty)
        {
            this.OfferId = dr["offer_id"].ToString();
        }
        if (dr["subject_id"].ToString() != string.Empty)
        {
            this.SubjectId = dr["subject_id"].ToString();
        }
    }
}
