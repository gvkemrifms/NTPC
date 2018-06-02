using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using GvkFMSAPP.BLL;
using GvkFMSAPP.PL;

public partial class TyreReceipt : Page
{
    private readonly FMSGeneral _fmsg = new FMSGeneral();
    private readonly Helper _helper = new Helper();
    public IInventory ObjTyreRecp = new FMSInventory();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Name"] == null) Response.Redirect("Login.aspx");
        if (!IsPostBack)
        {
            FillInventoryVehicles();
            btnOk.Attributes.Add("onclick", "return validation()");
            txtRemarks.Attributes.Add("onkeypress", "javascript:return remark(this,event)");
            var dsPerms = (DataSet) Session["PermissionsDS"];
            dsPerms.Tables[0].DefaultView.RowFilter = "Url='" + Page.Request.Url.Segments[Page.Request.Url.Segments.Length - 1] + "'";
            var p = new PagePermissions(dsPerms, dsPerms.Tables[0].DefaultView[0]["Url"].ToString(), dsPerms.Tables[0].DefaultView[0]["Title"].ToString());
            grvTyreDetailsForReceipt.Visible = false;
            if (p.View)
            {
                grvTyreDetailsForReceipt.Visible = true;
                grvTyreDetailsForReceipt.Columns[5].Visible = false;
            }

            if (p.Add)
            {
                grvTyreDetailsForReceipt.Visible = true;
                grvTyreDetailsForReceipt.Columns[5].Visible = true;
            }
        }
    }

    private void FillInventoryVehicles()
    {
        try
        {
            _fmsg.UserDistrictId = Convert.ToInt32(Session["UserdistrictId"].ToString());
            var ds = _fmsg.GetVehicleNumber();
            if (ds != null) _helper.FillDropDownHelperMethodWithDataSet(ds, "VehicleNumber", "VehicleID", null, ddlInventoryTyreReceiptVehicles);
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    private void FillGrid_TyreDetailsForReceipt(int fleetInventoryItemId, int vehicleId)
    {
        var ds = ObjTyreRecp.GetTyreDetailsForReceipt(1, vehicleId);
        if (ds == null) throw new ArgumentNullException(nameof(ds));
        grvTyreDetailsForReceipt.DataSource = ds.Tables[0];
        grvTyreDetailsForReceipt.DataBind();
    }

    protected void ddlInventoryTyreReceiptVehicles_SelectedIndexChanged(object sender, EventArgs e)
    {
        switch (ddlInventoryTyreReceiptVehicles.SelectedIndex)
        {
            case 0:
                grvTyreDetailsForReceipt.Visible = false;
                break;
            default:
                FillGrid_TyreDetailsForReceipt(1, Convert.ToInt32(ddlInventoryTyreReceiptVehicles.SelectedValue));
                grvTyreDetailsForReceipt.Visible = true;
                break;
        }
    }

    protected void BtnViewDetails_Click(object sender, EventArgs e)
    {
        var btnViewDetails = sender as LinkButton;
        if (btnViewDetails != null)
        {
            var rowIndex = Convert.ToInt32(btnViewDetails.Attributes["RowIndex"]);
            txtTyreRecVehicleNo.Text = grvTyreDetailsForReceipt.Rows[rowIndex].Cells[0].Text;
            txtTyreRecDistrict.Text = grvTyreDetailsForReceipt.Rows[rowIndex].Cells[1].Text;
        }
    }

    protected void grvTyreDetailsForReceipt_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grvTyreDetailsForReceipt.PageIndex = e.NewPageIndex;
        FillGrid_TyreDetailsForReceipt(1, Convert.ToInt32(ddlInventoryTyreReceiptVehicles.SelectedValue));
    }

    protected void btnOk_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow item in grvTyreReceiptDetailsPopup.Rows)
        {
            var invItemIssueId = Convert.ToInt32(txtIssueID.Text);
            var ds = ObjTyreRecp.GetGridTyreReceiptPopup(invItemIssueId);
            var detId = Convert.ToInt32(ds.Tables[1].Rows[0][2].ToString());
            var tyreNumber = ds.Tables[1].Rows[0][3].ToString();
            var make = ds.Tables[1].Rows[0][4].ToString();
            var model = ds.Tables[1].Rows[0][5].ToString();
            InsertTyreReceiptDetails(invItemIssueId, Convert.ToString(txtTyreRecVehicleNo.Text), Convert.ToString(txtTyreRecDistrict.Text), Convert.ToInt32(txtTyreDCNumber.Text), Convert.ToDateTime(txtTyreDCDate.Text), Convert.ToString(txtTyreRecCourierName.Text), Convert.ToString(txtRemarks.Text), Convert.ToDateTime(txtTyreRecDate.Text), detId, make, model, tyreNumber);
        }

        ClearControls();
        FillGrid_TyreDetailsForReceipt(1, Convert.ToInt32(ddlInventoryTyreReceiptVehicles.SelectedValue));
        Show("Tyre Receipt Generated");
    }

    private void InsertTyreReceiptDetails(int invItemIssueId, string vehicleNum, string district, int dcNumber, DateTime dcDate, string courierName, string remarks, DateTime receivedDate, int detId, string make, string model, string tyreNumber)
    {
        var res = ObjTyreRecp.InsertTyreReceiptDetails(invItemIssueId, vehicleNum, district, dcNumber, dcDate, courierName, remarks, receivedDate, detId, make, model, tyreNumber);
        if (res > 0)
        {
            var strFmsScript = "<script language=JavaScript>alert('" + "Tyre Receipt Details Saved" + "')</script>";
            ClientScript.RegisterStartupScript(GetType(), "Success", strFmsScript);
        }
        else
        {
            var strFmsScript = "<script language=JavaScript>alert('" + "Failure " + "')</script>";
            ClientScript.RegisterStartupScript(GetType(), "failure", strFmsScript);
        }
    }

    protected void btnNo_Click(object sender, EventArgs e)
    {
        ClearControls();
        gv_ModalPopupExtenderTyreReceipt.Hide();
    }

    private void ClearControls()
    {
        txtTyreRecDate.Text = "";
        txtRemarks.Text = "";
    }

    public void Show(string message)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('" + message + "');", true);
    }

    protected void grvTyreDetailsForReceipt_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        var id = Convert.ToInt32(e.CommandArgument.ToString());
        txtIssueID.Text = e.CommandArgument.ToString();
        var ds = ObjTyreRecp.GetGridTyreReceiptPopup(id);
        if (ds == null) throw new ArgumentNullException(nameof(ds));
        txtTyreDCNumber.Text = ds.Tables[0].Rows[0][1].ToString();
        txtTyreDCDate.Text = ds.Tables[0].Rows[0][2].ToString();
        txtTyreRecCourierName.Text = ds.Tables[0].Rows[0][3].ToString();
        txtTyreRecHORemarks.Text = ds.Tables[0].Rows[0][4].ToString();
        grvTyreReceiptDetailsPopup.DataSource = ds.Tables[1];
        grvTyreReceiptDetailsPopup.DataBind();
        gv_ModalPopupExtenderTyreReceipt.Show();
    }
}