<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TLDReagentReport.aspx.cs" Inherits="Pages_Reports_TLDReagentReport" %>
<%@ Register Assembly="Telerik.ReportViewer.WebForms, Version=9.1.15.731, Culture=neutral, PublicKeyToken=a9d7983dfcc261be" Namespace="Telerik.ReportViewer.WebForms" TagPrefix="telerik" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager" runat="server" />
        <telerik:RadButton Skin="Metro" RenderMode="Lightweight" runat="server" tooltip = "Previous" ID="Previous" Text="Previous" OnClick ="Previous_Click">
            <Icon PrimaryIconCssClass="rbPrevious"></Icon>
        </telerik:RadButton>
        <telerik:ReportViewer ID="ReportViewer1" runat="server" Width="100%" Height="800px"></telerik:ReportViewer>
    </div>
    </form>
</body>
</html>
