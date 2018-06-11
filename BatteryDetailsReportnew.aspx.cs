using System;
using System.Web.UI;

public partial class BatteryDetailsReportnew : Page
{
    private readonly Helper _helper = new Helper();

    public string UserId { get; private set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] == null)
            Response.Redirect("Login.aspx");
        else
            UserId = (string) Session["User_Id"];
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
            var sqlQuery = "select d.ds_dsid,d.ds_lname from M_FMS_Districts d join m_users u on d.ds_dsid=u.stateId where u.UserId ='" + UserId + "'";
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
            _helper.FillDropDownHelperMethodWithSp("P_FMSReport_BatteryDetails", null, null, null, null, null, null, null, null, null, null, null, GrdBataryData);
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
            _helper.FillDropDownHelperMethodWithSp("P_FMSReport_BatteryDetails", null, null, ddldistrict, null, null, null, "@DistrictID", null, null, null, null, GrdBataryData);
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
        if (ddldistrict == null || ddldistrict.SelectedValue != "0")
            Loaddata();
        else
            Withoutdist();
    }
}