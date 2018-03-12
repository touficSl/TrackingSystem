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

public partial class Pages_SSDL : System.Web.UI.Page
{ 
    //*** Page_Load
    protected void Page_Load(object sender, EventArgs e)
    { 
       // Page.Header.DataBind();
        try
        {
            if (string.IsNullOrEmpty(Session["user_name"] as string))
            {
                Redirect("../Login/Login.aspx");
            }

            using (SqlConnection con = new SqlConnection(Controller.connection))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    con.Open();
                    

                    if (!IsPostBack)
                    {
                        Translate(cmd, con);

                        langRCB.DataSource  = Controller.SelectFrom(cmd, con, "SET_Language", new ArrayList(), new Dictionary<string, string>(), new ArrayList(), true, false, "");
                        langRCB.DataBind();

                        DataTable dtSec = Controller.Security(cmd, con, Path.GetFileName(Request.PhysicalPath), Session["user_name"].ToString());

                        Security(dtSec);

                        if (Session["language_id"].ToString() != "")
                        {
                            langRCB.SelectedValue = Session["language_id"].ToString();
                        }

                        CheckIfSended(cmd, con);

                        fillData();
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

    protected void fillData()
    {
        try
        {

            string po_id = getPoID();

            using (SqlConnection con = new SqlConnection(Controller.connection))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    con.Open();
                    ArrayList field = new ArrayList();
                    field.Add("feesPerInstrument");
                    Dictionary<String, String> conditions = new Dictionary<string, string>();
                    conditions.Add("id", "1");

                    DataTable dt = Controller.SelectFrom(cmd, con, "SET_Settings", field, conditions, new ArrayList(), false, false, "");
                    totalCostTXT.Text = dt.Rows[0]["feesPerInstrument"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            Controller.SaveErrors(Path.GetFileName(Request.PhysicalPath), ex.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name);
        }
    }

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


    //*** CheckIfSended
    protected void CheckIfSended(SqlCommand cmd, SqlConnection con)
    { 
        string po_id = getPoID();

        ArrayList fields = new ArrayList();
        fields.Add("po_sendIt");

        Dictionary<string, string> conditions = new Dictionary<string, string>();
        conditions.Add("po_id", po_id);  

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
            //RadGridBoxes2.MasterTableView.GetColumn("EditCommandColumn").Display = true;
            //RadGridBoxes2.MasterTableView.GetColumn("delete").Display = true;
            RadGridBoxes.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.Top;
            RadGridBoxes.Rebind();
            //RadGridBoxes2.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.Top;
            //RadGridBoxes2.Rebind();
        }
    }
    //***

    //*** RadGridBoxes_NeedDataSource1
    protected void RadGridBoxes_NeedDataSource1(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {

            string po_id = getPoID();

            using (SqlConnection con = new SqlConnection(Controller.connection))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    con.Open();
                    Dictionary<String, String> conditions = new Dictionary<string, string>();
                    conditions.Add("active", true.ToString());
                    conditions.Add("po_id", po_id);

                    Controller.FillRadGrid(cmd, con, sender, "CNRS_SSDLServiceFees", new ArrayList(), conditions, true, "");
                }
            }
        }
        catch (Exception ex)
        {
            Controller.SaveErrors(Path.GetFileName(Request.PhysicalPath), ex.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name);
        }
    } 
    //***

    //*** RadGridBoxes_ItemCreated
    //protected void RadGridBoxes_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
    //{
    //    try
    //    {
    //        if (e.Item is GridEditableItem && e.Item.IsInEditMode)
    //        {
    //            GridEditableItem item = (GridEditableItem)e.Item;
    //            TextBox ssdlsf_ManufacturerTXT = (TextBox)item.FindControl("ssdlsf_ManufacturerTXT");
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        Controller.SaveErrors(Path.GetFileName(Request.PhysicalPath), ex.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name);
    //    }
    //} 
    //***

    //*** RadGridBoxes_ItemCommand
    protected void RadGridBoxes_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "PerformInsert")   // add
            { 
                string po_id = getPoID();

                GridDataItem item = e.Item as GridDataItem;

                //string ssdlsf_totalCost = Controller.Clean(((TextBox)item.FindControl("ssdlsf_totalCostTXT")).Text);
                string ssdlsf_radiationQuality = ((TextBox)item.FindControl("ssdlsf_radiationQualityTXT")).Text;
                string ssdlsf_serialNbr = ((TextBox)item.FindControl("ssdlsf_serialNbrTXT")).Text;
                string ssdlsf_modelNbr = ((TextBox)item.FindControl("ssdlsf_modelNbrTXT")).Text;
                string ssdlsf_Manufacturer = ((TextBox)item.FindControl("ssdlsf_ManufacturerTXT")).Text;
                string ssdlsf_Date = ((TextBox)item.FindControl("ssdlsf_DateTXT")).Text;
                string ssdlsf_typeOfInstrument = Controller.Clean(((TextBox)item.FindControl("ssdlsf_typeOfInstrumentTXT")).Text);

                using (SqlConnection con = new SqlConnection(Controller.connection))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        Dictionary<string, string> fields = new Dictionary<string, string>();
                        fields.Add("po_id", po_id);
                        //fields.Add("ssdlsf_totalCost", ssdlsf_totalCost);
                        fields.Add("ssdlsf_radiationQuality", ssdlsf_radiationQuality);
                        fields.Add("ssdlsf_serialNbr", ssdlsf_serialNbr);
                        fields.Add("ssdlsf_modelNbr", ssdlsf_modelNbr);
                        fields.Add("ssdlsf_Manufacturer", ssdlsf_Manufacturer);
                        fields.Add("ssdlsf_Date", ssdlsf_Date);
                        fields.Add("ssdlsf_typeOfInstrument", ssdlsf_typeOfInstrument);
                        fields.Add("createdBy", Session["user_name"].ToString());

                        con.Open();
                        Controller.InsertInto(cmd, con, "CNRS_SSDLServiceFees", fields, false);
                        AlertJS(1);
                    }
                }
            }


            if (e.CommandName == "Update")   //edit
            {
                if (e.Item is GridDataItem)
                {
                    GridDataItem item = e.Item as GridDataItem;

                    int ssdlsf_id = (int)item.GetDataKeyValue("ssdlsf_id");
                    //string ssdlsf_totalCost = ((TextBox)item.FindControl("ssdlsf_totalCostTXT")).Text;
                    string ssdlsf_radiationQuality = ((TextBox)item.FindControl("ssdlsf_radiationQualityTXT")).Text;
                    string ssdlsf_serialNbr = ((TextBox)item.FindControl("ssdlsf_serialNbrTXT")).Text;
                    string ssdlsf_modelNbr = ((TextBox)item.FindControl("ssdlsf_modelNbrTXT")).Text;
                    string ssdlsf_Manufacturer = ((TextBox)item.FindControl("ssdlsf_ManufacturerTXT")).Text;
                    string ssdlsf_Date = ((TextBox)item.FindControl("ssdlsf_DateTXT")).Text;
                    string ssdlsf_typeOfInstrument = ((TextBox)item.FindControl("ssdlsf_typeOfInstrumentTXT")).Text;

                    using (SqlConnection con = new SqlConnection(Controller.connection))
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            Dictionary<string, string> fields = new Dictionary<string, string>();
                            //fields.Add("ssdlsf_totalCost", ssdlsf_totalCost);
                            fields.Add("ssdlsf_radiationQuality", ssdlsf_radiationQuality);
                            fields.Add("ssdlsf_serialNbr", ssdlsf_serialNbr);
                            fields.Add("ssdlsf_modelNbr", ssdlsf_modelNbr);
                            fields.Add("ssdlsf_Manufacturer", ssdlsf_Manufacturer);
                            fields.Add("ssdlsf_Date", ssdlsf_Date);
                            fields.Add("ssdlsf_typeOfInstrument", ssdlsf_typeOfInstrument);
                            fields.Add("modifedBy", Session["user_name"].ToString());
                            fields.Add("modifiedOn", DateTime.Now.ToString());

                            Dictionary<string, string> conditions = new Dictionary<string, string>();
                            conditions.Add("ssdlsf_id", ssdlsf_id.ToString()); 

                            con.Open();
                            Controller.Update(cmd, con, "CNRS_SSDLServiceFees", fields, conditions);
                            AlertJS(1);
                        }
                    }
                }
            }


            else if (e.CommandName == "Delete")  // inactivate
            {
                if (e.Item is GridDataItem)
                {
                    GridDataItem item = e.Item as GridDataItem;

                    int ssdlsf_id = (int)item.GetDataKeyValue("ssdlsf_id");
                    using (SqlConnection con = new SqlConnection(Controller.connection))
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            con.Open();
                            Controller.Inactivate(cmd, con, "CNRS_SSDLServiceFees", "ssdlsf_id", ssdlsf_id.ToString(), Session["user_name"].ToString());
                            AlertJS(1);
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
    protected void RadGridBoxes_NeedDataSource2(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {

            string po_id = getPoID();

            using (SqlConnection con = new SqlConnection(Controller.connection))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    con.Open();
                    Dictionary<String, String> conditions = new Dictionary<string, string>();
                    conditions.Add("active", true.ToString());
                    conditions.Add("po_id", po_id);

                    Controller.FillRadGrid(cmd, con, sender, "CNRS_SSDLLabReceived", new ArrayList(), conditions, true, "");
                }
            }
        }
        catch (Exception ex)
        {
            Controller.SaveErrors(Path.GetFileName(Request.PhysicalPath), ex.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name);
        }
    }
    //***

    ////*** RadGridBoxes_ItemCreated
    //protected void RadGridBoxes_ItemCreated2(object sender, Telerik.Web.UI.GridItemEventArgs e)
    //{
    //    try
    //    {
    //        if (e.Item is GridEditableItem && e.Item.IsInEditMode)
    //        {
    //            GridEditableItem item = (GridEditableItem)e.Item;
    //            RadDatePicker ssdlsf_dateManufacturerRDP = (RadDatePicker)item.FindControl("ssdllr_dateRDP");
    //            ssdlsf_dateManufacturerRDP.SelectedDate = DateTime.Now;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        Controller.SaveErrors(Path.GetFileName(Request.PhysicalPath), ex.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name);
    //    }
    //}
    ////***

    ////*** RadGridBoxes_ItemCommand
    //protected void RadGridBoxes_ItemCommand2(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    //{
    //    try
    //    {
    //        if (e.CommandName == "PerformInsert")   // add
    //        {

    //            string po_id = getPoID();

    //            GridDataItem item = e.Item as GridDataItem;

    //            RadDatePicker ssdllr_dateRDP = ((RadDatePicker)item.FindControl("ssdllr_dateRDP"));
    //            string ssdllr_nameReceiver = Controller.Clean(((TextBox)item.FindControl("ssdllr_nameReceiverTXT")).Text);
    //            string ssdllr_clientName = ((TextBox)item.FindControl("ssdllr_clientNameTXT")).Text;
    //            string ssdllr_functionalStatus = ((TextBox)item.FindControl("ssdllr_functionalStatusTXT")).Text;

    //            DateTime ssdllr_date = (DateTime)ssdllr_dateRDP.SelectedDate;

    //            using (SqlConnection con = new SqlConnection(Controller.connection))
    //            {
    //                using (SqlCommand cmd = new SqlCommand())
    //                {
    //                    Dictionary<string, string> fields = new Dictionary<string, string>();
    //                    fields.Add("po_id", po_id);
    //                    fields.Add("ssdllr_date", ssdllr_date.ToString());
    //                    fields.Add("ssdllr_nameReceiver", ssdllr_nameReceiver);
    //                    fields.Add("ssdllr_clientName", ssdllr_clientName);
    //                    fields.Add("ssdllr_functionalStatus", ssdllr_functionalStatus);
    //                    fields.Add("createdBy", Session["user_name"].ToString());

    //                    con.Open();
    //                    Controller.InsertInto(cmd, con, "CNRS_SSDLLabReceived", fields, false);
    //                    AlertJS(1);
    //                }
    //            }
    //        }


    //        if (e.CommandName == "Update")   //edit
    //        {
    //            if (e.Item is GridDataItem)
    //            {
    //                GridDataItem item = e.Item as GridDataItem;

    //                int ssdllr_id = (int)item.GetDataKeyValue("ssdllr_id");
    //                RadDatePicker ssdllr_dateRDP = ((RadDatePicker)item.FindControl("ssdllr_dateRDP"));
    //                string ssdllr_nameReceiver = Controller.Clean(((TextBox)item.FindControl("ssdllr_nameReceiverTXT")).Text);
    //                string ssdllr_clientName = ((TextBox)item.FindControl("ssdllr_clientNameTXT")).Text;
    //                string ssdllr_functionalStatus = ((TextBox)item.FindControl("ssdllr_functionalStatusTXT")).Text;

    //                DateTime ssdllr_date = (DateTime)ssdllr_dateRDP.SelectedDate;

    //                using (SqlConnection con = new SqlConnection(Controller.connection))
    //                {
    //                    using (SqlCommand cmd = new SqlCommand())
    //                    {
    //                        Dictionary<string, string> fields = new Dictionary<string, string>();
    //                        fields.Add("ssdllr_date", ssdllr_date.ToString());
    //                        fields.Add("ssdllr_nameReceiver", ssdllr_nameReceiver);
    //                        fields.Add("ssdllr_clientName", ssdllr_clientName);
    //                        fields.Add("ssdllr_functionalStatus", ssdllr_functionalStatus);
    //                        fields.Add("modifedBy", Session["user_name"].ToString());
    //                        fields.Add("modifiedOn", DateTime.Now.ToString());

    //                        Dictionary<string, string> conditions = new Dictionary<string, string>();
    //                        conditions.Add("ssdllr_id", ssdllr_id.ToString());

    //                        con.Open();
    //                        Controller.Update(cmd, con, "CNRS_SSDLLabReceived", fields, conditions);
    //                        AlertJS(1);
    //                    }
    //                }
    //            }
    //        }


    //        else if (e.CommandName == "Delete")  // inactivate
    //        {
    //            if (e.Item is GridDataItem)
    //            {
    //                GridDataItem item = e.Item as GridDataItem;

    //                int ssdllr_id = (int)item.GetDataKeyValue("ssdllr_id");
    //                using (SqlConnection con = new SqlConnection(Controller.connection))
    //                {
    //                    using (SqlCommand cmd = new SqlCommand())
    //                    {
    //                        con.Open();
    //                        Controller.Inactivate(cmd, con, "CNRS_SSDLLabReceived", "ssdllr_id", ssdllr_id.ToString(), Session["user_name"].ToString());
    //                        AlertJS(1);
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        Controller.SaveErrors(Path.GetFileName(Request.PhysicalPath), ex.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name);
    //    }
    //}
    //***  

        //*** send
    protected void SendBTN_Click(object sender, EventArgs e)
    {
        try
        { 
            string po_id = getPoID();

            if (RadGridBoxes.MasterTableView.Items.Count > 0 )//&& RadGridBoxes2.MasterTableView.Items.Count > 0)
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

    //*** hideAdd
    protected void hideAdd()
    {
        SendBTN.Visible = false; 
        RadGridBoxes.MasterTableView.GetColumn("EditCommandColumn").Display = false;
        RadGridBoxes.MasterTableView.GetColumn("delete").Display = false;
        //RadGridBoxes2.MasterTableView.GetColumn("EditCommandColumn").Display = false;
        //RadGridBoxes2.MasterTableView.GetColumn("delete").Display = false;
        RadGridBoxes.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.None;
        RadGridBoxes.Rebind();
        //RadGridBoxes2.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.None;
        //RadGridBoxes2.Rebind();
    }
    //****

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
        }
    }
    //***

    //*** Translate
    protected void Translate(SqlCommand cmd, SqlConnection con)
    {
        ArrayList componentID = new ArrayList();
        componentID.Add("ssdlsf_typeOfInstrument");
        Controller.Translate(cmd, con, RadGridBoxes, Session["language_id"].ToString(), Path.GetFileName(Request.PhysicalPath), componentID);
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


    //*** Redirect
    protected void Redirect(string path)
    {
        Response.Redirect(path, false);
        Context.ApplicationInstance.CompleteRequest();
    }
    //***

    protected void previous_Click(object sender, EventArgs e)
    {
        Redirect("PurchaseOrder.aspx");
    }
}