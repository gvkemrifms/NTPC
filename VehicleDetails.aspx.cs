using System;
using System.Data;
using System.Web.UI;
using GvkFMSAPP.PL;

public partial class VehicleDetails : Page
{
    private readonly GvkFMSAPP.BLL.Prior_MaintenanceStage.VehicleDetails _vehdet = new GvkFMSAPP.BLL.Prior_MaintenanceStage.VehicleDetails();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Name"] == null) Response.Redirect("Error.aspx");
        if (!IsPostBack)
        {
            var dsPerms = (DataSet) Session["PermissionsDS"];
            if (dsPerms == null) throw new ArgumentNullException(nameof(dsPerms));
            dsPerms.Tables[0].DefaultView.RowFilter = "Url='" + Page.Request.Url.Segments[Page.Request.Url.Segments.Length - 1] + "'";
            var p = new PagePermissions(dsPerms, dsPerms.Tables[0].DefaultView[0]["Url"].ToString(), dsPerms.Tables[0].DefaultView[0]["Title"].ToString());
            btnSave.Attributes.Add("onclick", "return validation()");
            pnlNewVehicleDetails.Visible = p.Add;
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        _vehdet.ChassisNumber = txtChassisNumber.Text;
        _vehdet.EngineNumber = txtEngineNumber.Text;
        _vehdet.VehicleNumber = txtVehicleNumber.Text.Trim();
        var output = _vehdet.ValidateVehicleDetails();
        int ret;
        switch (output)
        {
            case 1:
                Show("Engine Number is already present");
                break;
            case 2:
                Show("Chassis Number is already present");
                break;
            case 3:
                if (txtVehicleNumber.Text != "")
                {
                    Show("Vehicle Number is already present");
                }
                else
                {
                    ret = _vehdet.InsVehicleDetails();
                    Show(ret > 0 ? "Record Inserted Successfully" : "Error");
                    ClearControls();
                }

                break;
            default:
                ret = _vehdet.InsVehicleDetails();
                Show(ret > 0 ? "Record Inserted Successfully" : "Error");
                ClearControls();
                break;
        }
    }

    public void Show(string message)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('" + message + "');", true);
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearControls();
    }

    public void ClearControls()
    {
        txtChassisNumber.Text = "";
        txtEngineNumber.Text = "";
        txtVehicleNumber.Text = "";
    }
}