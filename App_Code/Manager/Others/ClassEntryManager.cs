using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for ClassEntryManager
/// </summary>
public class ClassEntryManager
{
    ClassEntryGateway aclassentrygateway = new ClassEntryGateway();

    public string GetClassAutoId()
    {
        return aclassentrygateway.GetClassAutoId();
    }

    public DataTable GetAllClassInformation()
    {
        DataTable table = aclassentrygateway.GetAllClassInformation();
        return table;
    }

    public void SaveTheClassInformation(ClassItem aClassitemObj)
    {
        aclassentrygateway.SaveTheClassInformation(aClassitemObj);
    }

    public void UpdateTheClassInformation(ClassItem aClassitemObj)
    {
        aclassentrygateway.UpdateTheClassInformation(aClassitemObj);
    }

    public void DeleteTheClassInformation(ClassItem aClassitemObj)
    {
        aclassentrygateway.DeleteTheClassInformation(aClassitemObj);
    }
}