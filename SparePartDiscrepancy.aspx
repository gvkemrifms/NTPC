<%@ Page Language="C#" MasterPageFile="~/temp.master" AutoEventWireup="true" CodeFile="SparePartDiscrepancy.aspx.cs" Inherits="SparePartDiscrepancy" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <script type="text/javascript">

                function validation() {
                    var txtReceivedQuantity = document.getElementById(window.Remarks);
                    if (!RequiredValidation(txtReceivedQuantity, "Remarks cannot be blank"))
                        return false;
                    return true;
                }


            </script>

            <div style="background-color: #f7f7f7; border: 1px #E2BBA0 solid; height: 150px; margin: 0 0px 15px 0px; padding: 5px;">
                <img src="images/b1.jpg" alt="banner" width="653" height="150"/>
            </div>
            <fieldset style="padding: 10px;">
                <legend>Spare Part Discrepancy</legend>
                <asp:Panel ID="pnlSparePartDiscrepancy" runat="server">
                    <table style="width: 100%">
                        <tr>
                            <td class="rowseparator"></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Panel runat="server">
                                    <table>
                                        <tr>
                                            <td align="center" colspan="4">
                                                <asp:GridView ID="gvSparePartDiscrepancy" runat="server" AutoGenerateColumns="False"
                                                              GridLines="None" CssClass="gridview" CellPadding="3" CellSpacing="2" EmptyDataText="No Records Found">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chk" runat="server"/>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:BoundField HeaderText="Dist" DataField="District"/>

                                                        <asp:BoundField HeaderText="RecQty" DataField="ReceivedQty"/>
                                                        <asp:BoundField HeaderText="IssQty" DataField="IssuedQty"/>

                                                        <asp:BoundField HeaderText="VehNo" DataField="Vehicle"/>
                                                        <asp:BoundField HeaderText="RecDate" DataField="ReceiptDate"/>

                                                        <asp:TemplateField HeaderText="Remarks">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtRemarks" runat="server" MaxLength="25" TextMode="MultiLine" onkeypress="return remark(event);" onKeyUp="CheckLength(this,50)"
                                                                             onChange="CheckLength(this,50)">
                                                                </asp:TextBox>
                                                                <asp:Label ID="lbdistrict" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "DistrictID") %>'
                                                                           Visible="false">
                                                                </asp:Label>
                                                                <asp:Label ID="lbDetID" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "SparePartReceiptDetID") %>'
                                                                           Visible="false">
                                                                </asp:Label>
                                                                <asp:Label ID="lbVehicle" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "VehicleID") %>'
                                                                           Visible="false">
                                                                </asp:Label>
                                                                <asp:Label ID="lbCreatedBy" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "CreatedBy") %>'
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
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td class="rowseparator"></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Panel ID="pnlButtons" runat="server">
                                    <table>
                                        <tr>
                                            <td class="columnseparator"></td>
                                            <td align="right">
                                                <asp:Button ID="btSave" CssClass="form-submit-button" runat="server" Text="Submit" OnClick="btSave_Click"/>
                                            </td>
                                            <td align="left">
                                                <asp:Button ID="btCancel" CssClass="form-reset-button" runat="server" Text="Cancel" OnClick="btCancel_Click"/>
                                            </td>
                                            <td class="columnseparator"></td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td class="rowseparator"></td>
                        </tr>
                    </table>
                </asp:Panel>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>