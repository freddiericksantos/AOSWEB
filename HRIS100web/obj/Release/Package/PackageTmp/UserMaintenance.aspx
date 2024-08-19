<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin.master" CodeBehind="UserMaintenance.aspx.vb" Inherits="AOS100web.UserMaintenance" %>


<%@ Register Assembly="EditableDropDownList" Namespace="EditableControls" TagPrefix="editable" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentOfAdministrator" runat="server">

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
			if (confirm("Are you SURE to RESET Password?")) {
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
			if (confirm("Are you SURE to SAVE User ID?")) {
				confirm_value.value = "Yes";
			} else {
				confirm_value.value = "No";
			}
			document.forms[0].appendChild(confirm_value);
		}
				
	</script>

	<link href="css/admGen.css" rel="stylesheet" />

	<table style="width: 98%; font-family: 'Segoe UI'; float: left; margin-top: -2px; padding-top: 0px">
		<tr>
			<td style="background-color: #b1bbd7; border-top: solid; border-bottom: solid; border-color: red; border-width: 2px; width: 100%; font-family: 'Segoe UI'; font-size: 10px; text-align: center; min-height: 28px; max-height: 28px; padding-bottom: 1px;" colspan="12">
				<asp:LinkButton ID="lbNew" runat="server" OnClick="lbNew_Click" Style="text-decoration: none; background-color: #7fc6f6; font-family: Tahoma; font-size: 14px; color: white; margin-right: 5px; padding-left: 5px; padding-right: 5px; min-height: 22px;" BorderStyle="Solid" ToolTip="Reset Fields" CssClass="StdBottomL">
					<asp:Image runat="server" imageurl="~/images/new_16.png" style="vertical-align: middle"/>&nbsp;New
				</asp:LinkButton>
				<asp:LinkButton ID="lbSave" runat="server" Style="text-decoration: none; background-color: #7fc6f6; padding-right: 5px; font-family: Tahoma; font-size: 14px; color: white; margin-right: 5px; padding-left: 5px; min-height: 22px;" BorderStyle="Solid" ToolTip="Save" Enabled="true">
					<asp:Image runat="server" imageurl="~/images/save_orange_16.png" style="vertical-align: middle"/>&nbsp;Save
				</asp:LinkButton>
				<asp:LinkButton ID="lbPrint" runat="server" Style="text-decoration: none; background-color: #7fc6f6; padding-right: 5px; font-family: Tahoma; font-size: 14px; color: white; margin-right: 5px; padding-left: 5px; min-height: 22px;"
					BorderStyle="Solid" ToolTip="Print" Enabled="false">
					<asp:Image runat="server" imageurl="~/images/Print_16.png" style="vertical-align: middle"/>&nbsp;Print
				</asp:LinkButton>
				<asp:LinkButton ID="lbDelete" runat="server" Style="text-decoration: none; background-color: #7fc6f6; padding-right: 5px; font-family: Tahoma; font-size: 14px; color: white; margin-right: 5px; padding-left: 5px; min-height: 22px;" BorderStyle="Solid" ToolTip="Delete" Enabled="false">
					<asp:Image runat="server" imageurl="~/images/delete_16.png" style="vertical-align: middle"/>&nbsp;Delete
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
			<td style="border: 1px solid #000000; width: auto; background-color: lightgray;" colspan="12"></td>
		</tr>

		<tr>
			<td style="border: 1px solid #000000; width: 400px; height: 30px; background-color: lightgray;" colspan="3">&nbsp;&nbsp;
				<asp:Label ID="lblTitle" runat="server" Text="User Management and Maintenance" Font-Size="Larger" Font-Italic="true" ForeColor="Red"></asp:Label>
			</td>
			<td style="border: 1px solid #000000; width: 700px; height: 30px; background-color: lightgray;" colspan="9">&nbsp;&nbsp;
				<asp:Label ID="lblMsg" runat="server" Text="Message Box" Font-Size="Medium" Font-Italic="true" ForeColor="Red"></asp:Label>
			</td>
		</tr>

		<tr style="height: 26px; ">
			<td style="border: 1px solid #000000; width: 250px; height: 753px; text-align:center; background-color: floralwhite;" rowspan="16">
				<asp:Panel ID="Panel1" runat="server" Width="98%" Height="750px">
					<asp:GridView ID="dgvSideListView" runat="server" AutoGenerateColumns="False" OnRowDataBound="dgvSideListView_RowDataBound" OnRowCreated="dgvSideListView_RowCreated" Font-Names="Segoe UI" Font-Size="small" BackColor="White" BorderColor="#999999" BorderStyle="Solid" AllowSorting="true" BorderWidth="1px" CellPadding="3" GridLines="Vertical" HeaderStyle-BorderColor="Black" ShowHeader="true" HeaderStyle-HorizontalAlign="Center" >
						<AlternatingRowStyle BackColor="#DCDCDC" />
						<Columns>
							<asp:CommandField ShowSelectButton="true" HeaderText="Select">
								<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="60px"></ItemStyle>
							</asp:CommandField>
							<asp:BoundField DataField="username" HeaderText="User ID">
								<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="200px"></ItemStyle>
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
									$('#<%=dgvSideListView.ClientID %>').Scrollable({
										ScrollHeight: 723,
										IsInPanel: true
									});
								});
				</script>

			</td>
	
			<td style="width: 5px; " rowspan="16"></td>
			<td style="width: auto; " colspan="4"></td>

			<td style="border: 1px solid #000000; width: 500px; background-color: lightgray; font-size:small; text-align:center; " colspan="6">
				<asp:Label ID="lblPrevil" runat="server" Text="Grandted Authorization"></asp:Label>
			</td>
			
			
		</tr>
		<tr style="height: 26px; ">
			<td style="border: 1px solid #000000; width: 120px; text-align:right; font-size:small; ">User Group:&nbsp;&nbsp;</td>
			<td style="border: 1px solid #000000; width: 200px; text-align:center; " colspan="2">
				<asp:DropDownList ID="cboUserGroup" runat="server" Width="98%" Font-Names="Segoe UI" Font-Size="small" Height="22px" >
				</asp:DropDownList>
			</td>
			<td style="width: 5px; "></td>

			<td style="border: 1px solid #000000; width: 500px; background-color: floralwhite; " colspan="6" rowspan="15">
				<asp:Panel ID="Panel2" runat="server" Width="98%" Height="724px">
					<asp:GridView ID="dgvGrantedAccess" runat="server" AutoGenerateColumns="False" OnRowDataBound="dgvGrantedAccess_RowDataBound" OnRowCreated="dgvGrantedAccess_RowCreated" Font-Names="Segoe UI" Font-Size="small" BackColor="White" BorderColor="#999999" BorderStyle="Solid" AllowSorting="true" BorderWidth="1px" CellPadding="3" GridLines="Vertical" HeaderStyle-BorderColor="Black" ShowHeader="true" HeaderStyle-HorizontalAlign="Center" >
						<AlternatingRowStyle BackColor="#DCDCDC" />
						<Columns>
							<asp:BoundField DataField="formcode" HeaderText="Authorization Access">
								<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="480px"></ItemStyle>
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
						$('#<%=dgvGrantedAccess.ClientID %>').Scrollable({
							ScrollHeight: 396,
							IsInPanel: true
						});
					});
				</script>
			</td>
					
		</tr>

		<tr style="height: 26px; ">
			<td style="border: 1px solid #000000; width: 120px; text-align:right; font-size:small; ">User ID:&nbsp;&nbsp;</td>
			<td style="border: 1px solid #000000; width: 200px; text-align:left; " colspan="2">
				<asp:TextBox ID="txtUserID" runat="server" Font-Names="Segoe UI" Font-Size="small" MaxLength="25"></asp:TextBox>
			</td>
			<td style="width: 5px; "></td>
			
					
		</tr>

		<tr style="height: 26px; ">
			<td style="border: 1px solid #000000; width: 120px; text-align:right; font-size:small; ">Fullname:&nbsp;&nbsp;</td>
			<td style="border: 1px solid #000000; width: 200px; text-align:left; " colspan="2">
				<asp:TextBox ID="txtUserFullname" runat="server" Font-Names="Segoe UI" Font-Size="small" CssClass="txtBoxL" ></asp:TextBox>
			</td>
			<td style="width: 5px; "></td>
			
		</tr>

		<tr style="height: 26px; ">
			<td style="border: 1px solid #000000; width: 120px; text-align:right; font-size:small; ">Password:&nbsp;&nbsp;</td>
			<td style="border: 1px solid #000000; width: 200px; text-align:left; " colspan="2">
				<asp:TextBox ID="txtUserPass" runat="server" Font-Names="Segoe UI" Font-Size="small"  ></asp:TextBox>
				
			</td>
			<td style="width: 100px; text-align:left; font-size:small; " >
				&nbsp;&nbsp;
				<asp:Button ID="btnResetPass" runat="server" Text="Reset" Width="80px" OnClick="OnConfirm" OnClientClick="Confirm()"  />
			</td>
			
		</tr>

		<tr style="height: 26px; ">
			<td style="border: 1px solid #000000; width: 120px; text-align:right; font-size:small; ">Telephone No.:&nbsp;&nbsp;</td>
			<td style="border: 1px solid #000000; width: 200px; text-align:left; " colspan="2">
				<asp:TextBox ID="txtUserTelNo" runat="server" Font-Names="Segoe UI" Font-Size="small" CssClass="txtBoxL"></asp:TextBox>
			</td>
			<td style="width: 5px; "></td>
		
		</tr>

		<tr style="height: 26px; ">
			<td style="border: 1px solid #000000; width: 120px; text-align:right; font-size:small; ">Email Address:&nbsp;&nbsp;</td>
			<td style="border: 1px solid #000000; width: 200px; text-align:left; " colspan="2">
				<asp:TextBox ID="txtUserEmailAdd" runat="server" Font-Names="Segoe UI" Font-Size="small" CssClass="txtBoxL" ></asp:TextBox>
			</td>
			<td style="width: 5px; "></td>
			
		</tr>

		<tr style="height: 26px; ">
			<td style="border: 1px solid #000000; width: 120px; text-align:right; font-size:small; ">Designation:&nbsp;&nbsp;</td>
			<td style="border: 1px solid #000000; width: 200px; text-align:left; " colspan="2">
				<asp:TextBox ID="txtUserNotes" runat="server" Font-Names="Segoe UI" Font-Size="small" CssClass="txtBoxL"></asp:TextBox>
			</td>
			<td style="width: 5px; "></td>
			
		</tr>

		<tr style="height: 26px; ">
			<td style="border: 1px solid #000000; width: 120px; text-align:right; font-size:small; ">Employee No.:&nbsp;&nbsp;</td>
			<td style="border: 1px solid #000000; width: 200px; text-align:left; ">
				<asp:TextBox ID="txtEmpNo" runat="server" Font-Names="Segoe UI" Font-Size="small" Width="96%" ></asp:TextBox>
			</td>
			<td style="width: 120px; font-size: small;" colspan="3">
				
			</td>
			
			<td style="width: 120px;  text-align:center; "></td>
			
		</tr>

		<tr style="height: 26px; ">
			<td style="border: 1px solid #000000; width: 120px; text-align:right; font-size:small; ">Salesman No.:&nbsp;&nbsp;</td>
			<td style="border: 1px solid #000000; width: 200px; text-align:left; ">
				<asp:TextBox ID="txtSmnNo" runat="server" Font-Names="Segoe UI" Font-Size="small" Width="96%" ></asp:TextBox>
			</td>
			<td style="width: 300px; font-size: small;" colspan="3">
				<asp:CheckBox ID="CheckBox1" runat="server" Text="For Sales Representative" />
			</td>
			
		
		</tr>


		<tr style="height: 26px; ">
			<td style="border: 1px solid #000000; width: 120px; text-align:right; font-size:small; ">User's Location:&nbsp;&nbsp;</td>
			<td style="border: 1px solid #000000; width: 240px; text-align:left; font-size: small; " colspan="2">
				&nbsp;&nbsp;
				<asp:RadioButton ID="RadioButton1" runat="server" Text="Local" GroupName="UserLoc"/>
				&nbsp;&nbsp;
				<asp:RadioButton ID="RadioButton2" runat="server" Text="Remote" GroupName="UserLoc"/>
				&nbsp;&nbsp;
				<asp:CheckBox ID="CheckBox2" runat="server" Text="WFH Allowed" />
			</td>
			<td style="width: 5px; "></td>
			
		</tr>

		<tr>
			<td style="width: auto; " colspan="12" rowspan="8"></td>
		</tr>



	</table>

</asp:Content>
