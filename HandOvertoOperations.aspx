<%@ Page Language="C#" MasterPageFile="~/temp.master" AutoEventWireup="true" CodeFile="HandOvertoOperations.aspx.cs" Inherits="HandOvertoOperations" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <script type="text/javascript">
        function validation() {
            var handOverto = document.getElementById('<%= txtHandOverto.ClientID %>');
            var handoverDate = document.getElementById('<%= txtHandoverDate.ClientID %>');
            var handOverBy = document.getElementById('<%= txtHandOverBy.ClientID %>');
            var qualityInspectionNo = document.getElementById('<%= txtQualityInspectionNo.ClientID %>');
            var inspectionDate = document.getElementById('<%= txtInspectionDate.ClientID %>');
            var inspectedBy = document.getElementById('<%= txtInspectedBy.ClientID %>');
            var remarks = document.getElementById('<%= txtRemarks.ClientID %>');
            var vehicleRegistrationDate = document.getElementById('<%= vehicleRegistrationDate.ClientID %>');
            var vehiclePurchaseDate = document.getElementById('<%= vehiclePurchaseDate.ClientID %>');
            var now = new Date();
            var id = document.getElementById('<%= ddlVehicleNo.ClientID %>');
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

            if (!RequiredValidation(handOverto, "Handover To Cannot be Blank"))
                return false;

            if (!RequiredValidation(handoverDate, "Handover Date Cannot be Blank"))
                return false;

            if (!isValidDate(handoverDate.value)) {
                alert("Enter Valid Date");
                handoverDate.focus();
                return false;
            }

            if (Date.parse(handoverDate.value) > Date.parse(now)) {
                alert("Handover Date should not be greater than Current Date");
                handoverDate.focus();
                return false;
            }

            if (Date.parse(handoverDate.value) < Date.parse(vehicleRegistrationDate.value)) {
                alert("Handover Date should not be greater than Registration Date.(Registration Date-" +
                    vehicleRegistrationDate.value +
                    ")");
                handoverDate.focus();
                return false;
            }

            if (!RequiredValidation(handOverBy, "HandOver By Cannot be Blank"))
                return false;

            if (!RequiredValidation(qualityInspectionNo, "Quality Inspection Number Cannot be Blank"))
                return false;

            if (!RequiredValidation(inspectionDate, "Inspection Date Cannot be Blank"))
                return false;

            if (!isValidDate(inspectionDate.value)) {
                alert("Enter Valid Date");
                inspectionDate.focus();
                return false;
            }

            if (Date.parse(inspectionDate.value) > Date.parse(now)) {
                alert("Inspection Date should not be greater than Current Date");
                inspectionDate.focus();
                return false;
            }

            if (Date.parse(inspectionDate.value) > Date.parse(handoverDate.value)) {
                alert("Inspection Date should not be greater than Handover Date");
                inspectionDate.focus();
                return false;
            }
            if (Date.parse(inspectionDate.value) < Date.parse(vehiclePurchaseDate.value)) {
                alert("Inspection Date should be greater than Purchase Date(Purchase Date-" +
                    vehiclePurchaseDate.value +
                    ")");
                inspectionDate.focus();
                return false;
            }


            if (!RequiredValidation(inspectedBy, "Inspected By Cannot be Blank"))
                return false;

            if (!RequiredValidation(remarks, "Remarks Cannot be Blank"))
                return false;
            return true;
        }

    </script>


    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlHandOverToOperation" runat="server">
                <legend align="center" style="color: brown">Hand Over To Operations</legend>
                <table style="width: 100%">
                    <tr>
                        <td align="center" style="font-size: small; font-weight: bold" colspan="4">
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="4">
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="4">
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="4">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 440px">
                        </td>
                        <td align="left" style="width: 300px">
                            Vehicle No.<span style="color: Red">*</span>
                        </td>
                        <td align="left" style="width: 288px">
                            <cc1:ComboBox AutoCompleteMode="Append" ID="ddlVehicleNo" runat="server" Width="150px"
                                          onselectedindexchanged="ddlVehicleNo_SelectedIndexChanged"
                                          AutoPostBack="True" DropDownStyle="DropDownList">
                            </cc1:ComboBox>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 440px">
                        </td>
                        <td align="left" style="width: 300px">
                            HandOver To<span style="color: Red">*</span>
                        </td>
                        <td align="left" style="width: 288px">

                            <asp:TextBox ID="txtHandOverto" CssClass="search_3" runat="server" MaxLength="35" Width="145px" onkeypress="return alpha_only_withspace(event);"></asp:TextBox>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 440px">
                        </td>
                        <td align="left" style="width: 300px">
                            Handover Date<span style="color: Red">*</span>
                        </td>
                        <td align="left" style="width: 288px">
                            <asp:TextBox ID="txtHandoverDate" CssClass="search_3" runat="server" Width="145px" onkeypress="return false" oncut="return false;" onpaste="return false;">
                            </asp:TextBox>
                            <asp:ImageButton ID="imgBtnCalendarHandoverDate" runat="server" Style="vertical-align: top"
                                             alt="" src="images/Calendar.gif"/>
                            <cc1:CalendarExtender CssClass="cal_Theme1" runat="server" TargetControlID="txtHandoverDate"
                                                  PopupButtonID="imgBtnCalendarHandoverDate" Format="MM/dd/yyyy">
                            </cc1:CalendarExtender>
                        </td>
                        <td rowspan="4">
                            <asp:CheckBoxList runat="server" Visible="False">
                                <asp:ListItem Value="1">Select</asp:ListItem>
                                <asp:ListItem Value="0">Dummy</asp:ListItem>
                            </asp:CheckBoxList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 440px">
                        </td>
                        <td align="left" style="width: 300px">
                            Handover By<span style="color: Red">*</span>
                        </td>
                        <td align="left" style="width: 288px">
                            <asp:TextBox ID="txtHandOverBy" runat="server" CssClass="search_3" MaxLength="35" Width="145px" onkeypress="return alpha_only_withspace(event);"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 440px">
                        </td>
                        <td align="left" style="width: 300px">
                            Quality Inspection No<span style="color: Red">*</span>
                        </td>
                        <td align="left" style="width: 288px">
                            <asp:TextBox ID="txtQualityInspectionNo" CssClass="search_3" runat="server" Width="145px" MaxLength="15" onkeypress="return alphanumeric_only(event);"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 440px">
                        </td>
                        <td align="left" style="width: 300px">
                            Inspection Date<span style="color: Red">*</span>
                        </td>
                        <td align="left" style="width: 288px">
                            <asp:TextBox ID="txtInspectionDate" CssClass="search_3" runat="server" Width="145px" onkeypress="return false" oncut="return false;" onpaste="return false;">
                            </asp:TextBox>
                            <asp:ImageButton ID="imBtnInspectionDate" runat="server" Style="vertical-align: top"
                                             alt="" src="images/Calendar.gif"/>
                            <cc1:CalendarExtender runat="server" TargetControlID="txtInspectionDate"
                                                  PopupButtonID="imBtnInspectionDate" CssClass="cal_Theme1" Format="MM/dd/yyyy">
                            </cc1:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 440px">
                        </td>
                        <td align="left" style="width: 300px">
                            Inspected By<span style="color: Red">*</span>
                        </td>
                        <td align="left" style="width: 288px">
                            <asp:TextBox ID="txtInspectedBy" CssClass="search_3" runat="server" Width="145px" MaxLength="35" onkeypress="return alpha_only_withspace(event);"></asp:TextBox>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 440px">
                        </td>
                        <td align="left" style="width: 300px">
                            Remarks<span style="color: Red">*</span>
                        </td>
                        <td align="left" style="width: 288px">
                            <asp:TextBox ID="txtRemarks" runat="server" Width="145px" TextMode="MultiLine"
                                         MaxLength="200" CssClass="search_3" onkeypress="return remark(event);">
                            </asp:TextBox>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 440px">
                            &nbsp;
                        </td>
                        <td align="center" style="width: 300px">
                            <asp:Button ID="btHandover" Text="Handover" runat="server" OnClick="btHandover_Click" CssClass="form-submit-button"/>
                        </td>
                        <td align="left" style="width: 288px">
                            <asp:Button ID="btReset" Text="Reset" runat="server" OnClick="btReset_Click" CssClass="form-reset-button"/>
                        </td>
                        <td width="300px">
                            <asp:LinkButton runat="server" Visible="False">
                                Attach
                                Inspection Report
                            </asp:LinkButton>
                        </td>
                    </tr>
                    <asp:HiddenField ID="vehiclePurchaseDate" runat="server"/>
                    <asp:HiddenField ID="vehicleRegistrationDate" runat="server"/>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>