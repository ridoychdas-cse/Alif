﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Baby_One_Transcript_Class_Test_1.aspx.cs" Inherits="Report_AMC_Report_UI_Baby_One_Transcript_Class_Test_1" %>

  
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
     <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" 
            AutoDataBind="true" onload="CrystalReportViewer1_Load" />
    </div>
    </form>
</body>
</html>
