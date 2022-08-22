
using DotNetNuke.Modules.Blog.Common;
using DotNetNuke.Modules.Blog.Controls;
using DotNetNuke.Security.Permissions;
using DotNetNuke.Services.Localization;
using DotNetNuke.Web.UI.WebControls;
using Microsoft.ApplicationBlocks.Data;
using Mohammad.Modules.ProductsViewer.Data;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Mohammad.Modules.ProductsViewer.Controls
{
    public partial class CategoriesManagement : ProductsViewerModuleBase
    {
        

        protected void Page_Load(object sender, EventArgs e)
        {
            LocalResourceFile=("~/DesktopModules/ProductsViewer/App_LocalResources/CategoriesManagement.ascx.resx");

            if (!Page.IsPostBack)
            {
                Session["showSaved" + ModuleId] = false;
            }
            if (ModulePermissionController.CanEditModuleContent(ModuleConfiguration))
            {
                SqlDataSource_catList.SelectParameters["local"].DefaultValue = Thread.CurrentThread.CurrentCulture.Name;
                SqlDataSource_catList.SelectParameters["ModuleID"].DefaultValue =""+ ModuleId;
                SqlDataSource_catList.DataBind();
                GridView_CatList.DataBind();
                if (!(bool)Session["showSaved" + ModuleId])
                    return;
                DatabaseHelper.ShowMessage((Control)this, ((object)this).GetType(), LocalizeString("Saved.Text"), DatabaseHelper.MessageType.Success);
                Session["showSaved" + ModuleId] = false;
            }
            else
                ClearValriables();
        }

        protected void lnkbtn_Cancel_Click(object sender, EventArgs e)
        {
            ClearValriables();
        }

        private void ClearValriables()
        {
            Session["currentControl" + ModuleId] = (object)null;
            Session["CatgoryID" + ModuleId] = (object)-1;
            Session.Clear();
            Response.Redirect(Request.Url.AbsoluteUri);
        }

        private void ResetVariables()
        {
            Session["CatgoryID" + ModuleId] = (object)null;
            txt_Order.Text = string.Empty;
            txtTitle.DefaultText=(string.Empty);
            txtTitle.LocalizedTexts=(new LocalizedText());
            description.DefaultText=(string.Empty);
            description.LocalizedTexts=(new LocalizedText());
            txtTitle.InitialBind();
            description.InitialBind();
            imagePicker1.FileID=(-1);
            ((Control)imagePicker1).DataBind();
            bgImageUploader.FileID = (-1);
            ((Control)bgImageUploader).DataBind();
        }

        protected void lnkbtn_Save_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtTitle.DefaultText))
                {
                    SqlParameter[] sqlParameterArray1 = new SqlParameter[8];
                    SqlParameter[] sqlParameterArray2 = sqlParameterArray1;
                    int index1 = 0;
                    SqlParameter sqlParameter1 = new SqlParameter("@id", SqlDbType.Int);
                    sqlParameter1.Value = Session["CatgoryID" + ModuleId];
                    SqlParameter sqlParameter2 = sqlParameter1;
                    sqlParameterArray2[index1] = sqlParameter2;
                    sqlParameterArray1[1] = new SqlParameter("@name", (object)txtTitle.DefaultText);
                    sqlParameterArray1[2] = new SqlParameter("@description", (object)description.DefaultText);
                    sqlParameterArray1[3] = new SqlParameter("@imageFileID", (object)imagePicker1.FileID);
                    

                    SqlParameter[] sqlParameterArray3 = sqlParameterArray1;
                    int index2 = 4;
                    SqlParameter sqlParameter3 = new SqlParameter("@SortOrder", SqlDbType.Int);
                    sqlParameter3.Value = (object)Convert.ToInt32(string.IsNullOrEmpty(txt_Order.Text) ? string.Concat((object)(Convert.ToInt32(SqlHelper.ExecuteScalar(DatabaseHelper.SiteConnStr, CommandType.Text, "select isnull(max(SortOrder),-1) from ProductsViewer_Category where isDeleted = 0")) + 1)) : txt_Order.Text);
                    SqlParameter sqlParameter4 = sqlParameter3;
                    sqlParameterArray3[index2] = sqlParameter4;
                    sqlParameterArray1[5] = new SqlParameter("@local", (object)txtTitle.DefaultLanguage);
                    sqlParameterArray1[6] = new SqlParameter("@bgImageFileID", (object)bgImageUploader.FileID);
                    sqlParameterArray1[7] = new SqlParameter("@ModuleID", ModuleId);
                    object obj = SqlHelper.ExecuteScalar(DatabaseHelper.SiteConnStr, "ProductsViewer_insertOrUpdateCategory", (object[])sqlParameterArray1);
                    if (obj != null)
                    {
                        Session["CatgoryID" + ModuleId] = obj;
                        if (((LocaleCollection)txtTitle.SupportedLocales).AllKeys.Length > 1)
                        {
                            for (int index3 = 0; index3 < ((LocaleCollection)txtTitle.SupportedLocales).AllKeys.Length; ++index3)
                            {
                                if (((LocaleCollection)txtTitle.SupportedLocales).AllKeys[index3] != txtTitle.DefaultLanguage)
                                {
                                    SqlParameter[] sqlParameterArray4 = new SqlParameter[4];
                                    SqlParameter[] sqlParameterArray5 = sqlParameterArray4;
                                    int index4 = 0;
                                    SqlParameter sqlParameter5 = new SqlParameter("@catID", SqlDbType.Int);
                                    sqlParameter5.Value = Session["CatgoryID" + ModuleId];
                                    SqlParameter sqlParameter6 = sqlParameter5;
                                    sqlParameterArray5[index4] = sqlParameter6;
                                    sqlParameterArray4[1] = new SqlParameter("@name", (object)txtTitle.GetLocalizedTexts().GetDictionary()[((LocaleCollection)txtTitle.SupportedLocales).AllKeys[index3]]);
                                    sqlParameterArray4[2] = new SqlParameter("@description", (object)description.GetLocalizedTexts().GetDictionary()[((LocaleCollection)txtTitle.SupportedLocales).AllKeys[index3]]);
                                    sqlParameterArray4[3] = new SqlParameter("@local", (object)((LocaleCollection)txtTitle.SupportedLocales).AllKeys[index3]);
                                    SqlHelper.ExecuteScalar(DatabaseHelper.SiteConnStr, "ProductsViewer_insertOrUpdateCategoryLocal", (object[])sqlParameterArray4);
                                }
                            }
                        }
                    }
                    GridView_CatList.DataBind();
                    DatabaseHelper.ShowMessage((Control)this, ((object)this).GetType(), LocalizeString("Saved.Text"), DatabaseHelper.MessageType.Success);
                    ResetVariables();
                }
                else
                    DatabaseHelper.ShowMessage((Control)this, ((object)this).GetType(), LocalizeString("FillData.Text"), DatabaseHelper.MessageType.Warning);
            }
            catch (Exception ex)
            {
                DatabaseHelper.ShowMessage((Control)this, ((object)this).GetType(), string.Concat((object)ex), DatabaseHelper.MessageType.Error);
            }
        }

        protected void GridView_CatList_PreRender(object sender, EventArgs e)
        {
            localizeGridView();
        }

        private void localizeGridView()
        {
            if (!(GridView_CatList.Columns[0].HeaderText != LocalizeString("id.HeaderText")))
                return;
            for (int index = 0; index < GridView_CatList.Columns.Count; ++index)
                GridView_CatList.Columns[index].HeaderText = LocalizeString(GridView_CatList.Columns[index].HeaderText + ".HeaderText");
        }

        protected void GridView_CatList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Session["CatgoryID"+ModuleId] = e.CommandArgument;
            switch (e.CommandName)
            {
                case "InsertProduct":
                    Session["currentControl" + ModuleId] = (object)"Controls/ProductsManagement.ascx";
                    Response.Redirect(Request.Url.AbsoluteUri);
                    break;
                case "EditRow":
                    string str = string.Format("SELECT name, description, imageFileID,bgImageFileID, SortOrder FROM ProductsViewer_Category where id = {0}", e.CommandArgument);
                    DataSet dataSet1 = SqlHelper.ExecuteDataset(DatabaseHelper.SiteConnStr, CommandType.Text, str);
                    if (dataSet1.Tables[0].Rows.Count > 0)
                    {
                        txtTitle.DefaultText=(dataSet1.Tables[0].Rows[0]["name"].ToString());
                        description.DefaultText=(dataSet1.Tables[0].Rows[0]["description"].ToString());
                        txt_Order.Text = dataSet1.Tables[0].Rows[0]["SortOrder"].ToString();
                        imagePicker1.FileID=((int)dataSet1.Tables[0].Rows[0]["imageFileID"]);
                        bgImageUploader.FileID = ((int)dataSet1.Tables[0].Rows[0]["bgImageFileID"]);
                        txtTitle.InitialBind();
                        description.InitialBind();
                    }
                    if (((LocaleCollection)txtTitle.SupportedLocales).AllKeys.Length <= 1)
                        break;
                    DataSet dataSet2 = SqlHelper.ExecuteDataset(DatabaseHelper.SiteConnStr, CommandType.Text, string.Format("SELECT name, description, local FROM ProductsViewer_CategoryLocal where catID = {0}", e.CommandArgument));
                    if (dataSet2.Tables[0].Rows.Count <= 0)
                        break;
                    LocalizedText localizedText1 = new LocalizedText();
                    LocalizedText localizedText2 = new LocalizedText();
                    foreach (DataRow row in (InternalDataCollectionBase)dataSet2.Tables[0].Rows)
                    {
                        localizedText1.Add(row["local"].ToString(), row["name"].ToString());
                        localizedText2.Add(row["local"].ToString(), row["description"].ToString());
                    }
                txtTitle.LocalizedTexts=(localizedText1);
                    description.LocalizedTexts=(localizedText2);
                    txtTitle.InitialBind();
                    description.InitialBind();
                    break;
                case "DeleteRow":
                    if (SqlHelper.ExecuteNonQuery(DatabaseHelper.SiteConnStr, CommandType.Text, string.Format("UPDATE ProductsViewer_Category SET isDeleted=1 where id = {0}", e.CommandArgument)) > 0)
                        DatabaseHelper.ShowMessage((Control)this, ((object)this).GetType(), LocalizeString("Deleted.Text"), DatabaseHelper.MessageType.Success);
                    GridView_CatList.DataBind();
                    ResetVariables();
                    break;
            }
        }
    }
}
