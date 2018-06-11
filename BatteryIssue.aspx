<%@ Page AutoEventWireup="true" CodeFile="BatteryIssue.aspx.cs" Inherits="BatteryIssue" Language="C#" MasterPageFile="~/temp.master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <script type="text/javascript">
        function validation() {
            var dcNumberPopup = document.getElementById('<%= txtDcNumberPopup.ClientID %>');
            var dcDate = document.getElementById('<%= txtDcDate.ClientID %>');
            var courierName = document.getElementById('<%= txtCourierName.ClientID %>');
            var remarks = document.getElementById('<%= txtRemarks.ClientID %>');
            if (!RequiredValidation(dcNumberPopup, "DC Number cannot be Blank"))
                return false;
            if (!RequiredValidation(dcDate, "DC Date cannot be Blank"))
                return false;
            if (trim(dcDate.value) !== "") {
                if (!isValidDate(dcDate.value)) {
                    alert("Enter the DC Date");
                    dcDate.focus();
                    return false;
                }
            }
            var now = new Date();
            if (Date.parse(dcDate.value) > Date.parse(now)) {
                alert("DC Date should not be greater than Current Date");
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
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div runat="server">
                <fieldset style="padding: 10px">
                    <legend>Battery Issue</legend>
                    <div runat="server" align="center">
                        Vehicle Number<span style="color: Red">*</span>
                        <ajaxToolkit:ComboBox AutoCompleteMode="Append" ID="ddlInventoryBatteryIssueVehicles" runat="server" AutoPostBack="True"
                                              OnSelectedIndexChanged="ddlInventoryBatteryIssueVehicles_SelectedIndexChanged" DropDownStyle="DropDownList">
                        </ajaxToolkit:ComboBox>
                    </div>
                    <br/>
                    <div runat="server">
                        Pending for Issue
                    </div>
                    <div runat="server" align="center">
                        <asp:GridView ID="grvBatteryPendingForIssue" runat="server" CellPadding="3"
                                      EmptyDataText="Details are not available" AutoGenerateColumns="False"
                                      CssClass="gridviewStyle" AllowPaging="True" DataKeyNames="FleetInventoryReqID"
                                      PageSize="5" OnPageIndexChanging="grvBatteryPendingForIssue_PageIndexChanging"
                                      OnRowCommand="grvBatteryPendingForIssue_RowCommand" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px">
                            <Columns>
                                <asp:BoundField HeaderText="Vehicle Number" DataField="VehicleNum"/>
                                <asp:BoundField HeaderText="No. of Batteries" DataField="RequestedQty"/>
                                <asp:BoundField HeaderText="Request Date" DataField="RequestedDate" DataFormatString="{0:dd/MM/yyyy}"/>
                                <asp:BoundField HeaderText="Request By" DataField="RequestedBy"/>
                                <asp:TemplateField ControlStyle-Width="50px" HeaderStyle-Width="60px">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnViewDetails" runat="server" Text="Issue" ToolTip="Click here to Issue the details"
                                                        OnClick="BtnViewDetails_Click" CssClass="button2" RowIndex="<%# Container.DisplayIndex %>"
                                                        CommandName="Show" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "FleetInventoryReqID") %>'/>
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
                    </div>
                    <asp:Button ID="btnShowPopup" runat="server" Style="display: none"/>
                    <ajaxToolkit:ModalPopupExtender ID="gv_ModalPopupExtenderBatteryIssue" BehaviorID="mdlPopup"
                                                    runat="server" TargetControlID="btnShowPopup" PopupControlID="pnlPopup" BackgroundCssClass="modalBackground"/>
                    <asp:Panel ID="pnlPopup" runat="server" CssClass="modalPanel">
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <fieldset style="padding: 10px">
                                    <legend>Battery Issue Detail</legend>
                                    <table class="style1">
                                        <tr>
                                            <td align="left">
                                                Vehicle ID
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtBatIssVehicleID" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                Inventory Request ID
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtInvReqIdPopUp" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                            <td align="left">
                                                DC Number
                                            </td>
                                            <td align="left">
                                            <asp:TextBox ID="txtDcNumberPopup" runat="server" MaxLength="5"></asp:TextBox>
                                        </tr>
                                        <tr align="center">
                                            <td align="left">
                                                DC Date
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtDcDate" runat="server" onkeypress="return false" MaxLength="20"
                                                             oncut="return false;" onpaste="return false;" oncopy="return false;">
                                                </asp:TextBox>
                                                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtDcDate"
                                                                              Format="dd/MM/yyyy" PopupButtonID="ImageButton1">
                                                </ajaxToolkit:CalendarExtender><asp:ImageButton ID="ImageButton1" runat="server" alt="" src="images/Calendar.gif" Style="vertical-align: top"/>
                                            </td>
                                            <td align="left">
                                                Courier Name
                                            </td>
                                            <td align="left">
                                            <asp:TextBox ID="txtCourierName" runat="server" MaxLength="20"></asp:TextBox>
                                        </tr>
                                    </table>
                                    <br/>
                                    <asp:GridView ID="grvBatteryIssueDetailsPopup" runat="server" AutoGenerateColumns="False" CssClass="gridviewStyle" CellPadding="3" Width="95%"
                                                  OnRowDataBound="grvBatteryIssueDetailsPopup_RowDataBound" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px">
                                        <Columns>
                                            <asp:BoundField HeaderText="Battery Position ID" DataField="BatteryPositionID"/>
                                            <asp:BoundField HeaderText="Old Battery Number" DataField="BatteryNumber"/>
                                            <asp:TemplateField HeaderText="New Battery Number">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddlNewBatteryNumber" runat="server">
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="RequestedQty" DataField="RequestedQty"/>

                                            <asp:TemplateField HeaderText="IssuedQty">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtBatteryIssuedQty" runat="server" onkeypress="return isNumberKey(event);"
                                                                 MaxLength="20">
                                                    </asp:TextBox>
                                                </ItemTemplate>
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
                                    <div id="Div7" align="center" style="background-color: white; width: 95%;">
                                        Remarks
                                        <asp:TextBox ID="txtRemarks" runat="server" MaxLength="20" TextMode="MultiLine"
                                                     onKeyUp="CheckLength(this,50)" onChange="CheckLength(this,50)">
                                        </asp:TextBox>
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btnOk" runat="server" Text="Issue" Width="50px" OnClick="btnOk_Click"/>
                                        &nbsp;&nbsp;
                                        <asp:Button runat="server" Text="Cancel" Width="50px" OnClick="btnNo_Click"/>
                                    </div>
                                </fieldset>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:Panel>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>