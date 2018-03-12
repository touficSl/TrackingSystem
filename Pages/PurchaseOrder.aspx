<%@ Page Language="C#" validateRequest="false" MasterPageFile="~/Pages/MasterPage.master" AutoEventWireup="true" EnableEventValidation="true"  CodeFile="PurchaseOrder.aspx.cs" Inherits="PurchaseOrder" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:content ID="Content2" ContentPlaceHolderID="head" Runat="Server">
    <title >
        List of Requests
    </title> 
    <script type = "text/javascript">
        //function Confirm() {
        //    var confirm_value = document.createElement("INPUT");
        //    confirm_value.type = "hidden";
        //    confirm_value.name = "confirm_value";
        //    if (confirm("Do you want to save data?")) {
        //        confirm_value.value = "Yes";
        //    } else {
        //        confirm_value.value = "No";
        //    }
        //    document.forms[0].appendChild(confirm_value);
        //}

        function CheckedChange(objCheckBox) {
            if (confirm('Are you sure?')) {
                __doPostBack("'" + objCheckBox.id + "'", '');
                return true;
            }
            else {
                objCheckBox.checked = false;
                return false;
            }
        }
    </script>
</asp:content> 
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" Runat="Server">
    <div style ="float:right "> 
        <telerik:RadButton skin="Metro" RenderMode="Lightweight" ID="refresh" runat="server" Text="Refresh" OnClick ="refresh_Click">
            <Icon SecondaryIconCssClass="rbRefresh"></Icon>
        </telerik:RadButton >
    </div>

    <telerik:RadComboBox  runat ="server" Visible="false" ID="langRCB" Width="50" Filter="Contains" EnableVirtualScrolling="true" DropDownHeight="150px" DataTextField="language_name" DataValueField ="language_id" OnSelectedIndexChanged ="langRCB_SelectedIndexChanged" AutoPostBack="true"/>
    <telerik:RadWindowManager ID="rwm" runat="server" Skin="Metro">
        <Windows>
            <telerik:RadWindow runat="server" ID="window1" Skin="Metro">
            </telerik:RadWindow>
            <telerik:RadWindow runat="server" ID="window2" Skin="Metro">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>  
                         
	<telerik:RadGrid ID="RadGridBoxes" OnItemDataBound="RadGridBoxes_ItemDataBound" OnSelectedIndexChanged="RadGridBoxes_SelectedIndexChanged" OnNeedDataSource="RadGridBoxes_NeedDataSource1" OnItemCommand ="RadGridBoxes_ItemCommand" OnItemCreated ="RadGridBoxes_ItemCreated" Skin="Metro" AllowFilteringByColumn="true" runat="server" AllowPaging="True" CssClass="RadGridCustomClass" AllowSorting="True" PageSize="15">
        <ClientSettings  EnableRowHoverStyle="true">
            <Resizing AllowColumnResize="true" ShowRowIndicatorColumn ="false" AllowRowResize="True" ResizeGridOnColumnResize="true" ClipCellContentOnResize="false" EnableRealTimeResize="false" AllowResizeToFit="true" />
        </ClientSettings> 
        <ClientSettings>
            <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true" FrozenColumnsCount="2"></Scrolling>
        </ClientSettings>
        <ClientSettings Selecting-AllowRowSelect="true" EnablePostBackOnRowClick="true"/>

        <PagerStyle Mode="Advanced" ></PagerStyle> 

        <MasterTableView AutoGenerateColumns="false" ShowHeadersWhenNoRecords="true" CssClass="MasterClass" DataKeyNames="po_id" CommandItemDisplay="Top" EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnFirstPage" Width="100%" >
            <CommandItemSettings AddNewRecordText="Add" />

            <Columns> 
                <telerik:GridTemplateColumn HeaderStyle-Width="10%" DataType="system.String" AutoPostBackOnFilter ="true" ShowFilterIcon="false" HeaderText="Contact Person" DataField="user_contactPerson" SortExpression="user_contactPerson" UniqueName="user_contactPerson" >
                    <ItemTemplate>
                        <asp:Label runat="server" width="100%" id="user_contactPersonLBL" text ='<%# Eval("user_contactPerson").ToString()%>' />
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:Label runat="server" width="100%" text ='<%# Eval("user_contactPerson").ToString()%>' />
                    </EditItemTemplate>
                </telerik:GridTemplateColumn>

                <telerik:GridTemplateColumn HeaderStyle-Width="10%" DataType="system.String" Visible ="false"  AutoPostBackOnFilter ="true" ShowFilterIcon="false">
                    <ItemTemplate>
                        <asp:Label runat="server" width="100%" id="user_emailLBL" text ='<%# Eval("user_email").ToString()%>' />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>

                <telerik:GridTemplateColumn HeaderStyle-Width="10%" DataType="system.String" AutoPostBackOnFilter ="true" ShowFilterIcon="false" HeaderText="INSTITUTION" DataField="user_companyName" SortExpression="user_companyName" UniqueName="user_companyName" >
                    <ItemTemplate>
                        <asp:Label runat="server" width="100%"  text ='<%# Eval("user_companyName").ToString()%>' />
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:Label runat="server" width="100%" text ='<%# Eval("user_companyName").ToString()%>' />
                    </EditItemTemplate>
                </telerik:GridTemplateColumn>

                <telerik:GridTemplateColumn HeaderStyle-Width="10%" DataType="system.String" AutoPostBackOnFilter ="true" ShowFilterIcon="false" HeaderText="REQUEST #" DataField="po_number" SortExpression="po_number" UniqueName="po_number" >
                    <ItemTemplate>
                        <asp:Label runat="server" width="100%" text ='<%# Eval("po_number").ToString()%>' id="po_numberLBL"/>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox runat="server" width="100%" placeholder="Request #" MaxLength ="100" id="po_numberTXT" text ='<%# Eval("po_number").ToString()%>'/>
                        <%--<asp:RequiredFieldValidator runat ="server" ID="po_numberRFV" ForeColor ="Red" text="Required." ControlToValidate ="po_numberTXT"/>--%>
                    </EditItemTemplate>
                </telerik:GridTemplateColumn>

                <telerik:GridTemplateColumn HeaderStyle-Width="10%" DataType="system.String" AutoPostBackOnFilter ="true" ShowFilterIcon="false" HeaderText="OFFICIAL #" DataField="official_number" SortExpression="official_number" UniqueName="official_number" >
                    <ItemTemplate>
                        <asp:Label runat="server" width="100%" text ='<%# Eval("official_number").ToString()%>' id="official_numberLBL"/>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox runat="server" width="100%" placeholder="official" id="official_numberTXT" MaxLength ="100" text ='<%# Eval("official_number").ToString()%>'/>
                        <%--<asp:RegularExpressionValidator ID="official_numberREV" runat="server" ErrorMessage="This entry can only contain 6-digits." ForeColor ="red" ControlToValidate="official_numberTXT" ValidationExpression="^[0-9][0-9]{0,5}$" SetFocusOnError="true"/>--%>
                    </EditItemTemplate>
                </telerik:GridTemplateColumn>

                <telerik:GridTemplateColumn HeaderStyle-Width="10%" DataType="system.String" AutoPostBackOnFilter ="true" ShowFilterIcon="false" HeaderText="ITEM" DataField="item_name" SortExpression="item_name" UniqueName="item_name" >
                    <ItemTemplate> 
                        <asp:Label ID="item_idLBL" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "item_id").ToString()%>' Visible ="false"/>
                        <asp:Label ID="item_nameLBL" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "item_name").ToString()%>'/>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:Label ID="item_idLBL1" runat="server" width="100%" text='<%# DataBinder.Eval(Container.DataItem, "item_name").ToString()%>'/>
                        <asp:Label ID="startLBL" runat="server" text=""/>
                        <telerik:RadComboBox ID="item_idRCB" runat="server" Width="98%" Filter="Contains" DataTextField="item_name" DataValueField="item_id" EnableVirtualScrolling="true" DropDownHeight="200px"/>
                        <asp:RequiredFieldValidator runat ="server" ID="item_idRFV" ForeColor ="Red" text="Required." ControlToValidate ="item_idRCB"/>
                    </EditItemTemplate>
                </telerik:GridTemplateColumn>

               <%-- <telerik:GridTemplateColumn HeaderStyle-Width="10%" DataType="system.String" AutoPostBackOnFilter ="true" ShowFilterIcon="false" HeaderText="ADDRESS" DataField="po_address" SortExpression="po_address" UniqueName="po_address" >
                    <ItemTemplate> 
                        <asp:Label ID="po_addressLBL" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "po_address").ToString()%>'/>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:textbox ID="po_addressTXT" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "po_address").ToString()%>'/>
                        <asp:RequiredFieldValidator runat ="server" ID="po_addressRFV" ForeColor ="Red" text="Required." ControlToValidate ="po_addressTXT"/>
                    </EditItemTemplate>
                </telerik:GridTemplateColumn>--%>

                <telerik:GridTemplateColumn HeaderStyle-Width="10%" DataType="system.String" AutoPostBackOnFilter ="true" ShowFilterIcon="false" HeaderText="ADDRESS" DataField="user_address" SortExpression="user_address" UniqueName="user_address" >
                    <ItemTemplate> 
                        <asp:Label ID="user_addressLBL" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "user_address").ToString()%>'/>
                    </ItemTemplate>
                    <%--<EditItemTemplate>
                        <asp:textbox ID="po_addressTXT" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "po_address").ToString()%>'/>
                        <asp:RequiredFieldValidator runat ="server" ID="po_addressRFV" ForeColor ="Red" text="Required." ControlToValidate ="po_addressTXT"/>
                    </EditItemTemplate>--%>
                </telerik:GridTemplateColumn>

                <%--<telerik:GridTemplateColumn HeaderStyle-Width="10%" DataType="system.String" AutoPostBackOnFilter ="true" ShowFilterIcon="false" HeaderText="CONTACT PERSON" DataField="po_contactPerson" SortExpression="po_contactPerson" UniqueName="po_contactPerson" >
                    <ItemTemplate> 
                        <asp:Label ID="po_contactPersonLBL" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "po_contactPerson").ToString()%>' />
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:textbox placeholder="Contact person" MaxLength ="100" ID="po_contactPersonTXT" runat="server" width="100%" text='<%# DataBinder.Eval(Container.DataItem, "po_contactPerson").ToString()%>'/>
                        <asp:RequiredFieldValidator runat ="server" ID="po_contactPersonRFV" ForeColor ="Red" text="Required." ControlToValidate ="po_contactPersonTXT"/>
                    </EditItemTemplate>
                </telerik:GridTemplateColumn>--%>

                <telerik:GridTemplateColumn HeaderStyle-Width="10%" DataType="system.String" AutoPostBackOnFilter ="true" ShowFilterIcon="false" HeaderText="DATE" DataField="po_date" SortExpression="po_date" UniqueName="po_date" >
                    <ItemTemplate>
                        <asp:Label runat="server" width="100%" text ='<%# Controller.ToShortDateString((DateTime)DataBinder.Eval(Container.DataItem, "po_date"))%>' id="po_dateLBL"/>
                    </ItemTemplate>

                    <EditItemTemplate>
                        <telerik:RadDatePicker runat="server" placeholder="Date" width="90%" id="po_dateRDPTXT" SelectedDate ='<%# Controller.FixDate(DataBinder.Eval(Container.DataItem, "po_date").ToString())%>'/>
                        <asp:RequiredFieldValidator runat ="server" width="10%" ID="po_dateRFV" ForeColor ="Red" text="Required." ControlToValidate ="po_dateRDPTXT"/>
                    </EditItemTemplate>

                    <%--<FilterTemplate>
                        <telerik:RadDatePicker RenderMode="Lightweight" ID="po_dateRDPF" runat="server" Width="100%" MinDate="07-04-1996"
                            MaxDate="05-06-1998" FocusedDate="07-04-1996" ClientEvents-OnDateSelected="DateSelected"
                            DbSelectedDate='<%# SetPoDate(Container) %>' />
                        <telerik:RadScriptBlock ID="RadScriptBlock2" runat="server">
                            <script type="text/javascript">
                                function DateSelected(sender, args) {
                                    var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
 
                                var date = FormatSelectedDate(sender);
 
                                tableView.filter("po_date", date, "EqualTo");
                            }
                            function FormatSelectedDate(picker) {
                                var date = picker.get_selectedDate();
                                var dateInput = picker.get_dateInput();
                                var formattedDate = dateInput.get_dateFormatInfo().FormatDate(date, dateInput.get_displayDateFormat());
 
                                return formattedDate;
                            }
                            </script>
                        </telerik:RadScriptBlock>
                    </FilterTemplate>--%>
                </telerik:GridTemplateColumn> 

                <telerik:GridTemplateColumn HeaderStyle-Width="10%" DataType="system.String" HeaderText ="RECIEVED DATE" AutoPostBackOnFilter ="true" ShowFilterIcon="false" DataField="createdOn" SortExpression="createdOn" UniqueName="createdOn">
                    <ItemTemplate>
                        <%# Eval("createdOn").ToString()%>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                
                <telerik:GridTemplateColumn HeaderStyle-Width="8%" DataType="System.String" AutoPostBackOnFilter ="true" ShowFilterIcon="false" HeaderText="SECTRETARY ACCEPTED DATE" DataField="po_date_secretary" SortExpression="po_date_secretary" UniqueName="po_date_secretary" >
                    <ItemTemplate> 
                        <%# DataBinder.Eval(Container.DataItem, "po_date_secretary").ToString()%>
                    </ItemTemplate>
                    <EditItemTemplate></EditItemTemplate>
                    <%--<FilterTemplate>
                        <telerik:RadDatePicker RenderMode="Lightweight" ID="po_date_secretaryRDP" runat="server" Width="100%" MinDate="01-04-1996"
                            MaxDate="1-12-2200" FocusedDate="07-04-1996" ClientEvents-OnDateSelected="DateSelected" DbSelectedDate='<%# SetPoDate(Container) %>' />
                        <telerik:RadScriptBlock  runat="server">
                            <script type="text/javascript">
                                function DateSelected(sender, args) {
                                    var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
 
                                var date = FormatSelectedDate(sender);
 
                                tableView.filter("po_date_secretary", date, "EqualTo");
                            }
                            function FormatSelectedDate(picker) {
                                var date = picker.get_selectedDate();
                                var dateInput = picker.get_dateInput();
                                var formattedDate = dateInput.get_dateFormatInfo().FormatDate(date, dateInput.get_displayDateFormat());
 
                                return formattedDate;
                            }
                            </script>
                        </telerik:RadScriptBlock>
                    </FilterTemplate>--%>
                </telerik:GridTemplateColumn>
                
                <telerik:GridTemplateColumn HeaderStyle-Width="8%" DataType="System.String" AutoPostBackOnFilter ="true" ShowFilterIcon="false" HeaderText="DIRECTOR ACCEPTED DATE" DataField="po_date_director" SortExpression="po_date_director" UniqueName="po_date_director" >
                    <ItemTemplate> 
                        <%# DataBinder.Eval(Container.DataItem, "po_date_director").ToString()%>
                    </ItemTemplate>
                    <EditItemTemplate></EditItemTemplate>
                   <%-- <FilterTemplate>
                         <telerik:RadDatePicker RenderMode="Lightweight" ID="po_date_directorRDP" runat="server" Width="100%" MinDate="01-04-1996"
                            MaxDate="1-12-2200" FocusedDate="07-04-1996" ClientEvents-OnDateSelected="DateSelected" DbSelectedDate='<%# SetPoDate(Container) %>' />
                        <telerik:RadScriptBlock  runat="server">
                            <script type="text/javascript">
                                function DateSelected(sender, args) {
                                    var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
 
                                var date = FormatSelectedDate(sender);
 
                                tableView.filter("po_date_director", date, "EqualTo");
                            }
                            function FormatSelectedDate(picker) {
                                var date = picker.get_selectedDate();
                                var dateInput = picker.get_dateInput();
                                var formattedDate = dateInput.get_dateFormatInfo().FormatDate(date, dateInput.get_displayDateFormat());
 
                                return formattedDate;
                            }
                            </script>
                        </telerik:RadScriptBlock>
                    </FilterTemplate>--%>
                </telerik:GridTemplateColumn>

                <telerik:GridTemplateColumn HeaderStyle-Width="8%" DataType="System.String" AutoPostBackOnFilter ="true" ShowFilterIcon="false" HeaderText="HEAD OF DEPARTMENT ACCEPTED DATE" DataField="po_date_headOfDepartment" SortExpression="po_date_headOfDepartment" UniqueName="po_date_headOfDepartment" >
                    <ItemTemplate> 
                        <%# DataBinder.Eval(Container.DataItem, "po_date_headOfDepartment").ToString()%>
                    </ItemTemplate>
                    <EditItemTemplate></EditItemTemplate>
                    <%--<FilterTemplate>
                         <telerik:RadDatePicker RenderMode="Lightweight" ID="po_date_headOfDepartmentRDP" runat="server" Width="100%" MinDate="01-04-1996"
                            MaxDate="1-12-2200" FocusedDate="07-04-1996" ClientEvents-OnDateSelected="DateSelected" DbSelectedDate='<%# SetPoDate(Container) %>' />
                        <telerik:RadScriptBlock runat="server">
                            <script type="text/javascript">
                                function DateSelected(sender, args) {
                                    var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
 
                                var date = FormatSelectedDate(sender);
 
                                tableView.filter("po_date_headOfDepartment", date, "EqualTo");
                            }
                            function FormatSelectedDate(picker) {
                                var date = picker.get_selectedDate();
                                var dateInput = picker.get_dateInput();
                                var formattedDate = dateInput.get_dateFormatInfo().FormatDate(date, dateInput.get_displayDateFormat());
 
                                return formattedDate;
                            }
                            </script>
                        </telerik:RadScriptBlock>
                    </FilterTemplate>--%>
                </telerik:GridTemplateColumn> 

                <telerik:GridTemplateColumn HeaderStyle-Width="8%" DataType="System.Boolean" AutoPostBackOnFilter ="true" ShowFilterIcon="false" HeaderText="ACCEPT REQUEST" DataField="po_accept_secretary" SortExpression="po_accept_secretary" UniqueName="po_accept_secretary" >
                    <ItemTemplate> 
                        <asp:CheckBox  id="po_accept_secretaryCB" onclick="javascript:return CheckedChange(this)" Visible ='<%# CheckAccepted(DataBinder.Eval(Container.DataItem, "po_accept_director").ToString())%>' runat="server" AutoPostBack="true" OnCheckedChanged="po_accept_secretaryCB_CheckedChanged" Checked='<%# DataBinder.Eval(Container.DataItem, "po_accept_secretary")%>' />
                    </ItemTemplate>
                    <EditItemTemplate></EditItemTemplate>
                </telerik:GridTemplateColumn>

                <telerik:GridTemplateColumn HeaderStyle-Width="8%" DataType="System.Boolean" AutoPostBackOnFilter ="true" ShowFilterIcon="false" HeaderText="ACCEPT REQUEST" DataField="po_accept_director" SortExpression="po_accept_director" UniqueName="po_accept_director" >
                    <ItemTemplate> 
                        <asp:CheckBox  id="po_accept_directorCB" onclick="javascript:return CheckedChange(this)" runat="server" AutoPostBack="true" Visible ='<%# CheckAccepted(DataBinder.Eval(Container.DataItem, "po_accept_headOfDepartment").ToString())%>' OnCheckedChanged="po_accept_directorCB_CheckedChanged" Checked='<%# DataBinder.Eval(Container.DataItem, "po_accept_director")%>' />
                    </ItemTemplate>
                    <EditItemTemplate></EditItemTemplate>
                </telerik:GridTemplateColumn>

                <telerik:GridTemplateColumn HeaderStyle-Width="6%" DataType="system.Boolean" AutoPostBackOnFilter ="true" ShowFilterIcon="false" HeaderText="ACCEPT REQUEST" DataField="po_accept_headOfDepartment" SortExpression="po_accept_headOfDepartment" UniqueName="po_accept_headOfDepartment" >
                    <ItemTemplate> 
                        <asp:CheckBox  id="po_accept_headOfDepartmentCB" onclick="javascript:return CheckedChange(this)" runat="server" AutoPostBack="true" OnCheckedChanged="po_accept_headOfDepartmentCB_CheckedChanged" Checked='<%# DataBinder.Eval(Container.DataItem, "po_accept_headOfDepartment")%>' />
                    </ItemTemplate>
                    <EditItemTemplate></EditItemTemplate>
                </telerik:GridTemplateColumn> 

                <telerik:GridTemplateColumn HeaderStyle-Width="5%" DataType="System.String" AutoPostBackOnFilter ="true" ShowFilterIcon="false" HeaderText="REQUEST COMPLETED/DATE" DataField="po_date_end_technicalOfficer" SortExpression="po_date_end_technicalOfficer" UniqueName="po_date_end_technicalOfficer" >
                    <ItemTemplate> 
                        <asp:Label  id="po_sendItLBL" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "po_sendIt")%>' Visible="false" />
                        <asp:Label  id="po_date_end_technicalOfficerLBL" runat="server" Text='<%#  ProcessMyDataItem(DataBinder.Eval(Container.DataItem, "po_date_end_technicalOfficer"))%>' />
                        <asp:checkbox  id="po_date_end_technicalOfficerRCB" onclick="javascript:return CheckedChange(this)" Checked ='<%# CheckIfPoIsEnded(DataBinder.Eval(Container.DataItem, "po_date_end_technicalOfficer").ToString())%>' runat="server" AutoPostBack="true" OnCheckedChanged ="po_date_end_technicalOfficerRCB_Click">
                        </asp:checkbox> 
                    </ItemTemplate>
                    <EditItemTemplate></EditItemTemplate>
                </telerik:GridTemplateColumn>

                <telerik:GridEditCommandColumn ButtonType="ImageButton" HeaderStyle-Width="2%" UniqueName="EditCommandColumn"/>

                <telerik:GridButtonColumn HeaderStyle-Width="2%" UniqueName="delete" Text="Delete" CommandName="Delete" ButtonType="ImageButton" ConfirmText="Are you sure you want to delete?" ConfirmDialogType="RadWindow" ConfirmTitle="Delete"/>
                             
                <telerik:GridTemplateColumn HeaderStyle-Width="3%" HeaderText="DETAILS" AllowFiltering="false" UniqueName="nextStep">
                    <ItemTemplate>
                        <%--<telerik:RadButton skin="Metro" skin="Metro" RenderMode="Lightweight" runat="server" tooltip = "Next" Text="Next" CommandArgument ='<%# DataBinder.Eval(Container.DataItem, "po_id").ToString()%>' OnClick ="Next_Click">
                            <Icon PrimaryIconCssClass="rbNext"></Icon>
                        </telerik:RadButton skin="Metro">--%>
                        <asp:LinkButton runat ="server" Text ="Details" tooltip = "Details" CommandArgument ='<%# DataBinder.Eval(Container.DataItem, "po_id").ToString()%>' OnClick ="Next_Click"/>
                    </ItemTemplate>
                </telerik:GridTemplateColumn> 

                <telerik:GridTemplateColumn HeaderStyle-Width="3%" HeaderText="PRINT" AllowFiltering="false" UniqueName="Print">
                    <ItemTemplate>
                        <telerik:RadButton skin="Metro" RenderMode="Lightweight" runat="server"  tooltip = "Print" text="Print" CommandArgument ='<%# DataBinder.Eval(Container.DataItem, "po_id").ToString()%>' OnClick ="Print_Click">
                            <Icon PrimaryIconCssClass="rbPrint"></Icon>
                        </telerik:RadButton>
                    </ItemTemplate> 
                </telerik:GridTemplateColumn> 

                
                <telerik:GridTemplateColumn HeaderStyle-Width="3%" HeaderText="FOLLOW UP" AllowFiltering="false" UniqueName="followup">
                    <ItemTemplate>
                        <telerik:RadButton  skin="Metro" RenderMode="Lightweight" ID="FollowUp" runat="server" Text="Follow Up" OnClick ="FollowUp_Click" CommandArgument ='<%# DataBinder.Eval(Container.DataItem, "po_id").ToString()%>'>
                            <Icon SecondaryIconCssClass="rbHelp"></Icon>
                        </telerik:RadButton >
                    </ItemTemplate> 
                </telerik:GridTemplateColumn> 

                <telerik:GridTemplateColumn  Visible ="false" >
                    <ItemTemplate> 
                        <asp:Label ID="po_isnewforSectretary" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "po_isnewforSectretary").ToString()%>'/>
                    </ItemTemplate> 
                </telerik:GridTemplateColumn>

                <telerik:GridTemplateColumn  Visible ="false" >
                    <ItemTemplate> 
                        <asp:Label ID="po_isnewforDirector" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "po_isnewforDirector").ToString()%>'/>
                    </ItemTemplate> 
                </telerik:GridTemplateColumn>

                <telerik:GridTemplateColumn  Visible ="false" >
                    <ItemTemplate> 
                        <asp:Label ID="po_isnewforHOD" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "po_isnewforHOD").ToString()%>'/>
                    </ItemTemplate> 
                </telerik:GridTemplateColumn>

                <telerik:GridTemplateColumn  Visible ="false" >
                    <ItemTemplate> 
                        <asp:Label ID="po_isnewforTechmical" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "po_isnewforTechmical").ToString()%>'/>
                    </ItemTemplate> 
                </telerik:GridTemplateColumn>

                <telerik:GridTemplateColumn  Visible ="false" >
                    <ItemTemplate> 
                        <asp:Label ID="po_isnewforAdmin" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "po_isnewforAdmin").ToString()%>'/>
                    </ItemTemplate> 
                </telerik:GridTemplateColumn>
            </Columns>

        </MasterTableView>
    </telerik:RadGrid> 
</asp:Content>




