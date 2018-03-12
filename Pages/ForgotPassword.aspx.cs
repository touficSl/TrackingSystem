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


public partial class Pages_ForgotPassword : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
       
    }

    protected void SendEmail(object sender, EventArgs e)
    {
        var decrypt = new eXSecurity.Decryption();
        string username = string.Empty;
        string password = string.Empty;
        DataTable dt = new DataTable();
        Dictionary<string, string> conditions = new Dictionary<string, string>();
        conditions.Add("user_email", user_emailTXT.Text.Trim());
        conditions.Add("user_name", usernameTXT.Value);

        using (SqlConnection con = new SqlConnection(Controller.connection))
        {
            using (SqlCommand cmd = new SqlCommand())
            {
               con.Open();
               dt = Controller.ExtraSelect(con, cmd, "SELECT user_contactPerson, user_password FROM SEC_User", conditions, "");
            }
        }

        if (dt.Rows.Count > 0)
        {
            SendEmailToUser(decrypt.DecryptData(dt.Rows[0]["user_password"].ToString()), dt.Rows[0]["user_contactPerson"].ToString());

            AlertJS(1);

            ClearTXT();
        }
        else
        {
            AlertJS(2);
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

    protected void SendEmailToUser(string user_password, string user_contactPerson)
    {
        string body = "Dear " + user_contactPerson + ", " + Environment.NewLine + Messages.forgotPass + ": " + user_password;
        string subject = "Forgot Password";
        Controller.SendEmail(user_emailTXT.Text.Trim(), usernameTXT.Value, subject, body);
    }

    protected void ClearTXT()
    {
        user_emailTXT.Text = "";
        usernameTXT.Value = "";
    }
}
