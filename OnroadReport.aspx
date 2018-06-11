<%@ Page Title="" Language="C#" MasterPageFile="~/temp.master" AutoEventWireup="true" CodeFile="OnroadReport.aspx.cs" Inherits="OnroadReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <legend align="center" style="color: brown">OnRoad Report</legend>
    <br/>
    <table align="center">
        <tr>
            <td>
                <asp:Button runat="server" CssClass="form-submit-button" class="btn btn-primary"
                            Text="Show" OnClick="btnShow_Click">
                </asp:Button>
            </td>
            <td>
                <asp:Button ID="btntoExcel" runat="server" CssClass="form-submit-button" OnClick="btntoExcel_Click" style="margin-left: 20px" Text="Excel"></asp:Button>
            </td>
        </tr>
    </table>
    <br/>
    <div align="center">
        <div style="margin-left: 0%; margin-right: auto;">
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Style="margin-top: 2%;"
                          EmptyDataText="No Data Found" EmptyDataRowStyle-ForeColor="Red"
                          HeaderStyle-ForeColor="#337ab7" class="table table-striped table-bordered table-hover" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3">
                <Columns>
                    <asp:TemplateField HeaderText="S&nbsp;No">
                        <ItemTemplate>
                            <%#Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="VehicleNumber" HeaderText="Vehicle&nbsp;Number"/>
                    <asp:BoundField DataField="OffRoadVehcileId" HeaderText="Offroad Vehicle Id"/>
                    <asp:BoundField DataField="OffRoadDate" HeaderText="Inactive From"/>
                    <asp:BoundField DataField="ExpDateOfRecovery" HeaderText="Expected Date of Recovery"/>
                    <asp:BoundField DataField="downodo" HeaderText="Down Odo Reading"/>

                    <asp:BoundField DataField="District" HeaderText="District"/>
                    <asp:BoundField DataField="ReasonForOffRoad" HeaderText="Reason"/>

                    <asp:BoundField DataField="totEstCost" HeaderText="Estimated Cost"/>
                    <asp:BoundField DataField="PilotName" HeaderText="Pilot Details"/>
                    <asp:BoundField DataField="ContactNumber" HeaderText="Contact Number"/>
                    <asp:BoundField DataField="RequestedBy" HeaderText="Requested By"/>
                </Columns>

                <EmptyDataRowStyle ForeColor="Red"></EmptyDataRowStyle>

                <FooterStyle BackColor="White" ForeColor="#000066"/>

                <HeaderStyle ForeColor="White" BackColor="#006699" Font-Bold="True"></HeaderStyle>
                <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left"/>
                <RowStyle ForeColor="#000066"/>
                <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White"/>
                <SortedAscendingCellStyle BackColor="#F1F1F1"/>
                <SortedAscendingHeaderStyle BackColor="#007DBB"/>
                <SortedDescendingCellStyle BackColor="#CAC9C9"/>
                <SortedDescendingHeaderStyle BackColor="#00547E"/>
            </asp:GridView>
        </div>
    </div>
</asp:Content>