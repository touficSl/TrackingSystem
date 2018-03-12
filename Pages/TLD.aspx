<%@ Page Language="C#" validateRequest="false" MasterPageFile="~/Pages/MasterPage.master" AutoEventWireup="true" CodeFile="TLD.aspx.cs" Inherits="Pages_TLD_TLD" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %> 
<asp:content ID="Content2" ContentPlaceHolderID="head" Runat="Server">
    <title >
        TLD
    </title>
</asp:content> 
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" Runat="Server">
    <telerik:RadComboBox  runat ="server"  Visible="false"  ID="langRCB" Width="50" Filter="Contains" EnableVirtualScrolling="true" DropDownHeight="150px" DataTextField="language_name" DataValueField ="language_id" OnSelectedIndexChanged ="langRCB_SelectedIndexChanged" AutoPostBack="true"/>
    <div style ="float :right">
        <telerik:RadButton skin="Metro" RenderMode="Lightweight" ID="previous" OnClick ="previous_Click" runat="server" Text="Previous">
            <Icon PrimaryIconCssClass="rbPrevious"></Icon>
        </telerik:RadButton>
    </div>

    <telerik:RadWindowManager ID="rwm" runat="server" Skin="Metro">
        <Windows>
            <telerik:RadWindow runat="server" ID="window1" Skin="Metro">
            </telerik:RadWindow>
            <telerik:RadWindow runat="server" ID="window2" Skin="Metro">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager> 

    <telerik:RadGrid ID="RadGridBoxes" OnItemCommand ="RadGridBoxes_ItemCommand" OnDetailTableDataBind ="RadGrid1_DetailTableDataBind1" OnItemCreated ="RadGridBoxes_ItemCreated" runat="server" AutoGenerateColumns="False" OnNeedDataSource ="RadGrid1_NeedDataSource" PageSize="30" AllowSorting="True" AllowPaging="True" CssClass="RadGridCustomClass" Skin="Metro">   <%--AllowFilteringByColumn="true"--%>
        <PagerStyle Mode="NumericPages"></PagerStyle>  <%--Advanced    Caption="Caption" CssClass="MasterClass"    --%>
        <ClientSettings  EnableRowHoverStyle="true">
            <Resizing AllowColumnResize="true" ShowRowIndicatorColumn ="false" AllowRowResize="True" ResizeGridOnColumnResize="false" ClipCellContentOnResize="false" EnableRealTimeResize="false" AllowResizeToFit="true" />
        </ClientSettings>   

        <MasterTableView  ClientDataKeyNames="tldh_id" DataKeyNames="tldh_id" CommandItemDisplay="Top" InsertItemPageIndexAction="ShowItemOnFirstPage" HeaderStyle-BackColor ="#eed7d7" AllowMultiColumnSorting="True" HierarchyLoadMode="Client">
            <CommandItemSettings ShowAddNewRecordButton="false" />
             
            <DetailTables>
                <telerik:GridTableView ClientDataKeyNames="tldhd_id" Name ="Details" DataKeyNames="tldhd_id" HierarchyLoadMode="Client">
                    <NoRecordsTemplate>
                        <asp:textbox runat="server" width="32.3%" style="visibility :hidden " placeholder="Employee number" TextMode ="Number" text ='<%# Eval("tldd_employeNbr").ToString()%>' id="Textbox1"/>         <%--return radconfirm('Are you sure you want to edit??', confirmCallBackFn); return false;--%>
                        <asp:textbox runat="server" width="31.2%" placeholder="Employee number" TextMode ="Number" text ='<%# Eval("tldd_employeNbr").ToString()%>' id="tldd_employeNbrTXT"/>
                        
                        <telerik:RadButton skin="Metro" ID ="noRecEditBTN" BorderColor ="White" BackColor ="White" SingleClick ="true" runat="server" ToolTip="Edit"  style="float :right " CommandArgument ='<%# Eval("tldhd_id").ToString()%>' Width ="2.7%"  OnClick ="NoRecordsEdit_Click" OnClientClick="RadConfirm">
                                <Icon  PrimaryIconUrl="~/Pages/images/editBTN.png"/>
                        </telerik:RadButton> 
                        <asp:textbox runat="server" width="31%" style="float :right;margin-right :2% " placeholder="Employee name" MaxLength="100" text ='<%# Eval("tldd_employeName").ToString()%>' id="tldd_employeNameTXT"/>
                    </NoRecordsTemplate>

                    <ParentTableRelation>
                        <telerik:GridRelationFields DetailKeyField="tldh_id" MasterKeyField="tldh_id"></telerik:GridRelationFields>
                    </ParentTableRelation> 
                    <Columns>
                        <telerik:GridTemplateColumn AutoPostBackOnFilter ="true" DataType="system.String" ShowFilterIcon="false" HeaderText="SECTION" HeaderStyle-BackColor ="#eed7d7" DataField="tldhd_name" SortExpression="tldhd_name" UniqueName="tldhd_name" >
                            <ItemTemplate>
                                <asp:label runat="server" width="100%" text ='<%# Eval("tldhd_id").ToString()%>' id="tldhd_idLBL" Visible ="false"/>
                                <asp:label runat="server" width="100%" id="tldd_idLBL" Visible ="false"/>
                                <asp:label runat="server" width="100%" text ='<%# Eval("tldhd_name").ToString()%>' id="tldhd_nameLBL"/>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>  
                         
                        <telerik:GridTemplateColumn AutoPostBackOnFilter ="true" DataType="system.String" ShowFilterIcon="false" HeaderText="EMPLOYEE #" HeaderStyle-BackColor ="#eed7d7" DataField="tldd_employeNbr" SortExpression="tldd_employeNbr" UniqueName="tldd_employeNbr" >
                            <ItemTemplate>
                                <asp:textbox runat="server" width="100%" placeholder="Employee number" TextMode ="Number"  id="tldd_employeNbrTXT"/>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn AutoPostBackOnFilter ="true" DataType="system.String" HeaderStyle-BackColor ="#eed7d7" ShowFilterIcon="false" HeaderText="EMPLOYEE NAME" DataField="tldd_employeName" SortExpression="tldd_employeName" UniqueName="tldd_employeName" >
                            <ItemTemplate>
                                <asp:textbox runat="server" width="100%" placeholder="Employee name" MaxLength="100"  id="tldd_employeNameTXT"/>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridButtonColumn HeaderStyle-Width="3%" HeaderStyle-BackColor ="#eed7d7" UniqueName="EditCommandColumn" Text="Edit" CommandName="Update" ButtonType="ImageButton" ConfirmDialogType="RadWindow" ConfirmTitle="Edit" ImageUrl ="~/Pages/images/editBTN.png"/>
                    </Columns>
                    <SortExpressions>
                        <telerik:GridSortExpression FieldName="tldhd_name"></telerik:GridSortExpression>
                    </SortExpressions>
                </telerik:GridTableView>
            </DetailTables>
            <Columns>
                <telerik:GridTemplateColumn AutoPostBackOnFilter ="true" AllowFiltering ="false" ShowFilterIcon="false" HeaderText="SECTION" HeaderStyle-BackColor ="#eed7d7" DataField="tldh_name" SortExpression="tldh_name" UniqueName="tldh_name" >
                    <ItemTemplate>
                        <asp:label runat="server" width="100%" text ='<%# Eval("tldh_name").ToString()%>' id="tldh_nameLBL"/>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>  
            </Columns>
            <SortExpressions>
                <telerik:GridSortExpression FieldName="tldh_name" />
            </SortExpressions>
        </MasterTableView>
    </telerik:RadGrid> 

    <br />

    <telerik:RadGrid ID="RadGrid1" OnSelectedIndexChanged="RadGrid1_SelectedIndexChanged" onItemDataBound="RadGrid1_ItemDataBound" OnNeedDataSource="RadGridBoxes_NeedDataSource1" Skin="Metro" AllowFilteringByColumn="false" runat="server" AllowPaging="True" CssClass="RadGridCustomClass" AllowSorting="True" PageSize="30">
        <ClientSettings  EnableRowHoverStyle="true">
            <Resizing AllowColumnResize="true" ShowRowIndicatorColumn ="false" AllowRowResize="True" ResizeGridOnColumnResize="false" ClipCellContentOnResize="false" EnableRealTimeResize="false" AllowResizeToFit="true" />
        </ClientSettings> 
        <ClientSettings Selecting-AllowRowSelect="true" EnablePostBackOnRowClick="true"/>
         
        <PagerStyle Mode="Advanced" ></PagerStyle> 

        <MasterTableView AutoGenerateColumns="false" ShowHeadersWhenNoRecords="true" DataKeyNames="tldReagent_id" CommandItemDisplay="Top" EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnFirstPage" Width="100%" >
            <CommandItemSettings ShowAddNewRecordButton ="false" />

            <Columns> 

                <telerik:GridTemplateColumn HeaderStyle-Width="10%" DataType="system.String" AutoPostBackOnFilter ="true" ShowFilterIcon="false" HeaderText="#" DataField="tldReagent_minNbr" SortExpression="tldReagent_minNbr" UniqueName="tldReagent_minNbr" >
                    <ItemTemplate>
                        <asp:Label runat="server" width="49%" text ='<%# Eval("tldReagent_minNbr").ToString()%>' id="tldReagent_minNbrLBL"/>
                    </ItemTemplate> 
                </telerik:GridTemplateColumn>

                <telerik:GridTemplateColumn HeaderStyle-Width="3%" AutoPostBackOnFilter ="true" ShowFilterIcon="false" AllowFiltering ="false" >
                    <ItemTemplate>
                        --->
                    </ItemTemplate> 
                </telerik:GridTemplateColumn>

                <telerik:GridTemplateColumn HeaderStyle-Width="10%" DataType="system.String" AutoPostBackOnFilter ="true" ShowFilterIcon="false" HeaderText="#" DataField="tldReagent_maxNbr" SortExpression="tldReagent_maxNbr" UniqueName="tldReagent_maxNbr" >
                    <ItemTemplate>
                        <asp:Label runat="server" width="49%" text ='<%# Eval("tldReagent_maxNbr").ToString()%>' id="tldReagent_maxNbrLBL"/>
                    </ItemTemplate> 
                </telerik:GridTemplateColumn>

                <telerik:GridTemplateColumn HeaderStyle-Width="10%" DataType="system.String" AutoPostBackOnFilter ="true" ShowFilterIcon="false" HeaderText="INSTEAD OF INSURANCE" DataField="tldReagent_InsteadOfInsurance" SortExpression="tldReagent_InsteadOfInsurance" UniqueName="tldReagent_InsteadOfInsurance" >
                    <ItemTemplate>  
                        <asp:Label ID="tldReagent_InsteadOfInsuranceLBL" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "tldReagent_InsteadOfInsurance").ToString()%>'/>
                    </ItemTemplate> 
                </telerik:GridTemplateColumn>  

            </Columns>

        </MasterTableView>
    </telerik:RadGrid> 

    <br />

    <telerik:RadGrid ID="RadGridBoxes2" OnNeedDataSource="RadGridBoxes2_NeedDataSource1" onItemDataBound="RadGridBoxes2_ItemDataBound"  OnSelectedIndexChanged="RadGridBoxes2_SelectedIndexChanged" Skin="Metro" AllowFilteringByColumn="false" runat="server" AllowPaging="True" CssClass="RadGridCustomClass" AllowSorting="True" PageSize="30">
        <ClientSettings  EnableRowHoverStyle="true">
            <Resizing AllowColumnResize="true" ShowRowIndicatorColumn ="false" AllowRowResize="True" ResizeGridOnColumnResize="false" ClipCellContentOnResize="false" EnableRealTimeResize="false" AllowResizeToFit="true" />
        </ClientSettings> 
        <ClientSettings Selecting-AllowRowSelect="true" EnablePostBackOnRowClick="true"/>
         
        <PagerStyle Mode="Advanced" ></PagerStyle> 

        <MasterTableView AutoGenerateColumns="false" ShowHeadersWhenNoRecords="true" DataKeyNames="tldReagent_id" CommandItemDisplay="Top" EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnFirstPage" Width="100%" >
            <CommandItemSettings AddNewRecordText="Add" ShowAddNewRecordButton ="false"/>

            <Columns> 

                <telerik:GridTemplateColumn HeaderStyle-Width="10%" AutoPostBackOnFilter ="true" ShowFilterIcon="false" HeaderText="#" DataField="tldReagent_minNbr" SortExpression="tldReagent_minNbr" UniqueName="tldReagent_minNbr" >
                    <ItemTemplate>
                        <asp:Label runat="server" width="49%" text ='<%# Eval("tldReagent_minNbr").ToString()%>' id="tldReagent_minNbrLBL2"/>
                    </ItemTemplate> 
                </telerik:GridTemplateColumn>

                <telerik:GridTemplateColumn HeaderStyle-Width="3%" AllowFiltering ="false" AutoPostBackOnFilter ="true" ShowFilterIcon="false" >
                    <ItemTemplate>
                        --->
                    </ItemTemplate> 
                </telerik:GridTemplateColumn>

                <telerik:GridTemplateColumn HeaderStyle-Width="10%" AutoPostBackOnFilter ="true" ShowFilterIcon="false" HeaderText="#" DataField="tldReagent_maxNbr" SortExpression="tldReagent_maxNbr" UniqueName="tldReagent_maxNbr" >
                    <ItemTemplate>
                        <asp:Label runat="server" width="49%" text ='<%# Eval("tldReagent_maxNbr").ToString()%>' id="tldReagent_maxNbrLBL2"/>
                    </ItemTemplate> 
                </telerik:GridTemplateColumn>

                <telerik:GridTemplateColumn HeaderStyle-Width="10%" AutoPostBackOnFilter ="true" ShowFilterIcon="false" HeaderText="INSTEAD OF INSURANCE" DataField="tldReagent_InsteadOfInsurance" SortExpression="tldReagent_InsteadOfInsurance" UniqueName="tldReagent_InsteadOfInsurance" >
                    <ItemTemplate>  
                        <asp:Label ID="tldReagent_InsteadOfInsuranceLBL2" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "tldReagent_InsteadOfInsurance").ToString()%>'/>
                    </ItemTemplate> 
                </telerik:GridTemplateColumn> 
                
                <telerik:GridTemplateColumn HeaderStyle-Width="10%" AutoPostBackOnFilter ="true" ShowFilterIcon="false" HeaderText="PART" DataField="tldReagent_Part" SortExpression="tldReagent_Part" UniqueName="tldReagent_Part" >
                    <ItemTemplate>  
                        <asp:Label ID="tldReagent_PartLBL" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "tldReagent_Part").ToString()%>'/>
                    </ItemTemplate> 
                </telerik:GridTemplateColumn>  

            </Columns>

        </MasterTableView>
    </telerik:RadGrid>

    <br />
    <telerik:RadButton skin="Metro" ID="SendBTN" runat="server" Text="Send" width="110"   Height ="35" CssClass="SendBtn" SingleClick ="true" OnClientClicking="RadConfirm" OnClick="SendBTN_Click"/>
</asp:Content>

