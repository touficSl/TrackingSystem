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

public partial class PurchaseOrder : System.Web.UI.Page
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
        CheckSession();
    //    RadGridBoxes.MasterTableView.GetColumn("EditCommandColumn").Display = false;
    //   RadGridBoxes.MasterTableView.GetColumn("edit").Display = true;

        try
        {
            Visibilty();

            using (SqlConnection con = new SqlConnection(Controller.connection))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    con.Open();

                    if (!IsPostBack)
                    {
                        //Translate(cmd, con);

                        //langRCB.DataSource  = Controller.SelectFrom(cmd, con, "SET_Language", new ArrayList(), new Dictionary<string, string>(), new ArrayList(), true, false, "");
                        //langRCB.DataBind();

                        if (Session["language_id"].ToString() != "")
                        {
                            langRCB.SelectedValue = Session["language_id"].ToString();
                        }

                        DataTable dtSec = Controller.Security(cmd, con, Path.GetFileName(Request.PhysicalPath), Session["user_name"].ToString());

                        Security(dtSec);
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

    //*** Visibilty
    protected void Visibilty()
    {
        RadGridBoxes.MasterTableView.GetColumn("nextStep").Display = true;
        RadGridBoxes.MasterTableView.GetColumn("Print").Display = true;
    }
    //****     

    //*** CheckSession
    protected void CheckSession()
    { 
        string role_id = getRoleID();
         
        if (string.IsNullOrEmpty(Session["user_name"] as string) || role_id == null)
        {
            Redirect("~/Login/Login.aspx");
        }
    }
    //***       

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

    //*** RadGridBoxes_NeedDataSource1
    protected void RadGridBoxes_NeedDataSource1(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            string role_id = getRoleID();

            CheckSession();
            //RadGridBoxes.MasterTableView.IsItemInserted = true;

            using (SqlConnection con = new SqlConnection(Controller.connection))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    con.Open();
                    Dictionary<String, String> conditions = new Dictionary<string, string>();
                    conditions.Add("active", true.ToString());

                    if (role_id == "2") // client
                        conditions.Add("user_name", Session["user_name"].ToString());
                    else if (role_id == "3") // Secretary 
                        conditions.Add("po_sendIt", true.ToString());
                    else if(role_id == "4") // Director
                        conditions.Add("po_accept_secretary", true.ToString());
                    else if (role_id == "5") // Head of Department
                        conditions.Add("po_accept_director", true.ToString());
                    else if (role_id == "6") // Technical Officer
                        conditions.Add("po_accept_headOfDepartment", true.ToString());

                    Controller.FillRadGrid(cmd, con, sender, "View_PurchaseOrder", new ArrayList(), conditions, true, "createdOn");
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
    protected void Security (DataTable dt)
    {
        string role_id = getRoleID();

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

            RadGridBoxes.MasterTableView.GetColumn("createdOn").Display = Controller.Can(dt, "can_viewReceivedDate");
            
            RadGridBoxes.MasterTableView.GetColumn("po_number").Display = Controller.Can(dt, "can_fillPoNbr");

            RadGridBoxes.MasterTableView.GetColumn("official_number").Display = Controller.Can(dt, "can_fillOfficialNbre");

            RadGridBoxes.MasterTableView.GetColumn("delete").Display = Controller.Can(dt, "can_delete");

            RadGridBoxes.MasterTableView.GetColumn("EditCommandColumn").Display = Controller.Can(dt, "can_edit");

            RadGridBoxes.MasterTableView.GetColumn("Print").Display = Controller.Can(dt, "can_export");


            RadGridBoxes.MasterTableView.GetColumn("po_accept_director").Display = false;
            RadGridBoxes.MasterTableView.GetColumn("po_accept_secretary").Display = false;
            RadGridBoxes.MasterTableView.GetColumn("po_accept_headOfDepartment").Display = false;

            if (role_id == "4") // director
                RadGridBoxes.MasterTableView.GetColumn("po_accept_director").Display = Controller.Can(dt, "can_acceptPO");
            else if (role_id == "3") //secretary
                RadGridBoxes.MasterTableView.GetColumn("po_accept_secretary").Display = Controller.Can(dt, "can_acceptPO");
            else if (role_id == "5") //headOfDepartment
                RadGridBoxes.MasterTableView.GetColumn("po_accept_headOfDepartment").Display = Controller.Can(dt, "can_finishAcceptingPO");

            RadGridBoxes.MasterTableView.GetColumn("po_date_end_technicalOfficer").Display = Controller.Can(dt, "can_reachedPO");

            RadGridBoxes.MasterTableView.GetColumn("po_date_secretary").Display = Controller.Can(dt, "can_viewSecretayRD"); 

            RadGridBoxes.MasterTableView.GetColumn("po_date_director").Display = Controller.Can(dt, "can_viewDirectorRD"); 

            RadGridBoxes.MasterTableView.GetColumn("po_date_headOfDepartment").Display = Controller.Can(dt, "can_viewHODRD");

            //RadGridBoxes.MasterTableView.GetColumn("po_date_technicalOfficer").Display = Controller.Can(dt, "can_viewTechnicalOffRD");
        }

        RadGridBoxes.ClientSettings.EnablePostBackOnRowClick = true;
        RadGridBoxes.ClientSettings.Selecting.AllowRowSelect = true;
        RadGridBoxes.ClientSettings.Scrolling.AllowScroll = false;
        if (role_id == "2")  //  client
        {
            RadGridBoxes.MasterTableView.GetColumn("user_contactPerson").Display = false;
            RadGridBoxes.MasterTableView.GetColumn("user_companyName").Display = false;
            RadGridBoxes.MasterTableView.GetColumn("user_address").Display = false;

            RadGridBoxes.ClientSettings.EnablePostBackOnRowClick = false;
            RadGridBoxes.ClientSettings.Selecting.AllowRowSelect = false;
        }
        else if (role_id == "1") //admin
        {
            RadGridBoxes.ClientSettings.Scrolling.AllowScroll = true;
        }
    }
    //***

    //*** Translate
    protected void Translate(SqlCommand cmd, SqlConnection con)
    {
        ArrayList componentID = new ArrayList();
        componentID.Add("po_number"); 
        componentID.Add("user_name");
        componentID.Add("official_number");
        componentID.Add("item_name"); 
        componentID.Add("po_date"); 
        componentID.Add("po_accept_director");  
        componentID.Add("po_accept_headOfDepartment");
        componentID.Add("po_date_end_technicalOfficer");
        componentID.Add("nextStep");
        componentID.Add("Print");
        Controller.Translate(cmd, con, RadGridBoxes, Session["language_id"].ToString(), Path.GetFileName(Request.PhysicalPath), componentID);
    }
    //***


    protected void RadGridBoxes_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        try
        {
            string role_id = getRoleID(); 

            if (!(e.Item is GridDataInsertItem) || !(e.Item is GridEditableItem))
            { 
                if (e.Item is GridDataItem)
                {
                    GridDataItem ditem = (GridDataItem)e.Item;
                    Label po_sendItLBL = (Label)ditem.FindControl("po_sendItLBL");
                    ImageButton EditCommandColumn = (ImageButton)ditem["EditCommandColumn"].Controls[0];
                    ImageButton DeleteCommandColumn = (ImageButton)ditem["delete"].Controls[0];

                    if (role_id == "2")
                    {
                        if (po_sendItLBL.Text.Equals("True"))
                        {
                            EditCommandColumn.Visible = false;
                            DeleteCommandColumn.Visible = false;
                        }
                        else
                        {
                            EditCommandColumn.Visible = true;
                            DeleteCommandColumn.Visible = true;
                        }
                    }

                    if (role_id == "1")
                    {
                        Label po_isnewforAdmin = (Label)ditem.FindControl("po_isnewforAdmin");
                        if (Boolean.Parse(po_isnewforAdmin.Text) == true)
                        {
                            ditem.BackColor = System.Drawing.ColorTranslator.FromHtml("#4F52BA");
                            ditem.ForeColor = Color.White;
                        }
                    }

                    if (role_id == "3")
                    {
                        Label po_isnewforSectretary = (Label)ditem.FindControl("po_isnewforSectretary");
                        if (Boolean.Parse(po_isnewforSectretary.Text) == true)
                        {
                            ditem.BackColor = System.Drawing.ColorTranslator.FromHtml("#4F52BA");
                            ditem.ForeColor = Color.White;
                        }
                    }

                    if (role_id == "4")
                    {
                        Label po_isnewforDirector = (Label)ditem.FindControl("po_isnewforDirector");
                        if (Boolean.Parse(po_isnewforDirector.Text) == true)
                        {
                            ditem.BackColor = System.Drawing.ColorTranslator.FromHtml("#4F52BA");
                            ditem.ForeColor = Color.White;
                        }
                    }

                    if (role_id == "5")
                    {
                        Label po_isnewforHOD = (Label)ditem.FindControl("po_isnewforHOD");
                        if (Boolean.Parse(po_isnewforHOD.Text) == true)
                        {
                            ditem.BackColor = System.Drawing.ColorTranslator.FromHtml("#4F52BA");
                            ditem.ForeColor = Color.White;
                        }
                    }

                    if (role_id == "6") // Technical Officer
                    {
                        Label po_isnewforTechmical = (Label)ditem.FindControl("po_isnewforTechmical");
                        if (Boolean.Parse(po_isnewforTechmical.Text) == true)
                        {
                            ditem.BackColor = System.Drawing.ColorTranslator.FromHtml("#4F52BA");
                            ditem.ForeColor = Color.White;
                        }


                        Label po_date_end_technicalOfficerLBL = (Label)ditem.FindControl("po_date_end_technicalOfficerLBL");
                        CheckBox po_date_end_technicalOfficerRCB = (CheckBox)ditem.FindControl("po_date_end_technicalOfficerRCB");


                        if (po_date_end_technicalOfficerLBL.Text != "")
                        {
                            po_date_end_technicalOfficerLBL.Visible = true;
                            po_date_end_technicalOfficerRCB.Visible = false;
                        }

                        else
                        {
                            po_date_end_technicalOfficerLBL.Visible = false;
                            po_date_end_technicalOfficerRCB.Visible = true;
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

    //RadGridBoxes_SelectedIndexChanged
    protected void RadGridBoxes_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string role_id = getRoleID();
            if (role_id != "2")
            {
                GridDataItem item = (GridDataItem)RadGridBoxes.SelectedItems[0];//get selected row
                string po_id = item.GetDataKeyValue("po_id").ToString();

                Dictionary<string, string> fields = new Dictionary<string, string>();
                Dictionary<string, string> conditions = new Dictionary<string, string>();
                conditions.Add("po_id", po_id);


                if (role_id == "1")
                {
                    fields.Add("po_isnewforAdmin", false.ToString());
                }
                else
                if (role_id == "3")
                {
                    fields.Add("po_isnewforSectretary", false.ToString());
                }
                else
                if (role_id == "4")
                {
                    fields.Add("po_isnewforDirector", false.ToString());
                }
                else
                if (role_id == "5")
                {
                    fields.Add("po_isnewforHOD", false.ToString());
                }
                else
                if (role_id == "6")
                {
                    fields.Add("po_isnewforTechmical", false.ToString());
                }


                using (SqlConnection con = new SqlConnection(Controller.connection))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        con.Open();
                        Controller.Update(cmd, con, "CNRS_PurchaseOrder", fields, conditions);
                    }
                }
            RadGridBoxes.Rebind();
            }
        }
        catch (Exception ex)
        {
            Controller.SaveErrors(Path.GetFileName(Request.PhysicalPath), ex.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name);
        }
    }
    //***


    //*** RadGridBoxes_ItemCreated
    protected void RadGridBoxes_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        try
        {
            string role_id = getRoleID();

            CheckSession();
            if (e.Item is GridEditableItem && e.Item.IsInEditMode)
            {
                RadGridBoxes.MasterTableView.GetColumn("nextStep").Display = false;
                RadGridBoxes.MasterTableView.GetColumn("Print").Display = false;

                GridEditableItem item = (GridEditableItem)e.Item;
                RadComboBox item_idRCB = (RadComboBox)item.FindControl("item_idRCB");
                RequiredFieldValidator item_idRFV = (RequiredFieldValidator)item.FindControl("item_idRFV");
                Label item_idLBL1 = (Label)item.FindControl("item_idLBL1");
                Label startLBL = (Label)item.FindControl("startLBL");
                if (e.Item is GridEditFormInsertItem || e.Item is GridDataInsertItem)
                {
                    // insert item 
                    item_idRCB.Visible = true;
                    item_idRFV.Enabled = true;
                    item_idLBL1.Visible = false;
                    startLBL.Visible = false;

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
                            item_idRCB.DataSource = Controller.SelectFrom(cmd, con, "CNRS_Item", fields, conditions, new ArrayList(), false, false, "");
                        }
                    }
                }
                else
                {
                    // edit item
                    GridEditableItem editItem = (GridEditableItem)e.Item;

                    RadDatePicker edit_po_dateRDP = ((RadDatePicker)editItem.FindControl("po_dateRDPTXT"));

                    item_idRCB.Visible = false;
                    item_idRFV.Enabled = false;
                    item_idLBL1.Visible = true;
                    startLBL.Visible = true; 

                    if (role_id != "1" && role_id != "2")  // != admin and != client
                    {
                        edit_po_dateRDP.Enabled = false;
                    }
                    else
                    {
                        edit_po_dateRDP.Enabled = true;
                    }
                } 

                //RadDatePicker po_dateRDP = (RadDatePicker)item.FindControl("po_dateRDP");
                //po_dateRDP.SelectedDate = DateTime.Now;  
            }

            if (e.Item is GridDataItem)
            {
                if (role_id == "6") // Technical Officer
                {
                    GridDataItem ditem = (GridDataItem)e.Item;
                    Label po_date_end_technicalOfficerLBL = (Label)ditem.FindControl("po_date_end_technicalOfficerLBL");
                    CheckBox po_date_end_technicalOfficerRCB = (CheckBox)ditem.FindControl("po_date_end_technicalOfficerRCB");

                    if (po_date_end_technicalOfficerLBL.Text != "")
                    {
                        po_date_end_technicalOfficerLBL.Visible = true;
                        po_date_end_technicalOfficerRCB.Visible = false;
                    }

                    else
                    {
                        po_date_end_technicalOfficerLBL.Visible = false;
                        po_date_end_technicalOfficerRCB.Visible = true;
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
            CheckSession();
            if (e.CommandName == "PerformInsert")   // add
            {
                GridDataItem item = e.Item as GridDataItem;

                string po_id = "";
                //string po_number = Controller.Clean(((TextBox)item.FindControl("po_numberTXT")).Text);
                //string official_number = Controller.Clean(((TextBox)item.FindControl("official_numberTXT")).Text);
                RadDatePicker add_po_dateRDP = ((RadDatePicker)item.FindControl("po_dateRDPTXT"));
                string item_id = ((RadComboBox)item.FindControl("item_idRCB")).SelectedValue;
                //string po_address = ((TextBox)item.FindControl("po_addressTXT")).Text;
                //string po_contactPerson = Controller.Clean(((TextBox)item.FindControl("po_contactPersonTXT")).Text);
                string po_date = add_po_dateRDP.SelectedDate.ToString();

                using (SqlConnection con = new SqlConnection(Controller.connection))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        Dictionary<string, string> fields = new Dictionary<string, string>();
                       // fields.Add("po_number", po_number);
                        //fields.Add("official_number", official_number);
                        fields.Add("item_id", item_id);
                        fields.Add("po_date", po_date);
                        //fields.Add("po_address", po_address);
                        fields.Add("user_name", Session["user_name"].ToString());
                        fields.Add("createdBy", Session["user_name"].ToString());

                        con.Open();
                        po_id = Controller.InsertInto(cmd, con, "CNRS_PurchaseOrder", fields, true);
                    }
                }
                RadGridBoxes.MasterTableView.IsItemInserted = false;

                if (po_id != "")
                    NavigateNextStep(item_id, po_id, 0);
            }


            if (e.CommandName == "Update")   //edit
            {
                if (e.Item is GridDataItem)
                {
                    string role_id = getRoleID();

                    GridDataItem item = e.Item as GridDataItem;

                    string po_id = item.GetDataKeyValue("po_id").ToString();
                    string po_number = Controller.Clean(((TextBox)item.FindControl("po_numberTXT")).Text);
                    string official_number = Controller.Clean(((TextBox)item.FindControl("official_numberTXT")).Text);
                    RadDatePicker po_dateRDP = ((RadDatePicker)item.FindControl("po_dateRDPTXT"));
                    //string po_contactPerson = Controller.Clean(((TextBox)item.FindControl("po_contactPersonTXT")).Text);
                    //string po_address = ((TextBox)item.FindControl("po_addressTXT")).Text;
                    string po_date = po_dateRDP.SelectedDate.ToString();

                    using (SqlConnection con = new SqlConnection(Controller.connection))
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            Dictionary<string, string> fields = new Dictionary<string, string>(); 

                            Dictionary<string, string> conditions = new Dictionary<string, string>();
                            conditions.Add("po_id", po_id);

                            con.Open();
                            if (role_id == "3")  //  Secretary
                            {
                                fields.Add("official_number", official_number);
                                //fields.Add("po_date_secretary", DateTime.Now.ToString());
                                Controller.Update(cmd, con, "CNRS_PurchaseOrder", fields, conditions);
                                return;
                            }

                            else if (role_id == "5")  //  Head of Department
                            {
                                fields.Add("po_number", po_number);
                                fields.Add("po_date_headOfDepartment", DateTime.Now.ToString());
                                Controller.Update(cmd, con, "CNRS_PurchaseOrder", fields, conditions);
                                return;
                            }

                            fields.Clear();
                            if (role_id == "1")
                            {
                                fields.Add("po_number", po_number);
                                fields.Add("official_number", official_number);
                            }

                            fields.Add("po_date", po_date);
                            fields.Add("user_name", Session["user_name"].ToString()); 
                            fields.Add("modifedBy", Session["user_name"].ToString());
                            fields.Add("modifiedOn", DateTime.Now.ToString());
                            //fields.Add("po_address", po_address);
                            //fields.Add("po_contactPerson", Session["user_name"].ToString());

                            Controller.Update(cmd, con, "CNRS_PurchaseOrder", fields, conditions);
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
                    Label item_idLBL = (Label)item.FindControl("item_idLBL");
                    string item_id = item_idLBL.Text;

                    int po_id = (int)item.GetDataKeyValue("po_id");
                    using (SqlConnection con = new SqlConnection(Controller.connection))
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            con.Open();
                            Controller.Inactivate(cmd, con, "CNRS_PurchaseOrder", "po_id", po_id.ToString(), Session["user_name"].ToString());

                            Dictionary<string, string> fields = new Dictionary<string, string>();
                            fields.Add("active", false.ToString());

                            Dictionary<string, string> conditions = new Dictionary<string, string>();
                            conditions.Add("po_id", po_id.ToString());
                             
                            if (item_id == "1") //TLD
                            {
                                Controller.Update(cmd, con, "CNRS_TLDDetails", fields, conditions);
                            }
                            else if (item_id == "2") //SSDL
                            {
                                Controller.Update(cmd, con, "CNRS_SSDLServiceFees", fields, conditions);
                            }
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

    //*** NavigateNextStep
    protected void NavigateNextStep(string item_id, string po_id, int i)
    {
        var encrypt = new eXSecurity.Encryption();

        if (item_id == "1")   //TLD
        {
            if (i == 0)
            { 
                using (SqlConnection con = new SqlConnection(Controller.connection))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        //select from CNRS_TLDHeaderDetails all tldh_id
                        Dictionary<string, string> conditions = new Dictionary<string, string>();
                        ArrayList field = new ArrayList();
                        field.Add("tldhd_id");
                        conditions.Add("active", true.ToString());
                        con.Open();
                        DataTable dt = Controller.SelectFrom(cmd, con, "CNRS_TLDHeaderDetails", field, conditions, new ArrayList(), false, false, "");

                        //insert into table TLDDetails  
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow row in dt.Rows)
                            {
                                Dictionary<string, string> fields = new Dictionary<string, string>();
                                fields.Add("tldhd_id", row["tldhd_id"].ToString());
                                fields.Add("po_id", po_id);
                                fields.Add("createdBy", Session["user_name"].ToString());
                                Controller.InsertInto(cmd, con, "CNRS_TLDDetails", fields, false);
                            }
                        }
                    }
                }
            }
            Redirect("~/Pages/TLD.aspx?P=" + encrypt.EncryptData(po_id));
        }

        else if (item_id == "2")    //SSDL
        {  
            Redirect("~/Pages/SSDL.aspx?P=" + encrypt.EncryptData(po_id));
        }

        else if (item_id == "3")    //Quality Control
        {
            Redirect("QualityOfControll.aspx");
        }

        else if (item_id == "4")
        {
            Redirect(".aspx");
        }
    }
    // *** 

    //*** Redirect
    protected void Redirect(string path)
    {
        Response.Redirect(path, false);
        Context.ApplicationInstance.CompleteRequest();
    }
    //***

    //***   Menu    
    protected void logoutBTN_Click(Object sender , System.EventArgs e)
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

    protected void Next_Click(object sender, EventArgs e)
    {
        try
        { 
            LinkButton LBTN = (LinkButton)sender;
            string po_id = LBTN.CommandArgument;
            Telerik.Web.UI.GridDataItem dataItem = (Telerik.Web.UI.GridDataItem)((Control)sender).NamingContainer;
            Label item_id = (Label)dataItem.FindControl("item_idLBL");

            NavigateNextStep(item_id.Text, po_id, 1);
        }
        catch (Exception ex)
        {
            Controller.SaveErrors(Path.GetFileName(Request.PhysicalPath), ex.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name);
        }
    }

    protected void Print_Click(object sender, EventArgs e)
    {
        try
        {
            RadButton LBTN = (RadButton)sender;
            string po_id = LBTN.CommandArgument;
            var encrypt = new eXSecurity.Encryption();
            Telerik.Web.UI.GridDataItem dataItem = (Telerik.Web.UI.GridDataItem)((Control)sender).NamingContainer;
            string item_id = ((Label)dataItem.FindControl("item_idLBL")).Text;
            if (item_id == "1") //TLD
            {
                Response.Write("<script>");
                Response.Write("window.open('Reports/TLDReport.aspx?P=" + encrypt.EncryptData(po_id) + "')");
                Response.Write("</script>");
            }
            else if (item_id == "2") //SSDL
            {
                Response.Write("<script>");
                Response.Write("window.open('Reports/SSDLReport.aspx?P=" + encrypt.EncryptData(po_id) + "')");
                Response.Write("</script>");
            }
        }
        catch (Exception ex)
        {
            Controller.SaveErrors(Path.GetFileName(Request.PhysicalPath), ex.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name);
        }
    }

    protected DateTime? SetPoDate(GridItem item)
    {
        if (item.OwnerTableView.GetColumn("po_date").CurrentFilterValue == string.Empty)
        {
            return new DateTime?();
        }
        else
        {
            return DateTime.Parse(item.OwnerTableView.GetColumn("po_date").CurrentFilterValue);
        }
    } 

    protected void po_accept_secretaryCB_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox cb = (CheckBox)sender;
        Telerik.Web.UI.GridDataItem item = (Telerik.Web.UI.GridDataItem)((Control)sender).NamingContainer;

        string po_id = item.GetDataKeyValue("po_id").ToString();

        try
        {
            using (SqlConnection con = new SqlConnection(Controller.connection))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    con.Open();

                    Dictionary<string, string> fields = new Dictionary<string, string>();
                    fields.Add("po_date_secretary", DateTime.Now.ToString());

                    Dictionary<string, string> conditions = new Dictionary<string, string>();
                    conditions.Add("po_id", po_id);

                    if (cb.Checked == true)
                    {
                        fields.Add("po_accept_secretary", true.ToString());
                        fields.Add("po_isnewforSectretary", false.ToString());
                        Controller.Update(cmd, con, "CNRS_PurchaseOrder", fields, conditions);
                    }
                    else
                    {
                        fields.Add("po_accept_secretary", false.ToString());
                        Controller.Update(cmd, con, "CNRS_PurchaseOrder", fields, conditions);
                    }
                    AlertJS();
                    RadGridBoxes.Rebind();
                }
            }
        }
        catch (Exception ex)
        {
            Controller.SaveErrors(Path.GetFileName(Request.PhysicalPath), ex.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name);
        }
    }
    protected void po_accept_directorCB_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox cb = (CheckBox)sender;
        Telerik.Web.UI.GridDataItem item = (Telerik.Web.UI.GridDataItem)((Control)sender).NamingContainer;

        string po_id = item.GetDataKeyValue("po_id").ToString();

        try
        {
            using (SqlConnection con = new SqlConnection(Controller.connection))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    con.Open();

                    Dictionary<string, string> fields = new Dictionary<string, string>();
                    fields.Add("po_date_director", DateTime.Now.ToString());

                    Dictionary<string, string> conditions = new Dictionary<string, string>();
                    conditions.Add("po_id", po_id);

                    if (cb.Checked == true)
                    {
                        fields.Add("po_accept_director", true.ToString());
                        fields.Add("po_isnewforDirector", false.ToString());
                        Controller.Update(cmd, con, "CNRS_PurchaseOrder", fields, conditions);
                    }
                    else
                    {
                        fields.Add("po_accept_director", false.ToString());
                        Controller.Update(cmd, con, "CNRS_PurchaseOrder", fields, conditions);
                    }
                    AlertJS();
                    RadGridBoxes.Rebind();
                }
            } 
        }
        catch (Exception ex)
        {
            Controller.SaveErrors(Path.GetFileName(Request.PhysicalPath), ex.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name);
        }
    }

    protected void po_accept_headOfDepartmentCB_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox cb = (CheckBox)sender;
        Telerik.Web.UI.GridDataItem item = (Telerik.Web.UI.GridDataItem)((Control)sender).NamingContainer;

        string po_id = item.GetDataKeyValue("po_id").ToString();

        try
        {
            using (SqlConnection con = new SqlConnection(Controller.connection))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    con.Open();

                    Dictionary<string, string> fields = new Dictionary<string, string>();
                    fields.Add("po_date_headOfDepartment", DateTime.Now.ToString());

                    Dictionary<string, string> conditions = new Dictionary<string, string>();
                    conditions.Add("po_id", po_id);

                    if (cb.Checked == true)
                    {
                        fields.Add("po_accept_headOfDepartment", true.ToString());
                        fields.Add("po_isnewforHOD", false.ToString());
                        Controller.Update(cmd, con, "CNRS_PurchaseOrder", fields, conditions);
                    }
                    else
                    {
                        fields.Add("po_accept_headOfDepartment", false.ToString());
                        Controller.Update(cmd, con, "CNRS_PurchaseOrder", fields, conditions);
                    }
                    AlertJS();
                    RadGridBoxes.Rebind();
                }
            }
        }
        catch (Exception ex)
        {
            Controller.SaveErrors(Path.GetFileName(Request.PhysicalPath), ex.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name);
        }
    } 

    protected void po_date_end_technicalOfficerRCB_Click(object sender, EventArgs e)
    {
        try
        { 
            CheckBox rcb = (CheckBox)sender;
            Telerik.Web.UI.GridDataItem item = (Telerik.Web.UI.GridDataItem)((Control)sender).NamingContainer;

            string po_id = item.GetDataKeyValue("po_id").ToString();
            string user_email = ((Label)item.FindControl("user_emailLBL")).Text;
            string user_contactPerson = ((Label)item.FindControl("user_contactPersonLBL")).Text;


            Dictionary<string, string> fields = new Dictionary<string, string>();
            Dictionary<string, string> conditions = new Dictionary<string, string>();
            conditions.Add("po_id", po_id);

            if (rcb.Checked == true)
            {
                fields.Add("po_date_end_technicalOfficer", DateTime.Now.ToString());
                fields.Add("po_isnewforTechmical", false.ToString());
            }
            else
                fields.Add("po_date_end_technicalOfficer", DBNull.Value.ToString());

            bool sendedEmail = false;
            if (rcb.Checked == true)
                sendedEmail = SendEmail(user_email, user_contactPerson, 1);
            else
                sendedEmail = SendEmail(user_email, user_contactPerson, 2);

            using (SqlConnection con = new SqlConnection(Controller.connection))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    con.Open();
                    if (sendedEmail == true)
                    {
                        Controller.Update(cmd, con, "CNRS_PurchaseOrder", fields, conditions);
                        AlertJS();
                        RadGridBoxes.Rebind();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Controller.SaveErrors(Path.GetFileName(Request.PhysicalPath), ex.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name);
        }
    }

    //*** SendEmail   
    protected bool SendEmail(string toEmail, string toUser_name, int index)
    {
        string subject = "";
        string body = "";
         
        subject = "Request Completed";
        if (index == 1)
            body = "Hello " + toUser_name + ", " + Environment.NewLine + Messages.endRequest;
        else
            body = "Hello " + toUser_name + ", " + Environment.NewLine + Messages.sorry;

        return Controller.SendEmail(toEmail, toUser_name, subject, body);
    }

    public DateTime ProcessMyDataItem(object myValue)
    {
        string str = myValue.ToString();
        if (str == "")
        {
            return default(DateTime);
        }

        return (DateTime)myValue;
    }

    protected bool CheckIfPoIsEnded(string date)
    {
        if (date != DBNull.Value.ToString() && date != "1/1/1900 12:00:00 AM")
        {
            return true;
        }
        return false;
    }
    protected void refresh_Click(object sender, EventArgs e)
    {
        Redirect(Request.RawUrl);
    } 
    protected void FollowUp_Click(object sender, EventArgs e)
    {
        RadButton LBTN = (RadButton)sender;
        string po_id = LBTN.CommandArgument;
        var encrypt = new eXSecurity.Encryption();
         
        Redirect("OrderLine.aspx?P=" + encrypt.EncryptData(po_id));
    }

    protected bool CheckAccepted(string nextAccept)
    {
        if (nextAccept == "True")
        {
            return false;
        }
        return true;
    }
}