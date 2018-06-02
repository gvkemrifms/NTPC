using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using GvkFMSAPP.BLL;
using GvkFMSAPP.PL;

public partial class SparePartDiscrepancy : Page
{
    public IInventory ObjInventory = new FMSInventory();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Name"] == null) Response.Redirect("Login.aspx");
        if (!IsPostBack)
        {
            FillGridSparePartDiscrepancy();
            var dsPerms = (DataSet) Session["PermissionsDS"];
            if (dsPerms == null) throw new ArgumentNullException(nameof(dsPerms));
            dsPerms.Tables[0].DefaultView.RowFilter = "Url='" + Page.Request.Url.Segments[Page.Request.Url.Segments.Length - 1] + "'";
            var p = new PagePermissions(dsPerms, dsPerms.Tables[0].DefaultView[0]["Url"].ToString(), dsPerms.Tables[0].DefaultView[0]["Title"].ToString());
            pnlSparePartDiscrepancy.Visible = false;
            if (p.View)
            {
                pnlSparePartDiscrepancy.Visible = true;
                pnlButtons.Visible = false;
            }

            if (p.Add)
            {
                pnlSparePartDiscrepancy.Visible = true;
                pnlButtons.Visible = true;
            }
        }
    }

    private void FillGridSparePartDiscrepancy()
    {
        var districtId = -1;
        if (Session["UserdistrictId"] != null) districtId = Convert.ToInt32(Session["UserdistrictId"].ToString());
        var ds = ObjInventory.GetGridSparePartDiscrepancy(districtId);
        switch (ds.Tables[0].Rows.Count)
        {
            case 0:
                Reset();
                gvSparePartDiscrepancy.DataSource = ds;
                gvSparePartDiscrepancy.DataBind();
                break;
            default:
                gvSparePartDiscrepancy.DataSource = ds;
                gvSparePartDiscrepancy.DataBind();
                break;
        }
    }

    private void Reset()
    {
        btSave.Visible = false;
        btCancel.Visible = false;
    }

    protected void btSave_Click(object sender, EventArgs e)
    {
        var chk = 0;
        var dtAddRequisition = new DataTable();
        dtAddRequisition.Columns.Add("VehicleID", typeof(int));
        dtAddRequisition.Columns.Add("SparePartReceiptID", typeof(int));
        dtAddRequisition.Columns.Add("DistrictID", typeof(int));
        dtAddRequisition.Columns.Add("ReceivedQty", typeof(int));
        dtAddRequisition.Columns.Add("IssuedQty", typeof(int));
        dtAddRequisition.Columns.Add("ReceivedBy", typeof(int));
        dtAddRequisition.Columns.Add("Remarks", typeof(string));
        dtAddRequisition.Columns.Add("RequestedBy", typeof(int));
        foreach (GridViewRow row in gvSparePartDiscrepancy.Rows)
            if (((CheckBox) row.FindControl("chk")).Checked)
            {
                var txt = (TextBox) row.FindControl("txtRemarks");
                switch (txt.Text)
                {
                    case "":
                        chk++;
                        break;
                }

                var dr = dtAddRequisition.NewRow();
                dr["VehicleID"] = ((Label) row.FindControl("lbVehicle")).Text;
                dr["SparePartReceiptID"] = ((Label) row.FindControl("lbDetID")).Text;
                dr["DistrictID"] = ((Label) row.FindControl("lbdistrict")).Text;
                dr["ReceivedQty"] = row.Cells[2].Text;
                dr["IssuedQty"] = row.Cells[3].Text;
                dr["ReceivedBy"] = ((Label) row.FindControl("lbCreatedBy")).Text;
                dr["Remarks"] = ((TextBox) row.FindControl("txtRemarks")).Text;
                dr["RequestedBy"] = 3;
                dtAddRequisition.Rows.Add(dr);
            }

        if (chk <= 0)
        {
            string strFmsScript;
            switch (dtAddRequisition.Rows.Count)
            {
                case 0:
                    strFmsScript = "Please Check And Submit";
                    Show(strFmsScript);
                    break;
                default:
                    var updResult = ObjInventory.InsertingSparePartDiscrepancy(dtAddRequisition);
                    if (updResult)
                    {
                        strFmsScript = "Spare Part Discrepancy Inserted";
                        Show(strFmsScript);
                    }
                    else
                    {
                        strFmsScript = "Failure";
                        Show(strFmsScript);
                    }

                    break;
            }

            FillGridSparePartDiscrepancy();
        }
        else
            Show("Please Fill The Remarks");

        ClearFields();
    }

    private void Show(string message)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('" + message + "');", true);
    }

    protected void gvSparePartDiscrepancy_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow) ScriptManager.RegisterClientScriptBlock(this, GetType(), "dsad", "var Remarks = '" + ((TextBox) e.Row.FindControl("txtRemarks")).ClientID + "'", true);
    }

    protected void btCancel_Click(object sender, EventArgs e)
    {
        ClearFields();
    }

    private void ClearFields()
    {
        foreach (GridViewRow row in gvSparePartDiscrepancy.Rows)
        {
            var c = (CheckBox) row.FindControl("chk");
            c.Checked = false;
            var txt = (TextBox) row.FindControl("txtRemarks");
            txt.Text = "";
        }
    }
}