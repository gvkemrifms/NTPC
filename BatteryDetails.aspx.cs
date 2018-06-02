using System;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class BatteryDetails : Page
{
    private readonly FleetMaster _fleetMaster = new FleetMaster();

    #region Page Load

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Name"] == null) Response.Redirect("Error.aspx");
        if (!IsPostBack)
        {
            grvBatteryDetails.Columns[0].Visible = false;
            txtBatteryItemCode.Attributes.Add("onkeypress", "javascript:return OnlyNumbers(this,event)");
            txtBatteryMake.Attributes.Add("onkeypress", "javascript:return OnlyAlphabets(this,event)");
            txtBatteryModel.Attributes.Add("onkeypress", "javascript:return OnlyAlphaNumeric(this,event)");
            txtBatteryCapacity.Attributes.Add("onkeypress", "javascript:return OnlyAlphaNumeric(this,event)");
            FillGrid_BatteryDetails();
        }
    }

    #endregion

    #region Filling Gridview of Battery Details

    public void FillGrid_BatteryDetails()
    {
        var ds = _fleetMaster.FillGrid_BatteryDetails();
        if (ds == null) return;
        grvBatteryDetails.DataSource = ds.Tables[0];
        grvBatteryDetails.DataBind();
    }

    #endregion

    #region Reset Function

    private void FleetBatteryDetailsReset()
    {
        txtBatteryItemCode.Text = "";
        txtBatteryMake.Text = "";
        txtBatteryModel.Text = "";
        txtBatteryCapacity.Text = "";
        btnBatterySave.Text = "Save";
        txtBatteryExpiryDate.Text = "";
    }

    #endregion

    #region Save and Update Button

    protected void btnBatterySave_Click1(object sender, EventArgs e)
    {
        switch (btnBatterySave.Text)
        {
            case "Save":
            {
                var ds = _fleetMaster.FillGrid_BatteryDetails();
                if (ds == null) throw new ArgumentNullException(nameof(ds));
                if (ds.Tables[0].Select("Battery_Item_Code='" + txtBatteryItemCode.Text + "'").Length > 0)
                {
                    Show("Battery Item Code already exists");
                }
                else
                {
                    var mbatteryitemcode = txtBatteryItemCode.Text;
                    var mbmake = txtBatteryMake.Text;
                    var mbmodel = txtBatteryModel.Text;
                    var mbcapacity = txtBatteryCapacity.Text;
                    if (txtBatteryExpiryDate != null)
                    {
                        var mbexpiry = DateTime.ParseExact(txtBatteryExpiryDate.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                        var mbstatus = 1;
                        var mbinactdate = DateTime.Today;
                        var mbcreationdate = DateTime.Today;
                        var mbcreateby = Convert.ToString(Session["User_Id"]);
                        var mbupdatedate = DateTime.Today;
                        var mbupdateby = Convert.ToString(Session["User_Id"]);
                        var mboutput = string.Empty;
                        ds = _fleetMaster.InsertBatteryDetails(mbatteryitemcode, mbmake, mbmodel, mbcapacity, mbexpiry, mbstatus, mbinactdate, mbcreationdate, mbcreateby, mbupdatedate, mbupdateby, ref mboutput);
                        if (ds.Tables.Count == 0 && mboutput == "Success")
                        {
                            Show("Battery Details added successfully");
                            FleetBatteryDetailsReset();
                        }
                        else
                        {
                            Show("This Battery details already exists");
                        }
                    }
                }

                break;
            }
            default:
            {
                var ds = _fleetMaster.FillGrid_BatteryDetails();
                if (ds == null) throw new ArgumentNullException(nameof(ds));
                if (ds.Tables[0].Select("Battery_Item_Code='" + txtBatteryItemCode.Text + "' And Battery_Id<>'" + hidManId.Value + "'").Length <= 0)
                {
                    int ubatid = Convert.ToInt16(hidManId.Value);
                    var batitemcode = txtBatteryItemCode.Text;
                    var umake = txtBatteryMake.Text;
                    var umodel = txtBatteryModel.Text;
                    var ucapac = txtBatteryCapacity.Text;
                    var uexpiry = DateTime.ParseExact(txtBatteryExpiryDate.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    ds = _fleetMaster.UpdateBatteryDetails(ubatid, batitemcode, umake, umodel, ucapac, uexpiry);
                    switch (ds.Tables.Count)
                    {
                        case 0:
                            Show("Battery Details Updated successfully");
                            FleetBatteryDetailsReset();
                            break;
                        default:
                            Show("This Battery details already exists ");
                            break;
                    }
                }
                else
                {
                    Show("Battery Item Code already exists");
                }

                break;
            }
        }

        FillGrid_BatteryDetails();
    }

    #endregion

    #region Row Editing of Battery Details

    protected void grvBatteryDetails_RowEditing(object sender, GridViewEditEventArgs e)
    {
        btnBatterySave.Text = "Update";
        var index = e.NewEditIndex;
        var lblid = (Label) grvBatteryDetails.Rows[index].FindControl("lblbatId");
        hidManId.Value = lblid.Text;
        int bfId = Convert.ToInt16(hidManId.Value);
        var ds = _fleetMaster.RowEditBatteryDetails(bfId);
        txtBatteryItemCode.Text = Convert.ToString(ds.Tables[0].Rows[0]["Battery_Item_Code"].ToString()).Trim();
        txtBatteryMake.Text = Convert.ToString(ds.Tables[0].Rows[0]["Make"].ToString());
        txtBatteryModel.Text = Convert.ToString(ds.Tables[0].Rows[0]["Model"].ToString());
        txtBatteryCapacity.Text = Convert.ToString(ds.Tables[0].Rows[0]["CapaCity"].ToString());
        var shortDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["BatteryExpiryDate"]);
        txtBatteryExpiryDate.Text = shortDate.ToShortDateString();
    }

    #endregion

    #region Reset Click Event

    protected void btnManufacturerReset_Click(object sender, EventArgs e)
    {
        FleetBatteryDetailsReset();
    }

    #endregion

    #region Page Index Changing Event

    protected void grvBatteryDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grvBatteryDetails.PageIndex = e.NewPageIndex;
        FillGrid_BatteryDetails();
    }

    #endregion

    public void Show(string message)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('" + message + "');", true);
    }
}