<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/FI.master" CodeBehind="JVPosting.aspx.vb" Inherits="AOS100web.JVPosting" %>

<%@ Register Assembly="EditableDropDownList" Namespace="EditableControls" TagPrefix="editable" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentOfFinAcctg" runat="server">
	<style type="text/css">
		div {
			z-index: 9999;
		}
	</style>
	<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

	<style>
		.Grid1 {
			margin-left: 1px;
			margin-right: 3px;
			margin-bottom: 2px;
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
			if (confirm("Are you sure to POST Selected JV?")) {
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
			if (confirm("Are you SURE to RESET JV Form?")) {
				confirm_value.value = "Yes";
			} else {
				confirm_value.value = "No";
			}
			document.forms[0].appendChild(confirm_value);
		}
				
	</script>

	<link href="css/admGen.css" rel="stylesheet" />

	<table style="width: 100%; font-family: 'Segoe UI'; float: left; margin-top: 0px; padding-top: 0px">
		<tr>
			<td style="background-color: #b1bbd7; border-top: solid; border-bottom: solid; border-color: red; border-width: 2px; width: 100%; font-family: 'Segoe UI'; font-size: 10px; text-align: center; min-height: 28px; max-height: 28px; padding-bottom: 1px;"
				colspan="9">
				<asp:LinkButton ID="lbNew" runat="server" Style="text-decoration: none; background-color: #7fc6f6; font-family: Tahoma; font-size: 14px; color: white; margin-right: 5px; padding-left: 5px; padding-right: 5px; min-height: 22px;"
					BorderStyle="Solid" ToolTip="Reset Fields" CssClass="StdBottomL">
					<asp:Image runat="server" imageurl="~/images/new_16.png" style="vertical-align: middle"/>&nbsp;New
				</asp:LinkButton>
				<asp:LinkButton ID="lbSave" runat="server" Text="Save " OnClick="OnConfirm" OnClientClick="Confirm()" Style="text-decoration: none; background-color: #7fc6f6; padding-right: 5px; font-family: Tahoma; font-size: 14px; color: white; margin-right: 5px; padding-left: 5px; min-height: 22px;"
					BorderStyle="Solid" ToolTip="Save">
					<asp:Image runat="server" imageurl="~/images/save_orange_16.png" style="vertical-align: middle"/>&nbsp;
				</asp:LinkButton>
				<asp:LinkButton ID="lbPrint" runat="server" OnClientClick="target ='_blank';" Style="text-decoration: none; background-color: #7fc6f6; padding-right: 5px; font-family: Tahoma; font-size: 14px; color: white; margin-right: 5px; padding-left: 5px; min-height: 22px;"
					BorderStyle="Solid" ToolTip="Print" Enabled="false">
					<asp:Image runat="server" imageurl="~/images/Print_16.png" style="vertical-align: middle"/>&nbsp;Print
				</asp:LinkButton>
				<asp:LinkButton ID="lbDelete" runat="server" Style="text-decoration: none; background-color: #7fc6f6; padding-right: 5px; font-family: Tahoma; font-size: 14px; color: white; margin-right: 5px; padding-left: 5px; min-height: 22px;"
					BorderStyle="Solid" ToolTip="Void" Enabled="false">
					<asp:Image runat="server" imageurl="~/images/delete_16.png" style="vertical-align: middle"/>&nbsp;Void
				</asp:LinkButton>
				<asp:LinkButton ID="lbClose" runat="server" Style="text-decoration: none; background-color: #7fc6f6; font-family: Tahoma; font-size: 14px; color: white; min-height: 22px; padding-left: 5px;"
					BorderStyle="Solid" ToolTip="Close" CssClass="StdBottomR">
					<asp:Image runat="server"  imageurl="~/images/Exit_16.png" style="vertical-align: middle"/>&nbsp;Close&nbsp;
				</asp:LinkButton>

				<asp:Label ID="lblUser" runat="server" Text="User" Visible="false"></asp:Label>
				<asp:Label ID="lblGrpUser" runat="server" Text="Grp User" Visible="false"></asp:Label>

			</td>

			<td style="border: 1px solid #000000; min-width: 123px; background-color: lightgray; text-align: center; font-family: 'Segoe UI'; font-size: medium;">TC: &nbsp;
				<asp:Label ID="lblTC" runat="server" Text="00"></asp:Label>
			</td>

		</tr>
		<tr>
			<td style="border: 1px solid #000000; width: auto; background-color: lightgray;" colspan="10"></td>
		</tr>

		<%--line1--%>
		<tr style="height: 26px; ">
			<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; font-family: 'Segoe UI'; font-size: small;">JV No.:&nbsp;&nbsp; 
			</td>
			<td style="border: 1px solid #000000; width: 150px; background-color: lightgray; text-align: center; font-family: 'Segoe UI'; font-size: small;">
				<asp:TextBox ID="txtJVNo" runat="server" Font-Names="Segoe UI" Font-Size="small" CssClass="txtBoxC"></asp:TextBox>
			</td>

			<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right;font-family: 'Segoe UI'; font-size: small;">New Status:&nbsp;&nbsp;
			</td>
			<td style="border: 1px solid #000000; width: 150px; background-color: lightgray; text-align: center; ">
				<asp:DropDownList ID="cboStat" runat="server" Width="98%" Font-Names="Segoe UI" Font-Size="small" Height="22px" >
				</asp:DropDownList>

			</td>
						
			<td style="border: 1px solid #000000; width: 120px; background-color: lightgray; text-align: right; font-family: 'Segoe UI'; font-size: small;">Posting Date:&nbsp;&nbsp;
			</td>

			<td style="border: 1px solid #000000; width: 120px; background-color: lightgray; text-align: left; padding-left: 2px; font-family: 'Segoe UI'; font-size: small;">
				<asp:TextBox ID="dpPostDate" runat="server" TextMode="Date" Height="20px" Font-Names="Segoe UI"	Font-Size="small" AutoPostBack="true" CssClass="DateBox" Width="120px" ></asp:TextBox>

			</td>

			<td style=" width: 400px; text-align: right;" colspan="3">

			</td>
			<td style="width: auto; text-align: right;">

			</td>

		</tr>
		<%--line2--%>
		<tr style="height: 26px; ">
			<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; font-family: 'Segoe UI'; font-size: small;">JV Type:&nbsp;&nbsp; 
			</td>
			<td style="border: 1px solid #000000; width: 150px; background-color: cornsilk; text-align: center; font-family: 'Segoe UI'; font-size: small;">
				<asp:Label ID="lblJVtype2" runat="server" ForeColor="Red"></asp:Label>
			</td>

			<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; font-family: 'Segoe UI'; font-size: small;">Doc. Source:&nbsp;&nbsp;
			</td>
			<td style="border: 1px solid #000000; width: 150px; background-color: cornsilk; text-align: center; right;font-family: 'Segoe UI'; font-size: small;">
				<asp:Label ID="lblJVtype" runat="server" ForeColor="Red"></asp:Label>
			</td>
						
			<td style="border: 1px solid #000000; width: 120px; background-color: cornsilk; text-align: center; font-family: 'Segoe UI'; font-size: small;">
				<asp:Label ID="lblDateFr" runat="server" ForeColor="Red"></asp:Label>
			</td>

			<td style="border: 1px solid #000000; width: 120px; background-color: cornsilk; text-align: center; padding-left: 2px; font-family: 'Segoe UI'; font-size: small;">
				<asp:Label ID="lblDateTo" runat="server" ForeColor="Red"></asp:Label>
			</td>
			<td style=" width: 400px;  text-align: left;" colspan="3">
				<asp:CheckBox ID="CheckBox1" Text="Beg Setup" Visible="false" runat="server" />
			</td>
			<td style="width: auto; text-align: right;">
			</td>

		</tr>
		<%--line3--%>
		<tr style="height: 26px; ">
			<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; vertical-align:top ;font-family: 'Segoe UI'; font-size: small;" rowspan="2">Remarks:&nbsp;&nbsp;
			</td>
			<td style="border: 1px solid #000000; width: 150px; background-color: cornsilk; text-align: left; vertical-align:top ;font-family: 'Segoe UI'; font-size: small;" colspan="3" rowspan="2">&nbsp;&nbsp;
				<asp:Label ID="lblRem" runat="server" ForeColor="Red"></asp:Label>
			</td>
			<td style="border: 1px solid #000000; width: 120px; background-color: lightgray; text-align: right; padding-left: 2px; font-family: 'Segoe UI'; font-size: small;">Date From:&nbsp;&nbsp;
			</td>
			<td style="border: 1px solid #000000; width: auto; background-color: lightgray; text-align: left; padding-left: 2px; font-family: 'Segoe UI'; font-size: small;">
				<asp:TextBox ID="dpFrom" runat="server" TextMode="Date" Height="20px" Font-Names="Segoe UI"
					Font-Size="small" AutoPostBack="true" CssClass="DateBox" Width="120px" ></asp:TextBox>
			</td>
			<td style="border: 1px solid #000000; width: 400px; background-color: khaki; text-align: center; " colspan="3" rowspan="3">Status/Error Message: <
				<asp:Label ID="tssErrorMsg" runat="server" Text="" ForeColor="Red" Font-Size="Medium"></asp:Label> >
			</td>
			<td style="width: 120px; text-align: right;">
			</td>

		</tr>
		<%--line4--%>
		<tr style="height: 26px; ">
			<td style="border: 1px solid #000000; width: 120px; background-color: lightgray; text-align: right; font-family: 'Segoe UI'; font-size: small;">Date To:&nbsp;&nbsp;
			</td>
			<td style="border: 1px solid #000000; width: auto; background-color: lightgray; text-align: left; padding-left: 2px; font-family: 'Segoe UI'; font-size: small;">
				<asp:TextBox ID="dpTo" runat="server" TextMode="Date" Height="20px" Font-Names="Segoe UI" Font-Size="small" CssClass="DateBox" Width="120px"></asp:TextBox>
			</td>

			<%--<td style="width: 400px; text-align: right;" colspan="3">

			</td>--%>
			<td style="width: 120px; text-align: right;">

			</td>

		</tr>
		<%--line5--%>
		<tr style="height: 26px; ">
			<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; font-family: 'Segoe UI'; font-size: small;">Prepared By:&nbsp;&nbsp;
			</td>
			<td style="border: 1px solid #000000; width: 150px; background-color: cornsilk; text-align: center; font-family: 'Segoe UI'; font-size: small;">
				<asp:Label ID="lblPrepBy" runat="server" ForeColor="Red"></asp:Label>&nbsp;&nbsp;
			</td>

			<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right;font-family: 'Segoe UI'; font-size: small;">Date Prepared:&nbsp;&nbsp;
			</td>
			<td style="border: 1px solid #000000; width: 150px; background-color: cornsilk; text-align: center; right;font-family: 'Segoe UI'; font-size: small;">
				<asp:Label ID="lblDatePrep" runat="server" ForeColor="Red"></asp:Label>&nbsp;&nbsp;

			</td>
						
			<td style="border: 1px solid #000000; width: 120px; background-color: lightgray; text-align: right; font-family: 'Segoe UI'; font-size: small;">Status:&nbsp;&nbsp;
			</td>

			<td style="border: 1px solid #000000; width: 120px; background-color: lightgray; text-align: left; padding-left: 2px; font-family: 'Segoe UI'; font-size: small;">
				<asp:DropDownList ID="cboStatList" runat="server" AutoPostBack="true" Width="124px"
					Font-Names="Segoe UI" Font-Size="small" Height="22px">
					<asp:ListItem>PARK</asp:ListItem>
					<asp:ListItem>POST</asp:ListItem>
				</asp:DropDownList>

				<asp:LinkButton ID="lbStatList" runat="server" Style="text-decoration: none; background-color: lightgray; float: right; padding-right: 2px;">
				<asp:Image runat="server" imageurl="~/images/check_16.png" style="vertical-align: middle"/>
				</asp:LinkButton>&nbsp;
			</td>

			<%--<td style="width: 400px; text-align: right;" colspan="3">

			</td>--%>
			<td style="width: auto; text-align: right;">
				
			</td>

		</tr>

		<tr>
			<td style="border: 1px solid #000000; width: auto; background-color: lightgray;" colspan="10"></td>
		</tr>
		
		<tr>
			<td style="border: 1px solid #000000; width: auto; background-color: red;" colspan="10"></td>
		</tr>

		<tr>
			<td style="border: 1px solid #000000; width: auto; height: 298px; background-color: floralwhite; text-align: left; vertical-align: top" colspan="10">
				<asp:Panel ID="Panel2" runat="server" Width="100%">
					<asp:GridView ID="DgvJVhdr" runat="server" AutoGenerateColumns="False" OnRowDataBound="DgvJVhdr_RowDataBound"
							OnRowCreated="DgvJVhdr_RowCreated" Font-Names="Segoe UI" Font-Size="small" BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ShowHeader="true" GridLines="Vertical">
						<AlternatingRowStyle BackColor="#DCDCDC" />
							<Columns>
								<asp:CommandField ShowSelectButton="true" HeaderText="Select">
									<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="60px"></ItemStyle>
								</asp:CommandField>
								<asp:BoundField DataField="jvno" HeaderText="JV No.">
									<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="120px"></ItemStyle>
								</asp:BoundField>
								<asp:BoundField DataField="transdate" HeaderText="Date" DataFormatString="{0:yyyy-MM-dd}">
									<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></ItemStyle>
								</asp:BoundField>
								<asp:BoundField DataField="jvtype" HeaderText="JV TYpe">
									<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="150px"></ItemStyle>
								</asp:BoundField>
								<asp:BoundField DataField="sourcedoc" HeaderText="Doc Source">
									<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="150px"></ItemStyle>
								</asp:BoundField>
								<asp:BoundField DataField="dramt" HeaderText="DR Amount" DataFormatString="{0:#,##0.00;(#,##0.00)}">
									<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="120px"></ItemStyle>
								</asp:BoundField>				
								<asp:BoundField DataField="cramt" HeaderText="CR Amount" DataFormatString="{0:#,##0.00;(#,##0.00)}">
									<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="120px"></ItemStyle>
								</asp:BoundField>
								<asp:BoundField DataField="remarks" HeaderText="Remarks">
									<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="500px"></ItemStyle>
								</asp:BoundField>
								<asp:BoundField DataField="user" HeaderText="Prepared By">
									<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="150px"></ItemStyle>
								</asp:BoundField>
								<asp:BoundField DataField="status" HeaderText="Status">
									<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="150px"></ItemStyle>
								</asp:BoundField>
							</Columns>

							<FooterStyle BackColor="#CCCCCC" ForeColor="Black" BorderStyle="Solid" />
							<HeaderStyle BackColor="#007acc" Font-Bold="True" ForeColor="White" Height="24px"/>
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
						$('#<%=DgvJVhdr.ClientID %>').Scrollable({
							ScrollHeight: 275,
							IsInPanel: true
						});
					});
				</script>
			
			</td>
		</tr>
		<tr>
			<td style="border: 1px solid #000000; width: auto; background-color: lightgray;" colspan="10"></td>
		</tr>

		<tr>
			<td style="border: 1px solid #000000; width: auto; height: 320px; background-color: floralwhite; text-align: left; vertical-align: top; " colspan="10">
				<asp:Panel ID="Panel1" runat="server" Width="100%">
					<asp:GridView ID="DgvJVdet" runat="server" AutoGenerateColumns="False" OnRowDataBound="DgvJVdet_RowDataBound"
							OnRowCreated="DgvJVdet_RowCreated" Font-Names="Segoe UI" Font-Size="small" BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ShowHeader="true" GridLines="Vertical">
						<AlternatingRowStyle BackColor="#DCDCDC" />
							<Columns>
								<asp:BoundField DataField="itmno" HeaderText="Itm No.">
									<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="80px"></ItemStyle>
								</asp:BoundField>
								<asp:BoundField DataField="ccno" HeaderText="Cost Center">
									<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="300px"></ItemStyle>
								</asp:BoundField>

								<asp:BoundField DataField="acctno" HeaderText="GL Account">
									<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="500px"></ItemStyle>
								</asp:BoundField>
								<asp:BoundField DataField="dramt" HeaderText="DR Amount" DataFormatString="{0:#,##0.00;(#,##0.00)}">
									<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="120px"></ItemStyle>
								</asp:BoundField>				
								<asp:BoundField DataField="cramt" HeaderText="CR Amount" DataFormatString="{0:#,##0.00;(#,##0.00)}">
									<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="120px"></ItemStyle>
								</asp:BoundField>
								<asp:BoundField DataField="pk" HeaderText="PK">
									<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="80px"></ItemStyle>
								</asp:BoundField>

								<asp:BoundField DataField="subacct" HeaderText="Sub Acct">
									<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="100px"></ItemStyle>
								</asp:BoundField>

							</Columns>

							<FooterStyle BackColor="#CCCCCC" ForeColor="Black" BorderStyle="Solid" />
							<HeaderStyle BackColor="#007acc" Font-Bold="True" ForeColor="White" Height="24px"/>
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
						$('#<%=DgvJVdet.ClientID %>').Scrollable({
							ScrollHeight: 275,
							IsInPanel: true
						});
					});
				</script>
			</td>
		</tr>
	</table>

</asp:Content>
