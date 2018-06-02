<%@ Page Language="C#" MasterPageFile="~/temp.master" AutoEventWireup="true" CodeFile="FabricatedVehicleDetails.aspx.cs" Inherits="FabricatedVehicleDetails" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<script type="text/javascript">
    function validation() {
        var fabricatorName = document.getElementById('<%= ddlFabricatorName.ClientID %>');
        var invoiceNo = document.getElementById('<%= txtInvoiceNo.ClientID %>');
        var invoiceDate = document.getElementById('<%= txtInvoiceDate.ClientID %>');
        var fabricationCost = document.getElementById('<%= txtFabricationCost.ClientID %>');
        var vehicleHandoverDate = document.getElementById('<%= txtVehicleHandoverDate.ClientID %>');
        var fabricationCompDate = document.getElementById('<%= txtFabricationCompDate.ClientID %>');
        var inspecetedBy = document.getElementById('<%= txtInspecetedBy.ClientID %>');
        var inspectionDate = document.getElementById('<%= txtInspectionDate.ClientID %>');
        var vehiclePurchaseDate = document.getElementById('<%= vehiclePurchaseDate.ClientID %>');
        var now = new Date();
        var trNo = document.getElementById('<%= ddlTRNo.ClientID %>');
        if (trNo.selectedIndex === 0) {
            alert("Please select a Vehicle");
            trNo.focus();
            return false;
        }
        switch (fabricatorName.selectedIndex) {
        case 0:
            alert("Please select Fabricator Name");
            fabricatorName.focus();
            return false;
        }
        if (!RequiredValidation(invoiceNo, "Invoice Number Cannot be Blank"))
            return false;

        if (!RequiredValidation(invoiceDate, "Invoice Date Cannot be Blank"))
            return false;

        if (!isValidDate(invoiceDate.value)) {
            alert("Enter Valid Invoice Date");
            invoiceDate.focus();
            return false;
        }

        if (Date.parse(invoiceDate.value) > Date.parse(now)) {
            alert("Invoice Date should not be greater than Current Date");
            invoiceDate.focus();
            return false;
        }

        if (Date.parse(invoiceDate.value) < Date.parse(vehiclePurchaseDate.value)) {
            alert("Invoice Date should be greater than Purchase Date.(PurchaseDate-" + vehiclePurchaseDate.value + ")");
            invoiceDate.focus();
            return false;
        }


        if (!RequiredValidation(fabricationCost, "Fabrication Cost Cannot be Blank"))
            return false;

        if (!RequiredValidation(vehicleHandoverDate, "Vehicle Handover To Fabricator Date Cannot be Blank"))
            return false;

        if (!isValidDate(vehicleHandoverDate.value)) {
            alert("Enter Valid Vehicle Handover Date");
            vehicleHandoverDate.focus();
            return false;
        }

        if (Date.parse(vehicleHandoverDate.value) < Date.parse(vehiclePurchaseDate.value)) {
            alert("Vehicle Handover Date should be greater than Purchase Date.(PurchaseDate-" +
                vehiclePurchaseDate.value +
                ")");
            invoiceDate.focus();
            return false;
        }

        if (Date.parse(vehicleHandoverDate.value) > Date.parse(now)) {
            alert("Vehicle Handover Date should not be greater than Current Date");
            vehicleHandoverDate.focus();
            return false;
        }

        if (!RequiredValidation(fabricationCompDate, "Fabrication Completion Date Cannot be Blank"))
            return false;

        if (!isValidDate(fabricationCompDate.value)) {
            alert("Enter Valid Fabrication Completion Date");
            fabricationCompDate.focus();
            return false;
        }

        if (Date.parse(fabricationCompDate.value) > Date.parse(now)) {
            alert("Fabrication Completion Date should not be greater than Current Date");
            fabricationCompDate.focus();
            return false;
        }

        if (Date.parse(fabricationCompDate.value) <= Date.parse(vehicleHandoverDate.value)) {
            alert("Fabrication Completion Date should be greater than Vehicle Handover Date");
            fabricationCompDate.focus();
            return false;
        }

        if (!RequiredValidation(inspecetedBy, "Inspeceted By Cannot be Blank"))
            return false;

        if (!RequiredValidation(inspectionDate, "Inspection Date Cannot be Blank"))
            return false;

        if (!isValidDate(inspectionDate.value)) {
            alert("Enter Valid Inspection Date");
            inspectionDate.focus();
            return false;
        }
        if (Date.parse(inspectionDate.value) > Date.parse(now)) {
            alert("Inspection Date should not be greater than Current Date");
            inspectionDate.focus();
            return false;
        }

        if (Date.parse(inspectionDate.value) < Date.parse(fabricationCompDate.value)) {
            alert("Inspection Date should be greater than Fabrication Completion Date");
            inspectionDate.focus();
            return false;
        }
        return true;
    }

</script>


<asp:UpdatePanel runat="server">
<ContentTemplate>
<script type="text/javascript">
    function pageLoad() {
        $('#<%= ddlTRNo.ClientID %>').select2({
            disable_search_threshold: 5,
            search_contains: true,
            minimumResultsForSearch: 2,
            placeholder: "Select an option"
        });
        $('#<%= ddlFabricatorName.ClientID %>').select2({
            disable_search_threshold: 5,
            search_contains: true,
            minimumResultsForSearch: 2,
            placeholder: "Select an option"
        });
    }
</script>
<br />
<legend align="center" style="color: brown">
    <caption>
        Fabricated Vehicle Details</caption>
</legend>
<table align="center">
<tr>
    
    <td class="rowseparator"></td>
</tr>
<tr>
    <td>
        <asp:Panel ID="pnlFabricatedVehicleDetails" runat="server">
            <table class="table table-striped table-bordered table-hover">
                <tr>
                    <td align="center" style="font-size: small; font-weight: bold" colspan="4"></td>
                </tr>

                <tr>
                    <td style="width: 314px"></td>
                    <td align="left" style="width: 458px">
                        T/R No.<span style="color: Red">*</span>
                    </td>
                    <td align="left" style="width: 400px">
                        <asp:DropDownList ID="ddlTRNo" CssClass="text1" runat="server" Width="150px" OnSelectedIndexChanged="ddlTRNo_SelectedIndexChanged" AutoPostBack="True">
                            <asp:ListItem Value="-1">--Select--</asp:ListItem>
                            <asp:ListItem Value="0">Dummy</asp:ListItem>
                        </asp:DropDownList>

                        <asp:TextBox ID="txtTrNo" runat="server" Visible="False" CssClass="text1" Width="145px" ReadOnly="True"></asp:TextBox>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td style="width: 314px"></td>
                    <td align="left" style="width: 458px">
                        Fabricator Name<span style="color: Red">*</span>
                    </td>
                    <td align="left" style="width: 400px">
                        <asp:DropDownList ID="ddlFabricatorName" CssClass="text1" runat="server" Width="150px">
                            <asp:ListItem Value="-1">--Select--</asp:ListItem>
                            <asp:ListItem Value="0">Dummy</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td style="width: 314px"></td>
                    <td align="left" style="width: 458px">
                        Invoice No<span style="color: Red">*</span>
                    </td>
                    <td align="left" style="width: 400px">
                        <asp:TextBox ID="txtInvoiceNo" runat="server" CssClass="search_3" Width="145px" MaxLength="15" onkeypress="return alphanumeric_only(event);"></asp:TextBox>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td style="width: 314px"></td>
                    <td align="left" style="width: 458px">
                        Invoice Date<span style="color: Red">*</span>
                    </td>
                    <td align="left" style="width: 400px">
                        <asp:TextBox ID="txtInvoiceDate" runat="server" CssClass="search_3" Width="145px" onkeypress="return false"
                                     oncut="return false;" onpaste="return false;">
                        </asp:TextBox>
                        <asp:ImageButton ID="imgBtnCalendarInvoiceDate"
                                         runat="server" Style="vertical-align: top" alt="" src="images/Calendar.gif"/>
                        <cc1:CalendarExtender runat="server" TargetControlID="txtInvoiceDate"
                                              PopupButtonID="imgBtnCalendarInvoiceDate" CssClass="cal_Theme1" Format="MM/dd/yyyy">
                        </cc1:CalendarExtender>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td style="width: 314px"></td>
                    <td align="left" style="width: 458px">
                        Fabrication Cost<span style="color: Red">*</span>
                    </td>
                    <td align="left" style="width: 400px">
                        <asp:TextBox ID="txtFabricationCost" runat="server" CssClass="search_3" Width="145px" onkeypress="return numericOnly(this);"
                                     MaxLength="9">
                        </asp:TextBox>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td style="width: 314px"></td>
                    <td align="left" style="width: 458px">
                        Vehicle Handover to Fabricator Date<span style="color: Red">*</span>
                    </td>
                    <td align="left" style="width: 400px">
                        <asp:TextBox ID="txtVehicleHandoverDate" runat="server" CssClass="search_3" Width="145px" onkeypress="return false"
                                     oncut="return false;" onpaste="return false;">
                        </asp:TextBox>
                        <asp:ImageButton ID="imgbtHandover"
                                         runat="server" Style="vertical-align: top" alt="" src="images/Calendar.gif"/>
                        <cc1:CalendarExtender runat="server" CssClass="cal_Theme1" TargetControlID="txtVehicleHandoverDate"
                                              PopupButtonID="imgbtHandover" Format="MM/dd/yyyy">
                        </cc1:CalendarExtender>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td style="width: 314px"></td>
                    <td align="left" style="width: 458px">
                        Fabrication Completion Date<span style="color: Red">*</span>
                    </td>
                    <td align="left" style="width: 400px">
                        <asp:TextBox ID="txtFabricationCompDate" runat="server" CssClass="search_3" Width="145px" onkeypress="return false"
                                     oncut="return false;" onpaste="return false;">
                        </asp:TextBox>
                        <asp:ImageButton ID="imgbtFabricationDate"
                                         runat="server" Style="vertical-align: top" alt="" src="images/Calendar.gif"/>
                        <cc1:CalendarExtender runat="server" CssClass="cal_Theme1" TargetControlID="txtFabricationCompDate"
                                              PopupButtonID="imgbtFabricationDate" Format="MM/dd/yyyy">
                        </cc1:CalendarExtender>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td style="width: 314px"></td>
                    <td align="left" style="width: 458px">
                        Inspected By<span style="color: Red">*</span>
                    </td>
                    <td align="left" style="width: 400px">
                        <asp:TextBox ID="txtInspecetedBy" runat="server" Width="145px" CssClass="search_3" MaxLength="15" onkeypress="return alpha_only_withspace(event);"></asp:TextBox>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td style="width: 314px"></td>
                    <td align="left" style="width: 458px">
                        Inspection Date<span style="color: Red">*</span>
                    </td>
                    <td align="left" style="width: 400px">
                        <asp:TextBox ID="txtInspectionDate" runat="server" CssClass="search_3" Width="145px" onkeypress="return false"
                                     oncut="return false;" onpaste="return false;">
                        </asp:TextBox>
                        <asp:ImageButton ID="imgbtInspectionDate"
                                         runat="server" Style="vertical-align: top" alt="" src="images/Calendar.gif"/>
                        <cc1:CalendarExtender ID="calExtInspectionDate" runat="server" CssClass="cal_Theme1" TargetControlID="txtInspectionDate"
                                              PopupButtonID="imgbtInspectionDate" Format="MM/dd/yyyy">
                        </cc1:CalendarExtender>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td style="width: 314px"></td>
                    <td align="left" style="width: 458px"></td>
                    <td align="left" style="width: 400px"></td>
                    <td></td>
                </tr>
                <tr>
                    <td style="width: 314px">
                        &nbsp;
                    </td>
                    <td align="left" style="width: 458px"></td>
                    <td align="left" style="width: 400px"></td>
                    <td></td>
                </tr>
                <tr>
                    <td style="width: 314px"></td>
                    <td align="center" style="width: 458px">
                        <asp:Button ID="btSave" Text="Save" runat="server" CssClass="form-submit-button" style="margin-top: 10px" OnClick="btSave_Click"/>
                    </td>
                    <td align="left" style="width: 400px">
                        <asp:Button ID="btReset" Text="Reset" runat="server" CssClass="form-submit-button" style="margin-top: 10px" OnClick="btReset_Click"/>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td style="width: 314px">
                        &nbsp;
                    </td>
                    <td align="center" style="width: 458px">
                        &nbsp;
                    </td>
                    <td align="left" style="width: 400px">
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>

            </table>
        </asp:Panel>
    </td>
</tr>
<tr>
    <td class="rowseparator"></td>
</tr>
<tr>
    <td>
        <table align="center" style="margin-top:-100px">
            <tr align="center">
                <td>
                    <asp:GridView ID="gvFabricatedVehicleDetails" runat="server" EmptyDataText="No Records Found"
                                  CellPadding="3" Width="630px" AllowPaging="True"
                                  AllowSorting="True" OnSorting="gridView_Sorting" OnPageIndexChanging="gridView_PageIndexChanging"
                                  AutoGenerateColumns="False" OnRowCommand="gvFabricatedVehicleDetails_RowCommand"
                                  class="table table-striped table-bordered table-hover" PagerStyle-CssClass="pager"
                                  HeaderStyle-ForeColor="#337ab7" style="margin-top: 100px; text-align: center;" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px">
                        <RowStyle CssClass="rows" ForeColor="#000066"/>
                        <Columns>
                            <asp:TemplateField HeaderText="T/R No">
                                <ItemTemplate>
                                    <asp:Label ID="lblTRNo" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "TRNo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Fabricator Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblFabricatorName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "FleetFabricator_Name") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Hand Over Date">
                                <ItemTemplate>
                                    <asp:Label ID="lblVehicleHandoverDate" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "VehicleHandoverToFabricatorDate") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Fabrication Complition Date">
                                <ItemTemplate>
                                    <asp:Label ID="lblFabricationCompDate" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "FabricationCompletionDate") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Fabrication Cost">
                                <ItemTemplate>
                                    <asp:Label ID="lblFabricationCost" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "FabricationCost") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Edit">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkEdit" runat="server" CommandName="fabVehEdit" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "FabricatedVehicleDetID") %>'
                                                    Text="Edit">
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Delete">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkDelete" runat="server" CommandName="fabVehDelete" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "FabricatedVehicleDetID") %>'
                                                    Text="Delete">
                                    </asp:LinkButton>
                                    <cc1:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" TargetControlID="lnkDelete"
                                                               ConfirmText="Are you sure you want to Delete?">
                                    </cc1:ConfirmButtonExtender>
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
            <asp:HiddenField ID="vehiclePurchaseDate" runat="server"/>
        </table>
    </td>
</tr>
</table>

</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>