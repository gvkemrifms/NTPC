<%@ Page Language="C#" MasterPageFile="~/temp.master" AutoEventWireup="true" CodeFile="SparePartIssue.aspx.cs" Inherits="SparePartIssue" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolKit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<script type="text/javascript">

    function validation() {

        var vehicleId = document.getElementById('<%= ddlVehicles.ClientID %>');

        var dcNumber = document.getElementById('<%= txtDCNumber.ClientID %>');

        var dcDate = document.getElementById('<%= txtDCDate.ClientID %>');

        var courierName = document.getElementById('<%= txtCourierName.ClientID %>');

        var remarks = document.getElementById('<%= txtRemarks.ClientID %>');

        if (!RequiredValidation(dcNumber, "DC Number Cannot be Blank"))
            return false;

        if (!RequiredValidation(dcDate, "DC Date cannot be Blank"))
            return false;

        switch (vehicleId.selectedIndex) {
        case 0:
            alert("Please select Vehicle");
            vehicleId.focus();
            return false;
        }


        if (trim(dcDate.value) !== "") {
            if (!isValidDate(dcDate.value)) {
                alert("Enter the Valid Date");
                dcDate.focus();
                return false;
            }
        }

        var now = new Date();
        if (Date.parse(dcDate.value) > Date.parse(now)) {
            alert("DCDate should not be greater than Current Date");
            dcDate.focus();
            return false;
        }

        if (!RequiredValidation(courierName, "Courier Name cannot be Blank"))
            return false;

        if (!RequiredValidation(remarks, "Remarks cannot be Blank"))
            return false;

        var txtIssuedQuantity = document.getElementById(window.IssuedQuantity);
        if (!RequiredValidation(txtIssuedQuantity, "Issue Qty cannot be Blank"))
            return false;
        return true;
    }

    function ValidateIssueQty(issQtyId, reqQty) {
        var objIssQty = document.getElementById(issQtyId);
        if (parseInt(objIssQty.value) > parseInt(reqQty)) {
            alert("Issue Quantity Cannot be more than Request Qunatity");
            objIssQty.focus();
            return false;
        }
        return true;
    }


</script>
<asp:UpdatePanel ID="UpdPanel1" runat="server">
<ContentTemplate>
<fieldset style="padding: 10px">
<legend align="center" style="color: brown">Spare Parts Issue</legend>
<table style="width: 100%">
<tr>
    <td class="rowseparator"></td>
</tr>
<tr>
    <td align="center">
        <asp:Label ID="lb_Vehicles" runat="server" Text="Vehicles"></asp:Label>
        <span style="color: Red">*</span>&nbsp;
        <ajaxToolKit:ComboBox AutoCompleteMode="Append" ID="ddlVehicles" runat="server"
                              AutoPostBack="True" OnSelectedIndexChanged="ddlVehicles_SelectedIndexChanged"
                              DropDownStyle="DropDownList">
        </ajaxToolKit:ComboBox>
    </td>
</tr>
<tr>
    <td align="center">
        <asp:GridView ID="gvApprovedRequisition" runat="server" CellPadding="3"
                      EmptyDataText="Details are not available" CssClass="gridviewStyle"
                      OnPageIndexChanging="gvApprovedRequisition_PageIndexChanging" AutoGenerateColumns="False"
                      OnRowCommand="gvApprovedRequisition_RowCommand" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px">
            <Columns>
                <asp:BoundField HeaderText="VehicleNo" DataField="VehicleNum"/>
                <asp:BoundField HeaderText="DistrictID" DataField="DistrictID"/>
                <asp:BoundField HeaderText="No. of Spare Parts" DataField="RequestedQty"/>
                <asp:BoundField HeaderText="Requested By" DataField="RequestedBy"/>
                <asp:TemplateField ControlStyle-Width="50px" HeaderStyle-Width="60px">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnViewDetails" runat="server" Text="Issue" ToolTip="Click here to Issue the details"
                                        RowIndex="<%# Container.DisplayIndex %>" CommandName="Show" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "FleetInventoryReqID") %>'/>
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
        <ajaxToolKit:ModalPopupExtender ID="gv_ModalPopupExtender1" BehaviorID="mdlPopup"
                                        runat="server" TargetControlID="btnShowPopup" PopupControlID="pnlPopup" BackgroundCssClass="modalBackground"/>
    </td>
</tr>
<tr>
    <td class="rowseparator"></td>
</tr>
<tr>
    <td align="center">
        <asp:Panel ID="pnlPopup" runat="server" CssClass="modalPanel" Style="display: none; padding: 10px;">
            <fieldset style="padding: 10px;">
                <legend>Issue Details</legend>
                <table>
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
                            <asp:Label runat="server" Text="InvReqID"></asp:Label>
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
                            <span style="color: Red">
                                *
                            </span>
                        </td>
                        <td class="columnseparator"></td>
                        <td>
                            <asp:TextBox ID="txtDCNumber" runat="server" MaxLength="5"></asp:TextBox>
                        </td>
                        <td class="columnseparator"></td>
                        <td>
                            <asp:Label runat="server" Text="DC Date"></asp:Label>
                            <span style="color: Red">
                                *
                            </span>
                        </td>
                        <td class="columnseparator"></td>
                        <td>
                            <asp:TextBox ID="txtDCDate" runat="server" onkeypress="return false" MaxLength="20"
                                         oncut="return false;" onpaste="return false;" oncopy="return false;">
                            </asp:TextBox>
                            <ajaxToolKit:CalendarExtender ID="ccl1" runat="server" TargetControlID="txtDCDate"
                                                          Format="MM/dd/yyyy" PopupButtonID="ImageButton1">
                            </ajaxToolKit:CalendarExtender>
                            <asp:ImageButton ID="ImageButton1" runat="server" alt="" src="images/Calendar.gif"
                                             Style="vertical-align: top"/>
                        </td>
                    </tr>
                    <tr>
                        <td class="rowseparator"></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" Text="CourierName"></asp:Label>
                            <span
                                style="color: Red">
                                *
                            </span>
                        </td>
                        <td class="columnseparator"></td>
                        <td>
                            <asp:TextBox ID="txtCourierName" runat="server" MaxLength="20"></asp:TextBox>
                        </td>
                        <td class="columnseparator"></td>
                        <td>
                            <asp:Label runat="server" Text="Remarks"></asp:Label>
                            <span style="color: Red">
                                *
                            </span>
                        </td>
                        <td class="columnseparator"></td>
                        <td>
                            <asp:TextBox ID="txtRemarks" runat="server" MaxLength="20" TextMode="MultiLine" onKeyUp="CheckLength(this,50)"
                                         onChange="CheckLength(this,50)">
                            </asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="rowseparator"></td>
                    </tr>
                    <tr>
                        <td class="rowseparator"></td>
                    </tr>
                    <tr>
                        <td align="center" colspan="7">
                            <asp:GridView ID="gvIssueDetails" runat="server" GridLines="None" CssClass="gridviewStyle"
                                          CellPadding="3" CellSpacing="2" AutoGenerateColumns="false" OnRowDataBound="gvIssueDetails_RowDataBound">
                                <Columns>
                                    <asp:BoundField HeaderText="SparePartName" DataField="SparePart_Name"/>
                                    <asp:BoundField HeaderText="RequestedQty" DataField="RequestedQty"/>
                                    <asp:TemplateField HeaderText="IssuedQty">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtIssuedQty" runat="server" onkeypress="return isNumberKey(event);"
                                                         MaxLength="4">
                                            </asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <RowStyle CssClass="rowStyleGrid"/>
                                <FooterStyle CssClass="footerStylegrid"/>
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
                        <td colspan="7" align="center">
                            <asp:Button ID="btIssue" runat="server" Text="Issue" OnClick="btIssue_Click"/>
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
<tr>
    <td class="rowseparator"></td>
</tr>
</table>
</fieldset>
</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>