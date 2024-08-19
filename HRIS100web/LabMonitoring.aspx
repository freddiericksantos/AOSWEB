<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MM.master" CodeBehind="LabMonitoring.aspx.vb" Inherits="AOS100web.LabMonitoring" %>

<%@ Register Assembly="EditableDropDownList" Namespace="EditableControls" TagPrefix="editable" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

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

	<ajaxToolkit:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" Style="width: 99%; height: auto;" >
		<ajaxToolkit:TabPanel ID="TabPanel1" runat="server" HeaderText="Monitoring">
			<ContentTemplate>
				<table style="width: 100%; font-family: 'Segoe UI'; font-size: small; float: left; background-color: lightgrey; border-spacing: 0px; margin-left: 0px; padding-left: 0px; padding-right: 0px;">
					<%--L1--%>
					<tr style="height: 26px; background-color: lightgray">
						<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:Label ID="Label18" runat="server" Text="Date From:"></asp:Label>&nbsp;&nbsp;
						</td>

						<td style="border: 1px solid #000000; width: 125px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:TextBox ID="dpFrom" runat="server" TextMode="Date" Height="20px" Font-Names="Segoe UI" Font-Size="small" AutoPostBack="true" CssClass="DateBox" Width="120px"></asp:TextBox>
						</td>

						<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:Label ID="Label19" runat="server" Text="Date To:"></asp:Label>&nbsp;&nbsp;
						</td>

						<td style="border: 1px solid #000000; width: 125px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:TextBox ID="dpTo" runat="server" TextMode="Date" Height="20px" Font-Names="Segoe UI" Font-Size="small" AutoPostBack="true" CssClass="DateBox" Width="120px"></asp:TextBox>
						</td>

						<td style="width: 2px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
						</td>
						<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Plant:&nbsp;&nbsp;
						</td>
						
						<td style="border: 1px solid #000000; width: 400px; height: 26px;background-color: whitesmoke; text-align: center ; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:DropDownList ID="cboPlnt" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" Width="98%">
							</asp:DropDownList>
						</td>

						<td style="width: 150px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">&nbsp;&nbsp;
						</td>

						<td style="width: 250px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">&nbsp;&nbsp;
						</td>

					</tr>
					<%--middle line--%>
					<tr style="height: auto; background-color: lightgray">
						<td style="width: auto; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="9"></td>
					</tr>
					<%--L2--%>
					<tr style="height: 26px; background-color: lightgray">
						<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">MM Group:&nbsp;&nbsp;
						</td>

						<td style="border: 1px solid #000000; width: 350px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="3">
							<asp:DropDownList ID="cboMMgrp" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" Width="98%">
							</asp:DropDownList>

						</td>

						<td style="width: 2px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
						</td>

						<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Product:&nbsp;&nbsp;
						</td>

						<td style="border: 1px solid #000000; width: 400px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:DropDownList ID="cboFG" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" Width="98%">
							</asp:DropDownList>
						</td>

						<td style="border: 1px solid #000000; width: 150px; background-color: floralwhite; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">&nbsp;&nbsp;
						<asp:Label ID="lblCodeNo" runat="server" Text="" ForeColor="Red"></asp:Label>
						</td>

						<td style="width: 250px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
						</td>
											
					</tr>
					<tr style="height: auto; background-color: lightgray">
						<td style="width: auto; background-color: white; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="9"></td>
					</tr>

					<tr style="height: auto; background-color: lightgray">
						<td style="border: 1px solid #000000; height: auto; width: auto; background-color: floralwhite; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="9">
							<asp:Panel ID="Panel1" runat="server" Width="99%" Height="640px">
								<asp:GridView ID="dgvLab" runat="server" AutoGenerateColumns="False" OnRowDataBound="dgvLab_RowDataBound" OnRowCreated="dgvLab_RowCreated" Font-Names="Segoe UI" Font-Size="small" BackColor="White" BorderColor="#999999" BorderStyle="Solid" AllowSorting="true" BorderWidth="1px" CellPadding="3" GridLines="Vertical" HeaderStyle-BorderColor="Black" ShowHeader="true" HeaderStyle-HorizontalAlign="Center">
									<AlternatingRowStyle BackColor="#DCDCDC" />
									<Columns>
										<asp:BoundField DataField="mmrrno" HeaderText="PW No.">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="110px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="mfgdate" HeaderText="Mfg. Date" DataFormatString="{0:yyyy-MM-dd}">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="110px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="expdate" HeaderText="Exp. Date" DataFormatString="{0:yyyy-MM-dd}">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="110px"></ItemStyle>
										</asp:BoundField>
										
										<asp:BoundField DataField="codeno" HeaderText="Code No.">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="110px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="mmdesc" HeaderText="MM Description">
											<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="500px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="dsrno" HeaderText="Lot No.">
											<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="150px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="batchno" HeaderText="Batch No.">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="80px"></ItemStyle>
										</asp:BoundField>
																				
										<asp:BoundField DataField="wt" HeaderText="Wt/Vol (g)" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="110px"></ItemStyle>
										</asp:BoundField>
										
										<asp:BoundField DataField="labstat" HeaderText="Status">
											<ItemStyle HorizontalAlign="left" VerticalAlign="Middle" Width="150px"></ItemStyle>
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
									$('#<%=dgvLab.ClientID %>').Scrollable({
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
		<ajaxToolkit:TabPanel ID="TabPanel2" runat="server" HeaderText="Disposal per Date">
			<ContentTemplate>
				<table style="width: 100%; font-family: 'Segoe UI'; font-size: small; float: left; background-color: lightgrey; border-spacing: 0px; margin-left: 0px; padding-left: 0px; padding-right: 0px;">
					<%--L1--%>
					<tr style="height: 26px; background-color: lightgray">
						<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							Date From: &nbsp;&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: 125px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" >
							<asp:TextBox ID="dpDateT21" runat="server" TextMode="Date" Height="20px" Font-Names="Segoe UI" Font-Size="small" AutoPostBack="true" CssClass="DateBox" Width="120px"></asp:TextBox>
						</td>
						<td style="width: 2px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
						</td>

						<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							Date To: &nbsp;&nbsp;
						</td>

						<td style="border: 1px solid #000000; width: 125px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:TextBox ID="dpDateT22" runat="server" TextMode="Date" Height="20px" Font-Names="Segoe UI" Font-Size="small" AutoPostBack="true" CssClass="DateBox" Width="120px"></asp:TextBox>
						</td>

						<td style="width: 2px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
						</td>

						<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"> MM Group:&nbsp;&nbsp;
							
						</td>

						<td style="border: 1px solid #000000; width: 250px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:DropDownList ID="cboMMgrpT2" runat="server" Font-Names="Segoe UI" Font-Size="small" Width="98%" AutoPostBack="true">
							</asp:DropDownList>
						</td>

						<td style="width: 100px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:LinkButton ID="btnGenerate" runat="server" Style="text-decoration: none; background-color: #7fc6f6; padding-right: 5px; padding-top: 0px; height: 20px; width: 80px;font-family: Tahoma; font-size: 14px; color: white; margin-right: 5px; padding-left: 0px; vertical-align: middle;" BorderStyle="Solid" ToolTip="Process" CssClass="StdButtLarge">
							<asp:Image runat="server" imageurl="~/images/check_16.png" style="vertical-align: middle; " AlternateText=""/>&nbsp; Load
							</asp:LinkButton>

						</td>

						<td style="width: 2px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
						</td>
												
						<td style="width: 500px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							Date Disposed:&nbsp;&nbsp;

							<asp:TextBox ID="dpDispDate" runat="server" TextMode="Date" Height="20px" Font-Names="Segoe UI" Font-Size="small" AutoPostBack="true" CssClass="DateBox" Width="120px"></asp:TextBox>
							&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;

							<asp:CheckBox ID="CheckBox2" runat="server" Text="Select All" AutoPostBack="true"/>
							&nbsp;&nbsp;

						</td>

					</tr>

					<tr style="height: auto; background-color: whitesmoke">
						<td style="width: auto; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="11"></td>
					</tr>

					<tr style="height: auto; background-color: lightgray">
						<td style="border: 1px solid #000000; height: auto; width: auto; background-color: floralwhite; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="90">
							<asp:Panel ID="Panel2" runat="server" Width="99%" Height="670px">
								<asp:GridView ID="dgvT2List" runat="server" AutoGenerateColumns="False" OnRowDataBound="dgvT2List_RowDataBound" OnRowCreated="dgvT2List_RowCreated" Font-Names="Segoe UI" Font-Size="small" BackColor="White" BorderColor="#999999" BorderStyle="Solid" AllowSorting="true" BorderWidth="1px" CellPadding="3" GridLines="Vertical" HeaderStyle-BorderColor="Black" ShowHeader="true" HeaderStyle-HorizontalAlign="Center">
									<AlternatingRowStyle BackColor="#DCDCDC" />
									<Columns>
										<asp:BoundField DataField="mmrrno" HeaderText="PW No.">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="110px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="mfgdate" HeaderText="Mfg. Date" DataFormatString="{0:yyyy-MM-dd}">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="110px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="expdate" HeaderText="Exp. Date" DataFormatString="{0:yyyy-MM-dd}">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="110px"></ItemStyle>
										</asp:BoundField>
										
										<asp:BoundField DataField="codeno" HeaderText="Code No.">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="110px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="mmdesc" HeaderText="MM Description">
											<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="500px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="dsrno" HeaderText="Lot No.">
											<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="150px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="batchno" HeaderText="Batch No.">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="80px"></ItemStyle>
										</asp:BoundField>
																				
										<asp:BoundField DataField="wt" HeaderText="Wt/Vol (g)" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="110px"></ItemStyle>
										</asp:BoundField>
										
										<asp:BoundField DataField="labstat" HeaderText="Status">
											<ItemStyle HorizontalAlign="left" VerticalAlign="Middle" Width="150px"></ItemStyle>
										</asp:BoundField>

										<asp:TemplateField HeaderText="Check" ItemStyle-HorizontalAlign="Center">
											<ItemTemplate>
												<asp:CheckBox ID="CheckBox1" runat="server" />
												
											</ItemTemplate>

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
									$('#<%=dgvT2List.ClientID %>').Scrollable({
										ScrollHeight: 630,
										IsInPanel: true
									});
								});
							</script>
						</td>
					</tr>
				</table>
						

			</ContentTemplate>
		</ajaxToolkit:TabPanel>

		<ajaxToolkit:TabPanel ID="TabPanel3" runat="server" HeaderText="Disposal per Barcode">
			<ContentTemplate>
		
			</ContentTemplate>
		</ajaxToolkit:TabPanel>

		<ajaxToolkit:TabPanel ID="TabPanel4" runat="server" HeaderText="Search">
			<ContentTemplate>
		
			</ContentTemplate>
		</ajaxToolkit:TabPanel>

	</ajaxToolkit:TabContainer>



</asp:Content>
