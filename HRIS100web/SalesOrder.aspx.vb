Imports MySql.Data.MySqlClient
Imports System
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports MySql.Data
Imports System.Windows.Forms

Public Class SalesOrder
	Inherits System.Web.UI.Page
	Dim dt As DataTable
	Dim sql As String
	Dim sqldata As String
	Dim SP As Double
	Dim Amt As Double
	Dim GrossAmt As Double
	Dim NetAmt As Double
	Dim DiscAmt As Double
	Dim DiscRate As Double
	Dim strReport As String
	Dim admDeal As String
	Dim WtQty As Double
	Dim ChkVal As String
	Dim ds As New DataSet()

	Protected Sub AdmMsgBox(ByVal sMessage As String)
		Dim msg As String
		msg = "<script language='javascript'>"
		msg += "alert('" & sMessage & "');"
		msg += "<" & "/script>"
		Response.Write(msg)
	End Sub

	Protected Sub ClrErrMsg()
		lblErrMsg.Text = "No Error"
	End Sub

	Protected Sub SaveLogs()
		Dim strForm As String = "Sales Order"
		sql = "insert into translog(trans,form,datetimelog,user,docno,tc)values" &
			  "('" & strReport & "','" & strForm & "','" & Format(CDate(Now), "yyyy-MM-dd hh:mm:ss") & "'," &
			  "'" & lblUser.Text & "','" & txtSOno.Text & "','" & lblTC.Text & "')"
		ExecuteNonQuery(sql)

	End Sub

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

		If lblUser.Text Is Nothing Then
			Response.Redirect("login.aspx")

		End If

		If Not Me.IsPostBack Then
			lblUser.Text = Session("UserID")
			lblGrpUser.Text = Session("UserGrp")

		End If

	End Sub
	Protected Sub NewLoadProc()
		tssDocNo.Text = Nothing
		vThisFormCode = "017"
		Call CheckGroupRights()
		txtSOno.ReadOnly = False

		tssDocStat.Text = "New"
		'MsgBox(vLoggedBussArea)

		strReport = "Load Form"
		Call SaveLogs()

		If Not Me.IsPostBack Then
			Call GetCustList()
			Call GetSmnList()
			Call GetPClass()
		End If

		cboPlnt.Items.Clear()
		dt = GetDataTable("select concat(plntno,space(1),plntname) from plnttbl where status = 'active' and invset = 'On'")
		If Not CBool(dt.Rows.Count) Then
			lblErrMsg.Text = "Not found."
			Exit Sub
		Else
			For Each dr As DataRow In dt.Rows
				cboPlnt.Items.Add(dr.Item(0).ToString())

			Next
		End If

		Call dt.Dispose()

		Select Case vLoggedBussArea
			Case "8100"
				txtSOno.MaxLength = 9

			Case Else
				txtSOno.MaxLength = 8

		End Select

		dpTransDate.Text = Format(CDate(Now()), "yyyy-MM-dd")

	End Sub

	Protected Sub GetPClass()
		ClrErrMsg()
		cboPClass.Items.Clear()

		Select Case vLoggedBussArea
			Case "8100"
				cboPClass.Items.Add("1 Veterinary Products")
				If cboPClass.Items.Count > 0 Then
					cboPClass.SelectedIndex = 0
				End If
			Case "8200"
				cboPClass.Items.Add("1 Trading Goods")
				If cboPClass.Items.Count > 0 Then
					cboPClass.SelectedIndex = 0
				End If

			Case "8300"
				PopPClass()

		End Select
	End Sub

	Protected Sub GetMMast()
		ClrErrMsg()
		cboMMdesc.Items.Clear()
		Select Case vLoggedBussArea
			Case "8300"
				dt = GetDataTable("select ifnull(codename,mmdesc),mmtype from mmasttbl where trade = 'Y' and pc = '" & cboPClass.Text.Substring(0, 1) & "' " &
								  "and status = 'In Used' order by ifnull(codename,mmdesc)")

			Case Else
				dt = GetDataTable("select ifnull(codename,mmdesc),mmtype from mmasttbl where trade = 'Y' " &
								  "and status = 'In Used' order by ifnull(codename,mmdesc)")
		End Select

		If Not CBool(dt.Rows.Count) Then
			lblErrMsg.Text = "Not found."
			Exit Sub

		Else
			cboMMdesc.Items.Add("")
			For Each dr As DataRow In dt.Rows
				cboMMdesc.Items.Add(dr.Item(0).ToString())

			Next

		End If

		Call dt.Dispose()

	End Sub

	Private Sub GetMMastAvail()
		ClrErrMsg()
		'get inventory
		CreateNewInvBeg()

		cboMMdesc.Items.Clear()

		dt = GetDataTable("select ifnull(b.codename,b.mmdesc) from tempinvbalsnew_" & lblUser.Text & " a left join mmasttbl b on a.codeno = b.codeno " &
						  "where b.pc = '" & cboPClass.Text.Substring(0, 1) & "' and b.status = 'In Used' group by a.codeno order by ifnull(b.codename,b.mmdesc)")
		If Not CBool(dt.Rows.Count) Then
			lblErrMsg.Text = "Not found."
			Exit Sub
		Else
			For Each dr As DataRow In dt.Rows
				cboMMdesc.Items.Add(dr.Item(0).ToString())

			Next

		End If

		Call dt.Dispose()

	End Sub

	Private Sub CreateNewInvBeg()
		sql = "delete from tempinvbals where user = '" & lblUser.Text & "'"
		ExecuteNonQuery(sql)
		'mmrr
		sql = "insert into tempinvbals(codeno,lotno,user,mmdesc) select a.codeno,a.lotno,'" & lblUser.Text & "',c.mmdesc " &
			  "from invdettbl a left join invhdrtbl b on a.mmrrno=b.mmrrno left join mmasttbl c on a.codeno=c.codeno where " &
			  "b.plntno = '" & cboPlnt.Text.Substring(0, 3) & "' and b.status <> 'void' and " &
			  "b.transdate <= '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and c.trade = 'Y' group by a.codeno,a.lotno"
		ExecuteNonQuery(sql)
		'do
		sql = "insert into tempinvbals(codeno,lotno,user,mmdesc) select a.codeno,a.lotno,'" & lblUser.Text & "',c.mmdesc " &
			  "from issdettbl a left join isshdrtbl b on a.dono=b.dono left join mmasttbl c on a.codeno=c.codeno where " &
			  "b.plntno = '" & cboPlnt.Text.Substring(0, 3) & "' and b.status <> 'void' and " &
			  "b.transdate <= '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and c.trade = 'Y' group by a.codeno,a.lotno"
		ExecuteNonQuery(sql)
		'wrr
		sql = "insert into tempinvbals(codeno,lotno,user,mmdesc) select a.codeno,a.lotno,'" & lblUser.Text & "',c.mmdesc " &
			  "from wrrdettbl a left join wrrhdrtbl b on a.wrrno=b.wrrno left join mmasttbl c on a.codeno=c.codeno where " &
			  "b.plntno = '" & cboPlnt.Text.Substring(0, 3) & "' and b.status <> 'void' and " &
			  "b.transdate <= '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and c.trade = 'Y' group by a.codeno,a.lotno"
		ExecuteNonQuery(sql)

		dt = GetDataTable("select * from information_schema.tables where table_schema =  '" & vDbase & "' and " &
						  "table_name = '" & "tempinvbalsnew_" & lblUser.Text & "'")
		If Not CBool(dt.Rows.Count) Then
			sql = "CREATE TABLE tempinvbalsnew_" & lblUser.Text & " LIKE tempinvbals"
			ExecuteNonQuery(sql)
		Else
			sql = "truncate table tempinvbalsnew_" & lblUser.Text
			ExecuteNonQuery(sql)
		End If

		sql = "insert into tempinvbalsnew_" & lblUser.Text & "(codeno,mmdesc,lotno,user) select codeno,mmdesc,lotno,user from tempinvbals " &
			  "where user = '" & lblUser.Text & "' group by codeno,lotno"
		ExecuteNonQuery(sql)

		'prodn
		sql = "update tempinvbalsnew_" & lblUser.Text & " a,(select a.codeno,sum(ifnull(a.qty,0)) as qty,sum(ifnull(a.wt,0)) as wt,a.lotno " &
			  "from invdettbl a left join invhdrtbl b on a.mmrrno=b.mmrrno where b.mov = '801' and b.plntno = '" & cboPlnt.Text.Substring(0, 3) & "' and " &
			  "b.status <> 'void' and b.transdate < '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' group by a.codeno,a.lotno) as b set " &
			  "a.prodqtybeg = b.qty,a.prodwtbeg = b.wt where a.user = '" & lblUser.Text & "' and " &
			  "a.codeno = b.codeno and a.lotno=b.lotno"
		ExecuteNonQuery(sql)

		'mmrr
		sql = "update tempinvbalsnew_" & lblUser.Text & " a,(select a.codeno,sum(ifnull(a.qty,0)) as qty,sum(ifnull(a.wt,0)) as wt,a.lotno " &
			  "from invdettbl a left join invhdrtbl b on a.mmrrno=b.mmrrno where b.tc = '10' and (b.mov <> '701' and b.mov <> '511') and " &
			  "b.plntno = '" & cboPlnt.Text.Substring(0, 3) & "' and b.status <> 'void' and b.transdate < '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
			  "group by a.codeno,a.lotno) as b set a.recqtybeg = b.qty,a.recwtbeg = b.wt where a.user = '" & lblUser.Text & "' and " &
			  "a.codeno = b.codeno and a.lotno=b.lotno"
		ExecuteNonQuery(sql)

		'mdiMain.tsPbar.Value = 15

		'return
		sql = "update tempinvbalsnew_" & lblUser.Text & " a,(select a.codeno,sum(ifnull(a.qty,0)) as qty,sum(ifnull(a.wt,0)) as wt,a.lotno from " &
			  "wrrdettbl a left join wrrhdrtbl b on a.wrrno=b.wrrno where b.plntno = '" & cboPlnt.Text.Substring(0, 3) & "' and b.status <> 'void' and " &
			  "b.transdate < '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' group by a.codeno,a.lotno) as b set " &
			  "a.wrrqtybeg = b.qty,a.wrrwtbeg = b.wt where a.user = '" & lblUser.Text & "' and " &
			  "a.codeno = b.codeno and a.lotno=b.lotno"
		ExecuteNonQuery(sql)

		'mdiMain.tsPbar.Value = 20

		'issuance
		sql = "update tempinvbalsnew_" & lblUser.Text & " a,(select a.codeno,sum(ifnull(a.qty,0)) as qty,sum(ifnull(a.wt,0)) as wt,a.lotno from " &
			  "issdettbl a left join isshdrtbl b on a.dono=b.dono where b.tc = '30' and b.mov <> '701' and b.plntno = '" & cboPlnt.Text.Substring(0, 3) & "' " &
			  "and b.status <> 'void' and b.transdate < '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' group by a.codeno,a.lotno) as b set " &
			  "a.issqtybeg = b.qty,a.isswtbeg = b.wt where a.user = '" & lblUser.Text & "' and a.codeno = b.codeno and a.lotno=b.lotno"
		ExecuteNonQuery(sql)

		'mdiMain.tsPbar.Value = 25

		'adjustment in
		sql = "update tempinvbalsnew_" & lblUser.Text & " a,(select a.codeno,sum(ifnull(a.qty,0)) as qty,sum(ifnull(a.wt,0)) as wt,a.lotno from " &
			  "invdettbl a left join invhdrtbl b on a.mmrrno=b.mmrrno where b.tc = '10' and b.mov = '701' and b.plntno = '" & cboPlnt.Text.Substring(0, 3) & "' " &
			  "and b.status <> 'void' and b.transdate < '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' group by a.codeno,a.lotno) as b set " &
			  "a.inadjtqtybeg = b.qty,a.inadjtwtbeg = b.wt where a.user = '" & lblUser.Text & "' and a.codeno = b.codeno and a.lotno=b.lotno"
		ExecuteNonQuery(sql)

		'mdiMain.tsPbar.Value = 50

		'adjustment out
		sql = "update tempinvbalsnew_" & lblUser.Text & " a,(select a.codeno,sum(ifnull(a.qty,0)) as qty,sum(ifnull(a.wt,0)) as wt,a.lotno from " &
			  "issdettbl a left join isshdrtbl b on a.dono=b.dono where b.tc = '30' and b.mov = '701' and b.plntno = '" & cboPlnt.Text.Substring(0, 3) & "' " &
			  "and b.status <> 'void' and b.transdate < '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' group by a.codeno,a.lotno) as b set " &
			  "a.outadjtqtybeg = b.qty,a.outadjtwtbeg = b.wt where a.user = '" & lblUser.Text & "' and a.codeno = b.codeno and a.lotno=b.lotno"
		ExecuteNonQuery(sql)

		'mdiMain.tsPbar.Value = 70

		'initial stock
		sql = "update tempinvbalsnew_" & lblUser.Text & " a,(select a.codeno,sum(ifnull(a.qty,0)) as qty,sum(ifnull(a.wt,0)) as wt,a.lotno from " &
			  "invdettbl a left join invhdrtbl b on a.mmrrno=b.mmrrno where b.tc = '10' and b.mov = '511' and b.plntno = '" & cboPlnt.Text.Substring(0, 3) & "' " &
			  "and b.status <> 'void' and b.transdate < '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' group by a.codeno,a.lotno) as b set " &
			  "a.initqtybeg = b.qty,a.initwtbeg = b.wt where a.user = '" & lblUser.Text & "' and a.codeno = b.codeno and a.lotno=b.lotno"
		ExecuteNonQuery(sql)

		'mdiMain.tsPbar.Value = 80

		'inv balance
		sql = "update tempinvbalsnew_" & lblUser.Text & " set qtybalbeg = ifnull(prodqtybeg,0) + ifnull(recqtybeg,0) + ifnull(wrrqtybeg,0) + ifnull(inadjtqtybeg,0) + ifnull(initqtybeg,0) - " &
			  "ifnull(issqtybeg,0) - ifnull(outadjtqtybeg,0),wtbalbeg = ifnull(prodwtbeg,0) + ifnull(recwtbeg,0) + ifnull(wrrwtbeg,0) + ifnull(inadjtwtbeg,0) " &
			  "+ ifnull(initwtbeg,0) - ifnull(isswtbeg,0) - ifnull(outadjtwtbeg,0) where user = '" & lblUser.Text & "'"
		ExecuteNonQuery(sql)


		sql = "delete from tempinvbalsnew_" & lblUser.Text & " where qtybal = 0 and wtbal = 0 and user = '" & lblUser.Text & "'"
		ExecuteNonQuery(sql)

		'mdiMain.tsPbar.Value = 100
		'mdiMain.tsPbar.Visible = False

	End Sub

	Protected Sub GetMMastCust()
		ClrErrMsg()
		Call cboMMdesc.Items.Clear()

		dt = GetDataTable("select ifnull(a.codename,a.mmdesc) from salesdettbl c left join saleshdrtbl b on c.invno=b.invno " &
						  "left join mmasttbl a on a.codeno=c.codeno where b.custno = '" & cboCustName.Text.Substring(0, 5) & "' " &
						  "group by c.codeno order by ifnull(a.codename,a.mmdesc)")
		If Not CBool(dt.Rows.Count) Then
			lblErrMsg.Text = "No Material found."
			Exit Sub
		Else
			cboMMdesc.Items.Add("")
			For Each dr As DataRow In dt.Rows
				cboMMdesc.Items.Add(dr.Item(0).ToString())

			Next
		End If

		Call dt.Dispose()

	End Sub

	Protected Sub GetSmnList()
		ClrErrMsg()
		cboSmnName.Items.Clear()
		dt = GetDataTable("select concat(smnno,space(1),fullname) from smnmtbl where status = 'active' order by fullname")
		If Not CBool(dt.Rows.Count) Then
			lblErrMsg.Text = "Not found."
			Exit Sub
		Else
			For Each dr As DataRow In dt.Rows
				cboSmnName.Items.Add(dr.Item(0).ToString())

			Next

		End If

		Call dt.Dispose()

	End Sub

	Protected Sub GetCustList()

		cboCustName.Items.Clear()
		dt = GetDataTable("select concat(custno,space(1),bussname) from custmasttbl where accttype = 'main' and status = 'Active' order by bussname")
		If Not CBool(dt.Rows.Count) Then
			lblErrMsg.Text = "Not found."
			Exit Sub

		Else
			For Each dr As DataRow In dt.Rows
				cboCustName.Items.Add(dr.Item(0).ToString())

			Next

		End If

		Call dt.Dispose()

	End Sub

	Protected Sub getCustListALL()
		ClrErrMsg()
		cboCustName.Items.Clear()
		dt = GetDataTable("select concat(codeno,space(1),bussname) from custmasttbl where custno between '00000' and '19999' and " &
						  "accttype = 'main' and status = 'Active' order by bussname")
		If Not CBool(dt.Rows.Count) Then
			lblErrMsg.Text = "No Customer/Client found."
			Exit Sub
		Else
			cboCustName.Items.Add("Load SMN")
			For Each dr As DataRow In dt.Rows
				cboCustName.Items.Add(dr.Item(0).ToString())

			Next

		End If

		Call dt.Dispose()

	End Sub

	Private Sub getCustListSmn()
		ClrErrMsg()
		cboCustName.Items.Clear()
		dt = GetDataTable("select concat(custno,space(1),bussname )from custmasttbl where smnno = '" & cboSmnName.Text.Substring(0, 3) & "' and " &
						  "custno between '00000' and '19999' and accttype = 'Main' and status = 'Active' order by bussname")
		If Not CBool(dt.Rows.Count) Then
			lblErrMsg.Text = "No Customer/Client found."
			Exit Sub
		Else
			For Each dr As DataRow In dt.Rows
				cboCustName.Items.Add(dr.Item(0).ToString())

			Next

		End If

		Call dt.Dispose()

	End Sub

	Protected Sub CheckGroupRights()
		If IsAllowed(lblGrpUser.Text, vThisFormCode, 3) = True Or IsAllowed(lblGrpUser.Text, vThisFormCode, 2) = True Then ' 3 = Insert | 2 = EDIT
			' Enable objects here
			lbSave.Enabled = True

		Else
			lbSave.Enabled = False
			' Disable objects here
		End If

		If IsAllowed(lblGrpUser.Text, vThisFormCode, 4) = True Then ' 4 = Delete
			lbDelete.Enabled = True
			' Enable Delete/VOID button here
		Else
			lbDelete.Enabled = False
			' Disable Delete/VOID button here

		End If

	End Sub


	Protected Sub lbSave_Click(sender As Object, e As EventArgs)
		ClrErrMsg()
		If txtPONo.Text = "" Then
			lblErrMsg.Text = "PO No is Blank"
			txtPONo.Focus()
			Exit Sub

		ElseIf cboCustName.Text = "" Or cboCustName.Text = Nothing Then
			lblErrMsg.Text = "Sold To No is Blank"
			Exit Sub

		ElseIf cboShipTo.Text = "" Or cboShipTo.Text = Nothing Then
			lblErrMsg.Text = "Ship To No is Blank"
			Exit Sub

		ElseIf cboSmnName.Text = "" Or cboSmnName.Text = Nothing Then
			lblErrMsg.Text = "Salesman No is Blank"
			Exit Sub

			'ElseIf lblSOStat.Text = "Credit Status" Then
			'    cmdChkCR.Focus()
			'    MsgBox("A/R not yet Checked", MsgBoxStyle.Critical)
			'    Exit Sub

		ElseIf cboPlnt.Text = "" Then
			lblErrMsg.Text = "Select Plant"
			Exit Sub

		ElseIf DgvSOdet.Rows.Count = 0 Then
			lblErrMsg.Text = "No line item/s yet"
			Exit Sub

		End If

		If tssDocStat.Text = "New" Then
			If txtSOno.Text = "" Then
				Call GetSONo()
				txtSOno.ReadOnly = True

			End If

		Else
			If tssDocNo.Text <> txtSOno.Text Then
				lblErrMsg.Text = "Doc. No. should not be CHANGED"
				txtSOno.Text = tssDocNo.Text

			End If

		End If

		Select Case vLoggedBussArea
			Case "8100"
				If Len(txtSOno.Text) <> 9 Then
					lblErrMsg.Text = "SO No. should be 9 Digits"
					Exit Sub
				End If

			Case Else
				If Len(txtSOno.Text) <> 8 Then
					lblErrMsg.Text = "SO No. should be 8 Digits"
					Exit Sub
				End If

		End Select

		'Call chkNewBals()
		'gRepTbox(txtRemarks)

		Call SaveHdrProc()
		'ToolStripMenuItem3.Enabled = True

		'mdiMain.tslblLastDoc2.Text = "SO No.: " & txtSOno.Text

		'gRepTboxUndo(txtRemarks)
		lbPrint.Enabled = True

	End Sub

	Private Sub GetSONo()
		ClrErrMsg()
		dt = GetDataTable("select ifnull(sono,0) from sohdrtbl where sono between '20000001' and '29999999' order by sono desc limit 1")
		If Not CBool(dt.Rows.Count) Then
			txtSOno.Text = "20000001"

		Else
			For Each dr As DataRow In dt.Rows
				Dim SONo As Long = CLng(dr.Item(0).ToString()) + 1
				Select Case vLoggedBussArea
					Case "8100"
						txtSOno.Text = Format(SONo, "#000000000")
					Case Else
						txtSOno.Text = Format(SONo, "#00000000")
				End Select


			Next

		End If

		Call dt.Dispose()

	End Sub

	Private Sub SaveHdrProc()
		ClrErrMsg()
		dt = GetDataTable("select delstat from sohdrtbl where sono = '" & txtSOno.Text & "'")
		If Not CBool(dt.Rows.Count) Then
			Call SaveSOHdr()

			'AdmMsgBox("SO No. " & txtSOno.Text & " saved")
			strReport = "save so"
			Call SaveLogs()

		Else
			For Each dr As DataRow In dt.Rows
				Select Case UCase(dr.Item(0).ToString())
					Case "SERVED"
						'AdmMsgBox("SO No.: " & txtSOno.Text & " Already SERVED, channges not SAVED")
						strReport = "Update SO, Not SAVED"
						Call SaveLogs()
						Exit Sub

				End Select
			Next

			UpdateSOHdr()

			'AdmMsgBox("SO No. " & txtSOno.Text & " Update SAVED")
			strReport = "Update SO"
			Call SaveLogs()

		End If

		Call dt.Dispose()

		Select Case UCase(lblSOStat.Text)
			Case "APPROVED"
				sql = "update sohdrtbl set apprvdby = 'Auto Approved' where sono = '" & txtSOno.Text & "'"
				ExecuteNonQuery(sql)

		End Select

		sql = "update sohdrtbl set sysver = 'AOS100 Web' where sono = '" & txtSOno.Text & "'"
		ExecuteNonQuery(sql)

		Call SaveDetProc()

		'mdiMain.tslblLastDoc2.Text = "SO No.:" & txtSOno.Text

	End Sub

	Private Sub SaveSOHdr()
		sql = "insert into sohdrtbl(sono,transdate,custno,shipto,smnno,pono,delstat,user,30days,60days,90days,120days," &
			  "soamt,status,totar,deldate,apprvdby,remarks,plntno,branch,91over,deposit,pdc,pdate)values" &
			  "('" & txtSOno.Text & "','" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "'," &
			  "'" & cboCustName.Text.Substring(0, 5) & "','" & cboShipTo.Text.Substring(0, 5) & "','" & cboSmnName.Text.Substring(0, 3) & "'," &
			  "'" & txtPONo.Text & "','OPEN','" & lblUser.Text & "'," & CDbl(IIf(lbl30days.Text = "", 0, lbl30days.Text)) & "," &
			  CDbl(IIf(lbl60days.Text = "", 0, lbl60days.Text)) & "," & CDbl(IIf(lbl90days.Text = "", 0, lbl90days.Text)) & "," &
			  CDbl(IIf(lbl91over.Text = "", 0, lbl91over.Text)) & "," & CDbl(IIf(lblTotNetAmt.Text = "", 0, lblTotNetAmt.Text)) & "," &
			  "'" & lblSOStat.Text & "'," & CDbl(IIf(lblAcctBal.Text = "", 0, lblAcctBal.Text)) & "," &
			  "'" & Format(CDate(dpDelDate.Text), "yyyy-MM-dd") & "',null,'" & txtRemarks.Text & "'," &
			  "'" & cboPlnt.Text.Substring(0, 3) & "','" & vLoggedBranch & "'," & CDbl(IIf(lbl121over.Text = "", 0, lbl121over.Text)) & "," &
			  CDbl(IIf(lblDepAmt.Text = "", 0, lblDepAmt.Text)) & "," & CDbl(IIf(lblDepAmt.Text = "", 0, lblDepAmt.Text)) & "," &
			  "'" & Format(CDate(Now()), "yyyy-MM-dd HH:mm:ss") & "')"
		ExecuteNonQuery(sql)

		'vLastAct = Me.Text & " SAVE SO No. " & txtSOno.Text & Space(1) & txtCustNo.Text & Space(1) & txtSmnNo.Text & Space(1) & txtShipTo.Text
		'WriteToLogs(vLastAct)

	End Sub

	Private Sub UpdateSOHdr()
		sql = "update sohdrtbl set transdate='" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "'," &
			  "custno='" & cboCustName.Text.Substring(0, 5) & "',shipto='" & cboShipTo.Text.Substring(0, 5) & "'," &
			  "smnno='" & cboSmnName.Text.Substring(0, 3) & "',pono='" & txtPONo.Text & "',delstat='OPEN'," &
			  "user='" & lblUser.Text & "',30days=" & CDbl(IIf(lbl30days.Text = "", 0, lbl30days.Text)) & "," &
			  "60days=" & CDbl(IIf(lbl60days.Text = "", 0, lbl60days.Text)) & ",90days= " & CDbl(IIf(lbl90days.Text = "", 0, lbl90days.Text)) & "," &
			  "120days=" & CDbl(IIf(lbl91over.Text = "", 0, lbl91over.Text)) & "," &
			  "soamt=" & CDbl(IIf(lblTotNetAmt.Text = "", 0, lblTotNetAmt.Text)) & ",status='" & lblSOStat.Text & "'," &
			  "totar=" & CDbl(IIf(lblAcctBal.Text = "", 0, lblAcctBal.Text)) & ",deldate='" & Format(CDate(dpDelDate.Text), "yyyy-MM-dd") & "'," &
			  "apprvdby=null,remarks='" & txtRemarks.Text & "',plntno='" & cboPlnt.Text.Substring(0, 3) & "',branch='" & vLoggedBranch & "'," &
			  "91over = " & CDbl(IIf(lbl121over.Text = "", 0, lbl121over.Text)) & ",deposit = " & CDbl(IIf(lblDepAmt.Text = "", 0, lblDepAmt.Text)) & "," &
			  "pdc = " & CDbl(IIf(lblDepAmt.Text = "", 0, lblDepAmt.Text)) & ",pdate = '" & Format(CDate(Now()), "yyyy-MM-dd HH:mm:ss") & "' " &
			  "where sono='" & txtSOno.Text & "'"
		ExecuteNonQuery(sql)

		'vLastAct = Me.Text & " UPDATE SO No. " & txtSOno.Text & Space(1) & txtCustNo.Text & Space(1) & txtSmnNo.Text & Space(1) & txtShipTo.Text
		'WriteToLogs(vLastAct)

	End Sub

	Private Sub SaveDetProc()
		ClrErrMsg()
		dt = GetDataTable("select * from sodettbl where sono = '" & txtSOno.Text & "'")
		If Not CBool(dt.Rows.Count) Then
			Call SaveSODet()

			'strReport = "save item no.:" & txtItm.Text
			'Call SaveLogs()

		Else
			sql = "delete from sodettbl where sono = '" & txtSOno.Text & "'"
			ExecuteNonQuery(sql)

			Call SaveSODet()

			'strReport = "Update Item No.:" & txtItm.Text
			'Call SaveLogs()

		End If

		txtSOno.ReadOnly = False

		Call dt.Dispose()

	End Sub

	Private Sub SaveSODet()
		sql = "insert into sodettbl(sono,codeno,qty,wt,sp,itmamt,itmno,um,qtbal,wtbal,prodreq,discamt,netamt,status,sloc) " &
			  "select '" & txtSOno.Text & "',codeno,qty,wt,sp,itmamt,itmno,um,qtbal,wtbal,prodreq,discamt,netamt,status,sloc " &
			  "from tempsodettbl where user = '" & lblUser.Text & "'"
		ExecuteNonQuery(sql)

	End Sub

	Protected Sub lbPrint_Click(sender As Object, e As EventArgs)
		'ClrErrMsg()
		'lblErrMsg.Text = "Not Yet Available"
		'Exit Sub

		vDocNo = txtSOno.Text
		DeliveryToPrint = "SO"
		Response.Redirect("SDrepViewer.aspx")

	End Sub

	Protected Sub lbDelete_Click(sender As Object, e As EventArgs)

	End Sub

	Protected Sub lbClose_Click(sender As Object, e As EventArgs)
		Response.Redirect("SalesAndDist.aspx")

	End Sub

	Protected Sub lbDelItem_Click(sender As Object, e As EventArgs)

	End Sub

	Private Sub SalesOrder_Unload(sender As Object, e As EventArgs) Handles Me.Unload
		Try
			ClrErrMsg()
			dt = GetDataTable("select ifnull(soamt,0) from sohdrtbl where sono = '" & txtSOno.Text & "'")
			If Not CBool(dt.Rows.Count) Then
				strReport = "Unload Form Not Save "

				sql = "delete from sodettbl where sono = '" & txtSOno.Text & "'"
				ExecuteNonQuery(sql)

				Response.Redirect("SalesAndDist.aspx")

				'Answer = MsgBox("SO No.:" & txtSOno.Text & " not yet Save, Are you sure to exit?", "SO Processing", vbExclamation + vbYesNo)
				'If Answer = vbYes Then


				'Else
				'	strReport = "Continue Editing"

				'End If

				Call SaveLogs()

			Else
				'check if Det and Hdr is equal
				Dim HdrAmt As Double
				For Each dr As DataRow In dt.Rows
					HdrAmt = CDbl(dr.Item(0).ToString())

				Next

				Dim DetAmt As Double = CDbl(IIf(lblTotAmt.Text = "", 0, lblTotAmt.Text))

				'If DetAmt <> HdrAmt Then
				'	Answer = MsgBox("SO Detailed not Equal to Total SO", "Not Possible", vbExclamation + vbYesNo)
				'	If Answer = vbYes Then
				'		Response.Redirect("SalesAndDist.aspx")
				'	Else
				'		Exit Sub

				'	End If

				'End If

			End If

			Call dt.Dispose()

			strReport = "Close Form"
			Call SaveLogs()

		Catch ex As Exception

		End Try


	End Sub

	Private Sub cboSmnName_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboSmnName.SelectedIndexChanged
		ClrErrMsg()
		Call getCustListSmn()

	End Sub

	Private Sub cboCustName_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboCustName.SelectedIndexChanged
		'Try
		ClrErrMsg()
		If dpTransDate.Text = Nothing Then
			lblErrMsg.Text = "Select Transaction Date"
			Exit Sub
		End If

		Select Case cboCustName.Text
			Case "Load All"
				getCustListALL()

			Case "Load SMN"
				getCustListSmn()

			Case Else
				dt = GetDataTable("select sptype,areano,ifnull(crlimit,0) as crlimit,custtype,term from custmasttbl where " &
								  "custno = '" & cboCustName.Text.Substring(0, 5) & "'")
				If Not CBool(dt.Rows.Count) Then
					lblErrMsg.Text = "Not found."
					Exit Sub
				Else
					For Each dr As DataRow In dt.Rows
						lblSPtype.Text = dr.Item(0).ToString() & ""
						lblArea.Text = dr.Item(1).ToString() & ""
						lblCRLimit.Text = Format(CDbl(dr.Item(2).ToString()), "#,##0.00")
						lblCustType.Text = dr.Item(3).ToString() & ""
						lblTerm.Text = dr.Item(4).ToString() & "Days"

					Next

				End If

				Call dt.Dispose()


				If cboCustName.Text <> "" Then
					Call cboShipTo.Items.Clear()
					dt = GetDataTable("select concat(custno,space(1),bussname) from custmasttbl where moacctno = '" & cboCustName.Text.Substring(0, 5) & "' " &
									  "order by bussname")
					If Not CBool(dt.Rows.Count) Then
						Exit Sub
					Else
						cboShipTo.Items.Add("")
						For Each dr As DataRow In dt.Rows
							cboShipTo.Items.Add(dr.Item(0).ToString())

						Next

					End If

					Call dt.Dispose()

					If CheckBox1.Checked = True Then

					Else
						Call ChkNewBals()
					End If

					Call GetMMastCust()

					cboShipTo.Focus()

				End If

		End Select

		'Catch ex As Exception
		'	AdmMsgBox("Error in Loading Customer's Details", MsgBoxStyle.Critical)

		'End Try
	End Sub

	Private Sub getSetUpAllVer2()

		dt = GetDataTable("select * from information_schema.tables where table_schema =  '" & vDbase & "' and " &
						  "table_name = '" & "tempaging_" & lblUser.Text & "'")
		If Not CBool(dt.Rows.Count) Then
			sql = "CREATE TABLE tempaging_" & lblUser.Text & " LIKE tempaging"
			ExecuteNonQuery(sql)
		Else
			sql = "truncate table tempaging_" & lblUser.Text
			ExecuteNonQuery(sql)
		End If

		'1 get invoice setup
		sql = "insert into tempaging_" & lblUser.Text & "(invno,transdate,originvamt,tc,custno,smnno,shipto,pc,areano,term," &
			  "custname,shiptoname,branch) select a.invno,a.transdate,round(a.netamt,2),a.tc,a.custno,a.smnno,ifnull(a.shipto,a.custno)," &
			  "a.pc,b.areano,b.term,b.bussname,c.bussname,'" & vLoggedBranch & "' from saleshdrtbl a left join custmasttbl b on a.custno=b.custno " &
			  "left join custmasttbl c on a.shipto=c.custno where a.transdate <= '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
			  "and a.status <> 'void' and a.arflag is null and a.custno = '" & cboCustName.Text.Substring(0, 5) & "'"
		ExecuteNonQuery(sql)

		'2 get DM setup w/ DM # ref
		sql = "insert into tempaging_" & lblUser.Text & "(invno,transdate,originvamt,tc,custno,smnno,shipto,pc,areano,term," &
			  "custname,shiptoname,branch) select a.dmcmno,a.transdate,ifnull(a.totamt,0),a.tc,a.custno,a.smnno,a.custno,a.pc," &
			  "b.areano,b.term,b.bussname,b.bussname,'" & vLoggedBranch & "' from custdmcmhdrtbl a left join custmasttbl b on a.custno=b.custno " &
			  "where a.transdate <= '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and a.tc = '92' and a.status <> 'void' and " &
			  "a.custno = '" & cboCustName.Text.Substring(0, 5) & "'"
		ExecuteNonQuery(sql) 'new code

		'3. get payment w/ ref no
		sql = "update tempaging_" & lblUser.Text & " a,(select a.refno,a.tc,round(sum(a.oramt),2) as oramt,c.custno from coldettbl a " &
			  "left join colhdrtbl c on a.orno=c.orno where a.tc <> '60' and a.oramt <> 0 and c.status <> 'void' and " &
			  "c.transdate <= '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and c.custno = '" & cboCustName.Text.Substring(0, 5) & "' group by a.refno) as colref " &
			  "set a.origoramt = colref.oramt where a.invno = colref.refno and a.tc = colref.tc and a.custno = colref.custno" 'and c.branch = '" & vLoggedBranch & "' 
		ExecuteNonQuery(sql)

		'4. get CM
		sql = "update tempaging_" & lblUser.Text & " a,(select a.refno,a.reftc,round(sum(a.amt),2) as cmamt,c.custno from custdmcmdettbl a " &
			  "left join custdmcmhdrtbl c on a.dmcmno=c.dmcmno where c.tc = '90' and a.amt <> 0 and c.custno = '" & cboCustName.Text.Substring(0, 5) & "'" &
			  "and c.status = 'posted' and c.transdate <= '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
			  "group by a.refno,a.reftc) as cmref set a.cmamt = cmref.cmamt " &
			  "where a.invno = cmref.refno and a.tc = cmref.reftc and a.custno = cmref.custno" 'and c.branch = '" & vLoggedBranch & "' 
		ExecuteNonQuery(sql)

		'5. PDC w/ ref
		sql = "update tempaging_" & lblUser.Text & " a,(select a.refno,a.tc,round(sum(a.oramt),2) as oramt,c.custno from coldettbl a " &
			  "left join colhdrtbl c on a.orno=c.orno where a.tc <> '60' and a.oramt <> 0 and c.custno = '" & cboCustName.Text.Substring(0, 5) & "' " &
			  "and c.status <> 'void' and c.transdate > '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
			  "group by a.refno) as colref set a.pdc = colref.oramt " &
			  "where a.invno = colref.refno and a.tc = colref.tc and a.custno = colref.custno"
		ExecuteNonQuery(sql)

		'6. deposits
		sql = "insert into tempaging_" & lblUser.Text & "(invno,transdate,origoramt,tc,custno,smnno,shipto,pc,areano,term," &
			  "custname,shiptoname,branch) select c.orno,c.transdate,round(sum(a.oramt),2) as oramt,a.tc,c.custno,c.smnno,c.custno,c.pc, " &
			  "b.areano,b.term,b.bussname,b.bussname,'" & vLoggedBranch & "' from coldettbl a left join colhdrtbl c on a.orno=c.orno " &
			  "left join custmasttbl b on c.custno=b.custno where a.tc = '60' and a.oramt <> 0 and c.status <> 'void' " &
			  "and c.transdate <= '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and a.coltype = 'trade' and " &
			  "c.custno = '" & cboCustName.Text.Substring(0, 5) & "' group by a.refno"
		ExecuteNonQuery(sql)

		'7. deposits PDC
		sql = "insert into tempaging_" & lblUser.Text & "(invno,transdate,origoramt,tc,custno,smnno,shipto,pc,areano,term," &
			  "custname,shiptoname,branch) select c.orno,c.transdate,round(sum(a.oramt),2) as oramt,a.tc,c.custno,c.smnno,c.custno,c.pc, " &
			  "b.areano,b.term,b.bussname,b.bussname,'" & vLoggedBranch & "' from coldettbl a left join colhdrtbl c on a.orno=c.orno " &
			  "left join custmasttbl b on c.custno=b.custno where a.tc = '60' and a.oramt <> 0 and c.status <> 'void' and " &
			  "c.transdate > '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and a.coltype = 'trade' and " &
			  "c.custno = '" & cboCustName.Text.Substring(0, 5) & "' group by a.refno"
		ExecuteNonQuery(sql)

		FinalUpdateAR() 'ok
		GetAgingAllver2()

	End Sub

	Private Sub FinalUpdateAR()
		sql = "update tempaging_" & lblUser.Text & " set invamt = ifnull(originvamt,0) + ifnull(dmamt,0),dep = ifnull(origoramt,0) + ifnull(cmamt,0)"
		ExecuteNonQuery(sql)

		sql = "update tempaging_" & lblUser.Text & " set totalar = invamt - ifnull(dep,0)" ' where user = '" & lblUser.Text & "'"
		ExecuteNonQuery(sql)

		sql = "update tempaging_" & lblUser.Text & " set net = totalar-ifnull(pdc,0)" 'where user = '" & lblUser.Text & "'"
		ExecuteNonQuery(sql)

		sql = "update tempaging_" & lblUser.Text & " set totalar = round(totalar,2)" 'where user = '" & lblUser.Text & "'"
		ExecuteNonQuery(sql)

		sql = "delete from tempaging_" & lblUser.Text & " where totalar = 0" 'and user = '" & lblUser.Text & "'"
		ExecuteNonQuery(sql)

		sql = "update tempaging_" & lblUser.Text & " set days = datediff('" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "', transdate) - term" '" & _
		ExecuteNonQuery(sql)


		sql = "update tempaging_" & lblUser.Text & " set days = 0 where days < 0" ' and user = '" & lblUser.Text & "'"
		ExecuteNonQuery(sql)

		sql = "update tempaging_" & lblUser.Text & " set datedue = date_add(transdate,interval term day)"
		ExecuteNonQuery(sql)

		sql = "update tempaging_" & lblUser.Text & " a left join saleshdrtbl b on a.invno=b.invno " &
			  "set a.docno = b.docno,a.dsrno = b.dsrno where a.dsrno is null and a.branch = '" & vLoggedBranch & "'" 'a.tc = '84' and
		ExecuteNonQuery(sql)

		sql = "update tempaging_" & lblUser.Text & " a left join colhdrtbl b on a.invno=b.orno " &
			  "set a.docno = b.docno where a.dsrno is null and a.branch = '" & vLoggedBranch & "'"
		ExecuteNonQuery(sql)

		'update PO
		sql = "update tempaging_" & lblUser.Text & " a,(select a.pono,b.dono,c.invno FROM sohdrtbl a left join isshdrtbl b on a.sono=b.sono " &
			  "left join saleshdrtbl c on b.dono=c.dono where c.custno = a.custno) as b set a.dsrno = b.pono where a.invno=b.invno"
		ExecuteNonQuery(sql)

	End Sub

	Private Sub GetAgingAllver2()

		sql = "update tempaging_" & lblUser.Text & " set 30d = case when days between 1 and 30 then totalar else 0 end," &
			  "60d = case when days between 31 and 60 then totalar else 0 end," &
			  "90d = case when days between 61 and 90 then totalar else 0 end," &
			  "120d = case when days between 91 and 120 then totalar else 0 end," &
			  "91d = case when days between 121 and 999999 then totalar else 0 end," &
			  "curr = case when days < 1 then totalar else 0 end " &
			  "where invno = invno"
		ExecuteNonQuery(sql)

	End Sub

	Private Sub ChkNewBals()
		Try
			Call getSetUpAllVer2()

			'====1 to 30 ===
			dt = GetDataTable("select sum(ifnull(totalar,0)) from tempaging_" & lblUser.Text & " where days between 1 and 30 and tc <> '60' group by custno")
			If Not CBool(dt.Rows.Count) Then
				lbl30days.Text = "0.00"

			Else
				For Each dr As DataRow In dt.Rows
					lbl30days.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0.00 ; (#,##0.00)")
				Next

				Call dt.Dispose()

			End If

			'====31 to 60 ===
			dt = GetDataTable("select sum(ifnull(totalar,0)) from tempaging_" & lblUser.Text & " where days between 31 and 60 and tc <> '60' group by custno")
			If Not CBool(dt.Rows.Count) Then
				lbl60days.Text = "0.00"

			Else
				For Each dr As DataRow In dt.Rows
					lbl60days.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0.00 ; (#,##0.00)")
				Next

				Call dt.Dispose()

			End If

			'====61 to 90 ===
			dt = GetDataTable("select sum(ifnull(totalar,0)) from tempaging_" & lblUser.Text & " where days between 61 and 90 and tc <> '60' group by custno")
			If Not CBool(dt.Rows.Count) Then
				lbl90days.Text = "0.00"

			Else
				For Each dr As DataRow In dt.Rows
					lbl90days.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0.00 ; (#,##0.00)")
				Next

				Call dt.Dispose()

			End If


			'====91-120 ===
			dt = GetDataTable("select sum(ifnull(totalar,0)) from tempaging_" & lblUser.Text & " where days between 91 and 120 and tc <> '60' group by custno")
			If Not CBool(dt.Rows.Count) Then
				lbl91over.Text = "0.00"

			Else
				For Each dr As DataRow In dt.Rows
					lbl91over.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0.00 ; (#,##0.00)")
				Next

				Call dt.Dispose()

			End If

			'====121 days over===

			dt = GetDataTable("select sum(ifnull(totalar,0)) from tempaging_" & lblUser.Text & " where days > 121 and tc <> '60' group by custno")
			If Not CBool(dt.Rows.Count) Then
				lbl121over.Text = "0.00"

			Else
				For Each dr As DataRow In dt.Rows
					lbl121over.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0.00 ; (#,##0.00)")

				Next

				Call dt.Dispose()

			End If

			'===add over due acct
			Dim overdue As Double = CDbl(IIf(lbl30days.Text = "", 0, lbl30days.Text)) + CDbl(IIf(lbl60days.Text = "", 0, lbl60days.Text)) +
									CDbl(IIf(lbl90days.Text = "", 0, lbl90days.Text)) + CDbl(IIf(lbl91over.Text = "", 0, lbl91over.Text)) +
									CDbl(IIf(lbl121over.Text = "", 0, lbl121over.Text))


			'lblOverDue.Text = Format(overdue, "#,##0.00 ; (#,##0.00)")

			''===get deposits
			dt = GetDataTable("select round(sum(a.oramt),2) as oramt from coldettbl a left join colhdrtbl c on a.orno=c.orno " &
							  "where a.tc = '60' and c.status <> 'void' and a.coltype = 'trade' " &
							  "and c.transdate <= '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
							  "c.custno = '" & cboCustName.Text.Substring(0, 5) & "' group by c.custno")
			If Not CBool(dt.Rows.Count) Then
				lblDepAmt.Text = "0.00"
			Else
				For Each dr As DataRow In dt.Rows
					lblDepAmt.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0.00 ; (#,##0.00)")
				Next

				Call dt.Dispose()

			End If

			'====get PDC
			dt = GetDataTable("select round(sum(a.oramt),2) as oramt from coldettbl a left join colhdrtbl c on a.orno=c.orno " &
							  "where c.status <> 'void' and a.coltype = 'trade' and c.transdate > '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "'" &
							  "and c.custno = '" & cboCustName.Text.Substring(0, 5) & "' group by c.custno")
			If Not CBool(dt.Rows.Count) Then
				lblPDC.Text = "0.00"
			Else
				For Each dr As DataRow In dt.Rows
					lblPDC.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0.00 ; (#,##0.00)")
				Next

				Call dt.Dispose()

			End If

			'total A/R
			dt = GetDataTable("select sum(ifnull(totalar,0)) from tempaging_" & lblUser.Text & " group by custno")
			If Not CBool(dt.Rows.Count) Then
				lblAcctBal.Text = "0.00"

			Else
				For Each dr As DataRow In dt.Rows
					lblAcctBal.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0.00 ; (#,##0.00)")

				Next

				Call dt.Dispose()

			End If


			'===add all acct
			'Dim allbal As Double = CDbl(IIf(lblCurr.Text = "", 0, lblCurr.Text)) + CDbl(IIf(lbl30days.Text = "", 0, lbl30days.Text)) +
			'					   CDbl(IIf(lbl60days.Text = "", 0, lbl60days.Text)) + CDbl(IIf(lbl90days.Text = "", 0, lbl90days.Text)) +
			'					   CDbl(IIf(lbl91over.Text = "", 0, lbl91over.Text)) + CDbl(IIf(lbl121over.Text = "", 0, lbl121over.Text))
			'lblAcctBal.Text = Format(allbal, "#,##0.00 ; (#,##0.00)")

			''==== NEW BALS
			'Dim NewBal As Double = allbal + CDbl(IIf(lblSOamt.Text = "", 0, lblSOamt.Text))
			'lblNewBal.Text = Format(NewBal, "#,##0.00 ; (#,##0.00)")

			''==== CREDIT LIMIT BALS

			'If CDbl(lblCRLimit.Text) = 0 Then
			'	lblAvaiLimit.Text = "0.00"
			'Else
			'	Dim CrBal As Double = CDbl(IIf(lblCRLimit.Text = "", 0, lblCRLimit.Text)) - CDbl(IIf(lblNewBal.Text = "", 0, lblNewBal.Text)) +
			'						  CDbl(IIf(lblDepAmt.Text = "", 0, lblDepAmt.Text))
			'	lblAvaiLimit.Text = Format(CrBal, "#,##0.00 ; (#,##0.00)")
			'End If

			'If CDbl(lblAvaiLimit.Text) < 0 Then
			'	lblAvaiLimit.ForeColor = Color.Red
			'Else
			'	lblAvaiLimit.ForeColor = Color.Black
			'End If

			'Call getOpenSOamt()

			Call GetSOstatnew()

		Catch ex As Exception
			lblErrMsg.Text = "Error in processing Credit Limit"

		End Try

	End Sub

	Private Sub GetSOstatnew()

		If lbl121over.Text > 0 Then
			lblSOStat.Text = "OVER DUE"
			Exit Sub
		ElseIf lbl91over.Text > 0 Then
			lblSOStat.Text = "OVER DUE"
			Exit Sub
		ElseIf lbl90days.Text > 0 Then
			lblSOStat.Text = "OVER DUE"
			Exit Sub
		ElseIf lbl60days.Text > 0 Then
			lblSOStat.Text = "OVER DUE"
			Exit Sub
		ElseIf lbl30days.Text > 0 Then
			lblSOStat.Text = "OVER DUE"
			Exit Sub
			'ElseIf lblAvaiLimit.Text < 0 Then
			'	lblSOStat.Text = "OVER CR LIMIT"
			'	Exit Sub
		Else
			lblSOStat.Text = "APPROVED"

		End If

		'End If

	End Sub

	Private Sub ClrSONoOnly()
		txtSOno.Text = ""
	End Sub

	Private Sub ClrHdrDetails()

		txtPONo.Text = ""

		cboCustName.Text = Nothing
		cboShipTo.Text = Nothing
		cboSmnName.Text = Nothing
		cboPClass.Enabled = True
		lbl30days.Text = ""
		lbl60days.Text = ""
		lbl90days.Text = ""
		lbl91over.Text = ""
		lblAddress.Text = ""

		lblArea.Text = ""

		lblCustType.Text = ""

		lblSOStat.Text = ""
		lblSPtype.Text = ""
		lblTerm.Text = ""
		lblTotAmt.Text = "0.00"
		lblTotQty.Text = "0"
		lblTotWt.Text = "0.00"
		txtRemarks.Text = ""
		txtSOno.ReadOnly = False
		tssDocStat.Text = "New"
		tssDocNo.Text = ""

	End Sub

	Private Sub cboShipTo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboShipTo.SelectedIndexChanged
		ClrErrMsg()
		dt = GetDataTable("select address from custmasttbl where custno = '" & cboShipTo.Text.Substring(0, 5) & "'")
		If Not CBool(dt.Rows.Count) Then
			lblErrMsg.Text = "Not found."
			Exit Sub
		Else
			For Each dr As DataRow In dt.Rows
				lblAddress.Text = dr.Item(0).ToString()
			Next

		End If

		Call dt.Dispose()

		'select default salesman
		If cboSmnName.Text <> "" Then
			dt = GetDataTable("select a.smnno from custmasttbl a left join smnmtbl b on a.smnno=b.smnno " &
							  "where a.custno = '" & cboShipTo.Text.Substring(0, 5) & "'")
			If Not CBool(dt.Rows.Count) Then
				Exit Sub
			Else
				For Each dr As DataRow In dt.Rows
					If cboSmnName.Text.Substring(0, 3) <> dr.Item(0).ToString() Then
						'Answer = MsgBox("Do you want to UPDATE Default Salesman for this Customer?", "SO Processing", vbExclamation + vbYesNo)
						'If Answer = vbYes Then
						sql = "update custmasttbl set smnno = '" & cboSmnName.Text.Substring(0, 3) & "' where " &
							  "custno = '" & cboShipTo.Text.Substring(0, 5) & "'"
						ExecuteNonQuery(sql)

						'End If

					End If
				Next
			End If

			dt.Dispose()

		End If

	End Sub

	Protected Sub MMdescProc()
		ClrErrMsg()
		If cboCustName.Text = "" Then
			lblErrMsg.Text = "Select Customer First"
			Exit Sub
		End If

		dt = GetDataTable("select codeno,billref,qtpk,ifnull(wtfr,0),precodeno from mmasttbl where ifnull(codename,mmdesc) = '" & cboMMdesc.Text & "'")
		If Not CBool(dt.Rows.Count) Then
			lblErrMsg.Text = "Not found."
			Exit Sub

		Else
			For Each dr As DataRow In dt.Rows
				Select Case UCase(dr.Item(4).ToString() & "")
					Case "OTC"
						txtCodeNo.Text = dr.Item(0).ToString()
						lblBillRef.Text = dr.Item(1).ToString() & ""
						Dim QtPk As Double = CDbl(dr.Item(2).ToString())
						Dim UnitVol As Double = CDbl(dr.Item(3).ToString())
						lblQtPk.Text = Format(UnitVol / 1000, "#,##0.00")

					Case Else
						txtCodeNo.Text = dr.Item(0).ToString()
						lblBillRef.Text = dr.Item(1).ToString() & ""
						lblQtPk.Text = dr.Item(2).ToString() & "" ' for revision

				End Select
			Next

		End If

		Call dt.Dispose()
		Call GetSP()

	End Sub

	Protected Sub cboMMdesc_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboMMdesc.SelectedIndexChanged
		ClrErrMsg()
		If dpTransDate.Text = Nothing Then
			lblErrMsg.Text = "Pick Date"
			Exit Sub
		ElseIf dpDelDate.Text = Nothing Then
			lblErrMsg.Text = "Delivery date is not Selected"
			Exit Sub
		ElseIf cboCustName.Text = Nothing Then
			lblErrMsg.Text = "Select Customer"
			Exit Sub
		End If

		MMdescProc()

	End Sub

	Private Sub GetLastPrice()
		ClrErrMsg()
		Dim spamt As Double
		dt = GetDataTable("select ifnull(sp,0) from salesdettbl a left join saleshdrtbl b on a.invno=b.invno where a.codeno = '" & txtCodeNo.Text & "' and " &
						  "b.custno = '" & cboShipTo.Text.Substring(0, 5) & "' and b.status <> 'VOID' and b.transdate " &
						  "<= '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
						  "order by b.transdate desc limit 1")
		If Not CBool(dt.Rows.Count) Then
			lblErrMsg.Text = "No SP for material " & txtCodeNo.Text & " Found"
			txtQty.Focus()
			Exit Sub
		Else
			For Each dr As DataRow In dt.Rows
				spamt = CDbl(dr.Item(0).ToString())
				txtSP.Text = Format(spamt, "##,##0.0000")
				txtQty.Focus()
			Next

		End If

		Call dt.Dispose()
	End Sub

	Protected Sub GetSP()
		ClrErrMsg()
		If CheckBox2.Checked = True Then
			GetLastPrice()

		Else
			Select Case lblSPtype.Text
				Case "Area"
					dt = GetDataTable("select currprc from plsttbl where codeno = '" & txtCodeNo.Text & "' and " &
									  "areano = '" & lblArea.Text & "' and '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
									  "between effdate and prvdate")
					If Not CBool(dt.Rows.Count) Then
						GetLastPrice()
					Else
						For Each dr As DataRow In dt.Rows
							SP = dr.Item(0).ToString()
							txtSP.Text = Format(SP, "#,##0.0000")

						Next

					End If

					Call dt.Dispose()

				Case "Customer"
					dt = GetDataTable("select currprc,ifnull(drate,0) from plsttbl where codeno = '" & txtCodeNo.Text & "' and " &
									  "custno = '" & cboCustName.Text.Substring(0, 5) & "' and '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
									  "between effdate and prvdate")
					If Not CBool(dt.Rows.Count) Then
						GetLastPrice()

					Else
						For Each dr As DataRow In dt.Rows
							SP = dr.Item(0).ToString()
							txtSP.Text = Format(SP, "#,##0.0000")
							'txtDiscRate.Text = Format(CDbl(dr.Item(1).ToString()), "#,##0.00")

						Next
					End If

					Call dt.Dispose()

			End Select

		End If

	End Sub

	Private Sub txtCodeNo_TextChanged(sender As Object, e As EventArgs) Handles txtCodeNo.TextChanged


	End Sub

	Protected Sub SpProc()
		'Try
		ClrErrMsg()
		SP = CDbl(IIf(txtSP.Text = "", 0, txtSP.Text))
		txtSP.Text = Format(SP, "#,##0.0000")
		Select Case UCase(lblBillRef.Text)
			Case "QTY"
				Amt = CDbl(IIf(txtQty.Text = "", 0, txtQty.Text)) * SP
			Case "WT"
				Amt = CDbl(IIf(txtWt.Text = "", 0, txtWt.Text)) * SP
			Case Else
				Amt = CDbl(IIf(txtWt.Text = "", 0, txtWt.Text)) * SP
		End Select

		lblAmt.Text = Format(Amt, "#,##0.00")
		'DiscAmt = Amt * CDbl(IIf(txtDiscRate.Text = "", 0, txtDiscRate.Text)) / 100
		txtDiscAmt.Text = Format(DiscAmt, "#,##0.00")
		NetAmt = Amt - DiscAmt
		lblNetAmt.Text = Format(NetAmt, "#,##0.00")

		If SP <= 0 Then
			'If lvDetSO.Items.Count = 0 Then
			'	MsgBox("Zero is not Allowed in 1st Line Item")
			'	'txtSP.Focus()
			'	Exit Sub

			'Else
			' change to Mysql query

			'For g As Integer = 0 To lvDetSO.Items.Count - 1
			'	If txtCodeNo.Text = lvDetSO.Items(g).SubItems(1).Text Then
			'		Dim admAmt As Double = CDbl(IIf(lvDetSO.Items(g).SubItems(5).Text = "", 0, lvDetSO.Items(g).SubItems(5).Text))
			'		'MsgBox(admAmt)
			'		If admAmt = 0 Then
			'			MsgBox("Zero is not Allowed")
			'			'txtSP.Focus()
			'			Exit Sub
			'		Else

			'		End If

			'	End If

			'Next

			'End If

		End If

		txtDiscAmt.Focus()


		'Catch ex As Exception

		'End Try
	End Sub

	Private Sub cboPlnt_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboPlnt.SelectedIndexChanged

		If cboPlnt.Text = "" Then
			Exit Sub
		End If




	End Sub

	Protected Sub PopPClass()
		ClrErrMsg()
		dt = GetDataTable("select concat(pc,space(1),pclass) from pctrtbl where tradetype = 'trade'")
		If Not CBool(dt.Rows.Count) Then
			lblErrMsg.Text = "PC Not found."
			Exit Sub
		Else
			For Each dr As DataRow In dt.Rows
				cboPClass.Items.Add(dr.Item(0).ToString())

			Next

		End If

		Call dt.Dispose()

	End Sub

	Protected Sub AddNewLineItem()
		ClrErrMsg()
		If cboCustName.Text = "" Then
			lblErrMsg.Text = "Customer is Blank"
			Exit Sub

		ElseIf CheckBox3.Checked = True Then
			If CDbl(IIf(txtSP.Text = "", 0, txtSP.Text)) > 0 Then
				lblErrMsg.Text = "Deal Item Should have no SP"
				Exit Sub
			End If

		End If

		If txtItm.Text = "" Then
			lblErrMsg.Text = "Line Item Number is Blank"
			Exit Sub
		ElseIf txtCodeNo.Text = "" Then
			cboMMdesc.Focus()
			lblErrMsg.Text = "Select Material from SO"
			Exit Sub

		ElseIf txtQty.Text = "" Then
			txtQty.Focus()
			lblErrMsg.Text = "Qty is Blank"
			Exit Sub

		ElseIf txtWt.Text = "" Then
			txtWt.Focus()
			lblErrMsg.Text = "Wt/Vol. is Blank"
			Exit Sub

		ElseIf txtSP.Text = "" Then
			txtSP.Focus()
			lblErrMsg.Text = "SP is Blank"
			Exit Sub

		End If

		dt = GetDataTable("select ifnull(billref,'No Bill Ref') from mmasttbl where codeno = '" & txtCodeNo.Text & "'")
		If Not CBool(dt.Rows.Count) Then
			lblErrMsg.Text = "Code No. " & txtCodeNo.Text & " Not Found"
			Exit Sub

		Else
			For Each dr As DataRow In dt.Rows
				Select Case dr.Item(0).ToString()
					Case "No Bill Ref"
						lblErrMsg.Text = "No Bill Ref Assigned, please Contact Material Maintenance "
						Exit Sub

					Case ""
						lblErrMsg.Text = "No Bill Ref Assigned, please Contact Material Maintenance "
						Exit Sub

					Case Else
						lblBillRef.Text = dr.Item(0).ToString()

				End Select

			Next
		End If

		dt.Dispose()

		Select Case lblLineItm.Text
			Case "New"
				sql = "delete from tempsodettbl where user = '" & lblUser.Text & "'"
				ExecuteNonQuery(sql)

		End Select

		dt = GetDataTable("select concat(a.codeno,space(1),ifnull(b.codename,b.mmdesc)) from tempsodettbl a " &
						  "left join mmasttbl b on a.codeno=b.codeno where a.itmno = " & txtItm.Text & " and " &
						  "a.user = '" & lblUser.Text & "'")
		If Not CBool(dt.Rows.Count) Then

		Else
			For Each dr As DataRow In dt.Rows
				lblErrMsg.Text = "Line Item # " & txtItm.Text & " Already Used to " & dr.Item(0).ToString() & ", New Item # will be assigned"
				GetLineItmNo()
				Exit Sub
			Next

		End If

		dt.Dispose()

		If CheckBox3.Checked = True Then
			admDeal = "Y"
		Else
			admDeal = "N"
		End If

		If CheckBox4.Checked = True Then
			WtQty = CDbl(IIf(lblQtPk.Text = "", 0, lblQtPk.Text)) * CDbl(IIf(txtQty.Text = "", 0, txtQty.Text))
		Else
			WtQty = CDbl(IIf(txtQty.Text = "", 0, txtQty.Text))
		End If

		txtWt.Text = Format(WtQty, "#,##0.00")

		SP = CDbl(IIf(txtSP.Text = "", 0, txtSP.Text))

		Select Case vLoggedBussArea
			Case "8100"
				txtSP.Text = Format(SP, "#,##0.0000")
			Case "8200"
				txtSP.Text = Format(SP, "#,##0.00")
			Case "8300"
				txtSP.Text = Format(SP, "#,##0.0000")

		End Select

		Select Case LCase(lblBillRef.Text)
			Case "qty"
				GrossAmt = CDbl(IIf(txtQty.Text = "", 0, txtQty.Text)) * CDbl(IIf(txtSP.Text = "", 0, txtSP.Text))

			Case "wt"
				GrossAmt = CDbl(IIf(txtWt.Text = "", 0, txtWt.Text)) * CDbl(IIf(txtSP.Text = "", 0, txtSP.Text))

			Case Else
				GrossAmt = CDbl(IIf(txtWt.Text = "", 0, txtWt.Text)) * CDbl(IIf(txtSP.Text = "", 0, txtSP.Text))

		End Select

		lblAmt.Text = Format(GrossAmt, "#,##0.00")

		NetAmt = GrossAmt - CDbl(IIf(txtDiscAmt.Text = "", 0, txtDiscAmt.Text))

		lblNetAmt.Text = Format(NetAmt, "#,##0.00")
		'AdmMsgBox(NetAmt & " / " & GrossAmt & " / " & txtSP.Text)
		If CheckBox4.Checked = True Then
			ChkVal = "Y"
		Else
			ChkVal = "N"
		End If

		sql = "insert into tempsodettbl(itmno,codeno,qty,wt,sp,itmamt,discamt,netamt,um,qtbal,wtbal,status,sloc,user)values" &
			  "(" & CLng(IIf(txtItm.Text = "", 1, txtItm.Text)) & ",'" & txtCodeNo.Text & "'," & CLng(IIf(txtQty.Text = "", 0, txtQty.Text)) & "," &
			  CDbl(IIf(txtWt.Text = "", 0, txtWt.Text)) & "," & CDbl(IIf(txtSP.Text = "", 0, txtSP.Text)) & "," &
			  CDbl(GrossAmt) & "," & CDbl(IIf(txtDiscAmt.Text = "", 0, txtDiscAmt.Text)) & "," & CDbl(NetAmt) & "," &
			  "'" & lblBillRef.Text & "'," & CDbl(IIf(txtQty.Text = "", 0, txtQty.Text)) & "," & CDbl(IIf(txtWt.Text = "", 0, txtWt.Text)) & "," &
			  "'" & admDeal & "','" & ChkVal & "','" & lblUser.Text & "')"
		ExecuteNonQuery(sql)

		'get gridview


		lblLineItm.Text = "On Process"

		Call AddMMtoCust()
		Call ClrLines()

		If lblSOStat.Text <> "APPROVED" Then
			lbPrint.Enabled = False

		Else
			lbPrint.Enabled = True

		End If

		cboMMdesc.Focus()

	End Sub

	Protected Sub GetLineItmNo()
		ClrErrMsg()
		dt = GetDataTable("select count(qty) from tempsodettbl where user = '" & lblUser.Text & "'")
		If Not CBool(dt.Rows.Count) Then
			txtItm.Text = "1"
		Else
			For Each dr As DataRow In dt.Rows
				txtItm.Text = Format(CDbl(dr.Item(0).ToString() + 1), "#,##0")
			Next

		End If

		dt.Dispose()

	End Sub

	Protected Sub AddMMtoCust()
		ClrErrMsg()
		dt = GetDataTable("select * from custmmtbl where custno = '" & cboCustName.Text.Substring(0, 5) & "' and codeno = '" & txtCodeNo.Text & "'")
		If Not CBool(dt.Rows.Count) Then
			Call AddMatl()

		End If

		Call dt.Dispose()

	End Sub

	Protected Sub AddMatl()
		sql = "insert into custmmtbl(custno,codeno,user,dateupdate)values" &
			  "('" & cboCustName.Text.Substring(0, 5) & "','" & txtCodeNo.Text & "','" & lblUser.Text & "'," &
			  "'" & Format(CDate(Now), "yyyy-MM-dd hh:mm:ss") & "')"
		ExecuteNonQuery(sql)

	End Sub

	Protected Sub CheckBox3_CheckedChanged(sender As Object, e As EventArgs)
		If CheckBox3.Checked = True Then
			CheckBox3.Text = "Deal"
		Else
			CheckBox3.Text = "Disc"
		End If

	End Sub

	Protected Sub ClrLines()
		ClrErrMsg()
		txtCodeNo.Text = ""
		cboMMdesc.Text = Nothing
		txtQty.Text = ""
		txtWt.Text = ""
		txtSP.Text = ""
		lblAmt.Text = "0.00"
		lblQtPk.Text = "1"
		lblBillRef.Text = ""
		lblNetAmt.Text = "0.00"
		txtDiscAmt.Text = ""
		CheckBox3.Checked = False

		Select Case lblLineItm.Text
			Case "New"
				txtItm.Text = "1"
			Case Else
				GetLineItmNo()

		End Select

	End Sub

	Protected Sub DgvSOdet_RowDataBound(sender As Object, e As GridViewRowEventArgs)

	End Sub

	Protected Sub DgvSOdet_RowCreated(sender As Object, e As GridViewRowEventArgs)

	End Sub

	'Private Sub btnReset_Click(sender As Object, e As ImageClickEventArgs)
	'	ClrLines()

	'End Sub

	Protected Sub OnConfirm2(sender As Object, e As EventArgs)
		Dim confirmValue As String = Request.Form("confirm_value")
		If confirmValue = "Yes" Then
			RemoveLineItem()
		Else
			lblErrMsg.Text = "Delete Cancelled"
		End If

	End Sub

	Protected Sub OnConfirm(sender As Object, e As EventArgs)
		If txtSOno.Text = Nothing Then
			NewLoadProc()
		Else
			Dim confirmValue As String = Request.Form("confirm_value")
			If confirmValue = "Yes" Then
				Call ClrSONoOnly()
				Call ClrHdrDetails()
				Call ClrLines()
				Call GetSmnList()

				'Dim ds As New DataSet()
				DgvSOdet.DataSource = Nothing
				DgvSOdet.DataBind()
				ds.Clear()

				strReport = "Reset Button"
				Call SaveLogs()
				txtItm.Text = "1"
				lblLineItm.Text = "New"

			Else
				lblErrMsg.Text = "Action Aborted"

			End If
		End If

	End Sub

	Private Sub btnAdd_Click(sender As Object, e As ImageClickEventArgs) Handles btnAdd.Click

		AddNewLineItem()
		FilldgvSOdet()
		cboPClass.Enabled = False
	End Sub

	Protected Sub FilldgvSOdet()
		Dim adapter As New MySqlDataAdapter
		'Dim ds As New DataSet()
		'Dim i As Integer = 0
		sqldata = Nothing

		sqldata = "select a.itmno,a.codeno,ifnull(b.codename,b.mmdesc) as mmdesc,ifnull(a.qty,0) as qty,ifnull(a.wt,0) as wt," &
				  "ifnull(a.sp,0) as sp,ifnull(a.itmamt,0) as itmamt,ifnull(a.discamt,0) as discamt,ifnull(a.netamt,0) as netamt " &
				  "from tempsodettbl a left join mmasttbl b on a.codeno=b.codeno where a.user = '" & lblUser.Text & "' order by a.itmno"

		conn.Open()
		Dim command As New MySqlCommand(sqldata, conn)
		adapter.SelectCommand = command
		adapter.Fill(ds)
		adapter.Dispose()
		command.Dispose()
		conn.Close()
		DgvSOdet.DataSource = ds.Tables(0)
		DgvSOdet.DataBind()

		GetColTotals()

		lblLineItm.Text = "On Process"

	End Sub

	Protected Sub GetColTotals()
		ClrErrMsg()
		dt = GetDataTable("select sum(ifnull(qty,0)),sum(ifnull(wt,0)),sum(ifnull(itmamt,0)),sum(ifnull(discamt,0)),sum(ifnull(netamt,0)) " &
						  "from tempsodettbl where user = '" & lblUser.Text & "' group by user")
		If Not CBool(dt.Rows.Count) Then
			txtItm.Text = "1"
		Else
			For Each dr As DataRow In dt.Rows
				lblTotQty.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0")
				lblTotWt.Text = Format(CDbl(dr.Item(1).ToString()), "#,##0.00")
				lblTotAmt.Text = Format(CDbl(dr.Item(2).ToString()), "#,##0.00")
				lblTotDiscAmt.Text = Format(CDbl(dr.Item(3).ToString()), "#,##0.00")
				lblTotNetAmt.Text = Format(CDbl(dr.Item(4).ToString()), "#,##0.00")

			Next

		End If

		dt.Dispose()

	End Sub

	Private Sub WtProc()
		'Try
		If Not Me.IsPostBack Then
			Dim Wt As Double = CDbl(IIf(txtWt.Text = "", 0, txtWt.Text))
			txtWt.Text = Format(Wt, "#,##0.00")
			txtSP.Focus()
		End If

		'Catch ex As Exception

		'End Try
	End Sub

	Private Sub QtyProc()
		'Try
		'If Not Me.IsPostBack Then
		Dim Qty As Long = CDbl(IIf(txtQty.Text = "", 0, txtQty.Text))

		txtQty.Text = Format(Qty, "#,##0")


		Dim QtyPk As Double = CDbl(IIf(txtQty.Text = "", 0, txtQty.Text)) * CDbl(IIf(lblQtPk.Text = "", 0, lblQtPk.Text))

		If lblQtPk.Text = "" Then
			txtWt.Text = Format(Qty, "#,##0.00")

		Else
			txtWt.Text = Format(QtyPk, "#,##0.00")

		End If

		txtWt.Focus()


		'End If


		'Catch ex As Exception

		'End Try

	End Sub

	Private Sub txtQty_TextChanged(sender As Object, e As EventArgs) Handles txtQty.TextChanged
		QtyProc()
		SpProc()
		txtWt.Focus()

	End Sub

	Private Sub txtWt_TextChanged(sender As Object, e As EventArgs) Handles txtWt.TextChanged
		WtProc()
		txtSP.Focus()

	End Sub

	Private Sub txtSP_TextChanged(sender As Object, e As EventArgs) Handles txtSP.TextChanged
		SpProc()
		txtDiscRate.Text = "0.00"
		txtDiscAmt.Focus()

	End Sub
	Private Sub txtDiscAmt_TextChanged(sender As Object, e As EventArgs) Handles txtDiscAmt.TextChanged
		DiscProc()

	End Sub

	Private Sub DiscProc()
		If CDbl(IIf(txtItm.Text = "", 0, txtItm.Text)) > 10 Then
			AdmMsgBox("You already reached the maximum items per Sales Order, please make separate SO")
			Exit Sub

		End If

		Amt = CDbl(IIf(lblAmt.Text = "", 0, lblAmt.Text))
		DiscRate = CDbl(IIf(txtDiscRate.Text = Nothing, 0, txtDiscRate.Text))
		If DiscRate > 0 Then
			DiscAmt = Amt * DiscRate / 100
			txtDiscAmt.Text = Format(DiscAmt, "#,##0.00")
		Else
			DiscAmt = CDbl(IIf(txtDiscAmt.Text = Nothing, 0, txtDiscAmt.Text))
			txtDiscAmt.Text = Format(DiscAmt, "#,##0.00")
		End If

		NetAmt = Amt - DiscAmt
		lblNetAmt.Text = Format(NetAmt, "#,##0.00")

		'AddNewLineItem()

	End Sub

	Private Sub btnReset_Click(sender As Object, e As ImageClickEventArgs) Handles btnReset.Click
		'temp only
		ClrLines()

		'FilldgvSOdet()
		'GetColTotals()

	End Sub

	Private Sub DgvSOdet_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DgvSOdet.SelectedIndexChanged
		txtItm.Text = DgvSOdet.SelectedRow.Cells(1).Text
		GetLineItmFrTemp()

	End Sub

	Protected Sub GetLineItmFrTemp()
		'tempsodettbl(itmno,codeno,qty,wt,sp,itmamt,discamt,netamt,um,qtbal,wtbal,status,sloc,user
		dt = GetDataTable("select a.codeno,ifnull(b.codename,b.mmdesc),ifnull(a.qty,0),ifnull(a.wt,0),ifnull(a.sp,0)," &
						  "a.itmamt,ifnull(a.discamt,0),ifnull(a.netamt,0),a.um,ifnull(a.status,'N'),ifnull(a.sloc,'N') " &
						  "from tempsodettbl a left join mmasttbl b on a.codeno=b.codeno " &
						  "where a.user = '" & lblUser.Text & "' and  a.itmno = " & CLng(txtItm.Text))
		If Not CBool(dt.Rows.Count) Then
			lblErrMsg.Text = "Item No. not Found..."
		Else
			For Each dr As DataRow In dt.Rows
				txtCodeNo.Text = dr.Item(0).ToString()
				cboMMdesc.Items.Add(dr.Item(1).ToString())
				cboMMdesc.Text = dr.Item(1).ToString()
				txtQty.Text = Format(CDbl(dr.Item(2).ToString()), "#,##0")
				txtWt.Text = Format(CDbl(dr.Item(3).ToString()), "#,##0.00")
				txtSP.Text = Format(CDbl(dr.Item(4).ToString()), "#,##0.00")
				lblAmt.Text = Format(CDbl(dr.Item(5).ToString()), "#,##0.00")
				txtDiscAmt.Text = Format(CDbl(dr.Item(6).ToString()), "#,##0.00")
				lblNetAmt.Text = Format(CDbl(dr.Item(7).ToString()), "#,##0.00")
				lblBillRef.Text = dr.Item(8).ToString()
				If dr.Item(9).ToString() = "Y" Then
					CheckBox3.Checked = True
				Else
					CheckBox3.Checked = False
				End If

				If dr.Item(10).ToString() = "Y" Then
					CheckBox4.Checked = True
				Else
					CheckBox4.Checked = False
				End If

			Next

		End If

		dt.Dispose()

		sql = "delete From tempsodettbl where user = '" & lblUser.Text & "' and  itmno = " & CLng(txtItm.Text)
		ExecuteNonQuery(sql)

		FilldgvSOdet()

		'get pkqt
		dt = GetDataTable("select billref,qtpk,ifnull(wtfr,0),precodeno from mmasttbl where codeno = '" & txtCodeNo.Text & "'")
		If Not CBool(dt.Rows.Count) Then
			lblErrMsg.Text = "Not found."
			Exit Sub

		Else
			For Each dr As DataRow In dt.Rows
				Select Case UCase(dr.Item(3).ToString() & "")
					Case "OTC"
						lblBillRef.Text = dr.Item(0).ToString() & ""
						Dim QtPk As Double = CDbl(dr.Item(1).ToString())
						Dim UnitVol As Double = CDbl(dr.Item(2).ToString())
						lblQtPk.Text = Format(UnitVol / 1000, "#,##0.00")

					Case Else
						lblBillRef.Text = dr.Item(0).ToString() & ""
						lblQtPk.Text = dr.Item(1).ToString() & "" ' for revision

				End Select
			Next

		End If

	End Sub

	Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
		GetMMastAvail()

	End Sub

	Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
		GetMMast()
	End Sub

	Protected Sub CheckBox4_CheckedChanged(sender As Object, e As EventArgs)

		'If Not Me.IsPostBack Then
		'	If CheckBox4.Checked = True Then
		'		CheckBox4.Text = "Compute"
		'	Else
		'		CheckBox4.Text = "As Is"
		'	End If
		'End If
	End Sub

	Private Sub lbReLoadSO_Click(sender As Object, e As EventArgs) Handles lbReLoadSO.Click
		ExecBtnProc()
	End Sub

	Private Sub ExecBtnProc()
		If txtSOno.Text <> "" Then
			'Answer = MessageBox.Show("Do you want to get new A/R balance?, Click No to RELOAD Only", "SO Processing", MessageBoxButtons.YesNo)
			'If Answer = vbYes Then
			'	CheckBox1.Checked = False
			'Else
			'	CheckBox1.Checked = True
			'End If
			'Dim ds As New DataSet()
			DgvSOdet.DataSource = Nothing
			DgvSOdet.DataBind()
			ds.Clear()

			Call ClrHdrDetails()

			dt = GetDataTable("select delstat from sohdrtbl where sono = '" & txtSOno.Text & "'")
			If Not CBool(dt.Rows.Count) Then
				Exit Sub
			Else
				For Each dr As DataRow In dt.Rows
					Dim strDelStat As String
					strDelStat = dr.Item(0).ToString()

					Select Case UCase(strDelStat)
						Case "OPEN", "DR SAVED"
							'Call reLoadSo()ve
							tssDocStat.Text = "EDIT"
							'mdiMain.tsDocStat.Text = vDocStat
							tssDocNo.Text = txtSOno.Text
							lbSave.Enabled = True
							lbDelete.Enabled = True
							btnDel.Enabled = True
							CheckGroupRights()

						Case Else
							lbSave.Enabled = False
							lbDelete.Enabled = False
							btnDel.Enabled = False
							lblErrMsg.Text = "SO No. " & txtSOno.Text & " cannot be edited due to Status: " & strDelStat

					End Select

					Call ReLoadSo()

				Next

			End If

			dt.Dispose()

		End If

		FilldgvSOdet()

		If CheckBox1.Checked = True Then

		End If
		'Call remItmReload()

	End Sub

	Private Sub RemItmReload()

		dt = GetDataTable("select ifnull(a.codename,a.mmdesc) from mmasttbl a left join sodettbl b on a.codeno=b.codeno " &
						  "left join sohdrtbl c on b.sono=c.sono where b.sono = '" & txtSOno.Text & "' group by b.codeno order by a.mmdesc")
		If Not CBool(dt.Rows.Count) Then
			lblErrMsg.Text = "no material found."
			Exit Sub
		Else
			For Each dr As DataRow In dt.Rows
				cboMMdesc.Items.Remove(dr.Item(0).ToString())

			Next
		End If

		Call dt.Dispose()

	End Sub

	Private Sub ReLoadSo()
		Try
			dt = GetDataTable("select ifnull(sysver,'2.1041077') from sohdrtbl where sono = '" & txtSOno.Text & "'")
			If Not CBool(dt.Rows.Count) Then
				lblErrMsg.Text = "SO not found."
				Exit Sub
			Else
				For Each dr As DataRow In dt.Rows
					'AOS100 Web
					Select Case dr.Item(0).ToString()
						Case "AOS100 Web"

						Case Else
							lblErrMsg.Text = "Can't Open Here, Use AOS100 ver 2.1 App"
							lbSave.Enabled = False

					End Select

				Next

			End If

			Call dt.Dispose()

			dt = GetDataTable("select a.transdate,concat(a.smnno,space(1),d.fullname),concat(a.custno,space(1),b.bussname)," &
							  "concat(a.shipto,space(1),c.bussname),a.pono,ifnull(a.currbal,0),ifnull(a.30days,0),ifnull(a.60days,0)," &
							  "ifnull(a.90days,0),ifnull(a.120days,0),ifnull(a.soamt,0),a.status,b.sptype,b.areano,ifnull(b.crlimit,0)," &
							  "b.term,ifnull(c.address,b.address),ifnull(a.91over,0),ifnull(a.deposit,0),ifnull(a.pdc,0)," &
							  "concat(a.plntno,space(1),e.plntname),concat(a.pc,space(1),f.pclass),a.deldate,b.custtype from sohdrtbl a " &
							  "left join custmasttbl b on a.custno=b.custno left join custmasttbl c on a.shipto=c.custno " &
							  "left join smnmtbl d on a.smnno=d.smnno left join plnttbl e on a.plntno=e.plntno " &
							  "left join pctrtbl f on a.pc=f.pc where a.sono = '" & txtSOno.Text & "'")
			If Not CBool(dt.Rows.Count) Then
				lblErrMsg.Text = "SO Not found."
				Exit Sub
			Else
				For Each dr As DataRow In dt.Rows
					dpTransDate.Text = Format(CDate(dr.Item(0).ToString()), "yyyy-MM-dd")
					cboSmnName.Text = dr.Item(1).ToString() & ""
					cboCustName.Text = dr.Item(2).ToString() & ""
					GetShipToNames()
					cboShipTo.Text = dr.Item(3).ToString() & ""
					txtPONo.Text = dr.Item(4).ToString() & ""
					'lblCurr.Text = Format(dr.Item(5).ToString(), "Standard")
					lbl30days.Text = Format(CDbl(dr.Item(6).ToString()), "#,##0.00")
					lbl60days.Text = Format(CDbl(dr.Item(7).ToString()), "#,##0.00")
					lbl90days.Text = Format(CDbl(dr.Item(8).ToString()), "#,##0.00")
					lbl91over.Text = Format(CDbl(dr.Item(9).ToString()), "#,##0.00")
					'lblSOamt.Text = Format(dr.Item(10).ToString(), "Standard")
					lblSOStat.Text = dr.Item(11).ToString()
					lblSPtype.Text = dr.Item(12).ToString()
					lblArea.Text = dr.Item(13).ToString()
					lblCRLimit.Text = Format(CDbl(dr.Item(14).ToString()), "#,##0.00")
					lblTerm.Text = dr.Item(15).ToString()
					lblAddress.Text = dr.Item(16).ToString()
					lbl121over.Text = Format(CDbl(dr.Item(17).ToString()), "#,##0.00")
					lblDepAmt.Text = Format(CDbl(dr.Item(18).ToString()), "#,##0.00")
					lblPDC.Text = Format(CDbl(dr.Item(19).ToString()), "#,##0.00")
					cboPlnt.Text = dr.Item(20).ToString() & ""
					cboPClass.Items.Add(dr.Item(21).ToString())
					cboPClass.Text = dr.Item(21).ToString()
					dpDelDate.Text = Format(CDate(dr.Item(22).ToString()), "yyyy-MM-dd")
					lblCustType.Text = dr.Item(23).ToString()

				Next
			End If

			Call dt.Dispose()

			'pop details temdata
			sql = "delete from tempsodettbl where user ='" & lblUser.Text & "'"
			ExecuteNonQuery(sql)

			sql = "insert into tempsodettbl(itmno,codeno,qty,wt,sp,itmamt,discamt,netamt,um,qtbal,wtbal,status,sloc,user) " &
				  "select itmno,codeno,qty,wt,sp,itmamt,discamt,netamt,um,qtbal,wtbal,status,sloc,'" & lblUser.Text & "' " &
				  "from sodettbl where sono = '" & txtSOno.Text & "'"
			ExecuteNonQuery(sql)

			GetLineItmNo()

			If CheckBox1.Checked = True Then

			Else
				Call ChkNewBals()
			End If

			lbPrint.Enabled = True

		Catch ex As Exception
			MsgBox("Error in loading details", MsgBoxStyle.Exclamation)
			Exit Sub

		End Try

	End Sub

	Protected Sub GetShipToNames()
		cboShipTo.Items.Clear()
		dt = GetDataTable("select concat(custno,space(1),bussname) from custmasttbl where moacctno = '" & cboCustName.Text.Substring(0, 5) & "' " &
						  "order by bussname")
		If Not CBool(dt.Rows.Count) Then
			Exit Sub
		Else
			For Each dr As DataRow In dt.Rows
				cboShipTo.Items.Add(dr.Item(0).ToString())

			Next

		End If

		Call dt.Dispose()

	End Sub

	'Protected Sub OnConfirm2(sender As Object, e As EventArgs)
	'	Dim confirmValue2 As String = Request.Form("confirm_value2")
	'	If confirmValue2 = "Yes" Then
	'		AdmMsgBox("Test")
	'	Else

	'	End If


	'End Sub

	'Protected Sub btnDel_Click(sender As Object, e As ImageClickEventArgs) Handles btnDel.Click
	'	'If IsAllowed(lblGrpUser.text, vThisFormCode, 5) = True Then ' 4 = Delete
	'	'	lbSave.Enabled = True
	'	'	RemoveLineItem()
	'	'Else
	'	'	lbSave.Enabled = False
	'	'	AdmMsgBox("Your are not allowed to DELETE Line Item")
	'	'	Exit Sub
	'	'End If

	'	RemoveLineItem()

	'End Sub

	Protected Sub RemoveLineItem()
		'create confirmation ajax
		If txtItm.Text = "" Then
			lblErrMsg.Text = "Item No. is Blank"
			Exit Sub

		ElseIf txtCodeNo.Text = "" Or txtCodeNo.Text = Nothing Then
			lblErrMsg.Text = "Code No. is Blank"
			Exit Sub

		End If

		If IsNumeric(txtItm.Text) Then

		Else
			lblErrMsg.Text = "Invalid Line Item No."
			Exit Sub

		End If

		dt = GetDataTable("select b.dono from issdettbl a left join isshdrtbl b on a.dono=b.dono " &
						  "where a.codeno = '" & txtCodeNo.Text & "' and b.sono = '" & txtSOno.Text & "' and b.status <> 'void'")
		If Not CBool(dt.Rows.Count) Then

		Else
			For Each dr As DataRow In dt.Rows
				lblErrMsg.Text = "DELETE Not ALLOWED, Already Issued to DR " & dr.Item(0).ToString()

			Next

			Exit Sub

		End If

		dt.Dispose()

		DelRowItemProc()

	End Sub

	Protected Sub DelRowItemProc()
		If txtSOno.Text = "" Or txtSOno.Text = Nothing Then
			sql = "delete from tempsodettbl where itmno = " & CLng(txtItm.Text) & " and user = '" & lblUser.Text & "'"
			ExecuteNonQuery(sql)
			FilldgvSOdet()
		Else
			sql = "delete from sodettbl where itmno = " & CLng(txtItm.Text) & " and sono = '" & lblUser.Text & "'"
			ExecuteNonQuery(sql)

			sql = "delete from tempsodettbl where itmno = " & CLng(txtItm.Text) & " and user = '" & lblUser.Text & "'"
			ExecuteNonQuery(sql)

			FilldgvSOdet()
			UpdateSOHdr()
			ClrLines()

		End If



	End Sub

	Protected Sub lbSearch_Click(sender As Object, e As EventArgs)
		If txtSearch.Text = "" Or txtSearch.Text = Nothing Then
			Exit Sub
		End If

		SearchCustomerProc()

	End Sub

	Protected Sub SearchCustomerProc()
		cboCustName.Items.Clear()
		dt = GetDataTable("select concat(custno,space(1),bussname) from custmasttbl where bussname  like '" & "%" & txtSearch.Text & "%" & "' " &
						  "and accttype ='main' order by bussname")
		If Not CBool(dt.Rows.Count) Then
			Exit Sub
		Else
			For Each dr As DataRow In dt.Rows
				cboCustName.Items.Add(dr.Item(0).ToString())

			Next

		End If

		Call dt.Dispose()

		txtSearch.Text = Nothing

	End Sub

	Private Sub txtSOno_TextChanged(sender As Object, e As EventArgs) Handles txtSOno.TextChanged
		If txtSOno.Text = Nothing Then
			Exit Sub
		End If

		ExecBtnProc()

	End Sub

	Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
		SearchCustomerProc()
	End Sub

	Private Sub cboPClass_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboPClass.SelectedIndexChanged


	End Sub

	Private Sub cboPClass_PreRender(sender As Object, e As EventArgs) Handles cboPClass.PreRender

	End Sub

	Protected Sub btnDel_Click(sender As Object, e As ImageClickEventArgs)

	End Sub
End Class