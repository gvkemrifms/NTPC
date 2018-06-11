<%@ Page Language="C#" MasterPageFile="~/temp.master" AutoEventWireup="true" CodeFile="DistrictUserMapping.aspx.cs" Inherits="DistrictUserMapping" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <script type="text/javascript">
        function validation() {
            var userList = document.getElementById('<%= ddlUserList.ClientID %>');
            switch (userList.selectedIndex) {
            case 0:
                return alert("Please select User Name");
            }
            return true;
        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <script type="text/javascript">
                function pageLoad() {
                    $('#<%= ddlUserList.ClientID %>').select2({
                        disable_search_threshold: 5,
                        search_contains: true,
                        minimumResultsForSearch: 2,
                        placeholder: "Select an option"
                    });
                }
            </script>
            <fieldset style="padding: 10px">
                <legend align="center" style="color: brown">State User Mapping</legend>
                <table style="width: 100%">
                    <tr>
                        <td align="top" align="center">
                            <table>
                                <tr>
                                    <td align="center">
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" valign="top">
                            <table>
                                <tr>
                                    <td valign="top" nowrap="nowrap">
                                        &nbsp;<asp:Label runat="server" Text="UserName: "></asp:Label>
                                    </td>
                                    <td valign="top">
                                        <asp:DropDownList ID="ddlUserList" runat="server" Width="150px" OnSelectedIndexChanged="ddlUserList_SelectedIndexChanged" AutoPostBack="True">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 5%">
                                    </td>
                                    <td nowrap="nowrap" valign="top">
                                        <asp:Label ID="lblDistrict" runat="server" Text="State: "></asp:Label>
                                    </td>
                                    <td valign="top" align="left">
                                        <asp:CheckBoxList ID="chkDistrictList" width="250px" runat="server">
                                        </asp:CheckBoxList>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" valign="top">
                            <asp:Button runat="server" Text="Save" CssClass="form-submit-button" OnClick="btnMapping_Click" OnClientClick="if (!validation()) return false;"/>
                            <asp:Button runat="server" Text="Cancel" CssClass="form-reset-button" OnClick="btnCancel_Click"/>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>