using System;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using GvkFMSAPP.BLL;
using GvkFMSAPP.BLL.StatutoryCompliance;
using GvkFMSAPP.DLL;
using FMSGeneral = GvkFMSAPP.BLL.FMSGeneral;

public partial class AttachDocuments : Page
{
    private readonly AttachmentForVehiclesBLL _attachmentForVehicle = new AttachmentForVehiclesBLL();
    private readonly FMSGeneral _fmsGeneral = new FMSGeneral();
    private readonly Helper _helper = new Helper();
    private readonly VehicleInsurance _vehicleInsurance = new VehicleInsurance();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Name"] == null) Response.Redirect("Login.aspx");
        if (IsPostBack) return;
        if (Session["UserdistrictId"] != null) _vehicleInsurance.UserDistrictId = Convert.ToInt32(Session["UserdistrictId"].ToString());
        GetVehicleNumber();
        btnAttachFiles.Attributes.Add("onclick", "return validation(this,'" + btnAttachFiles.ID + "')");
        FillVehicleAttachment();
    }

    protected void btnAttachFiles_Click(object sender, EventArgs e)
    {
        try
        {
            if (!Directory.Exists(Server.MapPath("Data") + "\\" + ddlistVehicleNumber.SelectedItem.Text)) Directory.CreateDirectory(Server.MapPath("Data") + "\\" + ddlistVehicleNumber.SelectedItem.Text);
            if (fileAttachmentPurpose.PostedFile == null)
            {
                Show("Please select a file to upload.");
            }
            else
            {
                var filename = Path.GetFileName(fileAttachmentPurpose.PostedFile.FileName);
                var renameFilename = ddlistVehicleNumber.SelectedItem.Text + "\\" + filename.Split('.')[0] + "_" + DateTime.Now.ToString("ddMMyyhhmmss") + "." + filename.Split('.')[1];
                _attachmentForVehicle.VehicleID = int.Parse(ddlistVehicleNumber.SelectedItem.Value);
                _attachmentForVehicle.AttachmentPurposeFile = filename;
                if (_attachmentForVehicle.CheckAttachmentFileExistByVehicleId())
                {
                    Show("File Already been Uploaded for the existing Vehicle .");
                }
                else
                {
                    var saveLocation = Server.MapPath("Data") + "\\" + ddlistVehicleNumber.SelectedItem.Text + "\\" + filename.Split('.')[0] + "_" + DateTime.Now.ToString("ddMMyyhhmmss") + "." + filename.Split('.')[1];
                    try
                    {
                        fileAttachmentPurpose.PostedFile.SaveAs(saveLocation);
                        _attachmentForVehicle.VehicleID = int.Parse(ddlistVehicleNumber.SelectedItem.Value);
                        _attachmentForVehicle.Remarks = txtRemarks.Text;
                        _attachmentForVehicle.AttachmentPurpose = ddlistAttachmentPurpose.SelectedItem.Value;
                        _attachmentForVehicle.AttachmentPurposeFile = filename;
                        _attachmentForVehicle.RenameFileName = renameFilename;
                        _attachmentForVehicle.CreatedBy = Session["User_Name"].ToString();
                        _attachmentForVehicle.InsertFillAttachmentToVehicle();
                        Show("File attached Successfully");
                    }
                    catch (Exception ex)
                    {
                        _helper.ErrorsEntry(ex);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ErrorHandler.ErrorsEntry(ex.GetBaseException().ToString(), "class: AttachDocuments;Method: btnAttachFiles_Click()-InsertFillAttachmentToVehicle", 0);
        }

        FillVehicleAttachment();
        ClearControls();
    }

    public void GetVehicleNumber()
    {
        try
        {
            if (_fmsGeneral != null)
            {
                _fmsGeneral.UserDistrictId = Convert.ToInt32(Session["UserdistrictId"].ToString());
                var ds = _fmsGeneral.GetVehicleNumber();
                if (ds == null) return;
                _helper.FillDropDownHelperMethodWithDataSet(ds, "VehicleNumber", "VehicleID", ddlistVehicleNumber);
            }
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    protected void ClearControls()
    {
        ddlistAttachmentPurpose.SelectedIndex = 0;
        txtRemarks.Text = "";
        ddlistVehicleNumber.SelectedIndex = 0;
    }

    protected void FillVehicleAttachment()
    {
        try
        {
            var dv = _attachmentForVehicle.FillAttachmentToVehicle().Tables[0].DefaultView; // objFMSOther.IFillAttachmentToVehicle().Tables[0].DefaultView;
            if (dv == null) throw new ArgumentNullException(nameof(dv));
            grdVehicleAttachment.DataSource = dv;
            grdVehicleAttachment.DataBind();
        }
        catch (Exception ex)
        {
            _helper.ErrorsEntry(ex);
        }
    }

    protected void grdVehicleAttachment_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdVehicleAttachment.PageIndex = e.NewPageIndex;
        FillVehicleAttachment();
    }

    public void Show(string message)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('" + message + "');", true);
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
    }
}