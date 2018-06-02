<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/temp.master" CodeFile="VehiclemaintenanceNonoffroad.aspx.cs" Inherits="VehiclemaintenanceNonoffroad" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .WrapStyle TD { word-break: break-all; }
    </style>
    <script language="javascript" type="text/javascript">

        function Validation() {
            var vehiclenoddl = document.getElementById('<%= ddlVehicles.ClientID %>');
            if (vehiclenoddl && vehiclenoddl.selectedIndex === 0) {
                alert("Please select Vehicle number");
                vehiclenoddl.focus();
                return false;
            }
            var maintenanceType = document.getElementById('<%= ddlMaintenanceType.ClientID %>');
            if (maintenanceType && maintenanceType.selectedIndex === 0) {
                alert("Please select Maintenance Type");
                maintenanceType.focus();
                return false;
            }
            var vendorName = document.getElementById('<%= ddlVendorName.ClientID %>');
            if (vendorName && vendorName.selectedIndex === 0) {
                alert("Please select Vandor Name");
                vendorName.focus();
                return false;
            }
            var txtmaintenanceDate = $('#<%= txtMaintenanceDate.ClientID %>').val();
            if (txtmaintenanceDate === "") {
                return alert('Maintenance Date is Mandatory');
            }
            var txtbillDate = $('#<%= txtBillDate.ClientID %>').val();
            if (txtbillDate === "") {
                return alert('Bill Date is Mandatory');
            }
            var txtbillNumber = $('#<%= txtBillNo.ClientID %>').val();
            if (txtbillNumber === "") {
                return alert('Bill Number is Mandatory');
            }
            var txtbillAmount = $('#<%= txtBillAmount.ClientID %>').val();
            if (txtbillAmount === "") {
                return alert('Bill Amount is Mandatory');
            }
            var txtquantity = $('#<%= txtQuant.ClientID %>').val();
            if (txtquantity === "") {
                return alert('Item Quantity is mandatory');
            }     
            return true;
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <script type="text/javascript">
                function pageLoad() {
                    $('#<%= ddlVehicles.ClientID %>').select2({
                        disable_search_threshold: 5,
                        search_contains: true,
                        minimumResultsForSearch: 20,
                        placeholder: "Select an option"
                    });
                    $('#<%= ddlMaintenanceType.ClientID %>').select2({
                        disable_search_threshold: 5,
                        search_contains: true,
                        minimumResultsForSearch: 20,
                        placeholder: "Select an option"
                    });
                    $('#<%= ddlVendorName.ClientID %>').select2({
                        disable_search_threshold: 5,
                        search_contains: true,
                        minimumResultsForSearch: 20,
                        placeholder: "Select an option"
                    });
                }
            </script>

            <fieldset style="padding: 10px">
                <legend align="center" style="color: brown">Vehicle Non OffRoad</legend>
                <table align="center">
                    <tr>
                        <td >
                            Vehicle Number<span style="color: Red">*</span>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlVehicles" runat="server" AutoPostBack="true" Width="150px" OnSelectedIndexChanged="ddlVehicles_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>

                    </tr>
                    <tr>
                        <td>
                            District
                        </td>
                        <td >
                            <asp:TextBox runat="server" ID="txtDistrict" Width="150px" Enabled="False"/>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Location
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtLocation" Enabled="False"/>
                        </td>
                    </tr>


                </table>
                <asp:Panel runat="server" style="margin-top: 50px" HorizontalAlign="Center">
                    <fieldset style="padding: 0px 0px 0px 0px">
                        <legend align="center" style="color: brown">Maintenance Details </legend>

                        <table align="center">
                            <tr>
                                <td>
                                    Maintenance Type <span style="color: Red">*</span>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlMaintenanceType" runat="server" Width="180px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>

                                <td>
                                   Maintenance Date<span style="color: Red">*</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtMaintenanceDate" runat="server"
                                                 onkeypress="return false">
                                    </asp:TextBox>
                                    <cc1:CalendarExtender runat="server" Format="MM/dd/yyyy"
                                                          PopupButtonID="imgBtnQuotationDate" TargetControlID="txtMaintenanceDate">
                                    </cc1:CalendarExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Vendor Name <span style="color: Red">*</span>
                                </td>

                                <td>
                                    <asp:DropDownList ID="ddlVendorName" runat="server" Width="180px"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Bill Number<span style="color: Red">*</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtBillNo" runat="server" MaxLength="10"
                                                 onkeypress="return numeric_only(event)">
                                    </asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                  Bill Date <span style="color: Red">*</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtBillDate" runat="server"
                                                 onkeypress="return false">
                                    </asp:TextBox>
                                    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Format="MM/dd/yyyy"
                                                          PopupButtonID="imgBtnQuotationDate" TargetControlID="txtBillDate">
                                    </cc1:CalendarExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                   Part Code
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPartCode" runat="server" MaxLength="10"
                                                 onkeypress="return numeric_only(event);">
                                    </asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label Text="Item Description" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtItemDesc" runat="server"
                                                 onkeypress="return alpha_only_withspace(event)">
                                    </asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                   Item Quantity<span style="color: Red">*</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtQuant" runat="server" MaxLength="5"
                                                 onkeypress="return numeric_only(event)">
                                    </asp:TextBox>
                                </td>
                            </tr>

                            <tr>
                                <td>
                                   Bill Amount<span style="color: Red">*</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtBillAmount" runat="server" MaxLength="12" onkeypress="return numericOnly(this);"></asp:TextBox>
                                </td>

                            </tr>
                            <tr>
                                <td>
                                    <asp:Button runat="server" CssClass="form-submit-button" Text="Save" Width="52px"
                                                OnClick="btnSave_Click" OnClientClick="if(!Validation()) return false;"/>
                                </td>
                                <td>
                                    <asp:Button runat="server" CssClass="form-reset-button" Text="Reset"
                                                OnClick="btnSPReset_Click"/>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <br/>
                </asp:Panel>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>