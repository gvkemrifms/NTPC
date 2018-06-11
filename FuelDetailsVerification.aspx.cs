using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using GvkFMSAPP.BLL;
public partial class FuelDetailsVerification : Page
{
    private readonly FMSGeneral _fmsg = new FMSGeneral();
    private readonly Helper _helper = new Helper();
    public IFuelManagement Objfuelver = new FuelManagement();

    protected void Page_Load(object sender,EventArgs e)
    {
        if (Session["User_Name"] == null) Response.Redirect("Login.aspx");
        if (!IsPostBack)
        {
            if (Session["UserdistrictId"] != null) _fmsg.UserDistrictId = Convert.ToInt32(Session["UserdistrictId"].ToString());
            FillVehicles();
            var dsPerms = (DataSet) Session["PermissionsDS"];
            if (dsPerms == null) throw new ArgumentNullException(nameof(dsPerms));
            dsPerms.Tables[0].DefaultView.RowFilter = "Url='" + Page.Request.Url.Segments[Page.Request.Url.Segments.Length - 1] + "'";
        }
    }

    private void FillVehicles()
    {
        var ds = _fmsg.GetVehicleNumber();
        if (ds == null) return;
        try
        {
            _helper.FillDropDownHelperMethodWithDataSet(ds,"VehicleNumber","VehicleID",ddlVehicleNumber);
            ddlVehicleNumber.Enabled = true;
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    private void FillGridVerification()
    {
        var districtId = -1;
        var vehicleId = -1;
        if (Session["UserdistrictId"] != null) districtId = Convert.ToInt32(Session["UserdistrictId"].ToString());
        if (ddlVehicleNumber.SelectedValue != null) vehicleId = Convert.ToInt32(ddlVehicleNumber.SelectedValue);
        var ds = Objfuelver.IFillGridVerification(districtId,vehicleId);
        if (ds == null) throw new ArgumentNullException(nameof(ds));
        switch (ds.Tables[0].Rows.Count)
        {
            case 0:
                gvVerification.DataSource = ds;
                gvVerification.DataBind();
                break;
            default:
                gvVerification.DataSource = ds;
                gvVerification.DataBind();
                break;
        }
    }

    protected void btnApprove_Click(object sender,EventArgs e)
    {
    }

    protected void Approve_Click(object sender,EventArgs e)
    {
        var i1 = 0;
        var i2 = 0;
        var fuelEntryId = string.Empty;
        var fuelReconcilationId = string.Empty;
        var emptyRemarkId = string.Empty;
        foreach (GridViewRow item in gvVerification.Rows)
            if (((CheckBox) item.FindControl("checkSelect")).Checked)
            {
                var remText = (TextBox) item.FindControl("txtRemarks");
                var lblId = (Label) item.FindControl("lblId");
                fuelEntryId = fuelEntryId == string.Empty ? lblId.Text : fuelEntryId + "," + lblId.Text;
                i1++;
                if (remText.Text != string.Empty) continue;
                emptyRemarkId = Convert.ToString(remText.ClientID);
                break;
            }

        foreach (GridViewRow item in gvReconcilation.Rows)
            if (((CheckBox) item.FindControl("checkSelect")).Checked)
            {
                var rblid = (Label) item.FindControl("Rblid");
                fuelReconcilationId = fuelReconcilationId == string.Empty ? rblid.Text : fuelReconcilationId + "," + rblid.Text;
                i2++;
            }

        if (i1 <= 0 || i2 <= 0 || i1 != i2)
            Show("Please Select Reconciliation Details and Approve");
        else
            ScriptManager.RegisterStartupScript(this,GetType(),"msg1","ConfirmApprove('" + btnHdnApprove.ClientID + "','" + emptyRemarkId + "');",true);
    }

    private void ClearControls()
    {
        foreach (GridViewRow item in gvVerification.Rows)
        foreach (GridViewRow item1 in gvReconcilation.Rows)
        {
            var c = (CheckBox) item.FindControl("checkSelect");
            var c1 = (CheckBox) item1.FindControl("checkSelect");
            c.Checked = false;
            c1.Checked = false;
        }
    }

    private int[] SplitFuelIDs(string fuelEntryId)
    {
        var ds = fuelEntryId.Split(',');
        var fuelIDs = new int[ds.Length];
        for (var i = 0; i < ds.Length; i++) fuelIDs[i] = Convert.ToInt32(ds[i]);
        return fuelIDs;
    }

    protected void gvVerification_RowEditing(object sender,GridViewEditEventArgs e)
    {
    }

    protected void Reject_Click(object sender,EventArgs e)
    {
        var i1 = 0;
        var fuelEntryId = string.Empty;
        var emptyRemarkId = string.Empty;
        foreach (GridViewRow item in gvVerification.Rows)
            if (((CheckBox) item.FindControl("checkSelect")).Checked)
            {
                var remText = (TextBox) item.FindControl("txtRemarks");
                var lblId = (Label) item.FindControl("lblId");
                fuelEntryId = fuelEntryId == string.Empty ? lblId.Text : fuelEntryId + "," + lblId.Text;
                i1++;
                if (remText.Text != string.Empty) continue;
                emptyRemarkId = Convert.ToString(remText.ClientID);
                break;
            }

        if (i1 <= 0)
            Show("Please Select and Reject");
        else
            ScriptManager.RegisterStartupScript(this,GetType(),"msg1","ConfirmReject('" + btnHdnReject.ClientID + "','" + emptyRemarkId + "');",true);
    }

    public void Show(string message)
    {
        ScriptManager.RegisterStartupScript(this,GetType(),"msg","alert('" + message + "');",true);
    }

    protected void btnHdnApprove_Click(object sender,EventArgs e)
    {
        var i1 = 0;
        var i2 = 0;
        var fuelEntryId = string.Empty;
        var fuelReconcilationId = string.Empty;
        foreach (GridViewRow item in gvVerification.Rows)
            if (((CheckBox) item.FindControl("checkSelect")).Checked)
            {
                var remText = (TextBox) item.FindControl("txtRemarks");
                var lblId = (Label) item.FindControl("lblId");
                fuelEntryId = fuelEntryId == string.Empty ? lblId.Text : fuelEntryId + "," + lblId.Text;
                i1++;
                if (remText.Text != string.Empty) continue;
                break;
            }

        foreach (GridViewRow item in gvReconcilation.Rows)
            if (((CheckBox) item.FindControl("checkSelect")).Checked)
            {
                var rblid = (Label) item.FindControl("Rblid");
                fuelReconcilationId = fuelReconcilationId == string.Empty ? rblid.Text : fuelReconcilationId + "," + rblid.Text;
                i2++;
            }

        if (i1 > 0 && i2 > 0 && i1 == i2)
        {
            var fuelIDs = SplitFuelIDs(fuelEntryId);
            var reconcilationIDs = SplitFuelIDs(fuelReconcilationId);
            var success = new int[fuelIDs.Length];
            var success1 = new int[reconcilationIDs.Length];
            foreach (GridViewRow item in gvVerification.Rows)
                for (var i = 0; i < fuelIDs.Length; i++)
                {
                    var remarks = ((TextBox) item.FindControl("txtRemarks")).Text;
                    var chk = (CheckBox) item.FindControl("checkSelect");
                    if (chk.Checked)
                        if (remarks != string.Empty)
                        {
                            btnApprove_Click(sender,e);
                            success[i] = Objfuelver.IApproveStatus(fuelIDs[i],remarks);
                        }
                        else
                        {
                            var strFmsScript = "Please Enter Remarks";
                            Show(strFmsScript);
                            break;
                        }
                }

            foreach (GridViewRow item in gvReconcilation.Rows)
                for (var i = 0; i < reconcilationIDs.Length; i++)
                {
                    var chk = (CheckBox) item.FindControl("checkSelect");
                    if (chk.Checked)
                    {
                        btnApprove_Click(sender,e);
                        success1[i] = Objfuelver.IApproveReconcilation(reconcilationIDs[i]);
                    }
                }

            var count = 0;
            var count1 = 0;
            foreach (var item in success)
                if (item == 1)
                    count++;
            foreach (var item in success1)
                if (item == 1)
                    count1++;
            if (count == success.Length && count1 == success1.Length)
            {
                var strFmsScript = "Fuel Entry Approved";
                Show(strFmsScript);
                FillGridVerification();
                FillGridReconcilation();
                ClearControls();
            }
            else
            {
                var strFmsScript = "Failure";
                Show(strFmsScript);
            }

            FillGridVerification();
            FillGridReconcilation();
            ClearControls();
        }
    }

    protected void btnHdnReject_Click(object sender,EventArgs e)
    {
        var i1 = 0;
        var fuelEntryId = string.Empty;
        foreach (GridViewRow item in gvVerification.Rows)
            if (((CheckBox) item.FindControl("checkSelect")).Checked)
            {
                var lblId = (Label) item.FindControl("lblId");
                fuelEntryId = fuelEntryId == string.Empty ? lblId.Text : fuelEntryId + "," + lblId.Text;
                i1++;
            }

        if (i1 > 0)
        {
            var fuelIDs = SplitFuelIDs(fuelEntryId);
            var success = new int[fuelIDs.Length];
            foreach (GridViewRow item in gvVerification.Rows)
                for (var i = 0; i < fuelIDs.Length; i++)
                {
                    var remarks = ((TextBox) item.FindControl("txtRemarks")).Text;
                    var chk = (CheckBox) item.FindControl("checkSelect");
                    if (chk.Checked)
                        if (remarks != string.Empty)
                        {
                            btnApprove_Click(sender,e);
                            success[i] = Objfuelver.IRejectStatus(fuelIDs[i],remarks);
                        }
                        else
                        {
                            var strFmsScript = "Please Enter Remarks";
                            Show(strFmsScript);
                            break;
                        }
                }

            var count = 0;
            foreach (var item in success)
                if (item == 1)
                    count++;
            if (count == success.Length)
            {
                var strFmsScript = "Fuel Entry Rejected";
                Show(strFmsScript);
                FillGridVerification();
                ClearControls();
            }
            else
            {
                var strFmsScript = "Failure";
                Show(strFmsScript);
            }

            FillGridVerification();
            ClearControls();
        }
        else
        {
            Show("Please Select and Reject");
        }

        FillGridVerification();
        ClearControls();
    }

    protected void gvVerification_PageIndexChanging1(object sender,GridViewPageEventArgs e)
    {
        gvVerification.PageIndex = e.NewPageIndex;
        var districtId = -1;
        var vehicleId = -1;
        if (Session["UserdistrictId"] != null) districtId = Convert.ToInt32(Session["UserdistrictId"].ToString());
        if (ddlVehicleNumber.SelectedValue != null) vehicleId = Convert.ToInt32(ddlVehicleNumber.SelectedValue);
        var ds = Objfuelver.IFillGridVerification(districtId,vehicleId);
        if (ds == null) throw new ArgumentNullException(nameof(ds));
        gvVerification.DataSource = ds;
        gvVerification.DataBind();
    }

    protected void ddlVehicleNumber_SelectedIndexChanged(object sender,EventArgs e)
    {
        FillGridVerification();
        FillGridReconcilation();
    }

    private void FillGridReconcilation()
    {
        var districtId = -1;
        var vehicleId = -1;
        if (Session["UserdistrictId"] != null) districtId = Convert.ToInt32(Session["UserdistrictId"].ToString());
        if (ddlVehicleNumber.SelectedValue != null) vehicleId = Convert.ToInt32(ddlVehicleNumber.SelectedValue);
        var ds = Objfuelver.IFillGridReconcilation(districtId,vehicleId);
        if (ds == null) throw new ArgumentNullException(nameof(ds));
        switch (ds.Tables[0].Rows.Count)
        {
            case 0:
                gvReconcilation.DataSource = ds;
                gvReconcilation.DataBind();
                break;
            default:
                gvReconcilation.DataSource = ds;
                gvReconcilation.DataBind();
                break;
        }
    }

    protected void gvReconcilation_RowEditing(object sender,GridViewEditEventArgs e)
    {
    }

    protected void gvReconcilation_PageIndexChanging1(object sender,GridViewPageEventArgs e)
    {
        gvReconcilation.PageIndex = e.NewPageIndex;
        var districtId = -1;
        var vehicleId = -1;
        if (Session["UserdistrictId"] != null) districtId = Convert.ToInt32(Session["UserdistrictId"].ToString());
        if (ddlVehicleNumber.SelectedValue != null) vehicleId = Convert.ToInt32(ddlVehicleNumber.SelectedValue);
        var ds = Objfuelver.IFillGridReconcilation(districtId,vehicleId);
        if (ds == null) throw new ArgumentNullException(nameof(ds));
        gvReconcilation.DataSource = ds;
        gvReconcilation.DataBind();
    }
}