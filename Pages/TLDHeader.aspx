<%@ Page Language="C#" validateRequest="false" MasterPageFile="~/Pages/MasterPage.master" AutoEventWireup="true" CodeFile="TLDHeader.aspx.cs" Inherits="Pages_TLD_TLDHeader" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:content ID="Content2" ContentPlaceHolderID="head" Runat="Server">
    <title >
        TLD Headers
    </title>
</asp:content> 
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" Runat="Server">
    <telerik:RadComboBox  runat ="server" Visible="false" ID="langRCB" Width="50" Filter="Contains" EnableVirtualScrolling="true" DropDownHeight="150px" DataTextField="language_name" DataValueField ="language_id" OnSelectedIndexChanged ="langRCB_SelectedIndexChanged" AutoPostBack="true"/>
              
    <telerik:RadWindowManager ID="rwm" runat="server" Skin="Metro">
        <Windows>
            <telerik:RadWindow runat="server" ID="window1" Skin="Metro">
            </telerik:RadWindow>
            <telerik:RadWindow runat="server" ID="window2" Skin="Metro">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>   

    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadGridBoxes">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGridBoxes"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting> 
        </AjaxSettings>
    </telerik:RadAjaxManager>

    <telerik:RadGrid ID="RadGridBoxes" OnNeedDataSource="RadGridBoxes_NeedDataSource1" OnItemCommand ="RadGridBoxes_ItemCommand" Skin="Metro" AllowFilteringByColumn="false" runat="server" AllowPaging="True" CssClass="RadGridCustomClass" AllowSorting="True" PageSize="30">
        <ClientSettings  EnableRowHoverStyle="true">
            <Resizing AllowColumnResize="true" ShowRowIndicatorColumn ="false" AllowRowResize="True" ResizeGridOnColumnResize="false" ClipCellContentOnResize="false" EnableRealTimeResize="false" AllowResizeToFit="true" />
        </ClientSettings> 
         
        <PagerStyle Mode="Advanced" ></PagerStyle> 

        <MasterTableView AutoGenerateColumns="false" ShowHeadersWhenNoRecords="true" DataKeyNames="tldh_id" CommandItemDisplay="Top" EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnFirstPage" Width="100%" >
            <CommandItemSettings AddNewRecordText="Add" />

            <Columns> 

                <telerik:GridTemplateColumn HeaderStyle-Width="10%" DataType="system.String" AutoPostBackOnFilter ="true" ShowFilterIcon="false" HeaderText="NAME" DataField="tldh_name" SortExpression="tldh_name" UniqueName="tldh_name" >
                    <ItemTemplate>
                        <asp:Label runat="server" width="100%" text ='<%# Eval("tldh_name").ToString()%>' id="tldh_nameLBL"/>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox runat="server" width="100%" MaxLength ="100" placeholder="Name" id="tldh_nameTXT" text ='<%# Eval("tldh_name").ToString()%>'/>
                        <asp:RequiredFieldValidator runat ="server" ID="tldh_nameRFV" ForeColor ="Red" text="Required." ControlToValidate ="tldh_nameTXT"/>
                    </EditItemTemplate>
                </telerik:GridTemplateColumn>      

                <telerik:GridEditCommandColumn ButtonType="ImageButton" HeaderStyle-Width="3%" UniqueName="EditCommandColumn"/>

                <telerik:GridButtonColumn HeaderStyle-Width="3%" UniqueName="delete" Text="Delete" CommandName="Delete" ButtonType="ImageButton" ConfirmText="Are you sure you want to delete?" ConfirmDialogType="RadWindow" ConfirmTitle="Delete"/>
                             
                <telerik:GridTemplateColumn HeaderStyle-Width="3%" HeaderText="Details" AllowFiltering ="false">
                    <ItemTemplate>
                        <asp:LinkButton runat="server" width="100%" text = "Details" CommandArgument ='<%# DataBinder.Eval(Container.DataItem, "tldh_id").ToString()%>' OnClick ="Details_Click"/>
                    </ItemTemplate>
                </telerik:GridTemplateColumn> 
            </Columns>

        </MasterTableView>
    </telerik:RadGrid> 
</asp:Content>