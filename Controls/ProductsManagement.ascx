<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProductsManagement.ascx.cs" Inherits="Mohammad.Modules.ProductsViewer.Controls.ProductsManagement" %>
<%@ Register Assembly="DotNetNuke.Web" Namespace="DotNetNuke.Web.UI.WebControls.Internal" TagPrefix="dnn" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="ImageUploader" Src="~/controls/filepickeruploader.ascx" %>
<%@ Register TagPrefix="blog" Namespace="DotNetNuke.Modules.Blog.Controls" Assembly="DotNetNuke.Modules.Blog" %>
<div class="container">
<div id="dnnMyPanels">
    <h2 class="dnnFormSectionHead">
        <a href="#"><%= LocalizeString("AddRemoveProduct.Text") %>
        </a>
    </h2>
    <fieldset>
        <div class="dnnClear" id="dnnBlogEditContent">
            <fieldset>
                <div class="row">
                    <div style="width: 15%" class="leftAndRightFloat">
                        <span class="spanLabel"><%= LocalizeString("Category.Text") %>  : </span>
                    </div>
                    <div style="width: 33%" class="leftAndRightFloat">
                        <dnn:DnnComboBox runat="server"
                            ID="DnnCombo_ProdCat" runat="server" AutoPostBack="False" DataSourceID="SqlDataSource_DDCatsList" DataTextField="name" DataValueField="id"
                            CssClass="smallDDList"  ValidationGroup="Save"  CausesValidation="true" Width="400">
                        </dnn:DnnComboBox>
                    </div>
                </div>
                <div class="row">
                <span class="spanLabel" suffix=":"><%= LocalizeString("Title.Text") %> : </span>
                <br />
                <blog:ShortTextEdit id="txtTitle" runat="server" Required="true" CssClass="blog_rte_full" RequiredResourceKey="Title.Required"
                    CssPrefix="blog_rte_" Width="100%"  ValidationGroup="Save"  CausesValidation="true"/>
                <br />
                    </div>
                <div class="row">
                <span class="spanLabel"><%= LocalizeString("SortOrder.Text") %> : </span>
                <br />
                <asp:TextBox ID="txt_Order" runat="server" type="number" Text="0"></asp:TextBox>
                <br />
                    </div>
                <div class="row">
                <span class="spanLabel"><%= LocalizeString("ImageLink.Text") %> : </span>
                <br />
                <dnn:ImageUploader ID="imagePicker1" runat="server" Required="true" FileFilter="jpg,jpeg,gif,png" ValidationGroup="Save"  CausesValidation="true" />
                <br />
                    </div>
                <div class="row">
                <span class="spanLabel"><%= LocalizeString("Description.Text") %> : </span>
                <br />
                <blog:LongTextEdit id="description" runat="server" Width="100%" TextBoxHeight="500" TextBoxWidth="100%" ShowRichTextBox="True" CssClass="blog_rte" CssPrefix="blog_rte_" Required="true" />
                </div>
                    <div class="row">
                    <asp:LinkButton ID="lnkbtn_Cancel" runat="server" class="dnnSecondaryAction btnMargins" OnClick="lnkbtn_Cancel_Click" CausesValidation="false"
                        Style="margin: 10px !important;"><%= LocalizeString("Cancel.Text") %></asp:LinkButton>
                    <asp:LinkButton ID="lnkbtn_Save" runat="server" class="dnnPrimaryAction btnMargins" ValidationGroup="Save" CausesValidation="true" OnClick="lnkbtn_Save_Click" Style="margin: 10px !important;"><%= LocalizeString("Save.Text") %></asp:LinkButton>
                </div>
            </fieldset>
        </div>

    </fieldset>
    <h2 class="dnnFormSectionHead">
        <a href="#"><%= LocalizeString("ProductsList.Text") %>
        </a>
    </h2>
    <fieldset>
        <div class="row">

            <div style="width: 15%" class="leftAndRightFloat">
                <span class="spanLabel"><%= LocalizeString("Category.Text") %>  : </span>
            </div>
            <div style="width: 33%" class="leftAndRightFloat">
                <dnn:DnnComboBox runat="server"
                    ID="ddl_CatsList" runat="server" AutoPostBack="True" DataSourceID="SqlDataSource_DDCatsList" DataTextField="name" DataValueField="id"
                    OnSelectedIndexChanged="ddl_CatsList_SelectedIndexChanged" CssClass="smallDDList"
                    CausesValidation="false" Width="400">
                </dnn:DnnComboBox>

            </div>

            <asp:SqlDataSource ID="SqlDataSource_DDCatsList" runat="server" ConnectionString="<%$ ConnectionStrings:SiteSqlServer %>" SelectCommand="ProductsViewer_getCategoriesListForDD" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:Parameter Name="local" Type="String" />
                    <asp:Parameter Name="ModuleID" Type="String" />
                </SelectParameters>
            </asp:SqlDataSource>
        </div>

        <div class="row">
            <%--ProductsViewer_getCategoriesList--%>
            <asp:GridView ID="GridView_ProductsList" runat="server" CssClass="table table-bordered" AutoGenerateColumns="False" DataKeyNames="id" DataSourceID="SqlDataSource_ProductsList" AllowPaging="True" AllowSorting="True"
                OnPreRender="GridView_ProductsList_PreRender" OnRowCommand="GridView_ProductsList_RowCommand" >

                <EmptyDataTemplate>

                    <tr>
                        <td colspan="5">
                            <div><%= LocalizeString("NoData.Text") %></div>
                        </td>
                    </tr>
                </EmptyDataTemplate>

                <Columns>
                    <asp:BoundField DataField="id" HeaderText="id" ReadOnly="True" InsertVisible="False" SortExpression="id" ></asp:BoundField>
                    <asp:BoundField DataField="name" HeaderText="name" SortExpression="name"></asp:BoundField>
                    <asp:BoundField DataField="SortOrder" HeaderText="SortOrder" SortExpression="SortOrder"></asp:BoundField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton ID="EditProdButton" CommandArgument='<%# Eval("id") %>' CommandName="EditProd" ToolTip="Edit" AlternateText="Edit" runat="server" ImageUrl="/images/eip_edit.png" CausesValidation="false" />
                            <asp:ImageButton ID="DelButton"  CommandArgument='<%# Eval("id") %>' CommandName="DeleteRow" ToolTip="Delete" AlternateText="Delete" runat="server" ImageUrl="/images/action_delete.gif" CssClass="gridDeleteCmd"  CausesValidation="false" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:SqlDataSource runat="server" ID="SqlDataSource_ProductsList" ConnectionString='<%$ ConnectionStrings:SiteSqlServer %>' SelectCommand="ProductsViewer_getCategoryProductsList" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:Parameter Name="local" Type="String"></asp:Parameter>
                    <asp:Parameter Name="catID" Type="String"></asp:Parameter>
                </SelectParameters>
            </asp:SqlDataSource>
        </div>
    </fieldset>
</div>
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

    function OnClientCommandExecuting(editor, args) {}
</script>

