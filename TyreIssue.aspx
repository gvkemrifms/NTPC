<%@ Page Language="C#" MasterPageFile="~/temp.master" AutoEventWireup="true" CodeFile="TyreIssue.aspx.cs" Inherits="TyreIssue" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script language="javascript" type="text/javascript">
        function validation() {
            var tyreCost = document.getElementById('<%= txtTyreCost.ClientID %>');
            var dcNumPopup = document.getElementById('<%= txtDcNumberPopup.ClientID %>');
            var dcDate = document.getElementById('<%= txtDcDate.ClientID %>');
            var courierName = document.getElementById('<%= txtCourierName.ClientID %>');
            if (!RequiredValidation(tyreCost, "Tyre Cost cannot be Blank"))
                return false;
            if (!RequiredValidation(dcNumPopup, "DC Number cannot be Blank"))
                return false;
            if (!RequiredValidation(dcDate, "DC Date cannot be Blank"))
                return false;
            if (trim(dcDate.value) !== "" && !isValidDate(dcDate.value)) {
                alert("Enter the DC Date");
                dcDate.focus();
                return false;
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
            return true;
        }

        function ValidateIssueQty(issQtyId, reqQty) {
            var objIssQty = document.getElementById(issQtyId);
            if (parseInt(objIssQty.value) > parseInt(reqQty)) {
                alert("Issue Quantity Cannot be more than Request Quantity");
                objIssQty.focus();
                return false;
            }
            return true;
        }
    </script>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <script type="text/javascript">
                function pageLoad() {
                    $('#<%=txtDcDate.ClientID%>').datepicker({
                        dateFormat: 'mm/dd/yy',
                        changeMonth: true,
                        changeYear: true
                    });
                }
            </script>
            <div>
                <fieldset style="padding: 10px">
                    <legend>Tyre Issue</legend>
                    <div id="Div2" runat="server" align="center">
                        Vehicle Number<span style="color: Red">*</span>

                        <ajaxToolkit:ComboBox AutoCompleteMode="Append" ID="ddlInventoryTyreIssueVehicles" runat="server" OnSelectedIndexChanged="ddlInventoryTyreIssueVehicles_SelectedIndexChanged"
                                              AutoPostBack="True" DropDownStyle="DropDownList">
                        </ajaxToolkit:ComboBox>
                    </div>
                    <br/>
                    <div id="Div5" runat="server">
                        Pending for Issue
                    </div>
                    <br/>
                    <div id="Div6" runat="server" align="center">
                        <asp:GridView ID="grvTyrePendingForIssue" runat="server" CssClass="gridviewStyle"
                                      EmptyDataText="Details are not available" CellPadding="3" AutoGenerateColumns="False"
                                      AllowPaging="True" DataKeyNames="FleetInventoryReqID" PageSize="5" OnPageIndexChanging="grvTyrePendingForIssue_PageIndexChanging"
                                      OnRowCommand="grvTyrePendingForIssue_RowCommand" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px">
                            <Columns>
                                <asp:BoundField HeaderText="Vehicle Number" DataField="VehicleNum"/>
                                <asp:BoundField HeaderText="Request Date" DataField="RequestedDate" DataFormatString="{0:dd/MM/yyyy}"/>
                                <asp:BoundField HeaderText="Request By" DataField="RequestedBy"/>
                                <asp:BoundField HeaderText="No. of Tyres" DataField="RequestedQty"/>
                                <asp:TemplateField ControlStyle-Width="50px" HeaderStyle-Width="60px">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnViewDetails" runat="server" Text="Issue" ToolTip="Click here to Issue the details"
                                                        OnClick="BtnViewDetails_Click" RowIndex="<%# Container.DisplayIndex %>" CommandName="Show"
                                                        CommandArgument='<%#DataBinder.Eval(Container.DataItem, "FleetInventoryReqID") %>'/>
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
                    </div>
                    <asp:Button ID="btnShowPopup" runat="server" Style="display: none"/>
                    <ajaxToolkit:ModalPopupExtender ID="gv_ModalPopupExtenderTyreIssue" BehaviorID="mdlPopup"
                                                    runat="server" TargetControlID="btnShowPopup" PopupControlID="pnlPopup" BackgroundCssClass="modalBackground"/>
                    <asp:Panel ID="pnlPopup" runat="server" CssClass="modalPanel" Style="padding: 5px;margin-top: 125px;position:fixed">
                        <asp:UpdatePanel ID="updPnlReqDetail" runat="server">
                            <ContentTemplate>
            
                                    <legend style="margin-top: 1px;color:brown">Tyre Issue Detail</legend>
                                    <table class="style1" align="center" style="border:1px solid brown">
                                        <tr>
                                            <td class="rowseparator"></td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                Vehicle Number
                                            </td>
                                            <td class="columnseparator"></td>
                                            <td align="left">
                                                <asp:TextBox ID="txtTyreIssVehicleNumber" runat="server" ReadOnly="True" MaxLength="20"></asp:TextBox>
                                            </td>
                                            <td class="columnseparator"></td>
                                            <td align="left">
                                                District
                                            </td>
                                            <td class="columnseparator"></td>
                                            <td align="left">
                                                <asp:TextBox ID="txtTyreIssDistrict" runat="server" ReadOnly="True" MaxLength="20"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="rowseparator"></td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                Total Tyre Cost
                                            </td>
                                            <td class="columnseparator"></td>
                                            <td align="left">
                                                <asp:TextBox ID="txtTyreCost" runat="server" MaxLength="6" Height="20px"></asp:TextBox>
                                            </td>
                                            <td class="columnseparator"></td>
                                            <td align="left">
                                                DC Number
                                            </td>
                                            <td class="columnseparator"></td>
                                            <td align="left">
                                                <asp:TextBox ID="txtDcNumberPopup" runat="server" MaxLength="5"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="rowseparator"></td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                DC Date
                                            </td>
                                            <td class="columnseparator"></td>
                                            <td align="left">
                                                <asp:TextBox ID="txtDcDate" runat="server" MaxLength="20" onkeypress="return false"
                                                             oncut="return false;" onpaste="return false;" oncopy="return false;">
                                                </asp:TextBox>
                                            </td>
                                            <td class="columnseparator"></td>
                                            <td align="left">
                                                Courier Name
                                            </td>
                                            <td class="columnseparator"></td>
                                            <td align="left">
                                                <asp:TextBox ID="txtCourierName" runat="server" MaxLength="15"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="rowseparator"></td>
                                        </tr>
                                        <tr>
                                            <td colspan="7">
                                                <asp:GridView ID="grvTyreIssueDetailsPopup" runat="server" AutoGenerateColumns="False"  Width="100%"
                                                              OnRowDataBound="grvTyreIssueDetailsPopup_RowDataBound" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3">
                                                    <Columns>

                                                        <asp:BoundField DataField="TyrePosition" HeaderText="Current Tyre Position"/>
                                                        <asp:BoundField DataField="TyreNumber" HeaderText="Current Tyre Number"/>
                                                        <asp:BoundField DataField="Make" HeaderText="Make"/>
                                                        <asp:BoundField DataField="Model" HeaderText="Model"/>
                                                        <asp:BoundField DataField="TotalKmTravelled" HeaderText="Current Total Travel ODO"/>
                                                        <asp:TemplateField HeaderText="Tyre Number">
                                                            <ItemTemplate>
                                                                <asp:DropDownList ID="ddlTyreNumber" runat="server">
                                                                </asp:DropDownList>
                                                            </ItemTemplate>
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
                                                <div id="Div7" align="center" style="background-color: white; width: 95%;">
                                                    Remarks
                                                    <asp:TextBox ID="txtRemarks" runat="server" onkeypress="return OnlyAlphaNumeric(event)"
                                                                 MaxLength="20" TextMode="MultiLine" onKeyUp="CheckLength(this,50)" onChange="CheckLength(this,50)">
                                                    </asp:TextBox>

                                                    <asp:Button ID="btnOk" runat="server" CssClass="form-submit-button" Text="Issue" Width="50px" OnClick="btnOk_Click"/>

                                                    <asp:Button runat="server" Text="Cancel" CssClass="form-reset-button" Width="50px" OnClick="btnNo_Click"/>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:Panel>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>