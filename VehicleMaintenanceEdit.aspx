<%@ Page Language="C#" MasterPageFile="~/temp.master" AutoEventWireup="true" CodeFile="VehicleMaintenanceEdit.aspx.cs" Inherits="VehicleMaintenanceEdit" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<script language="javascript" type="text/javascript">

    function validation() {

        var vehicleNumber = document.getElementById('<%= ddlVehicleNumber.ClientID %>');
        var district = document.getElementById('<%= ddlDistrict.ClientID %>');
        if (district && district.selectedIndex === 0) {
            alert("Please select District");
            district.focus();
            return false;
        }

        if (vehicleNumber && vehicleNumber.selectedIndex === 0) {
            alert("Please select Vehicle Number");
            vehicleNumber.focus();
            return false;
        }
        return true;
    }


</script>


<asp:UpdatePanel runat="server">
<ContentTemplate>
<script type="text/javascript">
    function pageLoad() {
        $('#<%= ddlDistrict.ClientID %>').select2({
            disable_search_threshold: 5,
            search_contains: true,
            minimumResultsForSearch: 20,
            placeholder: "Select an option"
        });
        $('#<%= ddlVehicleNumber.ClientID %>').select2({
            disable_search_threshold: 5,
            search_contains: true,
            minimumResultsForSearch: 20,
            placeholder: "Select an option"
        });
    }
</script>
<table align="center">
<tr>
<td>
<fieldset style="padding: 10px">
<legend align="center" style="color: brown">
    Vehicle Maintenance Details
</legend>
<table align="center">

    <tr>
    <td>
        District
    </td>
    <td>

        <asp:DropDownList ID="ddlDistrict" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlDistrict_SelectedIndexChanged">
            <asp:ListItem Value="-1">--Select--</asp:ListItem>
        </asp:DropDownList>
    </td>
    <tr>
        <td>
            Vehicle No
        </td>

        <td>
            <asp:DropDownList ID="ddlVehicleNumber" runat="server" Width="135px" AutoPostBack="True"
                              OnSelectedIndexChanged="ddlVehicleNumber_SelectedIndexChanged">
                <asp:ListItem Value="-1">--Select--</asp:ListItem>
            </asp:DropDownList>
            <asp:TextBox ID="txtVehicleNumber" runat="server" Visible="false" onkeypress="return false;"></asp:TextBox>
        </td>
    </tr>

    <tr>
        <td class="rowseparator"></td>
    </tr>
    <tr>
        <td nowrap="nowrap" visible="false"></td>
        <td>
            <asp:TextBox ID="txtMaintenanceType" runat="server" Width="135px" onkeypress="return false;" Visible="false"></asp:TextBox>

        </td>
        <td></td>
    </tr>

</table>
<br/>
<fieldset style="padding: 10px 10px 10px 10px" id="fsMaintenance" runat="server">
<legend align="center">
    <asp:CheckBox ID="chkAmount" runat="server" Text="No Maintenance Amount" OnCheckedChanged="chkAmount_CheckedChanged"
                  AutoPostBack="true"/>
</legend>
<table align="center">
    <tr>
        <td>
            <table align="center">
                <tr>
                    <td class="rowseparator"></td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBoxList ID="chkbxlistBillType" runat="server" CellPadding="10" CellSpacing="10"
                                          RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="chkbxlistBillType_SelectedIndexChanged">
                            <asp:ListItem Value="SpareParts">Spare Parts</asp:ListItem>
                            <asp:ListItem Value="Lubricant">Lubricant</asp:ListItem>
                            <asp:ListItem Value="LabourCharges">Labour Charges</asp:ListItem>
                        </asp:CheckBoxList>
                    </td>
                </tr>
                <tr>
                    <td class="rowseparator"></td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<asp:Panel ID="pnlSPBillDetails" runat="server" Visible="false">
    <fieldset style="padding: 10px 10px 0px 10px">
        <legend align="center">Spare Parts </legend>
        <table align="center">
            <tr>
                <td>
                    <table align="center">
                        <tr>
                            <td>
                                <asp:GridView ID="grdvwSPBillDetails" runat="server" AutoGenerateColumns="False"
                                              BackColor="White" BorderColor="#CCCCCC" BorderWidth="1px"
                                              CellPadding="3"
                                              DataKeyNames="RowNumber" CssClass="gridviewStyle"
                                              OnRowDataBound="grdvwSPBillDetails_RowDataBound" BorderStyle="None">
                                    <RowStyle CssClass="rowStyleGrid" Width="100%" ForeColor="#000066"/>
                                    <Columns>
                                        <asp:BoundField DataField="RowNumber" HeaderText="S.No" ItemStyle-HorizontalAlign="Center">
                                        <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRowno" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Vendor Name">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlSpareVendorName" runat="server" Width="60px"/>

                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Bill No.">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtSpareBillNo" runat="server" Width="60px" MaxLength="10" Text='<%# Eval("ColSpBillno") %>'
                                                             onkeypress="return alphanumeric_only(event);">
                                                </asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Bill Date">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtSpareBillDate" runat="server" Width="60px" Wrap="true" onpaste="return false"
                                                             Text='<%# Eval("ColSpBillDate") %>' oncopy="return false" oncut="return false"
                                                             onkeypress="return false">
                                                </asp:TextBox>
                                                <cc1:CalendarExtender ID="calextndrSpareBillDate" runat="server" Format="dd/MM/yyyy"
                                                                      PopupButtonID="imgBtnQuotationDate" TargetControlID="txtSpareBillDate">
                                                </cc1:CalendarExtender>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="EMRI Part Code">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtSpareEMRIpc" runat="server" Width="70px" MaxLength="10" Text='<%# Eval("ColSpEMRIPartCode") %>'
                                                             onkeypress="return alphanumeric_only(event);">
                                                </asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Part Code">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtSparePartCode" runat="server" Width="60px" MaxLength="10" Text='<%# Eval("ColSpPartCode") %>'
                                                             onkeypress="return alphanumeric_only(event);">
                                                </asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Item Descrip">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlSpareItemDesc" runat="server" Width="60px"
                                                                  OnSelectedIndexChanged="ddlSpareItemDesc_SelectedIndexChanged"/>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Quantity">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtSpareQuant" runat="server" Width="60px" MaxLength="10" Text='<%# Eval("ColSpQuantity") %>'
                                                             onkeypress="return isNumberKey(event);">
                                                </asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Bill Amount">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtSpareBillAmount" runat="server" Width="60px" MaxLength="6" onkeypress="return isDecimalNumberKey(event);"
                                                             Text='<%# Eval("Column3") %>'>
                                                </asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle CssClass="footerStylegrid" BackColor="White" ForeColor="#000066"/>
                                    <PagerStyle CssClass="pagerStylegrid" BackColor="White" ForeColor="#000066" HorizontalAlign="Left"/>
                                    <SelectedRowStyle CssClass="selectedRowStyle" BackColor="#669999" Font-Bold="True" ForeColor="White"/>
                                    <HeaderStyle CssClass="headerStyle" BackColor="#006699" Font-Bold="True" ForeColor="White"/>
                                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                    <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                    <SortedDescendingHeaderStyle BackColor="#00547E" />
                                </asp:GridView>

                            </td>
                        </tr>
                        <tr>
                            <td class="rowseparator"></td>
                        </tr>
                        <tr>
                            <td>
                                <table width="100%" align="center">
                                    <tr>
                                        <td align="center" style="height: 26px">
                                            <asp:Button ID="btnAddNewSPRow" CssClass="form-submit-button" runat="server" Text="Add Row" OnClick="btnAddNewSPRow_Click"/>
                                        </td>
                                        <td style="height: 26px">
                                            <asp:Button ID="btnSPReset" CssClass="form-reset-button" runat="server" Text="Reset" OnClick="btnSPReset_Click"/>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
                <td></td>
            </tr>
        </table>
    </fieldset>
    <br/>
</asp:Panel>
<asp:Panel ID="pnlLubricantBillDetails" runat="server" Visible="false">
    <fieldset style="padding: 10px 10px 0px 10px">
        <legend>Lubricant</legend>
        <table width="100%">
            <tr>
                <td>
                    <table align="center">
                        <tr>
                            <td>
                                <asp:GridView ID="grdvwLubricantBillDetails" runat="server" AutoGenerateColumns="False"
                                              BackColor="LightGoldenrodYellow" BorderColor="Tan" BorderWidth="1px"
                                              CellPadding="2"
                                              DataKeyNames="RowNumberLubri" CssClass="gridviewStyle" GridLines="None"
                                              OnRowDataBound="grdvwLubricantBillDetails_RowDataBound" ForeColor="Black">
                                    <RowStyle CssClass="rowStyleGrid" Width="100%"/>
                                    <AlternatingRowStyle BackColor="PaleGoldenrod" />
                                    <Columns>
                                        <asp:BoundField DataField="RowNumberLubri" HeaderText="S.No" ItemStyle-HorizontalAlign="Center">
                                        <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRowno" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Vendor Name">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlLubricantVendorName" runat="server" Width="60px"/>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Bill Number">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtLubricantBillNo" runat="server" Width="60px" MaxLength="10" Text='<%# Eval("ColLubriBillNo") %>'
                                                             onkeypress="return alphanumeric_only(event);">
                                                </asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Bill Date">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtLubricantBillDate" runat="server" Width="60px" Wrap="true" onpaste="return false"
                                                             Text='<%# Eval("ColLubriBillDate") %>' oncopy="return false" oncut="return false"
                                                             onkeypress="return false">
                                                </asp:TextBox>
                                                <cc1:CalendarExtender ID="calextndrLubricantBillDate" runat="server" Format="dd/MM/yyyy"
                                                                      PopupButtonID="imgBtnQuotationDate" TargetControlID="txtLubricantBillDate">
                                                </cc1:CalendarExtender>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="EMRI Part Code">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtLubricantEMRIpc" runat="server" Width="70px" MaxLength="10"
                                                             Text='<%# Eval("ColLubriEMRIPartCode") %>'
                                                             onkeypress="return alphanumeric_only(event);">
                                                </asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Part Code">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtLubricantPartCode" runat="server" Width="60px" MaxLength="10"
                                                             Text='<%# Eval("ColLubriPartCode") %>' onkeypress="return alphanumeric_only(event);">
                                                </asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Item Descp.">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtLubricantItemDesc" runat="server" Width="60px" MaxLength="10"
                                                             Text='<%# Eval("ColLubriItemDescription") %>' onkeypress="return alpha_only(event);">
                                                </asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Quantity">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtLubricantQuant" runat="server" Width="60px" MaxLength="10"
                                                             Text='<%# Eval("ColLabQuantity") %>'
                                                             onkeypress="return isNumberKey(event);">
                                                </asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Bill Amount">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtLubricantBillAmount" runat="server" Width="60px" MaxLength="6"
                                                             Text='<%# Eval("ColLubriBillAmount") %>' onkeypress="return isDecimalNumberKey(event);">
                                                </asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle CssClass="footerStylegrid" BackColor="Tan"/>
                                    <PagerStyle CssClass="pagerStylegrid" BackColor="PaleGoldenrod" ForeColor="DarkSlateBlue" HorizontalAlign="Center"/>
                                    <SelectedRowStyle CssClass="selectedRowStyle" BackColor="DarkSlateBlue" ForeColor="GhostWhite"/>
                                    <HeaderStyle CssClass="headerStyle" BackColor="Tan" Font-Bold="True"/>
                                    <SortedAscendingCellStyle BackColor="#FAFAE7" />
                                    <SortedAscendingHeaderStyle BackColor="#DAC09E" />
                                    <SortedDescendingCellStyle BackColor="#E1DB9C" />
                                    <SortedDescendingHeaderStyle BackColor="#C2A47B" />
                                </asp:GridView>

                            </td>
                        </tr>
                        <tr>
                            <td class="rowseparator"></td>
                        </tr>
                        <tr>
                            <td>
                                <table width="100%" align="center">
                                    <tr>
                                        <td align="center" style="height: 26px">
                                            <asp:Button ID="btnAddNewLubriRow" runat="server" Text="Add Row" OnClick="btnAddNewLubriRow_Click"/>
                                        </td>
                                        <td style="height: 26px">
                                            <asp:Button ID="btnLubriReset" runat="server" Text="Reset" OnClick="btnLubriReset_Click"/>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
                <td></td>
            </tr>
        </table>
    </fieldset>
    <br/>
</asp:Panel>
<asp:Panel ID="pnlLabourBillDetails" runat="server" Visible="false">
    <fieldset style="padding: 10px 10px 0px 10px">
        <legend>LabourCharges</legend>
        <table width="100%">
            <tr>
                <td>
                    <table align="center">
                        <tr>
                            <td>
                                <asp:GridView ID="grdvwLabourBillDetails" runat="server" AutoGenerateColumns="False"
                                              CellPadding="4"
                                              DataKeyNames="RowNumberLabour" CssClass="gridviewStyle" GridLines="None"
                                              OnRowDataBound="grdvwLabourBillDetails_RowDataBound" ForeColor="#333333">
                                    <RowStyle CssClass="rowStyleGrid" Width="100%" BackColor="#F7F6F3" ForeColor="#333333"/>
                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                    <Columns>
                                        <asp:BoundField DataField="RowNumberLabour" HeaderText="S.No" ItemStyle-HorizontalAlign="Center">
                                        <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRowno" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Vendor Name">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlLabourVendorName" runat="server" Width="60px"/>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Bill Number">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtLabourBillNo" runat="server" Width="60px" MaxLength="10" Text='<%# Eval("ColLabBillNo") %>'
                                                             onkeypress="return alphanumeric_only(event);">
                                                </asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Bill Date">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtLabourBillDate" runat="server" Width="60px" Wrap="true" onpaste="return false"
                                                             Text='<%# Eval("ColLabBillDate") %>' oncopy="return false" oncut="return false"
                                                             onkeypress="return false">
                                                </asp:TextBox>
                                                <cc1:CalendarExtender ID="calextndrLabourBillDate" runat="server" Format="dd/MM/yyyy"
                                                                      PopupButtonID="imgBtnQuotationDate" TargetControlID="txtLabourBillDate">
                                                </cc1:CalendarExtender>

                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Aggre">
                                            <ItemTemplate>
                                                <cc1:ComboBox AutoCompleteMode="Append" ID="ddlLabourAggregates" runat="server"
                                                              Width="60px" DropDownStyle="DropDownList">
                                                </cc1:ComboBox>

                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Category">
                                            <ItemTemplate>
                                                <cc1:ComboBox AutoCompleteMode="Append" ID="ddlLabourCategories" runat="server"
                                                              Width="60px" DropDownStyle="DropDownList">
                                                </cc1:ComboBox>

                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Sub Category">
                                            <ItemTemplate>
                                                <cc1:ComboBox AutoCompleteMode="Append" ID="ddlLabourSubCategories" runat="server"
                                                              Width="60px" DropDownStyle="DropDownList">
                                                </cc1:ComboBox>

                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Item Descp.">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtLabourItemDesc" runat="server" Width="60px" MaxLength="10" Text='<%# Eval("ColLabItemDescription") %>'
                                                             onkeypress="return alpha_only(event);">
                                                </asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Quantity">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtLabourQuant" runat="server" Width="60px" MaxLength="10" Text='<%# Eval("ColLabQuantity") %>'
                                                             onkeypress="return isNumberKey(event);">
                                                </asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Bill Amount">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtLabourBillAmount" runat="server" Width="60px" MaxLength="6" Text='<%# Eval("Column3") %>'
                                                             onkeypress="return isDecimalNumberKey(event);">
                                                </asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EditRowStyle BackColor="#999999" />
                                    <FooterStyle CssClass="footerStylegrid" BackColor="#5D7B9D" Font-Bold="True" ForeColor="White"/>
                                    <PagerStyle CssClass="pagerStylegrid" BackColor="#284775" ForeColor="White" HorizontalAlign="Center"/>
                                    <SelectedRowStyle CssClass="selectedRowStyle" BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333"/>
                                    <HeaderStyle CssClass="headerStyle" BackColor="#5D7B9D" Font-Bold="True" ForeColor="White"/>
                                    <SortedAscendingCellStyle BackColor="#E9E7E2" />
                                    <SortedAscendingHeaderStyle BackColor="#506C8C" />
                                    <SortedDescendingCellStyle BackColor="#FFFDF8" />
                                    <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                                </asp:GridView>

                            </td>
                        </tr>
                        <tr>
                            <td class="rowseparator"></td>
                        </tr>
                        <tr>
                            <td>
                                <table width="100%" align="center">
                                    <tr>
                                        <td align="center" style="height: 26px">
                                            <asp:Button ID="btnAddNewLabourRow" runat="server" Text="Add Row" OnClick="btnAddNewLabourRow_Click"/>
                                        </td>
                                        <td style="height: 26px">
                                            <asp:Button ID="btnLabourReset" runat="server" Text="Reset" OnClick="btnLabourReset_Click"/>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
                <td></td>
            </tr>
        </table>
    </fieldset>
    <br/>
</asp:Panel>
<asp:Panel ID="pnlBillDetailsSummaryBtn" runat="server" Visible="false">
    <table width="100%">
        <tr>
            <td class="rowseparator"></td>
        </tr>
        <tr>
            <td align="center">
                <asp:Button ID="btnBillDetailsSummary" runat="server" Text="Bill Details Summary"
                            OnClick="btnBillDetailsSummary_Click"/>
            </td>
        </tr>
        <tr>
            <td class="rowseparator"></td>
        </tr>
    </table>
</asp:Panel>
<asp:Panel ID="pnlBillSummaryDetails" runat="server" Visible="false">
    <fieldset style="padding: 10px 10px 0px 10px">
        <legend>Bill Details Summary</legend>
        <table width="100%">
            <tr>
                <td>
                    <table align="center">
                        <tr>
                            <td>
                                <asp:GridView ID="grdvwBillDetailsSummary" runat="server" AutoGenerateColumns="False"
                                              BackColor="White" BorderColor="White" BorderWidth="2px" CellPadding="3"
                                              CellSpacing="1" DataKeyNames="TypeBillDetails" CssClass="gridviewStyle" GridLines="None"
                                              Width="100%" BorderStyle="Ridge">
                                    <RowStyle CssClass="rowStyleGrid" Width="100%" BackColor="#DEDFDE" ForeColor="Black"/>
                                    <Columns>
                                        <asp:BoundField DataField="TypeBillDetails" HeaderText="Type" HeaderStyle-Width="150px">
                                        <HeaderStyle Width="150px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TotalBillNumbers" HeaderText="Total Bills" HeaderStyle-Width="100px"
                                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <HeaderStyle HorizontalAlign="Center" Width="100px" />
                                        <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TotalBillAmount" HeaderText="Total Bill Amount" HeaderStyle-Width="100px"
                                                        HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <HeaderStyle HorizontalAlign="Center" Width="100px" Wrap="False" />
                                        <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                    </Columns>
                                    <FooterStyle CssClass="footerStylegrid" BackColor="#C6C3C6" ForeColor="Black"/>
                                    <PagerStyle CssClass="pagerStylegrid" BackColor="#C6C3C6" ForeColor="Black" HorizontalAlign="Right"/>
                                    <SelectedRowStyle CssClass="selectedRowStyle" BackColor="#9471DE" Font-Bold="True" ForeColor="White"/>
                                    <HeaderStyle CssClass="headerStyle" BackColor="#4A3C8C" Font-Bold="True" ForeColor="#E7E7FF"/>
                                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                    <SortedAscendingHeaderStyle BackColor="#594B9C" />
                                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                    <SortedDescendingHeaderStyle BackColor="#33276A" />
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td class="rowseparator"></td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </fieldset>
    <br/>
</asp:Panel>

</fieldset>
<table width="100%">
    <tr>
        <td class="rowseparator"></td>
    </tr>
    <tr>
        <td></td>
        <div id="divBillAmount" runat="server">
            <td style="width: 98px">
                Total Bill Amount
            </td>
            <td style="width: 120px">
                <asp:TextBox ID="txtTotalBillAmt" runat="server" Width="120px" onkeypress="return false;"></asp:TextBox>
            </td>
        </div>
    </tr>
</table>
<br/>
<table align="center">
    <tr>
        <td>
            <asp:Button ID="btnSave" runat="server" Text="Update" OnClick="btnSave_Click"/>
        </td>
        <td style="width: 100px"></td>
        <td>
            <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click"/>
        </td>
    </tr>
</table>
<br/>
<br/>
<table align="center">
    <tr align="center">
        <td>

            <asp:GridView ID="gvVehicleMaintenanceDetails" runat="server" EmptyDataText="No Records Found"
                          AllowSorting="True" AutoGenerateColumns="False" CssClass="gridviewStyle" CellSpacing="2"
                          CellPadding="4" ForeColor="Black" Width="630px" AllowPaging="True"
                          OnPageIndexChanging="gvVehicleMaintenanceDetails_PageIndexChanging" OnRowCommand="gvVehicleMaintenanceDetails_RowCommand"
                          EnableSortingAndPagingCallbacks="True" BackColor="#CCCCCC" BorderColor="#999999" BorderStyle="Solid" BorderWidth="3px">
                <RowStyle CssClass="rowStyleGrid" BackColor="White"/>
                <Columns>
                    <asp:TemplateField HeaderText="VehicleNo">
                        <ItemTemplate>
                            <asp:Label ID="lblVehicle_No" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "OffRoadVehicle_No") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="District">
                        <ItemTemplate>
                            <asp:Label ID="lblDistrict" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "District") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Maintenanace Type">
                        <ItemTemplate>
                            <asp:Label ID="lblMaintenanaceType" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "MaintenanaceType") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="MaintenanceDate" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblMaintenanceDate" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "MaintenanceDate") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="DownOdo" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblDownOdo" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "DownTimeOdoReading") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="DownTime">
                        <ItemTemplate>
                            <asp:Label ID="lblDowntime" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Downtime") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="UpOdo" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblUpOdo" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "UptimeOdoReading") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="UpTime">
                        <ItemTemplate>
                            <asp:Label ID="lblUptime" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Uptime") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="SpareBillNo" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblSpareBillNo" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "SpareBillNo") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="SpareBillDate" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblSpareBillDate" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "SpareBillDate") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="SpareBillAmount" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblSpareBillAmount" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "SpareBillAmount") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="LubricantBillNo" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblLubricantBillNo" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "LubricantBillNo") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="LubricantBillDate" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblLubricantBillDate" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "LubricantBillDate") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="LubricantBillAmount" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblLubricantBillAmount" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "LubricantBillAmount") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="LabourBillNo" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblLabourBillNo" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "LabourBillNo") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="LabourBillDate" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblLabourBillDate" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "LabourBillDate") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="LabourBillAmount" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblLabourBillAmount" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "LabourBillAmount") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Edit">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkEdit" runat="server" CommandName="VehMainEdit" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "OffRoad_Id") %>'
                                            Text="Edit">
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Delete">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkDelete" runat="server" CommandName="VehMainDelete" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "OffRoad_Id") %>'
                                            Text="Delete">
                            </asp:LinkButton>
                            <cc1:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" TargetControlID="lnkDelete"
                                                       ConfirmText="Are you sure you want to Delete?">
                            </cc1:ConfirmButtonExtender>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <FooterStyle CssClass="footerStylegrid" BackColor="#CCCCCC"/>
                <PagerStyle CssClass="pagerStylegrid" BackColor="#CCCCCC" ForeColor="Black" HorizontalAlign="Left"/>
                <SelectedRowStyle CssClass="selectedRowStyle" BackColor="#000099" Font-Bold="True" ForeColor="White"/>
                <HeaderStyle CssClass="headerStyle" BackColor="Black" Font-Bold="True" ForeColor="White"/>
                <SortedAscendingCellStyle BackColor="#F1F1F1" />
                <SortedAscendingHeaderStyle BackColor="#808080" />
                <SortedDescendingCellStyle BackColor="#CAC9C9" />
                <SortedDescendingHeaderStyle BackColor="#383838" />
            </asp:GridView>

        </td>
    </tr>
</table>
</fieldset>
</td>
</tr>
</table>
</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>