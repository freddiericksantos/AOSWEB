
Public Class FI
	Inherits System.Web.UI.MasterPage
	Dim dt As DataTable

	Protected Sub AdmMsgBox(ByVal sMessage As String)
		Dim msg As String
		msg = "<script language='javascript'>"
		msg += "alert('" & sMessage & "');"
		msg += "<" & "/script>"
		Response.Write(msg)
	End Sub

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		If vLoggedUserID Is Nothing Then
			Response.Redirect("login.aspx")
			Exit Sub
		End If

		If Not Me.IsPostBack Then
			lblUserID.Text = Session("UserID")
			lblGrpUser.Text = Session("UserGrp")
			lblBA.Text = Session("BA")

			Select Case lblBA.Text
				Case "8100"
					fillTreeViewFI8100()

				Case "8200"

				Case "8300"

			End Select

		End If

		'vLoggedUserID = lblUserID.Text
		'vLoggedUserGroupID = lblGrp.Text

		LoadCurrMon()



	End Sub

	Protected Sub fillTreeViewFI8100()
		tvwMenuItems.Nodes.Clear()
		Dim root1 = New TreeNode("Sales Accounting")
		tvwMenuItems.Nodes.Add(root1)

		' Create child nodes
		Dim childNode1 As New TreeNode("Daily Sales Report")
		Dim childNode2 As New TreeNode("Deposit Application")
		Dim childNode3 As New TreeNode("Selling Price Update")
		Dim childNode4 As New TreeNode("Pay Transfer Request")
		Dim childNode5 As New TreeNode("Pay Transfer Approval")
		' Create grandchild nodes
		'Dim grandChildNode1 As New TreeNode("Daily Sales Report")
		'Dim grandChildNode2 As New TreeNode("Deposit Application")

		With root1
			.ChildNodes.Add(childNode1)
			.ChildNodes.Add(childNode2)
			.ChildNodes.Add(childNode3)
			.ChildNodes.Add(childNode4)
			.ChildNodes.Add(childNode5)

		End With

		Dim root2 = New TreeNode("Account Payables")
		tvwMenuItems.Nodes.Add(root2)

		Dim childNode21 As New TreeNode("Voucher Payable")
		Dim childNode22 As New TreeNode("Weekly Expense")
		Dim childNode23 As New TreeNode("Vendor DM")
		Dim childNode24 As New TreeNode("Document Posting")
		Dim childNode25 As New TreeNode("CA Liquidation")
		Dim childNode26 As New TreeNode("Gas PO Entry")
		Dim childNode27 As New TreeNode("Asset Management")
		Dim childNode28 As New TreeNode("A/P Reports")

		With root2
			.ChildNodes.Add(childNode21)
			.ChildNodes.Add(childNode22)
			.ChildNodes.Add(childNode23)
			.ChildNodes.Add(childNode24)
			.ChildNodes.Add(childNode25)
			.ChildNodes.Add(childNode26)
			.ChildNodes.Add(childNode27)
			.ChildNodes.Add(childNode28)

		End With

		Dim root3 = New TreeNode("Account Receivables")
		tvwMenuItems.Nodes.Add(root3)

		Dim childNode31 As New TreeNode("Customer DM/CM")
		Dim childNode32 As New TreeNode("A/R Reports")
		Dim childNode33 As New TreeNode("A/R Closing")

		With root3
			.ChildNodes.Add(childNode31)
			.ChildNodes.Add(childNode32)
			.ChildNodes.Add(childNode33)

		End With

		Dim root4 = New TreeNode("Cashiering")
		tvwMenuItems.Nodes.Add(root4)

		Dim childNode41 As New TreeNode("Collection")
		Dim childNode42 As New TreeNode("Cashier`s Daily Report")
		Dim childNode43 As New TreeNode("Bank Deposit")
		Dim childNode44 As New TreeNode("Check Voucher")
		Dim childNode45 As New TreeNode("Petty Cash Voucher")
		Dim childNode46 As New TreeNode("Revolving Fund Voucher")
		Dim childNode47 As New TreeNode("Fund Replenishment")
		Dim childNode48 As New TreeNode("CWT(2307) Update")
		Dim childNode49 As New TreeNode("Cashier`s Reports")

		With root4
			.ChildNodes.Add(childNode41)
			.ChildNodes.Add(childNode42)
			.ChildNodes.Add(childNode43)
			.ChildNodes.Add(childNode44)
			.ChildNodes.Add(childNode45)
			'.ChildNodes.Add(childNode46)
			.ChildNodes.Add(childNode47)
			.ChildNodes.Add(childNode48)
			.ChildNodes.Add(childNode49)

		End With

		Dim root5 = New TreeNode("Controlling")
		tvwMenuItems.Nodes.Add(root5)

		Dim childNode51 As New TreeNode("Costing Entry")
		Dim childNode52 As New TreeNode("Fund Cash Count")
		Dim childNode53 As New TreeNode("Bank Recon")

		With root5
			.ChildNodes.Add(childNode51)
			.ChildNodes.Add(childNode52)
			.ChildNodes.Add(childNode53)

		End With

		Dim root6 = New TreeNode("General Ledger")
		tvwMenuItems.Nodes.Add(root6)

		Dim childNode61 As New TreeNode("Journal Voucher Entry")
		Dim childNode62 As New TreeNode("Edit Auto JV Entry")
		'Dim childNode63 As New TreeNode("Monthly Transaction")
		Dim childNode64 As New TreeNode("JV Posting")
		Dim childNode65 As New TreeNode("Period Closing")
		'Dim childNode66 As New TreeNode("Standard Cost Update")
		Dim childNode67 As New TreeNode("GL Reports")

		With root6
			.ChildNodes.Add(childNode61)
			.ChildNodes.Add(childNode62)
			'.ChildNodes.Add(childNode63)
			.ChildNodes.Add(childNode64)
			.ChildNodes.Add(childNode65)
			'.ChildNodes.Add(childNode66)
			.ChildNodes.Add(childNode67)
		End With

		Dim root7 = New TreeNode("Masterdata Maintenance")
		tvwMenuItems.Nodes.Add(root7)

		Dim childNode71 As New TreeNode("Chart of Accounts")
		Dim childNode72 As New TreeNode("Cost Center")
		Dim childNode73 As New TreeNode("Product Class Config")
		Dim childNode74 As New TreeNode("Vendor")
		Dim childNode75 As New TreeNode("Bank Center")
		Dim childNode76 As New TreeNode("TC Configuration")

		With root7
			.ChildNodes.Add(childNode71)
			.ChildNodes.Add(childNode72)
			.ChildNodes.Add(childNode73)
			.ChildNodes.Add(childNode74)
			.ChildNodes.Add(childNode75)
			.ChildNodes.Add(childNode76)

		End With

		tvwMenuItems.ExpandAll()

	End Sub

	Private Sub LoadCurrMon()

		Dim strGLStat As String
		strGLStat = "OPEN"

		dt = GetDataTable("select dfrom from gltransmonstatus where yearstat = '" & strGLStat & "' " &
						  "and monstat = '" & strGLStat & "'")
		If Not CBool(dt.Rows.Count) Then
			Exit Sub
		Else
			For Each dr As DataRow In dt.Rows
				vTransMon = dr.Item(0).ToString()

			Next
		End If

		Call dt.Dispose()

		dt = GetDataTable("select dto from gltransmonstatus where yearstat = 'OPEN' " &
						  "and monstat = 'CLOSE' and yearendcltype <> 'Final' and transmon = '12'")
		If Not CBool(dt.Rows.Count) Then
			vTransYear = LastDayOfMonth(vTransMon)
		Else
			For Each dr As DataRow In dt.Rows
				vTransYear = dr.Item(0).ToString()

			Next
		End If

		Call dt.Dispose()

	End Sub

	Private Sub tvwMenuItems_SelectedNodeChanged(sender As Object, e As EventArgs) Handles tvwMenuItems.SelectedNodeChanged
		Session("UserID") = lblUserID.Text
		Session("UserGrp") = lblGrpUser.Text

		Select Case tvwMenuItems.SelectedNode.Text
			'sales Accounting
			Case "Daily Sales Report"
				If IsAllowed(lblGrpUser.Text, "046", 1) = True Then
					Response.Redirect("DailySalesReport.aspx")

				Else
					AdmMsgBox("You're not allowed to this Module")
					Response.Redirect("FinancialAccounting.aspx")

				End If

			Case "A/R Reports"
				If IsAllowed(lblGrpUser.Text, "014", 1) = True Then
					Response.Redirect("ARreports.aspx")

				Else
					AdmMsgBox("You're not allowed to this Module")
					Response.Redirect("FinancialAccounting.aspx")

				End If

			Case "A/P Reports"
				If IsAllowed(lblGrpUser.Text, "032", 1) = True Then
					Response.Redirect("APreports.aspx")

				Else
					AdmMsgBox("You're not allowed to this Module")
					Response.Redirect("FinancialAccounting.aspx")

				End If

			Case "Financial Reports"
				If IsAllowed(lblGrpUser.Text, "063", 1) = True Then
					Select Case vLoggedBussArea
						Case "8100"
							Response.Redirect("FS.aspx")
						Case "8200"
							'Response.Redirect("FS.aspx")
						Case "8300"
							Response.Redirect("FSChemag.aspx")
					End Select

				Else
					AdmMsgBox("You're not allowed to this Module")
					Response.Redirect("FinancialAccounting.aspx")

				End If

			Case "Document Posting"
				If IsAllowed(lblGrpUser.Text, "027", 1) = True Then
					Response.Redirect("APapp.aspx")

				Else
					AdmMsgBox("You're not allowed to this Module")
					Response.Redirect("FinancialAccounting.aspx")

				End If

			Case "Budget Monitoring"
				Select Case vLoggedBussArea
					Case "8300"
						If IsAllowed(lblGrpUser.Text, "079", 1) = True Then
							Response.Redirect("BudgetMonitor.aspx")

						Else
							AdmMsgBox("You're not allowed to this Module")
							Response.Redirect("FinancialAccounting.aspx")

						End If
					Case Else
						AdmMsgBox("Not Yet Available to your Buss Area")
						Response.Redirect("FinancialAccounting.aspx")

				End Select

			Case "Journal Voucher Entry"
				If IsAllowed(lblGrpUser.Text, "052", 1) = True Then
					Response.Redirect("JournalVoucher.aspx")

				Else
					AdmMsgBox("You're not allowed to this Module")
					Response.Redirect("FinancialAccounting.aspx")

				End If

			Case "JV Posting"
				If IsAllowed(lblGrpUser.Text, "038", 1) = True Then
					Response.Redirect("JVPosting.aspx")

				Else
					AdmMsgBox("You're not allowed to this Module")
					Response.Redirect("FinancialAccounting.aspx")

				End If

			Case "GL Reports"
				If IsAllowed(lblGrpUser.Text, "052", 1) = True Then
					Response.Redirect("GLReports.aspx")

				Else
					AdmMsgBox("You're not allowed to this Module")
					Response.Redirect("FinancialAccounting.aspx")

				End If


			Case "CA Liquidation"
				If IsAllowed(lblGrpUser.Text, "038", 1) = True Then
					vFormOpen = "CA Liquidation"
					Session("FormOpen") = vFormOpen
					Response.Redirect("Liquidation.aspx")
				Else
					AdmMsgBox("You're not allowed to this Module")
					Response.Redirect("FinancialAccounting.aspx")

				End If

			Case "Cashier's Reports"
				If IsAllowed(lblGrpUser.Text, "047", 1) = True Then
					vFormOpen = "Cashier Reports"
					Session("FormOpen") = vFormOpen
					Response.Redirect("CashierReports.aspx")
				Else
					AdmMsgBox("You're not allowed to this Module")
					Response.Redirect("FinancialAccounting.aspx")

				End If

			Case "COS vs ASP Analysis"
				If IsAllowed(lblGrpUser.Text, "059", 1) = True Then
					vFormOpen = "COS vs ASP Analysis"
					Session("FormOpen") = vFormOpen
					Response.Redirect("COSvsASP.aspx")
				Else
					AdmMsgBox("You're not allowed to this Module")
					Response.Redirect("FinancialAccounting.aspx")

				End If

			Case Else
				AdmMsgBox("Not Yet Available")
				Response.Redirect("FinancialAccounting.aspx")

		End Select

	End Sub

	Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
		If CheckBox1.Checked = True Then
			CheckBox1.Text = "Show Side Menu"
			Panel1.Visible = False
		Else
			CheckBox1.Text = "Hide Side Menu"
			Panel1.Visible = True
		End If
	End Sub
End Class