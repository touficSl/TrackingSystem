
<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/Pages/MasterPage.master" CodeFile="Profile.aspx.cs" Inherits="Pages_ForgotPassword" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="Server">
    <title>Profile
    </title> 
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">

        <div class="container">
        <div class="container">
            <div class="row">
                <div class="col-md-12">
                    <div class="well well-sm">
                        <div class="form-horizontal" >
                            <fieldset>
                                <legend class="text-center header">Edit Profile</legend>

                                <div class="form-group">Name
                                    <span class="col-md-1 col-md-offset-2 text-center"><i class="fa fa-user bigicon"></i></span>
                                    <div class="col-md-8">
                                        <input id="user_contactPersonTXT" name="name" type="text" runat="server" placeholder="Name" class="form-control"  required="required" />
                                        <asp:RequiredFieldValidator ID="user_contactPersonRFV" runat ="server" ControlToValidate ="user_contactPersonTXT" ErrorMessage ="*" ForeColor ="red" validationgroup="save" />
                                    </div>
                                </div> 
                                 <div class="form-group">
                                    <span class="col-md-1 col-md-offset-2 text-center"><i class="fa fa-building-o bigicon"></i></span>
                                    <div class="col-md-8">
                                        <input id="companynameTXT" name="company" type="text" runat="server" placeholder="Company Name" class="form-control" required="required" />
                                        <asp:RequiredFieldValidator ID="companynameTXTREV" runat ="server" ControlToValidate ="companynameTXT" ErrorMessage ="*" ForeColor ="red" validationgroup="save"/>
                                    </div>
                                </div> 

                                <div class="form-group">
                                    <span class="col-md-1 col-md-offset-2 text-center"><i class="fa fa-pencil-square-o bigicon"></i></span>
                                    <div class="col-md-8">
                                        <asp:textbox class="form-control" runat="server" id="user_emailTXT" placeholder="Email" validationgroup="save"></asp:textbox>
                                        <asp:RequiredFieldValidator ID="user_emailRFV" runat ="server" ControlToValidate ="user_emailTXT" ErrorMessage ="*" ForeColor ="red" />
                                        <div style ="float :right;">
                                            <asp:RegularExpressionValidator ID="user_emailREV" runat="server" ErrorMessage="Invalid e-mail address." ForeColor ="red" ControlToValidate="user_emailTXT" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" SetFocusOnError="true" />
                                        </div>
                                    </div>
                                </div> 
                                <div class="form-group">
                                    <span class="col-md-1 col-md-offset-2 text-center"><i class="fa fa-home bigicon"></i></span>
                                    <div class="col-md-8">
                                        <input id="addressTXT" name="address" type="text" runat="server" placeholder="Address" class="form-control" required="required" />
                                        <asp:RequiredFieldValidator ID="addressTXTREV" runat ="server" ControlToValidate ="addressTXT" ErrorMessage ="*" ForeColor ="red" validationgroup="save"/>
                                    </div>
                                </div> 
                                <div class="form-group">
                                    <span class="col-md-1 col-md-offset-2 text-center"><i class="fa fa-print bigicon"></i></span>
                                    <div class="col-md-8">
                                        <input id="faxTXT" name="fax" type="text" runat="server" placeholder="Fax" class="form-control" />
                                        <asp:RequiredFieldValidator ID="faxREV" runat ="server" ControlToValidate ="faxTXT" ErrorMessage ="*" ForeColor ="red" validationgroup="save" />
                                    </div>
                                </div> 
                                <div class="form-group">
                                    <span class="col-md-1 col-md-offset-2 text-center"><i class="fa fa-mobile bigicon"></i></span>
                                    <div class="col-md-8">
                                        <input id="phoneTXT" name="phone" type="text" runat="server" placeholder="phone" class="form-control" />
                                        <asp:RequiredFieldValidator ID="phoneREV" runat ="server" ControlToValidate ="phoneTXT" ErrorMessage ="*" ForeColor ="red" validationgroup="save" />
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-md-12 text-center">
                                        <button id="BTNSave" type="submit" runat="server" value ="Save" onserverclick="BTNSave_ServerClick" class="btn btn-primary btn-lg" validationgroup="save">Save</button>
                                    </div>
                                </div> 
                                <div id="DivChangePassword" runat="server">
                                 <div class="form-group" >
                                    <span class="col-md-1 col-md-offset-2 text-center"></span>
                                    <div class="col-md-8">
                                        <input id="oldpasswordTXT" name="oldpass" type="text" runat="server" placeholder="Old Password" class="form-control" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat ="server" ControlToValidate ="oldpasswordTXT" ErrorMessage ="*" ForeColor ="red" validationgroup="changepassword" />
                                    </div>

                                </div>
                                <div class="form-group"  >
                                    <span class="col-md-1 col-md-offset-2 text-center"></span>
                                    <div class="col-md-8">
                                        <input id="newpasswordTXT" name="newpass1" type="text" runat="server" placeholder="New Password" class="form-control" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat ="server" ControlToValidate ="newpasswordTXT1" ErrorMessage ="*" ForeColor ="red" validationgroup="changepassword" />
                                    </div>
                                </div>
                                 <div class="form-group"  >
                                    <span class="col-md-1 col-md-offset-2 text-center"></span>
                                    <div class="col-md-8">
                                        <input id="verificationpasswordTXT" name="veriicationpass" type="text" runat="server" placeholder="Verification Password" class="form-control" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat ="server" ControlToValidate ="verificationpasswordTXT" ErrorMessage ="*" ForeColor ="red" validationgroup="changepassword" />
                                    </div>
                                </div>
                                </div>
                            </fieldset>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        </div>
</asp:Content>

