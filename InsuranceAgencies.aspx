<%@ Page Language="C#" MasterPageFile="~/temp.master" AutoEventWireup="true" CodeFile="InsuranceAgencies.aspx.cs" Inherits="InsuranceAgencies" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <script type="text/javascript">
        function validationInsuranceDetails() {
            switch (document.getElementById("<%= txtInsuranceAgency.ClientID %>").value) {
            case '':
                alert("Please Enter Insurance Agency Name");
                document.getElementById("<%= txtInsuranceAgency.ClientID %>").focus();
                return false;
            }
            switch (document.getElementById("<%= txtAddress.ClientID %>").value) {
            case '':
                alert("Please Enter Address");
                document.getElementById("<%= txtAddress.ClientID %>").focus();
                return false;
            }
            switch (document.getElementById("<%= txtContactPerson.ClientID %>").value) {
            case '':
                alert("Please Enter ContactPerson Name");
                document.getElementById("<%= txtContactPerson.ClientID %>").focus();
                return false;
            }
            switch (document.getElementById("<%= txtContactNo.ClientID %>").value) {
            case '':
                alert("Please Enter  Contact Number");
                document.getElementById("<%= txtContactNo.ClientID %>").focus();
                return false;
            }
            var phone = document.getElementById("<%= txtContactNo.ClientID %>").value;
            if (isNaN(parseInt(phone))) {
                alert("The phone number contains illegal characters");
                return false;
            }
            if (!((phone.length >= 10) && (phone.length <= 15))) {
                alert("The phone number is the wrong length");
                return false;
            }
            return true;
        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table align="center">
                <tr>
                    <td style="height: 200px">
                        <fieldset style="padding: 10px;">
                            <legend align="center" style="color: brown">Insurance Agency</legend>
                            <table id="table2" width="91%" align="center">
                                <tr>
                                    <td>
                                        <table align="center">
                                            <tr>
                                                <td style="width: 150px" align="left">
                                                    Insurance Agency <span style="color: Red">*</span>
                                                </td>
                                                <td class="columnseparator">
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtInsuranceAgency" runat="server" CssClass="search_3" Width="150px"
                                                                 MaxLength="35">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="rowseparator">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 150px" align="left">
                                                    Address <span style="color: Red">*</span>
                                                </td>
                                                <td class="columnseparator">
                                                </td>
                                                <td style="height: 23px">
                                                    <asp:TextBox ID="txtAddress" runat="server" CssClass="search_3" TextMode="MultiLine"
                                                                 onKeyUp="CheckLength(this,300)" onChange="CheckLength(this,300)">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="rowseparator">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 150px" align="left">
                                                    Contact Person <span style="color: Red">*</span>
                                                </td>
                                                <td class="columnseparator">
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtContactPerson" runat="server" CssClass="search_3" MaxLength="35"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="rowseparator">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 150px" align="left">
                                                    Contact No <span style="color: Red">*</span>
                                                </td>
                                                <td class="columnseparator">
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtContactNo" runat="server" CssClass="search_3" onkeypress="return numeric_only(event)" MaxLength="15"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="rowseparator">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3" style="height: 41px">
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:Button ID="btnInsuranceUpdate" Width="55px" runat="server" CssClass="form-submit-button"
                                                                Text="Insert" OnClientClick="return validationInsuranceDetails();" OnClick="btnInsuranceUpdate_Click">
                                                    </asp:Button>
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:Button Width="55px" runat="server"
                                                                CssClass="form-submit-button" Text="Reset" CausesValidation="false" OnClick="btnInsuranceReset_Click">
                                                    </asp:Button>
                                                    <input type="hidden" runat="server"/>
                                                </td>
                                            </tr>
                                        </table>
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
                        <fieldset style="padding: 10px;">
                            <asp:GridView ID="grvInsuranceAgencyDetails" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                          BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px"
                                          CellPadding="3" PageSize="5" Width="548px" OnRowCommand="grvInsuranceAgencyDetails_RowCommand"
                                          OnPageIndexChanging="grvInsuranceAgencyDetails_PageIndexChanging">
                                <Columns>
                                    <asp:TemplateField HeaderText="Id">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="InsuranceAgency">
                                        <ItemTemplate>
                                            <asp:Label ID="lblInsuranceAgency" runat="server" Text='<%#Eval("InsuranceAgency") %>'/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ContactPerson">
                                        <ItemTemplate>
                                            <asp:Label ID="lblContactPerson" runat="server" Text='<%#Eval("ContactPerson") %>'/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ContactNumber">
                                        <ItemTemplate>
                                            <asp:Label ID="lblContactNumber" runat="server" Text='<%#Eval("ContactNumber") %>'/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Edit">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkbtnEdit" runat="server" CommandName="EditAgency" Text="Edit"
                                                            CommandArgument='<%#Eval("InsuranceId") %>'/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Delete">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkbtnDelete" runat="server" CommandName="DeleteAgency" Text="Delete"
                                                            CommandArgument='<%#Eval("InsuranceId") %>'/>
                                            <asp:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" ConfirmText="Are you Sure Want to delete"
                                                                       TargetControlID="lnkbtnDelete">
                                            </asp:ConfirmButtonExtender>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <FooterStyle BackColor="White" ForeColor="#000066"/>
                                <RowStyle ForeColor="#000066"/>
                                <PagerStyle ForeColor="#000066" HorizontalAlign="Left" BackColor="White"/>
                                <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White"/>
                                <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White"/>
                                <SortedAscendingCellStyle BackColor="#F1F1F1"/>
                                <SortedAscendingHeaderStyle BackColor="#007DBB"/>
                                <SortedDescendingCellStyle BackColor="#CAC9C9"/>
                                <SortedDescendingHeaderStyle BackColor="#00547E"/>
                            </asp:GridView>
                        </fieldset>
                    </td>
                </tr>
            </table>
            <br/>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>