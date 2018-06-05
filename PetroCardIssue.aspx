<%@ Page Language="C#" MasterPageFile="~/temp.master" AutoEventWireup="true" CodeFile="PetroCardIssue.aspx.cs" Inherits="PetroCardIssue" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <script type="text/javascript">
        $(function() {
            $('#<%=txtValidityEndDate.ClientID%>,#<%=txtIssuedDate.ClientID%>').datepicker({
                dateFormat: 'mm/dd/yy',
                changeMonth: true,
                changeYear: true
            });
        });
        function isMandatory() {

            switch (document.getElementById("<%= txtPetroCardNumber.ClientID %>").value) {
            case '':
                alert("Please enter PetroCardNumber");
                document.getElementById("<%= txtPetroCardNumber.ClientID %>").focus();
                return false;
            }

            var agency = document.getElementById('<%= ddlAgency.ClientID %>');
            switch (agency.selectedIndex) {
            case 0:
                alert("Please select the Agency");
                agency.focus();
                return false;
            }

            var cardType = document.getElementById('<%= ddlCardType.ClientID %>');
            switch (cardType.selectedIndex) {
            case 0:
                alert("Please select the CardType");
                cardType.focus();
                return false;
            }


            switch (document.getElementById("<%= txtValidityEndDate.ClientID %>").value) {
            case '':
                alert("Please Select ValidityEndDate");
                document.getElementById("<%= txtValidityEndDate.ClientID %>").focus();
                return false;
            }
            switch (document.getElementById("<%= txtIssuedDate.ClientID %>").value) {
            case '':
                alert("Please Select IssuedDate");
                document.getElementById("<%= txtIssuedDate.ClientID %>").focus();
                return false;
            }
            var listFe = document.getElementById('<%= dd_listFe.ClientID %>');
            switch (listFe.selectedIndex) {
            case 0:
                alert("Please select the FE");
                listFe.focus();
                return false;
            }


            var validityEndDate = document.getElementById('<%= txtValidityEndDate.ClientID %>');

            var issuedDate = document.getElementById('<%= txtIssuedDate.ClientID %>');

            if (trim(validityEndDate.value) !== "" && !isValidDate(validityEndDate.value)) {
                alert("Enter the Valid Date");
                validityEndDate.focus();
                return false;
            }


            if (trim(issuedDate.value) !== "" && !isValidDate(issuedDate.value)) {
                alert("Enter the Valid Date");
                issuedDate.focus();
                return false;
            }
            var now = new Date();
            if (Date.parse(validityEndDate.value) < Date.parse(now)) {
                alert("ValidityEndDate should be greater than Current Date");
                validityEndDate.focus();
                return false;
            }

            now = new Date();
            if (Date.parse(issuedDate.value) > Date.parse(now)) {
                alert("Issued Date should not be greater than Current Date");
                issuedDate.focus();
                return false;
            }

            var id = document.getElementById('<%= ddlVehicles.ClientID %>');

            var inputs = id.getElementsByTagName('input');
            var i;
            for (i = 0; i < inputs.length; i++) {
                if (inputs[i].type !== 'text')
                    continue;
                if (inputs[i].value !== "" && inputs[i].value != null && inputs[i].value === "--Select--") {
                    alert('Select the Vehicle');
                    return false;
                }

                break;
            }


            return true;
        }


    </script>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>

            <legend align="center" style="color: brown">Petro Card Issue</legend>
            <br/>
            <table align="center">
                <tr>
                    <td >
                        <asp:Label ID="lblDistrict" runat="server" Text="Select District" Visible="false"></asp:Label>
                    </td>
                    <td >
                        <asp:DropDownList ID="ddlDistricts" runat="server" Width="150px" AutoPostBack="True"
                                          Visible="false" CssClass="search_3" OnSelectedIndexChanged="ddlDistricts_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td >
                        <asp:TextBox ID="txtEdit" runat="server" width="150px" CssClass="search_3" Visible="False"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td >
                        Petro Card Number<span style="color: Red">*</span>
                    </td>
                    <td >
                        <asp:TextBox ID="txtPetroCardNumber" CssClass="search_3" runat="server" MaxLength="16"></asp:TextBox>
                    </td>

                </tr>
                <tr>
                    <td >
                        Agency<span style="color: Red">*</span>
                    </td>
                    <td >
                        <asp:DropDownList ID="ddlAgency" CssClass="search_3" runat="server" Width="153px" AutoPostBack="True"
                                          OnSelectedIndexChanged="ddlAgency_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td >
                        Card Type<span style="color: Red">*</span>
                    </td>
                    <td >
                        <asp:DropDownList ID="ddlCardType" CssClass="search_3" runat="server" Width="153px" AutoPostBack="True">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td >
                        Validity End Date<span style="color: Red">*</span>
                    </td>
                    <td >
                        <asp:TextBox ID="txtValidityEndDate" runat="server" CssClass="search_3" oncut="return false;" onpaste="return false;"
                                     oncopy="return false;" onkeypress="return false">
                        </asp:TextBox>
                    </td>

                </tr>
                <tr>
                    <td >
                        Issued To FE<span style="color: Red">*</span>
                    </td>
                    <td >
                        <asp:DropDownList ID="dd_listFe" runat="server" CssClass="search_3" AutoPostBack="True"
                                          onselectedindexchanged="dd_listFe_SelectedIndexChanged">
                        </asp:DropDownList>

                    </td>

                </tr>
                <tr>
                    <td >
                        Issued Date<span style="color: Red">*</span>
                    </td>
                    <td >
                        <asp:TextBox ID="txtIssuedDate" CssClass="search_3" runat="server" MaxLength="15" onkeypress="return false"
                                     oncut="return false;" onpaste="return false;" oncopy="return false;">
                        </asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td >
                        Issued To Vehicle<span style="color: Red">*</span>
                    </td>
                    <td style="height: 19px; width: 101px;">
                        <cc1:ComboBox AutoCompleteMode="Append" DropDownStyle="DropDownList" ID="ddlVehicles" runat="server" AutoPostBack="True"
                                      Enabled="False">
                        </cc1:ComboBox>


                    </td>
                </tr>
                <tr>
                    <td >
                        Issued To District<span style="color: Red">*</span>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlFeuserDistrict" CssClass="search_3" runat="server" AutoPostBack="True" >
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td >
                    <asp:Button ID="btSave" runat="server" Text="Save" CssClass="form-submit-button" OnClick="btSave_Click"/>
                    <td >
                        <asp:Button ID="btReset" runat="server" Text="Reset" CssClass="form-reset-button" OnClick="btReset_Click"/>
                    </td>
                </tr>
            </table>
            <br/>
            <table align="center">
                <tr>
                    <td>
                        <asp:GridView ID="gvPetroCardIssue" runat="server" Width="1000px" AllowPaging="True"
                                      OnPageIndexChanging="gvPetroCardIssue_PageIndexChanging" PageSize="5" CellPadding="3" wrap="nowrap" CssClass="gridview" HorizontalAlign="Justify"
                                      OnRowEditing="gvPetroCardIssue_RowEditing" OnRowDeleting="gvPetroCardIssue_RowDeleting"
                                      AutoGenerateColumns="False" OnRowCommand="gvPetroCardIssue_RowCommand" EmptyDataText="No Records Found" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px">
                            <RowStyle CssClass="rowStyleGrid" ForeColor="#000066"/>
                            <Columns>
                                <asp:BoundField HeaderText="District" DataField="District"/>
                                <asp:BoundField HeaderText="Card" DataField="CardNum"/>
                                <asp:BoundField HeaderText="Vehicle" DataField="Vehicle"/>
                                <asp:BoundField HeaderText="CardType" DataField="CardType"/>
                                <asp:BoundField HeaderText="Validity" DataField="Validity"/>
                                <asp:BoundField HeaderText="IssuedFE" DataField="IssueToFE"/>
                                <asp:BoundField HeaderText="Date" DataField="IssuedDate"/>
                                <asp:BoundField HeaderText="IssuedDistrict" DataField="IssuedToDistrict"/>
                                <asp:BoundField HeaderText="IssuedVehicle" DataField="IssuedToVehicle"/>
                                <asp:TemplateField HeaderText="Edit">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkEdit" runat="server" Text="Edit" CommandName="Edit" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "IssueID") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Deactivate">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnDeactivate" runat="server" Text="Deactivate" CommandName="Delete"
                                                        CommandArgument='<%#DataBinder.Eval(Container.DataItem, "IssueID") %>'>
                                        </asp:LinkButton>
                                        <cc1:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" TargetControlID="lnDeactivate"
                                                                   ConfirmText="Are you sure you want to Deactivate?">
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
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>