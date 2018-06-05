<%@ Page Language="C#" MasterPageFile="~/temp.master" AutoEventWireup="true" CodeFile="VehicleInsurance.aspx.cs" Inherits="VehicleInsurance" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<script type="text/javascript">

    function validation() {
        var id = document.getElementById('<%= ddlVehicleNo.ClientID %>');
        switch (id.selectedIndex) {
        case 0:
            alert("Please select Vehicle");
            id.focus();
            return false;
        }
        var district = document.getElementById('<%= txtDistrict.ClientID %>');
        var insuranceType = document.getElementById('<%= txtInsuranceType.ClientID %>');
        var insuranceTypeDdl = document.getElementById('<%= ddlInsuranceType.ClientID %>');
        var insuranceAgency = document.getElementById('<%= txtInsuranceAgency.ClientID %>');
        var insuranceAgencyDdl = document.getElementById('<%= ddlInsuranceAgency.ClientID %>');
        var insurancePolicyNo = document.getElementById('<%= txtInsurancePolicyNo.ClientID %>');
        var currentPolicyEndDate = document.getElementById('<%= txtCurrentPolicyEndDate.ClientID %>');
        var receiptNumber = document.getElementById('<%= txtReceiptNumber.ClientID %>');
        var feesPaid = document.getElementById('<%= txtFeesPaid.ClientID %>');
        var feesPaidDate = document.getElementById('<%= txtFeesPaidDate.ClientID %>');
        var policyStartDate = document.getElementById('<%= txtPolicyStartDate.ClientID %>');
        var policyValidityPeriod = document.getElementById('<%= ddlPolicyValidityPeriod.ClientID %>');
        var now = new Date();

        if (!RequiredValidation(district, "State Cannot be Blank"))
            return false;


        if (document.getElementById('<%= chkBoxChangeInsuranceDetails.ClientID %>').checked) {

            switch (insuranceTypeDdl.selectedIndex) {
            case 0:
                alert("Please select Insurance Type");
                insuranceTypeDdl.focus();
                return false;
            }

            switch (insuranceAgencyDdl.selectedIndex) {
            case 0:
                alert("Please select Insurance Agency");
                insuranceAgencyDdl.focus();
                return false;
            }
        } else {
            if (!RequiredValidation(insuranceType, "Insurance Type Cannot be Blank"))
                return false;
            if (!RequiredValidation(insuranceAgency, "Insurance Agency Cannot be Blank"))
                return false;
        }

        if (!RequiredValidation(insurancePolicyNo, "Insurance Policy Number Cannot be Blank"))
            return false;

        if (!RequiredValidation(currentPolicyEndDate, "Current Policy End Date Cannot be Blank"))
            return false;

        if (!isValidDate(currentPolicyEndDate.value)) {
            alert("Enter Valid Current Policy End Date");
            currentPolicyEndDate.focus();
            return false;
        }

        if (!RequiredValidation(receiptNumber, "Receipt Number Cannot be Blank"))
            return false;

        if (!RequiredValidation(feesPaid, "Fees Paid Cannot be Blank"))
            return false;

        if (!RequiredValidation(feesPaidDate, "Fees Paid Date Cannot be Blank"))
            return false;

        if (!isValidDate(feesPaidDate.value)) {
            alert("Enter Valid Fees Paid Date");
            feesPaidDate.focus();
            return false;
        }

        if (Date.parse(feesPaidDate.value) > Date.parse(now)) {
            alert("Fees Paid Date should not be greater than Current Date");
            policyStartDate.focus();
            return false;
        }

        if (!RequiredValidation(policyStartDate, "Policy Start Date Cannot be Blank"))
            return false;

        if (!isValidDate(policyStartDate.value)) {
            alert("Enter Valid Policy Start Date");
            policyStartDate.focus();
            return false;
        }

        if (Date.parse(policyStartDate.value) > Date.parse(now)) {
            alert("Policy Start Date should not be greater than Current Date");
            policyStartDate.focus();
            return false;
        }

        if (Date.parse(policyStartDate.value) < Date.parse(feesPaidDate.value)) {
            alert("Policy Start Date should not be less than Fees Paid Date");
            policyStartDate.focus();
            return false;
        }

        if (Date.parse(policyStartDate.value) <= Date.parse(currentPolicyEndDate.value)) {
            alert("Policy Start Date should  be greater than Current Policy End Date");
            policyStartDate.focus();
            return false;
        }

        switch (policyValidityPeriod.selectedIndex) {
        case 0:
            alert("Please select Policy Validity Period");
            policyValidityPeriod.focus();
            return false;
        }
        return true;
    }

</script>
<asp:UpdatePanel runat="server">
<ContentTemplate>
<script>
    function pageLoad() {
        $('#<%= ddlVehicleNo.ClientID %>').select2({
            disable_search_threshold: 5,
            search_contains: true,
            minimumResultsForSearch: 20,
            placeholder: "Select an option"
        });
    }
</script>
<legend align="center" style="color: brown">Vehicle Insurance</legend>
<div  class="dropdown">

<table >
    <tr>
        <td >
            <a href="VehicleInsuranceViewHistory.aspx" style="color: dodgerblue" ><h5 class="search_3" style="width:90px">View History</h5></a>
        </td>
    </tr>
</table>
<table class="table table-striped table-bordered table-hover" align="center">
<tr>
    <td class="rowseparator"></td>
</tr>
<tr>
<td >
<asp:Panel ID="pnlVehicleInsurance" runat="server" Width="900px" align="center">
    <table align="center">

        <tr>
            <td style="width: 296px"></td>
            <td style="width: 572px"></td>
            <td align="left" style="width: 244px">
                Vehicle No.<span style="color: Red">*</span>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlVehicleNo" runat="server" OnSelectedIndexChanged="ddlVehicleNo_SelectedIndexChanged" Width="150px">
                    <asp:ListItem Value="-1">--Select--</asp:ListItem>
                    <asp:ListItem Value="0">--Dummy--</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="2"></td>
            <td align="left" style="width: 244px">
                State<span style="color: Red">*</span>
            </td>
            <td align="left">
                <asp:TextBox ID="txtDistrict" CssClass="search_3" runat="server" BackColor="DarkGray" ReadOnly="True"
                             Width="150px">
                </asp:TextBox>
            </td>
            <td rowspan="3">
                <asp:CheckBox ID="chkBoxChangeInsuranceDetails" runat="server" AutoPostBack="True"
                              OnCheckedChanged="chkBoxChangeInsuranceDetails_CheckedChanged" Text="Change Insurance Details"
                              Width="180px"/>
            </td>
        </tr>
        <tr>
            <td colspan="2"></td>
            <td align="left" style="width: 244px">
                Insurance Type<span style="color: Red">*</span>
            </td>
            <td align="left">
                <asp:TextBox ID="txtInsuranceType" CssClass="search_3" runat="server" BackColor="Red" ReadOnly="True"
                             Width="150px">
                </asp:TextBox>
                <asp:DropDownList ID="ddlInsuranceType" CssClass="search_3" runat="server" OnSelectedIndexChanged="ddlInsuranceType_SelectedIndexChanged"
                                  Visible="False" Width="150px">
                    <asp:ListItem Value="-1">--Select--</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td colspan="2"></td>
            <td align="left" style="width: 244px">
                Insurance Agency<span style="color: Red">*</span>
            </td>
            <td align="left">
                <asp:TextBox ID="txtInsuranceAgency" CssClass="search_3" runat="server" BackColor="DarkGray" ReadOnly="True"
                             Width="145px">
                </asp:TextBox>
                <asp:DropDownList ID="ddlInsuranceAgency" CssClass="search_3" runat="server" OnSelectedIndexChanged="ddlInsuranceAgency_SelectedIndexChanged"
                                  Visible="False" Width="150px">
                    <asp:ListItem Value="-1">--Select--</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td colspan="2"></td>
            <td align="left" style="width: 244px">
                Insurance Policy No
            </td>
            <td align="left">
                <asp:TextBox ID="txtInsurancePolicyNo" class="text1" runat="server" BackColor="DarkGray" ReadOnly="True"
                             Width="145px" onkeypress="return alphanumeric_only(event);" onpaste="return false;"
                             MaxLength="20">
                </asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2"></td>
            <td align="left" style="width: 244px">
                Current Policy End Date<span style="color: Red">*</span>
            </td>
            <td align="left">
                <asp:TextBox ID="txtCurrentPolicyEndDate" class="text1" runat="server" BackColor="DarkGray" ReadOnly="True"
                             Width="145px">
                </asp:TextBox>
            </td>
            <td></td>
        </tr>
        <tr>
            <td colspan="2"></td>
            <td align="left" style="width: 244px">
                Receipt Number<span style="color: Red">*</span>
            </td>
            <td align="left">
                <asp:TextBox ID="txtReceiptNumber" CssClass="text1" runat="server" MaxLength="15" Width="145px" onkeypress="return alphanumeric_only(event);"></asp:TextBox>
            </td>
            <td></td>
        </tr>
        <tr>
            <td colspan="2"></td>
            <td align="left" style="width: 244px">
                Fees Paid<span style="color: Red">*</span>
            </td>
            <td align="left">
                <asp:TextBox ID="txtFeesPaid" runat="server" class="text1" MaxLength="10" onkeypress="return numericOnly(this);"
                             Width="145px">
                </asp:TextBox>
            </td>
            <td></td>
        </tr>
        <tr>
            <td colspan="2"></td>
            <td align="left" style="width: 244px">
                Fees Paid Date<span style="color: Red">*</span>
            </td>
            <td align="left">
                <asp:TextBox ID="txtFeesPaidDate" class="text1" runat="server" Width="145px" onkeypress="return false"
                             oncut="return false;" onpaste="return false;">
                </asp:TextBox>
                <cc1:CalendarExtender ID="calextFeesPaidDate" runat="server" Format="MM/dd/yyyy"
                                      PopupButtonID="imgBtnFeesPaidDate" TargetControlID="txtFeesPaidDate">
                </cc1:CalendarExtender>
            </td>
            <td>
                <asp:ImageButton ID="imgBtnFeesPaidDate" runat="server" alt="" src="images/Calendar.gif"
                                 Style="vertical-align: top"/>
            </td>
        </tr>
        <tr>
            <td colspan="2"></td>
            <td align="left" style="width: 244px">
                Policy Start Date<span style="color: Red">*</span>
            </td>
            <td align="left">
                <asp:TextBox ID="txtPolicyStartDate" class="text1" runat="server" AutoPostBack="true" OnTextChanged="txtPolicyStartDate_TextChanged"
                             Width="145px" onkeypress="return false" oncut="return false;" onpaste="return false;">
                </asp:TextBox>
                <cc1:CalendarExtender ID="calextPolicyStartDate" runat="server" Format="MM/dd/yyyy"
                                      PopupButtonID="imgBtnPolicyStartDate" TargetControlID="txtPolicyStartDate">
                </cc1:CalendarExtender>
            </td>
            <td>
                <asp:ImageButton ID="imgBtnPolicyStartDate" runat="server" alt="" src="images/Calendar.gif"
                                 Style="vertical-align: top"/>
            </td>
        </tr>
        <tr>
            <td colspan="2"></td>
            <td align="left" style="width: 244px" nowrap="nowrap">
                Policy Validity Period<span style="color: Red">*</span>
            </td>
            <td align="left">
                <asp:DropDownList class="text1" ID="ddlPolicyValidityPeriod" runat="server" AutoPostBack="True"
                                  OnSelectedIndexChanged="ddlPolicyValidityPeriod_SelectedIndexChanged" Width="150px">
                    <asp:ListItem Value="-1">--Select--</asp:ListItem>
                    <asp:ListItem Value="3">3 Month</asp:ListItem>
                    <asp:ListItem Value="6">6 Month</asp:ListItem>
                    <asp:ListItem Value="9">9 Month</asp:ListItem>
                    <asp:ListItem Value="12">1 Year</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td></td>
        </tr>
        <tr>
            <td colspan="2"></td>
            <td align="left" style="width: 244px">
                Policy End Date
            </td>
            <td align="left">
                <asp:TextBox ID="txtPolicyEndDate" runat="server" BackColor="DarkGray" ReadOnly="True"
                             Width="145px">
                </asp:TextBox>
            </td>
            <td></td>
        </tr>
        <tr>
            <td colspan="2">
                &nbsp;
            </td>
            <td align="left" style="width: 244px">
                &nbsp;
            </td>
            <td align="left">
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="2"></td>
            <td align="center" style="width: 244px">
                <asp:Button ID="btSave" CssClass="form-submit-button" runat="server" OnClick="btSave_Click" Text="Save"/>
            </td>
            <td align="left">
                <asp:Button ID="btReset" CssClass="form-reset-button" runat="server" OnClick="btReset_Click" Text="Reset" />
            </td>
        </tr>
    </table>
</asp:Panel>
</td>
</tr>
<tr>
    <td class="rowseparator" style="width: 589px"></td>
</tr>
<asp:HiddenField ID="vehiclePurchaseDate" runat="server"/>
</table>
</div>
</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>