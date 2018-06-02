using System;
using System.Configuration;
using System.Web.UI;

public partial class InvoiceSubmissionToFinanceReport : Page
{
    private readonly Helper _helper = new Helper();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Name"] == null) Response.Redirect("Login.aspx");
        if (!IsPostBack)
            BindDistrictdropdown();
    }

    private void BindDistrictdropdown()
    {
        try
        {
            var sqlQuery = ConfigurationManager.AppSettings["Query"];
            _helper.FillDropDownHelperMethod(sqlQuery, "district_name", "district_id", ddldistrict);
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

    public override void VerifyRenderingInServerForm(Control control)
    {
    }

    protected void ddldistrict_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddldistrict.SelectedIndex <= 0)
        {
            ddlvendor.Enabled = false;
        }
        else
        {
            ddlvendor.Enabled = true;
            try
            {
                _helper.FillDropDownHelperMethodWithSp("P_Get_Agency", "AgencyName", "AgencyID", ddldistrict, ddlvendor, null, null, "@DistrictID");
            }
            catch (Exception ex)
            {
                _helper.ErrorsEntry(ex);
            }
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
            _helper.FillDropDownHelperMethodWithSp("P_ReportsVendorWiseINvoiceSubtoFinance", null, null, ddldistrict, ddlvendor, txtfrmDate, txttodate, "@districtID", "@VendorID", "@From", "@To", null, Grddetails);
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }
}