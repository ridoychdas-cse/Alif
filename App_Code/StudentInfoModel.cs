using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for StudentInfoModel
/// </summary>
public class StudentInfoModel
{
	public StudentInfoModel()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public string SlNo { get; set; }

    public string NID { get; set; }

    public string Name { get; set; }

    public string StdPhone { get; set; }

    public string FthName { get; set; }

    public string FthPhone { get; set; }

    public string MthName { get; set; }

    public string MthPhone { get; set; }


    public string DBO { get; set; }

    public string Email { get; set; }

    public string Nationality { get; set; }

    public string Religion { get; set; }

    public string MaritalStatus { get; set; }

    public string LastEducation { get; set; }
    public string Bord { get; set; }
    public decimal Result { get; set; }

    public Byte[] StdPhoto { get; set; }

    public string PresentAddress { get; set; }

    public string ParVill { get; set; }

    public string ParPO { get; set; }

    public string ParPS { get; set; }

    public string ParDis { get; set; }

    public string CourseName { get; set; }

    public string TrainerName { get; set; }

    public string BatchNo { get; set; }

    public string ClassTime { get; set; }

    public string APM { get; set; }

    public string AdmissionDate { get; set; }

    public decimal CourseFee { get; set; }

    public decimal Waiver { get; set; }

    public decimal DisCount { get; set; }

    public decimal TotalAmount { get; set; }

    public decimal PayAmount { get; set; }

    public decimal Due { get; set; }

    public string AddmissionYear { get; set; }

    public string CertificationDate { get; set; }

    public string SatDay { get; set; }

    public string SunDay { get; set; }

    public string MonDay { get; set; }

    public string TuesDay { get; set; }

    public string WednessDay { get; set; }

    public string ThusDay { get; set; }

    public string FriDay { get; set; }
}