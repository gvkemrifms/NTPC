<%@ Page Language="C#" MasterPageFile="~/temp.master" AutoEventWireup="true" CodeFile="GetWheelAlignmentAlert.aspx.cs" Inherits="GetWheelAlignmentAlert" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>
        <tr>
            <td class="rowseparator">
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <fieldset style="padding: 10px">
                            <legend>
                                <asp:Label ID="lblheader" runat="server"></asp:Label>
                            </legend>
                            <table>
                                <tr>
                                    <td class="rowseparator">
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:GridView ID="grdWheelAlignment" runat="server" AutoGenerateColumns="False" CellPadding="3" Width="622px" AllowPaging="True" EmptyDataText="No Records Found"
                                                      CssClass="gridviewStyle" OnPageIndexChanging="grdWheelAlignment_PageIndexChanging" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px">
                                            <RowStyle CssClass="rowStyleGrid" ForeColor="#000066"/>
                                            <Columns>
                                                <asp:TemplateField HeaderText="Distict">
                                                    <ItemTemplate>
                                                        <%#Eval("DistrictName") %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Vehicle Number">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnk_VehicleNumeber" runat="server" Text='<%#Eval("VehicleNumber") %>'
                                                                        OnCommand="lnk_VehicleNumeber_Click" CommandArgument='<%#Eval("vehicleID") %> '>
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
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
                                <tr>
                                    <td align="center" valign="middle">
                                        <asp:Button runat="server" Text="Send Mail" OnClick="btnSendMail_Click" CssClass="form-submit-button"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="rowseparator">
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
</asp:Content>