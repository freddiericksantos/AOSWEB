<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin.Master" CodeBehind="Administrator.aspx.vb" Inherits="AOS100web.Administrator" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentOfAdministrator" runat="server">
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
			<td style="border-top:dotted; border-bottom-color:blanchedalmond; width: 490px; background-color: floralwhite; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: medium;">&nbsp;&nbsp;Option to Hide/Show Side Menu	
			</td>
			<td style="width: 12px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
			<td style="border-top:dotted; border-bottom-color:blanchedalmond; width: 490px; background-color: floralwhite; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: Medium;">&nbsp;&nbsp;	
			</td>
			<td style="width: 12px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
			<td style="border-top:dotted; border-bottom-color:blanchedalmond; width: 490px; background-color: floralwhite; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">		
			</td>
			<td style="width: 10px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
		</tr>
		<tr style="height: 26px; background-color: lightgray;">
			<td style="width: 10px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
			<td style="width: 490px; background-color: floralwhite; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">&nbsp;&nbsp;&nbsp;&nbsp;
			</td>
			<td style="width: 12px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
			<td style="width: 490px; background-color: floralwhite; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">&nbsp;&nbsp;&nbsp;&nbsp;
				<%--<asp:LinkButton ID="lbIssuance" runat="server">Issuance</asp:LinkButton>--%>
			</td>
			<td style="width: 12px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
			<td style="width: 490px; background-color: floralwhite; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">		
			</td>
			<td style="width: 10px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
		</tr>
		<tr style="height: 26px; background-color: lightgray;">
			<td style="width: 10px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
			<td style="width: 490px; background-color: floralwhite; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">&nbsp;&nbsp;&nbsp;&nbsp;		
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
			<td style="width: 490px; background-color: floralwhite; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">		
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
			<td style="width: 490px; background-color: floralwhite; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">		
			</td>
			<td style="width: 12px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
			<td style="width: 490px; background-color: floralwhite; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
				 

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
			<td style="width: 490px; background-color: floralwhite; text-align: left; padding-left: 0px; font-family: 'Segoe UI'; font-size: medium;">&nbsp;&nbsp;
				
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
				 <%--<asp:LinkButton ID="lbInvReport" runat="server">Inventory Reports</asp:LinkButton>--%>
				
			</td>
			<td style="width: 12px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
			<td style="width: 490px; background-color: floralwhite; text-align: center; padding-left: 0px; font-family: 'Segoe UI'; font-size: small;">		
			</td>
			<td style="width: 10px; background-color: whitesmoke; text-align: right; padding-left: 0px;"></td>
		</tr>
		<tr style="height: 26px; background-color: floralwhite;">
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
		<tr style="height: 26px; background-color: floralwhite;">
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
		<tr style="height: 26px; background-color: floralwhite;">
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
		<tr style="height: 26px; background-color: floralwhite;">
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
		<tr style="height: 26px; background-color: floralwhite;">
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
		<tr style="height: 26px; background-color: floralwhite;">
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
		<tr style="height: 26px; background-color: floralwhite;">
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
		<tr style="height: 26px; background-color: floralwhite;">
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
		<tr style="height: 26px; background-color: floralwhite;">
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
		<tr style="height: 26px; background-color: floralwhite;">
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
		<tr style="height: 26px; background-color: floralwhite;">
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
		<tr style="height: 26px; background-color: floralwhite;">
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
		<tr style="height: 213px; background-color: floralwhite;">
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
