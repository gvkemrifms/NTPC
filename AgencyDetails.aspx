<%@ Page AutoEventWireup="true" CodeFile="AgencyDetails.aspx.cs" Inherits="AgencyDetails" Language="C#" MasterPageFile="~/temp.master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<script type="text/javascript">
    function validationAgencyDetails() {
        $('#<%= ddlDistrict.ClientID %>').chosen();
        var ddldistrict = $('#<%= ddlDistrict.ClientID %> option:selected').text().toLowerCase();
        if (ddldistrict === '--select--') {
            return alert("Please Select District");
        }

        if (document.getElementById('<%= txtAgencyName.ClientID %>').value === "") {
            alert("Please Enter Agency Name");
            document.getElementById("<%= txtAgencyName.ClientID %>").focus();
            return false;
        }

        if (document.getElementById("<%= txtAddress.ClientID %>").value === "") {
            document.getElementById("<%= txtAddress.ClientID %>").focus();
            return alert("Please Enter Address");

        }
        if (document.getElementById("<%= txtContactNo.ClientID %>").value === "") {
            document.getElementById("<%= txtContactNo.ClientID %>").focus();
            return alert("Please Enter Contact Number");
        }

        var phone = document.getElementById("<%= txtContactNo.ClientID %>").value;
        if (isNaN(parseInt(phone))) {
            document.getElementById("<%= txtContactNo.ClientID %>").focus();
            return alert("The Contact number contains illegal characters");
        }
        if (!((phone.length >= 10) && (phone.length <= 15))) {
            document.getElementById("<%= txtContactNo.ClientID %>").focus();
            return alert("The Contact number is the wrong length");
        }


        if (document.getElementById("<%= txtPanNo.ClientID %>").value === "") {
            document.getElementById("<%= txtPanNo.ClientID %>").focus();
            return alert("Please Enter PAN");


        }
        var pan = document.getElementById("<%= txtPanNo.ClientID %>").value;
        if (isValidPAN(pan) === false) {
            return false;
        }

        if (!isValidPAN(document.getElementById("<%= txtPanNo.ClientID %>").value)) {
            document.getElementById("<%= txtPanNo.ClientID %>").value = "";
            document.getElementById("<%= txtPanNo.ClientID %>").focus();
            return false;
        }

        if (document.getElementById("<%= txtTin.ClientID %>").value === "") {
            document.getElementById("<%= txtTin.ClientID %>").focus();
            return alert("Please Enter TIN");

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
    }
</script>
<table align="center">
    <tr>
        <td class="rowseparator "></td>
    </tr>
    <tr>
        <td>
        <fieldset style="padding: 10px">
        <legend align="center" style="color:brown">Agency Details</legend>
        <asp:Panel runat="server">
        <table style="height: 150px; width: 100%;">
        <tr>
            <td align="left" style="width: 141px">
                Agency Name <span style="color: Red">*</span>
            </td>
            <td class="columnseparator"></td>
            <td align="left" style="width: 200px">
                <asp:TextBox ID="txtAgencyName" runat="server" MaxLength="35" CssClass="txtbox"></asp:TextBox>
            </td>
            <td class="columnseparator"></td>           
        </tr>
        <tr>
            <td class="rowseparator"></td>
        </tr>
        <tr>
            <td align="left" style="width: 200px">
                Contact Number <span style="color: Red">*</span>
            </td>
            <td class="columnseparator"></td>
            <td align="left" style="width: 200px">
                <asp:TextBox ID="txtContactNo" runat="server" MaxLength="15" CssClass="txtbox" onkeypress="return numeric_only(event)"></asp:TextBox>
            </td>
            <td class="columnseparator"></td>
            <td align="left" style="width: 146px">
                State<span style="color: Red">*</span>
            </td>
            <td class="columnseparator"></td>
            <td align="left">
                <asp:DropDownList ID="ddlDistrict" runat="server" Width="150px">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="rowseparator"></td>
        </tr>
        <tr>
            <td align="left" style="width: 141px">
                PAN No <span style="color: Red">*</span>
            </td>
            <td class="columnseparator"></td>
            <td align="left" style="width: 200px">
                <asp:TextBox ID="txtPanNo" runat="server" MaxLength="10" CssClass="txtbox"></asp:TextBox>
            </td>
            <td class="columnseparator"></td>
            <td align="left" style="width: 146px">
                Address <span style="color: Red">*</span>
            </td>
            <td class="columnseparator"></td>
            <td align="left">
                <asp:TextBox ID="txtAddress" runat="server" TextMode="MultiLine" onKeyUp="CheckLength(this,300)"
                             onChange="CheckLength(this,300)">
                </asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="rowseparator"></td>
        </tr>
        <tr>
            <td align="left" style="width: 141px">
                TIN <span style="color: Red">*</span>
            </td>
            <td class="columnseparator"></td>
            <td align="left" style="width: 200px">
                <asp:TextBox ID="txtTin" CssClass="txtbox" runat="server" MaxLength="11"></asp:TextBox>
            </td>
            <td class="columnseparator"></td>

        </tr>
        <tr>
            <td class="rowseparator"></td>
        </tr>
        <tr>
            <td style="width: 141px">
                &nbsp;
            </td>
            <td class="columnseparator"></td>
            <td style="width: 200px">
                &nbsp;
            </td>
            <td class="columnseparator"></td>

        </tr>
        <tr>
            <td class="rowseparator"></td>
        </tr>
        <tr>
            <td style="width: 141px">
                &nbsp;
            </td>
            <td class="columnseparator"></td>
            <td style="width: 200px">
                &nbsp;
            </td>
            <td class="columnseparator"></td>
            <td align="left" style="width: 146px">
                &nbsp;
            </td>
            <td class="columnseparator"></td>
            <td align="left">
                <asp:TextBox ID="txtEdit" runat="server" Visible="False"></asp:TextBox>
            </td>
        </tr>
        <tr>
        <td class="rowseparator"></td>
        <tr>
            <td align="center" colspan="8">
                <asp:Button ID="btnSaveAgencyDetails" runat="server" Width="55px" Height="20px" OnClick="btnSaveAgencyDetails_Click"
                            Text="Save" class="form-submit-button" ClientIDMode="static" EnableViewState="True" OnClientClick="if (!validationAgencyDetails()) return false;"/>
                &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button runat="server" Width="55px" Height="20px"
                            OnClick="btnResetAgencyDetails_Click" class="form-reset-button" Text="Reset"/>
            </td>
        </tr>
    </tr>
</table>
</asp:Panel>
</fieldset>
</td>
</tr>
<tr>
    <td class="rowseparator "></td>
</tr>
<tr>
    <td class="rowseparator "></td>
</tr>
<tr>
    <td>
        <fieldset style="padding: 10px">
            <table style="height: 60px; width: 100%;">
                <caption>
                    <tr>
                        <td class="rowseparator "></td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:GridView ID="grvAgencyDetails" runat="server" AutoGenerateColumns="False" CellPadding="3" CssClass="gridviewStyle" OnPageIndexChanging="grvAgencyDetails_PageIndexChanging"
                                          OnRowDeleting="grvAgencyDetails_RowDeleting" OnRowEditing="grvAgencyDetails_RowEditing" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px">
                                <RowStyle CssClass="rowStyleGrid" BorderStyle="Solid" BorderColor="brown" BorderWidth="1px" ForeColor="#000066"/>
                                <Columns>
                                    <asp:TemplateField HeaderText="Agency Id">
                                        <ItemTemplate>
                                            <asp:Label ID="lblId" runat="server" Text='<%#Eval("AgencyID") %>'/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Agency Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblName" runat="server" Text='<%#Eval("AgencyName") %>'/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Contact Number">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCntNum" runat="server" Text='<%#Eval("ContactNum") %>'/>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Address">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAddress" runat="server" Text='<%#Eval("Address") %>'/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Created Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDate" runat="server" Text='<%#Eval("CreatedDate", "{0:d}") %>'/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Edit">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" Text="Edit"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Delete">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" Text="Delete"></asp:LinkButton>
                                            <asp:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" ConfirmText="Are you sure you want to DELETE"
                                                                       TargetControlID="lnkDelete">
                                            </asp:ConfirmButtonExtender>
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
                        <td align="center">
                            &nbsp;
                        </td>
                    </tr>
                </caption>
            </table>
        </fieldset>
        <asp:HiddenField ID="hidAgencyId" runat="server"/>
    </td>
</tr>
<tr>
    <td class="rowseparator "></td>
</tr>
</table>
</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>