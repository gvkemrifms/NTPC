using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using GvkFMSAPP.BLL;
using GvkFMSAPP.PL;

public partial class BatteryReceipt : Page
{
    private readonly FMSGeneral _fmsg = new FMSGeneral();
    private readonly Helper _helper = new Helper();
    public IInventory ObjFmsInvBatRecp = new FMSInventory();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Name"] == null) Response.Redirect("Error.aspx");
        if (!IsPostBack)
        {
            FillInventoryVehicles();
            btnOk.Attributes.Add("onclick", "return validation()");
            txtBatRecInvoiceNo.Attributes.Add("onkeypress", "javascript:return isNumberKey(this,event)");
            txtRemarks.Attributes.Add("onkeypress", "javascript:return remark(this,event)");
            var dsPerms = (DataSet) Session["PermissionsDS"];
            dsPerms.Tables[0].DefaultView.RowFilter = "Url='" + Page.Request.Url.Segments[Page.Request.Url.Segments.Length - 1] + "'";
            var p = new PagePermissions(dsPerms, dsPerms.Tables[0].DefaultView[0]["Url"].ToString(), dsPerms.Tables[0].DefaultView[0]["Title"].ToString());
            grvBatteryDetailsForReceipt.Visible = false;
            if (p.View)
            {
                grvBatteryDetailsForReceipt.Visible = true;
                grvBatteryDetailsForReceipt.Columns[6].Visible = false;
            }

            if (p.Add)
            {
                grvBatteryDetailsForReceipt.Visible = true;
                grvBatteryDetailsForReceipt.Columns[6].Visible = true;
            }
        }
    }

    private void FillInventoryVehicles()
    {
        try
        {
            if (_fmsg != null)
            {
                _fmsg.UserDistrictId = Convert.ToInt32(Session["UserdistrictId"].ToString());
                var ds = _fmsg.GetVehicleNumber();
                if (ds != null) _helper.FillDropDownHelperMethodWithDataSet(ds, "VehicleNumber", "VehicleID", null, ddlInventoryBatteryReceiptVehicles);
            }
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    private void FillGrid_BatteryDetailsForReceipt(int fleetInventoryItemId, int vehicleId)
    {
        var ds = ObjFmsInvBatRecp.GetBatteryDetailsForReceipt(fleetInventoryItemId, vehicleId);
        if (ds == null) throw new ArgumentNullException(nameof(ds));
        grvBatteryDetailsForReceipt.DataSource = ds.Tables[0];
        grvBatteryDetailsForReceipt.DataBind();
    }

    protected void BtnViewDetails_Click(object sender, EventArgs e)
    {
        var btnViewDetails = sender as LinkButton;
        if (btnViewDetails != null)
        {
            var rowIndex = Convert.ToInt32(btnViewDetails.Attributes["RowIndex"]);
            txtBatRecVehicleNo.Text = grvBatteryDetailsForReceipt.Rows[rowIndex].Cells[0].Text;
            txtBatRecDistrict.Text = grvBatteryDetailsForReceipt.Rows[rowIndex].Cells[1].Text;
        }
    }

    protected void ddlInventoryBatteryReceiptVehicles_SelectedIndexChanged(object sender, EventArgs e)
    {
        switch (ddlInventoryBatteryReceiptVehicles.SelectedIndex)
        {
            case 0:
                grvBatteryDetailsForReceipt.Visible = false;
                break;
            default:
                FillGrid_BatteryDetailsForReceipt(3, Convert.ToInt32(ddlInventoryBatteryReceiptVehicles.SelectedValue));
                break;
        }
    }

    private void InsertBatteryReceiptDetails(int inventoryItemIssueId, string vehicleNumer, int districtId, int poNumer, DateTime poDate, string courierName, string hoRemarks, int invoiceNo, DateTime invoiceDate, DateTime receiptDate, int batteryDetailsId, int receivedQty)
    {
        var res = ObjFmsInvBatRecp.InsertBatteryReceiptDetails(inventoryItemIssueId, vehicleNumer, districtId, poNumer, poDate, courierName, hoRemarks, invoiceNo, invoiceDate, receiptDate, batteryDetailsId, receivedQty);
        if (res > 0)
        {
            var strFmsScript = "<script language=JavaScript>alert('" + "Battery Receipt Details Submitted" + "')</script>";
            ClientScript.RegisterStartupScript(GetType(), "Success", strFmsScript);
        }
        else
        {
            var strFmsScript = "<script language=JavaScript>alert('" + "Failure " + "')</script>";
            ClientScript.RegisterStartupScript(GetType(), "failure", strFmsScript);
        }
    }

    protected void btnOk_Click(object sender, EventArgs e)
    {
        var test = false;
        foreach (GridViewRow item in grvBatteryReceiptDetailsPopup.Rows)
        {
            var receivedQuantity = Convert.ToInt32(((Label) item.FindControl("txtBatteryReceivedQty")).Text);
            var batteryDetailsId = Convert.ToInt32(((Label) item.FindControl("LbBatteryID")).Text);
            var itemIssueId = Convert.ToInt32(((Label) item.FindControl("LbIssueID")).Text);
            switch (receivedQuantity)
            {
                case 1:
                    InsertBatteryReceiptDetails(itemIssueId, Convert.ToString(txtBatRecVehicleNo.Text), 1, Convert.ToInt32(txtBatRecPONumber.Text), Convert.ToDateTime(txtBatRecPODate.Text), Convert.ToString(txtBatRecCourierName.Text), Convert.ToString(txtRemarks.Text), Convert.ToInt32(txtBatRecInvoiceNo.Text), Convert.ToDateTime(txtBatRecInvoiceDate.Text), Convert.ToDateTime(txtBatRecDate.Text), batteryDetailsId, receivedQuantity);
                    ClearControls();
                    FillGrid_BatteryDetailsForReceipt(3, Convert.ToInt32(ddlInventoryBatteryReceiptVehicles.SelectedValue));
                    Show("Battery Receipt Issued");
                    break;
                default:
                    test = true;
                    break;
            }
        }

        if (test) Show("Received Quantity can't be greater than 1");
    }

    protected void btnNo_Click(object sender, EventArgs e)
    {
        ClearControls();
        gv_ModalPopupExtenderBatteryReceipt.Hide();
    }

    protected void grvBatteryDetailsForReceipt_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grvBatteryDetailsForReceipt.PageIndex = e.NewPageIndex;
        FillGrid_BatteryDetailsForReceipt(3, Convert.ToInt32(ddlInventoryBatteryReceiptVehicles.SelectedValue));
    }

    private void ClearControls()
    {
        txtBatRecInvoiceNo.Text = "";
        txtBatRecInvoiceDate.Text = "";
        txtBatRecDate.Text = "";
        txtRemarks.Text = "";
    }

    public void Show(string message)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('" + message + "');", true);
    }

    protected void grvBatteryReceiptDetailsPopup_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow) ScriptManager.RegisterClientScriptBlock(this, GetType(), "dsad", "var IssuedQuantity = '" + ((Label) e.Row.FindControl("txtBatteryReceivedQty")).ClientID + "'", true);
    }

    protected void grvBatteryDetailsForReceipt_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        var id = Convert.ToInt32(e.CommandArgument.ToString());
        var ds = ObjFmsInvBatRecp.GetGridBatteryReceiptPopup(id);
        if (ds == null) throw new ArgumentNullException(nameof(ds));
        txtBatRecPONumber.Text = ds.Tables[0].Rows[0][1].ToString();
        txtBatRecPODate.Text = ds.Tables[0].Rows[0][2].ToString();
        txtBatRecCourierName.Text = ds.Tables[0].Rows[0][3].ToString();
        txtBatRecHORemarks.Text = ds.Tables[0].Rows[0][4].ToString();
        grvBatteryReceiptDetailsPopup.DataSource = ds.Tables[1];
        grvBatteryReceiptDetailsPopup.DataBind();
        gv_ModalPopupExtenderBatteryReceipt.Show();
    }
}