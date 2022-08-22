<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProductsPreview.ascx.cs" Inherits="Mohammad.Modules.ProductsViewer.Controls.ProductsPreview" %>

<%--<div class="container-fluid products">
        <div class="container-fluid logoAndHeader" id="logoBG" runat="server">
            <div class="container">
                <asp:Image CssClass="logo" ID="categoryLogo" runat="server" ImageUrl="/Images/ReckittBenckiser.png"/>
                <div class="row">
                    <div class="col-lg-8">
                        <p class="normalp">
                            <asp:Literal ID="categoryDesc" runat="server"></asp:Literal>
                            </p>
                    </div>
                </div>

            </div>
        </div>
    </div>--%>
<div class="container">
    <div class="row brands">
        <label class="brandsLabel"><%= LocalizeString("Brands.Text") %>  :</label>
        <div class="categoriesList leftAndRightFloat">
            <asp:Repeater ID="Repeater_CatsList" runat="server" DataSourceID="SqlDataSource_DDCatsList">
                <HeaderTemplate>
                    <ul class="brandsList">
                </HeaderTemplate>
                <ItemTemplate>
                    <li id="CatItem" runat="server">
                        <asp:LinkButton ID="lnkbtn_CatsList" runat="server" CommandArgument='<%# Bind("id") %>' CommandName="ShowProducts" OnCommand="lnkbtn_CatsList_Command">
                            <asp:Literal ID="Literal3" runat="server" Text='<%# Eval("name")%>'></asp:Literal>
                        </asp:LinkButton>
                    </li>
                </ItemTemplate>
                <FooterTemplate>
                    </ul>
                </FooterTemplate>
            </asp:Repeater>
            <%-- <asp:ListBox ID="ListBox_Cats" runat="server" OnSelectedIndexChanged="ListBox_Cats_SelectedIndexChanged" DataSourceID="SqlDataSource_DDCatsList"
                DataTextField="name" DataValueField="id" AutoPostBack="True" CssClass="categoriesListMenu" Width="100%"
                SelectionMode="Single"></asp:ListBox>--%>

            <asp:SqlDataSource ID="SqlDataSource_DDCatsList" runat="server" ConnectionString="<%$ ConnectionStrings:SiteSqlServer %>" SelectCommand="ProductsViewer_getCategoriesListForDD" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:Parameter Name="local" Type="String" />
                    <asp:Parameter Name="ModuleID" Type="String" />
                </SelectParameters>
            </asp:SqlDataSource>
        </div>
    </div>

    <div class="row productsList">
        <asp:HiddenField ID="hf_ActiveA" runat="server" ClientIDMode="Static" Value="-1" />
        <div class="row">
            <div runat="server" id="divNoData" visible="false"><%= LocalizeString("NoData.Text") %></div>

            <div class="productsGallery leftAndRightFloat">
                <asp:Repeater ID="Repeater_Products" runat="server" DataSourceID="SqlDataSource_ProductsList">
                    <HeaderTemplate>
                    </HeaderTemplate>
                    <ItemTemplate>

                        <asp:LinkButton CssClass="card" ID="lnkbtn_ProductsGallery" runat="server" CommandArgument='<%# Bind("id") %>' CommandName="ShowDetails" OnCommand="lnkbtn_ProductsGallery_Command">

                            <img src='<%# getFilePath(Eval("imageFileID").ToString()) %>' />
                            <div class="cardcontainer">
                                <asp:Literal ID="Literal4" runat="server" Text='<%# Bind("name")%>'></asp:Literal>
                            </div>
                        </asp:LinkButton>

                    </ItemTemplate>
                    <FooterTemplate>
                    </FooterTemplate>
                </asp:Repeater>
                <asp:Repeater ID="Repeater_ProdDesc" runat="server" DataSourceID="SqlDataSource_ProdDesc" Visible="true">
                    <ItemTemplate>
                        <div class="row">
                            <h2>
                                <asp:Literal ID="Literal1" runat="server" Text='<%# Bind("name")%>'></asp:Literal></h2>
                        </div>
                        <div class="row">
                            <div class="imgHolder leftAndRightFloat">
                                <img src='<%# getFilePath(Eval("imageFileID").ToString()) %>' />
                            </div>
                            <div class="descHolder leftAndRightFloat">
                                <div>
                                    <asp:Literal ID="Literal2" runat="server" Text='<%# decodeHTML(Eval("description").ToString())%>'> </asp:Literal>

                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
                <asp:Panel ID="Panel1" runat="server">
                    <div class="row">
                    </div>
                </asp:Panel>
                <asp:SqlDataSource runat="server" ID="SqlDataSource_ProductsList" ConnectionString='<%$ ConnectionStrings:SiteSqlServer %>' SelectCommand="ProductsViewer_getCategoryProductsList" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:Parameter Name="local" Type="String"></asp:Parameter>
                        <asp:Parameter Name="catID" Type="String"></asp:Parameter>
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource runat="server" ID="SqlDataSource_ProdDesc" ConnectionString='<%$ ConnectionStrings:SiteSqlServer %>' SelectCommand="ProductsViewer_getCategoryProductDesc" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:Parameter Name="local" DefaultValue="ar-JO" Type="String"></asp:Parameter>
                        <asp:Parameter Name="id" DefaultValue="-1" Type="String"></asp:Parameter>
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
        </div>
    </div>
</div>
<script type="text/javascript">
    <%--$(document).ready(function () {

        try{
            $("#" + <%=Session["ControlID"].ToString() %>).addClass("active");
            console.info($("#hf_ActiveA").val());
        } catch (e) {
            console.info(e.message);
            }

    });--%>
    //function updateBG(id){
    //    console.info("CAAAled", id);
    //    $(id).addClass("active");
    //}
    function cleareClass() {
        console.info("Called");
        $("#productViewerUL").find("a").removeClass();
    }
</script>
