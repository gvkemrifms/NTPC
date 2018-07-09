<%@ Page Language="C#" MasterPageFile="~/temp.master" AutoEventWireup="true" CodeFile="VehicleRegistration.aspx.cs" Inherits="VehicleRegistration" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<script src="../JavaValidations/RequiredFieldValidations.js" type="text/javascript"></script>
<script type="text/javascript">
    function pageLoad() {
        $('#<%= txtRegistrationDate.ClientID %>').datepicker({
            dateFormat: 'mm/dd/yy',
            changeMonth: true,
            changeYear: true
        });
        $('#<%= ddlDistrict.ClientID %>').select2({
            disable_search_threshold: 5,
            search_contains: true,
            minimumResultsForSearch: 20,
            placeholder: "Select an option"
        });
    }
    function validation() {
        var sittingCapacity = document.getElementById('<%= txtSittingCapacity.ClientID %>');
        var prNo = document.getElementById('<%= txtPRNo.ClientID %>');
        var registrationDate = document.getElementById('<%= txtRegistrationDate.ClientID %>');
        var rtaCircle = document.getElementById('<%= txtRTACircle.ClientID %>');
        var district = document.getElementById('<%= ddlDistrict.ClientID %>');
        var regisExpenses = document.getElementById('<%= txtRegisExpenses.ClientID %>');
        var vehiclePdiDate = document.getElementById('<%= vehiclePDIDate.ClientID %>');
        var now = new Date();
        var id = document.getElementById('<%= ddlTRNo.ClientID %>');
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
        if (!RequiredValidation(sittingCapacity, "Sitting Capacity Cannot be Blank"))
            return false;
        if (!RequiredValidation(prNo, "P/R No Cannot be Blank"))
            return false;
        if (prNo.value !== "") {
            if (!isValidVehicleNumber(prNo.value)) {
                prNo.value = "";
                prNo.focus();
                return false;
            }
        }
        if (!RequiredValidation(registrationDate, "Registration Date Cannot be Blank"))
            return false;
        if (!isValidDate(registrationDate.value)) {
            alert("Enter Valid Date");
            registrationDate.focus();
            return false;
        }
        debugger;
        if (date.parse(registrationDate.value) < date.parse(vehiclePdiDate.value)) {
            alert(
                "Registration Date should be greater than Pre-Delivery Inspection Date.(Pre-Delivery Inspection Date-" +
                vehiclePdiDate.value +
                ")");
            registrationDate.focus();
            return false;
        }
        if (Date.parse(registrationDate.value) > Date.parse(now)) {
            alert("Registration Date should not be greater than Current Date");
            registrationDate.focus();
            return false;
        }
        if (!RequiredValidation(rtaCircle, "RTA Circle Cannot be Blank"))
            return false;
        switch (district.selectedIndex) {
        case 0:
            alert("Please Select District");
            district.focus();
            return false;
        }
        if (!RequiredValidation(regisExpenses, "Registration Expenses Cannot be Blank"))
            return false;
        return true;
    }
</script>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>

<table class="table table-striped table-bordered table-hover">
<tr>
    <legend align="center" style="color: brown">
        <caption>
            Vehicle Registration
        </caption>
    </legend>
    <td class="rowseparator"></td>
</tr>
<tr>
    <td>
        <asp:Panel ID="pnlVehicleRegistration" runat="server">
            <table style="width: 100%">
                <tr>
                    <td align="center" colspan="4"></td>
                </tr>
                <tr>
                    <td align="center" colspan="4"></td>
                </tr>
                <tr>
                    <td align="center" colspan="4"></td>
                </tr>
                <tr>
                    <td style="width: 287px"></td>
                    <td align="left" style="width: 300px">
                        T/R No.<span style="color: Red">*</span>
                    </td>
                    <td align="left" style="width: 400px">
                        <cc1:ComboBox ID="ddlTRNo" runat="server" AutoCompleteMode="Append"
                                      AutoPostBack="True" OnSelectedIndexChanged="ddlTRNo_SelectedIndexChanged"
                                      Width="145px" DropDownStyle="DropDownList">
                            <asp:ListItem Value="-1">--Select--</asp:ListItem>
                            <asp:ListItem Value="0">Dummy</asp:ListItem>
                        </cc1:ComboBox>
                        <asp:TextBox ID="txtTrNo" runat="server" CssClass="search_3" ReadOnly="True" Visible="False"
                                     Width="145px">
                        </asp:TextBox>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td style="width: 287px"></td>
                    <td align="left" style="width: 300px">
                        Engine No
                    </td>
                    <td align="left" style="width: 400px">
                        <asp:TextBox ID="txtEngineNo" runat="server" CssClass="search_3" BackColor="DarkGray"
                                     ReadOnly="True" Width="145px">
                        </asp:TextBox>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td style="width: 287px"></td>
                    <td align="left" style="width: 300px">
                        Chassis No
                    </td>
                    <td align="left" style="width: 400px">
                        <asp:TextBox ID="txtChassisNo" runat="server" CssClass="search_3" BackColor="DarkGray"
                                     ReadOnly="True" Width="145px">
                        </asp:TextBox>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td style="width: 287px"></td>
                    <td align="left" style="width: 300px">
                        Seating Capacity<span style="color: Red">*</span>
                    </td>
                    <td align="left" style="width: 400px">
                        <asp:TextBox ID="txtSittingCapacity" runat="server" CssClass="search_3" MaxLength="2"
                                     onkeypress="_return isDecimalNumberOnly(event);" Width="145px">
                        </asp:TextBox>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td style="width: 287px"></td>
                    <td align="left" style="width: 300px">
                        P/R No<span style="color: Red">*</span>
                    </td>
                    <td align="left" style="width: 400px">
                        <asp:TextBox ID="txtPRNo" runat="server" CssClass="search_3" MaxLength="10"
                                     onkeypress="_return alphanumeric_only(event);" Width="145px">
                        </asp:TextBox>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td style="width: 287px"></td>
                    <td align="left" style="width: 300px">
                        Registration Date<span style="color: Red">*</span>
                    </td>
                    <td align="left" style="width: 400px">
                        <asp:TextBox ID="txtRegistrationDate" CssClass="search_3" runat="server"  oncut="return false;"
                                     onkeypress="return false" Width="145px">
                        </asp:TextBox>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td style="width: 287px"></td>
                    <td align="left" style="width: 300px">
                        RTA Circle<span style="color: Red">*</span>
                    </td>
                    <td align="left" style="width: 400px">
                        <asp:TextBox ID="txtRTACircle" runat="server" CssClass="search_3" MaxLength="20"
                                     onkeypress="_return alpha_only_withspace(event);" Width="145px">
                        </asp:TextBox>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td style="width: 287px"></td>
                    <td align="left" style="width: 300px">
                        State<span style="color: Red">*</span>
                    </td>
                    <td align="left" style="width: 400px">
                        <asp:DropDownList ID="ddlDistrict" runat="server" Width="145px">
                            <asp:ListItem Value="-1">--Select--</asp:ListItem>
                            <asp:ListItem Value="0">Dummy</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td style="width: 287px"></td>
                    <td align="left" style="width: 300px">
                        Registration Expenses<span style="color: Red">*</span>
                    </td>
                    <td align="left" style="width: 400px">
                        <asp:TextBox ID="txtRegisExpenses" runat="server" CssClass="search_3" MaxLength="9"
                                     onkeypress="_return isDecimalNumberKey(event);" Width="145px">
                        </asp:TextBox>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td style="width: 287px">
                        &nbsp;
                    </td>
                    <td align="left" style="width: 300px">
                        &nbsp;
                    </td>
                    <td align="left" style="width: 400px">
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="width: 287px">
                        &nbsp;
                    </td>
                    <td align="center" style="width: 300px">
                        <asp:Button ID="btSave" Text="Save" runat="server" CssClass="form-submit-button" OnClick="btSave_Click"/>
                    </td>
                    <td align="left" style="width: 400px">
                        <asp:Button ID="btReset" runat="server" OnClick="btReset_Click" CssClass="form-submit-button" Text="Reset"/>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td style="width: 287px">
                        &nbsp;
                    </td>
                    <td align="center" style="width: 300px">
                        &nbsp;
                    </td>
                    <td align="left" style="width: 400px">
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="4"></td>
                </tr>
                <tr>
                    <td colspan="4"></td>
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
        <table align="center">
            <tr align="center">
                <td>
                    <asp:GridView ID="gvVehicleRegistration" runat="server" EmptyDataText="No Records Found"
                                  AutoGenerateColumns="False" CellPadding="3" Width="630px" OnRowCommand="gvVehicleRegistration_RowCommand" AllowPaging="True"
                                  OnPageIndexChanging="gvVehicleRegistration_PageIndexChanging"
                                  class="table table-striped table-bordered table-hover" HeaderStyle-ForeColor="#337ab7" PagerStyle-CssClass="pager" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px">
                        <RowStyle CssClass="rows" ForeColor="#000066"/>
                        <Columns>
                            <asp:TemplateField HeaderText="T/R No">
                                <ItemTemplate>
                                    <asp:Label ID="lblTRNo" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"TRNo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="P/R No">
                                <ItemTemplate>
                                    <asp:Label ID="lblPRNo" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"PRNo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Registration Date">
                                <ItemTemplate>
                                    <asp:Label ID="lblRegDate" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"RegDate") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="RTA Circle">
                                <ItemTemplate>
                                    <asp:Label ID="lblRTACircle" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"VehicleRTACircle") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="District">
                                <ItemTemplate>
                                    <asp:Label ID="lblDistrict" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"district_name") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkEdit" runat="server" CommandName="VehRegEdit" CommandArgument='<%#DataBinder.Eval(Container.DataItem,"VehicleRegID") %>'
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
        </table>
    </td>
</tr>
<tr>
    <td class="rowseparator"></td>
</tr>
<asp:HiddenField ID="vehiclePDIDate" runat="server"/>
</table>

</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>