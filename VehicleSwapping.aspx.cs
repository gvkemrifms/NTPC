using System;
using System.Configuration;
using System.Data;
using System.Web.UI;
using GvkFMSAPP.BLL;
using GvkFMSAPP.BLL.VAS_BLL;

public partial class VehicleSwapping : Page
{
    private readonly BaseVehicleDetails _fmsobj = new BaseVehicleDetails();
    private readonly Helper _helper = new Helper();
    private readonly VASGeneral _vasbll = new VASGeneral();

    public string UserId { get; private set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] == null)
            Response.Redirect("Login.aspx");
        else
            UserId = (string)Session["User_Id"];
        if (IsPostBack) return;
        btnSubmit.Attributes.Add("onclick", "return validation()");
        GetDistrict();
        var o = Session["User_Name"];
        if (o != null) txtRequestedBy.Text = o.ToString();
        btnSubmit.Enabled = true;
    }

    public void GetDistrict()
    {
        try
        {
            var sqlQuery = ConfigurationManager.AppSettings["Query"] + " " + "where u.UserId ='" + UserId + "'";
            _helper.FillDropDownHelperMethod(sqlQuery, "district_name", "district_id", ddlDistrict);
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        btnSubmit.Enabled = false;
        _vasbll.DistrictId = Convert.ToInt32(ddlDistrict.SelectedItem.Value);
        _vasbll.SrcLocId = Convert.ToInt32(ViewState["SrcBaseLocationId"]);
        _vasbll.DestLocId = Convert.ToInt32(ViewState["DestBaseLocationId"]);
        _vasbll.SrcVehNo = ddlSrcVehicle.SelectedItem.Text;
        _vasbll.DestVehNo = ddlDestVehicle.SelectedItem.Text;
        _vasbll.SrcContactNo = txtSrcContactNo.Text;
        _vasbll.DestContactNo = txtDestContactNo.Text;
        if (string.IsNullOrEmpty(Convert.ToString(ConfigurationManager.AppSettings["AVLT"])))
        {
            var swapres = _vasbll.VehicleSwapping();
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
                    var swapres = _vasbll.VehicleSwapping();
                    Show(swapres != 0 ? "Vehicles are Swapped Successfully!!" : "Error!!");
                    break;
            }
        }

        ClearAll();
    }

    public void Show(string message)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('" + message + "');", true);
    }

    protected void ddlDistrict_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            _vasbll.DistrictId = Convert.ToInt32(ddlDistrict.SelectedItem.Value);
            var ds = _vasbll.GetActiveVehicles();
            _helper.FillDropDownHelperMethodWithDataSet(ds, "VehicleNumber", "VehicleID", ddlSrcVehicle);
            Session["dsvehicle"] = ds;
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    protected void ddlSrcVehicle_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlSrcVehicle.SelectedIndex == 0) return;
            var ds = (DataSet) Session["dsvehicle"];
            var dsVehicle = new DataSet();
            var dvsrcvehbase = new DataView(ds.Tables[0], "VehicleID ='" + ddlSrcVehicle.SelectedItem.Value + "'", "VehicleNumber", DataViewRowState.CurrentRows);
            txtSrcBaseLocation.Text = dvsrcvehbase[0][1].ToString();
            txtSrcContactNo.Text = dvsrcvehbase[0][3].ToString();
            ViewState["SrcBaseLocationId"] = Convert.ToInt32(dvsrcvehbase[0][4]);
            _helper.FillDropDownHelperMethodWithDataSet(ds, "VehicleNumber", "VehicleID", ddlDestVehicle);
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    protected void ddlDestVehicle_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlDestVehicle.SelectedIndex == 0) return;
        var dsDestVeh = (DataSet) Session["dsvehicle"];
        if (dsDestVeh == null) throw new ArgumentNullException(nameof(dsDestVeh));
        var dvdestvehbase = new DataView(dsDestVeh.Tables[0], "VehicleID ='" + ddlDestVehicle.SelectedItem.Value + "'", "VehicleNumber", DataViewRowState.CurrentRows);
        txtDestBaseLocation.Text = dvdestvehbase[0][1].ToString();
        txtDestContactNo.Text = dvdestvehbase[0][3].ToString();
        ViewState["DestBaseLocationId"] = Convert.ToInt32(dvdestvehbase[0][4]);
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        ClearAll();
    }

    public void ClearAll()
    {
        ddlDistrict.SelectedIndex = 0;
        ddlSrcVehicle.SelectedIndex = 0;
        ddlDestVehicle.SelectedIndex = 0;
        txtSrcBaseLocation.Text = "";
        txtDestBaseLocation.Text = "";
        txtSrcContactNo.Text = "";
        txtDestContactNo.Text = "";
        btnSubmit.Enabled = true;
    }
}