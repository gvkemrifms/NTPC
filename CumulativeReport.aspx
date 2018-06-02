<%@ Page Title="" Language="C#" MasterPageFile="~/temp.master" AutoEventWireup="true" CodeFile="CumulativeReport.aspx.cs" Inherits="CumulativeReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="main">
        <div id="five" style="float: left; width: 5%">
            <div class="row" style="margin-top: 30px">
                <asp:Button ID="btntoExcel" class="form-submit-button"  runat="server" OnClick="btntoExcel_Click" Text="Export To Excel" Style="font-size: 12px; height: 33px; width: 150px;margin-top:-20px;margin-bottom:20px;margin-left:20px"></asp:Button>
            </div>
        </div>

    </div>
    <div class="row" style="margin-left: 0%;">
        <div style="margin-left: 0%; margin-right: auto;">
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Style="margin-top: 2%;"
                          EmptyDataText="No Data Found" EmptyDataRowStyle-ForeColor="Red" CssClass="gridview"
                          HeaderStyle-ForeColor="#337ab7" class="table table-striped table-bordered table-hover" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3">
                <Columns>
                    <asp:TemplateField HeaderText="S&nbsp;No">
                        <ItemTemplate>
                            <%#Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="VehicleNumber" HeaderText="Vehicle Number"/>
                    <asp:BoundField DataField="Make" HeaderText="Make"/>
                    <asp:BoundField DataField="Location" HeaderText="Location"/>
                    <asp:BoundField DataField="status" HeaderText="On Road/off Road"/>
                    <asp:BoundField DataField="Region" HeaderText="Region"/>
                    <asp:BoundField DataField="Zone" HeaderText="Zone"/>
                    <asp:BoundField DataField="District" HeaderText="District"/>
                    <asp:BoundField DataField="offdate" HeaderText="OFF ROAD DATE TIME"/>
                    <asp:BoundField DataField="offroadodo" HeaderText="OFF Road K.M"/>
                    <asp:BoundField DataField="offroadRequestedby" HeaderText="OffRoad INFORMER NAME"/>
                    <asp:BoundField DataField="UpTime" HeaderText="On Road DATE TIME"/>
                    <asp:BoundField DataField="onroadOdo" HeaderText="On Road K.M"/>
                    <asp:BoundField DataField="Onroadrequestedby" HeaderText="OnRoad INFORMER NAME"/>
                    <asp:BoundField DataField="ReasonForOffRoad" HeaderText="Main Reason"/>
                    <asp:BoundField DataField="subreasons" HeaderText="Secondary Reason"/>
                    <asp:BoundField DataField="erooffroad" HeaderText="ERO Name , While Off Road"/>
                    <asp:BoundField DataField="eroOnroad" HeaderText="ERO Name , While On Road"/>
                    <asp:BoundField DataField="pname" HeaderText="Pilot Name"/>
                    <asp:BoundField DataField="pid" HeaderText="Pilot GID"/>

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