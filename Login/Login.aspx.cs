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

public partial class login_Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    { 
        if (!IsPostBack)
        {
            Session["user_name"] = "";
        }
    } 

    protected void LogIn(object sender, EventArgs e)
    {
        try
        { 
            var encrypt = new eXSecurity.Encryption();
            using (SqlConnection con = new SqlConnection(Controller.connection))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    con.Open();

                    ArrayList fields = new ArrayList();
                    fields.Add("user_name"); 
                    fields.Add("active");
                    fields.Add("role_id");
                    Dictionary<string, string> conditions = new Dictionary<string, string>();
                    conditions.Add("user_name", login_user_nameTXT.Text);
                    //conditions.Add("user_name", login_user_nameTXT.Value);
                    conditions.Add("user_password", encrypt.EncryptData(login_user_passwordTXT.Value)); 
                    DataTable dt = Controller.SelectFrom(cmd, con, "View_User", fields, conditions, new ArrayList(), false, false, "");
                    if (dt.Rows.Count > 0)
                    {
                        if ((bool) dt.Rows[0]["active"] == true)
                        {
                            fillSessions(cmd, con, dt.Rows[0]["role_id"].ToString());

                            Redirect("../Pages/Home.aspx");
                        }
                        else
                        {
                            AlertJS(3);
                        }
                    }
                    else
                    { 
                        AlertJS(2);
                    }
                    ClearTXT();
                }
            }
        }
        catch (Exception ex)
        {
            Controller.SaveErrors(Path.GetFileName(Request.PhysicalPath), ex.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name);
        }
    }


    protected void Register(object sender, EventArgs e)
    {  
        try
        {
            if (Page.IsValid == false) //Will be true if captcha text is correct otherwise it will be false
            {
                ClearTXT();
                AlertJS(4);
                return;
            }
            using (SqlConnection con = new SqlConnection(Controller.connection))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    con.Open();
                     
                    Dictionary<string, string> conditions = new Dictionary<string, string>();
                    conditions.Add("user_name", register_user_nameTXT.Value);
                    DataTable dt = Controller.SelectFrom(cmd, con, "SEC_User", new ArrayList(), conditions, new ArrayList(), true, false, "");
                    if (dt.Rows.Count > 0)
                    {
                        ClearTXT();
                        AlertJS(1);
                        return;
                    }

                    DataTable dtSecreatire = Controller.SelectSecretaireRole(cmd, con); // and admin
                    if (dtSecreatire.Rows.Count > 0)
                    {
                        bool Allsended = true;
                        foreach (DataRow row in dtSecreatire.Rows)
                        {
                            bool sended = SendEmail(row["user_email"].ToString(), row["user_contactPerson"].ToString(), 2);
                            if (sended == false)
                            {
                                Allsended = false;
                                break;
                            }
                        }

                        bool sendedEmail = SendEmail(user_emailTXT.Value, user_contactPersonTXT.Value, 1);
                        if (sendedEmail == true && Allsended == true)
                        {
                            var encrypt = new eXSecurity.Encryption();

                            Dictionary<string, string> fields = new Dictionary<string, string>();
                            fields.Add("user_name", register_user_nameTXT.Value);
                            fields.Add("user_password", encrypt.EncryptData(register_user_passwordTXT.Value));
                            fields.Add("user_email", user_emailTXT.Value);
                            fields.Add("user_phone", user_phoneTXT.Value);
                            fields.Add("user_companyName", user_companyNameTXT.Value);
                            fields.Add("user_contactPerson", user_contactPersonTXT.Value);
                            fields.Add("user_address", user_addressTXT.Value);
                            fields.Add("user_fax", user_faxTXT.Value);
                            Controller.InsertInto(cmd, con, "SEC_User", fields, false);

                            fields.Clear();
                            fields.Add("user_name", register_user_nameTXT.Value);
                            fields.Add("role_id", "2");  //client
                            Controller.InsertInto(cmd, con, "SEC_RoleUser", fields, false);
                            ClearTXT();
                        }
                        else
                        {
                            ClearTXT();
                            AlertJS(5);
                            return;
                        }
                    }
                    else
                    {
                        ClearTXT();
                        AlertJS(6);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ClearTXT();
            Controller.SaveErrors(Path.GetFileName(Request.PhysicalPath), ex.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name);
        }
    } 

    protected void ClearTXT()
    {
        login_user_nameTXT.Text = "";
        //login_user_nameTXT.Value = "";
        login_user_passwordTXT.Value = "";
        register_user_nameTXT.Value = "";
        register_user_passwordTXT.Value = "";
        user_emailTXT.Value = "";
        user_phoneTXT.Value = "";
        user_companyNameTXT.Value = "";
        user_contactPersonTXT.Value = "";
        user_addressTXT.Value = "";
    }

    protected void fillSessions(SqlCommand cmd, SqlConnection con, string role_id)
    {
        var encrypt = new eXSecurity.Encryption();

        Session["user_name"] = login_user_nameTXT.Text;
        //Session["user_name"] = login_user_nameTXT.Value;
        Session["language_id"] = "1";   //EN 
        Session["role_id"] = encrypt.EncryptData(role_id);   //EN 

    }

    //*** Alert 
    protected void Alert(string msg, string title, string imgUrl)
    {
        //radalert(text, oWidth, oHeight, oTitle, callbackFn, imgUrl);
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

    //*** SendEmail   
    protected bool SendEmail(string toEmail, string toUser_name, int index)
    {
        string subject = "";
        string body = "";

        if (index == 1)
        {
            subject = "Welcome ";
            body = "Hello " + toUser_name + ", " + Environment.NewLine + Messages.welcomeUser;
        }
        else if (index == 2)
        {
            subject = "New Customer";
            body = "Hello " + toUser_name + ", " + Environment.NewLine + Messages.newCustomer;
        }

        return Controller.SendEmail(toEmail, toUser_name, subject, body);
    }


    //*** AlertJS
    protected void AlertJS(int index)
    {
        string fct = "";
        if (index == 1)  //error user
            fct = "ErralertUser();";
        else if (index == 2)
            fct = "ErralertValid();";
        else if (index == 3)
            fct = "ErralertAcc();";
        else if (index == 4)
            fct = "ErralertRecaptcha();";
        else if (index == 5)
            fct = "ErralertEmail();";
        else if (index == 6)
            fct = "ErrSomthingWrong();";

        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", fct, true);

    }

    //*** Redirect
    protected void Redirect(string path)
    {
        Response.Redirect(path, false);
        Context.ApplicationInstance.CompleteRequest();
    }
    //***

    //***

    //private void AlertJS(string txt)
    //{
    //    string script = "<script>alert('" + txt + "')</" + "script>";
    //    ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", script, false);
    //}

    public class FailValidator : BaseValidator
    {
        protected override bool ControlPropertiesValid()
        {
            // make setting ControlToValidate optional
            return true;
        }

        protected override bool EvaluateIsValid()
        {
            return false;
        }
    }
}