using System;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using GvkFMSAPP.BLL;
using GvkFMSAPP.PL;

public partial class VehicleAccidentDetails : Page
{
    private readonly FMSGeneral _fmsg = new FMSGeneral();
    private readonly Helper _helper = new Helper();
    private readonly VehicleAccidentDetailsBLL _vehicleAccidentDetail = new VehicleAccidentDetailsBLL();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Name"] == null) Response.Redirect("Login.aspx");
        if (!IsPostBack)
        {
            var dsPerms = (DataSet) Session["PermissionsDS"];
            if (dsPerms == null) throw new ArgumentNullException(nameof(dsPerms));
            dsPerms.Tables[0].DefaultView.RowFilter = "Url='" + Page.Request.Url.Segments[Page.Request.Url.Segments.Length - 1] + "'";
            var p = new PagePermissions(dsPerms, dsPerms.Tables[0].DefaultView[0]["Url"].ToString(), dsPerms.Tables[0].DefaultView[0]["Title"].ToString());
            btnSave.Attributes.Add("onclick", "return validation(this,'" + btnSave.ID + "')");
            GetVehicleNumber();
            FillHour();
            FillSecond();
            pnlVehicleAccidentDetails.Visible = false;
            grdVehicleAccidentDetails.Visible = false;
            if (p.Add) pnlVehicleAccidentDetails.Visible = true;
        }
    }

    public void GetVehicleNumber()
    {
        try
        {
            if (Session["UserdistrictId"] != null) _fmsg.UserDistrictId = Convert.ToInt32(Session["UserdistrictId"].ToString());
            var ds = _fmsg.GetVehicleNumberAccident();
            if (ds != null) _helper.FillDropDownHelperMethodWithDataSet(ds, "VehicleNumber", "VehicleID", null, ddlistVehicleNumber);
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    protected void FillHour()
    {
        ddlistHour.Items.Clear();
        ddlistInitiatedHr.Items.Clear();
        for (var i = 0; i <= 23; i++)
        {
            ddlistHour.Items.Add(new ListItem(i.ToString(), i.ToString()));
            ddlistInitiatedHr.Items.Add(new ListItem(i.ToString(), i.ToString()));
        }

        ddlistHour.Items.Insert(0, new ListItem("--hh--", "0"));
        ddlistInitiatedHr.Items.Insert(0, new ListItem("--hh--", "0"));
    }

    protected void FillSecond()
    {
        ddlistMinute.Items.Clear();
        ddlistInitiatedTimeMin.Items.Clear();
        for (var i = 0; i <= 59; i++)
        {
            ddlistMinute.Items.Add(new ListItem(i.ToString(), i.ToString()));
            ddlistInitiatedTimeMin.Items.Add(new ListItem(i.ToString(), i.ToString()));
        }

        ddlistMinute.Items.Insert(0, new ListItem("--mm--", "0"));
        ddlistInitiatedTimeMin.Items.Insert(0, new ListItem("--mm--", "0"));
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (rdBtnIsInsuranceClaimed.SelectedValue == "")
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "tmp", "<script type='text/javascript'>alert(\"Please select your option for radio button list.\");</script>", false);
                return;
            }

            if (txtAccidentDateTime.Text == "") txtAccidentDateTime.Text = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            if (txtInitiatedTime.Text == "") txtInitiatedTime.Text = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            if (txtAgeofVehicle.Text == "") txtAgeofVehicle.Text = "0";
            _vehicleAccidentDetail.VehicleID = int.Parse(ddlistVehicleNumber.SelectedValue);
            _vehicleAccidentDetail.IncidentTitle = txtIncidentTitle.Text;
            _vehicleAccidentDetail.AgeOfTheVehicle = float.Parse(txtAgeofVehicle.Text);
            _vehicleAccidentDetail.KilometerRun = txtKilometerRun.Text;
            _vehicleAccidentDetail.IncidentHandledBy = txtIncidentHandledBy.Text;
            _vehicleAccidentDetail.AccidentDescription = txtAccidentDescription.Text;
            _vehicleAccidentDetail.AccidentDateTime = DateTime.Parse(txtAccidentDateTime.Text);
            _vehicleAccidentDetail.AccidentTimeHrs = ddlistHour.SelectedItem.Value;
            _vehicleAccidentDetail.AccidentTimeMinutes = ddlistMinute.SelectedItem.Value;
            _vehicleAccidentDetail.ActionInitiatedBy = txtActionInitiatedBy.Text;
            _vehicleAccidentDetail.InitiatedTime = DateTime.Parse(txtInitiatedTime.Text);
            _vehicleAccidentDetail.InitiatedTimeHrs = ddlistInitiatedHr.SelectedItem.Value;
            _vehicleAccidentDetail.InitiatedTimeMinutes = ddlistInitiatedTimeMin.SelectedItem.Value;
            _vehicleAccidentDetail.InitialContainmentAction = txtInitialContainmentAction.Text;
            _vehicleAccidentDetail.AccidentRootCause = txtAccidentRootCause.Text;
            _vehicleAccidentDetail.DamageToAmbulance = txtDamagetoAmbulance.Text;
            _vehicleAccidentDetail.Damageto3rdPartyProperty = txtDamageto3rdPartyProperty.Text;
            _vehicleAccidentDetail.PilotName = txtPilotName.Text;
            _vehicleAccidentDetail.DrivingLicenseNumber = txtDrivingLicenseNumber.Text;
            _vehicleAccidentDetail.ExpiryDate = DateTime.Parse(txtExpiryDate.Text);
            _vehicleAccidentDetail.EMTName = txtEmtName.Text;
            _vehicleAccidentDetail.IsVehicleOperational = true;
            _vehicleAccidentDetail.InjuriesToEmriStaff = txtInjuriestoEMRIStaff.Text;
            _vehicleAccidentDetail.Injuryto3rdPartyPersonal = txt3rdPartyPersonal.Text;
            _vehicleAccidentDetail.ApproxRepairCost = txtApproxRepairCost.Text;
            _vehicleAccidentDetail.AreaPoliceStation = txtAreaPoliceStation.Text;
            _vehicleAccidentDetail.CDFIRPanchNama = txtFirPanchname.Text;
            _vehicleAccidentDetail.ReportedBy = txtReportedBY.Text;
            _vehicleAccidentDetail.Remarks = txtRemarks.Text;
            _vehicleAccidentDetail.IsInsuranceClaimRequired = Convert.ToBoolean(rdBtnIsInsuranceClaimed.SelectedItem.Value);
            var iReturn = _vehicleAccidentDetail.InsertVehicleAccidentDetails();
            if (iReturn != 0)
            {
                Show("Record Inserted Successfully");
                ClearControls();
            }
            else
            {
                Show("Error");
            }
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        ClearControls();
    }

    public void Show(string message)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('" + message + "');", true);
    }

    private void ClearControls()
    {
        txt3rdPartyPersonal.Text = "";
        txtAccidentDateTime.Text = "";
        txtAccidentDescription.Text = "";
        txtAccidentRootCause.Text = "";
        txtActionInitiatedBy.Text = "";
        txtAgeofVehicle.Text = "";
        txtApproxRepairCost.Text = "";
        txtAreaPoliceStation.Text = "";
        txtDamageto3rdPartyProperty.Text = "";
        txtDamagetoAmbulance.Text = "";
        txtDrivingLicenseNumber.Text = "";
        txtExpiryDate.Text = "";
        txtFirPanchname.Text = "";
        txtIncidentHandledBy.Text = "";
        txtIncidentTitle.Text = "";
        txtInitialContainmentAction.Text = "";
        txtInitiatedTime.Text = "";
        txtInjuriestoEMRIStaff.Text = "";
        txtKilometerRun.Text = "";
        txtRemarks.Text = "";
        txtReportedBY.Text = "";
        txtEmtName.Text = "";
        ddlistHour.SelectedIndex = 0;
        ddlistInitiatedHr.SelectedIndex = 0;
        ddlistInitiatedTimeMin.SelectedIndex = 0;
        ddlistMinute.SelectedIndex = 0;
        ddlistVehicleNumber.SelectedIndex = 0;
        txtPilotName.Text = "";
    }

    public void FindAllTextBox(Control ctrl)
    {
        if (ctrl != null)
            foreach (Control c in ctrl.Controls)
            {
                var box = c as TextBox;
                if (box != null) box.Text = "";
                FindAllTextBox(c);
            }
    }

    protected void grdVehicleAccidentDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdVehicleAccidentDetails.PageIndex = e.NewPageIndex;
    }

    protected void ddlistVehicleNumber_SelectedIndexChanged(object sender, EventArgs e)
    {
        switch (ddlistVehicleNumber.SelectedIndex)
        {
            case 0:
                txtAgeofVehicle.Text = "";
                txtKilometerRun.Text = "";
                txtPilotName.Text = "";
                txtEmtName.Text = "";
                break;
            default:
                var dtManufacturer = _fmsg.GetManufactureDate(Convert.ToInt32(ddlistVehicleNumber.SelectedItem.Value));
                txtAgeofVehicle.Text = dtManufacturer.Tables[0].Rows[0]["differenc"].ToString();
                txtKilometerRun.Text = dtManufacturer.Tables[1].Rows[0]["ODO"].ToString();
                var dtNames = _fmsg.GetNames(ddlistVehicleNumber.SelectedItem.Text);
                txtPilotName.Text = dtNames.Tables[0].Rows[0]["PilotName"].ToString();
                txtEmtName.Text = dtNames.Tables[0].Rows[0]["EMTName"].ToString();
                break;
        }
    }
}