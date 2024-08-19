<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/FI.master" CodeBehind="BudgetMonitor.aspx.vb" Inherits="AOS100web.BudgetMonitor" %>

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
			<td style="background-color: #b1bbd7; border-top: solid; border-bottom: solid; border-color: red; border-width: 2px; width: 100%; font-family: 'Segoe UI'; font-size: 14px; text-align: center; min-height: 28px; max-height: 28px; padding-bottom: 1px;" colspan="7">
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
			<td style=" width:auto ; height:26px ;background-color: lightgray; font-family: 'Segoe UI'; font-size: small;" colspan="8">&nbsp;&nbsp;
				For The Year:&nbsp;&nbsp;
				<asp:DropDownList ID="cboYear" runat="server" AutoPostBack="true" Width="80px" Height="22px"
					Font-Names="Segoe UI" Font-Size="small" >
				</asp:DropDownList>
			</td>
		</tr>
		<tr>
			<td style="border: 1px solid #000000; width: auto; background-color: lightgray;" colspan="8"></td>
		</tr>

	</table>

	<ajaxToolkit:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" Style="width: 99%; height: auto;" >
		<ajaxToolkit:TabPanel ID="TabPanel1" runat="server" HeaderText="Monitoring">
			<ContentTemplate>
				<table style="width: 100%; font-family: 'Segoe UI'; font-size: small; float: left; background-color: lightgrey; border-spacing: 0px; margin-left: 0px; padding-left: 0px; padding-right: 0px;">
					<tr style="height: 26px; background-color: lightgray;">
						<td style="border: 1px solid #000000; width: auto; background-color: lightgray; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"colspan="14">&nbsp;&nbsp;&nbsp;For the month:
							&nbsp;
							<asp:DropDownList ID="cboBudMon" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" Width="150px"></asp:DropDownList>&nbsp;&nbsp;&nbsp;
							Date From:
							<asp:Label ID="lblDateBM1" runat="server" Text="yyyy-MM-dd" Width="100px" ForeColor="Red"></asp:Label>
							Date To:
							<asp:Label ID="lblDateBM2" runat="server" Text="yyyy-MM-dd" Width="100px" ForeColor="Red"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
							Cost Center:&nbsp;&nbsp;
							<asp:DropDownList ID="cboCCenterBM" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" Width="350px"></asp:DropDownList>
						</td>
					</tr>

					<tr style="height: 2px; background-color: whitesmoke;">
						<td style="width: auto; background-color: whitesmoke; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"colspan="14">

						</td>
					</tr>

					<tr style="height: 660px; background-color: lightgray;">
						<td style="border: 1px solid #000000; width: 860px; background-color: floralwhite; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; vertical-align:top; "colspan="8">
							<asp:Panel ID="Panel1" runat="server" Width="99%" Height="650px">
								<asp:GridView ID="dgvBudMon" runat="server" AutoGenerateColumns="False" OnRowDataBound="dgvBudMon_RowDataBound" OnRowCreated="dgvBudMon_RowCreated" Font-Names="Segoe UI" Font-Size="small" BackColor="White" BorderColor="#999999" BorderStyle="Solid" AllowSorting="true" BorderWidth="1px" CellPadding="3" GridLines="Vertical" HeaderStyle-BorderColor="Black" ShowHeader="true" HeaderStyle-HorizontalAlign="Center">
									<AlternatingRowStyle BackColor="#DCDCDC" />
									<Columns>
										<asp:CommandField HeaderText="View" ShowSelectButton="true" >
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="60px"></ItemStyle>
										</asp:CommandField>

										<asp:BoundField DataField="acct" >
											<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="445px" ></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="budamt" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="110px">
											</ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="actamt" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="110px">
											</ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="varamt" DataFormatString="{0:#,##0.00;(#,##0.00)}">
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
									$('#<%=dgvBudMon.ClientID %>').Scrollable({
										ScrollHeight: 630,
										IsInPanel: true
									});
								});
							</script>

						</td>
						<td style="width: 2px; background-color: whitesmoke; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" rowspan="2"></td>

						<td style="border: 1px solid #000000; width: 640px; background-color: floralwhite; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;vertical-align:top;"colspan="5">
							<asp:Panel ID="Panel2" runat="server" Width="99%" Height="650px">
								<asp:GridView ID="dgvActDet" runat="server" AutoGenerateColumns="False" OnRowDataBound="dgvActDet_RowDataBound" OnRowCreated="dgvActDet_RowCreated" Font-Names="Segoe UI" Font-Size="small" BackColor="White" BorderColor="#999999" BorderStyle="Solid" AllowSorting="true" BorderWidth="1px" CellPadding="3" GridLines="Vertical" HeaderStyle-BorderColor="Black" ShowHeader="true" HeaderStyle-HorizontalAlign="Center">
									<AlternatingRowStyle BackColor="#DCDCDC" />
									<Columns>
										<asp:BoundField DataField="docno" >
											<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="100px" ></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="tc" >
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="40px" ></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="datechange" DataFormatString="{0:yyyy-MM-dd}">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="ven" >
											<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="290px" ></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="amt" DataFormatString="{0:#,##0.00;(#,##0.00)}">
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
									$('#<%=dgvActDet.ClientID %>').Scrollable({
										ScrollHeight: 630,
										IsInPanel: true
									});
								});
							</script>

						</td>
					</tr>

					<tr style="height: 26px; background-color: lightgray;">
						<td style="border: 1px solid #000000; width:520px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="4">
							Total:&nbsp;&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: 110px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:Label ID="lblTotBudStd" runat="server" Text="0.00" ForeColor="Red"></asp:Label>&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: 110px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:Label ID="lblTotBudAct" runat="server" Text="0.00" ForeColor="Red"></asp:Label>&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: 110px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:Label ID="lblTotBudVar" runat="server" Text="0.00" ForeColor="Red"></asp:Label>&nbsp;
						</td>
						<td style="border: 1px solid #000000; width:20px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							
						</td>

						<td style="border: 1px solid #000000; width:520px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="4">
							Total:&nbsp;&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: 110px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:Label ID="lblActAmtDet" runat="server" Text="0.00" ForeColor="Red"></asp:Label>&nbsp;
						</td>
					</tr>

				</table>

			</ContentTemplate>

		</ajaxToolkit:TabPanel>
			
		<ajaxToolkit:TabPanel ID="TabPanel2" runat="server" HeaderText="Reserved Tab">
			<ContentTemplate>
				<center>
					<h1>Under Construction</h1>
				</center>
				
			</ContentTemplate>

		</ajaxToolkit:TabPanel>

		
		<%--<ajaxToolkit:TabPanel ID="TabPanel3" runat="server" HeaderText="Monitoring">
			<ContentTemplate>


			</ContentTemplate>

		</ajaxToolkit:TabPanel>--%>

	</ajaxToolkit:TabContainer>


</asp:Content>
