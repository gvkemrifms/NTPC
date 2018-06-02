using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using GvkFMSAPP.BLL.StatutoryCompliance;

public partial class VehicleInsuranceViewHistory : Page
{
    private readonly VehicleInsurance _vehins = new VehicleInsurance();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Name"] == null) Response.Redirect("Login.aspx");
        GetVehicleInsurance();
    }

    public void GetVehicleInsurance()
    {
        _vehins.UserDistId = Convert.ToInt32(Session["UserdistrictId"].ToString());
        gvViewHistory.DataSource = _vehins.GetVehicleInsurance();
        gvViewHistory.DataBind();
    }

    protected void gvViewHistory_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvViewHistory.PageIndex = e.NewPageIndex;
        GetVehicleInsurance();
    }
}