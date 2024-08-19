<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/FI.master" CodeBehind="Liquidation.aspx.vb" Inherits="AOS100web.Liquidation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentOfFinAcctg" runat="server">

	<%@ Register Assembly="EditableDropDownList" Namespace="EditableControls" TagPrefix="editable" %>
	<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

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
				<asp:LinkButton ID="lbNew" runat="server" Style="text-decoration: none; background-color: #7fc6f6; font-family: Tahoma; font-size: 14px; color: white; margin-right: 5px; padding-left: 5px; padding-right: 5px; min-height: 22px;"
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
				<asp:LinkButton ID="lbDelete" runat="server" Style="text-decoration: none; background-color: #7fc6f6; padding-right: 5px; font-family: Tahoma; font-size: 14px; color: white; margin-right: 5px; padding-left: 5px; min-height: 22px;"
					BorderStyle="Solid" ToolTip="Void" Enabled="false">
					<asp:Image runat="server" imageurl="~/images/delete_16.png" style="vertical-align: middle"/>&nbsp;Void
				</asp:LinkButton>
				<asp:LinkButton ID="lbClose" runat="server" Style="text-decoration: none; background-color: #7fc6f6; font-family: Tahoma; font-size: 14px; color: white; min-height: 22px; padding-left: 5px;"
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

		<tr>
			<td style="width: auto; height: 35px; background-color: whitesmoke;" colspan="7">&nbsp; &nbsp;
				<asp:Label ID="lblFormName" runat="server" Text="Title" Font-Names="Segoe UI" ForeColor="Red" Font-Italic="true" Font-Size="Larger" Font-Bold="true"></asp:Label>
			</td>
		</tr>

	</table>

	<ajaxToolkit:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" Style="width: 100%; height: auto; font-family: 'Segoe UI'; font-size: medium;">
		<ajaxToolkit:TabPanel ID="TabPanel1" runat="server" HeaderText="List">
			<ContentTemplate>
				<table style="width: 100%; font-family: 'Segoe UI'; font-size: small; float: left; background-color: lightgrey; border-spacing: 0px; margin-left: 0px; padding-left: 0px; padding-right: 0px;">
					<tr style="height: 26px; background-color: lightgray">
						<td style="border: 1px solid #000000; width: 80px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Date From:&nbsp;&nbsp;
						</td>

						<td style="border: 1px solid #000000; width: 122px; background-color: lightgray; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:TextBox ID="dpTransDate1" runat="server" TextMode="Date" Width="120px" Height="99%" Font-Names="Segoe UI" Font-Size="small" AutoPostBack="true" CssClass="DateBox"></asp:TextBox>
						</td>

						<td style="border: 1px solid #000000; width: 80px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Date To:&nbsp;&nbsp;
						</td>

						<td style="border: 1px solid #000000; width: 122px; background-color: lightgray; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:TextBox ID="dpTransDate2" runat="server" TextMode="Date" Width="120px" Height="99%" Font-Names="Segoe UI" Font-Size="small" AutoPostBack="true" CssClass="DateBox"></asp:TextBox>
						</td>

						<td style="border: 1px solid #000000; width: 120px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Status:&nbsp;&nbsp;
						</td>

						<td style="border: 1px solid #000000; width: 150px; background-color: lightgray; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:DropDownList ID="cboStat" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" CssClass="cboBox3" Width="148px">
							</asp:DropDownList>

						</td>

						<td style="border: 1px solid #000000; width: auto; background-color: lightgray; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="7">
							<asp:LinkButton ID="lbStat" runat="server" Style="text-decoration: none; background-color: lightgray; float: left; padding-right: 2px;">&nbsp;&nbsp;
							<asp:Image runat="server" imageurl="~/images/check_16.png" style="vertical-align: middle"/>
							</asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						    
						</td>

					</tr>
					<tr>
						<td style="border: 1px solid #000000; height: 20px; width: auto; background-color: lightgray; text-align: center;" colspan="11">Summary Per Doc. List
						</td>
					</tr>

					<tr style="background-color: lightgray">
						<td style="border: 1px solid #000000; height: 373px; background-color: floralwhite; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; vertical-align: top; margin-left: 0px" colspan="11">
							<asp:Panel ID="Panel1" runat="server" Width="100%" Height="371px">
								<asp:GridView ID="dgvSum" runat="server" AutoGenerateColumns="False" OnRowDataBound="dgvSum_RowDataBound" OnRowCreated="dgvSum_RowCreated" Font-Names="Segoe UI" Font-Size="small" BackColor="White" BorderColor="#999999" BorderStyle="Solid" AllowSorting="true" BorderWidth="1px" CellPadding="3" GridLines="Vertical" HeaderStyle-BorderColor="Black" ShowHeader="true">
									<AlternatingRowStyle BackColor="#DCDCDC" />
									<Columns>
										<asp:CommandField HeaderText="Select" ShowSelectButton="true">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="72px"></ItemStyle>
										</asp:CommandField>

										<asp:BoundField DataField="docno" HeaderText="Doc. No.">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="110px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="tc" HeaderText="TC">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="60px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="transdate" HeaderText="Date" DataFormatString="{0:yyyy-MM-dd}">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="vendor" HeaderText="Vendor">
											<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="350px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="dramt" HeaderText="Amount" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="118px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="remarks" HeaderText="Remarks">
											<ItemStyle HorizontalAlign="left" VerticalAlign="Middle" Width="550px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="replno" HeaderText="Repl No.">
											<ItemStyle HorizontalAlign="left" VerticalAlign="Middle" Width="100px"></ItemStyle>
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
									$('#<%=dgvSum.ClientID %>').Scrollable({
										ScrollHeight: 348,
										IsInPanel: true
									});
								});
							</script>
						</td>

					</tr>
					<tr>
						<td style="width: auto; background-color: white;" colspan="11"></td>
					</tr>
					<tr>
						<td style="border: 1px solid #000000; height: 20px; width: auto; background-color: lightgray; text-align: center;" colspan="11">GL Entry
						</td>


					</tr>
					<tr style="background-color: lightgray">
						<td style="border: 1px solid #000000; height: 226px; background-color: floralwhite; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; vertical-align: top; margin-left: 0px" colspan="11">
							<asp:Panel ID="Panel2" runat="server" Width="100%" Height="224px">
								<asp:GridView ID="dgvGLentry" runat="server" AutoGenerateColumns="False" OnRowDataBound="dgvGLentry_RowDataBound" OnRowCreated="dgvGLentry_RowCreated" Font-Names="Segoe UI" Font-Size="small" BackColor="White" BorderColor="#999999" BorderStyle="Solid" AllowSorting="true" BorderWidth="1px" CellPadding="3" GridLines="Vertical" HeaderStyle-BorderColor="Black" ShowHeader="true">
									<AlternatingRowStyle BackColor="#DCDCDC" />
									<Columns>
										<asp:BoundField DataField="ccno" HeaderText="Cost Center">
											<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="350px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="acctno" HeaderText="GL Account">
											<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="450px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="dramt" HeaderText="DR Amount" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="118px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="cramt" HeaderText="CR Amount" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="118px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="pk" HeaderText="PK">
											<ItemStyle HorizontalAlign="center" VerticalAlign="Middle" Width="80px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="subacct" HeaderText="Sub Acct">
											<ItemStyle HorizontalAlign="center" VerticalAlign="Middle" Width="120px"></ItemStyle>
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
									$('#<%=dgvGLentry.ClientID %>').Scrollable({
										ScrollHeight: 220,
										IsInPanel: true
									});
								});
							</script>

						</td>

					</tr>

					<tr style="height: 24px; background-color: lightgray">
						<td style="border: 1px solid #000000; width: 800px; background-color: lightgray; text-align: center;" colspan="6">Total
						</td>

						<td style="border: 1px solid #000000; width: 120px; background-color: whitesmoke; text-align: center;">
							<asp:Label ID="lblDRamtTot" runat="server" Text="0.00" ForeColor="Black"></asp:Label>
						</td>

						<td style="border: 1px solid #000000; width: 120px; background-color: whitesmoke; text-align: center;">
							<asp:Label ID="lblCRamtTot" runat="server" Text="0.00" ForeColor="Black"></asp:Label>
						</td>

						<td style="border: 1px solid #000000; width: auto; background-color: lightgray; text-align: center;" colspan="3"></td>
					</tr>

				</table>


			</ContentTemplate>

		</ajaxToolkit:TabPanel>

		<%--Manual closing--%>
		<ajaxToolkit:TabPanel ID="TabPanel2" runat="server" HeaderText="Manual Closing">
			<ContentTemplate>
				<table style="width: 100%; font-family: 'Segoe UI'; font-size: small; float: left; background-color: lightgrey; border-spacing: 0px; margin-left: 0px; padding-left: 0px; padding-right: 0px;">
					<%--L1--%>
					<tr style="height: 26px; background-color: lightgray;">
						<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">JV No.:&nbsp;&nbsp;

						</td>

						<td style="border: 1px solid #000000; width: 150px; background-color: cornsilk; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:TextBox ID="txtJVNo" runat="server" Font-Names="Segoe UI" Font-Size="small" ReadOnly="true" AutoPostBack="true" CssClass="txtBoxC"></asp:TextBox>
						</td>

						<td style="border: 1px solid #000000; width: 80px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Ref VP No.:&nbsp;&nbsp;

						</td>
						<%--BorderStyle="Solid" BorderWidth="1px" BorderColor="Black"--%>
						<td style="border: 1px solid #000000; width: 200px; background-color: cornsilk; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:Label ID="txtREfNo" runat="server" ForeColor="Red" Width="145px"></asp:Label>

							<asp:Label ID="txtTC" runat="server" ForeColor="Red" Width="30px"></asp:Label>

						</td>

						<td style="border: 1px solid #000000; width: 80px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Vendor:&nbsp;&nbsp;

						</td>

						<td style="border: 1px solid #000000; width: 500px; background-color: cornsilk; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">&nbsp;&nbsp;
							<asp:Label ID="txtVendor" runat="server" Text="Vendor" ForeColor="Red"></asp:Label>
						</td>

						<td style="width: 200px; background-color: white; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:CheckBox ID="CheckBox2" Text="Single Vendor" AutoPostBack="true" runat="server" />
						</td>

						<td style="width: 350px; background-color: white; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>

					</tr>

					<tr style="background-color: lightgray;">
						<td style="width: auto; background-color: white; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="8"></td>
					</tr>
					<%--L2--%>
					<tr style="height: 26px; background-color: lightgray;">
						<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Posting Date:&nbsp;&nbsp;

						</td>

						<td style="border: 1px solid #000000; width: 150px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:TextBox ID="dpPostDate" runat="server" TextMode="Date" Height="20px" Font-Names="Segoe UI" Font-Size="small" AutoPostBack="true" CssClass="DateBox" Width="146px"></asp:TextBox>
						</td>

						<td style="border: 1px solid #000000; width: 80px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Amount:&nbsp;&nbsp;

						</td>

						<td style="border: 1px solid #000000; width: 200px; background-color: cornsilk; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:Label ID="txtVPamt" runat="server" Text="0.00" ForeColor="Red"></asp:Label>
						</td>

						<td style="border: 1px solid #000000; width: 80px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Remarks:&nbsp;&nbsp;

						</td>

						<td style="border: 1px solid #000000; width: 500px; background-color: cornsilk; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">&nbsp;&nbsp;
							<asp:Label ID="txtRemarks" runat="server" Text="Remarks" ForeColor="Red"></asp:Label>
						</td>

						<td style="width: 200px; background-color: white; text-align: LEFT; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:CheckBox ID="CheckBox1" Text="Show MMRR" AutoPostBack="true" runat="server" />
						</td>

						<td style="width: 350px; background-color: white; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>


					</tr>
					<tr style="background-color: lightgray;">
						<td style="width: auto; background-color: white; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="8"></td>
					</tr>
				</table>

				<table style="width: 100%; font-family: 'Segoe UI'; font-size: small; float: left; background-color: lightgrey; border-spacing: 0px; margin-left: 0px; padding-left: 0px; padding-right: 0px;">
					<tr style="height: 26px; background-color: lightgray">
						<td style="border: 1px solid #000000; width: 80px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Itm No.</td>
						<td style="border: 1px solid #000000; width: 300px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Cost Center
							<asp:LinkButton ID="lbCCenter" runat="server" Style="text-decoration: none; background-color: lightgray; float: right; padding-right: 2px;">&nbsp;
							<asp:Image runat="server" imageurl="~/images/check_16.png" style="vertical-align: middle"/>
							</asp:LinkButton>&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: 450px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">GL Account
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
						<%--<td style="width: 25px; background-color: white; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" rowspan="2"></td>--%>

						<td style="width: 350px; background-color: white; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; vertical-align: top;" rowspan="4">
							<asp:Panel ID="Panel5" runat="server" Height="621px" Width="348px" Visible="false" BorderStyle="Solid" BorderWidth="1px">
								<table style="width: 100%; font-family: 'Segoe UI'; font-size: small; float: left; background-color: lightgrey; border-spacing: 0px; margin-left: 0px; padding-left: 0px; padding-right: 0px;">
									<tr style="height: 26px; background-color: lightgray;">
										<td style="border: 1px solid #000000; width: 150px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
											<asp:Label ID="lblPopUpLabel" runat="server" Text=""></asp:Label>
											&nbsp;&nbsp;:
										</td>

										<td style="border: 1px solid #000000; width: 200px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
											<asp:Label ID="txtAmtSelect" runat="server" Text="0.00"></asp:Label>
										</td>
									</tr>
									<tr style="background-color: lightgray;">
										<td style="border: 1px solid #000000; height: 554px; background-color: floralwhite; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="2">
											<asp:Panel ID="Panel6" runat="server" Width="99%" Height="99%">
												<asp:GridView ID="dgvMMRRlist" runat="server" AutoGenerateColumns="False" OnRowDataBound="dgvGLentry_RowDataBound" OnRowCreated="dgvGLentry_RowCreated" Font-Names="Segoe UI" Font-Size="small" BackColor="White" BorderColor="#999999" BorderStyle="Solid" AllowSorting="true" BorderWidth="1px" CellPadding="3" GridLines="Vertical" HeaderStyle-BorderColor="Black" ShowHeader="false">
													<AlternatingRowStyle BackColor="#DCDCDC" />
													<Columns>
														<asp:TemplateField>
															<ItemTemplate>
																<asp:CheckBox ID="chkRow" runat="server" AutoPostBack="true" />
															</ItemTemplate>
														</asp:TemplateField>

														<asp:BoundField DataField="venno">
															<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="60px"></ItemStyle>
														</asp:BoundField>

														<asp:BoundField DataField="venname">
															<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="300px"></ItemStyle>
														</asp:BoundField>

														<asp:BoundField DataField="dramt" DataFormatString="{0:#,##0.00;(#,##0.00)}">
															<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="135px"></ItemStyle>
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
													$('#<%=dgvMMRRlist.ClientID %>').Scrollable({
										ScrollHeight: 563,
										IsInPanel: true
									});
								});
											</script>
										</td>

									</tr>


								</table>


							</asp:Panel>

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
							<asp:TextBox ID="txtSubAcct" runat="server" Font-Names="Segoe UI" Font-Size="Small" AutoPostBack="true" MaxLength="5" CssClass="txtBoxC" Width="94%"></asp:TextBox>
						</td>


					</tr>

					<tr style="height: auto; background-color: lightgray">
						<td style="border: 1px solid #000000; height: 542px; background-color: floralwhite; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; vertical-align: top; margin-left: 0px" colspan="7">
							<asp:Panel ID="Panel3" runat="server" ScrollBars="Auto" CssClass="PanelStd2">
								<asp:GridView ID="DgvJVdet" runat="server" AutoGenerateColumns="False" OnRowDataBound="DgvJVdet_RowDataBound" OnRowCreated="DgvJVdet_RowCreated" Font-Names="Segoe UI" Font-Size="Small" BackColor="White" BorderColor="#999999" BorderStyle="Solid"
									BorderWidth="1px" CellPadding="3" GridLines="Vertical" ShowHeader="False" CssClass="Grid1">
									<AlternatingRowStyle BackColor="Gainsboro" />
									<Columns>
										<asp:BoundField DataField="itmno">
											<ItemStyle HorizontalAlign="center" VerticalAlign="Middle" Width="89px">
											</ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="ccno">
											<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="342px">
											</ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="acctno">
											<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="513px">
											</ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="dramt" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="135px">
											</ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="cramt" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="135px">
											</ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="pk">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="53px">
											</ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="subacct">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="69px">
											</ItemStyle>
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
							<asp:ImageButton ID="btnDel" runat="server" ImageUrl="~/images/delete_16.png" />

						</td>

					</tr>

					<tr style="height: 26px; background-color: lightgray">
						<td style="border: 1px solid #000000; width: auto; background-color: lightgray; text-align: right; font-family: 'Segoe UI'; font-size: small;" colspan="3">Total &nbsp;&nbsp;
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

						<%--<td style="background-color: white; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>--%>
					</tr>

					<tr style="height: auto; background-color: lightgray">
						<td style="border: 1px solid #000000; height: 20px; background-color: lightgray; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="13">&nbsp;&nbsp; 
							Doc Status: &nbsp;&nbsp;
								<asp:Label ID="tssDocStat" runat="server" Text="New" ForeColor="Red"></asp:Label>
							&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Edit Doc No.: &nbsp;&nbsp;
								<asp:Label ID="tssDocNo" runat="server" Text="00000000" ForeColor="Red"></asp:Label>
							&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Line Item: &nbsp;&nbsp;
								<asp:Label ID="lblLineItm" runat="server" Text="New" ForeColor="Red"></asp:Label>
							&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
							Doc No. Edit:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
							<asp:Label ID="tssDocNoEdit" runat="server" ForeColor="Red"></asp:Label>
							&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
							<asp:Label ID="tssNameMe" runat="server" Text="NameMe" ForeColor="Red"></asp:Label>
							&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
							<asp:Label ID="tsErrMsg" runat="server" Text="" ForeColor="Red"></asp:Label>

						</td>
					</tr>


				</table>

			</ContentTemplate>

		</ajaxToolkit:TabPanel>

		<%--Subsidiary--%>
		<ajaxToolkit:TabPanel ID="TabPanel3" runat="server" HeaderText="Subsidiary">
			<ContentTemplate>
				<table style="width: 100%; font-family: 'Segoe UI'; font-size: small; float: left; background-color: lightgrey; border-spacing: 0px; margin-left: 0px; padding-left: 0px; padding-right: 0px;">
					<tr style="height: 26px; background-color: lightgray">
						<td style="border: 1px solid #000000; width: 80px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">As Of:&nbsp;&nbsp;
						</td>

						<td style="border: 1px solid #000000; width: 122px; background-color: lightgray; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:TextBox ID="dpTransDate" runat="server" TextMode="Date" Width="120px" Height="99%" Font-Names="Segoe UI" Font-Size="small" AutoPostBack="true" CssClass="DateBox"></asp:TextBox>
						</td>

						<td style="border: 1px solid #000000; width: 160px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Select Vendor:&nbsp;&nbsp;
						</td>

						<td style="border: 1px solid #000000; width: 450px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:DropDownList ID="cboVenName" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" CssClass="cboBox3" Width="99%">
							</asp:DropDownList>

						</td>

						<td style="border: 1px solid #000000; width: auto; background-color: lightgray; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="7">
							<asp:LinkButton ID="LinkButton1" runat="server" Style="text-decoration: none; background-color: lightgray; float: left; padding-right: 2px;">
							<asp:Image runat="server" imageurl="~/images/check_16.png" style="vertical-align: middle"/>
							</asp:LinkButton>&nbsp;
						</td>

					</tr>

					<tr style="background-color: lightgray">
						<td style="border: 1px solid #000000; height: 653px; background-color: floralwhite; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; vertical-align: top; margin-left: 0px" colspan="11">
							<asp:Panel ID="Panel4" runat="server" Width="100%" Height="651px">
								<asp:GridView ID="dgvSubs" runat="server" AutoGenerateColumns="False" OnRowDataBound="dgvSubs_RowDataBound" OnRowCreated="dgvSubs_RowCreated" Font-Names="Segoe UI" Font-Size="small" BackColor="White" BorderColor="#999999" BorderStyle="Solid" AllowSorting="true" BorderWidth="1px" CellPadding="3" GridLines="Vertical" HeaderStyle-BorderColor="Black" ShowHeader="true">
									<AlternatingRowStyle BackColor="#DCDCDC" />
									<Columns>
										<asp:CommandField HeaderText="Select" ShowSelectButton="true">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="72px"></ItemStyle>
										</asp:CommandField>

										<asp:BoundField DataField="docno" HeaderText="DO No.">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="110px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="tc" HeaderText="TC">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="60px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="transdate" HeaderText="Date" DataFormatString="{0:yyyy-MM-dd}">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="venno" HeaderText="Vendor">
											<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="400px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="dramt" HeaderText="Amount" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="118px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="remarks" HeaderText="Remarks">
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
									$('#<%=dgvSubs.ClientID %>').Scrollable({
										ScrollHeight: 620,
										IsInPanel: true
									});
								});
							</script>
						</td>

					</tr>

					<tr style="height: 24px; background-color: lightgray">
						<td style="border: 1px solid #000000; width: 800px; background-color: lightgray; text-align: center;" colspan="6">Total
						</td>

						<td style="border: 1px solid #000000; width: 120px; background-color: whitesmoke; text-align: right;">
							<asp:Label ID="lblDRamtTot3" runat="server" Text="0.00" ForeColor="Black"></asp:Label>&nbsp;&nbsp;
						</td>

						<td style="border: 1px solid #000000; width: 120px; background-color: whitesmoke; text-align: right;">
							<asp:Label ID="lblDCRamtTot3" runat="server" Text="0.00" ForeColor="Black"></asp:Label>&nbsp;&nbsp;
						</td>

						<td style="border: 1px solid #000000; width: auto; background-color: lightgray; text-align: center;" colspan="3"></td>
					</tr>

				</table>

			</ContentTemplate>

		</ajaxToolkit:TabPanel>

		<ajaxToolkit:TabPanel ID="TabPanel4" runat="server" HeaderText="Transactions">
			<ContentTemplate>
				<h1>Not Yet Available</h1>
			</ContentTemplate>

		</ajaxToolkit:TabPanel>

		<ajaxToolkit:TabPanel ID="TabPanel5" runat="server" HeaderText="Manual Closing With JV">
			<ContentTemplate>
				<h1>Not Yet Available</h1>
			</ContentTemplate>

		</ajaxToolkit:TabPanel>

	</ajaxToolkit:TabContainer>

</asp:Content>
