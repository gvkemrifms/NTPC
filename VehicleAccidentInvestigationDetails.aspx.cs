using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using GvkFMSAPP.BLL;
using GvkFMSAPP.DLL;
using FMSGeneral = GvkFMSAPP.BLL.FMSGeneral;

public partial class VehicleAccidentInvestigationDetails : Page
{
    private readonly FMSGeneral _fmsg = new FMSGeneral();
    private readonly Helper _helper = new Helper();
    private VehicleAccidentDetailsBLL _vehicleAccidentDetail;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Name"] == null) Response.Redirect("Login.aspx");
        if (!IsPostBack)
        {
            GetVehicleNumber();
            txtVehNum.Visible = false;
        }
    }

    public void GetVehicleNumber()
    {
        try
        {
            var ds = _fmsg.GetVehicleNumberInsurance();
            if (ds == null) throw new ArgumentNullException(nameof(ds));
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count != 0)
            {
                _helper.FillDropDownHelperMethodWithDataSet(ds, "VehicleNumber", "VehicleID", null, ddlistVehicleNumber);
                ViewState["dsVehicles"] = ds;
            }

            if (ds.Tables[1] != null && ds.Tables[1].Rows.Count != 0)
            {
                gvVehicleDetails.DataSource = ds.Tables[1];
                gvVehicleDetails.DataBind();
            }
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    protected void ddlistVehicleNumber_SelectedIndexChanged(object sender, EventArgs e)
    {
        var ds = _fmsg.GetInsuranceAccVehicle(Convert.ToString(ddlistVehicleNumber.SelectedItem.Text), Convert.ToInt16(ddlistVehicleNumber.SelectedValue));
        if (ds.Tables[0].Rows.Count > 0)
        {
            TxtPolicyNumber.Text = ds.Tables[0].Rows[0]["InsurancePolicyNo"].ToString();
            TxtAgency.Text = ds.Tables[0].Rows[0]["InsuranceAgency"].ToString();
            TxtInsuranceStartDate.Text = ds.Tables[0].Rows[0]["PolicyStartDate"].ToString();
            TxtInsuranceEndDate.Text = ds.Tables[0].Rows[0]["CurrentPolicyEndDate"].ToString();
        }

        if (ds.Tables[1].Rows.Count > 0)
        {
            TxtAccidentTitle.Text = ds.Tables[1].Rows[0]["IncidentTitle"].ToString();
            TxtAccidentDateTime.Text = ds.Tables[1].Rows[0]["AccidentDateTime"].ToString();
        }
    }

    protected void BtnSave_Click(object sender, EventArgs e)
    {
        int iReturn;
        _vehicleAccidentDetail = new VehicleAccidentDetailsBLL();
        _vehicleAccidentDetail = GetVehicelDetails();
        switch (BtnSave.Text)
        {
            case "Update":
                _vehicleAccidentDetail.HdnId = int.Parse(ViewState["hdnvalue"].ToString());
                iReturn = _vehicleAccidentDetail.UpdVehicleAccidentInvestigationDetails();
                switch (iReturn)
                {
                    case 0:
                        Show("Records Not Inserted");
                        ClearAll();
                        break;
                    default:
                        Show("Record Inserted Successfully");
                        GetVehicleNumber();
                        ClearAll();
                        break;
                }

                break;
            default:
                switch (ddlistVehicleNumber.SelectedIndex)
                {
                    case 0:
                        Show("Select Vehicle");
                        break;
                    default:
                        iReturn = _vehicleAccidentDetail.InsertVehicleAccidentInvestigationDetails();
                        switch (iReturn)
                        {
                            case 0:
                                Show("Records Not Inserted");
                                ClearAll();
                                break;
                            default:
                                Show("Record Inserted Successfully");
                                GetVehicleNumber();
                                ClearAll();
                                break;
                        }

                        break;
                }

                break;
        }
    }

    private VehicleAccidentDetailsBLL GetVehicelDetails()
    {
        try
        {
            _vehicleAccidentDetail.VehicleNumber = ddlistVehicleNumber.SelectedIndex != 0 ? Convert.ToString(ddlistVehicleNumber.SelectedItem.Text) : txtVehNum.Text;
            _vehicleAccidentDetail.AccTime = TxtAccidentDateTime.Text == string.Empty ? (DateTime?) null : DateTime.Parse(TxtAccidentDateTime.Text);
            _vehicleAccidentDetail.AccidentDescription = TxtAccidentTitle.Text;
            _vehicleAccidentDetail.SpotSurveyor = TxtSpotSurveyor.Text;
            _vehicleAccidentDetail.SpotSurDate = TxtSpotSurveyorDate.Text == string.Empty ? (DateTime?) null : DateTime.Parse(TxtSpotSurveyorDate.Text);
            _vehicleAccidentDetail.FinalSurveyor = TxtFinalSurveyor.Text;
            _vehicleAccidentDetail.FinalSurDate = TxtFinalSurveyorDate.Text == string.Empty ? (DateTime?) null : DateTime.Parse(TxtFinalSurveyorDate.Text);
            _vehicleAccidentDetail.ReInsSurveyor = TxtReinspectionSurveyor.Text;
            _vehicleAccidentDetail.ReInsSurDate = TxtReinspectionSurveyorDate.Text == string.Empty ? (DateTime?) null : DateTime.Parse(TxtReinspectionSurveyorDate.Text);
            _vehicleAccidentDetail.PolicyNo = TxtPolicyNumber.Text;
            _vehicleAccidentDetail.Agency = TxtAgency.Text;
            _vehicleAccidentDetail.InsStDate = TxtInsuranceStartDate.Text == string.Empty ? (DateTime?) null : DateTime.Parse(TxtInsuranceStartDate.Text);
            _vehicleAccidentDetail.InsEndDate = TxtInsuranceEndDate.Text == string.Empty ? (DateTime?) null : DateTime.Parse(TxtInsuranceEndDate.Text);
            _vehicleAccidentDetail.ClaimDate = TxtClaimFormSubmissionDate.Text == string.Empty ? (DateTime?) null : DateTime.Parse(TxtClaimFormSubmissionDate.Text);
            _vehicleAccidentDetail.CostRepairs = TxtTotalCostofRepairs.Text == string.Empty ? (float?) null : float.Parse(TxtTotalCostofRepairs.Text);
            _vehicleAccidentDetail.AssValue = TxtSurveyorAssessmentValue.Text == string.Empty ? (float?) null : float.Parse(TxtSurveyorAssessmentValue.Text);
            _vehicleAccidentDetail.BillDate = TxtBillSubmissionDate.Text == string.Empty ? (DateTime?) null : DateTime.Parse(TxtBillSubmissionDate.Text);
            _vehicleAccidentDetail.PayStatus = Convert.ToString(ddlistPaymentStatus.SelectedItem.Value);
            _vehicleAccidentDetail.Remarks = txtRemarks.Text;
            _vehicleAccidentDetail.PayDate = TxtPaymentRecievedDate.Text == string.Empty ? (DateTime?) null : DateTime.Parse(TxtPaymentRecievedDate.Text);
            _vehicleAccidentDetail.Cheque = TxtChequeNo.Text;
            _vehicleAccidentDetail.AmtRecieved = TxtAmountRecievedFromInsurance.Text == string.Empty ? (float?) null : float.Parse(TxtAmountRecievedFromInsurance.Text);
            _vehicleAccidentDetail.Cost = TxtCostToCompany.Text == string.Empty ? (float?) null : float.Parse(TxtCostToCompany.Text);
            return _vehicleAccidentDetail;
        }
        catch (Exception ex)
        {
            ErrorHandler.ErrorsEntry(ex.GetBaseException().ToString(), "class: AttachDocuments;Method: BtnSave_Click()-VehicleAccidentInvestigationDetails", 0);
            return _vehicleAccidentDetail;
        }
    }

    public void Show(string message)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('" + message + "');", true);
    }

    protected void gvVehicleDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "_Details":
                BtnSave.Text = "Update";
                ddlistVehicleNumber.Visible = false;
                txtVehNum.Visible = true;
                txtVehNum.Enabled = false;
                var row = (GridViewRow) ((WebControl) e.CommandSource).Parent.Parent;
                var hdnfield = (HiddenField) row.FindControl("hdnvehicelvalue");
                var accdetails = new VehicleAccidentDetailsBLL();
                var ds = accdetails.GetaccvehicelDetails(int.Parse(hdnfield.Value));
                ViewState["hdnvalue"] = int.Parse(hdnfield.Value);
                txtVehNum.Text = Convert.ToString(ds.Tables[1].Rows[0]["VehicleNumber"].ToString());
                TxtAccidentDateTime.Text = Convert.ToString(ds.Tables[0].Rows[0]["AccidentDateTime"].ToString());
                TxtAccidentTitle.Text = Convert.ToString(ds.Tables[0].Rows[0]["AccidentTitle"].ToString());
                TxtSpotSurveyor.Text = Convert.ToString(ds.Tables[0].Rows[0]["SpotSurveyor"].ToString());
                TxtSpotSurveyorDate.Text = Convert.ToString(ds.Tables[0].Rows[0]["SpotSurDate"].ToString());
                TxtFinalSurveyor.Text = Convert.ToString(ds.Tables[0].Rows[0]["FinalSurveyor"].ToString());
                TxtFinalSurveyorDate.Text = Convert.ToString(ds.Tables[0].Rows[0]["FinalSurDate"].ToString());
                TxtReinspectionSurveyor.Text = Convert.ToString(ds.Tables[0].Rows[0]["ReInsSurveyor"].ToString());
                TxtReinspectionSurveyorDate.Text = Convert.ToString(ds.Tables[0].Rows[0]["ReInsSurDate"].ToString());
                TxtPolicyNumber.Text = Convert.ToString(ds.Tables[0].Rows[0]["PolicyNo"].ToString());
                TxtAgency.Text = Convert.ToString(ds.Tables[0].Rows[0]["Agency"].ToString());
                TxtInsuranceStartDate.Text = Convert.ToString(ds.Tables[0].Rows[0]["InsStDate"].ToString());
                TxtInsuranceEndDate.Text = Convert.ToString(ds.Tables[0].Rows[0]["InsEndDate"].ToString());
                TxtClaimFormSubmissionDate.Text = Convert.ToString(ds.Tables[0].Rows[0]["ClaimDate"].ToString());
                TxtTotalCostofRepairs.Text = Convert.ToString(ds.Tables[0].Rows[0]["CostRepairs"].ToString());
                TxtSurveyorAssessmentValue.Text = Convert.ToString(ds.Tables[0].Rows[0]["AssValue"].ToString());
                TxtBillSubmissionDate.Text = Convert.ToString(ds.Tables[0].Rows[0]["BillSubmissionDate"].ToString());
                ddlistPaymentStatus.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["PayStatus"].ToString());
                txtRemarks.Text = Convert.ToString(ds.Tables[0].Rows[0]["Remarks"].ToString());
                TxtPaymentRecievedDate.Text = Convert.ToString(ds.Tables[0].Rows[0]["PayDate"].ToString());
                TxtChequeNo.Text = Convert.ToString(ds.Tables[0].Rows[0]["Cheque"].ToString());
                TxtAmountRecievedFromInsurance.Text = Convert.ToString(ds.Tables[0].Rows[0]["AmtRecieved"].ToString());
                TxtCostToCompany.Text = Convert.ToString(ds.Tables[0].Rows[0]["Cost"].ToString());
                break;
        }
    }

    protected void BtnReset_Click(object sender, EventArgs e)
    {
        ClearAll();
    }

    private void ClearAll()
    {
        txtVehNum.Visible = false;
        txtVehNum.Text = "";
        ddlistVehicleNumber.Visible = true;
        TxtAccidentDateTime.Text = "";
        TxtAccidentTitle.Text = "";
        TxtSpotSurveyor.Text = "";
        TxtSpotSurveyorDate.Text = "";
        TxtFinalSurveyor.Text = "";
        TxtFinalSurveyorDate.Text = "";
        TxtReinspectionSurveyor.Text = "";
        TxtReinspectionSurveyorDate.Text = "";
        TxtPolicyNumber.Text = "";
        TxtAgency.Text = "";
        TxtInsuranceStartDate.Text = "";
        TxtInsuranceEndDate.Text = "";
        TxtClaimFormSubmissionDate.Text = "";
        TxtTotalCostofRepairs.Text = "";
        TxtSurveyorAssessmentValue.Text = "";
        TxtBillSubmissionDate.Text = "";
        ddlistPaymentStatus.SelectedIndex = 0;
        txtRemarks.Text = "";
        TxtPaymentRecievedDate.Text = "";
        TxtChequeNo.Text = "";
        TxtAmountRecievedFromInsurance.Text = "";
        TxtCostToCompany.Text = "";
        BtnSave.Text = "Save";
    }
}