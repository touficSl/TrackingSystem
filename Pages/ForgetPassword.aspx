<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ForgetPassword.aspx.cs" Inherits="Pages_ForgetPassword" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />

    <link rel="stylesheet" type="text/css" href="css/bootstrap.min.css" />
    <link rel="stylesheet" type="text/css" href="css/font-awesome.css" />
    
    <script src="../SweetAlert/sweetalert2.min.js"></script> 
    <link rel="stylesheet" href="../SweetAlert/sweetalert2.min.css"/>

    <script type="text/javascript" src="js/jquery-1.10.2.min.js"></script> 
    <script>
        function erroralert() {
            swal({
                title: 'Warning',
                text: 'Invalid Username or Email.',
                type: 'warning'
            });
        }
        function successalert() {
            swal({
                title: 'Successful!',
                text: 'Successful',
                type: 'success'
            });
        }
    </script>
</head>
<body>
    <form runat ="server">
        <asp:ScriptManager ID="ScriptManager1" runat ="server" />
        <div class="container">

        <div class="page-header">
            
        </div>

        <!-- Contact Form - START -->
        <div class="container">
            <div class="row">
                <div class="col-md-12">
                    <div class="well well-sm">
                        <div class="form-horizontal" >
                            <fieldset>
                                <legend class="text-center header">Forgot Password</legend>

                                <div class="form-group">
                                    <span class="col-md-1 col-md-offset-2 text-center"><i class="fa fa-user bigicon"></i></span>
                                    <div class="col-md-8">
                                        <input id="usernameTXT" name="name" type="text" runat="server" placeholder="User Name" class="form-control" required="required"/>
                                        <asp:RequiredFieldValidator ID="usernameRFV" runat ="server" ControlToValidate ="usernameTXT" ErrorMessage ="*" ForeColor ="red" />
                                    </div>
                                </div> 

                                <div class="form-group">
                                    <span class="col-md-1 col-md-offset-2 text-center"><i class="fa fa-pencil-square-o bigicon"></i></span>
                                    <div class="col-md-8">
                                        <asp:textbox class="form-control" runat="server" id="user_emailTXT" placeholder="Email" ></asp:textbox>
                                        <asp:RequiredFieldValidator ID="user_emailRFV" runat ="server" ControlToValidate ="user_emailTXT" ErrorMessage ="*" ForeColor ="red" />
                                        <div style ="float :right;">
                                            <asp:RegularExpressionValidator ID="user_emailREV" runat="server" ErrorMessage="Invalid e-mail address." ForeColor ="red" ControlToValidate="user_emailTXT" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" SetFocusOnError="true" />
                                        </div>
                                    </div>
                                </div> 

                                <div class="form-group">
                                    <div class="col-md-12 text-center">
                                        <button id="BTN" type="submit" runat="server" value ="Get Password" onserverclick="SendEmail" class="btn btn-primary btn-lg">Get Password</button>
                                    </div>
                                </div>
                            </fieldset>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <style>
            .header {
                color: #36A0FF;
                font-size: 27px;
                padding: 10px;
            }

            .bigicon {
                font-size: 35px;
                color: #36A0FF;
            }
        </style> 

        </div>
    </form>
</body>
</html>

