using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class MasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            CheckSession();

            using (SqlConnection con = new SqlConnection(Controller.connection))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    con.Open();


                    if (!IsPostBack)
                    {
                        Translate(cmd, con);

                        //langRCB.DataSource = Controller.SelectFrom(cmd, con, "SET_Language", new ArrayList(), new Dictionary<string, string>(), new ArrayList(), true, false, "");
                        //langRCB.DataBind();

                        //if (Session["language_id"].ToString() != "")
                        //{
                        //    langRCB.SelectedValue = Session["language_id"].ToString();
                        //}

                        DataTable dtSec = Controller.Security(cmd, con, Path.GetFileName(Request.PhysicalPath), Session["user_name"].ToString());

                        Security(dtSec);

                        fillData(con, cmd);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Controller.SaveErrors(Path.GetFileName(Request.PhysicalPath), ex.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name);
        }
    }
    protected void navgHome_Click(object sender, ImageClickEventArgs e)
    {
        Redirect("~/Pages/Home/Home.aspx");
    }

    protected void fillData(SqlConnection con, SqlCommand cmd)
    {
        user_name.InnerText = Session["user_name"].ToString().ToUpper();

        Dictionary<string, string> conditions = new Dictionary<string, string>();
        conditions.Add("role_id", "2");  //client
        conditions.Add("user_isnew", true.ToString());
        DataTable dt = Controller.ExtraSelect(con, cmd, "select Count(user_name) as count_new_user_name from View_User", conditions, "");
        if (dt.Rows[0]["count_new_user_name"].ToString() != "0")
        {
            new_user_name.Visible = true;
            user_accepted.Visible = true;
            new_user_name.Text = dt.Rows[0]["count_new_user_name"].ToString();
            user_accepted.Text = dt.Rows[0]["count_new_user_name"].ToString();
        }
        else
        {
            user_accepted.Visible = false;
            new_user_name.Visible = false;
        }

        conditions.Clear();
        conditions.Add("role_id", "2");  //client
        conditions.Add("active", false.ToString());

        //DataTable dt1 = Controller.ExtraSelect(con, cmd, "select Count(user_name) as count_user_name_not_accepted from View_User", conditions, "");
        //user_accepted.Text = dt1.Rows[0]["count_user_name_not_accepted"].ToString();
    }

    //*** Redirect
    protected void Redirect(string path)
    {
        Response.Redirect(path, false);
        Context.ApplicationInstance.CompleteRequest();
    }
    //***


    //*** CheckSession
    protected void CheckSession()
    {
        if (string.IsNullOrEmpty(Session["user_name"] as string))
        {
            Redirect("~/Login/Login.aspx");
        }
    }
    //*** 

    //***   Menu    
    protected void logoutBTN_Click(Object sender, System.EventArgs e)
    {
        Session["user_name"] = "";
        Session["language_id"] = "";
        //Redirect("~/Login/Login.aspx");
        Response.Write("<script>");
        Response.Write("window.open('../Login/Login.aspx','_self')");
        Response.Write("</script>");
    }

    protected void navgPoBTN_Click(object sender, ImageClickEventArgs e)
    {
        Redirect("PurchaseOrder.aspx");
    }
    //***

    protected void langRCB_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        //Session["language_id"] = langRCB.SelectedValue;
        try
        {
            using (SqlConnection con = new SqlConnection(Controller.connection))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    Translate(cmd, con);
                    //RadGridBoxes.Rebind();
                }
            }
        }
        catch (Exception ex)
        {
            Controller.SaveErrors(Path.GetFileName(Request.PhysicalPath), ex.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name);
        }
    }


    //*** Translate
    protected void Translate(SqlCommand cmd, SqlConnection con)
    {
        ArrayList componentID = new ArrayList();
        componentID.Add("po_number");
        componentID.Add("po_id");
        //Controller.Translate(cmd, con, RadGridBoxes, Session["language_id"].ToString(), Path.GetFileName(Request.PhysicalPath), componentID);
    }
    //***

    //*** Security
    protected void Security(DataTable dt)
    {
        if (dt.Rows.Count > 0)
        {
            can_acceptUser.Visible = Controller.Can(dt, "can_acceptUser");
            show_new_user.Visible = Controller.Can(dt, "can_acceptUser");
            can_fillTld.Visible = Controller.Can(dt, "can_fillTldHeader");
        }
    }
    //***
}