using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using GvkFMSAPP.BLL;
public partial class PetroCardMapping : Page
{
    private readonly Helper _helper = new Helper();
    public IFuelManagement Objpetmap = new FuelManagement();

    protected void Page_Load(object sender,EventArgs e)
    {
        if (Session["User_Name"] == null) Response.Redirect("Login.aspx");
        if (!IsPostBack)
        {
            FillVehiclesformapping();
            FillPetroCardformapping();
            FillGridformapping();
            FillddlReasons();
            var dsPerms = (DataSet) Session["PermissionsDS"];
            if (dsPerms == null) throw new ArgumentNullException(nameof(dsPerms));
            dsPerms.Tables[0].DefaultView.RowFilter = "Url='" + Page.Request.Url.Segments[Page.Request.Url.Segments.Length - 1] + "'";
        }
    }

    private void FillddlReasons()
    {
        try
        {
            var ds = Objpetmap.IFillddlReasons();
            if (ds == null) throw new ArgumentNullException(nameof(ds));
            _helper.FillDropDownHelperMethodWithDataSet(ds,"Reason","ReasonsID",ddlReason);
            ddlReason.Enabled = true;
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    private void FillGridformapping()
    {
        var districtId = -1;
        if (Session["UserdistrictId"] != null) districtId = Convert.ToInt32(Session["UserdistrictId"].ToString());
        var ds = Objpetmap.IFillGridformapping(districtId);
        if (ds == null) throw new ArgumentNullException(nameof(ds));
        gvPetroCardMapping.DataSource = ds;
        gvPetroCardMapping.DataBind();
    }

    private void FillPetroCardformapping()
    {
        try
        {
            var userId = -1;
            if (Session["User_Id"] != null) userId = Convert.ToInt32(Session["User_Id"].ToString());
            var ds = Objpetmap.IFillPetroCardformapping(userId);
            if (ds == null) throw new ArgumentNullException(nameof(ds));
            _helper.FillDropDownHelperMethodWithDataSet(ds,"PetroCardNum","PetroCardIssueID",ddlPetroCardNumber);
            ddlPetroCardNumber.Enabled = true;
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    private void FillPetroCardformapping1()
    {
        try
        {
            var ds = Objpetmap.IFillPetroCardformapping1();
            if (ds == null) throw new ArgumentNullException(nameof(ds));
            _helper.FillDropDownHelperMethodWithDataSet(ds,"PetroCardNum","PetroCardIssueID",ddlPetroCardNumber);
            ddlPetroCardNumber.Enabled = true;
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    private void FillVehiclesformapping()
    {
        try
        {
            var districtId = -1;
            if (Session["UserdistrictId"] != null) districtId = Convert.ToInt32(Session["UserdistrictId"].ToString());
            var ds = Objpetmap.IFillVehiclesformapping(districtId);
            if (ds == null) throw new ArgumentNullException(nameof(ds));
            _helper.FillDropDownHelperMethodWithDataSet(ds,"VehicleNumber","VehicleID",null,ddlVehicleNumber);
            ddlVehicleNumber.Enabled = true;
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    private void FillVehiclesformappingEdit()
    {
        try
        {
            var districtId = -1;
            if (Session["UserdistrictId"] != null) districtId = Convert.ToInt32(Session["UserdistrictId"].ToString());
            var ds = Objpetmap.IFillVehiclesformappingEdit(districtId);
            if (ds == null) throw new ArgumentNullException(nameof(ds));
            _helper.FillDropDownHelperMethodWithDataSet(ds,"VehicleNumber","VehicleID",null,ddlVehicleNumber);
            ddlVehicleNumber.Enabled = true;
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    protected void ddlVehicleNumber_SelectedIndexChanged(object sender,EventArgs e)
    {
        FillNewVehicleNumber();
        FillPetroCardformapping();
    }

    private void FillNewVehicleNumber()
    {
        try
        {
            var districtId = -1;
            if (Session["UserdistrictId"] != null) districtId = Convert.ToInt32(Session["UserdistrictId"].ToString());
            var ds = Objpetmap.IFillVehiclesformapping(districtId);
            if (ds == null) throw new ArgumentNullException(nameof(ds));
            _helper.FillDropDownHelperMethodWithDataSet(ds,"VehicleNumber","VehicleID",ddlNewVehicleNumber);
            ddlNewVehicleNumber.Items.Remove(ddlVehicleNumber.SelectedItem);
            ddlNewVehicleNumber.Enabled = true;
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    protected void ddlPetroCardNumber_SelectedIndexChanged(object sender,EventArgs e)
    {
        FillCardTypeAgencyValidity(Convert.ToInt32(ddlPetroCardNumber.SelectedValue));
    }

    private void FillCardTypeAgencyValidity(int vehicleId)
    {
        try
        {
            var ds = Objpetmap.IFillCardTypeAgencyValidity(vehicleId);
            if (ds == null) throw new ArgumentNullException(nameof(ds));
            txtCardValidity.Text = ds.Tables[0].Rows[0][0].ToString();
            txtAgency.Text = ds.Tables[0].Rows[0][1].ToString();
            txtCardType.Text = ds.Tables[0].Rows[0][2].ToString();
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    protected void Deactivate_CheckedChanged(object sender,EventArgs e)
    {
        if (Deactivate.Checked)
        {
            ddlReason.Visible = true;
            lbReason.Visible = true;
            ddlNewVehicleNumber.Visible = false;
            lbPetroCard.Visible = false;
            txtMappedCardNum.Visible = false;
            lbTransfer.Visible = false;
            ddlVehicleNumber.Enabled = false;
            Save.Text = "Update";
            ddlNewVehicleNumber.Enabled = false;
            ddlPetroCardNumber.Enabled = false;
        }
    }

    protected void TransfertoNewVehicle_CheckedChanged(object sender,EventArgs e)
    {
        if (TransfertoNewVehicle.Checked)
        {
            ddlNewVehicleNumber.Visible = true;
            ddlNewVehicleNumber.Enabled = true;
            lbTransfer.Visible = true;
            ddlVehicleNumber.Enabled = false;
            ddlReason.Visible = false;
            lbReason.Visible = false;
            txtRemarks.Visible = false;
            Save.Text = "Update";
            ddlPetroCardNumber.Enabled = false;
        }
    }

    protected void Save_Click(object sender,EventArgs e)
    {
        if (Save.Text == "Save")
        {
            InsMapping(Convert.ToInt32(ddlVehicleNumber.SelectedValue),Convert.ToInt32(ddlPetroCardNumber.SelectedValue),Convert.ToDateTime(txtIssDate.Text),12,DateTime.Now,15,DateTime.Now);
            FillGridformapping();
        }
        else
        {
            if (Save.Text == "Update" && ddlVehicleNumber.Enabled == false && Deactivate.Checked == false && TransfertoNewVehicle.Checked == false)
            {
                UpdMapping(Convert.ToInt32(txtEdit.Text),Convert.ToInt32(ddlVehicleNumber.SelectedValue),Convert.ToInt32(ddlPetroCardNumber.SelectedValue),Convert.ToDateTime(txtIssDate.Text));
                FillGridformapping();
            }
            else if (Save.Text == "Update" && ddlVehicleNumber.Enabled == false && ddlNewVehicleNumber.Enabled && TransfertoNewVehicle.Checked && Deactivate.Checked == false)
            {
                if (ddlNewVehicleNumber.SelectedIndex != 0)
                {
                    UpdMapping1(Convert.ToInt32(txtEdit.Text),Convert.ToInt32(ddlNewVehicleNumber.SelectedValue),Convert.ToInt32(ddlPetroCardNumber.SelectedValue),Convert.ToDateTime(txtIssDate.Text));
                }
                else
                {
                    var strFmsScript = "Please Select a new Vehicle to transfer";
                    Show(strFmsScript);
                }

                FillGridformapping();
            }
            else
            {
                if (Save.Text == "Update" && ddlNewVehicleNumber.Enabled == false && TransfertoNewVehicle.Checked == false && Deactivate.Checked)
                {
                    var petroCardId = Convert.ToInt32(ddlPetroCardNumber.SelectedValue);
                    var ds = Objpetmap.ICheckPetroCard(petroCardId);
                    if (ds.Tables[0].Rows.Count == 0 && ddlReason.SelectedIndex != 0)
                    {
                        UpdMapping2(Convert.ToInt32(txtEdit.Text),Convert.ToInt32(ddlReason.SelectedValue),Convert.ToString(txtRemarks.Text));
                    }
                    else if (ds.Tables[0].Rows.Count == 0 && ddlReason.SelectedIndex == 0)
                    {
                        var strFmsScript = "Please Select A Reason and Deactivate";
                        Show(strFmsScript);
                    }
                    else
                    {
                        var strFmsScript = "Please Approve or Reject the Fuel Entries for this card and Deactivate";
                        Show(strFmsScript);
                    }

                    FillGridformapping();
                }
            }
        }
    }

    private void UpdMapping(int petroCardVehicleMapId,int vehicleId,int petroCardIssueId,DateTime issuedToAmbyDate)
    {
        var res = Objpetmap.IUpdMapping(petroCardVehicleMapId,vehicleId,petroCardIssueId,issuedToAmbyDate);
        switch (res)
        {
            case 1:
            {
                var strFmsScript = "Mapping Details Updated";
                Show(strFmsScript);
                break;
            }
            default:
            {
                var strFmsScript = "Failure";
                Show(strFmsScript);
                break;
            }
        }

        ClearFields();
    }

    private void UpdMapping1(int petroCardVehicleMapId,int vehicleId,int petroCardIssueId,DateTime issuedToAmbyDate)
    {
        var res = Objpetmap.IUpdMapping(petroCardVehicleMapId,vehicleId,petroCardIssueId,issuedToAmbyDate);
        switch (res)
        {
            case 1:
            {
                var strFmsScript = "Mapping Details Updated";
                Show(strFmsScript);
                break;
            }
            default:
            {
                var strFmsScript = "Failure";
                Show(strFmsScript);
                break;
            }
        }

        ClearFields();
    }

    private void UpdMapping2(int petroCardVehicleMapId,int reasonsId,string remarks)
    {
        var res = Objpetmap.IUpdMapping2(petroCardVehicleMapId,reasonsId,remarks);
        switch (res)
        {
            case 2:
            {
                var strFmsScript = "Mapping Details Updated";
                Show(strFmsScript);
                break;
            }
            default:
            {
                var strFmsScript = "Failure";
                Show(strFmsScript);
                break;
            }
        }

        ClearFields();
    }

    private void InsMapping(int vehicleId,int petroCardIssueId,DateTime issuedToFe,int createdBy,DateTime createdDate,int updatedBy,DateTime updatedDate)
    {
        var res = Objpetmap.IInsMapping(vehicleId,petroCardIssueId,issuedToFe,createdBy,createdDate,updatedBy,updatedDate);
        switch (res)
        {
            case 2:
            {
                var strFmsScript = "Mapping Details Inserted";
                Show(strFmsScript);
                break;
            }
            default:
            {
                var strFmsScript = "Failure";
                Show(strFmsScript);
                break;
            }
        }

        ClearFields();
    }

    protected void gvPetroCardMapping_RowEditing(object sender,GridViewEditEventArgs e)
    {
    }

    protected void gvPetroCardMapping_PageIndexChanging(object sender,GridViewPageEventArgs e)
    {
        try
        {
            gvPetroCardMapping.PageIndex = e.NewPageIndex;
            var districtId = -1;
            if (Session["UserdistrictId"] != null) districtId = Convert.ToInt32(Session["UserdistrictId"].ToString());
            var ds = Objpetmap.IFillGridformapping(districtId);
            if (ds == null) throw new ArgumentNullException(nameof(ds));
            gvPetroCardMapping.DataSource = ds;
            gvPetroCardMapping.DataBind();
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    protected void ddlReason_SelectedIndexChanged(object sender,EventArgs e)
    {
        txtRemarks.Visible = ddlReason.SelectedIndex == 1;
    }

    protected void ddlNewVehicleNumber_SelectedIndexChanged(object sender,EventArgs e)
    {
        txtMappedCardNum.Visible = true;
        FillMappedCardNum(Convert.ToInt32(ddlNewVehicleNumber.SelectedValue));
    }

    private void FillMappedCardNum(int vehicleId)
    {
        var ds = Objpetmap.IFillMappedCardNum(vehicleId);
        switch (ds.Tables[0].Rows.Count)
        {
            case 0:
                txtMappedCardNum.Text = "No Cards Mapped";
                txtMappedCardNum.Enabled = false;
                break;
            default:
                txtMappedCardNum.Text = ds.Tables[0].Rows[0][0].ToString();
                break;
        }
    }

    protected void Reset_Click(object sender,EventArgs e)
    {
        ClearFields();
    }

    private void ClearFields()
    {
        ddlVehicleNumber.SelectedIndex = 0;
        ddlVehicleNumber.Enabled = true;
        ddlPetroCardNumber.Enabled = true;
        txtAgency.Text = "";
        txtCardType.Text = "";
        Deactivate.Visible = false;
        TransfertoNewVehicle.Visible = false;
        ddlReason.SelectedIndex = 0;
        txtCardValidity.Text = "";
        txtEdit.Text = "";
        txtIssDate.Text = "";
        txtMappedCardNum.Text = "";
        txtRemarks.Text = "";
        Deactivate.Checked = false;
        TransfertoNewVehicle.Checked = false;
        txtRemarks.Visible = false;
        txtMappedCardNum.Visible = false;
        ddlNewVehicleNumber.Visible = false;
        ddlReason.Visible = false;
        lbReason.Visible = false;
        lbTransfer.Visible = false;
        lbPetroCard.Visible = false;
        Save.Text = "Save";
        ddlPetroCardNumber.Items.Clear();
    }

    public void Show(string message)
    {
        ScriptManager.RegisterStartupScript(this,GetType(),"msg","alert('" + message + "');",true);
    }

    protected void gvPetroCardMapping_RowCommand(object sender,GridViewCommandEventArgs e)
    {
        FillVehiclesformappingEdit();
        FillPetroCardformapping1();
        FillNewVehicleNumber();
        Deactivate.Visible = true;
        TransfertoNewVehicle.Visible = true;
        Save.Text = "Update";
        var id = Convert.ToInt32(e.CommandArgument.ToString());
        var ds = Objpetmap.IEditMappingDetails(id);
        txtEdit.Text = Convert.ToString(id);
        ddlVehicleNumber.SelectedValue = ds.Tables[0].Rows[0]["VehicleID"].ToString();
        FillNewVehicleNumber();
        ddlVehicleNumber.Enabled = false;
        var petro = ds.Tables[0].Rows[0]["PetroCardIssueID"].ToString();
        ddlPetroCardNumber.SelectedValue = petro;
        ddlPetroCardNumber.Enabled = false;
        var pid = Convert.ToInt32(ddlPetroCardNumber.SelectedValue);
        FillCardTypeAgencyValidity(pid);
        txtCardValidity.Text = ds.Tables[0].Rows[0]["Validity"].ToString();
        txtAgency.Text = ds.Tables[0].Rows[0]["Agency"].ToString();
        txtCardType.Text = ds.Tables[0].Rows[0]["Card"].ToString();
        txtIssDate.Text = ds.Tables[0].Rows[0]["Date"].ToString();
    }
}