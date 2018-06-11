using System;
using System.Web.UI;

public partial class FuelReport : Page
{
    private readonly Helper _helper = new Helper();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Name"] == null) Response.Redirect("Login.aspx");
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            var query = "SELECT distinct  v.VehicleNumber,tbl.fuelentryid , hist.Odometer 'Previous_odo',  tbl.Odometer 'present_odo', tbl.Quantity, " + " tbl.Location, tbl.KMPL, tbl.Odometer - hist.Odometer 'Totalkm_Run', tbl.EntryDate, tbl.Pilot, tbl.PilotName, " + "         tbl.Amount  FROM dbo.T_FMS_FuelEntryDetails tbl join [dbo].[M_FMS_Vehicles] v on " + "  tbl.VehicleID = v.VehicleID  INNER JOIN dbo.T_FMS_FuelEntryDetails hist ON hist.VehicleID = tbl.VehicleID " + "   AND hist.EntryDate = (SELECT MAX(EntryDate) FROM dbo.T_FMS_FuelEntryDetails sub WHERE sub.VehicleID = tbl.VehicleID " + "   AND sub.EntryDate < tbl.EntryDate ) and tbl.EntryDate between '" + txtfromdate.Text.Trim() + "' and '" + txttodate.Text.Trim() + "' union " + " SELECT distinct v.VehicleNumber,tbl.fuelentryid , 0 'Previous_odo',  tbl.Odometer 'present_odo', tbl.Quantity, " + "   tbl.Location, tbl.KMPL, tbl.Odometer - 0 'Totalkm_Run', tbl.EntryDate, tbl.Pilot, tbl.PilotName, " + " tbl.Amount  FROM dbo.T_FMS_FuelEntryDetails tbl join [dbo].[M_FMS_Vehicles] v on  tbl.VehicleID = v.VehicleID join " + "  (select min(EntryDate) EntryDate, VehicleID from T_FMS_FuelEntryDetails tbl where tbl.EntryDate  between '" + txtfromdate.Text.Trim() + "' and '" + txttodate.Text.Trim() + "' " + "   group by VehicleID ) stbl on stbl.VehicleID = tbl.VehicleID and tbl.EntryDate = stbl.EntryDate left join " + "   (SELECT distinct  v.VehicleNumber,tbl.fuelentryid , hist.Odometer 'Previous_odo',  tbl.Odometer 'present_odo', tbl.Quantity, " + "   tbl.Location, tbl.KMPL, tbl.Odometer - hist.Odometer 'Totalkm_Run', tbl.EntryDate, tbl.Pilot, tbl.PilotName, " + "   tbl.Amount  FROM dbo.T_FMS_FuelEntryDetails tbl join [dbo].[M_FMS_Vehicles] v on " + "   tbl.VehicleID = v.VehicleID  INNER JOIN dbo.T_FMS_FuelEntryDetails hist ON hist.VehicleID = tbl.VehicleID " + "    AND hist.EntryDate = (SELECT MAX(EntryDate) FROM dbo.T_FMS_FuelEntryDetails sub WHERE sub.VehicleID = tbl.VehicleID " + "   AND sub.EntryDate < tbl.EntryDate ) and tbl.EntryDate between '" + txtfromdate.Text.Trim() + " 00:00:00' and '" + txttodate.Text.Trim() + " 23:59:59') tblo on " + " tblo.FuelEntryID = tbl.FuelEntryID where tblo.FuelEntryID is null order by VehicleNumber ";
            var dataTable = _helper.ExecuteSelectStmt(query);
            GridView1.DataSource = dataTable;
            GridView1.DataBind();
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
            _helper.LoadExcelSpreadSheet(this, null, "gvtoexcel.xls", GridView1);
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