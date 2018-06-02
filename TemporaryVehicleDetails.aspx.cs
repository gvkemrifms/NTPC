using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using GvkFMSAPP.BLL;

public partial class TemporaryVehicleDetails : Page
{
    private readonly BaseVehicleDetails _basevehdet = new BaseVehicleDetails();
    private readonly Helper _helper = new Helper();
    private readonly GvkFMSAPP.BLL.Prior_MaintenanceStage.TemporaryVehicleDetails _tempvehdet = new GvkFMSAPP.BLL.Prior_MaintenanceStage.TemporaryVehicleDetails();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Name"] == null) Response.Redirect("Login.aspx");
        if (!IsPostBack)
        {
            var dsPerms = (DataSet) Session["PermissionsDS"];
            dsPerms.Tables[0].DefaultView.RowFilter = "Url='" + Page.Request.Url.Segments[Page.Request.Url.Segments.Length - 1] + "'";
            var btnTempVehDetail = tempVehDetWizard.FindControl("StartNavigationTemplateContainerID").FindControl("StartNextButton") as Button;
            if (btnTempVehDetail != null) btnTempVehDetail.Attributes.Add("onclick", "return validation(this,'" + btnTempVehDetail.ID + "')");
            var btnTempVehDetailStepNextButton = tempVehDetWizard.FindControl("StepNavigationTemplateContainerID").FindControl("StepNextButton") as Button;
            if (btnTempVehDetailStepNextButton != null) btnTempVehDetailStepNextButton.Attributes.Add("onclick", "return stepvalidation()");
            var btnbaseVechDetailStepPreviousButton = tempVehDetWizard.FindControl("StepNavigationTemplateContainerID").FindControl("StepPreviousButton") as Button;
            if (btnbaseVechDetailStepPreviousButton != null) btnbaseVechDetailStepPreviousButton.Attributes.Add("onclick", "return previousValidation()");
            var btnTempVehDetailFinishButton = tempVehDetWizard.FindControl("FinishNavigationTemplateContainerID").FindControl("FinishButton") as Button;
            if (btnTempVehDetailFinishButton != null) btnTempVehDetailFinishButton.Attributes.Add("onclick", "return finalStepValidation()");
            GetVehicleType();
            GetTyreMake();
            GetTyreModelSize();
            GetBatteryMake();
            GetBatteryModelCapacity();
            GetDistrict();
            GetVehicleModel();
            GetManufacturerName();
            GetFuelType();
            GetInsuranceType();
            GetAgency();
            pnlTemporaryVehicleDetails.Visible = false;
        }
    }

    public void GetVehicleType()
    {
        try
        {
            var ds = _tempvehdet.GetVehicleType();
            if (ds != null) _helper.FillDropDownHelperMethodWithDataSet(ds, "Vehicle_Type", "VehicleType_Id", ddlVehicleType);
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    public void GetTyreMake()
    {
        var ds = _tempvehdet.GetTyreMake();
        if (ds != null)
            try
            {
                _helper.FillDropDownHelperMethodWithDataSet(ds, "Make", "Tyre_Id", ddlTyreMakeFL);
                _helper.FillDropDownHelperMethodWithDataSet(ds, "Make", "Tyre_Id", ddlTyreMakeFR);
                _helper.FillDropDownHelperMethodWithDataSet(ds, "Make", "Tyre_Id", ddlTyreMakeRL);
                _helper.FillDropDownHelperMethodWithDataSet(ds, "Make", "Tyre_Id", ddlTyreMakeRR);
                _helper.FillDropDownHelperMethodWithDataSet(ds, "Make", "Tyre_Id", ddlTyreMakeSpareWheel);
            }
            catch (Exception ex)
            {
                _helper.ErrorsEntry(ex);
            }
    }

    public void GetTyreModelSize()
    {
        var ds = _tempvehdet.GetTyreModelSize();
        if (ds != null)
            try
            {
                _helper.FillDropDownHelperMethodWithDataSet(ds, "ModelSize", "Tyre_Id", ddlModelSizeFL);
                _helper.FillDropDownHelperMethodWithDataSet(ds, "ModelSize", "Tyre_Id", ddlModelSizeFR);
                _helper.FillDropDownHelperMethodWithDataSet(ds, "ModelSize", "Tyre_Id", ddlModelSizeRL);
                _helper.FillDropDownHelperMethodWithDataSet(ds, "ModelSize", "Tyre_Id", ddlModelSizeRR);
                _helper.FillDropDownHelperMethodWithDataSet(ds, "ModelSize", "Tyre_Id", ddlModelSizeSpareWheel);
            }
            catch (Exception ex)
            {
                _helper.ErrorsEntry(ex);
            }
    }

    public void GetBatteryMake()
    {
        var ds = _tempvehdet.GetBatteryMake();
        if (ds != null)
            try
            {
                _helper.FillDropDownHelperMethodWithDataSet(ds, "Make", "Battery_Id", ddlBatteryMakeBattery1);
                _helper.FillDropDownHelperMethodWithDataSet(ds, "Make", "Battery_Id", ddlBatteryMakeBattery2);
            }
            catch (Exception ex)
            {
                _helper.ErrorsEntry(ex);
            }
    }

    public void GetAgency()
    {
        try
        {
            var ds = _basevehdet.GetAgency();
            if (ds != null) _helper.FillDropDownHelperMethodWithDataSet(ds, "InsuranceAgency", "InsuranceId", ddlAgency);
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    public void GetInsuranceType()
    {
        try
        {
            var ds = _basevehdet.GetInsuranceType();
            if (ds != null) _helper.FillDropDownHelperMethodWithDataSet(ds, "InsuranceTypeName", "InsuranceTypeId", ddlInsType);
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    public void GetFuelType()
    {
        try
        {
            var ds = _basevehdet.GetFuelType();
            if (ds != null) _helper.FillDropDownHelperMethodWithDataSet(ds, "FuelTypeName", "FuelTypeId", ddlFuelType);
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    public void GetDistrict()
    {
        try
        {
            var ds = _tempvehdet.GetDistrict();
            if (ds != null) _helper.FillDropDownHelperMethodWithDataSet(ds, "ds_lname", "ds_dsid", ddlDistrict);
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    public void GetVehicleModel()
    {
        try
        {
            var ds = _basevehdet.GetVehicleModel();
            if (ds != null) _helper.FillDropDownHelperMethodWithDataSet(ds, "VehicleModelName", "VehicleModelId", ddlVehicleModel);
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    public void GetManufacturerName()
    {
        try
        {
            var ds = _basevehdet.GetManufacturerName();
            if (ds != null) _helper.FillDropDownHelperMethodWithDataSet(ds, "FleetManufacturer_Name", "FleetManufacturer_Id", ddlManufacturerName);
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    public void GetBatteryModelCapacity()
    {
        var ds = _tempvehdet.GetBatteryModelCapacity();
        if (ds != null)
            try
            {
                _helper.FillDropDownHelperMethodWithDataSet(ds, "ModelCapacity", "Battery_Id", ddlModelCapacityBattery1);
                _helper.FillDropDownHelperMethodWithDataSet(ds, "ModelCapacity", "Battery_Id", ddlModelCapacityBattery2);
            }
            catch (Exception ex)
            {
                _helper.ErrorsEntry(ex);
            }
    }

    protected void Wizard1_FinishButtonClick(object sender, WizardNavigationEventArgs e)
    {
        _tempvehdet.EngineNo = txtEngineNo.Text;
        _tempvehdet.ChassisNo = txtChassisNo.Text;
        _tempvehdet.VehicleNo = txtVehicleNo.Text;
        _tempvehdet.InspectedBy = txtInspectedBy.Text;
        _tempvehdet.InspectedDate = DateTime.Parse(txtInspectedDate.Text);
        _tempvehdet.VehicleTypeIBD = ""; // ddlVehicleTypeIBD.SelectedItem.Value.ToString();
        _tempvehdet.VehicleStatus = ddlVehicleStatus.SelectedItem.Text;
        _tempvehdet.VehicleModel = ddlVehicleModel.SelectedItem.Value;
        _tempvehdet.KMPL = float.Parse(txtKmpl.Text);
        _tempvehdet.VehicleType = ddlVehicleType.SelectedItem.Value;
        _tempvehdet.VehicleEmissionType = txtVehicleEmissionType.Text;
        _tempvehdet.PurchaseDate = DateTime.Parse(txtPurchaseDate.Text);
        _tempvehdet.OwnerName = txtOwnerName.Text;
        _tempvehdet.ManufacturerName = ddlManufacturerName.SelectedItem.Value;
        _tempvehdet.VehicleCost = float.Parse(txtVehicleCost.Text);
        _tempvehdet.ManufacturingDate = DateTime.Parse(txtManufacturingDate.Text);
        _tempvehdet.EngineCapacity = txtEngineCapacity.Text;
        _tempvehdet.FuelType = ddlFuelType.SelectedItem.Value;
        _tempvehdet.District = Convert.ToInt32(ddlDistrict.SelectedItem.Value);
        _tempvehdet.InsuranceType = Convert.ToInt32(ddlInsType.SelectedItem.Value);
        _tempvehdet.InsuranceAgency = Convert.ToInt32(ddlAgency.SelectedItem.Value);
        _tempvehdet.InsurancePolicyNo = txtInPolicyNo.Text;
        _tempvehdet.CurrentPolicyEndDate = DateTime.Parse(txtCurrentPolicyEndDate.Text);
        _tempvehdet.InsuranceReceiptNo = txtInsuranceReceiptNo.Text;
        _tempvehdet.InsuranceFeesPaid = float.Parse(txtInsFee.Text);
        _tempvehdet.InsuranceFeesPaidDate = DateTime.Parse(txtInsuranceFeesPaidDate.Text);
        _tempvehdet.PolicyStartDate = DateTime.Parse(txtValiSDate.Text);
        _tempvehdet.PolicyValidityPeriod = ddlPolicyValidityPeriod.SelectedItem.Value;
        _tempvehdet.PolicyEndDate = DateTime.Parse(txtValEDate.Text);
        _tempvehdet.FL = txtFL.Text;
        _tempvehdet.FR = txtFR.Text;
        _tempvehdet.RL = txtRL.Text;
        _tempvehdet.RR = txtRR.Text;
        _tempvehdet.Stephny = txtSpareWheel.Text;
        _tempvehdet.TyreMakeFL = ddlTyreMakeFL.Text;
        _tempvehdet.TyreModelSizeFL = int.Parse(ddlModelSizeFL.Text);
        _tempvehdet.TyreMakeFR = ddlTyreMakeFR.Text;
        _tempvehdet.TyreModelSizeFR = int.Parse(ddlModelSizeFR.Text);
        _tempvehdet.TyreMakeRL = ddlTyreMakeRL.Text;
        _tempvehdet.TyreModelSizeRL = int.Parse(ddlModelSizeRL.Text);
        _tempvehdet.TyreMakeRR = ddlTyreMakeRR.Text;
        _tempvehdet.TyreModelSizeRR = int.Parse(ddlModelSizeRR.Text);
        _tempvehdet.TyreMakeSpareWheel = ddlTyreMakeSpareWheel.Text;
        _tempvehdet.TyreModelSizeSpareWheel = int.Parse(ddlModelSizeSpareWheel.Text);
        _tempvehdet.OdometerReading = long.Parse(txtOdometerReading.Text);
        _tempvehdet.Battery1 = txtBattery1.Text;
        _tempvehdet.Battery2 = txtBattery2.Text;
        _tempvehdet.BatteryMakeBattery1 = ddlBatteryMakeBattery1.Text;
        _tempvehdet.BatteryModelCapacityBattery1 = int.Parse(ddlModelCapacityBattery1.Text);
        _tempvehdet.BatteryMakeBattery2 = ddlBatteryMakeBattery2.Text;
        _tempvehdet.BatteryModelCapacityBattery2 = int.Parse(ddlModelCapacityBattery2.Text);
        var vehOut = _tempvehdet.ValidateVehicleDetails();
        var tyreOut = _tempvehdet.ValidateTyre();
        var batteryOut = _tempvehdet.ValidateBattery();
        if (vehOut == 0 && tyreOut == 0 && batteryOut == 0)
        {
            var ret = _tempvehdet.InsTempVehicleDetails();
            Show(ret > 0 ? "Record Inserted Successfully" : "Error");
            ClearControls();
            tempVehDetWizard.MoveTo(WizardStep1);
        }
        else
        {
            if (vehOut > 0)
                switch (vehOut)
                {
                    case 1:
                        Show("Engine Number is already present");
                        break;
                    case 2:
                        Show("Chassis Number is already present");
                        break;
                    default:
                        Show("Vehicle Number is already present");
                        break;
                }
            if (tyreOut > 0)
                switch (tyreOut)
                {
                    case 1:
                        Show("FL Tyre is already present");
                        break;
                    case 2:
                        Show("FR Tyre is already present");
                        break;
                    case 3:
                        Show("RL Tyre is already present");
                        break;
                    case 4:
                        Show("RR Tyre is already present");
                        break;
                    default:
                        Show("Stephny Tyre is already present");
                        break;
                }
            if (batteryOut > 0) Show(batteryOut == 1 ? "Battery1 is already present" : "Battery2 is already present");
        }
    }

    public void Show(string message)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('" + message + "');", true);
    }

    protected void ddlModelCapacityBattery1_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void ddlPolicyValidityPeriod_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlPolicyValidityPeriod.SelectedIndex != 0) txtValEDate.Text = Convert.ToDateTime(txtValiSDate.Text).AddMonths(Convert.ToInt16(ddlPolicyValidityPeriod.SelectedItem.Value)).ToString();
    }

    public void ClearControls()
    {
        txtEngineNo.Text = "";
        txtChassisNo.Text = "";
        txtVehicleNo.Text = "";
        txtInspectedBy.Text = "";
        txtInspectedDate.Text = "";
        ddlVehicleStatus.SelectedIndex = 0;
        ddlVehicleModel.SelectedIndex = 0;
        txtKmpl.Text = "";
        ddlVehicleType.SelectedIndex = 0;
        txtVehicleEmissionType.Text = "";
        txtPurchaseDate.Text = "";
        txtOwnerName.Text = "";
        ddlManufacturerName.SelectedIndex = 0;
        txtVehicleCost.Text = "";
        txtManufacturingDate.Text = "";
        txtEngineCapacity.Text = "";
        ddlFuelType.SelectedIndex = 0;
        ddlDistrict.SelectedIndex = 0;
        ddlInsType.SelectedIndex = 0;
        ddlAgency.SelectedIndex = 0;
        txtInPolicyNo.Text = "";
        txtInsuranceReceiptNo.Text = "";
        txtInsFee.Text = "";
        txtInsuranceFeesPaidDate.Text = "";
        txtValiSDate.Text = "";
        ddlPolicyValidityPeriod.SelectedIndex = 0;
        txtValEDate.Text = "";
        txtFL.Text = "";
        txtFR.Text = "";
        txtRL.Text = "";
        txtRR.Text = "";
        txtSpareWheel.Text = "";
        ddlTyreMakeFL.SelectedIndex = 0;
        ddlModelSizeFL.SelectedIndex = 0;
        ddlTyreMakeFR.SelectedIndex = 0;
        ddlModelSizeFR.SelectedIndex = 0;
        ddlTyreMakeRL.SelectedIndex = 0;
        ddlModelSizeRL.SelectedIndex = 0;
        ddlTyreMakeRR.SelectedIndex = 0;
        ddlModelSizeRR.SelectedIndex = 0;
        ddlTyreMakeSpareWheel.SelectedIndex = 0;
        ddlModelSizeSpareWheel.SelectedIndex = 0;
        txtOdometerReading.Text = "";
        txtBattery1.Text = "";
        txtBattery2.Text = "";
        ddlBatteryMakeBattery1.SelectedIndex = 0;
        ddlModelCapacityBattery1.SelectedIndex = 0;
        ddlBatteryMakeBattery2.SelectedIndex = 0;
        ddlModelCapacityBattery2.SelectedIndex = 0;
    }

    protected void txtValiSDate_TextChanged(object sender, EventArgs e)
    {
        ddlPolicyValidityPeriod.SelectedIndex = 0;
        txtValEDate.Text = "";
    }
}