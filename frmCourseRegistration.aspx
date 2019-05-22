<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="frmCourseRegistration.aspx.cs" Inherits="frmCourseRegistration" %>

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
          Font-Size="Medium" ForeColor="#CC3300" Text="Course Entry Information" 
          Width="95%" Height="25px" BorderStyle="Inset"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right" style="height:14px; " class="style1">
                <table style="width: 100%">
                    <tr>
                        <td style="width: 15%">
                        </td>
                        <td style="width: 70%"></td>
                        <td></td>
                        <td style="width:15%">                         
                    
                    </tr>
                    <tr>
                        <td style="width: 15%">
                            &nbsp;</td>
                        <td style="width: 70%">
                         <fieldset style="vertical-align: top; border: solid .5px #8BB381;text-align:left;line-height:1.5em;">
                                            <legend style="color: maroon;"><b>Course Setup</b></legend>
                                            <asp:updatepanel ID="UP1" runat="server" UpdateMode="Conditional"><ContentTemplate>
                        <table style="width:100%">
                        <tr>
                        <td style="width:20%" align="right"> 
                            <asp:Label ID="Label4" runat="server" Text="Course Id"></asp:Label>
                            </td>
                        <td style="width:2%"></td>
                        <td style="width:28%" align="left">
                        <asp:TextBox ID="CourseIdTextBox" runat="server" style="text-align:center"
                    Height="20px" Width="100%" SkinID="tbPlain" CssClass="TextBox"></asp:TextBox></td>
                     <td style="width:20%" align="right"> 
                           
                            <asp:Label ID="Label9" runat="server" Text="Trac Name"></asp:Label>
                           
                            </td>
                        <td style="width:2%"></td>
                        <td style="width:28%" align="left">
                            <asp:DropDownList ID="ddlCourseName" runat="server" Font-Size="8pt" 
                                Height="26px"  
                                SkinID="ddlPlain" Width="99%">
                            </asp:DropDownList>
                       </td>
                        </tr>
                        <tr>
                        <td style="width:20%" align="right"> 
                            <asp:Label ID="Label5" runat="server" Text="Course Name"></asp:Label>
                            </td>
                        <td style="width:2%">&nbsp;</td>
                        <td align="left" colspan="4"> 
                            <asp:TextBox ID="txtCourseNameTextBox" 
                                runat="server" Height="20px" 
                    Width="100%" SkinID="tbPlain" CssClass="TextBox"></asp:TextBox></td>
                        </tr>
                        <tr>
                        <td style="width:20%" align="right"> 
                            <asp:Label ID="Label10" runat="server" Text="Course Fee"></asp:Label>
                            </td>
                        <td style="width:2%">&nbsp;</td>
                        <td style="width:28%" align="left">
                            <asp:TextBox ID="txtCourseFee" runat="server" Width="50%"></asp:TextBox>
                            </td>
                 <td style="width:20%" align="right">  
                     <asp:Label ID="Label11" runat="server" Text="Discount Offer"></asp:Label>
                            </td>
                        <td style="width:2%"><b style="color: #FF0000">%</b></td>
                        <td style="width:28%" align="left"> 
                            <asp:TextBox ID="txtDiscount" runat="server" Width="40%"></asp:TextBox>
                            </td>
                        </tr>
                        </table>
                        </ContentTemplate></asp:updatepanel>
                                        </fieldset>
                        </td>
                        <td>&nbsp;</td>
                        <td style="width:15%">                         
                    
                            &nbsp;</tr>
                    <tr>
                        <td style="width: 15%">
                            &nbsp;</td>
                        <td style="width: 70%">
                            <asp:Label ID="lblID" runat="server" Visible="False"></asp:Label>
                        </td>
                        <td>&nbsp;</td>
                        <td style="width:15%">                         
                    
                            &nbsp;</tr>
                    <tr>
                        <td style="width: 15%">
                            &nbsp;</td>
                        <td style="width: 70%">
                <asp:GridView ID="dgCourse" runat="server" AllowPaging="True" CssClass="mGrid" 
                        PageSize="30" Width="100%" Caption="Course History" 
                        onselectedindexchanged="dgCourse_SelectedIndexChanged" 
                    AutoGenerateColumns="False" onrowdatabound="dgCourse_RowDataBound">
                        <Columns>
                           <asp:CommandField ShowSelectButton="True">
                                                <ItemStyle ForeColor="Blue" Height="21px" HorizontalAlign="Center" 
                                                    Width="5%" />
                                                </asp:CommandField>
                            <asp:BoundField DataField="ID" HeaderText="ID">
                                                <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                </asp:BoundField>
                           <%-- <asp:BoundField DataField="ID" HeaderText="ID" />--%>
                             <asp:BoundField  DataField="CourseID" HeaderText="Course ID">
                                                <ItemStyle HorizontalAlign="Center" Width="15%" />
                                                </asp:BoundField>
                         <%--   <asp:BoundField DataField="CourseID" HeaderText="Course ID" />--%>
                             <asp:BoundField DataField="CourseName" HeaderText="Course Name">
                                                <ItemStyle HorizontalAlign="Center" Width="25%" />
                                                </asp:BoundField>
                          <%--  <asp:BoundField DataField="CourseName" HeaderText="Course Name" />--%>
                             <asp:BoundField DataField="TracName" HeaderText="Trac Name">
                                                <ItemStyle HorizontalAlign="Center" Width="15%" />
                                                </asp:BoundField>
                         <%--   <asp:BoundField DataField="TracName" HeaderText="Trac Name" />--%>
                             <asp:BoundField DataField="Fees" HeaderText="Fee">
                                                <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                </asp:BoundField>
                          <%--  <asp:BoundField DataField="CourseFee" HeaderText="Fee" />--%>
                             <asp:BoundField DataField="Discount" HeaderText="Discount">
                                                <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                </asp:BoundField>
                            <%--asp:BoundField DataField="Doscount" HeaderText="Discount" />--%>
                       
                        </Columns>
                    </asp:GridView>
                        </td>
                        <td>&nbsp;</td>
                        <td style="width:15%">                         
                    
                            &nbsp;</tr>
                </table>
                </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
        </tr>
    </table>
    </div>
</asp:Content>

