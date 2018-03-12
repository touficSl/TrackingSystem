<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Pages/MasterPage.master" CodeFile="OrderLine.aspx.cs" Inherits="Pages_OrderLine" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>  
<asp:content ID="Content2" ContentPlaceHolderID="head" Runat="Server">
    <title >
        Order Line
    </title>  
     
    <style >
        body
        {
          margin:0 auto;
          padding:0;
          /*text-align:center;*/ 
          background-color:#D8D8D8;
        }
        #wrapper1
        {
          width:995px;
          padding:0px;
          margin:0px auto;
          font-family:helvetica;
          position:relative;
        }
        #wrapper1 .baricon
        {
          display:inline-block;
          border-radius:100%;
          padding:12px;
          background-color:#4F52BA;
          color:white;
        }
        #wrapper1 .progress_bar
        {
          width:100px;
          height:5px;
          border-radius:20px;
          background-color:#D8D8D8;
          display:inline-block;
          font-size :small;
        }
        #wrapper1 form  
        {
          margin-left:340px;
          padding:10px;
          box-sizing:border-box;
          width:300px;
          margin-top:50px;
          background-color:#585858;
        }
        #wrapper1 form p
        {
          color:#F2F2F2;
          margin:0px;
          margin-top:10px;
          font-weight:bold;
        }
        #wrapper1 form .form_head
        {
          font-size:22px;
          font-weight:bold;
          margin-bottom:30px;
        }
        #wrapper1 form input[type="text"]
        {
          width:200px;
          height:40px;
          padding:5px;
          border-radius:5px;
          border:none;
          margin-top:10px;
        }
        #wrapper1 form input[type="button"],input[type="submit"]
        {
          width:80px;
          height:40px;
          border-radius:5px;
          border:2px solid white;
          background:none;
          color:white;
          margin:5px;
          margin-top:10px;
        }
        #user_details,#qualification
        {
          display:none;
        }
         .progress_bar1
        {
          width:100px;
          height:5px;
          border-radius:20px; 
          display:inline-block;
          font-size :small; 
        }
    </style> 
</asp:content> 

<asp:content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" Runat="Server">
     <div id="wrapper1">
          <br/>
          <br/>
          <span class='baricon'>Client</span>
          <span id="bar1" class='progress_bar' runat="server"></span>
          <span class='baricon'>Secretary</span>
          <span id="bar2" class='progress_bar' runat="server"></span>
          <span class='baricon'>Director</span>
          <span id="bar3" class='progress_bar' runat="server"></span>
          <span class='baricon'>Head of Department</span>
          <span id="bar4" class='progress_bar' runat="server"></span>
          <span class='baricon'>Technical Officer</span> 

         <span class='baricon' style ="visibility :hidden ">Client</span>
          <span id="Span1" class='progress_bar1' style ="font-size :small;color :#4F52BA" runat="server"></span>
          <span class='baricon' style ="visibility :hidden ">Secretary</span>
          <span id="Span2" class='progress_bar1' style ="font-size :small;color :#4F52BA" runat="server"></span>
          <span class='baricon' style ="visibility :hidden ">Director</span>
          <span id="Span3" class='progress_bar1' style ="font-size :small;color :#4F52BA" runat="server"></span>
          <span class='baricon' style ="visibility :hidden ">Head of Department</span>
          <span id="Span4" class='progress_bar1' style ="font-size :small;color :#4F52BA" runat="server"></span>
          <span class='baricon' style ="visibility :hidden ">Technical Officer</span> 
     </div>
</asp:content> 
