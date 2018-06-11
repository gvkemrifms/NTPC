using System;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using GvkFMSAPP.BLL;
using GvkFMSAPP.PL;

public partial class FabricatedVehicleDetails : Page
{
    private readonly GvkFMSAPP.BLL.FabricatedVehicleDetails _fabricatedvehicledet = new GvkFMSAPP.BLL.FabricatedVehicleDetails();
    private readonly FMSGeneral _fmsGeneral = new FMSGeneral();
    private readonly Helper _helper = new Helper();
    private int _ret;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Name"] == null) Response.Redirect("Login.aspx");
        if (!IsPostBack)
        {
            var dsPerms = (DataSet) Session["PermissionsDS"];
            if (dsPerms == null) throw new ArgumentNullException(nameof(dsPerms));
            dsPerms.Tables[0].DefaultView.RowFilter = "Url='" + Page.Request.Url.Segments[Page.Request.Url.Segments.Length - 1] + "'";
            var p = new PagePermissions(dsPerms, dsPerms.Tables[0].DefaultView[0]["Url"].ToString(), dsPerms.Tables[0].DefaultView[0]["Title"].ToString());
            btSave.Attributes.Add("onclick", "return validation()");
            GetFabricatedVehicleDetails();
            GetTrNo();
            GetFabricatorName();
            ViewState["Add"] = 0;
            pnlFabricatedVehicleDetails.Visible = false;
            gvFabricatedVehicleDetails.Visible = false;
            if (p.View)
            {
                gvFabricatedVehicleDetails.Visible = true;
                gvFabricatedVehicleDetails.Columns[6].Visible = false;
                gvFabricatedVehicleDetails.Columns[5].Visible = false;
            }

            if (p.Add)
            {
                pnlFabricatedVehicleDetails.Visible = true;
                gvFabricatedVehicleDetails.Visible = true;
                gvFabricatedVehicleDetails.Columns[6].Visible = false;
                gvFabricatedVehicleDetails.Columns[5].Visible = false;
                ViewState["Add"] = 1;
            }

            if (p.Modify)
            {
                gvFabricatedVehicleDetails.Visible = true;
                gvFabricatedVehicleDetails.Columns[6].Visible = true;
                gvFabricatedVehicleDetails.Columns[5].Visible = true;
            }
        }
    }

    public void GetFabricatedVehicleDetails()
    {
        if (_fabricatedvehicledet != null) gvFabricatedVehicleDetails.DataSource = _fabricatedvehicledet.GetFabricatedVehicleDetails();
        gvFabricatedVehicleDetails.DataBind();
    }

    public void GetTrNo()
    {
        if (_fabricatedvehicledet != null)
            try
            {
                var ds = _fabricatedvehicledet.GetTRNo();
                if (ds == null) return;
                _helper.FillDropDownHelperMethodWithDataSet(ds, "TRNo", "VehicleID", ddlTRNo);
            }
            catch (Exception ex)
            {
                _helper.ErrorsEntry(ex);
            }
    }

    public void GetFabricatorName()
    {
        if (_fabricatedvehicledet != null)
        {
            try
            {
                var ds = _fabricatedvehicledet.GetFabicatorName();
                if (ds == null) return;
                _helper.FillDropDownHelperMethodWithDataSet(ds, "FleetFabricator_Name", "FleetFabricator_Id", ddlFabricatorName);
            }
            catch (Exception ex)
            {
                _helper.ErrorsEntry(ex);
            }

            ;
        }
    }

    protected void btSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["FabricatedVehicleDetID"] != null) _fabricatedvehicledet.FabricatedVehicleDetID = int.Parse(ViewState["FabricatedVehicleDetID"].ToString());
            _fabricatedvehicledet.FabricatorName = ddlFabricatorName.SelectedItem.Value;
            _fabricatedvehicledet.InvoiceNo = txtInvoiceNo.Text;
            _fabricatedvehicledet.InvoiceDate = DateTime.ParseExact(txtInvoiceDate.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            _fabricatedvehicledet.FabricationCost = float.Parse(txtFabricationCost.Text);
            _fabricatedvehicledet.VehicleHandoverToFabricatorDate = DateTime.ParseExact(txtVehicleHandoverDate.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            _fabricatedvehicledet.FabricationCompletionDate = DateTime.ParseExact(txtFabricationCompDate.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            _fabricatedvehicledet.FVDInspectedBy = txtInspecetedBy.Text;
            _fabricatedvehicledet.FVDInspectedDate = DateTime.ParseExact(txtInspectionDate.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            var valfabvel = _fabricatedvehicledet.ValidateFabricatedVehicleDet();
            if (valfabvel.Tables[0].Rows.Count > 0)
            {
                Show("Data is already present for this vehicle");
            }
            else
            {
                switch (btSave.Text)
                {
                    case "Save":
                        _fabricatedvehicledet.VehicleID = int.Parse(ddlTRNo.SelectedItem.Value);
                        _ret = _fabricatedvehicledet.InsFabricatedVehicleDetails();
                        break;
                    case "Update":
                        _fabricatedvehicledet.VehicleID = int.Parse(ViewState["VehId"].ToString());
                        _ret = _fabricatedvehicledet.UpdtFabricatedVehicleDetails();
                        btSave.Text = "Save";
                        ddlTRNo.Visible = true;
                        txtTrNo.Visible = false;
                        if (int.Parse(ViewState["Add"].ToString()) == 0) pnlFabricatedVehicleDetails.Visible = false;
                        break;
                }

                if (ViewState["FabricatedVehicleDetID"] == null)
                {
                    Show("Record Inserted Successfully");
                }
                else
                {
                    Show("Record Updated Successfully");
                    ViewState["FabricatedVehicleDetID"] = null;
                }
            }

            ClearControls();
            GetFabricatedVehicleDetails();
            GetTrNo();
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    protected void btReset_Click(object sender, EventArgs e)
    {
        ClearControls();
        ddlTRNo.Visible = true;
        txtTrNo.Visible = false;
    }

    public void Show(string message)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('" + message + "');", true);
    }

    protected void gridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvFabricatedVehicleDetails.PageIndex = e.NewPageIndex;
        GetFabricatedVehicleDetails();
    }

    protected void gridView_Sorting(object sender, GridViewSortEventArgs e)
    {
        var dataTable = (DataTable) gvFabricatedVehicleDetails.DataSource;
        if (dataTable == null) return;
        GetFabricatedVehicleDetails();
    }

    protected void ClearControls()
    {
        ddlTRNo.ClearSelection();
        ddlFabricatorName.ClearSelection();
        txtInvoiceNo.Text = "";
        txtInvoiceDate.Text = "";
        txtFabricationCost.Text = "";
        txtVehicleHandoverDate.Text = "";
        txtFabricationCompDate.Text = "";
        txtInspecetedBy.Text = "";
        txtInspectionDate.Text = "";
        btSave.Text = "Save";
    }

    protected void gvFabricatedVehicleDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName != null)
            try
            {
                switch (e.CommandName)
                {
                    case "fabVehEdit":
                        ViewState["FabricatedVehicleDetID"] = e.CommandArgument.ToString();
                        var dsedit = _fabricatedvehicledet.GetFabricatedVehicleDetails();
                        var dr = dsedit.Tables[0].Select("FabricatedVehicleDetID=" + e.CommandArgument);
                        ClearControls();
                        ddlTRNo.Visible = false;
                        txtTrNo.Visible = true;
                        txtTrNo.Text = dr[0][11].ToString();
                        ViewState["VehId"] = Convert.ToInt16(dr[0][1].ToString());
                        ddlFabricatorName.Items.FindByValue(dr[0][2].ToString()).Selected = true;
                        txtInvoiceNo.Text = dr[0][3].ToString();
                        txtInvoiceDate.Text = dr[0][4].ToString();
                        txtFabricationCost.Text = dr[0][5].ToString();
                        txtVehicleHandoverDate.Text = dr[0][6].ToString();
                        txtFabricationCompDate.Text = dr[0][7].ToString();
                        txtInspecetedBy.Text = dr[0][8].ToString();
                        txtInspectionDate.Text = dr[0][9].ToString();
                        var dtUp = _fmsGeneral.GetPurchaseDate(int.Parse(ViewState["VehId"].ToString()));
                        vehiclePurchaseDate.Value = dtUp.ToString(CultureInfo.InvariantCulture);
                        pnlFabricatedVehicleDetails.Visible = true;
                        btSave.Text = "Update";
                        break;
                    case "fabVehDelete":
                        _fabricatedvehicledet.FabricatedVehicleDetID = int.Parse(e.CommandArgument.ToString());
                        var output = _fabricatedvehicledet.ValidateRegVehicle();
                        switch (output)
                        {
                            case 0:
                                _ret = _fabricatedvehicledet.DelFabricatedVehicleDetails();
                                Show(_ret == 1 ? "Record Deleted Successfully" : "Error");
                                break;
                            default:
                                Show("Vehicle Registation has been completed, can not delete");
                                break;
                        }

                        ViewState["FabricatedVehicleDetID"] = null;
                        ClearControls();
                        GetFabricatedVehicleDetails();
                        GetTrNo();
                        break;
                }
            }
            catch (Exception ex)
            {
                _helper.ErrorsEntry(ex);
            }
    }

    protected void ddlTRNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlTRNo.SelectedIndex == 0) return;
        if (_fmsGeneral != null)
        {
            var dt = _fmsGeneral.GetPurchaseDate(int.Parse(ddlTRNo.SelectedItem.Value));
            vehiclePurchaseDate.Value = dt.ToString(CultureInfo.InvariantCulture);
        }
    }
}