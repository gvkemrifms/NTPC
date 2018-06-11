<%@ Page Language="C#" MasterPageFile="~/temp.master" AutoEventWireup="true" CodeFile="FitnessRenewal.aspx.cs" Inherits="FitnessRenewal" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<link href="css/FitnessRenewal.css" rel="stylesheet"/>
<script type="text/javascript">
    function validation() {
        var fitnessValidityStartDate = document.getElementById('<%= txtFitnessValidityStartDate.ClientID %>');
        var fitnessValidityPeriod = document.getElementById('<%= ddlFitnessValidityPeriod.ClientID %>');
        var vehicleRtaCircle = document.getElementById('<%= txtVehicleRTACircle.ClientID %>');
        var fitnessReceiptNo = document.getElementById('<%= txtFitnessReceiptNo.ClientID %>');
        var fitnessFee = document.getElementById('<%= txtFitnessFee.ClientID %>');
        var vehiclePurchaseDate = document.getElementById('<%= vehiclePurchaseDate.ClientID %>');
        var now = new Date();
        var id = document.getElementById('<%= ddlVehicleNumber.ClientID %>');
        var inputs = id.getElementsByTagName('input');
        var i;
        for (i = 0; i < inputs.length; i++) {
            switch (inputs[i].type) {
            case 'text':
                if (inputs[i].value !== "" && inputs[i].value != null && inputs[i].value === "--Select--") {
                    alert('Select the Vehicle');
                    return false;
                }
                break;
            }
        }
        if (!RequiredValidation(fitnessValidityStartDate, "Fitness Validity Start Date Cannot be Blank"))
            return false;
        if (!isValidDate(fitnessValidityStartDate.value)) {
            alert("Enter Valid Date");
            fitnessValidityStartDate.focus();
            return false;
        }
        if (Date.parse(fitnessValidityStartDate.value) > Date.parse(now)) {
            alert("Fitness Validity Start Date should not be greater than Current Date");
            fitnessValidityStartDate.focus();
            return false;
        }
        if (Date.parse(fitnessValidityStartDate.value) < Date.parse(vehiclePurchaseDate.value)) {
            alert("Fitness Validity Start Date should be greater than Purchase Date.(PurchaseDate-" +
                vehiclePurchaseDate.value +
                ")");
            fitnessValidityStartDate.focus();
            return false;
        }
        switch (fitnessValidityPeriod.selectedIndex) {
        case 0:
            alert("Please select Fitness Validity Period");
            fitnessValidityPeriod.focus();
            return false;
        }
        if (!RequiredValidation(vehicleRtaCircle, "Vehicle RTA Circle Cannot be Blank"))
            return false;
        if (!RequiredValidation(fitnessReceiptNo, "Fitness Receipt No Cannot be Blank"))
            return false;
        if (!RequiredValidation(fitnessFee, "Fitness Fee Cannot be Blank"))
            return false;
        return true;
    }
</script>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
<script type="text/javascript">
    function pageLoad() {
        $('#<%= txtFitnessValidityStartDate.ClientID %>').datepicker({
            dateFormat: 'mm/dd/yy',
            changeMonth: true,
            changeYear: true
        });
    }
</script>
<legend align="center" style="color: brown">Fitness Renewal</legend>
<br/>
<table>
<tr>
    <td class="rowseparator"></td>
</tr>
<tr>
    <td>
        <asp:Panel ID="pnlFitnessRenewal" runat="server">
            <table style="width: 100%">
                <tr>
                    <td align="center" colspan="5"></td>
                </tr>
                <tr>
                    <td style="width: 200px"></td>
                    <td align="left" style="width: 226px">
                        Vehicle Number<span style="color: Red">*</span>
                    </td>
                    <td align="left" colspan="2">
                        <cc1:ComboBox AutoCompleteMode="Append" ID="ddlVehicleNumber" runat="server" Width="140px" Height="30px"
                                      OnSelectedIndexChanged="ddlVehicleNumber_SelectedIndexChanged"
                                      AutoPostBack="True" DropDownStyle="DropDownList" CssClass="CustomComboBoxStyle">
                            <asp:ListItem Value="-1">--SELECT--</asp:ListItem>
                            <asp:ListItem Value="0">Dummy</asp:ListItem>
                        </cc1:ComboBox>
                        <asp:TextBox ID="txtVehicleNumber" class="text1" runat="server" MaxLength="6" CssClass="search_3" onkeypress="return numericOnly(this);"
                                     ReadOnly="True" Visible="False" Width="145px">
                        </asp:TextBox>
                    </td>
                    <td align="center" style="width: 185px"></td>
                </tr>
                <tr>
                    <td align="center"></td>
                    <td align="left" style="width: 286px">
                        Fitness Validity Start Date<span style="color: Red">*</span>
                    </td>
                    <td align="left" colspan="2">
                        <asp:TextBox ID="txtFitnessValidityStartDate" class="text1" AutoPostBack="true" runat="server"
                                     Width="145px" OnTextChanged="txtFitnessValidityStartDate_TextChanged1" CssClass="search_3" onkeypress="return false;"
                                     oncut="return false;" onpaste="return false;">
                        </asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="center"></td>
                    <td align="left" style="width: 226px">
                        Fitness Validity Period<span style="color: Red">*</span>
                    </td>
                    <td align="left" colspan="2">
                        <asp:DropDownList ID="ddlFitnessValidityPeriod" class="text1" runat="server" CssClass="search_3" Width="150px" OnSelectedIndexChanged="ddlFitnessValidityPeriod_SelectedIndexChanged"
                                          AutoPostBack="True">
                            <asp:ListItem Value="-1">--Select--</asp:ListItem>
                            <asp:ListItem Value="3">3 Month</asp:ListItem>
                            <asp:ListItem Value="6">6 Month</asp:ListItem>
                            <asp:ListItem Value="9">9 Month</asp:ListItem>
                            <asp:ListItem Value="12">1 Year</asp:ListItem>
                            <asp:ListItem Value="24">2 Year</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td align="center"></td>
                </tr>
                <tr>
                    <td align="center"></td>
                    <td align="left" style="width: 226px" nowrap="nowrap">
                        Fitness Validity End Date<span style="color: Red">*</span>
                    </td>
                    <td align="left" colspan="2">
                        <asp:TextBox ID="txtFitnessValidityEndDate" class="text1" runat="server" CssClass="search_3" Width="150px" BackColor="DarkGray"
                                     ReadOnly="True">
                        </asp:TextBox>
                    </td>
                    <td align="center"></td>
                </tr>
                <tr>
                    <td align="center">
                        &nbsp;
                    </td>
                    <td align="left" style="width: 226px">
                        Vehicle RTA Circle<span style="color: Red">*</span>
                    </td>
                    <td align="left" colspan="2">
                        <asp:TextBox ID="txtVehicleRTACircle" class="text1" runat="server" CssClass="search_3" Width="150px" MaxLength="35"
                                     onkeypress="return alphanumeric_only(event);">
                        </asp:TextBox>
                    </td>
                    <td align="center">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td align="center"></td>
                    <td align="left" style="width: 226px">
                        Fitness Receipt No.<span style="color: Red">*</span>
                    </td>
                    <td align="left" colspan="2">
                        <asp:TextBox ID="txtFitnessReceiptNo" class="text1" runat="server" CssClass="search_3" Width="145px" MaxLength="15"
                                     onkeypress="return alphanumeric_only(event);">
                        </asp:TextBox>
                    </td>
                    <td align="center"></td>
                </tr>
                <tr>
                    <td align="center"></td>
                    <td align="left" style="width: 226px">
                        Fitness Fee<span style="color: Red">*</span>
                    </td>
                    <td align="left" colspan="2">
                        <asp:TextBox ID="txtFitnessFee" runat="server" class="text1" CssClass="search_3" onkeypress="return numericOnly(this);"
                                     Width="145px" MaxLength="9">
                        </asp:TextBox>
                    </td>
                    <td align="center"></td>
                </tr>
                <tr>
                    <td align="center">
                        &nbsp;
                    </td>
                    <td align="left" style="width: 226px">
                        &nbsp;
                    </td>
                    <td align="left" colspan="2">
                        &nbsp;
                    </td>
                    <td align="center">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td align="center"></td>
                    <td align="center" style="width: 226px">
                        <asp:Button ID="btSave" runat="server" CssClass="form-submit-button" Text="Save" OnClick="btSave_Click"/>
                    </td>
                    <td align="left">
                        <asp:Button ID="btReset" runat="server" CssClass="form-submit-button" Text="Reset" OnClick="btReset_Click"/>
                    </td>
                    <td align="center" style="width: 50px">
                        &nbsp;
                    </td>
                    <td align="center">
                        <asp:LinkButton ID="lbtnViewHistory" runat="server" Visible="False">View History</asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        &nbsp;
                    </td>
                    <td align="center" style="width: 226px"></td>
                    <td align="left" colspan="2"></td>
                    <td align="center"></td>
                </tr>
            </table>
        </asp:Panel>
    </td>
</tr>
<tr>
    <td class="rowseparator"></td>
</tr>
<tr align="center">
    <td >
        <asp:GridView ID="gvFitnessRenewal" runat="server" AutoGenerateColumns="False" CellPadding="3" BorderWidth="1px" BorderColor="#CCCCCC" OnRowCommand="gvFitnessRenewal_RowCommand"
                      Width="630px" OnRowDataBound="gvFitnessRenewal_RowDataBound" AllowPaging="True"
                      EmptyDataText="No Records Found" OnPageIndexChanging="gvFitnessRenewal_PageIndexChanging"
                      CssClass="mydatagrid" PagerStyle-CssClass="pager"
                      HeaderStyle-CssClass="header" BackColor="White" BorderStyle="None">
            <RowStyle CssClass="rows" ForeColor="#000066"/>
            <Columns>
                <asp:TemplateField HeaderText="Vehicle Number">
                    <ItemTemplate>
                        <asp:Label ID="lblVehicleNumber" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"VehicleNumber") %>'>
                        </asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="FRValidity StartDate">
                    <ItemTemplate>
                        <asp:Label ID="lblFRValidityStartDate" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"FRValidityStartDate") %>'>
                        </asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="FRValidity Period">
                    <ItemTemplate>
                        <asp:Label ID="lblFRValidityPeriod" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"FRValidityPeriod") %>'>
                        </asp:Label>
                        <asp:Label ID="lblFRValidityPeriodText" runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="FRValidity EndDate">
                    <ItemTemplate>
                        <asp:Label ID="lblFRValidityEndDate" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"FRValidityEndDate") %>'>
                        </asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Vehicle RTACircle">
                    <ItemTemplate>
                        <asp:Label ID="lblVehicleRTACircle" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"VehicleRTACircle") %>'>
                        </asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="FRReceipt No">
                    <ItemTemplate>
                        <asp:Label ID="lblFRReceiptNo" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"FRReceiptNo") %>'>
                        </asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="FRFee">
                    <ItemTemplate>
                        <asp:Label ID="lblFRFee" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"FRFee") %>'>
                        </asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Edit">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkEdit" runat="server" CommandName="fitnessRenewalEdit" CommandArgument='<%#DataBinder.Eval(Container.DataItem,"FitnessRenewalID") %>'
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
<asp:HiddenField ID="vehiclePurchaseDate" runat="server"/>
</table>

</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>