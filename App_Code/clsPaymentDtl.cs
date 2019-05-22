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
/// Summary description for clsPaymentDtl
/// </summary>
public class clsPaymentDtl
{
    public string PaymentId;
    public string PayId;
    public string PayAmt;
    public string Discount_AMT;
    
    public clsPaymentDtl()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public clsPaymentDtl(DataRow dr)
    {
        if (dr["payment_id"].ToString() != string.Empty)
        {
            this.PaymentId = dr["payment_id"].ToString();
        }
        if (dr["pay_id"].ToString() != string.Empty)
        {
            this.PayId = dr["pay_id"].ToString();
        }
        if (dr["pay_amt"].ToString() != string.Empty)
        {
            this.PayAmt = dr["pay_amt"].ToString();
        }
        if (dr["Discount_AMT"].ToString() != string.Empty)
        {
            this.PayAmt = dr["Discount_AMT"].ToString();
        }
    }
}
