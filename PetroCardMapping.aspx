<%@ Page Language="C#" MasterPageFile="~/temp.master" AutoEventWireup="true" CodeFile="PetroCardMapping.aspx.cs" Inherits="PetroCardMapping" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <script type="text/javascript">
        $(function() {
            $('#<%= txtIssDate.ClientID %>').datepicker({
                dateFormat: 'mm/dd/yy',
                changeMonth: true,
                changeYear: true
            });
        });

        function isMandatory(evt) {
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
            switch (document.getElementById("<%= ddlPetroCardNumber.ClientID %>").selectedIndex) {
            case 0:
                alert("Please Select PetroCardNumber");
                document.getElementById("<%= ddlPetroCardNumber.ClientID %>").focus();
                return false;
            }
            switch (document.getElementById("<%= txtIssDate.ClientID %>").value) {
            case '':
                alert("Please enter Issued Date");
                document.getElementById("<%= txtIssDate.ClientID %>").focus();
                return false;
            }
            var issuedDate = document.getElementById('<%= txtIssDate.ClientID %>');
            var now = new Date();
            if (Date.parse(issuedDate.value) > Date.parse(now)) {
                alert("Issued Date should not be greater than Current Date");
                issuedDate.focus();
                return false;
            }
            return true;
        }
    </script>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <table align="center">
                <tr>
                    <td class="rowseparator">
                    </td>
                </tr>
                <tr>
                    <td>
                        <fieldset style="padding: 10px">
                            <legend align="center" style="color: brown">Petro Card Mapping</legend>
                            <table align="center">
                                <tr>
                                    <td class="rowseparator">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 100px">
                                        <asp:GridView ID="gvPetroCardMapping" runat="server" Width="700px" CellPadding="3" CssClass="gridview" OnPageIndexChanging="gvPetroCardMapping_PageIndexChanging"
                                                      OnRowEditing="gvPetroCardMapping_RowEditing" AutoGenerateColumns="False" OnRowCommand="gvPetroCardMapping_RowCommand" EmptyDataText="No Records Found" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px">
                                            <RowStyle CssClass="rowStyleGrid" ForeColor="#000066"/>
                                            <Columns>
                                                <asp:BoundField HeaderText="Vehicle" DataField="Vehicle"/>
                                                <asp:BoundField HeaderText="CardNo" DataField="CardNo"/>
                                                <asp:BoundField HeaderText="IssuedDate" DataField="IssuedToAmbyDate"/>
                                                <asp:BoundField HeaderText="Agency" DataField="Agency"/>
                                                <asp:BoundField HeaderText="Card" DataField="Card"/>
                                                <asp:BoundField HeaderText="Validity" DataField="Validity"/>
                                                <asp:TemplateField HeaderText="Edit">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkEdit" runat="server" Text="Edit" CommandName="Edit" CommandArgument='<%#DataBinder.Eval(Container.DataItem,"MapID") %>'></asp:LinkButton>
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
                        </fieldset>
                    </td>
                </tr>
                <tr>
                    <td class="rowseparator">
                    </td>
                </tr>
                <tr>
                    <td>
                        <fieldset style="padding: 10px">
                            <table style="width: 463px" align="center">
                                <tr>
                                    <td align="left" style="height: 19px; width: 155px;">
                                        Vehicle Number<span style="color: Red">*</span>
                                    </td>
                                    <td style="height: 19px; width: 109px;">
                                        <cc1:ComboBox AutoCompleteMode="Append" ID="ddlVehicleNumber" runat="server" AutoPostBack="True" DropDownStyle="DropDownList" Width="154px"
                                                      OnSelectedIndexChanged="ddlVehicleNumber_SelectedIndexChanged">
                                        </cc1:ComboBox>
                                    </td>
                                    <td style="height: 19px; width: 100px;">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" style="width: 155px">
                                        Petro Card Number<span style="color: Red">*</span>
                                    </td>
                                    <td style="width: 109px">
                                        <asp:DropDownList ID="ddlPetroCardNumber" CssClass="search_3" runat="server" AutoPostBack="True" Width="154px"
                                                          OnSelectedIndexChanged="ddlPetroCardNumber_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 100px">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" style="width: 155px">
                                        Card Validity<span style="color: Red">*</span>
                                    </td>
                                    <td style="width: 109px">
                                        <asp:TextBox ID="txtCardValidity" CssClass="search_3" runat="server" ReadOnly="True"></asp:TextBox>
                                    </td>
                                    <td style="width: 100px">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" style="width: 155px">
                                        Agency<span style="color: Red">*</span>
                                    </td>
                                    <td style="width: 109px">
                                        <asp:TextBox ID="txtAgency" CssClass="search_3" runat="server" ReadOnly="True"></asp:TextBox>
                                    </td>
                                    <td style="width: 100px">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" style="width: 155px">
                                        Card Type<span style="color: Red">*</span>
                                    </td>
                                    <td style="width: 109px">
                                        <asp:TextBox ID="txtCardType" CssClass="search_3" runat="server" ReadOnly="True"></asp:TextBox>
                                    </td>
                                    <td style="width: 100px">
                                        <asp:TextBox ID="txtEdit" runat="server" Visible="False"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" style="width: 155px">
                                        Issue To Ambulance Date<span style="color: Red">*</span>
                                    </td>
                                    <td align="left" style="width: 109px">
                                        <asp:TextBox ID="txtIssDate" CssClass="search_3" runat="server" oncut="return false;" onpaste="return false;"
                                                     oncopy="return false;" onkeypress="return false" MaxLength="15">
                                        </asp:TextBox>
                                    </td>
                                    <td wrap="nowrap" style="width: 51px">

                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" style="width: 155px">
                                        <asp:RadioButton ID="Deactivate" CssClass="form-submit-button" runat="server" Text="Deactivate" OnCheckedChanged="Deactivate_CheckedChanged"
                                                         AutoPostBack="true" Visible="false" GroupName="kk"/>
                                    </td>
                                    <td style="width: 109px">
                                        <asp:RadioButton ID="TransfertoNewVehicle" CssClass="form-submit-button" runat="server" OnCheckedChanged="TransfertoNewVehicle_CheckedChanged"
                                                         Text="Transfer" AutoPostBack="true" Visible="false" GroupName="kk"/>
                                    </td>
                                    <td style="width: 100px">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" style="height: 19px; width: 155px;">
                                        <asp:Label ID="lbReason" runat="server" Text="Reason" Visible="False"></asp:Label>
                                    </td>
                                    <td style="height: 19px; width: 109px;">
                                        <asp:DropDownList ID="ddlReason" CssClass="search_3" runat="server" AutoPostBack="True" Width="154px"
                                                          Visible="false" OnSelectedIndexChanged="ddlReason_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="height: 19px; width: 100px;">
                                        <asp:TextBox ID="txtRemarks" CssClass="search_3" runat="server" Visible="False" onkeypress="return OnlyAlphabets(event);"
                                                     MaxLength="15">
                                        </asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" style="width: 155px">
                                        <asp:Label ID="lbTransfer" runat="server" Text="Transfer To New Vehicle" Visible="False"></asp:Label>
                                    </td>
                                    <td style="width: 109px">
                                        <asp:DropDownList ID="ddlNewVehicleNumber" runat="server" AutoPostBack="True" Width="154px"
                                                          Visible="False" OnSelectedIndexChanged="ddlNewVehicleNumber_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 100px">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" style="width: 155px">
                                        <asp:Label ID="lbPetroCard" runat="server" Text="Petro Card Mapped" Visible="False"></asp:Label>
                                    </td>
                                    <td style="width: 109px">
                                        <asp:TextBox ID="txtMappedCardNum" runat="server" Visible="False" Enabled="False"></asp:TextBox>
                                    </td>
                                    <td style="width: 100px">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="rowseparator">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" style="height: 19px; width: 100px;">
                                        <asp:Button ID="Save" runat="server" Text="Save" CssClass="form-submit-button" style="margin-top: -10px" OnClientClick="return isMandatory();"
                                                    OnClick="Save_Click"/>&nbsp;
                                    </td>
                                    <td align="left" style="height: 19px; width: 99px;">
                                        &nbsp;<asp:Button ID="Reset" CssClass="form-submit-button" runat="server" style="margin-top: -10px" Text="Reset" OnClick="Reset_Click"/>
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