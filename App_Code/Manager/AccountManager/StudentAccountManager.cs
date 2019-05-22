using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for StudentAccountManager
/// </summary>
public class StudentAccountManager
{
    private StudentAccountGateway aStudentAccountsGatewayObj = new StudentAccountGateway();
	public StudentAccountManager()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public void SaveAccountInformation(StudentPayment aStudentPayment, List<StudentPayment> studentPaymentList)
    {
        aStudentAccountsGatewayObj.SaveAccountInformation(aStudentPayment,studentPaymentList);
    }

    public string GetShowAutoId()
    {
        return aStudentAccountsGatewayObj.GetShowAutoId();
    }
}