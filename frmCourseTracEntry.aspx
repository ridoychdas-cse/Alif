<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="frmCourseTracEntry.aspx.cs" Inherits="frmCourseTracEntry" %>

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
                <asp:Button ID="btnSave" runat="server" Text="Save" Width="100px" 
                    onclick="btnSave_Click" CssClass="buttonclass" /></td>	
	<td style="vertical-align:middle;" align="center">                
                <asp:Button ID="CloseButton" runat="server" Text="Clear" Width="100px" 
                    onclick="CloseButton_Click" CssClass="buttonclass" />
            </td>      
                  
	</tr>		
	</table>

     <table style="width:100%;">
        <tr>
            <td align="center" >
      <asp:Label ID="Label3" runat="server" BackColor="#CCCCCC" Font-Bold="True" 
          Font-Size="Medium" ForeColor="#CC3300" Text="Course Trac Entry Information" 
          Width="95%" Height="25px" BorderStyle="Inset"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right" style="height:14px; " class="style1">
                <table style="width: 100%">
                    <tr>
                        <td style="width: 25%; height: 68px">
                        </td>
                        <td style="width:50%; height: 68px">
                            <table style="width:100%; font-size:8pt; border:solid 1px lightgray;">
                                <tr>
                                    <td align="right" style="width: 20%; height: 33px">
                                        Trac Id :</td>
                                    <td style="width: 3%; height: 33px">
                                    </td>
                                    <td align="left" style="height: 33px; width:77%">
                <asp:TextBox ID="txtColurseTracId" runat="server" style="text-align:center"
                    Height="20px" Width="45%" SkinID="tbPlain" CssClass="TextBox"></asp:TextBox>
                                        <asp:Label ID="lblID" runat="server" Visible="False"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" style="width: 20%; height: 34px;">
                                        Course Trac Name :</td>
                                    <td style="width: 3%; height: 34px;">
                                        </td>
                                    <td align="left" style="height: 34px; width:77%">
                <asp:TextBox ID="txtCourseTracName" runat="server" Height="20px" 
                    Width="98%" SkinID="tbPlain" CssClass="TextBox"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" style="height: 34px;" colspan="3">
                <asp:GridView ID="dgCourseTrac" runat="server" AllowPaging="True" CssClass="mGrid" 
                        PageSize="30" Width="100%" Caption="Course Trac History" 
                        onselectedindexchanged="dgCourseTrac_SelectedIndexChanged" 
                    AutoGenerateColumns="False" onrowdatabound="dgCourseTrac_RowDataBound">
                        <Columns>
                            <asp:CommandField ShowSelectButton="True" />
                            <asp:BoundField DataField="id" HeaderText="ID" />
                            <asp:BoundField DataField="TracId" HeaderText="Course ID" />
                            <asp:BoundField DataField="TracName" HeaderText="Course Trac Name" />
                        </Columns>
                    </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style=" width:25%; height: 68px">
                        </td>
                    </tr>
                </table>
                </td>
        </tr>
        <tr>
            <td style="height:400px;" valign="top" align="center">
                &nbsp;</td>
        </tr>
    </table>
    </div>
</asp:Content>

