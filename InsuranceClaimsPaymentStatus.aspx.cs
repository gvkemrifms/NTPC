using System;
using System.Drawing;
using System.Globalization;
using System.Web.UI;

public partial class InsuranceClaimsPaymentStatus : Page
{
    private readonly GvkFMSAPP.BLL.StatutoryCompliance.InsuranceClaims.InsuranceClaimsPaymentStatus _vehinsclaimpend = new GvkFMSAPP.BLL.StatutoryCompliance.InsuranceClaims.InsuranceClaimsPaymentStatus();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Name"] == null) Response.Redirect("Login.aspx");
        if (!IsPostBack)
        {
            var vehsts = Session["VehicleStatus"].ToString();
            btSave.Attributes.Add("onclick", "return validation()");
            _vehinsclaimpend.VehicleID = Convert.ToInt32(Session["VehicleID"].ToString());
            _vehinsclaimpend.VehicleNumber = Session["VehicleNumber"].ToString();
            switch (vehsts)
            {
                case "Pending":
                    GetAccidentDetails();
                    break;
                case "Received":
                    txtSpotSurveyor.ReadOnly = true;
                    txtSpotSurveyor.BackColor = Color.DarkGray;
                    txtFinalSurveyor.ReadOnly = true;
                    txtFinalSurveyor.BackColor = Color.DarkGray;
                    txtReinspectionSurveyor.ReadOnly = true;
                    txtReinspectionSurveyor.BackColor = Color.DarkGray;
                    txtClaimFormSubmissionDate.ReadOnly = true;
                    txtClaimFormSubmissionDate.BackColor = Color.DarkGray;
                    txtTotalCostOfRepairs.ReadOnly = true;
                    txtTotalCostOfRepairs.BackColor = Color.DarkGray;
                    txtSurveyorAssessmentValue.ReadOnly = true;
                    txtSurveyorAssessmentValue.BackColor = Color.DarkGray;
                    txtBillSubmissionDate.ReadOnly = true;
                    txtBillSubmissionDate.BackColor = Color.DarkGray;
                    txtRemarks.ReadOnly = true;
                    txtRemarks.BackColor = Color.DarkGray;
                    txtPaymentReceivedDate.ReadOnly = true;
                    txtPaymentReceivedDate.BackColor = Color.DarkGray;
                    txtChequeNo.ReadOnly = true;
                    txtChequeNo.BackColor = Color.DarkGray;
                    txtAmountReceivedFromInsurance.ReadOnly = true;
                    txtAmountReceivedFromInsurance.BackColor = Color.DarkGray;
                    txtCostToCompany.ReadOnly = true;
                    txtCostToCompany.BackColor = Color.DarkGray;
                    GetInsuranceClaimsDetailsReceived();
                    btSave.Enabled = false;
                    break;
                default:
                    txtPaymentReceivedDate.Visible = false;
                    txtChequeNo.Visible = false;
                    txtAmountReceivedFromInsurance.Visible = false;
                    lblAmountReceivedFromInsurance.Visible = false;
                    lblChequeNo.Visible = false;
                    lblPaymentReceivedDate.Visible = false;
                    lblChkNo.Visible = false;
                    lblamtrec.Visible = false;
                    lblpaymntrecdt.Visible = false;
                    GetInsuranceClaimsDetailsRejected();
                    btSave.Enabled = false;
                    chbxCloseClaim.Visible = true;
                    break;
            }

            GetInsuranceDetails();
        }
    }

    public void GetAccidentDetails()
    {
        var ds = _vehinsclaimpend.GetAccidentDetails();
        if (ds == null) throw new ArgumentNullException(nameof(ds));
        txtVehicleNumber.Text = ds.Tables[0].Rows[0]["VehicleNumber"].ToString();
        txtAccidentTitle.Text = ds.Tables[0].Rows[0]["AccidentTitle"].ToString();
        txtAccidentDateTime.Text = ds.Tables[0].Rows[0]["AccidentDateTime"].ToString();
    }

    public void GetInsuranceDetails()
    {
        var ds = _vehinsclaimpend.GetInsuranceDetails();
        if (ds == null) throw new ArgumentNullException(nameof(ds));
        txtPolicyNumber.Text = ds.Tables[0].Rows[0]["InsurancePolicyNo"].ToString();
        txtAgency.Text = ds.Tables[0].Rows[0]["InsuranceAgency"].ToString();
        txtInsuranceStartDate.Text = ds.Tables[0].Rows[0]["PolicyStartDate"].ToString();
        txtInsuranceEndDate.Text = ds.Tables[0].Rows[0]["PolicyEndDate"].ToString();
        txtDateInsurance.Text = ds.Tables[0].Rows[0]["PolicyValidityPeriod"].ToString();
    }

    public void GetInsuranceClaimsDetailsReceived()
    {
        var ds = _vehinsclaimpend.GetInsuranceClaimsDetailsReceived();
        if (ds == null) throw new ArgumentNullException(nameof(ds));
        txtVehicleNumber.Text = ds.Tables[0].Rows[0]["VehicleNumber"].ToString();
        txtAccidentTitle.Text = ds.Tables[0].Rows[0]["AccidentTitle"].ToString();
        txtAccidentDateTime.Text = ds.Tables[0].Rows[0]["AccidentDateTime"].ToString();
        txtSpotSurveyor.Text = ds.Tables[0].Rows[0]["SpotSurveyor"].ToString();
        txtFinalSurveyor.Text = ds.Tables[0].Rows[0]["FinalSurveyor"].ToString();
        txtReinspectionSurveyor.Text = ds.Tables[0].Rows[0]["ReInspectionSurveyor"].ToString();
        txtClaimFormSubmissionDate.Text = ds.Tables[0].Rows[0]["ClaimFormSubmissionDate"].ToString();
        txtTotalCostOfRepairs.Text = ds.Tables[0].Rows[0]["TotalCostOfRepairs"].ToString();
        txtSurveyorAssessmentValue.Text = ds.Tables[0].Rows[0]["SurveyorAssessmentValue"].ToString();
        txtBillSubmissionDate.Text = ds.Tables[0].Rows[0]["BillSubmissionDate"].ToString();
        ddlPaymentStatus.Text = ds.Tables[0].Rows[0]["PaymentStatus"].ToString();
        txtRemarks.Text = ds.Tables[0].Rows[0]["Remarks"].ToString();
        txtPaymentReceivedDate.Text = ds.Tables[0].Rows[0]["PaymentReceivedDate"].ToString();
        txtChequeNo.Text = ds.Tables[0].Rows[0]["CheckNo"].ToString();
        txtAmountReceivedFromInsurance.Text = ds.Tables[0].Rows[0]["AmountReceivedFromInsurance"].ToString();
    }

    public void GetInsuranceClaimsDetailsRejected()
    {
        var ds = _vehinsclaimpend.GetInsuranceClaimsDetailsReceived();
        if (ds == null) throw new ArgumentNullException(nameof(ds));
        txtVehicleNumber.Text = ds.Tables[0].Rows[0]["VehicleNumber"].ToString();
        txtAccidentTitle.Text = ds.Tables[0].Rows[0]["AccidentTitle"].ToString();
        txtAccidentDateTime.Text = ds.Tables[0].Rows[0]["AccidentDateTime"].ToString();
        txtSpotSurveyor.Text = ds.Tables[0].Rows[0]["SpotSurveyor"].ToString();
        txtFinalSurveyor.Text = ds.Tables[0].Rows[0]["FinalSurveyor"].ToString();
        txtReinspectionSurveyor.Text = ds.Tables[0].Rows[0]["ReInspectionSurveyor"].ToString();
        txtClaimFormSubmissionDate.Text = ds.Tables[0].Rows[0]["ClaimFormSubmissionDate"].ToString();
        txtTotalCostOfRepairs.Text = ds.Tables[0].Rows[0]["TotalCostOfRepairs"].ToString();
        txtSurveyorAssessmentValue.Text = ds.Tables[0].Rows[0]["SurveyorAssessmentValue"].ToString();
        txtBillSubmissionDate.Text = ds.Tables[0].Rows[0]["BillSubmissionDate"].ToString();
        ddlPaymentStatus.Text = ds.Tables[0].Rows[0]["PaymentStatus"].ToString();
        txtRemarks.Text = ds.Tables[0].Rows[0]["Remarks"].ToString();
    }

    protected void btSave_Click(object sender, EventArgs e)
    {
        _vehinsclaimpend.VehicleNumber = txtVehicleNumber.Text;
        _vehinsclaimpend.AccidentDateTime = DateTime.Parse(txtAccidentDateTime.Text);
        _vehinsclaimpend.AccidentTitle = txtAccidentTitle.Text;
        _vehinsclaimpend.SpotSurveyor = txtSpotSurveyor.Text;
        _vehinsclaimpend.FinalSurveyor = txtFinalSurveyor.Text;
        _vehinsclaimpend.ReInspectionSurveyor = txtReinspectionSurveyor.Text;
        _vehinsclaimpend.InsurancePolicyNo = txtPolicyNumber.Text;
        _vehinsclaimpend.InsuranceAgency = txtAgency.Text;
        _vehinsclaimpend.InsuranceStartDate = DateTime.Parse(txtInsuranceStartDate.Text);
        _vehinsclaimpend.InsuranceEndDate = DateTime.Parse(txtInsuranceEndDate.Text);
        _vehinsclaimpend.DateInsurance = txtDateInsurance.Text;
        _vehinsclaimpend.ClaimFormSubmissionDate = DateTime.Parse(txtClaimFormSubmissionDate.Text);
        _vehinsclaimpend.TotalCostOfRepairs = float.Parse(txtTotalCostOfRepairs.Text);
        _vehinsclaimpend.SurveyorAssessmentValue = float.Parse(txtSurveyorAssessmentValue.Text);
        _vehinsclaimpend.BillSubmissionDate = DateTime.Parse(txtBillSubmissionDate.Text);
        _vehinsclaimpend.PaymentStatus = ddlPaymentStatus.SelectedItem.Text;
        _vehinsclaimpend.Remarks = txtRemarks.Text;
        _vehinsclaimpend.PaymentReceivedDate = DateTime.Parse(txtPaymentReceivedDate.Text);
        _vehinsclaimpend.CheckNo = txtChequeNo.Text;
        _vehinsclaimpend.AmountReceivedFromInsurance = float.Parse(txtAmountReceivedFromInsurance.Text);
        _vehinsclaimpend.CostToCompany = float.Parse(txtCostToCompany.Text);
        var ret = _vehinsclaimpend.InsInsuranceClaimsPaymentStatusPending();
        _vehinsclaimpend.InsInsuranceClaimDet();
        Show(ret == 1 ? "Record Inserted Successfully" : "Error");
    }

    protected void ddlPaymentStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        switch (ddlPaymentStatus.SelectedItem.Value)
        {
            case "0":
                txtPaymentReceivedDate.Visible = true;
                txtChequeNo.Visible = true;
                txtAmountReceivedFromInsurance.Visible = true;
                lblAmountReceivedFromInsurance.Visible = true;
                lblChequeNo.Visible = true;
                lblPaymentReceivedDate.Visible = true;
                lblChkNo.Visible = true;
                lblamtrec.Visible = true;
                lblpaymntrecdt.Visible = true;
                txtPaymentReceivedDate.ReadOnly = false;
                txtPaymentReceivedDate.BackColor = Color.Empty;
                txtChequeNo.ReadOnly = false;
                txtChequeNo.BackColor = Color.Empty;
                txtAmountReceivedFromInsurance.ReadOnly = false;
                txtAmountReceivedFromInsurance.BackColor = Color.Empty;
                break;
            case "1":
                txtPaymentReceivedDate.Visible = false;
                txtChequeNo.Visible = false;
                txtAmountReceivedFromInsurance.Visible = false;
                lblAmountReceivedFromInsurance.Visible = false;
                lblChequeNo.Visible = false;
                lblPaymentReceivedDate.Visible = false;
                lblChkNo.Visible = false;
                lblamtrec.Visible = false;
                lblpaymntrecdt.Visible = false;
                txtCostToCompany.Text = float.Parse(txtTotalCostOfRepairs.Text).ToString(CultureInfo.InvariantCulture);
                break;
            default:
                txtPaymentReceivedDate.Visible = true;
                txtChequeNo.Visible = true;
                txtAmountReceivedFromInsurance.Visible = true;
                lblAmountReceivedFromInsurance.Visible = true;
                lblChequeNo.Visible = true;
                lblPaymentReceivedDate.Visible = true;
                lblChkNo.Visible = true;
                lblamtrec.Visible = true;
                lblpaymntrecdt.Visible = true;
                txtPaymentReceivedDate.ReadOnly = true;
                txtPaymentReceivedDate.BackColor = Color.DarkGray;
                txtChequeNo.ReadOnly = true;
                txtChequeNo.BackColor = Color.DarkGray;
                txtAmountReceivedFromInsurance.ReadOnly = true;
                txtAmountReceivedFromInsurance.BackColor = Color.DarkGray;
                break;
        }
    }

    protected void txtAmountReceivedFromInsurance_TextChanged(object sender, EventArgs e)
    {
        switch (ddlPaymentStatus.SelectedItem.Value)
        {
            case "0":
                txtCostToCompany.Text = (float.Parse(txtTotalCostOfRepairs.Text) - float.Parse(txtAmountReceivedFromInsurance.Text)).ToString(CultureInfo.InvariantCulture);
                break;
        }
    }

    protected void btReset_Click(object sender, EventArgs e)
    {
        // ClearControls();
    }

    public void Show(string message)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('" + message + "');", true);
    }
}