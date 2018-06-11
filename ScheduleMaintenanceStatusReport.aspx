<%@ Page Title="" Language="C#" MasterPageFile="~/temp.master" AutoEventWireup="true" CodeFile="ScheduleMaintenanceStatusReport.aspx.cs" Inherits="ScheduleMaintenanceStatusReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script>
        function pageLoad() {
            $('#<%= ddldistrict.ClientID %>,#<%= ddlvehicle.ClientID %>').select2({
                disable_search_threshold: 5,
                search_contains: true,
                minimumResultsForSearch: 20,
                placeholder: "Select an option"
            });
        }

        function Validations() {
            $('#<%= ddldistrict.ClientID %>,#<%= ddlvehicle.ClientID %>').chosen();
            var ddlDistrict = $('#<%= ddldistrict.ClientID %> option:selected').text().toLowerCase();
            if (ddlDistrict === '--select--') {
                return alert("Please select State");
            }
            var ddlVehicle = $('#<%= ddlvehicle.ClientID %> option:selected').text().toLowerCase();
            if (ddlVehicle === '--select--') {
                return alert("Please select Vehicle");
            }
            return true;
        }
    </script>

    <table align="center">
        <tr>
            <td>
                <asp:Label Style="color: brown; font-size: 20px;" runat="server" Text=" Schedule Maintenance Status Report"></asp:Label>
            </td>
        </tr>
    </table>

    <table align="center">
        <tr>

            <td>
                State <asp:Label ID="lbldistrict" runat="server" Text="Select District" style="color: red">*</asp:Label>
            </td>

            <td>
                <asp:DropDownList ID="ddldistrict" runat="server" Style="width: 150px" AutoPostBack="true" OnSelectedIndexChanged="ddldistrict_SelectedIndexChanged"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                Vehicle<asp:Label runat="server" Text="" style="color: red">*</asp:Label>
            </td>

            <td>
                <asp:DropDownList ID="ddlvehicle" runat="server" Style="width: 150px"></asp:DropDownList>
            </td>

        </tr>

        <tr>
            <td>
                <asp:Button runat="server" Text="Report" CssClass="form-submit-button" OnClick="btnsubmit_Click" OnClientClick="if (!Validations()) return false;"></asp:Button>
            </td>

            <td>
                <asp:Button runat="server" Text="Excel" CssClass="form-reset-button" OnClick="btntoExcel_Click"></asp:Button>
            </td>

        </tr>

    </table>
    <br/>
    <div align="center">
        <asp:Panel ID="Panel2" runat="server" Style="margin-left: 2px;">
            <asp:GridView ID="Grddetails" runat="server" EmptyDataText="No Records todisplay" ShowHeaderWhenEmpty="true" GridLines="Both" BorderColor="Brown" BorderWidth="1px"></asp:GridView>
        </asp:Panel>
    </div>
</asp:Content>