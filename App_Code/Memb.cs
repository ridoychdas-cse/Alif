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
/// Summary description for Memb
/// </summary>
public class Memb
{
    public string EmpNo;
    public string Ieb;
    public string Ideb;
    public string Bcs;
    public string Bss;
    public string Bea;
    public string ProfOther;
    public string Dwes;
    public string Dwdea;
    public string Dhauks;
    public string Cba;
    public string WasaOther;

    public Memb()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public Memb(DataRow dr)
    {
        if (dr["emp_no"].ToString() != String.Empty)
        {
            this.EmpNo = dr["emp_no"].ToString();
        }
        if (dr["ieb"].ToString() != String.Empty)
        {
            this.Ieb = dr["ieb"].ToString();
        }
        if (dr["ideb"].ToString() != String.Empty)
        {
            this.Ideb = dr["ideb"].ToString();
        }
        if (dr["bcs"].ToString() != String.Empty)
        {
            this.Bcs = dr["bcs"].ToString();
        }
        if (dr["bss"].ToString() != String.Empty)
        {
            this.Bss = dr["bss"].ToString();
        }
        if (dr["bea"].ToString() != String.Empty)
        {
            this.Bea = dr["bea"].ToString();
        }
        if (dr["prof_other"].ToString() != String.Empty)
        {
            this.ProfOther = dr["prof_other"].ToString();
        }
        if (dr["dwes"].ToString() != String.Empty)
        {
            this.Dwes = dr["dwes"].ToString();
        }
        if (dr["dwdea"].ToString() != String.Empty)
        {
            this.Dwdea = dr["dwdea"].ToString();
        }
        if (dr["dhauks"].ToString() != String.Empty)
        {
            this.Dhauks = dr["dhauks"].ToString();
        }
        if (dr["cba"].ToString() != String.Empty)
        {
            this.Cba = dr["cba"].ToString();
        }
        if (dr["wasa_other"].ToString() != String.Empty)
        {
            this.WasaOther = dr["wasa_other"].ToString();
        }
    }
}
