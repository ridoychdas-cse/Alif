<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="frmPayment.aspx.cs" Inherits="frmPayment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <script type="text/javascript">
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
 <div id="frmMainDiv" style="width:99.5%; background-color:transparent; padding:3.5px;">
    <div id="Div1" style="background-color:White; width:100%;">
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

       <%--  <td style="vertical-align:middle;" align="center">
        <asp:Button ID="btnPPos" runat="server" ToolTip="Clear Form" Text="P.Pos" 
                 Width="100px" onclick="btnPPos_Click" Visible="False" />
        </td>--%>
	</tr>		
	</table>

    <table style="width:100%">
<tr>
<td style="width:10%">&nbsp;</td>
<td style="width:60%">
    &nbsp;</td>
<td style="width:30%">&nbsp;</td>
</tr>
<tr>
<td style="width:10%"></td>
<td style="width:60%">
 <asp:UpdatePanel ID="UpdatePanelMST" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                 <fieldset style=" text-align:left; vertical-align: top; border: solid 1px #8BB381;"><legend style="color: maroon;"><b>Payment Information</b></legend>
<table style="width:100%">
<tr>
<td style="width:15%" align="right">
    <asp:Label ID="Label2" runat="server" Text="Search Student"></asp:Label>
    </td>
<td colspan="5">
                            <asp:TextBox ID="txtSearchStudent" runat="server" CssClass="TextNewColor" 
                                AutoPostBack="True" placeholder="Search ** Student ID & Name - Roll No. - Class - Section - Phone Number"
                                Font-Size="8pt" MaxLength="20"  SkinID="tbPlain" Width="99%" 
                                ontextchanged="txtSearchStudent_TextChanged" 
        Height="20px"></asp:TextBox>
                                <ajaxtoolkit:AutoCompleteExtender ID="txtSearchStudent_AutoCompleteExtender"
                                                        runat="server" 
        CompletionInterval="20" CompletionSetCount="30"
                                                        EnableCaching="true" MinimumPrefixLength="2"
                                                        
        ServiceMethod="GetCompletionStudentId" ServicePath="~/AutoComplete.asmx"
                                                        
        TargetControlID="txtSearchStudent">
                                                    </ajaxtoolkit:AutoCompleteExtender>
                        </td>
</tr>
<tr>
<td style="width:15%" align="right">
                            <asp:Label ID="lblSubjectId" runat="server" 
        Font-Size="8pt">Student ID</asp:Label>
                        </td>
<td style="width:20%">
                            <asp:TextBox ID="txtStudentId" runat="server" AutoPostBack="true" 
                                CssClass="tbc" Enabled="true" Font-Size="8pt" MaxLength="15" 
                                ontextchanged="txtStudentId_TextChanged" 
        SkinID="tbPlain" Width="92%"></asp:TextBox>
                        </td>
<td style="width:13%" align="right">
    <asp:Label ID="Label3" runat="server" Text="Payment Date"></asp:Label>
    </td>
<td style="width:20%">
<asp:TextBox SkinID="tbPlain" ID="txtChequeDate" runat="server" placeholder="dd/MM/yyyy" Width="92%" 
        Font-Size="8" CssClass="tbc" 
        MaxLength="6" ></asp:TextBox>
<ajaxtoolkit:calendarextender runat="server" ID="Calendarextender1" 
        TargetControlID="txtChequeDate" Format="dd/MM/yyyy"/>
                        </td>
<td style="width:12%" align="right">
    <asp:Label ID="Label8" runat="server" Text="Course Fees"></asp:Label>
    </td>
<td style="width:20%">
    <asp:TextBox ID="txtCourseFees" runat="server" Width="100%" 
        onkeypress="return isNumber(event)" placeHolder="0.00" 
        style="text-align:right;" Enabled="False" ReadOnly="True" 
        BackColor="#FFCCFF"></asp:TextBox>
    </td>
</tr>
<tr>
<td style="width:15%" align="right">
    <asp:Label ID="Label17" runat="server" Text="Previous Discount"></asp:Label>
    </td>
<td style="width:20%">
    <asp:TextBox ID="txtPrevDiscount" runat="server" Enabled="False" 
        onkeypress="return isNumber(event)" placeHolder="0.00" ReadOnly="True" 
        style="text-align:right;" Width="92%" BackColor="#FFCCFF"></asp:TextBox>
    </td>
<td style="width:13%" align="right">
    <asp:Label ID="Label19" runat="server" Text="Waiver"></asp:Label>
    </td>
<td style="width:20%">
    <asp:TextBox ID="txtPrevWaiver" runat="server" AutoPostBack="True" 
        Enabled="False" onkeypress="return isNumber(event)" 
        ontextchanged="txtPayment_TextChanged" placeHolder="0.00" ReadOnly="True" 
        style="text-align:right;" Width="92%" BackColor="#FFCCFF"></asp:TextBox>
    </td>
<td style="width:12%" align="right">
    <asp:Label ID="Label20" runat="server" Text="Total Amount"></asp:Label>
    </td>
<td style="width:20%">
    <asp:TextBox ID="txtTotalPayable" runat="server" Enabled="False" 
        onkeypress="return isNumber(event)" placeHolder="0.00" ReadOnly="True" 
        style="text-align:right;" Width="100%" BackColor="#FFCCFF"></asp:TextBox>
    </td>
</tr>
    <tr>
        <td align="right" style="width:15%">
            <asp:Label ID="Label16" runat="server" Text="Previous Paid"></asp:Label>
        </td>
        <td style="width:20%">
            <asp:TextBox ID="txtPaidAmount" runat="server" Enabled="False" 
                onkeypress="return isNumber(event)" placeHolder="0.00" ReadOnly="True" 
                style="text-align:right;" Width="92%" BackColor="#FFCCFF"></asp:TextBox>
        </td>
        <td align="right" style="width:13%">
            <asp:Label ID="Label18" runat="server" Text="Previous Due"></asp:Label>
        </td>
        <td style="width:20%">
            <asp:TextBox ID="txtPrevDueAmount" runat="server" Enabled="False" 
                onkeypress="return isNumber(event)" placeHolder="0.00" ReadOnly="True" 
                style="text-align:right;" Width="92%" BackColor="#FFCCFF"></asp:TextBox>
        </td>
        <td align="right" style="width:12%">
            <asp:Label ID="Label11" runat="server" Text="Discount"></asp:Label>
        </td>
        <td style="width:20%">
            <asp:TextBox ID="txtDiscount" runat="server" AutoPostBack="True" 
                onkeypress="return isNumber(event)" ontextchanged="txtDiscount_TextChanged" 
                placeHolder="0.00" style="text-align:right;" Width="100%"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td align="right" style="width:15%">
            &nbsp;</td>
        <td style="width:20%">
            &nbsp;</td>
        <td align="right" style="width:13%">
            &nbsp;</td>
        <td style="width:20%">
            &nbsp;</td>
        <td align="right" style="width:12%">
            <asp:Label ID="Label12" runat="server" Text="Pay Amount"></asp:Label>
        </td>
        <td style="width:20%">
            <asp:TextBox ID="txtPayment" runat="server" AutoPostBack="True" 
                onkeypress="return isNumber(event)" ontextchanged="txtPayment_TextChanged" 
                placeHolder="0.00" style="text-align:right;" Width="100%"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td align="right" style="width:15%">
            &nbsp;</td>
        <td style="width:20%">
            <asp:Label ID="TotalDiscount" runat="server" Visible="False"></asp:Label>
            <asp:Label ID="lblTotalPaid" runat="server" Visible="False"></asp:Label>
        </td>
        <td align="right" style="width:13%">
            <asp:Label ID="RealID" runat="server" Visible="False"></asp:Label>
        </td>
        <td style="width:20%">
            <asp:Label ID="lblDueAmount" runat="server" Visible="False"></asp:Label>
        </td>
        <td align="right" style="width:12%">
            <asp:Label ID="Label13" runat="server" Text="Current Due"></asp:Label>
        </td>
        <td style="width:20%">
            <asp:TextBox ID="txtCurrDue" runat="server" AutoPostBack="True" 
                onkeypress="return isNumber(event)" ontextchanged="txtPayment_TextChanged" 
                placeHolder="0.00" style="text-align:right;" Width="100%" 
                BackColor="#CCFFFF" Enabled="False" ReadOnly="True"></asp:TextBox>
        </td>
    </tr>
</table>
</fieldset>
                </ContentTemplate></asp:UpdatePanel>
</td>
<td style="width:30%">
<fieldset style=" text-align:left; vertical-align: top; border: solid 1px #8BB381;">
<legend style="color: maroon;"><b>Student Information</b></legend>
            <asp:UpdatePanel ID="UpdatePanelDetails" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                <table style="width: 100%; height: 120px;">
                    <tr>
                        <td style="width: 15%">
                            <asp:Label ID="lblName0" runat="server" Font-Bold="True" Font-Size="8pt" 
                                Width="100%">Name :</asp:Label>
                        </td>
                        <td align="left" colspan="3" style="height: 26px">
                            <asp:Label ID="lblName" runat="server" Font-Bold="True" Font-Size="8pt"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 15%">
                            <asp:Label ID="lblClass8" runat="server" Font-Bold="True" Font-Size="8pt" 
                                Width="100%">ID No :</asp:Label>
                        </td>
                        <td style="width: 30%">
                            <asp:Label ID="lblSTDID" runat="server" Font-Bold="True" Font-Size="8pt" 
                                Width="100%"></asp:Label>
                        </td>
                        <td style="width: 15%">
                            <asp:Label ID="lblClass0" runat="server" Font-Bold="True" Font-Size="8pt" 
                                Width="100%">Batch :</asp:Label>
                        </td>
                        <td style="width: 30%">
                            <asp:Label ID="lblBatch" runat="server" Font-Bold="True" Font-Size="8pt" 
                                Width="100%"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 15%">
                            <asp:Label ID="lblClass2" runat="server" Font-Bold="True" Font-Size="8pt" 
                                Width="100%">Course :</asp:Label>
                        </td>
                        <td colspan="3">
                            <asp:Label ID="lblCourseName" runat="server" Font-Bold="True" Font-Size="8pt" 
                                Width="100%"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 15%">
                            <asp:Label ID="lblClassId" runat="server" Visible="False" style="display:none;"></asp:Label>
                            <asp:Label ID="lblVersonId" runat="server" Visible="False" 
                                style="display:none;"></asp:Label>
                            <asp:Label ID="lblSectionId" runat="server" Visible="False" 
                                style="display:none;"></asp:Label>
                            <asp:Label ID="lblRoll" runat="server" Visible="False" style="display:none;"></asp:Label>
                            <asp:Label ID="lblShiftId" runat="server" Visible="False" style="display:none;"></asp:Label>
                            <asp:Label ID="lblClass7" runat="server" Font-Bold="True" Font-Size="8pt" 
                                Width="100%">Date :</asp:Label>
                        </td>
                        <td style="width: 30%">
                            <asp:Label ID="lblDate" runat="server" Font-Bold="True" Font-Size="8pt"></asp:Label>
                        </td>
                        <td style="width: 15%">
                            <asp:Label ID="lblClass6" runat="server" Font-Bold="True" Font-Size="8pt" 
                                Width="100%">Year :</asp:Label>
                        </td>
                        <td style="width: 30%">
                            <asp:Label ID="lblYear" runat="server" Font-Bold="True" Font-Size="8pt" 
                                Visible="False"></asp:Label>
                        </td>
                    </tr>
                </table>
                   </ContentTemplate></asp:UpdatePanel>
                </b>
                </fieldset>
                </td>
</tr>

</table>
</div>
</asp:Content>

