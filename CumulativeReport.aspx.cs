using System;
using System.Web.UI;

public partial class CumulativeReport : Page
{
    private readonly Helper _helper = new Helper();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Name"] == null) Response.Redirect("Login.aspx");
        if (!IsPostBack)
            ShowReport();
    }

    protected void ShowReport()
    {
        try
        {
            _helper.FillDropDownHelperMethodWithSp("CumulativeReportOnRO", null, null, null, null, null, null, null, null, null, null, null, GridView1);
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
            _helper.LoadExcelSpreadSheet(this, null, "CumulativeReport.xls", GridView1);
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
    }
}