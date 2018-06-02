using System;
using System.Web.UI;

public partial class ZonewiseReport : Page
{
    private readonly Helper _helper = new Helper();

    protected void Page_Load(object sender, EventArgs e)
    {
        if(Session["User_Name"]==null)Response.Redirect("Login.aspx");
        if (!IsPostBack) BindDistrictdropdown();
    }

    private void BindDistrictdropdown()
    {
        try
        {
            var sqlQuery = "select ds_dsid,ds_lname from M_FMS_Districts";
            _helper.FillDropDownHelperMethod(sqlQuery, "ds_lname", "ds_dsid", ddldistrict);
        }
        catch
        {
            //
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
    }

    protected void btntoExcel_Click(object sender, EventArgs e)
    {
        try
        {
            _helper.LoadExcelSpreadSheet(this, Panel2, "VehicleZonewise.xls");
        }
        catch
        {
            // 
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
            _helper.FillDropDownHelperMethodWithSp("P_FMSReports_ZonewiseReport", null, null, ddldistrict, ddlmonth, null, null, "@DistrictID", "@Month", null,null, "@Year", Grddetails, ddlyear);
        }
        catch(Exception ex)
        {
           _helper.ErrorsEntry(ex);
        }
    }
}