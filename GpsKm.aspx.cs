using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;

public partial class GpsKm : Page
{
    private readonly Helper _helper = new Helper();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Name"] == null) Response.Redirect("login.aspx");
        if (!IsPostBack)
        {
            bindVehicles();
            BindData();
        }
    }

    private void BindData()
    {
        try
        {
            var query = ConfigurationManager.AppSettings["SqlQuery"];
            var dtData = _helper.ExecuteSelectStmt(query);
            if (dtData == null) throw new ArgumentNullException(nameof(dtData));
            if (dtData.Rows.Count <= 0)
            {
                grdRepData.DataSource = null;
                grdRepData.DataBind();
                Show("No Records Found");
            }
            else
            {
                grdRepData.DataSource = dtData;
                grdRepData.DataBind();
            }
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    public void Show(string message)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('" + message + "');", true);
    }

    private void bindVehicles()
    {
    }

    protected void btntoExcel_Click(object sender, EventArgs e)
    {
        try
        {
            _helper.LoadExcelSpreadSheet(this, null, "gvtoexcel.xls", grdRepData);
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
    }

    protected void grdRepData_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == null) return;
        try
        {
            switch (e.CommandName)
            {
                case "change":
                    var vehicleNumber = e.CommandArgument.ToString();
                    var dtVehicleDetails = _helper.ExecuteSelectStmt("select * from t_expfuelDump where vehiclenumber = '" + vehicleNumber + "'");
                    if (dtVehicleDetails != null && dtVehicleDetails.Rows.Count > 0)
                    {
                        dvSearch.Visible = true;
                        txtVehicleNumber.Text = dtVehicleDetails.Rows[0]["vehiclenumber"].ToString();
                        txtCardNumber.Text = dtVehicleDetails.Rows[0]["petrocardnum"].ToString();
                        txtLimit.Text = dtVehicleDetails.Rows[0]["expprice"].ToString();
                        chkpush.Checked = dtVehicleDetails.Rows[0]["canpush"].ToString() == "true";
                    }

                    break;
            }
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        try
        {
            var l = 0;
            if (chkpush.Checked) l = 1;
            var i = _helper.ExecuteInsertStatement("update t_expfuelDump set petrocardnum = '" + txtCardNumber.Text + "',expprice='" + txtLimit.Text + "', canpush = '" + l + "' where vehiclenumber = '" + txtVehicleNumber.Text + "' ");
            if (i > 0)
                Show("Record Updated Successfully");
            _helper.TraceService(txtVehicleNumber.Text + "~~~~~~" + i + "~~~~~~" + txtLimit.Text);
            PushDatatoIoc();
            Clear();
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    private void PushDatatoIoc()
    {
        try
        {
            var dtCredentials = _helper.ExecuteSelectStmt("select * from M_IOC_API_updateCurrency where isactive = 1;");
            if (dtCredentials == null) throw new ArgumentNullException(nameof(dtCredentials));
            if (dtCredentials.Rows.Count <= 0) return;
            var request = (HttpWebRequest) WebRequest.Create(dtCredentials.Rows[0]["url"].ToString());
            var postData = "UserName=" + dtCredentials.Rows[0]["username"];
            postData += "&Password=" + dtCredentials.Rows[0]["Password"];
            postData += "&CustomerId=" + dtCredentials.Rows[0]["CustomerId"];
            postData += "&CardNumber=" + txtCardNumber.Text;
            postData += "&CCMSLimitType=" + dtCredentials.Rows[0]["CCMSLimitType"];
            postData += "&CCMSLimitAmount=" + txtLimit.Text;
            var data = Encoding.ASCII.GetBytes(postData);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse) request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            dynamic jObj = JsonConvert.DeserializeObject(responseString);
            try
            {
                if (jObj.StatusCode != "0")
                {
                    string insertstmt = "update t_expfuelDump set ReferenceNo = '" + jObj.ReferenceNo + "', StatusCode = '" + jObj.StatusCode + "' where petrocardnum = '" + txtCardNumber.Text + "'";
                    _helper.ExecuteInsertStatement(insertstmt);
                }
                else
                {
                    string insertstmt = "update t_expfuelDump set ReferenceNo = '" + jObj.ReferenceNo + "', StatusCode = '" + jObj.StatusCode + "', Message ='" + jObj.Message + "' where petrocardnum = '" + txtCardNumber.Text + "'";
                    _helper.ExecuteInsertStatement(insertstmt);
                }
            }
            catch (Exception ex)
            {
                _helper.ErrorsEntry(ex);
                _helper.TraceService("Exception Raised ~ 1123~2 : " + ex);
            }
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    private void Clear()
    {
        txtVehicleNumber.Text = txtLimit.Text = txtCardNumber.Text = "";
        chkpush.Checked = false;
        BindData();
        dvSearch.Visible = false;
    }

    public class WebClient : System.Net.WebClient
    {
        public int Timeout { get; set; }

        protected override WebRequest GetWebRequest(Uri uri)
        {
            var lWebRequest = base.GetWebRequest(uri);
            if (lWebRequest != null)
            {
                lWebRequest.Timeout = Timeout;
                ((HttpWebRequest) lWebRequest).ReadWriteTimeout = Timeout;
            }

            return lWebRequest;
        }
    }
}