<%@ Page Language="C#" MasterPageFile="~/temp.master" AutoEventWireup="true" CodeFile="VehicleDetails.aspx.cs" Inherits="VehicleDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        function validation() {
            var engineNo = document.getElementById('<%= txtEngineNumber.ClientID %>');
            var chassisNo = document.getElementById('<%= txtChassisNumber.ClientID %>');
            var vehicleNo = document.getElementById('<%= txtVehicleNumber.ClientID %>');

            if (!RequiredValidation(engineNo, "Engine Number Cannot be Blank"))
                return false;

            if (!RequiredValidation(chassisNo, "Chassis Number Cannot be Blank"))
                return false;

            if (!RequiredValidation(vehicleNo, "Vehicle Number Cannot be Blank"))
                return false;

            if (vehicleNo.value !== "" && !isValidVehicleNumber(vehicleNo.value)) {
                vehicleNo.value = "";
                vehicleNo.focus();
                return false;
            }
            return true;

        }
    </script>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlNewVehicleDetails" runat="server">
                <legend align="center" style="color: brown">Vehicle Details</legend>
                <br/>
                <table align="center">
                    <tr>
                        <td >
                            Engine Number<span style="color: Red">*</span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtEngineNumber" CssClass="search_3" runat="server" MaxLength="18" onkeypress="return alphanumeric_withspace_only(event);"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td >
                            Chassis Number<span style="color: Red">*</span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtChassisNumber" CssClass="search_3" runat="server" MaxLength="18" onkeypress="return alphanumeric_only(event);"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td >
                            Vehicle T/R Number<span style="color: Red">*</span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtVehicleNumber" CssClass="search_3" runat="server" MaxLength="10" onchange="return isValidVehicleNumber(this.value)"></asp:TextBox>
                        </td>

                    </tr>

                    <tr>

                        <td >
                            <asp:Button ID="btnSave" CssClass="form-submit-button" runat="server" Text="Save" OnClick="btnSave_Click" Width="60px"/>
                        </td>
                        <td >
                            <asp:Button CssClass="form-reset-button" runat="server" Text="Cancel" OnClick="btnCancel_Click"
                                        Height="26px" Width="67px"/>
                        </td>
                    </tr>
                </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>