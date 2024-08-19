<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MM.master" CodeBehind="InventoryReports.aspx.vb" Inherits="AOS100web.InventoryReports" %>

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
				<asp:Label ID="lblTitle" runat="server" Text="Reports" Font-Size="Larger" Font-Italic="true" ForeColor="Red"></asp:Label>
			</td>
			<td style="border: 1px solid #000000; width: 600px; height: 30px; background-color: lightgray;" colspan="3">&nbsp;&nbsp;
				<asp:Label ID="lblMsg" runat="server" Text="Message Box" Font-Size="Medium" Font-Italic="true" ForeColor="Red"></asp:Label>
			</td>
		</tr>

	</table>

	<ajaxToolkit:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" Style="width: 99%; height: auto;" AutoPostBack="true">
		<ajaxToolkit:TabPanel ID="TabPanel1" runat="server" HeaderText="Inventory Summary">
			<ContentTemplate>
				<table style="width: 100%; font-family: 'Segoe UI'; font-size: small; float: left; background-color: lightgrey; border-spacing: 0px; margin-left: 0px; padding-left: 0px; padding-right: 0px;">
					<%--L1--%>
					<tr style="height: 26px; background-color: lightgray">
						<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Option:&nbsp;&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: 250px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">&nbsp;&nbsp;
							<asp:RadioButton ID="RadioButton1" runat="server" Text="Per Lot No." GroupName="T1" AutoPostBack="true" />
							&nbsp;&nbsp;
							<asp:RadioButton ID="RadioButton2" runat="server" Text="ALL" GroupName="T1" AutoPostBack="true" />
							&nbsp;&nbsp;
							

						</td>
						<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:CheckBox ID="CheckBox10" runat="server" ToolTip="As Of" AutoPostBack="true" />
							<asp:Label ID="Label18" runat="server" Text="Date From:"></asp:Label>&nbsp;&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: auto; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:TextBox ID="dpFrom" runat="server" TextMode="Date" Height="20px" Font-Names="Segoe UI" Font-Size="small" AutoPostBack="true" CssClass="DateBox" Width="120px"></asp:TextBox>
						</td>
						<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:Label ID="Label19" runat="server" Text="Date To:"></asp:Label>&nbsp;&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: auto; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:TextBox ID="dpTo" runat="server" TextMode="Date" Height="20px" Font-Names="Segoe UI" Font-Size="small" AutoPostBack="true" CssClass="DateBox" Width="120px"></asp:TextBox>
						</td>

						<td style="width: 400px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">&nbsp;&nbsp;
						</td>
						<td style="width: 300px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">&nbsp;&nbsp;
						</td>

					</tr>
					<%--middle line--%>
					<tr style="height: auto; background-color: lightgray">
						<td style="width: auto; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="8"></td>
					</tr>
					<%--L2--%>
					<tr style="height: 26px; background-color: lightgray">
						<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Plant:&nbsp;&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: 250px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:DropDownList ID="cboPlnt" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" Width="98%">
							</asp:DropDownList>

						</td>

						<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">MM Type:&nbsp;&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: auto; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="2">
							<asp:DropDownList ID="cboProdType" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" Width="98%">
							</asp:DropDownList>

						</td>
						<td style="border: 1px solid #000000; width: 120px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:Label ID="Label108" runat="server" Text="MMGroup" Visible="false"></asp:Label>&nbsp;&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: auto; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:DropDownList ID="cboMMgrpT2" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" Width="98%" Visible="false">
							</asp:DropDownList>

						</td>

						<td style="width: auto; background-color: whitesmoke; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">&nbsp;&nbsp;
							<asp:LinkButton ID="LbMMgrp" runat="server" Style="text-decoration: none; background-color: whitesmoke; float: left; padding-left: 2px;">
							<asp:Image runat="server" imageurl="~/images/check_16.png" style="vertical-align: middle"/>
							</asp:LinkButton>
						</td>

					</tr>
					<tr style="height: auto; background-color: lightgray">
						<td style="width: auto; background-color: white; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="8"></td>
					</tr>

					<tr style="height: auto; background-color: lightgray">
						<td style="border: 1px solid #000000; height: auto; width: auto; background-color: floralwhite; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="8">
							<asp:Panel ID="Panel1" runat="server" Width="100%" Height="650px">
								<asp:GridView ID="dgvMMinv" runat="server" AutoGenerateColumns="False" OnRowDataBound="dgvMMinv_RowDataBound" OnRowCreated="dgvMMinv_RowCreated" Font-Names="Segoe UI" Font-Size="small" BackColor="White" BorderColor="#999999" BorderStyle="Solid" AllowSorting="true" BorderWidth="1px" CellPadding="3" GridLines="Vertical" HeaderStyle-BorderColor="Black" ShowHeader="true" HeaderStyle-HorizontalAlign="Center">
									<AlternatingRowStyle BackColor="#DCDCDC" />
									<Columns>
										<asp:BoundField DataField="codeno" HeaderText="Code No.">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="110px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="mmdesc" HeaderText="MM Description">
											<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="500px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="lotno" HeaderText="Lot No.">
											<ItemStyle HorizontalAlign="left" VerticalAlign="Middle" Width="150px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="qty_in" HeaderText="Qty In" DataFormatString="{0:#,##0;(#,##0)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="110px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="wt_in" HeaderText="Wt/Vol In" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="110px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="qty_out" HeaderText="Qty Out" DataFormatString="{0:#,##0;(#,##0)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="110px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="wt_out" HeaderText="Wt/Vol Out" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="110px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="qty_bal" HeaderText="Qty Bal" DataFormatString="{0:#,##0;(#,##0)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="110px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="wt_bal" HeaderText="Wt/Vol Bal" DataFormatString="{0:#,##0.00;(#,##0.00)}">
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
									$('#<%=dgvMMinv.ClientID %>').Scrollable({
										ScrollHeight: 620,
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
		<ajaxToolkit:TabPanel ID="TabPanel2" runat="server" HeaderText="Inventory Analysis">
			<ContentTemplate>
				<table style="width: 100%; font-family: 'Segoe UI'; font-size: small; float: left; background-color: lightgrey; border-spacing: 0px; margin-left: 0px; padding-left: 0px; padding-right: 0px;">
					<%--L1--%>
					<tr style="height: 26px; background-color: lightgray">
						<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">As Of:&nbsp;&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:TextBox ID="dpInvDate" runat="server" TextMode="Date" Height="20px" Font-Names="Segoe UI" Font-Size="small" AutoPostBack="true" CssClass="DateBox" Width="120px"></asp:TextBox>
						</td>
						<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Material:&nbsp;&nbsp;
							
						</td>
						<td style="border: 1px solid #000000; width: 150px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:DropDownList ID="cboMMtype" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" Width="98%">
							</asp:DropDownList>
						</td>
						<td style="border: 1px solid #000000; width: 500px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="4">
							<asp:DropDownList ID="cboMMdesc" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" Width="98%">
							</asp:DropDownList>

						</td>
						<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">&nbsp;&nbsp;
							<asp:Label ID="lblCodeNo" runat="server" Text="00000" ForeColor="Red"></asp:Label>
						</td>
						<td style="width: 100px; background-color: white; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" rowspan="3">&nbsp;&nbsp;
							<asp:LinkButton ID="lbExcelT2" runat="server" OnClick="lbExcelT2_Click" Style="text-decoration: none; background-color: #7fc6f6; font-family: Tahoma; font-size: 12px; color: white; min-height: 22px; padding-left: 5px;"
								BorderStyle="Solid" BorderColor="Blue" ToolTip="Export To Excel">
							<asp:Image runat="server" imageurl="~/images/excel-logo_24.png" style="vertical-align: middle"/>&nbsp;Export&nbsp;
							</asp:LinkButton>

						</td>
						<td style="width: 100px; background-color: white; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">&nbsp;&nbsp;
						</td>

					</tr>
					<%--middle line--%>
					<tr style="height: auto; background-color: lightgray">
						<td style="width: auto; background-color: white; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="9"></td>
						<td style="width: auto; background-color: white; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="2"></td>
					</tr>
					<tr style="height: 26px; background-color: lightgray">
						<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Plant:&nbsp;&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: 250px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="2">
							<asp:DropDownList ID="cboPlnt2" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" Width="98%">
							</asp:DropDownList>
						</td>
						<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:Label ID="lblPClabel" runat="server" Text="PC:" Visible="false"></asp:Label>
							&nbsp;
							<asp:DropDownList ID="cboPCinvAna" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" Width="50px" Visible="false">
							</asp:DropDownList>

							&nbsp;&nbsp;&nbsp;&nbsp;
							Lot No:&nbsp;&nbsp;
							
						</td>

						<td style="border: 1px solid #000000; width: 150px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:DropDownList ID="cboLotNo" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" Width="98%">
							</asp:DropDownList>
						</td>
						<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Bals Qty:&nbsp;&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: 150px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:Label ID="lblBalQty" runat="server" Text="0" ForeColor="Red"></asp:Label>
						</td>
						<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Bals Wt/Vol:&nbsp;&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: 150px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:Label ID="lblBalWt" runat="server" Text="0.00" ForeColor="Red"></asp:Label>
						</td>

						<td style="width: 100px; background-color: white; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">&nbsp;&nbsp;
							<asp:Label ID="lblAdmDig" runat="server" Text="0" ForeColor="Red"></asp:Label>
						</td>
					   <%-- <td style="width: 100px; background-color: white; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">&nbsp;&nbsp;
						</td>--%>
					</tr>
					<tr style="height: auto; background-color: lightgray">
						<td style="width: auto; background-color: white; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="11"></td>
					</tr>
				</table>
				<table style="width: 100%; font-family: 'Segoe UI'; font-size: small; float: left; background-color: lightgrey; border-spacing: 0px; margin-left: 0px; padding-left: 0px; padding-right: 0px;">
					<tr style="height: 26px; background-color: lightgray">
						<td style="border: 1px solid #000000; width: 40%; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; vertical-align:middle;" colspan="4">
							<asp:CheckBox ID="CheckBox13" runat="server" AutoPostBack="true" Text="Real Time" visible="false"/>
							&nbsp;&nbsp;&nbsp;&nbsp;
							<asp:RadioButton ID="rbLeft" runat="server" GroupName="Tab2" ToolTip="Export to Excel" />&nbsp;&nbsp;&nbsp;&nbsp;
							(RECEIVING + PRODUCTION + RETURN)
						</td>
						<td style="width: auto; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
						<td style="border: 1px solid #000000; width: 60%; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; vertical-align:middle;" colspan="4">
							<asp:RadioButton ID="rbRight" runat="server" GroupName="Tab2" ToolTip="Export to Excel"/>&nbsp;&nbsp;&nbsp;&nbsp;
							ISSUANCE
						</td>
					</tr>
					<tr style="height: auto; background-color: lightgray">
						<td style="width: auto; background-color: white; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="9"></td>
					</tr>
					<tr style="height: 598px; background-color: lightgray">
						<td style="border: 1px solid #000000; width: 40%; background-color: floralwhite; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; vertical-align:top;" colspan="4">
							<%--Receiving--%>
							<asp:Panel ID="Panel2" runat="server" Width="100%" Height="588px">
								<asp:GridView ID="lvInv" runat="server" AutoGenerateColumns="False" OnRowDataBound="lvInv_RowDataBound" OnRowCreated="lvInv_RowCreated" Font-Names="Segoe UI" Font-Size="small" BackColor="White" BorderColor="#999999" BorderStyle="Solid" AllowSorting="true" BorderWidth="1px" CellPadding="3" GridLines="Vertical" HeaderStyle-BorderColor="Black" ShowHeader="true" HeaderStyle-HorizontalAlign="Center">
									<AlternatingRowStyle BackColor="#DCDCDC" />
									<Columns>
										<asp:BoundField DataField="dono" HeaderText="Doc No.">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField HeaderText="Date" DataField="transdate" DataFormatString="{0:yyyy-MM-dd}">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="qty" HeaderText="Qty" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="110px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="wt" HeaderText="Wt/Vol" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="110px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="tc" HeaderText="TC">
											<ItemStyle HorizontalAlign="center" VerticalAlign="Middle" Width="40px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="uc" HeaderText="UC" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="80px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="pono" HeaderText="F No.">
											<ItemStyle HorizontalAlign="center" VerticalAlign="Middle" Width="50px"></ItemStyle>
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
									$('#<%=lvInv.ClientID %>').Scrollable({
										ScrollHeight: 569,
										IsInPanel: true
									});
								});
							</script>
						</td>
						<td style="width: auto; background-color: white; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
						<td style="border: 1px solid #000000; width: 60%; background-color: floralwhite; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; vertical-align:top;" colspan="4">
							<%--issuance--%>
							<asp:Panel ID="Panel3" runat="server" Width="100%" Height="588px">
								<asp:GridView ID="lvIss" runat="server" AutoGenerateColumns="False" OnRowDataBound="lvIss_RowDataBound" OnRowCreated="lvIss_RowCreated" Font-Names="Segoe UI" Font-Size="small" BackColor="White" BorderColor="#999999" BorderStyle="Solid" AllowSorting="true" BorderWidth="1px" CellPadding="3" GridLines="Vertical" HeaderStyle-BorderColor="Black" ShowHeader="true" HeaderStyle-HorizontalAlign="Center">
									<AlternatingRowStyle BackColor="#DCDCDC" />
									<Columns>
										<asp:BoundField DataField="dono" HeaderText="Doc No.">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField HeaderText="Date" DataField="transdate" DataFormatString="{0:yyyy-MM-dd}">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="qty" HeaderText="Qty" DataFormatString="{0:#,##0;(#,##0)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="110px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="wt" HeaderText="Wt/Vol" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="110px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="tc" HeaderText="TC">
											<ItemStyle HorizontalAlign="center" VerticalAlign="Middle" Width="40px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="rem" HeaderText="Product/Client">
											<ItemStyle HorizontalAlign="left" VerticalAlign="Middle" Width="380px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="mov" HeaderText="MT">
											<ItemStyle HorizontalAlign="center" VerticalAlign="Middle" Width="40px"></ItemStyle>
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
									$('#<%=lvIss.ClientID %>').Scrollable({
										ScrollHeight: 569,
										IsInPanel: true
									});
								});
							</script>

						</td>
					</tr>
					<tr style="height: 26px; background-color: lightgray">
						<td style="border: 1px solid #000000; width: 200px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Total
						</td>
						<td style="border: 1px solid #000000; width: 110px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:Label ID="lblInvQty" runat="server" Text="0" ForeColor="Red"></asp:Label>
						</td>
						<td style="border: 1px solid #000000; width: 110px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:Label ID="lblInvWt" runat="server" Text="0.00" ForeColor="Red"></asp:Label>
						</td>
						<td style="border: 1px solid #000000; width: 150px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
						<%--Divider--%>
						<td style="width: auto; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
						<%--Divider--%>
						<td style="border: 1px solid #000000; width: 198px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Total
						</td>
						<td style="border: 1px solid #000000; width: 107px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:Label ID="lblIssQty" runat="server" Text="0" ForeColor="Red"></asp:Label>
						</td>
						<td style="border: 1px solid #000000; width: 107px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:Label ID="lblIssWt" runat="server" Text="0.00" ForeColor="Red"></asp:Label>
						</td>
						<td style="border: 1px solid #000000; width: 456px; background-color: lightgray; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:Label ID="lblIssRem" runat="server" Text="0.00" ForeColor="Red"></asp:Label>
						</td>
					</tr>
				</table>
			</ContentTemplate>

		</ajaxToolkit:TabPanel>

		<%--SO Monitoring--%>
		<ajaxToolkit:TabPanel ID="TabPanel3" runat="server" HeaderText="Inventory Count">
			<ContentTemplate>
				<center>
					<h1>Not Yet Available</h1>
				</center>



			</ContentTemplate>

		</ajaxToolkit:TabPanel>

		<ajaxToolkit:TabPanel ID="TabPanel4" runat="server" HeaderText="Unserved SO">
			<ContentTemplate>
				<table style="width: 100%; font-family: 'Segoe UI'; font-size: small; float: left; background-color: lightgrey; border-spacing: 0px; margin-left: 0px; padding-left: 0px; padding-right: 0px;">
					<%--L1--%>
					<tr style="height: 26px; background-color: lightgray">
						<td style="border: 1px solid #000000; width: 120px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:CheckBox ID="CheckBox1" runat="server" ToolTip="As Of" AutoPostBack="true" />&nbsp;&nbsp;
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

						<td style="border: 1px solid #000000; width: 80px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Prod Class:&nbsp;&nbsp;
							
						</td>
						<td style="border: 1px solid #000000; width: 170px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:DropDownList ID="cboPClassSO" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" Width="98%">
							</asp:DropDownList>
						</td>

						<td style="width: 2px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>

						<td style="border: 1px solid #000000; width: 300px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="4">
							<asp:DropDownList ID="cboMMGrpSO" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" Width="98%">
							</asp:DropDownList>

						</td>
						<td style="width: 280px; background-color: whitesmoke; text-align: left; vertical-align: middle; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" rowspan="2">&nbsp;&nbsp;&nbsp;&nbsp;

							<asp:LinkButton ID="lbExcelSO" runat="server" OnClick="lbExcelSO_Click" Style="text-decoration: none; background-color: #7fc6f6; font-family: Tahoma; font-size: 12px; color: white; min-height: 22px; padding-left: 5px;"
								BorderStyle="Solid" BorderColor="Blue" ToolTip="Export To Excel">
							<asp:Image runat="server" imageurl="~/images/excel-logo_24.png" style="vertical-align: middle"/>&nbsp;Export&nbsp;
							</asp:LinkButton>

						</td>

						<td style="width: 250px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">&nbsp;&nbsp;
						</td>

					</tr>

					<tr style="height: 2px; background-color: whitesmoke">
						<td style="width: 100%; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="15"></td>

					</tr>

					<tr style="height: 26px; background-color: lightgray">
						<td style="border: 1px solid #000000; width: auto; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="3">
							<asp:RadioButton ID="rbMon1" runat="server" GroupName="Tab4" Text="Salesman" AutoPostBack="true" />
							&nbsp;&nbsp;&nbsp;&nbsp;
							<asp:RadioButton ID="rbMon2" runat="server" GroupName="Tab4" Text="Customer" AutoPostBack="true" />
						</td>

						<td style="border: 1px solid #000000; width: auto; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="5">
							<asp:DropDownList ID="cboSmnCust" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" Width="98%">
							</asp:DropDownList>

						</td>

						<%--<td style="width: 2px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>--%>

						<td style="width: auto; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="7"></td>
					</tr>
					<tr style="height: 2px; background-color: whitesmoke">
						<td style="width: 100%; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="15"></td>
					</tr>

					<tr style="height: auto; background-color: lightgray">
						<td style="height: auto; width: auto; background-color: floralwhite; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="15">
							<ajaxToolkit:TabContainer ID="TabContainer2" runat="server" ActiveTabIndex="0" Style="width: 100%; height: auto;">
								<ajaxToolkit:TabPanel ID="TabPanel5" runat="server" HeaderText="Summary Per SKU">
									<ContentTemplate>
										<asp:Panel ID="Panel4" runat="server" Width="100%" Height="586px">
											<asp:GridView ID="dgvSOmonSum" runat="server" AutoGenerateColumns="False" OnRowDataBound="dgvSOmonSum_RowDataBound" OnRowCreated="dgvSOmonSum_RowCreated" Font-Names="Segoe UI" Font-Size="small" BackColor="White" BorderColor="#999999" BorderStyle="Solid" AllowSorting="true" BorderWidth="1px" CellPadding="3" GridLines="Vertical" HeaderStyle-BorderColor="Black" ShowHeader="true" HeaderStyle-HorizontalAlign="Center">
												<AlternatingRowStyle BackColor="#DCDCDC" />
												<Columns>
													<asp:BoundField DataField="codeno" HeaderText="Code No.">
														<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="120px"></ItemStyle>
													</asp:BoundField>
													<asp:BoundField DataField="mmdesc" HeaderText="Product Description">
														<ItemStyle HorizontalAlign="left" VerticalAlign="Middle" Width="450px"></ItemStyle>
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
												$('#<%=dgvSOmonSum.ClientID %>').Scrollable({
													ScrollHeight: 560,
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
													<td style="width: 720px; background-color: white; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
											</tr>

										</table>

									</ContentTemplate>

								</ajaxToolkit:TabPanel>

								<ajaxToolkit:TabPanel ID="TabPanel6" runat="server" HeaderText="Per SO">
									<ContentTemplate>
										<asp:Panel ID="Panel5" runat="server" Width="100%" Height="586px">
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
													ScrollHeight: 560,
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

		<ajaxToolkit:TabPanel ID="TabPanel7" runat="server" HeaderText="Production Analysis">
			<ContentTemplate>
				<table style="width: 100%; font-family: 'Segoe UI'; font-size: small; float: left; background-color: lightgrey; border-spacing: 0px; margin-left: 0px; padding-left: 0px; padding-right: 0px;">
					<tr style="height: 26px; background-color: lightgray">
						<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
						Date From:&nbsp;&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: 120px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:TextBox ID="dpDatePA1" runat="server" TextMode="Date" Height="20px" Font-Names="Segoe UI" Font-Size="small" CssClass="DateBox" Width="120px" AutoPostBack="true"></asp:TextBox>
						</td>
						
						<td style="width: 2px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
						
						<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
						Date To:&nbsp;&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: 120px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:TextBox ID="dpDatePA2" runat="server" TextMode="Date" Height="20px" Font-Names="Segoe UI" Font-Size="small" CssClass="DateBox" Width="120px" AutoPostBack="true">
							</asp:TextBox>
						</td>
						
						<td style="width: 2px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
						
						<td style="border: 1px solid #000000; width: 150px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
						Report Format:&nbsp;&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: 300px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
						   <asp:DropDownList ID="cboRepFormatPA" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" Width="98%">
							</asp:DropDownList>
						</td>
						
						<td style="width: 2px; background-color: white; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
						
						<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
						Plant:&nbsp;&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: 300px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
						   <asp:DropDownList ID="cboPlntPA" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" Width="98%">
							</asp:DropDownList>
						</td>
						<td style="width: 300px; background-color: white; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
					</tr>
					
					<tr style="height: 2px; background-color: lightgray">
						<td style="width: auto; background-color: white; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="12"></td>
					</tr>

					<tr style="height: 26px; background-color: lightgray">
						<td style="width: auto; background-color: white; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="12">&nbsp;&nbsp;Receiving Type:&nbsp;&nbsp;
							<asp:RadioButton ID="RadioButton8" runat="server" GroupName="Tab4" Text="Manual" />
							&nbsp;&nbsp;&nbsp;&nbsp;
							<asp:RadioButton ID="RadioButton9" runat="server" GroupName="Tab4" Text="Production Worksheet" />
						</td>
						<%--<td style="border: 1px solid #000000; width: auto; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="5">

						</td>--%>
					</tr>
				</table>

				<table style="width: 100%; font-family: 'Segoe UI'; font-size: small; float: left; background-color: lightgrey; border-spacing: 0px; margin-left: 0px; padding-left: 0px; padding-right: 0px;">
					<tr style="height: 24px; background-color: lightgray">
						<td style="border: 1px solid #000000; width: auto; background-color: lightgrey; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="5">
						   RM vs FG
						</td>
						<td style="width: 2px; background-color: white; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" rowspan="7" ></td>

						<td style="border: 1px solid #000000; width: auto; background-color: lightgrey; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="6">
						   Raw Materials
						</td>

					</tr>
					<tr style="height: auto; background-color: lightgray">
						<td style="border: 1px solid #000000; width: auto; height: auto; background-color: floralwhite; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; vertical-align: top;" colspan="5" rowspan="5">
							<%-- gridview dgvRmFg--%>
							<asp:Panel ID="Panel6" runat="server" Width="100%" Height="590px">
								<asp:GridView ID="dgvRmFg" runat="server" AutoGenerateColumns="False" OnRowDataBound="dgvRmFg_RowDataBound" OnRowCreated="dgvRmFg_RowCreated" Font-Names="Segoe UI" Font-Size="small" BackColor="White" BorderColor="#999999" BorderStyle="Solid" AllowSorting="true" BorderWidth="1px" CellPadding="3" GridLines="Vertical" HeaderStyle-BorderColor="Black" ShowHeader="true" HeaderStyle-HorizontalAlign="Center">
									<AlternatingRowStyle BackColor="#DCDCDC" />
									<Columns>
										<asp:CommandField ShowSelectButton="true" HeaderText="Select">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="60px"></ItemStyle>
										</asp:CommandField>
										<asp:BoundField DataField="transdate" HeaderText="Date" DataFormatString="{0:yyyy-MM-dd}">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="107px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="wt_in" HeaderText="RM Total" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="110px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="Wt_out" HeaderText="FG Total" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="110px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="Wt_Bal" HeaderText="Variance" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="110px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="varper" HeaderText="%age" DataFormatString="{0:#,##0.00%;(#,##0.00)%}">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="90px"></ItemStyle>
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
									$('#<%=dgvRmFg.ClientID %>').Scrollable({
													ScrollHeight: 576,
													IsInPanel: true
												});
											});
							</script>

						</td>

						<td style="border: 1px solid #000000; width: auto; height:280px ;background-color: floralwhite; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; vertical-align: top;" colspan="5">
						   <%--gridview dgvRMdet--%>
							<asp:Panel ID="Panel7" runat="server" Width="100%" Height="270px">
								<asp:GridView ID="dgvRMdet" runat="server" AutoGenerateColumns="False" OnRowDataBound="dgvRMdet_RowDataBound" OnRowCreated="dgvRMdet_RowCreated" Font-Names="Segoe UI" Font-Size="small" BackColor="White" BorderColor="#999999" BorderStyle="Solid" AllowSorting="true" BorderWidth="1px" CellPadding="3" GridLines="Vertical" HeaderStyle-BorderColor="Black" ShowHeader="true" HeaderStyle-HorizontalAlign="Center">
									<AlternatingRowStyle BackColor="#DCDCDC" />
									<Columns>
										<asp:BoundField DataField="dono" HeaderText="DO No.">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="mmdesc" HeaderText="Materials">
											<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="415px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="user" HeaderText="Encoded By">
											<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="80px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="lotno" HeaderText="Lot No.">
											<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="128px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="wt" HeaderText="Wt/Vol." DataFormatString="{0:#,##0.0000000;(#,##0.000000)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="120px"></ItemStyle>
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
									$('#<%=dgvRMdet.ClientID %>').Scrollable({
										ScrollHeight: 255,
										IsInPanel: true
									});
								});
							</script>


						</td>

					</tr>

					<tr style="height: 26px; background-color: lightgray">
						<td style="border: 1px solid #000000; width: auto; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="4">
							Total&nbsp;&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: 150px; background-color:  white; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:Label ID="txtTotRMpa" runat="server" Text="0.00" ForeColor="Red"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
						</td>
					</tr>

					 <tr style="height: 2px; background-color: lightgray">
						<td style="width: auto; background-color: white; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="5"></td>
					</tr>

					<tr style="height: 24px; background-color: lightgray">
						<td style="border: 1px solid #000000; width: auto; background-color: lightgrey; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="5">
						   Finished Goods
						</td>

					</tr>

					<tr style="height: auto; background-color: lightgray">
						<td style="border: 1px solid #000000; width: auto; height:265px ;background-color: floralwhite; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; vertical-align: top;" colspan="5">
						 <%--  gridview dgvFGdet--%>
							<asp:Panel ID="Panel8" runat="server" Width="100%" Height="258px">
								<asp:GridView ID="dgvFGdet" runat="server" AutoGenerateColumns="False" OnRowDataBound="dgvFGdet_RowDataBound" OnRowCreated="dgvFGdet_RowCreated" Font-Names="Segoe UI" Font-Size="small" BackColor="White" BorderColor="#999999" BorderStyle="Solid" AllowSorting="true" BorderWidth="1px" CellPadding="3" GridLines="Vertical" HeaderStyle-BorderColor="Black" ShowHeader="true" HeaderStyle-HorizontalAlign="Center">
									<AlternatingRowStyle BackColor="#DCDCDC" />
									<Columns>
										<asp:BoundField DataField="mmrrno" HeaderText="Doc No.">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="80px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="mmdesc" HeaderText="Materials">
											<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="362px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="user" HeaderText="Encoded By">
											<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="80px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="lotno" HeaderText="Lot No.">
											<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="115px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="qty" HeaderText="Qty" DataFormatString="{0:#,##0;(#,##0)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="80px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="wt" HeaderText="Wt/Vol." DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="120px"></ItemStyle>
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
									$('#<%=dgvFGdet.ClientID %>').Scrollable({
										ScrollHeight: 240,
										IsInPanel: true
									});
								});
							</script>


						</td>

					</tr>
					<tr style="height: 26px; background-color: lightgray">
						<td style="border: 1px solid #000000; width: 170px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							Total&nbsp;&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: 110px; background-color:  white; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:Label ID="txtTotRMwt" runat="server" Text="0.00" ForeColor="Red"></asp:Label>&nbsp;&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: 110px; background-color:  white; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:Label ID="txtTotFGwt" runat="server" Text="0.00" ForeColor="Red"></asp:Label>&nbsp;&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: 110px; background-color:  white; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:Label ID="txtTotVarWt" runat="server" Text="0.00" ForeColor="Red"></asp:Label>&nbsp;&nbsp;
						</td>
						 <td style="border: 1px solid #000000; width: 110px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
						   
						</td>
						<td style="border: 1px solid #000000; width: 540px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="3">
							For Lab Total:&nbsp;&nbsp;
							<asp:Label ID="lblLabTotal" runat="server" Text="0.00" ForeColor="Red"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
							Net Var.:&nbsp;&nbsp;
							<asp:Label ID="lblNetVar" runat="server" Text="0.00" ForeColor="Red"></asp:Label>
							&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
							Total&nbsp;&nbsp;
						</td>
												
						<td style="border: 1px solid #000000; width: 80px; background-color:  white; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:Label ID="txtTotFGpaQty" runat="server" Text="0" ForeColor="Red"></asp:Label>&nbsp;&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: 150px; background-color:  white; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:Label ID="txtTotFGpaWt" runat="server" Text="0.00" ForeColor="Red"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
						</td>
					</tr>
				</table>

			</ContentTemplate>

		</ajaxToolkit:TabPanel>

	</ajaxToolkit:TabContainer>

</asp:Content>
