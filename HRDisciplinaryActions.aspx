<%@ Page Language="C#" MasterPageFile="~/temp.master" AutoEventWireup="true" CodeFile="HrDisciplinaryActions.aspx.cs" Inherits="HrDisciplinaryActions" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="updtpnlFinanceReceipt" runat="server">
        <ContentTemplate>
            <script type="text/javascript">
                function pageLoad() {
                    $('#<%= ddlVehicleno.ClientID %>').select2({
                        disable_search_threshold: 5,
                        search_contains: true,
                        minimumResultsForSearch: 20,
                        placeholder: "Select an option"
                    });
                }
            </script>
            <legend align="center"> HR Disciplinay Action</legend>
            <table align="center">
                <tr>
                    <td>
                        Vehicle No <span style="color: red">*</span>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlVehicleno" runat="server" Width="150px"/>
                    </td>
                </tr>
                <tr>
                    <td>
                        Situation of Accident<span style="color: red">*</span>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlSitIfAction" runat="server" CssClass="search_3" AutoPostBack="True" OnSelectedIndexChanged="ddlSitIfAction_SelectedIndexChanged"/>
                    </td>
                </tr>
                <tr>
                    <td>

                        Cause Of Accident <span style="color: red">*</span>

                    </td>
                    <td>
                        <asp:DropDownList ID="ddlCause" runat="server" CssClass="search_3"/>
                    </td>
                </tr>
                <tr>
                    <td>
                        Minor Accident(0-100000rs)<span style="color: red">*</span>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlMinor" runat="server" CssClass="search_3" onChange="javascript:MinorfilterChanged()"/>
                    </td>
                </tr>
                <tr>
                    <td>
                        Major Accident(100000-500000rs) <span style="color: red">*</span>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlMajor" runat="server" CssClass="search_3" onChange="javascript:MajorfilterChanged()"/>
                    </td>
                </tr>
                <tr>
                    <td>
                        Major loss/Total Loss <span style="color: red">*</span>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlMajorOrtotLoss" runat="server" CssClass="search_3"/>
                    </td>
                </tr>
                <tr>
                    <td>
                        Severe injuries to personnel <span style="color: red"></span>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlSevereInj" runat="server" CssClass="search_3"/>
                    </td>
                </tr>
                <tr>
                    <td>
                        Fatal Accident <span style="color: red"></span>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlFatalAcc" runat="server" CssClass="search_3"/>
                    </td>
                </tr>
            </table>
            <br/>
            <table align="center">

                <tr>
                    <td>
                        <asp:Button runat="server" ID="btnSave" Text="Save" OnClick="btnSave_Click" CssClass="form-submit-button" OnClientClick="if (!ValidatePage()) { return false; }"/>
                    </td>
                    <td>
                        <asp:Button runat="server" ID="btnClear" CssClass="form-reset-button" Text="Reset"
                                    OnClick="btnClear_Click"/>
                    </td>
                </tr>
            </table>
            <script type="text/javascript">
                function commonMajor() {
                    var ddlMajorAccident = $('#<%= ddlMajor.ClientID %> option:selected').text().toLowerCase();
                    if (ddlMajorAccident !== '-- select --') {
                        return alert("Selected Accident Type(Minor)");
                    }
                    return true;
                }

                function commonMinor() {
                    var ddlMinorAccident = $('#<%= ddlMinor.ClientID %> option:selected').text().toLowerCase();
                    if (ddlMinorAccident !== '-- select --') {
                        return alert("Selected Accident Type(Major)");
                    }
                    return true;
                }

                function MajorfilterChanged() {
                    if ($('#<%= ddlMajor.ClientID %> option:selected').text().toLowerCase() !== '-- select --')
                        $('#<%= ddlMinor.ClientID %>').attr("disabled", true);
                    else
                        $('#<%= ddlMinor.ClientID %>').prop("disabled", false);

                    commonMinor();
                }

                function MinorfilterChanged() {
                    if ($('#<%= ddlMinor.ClientID %> option:selected').text().toLowerCase() !== '-- select --')
                        $('#<%= ddlMajor.ClientID %>').attr("disabled", true);
                    else
                        $('#<%= ddlMajor.ClientID %>').prop("disabled", false);
                    commonMajor();
                }


                function ValidatePage() {
                    var ddlVehicle = $('#<%= ddlVehicleno.ClientID %> option:selected').text().toLowerCase();
                    if (ddlVehicle === '--select--') {
                        return alert("Please select Vehicle");
                    }
                    var ddlSituation = $('#<%= ddlSitIfAction.ClientID %> option:selected').text().toLowerCase();
                    if (ddlSituation === '-- select --') {
                        return alert("Please select situation of Accident");

                    }
                    var ddlCauseofAccident = $('#<%= ddlCause.ClientID %> option:selected').text().toLowerCase();
                    if (ddlCauseofAccident === '--select--') {
                        return alert("Please select Cause of Accident");
                    }
                    var ddlMajorLoss = $('#<%= ddlMajorOrtotLoss.ClientID %> option:selected').text().toLowerCase();
                    if (ddlMajorLoss === '-- select --') {
                        return alert("Please select MajorLoss Field");
                    }
                    if ($('#<%= ddlMinor.ClientID %> option:selected').text().toLowerCase() === "-- select --" &&
                        $('#<%= ddlMajor.ClientID %> option:selected').text().toLowerCase() === "-- select --")
                        return (alert("Please select Type Of Accident"));
                    else
                        return true;
                }
            </script>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>