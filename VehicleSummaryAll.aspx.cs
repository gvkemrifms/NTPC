using System;
using System.Web.UI;

public partial class VehicleSummaryAll : Page
{
    private readonly Helper _helper = new Helper();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Name"] == null) Response.Redirect("Login.aspx");
        if (!IsPostBack)
            BindGridData();
    }

    private void BindGridData()
    {
        try
        {
            _helper.FillDropDownHelperMethodWithSp("vas_allvehicleregin", null, null, null, null, null, null, null, null, null, null, null, GrdtotalData);
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
            if (GrdtotalData.Rows.Count > 0)
                _helper.LoadExcelSpreadSheet(this, Panel2, "VehicleSummaryAll.xls");
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }
}