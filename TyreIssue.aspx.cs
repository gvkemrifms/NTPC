using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using GvkFMSAPP.BLL;
using GvkFMSAPP.PL;

public partial class TyreIssue : Page
{
    private readonly FMSGeneral _fmsg = new FMSGeneral();
    private readonly Helper _helper = new Helper();
    public IInventory ObjTyreIssue = new FMSInventory();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Name"] == null) Response.Redirect("Login.aspx");
        if (!IsPostBack)
        {
            FillInventoryVehicles();
            btnOk.Attributes.Add("onclick", "return validation()");
            txtTyreCost.Attributes.Add("onkeypress", "javascript:return isNumberKey(this,event)");
            txtDcNumberPopup.Attributes.Add("onkeypress", "javascript:return isNumberKey(this,event)");
            txtCourierName.Attributes.Add("onkeypress", "javascript:return OnlyAlphabets(this,event)");
            txtRemarks.Attributes.Add("onkeypress", "javascript:return remark(this,event)");
            var dsPerms = (DataSet) Session["PermissionsDS"];
            if (dsPerms == null) throw new ArgumentNullException(nameof(dsPerms));
            dsPerms.Tables[0].DefaultView.RowFilter = "Url='" + Page.Request.Url.Segments[Page.Request.Url.Segments.Length - 1] + "'";
            var p = new PagePermissions(dsPerms, dsPerms.Tables[0].DefaultView[0]["Url"].ToString(), dsPerms.Tables[0].DefaultView[0]["Title"].ToString());
            grvTyrePendingForIssue.Columns[2].Visible = false;
            grvTyrePendingForIssue.Visible = false;
            if (p.View)
            {
                grvTyrePendingForIssue.Visible = true;
                grvTyrePendingForIssue.Columns[4].Visible = false;
            }

            if (p.Add)
            {
                grvTyrePendingForIssue.Visible = true;
                grvTyrePendingForIssue.Columns[4].Visible = true;
            }
        }
    }

    private void FillInventoryVehicles()
    {
        try
        {
            _fmsg.UserDistrictId = Convert.ToInt32(Session["UserdistrictId"].ToString());
            var ds = _fmsg.GetVehicleNumber();
            if (ds != null) _helper.FillDropDownHelperMethodWithDataSet(ds, "VehicleNumber", "VehicleID", null, ddlInventoryTyreIssueVehicles);
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    protected void ddlInventoryTyreIssueVehicles_SelectedIndexChanged(object sender, EventArgs e)
    {
        switch (ddlInventoryTyreIssueVehicles.SelectedIndex)
        {
            case 0:
                grvTyrePendingForIssue.Visible = false;
                break;
            default:
                FillGrid_TyreForIssue(1, Convert.ToInt32(ddlInventoryTyreIssueVehicles.SelectedValue));
                Session["vehId"] = Convert.ToInt32(ddlInventoryTyreIssueVehicles.SelectedValue);
                grvTyrePendingForIssue.Visible = true;
                break;
        }
    }

    private void FillGrid_TyreForIssue(int fleetInventoryItemId, int vehicleId)
    {
        try
        {
            var ds = ObjTyreIssue.GetTyrePendingForIssue(1, vehicleId);
            if (ds == null) throw new ArgumentNullException(nameof(ds));
            grvTyrePendingForIssue.DataSource = ds.Tables[0];
            grvTyrePendingForIssue.DataBind();
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    protected void BtnViewDetails_Click(object sender, EventArgs e)
    {
    }

    protected void grvTyrePendingForIssue_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grvTyrePendingForIssue.PageIndex = e.NewPageIndex;
        FillGrid_TyreForIssue(1, Convert.ToInt32(ddlInventoryTyreIssueVehicles.SelectedValue));
    }

    protected void grvTyreIssueDetailsPopup_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            var ds = ObjTyreIssue.IFillTyreNumbers();
            if (ds == null) throw new ArgumentNullException(nameof(ds));
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    var ddl = (DropDownList) e.Row.FindControl("ddlTyreNumber");
                    _helper.FillDropDownHelperMethodWithDataSet(ds, "TyreNumber", "Tyre_Id", ddl);
                    break;
            }
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    private void InsertTyreIssueDetails(int fleetInventoryReqId, int dcNumber, DateTime dcDate, string courierName, string remarks, int totalTyreCost, string make, string model, int vehicleId, string tyreNumber, string tyrePosition)
    {
        var res = ObjTyreIssue.InsertTyreIssueDetails(fleetInventoryReqId, dcNumber, dcDate, courierName, remarks, totalTyreCost, make, model, vehicleId, tyreNumber, tyrePosition);
        if (res > 0)
        {
            var strFmsScript = "<script language=JavaScript>alert('" + "TYRE ISSUED" + "')</script>";
            ClientScript.RegisterStartupScript(GetType(), "Success", strFmsScript);
        }
        else
        {
            var strFmsScript = "<script language=JavaScript>alert('" + "FAILURE " + "')</script>";
            ClientScript.RegisterStartupScript(GetType(), "failure", strFmsScript);
        }
    }

    protected void btnOk_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow row in grvTyreIssueDetailsPopup.Rows)
        {
            UpdateSessionTable();
            var tyreIssue = (DataSet) Session["TyreIssue"];
            var fleetReqId = Convert.ToInt32(tyreIssue.Tables[1].Rows[0]["FleetInventoryReqID"].ToString());
            var make = tyreIssue.Tables[1].Rows[0]["Make"].ToString();
            var model = tyreIssue.Tables[1].Rows[0]["Model"].ToString();
            var vehId = Convert.ToInt32(Session["vehId"]);
            var ddl = (DropDownList) row.FindControl("ddlTyreNumber");
            if (ddl.SelectedIndex != 0)
            {
                var tyreNumber = Session["tyreNumber"].ToString();
                var tyrePosition = Convert.ToString(grvTyreIssueDetailsPopup.Rows[0].Cells[0].Text);
                var ds = _fmsg.GetRegistrationDate(vehId);
                if (DateTime.Parse(ds.Tables[0].Rows[0]["RegDate"].ToString()) >= DateTime.Parse(txtDcDate.Text))
                {
                    Show("DC date should be greater than Registration Date " + ds.Tables[0].Rows[0]["RegDate"]);
                    gv_ModalPopupExtenderTyreIssue.Show();
                }
                else
                {
                    InsertTyreIssueDetails(fleetReqId, Convert.ToInt32(txtDcNumberPopup.Text), Convert.ToDateTime(txtDcDate.Text), txtCourierName.Text, txtRemarks.Text, Convert.ToInt32(txtTyreCost.Text), make, model, vehId, tyreNumber, tyrePosition);
                    FillGrid_TyreForIssue(1, Convert.ToInt32(ddlInventoryTyreIssueVehicles.SelectedValue));
                    ClearControls();
                    gv_ModalPopupExtenderTyreIssue.Hide();
                    Show("Tyre Issued SuccessFully");
                }
            }
            else
            {
                Show("Please Select Tyre");
                gv_ModalPopupExtenderTyreIssue.Show();
                break;
            }
        }
    }

    public void Show(string message)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('" + message + "');", true);
    }

    protected void UpdateSessionTable()
    {
        foreach (GridViewRow gvrow in grvTyreIssueDetailsPopup.Rows)
        {
            var ddlSpPart = (DropDownList) gvrow.FindControl("ddlTyreNumber");
            Session["tyreNumber"] = ddlSpPart.SelectedItem;
        }
    }

    protected void btnNo_Click(object sender, EventArgs e)
    {
        ClearControls();
        gv_ModalPopupExtenderTyreIssue.Hide();
    }

    private void ClearControls()
    {
        txtTyreCost.Text = "";
        txtDcNumberPopup.Text = "";
        txtDcDate.Text = "";
        txtCourierName.Text = "";
        txtRemarks.Text = "";
        gv_ModalPopupExtenderTyreIssue.Hide();
    }

    protected void grvTyrePendingForIssue_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        var id = Convert.ToInt32(e.CommandArgument.ToString());
        var ds = ObjTyreIssue.GetGridTyreIssuePopup(id);
        if (ds == null) throw new ArgumentNullException(nameof(ds));
        txtTyreIssVehicleNumber.Text = ds.Tables[0].Rows[0][0].ToString();
        txtTyreIssDistrict.Text = ds.Tables[0].Rows[0][1].ToString();
        Session["TyreIssue"] = ds;
        grvTyreIssueDetailsPopup.DataSource = ds.Tables[1];
        grvTyreIssueDetailsPopup.DataBind();
        gv_ModalPopupExtenderTyreIssue.Show();
    }
}