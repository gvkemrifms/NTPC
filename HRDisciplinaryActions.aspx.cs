using System;
using System.Web.UI;
using GvkFMSAPP.BLL;

public partial class HrDisciplinaryActions : Page
{
    private readonly VAS _obj = new VAS();
    readonly Helper _helper = new Helper();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Name"] == null) Response.Redirect("Login.aspx");
        if (!IsPostBack)
        {
            BindData();
            GetVehicleNumbers();
        }
    }

    private void GetVehicleNumbers()
    {
        try
        {
            var ds = _obj.GetHRDiscVeh();
            if (ds != null) _helper.FillDropDownHelperMethodWithDataSet(ds, "VehicleNumber", "VehicleID", ddlVehicleno);
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    private void BindData()
    {
        var ds = _obj.GetHRDiscDropDown();
        if (ds != null && ds.Tables.Count > 0)
        {
            try
            {
                _helper.FillDifferentDataTables(ddlFatalAcc, ds.Tables[0], "FA_Details", "FA_ID");
                _helper.FillDifferentDataTables(ddlMajor, ds.Tables[1], "MajA_Details", "MajA_ID");
                _helper.FillDifferentDataTables(ddlMajorOrtotLoss, ds.Tables[2], "MajlossA_Details", "MajlossA_ID");
                _helper.FillDifferentDataTables(ddlMinor, ds.Tables[3], "MA_Details", "MA_ID");
                _helper.FillDifferentDataTables(ddlSevereInj, ds.Tables[4], "SevInj_Details", "SevInj_ID");
                _helper.FillDifferentDataTables(ddlSitIfAction, ds.Tables[5], "SituationOfAccident", "AccidentId");
            }
            catch (Exception ex)
            {
                _helper.ErrorsEntry(ex);
            }
        }
    }

    protected void ddlSitIfAction_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlSitIfAction == null) return;
        try
        {
            switch (ddlSitIfAction.SelectedIndex)
            {
                case 0:
                    ddlCause.Items.Clear();
                    ddlCause.Enabled = false;
                    break;
                default:
                    ddlCause.Enabled = true;
                    var x = ddlSitIfAction.SelectedIndex;
                    var ds = _obj.GetCausesforAcc(x);
                    if (ds == null) throw new ArgumentNullException(nameof(ds));
                    if (ds.Tables[0].Rows.Count>0)
                        ds.Tables[0].Rows[0].Delete();
                    _helper.FillDropDownHelperMethodWithDataSet(ds, "CauseOfAccident", "CauseId", ddlCause);
                    break;
            }
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        ddlSitIfAction.SelectedIndex = 0;
        ddlSevereInj.SelectedIndex = 0;
        ddlMajorOrtotLoss.SelectedIndex = 0;
        ddlFatalAcc.SelectedIndex = 0;
        ddlMajor.SelectedIndex = 0;
        ddlMinor.SelectedIndex = 0;
        ddlCause.SelectedIndex = 0;
        ddlVehicleno.SelectedIndex = 0;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (ddlVehicleno != null) _obj.VehicleNumber = Convert.ToString(ddlVehicleno.SelectedItem.Text);
        if (ddlSitIfAction != null) _obj.sitOfAcc = ddlSitIfAction.SelectedItem.ToString();
        if (ddlSitIfAction != null && ddlSitIfAction.SelectedIndex>0 && ddlCause != null) _obj.CauseAcc = ddlCause.SelectedItem.ToString();
        if (ddlMajor != null) _obj.majACC = ddlMajor.SelectedItem.ToString();
        if (ddlMinor != null) _obj.minAcc = ddlMinor.SelectedItem.ToString();
        if (ddlMajorOrtotLoss != null) _obj.majLoss = ddlMajorOrtotLoss.SelectedItem.ToString();
        if (ddlSevereInj != null) _obj.sevInj = ddlSevereInj.SelectedItem.ToString();
        if (ddlFatalAcc != null) _obj.fatalAcc = ddlFatalAcc.SelectedItem.ToString();
        if (_obj == null) return;
        var i = _obj.InsertHRDisciplinaryActions();
        switch (i)
        {
            case 0:
                Show("Insertion Failed");
                break;
            default:
                Show("Disciplinary Action Successfully inserted");
                btnClear_Click(this, null);
                break;
        }
    }

    public void Show(string message)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('" + message + "');", true);
    }
}