<%@ Page Language="C#" MasterPageFile="~/temp.master" AutoEventWireup="true" CodeFile="VehicleScheduleServiceRequest.aspx.cs" Inherits="VehicleScheduleServiceRequest" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="background-color: #f7f7f7; border: 1px #E2BBA0 solid; height: 150px; margin: 0 0px 15px 0px; padding: 5px;">
        <img src="images/b1.jpg" alt="banner" width="653" height="150"/>
    </div>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <script type="text/javascript">
                function pageLoad() {
                    $('#<%= ddlVehicleNo.ClientID %>').select2({
                        disable_search_threshold: 5,
                        search_contains: true,
                        minimumResultsForSearch: 20,
                        placeholder: "Select an option"
                    });
                    $('#<%= ddlScheduleCat.ClientID %>').select2({
                        disable_search_threshold: 5,
                        search_contains: true,
                        minimumResultsForSearch: 20,
                        placeholder: "Select an option"
                    });
                }

                function validations() {
                    var ddlVehicle = $('#<%= ddlVehicleNo.ClientID %> option:selected').text().toLowerCase();
                    if (ddlVehicle === '--select--' || ddlVehicle === '') {
                        return alert("Please select Vehicle");
                    }
                    var ddlSchedule = $('#<%= ddlScheduleCat.ClientID %> option:selected').text().toLowerCase();
                    if (ddlSchedule === '--select--' || ddlSchedule === '') {
                        return alert("Please select Category");
                    }
                    var txtSchedulePlan = $('#<%= txtSchedulePlanDate.ClientID %>').val();
                    if (txtSchedulePlan === '')
                        return alert("Date is Required");
                    return true;
                }
            </script>

            <table id="table1" cellspacing="0" cellpadding="0" width="500px" align="center" border="0"
                   style="height: 37px">
                <tr>
                    <td class="rowseparator"></td>
                </tr>
                <tr>
                    <td style="height: 200px">
                        <fieldset style="padding: 10px;">
                            <legend>Schedule Service Request</legend>
                            <table id="table2" width="91%" align="center">
                                <tr>
                                    <td>
                                        <table align="center">
                                            <tr>
                                                <td style="width: 150px" align="left">
                                                    Vehicle Number <span style="color: Red">*</span>
                                                </td>
                                                <td class="columnseparator"></td>
                                                <td>
                                                    <asp:DropDownList ID="ddlVehicleNo" runat="server" AutoPostBack="true" Width="150px" OnSelectedIndexChanged="ddlVehicleNo_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="rowseparator"></td>
                                            </tr>
                                            <tr>
                                                <td style="width: 150px" align="left">
                                                    Schedule Category <span style="color: Red">*</span>
                                                </td>
                                                <td class="columnseparator"></td>
                                                <td>
                                                    <asp:DropDownList ID="ddlScheduleCat" runat="server" AppendDataBoundItems="true"
                                                                      Width="150px">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="rowseparator"></td>
                                            </tr>
                                            <tr>
                                                <td style="width: 150px" align="left">
                                                    Schedule Plan Date <span style="color: Red">*</span>
                                                </td>
                                                <td class="columnseparator"></td>
                                                <td>
                                                    <asp:TextBox ID="txtSchedulePlanDate" runat="server" CssClass="search_3" onpaste="return false" oncopy="return false" oncut="return false" onkeypress="return false"></asp:TextBox>

                                                    <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Format="MM/dd/yyyy" PopupButtonID="txtSchedulePlanDate"
                                                                          TargetControlID="txtSchedulePlanDate">
                                                    </cc1:CalendarExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="rowseparator"></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="height: 41px" align="center">
                                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="form-submit-button" Width="65px"
                                                    OnClick="btnSubmit_Click" OnClientClick="if (!validations()) return false;"/>
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="form-submit-button" Width="65px"
                                                    OnClick="btnCancel_Click"/>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <asp:Panel ID="pnlDisplayDetails" runat="server" Width="100%" Visible="false">
                                <asp:GridView ID="grvScheduleServiceRequest" runat="server" AllowPaging="True" PageSize="5"
                                              AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None"
                                              BorderWidth="1px" CellPadding="3" Width="660px" OnRowEditing="grvScheduleServiceRequest_RowEditing"
                                              OnRowDeleting="grvScheduleServiceRequest_RowDeleting" OnPageIndexChanging="grvScheduleServiceRequest_PageIndexChanging"
                                              CssClass="gridviewStyle">
                                    <RowStyle CssClass="rowStyleGrid" ForeColor="#000066"/>
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl No" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSlNo" runat="server" Text='<%#Eval("Sl_No") %>'/>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Vehicle ID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblVehicleID" runat="server" Text='<%#Eval("Vehicle_Id") %>'/>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Vehicle Number">
                                            <ItemTemplate>
                                                <asp:Label ID="lblVehicleNo" runat="server" Text='<%#Eval("VehicleNumber") %>'/>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="District">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDistrict" runat="server" Text='<%#Eval("District_Name") %>'/>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Schedule Category">
                                            <ItemTemplate>
                                                <asp:Label ID="lblScheduleCategory" runat="server" Text='<%#Eval("Schedule_Category") %>'/>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Schedule Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblScheduleDate" runat="server" Text='<%#Eval("Scheduled_Date") %>'/>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Status">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatus" runat="server" Text='<%#Eval("Status") %>'/>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkEdit" runat="server" Text="Edit" CommandArgument='<%# Eval("Sl_No") %>'
                                                                CommandName="Edit">
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <span onclick="return confirm('Are you sure to Delete the record?')">
                                                    <asp:LinkButton ID="lnkDelete" runat="server" Text="Delete" CommandName="Delete"
                                                                    CommandArgument='<%# Eval("Sl_No") + "," + Eval("Vehicle_Id") %>' OnCommand="lnkDelete_Click">
                                                    </asp:LinkButton>
                                                </span>
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
                            </asp:Panel>
                        </fieldset>
                    </td>
                </tr>
            </table>

            <script language="javascript" type="text/javascript">
                function validationForServiceReq() {
                    switch (document.getElementById("<%= ddlVehicleNo.ClientID %>").value) {
                    case 0:
                        alert("Please Select Vehicle Number");
                        document.getElementById("<%= ddlVehicleNo.ClientID %>").focus();
                        return false;
                    }
                    switch (document.getElementById("<%= ddlScheduleCat.ClientID %>").value) {
                    case 0:
                        alert("Please Select Scheduled Category");
                        document.getElementById("<%= ddlScheduleCat.ClientID %>").focus();
                        return false;
                    }
                    var spDate = document.getElementById('<%= txtSchedulePlanDate.ClientID %>');
                    if (!window.requiredValidation(spDate, "Scheduled Plan date cannot be Blank"))
                        return false;

                    if (trim(spDate.value) !== "" && !isValidDate(spDate.value)) {
                        alert("Enter the correct format (mm/dd/yyyy)");
                        spDate.focus();
                        return false;
                    }
                    return true;
                }
            </script>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>