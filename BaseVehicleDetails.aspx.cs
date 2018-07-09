using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Web.UI;
using GvkFMSAPP.PL;

public partial class BaseVehicleDetails : Page
{
    private readonly GvkFMSAPP.BLL.BaseVehicleDetails _basevehdet = new GvkFMSAPP.BLL.BaseVehicleDetails();
    private readonly Helper _helper = new Helper();

    public string UserId{ get; private set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] == null)
            Response.Redirect("Login.aspx");
        else
            UserId = (string) Session["User_Id"];
        if (!IsPostBack)
        {
            var dsPerms = (DataSet) Session["PermissionsDS"];
            dsPerms.Tables[0].DefaultView.RowFilter = "Url='" + Page.Request.Url.Segments[Page.Request.Url.Segments.Length - 1] + "'";
            var p = new PagePermissions(dsPerms, dsPerms.Tables[0].DefaultView[0]["Url"].ToString(), dsPerms.Tables[0].DefaultView[0]["Title"].ToString());
            pnlBaseVehicleDetails.Visible = false;
            var items = new List<string> {"getEngineNumber", "getVehicleType", "getManufacturerName", "getTyreMake", "getTyreModelSize", "getBatteryMake", "getBatteryModelCapacity", "getDistrict", "getAgency", "getFuelType", "getVehicleModel", "GetInsuranceType"};
            foreach (var item in items) FillDifferentDropDowns(item);
            if (p.Add) pnlBaseVehicleDetails.Visible = true;
            ViewState["VehNo"] = "Not Present";
        }
    }

    private void FillDifferentDropDowns(string item)
    {
        if (item != null)
            try
            {
                switch (item)
                {
                    case "getEngineNumber":
                        var ds = _basevehdet.GetEngineNo();
                        if (ds != null) _helper.FillDropDownHelperMethodWithDataSet(ds, "EngineNumber", "VehicleID", ddlEngineNo);
                        break;
                    case "getVehicleType":
                        ds = _basevehdet.GetVehicleType();
                        if (ds != null) _helper.FillDropDownHelperMethodWithDataSet(ds, "Vehicle_Type", "VehicleType_Id", ddlVehicleType);
                        break;
                    case "getManufacturerName":
                        ds = _basevehdet.GetManufacturerName();
                        if (ds != null) _helper.FillDropDownHelperMethodWithDataSet(ds, "FleetManufacturer_Name", "FleetManufacturer_Id", ddlManufacturerName);
                        break;
                    case "getTyreMake":
                        ds = _basevehdet.GetTyreMake();
                        if (ds != null) _helper.FillDropDownHelperMethodWithDataSet(ds, "Make", "Tyre_Id", ddlTyreMake);
                        break;
                    case "getTyreModelSize":
                        ds = _basevehdet.GetTyreModelSize();
                        if (ds != null) _helper.FillDropDownHelperMethodWithDataSet(ds, "ModelSize", "Tyre_Id", ddlModelSize);
                        break;
                    case "getBatteryMake":
                        ds = _basevehdet.GetBatteryMake();
                        if (ds != null) _helper.FillDropDownHelperMethodWithDataSet(ds, "Make", "Battery_Id", ddlBatteryMake);
                        break;
                    case "getBatteryModelCapacity":
                        ds = _basevehdet.GetBatteryModelCapacity();
                        if (ds != null) _helper.FillDropDownHelperMethodWithDataSet(ds, "ModelCapacity", "Battery_Id", ddlModelCapacity);
                        break;
                    case "getDistrict":
                        var dsUser = new DataSet();
                        var sqlQuery = ConfigurationManager.AppSettings["Query"] + " " + "where u.UserId = '" + UserId + "'";
                        var dt = _helper.ExecuteSelectStmt(sqlQuery);
                        dsUser.Tables.Add(dt);
                        _helper.FillDropDownHelperMethodWithDataSet(dsUser, "district_name", "district_id", ddlDistrict, null, ddlTRDistrict);
                        break;
                    case "getAgency":
                        ds = _basevehdet.GetAgency();
                        if (ds != null) _helper.FillDropDownHelperMethodWithDataSet(ds, "InsuranceAgency", "InsuranceId", ddlAgency);
                        break;
                    case "getFuelType":
                        ds = _basevehdet.GetFuelType();
                        if (ds != null) _helper.FillDropDownHelperMethodWithDataSet(ds, "FuelTypeName", "FuelTypeId", ddlFuelType);
                        break;
                    case "getVehicleModel":
                        ds = _basevehdet.GetVehicleModel();
                        if (ds != null) _helper.FillDropDownHelperMethodWithDataSet(ds, "VehicleModelName", "VehicleModelId", ddlVehicleModel);
                        break;
                    case "GetInsuranceType":
                        ds = _basevehdet.GetInsuranceType();
                        if (ds != null) _helper.FillDropDownHelperMethodWithDataSet(ds, "InsuranceTypeName", "InsuranceTypeId", ddlInsType);
                        break;
                }
            }
            catch (Exception ex)
            {
                _helper.ErrorsEntry(ex);
            }
    }

    public void GetChassisNo()
    {
        switch (ddlEngineNo.SelectedIndex)
        {
            case 0:
                txtChassisNo.Text = "";
                txtTRNo.Text = "";
                break;
            default:
                txtTRNo.Text = "";
                txtTRNo.ReadOnly = false;
                _basevehdet.VehicleID = int.Parse(ddlEngineNo.SelectedItem.Value);
                var ds = _basevehdet.GetChassisNo();
                if (ds == null) throw new ArgumentNullException(nameof(ds));
                txtChassisNo.Text = ds.Tables[0].Rows[0]["ChasisNumber"].ToString();
                if (ds.Tables[0].Rows[0]["VehicleNumber"].ToString() != "")
                {
                    txtTRNo.Text = ds.Tables[0].Rows[0]["VehicleNumber"].ToString();
                    txtTRNo.ReadOnly = true;
                    ViewState["VehNo"] = "Present";
                }

                break;
        }
    }

    protected void ddlEngineNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        ClearData();
        GetChassisNo();
    }

    public void ClearData()
    {
        ViewState["ValidityPeriod"] = "";
        txtInvoiceNo.Text = "";
        txtInvoiceDate.Text = "";
        txtBasicPrice.Text = "";
        txtHandlingCharges.Text = "";
        txtExciseDuty.Text = "";
        txtEC.Text = "";
        txtVAT.Text = "";
        txtUAV.Text = "";
        txtSHEC.Text = "";
        txtVehCost.Text = "";
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
        txtValEDate.Text = "";
        txtInsuranceReceiptNo.Text = "";
        txtInsFee.Text = "";
        txtValEDate.Text = "";
        txtValiSDate.Text = "";
        ddlPolicyValidityPeriod.SelectedIndex = 0;
        txtValEDate.Text = "";
        txtInsuranceFeesPaidDate.Text = "";
        txtFL.Text = "";
        txtFR.Text = "";
        txtRL.Text = "";
        txtRR.Text = "";
        txtSpareWheel.Text = "";
        ddlTyreMake.SelectedIndex = 0;
        ddlModelSize.SelectedIndex = 0;
        txtOdometerReading.Text = "";
        txtTRNo.Text = "";
        txtTRNo.ReadOnly = false;
        txtTRDate.Text = "";
        ddlTRDistrict.SelectedIndex = 0;
        txtVeRTACircle.Text = "";
        txtRoadTaxFee.Text = "";
        txtSittingCapacity.Text = "";
        txtInspectionDate.Text = "";
        txtInspectedBy.Text = "";
        txtBattery1.Text = "";
        txtBattery2.Text = "";
        ddlBatteryMake.SelectedIndex = 0;
        ddlModelCapacity.SelectedIndex = 0;
    }

    protected void ddlPolicyValidityPeriod_SelectedIndexChanged(object sender, EventArgs e)
    {
        switch (ddlPolicyValidityPeriod.SelectedIndex)
        {
            case 0:
                txtValEDate.Text = "";
                txtVehicleCost.Text = txtVehCost.Text;
                break;
            default:
                switch (ddlPolicyValidityPeriod.SelectedIndex)
                {
                    case 0:
                        if (txtValiSDate.Text == "")
                        {
                            Show("Enter the Valid Start Date");
                            ddlPolicyValidityPeriod.SelectedIndex = 0;
                            txtValEDate.Text = "";
                            txtVehicleCost.Text = txtVehCost.Text;
                        }

                        break;
                    default:
                        var endTime = DateTime.ParseExact(txtValiSDate.Text,"MM/dd/yyyy",CultureInfo.InvariantCulture);

                        txtValEDate.Text = endTime.AddMonths(Convert.ToInt16(ddlPolicyValidityPeriod.SelectedItem.Value)).Subtract(new TimeSpan(1, 0, 0)).ToShortDateString();
                        txtVehicleCost.Text = txtVehCost.Text;
                        ViewState["ValidityPeriod"] = Convert.ToString(ddlPolicyValidityPeriod.SelectedItem.Value);
                        break;
                }

                break;
        }
    }

    public void Show(string message)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('" + message + "');", true);
    }

    protected void txtValiSDate_TextChanged(object sender, EventArgs e)
    {
        ddlPolicyValidityPeriod.SelectedIndex = 0;
        txtValEDate.Text = "";
        txtVehicleCost.Text = txtVehCost.Text;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (_basevehdet != null)
            try
            {
                _basevehdet.InvoiceNo = txtInvoiceNo.Text;
                _basevehdet.VehicleID = int.Parse(ddlEngineNo.SelectedItem.Value);
                _basevehdet.InvoiceDate = DateTime.ParseExact(txtInvoiceDate.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                _basevehdet.BasicPrice = float.Parse(txtBasicPrice.Text);
                _basevehdet.HandlingCharges = float.Parse(txtHandlingCharges.Text);
                _basevehdet.ExciseDuty = float.Parse(txtExciseDuty.Text);
                _basevehdet.EC = float.Parse(txtEC.Text);
                _basevehdet.VAT = float.Parse(txtVAT.Text);
                _basevehdet.CessOnUAV = float.Parse(txtUAV.Text);
                _basevehdet.SHEC = float.Parse(txtSHEC.Text);
                _basevehdet.TotalValue = float.Parse(txtVehCost.Text);
                _basevehdet.VehicleModel = ddlVehicleModel.SelectedItem.Value;
                _basevehdet.KMPL = float.Parse(txtKmpl.Text);
                _basevehdet.VehicleType = ddlVehicleType.SelectedItem.Value;
                _basevehdet.VehicleEmissionType = txtVehicleEmissionType.Text;
                _basevehdet.PurchaseDate = DateTime.ParseExact(txtPurchaseDate.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                _basevehdet.OwnerName = txtOwnerName.Text;
                _basevehdet.ManufacturerName = ddlManufacturerName.SelectedItem.Value;
                _basevehdet.VehicleCost = float.Parse(txtVehCost.Text);
                _basevehdet.ManufacturingDate = DateTime.ParseExact(txtManufacturingDate.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                _basevehdet.EngineCapacity = txtEngineCapacity.Text;
                _basevehdet.FuelType = ddlFuelType.SelectedItem.Value;
                _basevehdet.District = Convert.ToInt32(ddlDistrict.SelectedItem.Value);
                _basevehdet.InsuranceType = Convert.ToInt32(ddlInsType.SelectedItem.Value);
                _basevehdet.InsuranceAgency = Convert.ToInt32(ddlAgency.SelectedItem.Value);
                _basevehdet.InsurancePolicyNo = txtInPolicyNo.Text;
                _basevehdet.CurrentPolicyEndDate = DateTime.Now;
                _basevehdet.InsuranceReceiptNo = txtInsuranceReceiptNo.Text;
                _basevehdet.InsuranceFeesPaid = float.Parse(txtInsFee.Text);
                _basevehdet.InsuranceFeesPaidDate = DateTime.ParseExact(txtInsuranceFeesPaidDate.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                _basevehdet.PolicyStartDate = DateTime.ParseExact(txtValiSDate.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                _basevehdet.PolicyValidityPeriod = ddlPolicyValidityPeriod.SelectedItem.Value;
                _basevehdet.PolicyEndDate = DateTime.Parse(txtValEDate.Text);
                _basevehdet.FL = txtFL.Text;
                _basevehdet.FR = txtFR.Text;
                _basevehdet.RL = txtRL.Text;
                _basevehdet.RR = txtRR.Text;
                _basevehdet.Stephny = txtSpareWheel.Text;
                _basevehdet.TyreMake = ddlTyreMake.SelectedItem.Value;
                _basevehdet.TyreModelSize = int.Parse(ddlModelSize.SelectedItem.Value);
                _basevehdet.OdometerReading = long.Parse(txtOdometerReading.Text);
                _basevehdet.TRNo = txtTRNo.Text;
                _basevehdet.TRDate = DateTime.ParseExact(txtTRDate.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                _basevehdet.TRDistrict = ddlTRDistrict.SelectedItem.Text;
                _basevehdet.VehicleRTACircle = txtVeRTACircle.Text;
                _basevehdet.RoadTaxFee = float.Parse(txtRoadTaxFee.Text);
                _basevehdet.SeatingCapacity = int.Parse(txtSittingCapacity.Text);
                _basevehdet.InspectionDate = DateTime.ParseExact(txtInspectionDate.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                _basevehdet.Inspected = null;
                _basevehdet.InspectedBy = txtInspectedBy.Text;
                _basevehdet.Battery1 = txtBattery1.Text;
                _basevehdet.Battery2 = txtBattery2.Text;
                _basevehdet.BatteryMake = ddlBatteryMake.SelectedItem.Value;
                _basevehdet.BatteryModelCapacity = int.Parse(ddlModelCapacity.SelectedItem.Value);
                var vehOut = ViewState["VehNo"].ToString() == "Present" ? 0 : _basevehdet.ValidateVehicleNumber();
                var tyreOut = _basevehdet.ValidateTyre();
                var batteryOut = _basevehdet.ValidateBattery();
                if (vehOut == 0 && tyreOut == 0 && batteryOut == 0)
                {
                    var ret = _basevehdet.InsBaseVehicleDetails();
                    Show(ret > 0 ? "Record Inserted Successfully" : "Error");
                    ClearData();
                    txtChassisNo.Text = "";
                    ddlEngineNo.ClearSelection();
                    FillDifferentDropDowns("getEngineNumber");
                }
                else
                {
                    if (vehOut > 0)
                    {
                        Show("T/R Number is already present");
                        var validityPeriod = ViewState["ValidityPeriod"].ToString();
                        txtValEDate.Text = DateTime.ParseExact(txtValiSDate.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture).AddMonths(Convert.ToInt16(validityPeriod)).Subtract(new TimeSpan(1, 0, 0)).ToShortDateString();
                        txtVehicleCost.Text = txtVehCost.Text;
                    }

                    if (tyreOut > 0)
                        switch (tyreOut)
                        {
                            case 1:
                            {
                                Show("FL Tyre is already present");
                                var validityPeriod = ViewState["ValidityPeriod"].ToString();
                                txtValEDate.Text = DateTime.ParseExact(txtValiSDate.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture).AddMonths(Convert.ToInt16(validityPeriod)).Subtract(new TimeSpan(1, 0, 0)).ToShortDateString();
                                txtVehicleCost.Text = txtVehCost.Text;
                                break;
                            }
                            case 2:
                            {
                                Show("FR Tyre is already present");
                                var validityPeriod = ViewState["ValidityPeriod"].ToString();
                                txtValEDate.Text = DateTime.ParseExact(txtValiSDate.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture).AddMonths(Convert.ToInt16(validityPeriod)).Subtract(new TimeSpan(1, 0, 0)).ToShortDateString();
                                txtVehicleCost.Text = txtVehCost.Text;
                                break;
                            }
                            case 3:
                            {
                                Show("RL Tyre is already present");
                                var validityPeriod = ViewState["ValidityPeriod"].ToString();
                                txtValEDate.Text = DateTime.ParseExact(txtValiSDate.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture).AddMonths(Convert.ToInt16(validityPeriod)).Subtract(new TimeSpan(1, 0, 0)).ToShortDateString();
                                txtVehicleCost.Text = txtVehCost.Text;
                                break;
                            }
                            case 4:
                            {
                                Show("RR Tyre is already present");
                                var validityPeriod = ViewState["ValidityPeriod"].ToString();
                                txtValEDate.Text = DateTime.ParseExact(txtValiSDate.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture).AddMonths(Convert.ToInt16(validityPeriod)).Subtract(new TimeSpan(1, 0, 0)).ToShortDateString();
                                txtVehicleCost.Text = txtVehCost.Text;
                                break;
                            }
                            default:
                            {
                                Show("Stephny Tyre is already present");
                                var validityPeriod = ViewState["ValidityPeriod"].ToString();
                                txtValEDate.Text = DateTime.ParseExact(txtValiSDate.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture).AddMonths(Convert.ToInt16(validityPeriod)).Subtract(new TimeSpan(1, 0, 0)).ToShortDateString();
                                txtVehicleCost.Text = txtVehCost.Text;
                                break;
                            }
                        }
                    if (batteryOut > 0)
                        switch (batteryOut)
                        {
                            case 1:
                            {
                                Show("Battery1 is already present");
                                var validityPeriod = ViewState["ValidityPeriod"].ToString();
                                txtValEDate.Text = DateTime.ParseExact(txtValiSDate.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture).AddMonths(Convert.ToInt16(validityPeriod)).Subtract(new TimeSpan(1, 0, 0)).ToShortDateString();
                                txtVehicleCost.Text = txtVehCost.Text;
                                break;
                            }
                            default:
                            {
                                Show("Battery2 is already present");
                                var validityPeriod = ViewState["ValidityPeriod"].ToString();
                                txtValEDate.Text = DateTime.ParseExact(txtValiSDate.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture).AddMonths(Convert.ToInt16(validityPeriod)).Subtract(new TimeSpan(1, 0, 0)).ToShortDateString();
                                txtVehicleCost.Text = txtVehCost.Text;
                                break;
                            }
                        }
                }
            }
            catch (Exception ex)
            {
                _helper.ErrorsEntry(ex);
            }
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        ClearData();
        txtChassisNo.Text = "";
        ddlEngineNo.ClearSelection();
    }
}