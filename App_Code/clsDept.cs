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
/// Summary description for clsDept
/// </summary>
public class clsDept
{
    public string DeptId;
    public string DeptName;
    public string Abvr;
    public string Costs;
    public string Credit;
    public string SEMISTERFEE;
    public string ADMISSIONFEE;
    public string NOOFSEMI;
    public string SEMIHIGHCREDIT;
    public string PERCREDITCOST, Dept_MstID;


	public clsDept()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public clsDept(DataRow dr)
    {
        if (dr["dept_id"].ToString() != string.Empty)
        {
            this.DeptId = dr["dept_id"].ToString();
        }
        if (dr["dept_name"].ToString() != string.Empty)
        {
            this.DeptName = dr["dept_name"].ToString();
        }
        if (dr["abvr"].ToString() != string.Empty)
        {
            this.Abvr = dr["abvr"].ToString();
        }
        if (dr["credit"].ToString() != string.Empty)
        {
            this.Credit = dr["credit"].ToString();
        }
        if (dr["costs"].ToString() != string.Empty)
        {
            this.Costs = dr["costs"].ToString();
        }
        if (dr["SEMISTERFEE"].ToString() != string.Empty)
        {
            this.SEMISTERFEE = dr["SEMISTERFEE"].ToString();
        }
        if (dr["ADMISSIONFEE"].ToString() != string.Empty)
        {
            this.ADMISSIONFEE = dr["ADMISSIONFEE"].ToString();
        }
        if (dr["NOOFSEMI"].ToString() != string.Empty)
        {
            this.NOOFSEMI = dr["NOOFSEMI"].ToString();
        }
        if (dr["SEMIHIGHCREDIT"].ToString() != string.Empty)
        {
            this.SEMIHIGHCREDIT = dr["SEMIHIGHCREDIT"].ToString();
        }
        if (dr["PERCREDITCOST"].ToString() != string.Empty)
        {
            this.PERCREDITCOST = dr["PERCREDITCOST"].ToString();
        }

        if (dr["Dept_MstID"].ToString() != string.Empty)
        {
            this.Dept_MstID = dr["Dept_MstID"].ToString();
        }
    }

    public string LoginBy { get; set; }

    public string ID { get; set; }
}
