<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/FI.master" CodeBehind="GLReports.aspx.vb" Inherits="AOS100web.GLReportsNew" %>

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

	<ajaxToolkit:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" AutoPostBack="true" Style="width: 99%; height: auto;">
		<ajaxToolkit:TabPanel ID="TabPanel1" runat="server" HeaderText="Trial Balance">
			<ContentTemplate>
				<table style="width: 100%; font-family: 'Segoe UI'; font-size: small; float: left; background-color: whitesmoke; border-spacing: 0px; margin-left: 0px; padding-left: 0px; padding-right: 0px;">
					<tr style="height: 26px;">
						<td style="width: 150px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
						<td style="width: auto; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="8">
							 For the Month of:&nbsp;&nbsp;
							<asp:DropDownList ID="cboFSmonYear" runat="server" Width="150px" AutoPostBack="true"></asp:DropDownList>
							&nbsp;&nbsp;&nbsp;&nbsp;
							Report Option:&nbsp;&nbsp;
							<asp:RadioButton ID="rbMon" runat="server" Text="For the Month" AutoPostBack="true" GroupName="TB"/>&nbsp;&nbsp;&nbsp;&nbsp;
							<asp:RadioButton ID="rbYr" runat="server" text="YTD" AutoPostBack="true" GroupName="TB"/>
								<asp:Label ID="lblTBdate1" runat="server" Text="yyyy-MM-dd" Visible="false"></asp:Label>
							&nbsp;&nbsp;&nbsp;&nbsp;
								<asp:Label ID="lblTBdate2" runat="server" Text="yyyy-MM-dd" Visible="false"></asp:Label>
							&nbsp;&nbsp;&nbsp;&nbsp;
								<asp:Label ID="lblTBFirtDay" runat="server" Text="yyyy-MM-dd" Visible="false"></asp:Label>
						</td>
						<td style="width: 150px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>

					</tr>

					<%-- L1--%>
									 
					<tr style="height: 13px;">
						<td style="width: auto; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="10">
						   
						</td>
					</tr>

					<tr style="height: auto;">
						<td style="width: 150px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
						   
						</td>

						<td style="width: auto; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; " colspan="8">
							<asp:Panel ID="Panel3" runat="server" Width="100%" Height="670px">
								<asp:GridView ID="dgvTbals" runat="server" AutoGenerateColumns="False" OnRowDataBound="dgvTbals_RowDataBound" OnRowCreated="dgvTbals_RowCreated" Font-Names="Segoe UI" Font-Size="small" BackColor="White" BorderColor="#999999" BorderStyle="Solid" AllowSorting="true" BorderWidth="1px" CellPadding="3" GridLines="Vertical" HeaderStyle-BorderColor="Black" ShowHeader="true" HeaderStyle-HorizontalAlign="Center" ShowFooter="true">
									<AlternatingRowStyle BackColor="#DCDCDC" />
									<Columns>
										<%--<asp:BoundField DataField="acctdesc" HeaderText="Account Description">
											<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="740px"></ItemStyle>
										</asp:BoundField>

										 <asp:BoundField DataField="begamt" HeaderText="Beg Balance" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="120px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="dramt" HeaderText="Debit" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="120px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="cramt" HeaderText="Credit" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="120px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="balamt" HeaderText="Ending Bal" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="120px"></ItemStyle>
										</asp:BoundField>--%>

										<asp:TemplateField HeaderText="Account Description">
											<ItemTemplate>
												<asp:Label ID="Label1" runat="server" Text='<%# Bind("acctdesc") %>'></asp:Label>
											</ItemTemplate>
											<ItemStyle HorizontalAlign="Left" Width="740px" />
											<FooterTemplate>
												<div style="text-align: center;">
													<asp:Label ID="lblGTotal" runat="server" Font-Bold="true" BackColor="#CCCCCC" ForeColor="White" Width="100%" Height="100%" />
												</div>
											</FooterTemplate>
											<FooterStyle BorderStyle="Solid" HorizontalAlign="Right" VerticalAlign="Middle" />
										</asp:TemplateField>

										<asp:TemplateField HeaderText="Beg Balance">
											<ItemTemplate>
												<asp:Label ID="txtBeg" runat="server" Text='<%# Bind("begamt", "{0:#,##0.00;(#,##0.00)}") %>'></asp:Label>
											</ItemTemplate>
											<ItemStyle HorizontalAlign="Right" />
											<FooterTemplate>
												<div style="text-align: right;">
													<asp:Label ID="lblBeg" runat="server" Font-Bold="true" BackColor="#CCCCCC" ForeColor="White" Width="120px" Height="100%" />
												</div>
											</FooterTemplate>
											<FooterStyle BorderStyle="Solid" HorizontalAlign="Right" VerticalAlign="Middle" />
										</asp:TemplateField>

										<asp:TemplateField HeaderText="Debit">
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

										<asp:TemplateField HeaderText="Credit">
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

										<asp:TemplateField HeaderText="Ending Bal">
											<ItemTemplate>
												<asp:Label ID="txtEnd" runat="server" Text='<%# Bind("balamt", "{0:#,##0.00;(#,##0.00)}") %>'></asp:Label>
											</ItemTemplate>
											<ItemStyle HorizontalAlign="Right" />
											<FooterTemplate>
												<div style="text-align: right;">
													<asp:Label ID="lblEnd" runat="server" Font-Bold="true" BackColor="#CCCCCC" ForeColor="White" Width="120px" Height="100%" />
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
									$('#<%=dgvTbals.ClientID %>').Scrollable({
										ScrollHeight: 645,
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

		<%--SO List--%>
		<ajaxToolkit:TabPanel ID="TabPanel2" runat="server" HeaderText="General Ledger">
			<ContentTemplate>
				<table style="width: 100%; font-family: 'Segoe UI'; font-size: small; float: left; background-color: whitesmoke; border-spacing: 0px; margin-left: 0px; padding-left: 0px; padding-right: 0px;">
					<tr style="height: 26px;">
						<td style="width: 150px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
						<td style="width: auto; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="8">
							 For the Month of:&nbsp;&nbsp;
							<asp:DropDownList ID="cboFSmonYear2" runat="server" Width="150px" AutoPostBack="true"></asp:DropDownList>
							&nbsp;&nbsp;&nbsp;&nbsp;
							Report Option:&nbsp;&nbsp;
							<asp:RadioButton ID="RadioButton1" runat="server" Text="For the Month" AutoPostBack="true" GroupName="TB"/>&nbsp;&nbsp;&nbsp;&nbsp;
							<asp:RadioButton ID="RadioButton2" runat="server" text="YTD" AutoPostBack="true" GroupName="TB"/>
								<asp:Label ID="Label1" runat="server" Text="yyyy-MM-dd" Visible="false"></asp:Label>
							&nbsp;&nbsp;&nbsp;&nbsp;
								<asp:Label ID="Label2" runat="server" Text="yyyy-MM-dd" Visible="false"></asp:Label>
							&nbsp;&nbsp;&nbsp;&nbsp;
								<asp:Label ID="Label3" runat="server" Text="yyyy-MM-dd" Visible="false"></asp:Label>
						</td>
						<td style="width: 150px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>

					</tr>

					<%-- L1--%>
									 
					<tr style="height: 13px;">
						<td style="width: auto; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="10">
						   
						</td>
					</tr>

					<tr style="height: auto;">
						<td style="width: 150px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
						   
						</td>

						<td style="width: auto; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; vertical-align: top; " colspan="8">
							<asp:Panel ID="Panel1" runat="server" Width="100%" Height="615px">
								<asp:GridView ID="dgvGL" runat="server" AutoGenerateColumns="False" OnRowDataBound="dgvGL_RowDataBound" OnRowCreated="dgvGL_RowCreated" Font-Names="Segoe UI" Font-Size="small" BackColor="White" BorderColor="#999999" BorderStyle="Solid" AllowSorting="true" BorderWidth="1px" CellPadding="3" GridLines="Vertical" HeaderStyle-BorderColor="Black" ShowHeader="true" HeaderStyle-HorizontalAlign="Center">
									<AlternatingRowStyle BackColor="#DCDCDC" />
									<Columns>
										<asp:BoundField DataField="acctdesc" HeaderText="Account Description">
											<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="780px"></ItemStyle>
										</asp:BoundField>
																		
										<asp:BoundField DataField="begamt" HeaderText="Beg Balance" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="125px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="dramt" HeaderText="Debit" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="125px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="cramt" HeaderText="Credit" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="125px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="balamt" HeaderText="Ending Bal" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="125px"></ItemStyle>
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
									$('#<%=dgvGL.ClientID %>').Scrollable({
										ScrollHeight: 590,
										IsInPanel: true
									});
								});
							</script>

						</td>
						<td style="width: 150px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
						   
						</td>
					</tr>
									
					<tr style="height: 26px;">
						<td style="width: 150px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
						   
						</td>

						<td style="border: 1px solid #000000; width: 500px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="3">
						   Total&nbsp;&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: 125px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
						   <asp:Label ID="lblBegAmt2" runat="server" Text="0.00" Font-Bold="true"></asp:Label>&nbsp;&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: 125px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
						   <asp:Label ID="lblDRamt2" runat="server" Text="0.00" Font-Bold="true"></asp:Label>&nbsp;&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: 125px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:Label ID="lblCRamt2" runat="server" Text="0.00" Font-Bold="true"></asp:Label>&nbsp;&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: 125px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:Label ID="lblBalAmt2" runat="server" Text="0.00" Font-Bold="true"></asp:Label>&nbsp;&nbsp;
						</td>
						<td style="width: 17px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
						<td style="width: 150px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
					</tr>
				</table>

			</ContentTemplate>

		</ajaxToolkit:TabPanel>

		<%--SO Monitoring--%>
		<ajaxToolkit:TabPanel ID="TabPanel3" runat="server" HeaderText="Balance Sheet">
			<ContentTemplate>
			   

			</ContentTemplate>

		</ajaxToolkit:TabPanel>

	</ajaxToolkit:TabContainer>

</asp:Content>
