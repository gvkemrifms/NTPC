using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using GvkFMSAPP.BLL;
using GvkFMSAPP.BLL.Others;
using GvkFMSAPP.DLL;
using GvkFMSAPP.PL;

public partial class VehicleDecommissionProposal : Page
{
    private readonly VehicleDecommissionApprovalBLL _vehicleApprovalBol = new VehicleDecommissionApprovalBLL();
    private readonly VehicleDecommissionProposalBLL _vehicleProposalBol = new VehicleDecommissionProposalBLL();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Name"] == null) Response.Redirect("Error.aspx");
        if (!IsPostBack)
        {
            var dsPerms = (DataSet) Session["PermissionsDS"];
            if (dsPerms == null) throw new ArgumentNullException(nameof(dsPerms));
            dsPerms.Tables[0].DefaultView.RowFilter = "Url='" + Page.Request.Url.Segments[Page.Request.Url.Segments.Length - 1] + "'";
            var p = new PagePermissions(dsPerms, dsPerms.Tables[0].DefaultView[0]["Url"].ToString(), dsPerms.Tables[0].DefaultView[0]["Title"].ToString());
            btnSave.Attributes.Add("onclick", "return validation(this,'" + btnSave.ID + "')");
            FillVehicleDecommisionproposalDetails();
            FillVehicleDecommissionApproval();
            pnlgrdViewVehicleDecommisionProposal.Visible = false;
            pnlVehicleDecommissionProposal.Visible = false;
            grdViewVehicleDecomissionApproval.Visible = p.View;
            if (p.Add)
            {
                pnlgrdViewVehicleDecommisionProposal.Visible = true;
                pnlVehicleDecommissionProposal.Visible = true;
                grdViewVehicleDecomissionApproval.Visible = true;
            }

            if (p.Modify)
            {
                pnlgrdViewVehicleDecommisionProposal.Visible = true;
                pnlVehicleDecommissionProposal.Visible = true;
                grdViewVehicleDecomissionApproval.Visible = true;
            }
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            _vehicleProposalBol.vi_LocationId = Convert.ToInt32(ViewState["vi_LocationId"]);
            _vehicleProposalBol.VehicleID = Convert.ToInt32(ViewState["VehicleId"]);
            _vehicleApprovalBol.OffRoadDateTime = DateTime.Now;
            _vehicleProposalBol.TotalKmCovered = txtTotalKmCovered.Text;
            _vehicleProposalBol.DateOfRegistration = DateTime.Parse(txtDateOfRegistration.Text);
            _vehicleProposalBol.DateofLaunching = DateTime.Parse(txtDateOfLaunching.Text);
            _vehicleProposalBol.DateOfPurchase = DateTime.Parse(txtDateOfPurchase.Text);
            _vehicleProposalBol.SurveyDate = DateTime.Parse(txtSurveyDate.Text);
            _vehicleProposalBol.SurveyBy = txtSurveyBy.Text;
            _vehicleProposalBol.SurveyorRemarks = txtSurveyRemarks.Text;
            _vehicleProposalBol.ProposedRemarks = txtProposedRemarks.Text;
            _vehicleProposalBol.TotalMaintenanceExpenses = txtTotalMaintenanceExpenses.Text;
            _vehicleProposalBol.NumberOfAccidents = int.Parse(txtNumberofAccidents.Text);
            _vehicleProposalBol.ApprovedRejectRemarks = "";
            _vehicleProposalBol.IsApproved = 1;
            _vehicleProposalBol.DeCommissionDate = DateTime.Now;
            _vehicleProposalBol.InsertVehicleDecommissionProposal();
            FillVehicleDecommisionproposalDetails();
            FillVehicleDecommissionApproval();
            ClearTextBox();
        }
        catch (Exception ex)
        {
            ErrorHandler.ErrorsEntry(ex.GetBaseException().ToString(), "class: AttachDocuments;Method: btnAttachFiles_Click()-InsertFillAttachmentToVehicle", 0);
        }
    }

    protected void FillVehicleDecommisionproposalDetails()
    {
        grdViewVehicleDecommisionProposal.DataSource = _vehicleProposalBol.FillVehicleDecommissionProposal();
        grdViewVehicleDecommisionProposal.DataBind();
    }

    protected void FillVehicleDecommissionApproval()
    {
        grdViewVehicleDecomissionApproval.DataSource = _vehicleApprovalBol.FillVehicleDecommissionApproval();
        grdViewVehicleDecomissionApproval.DataBind();
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        ClearTextBox();
    }

    protected void ClearTextBox()
    {
        lblMessage.Text = "";
        txtVehicleNumber.Text = "";
        txtTotalKmCovered.Text = "";
        txtDateOfRegistration.Text = "";
        txtDateOfPurchase.Text = "";
        txtDateOfLaunching.Text = "";
        txtSurveyDate.Text = "";
        txtSurveyBy.Text = "";
        txtSurveyRemarks.Text = "";
        txtProposedRemarks.Text = "";
        txtTotalMaintenanceExpenses.Text = "";
        txtNumberofAccidents.Text = "";
    }

    protected void grdViewVehicleDecommisionProposal_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "vehicleProposal":
                _vehicleProposalBol.vi_LocationId = Convert.ToInt32(e.CommandArgument);
                var ds = _vehicleProposalBol.GetVehicleDecommissionProposalByVehicleProposalIdByCTIAPPS();
                if (ds == null) throw new ArgumentNullException(nameof(ds));
                ViewState["VehicleId"] = ds.Tables[0].Rows[0]["VehicleId"].ToString();
                ViewState["vi_LocationId"] = ds.Tables[0].Rows[0]["vi_LocationId"].ToString();
                txtVehicleNumber.Text = ds.Tables[0].Rows[0]["VehicleNumber"].ToString();
                txtTotalKmCovered.Text = ds.Tables[0].Rows[0]["TotalKmCovered"].ToString();
                txtDateOfRegistration.Text = ds.Tables[0].Rows[0]["RegDate"].ToString();
                txtDateOfPurchase.Text = ds.Tables[0].Rows[0]["PurchaseDate"].ToString();
                break;
        }
    }

    protected void grdViewVehicleDecommisionProposal_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdViewVehicleDecommisionProposal.PageIndex = e.NewPageIndex;
        FillVehicleDecommisionproposalDetails();
    }
}