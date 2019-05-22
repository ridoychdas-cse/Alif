using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for InstallmentFee
/// </summary>
public class InstallmentFee
{
	public InstallmentFee()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public string Id { get; set; }

    public string InsAmt { get; set; }

    public string InsDate { get; set; }

    public string StudentId { get; set; }

    public string TotalAmt { get; set; }

    public string PayDate { get; set; }

    public string MonthInterval { get; set; }

    public string InsQty { get; set; }

    public string AdmissionFee { get; set; }

    public string Loginby { get; set; }
}