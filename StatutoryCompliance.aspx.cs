using System;
using System.Data;
using System.Web.UI;
using GvkFMSAPP.BLL;
using InfoSoftGlobal;

public partial class StatutoryCompliance : Page
{
    private readonly FMSGeneral _fmsgrph = new FMSGeneral();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Name"] == null) Response.Redirect("Error.aspx");
        div1.InnerHtml = CreateChart1();
    }

    public string CreateChart1()
    {
        _fmsgrph.UserDistrictId = Convert.ToInt32(Session["UserdistrictId"].ToString());
        var ds = _fmsgrph.GetStatutoryComplianceGraph();
        var strXml = "";
        strXml += "<graph caption='Valid Expired Vehicles' YAxisName='No. of Vehicles' numDivLines='6' formatNumberScale='0' decimalPrecision='0' anchorSides='10' anchorRadius='3' anchorBorderColor='009900' rotateNames='1' animation='1' yAxisMinValue='0'>";
        strXml += "<categories>";
        strXml += "<category name='Vehicle Insurance' /> ";
        strXml += "<category name='Road Tax' /> ";
        strXml += "<category name='Pollution Under Control' /> ";
        strXml += "<category name='Fitness Renewal' /> ";
        strXml += "</categories>";
        strXml += "<dataset seriesName='No. of Valid Vehicles' color='8BBA00' showValues='1'>";
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            strXml += " <set value='" + dr[0] + "' color='' link=''/>";
            strXml += " <set value='" + dr[2] + "' color='' link=''/>";
            strXml += " <set value='" + dr[4] + "' color='' link=''/>";
            strXml += " <set value='" + dr[6] + "' color='' link=''/>";
        }

        strXml += " </dataset>";
        strXml += "<dataset seriesName='No. of Expired Vehicles' color='F6BD0F' showValues='1'>";
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            strXml += " <set value='" + dr[1] + "' color='' link=''/>";
            strXml += " <set value='" + dr[3] + "' color='' link=''/>";
            strXml += " <set value='" + dr[5] + "' color='' link=''/>";
            strXml += " <set value='" + dr[7] + "' color='' link=''/>";
        }

        strXml += " </dataset>";
        strXml += "</graph>";
        return FusionCharts.RenderChartHTML("FusionCharts/FCF_MSColumn3D.swf", "", strXml, "myNext", "700", "500", false);
    }
}