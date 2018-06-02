using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using GvkFMSAPP.BLL.VAS_BLL;

public partial class VasOffroadFleetManager : Page
{
    private readonly Helper _helper = new Helper();
    private readonly VASGeneral _obj = new VASGeneral();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Name"] == null) Response.Redirect("Login.aspx");
        if (!IsPostBack) BindGridView();
    }

    private void BindGridView()
    {
        var ds = _obj.GetVasOffroadFleetManager();
        if (ds == null) throw new ArgumentNullException(nameof(ds));
        gvVasOffroad.DataSource = ds.Tables[0];
        gvVasOffroad.DataBind();
    }

    protected void gvVasOffroad_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvVasOffroad.PageIndex = e.NewPageIndex;
        BindGridView();
    }

    public void Show(string message)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('" + message + "');", true);
    }

    protected void gvVasOffroad_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == null) return;
        try
        {
            switch (e.CommandName.ToUpper())
            {
                case "APPROVE":
                {
                    var row = (GridViewRow) ((WebControl) e.CommandSource).Parent.Parent;
                    var txtMm = row.FindControl("txtApprovedCost") as TextBox;
                    if (txtMm == null || txtMm.Text != "")
                    {
                        _obj.OFFid = int.Parse(((Label) row.FindControl("lblOffroadID")).Text);
                        _obj.VehicleNumber = ((Label) row.FindControl("lblVehicle_No")).Text;
                        _obj.District = ((Label) row.FindControl("lblDistrict")).Text;
                        _obj.OffRoadDate = DateTime.Parse(((Label) row.FindControl("lblDoOffRoad")).Text);
                        _obj.ReasonForOffRoad = ((Label) row.FindControl("lblReason")).Text;
                        _obj.totEstimated = ((Label) row.FindControl("lblEstimatedCost")).Text;
                        _obj.CheqAmt = ((TextBox) row.FindControl("txtApprovedCost")).Text;
                        var i = _obj.INsertVasOffFM();
                        if (i != 0)
                        {
                            Show("Approved Successfully");
                            BindGridView();
                        }
                    }
                    else
                    {
                        Show("Please enter Approved Cost");
                        txtMm.Focus();
                    }

                    break;
                }
                case "REJECT":
                {
                    var row = (GridViewRow) ((WebControl) e.CommandSource).Parent.Parent;
                    mpeReasonDetails.Show();
                    ViewState["OFFid"] = int.Parse(((Label) row.FindControl("lblOffroadID")).Text);
                    ViewState["VehicleNumber"] = ((Label) row.FindControl("lblVehicle_No")).Text;
                    ViewState["District"] = ((Label) row.FindControl("lblDistrict")).Text;
                    ViewState["OffRoadDate"] = DateTime.Parse(((Label) row.FindControl("lblDoOffRoad")).Text);
                    ViewState["ReasonForOffRoad"] = ((Label) row.FindControl("lblReason")).Text;
                    ViewState["totEstimated"] = ((Label) row.FindControl("lblEstimatedCost")).Text;
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    protected void btnReason_Click1(object sender, EventArgs e)
    {
        _obj.OFFid = (int) ViewState["OFFid"];
        _obj.VehicleNumber = (string) ViewState["VehicleNumber"];
        _obj.District = (string) ViewState["District"];
        _obj.OffRoadDate = (DateTime) ViewState["OffRoadDate"];
        _obj.ReasonForOffRoad = (string) ViewState["ReasonForOffRoad"];
        _obj.totEstimated = (string) ViewState["totEstimated"];
        _obj.CheqAmt = (string) ViewState["CheqAmt"];
        _obj.VenName = txtrejectReason.Text;
        var i = _obj.InsertVasOffFMRejected();
        if (i != 0)
        {
            Show("Rejected Successfully");
            BindGridView();
            txtrejectReason.Text = "";
        }
        else
        {
            Show("Rejection Declined");
        }
    }

    protected void btnDoWork_Click(object sender, EventArgs e)
    {
        txtrejectReason.Text = "";
    }

    protected void gvVasOffroad_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow) return;
        int cost;
        if (int.TryParse(((Label) e.Row.FindControl("lblEstimatedCost")).Text, out cost))
            if (cost > 25000)
            {
                var lnkApprove = (LinkButton) e.Row.FindControl("lnkApprove");
                var lnkReject = (LinkButton) e.Row.FindControl("lnkReject");
                var txtApprovedCost = (TextBox) e.Row.FindControl("txtApprovedCost");
                lnkApprove.Visible = false;
                lnkReject.Visible = false;
                txtApprovedCost.Visible = false;
            }
    }
}