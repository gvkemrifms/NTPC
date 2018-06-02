using System;
using System.Web.UI;

public partial class OutstandingSummaryReport : Page
{
    private readonly Helper _helper = new Helper();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Name"] == null) Response.Redirect("Login.aspx");
        if (!IsPostBack)
        {
            BindDistrictdropdown();
            Withoutdist();
        }
    }

    private void BindDistrictdropdown()
    {
        try
        {
            var sqlQuery = "select ds_dsid,ds_lname from M_FMS_Districts";
            _helper.FillDropDownHelperMethod(sqlQuery, "ds_lname", "ds_dsid", ddldistrict);
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    public void Withoutdist()
    {
        try
        {
            _helper.FillDropDownHelperMethodWithSp("P_Report_VendorWiseBillsOutstandingSummaryReport", null, null, null, null, null, null, null, null, null, null, null, Grdsummary);
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
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

    public void Loaddata()
    {
        try
        {
            _helper.FillDropDownHelperMethodWithSp("P_Report_VendorWiseBillsOutstandingSummaryReport", null, null, ddldistrict, null, null, null, "@districtID", null, null, null, null, Grdsummary);
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
    }

    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        switch (ddldistrict.SelectedValue)
        {
            case "0":
                Withoutdist();
                break;
            default:
                Loaddata();
                break;
        }
    }
}