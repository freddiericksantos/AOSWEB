<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/FI.master" CodeBehind="COSvsASP.aspx.vb" Inherits="AOS100web.COSvsASP" %>

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

	<table style="width: 100%; font-family: 'Segoe UI'; float: left; margin-top: 0px; padding-top: 0px; border-spacing: 0px;">
		<tr>
			<td style="background-color: #b1bbd7; border-top: solid; border-bottom: solid; border-color: red; border-width: 2px; width: 100%; font-family: 'Segoe UI'; font-size: 10px; text-align: center; min-height: 28px; max-height: 28px; padding-bottom: 1px;" colspan="12">
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
			<td style="width: auto; height: 1px; background-color: whitesmoke;" colspan="12"></td>
		</tr>

		<tr>
			<td style="border: 1px solid #000000; width: auto; background-color: lightgray;" colspan="12"></td>
		</tr>
		<tr>
			<td style="width: auto; height: 1px; background-color: whitesmoke;" colspan="12"></td>
		</tr>

		<tr style="height: auto; background-color: lightgray; " >
			<td style="width: 120px; height: 26px; background-color: whitesmoke; text-align:center;">&nbsp;&nbsp;
				<asp:CheckBox ID="CheckBox2" runat="server" Text="Monthly" AutoPostBack="true" Font-Size="Small"/>
			</td>
			<td style="border: 1px solid #000000; width: 120px; height: 26px; background-color: lightgray; text-align:right; font-size:small; padding-left: 0px;">
				Accounting Period:&nbsp;&nbsp;
			</td>
			<td style="border: 1px solid #000000; width: auto; height: 26px; background-color: lightgray; text-align:center; padding-left: 0px;">
			   <asp:TextBox ID="dpTransDate" runat="server" TextMode="Date" Height="20px" Font-Names="Segoe UI" Font-Size="small" AutoPostBack="true" CssClass="DateBox" Width="120px"></asp:TextBox>
			</td>
			<td style="border: 1px solid #000000; width: 30px; height: 26px; background-color: lightgray; text-align:center; padding-left: 0px; font-size: small;">
				To
			</td>
			<td style="border: 1px solid #000000; width: auto; height: 26px; background-color: lightgray; text-align:center; padding-left: 0px;">
			   <asp:TextBox ID="dpTransDate2" runat="server" TextMode="Date" Height="20px" Font-Names="Segoe UI" Font-Size="small" AutoPostBack="true" CssClass="DateBox" Width="120px"></asp:TextBox>
			</td>
			<td style="width: 1000px; height: 26px; background-color: whitesmoke; text-align:center;" colspan="7">
				
			</td>

			</tr>
			<tr>
				<td style="width: auto; height: 1px; background-color: whitesmoke;" colspan="12"></td>
			</tr>

	</table>
	<br />
	<ajaxToolkit:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" Style="width: 99%; height: auto;" >
		<ajaxToolkit:TabPanel ID="TabPanel1" runat="server" HeaderText="Real Time Costing-Production">
			<ContentTemplate>
				<table style="width: 100%; font-family: 'Segoe UI'; font-size: small; float: left; background-color: lightgrey; border-spacing: 0px; margin-left: 0px; padding-left: 0px; padding-right: 0px;">
					<tr style="height: 26px; background-color: lightgray;">
						<td style="border: 1px solid #000000; width: 1500px; background-color: lightgray; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="12">&nbsp;&nbsp;Raw Material Input:

						</td>
					</tr>
					<tr style="height: auto; background-color: lightgray;">
						<td style="border: 1px solid #000000; width: 1000px; background-color: floralwhite; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; vertical-align:top;" colspan="8" rowspan="12">
							<asp:Panel ID="Panel2" runat="server" Width="100%" Height="308px">
								<asp:GridView ID="dgvRMlist" runat="server" AutoGenerateColumns="False" OnRowDataBound="dgvRMlist_RowDataBound" OnRowCreated="dgvRMlist_RowCreated" Font-Names="Segoe UI" Font-Size="Small" BackColor="White" BorderColor="#999999" BorderStyle="Solid" AllowSorting="True" BorderWidth="1px" CellPadding="3" GridLines="Vertical">
									<AlternatingRowStyle BackColor="Gainsboro" />
									<Columns>
										<asp:BoundField DataField="mmdesc" HeaderText="Materials">
											<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="780px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="auc" HeaderText="Ave. UC" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="110px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="awt" HeaderText="Wt/Vol" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="103px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="amt" HeaderText="Amount" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="113px"></ItemStyle>
										</asp:BoundField>
									</Columns>

									<FooterStyle BackColor="#CCCCCC" ForeColor="Black" BorderStyle="Solid" />
									<HeaderStyle BackColor="#007ACC" Font-Bold="True" ForeColor="White" Height="22px" BorderColor="Black" HorizontalAlign="Center" />
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
									$('#<%=dgvRMlist.ClientID %>').Scrollable({
										ScrollHeight: 283,
										IsInPanel: true
									});
								});
							</script>

						</td>
						<td style="width: 5px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" >

						</td>
						<td style="width: 100px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" >

						</td>
						<td style="width: 250px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="2" >

						</td>
					</tr>

					<tr style="height: 26px; background-color: lightgray;">
						<td style="width: 5px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" >

						</td>
						<td style="border: 1px solid #000000; width: 100px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" >Prod Type:&nbsp;&nbsp;

						</td>
						<td style="border: 1px solid #000000; width: 250px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="2" >
							<asp:Label ID="lblPCtype" runat="server" Text="Manufactured" ForeColor="Red"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
							<asp:RadioButton ID="RadioButton1" Text="Produce" GroupName="PC" runat="server" />
						</td>
					</tr>
					<tr style="height: 26px; background-color: lightgray;">
						<td style="width: 5px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" >

						</td>
						<td style="border: 1px solid #000000; width: 100px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" >Prod Class:&nbsp;&nbsp;

						</td>
						<td style="border: 1px solid #000000; width: 250px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="2" >
							<asp:DropDownList ID="cboPClass" runat="server" AutoPostBack="True" Font-Names="Segoe UI" Font-Size="Small" Width="98%">
							</asp:DropDownList>
						</td>
					</tr>
					<tr style="height: 26px; background-color: lightgray;">
						<td style="width: 5px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" >

						</td>
						<td style="border: 1px solid #000000; width: 350px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="3" >
							<asp:Label ID="lblCCenter" runat="server" ForeColor="Red"></asp:Label>
						</td>
					</tr>
					<tr style="height: 26px; background-color: lightgray;">
						<td style="width: 5px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" >

						</td>
						<td style="border: 1px solid #000000; width: 350px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="3" >
							<asp:Label ID="lblProdnCCNo" runat="server" ForeColor="Red"></asp:Label>
						</td>
					</tr>
					<tr style="height: 26px; background-color: lightgray;">
						<td style="width: 5px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" >

						</td>
						<td style="width: 350px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="3" >
							<asp:Label ID="Label5" runat="server" ForeColor="Red"></asp:Label>
						</td>
					</tr>
					<tr style="height: 26px; background-color: lightgray;">
						<td style="width: 5px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" >

						</td>
						<td style="border: 1px solid #000000;width: 350px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="3" >
							<asp:Label ID="lblProcStatus" runat="server" ForeColor="Red"></asp:Label>
						</td>
					</tr>
					<tr style="height: 26px; background-color: lightgray;">
						<td style="width: 5px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" >

						</td>
						<td style="width: 100px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" >

						</td>
						<td style="width: 250px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="2" >

						</td>
					</tr>
					<tr style="height: 26px; background-color: lightgray;">
						<td style="width: 5px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" >

						</td>
						<td style="border: 1px solid #000000; width: 200px; background-color: lightsteelblue; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="2" >Total OH:&nbsp;&nbsp;

						</td>
						<td style="border: 1px solid #000000; width: 150px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" >
							<asp:Label ID="txtOHamt" runat="server" Text="0.00" ForeColor="Black" Font-Size="Small"></asp:Label>
						</td>
					</tr>
					<tr style="height: 26px; background-color: lightgray;">
						<td style="width: 5px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" >

						</td>
						<td style="border: 1px solid #000000; width: 200px; background-color: lightsteelblue; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="2" >DL / OH per Unit:&nbsp;&nbsp;

						</td>
						<td style="border: 1px solid #000000; width: 150px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" >
							<asp:Label ID="txtOHperWt" runat="server" Text="0.00" ForeColor="Black" Font-Size="Small"></asp:Label>
						</td>
					</tr>
					<tr style="height: 26px; background-color: lightgray;">
						<td style="width: 5px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" >

						</td>
						<td style="border: 1px solid #000000; width: 200px; background-color: lightsteelblue; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="2" >Total F/G Wt.:&nbsp;&nbsp;

						</td>
						<td style="border: 1px solid #000000; width: 150px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" >
							<asp:Label ID="txtPronWt" runat="server" Text="0.00" ForeColor="Black" Font-Size="Small"></asp:Label>
						</td>
					</tr>
					<tr style="height: 26px; background-color: lightgray;">
						<td style="width: 5px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" >

						</td>
						<td style="border: 1px solid #000000; width: 200px; background-color: lightsteelblue; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="2" >
							<asp:Label ID="lblFGlabelA" runat="server" Text="Total Act Wt.:" ForeColor="Black" Font-Size="Small"></asp:Label>&nbsp;&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: 150px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" >
							<asp:Label ID="txtProdnWtTot" runat="server" Text="0.00" ForeColor="Black" Font-Size="Small"></asp:Label>
						</td>
					</tr>
					
					<tr style="height: 26px; background-color: lightgray;">
						<td style="border: 1px solid #000000; width: 897px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="5" >Total Raw Materials Used:&nbsp;&nbsp;
						</td>
						<td style="border: 1px solid #000000; background-color: whitesmoke; width: 110px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" >
							<asp:Label ID="txtTotRMwt" runat="server" Text="0.00" ForeColor="Black" Font-Size="Small"></asp:Label>&nbsp;
						</td>
						<td style="border: 1px solid #000000; background-color: whitesmoke; width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" >
							<asp:Label ID="txtRMtotAmt" runat="server" Text="0.00" ForeColor="Black" Font-Size="Small"></asp:Label>&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: 18px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" >

						</td>
						<td style="width: 5px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" >

						</td>
						
						<td style="border: 1px solid #000000; width: 200px; background-color: lightsteelblue; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="2" >Total Wt. Var.(Lab):&nbsp;&nbsp;

						</td>
						<td style="border: 1px solid #000000; width: 150px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" >
							<asp:Label ID="txtWtLab" runat="server" Text="0.00" ForeColor="Black" Font-Size="Small"></asp:Label>
							<asp:Label ID="lblVarAmt" runat="server" Text="0.00" ForeColor="Black" Font-Size="Small" Visible="False"></asp:Label>
						</td>

					</tr>
					<tr style="height: 2px; background-color: whitesmoke;">
						<td style="width: 1500px; background-color: whitesmoke; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="12">
						</td>
					</tr>

				</table>
			   
				<table style="width: 100%; font-family: 'Segoe UI'; font-size: small; float: left; background-color: lightgrey; border-spacing: 0px; margin-left: 0px; padding-left: 0px; padding-right: 0px;">
					<tr style="height: 26px; background-color: lightgray;">
						<td style="border: 1px solid #000000; width: auto; background-color: lightgray; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="15">&nbsp;&nbsp;Finished Goods Manufactured:
						</td>
					</tr>
					<tr style="height: auto; background-color: lightgray;">
						<td style="border: 1px solid #000000; width: auto; height: 289px;background-color: floralwhite; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; vertical-align:top;" colspan="15">
							<asp:Panel ID="Panel1" runat="server" Width="100%" Height="280px">
								<asp:GridView ID="dgvFGlist" runat="server" AutoGenerateColumns="False" OnRowDataBound="dgvFGlist_RowDataBound" OnRowCreated="dgvFGlist_RowCreated" Font-Names="Segoe UI" Font-Size="Small" BackColor="White" BorderColor="#999999" BorderStyle="Solid" AllowSorting="True" BorderWidth="1px" CellPadding="3" GridLines="Vertical">
									<AlternatingRowStyle BackColor="Gainsboro" />
									<Columns>
										<asp:BoundField DataField="codeno">
											<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="100px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="mmdesc">
											<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="548px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="lotno">
											<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="120px"></ItemStyle>
										</asp:BoundField>
									   <asp:BoundField DataField="qty" DataFormatString="{0:#,##0;(#,##0)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="95px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="wt" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="114px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="rmcost" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="112px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="ohcos" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="114px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="totmrcos" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="114px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="avecos" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="114px"></ItemStyle>
										</asp:BoundField>
										
									</Columns>

									<FooterStyle BackColor="#CCCCCC" ForeColor="Black" BorderStyle="Solid" />
									<HeaderStyle BackColor="#007ACC" Font-Bold="True" ForeColor="White" Height="22px" BorderColor="Black" HorizontalAlign="Center" />
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
									$('#<%=dgvFGlist.ClientID %>').Scrollable({
										ScrollHeight: 264,
										IsInPanel: true
									});
								});
							</script>

						</td>
					</tr>
					<tr style="height: 26px; background-color: lightgray;">
						<td style="border: 1px solid #000000; width: 600px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="8">Total Finished Goods Manufactured:&nbsp;&nbsp;
						</td>
						<td style="border: 1px solid #000000; background-color: whitesmoke; width: 100px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" >
							<asp:Label ID="txtQtyTotFG" runat="server" Text="0" ForeColor="Black" Font-Size="Small"></asp:Label>&nbsp;
						</td>
						<td style="border: 1px solid #000000; background-color: whitesmoke; width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" >
							<asp:Label ID="txtTotWtFG" runat="server" Text="0.00" ForeColor="Black" Font-Size="Small"></asp:Label>&nbsp;
						</td>
						<td style="border: 1px solid #000000; background-color: whitesmoke; width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" >
							<asp:Label ID="txtTotRMamtFG" runat="server" Text="0.00" ForeColor="Black" Font-Size="Small"></asp:Label>&nbsp;
						</td>
						<td style="border: 1px solid #000000; background-color: whitesmoke; width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" >
							<asp:Label ID="txtTotCostAmtFG" runat="server" Text="0.00" ForeColor="Black" Font-Size="Small"></asp:Label>&nbsp;
						</td>
						<td style="border: 1px solid #000000; background-color: whitesmoke; width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" >
							<asp:Label ID="txtTotCostAmt" runat="server" Text="0.00" ForeColor="Black" Font-Size="Small"></asp:Label>&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: 120px; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" >
						</td>
						<td style="border: 1px solid #000000; width: 18px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" >
						</td>

					</tr>

				</table>

			</ContentTemplate>

		</ajaxToolkit:TabPanel>
		
		<ajaxToolkit:TabPanel ID="TabPanel2" runat="server" HeaderText="Real Time Costing-Trading">
			<ContentTemplate>
				<table style="width: 100%; font-family: 'Segoe UI'; font-size: small; float: left; background-color: lightgrey; border-spacing: 0px; margin-left: 0px; padding-left: 0px; padding-right: 0px;">
					<tr style="height: 26px; background-color: lightgray;">
						<td style="border: 1px solid #000000; width: 1500px; background-color: lightgray; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="12">&nbsp;&nbsp;Purchases:

						</td>
					</tr>

					<tr style="height: 26px; background-color: lightgray;">
						<td style="border: 1px solid #000000; width: 1000px; background-color: floralwhite; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; vertical-align:top;" colspan="8" rowspan="12">
							<asp:Panel ID="Panel3" runat="server" Width="100%" Height="308px">
								<asp:GridView ID="dgvRMlist2" runat="server" AutoGenerateColumns="False" OnRowDataBound="dgvRMlist2_RowDataBound" OnRowCreated="dgvRMlist2_RowCreated" Font-Names="Segoe UI" Font-Size="small" BackColor="White" BorderColor="#999999" BorderStyle="Solid" AllowSorting="true" BorderWidth="1px" CellPadding="3" GridLines="Vertical" HeaderStyle-BorderColor="Black" ShowHeader="true" HeaderStyle-HorizontalAlign="Center">
									<AlternatingRowStyle BackColor="#DCDCDC" />
									<Columns>
										<asp:BoundField DataField="mmdesc">
											<ItemStyle HorizontalAlign="left" VerticalAlign="Middle" Width="700px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="asp" HeaderText="Ave. UC" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="110px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="aqty" HeaderText="Wt/Vol" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="103px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="awt" HeaderText="Wt/Vol" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="103px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="amt" HeaderText="Amount" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="113px"></ItemStyle>
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
									$('#<%=dgvRMlist2.ClientID %>').Scrollable({
										ScrollHeight: 283,
										IsInPanel: true
									});
								});
							</script>


						</td>
						<td style="width: 5px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" >

						</td>
						<td style="width: 100px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" >

						</td>
						<td style="width: 250px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="2" >

						</td>
					</tr>

					<tr style="height: 26px; background-color: lightgray;">
						<td style="width: 5px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" >

						</td>
						<td style="border: 1px solid #000000; width: 100px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" >Prod Type:&nbsp;&nbsp;

						</td>
						<td style="border: 1px solid #000000; width: 250px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="2" >
							<asp:Label ID="lblPerSOwt" runat="server" Text="Trading" ForeColor="Red"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
							<asp:RadioButton ID="RadioButton2" Text="" GroupName="PC" ForeColor="Red" runat="server" />
						</td>
					</tr>
					<tr style="height: 26px; background-color: lightgray;">
						<td style="width: 5px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" >

						</td>
						<td style="border: 1px solid #000000; width: 100px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" >Prod Class:&nbsp;&nbsp;

						</td>
						<td style="border: 1px solid #000000; width: 250px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="2" >
							<asp:DropDownList ID="cboPClass2" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" Width="98%">
							</asp:DropDownList>
						</td>
					</tr>
					<tr style="height: 26px; background-color: lightgray;">
						<td style="width: 5px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" >

						</td>
						<td style="border: 1px solid #000000; width: 350px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="3" >
							<asp:Label ID="lblCCenter2" runat="server" Text="" ForeColor="Red"></asp:Label>
						</td>
					</tr>
					<tr style="height: 26px; background-color: lightgray;">
						<td style="width: 5px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" >

						</td>
						<td style="border: 1px solid #000000; width: 350px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="3" >
							<asp:Label ID="lblProdnCCNo2" runat="server" Text="" ForeColor="Red"></asp:Label>
						</td>
					</tr>
					<tr style="height: 26px; background-color: lightgray;">
						<td style="width: 5px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" >

						</td>
						<td style="width: 350px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="3" >
							<asp:Label ID="Label3" runat="server" Text="" ForeColor="Red"></asp:Label>
						</td>
					</tr>
					<tr style="height: 26px; background-color: lightgray;">
						<td style="width: 5px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" >

						</td>
						<td style="border: 1px solid #000000;width: 350px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="3" >
							<asp:Label ID="lblProcStatus2" runat="server" Text="" ForeColor="Red"></asp:Label>
						</td>
					</tr>
					<tr style="height: 26px; background-color: lightgray;">
						<td style="width: 5px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" >

						</td>
						<td style="width: 100px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" >

						</td>
						<td style="width: 250px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="2" >

						</td>
					</tr>
					<tr style="height: 26px; background-color: lightgray;">
						<td style="width: 5px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" >

						</td>
						<td style="width: 100px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" >

						</td>
						<td style="width: 250px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="2" >

						</td>
					</tr>
					<tr style="height: 26px; background-color: lightgray;">
						<td style="width: 5px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" >

						</td>
						<td style="width: 100px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" >

						</td>
						<td style="width: 250px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="2" >

						</td>
					</tr>
					<tr style="height: 26px; background-color: lightgray;">
						<td style="width: 5px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" >

						</td>
						<td style="width: 100px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" >

						</td>
						<td style="width: 250px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="2" >

						</td>
					</tr>
					<tr style="height: 26px; background-color: lightgray;">
						<td style="width: 5px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" >

						</td>
						<td style="width: 100px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" >

						</td>
						<td style="width: 250px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="2" >

						</td>
					</tr>
					
					<tr style="height: 26px; background-color: ghostwhite;">
						<td style="border: 1px solid #000000; width: 700px; text-align: right; background-color: lightgray; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="4" >Total Purchases:&nbsp;&nbsp;

						</td>
						<td style="border: 1px solid #000000; width: 110px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" >
							<asp:Label ID="txtTotRMqty2" runat="server" Text="0.00" ForeColor="Black" Font-Size="Small"></asp:Label>&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: 110px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" >
							<asp:Label ID="txtTotRMwt2" runat="server" Text="0.00" ForeColor="Black" Font-Size="Small"></asp:Label>&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: 110px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" >
							<asp:Label ID="txtRMtotAmt2" runat="server" Text="0.00" ForeColor="Black" Font-Size="Small"></asp:Label>&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: 18px; text-align: right; background-color: lightgray; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" >

						</td>
						<td style="width: 5px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" >

						</td>
						<td style="width: 100px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" >

						</td>
						<td style="width: 250px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="2" >

						</td>
					</tr>
					<tr style="height: 2px; background-color: whitesmoke;">
						<td style="width: 1500px; background-color: whitesmoke; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="12">
						</td>
					</tr>

				</table>
			   
				<table style="width: 100%; font-family: 'Segoe UI'; font-size: small; float: left; background-color: lightgrey; border-spacing: 0px; margin-left: 0px; padding-left: 0px; padding-right: 0px;">
					<tr style="height: 26px; background-color: lightgray;">
						<td style="border: 1px solid #000000; width: auto; background-color: lightgray; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="15">&nbsp;&nbsp;Finished Goods Sold:
						</td>
					</tr>
					<tr style="height: auto; background-color: lightgray;">
						<td style="border: 1px solid #000000; width: auto; height: 289px;background-color: floralwhite; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; vertical-align:top;" colspan="15">
							<asp:Panel ID="Panel4" runat="server" Width="99%" Height="270px">
								<asp:GridView ID="dgvFGlist2" runat="server" AutoGenerateColumns="False" OnRowDataBound="dgvFGlist2_RowDataBound" OnRowCreated="dgvFGlist2_RowCreated" Font-Names="Segoe UI" Font-Size="small" BackColor="White" BorderColor="#999999" BorderStyle="Solid" AllowSorting="true" BorderWidth="1px" CellPadding="3" GridLines="Vertical" HeaderStyle-BorderColor="Black" ShowHeader="true" HeaderStyle-HorizontalAlign="Center">
									<AlternatingRowStyle BackColor="#DCDCDC" />
									<Columns>
										<asp:BoundField DataField="codeno">
											<ItemStyle HorizontalAlign="left" VerticalAlign="Middle" Width="100px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="mmdesc">
											<ItemStyle HorizontalAlign="left" VerticalAlign="Middle" Width="563px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="wsno">
											<ItemStyle HorizontalAlign="left" VerticalAlign="Middle" Width="120px"></ItemStyle>
										</asp:BoundField>
									   <asp:BoundField DataField="qty" DataFormatString="{0:#,##0;(#,##0)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="110px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="wt" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="110px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="rmcost" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="102px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="asp" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="104px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="tcost" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="127px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="uc" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="102px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="wtvar" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="100px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="perc" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="90px"></ItemStyle>
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
									$('#<%=dgvFGlist2.ClientID %>').Scrollable({
										ScrollHeight: 264,
										IsInPanel: true
									});
								});
							</script>

						</td>
					</tr>
					<tr style="height: 26px; background-color: ghostwhite;">
						<td style="border: 1px solid #000000; width: 600px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="6">Total Sold:&nbsp;&nbsp;

						</td>
						<td style="border: 1px solid #000000; width: 100px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" >
							<asp:Label ID="txtTotWtFG2" runat="server" Text="0.00" ForeColor="Black" Font-Size="Small"></asp:Label>&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: 100px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" >
							<asp:Label ID="txtTotRMamtFG2" runat="server" Text="0.00" ForeColor="Black" Font-Size="Small"></asp:Label>&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: 100px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" >
							<asp:Label ID="txtTotCostAmtFG2" runat="server" Text="0.00" ForeColor="Black" Font-Size="Small"></asp:Label>&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: 100px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" >
							<asp:Label ID="lblUCpk20" runat="server" Text="" ForeColor="Black" Font-Size="Small"></asp:Label>&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: 120px; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" >
							<asp:Label ID="txtTotCostAmt2" runat="server" Text="0.00" ForeColor="Black" Font-Size="Small"></asp:Label>&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: 240px; background-color: lightgray; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="3">
							<asp:Label ID="lblUCpk2" runat="server" Text="" ForeColor="Black" Font-Size="Small"></asp:Label>&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: 18px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" >
						</td>

					</tr>

				</table>


			</ContentTemplate>

		</ajaxToolkit:TabPanel>

		
		<ajaxToolkit:TabPanel ID="TabPanel3" runat="server" HeaderText="COS vs ASP (Detailed)">
			<ContentTemplate>
			   <table style="width: 100%; font-family: 'Segoe UI'; font-size: small; float: left; background-color: lightgrey; border-spacing: 0px; margin-left: 0px; padding-left: 0px; padding-right: 0px;">
					<tr style="height: 26px; background-color: lightgray;">
						<td style="border: 1px solid #000000; width: 115px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">F/G Group:&nbsp;&nbsp;
						</td>
						 <td style="border: 1px solid #000000; width: 397px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="4">
							 <asp:DropDownList ID="cboMMgrp" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" Width="99%">
							</asp:DropDownList>
						</td>
						 <td style="width: 2px; background-color: whitesmoke; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">

						</td>
						 <td style="width: 800px; background-color: whitesmoke; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="6">

						</td>
					</tr>
				   <tr style="height: 26px; background-color: lightgray;">
						<td style="border: 1px solid #000000; width: 115px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">F/G:&nbsp;&nbsp;
						</td>
						 <td style="border: 1px solid #000000; width: 397px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="4">
							 <asp:DropDownList ID="cboMMdesc" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" Width="99%">
							</asp:DropDownList>
						</td>
						 <td style="width: 2px; background-color: whitesmoke; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">

						</td>
						 <td style="width: 800px; background-color: whitesmoke; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="6">

						</td>
					</tr>
				   <tr style="height: 26px; background-color: lightgray;">
						<td style="border: 1px solid #000000; width: 115px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Code No.:&nbsp;&nbsp;
						</td>
						 <td style="border: 1px solid #000000; width: 140px; background-color: white; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							 <asp:Label ID="txtCodeNo" runat="server" Text="" ForeColor="Black" Font-Size="Small"></asp:Label>&nbsp;
						</td>
						<td style="width: 2px; background-color: whitesmoke; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>

						<td style="border: 1px solid #000000; width: 115px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Base Code No.:&nbsp;&nbsp;
						</td>
						 <td style="border: 1px solid #000000; width: 140px; background-color: white; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							 <asp:Label ID="txtCodeNo2" runat="server" Text="" ForeColor="Black" Font-Size="Small"></asp:Label>&nbsp;
						</td>

						 <td style="width: 2px; background-color: whitesmoke; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">

						</td>
						 <td style="width: 800px; background-color: whitesmoke; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="6">&nbsp;&nbsp;
							 <asp:CheckBox ID="CheckBox3" AutoPostBack="true" Text="Exclude Free/Deal" runat="server" />
						</td>
					</tr>
				   <tr style="height: 2px; background-color: whitesmoke;">
						<td style="width: auto; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="12">
						</td>
					</tr>
				   <tr style="height: 26px; background-color: lightgray;">
						<td style="border: 1px solid #000000; width: auto; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="5">
							<asp:RadioButton ID="RadioButton15" GroupName="FG" AutoPostBack="true" Text="Per Lot Issued" runat="server" />
						   &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
							<asp:Label ID="Label4" runat="server" Text="Finished Goods" ForeColor="Red" Font-Size="Small"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
							<asp:RadioButton ID="RadioButton6" GroupName="FG" AutoPostBack="true" Text="Average" runat="server" />
						</td>
						 <td style="width: 2px; background-color: whitesmoke; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">

						</td>
						 <td style="border: 1px solid #000000; width: 800px; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="6">
							 <asp:Label ID="Label6" runat="server" Text="Customer" ForeColor="Red" Font-Size="Small"></asp:Label>
						</td>
					</tr>
					<tr style="height: 604px; background-color: lightgray;">
						<td style="border: 1px solid #000000; width: auto; background-color: floralwhite; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; vertical-align:top;" colspan="5">
							<asp:Panel ID="Panel5" runat="server" Width="100%" Height="580px">
								<asp:GridView ID="dgvTab1L" runat="server" AutoGenerateColumns="False" OnRowDataBound="dgvTab1L_RowDataBound" OnRowCreated="dgvTab1L_RowCreated" Font-Names="Segoe UI" Font-Size="small" BackColor="White" BorderColor="#999999" BorderStyle="Solid" AllowSorting="true" BorderWidth="1px" CellPadding="3" GridLines="Vertical" HeaderStyle-BorderColor="Black" ShowHeader="true" HeaderStyle-HorizontalAlign="Center">
									<AlternatingRowStyle BackColor="#DCDCDC" />
									<Columns>
										<asp:BoundField DataField="codeno">
											<ItemStyle HorizontalAlign="left" VerticalAlign="Middle" Width="100px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="mmdesc">
											<ItemStyle HorizontalAlign="left" VerticalAlign="Middle" Width="302px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="avecos" DataFormatString="{0:#,##0.00;(#,##0.00)}">
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
									$('#<%=dgvTab1L.ClientID %>').Scrollable({
										ScrollHeight: 575,
										IsInPanel: true
									});
								});
							</script>


						</td>
						 <td style="width: 2px; background-color: whitesmoke; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>

						 <td style="border: 1px solid #000000; width: 800px; background-color: floralwhite; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; vertical-align:top;" colspan="6">
							 <asp:Panel ID="Panel6" runat="server" Width="100%" Height="580px">
								<asp:GridView ID="dgvTab1R" runat="server" AutoGenerateColumns="False" OnRowDataBound="dgvTab1R_RowDataBound" OnRowCreated="dgvTab1R_RowCreated" Font-Names="Segoe UI" Font-Size="small" BackColor="White" BorderColor="#999999" BorderStyle="Solid" AllowSorting="true" BorderWidth="1px" CellPadding="3" GridLines="Vertical" HeaderStyle-BorderColor="Black" ShowHeader="true" HeaderStyle-HorizontalAlign="Center">
									<AlternatingRowStyle BackColor="#DCDCDC" />
									<Columns>
										<asp:BoundField DataField="cust">
											<ItemStyle HorizontalAlign="left" VerticalAlign="Middle" Width="400px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="codeno">
											<ItemStyle HorizontalAlign="left" VerticalAlign="Middle" Width="100px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="mm">
											<ItemStyle HorizontalAlign="left" VerticalAlign="Middle" Width="350px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="qty" DataFormatString="{0:#,##0;(#,##0)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="90px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="wt" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="100px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="amt" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="120px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="asp" HeaderText="Amount" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="110px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="perkg" HeaderText="Amount" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="100px"></ItemStyle>
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
									$('#<%=dgvTab1R.ClientID %>').Scrollable({
										ScrollHeight: 575,
										IsInPanel: true
									});
								});
							</script>
						</td>
					</tr>

				   </table>

			</ContentTemplate>

		</ajaxToolkit:TabPanel>

		<ajaxToolkit:TabPanel ID="TabPanel4" runat="server" HeaderText="Per Customer">
			<ContentTemplate>
				<table style="width: 100%; font-family: 'Segoe UI'; font-size: small; float: left; background-color: lightgrey; border-spacing: 0px; margin-left: 0px; padding-left: 0px; padding-right: 0px;">
					<tr style="height: 26px; background-color: lightgray;">
						<td style="border: 1px solid #000000; width: 120px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" >Customer:&nbsp;&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: 500px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="4" >
							<asp:DropDownList ID="cboCustomer" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" Width="99%">
							</asp:DropDownList>
						</td>
						<td style="width: 880px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="7" >
						</td>
					</tr>
					<tr style="height: 2px; background-color: whitesmoke;">
						<td style="width: auto; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="12">
						</td>
					</tr>
					<tr style="height: 656px; background-color: floralwhite;">
						<td style="border: 1px solid #000000; width: auto; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="12">
							<asp:Panel ID="Panel7" runat="server" Width="99%" Height="650px">
								<asp:GridView ID="dgvListPerCust" runat="server" AutoGenerateColumns="False" OnRowDataBound="dgvListPerCust_RowDataBound" OnRowCreated="dgvListPerCust_RowCreated" Font-Names="Segoe UI" Font-Size="small" BackColor="White" BorderColor="#999999" BorderStyle="Solid" AllowSorting="true" BorderWidth="1px" CellPadding="3" GridLines="Vertical" HeaderStyle-BorderColor="Black" ShowHeader="true" HeaderStyle-HorizontalAlign="Center">
									<AlternatingRowStyle BackColor="#DCDCDC" />
									<Columns>
										<asp:BoundField DataField="mmdesc">
											<ItemStyle HorizontalAlign="left" VerticalAlign="Middle" Width="563px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="transdate" DataFormatString="{0:yyyy-MM-dd}">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="107px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="dsrno">
											<ItemStyle HorizontalAlign="left" VerticalAlign="Middle" Width="120px"></ItemStyle>
										</asp:BoundField>
									   <asp:BoundField DataField="qty" DataFormatString="{0:#,##0;(#,##0)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="110px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="wt" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="110px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="detamt" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="102px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="detdiscamt" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="104px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="sp" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="127px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="itmamt" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="102px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="detgrossamt" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="100px"></ItemStyle>
										</asp:BoundField>
										<asp:BoundField DataField="detvat" DataFormatString="{0:#,##0.00;(#,##0.00)}">
											<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="90px"></ItemStyle>
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
									$('#<%=dgvListPerCust.ClientID %>').Scrollable({
										ScrollHeight: 645,
										IsInPanel: true
									});
								});
							</script>

						</td>
					</tr>
					<tr style="height: 26px; background-color: lightgray;">
						<td style="border: 1px solid #000000; width: auto; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="12">
						</td>
					</tr>
				</table>

			</ContentTemplate>

		</ajaxToolkit:TabPanel>

		<ajaxToolkit:TabPanel ID="TabPanel5" runat="server" HeaderText="Inventory Cost">
			<ContentTemplate>
				<center>
					<h1>Under Construction</h1>
				</center>

			</ContentTemplate>

		</ajaxToolkit:TabPanel>

		<ajaxToolkit:TabPanel ID="TabPanel6" runat="server" HeaderText="All Products">
			<ContentTemplate>
				<center>
					<h1>Under Construction</h1>
				</center>

			</ContentTemplate>

		</ajaxToolkit:TabPanel>

	</ajaxToolkit:TabContainer>





</asp:Content>
