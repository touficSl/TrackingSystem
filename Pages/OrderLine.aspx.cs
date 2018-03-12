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

public partial class Pages_OrderLine : System.Web.UI.Page
{
    protected void Page_Load(object sender, System.EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            { 
                using (SqlConnection con = new SqlConnection(Controller.connection))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        con.Open();
                        Dictionary<String, String> conditions = new Dictionary<string, string>();
                        conditions.Add("po_id", getPoID());
                        DataTable dt = Controller.ExtraSelect(con, cmd, "select * from View_PurchaseOrder", conditions, "");
                        string secretary = "";
                        string director = "";
                        string headOD = "";
                        string technical = "";

                        if (dt.Rows[0]["po_date_secretary"].ToString() != "")
                            secretary = ((DateTime)dt.Rows[0]["po_date_secretary"]).ToShortDateString();
                        if (dt.Rows[0]["po_date_director"].ToString() != "")
                            director = ((DateTime)dt.Rows[0]["po_date_director"]).ToShortDateString();
                        if (dt.Rows[0]["po_date_headOfDepartment"].ToString() != "")
                            headOD = ((DateTime)dt.Rows[0]["po_date_headOfDepartment"]).ToShortDateString();
                        if (dt.Rows[0]["po_date_end_technicalOfficer"].ToString() != "")
                            technical = ((DateTime)dt.Rows[0]["po_date_end_technicalOfficer"]).ToShortDateString();

                        if (secretary != "")
                        {
                            Span1.InnerText = secretary;
                            bar1.Style.Add("background", "#4F52BA");
                        }

                        if (director != "")
                        {
                            Span2.InnerText = director;
                            bar1.Style.Add("background", "#4F52BA");
                            bar2.Style.Add("background", "#4F52BA");
                        }

                        if (headOD != "")
                        {
                            Span3.InnerText = headOD;
                            bar1.Style.Add("background", "#4F52BA");
                            bar2.Style.Add("background", "#4F52BA");
                            bar3.Style.Add("background", "#4F52BA");
                        } 

                        if (technical != "")
                        {
                            Span4.InnerText = technical;
                            bar1.Style.Add("background", "#4F52BA");
                            bar2.Style.Add("background", "#4F52BA");
                            bar3.Style.Add("background", "#4F52BA");
                            bar4.Style.Add("background", "#4F52BA");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Controller.SaveErrors(Path.GetFileName(Request.PhysicalPath), ex.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            }
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

