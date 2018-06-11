<%@ Page Language="C#" MasterPageFile="~/temp.master" AutoEventWireup="true" CodeFile="SparePartsRequisiton.aspx.cs" Inherits="SparePartsRequisiton" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolKit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

<script type="text/javascript">
    function validation() {
        var id = document.getElementById('<%= ddlVehicles.ClientID %>');
        var inputs = id.getElementsByTagName('input');
        var i;
        for (i = 0; i < inputs.length; i++) {
            switch (inputs[i].type) {
            case 'text':
                if (inputs[i].value !== "" && inputs[i].value != null && inputs[i].value === "--Select--") {
                    alert('Select the Vehicle');
                    return false;
                }
                break;
            }
        }
        return true;
    }

    function OnlyNumbers(evt) {
        var charCode = (evt.which) ? evt.which : event.keyCode;
        if (charCode > 31 && (charCode < 48 || charCode > 57))
            return false;
        else
            return true;
    }
</script>
<asp:UpdatePanel runat="server">
<ContentTemplate>
<fieldset style="padding: 10px;">
<legend align="center" style="color: brown">Spare Parts Requisiton</legend>
<table style="width: 100%">
<tr align="center">
    <td>
        <asp:Panel ID="pnlSparePartsRequisition" runat="server">
            <table>
                <tr>
                    <td class="rowseparator"></td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Label runat="server" Text="Vehicles"></asp:Label>
                        <span style="color: Red">*</span>
                    </td>
                    <td class="columnseparator"></td>
                    <td>
                        <ajaxToolKit:ComboBox AutoCompleteMode="Append" ID="ddlVehicles" style="margin-bottom: 15px" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlVehicles_SelectedIndexChanged" DropDownStyle="DropDownList">
                        </ajaxToolKit:ComboBox>
                    </td>
                </tr>
                <tr>
                    <td class="rowseparator"></td>
                </tr>
                <tr>
                    <td colspan="3" align="center">
                        <asp:GridView ID="grvNewSparePartsRequisition" runat="server" AutoGenerateColumns="False" style="margin-top: 30px"
                                      OnRowDeleting="grvNewSparePartsRequisition_RowDeleting" CellPadding="3" CssClass="gridviewStyle" OnSelectedIndexChanged="grvNewSparePartsRequisition_SelectedIndexChanged" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px">
                            <Columns>
                                <asp:BoundField HeaderText="S.No" DataField="SNo"/>
                                <asp:TemplateField HeaderText="Spare Part Name">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlSparePartName" runat="server">
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Quantity">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtQuantity" runat="server" Width="75" Text='<%# Eval("Quantity") %>'
                                                     MaxLength="3" onkeypress="return OnlyNumbers(event);">
                                        </asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ControlStyle-Width="50px" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Button ID="btnDelete" runat="server" Text="Delete" ToolTip='<%# Eval("SNo") %>'
                                                    CssClass="button2" OnClick="BtnDelete_Click"/>
                                    </ItemTemplate>
                                    <ControlStyle Width="50px"/>
                                    <HeaderStyle Width="60px"/>
                                    <ItemStyle HorizontalAlign="Center"/>
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
                    <td class="rowseparator"></td>
                </tr>
                <tr>
                    <td colspan="3" align="center">
                        <asp:Button runat="server" Text="Add Row" CssClass="form-submit-button" OnClick="btnAddRow_Click"/>
                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="form-submit-button" OnClick="btnSubmit_Click"/>
                        <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="form-submit-button" OnClick="btnReset_Click"/>
                        <asp:Button runat="server" CssClass="form-submit-button" Text="View History" OnClick="btnSparePartsReqHistory_Click" OnClientClick="return validation();"/>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </td>
</tr>
<tr>
    <td>
        <table style="margin-top: 50px">

        <tr>
            <td align="center">
                <asp:Label runat="server" Text="Requisitions Pending For Issue"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="rowseparator"></td>
        </tr>
        <tr>
            <td align="center">
                <asp:GridView ID="grvPendingforApproval" runat="server" AutoGenerateColumns="False" HorizontalAlign="Center"
                              EmptyDataText="Details are not available" DataKeyNames="FleetInventoryReqID"
                              CellPadding="3" CssClass="gridview" OnRowCommand="grvPendingforApproval_RowCommand" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px">
                    <Columns>
                        <asp:BoundField HeaderText="Row_ID" DataField="Row_ID"/>
                        <asp:BoundField HeaderText="Total Spare Parts Quantity" DataField="RequestedQty"/>
                        <asp:BoundField HeaderText="Requested Date" DataField="RequestedDate" DataFormatString="{0:MM/dd/yyyy}"/>
                        <asp:BoundField HeaderText="Requested By" DataField="RequestedBy"/>
                        <asp:BoundField HeaderText="Status" DataField="Status"/>
                        <asp:TemplateField ControlStyle-Width="50px" HeaderStyle-Width="60px">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnViewDetails" runat="server" Text="View" ToolTip="Click here to Approve/Reject the details"
                                                CssClass="button2" OnClick="btnViewDetails_Click" RowIndex="<%# Container.DisplayIndex %>"
                                                CommandName="Show" CommandArgument='<%#DataBinder.Eval(Container.DataItem,"FleetInventoryReqID") %>'/>
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
            <td class="rowseparator"></td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="RequisitionHistory" runat="server">
                    <table>
                        <tr>
                            <td>
                                <legend style="margin-top: 40px" align="center">Requisition History </legend>
                            </td>
                        </tr>
                        <caption>
                            <br/>
                            <tr>
                                <td>
                                    <asp:GridView ID="grvRequisitionHistory" HorizontalAlign="Center" runat="server" AllowPaging="True" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" CssClass="gridviewStyle" EmptyDataText="No History Found" OnPageIndexChanging="grvRequisitionHistory_PageIndexChanging" PageSize="5" Width="95%">
                                        <RowStyle CssClass="rowStyleGrid" ForeColor="#000066"/>
                                        <FooterStyle BackColor="White" CssClass="footerStylegrid" ForeColor="#000066"/>
                                        <PagerStyle BackColor="White" CssClass="pagerStylegrid" ForeColor="#000066" HorizontalAlign="Left"/>
                                        <SelectedRowStyle BackColor="#669999" CssClass="selectedRowStyle" Font-Bold="True" ForeColor="White"/>
                                        <HeaderStyle BackColor="#006699" CssClass="headerStyle" Font-Bold="True" ForeColor="White"/>
                                        <SortedAscendingCellStyle BackColor="#F1F1F1"/>
                                        <SortedAscendingHeaderStyle BackColor="#007DBB"/>
                                        <SortedDescendingCellStyle BackColor="#CAC9C9"/>
                                        <SortedDescendingHeaderStyle BackColor="#00547E"/>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </caption>
                    </table>
                    <asp:Button ID="hideHistory" runat="server" CssClass="form-submit-button" Text="Hide History" Visible="false" OnClick="hideHistory_Click"/>
                </asp:Panel>
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
        <asp:Panel ID="pnlPopup" runat="server" Style="display: none; padding: 10px" CssClass="modalPanel">
            <asp:UpdatePanel runat="server">
                <ContentTemplate>

                    <table style="border: 1px; margin-left: 700px; margin-top: 290px;">
                        <tr>
                            <td>
                                <legend align="center">Request Details</legend>
                            </td>
                        </tr>
                        <br/>

                        <tr>
                            <td >
                                <asp:Label runat="server" Text="VehicleNum"/>
                            </td>
                            <td >
                                <asp:TextBox ID="txtVehicleNumber" runat="server" ReadOnly="true"></asp:TextBox>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <asp:Label runat="server" Text="InvReqID"/>
                            </td>
                            <td >
                                <asp:TextBox ID="txtReqID" runat="server" ReadOnly="true"></asp:TextBox>
                            </td>
                        </tr>

                        <tr>
                            <td colspan="3">
                                <asp:GridView ID="grvBatteryRequestDetails" HorizontalAlign="Center" runat="server" align="left" AutoGenerateColumns="true"
                                              CellPadding="3" CellSpacing="2" CssClass="gridviewStyle" GridLines="None" Width="400px">
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
                            <td >
                                <asp:Button ID="btnOk" runat="server" CssClass="form-submit-button" OnClick="btnOk_Click" Text="Approve" Width="70px"/>
                                <ajaxToolKit:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" ConfirmText="Are you sure You want to Approve?"
                                                                   TargetControlID="btnOk">
                                </ajaxToolKit:ConfirmButtonExtender>
                                <asp:Button ID="btnNo" CssClass="form-submit-button" runat="server" OnClick="btnNo_Click" Text="Reject" Width="70px"/>
                                <ajaxToolKit:ConfirmButtonExtender ID="ConfirmButtonExtender2" runat="server" ConfirmText="Are you sure You want to Reject?"
                                                                   TargetControlID="btnNo">
                                </ajaxToolKit:ConfirmButtonExtender>
                                <asp:Button CssClass="form-submit-button" Width="70px" runat="server" OnClick="btnCancel_Click" Text="Cancel"/>
                            </td>
                        </tr>
                        <tr>
                            <td class="rowseparator"></td>
                        </tr>

                    </table>
                </fieldset>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
        <tr>
            <td class="rowseparator"></td>
        </tr>
    </td>
</tr>
</table>
</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>