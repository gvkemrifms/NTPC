using System;
using System.Configuration;
using System.Web.UI;

public partial class DetailedReport : Page
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
            ;
            _helper.FillDropDownHelperMethod(sqlQuery, "district_name", "district_id", ddldistrict);
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

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        Loaddata();
    }

    public void Loaddata()
    {
        try
        {
            _helper.FillDropDownHelperMethodWithSp("P_FMSReports_SummaryDetailed1", null, null, ddldistrict, null, txtfrmDate, txttodate, "@DistrictID", null, "@From", "@To", null, Grddtreport);
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }
}