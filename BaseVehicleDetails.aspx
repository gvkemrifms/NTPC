<%@ Page Language="C#" MasterPageFile="~/temp.master" AutoEventWireup="true" CodeFile="BaseVehicleDetails.aspx.cs" Inherits="BaseVehicleDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:UpdatePanel runat="server">
<ContentTemplate>
<script type="text/javascript">
    function pageLoad() {
        $(
                '#<%= txtInvoiceDate.ClientID %>,#<%= txtPurchaseDate.ClientID %>,#<%= txtManufacturingDate.ClientID %>,#<%= txtInsuranceFeesPaidDate.ClientID %>,#<%= txtValiSDate.ClientID %>,#<%= txtInspectionDate.ClientID %>,#<%= txtTRDate.ClientID %>')
            .datepicker({
                dateFormat: 'mm/dd/yy',
                changeMonth: true,
                changeYear: true
            });
        $('#<%= ddlTRDistrict.ClientID %>,#<%= ddlDistrict.ClientID %>').select2({
            disable_search_threshold: 5,
            search_contains: true,
            minimumResultsForSearch: 2,
            placeholder: "Select an option"
        });
    }
</script>
<script>
    var publicData;
    var vehcostdata;
    var inVoiceDate;
    var purchaseDate;
    var manFactDate;
    var trDate;
    var inscStartDate;
    var inscFeeDate;
    var inspectionDate;
    var valid;
    var now = new Date();

    function vehicleCostAddition(obj) {
        if (parseFloat(obj.value)) {
            var basicPrice = document.getElementById('<%= txtBasicPrice.ClientID %>').value === ''
                ? 0
                : document.getElementById('<%= txtBasicPrice.ClientID %>').value;
            var handlingCharges = document.getElementById('<%= txtHandlingCharges.ClientID %>').value === ''
                ? 0
                : document.getElementById('<%= txtHandlingCharges.ClientID %>').value;
            var exciseDuty = document.getElementById('<%= txtExciseDuty.ClientID %>').value === ''
                ? 0
                : document.getElementById('<%= txtExciseDuty.ClientID %>').value;
            var ec = document.getElementById('<%= txtEC.ClientID %>').value === ''
                ? 0
                : document.getElementById('<%= txtEC.ClientID %>').value;
            var vat = document.getElementById('<%= txtVAT.ClientID %>').value === ''
                ? 0
                : document.getElementById('<%= txtVAT.ClientID %>').value;
            var uav = document.getElementById('<%= txtUAV.ClientID %>').value === ''
                ? 0
                : document.getElementById('<%= txtUAV.ClientID %>').value;
            var shec = document.getElementById('<%= txtSHEC.ClientID %>').value === ''
                ? 0
                : document.getElementById('<%= txtSHEC.ClientID %>').value;
            var vehCost = document.getElementById('<%= txtVehCost.ClientID %>');
            vehCost.value =
            (parseFloat(basicPrice) +
                parseFloat(handlingCharges) +
                parseFloat(exciseDuty) +
                parseFloat(ec) +
                parseFloat(vat) +
                parseFloat(uav) +
                parseFloat(shec)).toFixed(2);
            document.getElementById('<%= txtVehicleCost.ClientID %>').value = vehCost.value;
        } else {
            alert('The value should be a valid decimal value and cannot be zero');
            obj.value = '';
        }
    }

    function validation() {
        var engineNo = document.getElementById('<%= ddlEngineNo.ClientID %>');
        var invoiceNo = document.getElementById('<%= txtInvoiceNo.ClientID %>');
        var invoiceDate = document.getElementById('<%= txtInvoiceDate.ClientID %>');
        var basicPrice = document.getElementById('<%= txtBasicPrice.ClientID %>');
        var handlingCharges = document.getElementById('<%= txtHandlingCharges.ClientID %>');
        var exciseDuty = document.getElementById('<%= txtExciseDuty.ClientID %>');
        var ec = document.getElementById('<%= txtEC.ClientID %>');
        var vat = document.getElementById('<%= txtVAT.ClientID %>');
        var uav = document.getElementById('<%= txtUAV.ClientID %>');
        var shec = document.getElementById('<%= txtSHEC.ClientID %>');
        var vehCost = document.getElementById('<%= txtVehCost.ClientID %>');
        publicData = vehCost;
        if (engineNo.selectedIndex === 0) {
            alert("Please select the Engine Number");
            engineNo.focus();
            return false;
        }
        if (!RequiredValidation(invoiceNo, "Invoice Number Cannot be Blank"))
            return false;
        if (!RequiredValidation(invoiceDate, "Invoice Date cannot be Blank"))
            return false;
        if (trim(invoiceDate.value) !== "") {
            if (!isValidDate(invoiceDate.value)) {
                alert("Enter the Valid Date");
                invoiceDate.focus();
                return false;
            }
        }
        inVoiceDate = invoiceDate.value;
        if (Date.parse(invoiceDate.value) > Date.parse(now)) {
            alert("Invoice Date should not be greater than Current Date");
            invoiceDate.focus();
            return false;
        }
        if (!RequiredValidation(basicPrice, "Basic Price cannot be Blank"))
            return false;
        if (!RequiredValidation(handlingCharges, "Handling Charges cannot be Blank"))
            return false;
        if (parseFloat(handlingCharges.value) > parseFloat(basicPrice.value)) {
            alert("Basic Price should be greater than Handling Charges");
            handlingCharges.focus();
            return false;
        }
        if (!RequiredValidation(exciseDuty, "Excise Duty cannot be Blank"))
            return false;
        if (parseFloat(exciseDuty.value) > parseFloat(basicPrice.value)) {
            alert("Basic Price should be greater than Excise Duty");
            exciseDuty.focus();
            return false;
        }
        if (!RequiredValidation(ec, "EC cannot be Blank"))
            return false;
        if (parseFloat(ec.value) > parseFloat(basicPrice.value)) {
            alert("Basic Price should be greater than EC");
            ec.focus();
            return false;
        }
        if (!RequiredValidation(vat, "VAT cannot be Blank"))
            return false;
        if (parseFloat(vat.value) > parseFloat(basicPrice.value)) {
            alert("Basic Price should be greater than VAT");
            vat.focus();
            return false;
        }
        if (!RequiredValidation(uav, "UAV cannot be Blank"))
            return false;
        if (parseFloat(uav.value) > parseFloat(basicPrice.value)) {
            alert("Basic Price should be greater than UAV");
            uav.focus();
            return false;
        }
        if (!RequiredValidation(shec, "SHEC cannot be Blank"))
            return false;
        if (parseFloat(shec.value) > parseFloat(basicPrice.value)) {
            alert("Basic Price should be greater than SHEC");
            shec.focus();
            return false;
        }
        vehCost.value =
        (parseFloat(basicPrice.value) +
            parseFloat(handlingCharges.value) +
            parseFloat(exciseDuty.value) +
            parseFloat(ec.value) +
            parseFloat(vat.value) +
            parseFloat(uav.value) +
            parseFloat(shec.value)).toFixed(2);
        var vehicleModel = document.getElementById('<%= ddlVehicleModel.ClientID %>');
        var kmpl = document.getElementById('<%= txtKmpl.ClientID %>');
        var vehicleType = document.getElementById('<%= ddlVehicleType.ClientID %>');
        var vehicleEmissionType = document.getElementById('<%= txtVehicleEmissionType.ClientID %>');
        var purchaseDate = document.getElementById('<%= txtPurchaseDate.ClientID %>');
        var ownerName = document.getElementById('<%= txtOwnerName.ClientID %>');
        var manufacturerName = document.getElementById('<%= ddlManufacturerName.ClientID %>');
        var vehicleCost = document.getElementById('<%= txtVehicleCost.ClientID %>');
        var manufacturingDate = document.getElementById('<%= txtManufacturingDate.ClientID %>');
        var engineCapacity = document.getElementById('<%= txtEngineCapacity.ClientID %>');
        var fuelType = document.getElementById('<%= ddlFuelType.ClientID %>');
        if (vehicleModel.selectedIndex === 0) {
            alert("Please select the Vehicle Model");
            vehicleModel.focus();
            return false;
        }
        if (!RequiredValidation(kmpl, "Kmpl cannot be Blank"))
            return false;
        if (vehicleType.selectedIndex === 0) {
            alert("Please select the Vehicle Type");
            vehicleType.focus();
            return false;
        }
        if (!RequiredValidation(vehicleEmissionType, "Vehicle Emission Type cannot be Blank"))
            return false;
        if (!RequiredValidation(purchaseDate, "Purchase Date cannot be Blank"))
            return false;
        if (trim(purchaseDate.value) !== "") {
            if (!isValidDate(purchaseDate.value)) {
                alert("Enter the Purchase Date");
                purchaseDate.focus();
                return false;
            }
        }
        if (Date.parse(purchaseDate.value) > Date.parse(now)) {
            alert("Purchase Date should not be greater than Current Date");
            purchaseDate.focus();
            return false;
        }
        if (Date.parse(purchaseDate.value) > Date.parse(inVoiceDate)) {
            alert("Purchase Date should not be greater than Invoice Date.(Invoice Date-" + inVoiceDate + ")");
            purchaseDate.focus();
            return false;
        }
        if (!RequiredValidation(ownerName, "Owner Name cannot be Blank"))
            return false;
        if (manufacturerName.selectedIndex === 0) {
            alert("Please select the Manufacturer Name");
            manufacturerName.focus();
            return false;
        }
        if (!RequiredValidation(vehicleCost, "Vehicle Cost cannot be Blank"))
            return false;
        if (parseInt(vehicleCost.value) > parseInt(publicData.value)) {
            alert("Vehicle Cost should be less than Total Value of the Vehicle.(Total Value-" + publicData.value + ")");
            vehicleCost.focus();
            return false;
        }
        if (!RequiredValidation(manufacturingDate, "Manufacturing Date cannot be Blank"))
            return false;
        if (Date.parse(manufacturingDate.value) > Date.parse(now)) {
            alert("Manufacturing Date should not be greater than Current Date");
            manufacturingDate.focus();
            return false;
        }
        if (Date.parse(manufacturingDate.value) > Date.parse(purchaseDate)) {
            alert("Manufacturing Date should be less than Purchase Date");
            manufacturingDate.focus();
            return false;
        }
        if (!RequiredValidation(engineCapacity, "Engine Capacity cannot be Blank"))
            return false;
        if (fuelType.selectedIndex === 0) {
            alert("Please select the Fuel Type");
            fuelType.focus();
            return false;
        }
        var district = document.getElementById('<%= ddlDistrict.ClientID %>');
        var inPolicyNo = document.getElementById('<%= txtInPolicyNo.ClientID %>');
        var insType = document.getElementById('<%= ddlInsType.ClientID %>');
        var insFee = document.getElementById('<%= txtInsFee.ClientID %>');
        var insuranceReceiptNo = document.getElementById('<%= txtInsuranceReceiptNo.ClientID %>');
        var insuranceFeesPaidDate = document.getElementById('<%= txtInsuranceFeesPaidDate.ClientID %>');
        var agency = document.getElementById('<%= ddlAgency.ClientID %>');
        var valiSDate = document.getElementById('<%= txtValiSDate.ClientID %>');
        var policyValidityPeriod = document.getElementById('<%= ddlPolicyValidityPeriod.ClientID %>');
        if (district.selectedIndex === 0) {
            alert("Please select the State");
            district.focus();
            return false;
        }
        if (!RequiredValidation(inPolicyNo, "Insurance Policy No cannot be Blank"))
            return false;
        if (insType.selectedIndex === 0) {
            alert("Please select the Insurance Type");
            insType.focus();
            return false;
        }
        if (!RequiredValidation(insFee, "Insurance Fee No cannot be Blank"))
            return false;
        if (!RequiredValidation(insuranceReceiptNo, "Insurance Receipt No cannot be Blank"))
            return false;
        if (!RequiredValidation(insuranceFeesPaidDate, "InsuranceFees PaidDate No cannot be Blank"))
            return false;
        if (trim(insuranceFeesPaidDate.value) !== "") {
            if (!isValidDate(insuranceFeesPaidDate.value)) {
                alert("Enter the valid InsuranceFees PaidDate");
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
            alert("Insurance Fees Paid Date should be greater than Purchase Date.(Purchase Date-" + purchaseDate + ")");
            insuranceFeesPaidDate.focus();
            return false;
        }
        if (Date.parse(insuranceFeesPaidDate.value) < Date.parse(inVoiceDate)) {
            alert("Insurance Fees Paid Date should be greater than Invoice Date.(Invoice Date-" + inVoiceDate + ")");
            insuranceFeesPaidDate.focus();
            return false;
        }
        if (agency.selectedIndex === 0) {
            alert("Please select the Agency");
            agency.focus();
            return false;
        }
        if (!RequiredValidation(valiSDate, "Validity Start Date cannot be Blank"))
            return false;
        if (trim(valiSDate.value) !== "") {
            if (!isValidDate(valiSDate.value)) {
                alert("Enter the validity start date");
                valiSDate.focus();
                return false;
            }
        }
        if (Date.parse(valiSDate.value) > Date.parse(now)) {
            alert("Validity Start Date should not be greater than Current Date");
            valiSDate.focus();
            return false;
        }
        inscStartDate = valiSDate.value;
        if (Date.parse(valiSDate.value) < Date.parse(insuranceFeesPaidDate.value)) {
            alert("Validity Start Date should not be less than Insurance Fees Paid Date");
            valiSDate.focus();
            return false;
        }
        if (policyValidityPeriod.selectedIndex === 0) {
            alert("Please select the Policy Validity Period");
            policyValidityPeriod.focus();
            return false;
        }
        var inspectionDate = document.getElementById('<%= txtInspectionDate.ClientID %>');
        var inspectedBy = document.getElementById('<%= txtInspectedBy.ClientID %>');
        if (!RequiredValidation(inspectionDate, "Inspection Date cannot be Blank"))
            return false;
        if (trim(inspectionDate.value) !== "") {
            if (!isValidDate(inspectionDate.value)) {
                alert("Enter the valid Inspection Date");
                inspectionDate.focus();
                return false;
            }
        }
        if (Date.parse(inspectionDate.value) > Date.parse(now)) {
            alert("Inspection Date should not be greater than Current Date");
            inspectionDate.focus();
            return false;
        }
        if (Date.parse(inspectionDate.value) < Date.parse(inscStartDate)) {
            alert("Inspection Date should be greater than Insurance Validity Start Date.(Insurance Start Date-" +
                inscStartDate +
                ")");
            inspectionDate.focus();
            return false;
        }
        inspectionDate = inspectionDate.value;
        if (!RequiredValidation(inspectedBy, "Inspected By cannot be Blank"))
            return false;
        var trNo = document.getElementById('<%= txtTRNo.ClientID %>');
        var trDate = document.getElementById('<%= txtTRDate.ClientID %>');
        var trDistrict = document.getElementById('<%= ddlTRDistrict.ClientID %>');
        var veRtaCircle = document.getElementById('<%= txtVeRTACircle.ClientID %>');
        var roadTaxFee = document.getElementById('<%= txtRoadTaxFee.ClientID %>');
        var sittingCapacity = document.getElementById('<%= txtSittingCapacity.ClientID %>');
        if (!RequiredValidation(trNo, "TR No cannot be Blank"))
            return false;
        if (!isValidVehicleNumber(trNo.value)) {
            trNo.value = "";
            trNo.focus();
            return false;
        }
        if (!RequiredValidation(trDate, "TR Date cannot be Blank"))
            return false;
        if (trim(trDate.value) !== "") {
            if (!isValidDate(trDate.value)) {
                alert("Enter the valid Inspection Date");
                trDate.focus();
                return false;
            }
        }
        if (Date.parse(trDate.value) > Date.parse(now)) {
            alert("TR Date should not be greater than Current Date");
            trDate.focus();
            return false;
        }
        if (Date.parse(trDate.value) < Date.parse(inspectionDate)) {
            alert("TR Date should be greater than Inspection Date.(Inspection Date-" + inspectionDate + ")");
            trDate.focus();
            return false;
        }
        if (trDistrict.selectedIndex === 0) {
            alert("Please select the State");
            trDistrict.focus();
            return false;
        }
        if (!RequiredValidation(veRtaCircle, "Vehicle RTA Circle cannot be Blank"))
            return false;
        if (!RequiredValidation(roadTaxFee, "Road Tax Fee cannot be Blank"))
            return false;
        if (!RequiredValidation(sittingCapacity, "Seating Capacity cannot be Blank"))
            return false;
        var fl = document.getElementById('<%= txtFL.ClientID %>');
        var fr = document.getElementById('<%= txtFR.ClientID %>');
        var rl = document.getElementById('<%= txtRL.ClientID %>');
        var rr = document.getElementById('<%= txtRR.ClientID %>');
        var spareWheel = document.getElementById('<%= txtSpareWheel.ClientID %>');
        var tyreMake = document.getElementById('<%= ddlTyreMake.ClientID %>');
        var modelSize = document.getElementById('<%= ddlModelSize.ClientID %>');
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
                if (tyre[i] === tyre[j]) {
                    alert("Tyre numbers should be unique");
                    return false;
                }
            }
        }
        if (tyreMake.selectedIndex === 0) {
            alert("Please select the Tyre Make");
            tyreMake.focus();
            return false;
        }
        if (modelSize.selectedIndex === 0) {
            alert("Please select the Model Size");
            modelSize.focus();
            return false;
        }
        if (!RequiredValidation(odometerReading, "Odometer Reading cannot be Blank"))
            return false;
        var battery1 = document.getElementById('<%= txtBattery1.ClientID %>');
        var battery2 = document.getElementById('<%= txtBattery2.ClientID %>');
        var batteryMake = document.getElementById('<%= ddlBatteryMake.ClientID %>');
        var modelCapacity = document.getElementById('<%= ddlModelCapacity.ClientID %>');
        if (!RequiredValidation(battery1, "Battery1 cannot be Blank"))
            return false;
        if (!RequiredValidation(battery2, "Battery2 cannot be Blank"))
            return false;
        if (battery1.value === battery2.value) {
            alert("Battery Numbers should be unique");
            battery2.focus();
            return false;
        }
        if (batteryMake.selectedIndex === 0) {
            alert("Please select the Battery Make");
            batteryMake.focus();
            return false;
        }
        if (modelCapacity.selectedIndex === 0) {
            alert("Please select the Model Capacity");
            modelCapacity.focus();
            return false;
        }
        return true;
    }

    var stepNo = 0;

    function previousValidation() {
        if (stepNo <= 0)
            return;
        stepNo--;
    }

    function stepvalidation() {
        ////-----------------Start of Validation of General Information----------------////
        var purchaseDate;
        if (stepNo === 0) {
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
            if (vehicleModel.selectedIndex === 0) {
                alert("Please select the Vehicle Model");
                vehicleModel.focus();
                return false;
            }
            if (!RequiredValidation(kmpl, "Kmpl cannot be Blank"))
                return false;
            if (vehicleType.selectedIndex === 0) {
                alert("Please select the Vehicle Type");
                vehicleType.focus();
                return false;
            }
            if (!RequiredValidation(vehicleEmissionType, "Vehicle Emission Type cannot be Blank"))
                return false;
            if (!RequiredValidation(purchaseDate, "Purchase Date cannot be Blank"))
                return false;
            if (trim(purchaseDate.value) !== "") {
                if (!isValidDate(purchaseDate.value)) {
                    alert("Enter the Purchase Date");
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
            if (Date.parse(purchaseDate.value) > Date.parse(inVoiceDate)) {
                alert("Purchase Date should not be greater than Invoice Date.(Invoice Date-" + inVoiceDate + ")");
                purchaseDate.focus();
                return false;
            }
            if (!RequiredValidation(ownerName, "Owner Name cannot be Blank"))
                return false;
            if (manufacturerName.selectedIndex === 0) {
                alert("Please select the Manufacturer Name");
                manufacturerName.focus();
                return false;
            }
            if (!RequiredValidation(vehicleCost, "Vehicle Cost cannot be Blank"))
                return false;
            if (parseInt(vehicleCost.value) > parseInt(publicData.value)) {
                alert("Vehicle Cost should be less than Total Value of the Vehicle.(Total Value-" +
                    publicData.value +
                    ")");
                vehicleCost.focus();
                return false;
            }
            if (!RequiredValidation(manufacturingDate, "Manufacturing Date cannot be Blank"))
                return false;
            if (trim(manufacturingDate.value) !== "") {
                if (!isValidDate(manufacturingDate.value)) {
                    alert("Enter the Manufacturing Date");
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
                alert("Manufacturing Date should be less than Purchase Date");
                manufacturingDate.focus();
                return false;
            }
            if (!RequiredValidation(engineCapacity, "Engine Capacity cannot be Blank"))
                return false;
            if (fuelType.selectedIndex === 0) {
                alert("Please select the Fuel Type");
                fuelType.focus();
                return false;
            }
            stepNo++;
        }
        //Insurance Information
        else if (stepNo === 1) {
            var district = document.getElementById('<%= ddlDistrict.ClientID %>');
            var inPolicyNo = document.getElementById('<%= txtInPolicyNo.ClientID %>');
            var insType = document.getElementById('<%= ddlInsType.ClientID %>');
            var insFee = document.getElementById('<%= txtInsFee.ClientID %>');
            var insuranceReceiptNo = document.getElementById('<%= txtInsuranceReceiptNo.ClientID %>');
            var insuranceFeesPaidDate = document.getElementById('<%= txtInsuranceFeesPaidDate.ClientID %>');
            var agency = document.getElementById('<%= ddlAgency.ClientID %>');
            var valiSDate = document.getElementById('<%= txtValiSDate.ClientID %>');
            var policyValidityPeriod = document.getElementById('<%= ddlPolicyValidityPeriod.ClientID %>');
            if (district.selectedIndex === 0) {
                alert("Please select the State");
                district.focus();
                return false;
            }
            if (!RequiredValidation(inPolicyNo, "Insurance Policy No cannot be Blank"))
                return false;
            if (insType.selectedIndex === 0) {
                alert("Please select the Insurance Type");
                insType.focus();
                return false;
            }
            if (!RequiredValidation(insFee, "Insurance Fee No cannot be Blank"))
                return false;
            if (!RequiredValidation(insuranceReceiptNo, "Insurance Receipt No cannot be Blank"))
                return false;
            if (!RequiredValidation(insuranceFeesPaidDate, "InsuranceFees PaidDate No cannot be Blank"))
                return false;
            if (trim(insuranceFeesPaidDate.value) !== "") {
                if (!isValidDate(insuranceFeesPaidDate.value)) {
                    alert("Enter the valid InsuranceFees PaidDate");
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
            if (Date.parse(insuranceFeesPaidDate.value) < Date.parse(inVoiceDate)) {
                alert("Insurance Fees Paid Date should be greater than Invoice Date.(Invoice Date-" +
                    inVoiceDate +
                    ")");
                insuranceFeesPaidDate.focus();
                return false;
            }
            if (agency.selectedIndex === 0) {
                alert("Please select the Agency");
                agency.focus();
                return false;
            }
            if (!RequiredValidation(valiSDate, "Validity Start Date cannot be Blank"))
                return false;
            if (trim(valiSDate.value) !== "") {
                if (!isValidDate(valiSDate.value)) {
                    alert("Enter the validity start date");
                    valiSDate.focus();
                    return false;
                }
            }
            if (Date.parse(valiSDate.value) > Date.parse(now)) {
                alert("Validity Start Date should not be greater than Current Date");
                valiSDate.focus();
                return false;
            }
            inscStartDate = valiSDate.value;
            if (Date.parse(valiSDate.value) < Date.parse(insuranceFeesPaidDate.value)) {
                alert("Validity Start Date should not be less than Insurance Fees Paid Date");
                valiSDate.focus();
                return false;
            }
            if (policyValidityPeriod.selectedIndex === 0) {
                alert("Please select the Policy Validity Period");
                policyValidityPeriod.focus();
                return false;
            }
            stepNo++;
        }
        //Inspection Information
        else if (stepNo === 2) {
            inspectionDate = document.getElementById('<%= txtInspectionDate.ClientID %>');
            var inspectedBy = document.getElementById('<%= txtInspectedBy.ClientID %>');
            if (!RequiredValidation(inspectionDate, "Inspection Date cannot be Blank"))
                return false;
            if (trim(inspectionDate.value) !== "") {
                if (!isValidDate(inspectionDate.value)) {
                    alert("Enter the valid Inspection Date");
                    inspectionDate.focus();
                    return false;
                }
            }
            if (Date.parse(inspectionDate.value) > Date.parse(now)) {
                alert("Inspection Date should not be greater than Current Date");
                inspectionDate.focus();
                return false;
            }
            if (Date.parse(inspectionDate.value) < Date.parse(inscStartDate)) {
                alert("Inspection Date should be greater than Insurance Validity Start Date.(Insurance Start Date-" +
                    inscStartDate +
                    ")");
                inspectionDate.focus();
                return false;
            }
            inspectionDate = inspectionDate.value;
            if (!RequiredValidation(inspectedBy, "Inspected By cannot be Blank"))
                return false;
            stepNo++;
        } else if (stepNo === 3) {
            var trNo = document.getElementById('<%= txtTRNo.ClientID %>');
            var trDate = document.getElementById('<%= txtTRDate.ClientID %>');
            var trDistrict = document.getElementById('<%= ddlTRDistrict.ClientID %>');
            var veRtaCircle = document.getElementById('<%= txtVeRTACircle.ClientID %>');
            var roadTaxFee = document.getElementById('<%= txtRoadTaxFee.ClientID %>');
            var sittingCapacity = document.getElementById('<%= txtSittingCapacity.ClientID %>');
            if (!RequiredValidation(trNo, "TR No cannot be Blank"))
                return false;
            if (!isValidVehicleNumber(trNo.value)) {
                trNo.value = "";
                trNo.focus();
                return false;
            }
            if (!RequiredValidation(trDate, "TR Date cannot be Blank"))
                return false;
            if (trim(trDate.value) !== "") {
                if (!isValidDate(trDate.value)) {
                    alert("Enter the valid Inspection Date");
                    trDate.focus();
                    return false;
                }
            }
            if (Date.parse(trDate.value) > Date.parse(now)) {
                alert("TR Date should not be greater than Current Date");
                trDate.focus();
                return false;
            }
            if (Date.parse(trDate.value) < Date.parse(inspectionDate)) {
                alert("TR Date should be greater than Inspection Date.(Inspection Date-" + inspectionDate + ")");
                trDate.focus();
                return false;
            }
            if (trDistrict.selectedIndex === 0) {
                alert("Please select the State");
                trDistrict.focus();
                return false;
            }
            if (!RequiredValidation(veRtaCircle, "Vehicle RTA Circle cannot be Blank"))
                return false;
            if (!RequiredValidation(roadTaxFee, "Road Tax Fee cannot be Blank"))
                return false;
            if (!RequiredValidation(sittingCapacity, "Seating Capacity cannot be Blank"))
                return false;
            stepNo++;
        } else if (stepNo === 4) {
            var fl = document.getElementById('<%= txtFL.ClientID %>');
            var fr = document.getElementById('<%= txtFR.ClientID %>');
            var rl = document.getElementById('<%= txtRL.ClientID %>');
            var rr = document.getElementById('<%= txtRR.ClientID %>');
            var spareWheel = document.getElementById('<%= txtSpareWheel.ClientID %>');
            var tyreMake = document.getElementById('<%= ddlTyreMake.ClientID %>');
            var modelSize = document.getElementById('<%= ddlModelSize.ClientID %>');
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
                    if (tyre[i] === tyre[j]) {
                        alert("Tyre numbers should be unique");
                        return false;
                    }
                }
            }
            if (tyreMake.selectedIndex === 0) {
                alert("Please select the Tyre Make");
                tyreMake.focus();
                return false;
            }
            if (modelSize.selectedIndex === 0) {
                alert("Please select the Model Size");
                modelSize.focus();
                return false;
            }
            if (!RequiredValidation(odometerReading, "Odometer Reading cannot be Blank"))
                return false;
            stepNo++;
        }
        return true;
    }

    function finalStepValidation() {
        var battery1 = document.getElementById('<%= txtBattery1.ClientID %>');
        var battery2 = document.getElementById('<%= txtBattery2.ClientID %>');
        var batteryMake = document.getElementById('<%= ddlBatteryMake.ClientID %>');
        var modelCapacity = document.getElementById('<%= ddlModelCapacity.ClientID %>');
        if (!RequiredValidation(battery1, "Battery1 cannot be Blank"))
            return false;
        if (!RequiredValidation(battery2, "Battery2 cannot be Blank"))
            return false;
        if (battery1.value === battery2.value) {
            alert("Battery Numbers should be unique");
            battery2.focus();
            return false;
        }
        if (batteryMake.selectedIndex === 0) {
            alert("Please select the Battery Make");
            batteryMake.focus();
            return false;
        }
        if (modelCapacity.selectedIndex === 0) {
            alert("Please select the Model Capacity");
            modelCapacity.focus();
            return false;
        }
        return true;
    }
</script>
<asp:Panel ID="pnlBaseVehicleDetails" runat="server">
<asp:HiddenField runat="server"/>
<legend align="center" style="color: brown">Base Vehicle Details </legend>
<br/>
<table align="center">
    <tr>
        <td >
            Engine No<span style="color: Red">*</span>
        </td>
        <td >
            <asp:DropDownList ID="ddlEngineNo" runat="server" AutoPostBack="True" CssClass="Dropdowncss" OnSelectedIndexChanged="ddlEngineNo_SelectedIndexChanged"
                              style="width: 150px;" TabIndex="1">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td>
            Chassis No<span style="color: Red; padding-left: 10px;">*</span>
        </td>
        <td >
            <asp:TextBox ID="txtChassisNo" runat="server" BackColor="DarkGray" CssClass="search_3" ReadOnly="True" Width="150px" TabIndex="2"></asp:TextBox>
        </td>
    </tr>
</table>
<br/>
<legend align="center" style="color: brown">Invoice and Base Details</legend>
<table align="center">
    <tr>
        <td class="rowseparator" style="width: 164px">
        </td>
    </tr>
    <tr>

        <td style="width: 168px">

        </td>
        <td >
            <lable style="margin-right: 1px">Invoice No</lable>
            <span style="color: Red">*</span>
        </td>
        <td style="margin-right: 1px">

        </td>
        <td>
            <asp:TextBox ID="txtInvoiceNo" runat="server" Width="100px" CssClass="search_3" onkeypress="return alphanumeric_only(event);"
                         MaxLength="15" TabIndex="3" style="margin-left: -100px">
            </asp:TextBox>
        </td>
        <td>
            &nbsp;
        </td>

        <td >
            Invoice Date<span style="color: Red">*</span>
        </td>
        <td >
            <asp:TextBox ID="txtInvoiceDate" runat="server" oncut="return false;" onpaste="return false;"
                         Width="100px" onkeypress="return false" CssClass="search_3" TabIndex="4" style="margin-left: -80px">
            </asp:TextBox>
        </td>

    </tr>
    <tr>
        <td class="rowseparator" style="width: 164px">
        </td>
    </tr>
    <tr>
        <td style="width: 164px">
            &nbsp;
        </td>
        <td style="width: 198px">
            Basic Price<span style="color: Red">*</span>
        </td>
        <td >
            <asp:TextBox ID="txtBasicPrice" runat="server" name="floats" Width="100px" ondrop="return false;"
                         onpaste="return false;" CssClass="search_3" onkeypress="return numericOnly(this);"
                         MaxLength="11" onchange="return vehicleCostAddition(this)" TabIndex="5">
            </asp:TextBox>
        </td>
        <td>
            &nbsp;
        </td>

        <td style="width: 164px">
            &nbsp;
        </td>
        <td style="width: 198px">
            Handling Charges<span style="color: Red">*</span>
        </td>
        <td >
            <asp:TextBox ID="txtHandlingCharges" style="margin-left: -80px" runat="server" name="floats" Width="100px" onkeypress="return numericOnly(this);"
                         MaxLength="8" onchange="return vehicleCostAddition(this)" CssClass="search_3" TabIndex="6">
            </asp:TextBox>
        </td>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td class="rowseparator" style="width: 164px">
        </td>
    </tr>
    <tr>
        <td style="width: 164px">
            &nbsp;
        </td>
        <td style="width: 198px">
            Excise Duty<span style="color: Red">*</span>
        </td>
        <td>
            <asp:TextBox ID="txtExciseDuty" runat="server" name="floats" Width="100px"
                         MaxLength="8" onchange="return vehicleCostAddition(this)" CssClass="search_3" onkeypress="return numericOnly(this);" TabIndex="7">
            </asp:TextBox>
        </td>
        <td>
            &nbsp;
        </td>

        <td style="width: 164px">
            &nbsp;
        </td>
        <td style="width: 198px">
            EC<span style="color: Red">*</span>
        </td>
        <td >
            <asp:TextBox ID="txtEC" runat="server" style="margin-left: -80px" Width="100px"
                         MaxLength="8" onchange="return vehicleCostAddition(this)" CssClass="search_3" onkeypress="return numericOnly(this);" TabIndex="8">
            </asp:TextBox>
        </td>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td class="rowseparator" style="width: 164px">
        </td>
    </tr>
    <tr>
        <td style="width: 164px">
            &nbsp;
        </td>
        <td style="width: 198px">
            VAT<span style="color: Red">*</span>
        </td>
        <td >
            <asp:TextBox ID="txtVAT" runat="server" Width="100px" onkeypress="return numericOnly(this);"
                         MaxLength="8" onchange="return vehicleCostAddition(this)" CssClass="search_3" TabIndex="9">
            </asp:TextBox>
        </td>
        <td>
            &nbsp;
        </td>

        <td style="width: 164px">
            &nbsp;
        </td>
        <td style="width: 198px">
            Cess on UAV<span style="color: Red">*</span>
        </td>
        <td >
            <asp:TextBox ID="txtUAV" style="margin-left: -80px" runat="server" Width="100px" onkeypress="return numericOnly(this);"
                         MaxLength="8" onchange="return vehicleCostAddition(this)" CssClass="search_3" TabIndex="10">
            </asp:TextBox>
        </td>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td class="rowseparator" style="width: 164px">
        </td>
    </tr>
    <tr>
        <td style="width: 164px">
            &nbsp;
        </td>
        <td style="width: 198px">
            SHEC<span style="color: Red">*</span>
        </td>
        <td >
            <asp:TextBox ID="txtSHEC" runat="server" Width="100px" onkeypress="return numericOnly(this);"
                         AutoPostBack="True" MaxLength="8"
                         onchange="return vehicleCostAddition(this)" CssClass="search_3" TabIndex="11">
            </asp:TextBox>
        </td>
        <td>
            &nbsp;
        </td>

        <td style="width: 164px">
            &nbsp;
        </td>
        <td style="width: 198px">
            Total Value<span style="color: Red">*</span>
        </td>
        <td >
            <asp:TextBox ID="txtVehCost" style="margin-left: -80px" runat="server" BackColor="DarkGray" Width="100px"
                         onkeypress="return false" CssClass="search_3" TabIndex="12">
            </asp:TextBox>
        </td>
        <td>
            &nbsp;
        </td>
    </tr>
</table>
<br/>
<legend align="center" style="color: brown">General Information</legend>
<table align="center">

    <tr>
        <td class="rowseparator" style="width: 69px">
        </td>
    </tr>
    <tr>
        <td style="width: 69px">
            &nbsp;
        </td>
        <td>
            Vehicle Model<span style="color: red">*</span>
        </td>
        <td colspan="2">
            <asp:DropDownList ID="ddlVehicleModel" runat="server" Width="105px" CssClass="Dropdowncss"
                              TabIndex="13">
            </asp:DropDownList>
        </td>
        <td>
            &nbsp;
        </td>

        <td style="width: 37px">
            &nbsp;
        </td>
        <td>
            KMPL<span style="color: Red">*</span>
        </td>
        <td colspan="2">
            <asp:TextBox ID="txtKmpl" runat="server" Width="100px" CssClass="search_3" onkeypress="return numericOnly(this);"
                         MaxLength="5" TabIndex="14">
            </asp:TextBox>
        </td>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td class="rowseparator" style="width: 69px">
        </td>
    </tr>
    <tr>
        <td style="width: 69px">
            &nbsp;
        </td>
        <td >
            Vehicle Type<span style="color: Red">*</span>
        </td>
        <td colspan="2">
            <asp:DropDownList ID="ddlVehicleType" runat="server" Width="105px" CssClass="Dropdowncss"
                              TabIndex="15">
            </asp:DropDownList>
        </td>
        <td>
            &nbsp;
        </td>

        <td style="width: 37px">
            &nbsp;
        </td>
        <td colspan>
            Vehicle Emission Type<span style="color: Red">*</span>
        </td>
        <td colspan="2">
            <asp:TextBox ID="txtVehicleEmissionType" runat="server" Width="100px" MaxLength="10"
                         onkeypress="return alpha_only(event);" CssClass="search_3" TabIndex="16">
            </asp:TextBox>
        </td>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td class="rowseparator" style="width: 69px">
        </td>
    </tr>
    <tr>
        <td style="width: 69px">
            &nbsp;
        </td>
        <td>
            Purchase Date<span style="color: Red">*</span>
        </td>
        <td colspan="2">
            <asp:TextBox ID="txtPurchaseDate" runat="server" Width="100px" onkeypress="return false"
                         oncut="return false;" onpaste="return false;" CssClass="search_3" TabIndex="17">
            </asp:TextBox>
        </td>
        <td>
            &nbsp;
        </td>

        <td style="width: 170px">
            &nbsp;
        </td>
        <td>
            Owner Name<span style="color: Red">*</span>
        </td>
        <td colspan="2">
            <asp:TextBox ID="txtOwnerName" runat="server" Width="100px" MaxLength="35"
                         onkeypress="return alpha_only_withspace(event);" CssClass="search_3" TabIndex="19">
            </asp:TextBox>
        </td>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td class="rowseparator" style="width: 69px">
        </td>
    </tr>
    <tr>
        <td style="width: 69px">
            &nbsp;
        </td>
        <td>
            Manufacturer Name<span style="color: Red">*</span>
        </td>
        <td colspan="2">
            <asp:DropDownList ID="ddlManufacturerName" runat="server" Width="105px" CssClass="Dropdowncss"
                              TabIndex="20">
            </asp:DropDownList>
        </td>
        <td>
            &nbsp;
        </td>

        <td style="width: 37px">
            &nbsp;
        </td>
        <td>
            Vehicle Cost<span style="color: Red">*</span>
        </td>
        <td colspan="2">
            <asp:TextBox ID="txtVehicleCost" runat="server" Width="100px" onkeypress="return numericOnly(this);"
                         MaxLength="11" ReadOnly="True" CssClass="search_3" TabIndex="21">
            </asp:TextBox>
        </td>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td class="rowseparator" style="width: 69px">
        </td>
    </tr>
    <tr>
        <td style="width: 69px">
            &nbsp;
        </td>
        <td>
            Manufacturing Date<span style="color: Red">*</span>
        </td>
        <td colspan="2">
            <asp:TextBox ID="txtManufacturingDate" runat="server" Width="100px" onkeypress="return false"
                         oncut="return false;" onpaste="return false;" CssClass="search_3" TabIndex="22">
            </asp:TextBox>
        </td>
        <td>
            &nbsp;
        </td>

        <td style="width: 37px">
            &nbsp;
        </td>
        <td>
            Engine Capacity<span style="color: Red">*</span>
        </td>
        <td colspan="2">
            <asp:TextBox ID="txtEngineCapacity" runat="server" Width="100px" onkeypress="return alphanumeric_only(event);"
                         MaxLength="8" CssClass="search_3" TabIndex="24">
            </asp:TextBox>
        </td>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td class="rowseparator" style="width: 69px">
        </td>
    </tr>
    <tr>
        <td style="width: 69px">
            &nbsp;
        </td>
        <td>
            Fuel Type<span style="color: Red">*</span>
        </td>
        <td colspan="2">
            <asp:DropDownList ID="ddlFuelType" runat="server" Width="105px" TabIndex="25" CssClass="Dropdowncss">
            </asp:DropDownList>
        </td>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td class="rowseparator" style="width: 69px">
        </td>
    </tr>
</table>
&nbsp;&nbsp;&nbsp;
<br/>
<legend align="center" style="color: brown">Insurance Information</legend>
<table align="center">

    <tr>
        <td class="rowseparator" style="width: 69px">
        </td>
    </tr>
    <tr>
        <td style="width: 69px">
            &nbsp;
        </td>
        <td>
            State<span style="color: Red">*</span>
        </td>
        <td>
            <asp:DropDownList ID="ddlDistrict" runat="server" Width="150px" TabIndex="26">
            </asp:DropDownList>
        </td>
        <td>
            &nbsp;
        </td>

        <td style="width: 170px">
            &nbsp;
        </td>
        <td>
            Insurance Policy No<span style="color: Red">*</span>
        </td>
        <td>
            <asp:TextBox ID="txtInPolicyNo" runat="server" Width="100px" MaxLength="15"
                         onkeypress="return alphanumeric_only(event);" CssClass="search_3" TabIndex="27">
            </asp:TextBox>
        </td>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td class="rowseparator" style="width: 69px">
        </td>
    </tr>
    <tr>
        <td style="width: 69px">
            &nbsp;
        </td>
        <td>
            Insurance Type<span style="color: Red">*</span>
        </td>
        <td>
            <asp:DropDownList ID="ddlInsType" runat="server" Width="105px" TabIndex="28" CssClass="Dropdowncss">
            </asp:DropDownList>
        </td>
        <td>
            &nbsp;
        </td>

        <td style="width: 35px">
            &nbsp;
        </td>
        <td>
            Insurance Fee<span style="color: Red">*</span>
        </td>
        <td>
            <asp:TextBox ID="txtInsFee" runat="server" Width="100px" onkeypress="return numericOnly(this);"
                         MaxLength="11" CssClass="search_3" TabIndex="29">
            </asp:TextBox>
        </td>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td class="rowseparator" style="width: 69px">
        </td>
    </tr>

    <tr>
        <td style="width: 69px">
            &nbsp;
        </td>
        <td>
            Insurance Receipt No<span style="color: Red">*</span>
        </td>
        <td>
            <asp:TextBox ID="txtInsuranceReceiptNo" runat="server" Width="100px" MaxLength="15"
                         onkeypress="return alphanumeric_only(event);" CssClass="search_3" TabIndex="30">
            </asp:TextBox>
        </td>
        <td>
            &nbsp;
        </td>

        <td style="width: 35px">
            &nbsp;
        </td>
        <td>
            Insurance Fees Paid Date
        </td>
        <td>
            <asp:TextBox ID="txtInsuranceFeesPaidDate" runat="server" Width="100px"
                         TabIndex="31" CssClass="search_3" onkeypress="return false">
            </asp:TextBox>
        </td>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td class="rowseparator" style="width: 69px">
        </td>
    </tr>
    <tr>
        <td style="width: 69px">
            &nbsp;
        </td>
        <td>
            Agency<span style="color: Red">*</span>
        </td>
        <td>
            <asp:DropDownList ID="ddlAgency" runat="server" Width="150px" TabIndex="33" CssClass="Dropdowncss">
            </asp:DropDownList>
        </td>
        <td>
            &nbsp;
        </td>

        <td style="width: 35px">
            &nbsp;
        </td>
        <td>
            Validity Start Date<span style="color: Red">*</span>
        </td>
        <td>
            <asp:TextBox ID="txtValiSDate" runat="server" Width="100px" AutoPostBack="True" OnTextChanged="txtValiSDate_TextChanged"
                         onkeypress="return false;" CssClass="search_3" oncut="return false;" onpaste="return false;"
                         TabIndex="34">
            </asp:TextBox>
        </td>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td class="rowseparator" style="width: 69px">
        </td>
    </tr>
    <tr>
        <td style="width: 69px">
            &nbsp;
        </td>
        <td>
            Policy Validity Period
        </td>
        <td>
            <asp:DropDownList ID="ddlPolicyValidityPeriod" runat="server" CssClass="Dropdowncss" Width="150px" AutoPostBack="True"
                              OnSelectedIndexChanged="ddlPolicyValidityPeriod_SelectedIndexChanged"
                              TabIndex="36">
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

        <td style="width: 35px">
            &nbsp;
        </td>
        <td>
            Validity End Date<span style="color: Red">*</span>
        </td>
        <td>
            <asp:TextBox ID="txtValEDate" runat="server" oncut="return false;" onpaste="return false;"
                         Width="100px" BackColor="DarkGray" CssClass="search_3" ReadOnly="True" TabIndex="37">
            </asp:TextBox>
        </td>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td class="rowseparator" style="width: 69px">
        </td>
    </tr>
</table>
<br/>
&nbsp;&nbsp;&nbsp;

<legend align="center" style="color: brown">Inspection Information</legend>
<table align="center">

    <tr>
        <td class="rowseparator" style="width: 67px">
        </td>
    </tr>
    <tr>
    <td style="width: 67px">
        &nbsp;
    </td>
    <td>
        Inspection Date<span style="color: Red">*</span>
    </td>
    <td>
        <asp:TextBox ID="txtInspectionDate" runat="server" Width="105px" onkeypress="return false;"
                     oncut="return false;" CssClass="search_3" onpaste="return false;" TabIndex="38">
        </asp:TextBox>
    </td>
    <td>
        &nbsp;
    </td>

    <td style="width: 170px">
        &nbsp;
    </td>
    <td>
        Inspected By<span style="color: Red">*</span>
    </td>
    <td>
        <asp:TextBox ID="txtInspectedBy" runat="server" TabIndex="40" onkeypress="return alpha_only_withspace(event);"
                     Width="105px" CssClass="search_3" MaxLength="35">
        </asp:TextBox>
    </td>
    <td>
        &nbsp;
    </td>
    <tr>
        <td class="rowseparator" style="width: 67px">
        </td>
    </tr>
</tr>
</table>
&nbsp;&nbsp;&nbsp;
<br/>
<legend align="center" style="color: brown">Temporary Registration</legend>
<table align="center">

    <tr>
        <td class="rowseparator" style="width: 68px">
        </td>
    </tr>
    <tr>
        <td style="width: 68px">
            &nbsp;
        </td>
        <td>
            T/R No<span style="color: Red">*</span>
        </td>
        <td>
            <asp:TextBox ID="txtTRNo" runat="server" Width="100px" MaxLength="10" CssClass="search_3"
                         TabIndex="41">
            </asp:TextBox>
        </td>
        <td>
            &nbsp;
        </td>

        <td style="width: 170px">
            &nbsp;
        </td>
        <td>
            T/R Date<span style="color: Red">*</span>
        </td>
        <td>
            <asp:TextBox ID="txtTRDate" runat="server" oncut="return false;" onpaste="return false;"
                         Width="100px" CssClass="search_3" onkeypress="return false;" TabIndex="42">
            </asp:TextBox>
        </td>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td class="rowseparator" style="width: 68px">
        </td>
    </tr>
    <tr>
        <td style="width: 68px">
            &nbsp;
        </td>
        <td>
            T/R State<span style="color: Red">*</span>
        </td>
        <td>
            <asp:DropDownList ID="ddlTRDistrict" runat="server" Width="105px" TabIndex="44" CssClass="Dropdowncss">
            </asp:DropDownList>
        </td>
        <td>
            &nbsp;
        </td>

        <td style="width: 52px">
            &nbsp;
        </td>
        <td>
            Vehicle RTA Circle<span style="color: Red">*</span>
        </td>
        <td>
            <asp:TextBox ID="txtVeRTACircle" runat="server" Width="100px" MaxLength="20" CssClass="search_3"
                         onkeypress="return alpha_only_withspace(event);" TabIndex="45">
            </asp:TextBox>
        </td>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td class="rowseparator" style="width: 68px">
        </td>
    </tr>
    <tr>
        <td style="width: 68px">
            &nbsp;
        </td>
        <td>
            Road Tax Fee<span style="color: Red">*</span>
        </td>
        <td>
            <asp:TextBox ID="txtRoadTaxFee" runat="server" Width="100px" onkeypress="return numericOnly(this);" CssClass="search_3"
                         MaxLength="9" TabIndex="46">
            </asp:TextBox>
        </td>
        <td>
            &nbsp;
        </td>

        <td style="width: 52px">
            &nbsp;
        </td>
        <td>
            Seating Capacity<span style="color: Red">*</span>
        </td>
        <td>
            <asp:TextBox ID="txtSittingCapacity" runat="server" Width="100px" onkeypress="return numeric(event);" CssClass="search_3"
                         MaxLength="2" TabIndex="47">
            </asp:TextBox>
        </td>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td class="rowseparator" style="width: 68px">
        </td>
    </tr>
</table>
<br/>
<legend align="center" style="color: brown">Tyre Information</legend>
<table align="center">

    <tr>
        <td class="rowseparator" style="width: 70px">
        </td>
    </tr>
    <tr>
        <td style="width: 70px">
            &nbsp;
        </td>
        <td>
            FL<span style="color: Red">*</span>
        </td>
        <td>
            <asp:TextBox ID="txtFL" runat="server" Width="100px" MaxLength="10" CssClass="search_3"
                         onkeypress="return alphanumeric_only(event);" TabIndex="48">
            </asp:TextBox>
        </td>
        <td>
            &nbsp;
        </td>

        <td style="width: 170px">
            &nbsp;
        </td>
        <td>
            FR<span style="color: Red">*</span>
        </td>
        <td>
            <asp:TextBox ID="txtFR" runat="server" Width="100px" MaxLength="10" CssClass="search_3"
                         onkeypress="return alphanumeric_only(event);" TabIndex="49">
            </asp:TextBox>
        </td>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td class="rowseparator" style="width: 70px">
        </td>
    </tr>
    <tr>
        <td style="width: 70px">
            &nbsp;
        </td>
        <td>
            RL<span style="color: Red">*</span>
        </td>
        <td>
            <asp:TextBox ID="txtRL" runat="server" Width="100px" MaxLength="10" CssClass="search_3"
                         onkeypress="return alphanumeric_only(event);" TabIndex="50">
            </asp:TextBox>
        </td>
        <td>
            &nbsp;
        </td>

        <td style="width: 67px">
            &nbsp;
        </td>
        <td>
            RR<span style="color: Red">*</span>
        </td>
        <td>
            <asp:TextBox ID="txtRR" runat="server" Width="100px" MaxLength="10" CssClass="search_3"
                         onkeypress="return alphanumeric_only(event);" TabIndex="51">
            </asp:TextBox>
        </td>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td class="rowseparator" style="width: 70px">
        </td>
    </tr>
    <tr>
        <td style="width: 70px">
            &nbsp;
        </td>
        <td>
            Spare Wheel<span style="color: Red">*</span>
        </td>
        <td>
            <asp:TextBox ID="txtSpareWheel" runat="server" Width="100px" MaxLength="10" CssClass="search_3"
                         onkeypress="return alphanumeric_only(event);" TabIndex="52">
            </asp:TextBox>
        </td>
        <td>
            &nbsp;
        </td>

        <td style="width: 67px">
            &nbsp;
        </td>
        <td>
            Make<span style="color: Red">*</span>
        </td>
        <td>
            <asp:DropDownList ID="ddlTyreMake" runat="server" Width="105px" TabIndex="53" CssClass="Dropdowncss">
            </asp:DropDownList>
        </td>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td class="rowseparator" style="width: 70px">
        </td>
    </tr>
    <tr>
        <td style="width: 70px">
            &nbsp;
        </td>
        <td>
            Model-Size<span style="color: Red">*</span>
        </td>
        <td>
            <asp:DropDownList ID="ddlModelSize" runat="server" Width="105px" TabIndex="54" CssClass="Dropdowncss">
            </asp:DropDownList>
        </td>
        <td>
            &nbsp;
        </td>

        <td style="width: 67px">
            &nbsp;
        </td>
        <td>
            Odometer Reading<span style="color: Red">*</span>
        </td>
        <td>
            <asp:TextBox ID="txtOdometerReading" runat="server" Width="100px" onkeypress="return OnlyNumbers(event);"
                         MaxLength="12" CssClass="search_3" TabIndex="55">
            </asp:TextBox>
        </td>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td class="rowseparator" style="width: 70px">
        </td>
    </tr>
</table>
<br/>
&nbsp;&nbsp;&nbsp;
<legend align="center" style="color: brown">Battery Information</legend>
<table align="center">

    <tr>
        <td class="rowseparator" style="width: 70px">
        </td>
    </tr>
    <tr>
        <td style="width: 70px">
            &nbsp;
        </td>
        <td>
            Battery 1<span style="color: Red">*</span>
        </td>
        <td>
            <asp:TextBox ID="txtBattery1" runat="server" Width="100px" MaxLength="10"
                         onkeypress="return alphanumeric_only(event);" CssClass="search_3" TabIndex="56">
            </asp:TextBox>
        </td>
        <td>
            &nbsp;
        </td>

        <td style="width: 170px;">
            &nbsp;
        </td>
        <td>
            Battery 2<span style="color: Red">*</span>
        </td>
        <td>
            <asp:TextBox ID="txtBattery2" runat="server" Width="100px" MaxLength="10"
                         onkeypress="return alphanumeric_only(event);" CssClass="search_3" TabIndex="57">
            </asp:TextBox>
        </td>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td class="rowseparator" style="width: 70px">
        </td>
    </tr>
    <tr>
        <td style="width: 70px">
            &nbsp;
        </td>
        <td>
            Make<span style="color: Red">*</span>
        </td>
        <td>
            <asp:DropDownList ID="ddlBatteryMake" runat="server" Width="105px" CssClass="Dropdowncss"
                              TabIndex="58">
            </asp:DropDownList>
        </td>
        <td>
            &nbsp;
        </td>

        <td style="width: 70px">
            &nbsp;
        </td>
        <td>
            Model Capacity<span style="color: Red">*</span>
        </td>
        <td>
            <asp:DropDownList ID="ddlModelCapacity" runat="server" Width="150px" CssClass="Dropdowncss"
                              TabIndex="59">
            </asp:DropDownList>
        </td>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td class="rowseparator" style="width: 70px">
        </td>
    </tr>
    <tr>
        <td style="width: 70px">
            &nbsp;
        </td>
        <td colspan="2">
            <b>
                <asp:LinkButton ID="lbtnMapVehicleTools" runat="server" Text="Map Vehicle Tools"
                                Visible="False">
                </asp:LinkButton>
            </b>
        </td>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td class="rowseparator" style="width: 70px">
        </td>
    </tr>
</table>
<br/>
<asp:Button runat="server" Text="validate" Style="display: none"/>
</asp:Panel>
<table align="center">
    <caption>
        <img id="loaderButton" alt="" src="images/savingimage.gif" style="display: none"/>
        <tr>
            <td>
                <asp:Button ID="btnSave" runat="server" Text="Submit" Width="60px" TabIndex="60"
                            OnClientClick="return validation();" Height="30px" CssClass="form-submit-button"
                            OnClick="btnSave_Click"/>
            </td>
            <td style="width: 10px">
            </td>
            <td>
                <asp:Button ID="btnReset" runat="server" Text="Reset" TabIndex="61" Height="30px"
                            Width="60px" CssClass="form-submit-button" OnClick="btnReset_Click"/>
            </td>
        </tr>
    </caption>
</table>

</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>