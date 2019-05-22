<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="frmCourseAndSheduleAssiagn.aspx.cs" Inherits="frmCourseAndSheduleAssiagn" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">

<table style="width:100%">
<tr>
<td style="width:10%"></td>
<td style="width:80%">
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<%--<head id="Head1" runat="server">
    <link href="css/black/PopUp.css" rel="stylesheet" type="text/css" />
    <title></title>
    
    <script src="Scripts/date.js" type="text/javascript"></script>
    <script src="Scripts/Timeout.js" type="text/javascript"></script>
    <script src="Scripts/valideDate.js" type="text/javascript"></script>
    <style>
        input[type=text], input[type=password], select {
            background: #ffffff url("../../images/bg_ip.png") repeat-x;
            padding: 3px;
            font-size: 10px;
            color: #000000;
            font-weight: bold;
            margin: 0;
            border: 1px solid #c0c0c0;
            }

        input[type=submit], input[type=button] {
            /*background-color:transparent;	*/
            background: #999999;
            color: Black;
            -webkit-border-radius: 4px;
            -moz-border-radius: 4px;
            border-radius: 4px;
            border: solid 1px #20538D;
            text-shadow: 0 -1px 0 rgba(0, 0, 0, 0.4);
            -webkit-box-shadow: inset 0 1px 0 rgba(255, 255, 255, 0.4), 0 1px 1px rgba(0,0, 0, 0.2);
            -moz-box-shadow: inset 0 1px 0 rgba(255, 255, 255, 0.4), 0 1px 1px rgba(0, 0, 0, 0.2);
            box-shadow: inset 0 1px 0 rgba(255, 255, 255, 0.4), 0 1px 1px rgba(0, 0, 0, 0.2);
            -webkit-transition-duration: 0.2s;
            -moz-transition-duration: 0.2s;
            transition-duration: 0.2s;
            -webkit-user-select: none;
            -moz-user-select: none;
            -ms-user-select: none;
            user-select: none;
            font-weight: bold;
        }
        .style1
        {
            width: 7%;
        }
        .style2
        {
            width: 5%;
        }
        </style>
     <script type = "text/javascript">
         function OnClose() {

             if (window.opener != null && !window.opener.closed) {
                 window.opener.HideModalDiv();
             }
         }
         function OnOpen() {
             window.opener.LoadModalDiv();
         }
         window.onbeforeunload = OnClose;
         window.onload = OnOpen;     

</script>  
</head>--%>
<body>
    <%-- <form id="form1" runat="server">--%>
        <%--<asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="1000">
        </asp:ScriptManager>--%>
         <div style="height: 20px">
          
        </div>
        <div>
          <fieldset style="vertical-align: top; border: solid 1px #8BB381;text-align:left;line-height:1.5em;"><legend style="color: maroon;">
              <strong>Faculty &amp; Course</strong><b> Set</b></legend>
            <table style="width:100%">
            <tr>
            <td style="width:10%"></td>
            <td style="width:80%">
            <table style="width:100%">
            <tr>
            <td style="width:15%" align="right">
                <asp:Label ID="Label2" runat="server" Text="Course Trac"></asp:Label>
                </td>
            <td style="width:25%">
                <asp:DropDownList ID="ddlCourseTrac" runat="server" Height="26px" Width="104%" 
                    AutoPostBack="True" 
                    onselectedindexchanged="ddlCourseTrac_SelectedIndexChanged">
                </asp:DropDownList>
                </td>
            <td style="width:15%" align="right">
                <asp:Label ID="Label3" runat="server" Text="Course Name"></asp:Label>
                </td>
            <td style="width:25%">
                <asp:DropDownList ID="ddlCourseName" runat="server" Height="26px" Width="104%" 
                    onselectedindexchanged="ddlCourseName_SelectedIndexChanged">
                </asp:DropDownList>
                </td>
            
            </tr>
            <tr>
            <td style="width:15%" align="right">
                <asp:Label ID="Label4" runat="server" Text="Faculty Name"></asp:Label>
                </td>
            <td style="width:25%">
                <asp:TextBox ID="txtFAcultySearch" runat="server" Width="100%" Height="18px" 
                    AutoPostBack="True" ontextchanged="txtFAcultySearch_TextChanged"></asp:TextBox>
                <ajaxtoolkit:AutoCompleteExtender ID="AutoCompleteExtender1"
                                                        runat="server" CompletionInterval="20" CompletionSetCount="30"
                                                        EnableCaching="true" MinimumPrefixLength="2"
                                                        ServiceMethod="GetExeistCourse" ServicePath="~/AutoComplete.asmx"
                                                        TargetControlID="txtFAcultySearch">
                                                    </ajaxtoolkit:AutoCompleteExtender>
                </td>
            <td style="width:15%" align="right">
                <asp:Label ID="Label5" runat="server" Text="Batch No"></asp:Label>
                </td>
            <td style="width:25%">
                <asp:TextBox ID="txtBatchNo" runat="server" Width="100%"></asp:TextBox>
                </td>
            
            </tr>
            <tr>
            <td style="width:15%" align="right">
                <asp:Label ID="Label6" runat="server" Text="Start Date"></asp:Label>
                </td>
            <td style="width:25%">
                  <asp:TextBox ID="txtStartDate" runat="server" CssClass="tbc" Font-Size="8" 
                                MaxLength="13" SkinID="tbPlain" TabIndex="1" Width="100%"></asp:TextBox>
                            <ajaxToolkit:CalendarExtender ID="Calendarextender2" runat="server" 
                                Format="MM/dd/yyyy" TargetControlID="txtStartDate" /></td>
            <td style="width:15%" align="right">
                <asp:Label ID="Label7" runat="server" Text="End Date"></asp:Label>
                </td>
            <td style="width:25%">
                  <asp:TextBox ID="txtEndDate" runat="server" CssClass="tbc" Font-Size="8" 
                                MaxLength="13" SkinID="tbPlain" TabIndex="1" Width="100%"></asp:TextBox>
                            <ajaxToolkit:CalendarExtender ID="Calendarextender1" runat="server" 
                                Format="MM/dd/yyyy" TargetControlID="txtEndDate" /></td>
            
            </tr>
            <tr>
            <td style="width:15%" align="right">
                <asp:Label ID="Label8" runat="server" Text="Year"></asp:Label>
                </td>
            <td style="width:25%">
                <asp:TextBox ID="txtYear" runat="server" Width="100%"></asp:TextBox>
                </td>
            <td style="width:15%" align="right">
                <asp:Label ID="Label9" runat="server" Text="Status"></asp:Label>
                </td>
            <td style="width:25%">
        <asp:DropDownList SkinID="ddlPlain" ID="ddlStatus" runat="server" Font-Size="8pt" 
            Width="104%" Height="26px" AutoPostBack="True"  TabIndex="8">
     <asp:ListItem Value="1">Active</asp:ListItem> 
     <asp:ListItem Value="2">Archive</asp:ListItem>
     <asp:ListItem Value="3">T.C</asp:ListItem>
    
     </asp:DropDownList>
                </td>
            
            </tr>
            <tr>
            <td style="width:15%" align="right">
                &nbsp;</td>
            <td style="width:25%">
                <asp:Label ID="lblFacultyID" runat="server" Visible="False"></asp:Label>
                <asp:Label ID="lblFlag" runat="server"></asp:Label>
                <asp:Label ID="lblID" runat="server" Visible="False"></asp:Label>
                </td>
            <td style="width:15%" align="right">
                &nbsp;</td>
            <td style="width:25%">
                &nbsp;</td>
            
            </tr>
            </table>
            </td>
            <td style="width:10%"></td>
            </tr>
            </table>
          </fieldset>
        </div>  
        <div style="text-align:center;">  
        <asp:UpdatePanel ID="UP1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>           
          <fieldset style="vertical-align: top; border: solid 1px #8BB381;text-align:left;line-height:1.5em;"><legend style="color: maroon;">
              <strong>Course</strong><b> Schedule</b></legend>
            <asp:GridView ID="dgSubSedule" CssClass="mGrid" runat="server" AutoGenerateColumns="False" style="text-align:center;"
                Width="100%" onrowdatabound="dgSubSedule_RowDataBound">
                <Columns>
                    <asp:TemplateField HeaderText="Days" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15%">
                         <ItemTemplate>
                            <asp:DropDownList SkinId="ddlPlain"  ID="ddlDays" runat="server" Width="95%"  Height="26px"
                                 Font-Size="8pt"  SelectedValue='<%# Eval("DaysID") %>'
                            AutoPostBack="true" DataSource='<%#PopulateItem()%>' DataTextField="Days_name" 
                                 DataValueField="ID" onselectedindexchanged="ddlDays_SelectedIndexChanged">
                         </asp:DropDownList>
                         </ItemTemplate> 
                            <ItemStyle Width="15%" HorizontalAlign="Center"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Start Time" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%">
                        <ItemTemplate>
                            <asp:TextBox ID="txtStartTime" style="text-align:center;" placeholder="hh:mm" 
                                runat="server" Width="80%" onblur="formatTime(txtStartTime)" 
                                Text='<%# Eval("StartTime") %>'></asp:TextBox>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="" ItemStyle-Width="10%">
                    <ItemTemplate>
                        <asp:RadioButtonList ID="rbStartAmPm" runat="server" 
                            RepeatDirection="Horizontal" AutoPostBack="True" 
                            onselectedindexchanged="rbStartAmPm_SelectedIndexChanged" 
                            SelectedValue='<%# Bind("StartAmPm") %>'>
                                         <asp:ListItem>AM</asp:ListItem>
                                         <asp:ListItem>PM</asp:ListItem>
                                         <asp:ListItem Text="" Value="" style="display:none"></asp:ListItem>
                                     </asp:RadioButtonList>
                    </ItemTemplate>
                  
<ItemStyle Width="10%"></ItemStyle>
                  
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="End Time " ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%">
                          <ItemTemplate>
                          <asp:TextBox ID="txtEndTime" runat="server" style="text-align:center;" 
                                  placeholder="hh:mm" Width="80%"  Text='<%# Eval("EndtTime") %>'></asp:TextBox>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="10%">
                        <ItemTemplate>
                        <asp:RadioButtonList ID="rbEndAmPm" runat="server" RepeatDirection="Horizontal" onselectedindexchanged="rbEndAmPm_SelectedIndexChanged" 
                                AutoPostBack="True" SelectedValue='<%# Bind("EndAmPm") %>'>
                                         <asp:ListItem>AM</asp:ListItem>
                                         <asp:ListItem>PM</asp:ListItem>
                                         <asp:ListItem Text="" Value="" style="display:none"></asp:ListItem>
                                     </asp:RadioButtonList>
                    </ItemTemplate>

<ItemStyle Width="10%"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Room No" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15%">
                    
                    <ItemTemplate>
                        <asp:TextBox ID="txtRoomNo" runat="server" style="text-align:center;" 
                            Width="80%" Text='<%# Eval("RoomNo") %>' 
                            ontextchanged="txtRoomNo_TextChanged" AutoPostBack="True"></asp:TextBox>
                    </ItemTemplate>
                      <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
           
        </fieldset>
        </ContentTemplate></asp:UpdatePanel>
        </div>
        <div>
            
        </div>
         <div>
          
             <table style="width:100%;">
                 <tr>
                     <td style="width:16%;">
                         &nbsp;</td>
                     <td style="width:16%;">
                         &nbsp;</td>
                     <td style="width:14%;" align="center">
                         <asp:Button ID="btnSave" runat="server" Height="30px" 
                             Text="Save Shedule" Width="90%" onclick="btnSave_Click" />
                     </td>
                     <td style="width:14%;" align="center">
                         <asp:Button ID="btnClear" runat="server" Height="30px" onclick="btnClear_Click" 
                             Text="Clear" Width="90%" />
                     </td>
                     <td style="width:16%;">
                         &nbsp;</td>
                     <td style="width:16%;">
                         <asp:HiddenField ID="hfID" runat="server" />
                     </td>
                 </tr>
             </table>
          
        </div>
   <%-- </form>--%>
</body>
</html>
</td>
<td style="width:10%"></td>
</tr>
<tr>
<td style="width:10%">&nbsp;</td>
<td style="width:80%">
    <asp:GridView runat="server" AllowPaging="True" AllowSorting="True" 
        AutoGenerateColumns="False" CellPadding="2" PageSize="15" BackColor="White" 
        BorderColor="LightGray" BorderWidth="1px" BorderStyle="Solid" CssClass="mGrid" 
        Font-Size="8pt" ForeColor="#333333" Width="100%" ID="HistoryGridView" 
        OnPageIndexChanging="HistoryGridView_PageIndexChanging" 
        OnSelectedIndexChanged="HistoryGridView_SelectedIndexChanged" 
        onrowdatabound="HistoryGridView_RowDataBound">
        <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
        <Columns>
            <asp:CommandField ShowSelectButton="True">
            <ItemStyle HorizontalAlign="Center" ForeColor="Blue" Height="25px" Width="40px">
            </ItemStyle>
            </asp:CommandField>
            <asp:BoundField DataField="CourseName" HeaderText="Course Name">
            <ItemStyle HorizontalAlign="Left" Width="150px"></ItemStyle>
            </asp:BoundField>
            <asp:BoundField DataField="TrainerName" HeaderText="Trainer Name">
            </asp:BoundField>
            <asp:BoundField DataField="BatchNo" HeaderText="Batch No"></asp:BoundField>
            <asp:BoundField DataField="stadte" HeaderText="Shedule Start Date">
            </asp:BoundField>
            <asp:BoundField DataField="enddate" HeaderText="End Date"></asp:BoundField>
            <asp:BoundField DataField="CourseID" HeaderText="Course ID">
            <ItemStyle HorizontalAlign="Center" Width="150px" />
            </asp:BoundField>
            <asp:BoundField DataField="ID" HeaderText="RealID">
            <ItemStyle HorizontalAlign="Center" Width="150px" />
            </asp:BoundField>
        </Columns>
        <HeaderStyle BackColor="Silver" Font-Bold="True" ForeColor="White">
        </HeaderStyle>
        <PagerStyle HorizontalAlign="Center" CssClass="pgr"></PagerStyle>
        <RowStyle BackColor="White"></RowStyle>
    </asp:GridView>
    </td>
<td style="width:10%">&nbsp;</td>
</tr>
</table>
</asp:Content>

