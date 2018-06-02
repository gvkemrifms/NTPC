using System;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using GvkFMSAPP.BLL;
using GvkFMSAPP.BLL.VAS_BLL;
using GvkFMSAPP.BLL.VehicleMaintenance;

public partial class VehicleMaintenanceDetailsNew : Page
{
    private readonly BaseVehicleDetails _fmsobj = new BaseVehicleDetails();
    private readonly VAS _fmsVas = new VAS();
    private readonly Helper _helper = new Helper();
    private readonly VASGeneral _vehallobj = new VASGeneral();
    private readonly VehicleMaintenance _vehMain = new VehicleMaintenance();
    private DataSet _dslabourAggregates;
    private DataSet _dsLabourCategories;
    private DataSet _dsLabourSubCategories;

    private bool _isedit;

    public bool Checkboxcount { get; private set; }

    protected void Page_PreInit(object sender, EventArgs e)
    {
        if (Session["Role_Id"] != null)
            switch (Session["Role_Id"].ToString())
            {
                case "118":
                    MasterPageFile = "~/MasterERO.master";
                    break;
            }
        else
            Response.Redirect("Login.aspx");
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            btnSave.Attributes.Add("onclick", "return validation()");
            GetDistrict();
            FillHoursandMins();
            GetTime();
            SetInitialRowSp();
            SetInitialRowLubricant();
            SetInitialRowLabour();
            btnSave.Enabled = true;
        }
    }

    protected void GetTime()
    {
        var timenow = DateTime.Now.ToString("dd/MM/yyyy hh:mm tt").Split(' ');
        txtUptime.Text = timenow[0];
        var hour = Convert.ToInt32(timenow[1].Split(':')[0]);
        var minute = Convert.ToInt32(timenow[1].Split(':')[1]);
        if (timenow[2] != "PM")
        {
            ddlUpHour.Items.FindByValue(timenow[1].Split(':')[0]).Selected = hour < 10;
        }
        else
        {
            if (hour == 12) hour = 0;
            hour = hour + 12;
            ddlUpHour.Items.FindByValue(hour.ToString()).Selected = true;
        }

        ddlUpMin.Items.FindByValue(timenow[1].Split(':')[1]).Selected = minute < 10;
    }

    public void GetVehicleNumber()
    {
        try
        {
            _fmsVas.District = ddlDistrict.SelectedItem.Text;
            var ds = _fmsVas.GetOffRoadVehicles(); // .GetVehicleNumber();
            if (ds == null) throw new ArgumentNullException(nameof(ds));
            _helper.FillDropDownHelperMethodWithDataSet(ds, "ORV_VehicleNumber", "ORV_VehOffRoadId", ddlVehicleNumber);
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    public void GetDistrict()
    {
        try
        {
            var ds = _fmsobj.GetDistricts_new();
            if (ds == null) throw new ArgumentNullException(nameof(ds));
            _helper.FillDropDownHelperMethodWithDataSet(ds, "district_name", "district_id", ddlDistrict);
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    private void FillHoursandMins()
    {
        int i;
        for (i = 0; i < 24; i++) ddlUpHour.Items.Add(i < 10 ? new ListItem("0" + i, "0" + i) : new ListItem(i.ToString(), i.ToString()));
        for (i = 0; i < 60; i++) ddlUpMin.Items.Add(i < 10 ? new ListItem("0" + i, "0" + i) : new ListItem(i.ToString(), i.ToString()));
    }

    protected void ddlVehicleNumber_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlVehicleNumber.SelectedIndex == 0) return;
            _fmsVas.VehicleNumber = ddlVehicleNumber.SelectedItem.Text;
            _fmsVas.VehOffRoadId = Convert.ToInt64(ddlVehicleNumber.SelectedValue);
            var ds = _fmsVas.GetOffRoadVehicleDetails();
            if (ds == null) throw new ArgumentNullException(nameof(ds));
            txtMaintenanceType.Text = ds.Tables[0].Rows[0][0].ToString();
            txtDownTime.Text = ds.Tables[1].Rows[0][0].ToString();
            txtDownOdo.Text = ds.Tables[2].Rows.Count != 0 ? ds.Tables[2].Rows[0][0].ToString() : "";
            lblBreakdownID.Text = ds.Tables[3].Rows[0][0].ToString();
            lblApprovedCost.Text = ds.Tables[4].Rows.Count != 0 ? ds.Tables[4].Rows[0][0].ToString() : "";
            GetVehicleMainDet();
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    protected void ddlSpareItemDesc_SelectedIndexChanged(object sender, EventArgs e)
    {
        var ddl = (DropDownList) sender;
        var row = (GridViewRow) ddl.NamingContainer;
        var rowIndex = row.RowIndex;
        _fmsVas.SpareId = Convert.ToString(((DropDownList) grdvwSPBillDetails.Rows[rowIndex].Cells[6].FindControl("ddlSpareItemDesc")).SelectedValue);
        _fmsVas.SpareItemDesc = ((DropDownList) grdvwSPBillDetails.Rows[rowIndex].Cells[6].FindControl("ddlSpareItemDesc")).SelectedItem.Text;
        _fmsVas.VehicleNumber = ddlVehicleNumber.SelectedItem.Text;
        var insres = _fmsVas.GetSpareCode();
        if (insres.Tables[0].Rows.Count > 0)
        {
            ((TextBox) grdvwSPBillDetails.Rows[rowIndex].Cells[5].FindControl("txtSparePartCode")).Text = Convert.ToString(insres.Tables[0].Rows[0]["ManufacturerSpare_Id"]);
            ((TextBox) grdvwSPBillDetails.Rows[rowIndex].Cells[8].FindControl("txtSpareBillAmount")).Text = Convert.ToString(insres.Tables[0].Rows[0]["Cost"]);
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        btnSave.Enabled = false;
        string spbillno = "", spbilldate = "", spbillamt = "", spvendorname = "", spemripartcode = "", sppartcode = "", spitemdesc = "", spqty = "";
        string lubribillno = "", lubribilldate = "", lubribillamt = "", lubrivendorname = "", lubriemripartcode = "", lubripartcode = "", lubriitemdesc = "", lubriqty = "";
        string labourbillno = "", labourbilldate = "", labourbillamt = "", labAggre = "", labCate = "", labsubCat = "", labItemDesc = "", labqty = "", labVendorname = "";
        _fmsVas.District = ddlDistrict.SelectedItem.Text; //txtDistrict.Text;
        _fmsVas.MaintenanaceType = txtMaintenanceType.Text;
        _fmsVas.MaintenanceDate = DateTime.Parse(txtMaintenanceDate.Text);
        _fmsVas.DownTimeOdoReading = txtDownOdo.Text;
        _fmsVas.Downtime = DateTime.Parse(txtDownTime.Text);
        _fmsVas.UptimeOdoReading = txtUpOdo.Text;
        _fmsVas.Uptime = DateTime.Parse(txtUptime.Text + " " + ddlUpHour.SelectedValue + ":" + ddlUpMin.SelectedValue + ":00");
        if (pnlSPBillDetails.Visible)
            for (var j = 0; j < grdvwSPBillDetails.Rows.Count; j++)
            {
                spbillno = spbillno + ((TextBox) grdvwSPBillDetails.Rows[j].Cells[2].FindControl("txtSpareBillNo")).Text + ",";
                spbilldate = spbilldate + ((TextBox) grdvwSPBillDetails.Rows[j].Cells[3].FindControl("txtSpareBillDate")).Text + ",";
                spbillamt = spbillamt + ((TextBox) grdvwSPBillDetails.Rows[j].Cells[8].FindControl("txtSpareBillAmount")).Text + ",";
                spemripartcode = spemripartcode + ((TextBox) grdvwSPBillDetails.Rows[j].Cells[4].FindControl("txtSpareEMRIpc")).Text + ",";
                spvendorname = spvendorname + ((DropDownList) grdvwSPBillDetails.Rows[j].Cells[1].FindControl("ddlSpareVendorName")).SelectedItem + ",";
                sppartcode = sppartcode + ((TextBox) grdvwSPBillDetails.Rows[j].Cells[5].FindControl("txtSparePartCode")).Text + ",";
                spitemdesc = spitemdesc + ((DropDownList) grdvwSPBillDetails.Rows[j].Cells[6].FindControl("ddlSpareItemDesc")).SelectedItem + ",";
                spqty = spqty + ((TextBox) grdvwSPBillDetails.Rows[j].Cells[7].FindControl("txtSpareQuant")).Text + ",";
            }

        if (pnlLubricantBillDetails.Visible)
            for (var j = 0; j < grdvwLubricantBillDetails.Rows.Count; j++)
            {
                lubribillno = lubribillno + ((TextBox) grdvwLubricantBillDetails.Rows[j].Cells[2].FindControl("txtLubricantBillNo")).Text + ",";
                lubribilldate = lubribilldate + ((TextBox) grdvwLubricantBillDetails.Rows[j].Cells[3].FindControl("txtLubricantBillDate")).Text + ",";
                lubribillamt = lubribillamt + ((TextBox) grdvwLubricantBillDetails.Rows[j].Cells[8].FindControl("txtLubricantBillAmount")).Text + ",";
                lubriemripartcode = lubriemripartcode + ((TextBox) grdvwLubricantBillDetails.Rows[j].Cells[4].FindControl("txtLubricantEMRIpc")).Text + ",";
                lubrivendorname = lubrivendorname + ((DropDownList) grdvwLubricantBillDetails.Rows[j].Cells[1].FindControl("ddlLubricantVendorName")).SelectedItem + ",";
                lubripartcode = lubripartcode + ((TextBox) grdvwLubricantBillDetails.Rows[j].Cells[5].FindControl("txtLubricantPartCode")).Text + ",";
                lubriitemdesc = lubriitemdesc + ((TextBox) grdvwLubricantBillDetails.Rows[j].Cells[6].FindControl("txtLubricantItemDesc")).Text + ",";
                lubriqty = lubriqty + ((TextBox) grdvwLubricantBillDetails.Rows[j].Cells[7].FindControl("txtLubricantQuant")).Text + ",";
            }

        if (pnlLabourBillDetails.Visible)
            for (var j = 0; j < grdvwLabourBillDetails.Rows.Count; j++)
            {
                labourbillno = labourbillno + ((TextBox) grdvwLabourBillDetails.Rows[j].Cells[2].FindControl("txtLabourBillNo")).Text + ",";
                labourbilldate = labourbilldate + ((TextBox) grdvwLabourBillDetails.Rows[j].Cells[3].FindControl("txtLabourBillDate")).Text + ",";
                labourbillamt = labourbillamt + ((TextBox) grdvwLabourBillDetails.Rows[j].Cells[9].FindControl("txtLabourBillAmount")).Text + ",";
                labVendorname = labVendorname + ((DropDownList) grdvwLabourBillDetails.Rows[j].Cells[1].FindControl("ddlLabourVendorName")).SelectedItem + ",";
                labAggre = labAggre + ((ComboBox) grdvwLabourBillDetails.Rows[j].Cells[4].FindControl("ddlLabourAggregates")).SelectedItem + ",";
                labCate = labCate + ((ComboBox) grdvwLabourBillDetails.Rows[j].Cells[5].FindControl("ddlLabourCategories")).SelectedItem + ",";
                labsubCat = labsubCat + ((ComboBox) grdvwLabourBillDetails.Rows[j].Cells[6].FindControl("ddlLabourSubCategories")).SelectedItem + ",";
                labItemDesc = labItemDesc + ((TextBox) grdvwLabourBillDetails.Rows[j].Cells[7].FindControl("txtLabourItemDesc")).Text + ",";
                labqty = labqty + ((TextBox) grdvwLabourBillDetails.Rows[j].Cells[8].FindControl("txtLabourQuant")).Text + ",";
            }

        _fmsVas.SpareBillNo = spbillno;
        _fmsVas.SpareBillAmount = spbillamt;
        _fmsVas.SpareBillDate = spbilldate;
        _fmsVas.SpareEmriPartCode = spemripartcode;
        _fmsVas.SpareItemDesc = spitemdesc;
        _fmsVas.SparePartCode = sppartcode;
        _fmsVas.SpareVendorName = spvendorname;
        _fmsVas.SpareQty = spqty;
        _fmsVas.LubricantBillNo = lubribillno;
        _fmsVas.LubricantBillAmount = lubribillamt;
        _fmsVas.LubricantBillDate = lubribilldate;
        _fmsVas.LubricantEmriPartCode = lubriemripartcode;
        _fmsVas.LubricantItemDesc = lubriitemdesc;
        _fmsVas.LubricantPartCode = lubripartcode;
        _fmsVas.LubricantVendorName = lubrivendorname;
        _fmsVas.LubricantQty = lubriqty;
        _fmsVas.LabourBillNo = labourbillno; //txtLabourBill.Text == "" ? "0" : txtLabourBill.Text;
        _fmsVas.LabourBillAmount = labourbillamt; //Convert.ToDecimal(txtLabourBillAmount.Text == "" ? "0" : txtLabourBillAmount.Text);
        _fmsVas.LabourBillDate = labourbilldate; //DateTime.Parse(txtLabourBillDate.Text == "" ? DateTime.Now.ToString() : txtLabourBillDate.Text);
        _fmsVas.LabourVendorName = labVendorname;
        _fmsVas.LabourAggregates = labAggre;
        _fmsVas.LabourCategories = labCate;
        _fmsVas.LabourSubCategories = labsubCat;
        _fmsVas.LabourItemDesc = labItemDesc;
        _fmsVas.LabourQty = labqty;
        var maintenanceDate = DateTime.ParseExact(txtMaintenanceDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        var downDate = DateTime.ParseExact(txtDownTime.Text, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
        if (downDate.Date <= maintenanceDate)
            switch (btnSave.Text)
            {
                case "Save":
                {
                    _fmsVas.VehicleNumber = ddlVehicleNumber.SelectedItem.Text;
                    var insres = _fmsVas.InsertOffRoadVehcileMaintenance();
                    if (Convert.ToString(insres.Tables[0].Rows[0]["Result"].ToString()) != string.Empty)
                    {
                        Show(Convert.ToString(insres.Tables[0].Rows[0]["Result"]));
                    }
                    else
                    {
                        Show("Records Inserted Successfully");
                        ClearAll();
                    }

                    GetVehicleMainDet();
                    break;
                }
                default:
                {
                    _fmsVas.VehicleNumber = txtVehicleNumber.Text;
                    _fmsVas.OffRoadId = Convert.ToInt32(ViewState["OffRoadId"]);
                    var insres = _fmsVas.UpdateOffRoadVehcileMaintenance();
                    if (Convert.ToString(insres.Tables[0].Rows[0]["Result"].ToString()) != string.Empty)
                    {
                        Show(Convert.ToString(insres.Tables[0].Rows[0]["Result"]));
                    }
                    else
                    {
                        Show("Records Updated Successfully");
                        ClearAll();
                    }

                    ddlVehicleNumber.Visible = true;
                    txtVehicleNumber.Visible = false;
                    break;
                }
            }
        else
            Show("Maintenance Date should be greater than Down Time.");
    }

    public void Show(string message)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('" + message + "');", true);
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        ClearAll();
    }

    public void ClearAll()
    {
        ddlVehicleNumber.Visible = true;
        txtVehicleNumber.Text = "";
        txtVehicleNumber.Visible = false;
        ddlVehicleNumber.SelectedIndex = 0;
        ddlDistrict.SelectedIndex = 0;
        txtMaintenanceType.Text = "";
        txtMaintenanceDate.Text = "";
        txtDownOdo.Text = "";
        txtDownTime.Text = "";
        txtUpOdo.Text = "";
        txtUptime.Text = "";
        ddlUpHour.SelectedIndex = 0;
        ddlUpMin.SelectedIndex = 0;
        lblBreakdownID.Text = "";
        chkAmount.Checked = true;
        foreach (ListItem item in chkbxlistBillType.Items) item.Selected = false;
        btnSave.Enabled = true;
        chkbxlistBillType.Enabled = false;
        pnlSPBillDetails.Visible = false;
        pnlLubricantBillDetails.Visible = false;
        pnlLabourBillDetails.Visible = false;
        pnlBillSummaryDetails.Visible = false;
        pnlBillDetailsSummaryBtn.Visible = false;
        txtTotalBillAmt.Text = "";
        lblApprovedCost.Text = "";
        btnSave.Text = "Save";
        btnSave.Enabled = true;
    }

    public void GetVehicleMainDet()
    {
        ViewState["VehicleMainDet"] = _fmsVas.GetVehicleMainDet();
        gvVehicleMaintenanceDetails.DataSource = (DataSet) ViewState["VehicleMainDet"];
        gvVehicleMaintenanceDetails.DataBind();
    }

    protected void gvVehicleMaintenanceDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvVehicleMaintenanceDetails.PageIndex = e.NewPageIndex;
        GetVehicleMainDet();
    }

    protected void gvVehicleMaintenanceDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "VehMainEdit":
                var drvehoffroad = ((DataSet) ViewState["VehicleMainDet"]).Tables[0].Select("OffRoad_Id=" + Convert.ToInt32(e.CommandArgument));
                ViewState["OffRoadId"] = Convert.ToInt32(drvehoffroad[0]["OffRoad_Id"].ToString());
                foreach (ListItem item in ddlDistrict.Items)
                {
                    if (!item.Selected) continue;
                    item.Selected = false;
                }

                ddlDistrict.Items.FindByText(drvehoffroad[0]["District"].ToString()).Selected = true;
                ddlVehicleNumber.Visible = false;
                txtVehicleNumber.Visible = true;
                txtVehicleNumber.Text = drvehoffroad[0]["OffRoadVehicle_No"].ToString();
                txtMaintenanceType.Text = drvehoffroad[0]["MaintenanaceType"].ToString();
                txtMaintenanceDate.Text = drvehoffroad[0]["MaintenanceDate"].ToString();
                txtDownOdo.Text = drvehoffroad[0]["DownTimeOdoReading"].ToString();
                txtDownTime.Text = drvehoffroad[0]["Downtime"].ToString();
                txtUpOdo.Text = drvehoffroad[0]["UptimeOdoReading"].ToString();
                var uptime = drvehoffroad[0]["Uptime"].ToString().Split(' ');
                txtUptime.Text = uptime[0];
                var hrmin = uptime[1].Split(':');
                ddlUpHour.SelectedIndex = uptime[2] == "PM" && hrmin[0] != "12" ? Convert.ToInt32(hrmin[0]) + 13 : Convert.ToInt32(hrmin[0]) + 1;
                ddlUpMin.SelectedIndex = Convert.ToInt32(hrmin[1]) + 1;
                DataSet dsbilldet;
                _fmsVas.OffRoadId = Convert.ToInt32(ViewState["OffRoadId"]);
                dsbilldet = _fmsVas.GetBillDetails();
                grdvwSPBillDetails.DataSource = dsbilldet.Tables[0];
                grdvwSPBillDetails.DataBind();
                SetInitialRowSp();
                grdvwLubricantBillDetails.DataSource = dsbilldet.Tables[1];
                grdvwLubricantBillDetails.DataBind();
                SetInitialRowLubricant();
                grdvwLabourBillDetails.DataSource = dsbilldet.Tables[2];
                grdvwLabourBillDetails.DataBind();
                SetInitialRowLabour();
                pnlSPBillDetails.Visible = false;
                pnlLubricantBillDetails.Visible = false;
                pnlLabourBillDetails.Visible = false;
                pnlBillDetailsSummaryBtn.Visible = false;
                pnlBillSummaryDetails.Visible = false;
                chkbxlistBillType.Items[0].Selected = false;
                chkbxlistBillType.Items[1].Selected = false;
                chkbxlistBillType.Items[2].Selected = false;
                if (dsbilldet.Tables[0].Rows.Count > 0)
                {
                    ViewState["SPBillDetails"] = dsbilldet.Tables[0];
                    DisplaySpBillDetails();
                    chkAmount.Checked = false;
                    chkbxlistBillType.Items[0].Selected = true;
                    pnlSPBillDetails.Visible = true;
                }

                if (dsbilldet.Tables[1].Rows.Count > 0)
                {
                    ViewState["LubriBillDetails"] = dsbilldet.Tables[1];
                    DisplayLubriBillDetails();
                    chkAmount.Checked = false;
                    chkbxlistBillType.Items[1].Selected = true;
                    pnlLubricantBillDetails.Visible = true;
                }

                if (dsbilldet.Tables[2].Rows.Count > 0)
                {
                    ViewState["LabourBillDetails"] = dsbilldet.Tables[2];
                    DisplayLabourBillDetails();
                    chkAmount.Checked = false;
                    chkbxlistBillType.Items[2].Selected = true;
                    pnlLabourBillDetails.Visible = true;
                }

                if (pnlSPBillDetails.Visible || pnlLubricantBillDetails.Visible || pnlLabourBillDetails.Visible)
                {
                    chkbxlistBillType.Enabled = true;
                    pnlBillDetailsSummaryBtn.Visible = true;
                    pnlBillSummaryDetails.Visible = true;
                    AddRowToGridSummary();
                }
                else
                {
                    chkAmount.Checked = true;
                    chkbxlistBillType.Enabled = false;
                }

                btnSave.Enabled = true;
                btnSave.Text = "Update";
                break;
            case "VehMainDelete":
                _fmsVas.OffRoadId = Convert.ToInt32(e.CommandArgument);
                var delres = _fmsVas.DelOffRoadVehcileMaintenance();
                Show(delres != 0 ? "Record Deleted Successfully!!" : "Error!!");
                GetVehicleMainDet();
                break;
        }
    }

    protected void ddlDistrict_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetVehicleNumber();
    }

    private void SetInitialRowSp()
    {
        var dt = new DataTable();
        //Define the Columns
        dt.Columns.Add(new DataColumn("RowNumber", typeof(string)));
        dt.Columns.Add(new DataColumn("ColSpVendorName", typeof(string)));
        dt.Columns.Add(new DataColumn("ColSpBillNo", typeof(string)));
        dt.Columns.Add(new DataColumn("ColSpBillDate", typeof(string)));
        dt.Columns.Add(new DataColumn("ColSpEMRIPartCode", typeof(string)));
        dt.Columns.Add(new DataColumn("ColSpPartCode", typeof(string)));
        dt.Columns.Add(new DataColumn("ColSpItemDescription", typeof(string)));
        dt.Columns.Add(new DataColumn("ColSpQuantity", typeof(string)));
        dt.Columns.Add(new DataColumn("Column3", typeof(string)));
        //Add a Dummy Data on Initial Load
        var dr = dt.NewRow();
        dr["RowNumber"] = 1;
        dt.Rows.Add(dr);
        //Store the DataTable in ViewState
        ViewState["CurrentTable"] = dt;
        //Bind the DataTable to the Grid
        grdvwSPBillDetails.DataSource = dt;
        grdvwSPBillDetails.DataBind();
        pnlBillSummaryDetails.Visible = false;
        btnSave.Enabled = false;
        txtTotalBillAmt.Text = "";
    }

    private void AddNewRowToGridSp()
    {
        if (ViewState["CurrentTable"] != null)
        {
            var dtCurrentTable = (DataTable) ViewState["CurrentTable"];
            var flag = false;
            if (dtCurrentTable.Rows.Count > 0)
            {
                var drCurrentRow = dtCurrentTable.NewRow();
                drCurrentRow["RowNumber"] = dtCurrentTable.Rows.Count + 1;
                for (var i = 0; i < dtCurrentTable.Rows.Count; i++)
                {
                    _fmsVas.SpareVendorName = ((DropDownList) grdvwSPBillDetails.Rows[i].Cells[1].FindControl("ddlSpareVendorName")).SelectedItem.Text;
                    _fmsVas.SpareBillNo = ((TextBox) grdvwSPBillDetails.Rows[i].Cells[2].FindControl("txtSpareBillNo")).Text;
                    var dsSpareValidation = _fmsVas.SpareValidation();
                    if (dsSpareValidation.Tables[0].Rows[0][0].ToString() != "1") continue;
                    Show(" Already Vendor Name :  " + ((DropDownList) grdvwSPBillDetails.Rows[i].Cells[1].FindControl("ddlSpareVendorName")).SelectedItem.Text + "    and Bill Number :   " + ((TextBox) grdvwSPBillDetails.Rows[i].Cells[2].FindControl("txtSpareBillNo")).Text + "   Exists in DataBase ");
                    flag = true;
                    break;
                }

                if (flag) return;
                //add new row to DataTable
                dtCurrentTable.Rows.Add(drCurrentRow);
                //Store the current data to ViewState
                ViewState["CurrentTable"] = dtCurrentTable;
                var ds1 = (DataSet) ViewState["Vendor"];
                for (var i = 0; i < dtCurrentTable.Rows.Count - 1; i++)
                {
                    //extract the DropDownList Selected Items
                    var ddl1 = (DropDownList) grdvwSPBillDetails.Rows[i].Cells[1].FindControl("ddlSpareVendorName");
                    if (((DropDownList) grdvwSPBillDetails.Rows[i].Cells[1].FindControl("ddlSpareVendorName")).Text == "" || ((DropDownList) grdvwSPBillDetails.Rows[i].Cells[1].FindControl("ddlSpareVendorName")).Text == string.Empty) _helper.FillDropDownHelperMethodWithDataSet(ds1, "AgencyName", "AgencyId", ddl1);
                    var txt2 = (TextBox) grdvwSPBillDetails.Rows[i].Cells[2].FindControl("txtSpareBillNo");
                    var txt3 = (TextBox) grdvwSPBillDetails.Rows[i].Cells[3].FindControl("txtSpareBillDate");
                    var txt4 = (TextBox) grdvwSPBillDetails.Rows[i].Cells[4].FindControl("txtSpareEMRIpc");
                    var txt5 = (TextBox) grdvwSPBillDetails.Rows[i].Cells[5].FindControl("txtSparePartCode");
                    var ddlSpareItem = (DropDownList) grdvwSPBillDetails.Rows[i].Cells[6].FindControl("ddlSpareItemDesc");
                    var txt7 = (TextBox) grdvwSPBillDetails.Rows[i].Cells[7].FindControl("txtSpareQuant");
                    var txt8 = (TextBox) grdvwSPBillDetails.Rows[i].Cells[8].FindControl("txtSpareBillAmount");
                    dtCurrentTable.Rows[i]["ColSpVendorName"] = ((DropDownList) grdvwSPBillDetails.Rows[i].Cells[1].FindControl("ddlSpareVendorName")).SelectedItem.Text;
                    dtCurrentTable.Rows[i]["ColSpBillNo"] = txt2.Text;
                    dtCurrentTable.Rows[i]["ColSpBillDate"] = txt3.Text;
                    dtCurrentTable.Rows[i]["ColSpEMRIPartCode"] = txt4.Text;
                    dtCurrentTable.Rows[i]["ColSpPartCode"] = txt5.Text;
                    dtCurrentTable.Rows[i]["ColSpItemDescription"] = ddlSpareItem.SelectedItem.Text;
                    dtCurrentTable.Rows[i]["ColSpQuantity"] = txt7.Text;
                    dtCurrentTable.Rows[i]["Column3"] = txt8.Text;
                    //Adding first three columns default value in the new row
                    if (i == dtCurrentTable.Rows.Count - 2)
                    {
                        var ddl2 = (DropDownList) grdvwSPBillDetails.Rows[i].Cells[1].FindControl("ddlSpareVendorName");
                        if (((DropDownList) grdvwSPBillDetails.Rows[i].Cells[1].FindControl("ddlSpareVendorName")).Text == "" || ((DropDownList) grdvwSPBillDetails.Rows[i].Cells[1].FindControl("ddlSpareVendorName")).Text == string.Empty) _helper.FillDropDownHelperMethodWithDataSet(ds1, "AgencyName", "AgencyId", ddl2);
                        dtCurrentTable.Rows[i + 1]["ColSpBillNo"] = txt2.Text;
                        dtCurrentTable.Rows[i + 1]["ColSpBillDate"] = txt3.Text;
                    }
                }

                //Rebind the Grid with the current data
                grdvwSPBillDetails.DataSource = dtCurrentTable;
                grdvwSPBillDetails.DataBind();
                pnlBillSummaryDetails.Visible = false;
                btnSave.Enabled = false;
                txtTotalBillAmt.Text = "";
            }

            ////AddRowToGridSummary();
        }
        else
        {
            Response.Write("ViewState is null");
        }

        SetPreviousDataSp();
    }

    private void DisplaySpBillDetails()
    {
        var dtspbilldet = (DataTable) ViewState["SPBillDetails"];
        var dt = new DataTable();
        //Define the Columns
        dt.Columns.Add(new DataColumn("RowNumber", typeof(string)));
        dt.Columns.Add(new DataColumn("ColSpVendorName", typeof(string)));
        dt.Columns.Add(new DataColumn("ColSpBillNo", typeof(string)));
        dt.Columns.Add(new DataColumn("ColSpBillDate", typeof(string)));
        dt.Columns.Add(new DataColumn("ColSpEMRIPartCode", typeof(string)));
        dt.Columns.Add(new DataColumn("ColSpPartCode", typeof(string)));
        dt.Columns.Add(new DataColumn("ColSpItemDescription", typeof(string)));
        dt.Columns.Add(new DataColumn("ColSpQuantity", typeof(string)));
        dt.Columns.Add(new DataColumn("Column3", typeof(string)));
        //Add Data on Load
        for (var i = 0; i < dtspbilldet.Rows.Count; i++)
        {
            var dr = dt.NewRow();
            dr["RowNumber"] = i + 1;
            dt.Rows.Add(dr);
            dt.Rows[i]["ColSpBillNo"] = dtspbilldet.Rows[i]["ColSpBillNo"].ToString();
            dt.Rows[i]["ColSpBillDate"] = dtspbilldet.Rows[i]["ColSpBillDate"].ToString();
            dt.Rows[i]["ColSpVendorName"] = dtspbilldet.Rows[i]["ColSpVendorName"].ToString();
            dt.Rows[i]["ColSpEMRIPartCode"] = dtspbilldet.Rows[i]["ColSpEMRIPartCode"].ToString();
            dt.Rows[i]["ColSpPartCode"] = dtspbilldet.Rows[i]["ColSpPartCode"].ToString();
            dt.Rows[i]["ColSpItemDescription"] = dtspbilldet.Rows[i]["ColSpItemDescription"].ToString();
            dt.Rows[i]["ColSpQuantity"] = dtspbilldet.Rows[i]["ColSpQuantity"].ToString();
            dt.Rows[i]["Column3"] = dtspbilldet.Rows[i]["Column3"].ToString();
        }

        _isedit = true;
        //Store the DataTable in ViewState
        ViewState["CurrentTable"] = dt;
        //Bind the DataTable to the Grid
        grdvwSPBillDetails.DataSource = dt;
        grdvwSPBillDetails.DataBind();
    }

    private void SetPreviousDataSp()
    {
        if (ViewState["CurrentTable"] != null)
        {
            var dt = (DataTable) ViewState["CurrentTable"];
            if (dt.Rows.Count > 0)
            {
                var ds1 = (DataSet) ViewState["Vendor"];
                var ds2 = (DataSet) ViewState["SpareItemDesc"];
                for (var i = 0; i < dt.Rows.Count; i++)
                {
                    //Set the Previous Selected Items on Each DropDownList on Postbacks
                    var ddl1 = (DropDownList) grdvwSPBillDetails.Rows[i].Cells[1].FindControl("ddlSpareVendorName");
                    _helper.FillDropDownHelperMethodWithDataSet(ds1, "AgencyName", "AgencyId", ddl1);
                    var ddl2 = (DropDownList) grdvwSPBillDetails.Rows[i].Cells[1].FindControl("ddlSpareItemDesc");
                    _helper.FillDropDownHelperMethodWithDataSet(ds2, "SparePart_Name", "ManufacturerSpare_Id", ddl2);
                    var txt2 = (TextBox) grdvwSPBillDetails.Rows[i].Cells[2].FindControl("txtSpareBillNo");
                    var txt3 = (TextBox) grdvwSPBillDetails.Rows[i].Cells[3].FindControl("txtSpareBillDate");
                    var txt4 = (TextBox) grdvwSPBillDetails.Rows[i].Cells[4].FindControl("txtSpareEMRIpc");
                    var txt5 = (TextBox) grdvwSPBillDetails.Rows[i].Cells[5].FindControl("txtSparePartCode");
                    var txt7 = (TextBox) grdvwSPBillDetails.Rows[i].Cells[7].FindControl("txtSpareQuant");
                    var txt8 = (TextBox) grdvwSPBillDetails.Rows[i].Cells[8].FindControl("txtSpareBillAmount");
                    if (i >= dt.Rows.Count - 1) continue;
                    var dvVendorName = new DataView(ds1.Tables[0]) {RowFilter = "AgencyName = '" + dt.Rows[i][1] + "'"};
                    var vendorId = dvVendorName.ToTable().Rows[0]["AgencyId"].ToString();
                    ddl1.SelectedValue = vendorId;
                    txt2.Text = dt.Rows[i]["ColSpBillNo"].ToString();
                    txt3.Text = dt.Rows[i]["ColSpBillDate"].ToString();
                    txt4.Text = dt.Rows[i]["ColSpEMRIPartCode"].ToString();
                    txt5.Text = dt.Rows[i]["ColSpPartCode"].ToString();
                    var dvItemDesc = new DataView(ds2.Tables[0]) {RowFilter = "SparePart_Name = '" + dt.Rows[i][6] + "'"};
                    var itemId = dvItemDesc.ToTable().Rows[0]["ManufacturerSpare_Id"].ToString();
                    ddl2.SelectedValue = itemId;
                    txt7.Text = dt.Rows[i]["ColSpQuantity"].ToString();
                    txt8.Text = dt.Rows[i]["Column3"].ToString();
                }
            }
        }
    }

    protected void btnAddNewSPRow_Click(object sender, EventArgs e)
    {
        AddNewRowToGridSp();
    }

    protected void btnSPReset_Click(object sender, EventArgs e)
    {
        SetInitialRowSp();
    }

    private void SetInitialRowLubricant()
    {
        var dt = new DataTable();
        //Define the Columns
        dt.Columns.Add(new DataColumn("RowNumberLubri", typeof(string)));
        dt.Columns.Add(new DataColumn("ColLubriVendorName", typeof(string)));
        dt.Columns.Add(new DataColumn("ColLubriBillNo", typeof(string)));
        dt.Columns.Add(new DataColumn("ColLubriBillDate", typeof(string)));
        dt.Columns.Add(new DataColumn("ColLubriEMRIPartCode", typeof(string)));
        dt.Columns.Add(new DataColumn("ColLubriPartCode", typeof(string)));
        dt.Columns.Add(new DataColumn("ColLubriItemDescription", typeof(string)));
        dt.Columns.Add(new DataColumn("ColLabQuantity", typeof(string)));
        dt.Columns.Add(new DataColumn("ColLubriBillAmount", typeof(string)));

        //Add a Dummy Data on Initial Load
        var dr = dt.NewRow();
        dr["RowNumberLubri"] = 1;
        dt.Rows.Add(dr);
        //Store the DataTable in ViewState
        ViewState["LubriCurrentTable"] = dt;
        //Bind the DataTable to the Grid
        grdvwLubricantBillDetails.DataSource = dt;
        grdvwLubricantBillDetails.DataBind();
        pnlBillSummaryDetails.Visible = false;
        btnSave.Enabled = false;
        txtTotalBillAmt.Text = "";
        ////AddRowToGridSummary();
    }

    private void AddNewRowToGridLubricant()
    {
        if (ViewState["LubriCurrentTable"] != null)
        {
            var dtCurrentTableLubri = (DataTable) ViewState["LubriCurrentTable"];
            var flag = false;
            if (dtCurrentTableLubri.Rows.Count > 0)
            {
                var drCurrentRowLubri = dtCurrentTableLubri.NewRow();
                drCurrentRowLubri["RowNumberLubri"] = dtCurrentTableLubri.Rows.Count + 1;
                for (var i = 0; i < dtCurrentTableLubri.Rows.Count; i++)
                {
                    _fmsVas.LubricantVendorName = ((DropDownList) grdvwLubricantBillDetails.Rows[i].Cells[1].FindControl("ddlLubricantVendorName")).SelectedItem.Text;
                    _fmsVas.LubricantBillNo = ((TextBox) grdvwLubricantBillDetails.Rows[i].Cells[2].FindControl("txtLubricantBillNo")).Text;
                    var dsLubriValidation = _fmsVas.LubriValidation();
                    if (dsLubriValidation.Tables[0].Rows[0][0].ToString() != "1") continue;
                    Show(" Already Vendor Name :   " + ((DropDownList) grdvwLubricantBillDetails.Rows[i].Cells[1].FindControl("ddlLubricantVendorName")).SelectedItem.Text + "    and Bill Number:   " + ((TextBox) grdvwLubricantBillDetails.Rows[i].Cells[2].FindControl("txtLubricantBillNo")).Text + "   Exists in DataBase ");
                    flag = true;
                    break;
                }

                if (flag) return;
                //add new row to DataTable
                dtCurrentTableLubri.Rows.Add(drCurrentRowLubri);
                //Store the current data to ViewState
                ViewState["LubriCurrentTable"] = dtCurrentTableLubri;
                var ds1 = (DataSet) ViewState["Vendor"];
                for (var i = 0; i < dtCurrentTableLubri.Rows.Count - 1; i++)
                {
                    //extract the DropDownList Selected Items
                    var ddl1 = (DropDownList) grdvwLubricantBillDetails.Rows[i].Cells[1].FindControl("ddlLubricantVendorName");
                    if (((DropDownList) grdvwLubricantBillDetails.Rows[i].Cells[1].FindControl("ddlLubricantVendorName")).Text == "" || ((DropDownList) grdvwLubricantBillDetails.Rows[i].Cells[1].FindControl("ddlLubricantVendorName")).Text == string.Empty) _helper.FillDropDownHelperMethodWithDataSet(ds1, "AgencyName", "AgencyId", ddl1);
                    var txt2 = (TextBox) grdvwLubricantBillDetails.Rows[i].Cells[2].FindControl("txtLubricantBillNo");
                    var txt3 = (TextBox) grdvwLubricantBillDetails.Rows[i].Cells[3].FindControl("txtLubricantBillDate");
                    var txt4 = (TextBox) grdvwLubricantBillDetails.Rows[i].Cells[4].FindControl("txtLubricantEMRIpc");
                    var txt5 = (TextBox) grdvwLubricantBillDetails.Rows[i].Cells[5].FindControl("txtLubricantPartCode");
                    var txt6 = (TextBox) grdvwLubricantBillDetails.Rows[i].Cells[6].FindControl("txtLubricantItemDesc");
                    var txt7 = (TextBox) grdvwLubricantBillDetails.Rows[i].Cells[7].FindControl("txtLubricantQuant");
                    var txt8 = (TextBox) grdvwLubricantBillDetails.Rows[i].Cells[8].FindControl("txtLubricantBillAmount");
                    dtCurrentTableLubri.Rows[i]["ColLubriVendorName"] = ((DropDownList) grdvwLubricantBillDetails.Rows[i].Cells[1].FindControl("ddlLubricantVendorName")).SelectedItem.Text;
                    dtCurrentTableLubri.Rows[i]["ColLubriBillNo"] = txt2.Text;
                    dtCurrentTableLubri.Rows[i]["ColLubriBillDate"] = txt3.Text;
                    dtCurrentTableLubri.Rows[i]["ColLubriEMRIPartCode"] = txt4.Text;
                    dtCurrentTableLubri.Rows[i]["ColLubriPartCode"] = txt5.Text;
                    dtCurrentTableLubri.Rows[i]["ColLubriItemDescription"] = txt6.Text;
                    dtCurrentTableLubri.Rows[i]["ColLabQuantity"] = txt7.Text;
                    dtCurrentTableLubri.Rows[i]["ColLubriBillAmount"] = txt8.Text;
                    if (i != dtCurrentTableLubri.Rows.Count - 2) continue;
                    dtCurrentTableLubri.Rows[i + 1]["ColLubriVendorName"] = ((DropDownList) grdvwLubricantBillDetails.Rows[i].Cells[1].FindControl("ddlLubricantVendorName")).SelectedItem.Text;
                    dtCurrentTableLubri.Rows[i + 1]["ColLubriBillNo"] = txt2.Text;
                    dtCurrentTableLubri.Rows[i + 1]["ColLubriBillDate"] = txt3.Text;
                }

                //Rebind the Grid with the current data
                grdvwLubricantBillDetails.DataSource = dtCurrentTableLubri;
                grdvwLubricantBillDetails.DataBind();
                pnlBillSummaryDetails.Visible = false;
                btnSave.Enabled = false;
                txtTotalBillAmt.Text = "";
            }

            ////AddRowToGridSummary();
        }
        else
        {
            Response.Write("ViewState is null");
        }

        SetPreviousDataLubri();
    }

    private void DisplayLubriBillDetails()
    {
        var dtlubribilldet = (DataTable) ViewState["LubriBillDetails"];
        var dt = new DataTable();

        //Define the Columns
        dt.Columns.Add(new DataColumn("RowNumberLubri", typeof(string)));
        dt.Columns.Add(new DataColumn("ColLubriBillNo", typeof(string)));
        dt.Columns.Add(new DataColumn("ColLubriBillDate", typeof(string)));
        dt.Columns.Add(new DataColumn("ColLubriBillAmount", typeof(string)));
        dt.Columns.Add(new DataColumn("ColLubriVendorName", typeof(string)));
        dt.Columns.Add(new DataColumn("ColLubriEMRIPartCode", typeof(string)));
        dt.Columns.Add(new DataColumn("ColLubriPartCode", typeof(string)));
        dt.Columns.Add(new DataColumn("ColLubriItemDescription", typeof(string)));
        dt.Columns.Add(new DataColumn("ColLabQuantity", typeof(string)));
        //Add Data on Load
        for (var i = 0; i < dtlubribilldet.Rows.Count; i++)
        {
            var dr = dt.NewRow();
            dr["RowNumberLubri"] = i + 1;
            dt.Rows.Add(dr);
            dt.Rows[i]["ColLubriBillNo"] = dtlubribilldet.Rows[i]["ColLubriBillNo"].ToString();
            dt.Rows[i]["ColLubriBillDate"] = dtlubribilldet.Rows[i]["ColLubriBillDate"].ToString();
            dt.Rows[i]["ColLubriBillAmount"] = dtlubribilldet.Rows[i]["ColLubriBillAmount"].ToString();
            dt.Rows[i]["ColLubriVendorName"] = dtlubribilldet.Rows[i]["ColLubriVendorName"].ToString();
            dt.Rows[i]["ColLubriEMRIPartCode"] = dtlubribilldet.Rows[i]["ColLubriEMRIPartCode"].ToString();
            dt.Rows[i]["ColLubriPartCode"] = dtlubribilldet.Rows[i]["ColLubriPartCode"].ToString();
            dt.Rows[i]["ColLubriItemDescription"] = dtlubribilldet.Rows[i]["ColLubriItemDescription"].ToString();
            dt.Rows[i]["ColLabQuantity"] = dtlubribilldet.Rows[i]["ColLabQuantity"].ToString();
        }

        _isedit = true;
        //Store the DataTable in ViewState
        ViewState["LubriCurrentTable"] = dt;
        //Bind the DataTable to the Grid
        grdvwLubricantBillDetails.DataSource = dt;
        grdvwLubricantBillDetails.DataBind();
    }

    private void SetPreviousDataLubri()
    {
        if (ViewState["LubriCurrentTable"] != null)
        {
            var dt = (DataTable) ViewState["LubriCurrentTable"];
            if (dt.Rows.Count > 0)
            {
                var ds1 = (DataSet) ViewState["Vendor"];
                for (var i = 0; i < dt.Rows.Count; i++)
                {
                    //Set the Previous Selected Items on Each DropDownList on Postbacks
                    var ddl1 = (DropDownList) grdvwLubricantBillDetails.Rows[i].Cells[1].FindControl("ddlLubricantVendorName");
                    _helper.FillDropDownHelperMethodWithDataSet(ds1, "AgencyName", "AgencyId", ddl1);
                    var txt2 = (TextBox) grdvwLubricantBillDetails.Rows[i].Cells[2].FindControl("txtLubricantBillNo");
                    var txt3 = (TextBox) grdvwLubricantBillDetails.Rows[i].Cells[3].FindControl("txtLubricantBillDate");
                    var txt4 = (TextBox) grdvwLubricantBillDetails.Rows[i].Cells[4].FindControl("txtLubricantEMRIpc");
                    var txt5 = (TextBox) grdvwLubricantBillDetails.Rows[i].Cells[5].FindControl("txtLubricantPartCode");
                    var txt6 = (TextBox) grdvwLubricantBillDetails.Rows[i].Cells[6].FindControl("txtLubricantItemDesc");
                    var txt7 = (TextBox) grdvwLubricantBillDetails.Rows[i].Cells[7].FindControl("txtLubricantQuant");
                    var txt8 = (TextBox) grdvwLubricantBillDetails.Rows[i].Cells[8].FindControl("txtLubricantBillAmount");
                    if (i >= dt.Rows.Count - 1) continue;
                    var dvVendorName = new DataView(ds1.Tables[0]) {RowFilter = "AgencyName = '" + dt.Rows[i][1] + "'"};
                    var vendorId = dvVendorName.ToTable().Rows[0]["AgencyId"].ToString();
                    ddl1.SelectedValue = vendorId;
                    txt2.Text = dt.Rows[i]["ColLubriBillNo"].ToString();
                    txt3.Text = dt.Rows[i]["ColLubriBillDate"].ToString();
                    txt4.Text = dt.Rows[i]["ColLubriEMRIPartCode"].ToString();
                    txt5.Text = dt.Rows[i]["ColLubriPartCode"].ToString();
                    txt6.Text = dt.Rows[i]["ColLubriItemDescription"].ToString();
                    txt7.Text = dt.Rows[i]["ColLabQuantity"].ToString();
                    txt8.Text = dt.Rows[i]["ColLubriBillAmount"].ToString();
                }
            }
        }
    }

    protected void btnAddNewLubriRow_Click(object sender, EventArgs e)
    {
        AddNewRowToGridLubricant();
    }

    protected void btnLubriReset_Click(object sender, EventArgs e)
    {
        SetInitialRowLubricant();
    }

    private void SetInitialRowLabour()
    {
        try
        {
            var dt = new DataTable();
            DataRow dr;
            dt.Columns.Add(new DataColumn("RowNumberLabour", typeof(string)));
            dt.Columns.Add(new DataColumn("ColLabVendorName", typeof(string)));
            dt.Columns.Add(new DataColumn("ColLabBillNo", typeof(string)));
            dt.Columns.Add(new DataColumn("ColLabBillDate", typeof(string)));
            dt.Columns.Add(new DataColumn("Aggregates", typeof(string)));
            dt.Columns.Add(new DataColumn("Categories", typeof(string)));
            dt.Columns.Add(new DataColumn("Sub_Categories", typeof(string)));
            dt.Columns.Add(new DataColumn("ColLabItemDescription", typeof(string)));
            dt.Columns.Add(new DataColumn("ColLabQuantity", typeof(string)));
            dt.Columns.Add(new DataColumn("Column3", typeof(string)));
            var dslabourFromDb = (DataSet) ViewState["Categories"];
            if (dslabourFromDb != null)
            {
                var count = 0;
                if (dslabourFromDb.Tables.Count > 0)
                    if (dslabourFromDb.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow drLabour in dslabourFromDb.Tables[0].Rows)
                        {
                            dr = dt.NewRow();
                            dr["Aggregates"] = Convert.ToString(drLabour["Aggregates"]);
                            dr["Categories"] = Convert.ToString(drLabour["Categories"]);
                            dr["Sub_Categories"] = Convert.ToString(drLabour["Sub_Categories"]);
                            dr["Column3"] = Convert.ToString(drLabour["Column3"]);
                            dr["RowNumberLabour"] = ++count;
                            dt.Rows.Add(dr);
                        }

                        ViewState["LabourCurrentTable"] = dt;
                        ViewState["IsGridSet"] = 1;
                        for (var i = 0; i < grdvwLabourBillDetails.Rows.Count; i++)
                            if (i < dt.Rows.Count)
                            {
                                var dslabourAggregatesnew = (DataSet) ViewState["LabourAggregates"];
                                var dsLabourCategoriesnew = (DataSet) ViewState["LabourCategories"];
                                var dsLabourSubCategories = (DataSet) ViewState["LabourSubCategories"];
                                var ddl4 = grdvwLabourBillDetails.Rows[i].FindControl("ddlLabourAggregates") as ComboBox;
                                if (ddl4 != null)
                                {
                                    _helper.FillDropDownHelperMethodWithDataSet(dslabourAggregatesnew, "Aggregates", "Aggregate_Id", null, ddl4);
                                    var dvagg = new DataView(dslabourAggregatesnew.Tables[0]) {RowFilter = "Aggregates = '" + dt.Rows[i][4] + "'"};
                                    var aggid = dvagg.ToTable().Rows[0]["Aggregate_Id"].ToString();
                                    ddl4.SelectedValue = aggid;
                                }

                                var ddl5 = grdvwLabourBillDetails.Rows[i].FindControl("ddlLabourCategories") as ComboBox;
                                if (ddl5 != null)
                                {
                                    _helper.FillDropDownHelperMethodWithDataSet(dsLabourCategoriesnew, "Categories", "Category_Id", null, ddl5);
                                    var dvcat = new DataView(dsLabourCategoriesnew.Tables[0]) {RowFilter = "Categories = '" + dt.Rows[i][5] + "'"};
                                    var catid = dvcat.ToTable().Rows[0]["Category_Id"].ToString();
                                    ddl5.SelectedValue = catid;
                                }

                                var ddl6 = grdvwLabourBillDetails.Rows[i].FindControl("ddlLabourSubCategories") as ComboBox;
                                if (ddl6 != null)
                                {
                                    _helper.FillDropDownHelperMethodWithDataSet(dsLabourSubCategories, "SubCategories", "SubCategory_Id", null, ddl6);
                                    var dvSub = new DataView(dsLabourSubCategories.Tables[0]) {RowFilter = "SubCategories = '" + dt.Rows[i][6] + "'"};
                                    var subid = dvSub.ToTable().Rows[0]["SubCategory_Id"].ToString();
                                    ddl6.SelectedValue = subid;
                                }
                            }

                        grdvwLabourBillDetails.DataSource = dt;
                        grdvwLabourBillDetails.DataBind();
                        var count1 = 0;
                        foreach (GridViewRow item in grdvwLabourBillDetails.Rows)
                        {
                            if (dt.Rows[count1]["Aggregates"].ToString() != string.Empty) ((ComboBox) item.FindControl("ddlLabourAggregates")).Items.FindByText(dt.Rows[count1]["Aggregates"].ToString()).Selected = true;
                            if (dt.Rows[count1]["Categories"].ToString() != string.Empty) ((ComboBox) item.FindControl("ddlLabourCategories")).Items.FindByText(dt.Rows[count1]["Categories"].ToString()).Selected = true;
                            if (dt.Rows[count1]["Sub_Categories"].ToString() != string.Empty) ((ComboBox) item.FindControl("ddlLabourSubCategories")).Items.FindByText(dt.Rows[count1]["Sub_Categories"].ToString()).Selected = true;
                            count1 += 1;
                        }
                    }
                    else
                    {
                        //Add a Dummy Data on Initial Load
                        dr = dt.NewRow();
                        dr["RowNumberLabour"] = 1;
                        dt.Rows.Add(dr);

                        //Store the DataTable in ViewState
                        ViewState["LabourCurrentTable"] = dt;
                        if (IsPostBack && dt.Rows.Count == 1)
                        {
                            grdvwLabourBillDetails.DataSource = dt;
                            grdvwLabourBillDetails.DataBind();
                            pnlBillSummaryDetails.Visible = false;
                            btnSave.Enabled = false;
                            txtTotalBillAmt.Text = "";
                        }
                    }
            }
            else
            {
                //Add a Dummy Data on Initial Load
                dr = dt.NewRow();
                dr["RowNumberLabour"] = 1;
                dt.Rows.Add(dr);
                grdvwLabourBillDetails.DataSource = dt;
                grdvwLabourBillDetails.DataBind();
                pnlBillSummaryDetails.Visible = false;
                btnSave.Enabled = false;
                txtTotalBillAmt.Text = "";
                //Store the DataTable in ViewState
                ViewState["LabourCurrentTable"] = dt;
            }

            if (IsPostBack == false)
            {
                //Bind the DataTable to the Grid
                grdvwLabourBillDetails.DataSource = dt;
                grdvwLabourBillDetails.DataBind();
                pnlBillSummaryDetails.Visible = false;
                btnSave.Enabled = false;
                txtTotalBillAmt.Text = "";
            }
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    private void AddNewRowToGridLabour()
    {
        if (ViewState["LabourCurrentTable"] != null)
        {
            var dtCurrentTableLabour = (DataTable) ViewState["LabourCurrentTable"];
            if (dtCurrentTableLabour.Rows.Count > 0)
            {
                var drCurrentRowLabour = dtCurrentTableLabour.NewRow();
                drCurrentRowLabour["RowNumberLabour"] = dtCurrentTableLabour.Rows.Count + 1;
                var flag = false;
                for (var i = 0; i < dtCurrentTableLabour.Rows.Count; i++)
                {
                    _fmsVas.LabourVendorName = ((DropDownList) grdvwLabourBillDetails.Rows[i].Cells[1].FindControl("ddlLabourVendorName")).SelectedItem.Text;
                    _fmsVas.LabourBillNo = ((TextBox) grdvwLabourBillDetails.Rows[i].Cells[2].FindControl("txtLabourBillNo")).Text;
                    var dsLabValidation = _fmsVas.LabValidation();
                    if (dsLabValidation.Tables[0].Rows[0][0].ToString() != "1") continue;
                    Show(" Already Vendor Name :    " + ((DropDownList) grdvwLabourBillDetails.Rows[i].Cells[1].FindControl("ddlLabourVendorName")).SelectedItem.Text + "and Bill Number :    " + ((TextBox) grdvwLabourBillDetails.Rows[i].Cells[2].FindControl("txtLabourBillNo")).Text + " Exists in DataBase ");
                    flag = true;
                    break;
                }

                if (flag) return;

                //add new row to DataTable
                dtCurrentTableLabour.Rows.Add(drCurrentRowLabour);
                //Store the current data to ViewState
                ViewState["LabourCurrentTable"] = dtCurrentTableLabour;
                var ds = (DataSet) ViewState["Categories"];
                var ds1 = (DataSet) ViewState["Vendor"];
                var dslabourAggregatesnew = (DataSet) ViewState["LabourAggregates"];
                var dsLabourCategoriesnew = (DataSet) ViewState["LabourCategories"];
                var dsLabourSubCategories = (DataSet) ViewState["LabourSubCategories"];
                for (var i = 0; i < dtCurrentTableLabour.Rows.Count - 1; i++)
                {
                    var txt2 = (TextBox) grdvwLabourBillDetails.Rows[i].Cells[2].FindControl("txtLabourBillNo");
                    var txt3 = (TextBox) grdvwLabourBillDetails.Rows[i].Cells[3].FindControl("txtLabourBillDate");
                    var ddl4 = (ComboBox) grdvwLabourBillDetails.Rows[i].Cells[4].FindControl("ddlLabourAggregates");
                    ddl4.SelectedItem.Text = ds.Tables[0].Rows.Count > i ? ds.Tables[0].Rows[i][4].ToString() : ((ComboBox) grdvwLabourBillDetails.Rows[i].Cells[4].FindControl("ddlLabourAggregates")).SelectedItem.Text;
                    var ddl5 = (ComboBox) grdvwLabourBillDetails.Rows[i].Cells[5].FindControl("ddlLabourCategories");
                    ddl5.SelectedItem.Text = ds.Tables[0].Rows.Count > i ? ds.Tables[0].Rows[i][5].ToString() : ((ComboBox) grdvwLabourBillDetails.Rows[i].Cells[5].FindControl("ddlLabourCategories")).SelectedItem.Text;
                    var ddl6 = (ComboBox) grdvwLabourBillDetails.Rows[i].Cells[6].FindControl("ddlLabourSubCategories");
                    ddl6.SelectedItem.Text = ds.Tables[0].Rows.Count > i ? ds.Tables[0].Rows[i][6].ToString() : ((ComboBox) grdvwLabourBillDetails.Rows[i].Cells[6].FindControl("ddlLabourSubCategories")).SelectedItem.Text;
                    var txt7 = (TextBox) grdvwLabourBillDetails.Rows[i].Cells[7].FindControl("txtLabourItemDesc");
                    var txt8 = (TextBox) grdvwLabourBillDetails.Rows[i].Cells[8].FindControl("txtLabourQuant");
                    var txt9 = (TextBox) grdvwLabourBillDetails.Rows[i].Cells[9].FindControl("txtLabourBillAmount");
                    dtCurrentTableLabour.Rows[i]["ColLabVendorName"] = ((DropDownList) grdvwLabourBillDetails.Rows[i].Cells[1].FindControl("ddlLabourVendorName")).SelectedItem.Text;
                    dtCurrentTableLabour.Rows[i]["ColLabBillNo"] = txt2.Text;
                    dtCurrentTableLabour.Rows[i]["ColLabBillDate"] = txt3.Text;
                    dtCurrentTableLabour.Rows[i]["Aggregates"] = ((ComboBox) grdvwLabourBillDetails.Rows[i].Cells[4].FindControl("ddlLabourAggregates")).SelectedItem.Text;
                    dtCurrentTableLabour.Rows[i]["Categories"] = ((ComboBox) grdvwLabourBillDetails.Rows[i].Cells[5].FindControl("ddlLabourCategories")).SelectedItem.Text;
                    dtCurrentTableLabour.Rows[i]["Sub_Categories"] = ((ComboBox) grdvwLabourBillDetails.Rows[i].Cells[6].FindControl("ddlLabourSubCategories")).SelectedItem.Text;
                    dtCurrentTableLabour.Rows[i]["ColLabItemDescription"] = txt7.Text;
                    dtCurrentTableLabour.Rows[i]["ColLabQuantity"] = txt8.Text;
                    dtCurrentTableLabour.Rows[i]["Column3"] = txt9.Text;
                    if (i == dtCurrentTableLabour.Rows.Count - 2)
                    {
                        dtCurrentTableLabour.Rows[i + 1]["ColLabBillNo"] = txt2.Text;
                        dtCurrentTableLabour.Rows[i + 1]["ColLabBillDate"] = txt3.Text;
                    }

                    //extract the DropDownList Selected Items
                    var ddl1 = (DropDownList) grdvwLabourBillDetails.Rows[i].Cells[1].FindControl("ddlLabourVendorName");
                    if (((DropDownList) grdvwLabourBillDetails.Rows[i].Cells[1].FindControl("ddlLabourVendorName")).Text == "" || ((DropDownList) grdvwLabourBillDetails.Rows[i].Cells[1].FindControl("ddlLabourVendorName")).Text == string.Empty) _helper.FillDropDownHelperMethodWithDataSet(ds1, "AgencyName", "AgencyId", ddl1);
                    var ddlLabourAggregates = (ComboBox) grdvwLabourBillDetails.Rows[i].Cells[1].FindControl("ddlLabourAggregates");
                    _helper.FillDropDownHelperMethodWithDataSet(dslabourAggregatesnew, "Aggregates", "Aggregate_Id", null, ddlLabourAggregates);
                    var ddlLabourCategories = (ComboBox) grdvwLabourBillDetails.Rows[i].Cells[1].FindControl("ddlLabourCategories");
                    _helper.FillDropDownHelperMethodWithDataSet(dsLabourCategoriesnew, "Categories", "Category_Id", null, ddlLabourCategories);
                    var ddlLabourSubCategories = (ComboBox) grdvwLabourBillDetails.Rows[i].Cells[1].FindControl("ddlLabourSubCategories");
                    _helper.FillDropDownHelperMethodWithDataSet(dsLabourSubCategories, "SubCategories", "SubCategory_Id", null, ddlLabourSubCategories);
                }

                //Rebind the Grid with the current data
                ViewState["LabourCurrentTable"] = dtCurrentTableLabour;
                grdvwLabourBillDetails.DataSource = dtCurrentTableLabour;
                grdvwLabourBillDetails.DataBind();
                pnlBillSummaryDetails.Visible = false;
                btnSave.Enabled = false;
                txtTotalBillAmt.Text = "";
                var count = 0;
                foreach (GridViewRow item in grdvwLabourBillDetails.Rows)
                {
                    if (dtCurrentTableLabour.Rows[count]["Aggregates"].ToString() != string.Empty) ((ComboBox) item.FindControl("ddlLabourAggregates")).Items.FindByText(dtCurrentTableLabour.Rows[count]["Aggregates"].ToString()).Selected = true;
                    if (dtCurrentTableLabour.Rows[count]["Categories"].ToString() != string.Empty) ((ComboBox) item.FindControl("ddlLabourCategories")).Items.FindByText(dtCurrentTableLabour.Rows[count]["Categories"].ToString()).Selected = true;
                    if (dtCurrentTableLabour.Rows[count]["Sub_Categories"].ToString() != string.Empty) ((ComboBox) item.FindControl("ddlLabourSubCategories")).Items.FindByText(dtCurrentTableLabour.Rows[count]["Sub_Categories"].ToString()).Selected = true;
                    count += 1;
                }
            }
        }
        else
        {
            Response.Write("ViewState is null");
        }

        SetPreviousDataLabour();
    }

    private void DisplayLabourBillDetails()
    {
        var dtlabourbilldet = (DataTable) ViewState["LabourBillDetails"];
        var dt = new DataTable();
        //Define the Columns
        dt.Columns.Add(new DataColumn("RowNumberLabour", typeof(string)));
        dt.Columns.Add(new DataColumn("ColLabVendorName", typeof(string)));
        dt.Columns.Add(new DataColumn("ColLabBillNo", typeof(string)));
        dt.Columns.Add(new DataColumn("ColLabBillDate", typeof(string)));
        dt.Columns.Add(new DataColumn("Aggregates", typeof(string)));
        dt.Columns.Add(new DataColumn("Categories", typeof(string)));
        dt.Columns.Add(new DataColumn("Sub_Categories", typeof(string)));
        dt.Columns.Add(new DataColumn("ColLabItemDescription", typeof(string)));
        dt.Columns.Add(new DataColumn("ColLabQuantity", typeof(string)));
        dt.Columns.Add(new DataColumn("Column3", typeof(string)));

        //Add Data on Load
        for (var i = 0; i < dtlabourbilldet.Rows.Count; i++)
        {
            var dr = dt.NewRow();
            dr["RowNumberLabour"] = i + 1;
            dt.Rows.Add(dr);
            dt.Rows[i]["ColLabBillNo"] = dtlabourbilldet.Rows[i]["ColLabBillNo"].ToString();
            dt.Rows[i]["ColLabBillDate"] = dtlabourbilldet.Rows[i]["ColLabBillDate"].ToString();
            dt.Rows[i]["ColLabVendorName"] = dtlabourbilldet.Rows[i]["ColLabVendorName"].ToString();
            dt.Rows[i]["Aggregates"] = dtlabourbilldet.Rows[i]["Aggregates"].ToString();
            dt.Rows[i]["Categories"] = dtlabourbilldet.Rows[i]["Categories"].ToString();
            dt.Rows[i]["Sub_Categories"] = dtlabourbilldet.Rows[i]["Sub_Categories"].ToString();
            dt.Rows[i]["ColLabItemDescription"] = dtlabourbilldet.Rows[i]["ColLabItemDescription"].ToString();
            dt.Rows[i]["ColLabQuantity"] = dtlabourbilldet.Rows[i]["ColLabQuantity"].ToString();
            dt.Rows[i]["Column3"] = dtlabourbilldet.Rows[i]["Column3"].ToString();
        }

        _isedit = true;
        //Store the DataTable in ViewState
        ViewState["LabourCurrentTable"] = dt;
        //Bind the DataTable to the Grid
        grdvwLabourBillDetails.DataSource = dt;
        grdvwLabourBillDetails.DataBind();
    }

    private void SetPreviousDataLabour()
    {
        if (ViewState["LabourCurrentTable"] != null)
        {
            var dt = (DataTable) ViewState["LabourCurrentTable"];
            if (dt.Rows.Count > 0)
            {
                var ds = (DataSet) ViewState["Categories"];
                var ds1 = (DataSet) ViewState["Vendor"];
                var dslabourAggregatesnew = (DataSet) ViewState["LabourAggregates"];
                var dsLabourCategoriesnew = (DataSet) ViewState["LabourCategories"];
                var dsLabourSubCategories = (DataSet) ViewState["LabourSubCategories"];
                for (var i = 0; i < dt.Rows.Count; i++)
                {
                    //Set the Previous Selected Items on Each DropDownList on Postbacks
                    var ddl1 = (DropDownList) grdvwLabourBillDetails.Rows[i].Cells[1].FindControl("ddlLabourVendorName");
                    _helper.FillDropDownHelperMethodWithDataSet(ds1, "AgencyName", "AgencyId", ddl1);
                    var ddlLabourAggregates = (ComboBox) grdvwLabourBillDetails.Rows[i].Cells[1].FindControl("ddlLabourAggregates");
                    _helper.FillDropDownHelperMethodWithDataSet(dslabourAggregatesnew, "Aggregates", "Aggregate_Id", null, ddlLabourAggregates);
                    var ddlLabourCategories = (ComboBox) grdvwLabourBillDetails.Rows[i].Cells[1].FindControl("ddlLabourCategories");
                    _helper.FillDropDownHelperMethodWithDataSet(dsLabourCategoriesnew, "Categories", "Category_Id", null, ddlLabourCategories);
                    var ddlLabourSubCategories = (ComboBox) grdvwLabourBillDetails.Rows[i].Cells[1].FindControl("ddlLabourSubCategories");
                    _helper.FillDropDownHelperMethodWithDataSet(dsLabourSubCategories, "SubCategories", "SubCategory_Id", null, ddlLabourSubCategories);
                    var txt2 = (TextBox) grdvwLabourBillDetails.Rows[i].Cells[2].FindControl("txtLabourBillNo");
                    var txt3 = (TextBox) grdvwLabourBillDetails.Rows[i].Cells[3].FindControl("txtLabourBillDate");
                    var ddl4 = (ComboBox) grdvwLabourBillDetails.Rows[i].Cells[4].FindControl("ddlLabourAggregates");
                    if (ds.Tables[0].Rows.Count > 0) ddl4.SelectedIndex = 0;
                    var ddl5 = (ComboBox) grdvwLabourBillDetails.Rows[i].Cells[5].FindControl("ddlLabourCategories");
                    if (ds.Tables[0].Rows.Count > 0) ddl5.SelectedIndex = 0;
                    var ddl6 = (ComboBox) grdvwLabourBillDetails.Rows[i].Cells[5].FindControl("ddlLabourSubCategories");
                    if (ds.Tables[0].Rows.Count > 0) ddl6.SelectedIndex = 0;
                    var txt7 = (TextBox) grdvwLabourBillDetails.Rows[i].Cells[7].FindControl("txtLabourItemDesc");
                    var txt8 = (TextBox) grdvwLabourBillDetails.Rows[i].Cells[8].FindControl("txtLabourQuant");
                    var txt9 = (TextBox) grdvwLabourBillDetails.Rows[i].Cells[9].FindControl("txtLabourBillAmount");
                    if (ds.Tables[0].Rows.Count > 0) txt9.Text = "";
                    if (i < dt.Rows.Count - 1)
                    {
                        var dvVendorName = new DataView(ds1.Tables[0]) {RowFilter = "AgencyName = '" + dt.Rows[i][1] + "'"};
                        var vendorId = dvVendorName.ToTable().Rows[0]["AgencyId"].ToString();
                        ddl1.SelectedValue = vendorId;
                        txt2.Text = dt.Rows[i]["ColLabBillNo"].ToString();
                        txt3.Text = dt.Rows[i]["ColLabBillDate"].ToString();
                        var dt1 = dslabourAggregatesnew.Tables[0];
                        var dvAggregateName = new DataView(dt1) {RowFilter = "Aggregates = '" + dt.Rows[i][4] + "'"};
                        var aggregateId = dvAggregateName.ToTable().Rows[0]["Aggregate_Id"].ToString();
                        _helper.FillDropDownHelperMethodWithDataSet(dslabourAggregatesnew, "Aggregates", "Aggregate_Id", null, ddl4);
                        foreach (ListItem ltItem in ddl4.Items)
                            if (ltItem.Text == dvAggregateName.ToTable().Rows[0]["Aggregates"].ToString())
                            {
                                ddl4.SelectedValue = aggregateId;
                                break;
                            }

                        var dvCategoryName = new DataView(dsLabourCategoriesnew.Tables[0]) {RowFilter = "Categories = '" + dt.Rows[i][5] + "'"};
                        var categoryId = dvCategoryName.ToTable().Rows[0]["Category_Id"].ToString();
                        ddl5.SelectedValue = categoryId;
                        txt7.Text = dt.Rows[i]["ColLabItemDescription"].ToString();
                        txt8.Text = dt.Rows[i]["ColLabQuantity"].ToString();
                        var dvSubCategoryName = new DataView(dsLabourSubCategories.Tables[0]) {RowFilter = "SubCategories = '" + dt.Rows[i][6] + "'"};
                        var subCategoryId = dvSubCategoryName.ToTable().Rows[0]["SubCategory_Id"].ToString();
                        ddl6.SelectedValue = subCategoryId;
                        txt9.Text = dt.Rows[i]["Column3"].ToString();
                    }
                }
            }
        }
    }

    protected void btnAddNewLabourRow_Click(object sender, EventArgs e)
    {
        AddNewRowToGridLabour();
    }

    protected void btnLabourReset_Click(object sender, EventArgs e)
    {
        SetInitialRowLabour();
    }

    private void AddRowToGridSummary()
    {
        var dt = new DataTable();
        //Define the Columns
        dt.Columns.Add(new DataColumn("TypeBillDetails", typeof(string)));
        dt.Columns.Add(new DataColumn("TotalBillNumbers", typeof(string)));
        dt.Columns.Add(new DataColumn("TotalBillAmount", typeof(string)));
        //Add a Dummy Data on Initial Load
        var dr = dt.NewRow();
        dr["TypeBillDetails"] = "Spare Parts";
        double totalamtsp = 0;
        for (var i = 0; i < grdvwSPBillDetails.Rows.Count; i++)
        {
            var txtbxspamt = grdvwSPBillDetails.Rows[i].FindControl("txtSpareBillAmount") as TextBox;
            if (txtbxspamt != null && txtbxspamt.Text != "") totalamtsp = totalamtsp + Convert.ToDouble(txtbxspamt.Text);
        }

        dr["TotalBillNumbers"] = Math.Abs(totalamtsp) <= 0.0 ? 0.0 : grdvwSPBillDetails.Rows.Count;
        dr["TotalBillAmount"] = totalamtsp;
        dt.Rows.Add(dr);
        //Add a Dummy Data on Initial Load
        dr = dt.NewRow();
        dr["TypeBillDetails"] = "Lubricant";
        double totalamtlubri = 0;
        for (var i = 0; i < grdvwLubricantBillDetails.Rows.Count; i++)
        {
            var txtbxlubriamt = grdvwLubricantBillDetails.Rows[i].FindControl("txtLubricantBillAmount") as TextBox;
            if (txtbxlubriamt != null && txtbxlubriamt.Text != "") totalamtlubri = totalamtlubri + Convert.ToDouble(txtbxlubriamt.Text);
        }

        dr["TotalBillNumbers"] = Math.Abs(totalamtlubri) <= 0.0 ? 0 : grdvwLubricantBillDetails.Rows.Count;
        dr["TotalBillAmount"] = totalamtlubri;
        dt.Rows.Add(dr);

        //Add a Dummy Data on Initial Load
        dr = dt.NewRow();
        dr["TypeBillDetails"] = "Labour Charges";
        double totalamtlabour = 0;
        for (var i = 0; i < grdvwLabourBillDetails.Rows.Count; i++)
        {
            var txtbxlabour = grdvwLabourBillDetails.Rows[i].FindControl("txtLabourBillAmount") as TextBox;
            if (txtbxlabour != null && txtbxlabour.Text != "") totalamtlabour = totalamtlabour + Convert.ToDouble(txtbxlabour.Text);
        }

        dr["TotalBillNumbers"] = Math.Abs(totalamtlabour) <= 0.0 ? 0 : grdvwLabourBillDetails.Rows.Count;
        dr["TotalBillAmount"] = totalamtlabour;
        dt.Rows.Add(dr);

        //Bind the DataTable to the Grid
        grdvwBillDetailsSummary.DataSource = dt;
        grdvwBillDetailsSummary.DataBind();
        txtTotalBillAmt.Text = (totalamtsp + totalamtlubri + totalamtlabour).ToString(CultureInfo.CurrentCulture);
    }

    protected void btnBillDetailsSummary_Click(object sender, EventArgs e)
    {
        if (chkbxlistBillType.Items[0].Selected)
        {
            _fmsVas.SpareVendorName = ((DropDownList) grdvwSPBillDetails.Rows[0].Cells[1].FindControl("ddlSpareVendorName")).SelectedItem.Text;
            _fmsVas.SpareBillNo = ((TextBox) grdvwSPBillDetails.Rows[0].Cells[2].FindControl("txtSpareBillNo")).Text;
            var dsSpareValidation = _fmsVas.SpareValidation();
            switch (dsSpareValidation.Tables[0].Rows[0][0].ToString())
            {
                case "1":
                    Show(" Already Vendor Name :  " + ((DropDownList) grdvwSPBillDetails.Rows[0].Cells[1].FindControl("ddlSpareVendorName")).SelectedItem.Text + "    and Bill Number :   " + ((TextBox) grdvwSPBillDetails.Rows[0].Cells[2].FindControl("txtSpareBillNo")).Text + "   Exists in DataBase ");
                    return;
            }
        }

        if (chkbxlistBillType.Items[1].Selected)
        {
            _fmsVas.LubricantVendorName = ((DropDownList) grdvwLubricantBillDetails.Rows[0].Cells[1].FindControl("ddlLubricantVendorName")).SelectedItem.Text;
            _fmsVas.LubricantBillNo = ((TextBox) grdvwLubricantBillDetails.Rows[0].Cells[2].FindControl("txtLubricantBillNo")).Text;
            var dsLubriValidation = _fmsVas.LubriValidation();
            switch (dsLubriValidation.Tables[0].Rows[0][0].ToString())
            {
                case "1":
                    Show(" Already Vendor Name :   " + ((DropDownList) grdvwLubricantBillDetails.Rows[0].Cells[1].FindControl("ddlLubricantVendorName")).SelectedItem.Text + "    and Bill Number:   " + ((TextBox) grdvwLubricantBillDetails.Rows[0].Cells[2].FindControl("txtLubricantBillNo")).Text + "   Exists in DataBase ");
                    return;
            }
        }

        if (chkbxlistBillType.Items[2].Selected)
        {
            _fmsVas.LabourVendorName = ((DropDownList) grdvwLabourBillDetails.Rows[0].Cells[1].FindControl("ddlLabourVendorName")).SelectedItem.Text;
            _fmsVas.LabourBillNo = ((TextBox) grdvwLabourBillDetails.Rows[0].Cells[2].FindControl("txtLabourBillNo")).Text;
            var dsLabValidation = _fmsVas.LabValidation();
            switch (dsLabValidation.Tables[0].Rows[0][0].ToString())
            {
                case "1":
                    Show(" Already Vendor Name :    " + ((DropDownList) grdvwLabourBillDetails.Rows[0].Cells[1].FindControl("ddlLabourVendorName")).SelectedItem.Text + "and Bill Number :    " + ((TextBox) grdvwLabourBillDetails.Rows[0].Cells[2].FindControl("txtLabourBillNo")).Text + " Exists in DataBase ");
                    return;
            }
        }

        if (ValidateSpGrid() && ValidateLubriGrid() && ValidateLabourGrid())
        {
            pnlBillSummaryDetails.Visible = true;
            AddRowToGridSummary();
            btnSave.Enabled = true;
        }
    }

    protected void chkbxlistBillType_SelectedIndexChanged(object sender, EventArgs e)
    {
        _isedit = false;
        pnlSPBillDetails.Visible = false;
        pnlLubricantBillDetails.Visible = false;
        pnlLabourBillDetails.Visible = false;
        pnlBillSummaryDetails.Visible = false;
        pnlBillDetailsSummaryBtn.Visible = false;
        if (chkbxlistBillType.Items[0].Selected)
        {
            pnlSPBillDetails.Visible = true;
            var ddlSpareItemDesc = grdvwSPBillDetails.Rows[0].FindControl("ddlSpareItemDesc") as DropDownList;
            if (ddlSpareItemDesc != null && ddlSpareItemDesc.SelectedIndex == 0 && ddlSpareItemDesc.Items.Count == 1)
            {
                var vehicleNumber = ddlVehicleNumber.SelectedItem.ToString();
                var ds2 = _vehMain.GetSpareParts(vehicleNumber);
                if (ds2 != null) _helper.FillDropDownHelperMethodWithDataSet(ds2, "SparePart_Name", "ManufacturerSpare_Id", ddlSpareItemDesc);
            }

            //SetInitialRowSP();
        }
        else
        {
            SetInitialRowSp();
        }

        if (chkbxlistBillType.Items[1].Selected)
            pnlLubricantBillDetails.Visible = true;
        else
            SetInitialRowLubricant();
        if (chkbxlistBillType.Items[2].Selected && !pnlLabourBillDetails.Visible)
        {
            Checkboxcount = true;
            pnlLabourBillDetails.Visible = true;
            if (ddlDistrict.SelectedIndex != 0)
            {
                var vehicleNo = Convert.ToString(ddlVehicleNumber.SelectedItem.Text);
                var ds = _vehMain.getcatedetails(vehicleNo);
                grdvwLabourBillDetails.DataSource = ds.Tables[0];
                grdvwLabourBillDetails.DataBind();
                ViewState["Categories"] = ds;
            }

            SetInitialRowLabour();
        }
        else if (chkbxlistBillType.Items[2].Selected && !pnlLabourBillDetails.Visible)
        {
            pnlLabourBillDetails.Visible = true;
        }

        if (chkbxlistBillType.Items[0].Selected || chkbxlistBillType.Items[1].Selected || chkbxlistBillType.Items[2].Selected) pnlBillDetailsSummaryBtn.Visible = true;
        txtTotalBillAmt.Text = "";
    }

    public void GetVendors()
    {
    }

    protected void chkAmount_CheckedChanged(object sender, EventArgs e)
    {
        SetInitialRowSp();
        SetInitialRowLubricant();
        SetInitialRowLabour();
        if (!chkAmount.Checked)
        {
            btnSave.Enabled = false;
            chkbxlistBillType.Enabled = true;
        }
        else
        {
            foreach (ListItem item in chkbxlistBillType.Items) item.Selected = false;
            btnSave.Enabled = true;
            chkbxlistBillType.Enabled = false;
            pnlSPBillDetails.Visible = false;
            pnlLubricantBillDetails.Visible = false;
            pnlLabourBillDetails.Visible = false;
            pnlBillSummaryDetails.Visible = false;
            pnlBillDetailsSummaryBtn.Visible = false;
        }

        if (btnSave.Text == "Update" && chkAmount.Checked) btnSave.Enabled = true;
    }

    private bool ValidateSpGrid()
    {
        if (pnlSPBillDetails.Visible)
            for (var i = 0; i < grdvwSPBillDetails.Rows.Count; i++)
            {
                var ddlVendorName = grdvwSPBillDetails.Rows[i].FindControl("ddlSpareVendorName") as DropDownList;
                if (ddlVendorName != null && ddlVendorName.SelectedIndex == 0)
                {
                    Show("Please Select Spare Parts Vendor Name");
                    return false;
                }

                var txtbxspbillno = grdvwSPBillDetails.Rows[i].FindControl("txtSpareBillNo") as TextBox;
                if (txtbxspbillno != null && txtbxspbillno.Text == "")
                {
                    Show("Please enter Spare Parts Bill Number");
                    return false;
                }

                var txtbxspdate = grdvwSPBillDetails.Rows[i].FindControl("txtSpareBillDate") as TextBox;
                if (txtbxspdate != null && txtbxspdate.Text == "")
                {
                    Show("Please enter Spare Parts Bill Date");
                    return false;
                }

                if (txtbxspdate != null && Convert.ToDateTime(txtbxspdate.Text) > DateTime.Now)
                {
                    Show("Spare Parts Bill Date should be less than Current Date");
                    return false;
                }

                var txtSpareEmrIpc = grdvwSPBillDetails.Rows[i].FindControl("txtSpareEMRIpc") as TextBox;
                if (txtSpareEmrIpc != null && txtSpareEmrIpc.Text == "")
                {
                    Show("Please enter Spare Parts EMRI Part Code");
                    return false;
                }

                var txtSparePartCode = grdvwSPBillDetails.Rows[i].FindControl("txtSparePartCode") as TextBox;
                if (txtSparePartCode != null && txtSparePartCode.Text == "")
                {
                    Show("Please enter Spare Parts Part Code");
                    return false;
                }

                var ddlSpareItemDesc = grdvwSPBillDetails.Rows[i].FindControl("ddlSpareItemDesc") as DropDownList;
                if (ddlSpareItemDesc != null && ddlSpareItemDesc.SelectedIndex == 0)
                {
                    Show("Please Select Spare Item Description");
                    return false;
                }

                var txtbxspamt = grdvwSPBillDetails.Rows[i].FindControl("txtSpareBillAmount") as TextBox;
                if (txtbxspamt != null && txtbxspamt.Text == "")
                {
                    Show("Please enter Spare Parts Bill Amount");
                    return false;
                }
            }

        return true;
    }

    private bool ValidateLubriGrid()
    {
        if (pnlLubricantBillDetails.Visible)
            for (var i = 0; i < grdvwLubricantBillDetails.Rows.Count; i++)
            {
                var ddlLubricantVendorName = grdvwLubricantBillDetails.Rows[i].FindControl("ddlLubricantVendorName") as DropDownList;
                if (ddlLubricantVendorName != null && ddlLubricantVendorName.SelectedIndex == 0)
                {
                    Show("Please Select Lubricant Vendor Name");
                    return false;
                }

                var txtbxlubribillno = grdvwLubricantBillDetails.Rows[i].FindControl("txtLubricantBillNo") as TextBox;
                if (txtbxlubribillno != null && txtbxlubribillno.Text == "")
                {
                    Show("Please enter Lubricant Bill Number");
                    return false;
                }

                var txtbxlubridate = grdvwLubricantBillDetails.Rows[i].FindControl("txtLubricantBillDate") as TextBox;
                if (txtbxlubridate != null && txtbxlubridate.Text == "")
                {
                    Show("Please enter Lubricant Bill Date");
                    return false;
                }

                if (txtbxlubridate != null && Convert.ToDateTime(txtbxlubridate.Text) > DateTime.Now)
                {
                    Show("Lubricant Bill Date should be less than Current Date");
                    return false;
                }

                var txtLubricantEmrIpc = grdvwLubricantBillDetails.Rows[i].FindControl("txtLubricantEMRIpc") as TextBox;
                if (txtLubricantEmrIpc != null && txtLubricantEmrIpc.Text == "")
                {
                    Show("Please enter Lubricant EMRI Part Code");
                    return false;
                }

                var txtLubricantPartCode = grdvwLubricantBillDetails.Rows[i].FindControl("txtLubricantPartCode") as TextBox;
                if (txtLubricantPartCode != null && txtLubricantPartCode.Text == "")
                {
                    Show("Please enter Lubricant Part Code");
                    return false;
                }

                var txtLubricantItemDesc = grdvwLubricantBillDetails.Rows[i].FindControl("txtLubricantItemDesc") as TextBox;
                if (txtLubricantItemDesc != null && txtLubricantItemDesc.Text == "")
                {
                    Show("Please enter Lubricant Item Description");
                    return false;
                }

                var txtbxlubriamt = grdvwLubricantBillDetails.Rows[i].FindControl("txtLubricantBillAmount") as TextBox;
                if (txtbxlubriamt != null && txtbxlubriamt.Text == "")
                {
                    Show("Please enter Lubricant Bill Amount");
                    return false;
                }
            }

        return true;
    }

    private bool ValidateLabourGrid()
    {
        if (pnlLabourBillDetails.Visible)
            for (var i = 0; i < grdvwLabourBillDetails.Rows.Count; i++)
            {
                var ddlLabourVendorName = grdvwLabourBillDetails.Rows[i].FindControl("ddlLabourVendorName") as DropDownList;
                if (ddlLabourVendorName != null && ddlLabourVendorName.SelectedIndex == 0)
                {
                    Show("Please Select Labour Vendor Name");
                    return false;
                }

                var txtbxlabourbillno = grdvwLabourBillDetails.Rows[i].FindControl("txtLabourBillNo") as TextBox;
                if (txtbxlabourbillno != null && txtbxlabourbillno.Text == "")
                {
                    Show("Please enter Labour Bill Number");
                    return false;
                }

                var txtbxlabourdate = grdvwLabourBillDetails.Rows[i].FindControl("txtLabourBillDate") as TextBox;
                if (txtbxlabourdate != null && txtbxlabourdate.Text == "")
                {
                    Show("Please enter Labour Bill Date");
                    return false;
                }

                if (txtbxlabourdate != null && Convert.ToDateTime(txtbxlabourdate.Text) > DateTime.Now)
                {
                    Show("Labour Bill Date should be less than Current Date");
                    return false;
                }

                var ddlLabourAggregates = grdvwLabourBillDetails.Rows[i].FindControl("ddlLabourAggregates") as ComboBox;
                if (ddlLabourAggregates != null && ddlLabourAggregates.SelectedIndex == 0)
                {
                    Show("Please Select Labour Aggregate");
                    return false;
                }

                var ddlLabourCategories = grdvwLabourBillDetails.Rows[i].FindControl("ddlLabourCategories") as ComboBox;
                if (ddlLabourCategories != null && ddlLabourCategories.SelectedIndex == 0)
                {
                    Show("Please Select Labour Categories");
                    return false;
                }

                var ddlLabourSubCategories = grdvwLabourBillDetails.Rows[i].FindControl("ddlLabourSubCategories") as ComboBox;
                if (ddlLabourSubCategories != null && ddlLabourSubCategories.SelectedIndex == 0)
                {
                    Show("Please Select Labour SubCategories");
                    return false;
                }

                var txtLabourItemDesc = grdvwLabourBillDetails.Rows[i].FindControl("txtLabourItemDesc") as TextBox;
                if (txtLabourItemDesc != null && txtLabourItemDesc.Text == "")
                {
                    Show("Please enter Labour Item Description");
                    return false;
                }

                var txtbxlabouramt = grdvwLabourBillDetails.Rows[i].FindControl("txtLabourBillAmount") as TextBox;
                if (txtbxlabouramt != null && txtbxlabouramt.Text == "")
                {
                    Show("Please enter Labour Bill Amount");
                    return false;
                }
            }

        return true;
    }

    protected void grdvwSPBillDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            var ddlSpareVendorName = e.Row.FindControl("ddlSpareVendorName") as DropDownList;
            var ddlSpareItemDesc = e.Row.FindControl("ddlSpareItemDesc") as DropDownList;
            var ds = _vehMain.IFillVendorsMaintenance();
            var vehicleNumber = ddlVehicleNumber.SelectedValue;
            var ds2 = _vehMain.GetSpareParts(vehicleNumber);
            if (ds != null)
            {
                _helper.FillDropDownHelperMethodWithDataSet(ds, "AgencyName", "AgencyId", ddlSpareVendorName);
                ViewState["Vendor"] = ds;
            }

            if (ds2 != null)
            {
                _helper.FillDropDownHelperMethodWithDataSet(ds2, "SparePart_Name", "ManufacturerSpare_Id", ddlSpareItemDesc);
                ViewState["SpareItemDesc"] = ds2;
            }

            if (_isedit)
            {
                var dsVehNo = _vehMain.GetSpareParts(txtVehicleNumber.Text);
                if (dsVehNo != null) _helper.FillDropDownHelperMethodWithDataSet(dsVehNo, "SparePart_Name", "ManufacturerSpare_Id", ddlSpareItemDesc);
                if (ds != null)
                {
                    var dv = ds.Tables[0].DefaultView;
                    dv.RowFilter = "AgencyName='" + Convert.ToString(((DataRowView) e.Row.DataItem).Row.ItemArray[1]) + "'";
                    var selectedId = Convert.ToString(dv.ToTable().Rows[0]["AgencyId"]);
                    ((DropDownList) e.Row.FindControl("ddlSpareVendorName")).SelectedValue = selectedId;
                }

                if (dsVehNo != null)
                {
                    var dv1 = dsVehNo.Tables[0].DefaultView;
                    dv1.RowFilter = "SparePart_Name='" + Convert.ToString(((DataRowView) e.Row.DataItem).Row.ItemArray[6]) + "'";
                    var sparePartId = Convert.ToString(dv1.ToTable().Rows[0]["ManufacturerSpare_Id"]);
                    ((DropDownList) e.Row.FindControl("ddlSpareItemDesc")).SelectedValue = sparePartId;
                }
            }
        }
    }

    protected void grdvwLubricantBillDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    var ddlLubricantVendorName = e.Row.FindControl("ddlLubricantVendorName") as DropDownList;
                    var ds = _vehMain.IFillVendorsMaintenance();
                    if (ds != null && ddlLubricantVendorName != null)
                    {
                        _helper.FillDropDownHelperMethodWithDataSet(ds, "AgencyName", "AgencyId", ddlLubricantVendorName);
                        ViewState["Vendor"] = ds;
                    }

                    if (_isedit && ds != null)
                    {
                        var dv = ds.Tables[0].DefaultView;
                        dv.RowFilter = "AgencyName='" + Convert.ToString(((DataRowView) e.Row.DataItem).Row.ItemArray[4]) + "'";
                        var selectedId = Convert.ToString(dv.ToTable().Rows[0]["AgencyId"]);
                        ((DropDownList) e.Row.FindControl("ddlLubricantVendorName")).SelectedValue = selectedId;
                    }

                    break;
            }
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    protected void grdvwLabourBillDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (grdvwLabourBillDetails.Rows.Count != -1)
            {
                var ddlLabourVendorName = e.Row.FindControl("ddlLabourVendorName") as DropDownList;
                var ds = _vehMain.IFillVendorsMaintenance();
                if (ds != null)
                {
                    _helper.FillDropDownHelperMethodWithDataSet(ds, "AgencyName", "AgencyId", ddlLabourVendorName);
                    ViewState["Vendor"] = ds;
                }

                _dslabourAggregates = new DataSet();
                _dslabourAggregates = _vehallobj.GetAggregates();
                var ddlLabourAggregates = e.Row.FindControl("ddlLabourAggregates") as ComboBox;
                if (_dslabourAggregates != null)
                {
                    _helper.FillDropDownHelperMethodWithDataSet(_dslabourAggregates, "Aggregates", "Aggregate_Id", null, ddlLabourAggregates);
                    ViewState["LabourAggregates"] = _dslabourAggregates;
                }

                _dsLabourCategories = new DataSet();
                _dsLabourCategories = _vehallobj.GetCategoriesMaintenance();
                var ddlLabourCategories = e.Row.FindControl("ddlLabourCategories") as ComboBox;
                if (_dsLabourCategories != null)
                {
                    _helper.FillDropDownHelperMethodWithDataSet(_dsLabourCategories, "Categories", "Category_Id", null, ddlLabourCategories);
                    ViewState["LabourCategories"] = _dsLabourCategories;
                }

                _dsLabourSubCategories = new DataSet();
                _dsLabourSubCategories = _vehallobj.GetSubcategoriesMaintenance();
                var ddlLabourSubCategories = e.Row.FindControl("ddlLabourSubCategories") as ComboBox;
                if (_dsLabourSubCategories != null)
                {
                    _helper.FillDropDownHelperMethodWithDataSet(_dsLabourSubCategories, "SubCategories", "SubCategory_Id", null, ddlLabourSubCategories);
                    ViewState["LabourSubCategories"] = _dsLabourSubCategories;
                }
            }

            if (_isedit)
            {
                var ddlLabourVendorName = e.Row.FindControl("ddlLabourVendorName") as DropDownList;
                var ds = _vehMain.IFillVendorsMaintenance();
                if (ds != null) _helper.FillDropDownHelperMethodWithDataSet(ds, "AgencyName", "AgencyId", ddlLabourVendorName);
                if (ds != null)
                {
                    var dv = new DataView(ds.Tables[0]) {RowFilter = "AgencyName='" + Convert.ToString(((DataRowView) e.Row.DataItem).Row.ItemArray[1]) + "'"};
                    if (dv.Count > 0)
                    {
                        var selectedId = Convert.ToString(dv.ToTable().Rows[0]["AgencyId"]);
                        ((DropDownList) e.Row.FindControl("ddlLabourVendorName")).SelectedValue = selectedId;
                    }
                    else
                    {
                        return;
                    }
                }

                _dslabourAggregates = new DataSet();
                _dslabourAggregates = _vehallobj.GetAggregates();
                var ddlLabourAggregates = e.Row.FindControl("ddlLabourAggregates") as ComboBox;
                if (_dslabourAggregates != null) _helper.FillDropDownHelperMethodWithDataSet(_dslabourAggregates, "Aggregates", "Aggregate_Id", null, ddlLabourAggregates);
                if (_dslabourAggregates != null)
                {
                    var dv1 = new DataView(_dslabourAggregates.Tables[0]) {RowFilter = "Aggregates='" + Convert.ToString(((DataRowView) e.Row.DataItem).Row.ItemArray[4]) + "'"};
                    if (dv1.Count > 0)
                    {
                        var selectedId1 = Convert.ToString(dv1.ToTable().Rows[0]["Aggregate_Id"]);
                        ((ComboBox) e.Row.FindControl("ddlLabourAggregates")).SelectedValue = selectedId1;
                    }
                    else
                    {
                        return;
                    }
                }

                _dsLabourCategories = new DataSet();
                _dsLabourCategories = _vehallobj.GetCategoriesMaintenance();
                var ddlLabourCategories = e.Row.FindControl("ddlLabourCategories") as ComboBox;
                if (_dsLabourCategories != null) _helper.FillDropDownHelperMethodWithDataSet(_dsLabourCategories, "Categories", "Category_Id", null, ddlLabourCategories);
                if (_dsLabourCategories != null)
                {
                    var dv2 = new DataView(_dsLabourCategories.Tables[0]) {RowFilter = "Categories='" + Convert.ToString(((DataRowView) e.Row.DataItem).Row.ItemArray[5]) + "'"};
                    if (dv2.Count > 0)
                    {
                        var selectedId2 = Convert.ToString(dv2.ToTable().Rows[0]["Category_Id"]);
                        ((ComboBox) e.Row.FindControl("ddlLabourCategories")).SelectedValue = selectedId2;
                    }
                    else
                    {
                        return;
                    }
                }

                _dsLabourSubCategories = new DataSet();
                _dsLabourSubCategories = _vehallobj.GetSubcategoriesMaintenance();
                var ddlLabourSubCategories = e.Row.FindControl("ddlLabourSubCategories") as ComboBox;
                if (_dsLabourSubCategories != null)
                {
                    _helper.FillDropDownHelperMethodWithDataSet(_dsLabourSubCategories, "SubCategories", "SubCategory_Id", null, ddlLabourSubCategories);
                    ViewState["LabourSubCategories"] = _dsLabourSubCategories;
                }

                if (_dsLabourSubCategories != null)
                {
                    var dv3 = _dsLabourSubCategories.Tables[0].DefaultView;
                    dv3.RowFilter = "SubCategories='" + Convert.ToString(((DataRowView) e.Row.DataItem).Row.ItemArray[6]) + "'";
                    if (dv3.Count > 0)
                    {
                        var selectedId3 = Convert.ToString(dv3.ToTable().Rows[0]["SubCategory_Id"]);
                        ((ComboBox) e.Row.FindControl("ddlLabourSubCategories")).SelectedValue = selectedId3;
                    }
                }
            }
        }
    }

    protected void gvVehicleMaintenanceDetails_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
}