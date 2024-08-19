Imports MySql.Data.MySqlClient
Imports System
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports MySql.Data

Public Class SalesInvoice
	Inherits System.Web.UI.Page
	Dim dt As DataTable
	Dim sql As String
	Dim sqldata As String
	Dim dt2 As DataTable
	Dim strStat As String
	Dim vProcADM As String
	Dim Answer As String
	Dim MyDA_conn As New MySqlDataAdapter
	Dim MyDataSet As New DataSet
	Dim MySqlScript As String
	Dim amt As Double
	Dim discrate As Double
	Dim discamt As Double
	Dim netamt As Double
	Dim spamt As Double
	Dim admSPstop As String
	Dim admStatType As String
	Dim admUpdateNo As String
	Dim strReport As String
	Dim SalesAcct As String
	Dim DiscAcct As String
	Dim strARacctNo As String
	Dim strCCNo As String

	Private Sub SalesInvoice_Unload(sender As Object, e As EventArgs) Handles Me.Unload
		'AdmMsgBox("Exit")
		tssErrorMsg.Text = "Unload"


	End Sub

	Protected Sub AdmMsgBox(ByVal sMessage As String)
		Dim msg As String
		msg = "<script language='javascript'>"
		msg += "alert('" & sMessage & "');"
		msg += "<" & "/script>"
		Response.Write(msg)
	End Sub

	Private Sub CheckGroupRights()
		If IsAllowed(lblGrpUser.Text, vThisFormCode, 3) = True Then ' 3 = Insert 
			lbSave.Enabled = True
		Else
			lbSave.Enabled = False
		End If

		If IsAllowed(lblGrpUser.Text, vThisFormCode, 2) = True Then ' 2 = EDIT
			lbSave.Enabled = True
		Else
			lbSave.Enabled = False
		End If

		'If IsAllowed(lblGrpUser.text, vThisFormCode, 4) = True Then ' 4 = Delete
		'    VoidToolStripMenuItem.Enabled = True
		'Else
		'    VoidToolStripMenuItem.Enabled = False
		'End If


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

	Protected Sub LoadFormNew()
		Select Case vLoggedBussArea
			Case "8100"
				txtApprvdBy.Text = "LVReyes"

			Case "8200"
				txtApprvdBy.Text = ""

			Case Else
				txtApprvdBy.Text = ""

		End Select

		PopSmnName()
		popPC()

		vEditMode = "New"
		txtSP.ReadOnly = False

	End Sub

	Protected Sub PopSmnName()
		cboSmnName.Items.Clear()
		dt = GetDataTable("select concat(smnno,space(1),fullname) from smnmtbl where status = 'active' " &
						  "and branch = '" & vLoggedBranch & "' order by fullname")
		If Not CBool(dt.Rows.Count) Then
			Exit Sub
		Else
			cboSmnName.Items.Add("")
			For Each dr As DataRow In dt.Rows
				cboSmnName.Items.Add(dr.Item(0).ToString())
			Next
		End If

		dt.Dispose()

	End Sub

	Private Sub popPC()

		cboPC.Items.Clear()
		dt = GetDataTable("select concat(pc,space(1),pclass) from pctrtbl where ba = '" & vLoggedBussArea & "' and " &
						  "tradetype = '" & "trade" & "'")
		If Not CBool(dt.Rows.Count) Then Exit Sub
		For Each dr As DataRow In dt.Rows
			cboPC.Items.Add(dr.Item(0).ToString())
			'cboPC.Text = dr.Item(0).ToString()

		Next

		dt.Dispose()

		'Select Case lblBA.Text
		'	Case "8100", "8200"
		'		If cboPC.Items.Count > 0 Then
		'			cboPC.SelectedIndex = 0

		'		End If

		'End Select

	End Sub

	Private Sub SaveLogs()
		Dim strForm As String = "Sales Invoice"
		sql = "insert into translog(trans,form,datetimelog,user,docno,tc)values" &
			  "('" & strReport & "','" & strForm & "','" & Format(CDate(Now), "yyyy-MM-dd hh:mm:ss") & "'," &
			  "'" & lblUser.Text & "','" & txtInvNo.Text & "','" & lblTC.Text & "')"
		ExecuteNonQuery(sql)

	End Sub

	Private Sub getCustListSmn()
		Try
			cboCustName.Items.Clear()
			dt = GetDataTable("select concat(custno,space,bussname) from custmasttbl where accttype = 'main' " &
							  "and smnno = '" & cboSmnName.Text.Substring(0, 3) & "' order by bussname")
			If Not CBool(dt.Rows.Count) Then
				tssErrorMsg.Text = "No Customer found."
				Exit Sub
			Else
				For Each dr As DataRow In dt.Rows
					cboCustName.Items.Add(dr.Item(0).ToString())
				Next

			End If

			Call dt.Dispose()

		Catch ex As Exception

		End Try

	End Sub

	Private Sub getMMstocks()
		Try
			cboMMdesc.Items.Clear()
			dt = GetDataTable("select mmdesc from mmasttbl where pc = '" & cboPC.Text.Substring(0, 1) & "' order by mmdesc")
			If Not CBool(dt.Rows.Count) Then
				Exit Sub
			Else
				For Each dr As DataRow In dt.Rows
					cboMMdesc.Items.Add(dr.Item(0).ToString())

				Next

			End If

			dt.Dispose()

		Catch ex As Exception
			tssErrorMsg.Text = ErrorToString()
		End Try

	End Sub

	Protected Sub lbSave_Click(sender As Object, e As EventArgs)
		Select Case vLoggedBussArea
			Case "8100", "8200"
				If cboPC.Items.Count > 0 Then
					cboPC.SelectedIndex = 0

				End If
		End Select

		If txtRefNo.Text = "" Then
			tssErrorMsg.Text = "Actual SI No. is Blank"
			Exit Sub
		End If

		If cboSmnName.Text = "" Then
			cboSmnName.Focus()
			tssErrorMsg.Text = "Select Salesman"
			Exit Sub

		ElseIf cboCustName.Text = "" Then
			cboCustName.Focus()
			tssErrorMsg.Text = "Select Customer"
			Exit Sub

		ElseIf DgvSalesdet.Rows.Count = 0 Then
			tssErrorMsg.Text = "No line item yet"
			Exit Sub

			'ElseIf (CDbl(IIf(lblTotNetAmt.Text = "", 0, lblTotNetAmt.Text)) - CDbl(IIf(lblTotVatAmt.Text = "", 0, lblTotVatAmt.Text))) <> CDbl(IIf(lblGrossAmt.Text = "", 0, lblGrossAmt.Text)) Then
			'    MsgBox("Header Amount is not Equal to Detailed Amount", MsgBoxStyle.Information)
			'    Exit Sub

		ElseIf cboPC.Text = "" Then
			tssErrorMsg.Text = "PC is Blank"
			Exit Sub

		End If

		Dim DetAmt As Double = Math.Round((CDbl(IIf(lblTotNetAmt.Text = "", 0, lblTotNetAmt.Text)) - CDbl(IIf(lblTotVatAmt.Text = "", 0, lblTotVatAmt.Text))), 2)
		Dim GrossAmt As Double = Math.Round(CDbl(IIf(lblGrossAmt.Text = "", 0, lblGrossAmt.Text)), 2)

		'MsgBox(DetAmt & " - " & GrossAmt)

		If DetAmt <> GrossAmt Then
			tssErrorMsg.Text = "Header Amount is not Equal to Detailed Amount"
			Exit Sub

		End If

		Dim Tgross As Double = Math.Round(CDbl(IIf(lblTotGross.Text = "", 0, lblTotGross.Text)), 2) - Math.Round(CDbl(IIf(lblTotDisc.Text = "", 0, lblTotDisc.Text)), 2)
		Dim Tnet As Double = Math.Round(CDbl(IIf(lblTotNetAmt.Text = "", 0, lblTotNetAmt.Text)), 2)

		If Tgross <> Tnet Then
			tssErrorMsg.Text = "Check Details"
			Exit Sub
		End If

		If tssDocStat.Text = "New" Then
			If txtInvNo.Text = "" Then
				Call getDocNo()
				tssErrorMsg.Text = "Getting New SI No."

			End If

		Else
			If tsDocNoEdit.Text <> txtInvNo.Text Then
				MsgBox("Doc. No. should not be CHANGED")
				txtInvNo.Text = tsDocNoEdit.Text

			End If

		End If

		If txtInvNo.Text = "" Then
			tssErrorMsg.Text = "Inv. No. is Blank"
			Exit Sub

		ElseIf Len(txtInvNo.Text) <> 8 Then
			tssErrorMsg.Text = "Check Inv. No."
			Exit Sub
		End If


		'check inv no
		dt = GetDataTable("select invno from saleshdrtbl where docno = '" & txtRefNo.Text & "' and invno <> '" & txtInvNo.Text & "' " &
						  "and status <> 'void' limit 1")
		If Not CBool(dt.Rows.Count) Then
			'UpdateRefNoSI()
		Else
			For Each dr As DataRow In dt.Rows
				tssErrorMsg.Text = "Invoice No.:" & txtRefNo.Text & " already used in AOS Doc. No.:" & dr.Item(0).ToString() & ", Not Saved"
				Exit Sub

			Next

		End If

		Call dt.Dispose()

		gRepTbox(txtRemarks)

		If IsNumeric(txtInvNo.Text) Then
			admUpdateNo = Format(CDate(Now()), "yyyyMMddHHmmss")
			Call SaveHdrProc()

		Else
			tssErrorMsg.Text = "Invalid Character in Inv No."
			txtInvNo.Text = "'"
			Exit Sub

		End If

		gRepTboxUndo(txtRemarks)

		tssErrorMsg.Text = " No.: " & txtInvNo.Text & " / DR No. " & txtDONo.Text

		'sql = "update saleshdrtbl set otherbadoc = '" & tssMMRRNo.Text & "' where invno = '" & txtInvNo.Text & "'"
		'ExecuteNonQuery(sql)

		vEditMode = "Edit"
		admStatType = Nothing
		'
	End Sub

	Private Sub SaveHdrProc()
		dt = GetDataTable("select status from saleshdrtbl where invno = '" & txtInvNo.Text & "'")
		If Not CBool(dt.Rows.Count) Then
			Call SaveHdr()
			strReport = "Save Header"

		Else
			For Each dr As DataRow In dt.Rows
				Select Case UCase(dr.Item(0).ToString())
					Case "VOID"
						tssErrorMsg.Text = "Invalid Invoice No., Invoice Already VOID"
						Exit Sub

					Case Else
						If admStatType = "Initial" Then

						Else
							admStatType = "Update"
							SaveOldDataHdr()

						End If

						UpdateHdr()
						strReport = "Update Header"

				End Select

			Next

		End If

		dt.Dispose()

		'Call SaveLogs()
		sql = "update isshdrtbl set invdate = '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' where " &
			  "dono = '" & txtDONo.Text & "'"
		ExecuteNonQuery(sql)

		If CheckBox2.Checked = True Then
			sql = "update custmasttbl set vat = 'V' where custno = '" & cboCustName.Text.Substring(0, 5) & "'"
			ExecuteNonQuery(sql)

		End If

		sql = "update saleshdrtbl set sysver = 'AOS100 Web' where invno = '" & txtInvNo.Text & "'"
		ExecuteNonQuery(sql)

		SaveDetProc()
		GLEntryProc()

		tsDocNoEdit.Text = txtInvNo.Text

		strStat = "SAVE"
		LogMeIn()



	End Sub

	Private Sub SaveOldDataHdr()
		sql = "insert into saleshdrtbl_update(updateid,invno,tc,transdate,custno,smnno,dsrno,grossamt,discamt,fhamt," &
			  "netamt,dono,remarks,status,glstat,user,postdate,pc,vat,docno,dsrstat,shipto,branch,doctype,otherbadoc," &
			  "updateby,postupdate) select '" & admUpdateNo & "',invno,tc,transdate,custno,smnno,dsrno,grossamt,discamt,fhamt," &
			  "netamt,dono,remarks,status,glstat,user,postdate,pc,vat,docno,dsrstat,shipto,branch,doctype,otherbadoc," &
			  "'" & lblUser.Text & "','" & Format(CDate(Now), "yyyy-MM-dd hh:mm:ss") & "' from saleshdrtbl where invno = '" & txtInvNo.Text & "'"
		ExecuteNonQuery(sql)

	End Sub

	Private Sub UpdateHdr()
		Dim strStat As String = "OPEN"
		sql = "update saleshdrtbl set transdate = '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "'," &
			  "custno = '" & cboCustName.Text.Substring(0, 5) & "',smnno = '" & cboSmnName.Text.Substring(0, 3) & "'," &
			  "dsrno = '" & lblDSRNo.Text & "',grossamt = " & CDbl(IIf(lblGrossAmt.Text = "", 0, lblGrossAmt.Text)) & "," &
			  "discamt = " & CDbl(IIf(txtDeductn.Text = "", 0, txtDeductn.Text)) & ",fhamt = " & CDbl(IIf(txtFHamt.Text = "", 0, txtFHamt.Text)) & "," &
			  "netamt = " & CDbl(IIf(lblNetAmt.Text = "", 0, lblNetAmt.Text)) & ",dono = '" & txtDONo.Text & "',remarks = '" & txtRemarks.Text & "'," &
			  "status = '" & strStat & "',glstat = '" & strStat & "',updateby = '" & lblUser.Text & "',postupdate = '" & Format(CDate(Now), "yyyy-MM-dd hh:mm:ss") & "'," &
			  "pc = '" & cboPC.Text.Substring(0, 1) & "',vat = " & CDbl(IIf(lblTaxes.Text = "", 0, lblTaxes.Text)) & ",docno = '" & txtRefNo.Text & "',dsrstat = '" & strStat & "'," &
			  "shipto = '" & cboShipTo.Text.Substring(0, 5) & "',branch = '" & vLoggedBranch & "',doctype = 'CI' " &
			  "where invno = '" & txtInvNo.Text & "'"
		ExecuteNonQuery(sql)

	End Sub

	Private Sub SaveHdr()

		Dim strStat As String = "OPEN"
		sql = "insert into saleshdrtbl(invno,tc,transdate,custno,smnno,dsrno,grossamt,discamt,fhamt," &
			  "netamt,dono,remarks,status,glstat,user,postdate,pc,vat,docno,dsrstat,shipto,branch,doctype)values" &
			  "('" & txtInvNo.Text & "','" & lblTC.Text & "','" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "'," &
			  "'" & cboCustName.Text.Substring(0, 5) & "','" & cboSmnName.Text.Substring(0, 3) & "','" & lblDSRNo.Text & "'," &
			  CDbl(IIf(lblGrossAmt.Text = "", 0, lblGrossAmt.Text)) & "," & CDbl(IIf(txtDeductn.Text = "", 0, txtDeductn.Text)) & "," &
			  CDbl(IIf(txtFHamt.Text = "", 0, txtFHamt.Text)) & "," & CDbl(IIf(lblNetAmt.Text = "", 0, lblNetAmt.Text)) & "," &
			  "'" & txtDONo.Text & "','" & txtRemarks.Text & "','" & strStat & "','" & strStat & "'," &
			  "'" & lblUser.Text & "','" & Format(CDate(Now), "yyyy-MM-dd hh:mm:ss") & "','" & cboPC.Text.Substring(0, 1) & "'," &
			  CDbl(IIf(lblTaxes.Text = "", 0, lblTaxes.Text)) & ",'" & txtRefNo.Text & "','" & strStat & "'," &
			  "'" & cboShipTo.Text.Substring(0, 5) & "','" & vLoggedBranch & "','CI')"
		ExecuteNonQuery(sql)


		'balance SO vs DO

	End Sub

	Private Sub GLEntryProc()
		Dim strPtype As String = "POST"
		Dim strSourceDoc As String = "Sales"

		dt = GetDataTable("select * from gltranstbl where docno = '" & txtInvNo.Text & "' and jvsource = '" & strSourceDoc & "' " &
						  "and posttype = '" & strPtype & "'")
		If Not CBool(dt.Rows.Count) Then
			Call GLEntry()
			'strReport = "gl entries"

		Else
			sql = "delete from gltranstbl where docno = '" & txtInvNo.Text & "' and jvsource = '" & strSourceDoc & "' " &
				  "and posttype = '" & strPtype & "'"
			ExecuteNonQuery(sql)

			Call GLEntry()
			'strReport = "Update GL Entries"

		End If

		'Call SaveLogs()

	End Sub

	Private Sub GLEntry()

		Dim strPtype As String = "POST"
		Dim strSourceDoc As String = "Sales"

		dt = GetDataTable("select acctno,gldisc from pctrtbl where pc = '" & cboPC.Text.Substring(0, 1) & "'")
		If Not CBool(dt.Rows.Count) Then Exit Sub

		For Each dr As DataRow In dt.Rows
			SalesAcct = dr.Item(0).ToString()
			DiscAcct = dr.Item(1).ToString()

		Next

		'Call dt.Dispose()

		If vLoggedBussArea = lblBAto.Text Then
			strARacctNo = "1110210"

		Else
			strARacctNo = "1110220"

		End If

		dt = GetDataTable("select nonexps from batbl where ba = '" & vLoggedBussArea & "'")
		If Not CBool(dt.Rows.Count) Then
			tssErrorMsg.Text = "BA Not found."
			Exit Sub
		Else
			For Each dr As DataRow In dt.Rows
				strCCNo = dr.Item(0).ToString() & ""
				'DR a/r net Sales

			Next
		End If

		dt.Dispose()

		sql = "insert into gltranstbl(acctno,dramt,cramt,ccno,transyear,transmon,subacct," &
			  "user,jvsource,glstatus,dateposted,transdate,docno,tc,pc,pk,posttype,branch,bato,branchto)values" &
			  "('" & strARacctNo & "'," & CDbl(IIf(lblNetAmt.Text = "", 0, lblNetAmt.Text)) & "," &
			  CDbl(0) & ",'" & strCCNo & "','" & Format(CDate(dpTransDate.Text), "yyyy") & "'," &
			  "'" & Format(CDate(dpTransDate.Text), "MM") & "','" & cboCustName.Text.Substring(0, 5) & "'," &
			  "'" & lblUser.Text & "','" & strSourceDoc & "','" & "open" & "','" & Format(CDate(Now), "yyyy-MM-dd hh:mm:ss") & "'," &
			  "'" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "','" & txtInvNo.Text & "'," &
			  "'" & lblTC.Text & "','" & cboPC.Text.Substring(0, 1) & "','09','" & strPtype & "'," &
			  "'" & vLoggedBranch & "','" & lblBAto.Text & "','" & lblBranchTo.Text & "')"
		ExecuteNonQuery(sql)

		'dr disc
		Dim deductAmt As Double = CDbl(IIf(txtDeductn.Text = "", 0, txtDeductn.Text))
		If deductAmt > 0 Then
			sql = "insert into gltranstbl(acctno,dramt,cramt,ccno,transyear,transmon," &
				  "user,jvsource,glstatus,dateposted,transdate,docno,tc,pc,pk,posttype,branch,bato,branchto)values" &
				  "('" & DiscAcct & "'," & CDbl(IIf(txtDeductn.Text = "", 0, txtDeductn.Text)) & "," &
				  CDbl(0) & ",'" & strCCNo & "','" & Format(CDate(dpTransDate.Text), "yyyy") & "'," &
				  "'" & Format(CDate(dpTransDate.Text), "MM") & "'," &
				  "'" & lblUser.Text & "','" & strSourceDoc & "','" & "open" & "'," &
				  "'" & Format(CDate(Now), "yyyy-MM-dd hh:mm:ss") & "'," &
				  "'" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "','" & txtInvNo.Text & "'," &
				  "'" & lblTC.Text & "','" & cboPC.Text.Substring(0, 1) & "','40','" & strPtype & "'," &
				  "'" & vLoggedBranch & "','" & lblBAto.Text & "','" & lblBranchTo.Text & "')"
			ExecuteNonQuery(sql)

		End If

		'cr addl
		Dim FMamt As Double = CDbl(IIf(txtFHamt.Text = "", 0, txtFHamt.Text))
		If FMamt > 0 Then
			sql = "insert into gltranstbl(acctno,dramt,cramt,ccno,transyear,transmon," &
				  "user,jvsource,glstatus,dateposted,transdate,docno,tc,pc,pk,posttype,branch,bato,branchto) select " &
				  "'" & "1230130" & "'," & CDbl(0) & "," & CDbl(IIf(txtFHamt.Text = "", 0, txtFHamt.Text)) & "," &
				  "ccno,'" & Format(CDate(dpTransDate.Text), "yyyy") & "'," &
				  "'" & Format(CDate(dpTransDate.Text), "MM") & "','" & lblUser.Text & "','" & strSourceDoc & "'," &
				  "'" & "open" & "','" & Format(CDate(Now), "yyyy-MM-dd hh:mm:ss") & "'," &
				  "'" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "'," &
				  "'" & txtInvNo.Text & "','" & lblTC.Text & "','" & cboPC.Text.Substring(0, 1) & "','50'," &
				  "'" & strPtype & "','" & vLoggedBranch & "','" & lblBAto.Text & "','" & lblBranchTo.Text & "' from " &
				  "pctrtbl where pc = '" & cboPC.Text.Substring(0, 1) & "'"
			ExecuteNonQuery(sql) ',subacct

		End If

		'cr vat 1210260
		Dim taxamt As Double = CDbl(IIf(lblTaxes.Text = "", 0, lblTaxes.Text))
		If taxamt > 0 Then
			sql = "insert into gltranstbl(acctno,dramt,cramt,ccno,transyear,transmon,subacct,user,jvsource,glstatus,dateposted,transdate," &
				  "docno,tc,pc,pk,posttype,branch,bato,branchto) select taxacct," & CDbl(0) & "," & CDbl(IIf(lblTaxes.Text = "", 0, lblTaxes.Text)) & "," &
				  "'" & strCCNo & "','" & Format(CDate(dpTransDate.Text), "yyyy") & "','" & Format(CDate(dpTransDate.Text), "MM") & "'," &
				  "'" & cboCustName.Text.Substring(0, 5) & "','" & lblUser.Text & "','" & strSourceDoc & "','" & "open" & "','" & Format(CDate(Now), "yyyy-MM-dd hh:mm:ss") & "'," &
				  "'" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "','" & txtInvNo.Text & "','" & lblTC.Text & "'," &
				  "'" & cboPC.Text.Substring(0, 1) & "','40','" & strPtype & "','" & vLoggedBranch & "','" & lblBAto.Text & "'," &
				  "'" & lblBranchTo.Text & "' from tccodestbl where tc = '" & lblTC.Text & "'"
			ExecuteNonQuery(sql)

		End If

		'cr sales
		sql = "insert into gltranstbl(acctno,dramt,cramt,ccno,transyear,transmon,user,jvsource,glstatus,dateposted,transdate," &
			  "docno,tc,pc,pk,posttype,branch,bato,branchto) select '" & SalesAcct & "'," & CDbl(0) & "," &
			  CDbl(IIf(lblGrossAmt.Text = "", 0, lblGrossAmt.Text)) & ",ccno,'" & Format(CDate(dpTransDate.Text), "yyyy") & "'," &
			  "'" & Format(CDate(dpTransDate.Text), "MM") & "','" & lblUser.Text & "','" & strSourceDoc & "'," &
			  "'" & "open" & "','" & Format(CDate(Now), "yyyy-MM-dd hh:mm:ss") & "','" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "'," &
			  "'" & txtInvNo.Text & "','" & lblTC.Text & "','" & cboPC.Text.Substring(0, 1) & "','40','" & strPtype & "'," &
			  "'" & vLoggedBranch & "','" & lblBAto.Text & "','" & lblBranchTo.Text & "' from pctrtbl where " &
			  "pc = '" & cboPC.Text.Substring(0, 1) & "'"
		ExecuteNonQuery(sql)

		'Call dt.Dispose()

	End Sub

	Private Sub SaveDetProc()
		Try
			dt = GetDataTable("select * from saleshdrtbl where invno = '" & txtInvNo.Text & "'")
			If Not CBool(dt.Rows.Count) Then
				Call SaveHdr()
				'tssMsg.Text = "Saving Header"

			End If

			dt = GetDataTable("select * from salesdettbl where invno = '" & txtInvNo.Text & "'")
			If Not CBool(dt.Rows.Count) Then
				Call SaveDet()
				tssErrorMsg.Text = "Invoice No. " & txtInvNo.Text & " Saved"

			Else
				SaveOldDataDet()

				sql = "delete from salesdettbl where invno = '" & txtInvNo.Text & "'"
				ExecuteNonQuery(sql)

				Call SaveDet()
				tssErrorMsg.Text = "Invoice No. " & txtInvNo.Text & " Update"
				'strReport = txtItm.Text & Space(1) & "REPLACE"


			End If

		Catch ex As Exception
			tssErrorMsg.Text = ErrorToString()

		End Try

	End Sub

	Private Sub SaveOldDataDet()
		sql = "insert into salesdettbl_update(updateid,invno,codeno,qty,wt,sp,detgrossamt,detdiscamt,itmamt,itemno," &
			  "dsrno,billref,transdate,detvat,status,nv) select '" & admUpdateNo & "',invno,codeno,qty,wt,sp,detgrossamt," &
			  "detdiscamt,itmamt,itemno,dsrno,billref,transdate,detvat,status,nv from salesdettbl where " &
			  "invno = '" & txtInvNo.Text & "'"
		ExecuteNonQuery(sql)

	End Sub

	Private Sub SaveDet()
		sql = "insert into salesdettbl(invno,codeno,qty,wt,sp,detgrossamt,detdiscamt,itmamt,itemno," &
			  "dsrno,billref,transdate,detvat,status,nv,dealstat) select '" & txtInvNo.Text & "'," &
			  "codeno,qty,wt,sp,detgrossamt,detdiscamt,itmamt,itemno,dsrno,billref,transdate,detvat," &
			  "status,nv,dsrstat from tempsalesdettbl2 where user = '" & lblUser.Text & "'"
		ExecuteNonQuery(sql)

		tssErrorMsg.Text = "Details Saved"

		If txtDONo.Text <> "" Then
			tssErrorMsg.Text = "Getting DO Balance Status"

			tssErrorMsg.Text = "Updating DO Status"

			sql = "update isshdrtbl set dsrno = '" & txtInvNo.Text & "',dsrstat = 'SERVED'," &
				  "salesdoc = 'CI' where dono = '" & txtDONo.Text & "'"
			ExecuteNonQuery(sql)

		End If

		tssErrorMsg.Text = ""

	End Sub


	Private Sub getDocNo()
		dt = GetDataTable("select max(invno) from saleshdrtbl where user <> 'Admin' and invno between '10000000' and '19999999' order by invno")
		If Not CBool(dt.Rows.Count) Then
			txtInvNo.Text = "10000001"

		Else
			For Each dr As DataRow In dt.Rows
				txtInvNo.Text = Format(CLng(dr.Item(0).ToString()) + 1, "#00000000")

			Next

		End If

		Call dt.Dispose()

	End Sub

	Protected Sub lbClose_Click(sender As Object, e As EventArgs)
		If tsSaveStat.Text <> "Saved" Then

		Else
			Response.Redirect("SalesAndDist.aspx")
		End If

	End Sub

	Protected Sub lbPrint_Click(sender As Object, e As EventArgs)

	End Sub

	Protected Sub DgvSalesdet_RowDataBound(sender As Object, e As GridViewRowEventArgs)

	End Sub

	Protected Sub DgvSalesdet_RowCreated(sender As Object, e As GridViewRowEventArgs)

	End Sub

	Private Sub SalesInvoice_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete

	End Sub

	Private Sub NewLoadProc()
		tsDocNoEdit.Text = Nothing

		vThisFormCode = "019"
		Call CheckGroupRights()

		vJVsource = "Sales"
		tssVoidReqNo.Text = ""

		tssDocStat.Text = "New"
		LoadFormNew()

		dpTransDate.Text = Format(CDate(Now()), "yyyy-MM-dd")

	End Sub

	Protected Sub OnConfirm(sender As Object, e As EventArgs)
		If txtInvNo.Text = Nothing Or txtInvNo.Text = "" Then
			NewLoadProc()
			clrControls()
			ClrLines()
		Else
			Dim confirmValue As String = Request.Form("confirm_value")
			If confirmValue = "Yes" Then
				clrControls()
				ClrLines()
			Else
				tssErrorMsg.Text = "Action Aborted"
			End If

		End If

	End Sub

	Protected Sub OnConfirm3(sender As Object, e As EventArgs)
		Dim confirmValue As String = Request.Form("confirm_value")
		If confirmValue = "Yes" Then
			dt = GetDataTable("select ifnull(jvno,'No JV') as adm from gltranstbl where docno = '" & txtInvNo.Text & "' " &
				  "and tc = '" & lblTC.Text & "' group by adm")
			If Not CBool(dt.Rows.Count) Then

			Else
				For Each dr As DataRow In dt.Rows
					Select Case dr.Item(0).ToString()
						Case "No JV"

						Case Else
							tssErrorMsg.Text = "VOID Not Possible, due to GL Transaction already Process: JV No. " & dr.Item(0).ToString()
							Exit Sub
					End Select

				Next

			End If

			dt.Dispose()

			If IsAllowed(lblGrpUser.Text, vThisFormCode, 4) = True Then
				'VOID INVHDR
				admUpdateNo = Format(CDate(Now()), "yyyyMMddHHmmss")
				admStatType = "Void"
				SaveOldDataHdr()

				sql = "update saleshdrtbl set status = 'void',docno = null where invno = '" & txtInvNo.Text & "'"
				ExecuteNonQuery(sql)

				'void invdet
				sql = "delete from salesdettbl where invno = '" & txtInvNo.Text & "'"
				ExecuteNonQuery(sql)

				'reset or ref
				sql = "update coldettbl set refno = orno, tc = '60' where refno = '" & txtInvNo.Text & "' and " &
						  "tc = '" & lblTC.Text & "'"
				ExecuteNonQuery(sql)

				'reset do
				sql = "update isshdrtbl set dsrno = '00000', dsrstat = 'open' where dsrno = '" & txtInvNo.Text & "'"
				ExecuteNonQuery(sql)

				'GL ENTRY
				sql = "delete from gltranstbl where docno = '" & txtInvNo.Text & "' and tc = '" & lblTC.Text & "' " &
						  "and jvsource = 'Sales'"
				ExecuteNonQuery(sql)

				AdmMsgBox("Successfully VOID")
				'insert log
				strReport = "VOID "
				Call SaveLogs()

			Else
				If tssVoidReqNo.Text <> "" Then
					tssErrorMsg.Text = "Void Request already DONE"
					Exit Sub
				End If

				dt = GetDataTable("select vdocno,reqstat from voidreqtbl where docno = '" & txtInvNo.Text & "' " &
								  "and tc = '" & lblTC.Text & "'")
				If Not CBool(dt.Rows.Count) Then

				Else
					For Each dr As DataRow In dt.Rows
						tssErrorMsg.Text = "Doc No.: " & txtInvNo.Text & " and TC:" & lblTC.Text & " already Request with Request No. " & dr.Item(0).ToString() &
									  " Status:" & dr.Item(1).ToString()
						tssVoidReqNo.Text = dr.Item(0).ToString()
						Exit Sub
					Next

				End If

				dt.Dispose()

				If tssVoidReqNo.Text <> "" Then
					tssErrorMsg.Text = "Void Request already DONE"
					Exit Sub
				End If

				'Save to Request
				'
				dt = GetDataTable("select vdocno from voidreqtbl where vdocno between '4100000001' and '4199999999' order by vdocno")
				If Not CBool(dt.Rows.Count) Then
					tssVoidReqNo.Text = "4100000001"

				Else
					For Each dr As DataRow In dt.Rows
						tssVoidReqNo.Text = Format(CLng(dr.Item(0).ToString()) + 1, "#0000000000")

					Next

				End If

				Call dt.Dispose()

				dt = GetDataTable("select docno,tc from voidreqtbl where vdocno = '" & tssVoidReqNo.Text & "'")
				If Not CBool(dt.Rows.Count) Then
					sql = "insert into voidreqtbl(vdocno,docno,tc,user,transdate,pdate,reqstat,remarks)values" &
						  "('" & tssVoidReqNo.Text & "','" & txtInvNo.Text & "','" & lblTC.Text & "','" & lblUser.Text & "'," &
						  "'" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "','" & Format(CDate(Now()), "yyyy-MM-dd HH:mm:ss") & "'," &
						  "'PARK','Request from Billing Module')"
					ExecuteNonQuery(sql)

					tssErrorMsg.Text = "Void Request PARK"


				Else
					For Each dr As DataRow In dt.Rows
						tssErrorMsg.Text = "Trans ID already Used in Doc. No.: " & dr.Item(0).ToString() & " / TC: " & dr.Item(1).ToString()
					Next
				End If

				dt.Dispose()
				strReport = "VOID Requested "
				Call SaveLogs()

			End If

		Else
			tssErrorMsg.Text = "Action Aborted"
		End If


	End Sub

	Private Sub clrControls()
		LogMeOut()
		cboSmnName.Enabled = True
		cboCustName.Enabled = True
		cboShipTo.Enabled = True

		txtInvNo.Text = ""
		txtDONo.ReadOnly = False
		txtDONo.Text = ""
		txtRefNo.Text = ""
		txtRemarks.Text = ""

		cboCustName.Items.Clear()
		cboCustName.Text = ""
		cboShipTo.Items.Clear()
		cboShipTo.Text = ""

		txtItm.Text = "1"

		lblArea.Text = "000"
		lblTerm.Text = "00"
		lblSPtype.Text = "Area"
		lblGrossAmt.Text = "0.00"
		lblTaxes.Text = "0.00"
		lblNetAmt.Text = "0.00"
		lblTotQty.Text = "0"
		lblTotWt.Text = "0.00"
		lblTotGross.Text = "0.00"
		lblTotDisc.Text = "0.00"
		lblTotNetAmt.Text = "0.00"
		lblTotVatAmt.Text = "0.00"
		'tssMMRRNo.Text = ""

		DgvSalesdet.DataSource = Nothing
		DgvSalesdet.DataBind()

		'Call lvSalesDet.Items.Clear()

		'lblDSRNo.Text = "00000"
		tssDocStat.Text = "New"
		'mdiMain.tsDocStat.Text = tssDocStat.text
		tssVoidReqNo.Text = ""

	End Sub

	Protected Sub DgvDOList_RowDataBound(sender As Object, e As GridViewRowEventArgs)

	End Sub

	Protected Sub DgvDOList_RowCreated(sender As Object, e As GridViewRowEventArgs)

	End Sub

	Protected Sub DgvDOdetList_RowDataBound(sender As Object, e As GridViewRowEventArgs)

	End Sub

	Protected Sub DgvDOdetList_RowCreated(sender As Object, e As GridViewRowEventArgs)

	End Sub

	Private Sub txtCodeNo_TextChanged(sender As Object, e As EventArgs) Handles txtCodeNo.TextChanged
		If txtCodeNo.Text = "" Then
			tssErrorMsg.Text = "Code is Blank"
			txtCodeNo.Focus()
			Exit Sub

		End If

		Call SPcheck()

	End Sub

	Private Sub GetSOprice()
		dt = GetDataTable("select ifnull(a.sp,0) from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
						  "left join isshdrtbl c on b.sono = c.sono where a.codeno = '" & txtCodeNo.Text & "' " &
						  "and c.dono = '" & txtDONo.Text & "'")
		If Not CBool(dt.Rows.Count) Then
			tssErrorMsg.Text = "No SP for material " & txtCodeNo.Text & " Found"
			Exit Sub
		Else
			For Each dr As DataRow In dt.Rows
				spamt = CDbl(dr.Item(0).ToString())
				txtSP.Text = Format(spamt, "#,##0.0000")
				txtQty.Focus()
			Next

		End If

		'Call dt.Dispose()

	End Sub

	Private Sub GetLastPrice()
		dt = GetDataTable("select ifnull(sp,0) from salesdettbl a left join saleshdrtbl b on a.invno=b.invno where " &
						  "a.codeno = '" & txtCodeNo.Text & "' and b.custno = '" & cboShipTo.Text.Substring(0, 5) & "' " &
						  "and b.status <> 'VOID' and b.transdate <= '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
						  "order by b.transdate desc limit 1")
		If Not CBool(dt.Rows.Count) Then
			tssErrorMsg.Text = "No SP for material " & txtCodeNo.Text & " Found"
			txtQty.Focus()
			Exit Sub
		Else
			For Each dr As DataRow In dt.Rows
				spamt = CDbl(dr.Item(0).ToString())
				txtSP.Text = Format(spamt, "#,##0.0000")
				txtQty.Focus()
			Next

		End If

		'Call dt.Dispose()

	End Sub

	Private Sub SPcheck()
		Try
			If CheckBox3.Checked = True Then
				GetLastPrice()
			Else
				Select Case lblSPtype.Text
					Case "Customer"
						dt = GetDataTable("select ifnull(currprc,0),ifnull(drate,0) from plsttbl where " &
										  "codeno = '" & txtCodeNo.Text & "' and custno = '" & cboShipTo.Text.Substring(0, 5) & "' " &
										  "and '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
										  "between effdate and prvdate")
						If Not CBool(dt.Rows.Count) Then
							GetSOprice()
						Else
							For Each dr As DataRow In dt.Rows
								spamt = CDbl(dr.Item(0).ToString())
								txtSP.Text = Format(spamt, "#,##0.0000")
								txtDiscRate.Text = Format(CDbl(dr.Item(1).ToString()), "#,##0.00")
								txtQty.Focus()
							Next

						End If

						Call dt.Dispose()

					Case Else
						dt = GetDataTable("select ifnull(currprc,0) from plsttbl where codeno = '" & txtCodeNo.Text & "' and " &
										  "areano = '" & lblArea.Text & "' and '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
										  "between effdate and prvdate")
						If Not CBool(dt.Rows.Count) Then
							GetSOprice()
						Else
							For Each dr As DataRow In dt.Rows
								spamt = CDbl(dr.Item(0).ToString())
								txtSP.Text = Format(spamt, "#,##0.0000")
								txtQty.Focus()
							Next

						End If

						Call dt.Dispose()

				End Select
			End If

		Catch ex As Exception

		End Try

	End Sub

	Private Sub cboCustName_TextChanged(sender As Object, e As EventArgs) Handles cboCustName.TextChanged
		dt = GetDataTable("select sptype,areano,custtype,bato,branchto,ifnull(vat,'NV') from custmasttbl where " &
						  "custno = '" & cboCustName.Text.Substring(0, 5) & "'")
		If Not CBool(dt.Rows.Count) Then
			tssErrorMsg.Text = "Customer Not found."
			Exit Sub
		Else
			For Each dr As DataRow In dt.Rows
				lblSPtype.Text = dr.Item(0).ToString() & ""
				lblArea.Text = dr.Item(1).ToString() & ""
				lblCustType.Text = dr.Item(2).ToString() & ""
				lblBAto.Text = dr.Item(3).ToString() & ""
				lblBranchTo.Text = dr.Item(4).ToString() & ""
				lblCustVat.Text = dr.Item(5).ToString()
				If lblCustVat.Text = "V" Then
					CheckBox2.Checked = True

				Else
					CheckBox2.Checked = False

				End If

			Next
		End If

		Call dt.Dispose()

		cboShipTo.Items.Clear()

		dt = GetDataTable("select concat(custno,space(1),bussname) from custmasttbl where moacctno = '" & cboCustName.Text.Substring(0, 5) & "' " &
						  "order by bussname")
		If Not CBool(dt.Rows.Count) Then
			tssErrorMsg.Text = "Shipt To Not found."
			Exit Sub
		Else
			For Each dr As DataRow In dt.Rows
				cboShipTo.Items.Add(dr.Item(0).ToString())

			Next

		End If

		Call dt.Dispose()

	End Sub

	Private Sub cboMMdesc_TextChanged(sender As Object, e As EventArgs) Handles cboMMdesc.TextChanged
		If cboMMdesc.Text = "" Then
			txtCodeNo.Text = ""
			Exit Sub

			'ElseIf cboPC.Text = "" Then
			'    'cboPC.Focus()
			'    MsgBox("Select Product Class")
			'    Exit Sub

		End If

		CheckMMprofile()

		If admSPstop = "Stop" Then
			Exit Sub
		Else
			Call SPcheck()
		End If

		txtQty.Focus()

	End Sub

	Private Sub CheckMMprofile()

		dt = GetDataTable("select codeno,billref,vat from mmasttbl where mmdesc = '" & cboMMdesc.Text & "'")
		If Not CBool(dt.Rows.Count) Then
			tssErrorMsg.Text = "Not found in Materfile."
			Exit Sub

		Else
			For Each dr As DataRow In dt.Rows
				txtCodeNo.Text = dr.Item(0).ToString()
				lblBillRef.Text = dr.Item(1).ToString() & ""
				lblVNV.Text = dr.Item(2).ToString() & ""

			Next

		End If

		Call dt.Dispose()

	End Sub

	Private Sub txtInvNo_TextChanged(sender As Object, e As EventArgs) Handles txtInvNo.TextChanged
		If txtInvNo.Text = Nothing Then
			Exit Sub
		End If

		InvNoEnterProc()

	End Sub

	Private Sub InvNoEnterProc()
		If txtInvNo.Text = "" Or txtInvNo.Text = Nothing Then
			Exit Sub

		End If

		'cboType.Text = "SI"

		tsDocNoEdit.Text = txtInvNo.Text

		dt = GetDataTable("select status,tc,currstat,user2,doctype from saleshdrtbl where invno = '" & txtInvNo.Text & "'")
		If Not CBool(dt.Rows.Count) Then
			tssErrorMsg.Text = "Invoice No.:" & txtInvNo.Text & " Not Found"
			Exit Sub

		Else
			For Each dr As DataRow In dt.Rows
				If dr.Item(2).ToString() = "editing" Then
					If dr.Item(3).ToString() = lblUser.Text Then

					Else
						tssErrorMsg.Text = "SI No.:" & txtInvNo.Text & " is Currently open by: " & dr.Item(3).ToString()
						Exit Sub

					End If

				End If

				Dim Stat As String = UCase(dr.Item(0).ToString())
				If Stat = "CLOSE" Then
					tssErrorMsg.Text = "Invoice No.:" & txtInvNo.Text & Space(1) & " Already " & dr.Item(0).ToString()
					lbSave.Enabled = False
					lbDelete.Enabled = False
					'Exit Sub

				ElseIf Stat = "VOID" Then
					tssErrorMsg.Text = "Invoice No.:" & txtInvNo.Text & Space(1) & " Already " & dr.Item(0).ToString()
					lbSave.Enabled = False
					lbDelete.Enabled = False
					'Exit Sub

				Else
					tssErrorMsg.Text = "Invoice No.:" & txtInvNo.Text & Space(1) & " Belong to TC:" & dr.Item(1).ToString()
					tssDocStat.Text = "EDIT"
					'mdiMain.tsDocStat.Text = tssDocStat.text

				End If

				If dr.Item(4).ToString() = "DR" Then
					lbSave.Enabled = False
				End If

			Next

			Select Case lblTC.Text
				Case "53"
					cboSmnName.Enabled = False
					Call LoadSIHrd()

				Case Else
					'reserved for DSR transactions

			End Select

		End If

		dt.Dispose()

	End Sub

	Private Sub EditingMode()
		sql = "update saleshdrtbl set currstat = '" & "editing" & "',user2 = '" & lblUser.Text & "' " &
			  "where invno = '" & txtInvNo.Text & "'"
		ExecuteNonQuery(sql)

	End Sub

	Private Sub LoadSIHrd()

		vEditMode = "Edit"
		tsDocNoEdit.Text = txtInvNo.Text
		EditingMode()

		tssDocStat.Text = "EDIT"

		dt = GetDataTable("select a.transdate,concat(a.custno,space(1),b.bussname),concat(a.smnno,space(1),d.fullname)," &
						  "a.remarks,a.discamt,a.fhamt,a.docno,concat(a.shipto,space(1),c.bussname)," &
						  "b.areano,b.term,b.sptype,a.dono,concat(a.pc,space(1),f.pclass),a.otherbadoc " &
						  "from saleshdrtbl a left join custmasttbl b on a.custno=b.custno left join custmasttbl c on a.shipto=c.custno " &
						  "left join smnmtbl d on a.smnno=d.smnno left join areatbl e on b.areano=e.areano " &
						  "left join pctrtbl f on a.pc=f.pc where a.invno = '" & txtInvNo.Text & "' " &
						  "and a.status = '" & "open" & "'")
		If Not CBool(dt.Rows.Count) Then
			tssErrorMsg.Text = txtInvNo.Text & " not found"
			Exit Sub

		Else
			For Each dr As DataRow In dt.Rows
				dpTransDate.Text = Format(CDate(dr.Item(0).ToString()), "yyyy-MM-dd")
				cboCustName.Items.Add(dr.Item(1).ToString() & "")
				cboCustName.Text = dr.Item(1).ToString() & ""
				cboSmnName.Items.Add(dr.Item(2).ToString() & "")
				cboSmnName.Text = dr.Item(2).ToString() & ""
				txtRemarks.Text = dr.Item(3).ToString() & ""
				txtDeductn.Text = Format(CDbl(dr.Item(4).ToString()), "##,##0.00")
				txtFHamt.Text = Format(CDbl(dr.Item(5).ToString()), "##,##0.00")
				txtRefNo.Text = dr.Item(6).ToString() & ""
				cboShipTo.Items.Add(dr.Item(7).ToString() & "")
				cboShipTo.Text = dr.Item(7).ToString() & ""
				lblArea.Text = dr.Item(8).ToString() & ""
				lblTerm.Text = dr.Item(9).ToString() & ""
				lblSPtype.Text = dr.Item(10).ToString() & ""
				txtDONo.Text = dr.Item(11).ToString() & ""

				If cboPC.Items.Count = 0 Then
					cboPC.Items.Add(dr.Item(12).ToString() & "")
				End If

				cboPC.Text = dr.Item(12).ToString() & ""


				'tssMMRRNo.Text = dr.Item(18).ToString() & ""

			Next

		End If

		Call dt.Dispose()

		Call Filldgvdet()

		dt = GetDataTable("select bato,branchto from custmasttbl where custno = '" & cboCustName.Text.Substring(0, 5) & "'")
		If Not CBool(dt.Rows.Count) Then Exit Sub

		For Each dr As DataRow In dt.Rows
			lblBAto.Text = dr.Item(0).ToString() & ""
			lblBranchTo.Text = dr.Item(1).ToString() & ""

		Next

		Call dt.Dispose()

		cboCustName.Enabled = False
		cboShipTo.Enabled = False

		dt = GetDataTable("select user from doclocktbl where docno = '" & txtInvNo.Text & "' and tc = '" & lblTC.Text & "' and user <> '" & lblUser.Text & "'")
		If Not CBool(dt.Rows.Count) Then
			strStat = "EDIT"
			LogMeIn()
		Else
			For Each dr As DataRow In dt.Rows
				tssErrorMsg.Text = "SI No. " & txtInvNo.Text & " is currently open by: " & dr.Item(0).ToString() & ", try again later"
				lbSave.Enabled = False
				lbDelete.Enabled = False
				btnDel.Enabled = False
			Next

		End If

		dt.Dispose()

		txtDONo.ReadOnly = True

	End Sub

	Protected Sub Filldgvdet()
		CreateTempTable()

		sql = "delete from tempsalesdettbl2 where user = '" & lblUser.Text & "'"
		ExecuteNonQuery(sql)

		sql = "insert into tempsalesdettbl2(itemno,codeno,qty,wt,sp,itmamt,detgrossamt,detdiscamt,detvat," &
			  "nv,billref,dsrstat,user) select itemno,codeno,qty,wt,sp,itmamt,detgrossamt,detdiscamt,detvat," &
			  "nv,billref,dealstat,'" & lblUser.Text & "' from salesdettbl where invno = '" & txtInvNo.Text & "'"
		ExecuteNonQuery(sql)

		FillDGVsalesDetNew()

	End Sub

	Private Sub LogMeIn()
		sql = "insert into doclocktbl(docno,tc,transdate,user,stat,pdate)values" &
			  "('" & txtInvNo.Text & "','" & lblTC.Text & "','" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "'," &
			  "'" & lblUser.Text & "','" & strStat & "','" & Format(CDate(Now()), "yyyy-MM-dd HH:mm:ss") & "')"
		ExecuteNonQuery(sql)

	End Sub

	Private Sub LogMeOut()
		sql = "delete from doclocktbl where docno = '" & txtInvNo.Text & "' and tc = '" & lblTC.Text & "'"
		ExecuteNonQuery(sql)

	End Sub

	Private Sub cboSmnName_TextChanged(sender As Object, e As EventArgs) Handles cboSmnName.TextChanged
		If cboSmnName.Text = Nothing Then
			Exit Sub
		End If

		PopCustListT2()

	End Sub

	Protected Sub PopCustListT2()
		CboCustList.Items.Clear()
		dt = GetDataTable("select concat(a.custno,space(1),b.bussname) from isshdrtbl a " &
						  "left join custmasttbl b on a.custno=b.custno where a.dsrstat = 'open' and a.status = 'Open' " &
						  "and a.smnno = '" & cboSmnName.Text.Substring(0, 3) & "' and ifnull(a.nosi,'No') = 'No' " &
						  "group by a.custno order by b.bussname") 'and pc = '" & lblPC.Text & "'
		If Not CBool(dt.Rows.Count) Then

		Else
			CboCustList.Items.Add("ALL")
			For Each dr As DataRow In dt.Rows
				CboCustList.Items.Add(dr.Item(0).ToString() & "")
			Next

		End If

		dt.Dispose()

	End Sub

	Private Sub CboCustList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CboCustList.SelectedIndexChanged
		If CboCustList.Text = Nothing Then
			Exit Sub
		End If

		DgvDOList.DataSource = Nothing
		DgvDOList.DataBind()
		DgvDOdetList.DataSource = Nothing
		DgvDOdetList.DataBind()
		lblTotQtyT2.Text = "0"
		lblTotWtT2.Text = "0.00"
		FillDgvDOlist()

	End Sub

	Protected Sub FillDgvDOlist()
		Select Case CboCustList.Text
			Case "ALL"
				dt = GetDataTable("select a.status from isshdrtbl a left join issdettbl b on a.dono=b.dono " &
								  "where a.dsrstat = 'open' and a.status = 'Open' and a.smnno = '" & cboSmnName.Text.Substring(0, 3) & "' " &
								  "and ifnull(a.nosi,'No') = 'No' and b.codeno is not null")
			Case Else
				dt = GetDataTable("select a.status from isshdrtbl a left join issdettbl b on a.dono=b.dono " &
								  "where a.dsrstat = 'open' and a.status = 'Open' and a.smnno = '" & cboSmnName.Text.Substring(0, 3) & "' " &
								  "and ifnull(a.nosi,'No') = 'No' and b.codeno is not null and " &
								  "a.custno = '" & CboCustList.Text.Substring(0, 5) & "'")
		End Select

		If Not CBool(dt.Rows.Count) Then
			tssErrorMsg.Text = "No Open DO Found"
			DgvDOList.DataSource = Nothing
			DgvDOList.DataBind()

		End If

		Dim adapter As New MySqlDataAdapter
		Dim ds As New DataSet()
		sqldata = Nothing
		Select Case CboCustList.Text
			Case "ALL"
				sqldata = "select a.dono,a.transdate,concat(a.custno,space(1),e.bussname) as custno," &
						  "concat(a.shipto,space(1),c.bussname) as shipto,a.status,a.sono from isshdrtbl a " &
						  "left join issdettbl b on a.dono=b.dono left join custmasttbl e on a.custno=e.custno " &
						  "left join custmasttbl c on a.shipto=c.custno where a.dsrstat = 'open' and a.status = 'Open' " &
						  "and a.smnno = '" & cboSmnName.Text.Substring(0, 3) & "' and ifnull(a.nosi,'No') = 'No' " &
						  "and b.codeno is not null order by a.transdate desc"
			Case Else
				sqldata = "select a.dono,a.transdate,concat(a.custno,space(1),e.bussname) as custno," &
						  "concat(a.shipto,space(1),c.bussname) as shipto,a.status,a.sono from isshdrtbl a " &
						  "left join issdettbl b on a.dono=b.dono left join custmasttbl e on a.custno=e.custno " &
						  "left join custmasttbl c on a.shipto=c.custno where a.dsrstat = 'open' and a.status = 'Open' " &
						  "and a.smnno = '" & cboSmnName.Text.Substring(0, 3) & "' and ifnull(a.nosi,'No') = 'No' " &
						  "and b.codeno is not null and a.custno = '" & CboCustList.Text.Substring(0, 5) & "' order by a.transdate desc"
		End Select

		conn.Open()
		Dim command As New MySqlCommand(sqldata, conn)
		adapter.SelectCommand = command
		adapter.Fill(ds)
		adapter.Dispose()
		command.Dispose()
		conn.Close()
		DgvDOList.DataSource = ds.Tables(0)
		DgvDOList.DataBind()

	End Sub

	Private Sub DgvDOList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DgvDOList.SelectedIndexChanged
		DgvDOdetList.DataSource = Nothing
		DgvDOdetList.DataBind()
		FillDOdetList()


		If DgvDOdetList.Rows.Count > 0 Then
			clrControls()
			txtDONo.Text = DgvDOList.SelectedRow.Cells(1).Text
			ExecLoadDO()

		Else
			Exit Sub
		End If
	End Sub

	Protected Sub FillDOdetList()
		Dim adapter As New MySqlDataAdapter
		Dim ds As New DataSet()
		sqldata = Nothing

		dt = GetDataTable("select sum(ifnull(qty,0)),sum(ifnull(wt,0)) from issdettbl where " &
						  "dono = '" & DgvDOList.SelectedRow.Cells(1).Text & "' group by dono")
		If Not CBool(dt.Rows.Count) Then
			lblTotQtyT2.Text = "0"
			lblTotWtT2.Text = "0.00"
			DgvDOdetList.DataSource = Nothing
			DgvDOdetList.DataBind()

		Else
			For Each dr As DataRow In dt.Rows
				lblTotQtyT2.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0 ; (#,##0)")
				lblTotWtT2.Text = Format(CDbl(dr.Item(1).ToString()), "#,##0.00 ; (#,##0.00)")

			Next

		End If

		dt.Dispose()

		sqldata = "select a.itmno,a.codeno,ifnull(b.codename,b.mmdesc) as mmdesc,ifnull(a.qty,0) as qty," &
				  "ifnull(a.wt,0) as wt from issdettbl a left join mmasttbl b on a.codeno=b.codeno where " &
				  "a.dono = '" & DgvDOList.SelectedRow.Cells(1).Text & "' order by a.itmno"

		conn.Open()
		Dim command As New MySqlCommand(sqldata, conn)
		adapter.SelectCommand = command
		adapter.Fill(ds)
		adapter.Dispose()
		command.Dispose()
		conn.Close()
		DgvDOdetList.DataSource = ds.Tables(0)
		DgvDOdetList.DataBind()

	End Sub

	Private Sub ExecLoadDO()
		Try
			If vProcADM = "AutoADM" Then
				Exit Sub
			End If
			If txtDONo.Text = "" Then
				Exit Sub
			End If

			Dim strStat As String
			strStat = "OPEN"

			dt = GetDataTable("select dsrstat,dsrno,mov from isshdrtbl where dono = '" & txtDONo.Text & "' and dsrno <> '" & txtInvNo.Text & "'")
			If Not CBool(dt.Rows.Count) Then
				tssErrorMsg.Text = "Not found."
				Exit Sub
			Else
				For Each dr As DataRow In dt.Rows
					Dim strDOstat As String = dr.Item(0).ToString()

					If strDOstat = "SERVED" Then
						tssErrorMsg.Text = "DR No. " & txtDONo.Text & " Already " & dr.Item(0).ToString() & " to Inv. No.:" & dr.Item(1).ToString()
						Exit Sub

					Else
						Select Case dr.Item(2).ToString()
							Case "321"

							Case "601"

							Case "605"

							Case "607"

							Case "609"

							Case Else
								tssErrorMsg.Text = "DR No. " & txtDONo.Text & " Issued to Movement:" & dr.Item(2).ToString()
								Exit Sub

						End Select

					End If

				Next

			End If

			'Call dt.Dispose()

			dt = GetDataTable("select concat(a.custno,space(1),e.bussname),concat(a.smnno,space(1),b.fullname)," &
							  "concat(a.shipto,space(1),f.bussname),e.areano,e.term,e.sptype,e.bato,e.branchto," &
							  "ifnull(e.vat,'NV') from isshdrtbl a left join smnmtbl b on a.smnno=b.smnno " &
							  "left join plnttbl c on a.plntno=c.plntno left join custmasttbl e on a.custno=e.custno " &
							  "left join custmasttbl f on a.shipto=f.custno where a.dono = '" & txtDONo.Text & "' " &
							  "and a.dsrstat <> '" & "served" & "'")  'and a.mov = '" & "601" & "' or a.mov = '" & "607" & "'")
			If Not CBool(dt.Rows.Count) Then
				tssErrorMsg.Text = "Not found."
				Exit Sub
			Else
				For Each dr As DataRow In dt.Rows

					cboCustName.Items.Add(dr.Item(0).ToString())
					cboCustName.Text = dr.Item(0).ToString()
					cboSmnName.Text = dr.Item(1).ToString()
					cboShipTo.Items.Add(dr.Item(2).ToString())
					cboShipTo.Text = dr.Item(2).ToString()
					lblArea.Text = dr.Item(3).ToString()
					lblTerm.Text = dr.Item(4).ToString()
					lblSPtype.Text = dr.Item(5).ToString()
					lblBAto.Text = dr.Item(6).ToString()
					lblBranchTo.Text = dr.Item(7).ToString()
					lblCustVat.Text = dr.Item(8).ToString()

				Next

			End If

			dt.Dispose()

			dt = GetDataTable("select concat(a.pc,space(1),b.pclass) from isshdrtbl a left " &
							  "join pctrtbl b on a.pc=b.pc where a.dono = '" & txtDONo.Text & "'")  'and a.mov = '" & "601" & "' or a.mov = '" & "607" & "'")
			If Not CBool(dt.Rows.Count) Then
				tssErrorMsg.Text = "Not found."

			Else
				For Each dr As DataRow In dt.Rows
					cboPC.Text = dr.Item(0).ToString()
				Next
			End If

			dt.Dispose()

			cboCustName.Enabled = False
			cboShipTo.Enabled = False

			FillDgvDetDR()

			'PreSIsaveProc()
			admStatType = "Initial"

		Catch ex As Exception
			tssErrorMsg.Text = ErrorToString()
		End Try

	End Sub

	Protected Sub CreateTempTable()
		dt = GetDataTable("select * from information_schema.tables where table_schema =  '" & vDbase & "' and " &
						  "table_name = 'tempsalesdettbl2'")
		If Not CBool(dt.Rows.Count) Then
			sql = "CREATE TABLE tempsalesdettbl2 LIKE tempsalesdettbl"
			ExecuteNonQuery(sql)

		End If

		dt.Dispose()
	End Sub

	Protected Sub FillDgvDetDR()
		CreateTempTable()

		sql = "delete from tempsalesdettbl2 where user = '" & lblUser.Text & "'"
		ExecuteNonQuery(sql)

		sql = "insert into tempsalesdettbl2(itemno,codeno,qty,wt,sp,itmamt,detgrossamt,nv,billref,dsrstat,user) " &
			  "select a.itmno,a.codeno,ifnull(a.qty,0),ifnull(a.wt,0),ifnull(a.sp,0),ifnull(a.itmamt,0)," &
			  "ifnull(a.itmamt,0),b.vat,ifnull(b.billref,'wt'),ifnull(a.stat,'N'),'" & lblUser.Text & "' from " &
			  "issdettbl a left join mmasttbl b on a.codeno=b.codeno where a.dono = '" & txtDONo.Text & "' " &
			  "group by a.idno"
		ExecuteNonQuery(sql)

		Select Case UCase(lblCustVat.Text)
			Case "V"
				sql = "update tempsalesdettbl2 set detvat = itmamt - (itmamt/1.12) where user = '" & lblUser.Text & "'"
				ExecuteNonQuery(sql)

			Case Else
				sql = "update tempsalesdettbl2 set detvat = 0 where user = '" & lblUser.Text & "'"
				ExecuteNonQuery(sql)
		End Select

		cboMMdesc.Items.Clear()
		dt = GetDataTable("select ifnull(b.codename,b.mmdesc) as mmdesc from tempsalesdettbl2 a " &
						  "left join mmasttbl b on a.codeno=b.codeno where a.user = '" & lblUser.Text & "' " &
						  "group by a.codeno order by mmdesc")
		If Not CBool(dt.Rows.Count) Then
			tssErrorMsg.Text = "Not found."
			Exit Sub
		Else
			cboMMdesc.Items.Add("")
			For Each dr As DataRow In dt.Rows
				cboMMdesc.Items.Add(dr.Item(0).ToString())

			Next
		End If

		dt.Dispose()

		FillDGVsalesDetNew()



	End Sub

	Protected Sub FillDGVsalesDetNew()
		Dim adapter As New MySqlDataAdapter
		Dim ds As New DataSet()
		sqldata = Nothing

		dt = GetDataTable("select sum(ifnull(qty,0)),sum(ifnull(wt,0)),sum(ifnull(itmamt,0)),sum(ifnull(detdiscamt,0))," &
						  "sum(ifnull(detgrossamt,0)),sum(ifnull(detvat,0)) from tempsalesdettbl2 where " &
						  "user = '" & lblUser.Text & "'")
		If Not CBool(dt.Rows.Count) Then
			lblTotQty.Text = "0"
			lblTotWt.Text = "0.00"
			lblTotGross.Text = "0.00"
			lblTotDisc.Text = "0.00"
			lblTotNetAmt.Text = "0.00"
			lblTotVatAmt.Text = "0.00"
			DgvSalesdet.DataSource = Nothing
			DgvSalesdet.DataBind()

		Else
			For Each dr As DataRow In dt.Rows
				lblTotQty.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0 ; (#,##0)")
				lblTotWt.Text = Format(CDbl(dr.Item(1).ToString()), "#,##0.00 ; (#,##0.00)")
				lblTotGross.Text = Format(CDbl(dr.Item(2).ToString()), "#,##0.00 ; (#,##0.00)")
				lblTotDisc.Text = Format(CDbl(dr.Item(3).ToString()), "#,##0.00 ; (#,##0.00)")
				lblTotNetAmt.Text = Format(CDbl(dr.Item(4).ToString()), "#,##0.00 ; (#,##0.00)")
				lblTotVatAmt.Text = Format(CDbl(dr.Item(5).ToString()), "#,##0.00 ; (#,##0.00)")
			Next

		End If

		dt.Dispose()

		sqldata = "select a.itemno,a.codeno,ifnull(b.codename,b.mmdesc) as mmdesc,a.qty,a.wt,a.sp,a.itmamt," &
				  "a.detdiscamt,a.detgrossamt,a.detvat from tempsalesdettbl2 a left join mmasttbl b on " &
				  "a.codeno=b.codeno where a.user = '" & lblUser.Text & "' order by a.itemno"

		conn.Open()
		Dim command As New MySqlCommand(sqldata, conn)
		adapter.SelectCommand = command
		adapter.Fill(ds)
		adapter.Dispose()
		command.Dispose()
		conn.Close()
		DgvSalesdet.DataSource = ds.Tables(0)
		DgvSalesdet.DataBind()


		Dim GrossWVAT As Double = CDbl(IIf(lblTotNetAmt.Text = "", 0, lblTotNetAmt.Text)) - CDbl(IIf(lblTotVatAmt.Text = "", 0, lblTotVatAmt.Text))
		lblGrossAmt.Text = Format(GrossWVAT, "##,##0.00")
		lblTaxes.Text = lblTotVatAmt.Text

		txtItm.Text = DgvSalesdet.Rows.Count + 1

		Call CompNetSales()
	End Sub

	Private Sub CompNetSales()

		Dim NetSales = CDbl(IIf(lblGrossAmt.Text = "", 0, lblGrossAmt.Text)) + CDbl(IIf(lblTaxes.Text = "", 0, lblTaxes.Text)) +
					   CDbl(IIf(txtFHamt.Text = "", 0, txtFHamt.Text)) - CDbl(IIf(txtDeductn.Text = "", 0, txtDeductn.Text))
		lblNetAmt.Text = Format(NetSales, "##,##0.00")

	End Sub

	Private Sub ClrLines()

		Dim lvCount As Long = DgvSalesdet.Rows.Count
		txtItm.Text = lvCount + 1

		txtCodeNo.Text = ""
		cboMMdesc.Text = ""
		txtQty.Text = ""
		txtWt.Text = ""
		txtSP.Text = ""
		lblGrossAmtDet.Text = ""
		txtDiscDet.Text = ""
		lblNetAmtDet.Text = ""
		txtVATamt.Text = ""
		txtDiscRate.Text = ""
		txtSP.ReadOnly = False
		lblBillRef.Text = ""
		cboMMdesc.Focus()

	End Sub

	Protected Sub PreSIsaveProc()
		If tssDocStat.Text = "New" Then
			If txtInvNo.Text = "" Or txtInvNo.Text = Nothing Then
				PreSaveHdrProc()

			End If

		End If

	End Sub

	Private Sub PreSaveHdrProc()

		sql = "insert into saleshdrtbl(invno,tc,dono,user) select max(invno)+ 1,'" & lblTC.Text & "','" & txtDONo.Text & "'," &
			  "'" & lblUser.Text & "' from saleshdrtbl where invno between '10000001' and '19999999' and user <> 'admin'"
		ExecuteNonQuery(sql)

		dt = GetDataTable("select max(invno) from saleshdrtbl where user = '" & lblUser.Text & "' and invno between " &
						  "'10000001' and '19999999' order by invno")
		If Not CBool(dt.Rows.Count) Then Exit Sub

		For Each dr As DataRow In dt.Rows
			txtInvNo.Text = dr.Item(0).ToString()

		Next

		dt.Dispose()

	End Sub

	Private Sub PreSaveHdr()
		Dim strStat As String = "OPEN"
		sql = "insert into saleshdrtbl(invno,tc,dono,user)values('" & txtInvNo.Text & "','" & lblTC.Text & "'," &
			  "'" & txtDONo.Text & "','" & lblUser.Text & "')"
		ExecuteNonQuery(sql)

	End Sub

	Protected Sub DgvSIsumList_RowDataBound(sender As Object, e As GridViewRowEventArgs)

	End Sub

	Protected Sub DgvSIsumList_RowCreated(sender As Object, e As GridViewRowEventArgs)

	End Sub

	Private Sub dpDateFrT3_TextChanged(sender As Object, e As EventArgs) Handles dpDateFrT3.TextChanged, dpDateToT3.TextChanged
		If dpDateFrT3.Text = Nothing Then
			Exit Sub
		ElseIf dpDateToT3.Text = Nothing Then
			Exit Sub
		ElseIf Format(CDate(dpDateFrT3.Text), "yyyy-MM-dd") > Format(CDate(dpDatetoT3.Text), "yyyy-MM-dd") Then
			Exit Sub
		End If

		PopSIstatus()

	End Sub

	Protected Sub PopSIstatus()
		cboSIstatus.Items.Clear()
		dt = GetDataTable("select status from saleshdrtbl where transdate between '" & Format(CDate(dpDateFrT3.Text), "yyyy-MM-dd") & "' " &
						  "and '" & Format(CDate(dpDateToT3.Text), "yyyy-MM-dd") & "' and doctype is not null group by status")
		If Not CBool(dt.Rows.Count) Then

		Else
			cboSIstatus.Items.Add("")
			For Each dr As DataRow In dt.Rows
				cboSIstatus.Items.Add(dr.Item(0).ToString() & "")
			Next

		End If

		dt.Dispose()

	End Sub

	Private Sub cboSIstatus_TextChanged(sender As Object, e As EventArgs) Handles cboSIstatus.TextChanged
		If cboSIstatus.Text = "" Then
			Exit Sub
		End If

		FillDgvSIsummary()

	End Sub

	Protected Sub FillDgvSIsummary()
		dt = GetDataTable("select status from saleshdrtbl where transdate between '" & Format(CDate(dpDateFrT3.Text), "yyyy-MM-dd") & "' and " &
						  "'" & Format(CDate(dpDateToT3.Text), "yyyy-MM-dd") & "' and doctype is not null group by status")
		If Not CBool(dt.Rows.Count) Then
			DgvSIsumList.DataSource = Nothing
			DgvSIsumList.DataBind()
			Exit Sub
		End If

		dt.Dispose()

		Dim adapter As New MySqlDataAdapter
		Dim ds As New DataSet()
		sqldata = Nothing

		sqldata = "select a.doctype,a.invno,a.docno,a.transdate,concat(a.custno,space(1),b.bussname) as custno," &
				  "concat(a.shipto,space(1),c.bussname) as shipto,a.dono,a.status from saleshdrtbl a " &
				  "left join custmasttbl b on a.custno=b.custno left join custmasttbl c on a.shipto=c.custno " &
				  "where a.transdate between '" & Format(CDate(dpDateFrT3.Text), "yyyy-MM-dd") & "' and " &
				  "'" & Format(CDate(dpDateToT3.Text), "yyyy-MM-dd") & "' and a.status = '" & cboSIstatus.Text & "' " &
				  "and doctype is not null order by a.transdate,a.invno"

		conn.Open()
		Dim command As New MySqlCommand(sqldata, conn)
		adapter.SelectCommand = command
		adapter.Fill(ds)
		adapter.Dispose()
		command.Dispose()
		conn.Close()
		DgvSIsumList.DataSource = ds.Tables(0)
		DgvSIsumList.DataBind()


	End Sub

	Private Sub DgvSIsumList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DgvSIsumList.SelectedIndexChanged
		txtInvNo.Text = DgvSIsumList.SelectedRow.Cells(2).Text
		If txtInvNo.Text = Nothing Then
			Exit Sub
		End If

		InvNoEnterProc()

	End Sub

End Class