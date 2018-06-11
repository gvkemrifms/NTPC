<%@ Page Language="C#" MasterPageFile="~/temp.master" AutoEventWireup="true" CodeFile="VehicleAccidentInvestigationDetails.aspx.cs" Inherits="VehicleAccidentInvestigationDetails" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<script type="text/javascript">
    function pageLoad() {
        $(
                '#<%= TxtAccidentDateTime.ClientID %>,#<%= TxtInsuranceStartDate.ClientID %>,#<%= TxtInsuranceEndDate.ClientID %>,#<%= TxtSpotSurveyorDate.ClientID %>,#<%= TxtFinalSurveyorDate.ClientID %>,#<%= TxtReinspectionSurveyorDate.ClientID %>,#<%= TxtClaimFormSubmissionDate.ClientID %>,#<%= TxtBillSubmissionDate.ClientID %>,#<%= TxtPaymentRecievedDate.ClientID %>')
            .datepicker({
                dateFormat: 'mm/dd/yy',
                changeMonth: true,
                changeYear: true
            });
    }

    function vehicleCostAddition(obj) {
        if (!parseFloat(obj.value)) {
            alert('The value should be a valid decimal value and cannot be zero');
            obj.value = '';
        } else {
            var totalCostofRepairs = document.getElementById('<%= TxtTotalCostofRepairs.ClientID %>').value === '' ? 0 : document.getElementById('<%= TxtTotalCostofRepairs.ClientID %>').value;
            var amountRecievedFromInsurance =
                document.getElementById('<%= TxtAmountRecievedFromInsurance.ClientID %>').value === '' ? 0 : document.getElementById('<%= TxtAmountRecievedFromInsurance.ClientID %>').value;
            var costToCompany = document.getElementById('<%= TxtCostToCompany.ClientID %>');
            costToCompany.value = (parseFloat(totalCostofRepairs) - parseFloat(amountRecievedFromInsurance)).toFixed(2);
        }
    }

    function validation() {
        var costToCompany = document.getElementById('<%= TxtCostToCompany.ClientID %>');
        costToCompany.value =
            (parseFloat(window.TotalCostofRepairs) - parseFloat(window.AmountRecievedFromInsurance)).toFixed(2);
    }
</script>
<asp:UpdatePanel runat="server">
<ContentTemplate>
<table align="center">
<tr>
<td>
<legend align="center" style="color: brown">Vehicle Accident Investigation Details </legend><br/>
<table align="center">
<tr>
    <td>
        Vehicle Number<asp:Label ID="lblVehicleNumber" runat="server" Text="" style="color: red">*</asp:Label>
    </td>
    <td>
        <cc1:ComboBox AutoCompleteMode="Append" ID="ddlistVehicleNumber" runat="server" Width="130px"
                      AutoPostBack="True" DropDownStyle="DropDownList" OnSelectedIndexChanged="ddlistVehicleNumber_SelectedIndexChanged">
        </cc1:ComboBox>
        <asp:TextBox ID="txtVehNum" runat="server"></asp:TextBox>

    </td>
    <td></td>
    <td></td>
    <td></td>
    <td>
        <asp:Label ID="LblPolicyNumber" runat="server" Text="Policy Number" style="margin-left: 40px"></asp:Label>
    </td>
    <td>
        <asp:TextBox ID="TxtPolicyNumber" CssClass="search_3" runat="server"></asp:TextBox>
    </td>
    <td></td>
</tr>
<tr>
    <td>
        <asp:Label ID="LblAccidentDateTime" runat="server" Text="Accident Date Time"></asp:Label>
    </td>
    <td>
        <asp:TextBox ID="TxtAccidentDateTime" CssClass="search_3" runat="server"></asp:TextBox>
    </td>
    <td></td>
    <td></td>
    <td></td>

    <td>
        <asp:Label ID="LblAgency" runat="server" Text="Agency" style="margin-left: 80px;"></asp:Label>
    </td>
    <td>
        <asp:TextBox ID="TxtAgency" CssClass="search_3" runat="server"></asp:TextBox>
    </td>
    <td></td>
</tr>
<tr>
    <td>
        <asp:Label ID="LblAccidentTitle" runat="server" Text="Accident Title"></asp:Label>
    </td>
    <td>
        <asp:TextBox ID="TxtAccidentTitle" CssClass="search_3" runat="server"></asp:TextBox>
    </td>
    <td></td>
    <td></td>
    <td></td>
    <td>
        <asp:Label ID="LblInsuranceStartDate" runat="server" Text="Insurance Start Date" style="margin-left: 40px"></asp:Label>
    </td>
    <td>
        <asp:TextBox ID="TxtInsuranceStartDate" CssClass="search_3" runat="server"></asp:TextBox>
    </td>
</tr>
<tr>
    <td>
        <asp:Label ID="LblSpotSurveyor" runat="server" Text="Spot Surveyor"></asp:Label>
    </td>
    <td>
        <asp:TextBox ID="TxtSpotSurveyor" CssClass="search_3" runat="server"></asp:TextBox>
    </td>
    <td></td>
    <td></td>
    <td></td>
    <td>
        <asp:Label ID="LblInsuranceEndDate" runat="server" Text="Insurance End Date" style="margin-left: 40px"></asp:Label>
    </td>
    <td>
        <asp:TextBox ID="TxtInsuranceEndDate" CssClass="search_3" runat="server"></asp:TextBox>
    </td>
</tr>
<tr>
    <td>
        <asp:Label ID="LblSpotSurveyorDate" runat="server" Text="Spot Surveillance Date"></asp:Label>
    </td>
    <td>
        <asp:TextBox ID="TxtSpotSurveyorDate" CssClass="search_3" runat="server"></asp:TextBox>
    </td>
    <td></td>
    <td></td>
</tr>
<tr>
    <td>
        <asp:Label ID="LblFinalSurveyor" runat="server" Text="Final Surveyor"></asp:Label>
    </td>
    <td>
        <asp:TextBox ID="TxtFinalSurveyor" CssClass="search_3" runat="server"></asp:TextBox>
    </td>
    <td></td>
    <td></td>
    <td></td>
    <td>
        <asp:Label ID="LblReinspectionSurveyor" runat="server" Text="Re-inspection Surveyor" style="margin-left: 40px"></asp:Label>
    </td>
    <td>
        <asp:TextBox ID="TxtReinspectionSurveyor" CssClass="search_3" runat="server"></asp:TextBox>
    </td>
</tr>
<tr>
    <td>
        <asp:Label ID="LblFinalSurveyorDate" runat="server" Text="Final Surveillance Date"></asp:Label>
    </td>
    <td>
        <asp:TextBox ID="TxtFinalSurveyorDate" CssClass="search_3" runat="server"></asp:TextBox>
    </td>
    <td>
    <td></td>
    <td></td>
    <td>
        <asp:Label runat="server" Text="Re-inspection Surveillance Date" style="margin-left: 30px"></asp:Label>
    </td>
    <td>
        <asp:TextBox ID="TxtReinspectionSurveyorDate" CssClass="search_3" runat="server"></asp:TextBox>
    </td>
</tr>

<tr>
    <td>
        <asp:Label runat="server" Text="Claim Form Submission Date"></asp:Label>
    </td>
    <td>
        <asp:TextBox ID="TxtClaimFormSubmissionDate" CssClass="search_3" runat="server"></asp:TextBox>
    </td>
</tr>
<tr>
    <td>
        <asp:Label runat="server" Text="Total Cost of Repairs"></asp:Label>
    </td>
    <td>
        <asp:TextBox ID="TxtTotalCostofRepairs" runat="server" CssClass="search_3" onchange="return vehicleCostAddition(this)"></asp:TextBox>
    </td>
    <td></td>
</tr>
<tr>
    <td>
        <asp:Label runat="server" Text="Surveyor Assessment Value"></asp:Label>
    </td>
    <td>
        <asp:TextBox ID="TxtSurveyorAssessmentValue" CssClass="search_3" runat="server"></asp:TextBox>
    </td>
    <td></td>
</tr>
<tr>
    <td>
        <asp:Label runat="server" Text="Bill Submission Date"></asp:Label>
    </td>
    <td>
        <asp:TextBox ID="TxtBillSubmissionDate" CssClass="search_3" runat="server"></asp:TextBox>
    </td>
</tr>
<tr>
    <td>
        <asp:Label runat="server" Text="Payment Status"></asp:Label>
    </td>
    <td>
        <asp:DropDownList ID="ddlistPaymentStatus" CssClass="search_3" runat="server">
            <asp:ListItem Value="0">--Select--</asp:ListItem>
            <asp:ListItem Value="1">Pending for Claim</asp:ListItem>
            <asp:ListItem Value="2">Under repair</asp:ListItem>
            <asp:ListItem Value="3">Bill Submitted</asp:ListItem>
            <asp:ListItem Value="4">Pending for Settlement</asp:ListItem>
        </asp:DropDownList>
    </td>
    <td></td>
</tr>
<tr>
    <td>
        <asp:Label runat="server" Text="Remarks"></asp:Label>
    </td>
    <td style="padding-top: 4px">
        <asp:TextBox ID="txtRemarks" runat="server" CssClass="search_3" MaxLength="250" TextMode="MultiLine"
                     Width="150px">
        </asp:TextBox>
    </td>
    <td></td>
</tr>
<tr>
    <td>
        <asp:Label runat="server" Text="Payment Recieved Date"></asp:Label>
    </td>
    <td>
        <asp:TextBox ID="TxtPaymentRecievedDate" CssClass="search_3" runat="server"></asp:TextBox>
    </td>
</tr>
<tr>
    <td>
        <asp:Label runat="server" Text="Cheque No"></asp:Label>
    </td>
    <td>
        <asp:TextBox ID="TxtChequeNo" CssClass="search_3" runat="server"></asp:TextBox>
    </td>
    <td></td>
</tr>
<tr>
    <td>
        <asp:Label runat="server" Text="Amount Recieved From Insurance"></asp:Label>
    </td>
    <td>
        <asp:TextBox ID="TxtAmountRecievedFromInsurance" CssClass="search_3" runat="server" onchange="return vehicleCostAddition(this)"></asp:TextBox>
    </td>
    <td></td>
</tr>
<tr>
    <td>
        <asp:Label runat="server" Text="Cost To Company"></asp:Label>
    </td>
    <td>
        <asp:TextBox ID="TxtCostToCompany" CssClass="search_3" runat="server" onkeypress="return false"></asp:TextBox>
    </td>

</tr>
</table>
<br>

<div align="center">
    <asp:Button ID="BtnSave" runat="server" CssClass="form-submit-button" Text="Save" Width="70px" OnClick="BtnSave_Click"/>

    <asp:Button ID="BtnReset" runat="server" CssClass="form-reset-button" Text="Reset" Width="70px"
                OnClick="BtnReset_Click"/>

</div>

<br/>
<table align="center">
    <tr>
        <td>
            <asp:GridView ID="gvVehicleDetails" AutoGenerateColumns="False" runat="server" BorderWidth="1px" CssClass="gridviewStyle"
                          CellPadding="3" Width="630px"
                          OnRowCommand="gvVehicleDetails_RowCommand" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None">
                <RowStyle CssClass="rowStyleGrid" ForeColor="#000066"/>

                <Columns>
                    <asp:TemplateField HeaderText="S No" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                        <ItemStyle Width="2%"/>
                    </asp:TemplateField>
                    <asp:BoundField DataField="VehicleNumber" HeaderText="Vehicle Number"
                                    ItemStyle-HorizontalAlign="Center">
                        <ItemStyle HorizontalAlign="Center"/>
                    </asp:BoundField>
                    <asp:BoundField DataField="AccidentDateTime" HeaderText="Accident DateTime"/>
                    <asp:BoundField DataField="AccidentTitle" HeaderText="Accident Title"/>
                    <asp:BoundField DataField="PayStatus" HeaderText="Status"/>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton runat="server" CommandName="_Details" Text="View Details" CommandArgument=" <%# Container.DataItemIndex %>"/>
                            <asp:HiddenField ID="hdnvehicelvalue" runat="server" Value='<%#Eval("VehicelInsuranceDetId") %>'/>
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
        </td>
    </tr>
</table>
</td>
</tr>
</table>
</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>