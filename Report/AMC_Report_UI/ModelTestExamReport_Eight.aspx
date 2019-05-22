<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ModelTestExamReport_Eight.aspx.cs" Inherits="Report_UI_ModelTestExamReport_Eight" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <fieldset>
    <legend><b>  Search Option </b></legend>
    <div>
    
        &nbsp;<asp:Label ID="Label2" runat="server" Text="Year :"></asp:Label>
        &nbsp;<asp:TextBox ID="YearTextBox" runat="server" Height="22px" Width="151px"></asp:TextBox>
&nbsp;
    
        <asp:Label ID="Label1" runat="server" Text="Student Id :"></asp:Label>
        &nbsp;<asp:TextBox ID="IdTextTextBox" runat="server" Height="22px" Width="151px"></asp:TextBox>
        &nbsp;<asp:Button ID="SearchButton" runat="server" Text="Search " />
    
    &nbsp;
        <asp:Button ID="refreshButton" runat="server" onclick="refreshButton_Click" 
            Text="Refresh" />
    
    </div>
    </fieldset>        
    <div>
        <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" 
            AutoDataBind="true" onload="CrystalReportViewer1_Load" />
    </div>
    </form>
</body>
</html>
