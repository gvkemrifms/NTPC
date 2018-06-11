using System;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using GvkFMSAPP.BLL;
using GvkFMSAPP.PL;

public partial class PreDeliveryInspection : Page
{
    private readonly FMSGeneral _fmsGeneral = new FMSGeneral();
    private readonly Helper _helper = new Helper();
    private readonly GvkFMSAPP.BLL.PreDeliveryInspection _predelinsp = new GvkFMSAPP.BLL.PreDeliveryInspection();
    private int _ret;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Name"] == null) Response.Redirect("login.aspx");
        if (!IsPostBack)
        {
            var dsPerms = (DataSet) Session["PermissionsDS"];
            dsPerms.Tables[0].DefaultView.RowFilter = "Url='" + Page.Request.Url.Segments[Page.Request.Url.Segments.Length - 1] + "'";
            var p = new PagePermissions(dsPerms, dsPerms.Tables[0].DefaultView[0]["Url"].ToString(), dsPerms.Tables[0].DefaultView[0]["Title"].ToString());
            btSave.Attributes.Add("onclick", "return validation()");
            GetPreDeliveryInspection();
            GetTrNo();
            GetVehicleRecievedFrom();
            ViewState["Add"] = 0;
            pnlPreDeliveryInspection.Visible = false;
            gvPreDeliveryInspection.Visible = false;
            if (p.View)
            {
                gvPreDeliveryInspection.Visible = true;
                gvPreDeliveryInspection.Columns[6].Visible = false;
                gvPreDeliveryInspection.Columns[5].Visible = false;
            }

            if (p.Add)
            {
                pnlPreDeliveryInspection.Visible = true;
                gvPreDeliveryInspection.Visible = true;
                gvPreDeliveryInspection.Columns[6].Visible = false;
                gvPreDeliveryInspection.Columns[5].Visible = false;
                ViewState["Add"] = 1;
            }

            if (p.Modify)
            {
                gvPreDeliveryInspection.Visible = true;
                gvPreDeliveryInspection.Columns[6].Visible = true;
                gvPreDeliveryInspection.Columns[5].Visible = true;
            }
        }
    }

    public void GetPreDeliveryInspection()
    {
        gvPreDeliveryInspection.DataSource = _predelinsp.GetPreDeliveryInspection();
        gvPreDeliveryInspection.DataBind();
    }

    public void GetTrNo()
    {
        try
        {
            var ds = _predelinsp.GetTRNo(); //FMS.BLL.PreDeliveryInspection.GetTRNo();
            if (ds != null) _helper.FillDropDownHelperMethodWithDataSet(ds, "TRNo", "VehicleID", null, ddlTRNo);
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    public void GetVehicleRecievedFrom()
    {
        try
        {
            var ds = _predelinsp.GetVehicleRecievedFrom();
            if (ds != null) _helper.FillDropDownHelperMethodWithDataSet(ds, "FleetFabricator_Name", "FleetFabricator_Id", ddlVehicleReceived);
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    protected void btSave_Click(object sender, EventArgs e)
    {
        Show("Record Inserted Successfully on" + " " + DateTime.Now.ToString(CultureInfo.InvariantCulture));
        if (ViewState["PreDeliveryInspectionID"] != null) _predelinsp.PreDeliveryInspectionID = int.Parse(ViewState["PreDeliveryInspectionID"].ToString());
        _predelinsp.VehicleReceivedFrom = ddlVehicleReceived.SelectedItem.Value;
        _predelinsp.ReceivedDate = DateTime.ParseExact(txtReceivedDate.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
        _predelinsp.Odometer = int.Parse(txtOdometer.Text);
        _predelinsp.PDIBy = txtPDIBy.Text;
        _predelinsp.PDIDate = DateTime.ParseExact(txtPDIDate.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
        switch (btSave.Text)
        {
            case "Save":
                _predelinsp.VehicleID = int.Parse(ddlTRNo.SelectedItem.Value);
                var valpredelinsp = _predelinsp.ValidatePreDeliveryInspection();
                if (valpredelinsp.Tables[0].Rows.Count > 0)
                {
                    Show("Data is already present for this vehicle");
                    ViewState["PreDeliveryInspectionID"] = null;
                }
                else
                {
                    _ret = _predelinsp.InsPreDeliveryInspection();
                    GetPreDeliveryInspection();
                    Show(_ret == 1 ? "Record Inserted Successfully" : "Error");
                }

                break;
            case "Update":
                _predelinsp.VehicleID = int.Parse(ViewState["VehId"].ToString());
                _ret = _predelinsp.UpdtPreDeliveryInspection();
                GetPreDeliveryInspection();
                Show(_ret == 1 ? "Record Updated Successfully" : "Error");
                btSave.Text = "Save";
                ViewState["PreDeliveryInspectionID"] = null;
                ddlTRNo.Visible = true;
                txtTRNo.Visible = false;
                if (int.Parse(ViewState["Add"].ToString()) == 0) pnlPreDeliveryInspection.Visible = false;
                break;
            default:
                Show("Error");
                break;
        }

        ClearControls();
        GetTrNo();
    }

    protected void btReset_Click(object sender, EventArgs e)
    {
        ClearControls();
        btSave.Text = "Save";
        ViewState["PreDeliveryInspectionID"] = null;
        ddlTRNo.Visible = true;
        txtTRNo.Visible = false;
    }

    public void ClearControls()
    {
        ddlTRNo.ClearSelection();
        ddlVehicleReceived.ClearSelection();
        txtReceivedDate.Text = "";
        txtOdometer.Text = "";
        txtPDIBy.Text = "";
        txtPDIDate.Text = "";
    }

    protected void gvPreDeliveryInspection_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == null) return;
        switch (e.CommandName)
        {
            case "pdiEdit":
                ViewState["PreDeliveryInspectionID"] = e.CommandArgument.ToString();
                var dspdiedit = _predelinsp.GetPreDeliveryInspection();
                var drpdi = dspdiedit.Tables[0].Select("PreDeliveryInspectionID=" + e.CommandArgument);
                ClearControls();
                ddlTRNo.Visible = false;
                txtTRNo.Visible = true;
                txtTRNo.Text = drpdi[0][7].ToString();
                ViewState["VehId"] = Convert.ToInt16(drpdi[0][1].ToString());
                ddlVehicleReceived.Items.FindByValue(drpdi[0][2].ToString()).Selected = true;
                txtReceivedDate.Text = drpdi[0][3].ToString();
                txtOdometer.Text = drpdi[0][4].ToString();
                txtPDIBy.Text = drpdi[0][5].ToString();
                txtPDIDate.Text = drpdi[0][6].ToString();
                var datesUpdt = _fmsGeneral.GetFabInspDate(int.Parse(ViewState["VehId"].ToString()));
                var dtUpdat = Convert.ToDateTime(datesUpdt.Tables[0].Rows[0]["FVDInspectedDate"].ToString());
                vehicleFabInspDate.Value = dtUpdat.ToString(CultureInfo.InvariantCulture);
                pnlPreDeliveryInspection.Visible = true;
                btSave.Text = "Update";
                break;
            case "pdiDelete":
                _predelinsp.PreDeliveryInspectionID = int.Parse(e.CommandArgument.ToString());
                var output = _predelinsp.ValidatePDIRegVehicle();
                switch (output)
                {
                    case 0:
                        _ret = _predelinsp.DelPreDeliveryInspection();
                        GetPreDeliveryInspection();
                        Show(_ret == 1 ? "Record Deleted Successfully" : "Error");
                        break;
                    default:
                        Show("Vehicle Registation has been completed, can not delete");
                        break;
                }

                ClearControls();
                btSave.Text = "Save";
                ViewState["PreDeliveryInspectionID"] = null;
                GetTrNo();
                break;
        }
    }

    public void Show(string message)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('" + message + "');", true);
    }

    protected void gvPreDeliveryInspection_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvPreDeliveryInspection.PageIndex = e.NewPageIndex;
        GetPreDeliveryInspection();
    }

    protected void ddlTRNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            var dates = _fmsGeneral.GetFabInspDate(int.Parse(ddlTRNo.SelectedItem.Value));
            var dt = Convert.ToDateTime(dates.Tables[0].Rows[0]["FVDInspectedDate"].ToString());
            vehicleFabInspDate.Value = dt.ToString(CultureInfo.InvariantCulture);
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }
}