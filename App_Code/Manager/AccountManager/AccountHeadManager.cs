using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for AccountHeadManager
/// </summary>
public class AccountHeadManager
{
    private AccountHeadGateway accountHeadGatewayObj = new AccountHeadGateway();


    public void SaveTheAcccountInformation(AccountHead accountHeadObj)
    {
        accountHeadGatewayObj.SaveTheAccountHeadInformation(accountHeadObj);
    }

    public string GetAccountAutoId()
    {
        return accountHeadGatewayObj.GetAccountHeadAutoId();
    }

    public DataTable GetAllAccountsInformation()
    {
        DataTable table = accountHeadGatewayObj.GetAllAccountsHeadInformation();
        return table;
    }

    public void DeleteTheAcccountInformation(AccountHead accountHeadObj)
    {
        accountHeadGatewayObj.DeleteTheAcccountInformation(accountHeadObj);
    }

    public void UpdateTheAcccountInformation(AccountHead accountHeadObj)
    {
        accountHeadGatewayObj.UpdateTheAcccountInformation(accountHeadObj);
    }

    
}