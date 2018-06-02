using System;
using System.Web.UI;

public partial class EnquiryScreenReportnew : Page
{
    private readonly Helper _helper = new Helper();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Name"] == null) Response.Redirect("Login.aspx");
        if (!IsPostBack)

            BindVehicledropdown();
    }

    private void BindVehicledropdown()
    {
        try
        {
            var sqlQuery = "select vehicleid,vehicleNumber from M_FMS_Vehicles";
            _helper.FillDropDownHelperMethod(sqlQuery, "VehicleNumber", "VehicleID", ddlvehicle);
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    protected void btClick_ShowReport(object sender, EventArgs e)
    {
        Loaddata();
    }

    private void Loaddata()
    {
        try
        {
            _helper.FillDropDownHelperMethodWithSp("p_report_enquiryscreenreport", null, null, ddlvehicle, null, null, null, "@VehicleID", null, null, null, null, Grddetails);
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }
}