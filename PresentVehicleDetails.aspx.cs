using System;
using System.Web.UI;
using GvkFMSAPP.BLL;

public partial class PresentVehicleDetails : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Name"] == null) Response.Redirect("Login.aspx");
        if (!IsPostBack) FillVehicleNumber();
    }

    protected void FillVehicleNumber()
    {
        var fmsGeneral = new FMSGeneral();
        if (Session["UserdistrictId"] != null) fmsGeneral.UserDistrictId = Convert.ToInt32(Session["UserdistrictId"].ToString());
        Accordion_VehicleNumber.DataSource = fmsGeneral.GetVehicleDetails().Tables[0].DefaultView;
        Accordion_VehicleNumber.DataBind();
    }
}