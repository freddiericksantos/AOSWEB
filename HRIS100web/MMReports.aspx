<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MM.master" CodeBehind="MMReports.aspx.vb" Inherits="AOS100web.MMReports" %>

<%@ Register Assembly="EditableDropDownList" Namespace="EditableControls" TagPrefix="editable" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentOfMM" runat="server">
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

	<ajaxToolkit:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" Style="width: 99%; height: 600px;" >
		<ajaxToolkit:TabPanel ID="TabPanel1" runat="server" HeaderText="Register">
			<ContentTemplate>
				<table style="width: 100%; font-family: 'Segoe UI'; font-size: small; float: left; background-color: lightgrey; border-spacing: 0px; margin-left: 0px; padding-left: 0px; padding-right: 0px;">
					<%--L1--%>
					<tr style="height: 26px; background-color: lightgray;">
						<td style="border: 1px solid #000000; width: 110px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:Label ID="lblDate" runat="server" Text="Date From:"></asp:Label>&nbsp;&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: 110px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:TextBox ID="dpTransDate" runat="server" TextMode="Date" Height="20px" Font-Names="Segoe UI" Font-Size="small" AutoPostBack="true" CssClass="DateBox" Width="110px"></asp:TextBox>
						</td>
						<td style="width: 2px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
						<td style="border: 1px solid #000000; width: 120px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Report Format:&nbsp;&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: 400px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:DropDownList ID="cboFormat" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" Width="99%">
							</asp:DropDownList>
						</td>
						<td style="border: 1px solid #000000; width: 120px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:CheckBox ID="CheckBox3" runat="server" Text="Void Only" Visible="false" />
						</td>

						<td style="width: 2px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
						<td style="border: 1px solid #000000; width: 120px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:Label ID="lblFilter3" runat="server" Text="Filter 3:" Visible="true"></asp:Label>&nbsp;&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: 400px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:DropDownList ID="cboFilter3" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" Width="99%" Visible="true">
							</asp:DropDownList>
						</td>
						<td style="border: 1px solid #000000; width: 120px; background-color: khaki; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:Label ID="txtText3" runat="server" Text="" Visible="true"></asp:Label>

						</td>
						<td style="width: 2px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>

						<td style="width: 180px; background-color: whitesmoke; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; vertical-align: middle" rowspan="3">
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
						<td style="border: 1px solid #000000; width: 120px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:Label ID="lblFilter1" runat="server" Text="Filter 1:"></asp:Label>&nbsp;&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: 400px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:DropDownList ID="cboFilter1" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" Width="99%">
							</asp:DropDownList>
						</td>
						<td style="border: 1px solid #000000; width: 120px; background-color: khaki; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:Label ID="txtText1" runat="server" Text="" Visible="true"></asp:Label>

						</td>

						<td style="width: 2px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
						<td style="border: 1px solid #000000; width: 120px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:Label ID="lblFilter4" runat="server" Text="Filter 4:"></asp:Label>&nbsp;&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: 400px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:DropDownList ID="cboFilter4" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" Width="99%">
							</asp:DropDownList>
						</td>
						<td style="border: 1px solid #000000; width: 120px; background-color: khaki; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:Label ID="txtText4" runat="server" Text="" Visible="true"></asp:Label>

						</td>
						<td style="width: 2px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>

						<%--L3--%>
						<tr style="height: 26px; background-color: lightgray;">
							<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Main Report:&nbsp;&nbsp;</td>


							<td style="border: 1px solid #000000; width: auto; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								<asp:DropDownList ID="cboMain" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" Width="99%">
								</asp:DropDownList>
							</td>

							<td style="width: 2px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="border: 1px solid #000000; width: 120px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								<asp:Label ID="lblFilter2" runat="server" Text="Filter 2:" Visible="true"></asp:Label>&nbsp;&nbsp;
							
							</td>
							<td style="border: 1px solid #000000; width: 400px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								<asp:DropDownList ID="cboFilter2" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" Width="99%" Visible="true">
								</asp:DropDownList>
							</td>
							<td style="border: 1px solid #000000; width: 120px; background-color: khaki; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								<asp:Label ID="txtText2" runat="server" Text="" Visible="true"></asp:Label>

							</td>
							<td style="width: 2px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
							<td style="border: 1px solid #000000; width: 120px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								<asp:Label ID="lblFilter5" runat="server" Text="Filter 5:" Visible="true"></asp:Label>&nbsp;&nbsp;
							</td>
							<td style="border: 1px solid #000000; width: 400px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								<asp:DropDownList ID="cboFilter5" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" Width="99%" Visible="true">
								</asp:DropDownList>
							</td>
							<td style="border: 1px solid #000000; width: 120px; background-color: khaki; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
								<asp:Label ID="txtText5" runat="server" Text="" Visible="true" ForeColor="Red"></asp:Label>

							</td>
							<td style="width: 2px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>

							<%--L4--%>

							<tr style="height: 26px; background-color: white;">
								<td style="border: 1px solid #000000; width: auto; background-color: white; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="12">
									<asp:Label ID="lblOptLabel" runat="server" Text="Select Option:" Visible="false"></asp:Label>&nbsp;&nbsp;
								<asp:RadioButton ID="RadioButton12" runat="server" Text="ALL" GroupName="RegOpt" Visible="false" />&nbsp;&nbsp;
								<asp:RadioButton ID="RadioButton13" runat="server" Text="On Process" GroupName="RegOpt" Visible="false" />&nbsp;&nbsp;
								<asp:RadioButton ID="RadioButton14" runat="server" Text="Finished Only" GroupName="RegOpt" Visible="false" />
								</td>
							</tr>
							<tr style="height: 2px; background-color: white;">
								<td style="width: auto; background-color: white; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="12"></td>
							</tr>

							<tr style="height: 601px; background-color: floralwhite;">
								<td style="border: 1px solid #000000; width: auto; background-color: floralwhite; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; vertical-align: top;" colspan="12">
									<%-- ### Receivng ### --%>
									<%--Register Summary--%>
									<asp:Panel ID="PregSum" runat="server" Width="99%" Height="575px" Visible="false">
										<asp:GridView ID="dgvRegSum" runat="server" AutoGenerateColumns="False" OnRowDataBound="dgvRegSum_RowDataBound"
											OnRowCreated="dgvRegSum_RowCreated" Font-Names="Segoe UI" Font-Size="small" BackColor="White" BorderColor="#999999" BorderStyle="Solid" AllowSorting="true" BorderWidth="1px" CellPadding="3" GridLines="Vertical" HeaderStyle-BorderColor="Black" ShowHeader="true" ShowFooter="true">
											<AlternatingRowStyle BackColor="#DCDCDC" />
											<Columns>
												<asp:BoundField DataField="docno">
													<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="90px"></ItemStyle>
												</asp:BoundField>

												<asp:BoundField DataField="tc">
													<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="40px"></ItemStyle>
												</asp:BoundField>

												<asp:BoundField DataField="transdate" DataFormatString="{0:yyyy-MM-dd}">
													<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="90px"></ItemStyle>
												</asp:BoundField>

												<asp:TemplateField HeaderText="Vendor">
													<ItemTemplate>
														<asp:Label ID="Label1" runat="server" Text='<%# Bind("venno") %>'></asp:Label>
													</ItemTemplate>
													<ItemStyle HorizontalAlign="Left" Width="400px" />
													<FooterTemplate>
														<div style="text-align: center;">
															<asp:Label ID="lblGTotal" runat="server" Font-Bold="true" BackColor="#CCCCCC" ForeColor="White" Width="100%" Height="100%" />
														</div>
													</FooterTemplate>
													<FooterStyle BorderStyle="Solid" HorizontalAlign="Right" VerticalAlign="Middle" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Qty">
													<ItemTemplate>
														<asp:Label ID="txtQty" runat="server" Text='<%# Bind("qty", "{0:#,##0;(#,##0)}") %>'></asp:Label>
													</ItemTemplate>
													<ItemStyle HorizontalAlign="Right" />
													<FooterTemplate>
														<div style="text-align: right;">
															<asp:Label ID="lblQty" runat="server" Font-Bold="true" BackColor="#CCCCCC" ForeColor="White" Width="100%" Height="100%" />
														</div>
													</FooterTemplate>
													<FooterStyle BorderStyle="Solid" HorizontalAlign="Right" VerticalAlign="Middle" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Wt/Vol">
													<ItemTemplate>
														<asp:Label ID="txtWt" runat="server" Text='<%# Bind("wt", "{0:#,##0.00;(#,##0.00)}") %>'></asp:Label>
													</ItemTemplate>
													<ItemStyle HorizontalAlign="Right" />
													<FooterTemplate>
														<div style="text-align: right;">
															<asp:Label ID="lblWt" runat="server" Font-Bold="true" BackColor="#CCCCCC" ForeColor="White" Width="100%" Height="100%" />
														</div>
													</FooterTemplate>
													<FooterStyle BorderStyle="Solid" HorizontalAlign="Right" VerticalAlign="Middle" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Amount">
													<ItemTemplate>
														<asp:Label ID="txtAmt" runat="server" Text='<%# Bind("amt", "{0:#,##0.00;(#,##0.00)}") %>'></asp:Label>
													</ItemTemplate>
													<ItemStyle HorizontalAlign="Right" />
													<FooterTemplate>
														<div style="text-align: right;">
															<asp:Label ID="lblAmt" runat="server" Font-Bold="true" BackColor="#CCCCCC" ForeColor="White" Width="100%" Height="100%" />
														</div>
													</FooterTemplate>
													<FooterStyle BorderStyle="Solid" HorizontalAlign="Right" VerticalAlign="Middle" />
												</asp:TemplateField>

												<asp:BoundField DataField="plntno">
													<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="200px"></ItemStyle>
												</asp:BoundField>

												<asp:BoundField DataField="mov">
													<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="400px"></ItemStyle>
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
											$('#<%=dgvRegSum.ClientID %>').Scrollable({
												ScrollHeight: 570,
												IsInPanel: true
											});
										});
									</script>

									<%--Prod Reg Detailed--%>
									<asp:Panel ID="PPRegDet" runat="server" Width="99%" Height="575px" Visible="false">
										<asp:GridView ID="dgvProdRegDet" runat="server" AutoGenerateColumns="False" OnRowDataBound="dgvProdRegDet_RowDataBound"
											OnRowCreated="dgvProdRegDet_RowCreated" Font-Names="Segoe UI" Font-Size="small" BackColor="White" BorderColor="#999999" BorderStyle="Solid" AllowSorting="true" BorderWidth="1px" CellPadding="3" GridLines="Vertical" HeaderStyle-BorderColor="Black" ShowHeader="true" ShowFooter="true">
											<AlternatingRowStyle BackColor="#DCDCDC" />
											<Columns>
												<asp:BoundField DataField="plntno">
													<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="200px"></ItemStyle>
												</asp:BoundField>

												<asp:BoundField DataField="mmrrno">
													<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></ItemStyle>
												</asp:BoundField>

												<asp:BoundField DataField="transdate" DataFormatString="{0:yyyy-MM-dd}">
													<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></ItemStyle>
												</asp:BoundField>

												<asp:BoundField DataField="codeno">
													<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></ItemStyle>
												</asp:BoundField>

												<asp:TemplateField HeaderText="Product Description">
													<ItemTemplate>
														<asp:Label ID="Label1" runat="server" Text='<%# Bind("mmdesc") %>'></asp:Label>
													</ItemTemplate>
													<ItemStyle HorizontalAlign="Left" Width="500px" />
													<FooterTemplate>
														<div style="text-align: center;">
															<asp:Label ID="lblGTotal" runat="server" Font-Bold="true" BackColor="#CCCCCC" ForeColor="White" Width="100%" Height="100%" />
														</div>
													</FooterTemplate>
													<FooterStyle BorderStyle="Solid" HorizontalAlign="Right" VerticalAlign="Middle" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Qty">
													<ItemTemplate>
														<asp:Label ID="txtQty" runat="server" Text='<%# Bind("qty", "{0:#,##0;(#,##0)}") %>'></asp:Label>
													</ItemTemplate>
													<ItemStyle HorizontalAlign="Right" />
													<FooterTemplate>
														<div style="text-align: right;">
															<asp:Label ID="lblQty" runat="server" Font-Bold="true" BackColor="#CCCCCC" ForeColor="White" Width="100%" Height="100%" />
														</div>
													</FooterTemplate>
													<FooterStyle BorderStyle="Solid" HorizontalAlign="Right" VerticalAlign="Middle" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Wt/Vol">
													<ItemTemplate>
														<asp:Label ID="txtWt" runat="server" Text='<%# Bind("wt", "{0:#,##0.00;(#,##0.00)}") %>'></asp:Label>
													</ItemTemplate>
													<ItemStyle HorizontalAlign="Right" />
													<FooterTemplate>
														<div style="text-align: right;">
															<asp:Label ID="lblWt" runat="server" Font-Bold="true" BackColor="#CCCCCC" ForeColor="White" Width="100%" Height="100%" />
														</div>
													</FooterTemplate>
													<FooterStyle BorderStyle="Solid" HorizontalAlign="Right" VerticalAlign="Middle" />
												</asp:TemplateField>
												<asp:BoundField DataField="lotno">
													<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="150px"></ItemStyle>
												</asp:BoundField>

												<asp:BoundField DataField="batchno">
													<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></ItemStyle>
												</asp:BoundField>

												<asp:BoundField DataField="dsrstat">
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
											$('#<%=dgvProdRegDet.ClientID %>').Scrollable({
											ScrollHeight: 570,
											IsInPanel: true
										});
									});
									</script>

									<%--MMRR Register per Vendor - Detailed--%>
									<asp:Panel ID="PRpVD" runat="server" Width="99%" Height="575px" Visible="false">
										<asp:GridView ID="dgvPRpVD" runat="server" AutoGenerateColumns="False" OnRowDataBound="dgvPRpVD_RowDataBound" OnRowCreated="dgvPRpVD_RowCreated" Font-Names="Segoe UI" Font-Size="small" BackColor="White" BorderColor="#999999" BorderStyle="Solid" AllowSorting="true" BorderWidth="1px" CellPadding="3" GridLines="Vertical" HeaderStyle-BorderColor="Black" ShowHeader="true" ShowFooter="true">
											<AlternatingRowStyle BackColor="#DCDCDC" />
											<Columns>
												<asp:BoundField DataField="venno">
													<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="70px"></ItemStyle>
												</asp:BoundField>

												<asp:BoundField DataField="venname">
													<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="330px"></ItemStyle>
												</asp:BoundField>

												<asp:BoundField DataField="mmrrno">
													<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></ItemStyle>
												</asp:BoundField>

												<asp:BoundField DataField="transdate" DataFormatString="{0:yyyy-MM-dd}">
													<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></ItemStyle>
												</asp:BoundField>

												<asp:BoundField DataField="codeno">
													<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></ItemStyle>
												</asp:BoundField>

												<asp:TemplateField >
													<ItemTemplate>
														<asp:Label ID="Label1" runat="server" Text='<%# Bind("mmdesc") %>'></asp:Label>
													</ItemTemplate>
													<ItemStyle HorizontalAlign="Left" Width="500px" />
													<FooterTemplate>
														<div style="text-align: center;">
															<asp:Label ID="lblGTotal" runat="server" Font-Bold="true" BackColor="#CCCCCC" ForeColor="White" Width="100%" Height="100%" />
														</div>
													</FooterTemplate>
													<FooterStyle BorderStyle="Solid" HorizontalAlign="Right" VerticalAlign="Middle" />
												</asp:TemplateField>

												<asp:TemplateField >
													<ItemTemplate>
														<asp:Label ID="txtQtyRpVD" runat="server" Text='<%# Bind("qty", "{0:#,##0;(#,##0)}") %>'></asp:Label>
													</ItemTemplate>
													<ItemStyle HorizontalAlign="Right" />
													<FooterTemplate>
														<div style="text-align: right;">
															<asp:Label ID="lblQty" runat="server" Font-Bold="true" BackColor="#CCCCCC" ForeColor="White" Width="100%" Height="100%" />
														</div>
													</FooterTemplate>
													<FooterStyle BorderStyle="Solid" HorizontalAlign="Right" VerticalAlign="Middle" />
												</asp:TemplateField>

												<asp:TemplateField >
													<ItemTemplate>
														<asp:Label ID="txtWtRpVD" runat="server" Text='<%# Bind("wt", "{0:N2}") %>'></asp:Label>
													</ItemTemplate>
													<ItemStyle HorizontalAlign="Right" />
													<FooterTemplate>
														<div style="text-align: right;">
															<asp:Label ID="lblWt" runat="server" Font-Bold="true" BackColor="#CCCCCC" ForeColor="White" Width="100%" Height="100%" />
														</div>
													</FooterTemplate>
													<FooterStyle BorderStyle="Solid" HorizontalAlign="Right" VerticalAlign="Middle" />
												</asp:TemplateField>

												 <asp:BoundField DataField="sp" DataFormatString="{0:N2}">
													<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="100px"></ItemStyle>
												</asp:BoundField>

												<asp:TemplateField >
													<ItemTemplate>
														<asp:Label ID="txtAmtRpVD" runat="server" Text='<%# Bind("amt", "{0:N2}") %>'></asp:Label>
													</ItemTemplate>
													<ItemStyle HorizontalAlign="Right" />
													<FooterTemplate>
														<div style="text-align: right;">
															<asp:Label ID="lblAmt" runat="server" Font-Bold="true" BackColor="#CCCCCC" ForeColor="White" Width="100%" Height="100%" />
														</div>
													</FooterTemplate>
													<FooterStyle BorderStyle="Solid" HorizontalAlign="Right" VerticalAlign="Middle" />
												</asp:TemplateField>

												<asp:BoundField DataField="lotno">
													<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="100px"></ItemStyle>
												</asp:BoundField>

												<asp:BoundField DataField="mov">
													<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="70px"></ItemStyle>
												</asp:BoundField>

												<asp:BoundField DataField="plntno">
													<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="70px"></ItemStyle>
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
											$('#<%=dgvPRpVD.ClientID %>').Scrollable({
												ScrollHeight: 570,
												IsInPanel: true
											});
										});
									</script>

									<%--mm reg per material--%>

									<asp:Panel ID="Panel3" runat="server" Width="99%" Height="575px" Visible="false">
										<asp:GridView ID="dgvMMperMatl" runat="server" AutoGenerateColumns="False" OnRowDataBound="dgvMMperMatl_RowDataBound" OnRowCreated="dgvMMperMatl_RowCreated" Font-Names="Segoe UI" Font-Size="small" BackColor="White" BorderColor="#999999" BorderStyle="Solid" AllowSorting="true" BorderWidth="1px" CellPadding="3" GridLines="Vertical" HeaderStyle-BorderColor="Black" ShowHeader="true" ShowFooter="true">
											<AlternatingRowStyle BackColor="#DCDCDC" />
											<Columns>
												<asp:BoundField DataField="venno">
													<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="70px"></ItemStyle>
												</asp:BoundField>

												<asp:BoundField DataField="venname">
													<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="330px"></ItemStyle>
												</asp:BoundField>

												<asp:BoundField DataField="mmrrno">
													<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></ItemStyle>
												</asp:BoundField>

												<asp:BoundField DataField="transdate" DataFormatString="{0:yyyy-MM-dd}">
													<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></ItemStyle>
												</asp:BoundField>

												<asp:BoundField DataField="codeno">
													<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></ItemStyle>
												</asp:BoundField>

												<asp:TemplateField >
													<ItemTemplate>
														<asp:Label ID="Label1" runat="server" Text='<%# Bind("mmdesc") %>'></asp:Label>
													</ItemTemplate>
													<ItemStyle HorizontalAlign="Left" Width="500px" />
													<FooterTemplate>
														<div style="text-align: center;">
															<asp:Label ID="lblGTotal" runat="server" Font-Bold="true" BackColor="#CCCCCC" ForeColor="White" Width="100%" Height="100%" />
														</div>
													</FooterTemplate>
													<FooterStyle BorderStyle="Solid" HorizontalAlign="Right" VerticalAlign="Middle" />
												</asp:TemplateField>

												<asp:TemplateField >
													<ItemTemplate>
														<asp:Label ID="txtQtyRpVD" runat="server" Text='<%# Bind("qty", "{0:#,##0;(#,##0)}") %>'></asp:Label>
													</ItemTemplate>
													<ItemStyle HorizontalAlign="Right" />
													<FooterTemplate>
														<div style="text-align: right;">
															<asp:Label ID="lblQty" runat="server" Font-Bold="true" BackColor="#CCCCCC" ForeColor="White" Width="100%" Height="100%" />
														</div>
													</FooterTemplate>
													<FooterStyle BorderStyle="Solid" HorizontalAlign="Right" VerticalAlign="Middle" />
												</asp:TemplateField>

												<asp:TemplateField >
													<ItemTemplate>
														<asp:Label ID="txtWtRpVD" runat="server" Text='<%# Bind("wt", "{0:N2}") %>'></asp:Label>
													</ItemTemplate>
													<ItemStyle HorizontalAlign="Right" />
													<FooterTemplate>
														<div style="text-align: right;">
															<asp:Label ID="lblWt" runat="server" Font-Bold="true" BackColor="#CCCCCC" ForeColor="White" Width="100%" Height="100%" />
														</div>
													</FooterTemplate>
													<FooterStyle BorderStyle="Solid" HorizontalAlign="Right" VerticalAlign="Middle" />
												</asp:TemplateField>

												 <asp:BoundField DataField="sp" DataFormatString="{0:N2}">
													<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="100px"></ItemStyle>
												</asp:BoundField>

												<asp:TemplateField >
													<ItemTemplate>
														<asp:Label ID="txtAmtRpVD" runat="server" Text='<%# Bind("amt", "{0:N2}") %>'></asp:Label>
													</ItemTemplate>
													<ItemStyle HorizontalAlign="Right" />
													<FooterTemplate>
														<div style="text-align: right;">
															<asp:Label ID="lblAmt" runat="server" Font-Bold="true" BackColor="#CCCCCC" ForeColor="White" Width="100%" Height="100%" />
														</div>
													</FooterTemplate>
													<FooterStyle BorderStyle="Solid" HorizontalAlign="Right" VerticalAlign="Middle" />
												</asp:TemplateField>

												<asp:BoundField DataField="lotno">
													<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="100px"></ItemStyle>
												</asp:BoundField>

												<asp:BoundField DataField="mov">
													<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="70px"></ItemStyle>
												</asp:BoundField>

												<asp:BoundField DataField="plntno">
													<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="70px"></ItemStyle>
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
											$('#<%=dgvMMperMatl.ClientID %>').Scrollable({
												ScrollHeight: 570,
												IsInPanel: true
											});
										});
									</script>




									<%-- ### Issuance ### --%>
									<%--Register Summary--%>
									<asp:Panel ID="PRegSumIss" runat="server" Width="99%" Height="575px" Visible="false">
									<asp:GridView ID="dgvRegSumIss" runat="server" AutoGenerateColumns="False" OnRowDataBound="dgvRegSumIss_RowDataBound" OnRowCreated="dgvRegSumIss_RowCreated" Font-Names="Segoe UI" Font-Size="small" BackColor="White" BorderColor="#999999" BorderStyle="Solid" AllowSorting="true" BorderWidth="1px" CellPadding="3" GridLines="Vertical" HeaderStyle-BorderColor="Black" ShowHeader="true">
										<AlternatingRowStyle BackColor="#DCDCDC" />
										<Columns>
											<asp:BoundField DataField="dono">
												<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="90px"></ItemStyle>
											</asp:BoundField>
											<asp:BoundField DataField="refdoc">
												<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="90px"></ItemStyle>
											</asp:BoundField>
											<asp:BoundField DataField="transdate" DataFormatString="{0:yyyy-MM-dd}">
												<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="90px"></ItemStyle>
											</asp:BoundField>
											<asp:BoundField DataField="custno">
												<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="400px"></ItemStyle>
											</asp:BoundField>
											<asp:BoundField DataField="shipto">
												<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="400px"></ItemStyle>
											</asp:BoundField>
											<asp:BoundField DataField="smnno">
												<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="90px"></ItemStyle>
											</asp:BoundField>
											<asp:BoundField DataField="pono">
												<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="90px"></ItemStyle>
											</asp:BoundField>
											<asp:BoundField DataField="sono">
												<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="90px"></ItemStyle>
											</asp:BoundField>
											<asp:BoundField DataField="plntno">
												<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="90px"></ItemStyle>
											</asp:BoundField>
											<asp:BoundField DataField="mov">
												<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="90px"></ItemStyle>
											</asp:BoundField>
											<asp:BoundField DataField="dsrstat">
												<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></ItemStyle>
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
										$('#<%=dgvRegSumIss.ClientID %>').Scrollable({
											ScrollHeight: 570,
											IsInPanel: true
										});
									});
								</script>

									<%--Issue to Production--%>
									<asp:Panel ID="PIssToProd" runat="server" Width="99%" Height="575px" Visible="false">
										<asp:GridView ID="dgvIssProdMatl" runat="server" AutoGenerateColumns="False" OnRowDataBound="dgvIssProdMatl_RowDataBound" OnRowCreated="dgvIssProdMatl_RowCreated" Font-Names="Segoe UI" Font-Size="small" BackColor="White" BorderColor="#999999" BorderStyle="Solid" AllowSorting="true" BorderWidth="1px" CellPadding="3" GridLines="Vertical" HeaderStyle-BorderColor="Black" ShowHeader="true" ShowFooter="true">
											<AlternatingRowStyle BackColor="#DCDCDC" />
											<Columns>
												<asp:BoundField DataField="dono">
													<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></ItemStyle>
												</asp:BoundField>

												<asp:BoundField DataField="refdoc">
													<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></ItemStyle>
												</asp:BoundField>

												<asp:BoundField DataField="transdate" DataFormatString="{0:yyyy-MM-dd}">
													<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></ItemStyle>
												</asp:BoundField>

												<asp:BoundField DataField="codeno">
													<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></ItemStyle>
												</asp:BoundField>

												<asp:TemplateField HeaderText="Product Description">
													<ItemTemplate>
														<asp:Label ID="Label1" runat="server" Text='<%# Bind("mmdesc") %>'></asp:Label>
													</ItemTemplate>
													<ItemStyle HorizontalAlign="Left" Width="500px" />
													<FooterTemplate>
														<div style="text-align: center;">
															<asp:Label ID="lblGTotal" runat="server" Font-Bold="true" BackColor="#CCCCCC" ForeColor="White" Width="100%" Height="100%" />
														</div>
													</FooterTemplate>
													<FooterStyle BorderStyle="Solid" HorizontalAlign="Right" VerticalAlign="Middle" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Qty">
													<ItemTemplate>
														<asp:Label ID="txtQty" runat="server" Text='<%# Bind("qty", "{0:#,##0;(#,##0)}") %>'></asp:Label>
													</ItemTemplate>
													<ItemStyle HorizontalAlign="Right" />
													<FooterTemplate>
														<div style="text-align: right;">
															<asp:Label ID="lblQty" runat="server" Font-Bold="true" BackColor="#CCCCCC" ForeColor="White" Width="100%" Height="100%" />
														</div>
													</FooterTemplate>
													<FooterStyle BorderStyle="Solid" HorizontalAlign="Right" VerticalAlign="Middle" />
												</asp:TemplateField>

												<asp:TemplateField HeaderText="Wt/Vol">
													<ItemTemplate>
														<asp:Label ID="txtWt" runat="server" Text='<%# Bind("wt", "{0:#,##0.00;(#,##0.00)}") %>'></asp:Label>
													</ItemTemplate>
													<ItemStyle HorizontalAlign="Right" />
													<FooterTemplate>
														<div style="text-align: right;">
															<asp:Label ID="lblWt" runat="server" Font-Bold="true" BackColor="#CCCCCC" ForeColor="White" Width="100%" Height="100%" />
														</div>
													</FooterTemplate>
													<FooterStyle BorderStyle="Solid" HorizontalAlign="Right" VerticalAlign="Middle" />
												</asp:TemplateField>
												<asp:BoundField DataField="lotno">
													<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="150px"></ItemStyle>
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
											$('#<%=dgvIssProdMatl.ClientID %>').Scrollable({
												ScrollHeight: 570,
												IsInPanel: true
											});
										});
									</script>

									<%--Issue to Production per Material--%>
									<asp:Panel ID="PIssProdMatl" runat="server" Width="99%" Height="575px" Visible="false">
										<asp:GridView ID="dgvIssProdPerMatl" runat="server" AutoGenerateColumns="False" OnRowDataBound="dgvIssProdPerMatl_RowDataBound" OnRowCreated="dgvIssProdPerMatl_RowCreated" Font-Names="Segoe UI" Font-Size="small" BackColor="White" BorderColor="#999999" BorderStyle="Solid" AllowSorting="true" BorderWidth="1px" CellPadding="3" GridLines="Vertical" HeaderStyle-BorderColor="Black" ShowHeader="true" ShowFooter="true">
											<AlternatingRowStyle BackColor="#DCDCDC" />
											<Columns>
												<asp:BoundField DataField="dono">
													<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></ItemStyle>
												</asp:BoundField>

												<asp:BoundField DataField="transdate" DataFormatString="{0:yyyy-MM-dd}">
													<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></ItemStyle>
												</asp:BoundField>

												<asp:BoundField DataField="lotno">
													<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="150px"></ItemStyle>
												</asp:BoundField>

												<asp:TemplateField >
													<ItemTemplate>
														<asp:Label ID="Label1" runat="server" Text='<%# Bind("dsrno") %>'></asp:Label>
													</ItemTemplate>
													<ItemStyle HorizontalAlign="Left" Width="600px" />
													<FooterTemplate>
														<div style="text-align: center;">
															<asp:Label ID="lblGTotal" runat="server" Font-Bold="true" BackColor="#CCCCCC" ForeColor="White" Width="100%" Height="100%" />
														</div>
													</FooterTemplate>
													<FooterStyle BorderStyle="Solid" HorizontalAlign="Right" VerticalAlign="Middle" />
												</asp:TemplateField>
																								
												<asp:TemplateField >
													<ItemTemplate>
														<asp:Label ID="txtWt" runat="server" Text='<%# Bind("wt", "{0:N5}") %>'></asp:Label>
													</ItemTemplate>
													<ItemStyle HorizontalAlign="Right" />
													<FooterTemplate>
														<div style="text-align: right;">
															<asp:Label ID="lblWt" runat="server" Font-Bold="true" BackColor="#CCCCCC" ForeColor="White" Width="100%" Height="100%" />
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
											$('#<%=dgvIssProdPerMatl.ClientID %>').Scrollable({
												ScrollHeight: 570,
												IsInPanel: true
											});
										});
									</script>

									<%--Issuance Summary per Material--%>
									<asp:Panel ID="Pispm" runat="server" Width="99%" Height="575px" Visible="false">
										<asp:GridView ID="dgvISpM" runat="server" AutoGenerateColumns="False" OnRowDataBound="dgvISpM_RowDataBound" OnRowCreated="dgvISpM_RowCreated" Font-Names="Segoe UI" Font-Size="small" BackColor="White" BorderColor="#999999" BorderStyle="Solid" AllowSorting="true" BorderWidth="1px" CellPadding="3" GridLines="Vertical" HeaderStyle-BorderColor="Black" ShowHeader="true" ShowFooter="true">
											<AlternatingRowStyle BackColor="#DCDCDC" />
											<Columns>
												<asp:BoundField DataField="codeno">
													<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="130px"></ItemStyle>
												</asp:BoundField>

												<asp:TemplateField>
													<ItemTemplate>
														<asp:Label ID="Label1" runat="server" Text='<%# Bind("mmdesc") %>'></asp:Label>
													</ItemTemplate>
													<ItemStyle HorizontalAlign="Left" Width="600px" />
													<FooterTemplate>
														<div style="text-align: center;">
															<asp:Label ID="lblGTotal" runat="server" Font-Bold="true" BackColor="#CCCCCC" ForeColor="White" Width="100%" Height="100%" />
														</div>
													</FooterTemplate>
													<FooterStyle BorderStyle="Solid" HorizontalAlign="Right" VerticalAlign="Middle" />
												</asp:TemplateField>

												<asp:TemplateField>
													<ItemTemplate>
														<asp:Label ID="txtQtyPM" runat="server" Text='<%# Bind("qty", "{0:#,##0;(#,##0)}") %>'></asp:Label>
													</ItemTemplate>
													<ItemStyle HorizontalAlign="Right" />
													<FooterTemplate>
														<div style="text-align: right;">
															<asp:Label ID="lblQty" runat="server" Font-Bold="true" BackColor="#CCCCCC" ForeColor="White" Width="120px" Height="100%" />
														</div>
													</FooterTemplate>
													<FooterStyle BorderStyle="Solid" HorizontalAlign="Right" VerticalAlign="Middle" />
												</asp:TemplateField>

												<asp:TemplateField>
													<ItemTemplate>
														<asp:Label ID="txtWtPM" runat="server" Text='<%# Bind("wt", "{0:#,##0.00000;(#,##0.00000)}") %>'></asp:Label>
													</ItemTemplate>
													<ItemStyle HorizontalAlign="Right" />
													<FooterTemplate>
														<div style="text-align: right;">
															<asp:Label ID="lblWt" runat="server" Font-Bold="true" BackColor="#CCCCCC" ForeColor="White" Width="150px" Height="100%" />
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
											$('#<%=dgvISpM.ClientID %>').Scrollable({
												ScrollHeight: 570,
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
		<ajaxToolkit:TabPanel ID="TabPanel2" runat="server" HeaderText="Other Reports">
			<ContentTemplate>
				<center>
					<h1>UNDER CONSTRUCTION</h1>
				</center>

			</ContentTemplate>

		</ajaxToolkit:TabPanel>

		<%--SO Monitoring--%>
		<ajaxToolkit:TabPanel ID="TabPanel3" runat="server" HeaderText="Material To Material">
			<ContentTemplate>
				<table style="width: 100%; font-family: 'Segoe UI'; font-size: small; float: left; background-color: lightgrey; border-spacing: 0px; margin-left: 0px; padding-left: 0px; padding-right: 0px;">
					<%--Line1--%>
					<tr style="height: 26px; background-color: lightgray">
						<td style="border: 1px solid #000000; width: 110px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
						Date From:&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: 120px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:TextBox ID="dpMtoMdate1" runat="server" TextMode="Date" Height="20px" Font-Names="Segoe UI" Font-Size="small" CssClass="DateBox" Width="97%" AutoPostBack="true"></asp:TextBox>
						</td>
						
						<td style="width: 2px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
						
						<td style="border: 1px solid #000000; width: 110px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
						Date To:&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: 120px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:TextBox ID="dpMtoMdate2" runat="server" TextMode="Date" Height="20px" Font-Names="Segoe UI" Font-Size="small" CssClass="DateBox" Width="97%" AutoPostBack="true">
							</asp:TextBox>
						</td>
						
						<td style="width: 2px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
						
						<td style="border: 1px solid #000000; width: 110px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
						Plant:&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: 260px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
						   <asp:DropDownList ID="cboPlntMtoM" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" Width="98%">
							</asp:DropDownList>
						</td>
						<td style="width: 2px; background-color: white; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" ></td>
						<td style="border: 1px solid #000000; width: auto; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="6">
							<asp:Label ID="lblMovMtoM" runat="server" Text="Movement Type" ForeColor="Red" ></asp:Label>
						</td>

					</tr>
					<%--Line2--%>
					<tr style="height: 2px; background-color: lightgray">
						<td style="width: auto; background-color: white; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="15"></td>
					</tr>

					<%--Line3--%>

					<tr style="height: 26px; background-color: lightgray">
						<td style="border: 1px solid #000000;width: 44%; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="8" >&nbsp;&nbsp;LIST&nbsp;&nbsp;
							
						</td>
						<td style="width: 3px; background-color: white; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" ></td>
						<td style="border: 1px solid #000000;width: 55%; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="6" >&nbsp;&nbsp;INPUT&nbsp;&nbsp;
						</td>
					</tr>

					<%--Line4--%>

					<tr style="height: 290px; background-color: lightgray">
						<td style="border: 1px solid #000000; width: 44%; background-color: floralwhite; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;vertical-align:top; " colspan="8" rowspan="6" >
							<asp:Panel ID="Panel5" runat="server" Width="100%" Height="581px">
								<asp:GridView ID="dgvMtoMlist" runat="server" AutoGenerateColumns="False" OnRowDataBound="dgvMtoMlist_RowDataBound" OnRowCreated="dgvMtoMlist_RowCreated" Font-Names="Segoe UI" Font-Size="small" BackColor="White" BorderColor="#999999" BorderStyle="Solid" AllowSorting="true" BorderWidth="1px" CellPadding="3" GridLines="Vertical" HeaderStyle-BorderColor="Black" ShowHeader="true" HeaderStyle-HorizontalAlign="Center">
									<AlternatingRowStyle BackColor="#DCDCDC" />
									<Columns>
										<asp:CommandField ShowSelectButton="true" HeaderText="Select">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="60px"></ItemStyle>
										</asp:CommandField>
										<asp:BoundField DataField="mmrrno">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="110px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="transdate" DataFormatString="{0:yyyy-MM-dd}">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="110px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="tc">
											<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="50px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="pc">
											<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="150px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="stat" >
											<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="150px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="user" >
											<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="150px"></ItemStyle>
										</asp:BoundField>

									</Columns>

									<FooterStyle BackColor="#CCCCCC" ForeColor="Black" BorderStyle="Solid" Height="22px"/>
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
									$('#<%=dgvMtoMlist.ClientID %>').Scrollable({
													ScrollHeight: 560,
													IsInPanel: true
												});
											});
							</script>

						</td>

						<td style="width: 3px; background-color: white; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" rowspan="6"></td>
						<td style="border: 1px solid #000000;width: 55%; background-color: floralwhite; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; vertical-align:top;" colspan="6" >
							<asp:Panel ID="Panel2" runat="server" Width="100%" Height="334px">
								<asp:GridView ID="dgvMtoMin" runat="server" AutoGenerateColumns="False" OnRowDataBound="dgvMtoMin_RowDataBound" OnRowCreated="dgvMtoMin_RowCreated" Font-Names="Segoe UI" Font-Size="small" BackColor="White" BorderColor="#999999" BorderStyle="Solid" AllowSorting="true" BorderWidth="1px" CellPadding="3" GridLines="Vertical" HeaderStyle-BorderColor="Black" ShowHeader="true" HeaderStyle-HorizontalAlign="Center" ShowFooter="true">
									<AlternatingRowStyle BackColor="#DCDCDC" />
									<Columns>
										<asp:TemplateField HeaderText="Code No">
											<ItemTemplate>
												<asp:Label ID="lblCodeNoIn" runat="server" Text='<%# Bind("codeno") %>'></asp:Label>
											</ItemTemplate>
											<ItemStyle HorizontalAlign="Center" Width="110px" />
											<FooterTemplate>
												<div style="text-align: center;">
													<asp:Label ID="lblCodeNo" runat="server" Font-Bold="true" BackColor="#CCCCCC" ForeColor="White" Width="103px" Height="100%" />
												</div>
											</FooterTemplate>
											<FooterStyle BorderStyle="Solid" HorizontalAlign="Right" VerticalAlign="Middle" BackColor="#999999"/>
										</asp:TemplateField>
										<asp:TemplateField HeaderText="MM Description">
											<ItemTemplate>
												<asp:Label ID="Label1" runat="server" Text='<%# Bind("mmdesc") %>'></asp:Label>
											</ItemTemplate>
											<ItemStyle HorizontalAlign="Left" Width="270px" />
											<FooterTemplate>
												<div style="text-align: center;">
													<asp:Label ID="lblGTotal" runat="server" Font-Bold="true" BackColor="#999999" ForeColor="White" Width="265px" Height="100%" />
												</div>
											</FooterTemplate>
											<FooterStyle BorderStyle="Solid" HorizontalAlign="Right" VerticalAlign="Middle" BackColor="#999999"/>
										</asp:TemplateField>
										
										<asp:TemplateField HeaderText="Lot No">
											<ItemTemplate>
												<asp:Label ID="lblLotNoIn" runat="server" Text='<%# Bind("lotno") %>'></asp:Label>
											</ItemTemplate>
											<ItemStyle HorizontalAlign="Center" Width="130px" />
											<FooterTemplate>
												<div style="text-align: center;">
													<asp:Label ID="lblLotNo" runat="server" Font-Bold="true" BackColor="#CCCCCC" ForeColor="White" Width="125px" Height="100%" />
												</div>
											</FooterTemplate>
											<FooterStyle BorderStyle="Solid" HorizontalAlign="Right" VerticalAlign="Middle" BackColor="#999999" />
										</asp:TemplateField>


										<asp:TemplateField HeaderText="Qty">
											<ItemTemplate>
												<asp:Label ID="txtQty" runat="server" Text='<%# Bind("qty", "{0:#,##0;(#,##0)}") %>'></asp:Label>
											</ItemTemplate>
											<ItemStyle HorizontalAlign="Right" />
											<FooterTemplate>
												<div style="text-align: right;">
													<asp:Label ID="lblQty" runat="server" Font-Bold="true" BackColor="WhiteSmoke" ForeColor="Red" Width="110px" Height="100%" />
												</div>
											</FooterTemplate>
											<FooterStyle BorderStyle="Solid" HorizontalAlign="Right" VerticalAlign="Middle" BackColor="WhiteSmoke"/>
										</asp:TemplateField>

										<asp:TemplateField HeaderText="Wt/Vol">
											<ItemTemplate>
												<asp:Label ID="txtWt" runat="server" Text='<%# Bind("wt", "{0:#,##0.00;(#,##0.00)}") %>'></asp:Label>
											</ItemTemplate>
											<ItemStyle HorizontalAlign="Right" />
											<FooterTemplate>
												<div style="text-align: right;">
													<asp:Label ID="lblWt" runat="server" Font-Bold="true" BackColor="WhiteSmoke" ForeColor="Red" Width="110px" Height="100%" />
												</div>
											</FooterTemplate>
											<FooterStyle BorderStyle="Solid" HorizontalAlign="Right" VerticalAlign="Middle" BackColor="WhiteSmoke" />
										</asp:TemplateField>

									</Columns>

									<FooterStyle BackColor="#CCCCCC" ForeColor="Black" BorderStyle="Solid" Height="22px"/>
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
									$('#<%=dgvMtoMin.ClientID %>').Scrollable({
										ScrollHeight: 310,
										IsInPanel: true
									});
								});
							</script>

						</td>

					</tr>

					<%--Line5--%>
							

					<tr style="height: 2px; background-color: lightgray">
						<td style="width: auto; background-color: white; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="15"></td>
					</tr>
						<%--Line6--%>
					<tr style="height: 26px; background-color: lightgray">
						<td style="border: 1px solid #000000;width: auto; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="6">&nbsp;&nbsp;OUTPUT&nbsp;&nbsp;
						</td>
					</tr>

						<%--Line7--%>

					<tr style="height: 285px; background-color: lightgray">
						<td style="border: 1px solid #000000;width: 55%; background-color: floralwhite; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; vertical-align: top;" colspan="6" >
							<asp:Panel ID="Panel1" runat="server" Width="100%" Height="286px">
								<asp:GridView ID="dgvMtoMout" runat="server" AutoGenerateColumns="False" OnRowDataBound="dgvMtoMout_RowDataBound" OnRowCreated="dgvMtoMout_RowCreated" Font-Names="Segoe UI" Font-Size="small" BackColor="White" BorderColor="#999999" BorderStyle="Solid" AllowSorting="true" BorderWidth="1px" CellPadding="3" GridLines="Vertical" HeaderStyle-BorderColor="Black" ShowHeader="true" HeaderStyle-HorizontalAlign="Center" ShowFooter="true">
									<AlternatingRowStyle BackColor="#DCDCDC" />
									<Columns>
										 <asp:TemplateField HeaderText="Code No">
											<ItemTemplate>
												<asp:Label ID="lblCodeNoOut" runat="server" Text='<%# Bind("codeno") %>'></asp:Label>
											</ItemTemplate>
											<ItemStyle HorizontalAlign="Center" Width="110px" />
											<FooterTemplate>
												<div style="text-align: center;">
													<asp:Label ID="lblCodeNo2" runat="server" Font-Bold="true" BackColor="#CCCCCC" ForeColor="White" Width="103px" Height="100%" />
												</div>
											</FooterTemplate>
											<FooterStyle BorderStyle="Solid" HorizontalAlign="Right" VerticalAlign="Middle" BackColor="#999999"/>
										</asp:TemplateField>
										<asp:TemplateField HeaderText="MM Description">
											<ItemTemplate>
												<asp:Label ID="Label1" runat="server" Text='<%# Bind("mmdesc") %>'></asp:Label>
											</ItemTemplate>
											<ItemStyle HorizontalAlign="Left" Width="270px" />
											<FooterTemplate>
												<div style="text-align: center;">
													<asp:Label ID="lblGTotal" runat="server" Font-Bold="true" BackColor="#999999" ForeColor="White" Width="265px" Height="100%" />
												</div>
											</FooterTemplate>
											<FooterStyle BorderStyle="Solid" HorizontalAlign="Right" VerticalAlign="Middle" BackColor="#999999"/>
										</asp:TemplateField>
										
										<asp:TemplateField HeaderText="Lot No">
											<ItemTemplate>
												<asp:Label ID="lblLotNoOut" runat="server" Text='<%# Bind("lotno") %>'></asp:Label>
											</ItemTemplate>
											<ItemStyle HorizontalAlign="Center" Width="130px" />
											<FooterTemplate>
												<div style="text-align: center;">
													<asp:Label ID="lblLotNo2" runat="server" Font-Bold="true" BackColor="#CCCCCC" ForeColor="White" Width="125px" Height="100%" />
												</div>
											</FooterTemplate>
											<FooterStyle BorderStyle="Solid" HorizontalAlign="Right" VerticalAlign="Middle" BackColor="#999999" />
										</asp:TemplateField>


										<asp:TemplateField HeaderText="Qty">
											<ItemTemplate>
												<asp:Label ID="txtQty2" runat="server" Text='<%# Bind("qty", "{0:#,##0;(#,##0)}") %>'></asp:Label>
											</ItemTemplate>
											<ItemStyle HorizontalAlign="Right" />
											<FooterTemplate>
												<div style="text-align: right;">
													<asp:Label ID="lblQty" runat="server" Font-Bold="true" BackColor="WhiteSmoke" ForeColor="Red" Width="110px" Height="100%" />
												</div>
											</FooterTemplate>
											<FooterStyle BorderStyle="Solid" HorizontalAlign="Right" VerticalAlign="Middle" BackColor="WhiteSmoke"/>
										</asp:TemplateField>

										<asp:TemplateField HeaderText="Wt/Vol">
											<ItemTemplate>
												<asp:Label ID="txtWt2" runat="server" Text='<%# Bind("wt", "{0:#,##0.00;(#,##0.00)}") %>'></asp:Label>
											</ItemTemplate>
											<ItemStyle HorizontalAlign="Right" />
											<FooterTemplate>
												<div style="text-align: right;">
													<asp:Label ID="lblWt" runat="server" Font-Bold="true" BackColor="WhiteSmoke" ForeColor="Red" Width="110px" Height="100%" />
												</div>
											</FooterTemplate>
											<FooterStyle BorderStyle="Solid" HorizontalAlign="Right" VerticalAlign="Middle" BackColor="WhiteSmoke"/>
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
									$('#<%=dgvMtoMout.ClientID %>').Scrollable({
										ScrollHeight:260,
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




</asp:Content>
