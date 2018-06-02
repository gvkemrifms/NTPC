<%@ Page Language="C#" MasterPageFile="~/temp.master" AutoEventWireup="true" CodeFile="FuelDetailsVerification.aspx.cs" Inherits="FuelDetailsVerification" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table align="center">
        <tr>
            <td class="rowseparator">
            </td>
        </tr>
        <tr>
            <td>
                <fieldset style="padding: 10px">
                    <legend align="center" style="color: brown">Fuel Detail Verification</legend>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">

                        <ContentTemplate>
                            <script type="text/javascript">
                                function pageLoad() {
                                    $('#<%= ddlVehicleNumber.ClientID %>').select2({
                                        disable_search_threshold: 5,
                                        search_contains: true,
                                        minimumResultsForSearch: 20,
                                        placeholder: "Select an option"
                                    });
                                }
                            </script>
                            <table style="width: 100%">
                                <tr>
                                    <td align="right">

                                        <asp:Label runat="server" Text="Select Vehicle"></asp:Label>
                                        <span style="color: Red">*</span>

                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="ddlVehicleNumber" runat="server" Width="170px" AutoPostBack="True"
                                                          onselectedindexchanged="ddlVehicleNumber_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr class="rowseparator">
                                    <td align="right">
                                        &nbsp;
                                    </td>
                                    <td align="left">
                                        &nbsp;
                                    </td>
                                </tr>
                                <caption>
                                    <br/>
                                    <tr>
                                        <td align="center" colspan="2">
                                            <asp:GridView ID="gvVerification" runat="server" AllowPaging="True" AutoGenerateColumns="False" BorderColor="#CCCCCC" BorderWidth="1px" Caption="Fuel Entry Details" CaptionAlign="Top" CellPadding="3" CssClass="gridviewStyle" EmptyDataText="No Records to Approve/Reject" onpageindexchanging="gvVerification_PageIndexChanging1" OnRowEditing="gvVerification_RowEditing" Width="434px" BackColor="White" BorderStyle="None">
                                                <RowStyle CssClass="rowStyleGrid" ForeColor="#000066" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Check">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="checkSelect" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="vehno" HeaderText="Vehicle" />
                                                    <asp:BoundField DataField="EntryDate" HeaderText="Date" />
                                                    <asp:BoundField DataField="Qty" HeaderText="Quaantity" />
                                                    <asp:BoundField DataField="Price" HeaderText="UnitPrice" />
                                                    <asp:BoundField DataField="Amount" HeaderText="Amount" />
                                                    <asp:BoundField DataField="KMPL" HeaderText="KMPL" />
                                                    <asp:TemplateField HeaderText="Remarks">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtRemarks" runat="server" MaxLength="30" onkeypress="return remark(event);"></asp:TextBox>
                                                            <asp:Label ID="lblId" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "FuelEntryID") %>' Visible="false"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <FooterStyle CssClass="footerStylegrid" BackColor="White" ForeColor="#000066" />
                                                <PagerStyle CssClass="pagerStylegrid" BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                                <SelectedRowStyle CssClass="selectedRowStyle" BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                                <HeaderStyle CssClass="headerStyle" BackColor="#006699" Font-Bold="True" ForeColor="White" />
                                                <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                                <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                                <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                                <SortedDescendingHeaderStyle BackColor="#00547E" />
                                            </asp:GridView>
                                            <br/>
                                            <br/>
                                            <br/>
                                            <br/>
                                            <asp:GridView ID="gvReconcilation" runat="server" AllowPaging="True" AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" Caption="Reconciliation Details" CaptionAlign="Top" CellPadding="3" CssClass="gridviewStyle" EmptyDataText="No Records to Approve/Reject" onpageindexchanging="gvReconcilation_PageIndexChanging1" OnRowEditing="gvReconcilation_RowEditing" Width="434px">
                                                <RowStyle CssClass="rowStyleGrid" ForeColor="#000066" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Check">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="checkSelect" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="VehicleNumber" HeaderText="Vehicle" />
                                                    <asp:BoundField DataField="AccountID" HeaderText="Account" />
                                                    <asp:BoundField DataField="Dealer" HeaderText="Dealer" />
                                                    <asp:BoundField DataField="Location" HeaderText="Location" />
                                                    <asp:BoundField DataField="TransactionDate" HeaderText="Date" />
                                                    <asp:BoundField DataField="Amount" HeaderText="Amount" />
                                                    <asp:BoundField DataField="Quantity" HeaderText="Quantity" />
                                                    <asp:TemplateField Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="Rblid" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "TransactionID") %>' Visible="false"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <FooterStyle BackColor="White" CssClass="footerStylegrid" ForeColor="#000066" />
                                                <PagerStyle BackColor="White" CssClass="pagerStylegrid" ForeColor="#000066" HorizontalAlign="Left" />
                                                <SelectedRowStyle BackColor="#669999" CssClass="selectedRowStyle" Font-Bold="True" ForeColor="White" />
                                                <HeaderStyle BackColor="#006699" CssClass="headerStyle" Font-Bold="True" ForeColor="White" />
                                                <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                                <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                                <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                                <SortedDescendingHeaderStyle BackColor="#00547E" />
                                            </asp:GridView>
                                            <br/>
                                            <br/>
                                            <table align="left" style="width: 66%">
                                                <tr>
                                                    <td align="center">
                                                        <asp:Button ID="Approve" runat="server" CssClass="form-submit-button" OnClick="Approve_Click" Text="Approve" />
                                                        <asp:Button ID="btnHdnApprove" runat="server" CssClass="form-submit-button" onclick="btnHdnApprove_Click" style="display: none;" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:Button ID="Reject" runat="server" CssClass="form-reset-button" OnClick="Reject_Click" Text="Reject" />
                                                        <asp:Button ID="btnHdnReject" runat="server" CssClass="form-reset-button" onclick="btnHdnReject_Click" style="display: none;" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">&nbsp; </td>
                                    </tr>
                                </caption>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </fieldset>
            </td>
        </tr>
        <tr>
            <td class="rowseparator">
            </td>
        </tr>
    </table>
</asp:Content>