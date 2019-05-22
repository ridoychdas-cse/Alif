using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for clsCourseSheduleAssign
/// </summary>
public class clsCourseSheduleAssign
{
	public clsCourseSheduleAssign()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public string TracID { get; set; }

    public string CourseID { get; set; }

    public string FacultyID { get; set; }

    public string BatchNo { get; set; }

    public string Status { get; set; }

    public string StartDate { get; set; }

    public string EndDate { get; set; }

    public string Year { get; set; }

    public string LoginBy { get; set; }
}