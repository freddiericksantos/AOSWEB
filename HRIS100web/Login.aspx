<%@ Page Title="" Language="vb" AutoEventWireup="true" MasterPageFile="~/AOS100_main.Master" CodeBehind="Login.aspx.vb" Inherits="AOS100web.login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<link href="css/Main_Master.css" rel="stylesheet" />--%>
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
        <h2>User's Login</h2>
        <div class="Login">
            <br />

            <div class="Login_Form" style="align-self:auto; ">
                <asp:DropDownList ID="cboCompany" runat="server" AutoPostBack="true" CssClass="Login_Co"></asp:DropDownList>
                <br />
                <asp:TextBox ID="txtUserID" runat="server" placeholder="USER ID" CssClass="Login_un"></asp:TextBox>
                <br />
                <asp:TextBox ID="txtPassWord" runat="server" placeholder="PASSWORD" CssClass="Login_pwd"></asp:TextBox>
                <br />
                <asp:Button ID="btnLogin" runat="server" Text="SUBMIT" CssClass="Login_btn"></asp:Button>
                <br />
                <asp:Label ID="lblLog_Error" runat="server" Text="" ForeColor="red" CssClass="Login_adm"></asp:Label>
                <asp:Label ID="lblStrike" runat="server" Text="0"></asp:Label>

                <i class="fa fa-user"></i>

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
