using System;
using System.Configuration;
using System.Web.UI;

public partial class VehicleSummaryDistrictwise : Page
{
    private readonly Helper _helper = new Helper();

    public string UserId { get; private set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] == null)
            Response.Redirect("Login.aspx");
        else
            UserId = (string)Session["User_Id"];
        if (!IsPostBack) BindDistrictdropdown();
    }

    private void BindDistrictdropdown()
    {
        try
        {
            var sqlQuery = ConfigurationManager.AppSettings["Query"] + " " + "where u.UserId ='" + UserId + "'";
            _helper.FillDropDownHelperMethod(sqlQuery, "district_name", "district_id", ddldistrict);
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