<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="frmPaymentInfo.aspx.cs" Inherits="frmPaymentInfo" Title="Payment Type Setup" Theme="Themes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">


<div id="frmMainDiv" style="background-color:White; width:100%;">

<table style="width:100%;">
<tr>
  <td colspan="3" align="center">
      <asp:Label ID="Label3" runat="server" BackColor="#CCCCCC" Font-Bold="True" 
          Font-Size="Medium" ForeColor="#CC3300" Text="Student Pay Head Setting" 
          Width="95%" Height="25px" BorderStyle="Inset"></asp:Label>
    </td>
</tr>
<tr><td colspan="3"></td></tr>
<tr>
<td colspan="3">
    <ajaxToolkit:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="1" 
        Width="100%">
        <ajaxToolkit:TabPanel runat="server" HeaderText="Head Setting" ID="TabPanel1">
            <ContentTemplate>
                <table style="width: 100%">
                    <tr>
                        <td style="width: 234px">
                            &nbsp;</td>
                        <td style="width: 658px">
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width: 234px">
                            &nbsp;</td>
                        <td align="center" style="width: 658px">
                        <fieldset style="vertical-align: top; border: solid 1px #8BB381; line-height: 1.5em;">
                            <table style="width: 100%">
                                <tr>
                                    <td align="right" style="width: 247px">
                                        &nbsp;</td>
                                    <td style="width: 18px">
                                        &nbsp;</td>
                                    <td align="left">
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td align="right" style="width: 247px">
                                        <asp:Label ID="Label1" runat="server" Text="ID"></asp:Label>
                                    </td>
                                    <td style="width: 18px">
                                        &nbsp;</td>
                                    <td align="left">
                                        <asp:TextBox ID="txtId" runat="server" Width="250px" Enabled="False" 
                                            Height="22px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" style="width: 247px">
                                        <asp:Label ID="Label2" runat="server" Text="Head Name"></asp:Label>
                                    </td>
                                    <td style="width: 18px">
                                        &nbsp;</td>
                                    <td align="left">
                                        <asp:TextBox ID="txtHeadName"  runat="server" Width="250px" Height="22px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" style="width: 247px">
                                        &nbsp;</td>
                                    <td style="width: 18px">
                                        &nbsp;</td>
                                    <td align="left">
                                        &nbsp;</td>
                                </tr>
                            </table>
                            </fieldset>
                        </td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width: 234px">
                            &nbsp;</td>
                        <td style="width: 658px">
                            <table align="center" style="width: 100%">
                                <tr>
                                    <td align="center" style="width: 156px">
                                        <asp:Button ID="btnDelete" runat="server" BorderStyle="Outset" 
                                            BorderWidth="1px" Height="28px" Text="Delete" 
                                            Width="100px" onclick="btnDelete_Click" />
                                    </td>
                                    <td align="center">
                                        <asp:Button ID="btnSave" runat="server" BorderStyle="Outset" BorderWidth="1px" 
                                            Height="28px" OnClick="btnSave_Click" Text="Save" Width="100px" />
                                        <asp:Button ID="btnUpdate" runat="server" BorderStyle="Outset" 
                                            BorderWidth="1px" Height="28px" OnClick="btnUpdate_Click" Text="Update" 
                                            Width="100px" />
                                    </td>
                                    <td align="center">
                                        <asp:Button ID="btnClear" runat="server" BorderStyle="Outset" BorderWidth="1px" 
                                            Height="28px" Text="Clear" Width="100px" onclick="btnClear_Click" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width: 234px">
                            &nbsp;</td>
                        <td style="width: 658px;" >
                            <asp:GridView ID="GridView1" runat="server" style="text-align:left;" 
                                AutoGenerateColumns="False"  CssClass="mGrid" Width="100%" AllowPaging="True" 
                                onpageindexchanging="GridView1_PageIndexChanging" 
                                onselectedindexchanged="GridView1_SelectedIndexChanged" PageSize="20">
                                <Columns>
                                    <asp:CommandField ShowSelectButton="True"> <ItemStyle HorizontalAlign="Center" Width="80px" /></asp:CommandField>
                                    <asp:BoundField HeaderText="ID" DataField="ID"><ItemStyle HorizontalAlign="Left" Width="50px" /></asp:BoundField>
                                    <asp:BoundField HeaderText="Head Name" DataField="Head_Name"></asp:BoundField>
                                </Columns>
                            </asp:GridView>
                        </td>
                        <td>
                            &nbsp;</td>
                    </tr>
                </table>
            </ContentTemplate>
        </ajaxToolkit:TabPanel>
        <ajaxToolkit:TabPanel ID="TabPanel2" HeaderText="Paye Head" runat="server">
            <ContentTemplate>
                <table style="width: 100%">
                    <tr>
                        <td>
                        <fieldset style="border:solid 1px lightgray;"><legend>Search Option</legend>
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 120px">
                                        <asp:Label ID="Label4" runat="server" Text="Class :"></asp:Label>
                                    </td>
                                    <td style="width: 247px">
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlClass" runat="server" Width="95%" AutoPostBack="True" 
                                            Height="26px" onselectedindexchanged="ddlClass_SelectedIndexChanged1">
                                        </asp:DropDownList>
                                        </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td style="width: 24px">                                       
                                        
                                    </td>
                                    <td style="width: 83px">
                                        <asp:Label ID="Label5" runat="server" Text="Verson :"></asp:Label>
                                    </td>
                                    <td style="width: 238px">
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlVersion" runat="server" Width="95%" Height="26px">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                    </asp:UpdatePanel>
                                    </td>
                                    <td style="width: 103px">
                                        <asp:Button ID="btnSearch" runat="server" Text="Search" 
                                            onclick="btnSearch_Click" />
                                    </td>
                                    <td style="width: 94px">
                                        <asp:Button ID="btnRefresh" runat="server" Text="Refresh" 
                                            onclick="btnRefresh_Click" />
                                    </td>
                                    <td>
                                        &nbsp;</td>
                                </tr>
                            </table>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 445px" valign="top">
                            <asp:GridView ID="dgPay" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" 
                                BackColor="White" BorderColor="LightGray" BorderStyle="Solid" BorderWidth="1px" 
                                Caption="Student Pay Head Setting" CellPadding="2" CssClass="mGrid" 
                                Font-Size="8pt" onpageindexchanging="dgPay_PageIndexChanging" 
                                onrowcancelingedit="dgPay_RowCancelingEdit" onrowcommand="dgPay_RowCommand" 
                                onrowdatabound="dgPay_RowDataBound" onrowdeleting="dgPay_RowDeleting" 
                                onrowediting="dgPay_RowEditing" onrowupdating="dgPay_RowUpdating" 
                                PageSize="30" Width="100%">
                                <HeaderStyle BackColor="LightGray" Font-Bold="True" Font-Names="Arial" 
                                    Font-Size="8pt" ForeColor="Black" HorizontalAlign="Center" />
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>                                            
                                            <asp:LinkButton ID="lbDelete" runat="server" CausesValidation="False" CommandName="Delete" onclientclick="javascript:return window.confirm('are u really want to delete  these data')" Text="Remove"></asp:LinkButton>
                                            <asp:LinkButton ID="lbEdit" runat="server" CausesValidation="False" CommandName="Edit" Text="Edit"></asp:LinkButton>
                                            <asp:LinkButton ID="btnAddDet" runat="server" CommandName="AddNew" Text="New"></asp:LinkButton>                                            
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:LinkButton ID="lbCancel" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel" ForeColor="#FF3300"></asp:LinkButton>
                                            <asp:LinkButton ID="lbUpdate" runat="server" CausesValidation="True" CommandName="Update" Text="Update" ForeColor="#FF3300"></asp:LinkButton> 
                                        </EditItemTemplate>
                                        <FooterTemplate>                                            
                                            <asp:LinkButton ID="lbCancel" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel"></asp:LinkButton>
                                            <asp:LinkButton ID="lbInsert" runat="server" CommandName="Insert" Text="Add"></asp:LinkButton>
                                        </FooterTemplate>
                                        <ItemStyle Font-Size="8pt" Height="20px" Width="200px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ID">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPayId" runat="server" Text='<%#Eval("pay_id") %>' Width="100px"></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtPayId" runat="server" CssClass="tbc" Font-Size="8pt" 
                                                MaxLength="7" SkinID="tbPlain" Text='<%#Eval("pay_id") %>' Width="90px"></asp:TextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtPayId" runat="server" CssClass="tbc" Font-Size="8pt" 
                                                MaxLength="7" SkinID="tbPlain" Width="90px"></asp:TextBox>
                                        </FooterTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="40px" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Payment Name">
                                        <ItemTemplate>
                                         <asp:Label ID="lblPayNameID" runat="server" Text='<%#Eval("pay_Name_Id") %>' Width="100px" Visible="false"></asp:Label>
                                         <asp:Label ID="lblPayName" runat="server" Text='<%#Eval("pay_head_id") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                             <asp:DropDownList ID="ddlPayHead" runat="server" Font-Size="8pt" Height="18px" 
                                                SkinID="ddlPlain" TabIndex="66" Width="120px">
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:DropDownList ID="ddlPayHead" runat="server" Font-Size="8pt" Height="18px" 
                                                SkinID="ddlPlain" TabIndex="66" Width="120px">
                                            </asp:DropDownList>
                                        </FooterTemplate>
                                        <ItemStyle HorizontalAlign="Left" Width="120px" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Payment Type">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPayType" runat="server" Text='<%#Eval("pay_type") %>' 
                                                Width="120px"></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlPayType" runat="server" Font-Size="8pt" Height="18px" 
                                                SkinID="ddlPlain" TabIndex="66" Width="120px">
                                                <asp:ListItem>Select Type</asp:ListItem>
                                                <asp:ListItem Text="Yearly Payment" Value="Y"></asp:ListItem>
                                                <asp:ListItem Text="Yearly Payment New Student" Value="N"></asp:ListItem>
                                                <asp:ListItem Text="Yearly Payment Old Student" Value="O"></asp:ListItem>
                                                <asp:ListItem Text="Monthly Payment" Value="M"></asp:ListItem>
                                                <asp:ListItem Text="Tri-monthly Payment" Value="T"></asp:ListItem>
                                                <asp:ListItem Text="Half-yearly Payment" Value="H"></asp:ListItem>
                                                <asp:ListItem Text="Transport Fee" Value="TR"></asp:ListItem>
                                                <asp:ListItem Text="Late Fee" Value="L"></asp:ListItem>
                                                <asp:ListItem Text="Extra Charge" Value="E"></asp:ListItem>
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:DropDownList ID="ddlPayType" runat="server" Font-Size="8pt" Height="18px" 
                                                SkinID="ddlPlain" TabIndex="66" Width="120px">
                                                <asp:ListItem></asp:ListItem>
                                                <asp:ListItem Text="Yearly Payment" Value="Y"></asp:ListItem>
                                                <asp:ListItem Text="Yearly Payment New Student" Value="N"></asp:ListItem>
                                                <asp:ListItem Text="Yearly Payment Old Student" Value="O"></asp:ListItem>
                                                <asp:ListItem Text="Monthly Payment" Value="M"></asp:ListItem>
                                                <asp:ListItem Text="Tri-monthly Payment" Value="T"></asp:ListItem>
                                                <asp:ListItem Text="Half-yearly Payment" Value="H"></asp:ListItem>
                                                <asp:ListItem Text="Transport Fee" Value="TR"></asp:ListItem>
                                                <asp:ListItem Text="Late Fee" Value="L"></asp:ListItem>
                                                <asp:ListItem Text="Extra Charge" Value="E"></asp:ListItem>
                                            </asp:DropDownList>
                                        </FooterTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="120px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="For Class">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPayClassName" runat="server" Text='<%#Eval("class") %>' 
                                                Width="100px"></asp:Label>
                                            <asp:Label ID="lblPayClass" runat="server" style="display:none" 
                                                Text='<%#Eval("pay_class") %>' Width="100px"></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlPayClass" runat="server" AutoPostBack="true" 
                                                Font-Size="8pt" Height="18px" SkinID="ddlPlain" TabIndex="66" Width="90px">
                                                <asp:ListItem></asp:ListItem>
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:DropDownList ID="ddlPayClass" runat="server" AutoPostBack="true" 
                                                Font-Size="8pt" Height="30px" 
                                                OnSelectedIndexChanged="ddlClass_SelectedIndexChanged" SkinID="ddlPlain" 
                                                Width="90px">
                                                <asp:ListItem></asp:ListItem>
                                            </asp:DropDownList>
                                        </FooterTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="90px" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Version">
                                        <ItemTemplate>
                                            <asp:Label ID="lblVersionName" runat="server" Text='<%#Eval("version_name") %>' 
                                                Width="100px"></asp:Label>
                                            <asp:Label ID="lblVersion" runat="server" style="display:none" 
                                                Text='<%#Eval("version") %>' Width="90px"></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlVersion" runat="server" Font-Size="8pt" Height="18px" 
                                                SkinID="ddlPlain" TabIndex="66" Width="100px">
                                                <asp:ListItem></asp:ListItem>
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:DropDownList ID="ddlVersion" runat="server" Font-Size="8pt" Height="30px" 
                                                SkinID="ddlPlain" Width="90px">
                                                <asp:ListItem></asp:ListItem>
                                            </asp:DropDownList>
                                        </FooterTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="90px" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Group">
                                        <ItemTemplate>
                                            <asp:Label ID="lblGroupName" runat="server" Text='<%#Eval("GroupName") %>' Width="100px"></asp:Label>
                                            <asp:Label ID="lblGroupID" runat="server" style="display:none" Text='<%#Eval("GroupID") %>' Width="90px"></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlGroup" runat="server" Font-Size="8pt" Height="18px" SkinID="ddlPlain" TabIndex="66" Width="100px">
                                                <asp:ListItem></asp:ListItem>
                                                <asp:ListItem Text="Science" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Commerce" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="Humanities" Value="3"></asp:ListItem>
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:DropDownList ID="ddlGroup" runat="server" Font-Size="8pt" Height="30px" SkinID="ddlPlain" Width="90px">
                                                 <asp:ListItem></asp:ListItem>
                                                <asp:ListItem Text="Science" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Commerce" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="Humanities" Value="3"></asp:ListItem>
                                            </asp:DropDownList>
                                        </FooterTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="90px" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="For All Students?">
                                        <ItemTemplate>
                                            <asp:Label ID="lblForAllStd" runat="server" Text='<%#Eval("for_all_std") %>' 
                                                Width="100px"></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlForAllStd" runat="server" Font-Size="8pt" 
                                                Height="18px" SkinID="ddlPlain" Width="100px">
                                                <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                                <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:DropDownList ID="ddlForAllStd" runat="server" Font-Size="8pt" 
                                                Height="18px" SkinID="ddlPlain" Width="100px">                                                
                                                <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                                <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                            </asp:DropDownList>
                                        </FooterTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Payment Amount">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPayAmt" runat="server" style=" text-align:right;" 
                                                Text='<%#Eval("pay_amt") %>' Width="120px"></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtPayAmt" runat="server" CssClass="tbl" Font-Size="8pt" style=" text-align:right;" 
                                                MaxLength="35" SkinID="tbPlain" Text='<%#Eval("pay_amt") %>' Width="110px"></asp:TextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtPayAmt" runat="server" CssClass="tbl" Font-Size="8pt" 
                                                MaxLength="35" SkinID="tbPlain" style=" text-align:right;" Width="110px"></asp:TextBox>
                                        </FooterTemplate>
                                        <ItemStyle HorizontalAlign="Right" Width="120px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Waiver?">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDiscount" runat="server" Text='<%#Eval("discount") %>' 
                                                Width="100px"></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlDiscount" runat="server" Font-Size="8pt" Height="18px" 
                                                SkinID="ddlPlain" Width="90px">
                                                <asp:ListItem></asp:ListItem>
                                                <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                                <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:DropDownList ID="ddlDiscount" runat="server" Font-Size="8pt" 
                                                Height="18px" SkinID="ddlPlain" Width="90px">
                                                <asp:ListItem></asp:ListItem>
                                                <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                                <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                            </asp:DropDownList>
                                        </FooterTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="90px" />
                                    </asp:TemplateField>
                                </Columns>
                                <AlternatingRowStyle CssClass="alt" />
                                <PagerStyle CssClass="pgr" />
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;</td>
                    </tr>
                </table>
            </ContentTemplate>
        </ajaxToolkit:TabPanel>
    </ajaxToolkit:TabContainer>
    </td>
</tr>
<tr>
<td style="width:1%;"></td>
<td style="width:98%;" align="center">
    &nbsp;</td>
<td style="width:1%;"></td>
</tr>
</table> 
</div>
</asp:Content>

