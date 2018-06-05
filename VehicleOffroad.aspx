<%@ Page Title="" Language="C#" MasterPageFile="~/temp.master" AutoEventWireup="true" CodeFile="VehicleOffroad.aspx.cs" Inherits="VehicleOffroad" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<script type="text/javascript" language="javascript">
    function pageLoad() {
        $(function() {
            $('#<%=txtOfftimeDate.ClientID%>,#<%=txtExpDateOfRec.ClientID%>').datepicker({
                dateFormat: 'mm/dd/yy',
                changeMonth: true,
                changeYear: true
            });
        });
        $('#<%= ddlDistrict.ClientID %>').select2({
            disable_search_threshold: 5,
            search_contains: true,
            minimumResultsForSearch: 20,
            placeholder: "Select an option"
        });
        $('#<%= ddlVehicleNumber.ClientID %>').select2({
            disable_search_threshold: 5,
            search_contains: true,
            minimumResultsForSearch: 20,
            placeholder: "Select an option"
        });
        $('#<%= ddlreasons.ClientID %>').select2({
            disable_search_threshold: 5,
            search_contains: true,
            minimumResultsForSearch: 20,
            placeholder: "Select an option"
        });
    }

    function addZero(i) {
        if (i < 10) {
            i = "0" + i;
        }
        return i;
    }

    function validation() {

        var fldDistrict = document.getElementById('<%= ddlDistrict.ClientID %>');
        var id = document.getElementById('<%= ddlVehicleNumber.ClientID %>');
        var fldReason = document.getElementById('<%= ddlreasons.ClientID %>');
        var fldContactNumber = document.getElementById('<%= txtContactNumber.ClientID %>');
        var fldOdo = document.getElementById('<%= txtOdo.ClientID %>');
        var fldEstCost = document.getElementById('<%= txtAllEstimatedCost.ClientID %>');
        var fldComments = document.getElementById('<%= txtComment.ClientID %>');
        var fldEmeId = document.getElementById('<%= txtEMEId.ClientID %>');
        var fldPilotId = document.getElementById('<%= txtPilotId.ClientID %>');
        var fldPilotName = document.getElementById('<%= txtPilotName.ClientID %>');
        var fldOfftime = document.getElementById('<%= txtOfftimeDate.ClientID %>');
        var fldHrs = document.getElementById('<%= ddlOFFHour.ClientID %>');
        var fldMins = document.getElementById('<%= ddlOFFMin.ClientID %>');
        var fldExpDateOfRecovery = document.getElementById('<%= txtExpDateOfRec.ClientID %>');
        var fldHrsEdr = document.getElementById('<%= ddlExpDateOfRecHr.ClientID %>');
        var fldMinsEdr = document.getElementById('<%= ddlExpDateOfRecMin.ClientID %>');
        var today = new Date();
        var dd = today.getDate();
        var mm = today.getMonth() + 1; //January is 0!
        var yyyy = today.getFullYear();
        if (dd < 10) {
            dd = '0' + dd;
        }
        if (mm < 10) {
            mm = '0' + mm;
        }
        var now = dd + '/' + mm + '/' + yyyy;
        if (fldDistrict && fldDistrict.selectedIndex === 0) {
            alert("Please Select State");
            fldDistrict.focus();
            return false;
        }

        var inputs = id.getElementsByTagName('input');
        var i;
        for (i = 0; i < inputs.length; i++) {
            if (inputs[i].type === 'text') {
                if (inputs[i].value !== "" && inputs[i].value != null)
                    if (inputs[i].value === "Select") {
                        alert('Select the Vehicle');
                        return false;
                    }

                break;
            }
        }

        if (fldReason)
            if (fldReason.selectedIndex === 0) {
                alert("Please select Reason");
                fldReason.focus();
                return false;
            }

        if (!RequiredValidation(fldContactNumber, "Contact number cannot be left blank"))
            return false;

        if (!RequiredValidation(fldOdo, "Odometer cannot be left blank"))
            return false;

        if (!RequiredValidation(fldEstCost, "Estimated Cost cannot be left blank"))
            return false;

        if (!RequiredValidation(fldComments, "Comments cannot be left blank"))
            return false;

        if (!RequiredValidation(fldEmeId, "EMEID cannot be left blank"))
            return false;

        if (!RequiredValidation(fldPilotId, "PilotId cannot be left blank"))
            return false;

        if (!RequiredValidation(fldPilotName, "PilotName cannot be left blank"))
            return false;

        if (!RequiredValidation(fldOfftime, "Uptime cannot be blank"))
            return false;

        if (fldHrs && fldHrs.selectedIndex === 0) {
            alert("Please select Hrs for Offtime Date");
            fldHrs.focus();
            return false;
        }

        if (fldMins && fldMins.selectedIndex === 0) {
            alert("Please select Mins for Offtime Date");
            fldMins.focus();
            return false;
        }

        today = new Date();
        dd = addZero(today.getDate());
        mm = addZero(today.getMonth() + 1); //January is 0!
        var h = addZero(today.getHours());
        var m = addZero(today.getMinutes());
        var s = addZero(today.getSeconds());
        yyyy = today.getFullYear();
        now = dd + '/' + mm + '/' + yyyy + " " + h + ":" + m + ":" + s;
        if (!RequiredValidation(fldExpDateOfRecovery, "Expected Date of Recovery cannot be blank"))
            return false;

        if (fldHrsEdr && fldHrsEdr.selectedIndex === 0) {
            alert("Please select Hrs for Expected Date of Recovery");
            fldHrsEdr.focus();
            return false;
        }

        if (fldMinsEdr && fldMinsEdr.selectedIndex === 0) {
            alert("Please select Mins for Expected Date of Recovery");
            fldMinsEdr.focus();
            return false;
        }

        if (Date.parse(fldOfftime.value + " " + fldHrs.value + ":" + fldMins.value) >=
            Date.parse(fldExpDateOfRecovery.value + " " + fldHrsEdr.value + ":" + fldMinsEdr.value)) {
            alert("Expected Date of Recovery should be greater than Offtime Date");
            fldExpDateOfRecovery.focus();
            return false;
        }

        document.getElementById("loaderButton").style.display = '';
        document.all('<%= pnlButton.ClientID %>').style.display = "none";
        return true;
    }
</script>
<fieldset style="padding: 10px">
<legend align="center" style="color: brown">Vehicle Off Road</legend>
<table align="center">
<tr>
    <td>
        State<span style="color: Red">*</span>
    </td>
    <td >
        <asp:DropDownList ID="ddlDistrict" runat="server" Width="150px" OnSelectedIndexChanged="ddlDistrict_SelectedIndexChanged"
                          AutoPostBack="True">
            <asp:ListItem Value="-1">--Select--</asp:ListItem>
        </asp:DropDownList>
    </td>
</tr>
<tr>
    <td class="rowseparator"></td>
</tr>
<tr>
    <td >
        Vehicle Number<span style="color: Red">*</span>
    </td>
    <td >
        <asp:DropDownList ID="ddlVehicleNumber" runat="server" Width="150px" OnSelectedIndexChanged="ddlVehicleNumber_SelectedIndexChanged"
                          AutoPostBack="True">
            <asp:ListItem Value="-1">--Select--</asp:ListItem>
        </asp:DropDownList>

    </td>
    <td class="columnseparator"></td>
    <td >
        <asp:Label ID="lblSegment" runat="server" Text="Base Location" Visible="false"></asp:Label>
    </td>
    <td class="columnseparator"></td>
    <td >
        <asp:Label ID="lblSegmentName" runat="server" Text="" Visible="false"></asp:Label>
    </td>
</tr>
<tr>
    <td class="rowseparator"></td>
</tr>
<tr>
    <td>
        Reason <span style="color: Red">*</span>
    </td>
    <td>
        <asp:DropDownList ID="ddlreasons" Width="150px" runat="server" AutoPostBack="True"
                          OnSelectedIndexChanged="ddlreasons_SelectedIndexChanged">
        </asp:DropDownList>
    </td>
</tr>
<tr>
    <td >
        Contact Number<span style="color: Red">*</span>
    </td>
    <td>
        <asp:TextBox ID="txtContactNumber" CssClass="search_3" runat="server" onkeypress="return numeric_only(event)"
                     MaxLength="12">
        </asp:TextBox>
    </td>
</tr>
<tr>
    <td class="rowseparator"></td>
</tr>
<div id="divAggre" runat="server">
    <tr>
        <td>
            Aggregates<span style="color: Red">*</span>
        </td>
        <td>
            <asp:DropDownList ID="ddlAggregates" CssClass="search_3" Width="150px" runat="server" AutoPostBack="True"
                              OnSelectedIndexChanged="ddlAggregates_SelectedIndexChanged">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td>
            Categories<span style="color: Red">*</span>
        </td>
        <td>
            <asp:DropDownList ID="ddlCategories" CssClass="search_3" Width="150px" runat="server" AutoPostBack="True"
                              OnSelectedIndexChanged="ddlCategories_SelectedIndexChanged">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td class="rowseparator"></td>
    </tr>
    <tr>
        <td>
            Sub-Categories<span style="color: Red">*</span>
        </td>
        <td>
            <asp:DropDownList ID="ddlSubCategories" CssClass="search_3" Width="150px" runat="server" OnSelectedIndexChanged="ddlSubCategories_SelectedIndexChanged"
                              AutoPostBack="True">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td style="margin-left: 50px">
            Estimated Cost<span style="color: Red">*</span>
        </td>

        <td>
            <asp:TextBox ID="txtEstCost" CssClass="search_3" runat="server" Width="150px" onkeypress="return isDecimalNumberKey(event);"/>
        </td>
    </tr>
    <tr>
        <td class="rowseparator"></td>
    </tr>
    <tr>

        <td>
            <asp:Button runat="server" ID="btnAdd" Text="Add" OnClick="btnAdd_Click"/>
        </td>
    </tr>
    <tr>
        <td class="rowseparator"></td>
    </tr>
    <br/>
    <table align="center">
        <tr>
            <td>
                <asp:GridView ID="grdvwBreakdownDetails" runat="server" BackColor="White" BorderColor="#CCCCCC"
                              BorderStyle="None" BorderWidth="1px" CellPadding="3" AutoGenerateDeleteButton="True"
                              OnRowDeleting="grdvwBreakdownDetails_RowDeleting">
                    <RowStyle ForeColor="#000066"/>
                    <FooterStyle BackColor="White" ForeColor="#000066"/>
                    <PagerStyle ForeColor="#000066" HorizontalAlign="Left" BackColor="White"/>
                    <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White"/>
                    <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White"/>
                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                    <SortedAscendingHeaderStyle BackColor="#007DBB" />
                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                    <SortedDescendingHeaderStyle BackColor="#00547E" />
                </asp:GridView>
            </td>
        </tr>
    </table>
</div>
<tr>
    <td class="rowseparator"></td>
</tr>
<tr>
    <td>
        Odometer<span style="color: Red">*</span>
    </td>
    <td>
        <asp:TextBox ID="txtOdo" CssClass="search_3" runat="server" onkeypress="return isNumberKey(event)" MaxLength="6"></asp:TextBox>
    </td>
</tr>
<tr>
    <td>
        Estimated Cost<span style="color: Red">*</span>
    </td>

    <td>
        <asp:TextBox ID="txtAllEstimatedCost" CssClass="search_3" runat="server" onkeypress="return onlyNumbers();"
                     Width="150px"/>
    </td>
</tr>

<tr>
    <td class="rowseparator"></td>
</tr>
<tr>
    <td>
        Comments<span style="color: Red">*</span>
    </td>
    <td>
        <asp:TextBox ID="txtComment" CssClass="search_3" runat="server" TextMode="MultiLine"></asp:TextBox>
    </td>
</tr>
<tr>
    <td class="rowseparator"></td>
</tr>
<tr>
    <td>
        Requested By (EME Name)<span style="color: Red">*</span>
    </td>
    <td>
        <asp:TextBox ID="txtReqBy" CssClass="search_3" runat="server" onkeypress="return onlyAlphabets(event,this);"
                     Width="150px">
        </asp:TextBox>
    </td>
</tr>
<tr>
    <td>
        EME ID<span style="color: Red">*</span>
    </td>
    <td>
        <asp:TextBox ID="txtEMEId" CssClass="search_3" onkeypress="return numeric_only(event)" runat="server"></asp:TextBox>
    </td>
</tr>
<tr>
    <td class="rowseparator"></td>
</tr>
<tr>
    <td>
        Pilot Name<span style="color: Red">*</span>
    </td>
    <td>
        <asp:TextBox ID="txtPilotName" CssClass="search_3" onkeypress="return onlyAlphabets(event,this);" runat="server"
                     Width="150px">
        </asp:TextBox>
    </td>
</tr>
<tr>
    <td>
        Pilot ID<span style="color: Red">*</span>
    </td>

    <td>
        <asp:TextBox ID="txtPilotId" CssClass="search_3" onkeypress="return numeric_only(event)" runat="server"></asp:TextBox>
    </td>
</tr>


</tr>
<tr>
    <td class="rowseparator"></td>
</tr>
<tr>
    <td>
        Off Time <span style="color: Red">*</span>
    </td>
    <td nowrap="nowrap" colspan="6">
        <table style="width: 100%">
            <tr>
                <td nowrap="nowrap" style="width: 20%">
                    <asp:TextBox ID="txtOfftimeDate" CssClass="search_3" runat="server" Width="150px" onkeypress="return false"></asp:TextBox>
                </td>
                <td class="columnseparator"></td>
                <td class="columnseparator"></td>
                <td >
                    <asp:DropDownList ID="ddlOFFHour" CssClass="search_3" runat="server" Width="55px" style="margin-left: 10px">
                        <asp:ListItem Value="-1">--hh--</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:DropDownList ID="ddlOFFMin" CssClass="search_3" runat="server" Width="60px" style="margin-left: 10px">
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
    <td>
        Expected Date of Recovery <span style="color: Red">*</span>
    </td>
    <td nowrap="nowrap" colspan="6">
        <table style="width: 100%">
            <tr>
                <td nowrap="nowrap" style="width: 20%">
                    <asp:TextBox ID="txtExpDateOfRec" CssClass="search_3" runat="server" Width="150px" onkeypress="return false"></asp:TextBox>
                </td>
                <td style="width: 80%">
                    <asp:DropDownList ID="ddlExpDateOfRecHr" CssClass="search_3" runat="server" Width="55px">
                        <asp:ListItem Value="-1">--hh--</asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddlExpDateOfRecMin" CssClass="search_3" runat="server" Width="60px">
                        <asp:ListItem Value="-1">--mm--</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
    </td>
</tr>
<tr>
    <td></td>
</tr>
<tr>
    <td colspan="7">
        <table width="100%">
            <tr>
                <td>
                    <asp:Label ID="lblmsg" runat="server" Visible="false" ForeColor="SteelBlue" Text="There are more than one Vehicles in that Segment,then no Segment Down "></asp:Label>
                    <asp:Panel ID="pnlRadioBtnList" runat="server" Visible="false">
                        <asp:RadioButtonList ID="rdbtnlstOption" runat="server" AutoPostBack="true" CellSpacing="5"
                                             RepeatDirection="Horizontal" OnSelectedIndexChanged="rdbtnlstOption_SelectedIndexChanged">
                            <asp:ListItem Text="Map Mandal to Other Segment" Value="rdbothersegment"></asp:ListItem>
                            <asp:ListItem Text="Place Other Segment Vehicle" Value="rdbothervehicle"></asp:ListItem>
                        </asp:RadioButtonList>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Panel ID="pnlothersegment" runat="server" Style="padding: 10px" Visible="false">
                        <fieldset style="padding: 10px" visible="false">
                            <legend>Map Mandal To Other Segment</legend>
                            <br/>
                            <asp:GridView ID="grdvothersegment" runat="server" CellPadding="3" AutoGenerateColumns="False" OnRowDataBound="grdvothersegment_RowDataBound" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px">
                                <FooterStyle BackColor="White" ForeColor="#000066"/>
                                <RowStyle ForeColor="#000066"/>
                                <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White"/>
                                <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left"/>
                                <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White"/>
                                <Columns>
                                    <asp:BoundField DataField="sg_lname" HeaderText="Mandal Name"/>
                                    <asp:TemplateField HeaderText="Segments">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="DropDownList1" runat="server">
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                <SortedDescendingHeaderStyle BackColor="#00547E" />
                            </asp:GridView>
                        </fieldset>
                    </asp:Panel>
                    <asp:Panel ID="pnlothervehicle" runat="server" Style="padding: 10px" Visible="false">
                        <fieldset style="padding: 10px">
                            <legend>Place Other Segment Vehicle</legend>
                            <br/>
                            <table width="100%">
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="Label3" runat="server" Text="Vehicles"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlothervehicle" CssClass="search_3" runat="server" AutoPostBack="True" Width="200px"
                                                          OnSelectedIndexChanged="ddlothervehicle_SelectedIndexChanged">
                                            <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td class="columnseparator"></td>
                                    <td align="left">
                                        <asp:Label ID="lblothercontactno" runat="server" Text="Contact Number"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtothercontactno" CssClass="search_3" runat="server" Width="124px" onkeypress="return false;"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="rowseparator"></td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="lblotherbaselocation" runat="server" Text="Base Location"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtotherbaselocation" CssClass="search_3" runat="server" Width="200px" onkeypress="return false;"></asp:TextBox>
                                    </td>
                                    <td class="columnseparator"></td>
                                    <td align="left">
                                        <asp:Label ID="lblOtherVehSegment" runat="server" Text="Segment" Visible="false"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lblOtherVehSegmentName" runat="server" Text="" Visible="false"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="rowseparator"></td>
                                </tr>
                                <tr>
                                    <td colspan="5" align="center">
                                        <asp:GridView ID="grdvothervehicle" runat="server" CellPadding="3" AutoGenerateColumns="False" OnRowDataBound="grdvothervehicle_RowDataBound" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px">
                                            <FooterStyle BackColor="White" ForeColor="#000066"/>
                                            <RowStyle ForeColor="#000066"/>
                                            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White"/>
                                            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left"/>
                                            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White"/>
                                            <Columns>
                                                <asp:BoundField DataField="sg_lname" HeaderText="Mandal Name"/>
                                                <asp:TemplateField HeaderText="Segments" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="DropDownlist2" runat="server">
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                            <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                            <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                            <SortedDescendingHeaderStyle BackColor="#00547E" />
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                        <br/>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </td>
</tr>
<tr>
    <td class="rowseparator"></td>
</tr>
<tr>
    <div style="top: 0px; width: 68px;"></div>
</tr>
<caption>
    <img id="loaderButton" alt="" src="images/savingimage.gif" style="display: none"/>
    <tr>
        <td align="center" colspan="7" style="">
            <asp:Panel ID="pnlButton" runat="server">
                <asp:Button ID="btnSubmit" CssClass="form-submit-button" runat="server" OnClick="btnSubmit_Click" Text="Submit"/>
                &nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnReset" runat="server" CssClass="form-reset-button" OnClick="btnReset_Click" Text="Reset"/>
            </asp:Panel>
        </td>
    </tr>
</caption>
</table>
</fieldset>
</asp:Content>