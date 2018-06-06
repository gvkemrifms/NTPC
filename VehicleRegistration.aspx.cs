using System;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using GvkFMSAPP.BLL;
using GvkFMSAPP.PL;
using BaseVehicleDetails = GvkFMSAPP.BLL.BaseVehicleDetails;

public partial class VehicleRegistration : Page
{
    private readonly FMSGeneral _fmsGeneral = new FMSGeneral();
    private readonly BaseVehicleDetails _getDistrict = new BaseVehicleDetails();
    private readonly Helper _helper = new Helper();
    private readonly GvkFMSAPP.BLL.VehicleRegistration _vehreg = new GvkFMSAPP.BLL.VehicleRegistration();
    private int _ret;

    public string UserId { get; private set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] == null)
            Response.Redirect("Login.aspx");
        else
            UserId = (string)Session["User_Id"];
        if (!IsPostBack)
        {
            var dsPerms = (DataSet) Session["PermissionsDS"];
            if (dsPerms == null) throw new ArgumentNullException(nameof(dsPerms));
            dsPerms.Tables[0].DefaultView.RowFilter = "Url='" + Page.Request.Url.Segments[Page.Request.Url.Segments.Length - 1] + "'";
            var p = new PagePermissions(dsPerms, dsPerms.Tables[0].DefaultView[0]["Url"].ToString(), dsPerms.Tables[0].DefaultView[0]["Title"].ToString());
            btSave.Attributes.Add("onclick", "return validation()");
            GetVehicleRegistration();
            GetTrNo();
            GetDistricts();
            ViewState["Add"] = 0;
            pnlVehicleRegistration.Visible = false;
            gvVehicleRegistration.Visible = false;
            if (p.View)
            {
                gvVehicleRegistration.Visible = true;
                gvVehicleRegistration.Columns[5].Visible = false;
            }

            if (p.Add)
            {
                pnlVehicleRegistration.Visible = true;
                gvVehicleRegistration.Visible = true;
                gvVehicleRegistration.Columns[5].Visible = false;
                ViewState["Add"] = 1;
            }

            if (p.Modify)
            {
                gvVehicleRegistration.Visible = true;
                gvVehicleRegistration.Columns[5].Visible = true;
            }
        }
    }

    public void GetVehicleRegistration()
    {
        gvVehicleRegistration.DataSource = _vehreg.GetVehicleRegistration_new();
        gvVehicleRegistration.DataBind();
    }

    public void GetTrNo()
    {
        try
        {
            var ds = _vehreg.GetTRNo(); //FMS.BLL.VehicleRegistration.GetTrNo();
            if (ds != null) _helper.FillDropDownHelperMethodWithDataSet(ds, "TRNo", "VehicleID", null, ddlTRNo);
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    public void GetDistricts()
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

    protected void btSave_Click(object sender, EventArgs e)
    {
        if (ViewState["VehicleRegID"] != null) _vehreg.VehicleRegID = int.Parse(ViewState["VehicleRegID"].ToString());
        _vehreg.EngineNumber = txtEngineNo.Text;
        _vehreg.ChasisNumber = txtChassisNo.Text;
        _vehreg.SeatingCapacity = int.Parse(txtSittingCapacity.Text);
        _vehreg.PRNo = txtPRNo.Text;
        _vehreg.RegDate = DateTime.ParseExact(txtRegistrationDate.Text,"MM/dd/yyyy",CultureInfo.InvariantCulture);
        _vehreg.VehicleRTACircle = txtRTACircle.Text;
        _vehreg.District = ddlDistrict.SelectedItem.Value;
        _vehreg.RegExpenses = float.Parse(txtRegisExpenses.Text);
        switch (btSave.Text)
        {
            case "Save":
                var output = _vehreg.ValidateVehicleNumber();
                switch (output)
                {
                    case 0:
                        _vehreg.VehicleID = int.Parse(ddlTRNo.SelectedItem.Value);
                        _ret = _vehreg.InsVehicleRegistration();
                        GetVehicleRegistration();
                        if (_ret >= 1)
                        {
                            Show("Record Inserted Successfully");
                            _vehreg.UpdtTempVehRegStatus();
                            _vehreg.UpdtVehicleNumber();
                            ClearControls();
                            GetTrNo();
                        }
                        else
                        {
                            Show("Error");
                        }

                        break;
                    default:
                        Show("P/R Number is already present");
                        break;
                }

                break;
            case "Update":
                _vehreg.VehicleID = int.Parse(ViewState["VehId"].ToString());
                _ret = _vehreg.UpdtVehicleRegistration();
                GetVehicleRegistration();
                switch (_ret)
                {
                    case 1:
                        Show("Record Updated Successfully");
                        _vehreg.UpdtVehicleNumber();
                        if (int.Parse(ViewState["Add"].ToString()) == 0) pnlVehicleRegistration.Visible = false;
                        break;
                    default:
                        Show("Error");
                        break;
                }

                ClearControls();
                btSave.Text = "Save";
                ddlTRNo.Items.Clear();
                ViewState["VehicleRegID"] = null;
                ddlTRNo.Visible = true;
                txtTrNo.Visible = false;
                GetTrNo();
                break;
            default:
                Show("Error");
                break;
        }
    }

    protected void ddlTRNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtEngineNo.Text = "";
        txtChassisNo.Text = "";
        GetChassisAndEngineNo();
        var dt = _fmsGeneral.GetPDIDate(int.Parse(ddlTRNo.SelectedItem.Value));
        vehiclePDIDate.Value = dt.ToString(CultureInfo.InvariantCulture);
    }

    public void GetChassisAndEngineNo()
    {
        switch (ddlTRNo.SelectedIndex)
        {
            case 0:
                txtEngineNo.Text = "";
                txtChassisNo.Text = "";
                break;
            default:
                _vehreg.VehicleID = int.Parse(ddlTRNo.SelectedItem.Value);
                var ds = _vehreg.GetChassisAndEngineNo();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtEngineNo.Text = ds.Tables[0].Rows[0]["EngineNumber"].ToString();
                    txtChassisNo.Text = ds.Tables[0].Rows[0]["ChasisNumber"].ToString();
                }

                break;
        }
    }

    protected void gvVehicleRegistration_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "VehRegEdit":
                ViewState["VehicleRegID"] = e.CommandArgument.ToString();
                var dsvehregedit = _vehreg.GetVehicleRegistration_new();
                var drvehreg = dsvehregedit.Tables[0].Select("VehicleRegID=" + e.CommandArgument);
                ClearControls();
                ddlTRNo.Visible = false;
                txtTrNo.Visible = true;
                txtTrNo.Text = drvehreg[0][11].ToString();
                ViewState["VehId"] = Convert.ToInt16(drvehreg[0][1].ToString());
                txtEngineNo.Text = drvehreg[0][2].ToString();
                txtChassisNo.Text = drvehreg[0][3].ToString();
                txtSittingCapacity.Text = drvehreg[0][4].ToString();
                txtPRNo.Text = drvehreg[0][5].ToString();
                txtRegistrationDate.Text = drvehreg[0][6].ToString();
                txtRTACircle.Text = drvehreg[0][7].ToString();
                ddlDistrict.Items.FindByValue(drvehreg[0][8].ToString()).Selected = true;
                txtRegisExpenses.Text = drvehreg[0][9].ToString();
                var dtUpdt = _fmsGeneral.GetPDIDate(int.Parse(ViewState["VehId"].ToString()));
                vehiclePDIDate.Value = dtUpdt.ToString(CultureInfo.InvariantCulture);
                pnlVehicleRegistration.Visible = true;
                btSave.Text = "Update";
                break;
        }
    }

    protected void btReset_Click(object sender, EventArgs e)
    {
        ClearControls();
        btSave.Text = "Save";
        ddlTRNo.Visible = true;
        txtTrNo.Visible = false;
    }

    public void ClearControls()
    {
        ddlTRNo.ClearSelection();
        txtEngineNo.Text = "";
        txtChassisNo.Text = "";
        txtSittingCapacity.Text = "";
        txtPRNo.Text = "";
        txtRegistrationDate.Text = "";
        txtRTACircle.Text = "";
        ddlDistrict.ClearSelection();
        txtRegisExpenses.Text = "";
        txtTrNo.Text = "";
    }

    public void Show(string message)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('" + message + "');", true);
    }

    protected void gvVehicleRegistration_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvVehicleRegistration.PageIndex = e.NewPageIndex;
        GetVehicleRegistration();
    }
}