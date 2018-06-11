using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using GvkFMSAPP.BLL.Admin;
using ServiceReference2;
using DistrictVehicleMapping = GvkFMSAPP.DLL.Admin.DistrictVehicleMapping;
public partial class DistrictUserMapping : Page
{
    private readonly DistrictUserMappping _distUserMapping = new DistrictUserMappping();
    private readonly Helper _helper = new Helper();

    protected void Page_Load(object sender,EventArgs e)
    {
        if (Session["User_Name"] == null) Response.Redirect("Login.aspx");
        if (!IsPostBack)
        {
            FillUserList();
            GetDistrict();
        }
    }

    public void FillUserList()
    {
        var client = new ACLServiceClient();
        using (var ds = client.GetUsersList(0,0,"FMSGlobalization",""))
        {
            if (ds != null)
                try
                {
                    _helper.FillDropDownHelperMethodWithDataSet(ds,"Login_Name","PK_USER_ID",ddlUserList);
                }
                catch (Exception ex)
                {
                    _helper.ErrorsEntry(ex);
                }
        }

        ddlUserList.Items.Remove("FMSAdminUser");
    }

    public void GetDistrict()
    {
        var ds = DistrictVehicleMapping.GetDistrict();
        if (ds != null)
            try
            {
                chkDistrictList.DataSource = ds;
                chkDistrictList.DataTextField = "district_name";
                chkDistrictList.DataValueField = "district_id";
                chkDistrictList.DataBind();
            }
            catch (Exception ex)
            {
                _helper.ErrorsEntry(ex);
            }
    }

    protected void btnMapping_Click(object sender,EventArgs e)
    {
        var districtIds = new ArrayList();
        foreach (ListItem lstSelectedDistrict in chkDistrictList.Items)
            if (lstSelectedDistrict.Selected)
                districtIds.Add(lstSelectedDistrict.Value);
        if (districtIds.Count > 0)
        {
            if (_distUserMapping != null)
            {
                var ret = _distUserMapping.InsertDistrictUserMapping(Convert.ToInt32(ddlUserList.SelectedItem.Value),districtIds);
                Show(ret != 0 ? "User Mapped to States Successfully" : "Error");
            }
        }
        else
        {
            Show("Select State");
        }

        ClearControls();
    }

    protected void btnCancel_Click(object sender,EventArgs e)
    {
        ClearControls();
    }

    protected void ddlUserList_SelectedIndexChanged(object sender,EventArgs e)
    {
        var districtId = new ArrayList();
        if (ddlUserList.SelectedIndex == 0) return;
        if (_distUserMapping != null)
        {
            var ds = _distUserMapping.GetSelectedDistrictByUserList(Convert.ToInt32(ddlUserList.SelectedItem.Value));
            if (ds == null) throw new ArgumentNullException(nameof(ds));
            foreach (DataRow dr in ds.Tables[0].Rows) districtId.Add(dr["DistrictId"].ToString());
        }

        foreach (ListItem lstSelectedDistrict in chkDistrictList.Items) lstSelectedDistrict.Selected = districtId.Contains(lstSelectedDistrict.Value);
    }

    public void Show(string message)
    {
        ScriptManager.RegisterStartupScript(this,GetType(),"msg","alert('" + message + "');",true);
    }

    public void ClearControls()
    {
        ddlUserList.SelectedIndex = 0;
        foreach (ListItem lstSelectedDistricts in chkDistrictList.Items) lstSelectedDistricts.Selected = false;
    }
}