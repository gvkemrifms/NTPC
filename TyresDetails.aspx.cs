using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using GvkFMSAPP.PL;
public partial class TyresDetails : Page
{
    private readonly FleetMaster _fleetMaster = new FleetMaster();

    protected void Page_Load(object sender,EventArgs e)
    {
        if (Session["User_Name"] == null) Response.Redirect("Login.aspx");
        if (!IsPostBack)
        {
            grvTyresDetails.Columns[0].Visible = false;
            FillGrid_TyresDetails();
            txtTyreItemCode.Attributes.Add("onkeypress","javascript:return OnlyAlphaNumeric(this,event)");
            txtTyreNumber.Attributes.Add("onkeypress","javascript:return OnlyAlphaNumeric(this,event)");
            txtTyreMake.Attributes.Add("onkeypress","javascript:return OnlyAlphabets(this,event)");
            txtTyreModel.Attributes.Add("onkeypress","javascript:return OnlyAlphaNumeric(this,event)");
            txtTyreSize.Attributes.Add("onkeypress","javascript:return OnlyNumbers(this,event)");
            //Permissions
            var dsPerms = (DataSet) Session["PermissionsDS"];
            dsPerms.Tables[0].DefaultView.RowFilter = "Url='" + Page.Request.Url.Segments[Page.Request.Url.Segments.Length - 1] + "'";
            var p = new PagePermissions(dsPerms,dsPerms.Tables[0].DefaultView[0]["Url"].ToString(),dsPerms.Tables[0].DefaultView[0]["Title"].ToString());
            pnltyredetails.Visible = false;
            grvTyresDetails.Visible = false;
            if (p.View)
            {
                grvTyresDetails.Visible = true;
                grvTyresDetails.Columns[6].Visible = false;
            }

            if (p.Add)
            {
                pnltyredetails.Visible = true;
                grvTyresDetails.Visible = true;
                grvTyresDetails.Columns[6].Visible = false;
            }

            if (p.Modify)
            {
                grvTyresDetails.Visible = true;
                grvTyresDetails.Columns[6].Visible = true;
            }
        }
    }

    public void FillGrid_TyresDetails()
    {
        var ds = _fleetMaster.FillGrid_TyresDetails();
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            grvTyresDetails.DataSource = ds.Tables[0];
            grvTyresDetails.DataBind();
        }
        else
        {
            var strScript1 = "<script language=JavaScript>alert('" + "No record found" + "')</script>";
            ClientScript.RegisterStartupScript(GetType(),"Success",strScript1);
        }
    }

    public void TyresDetailsReset()
    {
        txtTyreItemCode.Text = "";
        txtTyreMake.Text = "";
        txtTyreModel.Text = "";
        txtTyreNumber.Text = "";
        txtTyreSize.Text = "";
        btnTyresDetailsSave.Text = "Save";
    }

    protected void btnTyresDetailsSave_Click(object sender,EventArgs e)
    {
        switch (btnTyresDetailsSave.Text)
        {
            case "Save":
            {
                var ds = _fleetMaster.FillGrid_TyresDetails();
                if (ds == null) throw new ArgumentNullException(nameof(ds));
                if (ds.Tables[0].Select("TyreNumber='" + txtTyreNumber.Text + "' or Tyre_Item_Code='" + txtTyreItemCode.Text + "'").Length > 0)
                {
                    Show("Tyre Number and Tyre Item Code already exists");
                }
                else
                {
                    var tyreItemCode = txtTyreItemCode.Text;
                    var tyreNumber = txtTyreNumber.Text;
                    var make = txtTyreMake.Text;
                    var model = txtTyreModel.Text;
                    var size = txtTyreSize.Text;
                    var tStatus = 1;
                    var tInactdate = DateTime.Today;
                    var tCreatedate = DateTime.Today;
                    var tCreateby = Convert.ToString(Session["User_Id"]);
                    var tUpdatedate = DateTime.Today;
                    var tUpdateby = Convert.ToString(Session["User_Id"]);
                    ds = _fleetMaster.InsertTyresDetails(tyreItemCode,tyreNumber,make,model,size,tStatus,tInactdate,tCreatedate,tCreateby,tUpdatedate,tUpdateby);
                    switch (ds.Tables.Count)
                    {
                        case 0:
                            Show("Tyres Details added successfully");
                            TyresDetailsReset();
                            break;
                        default:
                            Show("This Tyres details already exists");
                            break;
                    }
                }

                break;
            }
            default:
            {
                var ds = _fleetMaster.FillGrid_TyresDetails();
                if (ds.Tables[0].Select("TyreNumber='" + txtTyreNumber.Text + "' And Tyre_Id<>'" + hidTyresId.Value + "'").Length > 0)
                {
                    Show("Tyre Number and Tyre Item Code already exists");
                }
                else
                {
                    int tyreId = Convert.ToInt16(hidTyresId.Value);
                    var tyreItemCode = txtTyreItemCode.Text;
                    var tyreNumber = txtTyreNumber.Text;
                    var make = txtTyreMake.Text;
                    var model = txtTyreModel.Text;
                    var size = txtTyreSize.Text;
                    ds = _fleetMaster.UpdateTyresDetails(tyreId,tyreItemCode,tyreNumber,make,model,size);
                    switch (ds.Tables.Count)
                    {
                        case 0:
                            Show("Tyres Details updated successfully");
                            TyresDetailsReset();
                            break;
                        default:
                            Show("This Tyres details already exists");
                            break;
                    }
                }

                break;
            }
        }

        FillGrid_TyresDetails();
    }

    protected void btnTyresDetailsReset_Click(object sender,EventArgs e)
    {
        TyresDetailsReset();
    }

    protected void grvTyresDetails_PageIndexChanging(object sender,GridViewPageEventArgs e)
    {
        grvTyresDetails.PageIndex = e.NewPageIndex;
        FillGrid_TyresDetails();
    }

    protected void grvTyresDetails_RowEditing(object sender,GridViewEditEventArgs e)
    {
        btnTyresDetailsSave.Text = "Update";
        var index = e.NewEditIndex;
        var lblid = (Label) grvTyresDetails.Rows[index].FindControl("lblId");
        hidTyresId.Value = lblid.Text;
        int tyreId = Convert.ToInt16(hidTyresId.Value);
        using (var ds = _fleetMaster.RowEditTyresDetails(tyreId))
        {
            txtTyreItemCode.Text = Convert.ToString(ds.Tables[0].Rows[0]["Tyre_Item_Code"].ToString()).Trim();
            txtTyreNumber.Text = Convert.ToString(ds.Tables[0].Rows[0]["TyreNumber"].ToString());
            txtTyreMake.Text = Convert.ToString(ds.Tables[0].Rows[0]["Make"].ToString());
            txtTyreModel.Text = Convert.ToString(ds.Tables[0].Rows[0]["Model"].ToString());
            txtTyreSize.Text = Convert.ToString(ds.Tables[0].Rows[0]["Size"].ToString());
        }
    }

    public void Show(string message)
    {
        ScriptManager.RegisterStartupScript(this,GetType(),"msg","alert('" + message + "');",true);
    }
}