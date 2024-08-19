<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/FI.master" CodeBehind="CashierReports.aspx.vb" Inherits="AOS100web.CashierReports" %>

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

	<ajaxToolkit:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" Style="width: 99%; height: auto;">
		<ajaxToolkit:TabPanel ID="TabPanel1" runat="server" HeaderText="Registers">
			<ContentTemplate>
				<center>
					<h1>
						Under Development
					</h1>
				</center>
			</ContentTemplate>

		</ajaxToolkit:TabPanel>

		<ajaxToolkit:TabPanel ID="TabPanel2" runat="server" HeaderText="Collections">
			<ContentTemplate>
				<table style="width: 100%; font-family: 'Segoe UI'; font-size: small; float: left; background-color: lightgrey; border-spacing: 0px; margin-left: 0px; padding-left: 0px; padding-right: 0px;">
					<%--line1--%>
					<tr style="height: 26px; background-color: lightgray">
						<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Reports:&nbsp;&nbsp;
							
						</td>
						<td style="border: 1px solid #000000; width: auto; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="4">
							<asp:DropDownList ID="cboColReport" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" Width="99%">
							</asp:DropDownList>
						</td>

						<td style="width: 2px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>

						<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Salesman:&nbsp;&nbsp; 
							
						</td>
						<td style="border: 1px solid #000000; width: 250px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:DropDownList ID="cboColSmn" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" Width="98%">
							</asp:DropDownList>
						</td>
						<td style="width: auto; background-color: whitesmoke; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
					
						<td style="width: 380px; background-color: whitesmoke; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" rowspan="2"></td>
					</tr>

					<tr style="height: 26px; background-color: lightgray">
						<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:Label ID="Label3" runat="server" Text="Date From:"></asp:Label>&nbsp;&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: 124px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:TextBox ID="dpColDFrom" runat="server" TextMode="Date" Height="20px" Font-Names="Segoe UI" Font-Size="small" AutoPostBack="true" CssClass="DateBox" Width="120px"></asp:TextBox>
						</td>

						<td style="width: 2px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>

						<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:Label ID="Label4" runat="server" Text="Date To:"></asp:Label>&nbsp;&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: 124px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:TextBox ID="dpColDTo" runat="server" TextMode="Date" Height="20px" Font-Names="Segoe UI" Font-Size="small" AutoPostBack="true" CssClass="DateBox" Width="120px"></asp:TextBox>
						</td>

						<td style="width: 2px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
						<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:Label ID="lblLabelCust" runat="server" Text="Customer:" Visible="false"></asp:Label>&nbsp;&nbsp;
						</td>
						
						<td style="border: 1px solid #000000; width: 550px; background-color: lightgray; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="2">&nbsp;
							<asp:DropDownList ID="cboColCust" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" Width="98%" Visible="false">
							</asp:DropDownList>
						</td>
					</tr>

					<tr style="height: auto; background-color: lightgray">
						<td style="width: auto; background-color: floralwhite; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="11"></td>
					</tr>
					<tr style="height: auto; background-color: lightgray">
						<td style="border: 1px solid #000000; height: 625px; width: auto; background-color: floralwhite; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; vertical-align:top; " colspan="11">
							<asp:Panel ID="Panel3" runat="server" Width="99%" Height="600px" Visible="false" >
								<asp:GridView ID="dgvColReg" runat="server" AutoGenerateColumns="False" OnRowDataBound="dgvColReg_RowDataBound"
									OnRowCreated="dgvColReg_RowCreated" Font-Names="Segoe UI" Font-Size="small" BackColor="White" BorderColor="#999999" BorderStyle="Solid" AllowSorting="true" BorderWidth="1px" CellPadding="3" GridLines="Vertical" HeaderStyle-BorderColor="Black" ShowHeader="true" HeaderStyle-HorizontalAlign="Center" >
									<AlternatingRowStyle BackColor="#DCDCDC" />
									<Columns>
										<asp:BoundField DataField="orno">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></ItemStyle>
										</asp:BoundField>
										
										<asp:BoundField DataField="transdate" DataFormatString="{0:yyyy-MM-dd}">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></ItemStyle>
										</asp:BoundField>
										
										<asp:BoundField DataField="docno">
											<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="100px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="smnno">
											<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="260px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="custno">
											<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="425px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="coltype">
											<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="100px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="cdcrno">
											<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="100px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="colamt" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="100px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="ewtamt" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="100px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="netamt" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="100px"></ItemStyle>
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
									$('#<%=dgvColReg.ClientID %>').Scrollable({
										ScrollHeight: 600,
										IsInPanel: true
									});
								});
							</script>

							<%--for detailed--%>
							<asp:Panel ID="Panel4" runat="server" Width="99%" Height="600px" Visible="false">
								<asp:GridView ID="dgvColSum" runat="server" AutoGenerateColumns="False" OnRowDataBound="dgvColSum_RowDataBound" OnRowCreated="dgvColSum_RowCreated" Font-Names="Segoe UI" Font-Size="small" BackColor="White" BorderColor="#999999" BorderStyle="Solid" AllowSorting="true" BorderWidth="1px" CellPadding="3" GridLines="Vertical" HeaderStyle-BorderColor="Black" ShowHeader="true" HeaderStyle-HorizontalAlign="Center" >
									<AlternatingRowStyle BackColor="#DCDCDC" />
									<Columns>
										<asp:BoundField DataField="smnno">
											<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="275px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="custno">
											<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="910px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="colamt" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="100px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="ewtamt" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="100px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="netamt" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="100px"></ItemStyle>
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
									$('#<%=dgvColSum.ClientID %>').Scrollable({
										ScrollHeight: 600,
										IsInPanel: true
									});
								});
							</script>
						</td>
					</tr>
					
				</table>

				<table style="width: 100%; font-family: 'Segoe UI'; font-size: small; float: left; background-color: lightgrey; border-spacing: 0px; margin-left: 0px; padding-left: 0px; padding-right: 0px;" >
					<tr style="height: 26px; background-color: lightgray;">
						<td style="border: 1px solid #000000; width: 1185px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Total &nbsp;&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: 102px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:Label ID="lblColAmt" runat="server" Text="0.00" ForeColor="Red"></asp:Label>
						</td>
						<td style="border: 1px solid #000000; width: 102px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:Label ID="lblEwtAmt" runat="server" Text="0.00" ForeColor="Red"></asp:Label>
						</td>
						<td style="border: 1px solid #000000; width: 102px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:Label ID="lblNetAmt" runat="server" Text="0.00" ForeColor="Red"></asp:Label>
						</td>
						<td style="border: 1px solid #000000; width: 15px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
						</td>
					</tr>
				</table>

			</ContentTemplate>

		</ajaxToolkit:TabPanel>

		<ajaxToolkit:TabPanel ID="TabPanel3" runat="server" HeaderText="Deposits">
			<ContentTemplate>
				<table style="width: 100%; font-family: 'Segoe UI'; font-size: small; float: left; background-color: lightgrey; border-spacing: 0px; margin-left: 0px; padding-left: 0px; padding-right: 0px;">
					<%--line1--%>
					<tr style="height: 26px; background-color: lightgray">
						<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Reports:&nbsp;&nbsp;
							
						</td>
						<td style="border: 1px solid #000000; width: auto; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="4">
							<asp:DropDownList ID="cboDepReport" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" Width="99%">
							</asp:DropDownList>
						</td>

						<td style="width: 2px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>

						<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Bank:&nbsp;&nbsp; 
							
						</td>
						<td style="border: 1px solid #000000; width: 450px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:DropDownList ID="cboBank" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" Width="98%">
							</asp:DropDownList>
						</td>
						<td style="width: auto; background-color: whitesmoke; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>

						<td style="width: 380px; background-color: whitesmoke; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" rowspan="2"></td>
					</tr>

					<tr style="height: 26px; background-color: lightgray">
						<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:Label ID="Label1" runat="server" Text="Date From:"></asp:Label>&nbsp;&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: 124px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:TextBox ID="dpDepDate1" runat="server" TextMode="Date" Height="20px" Font-Names="Segoe UI" Font-Size="small" AutoPostBack="true" CssClass="DateBox" Width="120px"></asp:TextBox>
						</td>

						<td style="width: 2px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>

						<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:Label ID="Label2" runat="server" Text="Date To:"></asp:Label>&nbsp;&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: 124px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:TextBox ID="dpDepDate2" runat="server" TextMode="Date" Height="20px" Font-Names="Segoe UI" Font-Size="small" AutoPostBack="true" CssClass="DateBox" Width="120px"></asp:TextBox>
						</td>

						<td style="width: 2px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
						<td style="background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							
						</td>

						<td style="width: 550px; background-color: whitesmoke; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="2">&nbsp;
							
						</td>
					</tr>

					<tr style="height: auto; background-color: lightgray">
						<td style="width: auto; background-color: floralwhite; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="11"></td>
					</tr>

					<tr style="height: auto; background-color: lightgray">
						<td style="width: auto; background-color: floralwhite; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="11"></td>
					</tr>
					<tr style="height: auto; background-color: lightgray">
						<td style="border: 1px solid #000000; height: 625px; width: auto; background-color: floralwhite; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; vertical-align:top; " colspan="11">
							<asp:Panel ID="Panel1" runat="server" Width="99%" Height="600px" Visible="false" >
								<asp:GridView ID="dgvDepReg" runat="server" AutoGenerateColumns="False" OnRowDataBound="dgvDepReg_RowDataBound"
									OnRowCreated="dgvDepReg_RowCreated" Font-Names="Segoe UI" Font-Size="small" BackColor="White" BorderColor="#999999" BorderStyle="Solid" AllowSorting="true" BorderWidth="1px" CellPadding="3" GridLines="Vertical" HeaderStyle-BorderColor="Black" ShowHeader="true" HeaderStyle-HorizontalAlign="Center" >
									<AlternatingRowStyle BackColor="#DCDCDC" />
									<Columns>
										<asp:BoundField DataField="depno">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></ItemStyle>
										</asp:BoundField>
										
										<asp:BoundField DataField="transdate" DataFormatString="{0:yyyy-MM-dd}">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="cdcrno">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></ItemStyle>
										</asp:BoundField>
										
										<asp:BoundField DataField="acctno">
											<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="850px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="cashamt" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="100px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="checkamt" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="100px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="totamt" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="100px"></ItemStyle>
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
									$('#<%=dgvDepReg.ClientID %>').Scrollable({
										ScrollHeight: 600,
										IsInPanel: true
									});
								});
							</script>

							<%--for detailed--%>
							<asp:Panel ID="Panel2" runat="server" Width="99%" Height="600px" Visible="false">
								<asp:GridView ID="dgvDepSum" runat="server" AutoGenerateColumns="False" OnRowDataBound="dgvDepSum_RowDataBound" OnRowCreated="dgvDepSum_RowCreated" Font-Names="Segoe UI" Font-Size="small" BackColor="White" BorderColor="#999999" BorderStyle="Solid" AllowSorting="true" BorderWidth="1px" CellPadding="3" GridLines="Vertical" HeaderStyle-BorderColor="Black" ShowHeader="true" HeaderStyle-HorizontalAlign="Center" >
									<AlternatingRowStyle BackColor="#DCDCDC" />
									<Columns>
										<asp:BoundField DataField="depno">
											<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="275px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="remarks">
											<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="910px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="cashamt" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="100px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="checkamt" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="100px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="totamt" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="100px"></ItemStyle>
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
									$('#<%=dgvDepSum.ClientID %>').Scrollable({
										ScrollHeight: 600,
										IsInPanel: true
									});
								});
							</script>
						</td>
					</tr>
				</table>

				<table style="width: 100%; font-family: 'Segoe UI'; font-size: small; float: left; background-color: lightgrey; border-spacing: 0px; margin-left: 0px; padding-left: 0px; padding-right: 0px;" >
					<tr style="height: 26px; background-color: lightgray;">
						<td style="border: 1px solid #000000; width: 1185px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Total &nbsp;&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: 102px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:Label ID="lblCashAmt" runat="server" Text="0.00" ForeColor="Red"></asp:Label>
						</td>
						<td style="border: 1px solid #000000; width: 102px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:Label ID="lblCheckAmt" runat="server" Text="0.00" ForeColor="Red"></asp:Label>
						</td>
						<td style="border: 1px solid #000000; width: 102px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:Label ID="lblTotAmt" runat="server" Text="0.00" ForeColor="Red"></asp:Label>
						</td>
						<td style="border: 1px solid #000000; width: 15px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
						</td>
					</tr>
				</table>



			

			</ContentTemplate>

		</ajaxToolkit:TabPanel>

	</ajaxToolkit:TabContainer>


</asp:Content>
