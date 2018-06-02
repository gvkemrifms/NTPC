<%@ Page Language="C#" MasterPageFile="~/temp.master" AutoEventWireup="true" CodeFile="KmplMaster.aspx.cs" Inherits="KmplMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <table align="center" width="100%">
                <tr>
                    <td>
                        <fieldset style="padding: 10px">
                        <legend style="color: brown" align="center">KMPL</legend>
                        <table align="center">
                            <tr>
                                <td>
                                    Vehicle Number
                                </td>
                                <td class="columnseparator">
                                </td>
                                <td>
                                    <cc1:ComboBox AutoCompleteMode="Append" name="vehicleNumber" ID="ddlVehNumber" runat="server" AutoPostBack="true"
                                                  onselectedindexchanged="ddlVehNumber_SelectedIndexChanged" DropDownStyle="DropDownList">
                                    </cc1:ComboBox>

                                </td>
                            </tr>

                            <tr>
                                <td class="rowseparator">
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    KMPL
                                </td>
                                <td class="columnseparator">
                                </td>
                                <td>
                                    <asp:TextBox ID="txtKMPL" runat="server"/>
                                </td>
                            </tr>
                        </table>
                        <div align="center">
                            <div style="float: left; width: 300px;">
                            </div>
                            <div>
                                <asp:Button runat="server" ID="btnUpdate" Text="Update" CssClass="form-submit-button" style="margin: 20px"
                                            OnClientClick="if (!validationFuelEntry()) return false;" onclick="btnUpdate_Click"/>
                            </div>
                        </div>
                        <div align="center">
                            <div align="center">
                                <asp:GridView ID="gvVehKmplDetails" runat="server" EmptyDataText="No records found" PageSize="20"
                                              AllowSorting="True" BorderWidth="1px" BorderColor="#CCCCCC" AutoGenerateColumns="False" CssClass="gridviewStyle"
                                              CellPadding="3" Width="630px" AllowPaging="True"
                                              EnableSortingAndPagingCallbacks="True"
                                              onrowcommand="gvVehKmplDetails_RowCommand"
                                              onpageindexchanging="gvVehKmplDetails_PageIndexChanging" BackColor="White" BorderStyle="None">
                                    <RowStyle CssClass="rowStyleGrid" ForeColor="#000066"/>
                                    <Columns>
                                        <asp:TemplateField HeaderText="VehicleNumber">
                                            <ItemTemplate>
                                                <asp:Label ID="lblVehNumber" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "VehicleNumber") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="KMPL">
                                            <ItemTemplate>
                                                <asp:Label ID="lblKMPL" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "KMPL") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Edit">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkEdit" runat="server" CommandName="MainEdit" CommandArgument=" <%# Container.DataItemIndex %>"
                                                                Text="Edit">
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
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
                        </div>
                    </td>
                </tr>
            </table>

            <script type="text/javascript" language="javascript">
                function validationFuelEntry() {
                    var districts = document.getElementById("<%= ddlVehNumber.ClientID %>").control._textBoxControl
                        .value;
                    switch (districts) {
                    case '--Select--':
                        return alert("Please Select the VehicleNumber");

                    }
                    switch (document.getElementById("<%= txtKMPL.ClientID %>").value) {
                    case '':
                        document.getElementById("<%= txtKMPL.ClientID %>").focus();
                        return alert("KMPL Should not be Blank");
                    }
                    return true;
                }
            </script>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>