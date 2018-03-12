using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Messages
/// </summary>
public class Messages
{
    public static string acceptUser = "You have been accepted by Track, you can log in now.";

    public static string welcomeUser = "thanks for registration, You should wait to be accepted from Track.";

    public static string newCustomer = "A new customer has been registered.";

    public static string cancelAccepted = "Your account has been disabled, Please contact your administrator.";

    public static string endRequest = "Your request has been completed in Date : '" + DateTime.Now + "'.";

    public static string sorry = "Sorry it was a mistake, Your request was not completed.";

    public static string forgotPass = "Your Password is ";
    
    public Messages()
    {
        //
        // TODO: Add constructor logic here
        //
    }
}