<%@ Page Language="C#" MasterPageFile="~/temp.master" AutoEventWireup="true" CodeFile="TyreRequisition.aspx.cs" Inherits="TyreRequisition" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolKit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<script type="text/javascript">
    function validationInventoryBatteryVehicleType() {
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
</script>
<asp:UpdatePanel ID="UpdPnl1" runat="server">
<ContentTemplate>
<fieldset style="padding: 10px">
    <legend>Tyre Requisition</legend>
    <table align="center">
    <tr>
        <td class="rowseparator"></td>
    </tr>
    <tr align="center">
        <td>
            <asp:Panel ID="pnlNewTyreRequisition" runat="server">
                <table>
                    <tr>
                        <td class="rowseparator"></td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="lbVehicles" runat="server" Text="Vehicles"></asp:Label>
                            <span style="color: Red">*</span>
                            <ajaxToolKit:ComboBox AutoCompleteMode="Append" ID="ddlVehicles" runat="server"
                                                  AutoPostBack="True" OnSelectedIndexChanged="ddlVehicles_SelectedIndexChanged" DropDownStyle="DropDownList">
                            </ajaxToolKit:ComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="rowseparator"></td>
                    </tr>
                    <caption>
                        <br/>
                        <tr>
                            <td>
                                <asp:GridView ID="gvTyreRequisition" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" CssClass="table table-striped table-bordered table-hover" EmptyDataText="Details are not available" OnPageIndexChanging="gvTyreRequisition_PageIndexChanging">
                                    <Columns>
                                        <asp:TemplateField HeaderText="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chk" runat="server" Enabled='<%# !Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"Enabled") == DBNull.Value ? 0 : 1) %>'/>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center"/>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="TyrePosition" HeaderText="Tyre Position"/>
                                        <asp:BoundField DataField="TyreNumber" HeaderText="TyreNumber"/>
                                        <asp:BoundField DataField="TyreMake" HeaderText="TyreMake"/>
                                        <asp:BoundField DataField="TyreModelSize" HeaderText="TyreModelSize"/>
                                        <asp:BoundField DataField="IssueDate" HeaderText="IssuedDate"/>
                                        <asp:BoundField DataField="IssueODOReading" HeaderText="Total Km Run"/>
                                        <asp:TemplateField HeaderText="Remarks">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtRemarks" runat="server" Enabled='<%# !Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"Enabled") == DBNull.Value ? 0 : 1) %>' MaxLength="50" onChange="CheckLength(this,50)" onkeypress="return remark(event);" onKeyUp="CheckLength(this,50)" Text='<%# DataBinder.Eval(Container.DataItem,"Enabled") %>' TextMode="MultiLine">
                                                </asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <RowStyle CssClass="rowStyleGrid" ForeColor="#000066"/>
                                    <FooterStyle BackColor="White" CssClass="footerStylegrid" ForeColor="#000066"/>
                                    <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White"/>
                                    <PagerStyle BackColor="White" CssClass="pagerStylegrid" ForeColor="#000066" HorizontalAlign="Left"/>
                                    <SelectedRowStyle BackColor="#669999" CssClass="selectedRowStyle" Font-Bold="True" ForeColor="White"/>
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
                            <td align="center">
                                <asp:Button ID="btSave" runat="server" CssClass="form-submit-button" OnClick="btSave_Click" Text="Submit"/>
                                <asp:Button runat="server" CssClass="form-submit-button" OnClick="btCancel_Click" Text="Cancel"/>
                                <asp:Button runat="server" CssClass="form-submit-button" OnClick="btnTyreReqHistory_Click" OnClientClick="return validationInventoryBatteryVehicleType();" Text="View History"/>
                            </td>
                        </tr>
                    </caption>
                </table>
            </asp:Panel>
        </td>
    </tr>
    <tr>
    <td>
    <table>
        <tr>
            <td class="rowseparator"></td>
        </tr>
        <caption>
        <br/>
        <tr>
            <td>
                <asp:GridView ID="gvTyrePendingForApproval" runat="server" AllowPaging="True" AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" CssClass="gridviewStyle" OnPageIndexChanging="gvTyrePendingForApproval_PageIndexChanging" OnRowCommand="gvTyrePendingForApproval_RowCommand" PageSize="5">
                    <Columns>
                        <asp:BoundField DataField="VehicleNum" HeaderText="Vehicle Number"/>
                        <asp:BoundField DataField="RequestedQty" HeaderText="No. of Tyres"/>
                        <asp:BoundField DataField="RequestedDate" DataFormatString="{0:MM/dd/yyyy}" HeaderText="Request Date"/>
                        <asp:BoundField DataField="RequestedBy" HeaderText="Request By"/>
                        <asp:BoundField DataField="Status" HeaderText="Status"/>
                        <asp:TemplateField ControlStyle-Width="50px" HeaderStyle-Width="60px">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnViewDetails" runat="server" CommandArgument='<%#DataBinder.Eval(Container.DataItem,"FleetInventoryReqID") %>' CommandName="Show" CssClass="button2" OnClick="BtnViewDetails_Click" RowIndex="<%# Container.DisplayIndex %>" Text="View" ToolTip="Click here to Approve/Reject the details"/>
                            </ItemTemplate>
                            <ControlStyle Width="50px"/>
                            <HeaderStyle Width="60px"/>
                        </asp:TemplateField>
                    </Columns>
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
        <tr>
            <td class="rowseparator"></td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="RequisitionHistory" runat="server">
                    <fieldset style="padding: 10px; width: auto">
                        <legend>Requisition History </legend>
                        <table>
                            <tr>
                                <td>
                                    <asp:GridView ID="grvRequisitionHistory" runat="server" AllowPaging="True" AutoGenerateColumns="True" CellPadding="3" CellSpacing="2" CssClass="gridviewStyle" EmptyDataText="No History Found" GridLines="None" OnPageIndexChanging="grvRequisitionHistory_PageIndexChanging" PageSize="5" Width="95%">
                                        <RowStyle CssClass="rowStyleGrid"/>
                                        <FooterStyle CssClass="footerStylegrid"/>
                                        <PagerStyle CssClass="pagerStylegrid"/>
                                        <SelectedRowStyle CssClass="selectedRowStyle"/>
                                        <HeaderStyle CssClass="headerStyle"/>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                        <asp:Button ID="hideHistory" runat="server" OnClick="hideHistory_Click" Text="Hide History" Visible="false"/>
                    </fieldset>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td class="rowseparator"></td>
        </tr>
        <tr>
            <td>
            <asp:Button ID="btnShowPopup" runat="server" Style="display: none"/>
            <ajaxToolKit:ModalPopupExtender ID="gv_ModalPopupExtender1" runat="server" BackgroundCssClass="modalBackground" BehaviorID="mdlPopup" PopupControlID="pnlPopup" TargetControlID="btnShowPopup"/>
            <asp:Panel ID="pnlPopup" runat="server" CssClass="modalPanel" Style="display: block; padding: 30px; position: inherit;" Width="500px">
            <fieldset style="padding: 10px; width: auto">
            <table style="margin-left: 480px; margin-top: 180px">
            <td>
                <asp:Label runat="server" Text="VehicleNo"></asp:Label>
            </td>
            <td class="columnseparator"></td>
            <td>
                <asp:TextBox ID="txtVehicleNo" runat="server" ReadOnly="true"></asp:TextBox>
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
            <td align="center" colspan="7">
                <asp:GridView ID="gvTyreRequisitionDetails" runat="server" AutoGenerateColumns="true" CellPadding="3" CellSpacing="2" CssClass="gridviewStyle" GridLines="None">
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
            <td align="center" colspan="7">
                <asp:Button ID="btnOk" runat="server" OnClick="btnOk_Click" Text="Approve"/>
                <ajaxToolKit:ConfirmButtonExtender runat="server" ConfirmText="Are you sure you want to APPROVE" TargetControlID="btnOk">
                </ajaxToolKit:ConfirmButtonExtender>
                <asp:Button ID="btnNo" runat="server" OnClick="btnNo_Click" Text="Reject"/>
                <ajaxToolKit:ConfirmButtonExtender runat="server" ConfirmText="Are you sure you want to REJECT" TargetControlID="btnNo">
                </ajaxToolKit:ConfirmButtonExtender>
                <asp:Button runat="server" OnClick="btnCancel_Click" Text="Cancel"/>
            </td>
            &gt;
        </tr>
    </table>
</fieldset>
</asp:Panel>
</td>
</tr>
<tr>
    <td class="rowseparator"></td>
</tr>
</caption>
</table>
</td>
</tr>
</table>
</fieldset>
</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>