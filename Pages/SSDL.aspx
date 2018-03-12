<%@ Page Language="C#" validateRequest="false" MasterPageFile="~/Pages/MasterPage.master" AutoEventWireup="true" CodeFile="SSDL.aspx.cs" Inherits="Pages_SSDL" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:content ID="Content2" ContentPlaceHolderID="head" Runat="Server">
    <title >
        SSDL
    </title>
</asp:content> 
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" Runat="Server"> 
    <telerik:RadComboBox Visible="false" runat ="server" ID="langRCB" Width="50" Filter="Contains" EnableVirtualScrolling="true" DropDownHeight="150px" DataTextField="language_name" DataValueField ="language_id" OnSelectedIndexChanged ="langRCB_SelectedIndexChanged" AutoPostBack="true"/>
    <div style ="float :right">
        <telerik:RadButton skin="Metro" RenderMode="Lightweight" ID="previous" OnClick ="previous_Click" runat="server" Text="Previous">
            <Icon PrimaryIconCssClass="rbPrevious"></Icon>
        </telerik:RadButton>
    </div>

    <br />

    <telerik:RadWindowManager ID="rwm" runat="server" Skin="Metro">
        <Windows>
            <telerik:RadWindow runat="server" ID="window1" Skin="Metro">
            </telerik:RadWindow>
            <telerik:RadWindow runat="server" ID="window2" Skin="Metro">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager> 

    <asp:label runat ="server" ID="totalCostLBL" text="Fees per instrument = "/><asp:label runat ="server" ID="totalCostTXT" /> L.L   
    <br />
    <br />
    <telerik:RadGrid ID="RadGridBoxes" OnNeedDataSource="RadGridBoxes_NeedDataSource1"  OnItemCommand ="RadGridBoxes_ItemCommand" Skin="Metro" AllowFilteringByColumn="true" runat="server" AllowPaging="True" CssClass="RadGridCustomClass" AllowSorting="True" PageSize="30">
        <ClientSettings  EnableRowHoverStyle="true">
            <Resizing AllowColumnResize="true" ShowRowIndicatorColumn ="false" AllowRowResize="True" ResizeGridOnColumnResize="false" ClipCellContentOnResize="false" EnableRealTimeResize="false" AllowResizeToFit="true" />
        </ClientSettings> 
         
        <PagerStyle Mode="Advanced" ></PagerStyle> 

        <MasterTableView AutoGenerateColumns="false" DataKeyNames="ssdlsf_id" Caption="Service Fees" CssClass="MasterClass" CommandItemDisplay="Top" EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnFirstPage" Width="100%" >
            <CommandItemSettings AddNewRecordText="Add"/>

            <Columns>  
                                
                <%--  ClientDataKeyNames="ssdlsf_totalCost" 
                           
                    <telerik:GridTemplateColumn HeaderStyle-Width="10%" DataType="system.String" AutoPostBackOnFilter ="true" ShowFilterIcon="false" HeaderText="TOTAL" DataField="ssdlsf_totalCost" SortExpression="ssdlsf_totalCost" UniqueName="ssdlsf_totalCost" >
                    <ItemTemplate>
                        <asp:Label runat="server" width="100%" text ='<%# Controller.ChangeToIntForm(Eval("ssdlsf_totalCost").ToString())%>' />
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:textbox runat="server" width="100%" text ='<%# Eval("ssdlsf_totalCost")%>' placeholder="Total cost" MaxLength ="10" id="ssdlsf_totalCostTXT"/>
                        <asp:RequiredFieldValidator runat ="server" ID="ssdlsf_totalCostRFV" ForeColor ="Red" text="Required." ControlToValidate ="ssdlsf_totalCostTXT"/>
                        <asp:RegularExpressionValidator ID="ssdlsf_totalCostREV" runat="server" ErrorMessage="This entry can only contain numbers." ForeColor ="red" ControlToValidate="ssdlsf_totalCostTXT" ValidationExpression="^[+]?[0-9]{0,10}([.][0-9]{1,3})?$" SetFocusOnError="true"/>
                    </EditItemTemplate>
                </telerik:GridTemplateColumn>--%> 

                <telerik:GridTemplateColumn HeaderStyle-Width="10%" DataType="system.String" AutoPostBackOnFilter ="true" ShowFilterIcon="false" HeaderText="RADIATION QUALITY" DataField="ssdlsf_radiationQuality" SortExpression="ssdlsf_radiationQuality" UniqueName="ssdlsf_radiationQuality" >
                    <ItemTemplate>
                        <asp:Label runat="server" width="100%" text ='<%# Eval("ssdlsf_radiationQuality")%>' id="ssdlsf_radiationQualityLBL"/>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox runat="server" width="100%" text ='<%# Eval("ssdlsf_radiationQuality")%>' MaxLength ="100" placeholder="Radiation quality" id="ssdlsf_radiationQualityTXT"/>
                        <asp:RequiredFieldValidator runat ="server" ID="ssdlsf_radiationQualityRFV" ForeColor ="Red" text="Required." ControlToValidate ="ssdlsf_radiationQualityTXT"/>
                    </EditItemTemplate>
                </telerik:GridTemplateColumn> 

                <telerik:GridTemplateColumn HeaderStyle-Width="12%" DataType="system.String" AutoPostBackOnFilter ="true" ShowFilterIcon="false" HeaderText="SERIAL #" DataField="ssdlsf_serialNbr" SortExpression="ssdlsf_serialNbr" UniqueName="ssdlsf_serialNbr" >
                    <ItemTemplate>
                        <asp:Label runat="server" width="100%" text ='<%# Eval("ssdlsf_serialNbr")%>' id="ssdlsf_serialNbrLBL"/>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox runat="server" width="100%" text ='<%# Eval("ssdlsf_serialNbr")%>' placeholder="Serial #" id="ssdlsf_serialNbrTXT"/>
                        <asp:RequiredFieldValidator runat ="server" ID="ssdlsf_serialNbrRFV" ForeColor ="Red" text="Required." ControlToValidate ="ssdlsf_serialNbrTXT"/>
                        <%--<asp:RegularExpressionValidator ID="ssdlsf_serialNbrREV" runat="server" ErrorMessage="This entry can only contain 30-digits." ForeColor ="red" ControlToValidate="ssdlsf_serialNbrTXT" ValidationExpression="^[1-9][0-9]{0,29}$" SetFocusOnError="true"/>--%>
                    </EditItemTemplate>
                </telerik:GridTemplateColumn>  

                <telerik:GridTemplateColumn HeaderStyle-Width="14%" DataType="system.String" AutoPostBackOnFilter ="true" ShowFilterIcon="false" HeaderText="MODEL" DataField="ssdlsf_modelNbr" SortExpression="ssdlsf_modelNbr" UniqueName="ssdlsf_modelNbr" >
                    <ItemTemplate>
                        <asp:Label runat="server" width="100%" text ='<%# Eval("ssdlsf_modelNbr")%>' id="ssdlsf_modelNbrLBL"/>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox runat="server" width="100%" placeholder="Model" text ='<%# Eval("ssdlsf_modelNbr").ToString()%>' MaxLength="100" id="ssdlsf_modelNbrTXT"/>
                        <asp:RequiredFieldValidator runat ="server" ID="ssdlsf_modelNbrRFV" ForeColor ="Red" text="Required." ControlToValidate ="ssdlsf_modelNbrTXT"/>
                        <%--<asp:RegularExpressionValidator ID="ssdlsf_modelNbrREV" runat="server" ErrorMessage="This entry can only contain 6-digits." ForeColor ="red" ControlToValidate="ssdlsf_modelNbrTXT" ValidationExpression="^[1-9][0-9]{0,5}$" SetFocusOnError="true"/>--%>
                    </EditItemTemplate>
                </telerik:GridTemplateColumn>

                <telerik:GridTemplateColumn HeaderStyle-Width="10%" DataType="system.String" AutoPostBackOnFilter ="true" ShowFilterIcon="false" HeaderText="MANUFACTURER" DataField="ssdlsf_Manufacturer" SortExpression="ssdlsf_Manufacturer" UniqueName="ssdlsf_Manufacturer" >
                    <ItemTemplate>
                        <asp:label runat="server" width="100%" text ='<%# DataBinder.Eval(Container.DataItem, "ssdlsf_Manufacturer").ToString()%>' id="ssdlsf_ManufacturerLBL"/>
                    </ItemTemplate>

                    <EditItemTemplate>
                        <asp:textbox runat="server" width="90%" MaxLength ="100" text ='<%# DataBinder.Eval(Container.DataItem, "ssdlsf_Manufacturer").ToString()%>' id="ssdlsf_ManufacturerTXT"/>
                        <asp:RequiredFieldValidator runat ="server" width="10%" ID="ssdlsf_ManufacturerRFV" ForeColor ="Red" text="Required." ControlToValidate ="ssdlsf_ManufacturerTXT"/>
                    </EditItemTemplate>
                </telerik:GridTemplateColumn>

                <telerik:GridTemplateColumn HeaderStyle-Width="10%" DataType="system.String" AutoPostBackOnFilter ="true" ShowFilterIcon="false" HeaderText="DATE" DataField="ssdlsf_Date" SortExpression="ssdlsf_Date" UniqueName="ssdlsf_Date" >
                    <ItemTemplate>
                        <asp:label runat="server" width="100%" text ='<%# DataBinder.Eval(Container.DataItem, "ssdlsf_Date").ToString()%>' id="ssdlsf_DateLBL"/>
                    </ItemTemplate>

                    <EditItemTemplate>
                        <asp:textbox runat="server" width="90%" MaxLength ="100" text ='<%# DataBinder.Eval(Container.DataItem, "ssdlsf_Date").ToString()%>' id="ssdlsf_DateTXT"/>
                        <asp:RequiredFieldValidator runat ="server" width="10%" ID="ssdlsf_DateRFV" ForeColor ="Red" text="Required." ControlToValidate ="ssdlsf_DateTXT"/>
                    </EditItemTemplate>
                </telerik:GridTemplateColumn>

                <telerik:GridTemplateColumn HeaderStyle-Width="10%" DataType="system.String" AutoPostBackOnFilter ="true" ShowFilterIcon="false" HeaderText="TYPE OF INSTRUMENT" DataField="ssdlsf_typeOfInstrument" SortExpression="ssdlsf_typeOfInstrument" UniqueName="ssdlsf_typeOfInstrument" >
                    <ItemTemplate>
                        <asp:Label runat="server" width="100%" text ='<%# Eval("ssdlsf_typeOfInstrument")%>' id="ssdlsf_typeOfInstrumentLBL"/>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox runat="server" width="100%" text ='<%# Eval("ssdlsf_typeOfInstrument")%>' MaxLength ="100" placeholder="Type of instrument" id="ssdlsf_typeOfInstrumentTXT"/>
                        <asp:RequiredFieldValidator runat ="server" ID="ssdlsf_typeOfInstrumentRFV" ForeColor ="Red" text="Required." ControlToValidate ="ssdlsf_typeOfInstrumentTXT"/>
                    </EditItemTemplate>
                </telerik:GridTemplateColumn> 
                                
                <telerik:GridEditCommandColumn ButtonType="ImageButton" HeaderStyle-Width="3%" UniqueName="EditCommandColumn"/>

                <telerik:GridButtonColumn HeaderStyle-Width="3%" UniqueName="delete" Text="Delete" CommandName="Delete" ButtonType="ImageButton" ConfirmText="Are you sure you want to delete?" ConfirmDialogType="RadWindow" ConfirmTitle="Delete"/>
            </Columns>

        </MasterTableView>
    </telerik:RadGrid> 

    <%--<br />
    <asp:label runat ="server" ID="infoLBL" text="This part will be filled when the lab receive the detector" />
    <br />
    <br />--%>

    <%--<telerik:RadGrid ID="RadGridBoxes2" RenderMode="Lightweight" OnNeedDataSource="RadGridBoxes_NeedDataSource2"  OnItemCommand ="RadGridBoxes_ItemCommand2" OnItemCreated ="RadGridBoxes_ItemCreated2" Skin="Metro" AllowFilteringByColumn="true" runat="server" AllowPaging="True" CssClass="RadGridCustomClass" >
        <ClientSettings  EnableRowHoverStyle="true">
            <Resizing AllowColumnResize="True" AllowRowResize="false" ResizeGridOnColumnResize="false" ClipCellContentOnResize="true" EnableRealTimeResize="false" AllowResizeToFit="true" />
        </ClientSettings>

        <HeaderStyle CssClass="headerbold"/>
        <PagerStyle Mode="Advanced" ></PagerStyle> 

        <MasterTableView AutoGenerateColumns="false" DataKeyNames="ssdllr_id" CommandItemDisplay="Top" EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnFirstPage" Width="100%" >
            <CommandItemSettings AddNewRecordText="Add"/>

            <Columns> 
                        
                <telerik:GridTemplateColumn HeaderStyle-Width="10%" DataType="system.String" AutoPostBackOnFilter ="true" ShowFilterIcon="false" HeaderText="DATE" DataField="ssdllr_date" SortExpression="ssdllr_date" UniqueName="ssdllr_date" >
                    <ItemTemplate>
                        <asp:label runat="server" width="100%" text ='<%# Controller.ToShortDateString((DateTime)DataBinder.Eval(Container.DataItem, "ssdllr_date"))%>' id="ssdllr_dateLBL"/>
                    </ItemTemplate>

                    <EditItemTemplate>
                        <telerik:RadDatePicker runat="server" width="90%" text ='<%# DataBinder.Eval(Container.DataItem, "ssdllr_date").ToString()%>' id="ssdllr_dateRDP"/>
                        <asp:RequiredFieldValidator runat ="server" width="10%" ID="ssdllr_dateRFV" ForeColor ="Red" text="Required." ControlToValidate ="ssdllr_dateRDP"/>
                    </EditItemTemplate>
                </telerik:GridTemplateColumn> 
                        
                <telerik:GridTemplateColumn HeaderStyle-Width="12%" DataType="system.String" AutoPostBackOnFilter ="true" ShowFilterIcon="false" HeaderText="NAME OF RECEIVER" DataField="ssdllr_nameReceiver" SortExpression="ssdllr_nameReceiver" UniqueName="ssdllr_nameReceiver" >
                    <ItemTemplate>
                        <asp:Label runat="server" width="100%" text ='<%# Eval("ssdllr_nameReceiver")%>' id="ssdllr_nameReceiverLBL"/>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox runat="server" width="100%" text ='<%# Eval("ssdllr_nameReceiver")%>' MaxLength ="100" placeholder="Name of receiver" id="ssdllr_nameReceiverTXT"/>
                        <asp:RequiredFieldValidator runat ="server" ID="ssdllr_nameReceiverRFV" ForeColor ="Red" text="Required." ControlToValidate ="ssdllr_nameReceiverTXT"/>
                    </EditItemTemplate>
                </telerik:GridTemplateColumn>  
                        
                <telerik:GridTemplateColumn HeaderStyle-Width="10%" DataType="system.String" AutoPostBackOnFilter ="true" ShowFilterIcon="false" HeaderText="NAME OF CLIENT" DataField="ssdllr_clientName" SortExpression="ssdllr_clientName" UniqueName="ssdllr_clientName" >
                    <ItemTemplate>
                        <asp:Label runat="server" width="100%" text ='<%# Eval("ssdllr_clientName")%>' id="ssdllr_clientNameLBL"/>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox runat="server" width="100%" text ='<%# Eval("ssdllr_clientName")%>' MaxLength ="100" placeholder="Name of client" id="ssdllr_clientNameTXT"/>
                        <asp:RequiredFieldValidator runat ="server" ID="ssdllr_clientNameRFV" ForeColor ="Red" text="Required." ControlToValidate ="ssdllr_clientNameTXT"/>
                    </EditItemTemplate>
                </telerik:GridTemplateColumn>  

                <telerik:GridTemplateColumn HeaderStyle-Width="10%" DataType="system.String" AutoPostBackOnFilter ="true" ShowFilterIcon="false" HeaderText="FUNCTIONAL STATUS" DataField="ssdllr_functionalStatus" SortExpression="ssdllr_functionalStatus" UniqueName="ssdllr_functionalStatus" >
                    <ItemTemplate>
                        <asp:Label runat="server" width="100%" text ='<%# Controller.ChangeToIntForm(Eval("ssdllr_functionalStatus").ToString())%>' />
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:textbox runat="server" width="100%" text ='<%# Eval("ssdllr_functionalStatus")%>' placeholder="Functional status" MaxLength ="100" id="ssdllr_functionalStatusTXT"/>
                        <asp:RequiredFieldValidator runat ="server" ID="ssdllr_functionalStatusRFV" ForeColor ="Red" text="Required." ControlToValidate ="ssdllr_functionalStatusTXT"/>
                    </EditItemTemplate>
                </telerik:GridTemplateColumn>  
   
                <telerik:GridEditCommandColumn ButtonType="ImageButton" HeaderStyle-Width="3%" UniqueName="EditCommandColumn"/>

                <telerik:GridButtonColumn HeaderStyle-Width="3%" UniqueName="delete" Text="Delete" CommandName="Delete" ButtonType="ImageButton" ConfirmText="Are you sure you want to delete?" ConfirmDialogType="RadWindow" ConfirmTitle="Delete"/>
            </Columns>

        </MasterTableView>
    </telerik:RadGrid>--%>

    <br />
    <br />
        
    <telerik:RadButton skin="Metro" RenderMode="Lightweight" ID="SendBTN" runat="server" Text="Send" width="110"   Height ="35" CssClass="SendBtn" SingleClick ="true" OnClientClicking="RadConfirm" OnClick="SendBTN_Click"/>
</asp:Content>




 