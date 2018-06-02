using System;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using GvkFMSAPP.BLL;
using GvkFMSAPP.BLL.Alert;

public partial class MaintenanceAlert : Page
{
    private readonly Alert _fmsAlert = new Alert();
    private readonly Helper _helper = new Helper();
    private readonly VehicleRegistration _vehreg = new VehicleRegistration();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Name"] == null) Response.Redirect("Login.aspx");
        if (!IsPostBack) GetVehicles();
    }

    protected void FillGrid()
    {
        _fmsAlert.UserDistId = Convert.ToInt32(Session["UserdistrictId"].ToString());
        _fmsAlert.VehNum = Convert.ToString(ddlVehicle.SelectedItem.Text);
        var ds = _fmsAlert.GetMaintenanceAlert();
        if (ds == null) throw new ArgumentNullException(nameof(ds));
        ViewState["ds"] = ds;
        grdMaintAlert.DataSource = ds;
        grdMaintAlert.DataBind();
    }

    public void GetVehicles()
    {
        try
        {
            var ds = _vehreg.GetvehiclesReport(); //FMS.BLL.VehicleRegistration.GetDistrcts();
            if (ds != null) _helper.FillDropDownHelperMethodWithDataSet(ds, "VehicleNumber", "VehicleID", null, ddlVehicle);
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    public string CreateHtml(DataSet ds)
    {
        if (ds == null) throw new ArgumentNullException(nameof(ds));
        var htmlText = "";
        if (ds.Tables.Count > 0)
        {
            htmlText = "<table border='1' cellpadding='2'  WIDTH='75%' ";
            htmlText += "<tr><th>Vehicle Number</th><th>Latest Odometer</th><th>Last Maintenance Odo</th>";
            htmlText += "<th>Last Maintenance Date</th><th>Service Alert</th>";
            htmlText += "</tr>";
            if (ds.Tables.Count > 0)
            {
                for (var i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    htmlText += "<tr>";
                    htmlText += "<td> " + ds.Tables[0].Rows[i]["VehicleNumber"] + " </td>";
                    htmlText += "<td>" + ds.Tables[0].Rows[i]["Latest_odo"] + "</td>";
                    htmlText += "<td>" + ds.Tables[0].Rows[i]["LastMaintenanceOdo"] + "</td>";
                    htmlText += "<td>" + ds.Tables[0].Rows[i]["LastMaintenanceDate"] + "</td>";
                    htmlText += "<td> " + ds.Tables[0].Rows[i]["servicealert"] + " </td>";
                    htmlText += "</tr>";
                }

                htmlText += "</table>";
            }
        }

        return htmlText;
    }

    protected void grdMaintAlert_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdMaintAlert.PageIndex = e.NewPageIndex;
        FillGrid();
    }

    protected void btnSendMail_Click1(object sender, EventArgs e)
    {
        try
        {
            var subject = "Vehicle Maintenance Alert";
            var mailBody = CreateHtml((DataSet) ViewState["ds"]);
            MailHelper.SendMailMessage(ConfigurationManager.AppSettings["MasterMailid"], ConfigurationManager.AppSettings["AdminMailid"], "", "", subject, mailBody);
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    protected void ddlVehicle_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlVehicle.SelectedItem.Text != string.Empty) FillGrid();
    }
}