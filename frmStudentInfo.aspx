<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="frmStudentInfo.aspx.cs" Inherits="frmStudentInfo" %>

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
<script type="text/javascript">
    function SetImage() {
        document.getElementById('<%=lbImgUpload.ClientID %>').click();
    }
</script>
<script type="text/javascript">
    $(function () {
        $("#tabs").tabs();
        $("#MyAccordion").accordion();
    });  
</script>
<table  id="pageFooterWrapper">
 <tr>
	<td style="vertical-align:middle; height:100%;" align="center">
        <asp:Button  ID="BtnDelete" runat="server"  ToolTip="Delete Record"   
           
            
            
            onclientclick="javascript:return window.confirm('are u really want to delete these data')"  
            Text="Delete" Width="100px" Height="33px" BorderStyle="Outset" 
            BorderWidth="1px" onclick="BtnDelete_Click"/></td>	

         

      
	<td style="vertical-align:middle;" align="center">
        <asp:Button ID="BtnSave" 
            runat="server" ToolTip="Save Record" 
             Text="Save"             
             Width="100px" Height="33px" BorderStyle="Outset" BorderWidth="1px" onclick="BtnSave_Click" 
            /></td>
	<td style="vertical-align:middle;" align="center">
        <asp:Button ID="BtnReset" runat="server" ToolTip="Clear Form" 
             
            Text="Clear" 
             Width="100px" Height="33px" BorderStyle="Outset" BorderWidth="1px" onclick="BtnReset_Click" 
            /></td>      
             
             <td style="vertical-align:middle;" align="center">
       <asp:Button ID="PrintButton" runat="server" ToolTip="P"         
            Text="Print" 
             Width="100px" Height="33px" BorderStyle="Outset" BorderWidth="1px" onclick="PrintButton_Click" 
                   /></td>       
             
                  
                  
	</tr>		
	</table>
<table style="width: 100%">
    <tr>
       <td style="width: 2%"></td>
        <td style="width: 96%">
           <table style="width: 100%">
               <tr>
                   <td style="width: 14%"></td>
                   <td style="width: 2%"></td>
                   <td style="text-align: center;" colspan="5">
      <asp:Label ID="Label3" runat="server" BackColor="#CCCCCC" Font-Bold="True" 
          Font-Size="Medium" ForeColor="#CC3300" Text="ADMISSION FORM" 
          Width="95%" Height="25px" BorderStyle="Inset"></asp:Label>
                   </td>
                   <td style="width: 2%"></td>
                   <td style="width: 24%"></td>
               </tr>
               <tr>
                   <td style="width: 14%; text-align: right; font-weight: 700;">&nbsp;</td>
                   <td style="width: 2%; text-align: center; font-weight: 700;">&nbsp;</td>
                   <td style="width: 20%">&nbsp;</td>
                   <td style="width: 2%">&nbsp;</td>
                   <td style="width: 14%">&nbsp;</td>
                   <td style="width: 2%">&nbsp;</td>
                   <td style="width: 20%">&nbsp;</td>
                   <td style="width: 2%">&nbsp;</td>
                   <td style="width: 24%" >
                       &nbsp;</td>
               </tr>
           <tr>
               <td style="width: 14%; text-align: right; font-weight: 700;">Search</td>
               <td style="width: 2%; text-align: center; font-weight: 700;">:</td>
               <td colspan="5">
                       <asp:TextBox ID="txtSearch" runat="server" 
                           PlaceHolder="Sl No.-Student Name-Father Name-Mother Name-Phone No" Width="100%" 
                           AutoPostBack="True" ontextchanged="txtSearch_TextChanged"></asp:TextBox>
                   <ajaxtoolkit:AutoCompleteExtender ID="txtSearch_AutoCompleteExtender"
                                                     runat="server" CompletionInterval="20" CompletionSetCount="30"
                                                     EnableCaching="true" MinimumPrefixLength="1"
                                                     ServiceMethod="GetStudentInfo" ServicePath="~/AutoComplete.asmx"
                                                     TargetControlID="txtSearch">
                   </ajaxtoolkit:AutoCompleteExtender>
                   </td>
               <td style="width: 2%">&nbsp;</td>
               <td style="width: 24%" >
                   &nbsp;</td>
           </tr>
               <tr>
                   <td style="width: 14%; text-align: right; font-weight: 700;">Sl.No.</td>
                   <td style="width: 2%; text-align: center; font-weight: 700;">:</td>
                   <td style="width: 20%">
                       <asp:TextBox ID="txtSlNo" runat="server" Width="100%"></asp:TextBox>
                   </td>
                   <td style="width: 2%">&nbsp;</td>
                   <td style="width: 14%; text-align: right; font-weight: 700;">NID</td>
                   <td style="width: 2%; text-align: center; font-weight: 700;">:</td>
                   <td style="width: 20%">
                       <asp:TextBox ID="txtNid" runat="server" Width="100%"></asp:TextBox>
                   </td>
                   <td style="width: 2%; text-align: center;">&nbsp;</td>
                   <td style="width: 24%" rowspan="7">
                <asp:Image ID="imgStd" runat="server" Height="165px" Width="145px" BorderStyle="Solid" BackColor="#EFF3FB" BorderWidth="1px"  />  
               </tr>
               <tr>
                   <td style="width: 14%; text-align: right; font-weight: 700;">Name</td>
                   <td style="width: 2%; text-align: center; font-weight: 700;">:</td>
                   <td style="width: 20%">
                       <asp:TextBox ID="txtName" runat="server" Width="100%"></asp:TextBox>
                   </td>
                   <td style="width: 2%; text-align: center;">
        <asp:Label ID="Label4" runat="server" Font-Bold="True" ForeColor="Red" Text="*"></asp:Label>
                   </td>
                   <td style="width: 14%; text-align: right; font-weight: 700;">Phone No.</td>
                   <td style="width: 2%; text-align: center; font-weight: 700;">:</td>
                   <td style="width: 20%">
                       <asp:TextBox ID="txtStdPhoneNo" runat="server" Width="100%"></asp:TextBox>
                   </td>
                   <td style="width: 2%; text-align: center;">
        <asp:Label ID="Label1" runat="server" Font-Bold="True" ForeColor="Red" Text="*"></asp:Label>
                   </td>
               </tr>
               <tr>
                   <td style="width: 14%; text-align: right; font-weight: 700;">Father Name</td>
                   <td style="width: 2%; text-align: center; font-weight: 700;">:</td>
                   <td style="width: 20%">
                       <asp:TextBox ID="txtFatehrName" runat="server" Width="100%"></asp:TextBox>
                   </td>
                   <td style="width: 2%; text-align: center;">&nbsp;</td>
                   <td style="width: 14%; text-align: right; font-weight: 700;">Phone No.</td>
                   <td style="width: 2%; text-align: center; font-weight: 700;">:</td>
                   <td style="width: 20%">
                       <asp:TextBox ID="txtFthPhoneNo" runat="server" Width="100%"></asp:TextBox>
                   </td>
                   <td style="width: 2%; text-align: center;">&nbsp;</td>
               </tr>
               <tr>
                   <td style="width: 14%; text-align: right; font-weight: 700;">Mother Name</td>
                   <td style="width: 2%; text-align: center; font-weight: 700;">:</td>
                   <td style="width: 20%">
                       <asp:TextBox ID="txtMthName" runat="server" Width="100%"></asp:TextBox>
                   </td>
                   <td style="width: 2%; text-align: center;">&nbsp;</td>
                   <td style="width: 14%; text-align: right; font-weight: 700;">Phone No.</td>
                   <td style="width: 2%; text-align: center; font-weight: 700;">:</td>
                   <td style="width: 20%">
                       <asp:TextBox ID="txtMthPhoneNo" runat="server" Width="100%"></asp:TextBox>
                   </td>
                   <td style="width: 2%; text-align: center;">&nbsp;</td>
               </tr>
           <tr>
               <td style="width: 14%; text-align: right; font-weight: 700;">Date Of Birth</td>
               <td style="width: 2%; text-align: center; font-weight: 700;">:</td>
               <td style="width: 20%">
        <asp:TextBox runat="server" TabIndex="33" Width="100%" ID="txtDBO"  align="center" 
                           placeholder="dd/MM/yyyy"></asp:TextBox>

        <ajaxToolkit:CalendarExtender runat="server" Format="dd/MM/yyyy" Enabled="True" 
                           TargetControlID="txtDBO" ID="txtDBO_CalendarExtender">
                       </ajaxToolkit:CalendarExtender>
               </td>
               <td style="width: 2%; text-align: center;">&nbsp;</td>
               <td style="width: 14%; text-align: right; font-weight: 700;">Email</td>
               <td style="width: 2%; text-align: center; font-weight: 700;">:</td>
               <td style="width: 20%">
                       <asp:TextBox ID="txtEmail" runat="server" Width="100%"></asp:TextBox>
               </td>
               <td style="width: 2%; text-align: center;">&nbsp;</td>
           </tr>
           <tr>
               <td style="width: 14%; text-align: right; font-weight: 700;">Nationality</td>
               <td style="width: 2%; text-align: center; font-weight: 700;">:</td>
               <td style="width: 20%">
                       <asp:TextBox ID="txtNationality" runat="server" Width="100%"></asp:TextBox>
               </td>
               <td style="width: 2%; text-align: center;">&nbsp;</td>
               <td style="width: 14%; text-align: right; font-weight: 700;">Religion</td>
               <td style="width: 2%; text-align: center; font-weight: 700;">:</td>
               <td style="width: 20%">
                       <asp:TextBox ID="txtReligion" runat="server" Width="100%"></asp:TextBox>
               </td>
               <td style="width: 2%; text-align: center;">&nbsp;</td>
           </tr>
           <tr>
               <td style="width: 14%; text-align: right; font-weight: 700;">Marital Status</td>
               <td style="width: 2%; text-align: center; font-weight: 700;">:</td>
               <td style="width: 20%">
                       <asp:TextBox ID="txtMaritalStatus" runat="server" Width="100%"></asp:TextBox>
               </td>
               <td style="width: 2%; text-align: center;">&nbsp;</td>
               <td style="width: 14%; text-align: right; font-weight: 700;">Last Education</td>
               <td style="width: 2%; text-align: center; font-weight: 700;">:</td>
               <td style="width: 20%">
                       <asp:TextBox ID="txtLastEducation" runat="server" Width="100%"></asp:TextBox>
               </td>
               <td style="width: 2%; text-align: center;">&nbsp;</td>
           </tr>
           <tr>
               <td style="width: 14%; text-align: right; font-weight: 700;">Bord</td>
               <td style="width: 2%; text-align: center; font-weight: 700;">:</td>
               <td style="width: 20%">
                       <asp:TextBox ID="txtBord" runat="server" Width="100%"></asp:TextBox>
               </td>
               <td style="width: 2%">&nbsp;</td>
               <td style="width: 14%; text-align: right; font-weight: 700;">Result</td>
               <td style="width: 2%; text-align: center; font-weight: 700;">:</td>
               <td style="width: 20%">
                       <asp:TextBox ID="txtResult" runat="server" Width="100%"></asp:TextBox>
               </td>
               <td style="width: 2%">&nbsp;</td>
               <td style="width: 24%" >
                       <asp:FileUpload ID="imgUpload" runat="server" Font-Size="8pt" Height="20px" 
                           onchange="javascript:SetImage();" Size="20%" Visible="true" />
                       </td>
           </tr>
               <tr>
                   <td style="width: 14%; text-align: left; font-weight: 500;">
      <asp:Label ID="Label18" runat="server" BackColor="#CCCCCC" Font-Bold="True" 
          Font-Size="Small" ForeColor="#070504 " Text="Present Address" 
          Width="95%" Height="25px" BorderStyle="Inset"></asp:Label>
                   </td>
                   <td style="width: 2%; text-align: center; font-weight: 700;">:</td>
                   <td colspan="5">
                       <asp:TextBox ID="txtPresentAddress" runat="server" TextMode="MultiLine" 
                           Width="100%"></asp:TextBox>
                   </td>
                   <td style="width: 2%; text-align: center;">&nbsp;</td>
                   <td style="width: 24%">
                       <asp:Button ID="lbImgUpload" runat="server" Font-Size="8pt" Height="20px" 
                           onclick="lbImgUpload_Click" style="display:none;" Text="Upload" Width="50px" />
                   </td>
               </tr>
               <tr>
                  <%-- <td style="width: 14%; font-weight: 500;" class="table-text">
                       &nbsp;</td>--%>
                   <td style="width: 14%; text-align: left; font-weight: 500;">
      <asp:Label ID="Label19" runat="server" BackColor="#CCCCCC" Font-Bold="True" 
        Font-Size="Small" ForeColor="#070504 " Text="Permanent Address" 
          Width="95%" Height="25px" BorderStyle="Inset"></asp:Label>
                   </td>
                   <td style="width: 2%; text-align: center; font-weight: 700;">&nbsp;</td>
                   <td style="width: 20%">
                       &nbsp;</td>
                   <td style="width: 2%; text-align: center;">&nbsp;</td>
                   <td style="width: 14%; text-align: right; font-weight: 700;">&nbsp;</td>
                   <td style="width: 2%; text-align: center; font-weight: 700;">&nbsp;</td>
                   <td style="width: 20%">
                       &nbsp;</td>
                   <td style="width: 2%; text-align: center;">&nbsp;</td>
                   <td style="width: 24%"></td>
                  
               </tr>
               <tr>
                   <td style="width: 14%; font-weight: 700; text-align: right;" class="table-text">
                       Village</td>
                   <td style="width: 2%; text-align: center; font-weight: 700;">:</td>
                   <td style="width: 20%">
                       <asp:TextBox ID="txtVillage" runat="server" Width="100%"></asp:TextBox>
                   </td>
                   <td style="width: 2%; text-align: center;">&nbsp;</td>
                   <td style="width: 14%; text-align: right; font-weight: 700;">Post Office</td>
                   <td style="width: 2%; text-align: center; font-weight: 700;">:</td>
                   <td style="width: 20%">
                       <asp:TextBox ID="txtPostOffice" runat="server" Width="100%"></asp:TextBox>
                   </td>
                   <td style="width: 2%; text-align: center;">&nbsp;</td>
                   <td style="width: 24%; text-align: left;">
                       
                   </td>
               </tr>
               <tr>
                   <td style="width: 14%; font-weight: 700; text-align: right;" class="table-text">
                       Police Station</td>
                   <td style="width: 2%; text-align: center; font-weight: 700;">:</td>
                   <td style="width: 20%">
                       <asp:TextBox ID="txtPoliceStation" runat="server" Width="100%"></asp:TextBox>
                   </td>
                   <td style="width: 2%; text-align: center;">&nbsp;</td>
                   <td style="width: 14%; text-align: right; font-weight: 700;">District</td>
                   <td style="width: 2%; text-align: center; font-weight: 700;">:</td>
                   <td style="width: 20%">
                       <asp:TextBox ID="txtDistrict" runat="server" Width="100%"></asp:TextBox>
                   </td>
                   <td style="width: 2%; text-align: center;">&nbsp;</td>
                   <td style="width: 24%; text-align: center;">&nbsp;</td>
               </tr>
          
               <tr>
                   <td style="width: 14%; font-weight: 700; text-align: left;" class="table-text">
      <asp:Label ID="Label20" runat="server" BackColor="#CCCCCC" Font-Bold="True" 
          Font-Size="Small" ForeColor="#070504" Text="Course Information" 
          Width="95%" Height="25px" BorderStyle="Inset"></asp:Label>
                   </td>
                   <td style="width: 2%; text-align: center; font-weight: 700;">:</td>
                   <td style="width: 20%">
                       &nbsp;</td>
                   <td style="width: 2%; text-align: center;">&nbsp;</td>
                   <td style="width: 14%; text-align: right; font-weight: 700;">&nbsp;</td>
                   <td style="width: 2%; text-align: center; font-weight: 700;">&nbsp;</td>
                   <td style="width: 20%">
                       &nbsp;</td>
                   <td style="width: 2%; text-align: center;">
                       &nbsp;</td>
                   <td style="width: 24%; text-align: center;">&nbsp;</td>
               </tr>
          
         
          
        
          
               <tr>
                   <td style="width: 14%; font-weight: 700; text-align: right;" class="table-text">
    Course Name</td>
                   <td style="width: 2%; text-align: center; font-weight: 700;">:</td>
                   <td style="width: 20%">
                       <asp:TextBox ID="txtCourseName" runat="server" Width="100%"></asp:TextBox>
                   </td>
                   <td style="width: 2%; text-align: center;">
        <asp:Label ID="Label8" runat="server" Font-Bold="True" ForeColor="Red" Text="*"></asp:Label>
                   </td>
                   <td style="width: 14%; text-align: right; font-weight: 700;">Trainer Name</td>
                   <td style="width: 2%; text-align: center; font-weight: 700;">:</td>
                   <td style="width: 20%">
                       <asp:TextBox ID="txtTrainerName" runat="server" Width="100%"></asp:TextBox>
                   </td>
                   <td style="width: 2%; text-align: center;">
                       &nbsp;</td>
                   <td style="width: 24%; text-align: center;">&nbsp;</td>
               </tr>
          
               <tr>
                   <td style="width: 14%; font-weight: 700; text-align: right;" class="table-text">
                       Batch No.</td>
                   <td style="width: 2%; text-align: center; font-weight: 700;">:</td>
                   <td style="width: 20%">
                       <asp:TextBox ID="txtBatchNo" runat="server" Width="100%"></asp:TextBox>
                   </td>
                   <td style="width: 2%; text-align: center;">&nbsp;</td>
                   <td style="width: 14%; text-align: right; font-weight: 700;">Class Time</td>
                   <td style="width: 2%; text-align: center; font-weight: 700;">:</td>
                   <td style="width: 20%">
                       <asp:TextBox ID="txtClassTime" runat="server" Width="100%"  onkeypress="return isNumber(event)"></asp:TextBox>
                      
                   </td>
                   <td style="width: 2%; text-align: center;">
        <asp:Label ID="Label17" runat="server" Font-Bold="True" ForeColor="Red" Text="*"></asp:Label>
                   </td>
                   <td style="width: 24%; text-align: center;">
            <asp:Label runat="server" ID="lblTimeAM" Visible="False"></asp:Label>

                   </td>
               </tr>
          
               <tr>
                   <td style="width: 14%; font-weight: 700; text-align: right;" class="table-text">
    Admission Date</td>
                   <td style="width: 2%; text-align: center; font-weight: 700;">:</td>
                   <td style="width: 20%">
        <asp:TextBox runat="server" TabIndex="33" Width="100%" ID="txtAdmissionDate" align="center" 
                           placeholder="dd/MM/yyyy"></asp:TextBox>

        <ajaxToolkit:CalendarExtender runat="server" Format="dd/MM/yyyy" Enabled="True" 
                           TargetControlID="txtAdmissionDate" ID="Calendarextender1">
                       </ajaxToolkit:CalendarExtender>

                   </td>
                   <td style="width: 2%; text-align: center;">&nbsp;</td>
                   <td style="width: 14%; text-align: right; font-weight: 700;">&nbsp;</td>
                   <td style="width: 2%; text-align: center; font-weight: 700;">&nbsp;</td>
                   <td style="width: 20%">
            <asp:RadioButtonList runat="server" RepeatDirection="Horizontal" AutoPostBack="True" 
                           SelectedValue='<%# Bind("StartAmPm") %>' TabIndex="39" Width="40%" 
                           ID="rbStartAmPm" OnSelectedIndexChanged="rbStartAmPm_SelectedIndexChanged">
                <asp:ListItem>AM</asp:ListItem>
<asp:ListItem>PM</asp:ListItem>
</asp:RadioButtonList>

                   </td>
                   <td style="width: 2%; text-align: center;">&nbsp;</td>
                   <td style="width: 24%; text-align: center;">
                       <asp:HiddenField ID="idHiddenField" runat="server" />
                   </td>
               </tr>
           <tr>
               <td style="width: 14%; font-weight: 700; text-align: right;" class="table-text">
                   Course Fee</td>
               <td style="width: 2%; text-align: center; font-weight: 700;">:</td>
               <td style="width: 20%">
                       <asp:TextBox ID="txtCourseFee" runat="server" Width="100%" PlaceHolder="0.00" AutoPostBack="True" onkeypress="return isNumber(event)"
                           ontextchanged="txtCourseFee_TextChanged"  ></asp:TextBox>
                   </td>
               <td style="width: 2%; text-align: center;">
        <asp:Label ID="Label9" runat="server" Font-Bold="True" ForeColor="Red" Text="*"></asp:Label>
                   </td>
               <td style="width: 14%; text-align: right; font-weight: 700;">Waiver&nbsp;&nbsp;</td>
               <td style="width: 2%; text-align: center; font-weight: 700;">:</td>
               <td style="width: 20%">
                       <asp:TextBox ID="txtWaiver" runat="server" Width="100%" AutoPostBack="True" onkeypress="return isNumber(event)"
                           ontextchanged="txtWaiver_TextChanged"  ></asp:TextBox>
                   </td>
               <td style="width: 2%; text-align: center;">
<asp:Label runat="server" Text="%" Font-Bold="True" ForeColor="Red" ID="Label16"></asp:Label>
               </td>
               <td style="width: 24%; text-align: center;">
                   <asp:HiddenField ID="PaidHiddenField" runat="server" />
               </td>
           </tr>
               <tr>
                   <td style="width: 14%; font-weight: 700; text-align: right;" class="table-text">
                       DisCount</td>
                   <td style="width: 2%; text-align: center; font-weight: 700;">:</td>
                   <td style="width: 20%">
                       <asp:TextBox ID="txtDiscount" runat="server" Width="100%" AutoPostBack="True" onkeypress="return isNumber(event)"
                           ontextchanged="txtDiscount_TextChanged"  ></asp:TextBox>

                   </td>
                   <td style="width: 2%; text-align: center;">&nbsp;</td>
                   <td style="width: 14%; text-align: right; font-weight: 700;">Total Amount</td>
                   <td style="width: 2%; text-align: center; font-weight: 700;">:</td>
                   <td style="width: 20%">
                       <asp:TextBox ID="txtTotalAmount" runat="server" Width="100%" ReadOnly="True" onkeypress="return isNumber(event)"
                           AutoPostBack="True"  ></asp:TextBox>

                   </td>
                   <td style="width: 2%; text-align: center;">&nbsp;</td>
                   <td style="width: 24%; text-align: center;">&nbsp;</td>
               </tr>
          
               <tr>
                   <td style="width: 14%; font-weight: 700; text-align: right;" class="table-text">
                       Pay Amount</td>
                   <td style="width: 2%; text-align: center; font-weight: 700;">:</td>
                   <td style="width: 20%">
                       <asp:TextBox ID="txtPayAmount" runat="server" Width="100%" AutoPostBack="True" onkeypress="return isNumber(event)"
                           ontextchanged="txtPayAmount_TextChanged"  ></asp:TextBox>
                   </td>
                   <td style="width: 2%; text-align: center;">&nbsp;</td>
                   <td style="width: 14%; text-align: right; font-weight: 700;">Due Amount</td>
                   <td style="width: 2%; text-align: center; font-weight: 700;">:</td>
                   <td style="width: 20%">
                       <asp:TextBox ID="txtDueAmount" runat="server" Width="100%" ReadOnly="True"   ></asp:TextBox>

                   </td>
                   <td style="width: 2%; text-align: center;">&nbsp;</td>
                   <td style="width: 24%; text-align: center;">&nbsp;</td>
               </tr>
          
               <tr>
                   <td style="width: 14%; font-weight: 700; text-align: right;" class="table-text">
        Addmission Year</td>
                   <td style="width: 2%; text-align: center; font-weight: 700;">:</td>
                   <td style="width: 20%">
                       <asp:TextBox ID="txtAddmissionYear" runat="server" Width="100%" 
                            ></asp:TextBox>
                   </td>
                   <td style="width: 2%; text-align: center;">&nbsp;</td>
                   <td style="width: 14%; text-align: right; font-weight: 700;">Certification Date</td>
                   <td style="width: 2%; text-align: center; font-weight: 700;">:</td>
                   <td style="width: 20%">
        <asp:TextBox runat="server" TabIndex="33" Width="100%" ID="txtCertificationDate" align="center" 
                           placeholder="dd/MM/yyyy"></asp:TextBox>

        <ajaxToolkit:CalendarExtender runat="server" Format="dd/MM/yyyy" Enabled="True" 
                           TargetControlID="txtCertificationDate" 
                           ID="txtCertificationDate_CalendarExtender">
                       </ajaxToolkit:CalendarExtender>

                   </td>
                   <td style="width: 2%; text-align: center;">&nbsp;</td>
                   <td style="width: 24%; text-align: center;">&nbsp;</td>
               </tr>
          
               <tr>
                   <td style="width: 14%; font-weight: 700; text-align: right;" class="table-text">
        Weekly Schedule </td>
                   <td style="width: 2%; text-align: center; font-weight: 700;">:</td>
                   <td colspan="5">
            <asp:CheckBox runat="server" Text="Staturday" ID="SatCheckBox" 
                          ></asp:CheckBox>

            <asp:CheckBox runat="server" Text="SunDay" ID="SunCheckBox" 
                          ></asp:CheckBox>

            <asp:CheckBox runat="server" Text="MonDay" ID="MonCheckBox" 
                          ></asp:CheckBox>

            <asp:CheckBox runat="server" Text="TuesDay" ID="TueCheckBox" 
                          ></asp:CheckBox>

            <asp:CheckBox runat="server" Text="WednessDay" ID="WedCheckBox" 
                          ></asp:CheckBox>

            <asp:CheckBox runat="server" Text="Thusday" ID="ThusCheckBox" 
                          ></asp:CheckBox>

            <asp:CheckBox runat="server" Text="Friday" ID="FriCheckBox" 
                           ></asp:CheckBox>

                   </td>
                   <td style="width: 2%; text-align: center;">&nbsp;</td>
                   <td style="width: 24%; text-align: center;">&nbsp;</td>
               </tr>
          
           </table>
        </td>
        <td style="width: 2%"></td>
    </tr>
</table>
</asp:Content>

