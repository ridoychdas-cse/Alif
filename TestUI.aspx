<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TestUI.aspx.cs" Inherits="TestUI" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
   <form id="form1" runat="server">
    <div>
        Enter the Id like Abc001
        <asp:TextBox ID="txtSerials" runat="server" /><br />
        Enter Numbers of rows you want
        <asp:TextBox ID="txtNumbsers" runat="server" /><br />
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="true">
            <Columns>
            </Columns>
        </asp:GridView>
        <asp:Button ID="btnGenerateRow" runat="server" OnClick="GenerateRows" Text="Generate Rows" />
    </div>
    </form>
</body>
</html>
