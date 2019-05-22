<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="VersionEntryUI.aspx.cs" Inherits="VersionEntryUI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
<div id="frmMainDiv" style="background-color:White; width:100%;">
<table  id="pageFooterWrapper">
 <tr>
	<td style="vertical-align:middle; height:100%;" align="center">        
               <asp:Button ID="DeleteButton" runat="server" Text="Delete" Width="100px" 
                    CssClass="buttonclass" onclick="DeleteButton_Click" />
            </td>	

            <td style="vertical-align:middle;" align="center">
                <asp:Button 
                    ID="UpdateButton" runat="server" Text="Update" Width="100px" 
                    CssClass="buttonclass" onclick="UpdateButton_Click" /></td> 

             <td style="vertical-align:middle; height:100%;" align="center">
                <asp:Button ID="SaveButton" runat="server" Text="Save" Width="100px" 
                    onclick="SaveButton_Click" CssClass="buttonclass" /></td>	
	<td style="vertical-align:middle;" align="center">                
                <asp:Button ID="CloseButton" runat="server" Text="Clear" Width="100px" 
                    onclick="CloseButton_Click" CssClass="buttonclass" />
            </td>      
                  
	</tr>		
	</table>

    <table style="width:100%;">
        <tr>
            <td align="center" style="height:40px;">
      <asp:Label ID="Label3" runat="server" BackColor="#CCCCCC" Font-Bold="True" 
          Font-Size="Medium" ForeColor="#CC3300" Text="Version Entry Information " 
          Width="95%" Height="25px" BorderStyle="Inset"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right" style="height:40px;" height="40" class="style1">
                <table style="width: 100%">
                    <tr>
                        <td style="width: 25%"></td>
                        <td style="width: 50%">
                         <%--<asp:UpdatePanel ID="UpdatePanelMST" runat="server" UpdateMode="Conditional"> 
                          <ContentTemplate> --%>
                            <table style="width:100%; font-size:8pt; border:solid 1px lightgray;">
                                <tr>
                                    <td align="right" style="width: 196px; height: 34px;">
                                        &nbsp;Version Id
                                    </td>
                                    <td style="width: 14px; height: 34px;">
                                    </td>
                                    <td align="left" style="height: 34px">
                <asp:TextBox ID="VersionIdTextBox" runat="server" Enabled="False" style="text-align:center"
                    Height="20px" Width="220px" SkinID="tbPlain" CssClass="TextBox" 
                    ReadOnly="True"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" style="width: 196px; height: 31px;">
                                        &nbsp;Version Name
                                    </td>
                                    <td style="width: 14px; height: 31px;">
                                    </td>
                                    <td align="left" style="height: 31px">
                <asp:TextBox ID="VersionNameTextBox" runat="server" Height="20px" 
                    Width="220px" SkinID="tbPlain" CssClass="TextBox"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" style="width: 196px; height: 35px;">
                                        Class Name
                                    </td>
                                    <td style="width: 14px; height: 35px;">
                                    </td>
                                    <td align="left" style="height: 35px">
        <asp:DropDownList SkinID="ddlPlain" ID="ddlClassName" runat="server" 
            Font-Size="8pt" Width="227px" Height="27px" AutoPostBack="True" 
                                            onselectedindexchanged="ddlClassName_SelectedIndexChanged">
     </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            <%--</ContentTemplate></asp:UpdatePanel>--%>
                        </td>
                        <td style="width:25%;"></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center" style="height:12px;" class="style1">
            </td>
        </tr>
        <tr>
            <td style="height:400px;" valign="top" align="center">
                <asp:GridView ID="VersionGridview" runat="server" AllowPaging="True" CssClass="mGrid" 
                        PageSize="30" Width="80%" Caption="Version History" 
                    onselectedindexchanged="ShiftGridview_SelectedIndexChanged" 
                    AutoGenerateColumns="False" 
                    onpageindexchanging="VersionGridview_PageIndexChanging" 
                    onrowdatabound="VersionGridview_RowDataBound" >
                        <Columns>
                            <asp:CommandField ShowSelectButton="True" />
                            <asp:BoundField DataField="version_id" HeaderText="ID" />
                            <asp:BoundField DataField="version_name" HeaderText="Version" />
                            <asp:BoundField DataField="class_id" HeaderText="Class Id" />
                            <asp:BoundField DataField="class_name" HeaderText="Class" />
                        </Columns>
                    </asp:GridView>
            </td>
        </tr>
    </table>
    </div>
</asp:Content>

