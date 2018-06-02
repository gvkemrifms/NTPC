using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using GvkFMSAPP.BLL;

public partial class ScheduleServiceMaster : Page
{
    private readonly Helper _helper = new Helper();
    private readonly VAS _obj = new VAS();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Name"] == null) Response.Redirect("Login.aspx");
        if (!IsPostBack)
        {
            BindGrid();
            btnUpdate.Visible = false;
            BindData();
            txtSSAlert5.Enabled = false;
        }
    }

    private void BindGrid()
    {
        var ds = _obj.GetSSMaster();
        if (ds == null) throw new ArgumentNullException(nameof(ds));
        gvScheduleServiceMaster.DataSource = ds.Tables[0];
        gvScheduleServiceMaster.DataBind();
    }

    private void BindData()
    {
        try
        {
            var ds = _obj.GetManufaturerName();
            _helper.FillDropDownHelperMethodWithDataSet(ds, "FleetManufacturer_Name", "FleetManufacturer_Id", ddlManufactName,null,null,null,"1");
            ViewState["ManufaturerName"] = ds;
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        _obj.ManufacturerId = Convert.ToInt16(ddlManufactName.SelectedValue);
        _obj.ManufacturerName = ddlManufactName.SelectedItem.Text;
        _obj.GSAlert = txtGeneralService.Text == string.Empty ? (long?) null : Convert.ToInt64(txtGeneralService.Text);
        _obj.ScheduleServ1 = txtSSAlert1.Text == string.Empty ? (long?) null : Convert.ToInt64(txtSSAlert1.Text);
        _obj.ScheduleServ2 = txtSSAlert2.Text == string.Empty ? (long?) null : Convert.ToInt64(txtSSAlert2.Text);
        _obj.ScheduleServ3 = txtSSAlert3.Text == string.Empty ? (long?) null : Convert.ToInt64(txtSSAlert3.Text);
        _obj.ScheduleServ4 = txtSSAlert4.Text == string.Empty ? (long?) null : Convert.ToInt64(txtSSAlert4.Text);
        _obj.ScheduleServ5 = txtSSAlert5.Text == string.Empty ? (long?) null : Convert.ToInt64(txtSSAlert5.Text);
        var i = _obj.SaveScheduleServices();
        switch (i)
        {
            case 0:
                Show("Saving was Unsuccessfull");
                break;
            default:
                Show("Scheduling services Master saved successfully");
                ddlManufactName.SelectedIndex = 0;
                txtGeneralService.Text = "";
                txtSSAlert1.Text = "";
                txtSSAlert2.Text = "";
                txtSSAlert3.Text = "";
                txtSSAlert4.Text = "";
                txtSSAlert5.Text = "";
                txtSSAlert5.Enabled = false;
                BindGrid();
                break;
        }
    }

    public void Show(string message)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('" + message + "');", true);
    }

    protected void gvScheduleServiceMaster_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        switch (e.CommandName.ToUpper())
        {
            case "EDITSERVICE":
                btnSave.Visible = false;
                btnUpdate.Visible = true;
                var index = Convert.ToInt32(e.CommandArgument.ToString());
                var ds = (DataSet) ViewState["ManufaturerName"];
                var dv = ds.Tables[0].DefaultView;
                dv.RowFilter = "FleetManufacturer_Name='" + ((Label) gvScheduleServiceMaster.Rows[index].FindControl("lblManufacturerName")).Text + "'";
                ddlManufactName.SelectedValue = Convert.ToString(dv.ToTable().Rows[0]["FleetManufacturer_Id"]);
                ddlManufactName.Enabled = false;
                if (ddlManufactName.SelectedValue == "68") ddlManufactName_SelectedIndexChanged(this, null);
                txtGeneralService.Text = ((Label) gvScheduleServiceMaster.Rows[index].FindControl("lblGSAlert")).Text;
                txtSSAlert1.Text = ((Label) gvScheduleServiceMaster.Rows[index].FindControl("lblAlert1")).Text;
                txtSSAlert2.Text = ((Label) gvScheduleServiceMaster.Rows[index].FindControl("lblAlert2")).Text;
                txtSSAlert3.Text = ((Label) gvScheduleServiceMaster.Rows[index].FindControl("lblAlert3")).Text;
                txtSSAlert4.Text = ((Label) gvScheduleServiceMaster.Rows[index].FindControl("lblAlert4")).Text;
                txtSSAlert5.Text = ((Label) gvScheduleServiceMaster.Rows[index].FindControl("lblAlert5")).Text;
                break;
        }
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        _obj.ManufacturerId = Convert.ToInt16(ddlManufactName.SelectedValue);
        _obj.ManufacturerName = ddlManufactName.SelectedItem.Text;
        _obj.GSAlert = txtGeneralService.Text == string.Empty ? (long?) null : Convert.ToInt64(txtGeneralService.Text);
        _obj.ScheduleServ1 = txtGeneralService.Text == string.Empty ? (long?) null : Convert.ToInt64(txtSSAlert1.Text);
        _obj.ScheduleServ2 = txtSSAlert2.Text == string.Empty ? (long?) null : Convert.ToInt64(txtSSAlert2.Text);
        _obj.ScheduleServ3 = txtSSAlert3.Text == string.Empty ? (long?) null : Convert.ToInt64(txtSSAlert3.Text);
        _obj.ScheduleServ4 = txtSSAlert4.Text == string.Empty ? (long?) null : Convert.ToInt64(txtSSAlert4.Text);
        _obj.ScheduleServ5 = txtSSAlert5.Text == string.Empty ? (long?) null : Convert.ToInt64(txtSSAlert5.Text);
        var i = _obj.UpdateScheduleServices();
        switch (i)
        {
            case 0:
                Show("Updation was Unsuccessfull");
                break;
            default:
                Show("Scheduling services Master updated successfully");
                ddlManufactName.SelectedIndex = 0;
                btnSave.Visible = true;
                ddlManufactName.Enabled = true;
                btnUpdate.Visible = false;
                txtGeneralService.Text = "";
                txtSSAlert1.Text = "";
                txtSSAlert2.Text = "";
                txtSSAlert3.Text = "";
                txtSSAlert4.Text = "";
                txtSSAlert5.Text = "";
                txtSSAlert5.Enabled = false;
                BindGrid();
                break;
        }
    }

    protected void ddlManufactName_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtSSAlert5.Enabled = ddlManufactName.SelectedItem.ToString() == "TATA MOTORS LIMITED";
    }
}