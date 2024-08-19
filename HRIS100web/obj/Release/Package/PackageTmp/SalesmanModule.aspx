<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SD.master" CodeBehind="SalesmanModule.aspx.vb" Inherits="AOS100web.SalesmanModule" %>

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
		

	<table style="width: 1550px; font-family: 'Segoe UI'; float: left; margin-top: -2px; padding-top: 0px">
		<tr>
			<td style="background-color: #b1bbd7; border-top: solid; border-bottom: solid; border-color: red; border-width: 2px; width: 30%; font-family: 'Segoe UI'; font-size: 14px; text-align: center; min-height: 28px; max-height: 28px; padding-bottom: 1px;">
				<asp:Label ID="lblAccess" runat="server" Text="Access TYpe" Font-Size="Medium" Font-Italic="true" ForeColor="Red"></asp:Label>&nbsp;&nbsp;&nbsp;

				<asp:Label ID="lblCform" runat="server" Text="" Font-Size="small" ForeColor="Red"></asp:Label>
			</td>

			<td style="background-color: #b1bbd7; border-top: solid; border-bottom: solid; border-color: red; border-width: 2px; width: 70%; font-family: 'Segoe UI'; font-size: 14px; text-align: left; min-height: 28px; max-height: 28px; padding-bottom: 1px;" colspan="7">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
				<asp:LinkButton ID="lbNew" runat="server" OnClick="OnConfirm" OnClientClick="Confirm()" Style="text-decoration: none; background-color: #7fc6f6; font-family: Tahoma; font-size: 14px; color: white; margin-right: 5px; padding-left: 5px; padding-right: 5px; min-height: 22px;" BorderStyle="Solid" ToolTip="Reset Fields" CssClass="StdBottomL">
					<asp:Image runat="server" imageurl="~/images/new_16.png" style="vertical-align: middle"/>&nbsp;New
				</asp:LinkButton>
				<asp:LinkButton ID="lbSave" runat="server" OnClick="lbSave_Click" Style="text-decoration: none; background-color: #7fc6f6; padding-right: 5px; font-family: Tahoma; font-size: 14px; color: white; margin-right: 5px; padding-left: 5px; min-height: 22px;" BorderStyle="Solid" ToolTip="Save" Enabled="false">
					<asp:Image runat="server" imageurl="~/images/save_orange_16.png" style="vertical-align: middle"/>&nbsp;Save
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
			<td style="border: 1px solid #000000; width: auto; background-color: lightgray;" colspan="8"></td>
		</tr>

		<tr>
			<td style="border: 1px solid #000000; width: 50%; height: 30px; background-color: lightgray;" colspan="4">&nbsp;&nbsp;Salesman:&nbsp;&nbsp;
				<asp:Label ID="lblSmnName" runat="server" Text="Salesman's Name" Font-Size="Larger" Font-Italic="true" ForeColor="Red" Visible="false"></asp:Label>
				<asp:DropDownList ID="cboSmname" runat="server" AutoPostBack="true" Width="350px" Height="22px"
					Font-Names="Segoe UI" Font-Size="small" Visible="false">
				</asp:DropDownList>
			</td>
			<td style="border: 1px solid #000000; width: 50%; height: 30px; background-color: lightgray;" colspan="4" rowspan="3">&nbsp;&nbsp;
				<asp:Label ID="lblMsg" runat="server" Text="Message Box" Font-Size="Medium" Font-Italic="true" ForeColor="Red"></asp:Label>
			</td>
		</tr>

		<tr>
			<td style="border: 1px solid #000000; width: auto; height: 30px; background-color: lightgray;" colspan="3">&nbsp;&nbsp;Sales Area:&nbsp;&nbsp;
				<asp:Label ID="lblArea" runat="server" Text="Sales Area" Font-Size="Medium" Font-Italic="true" ForeColor="Red"></asp:Label>

			</td>
		</tr>

		<tr>
			<td style="border: 1px solid #000000; width: auto; height: 30px; background-color: lightgray;" colspan="3">&nbsp;&nbsp;Area Coverage:&nbsp;&nbsp;
				<asp:Label ID="lblAreaCov" runat="server" Text="Coverage" Font-Size="Small" Font-Italic="true" ForeColor="Red"></asp:Label>

			</td>
		</tr>

	</table>

	<ajaxToolkit:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" Style="width: 1555px; height: auto;">
		<ajaxToolkit:TabPanel ID="TabPanel1" runat="server" HeaderText="SO Monitoring">
			<ContentTemplate>
				<table style="width: 100%; font-family: 'Segoe UI'; font-size: small; float: left; background-color: lightgrey; border-spacing: 0px; margin-left: 0px; padding-left: 0px; padding-right: 0px;">

					<%--line1--%>
					<tr style="height: 26px; background-color: lightgray">
						<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:Label ID="Label18" runat="server" Text="Date From:"></asp:Label>&nbsp;&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: 124px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:TextBox ID="dpFrom" runat="server" TextMode="Date" Height="20px" Font-Names="Segoe UI" Font-Size="small" AutoPostBack="true" CssClass="DateBox" Width="120px"></asp:TextBox>
						</td>

						<td style="width: 2px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>

						<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:Label ID="Label19" runat="server" Text="Date To:"></asp:Label>&nbsp;&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: auto; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:TextBox ID="dpTo" runat="server" TextMode="Date" Height="20px" Font-Names="Segoe UI" Font-Size="small" AutoPostBack="true" CssClass="DateBox" Width="120px"></asp:TextBox>
						</td>

						<td style="width: 2px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>

						<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Customer:&nbsp;&nbsp; 
							
						</td>
						<td style="border: 1px solid #000000; width: 600px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:DropDownList ID="cboCustomer" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" Width="98%">
							</asp:DropDownList>
						</td>
						<%--<td style="width: 2px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>--%>

						<td style="width: 380px; background-color: whitesmoke; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">&nbsp;&nbsp;Option:&nbsp;&nbsp;
							<asp:RadioButton ID="RadioButton1" runat="server" GroupName="Optn" AutoPostBack="true" Text="All" />
							&nbsp;&nbsp;
							<asp:RadioButton ID="RadioButton2" runat="server" GroupName="Optn" AutoPostBack="true" Text="Open" />
							&nbsp;&nbsp;
							<asp:RadioButton ID="RadioButton3" runat="server" GroupName="Optn" AutoPostBack="true" Text="Partial" />
							&nbsp;&nbsp;
							<asp:RadioButton ID="RadioButton4" runat="server" GroupName="Optn" AutoPostBack="true" Text="Served" />
							&nbsp;&nbsp;
						</td>

					</tr>

					<tr style="height: auto; background-color: lightgray">
						<td style="width: auto; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="10"></td>
					</tr>

					<tr style="height: auto; background-color: lightgray">
						<td style="width: auto; background-color: floralwhite; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="10"></td>
					</tr>
					<tr style="height: 406px; background-color: lightgray">
						<td style="border: 1px solid #000000; height: auto; width: auto; background-color: floralwhite; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="10">

							<asp:Panel ID="Panel1" runat="server" Width="100%" Height="381px">
								<asp:GridView ID="dgvSOmon" runat="server" AutoGenerateColumns="False" OnRowDataBound="dgvSOmon_RowDataBound"
									OnRowCreated="dgvSOmon_RowCreated" Font-Names="Segoe UI" Font-Size="small" BackColor="White" BorderColor="#999999" BorderStyle="Solid" AllowSorting="true" BorderWidth="1px" CellPadding="3" GridLines="Vertical" HeaderStyle-BorderColor="Black" ShowHeader="true" HeaderStyle-HorizontalAlign="Center">
									<AlternatingRowStyle BackColor="#DCDCDC" />
									<Columns>
										<asp:CommandField ShowSelectButton="true">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="72px"></ItemStyle>
										</asp:CommandField>
										<asp:BoundField DataField="sono">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="110px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="transdate" DataFormatString="{0:yyyy-MM-dd}">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="110px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="custno">
											<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="400px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="shipto">
											<ItemStyle HorizontalAlign="left" VerticalAlign="Middle" Width="400px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="amt" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="110px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="stat">
											<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="100px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="delstat">
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
							<script src="Scripts/ScrollableGridViewPlugin_ASP.NetAJAXmin.js" type="text/javascript"></script>
							<script type="text/javascript">
								$(document).ready(function () {
									$('#<%=dgvSOmon.ClientID %>').Scrollable({
										ScrollHeight: 367,
										IsInPanel: true
									});
								});
							</script>

						</td>
					</tr>

					<tr style="height: auto; background-color: lightgray">
						<td style="width: auto; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="10"></td>
					</tr>

					<tr style="height: 200px; background-color: lightgray">
						<td style="border: 1px solid #000000; height: auto; width: auto; background-color: floralwhite; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="10">
							<asp:Panel ID="Panel2" runat="server" Width="100%" Height="170px">
								<asp:GridView ID="dgvSOdet" runat="server" AutoGenerateColumns="False" OnRowDataBound="dgvSOdet_RowDataBound"
									OnRowCreated="dgvSOdet_RowCreated" Font-Names="Segoe UI" Font-Size="small" BackColor="White" BorderColor="#999999" BorderStyle="Solid" AllowSorting="true" BorderWidth="1px" CellPadding="3" GridLines="Vertical" HeaderStyle-BorderColor="Black" ShowHeader="true" HeaderStyle-HorizontalAlign="Center">
									<AlternatingRowStyle BackColor="#DCDCDC" />
									<Columns>
										<asp:BoundField DataField="codeno">
											<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="120px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="mmdesc">
											<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="500px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="qty" DataFormatString="{0:#,##0;(#,##0)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="110px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="wt" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="110px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="doqty" DataFormatString="{0:#,##0;(#,##0)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="110px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="dowt" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="110px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="qtybal" DataFormatString="{0:#,##0;(#,##0)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="110px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="wtbal" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="110px"></ItemStyle>
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
									$('#<%=dgvSOdet.ClientID %>').Scrollable({
										ScrollHeight: 157,
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
		<ajaxToolkit:TabPanel ID="TabPanel2" runat="server" HeaderText="Sales">
			<ContentTemplate>
				<table style="width: 100%; font-family: 'Segoe UI'; font-size: small; float: left; background-color: lightgrey; border-spacing: 0px; margin-left: 0px; padding-left: 0px; padding-right: 0px;">
					<tr style="height: 26px; background-color: lightgray">
						<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:Label ID="Label1" runat="server" Text="Date From:"></asp:Label>&nbsp;&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: 124px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:TextBox ID="dpSalesDate1" runat="server" TextMode="Date" Height="20px" Font-Names="Segoe UI" Font-Size="small" AutoPostBack="true" CssClass="DateBox" Width="120px"></asp:TextBox>
						</td>

						<td style="width: 2px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>

						<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:Label ID="Label2" runat="server" Text="Date To:"></asp:Label>&nbsp;&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: auto; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:TextBox ID="dpSalesDate2" runat="server" TextMode="Date" Height="20px" Font-Names="Segoe UI" Font-Size="small" AutoPostBack="true" CssClass="DateBox" Width="120px"></asp:TextBox>
						</td>

						<td style="width: 2px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>

						<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Customer:&nbsp;&nbsp; 
							
						</td>
						<td style="border: 1px solid #000000; width: 600px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:DropDownList ID="cboSalesCust" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" Width="98%">
							</asp:DropDownList>
						</td>
						<%--<td style="width: 2px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>--%>

						<td style="width: 380px; background-color: white; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">&nbsp;&nbsp;&nbsp;&nbsp;
							
						</td>

					</tr>

					<tr style="height: auto; background-color: white">
						<td style="width: auto; background-color: white; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="10"></td>
					</tr>

				</table>

				<ajaxToolkit:TabContainer ID="TabContainer2" runat="server" ActiveTabIndex="0" Style="width: 100%; height: auto;">
					<ajaxToolkit:TabPanel ID="TabPanel7" runat="server" HeaderText="Invoices">
						<ContentTemplate>
							<table style="width: 100%; font-family: 'Segoe UI'; font-size: small; float: left; background-color: lightgrey; border-spacing: 0px; margin-left: 0px; padding-left: 0px; padding-right: 0px;">
								<%--datagrid--%>
								<tr style="height: 537px; background-color: white">
									<td style="border: 1px solid #000000; width: 100%; background-color: floralwhite; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="10">

										<asp:Panel ID="Panel3" runat="server" Width="100%" Height="527px">
											<asp:GridView ID="dgvInvoice" runat="server" AutoGenerateColumns="False" OnRowDataBound="dgvInvoice_RowDataBound" OnRowCreated="dgvInvoice_RowCreated" Font-Names="Segoe UI" Font-Size="small" BackColor="White" BorderColor="#999999" BorderStyle="Solid" AllowSorting="true" BorderWidth="1px" CellPadding="3" GridLines="Vertical" HeaderStyle-BorderColor="Black" ShowHeader="true" HeaderStyle-HorizontalAlign="Center">
												<AlternatingRowStyle BackColor="#DCDCDC" />
												<Columns>
													<asp:BoundField DataField="invno">
														<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="110px"></ItemStyle>
													</asp:BoundField>

													<asp:BoundField DataField="tc">
														<ItemStyle HorizontalAlign="center" VerticalAlign="Middle" Width="50px"></ItemStyle>
													</asp:BoundField>

													<asp:BoundField DataField="docno">
														<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="110px"></ItemStyle>
													</asp:BoundField>

													<asp:BoundField DataField="transdate" DataFormatString="{0:yyyy-MM-dd}">
														<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="110px"></ItemStyle>
													</asp:BoundField>

													<asp:BoundField DataField="custno">
														<ItemStyle HorizontalAlign="left" VerticalAlign="Middle" Width="402px"></ItemStyle>
													</asp:BoundField>

													<asp:BoundField DataField="shipto">
														<ItemStyle HorizontalAlign="left" VerticalAlign="Middle" Width="402px"></ItemStyle>
													</asp:BoundField>

													<asp:BoundField DataField="amt" DataFormatString="{0:#,##0.00;(#,##0.00)}">
														<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="138px"></ItemStyle>
													</asp:BoundField>

													<asp:BoundField DataField="stat">
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
										<script src="Scripts/ScrollableGridViewPlugin_ASP.NetAJAXmin.js" type="text/javascript"></script>
										<script type="text/javascript">
											$(document).ready(function () {
												$('#<%=dgvInvoice.ClientID %>').Scrollable({
													ScrollHeight: 500,
													IsInPanel: true
												});
											});
										</script>

									</td>
								</tr>

								<%--total--%>
								<tr style="height: 26px; background-color: lightgray">
									<td style="border: 1px solid #000000; width: 1100px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Total Sales&nbsp;&nbsp;</td>
									<td style="border: 1px solid #000000; width: 130px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
										<asp:Label ID="lblSalesTotal" runat="server" Text="0.00" Font-Bold="true" ForeColor="Red"></asp:Label>&nbsp;&nbsp;

									</td>
									<td style="border: 1px solid #000000; width: 130px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
								</tr>

							</table>

						</ContentTemplate>

					</ajaxToolkit:TabPanel>

					<ajaxToolkit:TabPanel ID="TabPanel8" runat="server" HeaderText="Product Summary With ASP">
						<ContentTemplate>
							<table style="width: 1240px; font-family: 'Segoe UI'; font-size: small; float: left; background-color: lightgrey; border-spacing: 0px; margin-left: 0px; padding-left: 0px; padding-right: 0px;">
								<%--datagrid--%>
								<tr style="height: 537px; background-color: white">
									<td style="border: 1px solid #000000; width: 100%; background-color: floralwhite; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="10">

										<asp:Panel ID="Panel5" runat="server" Width="100%" Height="527px">
											<asp:GridView ID="dgvSalesSum" runat="server" AutoGenerateColumns="False" OnRowDataBound="dgvSalesSum_RowDataBound" OnRowCreated="dgvSalesSum_RowCreated" Font-Names="Segoe UI" Font-Size="small" BackColor="White" BorderColor="#999999" BorderStyle="Solid" AllowSorting="true" BorderWidth="1px" CellPadding="3" GridLines="Vertical" HeaderStyle-BorderColor="Black" ShowHeader="true" HeaderStyle-HorizontalAlign="Center">
												<AlternatingRowStyle BackColor="#DCDCDC" />
												<Columns>
													<asp:BoundField DataField="codeno">
														<ItemStyle HorizontalAlign="left" VerticalAlign="Middle" Width="110px"></ItemStyle>
													</asp:BoundField>

													<asp:BoundField DataField="mmdesc">
														<ItemStyle HorizontalAlign="left" VerticalAlign="Middle" Width="512px"></ItemStyle>
													</asp:BoundField>

													<asp:BoundField DataField="qty" DataFormatString="{0:#,##0;(#,##0)}">
														<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="143px"></ItemStyle>
													</asp:BoundField>

													<asp:BoundField DataField="wt" DataFormatString="{0:#,##0.00;(#,##0.00)}">
														<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="143px"></ItemStyle>
													</asp:BoundField>

													<asp:BoundField DataField="amt" DataFormatString="{0:#,##0.00;(#,##0.00)}">
														<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="143px"></ItemStyle>
													</asp:BoundField>

													<asp:BoundField DataField="asp" DataFormatString="{0:#,##0.00;(#,##0.00)}">
														<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="118px"></ItemStyle>
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
												$('#<%=dgvSalesSum.ClientID %>').Scrollable({
													ScrollHeight: 508,
													IsInPanel: true
												});
											});
										</script>

									</td>
								</tr>

								<%--total--%>
								<tr style="height: 26px; background-color: lightgray">
									<td style="border: 1px solid #000000; width: 550px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Total Sales&nbsp;&nbsp;</td>

									<td style="border: 1px solid #000000; width: 130px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
										<asp:Label ID="lblASPqty" runat="server" Text="0.00" Font-Bold="true" ForeColor="Red"></asp:Label>&nbsp;&nbsp;
									</td>

									<td style="border: 1px solid #000000; width: 130px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
										<asp:Label ID="lblASPwt" runat="server" Text="0.00" Font-Bold="true" ForeColor="Red"></asp:Label>&nbsp;&nbsp;
									</td>

									<td style="border: 1px solid #000000; width: 130px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
										<asp:Label ID="lblASPamt" runat="server" Text="0.00" Font-Bold="true" ForeColor="Red"></asp:Label>&nbsp;&nbsp;
									</td>

									<td style="border: 1px solid #000000; width: 130px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>

								</tr>

							</table>

						</ContentTemplate>

					</ajaxToolkit:TabPanel>

					<%--<ajaxToolkit:TabPanel ID="TabPanel9" runat="server" HeaderText="Summary Product">
						<ContentTemplate>
							



						</ContentTemplate>

					</ajaxToolkit:TabPanel>--%>
				</ajaxToolkit:TabContainer>

			</ContentTemplate>

		</ajaxToolkit:TabPanel>

		<%--SO Monitoring--%>
		<ajaxToolkit:TabPanel ID="TabPanel3" runat="server" HeaderText="Collections">
			<ContentTemplate>
				<table style="width: 100%; font-family: 'Segoe UI'; font-size: small; float: left; background-color: lightgrey; border-spacing: 0px; margin-left: 0px; padding-left: 0px; padding-right: 0px;">
					<tr style="height: 26px; background-color: lightgray">
						<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:Label ID="Label4" runat="server" Text="Date From:"></asp:Label>&nbsp;&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: 124px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:TextBox ID="dpColDate1" runat="server" TextMode="Date" Height="20px" Font-Names="Segoe UI" Font-Size="small" AutoPostBack="true" CssClass="DateBox" Width="120px"></asp:TextBox>
						</td>

						<td style="width: 2px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>

						<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:Label ID="Label5" runat="server" Text="Date To:"></asp:Label>&nbsp;&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: 124px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:TextBox ID="dpColDate2" runat="server" TextMode="Date" Height="20px" Font-Names="Segoe UI" Font-Size="small" AutoPostBack="true" CssClass="DateBox" Width="120px"></asp:TextBox>
						</td>

						<td style="width: 2px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>

						<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Customer:&nbsp;&nbsp; 
							
						</td>
						<td style="border: 1px solid #000000; width: 600px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:DropDownList ID="cboColCust" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" Width="98%">
							</asp:DropDownList>
						</td>

						<td style="width: 380px; background-color: white; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="2">&nbsp;&nbsp;&nbsp;&nbsp;
							
						</td>

					</tr>

					<tr style="height: auto; background-color: white">
						<td style="width: auto; background-color: white; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="10"></td>
					</tr>

					<%--datagrid--%>
					<tr style="height: 584px; background-color: white">
						<td style="border: 1px solid #000000; width: 100%; background-color: floralwhite; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="10">

							<asp:Panel ID="Panel4" runat="server" Width="100%" Height="577px">
								<asp:GridView ID="dgvCollection" runat="server" AutoGenerateColumns="False" OnRowDataBound="dgvCollection_RowDataBound" OnRowCreated="dgvCollection_RowCreated" Font-Names="Segoe UI" Font-Size="small" BackColor="White" BorderColor="#999999" BorderStyle="Solid" AllowSorting="true" BorderWidth="1px" CellPadding="3" GridLines="Vertical" HeaderStyle-BorderColor="Black" ShowHeader="true" HeaderStyle-HorizontalAlign="Center">
									<AlternatingRowStyle BackColor="#DCDCDC" />
									<Columns>
										<asp:BoundField DataField="orno">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="110px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="doctype">
											<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="80px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="docno">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="110px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="transdate" DataFormatString="{0:yyyy-MM-dd}">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="110px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="custno">
											<ItemStyle HorizontalAlign="left" VerticalAlign="Middle" Width="450px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="invno">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="110px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="tc">
											<ItemStyle HorizontalAlign="center" VerticalAlign="Middle" Width="50px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="sino">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="103px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="amt" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="158px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="coltype">
											<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="110px"></ItemStyle>
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
									$('#<%=dgvCollection.ClientID %>').Scrollable({
										ScrollHeight: 555,
										IsInPanel: true
									});
								});
							</script>

						</td>
					</tr>

					<%--total--%>
					<tr style="height: 26px; background-color: lightgray">
						<td style="border: 1px solid #000000; width: 1255px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="8">Total Collection&nbsp;&nbsp;</td>
						<td style="border: 1px solid #000000; width: 130px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:Label ID="lblTotCol" runat="server" Text="0.00" Font-Bold="true" ForeColor="Red"></asp:Label>&nbsp;&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: 150px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
					</tr>
				</table>

			</ContentTemplate>

		</ajaxToolkit:TabPanel>

		<ajaxToolkit:TabPanel ID="TabPanel4" runat="server" HeaderText="Account Receivables">
			<ContentTemplate>
				<table style="width: 100%; font-family: 'Segoe UI'; font-size: small; float: left; background-color: lightgrey; border-spacing: 0px; margin-left: 0px; padding-left: 0px; padding-right: 0px;">
					<tr style="height: 26px; background-color: lightgray">
						<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:Label ID="Label3" runat="server" Text="As Of:"></asp:Label>&nbsp;&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: 124px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:TextBox ID="dpARdate" runat="server" TextMode="Date" Height="20px" Font-Names="Segoe UI" Font-Size="small" AutoPostBack="true" CssClass="DateBox" Width="120px"></asp:TextBox>
						</td>


						<td style="width: 2px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>

						<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Customer:&nbsp;&nbsp; 
							
						</td>
						<td style="border: 1px solid #000000; width: 600px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:DropDownList ID="cboARcust" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" Width="98%">
							</asp:DropDownList>

						</td>

						<td style="width: 380px; background-color: white; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="4">&nbsp;&nbsp;
							<asp:LinkButton ID="lbExcelAR" runat="server" OnClick="ExportToExcelAR" Style="text-decoration: none; background-color: #7fc6f6; font-family: Tahoma; font-size: 12px; color: white; min-height: 22px; padding-left: 5px;"
								BorderStyle="Solid" BorderColor="Blue" ToolTip="Export To Excel" >
							<asp:Image runat="server" imageurl="~/images/excel-logo_24.png" style="vertical-align: middle"/>&nbsp;Export&nbsp;
							</asp:LinkButton>
							<%--A/R Option:
							&nbsp;&nbsp;
							<asp:RadioButton ID="RadioButton5" runat="server" GroupName="AROptn" AutoPostBack="true" Text="Aging" />
							&nbsp;&nbsp;
							<asp:RadioButton ID="RadioButton6" runat="server" GroupName="AROptn" AutoPostBack="true" Text="A/R Details" />--%>
						</td>

					</tr>

					<tr style="height: auto; background-color: white">
						<td style="width: auto; background-color: white; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="10"></td>
					</tr>


					<%--datagrid--%>
				</table>

				<ajaxToolkit:TabContainer ID="TabContainer3" runat="server" ActiveTabIndex="0" Style="width: 100%; height: auto; max-width: 1550px">
					<ajaxToolkit:TabPanel ID="TabPanel9" runat="server" HeaderText="Aging">
						<ContentTemplate>
							<table style="width: 100%; max-width: 1550px;font-family: 'Segoe UI'; font-size: small; float: left; background-color: lightgrey; border-spacing: 0px; margin-left: 0px; padding-left: 0px; padding-right: 0px;">
								<tr style="height: 563px; background-color: white">
									<td style="border: 1px solid #000000; width: 100%; background-color: floralwhite; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="10">
										<asp:Panel ID="Panel6" runat="server" Width="99%" Height="554px">
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
														<ItemStyle HorizontalAlign="Center">
														</ItemStyle>
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
													ScrollHeight: 505,
													IsInPanel: true
												});
											});
										</script>

									</td>
								</tr>

							</table>

						</ContentTemplate>

					</ajaxToolkit:TabPanel>

					<ajaxToolkit:TabPanel ID="TabPanel10" runat="server" HeaderText="A/R List">
						<ContentTemplate>
							<table style="width: 100%; font-family: 'Segoe UI'; font-size: small; float: left; background-color: lightgrey; border-spacing: 0px; margin-left: 0px; padding-left: 0px; padding-right: 0px;">
								<tr style="height: 537px; background-color: white">
									<td style="border: 1px solid #000000; width: 100%; background-color: floralwhite; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="10">

										<asp:Panel ID="Panel7" runat="server" Width="99%" Height="525px">
											<asp:GridView ID="dgvARsummary" runat="server" AutoGenerateColumns="False" OnRowDataBound="dgvARsummary_RowDataBound" OnRowCreated="dgvARsummary_RowCreated" Font-Names="Segoe UI" Font-Size="Smaller" BackColor="White" BorderColor="#999999" BorderStyle="Solid" AllowSorting="true" BorderWidth="1px" CellPadding="3" GridLines="Vertical" HeaderStyle-BorderColor="Black" ShowHeader="true" ShowFooter="false" HeaderStyle-HorizontalAlign="Center">

												<AlternatingRowStyle BackColor="#DCDCDC" />
												<Columns>
													<asp:BoundField DataField="custno">
														<ItemStyle HorizontalAlign="left" VerticalAlign="Middle" Width="400px"></ItemStyle>
													</asp:BoundField>

													<asp:BoundField DataField="shipto">
														<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="400px"></ItemStyle>
													</asp:BoundField>

													<asp:BoundField DataField="invno">
														<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="110px"></ItemStyle>
													</asp:BoundField>

													<asp:BoundField DataField="tc">
														<ItemStyle HorizontalAlign="center" VerticalAlign="Middle" Width="60px"></ItemStyle>
													</asp:BoundField>

													<asp:BoundField DataField="dsrno">
														<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="110px"></ItemStyle>
													</asp:BoundField>

													<asp:BoundField DataField="transdate" DataFormatString="{0:yyyy-MM-dd}">
														<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="110px"></ItemStyle>
													</asp:BoundField>

													<asp:BoundField DataField="term">
														<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="80px"></ItemStyle>
													</asp:BoundField>

													<asp:BoundField DataField="datedue" DataFormatString="{0:yyyy-MM-dd}">
														<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="110px"></ItemStyle>
													</asp:BoundField>
																								
													<asp:BoundField DataField="invamt" DataFormatString="{0:#,##0.00;(#,##0.00)}">
														<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="158px"></ItemStyle>
													</asp:BoundField>

													<asp:BoundField DataField="dep" DataFormatString="{0:#,##0.00;(#,##0.00)}">
														<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="158px"></ItemStyle>
													</asp:BoundField>

													<asp:BoundField DataField="net" DataFormatString="{0:#,##0.00;(#,##0.00)}">
														<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="158px"></ItemStyle>
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
												$('#<%=dgvARsummary.ClientID %>').Scrollable({
										ScrollHeight: 505,
										IsInPanel: true
									});
								});
										</script>

									</td>
								</tr>

								<%--Total--%>
								<tr style="height: 26px; background-color: lightgray">
									<td style="border: 1px solid #000000; width: 1130px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="7">Total A/R&nbsp;&nbsp;</td>
									<td style="border: 1px solid #000000; width: 130px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
										<asp:Label ID="lblARdrAmt" runat="server" Text="0.00" Font-Bold="true" ForeColor="Red"></asp:Label>&nbsp;&nbsp;
									</td>
									<td style="border: 1px solid #000000; width: 133px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
										<asp:Label ID="lblARcrAmt" runat="server" Text="0.00" Font-Bold="true" ForeColor="Red"></asp:Label>&nbsp;&nbsp;
									</td>
									<td style="border: 1px solid #000000; width: 152px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
										<asp:Label ID="lblARbalAmt" runat="server" Text="0.00" Font-Bold="true" ForeColor="Red"></asp:Label>&nbsp;&nbsp;
									</td>
									
								</tr>


							</table>

						</ContentTemplate>

					</ajaxToolkit:TabPanel>

					<ajaxToolkit:TabPanel ID="TabPanel11" runat="server" HeaderText="Summary">
						<ContentTemplate>
							<table style="width: 100%; font-family: 'Segoe UI'; font-size: small; float: left; background-color: lightgrey; border-spacing: 0px; margin-left: 0px; padding-left: 0px; padding-right: 0px;">
								<tr style="height: 537px; background-color: white">
									<td style="border: 1px solid #000000; width: 100%; background-color: floralwhite; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="10">

										<asp:Panel ID="Panel10" runat="server" Width="99%" Height="525px">
											<asp:GridView ID="dgvARsum" runat="server" AutoGenerateColumns="False" OnRowDataBound="dgvARsum_RowDataBound" OnRowCreated="dgvARsum_RowCreated" Font-Names="Segoe UI" Font-Size="Smaller" BackColor="White" BorderColor="#999999" BorderStyle="Solid" AllowSorting="true" BorderWidth="1px" CellPadding="3" GridLines="Vertical" HeaderStyle-BorderColor="Black" ShowHeader="true" ShowFooter="false" HeaderStyle-HorizontalAlign="Center">

												<AlternatingRowStyle BackColor="#DCDCDC" />
												<Columns>
													<asp:BoundField DataField="custno">
														<ItemStyle HorizontalAlign="left" VerticalAlign="Middle" Width="650px"></ItemStyle>
													</asp:BoundField>

													<asp:BoundField DataField="shipto">
														<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="650px"></ItemStyle>
													</asp:BoundField>
																								
													<asp:BoundField DataField="invamt" DataFormatString="{0:#,##0.00;(#,##0.00)}">
														<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="130px"></ItemStyle>
													</asp:BoundField>

													<asp:BoundField DataField="dep" DataFormatString="{0:#,##0.00;(#,##0.00)}">
														<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="133px"></ItemStyle>
													</asp:BoundField>

													<asp:BoundField DataField="net" DataFormatString="{0:#,##0.00;(#,##0.00)}">
														<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="158px"></ItemStyle>
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
												$('#<%=dgvARsum.ClientID %>').Scrollable({
													ScrollHeight: 505,
													IsInPanel: true
												});
											});
										</script>

									</td>
								</tr>

								<%--Total--%>
								<tr style="height: 26px; background-color: lightgray">
									<td style="border: 1px solid #000000; width: 1130px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="7">Total A/R&nbsp;&nbsp;</td>
									<td style="border: 1px solid #000000; width: 130px; max-width: 130px;background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
										<asp:Label ID="lblSumARdr" runat="server" Text="0.00" Font-Bold="true" ForeColor="Red"></asp:Label>&nbsp;&nbsp;
									</td>
									<td style="border: 1px solid #000000; width: 133px; max-width: 130px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
										<asp:Label ID="lblSumARcr" runat="server" Text="0.00" Font-Bold="true" ForeColor="Red"></asp:Label>&nbsp;&nbsp;
									</td>
									<td style="border: 1px solid #000000; width: 152px; max-width: 152px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
										<asp:Label ID="lblSumARbal" runat="server" Text="0.00" Font-Bold="true" ForeColor="Red"></asp:Label>&nbsp;&nbsp;
									</td>
									
								</tr>


							</table>

						</ContentTemplate>
					</ajaxToolkit:TabPanel>

				</ajaxToolkit:TabContainer>



				<%--A/R Summary Option--%>
			</ContentTemplate>

		</ajaxToolkit:TabPanel>

		<ajaxToolkit:TabPanel ID="TabPanel5" runat="server" HeaderText="Request For SO" visible="false">
			<ContentTemplate>
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
							<%--<asp:LinkButton ID="lbReLoadSO" runat="server" Style="text-decoration: none; background-color: lightgray; float: left; padding-right: 2px;">&nbsp;
					<asp:Image runat="server" imageurl="~/images/check_16.png" style="vertical-align: middle"/>
							</asp:LinkButton>--%>
							Req No.:&nbsp;&nbsp;
						</td>

						<td style="border: 1px solid #000000; width: 120px; background-color: lightgray; text-align: left; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">
							<asp:TextBox ID="txtSOno" runat="server" Font-Names="Segoe UI" Font-Size="small" ReadOnly="true" AutoPostBack="true" CssClass="TxtBox"></asp:TextBox>
						</td>
						<td style="border: 1px solid #000000; width: 80px; background-color: lightgray; text-align: right; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">Salesman:&nbsp;&nbsp;
						</td>

						<td style="border: 1px solid #000000; width: 250px; background-color: oldlace; text-align: left; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">&nbsp;&nbsp;
							<asp:DropDownList ID="cboSmnName" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" CssClass="cboBox" Visible="false">
							</asp:DropDownList>
							<asp:Label ID="lblSmnNo" runat="server" Text="Smn No" Font-Size="small" Visible="false"></asp:Label>
						</td>

						<td style="border: 1px solid #000000; width: auto; background-color: lightgray; text-align: center; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;" colspan="2">
							<asp:CheckBox ID="CheckBox1" runat="server" Text="Reload Only" />
						</td>
						<td style="border: 1px solid #000000; width: auto; background-color: lightblue; text-align: center; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;" colspan="2" rowspan="2">
							<asp:Label ID="lblSOStat" runat="server" Text="Credit Status" ForeColor="Red" Font-Size="Large" Font-Bold="true"></asp:Label>
						</td>

						<td style="border: 1px solid #000000; width: auto; background-color: khaki; text-align: center; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;" colspan="2" rowspan="2">Error Message <
							<asp:Label ID="lblErrMsg" runat="server" Text="No Error" Font-Size="Medium"></asp:Label>
							>
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

						<td style="border: 1px solid #000000; width: 330px; background-color: lightgray; text-align: right; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;"colspan="2"><%--Plant:&nbsp;&nbsp;--%>
						</td>

						<%--<td style="border: 1px solid #000000; width: 250px; background-color: lightgray; text-align: left; padding-left: 1px; font-family: 'Segoe UI'; font-size: small;">
							<asp:DropDownList ID="cboPlnt" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" CssClass="cboBox" Visible="false">
							</asp:DropDownList>
						</td>--%>

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
							<asp:TextBox ID="txtSearch" runat="server" Width="250" Font-Names="Segoe UI" placeholder="Search Customer" Font-Size="Small" TextMode="Search" AutoPostBack="true"></asp:TextBox>
							<asp:LinkButton ID="lbSearch" runat="server" OnClick="lbSearch_Click" Style="text-decoration: none; background-color: lightgray; float: inherit; padding-right: 2px;">
							<asp:Image runat="server" imageurl="~/images/search_red16.png" style="vertical-align: middle"/>
							</asp:LinkButton>
							&nbsp;&nbsp;&nbsp;&nbsp;Area No.:
							<asp:Label ID="lblAreaSO" runat="server" Text="000" ForeColor="Red"></asp:Label>
							&nbsp;&nbsp;&nbsp;&nbsp;Terms:
							<asp:Label ID="lblTerm" runat="server" Text="0" ForeColor="Red"> Days </asp:Label>
							&nbsp;&nbsp;&nbsp;&nbsp;Cust Type:
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
							<asp:TextBox ID="txtRemarks" runat="server" AutoPostBack="true" CssClass="txtBoxL"></asp:TextBox>
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

				<ajaxToolkit:TabContainer ID="TabContainer4" runat="server" ActiveTabIndex="0" Style="width: 100%; margin-left: 3px; height: auto;">
					<ajaxToolkit:TabPanel ID="TabPanel12" runat="server" HeaderText="Item Request">
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
							<asp:CheckBox ID="CheckBox4" runat="server" ToolTip="Check to Auto Compute Wt"></asp:CheckBox>
									</td>
									<td style="border: 1px solid #000000; width: 120px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
										<asp:CheckBox ID="CheckBox2" runat="server" ToolTip="Check To Get Last Price" Text="" />
										SP</td>
									<td style="border: 1px solid #000000; width: 120px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Amount</td>
									<td style="border: 1px solid #000000; width: 120px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
										<asp:CheckBox ID="CheckBox3" runat="server" Text="Disc" ToolTip="Check for Deal Item" AutoPostBack="true" />
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
									<td style="border: 1px solid #000000; height: 282px; background-color: floralwhite; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; vertical-align: top; margin-left: 0px" colspan="12">
										<asp:Panel ID="Panel8" runat="server" ScrollBars="Auto" CssClass="PanelStd">
											<asp:GridView ID="dgvSOreqDet" runat="server" AutoGenerateColumns="False" OnRowDataBound="dgvSOreqDet_RowDataBound" OnRowCreated="dgvSOreqDet_RowCreated" Font-Names="Segoe UI" Font-Size="Small" BackColor="White" BorderColor="#999999" BorderStyle="Solid"
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
										&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Line Item: &nbsp;&nbsp;
				<asp:Label ID="lblLineItm" runat="server" Text="New" ForeColor="Red"></asp:Label>
										&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Current Customer: &nbsp;&nbsp;
				<asp:Label ID="lblCurrCustNo" runat="server" Text="CustNo" ForeColor="Red"></asp:Label>

									</td>
								</tr>
							</table>
						</ContentTemplate>
					</ajaxToolkit:TabPanel>

		<%--SO List--%>
					<ajaxToolkit:TabPanel ID="TabPanel13" runat="server" HeaderText="Request Monitoring">
						<ContentTemplate>
							<table style="width: 100%; font-family: 'Segoe UI'; font-size: small; float: left; background-color: lightgrey; border-spacing: 0px; margin-left: 0px; padding-left: 0px; padding-right: 0px;">
								<tr style="height: 26px; background-color: lightgray">
									<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
										<asp:Label ID="Label6" runat="server" Text="Date From:"></asp:Label>&nbsp;&nbsp;
									</td>
									<td style="border: 1px solid #000000; width: 124px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
										<asp:TextBox ID="dpSOreqFr" runat="server" TextMode="Date" Height="20px" Font-Names="Segoe UI" Font-Size="small" AutoPostBack="true" CssClass="DateBox" Width="120px"></asp:TextBox>
									</td>

									<td style="width: 2px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>

									<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
										<asp:Label ID="Label7" runat="server" Text="Date To:"></asp:Label>&nbsp;&nbsp;
									</td>
									<td style="border: 1px solid #000000; width: auto; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
										<asp:TextBox ID="dpSOreqTo" runat="server" TextMode="Date" Height="20px" Font-Names="Segoe UI" Font-Size="small" AutoPostBack="true" CssClass="DateBox" Width="120px"></asp:TextBox>
									</td>

									<td style="width: 2px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>

									<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Status:&nbsp;&nbsp; 
							
									</td>
									<td style="border: 1px solid #000000; width: 200px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
										<asp:DropDownList ID="cboSOreqStat" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" Width="98%">
										</asp:DropDownList>
									</td>
									<%--<td style="width: 2px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>--%>

									<td style="width: 780px; background-color: white; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">&nbsp;&nbsp;&nbsp;&nbsp; Under Construction
							
									</td>

								</tr>

								<tr style="height: auto; background-color: white">
									<td style="width: auto; background-color: white; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="10"></td>
								</tr>

								<tr style="height: auto; background-color: lightgray">
									<td style="border: 1px solid #000000; height: 356px; background-color: floralwhite; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; vertical-align: top; margin-left: 0px" colspan="12">
										<asp:Panel ID="Panel9" runat="server" ScrollBars="Auto" CssClass="PanelStd">
											<asp:GridView ID="dgvSOreqList" runat="server" AutoGenerateColumns="False" OnRowDataBound="dgvSOreqList_RowDataBound" OnRowCreated="dgvSOreqList_RowCreated" Font-Names="Segoe UI" Font-Size="Small" BackColor="White" BorderColor="#999999" BorderStyle="Solid"
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

										<script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
										<script src="Scripts/ScrollableGridViewPlugin_ASP.NetAJAXmin.js" type="text/javascript"></script>
										<script type="text/javascript">
											$(document).ready(function () {
												$('#<%=dgvSOreqList.ClientID %>').Scrollable({
													ScrollHeight: 325,
													IsInPanel: true
												});
											});
										</script>

									</td>
								</tr>

							</table>


						</ContentTemplate>
					</ajaxToolkit:TabPanel>

		<%--SO Monitoring--%>
					<%--<ajaxToolkit:TabPanel ID="TabPanel14" runat="server" HeaderText="Req't for SO Monitoring">
						<ContentTemplate>
							<h1>Not Yet Available</h1>




						</ContentTemplate>
					</ajaxToolkit:TabPanel>--%>
				</ajaxToolkit:TabContainer>
			</ContentTemplate>
		</ajaxToolkit:TabPanel>

		<ajaxToolkit:TabPanel ID="TabPanel6" runat="server" HeaderText="Pending SO">
			<ContentTemplate>
				<table style="width: 100%; font-family: 'Segoe UI'; font-size: small; float: left; background-color: lightgrey; border-spacing: 0px; margin-left: 0px; padding-left: 0px; padding-right: 0px;">
				
					<tr style="height: 26px; background-color: lightgray">
						<td style="border: 1px solid #000000; width: 120px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:CheckBox ID="CheckBox5" runat="server" ToolTip="As Of" AutoPostBack="true" />&nbsp;&nbsp;
							<asp:Label ID="lblSODateLabel" runat="server" Text="Date From:"></asp:Label>&nbsp;&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: auto; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:TextBox ID="dpSOdate1" runat="server" TextMode="Date" Height="20px" Font-Names="Segoe UI" Font-Size="small" AutoPostBack="true" CssClass="DateBox" Width="120px"></asp:TextBox>
						</td>

						<td style="width: 2px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>

						<td style="border: 1px solid #000000; width: 120px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Date To:&nbsp;&nbsp;</td>
						<td style="border: 1px solid #000000; width: auto; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:TextBox ID="dpSOdate2" runat="server" TextMode="Date" Height="20px" Font-Names="Segoe UI" Font-Size="small" AutoPostBack="true" CssClass="DateBox" Width="120px"></asp:TextBox>
						</td>

						<td style="width: 2px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>

						<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Prod Class:&nbsp;&nbsp;
							
						</td>
						<td style="border: 1px solid #000000; width: 150px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:DropDownList ID="cboPClassSO" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" Width="98%">
							</asp:DropDownList>
						</td>

						<td style="width: 2px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>

						<td style="border: 1px solid #000000; width: 300px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="4">
							<asp:DropDownList ID="cboMMGrpSO" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" Width="98%">
							</asp:DropDownList>

						</td>
						<td style="width: 200px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">&nbsp;&nbsp;
						</td>

						<td style="width: 200px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">&nbsp;&nbsp;
						</td>
					</tr>

					<tr style="height: 2px; background-color: whitesmoke">
						<td style="width: 100%; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="15"></td>
					</tr>

					<tr style="height: 26px; background-color: lightgray">
						<td style="border: 1px solid #000000; width: auto; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="3">
							<asp:RadioButton ID="rbMon1" runat="server" GroupName="Tab4" Text="Salesman" AutoPostBack="true"/>
							&nbsp;&nbsp;&nbsp;&nbsp;
							<asp:RadioButton ID="rbMon2" runat="server" GroupName="Tab4" Text="Customer" AutoPostBack="true"/>
						</td>

						<td style="border: 1px solid #000000; width: auto; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="5">
							<asp:DropDownList ID="cboSmnCust" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" Width="98%">
							</asp:DropDownList>
							
						</td>
											
						<td style="width: 2px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>

						<td style="width:auto; background-color: whitesmoke; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="6">&nbsp;&nbsp;
							<asp:Button ID="btnRefr" runat="server" Text="Refresh" />
						</td>

					</tr>
					<tr style="height: 2px; background-color: whitesmoke">
						<td style="width: 100%; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="15"></td>
					</tr>

					<tr style="height: auto; background-color: lightgray">
						<td style="height: auto; width: 100%; background-color: floralwhite; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="15">
							<ajaxToolkit:TabContainer ID="TabContainer5" runat="server" ActiveTabIndex="0" Style="width: 95%; height: auto;">
								<ajaxToolkit:TabPanel ID="TabPanel14" runat="server" HeaderText="Summary Per SKU">
									<ContentTemplate>
										<asp:Panel ID="Panel11" runat="server" Width="1490px" Height="526px">
											<asp:GridView ID="dgvSOmonSum" runat="server" AutoGenerateColumns="False" OnRowDataBound="dgvSOmonSum_RowDataBound" OnRowCreated="dgvSOmonSum_RowCreated" Font-Names="Segoe UI" Font-Size="small" BackColor="White" BorderColor="#999999" BorderStyle="Solid" AllowSorting="true" BorderWidth="1px" CellPadding="3" GridLines="Vertical" HeaderStyle-BorderColor="Black" ShowHeader="true" HeaderStyle-HorizontalAlign="Center">
												<AlternatingRowStyle BackColor="#DCDCDC" />
												<Columns>
													<asp:BoundField DataField="codeno" HeaderText="Code No.">
														<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="120px"></ItemStyle>
													</asp:BoundField>
													<asp:BoundField DataField="mmdesc" HeaderText="Product Description">
														<ItemStyle HorizontalAlign="left" VerticalAlign="Middle" Width="490px"></ItemStyle>
													</asp:BoundField>
													<asp:BoundField DataField="qtbal" HeaderText="Qty" DataFormatString="{0:#,##0;(#,##0)}">
														<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="115px"></ItemStyle>
													</asp:BoundField>
													<asp:BoundField DataField="wtbal" HeaderText="Wt/Vol" DataFormatString="{0:#,##0.00;(#,##0.00)}">
													<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="114px"></ItemStyle>
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
												$('#<%=dgvSOmonSum.ClientID %>').Scrollable({
													ScrollHeight: 500,
													IsInPanel: true
												});
											});
										</script>

										<table style="width: 100%; font-family: 'Segoe UI'; font-size: small; float: left; background-color: lightgrey; border-spacing: 0px; margin-left: 0px; padding-left: 0px; padding-right: 0px;">
											<tr style="height: 26px; background-color: lightgray">
												<td style="border: 1px solid #000000; width: 620px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Total
												</td>
												<td style="border: 1px solid #000000; width: 120px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
													<asp:Label ID="lblSOqtyBal" runat="server" Text="0" ForeColor="Red"></asp:Label>
												</td>
												<td style="border: 1px solid #000000; width: 120px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
													<asp:Label ID="lblSOwtBal" runat="server" Text="0.00" ForeColor="Red"></asp:Label>
												</td>
												<td style="border: 1px solid #000000; width: 18px; background-color: lightgray; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
													<td style="width:auto; background-color: white; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
											</tr>

										</table>

									</ContentTemplate>

								</ajaxToolkit:TabPanel>

								<ajaxToolkit:TabPanel ID="TabPanel15" runat="server" HeaderText="Per SO">
									<ContentTemplate>
										<asp:Panel ID="Panel12" runat="server" Width="1490px" Height="526px">
											<asp:GridView ID="dgvPerSO" runat="server" AutoGenerateColumns="False" OnRowDataBound="dgvPerSO_RowDataBound" OnRowCreated="dgvPerSO_RowCreated" Font-Names="Segoe UI" Font-Size="small" BackColor="White" BorderColor="#999999" BorderStyle="Solid" AllowSorting="true" BorderWidth="1px" CellPadding="3" GridLines="Vertical" HeaderStyle-BorderColor="Black" ShowHeader="true" HeaderStyle-HorizontalAlign="Center">
												<AlternatingRowStyle BackColor="#DCDCDC" />
												<Columns>
													<asp:BoundField DataField="sono" HeaderText="SO No.">
														<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="110px"></ItemStyle>
													</asp:BoundField>
													<asp:BoundField DataField="transdate" HeaderText="SO Date" DataFormatString="{0:yyyy-MM-dd}">
														<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="110px"></ItemStyle>
													</asp:BoundField>
													<asp:BoundField DataField="custno" HeaderText="Customer">
														<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="400px"></ItemStyle>
													</asp:BoundField>
													<asp:BoundField DataField="codeno" HeaderText="Code No.">
														<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="120px"></ItemStyle>
													</asp:BoundField>
													<asp:BoundField DataField="mmdesc" HeaderText="Product Description">
														<ItemStyle HorizontalAlign="left" VerticalAlign="Middle" Width="400px"></ItemStyle>
													</asp:BoundField>
													<asp:BoundField DataField="qtbal" HeaderText="Qty" DataFormatString="{0:#,##0;(#,##0)}">
														<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="110px"></ItemStyle>
													</asp:BoundField>
													<asp:BoundField DataField="wtbal" HeaderText="Wt/Vol" DataFormatString="{0:#,##0.00;(#,##0.00)}">
														<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="110px"></ItemStyle>
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
												$('#<%=dgvPerSO.ClientID %>').Scrollable({
													ScrollHeight: 500,
													IsInPanel: true
												});
											});
										</script>

										<table style="width: 100%; font-family: 'Segoe UI'; font-size: small; float: left; background-color: lightgrey; border-spacing: 0px; margin-left: 0px; padding-left: 0px; padding-right: 0px;">
											<tr style="height: 26px; background-color: lightgray">
												<td style="border: 1px solid #000000; width: 1170px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Total
												</td>
												<td style="border: 1px solid #000000; width: 115px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
													<asp:Label ID="lblPerSOqty" runat="server" Text="0" ForeColor="Red"></asp:Label>
												</td>
												<td style="border: 1px solid #000000; width: 115px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
													<asp:Label ID="lblPerSOwt" runat="server" Text="0.00" ForeColor="Red"></asp:Label>
												</td>
												<td style="border: 1px solid #000000; width: 18px; background-color: lightgray; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
													<td style="width: auto; background-color: white; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
											</tr>

										</table>

									</ContentTemplate>

								</ajaxToolkit:TabPanel>


							</ajaxToolkit:TabContainer>



						</td>

					</tr>


				</table>
				


			</ContentTemplate>
		</ajaxToolkit:TabPanel>
	</ajaxToolkit:TabContainer>

	
</asp:Content>
