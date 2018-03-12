<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Pages/MasterPage.master" CodeFile="TLDReagent.aspx.cs" Inherits="Pages_TLD_TLDReagent" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="Server">
    <title>TLD Reagent
    </title>
    <style>
        .RadGrid_Metro th.rgSorted {
            background-color: #eed7d7 !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <telerik:RadWindowManager ID="rwm" runat="server" Skin="Metro">
        <Windows>
            <telerik:RadWindow runat="server" ID="window1" Skin="Metro">
            </telerik:RadWindow>
            <telerik:RadWindow runat="server" ID="window2" Skin="Metro">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>

    <%-- <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadGridBoxes">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGridBoxes"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting> 
        </AjaxSettings>
    </telerik:RadAjaxManager>--%>

    <telerik:RadGrid ID="RadGridBoxes" OnNeedDataSource="RadGridBoxes_NeedDataSource1" OnItemCommand="RadGridBoxes_ItemCommand" Skin="Metro" AllowFilteringByColumn="false" runat="server" AllowPaging="True" CssClass="RadGridCustomClass" AllowSorting="True" PageSize="30">
        <ClientSettings EnableRowHoverStyle="true">
            <Resizing AllowColumnResize="true" ShowRowIndicatorColumn="false" AllowRowResize="True" ResizeGridOnColumnResize="false" ClipCellContentOnResize="false" EnableRealTimeResize="false" AllowResizeToFit="true" />
        </ClientSettings>

        <PagerStyle Mode="Advanced"></PagerStyle>

        <MasterTableView AutoGenerateColumns="false" ShowHeadersWhenNoRecords="true" DataKeyNames="tldReagent_id" CommandItemDisplay="Top" EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnFirstPage" Width="100%">
            <CommandItemSettings AddNewRecordText="Add" />

            <Columns>

                <telerik:GridTemplateColumn HeaderStyle-Width="10%" DataType="system.String" AutoPostBackOnFilter="true" ShowFilterIcon="false" HeaderText="#" DataField="tldReagent_minNbr" SortExpression="tldReagent_minNbr" UniqueName="tldReagent_minNbr">
                    <ItemTemplate>
                        <asp:Label runat="server" Width="49%" Text='<%# Eval("tldReagent_minNbr").ToString()%>' ID="tldReagent_minNbrLBL" />
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox runat="server" Width="100%" placeholder="Min #" TextMode="Number" ID="tldReagent_minNbrTXT" Text='<%# Eval("tldReagent_minNbr").ToString()%>' />
                        <asp:RequiredFieldValidator runat="server" ID="tldReagent_minNbrRFV" ForeColor="Red" Text="Required." ControlToValidate="tldReagent_minNbrTXT" />
                        <asp:RegularExpressionValidator ID="tldReagent_minNbrREV" runat="server" ErrorMessage="This entry can only contain 3-digits." ForeColor="red" ControlToValidate="tldReagent_minNbrTXT" ValidationExpression="^[0-9][0-9]{0,2}$" SetFocusOnError="true" />
                    </EditItemTemplate>
                </telerik:GridTemplateColumn>

                <telerik:GridTemplateColumn HeaderStyle-Width="3%" AutoPostBackOnFilter="true" AllowFiltering="false" ShowFilterIcon="false">
                    <ItemTemplate>
                        --->
                    </ItemTemplate>
                    <EditItemTemplate>
                        --->
                    </EditItemTemplate>
                </telerik:GridTemplateColumn>

                <telerik:GridTemplateColumn HeaderStyle-Width="10%" DataType="system.String" AutoPostBackOnFilter="true" ShowFilterIcon="false" HeaderText="#" DataField="tldReagent_maxNbr" SortExpression="tldReagent_maxNbr" UniqueName="tldReagent_maxNbr">
                    <ItemTemplate>
                        <asp:Label runat="server" Width="49%" Text='<%# Eval("tldReagent_maxNbr").ToString()%>' ID="tldReagent_maxNbrLBL" />
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox runat="server" Width="100%" placeholder="Max #" MaxLength="100" ID="tldReagent_maxNbrTXT" Text='<%# Eval("tldReagent_minNbr").ToString()%>' />
                        <asp:RequiredFieldValidator runat="server" ID="tldReagent_maxNbrRFV" ForeColor="Red" Text="Required." ControlToValidate="tldReagent_maxNbrTXT" />
                        <asp:Label runat="server" Width="49%" Text='*' class="hidden"  />
                        <%--<asp:RegularExpressionValidator ID="tldReagent_maxNbrREV" Enabled ="false" runat="server" ErrorMessage="This entry can only contain 3-digits." ForeColor ="red" ValidationExpression="^[0-9][0-9]{0,2}$" SetFocusOnError="true"/>--%>
                    </EditItemTemplate>
                </telerik:GridTemplateColumn>

                <telerik:GridTemplateColumn HeaderStyle-Width="10%" DataType="system.String" AutoPostBackOnFilter="true" ShowFilterIcon="false" HeaderText="INSTEAD OF INSURANCE" DataField="tldReagent_InsteadOfInsurance" SortExpression="tldReagent_InsteadOfInsurance" UniqueName="tldReagent_InsteadOfInsurance">
                    <ItemTemplate>
                        <asp:Label ID="tldReagent_InsteadOfInsuranceLBL" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "tldReagent_InsteadOfInsurance").ToString()%>' />
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox runat="server" Width="100%" MaxLength="10" placeholder="Instead of insurance" ID="tldReagent_InsteadOfInsuranceTXT" Text='<%# Eval("tldReagent_InsteadOfInsurance").ToString()%>' />
                        <asp:RequiredFieldValidator runat="server" ID="tldReagent_InsteadOfInsuranceRFV" ForeColor="Red" Text="Required." ControlToValidate="tldReagent_InsteadOfInsuranceTXT" />
                        <asp:RegularExpressionValidator ID="tldReagent_InsteadOfInsuranceREV" runat="server" ErrorMessage="This entry can only contain numbers." ForeColor="red" ControlToValidate="tldReagent_InsteadOfInsuranceTXT" ValidationExpression="^[+]?[0-9]{0,10}([.][0-9]{1,3})?$" SetFocusOnError="true" />
                    </EditItemTemplate>
                </telerik:GridTemplateColumn>

                <telerik:GridEditCommandColumn ButtonType="ImageButton" HeaderStyle-Width="3%" UniqueName="EditCommandColumn" />

                <telerik:GridButtonColumn HeaderStyle-Width="3%" UniqueName="delete" Text="Delete" CommandName="Delete" ButtonType="ImageButton" ConfirmText="Are you sure you want to delete?" ConfirmDialogType="RadWindow" ConfirmTitle="Delete" />

            </Columns>

        </MasterTableView>
    </telerik:RadGrid>

    <br />

    <telerik:RadGrid ID="RadGridBoxes2" OnNeedDataSource="RadGridBoxes2_NeedDataSource1" OnItemCommand="RadGridBoxes2_ItemCommand" Skin="Metro" AllowFilteringByColumn="false" runat="server" AllowPaging="True" CssClass="RadGridCustomClass" AllowSorting="True" PageSize="30">
        <ClientSettings EnableRowHoverStyle="true">
            <Resizing AllowColumnResize="true" ShowRowIndicatorColumn="false" AllowRowResize="True" ResizeGridOnColumnResize="false" ClipCellContentOnResize="false" EnableRealTimeResize="false" AllowResizeToFit="true" />
        </ClientSettings>

        <PagerStyle Mode="Advanced"></PagerStyle>

        <MasterTableView AutoGenerateColumns="false" ShowHeadersWhenNoRecords="true" DataKeyNames="tldReagent_id" CommandItemDisplay="Top" EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnFirstPage" Width="100%">
            <CommandItemSettings AddNewRecordText="Add" />

            <Columns>

                <telerik:GridTemplateColumn HeaderStyle-Width="10%" AutoPostBackOnFilter="true" ShowFilterIcon="false" HeaderText="#" DataField="tldReagent_minNbr" SortExpression="tldReagent_minNbr" UniqueName="tldReagent_minNbr">
                    <ItemTemplate>
                        <asp:Label runat="server" Width="49%" Text='<%# Eval("tldReagent_minNbr").ToString()%>' ID="tldReagent_minNbrLBL2" />
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox runat="server" Width="100%" placeholder="Min #" TextMode="Number" ID="tldReagent_minNbrTXT2" Text='<%# Eval("tldReagent_minNbr").ToString()%>' />
                        <asp:RequiredFieldValidator runat="server" ID="tldReagent_minNbrRFV2" ForeColor="Red" Text="Required." ControlToValidate="tldReagent_minNbrTXT2" />
                        <asp:Label runat="server" Width="49%" Text='*' class="hidden"  />
                        <%--<asp:RegularExpressionValidator ID="tldReagent_minNbrREV2" runat="server" ErrorMessage="This entry can only contain 3-digits." ForeColor="red" ControlToValidate="tldReagent_minNbrTXT2" ValidationExpression="^[0-9][0-9]{0,2}$" SetFocusOnError="true" />--%>
                    </EditItemTemplate>
                </telerik:GridTemplateColumn>

                <telerik:GridTemplateColumn HeaderStyle-Width="3%" AllowFiltering="false" AutoPostBackOnFilter="true" ShowFilterIcon="false">
                    <ItemTemplate>
                        --->
                    </ItemTemplate>
                    <EditItemTemplate>
                        --->
                    </EditItemTemplate>
                </telerik:GridTemplateColumn>

                <telerik:GridTemplateColumn HeaderStyle-Width="10%" AutoPostBackOnFilter="true" ShowFilterIcon="false" HeaderText="#" DataField="tldReagent_maxNbr" SortExpression="tldReagent_maxNbr" UniqueName="tldReagent_maxNbr">
                    <ItemTemplate>
                        <asp:Label runat="server" Width="49%" Text='<%# Eval("tldReagent_maxNbr").ToString()%>' ID="tldReagent_maxNbrLBL2" />
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox runat="server" Width="100%" placeholder="Max #" MaxLength="100" ID="tldReagent_maxNbrTXT2" Text='<%# Eval("tldReagent_minNbr").ToString()%>' />
                        <asp:RequiredFieldValidator runat="server" ID="tldReagent_maxNbrRFV2" ForeColor="Red" Text="Required." ControlToValidate="tldReagent_maxNbrTXT2" />
                        <asp:Label runat="server" Width="49%" Text='*' class="hidden"  />
                        <%--<asp:RegularExpressionValidator ID="tldReagent_maxNbrREV2" runat="server" ErrorMessage="This entry can only contain 3-digits." ForeColor="red" Enabled="false" ValidationExpression="^[0-9][0-9]{0,2}$" SetFocusOnError="true" />--%>
                    </EditItemTemplate>
                </telerik:GridTemplateColumn>

                <telerik:GridTemplateColumn HeaderStyle-Width="10%" AutoPostBackOnFilter="true" ShowFilterIcon="false" HeaderText="INSTEAD OF INSURANCE" DataField="tldReagent_InsteadOfInsurance" SortExpression="tldReagent_InsteadOfInsurance" UniqueName="tldReagent_InsteadOfInsurance">
                    <ItemTemplate>
                        <asp:Label ID="tldReagent_InsteadOfInsuranceLBL2" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "tldReagent_InsteadOfInsurance").ToString()%>' />
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox runat="server" Width="100%" MaxLength="10" placeholder="Instead of insurance" ID="tldReagent_InsteadOfInsuranceTXT2" Text='<%# Eval("tldReagent_InsteadOfInsurance").ToString()%>' />
                        <asp:RequiredFieldValidator runat="server" ID="tldReagent_InsteadOfInsuranceRFV2" ForeColor="Red" Text="Required." ControlToValidate="tldReagent_InsteadOfInsuranceTXT2" />
                        <asp:RegularExpressionValidator ID="tldReagent_InsteadOfInsuranceREV2" runat="server" ErrorMessage="This entry can only contain numbers." ForeColor="red" ControlToValidate="tldReagent_InsteadOfInsuranceTXT2" ValidationExpression="^[+]?[0-9]{0,10}([.][0-9]{1,3})?$" SetFocusOnError="true" />
                    </EditItemTemplate>
                </telerik:GridTemplateColumn>

                <telerik:GridTemplateColumn HeaderStyle-Width="10%" AutoPostBackOnFilter="true" ShowFilterIcon="false" HeaderText="PART" DataField="tldReagent_Part" SortExpression="tldReagent_Part" UniqueName="tldReagent_Part">
                    <ItemTemplate>
                        <asp:Label ID="tldReagent_PartLBL" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "tldReagent_Part").ToString()%>' />
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox runat="server" Width="100%" MaxLength="100" placeholder="Part" ID="tldReagent_PartTXT" Text='<%# Eval("tldReagent_Part").ToString()%>' />
                        <asp:RequiredFieldValidator runat="server" ID="tldReagent_PartRFV" ForeColor="Red" Text="Required." ControlToValidate="tldReagent_PartTXT" />
                    </EditItemTemplate>
                </telerik:GridTemplateColumn>

                <telerik:GridEditCommandColumn ButtonType="ImageButton" HeaderStyle-Width="3%" UniqueName="EditCommandColumn" />

                <telerik:GridButtonColumn HeaderStyle-Width="3%" UniqueName="delete" Text="Delete" CommandName="Delete" ButtonType="ImageButton" ConfirmText="Are you sure you want to delete?" ConfirmDialogType="RadWindow" ConfirmTitle="Delete" />

            </Columns>

        </MasterTableView>
    </telerik:RadGrid>
</asp:Content>
