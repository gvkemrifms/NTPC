using System;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using GvkFMSAPP.PL;

public partial class AgencyDetails : Page
{
    private readonly Helper _helper = new Helper();
    private readonly FleetMaster _fleetMaster = new FleetMaster();

    public string UserId { get;  set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] == null)
            Response.Redirect("Login.aspx");
        else
            UserId = (string)Session["User_Id"];
        if (!IsPostBack)
        {
            grvAgencyDetails.Columns[0].Visible = false;
            grvAgencyDetails.Columns[3].Visible = false;
            FillDistricts();
            FillGridAgencyDetails();
            txtAddress.Attributes.Add("onkeypress", "javascript:return  remark(this,event)");
            txtAgencyName.Attributes.Add("onkeypress", "javascript:return  OnlyAlphabets(this,event)");
            txtTin.Attributes.Add("onkeypress", "javascript:return  numeric_only(this,event)");
            //Permissions
            var dsPerms = (DataSet) Session["PermissionsDS"];
            dsPerms.Tables[0].DefaultView.RowFilter = "Url='" + Page.Request.Url.Segments[Page.Request.Url.Segments.Length - 1] + "'";
            var p = new PagePermissions(dsPerms, dsPerms.Tables[0].DefaultView[0]["Url"].ToString(), dsPerms.Tables[0].DefaultView[0]["Title"].ToString());
            if (p.Modify != true) return;
            grvAgencyDetails.Visible = true;
            grvAgencyDetails.Columns[5].Visible = true;
            grvAgencyDetails.Columns[6].Visible = false;
        }
    }

    private void FillGridAgencyDetails()
    {
        var ds = _fleetMaster.FillGridAgencyDetails();
        if (ds == null) throw new ArgumentNullException(nameof(ds));
        grvAgencyDetails.DataSource = ds;
        grvAgencyDetails.DataBind();
    }

    private void FillDistricts()
    {
        string query = ConfigurationManager.AppSettings["Query"]+" "+ "where u.UserId ='" + UserId + "'";
        try
        {
            _helper.FillDropDownHelperMethod(query, "district_name", "district_id", ddlDistrict);
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    protected void grvAgencyDetails_RowEditing(object sender, GridViewEditEventArgs e)
    {
        btnSaveAgencyDetails.Text = "Update";
        var index = e.NewEditIndex;
        var lblId = (Label) grvAgencyDetails.Rows[index].FindControl("lblId");
        hidAgencyId.Value = lblId.Text;
        int agencyId = Convert.ToInt16(hidAgencyId.Value);
        var ds = _fleetMaster.EditAgencyDetails(agencyId);
        txtEdit.Text = hidAgencyId.Value;
        txtAgencyName.Text = ds.Tables[0].Rows[0]["AgencyName"].ToString();
        FillDistricts();
        ddlDistrict.SelectedValue = ds.Tables[0].Rows[0]["DistrictID"].ToString();
        txtAddress.Text = ds.Tables[0].Rows[0]["Address"].ToString();
        txtContactNo.Text = ds.Tables[0].Rows[0]["ContactNum"].ToString();
        txtPanNo.Text = ds.Tables[0].Rows[0]["PANNum"].ToString();
        txtTin.Text = ds.Tables[0].Rows[0]["TIN"].ToString();
    }

    protected void grvAgencyDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        {
            Clearfields();
            var index = e.RowIndex;
            var lblId = (Label) grvAgencyDetails.Rows[index].FindControl("lblId");
            hidAgencyId.Value = lblId.Text;
            int agencyId = Convert.ToInt16(hidAgencyId.Value);
            var result = _fleetMaster.DeleteAgencyDetails(agencyId);
            Show(result == 1 ? "Agency Details Deleted Successfully" : "Agency Details Deletion Failure");
            FillGridAgencyDetails();
        }
    }

    protected void grvAgencyDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grvAgencyDetails.PageIndex = e.NewPageIndex;
        var ds = _fleetMaster.FillGridAgencyDetails();
        if (ds == null) throw new ArgumentNullException(nameof(ds));
        grvAgencyDetails.DataSource = ds;
        grvAgencyDetails.DataBind();
    }

    protected void btnSaveAgencyDetails_Click(object sender, EventArgs e)
    {
        try
        {
            switch (btnSaveAgencyDetails.Text)
            {
                case "Save":
                {
                    var ds = _fleetMaster.FillGridAgencyDetails();
                    if (ds == null) throw new ArgumentNullException(nameof(ds));
                    if (ds.Tables[0].Select("AgencyName='" + txtAgencyName.Text + "'").Length <= 0)
                        InsertAgencyDetails(Convert.ToString(txtAgencyName.Text), Convert.ToInt32(ddlDistrict.SelectedValue), Convert.ToInt32(0), Convert.ToInt32(0), Convert.ToInt64(txtContactNo.Text), Convert.ToString(txtPanNo.Text), Convert.ToInt64(txtTin.Text), Convert.ToString(txtAddress.Text));
                    else
                        Show("Agency Name already exists");
                    break;
                }
                default:
                {
                    var ds = _fleetMaster.FillGridAgencyDetails();
                    if (ds == null) throw new ArgumentNullException(nameof(ds));
                    if (ds.Tables[0].Select("AgencyName='" + txtAgencyName.Text + "' And AgencyID<>'" + txtEdit.Text + "'").Length <= 0)
                        UpdateAgencyDetails(Convert.ToInt32(txtEdit.Text), Convert.ToString(txtAgencyName.Text),  Convert.ToInt32(ddlDistrict.SelectedValue), Convert.ToInt32(0), Convert.ToInt32(0), Convert.ToInt64(txtContactNo.Text), Convert.ToString(txtPanNo.Text), Convert.ToInt64(txtTin.Text), Convert.ToString(txtAddress.Text));
                    else
                        Show("Agency Name already exists");
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }

        FillGridAgencyDetails();
    }


    private void UpdateAgencyDetails(int agencyId, string agencyName, int districtId, int mandalId, int cityId, long contactNum, string panNum, long tin, string address)
    {
        var res = _fleetMaster.UpdateAgencyDetails(agencyId, agencyName, districtId, mandalId, cityId, contactNum, panNum, tin, address);
        switch (res)
        {
            case 1:
                Show("Agency Details Updated Successfully");
                Clearfields();
                break;
            default:
                Show("Agency Details already exists");
                break;
        }
    }

    private void InsertAgencyDetails(string agencyName, int districtId, int mandalId, int cityId, long contactNum, string panNum, long tin, string address)
    {
        var res = _fleetMaster.InsertAgencyDetails(agencyName, districtId, mandalId, cityId, contactNum, panNum, tin, address);
        switch (res)
        {
            case 1:
                Show("Agency Details Added Successfully");
                Clearfields();
                break;
            default:
                Show("Agency Details already exists");
                break;
        }
    }

    protected void btnResetAgencyDetails_Click(object sender, EventArgs e)
    {
        Clearfields();
    }

    private void Clearfields()
    {
        btnSaveAgencyDetails.Text = "Save";
        txtAddress.Text = "";
        txtAgencyName.Text = "";
        txtContactNo.Text = "";
        txtEdit.Text = "";
        txtPanNo.Text = "";
        txtTin.Text = "";
        ddlDistrict.SelectedIndex = 0;
    }

    public void Show(string message)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('" + message + "');", true);
    }
}