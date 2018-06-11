<%@ Page AutoEventWireup="true" CodeFile="Fabricator.aspx.cs" Inherits="Fabricator" Language="C#" MasterPageFile="~/temp.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<script type="text/javascript">
    function validationFabricator() {
        switch (document.getElementById("<%= txtFabricatorName.ClientID %>").value) {
        case '':
            alert("Please Enter Fabricator Name");
            document.getElementById("<%= txtFabricatorName.ClientID %>").focus();
            return false;
        }
        switch (document.getElementById("<%= ddlFabricatorType.ClientID %>").selectedIndex) {
        case 0:
            alert("Please Select Fabricator Type");
            document.getElementById("<%= ddlFabricatorType.ClientID %>").focus();
            return false;
        }
        switch (document.getElementById("<%= txtFabricatorAddress.ClientID %>").value) {
        case '':
            alert("Please Enter Fabricator Address");
            document.getElementById("<%= txtFabricatorAddress.ClientID %>").focus();
            return false;
        }
        switch (document.getElementById("<%= txtFabricatorContactNumber.ClientID %>").value) {
        case '':
            alert("Please Enter Fabricator Contact Number");
            document.getElementById("<%= txtFabricatorContactNumber.ClientID %>").focus();
            return false;
        }
        var phone = document.getElementById("<%= txtFabricatorContactNumber.ClientID %>");
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
        switch (document.getElementById('<%= txtFabricatorContactPerson.ClientID %>').value) {
        case '':
            alert("Please Enter Fabricator Contact Person");
            document.getElementById("<%= txtFabricatorContactPerson.ClientID %>").focus();
            return false;
        }
        switch (document.getElementById("<%= txtFabricatorPanNo.ClientID %>").value) {
        case '':
            alert("Please Enter PAN No.");
            document.getElementById("<%= txtFabricatorPanNo.ClientID %>").focus();
            return false;
        }
        var pan = document.getElementById("<%= txtFabricatorPanNo.ClientID %>").value;
        if (isValidPAN(pan) === false) {
            return false;
        }
        switch (document.getElementById("<%= txtFabricatorEmailId.ClientID %>").value) {
        case '':
            alert("Please Enter Fabricator EmailId");
            document.getElementById("<%= txtFabricatorEmailId.ClientID %>").focus();
            return false;
        }
        var emailPat =
            /[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?/;
        var emailid = document.getElementById("<%= txtFabricatorEmailId.ClientID %>").value;
        var matchArray = emailid.match(emailPat);
        if (matchArray == null) {
            alert("Your email address seems incorrect. Please try again.");
            document.getElementById("<%= txtFabricatorEmailId.ClientID %>").focus();
            return false;
        } else {
            switch (document.getElementById("<%= txtFabricatorTin.ClientID %>").value) {
            case '':
                alert("Please Enter Fabricator Tin");
                document.getElementById("<%= txtFabricatorTin.ClientID %>").focus();
                return false;
            }
            switch (document.getElementById("<%= txtFabricatorErn.ClientID %>").value) {
            case '':
                alert("Please Enter Fabricator Ern");
                document.getElementById("<%= txtFabricatorErn.ClientID %>").focus();
                return false;
            }
        }
        return true;
    }
</script>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
<script type="text/javascript">
    function pageLoad() {
        $('#<%= ddlFabricatorDistrict.ClientID %>').select2({
            disable_search_threshold: 5,
            search_contains: true,
            minimumResultsForSearch: 2,
            placeholder: "Select an option"
        });
    }
</script>

<table id="table1" cellspacing="0" cellpadding="0" width="100%" align="center" border="0"
       style="height: 37px">
<tr>
    <td class="rowseparator"></td>
</tr>
<tr>
    <td style="height: 281px">
        <fieldset style="padding: 10px">
            <legend align="center" style="color: brown">Fabricator</legend>
            <asp:Panel ID="pnlfabricator" runat="server">
                <table id="table2"align="center">
                    <tr>
                        <td style="height: 294px; width: 518px">
                            <table>
                                <tr>
                                    <td style="width: 140px" align="left">
                                        Fabricator Name <span style="color: Red">*</span>
                                    </td>
                                    <td class="columnseparator"></td>
                                    <td>
                                        <asp:TextBox ID="txtFabricatorName" runat="server" CssClass="search_3" Width="150px" MaxLength="35"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="rowseparator"></td>
                                </tr>
                                <tr>
                                    <td style="width: 140px" align="left">
                                        Fabricator Type <span style="color: Red">*</span>
                                    </td>
                                    <td class="columnseparator"></td>
                                    <td>
                                        <asp:DropDownList ID="ddlFabricatorType" CssClass="search_3" Width="150px" runat="server">
                                            <asp:ListItem Value="0">--Select--</asp:ListItem>
                                            <asp:ListItem Value="1">Body</asp:ListItem>
                                            <asp:ListItem Value="2">FRP</asp:ListItem>
                                            <asp:ListItem Value="3">MS</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="rowseparator"></td>
                                </tr>

                                <tr>
                                    <td class="rowseparator"></td>
                                </tr>
                                <tr>
                                    <td style="width: 140px" align="left">
                                        State<span style="color: red">*</span>
                                    </td>
                                    <td class="columnseparator"></td>
                                    <td style="height: 23px">
                                        <asp:DropDownList ID="ddlFabricatorDistrict" runat="server" Width="150px"
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
                                        <asp:TextBox ID="txtFabricatorAddress" runat="server" TextMode="MultiLine" CssClass="search_3" Height="50px"
                                                     onKeyUp="CheckLength(this,300)" onChange="CheckLength(this,300)">
                                        </asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="rowseparator"></td>
                                </tr>
                                <tr>
                                    <td style="width: 140px" align="left">
                                        Contact Number <span style="color: Red">*</span>
                                    </td>
                                    <td class="columnseparator"></td>
                                    <td>
                                        <asp:TextBox ID="txtFabricatorContactNumber" runat="server" Width="150px" CssClass="search_3"
                                                     MaxLength="15">
                                        </asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="rowseparator"></td>
                                </tr>
                                <tr>
                                    <td style="width: 140px" align="left">
                                        Contact Person <span style="color: red">*</span>

                                    </td>
                                    <td class="columnseparator"></td>
                                    <td>
                                        <asp:TextBox ID="txtFabricatorContactPerson" Width="150px" runat="server" CssClass="search_3"
                                                     MaxLength="35">
                                        </asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="rowseparator"></td>
                                </tr>
                                <tr>
                                    <td style="width: 140px" align="left">
                                        PAN No <span style="color: Red">*</span>
                                    </td>
                                    <td class="columnseparator"></td>
                                    <td>
                                        <asp:TextBox ID="txtFabricatorPanNo" runat="server" Width="150px" CssClass="search_3" MaxLength="10"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="rowseparator"></td>
                                </tr>
                                <tr>
                                    <td align="left" style="width: 140px">
                                        E-mail ID <span style="color: Red">*</span>
                                    </td>
                                    <td class="columnseparator"></td>
                                    <td>
                                        <asp:TextBox ID="txtFabricatorEmailId" CssClass="search_3" Width="150px" runat="server" MaxLength="50"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="rowseparator"></td>
                                </tr>
                                <tr>
                                    <td align="left" style="width: 140px">
                                        TIN <span style="color: Red">*</span> &nbsp;
                                    </td>
                                    <td class="columnseparator"></td>
                                    <td>
                                        <asp:TextBox ID="txtFabricatorTin" CssClass="search_3" Width="150px" runat="server" MaxLength="11" onkeypress="return numeric_only(event);"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="rowseparator"></td>
                                </tr>
                                <tr>
                                    <td align="left" style="width: 140px">
                                        ERN <span style="color: Red">*</span>
                                    </td>
                                    <td class="columnseparator"></td>
                                    <td>
                                        <asp:TextBox ID="txtFabricatorErn" MaxLength="11" CssClass="search_3" Width="150px" runat="server" name="floats" ondrop="return false;" onpaste="return false;" onkeypress="return numeric_only(event);"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="rowseparator"></td>
                                </tr>
                                <tr>
                                    <td colspan="3" style="height: 41px">
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btnFabricatorSave" runat="server" CssClass="form-submit-button" OnClick="btnFabricatorSave_Click"
                                                    OnClientClick="return validationFabricator();" Text="Save" Width="55px"/>
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btnFabricatorReset" runat="server" CausesValidation="false" CssClass="form-reset-button"
                                                    OnClick="btnFabricatorReset_Click" Text="Reset" Width="55px"/>
                                        <input id="hidBatText" runat="server" type="hidden"/>
                                    </td>
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
<caption>
    <br/>
    <tr>
        <td align="center">
            <fieldset style="padding: 10px">
                <asp:GridView ID="grvFabricatorDetails" runat="server" AllowPaging="True" AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" CssClass="gridviewStyle" OnPageIndexChanging="grvFabricatorDetails_PageIndexChanging" OnRowEditing="grvFabricatorDetails_RowEditing" PageSize="5">
                    <RowStyle CssClass="rowStyleGrid" ForeColor="#000066"/>
                    <Columns>
                        <asp:TemplateField HeaderText="Id">
                            <ItemTemplate>
                                <asp:Label ID="lblId" runat="server" Text='<%#Eval("FleetFabricator_Id") %>'/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Fabricator_Name">
                            <ItemTemplate>
                                <asp:Label ID="lblFabricatorName" runat="server" Text='<%#Eval("FleetFabricator_Name") %>'/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="PAN No">
                            <ItemTemplate>
                                <asp:Label ID="lblPanno" runat="server" Text='<%#Eval("FleetFabricator_PAN") %>'/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="TIN No">
                            <ItemTemplate>
                                <asp:Label ID="lblTinno" runat="server" Text='<%#Eval("FleetFabricator_TIN") %>'/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Contact Person">
                            <ItemTemplate>
                                <asp:Label ID="lblContactPerson" runat="server" Text='<%#Eval("FleetFabricator_ContactPerson") %>'/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Contact Number">
                            <ItemTemplate>
                                <asp:Label ID="lblContactNumber" runat="server" Text='<%#Eval("FleetFabricator_ContactNo") %>'/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Creation Date">
                            <ItemTemplate>
                                <asp:Label ID="lblCreateDate" runat="server" Text='<%#Eval("Created_Date","{0:d}") %>'/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Edit">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkbtnEdit" runat="server" CommandName="Edit" Text="Edit"/>
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
                <asp:HiddenField ID="hidFabId" runat="server"/>
            </fieldset>
        </td>
    </tr>
    <tr>
        <td class="rowseparator"></td>
    </tr>
</caption>
</table>
</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>