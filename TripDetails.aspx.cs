using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using GvkFMSAPP.BLL;

public partial class TripDetails : Page
{
    private readonly FMSGeneral _fmsg = new FMSGeneral();
    private readonly Helper _helper = new Helper();
    public IFuelManagement ObjFuelEntry = new FuelManagement();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Name"] == null) Response.Redirect("Login.aspx");
        if (!IsPostBack)
        {
            if (Session["UserdistrictId"] != null) _fmsg.UserDistrictId = Convert.ToInt32(Session["UserdistrictId"].ToString());
            FillVehicles();
            FillTrips();
            FillHoursandMins();
            txtDestinationLocation.Attributes.Add("onkeypress", "javascript:return OnlyAlphabets(this,event)");
            txtRemarks.Attributes.Add("onkeypress", "javascript:return OnlyAlphabets(this,event)");
        }
    }

    private void FillTrips()
    {
        try
        {
            var ds = ObjFuelEntry.IGetTripTypes();
            if (ds == null) throw new ArgumentNullException(nameof(ds));
            _helper.FillDropDownHelperMethodWithDataSet(ds, "TripTypeDescription", "TripTypeID", ddlTripType);
            ddlTripType.Enabled = true;
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    private void FillVehicles()
    {
        try
        {
            var ds = _fmsg.GetVehicleNumber();
            if (ds == null) throw new ArgumentNullException(nameof(ds));
            _helper.FillDropDownHelperMethodWithDataSet(ds, "VehicleNumber", "VehicleID", null, ddlAmbulanceID);
            ddlAmbulanceID.Enabled = true;
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    private void FillHoursandMins()
    {
        int i;
        for (i = 0; i < 24; i++)
            if (i < 10)
            {
                ddlHours.Items.Add(new ListItem("0" + i, "0" + i));
                ddlHours1.Items.Add(new ListItem("0" + i, "0" + i));
            }
            else
            {
                ddlHours.Items.Add(new ListItem(i.ToString(), i.ToString()));
                ddlHours1.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }

        for (i = 0; i < 60; i++)
            if (i < 10)
            {
                ddlMinutes.Items.Add(new ListItem("0" + i, "0" + i));
                ddlMinutes2.Items.Add(new ListItem("0" + i, "0" + i));
            }
            else
            {
                ddlMinutes.Items.Add(new ListItem(i.ToString(), i.ToString()));
                ddlMinutes2.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        var tripDate = Convert.ToDateTime(txtTripDate.Text);
        var vehicle = Convert.ToInt32(ddlAmbulanceID.SelectedValue);
        var trip = Convert.ToInt32(ddlTripType.SelectedValue);
        var destination = Convert.ToString(txtDestinationLocation.Text);
        var staTim = ddlHours.SelectedItem.Text + ":" + ddlMinutes.SelectedItem.Text;
        var startTime = Convert.ToDateTime(staTim);
        var startOdo = Convert.ToInt32(txtStartOdo.Text);
        var endTim = ddlHours1.SelectedItem.Text + ":" + ddlMinutes2.SelectedItem.Text;
        var endTime = Convert.ToDateTime(endTim);
        var endOdo = Convert.ToInt32(txtEndOdo.Text);
        var remarks = Convert.ToString(txtRemarks.Text);
        var createdBy = Convert.ToInt32(Session["User_Id"].ToString());
        var res = ObjFuelEntry.IInsertTrips(tripDate, vehicle, trip, destination, startTime, startOdo, endTime, endOdo, remarks, createdBy);
        Show(res == 1 ? "Trip Entry Added Successfully" : "Failure,Please Try After sometime");
        ClearFields();
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        ClearFields();
    }

    private void ClearFields()
    {
        ddlHours.SelectedIndex = -1;
        ddlAmbulanceID.SelectedIndex = -1;
        ddlHours1.SelectedIndex = -1;
        ddlMinutes.SelectedIndex = -1;
        ddlMinutes2.SelectedIndex = -1;
        ddlTripType.SelectedIndex = -1;
        txtDestinationLocation.Text = "";
        txtEndOdo.Text = "";
        txtRemarks.Text = "";
        txtStartOdo.Text = "";
        txtTripDate.Text = "";
    }

    protected void lbtnViewHistory_Click(object sender, EventArgs e)
    {
    }

    public void Show(string message)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('" + message + "');", true);
    }

    protected void ddlAmbulanceID_SelectedIndexChanged(object sender, EventArgs e)
    {
        var dsOdo = ObjFuelEntry.IGetTripEntryOdo(Convert.ToInt32(ddlAmbulanceID.SelectedValue));
        maxOdo.Value = dsOdo.Tables[0].Rows.Count != 0 ? (dsOdo.Tables[0].Rows[0]["ODO"].ToString() != string.Empty ? dsOdo.Tables[0].Rows[0]["ODO"].ToString() : "0") : "0";
    }
}