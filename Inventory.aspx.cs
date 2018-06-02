using System;
using System.Data;
using System.Web.UI;
using GvkFMSAPP.BLL;
using InfoSoftGlobal;

public partial class Inventory : Page
{
    private readonly FMSGeneral _fmsgrph = new FMSGeneral();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Name"] == null) Response.Redirect("Login.aspx");
        div1.InnerHtml = CreateChart1();
    }

    public string CreateChart1()
    {
        var ds = _fmsgrph.GetInventoryGraph();
        var strXml = "";
        strXml += "<graph caption='Approved and Rejected Requisitions' YAxisName='No. of Requisitions' numDivLines='6' formatNumberScale='0' decimalPrecision='0' anchorSides='10' anchorRadius='3' anchorBorderColor='009900' rotateNames='1' animation='1' yAxisMinValue='0'>";
        strXml += "<categories>";
        strXml += "<category name='Tyre Requisition' /> ";
        strXml += "<category name='Battery Requisition' /> ";
        strXml += "<category name='Spare Parts Requisition' /> ";
        strXml += "</categories>";
        strXml += "<dataset seriesName='Total Requests' color='0000FF' showValues='1'>";
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            strXml += " <set value='" + dr[6] + "' color='' link=''/>";
            strXml += " <set value='" + dr[7] + "' color='' link=''/>";
            strXml += " <set value='" + dr[8] + "' color='' link=''/>";
        }

        strXml += " </dataset>";
        strXml += "<dataset seriesName='Approved' color='8BBA00' showValues='1'>";
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            strXml += " <set value='" + dr[0] + "' color='' link=''/>";
            strXml += " <set value='" + dr[2] + "' color='' link=''/>";
            strXml += " <set value='" + dr[4] + "' color='' link=''/>";
        }

        strXml += " </dataset>";
        strXml += "<dataset seriesName='Rejected' color='F6BD0F' showValues='1'>";
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            strXml += " <set value='" + dr[1] + "' color='' link=''/>";
            strXml += " <set value='" + dr[3] + "' color='' link=''/>";
            strXml += " <set value='" + dr[5] + "' color='' link=''/>";
        }

        strXml += " </dataset>";
        strXml += "</graph>";
        var myChart=FusionCharts.RenderChartHTML("FusionCharts/FCF_MSColumn3D.swf", "", strXml, "myNext", "700", "500", false);
        return myChart;
    }
}