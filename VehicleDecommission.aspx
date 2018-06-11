<%@ Page Language="C#" MasterPageFile="~/temp.master" AutoEventWireup="true" CodeFile="VehicleDecommission.aspx.cs" Inherits="VehicleDecommission" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        function pageLoad() {
            $('#<%= txtDecommDate.ClientID %>').datepicker({
                dateFormat: 'mm/dd/yy',
                changeMonth: true,
                changeYear: true
            });
        }

        function validation() {
            var district = document.getElementById('<%= ddlDistrict.ClientID %>');
            var vehicleId = document.getElementById('<%= ddlVehicleNumber.ClientID %>');
            var txtVehicleId = document.getElementById('<%= txtVehicleNumber.ClientID %>');
            var decommReason = document.getElementById('<%= txtDecommReason.ClientID %>');
            var decommDate = document.getElementById('<%= txtDecommDate.ClientID %>');
            var decommRemark = document.getElementById('<%= txtDecommRemark.ClientID %>');
            if (district && district.selectedIndex === 0) {
                alert("Please Select State");
                district.focus();
                return false;
            }
            if (vehicleId) {
                var inputs = vehicleId.getElementsByTagName('input');
                var i;
                for (i = 0; i < inputs.length; i++) {
                    switch (inputs[i].type) {
                    case 'text':
                        if (inputs[i].value !== "" && inputs[i].value != null && inputs[i].value === "--Select--") {
                            alert('Select the Vehicle');
                            return false;
                        }
                        break;
                    }
                }
            }
            if (txtVehicleId)
                if (!RequiredValidation(txtVehicleId, "Vehicle Number cannot be blank"))
                    return false;
            if (!RequiredValidation(decommReason, "Decommission Reason cannot be blank"))
                return false;
            if (!RequiredValidation(decommDate, "Decommission Date cannot be blank"))
                return false;
            var now = new Date();
            if (Date.parse(decommDate.value) > Date.parse(now)) {
                alert("Decommission Date should not be greater than Current Date");
                decommDate.focus();
                return false;
            }
            if (!RequiredValidation(decommRemark, "Decommission Remark cannot be blank"))
                return false;
            return true;
        }
    </script>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>

            <fieldset style="padding: 10px">
                <legend align="center" style="color: brown">Vehicle Decommission</legend>
                <br/>
                <table align="center">
                    <tr>
                        <td>
                            State<span style="color: Red">*</span>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlDistrict" CssClass="search_3" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlDistrict_SelectedIndexChanged">
                                <asp:ListItem Value="-1">--Select--</asp:ListItem>
                            </asp:DropDownList>
                            <asp:TextBox ID="txtDistrict" runat="server" Visible="false" onkeypress="return false;"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Vehicle Number<span style="color: Red">*</span>
                        </td>
                        <td>
                            <cc1:ComboBox AutoCompleteMode="Append" ID="ddlVehicleNumber" runat="server" AutoPostBack="true"
                                          DropDownStyle="DropDownList">
                                <asp:ListItem Value="-1">--Select--</asp:ListItem>
                            </cc1:ComboBox>
                            <asp:TextBox ID="txtVehicleNumber" runat="server" Visible="false" onkeypress="return false;"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Decommission Reason<span style="color: Red">*</span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDecommReason" CssClass="search_3" runat="server" Width="150px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Decommission Date<span style="color: Red">*</span>
                        </td>
                        <td >
                            <asp:TextBox ID="txtDecommDate" CssClass="search_3" runat="server" Width="150px" onkeypress="return false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Decommission Remark<span style="color: Red">*</span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDecommRemark" CssClass="search_3" runat="server" TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <table align="center">
                                <tr>
                                    <td >
                                        <asp:Button ID="btnSubmit" CssClass="form-submit-button" runat="server" Text="Submit" OnClick="btnSubmit_Click"/>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnReset" runat="server" CssClass="form-reset-button" Text="Reset" OnClick="btnReset_Click"/>
                                    </td>

                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
            </tr>
            </table>
            <br/>
            <br/>
            <table align="center">
            <tr align="center">
            <td>
            <asp:GridView ID="grdvwDecommVehicles" runat="server" AutoGenerateColumns="False"
                          CellPadding="3" BorderWidth="1px" BorderColor="#CCCCCC" Width="630px" AllowPaging="True"
                          EmptyDataText="No Records Found" CssClass="gridviewStyle" OnPageIndexChanging="grdvwDecommVehicles_PageIndexChanging"
                          OnRowCommand="grdvwDecommVehicles_RowCommand" BackColor="White" BorderStyle="None">
                <RowStyle CssClass="rowStyleGrid" ForeColor="#000066"/>
                <Columns>
                    <asp:TemplateField HeaderText="District">
                        <ItemTemplate>
                            <asp:Label ID="lblDistrict" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"District") %>'>
                            </asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Vehicle Number">
                        <ItemTemplate>
                            <asp:Label ID="lblVehicleNumber" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"VehicleNumber") %>'>
                            </asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Decommission Reason">
                        <ItemTemplate>
                            <asp:Label ID="lblDecommReason" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"DecommReason") %>'>
                            </asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Decommission Date">
                        <ItemTemplate>
                            <asp:Label ID="lblDecommDate" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"DecommDate") %>'>
                            </asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Edit">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkEdit" CssClass="form-submit-button" runat="server" CommandName="DecommVehEdit" CommandArgument='<%#DataBinder.Eval(Container.DataItem,"VehicleDecommId") %>'
                                            Text="Edit">
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Revert">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkRevert" CssClass="form-submit-button" runat="server" CommandName="DecommVehRevert" CommandArgument='<%#DataBinder.Eval(Container.DataItem,"VehicleDecommId") %>'
                                            Text="Revert">
                            </asp:LinkButton>
                            <cc1:ConfirmButtonExtender ID="confrmbtnextndrRevert" runat="server" TargetControlID="lnkRevert"
                                                       ConfirmText="Are you sure? Want to Revert?">
                            </cc1:ConfirmButtonExtender>
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
            </fieldset>
        </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>