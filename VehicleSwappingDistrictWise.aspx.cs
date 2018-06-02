using System;
using System.Configuration;
using System.Data;
using System.Web.UI;
using GvkFMSAPP.BLL;
using GvkFMSAPP.BLL.VAS_BLL;

public partial class VehicleSwappingDistrictWise : Page
{
    private readonly BaseVehicleDetails _fmsobj = new BaseVehicleDetails();
    private readonly Helper _helper = new Helper();
    private readonly VASGeneral _vasbll = new VASGeneral();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Name"] == null) Response.Redirect("Login.aspx");
        if (!IsPostBack)
        {
            btnSubmit.Attributes.Add("onclick", "return validation()");
            ddlSrcVehicle.Enabled = false;
            ddlDestVehicle.Enabled = false;
            ddlDestDistrict.Enabled = false;
            GetDistrict();
            var o = Session["User_Name"];
            if (o != null) txtRequestedBy.Text = o.ToString();
            btnSubmit.Enabled = true;
        }
    }

    public void GetDistrict()
    {
        try
        {
            var dsDistrict = _fmsobj.GetDistrict();
            if (dsDistrict == null) throw new ArgumentNullException(nameof(dsDistrict));
            _helper.FillDropDownHelperMethodWithDataSet(dsDistrict, "ds_lname", "ds_dsid", ddlSourceDistrict);
            ViewState["Districts"] = dsDistrict;
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    protected void ddlSrcVehicle_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlSrcVehicle.SelectedIndex == 0) return;
        var ds = (DataSet) Session["dsvehicle"];
        if (ds == null) throw new ArgumentNullException(nameof(ds));
        var dvsrcvehbase = new DataView(ds.Tables[0], "VehicleID ='" + ddlSrcVehicle.SelectedItem.Value + "'", "VehicleNumber", DataViewRowState.CurrentRows);
        if (dvsrcvehbase.ToTable().Rows.Count <= 0) return;
        txtSrcBaseLocation.Text = dvsrcvehbase[0][1].ToString();
        txtSrcContactNo.Text = dvsrcvehbase[0][3].ToString();
        ViewState["SrcBaseLocationId"] = Convert.ToInt32(dvsrcvehbase[0][4]);
    }

    protected void ddlDestVehicle_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlDestVehicle.SelectedIndex == 0) return;
        var dsDestVeh = (DataSet) Session["dsvehicle"];
        if (dsDestVeh == null) throw new ArgumentNullException(nameof(dsDestVeh));
        var dvdestvehbase = new DataView(dsDestVeh.Tables[0], "VehicleID ='" + ddlDestVehicle.SelectedItem.Value + "'", "VehicleNumber", DataViewRowState.CurrentRows);
        if (dvdestvehbase.ToTable().Rows.Count <= 0) return;
        txtDestBaseLocation.Text = dvdestvehbase[0][1].ToString();
        txtDestContactNo.Text = dvdestvehbase[0][3].ToString();
        ViewState["DestBaseLocationId"] = Convert.ToInt32(dvdestvehbase[0][4]);
    }

    protected void ddlSourceDistrict_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            switch (ddlSourceDistrict.SelectedIndex)
            {
                case 0:
                    ddlDestDistrict.DataSource = null;
                    ddlDestDistrict.DataBind();
                    ddlDestDistrict.Enabled = false;
                    ddlDestVehicle.DataSource = null;
                    ddlDestVehicle.DataBind();
                    ddlDestVehicle.Enabled = false;
                    ddlSrcVehicle.DataSource = null;
                    ddlSrcVehicle.DataBind();
                    ddlSrcVehicle.Enabled = false;
                    break;
                default:
                    ddlDestDistrict.Enabled = true;
                    //Binding Source Vehicles
                    ddlSrcVehicle.Enabled = true;
                    _vasbll.DistrictId = Convert.ToInt32(ddlSourceDistrict.SelectedItem.Value);
                    var ds = _vasbll.GetActiveVehicles();
                    ddlSrcVehicle.DataSource = ds;
                    _helper.FillDropDownHelperMethodWithDataSet(ds, "VehicleNumber", "VehicleID", ddlSrcVehicle);
                    Session["dsvehicle"] = ds;
                    //Destination District based on Source district
                    var dsDestDist = (DataSet) ViewState["Districts"];
                    var dvDestDist = dsDestDist.Tables[0].DefaultView;
                    dvDestDist.RowFilter = "ds_lname <>'" + ddlSourceDistrict.SelectedItem.Text + "'";
                    _helper.FillDropDownHelperMethodWithDataSet(dsDestDist, "ds_lname", "ds_dsid", ddlDestDistrict);
                    break;
            }
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    protected void ddlDestDistrict_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            switch (ddlDestDistrict.SelectedIndex)
            {
                case 0:
                    ddlDestDistrict.DataSource = null;
                    ddlDestDistrict.DataBind();
                    ddlDestDistrict.Enabled = false;
                    break;
                default:
                    //Binding Destination Vehicles
                    ddlDestVehicle.Enabled = true;
                    _vasbll.DistrictId = Convert.ToInt32(ddlDestDistrict.SelectedItem.Value);
                    var ds = _vasbll.GetActiveVehicles();
                    ddlDestVehicle.DataSource = ds;
                    _helper.FillDropDownHelperMethodWithDataSet(ds, "VehicleNumber", "VehicleID", ddlDestVehicle);
                    Session["dsvehicle"] = ds;
                    break;
            }
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        btnSubmit.Enabled = false;
        _vasbll.SrcDistrictId = Convert.ToInt32(ddlSourceDistrict.SelectedItem.Value);
        _vasbll.DestDistrictId = Convert.ToInt32(ddlDestDistrict.SelectedItem.Value);
        _vasbll.SrcLocId = Convert.ToInt32(ViewState["SrcBaseLocationId"]);
        _vasbll.DestLocId = Convert.ToInt32(ViewState["DestBaseLocationId"]);
        _vasbll.SrcVehNo = ddlSrcVehicle.SelectedItem.Text;
        _vasbll.DestVehNo = ddlDestVehicle.SelectedItem.Text;
        _vasbll.SrcContactNo = txtSrcContactNo.Text;
        _vasbll.DestContactNo = txtDestContactNo.Text;
        if (string.IsNullOrEmpty(Convert.ToString(ConfigurationManager.AppSettings["AVLT"])))
        {
            var swapres = _vasbll.VehicleSwappingDistrictWise();
            Show(swapres != 0 ? "Vehicles are Swapped Successfully!!" : "Error!!");
        }
        else
        {
            var avltBaseLoc = _vasbll.CheckBaseLocationAVLT();
            switch (avltBaseLoc)
            {
                case 2:
                    Show("BaseLocations of selected vehicles are not Present in AVLT Database");
                    break;
                default:
                    var swapres = _vasbll.VehicleSwappingDistrictWise();
                    Show(swapres != 0 ? "Vehicles are Swapped Successfully!!" : "Error!!");
                    break;
            }
        }

        ClearAll();
    }

    private void ClearAll()
    {
        ddlSourceDistrict.SelectedIndex = 0;
        ddlSrcVehicle.SelectedIndex = 0;
        ddlSrcVehicle.Enabled = false;
        ddlDestDistrict.SelectedIndex = 0;
        ddlDestDistrict.Enabled = false;
        ddlDestVehicle.SelectedIndex = 0;
        ddlDestVehicle.Enabled = false;
        txtSrcBaseLocation.Text = "";
        txtSrcContactNo.Text = "";
        txtDestBaseLocation.Text = "";
        txtDestContactNo.Text = "";
        btnSubmit.Enabled = true;
    }

    public void Show(string message)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('" + message + "');", true);
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        ClearAll();
    }
}