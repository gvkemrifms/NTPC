using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using GvkFMSAPP.BLL;
using GvkFMSAPP.BLL.VehicleMaintenance;

public partial class VehicleScheduleServiceRequest : Page
{
    private readonly FMSGeneral _fmsg = new FMSGeneral();
    private readonly Helper _helper = new Helper();
    private readonly VehicleMaintenance _vehMain = new VehicleMaintenance();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
            switch (Request.QueryString["VehID"])
            {
                case null:
                    if (Session["User_Name"] == null) Response.Redirect("Login.aspx");
                    txtSchedulePlanDate.Attributes.Add("onkeypress", "return false");
                    FillVehicles();
                    FillDropDownList();
                    break;
                default:
                    if (Request.QueryString["VehID"] != "" || Request.QueryString["VehID"] != null)
                    {
                        FillVehicles();
                        ddlVehicleNo.SelectedValue = Request.QueryString["VehID"];
                        FillDropDownList();
                    }

                    break;
            }
    }

    private void FillVehicles()
    {
        try
        {
            if (Session["UserdistrictId"] != null) _fmsg.UserDistrictId = Convert.ToInt32(Session["UserdistrictId"].ToString());
            var o = Session["UserdistrictId"];
            if (o != null)
            {
                var ds = _vehMain.GetVehicleNumber(Convert.ToInt32(o.ToString()));
                if (ds != null) _helper.FillDropDownHelperMethodWithDataSet(ds, "VehicleNumber", "VehicleID", ddlVehicleNo);
            }
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    private void FillDropDownList()
    {
        try
        {
            var ds = _fmsg.GetMaintenanceType();
            if (ds == null) throw new ArgumentNullException(nameof(ds));
            _helper.FillDropDownHelperMethodWithDataSet(ds, "Maint_Desc", "Maint_Type_ID", ddlScheduleCat);
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        int vehicleId;
        int scheduledCat;
        string scheduledCatName;
        var status = string.Empty;
        int res;
        switch (btnSubmit.Text)
        {
            case "Submit":
                vehicleId = Convert.ToInt32(ddlVehicleNo.SelectedValue);
                scheduledCat = Convert.ToInt32(ddlScheduleCat.SelectedValue);
                scheduledCatName = ddlScheduleCat.SelectedItem.Text;
                var scheduledPlanDate = Convert.ToDateTime(txtSchedulePlanDate.Text);
                var ds = _fmsg.GetRegistrationDate(vehicleId);
                if (scheduledPlanDate > Convert.ToDateTime(ds.Tables[0].Rows[0][0].ToString()) && scheduledPlanDate >= DateTime.Now)
                {
                    status = "Pending";
                    var createdBy = "user";
                    var creationdate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                    res = _vehMain.InsServiceRequestDetails(vehicleId, scheduledCat, scheduledCatName, scheduledPlanDate, status, creationdate, createdBy);
                    switch (res)
                    {
                        case 1:
                            ResetScheduleDetails();
                            Show("Service Request Details Submitted successfully");
                            break;
                        default:
                            ResetScheduleDetails();
                            Show("Service Request Details already exists");
                            break;
                    }

                    FillScheduleServiceRequestGrid(vehicleId);
                }
                else
                {
                    Show("Please select scheduled plan date greater than vehicle registration date ");
                }

                break;
            case "Update":
                vehicleId = Convert.ToInt32(ddlVehicleNo.SelectedValue);
                scheduledCat = Convert.ToInt32(ddlScheduleCat.SelectedValue);
                scheduledCatName = ddlScheduleCat.SelectedItem.Text;
                var scheduledPlanDate1 = Convert.ToDateTime(txtSchedulePlanDate.Text);
                var updatedBy = "user";
                var updateddate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                var slno = Convert.ToInt32(Session["SlNo"].ToString());
                res = _vehMain.UpdateServiceRequestDetails(vehicleId, slno, scheduledCat, scheduledCatName, scheduledPlanDate1, status, updateddate, updatedBy);
                switch (res)
                {
                    case 1:
                        ResetScheduleDetails();
                        Show("Service Request Details Updated successfully");
                        break;
                    default:
                        ResetScheduleDetails();
                        Show("Service Request Details already exists");
                        break;
                }

                FillScheduleServiceRequestGrid(vehicleId);
                btnSubmit.Text = "Submit";
                break;
        }
    }

    private void ResetScheduleDetails()
    {
        ddlVehicleNo.SelectedIndex = 0;
        ddlScheduleCat.SelectedIndex = 0;
        txtSchedulePlanDate.Text = "";
    }

    public void FillScheduleServiceRequestGrid(int vehicleId)
    {
        var ds = _vehMain.IBind_ServiceRequestDetails(vehicleId);
        if (ds == null)
        {
            pnlDisplayDetails.Visible = false;
        }
        else
        {
            if (ds.Tables[0].Rows.Count <= 0)
            {
                pnlDisplayDetails.Visible = false;
            }
            else
            {
                pnlDisplayDetails.Visible = true;
                grvScheduleServiceRequest.DataSource = ds.Tables[0];
                grvScheduleServiceRequest.DataBind();
            }
        }
    }

    protected void ddlVehicleNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlVehicleNo.SelectedValue == null) return;
        // SetInitialRow();
        FillScheduleServiceRequestGrid(Convert.ToInt32(ddlVehicleNo.SelectedValue));
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ddlVehicleNo.SelectedIndex = 0;
        ddlScheduleCat.SelectedIndex = 0;
        txtSchedulePlanDate.Text = "";
        pnlDisplayDetails.Visible = false;
        btnSubmit.Text = "Submit";
    }

    protected void grvScheduleServiceRequest_RowEditing(object sender, GridViewEditEventArgs e)
    {
        var index = e.NewEditIndex;
        var lblId = (Label) grvScheduleServiceRequest.Rows[index].Cells[1].FindControl("lblVehicleID");
        var lblSlNo = (Label) grvScheduleServiceRequest.Rows[index].Cells[0].FindControl("lblSlNo");
        var id = Convert.ToInt32(lblId.Text);
        var slNo = Convert.ToInt32(lblSlNo.Text);
        Session["SlNo"] = slNo.ToString();
        var ds = _vehMain.IGetServiceRequestDetailsToUpdate(id, slNo);
        if (ds != null)
        {
            ddlVehicleNo.SelectedValue = ds.Tables[0].Rows[0][1].ToString();
            ddlScheduleCat.SelectedValue = ds.Tables[0].Rows[0][3].ToString();
            txtSchedulePlanDate.Text = ds.Tables[0].Rows[0][4].ToString();
        }

        btnSubmit.Text = "Update";
    }

    protected void lnkDelete_Click(object sender, CommandEventArgs e)
    {
        var args = e.CommandArgument.ToString().Split(',');
        var vehicleId = Convert.ToInt32(args[1]);
        var slno = Convert.ToInt32(args[0]);
        var res = _vehMain.IDeleteServiceRequestDetails(slno);
        if (res == 0) return;
        FillScheduleServiceRequestGrid(vehicleId);
        ResetScheduleDetails();
        btnSubmit.Text = "Submit";
    }

    protected void grvScheduleServiceRequest_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
    }

    public void Show(string message)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('" + message + "');", true);
    }

    protected void grvScheduleServiceRequest_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grvScheduleServiceRequest.PageIndex = e.NewPageIndex;
        FillScheduleServiceRequestGrid(Convert.ToInt32(ddlVehicleNo.SelectedValue));
    }
}