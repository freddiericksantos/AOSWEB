<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/FI.master" CodeBehind="APReports.aspx.vb" Inherits="AOS100web.APReports" %>

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
					<asp:Image runat="server" imageurl="~/images/new_16.png" style="vertical-align: middle" AlternateText=""/>&nbsp;New
				</asp:LinkButton>
				<asp:LinkButton ID="lbSave" runat="server" Style="text-decoration: none; background-color: #7fc6f6; padding-right: 5px; font-family: Tahoma; font-size: 14px; color: white; margin-right: 5px; padding-left: 5px; min-height: 22px;" BorderStyle="Solid" ToolTip="Save" Enabled="false">
					<asp:Image runat="server" imageurl="~/images/save_16.png" style="vertical-align: middle" AlternateText=""/>&nbsp;Save
				</asp:LinkButton>
				<asp:LinkButton ID="lbPrint" runat="server" OnClick="lbPrint_Click" Style="text-decoration: none; background-color: #7fc6f6; padding-right: 5px; font-family: Tahoma; font-size: 14px; color: white; margin-right: 5px; padding-left: 5px; min-height: 22px;"
					BorderStyle="Solid" ToolTip="Print" Enabled="true">
					<asp:Image runat="server" imageurl="~/images/Print_16.png" style="vertical-align: middle" AlternateText=""/>&nbsp;Print
				</asp:LinkButton>
				<asp:LinkButton ID="lbDelete" runat="server" Style="text-decoration: none; background-color: #7fc6f6; padding-right: 5px; font-family: Tahoma; font-size: 14px; color: white; margin-right: 5px; padding-left: 5px; min-height: 22px;" BorderStyle="Solid" ToolTip="Void" Enabled="false">
					<asp:Image runat="server" imageurl="~/images/delete_16.png" style="vertical-align: middle" AlternateText=""/>&nbsp;Void
				</asp:LinkButton>
				<asp:LinkButton ID="lbClose" runat="server" OnClick="lbClose_Click" Style="text-decoration: none; background-color: #7fc6f6; font-family: Tahoma; font-size: 14px; color: white; min-height: 22px; padding-left: 5px;"
					BorderStyle="Solid" ToolTip="Close" CssClass="StdBottomR">
					<asp:Image runat="server" imageurl="~/images/Exit_16.png" style="vertical-align: middle" AlternateText=""/>&nbsp;Close&nbsp;
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
		<ajaxToolkit:TabPanel ID="TabPanel1" runat="server" HeaderText="Registers">
			<ContentTemplate>
				
				<%--<h1>Not Yet Available</h1>--%>

				<table style="width: 100%; font-family: 'Segoe UI'; font-size: small; float: left; background-color: lightgrey; border-spacing: 0px; margin-left: 0px; padding-left: 0px; padding-right: 0px;">
					<tr style="height: 26px; background-color: lightgray">
						<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:Label ID="lblDate" runat="server" Text="Date From:"></asp:Label>&nbsp;&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: auto; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:TextBox ID="dpTransDate" runat="server" TextMode="Date" Height="20px" Font-Names="Segoe UI" Font-Size="small" AutoPostBack="true" CssClass="DateBox" Width="120px"></asp:TextBox>
						</td>
						<td style="width: auto; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>

						<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:Label ID="lblDate2" runat="server" Text="Date To:"></asp:Label>&nbsp;&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: auto; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:TextBox ID="dpTransDate2" runat="server" TextMode="Date" Height="20px" Font-Names="Segoe UI" Font-Size="small" AutoPostBack="true" CssClass="DateBox" Width="120px"></asp:TextBox>
						</td>

						<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Main Report:&nbsp;&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: 250px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="2">
							<asp:DropDownList ID="cboMain" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" Width="98%">
							</asp:DropDownList>

						</td>
						<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Report Format:&nbsp;&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: 250px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="2">
							<asp:DropDownList ID="cboFormat" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" Width="98%">
							</asp:DropDownList>

						</td>
						<td style="border: 1px solid #000000; width: 300px; background-color: whitesmoke; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; vertical-align:middle; " rowspan="3">&nbsp;&nbsp;
							<asp:LinkButton ID="btnGenerate" runat="server" Style="text-decoration: none; background-color: #7fc6f6; padding-right: 5px;padding-top:3px; height: 44px; font-family: Tahoma; font-size: 14px; color: white; margin-right: 5px; padding-left: 5px; vertical-align:middle; " BorderStyle="Solid" ToolTip="Process" Enabled="false" CssClass="StdButtLarge">
							<asp:Image runat="server" imageurl="~/images/Process_40px.png" style="vertical-align: middle; " AlternateText=""/>&nbsp;Process
							</asp:LinkButton>



						</td>

					</tr>
					

					<tr style="height: auto; background-color: lightgray">
						<td style="width: auto; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="10"></td>
					</tr>
					
					<tr style="height: 26px; background-color: lightgray">
						<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Filter 1:&nbsp;&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: auto; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="4">
							<asp:DropDownList ID="cboFilter1" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" Width="98%">
							</asp:DropDownList>

						</td>
						<td style="border: 1px solid #000000; width: 120px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:Label ID="lblFiler2" runat="server" Text="Filter 2:" Visible="true"></asp:Label>&nbsp;&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: auto; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="2">
							<asp:DropDownList ID="cboFilter2" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" Width="98%" Visible="true">
							</asp:DropDownList>

						</td>

						<td style="border: 1px solid #000000; width: 120px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:Label ID="lblFilter3" runat="server" Text="Filter 3:" Visible="true"></asp:Label>&nbsp;&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: auto; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="2">
							<asp:DropDownList ID="cboFilter3" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" Width="98%" Visible="true">
							</asp:DropDownList>

						</td>
						

					</tr>
					<tr style="height: auto; background-color: lightgray">
						<td style="width: auto; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="14"></td>
					</tr>

					<tr style="height: 26px; background-color: lightgray">
						<td style="border: 1px solid #000000; width: auto; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="14">
							<asp:Panel ID="Tab1Panel2" runat="server" Visible="false">
								Option:
								<asp:DropDownList ID="cboAMUopt" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" Width="150px" Visible="false">
								</asp:DropDownList>
								&nbsp;&nbsp;
								<asp:RadioButton ID="RadioButton1" runat="server" Text="Doc. Date(VP Date)" GroupName="admOpt"/>
								&nbsp;&nbsp;
								<asp:RadioButton ID="RadioButton2" runat="server" Text="Invoice Date(Supplier Invoice)" GroupName="admOpt"/>
							</asp:Panel>
						</td>
					</tr>

					<tr style="height: 628px; background-color: floralwhite">
						<td style="border: 1px solid #000000; height: auto; width: auto; background-color: floralwhite; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; vertical-align:top;" colspan="14">
							<%--Reg Summary--%>
							<asp:Panel ID="Panel4" runat="server" Width="100%" Height="600px" Visible="false">
								<asp:GridView ID="dgvRegSum" runat="server" AutoGenerateColumns="False" OnRowDataBound="dgvRegSum_RowDataBound"
									OnRowCreated="dgvRegSum_RowCreated" Font-Names="Segoe UI" Font-Size="small" BackColor="White" BorderColor="#999999" BorderStyle="Solid" AllowSorting="true" BorderWidth="1px" CellPadding="3" GridLines="Vertical" HeaderStyle-BorderColor="Black" ShowHeader="true" ShowFooter="true">
									<AlternatingRowStyle BackColor="#DCDCDC" />
									<Columns>
										<asp:BoundField DataField="docno" >
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="tc" >
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="40px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="transdate" DataFormatString="{0:yyyy-MM-dd}">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="refdoc" >
											<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="100px"></ItemStyle>
										</asp:BoundField>

										<asp:TemplateField HeaderText="Vendor Name">
											<ItemTemplate>
												<asp:Label ID="Label1" runat="server" Text='<%# Bind("ven") %>'></asp:Label>
											</ItemTemplate>
											<ItemStyle HorizontalAlign="Left" Width="470px" />
											<FooterTemplate>
												<div style="text-align: center;">
													<asp:Label ID="lblGTotal" runat="server" Font-Bold="true" BackColor="#CCCCCC" ForeColor="White" Width="100%" Height="100%" />
												</div>
											</FooterTemplate>
											<FooterStyle BorderStyle="Solid" HorizontalAlign="Right" VerticalAlign="Middle" />
										</asp:TemplateField>
																				
										<asp:TemplateField HeaderText="Amount">
											<ItemTemplate>
												<asp:Label ID="txtNet" runat="server" Text='<%# Bind("docamt", "{0:#,##0.00;(#,##0.00)}") %>'></asp:Label>
											</ItemTemplate>
											<ItemStyle HorizontalAlign="Right" />
											<FooterTemplate>
												<div style="text-align: right;">
													<asp:Label ID="lblTotalNet" runat="server" Font-Bold="true" BackColor="#CCCCCC" ForeColor="White" Width="100%" Height="100%" />
												</div>
											</FooterTemplate>
											<FooterStyle BorderStyle="Solid" HorizontalAlign="Right" VerticalAlign="Middle" />
										</asp:TemplateField>

																	
										<asp:BoundField DataField="stat" >
											<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="100px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="remarks" >
											<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="450px"></ItemStyle>
										</asp:BoundField>

									</Columns>

									<FooterStyle BackColor="#CCCCCC" ForeColor="Black" BorderStyle="Solid"/>
									<HeaderStyle BackColor="#007acc" Font-Bold="True" ForeColor="White" Height="22px"/>
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
										ScrollHeight: 600,
										IsInPanel: true
									});
								});
							</script>

							<%--Reg Detailed--%>
							<asp:Panel ID="Panel5" runat="server" Width="99%" Height="600px" Visible="false">
								<asp:GridView ID="dgvRegDet" runat="server" AutoGenerateColumns="False" OnRowDataBound="dgvRegDet_RowDataBound"
									OnRowCreated="dgvRegDet_RowCreated" Font-Names="Segoe UI" Font-Size="small" BackColor="White" BorderColor="#999999" BorderStyle="Solid" AllowSorting="true" BorderWidth="1px" CellPadding="3" GridLines="Vertical" HeaderStyle-BorderColor="Black" ShowHeader="true" ShowFooter="true">
									<AlternatingRowStyle BackColor="#DCDCDC" />
									<Columns>
										<asp:BoundField DataField="docno">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="tc">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="40px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="transdate" DataFormatString="{0:yyyy-MM-dd}">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></ItemStyle>
										</asp:BoundField>

										<asp:TemplateField HeaderText="Vendor Name">
											<ItemTemplate>
												<asp:Label ID="Label1" runat="server" Text='<%# Bind("venno") %>'></asp:Label>
											</ItemTemplate>
											<ItemStyle HorizontalAlign="Left" Width="380px" />
											<FooterTemplate>
												<div style="text-align: center;">
													<asp:Label ID="lblGTotal" runat="server" Font-Bold="true" BackColor="#CCCCCC" ForeColor="White" Width="100%" Height="100%" />
												</div>
											</FooterTemplate>
											<FooterStyle BorderStyle="Solid" HorizontalAlign="Right" VerticalAlign="Middle" />
										</asp:TemplateField>

										<asp:BoundField DataField="ccno">
											<ItemStyle HorizontalAlign="left" VerticalAlign="Middle" Width="300px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="acctno">
											<ItemStyle HorizontalAlign="left" VerticalAlign="Middle" Width="400px"></ItemStyle>
										</asp:BoundField>

										<asp:TemplateField HeaderText="DR Amount">
											<ItemTemplate>
												<asp:Label ID="txtDRamt" runat="server" Text='<%# Bind("dramt", "{0:#,##0.00;(#,##0.00)}") %>'></asp:Label>
											</ItemTemplate>
											<ItemStyle HorizontalAlign="Right" />
											<FooterTemplate>
												<div style="text-align: right;">
													<asp:Label ID="lblTotalDr" runat="server" Font-Bold="true" BackColor="#CCCCCC" ForeColor="White" Width="100%" Height="100%" />
												</div>
											</FooterTemplate>
											<FooterStyle BorderStyle="Solid" HorizontalAlign="Right" VerticalAlign="Middle" />
										</asp:TemplateField>

										<asp:TemplateField HeaderText="CR Amount">
											<ItemTemplate>
												<asp:Label ID="txtCRamt" runat="server" Text='<%# Bind("cramt", "{0:#,##0.00;(#,##0.00)}") %>'></asp:Label>
											</ItemTemplate>
											<ItemStyle HorizontalAlign="Right" />
											<FooterTemplate>
												<div style="text-align: right;">
													<asp:Label ID="lblTotalCr" runat="server" Font-Bold="true" BackColor="#CCCCCC" ForeColor="White" Width="100%" Height="100%" />
												</div>
											</FooterTemplate>
											<FooterStyle BorderStyle="Solid" HorizontalAlign="Right" VerticalAlign="Middle" />
										</asp:TemplateField>

										<asp:TemplateField HeaderText="Net Amount">
											<ItemTemplate>
												<asp:Label ID="txtNetAmt" runat="server" Text='<%# Bind("netamt", "{0:#,##0.00;(#,##0.00)}") %>'></asp:Label>
											</ItemTemplate>
											<ItemStyle HorizontalAlign="Right" />
											<FooterTemplate>
												<div style="text-align: right;">
													<asp:Label ID="lblTotalNet" runat="server" Font-Bold="true" BackColor="#CCCCCC" ForeColor="White" Width="100%" Height="100%" />
												</div>
											</FooterTemplate>
											<FooterStyle BorderStyle="Solid" HorizontalAlign="Right" VerticalAlign="Middle" />
										</asp:TemplateField>

										<asp:BoundField DataField="pk">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="30px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="subacct">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px"></ItemStyle>
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
									$('#<%=dgvRegDet.ClientID %>').Scrollable({
										ScrollHeight: 600,
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
		<ajaxToolkit:TabPanel ID="TabPanel2" runat="server" HeaderText="Payment Monitoring">
			<ContentTemplate>
				<table style="width: 100%; font-family: 'Segoe UI'; font-size: small; float: left; background-color: lightgrey; border-spacing: 0px; margin-left: 0px; padding-left: 0px; padding-right: 0px;">
					<tr style="height: 26px; background-color: lightgray">
						<td style="border: 1px solid #000000; width: 80px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Date From:&nbsp;&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: 112px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:TextBox ID="dpDocDate1" runat="server" TextMode="Date" Height="20px" Font-Names="Segoe UI" Font-Size="small" AutoPostBack="true" CssClass="DateBox" Width="110px"></asp:TextBox>
						</td>
						
						<td style="width: auto; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
						
						<td style="border: 1px solid #000000; width:80px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Date To:&nbsp;&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: 112px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:TextBox ID="dpDocDate2" runat="server" TextMode="Date" Height="20px" Font-Names="Segoe UI" Font-Size="small" AutoPostBack="true" CssClass="DateBox" Width="110px"></asp:TextBox>
						</td>

						<td style="width: auto; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>

						<td style="border: 1px solid #000000; width:80px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Source:&nbsp;&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: 140px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:DropDownList ID="cboDocSource" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" Width="98%">
							</asp:DropDownList>
						</td>

						<td style="width: auto; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>

						<td style="border: 1px solid #000000; width:80px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Vendor:&nbsp;&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: 400px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:DropDownList ID="cboVendor" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" Width="98%">
							</asp:DropDownList>
						</td>
						
						<td style="width: auto; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>

						<td style="border: 1px solid #000000; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="2"> Check Voucher Summary </td>
					</tr>
					<%--line2--%>
					<tr style="height: 26px; background-color: lightgray">
						<td style="border: 1px solid #000000; width: 70%; height:auto ;background-color: floralwhite; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="11" rowspan="6">
							<%--Vendors--%>
							<asp:Panel ID="Panel1" runat="server" Width="100%" Height="680px">
								<asp:GridView ID="dgvStatList" runat="server" AutoGenerateColumns="False" OnRowDataBound="dgvStatList_RowDataBound"
									OnRowCreated="dgvStatList_RowCreated" Font-Names="Segoe UI" Font-Size="small" BackColor="White" BorderColor="#999999" BorderStyle="Solid" AllowSorting="true" BorderWidth="1px" CellPadding="3" GridLines="Vertical" HeaderStyle-BorderColor="Black" ShowHeader="true">
									<AlternatingRowStyle BackColor="#DCDCDC" />
									<Columns>
										<asp:CommandField HeaderText="Select" ShowSelectButton="true">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="72px"></ItemStyle>
										</asp:CommandField>

										<asp:BoundField DataField="mmrrno" >
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="110px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="transdate" DataFormatString="{0:yyyy-MM-dd}">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="110px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="ven" >
											<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="470px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="term" >
											<ItemStyle HorizontalAlign="left" VerticalAlign="Middle" Width="150px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="amt"  DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="110px">
											</ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="docnovp" >
											<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="110px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="expdate" DataFormatString="{0:yyyy-MM-dd}">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="110px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="docamt"  DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="110px">
											</ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="stat" >
											<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="100px"></ItemStyle>
										</asp:BoundField>

									</Columns>

									<FooterStyle BackColor="#CCCCCC" ForeColor="Black" BorderStyle="Solid" />
									<HeaderStyle BackColor="#007acc" Font-Bold="True" ForeColor="White" Height="22px"/>
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
									$('#<%=dgvStatList.ClientID %>').Scrollable({
										ScrollHeight: 656,
										IsInPanel: true
									});
								});
							</script>
						</td>
						
						<td style="width: auto; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
						
						<td style="border: 1px solid #000000; width: 30%; height: 398px; background-color: floralwhite; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="2">
							<%--Summary (Available Soon)--%>
							<asp:Label ID="lblMsgNoCheck" runat="server" Text="No Check Yet" ForeColor="Red" Font-Size="Large" Visible="false"></asp:Label>
								<asp:Panel ID="Panel2" runat="server" Width="100%" Height="400px">
								<asp:GridView ID="dgvCVdet" runat="server" AutoGenerateColumns="False" OnRowDataBound="dgvCVdet_RowDataBound" OnRowCreated="dgvCVdet_RowCreated" Font-Names="Segoe UI" Font-Size="small" BackColor="White" BorderColor="#999999" BorderStyle="Solid" AllowSorting="true" BorderWidth="1px" CellPadding="3" GridLines="Vertical" HeaderStyle-BorderColor="Black" ShowHeader="true">
									<AlternatingRowStyle BackColor="#DCDCDC" />
									<Columns>
										<asp:BoundField DataField="docno" >
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="120px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="chdate" DataFormatString="{0:yyyy-MM-dd}">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="120px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="chno" >
											<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="150px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="dramt"  DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="150px">
											</ItemStyle>
										</asp:BoundField>
																		

									</Columns>

									<FooterStyle BackColor="#CCCCCC" ForeColor="Black" BorderStyle="Solid" />
									<HeaderStyle BackColor="#007acc" Font-Bold="True" ForeColor="White" Height="22px"/>
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
									$('#<%=dgvCVdet.ClientID %>').Scrollable({
										ScrollHeight: 300,
										IsInPanel: true
									});
								});
							</script>--%>



						</td>
					</tr>
										
					<tr style="height: 26px; background-color: lightgray">
						<td style="width: auto; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>

						<td style="border: 1px solid #000000; width: 300px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" >Total:&nbsp;&nbsp; </td>
						<td style="border: 1px solid #000000; width: 120px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" >
							
							<asp:Label ID="lblTotCVamt" runat="server" Text="0.00" ForeColor="Red"></asp:Label>&nbsp;&nbsp;

						</td>
					</tr>
					<tr style="height: auto; background-color: whitesmoke">
						<td style="width: auto; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" ></td>
						<td style="width: auto; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="2" ></td>
					</tr>

					<tr style="height: 26px; background-color: lightgray">
						<td style="width: auto; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>

						<td style="border: 1px solid #000000; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="2">Check Voucher Details</td>
					</tr>
					<tr style="height: auto; background-color: lightgray">
						<td style="width: auto; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
						<td style="border: 1px solid #000000; width:auto; height: 196px; background-color: floralwhite; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="2">

							<%--Details (Available Soon)--%>

							<asp:Panel ID="Panel3" runat="server" Width="100%" Height="186px">
								<asp:GridView ID="dgvCVdetList" runat="server" AutoGenerateColumns="False" OnRowDataBound="dgvCVdetList_RowDataBound" OnRowCreated="dgvCVdetList_RowCreated" Font-Names="Segoe UI" Font-Size="small" BackColor="White" BorderColor="#999999" BorderStyle="Solid" AllowSorting="true" BorderWidth="1px" CellPadding="3" GridLines="Vertical" HeaderStyle-BorderColor="Black" ShowHeader="true">
									<AlternatingRowStyle BackColor="#DCDCDC" />
									<Columns>
										<asp:BoundField DataField="itemno" >
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="60px"></ItemStyle>
										</asp:BoundField>
																			
										<asp:BoundField DataField="vpno" >
											<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="120px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="tcref" >
											<ItemStyle HorizontalAlign="center" VerticalAlign="Middle" Width="40px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="transdate" DataFormatString="{0:yyyy-MM-dd}">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="120px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="dramt"  DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="110px">
											</ItemStyle>
										</asp:BoundField>
																		
										
									</Columns>

									<FooterStyle BackColor="#CCCCCC" ForeColor="Black" BorderStyle="Solid" />
									<HeaderStyle BackColor="#007acc" Font-Bold="True" ForeColor="White" Height="22px"/>
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
									$('#<%=dgvCVdetList.ClientID %>').Scrollable({
										ScrollHeight: 180,
										IsInPanel: true
									});
								});
							</script>

						</td>

					</tr>	

					<tr style="height: 26px; background-color: lightgray">
						<td style="width: auto; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
						<td style="border: 1px solid #000000; width: 300px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" >
							<asp:Label ID="Label10" runat="server" Text="Total" ForeColor="Black"></asp:Label>&nbsp;&nbsp;

						</td>
						<td style="border: 1px solid #000000; width: 110px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" >

							<asp:Label ID="lblTotDetAmt" runat="server" Text="0.00" ForeColor="Red"></asp:Label>&nbsp;&nbsp;

						</td>
					</tr>

				</table>
			</ContentTemplate>

		</ajaxToolkit:TabPanel>

		<%--SO Monitoring--%>
		<ajaxToolkit:TabPanel ID="TabPanel3" runat="server" HeaderText="Expense Query">
			<ContentTemplate>
				<h1>Not Yet Available</h1>



			</ContentTemplate>

		</ajaxToolkit:TabPanel>

		<ajaxToolkit:TabPanel ID="TabPanel4" runat="server" HeaderText="Budget Analysis">
			<ContentTemplate>
				<h1>Not Yet Available</h1>



			</ContentTemplate>

		</ajaxToolkit:TabPanel>

		<ajaxToolkit:TabPanel ID="TabPanel5" runat="server" HeaderText="Expense Query Per Account">
			<ContentTemplate>

				<h1>Not Yet Available</h1>


			</ContentTemplate>

		</ajaxToolkit:TabPanel>

	</ajaxToolkit:TabContainer>

</asp:Content>
