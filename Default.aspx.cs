using System;
using InfoSoftGlobal;

public partial class Default : System.Web.UI.Page
{
    void Page_PreInit(Object sender, EventArgs e)
    {
        if (Session["Role_Id"] == null)
            Response.Redirect("Login.aspx");
        else
        {
            if (Session["Role_Id"].ToString() == "118") MasterPageFile = "~/MasterERO.master";
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Name"] == null) Response.Redirect("Login.aspx");
        if (!IsPostBack) Button1_Click(sender, e);
    }

    public string CreateChart1()
    {
        string strXml = "";
        strXml += "<graph caption='Region-Wise Fuel Consumption' YAxisName='Fuel Consumption' numDivLines='6' formatNumberScale='0' decimalPrecision='0' anchorSides='10' anchorRadius='3' anchorBorderColor='009900' rotateNames='1' animation='1' yAxisMinValue='0'>";
        strXml += "<categories>";
        strXml += "<category name='Hyderabad' /> ";
        strXml += "<category name='Nizamabad' /> ";
        strXml += "<category name='Tirupati' /> ";
        strXml += "<category name='Vizag' /> ";
        strXml += "<category name='Vijayawada' /> ";
        strXml += "<category name='Warangal' /> ";
        strXml += "</categories>";
        strXml += "<dataset seriesName='Kilo Meters Traveled' color='8BBA00' showValues='1'>";
        strXml += " <set value='123' color='' link=''/>";
        strXml += " <set value='148' color='' link=''/>";
        strXml += " <set value='223' color='' link=''/>";
        strXml += " <set value='100' color='' link=''/>";
        strXml += " <set value='125' color='' link=''/>";
        strXml += " <set value='178' color='' link=''/>";
        strXml += " </dataset>";
        strXml += "<dataset seriesName='Qty in Lit' color='F6BD0F' showValues='1'>";
        strXml += " <set value='60' color='' link=''/>";
        strXml += " <set value='48' color='' link=''/>";
        strXml += " <set value='72' color='' link=''/>";
        strXml += " <set value='39' color='' link=''/>";
        strXml += " <set value='88' color='' link=''/>";
        strXml += " <set value='78' color='' link=''/>";
        strXml += " </dataset>";
        strXml += "<dataset seriesName='Total Amount' color='AFD8F8' showValues='1'>";
        strXml += " <set value='1980' color='' link=''/>";
        strXml += " <set value='1248' color='' link=''/>";
        strXml += " <set value='972' color='' link=''/>";
        strXml += " <set value='1239' color='' link=''/>";
        strXml += " <set value='1188' color='' link=''/>";
        strXml += " <set value='1578' color='' link=''/>";
        strXml += " </dataset>";
        strXml += "</graph>";
        return FusionCharts.RenderChartHTML("FusionCharts/FCF_MSColumn3D.swf", "", strXml, "myNext", "560", "400", false);
    }

    public string CreateChart2()
    {
        string strXml = "";
        strXml += "<graph  caption='Fuel Consumption by Vehicle Types' animation='1' isSliced='1' formatNumberScale='0' numberSufix='Lit' numberPrefix='' pieSliceDepth='30' decimalPrecision='0' shownames='1'>";
        strXml += "<set name='ALS' value='1000' color='AFD8F8' link=''/>";
        strXml += "<set name='BLS' value='660' color='F6BD0F' link='' isSliced='1' />";
        strXml += "<set name='RLS' value='980' color='D64646' link=''/>";
        strXml += "</graph>";
        return FusionCharts.RenderChartHTML("FusionCharts/FCF_Pie3D.swf ", "", strXml, "myNext", "560", "400", false);
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
    }
}