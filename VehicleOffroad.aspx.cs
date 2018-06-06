using System;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using GvkFMSAPP.BLL;
using GvkFMSAPP.BLL.VAS_BLL;
using MySql.Data.MySqlClient;

public partial class VehicleOffroad : Page
{
    private readonly FMSGeneral _fmsgeneral = new FMSGeneral();
    private readonly BaseVehicleDetails _fmsobj = new BaseVehicleDetails();
    private readonly Helper _helper = new Helper();
    private readonly VASGeneral _vehallobj = new VASGeneral();
    private DataTable _dtBreakdown = new DataTable();
    public IInventory ObjInventory = new FMSInventory();

    public string UserId { get; private set; }

    protected void Page_PreInit(object sender, EventArgs e)
    {
        if (Session["Role_Id"] == null)
            Response.Redirect("Login.aspx");
        else
            switch (Session["Role_Id"].ToString())
            {
                case "120":
                    MasterPageFile = "~/MasterERO.master";
                    break;
            }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        txtAllEstimatedCost.Attributes.Add("onKeyPress", "javascript: return Integersonly(event);");
        txtEMEId.Attributes.Add("onKeyPress", "javascript: return Integersonly(event);");
        txtPilotId.Attributes.Add("onKeyPress", "javascript: return Integersonly(event);");
        if (Session["User_Id"] == null)
            Response.Redirect("Login.aspx");
        else
            UserId = (string)Session["User_Id"];
        if (!IsPostBack)
        {
            divAggre.Visible = false;
            btnSubmit.Attributes.Add("onclick", "return validation()");
            GetDistrict();
            FillHoursandMins();
            GetTime();
            FillMaintenanceTypes();
            _dtBreakdown = null;
            btnSubmit.Enabled = true;
        }
    }

    public void FillMaintenanceTypes()
    {
        try
        {
            var dset = _fmsgeneral.GetMaintenanceType();
            if (dset == null) return;
            _helper.FillDropDownHelperMethodWithDataSet(dset, "Maint_Type_ID", "Maint_Desc", ddlreasons);
            ddlreasons.Items.Insert(12, new ListItem("RESOURCE SHORTAGE", "13"));
            ddlreasons.Items.Insert(13, new ListItem("NO FUEL", "14"));
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    protected void GetTime()
    {
        var timenow = DateTime.Now.ToString(CultureInfo.InvariantCulture).Split(' ');
        txtOfftimeDate.Text = timenow[0];
        var hour = Convert.ToInt32(timenow[1].Split(':')[0]);
        if (timenow.Length <= 2)
        {
            if (hour == 12)
                hour = 0;
            else if (hour > 12) hour = hour - 12;
            hour = hour + 12;
            ddlOFFHour.Items.FindByValue(hour.ToString()).Selected = true;
        }
        else
        {
            switch (timenow[2])
            {
                case "PM":
                    switch (hour)
                    {
                        case 12:
                            ddlOFFHour.Items.FindByValue(hour.ToString()).Selected = true;
                            break;
                        default:
                            hour = hour + 12;
                            ddlOFFHour.Items.FindByValue(hour.ToString()).Selected = true;
                            break;
                    }

                    break;
                default:
                    if (hour < 10)
                        ddlOFFHour.Items.FindByValue("0" + timenow[1].Split(':')[0]).Selected = true;
                    else
                        ddlOFFHour.Items.FindByValue(timenow[1].Split(':')[0]).Selected = true;
                    break;
            }
        }

        ddlOFFMin.Items.FindByValue(timenow[1].Split(':')[1]).Selected = true;
    }

    public void Insertdata(string smsContent, string name, string mobileno)
    {
    }

    public void InsertAgent(string offroadid, string vehicleNo, string agentId)
    {
        try
        {
            var ip = GetLocalIpAddress();
            var query = "insert into t_offroadAgent(offroadid ,vehicleNo, AgentID,AgentName,ip) values ('" + offroadid + "', '" + vehicleNo + "', '" + agentId + "', '" + Session["User_Name"] + "','" + ip + "')";
            _helper.ExecuteInsertStatement(query);
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    public void SendSms(string vehicleno, string breakdownid, string reason)
    {
        try
        {
            var query = "select * from m_vehicle_supervisors where VehicleNo = '" + vehicleno + "'";
            _helper.ExecuteSelectStmt(query);
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        btnSubmit.Enabled = false;
        var entrydate = DateTime.ParseExact(txtOfftimeDate.Text + " " + ddlOFFHour.SelectedValue + ":" + ddlOFFMin.SelectedValue, "MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture);
        if (entrydate > DateTime.Now)
        {
            Show("Off road date should not be greater than current date ");
            return;
        }

        //Declarations
        string segmentids = "", segmentnames = "", mandalids = "";
        try
        {
            var objStringBuilder = new StringBuilder();
            objStringBuilder.Append("<NewDataSet>");
            objStringBuilder.Append("<TransDtls>");
            _vehallobj.ContactNumber = txtContactNumber.Text;
            _vehallobj.Comments = txtComment.Text;
            _vehallobj.District = ddlDistrict.SelectedItem.Text;
            _vehallobj.OffRoadDate = Convert.ToDateTime(txtOfftimeDate.Text + " " + ddlOFFHour.SelectedItem.Text + ":" + ddlOFFMin.SelectedItem.Text);
            _vehallobj.OffRoadVehicleNo = ddlVehicleNumber.SelectedItem.Text;
            _vehallobj.ReasonForOffRoad = ddlreasons.SelectedItem.Text;
            _vehallobj.RequestedBy = txtReqBy.Text;
            _vehallobj.EMEID = txtEMEId.Text;
            _vehallobj.PilotID = txtPilotId.Text;
            _vehallobj.PilotName = txtPilotName.Text;
            _vehallobj.Odometer = txtOdo.Text;
            _vehallobj.ExpDateOfRecovery = Convert.ToDateTime(txtExpDateOfRec.Text + " " + ddlExpDateOfRecHr.SelectedItem.Text + ":" + ddlExpDateOfRecMin.SelectedItem.Text);
            _vehallobj.SegmentId = 0;
            _vehallobj.totEstimated = txtAllEstimatedCost.Text;
            if (ddlreasons.SelectedItem != null && ddlreasons.SelectedIndex == 4)
            {
                for (var intCount = 0; intCount < grdvwBreakdownDetails.Rows.Count; intCount++)
                {
                    objStringBuilder.Append("<BreakdownDetails>");
                    objStringBuilder.Append("<Aggregates>" + Convert.ToString(grdvwBreakdownDetails.Rows[intCount].Cells[1].Text) + "</Aggregates>");
                    objStringBuilder.Append("<Categories>" + Convert.ToString(grdvwBreakdownDetails.Rows[intCount].Cells[2].Text) + "</Categories>");
                    objStringBuilder.Append("<Subcategories>" + Convert.ToString(grdvwBreakdownDetails.Rows[intCount].Cells[3].Text) + "</Subcategories>");
                    objStringBuilder.Append("<EstimatedCost>" + Convert.ToString(grdvwBreakdownDetails.Rows[intCount].Cells[4].Text) + "</EstimatedCost>");
                    objStringBuilder.Append("</BreakdownDetails> ");
                }
            }
            else
            {
                objStringBuilder.Append("<BreakdownDetails>");
                objStringBuilder.Append("<Aggregates></Aggregates>");
                objStringBuilder.Append("<Categories></Categories>");
                objStringBuilder.Append("<Subcategories></Subcategories>");
                objStringBuilder.Append("<EstimatedCost>0</EstimatedCost>");
                objStringBuilder.Append("</BreakdownDetails>");
            }

            objStringBuilder.Append("</TransDtls> ");
            objStringBuilder.Append("</NewDataSet>");
            _vehallobj.BreakDownDetails = objStringBuilder.ToString();
            DataSet dsmandalsupd;
            if (pnlothersegment.Visible && grdvothersegment.Visible)
            {
                dsmandalsupd = (DataSet) Session["dsmandals"];
                for (var j = 0; j < dsmandalsupd.Tables[0].Rows.Count; j++)
                {
                    mandalids = mandalids + dsmandalsupd.Tables[0].Rows[j][0] + ",";
                    segmentids = segmentids + ((DropDownList) grdvothersegment.Rows[j].Controls[1].Controls[1]).SelectedValue + ",";
                    segmentnames = segmentnames + ((DropDownList) grdvothersegment.Rows[j].Controls[1].Controls[1]).SelectedItem.Text + ",";
                }

                _vehallobj.SegmentIds = segmentids;
                _vehallobj.MandalIds = mandalids;
                _vehallobj.SegmentNames = segmentnames;
                MakeVehicleoffRoad(ddlVehicleNumber.SelectedItem.Text, Convert.ToDateTime(txtOfftimeDate.Text + " " + ddlOFFHour.SelectedItem.Text + ":" + ddlOFFMin.SelectedItem.Text).ToString("yyyy-MM-dd HH:mm:ss"), ddlreasons.SelectedItem.Text, Session["User_Name"].ToString(), txtOdo.Text, txtReqBy.Text, txtPilotName.Text, txtPilotId.Text);
                var insres = _vehallobj.InsertOffRoadVehicleDetail();
                switch (insres)
                {
                    case 0:
                        Show("Record not Inserted Successfully!!");
                        break;
                    default:
                        InsertAgent(insres.ToString(), ddlVehicleNumber.SelectedItem.Text, Session["User_Id"].ToString());
                        SendSms(ddlVehicleNumber.SelectedItem.Text, insres.ToString(), ddlreasons.SelectedItem.Text);
                        Show("Record Inserted Successfully!! And BreakDown Id=" + insres);
                        break;
                }
            }
            else
            {
                if (pnlothervehicle.Visible && grdvothervehicle.Visible)
                {
                    dsmandalsupd = (DataSet) Session["dsmandals"];
                    for (var j = 0; j < dsmandalsupd.Tables[0].Rows.Count; j++)
                    {
                        mandalids = mandalids + dsmandalsupd.Tables[0].Rows[j][0] + ",";
                        segmentids = segmentids + Convert.ToInt32(Session["segmentid"]) + ",";
                        segmentnames = segmentnames + lblSegmentName.Text + ",";
                    }

                    _vehallobj.SegmentIds = segmentids;
                    _vehallobj.MandalIds = mandalids;
                    _vehallobj.SegmentNames = segmentnames;
                    _vehallobj.OtherVehicleNumber = ddlothervehicle.SelectedItem.Text;
                    MakeVehicleoffRoad(ddlVehicleNumber.SelectedItem.Text, Convert.ToDateTime(txtOfftimeDate.Text + " " + ddlOFFHour.SelectedItem.Text + ":" + ddlOFFMin.SelectedItem.Text).ToString("yyyy-MM-dd HH:mm:ss"), ddlreasons.SelectedItem.Text, Session["User_Name"].ToString(), txtOdo.Text, txtReqBy.Text, txtPilotName.Text, txtPilotId.Text);
                    var insres = _vehallobj.InsertOtherOffRoadVehicleDetail();
                    Session["offId"] = insres;
                    switch (insres)
                    {
                        case 0:
                            Show("Record not Inserted Successfully!!");
                            break;
                        default:
                            InsertAgent(insres.ToString(), ddlVehicleNumber.SelectedItem.Text, Session["User_Id"].ToString());
                            SendSms(ddlVehicleNumber.SelectedItem.Text, insres.ToString(), ddlreasons.SelectedItem.Text);
                            Show("Record Inserted Successfully!! And Breakdown Id=" + insres);
                            break;
                    }
                }
                else
                {
                    mandalids = "";
                    segmentids = "";
                    segmentnames = "";
                    _vehallobj.SegmentIds = segmentids;
                    _vehallobj.MandalIds = mandalids;
                    _vehallobj.SegmentNames = segmentnames;
                    MakeVehicleoffRoad(ddlVehicleNumber.SelectedItem.Text, Convert.ToDateTime(txtOfftimeDate.Text + " " + ddlOFFHour.SelectedItem.Text + ":" + ddlOFFMin.SelectedItem.Text).ToString("yyyy-MM-dd HH:mm:ss"), ddlreasons.SelectedItem.Text, Session["User_Name"].ToString(), txtOdo.Text, txtReqBy.Text, txtPilotName.Text, txtPilotId.Text);
                    var insres = _vehallobj.InsertOffRoadVehicleDetail();
                    Session["offId"] = insres;
                    switch (insres)
                    {
                        case 0:
                            Show("Record not Inserted Successfully!!");
                            break;
                        default:
                            InsertAgent(insres.ToString(), ddlVehicleNumber.SelectedItem.Text, Session["User_Id"].ToString());
                            SendSms(ddlVehicleNumber.SelectedItem.Text, insres.ToString(), ddlreasons.SelectedItem.Text);
                            Show("Record Inserted Successfully!!And Breakdown Id=" + insres);
                            break;
                    }
                }
            }

            ClearControls();
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    public static string GetLocalIpAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
            switch (ip.AddressFamily)
            {
                case AddressFamily.InterNetwork:
                    return ip.ToString();
            }
        throw new Exception("Local IP Address Not Found!");
    }

    private void MakeVehicleoffRoad(string vehicleNumber, string offroaddate, string offroadtype, string statusChangeBy, string odoMeter, string informerName, string piliotName, string piliotGid)
    {
        try
        {
            var conn = new MySqlConnection(ConfigurationManager.AppSettings["MySqlConn"]);
            conn.Open();
            var cmd = new MySqlCommand {CommandType = CommandType.StoredProcedure, CommandText = "UpdateVehicleStatus", Connection = conn};
            cmd.Parameters.AddWithValue("VehicleNumber", vehicleNumber);
            cmd.Parameters.AddWithValue("offroaddate", offroaddate);
            cmd.Parameters.AddWithValue("offroadtype", offroadtype);
            cmd.Parameters.AddWithValue("Status_change_by", statusChangeBy);
            cmd.Parameters.AddWithValue("odo_meter", odoMeter);
            cmd.Parameters.AddWithValue("informer_name", informerName);
            cmd.Parameters.AddWithValue("piliot_name", piliotName);
            cmd.Parameters.AddWithValue("piliot_gid", piliotGid);
            var adp = new MySqlDataAdapter(cmd);
            var ds = new DataSet();
            adp.Fill(ds);
            conn.Close();
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
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

    public void Show(string message)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('" + message + "');", true);
    }

    public void ClearControls()
    {
        //txtOfftimeDate.Text = "";
        txtContactNumber.Text = "";
        ddlDistrict.SelectedIndex = 0;
        ddlVehicleNumber.SelectedIndex = 0;
        ddlreasons.SelectedIndex = 0;
        txtReqBy.Text = "";
        txtEMEId.Text = "";
        txtPilotId.Text = "";
        txtPilotName.Text = "";
        txtOdo.Text = "";
        txtComment.Text = "";
        txtExpDateOfRec.Text = "";
        txtAllEstimatedCost.Text = "";
        switch (ddlreasons.SelectedValue)
        {
            case "BREAKDOWN":
                ddlAggregates.SelectedIndex = 0;
                ddlCategories.SelectedIndex = 0;
                ddlSubCategories.SelectedIndex = 0;
                break;
        }

        ddlExpDateOfRecHr.SelectedIndex = 0;
        ddlExpDateOfRecMin.SelectedIndex = 0;
        pnlRadioBtnList.Visible = false;
        pnlothersegment.Visible = false;
        pnlothervehicle.Visible = false;
        lblmsg.Visible = false;
        lblSegment.Visible = false;
        lblSegmentName.Visible = false;
        divAggre.Visible = false;
        Session["segmentid"] = "";
        Session["locationid"] = "";
        Session["dsmandals"] = "";
        Session["dsvehilce"] = "";
        Session["dssegment"] = "";
        grdvwBreakdownDetails.DataSource = null;
        grdvwBreakdownDetails.DataBind();
        btnSubmit.Enabled = true;
    }

    private void FillHoursandMins()
    {
        int i;
        for (i = 0; i < 24; i++)
            if (i >= 10)
            {
                ddlOFFHour.Items.Add(new ListItem(i.ToString(), i.ToString()));
                ddlExpDateOfRecHr.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }
            else
            {
                ddlOFFHour.Items.Add(new ListItem("0" + i, "0" + i));
                ddlExpDateOfRecHr.Items.Add(new ListItem("0" + i, "0" + i));
            }

        for (i = 0; i < 60; i++)
            if (i >= 10)
            {
                ddlOFFMin.Items.Add(new ListItem(i.ToString(), i.ToString()));
                ddlExpDateOfRecMin.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }
            else
            {
                ddlOFFMin.Items.Add(new ListItem("0" + i, "0" + i));
                ddlExpDateOfRecMin.Items.Add(new ListItem("0" + i, "0" + i));
            }
    }

    protected void ddlDistrict_SelectedIndexChanged(object sender, EventArgs e)
    {
        switch (ddlDistrict.SelectedIndex)
        {
            case 0:
                ddlVehicleNumber.SelectedIndex = 0;
                ddlVehicleNumber_SelectedIndexChanged(this, null);
                break;
            default:
                _vehallobj.DistrictId = int.Parse(ddlDistrict.SelectedItem.Value);
                var ds = _vehallobj.GetActiveVehiclesForOffRoad_new();
                _helper.FillDropDownHelperMethodWithDataSet(ds, "Vehicle", "VehicleNumber", ddlVehicleNumber);
                ddlVehicleNumber.Items[0].Value = "0";
                ddlVehicleNumber.SelectedIndex = 0;
                Session["dsvehilce"] = ds;
                lblSegmentName.Text = "";
                txtContactNumber.Text = "";
                ddlreasons.SelectedIndex = 0;
                ddlreasons_SelectedIndexChanged(this, null);
                grdvwBreakdownDetails.DataSource = null;
                grdvwBreakdownDetails.DataBind();
                break;
        }
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        ClearControls();
    }

    protected void ddlVehicleNumber_SelectedIndexChanged(object sender, EventArgs e)
    {
        switch (ddlVehicleNumber.SelectedIndex)
        {
            case 0:
                ddlreasons.SelectedIndex = 0;
                ddlreasons_SelectedIndexChanged(this, null);
                break;
            default:
                ddlreasons.SelectedIndex = 0;
                ddlreasons_SelectedIndexChanged(this, null);
                txtContactNumber.Text = "";
                var ds = ObjInventory.GetVehicleContactNumber(ddlVehicleNumber.SelectedItem.Text, ConfigurationManager.AppSettings["StrCTIAPPS"]);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows) txtContactNumber.Text = dr["vi_VehicleContact"].ToString();
                    foreach (DataRow dr in ds.Tables[1].Rows) txtOdo.Text = dr["Odometer"].ToString();
                }

                break;
        }

        lblmsg.Visible = false;
    }

    protected void rdbtnlstOption_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdbtnlstOption != null)
            switch (rdbtnlstOption.SelectedValue)
            {
                case "rdbothersegment":
                    pnlothersegment.Visible = true;
                    pnlothervehicle.Visible = false;
                    break;
                default:
                    if (ddlothervehicle.Items.Count > 0) ddlothervehicle.Items.Clear();
                    DataSet dsvehicler;
                    dsvehicler = (DataSet) Session["dsvehilce"];
                    var dv = new DataView(dsvehicler.Tables[0], "Vehicle <>'" + ddlVehicleNumber.SelectedValue + "'", "Sg_SName", DataViewRowState.CurrentRows);
                    ddlothervehicle.DataSource = dv;
                    ddlothervehicle.DataTextField = "VehicleNumber";
                    ddlothervehicle.DataValueField = "Vehicle";
                    ddlothervehicle.DataBind();
                    ddlothervehicle.Items.Insert(0, "--Select--");
                    txtotherbaselocation.Text = "";
                    txtothercontactno.Text = "";
                    lblOtherVehSegment.Visible = false;
                    lblOtherVehSegmentName.Visible = false;
                    rdbtnlstOption.SelectedValue = "rdbothervehicle";
                    grdvothervehicle.Visible = false;
                    pnlothervehicle.Visible = true;
                    pnlothersegment.Visible = false;
                    break;
            }
    }

    protected void grdvothersegment_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            _vehallobj.DistrictId = Convert.ToInt32(ddlDistrict.SelectedItem.Value);
            var dssegmentgot = _vehallobj.GetMappedSegments();
            if (dssegmentgot == null) throw new ArgumentNullException(nameof(dssegmentgot));
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    var ddl = (DropDownList) e.Row.FindControl("DropDownList1");
                    _helper.FillDropDownHelperMethodWithDataSet(dssegmentgot, "Sg_SName", "Sg_Segid", ddl);
                    break;
            }
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    protected void grdvothervehicle_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        var dsvehiclevgvo = (DataSet) Session["dsvehilce"];
        if (dsvehiclevgvo == null) throw new ArgumentNullException(nameof(dsvehiclevgvo));
        var dssegmentgvo = (DataSet) Session["dssegment"];
        if (dssegmentgvo == null) throw new ArgumentNullException(nameof(dssegmentgvo));
        using (var dv1 = new DataView(dsvehiclevgvo.Tables[0], "Vehicle ='" + ddlothervehicle.SelectedValue + "'", "Sg_SName", DataViewRowState.CurrentRows))
        {
            Session["othersegmentid"] = Convert.ToInt32(dv1[0][3]);
        }

        using (var dv = new DataView(dssegmentgvo.Tables[0], "Sg_Segid <>" + Convert.ToInt32(Session["othersegmentid"]), "Sg_SName", DataViewRowState.CurrentRows))
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    var ddl = (DropDownList) e.Row.FindControl("DropDownList2");
                    ddl.DataSource = dv;
                    ddl.DataTextField = "Sg_SName";
                    ddl.DataValueField = "Sg_Segid";
                    ddl.DataBind();
                    break;
            }
        }
    }

    protected void ddlothervehicle_SelectedIndexChanged(object sender, EventArgs e)
    {
        var dsvehiclevov = (DataSet) Session["dsvehilce"];
        if (dsvehiclevov == null) throw new ArgumentNullException(nameof(dsvehiclevov));
        using (var dv = new DataView(dsvehiclevov.Tables[0], "Vehicle ='" + ddlothervehicle.SelectedValue + "'", "Sg_SName", DataViewRowState.CurrentRows))
        {
            Session["othersegmentid"] = Convert.ToInt32(dv[0][3]);
            lblOtherVehSegmentName.Text = dv[0][2].ToString();
            lblOtherVehSegmentName.Visible = true;
            lblOtherVehSegment.Visible = true;
            txtotherbaselocation.Text = Convert.ToString(dv[0][5]);
            txtothercontactno.Text = Convert.ToString(dv[0][6]);
            Session["locationid"] = Convert.ToInt32(dv[0][4]);
        }

        _vehallobj.DistrictId = Convert.ToInt32(ddlDistrict.SelectedItem.Value);
        _vehallobj.SegmentId = Convert.ToInt32(Session["othersegmentid"]);
        var ds = _vehallobj.GetMandals();
        if (ds == null) throw new ArgumentNullException(nameof(ds));
        if (ds.Tables[0].Rows.Count <= 0)
        {
            grdvothervehicle.Visible = false;
        }
        else
        {
            pnlothervehicle.Visible = true;
            grdvothervehicle.Visible = true;
            grdvothervehicle.DataSource = ds.Tables[0];
            grdvothervehicle.DataBind();
            Session["dsmandals"] = ds;
        }

        var dvvehicleos = new DataView(dsvehiclevov.Tables[0], "SegmentId ='" + Convert.ToInt32(Session["othersegmentid"]) + "'", "Sg_SName", DataViewRowState.CurrentRows);
        if (dvvehicleos.Count > 1) grdvothervehicle.Visible = false;
    }

    public void GetAggregates()
    {
        _vehallobj.Aggregates = ddlAggregates.SelectedValue;
        _vehallobj.VehicleNumber = ddlVehicleNumber.SelectedItem.Text;
        var ds = _vehallobj.GetAggregatesOffRoad();
        if (ds == null) throw new ArgumentNullException(nameof(ds));
        try
        {
            _helper.FillDropDownHelperMethodWithDataSet(ds, "Aggregate_Id", "Aggregates", ddlAggregates);
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    protected void ddlreasons_SelectedIndexChanged(object sender, EventArgs e)
    {
        switch (ddlreasons.SelectedIndex)
        {
            case 4:
                txtAllEstimatedCost.Enabled = false;
                divAggre.Visible = true;
                ddlCategories.Enabled = false;
                ddlSubCategories.Enabled = false;
                GetAggregates();
                break;
            default:
                txtAllEstimatedCost.Text = "";
                txtAllEstimatedCost.Enabled = true;
                divAggre.Visible = false;
                grdvwBreakdownDetails.DataSource = null;
                grdvwBreakdownDetails.DataBind();
                _dtBreakdown = null;
                break;
        }
    }

    public void GetCategories()
    {
        _vehallobj.Aggregates2 = Convert.ToInt16(ddlAggregates.SelectedValue);
        _vehallobj.VehicleNumber = ddlVehicleNumber.SelectedItem.Text;
        var ds = _vehallobj.GetCategories();
        if (ds == null) throw new ArgumentNullException(nameof(ds));
        try
        {
            _helper.FillDropDownHelperMethodWithDataSet(ds, "Category_Id", "Categories", ddlCategories);
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    protected void ddlAggregates_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtEstCost.Text = "";
        switch (ddlAggregates.SelectedIndex)
        {
            case 0:
                ddlCategories.SelectedIndex = 0;
                ddlSubCategories.SelectedIndex = 0;
                ddlCategories.Enabled = false;
                break;
            default:
                GetCategories();
                ddlCategories.Enabled = true;
                ddlSubCategories.Enabled = false;
                break;
        }
    }

    public void GetSubCategories()
    {
        try
        {
            _vehallobj.Categories2 = Convert.ToInt16(ddlCategories.SelectedValue);
            var ds = _vehallobj.GetSubcategories();
            if (ds == null) throw new ArgumentNullException(nameof(ds));
            _helper.FillDropDownHelperMethodWithDataSet(ds, "SubCategory_Id", "SubCategories", ddlSubCategories);
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    protected void ddlCategories_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtEstCost.Text = "";
        switch (ddlCategories.SelectedIndex)
        {
            case 0:
                ddlSubCategories.SelectedIndex = 0;
                ddlSubCategories.Enabled = false;
                break;
            default:
                ddlSubCategories.Enabled = true;
                GetSubCategories();
                break;
        }
    }

    protected void ddlSubCategories_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtEstCost.Text = "";
        switch (ddlSubCategories.SelectedIndex)
        {
            case 0:
                return;
        }

        _vehallobj.SubCategories2 = Convert.ToInt16(ddlSubCategories.SelectedValue);
        var text = _vehallobj.GetEstCost();
        txtEstCost.Text = text;
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        double sum = 0;
        var aggregates = ddlAggregates.SelectedItem.ToString();
        var categories = ddlCategories.SelectedItem.ToString();
        var subcategories = ddlSubCategories.SelectedItem.ToString();
        var estimatedCost = txtEstCost.Text;
        if (ViewState["dtBreakdown"] != null) _dtBreakdown = (DataTable) ViewState["dtBreakdown"];
        switch (_dtBreakdown.Columns.Count)
        {
            case 0:
                _dtBreakdown.Columns.Add("Aggregates", typeof(string));
                _dtBreakdown.Columns.Add("Categories", typeof(string));
                _dtBreakdown.Columns.Add("Subcategories", typeof(string));
                _dtBreakdown.Columns.Add("EstimatedCost", typeof(string));
                break;
        }

        _dtBreakdown.Rows.Add(aggregates, categories, subcategories, estimatedCost);
        ViewState["dtBreakdown"] = _dtBreakdown;
        grdvwBreakdownDetails.DataSource = _dtBreakdown;
        grdvwBreakdownDetails.DataBind();
        foreach (GridViewRow item in grdvwBreakdownDetails.Rows) sum = Convert.ToDouble(item.Cells[4].Text) + sum;
        txtAllEstimatedCost.Text = sum.ToString(CultureInfo.InvariantCulture);
        ddlAggregates.SelectedIndex = 0;
        ddlCategories.SelectedIndex = 0;
        ddlSubCategories.SelectedIndex = 0;
        txtEstCost.Text = string.Empty;
        ddlCategories.Enabled = false;
        ddlSubCategories.Enabled = false;
    }

    protected void grdvwBreakdownDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        var sum = Convert.ToDouble(txtAllEstimatedCost.Text);
        var index = Convert.ToInt32(e.RowIndex);
        var dt = ViewState["dtBreakdown"] as DataTable;
        if (dt != null)
        {
            sum = sum - Convert.ToDouble(dt.Rows[index][3].ToString());
            dt.Rows[index].Delete();
            ViewState["dt"] = dt;
        }

        txtAllEstimatedCost.Text = sum.ToString(CultureInfo.InvariantCulture);
        BindGrid();
    }

    protected void BindGrid()
    {
        grdvwBreakdownDetails.DataSource = ViewState["dt"] as DataTable;
        grdvwBreakdownDetails.DataBind();
    }
}