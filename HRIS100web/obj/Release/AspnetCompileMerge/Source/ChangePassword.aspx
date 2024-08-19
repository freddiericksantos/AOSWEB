<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AOS100_main.Master" CodeBehind="ChangePassword.aspx.vb" Inherits="AOS100web.ChangePassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
	<link href="css/admSubMaster.css" rel="stylesheet" />

	<%--<link href="css/Login.css" rel="stylesheet" />--%>
	<center>

		<br />
		<br />
		<br />
		<h2>Change Password</h2>
		<div class="Login">
			<br />
			<div class="Login_Form" style="align-self">
	
				<%--<br />--%>
				<asp:TextBox ID="txtCurrPass" runat="server" placeholder="Current Password" TextMode="Password" CssClass="Login_pwd" >

				</asp:TextBox>
				<asp:TextBox ID="txtPass1" runat="server" placeholder="New Password" TextMode="Password" CssClass="Login_pwd" >

				</asp:TextBox>
				<asp:TextBox ID="txtPass2" runat="server" placeholder="Confirm Password" TextMode="Password" CssClass="Login_pwd">

				</asp:TextBox>
				<asp:TextBox ID="txtPWhint" runat="server" placeholder="Password Hint" CssClass="Login_un">

				</asp:TextBox>
				<%--<br />--%>
				<asp:Button ID="btnLogin" runat="server" Text="SUBMIT" CssClass="Login_btn">

				</asp:Button>

				<br />
				<asp:Label ID="lblLog_Error" runat="server" Text="" ForeColor="red" CssClass="Login_adm"></asp:Label>
				<%--<asp:Label ID="lblStrike" runat="server" Text="0"></asp:Label>--%>

			</div>
		</div>

		<br />
		<br />
		<br />
		<br />
		<br />
		<br />

	</center>

</asp:Content>
