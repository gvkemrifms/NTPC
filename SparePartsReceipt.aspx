<%@ Page Language="C#" MasterPageFile="~/temp.master" AutoEventWireup="true" CodeFile="SparePartsReceipt.aspx.cs" Inherits="SparePartsReceipt" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolKit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<script type="text/javascript">
    function validation() {
        var vehicleId = document.getElementById('<%= ddlVehicles.ClientID %>');

        var receiptDate = document.getElementById('<%= txtReceiptDate.ClientID %>');

        var receiptRemarks = document.getElementById('<%= txtReceiptRemarks.ClientID %>');

        var invoiceNo = document.getElementById('<%= txtInvoiceNo.ClientID %>');

        var invoiceDate = document.getElementById('<%= txtInvoiceDate.ClientID %>');

        var dcDate = document.getElementById('<%= txtDCDate.ClientID %>');

        var txtReceivedQuantity = document.getElementById(window.ReceivedQuantity);

        switch (vehicleId.selectedIndex) {
        case 0:
            alert("Please select Vehicle");
            vehicleId.focus();
            return false;
        }


        if (!RequiredValidation(receiptDate, "ReceiptDate cannot be Blank"))
            return false;

        if (!RequiredValidation(receiptRemarks, "ReceiptRemarks cannot be blank"))
            return false;

        if (!RequiredValidation(invoiceNo, "InvoiceNo cannot be blank"))
            return false;

        if (!RequiredValidation(invoiceDate, "InvoiceDate cannot be blank"))
            return false;

        if (!RequiredValidation(txtReceivedQuantity, "ReceivedQuantity cannot be blank"))
            return false;


        if (trim(receiptDate.value) !== "" && !isValidDate(receiptDate.value)) {
            alert("Enter the Valid Date");
            receiptDate.focus();
            return false;
        }

        var now = new Date();
        if (Date.parse(receiptDate.value) > Date.parse(now)) {
            alert("Receipt Date should not be greater than Current Date");
            receiptDate.focus();
            return false;
        }

        if (Date.parse(receiptDate.value) < Date.parse(invoiceDate.value)) {
            alert("Receipt Date should be greater than Invoice Date");
            receiptDate.focus();
            return false;
        }

        if (Date.parse(dcDate.value) > Date.parse(invoiceDate.value)) {
            alert("Invoice Date should be greater than DC Date");
            invoiceDate.focus();
            return false;
        }

        if (trim(invoiceDate.value) !== "" && !isValidDate(invoiceDate.value)) {
            alert("Enter the Valid Date");
            invoiceDate.focus();
            return false;
        }
        now = new Date();
        if (Date.parse(invoiceDate.value) > Date.parse(now)) {
            alert("Invoice Date should not be greater than Current Date");
            invoiceDate.focus();
            return false;
        }
        return true;
    }

    function ValidateIssueQty(issQtyId, reqQty) {
        var objIssQty = document.getElementById(issQtyId);
        if (parseInt(objIssQty.value) > parseInt(reqQty)) {
            alert("Receive Quantity Cannot be more than Issue Quantity");
            objIssQty.focus();
            return false;
        }
        return true;
    }
</script>
<asp:UpdatePanel ID="UpdPanel1" runat="server">
<ContentTemplate>
<fieldset style="padding: 10px">
<legend align="center" style="color:brown">Spare Part Receipt</legend>
<table style="width: 100%">
<tr>
    <td class="rowseparator"></td>
</tr>
<tr>
    <td align="center">
        <asp:Label runat="server" Text="Vehicles"></asp:Label>
        <span style="color: Red">*</span>
        <ajaxToolKit:ComboBox AutoCompleteMode="Append" ID="ddlVehicles" runat="server" AutoPostBack="True"
                              OnSelectedIndexChanged="ddlVehicles_SelectedIndexChanged" DropDownStyle="DropDownList">
        </ajaxToolKit:ComboBox>
    </td>
</tr>
<tr>
    <td class="rowseparator"></td>
</tr>
<tr>
    <td align="center">
        <asp:GridView ID="gvIssueDetails" runat="server" AutoGenerateColumns="False" CellPadding="3"
                      EmptyDataText="Details are not available" CssClass="gridviewStyle"
                      OnPageIndexChanging="gvIssueDetails_PageIndexChanging" OnRowCommand="gvIssueDetails_RowCommand" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px">
            <Columns>
                <asp:BoundField HeaderText="VehicleNum" DataField="VehicleNum"/>
                <asp:BoundField HeaderText="District" DataField="ds_lname"/>
                <asp:BoundField HeaderText="ReqQty" DataField="RequestedQty"/>
                <asp:BoundField HeaderText="ReqDate" DataField="RequestedDate"/>
                <asp:BoundField HeaderText="IssQty" DataField="IssuedQty"/>
                <asp:BoundField HeaderText="IssDate" DataField="IssueDate"/>
                <asp:TemplateField ControlStyle-Width="50px" HeaderStyle-Width="60px">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnViewDetails" runat="server" Text="Receipt" ToolTip="Click here to Receipt the details"
                                        OnClick="btnViewDetails_Click" CssClass="button2" RowIndex="<%# Container.DisplayIndex %>"
                                        CommandName="Show" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "InventoryItemIssueID") %>'/>
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
            <SortedAscendingCellStyle BackColor="#F1F1F1" />
            <SortedAscendingHeaderStyle BackColor="#007DBB" />
            <SortedDescendingCellStyle BackColor="#CAC9C9" />
            <SortedDescendingHeaderStyle BackColor="#00547E" />
        </asp:GridView>
    </td>
</tr>
<tr>
    <td class="rowseparator"></td>
</tr>
<tr>
    <td>
        <asp:Button ID="btnShowPopup" runat="server" Style="display: none"/>
    </td>
</tr>
<tr>
    <td class="rowseparator"></td>
</tr>
<tr>
    <td></td>
    <ajaxToolKit:ModalPopupExtender ID="gv_ModalPopupExtender1" BehaviorID="mdlPopup"
                                    runat="server" TargetControlID="btnShowPopup" PopupControlID="pnlPopup" BackgroundCssClass="modalBackground"/>
</tr>
<tr>
    <td class="rowseparator"></td>
</tr>
<tr>
    <td class="rowseparator"></td>
</tr>
<tr>
<td align="center">
<asp:Panel ID="pnlPopup" runat="server" CssClass="modalPanel" Style="display: none; padding: 10px;">
<fieldset style="padding: 10px;">
    <legend>Receipt Details </legend>
    <table>
        <tr>
            <td class="rowseparator"></td>
        </tr>
        <tr>
            <td>
                <asp:Label runat="server" Text="VehicleID"></asp:Label>
            </td>
            <td class="columnseparator"></td>
            <td>
                <asp:TextBox ID="txtVehicleID" runat="server" ReadOnly="true"></asp:TextBox>
            </td>
            <td class="columnseparator"></td>
            <td>
                <asp:Label runat="server" Text="VehicleNo"></asp:Label>
            </td>
            <td class="columnseparator"></td>
            <td>
                <asp:TextBox ID="txtVehicleNo" runat="server" ReadOnly="true"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="rowseparator"></td>
        </tr>
        <tr>
            <td>
                <asp:Label runat="server" Text="District"></asp:Label>
            </td>
            <td class="columnseparator"></td>
            <td>
                <asp:TextBox ID="txtDistrict" runat="server" ReadOnly="true"></asp:TextBox>
            </td>
            <td class="columnseparator"></td>
            <td>
                <asp:Label runat="server" Text="InvIssID"></asp:Label>
            </td>
            <td class="columnseparator"></td>
            <td>
                <asp:TextBox ID="txtInvReqID" runat="server" ReadOnly="true"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="rowseparator"></td>
        </tr>
        <tr>
            <td>
                <asp:Label runat="server" Text="DC No"></asp:Label>
            </td>
            <td class="columnseparator"></td>
            <td>
                <asp:TextBox ID="txtDCNumber" runat="server" ReadOnly="true" MaxLength="10"></asp:TextBox>
            </td>
            <td class="columnseparator"></td>
            <td>
                <asp:Label runat="server" Text="DC Date"></asp:Label>
            </td>
            <td class="columnseparator"></td>
            <td>
                <asp:TextBox ID="txtDCDate" runat="server" ReadOnly="true" oncut="return false;"
                             onpaste="return false;" oncopy="return false;">
                </asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="rowseparator"></td>
        </tr>
        <tr>
            <td>
                <asp:Label runat="server" Text="CourierName"></asp:Label>
            </td>
            <td class="columnseparator"></td>
            <td>
                <asp:TextBox ID="txtCourierName" runat="server" ReadOnly="true" MaxLength="20"></asp:TextBox>
            </td>
            <td class="columnseparator"></td>
            <td>
                <asp:Label runat="server" Text="HO Remarks"></asp:Label>
            </td>
            <td class="columnseparator"></td>
            <td>
                <asp:TextBox ID="txtRemarks" runat="server" ReadOnly="true" MaxLength="20"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="rowseparator"></td>
        </tr>
        <tr>
            <td>
                <asp:Label runat="server" Text="ReceiptDate"></asp:Label>
                <span
                    style="color: Red">
                    *
                </span>
            </td>
            <td class="columnseparator"></td>
            <td>
                <asp:TextBox ID="txtReceiptDate" runat="server" onkeypress="return false" MaxLength="15"
                             oncut="return false;" onpaste="return false;" oncopy="return false;">
                </asp:TextBox>
                <ajaxToolKit:CalendarExtender runat="server" TargetControlID="txtReceiptDate" Format="MM/dd/yyyy"
                                              PopupButtonID="ImageButton1">
                </ajaxToolKit:CalendarExtender>
                <asp:ImageButton ID="ImageButton1" runat="server" alt="" src="images/Calendar.gif"
                                 Style="vertical-align: top"/>
            </td>
            <td class="columnseparator"></td>
            <td>
                <asp:Label runat="server" Text="Remarks"></asp:Label>
                <span
                    style="color: Red">
                    *
                </span>
            </td>
            <td class="columnseparator"></td>
            <td>
                <asp:TextBox ID="txtReceiptRemarks" runat="server" MaxLength="50" TextMode="MultiLine"
                             onKeyUp="CheckLength(this,50)" onChange="CheckLength(this,50)">
                </asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="rowseparator"></td>
        </tr>
        <tr>
            <td>
                <asp:Label runat="server" Text="InvoiceNo"></asp:Label>
                <span style="color: Red">
                    *
                </span>
            </td>
            <td class="columnseparator"></td>
            <td>
                <asp:TextBox ID="txtInvoiceNo" runat="server" MaxLength="5"></asp:TextBox>
            </td>
            <td class="columnseparator"></td>
            <td>
                <asp:Label ID="lbInvoiceDate" runat="server" Text="InvoiceDate"></asp:Label>
                <span
                    style="color: Red">
                    *
                </span>
            </td>
            <td class="columnseparator"></td>
            <td>
                <asp:TextBox ID="txtInvoiceDate" runat="server" MaxLength="15" onkeypress="return false"
                             oncut="return false;" onpaste="return false;" oncopy="return false;">
                </asp:TextBox>
                <ajaxToolKit:CalendarExtender runat="server" TargetControlID="txtInvoiceDate" Format="MM/dd/yyyy"
                                              PopupButtonID="ImageButton2">
                </ajaxToolKit:CalendarExtender>
                <asp:ImageButton ID="ImageButton2" runat="server" alt="" src="images/Calendar.gif"
                                 Style="vertical-align: top"/>
            </td>
        </tr>
        <tr>
            <td class="rowseparator"></td>
        </tr>
        <tr>
            <td align="center" colspan="7">
                <asp:GridView ID="gvReceiptDetails" runat="server" BackColor="#DEBA84" BorderColor="#DEBA84"
                              BorderStyle="None" BorderWidth="1px" CellPadding="3" CellSpacing="2" AutoGenerateColumns="false"
                              OnRowDataBound="gvReceiptDetails_RowDataBound">
                    <RowStyle CssClass="rowStyleGrid"/>
                    <FooterStyle CssClass="footerStylegrid"/>
                    <Columns>
                        <asp:BoundField HeaderText="SparePartName" DataField="SparePart_Name"/>
                        <asp:BoundField HeaderText="RequestedQty" DataField="RequestedQty"/>
                        <asp:BoundField HeaderText="IssuedQty" DataField="IssuedQty"/>
                        <asp:TemplateField HeaderText="ReceivedQty">
                            <ItemTemplate>
                                <asp:TextBox ID="txtReceivedQty" runat="server" onkeypress="return isNumberKey(event);"
                                             MaxLength="4">
                                </asp:TextBox>
                                <asp:Label ID="LbDetID" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "SparePartIssueDetID") %>'
                                           Visible="false">
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <PagerStyle CssClass="pagerStylegrid"/>
                    <SelectedRowStyle CssClass="selectedRowStyle"/>
                    <HeaderStyle CssClass="headerStyle"/>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td class="rowseparator"></td>
        </tr>
        <tr>
            <td align="center" colspan="7">
                <asp:Button ID="btReceive" runat="server" Text="Received" OnClick="btReceive_Click"/>
                <asp:Button runat="server" Text="Cancel" OnClick="btCancel_Click"/>
            </td>
        </tr>
        <tr>
            <td class="rowseparator"></td>
        </tr>
    </table>
</fieldset>
</asp:Panel>
</td>
</tr>
</table>
</fieldset>
</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>