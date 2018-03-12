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

public partial class Pages_TLD_TLDReagent : System.Web.UI.Page
{
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
                    //DataTable dtSec = Controller.Security(cmd, con, Path.GetFileName(Request.PhysicalPath), Session["user_name"]);

                    // Security(dtSec);

                    if (!IsPostBack)
                    {
                        Translate(cmd, con);

                        //langRCB.DataSource = Controller.SelectFrom(cmd, con, "SET_Language", new ArrayList(), new Dictionary<string, string>(), new ArrayList(), true, false, "");
                        //langRCB.DataBind();



                        //if (Session["language_id"].ToString() != "")
                        //{
                        //    langRCB.SelectedValue = Session["language_id"].ToString();
                        //}
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
                    conditions.Add("tldReagent_isInterval", false.ToString());

                    Controller.FillRadGrid(cmd, con, sender, "CNRS_TLDReagent", new ArrayList(), conditions, true, "");
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

        }
    }
    //***

    //*** Translate
    protected void Translate(SqlCommand cmd, SqlConnection con)
    {
        ArrayList componentID = new ArrayList();
        componentID.Add("po_number");
        componentID.Add("po_id");
        Controller.Translate(cmd, con, RadGridBoxes, Session["language_id"].ToString(), Path.GetFileName(Request.PhysicalPath), componentID);
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

                string tldReagent_minNbr = Controller.Clean(((TextBox)item.FindControl("tldReagent_minNbrTXT")).Text);  
                string tldReagent_maxNbr = Controller.Clean(((TextBox)item.FindControl("tldReagent_maxNbrTXT")).Text);
                string tldReagent_InsteadOfInsurance = Controller.Clean(((TextBox)item.FindControl("tldReagent_InsteadOfInsuranceTXT")).Text);

                using (SqlConnection con = new SqlConnection(Controller.connection))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        Dictionary<string, string> fields = new Dictionary<string, string>();
                        fields.Add("tldReagent_minNbr", tldReagent_minNbr);
                        fields.Add("tldReagent_maxNbr", tldReagent_maxNbr);
                        fields.Add("tldReagent_InsteadOfInsurance", tldReagent_InsteadOfInsurance);  //badal al ta2min
                        fields.Add("tldReagent_isInterval", false.ToString());
                        fields.Add("createdBy", Session["user_name"].ToString());
                        con.Open();
                        Controller.InsertInto(cmd, con, "CNRS_TLDReagent", fields, true);
                        AlertJS();
                    }
                } 
            }


            if (e.CommandName == "Update")   //edit
            {
                if (e.Item is GridDataItem)
                {
                    GridDataItem item = e.Item as GridDataItem;

                    string tldReagent_id = item.GetDataKeyValue("tldReagent_id").ToString();
                    string tldReagent_minNbr = Controller.Clean(((TextBox)item.FindControl("tldReagent_minNbrTXT")).Text);
                    string tldReagent_maxNbr = Controller.Clean(((TextBox)item.FindControl("tldReagent_maxNbrTXT")).Text);
                    string tldReagent_InsteadOfInsurance = Controller.Clean(((TextBox)item.FindControl("tldReagent_InsteadOfInsuranceTXT")).Text);

                    using (SqlConnection con = new SqlConnection(Controller.connection))
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            Dictionary<string, string> fields = new Dictionary<string, string>();
                            fields.Add("tldReagent_minNbr", tldReagent_minNbr);
                            fields.Add("tldReagent_maxNbr", tldReagent_maxNbr);
                            fields.Add("tldReagent_InsteadOfInsurance", tldReagent_InsteadOfInsurance);
                            fields.Add("modifedBy", Session["user_name"].ToString());
                            fields.Add("modifiedOn", DateTime.Now.ToString());

                            Dictionary<string, string> conditions = new Dictionary<string, string>();
                            conditions.Add("tldReagent_id", tldReagent_id);

                            con.Open();
                            Controller.Update(cmd, con, "CNRS_TLDReagent", fields, conditions);
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

                    string tldReagent_id = item.GetDataKeyValue("tldReagent_id").ToString();
                    using (SqlConnection con = new SqlConnection(Controller.connection))
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            con.Open();
                            Controller.Inactivate(cmd, con, "CNRS_TLDReagent", "tldReagent_id", tldReagent_id, Session["user_name"].ToString());
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

    //*** RadGridBoxes2_NeedDataSource1
    protected void RadGridBoxes2_NeedDataSource1(object sender, GridNeedDataSourceEventArgs e)
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
                    conditions.Add("tldReagent_isInterval", true.ToString());

                    Controller.FillRadGrid(cmd, con, sender, "CNRS_TLDReagent", new ArrayList(), conditions, true, "");
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


    //*** RadGridBoxes2_ItemCommand
    protected void RadGridBoxes2_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "PerformInsert")   // add
            {
                GridDataItem item = e.Item as GridDataItem;

                string tldReagent_minNbr = Controller.Clean(((TextBox)item.FindControl("tldReagent_minNbrTXT2")).Text);
                string tldReagent_maxNbr = Controller.Clean(((TextBox)item.FindControl("tldReagent_maxNbrTXT2")).Text);
                string tldReagent_InsteadOfInsurance = Controller.Clean(((TextBox)item.FindControl("tldReagent_InsteadOfInsuranceTXT2")).Text);
                string tldReagent_Part = Controller.Clean(((TextBox)item.FindControl("tldReagent_PartTXT")).Text);

                using (SqlConnection con = new SqlConnection(Controller.connection))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        Dictionary<string, string> fields = new Dictionary<string, string>();
                        fields.Add("tldReagent_minNbr", tldReagent_minNbr);
                        fields.Add("tldReagent_maxNbr", tldReagent_maxNbr);
                        fields.Add("tldReagent_InsteadOfInsurance", tldReagent_InsteadOfInsurance);
                        fields.Add("tldReagent_isInterval", true.ToString());
                        fields.Add("createdBy", Session["user_name"].ToString());
                        fields.Add("tldReagent_Part", tldReagent_Part);

                        con.Open();
                        Controller.InsertInto(cmd, con, "CNRS_TLDReagent", fields, true);
                        AlertJS();
                    }
                }
            }


            if (e.CommandName == "Update")   //edit
            {
                if (e.Item is GridDataItem)
                {
                    GridDataItem item = e.Item as GridDataItem;

                    string tldReagent_id = item.GetDataKeyValue("tldReagent_id").ToString();
                    string tldReagent_minNbr = Controller.Clean(((TextBox)item.FindControl("tldReagent_minNbrTXT2")).Text);
                    string tldReagent_maxNbr = Controller.Clean(((TextBox)item.FindControl("tldReagent_maxNbrTXT2")).Text);
                    string tldReagent_InsteadOfInsurance = Controller.Clean(((TextBox)item.FindControl("tldReagent_InsteadOfInsuranceTXT2")).Text);
                    string tldReagent_Part = Controller.Clean(((TextBox)item.FindControl("tldReagent_PartTXT")).Text);

                    using (SqlConnection con = new SqlConnection(Controller.connection))
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            Dictionary<string, string> fields = new Dictionary<string, string>();
                            fields.Add("tldReagent_minNbr", tldReagent_minNbr);
                            fields.Add("tldReagent_maxNbr", tldReagent_maxNbr);
                            fields.Add("tldReagent_InsteadOfInsurance", tldReagent_InsteadOfInsurance);
                            fields.Add("modifedBy", Session["user_name"].ToString());
                            fields.Add("modifiedOn", DateTime.Now.ToString());
                            fields.Add("tldReagent_Part", tldReagent_Part);

                            Dictionary<string, string> conditions = new Dictionary<string, string>();
                            conditions.Add("tldReagent_id", tldReagent_id);

                            con.Open();
                            Controller.Update(cmd, con, "CNRS_TLDReagent", fields, conditions);
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

                    string tldReagent_id = item.GetDataKeyValue("tldReagent_id").ToString();
                    using (SqlConnection con = new SqlConnection(Controller.connection))
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            con.Open();
                            Controller.Inactivate(cmd, con, "CNRS_TLDReagent", "tldReagent_id", tldReagent_id, Session["user_name"].ToString());
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

    //***   Menu    
    protected void logoutBTN_Click(object sender, ImageClickEventArgs e)
    {
        Session["user_name"] = "";
        Session["language_id"] = "";
        Redirect("~/Login/Login.aspx");
    }

    protected void navgPoBTN_Click(object sender, ImageClickEventArgs e)
    {
        Redirect("PurchaseOrder.aspx");
    }
    //***

    //protected void langRCB_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    //{
    //    Session["language_id"] = langRCB.SelectedValue;
    //    try
    //    {
    //        using (SqlConnection con = new SqlConnection(Controller.connection))
    //        {
    //            using (SqlCommand cmd = new SqlCommand())
    //            {
    //                Translate(cmd, con);
    //                RadGridBoxes.Rebind();
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        Controller.SaveErrors(Path.GetFileName(Request.PhysicalPath), ex.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name);
    //    }
    //}


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