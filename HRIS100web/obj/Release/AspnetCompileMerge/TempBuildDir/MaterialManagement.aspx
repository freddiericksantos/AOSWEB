<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MM.master" CodeBehind="MaterialManagement.aspx.vb" Inherits="AOS100web.MaterialManagement" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentOfMM" runat="server">
	 <script type="text/javascript">
		 // JavaScript code to make the text flashing
		 function toggleVisibility() {
			 var flashingText = document.getElementById('flashingText');
			 flashingText.style.visibility = (flashingText.style.visibility === 'visible' ? 'hidden' : 'visible');
		 }

		 // Set the interval for toggling visibility (e.g., every 500 milliseconds)
		 setInterval(toggleVisibility, 500);
	 </script>

	<table style="width: 98%; font-family: 'Segoe UI'; float: left; margin-top: 0px; padding-top: 0px; border-spacing: 0px; ">
		<tr style="height: 26px; background-color: lightgray;">
			<td style="width: 10px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
			<td style="width: 490px; background-color: floralwhite; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: medium;">What's New		
			</td>
			<td style="width: 12px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>

			<td style="width: 490px; background-color: floralwhite; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: Medium;">Available Modules		
			</td>
			<td style="width: 12px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
			<td style="width: 490px; background-color: floralwhite; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: medium;">On Going
				<asp:Label ID="lblUser" runat="server" Text="User" Visible="false"></asp:Label>
				<asp:Label ID="lblGrpUser" runat="server" Text="Grp User" Visible="false"></asp:Label>
			</td>
			<td style="width: 10px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
		</tr>
		<tr style="height: 26px; background-color: lightgray;">
			<td style="width: 10px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
			<td style="border-top:dotted; border-bottom-color:blanchedalmond; width: 490px; background-color: floralwhite; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: medium;">&nbsp;&nbsp;Reports:	
			</td>
			<td style="width: 12px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
			<td style="border-top:dotted; border-bottom-color:blanchedalmond; width: 490px; background-color: floralwhite; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: Medium;">&nbsp;&nbsp;Modules:		
			</td>
			<td style="width: 12px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
			<td style="border-top:dotted; border-bottom-color:blanchedalmond; width: 490px; background-color: floralwhite; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">		
			</td>
			<td style="width: 10px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
		</tr>
		<tr style="height: 26px; background-color: lightgray;">
			<td style="width: 10px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
			<td style="width: 490px; background-color: floralwhite; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">&nbsp;&nbsp;&nbsp;&nbsp;Production Analysis		
			</td>
			<td style="width: 12px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
			<td style="width: 490px; background-color: floralwhite; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">&nbsp;&nbsp;&nbsp;&nbsp;
				<asp:LinkButton ID="lbIssuance" runat="server">Issuance</asp:LinkButton>
			</td>
			<td style="width: 12px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
			<td style="width: 490px; background-color: floralwhite; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">		
			</td>
			<td style="width: 10px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
		</tr>
		<tr style="height: 26px; background-color: lightgray;">
			<td style="width: 10px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
			<td style="width: 490px; background-color: floralwhite; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">&nbsp;&nbsp;&nbsp;&nbsp;Option to Hide/Show Side Menu		
			</td>
			<td style="width: 12px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
			<td style="width: 490px; background-color: floralwhite; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">&nbsp;&nbsp;&nbsp;&nbsp;
				 <%--<asp:LinkButton ID="lbSOapprov" runat="server">Sales Order Approval</asp:LinkButton>--%>
			</td>
			<td style="width: 12px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
			<td style="width: 490px; background-color: floralwhite; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">		
			</td>
			<td style="width: 10px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
		</tr>
		<tr style="height: 26px; background-color: lightgray;">
			<td style="width: 10px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
			<td style="width: 490px; background-color: floralwhite; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">&nbsp;&nbsp;&nbsp;&nbsp;MM Reports:		
			</td>
			<td style="width: 12px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
					   
			<td style="width: 490px; background-color: floralwhite; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">&nbsp;&nbsp;&nbsp;&nbsp;
				 <%--<asp:LinkButton ID="lbSalesInv" runat="server">Sales Invoice</asp:LinkButton>--%>
				
			</td>
			<td style="width: 12px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
			<td style="width: 490px; background-color: floralwhite; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">		
			</td>
			<td style="width: 10px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
		</tr>
		<tr style="height: 26px; background-color: lightgray;">
			<td style="width: 10px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
			<td style="width: 490px; background-color: floralwhite; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Register		
			</td>
			<td style="width: 12px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
			<td style="width: 490px; background-color: floralwhite; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">&nbsp;&nbsp;Reports:
				 

			</td>
			<td style="width: 12px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
			<td style="width: 490px; background-color: floralwhite; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">		
			</td>
			<td style="width: 10px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
		</tr>
		<tr style="height: 26px; background-color: lightgray;">
			<td style="width: 10px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
			<td style="width: 490px; background-color: floralwhite; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Material To Material			
			</td>
			<td style="width: 12px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
			<td style="width: 490px; background-color: floralwhite; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
				 <asp:LinkButton ID="lbInvReport" runat="server">Inventory Reports</asp:LinkButton>

				 <%--<asp:LinkButton ID="lbSmnAccess" runat="server">Salesman Access</asp:LinkButton>--%>
			</td>
			<td style="width: 12px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
			<td style="width: 490px; background-color: floralwhite; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">		
			</td>
			<td style="width: 10px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
		</tr>
		<tr style="height: 26px; background-color: lightgray;">
			<td style="width: 10px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
			<td style="width: 490px; background-color: floralwhite; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">		
			</td>
			<td style="width: 12px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
			<td style="width: 490px; background-color: floralwhite; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
				 <%--<asp:LinkButton ID="lbMgrAccess" runat="server">Sales Manager Access</asp:LinkButton>--%>
				 <asp:LinkButton ID="lblMMregister" runat="server">MM Registers</asp:LinkButton>
			</td>
			<td style="width: 12px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
			<td style="width: 490px; background-color: floralwhite; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">		
			</td>
			<td style="width: 10px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
		</tr>
		<tr style="height: 26px; background-color: lightgray;">
			<td style="width: 10px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
			<td style="width: 490px; background-color: floralwhite; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">		
			</td>
			<td style="width: 12px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
			<td style="width: 490px; background-color: floralwhite; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: medium;">
				
			</td>
			<td style="width: 12px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
			<td style="width: 490px; background-color: floralwhite; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">		
			</td>
			<td style="width: 10px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
		</tr>
		<tr style="height: 26px; background-color: lightgray;">
			<td style="width: 10px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
			<td style="width: 490px; background-color: floralwhite; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">		
			</td>
			<td style="width: 12px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
			<td style="width: 490px; background-color: floralwhite; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">&nbsp;&nbsp;&nbsp;&nbsp;
			
				
			</td>
			<td style="width: 12px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
			<td style="width: 490px; background-color: floralwhite; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">		
			</td>
			<td style="width: 10px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
		</tr>
		<tr style="height: 36px; background-color: floralwhite;">
			<td style="width: 10px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
			<td style="width: 1494px; background-color: floralwhite; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; " colspan ="5"> 
				<span id="flashingText"> 
					 <asp:Label ID="Label1" runat="server" Text="INVENTORY ALERT!!!" ForeColor="Red" Font-Bold="true"></asp:Label>
				</span>
						
			</td>
						
			<td style="width: 10px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
		</tr>
		<tr style="height: 26px; background-color: floralwhite;">
			<td style="width: 10px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
			<%--start here INVENTORY ALERT--%>
			<td style="border: 1px solid #000000; width: 1494px; background-color: floralwhite; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;  " colspan="5">	
				
				<asp:RadioButton ID="RadioButton1" runat="server" text="Raw Materials" GroupName="Inv" AutoPostBack="true"/>&nbsp;&nbsp;
				<asp:RadioButton ID="RadioButton2" runat="server" text="Packaging" GroupName="Inv" AutoPostBack="true"/>&nbsp;&nbsp;
				<asp:RadioButton ID="RadioButton3" runat="server" text="Finished Goods" GroupName="Inv" AutoPostBack="true"/> &nbsp;&nbsp;&nbsp;&nbsp;
				<asp:CheckBox ID="CheckBox1" runat="server" text ="With Serial Only" Visible="false" AutoPostBack="true"/>
								
			</td>
			
			<td style="width: 10px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
		</tr>
		<tr style="height: auto; background-color: floralwhite; ">
			<td style="width: 10px; background-color: whitesmoke; text-align: center; padding-left: 0px;"></td>
			<td style="border: 1px solid #000000; width: 1494px; background-color: floralwhite; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small; vertical-align:top;" colspan="5" rowspan="16">
				<asp:Panel ID="Panel1" runat="server" Width="100%" Height="496px" >
					<asp:GridView ID="dgvMMinv" runat="server" AutoGenerateColumns="False" OnRowDataBound="dgvMMinv_RowDataBound" OnRowCreated="dgvMMinv_RowCreated" Font-Names="Segoe UI" Font-Size="small" BackColor="White" BorderColor="#999999" BorderStyle="Solid" AllowSorting="true" BorderWidth="1px" CellPadding="3" GridLines="Vertical" HeaderStyle-BorderColor="Black" ShowHeader="true" HeaderStyle-HorizontalAlign="Center" >
						<AlternatingRowStyle BackColor="#DCDCDC" />
						<Columns>
							<asp:BoundField DataField="codeno" HeaderText="Code No.">
								<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="110px"></ItemStyle>
							</asp:BoundField>
							<asp:BoundField DataField="mmdesc" HeaderText="MM Description">
								<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="500px"></ItemStyle>
							</asp:BoundField>
							<asp:BoundField DataField="qty_bal" HeaderText="Qty Bal" DataFormatString="{0:#,##0;(#,##0)}">
								<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="110px"></ItemStyle>
							</asp:BoundField>
							<asp:BoundField DataField="wt_bal" HeaderText="Wt/Vol Bal" DataFormatString="{0:#,##0.00;(#,##0.00)}">
								<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="110px"></ItemStyle>
							</asp:BoundField>
							<asp:BoundField DataField="invlevel" HeaderText="Reorder Level" DataFormatString="{0:#,##0.00;(#,##0.00)}">
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

					<script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
							<script src="Scripts/ScrollableGridViewPlugin_ASP.NetAJAXmin.js" type="text/javascript"></script>
							<script type="text/javascript">
								$(document).ready(function () {
									$('#<%=dgvMMinv.ClientID %>').Scrollable({
										ScrollHeight: 470,
										IsInPanel: true
									});
								});
							</script>

				</asp:Panel>

			</td>
			
			<td style="width: 10px; background-color: whitesmoke; text-align: center; padding-left: 0px;"></td>
		</tr>
		<tr style="height: 26px; background-color: floralwhite;">
			<td style="width: 10px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
			<%--<td style="width: 490px; background-color: floralwhite; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">		
			</td>--%>
			<%--<td style="width: 12px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
			<td style="width: 490px; background-color: floralwhite; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
			</td>
			<td style="width: 12px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
			<td style="width: 490px; background-color: floralwhite; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">		
			</td>--%>
			<td style="width: 10px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
		</tr>
		<tr style="height: 26px; background-color: floralwhite;">
			<td style="width: 10px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
			<%--<td style="width: 490px; background-color: floralwhite; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">		
			</td>--%>
			<%--<td style="width: 12px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
			<td style="width: 490px; background-color: floralwhite; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
			</td>
			<td style="width: 12px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
			<td style="width: 490px; background-color: floralwhite; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">		
			</td>--%>
			<td style="width: 10px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
		</tr>
		<tr style="height: 26px; background-color: floralwhite;">
			<td style="width: 10px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
			<%--<td style="width: 490px; background-color: floralwhite; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">		
			</td>--%>
			<%--<td style="width: 12px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
			<td style="width: 490px; background-color: floralwhite; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
			</td>
			<td style="width: 12px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
			<td style="width: 490px; background-color: floralwhite; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">		
			</td>--%>
			<td style="width: 10px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
		</tr>
		<tr style="height: 26px; background-color: floralwhite;">
			<td style="width: 10px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
			<%--<td style="width: 490px; background-color: floralwhite; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">		
			</td>--%>
			<%--<td style="width: 12px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
			<td style="width: 490px; background-color: floralwhite; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
			</td>
			<td style="width: 12px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
			<td style="width: 490px; background-color: floralwhite; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">		
			</td>--%>
			<td style="width: 10px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
		</tr>
		<tr style="height: 26px; background-color: floralwhite;">
			<td style="width: 10px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
			<%--<td style="width: 490px; background-color: floralwhite; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">		
			</td>--%>
			<%--<td style="width: 12px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
			<td style="width: 490px; background-color: floralwhite; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
			</td>
			<td style="width: 12px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
			<td style="width: 490px; background-color: floralwhite; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">		
			</td>--%>
			<td style="width: 10px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
		</tr>
		<tr style="height: 26px; background-color: floralwhite;">
			<td style="width: 10px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
			<%--<td style="width: 490px; background-color: floralwhite; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">		
			</td>--%>
			<%--<td style="width: 12px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
			<td style="width: 490px; background-color: floralwhite; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
			</td>
			<td style="width: 12px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
			<td style="width: 490px; background-color: floralwhite; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">		
			</td>--%>
			<td style="width: 10px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
		</tr>
		<tr style="height: 26px; background-color: floralwhite;">
			<td style="width: 10px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
			<%--<td style="width: 490px; background-color: floralwhite; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">		
			</td>--%>
			<%--<td style="width: 12px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
			<td style="width: 490px; background-color: floralwhite; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
			</td>
			<td style="width: 12px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
			<td style="width: 490px; background-color: floralwhite; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">		
			</td>--%>
			<td style="width: 10px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
		</tr>
		<tr style="height: 26px; background-color: floralwhite;">
			<td style="width: 10px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
			<%--<td style="width: 490px; background-color: floralwhite; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">		
			</td>--%>
			<%--<td style="width: 12px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
			<td style="width: 490px; background-color: floralwhite; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
			</td>
			<td style="width: 12px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
			<td style="width: 490px; background-color: floralwhite; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">		
			</td>--%>
			<td style="width: 10px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
		</tr>
		<tr style="height: 26px; background-color: floralwhite;">
			<td style="width: 10px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
			<%--<td style="width: 490px; background-color: floralwhite; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">		
			</td>--%>
			<%--<td style="width: 12px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
			<td style="width: 490px; background-color: floralwhite; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
			</td>
			<td style="width: 12px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
			<td style="width: 490px; background-color: floralwhite; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">		
			</td>--%>
			<td style="width: 10px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
		</tr>
		<tr style="height: 26px; background-color: floralwhite;">
			<td style="width: 10px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
			
			<td style="width: 10px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
		</tr>
		<tr style="height: 26px; background-color: floralwhite;">
			<td style="width: 10px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
			
			<td style="width: 10px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
		</tr>
		<tr style="height: 26px; background-color: floralwhite;">
			<td style="width: 10px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
			
			<td style="width: 10px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
		</tr>
		<tr style="height: 26px; background-color: floralwhite;">
			<td style="width: 10px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
			
			<td style="width: 10px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
		</tr>
		<tr style="height: 26px; background-color: floralwhite;">
			<td style="width: 10px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
			
			<td style="width: 10px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
		</tr>
		<tr style="height: 26px; background-color: floralwhite;">
			<td style="width: 10px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
			
			<td style="width: 10px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
		</tr>

		<tr style="height: auto; background-color: floralwhite;">
			<td style="width: 10px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
			<td style="width: 490px; background-color: floralwhite; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">		
			</td>
			<td style="width: 12px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
			<td style="width: 490px; background-color: floralwhite; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">
			</td>
			<td style="width: 12px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
			<td style="width: 490px; background-color: floralwhite; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">		
			</td>
			<td style="width: 10px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
		</tr>

	</table>

</asp:Content>
