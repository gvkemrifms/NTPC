using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using GvkFMSAPP.BLL;
using GvkFMSAPP.BLL.Others;
using GvkFMSAPP.DLL;
using GvkFMSAPP.PL;

public partial class VehicleDecommissionApproval : Page
{
    private readonly Helper _helper = new Helper();
    private readonly VehicleDecommissionApprovalBLL _vehicleApprovalBol = new VehicleDecommissionApprovalBLL();
    private readonly VehicleDecommissionProposalBLL _vehicleProposalBol = new VehicleDecommissionProposalBLL();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Name"] == null) Response.Redirect("Login.aspx");
        if (!IsPostBack)
        {
            var dsPerms = (DataSet) Session["PermissionsDS"];
            if (dsPerms == null) throw new ArgumentNullException(nameof(dsPerms));
            dsPerms.Tables[0].DefaultView.RowFilter = "Url='" + Page.Request.Url.Segments[Page.Request.Url.Segments.Length - 1] + "'";
            var p = new PagePermissions(dsPerms, dsPerms.Tables[0].DefaultView[0]["Url"].ToString(), dsPerms.Tables[0].DefaultView[0]["Title"].ToString());
            btnAccept.Attributes.Add("onclick", "return validation(this,'" + btnAccept.ID + "')");
            btnReject.Attributes.Add("onclick", "return validation(this,'" + btnReject.ID + "')");
            FillVehicleDecommisionApproval();
            grdVehicleDecompositionApproval.Visible = false;
            pnlVehicleDecommissionApproval.Visible = false;
            if (p.Approve)
            {
                grdVehicleDecompositionApproval.Visible = true;
                pnlVehicleDecommissionApproval.Visible = true;
            }
        }
    }

    protected void FillVehicleDecommisionApproval()
    {
        grdVehicleDecompositionApproval.DataSource = _vehicleApprovalBol.FillVehicleDecommissionApproval();
        grdVehicleDecompositionApproval.DataBind();
    }

    protected void ClearTextBoxes()
    {
        txtVehicleNumber.Text = "";
        txtOffRoadDate.Text = "";
        txtDateOfRegistration.Text = "";
        txtDateOfLaunching.Text = "";
        txtSurveyDate.Text = "";
        txtSurveyBy.Text = "";
        txtProposedRemarks.Text = "";
        txtDecommisionDate.Text = "";
        txtApproveRejectedRemarks.Text = "";
        txtDateofPurchase.Text = "";
    }

    protected void grdVehicleDecompositionApproval_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == null) return;
        try
        {
            switch (e.CommandName)
            {
                case "vehicleApproval":
                    _vehicleProposalBol.VehicleDecommisionProposalID = Convert.ToInt32(e.CommandArgument);
                    var ds = _vehicleProposalBol.GetVehicleDecommissionProposalByVehicleProposalId();
                    ViewState["VehicleId"] = ds.Tables[0].Rows[0]["VehicleId"].ToString();
                    ViewState["vi_LocationId"] = ds.Tables[0].Rows[0]["vi_LocationId"].ToString();
                    ViewState["VehicleProposalId"] = ds.Tables[0].Rows[0]["VehicleProposalId"].ToString();
                    ViewState["TotalDistanceTravelled"] = ds.Tables[0].Rows[0]["TotalDistanceTravelled"].ToString();
                    txtVehicleNumber.Text = ds.Tables[0].Rows[0]["VehicleNumber"].ToString();
                    txtOffRoadDate.Text = ds.Tables[0].Rows[0]["OffRoadDate"].ToString();
                    txtDateOfRegistration.Text = ds.Tables[0].Rows[0]["DateOfRegistration"].ToString();
                    txtDateofPurchase.Text = ds.Tables[0].Rows[0]["DateOfPurchase"].ToString();
                    ViewState["DateOfPurchase"] = txtDateofPurchase.Text;
                    txtDateOfLaunching.Text = ds.Tables[0].Rows[0]["DateOfLaunching"].ToString();
                    txtSurveyDate.Text = ds.Tables[0].Rows[0]["SurveyDate"].ToString();
                    txtSurveyBy.Text = ds.Tables[0].Rows[0]["SurveyBy"].ToString();
                    ViewState["SurveyRemark"] = ds.Tables[0].Rows[0]["SurveyRemark"].ToString();
                    ViewState["TotalMaintenanceExpenses"] = ds.Tables[0].Rows[0]["TotalMaintenanceExpenses"].ToString();
                    txtProposedRemarks.Text = ds.Tables[0].Rows[0]["ProposedRemark"].ToString();
                    break;
            }
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    protected void btnAccept_Click(object sender, EventArgs e)
    {
        try
        {
            _vehicleProposalBol.vi_LocationId = Convert.ToInt32(ViewState["vi_LocationId"]);
            _vehicleProposalBol.VehicleDecommisionProposalID = Convert.ToInt32(ViewState["VehicleProposalId"]);
            _vehicleProposalBol.VehicleID = Convert.ToInt32(ViewState["VehicleId"]);
            _vehicleProposalBol.OffRoadDateTime = DateTime.Parse(txtOffRoadDate.Text);
            _vehicleProposalBol.TotalKmCovered = ViewState["TotalDistanceTravelled"].ToString();
            _vehicleProposalBol.DateOfRegistration = DateTime.Parse(txtDateOfRegistration.Text);
            _vehicleProposalBol.DateofLaunching = DateTime.Parse(txtDateOfLaunching.Text);
            _vehicleProposalBol.DateOfPurchase = DateTime.Parse(ViewState["DateOfPurchase"].ToString());
            _vehicleProposalBol.SurveyDate = DateTime.Parse(txtSurveyDate.Text);
            _vehicleProposalBol.SurveyBy = txtSurveyBy.Text;
            _vehicleProposalBol.DateOfPurchase = DateTime.Parse(txtDateofPurchase.Text);
            _vehicleProposalBol.ProposedRemarks = txtProposedRemarks.Text;
            _vehicleProposalBol.ApprovedRejectRemarks = txtApproveRejectedRemarks.Text;
            _vehicleProposalBol.IsApproved = 3;
            _vehicleProposalBol.SurveyorRemarks = ViewState["SurveyRemark"].ToString();
            _vehicleProposalBol.DeCommissionDate = Convert.ToDateTime(txtDecommisionDate.Text);
            _vehicleProposalBol.TotalMaintenanceExpenses = ViewState["TotalMaintenanceExpenses"].ToString();
            _vehicleProposalBol.UpdateVehicleDecommissionProposal();
            FillVehicleDecommisionApproval();
            ClearTextBoxes();
            Show("Vehicle Approved");
        }
        catch (Exception ex)
        {
            ErrorHandler.ErrorsEntry(ex.GetBaseException().ToString(), "class: AttachDocuments;Method: btnAttachFiles_Click()-InsertFillAttachmentToVehicle", 0);
        }
    }

    protected void btnReject_Click(object sender, EventArgs e)
    {
        try
        {
            _vehicleProposalBol.VehicleDecommisionProposalID = Convert.ToInt32(ViewState["VehicleProposalId"]);
            _vehicleProposalBol.VehicleID = Convert.ToInt32(ViewState["VehicleId"]);
            _vehicleProposalBol.OffRoadDateTime = DateTime.Parse(txtOffRoadDate.Text);
            _vehicleProposalBol.TotalKmCovered = ViewState["TotalDistanceTravelled"].ToString();
            _vehicleProposalBol.DateOfRegistration = DateTime.Parse(txtDateOfRegistration.Text);
            _vehicleProposalBol.DateofLaunching = DateTime.Parse(txtDateOfLaunching.Text);
            _vehicleProposalBol.DateOfPurchase = DateTime.Parse(ViewState["DateOfPurchase"].ToString());
            _vehicleProposalBol.SurveyDate = DateTime.Parse(txtSurveyDate.Text);
            _vehicleProposalBol.SurveyBy = txtSurveyBy.Text;
            _vehicleProposalBol.ProposedRemarks = txtProposedRemarks.Text;
            _vehicleProposalBol.ApprovedRejectRemarks = txtApproveRejectedRemarks.Text;
            _vehicleProposalBol.IsApproved = 2;
            _vehicleProposalBol.SurveyorRemarks = ViewState["SurveyRemark"].ToString();
            _vehicleProposalBol.DeCommissionDate = DateTime.Now; //Convert.ToDateTime(txtDecommisionDate.Text);
            _vehicleProposalBol.TotalMaintenanceExpenses = ViewState["TotalMaintenanceExpenses"].ToString();
            _vehicleProposalBol.UpdateVehicleDecommissionProposal();
            FillVehicleDecommisionApproval();
            ClearTextBoxes();
            Show("Vehicle Rejected");
        }
        catch (Exception ex)
        {
            ErrorHandler.ErrorsEntry(ex.GetBaseException().ToString(), "class: AttachDocuments;Method: btnAttachFiles_Click()-InsertFillAttachmentToVehicle", 0);
        }
    }

    public void Show(string message)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('" + message + "');", true);
    }
}