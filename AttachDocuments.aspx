<%@ Page AutoEventWireup="true" CodeFile="AttachDocuments.aspx.cs" Inherits="AttachDocuments" Language="C#" MasterPageFile="~/temp.master" %>
<%@ Import Namespace="System.ComponentModel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <script type="text/javascript">
                function pageLoad() {
                    $('#<%= ddlistVehicleNumber.ClientID %>').select2({
                        disable_search_threshold: 5,
                        search_contains: true,
                        minimumResultsForSearch: 20,
                        placeholder: "Select an option"
                    });
                }

                function ClearItems() {

                    $('#<%= ddlistVehicleNumber.ClientID %>').empty();
                    $('#<%= ddlistAttachmentPurpose.ClientID %>').empty();
                    $('#<%= txtRemarks.ClientID %>').val('');
                    $('#<%= ddlistVehicleNumber.ClientID %>').chosen();
                }

                function Validations() {
                    var remarks = $('#<%= txtRemarks.ClientID %>').val();
                    var fileAttachments = $('#<%= fileAttachmentPurpose.ClientID %>').val();
                    var ddlVehicle = $('#<%= ddlistVehicleNumber.ClientID %> option:selected').text().toLowerCase();
                    var attachmentPurpose = $('#<%= ddlistAttachmentPurpose.ClientID %> option:selected').text()
                        .toLowerCase();
                    if (ddlVehicle === '--select--')
                        return alert("Please select Vehicle");
                    if (attachmentPurpose === 'select')
                        return alert("Please select Attachment Purpose");
                    if (fileAttachments === "")
                        return alert("File Attachment is Mandatory");
                    if (remarks === "")
                        return alert('Remarks is Mandatory');
                    return true;
                }
            </script>
            <table align="center">
                <tr>
                    <td colspan="3">
                        <asp:Label style="color: brown; font-size: 20px;" runat="server" Text="Attach&nbsp;Documents"></asp:Label>
                    </td>
                </tr>
            </table>
            <br/>
            <table align="center">
                <tr>
                    <td class="rowseparator"></td>
                </tr>
                <tr>
                    <td class="tdlabel">
                        Vehicle Number<span style="color: red">*</span>
                    </td>
                    <td class="columnseparator"></td>
                    <td>
                        <asp:DropDownList ID="ddlistVehicleNumber" Width="150px" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="rowseparator"></td>
                </tr>
                <tr>
                    <td class="tdlabel">
                        Attachment Purpose<span style="color: red">*</span>
                    </td>
                    <td class="columnseparator"></td>
                    <td>
                        <asp:DropDownList ID="ddlistAttachmentPurpose" CssClass="search_3" runat="server">
                            <asp:ListItem>SELECT</asp:ListItem>
                            <asp:ListItem Value="General">General</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="rowseparator"></td>
                </tr>
                <tr>
                    <td class="tdlabel">
                        Remarks<span style="color: red">*</span>
                    </td>
                    <td class="columnseparator"></td>
                    <td>
                        <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" onkeypress="return remark(event);" CssClass="txtbox" Height="100px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="rowseparator"></td>
                </tr>
                <tr>
                    <td class="tdlabel" style="bottom: 30px; margin-top: -50px; position: relative;">
                        Attachment<span style="color: red">*</span>
                    </td>
                    <td class="columnseparator"></td>
                    <td>
                        <br/>

                        <asp:FileUpload ID="fileAttachmentPurpose" runat="server" ForeColor="red"/>
                        <asp:Button ID="btnAttachFiles" runat="server" Text="Attach Files" class="form-submit-button" Enabled="true"
                                    OnClick="btnAttachFiles_Click" OnClientClick="if (!Validations()) return false;"/>
                        <asp:Button runat="server" Text="Upload" Visible="False" OnClick="btnUpload_Click"/>
                        <asp:Button runat="server" Text="Cancel" Visible="True" class="form-submit-button" OnClientClick="ClearItems()"/>
                    </td>
                </tr>
                <tr>
                    <td class="rowseparator"></td>
                </tr>
                <tr>
                    <td colspan="3">

                        <asp:Button runat="server" Text="Hide/View Attached Files"
                                    Visible="False"/>
                    </td>
                </tr>
                <tr>
                    <td class="rowseparator"></td>
                </tr>

            </table>
            <br/>
            <br/>
            <table align="center">
                <tr>
                    <td colspan="3">
                        <asp:GridView ID="grdVehicleAttachment" runat="server" AutoGenerateColumns="False"
                                      Width="80%" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px"
                                      CellPadding="3" AllowPaging="True"
                                      OnPageIndexChanging="grdVehicleAttachment_PageIndexChanging">
                            <RowStyle ForeColor="#000066"/>
                            <Columns>
                                <asp:TemplateField HeaderText="File Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFileName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "AttachmentPurposeFile") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Remarks">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRemarks" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Remarks") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Attachment Purpose">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAttachmentPurpose" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "AttachmentPurpose") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="UploadDate">
                                    <ItemTemplate>
                                        <asp:Label ID="lblUploadDate" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "CreatedDate", "{0:dd-MM-yyyy}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="UploadBy">
                                    <ItemTemplate>
                                        <asp:Label ID="lblUploadBy" runat="server" Text="FE"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Delete" Visible="false">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkBtnDelete" runat="server" Text="Delete" CommandArgument='<% DataBinder.Eval(Container.Dataitem, "") %>'
                                                        CommandName="vehicleAccidentDelete">
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <FooterStyle BackColor="White" ForeColor="#000066"/>
                            <RowStyle BackColor="#FFF7E7" ForeColor="#8C4510"/>
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
                <tr>
                    <td class="rowseparator"></td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnAttachFiles"/>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>