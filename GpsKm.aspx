<%@ Page Title="" Language="C#" MasterPageFile="~/temp.master" AutoEventWireup="true" CodeFile="GpsKm.aspx.cs" Inherits="GpsKm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        function Validations() {
            var vehicleNumber = $('#<%= txtVehicleNumber.ClientID %>').val();
            if (vehicleNumber === "")
                return alert("Please enter Vehicle Number");
            var limit = $('#<%= txtLimit.ClientID %>').val();
            if (limit === "")
                return alert("Please enter Limit");
            var petrocardNumber = $('#<%= txtCardNumber.ClientID %>').val();
            if (petrocardNumber === "")
                return alert("Please enter PetroCard Number");
            return true;
        }
    </script>
    <div id="main" class="row">
        <div class="row">
            <div class="col-xs-12">
                <div class="panel">
                    <header class="panel-heading">
                        <legend style="color: brown" align="center">Vehicle List</legend>
                    </header>
                    <div class="row" runat="server" id="dvSearch" visible="false">
                        <table align="center">
                            <tr>
                                <td>
                                    Vehicle Number<span style="color: red" style="float: left">*</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtVehicleNumber" CssClass="search_3" runat="server" ReadOnly="True" class="form-control" onkeypress="return OnlyAlphaNumeric(event)"
                                                 MaxLength="12">
                                    </asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Limit<span style="color: red">*</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtLimit" CssClass="search_3" runat="server" class="form-control" onkeypress="return numericOnly(this)"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Petrocard Number <span style="color: red">*</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCardNumber" CssClass="search_3" runat="server" class="form-control" onkeypress="return numeric_only(event)"></asp:TextBox>
                                </td>

                            </tr>
                            <tr>
                                <td>
                                    Can Push Automatically<span style="color: red">*</span>
                                </td>
                                <td>
                                    <asp:CheckBox runat="server" ID="chkpush"/>
                                </td>

                            </tr>
                            <tr>
                                <td>
                                    <asp:Button runat="server" Text="Update and Submit" OnClick="btnsubmit_Click" CssClass="form-submit-button" OnClientClick="if (!Validations()) return false;" ID="btnsubmit" Width="150px"/>
                                </td>

                            </tr>
                        </table>
                    </div>
                </div>

            </div>
        </div>


        <div id="five" style="float: left; width: 5%">
            <div class="row" style="margin-left: 15px; margin-top: -10px;">
                <asp:Button ID="btntoExcel" runat="server" OnClick="btntoExcel_Click" Text="Export to Excel" CssClass="form-submit-button" Style="font-size: 12px; height: 33px; margin-top: -15px; width: 150px;"></asp:Button>
            </div>
        </div>

    </div>
    <div class="panel-body table-responsive">
        <asp:GridView ID="grdRepData" runat="server" OnRowCommand="grdRepData_RowCommand" CssClass="table table-bordered" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3">
            <Columns>
                <asp:TemplateField HeaderText="Change">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkEdit" runat="server" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "vehiclenumber") %>'
                                        CommandName="change" Text="Change">
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
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
    </div>
</asp:Content>