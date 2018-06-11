<%@ Page Language="C#" MasterPageFile="~/temp.master" AutoEventWireup="true" CodeFile="ScheduleServiceMaster.aspx.cs" Inherits="ScheduleServiceMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <script type="text/javascript">
                function Validations() {
                    var ddlDistrict = $('#<%= ddlManufactName.ClientID %> option:selected').text().toLowerCase();
                    if (ddlDistrict === '--select--') {
                        return alert("Please select Manufacturer Name");
                    }
                    var generalService = $('#<%= txtGeneralService.ClientID %>').val();
                    if (generalService === '') {
                        return alert('Please select general Service');
                    }
                    return true;
                }
            </script>
            <legend align="center" style="color: brown">Schedule Service Master</legend>
            <br/>
            <table align="center">
                <tr>
                    <td >
                        Manufacturer Name<span style="color: red">*</span>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlManufactName" runat="server" CssClass="search_3"
                                          OnSelectedIndexChanged="ddlManufactName_SelectedIndexChanged"
                                          AutoPostBack="True"/>
                    </td>
                </tr>

                <tr>
                    <td>
                        General Service <span style="color: red">*</span>
                    </td>

                    <td>
                        <asp:TextBox ID="txtGeneralService" CssClass="search_3" onkeypress="return numeric_only(event)" runat="server"/>
                    </td>
                </tr>
                <tr>
                    <td >
                        Schedule Service Alert1 <span style="color: red"></span>
                    </td>
                    <td>
                        <asp:TextBox ID="txtSSAlert1" CssClass="search_3" onkeypress="return numeric_only(event)" runat="server"/>
                    </td>
                </tr>
                <tr>
                    <td >
                        Schedule Service Alert2 <span style="color: red"></span>
                    </td>
                    <td>
                        <asp:TextBox ID="txtSSAlert2" CssClass="search_3" onkeypress="return numeric_only(event)" runat="server"/>
                    </td>
                </tr>
                <tr>
                    <td>
                        Schedule Service Alert3 <span style="color: red"></span>
                    </td>
                    <td>
                        <asp:TextBox ID="txtSSAlert3" CssClass="search_3" onkeypress="return numeric_only(event)" runat="server"/>
                    </td>
                </tr>
                <tr>
                    <td >
                        Schedule Service Alert4 <span style="color: red"></span>
                    </td>
                    <td>
                        <asp:TextBox ID="txtSSAlert4" CssClass="search_3" onkeypress="return numeric_only(event)" runat="server"/>
                    </td>
                </tr>
                <tr>
                    <td >
                        Schedule Service Alert5 <span style="color: red"></span>
                    </td>
                    <td>
                        <asp:TextBox ID="txtSSAlert5" CssClass="search_3" onkeypress="return numeric_only(event)" runat="server"/>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnSave" CssClass="form-reset-button" runat="server" Text="Save" OnClick="btnSave_Click" OnClientClick="if (!Validations()) return false;"/>
                    </td>

                    <td>
                        <asp:Button ID="btnUpdate" CssClass="form-reset-button" runat="server" Text="Update" OnClick="btnUpdate_Click"/>
                    </td>
                </tr>
            </table>
            <br/>
            <div align="center">
                <asp:GridView ID="gvScheduleServiceMaster" runat="server" EmptyDataText="No Records Found"
                              AllowSorting="True" AutoGenerateColumns="False" CssClass="gridview"
                              CellPadding="3" Width="650px" AllowPaging="True"
                              EnableSortingAndPagingCallbacks="True" OnRowCommand="gvScheduleServiceMaster_RowCommand" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px">
                    <RowStyle CssClass="rowStyleGrid" ForeColor="#000066"/>
                    <Columns>
                        <asp:TemplateField HeaderText="Manufacturer Name">
                            <ItemTemplate>
                                <asp:Label ID="lblManufacturerName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"FleetManufacturer_Name") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="GSAlert">
                            <ItemTemplate>
                                <asp:Label ID="lblGSAlert" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"GSAlert") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Alert 1">
                            <ItemTemplate>
                                <asp:Label ID="lblAlert1" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Alert_KMS1") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Alert 2">
                            <ItemTemplate>
                                <asp:Label ID="lblAlert2" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Alert_KMS2") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Alert 3">
                            <ItemTemplate>
                                <asp:Label ID="lblAlert3" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Alert_KMS3") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Alert 4">
                            <ItemTemplate>
                                <asp:Label ID="lblAlert4" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Alert_KMS4") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Alert 5">
                            <ItemTemplate>
                                <asp:Label ID="lblAlert5" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Alert_KMS5") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Edit">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkEdit" runat="server" CommandName="EditService" CommandArgument=" <%# Container.DataItemIndex %>"
                                                Text="Edit">
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
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>