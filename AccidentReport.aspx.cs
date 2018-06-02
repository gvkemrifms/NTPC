using System;
using System.Configuration;
using System.Web.UI;

public partial class AccidentReport : Page
{
    private readonly Helper _helper = new Helper();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["User_Name"] == null) Response.Redirect("Login.aspx");
            ddlvehicle.Enabled = false;
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

    protected void ddldistrict_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddldistrict.SelectedIndex <= 0)
        {
            ddlvehicle.Enabled = false;
        }
        else
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
    }

    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        Loaddata();
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

    public void Loaddata()
    {
        try
        {
            _helper.FillDropDownHelperMethodWithSp("P_FMSReports_FuelReport", null, null, ddldistrict, ddlvehicle, txtfrmDate, txttodate, "@DistrictID", "@VehicleID", "@From", "@End", null, Grddetails);
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
    }
}