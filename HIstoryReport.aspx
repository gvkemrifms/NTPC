<%@ Page Title="" Language="C#" MasterPageFile="~/temp.master" AutoEventWireup="true" CodeFile="HistoryReport.aspx.cs" Inherits="HistoryReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <script type="text/javascript">
        $(function() {
            $('#<%= ddldistrict.ClientID %>,#<%= ddlvehicle.ClientID %>').select2({
                disable_search_threshold: 5,
                search_contains: true,
                minimumResultsForSearch: 20,
                placeholder: "Select an option"
            });
        });

        function Validations() {
            var ddlDistrict = $('#<%= ddldistrict.ClientID %> option:selected').text().toLowerCase();
            if (ddlDistrict === '--select--') {
                return alert("Please select State");
            }
            var ddlVehicle = $('#<%= ddlvehicle.ClientID %> option:selected').text().toLowerCase();
            if (ddlVehicle === '--select--') {
                return alert("Please select Vehicle");
            }
            var ddlMonth = $('#<%= ddlmonth.ClientID %> option:selected').text().toLowerCase();
            if (ddlMonth === '--select--') {
                return alert("Please select Month of Registration");
            }
            var ddlYear = $('#<%= ddlyear.ClientID %> option:selected').text().toLowerCase();
            if (ddlYear === '--select--') {
                return alert("Please select Year of Registration");
            }
            return true;
        }
    </script>
    <table align="center">
        <tr>
            <td>
                <asp:Label style="color: brown; font-size: 20px;" runat="server" Text="History&nbsp;Report"></asp:Label>
            </td>
        </tr>
    </table>
    <br/>
    <table align="center">
        <tr>

            <td>
                State <asp:Label ID="lbldistrict" runat="server" Text="" style="color: red">*</asp:Label>
            </td>

            <td>
                <asp:DropDownList ID="ddldistrict" runat="server" style="width: 150px" AutoPostBack="true" OnSelectedIndexChanged="ddldistrict_SelectedIndexChanged"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                Vehicle <asp:Label runat="server" Text="" style="color: red">*</asp:Label>
            </td>

            <td>
                <asp:DropDownList ID="ddlvehicle" runat="server" style="width: 150px"></asp:DropDownList>
            </td>

        </tr>
        <tr>
            <td>
                Month Of Registration<asp:Label runat="server" Text="" style="color: red">*</asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddlmonth" runat="server" style="width: 150px" AutoPostBack="true" CssClass="search_3">
                    <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                    <asp:ListItem Text="January" Value="1"></asp:ListItem>
                    <asp:ListItem Text="February" Value="2"></asp:ListItem>
                    <asp:ListItem Text="March" Value="3"></asp:ListItem>
                    <asp:ListItem Text="April" Value="4"></asp:ListItem>
                    <asp:ListItem Text="May" Value="5"></asp:ListItem>
                    <asp:ListItem Text="June" Value="6"></asp:ListItem>
                    <asp:ListItem Text="July" Value="7"></asp:ListItem>
                    <asp:ListItem Text="August" Value="8"></asp:ListItem>
                    <asp:ListItem Text="September" Value="9"></asp:ListItem>
                    <asp:ListItem Text="October" Value="10"></asp:ListItem>
                    <asp:ListItem Text="November" Value="11"></asp:ListItem>
                    <asp:ListItem Text="December" Value="12"></asp:ListItem>
                </asp:DropDownList>

            </td>

        </tr>

        <tr>

            <td>
                Year Of Registration<asp:Label runat="server" Text="" style="color: red">*</asp:Label>
            </td>

            <td>
                <asp:DropDownList ID="ddlyear" runat="server" style="width: 150px" AutoPostBack="true" CssClass="search_3">
                    <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                    <asp:ListItem Text="2004" Value="2004"></asp:ListItem>
                    <asp:ListItem Text="2005" Value="2005"></asp:ListItem>
                    <asp:ListItem Text="2006" Value="2006"></asp:ListItem>
                    <asp:ListItem Text="2007" Value="2007"></asp:ListItem>
                    <asp:ListItem Text="2008" Value="2008"></asp:ListItem>
                    <asp:ListItem Text="2009" Value="2009"></asp:ListItem>
                    <asp:ListItem Text="2010" Value="2010"></asp:ListItem>
                    <asp:ListItem Text="2011" Value="2011"></asp:ListItem>
                    <asp:ListItem Text="2012" Value="2012"></asp:ListItem>
                    <asp:ListItem Text="2013" Value="2013"></asp:ListItem>
                    <asp:ListItem Text="2014" Value="2014"></asp:ListItem>
                    <asp:ListItem Text="2015" Value="2015"></asp:ListItem>
                    <asp:ListItem Text="2016" Value="2016"></asp:ListItem>
                    <asp:ListItem Text="2017" Value="2017"></asp:ListItem>
                    <asp:ListItem Text="2018" Value="2018"></asp:ListItem>
                    <asp:ListItem Text="2019" Value="2019"></asp:ListItem>
                    <asp:ListItem Text="2020" Value="2020"></asp:ListItem>

                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button runat="server" Text="Report" OnClick="btnsubmit_Click" CssClass="form-submit-button" OnClientClick="if (!Validations()) return false;"></asp:Button>
            </td>

            <td>
                <asp:Button runat="server" Text="Excel" CssClass="form-reset-button" OnClick="btntoExcel_Click"></asp:Button>
            </td>
        </tr>
    </table>
    <br/>

    <div align="center">
        <asp:Panel ID="Panel2" runat="server" Style="margin-left: 2px;">
            <asp:GridView ID="Grddetails" EmptyDataText="Records Not Available" runat="server" ShowHeaderWhenEmpty="True" BorderWidth="1px" BorderColor="#CCCCCC" BackColor="White" BorderStyle="None" CellPadding="3">
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