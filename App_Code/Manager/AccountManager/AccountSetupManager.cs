using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for AccountSetupManager
/// </summary>
public class AccountSetupManager
{
	AccountSetupGateway accountSetupGatewayObj=new AccountSetupGateway();

    public void GetSaveAccountSetupInformation(AccountSetup accountSetupObj)
    {
        accountSetupGatewayObj.GetSaveAccountSetupInformation(accountSetupObj);
    }

    public void UpdateAccountSetupInformation(AccountSetup accountSetupObj)
    {
        accountSetupGatewayObj.UpdateAccountSetupInformation(accountSetupObj);
    }

    public void DeleteAccountSetupInformation(AccountSetup accountSetupObj)
    {
        accountSetupGatewayObj.DeleteAccountSetupInformation(accountSetupObj);
    }

    public string GetAccountSetupAutoId()
    {
        return accountSetupGatewayObj.GetAccountSetupAutoId();
    }

    public DataTable GetAllAccountSetupInformation()
    {
        DataTable table = accountSetupGatewayObj.GetAllAccountsHeadInformation();
        return table;
    }
}