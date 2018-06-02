using System;
using System.Data;
using System.Diagnostics;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using GvkFMSAPP.BLL;
using GvkFMSAPP.DLL;
using GvkFMSAPP.PL;

public partial class HandOverToOwner : Page
{
    private readonly HandOverToOwnerBLL _handOvertoOwner = new HandOverToOwnerBLL();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Name"] == null) Response.Redirect("Login.aspx");
        if (!IsPostBack)
        {
            var dsPerms = (DataSet) Session["PermissionsDS"];
            if (dsPerms == null) throw new ArgumentNullException(nameof(dsPerms));
            dsPerms.Tables[0].DefaultView.RowFilter = "Url='" + Page.Request.Url.Segments[Page.Request.Url.Segments.Length - 1] + "'";
            var p = new PagePermissions(dsPerms, dsPerms.Tables[0].DefaultView[0]["Url"].ToString(), dsPerms.Tables[0].DefaultView[0]["Title"].ToString());
            btnSave.Attributes.Add("onclick", "return validation(this,'" + btnSave.ID + "')");
            FillTemporaryVehicle();
            FillHandOverVehicle();
            pnlgrdTemporaryVehicle.Visible = false;
            grdVehicleDecompositionApproval.Visible = false;
            switch (p.View)
            {
                case true:
                    grdVehicleDecompositionApproval.Visible = true;
                    break;
            }

            switch (p.Add)
            {
                case true:
                    pnlgrdTemporaryVehicle.Visible = true;
                    grdVehicleDecompositionApproval.Visible = true;
                    break;
            }

            switch (p.Modify)
            {
                case true:
                    break;
            }

            switch (p.Approve)
            {
                case true:
                    break;
            }
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            _handOvertoOwner.VehicleId = int.Parse(ViewState["vehicleID"].ToString());
            _handOvertoOwner.HandOverTo = txtHandOverTo.Text;
            _handOvertoOwner.HandOverDate = DateTime.Parse(txtHandOverDate.Text);
            _handOvertoOwner.HandOverBy = txtHandOverBy.Text;
            _handOvertoOwner.OdoReading = txtOdoreading.Text;
            _handOvertoOwner.UpdatedBy = "HO-FE/FM";
            _handOvertoOwner.InsertHandOverToOwner();
            btnClose_Click(btnClose, e);
        }
        catch (Exception ex)
        {
            ErrorHandler.ErrorsEntry(ex.GetBaseException().ToString(), "class: HandOverToOwner;Method: btnAttachFiles_Click()-InsertFillAttachmentToVehicle", 0);
        }

        FillTemporaryVehicle();
        FillHandOverVehicle();
    }

    protected void btnClose_Click(object sender, EventArgs e)
    {
    }

    protected void FillTemporaryVehicle()
    {
        try
        {
            if (Session["UserdistrictId"] != null) _handOvertoOwner.UserdistrictID = int.Parse(Session["UserdistrictId"].ToString());
            var dv = _handOvertoOwner.FillTemporaryHandOverVehicle().Tables[0].DefaultView;
            grdTemporaryVehicle.DataSource = dv;
            grdTemporaryVehicle.DataBind();
        }
        catch (Exception ex)
        {
            ErrorHandler.ErrorsEntry(ex.GetBaseException().ToString(), "class: AttachDocuments;Method: Page_Load()-FillTemporaryHandOverVehicle", 0);
        }
    }

    protected void FillHandOverVehicle()
    {
        try
        {
            var dv = _handOvertoOwner.FillTemporaryVehicle().Tables[0].DefaultView;
            grdVehicleDecompositionApproval.DataSource = dv;
            grdVehicleDecompositionApproval.DataBind();
        }
        catch (Exception ex)
        {
            ErrorHandler.ErrorsEntry(ex.GetBaseException().ToString(), "class: AttachDocuments;Method: Page_Load()-FillVehicleAttachment", 0);
        }
    }

    protected void grdVehicleDecompositionApproval_RowCommand(object sender, GridViewCommandEventArgs e)
    {
    }

    protected void grdVehicleDecompositionApproval_RowDataBound(object sender, GridViewRowEventArgs e)
    {
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        txtHandOverBy.Text = "";
        txtHandOverDate.Text = "";
        txtHandOverTo.Text = "";
        txtOdoreading.Text = "";
        txtVehicleNumber.Text = "";
    }

    protected void grdVehicleDecompositionApproval_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdVehicleDecompositionApproval.PageIndex = e.NewPageIndex;
        FillHandOverVehicle();
    }

    protected void grdTemporaryVehicle_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        Debug.Assert(e.CommandName != null, "e.CommandName != null");
        switch (e.CommandName)
        {
            case "vehicleAccidentedit":
                txtHandOverBy.Text = "";
                txtHandOverDate.Text = "";
                txtHandOverTo.Text = "";
                txtOdoreading.Text = "";
                txtVehicleNumber.Text = "";
                _handOvertoOwner.VehicleId = Convert.ToInt32(e.CommandArgument);
                var ds = _handOvertoOwner.GetVehicleByVehicleID();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    ViewState["vehicleID"] = dr["VehicleID"].ToString();
                    txtVehicleNumber.Text = dr["VehicleNumber"].ToString();
                }

                var gvr = (GridViewRow) ((LinkButton) e.CommandSource).NamingContainer;
                var modal = (ModalPopupExtender) gvr.FindControl("ModalPopupExtender1");
                modal.Show();
                break;
        }
    }
}