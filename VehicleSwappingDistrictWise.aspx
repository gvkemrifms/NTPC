<%@ Page Language="C#" MasterPageFile="~/temp.master" AutoEventWireup="true" CodeFile="VehicleSwappingDistrictWise.aspx.cs" Inherits="VehicleSwappingDistrictWise" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script language="javascript" type="text/javascript">

        function validation() {
            var district = document.getElementById('<%= ddlSourceDistrict.ClientID %>');
            var srcVehicle = document.getElementById('<%= ddlSrcVehicle.ClientID %>');
            var destVehicle = document.getElementById('<%= ddlDestVehicle.ClientID %>');
            var srcContactNo = document.getElementById('<%= txtSrcContactNo.ClientID %>');
            var destContactNo = document.getElementById('<%= txtDestContactNo.ClientID %>');

            if (district)
                switch (district.selectedIndex) {
                case 0:
                    alert("Please select District");
                    district.focus();
                    return false;
                }

            if (srcVehicle)
                switch (srcVehicle.selectedIndex) {
                case 0:
                    alert("Please select Source Vehicle");
                    srcVehicle.focus();
                    return false;
                }

            if (destVehicle && destVehicle.selectedIndex === 0) {
                alert("Please select Destination Vehicle");
                destVehicle.focus();
                return false;
            }

            if (!RequiredValidation(srcContactNo, "Source Contact Number Cannot be Blank"))
                return false;

            if (!RequiredValidation(destContactNo, "Destination Contact Number Cannot be Blank"))
                return false;

            document.getElementById("loaderButton").style.display = '';
            document.all('<%= pnlButton.ClientID %>').style.display = "none";
            return true;
        }

    </script>

    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <script type="text/javascript">
                function pageLoad() {
                    $('#<%= ddlSourceDistrict.ClientID %>').select2({
                        disable_search_threshold: 5,
                        search_contains: true,
                        minimumResultsForSearch: 20,
                        placeholder: "Select an option"
                    });
                    $('#<%= ddlSrcVehicle.ClientID %>').select2({
                        disable_search_threshold: 5,
                        search_contains: true,
                        minimumResultsForSearch: 20,
                        placeholder: "Select an option"
                    });
                    $('#<%= ddlDestDistrict.ClientID %>').select2({
                        disable_search_threshold: 5,
                        search_contains: true,
                        minimumResultsForSearch: 20,
                        placeholder: "Select an option"
                    });
                    $('#<%= ddlDestVehicle.ClientID %>').select2({
                        disable_search_threshold: 5,
                        search_contains: true,
                        minimumResultsForSearch: 20,
                        placeholder: "Select an option"
                    });
                }
            </script>
            <fieldset style="padding: 10px">
                <legend style="color: brown" align="center">District Vehicle Swapping</legend>
                <table style="width: 600px;" align="center">
                    <tr>
                        <td class="rowseparator"></td>
                    </tr>
                    <tr>
                        <td>
                            District
                        </td>
                        <td class="columnseparator"></td>
                        <td>
                            <asp:DropDownList ID="ddlSourceDistrict" runat="server" AutoPostBack="true" Width="155px"
                                              OnSelectedIndexChanged="ddlSourceDistrict_SelectedIndexChanged" style="border-right: 50px">
                                <asp:ListItem Value="-1">--Select--</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="columnseparator"></td>
                        <td>
                            Districts
                        </td>
                        <td class="columnseparator"></td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlDestDistrict" AutoPostBack="true" Width="155px"
                                              OnSelectedIndexChanged="ddlDestDistrict_SelectedIndexChanged">
                                <asp:ListItem Value="-1">--Select--</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="rowseparator"></td>
                    </tr>
                    <tr>
                        <td>
                            Source Vehicle
                        </td>
                        <td class="columnseparator"></td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlSrcVehicle" AutoPostBack="true" Width="155px"
                                              OnSelectedIndexChanged="ddlSrcVehicle_SelectedIndexChanged">
                                <asp:ListItem Value="-1">--Select--</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="columnseparator"></td>
                        <td>
                            Destination Vehicle
                        </td>
                        <td class="columnseparator"></td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlDestVehicle" AutoPostBack="true" Width="155px"
                                              OnSelectedIndexChanged="ddlDestVehicle_SelectedIndexChanged">
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
                            <asp:TextBox ID="txtSrcBaseLocation" runat="server" Width="150px" BackColor="#CCCCCC"
                                         ReadOnly="True">
                            </asp:TextBox>
                        </td>
                        <td class="columnseparator"></td>
                        <td>
                            Base Location
                        </td>
                        <td class="columnseparator"></td>
                        <td>
                            <asp:TextBox ID="txtDestBaseLocation" runat="server" Width="150px" BackColor="#CCCCCC"
                                         ReadOnly="True">
                            </asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="rowseparator"></td>
                    </tr>
                    <tr>
                        <td>
                            Contact Number
                        </td>
                        <td class="columnseparator"></td>
                        <td>
                            <asp:TextBox ID="txtSrcContactNo" runat="server" Width="150px" MaxLength="10" onkeypress="return numeric(event)"></asp:TextBox>
                        </td>
                        <td class="columnseparator"></td>
                        <td>
                            Contact Number
                        </td>
                        <td class="columnseparator"></td>
                        <td>
                            <asp:TextBox ID="txtDestContactNo" runat="server" Width="150px" MaxLength="10" onkeypress="return numeric(event)"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="rowseparator"></td>
                    </tr>
                    <tr>
                        <td>
                            Requested By
                        </td>
                        <td class="columnseparator"></td>
                        <td>
                            <asp:TextBox ID="txtRequestedBy" runat="server" ReadOnly="True" BackColor="#CCCCCC"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <div style="top: 0px; width: 68px;">
                        <caption>
                            <img src="../images/savingimage.gif" style="display: none" id="loaderButton" alt=""/>
                            <tr>
                                <td align="center" colspan="7" style="">
                                    <asp:Panel ID="pnlButton" runat="server">
                                        <asp:Button ID="btnSubmit" runat="server" CssClass="form-submit-button" OnClick="btnSubmit_Click" OnClientClick="return validation();" Text="Submit"/>
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btnReset" runat="server" CssClass="form-submit-button" OnClick="btnReset_Click" Text="Reset"/>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </caption>
                    </tr>
                </table>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>