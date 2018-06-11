<%@ Page Title="" Language="C#" MasterPageFile="~/temp.master" AutoEventWireup="true" CodeFile="ServiceStation.aspx.cs" Inherits="ServiceStation" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <legend align="center" style="color: brown">Service Station</legend>
            <table align="center">
                <tr>
                    <td>
                        Vehicle Number<span style="color: Red">*</span>
                    </td>
                    <td>
                        <cc1:ComboBox AutoCompleteMode="Append" ID="ddlVehicleNumber" runat="server" AutoPostBack="True"
                                      DropDownStyle="DropDownList"
                                      OnSelectedIndexChanged="ddlVehicleNumber_SelectedIndexChanged">
                        </cc1:ComboBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Service Station Name<span style="color: Red">*</span>
                    </td>
                    <td>
                        <asp:TextBox ID="txtServiceSrationName" CssClass="search_3" runat="server"/>
                    </td>
                </tr>
                <tr>
                    <td>
                        State<span style="color: Red">*</span>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlDistricts" CssClass="search_3" runat="server" Enabled="false"/>
                    </td>
                </tr>
            </table>
            <div align="center">
                <asp:Button runat="server" ID="btnSave" Text="Save" CssClass="form-submit-button" OnClick="btnSave_Click" OnClientClick="return validationFuelEntry();"/>
                <asp:Button runat="server" ID="btnUpdate" Text="Update" CssClass="form-reset-button" OnClientClick="return validationFuelEntry();"
                            OnClick="btnUpdate_Click"/>
            </div>
            <br/>
            <div align="center">
                <asp:GridView ID="gvServiceStationDetails" Visible="False" runat="server" EmptyDataText="No records found"
                              AllowSorting="True" AutoGenerateColumns="False" CssClass="gridviewStyle"
                              CellPadding="3" Width="630px" AllowPaging="True"
                              EnableSortingAndPagingCallbacks="True"
                              OnRowCommand="gvServiceStationDetails_RowCommand"
                              OnPageIndexChanging="gvServiceStationDetails_PageIndexChanging" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px">
                    <RowStyle CssClass="rowStyleGrid" ForeColor="#000066"/>
                    <Columns>
                        <asp:TemplateField HeaderText="Station">
                            <ItemTemplate>
                                <asp:Label ID="lblServiceStation" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ServiceStation_Name") %>'>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="District">
                            <ItemTemplate>
                                <asp:Label ID="lblDistricts" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ds_lname") %>'>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="VehicleNumber">
                            <ItemTemplate>
                                <asp:Label ID="lblVehNum" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"VehicleNumber") %>'>
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
                        <asp:TemplateField HeaderText="Delete">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkDelete" runat="server" CommandName="MainDelete" CommandArgument=" <%# Container.DataItemIndex %>"
                                                Text="Delete">
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
        </td>
        </tr>
        </table>

        <script type="text/javascript" language="javascript">
            function validationFuelEntry() {
                var vehicleNumber = document.getElementById('<%= ddlVehicleNumber.ClientID %>').control
                    ._textBoxControl.value;
                if (vehicleNumber === '--Select--') {
                    alert("Please select the Vehicle");
                    return false;
                }
                if (document.getElementById("<%= txtServiceSrationName.ClientID %>").value === 0) {
                    alert("Service Station Cannot be Blank");
                    document.getElementById("<%= txtServiceSrationName.ClientID %>").focus();
                    return false;
                }
                var districts = document.getElementById('<%= ddlDistricts.ClientID %>');
                if (districts.selectedIndex === 0) {
                    alert("Please select the District");
                    districts.focus();
                    return false;
                }
                return true;
            }

            function OnlyAlphabets(myfield, e, dec) {
                var key;
                if (window.event)
                    key = window.event.keyCode;
                else if (e)
                    key = e.which;
                else
                    return true;
                var keychar = String.fromCharCode(key);
                if ((("!@#$%^&*()_+=-';{}[]|?<>:,/\".1234567890").indexOf(keychar) > -1))
                    return false;
                else
                    return true;
            }
        </script>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>