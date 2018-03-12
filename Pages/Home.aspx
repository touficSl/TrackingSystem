<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Pages/MasterPage.master" CodeFile="Home.aspx.cs" Inherits="Pages_Home_Home" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript">
        var image1 = new Image()
        image1.src = "images/1.jpg"
        var image2 = new Image()
        image2.src = "images/2.jpg"
        var image3 = new Image()
        image3.src = "images/3.jpg"
        var image4 = new Image()
        image4.src = "images/4.jpg"
    </script>

    <title>Home</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <p align = "center">
        <img align="center" name="slide" src="Images/image.jpg" style="height:600px; width: 850px" />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
    </p>
    <script type="text/javascript">
        var step = 1;
        function slideit() {
            document.images.slide.src = eval("image" + step + ".src");
            if (step < 4)
                step++;
            else
                step = 1;
            setTimeout("slideit()", 2500);
        }
        slideit();
    </script>
</asp:Content>
