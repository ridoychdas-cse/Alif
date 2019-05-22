using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for AccountSetup
/// </summary>
public class AccountSetup
{
	public AccountSetup()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public string AccountHead { get; set; }

    public string TransportArea { get; set; }

    public string Amount { get; set; }

    public string Id { get; set; }

    public string Nature { get; set; }

    public string Class { get; set; }

    public string PostedBy { get; set; }

    public DateTime PostedDate { get; set; }
}