using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using GvkFMSAPP.BLL;

public partial class VehicleDecommission : Page
{
    private readonly FMSGeneral _fmsgenobj = new FMSGeneral();
    private readonly BaseVehicleDetails _fmsobj = new BaseVehicleDetails();
    private readonly Helper _helper = new Helper();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Name"] == null) Response.Redirect("Login.aspx");
        if (!IsPostBack)
        {
            btnSubmit.Attributes.Add("onclick", "return validation()");
            GetDistrict();
            GetDecommVehicleDetails();
        }
    }

    public void GetDistrict()
    {
        try
        {
            var ds = _fmsobj.GetDistrict();
            if (ds != null) _helper.FillDropDownHelperMethodWithDataSet(ds, "ds_lname", "ds_dsid", ddlDistrict);
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    protected void ddlDistrict_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            _fmsgenobj.UserDistrictId = int.Parse(ddlDistrict.SelectedItem.Value);
            var ds = _fmsgenobj.GetVehicleForDecomm();
            _helper.FillDropDownHelperMethodWithDataSet(ds, "VehicleNumber", "VehicleID", null, ddlVehicleNumber);
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        ClearAll();
    }

    private void ClearAll()
    {
        ddlDistrict.SelectedIndex = 0;
        ddlVehicleNumber.SelectedIndex = 0;
        txtDecommReason.Text = "";
        txtDecommDate.Text = "";
        txtDecommRemark.Text = "";
        ddlVehicleNumber.Visible = true;
        txtVehicleNumber.Visible = false;
        ddlDistrict.Visible = true;
        txtDistrict.Visible = false;
        btnSubmit.Text = "Submit";
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        _fmsgenobj.DecommReason = txtDecommReason.Text;
        _fmsgenobj.DecommDate = Convert.ToDateTime(txtDecommDate.Text);
        _fmsgenobj.DecommRemark = txtDecommRemark.Text;
        switch (btnSubmit.Text)
        {
            case "Submit":
                _fmsgenobj.UserDistrictId = Convert.ToInt16(ddlDistrict.SelectedItem.Value);
                _fmsgenobj.VehicleId = Convert.ToInt32(ddlVehicleNumber.SelectedItem.Value);
                var decommres = _fmsgenobj.InsVehicleDecommDetails();
                Show(decommres != 0 ? "Record Inserted Successfully!!" : "Error!!");
                break;
            default:
                _fmsgenobj.VehicleDecommId = Convert.ToInt32(ViewState["VehDecommId"]);
                var decommupdtres = _fmsgenobj.UpdtVehicleDecommDetails();
                Show(decommupdtres != 0 ? "Record Updated Successfully!!" : "Error!!");
                break;
        }

        ClearAll();
        GetDecommVehicleDetails();
    }

    public void Show(string message)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('" + message + "');", true);
    }

    private void GetDecommVehicleDetails()
    {
        ViewState["DecommVehDet"] = _fmsgenobj.GetDecommVehicleDetails();
        grdvwDecommVehicles.DataSource = (DataSet) ViewState["DecommVehDet"];
        grdvwDecommVehicles.DataBind();
    }

    protected void grdvwDecommVehicles_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdvwDecommVehicles.PageIndex = e.NewPageIndex;
        GetDecommVehicleDetails();
    }

    protected void grdvwDecommVehicles_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "DecommVehEdit":
                ddlVehicleNumber.Visible = false;
                txtVehicleNumber.Visible = true;
                ddlDistrict.Visible = false;
                txtDistrict.Visible = true;
                var dv = new DataView(((DataSet) ViewState["DecommVehDet"]).Tables[0], "VehicleDecommId ='" + e.CommandArgument + "'", "VehicleDecommId", DataViewRowState.CurrentRows);
                ViewState["VehDecommId"] = e.CommandArgument;
                txtDistrict.Text = dv[0]["District"].ToString();
                txtVehicleNumber.Text = dv[0]["VehicleNumber"].ToString();
                txtDecommReason.Text = dv[0]["DecommReason"].ToString();
                txtDecommDate.Text = dv[0]["DecommDate"].ToString();
                txtDecommRemark.Text = dv[0]["DecommRemark"].ToString();
                btnSubmit.Text = "Update";
                break;
            case "DecommVehRevert":
                _fmsgenobj.VehicleDecommId = Convert.ToInt32(e.CommandArgument);
                var revertvehdecommres = _fmsgenobj.RevertVehicleDecommDetails();
                Show(revertvehdecommres != 0 ? "Record Reverted Successfully!!" : "Error!!");
                GetDecommVehicleDetails();
                break;
        }
    }
}