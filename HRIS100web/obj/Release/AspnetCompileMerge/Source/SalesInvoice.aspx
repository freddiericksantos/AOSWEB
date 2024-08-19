<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SD.master" CodeBehind="SalesInvoice.aspx.vb" Inherits="AOS100web.SalesInvoice" %>

<%@ Register Assembly="EditableDropDownList" Namespace="EditableControls" TagPrefix="editable" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentOfSD" runat="server">
	<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
	<style>
		.Grid1 {
			margin-left: 2px;
			margin-right: auto;
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
			if (confirm("Do you sure to RESET Fields?")) {
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
			if (confirm_value("Are you sure to VOID Sales Invoice?")) {
				confirm_value.value = "Yes";
			} else {
				confirm_value.value = "No";
			}
			document.forms[0].appendChild(confirm_value);
		}


	</script>

	<link href="css/admGen.css" rel="stylesheet" />

	<table style="width: 100%; font-family: 'Segoe UI'; float: left; margin-top: -2px; padding-top: 0px">
		<tr>
			<td style="background-color: #b1bbd7; border-top: solid; border-bottom: solid; border-color: red; border-width: 2px; width: 100%; font-family: 'Segoe UI'; font-size: 10px; text-align: center; min-height: 28px; max-height: 28px; padding-bottom: 1px;"
				colspan="4">
				<asp:LinkButton ID="lbNew" runat="server" OnClick="OnConfirm" OnClientClick="Confirm()" Style="text-decoration: none; background-color: #7fc6f6; font-family: Tahoma; font-size: 14px; color: white; margin-right: 5px; padding-left: 5px; padding-right: 5px; min-height: 22px;" BorderStyle="Solid" ToolTip="Reset Fields" CssClass="StdBottomL">
					<asp:Image runat="server" imageurl="~/images/new_16.png" style="vertical-align: middle"/>&nbsp;New
				</asp:LinkButton>

				<asp:LinkButton ID="lbSave" runat="server" OnClick="lbSave_Click" Style="text-decoration: none; background-color: #7fc6f6; padding-right: 5px; font-family: Tahoma; font-size: 14px; color: white; margin-right: 5px; padding-left: 5px; min-height: 22px;"
					BorderStyle="Solid" ToolTip="Save">
					<asp:Image runat="server" imageurl="~/images/save_orange_16.png" style="vertical-align: middle"/>&nbsp;Save
				</asp:LinkButton>

				<asp:LinkButton ID="lbPrint" runat="server" OnClientClick="target ='_blank';" OnClick="lbPrint_Click" Style="text-decoration: none; background-color: #7fc6f6; padding-right: 5px; font-family: Tahoma; font-size: 14px; color: white; margin-right: 5px; padding-left: 5px; min-height: 22px;"
					BorderStyle="Solid" ToolTip="Print" Enabled="false">
					<asp:Image runat="server" imageurl="~/images/Print_16.png" style="vertical-align: middle"/>&nbsp;Print
				</asp:LinkButton>

				<asp:LinkButton ID="lbDelete" runat="server" OnClick="OnConfirm3" OnClientClick="Confirm3()" Style="text-decoration: none; background-color: #7fc6f6; padding-right: 5px; font-family: Tahoma; font-size: 14px; color: white; margin-right: 5px; padding-left: 5px; min-height: 22px;" BorderStyle="Solid" ToolTip="Void">
					<asp:Image runat="server" imageurl="~/images/delete_16.png" style="vertical-align: middle"/>&nbsp;Void
				</asp:LinkButton>

				<asp:LinkButton ID="lbClose" runat="server" OnClick="lbClose_Click" Style="text-decoration: none; background-color: #7fc6f6; font-family: Tahoma; font-size: 14px; color: white; min-height: 22px; padding-left: 5px;"
					BorderStyle="Solid" ToolTip="Close" CssClass="StdBottomR">
					<asp:Image runat="server" imageurl="~/images/Exit_16.png" style="vertical-align: middle"/>&nbsp;Close&nbsp;
				</asp:LinkButton>

				<asp:Label ID="lblUser" runat="server" Text="User" Visible="false"></asp:Label>
				<asp:Label ID="lblGrpUser" runat="server" Text="Grp User" Visible="false"></asp:Label>

			</td>

			<td style="border: 1px solid #000000; min-width: 123px; background-color: lightgray; text-align: center; font-family: 'Segoe UI'; font-size: medium;">TC: &nbsp;
				<asp:Label ID="lblTC" runat="server" Text="53"></asp:Label>

			</td>

		</tr>
		<tr>
			<td style="border: 1px solid #000000; width: auto; background-color: lightgray;" colspan="5"></td>
		</tr>

	</table>

	<table style="width: 100%; font-family: sans-serif; font-size: small; float: left; background-color: lightgrey; border-spacing: 1px; margin-left: 2px; padding-left: 0px; padding-right: 0px;">
		<%--line1--%>
		<tr style="height: 26px; background-color: lightgray">
			<%--R1C1--%>
			<td style="border: 1px solid #000000; width: 80px; background-color: lightgray; text-align: right; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">Date:&nbsp;&nbsp;
			</td>

			<%--R1C2--%>
			<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: center; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">
				<asp:TextBox ID="dpTransDate" runat="server" TextMode="Date" Height="99%" Font-Names="Segoe UI"
					Font-Size="small" CssClass="DateBox" Width="96%"></asp:TextBox>

			</td>

			<%--R1C3--%>
			<td style="border: 1px solid #000000; width: 80px; background-color: lightgray; text-align: right; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">Inv. No.:&nbsp;&nbsp;
			</td>

			<%--R1C4--%>
			<td style="border: 1px solid #000000; width: 120px; background-color: lightgray; text-align: left; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">
				<asp:TextBox ID="txtInvNo" runat="server" Font-Names="Segoe UI" Font-Size="small" AutoPostBack="true" CssClass="txtBoxC"></asp:TextBox>
			</td>

			<%--R1C5--%>
			<td style="border: 1px solid #000000; width: 130px; background-color: lightgray; text-align: left; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">&nbsp;&nbsp;DSR No.:&nbsp;&nbsp;
				<asp:Label ID="lblDSRNo" runat="server" Text="00000" ForeColor="Red"></asp:Label>&nbsp;&nbsp;
			</td>

			<%--R1C6--%>
			<td style="width: 80px; background-color: lightgray; text-align: right; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">BA Chrg:&nbsp;&nbsp;</td>

			<%--R1C7--%>
			<td style="width: 120px; background-color: lightgray; text-align: left; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">&nbsp;&nbsp;
				<asp:Label ID="lblBAto" runat="server" Text="0000" ForeColor="Red"></asp:Label>&nbsp;&nbsp;
			</td>

			<%--R1C8--%>
			<td style="width: 80px; background-color: lightgray; text-align: right; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">Branch:&nbsp;&nbsp;</td>

			<%--R1C9--%>
			<td style="width: 120px; background-color: lightgray; text-align: left; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">&nbsp;&nbsp;
				<asp:Label ID="lblBranchTo" runat="server" Text="Branch" ForeColor="Red"></asp:Label>
			</td>

			<%--R1C10--%>
			<td style="border: 1px solid #000000; width: 80px; background-color: lightgray; text-align: right; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">Gross Amount:&nbsp;&nbsp;</td>

			<%--R1C11--%>
			<td style="border: 1px solid #000000; width: 120px; background-color: cornsilk; text-align: right; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">
				<asp:Label ID="lblGrossAmt" runat="server" Text="0.00" ForeColor="Red"></asp:Label>&nbsp;&nbsp;
			</td>
		</tr>

		<%--line2--%>
		<tr style="height: 26px; background-color: lightgray">
			<%--L2C1--%>
			<td style="border: 1px solid #000000; width: 80px; background-color: lightgray; text-align: right; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">DO No:&nbsp;&nbsp;

			</td>

			<%--L2C2--%>
			<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: center; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">
				<asp:TextBox ID="txtDONo" runat="server" Font-Names="Segoe UI" Font-Size="small" AutoPostBack="true" CssClass="txtBoxC"></asp:TextBox>

			</td>

			<%--L2C3--%>
			<td style="border: 1px solid #000000; width: 80px; background-color: lightgray; text-align: right; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">Salesman:&nbsp;&nbsp;
			</td>

			<%--L2C4to5--%>
			<td style="border: 1px solid #000000; width: 250px; background-color: lightgray; text-align: center; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;" colspan="2">
				<asp:DropDownList ID="cboSmnName" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" CssClass="cboBox2" Width="98%">
				</asp:DropDownList>&nbsp;
			</td>

			<%--L2C6--%>
			<td style="width: 80px; background-color: lightgray; text-align: right; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">Area:&nbsp;&nbsp;
			</td>

			<%--L2C7--%>
			<td style="width: 120px; background-color: lightgray; text-align: left; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">&nbsp;&nbsp;
				<asp:Label ID="lblArea" runat="server" Text="000" ForeColor="Red"></asp:Label>&nbsp;&nbsp;

			</td>
			<%--L2C8--%>
			<td style="width: 80px; background-color: lightgray; text-align: right; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">Term:&nbsp;&nbsp;</td>
			<%--L2C9--%>
			<td style="width: 120px; background-color: lightgray; text-align: left; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">&nbsp;&nbsp;
				<asp:Label ID="lblTerm" runat="server" Text="00" ForeColor="Red"></asp:Label>&nbsp;&nbsp;

			</td>
			<%--L2C10--%>
			<td style="border: 1px solid #000000; width: 80px; background-color: lightgray; text-align: right; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">
				<asp:CheckBox ID="CheckBox2" runat="server" ToolTip="Check If VATable" AutoPostBack="true" />
				&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
				VAT:&nbsp;&nbsp;</td>
			<%--L2C11--%>
			<td style="border: 1px solid #000000; width: 120px; background-color: cornsilk; text-align: right; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">
				<asp:Label ID="lblTaxes" runat="server" Text="0.00" ForeColor="Red"></asp:Label>&nbsp;&nbsp;
			</td>

		</tr>

		<%--line3--%>
		<tr style="height: 26px; background-color: lightgray">
			<%--L3C1--%>
			<td style="border: 1px solid #000000; width: 80px; background-color: lightgray; text-align: right; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">Sold To:&nbsp;&nbsp;
			</td>

			<%--L3C2to5--%>
			<td style="border: 1px solid #000000; width: 430px; background-color: lightgray; text-align: left; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;" colspan="4">
				<asp:DropDownList ID="cboCustName" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" CssClass="cboBox3" Width="99%">
				</asp:DropDownList>

			</td>

			<%--L3C6--%>
			<td style="border: 1px solid #000000; width: 80px; background-color: lightgray; text-align: right; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">PC:&nbsp;&nbsp;
				
			</td>

			<%--L3C7to8--%>
			<td style="border: 1px solid #000000; width: 200px; background-color: lightgray; text-align: center; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;" colspan="2">
				<asp:DropDownList ID="cboPC" runat="server" Font-Names="Segoe UI" Font-Size="small" CssClass="cboBox2" Width="98%">
				</asp:DropDownList>
			</td>

			<%--L3C9--%>
			<td style="width: 120px; background-color: lightgray; text-align: center; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">
				<asp:Label ID="lblCustType" runat="server" Text="Customer" ForeColor="Red"></asp:Label>&nbsp;&nbsp;
			</td>
			<%--L3C10--%>
			<td style="border: 1px solid #000000; width: 80px; background-color: lightgray; text-align: right; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">:&nbsp;&nbsp; 

			</td>
			<%--L3C11--%>
			<td style="border: 1px solid #000000; width: 120px; background-color: cornsilk; text-align: right; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">
				<asp:Label ID="txtFHamt" runat="server" Text="0.00" ForeColor="Red"></asp:Label>&nbsp;&nbsp;
			</td>

		</tr>
		<%--line4--%>
		<tr style="height: 26px; background-color: lightgray">
			<%--L4C1--%>
			<td style="border: 1px solid #000000; width: 80px; background-color: lightgray; text-align: right; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">Ship To:&nbsp;&nbsp;
			</td>
			<%--L4C2to5--%>
			<td style="border: 1px solid #000000; width: 430px; background-color: lightgray; text-align: left; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;" colspan="4">
				<asp:DropDownList ID="cboShipTo" runat="server" Font-Names="Segoe UI" Font-Size="small" CssClass="cboBox3" Width="99%">
				</asp:DropDownList>
				<%--<asp:LinkButton ID="lbShipTo" runat="server" Style="text-decoration: none; background-color: lightgray; float: right; padding-right: 2px;">
					<asp:Image runat="server" imageurl="~/images/check_16.png" style="vertical-align: middle"/>
				</asp:LinkButton>&nbsp;--%>
			</td>
			<%--L4C6--%>
			<td style="border: 1px solid #000000; width: 80px; background-color: lightgray; text-align: center; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">&nbsp;
				<asp:Label ID="lblCustVat" runat="server" Text="V" ForeColor="Red"></asp:Label>&nbsp;&nbsp;
			</td>
			<%--L4C7--%>
			<td style="border: 1px solid #000000; width: 120px; background-color: deepskyblue; text-align: center; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;" colspan="3" rowspan="2">Error Message: <
				<asp:Label ID="tssErrorMsg" runat="server" Text="Okay" ForeColor="White" Font-Size="Medium"></asp:Label>
				>
			</td>
			<%--L4C8to10--%>
			<td style="border: 1px solid #000000; background-color: lightgray; text-align: right; padding-left: 1px; font-family: 'Segoe UI'; font-size: medium;">Net Invoice:&nbsp;&nbsp;
			</td>
			<%--L4C11--%>
			<td style="border: 1px solid #000000; width: 120px; background-color: cornsilk; text-align: right; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">
				<asp:Label ID="txtDeductn" runat="server" Text="0.00" ForeColor="Red"></asp:Label>&nbsp;&nbsp;
			</td>
		</tr>
		<%--line5--%>
		<tr style="height: 26px; background-color: lightgray">
			<%--1--%>
			<td style="border: 1px solid #000000; width: 80px; background-color: lightgray; text-align: right; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">Ref No:&nbsp;&nbsp;
			</td>
			<%--2--%>
			<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: center; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">
				<asp:TextBox ID="txtRefNo" runat="server" Font-Names="Segoe UI" Font-Size="small" AutoPostBack="true" CssClass="TxtBox"></asp:TextBox>
			</td>
			<%--3--%>
			<td style="border: 1px solid #000000; width: 80px; background-color: lightgray; text-align: right; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">Remarks:&nbsp;&nbsp;
				
			</td>
			<%--4to6--%>
			<td style="border: 1px solid #000000; width: auto; background-color: lightgray; text-align: left; padding-left: 1px; padding-right: 2px; font-family: 'Segoe UI'; font-size: small;" colspan="3">
				<asp:TextBox ID="txtRemarks" runat="server" Font-Names="Segoe UI" Font-Size="small" CssClass="txtBoxL"></asp:TextBox>
			</td>
			<%--10--%>
			<td style="border: 1px solid #000000; background-color: lightgray; text-align: right; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">Net Invoice:&nbsp;&nbsp;
			</td>

			<td style="border: 1px solid #000000; width: 120px; background-color: cornsilk; text-align: right; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">
				<asp:Label ID="lblNetAmt" runat="server" Text="0.00" ForeColor="Red"></asp:Label>&nbsp;&nbsp;
			</td>

		</tr>

		<tr>
			<%--red line--%>
			<td style="border: 1px solid #000000; width: auto; height: auto; background-color: red;" colspan="12"></td>
		</tr>

	</table>

	<ajaxToolkit:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" Style="width: 100%; margin-left: 3px; height: auto;">
		<ajaxToolkit:TabPanel ID="TabPanel1" runat="server" HeaderText="SI Processing">
			<ContentTemplate>
				<table style="width: 100%; font-family: 'Segoe UI'; font-size: small; float: left; background-color: lightgrey; border-spacing: 0px; margin-left: 0px; padding-left: 0px; padding-right: 0px;">
					<tr style="height: 26px; background-color: lightgray">
						<td style="border: 1px solid #000000; width: 80px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" rowspan="2">Select</td>
						<td style="border: 1px solid #000000; width: 80px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Item No.</td>
						<td style="border: 1px solid #000000; width: 120px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Code No.</td>
						<td style="border: 1px solid #000000; width: 450px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="3">
							<asp:CheckBox ID="CheckBox4" runat="server" ToolTip="Check If Deal" Text="Reg" AutoPostBack="true" />
							&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
							Description &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
							<asp:Label ID="lblQtPk" runat="server" Text="0" ForeColor="Red"></asp:Label>
						</td>
						<td style="border: 1px solid #000000; width: 120px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Qty
							
						</td>
						<td style="border: 1px solid #000000; width: 120px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Wt/Vol
							
						</td>
						<td style="border: 1px solid #000000; width: 120px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:CheckBox ID="CheckBox3" runat="server" ToolTip="Check To Get Last Price" Text="SP" />
						</td>
						<td style="border: 1px solid #000000; width: 120px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Amount</td>
						<td style="border: 1px solid #000000; width: 120px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">&nbsp;&nbsp;Disc&nbsp;&nbsp;
							<asp:TextBox ID="txtDiscRate" runat="server" Font-Names="Segoe UI" Font-Size="Small" AutoPostBack="true" CssClass="txtAmtBox" Width="60px"></asp:TextBox>
						</td>
						<td style="border: 1px solid #000000; width: 120px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Net Amount
						</td>

						<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:Label ID="lblVNV" runat="server" Text="NV" ForeColor="Black"></asp:Label>
						</td>

						<td style="border: 1px solid #000000; width: 25px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" rowspan="2">
							<%--here--%>
							<asp:Label ID="lblBillRef" runat="server" Text="Wt" ForeColor="Red"></asp:Label>
						</td>

					</tr>
					<tr style="height: 26px; background-color: lightgray">
						<td style="border: 1px solid #000000; width: 80px; background-color: whitesmoke; text-align: center; padding-left: 1px; padding-right: 1px; font-family: 'Segoe UI'; font-size: small;">
							<asp:TextBox ID="txtItm" runat="server" Font-Names="Segoe UI" Font-Size="Small" Text="1" AutoPostBack="true" CssClass="TxtBox"></asp:TextBox>
						</td>
						<td style="border: 1px solid #000000; width: 120px; background-color: whitesmoke; text-align: center; padding-left: 1px; padding-right: 1px; font-family: 'Segoe UI'; font-size: small;">
							<asp:TextBox ID="txtCodeNo" runat="server" Font-Names="Segoe UI" Font-Size="Small" AutoPostBack="true" CssClass="TxtBox"></asp:TextBox>
						</td>
						<td style="border: 1px solid #000000; width: 450px; background-color: whitesmoke; text-align: center; font-family: 'Segoe UI'; font-size: small;" colspan="3">
							<asp:DropDownList ID="cboMMdesc" runat="server" AutoPostBack="True" Font-Names="Segoe UI" Font-Size="Small" CssClass="cboBox"></asp:DropDownList>
						</td>
						<td style="border: 1px solid #000000; width: 120px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:TextBox ID="txtQty" runat="server" Font-Names="Segoe UI" Font-Size="Small" AutoPostBack="true" CssClass="txtAmtBox"></asp:TextBox>
						</td>
						<td style="border: 1px solid #000000; width: 120px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:TextBox ID="txtWt" runat="server" Font-Names="Segoe UI" Font-Size="Small" AutoPostBack="true" CssClass="txtAmtBox"></asp:TextBox>
						</td>
						<td style="border: 1px solid #000000; width: 120px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:TextBox ID="txtSP" runat="server" Font-Names="Segoe UI" Font-Size="Small" AutoPostBack="true" CssClass="txtAmtBox"></asp:TextBox>
						</td>
						<td style="border: 1px solid #000000; width: 120px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">

							<asp:Label ID="lblGrossAmtDet" runat="server" Text="0.00" CssClass="lblLabelAmt"></asp:Label>
						</td>
						<td style="border: 1px solid #000000; width: 120px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:TextBox ID="txtDiscDet" runat="server" Font-Names="Segoe UI" Font-Size="Small" AutoPostBack="true" ReadOnly="true" CssClass="txtAmtBox"></asp:TextBox>
						</td>

						<td style="border: 1px solid #000000; width: 120px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:Label ID="lblNetAmtDet" runat="server" Text="0.00" CssClass="lblLabelAmt"></asp:Label>
						</td>

						<td style="border: 1px solid #000000; width: 100px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:Label ID="txtVATamt" runat="server" Text="0.00" CssClass="lblLabelAmt"></asp:Label>
						</td>

					</tr>

					<tr style="height: auto; background-color: lightgray">
						<td style="border: 1px solid #000000; height: 496px; background-color: floralwhite; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; vertical-align: top; margin-left: 0px" colspan="13">

							<asp:Panel ID="Panel1" runat="server" ScrollBars="Auto" CssClass="PanelStd">

								<asp:GridView ID="DgvSalesdet" runat="server" AutoGenerateColumns="False" OnRowDataBound="DgvSalesdet_RowDataBound" OnRowCreated="DgvSalesdet_RowCreated" Font-Names="Segoe UI" Font-Size="Small" BackColor="White" BorderColor="#999999" BorderStyle="Solid"
									BorderWidth="1px" CellPadding="3" GridLines="Vertical" ShowHeader="False" CssClass="Grid1">
									<AlternatingRowStyle BackColor="Gainsboro" />
									<Columns>
										<asp:CommandField ShowSelectButton="True">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="74px"></ItemStyle>
										</asp:CommandField>
										<asp:BoundField DataField="itemno">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="84px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="codeno">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="124px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="mmdesc">
											<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="388px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="qty" DataFormatString="{0:#,##0;(#,##0)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="126px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="wt" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="126px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="sp" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="123px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="itmamt" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="122px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="detdiscamt" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="123px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="detgrossamt" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="120px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="detvat" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="100px"></ItemStyle>
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

						</td>

						<td style="border: 1px solid #000000; width: 25px; background-color: beige; text-align: center; vertical-align: text-top; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:ImageButton ID="btnReset" runat="server" ImageUrl="~/images/reset_16.png" CssClass="ImgBtn" />
							<asp:ImageButton ID="btnAdd" runat="server" ImageUrl="~/images/check_16.png" CssClass="ImgBtn" />
							<asp:ImageButton ID="btnDel" runat="server" ImageUrl="~/images/delete_16.png" />

						</td>

					</tr>

					<tr style="height: 26px; background-color: lightgray">

						<td style="border: 1px solid #000000; width: auto; background-color: lightgray; text-align: center; font-family: 'Segoe UI'; font-size: small;" colspan="6">
							<asp:TextBox ID="txtApprvdBy" runat="server" Text="LVReyes" Font-Names="Segoe UI" Font-Size="small" CssClass="TxtBox" Width="120px"></asp:TextBox>
							&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
							Total &nbsp;&nbsp;
						</td>

						<td style="border: 1px solid #000000; width: 120px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:Label ID="lblTotQty" runat="server" Text="0" ForeColor="Red" CssClass="lblLabelAmt"></asp:Label>
						</td>

						<td style="border: 1px solid #000000; width: 120px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:Label ID="lblTotWt" runat="server" Text="0.00" ForeColor="Red" CssClass="lblLabelAmt"></asp:Label>
						</td>

						<td style="border: 1px solid #000000; width: 120px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>

						<td style="border: 1px solid #000000; width: 120px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:Label ID="lblTotGross" runat="server" CssClass="lblLabelAmt" ForeColor="Red" Text="0.00"></asp:Label>
						</td>

						<td style="border: 1px solid #000000; width: 120px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:Label ID="lblTotDisc" runat="server" Text="0.00" ForeColor="Red" CssClass="lblLabelAmt"></asp:Label>
						</td>

						<td style="border: 1px solid #000000; width: 120px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:Label ID="lblTotNetAmt" runat="server" Text="0.00" ForeColor="Red" CssClass="lblLabelAmt"></asp:Label>
						</td>

						<td style="border: 1px solid #000000; width: 100px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:Label ID="lblTotVatAmt" runat="server" Text="0.00" ForeColor="Red" CssClass="lblLabelAmt"></asp:Label>
						</td>

						<td style="border: 1px solid #000000; width: 25px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>

					</tr>

					<tr style="height: auto; background-color: lightgray">
						<td style="border: 1px solid #000000; height: 20px; background-color: lightgray; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="14">&nbsp;&nbsp; 
							Save Status:&nbsp;&nbsp;
							<asp:Label ID="tsSaveStat" runat="server" Text="Not yet Saved" ForeColor="Red"></asp:Label>
							&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
							Doc Status: &nbsp;&nbsp;
							<asp:Label ID="tssDocStat" runat="server" Text="New" ForeColor="Red"></asp:Label>
							&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
							Edit Doc No.: &nbsp;&nbsp;
							<asp:Label ID="tsDocNoEdit" runat="server" Text="" ForeColor="Red"></asp:Label>
							&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
							<asp:Label ID="tssVoidReqNo" runat="server" Text="" ForeColor="Red"></asp:Label>
							&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
							Price Type:&nbsp;&nbsp
							<asp:Label ID="lblSPtype" runat="server" Text="Customer" ForeColor="Red"></asp:Label>
							<%--&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Bill Ref: &nbsp;&nbsp;--%>
			
				&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Line Item: &nbsp;&nbsp;
				<asp:Label ID="lblLineItm" runat="server" Text="New" ForeColor="Red"></asp:Label>

						</td>
					</tr>


				</table>

			</ContentTemplate>

		</ajaxToolkit:TabPanel>

		<%--SO List--%>
		<ajaxToolkit:TabPanel ID="TabPanel2" runat="server" HeaderText="Available DO">
			<ContentTemplate>
				<table style="width: 100%; font-family: 'Segoe UI'; font-size: small; float: left; background-color: lightgrey; border-spacing: 0px; margin-left: 0px; padding-left: 0px; padding-right: 0px;">
					<tr style="height: 26px; background-color: lightgray">
						<td style="border: 1px solid #000000; width: 200px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="2">Customers:&nbsp;&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: 650px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:DropDownList ID="CboCustList" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" CssClass="cboBox3">
							</asp:DropDownList>
							<asp:LinkButton ID="LbCustList" runat="server" Style="text-decoration: none; background-color: lightgray; float: right; padding-right: 2px;">
							<asp:Image runat="server" imageurl="~/images/check_16.png" style="vertical-align: middle"/>
							</asp:LinkButton>&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: auto; background-color: lightgray; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="5"></td>

					</tr>

					<tr style="background-color: lightgray">
						<td style="border: 1px solid #000000; height: 361px; background-color: floralwhite; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; vertical-align: top; margin-left: 0px" colspan="8">

							<asp:Panel ID="Panel2" runat="server" Width="100%" Height="392px">
								<asp:GridView ID="DgvDOList" runat="server" AutoGenerateColumns="False" OnRowDataBound="DgvDOList_RowDataBound" OnRowCreated="DgvDOList_RowCreated" Font-Names="Segoe UI" Font-Size="small" BackColor="White" BorderColor="#999999" BorderStyle="Solid" AllowSorting="true" BorderWidth="1px" CellPadding="3" GridLines="Vertical" HeaderStyle-BorderColor="Black" ShowHeader="true">
									<AlternatingRowStyle BackColor="#DCDCDC" />
									<Columns>
										<asp:CommandField HeaderText="Select" ShowSelectButton="true">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="72px"></ItemStyle>
										</asp:CommandField>

										<asp:BoundField DataField="dono" HeaderText="DO No.">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="110px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField HeaderText="Date" DataField="transdate" DataFormatString="{0:yyyy-MM-dd}">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="custno" HeaderText="Sold To">
											<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="500px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="shipto" HeaderText="Ship To">
											<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="500px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="status" HeaderText="Status">
											<ItemStyle HorizontalAlign="left" VerticalAlign="Middle" Width="150px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="sono" HeaderText="SO No.">
											<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="100px"></ItemStyle>
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
							<script src="Scripts/ScrollableGridViewPlugin_ASP.NetAJAXmin.js" type="text/javascript">

							</script>
							<script type="text/javascript">
								$(document).ready(function () {
									$('#<%=DgvDOList.ClientID %>').Scrollable({
										ScrollHeight: 367,
										IsInPanel: true
									});
								});
							</script>
						</td>
					</tr>
					<tr style="background-color: lightgray">
						<td style="background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="8"></td>
					</tr>
					<tr style="background-color: lightgray">
						<td style="border: 1px solid #000000; height: 155px; background-color: floralwhite; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; vertical-align: top; margin-left: 0px" colspan="8">
							<asp:Panel ID="Panel3" runat="server" Width="100%" Height="153px">
								<asp:GridView ID="DgvDOdetList" runat="server" AutoGenerateColumns="False" OnRowDataBound="DgvDOdetList_RowDataBound" OnRowCreated="DgvDOdetList_RowCreated" Font-Names="Segoe UI" Font-Size="small" BackColor="White" BorderColor="#999999" BorderStyle="Solid" AllowSorting="true" BorderWidth="1px" CellPadding="3" GridLines="Vertical" HeaderStyle-BorderColor="Black" ShowHeader="true">
									<AlternatingRowStyle BackColor="#DCDCDC" />
									<Columns>
										<asp:BoundField DataField="itmno" HeaderText="Itm No.">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="80px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="codeno" HeaderText="Code No.">
											<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="120px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="mmdesc" HeaderText="Product Description">
											<ItemStyle HorizontalAlign="left" VerticalAlign="Middle" Width="693px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="qty" HeaderText="Qty" DataFormatString="{0:#,##0;(#,##0)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="113px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="wt" HeaderText="Wt/Vol" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="111px"></ItemStyle>
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
							<script src="Scripts/ScrollableGridViewPlugin_ASP.NetAJAXmin.js" type="text/javascript">

							</script>
							<script type="text/javascript">
								$(document).ready(function () {
									$('#<%=DgvDOdetList.ClientID %>').Scrollable({
										ScrollHeight: 128,
										IsInPanel: true
									});
								});
							</script>
						</td>
					</tr>
					<tr style="height: 22px; background-color: lightgray">
						<td style="border: 1px solid #000000; width: 850px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="3">Total&nbsp;&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: 117px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:Label ID="lblTotQtyT2" runat="server" Text="0" ForeColor="Red"></asp:Label>&nbsp;&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: 117px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:Label ID="lblTotWtT2" runat="server" Text="0.00" ForeColor="Red"></asp:Label>&nbsp;&nbsp;
						</td>
						<%--<td style="border: 1px solid #000000; width: 117px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
													
						</td>--%>
						<td style="border: 1px solid #000000; width: 300px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="3"></td>
					</tr>


				</table>

			</ContentTemplate>

		</ajaxToolkit:TabPanel>

		<%--SO Monitoring--%>
		<ajaxToolkit:TabPanel ID="TabPanel3" runat="server" HeaderText="SI Summary">
			<ContentTemplate>
				<table style="width: 100%; font-family: 'Segoe UI'; font-size: small; float: left; background-color: lightgrey; border-spacing: 0px; margin-left: 0px; padding-left: 0px; padding-right: 0px;">
					<tr style="height: 26px; background-color: lightgray">
						<td style="border: 1px solid #000000; width: 80px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Date From:&nbsp;&nbsp;
						</td>

						<td style="border: 1px solid #000000; width: 150px; background-color: lightgray; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:TextBox ID="dpDateFrT3" runat="server" TextMode="Date" Width="120px" Height="99%" Font-Names="Segoe UI" Font-Size="small" AutoPostBack="true" CssClass="DateBox"></asp:TextBox>
						</td>

						<td style="border: 1px solid #000000; width: 80px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Date To:&nbsp;&nbsp;
						</td>

						<td style="border: 1px solid #000000; width: 150px; background-color: lightgray; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:TextBox ID="dpDateToT3" runat="server" TextMode="Date" Width="120px" Height="99%" Font-Names="Segoe UI" Font-Size="small" AutoPostBack="true" CssClass="DateBox"></asp:TextBox>
						</td>

						<td style="border: 1px solid #000000; width: 120px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Status:&nbsp;&nbsp;
						</td>

						<td style="border: 1px solid #000000; width: 150px; background-color: lightgray; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:DropDownList ID="cboSIstatus" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" CssClass="cboBox3">
							</asp:DropDownList>

						</td>

						<td style="border: 1px solid #000000; width: auto; background-color: lightgray; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:LinkButton ID="lbSIstatus" runat="server" Style="text-decoration: none; background-color: lightgray; float: left; padding-right: 2px;">
							<asp:Image runat="server" imageurl="~/images/check_16.png" style="vertical-align: middle"/>
							</asp:LinkButton>&nbsp;
						</td>

					</tr>
					<tr style="background-color: lightgray">
						<td style="border: 1px solid #000000; height: 574px; background-color: floralwhite; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; vertical-align: top; margin-left: 0px" colspan="7">
							<asp:Panel ID="Panel4" runat="server" Width="100%" Height="541px">
								<asp:GridView ID="DgvSIsumList" runat="server" AutoGenerateColumns="False" OnRowDataBound="DgvSIsumList_RowDataBound" OnRowCreated="DgvSIsumList_RowCreated" Font-Names="Segoe UI" Font-Size="small" BackColor="White" BorderColor="#999999" BorderStyle="Solid" AllowSorting="true" BorderWidth="1px" CellPadding="3" GridLines="Vertical" HeaderStyle-BorderColor="Black" ShowHeader="true">
							<AlternatingRowStyle BackColor="#DCDCDC" />
									<Columns>
										<asp:CommandField HeaderText="Select" ShowSelectButton="true">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="72px"> 

											</ItemStyle>
										</asp:CommandField>

										<asp:BoundField DataField="doctype" HeaderText="Doc Type">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="80px">

											</ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="invno" HeaderText="Doc No.">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px">

											</ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="docno" HeaderText="Ref No.">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px">

											</ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="transdate" HeaderText="Date" DataFormatString="{0:yyyy-MM-dd}">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px">

											</ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="custno" HeaderText="Sold To">
											<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="400px">

											</ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="shipto" HeaderText="Shipt To">
											<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="400px">

											</ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="dono" HeaderText="DO No.">
											<ItemStyle HorizontalAlign="center" VerticalAlign="Middle" Width="100px">

											</ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="status" HeaderText="Status">
											<ItemStyle HorizontalAlign="left" VerticalAlign="Middle" Width="100px">

											</ItemStyle>
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
									$('#<%=DgvSIsumList.ClientID %>').Scrollable({
										ScrollHeight: 547,
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
