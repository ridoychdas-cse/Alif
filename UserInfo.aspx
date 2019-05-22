<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="UserInfo.aspx.cs" Inherits="UserInfo" Title="User Administration"  Theme="Themes"%>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">  
<div id="frmMainDiv" style="width:100%; background-color:White;">
<table style="width:100%; font-family:Verdana;">
<tr>
<td style="width:1%;"></td>
<td style="width:98%;" align="center">
<div style="vertical-align:top;">
<table  id="pageFooterWrapper">
   <tr>
   <td align="center">
       <asp:Button ID="btnDelete" runat="server" ToolTip="Delete" onclick="btnDelete_Click"  
           onclientclick="javascript:return window.confirm('Are u really want to delete these data')" 
           Text="Delete"
            
             Width="100px" Height="30px" BorderStyle="Outset" BorderWidth="1px"/> </td> 
   <td align="center"> 
       <asp:Button ID="btnSave" runat="server" ToolTip="Save" onclick="btnSave_Click" 
       Text="Save"
            
             Width="100px" Height="30px" BorderStyle="Outset" BorderWidth="1px"  />       
           </td> 
   <td align="center"> 
    <asp:Button ID="btnFind" runat="server" ToolTip="Find" 
           onclick="btnFind_Click"  Text="Find"            
             Width="100px" Height="30px" BorderStyle="Outset" BorderWidth="1px" /> </td>  
   <td align="center"> 
       <asp:Button ID="btnClear" runat="server" ToolTip="Clear" 
           onclick="btnClear_Click" Text="Clear" 
            
             Width="100px" Height="30px" BorderStyle="Outset" BorderWidth="1px"/>       
           </td>            
   </tr>
</table>
 </div> 
<br />
<table style="width:100%; background-color:White;">
<tr>
<td style="width:100%;" align="center">
<table style="border:solid 1px LightGray; width:100%; line-height:2.5;">
<tr>
<td style="width:100px; vertical-align:middle;" align="left">
<asp:Label ID="lblUserIf" runat="server" Font-Size="8pt">User ID</asp:Label>
</td>
<td align="left"><asp:TextBox SkinID="tbGray" ID="txtUserId" runat="server"  Width="100px" Font-Size="8pt" Enabled="true" MaxLength="20"></asp:TextBox>
</td>
<td style="height: 27px" >&nbsp&nbsp</td>
<td style="width:100px; vertical-align:middle;" align="left">
<asp:Label ID="lblDescription" runat="server" Font-Size="8pt">User Name</asp:Label>
</td>
<td colspan="4" align="left"> <asp:TextBox SkinID="tbGray" ID="txtDescription" runat="server" Width="362px" 
        Font-Size="8" MaxLength="50" TabIndex="1"></asp:TextBox></td>
</tr>
<tr>
<td style="width:100px; vertical-align:middle;" align="left">
<asp:Label ID="Label3" runat="server" Font-Size="8pt">Password</asp:Label>
</td>
<td align="left"> <asp:TextBox SkinID="tbGray" ID="txtPassword" runat="server"  Width="100px" Font-Size="8" Enabled="true" MaxLength="15" TextMode="Password"></asp:TextBox></td>
<td style="height: 27px" >&nbsp&nbsp</td>
<td style="width:100px; vertical-align:middle;" align="left">
<asp:Label ID="Label2" runat="server" Font-Size="8pt">User Group</asp:Label>
</td>
<td align="left"> <asp:DropDownList SkinID="ddlPlain" ID="ddlUsrGrp" runat="server"  Font-Size="8" Width="105px" TabIndex="2">
  <asp:ListItem></asp:ListItem>
  <asp:ListItem Text="Operator" Value="1"></asp:ListItem> 
  <asp:ListItem Text="Supervisor" Value="2"></asp:ListItem>
  <asp:ListItem Text="Evaluator" Value="3"></asp:ListItem>
  <asp:ListItem Text="Administrator" Value="4"></asp:ListItem>
  </asp:DropDownList>
</td>
<td style="height: 27px" >&nbsp&nbsp</td>
<td style="width:100px;">
</td>
<td> 
</td>
</tr>
<tr>
<td style="width:100px;" align="left">
<asp:Label ID="lblFax" runat="server" Font-Size="8pt">Status</asp:Label>
</td>
<td align="left"> <asp:DropDownList SkinID="ddlPlain" ID="ddlStatus" runat="server"  Font-Size="8" Width="105px" TabIndex="2">
  <asp:ListItem></asp:ListItem>
  <asp:ListItem Text="Enabled" Value="A"></asp:ListItem>
  <asp:ListItem Text="Disabled" Value="U"></asp:ListItem>
  </asp:DropDownList>
</td>
<td style="height: 27px" >&nbsp&nbsp</td>
<td style="width:100px;" align="left">
<asp:Label ID="Label1" runat="server" Font-Size="8pt">Emplyee ID</asp:Label>
</td>
<td align="left"> <asp:TextBox SkinID="tbGray" ID="txtEmpNo" runat="server"  Width="100px" Font-Size="8" Enabled="true" MaxLength="15"></asp:TextBox>
</td>
</tr>
</table>
<asp:Label ID="lblTranStatus" runat="server" Visible="false" Text="" Font-Size="8pt"></asp:Label>

</td>
</tr>
</table>
</td>
<td style="width:1%;"></td>
</tr>
</table>
</div>
</asp:Content>

