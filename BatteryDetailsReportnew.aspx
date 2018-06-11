<%@ Page Title="" Language="C#" MasterPageFile="~/temp.master" AutoEventWireup="true" CodeFile="BatteryDetailsReportnew.aspx.cs" Inherits="BatteryDetailsReportnew" %>
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
        });

        function Validations() {
            var ddlDistrict = $('#<%= ddldistrict.ClientID %> option:selected').text().toLowerCase();
            if (ddlDistrict === '--select--') {
                return alert("Please select State");

            }
            return true;
        }

    </script>
    <style>
        .btnReport { margin-left: 100px; }
    </style>
    <table align="center">
        <tr>
            <td>
                <asp:Label style="color: brown; font-size: 20px;" runat="server" Text="Battery&nbsp;Details&nbsp;Report"></asp:Label>
            </td>
        </tr>
    </table>
    <br/>
    <table align="center">
        <tr>

            <td>
                 State <asp:Label ID="lbldistrict" runat="server" Text="Select&nbsp;District" style="color: red; margin-right: 1px">*</asp:Label>
            </td>

            <td>
                <asp:DropDownList ID="ddldistrict" runat="server" Style="width: 150px"></asp:DropDownList>
            </td>
        </tr>
    </table>
    <table align="center">
        <tr>
            <td>
                <asp:Button runat="server" Text="Report" CssClass="form-submit-button" OnClick="btnsubmit_Click" OnClientClick="if (!Validations()) return false;"></asp:Button>
            </td>


            <td>
                <asp:Button runat="server" Text="Excel" OnClick="btntoExcel_Click" CssClass="form-reset-button"></asp:Button>

            </td>
        </tr>
    </table>
    <br/>
    <div align="center">
        <asp:Panel ID="Panel2" runat="server" HorizontalAlign="Center">
            <asp:GridView ID="GrdBataryData" width="700px" runat="server" BorderColor="#CCCCCC" BorderWidth="1px" HorizontalAlign="Center" EmptyDataText="No Records Found" BackColor="White" BorderStyle="None" CellPadding="3">
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