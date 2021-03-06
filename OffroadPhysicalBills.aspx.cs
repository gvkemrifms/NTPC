﻿using System;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using GvkFMSAPP.BLL.VAS_BLL;
public partial class OffroadPhysicalBills : Page
{
    private readonly Helper _helper = new Helper();
    private readonly VASGeneral _obj = new VASGeneral();
    private DataSet _dsBillNo = new DataSet();
    private DataSet _dsVehNo = new DataSet();

    public string UserId{ get;private set; }

    protected void Page_Load(object sender,EventArgs e)
    {
        if (Session["User_Id"] == null)
            Response.Redirect("Login.aspx");
        else
            UserId = (string) Session["User_Id"];
        if (!IsPostBack)
        {
            ddlVehicleNo.Enabled = false;
            ddlBillNo.Enabled = false;
            btnUpdate.Visible = false;
            BindDropDown();
            BindGrid();
        }
    }

    public void BindDropDown()
    {
        try
        {
            var dsStates = new DataSet();
            var query = "SELECT d.district_id as ds_dsid,d.district_name as ds_lname from [m_district] d join m_users u on d.district_id=u.stateId where u.UserId='" + UserId + "' order by d.district_name";
            var dt = _helper.ExecuteSelectStmt(query);
            dsStates.Tables.Add(dt);
            _helper.FillDropDownHelperMethod(query,"ds_lname","ds_dsid",ddlDistricts);
            ViewState["dsDistricts"] = dsStates;
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    protected void ddlDistricts_SelectedIndexChanged(object sender,EventArgs e)
    {
        switch (ddlDistricts.SelectedIndex)
        {
            case 0:
                lblBreakdwn.Text = "";
                txtBillAmount.Text = "";
                txtDownTime.Text = "";
                txtUpTime.Text = "";
                break;
            default:
                ddlVehicleNo.Enabled = true;
                _obj.District = ddlDistricts.SelectedItem.ToString();
                _dsVehNo = _obj.GetOffRoadVehiclesforBilling();
                ddlVehicleNo.DataSource = _dsVehNo.Tables[0];
                ddlVehicleNo.DataTextField = "OffRoadVehicle_No";
                ddlVehicleNo.DataBind();
                ddlVehicleNo.Items.Insert(0,new ListItem("--Select--","0"));
                break;
        }
    }

    protected void ddlBillNo_SelectedIndexChanged(object sender,EventArgs e)
    {
        switch (ddlBillNo.SelectedIndex)
        {
            case 0:
                lblBreakdwn.Text = "";
                txtBillAmount.Text = "";
                txtDownTime.Text = "";
                txtUpTime.Text = "";
                break;
            default:
                _obj.District = ddlDistricts.SelectedItem.ToString();
                _obj.VehNumforNonOff = ddlVehicleNo.SelectedItem.ToString();
                _obj.NonOffBillNo = ddlBillNo.SelectedItem.ToString();
                _obj.BillDetailsID = ddlBillNo.SelectedValue;
                var ds = _obj.GetOffroadVehDet();
                if (ds == null) return;
                lblBreakdwn.Text = ds.Tables[0].Rows[0][0].ToString();
                txtBillAmount.Text = ds.Tables[0].Rows[0][1].ToString();
                txtDownTime.Text = ds.Tables[0].Rows[0][2].ToString();
                txtUpTime.Text = ds.Tables[0].Rows[0][3].ToString();
                HiddenField1.Value = ds.Tables[0].Rows[0][4].ToString();
                break;
        }
    }

    public void Show(string message)
    {
        ScriptManager.RegisterStartupScript(this,GetType(),"msg","alert('" + message + "');",true);
    }

    protected void ddlVehicleNo_SelectedIndexChanged(object sender,EventArgs e)
    {
        lblBreakdwn.Text = "";
        txtBillAmount.Text = "";
        txtDownTime.Text = "";
        txtUpTime.Text = "";
        if (ddlVehicleNo.SelectedIndex != 0)
        {
            ddlBillNo.Enabled = true;
            _obj.District = ddlDistricts.SelectedItem.ToString();
            _obj.VehNumforNonOff = ddlVehicleNo.SelectedItem.ToString();
            _dsBillNo = _obj.GetBillNoOfOffRoadVehicles();
            ddlBillNo.DataSource = _dsBillNo.Tables[0];
            ddlBillNo.DataTextField = "BillNumber";
            ddlBillNo.DataValueField = "BillDetailsId";
            ddlBillNo.DataBind();
            ddlBillNo.Items.Insert(0,new ListItem("--Select--","0"));
            ViewState["dsBillNo"] = _dsBillNo;
        }
    }

    protected void btnSave_Click(object sender,EventArgs e)
    {
        _obj.District = ddlDistricts.SelectedItem.ToString();
        _obj.VehNumforNonOff = ddlVehicleNo.SelectedItem.ToString();
        _obj.NonOffBillNo = ddlBillNo.SelectedItem.ToString();
        _obj.NonOffAmount = txtBillAmount.Text;
        _obj.OffRoadDate = DateTime.ParseExact(DateTime.Parse(txtDownTime.Text).ToShortDateString(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
        _obj.UpTime = DateTime.ParseExact(txtUpTime.Text,"MM/dd/yyyy",CultureInfo.InvariantCulture);
        _obj.ReceiptDate = DateTime.ParseExact(txtReceiptDate.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
        _obj.CourierName = txtCourierName.Text;
        _obj.DocketNo = txtDocketNo.Text;
        _obj.MandalId = int.Parse(lblBreakdwn.Text);
        _obj.VenName = HiddenField1.Value;
        _obj.BillDetailsID = ddlBillNo.SelectedValue;
        if (_obj.ReceiptDate < _obj.OffRoadDate)
        {
            Show("Receipt date cannot be less than down time");
        }
        else
        {
            var x = _obj.InsertOffroadPhysicalBills();
            switch (x)
            {
                case 0:
                    Show("Physical Bills saving declined");
                    break;
                default:
                    Show("Physical Bills successfully saved");
                    ddlDistricts.SelectedIndex = 0;
                    ddlDistricts_SelectedIndexChanged(this,null);
                    ddlVehicleNo.SelectedIndex = 0;
                    ddlVehicleNo.Enabled = false;
                    lblBreakdwn.Text = "";
                    ddlBillNo.Items.Clear();
                    ddlBillNo.Enabled = false;
                    txtBillAmount.Text = "";
                    txtDownTime.Text = "";
                    txtUpTime.Text = "";
                    txtReceiptDate.Text = "";
                    txtCourierName.Text = "";
                    txtDocketNo.Text = "";
                    btnSave.Visible = true;
                    btnUpdate.Visible = false;
                    lblBreakdwn.Text = "";
                    BindGrid();
                    break;
            }
        }
    }

    public void BindGrid()
    {
        var ds = _obj.GetOffroadPhysicalBills();
        if (ds == null) throw new ArgumentNullException(nameof(ds));
        gvVehiclePhysicalBillDetails.DataSource = ds.Tables[0];
        gvVehiclePhysicalBillDetails.DataBind();
    }

    protected void gvVehiclePhysicalBillDetails_RowCommand(object sender,GridViewCommandEventArgs e)
    {
        if (e.CommandName == null) return;
        switch (e.CommandName.ToUpper())
        {
            case "VEHMAINEDIT":
                var row = (GridViewRow) ((WebControl) e.CommandSource).Parent.Parent;
                var link = (Label) row.FindControl("lblDistrict");
                var districttext = link.Text;
                btnSave.Visible = false;
                btnUpdate.Visible = true;
                var dsDist = (DataSet) ViewState["dsDistricts"];
                var dvDistrict = dsDist.Tables[0].DefaultView;
                dvDistrict.RowFilter = "ds_lname='" + districttext + "'";
                ddlDistricts.SelectedValue = Convert.ToString(dvDistrict.ToTable().Rows[0]["ds_dsid"]);
                ddlDistricts.Enabled = false;
                ddlDistricts_SelectedIndexChanged(this,null);
                ddlVehicleNo.Items.Insert(1,new ListItem(((Label) row.FindControl("lblVehicle_No")).Text,"1"));
                ddlVehicleNo.SelectedIndex = 1;
                ddlVehicleNo.Enabled = false;
                ddlVehicleNo_SelectedIndexChanged(this,null);
                ddlBillNo.Items.Insert(0,new ListItem(((Label) row.FindControl("lblBillNo")).Text,"0"));
                ddlBillNo.SelectedIndex = 0;
                ddlBillNo.Enabled = false;
                txtBillAmount.Text = ((Label) row.FindControl("lblBillAmount")).Text;
                var dt2 = DateTime.ParseExact(((Label) row.FindControl("lblDownTime")).Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                txtDownTime.Text = dt2.ToString(CultureInfo.CurrentCulture);
                var dt3 = DateTime.ParseExact(((Label) row.FindControl("lblUptime")).Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                txtUpTime.Text = dt3.ToString(CultureInfo.CurrentCulture);
                lblBreakdwn.Text = ((Label) row.FindControl("lblBrkdwn")).Text;
                var dt = Convert.ToDateTime(((Label) row.FindControl("lblReceiptDate")).Text);
                txtReceiptDate.Text = dt.ToString(CultureInfo.CurrentCulture);
                txtCourierName.Text = ((Label) row.FindControl("lblCourier_Name")).Text;
                txtDocketNo.Text = ((Label) row.FindControl("lblDocketNo")).Text;
                break;
        }
    }

    protected void btnUpdate_Click(object sender,EventArgs e)
    {
        _obj.District = ddlDistricts.SelectedItem.ToString();
        _obj.VehNumforNonOff = ddlVehicleNo.SelectedItem.ToString();
        _obj.NonOffBillNo = ddlBillNo.SelectedItem.ToString();
        _obj.NonOffAmount = txtBillAmount.Text;
        _obj.ReceiptDate = DateTime.ParseExact(txtReceiptDate.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
        _obj.CourierName = txtCourierName.Text;
        _obj.DocketNo = txtDocketNo.Text;
        _obj.CityId = Convert.ToInt32(lblBreakdwn.Text);
        var datedown = DateTime.ParseExact(txtDownTime.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
        if (_obj.ReceiptDate < datedown)
        {
            Show("Receipt date cannot be less than down time");
        }
        else
        {
            var i = _obj.UpdateOffroadPhysicalBills();
            switch (i)
            {
                case 0:
                    Show("Physical Bills updation declined");
                    break;
                default:
                    Show("Physical Bills successfully Updated");
                    ddlDistricts.SelectedIndex = 0;
                    ddlVehicleNo.SelectedIndex = 0;
                    ddlVehicleNo.Enabled = false;
                    ddlDistricts.Enabled = true;
                    ddlBillNo.Items.Clear();
                    ddlBillNo.Enabled = false;
                    txtBillAmount.Text = "";
                    txtDownTime.Text = "";
                    txtUpTime.Text = "";
                    txtReceiptDate.Text = "";
                    txtCourierName.Text = "";
                    txtDocketNo.Text = "";
                    btnSave.Visible = true;
                    btnUpdate.Visible = false;
                    lblBreakdwn.Text = "";
                    BindGrid();
                    break;
            }
        }
    }

    protected void gvVehiclePhysicalBillDetails_PageIndexChanging(object sender,GridViewPageEventArgs e)
    {
        gvVehiclePhysicalBillDetails.PageIndex = e.NewPageIndex;
        BindGrid();
    }

    protected void btnReset_Click(object sender,EventArgs e)
    {
        ddlDistricts.SelectedIndex = 0;
        ddlDistricts_SelectedIndexChanged(this,null);
        ddlVehicleNo.SelectedIndex = 0;
        ddlVehicleNo.Enabled = false;
        lblBreakdwn.Text = "";
        ddlBillNo.Items.Clear();
        ddlBillNo.Enabled = false;
        txtBillAmount.Text = "";
        txtDownTime.Text = "";
        txtUpTime.Text = "";
        txtReceiptDate.Text = "";
        txtCourierName.Text = "";
        txtDocketNo.Text = "";
        btnSave.Visible = true;
        btnUpdate.Visible = false;
        lblBreakdwn.Text = "";
    }
}