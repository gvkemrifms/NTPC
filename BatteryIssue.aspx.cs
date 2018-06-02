using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using GvkFMSAPP.BLL;
using GvkFMSAPP.PL;

public partial class BatteryIssue : Page
{
    private readonly FMSGeneral _fmsg = new FMSGeneral();
    private readonly Helper _helper = new Helper();
    public IInventory ObjFmsInvBatIss = new FMSInventory();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Name"] == null) Response.Redirect("Login.aspx");
        if (!IsPostBack)
        {
            FillInventoryVehicles();
            btnOk.Attributes.Add("onclick", "return validation()");
            txtDcNumberPopup.Attributes.Add("onkeypress", "javascript:return isNumberKey(this,event)");
            txtCourierName.Attributes.Add("onkeypress", "javascript:return OnlyAlphabets(this,event)");
            txtRemarks.Attributes.Add("onkeypress", "javascript:return remark(this,event)");
            var dsPerms = (DataSet) Session["PermissionsDS"];
            if (dsPerms == null) throw new ArgumentNullException(nameof(dsPerms));
            dsPerms.Tables[0].DefaultView.RowFilter = "Url='" + Page.Request.Url.Segments[Page.Request.Url.Segments.Length - 1] + "'";
            var p = new PagePermissions(dsPerms, dsPerms.Tables[0].DefaultView[0]["Url"].ToString(), dsPerms.Tables[0].DefaultView[0]["Title"].ToString());
            grvBatteryPendingForIssue.Columns[4].Visible = false;
            grvBatteryPendingForIssue.Visible = false;
            if (p.View)
            {
                grvBatteryPendingForIssue.Visible = true;
                grvBatteryPendingForIssue.Columns[4].Visible = false;
            }

            if (p.Add)
            {
                grvBatteryPendingForIssue.Visible = true;
                grvBatteryPendingForIssue.Columns[4].Visible = true;
            }
        }
    }

    private void FillInventoryVehicles()
    {
        try
        {
            _fmsg.UserDistrictId = Convert.ToInt32(Session["UserdistrictId"].ToString());
            var ds = _fmsg.GetVehicleNumber();
            if (ds == null) return;
            _helper.FillDropDownHelperMethodWithDataSet(ds, "VehicleNumber", "VehicleID", null, ddlInventoryBatteryIssueVehicles);
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    protected void ddlInventoryBatteryIssueVehicles_SelectedIndexChanged(object sender, EventArgs e)
    {
        switch (ddlInventoryBatteryIssueVehicles.SelectedIndex)
        {
            case 0:
                grvBatteryPendingForIssue.Visible = false;
                break;
            default:
                FillGrid_BatteryForIssue(3, Convert.ToInt32(ddlInventoryBatteryIssueVehicles.SelectedValue));
                grvBatteryPendingForIssue.Visible = true;
                break;
        }
    }

    private void FillGrid_BatteryForIssue(int fleetInventoryItemId, int vehicleId)

    {
        var ds = ObjFmsInvBatIss.GetBatteryPendingForIssue(fleetInventoryItemId, vehicleId);
        if (ds == null) throw new ArgumentNullException(nameof(ds));
        grvBatteryPendingForIssue.DataSource = ds.Tables[0];
        grvBatteryPendingForIssue.DataBind();
        grvBatteryPendingForIssue.Visible = true;
    }

    protected void BtnViewDetails_Click(object sender, EventArgs e)
    {
    }

    protected void btnOk_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow item in grvBatteryIssueDetailsPopup.Rows)
        {
            var ddl = (DropDownList) item.FindControl("ddlNewBatteryNumber");
            if (ddl == null) throw new ArgumentNullException(nameof(ddl));
            if (ddl.SelectedIndex != 0)
            {
                var newBatteryNumber = Convert.ToString(((DropDownList) item.FindControl("ddlNewBatteryNumber")).SelectedItem);
                var batteryPosition = Convert.ToString(grvBatteryIssueDetailsPopup.Rows[0].Cells[0].Text);
                var issuedQuantity = Convert.ToInt32(((TextBox) item.FindControl("txtBatteryIssuedQty")).Text);
                var ds = _fmsg.GetRegistrationDate(Convert.ToInt32(ddlInventoryBatteryIssueVehicles.SelectedValue));
                if (ds == null) throw new ArgumentNullException(nameof(ds));
                if (DateTime.Parse(ds.Tables[0].Rows[0]["RegDate"].ToString()) < DateTime.Parse(txtDcDate.Text))
                {
                    InsertBatteryIssueDetails(Convert.ToInt32(txtInvReqIdPopUp.Text), Convert.ToInt32(txtDcNumberPopup.Text), Convert.ToDateTime(txtDcDate.Text), Convert.ToString(txtCourierName.Text), Convert.ToString(txtRemarks.Text), issuedQuantity, Convert.ToInt32(txtBatIssVehicleID.Text), newBatteryNumber, batteryPosition);
                }
                else
                {
                    Show("Dc Date should be greater than Registration Date " + ds.Tables[0].Rows[0]["RegDate"]);
                    gv_ModalPopupExtenderBatteryIssue.Show();
                    break;
                }
            }
            else
            {
                Show("Please Select Battery Number");
                gv_ModalPopupExtenderBatteryIssue.Show();
                break;
            }
        }

        ClearControls();
        FillGrid_BatteryForIssue(3, Convert.ToInt32(ddlInventoryBatteryIssueVehicles.SelectedValue));
        Show("Battery Issued Successfully");
    }

    private void InsertBatteryIssueDetails(int reqId, int dcNumber, DateTime dcDate, string courierName, string remarks, int issuedQuantity, int vehicleId, string newBatteryNumber, string batteryPosition)
    {
        var res = ObjFmsInvBatIss.InsertBatteryIssueDetails(reqId, dcNumber, dcDate, courierName, remarks, issuedQuantity, vehicleId, newBatteryNumber, batteryPosition);
        if (res > 0)
        {
            var strFmsScript = "<script language=JavaScript>alert('" + "Battery ISSUED" + "')</script>";
            ClientScript.RegisterStartupScript(GetType(), "Success", strFmsScript);
        }
        else
        {
            var strFmsScript = "<script language=JavaScript>alert('" + "Failure " + "')</script>";
            ClientScript.RegisterStartupScript(GetType(), "failure", strFmsScript);
        }
    }

    protected void grvBatteryIssueDetailsPopup_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    ((TextBox) e.Row.FindControl("txtBatteryIssuedQty")).Attributes.Add("onblur", "javascript:ValidateIssueQty('" + ((TextBox) e.Row.FindControl("txtBatteryIssuedQty")).ClientID + "','" + e.Row.Cells[3].Text + "')");
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "dsad", "var IssuedQuantity = '" + ((TextBox) e.Row.FindControl("txtBatteryIssuedQty")).ClientID + "'", true);
                    //Filling Drop Down in Popup Gridview
                    var ds = ObjFmsInvBatIss.IFillNewBatteryNumbers();
                    if (ds == null) throw new ArgumentNullException(nameof(ds));
                    var ddl = (DropDownList) e.Row.FindControl("ddlNewBatteryNumber");
                    _helper.FillDropDownHelperMethodWithDataSet(ds, "Battery Number", "Battery_Id", ddl);
                    break;
            }
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    protected void btnNo_Click(object sender, EventArgs e)
    {
        ClearControls();
        gv_ModalPopupExtenderBatteryIssue.Hide();
    }

    protected void grvBatteryPendingForIssue_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grvBatteryPendingForIssue.PageIndex = e.NewPageIndex;
        FillGrid_BatteryForIssue(3, Convert.ToInt32(ddlInventoryBatteryIssueVehicles.SelectedValue));
    }

    private void ClearControls()
    {
        txtDcNumberPopup.Text = "";
        txtDcDate.Text = "";
        txtRemarks.Text = "";
        txtCourierName.Text = "";
    }

    public void Show(string message)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('" + message + "');", true);
    }

    protected void grvBatteryPendingForIssue_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        var id = Convert.ToInt32(e.CommandArgument.ToString());
        var ds = ObjFmsInvBatIss.GetGridBatteryIssuePopup(id);
        if (ds == null) throw new ArgumentNullException(nameof(ds));
        txtInvReqIdPopUp.Text = ds.Tables[0].Rows[0][0].ToString();
        txtBatIssVehicleID.Text = ds.Tables[0].Rows[0][1].ToString();
        grvBatteryIssueDetailsPopup.DataSource = ds.Tables[1];
        grvBatteryIssueDetailsPopup.DataBind();
        gv_ModalPopupExtenderBatteryIssue.Show();
    }
}