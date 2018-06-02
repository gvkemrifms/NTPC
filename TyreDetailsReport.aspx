<%@ Page Title="" Language="C#" MasterPageFile="~/temp.master" AutoEventWireup="true" CodeFile="TyreDetailsReport.aspx.cs" Inherits="TyreDetailsReport" %>

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
        });
    </script>
    <table align="center">
        <tr>
            <td>
                <asp:Label Style="color: brown; font-size: 20px;" runat="server" Text="Tyre Details Report"></asp:Label>
            </td>
        </tr>
    </table>
    <br/>
    <table align="center">
        <tr>

            <td>
                <asp:Label ID="lbldistrict" runat="server" Text="Select&nbsp;District"></asp:Label>
            </td>

            <td>
                <asp:DropDownList ID="ddldistrict" runat="server" Style="width: 150px"></asp:DropDownList>
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
            <asp:GridView ID="Grdtyre" runat="server" BorderColor="#CCCCCC" BorderWidth="1px" BackColor="White" width="700px" BorderStyle="None" CellPadding="3">
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