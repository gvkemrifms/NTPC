<%@ Page Language="C#" MasterPageFile="~/temp.master" AutoEventWireup="true" CodeFile="HandOverToOwner.aspx.cs" Inherits="HandOverToOwner" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <script language="javascript" type="text/javascript">

        function validation(obj, id) {

            var txtVehicleNumber = obj.id.replace(id, "txtVehicleNumber");
            var txtHandOverTo = obj.id.replace(id, "txtHandOverTo");
            var txtHandOverDate = obj.id.replace(id, "txtHandOverDate");
            var txtHandOverBy = obj.id.replace(id, "txtHandOverBy");
            var txtOdoreading = obj.id.replace(id, "txtOdoreading");

            var vehicleNumber = document.getElementById(txtVehicleNumber);
            var handOverTo = document.getElementById(txtHandOverTo);
            var handOverDate = document.getElementById(txtHandOverDate);
            var handOverBy = document.getElementById(txtHandOverBy);
            var odoReading = document.getElementById(txtOdoreading);
            switch (trim(vehicleNumber.value)) {
            case '':
                alert("VehicleNumber To Cannot be Blank");
                vehicleNumber.focus();
                return false;
            }
            switch (trim(handOverTo.value)) {
            case '':
                alert("Hand Over To Cannot be Blank");
                handOverTo.focus();
                return false;
            }

            switch (trim(handOverDate.value)) {
            case '':
                alert("Hand Over Date Cannot be Blank");
                handOverDate.focus();
                return false;
            }

            if (handOverDate.value !== "" && !isValidDate(handOverDate.value)) {
                alert("Enter the Valid Date");
                handOverDate.focus();
                return false;
            }

            var now = new Date();

            if (Date.parse(handOverDate.value) > Date.parse(now)) {
                alert("HandOver Date should not be greater than Current Date");
                handOverDate.focus();
                return false;
            }

            switch (trim(handOverBy.value)) {
            case '':
                alert("Hand Over By Cannot be Blank");
                handOverBy.focus();
                return false;
            }

            switch (trim(odoReading.value)) {
            case '':
                alert("Odo Reading Cannot be Blank");
                odoReading.focus();
                return false;
            }

            return true;
        }

        function checkEnter(e) {

            var characterCode;

            if (e && e.which) {
                characterCode = e.which;
            } else {
                e = event;
                characterCode = e.keyCode;
            }

            switch (characterCode) {
            case 13:
                document.getElementById('btnget').click();
                return false;

            default:
                return true;

            }
        }

    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="2" cellspacing="2" width="100%">
                <tr>
                    <td class="heading">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="rowseparator">
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlgrdTemporaryVehicle" runat="server">
                            <fieldset style="padding: 10px">
                                <legend align="center" style="color: brown">Temporary Vehicle </legend>
                                <br/>
                                <asp:GridView ID="grdTemporaryVehicle" align="center" runat="server" AutoGenerateColumns="False"
                                              CellPadding="3" OnRowCommand="grdTemporaryVehicle_RowCommand"
                                              Width="238px" EmptyDataText="No Records Found" CssClass="gridviewStyle" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px">
                                    <RowStyle CssClass="rowStyleGrid" ForeColor="#000066"/>
                                    <Columns>
                                        <asp:TemplateField HeaderText="Vehicle Number">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkBtnVehicleNumber" runat="server" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "VehicleID") %>'
                                                                CommandName="vehicleAccidentedit">
                                                    <%#DataBinder.Eval(Container.DataItem, "VehicleNumber") %>
                                                </asp:LinkButton>
                                                <asp:LinkButton ID="hdnLnkShowModal" runat="server" Style="display: none;"></asp:LinkButton>
                                                <cc1:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="hdnLnkShowModal"
                                                                        PopupControlID="Panel1" OkControlID="btnClose" CancelControlID="btnReset" BackgroundCssClass="modalBackground">
                                                </cc1:ModalPopupExtender>
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
                            </fieldset>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td class="rowseparator">
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%;">
                        <asp:Panel ID="Panel1" runat="server" Style="display: none; padding: 10px" CssClass="modalPanel">
                            <table cellpadding="2" cellspacing="2" width="100%" align="center">
                                <tr>
                                    <td class="tdlabel">
                                        Vehicle Number
                                    </td>
                                    <td class="columnseparator">
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtVehicleNumber" runat="server" ReadOnly="True"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="rowseparator">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdlabel">
                                        Hand Over To<font color="red">*</font>
                                    </td>
                                    <td class="columnseparator">
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtHandOverTo" runat="server" onkeypress="return alpha_only_withspace(event);" MaxLength="35"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="rowseparator">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdlabel">
                                        Hand Over Date<font color="red">*</font>
                                    </td>
                                    <td class="columnseparator">
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtHandOverDate" runat="server" onkeypress="return false;"></asp:TextBox>
                                        <asp:ImageButton ID="imgBtnHandOverDate" runat="server" ImageUrl="images/Calendar.gif"/>
                                        <cc1:CalendarExtender runat="server" Format="MM/dd/yyyy" PopupButtonID="imgBtnHandOverDate"
                                                              TargetControlID="txtHandOverDate">
                                        </cc1:CalendarExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="rowseparator">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdlabel">
                                        Hand Over By<font color="red">*</font>
                                    </td>
                                    <td class="columnseparator">
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtHandOverBy" runat="server" onkeypress="return alpha_only_withspace(event);" MaxLength="35"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="rowseparator">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdlabel">
                                        Odo Reading<font color="red">*</font>
                                    </td>
                                    <td class="columnseparator">
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOdoreading" runat="server" onkeypress="return numericOnly(event);" MaxLength="10"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="rowseparator">
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" align="center">
                                        <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click"/>
                                        <asp:Button ID="btnClose" runat="server" Text="Save" Style="display: none;"/>
                                        <asp:Button ID="btnReset" runat="server" Text="Cancel" OnClick="btnReset_Click"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="rowseparator">
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td class="rowseparator">
                    </td>
                </tr>
                <tr>
                    <td>
                        <fieldset style="padding: 10px">
                            <legend align="center" style="color: brown">Hand Over to Owner</legend>
                            <asp:GridView ID="grdVehicleDecompositionApproval" align="center" runat="server" AutoGenerateColumns="False"
                                          Width="630px" OnRowCommand="grdVehicleDecompositionApproval_RowCommand" AllowPaging="True"
                                          OnPageIndexChanging="grdVehicleDecompositionApproval_PageIndexChanging" CellPadding="3" EmptyDataText="No Records Found" CssClass="gridviewStyle" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px">
                                <RowStyle CssClass="rowStyleGrid" ForeColor="#000066"/>
                                <Columns>
                                    <asp:TemplateField HeaderText="Vehicle Number">
                                        <ItemTemplate>
                                            <%#DataBinder.Eval(Container.DataItem, "VehicleNumber") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Recieved From">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRecievedFrom" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "HandOverBy") %>'></asp:Label>

                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Recieved Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRecievedDate" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "ReceivedDate") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Recieved Odo Reading">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRecievedOdoReading" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "OdoReading") %>'></asp:Label>
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
                        </fieldset>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>