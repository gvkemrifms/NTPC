<%@ Page Language="C#" MasterPageFile="~/temp.master" AutoEventWireup="true" CodeFile="NonOffRoadPhysicalBills.aspx.cs" Inherits="NonOffRoadPhysicalBills" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <script>
                function pageLoad() {
                    $('#<%= ddlDistricts.ClientID %>').select2({
                        disable_search_threshold: 5,
                        search_contains: true,
                        minimumResultsForSearch: 20,
                        placeholder: "Select an option"
                    });
                    $('#<%= ddlVehicleno.ClientID %>').select2({
                        disable_search_threshold: 5,
                        search_contains: true,
                        minimumResultsForSearch: 20,
                        placeholder: "Select an option"
                    });
                }

                function validation() {
                    var fldDistrict = document.getElementById('<%= ddlDistricts.ClientID %>');
                    var fldDocketNo = document.getElementById('<%= txtDocketNo.ClientID %>');
                    var fldVehicleno = document.getElementById('<%= ddlVehicleno.ClientID %>');
                    var fldBillNo = document.getElementById('<%= ddlBillNo.ClientID %>');
                    var fldReceiptDate = document.getElementById('<%= txtReceiptDate.ClientID %>');
                    var fldCourierName = document.getElementById('<%= txtCourierName.ClientID %>');
                    var txtToDate = $('#<%= txtReceiptDate.ClientID %>').val();
                    var toDate = (txtToDate).replace(/\D/g, '/');
                    var ordToDate = new Date(toDate);
                    var currentDate = new Date();
                    if (fldDistrict)
                        if (fldDistrict.selectedIndex === 0) {
                            alert("Please Select District");
                            fldDistrict.focus();
                            return false;
                        }
                    if (fldVehicleno)
                        if (fldVehicleno.selectedIndex === 0) {
                            alert("Please select Vehicle");
                            fldVehicleno.focus();
                            return false;
                        }
                    if (fldBillNo)
                        if (fldBillNo.selectedIndex === 0) {
                            alert("Please select Bill");
                            fldBillNo.focus();
                            return false;
                        }
                    if (fldReceiptDate.value === "") {
                        alert("Receipts Date is manadatory");
                        return false;
                    }
                    if (fldCourierName.value === "") {
                        alert("Courier Name is manadatory");
                        return false;
                    }
                    if (fldDocketNo.value === "") {
                        alert("Docket No is manadatory");
                        return false;
                    }

                    if (ordToDate > currentDate) {
                        return alert("Receipts Date should not be greater than Current Date");
                    }

                    return true;
                }

            </script>

            <legend align="center">
                Non Off Road Physical Bills<br/>
            </legend>
            <br/>
            <table align="center">

                <tr>
                    <td>
                        District<span style="color: red">*</span>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlDistricts" Height="16px" Width="150px" runat="server" OnSelectedIndexChanged="ddlDistricts_SelectedIndexChanged"
                                          AutoPostBack="true">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        Vehicle No<span style="color: red">*</span>
                    </td>

                    <td>
                        <asp:DropDownList ID="ddlVehicleno" Height="16px" Width="150px" runat="server" OnSelectedIndexChanged="ddlVehicleno_SelectedIndexChanged1"
                                          AutoPostBack="true">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        Bill No<span style="color: red">*</span>
                    </td>
                    <td>
                        <asp:DropDownList runat="server" CssClass="search_3" ID="ddlBillNo" Width="150px"
                                          AutoPostBack="True" OnSelectedIndexChanged="ddlBillNo_SelectedIndexChanged"/>
                    </td>
                </tr>

                <tr>
                    <td>
                        Bill Amount
                    </td>

                    <td>
                        <asp:TextBox ID="txtBillAmount" ReadOnly="True" CssClass="search_3" width="150px" runat="server"/>
                    </td>
                </tr>
                <tr>
                    <td>
                        BreakDown ID
                    </td>
                    <td>
                        <asp:Label
                            ID="lblBrkdwn" style="color: brown" runat="server"/>
                    </td>
                </tr>
                <tr>
                    <td>
                        Receipt Date<span style="color: red">*</span>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtReceiptDate" Width="150px" CssClass="search_3" onkeypress="return false" oncut="return false;" onpaste="return false;"/>
                        <cc1:CalendarExtender runat="server" CssClass="cal_Theme1" TargetControlID="txtReceiptDate" Format="MM/dd/yyyy" enabled="true">
                        </cc1:CalendarExtender>
                    </td>
                </tr>
                <tr>
                    <td>
                        Courier Name<span style="color: red">*</span>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtCourierName" CssClass="search_3" onkeypress="return alpha_only_withspace(event);"/>
                    </td>
                </tr>
                <tr>
                    <td>
                        Docket No<span style="color: red">*</span>
                    </td>
                    <td>
                        <asp:TextBox runat="server" CssClass="search_3" ID="txtDocketNo" onkeypress="return numeric_only(event);"/>
                    </td>
                </tr>
            </table>
            <table align="center">
                <tr>
                    <td>
                        <asp:Button runat="server" ID="btnSave" Text="Save" OnClick="btnSave_Click" CssClass="form-submit-button" OnClientClick="if (!validation()) return false;"/>
                        <asp:Button runat="server" ID="btnUpdate" CssClass="form-submit-button" Visible="false" Text="Update" OnClick="btnUpdate_Click"
                                    OnClientClick="return validation()"/>
                        <asp:Button runat="server" ID="btnReset" Text="Reset" CssClass="form-reset-button" OnClick="btnReset_Click"/>
                        <asp:HiddenField ID="HiddenField1" runat="server"/>

                    </td>
                </tr>
            </table>
            <br/>
            <div align="center">
                <div style="float: left; width: 200px;">
                </div>
                <div align="center">
                    <asp:GridView ID="gvVehiclePhysicalBillDetails" runat="server" EmptyDataText="No Records Found"
                                  AllowSorting="True" AutoGenerateColumns="False" CssClass="gridview"
                                  CellPadding="3" Width="630px" AllowPaging="True"
                                  EnableSortingAndPagingCallbacks="True" OnPageIndexChanging="gvVehiclePhysicalBillDetails_PageIndexChanging"
                                  OnRowCommand="gvVehiclePhysicalBillDetails_RowCommand" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px">
                        <Columns>
                            <asp:TemplateField HeaderText="District">
                                <ItemTemplate>
                                    <asp:Label ID="lblDistrict" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "District") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="VehicleNo">
                                <ItemTemplate>
                                    <asp:Label ID="lblVehicle_No" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Vechicleno") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="BreakDown ID">
                                <ItemTemplate>
                                    <asp:Label ID="lblBrkDwnID" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "ID") %>'></asp:Label>
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
                            <asp:TemplateField HeaderText="ReceiptDate">
                                <ItemTemplate>
                                    <asp:Label ID="lblReceiptDate" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "ReceiptDate") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Courier Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblCourier_Name" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Courier_Name") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Docket No">
                                <ItemTemplate>
                                    <asp:Label ID="lblDocketNo" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "DocketNo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Rejection Reason">
                                <ItemTemplate>
                                    <asp:Label ID="lblRejReason" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Reject") %>'></asp:Label>
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
                        <RowStyle ForeColor="#000066" />
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