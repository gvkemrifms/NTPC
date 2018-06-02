using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;

public class Helper
{
    public DataTable ExecuteSelectStmt(string query)
    {
        var cs = ConfigurationManager.AppSettings["Str"];
        var dtSyncData = new DataTable();
        SqlConnection connection = null;
        try
        {
            connection = new SqlConnection(cs);
            connection.Open();
            var dataAdapter = new SqlDataAdapter {SelectCommand = new SqlCommand(query, connection)};
            dataAdapter.Fill(dtSyncData);
            TraceService(query);
            return dtSyncData;
        }
        catch (Exception ex)
        {
            TraceService("executeSelectStmt() " + ex + query);
            return null;
        }
        finally
        {
            connection.Close();
        }
    }
   

    public DataTable ExecuteSelectStmt(string query, string parameterName1, int parameterValue1)
    {
        var cs = ConfigurationManager.AppSettings["Str"];
        var dtSyncData = new DataTable();
        SqlConnection connection = null;
        try
        {
            connection = new SqlConnection(cs);
            connection.Open();
            var cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue(parameterName1, parameterValue1);
            cmd.CommandType = CommandType.StoredProcedure;
            var dataAdapter = new SqlDataAdapter {SelectCommand = cmd};
            dataAdapter.Fill(dtSyncData);
            TraceService(query);
            return dtSyncData;
        }
        catch (Exception ex)
        {
            TraceService("executeSelectStmt() " + ex + query);
            return null;
        }
        finally
        {
            connection.Close();
        }
    }

    public void TraceService(string content)
    {
        var str = @"C:\smslog_1\Log.txt";
        var path1 = str.Substring(0, str.LastIndexOf("\\", StringComparison.Ordinal));
        var path2 = str.Substring(0, str.LastIndexOf(".txt", StringComparison.Ordinal)) + "-" + DateTime.Today.ToString("yyyy-MM-dd") + ".txt";
        try
        {
            if (!Directory.Exists(path1)) Directory.CreateDirectory(path1);
            if (path2.Length >= Convert.ToInt32(4000000)) path2 = str.Substring(0, str.LastIndexOf(".txt", StringComparison.Ordinal)) + "-" + "2" + ".txt";
            var streamWriter = File.AppendText(path2);
            streamWriter.WriteLine("====================" + DateTime.Now.ToLongDateString() + "  " + DateTime.Now.ToLongTimeString());
            streamWriter.WriteLine(content);
            streamWriter.Flush();
            streamWriter.Close();
        }
        catch
        {
            // traceService(ex.ToString());
        }
    }

    public int ExecuteInsertStatement(string insertStmt)
    {
        using (var conn = new SqlConnection(ConfigurationManager.AppSettings["Str"]))
        {
            using (var comm = new SqlCommand())
            {
                var i = 0;
                comm.Connection = conn;
                comm.CommandText = insertStmt;
                try
                {
                    conn.Open();
                    i = comm.ExecuteNonQuery();
                    TraceService(insertStmt);
                    return i;
                }
                catch (SqlException ex)
                {
                    TraceService(" executeInsertStatement " + ex + insertStmt);
                    return i;
                }
                finally
                {
                    conn.Close();
                }
            }
        }
    }

    public void FillDropDownHelperMethodWithSp(string commandText, string textFieldValue = null, string valueField = null, DropDownList dropDownValue = null, DropDownList dropDownValue2 = null, TextBox txtBox = null, TextBox txtBox1 = null, string parameterValue1 = null, string parameterValue2 = null, string parameterValue3 = null, string parameterValue4 = null, string parameterValue5 = null, GridView gridView = null, DropDownList dropDownValue3 = null, DropDownList dropDownValue4 = null, DropDownList dropDownValue5 = null, int? filter = null)
    {
        var conn = new SqlConnection(ConfigurationManager.AppSettings["Str"]);
        var ds = new DataSet();
        conn.Open();
        var cmd = new SqlCommand {Connection = conn, CommandType = CommandType.StoredProcedure, CommandText = commandText};
        if (dropDownValue != null && gridView == null && dropDownValue2 == null)
        {
            if (parameterValue1 != null)
                cmd.Parameters.AddWithValue(parameterValue1, dropDownValue.SelectedValue);
            CommonMethod(textFieldValue, valueField, dropDownValue, ds, cmd);
            dropDownValue.Items.Insert(0, new ListItem("--Select--", "0"));
        }
        else if (dropDownValue != null && dropDownValue2 != null && dropDownValue3 != null && gridView == null)
        {
            cmd.Parameters.AddWithValue(parameterValue1, dropDownValue.SelectedValue);
            cmd.Parameters.AddWithValue(parameterValue2, dropDownValue2.SelectedValue);
            CommonMethod(textFieldValue, valueField, dropDownValue3, ds, cmd);
        }
        else if (dropDownValue != null && dropDownValue2 != null && gridView == null)
        {
            cmd.Parameters.AddWithValue(parameterValue1, filter == null ? dropDownValue.SelectedItem.Value : dropDownValue.SelectedItem.Text);
            CommonMethod(textFieldValue, valueField, dropDownValue2, ds, cmd);
            dropDownValue2.Items.Insert(0, new ListItem("--Select--", "0"));
        }

        else if (gridView != null)
        {
            if (dropDownValue != null && dropDownValue.SelectedIndex >= 0) cmd.Parameters.AddWithValue(parameterValue1, dropDownValue.SelectedItem.Value);
            if (dropDownValue2 != null && dropDownValue2.SelectedIndex >= 0) cmd.Parameters.AddWithValue(parameterValue2, dropDownValue2.SelectedItem.Value);
            if (dropDownValue3 != null && dropDownValue3.SelectedIndex >= 0) cmd.Parameters.AddWithValue(parameterValue5,dropDownValue3.SelectedItem.Value);
            if (dropDownValue4 != null && dropDownValue4.SelectedIndex >= 0) cmd.Parameters.AddWithValue(parameterValue4, dropDownValue4.SelectedItem.Value);
            if (dropDownValue5 != null && dropDownValue5.SelectedIndex >= 0) cmd.Parameters.AddWithValue(parameterValue5, dropDownValue5.SelectedItem.Value);
            if (txtBox != null) cmd.Parameters.AddWithValue(parameterValue3, txtBox.Text + " 00:00:00");
            if (txtBox1 != null) cmd.Parameters.AddWithValue(parameterValue4, txtBox1.Text + " 23:59:59");
            var da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            var dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                gridView.DataSource = dt;
                gridView.DataBind();
            }
            else
            {
                gridView.DataSource = null;
                gridView.DataBind();
            }
        }
        else
        {
            var da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            var dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                gridView.DataSource = dt;
                gridView.DataBind();
            }
            else
            {
                gridView.DataSource = null;
                gridView.DataBind();
            }
        }

        conn.Close();
    }

    private static void CommonMethod(string textFieldValue, string valueField, DropDownList dropDownValue, DataSet ds, SqlCommand cmd)
    {
        var da = new SqlDataAdapter(cmd);
        da.Fill(ds);
        dropDownValue.DataSource = ds.Tables[0];
        dropDownValue.DataTextField = textFieldValue;
        dropDownValue.DataValueField = valueField;
        dropDownValue.DataBind();
    }

    public void FillDropDownHelperMethod(string query, string textFieldValue, string valueField, DropDownList dropdownId)
    {
        using (var con = new SqlConnection(ConfigurationManager.AppSettings["Str"]))
        {
            con.Open();
            var cmd = new SqlCommand(query, con);
            var da = new SqlDataAdapter(cmd);
            var ds = new DataSet();
            da.Fill(ds);
            dropdownId.Items.Clear();
            dropdownId.DataSource = ds.Tables[0];
            dropdownId.DataTextField = textFieldValue;
            dropdownId.DataValueField = valueField;
            dropdownId.DataBind();
            dropdownId.Items.Insert(0, new ListItem("--Select--", "0"));
            con.Close();
        }
    }

    public void FillDropDownHelperMethodWithDataSet(DataSet dataSet, string textFieldValue, string valueField, DropDownList dropdownId = null, ComboBox combo = null, DropDownList dropdownId1 = null, RadioButtonList radiolist = null, string filter = null)
    {
        if (dropdownId == null && dropdownId1 == null)
        {
            if (combo != null)
            {
                combo.Items.Clear();
                combo.DataSource = dataSet.Tables[0];
                combo.DataTextField = textFieldValue;
                combo.DataValueField = valueField;
                combo.DataBind();
                combo.Items.Insert(0, new ListItem("--Select--", "0"));
                combo.Items[0].Value = "0";
                combo.SelectedIndex = 0;
            }
        }
        else if (dropdownId1 == null && combo == null)
        {
            dropdownId.Items.Clear();
            dropdownId.DataSource = dataSet.Tables[0];
            dropdownId.DataTextField = textFieldValue;
            dropdownId.DataValueField = valueField;
            dropdownId.DataBind();

            if (filter == null)
            {
                dropdownId.Items.Insert(0, new ListItem("--Select--", "0"));
                dropdownId.Items[0].Value = "0";
                dropdownId.SelectedIndex = 0;
            }
        }
        else
        {
            if (dropdownId1 != null && dropdownId != null && combo == null)
            {
                dropdownId1.Items.Clear();
                dropdownId1.DataSource = dataSet.Tables[0];
                dropdownId1.DataTextField = textFieldValue;
                dropdownId1.DataValueField = valueField;
                dropdownId1.DataBind();
                dropdownId1.Items.Insert(0, new ListItem("--Select--", "0"));
                dropdownId1.Items[0].Value = "0";
                dropdownId1.SelectedIndex = 0;

                dropdownId.Items.Clear();
                dropdownId.DataSource = dataSet.Tables[0];
                dropdownId.DataTextField = textFieldValue;
                dropdownId.DataValueField = valueField;
                dropdownId.DataBind();
                dropdownId.Items.Insert(0, new ListItem("--Select--", "0"));
                dropdownId.Items[0].Value = "0";
                dropdownId.SelectedIndex = 0;
            }
        }
    }

    public void FillDifferentDataTables(DropDownList dropDownList, DataTable dt, string textField, string valueField)
    {
        dropDownList.DataSource = dt;
        dropDownList.DataValueField = valueField;
        dropDownList.DataTextField = textField;
        dropDownList.DataBind();
    }

    public void LoadExcelSpreadSheet(Page page, Panel panel = null, string fileName = null, GridView gridView = null)
    {
        page.Response.ClearContent();
        page.Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
        page.Response.ContentType = "application/excel";
        var sw = new StringWriter();
        var htw = new HtmlTextWriter(sw);
        if (gridView != null)
            gridView.RenderControl(htw);
        else
            panel.RenderControl(htw);
        page.Response.Write(sw.ToString());
        // HttpContext.Current.ApplicationInstance.CompleteRequest();
        // page.Response.End();
        page.Response.BufferOutput = true;
        page.Response.Flush();
        page.Response.Close();
    }

    public void ErrorsEntry(Exception ex)
    {
        var appSetting = ConfigurationManager.AppSettings["LogLocation"];
        if (appSetting == null) throw new ArgumentNullException(nameof(appSetting));
        var path = appSetting.Substring(0, appSetting.LastIndexOf("\\", StringComparison.Ordinal));
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        using (var streamWriter = File.AppendText(ConfigurationManager.AppSettings["LogLocation"]))
        {
            var trace = new StackTrace(ex, true);
            // Get the top stack frame
            var frame = trace.GetFrame(0);
            if (frame == null) throw new ArgumentNullException(nameof(frame));
            // Get the line number from the stack frame
            var errorNo = frame.GetFileLineNumber();
            //Get  Error Source
            var errorSource = ex.Source;
            if (errorSource == null) throw new ArgumentNullException(nameof(errorSource));
            //Get Error Description
            var errorDescription = ex.Message;
            streamWriter.WriteLine("====================" + DateTime.Now.ToLongDateString() + "  " + DateTime.Now.ToLongTimeString());
            streamWriter.WriteLine(errorDescription);
            streamWriter.WriteLine(errorSource);
            streamWriter.WriteLine(errorNo.ToString());
            streamWriter.Flush();
            streamWriter.Close();
        }
    }
   
}