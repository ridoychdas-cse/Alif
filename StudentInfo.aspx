<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="StudentInfo.aspx.cs" Inherits="StudentInfo" Title="Student Information" Theme="Themes" MaintainScrollPositionOnPostback="true" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Linq" %>
<%--<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">

    <script type="text/javascript">
    function SetImage()
    {        
        document.getElementById('<%=lbImgUpload.ClientID %>').click();
    }
</script>
 <script type="text/javascript">
     $(function () {
         $("#tabs").tabs();
         $("#MyAccordion").accordion();
     });  
        </script>  
<div id="frmMainDiv" style="background-color:White; width:100%;">

<table  id="pageFooterWrapper">
 <tr>
	<td style="vertical-align:middle; height:100%;" align="center">
        <asp:Button  ID="BtnDelete" runat="server"  ToolTip="Delete Record"   
            OnClick="BtnDelete_Click" 
            
            
            onclientclick="javascript:return window.confirm('are u really want to delete these data')"  
            Text="Delete" Width="100px" Height="33px" BorderStyle="Outset" 
            BorderWidth="1px"/></td>	

            <td style="vertical-align:middle;" align="center">
       <asp:Button ID="btnPrint" runat="server" ToolTip="Print" onclick="BtnFind_Click"         
            Text="Find" 
             Width="100px" Height="33px" BorderStyle="Outset" BorderWidth="1px" 
                     /></td> 

             <td style="vertical-align:middle; height:100%;" align="center">
                 <asp:Button ID="btnUpdate" runat="server" ToolTip="Update Record" 
             Text="Update"             
             Width="100px" Height="33px" BorderStyle="Outset" BorderWidth="1px" 
                     onclick="btnUpdate_Click"/></td>	
	<td style="vertical-align:middle;" align="center">
        <asp:Button ID="BtnSave" 
            runat="server" ToolTip="Save Record" 
            OnClick="BtnSave_Click" Text="Save"             
             Width="100px" Height="33px" BorderStyle="Outset" BorderWidth="1px" 
            /></td>
	<td style="vertical-align:middle;" align="center">
        <asp:Button ID="BtnReset" runat="server" ToolTip="Clear Form" 
            OnClick="BtnReset_Click"   
            Text="Clear" 
             Width="100px" Height="33px" BorderStyle="Outset" BorderWidth="1px" 
            /></td>      
             
             <td style="vertical-align:middle;" align="center">
       <asp:Button ID="PrintButton" runat="server" ToolTip="P"         
            Text="Print" 
             Width="100px" Height="33px" BorderStyle="Outset" BorderWidth="1px" 
                     onclick="PrintButton_Click" /></td>       
             
             <td style="vertical-align:middle;" align="center">
                 <asp:Button ID="Principalbtn" runat="server" onclick="Principalbtn_Click" 
                     Text="P-Print" Visible="true" Height="33px" />
     </td>       
             
             <td style="vertical-align:middle;" align="center">
       <asp:Button ID="TCPrintButton" runat="server" ToolTip="P"         
            Text="TC Print" 
             Width="100px" Height="33px" BorderStyle="Outset" BorderWidth="1px" 
                     onclick="TCPrintButton_Click" Visible="False" /></td>         
                  
             <td style="vertical-align:middle;" align="center">
                 <asp:Button ID="btnInsFeePrint" runat="server" onclick="btnInsFeePrint_Click" 
                     Text="Ins. Fee Print" Visible="False" />
     </td>         
                  
	</tr>		
	</table>

<table style="width:100%; color:Black;" >
<tr>
<td style="width:1%;"></td>
<td style="width:98%; margin-left: 40px;" align="center">
<table style="width:100%; font-size:8pt; border:solid 1px lightgray;">
<tr>
  <td style="width: 13%; height: 27px; font-size:8pt;" align="right">&nbsp;</td>
	<td style="height: 27px;" align="center" colspan="4">
      <asp:Label ID="Label3" runat="server" BackColor="#CCCCCC" Font-Bold="True" 
          Font-Size="Medium" ForeColor="#CC3300" Text="Student Information Entry Form" 
          Width="95%" Height="25px" BorderStyle="Inset"></asp:Label>
            </td>
	<td style="width:3%;" align="center">
                &nbsp;</td>
	<td style="width: 31%; height: 27px; vertical-align:top;" align="center" 
        rowspan="9" colspan="2">
            <table style="width:100%;">
            <tr>
            <td align="center">
            <asp:FileUpload ID="imgUpload" runat="server" Visible="true" Size="20%" Height="20px" Font-Size="8pt" onchange="javascript:SetImage();"/>
            <asp:Button ID="lbImgUpload" runat="server" Text="Upload" Font-Size="8pt"  
                    Width="50px" Height="20px" style="display:none;" onclick="lbImgUpload_Click"></asp:Button>
            </td>
            </tr>
            <tr>
            <td style=" height: 162px; vertical-align:top;" align="center">
                <asp:Image ID="imgStd" runat="server" Height="165px" Width="145px" BorderStyle="Solid" BackColor="#EFF3FB" BorderWidth="1px"  />  
            </td>	
            </tr>
            </table>
	</td>
</tr>
<tr>
  <td style="width: 13%; height: 27px; font-size:8pt;" align="right">Student ID</td>
	<td style="width: 18%; height: 27px;" align="left">
        <asp:TextBox SkinID="tbPlain" ID="txtStudentId"  CssClass="tbc"
            runat="server" Width="100%" Font-Size="8pt" TabIndex="1" ></asp:TextBox></td>
	<td style="width:3%;" align="center">
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="Label1" runat="server" Font-Bold="True" ForeColor="Red" Text="*"></asp:Label>
    </td>
	<td style="width: 13%; height: 27px; font-size:8pt;" align="right">Email No </td>
	<td style="width: 18%; height: 27px; margin-left: 40px;" align="left">
    <asp:TextBox SkinID="tbPlain" ID="txtEmail"  CssClass="tbl" Enabled="true"
            runat="server" Width="100%"  AutoPostBack="False" 
            Font-Size="8pt" TabIndex="2"></asp:TextBox>
    </td>
	<td style="width:3%;" align="center">
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="Label2" runat="server" Font-Bold="True" ForeColor="Red" Text="*"></asp:Label>
    </td>
</tr>
<tr>
  <td style="width: 13%; height: 27px; font-size:8pt;" align="right">Student Name</td>
	<td style="width: 18%; height: 27px;" align="left">
	<asp:TextBox SkinID="tbPlain" ID="txtFName"  CssClass="tbc" Enabled="true"
            runat="server" Width="100%" AutoPostBack="False" 
            Font-Size="8pt" TabIndex="3"></asp:TextBox>
    </td>
	<td style="width:3%;" align="center">
        &nbsp;</td>
	<td style="width: 13%; height: 27px; font-size:8pt;" align="right">Phone No </td>
	<td style="width: 18%; height: 27px;" align="left">
	    <asp:TextBox ID="txtMobileNo" runat="server" Width="100%" TabIndex="4"></asp:TextBox>
    </td>
	<td style="width:3%;" align="center">
        &nbsp;</td>
</tr>
<tr>
  <td style="width: 13%; height: 27px; font-size:8pt;" align="right">Father&#39;s Name </td>
	<td style="width: 18%; height: 27px;" align="left">
    <asp:TextBox SkinID="tbPlain" ID="txtFthName"  CssClass="tbl" Enabled="true"
            runat="server" Width="100%"  AutoPostBack="False" 
            Font-Size="8pt" MaxLength="100" TabIndex="5"></asp:TextBox>
     </td>
	<td style="width:3%;"></td>
	<td style="width: 13%; height: 27px; font-size:8pt;" align="right">Phone No</td>
	<td style="width: 18%; height: 27px;" align="left">
    <asp:TextBox SkinID="tbPlain" ID="txtFthTel"  CssClass="tbl" Enabled="true"
            runat="server" Width="100%"  AutoPostBack="False" 
            Font-Size="8pt" MaxLength="100" TabIndex="6"></asp:TextBox>
     </td>	 
     <td style="width:3%;" align="center"></td>           
</tr>
<tr>
    <td style="width:13%; height:27px; font-size:8pt;" align="right">Mother&#39;s Name</td>
    <td style="width:18%; height:27px;" align="left">
    <asp:TextBox SkinID="tbPlain" ID="txtMthName"  CssClass="tbl" Enabled="true"
            runat="server" Width="100%"  AutoPostBack="False" 
            Font-Size="8pt" MaxLength="100" TabIndex="7"></asp:TextBox>
    </td>
    <td style="height: 27px"></td>
    <td style="width:13%; height:27px; font-size:8pt;" align="right">
        Phone No
    </td>
    <td style="width:18%; height:27px;" align="left">
    <asp:TextBox SkinID="tbPlain" ID="txtMthTel"  CssClass="tbl" Enabled="true"
            runat="server" Width="100%"  AutoPostBack="False" 
            Font-Size="8pt" MaxLength="100" TabIndex="8"></asp:TextBox>
    </td>
    <td style="width:3%; height: 27px;" align="center">
        </td>
</tr>
<tr>
    <td style="height:27px; font-size:8pt;" align="left" colspan="2"><u> Permanent Address: </u></td>
    <td style="height: 27px"></td>
    <td style="width:13%; height:27px; font-size:8pt;" align="left">
        &nbsp;</td>
    <td style="width:18%; height:27px;" align="left">
        &nbsp;</td>
    <td style="width:3%; height: 27px;" align="center">
        
    </td>
</tr>
<tr>	
	<td style="width: 13%; height: 23px; font-size:8pt;" align="left">House/Road No.</td>
	<td style="width: 53%; height: 23px;" align="left" colspan="4">
        <asp:TextBox SkinID="tbPlain" ID="txtPerLoc"  CssClass="tbl" Enabled="true"
            runat="server" Width="100%"  AutoPostBack="False" 
            Font-Size="8pt" TabIndex="9"></asp:TextBox></td>
            
    </td>           
</tr>
<tr>	
	<td style="width: 13%; height: 27px; font-size:8pt;" align="right">District</td>
	<td style="width: 18%; height: 27px;" align="left">
    <asp:DropDownList SkinID="ddlPlain" ID="ddlPerDistCode" runat="server" AutoPostBack="true" 
            Font-Size="8pt" Width="104%" Height="18px" TabIndex="10" 
            onselectedindexchanged="ddlPerDistCode_SelectedIndexChanged">
     </asp:DropDownList></td>        
    <td style="width:3%;"></td>
  <td style="width: 13%; height: 27px; font-size:8pt;" align="right">Thana</td>
	<td style="width: 18%; height: 27px;" align="left">
	<asp:DropDownList SkinID="ddlPlain" ID="ddlPerThanaCode" runat="server" 
            Font-Size="8pt" Width="104%" Height="18px" TabIndex="11" >
     </asp:DropDownList></td>
     <td style="width:3%;" align="center">
        
    </td>     
</tr>
<tr>	
	<td style="width: 13%; height: 27px; font-size:8pt;" align="right">Post Office</td>
	<td style="width: 18%; height: 27px;" align="left">
    <asp:TextBox SkinID="tbPlain" ID="txtPerPostOffice"  CssClass="tbc" Enabled="true"
            runat="server" Width="100%"  AutoPostBack="False" 
            Font-Size="8pt" TabIndex="12"></asp:TextBox>
    </td>        
    <td style="width:3%;"></td>
  <td style="width: 13%; height: 27px; font-size:8pt;" align="right">Zip/Post Code</td>
	<td style="width: 18%; height: 27px;" align="left">
    <asp:TextBox SkinID="tbPlain" ID="txtPerZipCode"  CssClass="tbc" Enabled="true"
            runat="server" Width="100%"  AutoPostBack="False" 
            Font-Size="8pt" TabIndex="13"></asp:TextBox>
    </td> 
                <td style="width:3%;" align="center">
                    &nbsp;</td>
</tr>
<tr>
    <td align="left" colspan="2">
        <u> Present/Mailing Address: </u>
    </td>
    <td>
    </td>
    <td align="left">
        &nbsp;</td>
    <td>
	    &nbsp;</td>
    <td align="center">
        &nbsp;</td>
<td colspan="2" align="left" style="font-size:8pt;">&nbsp;</td>
</tr>
<tr>	
	<td style="width: 13%; height: 27px; font-size:8pt;" align="right">House/Road No.</td>
	<td style="width: 53%; height: 27px;" align="left" colspan="7">
        <asp:TextBox SkinID="tbPlain" ID="txtMailLoc"  CssClass="tbl" Enabled="true"
            runat="server" Width="99%"  AutoPostBack="False" 
            Font-Size="8pt" TabIndex="14" ontextchanged="txtMailLoc_TextChanged"></asp:TextBox></td>           
</tr>
<tr>	
	<td style="width: 13%; height: 27px; font-size:8pt;" align="right">District</td>
	<td style="width: 18%; height: 27px;" align="left">
    <asp:DropDownList SkinID="ddlPlain" ID="ddlMailDistCode" runat="server" AutoPostBack="true"
            Font-Size="8pt" Width="104%" Height="18px" TabIndex="15" 
            onselectedindexchanged="ddlMailDistCode_SelectedIndexChanged">
     </asp:DropDownList></td>        
    <td style="width:3%;"></td>
  <td style="width: 13%; height: 27px; font-size:8pt;" align="right">Thana</td>
	<td style="width: 18%; height: 27px;" align="left">
	<asp:DropDownList SkinID="ddlPlain" ID="ddlMailThanaCode" runat="server" 
            Font-Size="8pt" Width="104%" Height="18px" TabIndex="16" >
     </asp:DropDownList></td>  
   <td style="width:3%;"></td>
  <td style="width: 13%; height: 27px; font-size:8pt;" align="right">Zip/Post Code</td>
	<td style="width: 18%; height: 27px;" align="left">
	<asp:TextBox SkinID="tbPlain" ID="txtMailZipCode"  CssClass="tbl" Enabled="true"
            runat="server" Width="95%"  AutoPostBack="False" 
            Font-Size="8pt" TabIndex="17" ></asp:TextBox></td>   
</tr>
<tr>	
	<td style="width: 13%; height: 27px; font-size:8pt;" align="right"> Marital Status </td>
	<td style="width: 18%; height: 27px;" align="left">
        <asp:DropDownList SkinID="ddlPlain" ID="ddlMaritalStatus" runat="server" Font-Size="8pt" 
            Width="104%" Height="16px" TabIndex="18">
     <asp:ListItem></asp:ListItem> 
     <asp:ListItem Text="Single" Value="S"></asp:ListItem>
     <asp:ListItem Text="Married" Value="M"></asp:ListItem>
     </asp:DropDownList>
    </td>        
    <td style="width:3%;"></td>
  <td style="width: 13%; height: 27px; font-size:8pt;" align="right">Spous Name</td>
	<td style="width: 18%; height: 27px;" align="left">
    <asp:TextBox SkinID="tbPlain" ID="txtSpousName"  CssClass="tbl" Enabled="true"
            runat="server" Width="100%"  AutoPostBack="False" 
            Font-Size="8pt" TabIndex="19" ></asp:TextBox>
    </td> 
    <td style="width:3%;"></td>
  <td style="width: 13%; height: 27px; font-size:8pt;" align="right">Ocupation</td>
	<td style="width: 18%; height: 27px;" align="left">
    <asp:TextBox SkinID="tbPlain" ID="txtOcupation"  CssClass="tbl" Enabled="true"
            runat="server" Width="95%"  AutoPostBack="False" 
            Font-Size="8pt" MaxLength="100" TabIndex="20"></asp:TextBox>
    </td>      
</tr>
<tr>	
	<td style="width: 13%; height: 27px; font-size:8pt;" align="right"> Blood Group</td>
	<td style="width: 18%; height: 27px;" align="left">
	<asp:DropDownList SkinID="ddlPlain" ID="ddlBloodGroup" runat="server" 
            Font-Size="8pt" Width="104%" Height="18px" TabIndex="21" >
        <asp:ListItem></asp:ListItem>
        <asp:ListItem>B (+ve)</asp:ListItem>
        <asp:ListItem>B (-ve)</asp:ListItem>
        <asp:ListItem>A (+ve)</asp:ListItem>
        <asp:ListItem>A (-ve)</asp:ListItem>
        <asp:ListItem>O (+ve)</asp:ListItem>
        <asp:ListItem>O (-ve)</asp:ListItem>
        <asp:ListItem>AB (+ve)</asp:ListItem>
        <asp:ListItem>AB (-ve)</asp:ListItem>
     </asp:DropDownList>
    </td>        
    <td style="width:3%;">&nbsp;</td>
  <td style="width: 13%; height: 27px; font-size:8pt;" align="right">Religion</td>
	<td style="width: 18%; height: 27px;" align="left">
        <asp:DropDownList SkinID="ddlPlain" ID="ddlReligion" runat="server" Font-Size="8pt" 
            Width="104%" Height="16px" TabIndex="22">
     <asp:ListItem Text="Islam" Value="I"></asp:ListItem> 
     <asp:ListItem Text="Christian" Value="C"></asp:ListItem>
     <asp:ListItem Text="Hindu" Value="H"></asp:ListItem>
     <asp:ListItem Text="Buddha" Value="B"></asp:ListItem>
     </asp:DropDownList>
    </td> 
    <td style="width:3%;">&nbsp;</td>
  <td style="width: 13%; height: 27px; font-size:8pt;" align="right">Birth Date</td>
	<td style="width: 18%; height: 27px;" align="left">
    <asp:TextBox SkinID="tbPlain" ID="txtBirthDt"  CssClass="tbc"
            runat="server" Width="95%" AutoPostBack="False" placeholder="dd/MM/yyyy" Enabled="true"
            Font-Size="8pt" TabIndex="23" ></asp:TextBox>
     <ajaxtoolkit:calendarextender runat="server" ID="Calendarextender3" TargetControlID="txtBirthDt" Format="dd/MM/yyyy"/>
    </td>      
</tr>
<tr>	
	<td style="width: 13%; height: 27px; font-size:8pt;" align="right"> Status</td>
	<td style="width: 18%; height: 27px;" align="left">
        <u> 
        <asp:DropDownList SkinID="ddlPlain" ID="ddlStatus" runat="server" Font-Size="8pt" 
            Width="104%" Height="16px" AutoPostBack="True" 
            onselectedindexchanged="ddlStatus_SelectedIndexChanged" TabIndex="24">
     <asp:ListItem Value="1">Active</asp:ListItem> 
     <asp:ListItem Value="2">Archive</asp:ListItem>
     <asp:ListItem Value="3">T.C</asp:ListItem>
    
     </asp:DropDownList>
        </u>
    </td>        
    <td style="width:3%;">&nbsp;</td>
  <td style="width: 13%; height: 27px; font-size:8pt;" align="right">Sex</td>
	<td style="width: 18%; height: 27px;" align="left">
	<asp:DropDownList SkinID="ddlPlain" ID="ddlSex" runat="server" Font-Size="8pt" 
            Width="104%" Height="18px" TabIndex="25" EnableTheming="True">
     <asp:ListItem></asp:ListItem>       
     <asp:ListItem Text="Male" Value="M"></asp:ListItem> 
     <asp:ListItem Text="Female" Value="F"></asp:ListItem>
     </asp:DropDownList>
    </td> 
    <td style="width:3%;">&nbsp;</td>
  <td style="width: 13%; height: 27px; font-size:8pt;" align="right">&nbsp;</td>
	<td style="width: 18%; height: 27px;" align="left">
        &nbsp;</td>      
</tr>
<tr>	
	<td style="width: 13%; height: 27px; font-size:8pt;" align="right"> Last Education</td>
	<td style="width: 18%; height: 27px;" align="left">
        <asp:TextBox ID="txtLastEducation" runat="server" Width="100%" TabIndex="26"></asp:TextBox>
    </td>        
    <td style="width:3%;">&nbsp;</td>
  <td style="width: 13%; height: 27px; font-size:8pt;" align="right">Board</td>
	<td style="width: 18%; height: 27px;" align="left">
	    <asp:TextBox ID="txtBoradExam" runat="server" Width="100%" TabIndex="27"></asp:TextBox>
    </td> 
    <td style="width:3%;">&nbsp;</td>
  <td style="width: 13%; height: 27px; font-size:8pt;" align="right">Passing Year</td>
	<td style="width: 18%; height: 27px;" align="left">
        <asp:TextBox ID="txtPassinYear" runat="server" TabIndex="28"></asp:TextBox>
    </td>      
</tr>
<tr>	
	<td style="width: 13%; height: 27px; font-size:8pt;" align="right"> Note</td>
	<td style="height: 27px;" align="left" colspan="5">
        <asp:TextBox ID="txtNote" runat="server" Width="100%" TabIndex="29"></asp:TextBox>
    </td>        
  <td style="width: 13%; height: 27px; font-size:8pt;" align="right">Grade</td>
	<td style="width: 18%; height: 27px;" align="left">
        <asp:TextBox ID="txtResult" runat="server" TabIndex="30"></asp:TextBox>
    </td>      
</tr>
<tr>
<td colspan="8"><asp:Label ID="lblTranStatus" runat="server" Font-Size="8pt" Text="" Visible="false"></asp:Label></td>
</tr>
</table>

    <asp:GridView runat="server" AllowPaging="True" AllowSorting="True" 
        AutoGenerateColumns="False" CellPadding="2" PageSize="15" BackColor="White" 
        BorderColor="LightGray" BorderWidth="1px" BorderStyle="Solid" CssClass="mGrid" 
        Font-Size="8pt" ForeColor="#333333" Width="100%" ID="dgStd" 
        OnSelectedIndexChanged="HistoryGridView_SelectedIndexChanged" 
        OnPageIndexChanging="HistoryGridView_PageIndexChanging" Visible="False">
<AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
<Columns>
<asp:CommandField ShowSelectButton="True">
<ItemStyle HorizontalAlign="Center" ForeColor="Blue" Height="25px" Width="40px"></ItemStyle>
</asp:CommandField>
<asp:BoundField DataField="Id" HeaderText="Student Id" SortExpression="Id"></asp:BoundField>
<asp:BoundField DataField="Name" HeaderText="Student Name" SortExpression="Name"></asp:BoundField>
<asp:BoundField DataField="Father" HeaderText="Father&#39;s Name" 
        SortExpression="Father"></asp:BoundField>
<asp:BoundField DataField="Mother" HeaderText="Mother&#39;s Name" 
        SortExpression="Mother"></asp:BoundField>
<asp:BoundField DataField="CLASS NAME" HeaderText="CLASS NAME" 
        SortExpression="CLASS NAME"></asp:BoundField>
<asp:BoundField DataField="STUDENT ROLL" HeaderText="STUDENT ROLL" 
        SortExpression="STUDENT ROLL"></asp:BoundField>
</Columns>

<HeaderStyle BackColor="Silver" Font-Bold="True" ForeColor="White"></HeaderStyle>

<PagerStyle HorizontalAlign="Center" CssClass="pgr"></PagerStyle>

<RowStyle BackColor="White"></RowStyle>
</asp:GridView>

<br />

<img alt="" height="1px" src="img/box_bottom_filet.gif" width="100%" />

<%--<asp:UpdatePanel ID="TabUpdatePanel" runat="server" UpdateMode="Conditional">
<ContentTemplate>--%>
<div style=" width:100%;">
<cc:TabContainer ID="StdTabContainer" runat="server" Width="100%" 
        ActiveTabIndex="0" TabIndex="148">
<%--<cc:TabPanel ID="tab" runat="server" HeaderText="Parents' Information">                
<ContentTemplate><table style="width:100%; font-size:8pt; padding-right:10px;"><tr><td style="width: 13%; height: 27px; font-size:8pt;" align="left">Father Name</td><td style="width: 18%; height: 27px;" align="left"><asp:TextBox SkinID="tbPlain" ID="txtFthName"  CssClass="tbc"
            runat="server" Width="100%" TabIndex="15" 
            Font-Size="8pt" ></asp:TextBox></td><td style="width:3%;"><asp:Label ID="Label9" runat="server" Font-Bold="True" ForeColor="Red" Text="*"></asp:Label></td><td style="width: 13%; height: 27px; font-size:8pt;" align="left">Highest Degree</td><td style="width: 18%; height: 27px;" align="left"><asp:TextBox SkinID="tbPlain" ID="txtFthEdu"  CssClass="tbc" TabIndex="16"
            runat="server" Width="100%"
            Font-Size="8pt" MaxLength="20"></asp:TextBox></td><td style="width:3%;"></td><td style="width: 13%; height: 27px; font-size:8pt;" align="left">Profession</td><td style="width: 18%; height: 27px;" align="left"><asp:DropDownList SkinID="ddlPlain" ID="ddlFthOccup" runat="server" 
            Font-Size="8pt" Width="104%" Height="18px" TabIndex="17" ><asp:ListItem></asp:ListItem><asp:ListItem Text="Engineer" Value="E"></asp:ListItem><asp:ListItem Text="Doctor" Value="D"></asp:ListItem><asp:ListItem Text="Lawyer" Value="L"></asp:ListItem><asp:ListItem Text="Private Service" Value="P"></asp:ListItem><asp:ListItem Text="Govt.Service" Value="G"></asp:ListItem><asp:ListItem Text="Teaching" Value="T"></asp:ListItem><asp:ListItem Text="Business" Value="B"></asp:ListItem><asp:ListItem Text="Other's" Value="O"></asp:ListItem></asp:DropDownList></td></tr><tr><td style="width: 13%; height: 27px; font-size:8pt;" align="left">Organization</td><td style="width: 53%; height: 27px;" align="left" colspan="7"><asp:TextBox SkinID="tbPlain" ID="txtFthOrg"  CssClass="tbl"
            runat="server" Width="100%" 
            Font-Size="8pt" ></asp:TextBox></td></tr><tr><td style="width: 13%; height: 27px; font-size:8pt;" align="left">Telephone No</td><td style="width: 18%; height: 27px;" align="left"><asp:TextBox SkinID="tbPlain" ID="txtFthTel"  CssClass="tbc"
            runat="server" Width="100%" TabIndex="15" 
            Font-Size="8pt" MaxLength="50"></asp:TextBox></td><td style="width:3%;"></td><td style="width: 13%; height: 27px; font-size:8pt;" align="left">Other Activities</td><td style="width: 18%; height: 27px;" align="left" colspan="4"><asp:TextBox SkinID="tbPlain" ID="txtFthOthAct"  CssClass="tbc" TabIndex="16"
            runat="server" Width="100%"
            Font-Size="8pt" ></asp:TextBox></td></tr><tr><td style="width: 13%; height: 27px; font-size:8pt;" align="left">Mother Name</td><td style="width: 18%; height: 27px;" align="left"><asp:TextBox SkinID="tbPlain" ID="txtMthName"  CssClass="tbc"
            runat="server" Width="100%" TabIndex="15" 
            Font-Size="8pt"></asp:TextBox></td><td style="width:3%;"><asp:Label ID="Label10" runat="server" Font-Bold="True" ForeColor="Red" 
            Text="*"></asp:Label></td><td style="width: 13%; height: 27px; font-size:8pt;" align="left">Highest Degree</td><td style="width: 18%; height: 27px;" align="left"><asp:TextBox SkinID="tbPlain" ID="txtMthEdu"  CssClass="tbc" TabIndex="16"
            runat="server" Width="100%"
            Font-Size="8pt"></asp:TextBox></td><td style="width:3%;"></td><td style="width: 13%; height: 27px; font-size:8pt;" align="left">Occupation</td><td style="width: 18%; height: 27px;" align="left"><asp:DropDownList SkinID="ddlPlain" ID="ddlMthOccup" runat="server" 
            Font-Size="8pt" Width="104%" Height="18px" TabIndex="17" ><asp:ListItem></asp:ListItem><asp:ListItem Text="Engineer" Value="E"></asp:ListItem><asp:ListItem Text="Doctor" Value="D"></asp:ListItem><asp:ListItem Text="Lawyer" Value="L"></asp:ListItem><asp:ListItem Text="Private Service" Value="P"></asp:ListItem><asp:ListItem Text="Govt.Service" Value="G"></asp:ListItem><asp:ListItem Text="Teaching" Value="T"></asp:ListItem><asp:ListItem Text="Business" Value="B"></asp:ListItem><asp:ListItem Text="Other's" Value="O"></asp:ListItem></asp:DropDownList></td></tr><tr><td style="width: 13%; height: 27px; font-size:8pt;" align="left">Organization</td><td style="width: 53%; height: 27px;" align="left" colspan="7"><asp:TextBox SkinID="tbPlain" ID="txtMthOrg"  CssClass="tbl"
            runat="server" Width="100%" 
            Font-Size="8pt" ></asp:TextBox></td></tr><tr><td style="width: 13%; height: 27px; font-size:8pt;" align="left">Telephone No</td><td style="width: 18%; height: 27px;" align="left"><asp:TextBox SkinID="tbPlain" ID="txtMthTel"  CssClass="tbc"
            runat="server" Width="100%" TabIndex="15" 
            Font-Size="8pt" ></asp:TextBox></td><td style="width:3%;"></td><td style="width: 13%; height: 27px; font-size:8pt;" align="left">Other Activities</td><td style="width: 18%; height: 27px;" align="left" colspan="4"><asp:TextBox SkinID="tbPlain" ID="txtMthOthAct"  CssClass="tbc" TabIndex="16"
            runat="server" Width="100%"
            Font-Size="8pt"></asp:TextBox></td></tr></table></ContentTemplate>          
             </cc:TabPanel>--%>
             
<%--<cc:TabPanel ID="TabPanel1" runat="server" HeaderText="Other Family Members">                    
<ContentTemplate><table style="width:100%; font-size:x-small;"><tr><td style="font-size:x-small;" align="left"><asp:GridView CssClass="mGrid" ID="dgEmpFam" runat="server" AutoGenerateColumns="False" 
        AllowPaging="True" Width="100%" BackColor="White" BorderWidth="1px" BorderStyle="Solid"
        CellPadding="2" BorderColor="LightGray" Font-Size="8pt" 
        AllowSorting="True" PageSize="20" onrowcancelingedit="dgEmpFam_RowCancelingEdit" 
        onrowdeleting="dgEmpFam_RowDeleting" onrowediting="dgEmpFam_RowEditing" 
        onrowupdating="dgEmpFam_RowUpdating" 
        onrowdatabound="dgEmpFam_RowDataBound" onrowcommand="dgEmpFam_RowCommand"><HeaderStyle Font-Size="8pt" Font-Names="Arial" Font-Bold="True" BackColor="LightGray" 
        HorizontalAlign="Center" ForeColor="Black"/><Columns><asp:TemplateField><ItemTemplate><asp:LinkButton ID="lbEdit" runat="server" CausesValidation="False" CommandName="Edit" Text="Edit" ></asp:LinkButton><asp:LinkButton ID="lbDelete" runat="server" CausesValidation="False" CommandName="Delete" Text="Remove"
  onclientclick="javascript:return window.confirm('are u really want to delete  these data')"></asp:LinkButton><asp:LinkButton ID="btnAddDet" runat="server" CommandName="New" Text="New"></asp:LinkButton></ItemTemplate><EditItemTemplate><asp:LinkButton ID="lbUpdate" runat="server" CausesValidation="True" CommandName="Update" Text="Update" ></asp:LinkButton><asp:LinkButton ID="lbCancel" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel"></asp:LinkButton></EditItemTemplate><FooterTemplate><asp:LinkButton ID="lbInsert" runat="server" CommandName="Insert" Text="Add" ></asp:LinkButton><asp:LinkButton ID="lbCancel" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel"></asp:LinkButton></FooterTemplate><ItemStyle Font-Size="8pt" HorizontalAlign="Center" Width="130px" /></asp:TemplateField><asp:TemplateField HeaderText="Relative Name"><ItemTemplate><asp:Label ID="lblRelName" runat="server" Text='<%#Eval("rel_name") %>' Width="200px"  Font-Size="8pt"></asp:Label></ItemTemplate><EditItemTemplate><asp:TextBox SkinID="tbGray" ID="txtRelName" 
            runat="server" Width="200px" TabIndex="71" Font-Size="8pt" MaxLength="25"></asp:TextBox></EditItemTemplate><FooterTemplate><asp:TextBox SkinID="tbGray" ID="txtRelName" 
            runat="server" Width="200px" TabIndex="71" Font-Size="8pt" MaxLength="25"></asp:TextBox></FooterTemplate><ItemStyle HorizontalAlign="Left" Width="200px" Height="25px" /></asp:TemplateField><asp:TemplateField HeaderText="Relation"><ItemTemplate><asp:Label ID="lblRelation" runat="server" Text='<%#Eval("relation") %>' Width="100px"></asp:Label></ItemTemplate><EditItemTemplate><asp:DropDownList SkinID="ddlPlain" ID="ddlRelation" runat="server" Font-Size="8pt" Width="100px" Height="18px" TabIndex="72" ><asp:ListItem></asp:ListItem><asp:ListItem Text="Brother" Value="B"></asp:ListItem><asp:ListItem Text="Sister" Value="S"></asp:ListItem></asp:DropDownList></EditItemTemplate><FooterTemplate><asp:DropDownList SkinID="ddlPlain" ID="ddlRelation" runat="server" Font-Size="8pt" Width="100px" Height="18px" TabIndex="72" ><asp:ListItem></asp:ListItem><asp:ListItem Text="Brother" Value="B"></asp:ListItem><asp:ListItem Text="Sister" Value="S"></asp:ListItem></asp:DropDownList></FooterTemplate><ItemStyle HorizontalAlign="Center" Width="100px" /></asp:TemplateField><asp:TemplateField HeaderText="Birth Date"><ItemTemplate><asp:Label ID="lblBirthDt" runat="server" Text='<%#Eval("birth_dt") %>'  Font-Size="8pt"></asp:Label></ItemTemplate><EditItemTemplate><asp:TextBox SkinID="tbGray" ID="txtBirthDt" 
            runat="server" Width="100px" TabIndex="73" Font-Size="8pt" MaxLength="11"></asp:TextBox></EditItemTemplate><FooterTemplate><asp:TextBox SkinID="tbGray" ID="txtBirthDt" 
            runat="server" Width="100px" TabIndex="73" Font-Size="8pt" MaxLength="11"></asp:TextBox></FooterTemplate><ItemStyle HorizontalAlign="Center" Width="100px" /></asp:TemplateField><asp:TemplateField HeaderText="Age"><ItemTemplate><asp:Label ID="lblAge" runat="server" Text='<%#Eval("age") %>' Width="80px"></asp:Label></ItemTemplate><EditItemTemplate><asp:TextBox SkinID="tbGray" ID="txtAge" 
            runat="server" Width="80px" TabIndex="74" Font-Size="8pt" MaxLength="2"></asp:TextBox></EditItemTemplate><FooterTemplate><asp:TextBox SkinID="tbGray" ID="txtAge" 
            runat="server" Width="80px" TabIndex="74" Font-Size="8pt" MaxLength="2"></asp:TextBox></FooterTemplate><ItemStyle HorizontalAlign="Center" Width="80px" /></asp:TemplateField><asp:TemplateField HeaderText="Occupation"><ItemTemplate><asp:Label ID="lblOccupation" runat="server" Text='<%#Eval("occupation") %>'></asp:Label></ItemTemplate><EditItemTemplate><asp:TextBox SkinID="tbGray" ID="txtOccupation" 
            runat="server" Width="150px" TabIndex="75" Font-Size="8pt" MaxLength="40"></asp:TextBox></EditItemTemplate><FooterTemplate><asp:TextBox SkinID="tbGray" ID="txtOccupation" 
            runat="server" Width="150px" TabIndex="75" Font-Size="8pt" MaxLength="40"></asp:TextBox></FooterTemplate><ItemStyle HorizontalAlign="Center" Width="150px" /></asp:TemplateField></Columns><AlternatingRowStyle CssClass="alt" /><PagerStyle CssClass="pgr" /></asp:GridView></td></tr></table></ContentTemplate></cc:TabPanel>--%>
<%--<cc:TabPanel ID="TabPanel2" runat="server" HeaderText="Guardian/Receivers Information">                    
<ContentTemplate><table style="width:100%;"><tr><td colspan="8" align="left" style="font-size:8pt;">Contact Person Information:</td></tr><tr><td style="width: 13%; height: 27px; font-size:8pt;" align="left">Name</td><td style="width: 53%; height: 27px;" align="left" colspan="7"><asp:TextBox SkinID="tbPlain" ID="txtContPerson"  CssClass="tbl"
            runat="server" Width="100%" 
            Font-Size="8pt"></asp:TextBox></td></tr><tr><td style="width: 13%; height: 27px; font-size:8pt;" align="left">Address</td><td style="width: 53%; height: 27px;" align="left" colspan="7"><asp:TextBox SkinID="tbPlain" ID="txtContAddress"  CssClass="tbl"
            runat="server" Width="100%" 
            Font-Size="8pt" ></asp:TextBox></td></tr><tr><td style="width: 13%; height: 27px; font-size:8pt;" align="left">Relation</td><td style="width: 18%; height: 27px;" align="left"><asp:DropDownList SkinID="ddlPlain" ID="ddlContRelate" runat="server" 
            Font-Size="8pt" Width="104%" Height="18px" TabIndex="11" ><asp:ListItem></asp:ListItem><asp:ListItem Text="Father" Value="F"></asp:ListItem><asp:ListItem Text="Mother" Value="M"></asp:ListItem><asp:ListItem Text="Uncle" Value="U"></asp:ListItem><asp:ListItem Text="Aunt" Value="A"></asp:ListItem></asp:DropDownList></td><td style="width:3%;"></td><td style="width: 13%; height: 27px; font-size:8pt;" align="left">Phone No</td><td style="width: 18%; height: 27px;" align="left"><asp:TextBox SkinID="tbPlain" ID="txtContPhone"  CssClass="tbl"
            runat="server" Width="100%" 
            Font-Size="8pt"></asp:TextBox></td><td style="width:3%;"></td><td style="width: 13%; height: 27px; font-size:8pt;" align="left">Mobile No</td><td style="width: 18%; height: 27px;" align="left"><asp:TextBox SkinID="tbPlain" ID="txtContMobile"  CssClass="tbl"
            runat="server" Width="100%" 
            Font-Size="8pt" ></asp:TextBox></td></tr></table><table style="width:100%; font-size:x-small;"><tr><td colspan="5" align="left" style="font-size:8pt;">Name of Persons Receiving the Child:</td></tr><tr><td style="width: 13%; height: 27px; font-size:8pt;" align="left">Guardian 1 </td><td style="width: 35%; height: 27px;" align="left"><asp:TextBox SkinID="tbPlain" ID="txtChildRcv1"  CssClass="tbl"
            runat="server" Width="100%"  
            Font-Size="8pt"></asp:TextBox></td><td style="width:3%;"></td><td style="width: 13%; height: 27px; font-size:8pt;" align="left">Relation </td><td style="width: 18%; height: 27px;" align="left"><asp:DropDownList SkinID="ddlPlain" ID="ddlRelRcv1" runat="server" Font-Size="8pt" Width="100%" Height="18px" TabIndex="66" ><asp:ListItem></asp:ListItem><asp:ListItem Text="Uncle" Value="U"></asp:ListItem><asp:ListItem Text="Caretaker" Value="C"></asp:ListItem><asp:ListItem Text="Servant/Maid" Value="S"></asp:ListItem></asp:DropDownList></td></tr><tr><td style="width: 13%; height: 27px; font-size:8pt;" align="left">Guardian 2 </td><td style="width: 35%; height: 27px;" align="left"><asp:TextBox SkinID="tbPlain" ID="txtChildRcv2"  CssClass="tbl"
            runat="server" Width="100%"  
            Font-Size="8pt" ></asp:TextBox></td><td style="width:3%;"></td><td style="width: 13%; height: 27px; font-size:8pt;" align="left">Relation </td><td style="width: 18%; height: 27px;" align="left"><asp:DropDownList SkinID="ddlPlain" ID="ddlRelRcv2" runat="server" Font-Size="8pt" Width="100%" Height="18px" TabIndex="66" ><asp:ListItem></asp:ListItem><asp:ListItem Text="Uncle" Value="U"></asp:ListItem><asp:ListItem Text="Caretaker" Value="C"></asp:ListItem><asp:ListItem Text="Servant/Maid" Value="S"></asp:ListItem></asp:DropDownList></td></tr><tr><td style="width: 13%; height: 27px; font-size:8pt;" align="left">Guardian 3 </td><td style="width: 35%; height: 27px;" align="left"><asp:TextBox SkinID="tbPlain" ID="txtChildRcv3"  CssClass="tbl"
            runat="server" Width="100%"  
            Font-Size="8pt" ></asp:TextBox></td><td style="width:3%;"></td><td style="width: 13%; height: 27px; font-size:8pt;" align="left">Relation </td><td style="width: 18%; height: 27px;" align="left"><asp:DropDownList SkinID="ddlPlain" ID="ddlRelRcv3" runat="server" Font-Size="8pt" Width="100%" Height="18px" TabIndex="66" ><asp:ListItem></asp:ListItem><asp:ListItem Text="Uncle" Value="U"></asp:ListItem><asp:ListItem Text="Caretaker" Value="C"></asp:ListItem><asp:ListItem Text="Servant/Maid" Value="S"></asp:ListItem></asp:DropDownList></td></tr><tr><td style="width: 13%; height: 27px; font-size:8pt;" align="left">Guardian 4 </td><td style="width: 35%; height: 27px;" align="left"><asp:TextBox SkinID="tbPlain" ID="txtChildRcv4"  CssClass="tbl"
            runat="server" Width="100%"  
            Font-Size="8pt" ></asp:TextBox></td><td style="width:3%;"></td><td style="width: 13%; height: 27px; font-size:8pt;" align="left">Relation </td><td style="width: 18%; height: 27px;" align="left"><asp:DropDownList SkinID="ddlPlain" ID="ddlRelRcv4" runat="server" Font-Size="8pt" Width="100%" Height="18px" TabIndex="66" ><asp:ListItem></asp:ListItem><asp:ListItem Text="Uncle" Value="U"></asp:ListItem><asp:ListItem Text="Caretaker" Value="C"></asp:ListItem><asp:ListItem Text="Servant/Maid" Value="S"></asp:ListItem></asp:DropDownList></td></tr></table></ContentTemplate></cc:TabPanel>--%>

<%--<cc:TabPanel ID="TabPanel3" runat="server" HeaderText="Previous School">                    
<ContentTemplate><table style="width:100%;"><tr><td style="width: 13%; height: 27px; font-size:8pt;" align="left">School Name</td><td style="width: 53%; height: 27px;" align="left" colspan="7"><asp:TextBox SkinID="tbPlain" ID="txtPrevSch"  CssClass="tbl" Enabled="true"
            runat="server" Width="100%"  AutoPostBack="False" 
            Font-Size="8pt" ></asp:TextBox></td></tr><tr><td style="width: 13%; height: 27px; font-size:8pt;" align="left">School Address</td><td style="width: 53%; height: 27px;" align="left" colspan="7"><asp:TextBox SkinID="tbPlain" ID="txtPrevAdd"  CssClass="tbl" Enabled="true"
            runat="server" Width="100%"  AutoPostBack="False" 
            Font-Size="8pt" ></asp:TextBox></td></tr><tr><td style="width: 13%; height: 27px; font-size:8pt;" align="left">Last Class</td><td style="width: 18%; height: 27px;" align="left"><asp:TextBox SkinID="tbPlain" ID="txtLastClass"  CssClass="tbc" Enabled="true"
            runat="server" Width="100%" TabIndex="15" AutoPostBack="False" 
            Font-Size="8pt" ></asp:TextBox></td><td style="width:3%;"></td><td style="width: 13%; height: 27px; font-size:8pt;" align="left">Class Year</td><td style="width: 18%; height: 27px;" align="left"><asp:TextBox SkinID="tbPlain" ID="txtClassYear"  CssClass="tbc" TabIndex="16"
            runat="server" Width="100%" AutoPostBack="False"  Enabled="true"
            Font-Size="8pt" ></asp:TextBox></td><td style="width:3%;"></td><td style="width: 13%; height: 27px; font-size:8pt;" align="left">Position</td><td style="width: 18%; height: 27px;" align="left"><asp:TextBox SkinID="tbPlain" ID="txtClassPos"  CssClass="tbc" TabIndex="16"
            runat="server" Width="100%" AutoPostBack="False"  Enabled="true"
            Font-Size="8pt" ></asp:TextBox></td></tr><tr><td style="width: 13%; height: 27px; font-size:8pt;" align="left">Reason for Leave</td><td style="width: 53%; height: 27px;" align="left" colspan="7"><asp:TextBox SkinID="tbPlain" ID="txtReasonLeave"  CssClass="tbl" Enabled="true"
            runat="server" Width="100%"  AutoPostBack="False" 
            Font-Size="8pt" ></asp:TextBox></td></tr><tr><td style="width: 13%; height: 27px; font-size:8pt;" align="left">Any Physical Disorder</td><td style="width: 53%; height: 27px;" align="left" colspan="7"><asp:TextBox SkinID="tbPlain" ID="txtPhysicProb"  CssClass="tbl" Enabled="true"
            runat="server" Width="100%"  AutoPostBack="False" 
            Font-Size="8pt" ></asp:TextBox></td></tr><tr><td style="width: 13%; height: 27px; font-size:8pt;" align="left">Allergic to </td><td style="width: 53%; height: 27px;" align="left" colspan="7"><asp:TextBox SkinID="tbPlain" ID="txtAllergic"  CssClass="tbl" Enabled="true"
            runat="server" Width="100%"  AutoPostBack="False" 
            Font-Size="8pt" ></asp:TextBox></td></tr></table></ContentTemplate></cc:TabPanel>--%>

<%--<cc:TabPanel ID="TabPanel4" runat="server" HeaderText="Passport/Visa">                    
    <HeaderTemplate>Admission Fee Setting</HeaderTemplate><ContentTemplate><table style="width:100%;"><tr><td style="width: 13%; height: 27px; font-size:8pt;" align="left">&nbsp;</td><td style="width: 18%; height: 27px;" align="left">&nbsp;</td><td style="width:3%;"></td><td style="width: 13%; height: 27px; font-size:8pt;" align="left">&nbsp;</td><td style="width: 18%; height: 27px;" align="left">&nbsp;</td><td style="width:3%;"></td><td style="width: 13%; height: 27px; font-size:8pt;" align="left">&nbsp;</td><td style="width: 18%; height: 27px;" align="left">&nbsp;</td></tr><tr><td align="left" style="width: 13%; height: 27px; font-size:8pt;"><asp:Label ID="lblInstallmentAmt" runat="server" Font-Size="8pt">Total Amount</asp:Label></td><td align="left" style="width: 18%; height: 27px;"><asp:TextBox ID="txtTotalAmt" runat="server" style="text-align:center;" 
                Width="100%"></asp:TextBox></td><td style="width:3%;">&nbsp;</td><td align="left" style="width: 13%; height: 27px; font-size:8pt;">Pay Date</td><td align="left" style="width: 18%; height: 27px;"><asp:TextBox ID="txtDate" runat="server" style="text-align:center;" Width="95%"></asp:TextBox><cc:CalendarExtender ID="txtDate_CalendarExtender" runat="server" 
                Format="dd/MM/yyyy" TargetControlID="txtDate" Enabled="True" /></td><td style="width:3%;">&nbsp;</td><td align="left" style="width: 13%; height: 27px; font-size:8pt;">Month Inerval</td><td align="left" style="width: 18%; height: 27px;"><asp:TextBox ID="txtMonthInterval" runat="server" style="text-align:center;" 
                Width="100%"></asp:TextBox></td></tr><tr><td align="left" style="width: 13%; height: 27px; font-size:8pt;"><asp:Label ID="Label19" runat="server" Font-Size="8pt">Installment Quantity</asp:Label></td><td align="left" style="width: 18%; height: 27px;"><asp:TextBox ID="txtInstallQnty" runat="server" 
                  style="text-align:center;" 
                Width="95%"></asp:TextBox></td><td style="width:3%;">&nbsp;</td><td align="left" style="width: 13%; height: 27px; font-size:8pt;"><asp:Label ID="Label20" runat="server" Font-Size="8pt">Admission Fee</asp:Label></td><td align="left" style="width: 18%; height: 27px;"><asp:TextBox ID="txtAdmissionFee" runat="server" style="text-align:center;" 
                Width="95%"></asp:TextBox></td><td style="width:3%;">&nbsp;</td><td align="center" rowspan="2" style="width: 13%; font-size:8pt;"><asp:ImageButton ID="btnGenerateRow" runat="server" ImageUrl="~/img/Add.png" 
                onclick="GenerateRows" Width="25%" /></td><td align="left" style="width: 18%; height: 27px;"><asp:TextBox ID="txtInstallmentName" runat="server" 
                  style="text-align:center;" 
                Width="95%" Enabled="False" Visible="False"></asp:TextBox></td></tr><tr><td align="left" style="width: 13%; height: 27px; font-size:8pt;">&nbsp;</td><td align="left" style="width: 18%; height: 27px;">&nbsp;</td><td style="width:3%;">&nbsp;</td><td align="left" style="width: 13%; height: 27px; font-size:8pt;">&nbsp;</td><td align="left" style="width: 18%; height: 27px;">&nbsp;</td><td style="width:3%;">&nbsp;</td><td align="left" style="width: 18%; height: 27px;">&nbsp;</td></tr><tr><td align="left" style="width: 13%; height: 27px; font-size:8pt;">&nbsp;</td><td align="left" style="width: 18%; height: 27px;">&nbsp;</td><td style="width:3%;">&nbsp;</td><td align="left" style="width: 13%; height: 27px; font-size:8pt;">&nbsp;</td><td align="left" style="width: 18%; height: 27px;">&nbsp;</td><td style="width:3%;">&nbsp;</td><td align="left" style="width: 13%; height: 27px; font-size:8pt;">&nbsp;</td><td align="left" style="width: 18%; height: 27px;">&nbsp;</td></tr><tr><td align="left" colspan="8" style="height: 27px; font-size:8pt;"><asp:GridView ID="dgInstallment" runat="server" BackColor="White" BorderColor="LightGray" 
                BorderStyle="Solid" BorderWidth="1px" CellPadding="2" CssClass="mGrid" 
                Font-Size="8pt" ForeColor="#333333" PageSize="20" 
                style="text-align:center;" Width="100%"><AlternatingRowStyle CssClass="alt" /><PagerStyle CssClass="pgr" /><RowStyle BackColor="White" /><HeaderStyle BackColor="Silver" Font-Bold="True" ForeColor="White" /></asp:GridView></td></tr><tr><td align="left" style="width: 13%; height: 27px; font-size:8pt;">&nbsp;</td><td align="left" style="width: 18%; height: 27px;">&nbsp;</td><td style="width:3%;">&nbsp;</td><td align="left" style="width: 13%; height: 27px; font-size:8pt;">&nbsp;</td><td align="left" style="width: 18%; height: 27px;">&nbsp;</td><td style="width:3%;">&nbsp;</td><td align="left" style="width: 13%; height: 27px; font-size:8pt;">&nbsp;</td><td align="left" style="width: 18%; height: 27px;">&nbsp;</td></tr><tr><td align="left" style="width: 13%; height: 27px; font-size:8pt;">&nbsp;</td><td align="left" style="width: 18%; height: 27px;">&nbsp;</td><td style="width:3%;">&nbsp;</td><td align="left" style="width: 13%; height: 27px; font-size:8pt;">&nbsp;</td><td align="left" style="width: 18%; height: 27px;">&nbsp;</td><td style="width:3%;">&nbsp;</td><td align="left" style="width: 13%; height: 27px; font-size:8pt;">&nbsp;</td><td align="left" style="width: 18%; height: 27px;">&nbsp;</td></tr></table></ContentTemplate></cc:TabPanel>--%>

<%--<cc:TabPanel ID="TabPanel5" runat="server" HeaderText="Other Photographs">                    
<ContentTemplate><table style="width:100%;"><tr><td style="width:47%" align="center" colspan="3">Student Current Photograph: <br /><table style="width:100%;"><tr><td align="center"><asp:FileUpload ID="imgUploadStdCur" runat="server" Visible="true" Size="20%" Height="20px" Font-Size="8pt" onchange="javascript:SetImageStdCur();"/><asp:Button ID="lbImgUploadStdCur" runat="server" Text="Upload" Font-Size="8pt" style="display:none;"
                    Width="50px" Height="20px" onclick="lbImgUploadStdCur_Click"></asp:Button></td></tr><tr><td style="height: 162px; vertical-align:top;" align="center"><asp:Image ID="imgStdCur" runat="server" Height="165px" Width="145px" BorderStyle="Solid" BackColor="#EFF3FB" BorderWidth="1px"  /></td></tr></table></td></tr><tr><td style="width:47%" align="center">Father Photograph: <br /><table style="width:100%;"><tr><td align="center"><asp:FileUpload ID="imgUploadFth" runat="server" Visible="true" Size="20%" Height="20px" Font-Size="8pt" onchange="javascript:SetImageFth();"/><asp:Button ID="lbImgUploadFth" runat="server" Text="Upload" Font-Size="8pt"  style="display:none;"
                    Width="50px" Height="20px" onclick="lbImgUploadFth_Click"></asp:Button></td></tr><tr><td style="height: 162px; vertical-align:top;" align="center"><asp:Image ID="imgFth" runat="server" Height="165px" Width="145px" BorderStyle="Solid" BackColor="#EFF3FB" BorderWidth="1px"  /></td></tr></table></td><td style="width:6%;"></td><td style="width:47%" align="center">Mother Photograph: <br /><table style="width:100%;"><tr><td align="center"><asp:FileUpload ID="imgUploadMth" runat="server" Visible="true" Size="20%" Height="20px" Font-Size="8pt" onchange="javascript:SetImageMth();"/><asp:Button ID="lbImgUploadMth" runat="server" Text="Upload" Font-Size="8pt"  style="display:none;"
                    Width="50px" Height="20px" onclick="lbImgUploadMth_Click"></asp:Button></td></tr><tr><td style="height: 162px; vertical-align:top;" align="center"><asp:Image ID="imgMth" runat="server" Height="165px" Width="145px" BorderStyle="Solid" BackColor="#EFF3FB" BorderWidth="1px"  /></td></tr></table></td></tr><tr><td style="width:47%" align="center">Guardian-1 Photograph: <br /><table style="width:100%;"><tr><td align="center"><asp:FileUpload ID="imgUploadGuard1" runat="server" Visible="true" Size="20%" Height="20px" Font-Size="8pt" onchange="javascript:SetImageGuard1();"/><asp:Button ID="lbImgUploadGuard1" runat="server" Text="Upload" Font-Size="8pt"  style="display:none;"
                    Width="50px" Height="20px" onclick="lbImgUploadGuard1_Click"></asp:Button></td></tr><tr><td style="height: 162px; vertical-align:top;" align="center"><asp:Image ID="imgGuard1" runat="server" Height="165px" Width="145px" BorderStyle="Solid" BackColor="#EFF3FB" BorderWidth="1px"  /></td></tr></table></td><td style="width:6%;"></td><td style="width:47%" align="center">Guardian-2 Photograph: <br /><table style="width:100%;"><tr><td align="center"><asp:FileUpload ID="imgUploadGuard2" runat="server" Visible="true" Size="20%" Height="20px" Font-Size="8pt" onchange="javascript:SetImageGuard2();"/><asp:Button ID="lbImgUploadGuard2" runat="server" Text="Upload" Font-Size="8pt"  style="display:none;"
                    Width="50px" Height="20px" onclick="lbImgUploadGuard2_Click"></asp:Button></td></tr><tr><td style="height: 162px; vertical-align:top;" align="center"><asp:Image ID="imgGuard2" runat="server" Height="165px" Width="145px" BorderStyle="Solid" BackColor="#EFF3FB" BorderWidth="1px"  /></td></tr></table></td></tr><tr><td style="width:47%" align="center">Guardian-3 Photograph: <br /><table style="width:100%;"><tr><td align="center"><asp:FileUpload ID="imgUploadGuard3" runat="server" Visible="true" Size="20%" Height="20px" Font-Size="8pt" onchange="javascript:SetImageGuard3();"/><asp:Button ID="lbImgUploadGuard3" runat="server" Text="Upload" Font-Size="8pt"  style="display:none;"
                    Width="50px" Height="20px" onclick="lbImgUploadGuard3_Click"></asp:Button></td></tr><tr><td style="height: 162px; vertical-align:top;" align="center"><asp:Image ID="imgGuard3" runat="server" Height="165px" Width="145px" BorderStyle="Solid" BackColor="#EFF3FB" BorderWidth="1px"  /></td></tr></table></td><td style="width:6%;"></td><td style="width:47%" align="center">Guardian-4 Photograph: <br /><table style="width:100%;"><tr><td align="center"><asp:FileUpload ID="imgUploadGuard4" runat="server" Visible="true" Size="20%" Height="20px" Font-Size="8pt" onchange="javascript:SetImageGuard4();"/><asp:Button ID="lbImgUploadGuard4" runat="server" Text="Upload" Font-Size="8pt"  style="display:none;"
                    Width="50px" Height="20px" onclick="lbImgUploadGuard4_Click"></asp:Button></td></tr><tr><td style="height: 162px; vertical-align:top;" align="center"><asp:Image ID="imgGuard4" runat="server" Height="165px" Width="145px" BorderStyle="Solid" BackColor="#EFF3FB" BorderWidth="1px"  /></td></tr></table></td></tr></table></ContentTemplate></cc:TabPanel>--%>

<cc:TabPanel ID="TabPanel6" runat="server" HeaderText="Current Studentship">                    
<ContentTemplate><asp:Panel ID="pnlCurSt" runat="server" Width="100%" Visible="False"><table style="width:100%;"><tr><td colspan="2" align="left">Current</td></tr><tr><td style="width: 13%; height: 27px; font-size:10pt;" align="left">&#160;</td><td style="width: 27%; height: 27px;" align="left">&nbsp;</td></tr></table></asp:Panel><asp:Panel ID="pnlCurStEdit" runat="server" Width="100%"><table style="width:100%;"><tr><td align="right" style="width: 13%; height: 27px; font-size:10pt;">
    Course Name</td><td align="left" style="width: 20%; height: 27px;">
        <asp:DropDownList ID="ddlCourseName" runat="server" AutoPostBack="True" 
            Height="26px" OnSelectedIndexChanged="ddlCourseName_SelectedIndexChanged" 
            Width="61%" TabIndex="31"> 
        </asp:DropDownList>
    </td><td style="width:2%;"></td><td align="right" style="width: 13%; height: 27px; font-size:10pt;"><asp:Label ID="Label19" runat="server" Text="Batch No."></asp:Label></td><td align="left" style="width: 27%; height: 27px;">
    <asp:TextBox ID="txtBatch" runat="server" Font-Size="10pt" Width="60%" TabIndex="32"></asp:TextBox>
    </td></tr><tr><td align="right" style="width: 13%; height: 27px; font-size:10pt;">
    Admission Date</td><td align="left" style="width: 20%; height: 27px;">
        <asp:TextBox ID="txtSheduleTime" runat="server" align="center" 
            placeholder="dd/MM/yyyy" Width="60%" TabIndex="33"></asp:TextBox>
        <cc:CalendarExtender ID="Calendarextender1" runat="server" Enabled="True" 
            Format="dd/MM/yyyy" TargetControlID="txtSheduleTime">
        </cc:CalendarExtender>
    </td><td style="width:2%;"></td><td align="right" style="width: 13%; height: 27px; font-size:10pt;"><asp:Label ID="Label20" runat="server" Text="Course Fee"></asp:Label></td><td align="left" style="width: 27%; height: 27px;">
    <asp:TextBox ID="txtCourseFee" runat="server" AutoPostBack="True" 
                Font-Size="10pt" Width="60%" TabIndex="34"></asp:TextBox>
                </td></tr><tr><td align="right" style="width: 13%; height: 27px; font-size:10pt;">
        <asp:Label ID="Label26" runat="server" Text="Course End Date"></asp:Label>
        </td><td align="left" style="width: 20%; height: 27px;">
            <asp:TextBox ID="txtEndSheduleTime" runat="server" align="center" 
                placeholder="dd/MM/yyyy" Width="60%" TabIndex="35"></asp:TextBox>
            <cc:CalendarExtender ID="txtEndSheduleTime_CalendarExtender" runat="server" 
                Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtEndSheduleTime">
            </cc:CalendarExtender>
        </td><td style="width:2%;"></td><td align="right" style="width: 13%; height: 27px; font-size:10pt;"><asp:Label ID="Label14" runat="server" Text="Discount"></asp:Label></td><td align="left" style="width: 27%; height: 27px;">
        <asp:TextBox ID="txtDiscountTaka" runat="server" Font-Size="10pt" Width="60%" 
                AutoPostBack="True" ontextchanged="txtDiscountTaka_TextChanged" TabIndex="36"></asp:TextBox></td></tr><tr><td align="right" style="width: 13%; height: 27px; font-size:10pt;">
        <asp:Label ID="Label27" runat="server" Text="Class Time"></asp:Label>
        </td><td align="left" style="width: 20%; height: 27px; font-weight: 700;" 
            valign="middle">
            <asp:TextBox ID="txtClassTime" runat="server" align="center" Font-Size="10pt" 
                placeholder="tt:ss" Width="40%" TabIndex="37"></asp:TextBox>
        </td><td style="width:2%;">&#160;</td><td align="right" style="width: 13%; height: 27px; font-size:10pt;"><asp:Label ID="Label15" runat="server" Text="Waiver"></asp:Label></td><td align="left" style="width: 27%; height: 27px;">
        <asp:TextBox ID="txtWaiver" runat="server" Font-Size="10pt" Width="60%" 
                AutoPostBack="True" ontextchanged="txtWaiver_TextChanged" TabIndex="38"></asp:TextBox><asp:Label ID="Label16" runat="server" Font-Bold="True" ForeColor="Red" 
                Text="%"></asp:Label></td></tr><tr><td align="right" style="width: 13%; height: 27px; font-size: 10pt;">
        &nbsp;</td><td align="left" style="width: 20%; height: 27px; ">
            <asp:RadioButtonList ID="rbStartAmPm" runat="server" AutoPostBack="True" 
                OnSelectedIndexChanged="rbStartAmPm_SelectedIndexChanged" 
                RepeatDirection="Horizontal" SelectedValue='<%# Bind("StartAmPm") %>' 
                Width="40%" TabIndex="39">
                <asp:ListItem>AM</asp:ListItem>
                <asp:ListItem>PM</asp:ListItem>
            </asp:RadioButtonList>
        </td><td style="width:2%; height: 27px;"></td><td align="right" 
            style="width: 13%; height: 27px; font-size:10pt;"><asp:Label ID="Label25" runat="server" Text="Total Amount"></asp:Label></td><td align="left" style="width: 27%; height: 27px;">
            <asp:TextBox ID="txtTotalAMount" runat="server" Enabled="False" 
                Font-Size="10pt" ReadOnly="True" Width="60%" TabIndex="40"></asp:TextBox></td></tr><tr><td align="right" style="width: 13%; height: 27px; font-size:10pt;">
        Addmission Year</td><td align="left" style="width: 20%; height: 27px;">
            <asp:TextBox ID="txtAddmissionYear" runat="server" Font-Size="10pt" Width="60%" TabIndex="41"></asp:TextBox>
        </td><td style="width:2%;"></td><td align="right" style="width: 13%; height: 27px; font-size:10pt;"><asp:Label ID="Label23" runat="server" Text="Pay Amount"></asp:Label></td><td align="left" style="width: 27%; height: 27px;"><asp:TextBox ID="txtPayAmount" runat="server" AutoPostBack="True" 
                Font-Size="10pt" ontextchanged="txtPayAmount_TextChanged" Width="60%" TabIndex="42"></asp:TextBox></td></tr><tr><td align="right" style="width: 13%; height: 27px; font-size:10pt;">
        Trainer Name</td><td align="left" style="width: 20%; height: 27px;">
            <asp:TextBox ID="txtTrainerName" runat="server" CssClass="tbc" Font-Size="8pt" 
                SkinID="tbPlain" TabIndex="43" Width="60%"></asp:TextBox>
        </td><td style="width:2%;">&#160;</td><td align="right" style="width: 13%; height: 27px; font-size:10pt;"><asp:Label ID="Label24" runat="server" Text="Due Amount"></asp:Label></td><td align="left" style="width: 27%; height: 27px;"><asp:TextBox ID="txtDueAmount" runat="server" Enabled="False" Font-Size="10pt" 
                ReadOnly="True" Width="60%" TabIndex="44"></asp:TextBox></td></tr><tr><td align="right" style="width: 13%; height: 27px; font-size:10pt;">
        Weekly Schedule </td><td align="left" style="height: 27px;" colspan="4">
            <asp:CheckBox ID="SatCheckBox" runat="server" Text="Staturday" 
                oncheckedchanged="SatCheckBox_CheckedChanged" />
            <asp:CheckBox ID="SunCheckBox" runat="server" Text="SunDay" 
                oncheckedchanged="SunCheckBox_CheckedChanged" />
            <asp:CheckBox ID="MonCheckBox" runat="server" Text="MonDay" 
                oncheckedchanged="MonCheckBox_CheckedChanged" />
            <asp:CheckBox ID="TueCheckBox" runat="server" Text="TuesDay" 
                oncheckedchanged="TueCheckBox_CheckedChanged" />
            <asp:CheckBox ID="WedCheckBox" runat="server" Text="WednessDay" 
                oncheckedchanged="WedCheckBox_CheckedChanged" />
            <asp:CheckBox ID="ThusCheckBox" runat="server" Text="Thusday" 
                oncheckedchanged="ThusCheckBox_CheckedChanged" />
            <asp:CheckBox ID="FriCheckBox" runat="server" Text="Friday" 
                oncheckedchanged="FriCheckBox_CheckedChanged" />
        </td></tr><tr><td align="right" style="width: 13%; height: 27px; font-size:10pt;">
    Certification Date</td><td align="left" style="width: 20%; height: 27px;">
        <asp:TextBox ID="txtCertificationDate" runat="server" align="center" 
            placeholder="dd/MM/yyyy" Width="60%" TabIndex="46"></asp:TextBox>
        <cc:CalendarExtender ID="txtCertificationDate_CalendarExtender" runat="server" 
            Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtCertificationDate">
        </cc:CalendarExtender>
    </td><td style="width:2%;">&#160;</td><td align="right" style="width: 13%; height: 27px; font-size:10pt;">&#160;</td><td align="left" style="width: 27%; height: 27px;">&#160;</td></tr>
    <tr>
        <td align="right" style="width: 13%; height: 27px; font-size:10pt;">
            <asp:Label ID="lblTimeAM" runat="server" Visible="False"></asp:Label>
            <asp:Label ID="lblTracID" runat="server" Visible="False"></asp:Label>
        </td>
        <td align="left" style="width: 20%; height: 27px;">
            <asp:Label ID="lblCourseID" runat="server" Visible="False"></asp:Label>
            <asp:Label ID="lblFAcultyID" runat="server" Visible="False"></asp:Label>
            <asp:Label ID="lblRealID" runat="server" Visible="False"></asp:Label>
            <asp:Label ID="lblSheduleID" runat="server" Visible="False"></asp:Label>
        </td>
        <td style="width:2%;">
            &nbsp;</td>
        <td align="right" style="width: 13%; height: 27px; font-size:10pt;">
            &nbsp;</td>
        <td align="left" style="width: 27%; height: 27px;">
            &nbsp;</td>
    </tr>
    </table></asp:Panel></ContentTemplate>
               <%-- </cc:Panel>--%>
                </cc:TabPanel>
    <cc:TabPanel ID="TabPanel7" runat="server" HeaderText="History">
        <HeaderTemplate>History</HeaderTemplate>
    <ContentTemplate><fieldset><fieldset style=" text-align:left; vertical-align: top; border: solid 1px #8BB381;"><legend style="color: maroon; text-align:left;"><b>Course Wise Search Option</b></legend><table style="width:100%"><tr><td  align="center" colspan="5"><table style="width: 100%"><tr><td style="width: 12%; font-weight: 700;">Trac Name</td><td style="width: 25%" align="left"><asp:DropDownList ID="ddlTracID" runat="server" Width="100%" 
                            AutoPostBack="True" onselectedindexchanged="ddlTracID_SelectedIndexChanged"></asp:DropDownList></td><td style="width: 3%">&nbsp;</td><td style="width: 12%">&nbsp;</td><td style="width: 25%">&nbsp;</td><td style="width: 3%">&nbsp;</td><td style="width: 10%">&nbsp;</td><td style="width: 10%">&nbsp;</td></tr><tr><td style="width: 12%; font-weight: 700;">Course Name</td><td style="width: 25%"><asp:DropDownList ID="ddlCourseID" runat="server" Font-Size="8pt" Height="26px" 
                            SkinID="ddlPlain" Width="100%"><asp:ListItem></asp:ListItem></asp:DropDownList></td><td style="width: 3%">&nbsp;</td><td style="width: 12%; font-weight: 700;">Batch</td><td align="left" style="width: 25%"><asp:TextBox ID="txtBatchNo" runat="server" Height="22px" Width="100%"></asp:TextBox></td><td style="width: 3%">&nbsp;</td><td style="width: 10%"><asp:Button ID="SearchButton" runat="server" BorderStyle="Outset" 
                            BorderWidth="1px" Height="28px" OnClick="BtnSearch_Click" Text="Search" 
                            ToolTip="Search" Width="80px" /></td><td style="width: 10%"><asp:Button ID="Refresh" runat="server" BorderStyle="Outset" BorderWidth="1px" 
                            Height="28px" OnClick="Refresh_Click" Text="Refresh" ToolTip="Refresh" 
                            Width="80px" /></td></tr></table></td></tr><tr><td align="right">&nbsp;</td><td left"="">&nbsp;</td><td left"="" style=" align=">&nbsp;</td><td align="center" style="vertical-align:middle; height:100%;">&nbsp;</td><td align="center" style="vertical-align:middle; height:100%;">&nbsp;</td></tr></table></fieldset> <asp:GridView 
            ID="HistoryGridView" runat="server" AllowPaging="True" 
                AllowSorting="True" 
                AutoGenerateColumns="False" BackColor="White" BorderColor="LightGray" 
                BorderStyle="Solid" BorderWidth="1px" CellPadding="2" CssClass="mGrid" 
                Font-Size="8pt" ForeColor="#333333" 
                onpageindexchanging="dgStd_PageIndexChanging" 
                onselectedindexchanged="dgStd_SelectedIndexChanged" 
                PageSize="15" Width="100%"><AlternatingRowStyle CssClass="alt" /><Columns><asp:CommandField 
                        ShowSelectButton="True"><ItemStyle ForeColor="Blue" Height="25px" HorizontalAlign="Center" 
                        Width="40px" /></asp:CommandField><asp:BoundField 
                DataField="student_id" HeaderText="Student ID"><ItemStyle HorizontalAlign="Center" Width="90px" /></asp:BoundField><asp:BoundField 
                DataField="f_name" HeaderText="Name"><ItemStyle HorizontalAlign="Left" Width="150px" /></asp:BoundField><asp:BoundField 
                DataField="CourseID" HeaderText="Course ID"><ItemStyle HorizontalAlign="Center" Width="150px" /></asp:BoundField><asp:BoundField 
                DataField="CourseName" HeaderText="Course Name"><ItemStyle HorizontalAlign="Left" Width="150px" /></asp:BoundField><asp:BoundField DataField="TrainerName" HeaderText="Trainer Name" /><asp:BoundField DataField="BatchNo" HeaderText="Batch No" /><asp:BoundField DataField="mobile_no" HeaderText="Mobile No" /><asp:BoundField DataField="email" HeaderText="Email No" /></Columns><HeaderStyle BackColor="Silver" Font-Bold="True" ForeColor="White" /><PagerStyle 
            HorizontalAlign="Center" CssClass="pgr" /><RowStyle BackColor="White" /></asp:GridView></ContentTemplate>
    </cc:TabPanel>

</cc:TabContainer>
</div>

</td>
<td style="width:1%;"></td>
</tr>
</table>
</div>
</asp:Content>

