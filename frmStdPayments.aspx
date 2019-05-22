<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="frmStdPayments.aspx.cs" Inherits="frmStdPayments" Title="Students Payment" Theme="Themes"%>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
<script language="javascript" type="text/javascript" >
   function isNumber(evt) {
        evt = (evt) ? evt : window.event;
        var charCode = (evt.which) ? evt.which : evt.keyCode;
        if (charCode != 46 && charCode > 31
            && (charCode < 48 || charCode > 57)) {
            return false;
        }
        return true;
    } 
</script>
 <style type="text/css">    
.TextNewColor 
{
    width: 100%;
    text-indent:15px;   
    display: inline-block;
    border: 1px solid #ccc;
    border-radius: 4px;
    box-sizing: border-box;
    background:transparent !important;
}   
        .style4
        {
            width: 1%;
            height: 1px;
        }
        .style5
        {
            width: 98%;
            height: 1px;
        }
        .style6
        {
            width: 65%;
        }
    </style>
<div id="frmMainDiv" style="background-color:White; width:100%;">
<table  id="pageFooterWrapper">
 <tr>
	<td style="vertical-align:middle; height:100%;" align="center">
        <asp:Button  ID="btnDelete" runat="server"  ToolTip="Delete Record"   
            OnClick="btnDelete_Click"  TabIndex="903" 
            onclientclick="javascript:return window.confirm('are u really want to delete these data')"             
            Text="Delete"
             Width="120px" Height="35px" /></td>
	<td style="vertical-align:middle;" align="center">
	<asp:Button  ID="btnFind" runat="server"  ToolTip="Find"  
            OnClick="btnFind_Click"  TabIndex="902" 
            Text="Find"
             Width="120px" Height="35px" />
	</td>
	<td style="vertical-align:middle;" align="center"><asp:Button ID="btnSave" 
            runat="server" ToolTip="Save or Update Record" 
            OnClick="btnSave_Click" TabIndex="901" Text="Save"              
             Width="120px" Height="35px" /></td>
	<td style="vertical-align:middle;" align="center">
        <asp:Button ID="btnClear" runat="server" ToolTip="Clear Form" 
            OnClick="btnClear_Click"  TabIndex="904"   
            Text="Clear" 
             Width="120px" Height="35px" /></td>
    <td style="vertical-align:middle;" align="center">
        <asp:Button ID="btnPrint" runat="server" ToolTip="Clear Form" 
            OnClick="btnPrint_Click"  TabIndex="904"  Text="Print" Width="120px" 
            Height="35px" />
        </td>

         <td style="vertical-align:middle;" align="center">
        <asp:Button ID="btnPPos" runat="server" ToolTip="Clear Form" Text="P.Pos" 
                 Width="100px" onclick="btnPPos_Click" Visible="False" />
        </td>
	</tr>		
	</table>

<table style="width:100%; color:Black;" >
<tr>
<td style="width:1%;">&nbsp;</td>
<td style="width:98%;" align="center">
    &nbsp;</td>
<td style="width:1%;">&nbsp;</td>
</tr>
<tr>
<td style="width:1%;">&nbsp;</td>
<td style="width:98%;" align="center" valign="middle">
      <asp:Label ID="Label3" runat="server" BackColor="#CCCCCC" Font-Bold="True" 
          Font-Size="Medium" ForeColor="#CC3300" Text="Student Payment Information" 
          Width="95%" Height="25px" BorderStyle="Inset"></asp:Label>
    </td>
<td style="width:1%;">&nbsp;</td>
</tr>
<tr>
<td style="width:1%;">&nbsp;</td>
<td style="width:98%;" align="left">
    &nbsp;</td>
<td style="width:1%;">&nbsp;</td>
</tr>
<tr>
<td style="width:1%;">&nbsp;</td>
<td style="width:98%;" align="center">
 <table style="width: 100%">
        <tr>
            <td class="style6">
            <asp:UpdatePanel ID="UpdatePanelMST" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                 <fieldset style=" text-align:left; vertical-align: top; border: solid 1px #8BB381;"><legend style="color: maroon;"><b>Payment Information</b></legend>
                 <table style="width: 100%">
                    <tr>
                        <td style="width: 15%">
                            <asp:Label ID="Label7" runat="server" ForeColor="#FF3300" Text="Search Student"></asp:Label>
                        </td>
                        <td align="left" colspan="4">
                            <asp:TextBox ID="txtSearchStudent" runat="server" CssClass="TextNewColor" 
                                AutoPostBack="True" placeholder="Search ** Student ID & Name - Roll No. - Class - Section - Phone Number"
                                Font-Size="8pt" MaxLength="20"  SkinID="tbPlain" Width="99%" 
                                ontextchanged="txtSearchStudent_TextChanged" Height="25px"></asp:TextBox>
                                <ajaxtoolkit:AutoCompleteExtender ID="txtSearchStudent_AutoCompleteExtender"
                                                        runat="server" CompletionInterval="20" CompletionSetCount="30"
                                                        EnableCaching="true" MinimumPrefixLength="2"
                                                        ServiceMethod="GetCompletionStudentId" ServicePath="~/AutoComplete.asmx"
                                                        TargetControlID="txtSearchStudent">
                                                    </ajaxtoolkit:AutoCompleteExtender>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 15%">
                            <asp:Label ID="lblSubjectId" runat="server" Font-Size="8pt">Student ID</asp:Label>
                        </td>
                        <td style="width: 30%">
                            <asp:TextBox ID="txtStudentId" runat="server" AutoPostBack="true" 
                                CssClass="tbc" Enabled="true" Font-Size="8pt" MaxLength="15" 
                                ontextchanged="txtStudentId_TextChanged" SkinID="tbPlain" Width="92%"></asp:TextBox>
                        </td>
                        <td style="width: 3%" align="left">
                            <asp:Label ID="lblSubjectId1" runat="server" Font-Bold="True" Font-Size="8pt" 
                                ForeColor="#FF3300">*</asp:Label>
                        </td>
                        <td style="width: 15%">
                            Transaction No</td>
                        <td style="width: 25%">
                            <asp:TextBox ID="txtPaymentId" runat="server" CssClass="tbc" Font-Size="8" 
                                MaxLength="6" SkinID="tbPlain" Width="95%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 15%">
                            Date</td>
                        <td style="width: 30%">
                            <asp:TextBox ID="txtPayDate" runat="server" CssClass="tbc" Font-Size="8" 
                                MaxLength="13" SkinID="tbPlain" TabIndex="1" Width="92%"></asp:TextBox>
                            <ajaxToolkit:CalendarExtender ID="Calendarextender2" runat="server" 
                                Format="dd/MM/yyyy" TargetControlID="txtPayDate" />
                        </td>
                        <td style="width: 3%" align="center">
                            &nbsp;</td>
                        <td style="width: 15%">
                            Waiver Pct(%)</td>
                        <td style="width: 25%">
                       <asp:TextBox SkinID="tbPlain" ID="txtWaiverPct" runat="server"
                                CssClass="TextNewColor"  Font-Size="8" MaxLength="15" Enabled="False" ></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 15%"> Payment Mode</td>
                        <td style="width: 30%">
                        <asp:DropDownList SkinID="ddlPlain" ID="ddlPayMode" runat="server" Font-Size="8pt" 
        Width="95%" Height="18px" AutoPostBack="True" onselectedindexchanged="ddlPayMode_SelectedIndexChanged"  >
     <asp:ListItem></asp:ListItem>       
     <asp:ListItem Text="Cash" Value="C"></asp:ListItem> 
     <asp:ListItem Text="Cheque" Value="Q"></asp:ListItem>
</asp:DropDownList>
                        </td>
                        <td style="width: 3%" align="center">
                            <asp:LinkButton ID="LinkButton1" runat="server" onclick="LinkButton1_Click">...</asp:LinkButton>
                        </td>
                        <td style="width: 15%">
    <asp:Label ID="lblStudentName1" runat="server" Font-Size="8pt">Book No.</asp:Label>
                        </td>
                        <td style="width: 25%">
    <asp:TextBox ID="txtRefNo" runat="server" CssClass="tbc" Font-Size="8" 
        MaxLength="10" SkinID="tbPlain" Width="95%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 15%">
    <asp:Label ID="lblCheckNo" runat="server" Font-Size="8pt" Visible="False">Cheque No</asp:Label>
                        </td>
                        <td style="width: 30%">
                            <asp:TextBox SkinID="tbPlain" 
        ID="txtChequeNo" runat="server"  Width="92%" Font-Size="8" CssClass="tbc" 
        MaxLength="13" Visible="False" ></asp:TextBox>
                        </td>
                        <td style="width: 3%" align="center">
    <asp:Label ID="lblChkPoint" runat="server" Font-Size="8pt" Font-Bold="True" ForeColor="#FF3300" 
                                Visible="False">*</asp:Label>
                        </td>
                        <td style="width: 15%">
    <asp:Label ID="lblCheckDate" runat="server" Font-Size="8pt" Visible="False">Cheque Date</asp:Label>
                        </td>
                        <td style="width: 25%">
<asp:TextBox SkinID="tbPlain" ID="txtChequeDate" runat="server"  Width="95%" 
        Font-Size="8" CssClass="tbc" 
        MaxLength="6" Visible="False" ></asp:TextBox>
<ajaxtoolkit:calendarextender runat="server" ID="Calendarextender1" 
        TargetControlID="txtChequeDate" Format="dd/MM/yyyy"/>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 15%"><asp:Label ID="lblBankName" runat="server" 
                                Font-Size="8pt" Visible="False">Bank Name</asp:Label>
                            </td>
                        <td style="width: 30%">
<asp:DropDownList SkinID="ddlPlain" ID="ddlBankNo" runat="server" Font-Size="8pt" 
        Width="95%" Height="18px" Visible="False" AutoPostBack="True" 
                                onselectedindexchanged="ddlBankNo_SelectedIndexChanged"  >
</asp:DropDownList>
                        </td>
                        <td style="width: 3%" align="center">
    <asp:Label ID="lblBankPoint" runat="server" Font-Size="8pt" Font-Bold="True" ForeColor="#FF3300" 
                                Visible="False">*</asp:Label>
                        </td>
                        <td style="width: 15%">
                            <asp:Label ID="lblChkAmount" runat="server" 
                                Font-Size="8pt" Visible="False">Cheque Amount</asp:Label>
                           </td>
                        <td style="width: 25%">
<asp:TextBox SkinID="tbPlain" ID="txtChequeAmt" runat="server"  Width="95%" 
        Font-Size="8" CssClass="tbc" 
        MaxLength="15" Visible="False" ></asp:TextBox>
                        </td>
                    </tr>
                    </table>
                </fieldset>
                </ContentTemplate></asp:UpdatePanel>
            </td>
            <td style="width:40%" valign="top">
            <fieldset style=" text-align:left; vertical-align: top; border: solid 1px #8BB381;"><legend style="color: maroon;"><b>Student Information</b></legend>
            <asp:UpdatePanel ID="UpdatePanelDetails" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                <table style="width: 100%; height: 120px;">
                    <tr>
                        <td style="width: 15%">
                            <asp:Label ID="lblName0" runat="server" Font-Bold="True" Font-Size="8pt" 
                                Width="100%">Name</asp:Label>
                        </td>
                        <td align="left" colspan="3" style="height: 26px">
                            <asp:Label ID="lblName" runat="server" Font-Bold="True" Font-Size="8pt"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 15%">
                            <asp:Label ID="lblClass0" runat="server" Font-Bold="True" Font-Size="8pt" 
                                Width="100%">Batch No</asp:Label>
                        </td>
                        <td style="width: 30%">
                            <asp:Label ID="lblClass" runat="server" Font-Bold="True" Font-Size="8pt" 
                                Width="100%"></asp:Label>
                        </td>
                        <td style="width: 15%">
                            <asp:Label ID="lblClass1" runat="server" Font-Bold="True" Font-Size="8pt" 
                                Width="100%">Section</asp:Label>
                        </td>
                        <td style="width: 30%">
                            <asp:Label ID="lblSection" runat="server" Font-Bold="True" Font-Size="8pt" 
                                Width="100%"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 15%">
                            <asp:Label ID="lblClass2" runat="server" Font-Bold="True" Font-Size="8pt" 
                                Width="100%">Roll</asp:Label>
                        </td>
                        <td colspan="3">
                            <asp:Label ID="lblRollName" runat="server" Font-Bold="True" Font-Size="8pt" 
                                Width="100%"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 15%">
                            <asp:Label ID="lblClassId" runat="server" Visible="False" style="display:none;"></asp:Label>
                            <asp:Label ID="lblVersonId" runat="server" Visible="False" style="display:none;"></asp:Label>
                            <asp:Label ID="lblSectionId" runat="server" Visible="False" style="display:none;"></asp:Label>
                            <asp:Label ID="lblRoll" runat="server" Visible="False" style="display:none;"></asp:Label>
                            <asp:Label ID="lblShiftId" runat="server" Visible="False" style="display:none;"></asp:Label>
                            <asp:Label ID="lblClass7" runat="server" Font-Bold="True" Font-Size="8pt" 
                                Width="100%">Shift</asp:Label>
                        </td>
                        <td style="width: 30%">
                            <asp:Label ID="lblShift" runat="server" Font-Bold="True" Font-Size="8pt"></asp:Label>
                            <asp:Label ID="lblShiftNew" runat="server" Visible="False"></asp:Label>
                        </td>
                        <td style="width: 15%">
                            <asp:Label ID="lblClass6" runat="server" Font-Bold="True" Font-Size="8pt" 
                                Width="100%">Year</asp:Label>
                        </td>
                        <td style="width: 30%">
                            <asp:Label ID="lblYear" runat="server" Font-Bold="True" Font-Size="8pt" 
                                Visible="False"></asp:Label>
                            <asp:Label ID="lblYear0" runat="server" Font-Bold="True"></asp:Label>
                        </td>
                    </tr>
                </table>
                </ContentTemplate></asp:UpdatePanel>
                </fieldset>
            </td>
        </tr>
        </table>
    
</td>
<td style="width:1%;">&nbsp;</td>
</tr>
<tr>
<td class="style4"></td>
<td align="center" class="style5">
    <asp:Label ID="lblPaymentYear" runat="server" Visible="False"></asp:Label>
    </td>
<td class="style4"></td>
</tr>
<tr>
<td style="width:1%;">&nbsp;</td>
<td style="width:98%;" align="center">
<asp:GridView CssClass="mGrid" PagerStyle-CssClass="pgr" 
        AlternatingRowStyle-CssClass="alt"  ID="dgStd" runat="server" 
        AutoGenerateColumns="False" Visible="False" 
        AllowPaging="True" Width="100%" BackColor="White" BorderWidth="1px" BorderStyle="Solid"
        CellPadding="2" BorderColor="LightGray" Font-Size="8pt" 
        AllowSorting="True" PageSize="15" ForeColor="#333333"  
        onselectedindexchanged="dgStd_SelectedIndexChanged" 
        onpageindexchanging="dgStd_PageIndexChanging">
  <Columns>
  <asp:CommandField ShowSelectButton="True"  ItemStyle-Width="40px" 
          ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="Blue" 
          ItemStyle-Height="25px">
<ItemStyle HorizontalAlign="Center" ForeColor="Blue" Height="25px" Width="40px"></ItemStyle>
      </asp:CommandField>
  <asp:BoundField  HeaderText="Payment ID" DataField="payment_id" 
          ItemStyle-Width="90px" ItemStyle-HorizontalAlign="Center">
<ItemStyle HorizontalAlign="Center" Width="90px"></ItemStyle>
      </asp:BoundField>
  <asp:BoundField  HeaderText="Date" DataField="pay_date" ItemStyle-Width="90px" 
          ItemStyle-HorizontalAlign="Center">
<ItemStyle HorizontalAlign="Center" Width="90px"></ItemStyle>
      </asp:BoundField>
  <asp:BoundField  HeaderText="Class" DataField="class_id" ItemStyle-Width="200px" 
          ItemStyle-HorizontalAlign="Left">
<ItemStyle HorizontalAlign="Left" Width="90px"></ItemStyle>
      </asp:BoundField>
  <asp:BoundField  HeaderText="Study Year" DataField="pay_year" 
          ItemStyle-Width="90px" ItemStyle-HorizontalAlign="Center">
<ItemStyle HorizontalAlign="Center" Width="90px"></ItemStyle>
      </asp:BoundField>
  <asp:BoundField  HeaderText="Payment Mode" DataField="pay_mode" 
          ItemStyle-Width="90px" ItemStyle-HorizontalAlign="Center">
<ItemStyle HorizontalAlign="Center" Width="90px"></ItemStyle>
      </asp:BoundField>
      <asp:BoundField DataField="total_amount" HeaderText="Paid Amount" 
      ItemStyle-Width="90px" ItemStyle-HorizontalAlign="Center">
<ItemStyle HorizontalAlign="Center" Width="90px"></ItemStyle>
      </asp:BoundField>
  </Columns>
                        <RowStyle BackColor="white" />
                        <EditRowStyle BackColor="" />
                        <PagerStyle BackColor="" HorizontalAlign="Center" />
                        <HeaderStyle BackColor="Silver" Font-Bold="True" ForeColor="White" />
                        <AlternatingRowStyle BackColor="" />
  </asp:GridView>
<asp:Panel ID="pnlPay" runat="server" Width="100%">  
<table style="width:100%;">
<tr>
<td style="width:57%; vertical-align:top;" align="left">
<asp:UpdatePanel ID="upPayment" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView CssClass="mGrid" PagerStyle-CssClass="pgr"  
        AlternatingRowStyle-CssClass="alt"  ID="dgPayType" runat="server" AutoGenerateColumns="False" 
        Width="100%" BackColor="White" BorderWidth="1px" BorderStyle="Solid"
        CellPadding="2" BorderColor="LightGray" Font-Size="8pt" 
        ForeColor="#333333" OnRowDataBound="dgPayType_RowDataBound" PageSize="30">
  <Columns>
  <asp:BoundField  HeaderText="Payment ID" DataField="pay_id" ItemStyle-Width="90px" 
          ItemStyle-HorizontalAlign="Center">
      <ItemStyle HorizontalAlign="Center" Width="90px" />
      </asp:BoundField>
  <asp:BoundField  HeaderText="Payment Type" DataField="pay_head_id" 
          ItemStyle-Width="390px" ItemStyle-HorizontalAlign="Left" 
          ItemStyle-Height="20px">
      <ItemStyle Height="20px" HorizontalAlign="Left" Width="390px" />
      </asp:BoundField>
  <asp:BoundField  HeaderText="Amount" DataField="pay_amt" ItemStyle-Width="120px" 
          ItemStyle-HorizontalAlign="Right">
      <ItemStyle HorizontalAlign="Right" Width="120px" />
      </asp:BoundField>
  <asp:BoundField  HeaderText="Paid Amount" DataField="paid_amt" 
          ItemStyle-Width="120px" ItemStyle-HorizontalAlign="Right">
      <ItemStyle HorizontalAlign="Right" Width="120px" />
      </asp:BoundField>
  <asp:BoundField  HeaderText="Discount" DataField="Discount_Amt" 
          ItemStyle-Width="120px" ItemStyle-HorizontalAlign="Right">
      <ItemStyle HorizontalAlign="Right" Width="120px" />
      </asp:BoundField>
      <asp:BoundField  HeaderText="Due" DataField="due_amt" ItemStyle-Width="120px" 
          ItemStyle-HorizontalAlign="Right">
      <ItemStyle HorizontalAlign="Right" Width="120px" />
      </asp:BoundField>
  </Columns>
                        <RowStyle BackColor="white" />
                        <EditRowStyle BackColor="" />
                        <PagerStyle BackColor="" HorizontalAlign="Center" />
                        <HeaderStyle BackColor="Silver" Font-Bold="True" ForeColor="White" />
                        <AlternatingRowStyle BackColor="" />
</asp:GridView>
<table style="width:100%;">
<tr>
<td align="right" style="width: 160px; height: 20px;" ><b> Total Amount to Pay : </b></td>
<td style="width:139px; height: 20px;" align="right">
    <asp:Label ID="lblGrandTotal" runat="server" Font-Bold="True" Width="100%">0</asp:Label>
    </td>
<td rowspan="4" style="width: 4px"><img src="img/box_bottom_hori.gif" width="2px" style="height:75px;" alt="" /></td>
<td  align="right" style="width: 154px; height: 20px;"> <b> Last Payment Amount : </b></td>
<td align="right" style="height: 20px">
<asp:Label ID="lblTotalAmount" runat="server" Font-Bold="True" Width="100%">0</asp:Label>
</td>
</tr>
    <tr>
        <td align="right" style="width: 160px; height: 22px;"><b> Previous Discount: </b></td>
        <td align="right" style="width: 139px; height: 22px;" >
            <asp:Label ID="lblPreviousDiscount" runat="server" Font-Bold="True" 
                Width="100%">0</asp:Label>
        </td>
        <td align="right" style="width: 154px; height: 22px;" ><b> Current Discount : </b></td>
        <td align="right" style="height: 22px" >
            <asp:Label ID="lblCurrentDiscount" runat="server" Font-Bold="True" Width="100%">0</asp:Label>
        </td>
    </tr>
<tr>
<td  align="right" style="width: 160px; height: 22px;" > <b> Previous Payment : </b></td>
<td  align="right" style="width: 139px; height: 22px;">
    <asp:Label ID="lblPreviousAmount" runat="server" Font-Bold="True" Width="100%">0</asp:Label>
    </td>
<td  align="right" style="width: 154px; height: 22px;" ><b> Total Paid Amount : </b></td>
<td  align="right" style="height: 22px">
<asp:Label ID="lblTotalPaid" runat="server" Font-Size="8pt" Font-Bold="True" Width="100%">0</asp:Label>
</td>
</tr>
<tr>
<td  align="right" style="width: 160px; height: 19px;" ></td>
<td align="right" style="width: 139px; height: 19px;"></td>
<td  align="right" style="width: 154px; height: 19px;" ><b> Total Due : </b></td>
<td  align="right" style="height: 19px">
<asp:Label ID="lblTotalDue" runat="server" Font-Size="8pt" Font-Bold="True" Width="100%">0</asp:Label>
</td>
</tr>
<tr>
<td colspan="2" style="font-size:8pt;">
<img src="img/box_bottom_filet.gif" width="100%" alt="" />Note: <br />1) Bill includes current month. <br />2) negative value indicates the student is in debt.</td>
</tr>
</table>
</ContentTemplate>
</asp:UpdatePanel>
</td>
<td style="width:1%;" align="center">
    <img src="img/box_bottom_hori.gif" width="1px" height="100%" alt="" /></td>
<td style="width:40%; vertical-align:top;" align="center">
<asp:UpdatePanel ID="UpdatePanel1" runat="server"  UpdateMode="Conditional">
    <ContentTemplate>
        <asp:GridView ID="dgPay" runat="server" AllowPaging="True" AllowSorting="True" 
            AlternatingRowStyle-CssClass="alt" AutoGenerateColumns="False" 
            BackColor="White" BorderColor="LightGray" BorderStyle="Solid" BorderWidth="1px" 
            CellPadding="2" CssClass="mGrid" Font-Size="8pt" 
            onrowdatabound="dgPay_RowDataBound" onrowdeleting="dgPay_RowDeleting" 
            PagerStyle-CssClass="pgr" PageSize="30" Width="100%">
            <HeaderStyle BackColor="LightGray" Font-Bold="True" Font-Size="8pt" 
                ForeColor="Black" HorizontalAlign="Center" />
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton ID="lbDelete" runat="server" CausesValidation="False" 
                            CommandName="Delete" ImageAlign="Middle" ImageUrl="~/img/delete.png" 
                            OnClientClick="javascript:return window.confirm('are u really want to delete these data?')" 
                            Text="Delete" />
                    </ItemTemplate>
                    <ItemStyle Font-Size="8pt" HorizontalAlign="Center" Width="5%" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Pay ID">
                    <ItemTemplate>
                        <asp:TextBox ID="txtPayId" runat="server" CssClass="tbr" Font-Size="8pt" 
                            MaxLength="7" SkinID="tbPlain" Text='<%#Eval("pay_id") %>' Width="100%"></asp:TextBox>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Right" Width="1%" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Payment Name">
                    <ItemTemplate>
                        <asp:DropDownList ID="ddlPayName" runat="server" OnSelectedIndexChanged="ddlPayName_SelectedIndexChanged" AutoPostBack="true"
                            DataSource="<%#PopulatePayType()%>" DataTextField="pay_head_id" DataValueField="pay_id" Font-Size="8pt" Height="18px" 
                            SelectedValue='<%# Eval("pay_id") %>' SkinID="ddlPlain" Width="100%">
                        </asp:DropDownList>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Left" Width="50%" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Amount">
                    <ItemTemplate>
                        <asp:TextBox ID="txtPayAmt" runat="server" AutoPostBack="true" CssClass="tbr"  onkeypress="return isNumber(event)"
                            Font-Size="8pt" MaxLength="9" OnTextChanged="txtPayAmt_TextChanged" onFocus="this.select()"
                            SkinID="tbPlain" Text='<%#Eval("pay_amt") %>' Width="90%"></asp:TextBox>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Right" Width="20%" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Discount">
                    <ItemTemplate>
                        <asp:TextBox ID="txtDiscountAmt" runat="server" AutoPostBack="true" CssClass="tbr" onkeypress="return isNumber(event)"
                            Font-Size="8pt" MaxLength="9" OnTextChanged="txtPayAmt_TextChanged" onFocus="this.select()"
                            SkinID="tbPlain" Text='<%#Eval("Discount_AMT") %>' Width="90%"></asp:TextBox>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Right" Width="20%" />
                </asp:TemplateField>

            </Columns>
            <EditRowStyle BackColor="" />
            <AlternatingRowStyle BackColor="" />
        </asp:GridView>
    </ContentTemplate>
    <%--<Triggers> <asp:AsyncPostBackTrigger ControlID="lblTotalAmount" EventName="Load"/> </Triggers>--%>
</asp:UpdatePanel>
</td>
</tr>
<tr>
<td colspan="3">
    <table style="width: 100% ; border:2;">
        <tr>
            <td colspan="2">   &nbsp;   </td>
            
        </tr>   
        <tr>
        <td> 
            <asp:GridView ID="dgPaymentHistory" runat="server" AllowPaging="True" 
                AllowSorting="True" AlternatingRowStyle-CssClass="alt" BackColor="White" 
                BorderColor="LightGray" BorderStyle="Solid" BorderWidth="1px" CellPadding="2" 
                CssClass="mGrid" Font-Size="8pt" ForeColor="#333333" PagerStyle-CssClass="pgr" 
                PageSize="20" Width="100%">
                <AlternatingRowStyle CssClass="alt" />
                <PagerStyle HorizontalAlign="Center" />
            </asp:GridView>
            </td>
        </tr>    
    </table>
    </td>
</tr>
</table> 
</asp:Panel>
</td>
<td style="width:1%;"></td>
</tr>
        </table>
                


</div>

</asp:Content>

