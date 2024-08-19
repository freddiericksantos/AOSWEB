<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin.master" CodeBehind="VoidApproval.aspx.vb" Inherits="AOS100web.VoidApproval" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentOfAdministrator" runat="server">
	<style type="text/css">
		div {
			z-index: 9999;
		}
	</style>

	<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

	<script type="text/javascript">
		function Confirm() {
			var confirm_value = document.createElement("INPUT");
			confirm_value.type = "hidden";
			confirm_value.name = "confirm_value";
			if (confirm("Are you sure to POST Selected Document?")) {
				confirm_value.value = "Yes";
			} else {
				confirm_value.value = "No";
			}
			document.forms[0].appendChild(confirm_value);
		}
	</script>

	<style>
		.Grid1 {
			margin-left: 1px;
			margin-right: 1px;
			margin-bottom: 2px;
			padding-left: 1px;
			padding-right: 1px;
			border-style: solid;
			border-color: black;
		}
	</style>
		

<%--	<ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server"
		TargetControlID="btnShowModal" PopupControlID="pnlModal"
		OkControlID="btnOk" OnOkScript="btnOk_Click()" CancelControlID="btnCancel"
		BackgroundCssClass="modalBackground">
	</ajaxToolkit:ModalPopupExtender>

	function confirmDelete() {
	var modalPopup = $find('<%= ModalPopupExtender1.ClientID %>');
	modalPopup.show();
	return false;
}--%>

	<link href="css/admGen.css" rel="stylesheet" />
		
	<table style="width: 100%; font-family: 'Segoe UI'; float: left; margin-top: 0px; padding-top: 0px">
		<tr>
			<td style="background-color: #b1bbd7; border-top: solid; border-bottom: solid; border-color: red; border-width: 2px; width: 100%; font-family: 'Segoe UI'; font-size: 10px; text-align: center; min-height: 28px; max-height: 28px; padding-bottom: 1px; padding-top: 1px;"
				colspan="9">
				<asp:LinkButton ID="lbNew" runat="server" OnClick="lbNew_Click" Style="text-decoration: none; background-color: #7fc6f6; font-family: Tahoma; font-size: 14px; color: white; margin-right: 5px; padding-left: 5px; padding-right: 5px; min-height: 22px;"
					BorderStyle="Solid" ToolTip="Reset Fields" CssClass="StdBottomL">
					<asp:Image runat="server" imageurl="~/images/new_16.png" style="vertical-align: middle"/>&nbsp;New
				</asp:LinkButton>
				<asp:LinkButton ID="lbSave" runat="server" OnClick="lbSave_Click" OnClientClick="Confirm()" Style="text-decoration: none; background-color: #7fc6f6; padding-right: 5px; font-family: Tahoma; font-size: 14px; color: white; margin-right: 5px; padding-left: 5px; min-height: 22px;"
					BorderStyle="Solid" ToolTip="Save" Enabled="false">
					<asp:Image runat="server" imageurl="~/images/check_16.png" style="vertical-align: middle; "/>&nbsp;Post
				</asp:LinkButton>
				<asp:LinkButton ID="lbPrint" runat="server" OnClientClick="target ='_blank';" OnClick="lbPrint_Click" Style="text-decoration: none; background-color: #7fc6f6; padding-right: 5px; font-family: Tahoma; font-size: 14px; color: white; margin-right: 5px; padding-left: 5px; min-height: 22px;"
					BorderStyle="Solid" ToolTip="Reports" Enabled="false">
					<asp:Image runat="server" imageurl="~/images/report_16.png" style="vertical-align: middle"/>&nbsp;Reports
				</asp:LinkButton>
				<asp:LinkButton ID="lbDelete" runat="server" OnClick="lbDelete_Click" Style="text-decoration: none; background-color: #7fc6f6; padding-right: 5px; font-family: Tahoma; font-size: 14px; color: white; margin-right: 5px; padding-left: 5px; min-height: 22px;"
					BorderStyle="Solid" ToolTip="Delete" Enabled="false">
					<asp:Image runat="server" imageurl="~/images/delete_16.png" style="vertical-align: middle"/>&nbsp;Delete
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
			<td style="border: 1px solid #000000; width: auto; background-color: lightgray;" colspan="10"></td>
		</tr>
		
		<tr>
			<td style="border: 1px solid #000000; width: 500px; height: 30px; background-color: lightgray;" colspan="4">&nbsp;&nbsp;
				<asp:Label ID="lblTitle" runat="server" Text="Reports" Font-Size="Larger" Font-Italic="true" ForeColor="Red"></asp:Label>
			</td>
			<td style="border: 1px solid #000000; width: 600px; height: 30px; background-color: lightgray;" colspan="6">&nbsp;&nbsp;
				<asp:Label ID="lblMsg" runat="server" Text="Message Box" Font-Size="Medium" Font-Italic="true" ForeColor="Red"></asp:Label>
			</td>
			
		</tr>
		

	</table>

	<ajaxToolkit:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" Style="width: 1500px; height: auto;">
		<ajaxToolkit:TabPanel ID="TabPanel1" runat="server" HeaderText="For Approval">
			<ContentTemplate>
				<table style="width: 100%; font-family: 'Segoe UI'; font-size: small; float: left; background-color: lightgrey; border-spacing: 0px; margin-left: 0px; padding-left: 0px; padding-right: 0px;">
					<tr style="height: 26px; background-color: lightgray;">
						<td style="border: 1px solid #000000; width: 80px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">Doc Type:&nbsp;&nbsp;

						</td>
						<td style="border: 1px solid #000000; width: 400px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:DropDownList ID="cboDocTypeStat" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" Width="99%">
							</asp:DropDownList>

						</td>
						<td style="width: 1000px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="8">
							
						</td>
					</tr>
			
					<tr>
						<td style="width: auto; background-color: whitesmoke;" colspan="10"></td>
					</tr>
					<tr style="height: 681px; background-color: lightgray;">
						<td style="border: 1px solid #000000; background-color: floralwhite; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; vertical-align: top;" colspan="10">
							<asp:Panel ID="Panel5" runat="server" Width="100%" Height="646px">
								<asp:GridView ID="dgvAppList" runat="server" AutoGenerateColumns="False" OnRowDataBound="OnRowDataBound" OnRowCreated="dgvAppList_RowCreated" OnRowDeleting="OnRowDeleting" Font-Names="Segoe UI" Font-Size="small" BackColor="White" BorderColor="#999999" BorderStyle="Solid" AllowSorting="true" BorderWidth="1px" CellPadding="3" GridLines="Vertical" HeaderStyle-BorderColor="Black" ShowHeader="true" HeaderStyle-HorizontalAlign="Center" >
									<AlternatingRowStyle BackColor="#DCDCDC" />
									<Columns>

										<asp:ButtonField ButtonType="Button" CommandName="Select1" Text="Post" HeaderText="Post"/>
										<%--<asp:ButtonField ButtonType="Button" CommandName="Select2" Text="Cancel" HeaderText="Cancel" />--%>
										<asp:CommandField ShowDeleteButton="True" ButtonType="Button" DeleteText="Cancel" HeaderText="Cancel" />
										
										<asp:BoundField DataField="vdocno">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="docno">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="tc">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="transdate" DataFormatString="{0:yyyy-MM-dd}">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></ItemStyle>
										</asp:BoundField>
										
										<asp:BoundField DataField="user" >
											<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="130px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="remarks">
											<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="350px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="reqstat" >
											<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="150px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="appby" >
											<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="150px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="module" >
											<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="100px"></ItemStyle>
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
									$('#<%=dgvAppList.ClientID %>').Scrollable({
										ScrollHeight: 650,
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
		<ajaxToolkit:TabPanel ID="TabPanel2" runat="server" HeaderText="List">
			<ContentTemplate>
				<table style="width: 100%; font-family: 'Segoe UI'; font-size: small; float: left; background-color: lightgrey; border-spacing: 0px; margin-left: 0px; padding-left: 0px; padding-right: 0px;">
					<%--Line1--%>
					<tr style="height: 26px; background-color: lightgray">
						<td style="border: 1px solid #000000; width: 110px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
						Date From:&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: 120px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:TextBox ID="dpTransDate" runat="server" TextMode="Date" Height="20px" Font-Names="Segoe UI" Font-Size="small" CssClass="DateBox" Width="97%" AutoPostBack="true"></asp:TextBox>
						</td>
						
						<td style="width: 2px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
						
						<td style="border: 1px solid #000000; width: 110px; background-color: lightgray; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
						Date To:&nbsp;
						</td>
						<td style="border: 1px solid #000000; width: 120px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
							<asp:TextBox ID="dpTransDate2" runat="server" TextMode="Date" Height="20px" Font-Names="Segoe UI" Font-Size="small" CssClass="DateBox" Width="97%" AutoPostBack="true">
							</asp:TextBox>
						</td>
						
						<td style="width: 2px; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"></td>
						
						<td style="width: 750px; background-color: whitesmoke; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="4">&nbsp;&nbsp;&nbsp;&nbsp; Main Module: &nbsp;&nbsp;
							<asp:RadioButton ID="RadioButton1" runat="server" GroupName="T2" AutoPostBack="true" Text="Financial Accounting"/>
							<asp:RadioButton ID="RadioButton2" runat="server" GroupName="T2" AutoPostBack="true" Text="Material Management"/>
							<asp:RadioButton ID="RadioButton3" runat="server" GroupName="T2" AutoPostBack="true" Text="Sales and Distribution"/>

						
						</td>
						
						<td style="width: 200px; background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="2">
							
						</td>

					</tr>
					<%--Line2--%>
					<tr style="height: 2px; background-color: lightgray">
						<td style="width: auto; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="13"></td>
					</tr>

					<%--Line3--%>

					<tr style="height: 26px; background-color: lightgray">
						<td style="border: 1px solid #000000;width: 110px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;"  >Doc Type:&nbsp;&nbsp;
							
						</td>
						<td style="border: 1px solid #000000;width: 400px; background-color: lightgray; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="4" >
							<asp:DropDownList ID="cboDocTypeT4" runat="server" AutoPostBack="true" Font-Names="Segoe UI" Font-Size="small" Width="99%">
							</asp:DropDownList>
						</td>
						<td style="background-color: whitesmoke; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="7" >
						</td>
					</tr>
					<tr style="height: 2px; background-color: lightgray">
						<td style="width: auto; background-color: whitesmoke; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;" colspan="13"></td>
					</tr>
					<tr style="height: 651px; background-color: lightgray;">
						<td style="border: 1px solid #000000; background-color: floralwhite; text-align: right; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; vertical-align:top; " colspan="13">
							<%--dgvStatList--%>
							
							<asp:Panel ID="Panel1" runat="server" Width="100%" Height="620px">
								<asp:GridView ID="dgvStatList" runat="server" AutoGenerateColumns="False" OnRowDataBound="dgvStatList_RowDataBound" OnRowCreated="dgvStatList_RowCreated" Font-Names="Segoe UI" Font-Size="small" BackColor="White" BorderColor="#999999" BorderStyle="Solid" AllowSorting="true" BorderWidth="1px" CellPadding="3" GridLines="Vertical" HeaderStyle-BorderColor="Black" ShowHeader="true" HeaderStyle-HorizontalAlign="Center" >
									<AlternatingRowStyle BackColor="#DCDCDC" />
									<Columns>

										<asp:BoundField DataField="vdocno">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="docno">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="tc">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="transdate" DataFormatString="{0:yyyy-MM-dd}">
											<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></ItemStyle>
										</asp:BoundField>
										
										<asp:BoundField DataField="user" >
											<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="130px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="remarks">
											<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="350px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="reqstat" >
											<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="150px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="appby" >
											<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="150px"></ItemStyle>
										</asp:BoundField>

										<asp:BoundField DataField="module" >
											<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="100px"></ItemStyle>
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
									$('#<%=dgvStatList.ClientID %>').Scrollable({
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


	</ajaxToolkit:TabContainer>


</asp:Content>
