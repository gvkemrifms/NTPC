<%@ Page Title="" Language="C#" MasterPageFile="~/temp.master" AutoEventWireup="true" CodeFile="DistrictVehicleMapping.aspx.cs" Inherits="DistrictVehicleMapping" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript" src="http://maps.googleapis.com/maps/api/js?client=gme-gvkemergencymanagement3&places=West+Bengal&libraries=places"></script>
    <script src="locationpicker.js"></script>
    <title>:: GVK EMRI ::</title>
    <style>
        .pac-container:after { content: none !important; }

        .hightmap { height: 400px !important; }

        .modal {
            background-color: rgb(0, 0, 0); /* Fallback color */
            background-color: rgba(0, 0, 0, 0.4); /* Black w/ opacity */
            display: none; /* Hidden by default */
            height: 100%; /* Full height */
            left: 0;
            overflow: auto; /* Enable scroll if needed */
            padding-top: 100px; /* Location of the box */
            position: fixed; /* Stay in place */
            top: 0;
            width: 100%; /* Full width */
            z-index: 1; /* Sit on top */
        }

        .modal-content {
            background-color: #fefefe;
            border: 1px solid #888;
            height: 100%;
            margin: auto;
            padding: 20px;
            width: 80%;
        }

        .close {
            color: #aaaaaa;
            float: right;
            font-size: 28px;
            font-weight: bold;
        }
    </style>


    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <script type="text/javascript">
                function pageLoad() {
                    $('#<%= ddlVehicleNumber.ClientID %>').select2({
                        disable_search_threshold: 5,
                        search_contains: true,
                        minimumResultsForSearch: 2,
                        placeholder: "Select an option"
                    });
                    $('#<%= ddlDistrict.ClientID %>').select2({
                        disable_search_threshold: 5,
                        search_contains: true,
                        minimumResultsForSearch: 2,
                        placeholder: "Select an option"
                    });
                    $('#<%= ddlMandal.ClientID %>').select2({
                        disable_search_threshold: 5,
                        search_contains: true,
                        minimumResultsForSearch: 2,
                        placeholder: "Select an option"
                    });
                    $('#<%= ddlCity.ClientID %>').select2({
                        disable_search_threshold: 5,
                        search_contains: true,
                        minimumResultsForSearch: 2,
                        placeholder: "Select an option"
                    });
                    $('#<%= ddlBaseLocation.ClientID %>')
                        .chosen({ disable_search_threshold: 5, search_contains: true });
                    $('#<%= ddlVehType.ClientID %>').chosen({ disable_search_threshold: 5, search_contains: true });
                }
            </script>
            <script type="text/javascript">

                function validation() {

                    var fldDistrict = document.getElementById('<%= ddlDistrict.ClientID %>');
                    var fldVehicleNumber = document.getElementById('<%= ddlVehicleNumber.ClientID %>');
                    var fldMandals = document.getElementById('<%= ddlMandal.ClientID %>');
                    var fldCity = document.getElementById('<%= ddlCity.ClientID %>');
                    var fldContactNumber = document.getElementById('<%= txtContactNumber.ClientID %>');
                    var vehicleType = document.getElementById('<%= ddlVehType.ClientID %>');
                    var txtLatitudes = document.getElementById('<%= txtLatitude.ClientID %>');
                    var txtLongitudes = document.getElementById('<%= txtLongitude.ClientID %>');
                    var inputs = fldVehicleNumber.getElementsByTagName('input');
                    var i;
                    for (i = 0; i < inputs.length; i++) {
                        switch (inputs[i].type) {
                        case 'text':
                            if (inputs[i].value !== "" && inputs[i].value != null && inputs[i].value === "--Select--") {
                                alert('Please select Vehicle Number');
                                return false;
                            }
                            break;
                        }
                    }
                    if (fldVehicleNumber.selectedIndex === 0) {
                        alert("Please Select Vehicle");
                        fldVehicleNumber.focus();
                        return false;
                    }
                    if (fldDistrict && fldDistrict.selectedIndex === 0) {
                        alert("Please Select State");
                        fldDistrict.focus();
                        return false;
                    }
                    switch (fldMandals.selectedIndex) {
                    case 0:
                        alert("Please select Mandal");
                        fldMandals.focus();
                        return false;
                    }
                    switch (fldCity.selectedIndex) {
                    case 0:
                        alert("Please select City");
                        fldCity.focus();
                        return false;
                    }

                    if (!RequiredValidation(fldContactNumber, "Please select Contact Number"))
                        return false;
                    if (vehicleType.selectedIndex === 0) {
                        alert("Please Select VehicleType");
                        return false;
                    }
                    if (!RequiredValidation(txtLatitudes, "Please select Latitude"))
                        return false;
                    if (!RequiredValidation(txtLongitudes, "Please select Longitude"))
                        return false;
                    return true;
                }

                document.getElementById("loaderButton").style.display = '';
                function open() {
                    var modal = document.getElementById('myModal');
                    var span = document.getElementsByClassName("close")[0];
                    modal.style.display = "block";
                    span.onclick = function() {
                        modal.style.display = "none";
                    };
                    window.onclick = function(event) {
                        if (event.target === modal) {
                            modal.style.display = "none";
                        }
                    };
                }

            </script>
            <div align="center">
                <legend align="center" style="color: brown" >State Vehicle Mapping</legend>
                <table align="center">
                    <tr>
                        <td>
                            <table width="100%">
                                <tr>
                                    <td>
                                        Vehicle Number<span style="color: Red">*</span>
                                    </td>
                                    <td class="columnseparator"></td>
                                    <td>
                                        <asp:DropDownList ID="ddlVehicleNumber" runat="server" AutoPostBack="true" Width="150px" OnSelectedIndexChanged="ddlVehicleNumber_SelectedIndexChanged">
                                            <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="rowseparator"></td>
                                </tr>
                                <tr>
                                    <td>
                                        State<span style="color: Red">*</span>
                                    </td>
                                    <td class="columnseparator"></td>
                                    <td>
                                        <asp:DropDownList ID="ddlDistrict" runat="server" AutoPostBack="True" Width="150px" OnSelectedIndexChanged="ddlDistrict_SelectedIndexChanged">
                                            <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                        </asp:DropDownList>
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
                                        Mandal<span style="color: Red">*</span>
                                    </td>
                                    <td class="columnseparator"></td>
                                    <td>
                                        <asp:DropDownList ID="ddlMandal" runat="server" AutoPostBack="True" Width="150px" OnSelectedIndexChanged="ddlMandal_SelectedIndexChanged">
                                            <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="rowseparator"></td>
                                </tr>
                                <tr>
                                    <td>
                                        City/ Village<span style="color: Red">*</span>
                                    </td>
                                    <td class="columnseparator"></td>
                                    <td>
                                        <asp:DropDownList ID="ddlCity" runat="server" AutoPostBack="True" Width="150px" OnSelectedIndexChanged="ddlCity_SelectedIndexChanged">
                                            <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>

                                </tr>
                                <tr>
                                    <td class="rowseparator"></td>
                                </tr>
                                <tr>
                                    <td>
                                        Base Location
                                    </td>
                                    <td class="columnseparator"></td>
                                    <td>
                                        <table width="100%">
                                            <tr>
                                                <td style="width: 20%">
                                                    <asp:DropDownList ID="ddlBaseLocation" runat="server" AutoPostBack="True" Width="150px" OnSelectedIndexChanged="ddlBaseLocation_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <asp:TextBox ID="txtBaseLocation" runat="server" Width="150px" CssClass="search_3" Visible="false"></asp:TextBox>
                                                </td>
                                                <td style="margin: 5px">
                                                    <script>
                                                        function abc() {
                                                            $('#us2').locationpicker({
                                                                location: {
                                                                    latitude: '22.6754807',
                                                                    longitude: '88.0883874'
                                                                },
                                                                radius: 20,
                                                                zoom: 7,
                                                                inputBinding: {
                                                                    latitudeInput: $('#us2lat'),
                                                                    latitudeInput: $('.txtLatitude'),
                                                                    longitudeInput: $('#us2lon'),
                                                                    longitudeInput: $('.txtLongitude'),
                                                                    radiusInput: $('#us2radius'),
                                                                    locationNameInput: $('#address')
                                                                },
                                                                enableAutocomplete: true,
                                                                onchanged: function(currentLocation,
                                                                    radius,
                                                                    isMarkerDropped) {
                                                                    $('#us2lon').val(currentLocation.longitude);

                                                                }
                                                            });
                                                        }


                                                    </script>
                                                    <asp:LinkButton ID="lnkbtnExtngBaseLoc" runat="server" Visible="false" OnClick="lnkbtnExtngBaseLoc_Click">Existing Base Location</asp:LinkButton>
                                                    <asp:LinkButton ID="lnkbtnNewBaseLoc" runat="server" OnClick="lnkbtnNewBaseLoc_Click">New Base Location</asp:LinkButton>
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
                                        Contact Number<span style="color: Red">*</span>
                                    </td>
                                    <td class="columnseparator"></td>
                                    <td>
                                        <asp:TextBox ID="txtContactNumber" runat="server" class="search_3" Width="150px" onkeypress="return numeric_only(event)"
                                                     MaxLength="10">
                                        </asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="rowseparator"></td>
                                </tr>
                                <tr>
                                    <td>
                                        Vehicle Type <span style="color: red">*</span>
                                    </td>
                                    <td></td>
                                    <td>
                                        <asp:DropDownList ID="ddlVehType" Width="150px" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblLatitude" runat="server" Text="Latitude" Visible="false"></asp:Label>
                                        <asp:Label ID="lblMandatory1" runat="server" Text="*" ForeColor="Red" Visible="false"></asp:Label>
                                    </td>
                                    <td class="columnseparator"></td>
                                    <td>
                                        <asp:TextBox ID="txtLatitude" runat="server" Width="150px" Visible="false" CssClass="search_3" onblur="isDecimal(this);"
                                                     onkeydown="return numericOnly(this);">
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
                                        <asp:TextBox ID="txtLongitude" runat="server" CssClass="search_3" Width="150px" Visible="false" onblur="isDecimal(this);"
                                                     onkeydown="return numericOnly(this);">
                                        </asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table align="center">
                                <caption>
                                <img id="loaderButton" alt="" src="images/savingimage.gif" style="display: none"/>
                                <tr>
                                    <td align="center">
                                        <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" CssClass="form-submit-button"/>
                                    </td>
                                    <td class="columnseparator" style="width: 50px"></td>
                                    <td align="center">
                                        <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" CssClass="form-submit-button"/>
                                    </td>
                                </tr>
                                </caption>
                            </table>
                        </td>
                    </tr>
                </table>


                <div>
                </div>
            </div>
            <br/>
            <div align="center">
                    <asp:Label ID="lblVeh" runat="server"></asp:Label>
                <asp:GridView ID="grdVehicleData" runat="server" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3">
                    <FooterStyle BackColor="White" ForeColor="#000066" />
                    <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                    <RowStyle ForeColor="#000066" />
                    <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                    <SortedAscendingHeaderStyle BackColor="#007DBB" />
                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                    <SortedDescendingHeaderStyle BackColor="#00547E" />
                </asp:GridView>
            </div>
            <div id="myModal" class="modal" align="center">
                <div class="modal-content">
                    <span class="close">×</span>

                    <input type="text" name="address" id="address" style="width: 40%">
                    <input type="text" id="us2lat" value=""/><input type="text" id="us2lon" value=""/>
                    <div id="us2" style="height: 100%; width: 100%;" class=""></div>
                </div>
            </div>

        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>