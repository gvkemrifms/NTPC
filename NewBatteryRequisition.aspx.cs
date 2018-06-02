using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using GvkFMSAPP.BLL;
using GvkFMSAPP.PL;

public partial class NewBatteryRequisition : Page
{
    public IInventory ObjFmsInventory = new FMSInventory();
    readonly Helper _helper = new Helper();
    readonly GvkFMSAPP.BLL.FMSGeneral _fmsg = new GvkFMSAPP.BLL.FMSGeneral();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Name"] == null) Response.Redirect("Login.aspx");
        if (!IsPostBack)
        {
            RequisitionHistory.Visible = false;
            FillInventoryVehicles();
            grvInventoryNewBatteryRequisition.Visible = false;
            btnNewBatteryReqSave.Attributes.Add("OnClick", "return validationInventoryBatteryVehicleType()");
            DataSet dsPerms = (DataSet) Session["PermissionsDS"];
            dsPerms.Tables[0].DefaultView.RowFilter = "Url='" + Page.Request.Url.Segments[Page.Request.Url.Segments.Length - 1] + "'";
            PagePermissions p = new PagePermissions(dsPerms, dsPerms.Tables[0].DefaultView[0]["Url"].ToString(), dsPerms.Tables[0].DefaultView[0]["Title"].ToString());
            grvBatteryPendingForApproval.Columns[4].Visible = false;
            pnlNewBatteryRequisition.Visible = false;
            grvBatteryPendingForApproval.Visible = false;
            if (p.View)
            {
                grvBatteryPendingForApproval.Visible = true;
                grvBatteryPendingForApproval.Columns[5].Visible = false;
            }

            if (p.Add)
            {
                pnlNewBatteryRequisition.Visible = true;
                grvBatteryPendingForApproval.Visible = true;
                grvBatteryPendingForApproval.Columns[5].Visible = false;
            }

            if (p.Approve)
            {
                grvBatteryPendingForApproval.Visible = true;
                grvBatteryPendingForApproval.Columns[5].Visible = true;
            }
        }
    }

    private void FillInventoryVehicles()
    {
        try
        {
            _fmsg.UserDistrictId = global::System.Convert.ToInt32(Session["UserdistrictId"].ToString());
            DataSet ds = _fmsg.GetVehicleNumber();
            if (ds != null)
            {
                _helper.FillDropDownHelperMethodWithDataSet(ds, "VehicleNumber", "VehicleID", null, ddlInventoryVehicles);
                ddlInventoryVehicles.Items[0].Value = "0";
            }
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    protected void ddlInventoryVehicles_SelectedIndexChanged(object sender, EventArgs e)
    {
        switch (ddlInventoryVehicles.SelectedIndex)
        {
            case 0:
                grvInventoryNewBatteryRequisition.Visible = false;
                grvBatteryPendingForApproval.Visible = false;
                break;
            default:
                FillGrid_NewBatteryRequisition(Convert.ToInt32(ddlInventoryVehicles.SelectedValue));
                grvInventoryNewBatteryRequisition.Visible = true;
                FillGrid_BatteryPendingForApproval(3, Convert.ToInt32(ddlInventoryVehicles.SelectedValue));
                FillGrid_RequisitionHistory(Convert.ToInt32(ddlInventoryVehicles.SelectedValue), Convert.ToInt32(Session["UserdistrictId"].ToString()), 3);
                break;
        }
    }

    public void FillGrid_NewBatteryRequisition(int vehicleId)
    {
        var ds = ObjFmsInventory.GetInventoryIssueDetailsForVehicle(vehicleId);
        if (ds == null) throw new ArgumentNullException(nameof(ds));
        grvInventoryNewBatteryRequisition.DataSource = ds;
        grvInventoryNewBatteryRequisition.DataBind();
    }

    protected void btnNewBatteryReqSave_Click(object sender, EventArgs e)
    {
        int chk = 0;
        DataTable dtUpdateBattery = new DataTable();
        dtUpdateBattery.Columns.Add("VehicleID", typeof(Int32));
        dtUpdateBattery.Columns.Add("VehicleNum", typeof(String));
        dtUpdateBattery.Columns.Add("DistrictID", typeof(Int32));
        dtUpdateBattery.Columns.Add("FleetInventoryCategory", typeof(Int32));
        dtUpdateBattery.Columns.Add("BatteryNo", typeof(String));
        dtUpdateBattery.Columns.Add("BatteryReqQty", typeof(Int32));
        dtUpdateBattery.Columns.Add("BatteryReqRemarks", typeof(String));
        dtUpdateBattery.Columns.Add("RequestedBy", typeof(Int32));
        dtUpdateBattery.Columns.Add("Battery1or2", typeof(String));
        foreach (GridViewRow row in grvInventoryNewBatteryRequisition.Rows)
        {
            if (((CheckBox) row.FindControl("chk")).Checked)
            {
                TextBox txt = (TextBox) row.FindControl("txtRemarks");
                switch (txt.Text)
                {
                    case "":
                        chk++;
                        break;
                }

                var dr = dtUpdateBattery.NewRow();
                dr["VehicleID"] = Convert.ToInt16(ddlInventoryVehicles.SelectedValue);
                dr["VehicleNum"] = Convert.ToString(ddlInventoryVehicles.SelectedItem);
                dr["DistrictID"] = Convert.ToInt32(Session["UserdistrictId"].ToString());
                dr["FleetInventoryCategory"] = 3;
                dr["BatteryNo"] = row.Cells[2].Text;
                dr["BatteryReqQty"] = 1;
                dr["BatteryReqRemarks"] = ((TextBox) row.FindControl("txtRemarks")).Text;
                dr["RequestedBy"] = Convert.ToInt32(Session["User_Id"].ToString());
                dr["Battery1or2"] = row.Cells[1].Text;
                dtUpdateBattery.Rows.Add(dr);
            }
        }

        if (chk > 0)
            Show("Please Fill The Remarks");
        else
        {
            string strFmsScript;
            switch (dtUpdateBattery.Rows.Count)
            {
                case 0:
                    strFmsScript = "Please Check And Submit";
                    Show(strFmsScript);
                    break;
                default:
                    bool updResult = ObjFmsInventory.InsertingBatteryRequisitionRow(dtUpdateBattery);
                    if (updResult)
                    {
                        strFmsScript = "Battery Details Mapped successfully";
                        Show(strFmsScript);
                        FillGrid_BatteryPendingForApproval(3, Convert.ToInt32(ddlInventoryVehicles.SelectedValue));
                    }
                    else
                    {
                        strFmsScript = "This Battery Detail already exists ";
                        Show(strFmsScript);
                    }

                    break;
            }

            FillGrid_NewBatteryRequisition(Convert.ToInt32(ddlInventoryVehicles.SelectedValue));
            ViewState["vehicleid"] = ddlInventoryVehicles.SelectedValue;
        }
    }

    public void Show(string message)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('" + message + "');", true);
    }

    public void FillGrid_BatteryPendingForApproval(int fleetInventoryItemId, int vehicleId)
    {
        var ds = ObjFmsInventory.GetBatteryPendingForApproval(3, vehicleId);
        if (ds == null) throw new ArgumentNullException(nameof(ds));
        grvBatteryPendingForApproval.DataSource = ds.Tables[0];
        grvBatteryPendingForApproval.DataBind();
    }

    protected void grvBatteryPendingForApproval_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grvBatteryPendingForApproval.PageIndex = e.NewPageIndex;
        FillGrid_BatteryPendingForApproval(3, ViewState["vehicleid"] != null ? Convert.ToInt32(ViewState["vehicleid"]) : Convert.ToInt32(ddlInventoryVehicles.SelectedValue));
    }

    protected void btnNewBatteryReqReset_Click(object sender, EventArgs e)
    {
        ddlInventoryVehicles.SelectedValue = "0";
        grvInventoryNewBatteryRequisition.Visible = false;
        grvBatteryPendingForApproval.Visible = false;
        RequisitionHistory.Visible = false;
    }

    protected void BtnViewDetails_Click(object sender, EventArgs e)
    {
    }

    protected void btnOk_Click(object sender, EventArgs e)
    {
        int id = Convert.ToInt32(txtRequestIdPopup.Text);
        ObjFmsInventory.BatteryApproveRejectRequisition(id, 2);
        FillGrid_BatteryPendingForApproval(3, ViewState["vehicleid"] != null ? Convert.ToInt32(ViewState["vehicleid"]) : Convert.ToInt32(ddlInventoryVehicles.SelectedValue));
        Show("New Battery Request Approved");
    }

    protected void btnNo_Click(object sender, EventArgs e)
    {
        int id = Convert.ToInt32(txtRequestIdPopup.Text);
        ObjFmsInventory.BatteryApproveRejectRequisition(id, 3);
        FillGrid_BatteryPendingForApproval(3, ViewState["vehicleid"] != null ? Convert.ToInt32(ViewState["vehicleid"]) : Convert.ToInt32(ddlInventoryVehicles.SelectedValue));
        FillGrid_NewBatteryRequisition(ViewState["vehicleid"] != null ? Convert.ToInt32(ViewState["vehicleid"]) : Convert.ToInt32(ddlInventoryVehicles.SelectedValue));
        Show("New Battery Request Rejected");
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        gv_ModalPopupExtender1.Hide();
    }

    protected void btnNewBatteryReqViewHistory_Click(object sender, EventArgs e)
    {
        RequisitionHistory.Visible = true;
        FillGrid_RequisitionHistory(Convert.ToInt32(ddlInventoryVehicles.SelectedValue), Convert.ToInt32(Session["UserdistrictId"].ToString()), 3);
        hideHistory.Visible = true;
    }

    public void FillGrid_RequisitionHistory(int @vehicleId, int @districtId, int fleetInventoryItemId)
    {
        var ds = ObjFmsInventory.IFillFleetInventoryRequisitionHistory(vehicleId, districtId, 3);
        if (ds == null) throw new ArgumentNullException(nameof(ds));
        grvRequisitionHistory.DataSource = ds.Tables[0];
        grvRequisitionHistory.DataBind();
    }

    protected void grvRequisitionHistory_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grvRequisitionHistory.PageIndex = e.NewPageIndex;
        FillGrid_RequisitionHistory(Convert.ToInt32(ddlInventoryVehicles.SelectedValue), Convert.ToInt32(Session["UserdistrictId"].ToString()), 3);
    }

    protected void grvBatteryPendingForApproval_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int id = Convert.ToInt32(e.CommandArgument.ToString());
        var ds = ObjFmsInventory.GetInventoryReqForEdit(id);
        if (ds == null) throw new ArgumentNullException(nameof(ds));
        txtRequestIdPopup.Text = Convert.ToString(id);
        txtVehicleNumberPopUp.Text = ds.Tables[0].Rows[0]["VehicleNum"].ToString();
        grvBatteryRequestDetails.DataSource = ds.Tables[1];
        grvBatteryRequestDetails.DataBind();
        gv_ModalPopupExtender1.Show();
    }

    protected void hideHistory_Click(object sender, EventArgs e)
    {
        hideHistory.Visible = false;
        RequisitionHistory.Visible = false;
    }
}