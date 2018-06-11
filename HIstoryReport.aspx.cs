using System;
using System.Web.UI;
public partial class HistoryReport : Page
{
    private readonly Helper _helper = new Helper();

    public string UserId{ get;private set; }

    protected void Page_Load(object sender,EventArgs e)
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
            var sqlQuery = "select d.ds_dsid,d.ds_lname from M_FMS_Districts d join m_users u on d.ds_dsid=u.stateId where u.UserId ='" + UserId + "'";
            _helper.FillDropDownHelperMethod(sqlQuery,"ds_lname","ds_dsid",ddldistrict);
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    protected void ddldistrict_SelectedIndexChanged(object sender,EventArgs e)
    {
        if (ddldistrict.SelectedIndex <= 0)
        {
            ddlvehicle.Enabled = false;
        }
        else
        {
            ddlvehicle.Enabled = true;
            try
            {
                _helper.FillDropDownHelperMethodWithSp("P_Get_Vehicles","VehicleNumber","VehicleID",ddldistrict,ddlvehicle,null,null,"@DistrictID");
            }
            catch (Exception ex)
            {
                _helper.ErrorsEntry(ex);
            }
        }
    }

    protected void btnsubmit_Click(object sender,EventArgs e)
    {
        Loaddata();
    }

    public void Loaddata()
    {
        try
        {
            _helper.FillDropDownHelperMethodWithSp("P_Report_VehicleHistoryReport",null,null,ddldistrict,ddlvehicle,null,null,"@district_id","@VehID",null,"@Year","@Month",Grddetails,ddlmonth,ddlyear);
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    protected void btntoExcel_Click(object sender,EventArgs e)
    {
        try
        {
            _helper.LoadExcelSpreadSheet(this,Panel2,"VehicleHistoryReport.xls");
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