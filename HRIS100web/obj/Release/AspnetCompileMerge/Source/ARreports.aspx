<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/FI.master" CodeBehind="ARreports.aspx.vb" Inherits="AOS100web.ARreports" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentOfFinAcctg" runat="server">
	<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
	<script type="text/javascript">
		function Confirm() {
			var confirm_value = document.createElement("INPUT");
			confirm_value.type = "hidden";
			confirm_value.name = "confirm_value";
			if (confirm("Are you sure to POST Selected Document?")) {
				confirm_value.value = "Yes";
			} else {
				confirm_value.value = "No";
			}
			document.forms[0].appendChild(confirm_value);
		}
	</script>

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



	<table style="width: 100%; font-family: 'Segoe UI'; float: left; margin-top: -2px; padding-top: 0px">
		<tr>
			<td style="background-color: #b1bbd7; border-top: solid; border-bottom: solid; border-color: red; border-width: 2px; width: 100%; font-family: 'Segoe UI'; font-size: 10px; text-align: center; min-height: 28px; max-height: 28px; padding-bottom: 1px; padding-top: 1px"
				colspan="4">
				<asp:LinkButton ID="lbHome" runat="server" OnClick="lbHome_Click" Style="text-decoration: none; background-color: #7fc6f6; font-family: Tahoma; font-size: 14px; color: white; margin-right: 5px; padding-left: 5px; padding-right: 5px; min-height: 22px;"
					BorderStyle="Solid" ToolTip="Reset Fields">
					<asp:Image runat="server" imageurl="~/images/home_16.png" style="vertical-align: middle"/>&nbsp;Home
				</asp:LinkButton>
				<asp:LinkButton ID="lbReset" runat="server" OnClick="lbReset_Click" Style="text-decoration: none; background-color: #7fc6f6; padding-right: 5px; font-family: Tahoma; font-size: 14px; color: white; margin-right: 5px; padding-left: 5px; min-height: 22px;"
					BorderStyle="Solid" ToolTip="Save" Enabled="true">
					<asp:Image runat="server" imageurl="~/images/reset_16.png" style="vertical-align: middle; "/>&nbsp;Reset
				</asp:LinkButton>
				<asp:LinkButton ID="lbPrint" runat="server" OnClick="lbPrint_Click" Style="text-decoration: none; background-color: #7fc6f6; padding-right: 5px; font-family: Tahoma; font-size: 14px; color: white; margin-right: 5px; padding-left: 5px; min-height: 22px;"
					BorderStyle="Solid" ToolTip="Reports" Enabled="true">
					<asp:Image runat="server" imageurl="~/images/Print_16.png" style="vertical-align: middle"/>&nbsp;Print
				</asp:LinkButton>
				<asp:LinkButton ID="lbExcel" runat="server" OnClick="lbExcel_Click" Style="text-decoration: none; background-color: #7fc6f6; padding-right: 5px; font-family: Tahoma; font-size: 14px; color: white; margin-right: 5px; padding-left: 5px; min-height: 22px;"
					BorderStyle="Solid" ToolTip="Delete" Enabled="true">
					<asp:Image runat="server" imageurl="~/images/excel-logo_24.png" style="vertical-align: middle"/>&nbsp;Export
				</asp:LinkButton>
				<asp:LinkButton ID="lbClose" runat="server" OnClick="lbClose_Click" Style="text-decoration: none; background-color: #7fc6f6; font-family: Tahoma; font-size: 14px; color: white; min-height: 22px; padding-left: 5px;"
					BorderStyle="Solid" ToolTip="Close">
					<asp:Image runat="server" imageurl="~/images/Exit_16.png" style="vertical-align: middle"/>&nbsp;Close&nbsp;
				</asp:LinkButton>

				<asp:Label ID="lblUser" runat="server" Text="User" Visible="false"></asp:Label>
				<asp:Label ID="lblGrpUser" runat="server" Text="Grp User" Visible="false"></asp:Label>
			</td>
						
		</tr>
		<tr>
			<td style="border: 1px solid #000000; width: auto; background-color: lightgray;" colspan="7"></td>
		</tr>
		<tr>
			<td style="border: 1px solid #000000; width: auto; background-color: lightgray;"></td>
		</tr>
		<tr>
			<td style="height: 26px; width: auto; background-color: lightgray;" colspan="7">
				
			</td>
		</tr>

	</table>

	<ajaxToolkit:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" Style="width: 100%; height: auto;">
		<ajaxToolkit:TabPanel ID="TabPanel1" runat="server" HeaderText="Detailed">
			<ContentTemplate>
				<table style="width: 100%; font-family: 'Segoe UI'; font-size: small; float: left; background-color: whitesmoke; border-spacing: 0px; margin-left: 0px; padding-left: 0px; padding-right: 0px;">
					<tr style="height: 26px;">
						<td style="width: 150px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
						<td style="width: auto; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="8">
							As of:&nbsp;&nbsp;
							<asp:TextBox ID="dpTransdate" runat="server" TextMode="Date" Height="20px" Font-Names="Segoe UI" Font-Size="small" AutoPostBack="true" CssClass="DateBox" Width="120px"></asp:TextBox>
							&nbsp;&nbsp;&nbsp;&nbsp;
							Filter 1:&nbsp;&nbsp;
							<asp:DropDownList ID="cboFormat" runat="server" Width="200px" AutoPostBack="true"></asp:DropDownList>&nbsp;&nbsp;
							<asp:Label ID="lblFilter2" runat="server" Text="Filter 2:" Visible="false"></asp:Label>&nbsp;&nbsp;
							<asp:DropDownList ID="cboARcust" runat="server" Width="450px" AutoPostBack="true" Visible="false"></asp:DropDownList>
						</td>
						<td style="width: 150px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>

					</tr>

					<%-- L1--%>

					<tr style="height: 13px;">
						<td style="width: auto; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="10"></td>
					</tr>

					<tr style="height: auto;">
						<td style="width: auto; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; vertical-align: top;" colspan="10">
							<asp:Panel ID="Panel2" runat="server" Width="99%" Height="670px">
								<asp:GridView ID="dgvARreport" runat="server" AutoGenerateColumns="False" OnRowDataBound="dgvARreport_RowDataBound" OnRowCreated="dgvARreport_RowCreated" Font-Names="Segoe UI" Font-Size="10px" BackColor="White" BorderColor="#999999" BorderStyle="Solid" AllowSorting="true" BorderWidth="1px" CellPadding="3" GridLines="Vertical" HeaderStyle-BorderColor="Black" ShowHeader="true" ShowFooter="true" HeaderStyle-HorizontalAlign="Center">

									<AlternatingRowStyle BackColor="#DCDCDC" />
									<Columns>
										<asp:BoundField DataField="custno" HeaderText="Cust. No.">
											<ItemStyle HorizontalAlign="Center"></ItemStyle>
										</asp:BoundField>
										<asp:TemplateField HeaderText="Cust Name">
											<ItemTemplate>
												<asp:Label ID="Label1" runat="server" Text='<%# Bind("custname") %>'></asp:Label>
											</ItemTemplate>
											<ItemStyle HorizontalAlign="Left" Width="325px" />
											<FooterTemplate>
												<div style="text-align: center;">
													<asp:Label ID="lblGTotal" runat="server" Font-Bold="true" BackColor="Gray" ForeColor="White" Width="100%" Height="100%" />
												</div>
											</FooterTemplate>
											<FooterStyle BorderStyle="Solid" HorizontalAlign="Right" VerticalAlign="Middle" />
										</asp:TemplateField>
										<asp:BoundField DataField="shipto" HeaderText="Ship To">
											<ItemStyle HorizontalAlign="Center"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="shiptoname" HeaderText="Ship To Name">
											<ItemStyle HorizontalAlign="Left" Width="325px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="invno" HeaderText="Inv. No.">
											<ItemStyle HorizontalAlign="Center"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="tc" HeaderText="TC">
											<ItemStyle HorizontalAlign="Center"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="transdate" HeaderText="Date" DataFormatString="{0:yyyy-MM-dd}">
											<ItemStyle HorizontalAlign="Center" Width="80px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="term" HeaderText="Terms">
											<ItemStyle HorizontalAlign="Center"></ItemStyle>
										</asp:BoundField>

										<asp:TemplateField HeaderText="A/R Amount">
											<ItemTemplate>
												<asp:Label ID="txtNet" runat="server" Text='<%# Bind("totalar", "{0:#,##0.00;(#,##0.00)}") %>'></asp:Label>
											</ItemTemplate>
											<ItemStyle HorizontalAlign="Right" />
											<FooterTemplate>
												<div style="text-align: right;">
													<asp:Label ID="lblTotalNet" runat="server" Font-Bold="true" BackColor="Gray" ForeColor="White" Width="100%" Height="100%" />
												</div>
											</FooterTemplate>
											<FooterStyle BorderStyle="Solid" HorizontalAlign="Right" VerticalAlign="Middle" />
										</asp:TemplateField>

										<asp:TemplateField HeaderText="1-30 Days">
											<ItemTemplate>
												<asp:Label ID="txt30d" runat="server" Text='<%# Bind("30d", "{0:#,##0.00;(#,##0.00)}") %>'></asp:Label>
											</ItemTemplate>
											<ItemStyle HorizontalAlign="Right" />
											<FooterTemplate>
												<div style="text-align: right;">
													<asp:Label ID="lblTotal30d" runat="server" Font-Bold="true" BackColor="Gray" ForeColor="White" Width="100%" Height="100%" />
												</div>
											</FooterTemplate>
											<FooterStyle BorderStyle="Solid" HorizontalAlign="Right" VerticalAlign="Middle" />
										</asp:TemplateField>

										<asp:TemplateField HeaderText="31-60 Days">
											<ItemTemplate>
												<asp:Label ID="txt60d" runat="server" Text='<%# Bind("60d", "{0:#,##0.00;(#,##0.00)}") %>'></asp:Label>
											</ItemTemplate>
											<ItemStyle HorizontalAlign="Right" />
											<FooterTemplate>
												<div style="text-align: right;">
													<asp:Label ID="lblTotal60d" runat="server" Font-Bold="true" BackColor="Gray" ForeColor="White" Width="100%" Height="100%" />
												</div>
											</FooterTemplate>
											<FooterStyle BorderStyle="Solid" HorizontalAlign="Right" VerticalAlign="Middle" />
										</asp:TemplateField>

										<asp:TemplateField HeaderText="61-90 Days">
											<ItemTemplate>
												<asp:Label ID="txt90d" runat="server" Text='<%# Bind("90d", "{0:#,##0.00;(#,##0.00)}") %>'></asp:Label>
											</ItemTemplate>
											<ItemStyle HorizontalAlign="Right" />
											<FooterTemplate>
												<div style="text-align: right;">
													<asp:Label ID="lblTotal90d" runat="server" Font-Bold="true" BackColor="Gray" ForeColor="White" Width="100%" Height="100%" />
												</div>
											</FooterTemplate>
											<FooterStyle BorderStyle="Solid" HorizontalAlign="Right" VerticalAlign="Middle" />
										</asp:TemplateField>

										<asp:TemplateField HeaderText="91-120 Days">
											<ItemTemplate>
												<asp:Label ID="txt120d" runat="server" Text='<%# Bind("120d", "{0:#,##0.00;(#,##0.00)}") %>'></asp:Label>
											</ItemTemplate>
											<ItemStyle HorizontalAlign="Right" />
											<FooterTemplate>
												<div style="text-align: right;">
													<asp:Label ID="lblTotal120d" runat="server" Font-Bold="true" BackColor="Gray" ForeColor="White" Width="100%" Height="100%" />
												</div>
											</FooterTemplate>
											<FooterStyle BorderStyle="Solid" HorizontalAlign="Right" VerticalAlign="Middle" />
										</asp:TemplateField>

										<asp:TemplateField HeaderText="121-150 Days">
											<ItemTemplate>
												<asp:Label ID="txt150d" runat="server" Text='<%# Bind("150d", "{0:#,##0.00;(#,##0.00)}") %>'></asp:Label>
											</ItemTemplate>
											<ItemStyle HorizontalAlign="Right" />
											<FooterTemplate>
												<div style="text-align: right;">
													<asp:Label ID="lblTotal150d" runat="server" Font-Bold="true" BackColor="Gray" ForeColor="White" Width="100%" Height="100%" />
												</div>
											</FooterTemplate>
											<FooterStyle BorderStyle="Solid" HorizontalAlign="Right" VerticalAlign="Middle" />
										</asp:TemplateField>

										<asp:TemplateField HeaderText="151-180 Days">
											<ItemTemplate>
												<asp:Label ID="txt180d" runat="server" Text='<%# Bind("180d", "{0:#,##0.00;(#,##0.00)}") %>'></asp:Label>
											</ItemTemplate>
											<ItemStyle HorizontalAlign="Right" />
											<FooterTemplate>
												<div style="text-align: right;">
													<asp:Label ID="lblTotal180d" runat="server" Font-Bold="true" BackColor="Gray" ForeColor="White" Width="100%" Height="100%" />
												</div>
											</FooterTemplate>
											<FooterStyle BorderStyle="Solid" HorizontalAlign="Right" VerticalAlign="Middle" />
										</asp:TemplateField>

										<asp:TemplateField HeaderText="181-360 Days">
											<ItemTemplate>
												<asp:Label ID="txt121d" runat="server" Text='<%# Bind("121d", "{0:#,##0.00;(#,##0.00)}") %>'></asp:Label>
											</ItemTemplate>
											<ItemStyle HorizontalAlign="Right" />
											<FooterTemplate>
												<div style="text-align: right;">
													<asp:Label ID="lblTotal121d" runat="server" Font-Bold="true" BackColor="Gray" ForeColor="White" Width="100%" Height="100%" />
												</div>
											</FooterTemplate>
											<FooterStyle BorderStyle="Solid" HorizontalAlign="Right" VerticalAlign="Middle" />
										</asp:TemplateField>

										<asp:TemplateField HeaderText="Bad Debt">
											<ItemTemplate>
												<asp:Label ID="txt91d" runat="server" Text='<%# Bind("91d", "{0:#,##0.00;(#,##0.00)}") %>'></asp:Label>
											</ItemTemplate>
											<ItemStyle HorizontalAlign="Right" />
											<FooterTemplate>
												<div style="text-align: right;">
													<asp:Label ID="lblTotal91d" runat="server" Font-Bold="true" BackColor="Gray" ForeColor="White" Width="100%" Height="100%" />
												</div>
											</FooterTemplate>
											<FooterStyle BorderStyle="Solid" HorizontalAlign="Right" VerticalAlign="Middle" />
										</asp:TemplateField>

									</Columns>

									<FooterStyle BackColor="Gray" ForeColor="Black" BorderStyle="Solid" />
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
									$('#<%=dgvARreport.ClientID %>').Scrollable({
													ScrollHeight: 640,
													IsInPanel: true
												});
											});
							</script>
															
						</td>
											  
					 </tr>
									   

				</table>


			</ContentTemplate>

		</ajaxToolkit:TabPanel>

		<%--SO List--%>
		<ajaxToolkit:TabPanel ID="TabPanel2" runat="server" HeaderText="Summary">
			<ContentTemplate>




			</ContentTemplate>

		</ajaxToolkit:TabPanel>

		<%--SO Monitoring--%>
		<ajaxToolkit:TabPanel ID="TabPanel3" runat="server" HeaderText="Others">
			<ContentTemplate>
			</ContentTemplate>

		</ajaxToolkit:TabPanel>

	</ajaxToolkit:TabContainer>



</asp:Content>
