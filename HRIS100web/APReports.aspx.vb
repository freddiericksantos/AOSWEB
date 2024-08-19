Imports MySql.Data.MySqlClient
Imports System.IO
Imports System.Data.OleDb
Imports System.Data
Imports System.Data.DataTable
Imports System.Data.SqlTypes

Public Class aPReports
	Inherits System.Web.UI.Page
	Dim dt As DataTable
	Dim sql As String
	Dim sqldata As String
	Dim totAmt As Double = 0
	Dim drAmt As Double = 0
	Dim crAmt As Double = 0
	Dim netAmt As Double = 0
	Dim sdrAmt As Double = 0
	Dim scrAmt As Double = 0
	Dim snetAmt As Double = 0
	Dim admDocNo As Integer = 0
	Dim rowIndex As Integer = 1
	Dim TotRowIndex As Integer = 1

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		If lblUser.Text Is Nothing Then
			Response.Redirect("login.aspx")

		End If

		If Not Me.IsPostBack Then
			lblUser.Text = Session("UserID")
			lblGrpUser.Text = Session("UserGrp")

		End If

		lblMsg.Text = "UNDER CONSTRUCTION"

		If Not Me.IsPostBack Then
			Select Case TabContainer1.ActiveTabIndex
				Case 0
					lblTitle.Text = TabPanel1.HeaderText
				Case 1
					lblTitle.Text = TabPanel2.HeaderText
				Case 2
					lblTitle.Text = TabPanel3.HeaderText
				Case 3
					lblTitle.Text = TabPanel4.HeaderText
				Case 4
					lblTitle.Text = TabPanel5.HeaderText
			End Select

			cboDocSource.Items.Clear()
			cboDocSource.Items.Add("")
			cboDocSource.Items.Add("MMRR")

			popTab1MainList()

		End If

	End Sub


	Private Sub popTab1MainList()
		'cboMain.Items.Clear()
		'cboMain.Items.Add("")
		'cboMain.Items.Add("Voucher Payables")
		'cboMain.Items.Add("Petty Cash Fund")
		''cboMain.Items.Add("Weekly Expense Report")
		''cboMain.Items.Add("Revolving Fund")
		'cboMain.Items.Add("CA Liquidation")
		'cboMain.Items.Add("Check Voucher")
		''cboMain.Items.Add("Gas PO")
		'cboMain.Items.Add("All Expenses")

		cboAMUopt.Items.Clear()
		cboAMUopt.Items.Add("")
		cboAMUopt.Items.Add("ALL")
		cboAMUopt.Items.Add("With MMRR")
		cboAMUopt.Items.Add("W/o MMRR")

	End Sub


	Protected Sub lbClose_Click(sender As Object, e As EventArgs)
		Response.Redirect("FinancialAccounting.aspx")
	End Sub

	Private Sub TabContainer1_ActiveTabChanged(sender As Object, e As EventArgs) Handles TabContainer1.ActiveTabChanged
		Select Case TabContainer1.ActiveTabIndex
			Case 0
				lblTitle.Text = TabPanel1.HeaderText
			Case 1
				lblTitle.Text = TabPanel2.HeaderText
			Case 2
				lblTitle.Text = TabPanel3.HeaderText
			Case 3
				lblTitle.Text = TabPanel4.HeaderText
			Case 4
				lblTitle.Text = TabPanel5.HeaderText
		End Select

	End Sub

	Protected Sub dgvStatList_RowDataBound(sender As Object, e As GridViewRowEventArgs)

	End Sub

	Protected Sub dgvStatList_RowCreated(sender As Object, e As GridViewRowEventArgs)

	End Sub

	Private Sub cboDocSource_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboDocSource.SelectedIndexChanged
		lblMsg.Text = ""
		If cboDocSource.Text = "" Then
			Exit Sub

		ElseIf dpDocDate1.Text = Nothing Then
			lblMsg.Text = "Error: Select Date From"
			Exit Sub

		ElseIf dpDocDate2.Text = Nothing Then
			lblMsg.Text = "Error: Select Date To"
			Exit Sub
		ElseIf CDate(dpDocDate1.Text) > CDate(dpDocDate2.Text) Then
			lblMsg.Text = "Error: Invalid Date Selection"
			Exit Sub
		End If

		dgvStatList.DataSource = Nothing
		dgvStatList.DataBind()

		'dgvCVdet.DataSource = Nothing

		'dgvCVdetList.DataSource = Nothing

		cboVendor.Items.Clear()

		Select Case cboDocSource.Text
			Case "MMRR"
				PopVendor()

		End Select

	End Sub

	Private Sub PopVendor()
		cboVendor.Items.Clear()
		dt = GetDataTable("select concat(a.venno,space(1),b.venname) from invhdrtbl a left join venmasttbl b on a.venno=b.venno " &
						  "where a.tc = '10' and a.transdate between '" & Format(CDate(dpDocDate1.Text), "yyyy-MM-dd") & "' and " &
						  "'" & Format(CDate(dpDocDate2.Text), "yyyy-MM-dd") & "' and a.status <> 'void' and " &
						  "a.mov between '101' and '103' group by a.venno order by b.venname")
		If Not CBool(dt.Rows.Count) Then
			MsgBox("No Vendor Found")
			Exit Sub
		Else
			cboVendor.Items.Add("")
			cboVendor.Items.Add("ALL")
			For Each dr As DataRow In dt.Rows
				cboVendor.Items.Add(dr.Item(0).ToString() & "")
			Next

		End If

		Call dt.Dispose()

	End Sub

	Private Sub cboVendor_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboVendor.SelectedIndexChanged
		If cboVendor.Text = "" Then
			Exit Sub

		End If

		TempDataDocStat()
		FillPayStat()


	End Sub

	Private Sub TempDataDocStat()
		Select Case cboDocSource.Text
			Case "MMRR"
				dt = GetDataTable("select * from information_schema.tables where table_schema =  '" & vDbase & "' and " &
								  "table_name = 'tempinvdettbl'")
				If Not CBool(dt.Rows.Count) Then
					sql = "CREATE TABLE tempinvdettbl LIKE invdettbl"
					ExecuteNonQuery(sql)
				Else
					sql = "delete from tempinvdettbl where dsrstat = '" & lblUser.Text & "'"
					ExecuteNonQuery(sql)
				End If

				dt.Dispose()

				Select Case cboVendor.Text
					Case "ALL"
						sql = "insert into tempinvdettbl(mmrrno,transdate,venno,detamt,dsrstat) select mmrrno,transdate,venno," &
							  "ifnull(netamt,0),'" & lblUser.Text & "' from invhdrtbl where status <> 'void' and mov between '101' and '103' " &
							  "and tc='10' and transdate between '" & Format(CDate(dpDocDate1.Text), "yyyy-MM-dd") & "' and " &
							  "'" & Format(CDate(dpDocDate2.Text), "yyyy-MM-dd") & "'"
						ExecuteNonQuery(sql)

						'update VP
						sql = "update tempinvdettbl a,(select docno,transdate,docamt,mmrrno,status from exphdrtbl where reflabel = 'MMRR' and " &
							  "status <> 'void') as b set a.dsrno = ifnull(b.docno,'No VP'),a.sp = b.docamt,a.expdate = b.transdate," &
							  "a.status = b.status where a.mmrrno=b.mmrrno and a.dsrstat = '" & lblUser.Text & "'"
						ExecuteNonQuery(sql)

					Case Else
						sql = "insert into tempinvdettbl(mmrrno,transdate,venno,detamt,dsrstat) select mmrrno,transdate,venno," &
							  "ifnull(netamt,0),'" & lblUser.Text & "' from invhdrtbl where status <> 'void' and mov between '101' and '103' " &
							  "and tc='10' and transdate between '" & Format(CDate(dpDocDate1.Text), "yyyy-MM-dd") & "' and " &
							  "'" & Format(CDate(dpDocDate2.Text), "yyyy-MM-dd") & "' and venno = '" & cboVendor.Text.Substring(0, 5) & "'"
						ExecuteNonQuery(sql)

						'update VP
						sql = "update tempinvdettbl a,(select docno,transdate,docamt,mmrrno,status from exphdrtbl where reflabel = 'MMRR' and " &
							  "status <> 'void' and venno = '" & cboVendor.Text.Substring(0, 5) & "') as b set a.dsrno = ifnull(b.docno,'No VP')," &
							  "a.sp = b.docamt,a.expdate = b.transdate,a.status = b.status where a.mmrrno=b.mmrrno and a.dsrstat = '" & lblUser.Text & "'"
						ExecuteNonQuery(sql)

				End Select

		End Select

	End Sub

	Private Sub FillPayStat()
		dgvStatList.DataSource = Nothing
		dgvStatList.DataBind()
		'dgvCVdet.DataSource = Nothing

		'dgvCVdetList.DataSource = Nothing


		Dim adapter As New MySqlDataAdapter
		Dim ds As New DataSet()
		Dim i As Integer = 0
		sqldata = Nothing

		Select Case cboDocSource.Text
			Case "MMRR"
				dt = GetDataTable("select * from tempinvdettbl where dsrstat = '" & lblUser.Text & "'")
				If Not CBool(dt.Rows.Count) Then
					MsgBox("No MMRR Found")
					dgvStatList.DataSource = Nothing
					dgvStatList.DataBind()
					Exit Sub
				End If

				dt.Dispose()

				sqldata = "select a.mmrrno,a.transdate,concat(a.venno,space(1),b.venname) as ven,ifnull(concat(b.Terms,' Days'),'No Term') as term," &
						  "ifnull(a.detamt,0) as amt,ifnull(a.dsrno,'No VP') as docnovp,a.expdate,ifnull(a.sp,0) as docamt,ucase(a.status) as stat " &
						  "from tempinvdettbl a left join venmasttbl b on a.venno=b.venno where a.dsrstat = '" & lblUser.Text & "' order by a.transdate,a.mmrrno"
		End Select


		With dgvStatList
			.Columns(1).HeaderText = "MMRR No."
			.Columns(2).HeaderText = "Date"
			.Columns(3).HeaderText = "Vendor"
			.Columns(4).HeaderText = "Term"
			.Columns(5).HeaderText = "Amount"
			.Columns(6).HeaderText = "VP No"
			.Columns(7).HeaderText = "VP Date"
			.Columns(8).HeaderText = "VP Amt"
			.Columns(9).HeaderText = "Status"

		End With

		conn.Open()
		Dim command As New MySqlCommand(sqldata, conn)
		adapter.SelectCommand = command
		adapter.Fill(ds)
		adapter.Dispose()
		command.Dispose()
		conn.Close()
		dgvStatList.DataSource = ds.Tables(0)
		dgvStatList.DataBind()


	End Sub

	Private Sub dgvStatList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles dgvStatList.SelectedIndexChanged
		'dgvCVdetList.DataSource = Nothing
		lblTotDetAmt.Text = "0.00 "
		dgvCVdet.DataSource = Nothing
		dgvCVdet.DataBind()

		lblTotCVamt.Text = "0.00 "
		Label10.Text = "CV No. : Total"

		If dgvStatList.SelectedRow.Cells(6).Text = "No VP" Then
			dgvCVdet.DataSource = Nothing
			lblTotCVamt.Text = "0.00  "
			lblMsgNoCheck.Visible = True
			Exit Sub
		Else
			Select Case UCase(dgvStatList.SelectedRow.Cells(9).Text)
				Case "POSTED"
					'dgvStatList.Rows(e.RowIndex).DefaultCellStyle = New DataGridViewCellStyle With {.Font = New Drawing.Font("Segoe UI", 11.0!, FontStyle.Bold)}
					'BoldedRows.Add(dgvStatList.Rows(e.RowIndex))

					FillCVdetails()

				Case Else
					dgvCVdet.DataSource = Nothing
					lblTotCVamt.Text = "0.00 "
					Exit Sub
			End Select
		End If

	End Sub

	Private Sub FillCVdetails()

		dt = GetDataTable("select ifnull(sum(a.dramt),0) from cvdettbl a left join cvhdrtbl b on a.docno=b.docno " &
						  "where a.vpno = '" & dgvStatList.SelectedRow.Cells(6).Text & "' and b.status <> 'void' " &
						  "group by a.vpno")
		If Not CBool(dt.Rows.Count) Then
			lblMsgNoCheck.Visible = True
			dgvCVdet.DataSource = Nothing
			lblTotCVamt.Text = "0.00 "
			Exit Sub
		Else
			lblMsgNoCheck.Visible = False
			For Each dr As DataRow In dt.Rows
				lblTotCVamt.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0.00") & " "
			Next

		End If

		dt.Dispose()

		Dim adapter As New MySqlDataAdapter
		Dim ds As New DataSet()

		sqldata = Nothing

		sqldata = "select b.docno,b.chdate,b.chno,ifnull(a.dramt,0) as dramt from cvdettbl a " &
				  "left join cvhdrtbl b on a.docno=b.docno where b.status <> 'void' and " &
				  "a.vpno = '" & dgvStatList.SelectedRow.Cells(6).Text & "'"

		With dgvCVdet
			.Columns(0).HeaderText = "CV No."
			.Columns(1).HeaderText = "Ch Date"
			.Columns(2).HeaderText = "Check No"
			.Columns(3).HeaderText = "Amount"

		End With

		conn.Open()
		Dim command As New MySqlCommand(sqldata, conn)
		adapter.SelectCommand = command
		adapter.Fill(ds)
		adapter.Dispose()
		command.Dispose()
		conn.Close()
		dgvCVdet.DataSource = ds.Tables(0)
		dgvCVdet.DataBind()

		dgvCVdet.SelectedIndex = 0

		If dgvCVdet.Rows.Count > 0 Then
			Dim i As Integer
			For i = 0 To dgvCVdet.Rows.Count - 1
				If i = 0 Then
					Label10.Text = "CV No.: Total"
					FillCVdetList()
				End If
			Next
		End If



	End Sub

	Private Sub FillCVdetList()
		dt = GetDataTable("select ifnull(sum(dramt),0) from cvdettbl where docno = '" & dgvCVdet.SelectedRow.Cells(0).Text & "' " &
						  "and tcref ='42' group by docno")
		If Not CBool(dt.Rows.Count) Then
			MsgBox("No Check Voucher Detailed Found")
			dgvCVdetList.DataSource = Nothing
			lblTotDetAmt.Text = "0.00 "
			Exit Sub
		Else
			For Each dr As DataRow In dt.Rows
				lblTotDetAmt.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0.00") & " "
			Next

			Label10.Text = "CV No.: " & dgvCVdet.SelectedRow.Cells(0).Text & " Total"

		End If

		dt.Dispose()

		Dim adapter As New MySqlDataAdapter
		Dim ds As New DataSet()

		sqldata = Nothing
		sqldata = "select a.itemno,a.vpno,a.tcref,b.transdate,ifnull(a.dramt,0) as dramt from cvdettbl a " &
				  "left join exphdrtbl b on a.vpno=b.docno and a.tcref=b.tc where " &
				  "a.docno = '" & dgvCVdet.SelectedRow.Cells(0).Text & "' and a.tcref ='42' order by a.itemno"

		With dgvCVdetList
			.Columns(0).HeaderText = "Item No."
			.Columns(1).HeaderText = "VP No."
			.Columns(2).HeaderText = "TC"
			.Columns(3).HeaderText = "VP Date"
			.Columns(4).HeaderText = "Amount"

		End With

		conn.Open()
		Dim command As New MySqlCommand(sqldata, conn)
		adapter.SelectCommand = command
		adapter.Fill(ds)
		adapter.Dispose()
		command.Dispose()
		conn.Close()
		dgvCVdetList.DataSource = ds.Tables(0)
		dgvCVdetList.DataBind()

		Dim i As Integer

		For i = 0 To dgvCVdetList.Rows.Count - 1
			If dgvStatList.SelectedRow.Cells(6).Text = dgvCVdetList.Rows(i).Cells(1).Text Then
				dgvCVdetList.SelectedIndex = i
			End If
		Next

	End Sub


	Protected Sub dgvCVdet_RowDataBound(sender As Object, e As GridViewRowEventArgs)

	End Sub

	Protected Sub dgvCVdet_RowCreated(sender As Object, e As GridViewRowEventArgs)

	End Sub

	Protected Sub dgvCVdetList_RowDataBound(sender As Object, e As GridViewRowEventArgs)

	End Sub

	Protected Sub dgvCVdetList_RowCreated(sender As Object, e As GridViewRowEventArgs)

	End Sub

	Private Sub cboMain_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboMain.SelectedIndexChanged
		dgvRegSum.DataSource = Nothing
		dgvRegSum.DataBind()

		If cboMain.Text = "" Then
			Exit Sub
		ElseIf dpTransDate.Text = Nothing Then
			Exit Sub
		ElseIf dpTransDate2.Text = Nothing Then
			Exit Sub
		ElseIf CDate(dpTransDate.Text) > CDate(dpTransDate2.Text) Then
			Exit Sub
		End If

		cboFormat.Items.Clear()
		cboFormat.Items.Add("")

		Select Case cboMain.Text
			Case "Voucher Payables"
				cboFormat.Items.Add("Register - Summary")
				cboFormat.Items.Add("Register - Detailed")
				cboFormat.Items.Add("AP Aging-Detailed")
				cboFormat.Items.Add("AP Aging-Summary")
				cboFormat.Items.Add("Summary per Account")
				cboFormat.Items.Add("Summary per CC")
				cboFormat.Items.Add("Unposted " & cboMain.Text)
				RadioButton1.Visible = False
				RadioButton2.Visible = False
				Tab1Panel2.Visible = False

			Case "Weekly Expense Report"
				cboFormat.Items.Add("Register - Summary")
				cboFormat.Items.Add("Register - Detailed")
				cboFormat.Items.Add("Summary per Account")
				cboFormat.Items.Add("Summary per CC")
				cboFormat.Items.Add("Unposted " & cboMain.Text)
				'cboFormat.Items.Add("Summary per Vendor")
				RadioButton1.Visible = False
				RadioButton2.Visible = False
				Tab1Panel2.Visible = False

			Case "Revolving Fund"
				cboFormat.Items.Add("Register - Summary")
				cboFormat.Items.Add("Register - Detailed")
				cboFormat.Items.Add("Summary per Account")
				cboFormat.Items.Add("Summary per CC")
				'cboFormat.Items.Add("Summary per Vendor")
				cboFormat.Items.Add("Unposted " & cboMain.Text)
				RadioButton1.Visible = False
				RadioButton2.Visible = False

			Case "All Expenses"
				cboFormat.Items.Add("Summary per Account")
				cboFormat.Items.Add("Summary per CC")
				cboFormat.Items.Add("Single Account")
				'cboFormat.Items.Add("Per Account - Detailed")
				RadioButton1.Visible = False
				RadioButton2.Visible = False
				Tab1Panel2.Visible = False

			Case "Check Voucher"
				cboFormat.Items.Add("Register - Summary")
				cboFormat.Items.Add("Register - Detailed")
				RadioButton1.Visible = False
				RadioButton2.Visible = False
				Tab1Panel2.Visible = False

			Case Else
				cboFormat.Items.Add("Register - Summary")
				cboFormat.Items.Add("Register - Detailed")
				cboFormat.Items.Add("Summary per Account")
				cboFormat.Items.Add("Summary per CC")
				'cboFormat.Items.Add("Summary per Vendor")
				RadioButton1.Visible = False
				RadioButton2.Visible = False
				Tab1Panel2.Visible = False

		End Select

	End Sub

	Private Sub clrDataGridReg()
		If dgvRegSum.Rows.Count > 0 Then
			dgvRegSum.DataSource = Nothing
			dgvRegSum.DataBind()

		End If

		If dgvRegDet.Rows.Count > 0 Then
			dgvRegDet.DataSource = Nothing
			dgvRegDet.DataBind()

		End If

	End Sub


	Private Sub cboFormat_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboFormat.SelectedIndexChanged

		clrDataGridReg()


		If cboMain.Text = "" Then
			Exit Sub
		ElseIf dpTransDate.Text = Nothing Then
			Exit Sub
		ElseIf dpTransDate2.Text = Nothing Then
			Exit Sub
		ElseIf CDate(dpTransDate.Text) > CDate(dpTransDate2.Text) Then
			Exit Sub
		ElseIf cboFormat.Text = "" Then
			Exit Sub
		End If

		cboFilter1.Items.Clear()
		cboFilter1.Items.Add("")

		Select Case cboMain.Text
			Case "Voucher Payables"
				Select Case cboFormat.Text
					Case "AP Aging-Detailed"
						lblDate2.Visible = False
						dpTransDate2.Visible = False
						lblDate.Text = "AS of: "
						cboFilter1.Items.Add("ALL")
						cboFilter1.Items.Add("Vendor")


					Case "AP Aging-Summary"
						lblDate2.Visible = False
						dpTransDate2.Visible = False
						lblDate.Text = "AS of: "
						cboFilter1.Items.Add("ALL")

					Case "Summary per Account"

						cboFilter1.Items.Add("ALL")
						cboFilter1.Items.Add("Vendor")


					Case "Summary per CC"
						popCCtrDescF1()

					Case Else
						dpTransDate2.Visible = True
						lblDate2.Visible = True
						lblDate.Text = "From: "
						cboFilter1.Items.Add("ALL")
						cboFilter1.Items.Add("Vendor")

				End Select

			Case "All Expenses"
				Select Case cboFormat.Text
					Case "Summary per Account"
						cboFilter1.Items.Add("ALL")
						cboFilter1.Items.Add("Vendor")


					Case "Summary per CC"
						popCCtrDescF1()

					Case "Single Account"
						popGLacctDescF1()

						'Case "Per Account - Detailed"


				End Select

			Case "Check Voucher"
				cboFilter1.Items.Add("ALL")
				cboFilter1.Items.Add("Vendor")
				cboFilter1.Items.Add("Bank")


			Case "BIR Reports"
				cboFilter2.Items.Clear()
				Select Case cboFormat.Text
					Case "EWT 2307 Register"
						cboFilter1.Items.Add("ALL")

					Case Else

				End Select

				cboFilter1.Items.Add("Vendor")

			Case Else
				cboFilter1.Items.Add("ALL")
				cboFilter1.Items.Add("Vendor")


				Select Case cboFormat.Text

					Case "Summary per CC"
						popCCtrDescF1()

				End Select

		End Select

		btnGenerate.Enabled = True

	End Sub

	Private Sub popGLacctDescF1()

		cboFilter1.Items.Clear()
		dt = GetDataTable("select distinct concat(a.acctno,space(1),b.acctdesc) from expdettbl a left join acctcharttbl b on a.acctno=b.acctno " &
						  "left join exphdrtbl c on a.docno=c.docno where c.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
						  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' order by b.acctdesc")
		If Not CBool(dt.Rows.Count) Then
			Exit Sub
		Else
			cboFilter1.Items.Add("")
			For Each dr As DataRow In dt.Rows
				cboFilter1.Items.Add(dr.Item(0).ToString())

			Next
		End If



		Call dt.Dispose()

	End Sub

	Private Sub popCCtrDescF1()

		cboFilter1.Items.Clear()
		dt = GetDataTable("select distinct concat(ccno,space(1),ccdesc) from cctrnotbl order by ccdesc")
		If Not CBool(dt.Rows.Count) Then
			Exit Sub

		Else
			cboFilter1.Items.Add("")
			For Each dr As DataRow In dt.Rows
				cboFilter1.Items.Add(dr.Item(0).ToString())

			Next


		End If

		Call dt.Dispose()

	End Sub

	Private Sub btnGenerate_Click(sender As Object, e As EventArgs) Handles btnGenerate.Click
		If cboMain.Text = "" Or Nothing Then
			Exit Sub
		ElseIf dpTransDate.Text = Nothing Then
			Exit Sub
		ElseIf dpTransDate2.Text = Nothing Then
			Exit Sub
		ElseIf CDate(dpTransDate.Text) > CDate(dpTransDate2.Text) Then
			Exit Sub
		ElseIf cboFormat.Text = "" Or Nothing Then
			Exit Sub
		End If

		Select Case cboFormat.Text
			Case "Register - Summary"
				Panel5.Visible = False
				Panel4.Visible = True

				If cboFilter1.Text = "" Or Nothing Then
					Exit Sub

				End If

				Select Case cboMain.Text
					Case "Voucher Payables"
						ExpSumReg()

					Case "Petty Cash Fund"
						ExpSumReg()

					Case "Weekly Expense Report"
						ExpSumReg()

					Case "Revolving Fund"
						ExpSumReg()

					Case "CA Liquidation"
						ExpSumReg()

					Case "Gas PO"
						ExpSumReg()

						'Case "Check Voucher"
						'	DeliveryToPrint = "rptCVRegSum"
						'	PrtViewer()

						'Case "Vendor DM/CM"


						'Case "GL Entries"
						'	Select Case cboFilter1.Text
						'		Case "ALL"
						'			DeliveryToPrint = "rptGLentriesSumAll"

						'		Case Else
						'			DeliveryToPrint = "rptGLentriesSum"

						'	End Select

						'	PrtViewer()

				End Select

			Case "Register - Detailed"
				Panel4.Visible = False
				Panel5.Visible = True

				If cboFilter1.Text = "" Or Nothing Then
					Exit Sub

				End If

				Select Case cboMain.Text
					Case "Voucher Payables"
						ExpDetReg()

					Case "Petty Cash Fund"
						ExpDetReg()

					Case "Weekly Expense Report"
						ExpDetReg()

					Case "Revolving Fund"
						ExpDetReg()

					Case "CA Liquidation"
						ExpDetReg()

					Case "Gas PO"
						ExpDetReg()

					Case "Check Voucher"
						'DeliveryToPrint = "rptCVRegDet"
						'PrtViewer()

				End Select

			Case "AP Aging-Detailed"
				Select Case cboMain.Text
					Case "Voucher Payables"
						'getUnpaidVP()
						''getVPsetup()
						'DeliveryToPrint = "rptAPaging"
						'PrtViewer()

				End Select


			Case Else
				lblMsg.Text = "Not Yet Available"
				Exit Sub
		End Select


	End Sub

	Private Sub ExpDetReg()
		lblMsg.Text = ""

		Select Case cboFilter1.Text
			Case "ALL"
				Select Case cboMain.Text
					Case "Voucher Payables"
						dt = GetDataTable("select * from exphdrtbl where tc = '42' and status <> 'void' and " &
										  "transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
										  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "'")

					Case "Petty Cash Fund"
						dt = GetDataTable("select * from exphdrtbl where tc = '40' and status <> 'void' and " &
										  "transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
										  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "'")

					Case "Weekly Expense Report"
						dt = GetDataTable("select * from exphdrtbl where tc = '41' and status <> 'void' and " &
										  "transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
										  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "'")

					Case "Revolving Fund"
						dt = GetDataTable("select * from exphdrtbl where tc = '44' and status <> 'void' and " &
										  "transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
										  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "'")

					Case "CA Liquidation"
						dt = GetDataTable("select * from exphdrtbl where tc = '46' and status <> 'void' and " &
										  "transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
										  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "'")

					Case "Gas PO"
						dt = GetDataTable("select * from exphdrtbl where tc = '48' and status <> 'void' and " &
										  "transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
										  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "'")

				End Select

			Case Else
				If cboFilter2.Text = "" Or Nothing Then
					Exit Sub
				End If

				Select Case cboMain.Text
					Case "Voucher Payables"
						dt = GetDataTable("select * from exphdrtbl where tc = '42' and status <> 'void' and " &
										  "transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
										  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' " &
										  "and venno = '" & cboFilter2.Text.Substring(0, 5) & "'")

					Case "Petty Cash Fund"
						dt = GetDataTable("select * from exphdrtbl where tc = '40' and status <> 'void' and " &
										  "transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
										  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' " &
										  "and venno = '" & cboFilter2.Text.Substring(0, 5) & "'")

					Case "Weekly Expense Report"
						dt = GetDataTable("select * from exphdrtbl where tc = '41' and status <> 'void' and " &
										  "transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
										  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' " &
										  "and venno = '" & cboFilter2.Text.Substring(0, 5) & "'")

					Case "Revolving Fund"
						dt = GetDataTable("select * from exphdrtbl where tc = '44' and status <> 'void' and " &
										  "transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
										  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' " &
										  "and venno = '" & cboFilter2.Text.Substring(0, 5) & "'")

					Case "CA Liquidation"
						dt = GetDataTable("select * from exphdrtbl where tc = '46' and status <> 'void' and " &
										  "transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
										  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' " &
										  "and venno = '" & cboFilter2.Text.Substring(0, 5) & "'")

					Case "Gas PO"
						dt = GetDataTable("select * from exphdrtbl where tc = '48' and status <> 'void' and " &
										  "transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
										  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' " &
										  "and venno = '" & cboFilter2.Text.Substring(0, 5) & "'")

				End Select


		End Select

		If Not CBool(dt.Rows.Count) Then
			dgvRegDet.DataSource = Nothing
			dgvRegDet.DataBind()
			lblMsg.Text = "No " & cboFormat.Text & " Found from Dates selected"
			Exit Sub

		End If

		Call dt.Dispose()

		Dim adapter As New MySqlDataAdapter
		Dim ds As New DataSet()

		sqldata = Nothing

		Select Case cboFilter1.Text
			Case "ALL"
				Select Case cboMain.Text
					Case "Voucher Payables"
						sqldata = "select distinct a.docno,d.tc,d.transdate,concat(d.venno,space(1),f.venname) as venno,concat(a.ccno,space(1),b.ccdesc) as ccno," &
								  "concat(a.acctno,space(1),c.acctdesc) as acctno,a.dramt,a.cramt,(a.dramt-a.cramt) as netamt,a.pk,a.venno as subacct " &
								  "from expdettbl a left join exphdrtbl d on a.docno=d.docno left join cctrnotbl b on a.ccno=b.ccno " &
								  "left join acctcharttbl c on a.acctno=c.acctno left join venmasttbl f on d.venno=f.venno " &
								  "where d.tc = '42' and d.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
								  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and a.entryfr = 'Det' order by a.docno"

					Case "Petty Cash Fund"
						sqldata = "select distinct a.docno,d.tc,d.transdate,concat(d.venno,space(1),f.venname) as venno,concat(a.ccno,space(1),b.ccdesc) as ccno," &
								  "concat(a.acctno,space(1),c.acctdesc) as acctno,a.dramt,a.cramt,(a.dramt-a.cramt) as netamt,a.pk,a.venno as subacct " &
								  "from expdettbl a left join exphdrtbl d on a.docno=d.docno left join cctrnotbl b on a.ccno=b.ccno " &
								  "left join acctcharttbl c on a.acctno=c.acctno left join venmasttbl f on d.venno=f.venno " &
								  "where d.tc = '40' and d.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
								  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and a.entryfr = 'Det' order by a.docno"

					Case "Weekly Expense Report"
						sqldata = "select distinct a.docno,d.tc,d.transdate,concat(d.venno,space(1),f.venname) as venno,concat(a.ccno,space(1),b.ccdesc) as ccno," &
								  "concat(a.acctno,space(1),c.acctdesc) as acctno,a.dramt,a.cramt,(a.dramt-a.cramt) as netamt,a.pk,a.venno as subacct " &
								  "from expdettbl a left join exphdrtbl d on a.docno=d.docno left join cctrnotbl b on a.ccno=b.ccno " &
								  "left join acctcharttbl c on a.acctno=c.acctno left join venmasttbl f on d.venno=f.venno " &
								  "where d.tc = '41' and d.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
								  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and a.entryfr = 'Det' order by a.docno"

					Case "Revolving Fund"
						sqldata = "select distinct a.docno,d.tc,d.transdate,concat(d.venno,space(1),f.venname) as venno,concat(a.ccno,space(1),b.ccdesc) as ccno," &
								  "concat(a.acctno,space(1),c.acctdesc) as acctno,a.dramt,a.cramt,(a.dramt-a.cramt) as netamt,a.pk,a.venno as subacct " &
								  "from expdettbl a left join exphdrtbl d on a.docno=d.docno left join cctrnotbl b on a.ccno=b.ccno " &
								  "left join acctcharttbl c on a.acctno=c.acctno left join venmasttbl f on d.venno=f.venno " &
								  "where d.tc = '44' and d.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
								  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and a.entryfr = 'Det' order by a.docno"

					Case "CA Liquidation"
						sqldata = "select distinct a.docno,d.tc,d.transdate,concat(d.venno,space(1),f.venname) as venno,concat(a.ccno,space(1),b.ccdesc) as ccno," &
								  "concat(a.acctno,space(1),c.acctdesc) as acctno,a.dramt,a.cramt,(a.dramt-a.cramt) as netamt,a.pk,a.venno as subacct " &
								  "from expdettbl a left join exphdrtbl d on a.docno=d.docno left join cctrnotbl b on a.ccno=b.ccno " &
								  "left join acctcharttbl c on a.acctno=c.acctno left join venmasttbl f on d.venno=f.venno " &
								  "where d.tc = '46' and d.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
								  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and a.entryfr = 'Det' order by a.docno"

					Case "Gas PO"
						sqldata = "select distinct a.docno,d.tc,d.transdate,concat(d.venno,space(1),f.venname) as venno,concat(a.ccno,space(1),b.ccdesc) as ccno," &
								  "concat(a.acctno,space(1),c.acctdesc) as acctno,a.dramt,a.cramt,(a.dramt-a.cramt) as netamt,a.pk,a.venno as subacct " &
								  "from expdettbl a left join exphdrtbl d on a.docno=d.docno left join cctrnotbl b on a.ccno=b.ccno " &
								  "left join acctcharttbl c on a.acctno=c.acctno left join venmasttbl f on d.venno=f.venno " &
								  "where d.tc = '48' and d.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
								  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and a.entryfr = 'Det' order by a.docno"

				End Select

			Case Else
				Select Case cboMain.Text
					Case "Voucher Payables"
						sqldata = "select distinct a.docno,d.tc,d.transdate,concat(d.venno,space(1),f.venname) as venno,concat(a.ccno,space(1),b.ccdesc) as ccno," &
								  "concat(a.acctno,space(1),c.acctdesc) as acctno,a.dramt,a.cramt,(a.dramt-a.cramt) as netamt,a.pk,a.venno as subacct " &
								  "from expdettbl a left join exphdrtbl d on a.docno=d.docno left join cctrnotbl b on a.ccno=b.ccno " &
								  "left join acctcharttbl c on a.acctno=c.acctno left join venmasttbl f on d.venno=f.venno " &
								  "where d.tc = '42' and d.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
								  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and a.entryfr = 'Det' and " &
								  "a.venno = '" & cboFilter2.Text.Substring(0, 5) & "' order by a.docno"

					Case "Petty Cash Fund"
						sqldata = "select distinct a.docno,d.tc,d.transdate,concat(d.venno,space(1),f.venname) as venno,concat(a.ccno,space(1),b.ccdesc) as ccno," &
								  "concat(a.acctno,space(1),c.acctdesc) as acctno,a.dramt,a.cramt,(a.dramt-a.cramt) as netamt,a.pk,a.venno as subacct " &
								  "from expdettbl a left join exphdrtbl d on a.docno=d.docno left join cctrnotbl b on a.ccno=b.ccno " &
								  "left join acctcharttbl c on a.acctno=c.acctno left join venmasttbl f on d.venno=f.venno " &
								  "where d.tc = '40' and d.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
								  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and a.entryfr = 'Det' and " &
								  "a.venno = '" & cboFilter2.Text.Substring(0, 5) & "' order by a.docno"

					Case "Weekly Expense Report"
						sqldata = "select distinct a.docno,d.tc,d.transdate,concat(d.venno,space(1),f.venname) as venno,concat(a.ccno,space(1),b.ccdesc) as ccno," &
								  "concat(a.acctno,space(1),c.acctdesc) as acctno,a.dramt,a.cramt,(a.dramt-a.cramt) as netamt,a.pk,a.venno as subacct " &
								  "from expdettbl a left join exphdrtbl d on a.docno=d.docno left join cctrnotbl b on a.ccno=b.ccno " &
								  "left join acctcharttbl c on a.acctno=c.acctno left join venmasttbl f on d.venno=f.venno " &
								  "where d.tc = '41' and d.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
								  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and a.entryfr = 'Det' and " &
								  "a.venno = '" & cboFilter2.Text.Substring(0, 5) & "' order by a.docno"

					Case "Revolving Fund"
						sqldata = "select distinct a.docno,d.tc,d.transdate,concat(d.venno,space(1),f.venname) as venno,concat(a.ccno,space(1),b.ccdesc) as ccno," &
								  "concat(a.acctno,space(1),c.acctdesc) as acctno,a.dramt,a.cramt,(a.dramt-a.cramt) as netamt,a.pk,a.venno as subacct " &
								  "from expdettbl a left join exphdrtbl d on a.docno=d.docno left join cctrnotbl b on a.ccno=b.ccno " &
								  "left join acctcharttbl c on a.acctno=c.acctno left join venmasttbl f on d.venno=f.venno " &
								  "where d.tc = '44' and d.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
								  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and a.entryfr = 'Det' and " &
								  "a.venno = '" & cboFilter2.Text.Substring(0, 5) & "' order by a.docno"

					Case "CA Liquidation"
						sqldata = "select distinct a.docno,d.tc,d.transdate,concat(d.venno,space(1),f.venname) as venno,concat(a.ccno,space(1),b.ccdesc) as ccno," &
								  "concat(a.acctno,space(1),c.acctdesc) as acctno,a.dramt,a.cramt,(a.dramt-a.cramt) as netamt,a.pk,a.venno as subacct " &
								  "from expdettbl a left join exphdrtbl d on a.docno=d.docno left join cctrnotbl b on a.ccno=b.ccno " &
								  "left join acctcharttbl c on a.acctno=c.acctno left join venmasttbl f on d.venno=f.venno " &
								  "where d.tc = '46' and d.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
								  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and a.entryfr = 'Det' and " &
								  "a.venno = '" & cboFilter2.Text.Substring(0, 5) & "' order by a.docno"

					Case "Gas PO"
						sqldata = "select distinct a.docno,d.tc,d.transdate,concat(d.venno,space(1),f.venname) as venno,concat(a.ccno,space(1),b.ccdesc) as ccno," &
								  "concat(a.acctno,space(1),c.acctdesc) as acctno,a.dramt,a.cramt,(a.dramt-a.cramt) as netamt,a.pk,a.venno as subacct " &
								  "from expdettbl a left join exphdrtbl d on a.docno=d.docno left join cctrnotbl b on a.ccno=b.ccno " &
								  "left join acctcharttbl c on a.acctno=c.acctno left join venmasttbl f on d.venno=f.venno " &
								  "where d.tc = '48' and d.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
								  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and a.entryfr = 'Det' and " &
								  "a.venno = '" & cboFilter2.Text.Substring(0, 5) & "' order by a.docno"

				End Select

		End Select

		With dgvRegDet
			.Columns(0).HeaderText = "Doc No."
			.Columns(1).HeaderText = "TC."
			.Columns(2).HeaderText = "Date"
			.Columns(3).HeaderText = "Vendor"
			.Columns(4).HeaderText = "Cost Center"
			.Columns(5).HeaderText = "GL Account"
			.Columns(6).HeaderText = "DR Amt"
			.Columns(7).HeaderText = "CR Amt"
			.Columns(8).HeaderText = "Net Amt"
			.Columns(9).HeaderText = "PK"
			.Columns(10).HeaderText = "Sub"
		End With

		conn.Open()
		Dim command As New MySqlCommand(sqldata, conn)
		adapter.SelectCommand = command
		adapter.Fill(ds)
		adapter.Dispose()
		command.Dispose()
		conn.Close()
		dgvRegDet.DataSource = ds.Tables(0)
		dgvRegDet.DataBind()

	End Sub

	Private Sub ExpSumReg()

		lblMsg.Text = ""

		Select Case cboFilter1.Text
			Case "ALL"
				Select Case cboMain.Text
					Case "Voucher Payables"
						dt = GetDataTable("select * from exphdrtbl where tc = '42' and status <> 'void' and " &
										  "transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
										  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "'")

					Case "Petty Cash Fund"
						dt = GetDataTable("select * from exphdrtbl where tc = '40' and status <> 'void' and " &
										  "transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
										  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "'")

					Case "Weekly Expense Report"
						dt = GetDataTable("select * from exphdrtbl where tc = '41' and status <> 'void' and " &
										  "transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
										  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "'")

					Case "Revolving Fund"
						dt = GetDataTable("select * from exphdrtbl where tc = '44' and status <> 'void' and " &
										  "transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
										  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "'")

					Case "CA Liquidation"
						dt = GetDataTable("select * from exphdrtbl where tc = '46' and status <> 'void' and " &
										  "transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
										  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "'")

					Case "Gas PO"
						dt = GetDataTable("select * from exphdrtbl where tc = '48' and status <> 'void' and " &
										  "transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
										  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "'")

				End Select

			Case Else
				If cboFilter2.Text = "" Or Nothing Then
					Exit Sub
				End If

				Select Case cboMain.Text
					Case "Voucher Payables"
						dt = GetDataTable("select * from exphdrtbl where tc = '42' and status <> 'void' and " &
										  "transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
										  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' " &
										  "and venno = '" & cboFilter2.Text.Substring(0, 5) & "'")

					Case "Petty Cash Fund"
						dt = GetDataTable("select * from exphdrtbl where tc = '40' and status <> 'void' and " &
										  "transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
										  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' " &
										  "and venno = '" & cboFilter2.Text.Substring(0, 5) & "'")

					Case "Weekly Expense Report"
						dt = GetDataTable("select * from exphdrtbl where tc = '41' and status <> 'void' and " &
										  "transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
										  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' " &
										  "and venno = '" & cboFilter2.Text.Substring(0, 5) & "'")

					Case "Revolving Fund"
						dt = GetDataTable("select * from exphdrtbl where tc = '44' and status <> 'void' and " &
										  "transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
										  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' " &
										  "and venno = '" & cboFilter2.Text.Substring(0, 5) & "'")

					Case "CA Liquidation"
						dt = GetDataTable("select * from exphdrtbl where tc = '46' and status <> 'void' and " &
										  "transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
										  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' " &
										  "and venno = '" & cboFilter2.Text.Substring(0, 5) & "'")

					Case "Gas PO"
						dt = GetDataTable("select * from exphdrtbl where tc = '48' and status <> 'void' and " &
										  "transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
										  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' " &
										  "and venno = '" & cboFilter2.Text.Substring(0, 5) & "'")

				End Select


		End Select

		If Not CBool(dt.Rows.Count) Then
			dgvRegSum.DataSource = Nothing
			dgvRegSum.DataBind()
			lblMsg.Text = "No " & cboFormat.Text & " Found from Dates selected"
			Exit Sub

		End If

		Call dt.Dispose()

		Dim adapter As New MySqlDataAdapter
		Dim ds As New DataSet()

		sqldata = Nothing

		Select Case cboFilter1.Text
			Case "ALL"
				Select Case cboMain.Text
					Case "Voucher Payables"
						sqldata = "select a.docno,a.tc,a.transdate,a.refdoc,concat(a.venno,space(1),b.venname) as ven,a.docamt," &
								  "ucase(ifnull(a.cvno,'Unpaid')) as stat,a.remarks from exphdrtbl a left join venmasttbl b on a.venno=b.venno " &
								  "where a.tc = '42' and a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
								  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and a.status <> 'void' group by a.docno"

					Case "Petty Cash Fund"
						sqldata = "select a.docno,a.tc,a.transdate,a.refdoc,concat(a.venno,space(1),b.venname) as ven,a.docamt," &
								  "ucase(a.status) as stat,a.remarks from exphdrtbl a left join venmasttbl b on a.venno=b.venno " &
								  "where a.tc = '40' and a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
								  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and a.status <> 'void' group by a.docno"

					Case "Weekly Expense Report"
						sqldata = "select a.docno,a.tc,a.transdate,a.refdoc,concat(a.venno,space(1),b.venname) as ven,a.docamt," &
								  "ucase(a.status) as stat,a.remarks from exphdrtbl a left join venmasttbl b on a.venno=b.venno " &
								  "where a.tc = '41' and a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
								  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and a.status <> 'void' group by a.docno"

					Case "Revolving Fund"
						sqldata = "select a.docno,a.tc,a.transdate,a.refdoc,concat(a.venno,space(1),b.venname) as ven,a.docamt," &
								  "ucase(a.status) as stat,a.remarks from exphdrtbl a left join venmasttbl b on a.venno=b.venno " &
								  "where a.tc = '44' and a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
								  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and a.status <> 'void' group by a.docno"

					Case "CA Liquidation"
						sqldata = "select a.docno,a.tc,a.transdate,a.refdoc,concat(a.venno,space(1),b.venname) as ven,a.docamt," &
								  "ucase(a.status) as stat,a.remarks from exphdrtbl a left join venmasttbl b on a.venno=b.venno " &
								  "where a.tc = '46' and a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
								  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and a.status <> 'void' group by a.docno"

					Case "Gas PO"
						sqldata = "select a.docno,a.tc,a.transdate,a.refdoc,concat(a.venno,space(1),b.venname) as ven,a.docamt," &
								  "ucase(a.status) as stat,a.remarks from exphdrtbl a left join venmasttbl b on a.venno=b.venno " &
								  "where a.tc = '48' and a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
								  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and a.status <> 'void' group by a.docno"

				End Select

			Case Else
				Select Case cboMain.Text
					Case "Voucher Payables"
						sqldata = "select a.docno,a.tc,a.transdate,a.refdoc,concat(a.venno,space(1),b.venname) as ven,a.docamt," &
								  "ucase(ifnull(a.cvno,'Unpaid')) as stat,a.remarks from exphdrtbl a left join venmasttbl b on a.venno=b.venno " &
								  "where a.tc = '42' and a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
								  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and a.status <> 'void' and " &
								  "a.venno = '" & cboFilter2.Text.Substring(0, 5) & "' group by a.docno"

					Case "Petty Cash Fund"
						sqldata = "select a.docno,a.tc,a.transdate,a.refdoc,concat(a.venno,space(1),b.venname) as ven,a.docamt," &
								  "ucase(a.status) as stat,a.remarks from exphdrtbl a left join venmasttbl b on a.venno=b.venno " &
								  "where a.tc = '40' and a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
								  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and a.status <> 'void' and " &
								  "a.venno = '" & cboFilter2.Text.Substring(0, 5) & "' group by a.docno"

					Case "Weekly Expense Report"
						sqldata = "select a.docno,a.tc,a.transdate,a.refdoc,concat(a.venno,space(1),b.venname) as ven,a.docamt," &
								  "ucase(a.status) as stat,a.remarks from exphdrtbl a left join venmasttbl b on a.venno=b.venno " &
								  "where a.tc = '41' and a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
								  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and a.status <> 'void' and " &
								  "a.venno = '" & cboFilter2.Text.Substring(0, 5) & "' group by a.docno"

					Case "Revolving Fund"
						sqldata = "select a.docno,a.tc,a.transdate,a.refdoc,concat(a.venno,space(1),b.venname) as ven,a.docamt," &
								  "ucase(a.status) as stat,a.remarks from exphdrtbl a left join venmasttbl b on a.venno=b.venno " &
								  "where a.tc = '44' and a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
								  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and a.status <> 'void' and " &
								  "a.venno = '" & cboFilter2.Text.Substring(0, 5) & "' group by a.docno"

					Case "CA Liquidation"
						sqldata = "select a.docno,a.tc,a.transdate,a.refdoc,concat(a.venno,space(1),b.venname) as ven,a.docamt," &
								  "ucase(a.status) as stat,a.remarks from exphdrtbl a left join venmasttbl b on a.venno=b.venno " &
								  "where a.tc = '46' and a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
								  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and a.status <> 'void' and " &
								  "a.venno = '" & cboFilter2.Text.Substring(0, 5) & "' group by a.docno"

					Case "Gas PO"
						sqldata = "select a.docno,a.tc,a.transdate,a.refdoc,concat(a.venno,space(1),b.venname) as ven,a.docamt," &
								  "ucase(a.status) as stat,a.remarks from exphdrtbl a left join venmasttbl b on a.venno=b.venno " &
								  "where a.tc = '48' and a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
								  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and a.status <> 'void' and " &
								  "a.venno = '" & cboFilter2.Text.Substring(0, 5) & "' group by a.docno"

				End Select

		End Select

		With dgvRegSum
			.Columns(0).HeaderText = "Doc No."
			.Columns(1).HeaderText = "TC."
			.Columns(2).HeaderText = "Date"
			.Columns(3).HeaderText = "Ref No."
			.Columns(4).HeaderText = "Vendor"
			.Columns(5).HeaderText = "Amount"
			Select Case cboMain.Text
				Case "Voucher Payables"
					.Columns(6).HeaderText = "CV No."
				Case Else
					.Columns(6).HeaderText = "Status"
			End Select


			.Columns(7).HeaderText = "Remarks"

		End With

		conn.Open()
		Dim command As New MySqlCommand(sqldata, conn)
		adapter.SelectCommand = command
		adapter.Fill(ds)
		adapter.Dispose()
		command.Dispose()
		conn.Close()
		dgvRegSum.DataSource = ds.Tables(0)
		dgvRegSum.DataBind()


	End Sub

	Protected Sub dgvRegSum_RowDataBound(sender As Object, e As GridViewRowEventArgs)

		If e.Row.RowType = DataControlRowType.DataRow Then
			Dim tBalc2 As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "docamt").ToString())
			totAmt += tBalc2
		End If

		If e.Row.RowType = DataControlRowType.Footer Then
			Dim lblGTotal As Label = DirectCast(e.Row.FindControl("lblGTotal"), Label)
			lblGTotal.Text = "GRAND TOTAL"

			Dim lblTotalBal As Label = DirectCast(e.Row.FindControl("lblTotalNet"), Label)
			lblTotalBal.Text = Format(CDbl(totAmt.ToString()), "#,##0.00 ; (#,##0.00)")
			totAmt = 0

		End If


	End Sub

	Protected Sub dgvRegSum_RowCreated(sender As Object, e As GridViewRowEventArgs)

	End Sub

	Private Sub cboFilter1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboFilter1.SelectedIndexChanged
		clrDataGridReg()


		If cboMain.Text = "" Or Nothing Then
			Exit Sub
		ElseIf dpTransDate.Text = Nothing Then
			Exit Sub
		ElseIf dpTransDate2.Text = Nothing Then
			Exit Sub
		ElseIf CDate(dpTransDate.Text) > CDate(dpTransDate2.Text) Then
			Exit Sub
		ElseIf cboFormat.Text = "" Or Nothing Then
			Exit Sub
		End If

		Select Case cboFilter1.Text
			Case "ALL"
				cboFilter2.Visible = False
				lblFiler2.Visible = False
				cboFilter2.Text = Nothing
				cboFilter3.Visible = False
				lblFilter3.Visible = False

			Case "Vendor"
				cboFilter2.Items.Clear()
				cboFilter2.Items.Add("")
				Select Case cboMain.Text
					Case "BIR Reports"
						'If cboAMUopt.Text = "" Then
						'    MsgBox("Select Report Option")
						'    Exit Sub

						'End If

						'popVendorList()

						'Select Case cboFormat.Text
						'    Case "EWT 2307 Register"
						'        If cboAMUopt.Text = "" Then
						'            MsgBox("Select Report Option")
						'            Exit Sub

						'        End If

						'        If cboFilter1.Text = "ALL" Then
						'            Exit Sub
						'        Else
						'            popVendorList()
						'        End If

						'    Case Else


						'End Select

					Case "Voucher Payables"
						Select Case cboFormat.Text
							Case "AP Aging-Detailed"
								dt = GetDataTable("select distinct concat(a.venno,space(1),b.venname) from exphdrtbl a left join venmasttbl b on a.venno=b.venno " &
												  "where a.transdate <= '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
												  "and a.status = 'posted' and a.vpbals > 0 and a.tc = '42' order by b.venname")

							Case "AP Aging-Summary"
								dt = GetDataTable("select distinct concat(a.venno,space(1),b.venname) from exphdrtbl a left join venmasttbl b on a.venno=b.venno " &
												  "where a.transdate <= '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
												  "and a.status = 'posted' and a.vpbals > 0 and a.tc = '42' order by b.venname")

							Case "Unposted Voucher Payables"
								dt = GetDataTable("select distinct concat(a.venno,space(1),b.venname) from exphdrtbl a left join venmasttbl b on a.venno=b.venno " &
												  "where a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
												  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "'  " &
												  "and a.tc = '42' and (a.status = 'parked' or a.status = 'noted') order by b.venname")

							Case Else
								dt = GetDataTable("select distinct concat(a.venno,space(1),b.venname) from exphdrtbl a left join venmasttbl b on a.venno=b.venno " &
												  "where a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
												  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "'  " &
												  "and a.status <> 'Void' and a.tc = '42' order by b.venname")

						End Select

						If Not CBool(dt.Rows.Count) Then
							lblMsg.Text = "No Vendor/s found."
							Exit Sub
						Else
							For Each dr As DataRow In dt.Rows
								cboFilter2.Items.Add(dr.Item(0).ToString())
							Next
						End If

						Call dt.Dispose()

					Case "Petty Cash Fund" '40
						dt = GetDataTable("select distinct concat(a.venno,space(1),b.venname) from exphdrtbl a left join venmasttbl b on a.venno=b.venno " &
										  "where a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
										  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "'  " &
										  "and a.status <> 'Void' and a.tc = '40' order by b.venname")
						If Not CBool(dt.Rows.Count) Then
							lblMsg.Text = "No Vendor/s found."
							Exit Sub
						Else
							For Each dr As DataRow In dt.Rows
								cboFilter2.Items.Add(dr.Item(0).ToString())
							Next
						End If

						Call dt.Dispose()

					Case "Weekly Expense Report" '41
						Select Case cboFormat.Text
							Case "Unposted Weekly Expense Report"
								dt = GetDataTable("select distinct concat(a.venno,space(1),b.venname) from exphdrtbl a left join venmasttbl b on a.venno=b.venno " &
												 "where a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
												 "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "'  " &
												 "and a.tc = '41' and (a.status = 'parked' or a.status = 'noted') order by b.venname")

							Case Else
								dt = GetDataTable("select distinct concat(a.venno,space(1),b.venname) from exphdrtbl a left join venmasttbl b on a.venno=b.venno " &
												  "where a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
												  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "'  " &
												  "and a.status <> 'Void' and a.tc = '41' order by b.venname")

						End Select

						If Not CBool(dt.Rows.Count) Then
							lblMsg.Text = "No Vendor/s found."
							Exit Sub
						Else
							For Each dr As DataRow In dt.Rows
								cboFilter2.Items.Add(dr.Item(0).ToString())
							Next

						End If

						Call dt.Dispose()

					Case "Revolving Fund" '44
						Select Case cboFormat.Text
							Case "Unposted Revolving Fund"
								dt = GetDataTable("select distinct concat(a.venno,space(1),b.venname) from exphdrtbl a left join venmasttbl b on a.venno=b.venno " &
												 "where a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
												 "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "'  " &
												 "and a.tc = '44' and (a.status = 'parked' or a.status = 'noted') order by b.venname")

							Case Else
								dt = GetDataTable("select distinct concat(a.venno,space(1),b.venname) from exphdrtbl a left join venmasttbl b on a.venno=b.venno " &
												  "where a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
												  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "'  " &
												  "and a.status <> 'Void' and a.tc = '44' order by b.venname")

						End Select

						If Not CBool(dt.Rows.Count) Then
							lblMsg.Text = "No Vendor/s found."
							Exit Sub
						Else
							For Each dr As DataRow In dt.Rows
								cboFilter2.Items.Add(dr.Item(0).ToString())
							Next
						End If

						Call dt.Dispose()

					Case "CA Liquidation" '46 
						dt = GetDataTable("select distinct concat(a.venno,space(1),b.venname) from exphdrtbl a left join venmasttbl b on a.venno=b.venno " &
										  "where a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
										  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "'  " &
										  "and a.status <> 'Void' and a.tc = '46' order by b.venname")
						If Not CBool(dt.Rows.Count) Then lblMsg.Text = "No Vendor/s found." : Exit Sub
						For Each dr As DataRow In dt.Rows
							cboFilter2.Items.Add(dr.Item(0).ToString())
						Next
						Call dt.Dispose()

					Case "Gas PO" '48
						dt = GetDataTable("select distinct concat(a.venno,space(1),b.venname) from exphdrtbl a left join venmasttbl b on a.venno=b.venno " &
										  "where a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
										  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "'  " &
										  "and a.status <> 'Void' and a.tc = '48' order by b.venname")
						If Not CBool(dt.Rows.Count) Then lblMsg.Text = "No Vendor/s found." : Exit Sub
						For Each dr As DataRow In dt.Rows
							cboFilter2.Items.Add(dr.Item(0).ToString())
						Next
						Call dt.Dispose()

					Case "Check Voucher"
						dt = GetDataTable("select distinct concat(a.venno,space(1),b.venname) from cvhdrtbl a left join venmasttbl b on a.venno=b.venno " &
										  "where a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
										  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "'  " &
										  "and a.status <> 'Void' order by b.venname")
						If Not CBool(dt.Rows.Count) Then lblMsg.Text = "No Vendor/s found." : Exit Sub
						For Each dr As DataRow In dt.Rows
							cboFilter2.Items.Add(dr.Item(0).ToString())
						Next
						Call dt.Dispose()

					Case Else
						dt = GetDataTable("select distinct concat(a.venno,space(1),b.venname) from exphdrtbl a left join venmasttbl b on a.venno=b.venno " &
										  "where a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
										  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "'  " &
										  "and a.status <> 'Void' order by b.venname")
						If Not CBool(dt.Rows.Count) Then lblMsg.Text = "No Vendor/s found." : Exit Sub
						For Each dr As DataRow In dt.Rows
							cboFilter2.Items.Add(dr.Item(0).ToString())
						Next
						Call dt.Dispose()

				End Select

				cboFilter2.Visible = True
				lblFiler2.Visible = True

			Case "Bank"
				cboFilter2.Items.Clear()
				Select Case cboMain.Text
					Case "Check Voucher"
						dt = GetDataTable("select distinct bank from cvhdrtbl where transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
										  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and branch = '" & vLoggedBranch & "' " &
										  "and status <> 'Void' order by bank")
						If Not CBool(dt.Rows.Count) Then lblMsg.Text = "No Bank found." : Exit Sub
						For Each dr As DataRow In dt.Rows
							cboFilter2.Items.Add(dr.Item(0).ToString())
						Next
						Call dt.Dispose()
				End Select
				cboFilter2.Visible = True
				lblFiler2.Visible = True

			Case Else
				Select Case cboMain.Text
					Case "All Expenses"
						Select Case cboFormat.Text
							Case "Summary per CC"


							Case Else


						End Select


					Case Else


				End Select


		End Select


	End Sub

	Private Sub cboFilter2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboFilter2.SelectedIndexChanged
		clrDataGridReg()


	End Sub

	Private Sub dpTransDate_TextChanged(sender As Object, e As EventArgs) Handles dpTransDate.TextChanged, dpTransDate2.TextChanged
		If dpTransDate.Text = Nothing Then
			Exit Sub
		ElseIf dpTransDate2.Text = Nothing Then
			Exit Sub
		ElseIf CDate(dpTransDate.Text) > CDate(dpTransDate2.Text) Then
			Exit Sub

		End If

		popMainReport()

	End Sub

	Private Sub popMainReport()
		cboMain.Items.Clear()
		cboMain.Items.Add("")

		dt = GetDataTable("select distinct tc from exphdrtbl where transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
						  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and branch = '" & vLoggedBranch & "' " &
						  "and status <> 'Void'")
		If Not CBool(dt.Rows.Count) Then
			lblMsg.Text = "Expense Found"

		Else
			For Each dr As DataRow In dt.Rows
				Select Case dr.Item(0).ToString()
					Case "42"
						cboMain.Items.Add("Voucher Payables")
					Case "40"
						cboMain.Items.Add("Petty Cash Fund")
					Case "41"
						cboMain.Items.Add("Weekly Expense Report")
					Case "44"
						cboMain.Items.Add("Revolving Fund")
					Case "46"
						cboMain.Items.Add("CA Liquidation")
					Case "48"
						cboMain.Items.Add("Gas PO")
				End Select
			Next

		End If

		Call dt.Dispose()

		dt = GetDataTable("select * from cvhdrtbl where transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
						  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and status <> 'Void' limit 1")
		If Not CBool(dt.Rows.Count) Then
			lblMsg.Text = "Check Voucher Found"
		Else
			For Each dr As DataRow In dt.Rows
				cboMain.Items.Add("Check Voucher")
			Next
		End If

		Call dt.Dispose()

		cboMain.Items.Add("All Expenses")

	End Sub

	Protected Sub dgvRegDet_RowDataBound(sender As Object, e As GridViewRowEventArgs)
		If e.Row.RowType = DataControlRowType.DataRow Then
			admDocNo = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "docno").ToString())

			Dim tBalc2 As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "dramt").ToString())
			sdrAmt += tBalc2
			Dim td30c2 As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "cramt").ToString())
			scrAmt += td30c2
			Dim td60c2 As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "netamt").ToString())
			snetAmt += td60c2

			Dim tdrAmt As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "dramt").ToString())
			drAmt += tdrAmt
			Dim tcrAmt As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "cramt").ToString())
			crAmt += tcrAmt
			Dim tnetAmt As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "netamt").ToString())
			netAmt += tnetAmt

		End If

		'Dim newRow As Boolean = False
		If e.Row.RowType = DataControlRowType.Footer Then
			Dim lblGTotal As Label = DirectCast(e.Row.FindControl("lblGTotal"), Label)
			lblGTotal.Text = "GRAND TOTAL"

			Dim lblTotalBal As Label = DirectCast(e.Row.FindControl("lblTotalDr"), Label)
			lblTotalBal.Text = Format(CDbl(sdrAmt.ToString()), "#,##0.00 ; (#,##0.00)")
			drAmt = 0

			Dim lblTotal30d As Label = DirectCast(e.Row.FindControl("lblTotalCr"), Label)
			lblTotal30d.Text = Format(CDbl(scrAmt.ToString()), "#,##0.00 ; (#,##0.00)")
			crAmt = 0

			Dim lblTotal60d As Label = DirectCast(e.Row.FindControl("lblTotalNet"), Label)
			lblTotal60d.Text = Format(CDbl(snetAmt.ToString()), "#,##0.00 ; (#,##0.00)")
			netAmt = 0

		End If


	End Sub

	Protected Sub dgvRegDet_RowCreated(sender As Object, e As GridViewRowEventArgs)
		Dim newRow As Boolean = False

		If (admDocNo > 0) AndAlso (DataBinder.Eval(e.Row.DataItem, "docno") IsNot Nothing) Then
			If admDocNo <> Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "docno").ToString()) Then
				newRow = True
			End If
		End If
		If (admDocNo > 0) AndAlso (DataBinder.Eval(e.Row.DataItem, "docno") Is Nothing) Then
			newRow = True
			rowIndex = 0
		End If

		If newRow Then
			Dim dgvRegDet As GridView = DirectCast(sender, GridView)
			Dim NewTotalRow As New GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert)
			NewTotalRow.Font.Bold = True
			NewTotalRow.BackColor = System.Drawing.Color.Gray
			NewTotalRow.ForeColor = System.Drawing.Color.White

			Dim HeaderCell As New TableCell()
			HeaderCell.Text = admDocNo & " Total"
			HeaderCell.HorizontalAlign = HorizontalAlign.Center
			HeaderCell.Font.Size = 10
			HeaderCell.ColumnSpan = 6
			NewTotalRow.Cells.Add(HeaderCell)

			HeaderCell = New TableCell()
			HeaderCell.HorizontalAlign = HorizontalAlign.Right
			HeaderCell.Text = Format(CDbl(drAmt.ToString()), "#,##0.00 ; (#,##0.00)")
			NewTotalRow.Cells.Add(HeaderCell)

			HeaderCell = New TableCell()
			HeaderCell.HorizontalAlign = HorizontalAlign.Right
			HeaderCell.Text = Format(CDbl(crAmt.ToString()), "#,##0.00 ; (#,##0.00)")
			NewTotalRow.Cells.Add(HeaderCell)

			HeaderCell = New TableCell()
			HeaderCell.HorizontalAlign = HorizontalAlign.Right
			HeaderCell.Text = Format(CDbl(netAmt.ToString()), "#,##0.00 ; (#,##0.00)")
			NewTotalRow.Cells.Add(HeaderCell)

			HeaderCell = New TableCell()
			HeaderCell.Text = ""
			HeaderCell.HorizontalAlign = HorizontalAlign.Center
			HeaderCell.Font.Size = 10
			HeaderCell.ColumnSpan = 2
			NewTotalRow.Cells.Add(HeaderCell)

			dgvRegDet.Controls(0).Controls.AddAt(e.Row.RowIndex + rowIndex, NewTotalRow)
			rowIndex += 1
			TotRowIndex += 1

			drAmt = 0
			crAmt = 0
			netAmt = 0


		End If

	End Sub

	Protected Sub lbPrint_Click(sender As Object, e As EventArgs)
		Dim UserDetails As New UserDetails
		UserDetails.RepTitle = cboFormat.Text
		UserDetails.TransDateFr = Format(CDate(dpTransDate.Text), "yyyy-MM-dd")
		UserDetails.TransDateTo = Format(CDate(dpTransDate2.Text), "yyyy-MM-dd")
		Session("UserDetails") = UserDetails

		Response.Redirect("ReportViewer.aspx")



	End Sub
End Class