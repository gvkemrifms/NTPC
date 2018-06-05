<%@ Page Title="" Language="C#" MasterPageFile="~/temp.master" AutoEventWireup="true" CodeFile="FuelReport.aspx.cs" Inherits="FuelReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <script type="text/javascript">
        $(function() {
            $('#<%=txtfromdate.ClientID%>').datepicker({
                dateFormat: 'yy/mm/dd',
                changeMonth: true,
                changeYear: true
            });
            $('#<%=txttodate.ClientID%>').datepicker({
                dateFormat: 'yy/mm/dd',
                changeMonth: true,
                changeYear: true
            });
        });
        function Validations() {
            var txtFirstDate = $('#<%= txtfromdate.ClientID %>').val();
            var txtToDate = $('#<%= txttodate.ClientID %>').val();
            if (txtFirstDate === "") {
                return alert('From Date is Mandatory');

            }
            if (txtToDate === "") {
                return alert("End Date is Mandatory");

            }
            var fromDate = (txtFirstDate).replace(/\D/g, '/');
            var toDate = (txtToDate).replace(/\D/g, '/');
            var ordFromDate = new Date(fromDate);
            var ordToDate = new Date(toDate);
            var currentDate = new Date();
            if (ordFromDate > currentDate) {
                return alert("From date should not be greater than today's date");

            }
            if (ordToDate < ordFromDate) {
                return alert("Please select valid date range");
            }
            return true;
        }
    </script>
    <legend align="center" style="color: brown">FuelReport</legend>
    <br/>
    <div id="main" align="center">

        <div id="mains" style="width: 100%;">
            <div id="first" style="float: left; width: 22%">
                <div class="row" style="margin-top: 20px">
                    <div style="">
                        <label for="textfield" class="control-label col-sm-4" style="float: left; width: 23%;" id="PVlSuppCode9">
                            From&nbsp;Date
                        </label>

                        <div class="col-sm-6" style="width: 218px;" id="PVtSuppCode9">
                            <asp:TextBox ID="txtfromdate" style="width: 170px;" runat="server" placeholder="" MaxLength="20"
                                         class="search_3">
                            </asp:TextBox>
                            <style type="text/css">
                                .test .ajax__calendar_body {
                                    background-color: gainsboro;
                                    border: 1px solid;
                                    color: lightslategray;
                                    font-family: Courier New;
                                    font-weight: bold;
                                    margin-top: 10px;
                                    width: 170px;
                                }

                                .test .ajax__calendar_header {
                                    background-color: gainsboro;
                                    width: 170px;
                                }
                            </style>

                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="two" style="float: left; width: 20%">
            <div class="row" style="margin-top: 20px">
                <div style="">
                    <label class="control-label col-sm-4" style="float: left; width: 20%;" id="PVlSuppCode1">
                        To&nbsp;Date
                    </label>
                    <div class="col-sm-6" style="width: 218px;" id="PVtSuppCode1">
                        <asp:TextBox ID="txttodate" style="width: 170px;" CssClass="search_3" runat="server" placeholder="" MaxLength="20"
                                     class="form-control">
                        </asp:TextBox>
                    </div>
                </div>
            </div>
        </div>

        <div id="four" style="float: left; width: 6%">
            <div class="row" style="margin-top: 30px">
                <div class="col-sm-12" style="">
                    <asp:Button runat="server" class="btn btn-primary"
                                Text="Show" Style="border-radius: 3px; height: 33px; width: 55px;" OnClick="btnShow_Click" CssClass="form-submit-button" OnClientClick="if(!Validations()) return false;">
                    </asp:Button>
                </div>
            </div>
        </div>
        <div id="five" style="float: left; width: 5%">
            <div class="row" style="margin-top: 30px">
                <asp:Button ID="btntoExcel" runat="server" CssClass="form-reset-button" OnClick="btntoExcel_Click" Text="Excel" Style="font-size: 12px; height: 33px; width: 50px;" OnClientClick="if (!Validations()) return false;"></asp:Button>
            </div>
        </div>

    </div>
    <br/>
    <div align="center">
        <div align="center">
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Style="margin-top: 2%;"
                          EmptyDataText="No Data Found" EmptyDataRowStyle-ForeColor="Red"
                          HeaderStyle-ForeColor="#337ab7"  class="gridview" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3">
                <Columns>
                    <asp:TemplateField HeaderText="S&nbsp;No">
                        <ItemTemplate>
                            <%#Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="VehicleNumber" HeaderText="Vehicle Number"/>
                    <asp:BoundField DataField="fuelentryid" HeaderText="Fuel Entry ID"/>
                    <asp:BoundField DataField="Previous_odo" HeaderText="Opening ODO"/>
                    <asp:BoundField DataField="present_odo" HeaderText="Closing ODO"/>
                    <asp:BoundField DataField="Quantity" HeaderText="Quantity"/>
                    <asp:BoundField DataField="Location" HeaderText="Location"/>
                    <asp:BoundField DataField="KMPL" HeaderText="KMPL"/>
                    <asp:BoundField DataField="Totalkm_Run" HeaderText="Totalkm&nbsp;Run"/>
                    <asp:BoundField DataField="EntryDate" HeaderText="Filling&nbsp;Date"/>
                    <asp:BoundField DataField="Pilot" HeaderText="Pilot&nbsp;Id"/>
                    <asp:BoundField DataField="PilotName" HeaderText="Pilot&nbsp;Name"/>
                    <asp:BoundField DataField="Amount" HeaderText="Amount"/>

                </Columns>

<EmptyDataRowStyle ForeColor="Red"></EmptyDataRowStyle>

                <FooterStyle BackColor="White" ForeColor="#000066" />

<HeaderStyle ForeColor="White" BackColor="#006699" Font-Bold="True"></HeaderStyle>
                <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                <RowStyle ForeColor="#000066" />
                <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                <SortedAscendingCellStyle BackColor="#F1F1F1" />
                <SortedAscendingHeaderStyle BackColor="#007DBB" />
                <SortedDescendingCellStyle BackColor="#CAC9C9" />
                <SortedDescendingHeaderStyle BackColor="#00547E" />
            </asp:GridView>
        </div>
    </div>
</asp:Content>