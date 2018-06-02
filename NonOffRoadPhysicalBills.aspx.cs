using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class NonOffRoadPhysicalBills : Page
{
    private readonly Helper _helper = new Helper();
    DataSet _dsVehicle;
    DataSet _dsDistricts, _dsBillNo;
    readonly GvkFMSAPP.BLL.BaseVehicleDetails _fmsobj = new GvkFMSAPP.BLL.BaseVehicleDetails();
    readonly GvkFMSAPP.BLL.VAS_BLL.VASGeneral _obj = new GvkFMSAPP.BLL.VAS_BLL.VASGeneral();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Name"] == null) Response.Redirect("Login.aspx");
        if (!IsPostBack)
        {
            ddlVehicleno.Enabled = false;
            ddlBillNo.Enabled = false;
            BindData();
            BindGridView();
        }
    }

    public void BindData()
    {
        try
        {
            _dsDistricts = new DataSet();
            _dsDistricts = _fmsobj.GetDistrict();
            _helper.FillDropDownHelperMethodWithDataSet(_dsDistricts, "ds_lname", "ds_dsid", ddlDistricts);
            ViewState["dsDistricts"] = _dsDistricts;
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    protected void ddlDistricts_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            switch (ddlDistricts.SelectedIndex)
            {
                case 0:
                    ddlVehicleno.Items.Clear();
                    ddlVehicleno.Enabled = false;
                    ddlBillNo.Items.Clear();
                    ddlBillNo.Enabled = false;
                    txtBillAmount.Text = "";
                    break;
                default:
                    _dsVehicle = new DataSet();
                    ddlVehicleno.Enabled = true;
                    _obj.DistrictId = Convert.ToInt16(ddlDistricts.SelectedValue);
                    _dsVehicle = _obj.GetNonOffVehFoBilling();
                    _helper.FillDropDownHelperMethodWithDataSet(_dsVehicle, "Vehicleno", "", ddlVehicleno);
                    ViewState["dsVehicle"] = _dsVehicle;
                    break;
            }
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    protected void ddlVehicleno_SelectedIndexChanged1(object sender, EventArgs e)
    {
        try
        {
            switch (ddlVehicleno.SelectedIndex)
            {
                case 0:
                    ddlBillNo.Items.Clear();
                    ddlBillNo.Enabled = false;
                    txtBillAmount.Text = "";
                    break;
                default:
                    _dsBillNo = new DataSet();
                    ddlBillNo.Enabled = true;
                    _obj.SrcVehNo = ddlVehicleno.SelectedItem.ToString();
                    _dsBillNo = _obj.GetBillNo();
                    _helper.FillDropDownHelperMethodWithDataSet(_dsBillNo, "Billno", "", ddlBillNo);
                    ViewState["dsBillNo"] = _dsBillNo;
                    break;
            }
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (_obj != null)
            {
                _obj.District = ddlDistricts.SelectedItem.ToString();
                _obj.SrcVehNo = ddlVehicleno.SelectedItem.ToString();
                _obj.NonOffBillNo = ddlBillNo.SelectedItem.ToString();
                _obj.ReceiptDate = Convert.ToDateTime(txtReceiptDate.Text);
                _obj.CourierName = txtCourierName.Text;
                _obj.DocketNo = txtDocketNo.Text;
                _obj.NonOffAmount = txtBillAmount.Text;
                if (lblBrkdwn.Text != "")
                {
                    _obj.MandalId = int.Parse(lblBrkdwn.Text);
                    if (HiddenField1 != null) _obj.VenName = HiddenField1.Value;
                    int i = _obj.InsertNonoffroadPhysicalBills();
                    switch (i)
                    {
                        case 0:
                            Show("Records not inserted successfully");
                            break;
                        default:
                            Show("Records inserted successfully");
                            BindGridView();
                            ddlDistricts.SelectedIndex = 0;
                            ddlVehicleno.Items.Clear();
                            ddlBillNo.Items.Clear();
                            txtReceiptDate.Text = "";
                            txtCourierName.Text = "";
                            txtDocketNo.Text = "";
                            txtBillAmount.Text = "";
                            lblBrkdwn.Text = "";
                            break;
                    }
                }
                else
                {
                    Show("Breakdown value should not be null.Record not saved");
                }
            }
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }

    
}
public void BindGridView()
    {
        var ds = _obj.GetNonOffPhysicalBills();
        if (ds == null) throw new ArgumentNullException(nameof(ds));
        ViewState["VehiclePhyBill"] = ds.Tables[0];
        gvVehiclePhysicalBillDetails.DataSource = ds.Tables[0];
        gvVehiclePhysicalBillDetails.DataBind();
    }

    public void Show(string message)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('" + message + "');", true);
    }

    protected void gvVehiclePhysicalBillDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvVehiclePhysicalBillDetails.PageIndex = e.NewPageIndex;
        BindGridView();
    }

    protected void gvVehiclePhysicalBillDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToUpper() == "VEHMAINEDIT")
        {
            GridViewRow row = (GridViewRow) ((WebControl) e.CommandSource).Parent.Parent;
            btnSave.Visible = false;
            btnUpdate.Visible = true;
            DateTime dt = Convert.ToDateTime(((Label) ((row.FindControl("lblReceiptDate")))).Text);
            txtReceiptDate.Text = dt.ToString("dd/MM/yyyy");
            txtCourierName.Text = ((Label) row.FindControl("lblCourier_Name")).Text;
            txtDocketNo.Text = ((Label) row.FindControl("lblDocketNo")).Text;
            lblBrkdwn.Text = ((Label) row.FindControl("lblBrkDwnID")).Text;
            DataSet dsDist = (DataSet) ViewState["dsDistricts"];
            DataView dvDistrict = dsDist.Tables[0].DefaultView;
            dvDistrict.RowFilter = "ds_lname='" + ((Label) ((row.FindControl("lblDistrict")))).Text + "'";
            ddlDistricts.SelectedValue = Convert.ToString(dvDistrict.ToTable().Rows[0]["ds_dsid"]);
            ddlDistricts.Enabled = false;
            ddlDistricts_SelectedIndexChanged(this, null);
            ddlVehicleno.Items.Insert(1, new ListItem(((Label) row.FindControl("lblVehicle_No")).Text, "1"));
            ddlVehicleno.SelectedIndex = 1;
            ddlVehicleno.Enabled = false;
            ddlVehicleno_SelectedIndexChanged1(this, null);
            ddlBillNo.Items.Insert(1, new ListItem(((Label) row.FindControl("lblBillNo")).Text, "1"));
            ddlBillNo.SelectedIndex = 1;
            ddlBillNo.Enabled = false;
            txtBillAmount.Text = ((Label) row.FindControl("lblBillAmount")).Text;
        }
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        _obj.District = ddlDistricts.SelectedItem.ToString();
        _obj.SrcVehNo = ddlVehicleno.SelectedItem.ToString();
        _obj.NonOffBillNo = ddlBillNo.SelectedItem.ToString();
        _obj.ReceiptDate = Convert.ToDateTime(txtReceiptDate.Text);
        _obj.CourierName = txtCourierName.Text;
        _obj.DocketNo = txtDocketNo.Text;
        _obj.NonOffAmount = txtBillAmount.Text;
        int i = _obj.UpdateNonoffroadPhysicalBills();
        if (i != 0)
        {
            Show("Records Updated successfully");
            BindGridView();
            ddlDistricts.SelectedIndex = 0;
            ddlDistricts.Enabled = true;
            ddlVehicleno.Items.Clear();
            ddlVehicleno.Enabled = true;
            ddlBillNo.Items.Clear();
            ddlBillNo.Enabled = true;
            txtReceiptDate.Text = "";
            txtCourierName.Text = "";
            txtDocketNo.Text = "";
            btnSave.Visible = true;
            btnUpdate.Visible = false;
            txtBillAmount.Text = "";
            lblBrkdwn.Text = "";
        }
        else
            Show("Records not Updated successfully");
    }

    protected void ddlBillNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlBillNo.SelectedIndex != 0)
        {
            _obj.District = ddlDistricts.SelectedItem.ToString();
            _obj.VehNumforNonOff = ddlVehicleno.SelectedItem.ToString();
            _obj.NonOffBillNo = ddlBillNo.SelectedItem.ToString();
            DataSet ds = _obj.GetNonOffroadBillAmt();
            if (ds == null) throw new ArgumentNullException(nameof(ds));
if(ds.Tables[0].Rows.Count>0)
            txtBillAmount.Text = ds.Tables[0].Rows[0][0].ToString();
            if (ds.Tables[1].Rows.Count > 0)
            {
                lblBrkdwn.Text = ds.Tables[1].Rows[0][0].ToString();
                HiddenField1.Value = ds.Tables[1].Rows[0][1].ToString();
            }
        }
        else
            txtBillAmount.Text = "";
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        ddlDistricts.SelectedIndex = 0;
        ddlDistricts_SelectedIndexChanged(this, null);
        ddlVehicleno.Items.Clear();
        ddlVehicleno.Enabled = true;
        ddlBillNo.Items.Clear();
        ddlBillNo.Enabled = true;
        txtReceiptDate.Text = "";
        txtCourierName.Text = "";
        txtDocketNo.Text = "";
        btnSave.Visible = true;
        btnUpdate.Visible = false;
        txtBillAmount.Text = "";
        lblBrkdwn.Text = "";
    }
}