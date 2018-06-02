using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using GvkFMSAPP.BLL;
using GvkFMSAPP.PL;

public partial class VehicleTypes : Page
{
    private readonly FleetMaster _fleetMaster=new FleetMaster();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Name"] == null) Response.Redirect("Error.aspx");
        grvVehicleTypes.Columns[0].Visible = false;
        FillGrid_VehicleTypes();
        txtVehicleType.Attributes.Add("onkeypress", "javascript:return alpha_only_withspace(this,event)");
        txtVehicleDescription.Attributes.Add("onkeypress", "javascript:return remark(this,event)");
        //Permissions
        var dsPerms = (DataSet) Session["PermissionsDS"];
        if (dsPerms == null) throw new ArgumentNullException(nameof(dsPerms));
        dsPerms.Tables[0].DefaultView.RowFilter = "Url='" + Page.Request.Url.Segments[Page.Request.Url.Segments.Length - 1] + "'";
        var p = new PagePermissions(dsPerms, dsPerms.Tables[0].DefaultView[0]["Url"].ToString(), dsPerms.Tables[0].DefaultView[0]["Title"].ToString());
        pnlvehicletypes.Visible = false;
        grvVehicleTypes.Visible = p.View;
        switch (p.Add)
        {
            case true:
                pnlvehicletypes.Visible = true;
                grvVehicleTypes.Visible = true;
                break;
        }

        switch (p.Modify)
        {
            case true:
                grvVehicleTypes.Visible = true;
                break;
        }
    }

    private void VehicleTypesReset()
    {
        txtVehicleType.Text = "";
        txtVehicleDescription.Text = "";
        btnvehicleTypeSave.Text = "Save";
    }

    protected void vehicleTypeReset_Click(object sender, EventArgs e)
    {
        VehicleTypesReset();
    }

    protected void vehicleTypeSave_Click(object sender, EventArgs e)
    {
        switch (btnvehicleTypeSave.Text)
        {
            case "Save":
            {
                var ds = _fleetMaster.FillGrid_VehicleTypes();
                if (ds == null) throw new ArgumentNullException(nameof(ds));
                if (ds.Tables[0].Select("Vehicle_Type='" + txtVehicleType.Text + "'").Length > 0)
                {
                    Show("Vehicle Type already exists");
                }
                else
                {
                    var vehicleType = txtVehicleType.Text;
                    var vehicleTypeDescription = txtVehicleDescription.Text;
                    ds = _fleetMaster.InsertVehicleTypes(vehicleType, vehicleTypeDescription);
                    switch (ds.Tables.Count)
                    {
                        case 0:
                            Show("Vehicle Type Details added successfully");
                            VehicleTypesReset();
                            break;
                        default:
                            Show("This Vehicle Type details already exists ");
                            break;
                    }
                }

                break;
            }
            default:
            {
                var ds = _fleetMaster.FillGrid_VehicleTypes();
                if (ds == null) throw new ArgumentNullException(nameof(ds));
                if (ds.Tables[0].Select("Vehicle_Type='" + txtVehicleType.Text + "' And VehicleType_Id<>'" + hidVehicleType.Value + "'").Length > 0)
                {
                    Show("Vehicle Type already exists");
                }
                else
                {
                    int vehicleId = Convert.ToInt16(hidVehicleType.Value);
                    var vehicleType = txtVehicleType.Text;
                    var vehicleDescription = txtVehicleDescription.Text;
                    ds = _fleetMaster.UpdateVehicleTypes(vehicleId, vehicleType, vehicleDescription);
                    switch (ds.Tables.Count)
                    {
                        case 0:
                            Show("Vehicle Type Details updated successfully");
                            VehicleTypesReset();
                            break;
                        default:
                            Show("This Vehicle Type details already exists ");
                            break;
                    }
                }

                break;
            }
        }

        FillGrid_VehicleTypes();
    }

    public void FillGrid_VehicleTypes()
    {
        var ds = _fleetMaster.FillGrid_VehicleTypes();
        if (ds == null)
        {
            Show("No record found");
        }
        else
        {
            if (ds.Tables[0].Rows.Count <= 0) return;
            grvVehicleTypes.DataSource = ds.Tables[0];
            grvVehicleTypes.DataBind();
        }
    }

    protected void grvVehicleTypes_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grvVehicleTypes.PageIndex = e.NewPageIndex;
        FillGrid_VehicleTypes();
    }

    protected void grvVehicleTypes_RowEditing(object sender, GridViewEditEventArgs e)
    {
        btnvehicleTypeSave.Text = "Update";
        var index = e.NewEditIndex;
        var lblid = (Label) grvVehicleTypes.Rows[index].FindControl("lblVehicleTypeId");
        hidVehicleType.Value = lblid.Text;
        int vehicleId = Convert.ToInt16(hidVehicleType.Value);
        var ds = _fleetMaster.RowEditVehicleTypes(vehicleId);
        txtVehicleType.Text = Convert.ToString(ds.Tables[0].Rows[0]["Vehicle_Type"].ToString());
        txtVehicleDescription.Text = Convert.ToString(ds.Tables[0].Rows[0]["Vehicle_TypeDesc"].ToString());
    }

    public void Show(string message)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('" + message + "');", true);
    }
}