<%@ Page Title="" Language="C#" MasterPageFile="~/temp.master" AutoEventWireup="true" CodeFile="TyreAndBatteryDistrictWise.aspx.cs" Inherits="TyreAndBatteryDistrictWise" %>
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
        };
    </script>

    <table align="center">
        <tr>
            <td>
                <asp:Label Style="color: brown; font-size: 20px;" runat="server" Text="Tyre And Battery District wise Details Report"></asp:Label>
            </td>
        </tr>
    </table>
    <br/>
    <table align="center">
        <tr>

            <td>
                State<asp:Label ID="lbldistrict" runat="server" Text="" style="color: red"></asp:Label>
            </td>

            <td>
                <asp:DropDownList ID="ddldistrict" runat="server" Style="width: 150px"></asp:DropDownList>
            </td>
        </tr>
        <tr>

            <td>
                <asp:Button runat="server" CssClass="form-submit-button" Text="Report" OnClick="btnsubmit_Click"></asp:Button>
            </td>

            <td>
                <asp:Button runat="server" CssClass="form-reset-button" Text="Excel" OnClick="btntoExcel_Click"></asp:Button>
            </td>
        </tr>
    </table>
    <br/>
    <div align="center">
        <asp:Panel ID="Panel2" runat="server" HorizontalAlign="Center">
            <asp:GridView ID="GrdtyreBattery" BorderColor="#CCCCCC" BorderWidth="1px" HorizontalAlign="Center" EmptyDataText="No Records Found" runat="server" BackColor="White" BorderStyle="None" CellPadding="3">
                <FooterStyle BackColor="White" ForeColor="#000066"/>
                <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White"/>
                <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left"/>
                <RowStyle ForeColor="#000066"/>
                <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White"/>
                <SortedAscendingCellStyle BackColor="#F1F1F1"/>
                <SortedAscendingHeaderStyle BackColor="#007DBB"/>
                <SortedDescendingCellStyle BackColor="#CAC9C9"/>
                <SortedDescendingHeaderStyle BackColor="#00547E"/>
            </asp:GridView>
        </asp:Panel>
    </div>
</asp:Content>