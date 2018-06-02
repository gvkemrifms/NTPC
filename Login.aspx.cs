using System;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Xml;
using GvkFMSAPP.BLL;
using GvkFMSAPP.BLL.Admin;
using ServiceReference2;

public partial class Login : Page
{
    private readonly DistrictUserMappping _distUserMapping = new DistrictUserMappping();
    private readonly FMSGeneral _fmsgen = new FMSGeneral();

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void btnFmsLoginSubmit_Click(object sender, EventArgs e)
    {
        var client = new ACLServiceClient();
        var results = client.LoginAuthenticate(txtFmsUsername.Text, txtFmsPassword.Text, "FMSGlobalization", "1.2.3.4", 80);
        if (results == null) throw new ArgumentNullException(nameof(results));
        var dsResults = new DataSet();
        dsResults.ReadXml(new XmlTextReader(new StringReader(results)));
        var resultFlag = Convert.ToInt32(dsResults.Tables[0].Rows[0].ItemArray[0].ToString());
        switch (resultFlag)
        {
            case 1:
            case 4:
                lblResult.Text = "Valid User, Authentication is Sucess";
                // Create the Session Objects User Id, User Name, Role Id, Role Name, Module Name...
                Session["User_Id"] = dsResults.Tables[0].Rows[0].ItemArray[1].ToString();
                var ds1 = _distUserMapping.GetSelectedDistrictByUserList(Convert.ToInt32(dsResults.Tables[0].Rows[0].ItemArray[1].ToString()));
                Session["UserdistrictId"] = ds1.Tables.Count <= 0 ? -1 : (ds1.Tables[0].Rows.Count > 0 ? (object) _distUserMapping.GetSelectedDistrictByUserList(Convert.ToInt32(dsResults.Tables[0].Rows[0].ItemArray[1].ToString())).Tables[0].Rows[0].ItemArray[2].ToString() : -1);
                Session["User_Name"] = dsResults.Tables[0].Rows[0].ItemArray[2].ToString();
                Session["Role_Id"] = dsResults.Tables[0].Rows[0].ItemArray[3].ToString();
                Session["Role_Name"] = dsResults.Tables[0].Rows[0].ItemArray[4].ToString();
                Session["Module_Name"] = dsResults.Tables[0].Rows[0].ItemArray[5].ToString();
                var ds = client.GetMenuItems(Convert.ToInt16(Session["User_Id"].ToString()), Convert.ToInt16(dsResults.Tables[0].Rows[0]["m_moid"].ToString()), Convert.ToInt16(Session["Role_Id"].ToString()));
                if (ds == null) throw new ArgumentNullException(nameof(ds));
                Session["PermissionsDS"] = ds;
                // Redirect to Home Page...
                if (Convert.ToInt16(Session["UserdistrictId"].ToString()) > 0)
                {
                    var dist = _fmsgen.GetUserDistrict(Convert.ToInt16(Session["User_Id"].ToString()));
                    if (dist == null) throw new ArgumentNullException(nameof(dist));
                    Session["District_Name"] = dist.Tables[0].Rows[0]["district_name"].ToString();
                }
                else
                    Session["District_Name"] = "All";

                switch (Request.QueryString["alert"])
                {
                    case null:
                        break;
                    default:
                        var strQuery = Request.QueryString["alert"];
                        var id = Request.QueryString["vehicleNumber"];
                        switch (strQuery)
                        {
                            case "FR":
                                Response.Redirect("~/FitnessRenewal.aspx?vehicleid=" + id);
                                break;
                            case "PUC":
                                Response.Redirect("~/PollutionUnderControl.aspx?vehicleid=" + id);
                                break;
                            case "RT":
                                Response.Redirect("~/RoadTax.aspx?vehicleid=" + id);
                                break;
                            case "VI":
                                Response.Redirect("~/VehicleInsurance.aspx?vehicleid=" + id);
                                break;
                        }

                        break;
                }

                Response.Redirect("Default.aspx", true);
                //Server.Transfer("Default.aspx");
                break;
            case 2:
                lblResult.Text = "Invalid Password";
                //Show("Invalid Password");
                txtFmsPassword.Text = string.Empty;
                txtFmsPassword.Focus();
                break;
            case 3:
                //Show("Unauthorized user to login, contact Administrator");
                lblResult.Text = "Unauthorized user to login, contact Administrator";
                break;
            default:
                lblResult.Text = "Invalid user";
                break;
        }

        client.Close();
    }

    protected void btnFmsLoginReset_Click(object sender, EventArgs e)
    {
        txtFmsUsername.Text = "";
        txtFmsPassword.Text = "";
        RequiredFieldValidator1.ErrorMessage = "";
        RequiredFieldValidator2.ErrorMessage = "";
    }

    public void Show(string message)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('" + message + "');", true);
    }
}