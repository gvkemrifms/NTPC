using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using GvkFMSAPP.BLL;
using GvkFMSAPP.PL;

public partial class SparePartsReceipt : Page
{
    private readonly FMSGeneral _fmsg = new FMSGeneral();
    private readonly Helper _helper = new Helper();
    public IInventory ObjInventory = new FMSInventory();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Name"] == null) Response.Redirect("Error.aspx");
        if (!IsPostBack)
        {
            FillVehicles();
            btReceive.Attributes.Add("onclick", "return validation()");
            txtInvoiceNo.Attributes.Add("onkeypress", "javascript:return isNumberKey(this,event)");
            txtReceiptRemarks.Attributes.Add("onkeypress", "javascript:return remark(this,event)");
            var dsPerms = (DataSet) Session["PermissionsDS"];
            dsPerms.Tables[0].DefaultView.RowFilter = "Url='" + Page.Request.Url.Segments[Page.Request.Url.Segments.Length - 1] + "'";
            var p = new PagePermissions(dsPerms, dsPerms.Tables[0].DefaultView[0]["Url"].ToString(), dsPerms.Tables[0].DefaultView[0]["Title"].ToString());
            gvIssueDetails.Visible = false;
            if (p.View)
            {
                gvIssueDetails.Visible = true;
                gvIssueDetails.Columns[8].Visible = false;
            }

            if (p.Add)
            {
                gvIssueDetails.Visible = true;
                gvIssueDetails.Columns[8].Visible = true;
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

    protected void ddlVehicles_SelectedIndexChanged(object sender, EventArgs e)
    {
        switch (ddlVehicles.SelectedIndex)
        {
            case 0:
                gvIssueDetails.Visible = false;
                break;
            default:
                FillGridIssueDetails(Convert.ToInt32(ddlVehicles.SelectedValue), 2);
                break;
        }
    }

    private void FillGridIssueDetails(int vehicleId, int fleetInventoryItemId)
    {
        var ds = ObjInventory.GetInventoryIssueDetailsForVehicle1(vehicleId, fleetInventoryItemId);
        if (ds == null) throw new ArgumentNullException(nameof(ds));
        gvIssueDetails.DataSource = ds;
        gvIssueDetails.DataBind();
        gvIssueDetails.Visible = true;
    }

    protected void gvIssueDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvIssueDetails.PageIndex = e.NewPageIndex;
        FillGridIssueDetails(Convert.ToInt32(ddlVehicles.SelectedValue), 2);
    }

    protected void btnViewDetails_Click(object sender, EventArgs e)
    {
        var btnViewDetails = sender as LinkButton;
        if (btnViewDetails != null)
        {
            var rowIndex = Convert.ToInt32(btnViewDetails.Attributes["RowIndex"]);
            txtVehicleNo.Text = gvIssueDetails.Rows[rowIndex].Cells[0].Text;
            txtDistrict.Text = gvIssueDetails.Rows[rowIndex].Cells[1].Text;
        }
    }

    protected void btReceive_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow item in gvReceiptDetails.Rows)
        {
            var receivedQuantity = Convert.ToInt32(((TextBox) item.FindControl("txtReceivedQty")).Text);
            var detId = Convert.ToInt32(((Label) item.FindControl("LbDetID")).Text);
            InsertReceiptDetails(Convert.ToInt32(txtInvReqID.Text), receivedQuantity, Convert.ToInt32(txtInvoiceNo.Text), Convert.ToDateTime(txtInvoiceDate.Text), Convert.ToDateTime(txtReceiptDate.Text), Convert.ToString(txtReceiptRemarks.Text), detId);
        }

        FillGridIssueDetails(Convert.ToInt32(ddlVehicles.SelectedValue), 2);
        Reset();
    }

    private void InsertReceiptDetails(int issueId, int receivedQuantity, int invoiceNumber, DateTime invoiceDate, DateTime receiptDate, string remarks, int detId)
    {
        var res = ObjInventory.InsInventoryReceiptDet(issueId, receivedQuantity, invoiceNumber, invoiceDate, receiptDate, remarks, detId);
        if (res > 0)
        {
            var strFmsScript = "Spare Parts Received";
            Show(strFmsScript);
        }
        else
        {
            var strFmsScript = "Failure";
            Show(strFmsScript);
        }
    }

    protected void gvReceiptDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:
                ((TextBox) e.Row.FindControl("txtReceivedQty")).Attributes.Add("onblur", "javascript:ValidateIssueQty('" + ((TextBox) e.Row.FindControl("txtReceivedQty")).ClientID + "','" + e.Row.Cells[2].Text + "')");
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "dsad", "var ReceivedQuantity = '" + ((TextBox) e.Row.FindControl("txtReceivedQty")).ClientID + "'", true);
                break;
        }
    }

    public void Show(string message)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('" + message + "');", true);
    }

    protected void btCancel_Click(object sender, EventArgs e)
    {
        Reset();
    }

    private void Reset()
    {
        txtInvoiceDate.Text = "";
        txtInvoiceNo.Text = "";
        txtReceiptDate.Text = "";
        txtReceiptRemarks.Text = "";
    }

    protected void gvIssueDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        txtInvReqID.Text = e.CommandArgument.ToString();
        var id = Convert.ToInt32(e.CommandArgument.ToString());
        var ds = ObjInventory.GetGridReceiptDetails(id);
        txtVehicleID.Text = ds.Tables[1].Rows[0][1].ToString();
        txtDCNumber.Text = ds.Tables[0].Rows[0][0].ToString();
        txtDCDate.Text = ds.Tables[0].Rows[0][1].ToString();
        txtCourierName.Text = ds.Tables[0].Rows[0][2].ToString();
        txtRemarks.Text = ds.Tables[0].Rows[0][3].ToString();
        gvReceiptDetails.DataSource = ds.Tables[1];
        gvReceiptDetails.DataBind();
        gv_ModalPopupExtender1.Show();
    }
}