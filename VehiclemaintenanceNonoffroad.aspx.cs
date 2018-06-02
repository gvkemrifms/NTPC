using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using GvkFMSAPP.BLL.VAS_BLL;
using GvkFMSAPP.BLL.VehicleMaintenance;

public partial class VehiclemaintenanceNonoffroad : Page
{
    private readonly Helper _helper = new Helper();
    private readonly VASGeneral _vehicleobj = new VASGeneral();
    private readonly VehicleMaintenance _vehMain = new VehicleMaintenance();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Name"] == null) Response.Redirect("Login.aspx");
        if (!IsPostBack)
        {
            GetVehicles();
            SetInitialMaintenanceDetails();
            BindVendorDetails();
        }
    }

    private void BindVendorDetails()
    {
        try
        {
            var ds = _vehMain.IFillVendorsMaintenance();
            if (ds == null) return;
            _helper.FillDropDownHelperMethodWithDataSet(ds, "AgencyName", "AgencyId", ddlVendorName);
            _helper.FillDifferentDataTables(ddlMaintenanceType, ds.Tables[1], "MaintenanceType", "MaintId");
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    public void GetVehicles()
    {
        try
        {
            var ds = _vehicleobj.getVehforNonOffroad();
            if (ds == null) throw new ArgumentNullException(nameof(ds));
            _helper.FillDropDownHelperMethodWithDataSet(ds, "vi_VehicleNumber", "", ddlVehicles);
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    public void SetInitialMaintenanceDetails()
    {
        var dt = new DataTable();

        //Define the Columns
        dt.Columns.Add(new DataColumn("RowNumber", typeof(string)));
        dt.Columns.Add(new DataColumn("ColVehNo", typeof(string)));
        dt.Columns.Add(new DataColumn("ColMainType", typeof(string)));
        dt.Columns.Add(new DataColumn("ColMainBillDate", typeof(string)));
        dt.Columns.Add(new DataColumn("Vendor_Name", typeof(string)));
        dt.Columns.Add(new DataColumn("ColBillNo", typeof(string)));
        dt.Columns.Add(new DataColumn("ColBillDate", typeof(string)));
        dt.Columns.Add(new DataColumn("ColPartCode", typeof(string)));
        dt.Columns.Add(new DataColumn("ColItemDescription", typeof(string)));
        dt.Columns.Add(new DataColumn("ColQuantity", typeof(string)));
        dt.Columns.Add(new DataColumn("Column3", typeof(string)));
        //Add a Dummy Data on Initial Load
        var dr = dt.NewRow();
        dr["RowNumber"] = 1;
        dt.Rows.Add(dr);

        //Store the DataTable in ViewState
        ViewState["CurrentTable"] = dt;
    }

    protected void ddlVehicles_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlVehicles.SelectedIndex != 0)
        {
            _vehicleobj.VehNumforNonOff = ddlVehicles.SelectedItem.ToString();
            var ds = _vehicleobj.getDistandLocation();
            if (ds.Tables[0] != null) txtLocation.Text = ds.Tables[0].Rows[0]["vi_BaseLocation"].ToString();
            if (ds.Tables[1] != null) txtDistrict.Text = ds.Tables[1].Rows[0]["ds_lname"].ToString();
        }
    }

    protected void btnSPReset_Click(object sender, EventArgs e)
    {
        SetInitialMaintenanceDetails();
        ddlVehicles.SelectedIndex = 0;
        txtDistrict.Text = "";
        txtLocation.Text = "";
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        _vehicleobj.VehNumforNonOff = ddlVehicles.SelectedItem.Text;
        _vehicleobj.NonOffVenName = ddlVendorName.Text;
        _vehicleobj.NonOffMainType = ddlMaintenanceType.SelectedItem.Text;
        _vehicleobj.NonOffMainDate = Convert.ToDateTime(txtMaintenanceDate.Text);
        _vehicleobj.NonOffBillNo = txtBillNo.Text;
        _vehicleobj.NonOffBillDate = Convert.ToDateTime(txtBillDate.Text);
        _vehicleobj.NonOffPartCode = txtPartCode.Text;
        _vehicleobj.NonOffItemDesc = txtItemDesc.Text;
        _vehicleobj.NonOffQuant = txtQuant.Text;
        _vehicleobj.NonOffAmount = txtBillAmount.Text;
        _vehicleobj.District = txtDistrict.Text;
        _vehicleobj.LocationName = txtLocation.Text;
        var x = _vehicleobj.InsertNonOffroadVehMaintenance();
        switch (x)
        {
            case 0:
                Show("Records not inserted successfully");
                break;
            default:
                Show("Records inserted successfully");
                SetInitialMaintenanceDetails();
                ddlVehicles.SelectedIndex = 0;
                txtDistrict.Text = "";
                txtLocation.Text = "";
                ddlVehicles.SelectedIndex = 0;
                ddlVendorName.SelectedIndex = 0;
                ddlMaintenanceType.SelectedIndex = 0;
                txtMaintenanceDate.Text = "";
                txtBillNo.Text = "";
                txtBillDate.Text = "";
                txtPartCode.Text = "";
                txtItemDesc.Text = "";
                txtQuant.Text = "";
                txtBillAmount.Text = "";
                txtDistrict.Text = "";
                txtLocation.Text = "";
                break;
        }
    }

    protected void grdvwMaintenanceDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    var ddlVName = e.Row.FindControl("ddlVendorName") as DropDownList;
                    var ddlMType = e.Row.FindControl("ddlMaintenanceType") as DropDownList;
                    var ds = _vehMain.IFillVendorsMaintenance();
                    if (ddlVName != null)
                    {
                        ddlVName.Items.Clear();
                        if (ds != null)
                        {
                            _helper.FillDropDownHelperMethodWithDataSet(ds, "AgencyName", "AgencyId", ddlVName);
                            if (ddlMType != null) _helper.FillDifferentDataTables(ddlMType, ds.Tables[1], "MaintId", "MaintenanceType");
                        }
                    }

                    break;
            }
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    public void Show(string message)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('" + message + "');", true);
    }

    protected void grdvwMaintenanceDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
    }
}