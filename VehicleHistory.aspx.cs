using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using GvkFMSAPP.BLL;
using GvkFMSAPP.BLL.Admin;
using GvkFMSAPP.BLL.StatutoryCompliance;

public partial class VehicleHistory : Page
{
    private readonly DistrictVehicleMapping _distvehmapp = new DistrictVehicleMapping();
    private readonly FMSGeneral _fmsGeneral = new FMSGeneral();
    private readonly Helper _helper = new Helper();
    private readonly RoadTax _roadtax = new RoadTax();
    private readonly FitnessRenewal _vehicleFitnessRenewal = new FitnessRenewal();
    private readonly VehicleInsurance _vehicleInsurance = new VehicleInsurance();
    private readonly PollutionUnderControl _vehiclePuc = new PollutionUnderControl();
    public IFuelManagement ObjFuelEntry = new FuelManagement();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Name"] == null) Response.Redirect("Login.aspx");
        if (!IsPostBack) FillVehicleNumber();
    }

    protected void FillVehicleNumber()
    {
        try
        {
            if (Session["UserdistrictId"] != null) _fmsGeneral.UserDistrictId = Convert.ToInt32(Session["UserdistrictId"].ToString());
            var ds = _fmsGeneral.GetVehicleNumber();
            if (ds == null) throw new ArgumentNullException(nameof(ds));
            _helper.FillDropDownHelperMethodWithDataSet(ds, "VehicleNumber", "VehicleID", ddlVehicleList);
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    protected void ddlVehicleList_SelectedIndexChanged(object sender, EventArgs e)
    {
        switch (ddlVehicleList.SelectedIndex)
        {
            case 0:
                Panel_Detail.Visible = false;
                panel_vehicleDetail.Visible = false;
                break;
            default:
                panel_vehicleDetail.Visible = true;
                Panel_Detail.Visible = true;
                FillVehicleDetail();
                FillRoadTax();
                FillVehicleInsurance();
                FillPuc();
                FillVehicleFitnessRenewall();
                FillPetrolCard();
                FillFuelEntryDetails();
                break;
        }
    }

    public void FillRoadTax()
    {
        _roadtax.UserDistId = Convert.ToInt32(Session["UserdistrictId"].ToString());
        _roadtax.VehicleID = int.Parse(ddlVehicleList.SelectedItem.Value);
        var ds = _roadtax.GetRoadTaxByVehicleID();
        if (ds == null) throw new ArgumentNullException(nameof(ds));
        gvRoadTax.DataSource = ds;
        gvRoadTax.DataBind();
    }

    public void FillVehicleInsurance()
    {
        _vehicleInsurance.VehicleID = int.Parse(ddlVehicleList.SelectedItem.Value);
        _vehicleInsurance.UserDistId = Convert.ToInt32(Session["UserdistrictId"].ToString());
        var ds = _vehicleInsurance.GetVehicleInsurancebyVehicleID();
        if (ds == null) throw new ArgumentNullException(nameof(ds));
        gvVehicleInsurance.DataSource = ds;
        gvVehicleInsurance.DataBind();
    }

    protected void FillPuc()
    {
        _vehiclePuc.VehicleID = int.Parse(ddlVehicleList.SelectedItem.Value);
        _vehiclePuc.UserDistId = Convert.ToInt32(Session["UserdistrictId"].ToString());
        var ds = _vehiclePuc.GetPollutionUnderControlbyVehicle();
        if (ds == null) throw new ArgumentNullException(nameof(ds));
        gvPollutionUnderControl.DataSource = ds;
        gvPollutionUnderControl.DataBind();
    }

    protected void FillVehicleFitnessRenewall()
    {
        _vehicleFitnessRenewal.VehicleID = int.Parse(ddlVehicleList.SelectedValue);
        _vehicleFitnessRenewal.UserDistrictId = Convert.ToInt32(Session["UserdistrictId"].ToString());
        var ds = _vehicleFitnessRenewal.GetFitnessRenewalbyVehicleID();
        if (ds == null) throw new ArgumentNullException(nameof(ds));
        gvFitnessRenewal.DataSource = ds;
        gvFitnessRenewal.DataBind();
    }

    protected void FillVehicleDetail()
    {
        _distvehmapp.VehicleId = int.Parse(ddlVehicleList.SelectedItem.Value);
        var ds = _distvehmapp.GetSelectedDistrictByVehicleList();
        if (ds == null) throw new ArgumentNullException(nameof(ds));
        foreach (DataRow dr in ds.Tables[0].Rows) lblDistrict.Text = dr["ds_lname"].ToString();
    }

    protected void FillFuelEntryDetails()
    {
        var ds = ObjFuelEntry.IGetFuelEntryDetails(int.Parse(ddlVehicleList.SelectedItem.Value));
        grdFuelEntry.DataSource = ds;
        grdFuelEntry.DataBind();
    }

    protected void FillPetrolCard()
    {
        var ds = ObjFuelEntry.IFillCardNumber(int.Parse(ddlVehicleList.SelectedItem.Value));
        if (ds == null) throw new ArgumentNullException(nameof(ds));
        grdPetroCard.DataSource = ds;
        grdPetroCard.DataBind();
    }

    protected void gvFitnessRenewal_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvFitnessRenewal.PageIndex = e.NewPageIndex;
        FillVehicleFitnessRenewall();
    }

    protected void gvRoadTax_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvRoadTax.PageIndex = e.NewPageIndex;
        FillRoadTax();
    }

    protected void gvVehicleInsurance_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvVehicleInsurance.PageIndex = e.NewPageIndex;
        FillVehicleInsurance();
    }

    protected void gvPollutionUnderControl_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvPollutionUnderControl.PageIndex = e.NewPageIndex;
        FillPuc();
    }

    protected void gvFitnessRenewal_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:
                var lblFrValidityPeriod = (Label) e.Row.FindControl("lblFRValidityPeriod");
                var lblFrValidityPeriodText = (Label) e.Row.FindControl("lblFRValidityPeriodText");
                switch (lblFrValidityPeriod.Text)
                {
                    case "3":
                        lblFrValidityPeriodText.Text = "3 Months";
                        break;
                    case "6":
                        lblFrValidityPeriodText.Text = "6 Months";
                        break;
                    case "9":
                        lblFrValidityPeriodText.Text = "9 Months";
                        break;
                    default:
                        lblFrValidityPeriodText.Text = "1 Yrs";
                        break;
                }

                break;
        }
    }

    protected void grdPetroCard_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdPetroCard.PageIndex = e.NewPageIndex;
        FillPetrolCard();
    }
}