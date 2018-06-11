using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using GvkFMSAPP.BLL.VAS_BLL;
using GvkFMSAPP.PL;
public partial class SparePartsMaster : Page
{
    private readonly FleetMaster _fleetMaster = new FleetMaster();
    private readonly Helper _helper = new Helper();
    private readonly VASGeneral _obj = new VASGeneral();

    protected void Page_Load(object sender,EventArgs e)
    {
        if (Session["User_Name"] == null) Response.Redirect("Login.aspx");
        if (!IsPostBack)
        {
            gvSpareParts.Columns[0].Visible = false;
            btSave.Attributes.Add("onclick","return validation()");
            FillGridSpareParts();
            var dsPerms = (DataSet) Session["PermissionsDS"];
            dsPerms.Tables[0].DefaultView.RowFilter = "Url='" + Page.Request.Url.Segments[Page.Request.Url.Segments.Length - 1] + "'";
            var p = new PagePermissions(dsPerms,dsPerms.Tables[0].DefaultView[0]["Url"].ToString(),dsPerms.Tables[0].DefaultView[0]["Title"].ToString());
            BindManufacturerName();
            if (p.Modify)
            {
                gvSpareParts.Visible = true;
                gvSpareParts.Columns[8].Visible = false;
            }
        }
    }

    private void BindManufacturerName()
    {
        try
        {
            var ds = _obj.GetManufacturerName();
            if (ds == null) throw new ArgumentNullException(nameof(ds));
            _helper.FillDropDownHelperMethodWithDataSet(ds,"FleetManufacturer_Name","FleetManufacturer_Id",ddlManufacturerID,null,null,null,"1");
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    private void FillGridSpareParts()
    {
        var ds = _fleetMaster.FillGridSpareParts();
        if (ds == null) throw new ArgumentNullException(nameof(ds));
        gvSpareParts.DataSource = ds;
        gvSpareParts.DataBind();
    }

    protected void gvSpareParts_RowEditing(object sender,GridViewEditEventArgs e)
    {
        btSave.Text = "Update";
        var index = e.NewEditIndex;
        var lblId = (Label) gvSpareParts.Rows[index].FindControl("lblId");
        hidSpareId.Value = lblId.Text;
        int spId = Convert.ToInt16(hidSpareId.Value);
        var ds = _fleetMaster.EditSpareParts(spId);
        txtSparePartID.Text = hidSpareId.Value;
        txtSparePartName.Text = ds.Tables[0].Rows[0]["SparePart_Name"].ToString();
        txtManufacturerSpareID.Text = ds.Tables[0].Rows[0]["ManufacturerSpare_Id"].ToString();
        ddlManufacturerID.SelectedValue = ds.Tables[0].Rows[0]["Manufacturer_Id"].ToString();
        txtSparePartGroupID.Text = ds.Tables[0].Rows[0]["SparePart_Group_Id"].ToString();
        txtGroupName.Text = ds.Tables[0].Rows[0]["Group_Name"].ToString();
        var cGrade = Convert.ToString(ds.Tables[0].Rows[0]["Cost"].ToString()).Split('.');
        txtCost.Text = cGrade[0] + '.' + cGrade[1].Substring(0,2);
    }

    protected void gvSpareParts_RowDeleting(object sender,GridViewDeleteEventArgs e)
    {
        ClearFields();
        var index = e.RowIndex;
        var lblId = (Label) gvSpareParts.Rows[index].FindControl("lblId");
        hidSpareId.Value = lblId.Text;
        int spId = Convert.ToInt16(hidSpareId.Value);
        var result = _fleetMaster.DeleteSpareParts(spId);
        Show(result == 1 ? "Spare Part Deactivated Successfully" : "Failed To DeActivate");
        FillGridSpareParts();
    }

    protected void gvSpareParts_PageIndexChanging(object sender,GridViewPageEventArgs e)
    {
        gvSpareParts.PageIndex = e.NewPageIndex;
        var ds = _fleetMaster.FillGridSpareParts();
        if (ds == null) throw new ArgumentNullException(nameof(ds));
        gvSpareParts.DataSource = ds;
        gvSpareParts.DataBind();
    }

    protected void btSave_Click(object sender,EventArgs e)
    {
        switch (btSave.Text)
        {
            case "Save":
            {
                var ds = _fleetMaster.FillGridSpareParts();
                if (ds == null) throw new ArgumentNullException(nameof(ds));
                if (ds.Tables[0].Select("ManufacturerSpare_Id='" + txtManufacturerSpareID.Text + "'").Length > 0)
                    Show("Manufacturer Spare Id already exists");
                else
                    InsertSparePart(Convert.ToString(txtSparePartName.Text),Convert.ToInt32(txtManufacturerSpareID.Text),Convert.ToInt32(ddlManufacturerID.SelectedValue),Convert.ToInt32(txtSparePartGroupID.Text),Convert.ToString(txtGroupName.Text),Convert.ToDecimal(txtCost.Text));
                break;
            }
            default:
            {
                var ds = _fleetMaster.FillGridSpareParts();
                if (ds == null) throw new ArgumentNullException(nameof(ds));
                if (ds.Tables[0].Select("SparePart_Name='" + txtSparePartName.Text + "' And SparePart_Id<>'" + txtSparePartID.Text + "'").Length > 0)
                    Show("Spare Part Name already exists");
                else
                    UpdateSparePart(Convert.ToInt32(txtSparePartID.Text),Convert.ToString(txtSparePartName.Text),Convert.ToInt32(txtManufacturerSpareID.Text),Convert.ToInt32(ddlManufacturerID.SelectedValue),Convert.ToInt32(txtSparePartGroupID.Text),Convert.ToString(txtGroupName.Text),Convert.ToDecimal(txtCost.Text));
                break;
            }
        }

        FillGridSpareParts();
    }

    private void UpdateSparePart(int sparePartId,string spareName,int manufacturerSpareId,int manufacturerId,int sparePartGroupId,string groupName,decimal cost)
    {
        var res = _fleetMaster.UpdateSpareParts(sparePartId,spareName,manufacturerSpareId,manufacturerId,sparePartGroupId,groupName,cost);
        switch (res)
        {
            case 1:
                Show("Spare Parts Updated Successfully");
                ClearFields();
                break;
            default:
                Show("Failure please try later ");
                break;
        }
    }

    private void InsertSparePart(string spareName,int manufacturerSpareId,int manufacturerId,int sparePartGroupId,string groupName,decimal cost)
    {
        var res = _fleetMaster.InsertSpareParts(spareName,manufacturerSpareId,manufacturerId,sparePartGroupId,groupName,cost);
        switch (res)
        {
            case 1:
                Show("Spare Parts Added Successfully");
                ClearFields();
                break;
            default:
                Show("Failure please try later ");
                break;
        }
    }

    protected void btReset_Click(object sender,EventArgs e)
    {
        ClearFields();
    }

    private void ClearFields()
    {
        btSave.Text = "Save";
        txtGroupName.Text = "";
        ddlManufacturerID.SelectedIndex = 0;
        txtManufacturerSpareID.Text = "";
        txtSparePartGroupID.Text = "";
        txtSparePartID.Text = "";
        txtSparePartName.Text = "";
        txtCost.Text = "";
    }

    public void Show(string message)
    {
        ScriptManager.RegisterStartupScript(this,GetType(),"msg","alert('" + message + "');",true);
    }
}