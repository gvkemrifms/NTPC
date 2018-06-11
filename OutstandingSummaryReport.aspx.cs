using System;
using System.Web.UI;
public partial class OutstandingSummaryReport : Page
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
            BindDistrictdropdown();
            Withoutdist();
        }
    }

    private void BindDistrictdropdown()
    {
        try
        {
            var query = "SELECT d.district_id as ds_dsid,d.district_name as ds_lname from [m_district] d join m_users u on d.district_id=u.stateId where u.UserId='" + UserId + "' order by d.district_name";
            _helper.FillDropDownHelperMethod(query,"ds_lname","ds_dsid",ddldistrict);
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
            _helper.FillDropDownHelperMethodWithSp("P_Report_VendorWiseBillsOutstandingSummaryReport",null,null,null,null,null,null,null,null,null,null,null,Grdsummary);
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
            _helper.LoadExcelSpreadSheet(this,Panel2,"VehicleSummaryDistrictwise.xls");
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
            _helper.FillDropDownHelperMethodWithSp("P_Report_VendorWiseBillsOutstandingSummaryReport",null,null,ddldistrict,null,null,null,"@districtID",null,null,null,null,Grdsummary);
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
    }

    protected void btnsubmit_Click(object sender,EventArgs e)
    {
        switch (ddldistrict.SelectedValue)
        {
            case "0":
                Withoutdist();
                break;
            default:
                Loaddata();
                break;
        }
    }
}