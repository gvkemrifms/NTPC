using System;
using System.Configuration;
using System.Web.UI;

public partial class FuelVarienceReport : Page
{
    private readonly Helper _helper = new Helper();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Name"] == null) Response.Redirect("Login.aspx");
        if (!IsPostBack)
        {
            ddlvehicle.Enabled = false;
            ddlbunk.Enabled = false;
            BindDistrictdropdown();
        }
    }

    private void Bindbunkdropdown()
    {
        try
        {
            //[P_GetServiceStns]
            ddlbunk.Enabled = true;
            //  _helper.FillDropDownHelperMethodWithSp("P_GetServiceStns", "ServiceStnName", "District_Id", ddldistrict, ddlbunk,null,null,"@District");
            var newQuery = "select distinct ServiceStation_Name as ServiceStnName,District_Id  from M_FMS_ServiceStationNames where District_id = (select ds_dsid from M_FMS_Districts where ds_lname = '" + ddldistrict.SelectedItem.Text + "') order by ServiceStation_Name";
            _helper.FillDropDownHelperMethod(newQuery, "ServiceStnName", "District_Id", ddlbunk);
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
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
            ddlbunk.Enabled = false;
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

    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        Loaddata();
    }

    public void Loaddata()
    {
        try
        {
            _helper.FillDropDownHelperMethodWithSp("P_FMSReports_FuelVariance", null, null, ddldistrict, ddlvehicle, txtfrmDate, txttodate, "@districtID", "@VehicleID", "@From", "@To", "@Bunk", Grddetails, ddlbunk);
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
    }

    protected void ddlvehicle_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlvehicle.SelectedIndex <= 0)
            ddlbunk.Enabled = false;
        try
        {
            Bindbunkdropdown();
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }
}