/*
' Copyright (c) 2017  MohammadAbuHmead
'  All rights reserved.
'
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
'
*/

using System;
using DotNetNuke.Security;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Security.Permissions;
using System.Web.UI;
namespace Mohammad.Modules.ProductsViewer
{
    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The View class displays the content
    ///
    /// Typically your view control would be used to display content or functionality in your module.
    ///
    /// View may be the only control you have in your project depending on the complexity of your module
    ///
    /// Because the control inherits from ProductsViewerModuleBase you have access to any custom properties
    /// defined there, as well as properties from DNN such as PortalId, ModuleId, TabId, UserId and many more.
    ///
    /// </summary>
    /// -----------------------------------------------------------------------------
    public partial class View : ProductsViewerModuleBase, IActionable
    {
        public static Control currentControl;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (ModulePermissionController.CanEditModuleContent(ModuleConfiguration))
                {
                    Panel_cmd.Visible = true;
                    Panel_cmd.Enabled = true;
                    btn_addRemoveCategories.Text = LocalizeString("AddRemoveCategories");
                    btn_addRemoveProducts.Text = LocalizeString("AddRemoveProduct.Text");
                    btn_preview.Text = LocalizeString("PreviewProducts.Text");
                }
                if (Session["currentControl" + ModuleId] != null)
                {
                    PortalModuleBase portalModuleBase = (PortalModuleBase)((TemplateControl)this).LoadControl((string)Session["currentControl" + ModuleId]);
                    if (portalModuleBase == null)
                        return;
                    PlaceHolder1.Controls.Clear();
                    portalModuleBase.ModuleConfiguration=(ModuleConfiguration);
                    PlaceHolder1.Controls.Add((Control)portalModuleBase);
                }
                else
                {
                    PortalModuleBase portalModuleBase = (PortalModuleBase)((TemplateControl)this).LoadControl("Controls/ProductsPreview.ascx");
                    if (portalModuleBase == null)
                        return;
                    PlaceHolder1.Controls.Clear();
                    portalModuleBase.ModuleConfiguration=(ModuleConfiguration);
                    PlaceHolder1.Controls.Add((Control)portalModuleBase);
                }
            }
            catch (Exception ex)
            {
                DotNetNuke.Services.Exceptions.Exceptions.ProcessModuleLoadException((PortalModuleBase)this, ex);
            }
        }
        public ModuleActionCollection ModuleActions
        {
            get
            {
                ModuleActionCollection actionCollection = new ModuleActionCollection();
                actionCollection.Add(GetNextActionID(), DotNetNuke.Services.Localization.Localization.GetString("EditModule", LocalResourceFile), "", "", "", EditUrl(), false, (SecurityAccessLevel)1, true, false);
                return actionCollection;
            }
        }



        protected void btn_addRemoveCategories_Click(object sender, EventArgs e)
        {
            Session["currentControl" + ModuleId] = (object)"Controls/CategoriesManagement.ascx";
            Response.Redirect(Request.Url.AbsoluteUri);
        }

        protected void btn_addRemoveProducts_Click(object sender, EventArgs e)
        {
            Session["currentControl" + ModuleId] = (object)"Controls/ProductsManagement.ascx";
            Response.Redirect(Request.Url.AbsoluteUri);
        }

        protected void btn_previewProducts_Click(object sender, EventArgs e)
        {
            Session["currentControl" + ModuleId] = (object)null;
            Response.Redirect(Request.Url.AbsoluteUri);
        }
    }
}
