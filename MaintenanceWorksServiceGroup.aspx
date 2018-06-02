<%@ Page Language="C#" MasterPageFile="~/temp.master" AutoEventWireup="true" CodeFile="MaintenanceWorksServiceGroup.aspx.cs" Inherits="MaintenanceWorksServiceGroup" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        function validationMaintenanceWorksServiceGroup() {
            switch (document.getElementById("<%= txtServiceGroupName.ClientID %>").value) {
            case "":

                document.getElementById("<%= txtServiceGroupName.ClientID %>").focus();
                return alert("Please Enter Service Group Name");
            }
            switch (document.getElementById("<%= ddlManufacturerName.ClientID %>").selectedIndex) {
            case 0:

                document.getElementById("<%= ddlManufacturerName.ClientID %>").focus();
                return alert("Please Select Manufacturer Name");
            }
            return true;
        }

        function OnlyAlphabets(myfield, e, dec) {
            var keycode;
            if (window.event || event || e) keycode = window.event.keyCode;
            else return true;
            return (keycode >= 65 && keycode <= 90) || (keycode >= 97 && keycode <= 122) || (keycode === 32);
        }

    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table align="center">
                <tr>
                    <td>
                        <fieldset>
                            <legend style="color: brown">Maintenance Works-Service Group</legend>
                            <br/>
                            <asp:Panel ID="pnlmaintenanceworksServiceGrp" runat="server">
                                <table align="center">
                                    <tr>
                                        <td >
                                            Service Group Name <span style="color: Red">*</span> &nbsp;
                                        </td>
                                        <td >
                                            <asp:TextBox ID="txtServiceGroupName" CssClass="search_3" runat="server" Width="150px" MaxLength="15"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="rowseparator"></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Manufacturer Name <span style="color: Red">*</span> &nbsp;
                                        </td>
                                        <td >
                                            <asp:DropDownList ID="ddlManufacturerName" Cssclass="search_3" runat="server" Width="150px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="rowseparator"></td>
                                    </tr>
                                    <br/>
                                    <tr>
                                        <td >
                                            <asp:Button ID="btnSaveMaintenanceWorksServiceGroup" runat="server" Text="Save"
                                                        OnClick="btnSaveMaintenanceWorksServiceGroup_Click" Cssclass="form-submit-button" OnClientClick="if (!validationMaintenanceWorksServiceGroup()) return false;"/>
                                            &nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:Button Cssclass="form-submit-button" runat="server" Text="Reset"
                                                        OnClick="btnResetMaintenanceWorksServiceGroup_Click"/>
                                            &nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:Button ID="btnCancelMaintenanceWorksServiceGroup" Cssclass="form-submit-button" runat="server" Text="Cancel"
                                                        PostBackUrl="~/FleetMaster/MaintenanceWorksMaster.aspx"/>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </fieldset>
                    </td>
                </tr>
                <tr>
                    <td class="rowseparator"></td>
                </tr>
                <caption>
                    <br/>
                    <br/>
                    <tr>
                        <td>
                            <fieldset>
                                <asp:GridView ID="grvMaintenanceWorksServiceGroupDetails" runat="server" AllowPaging="True" AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" CssClass="gridviewStyle" OnPageIndexChanging="grvMaintenanceWorksServiceGroupDetails_PageIndexChanging" OnRowEditing="grvMaintenanceWorksServiceGroupDetails_RowEditing" PageSize="5" style="margin-top: 20px">
                                    <RowStyle CssClass="rowStyleGrid" ForeColor="#000066" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Service Group Id">
                                            <ItemTemplate>
                                                <asp:Label ID="lblServiceId" runat="server" Text='<%#Eval("ServiceGroup_Id") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Service Group Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblServiceGroupName" runat="server" Text='<%#Eval("ServiceGroup_Name") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Manufacturer ID">
                                            <ItemTemplate>
                                                <asp:Label ID="lblServiceName" runat="server" Text='<%#Eval("Manufacturer_Id") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Manufacturer Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblManufacturerName" runat="server" Text='<%#Eval("FleetManufacturer_Name") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Creation Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCreateDate" runat="server" Text='<%#Eval("Created_Date", "{0:d}") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Edit">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkbtnEdit" runat="server" CommandName="Edit" Text="Edit" />
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
                            </fieldset>
                            <asp:HiddenField ID="hidSerGrpId" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="rowseparator"></td>
                    </tr>
                </caption>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>