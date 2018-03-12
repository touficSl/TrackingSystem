<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SSDLReport.aspx.cs" Inherits="Pages_Reports_SSDLReport" %>
<%@ Register Assembly="Telerik.ReportViewer.WebForms, Version=9.1.15.731, Culture=neutral, PublicKeyToken=a9d7983dfcc261be" Namespace="Telerik.ReportViewer.WebForms" TagPrefix="telerik" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <telerik:ReportViewer ID="ReportViewer1" runat="server" Width="100%" Height="800px"></telerik:ReportViewer>
    </div>
    </form>
</body>
</html>
