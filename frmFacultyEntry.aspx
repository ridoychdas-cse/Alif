<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="frmFacultyEntry.aspx.cs" Inherits="frmFacultyEntry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <div id="frmMainDiv" style="background-color:White; width:100%;">
<table  id="pageFooterWrapper">
 <tr>
	<td style="vertical-align:middle; height:100%;" align="center">        
            <asp:Button ID="DeleteButton" runat="server" Text="Delete" Width="100px" onclientclick="javascript:return window.confirm('are u really want to delete these data')"
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
    <table style="width:100%; height:500px;">
        <tr>
            <td align="center" style="height:40px;" height="40" class="style1">
      <asp:Label ID="Label3" runat="server" BackColor="#CCCCCC" Font-Bold="True" 
          Font-Size="Medium" ForeColor="#CC3300" Text="Faculty Entry Information" 
          Width="95%" Height="25px" BorderStyle="Inset"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right" style="height:17px;" class="style1">
                <%--<asp:UpdatePanel ID="UpdatePanelMST" runat="server" UpdateMode="Conditional"> 
                <ContentTemplate>--%>  
                <table style="width: 100%">
                    <tr>
                        <td style="width: 25%"></td>
                        <td style="width: 50%">
                        <fieldset style="vertical-align: top; border: solid .5px #8BB381;text-align:left;line-height:1.5em;">
                                            <legend style="color: maroon;"><b>Faculty Setup</b></legend>
                                            <asp:updatepanel ID="UP1" runat="server" UpdateMode="Conditional"><ContentTemplate>
                            <table  style="width:100%; font-size:8pt; border:solid 1px lightgray;">
                                <tr>
                                    <td style=" width:20%; height: 28px" valign="middle" align="right">
                                        <asp:Label ID="Label4" runat="server" Text="Faculty Id"></asp:Label>
                                    </td>
                                    <td style="height: 28px; width:2% " >
                                        </td>
                                    <td align="left" style="height: 28px; width:28%" valign="middle">
                <asp:TextBox ID="txtFacultyId" runat="server" Width="98%" SkinID="tbPlain" 
                                            CssClass="TextBox" style="text-align:center" Height="20px"></asp:TextBox>
                                    </td>
                                    <td  style="width: 20%" align="right">
                                        <asp:Label ID="Label5" runat="server" Text="Faculty Type"></asp:Label>
                                    </td>
                                    <td  style="width: 2%"></td>
                                    <td  style="width: 28%">
                                        <asp:DropDownList ID="ddlFacultyType" runat="server" Height="26px" Width="100%">
                                            <asp:ListItem></asp:ListItem>
                                            <asp:ListItem Value="1">Parmanent</asp:ListItem>
                                            <asp:ListItem Value="2">Temporery</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 20%; height: 31px;" align="right">
                                        Faculty Name
                                    </td>
                                    <td style="width: 2%; height: 31px;">
                                        </td>
                                    <td align="left" style="height: 31px; " colspan="4">
                <asp:TextBox ID="txtFacultyName" runat="server" 
                    Width="98%" SkinID="tbPlain" CssClass="TextBox" Height="20px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 20%; height: 28px;" align="right">
                                        Course Name
                                    </td>
                                    <td style="width:2%; height: 28px;">
                                        </td>
                                    <td align="left" style="height: 28px; " colspan="4">
        <asp:DropDownList SkinID="ddlPlain" ID="ddlCourseName" runat="server" 
            Font-Size="8pt" Width="100%" Height="26px" 
                                           >
     </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" style="width: 20%; height: 28px;">
                                        <asp:Label ID="Label7" runat="server" Text="Designation"></asp:Label>
                                    </td>
                                    <td style="width:2%; height: 28px;">
                                        &nbsp;</td>
                                    <td align="left" style="height: 28px; width:28% ">
                                        <asp:DropDownList ID="ddlDesignation" runat="server" Height="26px" Width="100%">
                                        </asp:DropDownList>
                                    </td>
                                    <td align="right" style="width: 20%">
                                        <asp:Label ID="lblID" runat="server" Visible="False"></asp:Label>
                                        <asp:Label ID="Label6" runat="server" Text="Status"></asp:Label>
                                    </td>
                                    <td style="width: 2%">
                                        &nbsp;</td>
                                    <td style="width: 28%">
                                        <asp:DropDownList ID="ddlStatus" runat="server" Height="26px" Width="100%">
                                            <asp:ListItem></asp:ListItem>
                                            <asp:ListItem Value="1">Active</asp:ListItem>
                                            <asp:ListItem Value="2">In-Active</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" colspan="6" style="height: 28px;">
                                        <asp:GridView ID="dgFaculty" runat="server" AllowPaging="True" 
                                            AutoGenerateColumns="False" Caption="Faculty History" CssClass="mGrid" 
                                            onpageindexchanging="dgFaculty_PageIndexChanging" 
                                            onrowdatabound="dgFaculty_RowDataBound" 
                                            onselectedindexchanged="dgFaculty_SelectedIndexChanged" PageSize="30" 
                                            Width="100%">
                                            <Columns>
                                                <asp:CommandField ShowSelectButton="True" />
                                                <asp:BoundField DataField="ID" HeaderText="ID" />
                                                <asp:BoundField DataField="FacultyName" HeaderText="Faculty Name" />
                                                <asp:BoundField DataField="CourseName" HeaderText="Course Name" />
                                                <asp:BoundField DataField="desig_name" HeaderText="Designation" />
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                            </ContentTemplate></asp:updatepanel>
                                        </fieldset>
                        </td>
                        <td style="width: 25%"></td>
                    </tr>
                </table>
               <%-- </ContentTemplate>                
               </asp:UpdatePanel>--%>
            </td>
        </tr>
        <tr>
            <td>
            <%--<asp:UpdatePanel ID="UpdatePanelGride" runat="server" UpdateMode="Conditional"> 
                <ContentTemplate>  --%>
                   <%-- </ContentTemplate></asp:UpdatePanel>--%>
            </td>
        </tr>
    </table>
    </div>
</asp:Content>

