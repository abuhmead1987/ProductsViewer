<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="View.ascx.cs" Inherits="Mohammad.Modules.ProductsViewer.View" %>
<link id="Link1" runat="server" rel="stylesheet" type="text/css" href="/DesktopModules/ProductsViewer/module.css" />
<%
        if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.StartsWith("ar"))
        {

        %>
           <link id="RTLStylesheet" runat="server" rel="stylesheet" type="text/css" href="/DesktopModules/ProductsViewer/rtlModule.css" />
<%
    }
    %>
<div class="container">
<asp:Panel ID="Panel_cmd" runat="server" Visible="false" Enabled="false">
    <asp:LinkButton ID="btn_addRemoveCategories" runat="server" CssClass="dnnPrimaryAction" OnClick="btn_addRemoveCategories_Click" ValidationGroup="cmd"></asp:LinkButton>
    <asp:LinkButton ID="btn_addRemoveProducts" runat="server" CssClass="dnnPrimaryAction" OnClick="btn_addRemoveProducts_Click" ValidationGroup="cmd"></asp:LinkButton>
    <asp:LinkButton ID="btn_preview" runat="server" CssClass="dnnPrimaryAction" OnClick="btn_previewProducts_Click" ValidationGroup="cmd"></asp:LinkButton>
</asp:Panel>
    </div>
<div class="productsHolder">
<asp:UpdatePanel ID="UpdatePanel_Actions" runat="server" UpdateMode="Always" ChildrenAsTriggers="true">
    <ContentTemplate>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel_Actions">
                    <ProgressTemplate>
                        <div class="modal">
                            <div class="center">
                                <img src="/images/loading.gif">
                            </div>
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
        <div class="row">
            <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
        </div>
        <div class="row">
            <div class="messagealert" id="alert_container">
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>

    </div>
<script type="text/javascript">
    function ShowMessage(message, messagetype) {
        var cssclass;
        switch (messagetype) {
            case 'Success':
                cssclass = 'alert-success'
                break;
            case 'Error':
                cssclass = 'alert-danger'
                break;
            case 'Warning':
                cssclass = 'alert-warning'
                break;
            default:
                cssclass = 'alert-info'
        }
        $('#alert_container').append('<div id="alert_div" style="margin: 0 0.5%; -webkit-box-shadow: 3px 4px 6px #999;" class="alert fade in ' + cssclass + '"><a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a><span>' + message + '</span></div>');
        $('html,body').animate({ scrollTop: $('#alert_container').offset().top });
        setTimeout(function () {
            $('#alert_container').hide();
        }, 5000);
    }
</script>