<%@ Page Language="C#" MasterPageFile="~/temp.master" AutoEventWireup="true" CodeFile="Equipment.aspx.cs" Inherits="Equipment" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<asp:UpdatePanel runat="server">
<ContentTemplate>
<script type="text/javascript">
    function pageLoad() {
        $('#<%= ddlistVehicleNumber.ClientID %>').select2({
            disable_search_threshold: 5,
            search_contains: true,
            minimumResultsForSearch: 20,
            placeholder: "Select an option"
        });
    }
</script>
<script type="text/javascript">
    function validation() {
        var vehnum = document.getElementById('<%= ddlistVehicleNumber.ClientID %>');
        if (vehnum.selectedIndex === 0) {
            alert("Please select a Vehicle");
            vehnum.focus();
            return false;
        }
        return true;
    }
</script>
<table align="center">
<tr align="center">
<td>
<fieldset style="padding: 10px;">
    <legend align="center" style="color:brown">Vehicle Equipment Mapping </legend>
    <asp:Panel runat="server">
        <table align="center">
            <tr align="center">
                <td class="tdlabel">
                    Vehicle Number<font color="red">*</font>
                </td>
                <td class="columnseparator"></td>
                <td>
                    <asp:DropDownList ID="ddlistVehicleNumber" runat="server" Width="150px" AutoPostBack="True"
                                      OnSelectedIndexChanged="ddlistVehicleNumber_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <br/>
        <br/>
        <table align="center" border="1px">
            <tr >
                <td colspan="3">
                    <div style="height: 120px; overflow: scroll;">
                        <asp:GridView ID="grdviewMedicalEqupment" runat="server" AutoGenerateColumns="False"
                                      CellPadding="4" CellSpacing="4" GridLines="None" Width="380px" EmptyDataText="No Records Found"
                                      CssClass="gridviewStyle">
                            <RowStyle CssClass="rowStyleGrid"/>
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkMedicalEquipment" runat="server"/>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Medical Equipment">
                                    <ItemTemplate>
                                        <asp:Label ID="LblMedicalEquipmentName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "EquipmentName") %>'></asp:Label>
                                        <asp:Label ID="LblMedicalEquipmentId" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Id") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <FooterStyle CssClass="footerStylegrid"/>
                            <PagerStyle CssClass="pagerStylegrid"/>
                            <SelectedRowStyle CssClass="selectedRowStyle"/>
                            <HeaderStyle CssClass="headerStyle"/>
                        </asp:GridView>
                    </div>
                </td>
                <td colspan="3">
                    <div style="height: 120px; overflow: scroll;">
                        <asp:GridView ID="grdviewMedicalDisposables" runat="server" AutoGenerateColumns="False"
                                      CellPadding="4" CellSpacing="4" GridLines="None" Width="380px" EmptyDataText="No Records Found"
                                      CssClass="gridviewStyle">
                            <RowStyle CssClass="rowStyleGrid"/>
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkMedicalDisposables" runat="server"/>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Medical Disposables">
                                    <ItemTemplate>
                                        <asp:Label ID="LblDisposableName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "EquipmentName") %>'></asp:Label>
                                        <asp:Label ID="LblDisposableId" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Id") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <FooterStyle CssClass="footerStylegrid"/>
                            <PagerStyle CssClass="pagerStylegrid"/>
                            <SelectedRowStyle CssClass="selectedRowStyle"/>
                            <HeaderStyle CssClass="headerStyle"/>
                        </asp:GridView>
                    </div>
                </td>
            </tr>
        </table>
        <br/>
        <br/>
        <table border="1px">
            <tr>
                <td colspan="3">
                    <div style="height: 120px; overflow: scroll;">
                        <asp:GridView ID="grdviewExtricationTools" runat="server" AutoGenerateColumns="False"
                                      CellPadding="4" CellSpacing="4" GridLines="None" Width="380px" EmptyDataText="No Records Found"
                                      CssClass="gridviewStyle">
                            <RowStyle CssClass="rowStyleGrid"/>
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkExtricationTools" runat="server"/>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Extrication Tools">
                                    <ItemTemplate>
                                        <asp:Label ID="LblExtricationName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "EquipmentName") %>'></asp:Label>
                                        <asp:Label ID="LblExtricationId" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Id") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <FooterStyle CssClass="footerStylegrid"/>
                            <PagerStyle CssClass="pagerStylegrid"/>
                            <SelectedRowStyle CssClass="selectedRowStyle"/>
                            <HeaderStyle CssClass="headerStyle"/>
                        </asp:GridView>
                    </div>
                </td>
                <td colspan="3">
                    <div style="height: 120px; overflow: scroll;">
                        <asp:GridView ID="grdviewCOmmunicationTechnology" runat="server" AutoGenerateColumns="False"
                                      CellPadding="4" CellSpacing="4" GridLines="None" Width="380px" EmptyDataText="No Records Found"
                                      CssClass="gridviewStyle">
                            <RowStyle CssClass="rowStyleGrid"/>
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkCommunicationTechnology" runat="server"/>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Communication & Technology Equipment">
                                    <ItemTemplate>
                                        <asp:Label ID="LblCommunicationName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "EquipmentName") %>'></asp:Label>
                                        <asp:Label ID="LblCommunicationId" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Id") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <FooterStyle CssClass="footerStylegrid"/>
                            <PagerStyle CssClass="pagerStylegrid"/>
                            <SelectedRowStyle CssClass="selectedRowStyle"/>
                            <HeaderStyle CssClass="headerStyle"/>
                        </asp:GridView>
                    </div>
                </td>
            </tr>
        </table>
        <br/>
        <br/>
        <table border="1px">
            <tr>
                <td colspan="3">
                    <div style="height: 120px; overflow: scroll;">
                        <asp:GridView ID="grdviewMedicines" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                      CellSpacing="4" GridLines="None" Width="380px" EmptyDataText="No Records Found"
                                      CssClass="gridviewStyle">
                            <RowStyle CssClass="rowStyleGrid"/>
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkMedicines" runat="server"/>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Medicines">
                                    <ItemTemplate>
                                        <asp:Label ID="LblMedicineName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "EquipmentName") %>'></asp:Label>
                                        <asp:Label ID="LblMedicineId" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Id") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <FooterStyle CssClass="footerStylegrid"/>
                            <PagerStyle CssClass="pagerStylegrid"/>
                            <SelectedRowStyle CssClass="selectedRowStyle"/>
                            <HeaderStyle CssClass="headerStyle"/>
                        </asp:GridView>
                    </div>
                </td>
                <td colspan="3">
                    <div style="height: 120px; overflow: scroll;">
                        <asp:GridView ID="grdviewNoMedicalSupplies" runat="server" AutoGenerateColumns="False"
                                      CellPadding="4" CellSpacing="4" GridLines="None" Width="380px" EmptyDataText="No Records Found"
                                      CssClass="gridviewStyle">
                            <RowStyle CssClass="rowStyleGrid"/>
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkNoMedicalSupplies" runat="server"/>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Non Medical Supplies">
                                    <ItemTemplate>
                                        <asp:Label ID="LblNoMedicalName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "EquipmentName") %>'></asp:Label>
                                        <asp:Label ID="LblNoMedicalId" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Id") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <FooterStyle CssClass="footerStylegrid"/>
                            <PagerStyle CssClass="pagerStylegrid"/>
                            <SelectedRowStyle CssClass="selectedRowStyle"/>
                            <HeaderStyle CssClass="headerStyle"/>
                        </asp:GridView>
                    </div>
                </td>
            </tr>

        </table>
        <div align="center">
            <asp:Button ID="BtnSave" Text="Save" runat="server" CssClass="form-submit-button" OnClick="BtnSave_Click"/>
            <asp:Button runat="server" Text="Cancel" CssClass="form-reset-button" OnClick="Button1_Click"/>
        </div>
    </asp:Panel>
</fieldset>
</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>