<%@ Page Language="C#" MasterPageFile="~/temp.master" AutoEventWireup="true" CodeFile="InsuranceClaimsPaymentStatus.aspx.cs" Inherits="InsuranceClaimsPaymentStatus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<link href="css/InsuranceClaimsPaymentStatus.css" rel="stylesheet"/>
<script type="text/javascript">
    function validation() {

        var spotSurveyor = document.getElementById('<%= txtSpotSurveyor.ClientID %>');
        var finalSurveyor = document.getElementById('<%= txtFinalSurveyor.ClientID %>');
        var reinspectionSurveyor = document.getElementById('<%= txtReinspectionSurveyor.ClientID %>');
        var claimFormSubmissionDate = document.getElementById('<%= txtClaimFormSubmissionDate.ClientID %>');
        var totalCostOfRepairs = document.getElementById('<%= txtTotalCostOfRepairs.ClientID %>');
        var surveyorAssessmentValue = document.getElementById('<%= txtSurveyorAssessmentValue.ClientID %>');
        var billSubmissionDate = document.getElementById('<%= txtBillSubmissionDate.ClientID %>');
        var paymentStatus = document.getElementById('<%= ddlPaymentStatus.ClientID %>');
        var remarks = document.getElementById('<%= txtRemarks.ClientID %>');
        var paymentReceivedDate = document.getElementById('<%= txtPaymentReceivedDate.ClientID %>');
        var chequeNo = document.getElementById('<%= txtChequeNo.ClientID %>');
        var amountReceivedFromInsurance = document.getElementById('<%= txtAmountReceivedFromInsurance.ClientID %>');
        var costToCompany = document.getElementById('<%= txtCostToCompany.ClientID %>');


        if (!RequiredValidation(spotSurveyor, "Spot Surveyor Cannot be Blank"))
            return false;

        if (!RequiredValidation(finalSurveyor, "Final Surveyor Cannot be Blank"))
            return false;

        if (!RequiredValidation(reinspectionSurveyor, "Reinspection Surveyor Cannot be Blank"))
            return false;

        if (!RequiredValidation(claimFormSubmissionDate, "Claim Form Submission Date Cannot be Blank"))
            return false;

        if (isValidDate(claimFormSubmissionDate.value)) {
            if (!RequiredValidation(totalCostOfRepairs, "Total Cost Of Repairs Cannot be Blank"))
                return false;

            if (!RequiredValidation(surveyorAssessmentValue, "Surveyor Assessment Value Cannot be Blank"))
                return false;

            if (!RequiredValidation(billSubmissionDate, "Bill Submission Date Cannot be Blank"))
                return false;

            if (isValidDate(billSubmissionDate.value)) {
                switch (paymentStatus.selectedIndex) {
                case 0:
                    alert("Please select Payment Status");
                    paymentStatus.focus();
                    return false;
                }

                if (!RequiredValidation(remarks, "Remarks Cannot be Blank"))
                    return false;

                if (!RequiredValidation(paymentReceivedDate, "Payment Received Date Cannot be Blank"))
                    return false;

                if (isValidDate(paymentReceivedDate.value)) {
                    if (!RequiredValidation(chequeNo, "Cheque No Cannot be Blank"))
                        return false;

                    if (!RequiredValidation(amountReceivedFromInsurance,
                        "Amount Received From Insurance Cannot be Blank"))
                        return false;

                    if (!RequiredValidation(costToCompany, "Cost To Company Cannot be Blank"))
                        return false;
                } else {
                    alert("Enter Valid Date");
                    paymentReceivedDate.focus();
                    return false;
                }
            } else {
                alert("Enter Valid Date");
                billSubmissionDate.focus();
                return false;
            }
        } else {
            alert("Enter Valid Date");
            claimFormSubmissionDate.focus();
            return false;
        }
        return true;
    }
</script>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
<table>
<tr>
    <td class="rowseparator"></td>
</tr>
<tr>
<td>

<table style="width: 100%">
<tr>
    <td align="center" colspan="4">
        <b>Accident Details</b>
    </td>
    <td class="columnseparator"></td>
    <td align="center" colspan="3">
        <b>Insurance Details</b>
    </td>
</tr>
<tr>
    <td align="left" style="width: 120px" nowrap="nowrap">
        Vehicle Number
    </td>
    <td class="columnseparator"></td>
    <td align="center" style="width: 20px" colspan="2">
        <asp:TextBox ID="txtVehicleNumber" CssClass="text1" runat="server" Width="145px" BackColor="DarkGray"
                     ReadOnly="True">
        </asp:TextBox>
    </td>
    <td class="columnseparator"></td>
    <td align="left">
        Policy Number
    </td>
    <td class="columnseparator"></td>
    <td align="center">
        <asp:TextBox ID="txtPolicyNumber" CssClass="text1" runat="server" Width="145px" BackColor="DarkGray"
                     ReadOnly="True">
        </asp:TextBox>
    </td>
</tr>
<tr>
    <td align="left" nowrap="nowrap" style="width: 181px">
        Accident Date/Time
    </td>
    <td class="columnseparator"></td>
    <td align="center" colspan="2" style="width: 38px">
        <asp:TextBox ID="txtAccidentDateTime" CssClass="text1" runat="server" BackColor="DarkGray" Height="20px"
                     ReadOnly="True" Width="145px">
        </asp:TextBox>
    </td>
    <td class="columnseparator"></td>
    <td align="left">
        Agency
    </td>
    <td class="columnseparator"></td>
    <td align="center">
        <asp:TextBox ID="txtAgency" CssClass="text1" runat="server" BackColor="DarkGray" ReadOnly="True" Width="145px"></asp:TextBox>
    </td>
</tr>
<tr>
    <td align="left" style="width: 181px">
        Accident Title
    </td>
    <td class="columnseparator"></td>
    <td align="center" colspan="2" style="width: 38px">
        <asp:TextBox ID="txtAccidentTitle" CssClass="text1" runat="server" BackColor="DarkGray" ReadOnly="True"
                     Width="145px">
        </asp:TextBox>
    </td>
    <td class="columnseparator"></td>
    <td align="left">
        Insurance Start Date
    </td>
    <td class="columnseparator"></td>
    <td align="center">
        <asp:TextBox ID="txtInsuranceStartDate" CssClass="text1" runat="server" BackColor="DarkGray" ReadOnly="True"
                     Width="145px">
        </asp:TextBox>
    </td>
</tr>
<tr>
    <td align="left" style="width: 181px">
        Spot Surveyor
    </td>
    <td class="columnseparator"></td>
    <td align="center" colspan="2" style="width: 38px">
        <asp:TextBox ID="txtSpotSurveyor" CssClass="text1" runat="server" Width="145px" onkeypress="return alpha_only(event);"
                     MaxLength="35">
        </asp:TextBox>
    </td>
    <td class="columnseparator"></td>
    <td align="left">
        Insurance End Date
    </td>
    <td class="columnseparator"></td>
    <td align="center">
        <asp:TextBox ID="txtInsuranceEndDate" CssClass="text1" runat="server" BackColor="DarkGray" ReadOnly="True"
                     Width="145px">
        </asp:TextBox>
    </td>
</tr>
<tr>
    <td align="left" style="width: 181px">
        Final Surveyor
    </td>
    <td class="columnseparator"></td>
    <td align="center" colspan="2" style="width: 38px">
        <asp:TextBox ID="txtFinalSurveyor" CssClass="text1" runat="server" Width="145px" onkeypress="return alpha_only(event);"
                     MaxLength="35">
        </asp:TextBox>
    </td>
    <td class="columnseparator"></td>
    <td align="left">
        Date Insurance
    </td>
    <td class="columnseparator"></td>
    <td align="center">
        <asp:TextBox ID="txtDateInsurance" CssClass="text1" runat="server" BackColor="DarkGray" ReadOnly="True"
                     Width="145px">
        </asp:TextBox>
    </td>
</tr>
<tr>
    <td align="left" nowrap="nowrap" style="width: 181px">
        Reinspection Surveyor
    </td>
    <td class="columnseparator"></td>
    <td align="center" colspan="2" style="width: 38px">
        <asp:TextBox ID="txtReinspectionSurveyor" CssClass="text1" runat="server" Width="145px" onkeypress="return alpha_only(event);"
                     MaxLength="35">
        </asp:TextBox>
    </td>
</tr>
<tr>
    <td align="left" colspan="3"></td>
    <td align="center" colspan="2"></td>
    <td align="center" colspan="2"></td>
</tr>
<tr>
    <td align="center" rowspan="12" style="width: 116px">
        &nbsp;
    </td>
    <td align="left" colspan="3" nowrap="nowrap">
        Claim Form Submission Date<span style="color: Red">*</span>
    </td>
    <td class="columnseparator"></td>
    <td align="center" colspan="2">
        <asp:TextBox ID="txtClaimFormSubmissionDate" CssClass="text1" runat="server" Width="145px" onkeypress="return false"
                     oncut="return false;" onpaste="return false;">
        </asp:TextBox>
    </td>
    <td align="left">
        <cc1:CalendarExtender runat="server" Enabled="True"
                              Format="MM/dd/yyyy" PopupButtonID="imgBtnCalendarClaimFormSubmissionDate" TargetControlID="txtClaimFormSubmissionDate">
        </cc1:CalendarExtender>
        <asp:ImageButton ID="imgBtnCalendarClaimFormSubmissionDate" runat="server" alt=""
                         src="images/Calendar.gif" Style="vertical-align: top"/>
    </td>
</tr>
<tr>
    <td align="left" colspan="3">
        Total Cost Of Repairs<span style="color: Red">*</span>
    </td>
    <td class="columnseparator"></td>
    <td align="center" colspan="2">
        <asp:TextBox ID="txtTotalCostOfRepairs" CssClass="text1" runat="server" onkeypress="return isDecimalNumberKey(event);"
                     Width="145px" MaxLength="7">
        </asp:TextBox>
    </td>
</tr>
<tr>
    <td align="left" colspan="3">
        Surveyor Assessment Value<span style="color: Red">*</span>
    </td>
    <td class="columnseparator"></td>
    <td align="center" colspan="2">
        <asp:TextBox ID="txtSurveyorAssessmentValue" CssClass="text1" runat="server" onkeypress="return isDecimalNumberKey(event);"
                     Width="145px" MaxLength="7">
        </asp:TextBox>
    </td>
</tr>
<tr>
    <td align="left" colspan="3">
        Bill Submission Date<span style="color: Red">*</span>
    </td>
    <td class="columnseparator"></td>
    <td align="center" colspan="2">
        <asp:TextBox ID="txtBillSubmissionDate" CssClass="text1" runat="server" Width="145px" onkeypress="return false"
                     oncut="return false;" onpaste="return false;">
        </asp:TextBox>
    </td>
    <td align="left">
        <cc1:CalendarExtender runat="server" Enabled="True"
                              Format="MM/dd/yyyy" PopupButtonID="imgBtnCalendarBillSubmissionDate" TargetControlID="txtBillSubmissionDate">
        </cc1:CalendarExtender>
        <asp:ImageButton ID="imgBtnCalendarBillSubmissionDate" runat="server" alt="" src="images/Calendar.gif"
                         Style="vertical-align: top"/>
    </td>
</tr>
<tr>
    <td align="left" colspan="3">
        Payment Status<span style="color: Red">*</span>
    </td>
    <td class="columnseparator"></td>
    <td align="center" colspan="2">
        <asp:DropDownList ID="ddlPaymentStatus" class="text1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlPaymentStatus_SelectedIndexChanged"
                          Width="150px">
            <asp:ListItem Value="-1">--Select--</asp:ListItem>
            <asp:ListItem Value="0">Received</asp:ListItem>
            <asp:ListItem Value="1">Rejected</asp:ListItem>
        </asp:DropDownList>
    </td>
</tr>
<tr>
    <td align="left" colspan="3">
        Remarks<span style="color: Red">*</span>
    </td>
    <td class="columnseparator"></td>
    <td align="center" colspan="2">
        <asp:TextBox ID="txtRemarks" runat="server" CssClass="text1" TextMode="MultiLine" Width="145px" onkeypress="return remark(event);"
                     MaxLength="200">
        </asp:TextBox>
    </td>
    <td align="center">
        <asp:CheckBox ID="chbxCloseClaim" runat="server" Text="  Close This Claim" Visible="False"/>
    </td>
</tr>
<tr>
    <td align="left" colspan="3">
        <asp:Label ID="lblPaymentReceivedDate" runat="server" Text="Payment Received Date"></asp:Label>
        <asp:Label ID="lblpaymntrecdt" runat="server" ForeColor="Red" Text="*"></asp:Label>
    </td>
    <td class="columnseparator"></td>
    <td align="center" colspan="2">
        <asp:TextBox ID="txtPaymentReceivedDate" CssClass="text1" runat="server" BackColor="DarkGray" ReadOnly="True"
                     Width="145px" onkeypress="return false">
        </asp:TextBox>
    </td>
    <td align="left">
        <asp:ImageButton ID="imgBtnCalendarPaymentReceivedDate" runat="server" alt="" src="images/Calendar.gif"
                         Style="vertical-align: top"/>
        <cc1:CalendarExtender runat="server" Enabled="True"
                              Format="MM/dd/yyyy" PopupButtonID="imgBtnCalendarPaymentReceivedDate" TargetControlID="txtPaymentReceivedDate">
        </cc1:CalendarExtender>
    </td>
</tr>
<tr>
    <td align="left" colspan="3">
        <asp:Label ID="lblChequeNo" runat="server" Text="Cheque No"></asp:Label>
        <asp:Label ID="lblChkNo" runat="server" ForeColor="Red" Text="*"></asp:Label>
    </td>
    <td class="columnseparator"></td>
    <td align="center" colspan="2">
        <asp:TextBox ID="txtChequeNo" CssClass="text1" runat="server" BackColor="DarkGray" ReadOnly="True"
                     Width="145px">
        </asp:TextBox>
    </td>
</tr>
<tr>
    <td align="left" colspan="3" nowrap="nowrap">
        <asp:Label ID="lblAmountReceivedFromInsurance" runat="server" Text="Amount Received From Insurance"></asp:Label>
        <asp:Label ID="lblamtrec" runat="server" ForeColor="Red" Text="*"></asp:Label>
    </td>
    <td class="columnseparator"></td>
    <td align="center" colspan="2">
        <asp:TextBox ID="txtAmountReceivedFromInsurance" CssClass="text1" runat="server" AutoPostBack="True"
                     BackColor="DarkGray" onkeypress="return isDecimalNumberKey(event);" OnTextChanged="txtAmountReceivedFromInsurance_TextChanged"
                     ReadOnly="True" Width="145px">
        </asp:TextBox>
    </td>
</tr>
<tr>
    <td align="left" colspan="3">
        Cost To Company
    </td>
    <td class="columnseparator"></td>
    <td align="center" colspan="2">
        <asp:TextBox ID="txtCostToCompany" runat="server" CssClass="text1" BackColor="DarkGray" onkeypress="return isDecimalNumberKey(event);"
                     ReadOnly="True" Width="145px">
        </asp:TextBox>
    </td>
    <td align="center">
        <asp:LinkButton ID="lbtnCostComparison" runat="server">Cost Comparison</asp:LinkButton>
    </td>
</tr>
<tr>
    <td align="right" colspan="3">
        <asp:Button ID="btSave" CssClass="button" Style="background-color: #4CAF50;" runat="server" OnClick="btSave_Click" Text="Save"/>
    </td>
    <td align="right" colspan="3">
        <asp:Button ID="btReset" CssClass="button" Style="background-color: red;" runat="server" OnClick="btReset_Click" Text="Reset"/>
    </td>
</tr>
<tr>
    <td align="left" colspan="3">
        &nbsp;
    </td>
</tr>
</table>

</td>
</tr>
</table>
<tr>
    <td class="rowseparator"></td>
</tr>
</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>