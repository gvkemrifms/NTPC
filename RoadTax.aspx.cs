using System;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using GvkFMSAPP.BLL;
using GvkFMSAPP.PL;

public partial class RoadTax : Page
{
    private readonly FMSGeneral _fmsGeneral = new FMSGeneral();
    private readonly Helper _helper = new Helper();
    private readonly GvkFMSAPP.BLL.StatutoryCompliance.RoadTax _roadtax = new GvkFMSAPP.BLL.StatutoryCompliance.RoadTax();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Name"] == null) Response.Redirect("Login.aspx");
        if (!IsPostBack)
        {
            var dsPerms = (DataSet) Session["PermissionsDS"];
            dsPerms.Tables[0].DefaultView.RowFilter = "Url='" + Page.Request.Url.Segments[Page.Request.Url.Segments.Length - 1] + "'";
            var p = new PagePermissions(dsPerms, dsPerms.Tables[0].DefaultView[0]["Url"].ToString(), dsPerms.Tables[0].DefaultView[0]["Title"].ToString());
            btSave.Attributes.Add("onclick", "return validation()");
            GetRoadTax();
            GetVehicleNumber();
            if (Request.QueryString["vehicleid"] != null) ddlVehicleNumber.Items.FindByValue(Request.QueryString["vehicleid"]).Selected = true;
            ViewState["Add"] = 0;
            pnlRoadtax.Visible = false;
            gvRoadTax.Visible = false;
            if (p.View)
            {
                gvRoadTax.Visible = true;
                gvRoadTax.Columns[7].Visible = false;
            }

            if (p.Add)
            {
                pnlRoadtax.Visible = true;
                gvRoadTax.Visible = true;
                gvRoadTax.Columns[7].Visible = false;
                ViewState["Add"] = 1;
            }

            if (p.Modify)
            {
                gvRoadTax.Visible = true;
                gvRoadTax.Columns[7].Visible = true;
            }
        }

        if (chkbxTaxExempted.Checked == false)
        {
            lblRoadTaxFee.Visible = true;
            lblRoadTaxFeeStar.Visible = true;
            lblRoadTaxReceiptNo.Visible = true;
            lblRoadTaxReceiptNoStar.Visible = true;
            lblVehicleRTACircle.Visible = true;
            lblVehRTACirStar.Visible = true;
            txtRoadTaxFee.Visible = true;
            txtVehicleRTACircle.Visible = true;
            txtRoadTaxReceiptNo.Visible = true;
        }
    }

    public void GetRoadTax()
    {
        _roadtax.UserDistId = Convert.ToInt32(Session["UserdistrictId"].ToString());
        gvRoadTax.DataSource = _roadtax.GetRoadTax();
        gvRoadTax.DataBind();
    }

    public void GetVehicleNumber()
    {
        var ds = _roadtax.GetVehicleNumber();
        if (ds != null) _helper.FillDropDownHelperMethodWithDataSet(ds, "VehicleNumber", "VehicleID", ddlVehicleNumber);
    }

    protected void btSave_Click(object sender, EventArgs e)
    {
        _roadtax.RTValidityStartDate = DateTime.ParseExact(txtRoadTaxValidityStartDate.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
        _roadtax.RTValidityPeriod = ddlRoadTaxValidityPeriod.SelectedItem.Value;
        _roadtax.RTValidityEndDate = DateTime.Parse(txtRoadTaxValidityEndDate.Text);
        if (!chkbxTaxExempted.Checked)
        {
            _roadtax.VehicleRTACircle = txtVehicleRTACircle.Text;
            _roadtax.RTReceiptNo = txtRoadTaxReceiptNo.Text;
            _roadtax.RTFee = float.Parse(txtRoadTaxFee.Text);
        }

        switch (btSave.Text)
        {
            case "Save":
            {
                _roadtax.VehicleID = int.Parse(ddlVehicleNumber.SelectedItem.Value);
                var ret = _roadtax.InsRoadTax();
                Show(ret == 1 ? "Record Inserted Successfully" : "Error");
                break;
            }
            case "Update":
            {
                _roadtax.VehicleID = int.Parse(ViewState["VehNum"].ToString());
                if (ViewState["RoadTaxID"] != null) _roadtax.RoadTaxID = int.Parse(ViewState["RoadTaxID"].ToString());
                var ret = _roadtax.UpdtRoadTax();
                Show(ret == 1 ? "Record Updated Successfully" : "Error");
                break;
            }
            default:
                Show("Error");
                break;
        }

        if (int.Parse(ViewState["Add"].ToString()) == 0) pnlRoadtax.Visible = false;
        GetRoadTax();
        ClearControls();
        GetVehicleNumber();
    }

    protected void btReset_Click(object sender, EventArgs e)
    {
        ClearControls();
    }

    protected void ddlRoadTaxValidityPeriod_SelectedIndexChanged(object sender, EventArgs e)
    {
        switch (txtRoadTaxValidityStartDate.Text)
        {
            case "":
                Show("Enter Roadtax Validity Start Date");
                ddlRoadTaxValidityPeriod.SelectedIndex = 0;
                txtRoadTaxValidityEndDate.Text = "";
                break;
        }

        if (ddlRoadTaxValidityPeriod.SelectedIndex == 0) return;
        txtRoadTaxValidityEndDate.Text = Convert.ToDateTime(txtRoadTaxValidityStartDate.Text).AddMonths(Convert.ToInt16(ddlRoadTaxValidityPeriod.SelectedItem.Value)).Subtract(new TimeSpan(1, 0, 0)).ToString();
    }

    protected void chkbxTaxExempted_CheckedChanged(object sender, EventArgs e)
    {
        if (chkbxTaxExempted.Checked && chkbxTaxExempted.Checked)
        {
            lblRoadTaxFee.Visible = false;
            lblRoadTaxFeeStar.Visible = false;
            lblRoadTaxReceiptNo.Visible = false;
            lblRoadTaxReceiptNoStar.Visible = false;
            lblVehicleRTACircle.Visible = false;
            lblVehRTACirStar.Visible = false;
            txtRoadTaxFee.Visible = false;
            txtVehicleRTACircle.Visible = false;
            txtRoadTaxReceiptNo.Visible = false;
            txtRoadTaxFee.Text = "";
            txtVehicleRTACircle.Text = "";
            txtRoadTaxReceiptNo.Text = "";
        }
    }

    protected void gvRoadTax_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "roadTaxEdit":
                ViewState["RoadTaxID"] = e.CommandArgument.ToString();
                _roadtax.UserDistId = Convert.ToInt32(Session["UserdistrictId"].ToString());
                var dsroadtaxedit = _roadtax.GetRoadTax();
                var drroadtax = dsroadtaxedit.Tables[0].Select("RoadTaxID=" + e.CommandArgument);
                ClearControls();
                ddlVehicleNumber.Visible = false;
                txtVehicleNumber.Visible = true;
                txtVehicleNumber.Text = drroadtax[0][0].ToString();
                ViewState["VehNum"] = Convert.ToInt16(drroadtax[0][1].ToString());
                var dates = _fmsGeneral.GetRegistrationDate(int.Parse(drroadtax[0][1].ToString()));
                var dt = DateTime.ParseExact(dates.Tables[0].Rows[0]["RegDate"].ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                vehicleRegistrationDate.Value = dt.ToString();
                txtRoadTaxValidityStartDate.Text = drroadtax[0][3].ToString();
                ddlRoadTaxValidityPeriod.Items.FindByValue(drroadtax[0][4].ToString()).Selected = true;
                txtRoadTaxValidityEndDate.Text = drroadtax[0][5].ToString();
                txtVehicleRTACircle.Text = drroadtax[0][6].ToString();
                txtRoadTaxReceiptNo.Text = drroadtax[0][7].ToString();
                txtRoadTaxFee.Text = drroadtax[0][8].ToString();
                var datesUpdate = _fmsGeneral.GetRegistrationDate(int.Parse(ViewState["VehNum"].ToString()));
                var dtUpdate = DateTime.ParseExact(datesUpdate.Tables[0].Rows[0]["RegDate"].ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                vehicleRegistrationDate.Value = dtUpdate.ToString(CultureInfo.InvariantCulture);
                pnlRoadtax.Visible = true;
                btSave.Text = "Update";
                break;
        }
    }

    public void ClearControls()
    {
        ddlRoadTaxValidityPeriod.ClearSelection();
        ddlVehicleNumber.ClearSelection();
        txtRoadTaxReceiptNo.Text = "";
        txtRoadTaxValidityEndDate.Text = "";
        txtRoadTaxValidityStartDate.Text = "";
        txtVehicleRTACircle.Text = "";
        txtRoadTaxFee.Text = "";
        btSave.Text = "Save";
        ddlVehicleNumber.Visible = true;
        txtVehicleNumber.Visible = false;
    }

    public void Show(string message)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('" + message + "');", true);
    }

    protected void gvRoadTax_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:
                var lblRtValidityPeriod = (Label) e.Row.FindControl("lblRTValidityPeriod");
                var lblRtValidityPeriodText = (Label) e.Row.FindControl("lblRTValidityPeriodText");
                switch (lblRtValidityPeriod.Text)
                {
                    case "3":
                        lblRtValidityPeriodText.Text = "3 Months";
                        break;
                    case "6":
                        lblRtValidityPeriodText.Text = "6 Months";
                        break;
                    case "9":
                        lblRtValidityPeriodText.Text = "9 Months";
                        break;
                    default:
                        lblRtValidityPeriodText.Text = "1 Yrs";
                        break;
                }

                break;
        }
    }

    protected void gvRoadTax_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvRoadTax.PageIndex = e.NewPageIndex;
        GetRoadTax();
    }

    protected void txtRoadTaxValidityStartDate_TextChanged(object sender, EventArgs e)
    {
        ddlRoadTaxValidityPeriod.SelectedIndex = 0;
        txtRoadTaxValidityEndDate.Text = "";
    }

    protected void ddlVehicleNumber_SelectedIndexChanged(object sender, EventArgs e)
    {
        var dates = _fmsGeneral.GetRegistrationDate(int.Parse(ddlVehicleNumber.SelectedItem.Value));
        var dt = Convert.ToDateTime(dates.Tables[0].Rows[0]["RegDate"].ToString());
        vehicleRegistrationDate.Value = dt.ToString(CultureInfo.InvariantCulture);
    }
}