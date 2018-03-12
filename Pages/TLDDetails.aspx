<%@ Page Language="C#" validateRequest="false" MasterPageFile="~/Pages/MasterPage.master" AutoEventWireup="true" CodeFile="TLDDetails.aspx.cs" Inherits="Pages_TLD_TLDDetails" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:content ID="Content2" ContentPlaceHolderID="head" Runat="Server">
    <title >
        TLD Details
    </title>
</asp:content> 
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" Runat="Server">
    <div style ="float :right">
        <telerik:RadButton RenderMode="Lightweight" ID="previous" OnClick ="previous_Click" runat="server" Text="Previous">
            <Icon PrimaryIconCssClass="rbPrevious"></Icon>
        </telerik:RadButton>
    </div>
    <telerik:RadComboBox  runat ="server" ID="langRCB"  Visible="false"  Width="50" Filter="Contains" EnableVirtualScrolling="true" DropDownHeight="150px" DataTextField="language_name" DataValueField ="language_id" OnSelectedIndexChanged ="langRCB_SelectedIndexChanged" AutoPostBack="true"/>
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

        <MasterTableView AutoGenerateColumns="false" ShowHeadersWhenNoRecords="true" DataKeyNames="tldhd_id" CommandItemDisplay="Top" EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnFirstPage" Width="100%" >
            <CommandItemSettings AddNewRecordText="Add" />

            <Columns> 

                <telerik:GridTemplateColumn HeaderStyle-Width="10%" DataType="system.String" AutoPostBackOnFilter ="true" ShowFilterIcon="false" HeaderText="NAME" DataField="tldhd_name" SortExpression="tldhd_name" UniqueName="tldhd_name" >
                    <ItemTemplate>
                        <asp:Label runat="server" width="100%" text ='<%# Eval("tldhd_name").ToString()%>' id="tldhd_nameLBL"/>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox runat="server" width="100%" MaxLength ="100" id="tldhd_nameTXT" text ='<%# Eval("tldhd_name").ToString()%>'/>
                        <asp:RequiredFieldValidator runat ="server" ID="tldhd_nameRFV" ForeColor ="Red" text="Required." ControlToValidate ="tldhd_nameTXT"/>
                    </EditItemTemplate>
                </telerik:GridTemplateColumn>      

                <telerik:GridEditCommandColumn ButtonType="ImageButton" HeaderStyle-Width="3%" UniqueName="EditCommandColumn"/>

                <telerik:GridButtonColumn HeaderStyle-Width="3%" UniqueName="delete" Text="Delete" CommandName="Delete" ButtonType="ImageButton" ConfirmText="Are you sure you want to delete?" ConfirmDialogType="RadWindow" ConfirmTitle="Delete"/>
                              
            </Columns>

        </MasterTableView>
    </telerik:RadGrid> 
</asp:content> 

