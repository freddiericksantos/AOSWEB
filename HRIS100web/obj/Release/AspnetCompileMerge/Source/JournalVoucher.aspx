<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/FI.master" CodeBehind="JournalVoucher.aspx.vb" Inherits="AOS100web.JournalVoucher" %>

<%@ Register Assembly="EditableDropDownList" Namespace="EditableControls" TagPrefix="editable" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentOfFinAcctg" runat="server">
	<style type="text/css">
		div {
			z-index: 9999;
		}
	</style>
	<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

	<style>
		.Grid1 {
			margin-left: 1px;
			margin-right: 3px;
			margin-bottom: 2px;
			padding-left: 1px;
			padding-right: 1px;
			border-style: solid;
			border-color: black;
		}
	</style>

	<script type="text/javascript">
		function Confirm() {
			var confirm_value = document.createElement("INPUT");
			confirm_value.type = "hidden";
			confirm_value.name = "confirm_value";
			if (confirm("Do you sure to VOID JV?")) {
				confirm_value.value = "Yes";
			} else {
				confirm_value.value = "No";
			}
			document.forms[0].appendChild(confirm_value);
		}

		function Confirm2() {
			var confirm_value = document.createElement("INPUT");
			confirm_value.type = "hidden";
			confirm_value.name = "confirm_value";
			if (confirm("Are you SURE to RESET JV Form?")) {
				confirm_value.value = "Yes";
			} else {
				confirm_value.value = "No";
			}
			document.forms[0].appendChild(confirm_value);
		}

		function Confirm3() {
			var confirm_value = document.createElement("INPUT");
			confirm_value.type = "hidden";
			confirm_value.name = "confirm_value";
			if (confirm("Are you SURE to REMOVE Selected Line Item?")) {
				confirm_value.value = "Yes";
			} else {
				confirm_value.value = "No";
			}
			document.forms[0].appendChild(confirm_value);
		}

		function Confirm4() {
			var confirm_value = document.createElement("INPUT");
			confirm_value.type = "hidden";
			confirm_value.name = "confirm_value";
			if (confirm("Are you SURE to CLOSE JV Form?")) {
				confirm_value.value = "Yes";
			} else {
				confirm_value.value = "No";
			}
			document.forms[0].appendChild(confirm_value);
		}

	</script>

	<link href="css/admGen.css" rel="stylesheet" />

	<table style="width: 100%; font-family: 'Segoe UI'; float: left; margin-top: 0px; padding-top: 0px">
		<tr>
			<td style="background-color: #b1bbd7; border-top: solid; border-bottom: solid; border-color: red; border-width: 2px; width: 100%; font-family: 'Segoe UI'; font-size: 10px; text-align: center; min-height: 28px; max-height: 28px; padding-bottom: 1px;"
				colspan="4">
				<asp:LinkButton ID="lbNew" runat="server" OnClick="OnConfirm2" OnClientClick="Confirm2()" Style="text-decoration: none; background-color: #7fc6f6; font-family: Tahoma; font-size: 14px; color: white; margin-right: 5px; padding-left: 5px; padding-right: 5px; min-height: 22px;"
					BorderStyle="Solid" ToolTip="Reset Fields" CssClass="StdBottomL">
					<asp:Image runat="server" imageurl="~/images/new_16.png" style="vertical-align: middle"/>&nbsp;New
				</asp:LinkButton>
				<asp:LinkButton ID="lbSave" runat="server" OnClick="lbSave_Click" Text="Park " Style="text-decoration: none; background-color: #7fc6f6; padding-right: 5px; font-family: Tahoma; font-size: 14px; color: white; margin-right: 5px; padding-left: 5px; min-height: 22px;"
					BorderStyle="Solid" ToolTip="Save">
					<asp:Image runat="server" imageurl="~/images/save_orange_16.png" style="vertical-align: middle"/>&nbsp;
				</asp:LinkButton>
				<asp:LinkButton ID="lbPrint" runat="server" OnClientClick="target ='_blank';" OnClick="lbPrint_Click" Style="text-decoration: none; background-color: #7fc6f6; padding-right: 5px; font-family: Tahoma; font-size: 14px; color: white; margin-right: 5px; padding-left: 5px; min-height: 22px;"
					BorderStyle="Solid" ToolTip="Print" Enabled="false">
					<asp:Image runat="server" imageurl="~/images/Print_16.png" style="vertical-align: middle"/>&nbsp;Print
				</asp:LinkButton>
				<asp:LinkButton ID="lbDelete" runat="server" OnClick="OnConfirm" OnClientClick="Confirm()" Style="text-decoration: none; background-color: #7fc6f6; padding-right: 5px; font-family: Tahoma; font-size: 14px; color: white; margin-right: 5px; padding-left: 5px; min-height: 22px;"
					BorderStyle="Solid" ToolTip="Void" Enabled="false">
					<asp:Image runat="server" imageurl="~/images/delete_16.png" style="vertical-align: middle"/>&nbsp;Void
				</asp:LinkButton>
				<asp:LinkButton ID="lbClose" runat="server" OnClick="OnConfirm4" OnClientClick="Confirm4()" Style="text-decoration: none; background-color: #7fc6f6; font-family: Tahoma; font-size: 14px; color: white; min-height: 22px; padding-left: 5px;"
					BorderStyle="Solid" ToolTip="Close" CssClass="StdBottomR">
					<asp:Image runat="server"  imageurl="~/images/Exit_16.png" style="vertical-align: middle"/>&nbsp;Close&nbsp;
				</asp:LinkButton>

				<asp:Label ID="lblUser" runat="server" Text="User" Visible="false"></asp:Label>
				<asp:Label ID="lblGrpUser" runat="server" Text="Grp User" Visible="false"></asp:Label>

			</td>

			<td style="border: 1px solid #000000; min-width: 123px; background-color: lightgray; text-align: center; font-family: 'Segoe UI'; font-size: medium;">TC: &nbsp;
				<asp:Label ID="lblTC" runat="server" Text="00"></asp:Label>
			</td>

		</tr>
		<tr>
			<td style="border: 1px solid #000000; width: auto; background-color: lightgray;" colspan="7"></td>
		</tr>

	</table>

	<table style="width: 100%; font-family: sans-serif; font-size: small; float: left; background-color: lightgrey; border-spacing: 1px; margin-left: 2px; padding-left: 0px; padding-right: 0px;">

		<%--line1--%>
		<tr style="height: 26px; background-color: lightgray">

			<%--R1C1--%>
			<td style="border: 1px solid #000000; width: 120px; background-color: lightgray; text-align: right; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">JV No.:&nbsp;&nbsp;
			</td>

			<%--R1C2--%>
			<td style="border: 1px solid #000000; width: 180px; background-color: lightgray; text-align: center; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">
				<asp:TextBox ID="txtJVNo" runat="server" Font-Names="Segoe UI" Font-Size="small" ReadOnly="true" AutoPostBack="true" CssClass="txtBoxC"></asp:TextBox>

			</td>

			<%--R1C3--%>
			<td style="width: 120px; background-color: lightgray; text-align: center; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">
				<asp:LinkButton ID="lbReLoadJV" runat="server" Style="text-decoration: none; background-color: lightgray; float: left; padding-right: 2px;">&nbsp;
					<asp:Image runat="server" imageurl="~/images/check_16.png" style="vertical-align: middle"/>
				</asp:LinkButton>&nbsp;
				<asp:CheckBox ID="CheckBox1" runat="server" Text="Reg Entry" AutoPostBack="true" />
			</td>

			<%--R1C4--%>
			<td style="border: 1px solid #000000; width: 120px; background-color: lightgray; text-align: right; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">JV Type:&nbsp;&nbsp;
			</td>

			<%--R1C5to6--%>
			<td style="border: 1px solid #000000; width: 250px; background-color: lightgray; text-align: left; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;" colspan="2">
				<asp:DropDownList ID="cboJVtype" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" CssClass="cboBox">
				</asp:DropDownList>
			</td>

			<%--R1C7--%>
			<td style="border: 1px solid #000000; width: 80px; background-color: lightgray; text-align: right; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">
				<asp:LinkButton ID="lbJVtype" runat="server" Style="text-decoration: none; background-color: lightgray; float: left; padding-right: 2px;">&nbsp;
				<asp:Image runat="server" imageurl="~/images/check_16.png" style="vertical-align: middle"/>
				</asp:LinkButton>&nbsp;
				Status:&nbsp;&nbsp;
			</td>

			<%--R1C8--%>
			<td style="border: 1px solid #000000; width: 120px; background-color: oldlace; text-align: center; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">
				<asp:Label ID="lblStatus" runat="server" Text="Status" ForeColor="Red"></asp:Label>
			</td>

			<%--R1C9--%>
			<td style="border: 1px solid #000000; background-color: khaki; text-align: center; vertical-align: middle;padding-left: 1px; font-family: 'Segoe UI'; font-size: small;" colspan="2" rowspan="2">Important Message <
				<asp:Label ID="tssErrMsg" runat="server" ForeColor="Red" Font-Size="Medium"></asp:Label> >
			</td>

			<%--R1C10--%>
			<%--<td style="width: auto; background-color: lightgray; text-align: center; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;"></td>--%>
		</tr>

		<%--line2--%>
		<tr style="height: 26px; background-color: lightgray">
			<%--L2C1--%>
			<td style="border: 1px solid #000000; width: 120px; background-color: lightgray; text-align: right; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">Ref. No:&nbsp;&nbsp;
			</td>

			<%--L2C2--%>
			<td style="border: 1px solid #000000; width: 180px; background-color: lightgray; text-align: center; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">
				<asp:TextBox ID="txtRefNo" runat="server" Font-Names="Segoe UI" Font-Size="small" AutoPostBack="true" CssClass="txtBoxC"></asp:TextBox>
			</td>

			<%--L2C3--%>
			<td style="width: 120px; background-color: lightgray; text-align: center; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">
				<asp:Button ID="btnReproc" runat="server" Text="Reprocess" />
			</td>

			<%--L3C4--%>
			<td style="border: 1px solid #000000; width: 120px; background-color: lightgray; text-align: right; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">JV Source:&nbsp;&nbsp;
			</td>

			<%--L2C5-6--%>
			<td style="border: 1px solid #000000; width: 250px; background-color: lightgray; text-align: left; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;" colspan="2">
				<asp:DropDownList ID="cboJVsource" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" CssClass="cboBox">
				</asp:DropDownList>
			</td>

			<%--L2C7--%>
			<td style="border: 1px solid #000000; width: 200px; background-color: lightgray; text-align: right; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;" colspan="2">&nbsp;&nbsp;
			</td>

			<%--L2C8--%>
			<%--<td style="border: 1px solid #000000; width: 120px; background-color: lightgray; text-align: left; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">
			</td>--%>

			<%--L2C9--%>
			<%--<td style="width: 180px; background-color: lightgray; text-align: right; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;"></td>

			<td style="width: auto; background-color: lightgray; text-align: right; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;"></td>--%>
		</tr>

		<tr style="height: 26px; background-color: lightgray">
			<%--L3C1--%>
			<td style="border: 1px solid #000000; width: 120px; background-color: lightgray; text-align: right; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">Posting Date:&nbsp;&nbsp;
			</td>

			<%--L3C2--%>
			<td style="border: 1px solid #000000; width: 180px; background-color: lightgray; text-align: center; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">
				<asp:TextBox ID="dpTransDate" runat="server" TextMode="Date" Width="178px" Height="20px" AutoPostBack="true" Font-Names="Segoe UI"
					Font-Size="small" CssClass="DateBox"></asp:TextBox>
			</td>

			<%--L3C3--%>
			<td style="border: 1px solid #000000; width: 120px; background-color: lightgray; text-align: center; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;" colspan="2">
				<asp:CheckBox ID="cbCentry" runat="server" Text="Costing Entry" AutoPostBack="true" />
				&nbsp;&nbsp;PC:&nbsp;&nbsp;
				<asp:DropDownList ID="cboPC" runat="server" AutoPostBack="true" Width="60px" Font-Names="Segoe UI" Font-Size="small">
				</asp:DropDownList>
			</td>

			<%--L3C5--%>
			<td style="border: 1px solid #000000; width: 120px; background-color: lightgray; text-align: right; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">Trans Year:&nbsp;&nbsp;
			</td>

			<%--L3C6--%>
			<td style="border: 1px solid #000000; width: 130px; background-color: oldlace; text-align: center; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">
				<asp:Label ID="txtYear" runat="server" Text="yyyy" ForeColor="Red"></asp:Label>
			</td>

			<%--L3C7--%>
			<td style="border: 1px solid #000000; width: 80px; background-color: lightgray; text-align: right; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">Trans From:&nbsp;&nbsp;
			</td>

			<%--L3C8--%>
			<td style="border: 1px solid #000000; width: 120px; background-color: oldlace; text-align: center; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">
				<asp:Label ID="txtDfrom" runat="server" Text="yyyy-MM-dd" ForeColor="Red"></asp:Label>
			</td>

			<%--L3C9--%>
			<td style="width: 180px; background-color: lightgray; text-align: left; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">
				<asp:CheckBox ID="CheckBox3" runat="server" Text="Admin Use" AutoPostBack="true" />
			</td>

			<td style="width: auto; background-color: lightgray; text-align: right; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;"></td>
		</tr>

		<tr style="background-color: lightgray">
			<%--L4C1--%>
			<td style="border: 1px solid #000000; height: auto; width: 120px; background-color: lightgray; text-align: right; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;" rowspan="2">Remarks:&nbsp;&nbsp;
			</td>

			<%--L4C2to4--%>
			<td style="border: 1px solid #000000; height: auto; width: 420px; background-color: lightgray; text-align: left; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;" colspan="3" rowspan="2">
				<asp:TextBox ID="txtRemarks" runat="server" Height="54px" Width="98%" TextMode="MultiLine"></asp:TextBox>
			</td>

			<%--L4C5--%>
			<td style="border: 1px solid #000000; height: 26px; width: 120px; background-color: lightgray; text-align: right; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">Trans Mon:&nbsp;&nbsp;
			</td>

			<%--L4C6--%>
			<td style="border: 1px solid #000000; width: 130px; background-color: oldlace; text-align: center; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">
				<asp:Label ID="txtMon" runat="server" Text="MM" ForeColor="Red"></asp:Label>
			</td>

			<%--L4C7--%>
			<td style="border: 1px solid #000000; width: 80px; background-color: lightgray; text-align: right; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">Trans To:&nbsp;&nbsp;
			</td>

			<%--L4C8--%>
			<td style="border: 1px solid #000000; width: 120px; background-color: oldlace; text-align: center; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">
				<asp:Label ID="txtDto" runat="server" Text="yyyy-MM-dd" ForeColor="Red"></asp:Label>
			</td>

			<%--L4C9--%>
			<td style="width: 180px; background-color: lightgray; text-align: left; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">
				<asp:CheckBox ID="CheckBox4" runat="server" Text="Ref JV No." AutoPostBack="true" />
			</td>

			<td style="width: auto; background-color: lightgray; text-align: right; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;"></td>
		</tr>

		<tr style="background-color: lightgray">
			<%--L5C5--%>
			<td style="border: 1px solid #000000; width: auto; height: 22px; text-align: center; background-color: oldlace; font-family: 'Segoe UI'; font-size: small;" colspan="4">&nbsp;&nbsp;Entry Type:&nbsp;
				<asp:RadioButton ID="RadioButton1" GroupName="JV" Text="Regular" runat="server" Font-Names="Segoe UI" Font-Size="Small" />&nbsp;&nbsp;
				<asp:RadioButton ID="RadioButton2" GroupName="JV" Text="Reversal" runat="server" Font-Names="Segoe UI" Font-Size="Small" />
			</td>

			<td style="border: 1px solid #000000; width: 180px; height: 22px; background-color: lightgray;">
				<asp:TextBox ID="txtRecJVno" runat="server" Font-Names="Segoe UI" Font-Size="small" AutoPostBack="true" CssClass="txtBoxC" Height="20px"></asp:TextBox>
			</td>

			<td style="width: auto; height: 22px; text-align: center;">
				<asp:LinkButton ID="lbRecJVno" runat="server" Style="text-decoration: none; background-color: lightgray; float: left; padding-right: 2px;">&nbsp;
					<asp:Image runat="server" imageurl="~/images/check_16.png" style="vertical-align: middle"/>
				</asp:LinkButton>
			</td>

		</tr>

		<tr>
			<%--red line--%>
			<td style="border: 1px solid #000000; width: auto; height: auto; background-color: red;" colspan="10"></td>
		</tr>
	</table>

	<ajaxToolkit:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" Style="width: 100%; margin-left: 3px; height: auto; background-color: ghostwhite;">
		<ajaxToolkit:TabPanel ID="TabPanel1" runat="server" HeaderText="Data Entry" CssClass="TabPanel1" Height="585px">
			<ContentTemplate>
				<table style="width: 100%; font-family: 'Segoe UI'; font-size: small; float: left; background-color: lightgrey; border-spacing: 0px; margin-left: 0px; padding-left: 0px; padding-right: 0px;">
					<tr style="height: 26px; background-color: lightgray">
						<td style="border: 1px solid #000000; width: 80px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" rowspan="2">Select</td>
						<td style="border: 1px solid #000000; width: 80px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Itm No.</td>
						<td style="border: 1px solid #000000; width: 300px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Cost Center
							<asp:LinkButton ID="lbCCenter" runat="server" Style="text-decoration: none; background-color: lightgray; float: right; padding-right: 2px;">&nbsp;
							<asp:Image runat="server" imageurl="~/images/check_16.png" style="vertical-align: middle"/>
							</asp:LinkButton>&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: 450px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:CheckBox ID="CheckBox5" runat="server" Text="For Prior Year Adjt"/>
							&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
							GL Account
							<asp:LinkButton ID="lbGLdesc" runat="server" Style="text-decoration: none; background-color: lightgray; float: right; padding-right: 2px;">&nbsp;
							<asp:Image runat="server" imageurl="~/images/check_16.png" style="vertical-align: middle"/>
							</asp:LinkButton>&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: 120px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">DR Amount</td>
						<td style="border: 1px solid #000000; width: 120px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">CR Amount</td>
						<td style="border: 1px solid #000000; width: 50px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">PK</td>
						<td style="border: 1px solid #000000; width: 80px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Sub Acct</td>
						<td style="border: 1px solid #000000; width: 25px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" rowspan="2">
							<asp:Label ID="lblSubAcct" runat="server" Text=""></asp:Label>
						</td>

					</tr>
					<tr style="height: 26px; background-color: lightgray">
						<td style="border: 1px solid #000000; background-color: whitesmoke; text-align: center; padding-left: 1px; padding-right: 1px; font-family: 'Segoe UI'; font-size: small;">
							<asp:TextBox ID="txtItm" runat="server" Font-Names="Segoe UI" Font-Size="Small" Text="1" CssClass="TxtBox"></asp:TextBox>
						</td>
						<td style="border: 1px solid #000000; background-color: whitesmoke; text-align: center; padding-left: 1px; padding-right: 1px; font-family: 'Segoe UI'; font-size: small;">
							<asp:DropDownList ID="cboCCtr" runat="server" AutoPostBack="True" Font-Names="Segoe UI" Font-Size="Small" CssClass="cboBox"></asp:DropDownList>
						</td>
						<td style="border: 1px solid #000000; background-color: whitesmoke; text-align: center; padding-left: 1px; padding-right: 1px; font-family: 'Segoe UI'; font-size: small;">
							<asp:DropDownList ID="cboGLdesc" runat="server" AutoPostBack="True" Font-Names="Segoe UI" Font-Size="Small" CssClass="cboBox"></asp:DropDownList>
						</td>
						<td style="border: 1px solid #000000; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:TextBox ID="txtDRamt" runat="server" Font-Names="Segoe UI" Font-Size="Small" AutoPostBack="true" CssClass="txtAmtBox"></asp:TextBox>
						</td>
						<td style="border: 1px solid #000000; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:TextBox ID="txtCRamt" runat="server" Font-Names="Segoe UI" Font-Size="Small" AutoPostBack="true" CssClass="txtAmtBox"></asp:TextBox>
						</td>
						<td style="border: 1px solid #000000; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:Label ID="lblPK" runat="server" Text=""></asp:Label>
						</td>
						<td style="border: 1px solid #000000; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:TextBox ID="txtSubAcct" runat="server" Font-Names="Segoe UI" Font-Size="Small" AutoPostBack="true" MaxLength="5" CssClass="txtBoxC" Width="95%"></asp:TextBox>
						</td>

					</tr>

					<tr style="height: auto; background-color: lightgray">
						<td style="border: 1px solid #000000; height: 478px; background-color: floralwhite; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; vertical-align: top; margin-left: 0px" colspan="8">
							<asp:Panel ID="Panel1" runat="server" ScrollBars="Auto" CssClass="PanelStd2">
								<asp:GridView ID="DgvJVdet" runat="server" AutoGenerateColumns="False" OnRowDataBound="DgvJVdet_RowDataBound"
									OnRowCreated="DgvJVdet_RowCreated" Font-Names="Segoe UI" Font-Size="Small" BackColor="White" BorderColor="#999999" BorderStyle="Solid"
									BorderWidth="1px" CellPadding="3" GridLines="Vertical" ShowHeader="False" CssClass="Grid1">
									<AlternatingRowStyle BackColor="Gainsboro" />
									<Columns>
										<asp:CommandField ShowSelectButton="True">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="84px"></ItemStyle>
										</asp:CommandField>
										<asp:BoundField DataField="itmno">
											<ItemStyle HorizontalAlign="center" VerticalAlign="Middle" Width="89px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="ccno">
											<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="342px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="acctno">
											<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="513px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="dramt" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="135px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="cramt" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="135px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="pk">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="53px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="subacct">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="69px"></ItemStyle>
										</asp:BoundField>

									</Columns>
									<FooterStyle BackColor="#CCCCCC" ForeColor="Black" BorderStyle="Solid" />
									<HeaderStyle BackColor="#007ACC" Font-Bold="True" ForeColor="White" BorderColor="Black" />
									<PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
									<RowStyle BackColor="#EEEEEE" ForeColor="Black" />
									<SelectedRowStyle BackColor="Orange" Font-Bold="True" ForeColor="White" />
									<SortedAscendingCellStyle BackColor="#F1F1F1" />
									<SortedAscendingHeaderStyle BackColor="#0000A9" />
									<SortedDescendingCellStyle BackColor="#CAC9C9" />
									<SortedDescendingHeaderStyle BackColor="#000065" />
								</asp:GridView>
							</asp:Panel>

							<%--<script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
							<script src="Scripts/ScrollableGridViewPlugin_ASP.NetAJAXmin.js" type="text/javascript"></script>
							<script type="text/javascript">
								$(document).ready(function () {
									$('#<%=DgvJVdet.ClientID %>').Scrollable({
										ScrollHeight: 454,
										IsInPanel: true
									});
								});
							</script>--%>

						</td>

						<td style="border: 1px solid #000000; width: 25px; background-color: beige; text-align: center; vertical-align: text-top; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:ImageButton ID="btnReset" runat="server" ImageUrl="~/images/reset_16.png" CssClass="ImgBtn" />
							<asp:ImageButton ID="btnAdd" runat="server" ImageUrl="~/images/check_16.png" CssClass="ImgBtn" />
							<asp:ImageButton ID="btnDel" runat="server" OnClick="OnConfirm3" OnClientClick="Confirm3()" ImageUrl="~/images/delete_16.png" />

						</td>

					</tr>

					<tr style="height: 26px; background-color: lightgray">

						<td style="border: 1px solid #000000; width: auto; background-color: lightgray; text-align: right; font-family: 'Segoe UI'; font-size: small;" colspan="4">Total &nbsp;&nbsp;
						</td>

						<td style="border: 1px solid #000000; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:Label ID="lblDRtot" runat="server" Text="0.00" ForeColor="Red" CssClass="lblLabelAmt"></asp:Label>
						</td>

						<td style="border: 1px solid #000000; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:Label ID="lblCRtot" runat="server" Text="0.00" ForeColor="Red" CssClass="lblLabelAmt"></asp:Label>
						</td>

						<td style="border: 1px solid #000000; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="2">
							<asp:Label ID="lblBalAmt" runat="server" CssClass="lblLabelAmt" ForeColor="Red" Text="0.00"></asp:Label>
						</td>

						<td style="border: 1px solid #000000; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
					</tr>

					<tr style="height: auto; background-color: lightgray">
						<td style="border: 1px solid #000000; height: 20px; background-color: lightgray; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="13">&nbsp;&nbsp; Doc Status: &nbsp;&nbsp;
								<asp:Label ID="tssDocStat" runat="server" Text="New" ForeColor="Red"></asp:Label>
								&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Edit Doc No.: &nbsp;&nbsp;
								<asp:Label ID="tssDocNo" runat="server" Text="00000000" ForeColor="Red"></asp:Label>
								&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Line Item: &nbsp;&nbsp;
								<asp:Label ID="lblLineItm" runat="server" Text="New" ForeColor="Red"></asp:Label>
								&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
							Error Message:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
							<asp:Label ID="tssOthMsg" runat="server" ForeColor="Red"></asp:Label>
								&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
							<asp:Label ID="lblLastDocNo" runat="server" ForeColor="Red"></asp:Label>
							&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
							<asp:Label ID="txtFirstDayYear" runat="server" ForeColor="Red" ></asp:Label>
							&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
							<asp:Label ID="txtLastDayYear" runat="server" ForeColor="Red" ></asp:Label>

						</td>
					</tr>


				</table>


			</ContentTemplate>

		</ajaxToolkit:TabPanel>

		<%--JV List--%>
		<ajaxToolkit:TabPanel ID="TabPanel2" runat="server" HeaderText="JV Summary" CssClass="TabPanel1" Height="585px">
			<ContentTemplate>
				<table style="width: 100%; font-family: 'Segoe UI'; font-size: small; float: left; background-color: lightgrey; border-spacing: 0px; margin-left: 0px; padding-left: 0px; padding-right: 0px;">
					<tr style="height: 26px; background-color: lightgray">
						<td style="border: 1px solid #000000; width: 400px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Period:&nbsp;&nbsp;
						<asp:TextBox ID="dpDate1" runat="server" TextMode="Date" Width="120px" Height="20px" Font-Names="Segoe UI" Font-Size="small" AutoPostBack="true" CssClass="DateBox"></asp:TextBox>
							&nbsp;&nbsp;To&nbsp;&nbsp;
						<asp:TextBox ID="dpDate2" runat="server" TextMode="Date" Width="120px" Height="20px" Font-Names="Segoe UI" Font-Size="small" AutoPostBack="true" CssClass="DateBox"></asp:TextBox>

						</td>
						<td style="border: 1px solid #000000; width: 220px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							Status:&nbsp;&nbsp;
							<asp:DropDownList ID="cboStat" runat="server" AutoPostBack="True" Font-Names="Segoe UI" Font-Size="Small" CssClass="cboBox" Width="120px"></asp:DropDownList>
							<asp:LinkButton ID="LbStat" runat="server" Style="text-decoration: none; background-color: lightgray; float: right; padding-right: 2px;">
							<asp:Image runat="server" imageurl="~/images/check_16.png" style="vertical-align: middle"/>
							</asp:LinkButton>&nbsp;
						</td>

						<td style="border: 1px solid #000000; width: auto; background-color: lightgray; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>

					</tr>

					<tr style="background-color: lightgray">
						<td style="border: 1px solid #000000; height: 555px; background-color: floralwhite; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; vertical-align: top; margin-left: 0px" colspan="7">
							<asp:Panel ID="Panel2" runat="server" Width="100%" Height="536px">
								<asp:GridView ID="DgvJVsumList" runat="server" AutoGenerateColumns="False" OnRowDataBound="DgvJVsumList_RowDataBound" OnRowCreated="DgvJVsumList_RowCreated" Font-Names="Segoe UI" Font-Size="small" BackColor="White" BorderColor="#999999" BorderStyle="Solid" AllowSorting="true" BorderWidth="1px" CellPadding="3" GridLines="Vertical" HeaderStyle-BorderColor="Black" ShowHeader="true">
								<AlternatingRowStyle BackColor="#DCDCDC" />
									<Columns>
										<asp:CommandField HeaderText="Select" ShowSelectButton="true">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="72px"></ItemStyle>
										</asp:CommandField>

										<asp:BoundField DataField="jvno" HeaderText="JV No.">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="110px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="transdate" HeaderText="Date" DataFormatString="{0:yyyy-MM-dd}">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="jvtype" HeaderText="JV Type">
											<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="120px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="sourcedoc" HeaderText="Source Doc">
											<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="120px"></ItemStyle>
										</asp:BoundField>
																				
										<asp:BoundField DataField="status" HeaderText="Status">
											<ItemStyle HorizontalAlign="left" VerticalAlign="Middle" Width="120px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="remarks" HeaderText="Remarks">
											<ItemStyle HorizontalAlign="left" VerticalAlign="Middle" Width="500px"></ItemStyle>
										</asp:BoundField>
										
										<asp:BoundField DataField="user" HeaderText="Prepated By">
											<ItemStyle HorizontalAlign="left" VerticalAlign="Middle" Width="150px"></ItemStyle>
										</asp:BoundField>
										
										<asp:BoundField DataField="jvno2" HeaderText="Ref No.">
											<ItemStyle HorizontalAlign="left" VerticalAlign="Middle" Width="150px"></ItemStyle>
										</asp:BoundField>

									</Columns>

									<FooterStyle BackColor="#CCCCCC" ForeColor="Black" BorderStyle="Solid" />
									<HeaderStyle BackColor="#007acc" Font-Bold="True" ForeColor="White" Height="22px" />
									<PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
									<RowStyle BackColor="#EEEEEE" ForeColor="Black" />
									<SelectedRowStyle BackColor="Orange" Font-Bold="True" ForeColor="White" />
									<SortedAscendingCellStyle BackColor="#F1F1F1" />
									<SortedAscendingHeaderStyle BackColor="#0000A9" />
									<SortedDescendingCellStyle BackColor="#CAC9C9" />
									<SortedDescendingHeaderStyle BackColor="#000065" />
								</asp:GridView>

							</asp:Panel>

							<script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
							<script src="Scripts/ScrollableGridViewPlugin_ASP.NetAJAXmin.js" type="text/javascript"></script>
							<script type="text/javascript">
								$(document).ready(function () {
									$('#<%=DgvJVsumList.ClientID %>').Scrollable({
										ScrollHeight: 530,
										IsInPanel: true
									});
								});
							</script>
						</td>

					</tr>

				</table>

			</ContentTemplate>

		</ajaxToolkit:TabPanel>

		<ajaxToolkit:TabPanel ID="TabPanel3" runat="server" HeaderText="Vendor Search" CssClass="TabPanel1" Height="585px">
			<ContentTemplate>
				<table style="width: 100%; font-family: 'Segoe UI'; font-size: small; float: left; background-color: lightgrey; border-spacing: 0px; margin-left: 0px; padding-left: 0px; padding-right: 0px;">
					<tr style="height: 26px; background-color: lightgray">
						<td style="border: 1px solid #000000; width: 400px; background-color: lightgray; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">&nbsp;&nbsp; Search Vendor:&nbsp;&nbsp;
						<asp:TextBox ID="txtSearch" runat="server" Width="250" Font-Names="Segoe UI" placeholder="Search Customer" Font-Size="Small" TextMode="Search" AutoPostBack="true"></asp:TextBox>
							<asp:LinkButton ID="lbSearch" runat="server" OnClick="lbSearch_Click" Style="text-decoration: none; background-color: lightgray; float: inherit; padding-right: 2px;">
						<asp:Image runat="server" imageurl="~/images/search_red16.png" style="vertical-align: middle"/>
							</asp:LinkButton>

						</td>

					</tr>

					<tr style="background-color: lightgray">
						<td style="border: 1px solid #000000; height: 555px; background-color: floralwhite; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; vertical-align: top; margin-left: 0px" colspan="7">
							<asp:Panel ID="Panel3" runat="server" Width="100%" Height="536px">
								<asp:GridView ID="DgvVendor" runat="server" AutoGenerateColumns="False" OnRowDataBound="DgvVendor_RowDataBound" OnRowCreated="DgvVendor_RowCreated" Font-Names="Segoe UI" Font-Size="small" BackColor="White" BorderColor="#999999" BorderStyle="Solid" AllowSorting="true" BorderWidth="1px" CellPadding="3" GridLines="Vertical" HeaderStyle-BorderColor="Black" ShowHeader="true">
								<AlternatingRowStyle BackColor="#DCDCDC" />
									<Columns>
										<asp:CommandField HeaderText="Select" ShowSelectButton="true">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="72px"></ItemStyle>
										</asp:CommandField>

										<asp:BoundField DataField="venno" HeaderText="Vendor No.">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="110px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="venname" HeaderText="Vendor Name">
											<ItemStyle HorizontalAlign="left" VerticalAlign="Middle" Width="500px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="address" HeaderText="Address">
											<ItemStyle HorizontalAlign="left" VerticalAlign="Middle" Width="600px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="status" HeaderText="Status">
											<ItemStyle HorizontalAlign="left" VerticalAlign="Middle" Width="120px"></ItemStyle>
										</asp:BoundField>

									</Columns>

									<FooterStyle BackColor="#CCCCCC" ForeColor="Black" BorderStyle="Solid" />
									<HeaderStyle BackColor="#007acc" Font-Bold="True" ForeColor="White" Height="22px" />
									<PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
									<RowStyle BackColor="#EEEEEE" ForeColor="Black" />
									<SelectedRowStyle BackColor="Orange" Font-Bold="True" ForeColor="White" />
									<SortedAscendingCellStyle BackColor="#F1F1F1" />
									<SortedAscendingHeaderStyle BackColor="#0000A9" />
									<SortedDescendingCellStyle BackColor="#CAC9C9" />
									<SortedDescendingHeaderStyle BackColor="#000065" />
								</asp:GridView>

							</asp:Panel>

							<script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
							<script src="Scripts/ScrollableGridViewPlugin_ASP.NetAJAXmin.js" type="text/javascript"></script>
							<script type="text/javascript">
								$(document).ready(function () {
									$('#<%=DgvVendor.ClientID %>').Scrollable({
										ScrollHeight: 530,
										IsInPanel: true
									});
								});
							</script>
						</td>

					</tr>
				</table>


			</ContentTemplate>

		</ajaxToolkit:TabPanel>


	</ajaxToolkit:TabContainer>

</asp:Content>
