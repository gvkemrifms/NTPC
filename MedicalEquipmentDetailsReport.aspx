﻿<%@ Page Title="" Language="C#" MasterPageFile="~/temp.master" AutoEventWireup="true" CodeFile="MedicalEquipmentDetailsReport.aspx.cs" Inherits="MedicalEquipmentDetailsReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <script type="text/javascript">

        function pageLoad() {
            $('#<%= ddldistrict.ClientID %>').select2({
                disable_search_threshold: 5,
                search_contains: true,
                minimumResultsForSearch: 20,
                placeholder: "Select an option"
            });
        }

        function Validations() {
            var ddlDistrict = $('#<%= ddldistrict.ClientID %> option:selected').text().toLowerCase();
            if (ddlDistrict === '--select--') {
                return alert("Please select District");
            }
            return true;
        }
    </script>

    <table align="center">
        <tr>
            <td>
                <asp:Label style="color: brown; font-size: 20px;" runat="server" Text="Medical&nbsp;Equipment&nbsp;DetailsReport"></asp:Label>
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
                <asp:DropDownList ID="ddldistrict" runat="server" style="width: 150px"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button runat="server" Text="ShowReport" CssClass="form-submit-button" OnClick="btnsubmit_Click" OnClientClick="if (!Validations()) return false;"></asp:Button>
            </td>
            <td>
                <asp:Button runat="server" Text="ExportExcel" CssClass="form-reset-button" OnClick="btntoExcel_Click"></asp:Button>

            </td>
        </tr>
    </table>
    <br/>
    <div align="center">
        <asp:Panel ID="Panel2" runat="server" Style="margin-left: 2px;">
            <asp:GridView ID="Grdtyre" runat="server" style="border-color: brown; border-width: 1px" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3">
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