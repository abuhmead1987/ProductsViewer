using DotNetNuke.ComponentModel;
using DotNetNuke.Services.FileSystem;
using System;
using System.Collections.Specialized;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Mohammad.Modules.ProductsViewer.Controls
{
    public partial class ProductsPreview : ProductsViewerModuleBase
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            LocalResourceFile=("~/DesktopModules/ProductsViewer/App_LocalResources/ProductsPreview.ascx.resx");
            SqlDataSource_DDCatsList.SelectParameters["local"].DefaultValue = Thread.CurrentThread.CurrentCulture.Name;
            SqlDataSource_DDCatsList.SelectParameters["ModuleID"].DefaultValue = "" + ModuleId;
            SqlDataSource_DDCatsList.DataBind();
            Repeater_CatsList.DataBind();
            if (Repeater_CatsList.Items.Count <= 0)
                return;
            int index = ((UserControl)this).Session["ControlID" + ModuleId] == null ? 0 : Convert.ToInt32(((UserControl)this).Session["ControlID" + ModuleId].ToString());
            SqlDataSource_ProductsList.SelectParameters["local"].DefaultValue = Thread.CurrentThread.CurrentCulture.Name;
            SqlDataSource_ProductsList.SelectParameters["catID"].DefaultValue = ((LinkButton)Repeater_CatsList.Items[index].FindControl("lnkbtn_CatsList")).CommandArgument;
            SqlDataSource_ProductsList.DataBind();
            Repeater_Products.DataBind();
          //  updateCateogryInfo(SqlDataSource_ProductsList.SelectParameters["catID"].DefaultValue);
            ((WebControl)Repeater_CatsList.Items[index].FindControl("lnkbtn_CatsList")).CssClass = "active";
            string str = ((UserControl)this).Request.QueryString.Get("products");
            if (string.IsNullOrEmpty(str))
                return;
            Repeater_Products.Visible = false;
            SqlDataSource_ProdDesc.SelectParameters["local"].DefaultValue = Thread.CurrentThread.CurrentCulture.Name;
            SqlDataSource_ProdDesc.SelectParameters["id"].DefaultValue = str;
            SqlDataSource_ProdDesc.DataBind();
            Repeater_ProdDesc.DataBind();
            Repeater_ProdDesc.Visible = true;
        }

        public string decodeHTML(string str)
        {
            return ((UserControl)this).Server.HtmlDecode(str);
        }

        protected void ListBox_Cats_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        public string getFilePath(string fileID)
        {
            FileInfo file = (FileInfo)ComponentBase<IFileManager, FileManager>.Instance.GetFile(Convert.ToInt32(fileID));
            if (file != null)
                return ComponentBase<IFileManager, FileManager>.Instance.GetUrl((IFileInfo)file);
            return "/DesktopModules/ProductsViewer/Images/no-thumb.png";
        }

        protected void lnkbtn_ProductsGallery_Command(object sender, CommandEventArgs e)
        {
            string absoluteUri = ((UserControl)this).Request.Url.AbsoluteUri;
            NameValueCollection queryString = HttpUtility.ParseQueryString(((UserControl)this).Request.QueryString.ToString());
            if (string.IsNullOrEmpty(queryString.Get("products")))
                queryString.Add("products", string.Concat(e.CommandArgument));
            else
                queryString.Set("products", string.Concat(e.CommandArgument));
            ((UserControl)this).Response.Redirect(((UserControl)this).Request.Url.AbsolutePath + ("?" + queryString.ToString()));
        }

        protected void lnkbtn_CatsList_Command(object sender, CommandEventArgs e)
        {
            SqlDataSource_ProductsList.SelectParameters["local"].DefaultValue = Thread.CurrentThread.CurrentCulture.Name;
            SqlDataSource_ProductsList.SelectParameters["catID"].DefaultValue = e.CommandArgument.ToString();
            SqlDataSource_ProductsList.DataBind();
            Repeater_Products.DataBind();
            foreach (Control control in Repeater_CatsList.Items)
                ((WebControl)control.FindControl("lnkbtn_CatsList")).CssClass = "";
            int int32 = Convert.ToInt32(((Control)sender).ClientID.Substring(((Control)sender).ClientID.LastIndexOf('_') + 1));
            ((UserControl)this).Session["ControlID" + ModuleId] = (object)int32;
            ((WebControl)Repeater_CatsList.Items[int32].FindControl("lnkbtn_CatsList")).CssClass = "active";
           // updateCateogryInfo(e.CommandArgument.ToString());
            
            if (Repeater_Products.Visible)
                return;
            Repeater_Products.Visible = true;
            Repeater_ProdDesc.Visible = false;

            
        }
        //private void updateCateogryInfo(String catID)
        //{
        //    SqlParameter[] sqlParameterArray1 = new SqlParameter[2];
        //    sqlParameterArray1[0] = new SqlParameter("@local", Thread.CurrentThread.CurrentCulture.Name);
        //    sqlParameterArray1[1] = new SqlParameter("@categoryID", Convert.ToInt32(catID));
        //    DataSet ds = SqlHelper.ExecuteDataset(DatabaseHelper.SiteConnStr, "ProductsViewer_getCategory", sqlParameterArray1);
        //    if (ds.Tables[0].Rows.Count > 0)
        //    {
        //        categoryDesc.Text = ds.Tables[0].Rows[0]["description"].ToString();
        //        categoryLogo.ImageUrl = getFilePath(ds.Tables[0].Rows[0]["imageFileID"].ToString());
        //        logoBG.Style.Add("background-image", getFilePath(ds.Tables[0].Rows[0]["bgImageFileID"].ToString()));
        //    }
        //    ds.Clear();
        //}
    }
}
