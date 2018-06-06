using System;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using GvkFMSAPP.PL;

public partial class FleetManufacturers : Page
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
            grvManufacturerDetails.Columns[0].Visible = false;
            FillDistricts();
            FillGrid_FleetManufacturerDetails();
            btnManufacturerSave.Attributes.Add("onclick", "javascript:return validationFleetManufacturers()");
            txtManufacturerName.Attributes.Add("onkeypress", "javascript:return OnlyAlphabets(this,event)");
            txtManufacturerModel.Attributes.Add("onkeypress", "javascript:return OnlyAlphaNumeric(this,event)");
            txtManufacturerAddress.Attributes.Add("onkeypress", "javascript:return remark(this,event)");
            txtManufacturerContactPerson.Attributes.Add("onkeypress", "javascript:return OnlyAlphabets(this,event)");
        }

        //Permissions
        var dsPerms = (DataSet) Session["PermissionsDS"];
        if (dsPerms == null) throw new ArgumentNullException(nameof(dsPerms));
        dsPerms.Tables[0].DefaultView.RowFilter = "Url='" + Page.Request.Url.Segments[Page.Request.Url.Segments.Length - 1] + "'";
        var p = new PagePermissions(dsPerms, dsPerms.Tables[0].DefaultView[0]["Url"].ToString(), dsPerms.Tables[0].DefaultView[0]["Title"].ToString());
        pnlFleetManufacturers.Visible = false;
        grvManufacturerDetails.Visible = false;
        if (p.View)
        {
            grvManufacturerDetails.Visible = true;
            grvManufacturerDetails.Columns[5].Visible = false;
        }

        if (p.Add)
        {
            pnlFleetManufacturers.Visible = true;
            grvManufacturerDetails.Visible = true;
            grvManufacturerDetails.Columns[5].Visible = false;
        }

        if (p.Modify)
        {
            grvManufacturerDetails.Visible = true;
            grvManufacturerDetails.Columns[5].Visible = true;
        }
    }

    #endregion

    #region Reset Function

    private void FleetManufacturerDetailsReset()
    {
        txtManufacturerName.Text = "";
        txtManufacturerAddress.Text = "";
        txtManufacturerContactNumber.Text = "";
        txtManufacturerContactPerson.Text = "";
        txtManufacturerEmailId.Text = "";
        txtManufacturerErn.Text = "";
        txtManufacturerModel.Text = "";
        txtManufacturerTin.Text = "";
        ddlFleetManufacturerDistrict.SelectedIndex = 0;
        ddlManufacturerType.SelectedIndex = 0;
        btnManufacturerSave.Text = "Save";
    }

    #endregion
    #region Fill Districts Function

    private void FillDistricts()
    {
        string query = ConfigurationManager.AppSettings["Query"]+" "+ "where u.UserId ='" + UserId + "'";
            try
            {
                _helper.FillDropDownHelperMethod(query, "DISTRICT_NAME", "DISTRICT_ID", ddlFleetManufacturerDistrict);
            }
            catch (Exception ex)
            {
                _helper.ErrorsEntry(ex);
            }
    }

    #endregion

   
    #region Save and Update Button

    protected void btnManufacturerSave_Click(object sender, EventArgs e)
    {
        if (btnManufacturerSave != null)
            try
            {
                switch (btnManufacturerSave.Text)
                {
                    case "Save":
                    {
                        var ds = _fleetMaster.FillGrid_FleetManufacturerDetails();
                        if (ds.Tables[0].Select("FleetManufacturer_Name='" + txtManufacturerName.Text + "'").Length <= 0)
                        {
                            var mfname = txtManufacturerName.Text;
                            var mftypid = Convert.ToInt32(ddlManufacturerType.SelectedValue);
                            var mfmodel = txtManufacturerModel.Text;
                            var mfdist = Convert.ToInt32(ddlFleetManufacturerDistrict.SelectedValue);
                            var mfmandal = 0;
                            var mfcity = 0;
                            var mfaddress = txtManufacturerAddress.Text;
                            var mfcontno = Convert.ToInt64(txtManufacturerContactNumber.Text);
                            var mfcontper = txtManufacturerContactPerson.Text;
                            var mfmail = txtManufacturerEmailId.Text;
                            var mftin = Convert.ToInt64(txtManufacturerTin.Text);
                            var mfern = Convert.ToInt64(txtManufacturerErn.Text);
                            var mfstatus = 1;
                            var mfinactby = Convert.ToString(Session["User_Id"]);
                            var mfinactdate = DateTime.Today;
                            var mfcreatedate = DateTime.Today;
                            var mfcreateby = Convert.ToString(Session["User_Id"]);
                            var mfupdtdate = DateTime.Today;
                            var mfupdateby = Convert.ToString(Session["User_Id"]);
                            ds = _fleetMaster.InsertManufacturerDetails(mfname, mftypid, mfmodel, mfdist, mfmandal, mfcity, mfaddress, mfcontno, mfcontper, mfmail, mftin, mfern, mfstatus, mfinactby, mfinactdate, mfcreatedate, mfcreateby, mfupdtdate, mfupdateby);
                            switch (ds.Tables.Count)
                            {
                                case 0:
                                    Show("Manufacturer Details added successfully");
                                    FleetManufacturerDetailsReset();
                                    break;
                                default:
                                    Show("This Manufacturer details already exists");
                                    break;
                            }
                        }
                        else
                        {
                            Show("Manufacturer Name Already Exists");
                        }
                    }

                        break;

                    default:
                    {
                        var ds = _fleetMaster.FillGrid_FleetManufacturerDetails();
                        if (ds == null) throw new ArgumentNullException(nameof(ds));
                        if (ds.Tables[0].Select("FleetManufacturer_Name='" + txtManufacturerName.Text + "' And FleetManufacturer_Id<>'" + hidManId.Value + "'").Length <= 0)
                        {
                            int mfId = Convert.ToInt16(hidManId.Value);
                            var mfname = txtManufacturerName.Text;
                            var mftypid = Convert.ToInt32(ddlManufacturerType.SelectedValue);
                            var mfmodel = txtManufacturerModel.Text;
                            var mfdist = Convert.ToInt32(ddlFleetManufacturerDistrict.SelectedValue);
                            var mfmandal = 0;
                            var mfcity = 0;
                            var mfaddress = txtManufacturerAddress.Text;
                            var mfcontno = Convert.ToInt64(txtManufacturerContactNumber.Text);
                            var mfcontper = txtManufacturerContactPerson.Text;
                            var mfmail = txtManufacturerEmailId.Text;
                            var mftin = Convert.ToInt64(txtManufacturerTin.Text);
                            var mfern = Convert.ToInt64(txtManufacturerErn.Text);
                            ds = _fleetMaster.UpdateManufacturerDetails(mfId, mfname, mftypid, mfmodel, mfdist, mfmandal, mfcity, mfaddress, mfcontno, mfcontper, mfmail, mftin, mfern);
                            switch (ds.Tables.Count)
                            {
                                case 0:
                                    Show("Manufacturer Details Updated successfully");
                                    FleetManufacturerDetailsReset();
                                    break;
                                default:
                                    Show("This Manufacturer details already exists ");
                                    break;
                            }
                        }
                        else
                        {
                            Show("Manufacturer Name Already Exists");
                        }

                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                _helper.ErrorsEntry(ex);
            }

        FillGrid_FleetManufacturerDetails();
    }

    #endregion

    #region Reset Button

    protected void btnManufacturerReset_Click(object sender, EventArgs e)
    {
        FleetManufacturerDetailsReset();
    }

    #endregion

    #region Filling Gridview of Manufacturer Details

    public void FillGrid_FleetManufacturerDetails()
    {
        var ds = _fleetMaster.FillGrid_FleetManufacturerDetails();
        if (ds != null)
        {
            if (ds.Tables[0].Rows.Count <= 0) return;
            grvManufacturerDetails.DataSource = ds.Tables[0];
            grvManufacturerDetails.DataBind();
        }
        else
        {
            var strScript1 = "<script language=JavaScript>alert('" + "No record found" + "')</script>";
            ClientScript.RegisterStartupScript(GetType(), "Success", strScript1);
        }
    }

    #endregion

    #region Row Editing of Manufacturer Details

    protected void grvManufacturerDetails_RowEditing(object sender, GridViewEditEventArgs e)
    {
        btnManufacturerSave.Text = "Update";
        var index = e.NewEditIndex;
        var lblid = (Label) grvManufacturerDetails.Rows[index].FindControl("lblId");
        hidManId.Value = lblid.Text;
        int mfId = Convert.ToInt16(hidManId.Value);
        var ds = _fleetMaster.RowEditManufacturerDetails(mfId);
        Session["District_Id"] = ds.Tables[0].Rows[0].ItemArray[4].ToString();
        Session["Mandal_Id"] = ds.Tables[0].Rows[0].ItemArray[5].ToString();
        Session["City_Id"] = ds.Tables[0].Rows[0].ItemArray[6].ToString();
        txtManufacturerName.Text = Convert.ToString(ds.Tables[0].Rows[0]["FleetManufacturer_Name"].ToString());
        ddlManufacturerType.SelectedValue = ds.Tables[0].Rows[0]["FleetType_Id"].ToString();
        txtManufacturerModel.Text = Convert.ToString(ds.Tables[0].Rows[0]["Fleet_Model"].ToString());
        txtManufacturerAddress.Text = Convert.ToString(ds.Tables[0].Rows[0]["FleetManufacturer_Address"].ToString());
        txtManufacturerContactNumber.Text = Convert.ToString(ds.Tables[0].Rows[0]["FleetManufacturer_ContactNo"].ToString());
        txtManufacturerContactPerson.Text = Convert.ToString(ds.Tables[0].Rows[0]["FleetManufacturer_ContactPerson"].ToString());
        txtManufacturerEmailId.Text = Convert.ToString(ds.Tables[0].Rows[0]["FleetManufacturer_EmailId"].ToString());
        txtManufacturerTin.Text = Convert.ToString(ds.Tables[0].Rows[0]["FleetManufacturer_TIN"].ToString());
        txtManufacturerErn.Text = Convert.ToString(ds.Tables[0].Rows[0]["FleetManufacturer_ERN"].ToString());
        //FillStates();
        FillDistricts();
        ddlFleetManufacturerDistrict.SelectedValue = ds.Tables[0].Rows[0]["District_Id"].ToString();
    }

    #endregion

    #region Page Index Changing of Manufacturer Details

    protected void grvManufacturerDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grvManufacturerDetails.PageIndex = e.NewPageIndex;
        FillGrid_FleetManufacturerDetails();
    }

    #endregion

    public void Show(string message)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('" + message + "');", true);
    }

    #region Selected Index Change for Mandal Drop Down List

    #endregion
}