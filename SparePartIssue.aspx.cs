using System;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using GvkFMSAPP.BLL;
using GvkFMSAPP.PL;

public partial class SparePartIssue : Page
{
    private readonly FMSGeneral _fmsg = new FMSGeneral();
    private readonly Helper _helper = new Helper();
    public int fleetInventoryCategoryId = Convert.ToInt32(ConfigurationManager.AppSettings["fICategoryId"]);
    public IInventory ObjInventory = new FMSInventory();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Name"] == null) Response.Redirect("Error.aspx");
        if (!IsPostBack)
        {
            FillVehicles();
            btIssue.Attributes.Add("onclick", "return validation()");
            txtDCNumber.Attributes.Add("onkeypress", "javascript:return isNumberKey(this,event)");
            txtCourierName.Attributes.Add("onkeypress", "javascript:return OnlyAlphabets(this,event)");
            txtRemarks.Attributes.Add("onkeypress", "javascript:return remark(this,event)");
            var dsPerms = (DataSet) Session["PermissionsDS"];
            dsPerms.Tables[0].DefaultView.RowFilter = "Url='" + Page.Request.Url.Segments[Page.Request.Url.Segments.Length - 1] + "'";
            var p = new PagePermissions(dsPerms, dsPerms.Tables[0].DefaultView[0]["Url"].ToString(), dsPerms.Tables[0].DefaultView[0]["Title"].ToString());
            gvApprovedRequisition.Visible = false;
            gvApprovedRequisition.Columns[3].Visible = false;
            if (p.View)
            {
                gvApprovedRequisition.Visible = true;
                gvApprovedRequisition.Columns[4].Visible = false;
            }

            if (p.Add)
            {
                gvApprovedRequisition.Visible = true;
                gvApprovedRequisition.Columns[4].Visible = true;
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
                gvApprovedRequisition.Visible = false;
                break;
            default:
                FillGridApprovedRequisitions(Convert.ToInt32(ddlVehicles.SelectedValue));
                gvApprovedRequisition.Visible = true;
                break;
        }
    }

    private void FillGridApprovedRequisitions(int vehicleId)
    {
        var ds = ObjInventory.GetApprovedInventoryRequisitions(fleetInventoryCategoryId, vehicleId);
        gvApprovedRequisition.DataSource = ds;
        gvApprovedRequisition.DataBind();
    }

    protected void gvApprovedRequisition_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvApprovedRequisition.PageIndex = e.NewPageIndex;
        var ds = ObjInventory.GetApprovedInventoryRequisitions(fleetInventoryCategoryId, Convert.ToInt32(ddlVehicles.SelectedValue));
        gvApprovedRequisition.DataSource = ds;
        gvApprovedRequisition.DataBind();
    }

    protected void btIssue_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow item in gvIssueDetails.Rows)
        {
            var issuedQuantity = Convert.ToInt32(((TextBox) item.FindControl("txtIssuedQty")).Text);
            var ds = _fmsg.GetRegistrationDate(Convert.ToInt32(ddlVehicles.SelectedValue));
            if (ds == null) throw new ArgumentNullException(nameof(ds));
            if (DateTime.Parse(ds.Tables[0].Rows[0]["RegDate"].ToString()) < DateTime.Parse(txtDCDate.Text))
            {
                InsertIssueDetails(Convert.ToInt32(txtInvReqID.Text), Convert.ToInt32(txtDCNumber.Text), Convert.ToDateTime(txtDCDate.Text), Convert.ToString(txtCourierName.Text), Convert.ToString(txtRemarks.Text), issuedQuantity, Convert.ToInt32(txtVehicleID.Text));
                FillGridApprovedRequisitions(Convert.ToInt32(ddlVehicles.SelectedValue));
                Reset();
            }
            else
            {
                Show("DC Date should be greater than Registration Date" + ds.Tables[0].Rows[0]["RegDate"]);
                gv_ModalPopupExtender1.Show();
            }
        }
    }

    private void InsertIssueDetails(int reqId, int dcNumber, DateTime dcDate, string courierName, string remarks, int issuedQuantity, int vehicleId)
    {
        var res = ObjInventory.InsertIssueDetails(reqId, dcNumber, dcDate, courierName, remarks, issuedQuantity, vehicleId);
        if (res > 0)
        {
            var strFmsScript = "Spare Parts Issued";
            Show(strFmsScript);
        }
        else
        {
            var strFmsScript = "Failure";
            Show(strFmsScript);
        }
    }

    protected void gvIssueDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:
                ((TextBox) e.Row.FindControl("txtIssuedQty")).Attributes.Add("onblur", "javascript:ValidateIssueQty('" + ((TextBox) e.Row.FindControl("txtIssuedQty")).ClientID + "','" + e.Row.Cells[1].Text + "')");
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "dsad", "var IssuedQuantity = '" + ((TextBox) e.Row.FindControl("txtIssuedQty")).ClientID + "'", true);
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
        txtDCNumber.Text = "";
        txtDCDate.Text = "";
        txtCourierName.Text = "";
        txtRemarks.Text = "";
    }

    protected void gvApprovedRequisition_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "Show":
                var id = Convert.ToInt32(e.CommandArgument.ToString());
                var ds = ObjInventory.GetGridIssueDetails(id);
                txtVehicleID.Text = ds.Tables[0].Rows[0][1].ToString();
                txtVehicleNo.Text = ds.Tables[0].Rows[0][2].ToString();
                txtDistrict.Text = ds.Tables[0].Rows[0][3].ToString();
                txtInvReqID.Text = ds.Tables[0].Rows[0][0].ToString();
                gvIssueDetails.DataSource = ds.Tables[1];
                gvIssueDetails.DataBind();
                gv_ModalPopupExtender1.Show();
                break;
        }
    }
}