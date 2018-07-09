<%@ Page Language="C#" MasterPageFile="~/temp.master" AutoEventWireup="true" CodeFile="BatteryReceipt.aspx.cs" Inherits="BatteryReceipt" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<script language="javascript" type="text/javascript">
    function validation() {
        var recInvoiceNo = document.getElementById('<%= txtBatRecInvoiceNo.ClientID %>');
        var recInvoiceDate = document.getElementById('<%= txtBatRecInvoiceDate.ClientID %>');
        var recDate = document.getElementById('<%= txtBatRecDate.ClientID %>');
        var remarks = document.getElementById('<%= txtRemarks.ClientID %>');
        var poDate = document.getElementById('<%= txtBatRecPODate.ClientID %>');
        if (!RequiredValidation(recInvoiceNo, "Invoice Number cannot be Blank"))
            return false;
        if (!RequiredValidation(recInvoiceDate, "Invoice Date cannot be Blank"))
            return false;
        if (trim(recInvoiceDate.value) !== "" && !isValidDate(recInvoiceDate.value)) {
            alert("Enter the Invoice Date");
            recInvoiceDate.focus();
            return false;
        }
        if (Date.parse(recInvoiceDate.value) < Date.parse(poDate.value)) {
            alert("Invoice Date should be greater than PODate Date");
            recInvoiceDate.focus();
            return false;
        }
        var now = new Date();
        if (Date.parse(recInvoiceDate.value) > Date.parse(now)) {
            alert("Invoice Date should not be greater than Current Date");
            recInvoiceDate.focus();
            return false;
        }
        if (Date.parse(recDate.value) < Date.parse(recInvoiceDate.value)) {
            alert("Receipt Date should be greater than Invoice Date");
            recDate.focus();
            return false;
        }
        if (!RequiredValidation(recDate, "Receipt Date cannot be Blank"))
            return false;
        if (trim(recDate.value) !== "") {
            if (!isValidDate(recDate.value)) {
                alert("Enter the Receipt Date");
                recDate.focus();
                return false;
            }
        }
        now = new Date();
        if (Date.parse(recDate.value) > Date.parse(now)) {
            alert("Battery Receipt Date should not be greater than Current Date");
            recDate.focus();
            return false;
        }
        if (!RequiredValidation(remarks, "Remarks cannot be Blank"))
            return false;
        return true;
    }
</script>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
<fieldset style="padding: 10px">
<legend>Battery Receipt</legend>
<div runat="server" align="center">
    Vehicle Number<span style="color: Red">*</span>
    <ajaxToolkit:ComboBox AutoCompleteMode="Append" ID="ddlInventoryBatteryReceiptVehicles" runat="server" AutoPostBack="True" DropDownStyle="DropDownList"
                          OnSelectedIndexChanged="ddlInventoryBatteryReceiptVehicles_SelectedIndexChanged">
    </ajaxToolkit:ComboBox>
</div>
<br/>
<div runat="server">
    Battery Details
</div>
<br/>
<div runat="server" align="center">
    <table>
        <tr>
            <td class="rowseparator">
            </td>
        </tr>
        <tr>
            <td>
                <asp:GridView ID="grvBatteryDetailsForReceipt" runat="server" CellPadding="3"
                              EmptyDataText="Details are not available" CssClass="gridviewStyle"
                              AutoGenerateColumns="False" AllowPaging="True" DataKeyNames="FleetInventoryReqID"
                              PageSize="5" OnPageIndexChanging="grvBatteryDetailsForReceipt_PageIndexChanging"
                              OnRowCommand="grvBatteryDetailsForReceipt_RowCommand" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px">
                    <Columns>
                        <asp:BoundField HeaderText="Vehicle Number" DataField="VehicleNum"/>
                        <asp:BoundField HeaderText="District" DataField="ds_lname"/>
                        <asp:BoundField HeaderText="No. of Batteries" DataField="RequestedQty"/>
                        <asp:BoundField HeaderText="Request Date" DataField="RequestedDate" DataFormatString="{0:dd/MM/yyyy}"/>
                        <asp:BoundField HeaderText="Issued Quantity" DataField="IssuedQty"/>
                        <asp:BoundField HeaderText="Issued Date" DataField="IssueDate" DataFormatString="{0:dd/MM/yyyy}"/>
                        <asp:TemplateField ControlStyle-Width="50px" HeaderStyle-Width="60px">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnViewDetails" runat="server" Text="Receipt" ToolTip="Click here to Receipt the details"
                                                OnClick="BtnViewDetails_Click" CssClass="button2" RowIndex="<%# Container.DisplayIndex %>"
                                                CommandName="Show" CommandArgument='<%#DataBinder.Eval(Container.DataItem,"InventoryItemIssueID") %>'/>
                            </ItemTemplate>
                            <ControlStyle Width="50px"/>
                            <HeaderStyle Width="60px"/>
                        </asp:TemplateField>
                    </Columns>
                    <RowStyle CssClass="rowStyleGrid" ForeColor="#000066"/>
                    <FooterStyle CssClass="footerStylegrid" BackColor="White" ForeColor="#000066"/>
                    <PagerStyle CssClass="pagerStylegrid" BackColor="White" ForeColor="#000066" HorizontalAlign="Left"/>
                    <SelectedRowStyle CssClass="selectedRowStyle" BackColor="#669999" Font-Bold="True" ForeColor="White"/>
                    <HeaderStyle CssClass="headerStyle" BackColor="#006699" Font-Bold="True" ForeColor="White"/>
                    <SortedAscendingCellStyle BackColor="#F1F1F1"/>
                    <SortedAscendingHeaderStyle BackColor="#007DBB"/>
                    <SortedDescendingCellStyle BackColor="#CAC9C9"/>
                    <SortedDescendingHeaderStyle BackColor="#00547E"/>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td class="rowseparator">
            </td>
        </tr>
    </table>
</div>
<asp:Button ID="btnShowPopup" runat="server" Style="display: none"/>
<ajaxToolkit:ModalPopupExtender ID="gv_ModalPopupExtenderBatteryReceipt" BehaviorID="mdlPopup"
                                runat="server" TargetControlID="btnShowPopup" PopupControlID="pnlPopup" BackgroundCssClass="modalBackground"/>
<asp:Panel ID="pnlPopup" runat="server" CssClass="modalPanel" Style="display: none; padding: 10px;">
    <fieldset>
        <legend>Battery Receipt Details</legend>
        <table class="style1" align="center">
            <tr>
                <td class="rowseparator">
                </td>
            </tr>
            <tr>
                <td align="left">
                    Vehicle Number
                </td>
                <td class="columnseparator">
                </td>
                <td align="left">
                    <asp:TextBox ID="txtBatRecVehicleNo" runat="server" ReadOnly="True"></asp:TextBox>
                </td>
                <td class="columnseparator">
                </td>
                <td align="left">
                    District
                </td>
                <td class="columnseparator">
                </td>
                <td align="left">
                    <asp:TextBox ID="txtBatRecDistrict" runat="server" ReadOnly="True"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="rowseparator">
                </td>
            </tr>
            <tr>
                <td align="left">
                    PO Number
                </td>
                <td class="columnseparator">
                </td>
                <td align="left">
                    <asp:TextBox ID="txtBatRecPONumber" runat="server" ReadOnly="True"></asp:TextBox>
                </td>
                <td class="columnseparator">
                </td>
                <td align="left">
                    PO Date
                </td>
                <td class="columnseparator">
                </td>
                <td align="left">
                    <asp:TextBox ID="txtBatRecPODate" runat="server" ReadOnly="True" oncut="return false;"
                                 onpaste="return false;" oncopy="return false;">
                    </asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="rowseparator">
                </td>
            </tr>
            <tr>
                <td align="left">
                    Courier Name
                </td>
                <td class="columnseparator">
                </td>
                <td align="left">
                    <asp:TextBox ID="txtBatRecCourierName" runat="server" ReadOnly="True" MaxLength="20"></asp:TextBox>
                </td>
                <td class="columnseparator">
                </td>
                <td align="left">
                    HO Remarks
                </td>
                <td class="columnseparator">
                </td>
                <td align="left">
                    <asp:TextBox ID="txtBatRecHORemarks" runat="server" ReadOnly="True" MaxLength="20"
                                 TextMode="MultiLine">
                    </asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="rowseparator">
                </td>
            </tr>
            <tr>
                <td align="left">
                    Invoice No
                </td>
                <td class="columnseparator">
                </td>
                <td align="left">
                    <asp:TextBox ID="txtBatRecInvoiceNo" onkeypress="return numeric_only(event)" runat="server" MaxLength="5"></asp:TextBox>
                </td>
                <td class="columnseparator">
                </td>
                <td align="left">
                    Invoice Date
                </td>
                <td class="columnseparator">
                </td>
                <td align="left">
                    <asp:TextBox ID="txtBatRecInvoiceDate" runat="server" MaxLength="20" onkeypress="return false" oncut="return false;" onpaste="return false;" oncopy="return false;"></asp:TextBox>
                    <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtBatRecInvoiceDate"
                                                  Format="MM/dd/yyyy" PopupButtonID="ImageButton1">
                    </ajaxToolkit:CalendarExtender><asp:ImageButton ID="ImageButton1" runat="server" alt="" src="images/Calendar.gif" Style="vertical-align: top"/>
                </td>
            </tr>
            <tr>
                <td class="rowseparator">
                </td>
            </tr>
            <tr>
                <td align="left">
                    Receipt Date
                </td>
                <td class="columnseparator">
                </td>
                <td align="left" colspan="3">
                    <asp:TextBox ID="txtBatRecDate" runat="server" MaxLength="20" onkeypress="return false" oncut="return false;" onpaste="return false;" oncopy="return false;"></asp:TextBox>
                    <ajaxToolkit:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtBatRecDate"
                                                  Format="MM/dd/yyyy" PopupButtonID="ImageButton2">
                    </ajaxToolkit:CalendarExtender><asp:ImageButton ID="ImageButton2" runat="server" alt="" src="images/Calendar.gif" Style="vertical-align: top"/>
                </td>
            </tr>
            <tr>
                <td class="rowseparator">
                </td>
            </tr>
            <tr>
                <td colspan="3" align="center">
                    <asp:GridView ID="grvBatteryReceiptDetailsPopup" runat="server" AutoGenerateColumns="False"
                                  GridLines="None" CssClass="gridviewStyle" CellPadding="3" CellSpacing="2" BackColor="#DEBA84"
                                  Width="95%" OnRowDataBound="grvBatteryReceiptDetailsPopup_RowDataBound">
                        <Columns>
                            <asp:BoundField HeaderText="Battery Number" DataField="BatteryNumber"/>
                            <asp:BoundField HeaderText="Make" DataField="Make"/>
                            <asp:BoundField HeaderText="Model" DataField="Model"/>
                            <asp:BoundField HeaderText="Capacity" DataField="Capacity"/>
                            <asp:TemplateField HeaderText="ReceivedQty">
                                <ItemTemplate>
                                    <asp:Label ID="txtBatteryReceivedQty" runat="server" Text="1"></asp:Label>
                                    <asp:Label ID="LbIssueID" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"InventoryItemIssueID") %>'
                                               Visible="false">
                                    </asp:Label>
                                    <asp:Label ID="LbReqID" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"FleetInventoryReqID") %>'
                                               Visible="false">
                                    </asp:Label>
                                    <asp:Label ID="LbBatteryID" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"BatteryIssueDetailsID") %>'
                                               Visible="false">
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <RowStyle CssClass="rowStyleGrid"/>
                        <FooterStyle CssClass="footerStylegrid"/>
                        <PagerStyle CssClass="pagerStylegrid"/>
                        <SelectedRowStyle CssClass="selectedRowStyle"/>
                        <HeaderStyle CssClass="headerStyle"/>
                    </asp:GridView>
                    <div id="Div7" align="center" style="background-color: white; width: 95%;">
                    Remarks
                    <asp:TextBox ID="txtRemarks" runat="server" onKeyUp="CheckLength(this,50)"
                                 onChange="CheckLength(this,50)">
                    </asp:TextBox>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnOk" runat="server" Text="Issue" Width="50px" OnClick="btnOk_Click"/>
                    &nbsp;&nbsp;
                    <asp:Button runat="server" Text="Cancel" Width="50px" OnClick="btnNo_Click"/>
                </td>
            </tr>
            <tr>
                <td class="rowseparator">
                </td>
            </tr>
        </table>
    </fieldset>
</asp:Panel>
</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>