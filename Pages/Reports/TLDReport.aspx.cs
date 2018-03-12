using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_Reports_TLDReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            CheckSession();

            var instanceReportSource = new Telerik.Reporting.InstanceReportSource();
            instanceReportSource.ReportDocument = new ReportWebAppCNRS.TLDReport();
            instanceReportSource.Parameters.Add("poid", getPoID());
            ReportViewer1.ReportSource = instanceReportSource;
            ReportViewer1.RefreshReport();
        }
        catch (Exception ex)
        {
            Controller.SaveErrors(Path.GetFileName(Request.PhysicalPath), ex.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name);
        }
    }

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

    //*** Redirect
    protected void Redirect(string path)
    {
        Response.Redirect(path, false);
        Context.ApplicationInstance.CompleteRequest();
    }
    //***

    protected void Next_Click(object sender, EventArgs e)
    {
        Redirect("TLDReagentReport.aspx?P=" + Request["P"].ToString());
    }
}