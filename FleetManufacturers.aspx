<%@ Page Language="C#" MasterPageFile="~/temp.master" AutoEventWireup="true" CodeFile="FleetManufacturers.aspx.cs" Inherits="FleetManufacturers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<script type="text/javascript">

    function validationFleetManufacturers() {
        switch (document.getElementById("<%= txtManufacturerName.ClientID %>").value) {
        case '':
            alert("Please Enter Manufacturer Name");
            document.getElementById("<%= txtManufacturerName.ClientID %>").focus();
            return false;
        }

        switch (document.getElementById("<%= ddlManufacturerType.ClientID %>").selectedIndex) {
        case 0:
            alert("Please Select Manufacturer Type");
            document.getElementById("<%= ddlManufacturerType.ClientID %>").focus();
            return false;
        }
        switch (document.getElementById("<%= txtManufacturerModel.ClientID %>").value) {
        case '':
            alert("Please Select Manufacturer Model");
            document.getElementById("<%= txtManufacturerModel.ClientID %>").focus();
            return false;
        }
        switch (document.getElementById("<%= ddlFleetManufacturerDistrict.ClientID %>").selectedIndex) {
        case 0:
            alert("Please select State");
            document.getElementById("<%= ddlFleetManufacturerDistrict.ClientID %>").focus();
            return false;
        }
        switch (document.getElementById("<%= txtManufacturerAddress.ClientID %>").value) {
        case '':
            alert("Please Enter Manufacturer Address");
            document.getElementById("<%= txtManufacturerAddress.ClientID %>").focus();
            return false;
        }
        switch (document.getElementById("<%= txtManufacturerContactNumber.ClientID %>").value) {
        case '':
            alert("Please Enter Manufacturer Contact Number");
            document.getElementById("<%= txtManufacturerContactNumber.ClientID %>").focus();
            return false;

        }

        var phone = document.getElementById("<%= txtManufacturerContactNumber.ClientID %>");
        if (isNaN(parseInt(phone.value))) {
            alert("The phone number contains illegal characters");
            phone.focus();
            return false;
        }
        if (!((phone.value.length >= 10) && (phone.value.length <= 15))) {
            alert("The phone number is the wrong length");
            phone.focus();
            return false;
        }

        switch (document.getElementById("<%= txtManufacturerContactPerson.ClientID %>").value) {
        case '':
            alert("Please Enter Manufacturer Contact Person");
            document.getElementById("<%= txtManufacturerContactPerson.ClientID %>").focus();
            return false;
        }
        switch (document.getElementById("<%= txtManufacturerEmailId.ClientID %>").value) {
        case '':
            alert("Please Enter Manufacturer EmailId");
            document.getElementById("<%= txtManufacturerEmailId.ClientID %>").focus();
            return false;
        }

        var emailPat = /^(?:\w+\.?)*\w+@(?:\w+\.)+\w+$/;
        var emailid = document.getElementById("<%= txtManufacturerEmailId.ClientID %>").value;
        var matchArray = emailid.match(emailPat);
        if (matchArray == null) {
            alert("Your email address seems incorrect. Please try again.");
            document.getElementById("<%= txtManufacturerEmailId.ClientID %>").focus();
            return false;
        }

        switch (document.getElementById("<%= txtManufacturerTin.ClientID %>").value) {
        case '':
            alert("Please Enter Manufacturer Tin");
            document.getElementById("<%= txtManufacturerTin.ClientID %>").focus();
            return false;
        }
        switch (document.getElementById("<%= txtManufacturerErn.ClientID %>").value) {
        case '':
            alert("Please Enter Manufacturer Ern");
            document.getElementById("<%= txtManufacturerErn.ClientID %>").focus();
            return false;
        }
        return true;
    }
</script>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
<script type="text/javascript">
    function pageLoad() {

        $('#<%= ddlFleetManufacturerDistrict.ClientID %>').select2({
            disable_search_threshold: 5,
            search_contains: true,
            minimumResultsForSearch: 20,
            placeholder: "Select an option"
        });
    }
</script>
<table id="table1" align="center" style="height: 37px">
<tr>
    <td class="rowseparator"></td>
</tr>
<tr>
    <td>
        <fieldset style="padding: 10px">
            <legend align="center" style="color: brown">Fleet Manufacturers Details</legend>
            <asp:Panel ID="pnlFleetManufacturers" runat="server">
                <table id="table2" class="bordergreen" width="91%" align="center">
                    <tr>
                        <td>
                            <table align="center">
                                <tr>
                                    <td style="width: 150px" align="left">
                                        Manufacturer Name <span style="color: Red">*</span>
                                    </td>
                                    <td class="columnseparator"></td>
                                    <td>
                                        <asp:TextBox ID="txtManufacturerName" runat="server" Width="150px" CssClass="search_3" MaxLength="35"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="rowseparator"></td>
                                </tr>
                                <tr>
                                    <td style="width: 150px" align="left">
                                        Manufacturer Type <span style="color: Red">*</span>
                                    </td>
                                    <td class="columnseparator"></td>
                                    <td>
                                        <asp:DropDownList ID="ddlManufacturerType" runat="server" Width="150px" CssClass="search_3">
                                            <asp:ListItem Value="0">--Select--</asp:ListItem>
                                            <asp:ListItem Value="1">Battery</asp:ListItem>
                                            <asp:ListItem Value="2">Body</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="rowseparator"></td>
                                </tr>
                                <tr>
                                    <td style="width: 150px" align="left">
                                        Model <span style="color: Red">*</span>
                                    </td>
                                    <td class="columnseparator"></td>
                                    <td style="height: 23px">
                                        <asp:TextBox ID="txtManufacturerModel" runat="server" Width="150px" CssClass="search_3" MaxLength="15"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="rowseparator"></td>
                                </tr>
                                <tr>
                                    <td class="rowseparator"></td>
                                </tr>
                                <tr>
                                    <td style="width: 150px" align="left">
                                        State<span style="color: red">*</span>

                                    </td>
                                    <td class="columnseparator"></td>
                                    <td style="height: 23px">
                                        <asp:DropDownList ID="ddlFleetManufacturerDistrict" runat="server" Width="150px"
                                                          AutoPostBack="True">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="rowseparator"></td>
                                </tr>
                                <tr>
                                    <td style="width: 150px" align="left">
                                        Address <span style="color: Red">*</span>
                                    </td>
                                    <td class="columnseparator"></td>
                                    <td>
                                        <asp:TextBox ID="txtManufacturerAddress" Width="150px" runat="server" TextMode="MultiLine" CssClass="search_3"
                                                     onKeyUp="CheckLength(this,300)" onChange="CheckLength(this,300)">
                                        </asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="rowseparator"></td>
                                </tr>
                                <tr>
                                    <td style="width: 150px" align="left">
                                        Contact Number <span style="color: Red">*</span>
                                    </td>
                                    <td class="columnseparator"></td>
                                    <td>
                                        <asp:TextBox ID="txtManufacturerContactNumber" Width="150px" runat="server" CssClass="search_3"
                                                     MaxLength="15" onkeypress="return numeric_only(this)">
                                        </asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="rowseparator"></td>
                                </tr>
                                <tr>
                                    <td style="width: 150px" align="left">
                                        Contact Person

                                    </td>
                                    <td class="columnseparator"></td>
                                    <td>
                                        <asp:TextBox ID="txtManufacturerContactPerson" Width="150px" runat="server" CssClass="search_3"
                                                     MaxLength="35">
                                        </asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="rowseparator"></td>
                                </tr>
                                <tr>
                                    <td style="width: 150px" align="left">
                                        E-mail ID <span style="color: Red">*</span>
                                    </td>
                                    <td class="columnseparator"></td>
                                    <td>
                                        <asp:TextBox ID="txtManufacturerEmailId" Width="150px" runat="server" CssClass="search_3" MaxLength="50"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="rowseparator"></td>
                                </tr>
                                <tr>
                                    <td style="width: 150px" align="left">
                                        TIN <span style="color: Red">*</span> &nbsp;
                                    </td>
                                    <td class="columnseparator"></td>
                                    <td>
                                        <asp:TextBox ID="txtManufacturerTin" runat="server" Width="150px" CssClass="search_3" MaxLength="11" onkeypress="return numeric_only(this)"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="rowseparator"></td>
                                </tr>
                                <tr>
                                    <td style="width: 150px" align="left">
                                        ERN <span style="color: Red">*</span>
                                    </td>
                                    <td class="columnseparator"></td>
                                    <td>
                                        <asp:TextBox ID="txtManufacturerErn" runat="server" Width="150px" CssClass="search_3" MaxLength="11" onkeypress="return numeric_only(this)"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="rowseparator"></td>
                                </tr>
                                <tr>
                                    <td colspan="3" style="height: 41px">
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btnManufacturerSave" Width="55px" runat="server" CssClass="form-submit-button"
                                                    Text="Save" OnClick="btnManufacturerSave_Click">
                                        </asp:Button>
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button Width="55px" runat="server" CssClass="form-reset-button"
                                                    Text="Reset" CausesValidation="false" OnClick="btnManufacturerReset_Click">
                                        </asp:Button>
                                        <input type="hidden" runat="server"/>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="rowseparator"></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </fieldset>
    </td>
</tr>
<tr>
    <td class="rowseparator"></td>
</tr>
<tr>
    <td>
        <fieldset style="padding: 10px">
            <asp:GridView ID="grvManufacturerDetails" runat="server" AllowPaging="True" PageSize="5" width="700px"
                          AutoGenerateColumns="False" CellPadding="3" class="table table-striped table-bordered table-hover"
                          OnRowEditing="grvManufacturerDetails_RowEditing" OnPageIndexChanging="grvManufacturerDetails_PageIndexChanging" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px">
                <RowStyle CssClass="rowStyleGrid" ForeColor="#000066"/>
                <Columns>
                    <asp:TemplateField HeaderText="Id">
                        <ItemTemplate>
                            <asp:Label ID="lblId" runat="server" Text='<%#Eval("FleetManufacturer_Id") %>'/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Manufacturer_Name">
                        <ItemTemplate>
                            <asp:Label ID="lblManufacturerName" runat="server" Text='<%#Eval("FleetManufacturer_Name") %>'/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Contact Number">
                        <ItemTemplate>
                            <asp:Label ID="lblContactNumber" runat="server" Text='<%#Eval("FleetManufacturer_ContactNo") %>'/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Contact Person">
                        <ItemTemplate>
                            <asp:Label ID="lblContactPerson" runat="server" Text='<%#Eval("FleetManufacturer_ContactPerson") %>'/>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Creation Date">
                        <ItemTemplate>
                            <asp:Label ID="lblCreateDate" runat="server" Text='<%#Eval("Created_Date", "{0:d}") %>'/>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Edit">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkbtnEdit" runat="server" Text="Edit" CommandName="Edit"/>
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
            <br/>
            <asp:HiddenField ID="hidManId" runat="server"/>
        </fieldset>
    </td>
</tr>
<tr>
    <td class="rowseparator"></td>
</tr>
</table>
</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>