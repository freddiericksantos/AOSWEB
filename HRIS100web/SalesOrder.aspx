<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SD.master" CodeBehind="SalesOrder.aspx.vb" Inherits="AOS100web.SalesOrder" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentOfSD" runat="server">

	<style>
		.Grid1 {
			margin-left: 1px;
			margin-right: 1px;
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
			if (confirm("Do you sure to RESET Fields?")) {
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
			if (confirm("Are you SURE to DELETE Selected Line Item?")) {
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
				<asp:LinkButton ID="lbNew" runat="server" OnClick="OnConfirm" OnClientClick="Confirm()" Style="text-decoration: none; background-color: #7fc6f6; font-family: Tahoma; font-size: 14px; color: white; margin-right: 5px; padding-left: 5px; padding-right: 5px; min-height: 22px;"
					BorderStyle="Solid" ToolTip="Reset Fields" CssClass="StdBottomL">
					<asp:Image runat="server" imageurl="~/images/new_16.png" style="vertical-align: middle"/>&nbsp;New
				</asp:LinkButton>
				<asp:LinkButton ID="lbSave" runat="server" OnClick="lbSave_Click" Style="text-decoration: none; background-color: #7fc6f6; padding-right: 5px; font-family: Tahoma; font-size: 14px; color: white; margin-right: 5px; padding-left: 5px; min-height: 22px;"
					BorderStyle="Solid" ToolTip="Save">
					<asp:Image runat="server" imageurl="~/images/save_orange_16.png" style="vertical-align: middle"/>&nbsp;Save
				</asp:LinkButton>
				<asp:LinkButton ID="lbPrint" runat="server" OnClick="lbPrint_Click" Style="text-decoration: none; background-color: #7fc6f6; padding-right: 5px; font-family: Tahoma; font-size: 14px; color: white; margin-right: 5px; padding-left: 5px; min-height: 22px;"
					BorderStyle="Solid" ToolTip="Print" Enabled="true">
					<asp:Image runat="server" imageurl="~/images/Print_16.png" style="vertical-align: middle"/>&nbsp;Print
				</asp:LinkButton>
				<asp:LinkButton ID="lbDelete" runat="server" OnClick="lbDelete_Click" Style="text-decoration: none; background-color: #7fc6f6; padding-right: 5px; font-family: Tahoma; font-size: 14px; color: white; margin-right: 5px; padding-left: 5px; min-height: 22px;"
					BorderStyle="Solid" ToolTip="Void" Enabled="false">
					<asp:Image runat="server" imageurl="~/images/delete_16.png" style="vertical-align: middle"/>&nbsp;Void
				</asp:LinkButton>
				<asp:LinkButton ID="lbClose" runat="server" OnClick="lbClose_Click" Style="text-decoration: none; background-color: #7fc6f6; font-family: Tahoma; font-size: 14px; color: white; min-height: 22px; padding-left: 5px;"
					BorderStyle="Solid" ToolTip="Close" CssClass="StdBottomR">
					<asp:Image runat="server" imageurl="~/images/Exit_16.png" style="vertical-align: middle"/>&nbsp;Close&nbsp;
				</asp:LinkButton>

				<asp:Label ID="lblUser" runat="server" Text="User" Visible="false"></asp:Label>
				<asp:Label ID="lblGrpUser" runat="server" Text="Grp User" Visible="false"></asp:Label>

			</td>

			<td style="border: 1px solid #000000; min-width: 120px; background-color: lightgray; text-align: center; font-family: 'Segoe UI'; font-size: medium;">TC: &nbsp;
				<asp:Label ID="lblTC" runat="server" Text="05"></asp:Label>

			</td>

		</tr>
		<tr>
			<td style="border: 1px solid #000000; width: auto; background-color: lightgray;" colspan="5"></td>
		</tr>

	</table>

	<table style="width: 100%; font-family: 'Segoe UI'; font-size: small; float: left; background-color: lightgrey; border-spacing: 1px; margin-left: 2px; padding-left: 0px; padding-right: 0px;">
		<%--line1--%>
		<tr style="height: 26px; background-color: lightgray">
			<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">Date:&nbsp;&nbsp;
			</td>

			<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: left; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">
				<asp:TextBox ID="dpTransDate" runat="server" TextMode="Date" Width="120px" Height="99%" Font-Names="Segoe UI"
					Font-Size="small" CssClass="DateBox"></asp:TextBox>
			</td>

			<td style="border: 1px solid #000000; width: 80px; background-color: lightgray; text-align: right; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">
				<asp:LinkButton ID="lbReLoadSO" runat="server" Style="text-decoration: none; background-color: lightgray; float: left; padding-right: 2px;">&nbsp;
					<asp:Image runat="server" imageurl="~/images/check_16.png" style="vertical-align: middle"/>
				</asp:LinkButton>
				SO No.:&nbsp;&nbsp;
			</td>

			<td style="border: 1px solid #000000; width: 120px; background-color: lightgray; text-align: left; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">
				<asp:TextBox ID="txtSOno" runat="server" Font-Names="Segoe UI" Font-Size="small" ReadOnly="true" AutoPostBack="true" CssClass="TxtBox"></asp:TextBox>
			</td>
			<td style="border: 1px solid #000000; width: 80px; background-color: lightgray; text-align: right; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">Salesman:&nbsp;&nbsp;
			</td>

			<td style="border: 1px solid #000000; width: 250px; background-color: lightgray; text-align: left; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">
				<asp:DropDownList ID="cboSmnName" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" CssClass="cboBox">
				</asp:DropDownList>
			</td>

			<td style="border: 1px solid #000000; width: auto; background-color: lightgray; text-align: center; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;" colspan="2">
				<asp:CheckBox ID="CheckBox1" runat="server" Text="Reload Only" />
			</td>
			<td style="border: 1px solid #000000; width: auto; background-color: lightblue; text-align: center; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;" colspan="2" rowspan="2">
				<asp:Label ID="lblSOStat" runat="server" Text="Credit Status" ForeColor="Red" Font-Size="Large" Font-Bold="true"></asp:Label>
			</td>

			<td style="border: 1px solid #000000; width: auto; background-color: khaki; text-align: center; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;" colspan="2" rowspan="2">Error Message <
				<asp:Label ID="lblErrMsg" runat="server" Text="No Error" Font-Size="Medium"></asp:Label> >
			</td>

		</tr>
		<%--line2--%>
		<tr style="height: 26px; background-color: lightgray">
			<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">Del Date:&nbsp;&nbsp;
			</td>
			<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: left; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">
				<asp:TextBox ID="dpDelDate" runat="server" TextMode="Date" Width="120px" Height="20px" Font-Names="Segoe UI"
					Font-Size="small" CssClass="DateBox"></asp:TextBox>
			</td>

			<td style="border: 1px solid #000000; width: 80px; background-color: lightgray; text-align: right; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">PO No.:&nbsp;&nbsp;
			</td>

			<td style="border: 1px solid #000000; width: 120px; background-color: lightgray; text-align: left; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">
				<asp:TextBox ID="txtPONo" runat="server" Font-Names="Segoe UI" Font-Size="small" AutoPostBack="true" CssClass="TxtBox"></asp:TextBox>
			</td>

			<td style="border: 1px solid #000000; width: 80px; background-color: lightgray; text-align: right; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">Plant:&nbsp;&nbsp;
			</td>

			<td style="border: 1px solid #000000; width: 250px; background-color: lightgray; text-align: left; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">
				<asp:DropDownList ID="cboPlnt" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" CssClass="cboBox">
				</asp:DropDownList>
			</td>

			<td style="border: 1px solid #000000; width: auto; background-color: lightgray; text-align: right; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;" colspan="2">&nbsp;PC:&nbsp;
				 <asp:DropDownList ID="cboPClass" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" CssClass="cboBox2">
				 </asp:DropDownList>
			</td>
		</tr>
		<%--line3--%>
		<tr style="height: 26px; background-color: lightgray">
			<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">Sold To:&nbsp;&nbsp;
			</td>
			<td style="border: 1px solid #000000; width: 650px; background-color: lightgray; text-align: left; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;" colspan="5">
				<asp:DropDownList ID="cboCustName" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" CssClass="cboBox">
				</asp:DropDownList>
			</td>

			<td style="border: 1px solid #000000; width: auto; background-color: lightgray; text-align: left; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;" colspan="6">
				<asp:TextBox ID="txtSearch" runat="server" Width="250" Font-Names="Segoe UI" placeholder="Search Customer" Font-Size="Small" TextMode="Search" AutoPostBack="true" ></asp:TextBox>
				<asp:LinkButton ID="lbSearch" runat="server" OnClick="lbSearch_Click" Style="text-decoration: none; background-color: lightgray; float: inherit; padding-right: 2px;">
					<asp:Image runat="server" imageurl="~/images/search_red16.png" style="vertical-align: middle"/>
				</asp:LinkButton>
				&nbsp;&nbsp;&nbsp;&nbsp;
				Area No.:
				<asp:Label ID="lblArea" runat="server" Text="000" ForeColor="Red"></asp:Label>
				&nbsp;&nbsp;&nbsp;&nbsp;
				Terms:
				<asp:Label ID="lblTerm" runat="server" Text="0" ForeColor="Red"> Days </asp:Label>
				&nbsp;&nbsp;&nbsp;&nbsp;
				Cust Type:
				<asp:Label ID="lblCustType" runat="server" Text="Customer" ForeColor="Red"> </asp:Label>
				&nbsp;&nbsp;&nbsp;&nbsp;
			</td>
		</tr>
		<%--line4--%>
		<tr style="height: 26px; background-color: lightgray">
			<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">Ship To:&nbsp;&nbsp;
			</td>
			<td style="border: 1px solid #000000; width: 650px; background-color: lightgray; text-align: left; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;" colspan="5">
				<asp:DropDownList ID="cboShipTo" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" CssClass="cboBox">
				</asp:DropDownList>
			</td>

			<td style="border: 1px solid #000000; width: 250px; background-color: lightgray; text-align: left; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;" colspan="2">&nbsp;&nbsp;
				Pricelist:&nbsp;
				<asp:Label ID="lblSPtype" runat="server" Text="Customer" ForeColor="Red"></asp:Label>
			</td>

			<td style="border: 1px solid #000000; width: 120px; background-color: lightgray; text-align: center; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">Credit Limit
			</td>

			<td style="border: 1px solid #000000; width: 120px; background-color: lightgray; text-align: center; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">PDC
			</td>

			<td style="border: 1px solid #000000; width: 120px; background-color: lightgray; text-align: center; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">Deposit
			</td>

			<td style="border: 1px solid #000000; width: 120px; background-color: lightgray; text-align: center; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">Total A/R
			</td>

		</tr>
		<%--line5&6--%>
		<tr style="height: 26px; background-color: lightgray">
			<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;" rowspan="2">Address:&nbsp;&nbsp;
			</td>
			<td style="border: 1px solid #000000; width: 650px; height: 38px; background-color: lightgray; text-align: left; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;" colspan="5" rowspan="2">
				<asp:Label ID="lblAddress" runat="server" Text="Address" Height="100%"></asp:Label>
			</td>

			<td style="border: 1px solid #000000; width: 120px; height: 26px; background-color: lightgray; text-align: left; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;" colspan="2" rowspan="1">&nbsp;&nbsp;
				Customer's Account Aging: 
			</td>

			<td style="border: 1px solid #000000; width: 120px; background-color: oldlace; text-align: center; padding-left: 2px; font-family: 'Segoe UI'; font-size: small;">
				<asp:Label ID="lblCRLimit" runat="server" Text="0.00" ForeColor="Red"></asp:Label>
			</td>
			<td style="border: 1px solid #000000; width: 120px; background-color: oldlace; text-align: center; padding-left: 2px; font-family: 'Segoe UI'; font-size: small;">
				<asp:Label ID="lblPDC" runat="server" Text="0.00" ForeColor="Red"></asp:Label>
			</td>
			<td style="border: 1px solid #000000; width: 120px; background-color: oldlace; text-align: center; padding-left: 2px; font-family: 'Segoe UI'; font-size: small;">
				<asp:Label ID="lblDepAmt" runat="server" Text="0.00" ForeColor="Red"></asp:Label>
			</td>
			<td style="border: 1px solid #000000; width: 120px; background-color: oldlace; text-align: center; padding-left: 2px; font-family: 'Segoe UI'; font-size: small;">
				<asp:Label ID="lblAcctBal" runat="server" Text="0.00" ForeColor="Red"></asp:Label>
			</td>


		</tr>
		<%--line6--%>
		<tr style="height: 26px; background-color: lightgray">
			<td style="border: 1px solid #000000; width: 120px; background-color: lightgray; text-align: center; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">1-30 Days
			</td>
			<td style="border: 1px solid #000000; width: 120px; background-color: lightgray; text-align: center; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">31-60 Days
			</td>
			<td style="border: 1px solid #000000; width: 120px; background-color: lightgray; text-align: center; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">61-90 Days
			</td>
			<td style="border: 1px solid #000000; width: 120px; background-color: lightgray; text-align: center; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">91-120 Days
			</td>
			<td style="border: 1px solid #000000; width: 120px; background-color: lightgray; text-align: center; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">121-150 Days
			</td>
			<td style="border: 1px solid #000000; width: 120px; background-color: lightgray; text-align: center; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">Over 151 Days
			</td>
		</tr>
		<%--line7--%>
		<tr style="height: 26px; background-color: lightgray">
			<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">Remarks:&nbsp;&nbsp;
			</td>

			<td style="border: 1px solid #000000; width: 650px; background-color: lightgray; text-align: left; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;" colspan="5">
				<asp:TextBox ID="txtRemarks" runat="server" AutoPostBack="true" CssClass="txtBoxL" ></asp:TextBox>
			</td>

			<td style="border: 1px solid #000000; width: 120px; background-color: oldlace; text-align: center; padding-left: 2px; font-family: 'Segoe UI'; font-size: small;">
				<asp:Label ID="lbl30days" runat="server" Text="0.00" ForeColor="Red"></asp:Label>
			</td>
			<td style="border: 1px solid #000000; width: 120px; background-color: oldlace; text-align: center; padding-left: 2px; font-family: 'Segoe UI'; font-size: small;">
				<asp:Label ID="lbl60days" runat="server" Text="0.00" ForeColor="Red"></asp:Label>
			</td>
			<td style="border: 1px solid #000000; width: 120px; background-color: oldlace; text-align: center; padding-left: 2px; font-family: 'Segoe UI'; font-size: small;">
				<asp:Label ID="lbl90days" runat="server" Text="0.00" ForeColor="Red"></asp:Label>
			</td>
			<td style="border: 1px solid #000000; width: 120px; background-color: oldlace; text-align: center; padding-left: 2px; font-family: 'Segoe UI'; font-size: small;">
				<asp:Label ID="lbl91over" runat="server" Text="0.00" ForeColor="Red"></asp:Label>
			</td>
			<td style="border: 1px solid #000000; width: 120px; background-color: oldlace; text-align: center; padding-left: 2px; font-family: 'Segoe UI'; font-size: small;">
				<asp:Label ID="lbl121over" runat="server" Text="0.00" ForeColor="Red"></asp:Label>
			</td>
			<td style="border: 1px solid #000000; width: 120px; background-color: oldlace; text-align: center; padding-left: 2px; font-family: 'Segoe UI'; font-size: small;">
				<asp:Label ID="lbl150days" runat="server" Text="0.00" ForeColor="Red"></asp:Label>
			</td>

		</tr>

		<tr>
			<%--red line--%>
			<td style="border: 1px solid #000000; width: auto; height: auto; background-color: red;" colspan="12"></td>
		</tr>
	</table>

	<%--Insert Tab Here--%>
	<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

	<ajaxToolkit:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" Style="width: 100%; margin-left:3px;height: auto;">
		<ajaxToolkit:TabPanel ID="TabPanel1" runat="server" HeaderText="Order Processing" >
			<ContentTemplate>
				<table style="width: 100%; font-family: 'Segoe UI'; font-size: small; float: left; background-color: lightgrey; border-spacing: 0px; margin-left: 0px; padding-left: 0px; padding-right: 0px;">
					<tr style="height: 26px; background-color: lightgray">
						<td style="border: 1px solid #000000; width: 80px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" rowspan="2">Select</td>
						<td style="border: 1px solid #000000; width: 80px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Item No.</td>
						<td style="border: 1px solid #000000; width: 120px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Code No.</td>
						<td style="border: 1px solid #000000; width: 450px; background-color: lightgray; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="3">&nbsp;&nbsp;&nbsp;
							<asp:Button ID="Button2" runat="server" Text="Available" CssClass="SmlButton" />&nbsp;&nbsp;&nbsp;
							<asp:Button ID="Button1" runat="server" Text="Load All" CssClass="SmlButton" />&nbsp;&nbsp;&nbsp;
							Description &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
							<asp:Label ID="lblQtPk" runat="server" Text="0" ForeColor="Red"></asp:Label>
						</td>
						<td style="border: 1px solid #000000; width: 120px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Qty
							
						</td>
						<td style="border: 1px solid #000000; width: 120px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Wt/Vol
							<asp:CheckBox ID="CheckBox4" runat="server" OnCheckedChanged="CheckBox4_CheckedChanged" ToolTip="Check to Auto Compute Wt"></asp:CheckBox>
						</td>
						<td style="border: 1px solid #000000; width: 120px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:CheckBox ID="CheckBox2" runat="server" ToolTip="Check To Get Last Price" Text="" />
							SP</td>
						<td style="border: 1px solid #000000; width: 120px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Amount</td>
						<td style="border: 1px solid #000000; width: 120px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:CheckBox ID="CheckBox3" runat="server" OnCheckedChanged="CheckBox3_CheckedChanged" Text="Disc" ToolTip="Check for Deal Item" AutoPostBack="true" />
							&nbsp;&nbsp;
							<asp:TextBox ID="txtDiscRate" runat="server" Width="40px" Text="0.00" CssClass="txtBoxPer">
							</asp:TextBox>
						</td>
						<td style="border: 1px solid #000000; width: 120px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Net Amount
						</td>
						<td style="border: 1px solid #000000; width: 25px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" rowspan="2">
							<%--here--%>
							<asp:Label ID="lblBillRef" runat="server" Text="Wt" ForeColor="Red"></asp:Label>
						</td>
					</tr>
					<tr style="height: 26px; background-color: lightgray">
						<td style="border: 1px solid #000000; width: 80px; background-color: whitesmoke; text-align: center; padding-left: 1px; padding-right: 1px; font-family: 'Segoe UI'; font-size: small;">
							<asp:TextBox ID="txtItm" runat="server" Font-Names="Segoe UI" Font-Size="Small" Text="1" AutoPostBack="true"  CssClass="TxtBox"></asp:TextBox>
						</td>
						<td style="border: 1px solid #000000; width: 120px; background-color: whitesmoke; text-align: center; padding-left: 1px; padding-right: 1px; font-family: 'Segoe UI'; font-size: small;">
							<asp:TextBox ID="txtCodeNo" runat="server" Font-Names="Segoe UI" Font-Size="Small" AutoPostBack="true" CssClass="TxtBox"></asp:TextBox>
						</td>
						<td style="border: 1px solid #000000; width: 450px; background-color: whitesmoke; text-align: center; font-family: 'Segoe UI'; font-size: small;" colspan="3">
							<asp:DropDownList ID="cboMMdesc" runat="server" AutoPostBack="True" Font-Names="Segoe UI" Font-Size="Small" CssClass="cboBox"></asp:DropDownList>
						</td>
						<td style="border: 1px solid #000000; width: 120px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:TextBox ID="txtQty" runat="server" Font-Names="Segoe UI" Font-Size="Small" AutoPostBack="true"  CssClass="txtAmtBox"></asp:TextBox>
						</td>
						<td style="border: 1px solid #000000; width: 120px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:TextBox ID="txtWt" runat="server" Font-Names="Segoe UI" Font-Size="Small" AutoPostBack="true" CssClass="txtAmtBox"></asp:TextBox>
						</td>
						<td style="border: 1px solid #000000; width: 120px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:TextBox ID="txtSP" runat="server" Font-Names="Segoe UI" Font-Size="Small" AutoPostBack="true" CssClass="txtAmtBox"></asp:TextBox>
						</td>
						<td style="border: 1px solid #000000; width: 120px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">

							<asp:Label ID="lblAmt" runat="server" Text="0.00" CssClass="lblLabelAmt"></asp:Label>
						</td>
						<td style="border: 1px solid #000000; width: 120px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:TextBox ID="txtDiscAmt" runat="server" Font-Names="Segoe UI" Font-Size="Small" AutoPostBack="true" CssClass="txtAmtBox"></asp:TextBox>
						</td>

						<td style="border: 1px solid #000000; width: 120px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:Label ID="lblNetAmt" runat="server" Text="0.00" CssClass="lblLabelAmt"></asp:Label>
						</td>
					</tr>

					<tr style="height: auto; background-color: lightgray">
						<td style="border: 1px solid #000000; height: 435px; background-color: floralwhite; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; vertical-align: top; margin-left: 0px" colspan="12">

							<asp:Panel ID="Panel1" runat="server" ScrollBars="Auto" CssClass="PanelStd">

								<asp:GridView ID="DgvSOdet" runat="server" AutoGenerateColumns="False" OnRowDataBound="DgvSOdet_RowDataBound"
									OnRowCreated="DgvSOdet_RowCreated" Font-Names="Segoe UI" Font-Size="Small" BackColor="White" BorderColor="#999999" BorderStyle="Solid"
									BorderWidth="1px" CellPadding="3" GridLines="Vertical" ShowHeader="False" CssClass="Grid1">
									<AlternatingRowStyle BackColor="Gainsboro" />
									<Columns>
										<asp:CommandField ShowSelectButton="True">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="74px"></ItemStyle>
										</asp:CommandField>
										<asp:BoundField DataField="itmno">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="79px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="codeno">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="119px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="mmdesc">
											<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="471px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="qty" DataFormatString="{0:#,##0;(#,##0)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="117px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="wt" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="118px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="sp" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="118px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="itmamt" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="118px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="discamt" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="116px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="netamt" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="116px"></ItemStyle>
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
							<asp:ImageButton ID="btnDel" runat="server" OnClick="OnConfirm2" OnClientClick="Confirm2()" ImageUrl="~/images/delete_16.png" />

						</td>

					</tr>

					<tr style="height: 26px; background-color: lightgray">

						<td style="border: 1px solid #000000; width: auto; background-color: lightgray; text-align: right; font-family: 'Segoe UI'; font-size: small;" colspan="6">Total &nbsp;&nbsp;
						</td>

						<td style="border: 1px solid #000000; width: 120px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:Label ID="lblTotQty" runat="server" Text="0" ForeColor="Red" CssClass="lblLabelAmt"></asp:Label>
						</td>

						<td style="border: 1px solid #000000; width: 120px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:Label ID="lblTotWt" runat="server" Text="0.00" ForeColor="Red" CssClass="lblLabelAmt"></asp:Label>
						</td>

						<td style="border: 1px solid #000000; width: 120px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>

						<td style="border: 1px solid #000000; width: 120px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:Label ID="lblTotAmt" runat="server" CssClass="lblLabelAmt" ForeColor="Red" Text="0.00"></asp:Label>
						</td>

						<td style="border: 1px solid #000000; width: 120px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:Label ID="lblTotDiscAmt" runat="server" Text="0.00" ForeColor="Red" CssClass="lblLabelAmt"></asp:Label>
						</td>

						<td style="border: 1px solid #000000; width: 120px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:Label ID="lblTotNetAmt" runat="server" Text="0.00" ForeColor="Red" CssClass="lblLabelAmt"></asp:Label>
						</td>

						<td style="border: 1px solid #000000; width: 25px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>

					</tr>

					<tr style="height: auto; background-color: lightgray">

						<td style="border: 1px solid #000000; height: 20px; background-color: lightgray; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="13">&nbsp;&nbsp; Doc Status: &nbsp;&nbsp;
				<asp:Label ID="tssDocStat" runat="server" Text="New" ForeColor="Red"></asp:Label>

							&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Edit Doc No.: &nbsp;&nbsp;
				<asp:Label ID="tssDocNo" runat="server" Text="00000000" ForeColor="Red"></asp:Label>

							<%--&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Bill Ref: &nbsp;&nbsp;--%>
			
				&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Line Item: &nbsp;&nbsp;
				<asp:Label ID="lblLineItm" runat="server" Text="New" ForeColor="Red"></asp:Label>

						</td>
					</tr>


				</table>

			</ContentTemplate>

		</ajaxToolkit:TabPanel>

		<%--SO List--%>
		<ajaxToolkit:TabPanel ID="TabPanel2" runat="server" HeaderText="SO Summary">
			<ContentTemplate>


			</ContentTemplate>

		</ajaxToolkit:TabPanel>

		<%--SO Monitoring--%>
		<ajaxToolkit:TabPanel ID="TabPanel3" runat="server" HeaderText="SO Monitoring">
			<ContentTemplate>


			</ContentTemplate>

		</ajaxToolkit:TabPanel>

	</ajaxToolkit:TabContainer>




</asp:Content>
