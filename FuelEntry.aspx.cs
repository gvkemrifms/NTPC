using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using GvkFMSAPP.BLL;

public partial class FuelEntry : Page
{
    private readonly FMSGeneral _fmsg = new FMSGeneral();
    private readonly Helper _helper = new Helper();
    private string _bunkname;
    private bool _flag;
    private double _kmplInt;
    private double _mSkmplInt;
    public IFuelManagement ObjFuelEntry = new FuelManagement();

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
        if (Session["User_Name"] == null) Response.Redirect("Login.aspx");
        if (!IsPostBack)
        {
            if (Session["UserdistrictId"] != null) _fmsg.UserDistrictId = Convert.ToInt32(Session["UserdistrictId"].ToString());
            linkExisting.Visible = false;
            lnkNew.Visible = true;
            txtBunkName.Visible = true;
            txtBunkName.Enabled = false;
            ddlBunkName.Visible = false;
            FillVehicles();
            FillPayMode();
            //FillGridFuelEntry();
            txtAmount.Attributes.Add("onkeypress", "javascript:return numericOnly(event)");
            txtBillNumber.Attributes.Add("onkeypress", "javascript:return numeric_only(event)");
            txtLocation.Attributes.Add("onkeypress", "javascript:return OnlyAlphabets(this,event)");
            txtOdometer.Attributes.Add("onkeypress", "javascript:return numeric_only(event)");
            txtLocation.Attributes.Add("onkeypress", "javascript:return OnlyAlphabets(this,event)");
            txtPilotID.Attributes.Add("onkeypress", "javascript:return numeric_only(event)");
            txtPilotName.Attributes.Add("onkeypress", "javascript:return OnlyAlphabets(this,event)");
        }
    }

    private DataSet FillVehiclesWithCardsMapped()
    {
        var districtId = -1;
        if (Session["UserdistrictId"] != null) districtId = Convert.ToInt32(Session["UserdistrictId"].ToString());
        var ds = ObjFuelEntry.IFillVehiclesWithMappedCards(districtId);
        if (ds != null)
            try
            {
                _helper.FillDropDownHelperMethodWithDataSet(ds, "VehicleNumber", "VehicleID", ddlDistrict);
            }
            catch (Exception ex)
            {
                _helper.ErrorsEntry(ex);
            }

        var itemToRemove = ddlDistrict.Items.FindByValue(ddlVehicleNumber.SelectedValue);
        ddlDistrict.ClearSelection();
        if (itemToRemove != null) ddlDistrict.Items.Remove(itemToRemove);
        ddlDistrict.Enabled = true;
        return ds;
    }

    protected void ddlDistrict_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillCardNumber(Convert.ToInt32(ddlDistrict.SelectedValue));
        ddlPetroCardNumber.Enabled = false;
    }

    //Shiva...GetVehicleNumber() method
    private void FillVehicles()
    {
        var ds = _fmsg.GetVehicleNumber();
        if (ds == null) return;
        try
        {
            _helper.FillDropDownHelperMethodWithDataSet(ds, "VehicleNumber", "VehicleID", ddlVehicleNumber);
            ddlVehicleNumber.Enabled = true;
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    private void FillDistrictLocation()
    {
        _fmsg.vehicle = ddlVehicleNumber.SelectedItem.ToString();
        var dsDistrict = _fmsg.GetDistrictLoc();
        if (dsDistrict == null) throw new ArgumentNullException(nameof(dsDistrict));
        lblDistrict.Text = dsDistrict.Tables[0].Rows[0]["District"].ToString();
        lblLocation.Text = dsDistrict.Tables[0].Rows[0]["BaseLocation"].ToString();
        lblDistrict.ForeColor = Color.ForestGreen;
        lblLocation.ForeColor = Color.ForestGreen;
    }

    private void FillServiceStn()
    {
        _fmsg.ServiceStn = lblDistrict.Text;
        var dsServiceStn = _fmsg.GetServiceStns();
        if (dsServiceStn == null) return;
        _helper.FillDropDownHelperMethodWithDataSet(dsServiceStn, "ServiceStnName", "Id", ddlBunkName);
        ddlBunkName.Enabled = true;
    }

    private void FillServiceStnVeh()
    {
        _fmsg.ServiceStnVeh = Convert.ToInt32(ddlVehicleNumber.SelectedValue);
        var dsServiceStn = _fmsg.GetServiceStnsVeh();
        if (dsServiceStn == null) throw new ArgumentNullException(nameof(dsServiceStn));
        switch (dsServiceStn.Tables[0].Rows.Count)
        {
            case 0:
                txtBunkName.Enabled = true;
                break;
            default:
                txtBunkName.Text = Convert.ToString(dsServiceStn.Tables[0].Rows[0][1]);
                break;
        }
    }

    protected void ddlVehicleNumber_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillCardNumber(Convert.ToInt32(ddlVehicleNumber.SelectedValue));
        FillGridFuelEntry(Convert.ToInt32(ddlVehicleNumber.SelectedValue));
        ViewState["VehicleID"] = ddlVehicleNumber.SelectedValue;
        ddlPetroCardNumber.Enabled = false;
        var dsOdo = ObjFuelEntry.ICheckFuelEntryOdo(Convert.ToInt32(ddlVehicleNumber.SelectedValue));
        if (dsOdo == null) throw new ArgumentNullException(nameof(dsOdo));
        switch (dsOdo.Tables[0].Rows.Count)
        {
            case 0:
                maxOdo.Value = "0";
                break;
            default:
                maxOdo.Value = dsOdo.Tables[0].Rows[0]["ODO"].ToString() != string.Empty ? dsOdo.Tables[0].Rows[0]["ODO"].ToString() : "0";
                break;
        }

        if (ddlVehicleNumber.SelectedIndex > 0)
        {
            FillVehiclesWithCardsMapped();

            FillDistrictLocation();
            FillServiceStnVeh();
        }
        else
        {
            lblDistrict.Text = "";
            lblLocation.Text = "";
            txtBunkName.Text = "";
            ddlDistrict.Items.Clear();
        }
    }

    private void FillPayMode()
    {
        var ds = ObjFuelEntry.IFillPayMode();
        if (ds == null) return;
        try
        {
            _helper.FillDropDownHelperMethodWithDataSet(ds, "PayMode", "PayModeID", ddlPaymode);
            ddlPaymode.Enabled = true;
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    protected void ddlPaymode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlPaymode == null) return;
        switch (ddlPaymode.SelectedValue)
        {
            case "1":
                ddlPetroCardNumber.Enabled = true;
                ddlCardSwiped.Enabled = true;
                ddlAgency.Enabled = true;
                spAgency.Visible = true;
                spPetro.Visible = true;
                spCard.Visible = true;
                break;
            default:
                ddlPetroCardNumber.SelectedIndex = 0;
                ddlAgency.SelectedIndex = 0;
                ddlPetroCardNumber.Enabled = false;
                ddlAgency.Enabled = false;
                ddlCardSwiped.SelectedIndex = 1;               
                ddlCardSwiped.Enabled = false;
                spAgency.Visible = false;
                spPetro.Visible = false;
                spCard.Visible = false;
                break;
        }
    }

    private DataSet FillCardNumber(int vehicleId)
    {
        var ds = ObjFuelEntry.IFillCardNumber(vehicleId);
        if (ds == null) throw new ArgumentNullException(nameof(ds));
        switch (ds.Tables[0].Rows.Count)
        {
            case 0:
                var strFmsScript = "No cards mapped to this Vehicle";
                Show(strFmsScript);
                ddlPaymode.SelectedIndex = 2;
                ddlPaymode.Enabled = false;
                ddlCardSwiped.SelectedIndex = 1;
                ddlCardSwiped.Enabled = false;
              ddlPetroCardNumber.SelectedIndex = -1;
                break;
            default:
                _helper.FillDropDownHelperMethodWithDataSet(ds, "PetroCardNum", "PetroCardIssueID", ddlPetroCardNumber);
                ddlPaymode.Enabled = true;
                break;
        }

        return ds;
    }

    protected void ddlPetroCardNumber_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillFuelAgency(Convert.ToInt32(ddlPetroCardNumber.SelectedValue));
    }

    private void FillFuelAgency(int petroCardIssueId)
    {
        var ds = ObjFuelEntry.IFillFuelAgency(petroCardIssueId);
        if (ds == null) throw new ArgumentNullException(nameof(ds));
        try
        {
            _helper.FillDropDownHelperMethodWithDataSet(ds, "AgencyName", "AgencyID", ddlAgency);
            ddlAgency.Enabled = true;
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    protected void Save_Click(object sender, EventArgs e)
    {
        var dsOdo = ObjFuelEntry.ICheckFuelEntryOdo(Convert.ToInt32(ddlVehicleNumber.SelectedValue));
        if (dsOdo == null) throw new ArgumentNullException(nameof(dsOdo));
        try
        {
            switch (dsOdo.Tables[0].Rows.Count)
            {
                case 0:
                    maxOdo.Value = "0";
                    break;
                default:
                    if (dsOdo.Tables[0].Rows[0]["ODO"].ToString() == string.Empty)
                    {
                        maxOdo.Value = "0";
                    }
                    else
                    {
                        maxOdo.Value = dsOdo.Tables[0].Rows[0]["ODO"].ToString();
                        ViewState["maxodometer"] = dsOdo.Tables[0].Rows[0]["ODO"].ToString();
                    }

                    break;
            }

            var entrydate = DateTime.ParseExact(txtFuelEntryDate.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            if (entrydate > DateTime.Now)
            {
                Show("Fuel entry date should not be greater than current date ");
                return;
            }

            Save.Enabled = false;
            var fmsGeneral = new FMSGeneral();
            var ds = fmsGeneral.GetRegistrationDate(int.Parse(ddlVehicleNumber.SelectedItem.Value));
            if (ds == null) throw new ArgumentNullException(nameof(ds));
            Save.Enabled = true;
            switch (ds.Tables[0].Rows.Count)
            {
                case 0:
                    Show("Fuel Entry cannot be done as vehicle is not yet Registered");
                    break;
                default:
                    if (txtOdometer.Text.Trim() == string.Empty)
                    {
                        Show("Enter Odo value");
                        return;
                    }
                    else
                    {
                        if (Convert.ToInt32(ViewState["maxodometer"].ToString()) != 0)
                        {
                            var maxno = Convert.ToInt32(ViewState["maxodometer"].ToString()) + 1000;
                            if (maxno <= Convert.ToInt32(txtOdometer.Text) || Convert.ToInt32(txtOdometer.Text) <= Convert.ToInt32(ViewState["maxodometer"].ToString()))
                            {
                                Show("Odo value between  " + ViewState["maxodometer"] + " And " + maxno);
                                txtOdometer.Text = "";
                                txtOdometer.Focus();
                                return;
                            }
                        }
                    }

                    var dtofRegistration = DateTime.ParseExact(ds.Tables[0].Rows[0]["RegDate"].ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    var fuelEntry = DateTime.ParseExact(txtFuelEntryDate.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    if (dtofRegistration > fuelEntry)
                    {
                        Show("Fuel entry date should be greater than date of registration ");
                        return;
                    }

                    var dtpreviousentryDate = GetpreviousOdo(int.Parse(ddlVehicleNumber.SelectedItem.Value));
                    if (dtpreviousentryDate.Rows.Count > 0 && dtpreviousentryDate.Rows[0]["maxentry"].ToString() != "")
                    {
                        var dtprvrefill = Convert.ToDateTime(dtpreviousentryDate.Rows[0]["maxentry"].ToString());
                        if (dtprvrefill > DateTime.ParseExact(txtFuelEntryDate.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture))
                        {
                            Show("Fuel entry date must be greater than previous fuel entry date");
                            return;
                        }
                    }

                    Save.Enabled = false;
                    if (Save.Text == "Save" && ddlPetroCardNumber.Enabled)
                    {
                        _bunkname = ddlBunkName.Visible ? ddlBunkName.SelectedItem.Text : txtBunkName.Text;
                        InsFuelEntry(Convert.ToInt32(Session["UserdistrictId"].ToString()), Convert.ToInt32(ddlVehicleNumber.SelectedValue), Convert.ToInt32(ddlDistrict.SelectedValue), fuelEntry, Convert.ToInt64(txtBillNumber.Text), Convert.ToInt64(txtOdometer.Text), _bunkname, Convert.ToInt32(ddlPaymode.SelectedValue), Convert.ToDecimal(txtQuantity.Text), Convert.ToInt64(ddlPetroCardNumber.SelectedValue), Convert.ToDecimal(txtUnitPrice.Text), Convert.ToInt32(ddlAgency.SelectedValue), Convert.ToString(txtLocation.Text), Convert.ToInt32(Session["User_Id"].ToString()), DateTime.Now, 1, Convert.ToDecimal(txtAmount.Text), Convert.ToInt32(txtPilotID.Text), Convert.ToString(txtPilotName.Text), Convert.ToInt32(ddlCardSwiped.SelectedValue), Convert.ToString(txtRemarks.Text));
                        FillGridFuelEntry(Convert.ToInt32(ViewState["VehicleID"]));
                    }
                    else if (Save.Text == "Save" && ddlPetroCardNumber.Enabled == false)
                    {
                        _bunkname = ddlBunkName.Visible ? ddlBunkName.SelectedItem.Text : txtBunkName.Text;
                        InsFuelEntry1(Convert.ToInt32(Session["UserdistrictId"].ToString()), Convert.ToInt32(ddlVehicleNumber.SelectedValue), fuelEntry, Convert.ToInt64(txtBillNumber.Text), Convert.ToInt64(txtOdometer.Text), _bunkname, Convert.ToInt32(ddlPaymode.SelectedValue), Convert.ToDecimal(txtQuantity.Text), Convert.ToDecimal(txtUnitPrice.Text), Convert.ToString(txtLocation.Text), Convert.ToInt32(Session["User_Id"].ToString()), DateTime.Now, 1, Convert.ToDecimal(txtAmount.Text), Convert.ToInt32(txtPilotID.Text), Convert.ToString(txtPilotName.Text), Convert.ToInt32(ddlCardSwiped.SelectedValue), Convert.ToString(txtRemarks.Text));
                        FillGridFuelEntry(Convert.ToInt32(ViewState["VehicleID"]));
                    }
                    else if (Save.Text == "Update" && ddlPetroCardNumber.Enabled)
                    {
                        _bunkname = ddlBunkName.Visible ? ddlBunkName.SelectedItem.Text : txtBunkName.Text;
                        UpdFuelEntry(Convert.ToInt32(txtEdit.Text), Convert.ToInt32(Session["UserdistrictId"].ToString()), Convert.ToInt32(ddlVehicleNumber.SelectedValue), Convert.ToInt32(ddlDistrict.SelectedValue), fuelEntry, Convert.ToInt64(txtBillNumber.Text), Convert.ToInt64(txtOdometer.Text), _bunkname, Convert.ToInt32(ddlPaymode.SelectedValue), Convert.ToDecimal(txtQuantity.Text), Convert.ToInt64(ddlPetroCardNumber.SelectedValue), Convert.ToDecimal(txtUnitPrice.Text), Convert.ToInt32(ddlAgency.SelectedValue), Convert.ToString(txtLocation.Text), Convert.ToDecimal(txtAmount.Text), Convert.ToInt32(txtPilotID.Text), Convert.ToString(txtPilotName.Text), Convert.ToInt32(ddlCardSwiped.SelectedValue), Convert.ToString(txtRemarks.Text));
                        FillGridFuelEntry(Convert.ToInt32(ViewState["VehicleID"]));
                    }
                    else
                    {
                        _bunkname = ddlBunkName.Visible ? ddlBunkName.SelectedItem.Text : txtBunkName.Text;
                        UpdFuelEntry1(Convert.ToInt32(txtEdit.Text), Convert.ToInt32(Session["UserdistrictId"].ToString()), Convert.ToInt32(ddlVehicleNumber.SelectedValue), fuelEntry, Convert.ToInt64(txtBillNumber.Text), Convert.ToInt64(txtOdometer.Text), _bunkname, Convert.ToInt32(ddlPaymode.SelectedValue), Convert.ToDecimal(txtQuantity.Text), Convert.ToDecimal(txtUnitPrice.Text), Convert.ToString(txtLocation.Text), Convert.ToDecimal(txtAmount.Text), Convert.ToInt32(txtPilotID.Text), Convert.ToString(txtPilotName.Text), Convert.ToInt32(ddlCardSwiped.SelectedValue), Convert.ToString(txtRemarks.Text));
                        FillGridFuelEntry(Convert.ToInt32(ViewState["VehicleID"]));
                    }

                    break;
            }
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    private DataTable GetpreviousOdo(int vehicleId)
    {
        var query = "select max(entrydate) maxentry from T_FMS_FuelEntryDetails where vehicleid = '" + vehicleId + "' and status = 1";
        DataTable dtVehData = null;
        try
        {
            dtVehData = _helper.ExecuteSelectStmt(query);
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }

        return dtVehData;
    }

    private void ShowKmpl()
    {
        var ds = ObjFuelEntry.GetKMPL(Convert.ToInt32(ddlVehicleNumber.SelectedValue));
        if (ds == null) throw new ArgumentNullException(nameof(ds));
        if (ds.Tables[0].Rows.Count > 0)
        {
            switch (ds.Tables[0].Rows[0]["KMPL"].ToString())
            {
                case "":
                    _flag = false;
                    _kmplInt = 0;
                    break;
                default:
                    var kmpl = ds.Tables[0].Rows[0]["KMPL"].ToString();
                    _kmplInt = Convert.ToDouble(kmpl);
                    _flag = true;
                    break;
            }
        }
        else
        {
            _flag = false;
            _kmplInt = 0;
        }
    }

    private void ShowMasterKmpl()
    {
        var ds = ObjFuelEntry.GetMasterKMPL(Convert.ToInt32(ddlVehicleNumber.SelectedValue));
        if (ds == null) throw new ArgumentNullException(nameof(ds));
        if (ds.Tables[0].Rows.Count > 0)
        {
            switch (ds.Tables[0].Rows[0]["KMPL"].ToString())
            {
                case "":
                    _flag = false;
                    _mSkmplInt = 0;
                    break;
                default:
                    var masterkmpl = ds.Tables[0].Rows[0]["KMPL"].ToString();
                    _mSkmplInt = Convert.ToDouble(masterkmpl);
                    _flag = true;
                    break;
            }
        }
        else
        {
            _flag = false;
            _kmplInt = 0;
        }
    }

    private void UpdFuelEntry1(int fuelEntryId, int districtId, int vehicleId, DateTime entryDate, long billNumber, long odometer, string bunkName, int paymode, decimal quantity, decimal unitPrice, string location, decimal amount, int pilotId, string pilotName, int cardSwipedStatus, string remarks)
    {
        var res = ObjFuelEntry.IUpdFuelEntry1(fuelEntryId, districtId, vehicleId, entryDate, billNumber, odometer, bunkName, paymode, quantity, unitPrice, location, amount, pilotId, pilotName, cardSwipedStatus, remarks);
        ShowKmpl();
        ShowMasterKmpl();
        string strFmsScript;
        switch (res)
        {
            case 1:
                if (Math.Abs(Math.Abs(_kmplInt)) <= 0 && _flag == false)
                {
                    strFmsScript = "Fuel Entry Inserted and KMPL is NA since no past Fuel Entry Records are found for this vehicle";
                    Show(strFmsScript);
                }
                else if (_kmplInt < 8)
                {
                    strFmsScript = "Fuel Entry Inserted and KMPL = " + _kmplInt + "\\nBenchMark KMPL =" + _mSkmplInt;
                    Show(strFmsScript);
                }
                else
                {
                    strFmsScript = "Fuel Entry Inserted and KMPL = " + _kmplInt + "\\nBenchMark KMPL =" + _mSkmplInt;
                    Show(strFmsScript);
                }

                break;
            default:
                strFmsScript = "Failure";
                Show(strFmsScript);
                break;
        }

        ClearFields();
    }

    private void InsFuelEntry(int districtId, int vehicleId, int borrowedVehicle, DateTime entryDate, long billNumber, long odometer, string bunkName, int paymode, decimal quantity, long petroCardNumber, decimal unitPrice, int agencyId, string location, int createdBy, DateTime createdDate, int status, decimal amount, int pilotId, string pilotName, int cardSwipedStatus, string remarks)
    {
        var dsOdo = ObjFuelEntry.ICheckFuelEntryOdo(Convert.ToInt32(ddlVehicleNumber.SelectedValue));
        if (dsOdo == null) throw new ArgumentNullException(nameof(dsOdo));
        var maxodo = Convert.ToInt32(dsOdo.Tables[0].Rows[0]["ODO"].ToString());
        if (maxodo < odometer)
        {
            var dsres = ObjFuelEntry.IInsFuelEntry(districtId, vehicleId, borrowedVehicle, entryDate, billNumber, odometer, bunkName, paymode, quantity, petroCardNumber, unitPrice, agencyId, location, createdBy, createdDate, status, amount, pilotId, pilotName, cardSwipedStatus, remarks);
            if (dsres == null) throw new ArgumentNullException(nameof(dsres));
            ShowKmpl();
            ShowMasterKmpl();
            if (dsres.Tables[0].Rows.Count > 0)
            {
                var resid = dsres.Tables[0].Rows[0][0].ToString();
                if (Math.Abs(_kmplInt) <= 0 && _flag == false)
                {
                    var strFmsScript = "Fuel Entry Inserted and KMPL is NA since no past Fuel Entry Records are found for this vehicle";
                    Show(strFmsScript);
                }
                else if (_kmplInt < 8)
                {
                    var strFmsScript = "Fuel Entry Inserted and KMPL = " + _kmplInt + "\\nBenchMark KMPL =" + _mSkmplInt + "\\nTransaction Id = " + resid;
                    Show(strFmsScript);
                }
                else
                {
                    var strFmsScript = "Fuel Entry Inserted and KMPL = " + _kmplInt + "\\nBenchMark KMPL =" + _mSkmplInt + "\\nTransaction Id = " + resid;
                    Show(strFmsScript);
                }
            }
            else
            {
                var strFmsScript = "Failure";
                Show(strFmsScript);
            }

            ClearFields();
        }
        else
        {
            var strFmsScript = "Odometer Reading can't be less than the Previous Odometer Reading";
            Show(strFmsScript);
        }
    }

    private void InsFuelEntry1(int districtId, int vehicleId, DateTime entryDate, long billNumber, long odometer, string bunkName, int paymode, decimal quantity, decimal unitPrice, string location, int createdBy, DateTime createdDate, int status, decimal amount, int pilotId, string pilotName, int cardSwipedStatus, string remarks)
    {
        var ds = ObjFuelEntry.IInsFuelEntry1(districtId, vehicleId, entryDate, billNumber, odometer, bunkName, paymode, quantity, unitPrice, location, createdBy, createdDate, status, amount, pilotId, pilotName, cardSwipedStatus, remarks);
        if (ds == null) throw new ArgumentNullException(nameof(ds));
        ShowKmpl();
        ShowMasterKmpl();
        if (ds.Tables[0].Rows.Count > 0)
        {
            var resid = ds.Tables[0].Rows[0][0].ToString();
            if (Math.Abs(_kmplInt) >= 0 && _flag == false)
            {
                var strFmsScript = "Fuel Entry Inserted and KMPL is NA since no past Fuel Entry Records are found for this vehicle";
                Show(strFmsScript);
            }
            else if (_kmplInt < 8)
            {
                var strFmsScript = "Fuel Entry Inserted and KMPL = " + _kmplInt + "\\nBenchMark KMPL =" + _mSkmplInt + "\\nTransaction Id = " + resid;
                Show(strFmsScript);
            }
            else
            {
                var strFmsScript = "Fuel Entry Inserted and KMPL = " + _kmplInt + "\\nBenchMark KMPL =" + _mSkmplInt + "\\nTransaction Id = " + resid;
                Show(strFmsScript);
            }
        }
        else
        {
            var strFmsScript = "Failure";
            Show(strFmsScript);
        }

        ClearFields();
    }

    private void UpdFuelEntry(int fuelEntryId, int districtId, int vehicleId, int borrowedVehicle, DateTime entryDate, long billNumber, long odometer, string bunkName, int paymode, decimal quantity, long petroCardNumber, decimal unitPrice, int agencyId, string location, decimal amount, int pilotId, string pilotName, int cardSwipedStatus, string remarks)
    {
        var res = ObjFuelEntry.IUpdFuelEntry(fuelEntryId, districtId, vehicleId, borrowedVehicle, entryDate, billNumber, odometer, bunkName, paymode, quantity, petroCardNumber, unitPrice, agencyId, location, amount, pilotId, pilotName, cardSwipedStatus, remarks);
        ShowKmpl();
        ShowMasterKmpl();
        string strFmsScript;
        switch (res)
        {
            case 1:
                if (Math.Abs(_kmplInt) <= 0 && _flag == false)
                {
                    strFmsScript = "Fuel Entry Inserted and KMPL is NA since no past Fuel Entry Records are found for this vehicle";
                    Show(strFmsScript);
                }
                else if (_kmplInt < 8)
                {
                    strFmsScript = "Fuel Entry Inserted and KMPL = " + _kmplInt + "\\nBenchMark KMPL =" + _mSkmplInt;
                    Show(strFmsScript);
                }
                else
                {
                    strFmsScript = "Fuel Entry Inserted and KMPL = " + _kmplInt + "\\nBenchMark KMPL =" + _mSkmplInt;
                    Show(strFmsScript);
                }

                break;
            default:
                strFmsScript = "Failure";
                Show(strFmsScript);
                break;
        }

        ClearFields();
    }

    private void FillGridFuelEntry(int vehicleId)
    {
        try
        {
            gvFuelEntry.Visible = true;
            var ds = ObjFuelEntry.IFillGridFuelEntry(vehicleId);
            if (ds != null && ds.Tables.Count > 0)
            {
                gvFuelEntry.DataSource = ds;
                gvFuelEntry.DataBind();
                ViewState["maxodometer"] = ds.Tables[0].Rows[0]["odo"].ToString();
            }
            else
            {
                ViewState["maxodometer"] = 0;
            }
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    protected void gvFuelEntry_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvFuelEntry.PageIndex = e.NewPageIndex;
        var ds = ObjFuelEntry.IFillGridFuelEntry(Convert.ToInt32(ddlVehicleNumber.SelectedValue));
        if (ds == null) throw new ArgumentNullException(nameof(ds));
        gvFuelEntry.DataSource = ds;
        gvFuelEntry.DataBind();
    }

    protected void Reset_Click(object sender, EventArgs e)
    {
        ClearFields();
    }

    private void ClearFields()
    {
        txtAmount.Text = string.Empty;
        txtBillNumber.Text = string.Empty;
        if (ddlBunkName.Visible)
            ddlBunkName.Items.Clear();
        else
            txtBunkName.Text = string.Empty;
        txtEdit.Text = string.Empty;
        txtFuelEntryDate.Text = string.Empty;
        txtLocation.Text = string.Empty;
        txtOdometer.Text = string.Empty;
        txtQuantity.Text = string.Empty;
        txtSegmentID.Text = string.Empty;
        txtUnitPrice.Text = string.Empty;
        txtPilotID.Text = string.Empty;
        txtPilotName.Text = string.Empty;
        if (ddlAgency.Items.Count != 0) ddlAgency.SelectedIndex = -1;
        ddlPaymode.SelectedIndex = 0;
        if (ddlAgency.Items.Count != 0) ddlPetroCardNumber.SelectedIndex = -1;
        if (ddlAgency.Items.Count != 0) ddlVehicleNumber.SelectedIndex = -1;
        txtRemarks.Text = "";
        ddlAgency.Enabled = true;
        ddlAgency.Items.Clear();
        ddlPaymode.Enabled = true;
        ddlPetroCardNumber.Enabled = true;
        ddlPetroCardNumber.Items.Clear();
        ddlVehicleNumber.Enabled = true;
        ddlDistrict.Enabled = true;
        ddlDistrict.Items.Clear();
        ddlCardSwiped.SelectedIndex = -1;
        Save.Text = "Save";
        ddlVehicleNumber.SelectedIndex = 0;
        txtOdometer.Enabled = true;
        ddlCardSwiped.SelectedIndex = -1;
        ddlCardSwiped.Enabled = true;
        gvLastTransactions.Visible = false;
        gvFuelEntry.Visible = false;
        lblDistrict.Visible = false;
        lblLocation.Visible = false;
        txtBunkName.Visible = true;
        txtBunkName.Text = "";
        txtBunkName.Enabled = false;
        ddlBunkName.Visible = false;
        Save.Enabled = true;
    }

    public void Show(string message)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('" + message + "');", true);
    }

    protected void gvFuelEntry_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == null) return;
        try
        {
            switch (e.CommandName)
            {
                case "EditFuel":
                {
                    Save.Text = "Update";
                    var id = Convert.ToInt32(e.CommandArgument.ToString());
                    var ds = ObjFuelEntry.IEditFuelEntryDetails(id);
                    if (ds == null) throw new ArgumentNullException(nameof(ds));
                    FillCardNumber(Convert.ToInt32(ds.Tables[0].Rows[0]["VehicleID"].ToString()));
                    ddlPetroCardNumber.Enabled = false;
                    txtEdit.Text = Convert.ToString(id);
                    ddlPaymode.ClearSelection();
                    ddlPaymode.Items.FindByValue(ds.Tables[0].Rows[0]["Paymode"].ToString()).Selected = true;
                    ddlVehicleNumber.ClearSelection();
                    ddlVehicleNumber.Items.FindByValue(ds.Tables[0].Rows[0]["VehicleID"].ToString()).Selected = true;
                    ddlCardSwiped.ClearSelection();
                    ddlCardSwiped.Items.FindByValue(ds.Tables[0].Rows[0]["CardSwipedStatus"].ToString()).Selected = true;
                    ddlCardSwiped.Enabled = false;
                    FillVehiclesWithCardsMapped();                    
                    maxOdo.Value = "0";
                    txtFuelEntryDate.Text = ds.Tables[0].Rows[0]["EntryDate"].ToString();
                    txtBillNumber.Text = ds.Tables[0].Rows[0]["BillNumber"].ToString();
                    txtOdometer.Text = ds.Tables[0].Rows[0]["Odometer"].ToString();
                    txtBunkName.Text = ds.Tables[0].Rows[0]["BunkName"].ToString();
                    var coBGrade = Convert.ToString(ds.Tables[0].Rows[0]["Quantity"].ToString()).Split('.');
                    txtQuantity.Text = coBGrade[0] + '.' + coBGrade[1].Substring(0, 2);
                    txtLocation.Text = ds.Tables[0].Rows[0]["Location"].ToString();
                    var cGrade = Convert.ToString(ds.Tables[0].Rows[0]["UnitPrice"].ToString()).Split('.');
                    txtUnitPrice.Text = cGrade[0] + '.' + cGrade[1].Substring(0, 2);
                    var coAGrade = Convert.ToString(ds.Tables[0].Rows[0]["Amount"].ToString()).Split('.');
                    txtAmount.Text = coAGrade[0] + '.' + coAGrade[1].Substring(0, 2);
                    txtPilotID.Text = ds.Tables[0].Rows[0]["Pilot"].ToString();
                    txtPilotName.Text = ds.Tables[0].Rows[0]["PilotName"].ToString();
                    txtRemarks.Text = ds.Tables[0].Rows[0]["RemarksFuel"].ToString();
                        //if (ds.Tables[0].Rows[0]["PetroCardNumber"].ToString() != string.Empty)
                        //{
                        //    switch (Convert.ToInt32(ds.Tables[0].Rows[0]["BorrowedVehicleID"].ToString()))
                        //    {
                        //        case 0:
                        //            {
                        //                var vid = Convert.ToInt32(ds.Tables[0].Rows[0]["VehicleID"].ToString());
                        //                ddlPetroCardNumber.ClearSelection();
                        //                DataSet dsPetro=FillCardNumber(vid);
                        //                ddlPetroCardNumber.Items.FindByValue(dsPetro.Tables[0].Rows[0]["PetroCardNum"].ToString()).Selected = true;
                        //                var pid = Convert.ToInt32(ds.Tables[0].Rows[0]["PetroCardNumber"].ToString());
                        //                ddlAgency.ClearSelection();
                        //                FillFuelAgency(pid);
                        //                ddlAgency.Items.FindByValue(ds.Tables[0].Rows[0]["AgencyID"].ToString()).Selected = true;
                        //                break;
                        //            }
                        //        default:
                        //            {
                        //                var vid = Convert.ToInt32(ds.Tables[0].Rows[0]["BorrowedVehicleID"].ToString());
                        //                ddlPetroCardNumber.ClearSelection();
                        //                FillCardNumber(vid);
                        //                ddlPetroCardNumber.Items.FindByValue(ds.Tables[0].Rows[0]["PetroCardNumber"].ToString()).Selected = true;
                        //                var pid = Convert.ToInt32(ds.Tables[0].Rows[0]["PetroCardNumber"].ToString());
                        //                ddlAgency.ClearSelection();
                        //                FillFuelAgency(pid);
                        //                ddlAgency.Items.FindByValue(ds.Tables[0].Rows[0]["AgencyID"].ToString()).Selected = true;
                        //                ddlDistrict.ClearSelection();
                        //                ddlDistrict.Items.FindByValue(ds.Tables[0].Rows[0]["BorrowedVehicleID"].ToString()).Selected = true;
                        //                break;
                        //            }
                        //    }
                        //}
                        //else
                        //{
                        //    FillFuelAgency(0);
                        //    var vid = Convert.ToInt32(ds.Tables[0].Rows[0]["VehicleID"].ToString());
                        //    ObjFuelEntry.IFillCardNumber(vid);
                        //    ObjFuelEntry.IFillAgencyWoDistrictID();
                        //}

                        ddlVehicleNumber.Enabled = false;
                    ddlAgency.Enabled = true;
                    ddlPaymode.Enabled = false;
                    ddlPetroCardNumber.Enabled = true;
                    ddlDistrict.Enabled = false;
                    break;
                }
                case "DeleteFuel":
                {
                    var id = Convert.ToInt32(e.CommandArgument.ToString());
                    var result = ObjFuelEntry.IDeleteFuelEntry(id);
                    switch (result)
                    {
                        case 1:
                        {
                            var strFmsScript = "Fuel Entry Deactivated";
                            Show(strFmsScript);
                            break;
                        }
                        default:
                        {
                            var strFmsScript = "failure";
                            Show(strFmsScript);
                            break;
                        }
                    }

                    ClearFields();
                    FillGridFuelEntry(Convert.ToInt32(ViewState["VehicleID"]));
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    protected void lnkNew_Click(object sender, EventArgs e)
    {
        txtBunkName.Visible = false;
        ddlBunkName.Visible = true;
        linkExisting.Visible = true;
        lnkNew.Visible = false;
        FillServiceStn();
    }

    protected void linkExisting_Click(object sender, EventArgs e)
    {
        txtBunkName.Visible = true;
        ddlBunkName.Visible = false;
        linkExisting.Visible = false;
        lnkNew.Visible = true;
        FillServiceStnVeh();
    }
}