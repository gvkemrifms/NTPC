using System;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using GvkFMSAPP.BLL.VAS_BLL;
using Label = System.Web.UI.WebControls.Label;
using Page = System.Web.UI.Page;

public partial class NonOffroadApprovalPage : Page
{
    private readonly VASGeneral _objBll = new VASGeneral();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Name"] == null) Response.Redirect("Login.aspx");
        if (!IsPostBack)
        {
            BindGridview();
            txtApprovalDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtApprovalDate.Enabled = false;
        }
    }

    public void BindGridview()
    {
        var ds = _objBll.BindForNonOffroadApproval();
        if (ds == null) throw new ArgumentNullException(nameof(ds));
        gvNonOffroadApprovalPage.DataSource = ds.Tables[0];
        gvNonOffroadApprovalPage.DataBind();
    }

    protected void gvNonOffroadApprovalPage_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == null) return;
        switch (e.CommandName.ToUpper())
        {
            case "VEHAPPROVE":
            {
                var row = (GridViewRow) ((WebControl) e.CommandSource).Parent.Parent;
                if (_objBll != null)
                {
                    _objBll.District = ((Label) row.FindControl("lblDistrict")).Text;
                    _objBll.SrcVehNo = ((Label) row.FindControl("lblVehicle_No")).Text;
                    _objBll.NonOffBillNo = ((Label) row.FindControl("lblBillNo")).Text;
                    _objBll.NonOffBillDate = Convert.ToDateTime(((Label) row.FindControl("lblBillDate")).Text);
                    if (((Label) row.FindControl("lblDownTime")).Text != "NA") _objBll.OffRoadDate = Convert.ToDateTime(((Label) row.FindControl("lblDownTime")).Text);
                    if (((Label) row.FindControl("lblUpTime")).Text != "NA") _objBll.UpTime = Convert.ToDateTime(((Label) row.FindControl("lblUpTime")).Text);
                    _objBll.NonOffAmount = ((Label) row.FindControl("lblAmount")).Text;
                    _objBll.BaseLocId = Convert.ToInt64(((Label) row.FindControl("lblBrkDwn")).Text);
                    _objBll.VenName = ((Label) row.FindControl("lblVendorName")).Text;
                    _objBll.UpdatedDate = Convert.ToDateTime(txtApprovalDate.Text);
                    var x = _objBll.NonOffApproved();
                    switch (x)
                    {
                        case 0:
                            Show("Vehicle approved unsuccessfull");
                            break;
                        default:
                            Show("Vehicle approved successfully");
                            txtApprovalDate.Text = DateTime.Now.ToString(CultureInfo.InvariantCulture);
                            txtApprovalDate.Enabled = false;
                            BindGridview();
                            break;
                    }
                }

                break;
            }
            case "VEHREJECT":
            {
                var row = (GridViewRow) ((WebControl) e.CommandSource).Parent.Parent;
                ViewState["District"] = ((Label) row.FindControl("lblDistrict")).Text;
                ViewState["VehNo"] = ((Label) row.FindControl("lblVehicle_No")).Text;
                ViewState["BillNo"] = ((Label) row.FindControl("lblBillNo")).Text;
                ViewState["BillDate"] = ((Label) row.FindControl("lblBillDate")).Text;
                if (((Label) row.FindControl("lblDownTime")).Text != "NA") ViewState["downtime"] = ((Label) row.FindControl("lblDownTime")).Text;
                if (((Label) row.FindControl("lblUpTime")).Text != "NA") ViewState["Uptime"] = ((Label) row.FindControl("lblUpTime")).Text;
                ViewState["Amount"] = ((Label) row.FindControl("lblAmount")).Text;
                ViewState["brkdwn"] = ((Label) row.FindControl("lblBrkDwn")).Text;
                mpeReasonDetails.Show();
                break;
            }
        }
    }

    public void Show(string message)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('" + message + "');", true);
    }

    protected void gvNonOffroadApprovalPage_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvNonOffroadApprovalPage.PageIndex = e.NewPageIndex;
        BindGridview();
    }

    protected void lnkChangeDate_Click(object sender, EventArgs e)
    {
        txtApprovalDate.Text = "";
        txtApprovalDate.Enabled = true;
        txtApprovalDate.Focus();
    }

    protected void gvNonOffroadApprovalPage_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:
                var lnkReject = (LinkButton) e.Row.FindControl("lnkReject");
                lnkReject.CommandArgument = e.Row.RowIndex.ToString();
                break;
        }
    }

    protected void btnDoWork_Click(object sender, EventArgs e)
    {
    }

    protected void btnReason_Click(object sender, EventArgs e)
    {
        _objBll.District = (string) ViewState["District"];
        _objBll.SrcVehNo = (string) ViewState["VehNo"];
        _objBll.NonOffBillNo = (string) ViewState["BillNo"];
        _objBll.NonOffBillDate = Convert.ToDateTime(ViewState["BillDate"]);
        _objBll.OffRoadDate = Convert.ToDateTime(ViewState["downtime"]);
        _objBll.UpTime = Convert.ToDateTime(ViewState["Uptime"]);
        _objBll.NonOffAmount = (string) ViewState["Amount"];
        _objBll.CourierName = txtrejectReason.Text;
        _objBll.UpdatedDate = Convert.ToDateTime(txtApprovalDate.Text);
        _objBll.BaseLocationId = Convert.ToInt32(ViewState["brkdwn"]);
        var x = _objBll.NonOffandOffRejected();
        switch (x)
        {
            case 0:
                Show("Vehicle Rejected unsuccessfull");
                break;
            default:
                Show("Vehicle Rejected successfully");
                txtApprovalDate.Text = DateTime.Now.ToString(CultureInfo.InvariantCulture);
                txtApprovalDate.Enabled = false;
                txtrejectReason.Text = "";
                BindGridview();
                break;
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        txtrejectReason.Text = "";
    }
}