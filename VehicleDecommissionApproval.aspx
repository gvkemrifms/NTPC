<%@ Page Language="C#" MasterPageFile="~/temp.master" AutoEventWireup="true" CodeFile="VehicleDecommissionApproval.aspx.cs" Inherits="VehicleDecommissionApproval" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<script type="text/javascript">
    function validation(obj, id) {
        var txtVehicleNumber = obj.id.replace(id, "txtVehicleNumber");
        var txtApproveRejectedRemarks = obj.id.replace(id, "txtApproveRejectedRemarks");
        var txtDecommisionDate = obj.id.replace(id, "txtDecommisionDate");
        var vehicleNumber = document.getElementById(txtVehicleNumber);
        var approveRejectedRemarks = document.getElementById(txtApproveRejectedRemarks);
        var decommisionDate = document.getElementById(txtDecommisionDate);

        switch (trim(vehicleNumber.value)) {
        case '':
            alert("Vehicle Number Cannot be Blank");
            vehicleNumber.focus();
            return false;
        }

        switch (trim(approveRejectedRemarks.value)) {
        case '':
            alert("Approve Rejected Remarks Cannot be Blank");
            approveRejectedRemarks.focus();
            return false;
        }
        if (id !== "btnReject") {
            switch (trim(decommisionDate.value)) {
            case '':
                alert("Decommision Date Cannot be Blank");
                decommisionDate.focus();
                return false;
            }

            if (decommisionDate.value !== "" && !isValidDate(decommisionDate.value)) {
                alert("Enter the Valid Date");
                decommisionDate.focus();
                return false;
            }
        }
        return true;
    }


</script>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
<table align="center">

<tr>
    <td>
        <legend style="color: brown" align="center">Vehicle Decommission Approval</legend>
    </td>
</tr>
<tr>
    <td class="rowseparator"></td>
</tr>
<tr>
    <td>
        <asp:Panel ID="pnlVehicleDecommissionApproval" runat="server">
            <fieldset style="padding: 10px">
                <table align="center">
                    <tr>
                        <td >
                            Vehicle Number<span style="color: red">*</span>
                        </td>
                        <td class="columnseparator"></td>
                        <td>
                            <asp:TextBox ID="txtVehicleNumber" CssClass="search_3" runat="server" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="rowseparator"></td>
                    </tr>
                    <tr>
                        <td class="tdlabel">
                            Off Road Date and Time
                        </td>
                        <td class="columnseparator"></td>
                        <td nowrap="nowrap">
                            <table>
                                <tr>
                                    <td nowrap="nowrap">
                                        <asp:TextBox ID="txtOffRoadDate" CssClass="search_3" runat="server" onkeypress="return false;"></asp:TextBox>
                                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgOffRoadDate"
                                                              TargetControlID="txtOffRoadDate">
                                        </cc1:CalendarExtender>
                                        <asp:ImageButton ID="imgOffRoadDate" runat="server" ImageUrl="images/Calendar.gif"/>
                                    </td>
                                    <td>
                                        <asp:DropDownList CssClass="search_3" runat="server" Visible="false">
                                            <asp:ListItem Selected="True" Text="--hh--" Value="--hh--"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList CssClass="search_3" runat="server" Visible="false">
                                            <asp:ListItem Selected="True" Text="--mm--" Value="--hh--"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="rowseparator"></td>
                    </tr>
                    <tr>
                        <td class="tdlabel">
                            Date of Registration
                        </td>
                        <td class="columnseparator"></td>
                        <td>
                            <asp:TextBox ID="txtDateOfRegistration" CssClass="search_3" runat="server" onkeypress="return false;"></asp:TextBox>
                            <asp:ImageButton ID="imgBtnDateOfRegistration" runat="server" ImageUrl="images/Calendar.gif"/>
                            <cc1:CalendarExtender CssClass="cal_Theme1" runat="server" PopupButtonID="imgBtnDateOfRegistration"
                                                  TargetControlID="txtDateOfRegistration" Format="MM/dd/yyyy">
                            </cc1:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td class="rowseparator"></td>
                    </tr>
                    <tr>
                        <td class="tdlabel">
                            Date of Launching
                        </td>
                        <td class="columnseparator"></td>
                        <td>
                            <asp:TextBox ID="txtDateOfLaunching" CssClass="search_3" runat="server" onkeypress="return false;"></asp:TextBox>
                            <asp:ImageButton ID="imgBtnDateOfLaunching" runat="server" ImageUrl="images/Calendar.gif"/>
                            <cc1:CalendarExtender CssClass="cal_Theme1" runat="server" PopupButtonID="imgBtnDateOfLaunching"
                                                  TargetControlID="txtDateOfLaunching" Format="MM/dd/yyyy">
                            </cc1:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td class="rowseparator"></td>
                    </tr>
                    <tr>
                        <td class="tdlabel">
                            Date of Purchase
                        </td>
                        <td class="columnseparator"></td>
                        <td>
                            <asp:TextBox ID="txtDateofPurchase" CssClass="search_3" runat="server" onkeypress="return false;"></asp:TextBox>
                            <asp:ImageButton ID="imgBtnDateofPurchase" runat="server" ImageUrl="images/Calendar.gif"/>
                            <cc1:CalendarExtender CssClass="cal_Theme1" runat="server" PopupButtonID="imgBtnDateofPurchase"
                                                  TargetControlID="txtDateofPurchase" Format="MM/dd/yyyy">
                            </cc1:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td class="rowseparator"></td>
                    </tr>
                    <tr>
                        <td class="tdlabel">
                            Survey Date
                        </td>
                        <td class="columnseparator"></td>
                        <td>
                            <asp:TextBox ID="txtSurveyDate" CssClass="search_3" runat="server" onkeypress="return false;"></asp:TextBox>
                            <asp:ImageButton ID="imgBtnSurveyDate" runat="server" ImageUrl="images/Calendar.gif"/>
                            <cc1:CalendarExtender CssClass="cal_Theme1" runat="server" PopupButtonID="imgBtnSurveyDate"
                                                  TargetControlID="txtSurveyDate" Format="MM/dd/yyyy">
                            </cc1:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td class="rowseparator"></td>
                    </tr>
                    <tr>
                        <td class="tdlabel">
                            Survey By
                        </td>
                        <td class="columnseparator"></td>
                        <td>
                            <asp:TextBox ID="txtSurveyBy" runat="server" CssClass="search_3" onkeypress="return alpha_only_withspace(event);"
                                         MaxLength="35">
                            </asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="rowseparator"></td>
                    </tr>
                    <tr>
                        <td class="tdlabel">
                            Proposed Remarks
                        </td>
                        <td class="columnseparator"></td>
                        <td>
                            <asp:TextBox ID="txtProposedRemarks" CssClass="search_3" runat="server" TextMode="MultiLine" onkeypress="return alphanumeric_only_withspace(event);"
                                         MaxLength="200">
                            </asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="rowseparator"></td>
                    </tr>
                    <tr>
                        <td class="tdlabel">
                            Approved/Rejected Remarks <span style="color: red">*</span>
                        </td>
                        <td class="columnseparator"></td>
                        <td>
                            <asp:TextBox ID="txtApproveRejectedRemarks" CssClass="search_3" runat="server" onkeypress="return alphanumeric_only_withspace(event);"
                                         MaxLength="200" TextMode="MultiLine">
                            </asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="rowseparator"></td>
                    </tr>
                    <tr>
                        <td class="tdlabel">
                            Decommission Date <span style="color: red">*</span>
                        </td>
                        <td class="columnseparator"></td>
                        <td>
                            <asp:TextBox ID="txtDecommisionDate" CssClass="search_3" runat="server" onkeypress="return false;"></asp:TextBox>
                            <asp:ImageButton ID="imgBtnDecommisionDate" runat="server" ImageUrl="images/Calendar.gif"/>
                            <cc1:CalendarExtender CssClass="cal_Theme1" runat="server" PopupButtonID="imgBtnDecommisionDate"
                                                  TargetControlID="txtDecommisionDate" Format="MM/dd/yyyy">
                            </cc1:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td class="rowseparator"></td>
                    </tr>
                    <tr>
                        <td colspan="3" align="center">
                            <asp:Button ID="btnAccept" runat="server" CssClass="form-submit-button" Text="Accept" OnClick="btnAccept_Click"/>
                            <asp:Button ID="btnReject" runat="server" Text="Rejected" CssClass="form-reset-button" OnClick="btnReject_Click"/>
                        </td>
                    </tr>
                    <tr>
                        <td class="rowseparator"></td>
                    </tr>
                </table>
            </fieldset>
        </asp:Panel>
    </td>
</tr>
    <caption>
        <br/>
        <tr>
            <td>
                <asp:GridView ID="grdVehicleDecompositionApproval" runat="server" AllowPaging="True" AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" CssClass="gridviewStyle" EmptyDataText="No Records Found" OnRowCommand="grdVehicleDecompositionApproval_RowCommand" Width="630px">
                    <RowStyle CssClass="rowStyleGrid" ForeColor="#000066" />
                    <Columns>
                        <asp:TemplateField HeaderText="Vehicle Number">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkBtnVehicleNumber" runat="server" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "VehicleProposalId") %>' CommandName="vehicleApproval">
                            <%#DataBinder.Eval(Container.DataItem, "VehicleNumber") %>
                        </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Off Road Date">
                            <ItemTemplate>
                                <asp:Label ID="lblOffRoadDateandDate" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "OffRoadDate") %>'>&#39;&gt;</asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Total Km Covered">
                            <ItemTemplate>
                                <asp:Label ID="lblTotalKmCovered" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "TotalDistanceTravelled") %>'>&#39;&gt;</asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Date of Registration">
                            <ItemTemplate>
                                <asp:Label ID="lblDateofRegistration" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "DateOfRegistration") %>'>&#39;&gt;</asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Survey By">
                            <ItemTemplate>
                                <asp:Label ID="lblDateofPurchase" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "SurveyBy") %>'>&#39;&gt;</asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Survey Remarks">
                            <ItemTemplate>
                                <asp:Label ID="lblSurveyorRemark" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "SurveyRemark") %>'>&#39;&gt;</asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Proposed Remarks">
                            <ItemTemplate>
                                <asp:Label ID="lblProposedRemarks" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "ProposedRemark") %>'>&#39;&gt;</asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <FooterStyle BackColor="White" CssClass="footerStylegrid" ForeColor="#000066" />
                    <PagerStyle BackColor="White" CssClass="pagerStylegrid" ForeColor="#000066" HorizontalAlign="Left" />
                    <SelectedRowStyle BackColor="#669999" CssClass="selectedRowStyle" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#006699" CssClass="headerStyle" Font-Bold="True" ForeColor="White" />
                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                    <SortedAscendingHeaderStyle BackColor="#007DBB" />
                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                    <SortedDescendingHeaderStyle BackColor="#00547E" />
                </asp:GridView>
            </td>
        </tr>
    </caption>


</table>
</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>