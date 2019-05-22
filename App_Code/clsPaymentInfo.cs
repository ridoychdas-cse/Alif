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
/// Summary description for clsPaymentInfo
/// </summary>
public class clsPaymentInfo
{
    public string PayId;
    public string PayName;
    public string PayType;
    public string PayClass,ForAllStd,Version;
    public string PayAmt, Discount, GroupID;
    
    public clsPaymentInfo()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public clsPaymentInfo(DataRow dr)
    {
        if (dr["pay_id"].ToString() != string.Empty)
        {
            this.PayId = dr["pay_id"].ToString();
        }
        if (dr["pay_head_id"].ToString() != string.Empty)
        {
            this.PayName = dr["pay_head_id"].ToString();
        }
        if (dr["pay_type"].ToString() != string.Empty)
        {
            this.PayType = dr["pay_type"].ToString();
        }
        if (dr["pay_class"].ToString() != string.Empty)
        {
            this.PayClass = dr["pay_class"].ToString();
        }
        if (dr["for_all_std"].ToString() != string.Empty)
        {
            this.ForAllStd = dr["for_all_std"].ToString();
        }
        if (dr["pay_amt"].ToString() != string.Empty)
        {
            this.PayAmt = dr["pay_amt"].ToString();
        }
        if (dr["discount"].ToString() != string.Empty)
        {
            this.Discount = dr["discount"].ToString();
        }
        if (dr["version"].ToString() != string.Empty)
        {
            this.Version = dr["version"].ToString();
        }
        if (dr["GroupID"].ToString() != string.Empty)
        {
            this.GroupID = dr["GroupID"].ToString();
        }
    }
}
