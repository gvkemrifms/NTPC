<%@ Page Title="" Language="C#" MasterPageFile="~/temp.master" AutoEventWireup="true" CodeFile="ScheduleMaintenanceStatusReport.aspx.cs" Inherits="ScheduleMaintenanceStatusReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script>
        function pageLoad() {
            $('#<%= ddldistrict.ClientID %>').select2({
                disable_search_threshold: 5,
                search_contains: true,
                minimumResultsForSearch: 20,
                placeholder: "Select an option"
            });
            $('#<%= ddlvehicle.ClientID %>').select2({
                disable_search_threshold: 5,
                search_contains: true,
                minimumResultsForSearch: 20,
                placeholder: "Select an option"
            });
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
                Select District <asp:Label ID="lbldistrict" runat="server" Text="Select District" style="color: red">*</asp:Label>
            </td>

            <td>
                <asp:DropDownList ID="ddldistrict" runat="server" Style="width: 150px" AutoPostBack="true" OnSelectedIndexChanged="ddldistrict_SelectedIndexChanged"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                Select Vehicle<asp:Label runat="server" Text="" style="color: red">*</asp:Label>
            </td>

            <td>
                <asp:DropDownList ID="ddlvehicle" runat="server" Style="width: 150px"></asp:DropDownList>
            </td>

        </tr>

        <tr>
            <td>
                <asp:Button runat="server" Text="ShowReport" CssClass="form-submit-button" OnClick="btnsubmit_Click"></asp:Button>
            </td>

            <td>
                <asp:Button runat="server" Text="ExportExcel" CssClass="form-reset-button" OnClick="btntoExcel_Click"></asp:Button>
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