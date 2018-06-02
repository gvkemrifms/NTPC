<%@ Page Language="C#" MasterPageFile="~/temp.master" AutoEventWireup="true" CodeFile="RefurbishmentAlert.aspx.cs" Inherits="RefurbishmentAlert" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <table>
                <tr>
                    <td class="rowseparator">
                    </td>
                </tr>
                <tr>
                    <td>
                        <fieldset style="padding: 10px">
                            <legend align="center" style="color: brown">Refurbishment Alert </legend>
                            <table>
                                <tr>
                                    <td class="rowseparator">
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:GridView ID="grdRefAlert" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                      ForeColor="#333333" GridLines="Both" Width="622px" AllowPaging="True" EmptyDataText="No Records Found"
                                                      CssClass="gridviewStyle" CellSpacing="2" OnPageIndexChanging="grdRefAlert_PageIndexChanging">
                                            <RowStyle CssClass="rowStyleGrid"/>
                                            <Columns>
                                                <asp:TemplateField HeaderText="Vehicle Number" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <%#Eval("VehicleNumber") %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Latest Odometer" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <%#Eval("Latest_odo") %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Last Refurbishment Odo" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <%#Eval("LastRefurbishmentOdo") %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Last Refurbishment Date" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <%#Eval("LastRefurbishmentDate") %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Service Alert" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <%#Eval("refalert") %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <FooterStyle CssClass="footerStylegrid"/>
                                            <PagerStyle CssClass="pagerStylegrid"/>
                                            <SelectedRowStyle CssClass="selectedRowStyle"/>
                                            <HeaderStyle CssClass="headerStyle"/>
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="rowseparator">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" valign="middle">
                                        <asp:Button CssClass="form-submit-button" runat="server" Text="Send Mail" OnClick="btnSendMail_Click"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="rowseparator">
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>