<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="rptPaymentHistory.aspx.cs" Inherits="rptPaymentHistory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
<div id="frmMainDiv" style="width:99.5%; background-color:transparent; padding:3.5px;">
    <div id="Div1" style="background-color:White; width:100%;">
    <table style="width:100%">
        <tr>
            <td style="width:20%">
                &nbsp;</td>
            <td style="width:60%">
                <fieldset style="vertical-align: top; border: solid 1px #8BB381;">
                    <legend style="color: maroon;"><b>Report Generate</b></legend>
                    <asp:UpdatePanel ID="Up1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table style="width:100%">
                                <tr>
                                    <td align="right" style="width:10%">
                                        <asp:Label ID="Label1" runat="server" Text="Report Type"></asp:Label>
                                    </td>
                                    <td align="center" style="width:2%">
                                        &nbsp;</td>
                                    <td colspan="4">
                                        <asp:DropDownList ID="ddlReportType" runat="server" AutoPostBack="True" 
                                            Height="26px" onselectedindexchanged="ddlReportType_SelectedIndexChanged" 
                                            Width="98%">
                                            <asp:ListItem></asp:ListItem>
                                            <asp:ListItem Value="DWR">Date Wise All Report</asp:ListItem>
                                            <asp:ListItem Value="BWR">Batch Wise Report</asp:ListItem>
                                            <asp:ListItem Value="DWEL">Date Wise Existing List</asp:ListItem>
                                            <asp:ListItem Value="CR">Collection Report</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" colspan="6">
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td align="right" style="width:10%">
                                        <asp:Label ID="Label2" runat="server" Visible="False"></asp:Label>
                                    </td>
                                    <td align="center" style="width:2%">
                                        &nbsp;</td>
                                    <td style="width:20%">
                                        <asp:TextBox ID="txtStartDate" runat="server" CssClass="tbc" Enabled="true" 
                                            Font-Size="8pt" Height="20px" MaxLength="15" SkinID="tbPlain" Width="95%" 
                                            Visible="False"></asp:TextBox>
                                        <ajaxToolkit:CalendarExtender ID="Calendarextender3" runat="server" 
                                            Format="dd/MM/yyyy" TargetControlID="txtStartDate" />
                                        <asp:TextBox ID="txtSearchBatch" runat="server" Visible="False" Width="95%"></asp:TextBox>
                                    </td>
                                    <td align="right" style="width:10%">
                                        <asp:Label ID="Label3" runat="server" Text="End Date" Visible="False"></asp:Label>
                                    </td>
                                    <td align="center" style="width:2%">
                                        &nbsp;</td>
                                    <td style="width:20%">
                                        <asp:TextBox ID="txtEndDate" runat="server" CssClass="tbc" Enabled="true" 
                                            Font-Size="8pt" Height="20px" MaxLength="15" SkinID="tbPlain" Width="90%" 
                                            Visible="False"></asp:TextBox>
                                        <ajaxToolkit:CalendarExtender ID="Calendarextender8" runat="server" 
                                            Format="dd/MM/yyyy" TargetControlID="txtEndDate" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:10%">
                                        &nbsp;</td>
                                    <td style="width:2%">
                                        &nbsp;</td>
                                    <td style="width:20%">
                                        &nbsp;</td>
                                    <td style="width:10%">
                                        &nbsp;</td>
                                    <td style="width:2%">
                                        &nbsp;</td>
                                    <td style="width:20%">
                                        &nbsp;</td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </fieldset></td>
            <td style="width:20%">
                &nbsp;</td>
        </tr>
        <tr>
            <td style="width:20%">
                &nbsp;</td>
            <td style="width:60%">
                <table style="width:100%">
                    <tr>
                        <td style="width:10%">
                        </td>
                        <td style="width:10%">
                            <asp:Button ID="btnShowReport" runat="server" Height="35px" 
                                onclick="btnShowReport_Click" Text="Show" Width="120px" />
                        </td>
                        <td style="width:20%">
                            <asp:Button ID="btnRunReport" runat="server" Height="35px" 
                                onclick="btnRunReport_Click" Text="Run Report" Width="120px" />
                        </td>
                        <td style="width:20%">
                            <asp:Button ID="btnClear" runat="server" Height="35px" onclick="btnClear_Click" 
                                Text="Clear" Width="100px" />
                        </td>
                    </tr>
                </table>
            </td>
            <td style="width:20%">
                &nbsp;</td>
        </tr>
        <tr>
            <td style="width:20%">
                &nbsp;</td>
            <td style="width:60%">
                &nbsp;</td>
            <td style="width:20%">
                &nbsp;</td>
        </tr>
        <tr>
            <td colspan="3">
            <table style="width:100%">
            <tr>
            <td style="width:10%"></td>
            <td style="width:80%">
                <asp:GridView ID="dgPayHistory" runat="server" AllowSorting="True" 
                    AlternatingRowStyle-CssClass="alt" AutoGenerateColumns="False" 
                    BackColor="White" BorderColor="LightGray" BorderStyle="Solid" BorderWidth="1px" 
                    CellPadding="2" CssClass="mGrid" Font-Size="8pt" PagerStyle-CssClass="pgr" 
                    PageSize="400" Width="100%">
                    <HeaderStyle BackColor="" Font-Bold="True" Font-Size="8pt" 
                        HorizontalAlign="center" />
                    <FooterStyle BackColor="" Font-Bold="True" ForeColor="White" />
                    <AlternatingRowStyle CssClass="alt" />
                    <Columns>
                        <asp:BoundField DataField="pay_date" DataFormatString="{0:dd/MM/yyyy}" 
                            HeaderText="Payment Date" ItemStyle-HorizontalAlign="Center" 
                            ItemStyle-Width="60px">
                        <ItemStyle HorizontalAlign="Center" Width="40px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ID" HeaderText="MR. No." 
                            ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px">
                        <ItemStyle HorizontalAlign="Center" Width="20px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="student_id" HeaderText="Student ID" 
                            ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px">
                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="f_name" HeaderText="Name" 
                            ItemStyle-HorizontalAlign="Left" ItemStyle-Width="100px">
                        <ItemStyle HorizontalAlign="Left" Width="200px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="BatchNo" HeaderText="Batch" 
                            ItemStyle-HorizontalAlign="center" ItemStyle-Width="100px">
                        <ItemStyle HorizontalAlign="center" Width="50px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="PaidAmount" DataFormatString="{0:N2}" 
                            HeaderText="Pay Amount" ItemStyle-HorizontalAlign="Right" 
                            ItemStyle-Width="60px">
                        <ItemStyle HorizontalAlign="Right" Width="50px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Waiver" HeaderText="Waiver" 
                            ItemStyle-HorizontalAlign="Right" ItemStyle-Width="40px">
<ItemStyle HorizontalAlign="Right" Width="40px"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="Discount" DataFormatString="{0:N2}" 
                            HeaderText="Discount ">
                        <ItemStyle HorizontalAlign="Right" Width="40px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="PayAmount" DataFormatString="{0:N2}" 
                            HeaderText="Paid Amount" ItemStyle-HorizontalAlign="Right" 
                            ItemStyle-Width="60px">
                        <ItemStyle HorizontalAlign="Right" Width="60px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="DueAmount" HeaderText="Due" ItemStyle-HorizontalAlign="Right" 
                            ItemStyle-Width="60px">
                         <ItemStyle HorizontalAlign="Right" Width="60px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="SheduleStart" HeaderText="Start Date" ItemStyle-Width="60px">
                         <ItemStyle HorizontalAlign="Right" Width="60px" />
                          </asp:BoundField>
                        <asp:BoundField DataField="ShehuleEnd" HeaderText="End Date" ItemStyle-Width="60px">
                        <ItemStyle HorizontalAlign="Right" Width="60px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ClassTime" HeaderText="Time" ItemStyle-Width="60px">
                        <ItemStyle HorizontalAlign="Right" Width="60px" />
                        </asp:BoundField>
                    </Columns>
                    <RowStyle BackColor="" />
                    <EditRowStyle BackColor="" />
                    <PagerStyle BackColor="" ForeColor="" HorizontalAlign="Center" />
                    <HeaderStyle BackColor="" Font-Bold="True" ForeColor="Black" />
                </asp:GridView>
                </td>
            <td style="width:10%"></td>
            </tr>
            <tr>
            <td style="width:10%">&nbsp;</td>
            <td style="width:80%">&nbsp;</td>
            <td style="width:10%">&nbsp;</td>
            </tr>
            </table>
            </td>
        </tr>
    </table>
    </div>
</asp:Content>

