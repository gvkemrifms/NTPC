using System;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Web.UI;
using System.Web.UI.WebControls;
using GvkFMSAPP.BLL.VAS_BLL;
using GvkFMSAPP.DLL;
using MySql.Data.MySqlClient;

public partial class VehicleAllocation : Page
{
    private readonly GvkFMSAPP.BLL.BaseVehicleDetails _fmsobj = new GvkFMSAPP.BLL.BaseVehicleDetails();
    private readonly Helper _helper = new Helper();
    private readonly VASGeneral _vehallobj = new VASGeneral();

    protected void Page_PreInit(object sender, EventArgs e)
    {
        if (Session["Role_Id"] != null)
        {
            if (Session["Role_Id"].ToString() == "120") MasterPageFile = "~/MasterERO.master";
        }
        else
        {
            Response.Redirect("Login.aspx");
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Name"] == null) Response.Redirect("Login.aspx");
        if (!IsPostBack)
        {
            btnSubmit.Attributes.Add("onclick", "return validation()");
            GetDistrict();
            FillHoursandMins();
            if (Session["User_Name"] != null) txtReqBy.Text = Session["User_Name"].ToString();
            GetTime();
            btnSubmit.Enabled = true;
        }
    }

    public void GetDistrict()
    {
        try
        {
            var ds = _fmsobj.GetDistricts_new();
            if (ds != null) _helper.FillDropDownHelperMethodWithDataSet(ds, "district_name", "district_id", ddlDistrict);
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    protected void GetTime()
    {
        var timenow = DateTime.Now.ToString(CultureInfo.InvariantCulture).Split(' ');
        txtUptimeDate.Text = timenow[0];
        var hour = Convert.ToInt32(timenow[1].Split(':')[0]);
        var minute = Convert.ToInt32(timenow[1].Split(':')[1]);
        if (timenow.Length > 2)
        {
            if (timenow[2] != "PM")
                if (hour < 10)
                    ddlUPHour.Items.FindByValue("0" + timenow[1].Split(':')[0]).Selected = true;
                else
                    ddlUPHour.Items.FindByValue(timenow[1].Split(':')[0]).Selected = true;
            else
                switch (hour)
                {
                    case 12:
                        ddlUPHour.Items.FindByValue(hour.ToString()).Selected = true;
                        break;
                    default:
                        hour = hour + 12;
                        ddlUPHour.Items.FindByValue(hour.ToString()).Selected = true;
                        break;
                }
        }
        else
        {
            if (hour == 12)
                hour = 0;
            else if (hour > 12) hour = hour - 12;
            hour = hour + 12;
            ddlUPHour.Items.FindByValue(hour.ToString()).Selected = true;
        }

        ddlUPMin.Items.FindByValue(timenow[1].Split(':')[1]).Selected = minute < 10;
    }

    public static string GetLocalIpAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
            if (ip.AddressFamily == AddressFamily.InterNetwork)
                return ip.ToString();
        throw new Exception("Local IP Address Not Found!");
    }

    public void InsertAgent(string offroadid, string vehicleNo, string agentId)
    {
        try
        {
            var ip = GetLocalIpAddress();
            var insertQuery = "insert into t_onroadAgent(onroadId ,vehicleNo, AgentID,AgentName,ip) values ('" + offroadid + "', '" + vehicleNo + "', '" + agentId + "', '" + Session["User_Name"] + "','" + ip + "')";
            _helper.ExecuteInsertStatement(insertQuery);
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        btnSubmit.Enabled = false;
        if (Convert.ToDecimal(ViewState["OdoReading"].ToString()) <= Convert.ToDecimal(txtOdo.Text) && Convert.ToDateTime(txtDownTime.Text) < Convert.ToDateTime(txtUptimeDate.Text + " " + ddlUPHour.SelectedValue + ":" + ddlUPMin.SelectedValue))
        {
            _vehallobj.District = ddlDistrict.SelectedItem.Text;
            _vehallobj.OffRoadVehicleNo = ddlVehicleNumber.SelectedItem.Text;
            _vehallobj.ReasonForOffRoad = txtReasonforDown.Text;
            _vehallobj.OffRoadDate = Convert.ToDateTime(txtDownTime.Text);
            _vehallobj.Odometer = txtOdo.Text;
            _vehallobj.RequestedBy = txtReqBy.Text;
            _vehallobj.UpTime = Convert.ToDateTime(txtUptimeDate.Text + " " + ddlUPHour.SelectedValue + ":" + ddlUPMin.SelectedValue);
            _vehallobj.BaseLocation = "0";
            _vehallobj.NewSegFlag = "";
            _vehallobj.NewSegMandalIds = "";
            _vehallobj.Mandal = "";
            _vehallobj.City = "";
            _vehallobj.DistrictId = Convert.ToInt32(ddlDistrict.SelectedItem.Value);
            _vehallobj.MandalId = 0;
            _vehallobj.CityId = 0;
            _vehallobj.Flag = "Old";
            _vehallobj.Latitude = "0.00";
            _vehallobj.Longitude = "0.00";
            _vehallobj.SegFlag = "Active";
            _vehallobj.Segment = "";
            _vehallobj.ContactNumber = txtContactNumber.Text;
            MakeVehicleonRoad(ddlVehicleNumber.SelectedItem.Text, Convert.ToDateTime(txtUptimeDate.Text + " " + ddlUPHour.SelectedValue + ":" + ddlUPMin.SelectedValue).ToString("yyyy-MM-dd HH:mm:ss"), Session["User_Name"].ToString(), txtOdo.Text, txtReqBy.Text, "NA", "NA");
            var insres = _vehallobj.InsOffRoadVehAllocation();
            switch (insres)
            {
                case 0:
                    Show("Record not Inserted Successfully!!");
                    break;
                default:
                    InsertAgent(insres.ToString(), ddlVehicleNumber.SelectedItem.Text, Session["User_Id"].ToString());
                    SendSms(ddlVehicleNumber.SelectedItem.Text, "", txtReasonforDown.ToString());
                    Show("Record Inserted Successfully!!");
                    break;
            }

            ClearAll();
        }
        else
        {
            if (Convert.ToDecimal(ViewState["OdoReading"].ToString()) > Convert.ToDecimal(txtOdo.Text))
            {
                Show("Odometer reading should be greater than " + ViewState["OdoReading"]);
                btnSubmit.Enabled = true;
            }
            else
            {
                Show("DownTime should be less than UpTime");
                btnSubmit.Enabled = true;
            }
        }
    }

    public void SendSms(string vehicleno, string breakdownid, string reason)
    {
        try
        {
            var query = "select * from m_vehicle_supervisors where VehicleNo = '" + vehicleno + "'";
            var dtPenData = _helper.ExecuteSelectStmt(query);
            if (dtPenData.Rows.Count > 0)
            {
                var smsContent = "OnRoad-- Dear {name},\nVehicleNumber-" + vehicleno + " is made on road and base location:" + dtPenData.Rows[0]["BaseLocation"] + " by ERO:" + Session["User_Name"];
                Insertdata(smsContent, dtPenData.Rows[0]["Emename"].ToString(), dtPenData.Rows[0]["EmeContactNumber"].ToString());
                Insertdata(smsContent, dtPenData.Rows[0]["Pmname"].ToString(), dtPenData.Rows[0]["PmContactNumber"].ToString());
                Insertdata(smsContent, dtPenData.Rows[0]["Rmname"].ToString(), dtPenData.Rows[0]["RmContactNumber"].ToString());
            }
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    public void Insertdata(string smsContent, string name, string mobileno)
    {
        try
        {
            smsContent = smsContent.Replace("{name}", name);
            var query = "insert into t_SMS(smsContent ,mobileno) values ('" + smsContent + "', '" + mobileno + "')";
            _helper.ExecuteInsertStatement(query);
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

    protected void ddlDistrict_SelectedIndexChanged(object sender, EventArgs e)
    {
        ClearDropdown();
        GetVehicles();
    }

    public void GetVehicles()
    {
        try
        {
            _vehallobj.District = ddlDistrict.SelectedItem.Text;
            var ds = _vehallobj.GetVehicleNo();
            if (ds == null) throw new ArgumentNullException(nameof(ds));
            _helper.FillDropDownHelperMethodWithDataSet(ds, "OffRoadVehicle_No", "OffRoadVehicle_No", ddlVehicleNumber);
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    private void FillHoursandMins()
    {
        int i;
        for (i = 0; i < 24; i++) ddlUPHour.Items.Add(i < 10 ? new ListItem("0" + i, "0" + i) : new ListItem(i.ToString(), i.ToString()));
        for (i = 0; i < 60; i++) ddlUPMin.Items.Add(i < 10 ? new ListItem("0" + i, "0" + i) : new ListItem(i.ToString(), i.ToString()));
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        ClearAll();
    }

    public void ClearAll()
    {
        ddlDistrict.SelectedIndex = 0;
        ddlVehicleNumber.Items.Clear();
        ddlVehicleNumber.Items.Insert(0, new ListItem("--Select--", "0"));
        txtReasonforDown.Text = "";
        txtExpDateOfRec.Text = "";
        txtDownTime.Text = "";
        txtOdo.Text = "";
        ClearDropdown();
        btnSubmit.Enabled = true;
        lblLatitude.Visible = false;
        lblLongitude.Visible = false;
        lblMandatory1.Visible = false;
        lblMandatory2.Visible = false;
        txtLatitude.Visible = false;
        txtLongitude.Visible = false;
        txtLatitude.Text = "";
        txtLongitude.Text = "";
        lblpvODO.Text = "";
    }

    protected void ddlVehicleNumber_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            _vehallobj.OffRoadVehicleNo = ddlVehicleNumber.SelectedValue;
            var dsoffrdvehdt = _vehallobj.GetOffRoadVehData();
            txtReasonforDown.Text = dsoffrdvehdt.Tables[0].Rows[0]["ReasonForOffRoad"].ToString();
            txtDownTime.Text = dsoffrdvehdt.Tables[0].Rows[0]["OffRoadDate"].ToString();
            txtExpDateOfRec.Text = dsoffrdvehdt.Tables[0].Rows[0]["ExpDateOfRecovery"].ToString();
            txtContactNumber.Text = dsoffrdvehdt.Tables[0].Rows[0]["vi_VehicleContact"].ToString();
            ViewState["OdoReading"] = dsoffrdvehdt.Tables[0].Rows[0]["Odometer"].ToString();
            lblpvODO.Text = ViewState["OdoReading"].ToString();
            _vehallobj.DistrictId = Convert.ToInt32(ddlDistrict.SelectedItem.Value);
        }
        catch (Exception ex)
        {
            ErrorHandler.ErrorsEntry(ex.GetBaseException().ToString(), "class: FMSVehicleAllocation;Method: Vehicle Selected Index Changed()", 0);
        }
    }

    private void MakeVehicleonRoad(string vehicleNumber, string onroaddate, string statusChangeBy, string odoMeter, string informerName, string piliotName, string piliotGid)
    {
        try
        {
            var conn = new MySqlConnection(ConfigurationManager.AppSettings["MySqlConn"]);
            conn.Open();
            var cmd = new MySqlCommand();
            var adp = new MySqlDataAdapter(cmd);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "UpdateVehicleStatusOnroad";
            cmd.Parameters.AddWithValue("VehicleNumber", vehicleNumber);
            cmd.Parameters.AddWithValue("onroaddate", onroaddate);
            cmd.Parameters.AddWithValue("Status_change_by", statusChangeBy);
            cmd.Parameters.AddWithValue("odo_meter", odoMeter);
            cmd.Parameters.AddWithValue("informer_name", informerName);
            cmd.Parameters.AddWithValue("piliot_name", piliotName);
            cmd.Parameters.AddWithValue("piliot_gid", piliotGid);
            cmd.Connection = conn;
            var ds = new DataSet();
            adp.Fill(ds);
            conn.Close();
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    protected void chkbxinactivesegment_CheckedChanged(object sender, EventArgs e)
    {
        ClearDropdown();
    }

    private void ClearDropdown()
    {
        txtContactNumber.Text = "";
    }
}