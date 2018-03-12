
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Pages_TLD_TLDHeader : System.Web.UI.Page
{ 
    //*** OnInit
    //protected override void OnInit(EventArgs e)
    //{
    //RadMenuItem currentItem = RadMenu1.FindItemByUrl(Request.Url.PathAndQuery);
    //if (currentItem != null)
    //{
    //    currentItem.HighlightPath();

    //    switch (currentItem.Text)
    //    {
    //        case "Home":
    //            Control userControlHome = Page.LoadControl("Pages/Home.ascx");
    //           // Content.Controls.Add(userControlHome);
    //            break;
    //        case "PurchaseOrder":
    //            Control userControlChairs = Page.LoadControl("UserControls/Chairs.ascx");
    //         //   Content.Controls.Add(userControlChairs);
    //            break; 
    //    }
    //}
    //else
    //{
    //    //Control userControlHome = Page.LoadControl("PurchaseOrder.aspx");
    // //   Content.Controls.Add(userControlHome);
    //}


    //base.OnInit(e);
    //}
    //***


    //*** Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(Session["user_name"] as string))
        {
            Redirect("~/Login/Login.aspx");
        }
        //    RadGridBoxes.MasterTableView.GetColumn("EditCommandColumn").Display = false;
        //   RadGridBoxes.MasterTableView.GetColumn("edit").Display = true;

        try
        {
            using (SqlConnection con = new SqlConnection(Controller.connection))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    con.Open();

                    DataTable dtSec = Controller.Security(cmd, con, Path.GetFileName(Request.PhysicalPath), Session["user_name"].ToString());

                    Security(dtSec);

                    if (!IsPostBack)
                    {
                        Translate(cmd, con);

                        langRCB.DataSource = Controller.SelectFrom(cmd, con, "SET_Language", new ArrayList(), new Dictionary<string, string>(), new ArrayList(), true, false, "");
                        langRCB.DataBind();



                        if (Session["language_id"].ToString() != "")
                        {
                            langRCB.SelectedValue = Session["language_id"].ToString();
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Controller.SaveErrors(Path.GetFileName(Request.PhysicalPath), ex.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name);
        }
    }
    //*** 

    //*** RadGridBoxes_NeedDataSource1
    protected void RadGridBoxes_NeedDataSource1(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            //RadGridBoxes.MasterTableView.IsItemInserted = true;

            using (SqlConnection con = new SqlConnection(Controller.connection))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    con.Open();
                    Dictionary<String, String> conditions = new Dictionary<string, string>();
                    conditions.Add("active", true.ToString()); 

                    Controller.FillRadGrid(cmd, con, sender, "CNRS_TLDHeader", new ArrayList(), conditions, true, "tldh_name");
                }
            }
        }
        catch (Exception ex)
        {
            Controller.SaveErrors(Path.GetFileName(Request.PhysicalPath), ex.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name);
        }
    }
    //***

    //*** Security
    protected void Security(DataTable dt)
    {
        if (dt.Rows.Count > 0)
        {
            if (Controller.Can(dt, "can_add") == true)
            {
                RadGridBoxes.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.Top;
                RadGridBoxes.Rebind();
            }
            else
            {
                RadGridBoxes.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.None;
                RadGridBoxes.Rebind();
            }

            RadGridBoxes.MasterTableView.GetColumn("delete").Display = Controller.Can(dt, "can_delete");

            // RadGridBoxes.MasterTableView.GetColumn("edit").Display = Controller.Can(dt, "can_edit");

            //  RadGrid.MasterTableView.GetColumn("export").Display = Controller.Can(dt, "can_export");
        }
    }
    //***

    //*** Translate
    protected void Translate(SqlCommand cmd, SqlConnection con)
    {
        //ArrayList componentID = new ArrayList();
        //componentID.Add("tldh_name");
        //componentID.Add("tldh_id");
        //Controller.Translate(cmd, con, RadGridBoxes, Session["language_id"].ToString(), Path.GetFileName(Request.PhysicalPath), componentID);
    }
    //***

    //*** RadGridBoxes_ItemCommand
    protected void RadGridBoxes_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "PerformInsert")   // add
            {
                GridDataItem item = e.Item as GridDataItem;
                 
                string tldh_name = Controller.Clean(((TextBox)item.FindControl("tldh_nameTXT")).Text); 

                using (SqlConnection con = new SqlConnection(Controller.connection))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        Dictionary<string, string> fields = new Dictionary<string, string>();
                        fields.Add("tldh_name", tldh_name);  
                        fields.Add("createdBy", Session["user_name"].ToString());

                        con.Open();
                        Controller.InsertInto(cmd, con, "CNRS_TLDHeader", fields, false);
                    }
                } 
            }


            if (e.CommandName == "Update")   //edit
            {
                if (e.Item is GridDataItem)
                {
                    GridDataItem item = e.Item as GridDataItem;

                    int tldh_id = (int)item.GetDataKeyValue("tldh_id");
                    string tldh_name = Controller.Clean(((TextBox)item.FindControl("tldh_nameTXT")).Text); 

                    using (SqlConnection con = new SqlConnection(Controller.connection))
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            Dictionary<string, string> fields = new Dictionary<string, string>();
                            fields.Add("tldh_name", tldh_name); 
                            fields.Add("modifedBy", Session["user_name"].ToString());
                            fields.Add("modifiedOn", DateTime.Now.ToString()); 

                            Dictionary<string, string> conditions = new Dictionary<string, string>();
                            conditions.Add("tldh_id", tldh_id.ToString());

                            con.Open();
                            Controller.Update(cmd, con, "CNRS_TLDHeader", fields, conditions);
                            AlertJS();
                        }
                    }
                }
            }


            else if (e.CommandName == "Delete")  // inactivate
            {
                if (e.Item is GridDataItem)
                {
                    GridDataItem item = e.Item as GridDataItem;

                    int tldh_id = (int)item.GetDataKeyValue("tldh_id");
                    using (SqlConnection con = new SqlConnection(Controller.connection))
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            con.Open();
                            Controller.Inactivate(cmd, con, "CNRS_TLDHeader", "tldh_id", tldh_id.ToString(), Session["user_name"].ToString());
                            AlertJS();
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Controller.SaveErrors(Path.GetFileName(Request.PhysicalPath), ex.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name);
        }
    }
    //***  

    //*** Redirect
    protected void Redirect(string path)
    {
        Response.Redirect(path, false);
        Context.ApplicationInstance.CompleteRequest();
    }
    //***


    //***   Menu    
    protected void logoutBTN_Click(object sender, System.EventArgs e)
    {
        Session["user_name"] = "";
        Session["language_id"] = "";
        Redirect("../Login/Login.aspx");
    }

    protected void navgPoBTN_Click(object sender, ImageClickEventArgs e)
    {
        Redirect("PurchaseOrder.aspx");
    }
    //***

    //*** Details_Click
    protected void Details_Click(object sender, EventArgs e)
    {
        LinkButton LBTN = (LinkButton)sender;
        string tldh_id = LBTN.CommandArgument;
        var encrypt = new eXSecurity.Encryption();
        Redirect("TLDDetails.aspx"); 
        Session["tldh_id"] = encrypt.EncryptData(tldh_id);
    }
    //****

    protected void langRCB_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        Session["language_id"] = langRCB.SelectedValue;
        try
        {
            using (SqlConnection con = new SqlConnection(Controller.connection))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    Translate(cmd, con);
                    RadGridBoxes.Rebind();
                }
            }
        }
        catch (Exception ex)
        {
            Controller.SaveErrors(Path.GetFileName(Request.PhysicalPath), ex.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name);
        }
    }


    //*** Alert 
    protected void Alert(string msg, string title, string imgUrl)
    {
        string scriptstring = "radalert('<b>" + msg + "</b>!', 280, 198 ";
        if (title != "")
        {
            scriptstring += ", " + title;
            if (imgUrl != "")
                scriptstring += "callbackFn, " + imgUrl;
        }
        scriptstring += ");";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "radalert", scriptstring, true);
    }
    //*** 

    //*** AlertJS
    protected void AlertJS()
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert();", true);
    }
    //***  
}