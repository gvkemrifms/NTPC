using System;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using GvkFMSAPP.BLL;
using GvkFMSAPP.PL;

public partial class FitnessRenewal : Page
{
    private readonly GvkFMSAPP.BLL.StatutoryCompliance.FitnessRenewal _fitnessren = new GvkFMSAPP.BLL.StatutoryCompliance.FitnessRenewal();
    private readonly FMSGeneral _fmsGeneral = new FMSGeneral();
    private readonly Helper _helper = new Helper();
    private int _ret;

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
            GetFitnessRenewal();
            GetVehicleNumber();
            if (Request.QueryString["vehicleid"] != null) ddlVehicleNumber.Items.FindByValue(Request.QueryString["vehicleid"]).Selected = true;
            ViewState["Add"] = 0;
            pnlFitnessRenewal.Visible = false;
            gvFitnessRenewal.Visible = false;
            if (p.View)
            {
                gvFitnessRenewal.Visible = true;
                gvFitnessRenewal.Columns[7].Visible = false;
            }

            if (p.Add)
            {
                pnlFitnessRenewal.Visible = true;
                gvFitnessRenewal.Visible = true;
                gvFitnessRenewal.Columns[7].Visible = false;
                ViewState["Add"] = 1;
            }

            if (p.Modify)
            {
                gvFitnessRenewal.Visible = true;
                gvFitnessRenewal.Columns[7].Visible = true;
            }
        }
    }

    public void GetFitnessRenewal()
    {
        if (_fitnessren != null)
        {
            _fitnessren.UserDistrictId = Convert.ToInt32(Session["UserdistrictId"].ToString());
            gvFitnessRenewal.DataSource = _fitnessren.GetFitnessRenewal();
        }

        gvFitnessRenewal.DataBind();
    }

    public void GetVehicleNumber()
    {
        var ds = _fitnessren.GetVehicleNumber();
        if (ds != null) _helper.FillDropDownHelperMethodWithDataSet(ds, "VehicleNumber", "VehicleID", null, ddlVehicleNumber);
    }

    protected void btSave_Click(object sender, EventArgs e)
    {
        if (ViewState["FitnessRenewalID"] != null)
            if (_fitnessren != null)
                _fitnessren.FitnessRenewalID = int.Parse(ViewState["FitnessRenewalID"].ToString());
        if (_fitnessren != null)
            try
            {
                _fitnessren.FRValidityStartDate = DateTime.Parse(txtFitnessValidityStartDate.Text);
                _fitnessren.FRValidityPeriod = ddlFitnessValidityPeriod.SelectedItem.Value;
                _fitnessren.FRValidityEndDate = DateTime.Parse(txtFitnessValidityEndDate.Text);
                _fitnessren.VehicleRTACircle = txtVehicleRTACircle.Text;
                _fitnessren.FRReceiptNo = txtFitnessReceiptNo.Text;
                _fitnessren.FRFee = float.Parse(txtFitnessFee.Text);
                switch (btSave.Text)
                {
                    case "Save":
                        _fitnessren.VehicleID = int.Parse(ddlVehicleNumber.SelectedItem.Value);
                        _ret = _fitnessren.InsFitnessRenewal();
                        GetFitnessRenewal();
                        Show(_ret == 1 ? "Record Inserted Successfully" : "Error");
                        break;
                    case "Update":
                        _fitnessren.VehicleID = int.Parse(ViewState["VehNum"].ToString());
                        _ret = _fitnessren.UpdtFitnessRenewal();
                        GetFitnessRenewal();
                        Show(_ret == 1 ? "Record Updated Successfully" : "Error");
                        if (int.Parse(ViewState["Add"].ToString()) == 0) pnlFitnessRenewal.Visible = false;
                        break;
                    default:
                        Show("Error");
                        break;
                }
            }
            catch (Exception ex)
            {
                _helper.ErrorsEntry(ex);
            }

        ClearControls();
        GetVehicleNumber();
    }

    protected void btReset_Click(object sender, EventArgs e)
    {
        ClearControls();
    }

    protected void ddlFitnessValidityPeriod_SelectedIndexChanged(object sender, EventArgs e)
    {
        switch (txtFitnessValidityStartDate.Text)
        {
            case "":
                Show("Enter Fitness Validity Start Date");
                ddlFitnessValidityPeriod.SelectedIndex = 0;
                txtFitnessValidityEndDate.Text = "";
                break;
        }

        if (ddlFitnessValidityPeriod.SelectedIndex == 0) return;
        txtFitnessValidityEndDate.Text = Convert.ToDateTime(txtFitnessValidityStartDate.Text).AddMonths(Convert.ToInt16(ddlFitnessValidityPeriod.SelectedItem.Value)).Subtract(new TimeSpan(1, 0, 0)).ToString();
    }

    protected void gvFitnessRenewal_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName != null)
            try
            {
                switch (e.CommandName)
                {
                    case "fitnessRenewalEdit":
                        ViewState["FitnessRenewalID"] = e.CommandArgument.ToString();
                        _fitnessren.UserDistrictId = Convert.ToInt32(Session["UserdistrictId"].ToString());
                        var dsfitrenedit = _fitnessren.GetFitnessRenewal();
                        var drfitren = dsfitrenedit.Tables[0].Select("FitnessRenewalID=" + e.CommandArgument);
                        ClearControls();
                        ddlVehicleNumber.Visible = false;
                        txtVehicleNumber.Visible = true;
                        txtVehicleNumber.Text = drfitren[0][8].ToString();
                        ViewState["VehNum"] = Convert.ToInt16(drfitren[0][1].ToString());
                        txtFitnessValidityStartDate.Text = drfitren[0][2].ToString();
                        ddlFitnessValidityPeriod.Items.FindByValue(drfitren[0][3].ToString()).Selected = true;
                        txtFitnessValidityEndDate.Text = drfitren[0][4].ToString();
                        txtVehicleRTACircle.Text = drfitren[0][5].ToString();
                        txtFitnessReceiptNo.Text = drfitren[0][6].ToString();
                        txtFitnessFee.Text = drfitren[0][7].ToString();
                        var dtUpdat = _fmsGeneral.GetPurchaseDate(int.Parse(ViewState["VehNum"].ToString()));
                        vehiclePurchaseDate.Value = dtUpdat.ToString(CultureInfo.InvariantCulture);
                        pnlFitnessRenewal.Visible = true;
                        btSave.Text = "Update";
                        break;
                }
            }
            catch (Exception ex)
            {
                _helper.ErrorsEntry(ex);
            }
    }

    public void ClearControls()
    {
        ddlFitnessValidityPeriod.ClearSelection();
        ddlVehicleNumber.ClearSelection();
        txtFitnessFee.Text = "";
        txtFitnessReceiptNo.Text = "";
        txtFitnessValidityEndDate.Text = "";
        txtFitnessValidityStartDate.Text = "";
        txtVehicleRTACircle.Text = "";
        txtVehicleNumber.Text = "";
        btSave.Text = "Save";
        ddlVehicleNumber.Visible = true;
        txtVehicleNumber.Visible = false;
    }

    public void Show(string message)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('" + message + "');", true);
    }

    protected void gvFitnessRenewal_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row == null) return;
        try
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    var lblFrValidityPeriod = (Label) e.Row.FindControl("lblFRValidityPeriod");
                    if (lblFrValidityPeriod == null) throw new ArgumentNullException(nameof(lblFrValidityPeriod));
                    var lblFrValidityPeriodText = (Label) e.Row.FindControl("lblFRValidityPeriodText");
                    if (lblFrValidityPeriodText == null) throw new ArgumentNullException(nameof(lblFrValidityPeriodText));
                    switch (lblFrValidityPeriod.Text)
                    {
                        case "3":
                            lblFrValidityPeriodText.Text = "3 Months";
                            break;
                        case "6":
                            lblFrValidityPeriodText.Text = "6 Months";
                            break;
                        case "9":
                            lblFrValidityPeriodText.Text = "9 Months";
                            break;
                        default:
                            lblFrValidityPeriodText.Text = "1 Yrs";
                            break;
                    }

                    break;
            }
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    protected void gvFitnessRenewal_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvFitnessRenewal.PageIndex = e.NewPageIndex;
        GetFitnessRenewal();
    }

    protected void txtFitnessValidityStartDate_TextChanged1(object sender, EventArgs e)
    {
        ddlFitnessValidityPeriod.SelectedIndex = 0;
        txtFitnessValidityEndDate.Text = "";
    }

    protected void ddlVehicleNumber_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (_fmsGeneral == null) return;
        var dt = _fmsGeneral.GetPurchaseDate(int.Parse(ddlVehicleNumber.SelectedItem.Value));
        vehiclePurchaseDate.Value = dt.ToString(CultureInfo.InvariantCulture);
    }
}