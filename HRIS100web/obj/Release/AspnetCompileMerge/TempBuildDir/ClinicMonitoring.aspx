<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin.master" CodeBehind="ClinicMonitoring.aspx.vb" Inherits="AOS100web.ClinicMonitoring" %>

<%@ Register Assembly="EditableDropDownList" Namespace="EditableControls" TagPrefix="editable" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentOfAdministrator" runat="server">
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


	<%--<asp:Label ID="lblDim" runat="server" Text=""></asp:Label>--%>

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
				<asp:LinkButton ID="lbNew" runat="server" OnClick="lbNew_Click" Style="text-decoration: none; background-color: #7fc6f6; font-family: Tahoma; font-size: 14px; color: white; margin-right: 5px; padding-left: 5px; padding-right: 5px; min-height: 22px;" BorderStyle="Solid" ToolTip="Reset Fields" CssClass="StdBottomL">
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
				&nbsp;&nbsp;&nbsp;&nbsp;
				<asp:Label ID="lblUser" runat="server" Text="User" Visible="false"></asp:Label>
				<asp:Label ID="lblGrpUser" runat="server" Text="Grp User" Visible="false"></asp:Label>
			</td>
		</tr>
		<tr>
			<td style="border: 1px solid #000000; width: auto; background-color: lightgray;" colspan="6"></td>
		</tr>

		<tr>
			<td style="border: 1px solid #000000; width: 500px; height: 30px; background-color: lightgray;" colspan="3">&nbsp;&nbsp;
				<asp:Label ID="lblTitle" runat="server" Text="Tab" Font-Size="Larger" Font-Italic="true" ForeColor="Red"></asp:Label>
			</td>
			<td style="border: 1px solid #000000; width: 600px; height: 30px; background-color: lightgray;" colspan="3">&nbsp;&nbsp;
				<asp:Label ID="lblMsg" runat="server" Text="Message Box" Font-Size="Medium" Font-Italic="true" ForeColor="Red"></asp:Label>
			</td>
		</tr>

	</table>

	<ajaxToolkit:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" Style="width: 99%; height: auto;" AutoPostBack="true">
		<ajaxToolkit:TabPanel ID="TabPanel1" runat="server" HeaderText="Consultation">
			<ContentTemplate>



			</ContentTemplate>

		</ajaxToolkit:TabPanel>


		<ajaxToolkit:TabPanel ID="TabPanel2" runat="server" HeaderText="Reports">
			<ContentTemplate>
			</ContentTemplate>

		</ajaxToolkit:TabPanel>

		<ajaxToolkit:TabPanel ID="TabPanel3" runat="server" HeaderText="Inventory/Supply">
			<ContentTemplate>
			</ContentTemplate>

		</ajaxToolkit:TabPanel>


		<ajaxToolkit:TabPanel ID="TabPanel4" runat="server" HeaderText="Masterdata">
			<ContentTemplate>
				<table style="width: 100%; font-family: 'Segoe UI'; float: left; margin-top: -2px; padding-top: 0px">
					<tr>
						<td style="border: 1px solid #000000; width: 500px; height: 20px; background-color: ghostwhite;" colspan="3">&nbsp;&nbsp;
							<asp:Label ID="lblMstrTab" runat="server" Text="Tab" Font-Size="Medium" Font-Italic="true" ForeColor="Red">
							</asp:Label>
						</td>

					</tr>
				</table>

				<ajaxToolkit:TabContainer ID="TabContainer2" runat="server" ActiveTabIndex="0" Style="width: 99%; height: auto;" AutoPostBack="true">
					<ajaxToolkit:TabPanel ID="TabPanel5" runat="server" HeaderText="Patient">
						<ContentTemplate>
							<table style="width: 100%; font-family: 'Segoe UI'; font-size: small; float: left; background-color: lightgrey; border-spacing: 0px; margin-left: 0px; padding-left: 0px; padding-right: 0px;">
								<tr style="height: 26px; background-color: lightgray;">
									<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">

									</td>
									<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">

									</td>
									<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">

									</td>
									<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">

									</td>
									<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">

									</td>
									<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">

									</td>
									<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">

									</td>
									<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">

									</td>

								</tr>
								<tr style="height: 26px; background-color: lightgray;">
									
								</tr>
								

							</table>


						</ContentTemplate>

					</ajaxToolkit:TabPanel>

					<ajaxToolkit:TabPanel ID="TabPanel6" runat="server" HeaderText="Supplies">
						<ContentTemplate>


						</ContentTemplate>

					</ajaxToolkit:TabPanel>

				</ajaxToolkit:TabContainer>


			</ContentTemplate>

		</ajaxToolkit:TabPanel>

	</ajaxToolkit:TabContainer>

</asp:Content>
