<%@ Page Title="" Language="C#" MasterPageFile="~/temp.master" AutoEventWireup="true" CodeFile="AnalysisHourwiseReport.aspx.cs" Inherits="AnalysisHourwiseReport" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        $(function() {
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
        })
    </script>

    <script type="text/javascript">
        function Validations() {
            var ddlDistrict = $('#<%= ddldistrict.ClientID %> option:selected').text().toLowerCase();
            if (ddlDistrict === '--select--')
                return alert("Please select District");
            var ddlVehicle = $('#<%= ddlvehicle.ClientID %> option:selected').text().toLowerCase();
            if (ddlVehicle === '--select--')
                return alert("Please select Vehicle");
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
            if (ordFromDate > currentDate) {
                return alert("From date should not be greater than today's date");
            }
            if (ordToDate < ordFromDate) {
                alert("Please select valid date range");
            }
            return true;
        }
    </script>
    <table align="center">
        <tr>
            <td>
                <asp:Label Style="color: brown; font-size: 20px;" runat="server" Text="Analysis Hour Wise&nbsp;Report"></asp:Label>
            </td>
        </tr>
    </table>
    <br/>
    <br/>
    <table align="center">
        <tr>

            <td>
                Select District<asp:Label ID="lbldistrict" runat="server" Text="Select&nbsp;District" style="color: red">*</asp:Label>
            </td>

            <td>
                <asp:DropDownList ID="ddldistrict" runat="server" Style="width: 150px" AutoPostBack="true" OnSelectedIndexChanged="ddldistrict_SelectedIndexChanged"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                Select Vehicle<asp:Label runat="server" Text="Select&nbsp;Vehicle" style="color: red">*</asp:Label>
            </td>

            <td>
                <asp:DropDownList ID="ddlvehicle" runat="server" Style="width: 150px"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                From Date <asp:Label runat="server" Text="FromDate" style="color: red">*</asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtfrmDate" runat="server" CssClass="search_3" onkeypress="return false"></asp:TextBox>
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
                <asp:TextBox ID="txttodate" runat="server" CssClass="search_3" onkeypress="return false"></asp:TextBox>
            </td>
            <td>
                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Format="MM/dd/yyyy" TargetControlID="txttodate" Enabled="true" CssClass="cal_Theme1"></cc1:CalendarExtender>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button runat="server" Text="ShowReport" OnClick="btnsubmit_Click" CssClass="form-submit-button" OnClientClick="if (!Validations()) return false;"></asp:Button>
            </td>

            <td>
                <asp:Button runat="server" Text="ExportExcel" OnClick="btntoExcel_Click" CssClass="form-reset-button"></asp:Button>
            </td>
        </tr>
    </table>
    <br/>
    <br/>
            <asp:GridView ID="Grddetails" HorizontalAlign="Center" runat="server" BorderColor="#CCCCCC" BorderWidth="1px" BackColor="White" BorderStyle="None" CellPadding="3">
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
</asp:Content>