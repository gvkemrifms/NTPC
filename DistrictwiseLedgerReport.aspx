<%@ Page Title="" Language="C#" MasterPageFile="~/temp.master" AutoEventWireup="true" CodeFile="DistrictwiseLedgerReport.aspx.cs" Inherits="DistrictwiseLedgerReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <script type="text/javascript">
        $(function() {
            $('#<%= ddldistrict.ClientID %>').select2({
                disable_search_threshold: 5,
                search_contains: true,
                minimumResultsForSearch: 2,
                placeholder: "Select an option"
            });
            $('#<%= ddlvendor.ClientID %>').select2({
                disable_search_threshold: 5,
                search_contains: true,
                minimumResultsForSearch: 2,
                placeholder: "Select an option"
            });
        });

        function Validations() {
            var ddlDistrict = $('#<%= ddldistrict.ClientID %> option:selected').text().toLowerCase();
            if (ddlDistrict === '--select--')
                return alert("Please select District");
            var ddlVendor = $('#<%= ddlvendor.ClientID %> option:selected').text().toLowerCase();
            if (ddlVendor === '--select--')
                return alert("Please select Vendor");
            var txtFirstDate = $('#<%= txtfrmDate.ClientID %>').val();
            var txtToDate = $('#<%= txttodate.ClientID %>').val();
            if (txtFirstDate === "")
                return alert('From Date is Mandatory');
            if (txtToDate === "")
                return alert("End Date is Mandatory");
            var fromDate = (txtFirstDate).replace(/\D/g, '/');
            var toDate = (txtToDate).replace(/\D/g, '/');
            var ordFromDate = new Date(fromDate);
            var ordToDate = new Date(toDate);
            var currentDate = new Date();
            if (ordFromDate > currentDate)
                return alert("From date should not be greater than today's date");
            if (ordToDate < ordFromDate)
                return alert("Please select valid date range");
            return true;
        }
    </script>
    <table align="center">
        <tr>
            <td>
                <asp:Label style="color: brown; font-size: 20px;" runat="server" Text="DistrictwiseLedger&nbsp;Report"></asp:Label>
            </td>
        </tr>
    </table>
    <br/>
    <table align="center">
        <tr>

            <td>
                Select District <asp:Label ID="lbldistrict" runat="server" Text="Select&nbsp;District" style="color: red">*</asp:Label>
            </td>

            <td>
                <asp:DropDownList ID="ddldistrict" runat="server" style="width: 150px" AutoPostBack="true" OnSelectedIndexChanged="ddldistrict_SelectedIndexChanged"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                Select Vendor <asp:Label runat="server" Text="Select&nbsp;Vendor" style="color: red">*</asp:Label>
            </td>

            <td>
                <asp:DropDownList ID="ddlvendor" runat="server" style="width: 150px"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                From Date <asp:Label runat="server" Text="FromDate" style="color: red">*</asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtfrmDate" runat="server" CssClass="search_3" width="150px"></asp:TextBox>
            </td>
            <td>

                <cc1:CalendarExtender runat="server" Format="MM/dd/yyyy" TargetControlID="txtfrmDate" Enabled="true" CssClass="cal_Theme1"></cc1:CalendarExtender>
            </td>
        </tr>
        <tr>
            <td>
                To date <asp:Label runat="server" Text="To date" style="color: red">*</asp:Label>
            </td>

            <td>
                <asp:TextBox ID="txttodate" runat="server" CssClass="search_3" Width="150px"></asp:TextBox>
            </td>
            <td>
                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Format="MM/dd/yyyy" TargetControlID="txttodate" Enabled="true" CssClass="cal_Theme1"></cc1:CalendarExtender>

            </td>
        </tr>
        <tr>
            <td>
                <asp:Button runat="server" Text="ShowReport" CssClass="form-submit-button" OnClick="btnsubmit_Click" OnClientClick="if (!Validations()) return false;"></asp:Button>
            </td>

            <td>
                <asp:Button runat="server" Text="ExportExcel" CssClass="form-reset-button"></asp:Button>
            </td>
        </tr>
    </table>
    <br/>
    <div align="center">
        <asp:Panel ID="Panel2" runat="server" Style="margin-left: 2px;">
            <asp:GridView ID="Grddetails" EmptyDataText="Records Not Available" runat="server" ShowHeaderWhenEmpty="True" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3">
                <FooterStyle BackColor="White" ForeColor="#000066" />
                <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                <RowStyle ForeColor="#000066" />
                <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                <SortedAscendingCellStyle BackColor="#F1F1F1" />
                <SortedAscendingHeaderStyle BackColor="#007DBB" />
                <SortedDescendingCellStyle BackColor="#CAC9C9" />
                <SortedDescendingHeaderStyle BackColor="#00547E" />
            </asp:GridView>
        </asp:Panel>
    </div>
</asp:Content>