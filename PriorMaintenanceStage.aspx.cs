using System;
using System.Data;
using System.Web.UI;
using GvkFMSAPP.BLL;
using InfoSoftGlobal;

public partial class PriorMaintenanceStage : Page
{
    private readonly FMSGeneral _fmsgrph = new FMSGeneral();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Name"] == null)
            Response.Redirect("Login.aspx");
        div1.InnerHtml = CreateChart1();
    }

    public string CreateChart1()
    {
        var ds = _fmsgrph.GetPriorMaintenanceStageGraph();
        var strXml = "";
        strXml += "<graph caption='Vehicles in particular stages' YAxisName='No. of Vehicles' numDivLines='6' formatNumberScale='0' decimalPrecision='0' anchorSides='10' anchorRadius='3' anchorBorderColor='009900' rotateNames='1' animation='1' yAxisMinValue='0'>";
        strXml += "<categories>";
        strXml += "<category name='All Vehicles' /> ";
        strXml += "<category name='Fabricated Vehicles' /> ";
        strXml += "<category name='PreDelInspected Vehicles' /> ";
        strXml += "<category name='Registered Vehicles' /> ";
        strXml += "<category name='HandOvered Vehicles' /> ";
        strXml += "</categories>";
        strXml += "<dataset seriesName='Approved' color='8BBA00' showValues='1'>";
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            strXml += " <set value='" + dr[0] + "' color='8BBA00' link=''/>";
            strXml += " <set value='" + dr[1] + "' color='DBBA00' link=''/>";
            strXml += " <set value='" + dr[3] + "' color='ACBA00' link=''/>";
            strXml += " <set value='" + dr[4] + "' color='8AAA00' link=''/>";
            strXml += " <set value='" + dr[2] + "' color='CABA00' link=''/>";
        }

        strXml += " </dataset>";
        strXml += "</graph>";
        return FusionCharts.RenderChartHTML("FusionCharts/FCF_MSColumn3D.swf", "", strXml, "myNext", "700", "500", false);
    }
}