<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AOS100_main.Master" CodeBehind="Help.aspx.vb" Inherits="AOS100web.About" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<link href="css/Main_Master.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<%--<link href="css/testadm.css" rel="stylesheet" />--%>

	<br />
	<br />

	<h1>Help</h1>
	<asp:Label ID="lblUser" runat="server" Text="User" Visible="false"></asp:Label>
	<asp:Label ID="lblGrpUser" runat="server" Text="Grp User" Visible="false"></asp:Label>

<%--<div class="topnav">

  <div class="topnav-centered">
	<a href="#home" class="active">Home</a>
  </div>

  <a href="#news">News
	  <asp:Button ID="Button1" runat="server" Text="Button" />
  </a>
  <a href="#contact">Contact</a>

  <div class="topnav-right">
	<a href="#search"><span><img src="images/email_24.png" /></span>Search</a>
	<a href="#about">About</a>
  </div>

</div>--%>

	<%--<asp:TreeView ID="TreeView1" runat="server" Width="300px" Height="auto"></asp:TreeView>--%>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
