using DotNetNuke.Modules.Blog.Common;
using DotNetNuke.Security.Permissions;
using Microsoft.ApplicationBlocks.Data;
using Mohammad.Modules.ProductsViewer.Data;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace Mohammad.Modules.ProductsViewer.Controls
{
    public partial class ProductsManagement : ProductsViewerModuleBase
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            LocalResourceFile="~/DesktopModules/ProductsViewer/App_LocalResources/ProductsManagement.ascx.resx";
            if (ModulePermissionController.CanEditModuleContent(ModuleConfiguration))
            {
                SqlDataSource_DDCatsList.SelectParameters["local"].DefaultValue = Thread.CurrentThread.CurrentCulture.Name;
                SqlDataSource_DDCatsList.SelectParameters["ModuleID"].DefaultValue = "" + ModuleId;
                SqlDataSource_DDCatsList.DataBind();
                ddl_CatsList.DataBind();
                if (ddl_CatsList.Items.Count > 0)
                {
                    if (Session["CatgoryID" + ModuleId] != null)
                    {
                        ddl_CatsList.SelectedValue=Session["CatgoryID" + ModuleId].ToString();
                        DnnCombo_ProdCat.SelectedValue=Session["CatgoryID" + ModuleId].ToString();
                    }
                    else
                    {
                        ddl_CatsList.SelectedIndex=0;
                        DnnCombo_ProdCat.SelectedIndex=0;
                        Session["CatgoryID" + ModuleId] = ddl_CatsList.SelectedValue;
                    }
                    BindProductsList();
                }
                else
                    DatabaseHelper.ShowMessage(this, (this).GetType(), LocalizeString("PlzAddCats.Text"), DatabaseHelper.MessageType.Error);
            }
            else
                ClearValriables();
        }

        private void BindProductsList()
        {
            SqlDataSource_ProductsList.SelectParameters["local"].DefaultValue = Thread.CurrentThread.CurrentCulture.Name;
            SqlDataSource_ProductsList.SelectParameters["catID"].DefaultValue = Session["CatgoryID" + ModuleId].ToString();
            SqlDataSource_ProductsList.DataBind();
            GridView_ProductsList.DataBind();
        }

        protected void lnkbtn_Cancel_Click(object sender, EventArgs e)
        {
            ClearValriables();
        }

        private void ClearValriables()
        {
            Session["currentControl" + ModuleId] = null;
            Session["CatgoryID" + ModuleId] = -1;
            Session["ProductID" + ModuleId] = null;
            Session.Clear();
            Response.Redirect(Request.Url.AbsoluteUri);
        }

        private void ResetVariables()
        {
            Session["ProductID" + ModuleId] = null;
            txt_Order.Text = string.Empty;
            txtTitle.DefaultText=string.Empty;
            txtTitle.LocalizedTexts=new LocalizedText();
            description.DefaultText=string.Empty;
            description.LocalizedTexts=new LocalizedText();
            txtTitle.InitialBind();
            description.InitialBind();
            imagePicker1.FileID=-1;
            imagePicker1.DataBind();
        }

        protected void lnkbtn_Save_Click(object sender, EventArgs e)
        {
            try
            {
                if (DnnCombo_ProdCat.SelectedIndex > -1 && !string.IsNullOrEmpty(txtTitle.DefaultLanguage) && !string.IsNullOrEmpty(description.DefaultText))
                {
                    SqlParameter[] sqlParameterArray1 = new SqlParameter[7];
                    SqlParameter[] sqlParameterArray2 = sqlParameterArray1;
                    int index1 = 0;
                    SqlParameter sqlParameter1 = new SqlParameter("@catID", SqlDbType.Int);
                    sqlParameter1.Value = Convert.ToInt32(DnnCombo_ProdCat.SelectedItem.Value);
                    SqlParameter sqlParameter2 = sqlParameter1;
                    sqlParameterArray2[index1] = sqlParameter2;
                    SqlParameter[] sqlParameterArray3 = sqlParameterArray1;
                    int index2 = 1;
                    SqlParameter sqlParameter3 = new SqlParameter("@id", SqlDbType.Int);
                    sqlParameter3.Value = Convert.ToInt32(Session["ProductID" + ModuleId]);
                    SqlParameter sqlParameter4 = sqlParameter3;
                    sqlParameterArray3[index2] = sqlParameter4;
                    SqlParameter[] sqlParameterArray4 = sqlParameterArray1;
                    int index3 = 2;
                    SqlParameter sqlParameter5 = new SqlParameter("@SortOrder", SqlDbType.Int);
                    sqlParameter5.Value = Convert.ToInt32(string.IsNullOrEmpty(txt_Order.Text) ? string.Concat((Convert.ToInt32(SqlHelper.ExecuteScalar(DatabaseHelper.SiteConnStr, CommandType.Text, string.Format("select isnull(max(SortOrder),-1) from ProductsViewer_ProductsList where isDeleted=0 and catID ={0}", (DnnCombo_ProdCat.SelectedItem.Value)))) + 1)) : txt_Order.Text);
                    SqlParameter sqlParameter6 = sqlParameter5;
                    sqlParameterArray4[index3] = sqlParameter6;
                    sqlParameterArray1[3] = new SqlParameter("@imageFileID", Convert.ToInt32(imagePicker1.FileID));
                    sqlParameterArray1[4] = new SqlParameter("@name", txtTitle.DefaultText);
                    sqlParameterArray1[5] = new SqlParameter("@description", description.DefaultText);
                    sqlParameterArray1[6] = new SqlParameter("@local", txtTitle.DefaultLanguage);
                    object obj = SqlHelper.ExecuteScalar(DatabaseHelper.SiteConnStr, "ProductsViewer_insertOrUpdateCategoryProduct", (object[])sqlParameterArray1);
                    if (obj != null)
                    {
                        Session["ProductID" + ModuleId] = obj;
                        if (txtTitle.SupportedLocales.AllKeys.Length > 1)
                        {
                            for (int index4 = 0; index4 < (txtTitle.SupportedLocales).AllKeys.Length; ++index4)
                            {
                                if (txtTitle.SupportedLocales.AllKeys[index4] != txtTitle.DefaultLanguage)
                                {
                                    SqlParameter[] sqlParameterArray5 = new SqlParameter[4];
                                    SqlParameter[] sqlParameterArray6 = sqlParameterArray5;
                                    int index5 = 0;
                                    SqlParameter sqlParameter7 = new SqlParameter("@productID", SqlDbType.Int);
                                    sqlParameter7.Value = Session["ProductID" + ModuleId];
                                    SqlParameter sqlParameter8 = sqlParameter7;
                                    sqlParameterArray6[index5] = sqlParameter8;
                                    sqlParameterArray5[1] = new SqlParameter("@name", txtTitle.GetLocalizedTexts().GetDictionary()[(txtTitle.SupportedLocales).AllKeys[index4]]);
                                    sqlParameterArray5[2] = new SqlParameter("@description", description.GetLocalizedTexts().GetDictionary()[(txtTitle.SupportedLocales).AllKeys[index4]]);
                                    sqlParameterArray5[3] = new SqlParameter("@local", (txtTitle.SupportedLocales).AllKeys[index4]);
                                    SqlHelper.ExecuteScalar(DatabaseHelper.SiteConnStr, "ProductsViewer_insertOrUpdateCategoryProductLocal", (object[])sqlParameterArray5);
                                }
                            }
                        }
                    }
                    GridView_ProductsList.DataBind();
                    DatabaseHelper.ShowMessage(this, (this).GetType(), LocalizeString("Saved.Text"), DatabaseHelper.MessageType.Success);
                    ResetVariables();
                }
                else
                    DatabaseHelper.ShowMessage(this, (this).GetType(), LocalizeString("FillData.Text"), DatabaseHelper.MessageType.Warning);
            }
            catch (Exception ex)
            {
                DatabaseHelper.ShowMessage(this, (this).GetType(), string.Concat(ex), DatabaseHelper.MessageType.Error);
            }
        }

        protected void GridView_ProductsList_PreRender(object sender, EventArgs e)
        {
            localizeGridView();
        }

        private void localizeGridView()
        {
            if (!(GridView_ProductsList.Columns[0].HeaderText != LocalizeString("id.HeaderText")))
                return;
            for (int index = 0; index < GridView_ProductsList.Columns.Count; ++index)
                GridView_ProductsList.Columns[index].HeaderText = LocalizeString(GridView_ProductsList.Columns[index].HeaderText + ".HeaderText");
        }

        protected void GridView_ProductsList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Session["ProductID" + ModuleId] = e.CommandArgument;
            switch (e.CommandName)
            {
                case "EditProd":
                    string str = string.Format("SELECT name, description, imageFileID, SortOrder, catID FROM ProductsViewer_ProductsList where id = {0}", Session["ProductID" + ModuleId].ToString());
                    DataSet dataSet1 = SqlHelper.ExecuteDataset(DatabaseHelper.SiteConnStr, CommandType.Text, str);
                    if (dataSet1.Tables[0].Rows.Count > 0)
                    {
                        txtTitle.DefaultText=(dataSet1.Tables[0].Rows[0]["name"].ToString());
                        description.DefaultText=(dataSet1.Tables[0].Rows[0]["description"].ToString());
                        txt_Order.Text = dataSet1.Tables[0].Rows[0]["SortOrder"].ToString();
                        imagePicker1.FileID=((int)dataSet1.Tables[0].Rows[0]["imageFileID"]);
                        DnnCombo_ProdCat.SelectedValue=dataSet1.Tables[0].Rows[0]["catID"].ToString();
                        txtTitle.InitialBind();
                        description.InitialBind();
                    }
                    if ((txtTitle.SupportedLocales).AllKeys.Length <= 1)
                        break;
                    DataSet dataSet2 = SqlHelper.ExecuteDataset(DatabaseHelper.SiteConnStr, CommandType.Text, string.Format("SELECT name, description, local FROM ProductsViewer_ProductsListLocal where productID = {0}", Session["ProductID" + ModuleId]));
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
                    if (SqlHelper.ExecuteNonQuery(DatabaseHelper.SiteConnStr, CommandType.Text, string.Format("Update ProductsViewer_ProductsList set isDeleted = 1   where id = {0}", e.CommandArgument)) > 0)
                        DatabaseHelper.ShowMessage(this, (this).GetType(), LocalizeString("Deleted.Text"), DatabaseHelper.MessageType.Success);
                    GridView_ProductsList.DataBind();
                    ResetVariables();
                    break;
            }
        }

        protected void ddl_CatsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["CatgoryID" + ModuleId] = ddl_CatsList.SelectedValue;
            DnnCombo_ProdCat.SelectedValue=ddl_CatsList.SelectedValue;
            BindProductsList();
            ResetVariables();
        }
    }
}