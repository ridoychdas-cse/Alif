<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="frmDesignation.aspx.cs" Inherits="frmDesignation" Title="Designation Setup" Theme="Themes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
<script src='<%= ResolveUrl("~/Scripts/valideDate.js") %>' type="text/javascript"></script>

<div id="frmMainDiv" style="background-color:White; width:100%;">
<table style="width:100%;">
<tr>
<td style="width:1%;"></td>
<td style="width:98%;" align="center">
 <table  id="pageFooterWrapper">
 <tr>
	<td align="center"><asp:Button ID="BtnSave" runat="server" ToolTip="Save or Update Record" 
            OnClick="BtnSave_Click" TabIndex="901" Text="Save" 
        Height="25px" Width="100px" BorderStyle="Outset"  /></td>
	<td align="center">
	<asp:Button  ID="BtnFind" runat="server"  ToolTip="Find" 
            OnClick="BtnFind_Click"  TabIndex="902" Text="Find" 
        Height="25px" Width="100px" BorderStyle="Outset"  />
	</td>
	<td align="center"><asp:Button   ID="BtnDelete" runat="server"  ToolTip="Delete Record"  
            OnClick="BtnDelete_Click"  TabIndex="903" 
            onclientclick="javascript:return window.confirm('are u really want to delete  these data')" Text="Delete" 
        Height="25px" Width="100px" BorderStyle="Outset"  /></td>
	<td align="center">
        <asp:Button ID="BtnReset" runat="server" ToolTip="Clear Form" 
            OnClick="BtnReset_Click"  TabIndex="904" Text="Clear" 
        Height="25px" Width="100px" BorderStyle="Outset"  /></td>           
	</tr>
	</table>	
<table style="width:100%; font-size:8pt;">
<tr>
  <td style="width: 150px; height: 27px;" align="left">Designation ID</td>
	<td style="width: 200px; height: 27px;" align="left">
        <asp:TextBox SkinID="tbGray" ID="txtDesigCode"  CssClass="tbc"
            runat="server" Width="100px" Font-Size="8pt" MaxLength="3" TabIndex="1" ></asp:TextBox></td>
	
	<td style="width: 50px; height: 27px;" align="left"></td>	
	
	<td style="width: 100px; height: 27px;" align="left">Name</td>
	<td style="width: 200px; height: 27px;" align="left">
        <asp:TextBox SkinID="tbGray" ID="txtDesigName"  CssClass="tbl" Enabled="true" Text=""
            runat="server" Width="200px" TabIndex="2" Font-Size="8pt" MaxLength="40"></asp:TextBox> </td>        
</tr>
<tr>
  <td style="width: 150px; height: 27px;" align="left">Supervisor</td>
	<td style="width: 200px; height: 27px;" align="left">
        <asp:DropDownList SkinID="ddlPlain" ID="ddlMgrCode" runat="server" 
            Font-Size="8pt" Width="200px" Height="18px" TabIndex="8" >
     </asp:DropDownList></td>
	
	<td style="width: 50px; height: 27px;" align="left"></td>
	
	
	<td style="width: 100px; height: 27px;" align="left">Grade Code</td>
	<td style="width: 200px; height: 27px;" align="left">
        <asp:DropDownList SkinID="ddlPlain" ID="ddlGradeCode" runat="server" 
            Font-Size="8pt" Width="200px" Height="18px" TabIndex="9" >
     </asp:DropDownList> </td>        
</tr>

<tr>
  <td style="width: 150px; height: 27px;" align="left">Category 1</td>
	<td style="width: 200px; height: 27px;" align="left">
     <asp:DropDownList SkinID="ddlPlain" ID="ddlOfficerOrStaff" runat="server" Font-Size="8pt" Width="100px" Height="18px" TabIndex="10" >
     <asp:ListItem></asp:ListItem>
     <asp:ListItem Text="Teacher" Value="T"></asp:ListItem>      
     <asp:ListItem Text="Officer" Value="O"></asp:ListItem> 
     <asp:ListItem Text="Staff" Value="S"></asp:ListItem>
     </asp:DropDownList></td>
	
	<td style="width: 50px; height: 27px;" align="left"></td>
	
	
	<td style="width: 100px; height: 27px;" align="left">Category 2</td>
	<td style="width: 200px; height: 27px;" align="left">
     <asp:DropDownList SkinID="ddlPlain" ID="ddlClass" runat="server" Font-Size="8pt" Width="100px" Height="18px" TabIndex="11" >
     </asp:DropDownList> </td>        
</tr>

<tr>
  <td style="width: 150px; height: 27px;" align="left">Category 3</td>
	<td style="width: 200px; height: 27px;" align="left">
        <asp:DropDownList SkinID="ddlPlain" ID="ddlTectNtech" runat="server" Font-Size="8pt" 
            Width="100px" Height="18px" TabIndex="12" >
     <asp:ListItem></asp:ListItem>       
     <asp:ListItem Text="Non Technical" Value="N"></asp:ListItem> 
     <asp:ListItem Text="Technical" Value="T"></asp:ListItem>
     </asp:DropDownList></td>
	
	<td style="width: 50px; height: 27px;" align="left"></td>	
	
	<td style="width: 100px; height: 27px;" align="left">Short Name</td>
	<td style="width: 200px; height: 27px;" align="left">
       <asp:TextBox SkinID="tbGray" ID="txtDesigAbb"  CssClass="tbc" 
            runat="server" Width="200px"  Font-Size="8pt" Enabled="true"
            MaxLength="11" TabIndex="35" ></asp:TextBox></td>        
</tr>
</table>
<br />
<asp:GridView CssClass="mGrid" PagerStyle-CssClass="pgr"  AlternatingRowStyle-CssClass="alt" ID="dgDesig" runat="server" AutoGenerateColumns="false"
        AllowPaging="True" Width="100%" BackColor="White" BorderWidth="1px" BorderStyle="Solid"
        CellPadding="2" CellSpacing="0" BorderColor="LightGray" Font-Size="8pt" 
        AllowSorting="true" PageSize="20" ForeColor="#333333" 
        onselectedindexchanged="dgDesig_SelectedIndexChanged" 
        onpageindexchanging="dgDesig_PageIndexChanging" 
        onrowdatabound="dgDesig_RowDataBound">
  <Columns>
  <asp:CommandField ShowSelectButton="True"  ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="Blue" ItemStyle-Height="20px"/>
  <asp:BoundField  HeaderText="Desig Id" DataField="desig_code" ItemStyle-Width="0%" ItemStyle-HorizontalAlign="Center"/>
  <asp:BoundField  HeaderText="Desig. Name" DataField="desig_name" ItemStyle-Width="30%" ItemStyle-HorizontalAlign="Left"/>  
  <asp:BoundField  HeaderText="Supervisor" DataField="mgr_code" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Left"/>
  <asp:BoundField  HeaderText="Grade/Scale" DataField="grade_code" ItemStyle-Width="30%" ItemStyle-HorizontalAlign="Center"/>
  <asp:BoundField  HeaderText="Class" DataField="class" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Left"/>    
  </Columns>
                        <RowStyle BackColor="white" />
                        <PagerStyle BackColor="" HorizontalAlign="Center" />
                        <HeaderStyle BackColor="Silver" Font-Bold="True" ForeColor="Black" />
                        <AlternatingRowStyle BackColor="" />
  </asp:GridView>
</td>
<td style="width:1%;"></td>
</tr>
</table>
</div>
</asp:Content>

