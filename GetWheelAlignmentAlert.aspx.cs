using System;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using GvkFMSAPP.BLL;
using GvkFMSAPP.BLL.Alert;
using GvkFMSAPP.BLL.VehicleMaintenance;

public partial class GetWheelAlignmentAlert : Page
{
    private readonly Alert _fmsAlert = new Alert();
    private readonly Helper _helper = new Helper();
    private readonly VehicleMaintenance _vehMain = new VehicleMaintenance();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Name"] == null) Response.Redirect("Login.aspx");
        if (!IsPostBack)
        {
            lblheader.Text = "Wheel Alignment Alert";
            FillGrid();
        }
    }

    protected void btnSendMail_Click(object sender, EventArgs e)
    {
        try
        {
            var subject = "";
            var mailBody = CreateHtml((DataSet) ViewState["ds"]);
            MailHelper.SendMailMessage(ConfigurationManager.AppSettings["MasterMailid"], ConfigurationManager.AppSettings["AdminMailid"], "", "", subject, mailBody);
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    public string CreateHtml(DataSet ds)
    {
        var htmlText = "";
        if (ds.Tables.Count > 0)
        {
            htmlText = "<table border='1' cellpadding='2'  WIDTH='75%' ";
            htmlText += "<tr><th>District</th><th>Vehicle Number</th>";
            htmlText += "</tr>";
            if (ds.Tables.Count > 0)
            {
                for (var i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    htmlText += "<tr>";
                    htmlText += "<td> " + ds.Tables[0].Rows[i]["DistrictName"] + " </td>";
                    htmlText += "<td>" + ds.Tables[0].Rows[i]["VehicleNumber"] + "</td>";
                    htmlText += "</tr>";
                }

                htmlText += "</table>";
            }
        }

        return htmlText;
    }

    protected void grdWheelAlignment_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdWheelAlignment.PageIndex = e.NewPageIndex;
        FillGrid();
    }

    protected void FillGrid()
    {
        _fmsAlert.UserDistId = Convert.ToInt32(Session["UserdistrictId"].ToString());
        var ds = _fmsAlert.GetWheelAlignment();
        if (ds == null) throw new ArgumentNullException(nameof(ds));
        ViewState["ds"] = ds;
        grdWheelAlignment.DataSource = ds;
        grdWheelAlignment.DataBind();
    }

    protected void lnk_VehicleNumeber_Click(object sender, CommandEventArgs e)
    {
        var vehicleId = e.CommandArgument.ToString();
        var ds = _vehMain.GetVehiclesPendingForEntry(Convert.ToInt32(Session["UserdistrictId"].ToString()));
        if (ds == null) throw new ArgumentNullException(nameof(ds));
        if (Convert.ToInt32(ds.Tables[0].Select("Vehicle_ID=" + vehicleId).Length) <= 0)
            Response.Redirect("~/VehicleScheduleServiceRequest.aspx?VehID=" + vehicleId, true);
        else
            Show("This vehicle is already under maintenance..");
    }

    public void Show(string message)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('" + message + "');", true);
    }
}