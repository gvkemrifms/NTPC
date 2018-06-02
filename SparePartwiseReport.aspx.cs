using System;
using System.Configuration;
using System.Web.UI;

public partial class SparePartwiseReport : Page
{
    private readonly Helper _helper = new Helper();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Name"] == null) Response.Redirect("Login.aspx");
        if (!IsPostBack)
        {
            ddlvehicle.Enabled = false;
            ddlvendor.Enabled = false;
            BindDistrictdropdown();
        }
    }

    private void BindDistrictdropdown()
    {
        try
        {
            var sqlQuery = ConfigurationManager.AppSettings["Query"];
            _helper.FillDropDownHelperMethod(sqlQuery, "district_name", "district_id", ddldistrict);
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
    }

    protected void btntoExcel_Click(object sender, EventArgs e)
    {
        try
        {
            _helper.LoadExcelSpreadSheet(this, Panel2, "VehicleSummaryDistrictwise.xls");
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    protected void ddldistrict_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddldistrict.SelectedIndex > 0)
        {
            ddlvehicle.Enabled = true;
            try
            {
                _helper.FillDropDownHelperMethodWithSp("P_GetVehicleNumber", "VehicleNumber", "VehicleID", ddldistrict, ddlvehicle, null, null, "@districtID");
            }
            catch (Exception ex)
            {
                _helper.ErrorsEntry(ex);
            }
        }
        else
        {
            ddlvehicle.Enabled = false;
        }
    }

    protected void ddlvehicle_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddldistrict.SelectedIndex > 0)
        {
            ddlvendor.Enabled = true;
            try
            {
                _helper.FillDropDownHelperMethodWithSp("P_Get_Agency", "AgencyName", "AgencyID", ddldistrict, ddlvendor, null, null, "@DistrictID");
            }
            catch (Exception ex)
            {
                _helper.ErrorsEntry(ex);
            }
        }
        else
        {
            ddlvendor.Enabled = false;
        }
    }

    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        Loaddata();
    }

    public void Loaddata()
    {
        try
        {
            _helper.FillDropDownHelperMethodWithSp("P_Reports_SparePartsWise", null, null, ddldistrict, ddlvehicle, txtfrmDate, txttodate, "@DistrictID", "@VehicleID", "@From", "@To", "@SpareVenName", Grddetails, null, null, ddlvendor);
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }
}