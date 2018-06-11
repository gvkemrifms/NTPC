<%@ Page Language="C#" MasterPageFile="~/temp.master" AutoEventWireup="true" CodeFile="PollutionUnderControl.aspx.cs" Inherits="PollutionUnderControl" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<link href="css/PollutionUnderControl.css" rel="stylesheet"/>

<script type="text/javascript">
    $(function() {
        $('#<%= txtPollutionValidityStartDate.ClientID %>').datepicker({
            dateFormat: 'mm/dd/yy',
            changeMonth: true,
            changeYear: true
        });
    });

    function validation() {
        var pollutionValidityStartDate = document.getElementById('<%= txtPollutionValidityStartDate.ClientID %>');
        var pollutionValidityPeriod = document.getElementById('<%= ddlPollutionValidityPeriod.ClientID %>');
        var pollutionReceiptNo = document.getElementById('<%= txtPollutionReceiptNo.ClientID %>');
        var pollutionFee = document.getElementById('<%= txtPollutionFee.ClientID %>');
        var vehiclePurchaseDate = document.getElementById('<%= vehiclePurchaseDate.ClientID %>');
        var now = new Date();
        var id = document.getElementById('<%= ddlVehicleNumber.ClientID %>').control._textBoxControl.value;;
        if (id === '') {
            alert('Please select Vehicle');
            return false;
        }
        if (!RequiredValidation(pollutionValidityStartDate, "Pollution Validity Start Date Cannot be Blank"))
            return false;
        if (!isValidDate(pollutionValidityStartDate.value)) {
            alert("Enter Valid Date");
            pollutionValidityStartDate.focus();
            return false;
        }
        if (Date.parse(pollutionValidityStartDate.value) > Date.parse(now)) {
            alert("Pollution Validity Start Date should not be greater than Current Date");
            pollutionValidityStartDate.focus();
            return false;
        }
        if (Date.parse(pollutionValidityStartDate.value) < Date.parse(vehiclePurchaseDate.value)) {
            alert("Pollution Validity Start Date should be greater than Purchase Date.(PurchaseDate-" +
                vehiclePurchaseDate.value +
                ")");
            pollutionValidityStartDate.focus();
            return false;
        }
        switch (pollutionValidityPeriod.selectedIndex) {
        case 0:
            alert("Please select Pollution Validity Period");
            pollutionValidityPeriod.focus();
            return false;
        }
        if (!RequiredValidation(pollutionReceiptNo, "Pollution Receipt No Cannot be Blank"))
            return false;
        if (!RequiredValidation(pollutionFee, "Pollution Fee Cannot be Blank"))
            return false;
        return true;
    }
</script>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>

<table>
<tr>
    <td class="rowseparator"></td>
</tr>
<tr>
    <td>
        <asp:Panel ID="pnlPUC" runat="server">
            <table style="width: 100%" class="table table-striped table-bordered table-hover">
                <tr>
                    <td align="center" style="font-size: small; font-weight: bold" colspan="5"></td>
                </tr>
                <tr>
                    <td align="center" colspan="5"></td>
                </tr>
                <tr>
                    <td style="width: 200px"></td>
                    <td align="left" style="width: 226px">
                        Vehicle Number<span style="color: Red">*</span>
                    </td>
                    <td align="left" colspan="2">
                        <cc1:ComboBox AutoCompleteMode="Append" ID="ddlVehicleNumber" AutoPostBack="true" runat="server"
                                      Width="150px" OnSelectedIndexChanged="ddlVehicleNumber_SelectedIndexChanged" DropDownStyle="DropDownList">
                            <asp:ListItem></asp:ListItem>
                        </cc1:ComboBox>
                        <asp:TextBox ID="txtVehicleNumber" class="text1" CssClass="search_3" runat="server" MaxLength="15" Visible="False"
                                     Width="145px" ReadOnly="True">
                        </asp:TextBox>
                    </td>
                    <td align="center" style="width: 185px"></td>
                </tr>
                <tr>
                    <td align="center"></td>
                    <td align="left" style="width: 286px">
                        Pollution Validity Start Date<span style="color: Red">*</span>
                    </td>
                    <td align="left" colspan="2">
                        <asp:TextBox ID="txtPollutionValidityStartDate" CssClass="search_3" class="text1" AutoPostBack="true" runat="server"
                                     Width="145px" OnTextChanged="txtPollutionValidityStartDate_TextChanged1" onkeypress="return false"
                                     oncut="return false;" onpaste="return false;">
                        </asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="center"></td>
                    <td align="left" style="width: 226px">
                        Pollution Validity Period<span style="color: Red">*</span>
                    </td>
                    <td align="left" colspan="2">
                        <asp:DropDownList ID="ddlPollutionValidityPeriod" CssClass="search_3" class="text1" runat="server" Width="150px" OnSelectedIndexChanged="ddlPollutionValidityPeriod_SelectedIndexChanged"
                                          AutoPostBack="True">
                            <asp:ListItem Value="-1">--Select--</asp:ListItem>
                            <asp:ListItem Value="3">3 Month</asp:ListItem>
                            <asp:ListItem Value="6">6 Month</asp:ListItem>
                            <asp:ListItem Value="9">9 Month</asp:ListItem>
                            <asp:ListItem Value="12">1 Year</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td align="center"></td>
                </tr>
                <tr>
                    <td align="center"></td>
                    <td align="left" style="width: 226px">
                        Pollution Validity End Date<span style="color: Red">*</span>
                    </td>
                    <td align="left" colspan="2">
                        <asp:TextBox ID="txtPollutionValidityEndDate" CssClass="search_3" class="text1" runat="server" Width="145px" BackColor="DarkGray"
                                     ReadOnly="True">
                        </asp:TextBox>
                    </td>
                    <td align="center"></td>
                </tr>
                <tr>
                    <td align="center"></td>
                    <td align="left" style="width: 226px">
                        Pollution Receipt No.<span style="color: Red">*</span>
                    </td>
                    <td align="left" colspan="2">
                        <asp:TextBox ID="txtPollutionReceiptNo" CssClass="search_3" class="text1" runat="server" Width="145px" MaxLength="15"
                                     onkeypress="return alphanumeric_only(event);">
                        </asp:TextBox>
                    </td>
                    <td align="center"></td>
                </tr>
                <tr>
                    <td align="center"></td>
                    <td align="left" style="width: 226px">
                        Pollution Fee<span style="color: Red">*</span>
                    </td>
                    <td align="left" colspan="2">
                        <asp:TextBox ID="txtPollutionFee" CssClass="search_3" class="text1" runat="server" Width="145px" onkeypress="return numericOnly(event);"
                                     MaxLength="9">
                        </asp:TextBox>
                    </td>
                    <td align="center"></td>
                </tr>
                <tr>
                    <td align="center">
                        &nbsp;
                    </td>
                    <td align="center" style="width: 226px">
                        &nbsp;
                    </td>
                    <td align="left">
                        &nbsp;
                    </td>
                    <td align="center" style="width: 50px">
                        &nbsp;
                    </td>
                    <td align="center">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        &nbsp;
                    </td>
                    <td align="center" style="width: 226px">
                        <asp:Button ID="btSave" runat="server" CssClass="form-submit-button" Text="Save" OnClick="btSave_Click"/>
                    </td>
                    <td align="left">
                        <asp:Button ID="btReset" CssClass="form-submit-button" runat="server" Text="Reset" OnClick="btReset_Click"/>
                    </td>
                    <td align="center" style="width: 50px">
                        &nbsp;
                    </td>
                    <td align="center">
                        <asp:LinkButton runat="server" Visible="False">View History</asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td align="center"></td>
                    <td align="center" style="width: 226px"></td>
                    <td align="left" colspan="2"></td>
                    <td align="center"></td>
                </tr>
                <tr>
                    <td align="center">
                        &nbsp;
                    </td>
                    <td align="center" style="width: 226px">
                        &nbsp;
                    </td>
                    <td align="left" colspan="2">
                        &nbsp;
                    </td>
                    <td align="center">
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
<tr align="center">
    <td>
        <asp:GridView ID="gvPollutionUnderControl" runat="server" AutoGenerateColumns="False"
                      CellPadding="3" Width="630px" OnRowCommand="gvPollutionUnderControl_RowCommand"
                      OnRowDataBound="gvPollutionUnderControl_RowDataBound" EmptyDataText="No Records Found"
                      AllowPaging="True" OnPageIndexChanging="gvPollutionUnderControl_PageIndexChanging"
                      CssClass="mydatagrid" PagerStyle-CssClass="pager"
                      HeaderStyle-CssClass="header" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px">
            <RowStyle CssClass="rows" ForeColor="#000066"/>
            <Columns>
                <asp:TemplateField HeaderText="Vehicle Number">
                    <ItemTemplate>
                        <asp:Label ID="lblVehicleNumber" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"VehicleNumber") %>'>
                        </asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="PUC Validity Start Date">
                    <ItemTemplate>
                        <asp:Label ID="lblPUCValidityStartDate" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"PUCValidityStartDate") %>'>
                        </asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="PUC Validity Period">
                    <ItemTemplate>
                        <asp:Label ID="lblPUCValidityPeriod" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem,"PUCValidityPeriod") %>'>
                        </asp:Label>
                        <asp:Label ID="lblPUCValidityPeriodText" runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="PUC Validity End Date">
                    <ItemTemplate>
                        <asp:Label ID="lblPUCValidityEndDate" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"PUCValidityEndDate") %>'>
                        </asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="PUC Receipt No">
                    <ItemTemplate>
                        <asp:Label ID="lblPUCReceiptNo" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"PUCReceiptNo") %>'>
                        </asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="PUC Fee">
                    <ItemTemplate>
                        <asp:Label ID="lblPUCFee" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"PUCFee") %>'>
                        </asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Edit">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkEdit" runat="server" CommandName="roadTaxEdit" CommandArgument='<%#DataBinder.Eval(Container.DataItem,"PollutionUnderControlID") %>'
                                        Text="Edit">
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <FooterStyle CssClass="footerStylegrid" BackColor="White" ForeColor="#000066"/>
            <PagerStyle CssClass="pagerStylegrid" BackColor="White" ForeColor="#000066" HorizontalAlign="Left"/>
            <SelectedRowStyle CssClass="selectedRowStyle" BackColor="#669999" Font-Bold="True" ForeColor="White"/>
            <HeaderStyle CssClass="headerStyle" BackColor="#006699" Font-Bold="True" ForeColor="White"/>
            <SortedAscendingCellStyle BackColor="#F1F1F1"/>
            <SortedAscendingHeaderStyle BackColor="#007DBB"/>
            <SortedDescendingCellStyle BackColor="#CAC9C9"/>
            <SortedDescendingHeaderStyle BackColor="#00547E"/>
        </asp:GridView>
    </td>
</tr>
<tr>
    <td class="rowseparator"></td>
</tr>
<asp:HiddenField ID="vehiclePurchaseDate" runat="server"/>
</table>
</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>