using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for clsStdCurrentStatus
/// </summary>
public class clsStdCurrentStatus
{
    public string StudentId;
    public string std_admission_date;
    public string ClassId;
    public string ClassYear,Shift,Sect,version,ClassStart;
    public string Group,DeptId;

	public clsStdCurrentStatus()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public clsStdCurrentStatus(DataRow dr)
    {
        if (dr["std_admission_date"].ToString() != string.Empty) { this.std_admission_date = dr["std_admission_date"].ToString(); }
        if (dr["BatchNo"].ToString() != string.Empty) { this.BatchNo = dr["BatchNo"].ToString(); }
        if (dr["CourseFee"].ToString() != string.Empty) { this.CourseFee = dr["CourseFee"].ToString(); }
        if (dr["CourseId"].ToString() != string.Empty) { this.CourseID = dr["CourseId"].ToString(); }
        if (dr["PreviousCourseID"].ToString() != string.Empty) { this.PreviousCourseID = dr["PreviousCourseID"].ToString(); }
        if (dr["ScheduleId"].ToString() != string.Empty) { this.ScheduleId = dr["ScheduleId"].ToString(); }
        if (dr["TrainerName"].ToString() != string.Empty) { this.TrainerName = dr["TrainerName"].ToString(); }
        if (dr["tracId"].ToString() != string.Empty) { this.tracId = dr["tracId"].ToString(); }
        if (dr["Discount"].ToString() != string.Empty) { this.Discount = dr["Discount"].ToString(); }
        if (dr["Waiver"].ToString() != string.Empty) { this.Waiver = dr["Waiver"].ToString(); }
    }

    public string Roll { get; set; }

    public string AdmissionDt { get; set; }

    public string AddmissionYear { get; set; }

    public string BatchNo { get; set; }

    public string TracID { get; set; }

    public string CourseID { get; set; }

    public string CourseFee { get; set; }

    public string Waiver { get; set; }

    public string Discount { get; set; }

    public string ScheduleTime { get; set; }

    public string AddmissionDate { get; set; }

    public string LogineBy { get; set; }

    public string TrainerName { get; set; }

    public string PreviousCourseID { get; set; }

    public string ScheduleId { get; set; }

    public string tracId { get; set; }

    public string FacultyID { get; set; }

    public string PayAmount { get; set; }

    public string DueAmount { get; set; }

    public string TotalAmount { get; set; }

    public string PaidAmount { get; set; }

    public string TotalDiscount { get; set; }

    public string CurrentDue { get; set; }

    public string DateUpdate { get; set; }

    public string ScheduleTimeEnd { get; set; }

    public string APMTime { get; set; }

    public string ClassTime { get; set; }

    public string SatDay { get; set; }

    public string SunDay { get; set; }

    public string MonDay { get; set; }

    public string TuesDay { get; set; }

    public string WednessDay { get; set; }

    public string ThusDay { get; set; }

    public string FriDay { get; set; }

    public string CertificateDate { get; set; }
}