using System;
using System.Web.UI;

public partial class InvoiceTrackingReport : Page
{
    private readonly Helper _helper = new Helper();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Name"] == null) Response.Redirect("Login.aspx");
        if (!IsPostBack)
        {
            ddlbillno.Enabled = false;
            Bindvehiclesdropdown();
        }
    }

    private void Bindvehiclesdropdown()
    {
        try
        {
            _helper.FillDropDownHelperMethodWithSp("P_GetVehicleNumber", "VehicleNumber", "VehicleID", ddlvehicle);
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    protected void ddlvehicle_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlvehicle.SelectedIndex <= 0)
        {
            ddlbillno.Enabled = false;
        }
        else
        {
            ddlbillno.Enabled = true;
            try
            {
                _helper.FillDropDownHelperMethodWithSp("P_GetBillNo", "Billno", "", ddlvehicle, ddlbillno, null, null, "@vehNo", null, null, null, null, null, null, null, null, 1);
            }
            catch (Exception ex)
            {
                _helper.ErrorsEntry(ex);
            }
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

    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        Loaddata();
    }

    public void Loaddata()
    {
        try
        {
            _helper.FillDropDownHelperMethodWithSp("P_Reports_VenwiseInvoiceTracking", "", "", ddlvehicle, ddlbillno, null, null, "@VehicleNo", "@BillNo", null, null, null, Grddetails);
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