using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Net;
using System.Net.Mail;

/// <summary>
/// Summary description for Controller
/// </summary>
public class Controller
{
    public static string connection = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;

    public Controller()
    {
        
    }

    //*** SelectSecretaireRole()
    public static DataTable SelectSecretaireRole(SqlCommand cmd, SqlConnection con)
    {
        //ArrayList fields = new ArrayList();
        //fields.Add("user_email");
        //fields.Add("user_name");

        //Dictionary<string, string> conditions = new Dictionary<string, string>();
        //conditions.Add("role_id", "3");   //
        //conditions.Add("role_id", "1");   

        DataTable dtSecreatire = new DataTable();

        cmd.Connection = con;
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = "select user_email, user_contactPerson from View_User where role_id in (@role_id1, @role_id2);";    
        cmd.Parameters.AddWithValue("role_id1", "1");//Admin
        cmd.Parameters.AddWithValue("role_id2", "3");//Secretary
        SqlDataAdapter sda = new SqlDataAdapter();
        sda.SelectCommand = cmd;
        sda.Fill(dtSecreatire);

        //DataTable dtSecreatire = Controller.SelectFrom(cmd, con, "View_User", fields, conditions, new ArrayList(), false, false, "");

        return dtSecreatire;
    }
    //***

    //*** Select
    public static DataTable SelectFrom(SqlCommand cmd, SqlConnection con, string viewTables, ArrayList fields, Dictionary<string, string> conditions, ArrayList groupBy, bool all, bool distinct, string orderBy)
    {
        //connection must be open
        int camma = 1;
        string cmdtext = null;
        if (distinct == true)
        {
            cmdtext = "Select distinct ";
        }
        else {
            cmdtext = "Select ";
        }
        DataTable dt = new DataTable();
         
        if (distinct == true)
        {
            if (all == true)
            {
                cmdtext += "* from " + viewTables;
            }
            else {
                cmdtext += ConcatenateSelectFields(fields) + " from " + viewTables + " group by " + ConcatenateSelectFields(groupBy);
            }

            if (conditions.Count > 0)
            {
                cmdtext += " Where " + ConcatenateSelectConditionsWithParameters(cmd, conditions) + " group by " + ConcatenateSelectFields(groupBy);
            }
        }
        else {
            if (all == true)
            {
                cmdtext += "* from " + viewTables;
            }
            else {
                cmdtext += ConcatenateSelectFields(fields) + " from " + viewTables;
            }

            if (conditions.Count > 0)
            {
                cmdtext += " Where " + ConcatenateSelectConditionsWithParameters(cmd, conditions);
            }
        }
        if (orderBy == "")
            cmdtext += ";";
        else
            cmdtext += " order by " + orderBy + " desc;";

        cmd.Connection = con;
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = cmdtext;
        SqlDataAdapter sda = new SqlDataAdapter();
        sda.SelectCommand = cmd;
        sda.Fill(dt);

        return dt;
    }

    //public static DataTable Security(SqlCommand cmd, SqlConnection con, string v1, object v2)
    //{
    //    throw new NotImplementedException();
    //}

    protected static string ConcatenateSelectFields(ArrayList fields)
    {
        string cmdtext = "";
        int camma = 1;

        foreach (string fieldName in fields)
        {
            if (camma == 1)
            {
                cmdtext += fieldName;
                camma = 2;
            }
            else {
                cmdtext += ", " + fieldName;
            }
        }

        return cmdtext;
    }

    protected static string ConcatenateSelectConditionsWithParameters(SqlCommand cmd, Dictionary<string, string> conditions)
    {
        string cmdtext = "";
        int andstr = 1;
        cmd.Parameters.Clear();

        foreach (KeyValuePair<string, string> kvp in conditions)
        {
            string fieldName = kvp.Key;

            if (andstr == 1)
            {
                cmdtext += fieldName + " = @" + fieldName;
                andstr = 2;
            }
            else {
                cmdtext += " and " + fieldName + " = @" + fieldName;
            }

            string Value = kvp.Value;
            cmd.Parameters.AddWithValue(fieldName, Value);
        }

        return cmdtext;
    }
    //*** End select

    //*** Insert
    public static string InsertInto(SqlCommand cmd, SqlConnection con, string tableName, Dictionary<string, string> fields, bool getIdentity)
    {
        string cmdtext = "insert into " + tableName + " (";
        int andstr = 1;
         
        if (getIdentity == true)
        {
            cmdtext += ConcatenateInsertFieldsValues(cmd, fields) + ";Select @@IDENTITY;";
        }
        else {
            cmdtext += ConcatenateInsertFieldsValues(cmd, fields);
        }

        cmd.Connection = con;
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = cmdtext;

        if (getIdentity == true)
        {
            return cmd.ExecuteScalar().ToString();

        }
        else {
            cmd.ExecuteNonQuery();
            return "";

        }
    }

    protected static string ConcatenateInsertFieldsValues(SqlCommand cmd, Dictionary<string, string> fields)
    {
        string cmdtext = "";
        int camma1 = 1;
        int camma2 = 1;
        cmd.Parameters.Clear();

        foreach (KeyValuePair<string, string> kvp in fields)
        {
            string fieldName = kvp.Key;

            if (camma1 == 1)
            {
                cmdtext += fieldName;
                camma1 = 2;
            }
            else {
                cmdtext += ", " + fieldName;
            }
        }

        cmdtext += ") values (";

        foreach (KeyValuePair<string, string> kvp in fields)
        {
            string fieldName = kvp.Key;

            if (camma2 == 1)
            {
                cmdtext += "@" + fieldName;
                camma2 = 2;
            }
            else {
                cmdtext += ", @" + fieldName;
            }

            string Value = kvp.Value;
            cmd.Parameters.AddWithValue(fieldName, Value);
        }

        cmdtext += ");";

        return cmdtext;
    }
    //*** End Insert

    //*** Update
    public static void Update(SqlCommand cmd, SqlConnection con, string tableName, Dictionary<string, string> fields, Dictionary<string, string> conditions)
    {
        string cmdtext = "Update " + tableName + " set ";

        cmdtext += ConcatenateUpdateFields(cmd, fields) + " where " + ConcatenateUpdateConditionsWithParameters(cmd, conditions) + ";";

        cmd.Connection = con;
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = cmdtext;
        cmd.ExecuteNonQuery();
    }

    protected static string ConcatenateUpdateFields(SqlCommand cmd, Dictionary<string, string> fields)
    {
        string cmdtext = "";
        int camma = 1;
        cmd.Parameters.Clear();

        foreach (KeyValuePair<string, string> kvp in fields)
        {
            string fieldName = kvp.Key;

            if (camma == 1)
            {
                cmdtext += fieldName + " = @" + fieldName;
                camma = 2;
            }
            else {
                cmdtext += ", " + fieldName + " = @" + fieldName;
            }

            string Value = kvp.Value;
            cmd.Parameters.AddWithValue(fieldName, Value);
        }

        return cmdtext;
    }

    protected static string ConcatenateUpdateConditionsWithParameters(SqlCommand cmd, Dictionary<string, string> conditions)
    {
        string cmdtext = "";
        int andstr = 1; 

        foreach (KeyValuePair<string, string> kvp in conditions)
        {
            string fieldName = kvp.Key;

            if (andstr == 1)
            {
                cmdtext += fieldName + " = @" + fieldName;
                andstr = 2;
            }
            else {
                cmdtext += " and " + fieldName + " = @" + fieldName;
            }

            string Value = kvp.Value;
            cmd.Parameters.AddWithValue(fieldName, Value);
        }

        return cmdtext;
    }
    //*** End Update

    //*** Delete
    public static void DeleteFrom(SqlCommand cmd, SqlConnection con, string tableName, Dictionary<string, string> conditions)
    {
        string cmdtext = "Delete from " + tableName + " where ";

        cmdtext += ConcatenateDeleteConditionsWithParameters(cmd, conditions) + ";";

        cmd.Connection = con;
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = cmdtext;
        cmd.ExecuteNonQuery();
    }

    protected static string ConcatenateDeleteConditionsWithParameters(SqlCommand cmd, Dictionary<string, string> conditions)
    {
        string cmdtext = "";
        int andstr = 1;
        cmd.Parameters.Clear();

        foreach (KeyValuePair<string, string> kvp in conditions)
        {
            string fieldName = kvp.Key;

            if (andstr == 1)
            {
                cmdtext += fieldName + " = @" + fieldName;
                andstr = 2;
            }
            else {
                cmdtext += " and " + fieldName + " = @" + fieldName;
            }

            string Value = kvp.Value;
            cmd.Parameters.AddWithValue(fieldName, Value);
        }

        return cmdtext;
    }
    //*** End Delete

    //*** Inactive
    public static void Inactivate(SqlCommand cmd, SqlConnection con, string tableName, string idName, string idValue, string session)
    {
        string cmdtext = "Update " + tableName + " set active = @active, modifiedOn = @modifiedOn where " + idName + " = @" + idName + ";";

        cmd.Connection = con;
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = cmdtext;
        cmd.Parameters.AddWithValue(idName, idValue);
        cmd.Parameters.AddWithValue("active", false);
        cmd.Parameters.AddWithValue("modifedBy", session);
        cmd.Parameters.AddWithValue("modifiedOn", DateTime.Now);
        cmd.ExecuteNonQuery();
    }
    //*** End Inactive

    //*** Select Max id
    public static string SelectMaxID(SqlCommand cmd, SqlConnection con, string tableName, string id)
    {
        cmd.Connection = con;
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = "Select MAX(@id) from " + tableName + ";";
        cmd.Parameters.AddWithValue("id", id);

        if (!string.IsNullOrEmpty(cmd.ExecuteScalar().ToString()))
            return cmd.ExecuteScalar().ToString();
        else
            return "1";
    }
    //*** End Select Max id

    //*** SaveErrors
    public static void SaveErrors(string error_pageName, string error_message, string error_functionName)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(connection))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    Dictionary<string, string> fields = new Dictionary<string, string>();

                    fields.Add("error_page", error_pageName);
                    fields.Add("error_message", error_message);
                    fields.Add("error_function", error_functionName);

                    con.Open();
                    InsertInto(cmd, con, "SYS_Error", fields, false);
                }
            }
        }
        catch (Exception ex)
        {
            String error = ex.Message;
        }
    }
    //*** End SaveErrors

    //*** FillRadGrid
    public static void FillRadGrid(SqlCommand cmd, SqlConnection con, object sender, string TableName, ArrayList fields, Dictionary<string, string> conditions, bool all, string orderBy)
    {
        (sender as RadGrid).DataSource = SelectFrom(cmd, con, TableName, fields, conditions, new ArrayList(), true, false, orderBy);
    }
    //*** 'End FillRadGrid

    //*** FillRadCombobox
    public static void FillRadCombobox(RadComboBox combobx, SqlCommand cmd, SqlConnection con, string TableName, ArrayList fields, Dictionary<string, string> conditions, bool all)
    {
        combobx.DataSource = SelectFrom(cmd, con, TableName, fields, conditions, new ArrayList(), true, false, "");
        combobx.DataBind();
    }
    //*** 'End FillRadGrid

    //*** Security
    public static DataTable Security(SqlCommand cmd, SqlConnection con, string formName, string userName)
    {
        ArrayList fields = new ArrayList();
        fields.Add("role_id");
        Dictionary<string, string> conditons = new Dictionary<string, string>();
        conditons.Add("user_name", userName);
        DataTable dtrole = SelectFrom(cmd, con, "SEC_RoleUser", fields, conditons, new ArrayList(), false, false, "");

        if (dtrole.Rows.Count > 0)
        {
            conditons.Clear();
            conditons.Add("form_name", formName);
            DataTable dtform = SelectFrom(cmd, con, "SYS_Form", new ArrayList(), conditons, new ArrayList(), true, false, "");

            if (dtform.Rows.Count > 0)
            {
                conditons.Clear();
                conditons.Add("role_id", dtrole.Rows[0]["role_id"].ToString());
                conditons.Add("form_name", dtform.Rows[0]["form_name"].ToString());
                DataTable dt = SelectFrom(cmd, con, "SEC_FormRole", new ArrayList(), conditons, new ArrayList(), true, false, "");

                return dt;
            }
        }
        return new DataTable();
    }

    public static bool Can(DataTable dt, string can)
    {
        return Convert.ToBoolean(dt.Rows[0][can]);
    }
    //*** 'End Security 

    //*** ChangeToIntForm()
    public static string ChangeToIntForm(string value)
    {
        return value.Replace(".000", "");
    }
    //***

    //*** Clean()
    public static string Clean(string value)
    {
        value.Replace("'", "");
        value.Replace("<", "");
        value.Replace(">", "");
        return value;
    }
    //*** 

    //*** Translate
    public static String GetComcomponentName(SqlCommand cmd, SqlConnection con, string form_name, string language_id, String componentID, String componentName)
    {
        ArrayList fields = new ArrayList();
        fields.Add("componentTranslate");
        Dictionary<string, string> conditons = new Dictionary<string, string>();
        conditons.Add("language_id", language_id);
        conditons.Add("form_name", form_name);
        conditons.Add("componentId", componentID);
        DataTable dtlang = SelectFrom(cmd, con, "SYS_FormLanguage", fields, conditons, new ArrayList(), false, false, "");

        if (dtlang.Rows.Count > 0)
        {
            componentName = dtlang.Rows[0]["componentTranslate"].ToString();
        }
        return componentName;
    }
    //***

    //*** Translate
    public static void Translate(SqlCommand cmd, SqlConnection con, RadGrid radGridBoxes, string language_id, String formName, ArrayList componentID)
    {
        foreach (string component_id in componentID)
        {
            radGridBoxes.MasterTableView.GetColumn(component_id).HeaderText = GetComcomponentName(cmd, con, formName, language_id, component_id, radGridBoxes.MasterTableView.GetColumn(component_id).HeaderText);
        }

        SwapColumns(language_id, radGridBoxes);
    }
    //***

    //***   SwapColumns
    public static void SwapColumns(String language_id, RadGrid radGridBoxes)
    {
        if (language_id == "2")        //AR
            radGridBoxes.MasterTableView.Dir = GridTableTextDirection.RTL;
        else
            radGridBoxes.MasterTableView.Dir = GridTableTextDirection.LTR;
    }
    //*** 

    //** ToShortDateString
    public static String ToShortDateString(DateTime date)
    {
        if (date != null && date.ToString() != "1/1/0001 12:00:00 AM" && date.ToString () != "1/1/1900 12:00:00 AM")
            return date.ToShortDateString();
        else
            return "";
    }
    //***

    //** FixDate
    public static DateTime FixDate(string date)
    {
        if (date != null && date != "" && date.ToString() != "1/1/0001 12:00:00 AM" && date.ToString() != "1/1/1900 12:00:00 AM")
            return DateTime.Parse(date);
        else
            return DateTime.Now;
    }
    //***


    //*** SendEmail
    public static bool SendEmail(string toEmail, string toUser_name, string subject, string body)
    {
        string fromPass = "fromPass";
        string fromEmail = "fromEmail@gmail.com";  // should be a gmail account here because we are using the Gmail SMTP server
        string fromUser_name = "Sender Name";
        MailAddress fromAddress = new MailAddress(fromEmail, fromUser_name);
        MailAddress toAddress = new MailAddress(toEmail, toUser_name);

        SmtpClient smtp = new SmtpClient
        {
            Host = "smtp.gmail.com",
            Port = 587,
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(fromAddress.Address, fromPass)
        };
        using (var message = new MailMessage(fromAddress, toAddress)
        {
            Subject = subject,
            Body = body
        })
        {
            try {
                smtp.Send(message); 
            }
            catch (SmtpFailedRecipientException ex)
            {
                return false;
            }
        }
        return true;
    }
    //*** 

    //***  ExtraSelect
    public static DataTable ExtraSelect(SqlConnection con, SqlCommand cmd, string select, Dictionary <string, string> conditions, string groupBy)
    {
        DataTable dt = new DataTable();
        string cmdtext = select;
        if (conditions.Count > 0)
        {
            if (groupBy != "")
                cmdtext += " Where " + ConcatenateSelectConditionsWithParameters(cmd, conditions) + " group by " + groupBy;
            else
                cmdtext += " Where " + ConcatenateSelectConditionsWithParameters(cmd, conditions);
        }
        else
            return new DataTable();

        cmd.Connection = con;
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = cmdtext;
        SqlDataAdapter sda = new SqlDataAdapter();
        sda.SelectCommand = cmd;
        sda.Fill(dt);

        return dt;
    }
    //****

    //*** UpdateSendedDate
    public static void UpdateSendedDate(SqlConnection con, SqlCommand cmd, string po_id)
    {
        Dictionary<string, string> fields = new Dictionary<string, string>();
        Dictionary<string, string> conditions = new Dictionary<string, string>();
        fields.Add("po_sended_date", DateTime.Now.ToString());
        fields.Add("po_sendIt", true.ToString());
        conditions.Add("po_id", po_id);
        Update(cmd, con, "PurchaseOrder", fields, conditions);
    }
    //****
}