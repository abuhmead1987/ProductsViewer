<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CategoriesManagement.ascx.cs" Inherits="Mohammad.Modules.ProductsViewer.Controls.CategoriesManagement" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="ImageUploader" Src="~/controls/filepickeruploader.ascx" %>
<%@ Register TagPrefix="blog" Namespace="DotNetNuke.Modules.Blog.Controls" Assembly="DotNetNuke.Modules.Blog" %>
<div id="dnnMyPanels">
    <h2 class="dnnFormSectionHead">
        <a href="#"><%= LocalizeString("AddRemoveCat.Text") %>
        </a>
    </h2>
    <fieldset>
        <div class="dnnClear container" id="dnnBlogEditContent">
            <fieldset>
                <div class="row">
                    <span class="spanLabel"><%= LocalizeString("Title.Text") %>  : </span>
                    <br />
                    <blog:ShortTextEdit id="txtTitle" runat="server" CssClass="blog_rte_full" RequiredResourceKey="Title.Required"
                        CssPrefix="blog_rte_" Required="true" Width="100%" />
                    <br />
                </div>
                <div class="row">
                    <span class="spanLabel"><%= LocalizeString("SortOrder.Text") %>  : </span>
                    <br />
                    <asp:TextBox ID="txt_Order" runat="server" type="number" Text="0"></asp:TextBox>
                    <br />
                </div>
                <div class="row">
                    <span class="spanLabel"><%= LocalizeString("ImageLink.Text") %>  : </span>
                    <br />
                    <dnn:ImageUploader ID="imagePicker1" runat="server" FileFilter="jpg,jpeg,gif,png" />
                    <br />
                </div>
                <div class="row">
                    <span class="spanLabel"><%= LocalizeString("bgImageLink.Text") %>  : </span>
                    <br />
                    <dnn:ImageUploader ID="bgImageUploader" runat="server" FileFilter="jpg,jpeg,gif,png" />
                    <br />
                </div>
                <div class="row">
                    <span class="spanLabel"><%= LocalizeString("Description.Text") %>  : </span>
                    <br />
                    <blog:LongTextEdit id="description" runat="server" Width="600" TextBoxHeight="200" TextBoxWidth="600" ShowRichTextBox="False" CssClass="blog_rte" CssPrefix="blog_rte_" />
                </div>
                <div class="row">
                    <asp:LinkButton ID="lnkbtn_Cancel" runat="server" class="dnnSecondaryAction btnMargins" OnClick="lnkbtn_Cancel_Click" CausesValidation="false"
                        Style="margin: 10px !important;"><%= LocalizeString("Cancel.Text") %></asp:LinkButton>
                    <asp:LinkButton ID="lnkbtn_Save" runat="server" class="dnnPrimaryAction btnMargins" OnClick="lnkbtn_Save_Click" Style="margin: 10px !important;"><%= LocalizeString("Save.Text") %></asp:LinkButton>
                </div>
            </fieldset>
        </div>

    </fieldset>
    <h2 class="dnnFormSectionHead">
        <a href="#"><%= LocalizeString("CategoriesList.Text") %>
        </a>
    </h2>
    <fieldset>
        <div class="row">
            <%--ProductsViewer_getCategoriesList--%>
            <asp:GridView ID="GridView_CatList" CssClass="table table-hover table-bordered " runat="server" AutoGenerateColumns="False" DataKeyNames="id" DataSourceID="SqlDataSource_catList" AllowPaging="True" AllowSorting="True"
                OnPreRender="GridView_CatList_PreRender" OnRowCommand="GridView_CatList_RowCommand">

                <EmptyDataTemplate>

                    <tr>
                        <td colspan="5">
                            <div><%= LocalizeString("NoData.Text") %></div>
                        </td>
                    </tr>
                </EmptyDataTemplate>

                <Columns>

                    <asp:BoundField DataField="id" HeaderText="id" ReadOnly="True" InsertVisible="False" SortExpression="id"></asp:BoundField>
                    <asp:BoundField DataField="name" HeaderText="name" SortExpression="name"></asp:BoundField>
                    <asp:BoundField DataField="description" HeaderText="description" SortExpression="description"></asp:BoundField>
                    <asp:BoundField DataField="imageURL" HeaderText="imageURL" SortExpression="imageURL" Visible="false"></asp:BoundField>
                    <asp:BoundField DataField="bgImageFileID" HeaderText="bgImageFileID" SortExpression="bgImageFileID" Visible="false"></asp:BoundField>
                    <asp:BoundField DataField="SortOrder" HeaderText="SortOrder" SortExpression="SortOrder"></asp:BoundField>



                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton ID="EditButton" CommandArgument='<%# Eval("id") %>' CommandName="EditRow" ToolTip="Edit" AlternateText="Edit" runat="server" ImageUrl="/images/eip_edit.png" CausesValidation="false" />
                            <asp:ImageButton ID="AddButton" CommandArgument='<%# Eval("id") %>' CommandName="InsertProduct" ToolTip="Add Product" AlternateText="Add Product" runat="server" ImageUrl="/images/add.gif" CausesValidation="false" />
                            <asp:ImageButton ID="DelButton" CommandArgument='<%# Eval("id") %>' CommandName="DeleteRow" ToolTip="Delete" AlternateText="Delete" runat="server" ImageUrl="/images/action_delete.gif" CssClass="gridDeleteCmd" CausesValidation="false" />
                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>
            </asp:GridView>
            <asp:SqlDataSource runat="server" ID="SqlDataSource_catList" ConnectionString='<%$ ConnectionStrings:SiteSqlServer %>' SelectCommand="ProductsViewer_getCategoriesList" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:Parameter Name="local" Type="String"></asp:Parameter>
                    <asp:Parameter Name="ModuleID" Type="String"></asp:Parameter>
                </SelectParameters>
            </asp:SqlDataSource>


        </div>
    </fieldset>
</div>
<script>





    (function ($, Sys) {
        function setupDnnBlogSettings() {
            $('.gridDeleteCmd').dnnConfirm({
                text: '<%=LocalizeString("DeleteConfirm.Text") %>',
                yesText: '<%= LocalizeString("Yes.Text") %>',
                noText: '<%= LocalizeString("No.Text") %>',
                title: '<%=LocalizeString("DeleteConfirm.Header") %>',
                isButton: true
            });
        };

        $(document).ready(function () {
            setupDnnBlogSettings();

            $('#dnnMyPanels').dnnPanels();
            $('#dnnMyPanels h2 a:not(.dnnSectionExpanded)').click();
            $("#dnnMyPanels h2 a").first().click();

            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                setupDnnBlogSettings();
            });
        });

    }(jQuery, window.Sys));

    function OnClientCommandExecuting(editor, args) { }
</script>
