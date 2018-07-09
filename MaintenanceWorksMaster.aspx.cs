using System;
using System.Data;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using GvkFMSAPP.PL;
public partial class MaintenanceWorksMaster : Page
{
    private readonly FleetMaster _fleetMaster = new FleetMaster();
    private readonly Helper _helper = new Helper();

    #region Page Load

    protected void Page_Load(object sender,EventArgs e)
    {
        if (Session["User_Name"] == null) Response.Redirect("Login.aspx");
        if (!IsPostBack)
        {
            FillServiceGroupNames();
            FillGrid_MaintenanceWorksMaster();
            btnSaveMaintenanceWorksMaster.Attributes.Add("onclick","javascript:return validationMaintenanceWorksMaster()");
            txtServiceName.Attributes.Add("onkeypress","javascript:return OnlyAlphabets(this,event)");
            txtCostGrade.Attributes.Add("onkeypress", "javaScript:return numeric_only(event)");
            ddlMaintenanceManufacturerName.Enabled = false;
            ddlServiceGroupName.SelectedIndex = 0;
            //Permissions
            var dsPerms = (DataSet) Session["PermissionsDS"];
            if (dsPerms == null) throw new ArgumentNullException(nameof(dsPerms));
            dsPerms.Tables[0].DefaultView.RowFilter = "Url='" + Page.Request.Url.Segments[Page.Request.Url.Segments.Length - 1] + "'";
            var p = new PagePermissions(dsPerms,dsPerms.Tables[0].DefaultView[0]["Url"].ToString(),dsPerms.Tables[0].DefaultView[0]["Title"].ToString());
            pnlMaintenanceWorksMaster.Visible = false;
            grvMaintenanceWorksMasterDetails.Visible = false;
            if (p.View)
            {
                grvMaintenanceWorksMasterDetails.Visible = true;
                grvMaintenanceWorksMasterDetails.Columns[4].Visible = false;
            }

            if (p.Add)
            {
                pnlMaintenanceWorksMaster.Visible = true;
                grvMaintenanceWorksMasterDetails.Visible = true;
                grvMaintenanceWorksMasterDetails.Columns[4].Visible = false;
            }

            if (p.Modify)
            {
                grvMaintenanceWorksMasterDetails.Visible = true;
                grvMaintenanceWorksMasterDetails.Columns[4].Visible = true;
            }
        }
    }

    #endregion

    #region Reset Function

    private void MaintenanceWorksMasterReset()
    {
        txtCategories.Visible = true;
        ddlSSName.Visible = false;
        linkCat.Visible = true;
        linkNew.Visible = false;
        ddlServiceGroupName.SelectedIndex = 0;
        ddlMaintenanceManufacturerName.SelectedIndex = 0;
        ddlMaintenanceManufacturerName.Enabled = false;
        txtServiceName.Text = "";
        txtCategories.Text = "";
        txtCostGrade.Text = "";
        txtCostOtherGrade.Text = "";
        btnSaveMaintenanceWorksMaster.Text = "Save";
        txtTimeTaken.Text = "";
    }

    #endregion

    #region Filling Service Group Names

    private void FillServiceGroupNames()
    {
        try
        {
            var ds = _fleetMaster.FillServiceGroupNames();
            _helper.FillDropDownHelperMethodWithDataSet(ds,"ServiceGroup_Name","ServiceGroup_Id",ddlServiceGroupName);
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    #endregion

    #region Fillimg Gridview of Maintenance Works Master

    public void FillGrid_MaintenanceWorksMaster()
    {
        using (var ds = _fleetMaster.FillGrid_MaintenanceWorksMaster())
        {
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                grvMaintenanceWorksMasterDetails.DataSource = ds.Tables[0];
                grvMaintenanceWorksMasterDetails.DataBind();
            }
            else
            {
                var strScript1 = "<script language=JavaScript>alert('" + "No record found" + "')</script>";
                ClientScript.RegisterStartupScript(GetType(),"Success",strScript1);
            }
        }
    }

    #endregion

    #region Save and Update Button

    protected void btnSaveMaintenanceWorksMaster_Click(object sender,EventArgs e)
    {
        try
        {
            switch (btnSaveMaintenanceWorksMaster.Text)
            {
                case "Save":
                {
                    if (!ddlSSName.Visible)
                    {
                        if (txtCategories.Text == "") Show("Please enter category");
                    }
                    else
                    {
                        switch (ddlSSName.SelectedIndex)
                        {
                            case 0:
                                Show("Please select category");
                                break;
                        }
                    }

                    string subserviceName;
                    var serviceGroupId = Convert.ToInt32(ddlServiceGroupName.SelectedValue);
                    var serviceGroupName = ddlServiceGroupName.SelectedItem.Text;
                    var serviceName = txtServiceName.Text;
                    var vehicleManufacturer = Convert.ToInt32(ddlMaintenanceManufacturerName.SelectedValue);
                    int flag;
                    if (ddlSSName.Visible)
                    {
                        subserviceName = ddlSSName.SelectedItem.ToString();
                        flag = 1;
                    }
                    else
                    {
                        subserviceName = txtCategories.Text;
                        flag = 0;
                    }

                    var costAGrade = Convert.ToDecimal(txtCostGrade.Text);
                    var costOtherThanAGrade = Convert.ToDecimal(txtCostOtherGrade.Text);
                    var timeTaken = txtTimeTaken.Text;
                    var ds = _fleetMaster.InsertMaintenanceWorksMasterDetails(serviceGroupId,vehicleManufacturer,serviceGroupName,serviceName,subserviceName,costAGrade,costOtherThanAGrade,timeTaken,flag);
                    if (ds == null) throw new ArgumentNullException(nameof(ds));
                    switch (ds.Tables.Count)
                    {
                        case 0:
                            Show("Details added successfully");
                            MaintenanceWorksMasterReset();
                            break;
                        default:
                            Show("Details already exists");
                            break;
                    }

                    break;
                }
                default:
                {
                    int serviceId = Convert.ToInt16(hidWorksMasterId.Value);
                    var serviceName = txtServiceName.Text;
                    var serviceGroupId = Convert.ToInt32(ddlServiceGroupName.SelectedValue);
                    var subserviceName = txtCategories.Text;
                    var costAGrade = Convert.ToDecimal(txtCostGrade.Text);
                    var costOtherThanAGrade = Convert.ToDecimal(txtCostOtherGrade.Text);
                    var timeTaken = txtTimeTaken.Text;
                    var ds = _fleetMaster.UpdateMaintenanceWorksMasterDetails(serviceId,serviceGroupId,serviceName,costAGrade,costOtherThanAGrade,subserviceName,timeTaken);
                    if (ds == null) throw new ArgumentNullException(nameof(ds));
                    switch (ds.Tables.Count)
                    {
                        case 0:
                            Show("Details updated successfully");
                            linkCat.Visible = true;
                            MaintenanceWorksMasterReset();
                            break;
                        default:
                            Show("Details already exists");
                            break;
                    }

                    break;
                }
            }
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }

        FillGrid_MaintenanceWorksMaster();
    }

    #endregion

    #region Reset Button

    protected void btnResetMaintenanceWorksMaster_Click(object sender,EventArgs e)
    {
        MaintenanceWorksMasterReset();
    }

    #endregion

    #region Selected Index Change For Service Group Names

    protected void ddlServiceGroupName_SelectedIndexChanged(object sender,EventArgs e)
    {
        if (ddlServiceGroupName.SelectedIndex == 0) return;
        if (!ddlSSName.Visible)
        {
            var subservice = ddlServiceGroupName.SelectedItem.Text;
            ddlMaintenanceManufacturerName.Enabled = true;
            var dsSet = _fleetMaster.GetSubService(subservice);
            if (dsSet == null) throw new ArgumentNullException(nameof(dsSet));
            SubService(dsSet);
        }
        else
        {
            try
            {
                _helper.FillDropDownHelperMethodWithSp("P_GetCategories","Categories","Category_Id",ddlServiceGroupName,ddlMaintenanceManufacturerName,null,null,"@Aggre","@VehicleManufacturer",null,null,null,null,ddlSSName);
            }
            catch (Exception ex)
            {
                _helper.ErrorsEntry(ex);
            }
        }
    }

    #endregion

    #region Row Editing of Maintenance Works Master Details

    protected void grvMaintenanceWorksMasterDetails_RowEditing(object sender,GridViewEditEventArgs e)
    {
        btnSaveMaintenanceWorksMaster.Text = "Update";
        var index = e.NewEditIndex;
        var lblserId = (Label) grvMaintenanceWorksMasterDetails.Rows[index].FindControl("lblServiceId");
        hidWorksMasterId.Value = lblserId.Text;
        int serviceId = Convert.ToInt16(hidWorksMasterId.Value);
        var ds = _fleetMaster.RowEditMaintenanceWorksMasterDetails(serviceId);
        Session["Aggregates"] = ds.Tables[0].Rows[0].ItemArray[0].ToString();
        var manufacturerName = string.Empty;
        foreach (DataRow item in ds.Tables[1].Rows)
            if (item["Manufacturer_Id"].ToString() == ds.Tables[0].Rows[0]["Manufacturer_Id"].ToString())
                manufacturerName = item["Manufacturer_Id"].ToString();
        linkCat.Visible = false;
        ddlServiceGroupName.SelectedItem.Text = ds.Tables[0].Rows[0]["Aggregates"].ToString();
        var dset = _fleetMaster.GetSubService(ds.Tables[0].Rows[0]["Aggregates"].ToString());
        SubService(dset);
        ddlMaintenanceManufacturerName.SelectedValue = manufacturerName;
        txtServiceName.Text = Convert.ToString(ds.Tables[0].Rows[0]["Sub Categories"].ToString());
        txtCategories.Text = Convert.ToString(ds.Tables[0].Rows[0]["Categories"].ToString());
        var cGrade = Convert.ToString(ds.Tables[0].Rows[0]["Cost"].ToString()).Split('.');
        txtCostGrade.Text = cGrade[0] + '.' + cGrade[1].Substring(0,2);
        var coAGrade = Convert.ToString(ds.Tables[0].Rows[0]["Cost_Other_Than_A_Grade"].ToString()).Split('.');
        txtCostOtherGrade.Text = coAGrade[0] + '.' + coAGrade[1].Substring(0,2);
        txtTimeTaken.Text = Convert.ToString(ds.Tables[0].Rows[0]["Time_Taken"].ToString());
        FillServiceGroupNames();
        ddlServiceGroupName.ClearSelection();
        ddlMaintenanceManufacturerName.Enabled = true;
        ddlServiceGroupName.Items.FindByText(ds.Tables[0].Rows[0]["Aggregates"].ToString()).Selected = true;
    }

    #endregion

    public void Show(string message)
    {
        ScriptManager.RegisterStartupScript(this,GetType(),"msg","alert('" + message + "');",true);
    }

    protected void grvMaintenanceWorksMasterDetails_PageIndexChanging(object sender,GridViewPageEventArgs e)
    {
        grvMaintenanceWorksMasterDetails.PageIndex = e.NewPageIndex;
        FillGrid_MaintenanceWorksMaster();
    }

    protected void linkCat_Click(object sender,EventArgs e)
    {
        txtCategories.Visible = false;
        ddlSSName.Visible = true;
        linkCat.Visible = false;
        linkNew.Visible = true;
        ddlMaintenanceManufacturerName.Enabled = false;
        ddlServiceGroupName_SelectedIndexChanged(this,null);
    }

    protected void linkNew_Click(object sender,EventArgs e)
    {
        txtCategories.Visible = true;
        ddlSSName.Visible = false;
        linkCat.Visible = true;
        linkNew.Visible = false;
        ddlMaintenanceManufacturerName.Enabled = true;
    }

    protected void ddlMaintenanceManufacturerName_SelectedIndexChanged(object sender,EventArgs e)
    {
        txtCategories.Visible = true;
        ddlSSName.Visible = false;
        linkCat.Visible = true;
        linkNew.Visible = false;
    }

    private void SubService(DataSet dset)
    {
        try
        {
            _helper.FillDropDownHelperMethodWithDataSet(dset,"FleetManufacturer_Name","FleetManufacturer_Id",ddlMaintenanceManufacturerName,null,null,null,"1");
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }
}