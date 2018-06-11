using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using GvkFMSAPP.PL;

public partial class InsuranceAgencies : Page
{
    private readonly FleetMaster _fleetMaster = new FleetMaster();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Name"] == null) Response.Redirect("Login.aspx");
        if (!IsPostBack)
        {
            grvInsuranceAgencyDetails.Columns[0].Visible = false;
            FillGridInsuranceAgencyDetails();
            txtContactNo.Attributes.Add("onkeypress", "javascript:return numeric_only(event)");
            txtInsuranceAgency.Attributes.Add("onkeypress", "javascript:return OnlyAlphabets(this,event)");
            txtContactPerson.Attributes.Add("onkeypress", "javascript:return OnlyAlphabets(this,event)");
            txtAddress.Attributes.Add("onkeypress", "javascript:return remark(this,event)");
            //Permissions
            var dsPerms = (DataSet) Session["PermissionsDS"];
            if (dsPerms == null) throw new ArgumentNullException(nameof(dsPerms));
            dsPerms.Tables[0].DefaultView.RowFilter = "Url='" + Page.Request.Url.Segments[Page.Request.Url.Segments.Length - 1] + "'";
            var p = new PagePermissions(dsPerms, dsPerms.Tables[0].DefaultView[0]["Url"].ToString(), dsPerms.Tables[0].DefaultView[0]["Title"].ToString());
            if (p.Modify) return;
            grvInsuranceAgencyDetails.Visible = true;
            grvInsuranceAgencyDetails.Columns[5].Visible = false;
        }
    }

    #region Filling Gridview of Insurance Agency Details

    public void FillGridInsuranceAgencyDetails()
    {
        var ds = _fleetMaster.FillInsuranceAgencies();
        if (ds == null) return;
        grvInsuranceAgencyDetails.DataSource = ds;
        grvInsuranceAgencyDetails.DataBind();
    }

    #endregion

    #region Reset Function

    private void InsuranceAgencyDetailsReset()
    {
        txtInsuranceAgency.Text = "";
        txtAddress.Text = "";
        txtContactPerson.Text = "";
        txtContactNo.Text = "";
        btnInsuranceUpdate.Text = "Insert";
    }

    #endregion

    protected void btnInsuranceUpdate_Click(object sender, EventArgs e)
    {
        var result = 0;
        switch (btnInsuranceUpdate.Text)
        {
            case "Insert":
                if (_fleetMaster.CheckInsuranceAgency(txtInsuranceAgency.Text) > 0)
                    Show("Insurance Agency Already Exist");
                else
                    result = _fleetMaster.InsertInsuranceAgency(txtInsuranceAgency.Text, txtAddress.Text, txtContactPerson.Text, Convert.ToInt64(txtContactNo.Text));
                if (result != 0)
                {
                    Show("Insurance agency added successfully");
                    FillGridInsuranceAgencyDetails();
                    InsuranceAgencyDetailsReset();
                }

                break;
            default:
                if (_fleetMaster.CheckInsuranceAgency(int.Parse(ViewState["InsuranceId"].ToString()), txtInsuranceAgency.Text) <= 0)
                    result = _fleetMaster.UpdateInsuranceAgency(Convert.ToInt32(ViewState["InsuranceId"]), txtInsuranceAgency.Text, txtAddress.Text, txtContactPerson.Text, Convert.ToInt64(txtContactNo.Text));
                else
                    Show("Insurance Agency Already Exist");
                if (result > 0)
                {
                    Show("Insurance agency Updated successfully");
                    FillGridInsuranceAgencyDetails();
                    btnInsuranceUpdate.Text = "Insert";
                    InsuranceAgencyDetailsReset();
                }
                else
                {
                    Show("This insurance Agency  already exists");
                }

                break;
        }
    }

    public void Show(string message)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('" + message + "');", true);
    }

    protected void btnInsuranceReset_Click(object sender, EventArgs e)
    {
        InsuranceAgencyDetailsReset();
    }

    protected void grvInsuranceAgencyDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == null) return;
        switch (e.CommandName)
        {
            case "EditAgency":
                btnInsuranceUpdate.Text = "Update";
                ViewState["InsuranceId"] = e.CommandArgument.ToString();
                var ds = _fleetMaster.GetInsuranceAgenciesByInsuranceId(Convert.ToInt32(ViewState["InsuranceId"]));
                if (ds == null) throw new ArgumentNullException(nameof(ds));
                txtInsuranceAgency.Text = ds.Tables[0].Rows[0]["InsuranceAgency"].ToString();
                txtContactPerson.Text = ds.Tables[0].Rows[0]["ContactPerson"].ToString();
                txtAddress.Text = ds.Tables[0].Rows[0]["Address"].ToString();
                txtContactNo.Text = ds.Tables[0].Rows[0]["ContactNumber"].ToString();
                break;
            case "DeleteAgency":
                _fleetMaster.DeleteInsuranceAgency(int.Parse(e.CommandArgument.ToString()));
                Show("Insurance Agency Deleted Successfully");
                FillGridInsuranceAgencyDetails();
                InsuranceAgencyDetailsReset();
                break;
        }
    }

    protected void grvInsuranceAgencyDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grvInsuranceAgencyDetails.PageIndex = e.NewPageIndex;
        FillGridInsuranceAgencyDetails();
    }
}