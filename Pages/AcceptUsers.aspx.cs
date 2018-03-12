
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


public partial class Pages_AcceptUsers : System.Web.UI.Page 
{
    //*** Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(Session["user_name"] as string))
        {
            Response.Redirect("~/Login/Login.aspx");
        }

        RadGridBoxes.MasterTableView.GetColumn("edit").Display = true;

        try
        {
            using (SqlConnection con = new SqlConnection(Controller.connection))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    con.Open();
                    //DataTable dtSec = Controller.Security(cmd, con, Path.GetFileName(Request.PhysicalPath), Session["user_name"]);

                    // Security(dtSec);

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

    //*** RadGridBoxes_NeedDataSource1
    protected void RadGridBoxes_NeedDataSource1(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(Controller.connection))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    con.Open();

                    Dictionary<string, string> conditions = new Dictionary<string, string>();
                    conditions.Add("role_id", "2"); //client
                    conditions.Add("user_removed", false.ToString()); 
                    Controller.FillRadGrid(cmd, con, sender, "View_User", new ArrayList(), conditions, true, "createdOn");
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

            RadGridBoxes.MasterTableView.GetColumn("edit").Display = Controller.Can(dt, "can_edit");

            //  RadGrid.MasterTableView.GetColumn("export").Display = Controller.Can(dt, "can_export");
        }
    }
    //***

    //*** Translate
    protected void Translate(SqlCommand cmd, SqlConnection con)
    {
        ArrayList componentID = new ArrayList(); 
        Controller.Translate(cmd, con, RadGridBoxes, Session["language_id"].ToString(), Path.GetFileName(Request.PhysicalPath), componentID);
    }
    //***



    //*** RadGridBoxes_ItemCreated
    protected void RadGridBoxes_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem && e.Item.IsDataBound)
            {
                if (e.Item is GridEditFormInsertItem || e.Item is GridDataInsertItem)   // insert item
                {
                    RadGridBoxes.MasterTableView.GetColumn("EditCommandColumn").Display = true;
                    RadGridBoxes.MasterTableView.GetColumn("edit").Display = false;
                    GridEditableItem item = e.Item as GridEditableItem;

                    RadDatePicker add_po_dateRDP = (RadDatePicker)item.FindControl("add_po_dateRDP");
                    add_po_dateRDP.SelectedDate = DateTime.Now;

                    RadComboBox add_item_idRCB = (RadComboBox)item.FindControl("add_item_idRCB");
                    ArrayList fields = new ArrayList();
                    fields.Add("item_id");
                    fields.Add("item_name");
                    Dictionary<string, string> conditions = new Dictionary<string, string>();
                    conditions.Add("active", "true");
                    using (SqlConnection con = new SqlConnection(Controller.connection))
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            con.Open();
                            add_item_idRCB.DataSource = Controller.SelectFrom(cmd, con, "CNRS_Item", fields, conditions, new ArrayList(), false, false, "");
                            add_item_idRCB.DataBind();
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

    //*** RadGridBoxes_ItemCommand
    protected void RadGridBoxes_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        { 
            if (e.CommandName == "Update")   //edit
            {
                if (e.Item is GridDataItem)
                {
                    GridDataItem item = e.Item as GridDataItem;

                    string user_contactPerson = ((Label)item.FindControl("user_contactPersonLBL")).Text; 
                    string user_name = item.GetDataKeyValue("user_name").ToString();
                    string user_reason = ((TextBox)item.FindControl("edit_user_reasonTXT")).Text; 
                    string user_email = ((Label)item.FindControl("user_email")).Text;
                    bool active = ((CheckBox)item.FindControl("edit_activeCB")).Checked;

                    using (SqlConnection con = new SqlConnection(Controller.connection))
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            Dictionary<string, string> fields = new Dictionary<string, string>();
                            fields.Add("user_reason", user_reason);
                            fields.Add("active", active.ToString());
                            fields.Add("modifedBy", Session["user_name"].ToString());
                            fields.Add("modifiedOn", DateTime.Now.ToString());

                            Dictionary<string, string> conditions = new Dictionary<string, string>();
                            conditions.Add("user_name", user_name);

                            con.Open();
                            Controller.Update(cmd, con, "SEC_User", fields, conditions);
                            //Controller.ShowMsg("Successful!", msgLBL, true, Color.Blue);

                            if (active == true)   // accepted
                            {
                                string body = "Hello " + user_contactPerson + "," + Environment.NewLine + Messages.acceptUser;
                                Controller.SendEmail(user_email, user_contactPerson, "Accepted", body);
                                //ScriptManager.RegisterStartupScript(this, GetType(), "function", "UsersAccepted(0);", true);
                            }
                            else
                            {
                                string body = "Hello " + user_contactPerson + "," + Environment.NewLine + Messages.cancelAccepted;
                                Controller.SendEmail(user_email, user_contactPerson, "Account disabled", body);
                                //ScriptManager.RegisterStartupScript(this, GetType(), "function", "UsersAccepted(1);", true);
                            }

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

                    string user_name = item.GetDataKeyValue("user_name").ToString();
                    using (SqlConnection con = new SqlConnection(Controller.connection))
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            Dictionary<string, string> fields = new Dictionary<string, string>();
                            fields.Add("user_removed", true.ToString());
                            fields.Add("modifedBy", Session["user_name"].ToString());
                            fields.Add("modifiedOn", DateTime.Now.ToString());

                            Dictionary<string, string> conditions = new Dictionary<string, string>();
                            conditions.Add("user_name", user_name);
                            con.Open();
                            Controller.Update(cmd, con, "SEC_User", fields, conditions);
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

    //*** AlertJS
    protected void AlertJS()
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert();", true);
    }
    //***

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

    //*** Redirect
    protected void Redirect(string path)
    {
        Response.Redirect(path, false);
        Context.ApplicationInstance.CompleteRequest();
    }
    //***

    //**** RadGridBoxes_ItemDataBound
    protected void RadGridBoxes_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem ditem = (GridDataItem)e.Item;
            Label user_isnew = (Label)ditem.FindControl("user_isnew");
            if (Boolean.Parse(user_isnew.Text) == true)
            {
                ditem.BackColor = System.Drawing.ColorTranslator.FromHtml("#4F52BA");
                ditem.ForeColor = Color.White;
            }
        }
    }

    //***** RadGrid1_SelectedIndexChanged
    protected void RadGridBoxes_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            GridDataItem item = (GridDataItem)RadGridBoxes.SelectedItems[0];//get selected row
            string user_name = item.GetDataKeyValue("user_name").ToString();

            Dictionary<string, string> fields = new Dictionary<string, string>();
            Dictionary<string, string> conditions = new Dictionary<string, string>();
            fields.Add("user_isnew", false.ToString());
            conditions.Add("user_name", user_name);

            using (SqlConnection con = new SqlConnection(Controller.connection))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    con.Open();
                    Controller.Update(cmd, con, "SEC_User", fields, conditions);
                }
            }
            ScriptManager.RegisterStartupScript(this, GetType(), "function", "NewUsers();", true);
            RadGridBoxes.Rebind();
        }
        catch (Exception ex)
        {
            Controller.SaveErrors(Path.GetFileName(Request.PhysicalPath), ex.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name);
        }
    }
    //***

    protected void refresh_Click(object sender, EventArgs e)
    {
        Redirect(Request.RawUrl);
    }
}