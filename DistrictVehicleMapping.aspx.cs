using System;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using GvkFMSAPP.BLL.VAS_BLL;
public partial class DistrictVehicleMapping : Page
{
    private readonly GvkFMSAPP.BLL.Admin.DistrictVehicleMapping _distvehmapp = new GvkFMSAPP.BLL.Admin.DistrictVehicleMapping();
    private readonly FleetMaster _fleetMaster = new FleetMaster();
    private readonly Helper _helper = new Helper();
    private readonly VASGeneral _vehallobj = new VASGeneral();
    public string UserId{ get;private set; }

    protected void Page_Load(object sender,EventArgs e)
    {
        if (Session["User_Id"] == null)
            Response.Redirect("Login.aspx");
        else
            UserId = (string) Session["User_Id"];
        if (!IsPostBack)
        {
            ScriptManager.RegisterStartupScript(Page,Page.GetType(),"text","abc()",true);
            btnSave.Attributes.Add("onclick","return validation()");
            if (_distvehmapp != null)
                try
                {
                    var sqlQuery = ConfigurationManager.AppSettings["Query"] + " " + "where u.UserId ='" + UserId + "'";
                    _helper.FillDropDownHelperMethodWithDataSet(_distvehmapp.GetVehicleTypes(),"vehicle_type_name","vehicle_type_id",ddlVehType); //Fill VehicleTypes
                    _helper.FillDropDownHelperMethodWithDataSet(_distvehmapp.GetVehicles(),"VehicleNumber","VehicleID",ddlVehicleNumber); //GetVehicleNumber
                    _helper.FillDropDownHelperMethod(sqlQuery,"district_name","district_id",ddlDistrict); //GetDistricts
                }
                catch (Exception ex)
                {
                    _helper.ErrorsEntry(ex);
                }
        }
    }

    protected void lnkbtnNewBaseLoc_Click(object sender,EventArgs e)
    {
        ddlBaseLocation.Visible = false;
        txtBaseLocation.Visible = true;
        lnkbtnExtngBaseLoc.Visible = true;
        lnkbtnNewBaseLoc.Visible = false;
        ddlBaseLocation.SelectedIndex = 0;
        txtContactNumber.Text = "";
        lblLatitude.Visible = true;
        lblLongitude.Visible = true;
        lblMandatory1.Visible = true;
        lblMandatory2.Visible = true;
        txtLatitude.Visible = true;
        txtLongitude.Visible = true;
        ScriptManager.RegisterStartupScript(Page,Page.GetType(),"text",null,true);
    }

    protected void lnkbtnExtngBaseLoc_Click(object sender,EventArgs e)
    {
        ddlBaseLocation.Visible = true;
        txtBaseLocation.Visible = false;
        lnkbtnExtngBaseLoc.Visible = false;
        lnkbtnNewBaseLoc.Visible = true;
        lblLatitude.Visible = false;
        lblLongitude.Visible = false;
        lblMandatory1.Visible = false;
        lblMandatory2.Visible = false;
        txtLatitude.Visible = false;
        txtLongitude.Visible = false;
    }

    protected void ddlDistrict_SelectedIndexChanged(object sender,EventArgs e)
    {
        ddlMandal.Items.Clear();
        ddlMandal.Items.Insert(0,new ListItem("--Select--","0"));
        ddlCity.Items.Clear();
        ddlCity.Items.Insert(0,new ListItem("--Select--","0"));
        txtContactNumber.Text = "";
        if (ddlDistrict != null)
            if (_vehallobj != null)
                _vehallobj.DistrictId = Convert.ToInt32(ddlDistrict.SelectedItem.Value);
        if (_vehallobj != null)
            try
            {
                _helper.FillDropDownHelperMethodWithDataSet(_vehallobj.GetMandals_new(),"mandal_name","mandal_id",ddlMandal); //GetMandals
            }
            catch (Exception ex)
            {
                _helper.ErrorsEntry(ex);
            }
    }

    public void GetBaseLocation()
    {
        if (ddlCity != null && _vehallobj != null) _vehallobj.CityId = Convert.ToInt32(ddlCity.SelectedItem.Value);
        if (_vehallobj != null)
            using (var ds = _vehallobj.GetBaseLocation())
            {
                ViewState["ContactNumber"] = ds;
                if (ds != null)
                    try
                    {
                        _helper.FillDropDownHelperMethodWithDataSet(ds,"Base_Location","Location_ID",ddlBaseLocation);
                    }
                    catch (Exception ex)
                    {
                        _helper.ErrorsEntry(ex);
                    }
            }
    }

    protected void ddlSegments_SelectedIndexChanged(object sender,EventArgs e)
    {
        if (ddlDistrict != null && _vehallobj != null) _vehallobj.DistrictId = Convert.ToInt32(ddlDistrict.SelectedItem.Value);
        if (_vehallobj != null)
            try
            {
                _helper.FillDropDownHelperMethodWithDataSet(_vehallobj.GetMandals_new(),"mandal_name","mandal_id",ddlMandal); //GetMandals
            }
            catch (Exception ex)
            {
                _helper.ErrorsEntry(ex);
            }

        GetDistrictMandals();
    }

    public void GetDistrictMandals()
    {
        _vehallobj.DistrictId = Convert.ToInt32(ddlDistrict.SelectedItem.Value);
        _vehallobj.GetMandalsDistAndSegwise();
    }

    protected void ddlMandal_SelectedIndexChanged(object sender,EventArgs e)
    {
        if (_vehallobj != null)
        {
            if (ddlMandal != null) _vehallobj.MandalId = Convert.ToInt32(ddlMandal.SelectedItem.Value);
            try
            {
                _helper.FillDropDownHelperMethodWithDataSet(_vehallobj.GetCities_new(),"ct_lname","city_id",ddlCity);
            }
            catch (Exception ex)
            {
                _helper.ErrorsEntry(ex);
            }
        }
    }

    protected void ddlCity_SelectedIndexChanged(object sender,EventArgs e)
    {
        GetBaseLocation();
    }

    protected void btnReset_Click(object sender,EventArgs e)
    {
        ClearAll();
    }

    protected void btnSave_Click(object sender,EventArgs e)
    {
        try
        {
            if (ddlVehType.SelectedIndex <= 0)
            {
                Show("Please Select vehicle type");
            }
            else
            {
                _vehallobj.OffRoadVehcileId = Convert.ToInt32(ddlVehicleNumber.SelectedItem.Value);
                _vehallobj.OffRoadVehicleNo = ddlVehicleNumber.SelectedItem.Text;
                _vehallobj.DistrictId = Convert.ToInt32(ddlDistrict.SelectedItem.Value);
                _vehallobj.District = ddlDistrict.SelectedItem.Text;
                _vehallobj.MandalId = Convert.ToInt32(ddlMandal.SelectedItem.Value);
                _vehallobj.Mandal = ddlMandal.SelectedItem.Text;
                _vehallobj.CityId = Convert.ToInt32(ddlCity.SelectedItem.Value);
                _vehallobj.City = ddlCity.SelectedItem.Text;
                if (ddlBaseLocation.Visible)
                {
                    _vehallobj.BaseLocationId = Convert.ToInt32(ddlBaseLocation.SelectedItem.Value);
                    _vehallobj.BaseLocation = ddlBaseLocation.SelectedItem.Text;
                    _vehallobj.Flag = "Old";
                    _vehallobj.Latitude = "0.00";
                    _vehallobj.Longitude = "0.00";
                    // Bind Lat Longs
                }
                else
                {
                    _vehallobj.BaseLocationId = 0;
                    _vehallobj.BaseLocation = txtBaseLocation.Text;
                    _vehallobj.Flag = "New";
                    _vehallobj.Latitude = txtLatitude.Text;
                    _vehallobj.Longitude = txtLongitude.Text;
                }

                _vehallobj.SegmentId = 0;
                _vehallobj.Segment = "";
                _vehallobj.NewSegFlag = "New";
                //for (var i = 1; i < ddlMandal.Items.Count; i++) manids = manids + ddlMandal.Items[i].Value + ",";
                _vehallobj.NewSegMandalIds = "";
                _vehallobj.SegmentId = 0;
                _vehallobj.Latitude = txtLatitude.Text;
                _vehallobj.Longitude = txtLongitude.Text;
                _vehallobj.ContactNumber = txtContactNumber.Text;
                _vehallobj.VehType = ddlVehType.SelectedItem.Value;
                var clsGen = new ClsGeneral();
                var dtGetVehData = clsGen.getVehicleData(ddlVehicleNumber.SelectedItem.Text);
                var insres = _fleetMaster.InsNewVehAllocation_new(_vehallobj.OffRoadVehcileId,_vehallobj.OffRoadVehicleNo,_vehallobj.DistrictId,_vehallobj.District,0,"",_vehallobj.MandalId,_vehallobj.Mandal,_vehallobj.CityId,_vehallobj.City,_vehallobj.BaseLocationId,_vehallobj.BaseLocation,_vehallobj.ContactNumber,"New","New",_vehallobj.NewSegMandalIds,_vehallobj.Latitude,_vehallobj.Longitude,_vehallobj.VehType);
                switch (insres)
                {
                    case 0:
                        Show("Error!!");
                        break;
                    default:
                        clsGen.InsertVehicle(ddlVehicleNumber.SelectedItem.Value,ddlVehicleNumber.SelectedItem.Text,"1",txtContactNumber.Text,txtLatitude.Text,txtLongitude.Text,ddlVehType.SelectedItem.Text,ddlDistrict.SelectedItem.Value,ddlMandal.SelectedItem.Value,ddlBaseLocation.Visible ? ddlBaseLocation.SelectedItem.Text : txtBaseLocation.Text);
                        if (dtGetVehData.Rows.Count > 0) UpdateData(ddlVehicleNumber,txtContactNumber.Text,txtLatitude.Text,txtLongitude.Text,ddlDistrict.SelectedItem.Value,ddlMandal.SelectedItem.Value,ddlBaseLocation.SelectedItem.Text);
                        Show("Record Inserted Successfully!!");
                        break;
                }

                ClearAll();
            }
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    private void UpdateData(DropDownList vehicleNumber,string contactNumber,string latitude,string longitude,string district,string mandal,string baseLocation)
    {
        var statement = "";
        statement = statement + "update m_vehicle set  `contact_number` = '" + contactNumber + "',`latitude`= '" + latitude + "',`longitude` = '" + longitude + "',`mandal_id`  = '" + mandal + "',`location_name`  = '" + baseLocation + "', district_id= '" + district + "'";
        statement = statement + "  where `vehicle_no` = '" + vehicleNumber.SelectedItem.Text + "' ;";
        try
        {
            _helper.ExecuteInsertStatement(statement);
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    public void Show(string message)
    {
        ScriptManager.RegisterStartupScript(this,GetType(),"msg","alert('" + message + "');",true);
    }

    public void ClearAll()
    {
        ddlVehicleNumber.SelectedIndex = 0;
        ddlDistrict.SelectedIndex = 0;
        ddlVehType.SelectedIndex = 0;
        ddlMandal.Items.Clear();
        ddlMandal.Items.Insert(0,new ListItem("--Select--","0"));
        ddlCity.Items.Clear();
        ddlCity.Items.Insert(0,new ListItem("--Select--","0"));
        ddlBaseLocation.Items.Clear();
        ddlBaseLocation.Items.Insert(0,new ListItem("--Select--","0"));
        txtBaseLocation.Text = "";
        txtContactNumber.Text = "";
        lblLatitude.Visible = false;
        lblLongitude.Visible = false;
        lblMandatory1.Visible = false;
        lblMandatory2.Visible = false;
        txtLatitude.Visible = false;
        txtLongitude.Visible = false;
        txtLatitude.Text = "";
        txtLongitude.Text = "";
    }

    protected void ddlVehicleNumber_SelectedIndexChanged(object sender,EventArgs e)
    {
        if (_vehallobj == null) return;
        try
        {
            _vehallobj.OffRoadVehicleNo = ddlVehicleNumber.SelectedItem.Text;
            var dsvehalldet = _vehallobj.GetVehAllocationDet();
            if (dsvehalldet != null && dsvehalldet.Tables[0].Rows.Count <= 0) return;
            var sb = new StringBuilder();
            sb.Append("Vehicle is already allocated to :\\n");
            if (dsvehalldet != null)
            {
                sb.Append("\\nDistrict      - " + dsvehalldet.Tables[0].Rows[0]["District"]);
                sb.Append("\\nSegment       - " + dsvehalldet.Tables[0].Rows[0]["Segment"]);
                sb.Append("\\nMandal        - " + dsvehalldet.Tables[0].Rows[0]["Mandal"]);
                sb.Append("\\nCity/Village  - " + dsvehalldet.Tables[0].Rows[0]["City"]);
                sb.Append("\\nBase Location - " + dsvehalldet.Tables[0].Rows[0]["BaseLocation"]);
            }

            var str = sb.ToString();
            Show(str);
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    protected void lnkbtnNewSegment_Click(object sender,EventArgs e)
    {
        _vehallobj.DistrictId = Convert.ToInt32(ddlDistrict.SelectedItem.Value);
        _vehallobj.GetMandalsDistictwise();
        ddlMandal.Items.Clear();
        ddlMandal.Items.Insert(0,new ListItem("--Select--","0"));
        ddlCity.Items.Clear();
        ddlCity.Items.Insert(0,new ListItem("--Select--","0"));
        txtContactNumber.Text = "";
    }

    protected void lnkbtnExtngSegment_Click(object sender,EventArgs e)
    {
        ddlMandal.Items.Clear();
        ddlMandal.Items.Insert(0,new ListItem("--Select--","0"));
        ddlCity.Items.Clear();
        ddlCity.Items.Insert(0,new ListItem("--Select--","0"));
        txtContactNumber.Text = "";
    }

    protected void chkblstmandals_SelectedIndexChanged(object sender,EventArgs e)
    {
    }

    protected void ddlBaseLocation_SelectedIndexChanged(object sender,EventArgs e)
    {
        if (ddlBaseLocation == null) return;
        try
        {
            switch (ddlBaseLocation.SelectedIndex)
            {
                case 0:
                    txtContactNumber.Text = "";
                    txtLatitude.Text = "";
                    txtLongitude.Text = "";
                    break;
                default:
                    var dsblcn = (DataSet) ViewState["ContactNumber"];
                    var dv = new DataView(dsblcn.Tables[0],"Location_ID =" + Convert.ToInt32(ddlBaseLocation.SelectedItem.Value),"Contact_Number",DataViewRowState.CurrentRows);
                    txtContactNumber.Text = dv[0]["Contact_Number"].ToString();
                    txtLatitude.Text = dv[0]["latitude"].ToString();
                    txtLongitude.Text = dv[0]["longitude"].ToString();
                    var csg = new ClsGeneral();
                    var dtVehData = csg.getVehiclesinRadius(txtLatitude.Text,txtLongitude.Text,ConfigurationManager.AppSettings["Locateveh"]);
                    if (dtVehData.Rows.Count > 0)
                    {
                        lblVeh.Text = "Vehicles that are under " + ConfigurationManager.AppSettings["Locateveh"] + "KMs to base location ";
                        grdVehicleData.DataSource = dtVehData;
                        grdVehicleData.DataBind();
                    }

                    break;
            }
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }
}