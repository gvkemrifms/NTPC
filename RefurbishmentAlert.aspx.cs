using System;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using GvkFMSAPP.BLL;
using GvkFMSAPP.BLL.Alert;

public partial class RefurbishmentAlert : Page
{
    private readonly Alert _fmsAlert = new Alert();
    private readonly Helper _helper = new Helper();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Name"] == null) Response.Redirect("Login.aspx");
        if (!IsPostBack) FillGrid();
    }

    protected void FillGrid()
    {
        try
        {
            _fmsAlert.UserDistId = Convert.ToInt32(Session["UserdistrictId"].ToString());
            var ds = _fmsAlert.GetRefurbishmentAlert();
            ViewState["ds"] = ds;
            grdRefAlert.DataSource = ds;
            grdRefAlert.DataBind();
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    public string CreateHtml(DataSet ds)
    {
        var htmlText = "";
        if (ds.Tables.Count > 0)
        {
            htmlText = "<table border='1' cellpadding='2'  WIDTH='75%' ";
            htmlText += "<tr><th>Vehicle Number</th><th>Latest Odometer</th><th>Last Refurbishment Odo</th>";
            htmlText += "<th>Last Refurbishment Date</th><th>Service Alert</th>";
            htmlText += "</tr>";
            for (var i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                htmlText += "<tr>";
                htmlText += "<td> " + ds.Tables[0].Rows[i]["VehicleNumber"] + " </td>";
                htmlText += "<td>" + ds.Tables[0].Rows[i]["Latest_odo"] + "</td>";
                htmlText += "<td>" + ds.Tables[0].Rows[i]["LastRefurbishmentOdo"] + "</td>";
                htmlText += "<td>" + ds.Tables[0].Rows[i]["LastRefurbishmentDate"] + "</td>";
                htmlText += "<td> " + ds.Tables[0].Rows[i]["refalert"] + " </td>";
                htmlText += "</tr>";
            }

            htmlText += "</table>";
        }

        return htmlText;
    }

    protected void grdRefAlert_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdRefAlert.PageIndex = e.NewPageIndex;
        FillGrid();
    }

    protected void btnSendMail_Click(object sender, EventArgs e)
    {
        try
        {
            var subject = "Vehicle Refurbishment Alert";
            var mailBody = CreateHtml((DataSet) ViewState["ds"]);
            MailHelper.SendMailMessage(ConfigurationManager.AppSettings["MasterMailid"], ConfigurationManager.AppSettings["AdminMailid"], "", "", subject, mailBody);
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }
}