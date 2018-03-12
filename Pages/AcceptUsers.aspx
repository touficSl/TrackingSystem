<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation ="false" MasterPageFile="~/Pages/MasterPage.master" validateRequest="false" CodeFile="AcceptUsers.aspx.cs" Inherits="Pages_AcceptUsers" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:content ID="Content2" ContentPlaceHolderID="head" Runat="Server">
    <title >
        Users
    </title>
    <style >
        .RadGrid_Metro .rgSelectedRow>td {
            border-color: #4F52BA !important;
        }
        .rgSelectedRow {
            color: #fff;
            background: #4F52BA !important;
        }
    </style>
    <script >
        function NewUsers() { //new_user_name 
            if (window.parent.document.getElementById('new_user_name') != null) {
                if (parseInt(window.parent.document.getElementById('new_user_name').innerText) == 1) {
                    window.parent.document.getElementById('new_user_name').style.display = "none";
                    window.parent.document.getElementById('user_accepted').style.display = "none";
                } else if (parseInt(window.parent.document.getElementById('new_user_name').innerText) > 1) {
                    var msg = parseInt(window.parent.document.getElementById('new_user_name').innerText) - 1;
                    window.parent.document.getElementById('new_user_name').innerText = msg;
                    var msg = parseInt(window.parent.document.getElementById('user_accepted').innerText) - 1;
                    window.parent.document.getElementById('user_accepted').innerText = msg;
                }
            }
            var x = document.getElementsByTagName("INPUT");
            var cnt2 = 0;
            for (var cnt = 0; cnt < x.length; cnt++) {
                if (x[cnt].type == "text") {
                    if (x[cnt].id.search("TXT") == -1)
                        x[cnt].placeholder = "Search...";
                }
            } 
        }

        //function UsersAccepted(i) {  //user_accepted
        //    if (i == 0) {
        //        if (parseInt(window.parent.document.getElementById('user_accepted').innerText) == 1) {
        //            window.parent.document.getElementById('user_accepted').style.display = "none";
        //        } else if (parseInt(window.parent.document.getElementById('user_accepted').innerText) > 1) {
        //            var msg = parseInt(window.parent.document.getElementById('user_accepted').innerText) - 1;
        //            window.parent.document.getElementById('user_accepted').innerText = msg;
        //        }
        //    } else {
        //        var msg = parseInt(window.parent.document.getElementById('user_accepted').innerText) + 1;
        //        window.parent.document.getElementById('user_accepted').innerText = msg;
        //    }
        //}
    </script>
</asp:content> 
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" Runat="Server">
    
    <telerik:RadWindowManager ID="rwm" runat="server" Skin="Metro">
        <Windows>
            <telerik:RadWindow runat="server" ID="window1" Skin="Metro">
            </telerik:RadWindow>
            <telerik:RadWindow runat="server" ID="window2" Skin="Metro">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>  
    
    <div style ="float:right ">
        <telerik:RadButton skin="Metro" RenderMode="Lightweight" ID="refresh" runat="server" Text="Refresh" OnClick ="refresh_Click">
            <Icon SecondaryIconCssClass="rbRefresh"></Icon>
        </telerik:RadButton>
    </div>

    <asp:UpdatePanel runat ="server" >
        <ContentTemplate>  
            <telerik:RadComboBox  runat ="server" ID="langRCB" Width="50" Filter="Contains" EnableVirtualScrolling="true" DropDownHeight="150px" DataTextField="language_name" DataValueField ="language_id" OnSelectedIndexChanged ="langRCB_SelectedIndexChanged" AutoPostBack="true"/>


            <telerik:RadGrid ID="RadGridBoxes" OnSelectedIndexChanged="RadGridBoxes_SelectedIndexChanged" OnNeedDataSource="RadGridBoxes_NeedDataSource1"  OnItemCommand ="RadGridBoxes_ItemCommand" OnItemDataBound ="RadGridBoxes_ItemDataBound" OnItemCreated ="RadGridBoxes_ItemCreated" Skin="Metro" AllowFilteringByColumn="true" runat="server" AllowPaging="True" CssClass="RadGridCustomClass" AllowSorting="true">
                <ClientSettings  EnableRowHoverStyle="true">
                    <Resizing AllowColumnResize="true" ShowRowIndicatorColumn ="false" AllowRowResize="True" ResizeGridOnColumnResize="false" ClipCellContentOnResize="false" EnableRealTimeResize="false" AllowResizeToFit="true" />
                </ClientSettings> 
                <ClientSettings Selecting-AllowRowSelect="true" EnablePostBackOnRowClick="true"/>
                 
                <PagerStyle Mode="Advanced" ></PagerStyle> 

                <MasterTableView AutoGenerateColumns="false" DataKeyNames="user_name" EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnFirstPage" Width="100%" >
                    <%--<CommandItemSettings ShowAddNewRecordButton="false" />--%>

                    <Columns> 
                        <telerik:GridTemplateColumn HeaderStyle-Width="10%" AutoPostBackOnFilter ="true" HeaderText="NAME" ShowFilterIcon="false" DataField="user_contactPerson" SortExpression="user_contactPerson" UniqueName="user_contactPerson" DataType ="System.String"> 
                            <ItemTemplate>
                                <asp:label runat="server" text='<%# Eval("user_contactPerson").ToString()%>' id="user_contactPersonLBL"/> 
                            </ItemTemplate>  
                        </telerik:GridTemplateColumn> 
                                
                        <telerik:GridBoundColumn ShowFilterIcon="false" HeaderText="INSTITUTION" DataType ="System.String" AutoPostBackOnFilter="true"  DataField="user_companyName" SortExpression="user_companyName" UniqueName="user_companyName"/>
                                
                        <telerik:GridTemplateColumn HeaderStyle-Width="10%" AutoPostBackOnFilter ="true" HeaderText="EMAIL" ShowFilterIcon="false" DataField="user_email" SortExpression="user_email" UniqueName="user_email" DataType ="System.String"> 
                            <ItemTemplate>
                                <asp:label runat="server" text='<%# Eval("user_isnew").ToString()%>' Visible ="false" id="user_isnew"/> 
                                <asp:label runat="server" text='<%# Eval("user_email").ToString()%>' id="user_email"/> 
                            </ItemTemplate>  
                        </telerik:GridTemplateColumn> 

                        <telerik:GridBoundColumn ShowFilterIcon="false" HeaderText="PHONE" AutoPostBackOnFilter="true"  DataField="user_phone" SortExpression="user_phone" UniqueName="user_phone" DataType ="System.String"/>
                                 
                        <telerik:GridTemplateColumn HeaderStyle-Width="10%" AutoPostBackOnFilter ="true" HeaderText="ACCEPTED" ShowFilterIcon="false" DataField="active" SortExpression="active" UniqueName="active" DataType ="System.Boolean"> 
                            <ItemTemplate>
                                <asp:checkbox runat="server" checked='<%# Eval("active")%>' id="edit_activeCB"/>
                                <asp:Label runat="server" style="visibility :hidden" text="*" />
                            </ItemTemplate>  
                        </telerik:GridTemplateColumn>  

                        <telerik:GridTemplateColumn HeaderStyle-Width="20%" AutoPostBackOnFilter ="true" HeaderText="REASON" ShowFilterIcon="false" DataField="user_reason" SortExpression="user_reason" UniqueName="user_reason"> 
                            <ItemTemplate>
                                <asp:textbox textmode="MultiLine" Width ="100%" runat="server" text='<%# Eval("user_reason").ToString()%>' id="edit_user_reasonTXT" placeholder="Reason"/>
                                <asp:Label runat="server" style="visibility :hidden" text="*" />
                            </ItemTemplate>  
                        </telerik:GridTemplateColumn> 

                        <telerik:GridBoundColumn ShowFilterIcon="false" DataType ="System.String" HeaderText="ADDRESS" AutoPostBackOnFilter="true"  DataField="user_address" SortExpression="user_address" UniqueName="user_address"/>

                        <telerik:GridBoundColumn ShowFilterIcon="false" DataType ="System.String" HeaderText="FAX" AutoPostBackOnFilter="true"  DataField="user_fax" SortExpression="user_fax" UniqueName="user_fax"/>

                        <telerik:GridButtonColumn HeaderStyle-Width="3%"  UniqueName="edit" Text="Edit" CommandName="Update" ButtonType="ImageButton" ConfirmText="Are you sure you want to accept?" ConfirmDialogType="RadWindow" ConfirmTitle="Edit" ImageUrl ="~/Pages/images/editBTN.png"/>
                        
                        <telerik:GridButtonColumn HeaderStyle-Width="3%" UniqueName="delete" Text="Delete" CommandName="Delete" ButtonType="ImageButton" ConfirmText="Are you sure you want to delete?" ConfirmDialogType="RadWindow" ConfirmTitle="Delete"/>
                    </Columns> 
                </MasterTableView>
            </telerik:RadGrid>
          </ContentTemplate>
    </asp:UpdatePanel> 
</asp:Content>  
