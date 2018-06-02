using System;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using GvkFMSAPP.BLL;
using GvkFMSAPP.PL;

public partial class PollutionUnderControl : Page
{
    private readonly FMSGeneral _fmsGeneral = new FMSGeneral();
    private readonly Helper _helper = new Helper();
    private readonly GvkFMSAPP.BLL.StatutoryCompliance.PollutionUnderControl _puc = new GvkFMSAPP.BLL.StatutoryCompliance.PollutionUnderControl();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Name"] == null) Response.Redirect("Error.aspx");
        if (!IsPostBack)
        {
            var dsPerms = (DataSet) Session["PermissionsDS"];
            if (dsPerms == null) throw new ArgumentNullException(nameof(dsPerms));
            dsPerms.Tables[0].DefaultView.RowFilter = "Url='" + Page.Request.Url.Segments[Page.Request.Url.Segments.Length - 1] + "'";
            var p = new PagePermissions(dsPerms, dsPerms.Tables[0].DefaultView[0]["Url"].ToString(), dsPerms.Tables[0].DefaultView[0]["Title"].ToString());
            btSave.Attributes.Add("onclick", "return validation()");
            GetPollutionUnderControl();
            GetVehicleNumber();
            if (Request.QueryString["vehicleid"] != null) ddlVehicleNumber.Items.FindByValue(Request.QueryString["vehicleid"]).Selected = true;
            ViewState["Add"] = 0;
            pnlPUC.Visible = false;
            gvPollutionUnderControl.Visible = false;
            if (p.View)
            {
                gvPollutionUnderControl.Visible = true;
                gvPollutionUnderControl.Columns[6].Visible = false;
            }

            if (p.Add)
            {
                pnlPUC.Visible = true;
                gvPollutionUnderControl.Visible = true;
                gvPollutionUnderControl.Columns[6].Visible = false;
                ViewState["Add"] = 1;
            }

            if (p.Modify)
            {
                gvPollutionUnderControl.Visible = true;
                gvPollutionUnderControl.Columns[6].Visible = true;
            }
        }
    }

    public void GetPollutionUnderControl()
    {
        _puc.UserDistId = Convert.ToInt32(Session["UserdistrictId"].ToString());
        gvPollutionUnderControl.DataSource = _puc.GetPollutionUnderControl();
        gvPollutionUnderControl.DataBind();
    }

    public void GetVehicleNumber()
    {
        try
        {
            var ds = _puc.GetVehicleNumber(); //roadtax.GetVehicleNumber();
            if (ds != null) _helper.FillDropDownHelperMethodWithDataSet(ds, "VehicleNumber", "VehicleID", null, ddlVehicleNumber);
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    protected void btSave_Click(object sender, EventArgs e)
    {
        if (ViewState["PollutionUnderControlID"] != null) _puc.PollutionUnderControlID = int.Parse(ViewState["PollutionUnderControlID"].ToString());
        _puc.PUCValidityStartDate = DateTime.ParseExact(txtPollutionValidityStartDate.Text,"MM/dd/yyyy",CultureInfo.InvariantCulture);
        _puc.PUCValidityPeriod = ddlPollutionValidityPeriod.SelectedItem.Value;
        _puc.PUCValidityEndDate = DateTime.ParseExact(txtPollutionValidityEndDate.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
        _puc.PUCReceiptNo = txtPollutionReceiptNo.Text;
        _puc.PUCFee = float.Parse(txtPollutionFee.Text);
        switch (btSave.Text)
        {
            case "Save":
            {
                _puc.VehicleID = int.Parse(ddlVehicleNumber.SelectedItem.Value);
                var ret = _puc.InsPollutionUnderControl();
                GetPollutionUnderControl();
                Show(ret == 1 ? "Record Inserted Successfully" : "Error");
                break;
            }
            case "Update":
            {
                _puc.VehicleID = int.Parse(ViewState["VehNum"].ToString());
                var ret = _puc.UpdtPollutionUnderControl();
                GetPollutionUnderControl();
                Show(ret == 1 ? "Record Updated Successfully" : "Error");
                pnlPUC.Visible = true;
                break;
            }
            default:
                Show("Error");
                break;
        }

        GetPollutionUnderControl();
        ClearControls();
        GetVehicleNumber();
    }

    protected void btReset_Click(object sender, EventArgs e)
    {
        ClearControls();
    }

    protected void ddlPollutionValidityPeriod_SelectedIndexChanged(object sender, EventArgs e)
    {
        switch (txtPollutionValidityStartDate.Text)
        {
            case "":
                Show("Enter Pollution Validity Start Date");
                ddlPollutionValidityPeriod.SelectedIndex = 0;
                txtPollutionValidityEndDate.Text = "";
                break;
        }

        if (ddlPollutionValidityPeriod.SelectedIndex == 0) return;
        var test = Convert.ToDateTime(txtPollutionValidityStartDate.Text).AddMonths(Convert.ToInt16(ddlPollutionValidityPeriod.SelectedItem.Value)).ToString();
        txtPollutionValidityEndDate.Text = string.Format("{0:d}", test); //format test.ToString("0:d");
    }

    protected void gvPollutionUnderControl_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "roadTaxEdit":
                ViewState["PollutionUnderControlID"] = e.CommandArgument.ToString();
                _puc.UserDistId = Convert.ToInt32(Session["UserdistrictId"].ToString());
                var dspucedit = _puc.GetPollutionUnderControl();
                var drpuc = dspucedit.Tables[0].Select("PollutionUnderControlID=" + e.CommandArgument);
                ClearControls();
                ddlVehicleNumber.Visible = false;
                txtVehicleNumber.Visible = true;
                txtVehicleNumber.Text = drpuc[0][7].ToString();
                ViewState["VehNum"] = Convert.ToInt16(drpuc[0][1].ToString());
                txtPollutionValidityStartDate.Text = drpuc[0][2].ToString();
                ddlPollutionValidityPeriod.Items.FindByValue(drpuc[0][3].ToString()).Selected = true;
                txtPollutionValidityEndDate.Text = drpuc[0][4].ToString();
                txtPollutionReceiptNo.Text = drpuc[0][5].ToString();
                txtPollutionFee.Text = drpuc[0][6].ToString();
                var dtUpdate = _fmsGeneral.GetPurchaseDate(int.Parse(ViewState["VehNum"].ToString()));
                vehiclePurchaseDate.Value = dtUpdate.ToString(CultureInfo.InvariantCulture);
                pnlPUC.Visible = true;
                btSave.Text = "Update";
                break;
        }
    }

    public void ClearControls()
    {
        ddlVehicleNumber.ClearSelection();
        txtPollutionValidityStartDate.Text = "";
        ddlPollutionValidityPeriod.ClearSelection();
        txtPollutionFee.Text = "";
        txtPollutionReceiptNo.Text = "";
        txtPollutionValidityEndDate.Text = "";
        txtVehicleNumber.Text = "";
        ddlVehicleNumber.Visible = true;
        txtVehicleNumber.Visible = false;
        btSave.Text = "Save";
    }

    public void Show(string message)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('" + message + "');", true);
    }

    protected void gvPollutionUnderControl_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:
                var lblPucValidityPeriod = (Label) e.Row.FindControl("lblPUCValidityPeriod");
                var lblPucValidityPeriodText = (Label) e.Row.FindControl("lblPUCValidityPeriodText");
                switch (lblPucValidityPeriod.Text)
                {
                    case "3":
                        lblPucValidityPeriodText.Text = "3 Months";
                        break;
                    case "6":
                        lblPucValidityPeriodText.Text = "6 Months";
                        break;
                    case "9":
                        lblPucValidityPeriodText.Text = "9 Months";
                        break;
                    default:
                        lblPucValidityPeriodText.Text = "1 Yrs";
                        break;
                }

                break;
        }
    }

    protected void gvPollutionUnderControl_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvPollutionUnderControl.PageIndex = e.NewPageIndex;
        GetPollutionUnderControl();
    }

    protected void txtPollutionValidityStartDate_TextChanged1(object sender, EventArgs e)
    {
        ddlPollutionValidityPeriod.SelectedIndex = 0;
        txtPollutionValidityEndDate.Text = "";
    }

    protected void ddlVehicleNumber_SelectedIndexChanged(object sender, EventArgs e)
    {
        var dt = _fmsGeneral.GetPurchaseDate(int.Parse(ddlVehicleNumber.SelectedItem.Value));
        vehiclePurchaseDate.Value = dt.ToString(CultureInfo.InvariantCulture);
    }
}