<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="TrainReport.aspx.cs" Inherits="TrainReport" Title="Personnel Training Report" Theme="Themes" %>
<%--<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
<div id="frmMainDiv" style="background-color:White; width:100%;">
<table style="width:100%;">
<tr>
<td style="width:10%;"></td>
<td style="width:10%;" align="center">
<table style="width:600px;">
<tr>
<td style="width:200px; text-align:left;">
<table>
<tr><td style="font-size:8pt; color:Blue;">Select Criteria :</td></tr>
<tr>
<td>
    <asp:RadioButtonList ID="rdoSelectCriteria" runat="server" Font-Size="8pt" AutoPostBack="true" 
        BorderWidth="1" onselectedindexchanged="rdoSelectCriteria_SelectedIndexChanged">
    <asp:ListItem Text="By Year" Value="YEAR"></asp:ListItem>
    <asp:ListItem Text="By Branch/Unit Office" Value="BRANCH"></asp:ListItem>
    <asp:ListItem Text="By Training Title" Value="TITLE"></asp:ListItem>
    <asp:ListItem Text="Selected Emplyoee" Value="EMP"></asp:ListItem>
    <asp:ListItem Text="Select All Employee" Value="All" Selected="True"></asp:ListItem>
    </asp:RadioButtonList>
</td>
</tr>
</table>
</td>
<td style="width:200px; text-align:left;">
<table>
<tr><td style="font-size:8pt; color:Blue;">Select Report Type :</td></tr>
<tr>
<td>
    <asp:RadioButtonList ID="rdoReportType" runat="server" Font-Size="8pt" BorderWidth="1">
    <asp:ListItem Text="Detail Information" Value="D"></asp:ListItem>
    <asp:ListItem Text="Simple List" Value="S" Selected="True"></asp:ListItem>
    </asp:RadioButtonList>
</td>
</tr>
</table>
</td>
<td style="width:200px; text-align:left;">
<table>
<tr>
<td align="center">
<asp:Button ID="btnShow" runat="server" SkinId="lbPlain" 
        Text="Show" onclick="btnShow_Click"  Width="100px" Height="25px" BorderStyle="Outset" BorderWidth="1px" />    
</td>
</tr>
<tr>
<td align="center">
<asp:Button ID="btnReset" runat="server" SkinId="lbPlain"
        Text="Reset" onclick="btnReset_Click"  Width="100px" Height="25px" BorderStyle="Outset" BorderWidth="1px"  />    
</td>
</tr>
</table>
</td>
</tr>
<tr><td colspan="3"></td></tr>
<tr><td colspan="3" align="left">
<table>
<tr>
<td style="width:100px; font-size:8pt;">Year</td>
<td align="left"><asp:TextBox SkinID="tbPlain" ID="txtYear"  CssClass="tbc" runat="server" Width="100px" Font-Size="8pt" MaxLength="4" TabIndex="2"></asp:TextBox></td>
<td align="left"></td>
</tr>
<tr>
<td style="width: 100px; height: 27px; font-size:8pt;" align="left">Employee ID</td>
<td style="width: 100px; height: 27px;" align="left">
<asp:TextBox SkinID="tbPlain" ID="txtEmployeeId"  CssClass="tbc" runat="server"
            Width="100px" Font-Size="8pt" MaxLength="11" TabIndex="2" 
        AutoPostBack="True" ontextchanged="txtEmployeeId_TextChanged"></asp:TextBox></td>	
<td style="width: 200px; height: 27px; font-size:8pt;" align="left">
	<asp:TextBox SkinID="tbGray" ID="txtEmployeeName" Enabled="false"  CssClass="tbl" runat="server" Width="195px" Font-Size="8pt" MaxLength="11" TabIndex="3" ></asp:TextBox> 
</td>
</tr>
<tr>
<td style="width: 100px; height: 27px; font-size:8pt;" align="left">Training Title</td>
<td style="width: 300px; height: 27px; font-size:8pt;" align="left" colspan="2">
	<asp:TextBox SkinID="tbGray" ID="txtTrainTitle" CssClass="tbl" runat="server" Width="300px" Font-Size="8pt" MaxLength="50" TabIndex="3" ></asp:TextBox> 
</td>
</tr>
<tr>
<td style="width: 100px; height: 27px; font-size:8pt;" align="left">Branch</td>
<td style="width: 300px; height: 27px; font-size:8pt;" align="left" colspan="2">
	<asp:DropDownList SkinID="ddlPlain" ID="ddlBranchId" runat="server" Font-Size="8pt" Width="300px" Height="18px" TabIndex="5" >
     </asp:DropDownList>
</td>
</tr>
</table>
</td></tr>
</table>
<img alt="" height="1px" src="img/box_bottom_filet.gif" width="100%" />
<asp:Label ID="lblTranStatus" runat="server" Font-Size="8pt" Text="" Visible="false"></asp:Label>
</td>
<td style="width:10%;"></td>
</tr>
</table>
</div>
<%--<CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" 
    AutoDataBind="true" />--%>
</asp:Content>

