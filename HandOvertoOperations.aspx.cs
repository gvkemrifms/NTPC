using System;
using System.Data;
using System.Web.UI;
using GvkFMSAPP.PL;

public partial class HandOvertoOperations : Page
{
    private readonly GvkFMSAPP.BLL.HandOvertoOperations _handovertooperation = new GvkFMSAPP.BLL.HandOvertoOperations();

    private readonly Helper _helper = new Helper();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Name"] == null) Response.Redirect("Login.aspx");
        if (!IsPostBack)
        {
            var dsPerms = (DataSet) Session["PermissionsDS"];
            if (dsPerms == null) throw new ArgumentNullException(nameof(dsPerms));
            dsPerms.Tables[0].DefaultView.RowFilter = "Url='" + Page.Request.Url.Segments[Page.Request.Url.Segments.Length - 1] + "'";
            var p = new PagePermissions(dsPerms, dsPerms.Tables[0].DefaultView[0]["Url"].ToString(), dsPerms.Tables[0].DefaultView[0]["Title"].ToString());
            btHandover.Attributes.Add("onclick", "return validation()");
            GetVehicleNumber();
            pnlHandOverToOperation.Visible = p.Add;
        }
    }

    public void GetVehicleNumber()
    {
        var ds = _handovertooperation.GetVehicleNumber(); //FMS.BLL.HandOvertoOperations.GetVehicleNumber();
        if (ds != null)
            try
            {
                _helper.FillDropDownHelperMethodWithDataSet(ds, "VehicleNumber", "VehicleID", null, ddlVehicleNo);
            }
            catch (Exception ex)
            {
                _helper.ErrorsEntry(ex);
            }
    }

    protected void btHandover_Click(object sender, EventArgs e)
    {
        _handovertooperation.VehicleID = int.Parse(ddlVehicleNo.SelectedItem.Value);
        _handovertooperation.HandoverTo = txtHandOverto.Text;
        _handovertooperation.HandoverDate = DateTime.Parse(txtHandoverDate.Text);
        _handovertooperation.HandoverBy = txtHandOverBy.Text;
        _handovertooperation.QualityInspectionNo = txtQualityInspectionNo.Text;
        _handovertooperation.HTOInspectionDate = DateTime.Parse(txtInspectionDate.Text);
        _handovertooperation.HTOInspectionBy = txtInspectedBy.Text;
        _handovertooperation.Remarks = txtRemarks.Text;
        var ret = _handovertooperation.InsHandoverToOperations();
        Show(ret == 1 ? "Vehicle Handover is Completed" : "Error");
        ClearControls();
        GetVehicleNumber();
    }

    protected void btReset_Click(object sender, EventArgs e)
    {
        ClearControls();
    }

    public void Show(string message)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('" + message + "');", true);
    }

    protected void ClearControls()
    {
        ddlVehicleNo.Items[0].Selected = true;
        txtHandOverto.Text = "";
        txtHandOverBy.Text = "";
        txtHandoverDate.Text = "";
        txtInspectedBy.Text = "";
        txtInspectionDate.Text = "";
        txtQualityInspectionNo.Text = "";
        txtRemarks.Text = "";
    }

    protected void ddlVehicleNo_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
}