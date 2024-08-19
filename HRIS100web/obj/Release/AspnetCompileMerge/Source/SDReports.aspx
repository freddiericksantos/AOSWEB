<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SD.master" CodeBehind="SDReports.aspx.vb" Inherits="AOS100web.SDReports" %>

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
				<asp:Label ID="lblTitle" runat="server" Text="Registers Printing" Font-Size="Larger" Font-Italic="true" ForeColor="Red">       
				</asp:Label>
			</td>
			<td style="border: 1px solid #000000; width: 600px; height: 30px; background-color: lightgray;" colspan="3">&nbsp;&nbsp;
				<asp:Label ID="lblMsg" runat="server" Text="Message Box" Font-Size="Medium" Font-Italic="true" ForeColor="Red"></asp:Label>
			</td>
		</tr>

	</table>

	<center>
		<ajaxToolkit:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" AutoPostBack="true" Style="width: 99%; height: auto;">
			<ajaxToolkit:TabPanel ID="TabPanel1" runat="server" HeaderText="Registers">
				<ContentTemplate>
					<table style="width: 100%; font-family: 'Segoe UI'; font-size: small; float: left; background-color: lightgrey; border-spacing: 0px; margin-left: 0px; padding-left: 0px; padding-right: 0px;">
						<%--L1--%>
						<tr style="height: 26px; background-color: lightgray;">
							<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								<asp:Label ID="lblDate" runat="server" Text="Date From:"></asp:Label>&nbsp;&nbsp;
							</td>
							<td style="border: 1px solid #000000; width: 110px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								<asp:TextBox ID="dpTransDate" runat="server" TextMode="Date" Height="20px" Font-Names="Segoe UI" Font-Size="small" AutoPostBack="true" CssClass="DateBox" Width="110px"></asp:TextBox>
							</td>
							<td style="width: 2px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Report Format:&nbsp;&nbsp;
							</td>
							<td style="border: 1px solid #000000; width: 400px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								<asp:DropDownList ID="cboFormat" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" Width="99%">
								</asp:DropDownList>
							</td>
							<td style="width: 2px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								<asp:Label ID="lblFilter2" runat="server" Text="Filter 2:" Visible="false"></asp:Label>&nbsp;&nbsp;
							</td>
							<td style="border: 1px solid #000000; width: 400px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								<asp:DropDownList ID="cboFilter2" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" Width="99%" Visible="false">
								</asp:DropDownList>
							</td>
							<td style="border: 1px solid #000000; width: 120px; background-color: khaki; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								<asp:Label ID="txtText1" runat="server" Text="" Visible="false"></asp:Label>

							</td>
							<td style="width: 2px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>

							<td style="width: 200px; background-color: whitesmoke; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; vertical-align: middle" rowspan="2">
								<asp:LinkButton ID="btnGenerate" runat="server" Style="text-decoration: none; background-color: #7fc6f6; padding-right: 5px; padding-top: 3px; height: 44px; font-family: Tahoma; font-size: 14px; color: white; margin-right: 5px; padding-left: 5px; vertical-align: middle;" BorderStyle="Solid" ToolTip="Process" Enabled="false" CssClass="StdButtLarge">
							<asp:Image runat="server" imageurl="~/images/Process_40px.png" style="vertical-align: middle; " AlternateText=""/>&nbsp; Process
								</asp:LinkButton>

							</td>

						</tr>
						<%--L2--%>
						<tr style="height: 26px; background-color: lightgray;">
							<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								<asp:Label ID="lblDate2" runat="server" Text="Date To:"></asp:Label>&nbsp;&nbsp;
							</td>
							<td style="border: 1px solid #000000; width: 110px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								<asp:TextBox ID="dpTransDate2" runat="server" TextMode="Date" Height="20px" Font-Names="Segoe UI" Font-Size="small" AutoPostBack="true" CssClass="DateBox" Width="110px"></asp:TextBox>
							</td>
							<td style="width: 2px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								<asp:Label ID="lblFilter1" runat="server" Text="Filter 1:"></asp:Label>&nbsp;&nbsp;
							</td>
							<td style="border: 1px solid #000000; width: 400px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								<asp:DropDownList ID="cboFilter1" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" Width="99%">
								</asp:DropDownList>
							</td>
							<td style="width: 2px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								<asp:Label ID="lblFilter3" runat="server" Text="Filter 3:" Visible="false"></asp:Label>&nbsp;&nbsp;
							</td>
							<td style="border: 1px solid #000000; width: 400px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								<asp:DropDownList ID="cboFilter3" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" Width="99%" Visible="false">
								</asp:DropDownList>
							</td>
							<td style="border: 1px solid #000000; width: 120px; background-color: khaki; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								<asp:Label ID="txtText2" runat="server" Text="" Visible="false"></asp:Label>

							</td>
							<td style="width: 2px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>


							<%--L3--%>
							<tr style="height: 2px; background-color: white;">
								<td style="width: auto; background-color: white; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="12"></td>
							</tr>
							<tr style="height: 653px; background-color: floralwhite;">
								<td style="border: 1px solid #000000; width: auto; background-color: floralwhite; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; vertical-align: top;" colspan="12">
									<%--Reg Summary--%>

									<asp:Panel ID="PregSum" runat="server" Width="99%" Height="630px" Visible="false">
										<asp:GridView ID="dgvRegSum" runat="server" AutoGenerateColumns="False" OnRowDataBound="dgvRegSum_RowDataBound"
											OnRowCreated="dgvRegSum_RowCreated" Font-Names="Segoe UI" Font-Size="small" BackColor="White" BorderColor="#999999" BorderStyle="Solid" AllowSorting="true" BorderWidth="1px" CellPadding="3" GridLines="Vertical" HeaderStyle-BorderColor="Black" ShowHeader="true" ShowFooter="true">
											<AlternatingRowStyle BackColor="#DCDCDC" />
											<Columns>
												<asp:BoundField DataField="invno">
													<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="90px"></ItemStyle>
												</asp:BoundField>

												<asp:BoundField DataField="tc">
													<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="40px"></ItemStyle>
												</asp:BoundField>

												<asp:BoundField DataField="transdate" DataFormatString="{0:yyyy-MM-dd}">
													<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="90px"></ItemStyle>
												</asp:BoundField>

												<asp:BoundField DataField="dono">
													<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="90px"></ItemStyle>
												</asp:BoundField>

												<asp:BoundField DataField="docno">
													<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="90px"></ItemStyle>
												</asp:BoundField>

												<asp:BoundField DataField="smnno">
													<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="40px"></ItemStyle>
												</asp:BoundField>

												<asp:TemplateField HeaderText="Sold To">
													<ItemTemplate>
														<asp:Label ID="Label1" runat="server" Text='<%# Bind("custno") %>'></asp:Label>
													</ItemTemplate>
													<ItemStyle HorizontalAlign="Left" Width="375px" />
													<FooterTemplate>
														<div style="text-align: center;">
															<asp:Label ID="lblGTotal" runat="server" Font-Bold="true" BackColor="#CCCCCC" ForeColor="White" Width="100%" Height="100%" />
														</div>
													</FooterTemplate>
													<FooterStyle BorderStyle="Solid" HorizontalAlign="Right" VerticalAlign="Middle" />
												</asp:TemplateField>

												<asp:BoundField DataField="shiptoname" HeaderText="Ship To Name">
													<ItemStyle HorizontalAlign="Left" Width="375px"></ItemStyle>
												</asp:BoundField>

												<asp:TemplateField HeaderText="Gross Amt">
													<ItemTemplate>
														<asp:Label ID="txtGross" runat="server" Text='<%# Bind("grossamt", "{0:#,##0.00;(#,##0.00)}") %>'></asp:Label>
													</ItemTemplate>
													<ItemStyle HorizontalAlign="Right" />
													<FooterTemplate>
														<div style="text-align: right;">
															<asp:Label ID="lblGross" runat="server" Font-Bold="true" BackColor="#CCCCCC" ForeColor="White" Width="100%" Height="100%" />
														</div>
													</FooterTemplate>
													<FooterStyle BorderStyle="Solid" HorizontalAlign="Right" VerticalAlign="Middle" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Gross Amt">
													<ItemTemplate>
														<asp:Label ID="txtVat" runat="server" Text='<%# Bind("vat", "{0:#,##0.00;(#,##0.00)}") %>'></asp:Label>
													</ItemTemplate>
													<ItemStyle HorizontalAlign="Right" />
													<FooterTemplate>
														<div style="text-align: right;">
															<asp:Label ID="lblVat" runat="server" Font-Bold="true" BackColor="#CCCCCC" ForeColor="White" Width="100%" Height="100%" />
														</div>
													</FooterTemplate>
													<FooterStyle BorderStyle="Solid" HorizontalAlign="Right" VerticalAlign="Middle" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="F & H Amt">
													<ItemTemplate>
														<asp:Label ID="txtFH" runat="server" Text='<%# Bind("fhamt", "{0:#,##0.00;(#,##0.00)}") %>'></asp:Label>
													</ItemTemplate>
													<ItemStyle HorizontalAlign="Right" />
													<FooterTemplate>
														<div style="text-align: right;">
															<asp:Label ID="lblFH" runat="server" Font-Bold="true" BackColor="#CCCCCC" ForeColor="White" Width="100%" Height="100%" />
														</div>
													</FooterTemplate>
													<FooterStyle BorderStyle="Solid" HorizontalAlign="Right" VerticalAlign="Middle" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Discount">
													<ItemTemplate>
														<asp:Label ID="txtDisc" runat="server" Text='<%# Bind("discamt", "{0:#,##0.00;(#,##0.00)}") %>'></asp:Label>
													</ItemTemplate>
													<ItemStyle HorizontalAlign="Right" />
													<FooterTemplate>
														<div style="text-align: right;">
															<asp:Label ID="lblDisc" runat="server" Font-Bold="true" BackColor="#CCCCCC" ForeColor="White" Width="100%" Height="100%" />
														</div>
													</FooterTemplate>
													<FooterStyle BorderStyle="Solid" HorizontalAlign="Right" VerticalAlign="Middle" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Net Amt">
													<ItemTemplate>
														<asp:Label ID="txtNet" runat="server" Text='<%# Bind("netamt", "{0:#,##0.00;(#,##0.00)}") %>'></asp:Label>
													</ItemTemplate>
													<ItemStyle HorizontalAlign="Right" />
													<FooterTemplate>
														<div style="text-align: right;">
															<asp:Label ID="lblNet" runat="server" Font-Bold="true" BackColor="#CCCCCC" ForeColor="White" Width="100%" Height="100%" />
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
											$('#<%=dgvRegSum.ClientID %>').Scrollable({
											ScrollHeight: 605,
											IsInPanel: true
										});
									});
									</script>

									<%--Cust Gpr--%>
									<asp:Panel ID="PregSumCust" runat="server" Width="99%" Height="630px" Visible="false">
										<asp:GridView ID="dgvRegSumCustGrp" runat="server" AutoGenerateColumns="False" OnRowDataBound="dgvRegSumCustGrp_RowDataBound" OnRowCreated="dgvRegSumCustGrp_RowCreated" Font-Names="Segoe UI" Font-Size="small" BackColor="White" BorderColor="#999999" BorderStyle="Solid" AllowSorting="true" BorderWidth="1px" CellPadding="3" GridLines="Vertical" HeaderStyle-BorderColor="Black" ShowHeader="true" ShowFooter="true">
											<AlternatingRowStyle BackColor="#DCDCDC" />
											<Columns>
												<asp:BoundField DataField="custno">
													<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="70px"></ItemStyle>
												</asp:BoundField>

												<asp:BoundField DataField="bussname">
													<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="340px"></ItemStyle>
												</asp:BoundField>

												<asp:BoundField DataField="invno">
													<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="90px"></ItemStyle>
												</asp:BoundField>

												<asp:BoundField DataField="tc">
													<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="40px"></ItemStyle>
												</asp:BoundField>

												<asp:BoundField DataField="transdate" DataFormatString="{0:yyyy-MM-dd}">
													<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="90px"></ItemStyle>
												</asp:BoundField>

												<asp:BoundField DataField="dono">
													<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="90px"></ItemStyle>
												</asp:BoundField>

												<asp:BoundField DataField="docno">
													<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="90px"></ItemStyle>
												</asp:BoundField>

												<asp:BoundField DataField="smnno">
													<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="40px"></ItemStyle>
												</asp:BoundField>

												<asp:TemplateField HeaderText="Sold To">
													<ItemTemplate>
														<asp:Label ID="Label1" runat="server" Text='<%# Bind("shiptoname") %>'></asp:Label>
													</ItemTemplate>
													<ItemStyle HorizontalAlign="Left" Width="375px" />
													<FooterTemplate>
														<div style="text-align: center;">
															<asp:Label ID="lblGTotal" runat="server" Font-Bold="true" BackColor="#CCCCCC" ForeColor="White" Width="100%" Height="100%" />
														</div>
													</FooterTemplate>
													<FooterStyle BorderStyle="Solid" HorizontalAlign="Right" VerticalAlign="Middle" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Gross Amt">
													<ItemTemplate>
														<asp:Label ID="txtGross" runat="server" Text='<%# Bind("grossamt", "{0:#,##0.00;(#,##0.00)}") %>'></asp:Label>
													</ItemTemplate>
													<ItemStyle HorizontalAlign="Right" />
													<FooterTemplate>
														<div style="text-align: right;">
															<asp:Label ID="lblGross" runat="server" Font-Bold="true" BackColor="#CCCCCC" ForeColor="White" Width="100%" Height="100%" />
														</div>
													</FooterTemplate>
													<FooterStyle BorderStyle="Solid" HorizontalAlign="Right" VerticalAlign="Middle" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Gross Amt">
													<ItemTemplate>
														<asp:Label ID="txtVat" runat="server" Text='<%# Bind("vat", "{0:#,##0.00;(#,##0.00)}") %>'></asp:Label>
													</ItemTemplate>
													<ItemStyle HorizontalAlign="Right" />
													<FooterTemplate>
														<div style="text-align: right;">
															<asp:Label ID="lblVat" runat="server" Font-Bold="true" BackColor="#CCCCCC" ForeColor="White" Width="100%" Height="100%" />
														</div>
													</FooterTemplate>
													<FooterStyle BorderStyle="Solid" HorizontalAlign="Right" VerticalAlign="Middle" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="F & H Amt">
													<ItemTemplate>
														<asp:Label ID="txtFH" runat="server" Text='<%# Bind("fhamt", "{0:#,##0.00;(#,##0.00)}") %>'></asp:Label>
													</ItemTemplate>
													<ItemStyle HorizontalAlign="Right" />
													<FooterTemplate>
														<div style="text-align: right;">
															<asp:Label ID="lblFH" runat="server" Font-Bold="true" BackColor="#CCCCCC" ForeColor="White" Width="100%" Height="100%" />
														</div>
													</FooterTemplate>
													<FooterStyle BorderStyle="Solid" HorizontalAlign="Right" VerticalAlign="Middle" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Discount">
													<ItemTemplate>
														<asp:Label ID="txtDisc" runat="server" Text='<%# Bind("discamt", "{0:#,##0.00;(#,##0.00)}") %>'></asp:Label>
													</ItemTemplate>
													<ItemStyle HorizontalAlign="Right" />
													<FooterTemplate>
														<div style="text-align: right;">
															<asp:Label ID="lblDisc" runat="server" Font-Bold="true" BackColor="#CCCCCC" ForeColor="White" Width="100%" Height="100%" />
														</div>
													</FooterTemplate>
													<FooterStyle BorderStyle="Solid" HorizontalAlign="Right" VerticalAlign="Middle" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Net Amt">
													<ItemTemplate>
														<asp:Label ID="txtNet" runat="server" Text='<%# Bind("netamt", "{0:#,##0.00;(#,##0.00)}") %>'></asp:Label>
													</ItemTemplate>
													<ItemStyle HorizontalAlign="Right" />
													<FooterTemplate>
														<div style="text-align: right;">
															<asp:Label ID="lblNet" runat="server" Font-Bold="true" BackColor="#CCCCCC" ForeColor="White" Width="100%" Height="100%" />
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
											$('#<%=dgvRegSumCustGrp.ClientID %>').Scrollable({
											ScrollHeight: 605,
											IsInPanel: true
										});
									});
									</script>

									<%--Smn Grp--%>
									<asp:Panel ID="PregSumSmn" runat="server" Width="99%" Height="630px" Visible="false">
										<asp:GridView ID="dgvRegSumSmnGrp" runat="server" AutoGenerateColumns="False" OnRowDataBound="dgvRegSumSmnGrp_RowDataBound" OnRowCreated="dgvRegSumSmnGrp_RowCreated" Font-Names="Segoe UI" Font-Size="small" BackColor="White" BorderColor="#999999" BorderStyle="Solid" AllowSorting="true" BorderWidth="1px" CellPadding="3" GridLines="Vertical" HeaderStyle-BorderColor="Black" ShowHeader="true" ShowFooter="true">
											<AlternatingRowStyle BackColor="#DCDCDC" />
											<Columns>
												<asp:BoundField DataField="smnno">
													<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px"></ItemStyle>
												</asp:BoundField>

												<asp:BoundField DataField="fullname">
													<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="200px"></ItemStyle>
												</asp:BoundField>

												<asp:BoundField DataField="invno">
													<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="90px"></ItemStyle>
												</asp:BoundField>

												<asp:BoundField DataField="tc">
													<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="40px"></ItemStyle>
												</asp:BoundField>

												<asp:BoundField DataField="transdate" DataFormatString="{0:yyyy-MM-dd}">
													<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="90px"></ItemStyle>
												</asp:BoundField>

												<asp:BoundField DataField="dono">
													<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="90px"></ItemStyle>
												</asp:BoundField>

												<asp:BoundField DataField="docno">
													<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="90px"></ItemStyle>
												</asp:BoundField>


												<asp:TemplateField HeaderText="Sold To">
													<ItemTemplate>
														<asp:Label ID="Label1" runat="server" Text='<%# Bind("custno") %>'></asp:Label>
													</ItemTemplate>
													<ItemStyle HorizontalAlign="Left" Width="375px" />
													<FooterTemplate>
														<div style="text-align: center;">
															<asp:Label ID="lblGTotal" runat="server" Font-Bold="true" BackColor="#CCCCCC" ForeColor="White" Width="100%" Height="100%" />
														</div>
													</FooterTemplate>
													<FooterStyle BorderStyle="Solid" HorizontalAlign="Right" VerticalAlign="Middle" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Gross Amt">
													<ItemTemplate>
														<asp:Label ID="txtGross" runat="server" Text='<%# Bind("grossamt", "{0:#,##0.00;(#,##0.00)}") %>'></asp:Label>
													</ItemTemplate>
													<ItemStyle HorizontalAlign="Right" />
													<FooterTemplate>
														<div style="text-align: right;">
															<asp:Label ID="lblGross" runat="server" Font-Bold="true" BackColor="#CCCCCC" ForeColor="White" Width="100%" Height="100%" />
														</div>
													</FooterTemplate>
													<FooterStyle BorderStyle="Solid" HorizontalAlign="Right" VerticalAlign="Middle" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Gross Amt">
													<ItemTemplate>
														<asp:Label ID="txtVat" runat="server" Text='<%# Bind("vat", "{0:#,##0.00;(#,##0.00)}") %>'></asp:Label>
													</ItemTemplate>
													<ItemStyle HorizontalAlign="Right" />
													<FooterTemplate>
														<div style="text-align: right;">
															<asp:Label ID="lblVat" runat="server" Font-Bold="true" BackColor="#CCCCCC" ForeColor="White" Width="100%" Height="100%" />
														</div>
													</FooterTemplate>
													<FooterStyle BorderStyle="Solid" HorizontalAlign="Right" VerticalAlign="Middle" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="F & H Amt">
													<ItemTemplate>
														<asp:Label ID="txtFH" runat="server" Text='<%# Bind("fhamt", "{0:#,##0.00;(#,##0.00)}") %>'></asp:Label>
													</ItemTemplate>
													<ItemStyle HorizontalAlign="Right" />
													<FooterTemplate>
														<div style="text-align: right;">
															<asp:Label ID="lblFH" runat="server" Font-Bold="true" BackColor="#CCCCCC" ForeColor="White" Width="100%" Height="100%" />
														</div>
													</FooterTemplate>
													<FooterStyle BorderStyle="Solid" HorizontalAlign="Right" VerticalAlign="Middle" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Discount">
													<ItemTemplate>
														<asp:Label ID="txtDisc" runat="server" Text='<%# Bind("discamt", "{0:#,##0.00;(#,##0.00)}") %>'></asp:Label>
													</ItemTemplate>
													<ItemStyle HorizontalAlign="Right" />
													<FooterTemplate>
														<div style="text-align: right;">
															<asp:Label ID="lblDisc" runat="server" Font-Bold="true" BackColor="#CCCCCC" ForeColor="White" Width="100%" Height="100%" />
														</div>
													</FooterTemplate>
													<FooterStyle BorderStyle="Solid" HorizontalAlign="Right" VerticalAlign="Middle" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Net Amt">
													<ItemTemplate>
														<asp:Label ID="txtNet" runat="server" Text='<%# Bind("netamt", "{0:#,##0.00;(#,##0.00)}") %>'></asp:Label>
													</ItemTemplate>
													<ItemStyle HorizontalAlign="Right" />
													<FooterTemplate>
														<div style="text-align: right;">
															<asp:Label ID="lblNet" runat="server" Font-Bold="true" BackColor="#CCCCCC" ForeColor="White" Width="100%" Height="100%" />
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
											$('#<%=dgvRegSumSmnGrp.ClientID %>').Scrollable({
											ScrollHeight: 605,
											IsInPanel: true
										});
									});
									</script>

								</td>
							</tr>
					</table>

				</ContentTemplate>

			</ajaxToolkit:TabPanel>

			<ajaxToolkit:TabPanel ID="TabPanel2" runat="server" HeaderText="SO Monitoring">
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

							<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Salesman:&nbsp;&nbsp; 
							
							</td>
							<td style="border: 1px solid #000000; width: 250px; background-color: lightgray; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">&nbsp;
							<asp:DropDownList ID="cboSalesman" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" Width="97%">
							</asp:DropDownList>
							</td>
							<td style="width: auto; background-color: whitesmoke; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>

							<td style="width: 380px; background-color: whitesmoke; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" rowspan="2"></td>

						</tr>

						<tr style="height: 26px; background-color: lightgray">
							<td style="width: auto; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="6">Option:&nbsp;&nbsp;
							<asp:RadioButton ID="RadioButton1" runat="server" GroupName="Optn" AutoPostBack="true" Text="All" />
								&nbsp;&nbsp;
							<asp:RadioButton ID="RadioButton2" runat="server" GroupName="Optn" AutoPostBack="true" Text="Open" />
								&nbsp;&nbsp;
							<asp:RadioButton ID="RadioButton3" runat="server" GroupName="Optn" AutoPostBack="true" Text="Partial" />
								&nbsp;&nbsp;
							<asp:RadioButton ID="RadioButton4" runat="server" GroupName="Optn" AutoPostBack="true" Text="Served" />
								&nbsp;&nbsp;
							</td>
							<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Customer:&nbsp;&nbsp;
							
							</td>

							<td style="border: 1px solid #000000; width: 600px; background-color: lightgray; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="2">&nbsp;
							<asp:DropDownList ID="cboCustomer" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" Width="98%">
							</asp:DropDownList>
							</td>

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
								<asp:Panel ID="Panel2" runat="server" Width="100%" Height="243px">
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

			<ajaxToolkit:TabPanel ID="TabPanel3" runat="server" HeaderText="Sales Reports">
				<ContentTemplate>
					<table style="width: 100%; font-family: 'Segoe UI'; font-size: small; float: left; background-color: lightgrey; border-spacing: 0px; margin-left: 0px; padding-left: 0px; padding-right: 0px;">
						<%--line1--%>
						<tr style="height: 26px; background-color: lightgray">
							<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Reports:&nbsp;&nbsp;
							
							</td>
							<td style="border: 1px solid #000000; width: auto; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="4">
								<asp:DropDownList ID="cboReportT3" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" Width="99%">
								</asp:DropDownList>
							</td>

							<td style="width: 2px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>

							<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Salesman:&nbsp;&nbsp; 
							
							</td>
							<td style="border: 1px solid #000000; width: 250px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								<asp:DropDownList ID="cboSmnT3" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" Width="98%">
								</asp:DropDownList>
							</td>
							<td style="width: auto; background-color: whitesmoke; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>

							<td style="width: 380px; background-color: whitesmoke; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" rowspan="4"></td>
						</tr>

						<tr style="height: 26px; background-color: lightgray">
							<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								<asp:Label ID="Label3" runat="server" Text="Date From:"></asp:Label>&nbsp;&nbsp;
							</td>
							<td style="border: 1px solid #000000; width: 124px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								<asp:TextBox ID="dpT3DFrom" runat="server" TextMode="Date" Height="20px" Font-Names="Segoe UI" Font-Size="small" AutoPostBack="true" CssClass="DateBox" Width="120px"></asp:TextBox>
							</td>

							<td style="width: 2px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>

							<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								<asp:Label ID="Label4" runat="server" Text="Date To:"></asp:Label>&nbsp;&nbsp;
							</td>
							<td style="border: 1px solid #000000; width: 124px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								<asp:TextBox ID="dpT3DTo" runat="server" TextMode="Date" Height="20px" Font-Names="Segoe UI" Font-Size="small" AutoPostBack="true" CssClass="DateBox" Width="120px"></asp:TextBox>
							</td>

							<td style="width: 2px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Customer:&nbsp;&nbsp;
							
							</td>

							<td style="border: 1px solid #000000; width: 550px; background-color: lightgray; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="2">&nbsp;
							<asp:DropDownList ID="cboCustT3" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" Width="98%">
							</asp:DropDownList>
							</td>
							<td style="width: auto; background-color: whitesmoke; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" rowspan="2"></td>
						</tr>

						<tr style="height: auto; background-color: lightgray">
							<td style="width: auto; background-color: floralwhite; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="10"></td>
						</tr>
						<tr style="height: auto; background-color: lightgray">
							<td style="border: 1px solid #000000; height: 625px; width: auto; background-color: floralwhite; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; vertical-align: top;" colspan="10">
								<asp:Panel ID="Panel3" runat="server" Width="99%" Height="600px" Visible="false">
									<asp:GridView ID="dgvSalesSum" runat="server" AutoGenerateColumns="False" OnRowDataBound="dgvSalesSum_RowDataBound"
										OnRowCreated="dgvSalesSum_RowCreated" Font-Names="Segoe UI" Font-Size="small" BackColor="White" BorderColor="#999999" BorderStyle="Solid" AllowSorting="true" BorderWidth="1px" CellPadding="3" GridLines="Vertical" HeaderStyle-BorderColor="Black" ShowHeader="true" HeaderStyle-HorizontalAlign="Center">
										<AlternatingRowStyle BackColor="#DCDCDC" />
										<Columns>
											<asp:BoundField DataField="invno">
												<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></ItemStyle>
											</asp:BoundField>

											<asp:BoundField DataField="tc">
												<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="40px"></ItemStyle>
											</asp:BoundField>

											<asp:BoundField DataField="transdate" DataFormatString="{0:yyyy-MM-dd}">
												<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></ItemStyle>
											</asp:BoundField>

											<asp:BoundField DataField="docno">
												<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></ItemStyle>
											</asp:BoundField>

											<asp:BoundField DataField="custno">
												<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="375px"></ItemStyle>
											</asp:BoundField>

											<asp:BoundField DataField="shipto">
												<ItemStyle HorizontalAlign="left" VerticalAlign="Middle" Width="375px"></ItemStyle>
											</asp:BoundField>

											<asp:BoundField DataField="grossamt" DataFormatString="{0:#,##0.00;(#,##0.00)}">
												<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="110px"></ItemStyle>
											</asp:BoundField>

											<asp:BoundField DataField="discamt" DataFormatString="{0:#,##0.00;(#,##0.00)}">
												<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="110px"></ItemStyle>
											</asp:BoundField>

											<asp:BoundField DataField="vat" DataFormatString="{0:#,##0.00;(#,##0.00)}">
												<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="110px"></ItemStyle>
											</asp:BoundField>

											<asp:BoundField DataField="netamt" DataFormatString="{0:#,##0.00;(#,##0.00)}">
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
										$('#<%=dgvSalesSum.ClientID %>').Scrollable({
											ScrollHeight: 600,
											IsInPanel: true
										});
									});
								</script>

								<%--for detailed--%>
								<asp:Panel ID="Panel4" runat="server" Width="99%" Height="600px" Visible="false">
									<asp:GridView ID="dgvSalesDet" runat="server" AutoGenerateColumns="False" OnRowDataBound="dgvSalesDet_RowDataBound" OnRowCreated="dgvSalesDet_RowCreated" Font-Names="Segoe UI" Font-Size="small" BackColor="White" BorderColor="#999999" BorderStyle="Solid" AllowSorting="true" BorderWidth="1px" CellPadding="3" GridLines="Vertical" HeaderStyle-BorderColor="Black" ShowHeader="true" HeaderStyle-HorizontalAlign="Center">
										<AlternatingRowStyle BackColor="#DCDCDC" />
										<Columns>
											<asp:BoundField DataField="codeno">
												<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="120px"></ItemStyle>
											</asp:BoundField>
											<asp:BoundField DataField="mmdesc">
												<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="1000px"></ItemStyle>
											</asp:BoundField>
											<asp:BoundField DataField="qty" DataFormatString="{0:#,##0;(#,##0)}">
												<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="110px"></ItemStyle>
											</asp:BoundField>
											<asp:BoundField DataField="wt" DataFormatString="{0:#,##0.00;(#,##0.00)}">
												<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="110px"></ItemStyle>
											</asp:BoundField>
											<asp:BoundField DataField="amt" DataFormatString="{0:#,##0.00;(#,##0.00)}">
												<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="110px"></ItemStyle>
											</asp:BoundField>
											<asp:BoundField DataField="asp" DataFormatString="{0:#,##0.00;(#,##0.00)}">
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
										$('#<%=dgvSalesDet.ClientID %>').Scrollable({
											ScrollHeight: 600,
											IsInPanel: true
										});
									});
								</script>
							</td>
						</tr>

					</table>

					<table style="width: 100%; font-family: 'Segoe UI'; font-size: small; float: left; background-color: lightgrey; border-spacing: 0px; margin-left: 0px; padding-left: 0px; padding-right: 0px;">
						<tr style="height: 26px; background-color: lightgray;">
							<td style="border: 1px solid #000000; width: 1000px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Total &nbsp;&nbsp;
							</td>
							<td style="border: 1px solid #000000; width: 102px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								<asp:Label ID="lblGrossAmt" runat="server" Text="0.00" ForeColor="Red"></asp:Label>
							</td>
							<td style="border: 1px solid #000000; width: 102px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								<asp:Label ID="lblDiscAmt" runat="server" Text="0.00" ForeColor="Red"></asp:Label>
							</td>
							<td style="border: 1px solid #000000; width: 102px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								<asp:Label ID="lblVatAmt" runat="server" Text="0.00" ForeColor="Red"></asp:Label>
							</td>
							<td style="border: 1px solid #000000; width: 102px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								<asp:Label ID="lblNetAmt" runat="server" Text="0.00" ForeColor="Red"></asp:Label>
							</td>
							<td style="border: 1px solid #000000; width: 15px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
						</tr>
					</table>

				</ContentTemplate>

			</ajaxToolkit:TabPanel>

			<ajaxToolkit:TabPanel ID="TabPanel4" runat="server" HeaderText="Sales Monitoring">
				<ContentTemplate>
					<table style="width: 100%; font-family: 'Segoe UI'; font-size: small; float: left; background-color: lightgrey; border-spacing: 0px; margin-left: 0px; padding-left: 0px; padding-right: 0px;">
						<%--L1--%>
						<tr style="height: 26px; background-color: lightgray;">
							<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								<asp:Label ID="Label5" runat="server" Text="Date From:"></asp:Label>&nbsp;&nbsp;
							</td>
							<td style="border: 1px solid #000000; width: 110px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								<asp:TextBox ID="dpSalesDate1" runat="server" TextMode="Date" Height="20px" Font-Names="Segoe UI" Font-Size="small" AutoPostBack="true" CssClass="DateBox" Width="110px"></asp:TextBox>
							</td>
							<td style="width: 2px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Customer:&nbsp;&nbsp;
							</td>
							<td style="border: 1px solid #000000; width: 400px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="3">
								<asp:DropDownList ID="cboSalesCust" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" Width="99%">
								</asp:DropDownList>
							</td>
							
							<td style="width: 150px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								
							</td>
							<td style="width: 120px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								

							</td>
							<td style="width: 2px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>

							<td style="width: 200px; background-color: whitesmoke; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; vertical-align: middle" rowspan="2">
								

							</td>

						</tr>
						<%--L2--%>
						<tr style="height: 26px; background-color: lightgray;">
							<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								<asp:Label ID="Label8" runat="server" Text="Date To:"></asp:Label>&nbsp;&nbsp;
							</td>
							<td style="border: 1px solid #000000; width: 110px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								<asp:TextBox ID="dpSalesDate2" runat="server" TextMode="Date" Height="20px" Font-Names="Segoe UI" Font-Size="small" AutoPostBack="true" CssClass="DateBox" Width="110px"></asp:TextBox>
							</td>
							<td style="width: 2px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								<asp:Label ID="Label9" runat="server" Text="SKU:"></asp:Label>&nbsp;&nbsp;
							</td>
							<td style="border: 1px solid #000000; width: 400px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="3">
								<asp:DropDownList ID="cboSalesProd" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" Width="99%">
								</asp:DropDownList>
							</td>
							<td style="border: 1px solid #000000; width: 150px; background-color: khaki; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								<asp:Label ID="txtSalesCodeNo" runat="server" Text="" Visible="true"></asp:Label>
							</td>
							<td style="width: 120px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								

							</td>
							<td style="width: 2px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>


							<%--L3--%>
							<tr style="height: 2px; background-color: white;">
								<td style="width: auto; background-color: white; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="12"></td>
							</tr>
							<tr style="height: 653px; background-color: floralwhite;">
								<td style="border: 1px solid #000000; width: auto; background-color: floralwhite; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; vertical-align: top;" colspan="12">
									<asp:Panel ID="Panel7" runat="server" Width="99%" Height="600px" >
									<asp:GridView ID="dgvSalesProd" runat="server" AutoGenerateColumns="False" OnRowDataBound="dgvSalesProd_RowDataBound" OnRowCreated="dgvSalesProd_RowCreated" Font-Names="Segoe UI" Font-Size="small" BackColor="White" BorderColor="#999999" BorderStyle="Solid" AllowSorting="true" BorderWidth="1px" CellPadding="3" GridLines="Vertical" HeaderStyle-BorderColor="Black" ShowHeader="true" HeaderStyle-HorizontalAlign="Center">
										<AlternatingRowStyle BackColor="#DCDCDC" />
										<Columns>
											<asp:BoundField DataField="invno">
												<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></ItemStyle>
											</asp:BoundField>
											<asp:BoundField DataField="docno">
												<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></ItemStyle>
											</asp:BoundField>
											<asp:BoundField DataField="transdate" DataFormatString="{0:yyyy-MM-dd}">
												<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></ItemStyle>
											</asp:BoundField>
											<asp:BoundField DataField="cust">
												<ItemStyle HorizontalAlign="left" VerticalAlign="Middle" Width="375px"></ItemStyle>
											</asp:BoundField>
											<asp:BoundField DataField="codeno">
												<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="120px"></ItemStyle>
											</asp:BoundField>
											<asp:BoundField DataField="mm">
												<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="500px"></ItemStyle>
											</asp:BoundField>
											<asp:BoundField DataField="qty" DataFormatString="{0:#,##0;(#,##0)}">
												<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="110px"></ItemStyle>
											</asp:BoundField>
											<asp:BoundField DataField="wt" DataFormatString="{0:#,##0.00;(#,##0.00)}">
												<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="110px"></ItemStyle>
											</asp:BoundField>
											<asp:BoundField DataField="sp" DataFormatString="{0:#,##0.00;(#,##0.00)}">
												<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="110px"></ItemStyle>
											</asp:BoundField>
											<asp:BoundField DataField="itmamt" DataFormatString="{0:#,##0.00;(#,##0.00)}">
												<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="110px"></ItemStyle>
											</asp:BoundField>
											<asp:BoundField DataField="dono">
												<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="120px"></ItemStyle>
											</asp:BoundField>
											<asp:BoundField DataField="sono">
												<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="120px"></ItemStyle>
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
										$('#<%=dgvSalesProd.ClientID %>').Scrollable({
											ScrollHeight: 624,
											IsInPanel: true
										});
									});
								</script>

								</td>
							</tr>
					</table>
				</ContentTemplate>

			</ajaxToolkit:TabPanel>

			<ajaxToolkit:TabPanel ID="TabPanel5" runat="server" HeaderText="Other Reports">
				<ContentTemplate>
					<center>
						<h1>UNDER CONSTRUCTION</h1>
					</center>
				</ContentTemplate>

			</ajaxToolkit:TabPanel>

			<ajaxToolkit:TabPanel ID="TabPanel6" runat="server" HeaderText="Customer's PO Monitoring">
				<ContentTemplate>
					<table style="width: 100%; font-family: 'Segoe UI'; font-size: small; float: left; background-color: lightgrey; border-spacing: 0px; margin-left: 0px; padding-left: 0px; padding-right: 0px;">
						<tr style="height: 26px; background-color: whitesmoke;">
							<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								<asp:Label ID="Label1" runat="server" Text="Date From:"></asp:Label>&nbsp;&nbsp;
							</td>
							<td style="border: 1px solid #000000; width: 124px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								<asp:TextBox ID="dpPOdate1" runat="server" TextMode="Date" Height="20px" Font-Names="Segoe UI" Font-Size="small" AutoPostBack="true" CssClass="DateBox" Width="120px"></asp:TextBox>
							</td>

							<td style="width: 2px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>

							<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								<asp:Label ID="Label2" runat="server" Text="Date To:"></asp:Label>&nbsp;&nbsp;
							</td>
							<td style="border: 1px solid #000000; width: auto; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								<asp:TextBox ID="dpPOdate2" runat="server" TextMode="Date" Height="20px" Font-Names="Segoe UI" Font-Size="small" AutoPostBack="true" CssClass="DateBox" Width="120px"></asp:TextBox>
							</td>

							<td style="width: 2px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>

							<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Customer:&nbsp;&nbsp; 
							
							</td>
							<td style="border: 1px solid #000000; width: 450px; background-color: lightgray; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">&nbsp;
							<asp:DropDownList ID="cboCustPO" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" Width="97%">
							</asp:DropDownList>
							</td>
							<td style="width: 250px; background-color: whitesmoke; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>

							<td style="width: 250px; background-color: whitesmoke; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>

						</tr>

						<tr style="height: 2px; background-color: whitesmoke">
							<td style="width: auto; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="10"></td>
						</tr>

						<tr style="height: auto; background-color: whitesmoke">
							<td style="border: 1px solid #000000; width: auto; height: 650px; background-color: floralwhite; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; vertical-align: top;" colspan="2" rowspan="5">
								<asp:TreeView ID="tvPOlist" runat="server"></asp:TreeView>
							</td>
							<td style="width: 2px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" rowspan="5"></td>

							<td style="border: 1px solid #000000; width: auto; height: 26px; min-height: 26px; max-height: 26px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="7">Invoice List
							</td>
						</tr>
						<tr style="height: 416px; background-color: floralwhite">
							<td style="border: 1px solid #000000; width: auto; background-color: floralwhite; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; vertical-align: top;" colspan="7">
								<asp:Panel ID="Panel5" runat="server" Width="98%" Height="380px">
									<asp:GridView ID="dgvPODet" runat="server" AutoGenerateColumns="False" OnRowDataBound="dgvPODet_RowDataBound" OnRowCreated="dgvPODet_RowCreated" Font-Names="Segoe UI" Font-Size="small" BackColor="White" BorderColor="#999999" BorderStyle="Solid" AllowSorting="true" BorderWidth="1px" CellPadding="3" GridLines="Vertical" HeaderStyle-BorderColor="Black" ShowHeader="true" HeaderStyle-HorizontalAlign="Center">
										<AlternatingRowStyle BackColor="#DCDCDC" />
										<Columns>
											<asp:BoundField DataField="invno">
												<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="100px"></ItemStyle>
											</asp:BoundField>
											<asp:BoundField DataField="transdate" DataFormatString="{0:yyyy-MM-dd}">
												<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></ItemStyle>
											</asp:BoundField>
											<asp:BoundField DataField="docno">
												<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></ItemStyle>
											</asp:BoundField>
											<asp:BoundField DataField="codeno">
												<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="120px"></ItemStyle>
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
											<asp:BoundField DataField="sp" DataFormatString="{0:#,##0.00;(#,##0.00)}">
												<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="110px"></ItemStyle>
											</asp:BoundField>
											<asp:BoundField DataField="itmamt" DataFormatString="{0:#,##0.00;(#,##0.00)}">
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
										$('#<%=dgvPODet.ClientID %>').Scrollable({
											ScrollHeight: 360,
											IsInPanel: true
										});
									});
								</script>

							</td>
						</tr>

						<tr style="height: 2px; background-color: whitesmoke">
							<td style="width: auto; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="7"></td>
						</tr>

						<tr style="height: 26px; background-color: whitesmoke">
							<td style="border: 1px solid #000000; width: auto; height: 26px; min-height: 26px; max-height: 26px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="7">Order Summary
							</td>
						</tr>

						<tr style="height: 202px; background-color: floralwhite">
							<td style="border: 1px solid #000000; width: auto; background-color: floralwhite; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; vertical-align: top;" colspan="7">
								<asp:Panel ID="Panel6" runat="server" Width="98%" Height="170px">
									<asp:GridView ID="dgvPOsum" runat="server" AutoGenerateColumns="False" OnRowDataBound="dgvPOsum_RowDataBound" OnRowCreated="dgvPOsum_RowCreated" Font-Names="Segoe UI" Font-Size="small" BackColor="White" BorderColor="#999999" BorderStyle="Solid" AllowSorting="true" BorderWidth="1px" CellPadding="3" GridLines="Vertical" HeaderStyle-BorderColor="Black" ShowHeader="true" HeaderStyle-HorizontalAlign="Center">
										<AlternatingRowStyle BackColor="#DCDCDC" />
										<Columns>
											<asp:BoundField DataField="codeno">
												<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="120px"></ItemStyle>
											</asp:BoundField>
											<asp:BoundField DataField="mmdesc">
												<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="800px"></ItemStyle>
											</asp:BoundField>
											<asp:BoundField DataField="qty" DataFormatString="{0:#,##0;(#,##0)}">
												<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="110px"></ItemStyle>
											</asp:BoundField>
											<asp:BoundField DataField="wt" DataFormatString="{0:#,##0.00;(#,##0.00)}">
												<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="110px"></ItemStyle>
											</asp:BoundField>
											<asp:BoundField DataField="amt" DataFormatString="{0:#,##0.00;(#,##0.00)}">
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
										$('#<%=dgvPOsum.ClientID %>').Scrollable({
											ScrollHeight: 165,
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

	</center>

</asp:Content>
