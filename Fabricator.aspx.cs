using System;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using GvkFMSAPP.BLL;
using GvkFMSAPP.PL;

public partial class Fabricator : Page
{
    private readonly Helper _helper = new Helper();
    private readonly FleetMaster _fleetMaster = new FleetMaster();

    public string UserId { get;  set; }

    #region Page Load

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] == null)
            Response.Redirect("Login.aspx");
        else
            UserId = (string)Session["User_Id"];
        if (!IsPostBack)
        {
            grvFabricatorDetails.Columns[0].Visible = false;
            FillDistricts();
            FillGrid_FabricatorDetails();
        }

        txtFabricatorName.Attributes.Add("onkeypress", "javascript:return OnlyAlphabets(this,event)");
        txtFabricatorContactPerson.Attributes.Add("onkeypress", "javascript:return OnlyAlphabets(this,event)");
        txtFabricatorAddress.Attributes.Add("onkeypress", "javascript:return  remark(this,event)");
        txtFabricatorContactNumber.Attributes.Add("onkeypress", "javascript:return isNumberKey(this,event)");
        //Permissions
        var dsPerms = (DataSet) Session["PermissionsDS"];
        if (dsPerms == null) throw new ArgumentNullException(nameof(dsPerms));
        dsPerms.Tables[0].DefaultView.RowFilter = "Url='" + Page.Request.Url.Segments[Page.Request.Url.Segments.Length - 1] + "'";
        var p = new PagePermissions(dsPerms, dsPerms.Tables[0].DefaultView[0]["Url"].ToString(), dsPerms.Tables[0].DefaultView[0]["Title"].ToString());
        pnlfabricator.Visible = false;
        grvFabricatorDetails.Visible = false;
        if (p.View)
        {
            grvFabricatorDetails.Visible = true;
            grvFabricatorDetails.Columns[7].Visible = false;
        }

        if (p.Add)
        {
            pnlfabricator.Visible = true;
            grvFabricatorDetails.Visible = true;
            grvFabricatorDetails.Columns[7].Visible = false;
        }

        if (p.Modify)
        {
            grvFabricatorDetails.Visible = true;
            grvFabricatorDetails.Columns[7].Visible = true;
        }
    }

    #endregion

    #region Reset Function

    private void FabricatorDetailsReset()
    {
        txtFabricatorAddress.Text = "";
        txtFabricatorContactNumber.Text = "";
        txtFabricatorContactPerson.Text = "";
        ddlFabricatorDistrict.SelectedIndex = 0;
        txtFabricatorEmailId.Text = "";
        txtFabricatorErn.Text = "";
        txtFabricatorName.Text = "";
        txtFabricatorPanNo.Text = "";
        txtFabricatorTin.Text = "";
        ddlFabricatorType.SelectedIndex = 0;
        btnFabricatorSave.Text = "Save";
    }

    #endregion

    #region Fill Districts Function

    private void FillDistricts()
    {
        string query = ConfigurationManager.AppSettings["Query"]+" "+ "where u.UserId ='" + UserId + "'";
            try
            {
                _helper.FillDropDownHelperMethod(query, "DISTRICT_NAME", "DISTRICT_ID", ddlFabricatorDistrict);
            }
            catch (Exception ex)
            {
                _helper.ErrorsEntry(ex);
            }
    }

    #endregion 

    #region Save and Update Button

    protected void btnFabricatorSave_Click(object sender, EventArgs e)
    {
        if (btnFabricatorSave != null)
            try
            {
                switch (btnFabricatorSave.Text)
                {
                    case "Save":
                    {
                            var ds = _fleetMaster.FillGrid_FabricatorDetails();
                            if (ds.Tables[0].Select("FleetFabricator_Name='" + txtFabricatorName.Text + "'").Length <= 0)
                            {
                                var fname = txtFabricatorName.Text;
                                var ftype = Convert.ToInt32(ddlFabricatorType.SelectedValue);
                                var fdist = Convert.ToInt32(ddlFabricatorDistrict.SelectedValue);
                                var fmandal = 0;
                                var fcity = 0;
                                var faddress = txtFabricatorAddress.Text;
                                var fcontno = Convert.ToInt64(txtFabricatorContactNumber.Text);
                                var fcontper = txtFabricatorContactPerson.Text;
                                var fpan = txtFabricatorPanNo.Text;
                                var femail = txtFabricatorEmailId.Text;
                                var ftin = Convert.ToInt64(txtFabricatorTin.Text);
                                var fern = Convert.ToInt64(txtFabricatorErn.Text);
                                var fstatus = 1;
                                var finactby = Convert.ToString(Session["User_Id"]);
                                var finactdate = DateTime.Today;
                                var fcreatedate = DateTime.Today;
                                var fcreateby = Convert.ToString(Session["User_Id"]);
                                var fupdtdate = DateTime.Today;
                                var fupdateby = Convert.ToString(Session["User_Id"]);
                                //InsertFabricatorDetails()
                                ds = _fleetMaster.InsertFabricatorDetails(fname, ftype, fdist, fmandal, fcity, faddress, fcontno, fcontper, fpan, femail, ftin, fern, fstatus, finactby, finactdate, fcreatedate, fcreateby, fupdtdate, fupdateby);
                                switch (ds.Tables.Count)
                                {
                                    case 0:
                                        Show("Fabricator Details Added successfully");
                                        FabricatorDetailsReset();
                                        break;
                                    default:
                                        Show("This Fabricator Details already exists ");
                                        break;
                                }
                            }
                            else
                            {
                                Show("Fabricator Name already exists");
                            }

                        break;
                    }
                    default:
                    {
                        var ds = _fleetMaster.FillGrid_FabricatorDetails();
                        if (ds.Tables[0].Select("FleetFabricator_Name='" + txtFabricatorName.Text + "' And FleetFabricator_Id<>'" + hidFabId.Value + "'").Length <= 0)
                        {
                            int fId = Convert.ToInt16(hidFabId.Value);
                            var fname = txtFabricatorName.Text;
                            var ftype = Convert.ToInt32(ddlFabricatorType.SelectedValue);
                            var fdist = Convert.ToInt32(ddlFabricatorDistrict.SelectedValue);
                            var fmandal = 0;
                            var fcity = 0;
                            var faddress = txtFabricatorAddress.Text;
                            var fcontno = Convert.ToInt64(txtFabricatorContactNumber.Text);
                            var fcontper = txtFabricatorContactPerson.Text;
                            var fpan = txtFabricatorPanNo.Text;
                            var fmail = txtFabricatorEmailId.Text;
                            var ftin = Convert.ToInt64(txtFabricatorTin.Text);
                            var fern = Convert.ToInt64(txtFabricatorErn.Text);
                            //UpdateFAbricatorDetails
                            ds = _fleetMaster.UpdateFabricatorDetails(fId, fname, ftype, fdist, fmandal, fcity, faddress, fcontno, fcontper, fpan, fmail, ftin, fern);
                            switch (ds.Tables.Count)
                            {
                                case 0:
                                    Show("Fabricator Details Updated successfully");
                                    FabricatorDetailsReset();
                                    break;
                                default:
                                    Show("This Fabricator details already exists ");
                                    break;
                            }
                        }
                        else
                        {
                            Show("Fabricator Name already exists");
                        }

                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                _helper.ErrorsEntry(ex);
            }

        FillGrid_FabricatorDetails();
    }

    #endregion

    #region Reset Button

    protected void btnFabricatorReset_Click(object sender, EventArgs e)
    {
        FabricatorDetailsReset();
    }

    #endregion

    #region Filling Gridview of Fabricator Details

    public void FillGrid_FabricatorDetails()
    {
        var ds = _fleetMaster.FillGrid_FabricatorDetails();
        if (ds != null)
        {
            if (ds.Tables[0].Rows.Count <= 0) return;
            grvFabricatorDetails.DataSource = ds.Tables[0];
            grvFabricatorDetails.DataBind();
        }
        else
        {
            var strScript1 = "<script language=JavaScript>alert('" + "No record found" + "')</script>";
            ClientScript.RegisterStartupScript(GetType(), "Success", strScript1);
        }
    }

    #endregion

    #region Row editing gridview of Fabricator detials

    protected void grvFabricatorDetails_RowEditing(object sender, GridViewEditEventArgs e)
    {
        btnFabricatorSave.Text = "Update";
        var index = e.NewEditIndex;
        var lblid = (Label) grvFabricatorDetails.Rows[index].FindControl("lblId");
        hidFabId.Value = lblid.Text;
        int fId = Convert.ToInt16(hidFabId.Value);
        var ds = _fleetMaster.RowEditFabricatorDetails(fId);
        Session["District_Id"] = ds.Tables[0].Rows[0].ItemArray[3].ToString();
        Session["Mandal_Id"] = ds.Tables[0].Rows[0].ItemArray[4].ToString();
        Session["City_Id"] = ds.Tables[0].Rows[0].ItemArray[5].ToString();
        txtFabricatorName.Text = Convert.ToString(ds.Tables[0].Rows[0]["FleetFabricator_Name"].ToString());
        ddlFabricatorType.SelectedValue = ds.Tables[0].Rows[0]["FabricationType_Id"].ToString();
        txtFabricatorAddress.Text = Convert.ToString(ds.Tables[0].Rows[0]["FleetFabricator_Address"].ToString());
        txtFabricatorContactNumber.Text = Convert.ToString(ds.Tables[0].Rows[0]["FleetFabricator_ContactNo"].ToString());
        txtFabricatorContactPerson.Text = Convert.ToString(ds.Tables[0].Rows[0]["FleetFabricator_ContactPerson"].ToString());
        txtFabricatorPanNo.Text = Convert.ToString(ds.Tables[0].Rows[0]["FleetFabricator_PAN"].ToString());
        txtFabricatorEmailId.Text = Convert.ToString(ds.Tables[0].Rows[0]["FleetFabricator_EmailId"].ToString());
        txtFabricatorTin.Text = Convert.ToString(ds.Tables[0].Rows[0]["FleetFabricator_TIN"].ToString());
        txtFabricatorErn.Text = Convert.ToString(ds.Tables[0].Rows[0]["FleetFabricator_ERN"].ToString());
        //FillStates();
        FillDistricts();
        ddlFabricatorDistrict.SelectedValue = ds.Tables[0].Rows[0]["District_Id"].ToString();
    }

    #endregion

    #region Page Index Changing of Fabricator Details

    protected void grvFabricatorDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grvFabricatorDetails.PageIndex = e.NewPageIndex;
        FillGrid_FabricatorDetails();
    }

    #endregion

    public void Show(string message)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('" + message + "');", true);
    }
}