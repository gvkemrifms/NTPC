<%@ Page Language="C#" MasterPageFile="~/temp.master" AutoEventWireup="true" CodeFile="VehicleSwapping.aspx.cs" Inherits="VehicleSwapping" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript" language="javascript">

        function validation() {
            var district = document.getElementById('<%= ddlDistrict.ClientID %>');
            var srcVehicle = document.getElementById('<%= ddlSrcVehicle.ClientID %>');
            var destVehicle = document.getElementById('<%= ddlDestVehicle.ClientID %>');
            var srcContactNo = document.getElementById('<%= txtSrcContactNo.ClientID %>');
            var destContactNo = document.getElementById('<%= txtDestContactNo.ClientID %>');

            if (district && district.selectedIndex === 0) {
                alert("Please Select District");
                district.focus();
                return false;
            }

            if (srcVehicle && srcVehicle.selectedIndex === 0) {
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
                    $('#<%= ddlDistrict.ClientID %>').select2({
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
                    $('#<%= ddlDestVehicle.ClientID %>').select2({
                        disable_search_threshold: 5,
                        search_contains: true,
                        minimumResultsForSearch: 20,
                        placeholder: "Select an option"
                    });

                }
            </script>
            <fieldset style="padding: 10px">
                <legend align="center" style="color: brown">Vehicle Swapping</legend>
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
                            <asp:DropDownList ID="ddlDistrict" runat="server" AutoPostBack="true" Width="155px"
                                              OnSelectedIndexChanged="ddlDistrict_SelectedIndexChanged">
                                <asp:ListItem Value="-1">--Select--</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="columnseparator"></td>
                        <td>
                            Requested By
                        </td>
                        <td class="columnseparator"></td>
                        <td>
                            <asp:TextBox ID="txtRequestedBy" runat="server" ReadOnly="True" BackColor="#CCCCCC"></asp:TextBox>
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
                            <asp:DropDownList ID="ddlSrcVehicle" runat="server" AutoPostBack="true" Width="155px"
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
                            <asp:DropDownList ID="ddlDestVehicle" runat="server" AutoPostBack="true" Width="155px"
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
                        <div style="top: 0px; width: 68px;">
                    </tr>
                    <caption>
                        <img id="loaderButton" alt="" src="../images/savingimage.gif"
                             style="display: none"/>
                        <tr>
                            <td align="center" colspan="7" style="">
                                <asp:Panel ID="pnlButton" runat="server">
                                    <asp:Button ID="btnSubmit" runat="server" CssClass="form-submit-button" OnClick="btnSubmit_Click"
                                                Text="Submit"/>
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="btnReset" runat="server" CssClass="form-reset-button" OnClick="btnReset_Click"
                                                Text="Reset"/>
                                </asp:Panel>
                            </td>
                        </tr>
                    </caption>
                </table>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>