<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/FI.master" CodeBehind="FSChemag.aspx.vb" Inherits="AOS100web.FSChemag" %>

<%@ Register Assembly="EditableDropDownList" Namespace="EditableControls" TagPrefix="editable" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentOfFinAcctg" runat="server">
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

		function Confirm2() {
			var confirm_value = document.createElement("INPUT");
			confirm_value.type = "hidden";
			confirm_value.name = "confirm_value";
			if (confirm_value("Are you sure to VOID DO?")) {
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
			if (confirm_value("Are you sure to REMOVE selected Line Item?")) {
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
			if (confirm_value("DO Save, do you want to Print Now?")) {
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
			<td style="background-color: #b1bbd7; border-top: solid; border-bottom: solid; border-color: red; border-width: 2px; width: 100%; font-family: 'Segoe UI'; font-size: 10px; text-align: center; min-height: 28px; max-height: 28px; padding-bottom: 1px;" colspan="6">
				<asp:LinkButton ID="lbNew" runat="server" Style="text-decoration: none; background-color: #7fc6f6; font-family: Tahoma; font-size: 14px; color: white; margin-right: 5px; padding-left: 5px; padding-right: 5px; min-height: 22px;" BorderStyle="Solid" ToolTip="Reset Fields" CssClass="StdBottomL">
					<asp:Image runat="server" imageurl="~/images/new_16.png" style="vertical-align: middle"/>&nbsp;New
				</asp:LinkButton>
				<asp:LinkButton ID="lbSave" runat="server" Style="text-decoration: none; background-color: #7fc6f6; padding-right: 5px; font-family: Tahoma; font-size: 14px; color: white; margin-right: 5px; padding-left: 5px; min-height: 22px;" BorderStyle="Solid" ToolTip="Save" Enabled="false">
					<asp:Image runat="server" imageurl="~/images/save_16.png" style="vertical-align: middle"/>&nbsp;Save
				</asp:LinkButton>
				<asp:LinkButton ID="lbPrint" runat="server" Style="text-decoration: none; background-color: #7fc6f6; padding-right: 5px; font-family: Tahoma; font-size: 14px; color: white; margin-right: 5px; padding-left: 5px; min-height: 22px;"
					BorderStyle="Solid" ToolTip="Print" Enabled="false">
					<asp:Image runat="server" imageurl="~/images/Print_16.png" style="vertical-align: middle"/>&nbsp;Print
				</asp:LinkButton>
				<asp:LinkButton ID="lbDelete" runat="server" Style="text-decoration: none; background-color: #7fc6f6; padding-right: 5px; font-family: Tahoma; font-size: 14px; color: white; margin-right: 5px; padding-left: 5px; min-height: 22px;" BorderStyle="Solid" ToolTip="Void" Enabled="false">
					<asp:Image runat="server" imageurl="~/images/delete_16.png" style="vertical-align: middle"/>&nbsp;Void
				</asp:LinkButton>
				<asp:LinkButton ID="lbClose" runat="server" OnClick="lbClose_Click" Style="text-decoration: none; background-color: #7fc6f6; font-family: Tahoma; font-size: 14px; color: white; min-height: 22px; padding-left: 5px;"
					BorderStyle="Solid" ToolTip="Close" CssClass="StdBottomR">
					<asp:Image runat="server" imageurl="~/images/Exit_16.png" style="vertical-align: middle"/>&nbsp;Close&nbsp;
				</asp:LinkButton>

				<asp:Label ID="lblUser" runat="server" Text="User" Visible="false"></asp:Label>
				<asp:Label ID="lblGrpUser" runat="server" Text="Grp User" Visible="false"></asp:Label>

			</td>
		</tr>
		<tr>
			<td style="border: 1px solid #000000; width: auto; background-color: lightgray;" colspan="6"></td>
		</tr>

		<tr>
			<td style="border: 1px solid #000000; width: 500px; height: 30px; background-color: lightgray;" colspan="3">&nbsp;&nbsp;
				<asp:Label ID="lblTitle" runat="server" Text="Reports" Font-Size="Larger" Font-Italic="true" ForeColor="Red"></asp:Label>
			</td>
			<td style="border: 1px solid #000000; width: 600px; height: 30px; background-color: lightgray;" colspan="3">&nbsp;&nbsp;
				<asp:Label ID="lblMsg" runat="server" Text="Message Box" Font-Size="Medium" Font-Italic="true" ForeColor="Red"></asp:Label>
			</td>
		</tr>

	</table>

	<ajaxToolkit:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" Style="width: 100%; height: auto;" AutoPostBack="true">
		<ajaxToolkit:TabPanel ID="TabPanel1" runat="server" HeaderText="Profit and Loss">
			<ContentTemplate>
				<asp:Panel ID="Panel1" runat="server" Width="100%" Height="710px" ScrollBars="Auto">
					<table style="width: 100%; font-family: 'Segoe UI'; font-size: small; float: left; background-color: whitesmoke; border-spacing: 0px; margin-left: 0px; padding-left: 0px; padding-right: 0px;">
						<tr style="height: 20px;">
							<td style="width: auto; text-align: Left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="18">
								&nbsp;&nbsp; For the Month of:&nbsp;&nbsp;
								<asp:DropDownList ID="cboFSmonYear" runat="server" Width="150px" AutoPostBack="true"></asp:DropDownList>
								&nbsp;&nbsp;&nbsp;&nbsp;
								<asp:Label ID="lblFSdate1" runat="server" Text="yyyy-MM-dd" Visible="false"></asp:Label>
								&nbsp;&nbsp;&nbsp;&nbsp;
								<asp:Label ID="lblFSdate2" runat="server" Text="yyyy-MM-dd" Visible="false"></asp:Label>
								&nbsp;&nbsp;&nbsp;&nbsp;
								<asp:Label ID="lblFirtDayYr" runat="server" Text="yyyy-MM-dd" Visible="false"></asp:Label>

							</td>
						</tr>

						<%-- L1--%>
						<tr style="height: 13px;">
							<td style="width: auto; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="18"></td>
						</tr>

						<%-- L2--%>
						<tr style="height: 26px;">
							<td style="width: auto; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: medium;" colspan="18">
								<asp:Label ID="txtCoName" runat="server" Text="Company Name"></asp:Label>
							</td>
						</tr>

						<%-- L3--%>
						<tr style="height: 20px;">
							<td style="width: auto; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="18">
								<asp:Label ID="lblnewTitle" runat="server" Text="Report Title"></asp:Label>

							</td>
						</tr>

						<%-- L4--%>
						<tr style="height: 20px;">
							<td style="width: auto; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="18">
								<asp:Label ID="lblTransDate" runat="server" Text="Date"></asp:Label>
							</td>
						</tr>

						<%-- L5--%>
						<tr style="height: 20px;">
							<td style="width: auto; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="18"></td>
						</tr>

						<%-- L6--%>
						<tr style="height: 24px; vertical-align:middle;">
							<td style="width: auto; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="3"></td>
							<td style="width: auto; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="4">
								<asp:Label ID="Label3" runat="server" Text="T O T A L" ForeColor="White" Font-Bold="true"></asp:Label>
							</td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: auto; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="4">
								<asp:Label ID="Label4" runat="server" Text="F O O D S" ForeColor="White" Font-Bold="true"></asp:Label>
							</td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: auto; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="4">
								<asp:Label ID="Label5" runat="server" Text="P E T" ForeColor="White" Font-Bold="true"></asp:Label>&nbsp;&nbsp;&nbsp;
								<asp:Label ID="Label18" runat="server" Text="P R O D U C T S" ForeColor="White" Font-Bold="true"></asp:Label>
							</td>

						</tr>
						<%-- L7--%>
						<tr style="height: 24px;">
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 250px; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 190px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="2">
							This Month
							</td>
							<td style="width: 190px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="2"> 
							Year To Date</td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 190px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="2">
							This Month</td>
							<td style="width: 190px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="2"> 
							Year To Date</td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 190px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="2">
							This Month
							</td>
							<td style="width: 190px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="2">
								Year To Date
							</td>
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
						</tr>
						<%-- L8--%>
						<tr style="height: 20px;">
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 250px; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 110px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								Amount
							</td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								%age
							</td>
							<td style="width: 110px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								Amount
							</td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								%age
							</td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 110px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								Amount
							</td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								%age
							</td>
							<td style="width: 110px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								Amount
							</td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								%age
							</td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 110px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								Amount
							</td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								%age
							</td>
							<td style="width: 110px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								Amount
							</td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								%age
							</td>
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
						</tr>
						 <%-- L9--%>
						<tr style="height: 20px;">
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 300px; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="2">
								Sales
							</td>
							<td style="width: 110px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								<asp:Label ID="txtSalesTot" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								<asp:Label ID="txtSalesGPtot" runat="server" Text="100.00%"></asp:Label>
							</td>
							<td style="width: 110px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtSalesYTDtot" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								<asp:Label ID="txtSalesGPytdTot" runat="server" Text="100.00%"></asp:Label>
							</td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">

							</td>
							<td style="width: 110px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtSales" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								<asp:Label ID="txtSalesGP" runat="server" Text="0.00%"></asp:Label>
							</td>
							<td style="width: 110px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtSalesYTD" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								<asp:Label ID="txtSalesGPytd" runat="server" Text="0.00%"></asp:Label>
							</td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">

							</td>
							<td style="width: 110px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtSales2" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								<asp:Label ID="txtSalesGP2" runat="server" Text="0.00%"></asp:Label>
							</td>
							<td style="width: 110px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtSalesYTD2" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtSalesGPytd2" runat="server" Text="0.00%"></asp:Label>
							</td>
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
						</tr>
						<%-- L10--%>
						<tr style="height: 20px;">
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 50px; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								Less:
							</td>
							<td style="width: 250px; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								Sales Discount
							</td>
							<td style="width: 110px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtDiscTot" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtDiscGPtot" runat="server" Text="0.00%"></asp:Label>
							</td>
							<td style="width: 110px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtDiscYTDtot" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtDiscGPytdTot" runat="server" Text="0.00%"></asp:Label>
							</td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 110px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtDisc" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtDiscGP" runat="server" Text="0.00%"></asp:Label>
							</td>
							<td style="width: 110px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtDiscYTD" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								<asp:Label ID="txtDiscGPytd" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 110px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtDisc2" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtDiscGP2" runat="server" Text="0.00%"></asp:Label>
							</td>
							<td style="width: 110px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtDiscYTD2" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtDiscGPytd2" runat="server" Text="0.00%"></asp:Label>
							</td>
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
						</tr>
						
						<%-- L11--%>
						<tr style="height: 20px;">
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 50px; text-align: Left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 250px; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								Sales Return and Allowances
							</td>
							<td style="width: 110px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtSRAtot" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtSRAtotGP" runat="server" Text="0.00%"></asp:Label>
							</td>
							<td style="width: 110px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtSRAtotYTD" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtSRAtotYTDgp" runat="server" Text="0.00%"></asp:Label>
							</td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 110px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtSRA" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtSRAgp" runat="server" Text="0.00%"></asp:Label>
							</td>
							<td style="width: 110px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtSRAytd" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								<asp:Label ID="txtSRAytdGP" runat="server" Text="0.00%"></asp:Label>
							</td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 110px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtSRA2" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtSRA2gp" runat="server" Text="0.00%"></asp:Label>
							</td>
							<td style="width: 110px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtSRAytd2" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtSRAytd2GP" runat="server" Text="0.00%"></asp:Label>
							</td>
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
						</tr>

						<%-- L12--%>
						<tr style="height: 20px;">
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 250px; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								Rebate
							</td>
							<td style="width: 110px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; border-bottom: 1px solid #000000;">
								 <asp:Label ID="txtRebTot" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; ">
								 <asp:Label ID="txtRebTotGP" runat="server" Text="0.00%"></asp:Label>
							</td>
							<td style="width: 110px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; border-bottom: 1px solid #000000;">
								<asp:Label ID="txtRebTotYTD" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; ">
								 <asp:Label ID="txtRebTotYTDgp" runat="server" Text="0.00%"></asp:Label>
							</td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 110px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; border-bottom: 1px solid #000000;">
								 <asp:Label ID="txtReb" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; ">
								 <asp:Label ID="txtRebgp" runat="server" Text="0.00%"></asp:Label>
							</td>
							<td style="width: 110px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; border-bottom: 1px solid #000000;">
								 <asp:Label ID="txtRebYTD" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; ">
								 <asp:Label ID="txtRebYTDgp" runat="server" Text="0.00%"></asp:Label>
							</td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 110px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; border-bottom: 1px solid #000000;">
								 <asp:Label ID="txtReb2" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtReb2GP" runat="server" Text="0.00%"></asp:Label>
							</td>
							<td style="width: 110px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; border-bottom: 1px solid #000000;">
								 <asp:Label ID="txtRebYTD2" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtRebYTD2GP" runat="server" Text="0.00%"></asp:Label>
							</td>
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
						</tr>

						<%-- L13--%>
						<tr style="height: 26px;">
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 300px; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="2">
								Net Sales
							</td>
							<td style="width: 110px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtNetSalesTot" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtNetSalesGPtot" runat="server" Text="0.00%"></asp:Label>
							</td>
							<td style="width: 110px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtNetSalesYTDtot" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtNetSalesGPytdTot" runat="server" Text="0.00%"></asp:Label>
							</td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 110px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtNetSales" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtNetSalesGP" runat="server" Text="0.00%"></asp:Label>
							</td>
							<td style="width: 110px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtNetSalesYTD" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtNetSalesGPytd" runat="server" Text="0.00%"></asp:Label>
							</td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 110px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtNetSales2" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtNetSalesGP2" runat="server" Text="0.00%"></asp:Label>
							</td>
							<td style="width: 110px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtNetSalesYTD2" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtNetSalesGPytd2" runat="server" Text="0.00%"></asp:Label>
							</td>
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
						</tr>

						 <%-- L14--%>
						<tr style="height: 26px;">
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 50px; text-align:left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								Less:
							</td>
							<td style="width: 250px; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								Cost of Sales (Schedule 5)
							</td>
							<td style="width: 110px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; border-bottom: 1px solid #000000;">
								 <asp:Label ID="txtCOStot" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtCOSgpTot" runat="server" Text="0.00%"></asp:Label>
							</td>
							<td style="width: 110px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; border-bottom: 1px solid #000000;">
								 <asp:Label ID="txtCOSYTDtot" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtCOSgpYTDtot" runat="server" Text="0.00%"></asp:Label>
							</td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 110px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; border-bottom: 1px solid #000000;">
								 <asp:Label ID="txtCOS" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtCOSgp" runat="server" Text="0.00%"></asp:Label>
							</td>
							<td style="width: 110px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; border-bottom: 1px solid #000000;">
								 <asp:Label ID="txtCOSYTD" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtCOSgpYTD" runat="server" Text="0.00%"></asp:Label>
							</td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 110px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; border-bottom: 1px solid #000000;">
								 <asp:Label ID="txtCOS2" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtCOSgp2" runat="server" Text="0.00%"></asp:Label>
							</td>
							<td style="width: 110px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; border-bottom: 1px solid #000000;">
								 <asp:Label ID="txtCOSYTD2" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtCOSgpYTD2" runat="server" Text="0.00%"></asp:Label>
							</td>
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
						</tr>

						 <%-- L15--%>
						<tr style="height: 20px;">
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 300px; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="2">
								Gross Profit
							</td>
							<td style="width: 110px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtGProfitTot" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtGrossProGPtot" runat="server" Text="0.00%"></asp:Label>
							</td>
							<td style="width: 110px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtGProfitYTDtot" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtGrossProGPytdTot" runat="server" Text="0.00%"></asp:Label>
							</td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 110px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtGProfit" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtGrossProGP" runat="server" Text="0.00%"></asp:Label>
							</td>
							<td style="width: 110px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtGProfitYTD" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtGrossProGPytd" runat="server" Text="0.00%"></asp:Label>
							</td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 110px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtGProfit2" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtGrossProGP2" runat="server" Text="0.00%"></asp:Label>
							</td>
							<td style="width: 110px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtGProfitYTD2" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtGrossProGPytd2" runat="server" Text="0.00%"></asp:Label>
							</td>
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
						</tr>

						 <%-- L16--%>
						<tr style="height: 26px;">
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 50px; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								Less:
							</td>
							<td style="width: 250px; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								Operating Expenses
							</td>
							<td style="width: 110px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 110px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 110px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 110px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 110px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 110px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
						</tr>

						 <%-- L17--%>
						<tr style="height: 20px;">
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 250px; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								Admin Expenses (Schedule 1)
							</td>
							<td style="width: 110px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtAdmExpsTot" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtAdmGPtot" runat="server" Text="0.00%"></asp:Label>
							</td>
							<td style="width: 110px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtAdmExpsYTDtot" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtAdmGPytdTot" runat="server" Text="0.00%"></asp:Label>
							</td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 110px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 110px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 110px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 110px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
						</tr>

						 <%-- L18--%>
						<tr style="height: 20px;">
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 250px; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								Selling Expenses (Schedule 2)
							</td>
							<td style="width: 110px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtSelExpTot" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtSelGPtot" runat="server" Text="0.00%"></asp:Label>
							</td>
							<td style="width: 110px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtSelExpYTDtot" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtSelGPytdTot" runat="server" Text="0.00%"></asp:Label>
							</td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 110px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 110px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 110px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 110px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
						</tr>

						 <%-- L19--%>
						<tr style="height: 20px;">
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 250px; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								Technical Expenses (Schedule 3)
							</td>
							<td style="width: 110px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtTechTot" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtTechGPtot" runat="server" Text="0.00%"></asp:Label>
							</td>
							<td style="width: 110px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtTechYTDtot" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtTechGPytdTot" runat="server" Text="0.00%"></asp:Label>
							</td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 110px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 110px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 110px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 110px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
						</tr>

						 <%-- L20--%>
						<tr style="height: 20px;">
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 250px; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								Logistic Expenses (Schedule 4)
							</td>
							<td style="width: 110px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; border-bottom: 1px solid #000000;">
								 <asp:Label ID="txtLogisTot" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtLogisGPtot" runat="server" Text="0.00%"></asp:Label>
							</td>
							<td style="width: 110px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; border-bottom: 1px solid #000000;">
								 <asp:Label ID="txtLogisYTDtot" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtLogisGPytdTot" runat="server" Text="0.00%"></asp:Label>
							</td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 110px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 110px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 110px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 110px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
						</tr>

						<%-- L21--%>
						<tr style="height: 26px;">
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 250px; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								Total Operating Expenses
							</td>
							<td style="width: 110px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtOpexTot" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtOpExpGLtot" runat="server" Text="0.00%"></asp:Label>
							</td>
							<td style="width: 110px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtOpexYTDtot" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtOpExpGLytdTot" runat="server" Text="0.00%"></asp:Label>
							</td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 110px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 110px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 110px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 110px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
						</tr>

						<%-- L22--%>
						<tr style="height: 20px;">
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 50px; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								Add:
							</td>
							<td style="width: 250px; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								Miscelaneous Income
							</td>
							<td style="width: 110px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; border-bottom: 1px solid #000000;">
								 <asp:Label ID="txtMIothersTot" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtMIgpTot" runat="server" Text="0.00%"></asp:Label>
							</td>
							<td style="width: 110px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; border-bottom: 1px solid #000000;">
								 <asp:Label ID="txtMIothersYTDtot" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtMIgpYTDtot" runat="server" Text="0.00%"></asp:Label>
							</td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 110px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 110px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 110px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 110px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
						</tr>

						<%-- L23--%>
						<tr style="height: 26px;">
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 300px; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="2">
								Net Income (Loss) Before Tax 
							</td>
							<td style="width: 110px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtNetIncTot" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtNIncGPtot" runat="server" Text="0.00%"></asp:Label>
							</td>
							<td style="width: 110px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtNetIncYTDtot" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtNIncGPytdTot" runat="server" Text="0.00%"></asp:Label>
							</td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 110px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 110px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 110px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 110px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
						</tr>

						<%-- L24--%>
						<tr style="height: 20px;">
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 300px; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="2">
								Income Tax Expense
							</td>
							<td style="width: 110px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtIncTaxExp" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtTaxGP" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 110px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtIncTaxExpYTD" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtTaxGPytd" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 110px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 110px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 110px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 110px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
						</tr>

						<%-- L25--%>
						<tr style="height: 20px;">
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 50px; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								Add/Deduct:
							</td>
							<td style="width: 250px; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								Prior Years Adjustment
							</td>
							<td style="width: 110px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; border-bottom: 1px solid #000000;">
								 <asp:Label ID="txtAdjt" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtAdjtGP" runat="server" Text="0.00%"></asp:Label>
							</td>
							<td style="width: 110px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;border-bottom: 1px solid #000000;">
								 <asp:Label ID="txtAdjtYTD" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtAdjtGPytd" runat="server" Text="0.00%"></asp:Label>
							</td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 110px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 110px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 110px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 110px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
						</tr>

						<%-- L26--%>
						<tr style="height: 26px;">
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 300px; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="2">
								Net Income (Loss) After Tax 
							</td>
							<td style="width: 110px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtNetIncFinal" runat="server" Text="0.00" Font-Bold="true"></asp:Label>
							</td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtFinalIncGP" runat="server" Text="0.00%" Font-Bold="true"></asp:Label>
							</td>
							<td style="width: 110px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtNetIncFinalYTD" runat="server" Text="0.00" Font-Bold="true"></asp:Label>
							</td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtFinalIncGPytd" runat="server" Text="0.00%" Font-Bold="true"></asp:Label>
							</td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 110px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 110px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 110px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 110px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
						</tr>

					   <tr style="height: 120px;">
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 250px; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" ></td>
							<td style="width: 110px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; border-top-style:double;"></td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 110px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; border-top-style:double;"></td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 110px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 110px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 110px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 110px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 80px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
						</tr>

						

					</table>

				</asp:Panel>

			</ContentTemplate>

		</ajaxToolkit:TabPanel>

		<%--SO List--%>
		<ajaxToolkit:TabPanel ID="TabPanel2" runat="server" HeaderText="Cost Of Sales">
			<ContentTemplate>
				<asp:Panel ID="Panel2" runat="server" Width="100%" Height="710px" ScrollBars="Auto">
					<table style="width: 100%; font-family: 'Segoe UI'; font-size: small; float: left; background-color: whitesmoke; border-spacing: 0px; margin-left: 0px; padding-left: 0px; padding-right: 0px;">
						<tr style="height: 20px;">
							<td style="width: auto; text-align: Left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="16">
								&nbsp;&nbsp; For the Month of:&nbsp;&nbsp;
								<asp:DropDownList ID="cboFSmonYear2" runat="server" Width="150px" AutoPostBack="true"></asp:DropDownList>
								&nbsp;&nbsp;&nbsp;&nbsp;
								<asp:Label ID="lblFSdateT21" runat="server" Text="yyyy-MM-dd" Visible="false"></asp:Label>
								&nbsp;&nbsp;&nbsp;&nbsp;
								<asp:Label ID="lblFSdateT22" runat="server" Text="yyyy-MM-dd" Visible="false"></asp:Label>
								&nbsp;&nbsp;&nbsp;&nbsp;
								<asp:Label ID="lblFirtDayYr2" runat="server" Text="yyyy-MM-dd" Visible="false"></asp:Label>

							</td>

							<%-- L1--%>
						<tr style="height: 13px;">
							<td style="width: auto; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="16"></td>
						</tr>

						<%-- L2--%>
						<tr style="height: 26px;">
							<td style="width: auto; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: medium;" colspan="16">
								<asp:Label ID="txtCoName2" runat="server" Text="Company Name"></asp:Label>
							</td>
						</tr>

						<%-- L3--%>
						<tr style="height: 20px;">
							<td style="width: auto; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="16">
								<asp:Label ID="lblnewTitle2" runat="server" Text="Report Title"></asp:Label>
							</td>
						</tr>

						<%-- L4--%>
					   <tr style="height: 20px;">
							<td style="width: auto; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="16">
								<asp:Label ID="lblTransDate2" runat="server" Text="Date"></asp:Label>
							</td>
						</tr>

						<%-- L5--%>
						<tr style="height: 20px;">
							<td style="width: auto; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="16"></td>
						</tr>

						<%-- L6--%>
						<tr style="height: 24px; vertical-align:middle;">
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: auto; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="3"></td>
							<td style="width: auto; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="3">
								<asp:Label ID="Label6" runat="server" Text="F O O D S" ForeColor="White" Font-Bold="true"></asp:Label>
								
							</td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: auto; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="3">
								<asp:Label ID="Label7" runat="server" Text="P E T" ForeColor="White" Font-Bold="true"></asp:Label>&nbsp;&nbsp;&nbsp;
								<asp:Label ID="Label8" runat="server" Text="P R O D U C T S" ForeColor="White" Font-Bold="true"></asp:Label>
								
							</td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: auto; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="3">
								<asp:Label ID="Label2" runat="server" Text="T O T A L" ForeColor="White" Font-Bold="true"></asp:Label>
							</td>

						</tr>
						<%-- L7--%>
						<tr style="height: 26px;">
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 350px; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="3">
								<asp:Label ID="Label17" runat="server" Text="Raw Materials" Font-Bold="true"></asp:Label>
							</td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 10px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 10px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 10px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
						</tr>
						<%-- L8--%>
						<tr style="height: 20px;">
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 300px; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="2">
							   Raw Materials Received/Purchases
							</td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtFGusedAsRM" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 10px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 10px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtFGusedAsRMtot" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 10px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
						<%-- L09--%>
						<tr style="height: 20px;">
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 50px; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								Add:
							</td>
							<td style="width: 250px; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" >
								Beginning Inventory
							</td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								<asp:Label ID="txtRMbeg" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 10px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 10px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								<asp:Label ID="txtRMbegTot" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 10px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
						</tr>
						
						 <%-- L10--%>
						<tr style="height: 20px;">
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 50px; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								Less:
							</td>
							<td style="width: 250px; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" >
								Ending Inventory
							</td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								<asp:Label ID="txtRMend" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 10px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 10px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								<asp:Label ID="txtRMendTot" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 10px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
						</tr>
						 <%-- L11--%>
						<tr style="height: 20px;">
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 50px; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								Less:
							</td>
							<td style="width: 250px; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" >
								RM Issued Other Than Production
							</td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; border-bottom: 1px solid #000000;">
								 <asp:Label ID="txtRMforSales" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 10px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 10px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; border-bottom: 1px solid #000000;">
								<asp:Label ID="txtRMforSalesTot" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 10px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
						</tr>
						 <%-- L12--%>
						<tr style="height: 20px;">
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 350px; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="3">
								<asp:Label ID="Label20" runat="server" Text="Total RM Used" Font-Bold="true"></asp:Label>
							</td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtRMused" runat="server" Text="0.00" Font-Bold="true"></asp:Label>
							</td>
							<td style="width: 10px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">

							</td>
							<td style="width: 10px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								 <asp:Label ID="txtRMusedTot" runat="server" Text="0.00" Font-Bold="true"></asp:Label>
							</td>
							<td style="width: 10px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
						</tr>
						 <%-- L13--%>
						<tr style="height: 13px;">
							<td style="width: auto; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="16"></td>
						</tr>
						 <%-- L14--%>
						<tr style="height: 20px;">
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								Add:
							</td>
							<td style="width: 300px; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="2">
								Direct Labor and Ovehead
							</td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 10px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 10px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 10px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
						</tr>
						<%-- L15--%>
						<tr style="height: 20px;">
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 300px; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="2">
								Direct Labor
							</td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								<asp:Label ID="txtDL" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 10px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 10px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								<asp:Label ID="txtDLtot" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 10px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
						</tr>
							<%-- L16--%>
						<tr style="height: 20px;">
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 300px; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="2">
								Overhead
							</td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; border-bottom: 1px solid #000000;">
								<asp:Label ID="txtOH" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 10px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 10px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; border-bottom: 1px solid #000000;">
								<asp:Label ID="txtOHtot" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 10px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
						</tr>
							<%-- L17--%>
						<tr style="height: 26px;">
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 350px; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="3">
								<asp:Label ID="Label10" runat="server" Text="Total Cost of Goods Manufactured" Font-Bold="true"></asp:Label>
							</td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								<asp:Label ID="txtTotManuf" runat="server" Text="0.00" Font-Bold="true"></asp:Label>
							</td>
							<td style="width: 10px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 10px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								<asp:Label ID="txtTotManufTot" runat="server" Text="0.00" Font-Bold="true"></asp:Label>
							</td>
							<td style="width: 10px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
						</tr>
							 <%-- L18--%>
						<tr style="height: 13px;">
							<td style="width: auto; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="16"></td>
						</tr>
							 <%-- L19--%>
						<tr style="height: 20px;">
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 350px; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="3">
								<asp:Label ID="Label32" runat="server" Text="Finished Goods" Font-Bold="true"></asp:Label>
							</td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 10px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 10px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 10px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
						</tr>
							  <%-- L20--%>
						<tr style="height: 20px;">
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 300px; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="2">
								Finished Goods Received/Purchases
							</td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								<asp:Label ID="txtFGnchange" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 10px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								<asp:Label ID="txtFGnchange2" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 10px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								<asp:Label ID="txtFGnchangeTot" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 10px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
						</tr>
							 <%-- L21--%> 
						<tr style="height: 20px;">
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 300px; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="2">
								Finished Goods Inventory
							</td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 10px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 10px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 10px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
						</tr>
							 <%-- L22--%>    
						<tr style="height: 20px;">
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 50px; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								Add:
							</td>
							<td style="width: 250px; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								Beginning Inventory
							</td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								<asp:Label ID="txtFGbeg" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 10px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								<asp:Label ID="txtFGbeg2" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 10px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								<asp:Label ID="txtFGbegtot" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 10px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
						</tr>
						 <%-- L23--%>    
						<tr style="height: 20px;">
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 50px; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								Add/Less:
							</td>
							<td style="width: 250px; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							   Inventory Adjustment
							</td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								<asp:Label ID="txtFGadjt1" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 10px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								<asp:Label ID="txtFGadjt2" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 10px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								<asp:Label ID="txtFGadjtTot" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 10px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
						</tr>
							<%-- L24--%>    
						<tr style="height: 20px;">
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 50px; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								Less:
							</td>
							<td style="width: 250px; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							   Ending Inventory
							</td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; border-bottom: 1px solid #000000;">
								<asp:Label ID="txtFGend" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 10px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; border-bottom: 1px solid #000000;">
								<asp:Label ID="txtFGend2" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 10px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; border-bottom: 1px solid #000000;">
								<asp:Label ID="txtFGendTot" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 10px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
						</tr>
						  <%-- L25--%>
						<tr style="height: 20px;">
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 300px; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="2">
								Finished Goods Inventory Net Change
							</td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								<asp:Label ID="txtFGnetChange" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 10px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								<asp:Label ID="txtFGnetChange2" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 10px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; ">
								<asp:Label ID="txtFGnetChangeTot" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 10px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
						</tr>

							   <%-- L26--%>    
						<tr style="height: 20px;">
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								Add:
							</td>
							<td style="width: 300px; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="2">
							   Cost of Raw Materials Sold
							</td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; border-bottom: 1px solid #000000;">
								<asp:Label ID="txtRMcosFinal" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 10px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; border-bottom: 1px solid #000000;">
								<asp:Label ID="Label49" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 10px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; "></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; border-bottom: 1px solid #000000;">
								<asp:Label ID="txtRMcosFinalTot" runat="server" Text="0.00"></asp:Label>
							</td>
							<td style="width: 10px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
						</tr>
							<%-- L27--%>
						<tr style="height: 26px;">
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 350px; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="3">
								<asp:Label ID="Label45" runat="server" Text="Cost of Goods Sold" Font-Bold="true"></asp:Label>
							</td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								<asp:Label ID="txtNewCOS1" runat="server" Text="0.00" Font-Bold="true"></asp:Label>
							</td>
							<td style="width: 10px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								<asp:Label ID="txtNewCOS2" runat="server" Text="0.00" Font-Bold="true"></asp:Label>
							</td>
							<td style="width: 10px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								<asp:Label ID="txtNewCOStot" runat="server" Text="0.00" Font-Bold="true"></asp:Label>
							</td>
							<td style="width: 10px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
						</tr>
							<%-- template--%>
						<tr style="height: 126px;">
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 250px; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; border-top-style:double;"></td>
							<td style="width: 10px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; border-top-style:double;"></td>
							<td style="width: 10px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 2px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; border-top-style:double;"></td>
							<td style="width: 10px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="width: 50px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
						</tr>




					</table>
				</asp:Panel>

			</ContentTemplate>

		</ajaxToolkit:TabPanel>

		<%--Schedules--%>
		<ajaxToolkit:TabPanel ID="TabPanel3" runat="server" HeaderText="Schedules">
			<ContentTemplate>
				<table style="width: 100%; font-family: 'Segoe UI'; font-size: small; float: left; background-color: whitesmoke; border-spacing: 0px; margin-left: 0px; padding-left: 0px; padding-right: 0px;">
					<tr style="height: 26px;">
						<td style="width: auto; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							For the Month of:&nbsp;&nbsp;
						</td>
						<td style="width: 150px; text-align: Left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="9">
							<asp:DropDownList ID="cboFSmonYear3" runat="server" Width="150px" AutoPostBack="true"></asp:DropDownList>
							&nbsp;&nbsp;
								<asp:Label ID="lblFSdateT31" runat="server" Text="yyyy-MM-dd" Visible="false"></asp:Label>
							&nbsp;&nbsp;&nbsp;&nbsp;
								<asp:Label ID="lblFSdateT32" runat="server" Text="yyyy-MM-dd" Visible="false"></asp:Label>
							&nbsp;&nbsp;&nbsp;&nbsp;
								<asp:Label ID="lblFirtDayYr3" runat="server" Text="yyyy-MM-dd" Visible="false"></asp:Label>
						</td>
					</tr>

					<%-- L1--%>
					<tr style="height: 26px;">
						<td style="width: 150px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							 Select Report:&nbsp;&nbsp;
						</td>

						<td style="width: auto; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="9">
							<asp:DropDownList ID="cboFormat" runat="server" Width="200px" AutoPostBack="true"></asp:DropDownList>
						</td>
					</tr>

					<%-- L2--%>
					<tr style="height: 5px;">
						<td style="width: auto; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="10">
						   
						</td>
					</tr>

					<tr style="height: auto;">
						<td style="width: 150px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
						   
						</td>

						<td style="width: auto; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; vertical-align: top; " colspan="8">
							<asp:Panel ID="Panel3" runat="server" Width="100%" Height="615px">
								<asp:GridView ID="dgvSched" runat="server" AutoGenerateColumns="False" OnRowDataBound="dgvSched_RowDataBound" OnRowCreated="dgvSched_RowCreated" Font-Names="Segoe UI" Font-Size="small" BackColor="White" BorderColor="#999999" BorderStyle="Solid" AllowSorting="true" BorderWidth="1px" CellPadding="3" GridLines="Vertical" HeaderStyle-BorderColor="Black" ShowHeader="true" HeaderStyle-HorizontalAlign="Center" ShowFooter="true">
									<AlternatingRowStyle BackColor="#DCDCDC" />
									<Columns>
										<asp:TemplateField HeaderText="Expenses Description">
											<ItemTemplate>
												<asp:Label ID="Label1" runat="server" Text='<%# Bind("gldesc") %>'></asp:Label>
											</ItemTemplate>
											<ItemStyle HorizontalAlign="Left" Width="800px" />
											<FooterTemplate>
												<div style="text-align: center;">
													<asp:Label ID="lblGTotal" runat="server" Font-Bold="true" BackColor="#CCCCCC" ForeColor="White" Width="100%" Height="100%" />
												</div>
											</FooterTemplate>
											<FooterStyle BorderStyle="Solid" HorizontalAlign="Right" VerticalAlign="Middle" />
										</asp:TemplateField>

										<asp:TemplateField HeaderText="Month">
											<ItemTemplate>
												<asp:Label ID="txtDr" runat="server" Text='<%# Bind("dramt", "{0:#,##0.00;(#,##0.00)}") %>'></asp:Label>
											</ItemTemplate>
											<ItemStyle HorizontalAlign="Right" />
											<FooterTemplate>
												<div style="text-align: right;">
													<asp:Label ID="lblDr" runat="server" Font-Bold="true" BackColor="#CCCCCC" ForeColor="White" Width="120px" Height="100%" />
												</div>
											</FooterTemplate>
											<FooterStyle BorderStyle="Solid" HorizontalAlign="Right" VerticalAlign="Middle" />
										</asp:TemplateField>

										<asp:TemplateField HeaderText="YTD">
											<ItemTemplate>
												<asp:Label ID="txtCr" runat="server" Text='<%# Bind("cramt", "{0:#,##0.00;(#,##0.00)}") %>'></asp:Label>
											</ItemTemplate>
											<ItemStyle HorizontalAlign="Right" />
											<FooterTemplate>
												<div style="text-align: right;">
													<asp:Label ID="lblCr" runat="server" Font-Bold="true" BackColor="#CCCCCC" ForeColor="White" Width="120px" Height="100%" />
												</div>
											</FooterTemplate>
											<FooterStyle BorderStyle="Solid" HorizontalAlign="Right" VerticalAlign="Middle" />
										</asp:TemplateField>


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
									$('#<%=dgvSched.ClientID %>').Scrollable({
										ScrollHeight: 590,
										IsInPanel: true
									});
								});
							</script>
						</td>

						 <td style="width: 150px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
						   
						</td>

					</tr>
				

				</table>

			</ContentTemplate>

		</ajaxToolkit:TabPanel>

	</ajaxToolkit:TabContainer>


</asp:Content>
