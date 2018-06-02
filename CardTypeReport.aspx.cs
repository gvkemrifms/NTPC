using System;
using System.Configuration;
using System.Web.UI;

public partial class CardTypeReport : Page
{
    private readonly Helper _helper = new Helper();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["User_Name"] == null) Response.Redirect("Login.aspx");
            ddlstation.Enabled = false;
            BindDistrictdropdown();
        }
    }

    private void BindDistrictdropdown()
    {
        var sqlQuery = ConfigurationManager.AppSettings["Query"];
        _helper.FillDropDownHelperMethod(sqlQuery, "district_name", "district_id", ddldistrict);
    }

    protected void ddldistrict_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddldistrict.SelectedIndex > 0)
        {
            ddlstation.Enabled = true;
            try
            {
                _helper.FillDropDownHelperMethodWithSp("P_PMS_GetServiceStns", "ServiceStation_Name", "Id", ddldistrict, ddlstation, null, null, "@DistrictID");
            }
            catch (Exception ex)
            {
                _helper.ErrorsEntry(ex);
            }
        }
        else
        {
            ddlstation.Enabled = false;
        }
    }

    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        Loaddata();
    }

    public void Loaddata()
    {
        try
        {
            _helper.FillDropDownHelperMethodWithSp("P_FMSReports_GetCardType", null, null, ddldistrict, ddlstation, null, null, "@DistrictID", "@BunkID", null, null, null, GrdcardData);
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
    }

    protected void btntoExcel_Click(object sender, EventArgs e)
    {
        try
        {
            _helper.LoadExcelSpreadSheet(this, Panel2, "VehicleSummaryDistrictwise.xls");
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }
}