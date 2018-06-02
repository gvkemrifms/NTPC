<%@ Page Language="C#" MasterPageFile="~/temp.master" AutoEventWireup="true" CodeFile="OffroadPhysicalBills.aspx.cs" Inherits="OffroadPhysicalBills" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<asp:UpdatePanel runat="server">
    <ContentTemplate>
        <script type="text/javascript">
            function dateselect(ev) {
                var calendarBehavior1 = window.$find("cc1");
                var d = calendarBehavior1._selectedDate;
                var now = new Date();
                calendarBehavior1.get_element().value = d.format("MM/dd/yyyy");
            }

            function validation2() {
                var fldDocketNo = document.getElementById('<%= txtDocketNo.ClientID %>');
                var fldReceiptDate = document.getElementById('<%= txtReceiptDate.ClientID %>');
                var fldCourierName = document.getElementById('<%= txtCourierName.ClientID %>');
                var fldBillAm = document.getElementById('<%= txtBillAmount.ClientID %>');
                if (!RequiredValidation(fldReceiptDate, "Receipt Date cannot be left blank"))
                    return false;
                if (!RequiredValidation(fldBillAm, "Bill Amount cannot be left blank"))
                    return false;
                if (!RequiredValidation(fldCourierName, "Courier Name cannot be left blank"))
                    return false;

                if (!RequiredValidation(fldDocketNo, "Docket Number cannot be left blank"))
                    return false;
                return true;
            }


            function validation() {

                var fldDistrict = document.getElementById('<%= ddlDistricts.ClientID %>');
                var fldDocketNo = document.getElementById('<%= txtDocketNo.ClientID %>');
                var fldVehicleno = document.getElementById('<%= ddlVehicleNo.ClientID %>').control._textBoxControl
                    .value;
                var fldBillNo = document.getElementById('<%= ddlBillNo.ClientID %>');
                var fldReceiptDate = document.getElementById('<%= txtReceiptDate.ClientID %>');
                var fldCourierName = document.getElementById('<%= txtCourierName.ClientID %>');
                var now = new Date();

                if (fldDistrict.selectedIndex === 0) {
                    alert("Please Select District");
                    return false;
                }
                if (fldVehicleno === '--Select--') {
                    alert("Please select Vehicle");
                    return false;
                }
                if (!RequiredValidation(fldReceiptDate, "Receipt Date cannot be left blank"))
                    return false;
                if (!RequiredValidation(fldCourierName, "Courier Name cannot be left blank"))
                    return false;

                if (!RequiredValidation(fldDocketNo, "Docket Number cannot be left blank"))
                    return false;

                if (fldBillNo.selectedIndex === 0) {
                    alert("Please select BillNo");
                    return false;
                }
                if (Date.parse(fldReceiptDate.value) > Date.parse(now)) {
                    alert("Receipts Date should not be greater than Current Date");
                    fldReceiptDate.focus();
                    return false;
                }

                return true;
            }


        </script>
        <legend style="color: brown">
            Off Road Physical Bills<br/>
        </legend>
        <table align="center">
            <tr>
                <td>
                    District<span style="color: red">*</span>
                </td>
                <td>
                    <asp:DropDownList ID="ddlDistricts" Width="150px" runat="server" CssClass="search_3" OnSelectedIndexChanged="ddlDistricts_SelectedIndexChanged"
                                      AutoPostBack="true">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    Vehicle No<span style="color: red">*</span>
                </td>
                <td>
                <cc1:ComboBox AutoCompleteMode="Append" ID="ddlVehicleNo" runat="server" AutoPostBack="true"
                              OnSelectedIndexChanged="ddlVehicleNo_SelectedIndexChanged" DropDownStyle="DropDownList">
                </cc1:ComboBox>

            </tr>
            <tr>
                <td>
                    Receipt Date<span style="color: red">*</span>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtReceiptDate" CssClass="search_3" Width="150px" onkeypress="false;"/>
                    <cc1:CalendarExtender runat="server" TargetControlID="txtReceiptDate" Format="MM/dd/yyyy"
                                          OnClientDateSelectionChanged="dateselect">
                    </cc1:CalendarExtender>
                </td>
            </tr>
            <tr>
                <td>
                    Courier Name<span style="color: red">*</span>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtCourierName" CssClass="search_3" width="150px" onkeypress="return alpha_only_withspace(event);"/>
                </td>
            </tr>

            <tr>
                <td>
                    Docket No<span style="color: red">*</span>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtDocketNo" CssClass="search_3" onkeypress="return numeric_only(event);"/>
                </td>
            </tr>
            <tr>
                <td>
                    Bill No<span style="color: red">*</span>
                </td>
                <td>
                    <asp:DropDownList runat="server" CssClass="search_3" ID="ddlBillNo" Width="150px"
                                      AutoPostBack="True"
                                      onselectedindexchanged="ddlBillNo_SelectedIndexChanged"/>
                </td>
            </tr>
            <tr>
                <td>
                    BreakDown ID
                </td>
                <td>
                    <asp:Label runat="server" ID="lblBreakdwn" style="margin-left: 10px" ForeColor="red"/>
                </td>
            </tr>
            <tr>

                <td>
                    Bill Amount
                </td>
                <td>
                    <asp:TextBox ID="txtBillAmount" CssClass="search_3" runat="server"
                                 onkeypress="return numeric_only(event);"/>
                </td>
            </tr>
            <tr>
            <td>
                Down Time
            </td>
            <td>
                <asp:TextBox ID="txtDownTime" CssClass="search_3" runat="server" Enabled="false"/>
            </td>
            <tr>
                <td>
                    Up Time<span style="color: red">*</span>
                </td>

                <td>
                    <asp:TextBox ID="txtUpTime" CssClass="search_3" runat="server" Enabled="false"/>
                </td>
            </tr>

        </table>
        <br/>
        <table align="center">
            <tr>
                <td>

                    <asp:Button runat="server" ID="btnSave" Text="Save"
                                onclick="btnSave_Click" CssClass="form-submit-button" OnClientClick="if (!validation()) return false;"/>

                    <asp:HiddenField ID="HiddenField1" runat="server"/>
                    <asp:Button runat="server" ID="btnUpdate" Visible="false" Text="Update"
                                onclick="btnUpdate_Click" CssClass="form-submit-button" OnClientClick="if (!validation2()) return false;"/>
                    <asp:Button runat="server" ID="btnReset" Text="Reset" CssClass="form-submit-button" onclick="btnReset_Click"/>

                </td>
            </tr>
        </table>
        <br/>
        <div align="center">
            <asp:GridView ID="gvVehiclePhysicalBillDetails" runat="server" EmptyDataText="No Records Found"
                          AllowSorting="True" AutoGenerateColumns="False" CssClass="gridviewStyle"
                          CellPadding="3" Width="650px" AllowPaging="True"
                          EnableSortingAndPagingCallbacks="True"
                          onrowcommand="gvVehiclePhysicalBillDetails_RowCommand" onpageindexchanging="gvVehiclePhysicalBillDetails_PageIndexChanging" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px">
                <RowStyle CssClass="rowStyleGrid" ForeColor="#000066"/>
                <Columns>
                    <asp:TemplateField HeaderText="District">
                        <ItemTemplate>
                            <asp:Label ID="lblDistrict" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "District") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Break Down">
                        <ItemTemplate>
                            <asp:Label ID="lblBrkdwn" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "ID") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Vehicle No">
                        <ItemTemplate>
                            <asp:Label ID="lblVehicle_No" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Vechicleno") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Bill No">
                        <ItemTemplate>
                            <asp:Label ID="lblBillNo" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "BillNo") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Bill Amount">
                        <ItemTemplate>
                            <asp:Label ID="lblBillAmount" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Amount") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Down Time">
                        <ItemTemplate>
                            <asp:Label ID="lblDownTime" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "downtime") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Up Time">
                        <ItemTemplate>
                            <asp:Label ID="lblUptime" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Uptime") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Receipt Date">
                        <ItemTemplate>
                            <asp:Label ID="lblReceiptDate" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "ReceiptDate") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Courier Name">
                        <ItemTemplate>
                            <asp:Label ID="lblCourier_Name" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Courier_Name") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Rejection Reason">
                        <ItemTemplate>
                            <asp:Label ID="lblReject" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "ReasonforReject") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Docket No">
                        <ItemTemplate>
                            <asp:Label ID="lblDocketNo" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "DocketNo") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Edit">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkEdit" runat="server" CommandName="VehMainEdit" CommandArgument=" <%# Container.DataItemIndex %>"
                                            Text="Edit">
                            </asp:LinkButton>

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
        </div>
    </div>
    </ContentTemplate>
</asp:UpdatePanel>
</asp:Content>