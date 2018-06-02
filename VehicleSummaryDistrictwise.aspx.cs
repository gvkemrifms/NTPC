using System;
using System.Web.UI;

public partial class VehicleSummaryDistrictwise : Page
{
    private readonly Helper _helper = new Helper();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Name"] == null) Response.Redirect("Login.aspx");
        if (!IsPostBack) BindDistrictdropdown();
    }

    private void BindDistrictdropdown()
    {
        try
        {
            var sqlQuery = "SELECT district_id ds_dsid,district_name ds_lname from [m_district]  where state_id = 30 order by district_name";
            _helper.FillDropDownHelperMethod(sqlQuery, "ds_lname", "ds_dsid", ddldistrict);
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        try
        {
            _helper.FillDropDownHelperMethodWithSp("VAS_Districtwise_Vehicles_Inactive", null, null, ddldistrict, null, null, null, "@dsid", null, null, null, null, GridInactive);
            _helper.FillDropDownHelperMethodWithSp("VAS_Districtwise_Vehicles_Active", null, null, ddldistrict, null, null, null, "@dsid", null, null, null, null, GridActive);
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
}