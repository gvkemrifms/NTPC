<%@ Page Language="C#" MasterPageFile="~/temp.master" AutoEventWireup="true" CodeFile="VehicleAccidentDetails.aspx.cs" Inherits="VehicleAccidentDetails" %>
<%@ Import Namespace="System.ComponentModel" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<script type="text/javascript">
    function pageLoad() {
        $('#<%= txtExpiryDate.ClientID %>,#<%= txtAccidentDateTime.ClientID %>,#<%= txtInitiatedTime.ClientID %>')
            .datepicker(
                {
                    dateFormat: 'mm/dd/yy',
                    changeMonth: true,
                    changeYear: true
                });
    };

    function validation(obj, id) {

        var now = new Date();
        id = document.getElementById('<%= ddlistVehicleNumber.ClientID %>');
        var incidentTitle = document.getElementById('<%= txtIncidentTitle.ClientID %>');
        var kilometerRun = document.getElementById('<%= txtKilometerRun.ClientID %>');
        var accidentDateTime = document.getElementById('<%= txtAccidentDateTime.ClientID %>');
        var initiatedTime = document.getElementById('<%= txtInitiatedTime.ClientID %>');
        var hour = document.getElementById('<%= ddlistHour.ClientID %>');
        var minute = document.getElementById('<%= ddlistMinute.ClientID %>');
        var hour1 = document.getElementById('<%= ddlistInitiatedHr.ClientID %>');
        var minute1 = document.getElementById('<%= ddlistInitiatedTimeMin.ClientID %>');
        var pilotName = document.getElementById('<%= txtPilotName.ClientID %>');
        var drivingLicenseNumber = document.getElementById('<%= txtDrivingLicenseNumber.ClientID %>');
        var expiryDate = document.getElementById('<%= txtExpiryDate.ClientID %>');


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

        switch (trim(incidentTitle.value)) {
        case '':
            alert("Incident Title Cannot be Blank");
            incidentTitle.focus();
            return false;
        }

        switch (trim(kilometerRun.value)) {
        case '':
            alert("Kilometer Run Cannot be Blank");
            kilometerRun.focus();
            return false;
        }

        switch (trim(pilotName.value)) {
        case '':
            alert("Pilot Name Cannot be Blank");
            pilotName.focus();
            return false;
        }

        switch (trim(drivingLicenseNumber.value)) {
        case '':
            alert("Driving LicenseNumber Cannot be Blank");
            drivingLicenseNumber.focus();
            return false;
        }

        switch (trim(expiryDate.value)) {
        case '':
            alert("ExpiryDate cannot be Blank");
            expiryDate.focus();
            return false;
        }

        if (trim(expiryDate.value) !== "") {
            if (!isValidDate(expiryDate.value)) {
                alert("Enter the Valid Date");
                expiryDate.focus();
                return false;
            }
        }

        if (Date.parse(expiryDate.value) < Date.parse(now)) {
            alert("Expiry Date should not be less than Current Date");
            expiryDate.focus();
            return false;
        }

        if (trim(accidentDateTime.value) !== "") {
            if (!isValidDate(accidentDateTime.value)) {
                alert("Enter the Valid Date");
                accidentDateTime.focus();
                return false;
            }
        }

        if (Date.parse(accidentDateTime.value) > Date.parse(now)) {
            alert("Accident Date should not be greater than Current Date");
            accidentDateTime.focus();
            return false;
        }


        //txtInitiatedTime
        if (trim(initiatedTime.value) !== "") {
            if (!isValidDate(initiatedTime.value)) {
                alert("Enter the Valid Date");
                initiatedTime.focus();
                return false;
            }
        }

        if (Date.parse(initiatedTime.value) > Date.parse(now)) {
            alert("Initiated Date should not be greater than Current Date");
            initiatedTime.focus();
            return false;
        }
        if (Date.parse(accidentDateTime.value) > Date.parse(initiatedTime.value)) {
            alert("Initiated Date should be greater than Accident Date");
            initiatedTime.focus();
            return false;
        }

        if (hour.selectedIndex === 0) {
            alert("Please select hour");
            hour.focus();
            return false;
        }

        if (minute.selectedIndex === 0) {
            alert("Please select minute");
            minute.focus();
            return false;
        }

        if (hour1.selectedIndex === 0) {
            alert("Please select hour");
            hour1.focus();
            return false;
        }

        if (minute1.selectedIndex === 0) {
            alert("Please select minute");
            minute1.focus();
            return false;
        }

        if (parseInt(hour.value) > parseInt(hour1.value)) {
            alert("End Time should be greater than the Start Time value");
            hour1.focus();
            return false;
        }
        if (parseInt(hour.value) === parseInt(hour1.value)) {
            if (parseInt(minute.value) > parseInt(minute1.value)) {
                alert("End Time should be greater than the Start Time value");
                minute1.focus();
                return false;
            } else if (parseInt(minute.value) === parseInt(minute1.value)) {
                alert("End Time should be greater than the Start Time value");
                minute1.focus();
                return false;
            }
        }
        return true;
    }
</script>
<asp:UpdatePanel runat="server">
<ContentTemplate>
<asp:Panel ID="pnlVehicleAccidentDetails" runat="server">
<legend align="center" style="color: brown">
    <b>Vehicle Accident Details</b>
</legend>
<br/>
<table cellpadding="2" cellspacing="2" width="100%" style="margin-left: 50px;">
<tr>
<td valign="top" colspan="3">
<tr>
<td colspan="3">
    <b style="color: brown">
        <u>Incident and Vehicle Details</u>
    </b>
</td>
<td colspan="3" valign="top">
<b style="color: brown">
    <u>Resource Details</u>
</b>
<tr>
    <td>
        Vehicle Number <span style="color: red">*</span>
    </td>
    <td colspan="2">
        <cc1:ComboBox AutoCompleteMode="Append" ID="ddlistVehicleNumber" runat="server" Width="130px"
                      AutoPostBack="True"
                      OnSelectedIndexChanged="ddlistVehicleNumber_SelectedIndexChanged" DropDownStyle="DropDownList">
        </cc1:ComboBox>


    </td>
    <td>
        Pilot Name<span style="color: red">*</span>
    </td>
    <td colspan="2">
        <asp:TextBox ID="txtPilotName" CssClass="search_3" runat="server" Width="130px" onkeypress="return alpha_only(event);"></asp:TextBox>
    </td>
</tr>
<tr>
    <td>
        Incident Title<span style="color: red">*</span>
    </td>
    <td colspan="2">
        <asp:TextBox ID="txtIncidentTitle" CssClass="search_3" runat="server" MaxLength="20" Width="130px"
                     onkeypress="return alphanumeric_only_withspace(event);">
        </asp:TextBox>
    </td>
    <td>
        Driving License Number <span style="color: red">*</span>
    </td>
    <td colspan="2">
        <asp:TextBox ID="txtDrivingLicenseNumber" CssClass="search_3" runat="server" MaxLength="20" Width="130px"
                     onkeypress="return alphanumeric_only(event);">
        </asp:TextBox>
    </td>
</tr>
<tr>
    <td>
        Age of the Vehicle
    </td>
    <td colspan="2">
        <asp:TextBox ID="txtAgeofVehicle" CssClass="search_3" runat="server" MaxLength="10" Width="130px" onkeypress="return isDecimalNumberKey(event);" ReadOnly="true"></asp:TextBox>
    </td>
    <td>
        Expiry Date<span style="color: red">*</span>
    </td>
    <td>
        <asp:TextBox ID="txtExpiryDate" CssClass="search_3" runat="server" MaxLength="20" Width="130px" onkeypress="return false;"></asp:TextBox>
    </td>

</tr>
<tr>
    <td>
        Kilometer Run<span style="color: red">*</span>
    </td>
    <td colspan="2">
        <asp:TextBox ID="txtKilometerRun" CssClass="search_3" runat="server" MaxLength="15" onkeypress="return isDecimalNumberKey(event);"
                     Width="130px">
        </asp:TextBox>
    </td>
    <td>
        EMT Name
    </td>
    <td>
        <asp:TextBox ID="txtEmtName" CssClass="search_3" runat="server" Width="130px" onkeypress="return alpha_only(event);"></asp:TextBox>
    </td>
</tr>
<tr>
    <td>
        Incident Handled By
    </td>
    <td colspan="2">
        <asp:TextBox ID="txtIncidentHandledBy" CssClass="search_3" runat="server" MaxLength="35" Width="130px"
                     onkeypress="return alpha_only_withspace(event);">
        </asp:TextBox>
    </td>
    <td>
        Is Vehicle Operational
    </td>
    <td colspan="2">
        <asp:RadioButtonList ID="rdBtnIsVehicleOPerational" runat="server">
            <asp:ListItem Text="Yes" Value="True" runat="server"></asp:ListItem>
            <asp:ListItem Text="No" Value="False" Selected="True" runat="server"></asp:ListItem>
        </asp:RadioButtonList>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="rdBtnIsVehicleOPerational" ErrorMessage="RequiredFieldValidator"></asp:RequiredFieldValidator>
    </td>

</tr>
</td>
<tr>
    <td>
        &nbsp;
    </td>
    <td colspan="2">
        &nbsp;
    </td>
    <td>
        &nbsp;
    </td>
    <td colspan="2">
        &nbsp;
    </td>
</tr>
<tr>
    <td colspan="3">
        <b style="color: brown">
            <u>Initiation</u>
        </b>
    </td>
    <td colspan="3">
        <b style="color: brown">
            <u>Incident Impact Details</u>
        </b>
    </td>
</tr>
<tr>
    <td rowspan="2">
        Accident Description
    </td>
    <td colspan="2" rowspan="2">
        <asp:TextBox ID="txtAccidentDescription" CssClass="search_3" runat="server" MaxLength="250" TextMode="MultiLine"
                     Width="130px" onkeypress="return remark(event);">
        </asp:TextBox>
    </td>
    <td>
        Injuries to EMRI Staff
    </td>
    <td colspan="2">
        <asp:TextBox ID="txtInjuriestoEMRIStaff" CssClass="search_3" runat="server" MaxLength="20" Width="130px"
                     onkeypress="return numericOnly(this);">
        </asp:TextBox>
    </td>
</tr>
<tr>
    <td wrap="nowrap">
        Injuries to 3rd Party Personal
    </td>
    <td colspan="2">
        <asp:TextBox ID="txt3rdPartyPersonal" CssClass="search_3" runat="server" MaxLength="20" Width="130px"
                     onkeypress="return numericOnly(this);">
        </asp:TextBox>
    </td>
</tr>
<tr>
    <td style="height: 14px" rowspan="2">
        Accident Date and Time<span style="color: red">*</span>
    </td>
    <td style="height: 14px" rowspan="2">
        <asp:TextBox ID="txtAccidentDateTime" CssClass="search_3" runat="server" MaxLength="20" Width="130px"
                     onkeypress="return false;">
        </asp:TextBox>
        <asp:DropDownList ID="ddlistHour" CssClass="search_3" runat="server" Width="50px">
            <asp:ListItem Selected="True" Text="--hh--" Value="--hh--"></asp:ListItem>
        </asp:DropDownList>
        <asp:DropDownList ID="ddlistMinute" CssClass="search_3" runat="server" Width="50px">
            <asp:ListItem Selected="True" Text="--mm--" Value="--hh--"></asp:ListItem>
        </asp:DropDownList>
    </td>
    <td style="height: 14px" rowspan="2"></td>
    <td style="height: 14px" nowrap="nowrap">
        Approx Repair Cost
    </td>
    <td style="height: 14px" colspan="2">
        <asp:TextBox ID="txtApproxRepairCost" CssClass="search_3" runat="server" MaxLength="7" onkeypress="return numericOnly(event);"
                     Width="130px">
        </asp:TextBox>
    </td>
</tr>
<tr>
    <td nowrap="nowrap" style="height: 14px">
        &nbsp;
    </td>
    <td colspan="2" style="height: 14px">
        &nbsp;
    </td>
</tr>
<tr>
    <td>
        Action Initiated By
    </td>
    <td colspan="2">
        <asp:TextBox ID="txtActionInitiatedBy" CssClass="search_3" runat="server" MaxLength="35" Width="130px"
                     onkeypress="return alpha_only_withspace(event);">
        </asp:TextBox>
    </td>
</tr>
<tr>
    <td>
        Initiated Time<span style="color: red">*</span>
    </td>
    <td>
        <asp:TextBox ID="txtInitiatedTime" CssClass="search_3" runat="server" MaxLength="20" Width="130px" onkeypress="return false;"></asp:TextBox>
        <asp:DropDownList ID="ddlistInitiatedHr" CssClass="search_3" runat="server" Width="50px">
        </asp:DropDownList>
        <asp:DropDownList ID="ddlistInitiatedTimeMin" CssClass="search_3" runat="server" Width="50px">
            <asp:ListItem Selected="True" Text="--mm--" Value="--hh--"></asp:ListItem>
        </asp:DropDownList>
    </td>
    <td>
        &nbsp;
    </td>
    <td colspan="3">
        <b style="color: brown">
            <u>Other Details</u>
        </b>
    </td>
</tr>
<tr>
    <td>
        Initial Containment Action
    </td>
    <td colspan="2">
        <asp:TextBox ID="txtInitialContainmentAction" CssClass="search_3" runat="server" MaxLength="35" Width="130px"
                     onkeypress="return alphanumeric_only_withspace(event);">
        </asp:TextBox>
    </td>
    <td>
        Area Police Station
    </td>
    <td colspan="2">
        <asp:TextBox ID="txtAreaPoliceStation" CssClass="search_3" runat="server" MaxLength="35" Width="130px"
                     onkeypress="return alphanumeric_only_withspace(event);">
        </asp:TextBox>
    </td>
</tr>
</tr>
<tr>
    <td>
        Accident Root Cause
    </td>
    <td colspan="2">
        <asp:TextBox ID="txtAccidentRootCause" CssClass="search_3" runat="server" MaxLength="50" Width="130px"
                     onkeypress="return alphanumeric_only_withspace(event);">
        </asp:TextBox>
    </td>
    <td>
        CD/FIR NO/Panchnama
    </td>
    <td colspan="2">
        <asp:TextBox ID="txtFirPanchname" CssClass="search_3" runat="server" MaxLength="35" Width="130px"
                     onkeypress="return remark(event);">
        </asp:TextBox>
    </td>
</tr>
<tr>
    <td colspan="3" width="100">
        &nbsp;
    </td>
    <td style="width: 0">
        ReportedBy
    </td>
    <td width="100" colspan="2">
        <asp:TextBox ID="txtReportedBY" CssClass="search_3" runat="server" MaxLength="35" Width="130px"
                     onkeypress="return alpha_only_withspace(event);">
        </asp:TextBox>
    </td>
</tr>
<tr>
    <td colspan="3" width="100" nowrap="nowrap">
        <b style="color: brown">
            <u>Damage Details</u>
        </b>&nbsp;
    </td>
    <td style="width: 0">
        Remarks
    </td>
    <td colspan="2" width="100">
        <asp:TextBox ID="txtRemarks" CssClass="search_3" runat="server" MaxLength="200"
                     TextMode="MultiLine" Width="130px"
                     onkeypress="return remark(event);">
        </asp:TextBox>
    </td>
</tr>
<tr>
    <td>
        Damage to Ambulance
    </td>
    <td colspan="2">
        <asp:TextBox ID="txtDamagetoAmbulance" CssClass="search_3" runat="server" MaxLength="200" TextMode="MultiLine"
                     Width="130px" onkeypress="return remark(event);">
        </asp:TextBox>
    </td>
    <td>
        &nbsp;Is Insurance Claim Required
    </td>
    <td colspan="2">
        <asp:RadioButtonList name="InsClaimed" ID="rdBtnIsInsuranceClaimed" runat="server"
                             RepeatDirection="Horizontal">
            <asp:ListItem Value="True" Text="Yes"></asp:ListItem>
            <asp:ListItem Value="False" Text="No"></asp:ListItem>
        </asp:RadioButtonList>

    </td>
</tr>
<tr>
    <td nowrap="nowrap">
        Damage to 3rd Party Property
    </td>
    <td nowrap="nowrap" colspan="2">
        <asp:TextBox ID="txtDamageto3rdPartyProperty" CssClass="search_3" runat="server" MaxLength="200" TextMode="MultiLine"
                     Width="130px" onkeypress="return remark(event);">
        </asp:TextBox>
    </td>
    <td>
        &nbsp;
    </td>
    <td colspan="2">
        &nbsp;
    </td>
</tr>
<tr>
    <td colspan="3" nowrap="nowrap">
        &nbsp;
    </td>
    <td colspan="3">
        &nbsp;
    </td>
</tr>
<tr>
    <td colspan="6" align="center">
        <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" CssClass="form-submit-button" Text="Save"/>
        <asp:Button ID="btnReset" runat="server" OnClick="btnReset_Click" CssClass="form-reset-button" Text="Reset"/>
    </td>
</tr>
<tr>
    <td colspan="6">
        &nbsp;
    </td>
</tr>
</td>
</tr>
<tr>
    <td class="rowseparator" colspan="3" width="100"></td>
</tr>
<tr>
    <td colspan="6" nowrap="nowrap" width="100"></td>
</tr>

</table>
<br/>
</asp:Panel>
<table align="center">
    <tr>
        <td>
            <asp:GridView ID="grdVehicleAccidentDetails" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                          OnPageIndexChanging="grdVehicleAccidentDetails_PageIndexChanging" Width="100%" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3">
                <Columns>
                    <asp:TemplateField HeaderText="Accident Title">
                        <ItemTemplate>
                            <asp:Label ID="lblAccidentTitle" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Accident Date and Time">
                        <ItemTemplate>
                            <asp:Label ID="lblAccidentDateandTime" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Vehicle Number">
                        <ItemTemplate>
                            <asp:Label ID="lblVehicleNumber" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="District">
                        <ItemTemplate>
                            <asp:Label ID="lblDistrict" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="location">
                        <ItemTemplate>
                            <asp:Label ID="lbllocation" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkBtnEdit" runat="server" CommandArgument='<% DataBinder.Eval(Container.Dataitem, "") %>'
                                            CommandName="vehicleAccidentedit" Text="Edit">
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkBtnDelete" runat="server" CommandArgument='<% DataBinder.Eval(Container.Dataitem, "") %>'
                                            CommandName="vehicleAccidentDelete" Text="Delete">
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <FooterStyle BackColor="White" ForeColor="#000066"/>
                <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White"/>
                <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left"/>
                <RowStyle ForeColor="#000066"/>
                <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White"/>
                <SortedAscendingCellStyle BackColor="#F1F1F1"/>
                <SortedAscendingHeaderStyle BackColor="#007DBB"/>
                <SortedDescendingCellStyle BackColor="#CAC9C9"/>
                <SortedDescendingHeaderStyle BackColor="#00547E"/>
            </asp:GridView>
        </td>
    </tr>
</table>
</ContentTemplate>
</asp:UpdatePanel>

</asp:Content>