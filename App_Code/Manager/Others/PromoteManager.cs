using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using KHSC.DAO.Others;
using KHSC.Gateway.Others;



namespace KHSC.Manager.Others
{

public class PromoteManager
{
    PromoteGateway aPromoteGateway=new PromoteGateway();
    

    public DataTable GetShowStudentCurrentInformation(string Year, string Class, string Section, string Version, string Shift)
    {
        DataTable table = aPromoteGateway.GetShowStudentCurrentInformation(Year, Class, Section, Version, Shift);
        return table;
    }

    public void SaveStudentPromoteInformation(PromoteStudent aPromoteStudentobj, List<PromoteStudent> PromoteStudentlist)
    {
        aPromoteGateway.SaveStudentPromoteInformation(aPromoteStudentobj, PromoteStudentlist);
    }
    public void ArchiveStudentPromoteInformation(PromoteStudent aPromoteStudentobj, List<PromoteStudent> PromoteStudentlist)
    {
        aPromoteGateway.ArchiveStudentPromoteInformation(aPromoteStudentobj, PromoteStudentlist);
    }

    public void SaveStudentGroupPromoteInformation(PromoteStudent aPromoteStudentobj, List<PromoteStudent> PromoteStudentlist)
    {
        aPromoteGateway.SaveStudentGroupPromoteInformation(aPromoteStudentobj, PromoteStudentlist);
    }

    public object GetShowStudentArchiveInformation(string Year, string Class, string Section, string Version, string Shift)
    {
        DataTable table = aPromoteGateway.GetShowStudentArchiveInformation(Year, Class, Section,Version,Shift);
        return table;
    }

    public void SaveRetrieveStudentPromoteInformation(PromoteStudent aPromoteStudentobj, List<PromoteStudent> PromoteStudentlist)
    {
        aPromoteGateway.SaveRetrieveStudentPromoteInformation(aPromoteStudentobj, PromoteStudentlist);
    }

    public void SaveRetrieveStudentGroupPromoteInformation(PromoteStudent aPromoteStudentobj, List<PromoteStudent> PromoteStudentlist)
    {
        aPromoteGateway.SaveRetrieveStudentGroupPromoteInformation(aPromoteStudentobj, PromoteStudentlist);
    }
}
}