Imports MySql.Data.MySqlClient
Imports System
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports MySql.Data
Imports System.Windows.Forms

Public Class JournalVoucher
	Inherits System.Web.UI.Page
	Dim dt As DataTable
	Dim dt2 As DataTable
	Dim dt3 As DataTable
	Dim sql As String
	Dim strJVpre As String
	Dim strCV As String
	Dim strSalesAcctNo As String
	Dim strDiscAcctNo As String
	Dim strARacctNo As String
	Dim strCCNo As String
	Dim strCCNo2 As String
	Dim grossAmt As Double
	Dim FHamt As Double
	Dim DiscAmt As Double
	Dim NetAmt As Double
	Dim strCustNo As String
	Dim strUser As String
	Dim strInvNo As String
	Dim strPC As String
	Dim strTC As String
	Dim strTransDate As String
	Dim strEntryType As String
	Dim admSup As String
	Dim admPre As String
	Dim admInvAcct As String
	Dim admJVtype As String
	Dim DRamt As Double
	Dim CRamt As Double
	Dim MyDA_conn As New MySqlDataAdapter
	Dim MyDataSet As New DataSet
	Dim MySqlScript As String
	Dim admType As String
	Dim OpenProc As String
	Dim itm As Integer
	Dim sqldata As String
	Dim admCodeType As String

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

		If lblUser.Text Is Nothing Then
			Response.Redirect("login.aspx")

		End If

		If Not Me.IsPostBack Then
			lblUser.Text = Session("UserID")
			lblGrpUser.Text = Session("UserGrp")

		End If

	End Sub

	Protected Sub CheckGroupRights()
		If IsAllowed(lblGrpUser.Text, vThisFormCode, 3) = True Then ' 3 = Insert 
			lbSave.Enabled = True

		Else
			lbSave.Enabled = False

		End If

		If IsAllowed(lblGrpUser.Text, vThisFormCode, 2) = True Then ' 2 = EDIT
			lbSave.Enabled = True
			btnReproc.Enabled = True
			txtJVNo.ReadOnly = False

		Else
			lbSave.Enabled = False
			btnReproc.Enabled = False
			txtJVNo.ReadOnly = True

		End If

		If IsAllowed(lblGrpUser.Text, vThisFormCode, 4) = True Then ' 4 = Delete
			lbDelete.Enabled = True

		Else
			lbDelete.Enabled = False

		End If

	End Sub

	Protected Sub OpenGLstatus()
		Dim strStat As String = "Open"
		dt = GetDataTable("select transyear,transmon,dfrom,dto from gltransmonstatus where yearstat = '" & strStat & "' and " &
						  "monstat = '" & strStat & "'")
		If Not CBool(dt.Rows.Count) Then Exit Sub

		For Each dr As DataRow In dt.Rows
			txtYear.Text = dr.Item(0).ToString()
			txtMon.Text = dr.Item(1).ToString()
			txtDfrom.Text = Format(CDate(dr.Item(2).ToString()), "yyyy-MM-dd")
			txtDto.Text = Format(CDate(dr.Item(3).ToString()), "yyyy-MM-dd")
			dpTransDate.Text = Format(CDate(dr.Item(3).ToString()), "yyyy-MM-dd")

		Next

		Call dt.Dispose()

	End Sub

	Protected Sub GetCCtrAll()
		cboCCtr.Items.Clear()
		dt = GetDataTable("select concat(ccno,space(1),ccdesc) from cctrnotbl where ba = '" & vLoggedBussArea & "' and " &
						  "branch = '" & vLoggedBranch & "' order by ccno")
		If Not CBool(dt.Rows.Count) Then
			Exit Sub
		Else
			cboCCtr.Items.Add("")
			For Each dr As DataRow In dt.Rows
				cboCCtr.Items.Add(dr.Item(0).ToString())

			Next
		End If

		Call dt.Dispose()

	End Sub

	Protected Sub AdmMsgBox(ByVal sMessage As String)
		Dim msg As String
		msg = "<script language='javascript'>"
		msg += "alert('" & sMessage & "');"
		msg += "<" & "/script>"
		Response.Write(msg)

	End Sub

	Protected Sub lbSave_Click(sender As Object, e As EventArgs)
		tssErrMsg.Text = ""
		If CheckBox1.Checked = True Then
			Select Case cboJVsource.Text


			End Select

		End If

		Dim DRtotal As Double = CDbl(IIf(lblDRtot.Text = "", 0, lblDRtot.Text))
		Dim CRtotal As Double = CDbl(IIf(lblCRtot.Text = "", 0, lblCRtot.Text))

		If txtRefNo.Text = "" Then
			tssErrMsg.Text = "Ref. No. is Blank"
			Exit Sub
		ElseIf cboJVsource.Text = "" Then
			tssErrMsg.Text = "JV Source is Blank"
			Exit Sub
		ElseIf cboJVtype.Text = "" Then
			tssErrMsg.Text = "JV Type is Blank"
			Exit Sub
		ElseIf txtRemarks.Text = "" Then
			tssErrMsg.Text = "Remarks is Blank"
			Exit Sub
		ElseIf DRtotal <> CRtotal Then
			tssErrMsg.Text = "DR Amount and CR Amount is not Equal"
			Exit Sub
		ElseIf cbCentry.Checked = True Then
			If cboPC.Text = "" Then
				tssErrMsg.Text = "Select Product Class"
				Exit Sub
			End If
			'ElseIf lvDet.Items.Count = 0 Then
			'    MsgBox("No Line Item Found")
			'    Exit Sub
		End If

		If tssDocStat.Text = "New" Then
			If txtJVNo.Text = "" Or txtJVNo.Text = Nothing Then
				txtJVNo.ReadOnly = False
				getManualJVNo()
			End If

		Else
			If txtJVNo.Text <> vDocNo Then
				tssErrMsg.Text = "JV No. should not be CHANGED"
				txtJVNo.Text = tssDocNo.Text

			End If

		End If

		If txtJVNo.Text = "" Then
			tssErrMsg.Text = "JV No. is Blank"
			Exit Sub

		ElseIf RadioButton1.Checked = False And RadioButton2.Checked = False Then
			tssErrMsg.Text = "Select Entry Type"
			Exit Sub

		End If

		gRepTbox(txtRemarks)
		SaveHdrProc()

		dt = GetDataTable("select * from gljvhdrtbl where jvno = '" & txtJVNo.Text & "'")
		If Not CBool(dt.Rows.Count) Then
			lbDelete.Enabled = False

		Else
			lbDelete.Enabled = True
			lblStatus.Text = "PARK"

		End If

		gRepTboxUndo(txtRemarks)

	End Sub

	Protected Sub lbPrint_Click(sender As Object, e As EventArgs)
		tssErrMsg.Text = ""

	End Sub

	Protected Sub DgvJVdet_RowDataBound(sender As Object, e As GridViewRowEventArgs)

	End Sub

	Protected Sub DgvJVdet_RowCreated(sender As Object, e As GridViewRowEventArgs)

	End Sub

	Private Sub JournalVoucher_Unload(sender As Object, e As EventArgs) Handles Me.Unload


	End Sub

	Protected Sub PopGLDesc()
		tssErrMsg.Text = ""
		cboGLdesc.Items.Clear()
		dt = GetDataTable("select concat(a.acctno,space(1),a.acctdesc) from acctcharttbl a left join " &
						  "cctrnotbl b on a.acctnopre=b.acctnopre where b.ccno = '" & cboCCtr.Text.Substring(0, 5) & "' order by a.acctdesc") ' and a.baxchrgs = '" & "8888" & "'
		If Not CBool(dt.Rows.Count) Then
			Exit Sub
		Else
			cboGLdesc.Items.Add("")
			For Each dr As DataRow In dt.Rows
				cboGLdesc.Items.Add(dr.Item(0).ToString())

			Next

		End If

		Call dt.Dispose()

	End Sub

	Private Sub cboCCtr_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboCCtr.SelectedIndexChanged
		tssErrMsg.Text = ""
		If Not Me.IsPostBack Then

		End If

		If cboCCtr.Text = "" Then
			Exit Sub
		End If

		PopGLDesc()
		txtDRamt.Focus()

	End Sub

	Private Sub cboJVtype_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboJVtype.SelectedIndexChanged
		tssErrMsg.Text = ""
		If Not Me.IsPostBack Then

		End If

		GetJVtype()

	End Sub

	Protected Sub GetJVtype()
		tssErrMsg.Text = ""

		If cboJVtype.Text = "" Then
			Exit Sub
		End If

		cboJVsource.Items.Clear()
		cboJVsource.Items.Add("")
		Select Case cboJVtype.Text
			Case "Manual JV"
				cboJVsource.Items.Clear()
				cboJVsource.Items.Add("Manual JV")
				cboJVsource.Text = "Manual JV"
				DgvJVdet.DataSource = Nothing
				DgvJVdet.DataBind()
				lblCRtot.Text = "0.00"
				lblDRtot.Text = "0.00"
				cbCentry.Visible = True
				RadioButton1.Checked = True

			Case "Month End Entry"
				If OpenProc = "Rem Off" Then

				Else
					getTransType()
				End If

				dpTransDate.Text = Format(CDate(txtDto.Text), "yyyy-MM-dd")
				cbCentry.Visible = False
				cbCentry.Checked = False
				RadioButton1.Checked = True

			Case "Year End Closing"

				cboJVsource.Items.Add("Year End Closing")
				cboJVsource.Items.Add("Other Year End Entry")
				cbCentry.Visible = False
				cbCentry.Checked = False
				RadioButton2.Checked = True

			Case "2nd Closing"
				cboJVsource.Items.Add("2nd Closing")
				cbCentry.Visible = False
				cbCentry.Checked = False
				RadioButton2.Checked = True

		End Select

	End Sub

	Protected Sub getTransType()
		cboJVsource.Items.Clear()
		dt = GetDataTable("select distinct jvsource from gltranstbl where transyear = '" & txtYear.Text & "' and " &
						  "transmon = '" & txtMon.Text & "' and glstatus = 'Open' and " &
						  "jvsource <> 'GJV Entry' and (dramt > 0 or cramt > 0)")
		If Not CBool(dt.Rows.Count) Then
			'AdmMsgBox("No More Transaction Found")
			Exit Sub
		Else
			cboJVsource.Items.Add("")
			For Each dr As DataRow In dt.Rows
				cboJVsource.Items.Add(dr.Item(0).ToString())
			Next

		End If



		Call dt.Dispose()

	End Sub

	Private Sub cboJVsource_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboJVsource.SelectedIndexChanged
		If cboJVsource.Text = "" Then
			Exit Sub
		End If

		GetJVsource()

	End Sub

	Protected Sub GetJVsource()
		Select Case cboJVsource.Text
			Case "Year End Closing"
				CheckBox1.Checked = True
				RadioButton2.Checked = True
				ProcessYearEndClosingEntry()

			Case "Other Year End Entry", "2nd Closing"
				CheckBox1.Checked = True

			Case Else
				'txtFirstDayYear.Visible = False
				'txtLastDayYear.Visible = False
				FillLVdetSysGen()

		End Select

	End Sub

	Protected Sub ProcessYearEndClosingEntry()
		Dim dayyear As DateTime = New DateTime((CDate(txtDfrom.Text).Year), 1, 1)
		Dim dayyear2 As DateTime = New DateTime((CDate(txtDfrom.Text).Year), 12, 31)

		txtFirstDayYear.Text = Format(CDate(dayyear), "yyyy-MM-dd")
		txtLastDayYear.Text = Format(CDate(dayyear2), "yyyy-MM-dd")

		dt = GetDataTable("select jvno from gljvhdrtbl where status = 'park' and " &
						  "transyear = '" & Format(CDate(dpTransDate.Text), "yyyy") & "'")
		If Not CBool(dt.Rows.Count) Then
			ProcessYearEndClosing()
		Else
			For Each dr As DataRow In dt.Rows
				tssErrMsg.Text = "JV No. " & dr.Item(0).ToString() & " Still in PARK status"
			Next
			Exit Sub
		End If

		Dim admSup As String = Format(CDate(dpTransDate.Text), "yyyyMM")
		admPre = "YEC"
		txtRefNo.Text = admPre & admSup

	End Sub

	Protected Sub ProcessYearEndClosing()
		CreateTempEndJV()

		DgvJVdet.DataSource = Nothing
		DgvJVdet.DataBind()

		lblDRtot.Text = "0.00"
		lblCRtot.Text = "0.00"
		txtRemarks.Text = ""

		'EntryVerifier()

		FillDGVjvdetYrEnd()

		txtRemarks.Text = "Representing closing entry for the year " & Format(CDate(txtDfrom.Text), "yyyy")

		Dim lvCount As Long = DgvJVdet.Rows.Count
		txtItm.Text = lvCount + 1

		dpTransDate.Text = Format(CDate(txtDto.Text), "yyyy-MM-dd")

	End Sub

	Protected Sub CreateTempEndJV()

		dt = GetDataTable("select nonexps from batbl where ba = '" & vLoggedBussArea & "'")
		If Not CBool(dt.Rows.Count) Then Call MessageBox.Show("BA Not found.", "Costing", MessageBoxButtons.OK, MessageBoxIcon.Warning) : Exit Sub
		For Each dr As DataRow In dt.Rows
			strCCNo = dr.Item(0).ToString() & ""
		Next

		dt.Dispose()

		sql = "delete from tempgljvdettbl where user = '" & lblUser.Text & "'"
		ExecuteNonQuery(sql)

		sql = "insert into tempgljvdettbl(acctno,ccno,dramt,user) select acctno,ccno,sum(ifnull(balamt,0)),'" & lblUser.Text & "' " &
			  "from glmaintranstbl where ccno <> '" & strCCNo & "' and transyear = '" & txtYear.Text & "' and " &
			  "grpacct = '1400000' group by acctno,ccno"
		ExecuteNonQuery(sql)

		Dim baldata As Double
		dt = GetDataTable("select ifnull(sum(dramt),0) as bal from tempgljvdettbl where user = '" & lblUser.Text & "'")
		If Not CBool(dt.Rows.Count) Then Exit Sub

		For Each dr As DataRow In dt.Rows
			baldata = CDbl(dr.Item(0).ToString())

		Next

		Call dt.Dispose()


		sql = "delete from tempglmaintbl where user = '" & lblUser.Text & "'"
		ExecuteNonQuery(sql)

		If baldata > 0 Then
			sql = "insert into tempglmaintbl(acctno,ccno,dramt,pk,user)values('1301200','" & strCCNo & "'," & CDbl(baldata) & ",'40','" & lblUser.Text & "')"
			ExecuteNonQuery(sql)

			sql = "insert into tempglmaintbl(acctno,ccno,dramt,pk,user) select acctno,ccno,dramt *-1,'40','" & lblUser.Text & "' " &
				  "from tempgljvdettbl where user = '" & lblUser.Text & "' and dramt < 0"
			ExecuteNonQuery(sql)

			sql = "insert into tempglmaintbl(acctno,ccno,cramt,pk,user) select acctno,ccno,dramt,'50','" & lblUser.Text & "' " &
				  "from tempgljvdettbl where user = '" & lblUser.Text & "' and dramt > 0"
			ExecuteNonQuery(sql)

		Else
			sql = "insert into tempglmaintbl(acctno,ccno,dramt,pk,user) select acctno,ccno,dramt * -1,'40','" & lblUser.Text & "' " &
				  "from tempgljvdettbl where user = '" & lblUser.Text & "' and dramt < 0"
			ExecuteNonQuery(sql)

			sql = "insert into tempglmaintbl(acctno,ccno,cramt,pk,user) select acctno,ccno,dramt,'50','" & lblUser.Text & "' " &
				  "from tempgljvdettbl where user = '" & lblUser.Text & "' and dramt > 0"
			ExecuteNonQuery(sql)

			sql = "insert into tempglmaintbl(acctno,ccno,cramt,pk,user)values('1301200','" & strCCNo & "'," & CDbl(baldata) * -1 & ",'50','" & lblUser.Text & "')"
			ExecuteNonQuery(sql)

		End If

		sql = "update tempglmaintbl set balamt = ifnull(dramt,0)-ifnull(cramt,0) where user = '" & lblUser.Text & "'"
		ExecuteNonQuery(sql)

		sql = "delete from tempglmaintbl where user = '" & lblUser.Text & "' and balamt = 0"
		ExecuteNonQuery(sql)

		sql = "update tempglmaintbl a,(select itmno FROM tempglmaintbl where user ='" & lblUser.Text & "' limit 1) as b " &
			  "set a.begamt=a.itmno-b.itmno + 1 where user = '" & lblUser.Text & "'"
		ExecuteNonQuery(sql)

	End Sub

	Protected Sub FillLVdetSysGen()

		sql = "update gltranstbl set drcr = 'DR' where jvno is null and dramt > 0 and drcr is null"
		ExecuteNonQuery(sql)

		sql = "update gltranstbl set drcr = 'CR' where jvno is null and cramt > 0 and drcr is null"
		ExecuteNonQuery(sql)

		tssErrMsg.Text = ""
		Select Case UCase(cboJVsource.Text)
			Case "VOUCHER PAYABLES"
				dt = GetDataTable("select * from exphdrtbl where status = 'parked' and tc = '42' and transdate between " &
								  "'" & Format(CDate(txtDfrom.Text), "yyyy-MM-dd") & "' and '" & Format(CDate(txtDto.Text), "yyyy-MM-dd") & "'")
				If Not CBool(dt.Rows.Count) Then

				Else
					tssErrMsg.Text = "Can't process your request due to some VP are still in PARK status"
					Exit Sub

				End If

			Case "WEEKLY EXPENSE"
				dt = GetDataTable("select * from exphdrtbl where status = 'parked' and tc = '41' and transdate between " &
								  "'" & Format(CDate(txtDfrom.Text), "yyyy-MM-dd") & "' and '" & Format(CDate(txtDto.Text), "yyyy-MM-dd") & "'")
				If Not CBool(dt.Rows.Count) Then

				Else
					tssErrMsg.Text = "Can't process your request due to some WER are still in PARK status"
					Exit Sub

				End If

			Case "CUSTOMER DMCM"
				dt = GetDataTable("select * from custdmcmhdrtbl where status = 'parked' and tc = '90' and transdate between " &
								 "'" & Format(CDate(txtDfrom.Text), "yyyy-MM-dd") & "' and '" & Format(CDate(txtDto.Text), "yyyy-MM-dd") & "'")
				If Not CBool(dt.Rows.Count) Then

				Else
					tssErrMsg.Text = "Can't process your request due to some Customer Credit Memo are still in PARK status"
					Exit Sub

				End If

		End Select

		'lvDet.Items.Clear()
		lblDRtot.Text = ""
		lblCRtot.Text = ""
		txtRemarks.Text = ""

		EntryVerifier()

		Dim itm As Integer
		itm = 0

		FillTempDataEntry()
		FillDGVjvdet()

		lblLineItm.Text = "On Process"

		If OpenProc = "Rem Off" Then

		Else
			txtRemarks.Text = "Representing " & cboJVsource.Text & " Register for the period " & Format(CDate(txtDfrom.Text), "MMMM, dd, yyyy") _
							  & " - " & Format(CDate(txtDto.Text), "MMMM, dd, yyyy")
		End If

		'Dim lvCount As Long = lvDet.Items.Count
		'txtItm.Text = lvCount + 1

		Dim admSup As String = Format(CDate(dpTransDate.Text), "yyyyMM")

		Select Case UCase(cboJVsource.Text)
			Case "VOUCHER PAYABLES"
				admPre = "VP"
			Case "WEEKLY EXPENSE"
				admPre = "WER"
			Case "PCF VOUCHER"
				admPre = "PCF"
			Case "CHECK VOUCHER"
				admPre = "CV"
			Case "CUSTOMER DMCM"
				admPre = "CDMCM"
			Case "DEPOSITS"
				admPre = "DEP"
			Case "MMRR"
				admPre = "MMRR"
			Case "COLLECTIONS"
				admPre = "COL"
			Case "SALES"
				admPre = "SD"
			Case "COSTING"
				admPre = "COS"
			Case "COSTING PC1"
				admPre = "COSPC1"
			Case "COSTING PC2"
				admPre = "COSPC2"
			Case "PAYROLL"
				admPre = "PAY"
			Case "DEPRECIATION"
				admPre = "DEPN"
		End Select

		txtRefNo.Text = admPre & admSup

	End Sub

	Protected Sub FillDGVjvdetYrEnd()
		dt = GetDataTable("select sum(ifnull(dramt,0)),sum(ifnull(cramt,0)),sum(ifnull(balamt,0)) " &
						  "from tempglmaintbl where user = '" & lblUser.Text & "' group by user")
		If Not CBool(dt.Rows.Count) Then
			DgvJVdet.DataSource = Nothing
			DgvJVdet.DataBind()
			Exit Sub

		Else

			For Each dr As DataRow In dt.Rows
				lblDRtot.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0.00 ; (#,##0.00)")
				lblCRtot.Text = Format(CDbl(dr.Item(1).ToString()), "#,##0.00 ; (#,##0.00)")
				lblBalAmt.Text = Format(CDbl(dr.Item(2).ToString()), "#,##0.00 ; (#,##0.00)")

			Next

		End If

		dt.Dispose()

		'fill dgv
		Dim adapter As New MySqlDataAdapter
		Dim ds As New DataSet()
		'Dim i As Integer = 0
		sqldata = Nothing

		sqldata = "select a.begamt as itmno,concat(a.ccno,space(1),c.ccdesc) as ccno,concat(a.acctno,space(1),b.acctdesc) as acctno," &
				  "ifnull(a.dramt,0) as dramt,ifnull(a.cramt,0) as cramt,a.pk,ifnull(a.subacct,'20000') as subacct " &
				  "from tempglmaintbl a left join acctcharttbl b on a.acctno=b.acctno left join cctrnotbl c on a.ccno=c.ccno " &
				  "left join custmasttbl d on a.subacct = d.custno left join venmasttbl e on a.subacct=e.venno " &
				  "where a.user = '" & lblUser.Text & "' order by itmno"

		conn.Open()
		Dim command As New MySqlCommand(sqldata, conn)
		adapter.SelectCommand = command
		adapter.Fill(ds)
		adapter.Dispose()
		command.Dispose()
		conn.Close()
		DgvJVdet.DataSource = ds.Tables(0)
		DgvJVdet.DataBind()

		txtItm.Text = Format(CDbl(DgvJVdet.Rows.Count + 1), "#,##0")
	End Sub

	Protected Sub FillDGVjvdet()
		dt = GetDataTable("select sum(ifnull(dramt,0)),sum(ifnull(cramt,0)),sum(ifnull(balamt,0)) " &
						  "from tempgltranstbl where user = '" & lblUser.Text & "' group by user")
		If Not CBool(dt.Rows.Count) Then
			DgvJVdet.DataSource = Nothing
			DgvJVdet.DataBind()
			Exit Sub

		Else

			For Each dr As DataRow In dt.Rows
				lblDRtot.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0.00 ; (#,##0.00)")
				lblCRtot.Text = Format(CDbl(dr.Item(1).ToString()), "#,##0.00 ; (#,##0.00)")
				lblBalAmt.Text = Format(CDbl(dr.Item(2).ToString()), "#,##0.00 ; (#,##0.00)")

			Next

		End If

		dt.Dispose()

		'fill dgv
		Dim adapter As New MySqlDataAdapter
		Dim ds As New DataSet()
		'Dim i As Integer = 0
		sqldata = Nothing

		sqldata = "select a.transyear as itmno,concat(a.ccno,space(1),c.ccdesc) as ccno,concat(a.acctno,space(1),b.acctdesc) as acctno," &
				  "ifnull(a.dramt,0) as dramt,ifnull(a.cramt,0) as cramt,a.pk,ifnull(a.subacct,'20000') as subacct " &
				  "from tempgltranstbl a left join acctcharttbl b on a.acctno=b.acctno left join cctrnotbl c on a.ccno=c.ccno " &
				  "left join custmasttbl d on a.subacct = d.custno left join venmasttbl e on a.subacct=e.venno " &
				  "where a.user = '" & lblUser.Text & "' order by itmno"

		conn.Open()
		Dim command As New MySqlCommand(sqldata, conn)
		adapter.SelectCommand = command
		adapter.Fill(ds)
		adapter.Dispose()
		command.Dispose()
		conn.Close()
		DgvJVdet.DataSource = ds.Tables(0)
		DgvJVdet.DataBind()

		txtItm.Text = Format(CDbl(DgvJVdet.Rows.Count + 1), "#,##0")

	End Sub

	Protected Sub FillTempDataEntry()
		'gltranstbl
		dt = GetDataTable("select * from information_schema.tables where table_schema =  '" & vDbase & "' and " &
						  "table_name = 'tempgltranstbl'")
		If Not CBool(dt.Rows.Count) Then
			sql = "CREATE TABLE tempgltranstbl LIKE gltranstbl"
			ExecuteNonQuery(sql)
		Else
			sql = "delete from tempgltranstbl where user = '" & lblUser.Text & "'"
			ExecuteNonQuery(sql)

		End If

		dt.Dispose()

		Select Case UCase(cboJVsource.Text)
			Case "COSTING PC1", "COSTING PC2", "COSTING"
				sql = "insert into tempgltranstbl(ccno,acctno,dramt,cramt,balamt,pk,subacct,cv,pc,user)" &
					  "select a.ccno,a.acctno,sum(ifnull(a.dramt,0)),sum(ifnull(a.cramt,0))," &
					  "sum(ifnull(a.balamt,0)),a.pk,ifnull(a.subacct,'20000'),ifnull(b.cv,''),ifnull(a.pc,'1')," &
					  "'" & lblUser.Text & "' from gltranstbl a left join acctcharttbl b on a.acctno=b.acctno " &
					  "where a.jvsource = '" & cboJVsource.Text & "' and a.transdate between '" & Format(CDate(txtDfrom.Text), "yyyy-MM-dd") & "' " &
					  "and '" & Format(CDate(txtDto.Text), "yyyy-MM-dd") & "' and a.vericheck = 'Verified' and " &
					  "a.glstatus = 'open' and a.balamt <> 0 group by a.id order by sum(ifnull(a.balamt,0)) desc"
				ExecuteNonQuery(sql)

			Case Else
				sql = "insert into tempgltranstbl(ccno,acctno,dramt,cramt,balamt,pk,subacct,cv,pc,prioryradjt,user)" &
					   "select a.ccno,a.acctno,sum(ifnull(a.dramt,0)),sum(ifnull(a.cramt,0))," &
					   "sum(ifnull(a.balamt,0)),a.pk,ifnull(a.subacct,'20000'),ifnull(b.cv,''),ifnull(a.pc,'1')," &
					   "a.prioryradjt,'" & lblUser.Text & "' from gltranstbl a left join acctcharttbl b on a.acctno=b.acctno " &
					   "where a.jvsource = '" & cboJVsource.Text & "' and a.transdate between '" & Format(CDate(txtDfrom.Text), "yyyy-MM-dd") & "' " &
					   "and '" & Format(CDate(txtDto.Text), "yyyy-MM-dd") & "' and a.vericheck = 'Verified' and " &
					   "a.glstatus = 'open' and a.balamt <> 0 group by a.drcr,a.subacct,a.acctno,a.ccno,a.pc,a.prioryradjt " &
					   "order by sum(ifnull(a.balamt,0)) desc"
				ExecuteNonQuery(sql)

		End Select

		sql = "update tempgltranstbl a,(select id FROM tempgltranstbl where user ='" & lblUser.Text & "' limit 1) as b " &
			  "set a.transyear=a.id-b.id + 1 where user = '" & lblUser.Text & "'"
		ExecuteNonQuery(sql)

	End Sub

	Protected Sub EntryVerifier()
		vTC = ""
		sql = "update gltranstbl set balamt = ifnull(dramt,0) - ifnull(cramt,0) where balamt is null and jvsource = '" & cboJVsource.Text & "'"
		ExecuteNonQuery(sql)

		Select Case cboJVtype.Text
			Case "Manual JV"

			Case Else
				Select Case UCase(cboJVsource.Text)
					Case "SALES"
						veriSales()
						veriSalesReproc()
						UpdateGLtransProcNew()

					Case "VOUCHER PAYABLES" 'tc42
						vTC = "42"
						veriAP()

					Case "WEEKLY EXPENSE" 'tc41
						vTC = "41"
						veriAP()

					Case "PCF VOUCHER" 'tc40
						vTC = "40"
						veriAP()

					Case "RF VOUCHER" 'tc44
						vTC = "44"
						veriAP()

					Case "GAS PO" 'tc48
						vTC = "48"
						veriAP()

					Case "PAYROLL"
						veriPay()

					Case "CHECK VOUCHER"
						veriCV()

					Case "DEPOSITS"
						veriDeposits()

					Case "COLLECTIONS"
						veriColl()

					Case "CUSTOMER DMCM"
						veriDMCM()

					Case "MMRR"
						veriMMRR()

					Case "CA LIQUIDATION"
						vTC = "46"
						veriAP()

					Case "COSTING", "COSTING PC1", "COSTING PC2"
						veriCos()

					Case "DEPRECIATION"
						veriDepn()

					Case Else
						AdmMsgBox(cboJVsource.Text & " Not JV found")

				End Select

		End Select

	End Sub

	Protected Sub veriDepn()
		sql = "update gltranstbl set vericheck = 'Verified' where jvsource = '" & cboJVsource.Text & "' and transdate " &
			  "between '" & Format(CDate(txtDfrom.Text), "yyyy-MM-dd") & "' and '" & Format(CDate(txtDto.Text), "yyyy-MM-dd") & "'"
		ExecuteNonQuery(sql)
	End Sub

	Protected Sub UpdateGLtransProcNew()

		dt = GetDataTable("select nonexps from batbl where ba = '" & vLoggedBussArea & "'")
		If Not CBool(dt.Rows.Count) Then
			'Call MessageBox.Show("BA Not found.", "Costing", MessageBoxButtons.OK, MessageBoxIcon.Warning)
			Exit Sub
		Else
			For Each dr As DataRow In dt.Rows
				strCCNo = dr.Item(0).ToString() & ""
			Next
		End If

		dt.Dispose()

		dt = GetDataTable("select * from saleshdrtbl where transdate between '" & Format(CDate(txtDfrom.Text), "yyyy-MM-dd") & "' " &
						  "and '" & Format(CDate(txtDto.Text), "yyyy-MM-dd") & "' and status <> 'void' and invstat is null")
		If Not CBool(dt.Rows.Count) Then Exit Sub
		'ar own
		strARacctNo = "1110210"
		sql = "insert into gltranstbl(acctno,dramt,cramt,ccno,subacct,user,jvsource,glstatus,dateposted," &
			  "transdate,docno,tc,pc,pk,posttype,branch,bato,branchto,vericheck,cv) select '" & strARacctNo & "',a.netamt," & CDbl(0) & "," &
			  "'" & strCCNo & "',a.custno,a.user,'Sales','open',a.postdate,a.transdate,a.invno,a.tc,a.pc,'09','POST',a.branch," &
			  "'" & vLoggedBussArea & "','" & vLoggedBranch & "','Verified','update' from saleshdrtbl a left join custmasttbl b on a.custno=b.custno " &
			  "where a.transdate between '" & Format(CDate(txtDfrom.Text), "yyyy-MM-dd") & "' " &
			  "and '" & Format(CDate(txtDto.Text), "yyyy-MM-dd") & "' and a.status <> 'void' and a.invstat is null " &
			  "and b.bato = '" & vLoggedBussArea & "' group by a.invno"
		ExecuteNonQuery(sql) ',transyear,transmon

		'ar others
		strARacctNo = "1110220"
		sql = "insert into gltranstbl(acctno,dramt,cramt,ccno,subacct,user,jvsource,glstatus,dateposted," &
			  "transdate,docno,tc,pc,pk,posttype,branch,bato,branchto,vericheck,cv) select '" & strARacctNo & "',a.netamt," & CDbl(0) & "," &
			  "'" & strCCNo & "',a.custno,a.user,'Sales','open',a.postdate,a.transdate,a.invno,a.tc,a.pc,'09','POST',a.branch," &
			  "'" & vLoggedBussArea & "','" & vLoggedBranch & "','Verified','update' from saleshdrtbl a left join custmasttbl b on a.custno=b.custno " &
			  "where a.transdate between '" & Format(CDate(txtDfrom.Text), "yyyy-MM-dd") & "' " &
			  "and '" & Format(CDate(txtDto.Text), "yyyy-MM-dd") & "' and a.status <> 'void' and a.invstat is null " &
			  "and b.bato <> '" & vLoggedBussArea & "' group by a.invno"
		ExecuteNonQuery(sql) ',transyear,transmon

		'fh tax
		sql = "insert into gltranstbl(acctno,,cramt,ccno,user,jvsource,glstatus,dateposted," &
			  "transdate,docno,tc,pc,pk,posttype,branch,bato,branchto,vericheck,cv) select '1230130'," & CDbl(0) & ",fhamt,'" & strCCNo & "'," &
			  "user,'Sales','open',postdate,transdate,invno,tc,pc,'50','POST',branch,'" & vLoggedBussArea & "','" & vLoggedBranch & "'," &
			  "'Verified','update' from saleshdrtbl where transdate between '" & Format(CDate(txtDfrom.Text), "yyyy-MM-dd") & "' " &
			  "and '" & Format(CDate(txtDto.Text), "yyyy-MM-dd") & "' and status <> 'void' and invstat is null group by invno"
		ExecuteNonQuery(sql)

		'disc
		strDiscAcctNo = "1495000"
		sql = "insert into gltranstbl(acctno,,cramt,ccno,user,jvsource,glstatus,dateposted," &
			  "transdate,docno,tc,pc,pk,posttype,branch,bato,branchto,vericheck,cv) select '" & strDiscAcctNo & "',a.discamt," & CDbl(0) & "," &
			  "b.ccno,a.user,'Sales','open',a.postdate,a.transdate,a.invno,a.tc,a.pc,'40','POST',a.branch," &
			  "'" & vLoggedBussArea & "','" & vLoggedBranch & "','Verified','update' from saleshdrtbl a left join pctrtbl b on a.pc=b.pc " &
			  "where a.transdate between '" & Format(CDate(txtDfrom.Text), "yyyy-MM-dd") & "' and '" & Format(CDate(txtDto.Text), "yyyy-MM-dd") & "' " &
			  "and a.status <> 'void' and a.invstat is null group by a.invno"
		ExecuteNonQuery(sql)

		'sales
		strSalesAcctNo = "1401000"
		sql = "insert into gltranstbl(acctno,dramt,cramt,ccno,user,jvsource,glstatus,dateposted," &
			  "transdate,docno,tc,pc,pk,posttype,branch,bato,branchto,vericheck,cv) select '" & strSalesAcctNo & "'," & CDbl(0) & ",a.grossamt," &
			  "b.ccno,a.user,'Sales','open',a.postdate,a.transdate,a.invno,a.tc,a.pc,'40','POST',a.branch," &
			  "'" & vLoggedBussArea & "','" & vLoggedBranch & "','Verified','update' from saleshdrtbl a left join pctrtbl b on a.pc=b.pc " &
			  "left join custmasttbl c on a.custno=c.custno where a.transdate between '" & Format(CDate(txtDfrom.Text), "yyyy-MM-dd") & "' " &
			  "and '" & Format(CDate(txtDto.Text), "yyyy-MM-dd") & "' and a.status <> 'void' and a.invstat is null and group by a.invno"
		ExecuteNonQuery(sql)

		'strSalesAcctNo = "1110220"

		'sql = "insert into gltranstbl(acctno,dramt,cramt,ccno,user,jvsource,glstatus,dateposted," & _
		'      "transdate,docno,tc,pc,pk,posttype,branch,bato,branchto,vericheck,cv) select '" & strSalesAcctNo & "'," & CDbl(0) & ",a.grossamt," & _
		'      "b.ccno,a.user,'Sales','open',a.postdate,a.transdate,a.invno,a.tc,a.pc,'40','POST',a.branch," & _
		'      "'" & vLoggedBussArea & "','" & vLoggedBranch & "','Verified','update' from saleshdrtbl a left join pctrtbl b on a.pc=b.pc " & _
		'      "left join custmasttbl c on a.custno=c.custno where a.transdate between '" & Format(CDate(txtDfrom.Text), "yyyy-MM-dd") & "' " & _
		'      "and '" & Format(CDate(txtDto.Text), "yyyy-MM-dd") & "' and a.status <> 'void' and a.invstat is null and " & _
		'      "c.bato <> '" & vLoggedBussArea & "' group by a.invno"
		'ExecuteNonQuery(sql)

		'sql = "update gltranstbl set balamt = ifnull(dramt,0)-ifnull(cramt,0) where transdate between '" & Format(CDate(txtDfrom.Text), "yyyy-MM-dd") & "' " & _
		'      "and '" & Format(CDate(txtDto.Text), "yyyy-MM-dd") & "' "
		'ExecuteNonQuery(sql)

	End Sub

	Protected Sub veriCos()
		sql = "update gltranstbl set vericheck = 'Verified' where jvsource = '" & cboJVsource.Text & "' and transdate " &
			  "between '" & Format(CDate(txtDfrom.Text), "yyyy-MM-dd") & "' and '" & Format(CDate(txtDto.Text), "yyyy-MM-dd") & "'"
		ExecuteNonQuery(sql)

	End Sub

	Protected Sub veriPay()
		sql = "update gltranstbl,(select concat(payidno,ccno) as payid from payearntbl where transdate between '" & Format(CDate(txtDfrom.Text), "yyyy-MM-dd") & "' " &
			  "and '" & Format(CDate(txtDto.Text), "yyyy-MM-dd") & "') as pay set gltranstbl.vericheck = 'Verified' " &
			  "where gltranstbl.docno = pay.payid and gltranstbl.tc = '45' and gltranstbl.jvsource = '" & cboJVsource.Text & "'"
		ExecuteNonQuery(sql)

	End Sub

	Protected Sub veriMMRR()
		sql = "update gltranstbl,(select mmrrno,tc from invhdrtbl where transdate between '" & Format(CDate(txtDfrom.Text), "yyyy-MM-dd") & "' " &
			  "and '" & Format(CDate(txtDto.Text), "yyyy-MM-dd") & "' and status <> 'void' and tc = '10') as mmrr set gltranstbl.vericheck = 'Verified' " &
			  "where gltranstbl.docno = mmrr.mmrrno and gltranstbl.tc = '10' and gltranstbl.jvsource = '" & cboJVsource.Text & "'"
		ExecuteNonQuery(sql)

	End Sub

	Protected Sub veriColl()
		sql = "update gltranstbl,(select orno,tc from colhdrtbl where transdate between '" & Format(CDate(txtDfrom.Text), "yyyy-MM-dd") & "' " &
			  "and '" & Format(CDate(txtDto.Text), "yyyy-MM-dd") & "' and status <> 'void') as col set gltranstbl.vericheck = 'Verified' " &
			  "where gltranstbl.docno = col.orno and gltranstbl.tc = '60' and gltranstbl.jvsource = '" & cboJVsource.Text & "'"
		ExecuteNonQuery(sql)

	End Sub

	Protected Sub veriDeposits()
		sql = "update gltranstbl,(select depno from bankdephdrtbl where transdate between '" & Format(CDate(txtDfrom.Text), "yyyy-MM-dd") & "' " &
			  "and '" & Format(CDate(txtDto.Text), "yyyy-MM-dd") & "' and status <> 'void') as dep set gltranstbl.vericheck = 'Verified' " &
			  "where gltranstbl.docno = dep.depno and gltranstbl.tc = '64' and gltranstbl.jvsource = '" & cboJVsource.Text & "'"
		ExecuteNonQuery(sql)

	End Sub

	Protected Sub veriDMCM()
		sql = "update gltranstbl,(select dmcmno,tc from custdmcmhdrtbl where transdate between '" & Format(CDate(txtDfrom.Text), "yyyy-MM-dd") & "' " &
			  "and '" & Format(CDate(txtDto.Text), "yyyy-MM-dd") & "' and status = 'posted' and tc = '90') as DMCM set gltranstbl.vericheck = 'Verified' " &
			  "where gltranstbl.docno = DMCM.dmcmno and gltranstbl.tc = DMCM.tc and gltranstbl.tc = '90' and gltranstbl.jvsource = '" & cboJVsource.Text & "'"
		ExecuteNonQuery(sql)

		sql = "update gltranstbl,(select dmcmno,tc from custdmcmhdrtbl where transdate between '" & Format(CDate(txtDfrom.Text), "yyyy-MM-dd") & "' " &
			  "and '" & Format(CDate(txtDto.Text), "yyyy-MM-dd") & "' and status = 'posted' and tc = '92') as DMCM set gltranstbl.vericheck = 'Verified' " &
			  "where gltranstbl.docno = DMCM.dmcmno and gltranstbl.tc = DMCM.tc and gltranstbl.tc = '92' and gltranstbl.jvsource = '" & cboJVsource.Text & "'"
		ExecuteNonQuery(sql)

	End Sub

	Protected Sub veriCV()
		sql = "update gltranstbl,(select docno,tc from cvhdrtbl where transdate between '" & Format(CDate(txtDfrom.Text), "yyyy-MM-dd") & "' " &
			  "and '" & Format(CDate(txtDto.Text), "yyyy-MM-dd") & "' and status <> 'void' and tc = '43') as VP set gltranstbl.vericheck = 'Verified' " &
			  "where gltranstbl.docno = VP.docno and gltranstbl.tc = '43' and gltranstbl.jvsource = '" & cboJVsource.Text & "'"
		ExecuteNonQuery(sql)

	End Sub

	Protected Sub veriAP()
		sql = "update gltranstbl,(select a.docno,a.tc from expdettbl a left join exphdrtbl b on a.docno=b.docno where b.transdate " &
			  "between '" & Format(CDate(txtDfrom.Text), "yyyy-MM-dd") & "' and '" & Format(CDate(txtDto.Text), "yyyy-MM-dd") & "' " &
			  "and b.status = 'posted' and b.tc = '" & vTC & "') as vps set gltranstbl.vericheck = 'Verified' where gltranstbl.docno = vps.docno and " &
			  "gltranstbl.tc = vps.tc and gltranstbl.tc = '" & vTC & "' and gltranstbl.jvsource = '" & cboJVsource.Text & "'"
		ExecuteNonQuery(sql)

	End Sub

	Protected Sub veriSalesReproc()
		sql = "update gltranstbl,(select invno,tc from saleshdrtbl where transdate between '" & Format(CDate(txtDfrom.Text), "yyyy-MM-dd") & "' " &
			  "and '" & Format(CDate(txtDto.Text), "yyyy-MM-dd") & "' and status <> 'void') as sales set gltranstbl.vericheck = 'Verified' " &
			  "where gltranstbl.docno = sales.invno and gltranstbl.jvsource = 'Sales'" 'and gltranstbl.tc = sales.tc 
		ExecuteNonQuery(sql)

		sql = "update gltranstbl,(select invno,tc,netamt from saleshdrtbl where transdate between '" & Format(CDate(txtDfrom.Text), "yyyy-MM-dd") & "' " &
			  "and '" & Format(CDate(txtDto.Text), "yyyy-MM-dd") & "' and status <> 'void') as sales set gltranstbl.recamt = ifnull(sales.netamt,0) " &
			  "where gltranstbl.docno = sales.invno and gltranstbl.jvsource = 'Sales' and gltranstbl.acctno = '1110210'" 'and gltranstbl.tc = sales.tc 
		ExecuteNonQuery(sql)

		sql = "update gltranstbl,(select invno,tc,netamt from saleshdrtbl where transdate between '" & Format(CDate(txtDfrom.Text), "yyyy-MM-dd") & "' " &
			  "and '" & Format(CDate(txtDto.Text), "yyyy-MM-dd") & "' and status <> 'void') as sales set gltranstbl.recamt = ifnull(sales.netamt,0) " &
			  "where gltranstbl.docno = sales.invno and gltranstbl.jvsource = 'Sales' and gltranstbl.acctno = '1110220'" 'and gltranstbl.tc = sales.tc 
		ExecuteNonQuery(sql)

		'jv to inv
		sql = "update saleshdrtbl,(select docno,tc,vericheck from gltranstbl where jvsource = 'Sales' and transdate between " &
			  "'" & Format(CDate(txtDfrom.Text), "yyyy-MM-dd") & "' and '" & Format(CDate(txtDto.Text), "yyyy-MM-dd") & "' and " &
			  "vericheck = 'Verified') as verigl set saleshdrtbl.invstat = verigl.vericheck where saleshdrtbl.invno = verigl.docno " &
			  "and saleshdrtbl.status <> 'void'" 'and saleshdrtbl.tc = verigl.tc 
		ExecuteNonQuery(sql)

		'mark if other BA
		sql = "update saleshdrtbl a left join custmasttbl b on a.custno=b.custno set a.ba=b.bato where a.transdate between " &
			  "'" & Format(CDate(txtDfrom.Text), "yyyy-MM-dd") & "' and '" & Format(CDate(txtDto.Text), "yyyy-MM-dd") & "'"
		ExecuteNonQuery(sql)

	End Sub

	Protected Sub veriSales()
		'inv to JV
		sql = "update gltranstbl,(select invno,tc from saleshdrtbl where transdate between '" & Format(CDate(txtDfrom.Text), "yyyy-MM-dd") & "' " &
			  "and '" & Format(CDate(txtDto.Text), "yyyy-MM-dd") & "' and status <> 'void') as sales set gltranstbl.vericheck = 'Verified' " &
			  "where gltranstbl.docno = sales.invno and gltranstbl.jvsource = 'Sales'" 'and gltranstbl.tc = sales.tc
		ExecuteNonQuery(sql)

		sql = "update gltranstbl,(select invno,tc,netamt from saleshdrtbl where transdate between '" & Format(CDate(txtDfrom.Text), "yyyy-MM-dd") & "' " &
			  "and '" & Format(CDate(txtDto.Text), "yyyy-MM-dd") & "' and status <> 'void') as sales set gltranstbl.recamt = ifnull(sales.netamt,0) " &
			  "where gltranstbl.docno = sales.invno and gltranstbl.jvsource = 'Sales' and gltranstbl.acctno = '1110210'" 'and gltranstbl.tc = sales.tc  
		ExecuteNonQuery(sql)

		sql = "update gltranstbl,(select invno,tc,netamt from saleshdrtbl where transdate between '" & Format(CDate(txtDfrom.Text), "yyyy-MM-dd") & "' " &
			  "and '" & Format(CDate(txtDto.Text), "yyyy-MM-dd") & "' and status <> 'void') as sales set gltranstbl.recamt = ifnull(sales.netamt,0) " &
			  "where gltranstbl.docno = sales.invno and gltranstbl.jvsource = 'Sales' and gltranstbl.acctno = '1110220'" 'and gltranstbl.tc = sales.tc
		ExecuteNonQuery(sql)

		'jv to inv
		sql = "update saleshdrtbl,(select docno,tc,vericheck from gltranstbl where jvsource = 'Sales' and transdate between " &
			  "'" & Format(CDate(txtDfrom.Text), "yyyy-MM-dd") & "' and '" & Format(CDate(txtDto.Text), "yyyy-MM-dd") & "' and " &
			  "vericheck = 'Verified') as verigl set saleshdrtbl.invstat = verigl.vericheck where saleshdrtbl.invno = verigl.docno " &
			  "and saleshdrtbl.status <> 'void'" 'and saleshdrtbl.tc = verigl.tc 
		ExecuteNonQuery(sql)

		'mark if other BA
		sql = "update saleshdrtbl a left join custmasttbl b on a.custno=b.custno set a.ba=b.bato where a.transdate between " &
			  "'" & Format(CDate(txtDfrom.Text), "yyyy-MM-dd") & "' and '" & Format(CDate(txtDto.Text), "yyyy-MM-dd") & "'"
		ExecuteNonQuery(sql)

	End Sub

	Private Sub DgvJVdet_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DgvJVdet.SelectedIndexChanged
		txtItm.Text = DgvJVdet.SelectedRow.Cells(1).Text
		LoadLineItemEdit()

	End Sub

	Protected Sub LoadLineItemEdit()
		cboCCtr.Text = DgvJVdet.SelectedRow.Cells(2).Text

		'If Not Me.IsPostBack Then

		'End If

		PopGLDesc()
		cboGLdesc.Text = DgvJVdet.SelectedRow.Cells(3).Text
		txtDRamt.Text = DgvJVdet.SelectedRow.Cells(4).Text
		txtCRamt.Text = DgvJVdet.SelectedRow.Cells(5).Text
		lblPK.Text = DgvJVdet.SelectedRow.Cells(6).Text
		txtSubAcct.Text = DgvJVdet.SelectedRow.Cells(7).Text


	End Sub

	Protected Sub GetPKdr()

		dt = GetDataTable("select subacct,upperacct from acctcharttbl where acctno = '" & cboGLdesc.Text.Substring(0, 7) & "'")
		If Not CBool(dt.Rows.Count) Then
			'Call MessageBox.Show("Chart of Acct Not found.", "VP Entry", MessageBoxButtons.OK, MessageBoxIcon.Warning)
			Exit Sub
		Else
			For Each dr As DataRow In dt.Rows
				Dim strSub As String = dr.Item(0).ToString()
				Dim strUpp As String = dr.Item(1).ToString()

				Dim strDR As Double = CDbl(IIf(txtDRamt.Text = "", 0, txtDRamt.Text))

				If strDR > 0 Then
					Select Case strSub
						Case "Y"
							Select Case strUpp
								Case 1110190 To 1110300
									lblPK.Text = "09"

								Case 1131000 To 1141000
									lblPK.Text = "40"

								Case 1210100 To 1210500
									lblPK.Text = "29"

								Case Else
									lblPK.Text = "40"

							End Select

							txtSubAcct.ReadOnly = False

						Case Else
							lblPK.Text = "40"
							txtSubAcct.ReadOnly = True

					End Select

				Else
					Exit Sub

				End If

			Next
		End If

		Call dt.Dispose()

	End Sub

	Protected Sub GetPKcr()

		dt = GetDataTable("select subacct,upperacct from acctcharttbl where acctno = '" & cboGLdesc.Text.Substring(0, 7) & "'")
		If Not CBool(dt.Rows.Count) Then
			'Call MessageBox.Show("Chart of Acct Not found.", "VP Entry", MessageBoxButtons.OK, MessageBoxIcon.Warning)
			Exit Sub
		Else
			For Each dr As DataRow In dt.Rows
				Dim strSub As String = dr.Item(0).ToString()
				Dim strUpp As String = dr.Item(1).ToString()

				Dim strCR As Double = CDbl(IIf(txtCRamt.Text = "", 0, txtCRamt.Text))

				If strCR > 0 Then
					Select Case strSub
						Case "Y"
							Select Case strUpp
								Case 1110190 To 1110300
									lblPK.Text = "19"

								Case 1131000 To 1141000
									lblPK.Text = "50"

								Case 1210100 To 1210500
									lblPK.Text = "39"

								Case Else
									lblPK.Text = "50"

							End Select

							txtSubAcct.ReadOnly = False

						Case Else
							lblPK.Text = "50"
							txtSubAcct.ReadOnly = True

					End Select

				Else
					Exit Sub

				End If

			Next
		End If

		Call dt.Dispose()

	End Sub

	Private Sub txtDRamt_TextChanged(sender As Object, e As EventArgs) Handles txtDRamt.TextChanged
		If cboGLdesc.Text = Nothing Then
			Exit Sub
		End If

		DRamtProc()

	End Sub

	Protected Sub DRamtProc()
		DRamt = CDbl(IIf(txtDRamt.Text = "", 0, txtDRamt.Text))
		txtDRamt.Text = Format(DRamt, "##,##0.00")

		If DRamt < 0 Then
			tssErrMsg.Text = "Negative Amount Not Accepted"
			txtDRamt.Text = "0.00"
			Exit Sub
		End If

		GetPKdr()

		Select Case lblSubAcct.Text
			Case "Y"
				If DRamt > 0 Then


				End If

		End Select

		txtCRamt.Focus()

	End Sub

	Private Sub cboGLdesc_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboGLdesc.SelectedIndexChanged
		If cboCCtr.Text = Nothing Or cboCCtr.Text = "" Then
			tssErrMsg.Text = "Select Cost Center"
			Exit Sub
		End If

		GLdescProc()

	End Sub

	Protected Sub GLdescProc()
		If cboGLdesc.Text = Nothing Or cboGLdesc.Text = "" Then
			tssErrMsg.Text = "Select GL Account"
			Exit Sub
		End If

		dt = GetDataTable("select ifnull(subacct,'N'),cv from acctcharttbl where acctno = '" & cboGLdesc.Text.Substring(0, 7) & "'")
		If Not CBool(dt.Rows.Count) Then
			'Call MessageBox.Show("Chart of Acct Not found.", "JV Entry", MessageBoxButtons.OK, MessageBoxIcon.Warning)
			Exit Sub
		Else
			For Each dr As DataRow In dt.Rows
				lblSubAcct.Text = dr.Item(0).ToString()
				strCV = dr.Item(1).ToString()

			Next

		End If

		Call dt.Dispose()

		txtDRamt.Focus()

		If lblSubAcct.Text = "N" Then
			txtSubAcct.Text = "20000"
			txtSubAcct.ReadOnly = True
			'CheckBox2.Visible = False
			'lblLabelSA.TextAlign = ContentAlignment.MiddleCenter
		Else
			txtSubAcct.ReadOnly = False
			'CheckBox2.Visible = True
			'lblLabelSA.TextAlign = ContentAlignment.MiddleRight
		End If

	End Sub

	Private Sub txtCRamt_TextChanged(sender As Object, e As EventArgs) Handles txtCRamt.TextChanged
		If cboGLdesc.Text = Nothing Then
			Exit Sub
		End If

		CRamtProc()

	End Sub

	Protected Sub CRamtProc()
		CRamt = CDbl(IIf(txtCRamt.Text = "", 0, txtCRamt.Text))
		txtCRamt.Text = Format(CRamt, "##,##0.00")

		If CRamt < 0 Then
			tssErrMsg.Text = "Negative Amount Not Accepted"
			txtCRamt.Text = "0.00"
			Exit Sub
		End If

		GetPKcr()

		Select Case lblSubAcct.Text
			Case "Y"
				txtSubAcct.Focus()

			Case Else
				btnAdd.Focus()

		End Select

	End Sub

	Protected Sub BalanceCheck()
		Dim Bals As Double
		Bals = Math.Round(CDbl(IIf(lblDRtot.Text = "", 0, lblDRtot.Text)) - CDbl(IIf(lblCRtot.Text = "", 0, lblCRtot.Text)), 2)
		lblBalAmt.Text = Format(Bals, "#,##0.00")

		If Bals = 0 Then
			vThisFormCode = "052"
			If IsAllowed(lblGrpUser.Text, vThisFormCode, 3) = True Then ' 3 = Insert 
				lbSave.Enabled = True

			Else
				lbSave.Enabled = False

			End If

		End If

	End Sub

	Private Sub btnAdd_Click(sender As Object, e As ImageClickEventArgs) Handles btnAdd.Click
		If cboJVtype.Text = Nothing Then
			AdmMsgBox("Select JV Type")
			Exit Sub

		End If

		If cboCCtr.Text = "" Then
			cboCCtr.Focus()
			AdmMsgBox("CC is Blank")
			Exit Sub

		ElseIf cboGLdesc.Text = "" Then
			cboGLdesc.Focus()
			AdmMsgBox("GL Account is Blank")
			Exit Sub

		ElseIf lblPK.Text = "" Then
			txtDRamt.Focus()
			AdmMsgBox("No PK")
			Exit Sub

		ElseIf txtItm.Text = "" Then
			txtItm.Focus()
			AdmMsgBox("Line Item No. is Blank")

		ElseIf lblSubAcct.Text = "Y" Then
			If txtSubAcct.Text = "" Then
				AdmMsgBox("Sub Account is Blank")
				Exit Sub
			End If

		End If

		If CheckBox5.Checked = True Then
			Select Case cboGLdesc.Text.Substring(0, 3)
				Case "157", "158"

				Case Else
					AdmMsgBox("Not applicable to Non-Expense Account")
					Exit Sub
			End Select

		End If

		GetPKdr()
		GetPKcr()

		AddLineItemProc()
		BalanceCheck()
		ClrLineItem()
		FillDGVjvdet()

	End Sub

	Protected Sub AddLineItemProc()
		If txtJVNo.Text = Nothing And lblLineItm.Text = "New" Then
			sql = "delete from tempgltranstbl where user = '" & lblUser.Text & "'"
			ExecuteNonQuery(sql)

		End If

		dt = GetDataTable("select * from tempgltranstbl where transyear = " & txtItm.Text & " and " &
						  "user = '" & lblUser.Text & "'")
		If Not CBool(dt.Rows.Count) Then 'strCV
			sql = "insert into tempgltranstbl(transyear,ccno,acctno,dramt,cramt,balamt,pk,subacct,cv,pc,user)values" &
				  "(" & txtItm.Text & ",'" & cboCCtr.Text.Substring(0, 5) & "','" & cboGLdesc.Text.Substring(0, 7) & "'," &
				  CDbl(IIf(txtDRamt.Text = "", 0, txtDRamt.Text)) & "," & CDbl(IIf(txtCRamt.Text = "", 0, txtCRamt.Text)) & "," &
				  CDbl(IIf(txtDRamt.Text = "", 0, txtDRamt.Text)) & " - " & CDbl(IIf(txtCRamt.Text = "", 0, txtCRamt.Text)) & "," &
				  "'" & lblPK.Text & "','" & txtSubAcct.Text & "','" & strCV & "','" & cboPC.Text & "'," &
				  "'" & lblUser.Text & "')"
			ExecuteNonQuery(sql)

		Else
			sql = "update tempgltranstbl set ccno = '" & cboCCtr.Text.Substring(0, 5) & "',acctno = '" & cboGLdesc.Text.Substring(0, 7) & "'," &
				  "dramt = " & CDbl(IIf(txtDRamt.Text = "", 0, txtDRamt.Text)) & ",cramt = " & CDbl(IIf(txtCRamt.Text = "", 0, txtCRamt.Text)) & "," &
				  "balamt = " & CDbl(IIf(txtDRamt.Text = "", 0, txtDRamt.Text)) & " - " & CDbl(IIf(txtCRamt.Text = "", 0, txtCRamt.Text)) & "," &
				  "pk = '" & lblPK.Text & "',subacct = '" & txtSubAcct.Text & "',cv = '" & strCV & "',pc = '" & cboPC.Text & "' where " &
				  "transyear = " & txtItm.Text & " and user = '" & lblUser.Text & "'"
			ExecuteNonQuery(sql)

		End If

		If CheckBox5.Checked = True Then
			sql = "update tempgltranstbl set prioryradjt = 'Y' where transyear = " & txtItm.Text & " and " &
				  "user = '" & lblUser.Text & "'"
			ExecuteNonQuery(sql)

		End If

		lblLineItm.Text = "On Process"

	End Sub

	Protected Sub ClrLineItem()
		txtItm.Text = DgvJVdet.Rows.Count + 1

		cboCCtr.Text = Nothing
		cboGLdesc.Text = Nothing
		txtDRamt.Text = ""
		txtCRamt.Text = ""
		lblPK.Text = ""
		txtSubAcct.Text = ""
		strCV = Nothing


	End Sub

	Private Sub lbJVtype_Click(sender As Object, e As EventArgs) Handles lbJVtype.Click
		GetJVtype()

	End Sub

	Private Sub lbCCenter_Click(sender As Object, e As EventArgs) Handles lbCCenter.Click
		PopGLDesc()

	End Sub

	Private Sub lbGLdesc_Click(sender As Object, e As EventArgs) Handles lbGLdesc.Click
		GLdescProc()
	End Sub

	Private Sub dpTransDate_TextChanged(sender As Object, e As EventArgs) Handles dpTransDate.TextChanged
		If dpTransDate.Text = Nothing Then
			Exit Sub
		End If

		checkTransDate()

	End Sub

	Private Sub checkTransDate()
		If dpTransDate.Text = Nothing Then
			Exit Sub
		End If

		If CDate(dpTransDate.Text) < vTransMon Then
			If CheckBox1.Checked = True Then
				'dpTransDate.Text = Format(CDate(vTransYear), "yyyy-MM-dd")
				cboJVtype.Items.Clear()
				cboJVtype.Items.Add("")
				cboJVtype.Items.Add("Year End Closing")
				cboJVtype.Items.Add("2nd Closing")
				txtYear.Text = Format(CDate(vYearEnd), "yyyy")
				txtMon.Text = Format(CDate(vYearEnd), "MM")

				Dim firstDay As Date = FirstDayOfMonth(vYearEnd)
				Dim lastDay As Date = LastDayOfMonth(vYearEnd)

				txtDfrom.Text = Format(firstDay, "yyyy-MM-dd")
				txtDto.Text = Format(lastDay, "yyyy-MM-dd")

			Else
				If CheckBox4.Checked = True Then
					Dim firstDay As Date = FirstDayOfMonth(dpTransDate.Text)
					Dim lastDay As Date = LastDayOfMonth(dpTransDate.Text)

					txtDfrom.Text = Format(firstDay, "yyyy-MM-dd")
					txtDto.Text = Format(lastDay, "yyyy-MM-dd")

					txtYear.Text = Format(CDate(dpTransDate.Text), "yyyy")
					txtMon.Text = Format(CDate(dpTransDate.Text), "MM")

				Else
					AdmMsgBox(Format(CDate(dpTransDate.Text), "MMM yyyy") & " Is Already CLOSED")
					dpTransDate.Text = Format(CDate(Now()), "yyyy-MM-dd")
					txtYear.Text = Format(CDate(dpTransDate.Text), "yyyy")
					txtMon.Text = Format(CDate(dpTransDate.Text), "MM")
					Exit Sub
				End If

			End If

		End If 'transfer

		Select Case cboJVtype.Text
			Case "Manual JV"
				txtYear.Text = Format(CDate(dpTransDate.Text), "yyyy")
				txtMon.Text = Format(CDate(dpTransDate.Text), "MM")

				Dim firstDay As Date = FirstDayOfMonth(dpTransDate.Text)
				Dim lastDay As Date = LastDayOfMonth(dpTransDate.Text)

				txtDfrom.Text = Format(firstDay, "yyyy-MM-dd")
				txtDto.Text = Format(lastDay, "yyyy-MM-dd")

			Case "Month End Entry"


		End Select
	End Sub

	Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
		If CheckBox1.Checked = True Then
			dt = GetDataTable("select dto from gltransmonstatus where yearstat = 'OPEN' " &
							  "and monstat = 'CLOSE' and yearendcltype <> 'Final' and transmon = '12' order by transyear limit 1")
			If Not CBool(dt.Rows.Count) Then
				vTransYear = LastDayOfMonth(vTransMon)
			Else
				For Each dr As DataRow In dt.Rows
					vYearEnd = Format(CDate(dr.Item(0).ToString()), "yyyy-MM-dd")
					dpTransDate.Text = Format(CDate(dr.Item(0).ToString()), "yyyy-MM-dd")

				Next
			End If

			Call dt.Dispose()

			CheckBox1.Text = "Year End"
			RadioButton2.Checked = True
			checkTransDate()

			Select Case txtMon.Text
				Case "12"
					CheckBox1.Text = "Year End"

				Case Else
					CheckBox1.Text = "Reg Entry"

			End Select

		Else
			CheckBox1.Text = "Reg Entry"

		End If

	End Sub

	Protected Sub getManualJVNo()
		Select Case cboJVtype.Text
			Case "Manual JV"
				strJVpre = "GJV"

			Case "Month End Entry"
				strJVpre = "RJV"

			Case Else
				strJVpre = "YEC"

		End Select

		dt = GetDataTable("Select transid from gljvhdrtbl order by transid")
		If Not CBool(dt.Rows.Count) Then
			txtJVNo.Text = strJVpre & txtYear.Text & txtMon.Text & "00001"

		Else
			For Each dr As DataRow In dt.Rows
				txtJVNo.Text = strJVpre & txtYear.Text & txtMon.Text & Format(dr.Item(0).ToString() + 1, "#00000")

			Next

		End If

		Call dt.Dispose()

	End Sub

	Protected Sub SaveHdrProc()

		Select Case cboJVtype.Text
			Case "Manual JV"
				admJVtype = "Manual JV"
			Case Else
				admJVtype = cboJVsource.Text
		End Select

		dt = GetDataTable("Select status from gljvhdrtbl where jvno = '" & txtJVNo.Text & "'")
		If Not CBool(dt.Rows.Count) Then
			Call SaveHdr()

		Else
			Call UpdateHdr()

		End If

		SaveDetProc()

		Select Case UCase(cboJVsource.Text)
			Case "COSTING"
				sql = "update coshdrtbl set jvno = '" & txtJVNo.Text & "',glstat = 'Processed' where " &
					  "transdate = '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "'"
				ExecuteNonQuery(sql)

			Case "COSTING PC1"
				sql = "update coshdrtbl set jvno = '" & txtJVNo.Text & "',glstat = 'Processed' where " &
					  "transdate = '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and pc = '1'"
				ExecuteNonQuery(sql)

			Case "COSTING PC2"
				sql = "update coshdrtbl set jvno = '" & txtJVNo.Text & "',glstat = 'Processed' where " &
					  "transdate = '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and pc = '2'"
				ExecuteNonQuery(sql)

				'Case "COSTING", "COSTING PC1", "COSTING PC2"
				'	sql = "update coshdrtbl set jvno = '" & txtJVNo.Text & "' where transdate = '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "'"
				'	ExecuteNonQuery(sql)

		End Select

	End Sub

	Protected Sub SaveHdr()
		If RadioButton1.Checked = True Then
			strEntryType = RadioButton1.Text

		Else
			strEntryType = RadioButton2.Text

		End If

		sql = "insert into gljvhdrtbl(jvno,transdate,dramt,cramt,remarks,transyear,transmon,user,status,postdate,branch,sourcedoc," &
			  "jvno2,dfrom,dto,jvtype,entrytype)values('" & txtJVNo.Text & "','" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "'," &
			  CDbl(IIf(lblDRtot.Text = "", 0, lblDRtot.Text)) & "," & CDbl(IIf(lblCRtot.Text = "", 0, lblCRtot.Text)) & "," &
			  "'" & txtRemarks.Text & "','" & txtYear.Text & "','" & txtMon.Text & "','" & lblUser.Text & "'," &
			  "'" & "Park" & "','" & Format(CDate(Now), "yyyy-MM-dd HH:mm:ss") & "','" & vLoggedBranch & "','" & admJVtype & "'," &
			  "'" & txtRefNo.Text & "','" & Format(CDate(txtDfrom.Text), "yyyy-MM-dd") & "'," &
			  "'" & Format(CDate(txtDto.Text), "yyyy-MM-dd") & "','" & cboJVtype.Text & "','" & strEntryType & "')"
		ExecuteNonQuery(sql)

	End Sub

	Protected Sub UpdateHdr()

		If RadioButton1.Checked = True Then
			strEntryType = RadioButton1.Text

		Else
			strEntryType = RadioButton2.Text

		End If

		sql = "update gljvhdrtbl set transdate = '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "'," &
			  "dramt = " & CDbl(IIf(lblDRtot.Text = "", 0, lblDRtot.Text)) & ",cramt = " & CDbl(IIf(lblCRtot.Text = "", 0, lblCRtot.Text)) & "," &
			  "remarks = '" & txtRemarks.Text & "',transyear = '" & txtYear.Text & "',transmon = '" & txtMon.Text & "'," &
			  "user = '" & lblUser.Text & "',status = '" & "Park" & "',postdate ='" & Format(CDate(Now), "yyyy-MM-dd HH:mm:ss") & "'," &
			  "branch = '" & vLoggedBranch & "',sourcedoc = '" & admJVtype & "',jvno2 = '" & txtRefNo.Text & "'," &
			  "dfrom = '" & Format(CDate(txtDfrom.Text), "yyyy-MM-dd") & "',dto = '" & Format(CDate(txtDto.Text), "yyyy-MM-dd") & "'," &
			  "jvtype = '" & cboJVtype.Text & "',entrytype = '" & strEntryType & "' where jvno = '" & txtJVNo.Text & "'"
		ExecuteNonQuery(sql)

	End Sub

	Protected Sub SaveDetProc()

		dt = GetDataTable("select * from gljvdettbl where jvno = '" & txtJVNo.Text & "'")
		If Not CBool(dt.Rows.Count) Then
			Call SaveDet()
			tssErrMsg.Text = "Doc. No." & txtJVNo.Text & Space(1) & lbSave.Text

		Else
			sql = "delete from gljvdettbl where jvno = '" & txtJVNo.Text & "'"
			ExecuteNonQuery(sql)

			Call SaveDet()
			tssErrMsg.Text = "Doc. No." & txtJVNo.Text & " Update and " & lbSave.Text

		End If

		dt.Dispose()

	End Sub

	Protected Sub SaveDet()

		If RadioButton1.Checked = True Then
			strEntryType = RadioButton1.Text

		Else
			strEntryType = RadioButton2.Text

		End If

		Select Case cboJVtype.Text
			Case "Year End Closing", "2nd Closing" '
				sql = "insert into gljvdettbl(jvno,itmno,ccno,acctno,dramt,cramt,pk,subacct,transyear,transmon,user,sourcedoc," &
					  "yearend,status,cv,entrytype,pc) select '" & txtJVNo.Text & "',begamt,ccno,acctno,dramt," &
					  "cramt,pk,subacct,'" & txtYear.Text & "','" & txtMon.Text & "','" & lblUser.Text & "','" & cboJVsource.Text & "'," &
					  "'" & CheckBox1.Text & "','open', cv,'" & strEntryType & "',pc from tempglmaintbl where " &
					  "user = '" & lblUser.Text & "'"
				ExecuteNonQuery(sql)
			Case Else
				sql = "insert into gljvdettbl(jvno,itmno,ccno,acctno,dramt,cramt,pk,subacct,transyear,transmon,user,sourcedoc," &
					  "yearend,status,cv,entrytype,pc,prioryradjt) select '" & txtJVNo.Text & "',transyear,ccno,acctno,dramt," &
					  "cramt,pk,subacct,'" & txtYear.Text & "','" & txtMon.Text & "','" & lblUser.Text & "','" & cboJVsource.Text & "'," &
					  "'" & CheckBox1.Text & "','open', cv,'" & strEntryType & "',pc,prioryradjt from tempgltranstbl where " &
					  "user = '" & lblUser.Text & "'"
				ExecuteNonQuery(sql)
		End Select

		If cbCentry.Checked = True Then
			sql = "update gljvdettbl set cos = 'Y',pc = '" & cboPC.Text & "' where jvno = '" & txtJVNo.Text & "'"
			ExecuteNonQuery(sql)

		Else
			sql = "update gljvdettbl set cos = null where jvno = '" & txtJVNo.Text & "'"
			ExecuteNonQuery(sql)

		End If

		If cboJVtype.Text = "Month End Entry" Then
			sql = "update gltranstbl set jvno = '" & txtJVNo.Text & "',glstatus = 'processed' " &
				  "where jvsource = '" & cboJVsource.Text & "' and transyear = '" & txtYear.Text & "' and " &
				  "transmon = '" & txtMon.Text & "'"
			ExecuteNonQuery(sql)

			sql = "update gltranstbl a,(select jvno,acctno,prioryradjt from gljvdettbl where jvno = '" & txtJVNo.Text & "' " &
				  "and prioryradjt = 'Y') as b set a.prioryradjt = b.prioryradjt where a.acctno=b.acctno and " &
				  "a.jvno=b.jvno"
			ExecuteNonQuery(sql)

			Select Case cboJVsource.Text
				Case "MMRR"
					sql = "update gljvdettbl a left join acctcharttbl b on a.acctno=b.acctno set a.cos = b.invcos " &
						  "where a.jvno = '" & txtJVNo.Text & "' and b.invcos = 'Y'"
					ExecuteNonQuery(sql)

			End Select

			sql = "update gltranstbl set drcr = 'DR' where jvno = '" & txtJVNo.Text & "' and dramt > 0"
			ExecuteNonQuery(sql)

			sql = "update gltranstbl set drcr = 'CR' where jvno = '" & txtJVNo.Text & "' and cramt > 0"
			ExecuteNonQuery(sql)

		Else
			SaveGLtransDetProc()

		End If

		lblLastDocNo.Text = "JV No.: " & txtJVNo.Text

	End Sub

	Protected Sub SaveGLtransDetProc()

		dt = GetDataTable("select * from gltranstbl where docno = '" & txtJVNo.Text & "' and TC = '" & lblTC.Text & "' " &
						  "and branch = '" & vLoggedBranch & "'")
		If Not CBool(dt.Rows.Count) Then
			Call SaveGLtransDet()

		Else
			Select Case cboJVtype.Text
				Case "Manual JV", "Year End Closing", "2nd Closing"
					'create recov table

					sql = "delete from gltranstbl where docno = '" & txtJVNo.Text & "' and TC = '" & lblTC.Text & "'"
					ExecuteNonQuery(sql)

					Call SaveGLtransDet()

			End Select

		End If

		dt.Dispose()

		Select Case UCase(cboJVsource.Text)
			Case "COSTING"
				sql = "update coshdrtbl set jvno = '" & txtJVNo.Text & "',glstat = 'Processed' where " &
					  "transdate = '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "'"
				ExecuteNonQuery(sql)

			Case "COSTING PC1"
				sql = "update coshdrtbl set jvno = '" & txtJVNo.Text & "',glstat = 'Processed' where " &
					  "transdate = '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and pc = '1'"
				ExecuteNonQuery(sql)

			Case "COSTING PC2"
				sql = "update coshdrtbl set jvno = '" & txtJVNo.Text & "',glstat = 'Processed' where " &
					  "transdate = '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and pc = '2'"
				ExecuteNonQuery(sql)

		End Select

		If cbCentry.Checked = True Then
			sql = "update gltranstbl set cos = 'Y',pc = '" & cboPC.Text & "' where jvno = '" & txtJVNo.Text & "'"
			ExecuteNonQuery(sql)

		End If

		sql = "update gltranstbl set drcr = 'DR' where jvno = '" & txtJVNo.Text & "' and dramt > 0"
		ExecuteNonQuery(sql)

		sql = "update gltranstbl set drcr = 'CR' where jvno = '" & txtJVNo.Text & "' and cramt > 0"
		ExecuteNonQuery(sql)


	End Sub

	Protected Sub SaveGLtransDet()
		sql = "insert into gltranstbl(jvno,acctno,dramt,cramt,ccno,transyear,transmon,subacct,user,jvsource,glstatus,dateposted,transdate," &
			  "docno,tc,pk,posttype,branch,cv,prioryradjt) select '" & txtJVNo.Text & "',acctno,dramt,cramt,ccno,transyear,transmon,subacct," &
			  "'" & lblUser.Text & "','GJV Entry','processed','" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "'," &
			  "'" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "','" & txtJVNo.Text & "','" & lblTC.Text & "',pk,'JV det'," &
			  "'" & vLoggedBranch & "',cv,prioryradjt from gljvdettbl where jvno = '" & txtJVNo.Text & "' and sourcedoc = 'Manual JV'"
		ExecuteNonQuery(sql)

		'Select Case UCase(cboJVsource.Text)
		'	Case "COSTING", "COSTING PC1", "COSTING PC2"
		'		sql = "update coshdrtbl set jvno = '" & txtJVNo.Text & "',glstat = 'Processed' where " &
		'			  "transdate = '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "'"
		'		ExecuteNonQuery(sql)

		'End Select

		'If cbCentry.Checked = True Then
		'	sql = "update gltranstbl set cos = 'Y',pc = '" & cboPC.Text & "' where jvno = '" & txtJVNo.Text & "'"
		'	ExecuteNonQuery(sql)

		'End If

		'sql = "update gltranstbl set drcr = 'DR' where jvno is null and dramt > 0 and drcr is null"
		'ExecuteNonQuery(sql)

		'sql = "update gltranstbl set drcr = 'CR' where jvno is null and cramt > 0 and drcr is null"
		'ExecuteNonQuery(sql)

	End Sub

	Private Sub txtSubAcct_TextChanged(sender As Object, e As EventArgs) Handles txtSubAcct.TextChanged

		Select Case txtSubAcct.Text
			Case 0 To 19999
				dt = GetDataTable("select bussname from custmasttbl where custno = '" & txtSubAcct.Text & "'")
				admCodeType = "Customer"
			Case 20000 To 29999
				dt = GetDataTable("select venname from venmasttbl where venno = '" & txtSubAcct.Text & "'")
				admCodeType = "Vendor"
		End Select

		If Not CBool(dt.Rows.Count) Then
			tssErrMsg.Text = ("Not found.")
			Exit Sub
		Else
			For Each dr As DataRow In dt.Rows
				tssOthMsg.Text = admCodeType & ": " & dr.Item(0).ToString()

			Next

		End If

		Call dt.Dispose()

		btnAdd.Focus()

	End Sub

	Private Sub txtJVNo_TextChanged(sender As Object, e As EventArgs) Handles txtJVNo.TextChanged
		If txtJVNo.Text = Nothing Then
			Exit Sub
			'ElseIf cboJVtype.Text = Nothing Then
			'    tssErrMsg.Text = "Select JV Type"
			'    Exit Sub
		End If

		LoadJV()
		TabContainer1.ActiveTabIndex = 0
	End Sub

	Protected Sub LoadJV()

		dt = GetDataTable("select status,jvtype from gljvhdrtbl where jvno = '" & txtJVNo.Text & "'")
		If Not CBool(dt.Rows.Count) Then
			tssErrMsg.Text = "JV not found."
			Exit Sub

		Else
			For Each dr As DataRow In dt.Rows
				lblStatus.Text = UCase(dr.Item(0).ToString() & "")
				cboJVtype.Items.Add(dr.Item(1).ToString())
				cboJVtype.Text = dr.Item(1).ToString()
			Next

		End If

		dt.Dispose()

		tssDocStat.Text = "EDIT"
		tssDocNo.Text = txtJVNo.Text

		Select Case UCase(lblStatus.Text)
			Case "PARK"
				CheckGroupRights()
				btnReproc.Visible = True

			Case "POST"
				lbSave.Enabled = False
				btnAdd.Enabled = False
				btnDel.Enabled = False
				btnReproc.Visible = False
				lbDelete.Enabled = False

			Case "VOID"
				lbSave.Enabled = False
				btnAdd.Enabled = False
				btnDel.Enabled = False
				btnReproc.Visible = False
				lbDelete.Enabled = False

		End Select

		LoadJVHeader()
		LoadJVdet()

		lblLineItm.Text = "On Process"

	End Sub

	Private Sub LoadJVHeader()
		dt = GetDataTable("select transdate,dramt,cramt,remarks,transyear,transmon,sourcedoc," &
						  "jvno2,dfrom,dto,entrytype from gljvhdrtbl where jvno = '" & txtJVNo.Text & "'")
		If Not CBool(dt.Rows.Count) Then
			tssErrMsg.Text = ("JV not found.")
			Exit Sub
		End If

		For Each dr As DataRow In dt.Rows
			dpTransDate.Text = Format(CDate(dr.Item(0).ToString()), "yyyy-MM-dd")
			lblDRtot.Text = Format(CDbl(dr.Item(1).ToString()), "#,##0.00")
			lblCRtot.Text = Format(CDbl(dr.Item(2).ToString()), "#,##0.00")
			txtRemarks.Text = dr.Item(3).ToString()
			txtYear.Text = dr.Item(4).ToString()
			txtMon.Text = dr.Item(5).ToString()
			OpenProc = "Rem Off"

			Select Case cboJVtype.Text
				Case "Month End Entry", "Year End Closing", "2nd Closing"
					cboJVsource.Text = dr.Item(6).ToString()

				Case Else
					cboJVsource.Text = "Manual JV"

			End Select

			cboJVsource.Items.Add(dr.Item(6).ToString())
			cboJVsource.Text = dr.Item(6).ToString()
			txtRefNo.Text = dr.Item(7).ToString()
			txtDfrom.Text = Format(dr.Item(8), "yyyy-MM-dd".ToString())
			txtDto.Text = Format(dr.Item(9), "yyyy-MM-dd".ToString())

			Select Case dr.Item(10).ToString() & ""
				Case "Reversal"
					RadioButton2.Checked = True

				Case Else
					RadioButton1.Checked = True

			End Select
		Next

		dt.Dispose()

		If cboJVsource.Text = "Manual JV" Then
			cboJVtype.Text = "Manual JV"
		Else
			If CheckBox1.Checked = True Then
				cboJVtype.Text = "Year End Closing"
			Else
				cboJVtype.Text = "Month End Entry"
			End If


		End If

		cbCentry.Visible = True

	End Sub

	Private Sub LoadJVdet()
		DgvJVdet.DataSource = Nothing
		DgvJVdet.DataBind()

		'create tempdata here
		ReloadJVdetExist()
		FillDGVjvdet()
		ClrLineItem()

	End Sub

	Protected Sub ReloadJVdetExist()
		dt = GetDataTable("select * from information_schema.tables where table_schema =  '" & vDbase & "' and " &
						  "table_name = 'tempgltranstbl'")
		If Not CBool(dt.Rows.Count) Then
			sql = "CREATE TABLE tempgltranstbl LIKE gltranstbl"
			ExecuteNonQuery(sql)
		Else
			sql = "delete from tempgltranstbl where user = '" & lblUser.Text & "'"
			ExecuteNonQuery(sql)

		End If

		dt.Dispose()

		sql = "insert into tempgltranstbl(transyear,ccno,acctno,dramt,cramt,balamt,pk,subacct,cv,pc,user)" &
			  "select itmno,ccno,acctno,ifnull(dramt,0),ifnull(cramt,0),ifnull(dramt,0)-ifnull(cramt,0),pk," &
			  "ifnull(subacct,'20000'),ifnull(cv,''),ifnull(pc,'1'),'" & lblUser.Text & "' " &
			  "from gljvdettbl where jvno = '" & txtJVNo.Text & "' order by itmno"
		ExecuteNonQuery(sql)

	End Sub

	Protected Sub OnConfirm4(sender As Object, e As EventArgs)
		Dim confirmValue As String = Request.Form("confirm_value")
		If confirmValue = "Yes" Then
			Response.Redirect("FinancialAccounting.aspx")

		Else
			tssErrMsg.Text = "Action Aborted"
		End If

	End Sub

	Protected Sub OnConfirm3(sender As Object, e As EventArgs)
		Dim confirmValue As String = Request.Form("confirm_value")
		If confirmValue = "Yes" Then
			LineItemDelProc()

		Else
			tssErrMsg.Text = "Action Aborted"
		End If
	End Sub

	Protected Sub OnConfirm2(sender As Object, e As EventArgs)
		Dim confirmValue As String = Request.Form("confirm_value")
		If confirmValue = "Yes" Then
			clrHdrFields()
			ClrLineItem()
			lbDelete.Enabled = False
			tssErrMsg.Text = ""
			OpenGLstatus()
			vThisFormCode = "052"
			Call CheckGroupRights()

			cboJVtype.Items.Clear()
			cboJVtype.Items.Add("")
			cboJVtype.Items.Add("Manual JV")

			dt = GetDataTable("select * from gltranstbl where transdate between '" & Format(CDate(txtDfrom.Text), "yyyy-MM-dd") & "' " &
							  "and '" & Format(CDate(txtDto.Text), "yyyy-MM-dd") & "' and jvsource <> 'Manual JV' and jvno is null " &
							  "and (dramt > 0 or cramt > 0) limit 1")
			If Not CBool(dt.Rows.Count) Then
				cboJVtype.Items.Remove("Month End Entry")
			Else
				cboJVtype.Items.Add("Month End Entry")
			End If


			'OpenGLstatus()
			GetCCtrAll()

			If txtMon.Text = "12" Then
				cboJVtype.Items.Add("Year End Closing")
				cboJVtype.Items.Add("2nd Closing")
			End If

			If lblUser.Text = "Rothman" Then
				CheckBox4.Visible = True

			Else
				CheckBox4.Visible = False
			End If

		Else
			tssErrMsg.Text = "Action Aborted"
		End If

	End Sub

	Protected Sub clrHdrFields()
		txtJVNo.Text = ""
		txtRefNo.Text = ""
		txtRemarks.Text = ""
		DgvJVdet.DataSource = Nothing
		DgvJVdet.DataBind()
		lblDRtot.Text = "0.00"
		lblCRtot.Text = "0.00"
		lblSubAcct.Text = ""
		dpTransDate.Text = Format(CDate(Now()), "yyyy-MM-dd")
		lblStatus.Text = "New"
		cboJVsource.Text = Nothing
		OpenProc = Nothing
		tssDocStat.Text = "New"
		getTransType()
		lblLineItm.Text = "New"

	End Sub

	Protected Sub OnConfirm(sender As Object, e As EventArgs)
		Dim confirmValue As String = Request.Form("confirm_value")
		If confirmValue = "Yes" Then
			VoidProc()
		Else
			tssErrMsg.Text = "Action Aborted"
		End If
	End Sub

	Protected Sub VoidProc()
		If lblStatus.Text = "Post" Then
			tssErrMsg.Text = "Void Not Allowed"
			Exit Sub

		Else
			sql = "delete from gljvhdrtbl where jvno = '" & txtJVNo.Text & "' and status = '" & "Park" & "'"
			ExecuteNonQuery(sql)

			sql = "delete from gljvdettbl where jvno = '" & txtJVNo.Text & "'"
			ExecuteNonQuery(sql)

			Select Case UCase(cboJVsource.Text)
				Case "COSTING", "COSTING PC1", "COSTING PC2"
					sql = "update coshdrtbl set glstat = null,jvno = null where jvno = '" & txtJVNo.Text & "'"
					ExecuteNonQuery(sql)

					sql = "delete from gltranstbl where jvno = '" & txtJVNo.Text & "'"
					ExecuteNonQuery(sql)

				Case "Manual JV"
					sql = "delete from gltranstbl where jvno = '" & txtJVNo.Text & "'"
					ExecuteNonQuery(sql)

			End Select

			sql = "update gltranstbl set glstatus = 'Open',jvno = null where jvsource = 
				  '" & cboJVsource.Text & "' and transyear = '" & txtYear.Text & "' and " &
				  "transmon = '" & txtMon.Text & "'"
			ExecuteNonQuery(sql)

			tssErrMsg.Text = "JV from " & txtJVNo.Text & " Successfully VOID"

		End If

	End Sub

	Private Sub txtRecJVno_TextChanged(sender As Object, e As EventArgs) Handles txtRecJVno.TextChanged
		If txtRecJVno.Text = Nothing Then
			Exit Sub
		End If

		'load rec JV
		dt = GetDataTable("select * from information_schema.tables where table_schema =  '" & vDbase & "' and " &
						  "table_name = 'tempgltranstbl'")
		If Not CBool(dt.Rows.Count) Then
			sql = "CREATE TABLE tempgltranstbl LIKE gltranstbl"
			ExecuteNonQuery(sql)
		Else
			sql = "delete from tempgltranstbl where user = '" & lblUser.Text & "'"
			ExecuteNonQuery(sql)

		End If

		dt.Dispose()

		DgvJVdet.DataSource = Nothing
		DgvJVdet.DataBind()

		sql = "insert into tempgltranstbl(transyear,ccno,acctno,dramt,cramt,balamt,pk,subacct,cv,pc,user) " &
			  "select a.itmno,a.ccno,a.acctno,ifnull(a.dramt,0),ifnull(a.cramt,0),ifnull(a.dramt,0)-ifnull(a.cramt,0)," &
			  "a.pk,a.subacct,a.cv,ifnull(a.pc,'1'),'" & lblUser.Text & "' from gljvdettbl a left join cctrnotbl b " &
			  "on a.ccno=b.ccno left join acctcharttbl c on a.acctno=c.acctno where jvno = '" & txtRecJVno.Text & "' " &
			  "order by a.itmno"
		ExecuteNonQuery(sql)

		FillDGVjvdet()

	End Sub

	Private Sub btnReset_Click(sender As Object, e As ImageClickEventArgs) Handles btnReset.Click
		ClrLineItem()
	End Sub

	Private Sub LineItemDelProc()

		sql = "delete from tempgltranstbl where transyear = " & DgvJVdet.SelectedRow.Cells(1).Text & " " &
			  "and user = '" & lblUser.Text & "'"
		ExecuteNonQuery(sql)

		FillDGVjvdet()

	End Sub

	Protected Sub lbNew_Click(sender As Object, e As EventArgs)

	End Sub

	Private Sub dpDate1_TextChanged(sender As Object, e As EventArgs) Handles dpDate1.TextChanged, dpDate2.TextChanged

		GetJVstatusData()

	End Sub

	Protected Sub GetJVstatusData()
		If dpDate1.Text = Nothing Then
			Exit Sub
		ElseIf dpDate2.Text = Nothing Then
			Exit Sub
		End If

		If Format(CDate(dpDate1.Text), "yyyy-MM-dd") > Format(CDate(dpDate2.Text), "yyyy-MM-dd") Then
			Exit Sub
		End If

		cboStat.Items.Clear()
		dt = GetDataTable("select status from gljvhdrtbl where transdate between '" & Format(CDate(dpDate1.Text), "yyyy-MM-dd") & "' " &
						  "and '" & Format(CDate(dpDate2.Text), "yyyy-MM-dd") & "' group by status")
		If Not CBool(dt.Rows.Count) Then
			tssErrMsg.Text = "No JV Status found."
			Exit Sub

		Else
			For Each dr As DataRow In dt.Rows
				cboStat.Items.Add(dr.Item(0).ToString())

			Next

		End If

		dt.Dispose()

	End Sub

	Private Sub LbStat_Click(sender As Object, e As EventArgs) Handles LbStat.Click
		If cboStat.Text = Nothing Then
			Exit Sub

		End If

		JVsumListProc()

	End Sub

	Protected Sub JVsumListProc()
		dt = GetDataTable("select * from gljvhdrtbl where transdate between '" & Format(CDate(dpDate1.Text), "yyyy-MM-dd") & "' " &
						  "and '" & Format(CDate(dpDate2.Text), "yyyy-MM-dd") & "' and status = '" & cboStat.Text & "'")
		If Not CBool(dt.Rows.Count) Then
			tssErrMsg.Text = "No JV found."
			DgvJVsumList.DataSource = Nothing
			DgvJVsumList.DataBind()
			Exit Sub

		End If

		dt.Dispose()

		Dim adapter As New MySqlDataAdapter
		Dim ds As New DataSet()
		'Dim i As Integer = 0
		sqldata = Nothing

		sqldata = "select jvno,transdate,jvtype,sourcedoc,status,remarks,user,jvno2 from gljvhdrtbl where transdate " &
				  "between '" & Format(CDate(dpDate1.Text), "yyyy-MM-dd") & "' and " &
				  "'" & Format(CDate(dpDate2.Text), "yyyy-MM-dd") & "' and status = '" & cboStat.Text & "' order by transdate"

		conn.Open()
		Dim command As New MySqlCommand(sqldata, conn)
		adapter.SelectCommand = command
		adapter.Fill(ds)
		adapter.Dispose()
		command.Dispose()
		conn.Close()
		DgvJVsumList.DataSource = ds.Tables(0)
		DgvJVsumList.DataBind()

	End Sub

	Protected Sub DgvJVsumList_RowDataBound(sender As Object, e As GridViewRowEventArgs)

	End Sub

	Protected Sub DgvJVsumList_RowCreated(sender As Object, e As GridViewRowEventArgs)

	End Sub

	Private Sub cboStat_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboStat.SelectedIndexChanged
		JVsumListProc()

	End Sub

	Private Sub DgvJVsumList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DgvJVsumList.SelectedIndexChanged
		txtJVNo.Text = DgvJVsumList.SelectedRow.Cells(1).Text
		LoadJV()
		TabContainer1.ActiveTabIndex = 0
		DgvJVsumList.SelectedIndex = -1

	End Sub

	Protected Sub lbSearch_Click(sender As Object, e As EventArgs)

	End Sub

	Protected Sub DgvVendor_RowDataBound(sender As Object, e As GridViewRowEventArgs)

	End Sub

	Protected Sub DgvVendor_RowCreated(sender As Object, e As GridViewRowEventArgs)

	End Sub

	Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
		If txtSearch.Text = Nothing Then
			Exit Sub
		End If

		SearchVendors()

	End Sub

	Protected Sub SearchVendors()

		dt = GetDataTable("select * from venmasttbl where venname like " &
						  "'" & "%" & txtSearch.Text & "%" & "' order by venname")
		If Not CBool(dt.Rows.Count) Then
			tssErrMsg.Text = "No JV found."
			DgvVendor.DataSource = Nothing
			DgvVendor.DataBind()
			Exit Sub

		End If

		dt.Dispose()

		Dim adapter As New MySqlDataAdapter
		Dim ds As New DataSet()
		'Dim i As Integer = 0
		sqldata = Nothing

		sqldata = "select venno,venname,address,status from venmasttbl where venname like " &
				  "'" & "%" & txtSearch.Text & "%" & "' order by venname"

		conn.Open()
		Dim command As New MySqlCommand(sqldata, conn)
		adapter.SelectCommand = command
		adapter.Fill(ds)
		adapter.Dispose()
		command.Dispose()
		conn.Close()
		DgvVendor.DataSource = ds.Tables(0)
		DgvVendor.DataBind()

	End Sub

	Private Sub DgvVendor_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DgvVendor.SelectedIndexChanged
		If lblSubAcct.Text = "Y" Then
			txtSubAcct.Text = DgvVendor.SelectedRow.Cells(1).Text

		End If
	End Sub

	Private Sub lbClose_Click(sender As Object, e As EventArgs) Handles lbClose.Click

		Response.Redirect("FinancialAccounting.aspx")

	End Sub
End Class