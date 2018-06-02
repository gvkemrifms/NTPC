//using Microsoft.Office.Interop.Excel;

using System;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class IoclEmriRep : Page
{
    readonly Helper _helper = new Helper();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Name"] == null) Response.Redirect("login.aspx");
        if (!IsPostBack)
        {
            BindVehicles();
            tblHeader.Visible = false;
        }
    }

    private void BindVehicles()
    {
        try
        {
            var sqlQuery = "select * from m_fms_vehicles  order by vehicleNumber";
            _helper.FillDropDownHelperMethod(sqlQuery, "vehicleNumber", "vehicleid", ddlvehicleNo);
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
            _helper.LoadExcelSpreadSheet(this, Panel4, "gvtoexcel.xls");
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        //  getdates
        try
        {
            var dtData = _helper.ExecuteSelectStmt("exec getdates '" + txtfromdate.Text + "','" + txttodate.Text + "','" + ddlvehicleNo.SelectedValue + "'");
            if (dtData == null) throw new ArgumentNullException(nameof(dtData));
            if (dtData.Rows.Count <= 0)
            {
                grdRepData.DataSource = null;
                grdRepData.DataBind();
                Show("No Records Found");
                tblHeader.Visible = false;
            }
            else
            {
                grdRepData.DataSource = dtData;
                grdRepData.DataBind();
                tblHeader.Visible = true;
                Page.ClientScript.RegisterStartupScript(GetType(), "CallMyFunction", "rearrange()", true);
            }
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    public void Show(string message)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('" + message + "');", true);
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
    }

    protected void grdRepData_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:
                var ioc = e.Row.Cells[12].Text;
                var emri = e.Row.Cells[21].Text;
                if (ioc == "&nbsp;" || emri == "&nbsp;")
                {
                    if (ioc != emri) e.Row.Attributes["style"] = "background-color: #ED2C7733";
                }
                else
                {
                    var iocFloat = float.Parse(ioc);
                    var emriFloat = float.Parse(emri);
                    var iocint = (int) iocFloat;
                    var emriint = (int) emriFloat;
                    if (iocint != emriint) e.Row.Attributes["style"] = "background-color: #ED2C7733";
                }

                break;
        }
    }
}