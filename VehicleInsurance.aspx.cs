using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Web.UI;
using GvkFMSAPP.BLL;
using GvkFMSAPP.PL;

public partial class VehicleInsurance : Page
{
    private readonly FMSGeneral _fmsGeneral = new FMSGeneral();
    private readonly Helper _helper = new Helper();
    private readonly GvkFMSAPP.BLL.StatutoryCompliance.VehicleInsurance _vehicleInsurance = new GvkFMSAPP.BLL.StatutoryCompliance.VehicleInsurance();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Name"] == null) Response.Redirect("Login.aspx");
        if (!IsPostBack)
        {
            var dsPerms = (DataSet) Session["PermissionsDS"];
            if (dsPerms == null) throw new ArgumentNullException(nameof(dsPerms));
            dsPerms.Tables[0].DefaultView.RowFilter = "Url='" + Page.Request.Url.Segments[Page.Request.Url.Segments.Length - 1] + "'";
            var p = new PagePermissions(dsPerms, dsPerms.Tables[0].DefaultView[0]["Url"].ToString(), dsPerms.Tables[0].DefaultView[0]["Title"].ToString());
            btSave.Attributes.Add("onclick", "return validation()");
            GetVehicleNumber();
            if (Request.QueryString["vehicleid"] != null)
                try
                {
                    ddlVehicleNo.Items.FindByValue(Request.QueryString["vehicleid"]).Selected = true;
                    GetVehicleInsuranceData();
                    var pdt = _fmsGeneral.GetPurchaseDate(int.Parse(ddlVehicleNo.SelectedItem.Value));
                    vehiclePurchaseDate.Value = pdt.ToString(CultureInfo.CurrentCulture);
                }
                catch (Exception)
                {
                    Show("No vehicle");
                }

            GetAgency();
            GetInsuranceType();
            pnlVehicleInsurance.Visible = p.Add;
            if (p.View && p.Add == false && p.Modify == false && p.Approve == false) Response.Redirect("~/VehicleInsuranceViewHistory.aspx", false);
        }

        switch (chkBoxChangeInsuranceDetails.Checked)
        {
            case false:
                ddlInsuranceType.Visible = false;
                ddlInsuranceAgency.Visible = false;
                txtInsuranceAgency.Visible = true;
                txtInsuranceType.Visible = true;
                txtInsurancePolicyNo.BackColor = Color.DarkGray;
                break;
        }
    }

    public void GetVehicleNumber()
    {
        try
        {
            _vehicleInsurance.UserDistrictId = Convert.ToInt32(Session["UserdistrictId"].ToString());
            var ds = _vehicleInsurance.GetVehicleNumber();
            if (ds != null) _helper.FillDropDownHelperMethodWithDataSet(ds, "VehicleNumber", "VehicleID", ddlVehicleNo);
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    protected void btSave_Click(object sender, EventArgs e)
    {
        _vehicleInsurance.VehicleID = Convert.ToInt32(ddlVehicleNo.SelectedItem.Value);
        _vehicleInsurance.District = Convert.ToInt32(ViewState["District"].ToString());
        _vehicleInsurance.CurrentPolicyEndDate = Convert.ToDateTime(txtCurrentPolicyEndDate.Text);
        _vehicleInsurance.InsuranceFeesPaid = float.Parse(txtFeesPaid.Text);
        _vehicleInsurance.InsuranceFeesPaidDate = Convert.ToDateTime(txtFeesPaidDate.Text);
        _vehicleInsurance.InsurancePolicyNo = txtInsurancePolicyNo.Text;
        _vehicleInsurance.InsuranceReceiptNo = txtReceiptNumber.Text;
        if (chkBoxChangeInsuranceDetails.Checked)
        {
            _vehicleInsurance.InsuranceType = Convert.ToInt32(ddlInsuranceType.SelectedItem.Value);
            _vehicleInsurance.InsuranceAgency = Convert.ToInt32(ddlInsuranceAgency.SelectedItem.Value);
        }
        else
        {
            _vehicleInsurance.InsuranceType = Convert.ToInt32(ViewState["InsuranceType"].ToString());
            _vehicleInsurance.InsuranceAgency = Convert.ToInt32(ViewState["InsuranceAgency"].ToString());
        }

        _vehicleInsurance.PolicyEndDate = Convert.ToDateTime(txtPolicyEndDate.Text);
        _vehicleInsurance.PolicyStartDate = Convert.ToDateTime(txtPolicyStartDate.Text);
        _vehicleInsurance.PolicyValidityPeriod = ddlPolicyValidityPeriod.SelectedItem.Text;
        var ret = _vehicleInsurance.InsertVehicleInsurance();
        Show(ret == 1 ? "Record Inserted Successfully" : "Error");
        GetVehicleNumber();
        ClearControls();
    }

    protected void ddlPolicyValidityPeriod_SelectedIndexChanged(object sender, EventArgs e)
    {
        switch (txtPolicyStartDate.Text)
        {
            case "":
                Show("Enter Policy Start Date");
                ddlPolicyValidityPeriod.SelectedIndex = 0;
                txtPolicyEndDate.Text = "";
                break;
        }

        if (ddlPolicyValidityPeriod.SelectedIndex == 0) return;
        txtPolicyEndDate.Text = Convert.ToDateTime(txtPolicyStartDate.Text).AddMonths(Convert.ToInt16(ddlPolicyValidityPeriod.SelectedItem.Value)).ToString();
    }

    public void Show(string message)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('" + message + "');", true);
    }

    protected void ddlVehicleNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetVehicleInsuranceData();
        var pdt = _fmsGeneral.GetPurchaseDate(int.Parse(ddlVehicleNo.SelectedItem.Value));
        vehiclePurchaseDate.Value = pdt.ToString(CultureInfo.CurrentCulture);
    }

    public void GetInsuranceType()
    {
        try
        {
            var ds = _vehicleInsurance.GetInsuranceType();
            if (ds != null) _helper.FillDropDownHelperMethodWithDataSet(ds, "InsuranceTypeName", "InsuranceTypeId", ddlInsuranceType);
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
            var ds = _vehicleInsurance.GetAgency();
            if (ds != null) _helper.FillDropDownHelperMethodWithDataSet(ds, "InsuranceAgency", "InsuranceId", ddlInsuranceAgency);
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    public void GetVehicleInsuranceData()
    {
        try
        {
            switch (ddlVehicleNo.SelectedIndex)
            {
                case 0:
                    txtDistrict.Text = "";
                    txtInsuranceType.Text = "";
                    txtInsuranceAgency.Text = "";
                    txtInsurancePolicyNo.Text = "";
                    txtCurrentPolicyEndDate.Text = "";
                    break;
                default:
                    _vehicleInsurance.VehicleID = int.Parse(ddlVehicleNo.SelectedItem.Value);
                    var ds = _vehicleInsurance.GetVehicleInsuranceData();
                    txtDistrict.Text = ds.Tables[0].Rows[0]["ds_lname"].ToString();
                    txtInsuranceType.Text = ds.Tables[0].Rows[0]["InsuranceTypeName"].ToString();
                    txtInsuranceAgency.Text = ds.Tables[0].Rows[0]["AgencyName"].ToString();
                    txtInsurancePolicyNo.Text = ds.Tables[0].Rows[0]["InsurancePolicyNo"].ToString();
                    txtCurrentPolicyEndDate.Text = ds.Tables[0].Rows[0]["CurrentPolicyEndDate"].ToString();
                    ViewState["District"] = ds.Tables[0].Rows[0]["District"].ToString();
                    ViewState["InsuranceType"] = ds.Tables[0].Rows[0]["InsuranceType"].ToString();
                    ViewState["InsuranceAgency"] = ds.Tables[0].Rows[0]["InsuranceAgency"].ToString();
                    break;
            }
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    protected void chkBoxChangeInsuranceDetails_CheckedChanged(object sender, EventArgs e)
    {
        if (chkBoxChangeInsuranceDetails.Checked && chkBoxChangeInsuranceDetails.Checked)
        {
            ddlInsuranceAgency.Visible = true;
            ddlInsuranceType.Visible = true;
            txtInsuranceAgency.Visible = false;
            txtInsuranceType.Visible = false;
            txtInsurancePolicyNo.BackColor = Color.Empty;
            txtInsurancePolicyNo.ReadOnly = false;
        }
    }

    protected void btReset_Click(object sender, EventArgs e)
    {
        ClearControls();
    }

    private void ClearControls()
    {
        txtCurrentPolicyEndDate.Text = "";
        txtDistrict.Text = "";
        txtFeesPaid.Text = "";
        txtFeesPaidDate.Text = "";
        txtInsuranceAgency.Text = "";
        txtInsurancePolicyNo.Text = "";
        txtInsuranceType.Text = "";
        txtPolicyEndDate.Text = "";
        txtPolicyStartDate.Text = "";
        txtReceiptNumber.Text = "";
        ddlInsuranceAgency.ClearSelection();
        ddlInsuranceType.ClearSelection();
        ddlPolicyValidityPeriod.ClearSelection();
        ddlVehicleNo.ClearSelection();
    }

    protected void txtPolicyStartDate_TextChanged(object sender, EventArgs e)
    {
        ddlPolicyValidityPeriod.SelectedIndex = 0;
        txtPolicyEndDate.Text = "";
    }

    protected void ddlInsuranceType_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void ddlInsuranceAgency_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
}