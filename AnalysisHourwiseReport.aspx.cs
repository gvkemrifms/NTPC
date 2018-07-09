using System;
using System.Configuration;
using System.Web.UI;

public partial class AnalysisHourwiseReport : Page
{
    private readonly Helper _helper = new Helper();

    public string UserId { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] == null)
            Response.Redirect("Login.aspx");
        else
            UserId = (string) Session["User_Id"];
        if (!IsPostBack)
        {
            ddlvehicle.Enabled = false;
            BindDistrictdropdown();
        }
    }

    private void BindDistrictdropdown()
    {
        try
        {
            var sqlQuery = ConfigurationManager.AppSettings["Query"] + " " + "where u.UserId ='" + UserId + "'";
            _helper.FillDropDownHelperMethod(sqlQuery, "district_name", "district_id", ddldistrict);
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    protected void ddldistrict_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddldistrict.SelectedIndex > 0)
        {
            ddlvehicle.Enabled = true;
            try
            {
                _helper.FillDropDownHelperMethodWithSp("P_GetVehicleNumber", "VehicleNumber", "VehicleID", ddldistrict, ddlvehicle, null, null, "@districtID");
            }
            catch (Exception ex)
            {
                _helper.ErrorsEntry(ex);
            }
        }
        else
        {
            ddlvehicle.Enabled = false;
        }
    }

    protected void btntoExcel_Click(object sender, EventArgs e)
    {
        try
        {
            _helper.LoadExcelSpreadSheet(this, null, "VehicleSummaryDistrictwise.xls", Grddetails);
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
             // Grddetails.Columns[0].HeaderText = "State";
            _helper.FillDropDownHelperMethodWithSp("P_Report_AccidentAnalysisHourwise", null, null, ddldistrict, ddlvehicle, txtfrmDate, txttodate, "@DistrictID", "@VehicleID", "@From", "@To", null, Grddetails);
            Grddetails.HeaderRow.Cells[0].Text = "State";
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