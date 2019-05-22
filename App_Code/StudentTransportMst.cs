using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for StudentTransportMst
/// </summary>
public class StudentTransportMst
{


    public String TMST_Std_Id { get; set; }
    public String TMST_Id { get; set; }
    public String TMST_Std_Year { get; set; }
    public String T_Pay_amt { get; set; }
    public String From_Date { get; set; }
    public String To_Date { get; set; }
    public String Mst_Id { get; set; }
    
     
	public StudentTransportMst()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    //public StudentTransportMst(DataRow dr)
    //{
       
        

    //    if (dr["Std_Id"].ToString() != String.Empty)
    //    {
    //        this.Std_Id = dr["Std_Id"].ToString();
    //    }
         
    //    if (dr["Std_Year"].ToString() != String.Empty)
    //    {
    //        this.Std_Year = dr["Std_Year"].ToString();
    //    }
        
    //    if (dr["Pay_Amt"].ToString() != String.Empty)
    //    {
    //        this.Pay_Amt = dr["Pay_Amt"].ToString();
    //    }
    //    if (dr["Start_Date"].ToString() != String.Empty)
    //    {
    //        this.Start_Date = dr["Start_Date"].ToString();
    //    }
    //    if (dr["End_Date"].ToString() != String.Empty)
    //    {
    //        this.End_Date = dr["End_Date"].ToString();
    //    }
    // }

    public string GetShowAutoId()
    {
        throw new NotImplementedException();
    }
}