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

public partial class Pages_TLD_TLD : System.Web.UI.Page
{
    //*** Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        CheckSession();

        try
        {
            using (SqlConnection con = new SqlConnection(Controller.connection))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    con.Open();

                    //DataTable dtSec = Controller.Security(cmd, con, Path.GetFileName(Request.PhysicalPath), Session["user_name"].ToString());

                    Security();

                    if (!IsPostBack)
                    {
                        Translate(cmd, con);

                        langRCB.DataSource = Controller.SelectFrom(cmd, con, "SET_Language", new ArrayList(), new Dictionary<string, string>(), new ArrayList(), true, false, "");
                        langRCB.DataBind();



                        if (Session["language_id"].ToString() != "")
                        {
                            langRCB.SelectedValue = Session["language_id"].ToString();
                        }

                        CheckIfSended(cmd, con);

                        FillSessionTld();
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

    //*** CheckIfSended
    protected void CheckIfSended(SqlCommand cmd, SqlConnection con)
    {
        var decrypt = new eXSecurity.Decryption();
        string session;
         
        ArrayList fields = new ArrayList();
        fields.Add("po_sendIt");

        Dictionary<string, string> conditions = new Dictionary<string, string>();
        conditions.Add("po_id", getPoID());

        DataTable dt = Controller.SelectFrom(cmd, con, "CNRS_PurchaseOrder", fields, conditions, new ArrayList(), false, false, "");

        bool po_sendIt = (bool)dt.Rows[0]["po_sendIt"];
        if (po_sendIt == true)
        {
            hideAdd();
        }
        else
        {
            SendBTN.Visible = true;
            RadGridBoxes.MasterTableView.GetColumn("EditCommandColumn").Display = true;
            RadGridBoxes.MasterTableView.GetColumn("delete").Display = true;
            RadGridBoxes.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.Top;
            RadGridBoxes.Rebind();
        }
    }
    //***

    //*** CheckSession
    protected void CheckSession()
    {
        if (string.IsNullOrEmpty(Session["user_name"] as string))
        {
            Redirect("~/Login/Login.aspx");
            return;
        }
    }
    //***

    protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        CheckSession();
        try
        {
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

    protected void RadGrid1_DetailTableDataBind1(object sender, GridDetailTableDataBindEventArgs e)
    {
        CheckSession();

        GridDataItem dataItem = (GridDataItem)e.DetailTableView.ParentItem; 

        if (e.DetailTableView.Name == "Details")
        {
            try
            {

                using (SqlConnection con = new SqlConnection(Controller.connection))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    { 
                        string tldh_id = dataItem.GetDataKeyValue("tldh_id").ToString(); 

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

                        e.DetailTableView.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                Controller.SaveErrors(Path.GetFileName(Request.PhysicalPath), ex.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            }
        }
    }

    //*** RadGridBoxes_ItemCommand
    protected void RadGridBoxes_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    { 
        CheckSession();
        try
        {  
            if (e.CommandName == "Update")   //edit
            {
                if (e.Item is GridDataItem)
                {
                    GridDataItem item = e.Item as GridDataItem;

                    string tldhd_id = ((Label)item.FindControl("tldhd_idLBL")).Text;
                    string tldd_id = ((Label)item.FindControl("tldd_idLBL")).Text;
                    string tldd_employeNbr = Controller.Clean(((TextBox)item.FindControl("tldd_employeNbrTXT")).Text);
                    string tldd_employeName = Controller.Clean(((TextBox)item.FindControl("tldd_employeNameTXT")).Text);

                    using (SqlConnection con = new SqlConnection(Controller.connection))
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            Dictionary<string, string> fields = new Dictionary<string, string>();
                            fields.Add("tldd_employeNbr", tldd_employeNbr);
                            fields.Add("tldd_employeName", tldd_employeName);

                            con.Open();
                            Dictionary<string, string> conditions = new Dictionary<string, string>();
                            conditions.Add("tldd_id", tldd_id);

                            fields.Add("modifedBy", Session["user_name"].ToString());
                            fields.Add("modifiedOn", DateTime.Now.ToString());
                            Controller.Update(cmd, con, "CNRS_TLDDetails", fields, conditions);
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

    //*** Translate
    protected void Translate(SqlCommand cmd, SqlConnection con)
    {
        //ArrayList componentID = new ArrayList();
        //componentID.Add("po_number");
        //componentID.Add("po_id");
        //Controller.Translate(cmd, con, RadGridBoxes, Session["language_id"].ToString(), Path.GetFileName(Request.PhysicalPath), componentID);
    }
    //*** 

    protected void NoRecordsEdit_Click(object sender, EventArgs e)
    {
        try
        { 
            string po_id = getPoID();

            RadButton ibtn = (RadButton)sender;
            GridNoRecordsItem item = (GridNoRecordsItem)ibtn.NamingContainer;
            GridDataItem parentItem = (GridDataItem)item.OwnerTableView.ParentItem;
            string tldh_id = parentItem.GetDataKeyValue("tldh_id").ToString();
             
            TextBox tldd_employeNbr = (TextBox)item.FindControl("tldd_employeNbrTXT");    
            TextBox tldd_employeName = (TextBox)item.FindControl("tldd_employeNameTXT"); 
             
            using (SqlConnection con = new SqlConnection(Controller.connection))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    Dictionary<string, string> fields = new Dictionary<string, string>(); 
                    Dictionary<string, string> conditions = new Dictionary<string, string>(); 
                    conditions.Add("active", true.ToString());
                    conditions.Add("tldhd_name", "");
                    conditions.Add("tldh_id", tldh_id);
                    conditions.Add("po_id", po_id);

                    con.Open();
                    DataTable dt = Controller.SelectFrom(cmd, con, "View_TLDHeaderDetails", new ArrayList(), conditions, new ArrayList(), true, false, "");

                    fields.Clear();
                    fields.Add("tldd_employeNbr", tldd_employeNbr.Text);
                    fields.Add("tldd_employeName", tldd_employeName.Text);

                    if (dt.Rows.Count > 0)   // if exist this row 
                    {
                        conditions.Clear();
                        conditions.Add("tldd_id", dt.Rows[0]["tldd_id"].ToString());
                        fields.Add("modifedBy", Session["user_name"].ToString());
                        fields.Add("modifiedOn", DateTime.Now.ToString());
                        Controller.Update(cmd, con, "CNRS_TLDDetails", fields, conditions);
                    }
                    else
                    {
                        ArrayList field = new ArrayList();
                        field.Add("tldhd_id");
                        conditions.Clear(); 
                        conditions.Add("tldhd_name", "");
                        conditions.Add("tldh_id", tldh_id); 
                        DataTable dt1 = Controller.SelectFrom(cmd, con, "CNRS_TLDHeaderDetails", field, conditions, new ArrayList(), false, false, "");
                        string tldhd_id = dt1.Rows[0]["tldhd_id"].ToString();

                        fields.Add("createdBy", Session["user_name"].ToString());
                        fields.Add("tldhd_id", tldhd_id);
                        fields.Add("po_id", po_id);
                        Controller.InsertInto(cmd, con, "CNRS_TLDDetails", fields, false);
                    }

                    AlertJS();
                }
            }
        }
        catch (Exception ex)
        {
            Controller.SaveErrors(Path.GetFileName(Request.PhysicalPath), ex.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name);
        }
    }

    protected void RadGridBoxes_ItemCreated(object sender, GridItemEventArgs e)
    {
        CheckSession();
        try
        { 
            string po_id = getPoID();

            if (e.Item is GridNoRecordsItem)
            {
                GridNoRecordsItem item = (GridNoRecordsItem)e.Item;
                //GridNoRecordsItem item = (GridNoRecordsItem)RadGridBoxes.MasterTableView.GetItems(GridItemType.NoRecordsItem)[0];
                TextBox tldd_employeNbrTXT = (TextBox)item.FindControl("tldd_employeNbrTXT");
                TextBox tldd_employeNameTXT = (TextBox)item.FindControl("tldd_employeNameTXT");
                RadButton noRecEditBTN = (RadButton)item.FindControl("noRecEditBTN");

                string tldh_id;
                try
                {
                    GridDataItem parentItem = e.Item.OwnerTableView.ParentItem;
                    tldh_id = parentItem.GetDataKeyValue("tldh_id").ToString();
                }
                catch (Exception ex)
                {
                    return;
                }
                using (SqlConnection con = new SqlConnection(Controller.connection))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        con.Open();
                        DataTable dt = new DataTable();

                        cmd.CommandText = "select * from View_TLDHeaderDetails where active = @active and tldh_id = @tldh_id and tldhd_name = @tldhd_name and po_id = @po_id;";
                        cmd.Parameters.AddWithValue("active", true.ToString());
                        cmd.Parameters.AddWithValue("tldhd_name", "");
                        cmd.Parameters.AddWithValue("tldh_id", tldh_id);
                        cmd.Parameters.AddWithValue("po_id", po_id);
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.Text;
                        SqlDataAdapter sda = new SqlDataAdapter();
                        sda.SelectCommand = cmd;
                        sda.Fill(dt);

                        if (dt.Rows.Count > 0)
                        {
                            tldd_employeNbrTXT.Text = dt.Rows[0]["tldd_employeNbr"].ToString();
                            tldd_employeNameTXT.Text = dt.Rows[0]["tldd_employeName"].ToString();

                            if (Boolean.Parse(dt.Rows[0]["po_sendIt"].ToString()) == true)
                            {
                                noRecEditBTN.Visible = false; 
                            }
                        }
                    }
                }
            }

            else if ((e.Item is GridDataItem))
            {
                GridDataItem item = (GridDataItem)e.Item;

                TextBox tldd_employeNbrTXT = (TextBox)item.FindControl("tldd_employeNbrTXT");
                TextBox tldd_employeNameTXT = (TextBox)item.FindControl("tldd_employeNameTXT");  
                Label tldd_idLBL = (Label)item.FindControl("tldd_idLBL");
                string tldhd_id;

                try { 
                    tldhd_id = item.GetDataKeyValue("tldhd_id").ToString();
                }
                catch (Exception ex)
                {
                    return;
                }

                using (SqlConnection con = new SqlConnection(Controller.connection))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        con.Open();
                        DataTable dt = new DataTable();

                        cmd.CommandText = "select * from CNRS_TLDDetails where active = @active and tldhd_id = @tldhd_id and po_id = @po_id;";
                        cmd.Parameters.AddWithValue("active", true.ToString()); 
                        cmd.Parameters.AddWithValue("tldhd_id", tldhd_id);
                        cmd.Parameters.AddWithValue("po_id", po_id);
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.Text;
                        SqlDataAdapter sda = new SqlDataAdapter();
                        sda.SelectCommand = cmd;
                        sda.Fill(dt);

                        if (dt.Rows.Count > 0)
                        {
                            tldd_employeNbrTXT.Text = dt.Rows[0]["tldd_employeNbr"].ToString();
                            tldd_employeNameTXT.Text = dt.Rows[0]["tldd_employeName"].ToString();
                            tldd_idLBL.Text = dt.Rows[0]["tldd_id"].ToString();
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

    //*** send
    protected void SendBTN_Click(object sender, EventArgs e)
    {
        try
        { 
            string po_id = getPoID();

            if (RadGridBoxes.MasterTableView.Items.Count > 0)
            {
                using (SqlConnection con = new SqlConnection(Controller.connection))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    { 
                        con.Open();
                        Controller.UpdateSendedDate(con, cmd, po_id);
                        AlertJS(1);

                        hideAdd();
                    }
                }
            }
            else
            {
                AlertJS(2);
            }
        }
        catch (Exception ex)
        {
            Controller.SaveErrors(Path.GetFileName(Request.PhysicalPath), ex.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name);
        }
    }
    //***
    protected string getPoID()
    {
        var decrypt = new eXSecurity.Decryption();
        string session;
        try
        {
            session = Request["P"].ToString();
            session = session.Replace(" ", "+");
        }
        catch (Exception ex)
        {
            Redirect("~/Pages/PurchaseOrder.aspx");
            return "";
        }
        return decrypt.DecryptData(session);
    }

    protected void hideAdd()
    {
        SendBTN.Visible = false;
        RadGridBoxes.MasterTableView.DetailTables[0].GetColumn("EditCommandColumn").Visible = false; 
        RadGridBoxes.Rebind();
    }
    //****

    //*** AlertJS
    protected void AlertJS(int index)
    {
        if (index == 1)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert();", true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert();", true);
        }
    }
    //***

    //*** Security
    protected void Security()
    {
        string role_id = getRoleID();

        if (role_id == "1" || role_id == "2")  //  client or admin
        {
            RadGrid1.ClientSettings.EnablePostBackOnRowClick = true;
            RadGrid1.ClientSettings.Selecting.AllowRowSelect = true;
            RadGridBoxes2.ClientSettings.EnablePostBackOnRowClick = true;
            RadGridBoxes2.ClientSettings.Selecting.AllowRowSelect = true;
        }
        else
        {
            RadGrid1.ClientSettings.EnablePostBackOnRowClick = false;
            RadGrid1.ClientSettings.Selecting.AllowRowSelect = false;
            RadGridBoxes2.ClientSettings.EnablePostBackOnRowClick = false;
            RadGridBoxes2.ClientSettings.Selecting.AllowRowSelect = false;
        }
    }

    //***   Menu    
    protected void logoutBTN_Click(object sender, System.EventArgs e)
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

    protected string getRoleID()
    {
        var decrypt = new eXSecurity.Decryption();
        string session;
        try
        {
            session = Session["role_id"].ToString();
            session = session.Replace(" ", "+");
        }
        catch (Exception ex)
        {
            Session["user_name"] = "";
            Redirect("~/Login/Login.aspx");
            return "";
        }
        return decrypt.DecryptData(session);
    }

    protected void RadGrid1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string role_id = getRoleID();

            if (role_id == "1" || role_id == "2")
            {
                GridDataItem item = (GridDataItem)RadGrid1.SelectedItems[0];//get selected row
                string tldReagent_id = "";

                Dictionary<string, string> fields = new Dictionary<string, string>();
                Dictionary<string, string> conditions = new Dictionary<string, string>();
                if (item.BackColor == System.Drawing.ColorTranslator.FromHtml("#4F52BA"))
                    tldReagent_id = DBNull.Value.ToString();
                else
                    tldReagent_id = item.GetDataKeyValue("tldReagent_id").ToString();
                fields.Add("tldReagent_id_isNotInterval", tldReagent_id);
                conditions.Add("po_id", getPoID());

                using (SqlConnection con = new SqlConnection(Controller.connection))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        con.Open();
                        Controller.Update(cmd, con, "CNRS_PurchaseOrder", fields, conditions);
                        Session["tldReagent_id_isNotInterval"] = tldReagent_id;
                    }
                }
                Response.Redirect(Page.Request.RawUrl, false);
                Context.ApplicationInstance.CompleteRequest();
                //RadGrid1.Rebind();
            }
        }
        catch (Exception ex)
        {
            Controller.SaveErrors(Path.GetFileName(Request.PhysicalPath), ex.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name);
        }
    } 

    //**** RadGridBoxes_ItemDataBound
    protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                string tld = Session["tldReagent_id_isNotInterval"].ToString();
                if (tld != "")
                {
                    GridDataItem ditem = (GridDataItem)e.Item;
                    string tldReagent_id = ditem.GetDataKeyValue("tldReagent_id").ToString();
                    if (tldReagent_id == tld)
                    {
                        ditem.BackColor = System.Drawing.ColorTranslator.FromHtml("#4F52BA");
                        ditem.ForeColor = Color.White;
                    }
                }
                }
            }
        catch (Exception ex)
        {
            Controller.SaveErrors(Path.GetFileName(Request.PhysicalPath), ex.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name);
        }
    }

    protected void RadGridBoxes2_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            { 
                string tld = Session["tldReagent_id_isInterval"].ToString();
                if (tld != "")
                {
                    GridDataItem ditem = (GridDataItem)e.Item;
                    string tldReagent_id = ditem.GetDataKeyValue("tldReagent_id").ToString();
                    if (tldReagent_id == tld)
                    {
                        ditem.BackColor = System.Drawing.ColorTranslator.FromHtml("#4F52BA");
                        ditem.ForeColor = Color.White;
                    }
                }
                }
            }
        catch (Exception ex)
        {
            Controller.SaveErrors(Path.GetFileName(Request.PhysicalPath), ex.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name);
        }
    }

    protected void FillSessionTld()
    {
        try
        { 
            Dictionary<string, string> conditions = new Dictionary<string, string>();
            conditions.Add("po_id", getPoID());

            using (SqlConnection con = new SqlConnection(Controller.connection))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    con.Open();
                    DataTable dt = Controller.ExtraSelect (con, cmd, "select tldReagent_id_isNotInterval, tldReagent_id_isInterval from CNRS_PurchaseOrder", conditions, "");
                    Session["tldReagent_id_isNotInterval"] = dt.Rows[0]["tldReagent_id_isNotInterval"].ToString();
                    Session["tldReagent_id_isInterval"] = dt.Rows[0]["tldReagent_id_isInterval"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            Controller.SaveErrors(Path.GetFileName(Request.PhysicalPath), ex.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name);
        }
    }

    protected void RadGridBoxes2_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string role_id = getRoleID();

            if (role_id == "1" || role_id == "2")
            {
                GridDataItem item = (GridDataItem)RadGridBoxes2.SelectedItems[0];//get selected row
                string tldReagent_id = "";

                Dictionary<string, string> fields = new Dictionary<string, string>();
                Dictionary<string, string> conditions = new Dictionary<string, string>();
                if (item.BackColor == System.Drawing.ColorTranslator.FromHtml("#4F52BA"))
                    tldReagent_id = DBNull.Value.ToString();
                else
                    tldReagent_id = item.GetDataKeyValue("tldReagent_id").ToString();
                fields.Add("tldReagent_id_isInterval", tldReagent_id);
                conditions.Add("po_id", getPoID());

                using (SqlConnection con = new SqlConnection(Controller.connection))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        con.Open();
                        Controller.Update(cmd, con, "CNRS_PurchaseOrder", fields, conditions);
                        Session["tldReagent_id_isInterval"] = tldReagent_id;
                    }
                }
                Response.Redirect(Page.Request.RawUrl, false);
                Context.ApplicationInstance.CompleteRequest();
                // RadGridBoxes2.Rebind();
            }
        }
        catch (Exception ex)
        {
            Controller.SaveErrors(Path.GetFileName(Request.PhysicalPath), ex.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name);
        }
    }
    protected void previous_Click(object sender, EventArgs e)
    {
        Redirect("PurchaseOrder.aspx");
    }
}
