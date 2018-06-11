<%@ Page Language="C#" masterpagefile="~/temp.master" AutoEventWireup="true" CodeFile="MaintenanceWorksMaster.aspx.cs" Inherits="MaintenanceWorksMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<script type="text/javascript">

    function validationMaintenanceWorksMaster() {
        switch (document.getElementById("<%= ddlServiceGroupName.ClientID %>").selectedIndex) {
        case 0:
            document.getElementById("<%= ddlServiceGroupName.ClientID %>").focus();
            alert("Please Select Service Group Name");
            return false;
        }
        switch (document.getElementById("<%= ddlMaintenanceManufacturerName.ClientID %>").selectedIndex) {
        case 0:
            document.getElementById("<%= ddlMaintenanceManufacturerName.ClientID %>").focus();
            alert("Please Enter Manufacturer Name");
            return false;
        }
        if ($('#<%= txtCategories.ClientID %>').is(":visible")) {
            var value = $("#<%= txtCategories.ClientID %>").val();
            switch (value) {
            case '':
                $("<%= txtCategories.ClientID %>").focus();
                return alert("Please Enter Category Name(Or) Select Existing Category");
            }
        } else {
            switch (document.getElementById("<%= ddlSSName.ClientID %>").selectedIndex) {
            case 0:
                document.getElementById("<%= ddlSSName.ClientID %>").focus();
                return alert("Please select Category");

            }
        }
        switch (document.getElementById("<%= txtServiceName.ClientID %>").value) {
        case '':
            document.getElementById("<%= txtServiceName.ClientID %>").focus();
            alert("Please Enter Service Name");
            return false;
        }
        switch (document.getElementById("<%= txtCostGrade.ClientID %>").value) {
        case '':
            document.getElementById("<%= txtCostGrade.ClientID %>").focus();
            alert("Please Enter Cost For A Grade");
            return false;
        }
        switch (document.getElementById("<%= txtCostOtherGrade.ClientID %>").value) {
        case '':
            document.getElementById("<%= txtCostOtherGrade.ClientID %>").focus();
            alert("Please Enter Cost For Other Grade");
            return false;
        }
        switch (document.getElementById("<%= txtTimeTaken.ClientID %>").value) {
        case '':
            document.getElementById("<%= txtTimeTaken.ClientID %>").focus();
            alert("Please Enter Time taken");
            return false;
        }
        return true;

    }
</script>
<asp:UpdatePanel runat="server">
<ContentTemplate>
<legend style="color: brown" align="center">Maintenance Works Master</legend>
<table align="center" style="margin-top: 10px">
    <tr>
        <td class="rowseparator"></td>
    </tr>
    <tr>
        <td>
            <fieldset style="padding: 10px">

                <asp:Panel ID="pnlMaintenanceWorksMaster" runat="server">
                    <table align="center">
                        <tr>
                            <td align="left">
                                Aggregates <span style="color: Red">*</span> &nbsp;
                            </td>
                            <td class="columnseparator"></td>
                            <td>
                                <asp:DropDownList ID="ddlServiceGroupName" CssClass="search_3" runat="server" Width="150px"
                                                  AutoPostBack="True" OnSelectedIndexChanged="ddlServiceGroupName_SelectedIndexChanged">
                                    <asp:ListItem></asp:ListItem>
                                </asp:DropDownList>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="rowseparator"></td>
                        </tr>
                        <tr>
                            <td align="left">
                                Manufacturer Name <span style="color: Red">*</span> &nbsp;
                            </td>
                            <td class="columnseparator"></td>
                            <td>
                                <asp:DropDownList ID="ddlMaintenanceManufacturerName" CssClass="search_3" runat="server" AutoPostBack="true"
                                                  Width="150px"
                                                  OnSelectedIndexChanged="ddlMaintenanceManufacturerName_SelectedIndexChanged"/>
                                &nbsp;&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="rowseparator"></td>
                        </tr>
                        <tr>
                            <td align="left">
                                Categories <span style="color: Red">*</span> &nbsp;
                            </td>
                            <td class="columnseparator"></td>
                            <td>
                                <asp:TextBox ID="txtCategories" CssClass="search_3" runat="server" onkeypress="return OnlyAlphabets(event)"></asp:TextBox>
                                &nbsp;&nbsp;
                                <asp:DropDownList ID="ddlSSName" runat="server" Visible="false" Width="127px"/>
                            </td>
                            <td class="columnseparator"></td>
                            <td>
                                <asp:LinkButton runat="server" ID="linkCat" Text="Add existing"
                                                OnClick="linkCat_Click"/>
                                <asp:LinkButton runat="server" ID="linkNew" Text="Add New" OnClick="linkNew_Click"
                                                Visible="false"/>
                            </td>
                        </tr>
                        <tr>
                            <td class="rowseparator"></td>
                        </tr>

                        <tr>
                            <td align="left">
                                Sub Categories <span style="color: Red">*</span> &nbsp;
                            </td>
                            <td class="columnseparator"></td>
                            <td>
                                <asp:TextBox ID="txtServiceName" CssClass="search_3" runat="server" onkeypress="return OnlyAlphabets(e)"></asp:TextBox>
                                &nbsp;&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="rowseparator"></td>
                        </tr>
                        <tr>
                            <td align="left">
                                Cost For A Grade <span style="color: Red">*</span> &nbsp;
                            </td>
                            <td class="columnseparator"></td>
                            <td>
                                <asp:TextBox ID="txtCostGrade" CssClass="search_3" runat="server" MaxLength="10" onkeypress="return numericOnly(this)"></asp:TextBox>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="rowseparator"></td>
                        </tr>
                        <tr>
                            <td align="left">
                                Cost Other Than A Grade <span style="color: Red">*</span> &nbsp;
                            </td>
                            <td class="columnseparator"></td>
                            <td>
                                <asp:TextBox ID="txtCostOtherGrade" CssClass="search_3" runat="server" MaxLength="10" onkeypress="return numericOnly(this)"></asp:TextBox>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="rowseparator"></td>
                        </tr>
                        <tr>
                            <td align="left">
                                Time Taken(In Mins) <span style="color: Red">*</span> &nbsp;
                            </td>
                            <td class="columnseparator"></td>
                            <td>
                                <asp:TextBox ID="txtTimeTaken" runat="server" CssClass="search_3" MaxLength="5" onkeypress="return numericOnly(this) "></asp:TextBox>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="rowseparator"></td>
                        </tr>
                        <tr>
                            <td align="center" colspan="3">
                                <asp:Button ID="btnSaveMaintenanceWorksMaster" CssClass="form-submit-button" runat="server" Text="Save" OnClick="btnSaveMaintenanceWorksMaster_Click" OnClientClick="if (!validationMaintenanceWorksMaster()) return false;"/>
                                &nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button CssClass="form-reset-button" runat="server" Text="Reset" OnClick="btnResetMaintenanceWorksMaster_Click"/>
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
            <td>
                <fieldset style="padding: 10px">
                    <asp:GridView ID="grvMaintenanceWorksMasterDetails" runat="server" AllowPaging="True" AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" CssClass="gridviewStyle" OnPageIndexChanging="grvMaintenanceWorksMasterDetails_PageIndexChanging" OnRowEditing="grvMaintenanceWorksMasterDetails_RowEditing" PageSize="5">
                        <RowStyle CssClass="rowStyleGrid" ForeColor="#000066"/>
                        <Columns>
                            <asp:TemplateField HeaderText="Aggregates">
                                <ItemTemplate>
                                    <asp:Label ID="lblServiceGroupName" runat="server" Text='<%#Eval("Aggregates") %>'/>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Id">
                                <ItemTemplate>
                                    <asp:Label ID="lblServiceId" runat="server" Text='<%#Eval("Service_Id") %>'/>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Categories">
                                <ItemTemplate>
                                    <asp:Label ID="lblSubServiceName" runat="server" Text='<%#Eval("Categories") %>'/>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Sub Categories">
                                <ItemTemplate>
                                    <asp:Label ID="lblSubCategories" runat="server" Text='<%#Eval("Sub Categories") %>'/>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cost">
                                <ItemTemplate>
                                    <asp:Label ID="lblServiceName" runat="server" Text='<%#Eval("Cost") %>'/>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Time Taken(In Mins)">
                                <ItemTemplate>
                                    <asp:Label ID="lblTimeTaken" runat="server" Text='<%#Eval("Time_Taken") %>'/>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Creation Date">
                                <ItemTemplate>
                                    <asp:Label ID="lblCreateDate" runat="server" Text='<%#Eval("Creation_Date", "{0:d}") %>'/>
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
                </fieldset>
                <asp:HiddenField ID="hidWorksMasterId" runat="server"/>
            </td>
        </tr>
        <tr>
            <td class="rowseparator"></td>
        </tr>
        <tr>
            <td class="rowseparator"></td>
        </tr>
    </caption>
</table>
</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>