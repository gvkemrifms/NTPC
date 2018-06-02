<%@ Page Language="C#" MasterPageFile="~/temp.master" AutoEventWireup="true" CodeFile="SparePartsMaster.aspx.cs" Inherits="SparePartsMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        function validation() {
            var fldSparePartName = document.getElementById('<%= txtSparePartName.ClientID %>');
            var fldManufacturerSpareId = document.getElementById('<%= txtManufacturerSpareID.ClientID %>');
            var fldManufacturerId = document.getElementById('<%= ddlManufacturerID.ClientID %>');
            var fldSparePartGroupId = document.getElementById('<%= txtSparePartGroupID.ClientID %>');
            var fldGroupName = document.getElementById('<%= txtGroupName.ClientID %>');
            var fldCost = document.getElementById('<%= txtCost.ClientID %>');

            if (fldSparePartName)
                if (!RequiredValidation(fldSparePartName, "Please enter Spare Part Name"))
                    return false;
            if (fldManufacturerSpareId)
                if (!RequiredValidation(fldManufacturerSpareId, "Please enter ManufacturerId"))
                    return false;

            if (fldManufacturerId && fldManufacturerId.selectedIndex === 0) {
                alert("Please select Manufacturer");
                fldManufacturerId.focus();
                return false;
            }

            if (fldSparePartGroupId)
                if (!RequiredValidation(fldSparePartGroupId, "Please enter Spare Part GroupId"))
                    return false;
            if (fldGroupName)
                if (!RequiredValidation(fldGroupName, "Please enter Spare Part Group Name"))
                    return false;
            if (fldCost)
                if (!RequiredValidation(fldCost, "Please enter Cost"))
                    return false;
            return true;
        }

        function OnlyAlphabets(myfield, e, dec) {
            var key;
            if (window.event)
                key = window.event.keyCode;
            else if (e)
                key = e.which;
            else
                return true;
            var keychar = String.fromCharCode(key);
            if ((("!@#$%^&*()_+=-';{}[]|?<>:,/\".1234567890").indexOf(keychar) > -1))
                return false;
            else
                return true;
        }

        function onKeyPressBlockNumbers(value) {
            var reg = /^\-?([1-9]\d*|0)(\.\d?[1-9])?$/;
            if (!reg.test(value)) {
                alert("please enter numeric values only");
                document.getElementById("<%= txtCost.ClientID %>").value = "";
                return false;
            }

            return reg.test(value);
        }

    </script>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <table align="center">
                <tr>
                    <td class="rowseparator"></td>
                </tr>
                <tr>
                    <td>
                        <fieldset style="padding: 10px">
                            <legend align="center" style="color: brown">Spare Parts Details</legend>
                            <asp:Panel runat="server">
                                <table align="center">
                                    <tr>
                                        <td >
                                            <asp:Label runat="server" Text="Spare Part Name"></asp:Label>
                                            <span style="color: Red">*</span>
                                        </td>

                                        <td>
                                            <asp:TextBox ID="txtSparePartName" onkeypress="return alpha_only_withspace(event);" CssClass="search_3" runat="server" MaxLength="22"></asp:TextBox>
                                        </td>

                                        <td >
                                            <asp:Label runat="server" Text="Manufacturer Spare ID" style="margin-left: 20px">
                                            </asp:Label>
                                            <span style="color: Red">*</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtManufacturerSpareID" CssClass="search_3" runat="server" MaxLength="16" onkeypress="return numeric_only(event)"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label runat="server" Text="Manufacturer Name"></asp:Label>
                                            <span style="color: Red">*</span>
                                        </td>
                                        <td >
                                            <asp:DropDownList ID="ddlManufacturerID" CssClass="search_3" runat="server" Width="150px"/>
                                        </td>
                                        <td>
                                            <asp:Label runat="server" style="margin-left: 20px" Text="Spare Part Group ID"></asp:Label>
                                            <span style="color: Red">*</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSparePartGroupID" CssClass="search_3" runat="server" MaxLength="10" onkeypress="return numeric_only(event)"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td >
                                            <asp:Label runat="server" Text="Group Name"></asp:Label>
                                            <span style="color: Red">*</span>
                                        </td>
                                        <td >
                                            <asp:TextBox ID="txtGroupName" CssClass="search_3" runat="server" MaxLength="15" onkeypress="return alpha_only_withspace(event);"></asp:TextBox>
                                        </td>
                                        <td >
                                            <asp:Label runat="server" style="margin-left: 20px" Text="Cost"></asp:Label>
                                            <span style="color: Red">*</span>
                                        </td>

                                        <td >
                                            <asp:TextBox ID="txtCost" CssClass="search_3" runat="server" MaxLength="6" onchange="onKeyPressBlockNumbers(this.value);" onkeypress="return numeric(event)"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtSparePartID" CssClass="search_3" runat="server" Visible="False"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="rowseparator"></td>
                                    </tr>
                                    <tr>
                                        <td colspan="7" align="center">
                                            <asp:Button ID="btSave" runat="server" CssClass="form-submit-button" OnClick="btSave_Click" Text="Save" OnClientClick="return validation();"/>
                                            &nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:Button ID="btReset" runat="server" CssClass="form-reset-button" OnClick="btReset_Click" Text="Reset"/>
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
                                <asp:GridView ID="gvSpareParts" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" CssClass="gridviewStyle" OnPageIndexChanging="gvSpareParts_PageIndexChanging" OnRowDeleting="gvSpareParts_RowDeleting" OnRowEditing="gvSpareParts_RowEditing">
                                    <RowStyle CssClass="rowStyleGrid" ForeColor="#000066"/>
                                    <Columns>
                                        <asp:TemplateField HeaderText="SparePart Id">
                                            <ItemTemplate>
                                                <asp:Label ID="lblId" runat="server" Text='<%#Eval("SparePart_Id") %>'/>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="SparePart Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSparePartName" runat="server" Text='<%#Eval("SparePart_Name") %>'/>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Manufacturer SpareId">
                                            <ItemTemplate>
                                                <asp:Label ID="lblManSprId" runat="server" Text='<%#Eval("ManufacturerSpare_Id") %>'/>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Manufacturer Id">
                                            <ItemTemplate>
                                                <asp:Label ID="lblManId" runat="server" Text='<%#Eval("Manufacturer_Id") %>'/>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Group Id">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGroupId" runat="server" Text='<%#Eval("SparePart_Group_Id") %>'/>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Group Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGroupName" runat="server" Text='<%#Eval("Group_Name") %>'/>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Cost">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCost" runat="server" Text='<%#Eval("Cost") %>'/>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Edit">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" Text="Edit"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Delete">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" Text="Deactivate"></asp:LinkButton>
                                                <asp:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" ConfirmText="Are You sure you want to DEACTIVATE" TargetControlID="lnkDelete">
                                                </asp:ConfirmButtonExtender>
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
            <asp:HiddenField ID="hidSpareId" runat="server"/>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>