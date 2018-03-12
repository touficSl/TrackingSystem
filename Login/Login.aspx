<%@ Page Language="C#" AutoEventWireup="true" validateRequest="false" CodeFile="Login.aspx.cs" Inherits="login_Login" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="recaptcha" Namespace="Recaptcha" Assembly="Recaptcha" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
 <head>
    <meta charset="UTF-8">
    <title>Sign-Up/Login </title>
    <link href='http://fonts.googleapis.com/css?family=Titillium+Web:400,300,600' rel='stylesheet' type='text/css'>
    <link rel="stylesheet" href="css/normalize.css">
    <link rel="stylesheet" href="css/style.css">         
    <script src="../SweetAlert/sweetalert2.min.js"></script>
    <link rel="stylesheet" href="../SweetAlert/sweetalert2.min.css"> 
    <%--<script src="../Javascript/CleanTXT.js"></script>--%>

     <script type="text/javascript">
         //var RecaptchaOptions = {
         //    theme: 'custom',
         //    custom_theme_widget: 'recaptcha_widget'
         //};
         //var RecaptchaOptions = {
         //    theme: 'blackglass'
         //};
         var onloadCallback = function () {
             grecaptcha.reset();
            grecaptcha.render('html_element', {
                '6LfBkB0TAAAAAMGeWRUWQzCjoK6T7T1u1p5OXTL_': '6LfBkB0TAAAAAKacPsgpCr9pxtODxhILcIhxc__V'
            }); 
         };

         function ErralertRecaptcha() {
             swal({
                 title: 'Oops...',
                 text: 'Please check recaptcha.',
                 type: 'error'
             });
         }

         function ErralertAcc() {
             swal({
                 title: 'Oops...',
                 text: 'Your account is not accepted.',
                 type: 'error'
             });
         }

         function ErralertValid() {
             swal({
                 title: 'Oops...',
                 text: 'The username or password is incorrect. Please try again.',
                 type: 'error'
             });
         }

         function ErralertUser() {
             swal({
                 title: 'Oops...',
                 text: 'This username is already exists.',
                 type: 'error'
             });
         }

         function ErralertEmail() {
             swal({
                 title: 'Oops...',
                 text: 'Your Email is incorrect.',
                 type: 'error'
             });
         }

         function ErrSomthingWrong() {
             swal({
                 title: 'Oops...',
                 text: 'Something went wrong, Please contact your administrator.',
                 type: 'error'
             });
         }
    </script>- 
     <script src="//ajax.googleapis.com/ajax/libs/jquery/1.10.2/jquery.min.js"></script>
     <script src='https://www.google.com/recaptcha/api.js'></script>
     <style >           
         .field-wrap { 
            margin-bottom: 10px !important ;
        }

         body { 
          background-size: 100% 100% !important ;
        }
         html{
             height :100% !important;
              min-height: 1000px !important;
         }
         .visisbility{
             visibility :hidden ;
         }
     </style>
  </head>  
  <body>
    <form runat ="server" novalidate="novalidate">
        
        <div class="form">
      
          <ul class="tab-group">
            <li class="tab "><a href="#signup">Sign Up</a></li>
            <li class="tab active"><a href="#login">Log In</a></li>
          </ul>
      
          <div class="tab-content"> 
            <div id="login">   
              <h1>Welcome Back!</h1>
          
              <div  >
          
                <div class="field-wrap">
                <label>
                  User Name<span class="req">*</span>
                </label>
                <asp:textbox id="login_user_nameTXT" runat ="server" />
                <%--<input type="text" id="login_user_nameTXT" runat ="server" required="required" autocomplete="off"/>--%>
                <asp:RequiredFieldValidator runat ="server" ForeColor ="Red" text="Required." ControlToValidate ="login_user_nameTXT" ValidationGroup ="login"/>
              </div>
          
              <div class="field-wrap">
                <label>
                  Password<span class="req">*</span>
                </label>
                <input type="password" id="login_user_passwordTXT" runat ="server" required="required" autocomplete="off"/>
                <asp:RequiredFieldValidator runat ="server" ForeColor ="Red" text="Required." ControlToValidate ="login_user_passwordTXT" ValidationGroup ="login"/>
              </div>
          
              <div class="field-wrap"> 
                  <p class="forgot"><a href="../Pages/ForgetPassword.aspx" target ="_blank">Forgot Password?</a></p>
              </div> 
                   
                <input type="submit" id="loginBTN" onserverclick="LogIn" runat ="server" class="button button-block" value="LOG IN" ValidationGroup ="login"/>
              </div>

            </div>
        
            <div id="signup">   
              <h1>Sign Up</h1>
          
              <div >
                <asp:ScriptManager ID="ScriptManager" runat="server" />
          
              <div class="field-wrap">
                <label>
                  User Name<span class="req">*</span>
                </label>
                <input type="text"  id="register_user_nameTXT" maxlength ="100" runat ="server" required="required" autocomplete="off"/>
                <asp:RequiredFieldValidator runat ="server" ForeColor ="Red" text="Required." ControlToValidate ="register_user_nameTXT" />
              </div>  

              <div class="field-wrap">
                <label>
                  Password<span class="req">*</span>
                </label>
                <input type="password" id="register_user_passwordTXT" runat ="server" required="required" autocomplete="off"/>
                <asp:RequiredFieldValidator runat ="server" ForeColor ="Red" text="Required." ControlToValidate ="register_user_passwordTXT" />
                <div style ="float :right ">
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Invalid password (8-characters)." ForeColor ="red" ControlToValidate="register_user_passwordTXT" ValidationExpression=".{8}" SetFocusOnError="true" />
                </div>
              </div>

              <div class="field-wrap">
                <label>
                  Contact Person<span class="req">*</span>
                </label>
                <input id="user_contactPersonTXT" runat ="server" maxlength ="100" required="required" autocomplete="off"/>
                <asp:RequiredFieldValidator runat ="server" ForeColor ="Red" text="Required." ControlToValidate ="user_contactPersonTXT" />
              </div>

              <div class="field-wrap">
                <label>
                  Email Address<span class="req">*</span>
                </label>
                <input type="email" id="user_emailTXT" runat ="server" required="required" autocomplete="off"/>
                    <asp:RequiredFieldValidator runat ="server" ForeColor ="Red" text="Required." ControlToValidate ="user_emailTXT" />
                <div style ="float :right ">
                    <asp:RegularExpressionValidator runat="server" ErrorMessage="Invalid e-mail address." ForeColor ="red" ControlToValidate="user_emailTXT" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" SetFocusOnError="true" />
                </div>
              </div>

              <div class="field-wrap">
                <label>
                  Institution Name<span class="req">*</span>
                </label>
                <input type="text" id="user_companyNameTXT" maxlength ="150" runat ="server" required="required" autocomplete="off"/>
                <asp:RequiredFieldValidator runat ="server" ForeColor ="Red" text="Required." ControlToValidate ="user_companyNameTXT" />
              </div>

              <div class="field-wrap">
                <label>
                  Address<span class="req">*</span>
                </label>
                <input type="text" id="user_addressTXT" maxlength ="150" runat ="server" required="required" autocomplete="off"/>
                <asp:RequiredFieldValidator runat ="server" ForeColor ="Red" text="Required." ControlToValidate ="user_addressTXT" />
              </div>

              <div class="field-wrap">
                <label>
                  Phone Number
                </label>
                <input type="text" id="user_phoneTXT" maxlength ="15" runat ="server"  autocomplete="off"/>
                  <asp:Label Text ="*" runat="server" CssClass="visisbility"/>
                <%--<asp:RequiredFieldValidator runat ="server" ForeColor ="Red" text="Required." ControlToValidate ="user_phoneTXT"  />--%>
                <div style ="float :right ">
                    <asp:RegularExpressionValidator ID="po_numberREV" runat="server" ErrorMessage="Invalid phone number (8-digits)." ForeColor ="red" ControlToValidate="user_phoneTXT" ValidationExpression="^[0-9][0-9]{7}$" SetFocusOnError="true" />
                </div>
              </div>  

              <div class="field-wrap">
                <label>
                  Fax
                </label>
                <input type="text" id="user_faxTXT" runat ="server" maxlength ="150" autocomplete="off"/>
                <asp:Label Text ="*" runat="server" CssClass="visisbility"/>
                <%--<asp:RequiredFieldValidator runat ="server" ForeColor ="Red" text="Required." ControlToValidate ="user_phoneTXT"  />
                <div style ="float :right ">
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Invalid phone number (8-digits)." ForeColor ="red" ControlToValidate="user_phoneTXT" ValidationExpression="^[0-9][0-9]{7}$" SetFocusOnError="true" />
                </div>--%>
              </div> 
          
              <div class="field-wrap">
                  <recaptcha:RecaptchaControl ID="recaptcha" runat="server" PublicKey="6LfBkB0TAAAAAMGeWRUWQzCjoK6T7T1u1p5OXTL_" PrivateKey="6LfBkB0TAAAAAKacPsgpCr9pxtODxhILcIhxc__V" />
              </div> 

                <input type="submit" id="RegisterBTN" onserverclick="Register" runat ="server" class="button button-block" value="Get Started" />
              </div>

            </div>

          </div><!-- tab-content -->
      
        </div> <!-- /form -->
        

<%--      <telerik:RadWindowManager ID="rwm" runat="server" Skin="Metro"  OffsetElementID="ContentDiv">
            <Windows>
                <telerik:RadWindow runat="server" ID="window1" Skin="Metro">
                </telerik:RadWindow>
                <telerik:RadWindow runat="server" ID="window2" Skin="Metro">
                </telerik:RadWindow>
            </Windows>
        </telerik:RadWindowManager> --%>
    </form>
    <script src='http://cdnjs.cloudflare.com/ajax/libs/jquery/2.1.3/jquery.min.js'></script>
        <script src="js/index.js"></script>
  </body>
</html>
