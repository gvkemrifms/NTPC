using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using GvkFMSAPP.PL;

public partial class MaintenanceWorksServiceGroup : Page
{
    private readonly Helper _helper = new Helper();
    private readonly FleetMaster _fleetMaster = new FleetMaster();

    #region Page Load

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Name"] == null) Response.Redirect("Login.aspx");
        if (!IsPostBack)
        {
            btnCancelMaintenanceWorksServiceGroup.Visible = false;
            grvMaintenanceWorksServiceGroupDetails.Columns[0].Visible = false;
            FillManufacturerNames();
            FillGrid_MaintenanceWorksServiceGroup();
            pnlmaintenanceworksServiceGrp.Visible = false;
            grvMaintenanceWorksServiceGroupDetails.Visible = false;
            txtServiceGroupName.Attributes.Add("onkeypress", "javascript:return OnlyAlphabets()");
            btnSaveMaintenanceWorksServiceGroup.Attributes.Add("onclick", "javascript:return validationMaintenanceWorksServiceGroup(this,event)");
            var dsPerms = (DataSet) Session["PermissionsDS"];
            dsPerms.Tables[0].DefaultView.RowFilter = "Url='" + Page.Request.Url.Segments[Page.Request.Url.Segments.Length - 1] + "'";
            var p = new PagePermissions(dsPerms, dsPerms.Tables[0].DefaultView[0]["Url"].ToString(), dsPerms.Tables[0].DefaultView[0]["Title"].ToString());
            if (p.View)
            {
                grvMaintenanceWorksServiceGroupDetails.Visible = true;
                grvMaintenanceWorksServiceGroupDetails.Columns[4].Visible = false;
            }

            if (p.Add)
            {
                pnlmaintenanceworksServiceGrp.Visible = true;
                grvMaintenanceWorksServiceGroupDetails.Visible = true;
                grvMaintenanceWorksServiceGroupDetails.Columns[4].Visible = false;
            }

            if (p.Modify)
            {
                grvMaintenanceWorksServiceGroupDetails.Visible = true;
                grvMaintenanceWorksServiceGroupDetails.Columns[4].Visible = true;
            }
        }
    }

    #endregion

    #region Reset Function

    private void MaintenanceWorksServiceGroupReset()
    {
        txtServiceGroupName.Text = "";
        ddlManufacturerName.SelectedIndex = 0;
        btnSaveMaintenanceWorksServiceGroup.Text = "Save";
    }

    #endregion

    #region Filling Manufacturer Names

    private void FillManufacturerNames()
    {
        var ds = _fleetMaster.FillManufacturerNames();
        if (ds == null) throw new ArgumentNullException(nameof(ds));
        try
        {
            _helper.FillDropDownHelperMethodWithDataSet(ds, "FleetManufacturer_Name", "FleetManufacturer_Id", ddlManufacturerName);
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    #endregion

    #region Fillimg Gridview of Maintenance Works Service Group

    public void FillGrid_MaintenanceWorksServiceGroup()
    {
        using (var ds = _fleetMaster.FillGrid_MaintenanceWorksServiceGroup())
        {
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                grvMaintenanceWorksServiceGroupDetails.DataSource = ds.Tables[0];
                grvMaintenanceWorksServiceGroupDetails.DataBind();
            }
            else
            {
                var strScript1 = "<script language=JavaScript>alert('" + "No record found" + "')</script>";
                ClientScript.RegisterStartupScript(GetType(), "Success", strScript1);
            }
        }
    }

    #endregion

    #region Save and Update button

    protected void btnSaveMaintenanceWorksServiceGroup_Click(object sender, EventArgs e)
    {
        try
        {
            switch (btnSaveMaintenanceWorksServiceGroup.Text)
            {
                case "Save":
                {
                    var ds = _fleetMaster.FillGrid_MaintenanceWorksServiceGroup();
                    if (ds == null) throw new ArgumentNullException(nameof(ds));
                    if (ds.Tables[0].Select("ServiceGroup_Name='" + txtServiceGroupName.Text + "' And FleetManufacturer_Name='" + ddlManufacturerName.SelectedItem + "'").Length <= 0)
                    {
                        var serviceGroupName = txtServiceGroupName.Text;
                        var manufacturerId = Convert.ToInt32(ddlManufacturerName.SelectedValue);
                        var createdDate = DateTime.Today;
                        var createdBy = Convert.ToInt32(Session["User_Id"]);
                        ds = _fleetMaster.InsertMaintenanceWorksServiceGroupDetails(serviceGroupName, manufacturerId, createdDate, createdBy);
                        switch (ds.Tables.Count)
                        {
                            case 0:
                                Show("Details saved successfully");
                                MaintenanceWorksServiceGroupReset();
                                break;
                            default:
                                Show("Details already exists");
                                break;
                        }
                    }
                    else
                    {
                        Show("Service Group Name for selected Manufacturer already exists");
                    }

                    break;
                }
                default:
                {
                    var ds = _fleetMaster.FillGrid_MaintenanceWorksServiceGroup();
                    if (ds == null) throw new ArgumentNullException(nameof(ds));
                    if (ds.Tables[0].Select("ServiceGroup_Name='" + txtServiceGroupName.Text + "' And ServiceGroup_Id<>'" + hidSerGrpId.Value + "'").Length <= 0)
                    {
                        int serviceGroupId = Convert.ToInt16(hidSerGrpId.Value);
                        var serviceGroupName = txtServiceGroupName.Text;
                        var manufacturerId = Convert.ToInt32(ddlManufacturerName.SelectedValue);
                        ds = _fleetMaster.UpdateMaintenanceWorksServiceGroupDetails(serviceGroupId, serviceGroupName, manufacturerId);
                        switch (ds.Tables.Count)
                        {
                            case 0:
                                Show("Details updated successfully");
                                MaintenanceWorksServiceGroupReset();
                                break;
                            default:
                                Show("Details already exists");
                                break;
                        }
                    }
                    else
                    {
                        Show("Service Group Name already exists");
                    }

                    break;
                }
            }
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }

        FillGrid_MaintenanceWorksServiceGroup();
    }

    #endregion

    #region Reset Button

    protected void btnResetMaintenanceWorksServiceGroup_Click(object sender, EventArgs e)
    {
        MaintenanceWorksServiceGroupReset();
    }

    #endregion

    #region Page Index Changing for Service Group Details

    protected void grvMaintenanceWorksServiceGroupDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grvMaintenanceWorksServiceGroupDetails.PageIndex = e.NewPageIndex;
        FillGrid_MaintenanceWorksServiceGroup();
    }

    #endregion

    #region Row Editing of Maintenance Works Service Group Details

    protected void grvMaintenanceWorksServiceGroupDetails_RowEditing(object sender, GridViewEditEventArgs e)
    {
        btnSaveMaintenanceWorksServiceGroup.Text = "Update";
        var index = e.NewEditIndex;
        var lblServiceId = (Label) grvMaintenanceWorksServiceGroupDetails.Rows[index].FindControl("lblServiceId");
        hidSerGrpId.Value = lblServiceId.Text;
        int sgId = Convert.ToInt16(hidSerGrpId.Value);
        var ds = _fleetMaster.RowEditMaintenanceWorksServiceGroupDetails(sgId);
        if (ds == null) throw new ArgumentNullException(nameof(ds));
        Session["Manufacturer_Id"] = ds.Tables[0].Rows[0].ItemArray[1].ToString();
        txtServiceGroupName.Text = Convert.ToString(ds.Tables[0].Rows[0]["ServiceGroup_Name"].ToString());
        FillManufacturerNames();
        ddlManufacturerName.SelectedValue = ds.Tables[0].Rows[0]["Manufacturer_Id"].ToString();
    }

    #endregion

    public void Show(string message)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('" + message + "');", true);
    }
}