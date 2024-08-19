<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SD.master" CodeBehind="SOapp.aspx.vb" Inherits="AOS100web.SOapp" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentOfSD" runat="server">
	<style>
		.Grid1 {
			/*width: 1556px;*/
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
			if (confirm("Are you Sure to UPDATE Status?")) {
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
			<td style="background-color: #b1bbd7; border-top: solid; border-bottom: solid; border-color: red; border-width: 2px; width: 100%; font-family: 'Segoe UI'; font-size: 10px; text-align: center; min-height: 28px; max-height: 28px; padding-bottom: 1px; padding-top: 1px;"
				colspan="4">
				<asp:LinkButton ID="lbNew" runat="server" OnClick="lbNew_Click" Style="text-decoration: none; background-color: #7fc6f6; font-family: Tahoma; font-size: 14px; color: white; margin-right: 5px; padding-left: 5px; padding-right: 5px; min-height: 22px;"
					BorderStyle="Solid" ToolTip="Reset Fields" CssClass="StdBottomL">
					<asp:Image runat="server" imageurl="~/images/new_16.png" style="vertical-align: middle"/>&nbsp;New
				</asp:LinkButton>
				<asp:LinkButton ID="lbSave" runat="server" OnClick="OnConfirm" OnClientClick="Confirm()" Style="text-decoration: none; background-color: #7fc6f6; padding-right: 5px; font-family: Tahoma; font-size: 14px; color: white; margin-right: 5px; padding-left: 5px; min-height: 22px;"
					BorderStyle="Solid" ToolTip="Post">
					<asp:Image runat="server" imageurl="~/images/check_16.png" style="vertical-align: middle"/>&nbsp;Post
				</asp:LinkButton>
				<asp:LinkButton ID="lbPrint" runat="server" OnClientClick="target ='_blank';" OnClick="lbPrint_Click" Style="text-decoration: none; background-color: #7fc6f6; padding-right: 5px; font-family: Tahoma; font-size: 14px; color: white; margin-right: 5px; padding-left: 5px; min-height: 22px;"
					BorderStyle="Solid" ToolTip="Print" Enabled="false">
					<asp:Image runat="server" imageurl="~/images/Print_16.png" style="vertical-align: middle"/>&nbsp;Print
				</asp:LinkButton>
				<asp:LinkButton ID="lbDelete" runat="server" OnClick="lbDelete_Click" Style="text-decoration: none; background-color: #7fc6f6; padding-right: 5px; font-family: Tahoma; font-size: 14px; color: white; margin-right: 5px; padding-left: 5px; min-height: 22px;"
					BorderStyle="Solid" ToolTip="Void" Enabled="false">
					<asp:Image runat="server" imageurl="~/images/delete_16.png" style="vertical-align: middle"/>&nbsp;Void
				</asp:LinkButton>
				<asp:LinkButton ID="lbClose" runat="server" OnClick="lbClose_Click" Style="text-decoration: none; background-color: #7fc6f6; font-family: Tahoma; font-size: 14px; color: white; min-height: 22px; padding-left: 5px;"
					BorderStyle="Solid" ToolTip="Close" CssClass="StdBottomR">
					<asp:Image runat="server" imageurl="~/images/Exit_16.png" style="vertical-align: middle"/>&nbsp;Close&nbsp;
				</asp:LinkButton>

				<asp:Label ID="lblUser" runat="server" Text="User" Visible="false"></asp:Label>
				<asp:Label ID="lblGrpUser" runat="server" Text="Grp User" Visible="false"></asp:Label>

			</td>

			<td style="border: 1px solid #000000; min-width: 120px; background-color: lightgray; text-align: center; font-family: 'Segoe UI'; font-size: medium;">TC: &nbsp;
				<asp:Label ID="lblTC" runat="server" Text=""></asp:Label>

			</td>

		</tr>
		<tr>
			<td style="border: 1px solid #000000; width: auto; background-color: lightgray;" colspan="8"></td>
		</tr>
		<%--new code here--%>
		<tr style="height: 26px; background-color: lightgray">
			<td style="border: 1px solid #000000; width: 250px; background-color: lightgray; text-align: left; font-family: Segoe UI; font-size: small;">&nbsp;&nbsp;
				SO No.: &nbsp;&nbsp;
				<asp:TextBox ID="txtSOno" runat="server" Width="150px" Height="20px" Font-Names="Segoe UI"
					Font-Size="small" ReadOnly="true" CssClass="TxtBox"></asp:TextBox>
			</td>

			<td style="border: 1px solid #000000; width: 300px; background-color: lightgray; text-align: left; font-family: 'Segoe UI'; font-size: small;">&nbsp;&nbsp;
				Select New Status: &nbsp;&nbsp;
				<asp:DropDownList ID="cboStat" runat="server" AutoPostBack="true" Width="150px" Height="22px"
					Font-Names="Segoe UI" Font-Size="small">
				</asp:DropDownList>

			</td>

			<td style="border: 1px solid #000000; width: 250px; background-color: lightgray; text-align: left; font-family: 'Segoe UI'; font-size: small;">&nbsp;&nbsp;Posting Date:&nbsp;&nbsp;
				<asp:TextBox ID="dpPostDate" runat="server" TextMode="Date" Width="120px" Height="20px" Font-Names="Segoe UI"
					Font-Size="small" CssClass="DateBox"></asp:TextBox>
			</td>

			<td style="border: 1px solid #000000; width: 400px; background-color: lightgray; text-align: left; font-family: 'Segoe UI'; font-size: small;" colspan="2"></td>

		</tr>

		<tr style="height: 26px; background-color: lightgray">
			<td style="border: 1px solid #000000; width: auto; background-color: lightgray; text-align: left; font-family: 'Segoe UI'; font-size: small;" colspan="5">&nbsp;&nbsp;Customer:&nbsp;&nbsp;
				<asp:Label ID="lblCust" runat="server" Text="Customer" Font-Bold="true" ForeColor="Red"></asp:Label>&nbsp;&nbsp;&nbsp;
			</td>
		</tr>

		<tr style="height: 26px; background-color: lightgray">
			<td style="border: 1px solid #000000; width: 250px; background-color: lightgray; text-align: left; font-family: 'Segoe UI'; font-size: small;">&nbsp;&nbsp;Credit Limit:&nbsp;&nbsp; Php
				<asp:Label ID="lblCRlimit" runat="server" Text="0.00" Font-Bold="true" ForeColor="Red"></asp:Label>&nbsp;&nbsp;
			</td>

			<td style="border: 1px solid #000000; width: 300px; background-color: lightgray; text-align: left; font-family: 'Segoe UI'; font-size: small;">&nbsp;&nbsp;SO Amount:&nbsp;&nbsp; Php
				<asp:Label ID="lblSOamt" runat="server" Text="0.00" Font-Bold="true" ForeColor="Red"></asp:Label>&nbsp;&nbsp;
			</td>

			<td style="border: 1px solid #000000; width: auto; background-color: lightgray; text-align: left; font-family: 'Segoe UI'; font-size: small;">&nbsp;&nbsp;Term:&nbsp;&nbsp;
				<asp:Label ID="lblTerm" runat="server" Text="0" Font-Bold="true" ForeColor="Red"> </asp:Label>Day/s&nbsp;&nbsp;

			</td>

			<td style="border: 1px solid #000000; width: 400px; background-color: lightgray; text-align: left; font-family: 'Segoe UI'; font-size: small;" colspan="2"></td>

		</tr>
		<tr>
			<td style="border: 1px solid #000000; width: auto; background-color: lightgray;" colspan="5"></td>

		</tr>
		<tr>
			<td style="border: 1px solid #000000; width: auto; background-color: red;" colspan="5"></td>
		</tr>
		<tr>
			<td style="border: 1px solid #000000; width: auto; height: auto; background-color: lightgray; text-align: left;" colspan="5">
				<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

				<asp:Panel ID="Panel1" runat="server" Width="100%">

					<asp:GridView ID="DgvSOdet" runat="server" AutoGenerateColumns="False" OnRowDataBound="DgvSOdet_RowDataBound"
						OnRowCreated="DgvSOdet_RowCreated" Font-Names="Segoe UI" Font-Size="small" BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" GridLines="Vertical" HeaderStyle-BorderColor="Black">
						<AlternatingRowStyle BackColor="#DCDCDC" />
						<Columns>
							<asp:CommandField ShowSelectButton="true" HeaderText="Select">
								<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="60px"></ItemStyle>
							</asp:CommandField>

							<asp:BoundField DataField="sono" HeaderText="SO No.">
								<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="80px"></ItemStyle>
							</asp:BoundField>

							<asp:BoundField DataField="transdate" HeaderText="SO Date" DataFormatString="{0:yyyy-MM-dd}">
								<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></ItemStyle>
							</asp:BoundField>

							<asp:BoundField DataField="custno" HeaderText="Customer">
								<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="300px"></ItemStyle>
							</asp:BoundField>

							<asp:BoundField DataField="soamt" HeaderText="SO Amount" DataFormatString="{0:#,##0.00;(#,##0.00)}">
								<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="100px"></ItemStyle>
							</asp:BoundField>

							<asp:BoundField DataField="status" HeaderText="SO Status">
								<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="120px"></ItemStyle>
							</asp:BoundField>

							<asp:BoundField DataField="totar" HeaderText="Total A/R" DataFormatString="{0:#,##0.00;(#,##0.00)}">
								<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="100px"></ItemStyle>
							</asp:BoundField>

							<asp:BoundField DataField="soamt" HeaderText="SO Amount" DataFormatString="{0:#,##0.00;(#,##0.00)}">
								<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="100px"></ItemStyle>
							</asp:BoundField>

							<asp:BoundField DataField="30days" HeaderText="1-30 Days" DataFormatString="{0:#,##0.00;(#,##0.00)}">
								<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="100px"></ItemStyle>
							</asp:BoundField>

							<asp:BoundField DataField="60days" HeaderText="31-60 Days" DataFormatString="{0:#,##0.00;(#,##0.00)}">
								<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="100px"></ItemStyle>
							</asp:BoundField>

							<asp:BoundField DataField="90days" HeaderText="61-90 Days" DataFormatString="{0:#,##0.00;(#,##0.00)}">
								<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="100px"></ItemStyle>
							</asp:BoundField>

							<asp:BoundField DataField="120days" HeaderText="91-120 Days" DataFormatString="{0:#,##0.00;(#,##0.00)}">
								<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="100px"></ItemStyle>
							</asp:BoundField>

							<asp:BoundField DataField="91over" HeaderText="120 Days Over" DataFormatString="{0:#,##0.00;(#,##0.00)}">
								<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="100px"></ItemStyle>
							</asp:BoundField>

						</Columns>

						<FooterStyle BackColor="#CCCCCC" ForeColor="Black" BorderStyle="Solid" />
						<HeaderStyle BackColor="#007acc" Font-Bold="True" ForeColor="White" />
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
						$('#<%=DgvSOdet.ClientID %>').Scrollable({
							ScrollHeight: 660,
							IsInPanel: true
						});
					});
				</script>


			</td>

		</tr>


	</table>



</asp:Content>
