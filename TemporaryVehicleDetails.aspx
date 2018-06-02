<%@ Page Language="C#" MasterPageFile="~/temp.master" AutoEventWireup="true" CodeFile="TemporaryVehicleDetails.aspx.cs" Inherits="TemporaryVehicleDetails" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<link href="css/newStyles.css" rel="stylesheet"/>
<script type="text/javascript">
    var now = new Date();
    var inspectedDate;
    var purchaseDate;
    var manufacturingDate;

    function validation(obj, id) {
        var engineNo = document.getElementById('<%= txtEngineNo.ClientID %>');
        var chassisNo = document.getElementById('<%= txtChassisNo.ClientID %>');
        var vehicleNo = document.getElementById('<%= txtVehicleNo.ClientID %>');
        var inspectedBy = document.getElementById('<%= txtInspectedBy.ClientID %>');
        var inspectedDate = document.getElementById('<%= txtInspectedDate.ClientID %>');
        var vehicleStatus = document.getElementById('<%= ddlVehicleStatus.ClientID %>');
        if (!RequiredValidation(engineNo, "Engine Number Cannot be Blank"))
            return false;

        if (!RequiredValidation(chassisNo, "Chassis Number cannot be Blank"))
            return false;

        if (!RequiredValidation(vehicleNo, "Vehicle Number cannot be Blank"))
            return false;

        if (!isValidVehicleNumber(vehicleNo.value)) {
            vehicleNo.value = "";
            vehicleNo.focus();
            return false;
        }

        if (!RequiredValidation(inspectedBy, "Inspected By cannot be Blank"))
            return false;

        if (!RequiredValidation(inspectedDate, "Inspected Date cannot be Blank"))
            return false;

        if (trim(inspectedDate.value) !== "") {
            if (!isValidDate(inspectedDate.value)) {
                alert("Enter Valid Date");
                inspectedDate.focus();
                return false;
            }
        }
        inspectedDate = inspectedDate.value;

        if (Date.parse(inspectedDate.value) > Date.parse(now)) {
            alert("Inspected Date should not be greater than Current Date");
            inspectedDate.focus();
            return false;
        }

        switch (vehicleStatus.selectedIndex) {
        case 0:
            alert("Please select Vehicle Status");
            vehicleStatus.focus();
            return false;
        }
        return true;
    }


    var stepNo = 0;

    function previousValidation() {
        if (stepNo > 0) {
            stepNo--;
        }
    }

    function stepvalidation() {
        ////-----------------Start of Validation of General Information----------------////
        var purchaseDate;
        switch (stepNo) {
        case 0:
            var vehicleModel = document.getElementById('<%= ddlVehicleModel.ClientID %>');
            var kmpl = document.getElementById('<%= txtKmpl.ClientID %>');
            var vehicleType = document.getElementById('<%= ddlVehicleType.ClientID %>');
            var vehicleEmissionType = document.getElementById('<%= txtVehicleEmissionType.ClientID %>');
            purchaseDate = document.getElementById('<%= txtPurchaseDate.ClientID %>');
            var ownerName = document.getElementById('<%= txtOwnerName.ClientID %>');
            var manufacturerName = document.getElementById('<%= ddlManufacturerName.ClientID %>');
            var vehicleCost = document.getElementById('<%= txtVehicleCost.ClientID %>');
            var manufacturingDate = document.getElementById('<%= txtManufacturingDate.ClientID %>');
            var engineCapacity = document.getElementById('<%= txtEngineCapacity.ClientID %>');
            var fuelType = document.getElementById('<%= ddlFuelType.ClientID %>');
            switch (vehicleModel.selectedIndex) {
            case 0:
                alert("Please select the Vehicle Model");
                vehicleModel.focus();
                return false;
            }
            if (!RequiredValidation(kmpl, "Kmpl cannot be Blank"))
                return false;
            switch (vehicleType.selectedIndex) {
            case 0:
                alert("Please select the Vehicle Type");
                vehicleType.focus();
                return false;
            }
            if (!RequiredValidation(vehicleEmissionType, "VehicleEmission Type cannot be Blank"))
                return false;
            if (!RequiredValidation(purchaseDate, "Purchase Date cannot be Blank"))
                return false;
            if (trim(purchaseDate.value) !== "") {
                if (!isValidDate(purchaseDate.value)) {
                    alert("Enter the valid Purchase Date");
                    purchaseDate.focus();
                    return false;
                }
            }
            purchaseDate = purchaseDate.value;
            if (Date.parse(purchaseDate.value) > Date.parse(now)) {
                alert("Purchase Date should not be greater than Current Date");
                purchaseDate.focus();
                return false;
            }
            if (Date.parse(purchaseDate.value) > Date.parse(inspectedDate)) {
                alert("Purchase Date should not be greater than Inspected Date.(Inspected Date-" +
                    inspectedDate +
                    ")");
                purchaseDate.focus();
                return false;
            }
            if (!RequiredValidation(ownerName, "OwnerName cannot be Blank"))
                return false;
            switch (manufacturerName.selectedIndex) {
            case 0:
                alert("Please select the Manufacturer Name");
                manufacturerName.focus();
                return false;
            }
            if (!RequiredValidation(vehicleCost, "Vehicle Cost cannot be Blank"))
                return false;
            if (!RequiredValidation(manufacturingDate, "Manufacturing Date cannot be Blank"))
                return false;
            if (trim(manufacturingDate.value) !== "") {
                if (!isValidDate(manufacturingDate.value)) {
                    alert("Enter the valid Manufacturing Date");
                    manufacturingDate.focus();
                    return false;
                }
            }
            if (Date.parse(manufacturingDate.value) > Date.parse(now)) {
                alert("Manufacturing Date should not be greater than Current Date");
                manufacturingDate.focus();
                return false;
            }
            if (Date.parse(manufacturingDate.value) > Date.parse(purchaseDate)) {
                alert("Manufacturing Date should not be greater than Purchase Date.(Purchase Date-" +
                    purchaseDate +
                    ")");
                manufacturingDate.focus();
                return false;
            }
            if (!RequiredValidation(engineCapacity, "Engine Capacity cannot be Blank"))
                return false;
            switch (fuelType.selectedIndex) {
            case 0:
                alert("Please select the Fuel Type");
                fuelType.focus();
                return false;
            }
            stepNo++;
            break;
        case 1:
            var district = document.getElementById('<%= ddlDistrict.ClientID %>');
            var inPolicyNo = document.getElementById('<%= txtInPolicyNo.ClientID %>');
            var insType = document.getElementById('<%= ddlInsType.ClientID %>');
            var insFee = document.getElementById('<%= txtInsFee.ClientID %>');
            var currentPolicyEndDate = document.getElementById('<%= txtCurrentPolicyEndDate.ClientID %>');
            var insuranceReceiptNo = document.getElementById('<%= txtInsuranceReceiptNo.ClientID %>');
            var insuranceFeesPaidDate = document.getElementById('<%= txtInsuranceFeesPaidDate.ClientID %>');
            var agency = document.getElementById('<%= ddlAgency.ClientID %>');
            var valiSDate = document.getElementById('<%= txtValiSDate.ClientID %>');
            var policyValidityPeriod = document.getElementById('<%= ddlPolicyValidityPeriod.ClientID %>');
            switch (district.selectedIndex) {
            case 0:
                alert("Please select the District");
                district.focus();
                return false;
            }
            if (!RequiredValidation(inPolicyNo, "Insurance Policy No cannot be Blank"))
                return false;
            switch (insType.selectedIndex) {
            case 0:
                alert("Please select the Insurance Type");
                insType.focus();
                return false;
            }
            if (!RequiredValidation(insFee, "Insurance Fee cannot be Blank"))
                return false;
            if (!RequiredValidation(currentPolicyEndDate, "Current Policy End Date cannot be Blank"))
                return false;
            if (trim(currentPolicyEndDate.value) !== "") {
                if (!isValidDate(currentPolicyEndDate.value)) {
                    alert("Enter the valid Current Policy End Date");
                    currentPolicyEndDate.focus();
                    return false;
                }
            }
            if (Date.parse(currentPolicyEndDate.value) < Date.parse(purchaseDate)) {
                alert("Current Policy End Date should be greater than Purchase Date.(Purchase Date-" +
                    purchaseDate +
                    ")");
                currentPolicyEndDate.focus();
                return false;
            }
            if (!RequiredValidation(insuranceReceiptNo, "Insurance Receipt No cannot be Blank"))
                return false;
            if (!RequiredValidation(insuranceFeesPaidDate, "Insurance Fees Paid Date cannot be Blank"))
                return false;
            if (trim(insuranceFeesPaidDate.value) !== "") {
                if (!isValidDate(insuranceFeesPaidDate.value)) {
                    alert("Enter The valid Insurance Fees Paid Date");
                    insuranceFeesPaidDate.focus();
                    return false;
                }
            }
            if (Date.parse(insuranceFeesPaidDate.value) > Date.parse(now)) {
                alert("Insurance Fees Paid Date should not be greater than Current Date");
                insuranceFeesPaidDate.focus();
                return false;
            }
            if (Date.parse(insuranceFeesPaidDate.value) < Date.parse(purchaseDate)) {
                alert("Insurance Fees Paid Date should be greater than Purchase Date.(Purchase Date-" +
                    purchaseDate +
                    ")");
                insuranceFeesPaidDate.focus();
                return false;
            }
            switch (agency.selectedIndex) {
            case 0:
                alert("Please select the Agency");
                agency.focus();
                return false;
            }
            if (!RequiredValidation(valiSDate, "Validity Start Date cannot be Blank"))
                return false;
            if (trim(valiSDate.value) !== "") {
                if (!isValidDate(valiSDate.value)) {
                    alert("Enter The Valid Start Date");
                    valiSDate.focus();
                    return false;
                }
            }
            if (Date.parse(valiSDate.value) < Date.parse(insuranceFeesPaidDate.value)) {
                alert("Validity Start Date should not be less than Insurance Fees Paid Date");
                valiSDate.focus();
                return false;
            }
            if (Date.parse(valiSDate.value) <= Date.parse(currentPolicyEndDate.value)) {
                alert("Validity Start Date should not be less than Current Policy End Date");
                valiSDate.focus();
                return false;
            }
            switch (policyValidityPeriod.selectedIndex) {
            case 0:
                alert("Please Select The Policy Validity Period");
                policyValidityPeriod.focus();
                return false;
            }
            stepNo++;
            break;
        case 2:
            var fl = document.getElementById('<%= txtFL.ClientID %>');
            var fr = document.getElementById('<%= txtFR.ClientID %>');
            var rl = document.getElementById('<%= txtRL.ClientID %>');
            var rr = document.getElementById('<%= txtRR.ClientID %>');
            var spareWheel = document.getElementById('<%= txtSpareWheel.ClientID %>');
            var tyreMakeFl = document.getElementById('<%= ddlTyreMakeFL.ClientID %>');
            var tyreModelSizeFl = document.getElementById('<%= ddlModelSizeFL.ClientID %>');
            var tyreMakeFr = document.getElementById('<%= ddlTyreMakeFR.ClientID %>');
            var tyreModelSizeFr = document.getElementById('<%= ddlModelSizeFR.ClientID %>');
            var tyreMakeRl = document.getElementById('<%= ddlTyreMakeRL.ClientID %>');
            var tyreModelSizeRl = document.getElementById('<%= ddlModelSizeRL.ClientID %>');
            var tyreMakeRr = document.getElementById('<%= ddlTyreMakeRR.ClientID %>');
            var tyreModelSizeRr = document.getElementById('<%= ddlModelSizeRR.ClientID %>');
            var tyreMakeSpareWheel = document.getElementById('<%= ddlTyreMakeSpareWheel.ClientID %>');
            var tyreModelSizeSpareWheel = document.getElementById('<%= ddlModelSizeSpareWheel.ClientID %>');
            var odometerReading = document.getElementById('<%= txtOdometerReading.ClientID %>');
            var tyre = [fl.value, fr.value, rl.value, rr.value, spareWheel.value];
            if (!RequiredValidation(fl, "Front Left cannot be Blank"))
                return false;
            if (!RequiredValidation(fr, "Front Right cannot be Blank"))
                return false;
            if (!RequiredValidation(rl, "Rear Left cannot be Blank"))
                return false;
            if (!RequiredValidation(rr, "Rear Right cannot be Blank"))
                return false;
            if (!RequiredValidation(spareWheel, "Spare Wheel cannot be Blank"))
                return false;
            for (var i = 0; i < (tyre.length - 1); i++) {
                for (var j = i + 1; j < tyre.length; j++) {
                    switch (tyre[i]) {
                    case tyre[j]:
                        alert("Tyre numbers should be unique");
                        switch (j) {
                        case 1:
                            fr.focus();
                            break;
                        case 2:
                            rl.focus();
                            break;
                        case 3:
                            rr.focus();
                            break;
                        default:
                            spareWheel.focus();
                            break;
                        }
                        return false;
                    }
                }
            }
            switch (tyreMakeFl.selectedIndex) {
            case 0:
                alert("Please Select The Tyre Make FL");
                tyreMakeFl.focus();
                return false;
            }
            switch (tyreModelSizeFl.selectedIndex) {
            case 0:
                alert("Please Select The Tyre Model Size FL");
                tyreModelSizeFl.focus();
                return false;
            }
            switch (tyreMakeFr.selectedIndex) {
            case 0:
                alert("Please Select The Tyre Make FR");
                tyreMakeFr.focus();
                return false;
            }
            switch (tyreModelSizeFr.selectedIndex) {
            case 0:
                alert("Please Select The Tyre Model Size FL");
                tyreModelSizeFr.focus();
                return false;
            }
            switch (tyreMakeRl.selectedIndex) {
            case 0:
                alert("Please Select The Tyre Make RL");
                tyreMakeRl.focus();
                return false;
            }
            switch (tyreModelSizeRl.selectedIndex) {
            case 0:
                alert("Please Select The Tyre Model Size RL");
                tyreModelSizeRl.focus();
                return false;
            }
            switch (tyreMakeRr.selectedIndex) {
            case 0:
                alert("Please Select The Tyre Make RR");
                tyreMakeRr.focus();
                return false;
            }
            switch (tyreModelSizeRr.selectedIndex) {
            case 0:
                alert("Please Select The Tyre Model Size RR");
                tyreModelSizeRr.focus();
                return false;
            }
            switch (tyreMakeSpareWheel.selectedIndex) {
            case 0:
                alert("Please Select The Tyre Make SpareWheel");
                tyreMakeSpareWheel.focus();
                return false;
            }
            switch (tyreModelSizeSpareWheel.selectedIndex) {
            case 0:
                alert("Please Select The Tyre Model Size SpareWheel");
                tyreModelSizeSpareWheel.focus();
                return false;
            }
            if (!RequiredValidation(odometerReading, "Odometer Reading cannot be Blank"))
                return false;
            stepNo++;
            break;
        }

        return true;
    }

    function finalStepValidation() {
        var battery1 = document.getElementById('<%= txtBattery1.ClientID %>');
        var battery2 = document.getElementById('<%= txtBattery2.ClientID %>');
        var batteryMakeBattery1 = document.getElementById('<%= ddlBatteryMakeBattery1.ClientID %>');
        var batteryModelCapacityBattery1 = document.getElementById('<%= ddlModelCapacityBattery1.ClientID %>');
        var batteryMakeBattery2 = document.getElementById('<%= ddlBatteryMakeBattery2.ClientID %>');
        var batteryModelCapacityBattery2 = document.getElementById('<%= ddlModelCapacityBattery2.ClientID %>');

        if (!RequiredValidation(battery1, "Battery1 cannot be Blank"))
            return false;

        if (!RequiredValidation(battery2, "Battery2 cannot be Blank"))
            return false;

        switch (battery1.value) {
        case battery2.value:
            alert("Battery Numbers should be unique");
            battery2.focus();
            return false;
        }

        switch (batteryMakeBattery1.selectedIndex) {
        case 0:
            alert("Please Select The Battery Make Battery1");
            batteryMakeBattery1.focus();
            return false;
        }
        switch (batteryModelCapacityBattery1.selectedIndex) {
        case 0:
            alert("Please Select The Battery Model Capacity Battery1");
            batteryModelCapacityBattery1.focus();
            return false;
        }
        switch (batteryMakeBattery2.selectedIndex) {
        case 0:
            alert("Please Select The Battery Make Battery2");
            batteryMakeBattery2.focus();
            return false;
        }
        switch (batteryModelCapacityBattery2.selectedIndex) {
        case 0:
            alert("Please Select The Battery Model Capacity Battery2");
            batteryModelCapacityBattery2.focus();
            return false;
        }
        return true;
    }

    function isValidVehicleNumber(vehicleNo) {
        if (!vehicleNo.match(/^[A-Z]{2}[0-9]{2}[A-Z]{1,2}[0-9]{1,4}$/)) {
            return false;
        } else {
            return true;
        }
    }

</script>
<asp:UpdatePanel runat="server">
<ContentTemplate>
<asp:Panel ID="pnlTemporaryVehicleDetails" runat="server">

<asp:HiddenField runat="server"/>
&nbsp;&nbsp;&nbsp;
<asp:Wizard ID="tempVehDetWizard" runat="server" Height="224px" Width="664px" ActiveStepIndex="0"
            OnFinishButtonClick="Wizard1_FinishButtonClick" DisplaySideBar="False">
<StartNavigationTemplate>
    <asp:Button ID="StartNextButton" runat="server" CommandName="MoveNext" Text="Next"/>
</StartNavigationTemplate>
<WizardSteps>
<asp:WizardStep ID="WizardStep1" runat="server" Title="Invoice and Base Details ">
    <table style="width: 100%;">
        <tr>
            <td colspan="6" style="text-align: center">
                <b>Invoice and Base Details </b>
            </td>
        </tr>
        <tr>
            <td class="rowseparator" style="width: 177px"></td>
        </tr>
        <tr>
            <td style="width: 177px">
                &nbsp;
            </td>
            <td style="width: 198px">
                Engine No<span style="color: #FF0000">*</span>
            </td>
            <td style="width: 88px" colspan="2">
                <asp:TextBox ID="txtEngineNo" runat="server" CssClass="text1" Width="100px" MaxLength="15" onkeypress="return alphanumeric_only(event);"></asp:TextBox>
            </td>
            <td colspan="2">
                &nbsp;&nbsp;&nbsp;
            </td>
        </tr>
        <tr>
            <td class="rowseparator" style="width: 177px"></td>
        </tr>
        <tr>
            <td style="width: 177px">
                &nbsp;
            </td>
            <td style="width: 198px">
                Chassis No<span style="color: #FF0000">*</span>
            </td>
            <td style="width: 88px" nowrap="nowrap" colspan="2">

                <asp:TextBox ID="txtChassisNo" CssClass="text1" runat="server" Width="100px" MaxLength="15" onkeypress="return alphanumeric_only(event);"></asp:TextBox>
            </td>
            <td colspan="2">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="rowseparator" style="width: 177px"></td>
        </tr>
        <tr>
            <td style="width: 177px">
                &nbsp;
            </td>
            <td style="width: 198px">
                Vehicle No<span style="color: #FF0000">*</span>
            </td>
            <td style="width: 88px" colspan="2">
                <asp:TextBox ID="txtVehicleNo" CssClass="text1" runat="server" Width="100px" MaxLength="10"
                             onkeypress="return alphanumeric_only(event);">
                </asp:TextBox>
            </td>
            <td colspan="2">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="rowseparator" style="width: 177px"></td>
        </tr>
        <tr>
            <td style="width: 177px">
                &nbsp;
            </td>
            <td style="width: 198px">
                Inspected By<span style="color: #FF0000">*</span>
            </td>
            <td style="width: 88px" colspan="2">
                <asp:TextBox ID="txtInspectedBy" CssClass="text1" runat="server" Width="100px" MaxLength="35" onkeypress="return alpha_only_withspace(event);"></asp:TextBox>
            </td>
            <td colspan="2">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="rowseparator" style="width: 177px"></td>
        </tr>
        <tr>
            <td style="width: 177px">
                &nbsp;
            </td>
            <td style="width: 198px">
                Inspected Date<span style="color: #FF0000">*</span>
            </td>
            <td style="width: 88px">
                <asp:TextBox ID="txtInspectedDate" CssClass="text1" runat="server" Width="100px" oncut="return false;"
                             onpaste="return false;" onkeypress="return false">
                </asp:TextBox>
                <cc1:CalendarExtender runat="server" TargetControlID="txtInspectedDate"
                                      PopupButtonID="imgBtnCalendarInspectedDate" Format="MM/dd/yyyy">
                </cc1:CalendarExtender>
            </td>
            <td style="width: 88px">
                <asp:ImageButton ID="imgBtnCalendarInspectedDate" runat="server" alt="" Height="19px"
                                 src="images/Calendar.gif" Style="vertical-align: top" Width="19px"/>
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="rowseparator" style="width: 177px"></td>
        </tr>

        <tr>
            <td style="width: 177px">
                &nbsp;
            </td>
            <td style="width: 198px">
                Vehicle Status
            </td>
            <td style="width: 88px" colspan="2">
                <asp:DropDownList ID="ddlVehicleStatus" runat="server" CssClass="text1" Width="100px">
                    <asp:ListItem Value="-1">--Select--</asp:ListItem>
                    <asp:ListItem Value="1">Active</asp:ListItem>
                    <asp:ListItem Value="0">Inactive</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td colspan="2">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="rowseparator" style="width: 177px"></td>
        </tr>
    </table>
</asp:WizardStep>
<asp:WizardStep ID="WizardStep2" runat="server" Title="General Information ">
<table style="width: 100%;">
<tr>
    <td colspan="4" style="text-align: center">
        <b>General Information </b>
    </td>
</tr>
<tr>
    <td class="rowseparator" style="width: 114px"></td>
</tr>
<tr>
    <td style="width: 114px">
        &nbsp;
    </td>
    <td>
        Vehicle Model<span style="color: Red">*</span>
    </td>
    <td>
        <asp:DropDownList ID="ddlVehicleModel" CssClass="text1" runat="server" Width="105px">
            <asp:ListItem Value="-1">--Select--</asp:ListItem>
            <asp:ListItem Value="0">Dummy</asp:ListItem>
        </asp:DropDownList>
    </td>
    <td>
        &nbsp;
    </td>
</tr>
<tr>
    <td class="rowseparator" style="width: 114px"></td>
</tr>
<tr>
    <td style="width: 114px">
        &nbsp;
    </td>
    <td>
        KMPL<span style="color: Red">*</span>
    </td>
    <td>
        <asp:TextBox ID="txtKmpl" CssClass="text1" runat="server" Width="100px" onkeypress="return isDecimalNumberKey(event);"
                     MaxLength="5">
        </asp:TextBox>
    </td>
    <td>
        &nbsp;
    </td>
</tr>
<tr>
    <td class="rowseparator" style="width: 114px"></td>
</tr>
<tr>
    <td style="width: 114px">
        &nbsp;
    </td>
    <td>
        Vehicle Type<span style="color: Red">*</span>
    </td>
    <td>
        <asp:DropDownList ID="ddlVehicleType" CssClass="text1" runat="server" Width="105px">
            <asp:ListItem Value="-1">--Select--</asp:ListItem>
            <asp:ListItem Value="0">Dummy</asp:ListItem>
        </asp:DropDownList>
    </td>
    <td>
        &nbsp;
    </td>
</tr>
<tr>
    <td class="rowseparator" style="width: 114px"></td>
</tr>
<tr>
    <td style="width: 114px">
        &nbsp;
    </td>
    <td>
        Vehicle Emission Type<span style="color: Red">*</span>
    </td>
    <td>
        <asp:TextBox ID="txtVehicleEmissionType" CssClass="text1" runat="server" Width="100px" MaxLength="20"
                     onkeypress="return alpha_only_withspace(event);">
        </asp:TextBox>
    </td>
    <td>
        &nbsp;
    </td>
</tr>
<tr>
    <td class="rowseparator" style="width: 114px"></td>
</tr>
<tr>
    <td style="width: 114px">
        &nbsp;
    </td>
    <td>
        Purchase Date<span style="color: Red">*</span>
    </td>
    <td>
        <asp:TextBox ID="txtPurchaseDate" CssClass="text1" runat="server" Width="100px" onkeypress="return false" oncut="return false;"
                     onpaste="return false;">
        </asp:TextBox>
        <asp:ImageButton ID="imgBtnPurchaseDate" runat="server" alt="" src="images/Calendar.gif"
                         Style="vertical-align: top"/>
        <cc1:CalendarExtender runat="server" TargetControlID="txtPurchaseDate"
                              PopupButtonID="imgBtnPurchaseDate" Format="MM/dd/yyyy" Enabled="True">
        </cc1:CalendarExtender>
    </td>
    <td>
        &nbsp;
    </td>
</tr>
<tr>
    <td class="rowseparator" style="width: 114px"></td>
</tr>
<tr>
    <td style="width: 114px">
        &nbsp;
    </td>
    <td>
        Owner Name<span style="color: Red">*</span>
    </td>
    <td>
        <asp:TextBox ID="txtOwnerName" CssClass="text1" runat="server" Width="100px" MaxLength="35" onkeypress="return alpha_only_withspace(event);"></asp:TextBox>
    </td>
    <td>
        &nbsp;
    </td>
</tr>
<tr>
    <td class="rowseparator" style="width: 114px"></td>
</tr>
<tr>
    <td style="width: 114px">
        &nbsp;
    </td>
    <td>
        Manufacturer Name<span style="color: Red">*</span>
    </td>
    <td>
        <asp:DropDownList ID="ddlManufacturerName" CssClass="text1" runat="server" Width="105px">
            <asp:ListItem Value="-1">--Select--</asp:ListItem>
            <asp:ListItem Value="0">Dummy</asp:ListItem>
        </asp:DropDownList>
    </td>
    <td>
        &nbsp;
    </td>
</tr>
<tr>
    <td class="rowseparator" style="width: 114px"></td>
</tr>
<tr>
    <td style="width: 114px">
        &nbsp;
    </td>
    <td>
        Vehicle Cost<span style="color: Red">*</span>
    </td>
    <td>
        <asp:TextBox ID="txtVehicleCost" CssClass="text1" runat="server" Width="100px" onkeypress="return isDecimalNumberKey(event);"
                     MaxLength="11">
        </asp:TextBox>
    </td>
    <td>
        &nbsp;
    </td>
</tr>
<tr>
    <td class="rowseparator" style="width: 114px"></td>
</tr>
<tr>
    <td style="width: 114px">
        &nbsp;
    </td>
    <td>
        Manufacturing Date<span style="color: Red">*</span>
    </td>
    <td>
        <asp:TextBox ID="txtManufacturingDate" runat="server" Width="100px" onkeypress="return false" oncut="return false;"
                     onpaste="return false;">
        </asp:TextBox>
        <asp:ImageButton ID="imgBtnManufacturingDate" runat="server" alt="" src="images/Calendar.gif"
                         Style="vertical-align: top"/>
        <cc1:CalendarExtender runat="server" TargetControlID="txtManufacturingDate"
                              PopupButtonID="imgBtnManufacturingDate" Format="MM/dd/yyyy" Enabled="True">
        </cc1:CalendarExtender>
    </td>
    <td>
        &nbsp;
    </td>
</tr>
<tr>
    <td class="rowseparator" style="width: 114px"></td>
</tr>
<tr>
    <td style="width: 114px">
        &nbsp;
    </td>
    <td>
        Engine Capacity<span style="color: Red">*</span>
    </td>
    <td>
        <asp:TextBox ID="txtEngineCapacity" runat="server" Width="100px" onkeypress="return alphanumeric_only(event);"
                     MaxLength="8">
        </asp:TextBox>
    </td>
    <td>
        &nbsp;
    </td>
</tr>
<tr>
    <td class="rowseparator" style="width: 114px"></td>
</tr>
<tr>
    <td style="width: 114px">
        &nbsp;
    </td>
    <td>
        Fuel Type<span style="color: Red">*</span>
    </td>
    <td>
        <asp:DropDownList ID="ddlFuelType" runat="server" Width="105px">
            <asp:ListItem Value="-1">--Select--</asp:ListItem>
            <asp:ListItem Value="0">Dummy</asp:ListItem>
        </asp:DropDownList>
    </td>
    <td>
        &nbsp;
    </td>
</tr>
<tr>
    <td class="rowseparator" style="width: 114px"></td>
</tr>
</table>
</asp:WizardStep>
<asp:WizardStep runat="server" Title="Insurance Information ">
<table style="width: 100%">
<tr>
    <td colspan="4" style="text-align: center">
        <b>Insurance Information</b>
    </td>
</tr>
<tr>
    <td class="rowseparator" style="width: 130px"></td>
</tr>
<tr>
    <td style="width: 130px">
        &nbsp;
    </td>
    <td>
        District<span style="color: Red">*</span>
    </td>
    <td>
        <asp:DropDownList ID="ddlDistrict" runat="server" Width="105px">
            <asp:ListItem Value="-1">--Select--</asp:ListItem>
            <asp:ListItem Value="0">Dummy</asp:ListItem>
        </asp:DropDownList>
    </td>
    <td>
        &nbsp;
    </td>
</tr>
<tr>
    <td class="rowseparator" style="width: 130px"></td>
</tr>
<tr>
    <td style="width: 130px">
        &nbsp;
    </td>
    <td>
        Insurance Policy No<span style="color: Red">*</span>
    </td>
    <td>
        <asp:TextBox ID="txtInPolicyNo" runat="server" Width="100px" MaxLength="15" onkeypress="return alphanumeric_only(event);"></asp:TextBox>
    </td>
    <td>
        &nbsp;
    </td>
</tr>
<tr>
    <td class="rowseparator" style="width: 130px"></td>
</tr>
<tr>
    <td style="width: 130px">
        &nbsp;
    </td>
    <td>
        Insurance Type<span style="color: Red">*</span>
    </td>
    <td>
        <asp:DropDownList ID="ddlInsType" runat="server" Width="105px">
            <asp:ListItem Value="-1">--Select--</asp:ListItem>
            <asp:ListItem Value="0">Dummy</asp:ListItem>
        </asp:DropDownList>
    </td>
    <td>
        &nbsp;
    </td>
</tr>
<tr>
    <td class="rowseparator" style="width: 130px"></td>
</tr>
<tr>
    <td style="width: 130px">
        &nbsp;
    </td>
    <td>
        Insurance Fee<span style="color: Red">*</span>
    </td>
    <td>
        <asp:TextBox ID="txtInsFee" runat="server" Width="100px" onkeypress="return isDecimalNumberKey(event);"
                     MaxLength="10">
        </asp:TextBox>
    </td>
    <td>
        &nbsp;
    </td>
</tr>
<tr>
    <td class="rowseparator" style="width: 130px"></td>
</tr>
<tr>
    <td style="width: 130px">
        &nbsp;
    </td>
    <td>
        Current Policy End Date<span style="color: Red">*</span>
    </td>
    <td>
        <asp:TextBox ID="txtCurrentPolicyEndDate" runat="server" Width="100px" onkeypress="return false" oncut="return false;"
                     onpaste="return false;">
        </asp:TextBox>
        <asp:ImageButton ID="imgBtnCurrentPolicyEndDate" runat="server" alt="" src="images/Calendar.gif"
                         Style="vertical-align: top"/>
        <cc1:CalendarExtender runat="server" TargetControlID="txtCurrentPolicyEndDate"
                              PopupButtonID="imgBtnCurrentPolicyEndDate" Format="MM/dd/yyyy" Enabled="True">
        </cc1:CalendarExtender>
    </td>
    <td>
        &nbsp;
    </td>
</tr>
<tr>
    <td class="rowseparator" style="width: 130px"></td>
</tr>
<tr>
    <td style="width: 130px">
        &nbsp;
    </td>
    <td>
        Insurance Receipt No<span style="color: Red">*</span>
    </td>
    <td>
        <asp:TextBox ID="txtInsuranceReceiptNo" runat="server" Width="100px" MaxLength="15"
                     onkeypress="return alphanumeric_only(event);">
        </asp:TextBox>
    </td>
    <td>
        &nbsp;
    </td>
</tr>
<tr>
    <td class="rowseparator" style="width: 130px"></td>
</tr>
<tr>
    <td style="width: 130px">
        &nbsp;
    </td>
    <td>
        Insurance Fees Paid Date
    </td>
    <td>
        <asp:TextBox ID="txtInsuranceFeesPaidDate" runat="server" Width="100px" onkeypress="return false" oncut="return false;"
                     onpaste="return false;">
        </asp:TextBox>
        <asp:ImageButton ID="imgBtnInsuranceFeesPaidDate" runat="server" alt="" src="images/Calendar.gif"
                         Style="vertical-align: top"/>
        <cc1:CalendarExtender runat="server" TargetControlID="txtInsuranceFeesPaidDate"
                              PopupButtonID="imgBtnInsuranceFeesPaidDate" Format="MM/dd/yyyy" Enabled="True">
        </cc1:CalendarExtender>
    </td>
    <td>
        &nbsp;
    </td>
</tr>
<tr>
    <td class="rowseparator" style="width: 130px"></td>
</tr>
<tr>
    <td style="width: 130px">
        &nbsp;
    </td>
    <td>
        Agency<span style="color: Red">*</span>
    </td>
    <td>
        <asp:DropDownList ID="ddlAgency" runat="server" Width="105px">
            <asp:ListItem Value="-1">--Select--</asp:ListItem>
            <asp:ListItem Value="0">Dummy</asp:ListItem>
        </asp:DropDownList>
    </td>
    <td>
        &nbsp;
    </td>
</tr>
<tr>
    <td class="rowseparator" style="width: 130px"></td>
</tr>
<tr>
    <td style="width: 130px">
        &nbsp;
    </td>
    <td>
        Validity Start Date<span style="color: Red">*</span>
    </td>
    <td>
        <asp:TextBox ID="txtValiSDate" runat="server" Width="100px"
                     AutoPostBack="True" OnTextChanged="txtValiSDate_TextChanged" onkeypress="return false" oncut="return false;"
                     onpaste="return false;">
        </asp:TextBox>
        <asp:ImageButton ID="imgBtnValiSdate" runat="server" alt="" src="images/Calendar.gif"
                         Style="vertical-align: top"/>
        <cc1:CalendarExtender runat="server" TargetControlID="txtValiSDate"
                              PopupButtonID="imgBtnValiSDate" Format="MM/dd/yyyy" Enabled="True">
        </cc1:CalendarExtender>
    </td>
    <td>
        &nbsp;
    </td>
</tr>
<tr>
    <td class="rowseparator" style="width: 130px"></td>
</tr>
<tr>
    <td style="width: 130px">
        &nbsp;
    </td>
    <td>
        Policy Validity Period
    </td>
    <td>
        <asp:DropDownList ID="ddlPolicyValidityPeriod" runat="server" Width="105px" OnSelectedIndexChanged="ddlPolicyValidityPeriod_SelectedIndexChanged"
                          AutoPostBack="True">
            <asp:ListItem Value="-1">--Select--</asp:ListItem>
            <asp:ListItem Value="3">3 Month</asp:ListItem>
            <asp:ListItem Value="6">6 Month</asp:ListItem>
            <asp:ListItem Value="9">9 Month</asp:ListItem>
            <asp:ListItem Value="12">1 Year</asp:ListItem>
        </asp:DropDownList>
    </td>
    <td>
        &nbsp;
    </td>
</tr>
<tr>
    <td class="rowseparator" style="width: 130px"></td>
</tr>
<tr>
    <td style="width: 130px">
        &nbsp;
    </td>
    <td>
        Validity End Date<span style="color: Red">*</span>
    </td>
    <td>
        <%--<cc1:CalendarExtender ID="calextValEDate" runat="server" Enabled="true" PopupButtonID="imgbtValEDate"
                                        TargetControlID="txtValEDate" Format="MM/dd/yyyy">
                                    </cc1:CalendarExtender>--%>
        <asp:TextBox ID="txtValEDate" runat="server" oncut="return false;" Width="100px"
                     BackColor="DarkGray" ReadOnly="True">
        </asp:TextBox>
    </td>
    <td>
        &nbsp;
    </td>
</tr>
<tr>
    <td class="rowseparator" style="width: 130px"></td>
</tr>
</table>
</asp:WizardStep>
<asp:WizardStep runat="server" Title="Tyre Information ">
    <table style="width: 100%">
        <tr>
            <td colspan="6" style="text-align: center">
                <b>Tyre Information </b>
            </td>
        </tr>
        <tr>
            <td class="rowseparator"></td>
        </tr>
        <tr>
            <td></td>
            <td colspan="2" style="text-align: center">
                &nbsp;Tyre Information
            </td>
            <td>
                Make
            </td>
            <td>
                Model-Size
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="rowseparator"></td>
        </tr>
        <tr>
            <td>
                &nbsp;&nbsp;
            </td>
            <td>
                FL<span style="color: Red">*</span>
            </td>
            <td>
                <asp:TextBox ID="txtFL" runat="server" Width="100px" MaxLength="10" onkeypress="return alphanumeric_only(event);"></asp:TextBox>
            </td>
            <td>
                <asp:DropDownList ID="ddlTyreMakeFL" runat="server" Width="105px">
                    <asp:ListItem Value="-1">--Select--</asp:ListItem>
                    <asp:ListItem Value="0">Dummy</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                <asp:DropDownList ID="ddlModelSizeFL" runat="server" Width="105px">
                    <asp:ListItem Value="-1">--Select--</asp:ListItem>
                    <asp:ListItem Value="0">Dummy</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="rowseparator"></td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                FR<span style="color: Red">*</span>
            </td>
            <td style="width: 161px">
                <asp:TextBox ID="txtFR" runat="server" Width="100px" MaxLength="10" onkeypress="return alphanumeric_only(event);"></asp:TextBox>
            </td>
            <td>
                <asp:DropDownList ID="ddlTyreMakeFR" runat="server" Width="105px">
                    <asp:ListItem Value="-1">--Select--</asp:ListItem>
                    <asp:ListItem Value="0">Dummy</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                <asp:DropDownList ID="ddlModelSizeFR" runat="server" Width="105px">
                    <asp:ListItem Value="-1">--Select--</asp:ListItem>
                    <asp:ListItem Value="0">Dummy</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="rowseparator"></td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                RL<span style="color: Red">*</span>
            </td>
            <td>
                <asp:TextBox ID="txtRL" runat="server" Width="100px" MaxLength="10" onkeypress="return alphanumeric_only(event);"></asp:TextBox>
            </td>
            <td>
                <asp:DropDownList ID="ddlTyreMakeRL" runat="server" Width="105px">
                    <asp:ListItem Value="-1">--Select--</asp:ListItem>
                    <asp:ListItem Value="0">Dummy</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                <asp:DropDownList ID="ddlModelSizeRL" runat="server" Width="105px">
                    <asp:ListItem Value="-1">--Select--</asp:ListItem>
                    <asp:ListItem Value="0">Dummy</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="rowseparator"></td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                RR<span style="color: Red">*</span>
            </td>
            <td>
                <asp:TextBox ID="txtRR" runat="server" Width="100px" MaxLength="10" onkeypress="return alphanumeric_only(event);"></asp:TextBox>
            </td>
            <td>
                <asp:DropDownList ID="ddlTyreMakeRR" runat="server" Width="105px">
                    <asp:ListItem Value="-1">--Select--</asp:ListItem>
                    <asp:ListItem Value="0">Dummy</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                <asp:DropDownList ID="ddlModelSizeRR" runat="server" Width="105px">
                    <asp:ListItem Value="-1">--Select--</asp:ListItem>
                    <asp:ListItem Value="0">Dummy</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="rowseparator"></td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                Spare Wheel<span style="color: Red">*</span>
            </td>
            <td>
                <asp:TextBox ID="txtSpareWheel" runat="server" Width="100px" MaxLength="10" onkeypress="return alphanumeric_only(event);"></asp:TextBox>
            </td>
            <td>
                <asp:DropDownList ID="ddlTyreMakeSpareWheel" runat="server" Width="105px">
                    <asp:ListItem Value="-1">--Select--</asp:ListItem>
                    <asp:ListItem Value="0">Dummy</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                <asp:DropDownList ID="ddlModelSizeSpareWheel" runat="server" Width="105px">
                    <asp:ListItem Value="-1">--Select--</asp:ListItem>
                    <asp:ListItem Value="0">Dummy</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="rowseparator"></td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                Odometer Reading<span style="color: Red">*</span>
            </td>
            <td>
                <asp:TextBox ID="txtOdometerReading" runat="server" Width="100px" onkeypress="return isDecimalNumberKey(event);"
                             MaxLength="12">
                </asp:TextBox>
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
    </table>
</asp:WizardStep>
<asp:WizardStep runat="server" Title="Battery Information ">
    <table style="width: 100%">
        <tr>
            <td colspan="6" style="text-align: center">
                <b>Battery Information </b>
            </td>
        </tr>
        <tr>
            <td class="rowseparator"></td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td colspan="2" style="text-align: center">
                <b>Batter Information </b>
            </td>
            <td style="text-align: center">
                <b>Make</b>
            </td>
            <td>
                <b>Model-Capacity</b>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                Battery 1<span style="color: Red">*</span>
            </td>
            <td>
                <asp:TextBox ID="txtBattery1" runat="server" Width="100px" MaxLength="10" onkeypress="return alphanumeric_only(event);"></asp:TextBox>
            </td>
            <td>
                <asp:DropDownList ID="ddlBatteryMakeBattery1" runat="server" Width="105px">
                    <asp:ListItem Value="-1">--Select--</asp:ListItem>
                    <asp:ListItem Value="0">Dummy</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                <asp:DropDownList ID="ddlModelCapacityBattery1" runat="server" OnSelectedIndexChanged="ddlModelCapacityBattery1_SelectedIndexChanged"
                                  Width="105px">
                    <asp:ListItem Value="-1">--Select--</asp:ListItem>
                    <asp:ListItem Value="0">Dummy</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="rowseparator"></td>
        </tr>
        <tr>
            <td style="height: 22px">
                &nbsp;
            </td>
            <td style="height: 22px">
                Battery 2<span style="color: Red">*</span>
            </td>
            <td style="height: 22px">
                <asp:TextBox ID="txtBattery2" runat="server" Width="100px" MaxLength="10" onkeypress="return alphanumeric_only(event);"></asp:TextBox>
            </td>
            <td style="height: 22px">
                <asp:DropDownList ID="ddlBatteryMakeBattery2" runat="server" Width="105px">
                    <asp:ListItem Value="-1">--Select--</asp:ListItem>
                    <asp:ListItem Value="0">Dummy</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td style="height: 22px">
                <asp:DropDownList ID="ddlModelCapacityBattery2" runat="server" OnSelectedIndexChanged="ddlModelCapacityBattery1_SelectedIndexChanged"
                                  Width="105px">
                    <asp:ListItem Value="-1">--Select--</asp:ListItem>
                    <asp:ListItem Value="0">Dummy</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td style="height: 22px">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="rowseparator"></td>
        </tr>
    </table>
</asp:WizardStep>
</WizardSteps>
<FinishNavigationTemplate>
    <asp:Button ID="FinishPreviousButton" runat="server" CausesValidation="False" CommandName="MovePrevious"
                Text="Previous"/>
    <asp:Button ID="FinishButton" runat="server" CommandName="MoveComplete" Text="Finish"/>
</FinishNavigationTemplate>
<StepNavigationTemplate>
    <asp:Button ID="StepPreviousButton" runat="server" CausesValidation="False" CommandName="MovePrevious"
                Text="Previous"/>
    <asp:Button ID="StepNextButton" runat="server" CommandName="MoveNext" Text="Next"/>
</StepNavigationTemplate>
</asp:Wizard>
</fieldset>
</asp:Panel>
</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>