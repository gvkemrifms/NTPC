<%@ Page Language="C#" MasterPageFile="~/temp.master" AutoEventWireup="true" CodeFile="MaintenanceAlert.aspx.cs" Inherits="MaintenanceAlert" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <style>
                .setposition {
                    overflow: hidden;
                    position: relative;
                }
            </style>
            <legend align="center" class="setposition" style="color: brown">Maintenance Service Alert </legend>
            <br/>
            <table align="center">
                <tr>
                    <td>
                        Select Vehicle <span style="color: red";>*</span>
                        <cc1:ComboBox AutoCompleteMode="Append" ID="ddlVehicle" runat="server" AutoPostBack="true"
                                      Width="155px" style="margin: 25px" DropDownStyle="DropDownList"
                                      onselectedindexchanged="ddlVehicle_SelectedIndexChanged">
                        </cc1:ComboBox>
                    </td>
                </tr>
                <caption>
                    <br/>
                    <tr>
                        <td>
                            <asp:GridView ID="grdMaintAlert" runat="server" AllowPaging="True" AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" CssClass="setposition" EmptyDataText="No Records Found" OnPageIndexChanging="grdMaintAlert_PageIndexChanging" Width="622px">
                                <RowStyle CssClass="rowStyleGrid" ForeColor="#000066"/>
                                <Columns>
                                    <asp:TemplateField HeaderText="Vehicle Number" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <%#Eval("VehicleNumber") %>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center"/>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Latest Odometer" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <%#Eval("Latest_odo") %>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center"/>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Last Maintenance Odo">
                                        <ItemTemplate>
                                            <%#Eval("LastMaintenanceOdo") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Last Maintenance Date" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <%#Eval("LastMaintenanceDate") %>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center"/>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Service Alert">
                                        <ItemTemplate>
                                            <%#Eval("servicealert") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <FooterStyle BackColor="White" CssClass="footerStylegrid" ForeColor="#000066"/>
                                <PagerStyle BackColor="White" CssClass="pagerStylegrid" ForeColor="#000066" HorizontalAlign="Left"/>
                                <SelectedRowStyle BackColor="#669999" CssClass="selectedRowStyle" Font-Bold="True" ForeColor="White"/>
                                <HeaderStyle BackColor="#006699" CssClass="headerStyle" Font-Bold="True" ForeColor="White"/>
                                <SortedAscendingCellStyle BackColor="#F1F1F1"/>
                                <SortedAscendingHeaderStyle BackColor="#007DBB"/>
                                <SortedDescendingCellStyle BackColor="#CAC9C9"/>
                                <SortedDescendingHeaderStyle BackColor="#00547E"/>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" valign="middle">
                            <asp:Button runat="server" CssClass="form-submit-button" OnClick="btnSendMail_Click1" OnClientClick="if (!validationFuelEntry()) return false;" Text="Send Mail"/>
                        </td>
                    </tr>
                </caption>
            </table>
            <script type="text/javascript">
                function validationFuelEntry() {
                    var districts = document.getElementById("<%= ddlVehicle.ClientID %>").control._textBoxControl.value;
                    switch (districts) {
                    case '--Select--':
                        return alert("Please Select the VehicleNumber");
                    }
                    return true;
                }
            </script>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>