<%@ Page Title="Alif Group Computer Center - Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Home.aspx.cs" Inherits="Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
<div id="frmMainDiv" style="background-color:White; width:100%; overflow:visible;">
<table style="width:100%;">
<tr>
<td style="width:100%; padding-left:10px;" align="center">
<asp:Panel ID="pnlTask" runat="server" Visible="false" Width="100%">
            <table style="width:100%;">
            <tr>
            <td style="width:20%; height:400px; vertical-align:middle;" align="left">            
            <asp:LinkButton ID="lbAuthList" runat="server" Text="Show: Authorization List" 
                    Font-Size="8pt" onclick="lbAuthList_Click"></asp:LinkButton>            
            <br />
            <br />
            <asp:LinkButton ID="lbChangePass" runat="server" Text="Change Password" 
                    Font-Size="8pt" onclick="lbChangePass_Click" ></asp:LinkButton>
            </td>
            <td style="width:2%;">
                <img src="img/box_bottom_hori.gif" alt="" width="1px" height="500px" /></td>
  <td style="width:78%; vertical-align:top;" align="left">         
  <asp:GridView  RowStyle-Height="25px" CssClass="mGrid" PagerStyle-CssClass="pgr"  AlternatingRowStyle-CssClass="alt"  ID="dgVoucher" runat="server" AutoGenerateColumns="false" 
        AllowPaging="True" Width="100%" BackColor="White" BorderWidth="1px" BorderStyle="Solid"
        CellPadding="2" CellSpacing="0" BorderColor="LightGray" Font-Size="8pt" 
        AllowSorting="true" PageSize="15" 
        onselectedindexchanged="dgVoucher_SelectedIndexChanged" 
         onpageindexchanging="dgVoucher_PageIndexChanging" >
  <HeaderStyle Font-Size="8" Font-Names="Arial" Font-Bold="True" BackColor="Silver" HorizontalAlign="center"  ForeColor="Black" />

  <Columns>
  
  <asp:CommandField ShowSelectButton="True"  ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Center"/>
  <asp:BoundField  HeaderText="Voucher No" DataField="vch_sys_no" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center"/> 
  <asp:BoundField  HeaderText="Voucher Date" DataField="value_date" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center"/>
  <asp:BoundField  HeaderText="Particulars" DataField="particulars" ItemStyle-Height="20" ItemStyle-Width="300px"></asp:BoundField>
  <asp:BoundField  HeaderText="Amount" DataField="control_amt"  ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N2}"/>
  
  </Columns>
                        <RowStyle BackColor="White" />
                        <EditRowStyle BackColor="" />
                        <PagerStyle HorizontalAlign="Center" />
                        <%--<HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />--%>
                        <AlternatingRowStyle BackColor="#F5F5F5" />
</asp:GridView>
<br />
<asp:Panel runat="server" ID="pnlChangePass">
<table style="width:250px; font-size:8pt;">
<tr>
<td style="width:150px;">User ID</td>
<td style="width:100px;">
<asp:TextBox SkinID="tbGray" ID="txtCpUserName" runat="server" Width="100px" MaxLength="18" Font-Size="9" Enabled="false"></asp:TextBox>
</td>
</tr>
<tr>
<td style="width:150px;">Current Password</td>
<td style="width:100px;">
<asp:TextBox SkinID="tbGray" ID="txtCpCurPass" runat="server" Width="100px" MaxLength="18" Font-Size="9" TextMode="Password"></asp:TextBox>
</td>
</tr>
<tr>
<td style="width:150px;">New Password</td>
<td style="width:100px;">
<asp:TextBox SkinID="tbGray" ID="txtCpNewPass" runat="server" Width="100px" MaxLength="18" Font-Size="9" TextMode="Password"></asp:TextBox>
</td>
</tr>
<tr>
<td style="width:150px;">Confirm Password</td>
<td style="width:100px;">
<asp:TextBox SkinID="tbGray" ID="txtCpConfPass" runat="server" Width="100px" MaxLength="18" Font-Size="9" TextMode="Password"></asp:TextBox>
</td>
</tr>
<tr><td colspan="2" style="height:10px;"></td></tr>
<tr>
<td style="width:150px;" align="right">
<asp:Button ID="lbCancel" runat="server" Font-Size="8pt" Text="Cancel" 
        onclick="lbCancel_Click"  Width="100px" Height="25px" BorderStyle="Outset" BorderWidth="1px" ></asp:Button>
</td>
<td style="width:100px;" align="center">
<asp:Button ID="lbChangePassword" runat="server" Font-Size="8pt" Text="Change"  Width="100px" Height="25px" BorderStyle="Outset" BorderWidth="1px" OnClick="lbChangePassword_click"></asp:Button>
</td>
</tr>
<tr><td colspan="2"><asp:Label ID="lblTranStatus" runat="server" Visible="false" Text="" Font-Size="8pt"></asp:Label></td></tr>
</table>
</asp:Panel>
</td>
</tr>
</table>
</asp:Panel>

<%-- </ContentTemplate>
 </asp:UpdatePanel>--%>
</td>
</tr>
</table>
</div>      
</asp:Content>

