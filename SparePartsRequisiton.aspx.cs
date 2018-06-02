using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using GvkFMSAPP.BLL;
using GvkFMSAPP.DLL;
using GvkFMSAPP.PL;
using GvkFMSAPP.PL.Inventory.SpareParts;
using FMSGeneral = GvkFMSAPP.BLL.FMSGeneral;

public partial class SparePartsRequisiton : Page
{
    private readonly FMSGeneral _fmsg = new FMSGeneral();
    private readonly Helper _helper = new Helper();
    public IInventory ObjInventory = new FMSInventory();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Name"] == null) Response.Redirect("Login.aspx");
        if (!IsPostBack)
        {
            RequisitionHistory.Visible = false;
            Session["dsSpareParts"] = null;
            BindGrid(1);
            FillVehicles();
            btnSubmit.Attributes.Add("onclick", "return validation()");
            var dsPerms = (DataSet) Session["PermissionsDS"];
            dsPerms.Tables[0].DefaultView.RowFilter = "Url='" + Page.Request.Url.Segments[Page.Request.Url.Segments.Length - 1] + "'";
            var p = new PagePermissions(dsPerms, dsPerms.Tables[0].DefaultView[0]["Url"].ToString(), dsPerms.Tables[0].DefaultView[0]["Title"].ToString());
            grvPendingforApproval.Columns[5].Visible = false;
            pnlSparePartsRequisition.Visible = false;
            if (p.View)
            {
                grvPendingforApproval.Visible = true;
                grvPendingforApproval.Columns[5].Visible = false;
            }

            if (p.Add) pnlSparePartsRequisition.Visible = true;
            if (p.Modify) pnlSparePartsRequisition.Visible = true;
            if (p.Approve)
            {
                grvPendingforApproval.Visible = true;
                grvPendingforApproval.Columns[5].Visible = true;
            }
        }
    }

    private void FillVehicles()
    {
        try
        {
            _fmsg.UserDistrictId = Convert.ToInt32(Session["UserdistrictId"].ToString());
            var ds = _fmsg.GetVehicleNumber();
            if (ds != null) _helper.FillDropDownHelperMethodWithDataSet(ds, "VehicleNumber", "VehicleID", null, ddlVehicles);
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    protected void BindGrid(int iRows)
    {
        try
        {
            var ds = CreateEmptyRows(iRows);
            if (ds == null) throw new ArgumentNullException(nameof(ds));
            grvNewSparePartsRequisition.DataSource = ds;
            grvNewSparePartsRequisition.DataBind();
            Session["dsSpareParts"] = ds;
            foreach (GridViewRow gvrow in grvNewSparePartsRequisition.Rows)
            {
                var dsSpareParts = ObjInventory.GetSpareParts();
                if (dsSpareParts == null) throw new ArgumentNullException(nameof(dsSpareParts));
                var ddlSpareParts = (DropDownList) gvrow.FindControl("ddlSparePartName");
                _helper.FillDropDownHelperMethodWithDataSet(dsSpareParts, "SparePart_Name", "SparePart_Id", ddlSpareParts);
                ddlSpareParts.SelectedValue = ds.Tables[0].Rows[gvrow.RowIndex]["SparePart_Id"].ToString();
            }
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    protected dsEmptySpareParts CreateEmptyRows(int rowCount)
    {
        var ds = new dsEmptySpareParts();
        if (Session["dsSpareParts"] != null) ds = (dsEmptySpareParts) Session["dsSpareParts"];
        for (var i = 0; i < rowCount; i++)
        {
            var newSpRow = ds.Tables[0].NewRow();
            newSpRow["SNo"] = ds.Tables[0].Rows.Count + 1;
            newSpRow["Quantity"] = 0;
            ds.Tables[0].Rows.Add(newSpRow);
        }

        return ds;
    }

    protected void grvNewSparePartsRequisition_RowDataBound(object sender, GridViewRowEventArgs e)
    {
    }

    protected void btnAddRow_Click(object sender, EventArgs e)
    {
        UpdateSessionTable();
        BindGrid(1);
    }

    protected void UpdateSessionTable()
    {
        foreach (GridViewRow gvrow in grvNewSparePartsRequisition.Rows)
        {
            var ds = (dsEmptySpareParts) Session["dsSpareParts"];
            if (ds == null) throw new ArgumentNullException(nameof(ds));
            var txtqty = (TextBox) gvrow.FindControl("txtQuantity");
            var ddlSpPart = (DropDownList) gvrow.FindControl("ddlSparePartName");
            ds.Tables[0].Rows[gvrow.RowIndex]["Quantity"] = txtqty.Text;
            ds.Tables[0].Rows[gvrow.RowIndex]["SparePart_Id"] = ddlSpPart.SelectedValue;
            Session["dsSpareParts"] = ds;
        }
    }

    protected void AdjustSiNo()
    {
        var ds = (dsEmptySpareParts) Session["dsSpareParts"];
        if (ds == null) throw new ArgumentNullException(nameof(ds));
        for (var i = 0; i < ds.Tables[0].Rows.Count; i++) ds.Tables[0].Rows[i]["SNo"] = i + 1;
        ds.AcceptChanges();
    }

    public int CheckDuplicateRows(DataTable dTable, string colName)
    {
        var hTable = new Hashtable();
        var duplicateList = new ArrayList();
        foreach (DataRow drow in dTable.Rows)
            if (hTable.Contains(drow[colName]))
                duplicateList.Add(drow);
            else
                hTable.Add(drow[colName], string.Empty);
        return duplicateList.Count;
    }

    protected void BtnDelete_Click(object sender, EventArgs e)
    {
        UpdateSessionTable();
        //  get the gridviewrow from the sender so we can get the datakey we need
        var lnkDelInventoryReq = sender as Button;
        if (lnkDelInventoryReq != null)
        {
            var row = (GridViewRow) lnkDelInventoryReq.NamingContainer;
            try
            {
                var ds = new dsEmptySpareParts();
                if (Session["dsSpareParts"] != null)
                {
                    ds = (dsEmptySpareParts) Session["dsSpareParts"];
                    ds.Tables[0].Rows[row.RowIndex].Delete();
                    ds.AcceptChanges();
                    AdjustSiNo();
                }

                grvNewSparePartsRequisition.DataSource = ds;
                grvNewSparePartsRequisition.DataBind();
                Session["dsSpareParts"] = ds;
                foreach (GridViewRow gvrow in grvNewSparePartsRequisition.Rows)
                {
                    var dsSpareParts = ObjInventory.GetSpareParts();
                    var ddlSpareParts = (DropDownList) gvrow.FindControl("ddlSparePartName");
                    _helper.FillDropDownHelperMethodWithDataSet(dsSpareParts, "SparePart_Name", "SparePart_Id", ddlSpareParts);
                    ddlSpareParts.SelectedValue = ds.Tables[0].Rows[gvrow.RowIndex]["SparePart_Id"].ToString();
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.ErrorsEntry(ex.GetBaseException().ToString(), "class: NewTyreRequisition;Method: Page_Load()-BtnDelete_Click", 0);
            }
        }

        ////  rebind the datasource
        switch (ddlVehicles.SelectedIndex)
        {
            case 0:
                if (ViewState["VehicleID"] == null) return;
                GetInventoryReqPending(2, Convert.ToInt16(ViewState["VehicleID"]));
                break;
            default:
                GetInventoryReqPending(2, Convert.ToInt16(ddlVehicles.SelectedValue));
                break;
        }
    }

    protected void grvNewSparePartsRequisition_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        grvNewSparePartsRequisition.DeleteRow(e.RowIndex);
        grvNewSparePartsRequisition.DataBind();
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        var quantitySatus = false;
        UpdateSessionTable();
        var ds = (dsEmptySpareParts) Session["dsSpareParts"];
        if (ds == null) throw new ArgumentNullException(nameof(ds));
        var dt = ds.Tables[0];
        var duplicateCount = CheckDuplicateRows(dt, "SparePart_Id");
        if (duplicateCount > 0)
        {
            ScriptManager.RegisterStartupScript(this, typeof(string), "Error", "alert('Same Spare Part is requested Multiple Times. Please Correct it and Submit Again.');", true);
        }
        else
        {
            var dtSpareParts = new DataTable();
            dtSpareParts.Columns.Add("VehicleID", typeof(short));
            dtSpareParts.Columns.Add("VehicleNum", typeof(string));
            dtSpareParts.Columns.Add("DistrictID", typeof(short));
            dtSpareParts.Columns.Add("SparePartID", typeof(short));
            dtSpareParts.Columns.Add("SparePartReqQty", typeof(short));
            dtSpareParts.Columns.Add("FleetInventoryItemID", typeof(short));
            dtSpareParts.Columns.Add("RequestedBy", typeof(short));
            foreach (GridViewRow row in grvNewSparePartsRequisition.Rows)
            {
                var dr = dtSpareParts.NewRow();
                dr["VehicleID"] = Convert.ToInt16(ddlVehicles.SelectedValue);
                dr["VehicleNum"] = Convert.ToString(ddlVehicles.SelectedItem);
                dr["DistrictID"] = Convert.ToInt32(Session["UserdistrictId"].ToString());
                var ddl = (DropDownList) row.FindControl("ddlSparePartName");
                dr["SparePartID"] = ddl.SelectedValue;
                var txt = (TextBox) row.FindControl("txtQuantity");
                switch (int.Parse(txt.Text))
                {
                    case 0:
                        quantitySatus = true;
                        break;
                }

                dr["SparePartReqQty"] = Convert.ToInt16(txt.Text);
                dr["FleetInventoryItemID"] = 2;
                dr["RequestedBy"] = 1;
                dtSpareParts.Rows.Add(dr);
            }

            if (quantitySatus)
            {
                Show("Quantity cannnot be 0");
            }
            else
            {
                var updResult = ObjInventory.InsInventoryRequestion(dtSpareParts);
                if (updResult)
                {
                    var strFmsScript = "Spare Parts Requested";
                    Show(strFmsScript);
                }
                else
                {
                    var strFmsScript = "Failure";
                    Show(strFmsScript);
                }

                //ClearControls();
                Session["dsSpareParts"] = null;
                BindGrid(1);
            }

            switch (ddlVehicles.SelectedIndex)
            {
                case 0:
                    if (ViewState["VehicleID"] != null) GetInventoryReqPending(2, Convert.ToInt16(ViewState["VehicleID"]));
                    break;
                default:
                    GetInventoryReqPending(2, Convert.ToInt16(ddlVehicles.SelectedValue));
                    break;
            }
        }
    }

    public void Show(string message)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('" + message + "');", true);
    }

    protected void GetInventoryReqPending(int fleetInventoryItemId, int vehicleId)
    {
        try
        {
            grvPendingforApproval.DataSource = ObjInventory.GetInventoryReqPending(2, vehicleId);
            grvPendingforApproval.DataBind();
        }
        catch (Exception ex)
        {
            ErrorHandler.ErrorsEntry(ex.GetBaseException().ToString(), "class: FMSInventory.cs;Method: btnSubmit_Click()-GetInventoryReqPending", 0);
        }
    }

    protected void grvNewSparePartsRequisition_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void ddlVehicles_SelectedIndexChanged(object sender, EventArgs e)
    {
        switch (ddlVehicles.SelectedIndex)
        {
            case 0:
                grvPendingforApproval.Visible = false;
                grvRequisitionHistory.Visible = false;
                break;
            default:
                grvPendingforApproval.Visible = true;
                GetInventoryReqPending(2, Convert.ToInt16(ddlVehicles.SelectedValue));
                ViewState["VehicleID"] = ddlVehicles.SelectedValue;
                FillGrid_RequisitionHistory(Convert.ToInt32(ddlVehicles.SelectedValue), Convert.ToInt32(Session["UserdistrictId"].ToString()), 2);
                break;
        }
    }

    protected void btnViewDetails_Click(object sender, EventArgs e)
    {
    }

    protected void btnOk_Click(object sender, EventArgs e)
    {
        var id = Convert.ToInt32(txtReqID.Text);
        var res = ObjInventory.ApproveRejectRequisition(id, 2);
        if (res > 0)
        {
            var strFmsScript = "Spare Parts Requisition Approved";
            Show(strFmsScript);
        }
        else
        {
            var strFmsScript = "Failure";
            Show(strFmsScript);
        }

        ClearControls();
        switch (ddlVehicles.SelectedIndex)
        {
            case 0:
                if (ViewState["VehicleID"] != null) GetInventoryReqPending(2, Convert.ToInt16(ViewState["VehicleID"]));
                break;
            default:
                GetInventoryReqPending(2, Convert.ToInt16(ddlVehicles.SelectedValue));
                break;
        }
    }

    protected void btnNo_Click(object sender, EventArgs e)
    {
        var id = Convert.ToInt32(txtReqID.Text);
        var res = ObjInventory.ApproveRejectRequisition(id, 3);
        if (res > 0)
        {
            var strFmsScript = "Spare Parts Requisition Rejected";
            Show(strFmsScript);
        }
        else
        {
            var strFmsScript = "Failure";
            Show(strFmsScript);
        }

        ClearControls();
        switch (ddlVehicles.SelectedIndex)
        {
            case 0:
                if (ViewState["VehicleID"] != null) GetInventoryReqPending(2, Convert.ToInt16(ViewState["VehicleID"]));
                break;
            default:
                GetInventoryReqPending(2, Convert.ToInt16(ddlVehicles.SelectedValue));
                break;
        }
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        Session["dsSpareParts"] = null;
        BindGrid(1);
        RequisitionHistory.Visible = false;
        ddlVehicles.SelectedIndex = 0;
        grvPendingforApproval.Visible = false;
    }

    private void ClearControls()
    {
        ddlVehicles.SelectedIndex = 0;
        txtReqID.Text = "";
        txtVehicleNumber.Text = "";
        foreach (GridViewRow row in grvNewSparePartsRequisition.Rows)
        {
            var reset = (TextBox) row.FindControl("txtQuantity");
            reset.Text = "0";
        }

        grvPendingforApproval.Visible = false;
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        gv_ModalPopupExtender1.Hide();
    }

    public void FillGrid_RequisitionHistory(int vehicleId, int districtId, int fleetInventoryItemId)
    {
        var ds = ObjInventory.IFillFleetInventoryRequisitionHistory(vehicleId, districtId, 2);
        if (ds == null) throw new ArgumentNullException(nameof(ds));
        grvRequisitionHistory.DataSource = ds.Tables[0];
        grvRequisitionHistory.DataBind();
    }

    protected void btnSparePartsReqHistory_Click(object sender, EventArgs e)
    {
        hideHistory.Visible = true;
        RequisitionHistory.Visible = true;
        FillGrid_RequisitionHistory(Convert.ToInt32(ddlVehicles.SelectedValue), Convert.ToInt32(Session["UserdistrictId"].ToString()), 2);
    }

    protected void grvRequisitionHistory_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grvRequisitionHistory.PageIndex = e.NewPageIndex;
        FillGrid_RequisitionHistory(Convert.ToInt32(ddlVehicles.SelectedValue), Convert.ToInt32(Session["UserdistrictId"].ToString()), 2);
    }

    protected void grvPendingforApproval_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "Show":
                var id = Convert.ToInt32(e.CommandArgument.ToString());
                var ds = ObjInventory.GetInventoryReqForEDIT(id);
                if (ds == null) throw new ArgumentNullException(nameof(ds));
                txtReqID.Text = e.CommandArgument.ToString();
                txtVehicleNumber.Text = ds.Tables[0].Rows[0]["VehicleNum"].ToString();
                grvBatteryRequestDetails.DataSource = ds.Tables[1];
                grvBatteryRequestDetails.DataBind();
                gv_ModalPopupExtender1.Show();
                break;
        }
    }

    protected void hideHistory_Click(object sender, EventArgs e)
    {
        hideHistory.Visible = false;
        RequisitionHistory.Visible = false;
    }
}