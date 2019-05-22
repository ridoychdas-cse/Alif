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
/// Summary description for clsPaymentMst
/// </summary>
public class clsPaymentMst
{
    public string PaymentId;
    public string StudentId;
    public string ClassId;
    public string PayYear;
    public string PayDate;
    public string PayMode;
    public string ChequeNo;
    public string ChequeDate;
    public string ChequeAmt;
    public string BankNo;
    public string RefNo;
    public string EntryUser;
    public string EntryDate;
    public string UpdateUser;
    public string UpdateDate;
    public string Section;
    public string SectionName;
    public string ClassName;
    public string PayAmt;
    public string TotalPaidAmt;
    public string TotalDiscountAmt;
    public string Shift;
    public string Version;

    
    public clsPaymentMst()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public clsPaymentMst(DataRow dr)
    {
        if (dr["payment_id"].ToString() != string.Empty)
        {
            this.PaymentId = dr["payment_id"].ToString();
        }
        if (dr["student_id"].ToString() != string.Empty)
        {
            this.StudentId = dr["student_id"].ToString();
        }
        if (dr["class_id"].ToString() != string.Empty)
        {
            this.ClassId = dr["class_id"].ToString();
        }
        if (dr["sect"].ToString() != string.Empty)
        {
            this.Section = dr["sect"].ToString();
        }
        if (dr["pay_year"].ToString() != string.Empty)
        {
            this.PayYear = dr["pay_year"].ToString();
        }
        if (dr["pay_date"].ToString() != string.Empty)
        {
            this.PayDate = dr["pay_date"].ToString();
        }
        if (dr["pay_mode"].ToString() != string.Empty)
        {
            this.PayMode = dr["pay_mode"].ToString();
        }
        if (dr["cheque_no"].ToString() != string.Empty)
        {
            this.ChequeNo = dr["cheque_no"].ToString();
        }
        if (dr["cheque_date"].ToString() != string.Empty)
        {
            this.ChequeDate = dr["cheque_date"].ToString();
        }
        if (dr["cheque_amt"].ToString() != string.Empty)
        {
            this.ChequeAmt = dr["cheque_amt"].ToString();
        }
        if (dr["bank_no"].ToString() != string.Empty)
        {
            this.BankNo = dr["bank_no"].ToString();
        }
        if (dr["ref_no"].ToString() != string.Empty)
        {
            this.RefNo = dr["ref_no"].ToString();
        }
        if (dr["entry_user"].ToString() != String.Empty)
        {
            this.EntryUser = dr["entry_user"].ToString();
        }
        if (dr["entry_date"].ToString() != String.Empty)
        {
            this.EntryDate = dr["entry_date"].ToString();
        }
        if (dr["update_user"].ToString() != String.Empty)
        {
            this.UpdateUser = dr["update_user"].ToString();
        }
        if (dr["update_date"].ToString() != String.Empty)
        {
            this.UpdateDate = dr["update_date"].ToString();
        }
        if (dr["class_name"].ToString() != String.Empty)
        {
            this.ClassName = dr["class_name"].ToString();
        }
        if (dr["sec_name"].ToString() != String.Empty)
        {
            this.SectionName = dr["sec_name"].ToString();
        }

        if (dr["Pay_Amount"].ToString() != String.Empty)
        {
            this.PayAmt = dr["Pay_Amount"].ToString();
        }
        if (dr["Total_Paid_Amt"].ToString() != String.Empty)
        {
            this.TotalPaidAmt = dr["Total_Paid_Amt"].ToString();
        }
        if (dr["DiscountAmt"].ToString() != String.Empty)
        {
            this.TotalDiscountAmt = dr["DiscountAmt"].ToString();
        }
        if (dr["shift"].ToString() != String.Empty)
        {
            this.Shift = dr["shift"].ToString();
        }
        if (dr["version"].ToString() != String.Empty)
        {
            this.Version = dr["version"].ToString();
        }

         
    }
    
}
