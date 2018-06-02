<%@ Page Language="C#" MasterPageFile="~/temp.master" AutoEventWireup="true" CodeFile="VehicleAllocation.aspx.cs" Inherits="VehicleAllocation" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<script language="javascript" type="text/javascript">

    function validation() {
        var fldDistrict = document.getElementById('<%= ddlDistrict.ClientID %>');
        var fldVehicleNumber = document.getElementById('<%= ddlVehicleNumber.ClientID %>');
        var fldOdo = document.getElementById('<%= txtOdo.ClientID %>');
        var fldUptime = document.getElementById('<%= txtUptimeDate.ClientID %>');
        var fldHrs = document.getElementById('<%= ddlUPHour.ClientID %>');
        var fldMins = document.getElementById('<%= ddlUPMin.ClientID %>');
        var fldContactNumber = document.getElementById('<%= txtContactNumber.ClientID %>');
        if (fldDistrict && fldDistrict.selectedIndex === 0) {
            alert("Please Select District");
            fldDistrict.focus();
            return false;
        }

        if (fldVehicleNumber && fldVehicleNumber.selectedIndex === 0) {
            alert("Please select Vehicle Number");
            fldVehicleNumber.focus();
            return false;
        }

        if (!RequiredValidation(fldOdo, "Odometer cannot be blank"))
            return false;

        if (!RequiredValidation(fldUptime, "Uptime cannot be blank"))
            return false;

        if (fldHrs && fldHrs.selectedIndex === 0) {
            alert("Please select Hrs");
            fldHrs.focus();
            return false;
        }

        if (fldMins && fldMins.selectedIndex === 0) {
            alert("Please select Mins");
            fldMins.focus();
            return false;
        }

        if (window.fld_Segment && window.fld_Segment.selectedIndex === 0) {
            alert("Please select Segment");
            window.fld_Segment.focus();
            return false;
        }

        if (window.fld_SegmentName)
            if (!RequiredValidation(window.fld_SegmentName, "Segment cannot be blank"))
                return false;

        switch (window.fld_Mandals.selectedIndex) {
        case 0:
            alert("Please select Mandal");
            window.fld_Mandals.focus();
            return false;
        }

        switch (window.fld_City.selectedIndex) {
        case 0:
            alert("Please select City");
            window.fld_City.focus();
            return false;
        }

        if (window.fld_BaseLocation && window.fld_BaseLocation.selectedIndex === 0) {
            alert("Please select Base Location");
            window.fld_BaseLocation.focus();
            return false;
        }

        if (window.fld_TxtBaseLoc)
            if (!RequiredValidation(window.fld_TxtBaseLoc, "Base Location cannot be blank"))
                return false;

        if (!RequiredValidation(fldContactNumber, "Contact Number cannot be blank"))
            return false;


        document.getElementById("loaderButton").style.display = '';
        document.all('<%= pnlButton.ClientID %>').style.display = "none";
        return true;
    }


    function ChkDistrict() {
        var fldDistrict = document.getElementById('<%= ddlDistrict.ClientID %>');
        if (fldDistrict && fldDistrict.selectedIndex === 0) {
            alert("Please Select District");
            fldDistrict.focus();
            return false;
        }

        return true;
    }

    function OnlyNumPeriod(evt) {
        var charCode = (evt.which) ? evt.which : event.keyCode;
        if (evt.shiftKey === 1 ||
            (charCode >= 48 && charCode <= 57) ||
            (charCode > 95 && charCode < 106) ||
            charCode === 8 ||
            charCode === 46 ||
            charCode === 190 ||
            charCode === 9 ||
            charCode === 110) {
            alert('Shift Key is not allowed');
        }
        return false;
    }

    function isDecimal(control) {
        var input = control.value;
        if (input === '')
            return;
        var arr = input.toString().split('.');
        var id = document.getElementById(control.id);
        if (arr.length !== 2 ||
            arr.length === 2 && arr[0].length !== 2 ||
            arr.length === 2 && arr[1].length < 1 ||
            arr.length === 2 && arr[1].length > 5) {
            alert('InCorrect Format');
            control.value = "";
            id.focus();
        }
    }

    function addZero(i) {
        if (i < 10) {
            i = "0" + i;
        }
        return i;
    }
</script>
<asp:UpdatePanel runat="server">
<ContentTemplate>

    <script type="text/javascript">
        function pageLoad() {
            $('#<%= ddlVehicleNumber.ClientID %>').select2({
                disable_search_threshold: 5,
                search_contains: true,
                minimumResultsForSearch: 20,
                placeholder: "Select an option"
            });
            $('#<%= ddlDistrict.ClientID %>').select2({
                disable_search_threshold: 5,
                search_contains: true,
                minimumResultsForSearch: 20,
                placeholder: "Select an option"
            });
        }
    </script>
    <legend align="center" style="color: brown">Vehicle Allocation</legend>
    <table align="center">
        <tr>
            <td colspan="7"></td>
        </tr>
        <tr>
            <td>
                District<span style="color: Red">*</span>
            </td>
            <td class="columnseparator"></td>
            <td colspan="5">
                <asp:DropDownList ID="ddlDistrict" runat="server" OnSelectedIndexChanged="ddlDistrict_SelectedIndexChanged"
                                  AutoPostBack="True">
                    <asp:ListItem Value="-1">--Select--</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="rowseparator"></td>
        </tr>
        <tr>
            <td>
                Vehicle Number<span class="labelErr" style="color: Red">*</span>
            </td>
            <td class="columnseparator"></td>
            <td>
                <asp:DropDownList ID="ddlVehicleNumber" runat="server" Width="150px" AutoPostBack="true" OnSelectedIndexChanged="ddlVehicleNumber_SelectedIndexChanged">
                    <asp:ListItem Value="-1">--Select--</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="rowseparator"></td>
        </tr>
        <tr>
            <td>
                Reason for Down<span class="labelErr" style="color: Red">*</span>
            </td>
            <td class="columnseparator"></td>
            <td>
                <asp:TextBox ID="txtReasonforDown" CssClass="search_3" runat="server" TextMode="MultiLine" onkeypress="return false;"></asp:TextBox>
            </td>
            <td class="columnseparator"></td>
            <td>
                DownTime<span style="color: Red">*</span>
            </td>
            <td class="columnseparator"></td>
            <td colspan="2">
                <asp:TextBox ID="txtDownTime" CssClass="search_3" runat="server" onkeypress="return false;"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="7"></td>
        </tr>
        <tr>
            <td>
                Odometer<span style="color: Red">*</span>
            </td>
            <td class="columnseparator"></td>
            <td colspan="2">
                <asp:TextBox ID="txtOdo" runat="server" CssClass="search_3" onkeypress="return numeric_only(event)" MaxLength="6"
                             Width="90%">
                </asp:TextBox>
            </td>
            <td>
                Previous ODO<span style="color: Red">*</span>
            </td>
            <td class="columnseparator"></td>
            <td>
                <asp:Label ID="lblpvODO" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="7"></td>
        </tr>
        <tr>
            <td>
                Requested By<span style="color: Red">*</span>
            </td>
            <td class="columnseparator"></td>
            <td colspan="5">
                <asp:TextBox ID="txtReqBy" CssClass="search_3" runat="server" onkeypress="return false;"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="rowseparator"></td>
        </tr>
        <tr>
            <td>
                Expected Date of Recovery<span style="color: Red">*</span>
            </td>
            <td class="columnseparator"></td>
            <td colspan="5">
                <asp:TextBox ID="txtExpDateOfRec" CssClass="search_3" runat="server" onkeypress="return false;"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="rowseparator"></td>
        </tr>
        <tr>
            <td>
                Uptime <span style="color: Red">*</span>
            </td>
            <td class="columnseparator"></td>
            <td nowrap="nowrap" colspan="6">
                <table style="width: 100%">
                    <tr>
                        <td nowrap="nowrap" style="width: 20%">

                            <asp:TextBox ID="txtUptimeDate" CssClass="search_3" runat="server" Width="150px" onkeypress="return false;"></asp:TextBox>

                            <cc1:CalendarExtender runat="server" CssClass="cal_Theme1" TargetControlID="txtUptimeDate"
                                                  PopupButtonID="imgBtnUptimeDate" Format="dd/MM/yyyy">
                            </cc1:CalendarExtender>
                            <asp:ImageButton ID="imgBtnUptimeDate" runat="server" Style="vertical-align: top;"
                                             alt="" src="images/Calendar.gif"/>
                        </td>
                        <td style="width: 80%">
                            <asp:DropDownList ID="ddlUPHour" CssClass="search_3" style="margin-left: 50px" runat="server" Width="55px">
                                <asp:ListItem Value="-1">--hh--</asp:ListItem>
                            </asp:DropDownList>
                            <asp:DropDownList ID="ddlUPMin" CssClass="search_3" runat="server" Width="60px">
                                <asp:ListItem Value="-1">--mm--</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="rowseparator"></td>
        </tr>

        <tr>
            <td class="rowseparator"></td>
        </tr>

        <tr>
            <td>
                Contact Number<span style="color: Red">*</span>
            </td>
            <td class="columnseparator"></td>
            <td colspan="5">
                <asp:TextBox ID="txtContactNumber" CssClass="search_3" runat="server" onkeypress="return numeric_only(event)"
                             MaxLength="10">
                </asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="rowseparator"></td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblLatitude" runat="server" Text="Latitude" Visible="false"></asp:Label>
                <asp:Label ID="lblMandatory1" runat="server" Text="*" ForeColor="Red" Visible="false"></asp:Label>
            </td>
            <td class="columnseparator"></td>
            <td>
                <asp:TextBox ID="txtLatitude" CssClass="search_3" runat="server" Visible="false" onblur="numericOnly(this);"
                             onkeydown="return OnlyNumPeriod(event);">
                </asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="rowseparator"></td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblLongitude" runat="server" Text="Longitude" Visible="false"></asp:Label>
                <asp:Label ID="lblMandatory2" runat="server" Text="*" ForeColor="Red" Visible="false"></asp:Label>
            </td>
            <td class="columnseparator"></td>
            <td>
                <asp:TextBox ID="txtLongitude" CssClass="search_3" runat="server" Visible="false" onblur="numericOnly(this);"
                             onkeydown="return OnlyNumPeriod(event);">
                </asp:TextBox>
            </td>
        </tr>
        <tr>
            <div style="top: 0px; width: 68px;">
        </tr>
        <caption>
            <img id="loaderButton" alt="" src="../images/savingimage.gif"
                 style="display: none"/>
            <tr>
                <td align="center" colspan="7" style="">
                    <asp:Panel ID="pnlButton" runat="server">
                        <asp:Button ID="btnSubmit" CssClass="form-submit-button" runat="server" OnClick="btnSubmit_Click"
                                    Text="Submit"/>
                        &nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnReset" CssClass="form-reset-button" runat="server" OnClick="btnReset_Click"
                                    Text="Reset"/>
                    </asp:Panel>
                </td>
            </tr>
        </caption>
    </table>
</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>