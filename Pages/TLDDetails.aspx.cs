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


public partial class Pages_TLD_TLDDetails : System.Web.UI.Page
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
            var decrypt = new eXSecurity.Decryption();
            string session = Session["tldh_id"].ToString();
            session = session.Replace(" ", "+");
            string tldh_id = decrypt.DecryptData(session);

            //RadGridBoxes.MasterTableView.IsItemInserted = true;

            using (SqlConnection con = new SqlConnection(Controller.connection))
            {
                using (SqlCommand cmd = new SqlCommand())
                { 
                    con.Open();
                    DataTable dt = new DataTable();

                    cmd.CommandText = "select * from CNRS_TLDHeaderDetails where active = @active and tldh_id = @tldh_id and tldhd_name <> @tldhd_name;";
                    cmd.Parameters.AddWithValue("active", true.ToString());
                    cmd.Parameters.AddWithValue("tldhd_name", "");
                    cmd.Parameters.AddWithValue("tldh_id", tldh_id);
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.Text;
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(dt);

                    RadGridBoxes.DataSource = dt;
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
        //componentID.Add("po_number");
        //componentID.Add("po_id");
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
                var decrypt = new eXSecurity.Decryption();
                string session = Session["tldh_id"].ToString();
                session = session.Replace(" ", "+");
                string tldh_id = decrypt.DecryptData(session);

                GridDataItem item = e.Item as GridDataItem;

                string po_id = "";
                string tldhd_name = Controller.Clean(((TextBox)item.FindControl("tldhd_nameTXT")).Text);

                using (SqlConnection con = new SqlConnection(Controller.connection))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        Dictionary<string, string> fields = new Dictionary<string, string>();
                        fields.Add("tldhd_name", tldhd_name); 
                        fields.Add("tldh_id", tldh_id);
                        fields.Add("createdBy", Session["user_name"].ToString());

                        con.Open();
                        po_id = Controller.InsertInto(cmd, con, "CNRS_TLDHeaderDetails", fields, true);
                    }
                }
            }


            if (e.CommandName == "Update")   //edit
            {
                if (e.Item is GridDataItem)
                {
                    GridDataItem item = e.Item as GridDataItem;

                    int tldhd_id = (int)item.GetDataKeyValue("tldhd_id");
                    string tldhd_name = Controller.Clean(((TextBox)item.FindControl("tldhd_nameTXT")).Text);

                    using (SqlConnection con = new SqlConnection(Controller.connection))
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            Dictionary<string, string> fields = new Dictionary<string, string>();
                            fields.Add("tldhd_name", tldhd_name);
                            fields.Add("modifedBy", Session["user_name"].ToString());
                            fields.Add("modifiedOn", DateTime.Now.ToString());

                            Dictionary<string, string> conditions = new Dictionary<string, string>();
                            conditions.Add("tldhd_id", tldhd_id.ToString());

                            con.Open();
                            Controller.Update(cmd, con, "CNRS_TLDHeaderDetails", fields, conditions);
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

                    int tldhd_id = (int)item.GetDataKeyValue("tldhd_id");
                    using (SqlConnection con = new SqlConnection(Controller.connection))
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            con.Open();
                            Controller.Inactivate(cmd, con, "CNRS_TLDHeaderDetails", "tldhd_id", tldhd_id.ToString(), Session["user_name"].ToString());
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
    protected void previous_Click(object sender, EventArgs e)
    {
        Redirect("TLDHeader.aspx");
    }
}