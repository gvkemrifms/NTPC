<%@ Page Language="C#" MasterPageFile="~/temp.master" AutoEventWireup="true" CodeFile="TyresDetails.aspx.cs" Inherits="TyresDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        function validation() {
            switch (document.getElementById("<%= txtTyreItemCode.ClientID %>").value) {
            case '':
                alert("Please Enter Tyre Item Code");
                document.getElementById("<%= txtTyreItemCode.ClientID %>").focus();
                return false;
            }
            switch (document.getElementById("<%= txtTyreNumber.ClientID %>").value) {
            case '':
                alert("Please Enter Tyre Number");
                document.getElementById("<%= txtTyreNumber.ClientID %>").focus();
                return false;
            }
            switch (document.getElementById("<%= txtTyreMake.ClientID %>").value) {
            case '':
                alert("Please Enter Tyre Make");
                document.getElementById("<%= txtTyreMake.ClientID %>").focus();
                return false;
            }
            switch (document.getElementById("<%= txtTyreModel.ClientID %>").value) {
            case '':
                alert("Please Enter Tyre Model");
                document.getElementById("<%= txtTyreModel.ClientID %>").focus();
                return false;
            }

            switch (document.getElementById("<%= txtTyreSize.ClientID %>").value) {
            case '':
                alert("Please Enter Tyre Size");
                document.getElementById("<%= txtTyreSize.ClientID %>").focus();
                return false;
            }

            return true;
        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <legend align="center" style="color:brown">Tyres Details </legend>

            <asp:Panel ID="pnltyredetails" runat="server">
                <table align="center">
                    <tr>
                        <td align="left">
                            Tyre Item Code <span style="color: Red">*</span>
                        </td>
                        <td class="columnseparator"></td>
                        <td>
                            <asp:TextBox ID="txtTyreItemCode" CssClass="search_3" runat="server" MaxLength="15"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="rowseparator"></td>
                    </tr>
                    <tr>
                        <td align="left">
                            Tyre Number <span style="color: Red">*</span>
                        </td>
                        <td class="columnseparator"></td>
                        <td>
                            <asp:TextBox ID="txtTyreNumber" CssClass="search_3" runat="server" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="rowseparator"></td>
                    </tr>
                    <tr>
                        <td align="left">
                            Make <span style="color: Red">*</span>
                        </td>
                        <td class="columnseparator"></td>
                        <td>
                            <asp:TextBox ID="txtTyreMake" CssClass="search_3" runat="server" MaxLength="15"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="rowseparator"></td>
                    </tr>
                    <tr>
                        <td align="left">
                            &nbsp;Model <span style="color: Red">*</span>
                        </td>
                        <td class="columnseparator"></td>
                        <td>
                            <asp:TextBox ID="txtTyreModel" CssClass="search_3" runat="server" MaxLength="15"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="rowseparator"></td>
                    </tr>
                    <tr>
                        <td align="left">
                            &nbsp;Size <span style="color: Red">*</span>
                        </td>
                        <td class="columnseparator"></td>
                        <td>
                            <asp:TextBox ID="txtTyreSize" CssClass="search_3" runat="server" MaxLength="15"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="rowseparator"></td>
                    </tr>
                    <tr>
                        <td colspan="3" align="center">
                            <asp:Button ID="btnTyresDetailsSave" CssClass="form-submit-button" runat="server" Text="Save" OnClick="btnTyresDetailsSave_Click"
                                        OnClientClick="return validation();"/>
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button CssClass="form-reset-button" runat="server" Text="Reset" OnClick="btnTyresDetailsReset_Click"/>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </fieldset>
        </td>
        </tr>
        <tr>
            <td class="rowseparator" style="width: 450px"></td>
        </tr>
        <br/>
        <tr>
            <td style="width: 450px">
                <fieldset style="padding: 10px">
                    <asp:GridView ID="grvTyresDetails" runat="server" align="center" AllowPaging="True"
                                  PageSize="5" AutoGenerateColumns="False" BorderColor="#CCCCCC" BorderWidth="1px"
                                  CellPadding="3" OnPageIndexChanging="grvTyresDetails_PageIndexChanging"
                                  OnRowEditing="grvTyresDetails_RowEditing" BackColor="White" BorderStyle="None">
                        <RowStyle CssClass="rowStyleGrid" ForeColor="#000066"/>
                        <Columns>
                            <asp:TemplateField HeaderText="Tyre Id">
                                <ItemTemplate>
                                    <asp:Label ID="lblId" runat="server" Text='<%#Eval("Tyre_Id") %>'/>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tyre Item Code">
                                <ItemTemplate>
                                    <asp:Label ID="lblTyreItemCode" runat="server" Text='<%#Eval("Tyre_Item_Code") %>'/>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tyre Number">
                                <ItemTemplate>
                                    <asp:Label ID="lblTyreNumber" runat="server" Text='<%#Eval("TyreNumber") %>'/>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Make">
                                <ItemTemplate>
                                    <asp:Label ID="lblMake" runat="server" Text='<%#Eval("Make") %>'/>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Model">
                                <ItemTemplate>
                                    <asp:Label ID="lblModel" runat="server" Text='<%#Eval("Model") %>'/>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Size">
                                <ItemTemplate>
                                    <asp:Label ID="lblSize" runat="server" Text='<%#Eval("Size") %>'/>
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
                        <SortedAscendingCellStyle BackColor="#F1F1F1" />
                        <SortedAscendingHeaderStyle BackColor="#007DBB" />
                        <SortedDescendingCellStyle BackColor="#CAC9C9" />
                        <SortedDescendingHeaderStyle BackColor="#00547E" />
                    </asp:GridView>
                </fieldset>
                <asp:HiddenField ID="hidTyresId" runat="server"/>
            </td>
        </tr>
        <tr>
            <td class="rowseparator" style="width: 450px"></td>
        </tr>
        </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>