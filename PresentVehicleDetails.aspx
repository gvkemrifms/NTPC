<%@ Page Language="C#" MasterPageFile="~/temp.master" AutoEventWireup="true" CodeFile="PresentVehicleDetails.aspx.cs" Inherits="PresentVehicleDetails" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <fieldset style="padding: 10px">
                <legend align="center" style="color: brown">Vehicle Wise Record</legend>
                <table align="center">
                    <tr>
                        <td class="rowseparator">
                            <div style="height: 500px; overflow: scroll;">
                                <asp:Accordion ID="Accordion_VehicleNumber" runat="server" Width="650px" HeaderCssClass="accordionHeader"
                                               ContentCssClass="accordionContent" AutoSize="Fill" FadeTransitions="true" TransitionDuration="50">
                                    <HeaderTemplate>
                                        <%#DataBinder.Eval(Container.DataItem, "VehicleNumber") %>
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <table cellpadding="2" cellspacing="2" width="600px" border="1px solid brown">
                                            <tr>
                                                <td class="rowseparator" colspan="5"></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    District:
                                                </td>
                                                <td class="columnseparator"></td>
                                                <td align="left" nowrap="nowrap">
                                                    <asp:Label ID="lblVehicleID" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "VehicleID") %>'
                                                               Visible="false">
                                                    </asp:Label>
                                                    <asp:Label ID="lblDistrict" runat="server"><%#DataBinder.Eval(Container.DataItem, "District") %></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="rowseparator" colspan="5"></td>
                                            </tr>
                                            <tr>
                                                <td nowrap="nowrap">
                                                    <asp:Label ID="lblRoadTaxStart" runat="server">RoadTax StartDate: <%#DataBinder.Eval(Container.DataItem, "RT Start Date") %></asp:Label>
                                                </td>
                                                <td class="columnseparator"></td>
                                                <td nowrap="nowrap">
                                                    <asp:Label ID="lblRoadTaxEnd" runat="server">RoadTax End Date: <%#DataBinder.Eval(Container.DataItem, "RT End Date") %></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="rowseparator" colspan="5"></td>
                                            </tr>
                                            <tr>
                                                <td nowrap="nowrap">
                                                    <asp:Label ID="lblPucStartDate" runat="server">Pollution Under Control Start Date: <%#DataBinder.Eval(Container.DataItem, "PUC Start Date") %></asp:Label>
                                                </td>
                                                <td class="columnseparator"></td>
                                                <td nowrap="nowrap">
                                                    <asp:Label ID="lblPucEndDate" runat="server">Pollution Under Control End Date: <%#DataBinder.Eval(Container.DataItem, "PUC End Date") %></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="rowseparator" colspan="5"></td>
                                            </tr>
                                            <tr>
                                                <td nowrap="nowrap">
                                                    <asp:Label ID="Label1" runat="server">FitnessRenewal Start Date: <%#DataBinder.Eval(Container.DataItem, "FR Start Date") %></asp:Label>
                                                </td>
                                                <td class="columnseparator"></td>
                                                <td nowrap="nowrap">
                                                    <asp:Label ID="Label2" runat="server">FitnessRenewal End Date: <%#DataBinder.Eval(Container.DataItem, "FR End Date") %></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="rowseparator" colspan="5"></td>
                                            </tr>
                                            <tr>
                                                <td nowrap="nowrap">
                                                    <asp:Label ID="Label3" runat="server">Vehicle Insurance Start Date: <%#DataBinder.Eval(Container.DataItem, "VI Start Date") %></asp:Label>
                                                </td>
                                                <td class="columnseparator"></td>
                                                <td nowrap="nowrap">
                                                    <asp:Label ID="Label4" runat="server">Vehicle Insurance End Date: <%#DataBinder.Eval(Container.DataItem, "VI End Date") %></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="rowseparator" colspan="5"></td>
                                            </tr>
                                            <tr>
                                                <td nowrap="nowrap" colspan="3">
                                                    <asp:Label ID="Label5" runat="server">Latest Fuel Entry Date: <%#DataBinder.Eval(Container.DataItem, "Fuel Entry Date") %></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="rowseparator" colspan="5"></td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:Accordion>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="rowseparator"></td>
                    </tr>
                </table>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>