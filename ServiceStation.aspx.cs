using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using GvkFMSAPP.BLL;

public partial class ServiceStation : Page
{
    private readonly FMSGeneral _fmsg = new FMSGeneral();
    private readonly BaseVehicleDetails _fmsobj = new BaseVehicleDetails();
    private readonly Helper _helper = new Helper();
    private DataSet _ds = new DataSet();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Name"] == null) Response.Redirect("Login.aspx");
        if (!IsPostBack)
        {
            if (Session["UserdistrictId"] != null) _fmsg.UserDistrictId = Convert.ToInt32(Session["UserdistrictId"].ToString());
            Bindgrid();
            FillVehicles();
            BindData();
            btnUpdate.Visible = false;
            txtServiceSrationName.Attributes.Add("onkeypress", "javascript:return OnlyAlphabets(this,event)");
        }
    }

    private void FillVehicles()
    {
        try
        {
            _ds = null;
            _ds = _fmsg.GetVehicleNumber();
            _helper.FillDropDownHelperMethodWithDataSet(_ds, "VehicleNumber", "VehicleID", null, ddlVehicleNumber);
            ViewState["dsVehicles"] = _ds;
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    public void BindData()
    {
        try
        {
            _ds = null;
            _ds = _fmsobj.GetDistricts_new();
            _helper.FillDropDownHelperMethodWithDataSet(_ds, "district_name", "district_id", ddlDistricts);
            ViewState["dsDistricts"] = _ds;
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    public void Bindgrid()
    {
        var ds = _fmsobj.GetGridServiceNames();
        if (ds != null)
        {
            gvServiceStationDetails.DataSource = ds.Tables[0];
            gvServiceStationDetails.DataBind();
        }

        ViewState["dsGrid"] = ds;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        _fmsobj.VehicleId = Convert.ToInt16(ddlVehicleNumber.SelectedValue);
        _fmsobj.ServiceName = txtServiceSrationName.Text;
        _fmsobj.DistrictId = Convert.ToInt16(ddlDistricts.SelectedValue);
        var output = _fmsobj.InsertServiceName();
        if (output <= 0)
        {
            Show("Not Inserted");
        }
        else
        {
            Show("Inserted Succesfully");
            ClearAll();
            Bindgrid();
        }
    }

    private void ClearAll()
    {
        txtServiceSrationName.Text = "";
        ddlDistricts.SelectedIndex = 0;
        ddlVehicleNumber.SelectedIndex = 0;
    }

    public void Show(string message)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('" + message + "');", true);
    }

    protected void gvServiceStationDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == null) return;
        switch (e.CommandName)
        {
            case "MainEdit":
            {
                btnSave.Visible = false;
                btnUpdate.Visible = true;
                //int index = Convert.ToInt32(e.CommandArgument.ToString());
                var gvr = (GridViewRow) ((LinkButton) e.CommandSource).NamingContainer;
                var index = gvr.RowIndex;
                txtServiceSrationName.Text = ((Label) gvServiceStationDetails.Rows[index].FindControl("lblServiceStation")).Text;
                var dsDist = (DataSet) ViewState["dsDistricts"];
                var dvDistrict = dsDist.Tables[0].DefaultView;
                dvDistrict.RowFilter = "ds_lname='" + ((Label) gvServiceStationDetails.Rows[index].FindControl("lblDistricts")).Text + "'";
                ddlDistricts.SelectedValue = Convert.ToString(dvDistrict.ToTable().Rows[0]["ds_dsid"]);
                var dsVeh = (DataSet) ViewState["dsVehicles"];
                var dvVehicles = dsVeh.Tables[0].DefaultView;
                dvVehicles.RowFilter = "VehicleNumber='" + ((Label) gvServiceStationDetails.Rows[index].FindControl("lblVehNum")).Text + "'";
                ddlVehicleNumber.SelectedValue = Convert.ToString(dvVehicles.ToTable().Rows[0]["VehicleID"]);
                var ds1 = (DataSet) ViewState["dsGrid"];
                var dv = new DataView(ds1.Tables[0]) {RowFilter = "ServiceStation_Name='" + txtServiceSrationName.Text + "' and ds_lname='" + ddlDistricts.SelectedItem.Text + "' and VehicleNumber='" + ddlVehicleNumber.SelectedItem.Text + "'"};
                var dt = dv.ToTable();
                Session["Id"] = Convert.ToString(dt.Rows[0]["Id"]);
                break;
            }
            case "MainDelete":
            {
                var ds2 = (DataSet) ViewState["dsGrid"];
                var dv = new DataView(ds2.Tables[0]);
                var gvr = (GridViewRow) ((LinkButton) e.CommandSource).NamingContainer;
                var index = gvr.RowIndex;
                txtServiceSrationName.Text = ((Label) gvServiceStationDetails.Rows[index].FindControl("lblServiceStation")).Text;
                var dsDist = (DataSet) ViewState["dsDistricts"];
                var dvDistrict = dsDist.Tables[0].DefaultView;
                dvDistrict.RowFilter = "ds_lname='" + ((Label) gvServiceStationDetails.Rows[index].FindControl("lblDistricts")).Text + "'";
                ddlDistricts.SelectedValue = Convert.ToString(dvDistrict.ToTable().Rows[0]["ds_dsid"]);
                var dsVeh = (DataSet) ViewState["dsVehicles"];
                var dvVehicles = dsVeh.Tables[0].DefaultView;
                dvVehicles.RowFilter = "VehicleNumber='" + ((Label) gvServiceStationDetails.Rows[index].FindControl("lblVehNum")).Text + "'";
                ddlVehicleNumber.SelectedValue = Convert.ToString(dvVehicles.ToTable().Rows[0]["VehicleID"]);
                dv.RowFilter = "ServiceStation_Name='" + txtServiceSrationName.Text + "' and ds_lname='" + ddlDistricts.SelectedItem.Text + "' and VehicleNumber='" + ddlVehicleNumber.SelectedItem.Text + "'";
                var dt = dv.ToTable();
                Session["Id"] = Convert.ToString(dt.Rows[0]["Id"]);
                _fmsobj.GridId = Convert.ToInt16(Session["Id"]);
                var delres = _fmsobj.DeleteServiceName();
                Show(delres != 0 ? "Record Deleted Successfully!!" : "Error!!");
                ClearAll();
                Bindgrid();
                break;
            }
        }
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        var id = Session["Id"] == null ? 0 : Convert.ToInt16(Session["Id"]);
        _fmsobj.ServiceName = txtServiceSrationName.Text;
        _fmsobj.DistrictId = Convert.ToInt16(ddlDistricts.SelectedValue);
        _fmsobj.VehicleId = Convert.ToInt16(ddlVehicleNumber.SelectedValue);
        _fmsobj.GridId = id;
        var ds1 = _fmsobj.CheckServiceName();
        var isExists = false;
        if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0 && Convert.ToString(ds1.Tables[0].Rows[0]["Vehicle_ID"]) == ddlVehicleNumber.SelectedValue)
        {
            Show("Service Station already exists for Selected Vehicle");
            isExists = true;
            // ClearAll();
        }

        if (isExists) return;
        var output = _fmsobj.UpdServiceName();
        if (output <= 0)
        {
            Show("Not Inserted");
        }
        else
        {
            Show("Updated Succesfully");
            ClearAll();
            Bindgrid();
            btnUpdate.Visible = false;
            btnSave.Visible = true;
        }
    }

    protected void ddlVehicleNumber_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillDistrict();
        FillBunks();
    }

    private void FillDistrict()
    {
        _fmsg.vehicle = ddlVehicleNumber.SelectedItem.ToString();
        var dsDistrict = _fmsg.GetDistrictLoc();
        if (dsDistrict == null) throw new ArgumentNullException(nameof(dsDistrict));
        BindData();
        if (dsDistrict.Tables[0].Rows.Count >= 0)
            ddlDistricts.Enabled = true;
        else
            ddlDistricts.Items.FindByText(dsDistrict.Tables[0].Rows[0]["District"].ToString()).Selected = true;
    }

    private void FillBunks()
    {
        _fmsg.VehicleId = Convert.ToInt16(ddlVehicleNumber.SelectedValue);
        var dsServiceNames = _fmsg.GetBunkNames();
        txtServiceSrationName.Text = dsServiceNames.Tables[0].Rows.Count > 0 ? dsServiceNames.Tables[0].Rows[0]["ServiceStnName"].ToString() : "";
    }

    protected void gvServiceStationDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvServiceStationDetails.PageIndex = e.NewPageIndex;
        Bindgrid();
    }
}