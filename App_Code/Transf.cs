using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

/// <summary>
/// Summary description for Transf
/// </summary>
public class Transf
{
    public string EmpNo;
    public string OrderNo;
    public string TransDate;
    public string TransProm;
    public string BranchCode;
    public string DesigCode;

    public Transf()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public Transf(DataRow dr)
    {
        if (dr["emp_no"].ToString() != String.Empty)
        {
            this.EmpNo = dr["emp_no"].ToString();
        }
        if (dr["order_no"].ToString() != String.Empty)
        {
            this.OrderNo = dr["order_no"].ToString();
        }
        if (dr["trans_date"].ToString() != String.Empty)
        {
            this.TransDate = dr["trans_date"].ToString();
        }
        if (dr["trans_prom"].ToString() != String.Empty)
        {
            this.TransProm = dr["trans_prom"].ToString();
        }
        if (dr["branch_code"].ToString() != String.Empty)
        {
            this.BranchCode = dr["branch_code"].ToString();
        }
        if (dr["desig_code"].ToString() != String.Empty)
        {
            this.DesigCode = dr["desig_code"].ToString();
        }
    }
    public string FromDate { get; set; }

    public string ToDate { get; set; }

    public string Amount { get; set; }

    public string StudentId { get; set; }

    public string Year { get; set; }

    public string Id { get; set; }

    public string LoginBy { get; set; }
}
