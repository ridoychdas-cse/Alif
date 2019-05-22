using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for clsStdSpecialPay
/// </summary>
public class clsStdSpecialPay
{
    public string StudentId, ClassId, ClassYear, PayId, PayAmt, FromDt, ToDt, SerialNo;

	public clsStdSpecialPay()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public clsStdSpecialPay(DataRow dr)
    {
        if (dr["student_id"].ToString() != string.Empty) { this.StudentId = dr["student_id"].ToString(); }
        if (dr["class_id"].ToString() != string.Empty) { this.ClassId = dr["class_id"].ToString(); }
        if (dr["class_year"].ToString() != string.Empty) { this.ClassYear = dr["class_year"].ToString(); }
        if (dr["pay_id"].ToString() != string.Empty) { this.PayId = dr["pay_id"].ToString(); }
        if (dr["pay_amt"].ToString() != string.Empty) { this.PayAmt = dr["pay_amt"].ToString(); }
        if (dr["from_dt"].ToString() != string.Empty) { this.FromDt = dr["from_dt"].ToString(); }
        if (dr["to_dt"].ToString() != string.Empty) { this.ToDt = dr["to_dt"].ToString(); }
        if (dr["serial_no"].ToString() != string.Empty) { this.SerialNo = dr["serial_no"].ToString(); }
    }
}