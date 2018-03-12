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



public partial class Pages_ForgotPassword : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            fillData();
        }
    }

    public void fillData()
    {
        DivChangePassword.Visible = false;

        try
        {
            using (SqlConnection con = new SqlConnection(Controller.connection))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    Dictionary<String, String> conditions = new Dictionary<string, string>();
                    conditions.Add("user_name", Session["user_name"].ToString());
                    DataTable dt = Controller.ExtraSelect(con, cmd, "select * from SEC_User", conditions, "");

                    string usernameTXT = dt.Rows[0]["user_name"].ToString();
                    string companynameTXT = dt.Rows[0]["user_companyName"].ToString();
                    string user_emailTXT = dt.Rows[0]["user_email"].ToString();
                    string addressTXT = dt.Rows[0]["user_address"].ToString();
                    string faxTXT = dt.Rows[0]["user_fax"].ToString();
                    string phoneTXT = dt.Rows[0]["user_phone"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            Controller.SaveErrors(Path.GetFileName(Request.PhysicalPath), ex.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name);
        }
    } 

    protected void AlertJS(int index)
    {
        string fct = "";
        if (index == 1)
            fct = "successalert();";
        else
            fct = "erroralert()"; 
        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", fct, true);
    }  

    protected void BTN2_Click(object sender, EventArgs e)
    {
        DivChangePassword.Visible = true;
    }

    protected void BTNSave_ServerClick(object sender, EventArgs e)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(Controller.connection))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    //Dictionary<string, string> fields = new Dictionary<string, string>();
                    //fields.Add("user_reason", user_reason);
                    //fields.Add("active", active.ToString());
                    //fields.Add("modifedBy", Session["user_name"].ToString());
                    //fields.Add("modifiedOn", DateTime.Now.ToString());

                    //Dictionary<string, string> conditions = new Dictionary<string, string>();
                    //conditions.Add("user_name", Session["user_name"].toString());

                    //con.Open();
                    //Controller.Update(cmd, con, "SEC_User", fields, conditions);
                }
            }
        }
        catch (Exception ex)
        {
            //Controller.SaveErrors(Path.GetFileName(Request.PhysicalPath), ex.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name);
        }
    }
}
