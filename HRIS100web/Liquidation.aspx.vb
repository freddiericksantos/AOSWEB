Imports MySql.Data.MySqlClient
Imports System
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports MySql.Data

Public Class Liquidation
	Inherits System.Web.UI.Page
	Dim dt As DataTable
	Dim MyDA_conn As New MySqlDataAdapter
	Dim MyDataSet As New DataSet
	Dim Answer As String
	Dim MySqlScript As String
	Dim gList As String
	Dim sql As String
	Dim strCV As String
	Dim gAcctNo As String
	Dim currDate As Date
	Dim gPreNo As String
	Dim gYear As String
	Dim gMon As String
	Dim strCCNo As String
	Dim amt As Double
	Dim sqldata As String

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		If lblUser.Text Is Nothing Then
			Response.Redirect("login.aspx")

		End If

		If Not Me.IsPostBack Then
			lblUser.Text = Session("UserID")
			lblGrpUser.Text = Session("UserGrp")
			vFormOpen = Session("FormOpen")

		End If

	End Sub

	Protected Sub AdmMsgBox(ByVal sMessage As String)
		Dim msg As String
		msg = "<script language='javascript'>"
		msg += "alert('" & sMessage & "');"
		msg += "<" & "/script>"
		Response.Write(msg)

	End Sub

	Protected Sub lbSave_Click(sender As Object, e As EventArgs)

	End Sub

	Protected Sub lbPrint_Click(sender As Object, e As EventArgs)

	End Sub

	Protected Sub dgvSum_RowDataBound(sender As Object, e As GridViewRowEventArgs)

	End Sub

	Protected Sub dgvSum_RowCreated(sender As Object, e As GridViewRowEventArgs)

	End Sub

	Protected Sub dgvGLentry_RowDataBound(sender As Object, e As GridViewRowEventArgs)

	End Sub

	Protected Sub dgvGLentry_RowCreated(sender As Object, e As GridViewRowEventArgs)

	End Sub

	Protected Sub DgvJVdet_RowDataBound(sender As Object, e As GridViewRowEventArgs)

	End Sub

	Protected Sub DgvJVdet_RowCreated(sender As Object, e As GridViewRowEventArgs)

	End Sub

	Protected Sub dgvSubs_RowDataBound(sender As Object, e As GridViewRowEventArgs)

	End Sub

	Protected Sub dgvSubs_RowCreated(sender As Object, e As GridViewRowEventArgs)

	End Sub

	Private Sub lbNew_Click(sender As Object, e As EventArgs) Handles lbNew.Click
		lblFormName.Text = vFormOpen
		Select Case vFormOpen
			Case "Advances to Vendor"
				tssNameMe.Text = vFormOpen
				gAcctNo = "1141300"
				CheckBox1.Visible = True
				CheckBox2.Visible = False
			Case "CA Liquidation"
				tssNameMe.Text = vFormOpen
				gAcctNo = "1110380"
				'lblMMRRlabel.Visible = False
				'txtMMRRno.Visible = False
				CheckBox1.Visible = False
				CheckBox2.Visible = True
		End Select

		JVclear()
		JVlineItemClr()

		getCCtrAll()

		dt = GetDataTable("select concat(acctno,space(1),acctdesc) from acctcharttbl where acctno = '" & gAcctNo & "'")
		If Not CBool(dt.Rows.Count) Then
			Exit Sub
		Else
			For Each dr As DataRow In dt.Rows
				lblFormName.Text = dr.Item(0).ToString() & ""

			Next

		End If

		dt.Dispose()

	End Sub

	Private Sub JVclear()

		dt = GetDataTable("select now() from batbl where ba = '" & vLoggedBussArea & "'")
		If Not CBool(dt.Rows.Count) Then tsErrMsg.Text = "No Cost Center found." : Exit Sub

		For Each dr As DataRow In dt.Rows
			currDate = dr.Item(0).ToString()

		Next

		Call dt.Dispose()

		txtJVNo.Text = Nothing
		txtVPamt.Text = ""
		txtREfNo.Text = ""
		txtRemarks.Text = ""
		txtVendor.Text = ""

		DgvJVdet.DataSource = Nothing
		DgvJVdet.DataBind()

		'txtJVNo.ReadOnly = False
		'txtVPamt.ReadOnly = False
		lblDRtot.Text = "0.00"
		lblCRtot.Text = "0.00"
		lblBalAmt.Text = "0.00"

		dpPostDate.Text = Format(CDate(currDate), "yyyy-MM-dd")
		dpTransDate.Text = Format(CDate(currDate), "yyyy-MM-dd")

	End Sub

	Private Sub JVlineItemClr()
		Dim lvCount As Long = DgvJVdet.Rows.Count
		txtItm.Text = lvCount + 1

		cboCCtr.Text = Nothing
		cboGLdesc.Text = Nothing
		txtDRamt.Text = ""
		txtCRamt.Text = ""
		lblPK.Text = ""
		txtSubAcct.Text = ""

	End Sub

	Private Sub getCCtrAll()
		cboCCtr.Items.Clear()

		dt = GetDataTable("select concat(ccno,space(1),ccdesc) from cctrnotbl where ba = '" & vLoggedBussArea & "' and branch = '" & vLoggedBranch & "' order by ccno")
		If Not CBool(dt.Rows.Count) Then
			tsErrMsg.Text = "No Cost Center found."
			Exit Sub
		Else
			cboCCtr.Items.Add("")
			For Each dr As DataRow In dt.Rows
				cboCCtr.Items.Add(dr.Item(0).ToString())

			Next
		End If

		Call dt.Dispose()

		'Select Case tssNameMe.Text
		'    Case "Advances to Vendor"


		'    Case "CA Liquidation"


		'End Select

	End Sub

	Private Sub popStatList()
		cboStat.Items.Clear()

		Select Case tssNameMe.Text
			Case "Advances to Vendor"
				dt = GetDataTable("select ifnull(a.depstat,'Open') from expdettbl a left join exphdrtbl b on a.docno=b.docno where " &
								  "b.transdate between '" & Format(CDate(dpTransDate1.Text), "yyyy-MM-dd") & "' " &
								  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and " &
								  "a.acctno = '" & lblFormName.Text.Substring(0, 7) & "' and " &
								  "a.dramt > 0 group by a.depstat")
			Case "CA Liquidation"
				dt = GetDataTable("select ifnull(a.liqstat,'Open') from exphdrtbl a left join expdettbl c on a.docno=c.docno " &
								  "where a.transdate between '" & Format(CDate(dpTransDate1.Text), "yyyy-MM-dd") & "' " &
								  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and a.tc = '42' and " &
								  "c.acctno = '" & lblFormName.Text.Substring(0, 7) & "' group by a.liqstat")
		End Select

		If Not CBool(dt.Rows.Count) Then
			Exit Sub

		Else
			cboStat.Items.Add("")
			For Each dr As DataRow In dt.Rows
				cboStat.Items.Add(dr.Item(0).ToString())

			Next

		End If


		dt.Dispose()


	End Sub

	Private Sub dpTransDate1_TextChanged(sender As Object, e As EventArgs) Handles dpTransDate1.TextChanged, dpTransDate2.TextChanged
		If dpTransDate1.Text = Nothing Then
			Exit Sub
		ElseIf dpTransDate2.Text = Nothing Then
			Exit Sub
		ElseIf Format(CDate(dpTransDate1.Text), "yyyy-MM-dd") > Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") Then
			Exit Sub
		End If

		popStatList()


	End Sub

	Protected Sub FillDataToClose()
		Select Case UCase(cboStat.Text)
			Case "OPEN"
				Select Case tssNameMe.Text
					Case "Advances to Vendor"
						'If CheckBox1.Checked = True And dgvMMRRlist.RowCount > 0 Then
						'	Answer = MessageBox.Show("VP No. " & txtREfNo.Text & " currently on process, do you want to process new?", Me.Text, MessageBoxButtons.YesNo)
						'	If Answer = vbYes Then
						'		CheckBox1.Checked = False
						'	Else

						'	End If

						'End If

						'txtREfNo.Text = dgvSum.SelectedCells(0).Value.ToString
						'txtREfNo.ReadOnly = True

					Case "CA Liquidation"
						txtREfNo.Text = dgvSum.SelectedRow.Cells(1).Text
						'txtREfNo.ReadOnly = True
						If CheckBox2.Checked = True Then
							'FilldgvVendorMulti()
						End If
				End Select

				'CheckBox1

				txtTC.Text = dgvSum.SelectedRow.Cells(2).Text
				txtVendor.Text = dgvSum.SelectedRow.Cells(4).Text
				txtVPamt.Text = Format(CDbl(dgvSum.SelectedRow.Cells(5).Text), "#,##0.00")
				txtRemarks.Text = "Reversal for VP No." & dgvSum.SelectedRow.Cells(1).Text &
								  " Dated " & Format(CDate(dgvSum.SelectedRow.Cells(3).Text), "MMM dd, yyyy")
				cboCCtr.Focus()

			Case Else
				dt = GetDataTable("select status from gljvhdrtbl where jvno = '" & dgvSum.SelectedRow.Cells(7).Text & "'")
				If Not CBool(dt.Rows.Count) Then

				Else
					For Each dr As DataRow In dt.Rows
						Select Case UCase(dr.Item(0).ToString())
							Case "PARK"
								txtJVNo.Text = dgvSum.SelectedRow.Cells(7).Text
								txtREfNo.Text = dgvSum.SelectedRow.Cells(1).Text
								'txtREfNo.ReadOnly = True
								txtTC.Text = dgvSum.SelectedRow.Cells(2).Text
								txtVendor.Text = dgvSum.SelectedRow.Cells(4).Text
								txtVPamt.Text = Format(CDbl(dgvSum.SelectedRow.Cells(5).Text), "#,##0.00")
								txtRemarks.Text = "Reversal for VP No." & dgvSum.SelectedRow.Cells(1).Text &
												  " Dated " & Format(CDate(dgvSum.SelectedRow.Cells(3).Text), "MMM dd, yyyy")
								'ReloadJVdet()
								txtJVNo.ReadOnly = True
								'TabControl1.SelectedTab = TabPage2
								lbPrint.Enabled = True

								'If CheckBox2.Checked = True Then
								'	FilldgvVendorMulti()
								'End If



							Case Else

								'TabControl1.SelectedTab = TabPage3

						End Select

					Next
				End If

		End Select

		'cboVenName.Text = dgvSum.SelectedRow.Cells(4).Text

	End Sub

	Private Sub FillSumList()
		Dim adapter As New MySqlDataAdapter
		Dim ds As New DataSet()
		sqldata = Nothing

		Select Case UCase(cboStat.Text)
			Case "OPEN"
				Select Case tssNameMe.Text
					Case "Advances to Vendor"
						dt = GetDataTable("select * from expdettbl a left join exphdrtbl b on a.docno=b.docno where " &
										  "a.depstat is null and b.transdate between '" & Format(CDate(dpTransDate1.Text), "yyyy-MM-dd") & "' " &
										  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and " &
										  "a.acctno = '" & lblFormName.Text.Substring(0, 7) & "' and a.dramt > 0 and " &
										  "b.status <> 'void'")
						If Not CBool(dt.Rows.Count) Then
							dgvSum.DataSource = Nothing
							Exit Sub

						End If

						dt.Dispose()

						sqldata = "select b.docno,b.tc,b.transdate,concat(a.venno,space(1),c.venname) as vendor,sum(a.dramt) as dramt," &
								  "b.remarks from expdettbl a left join exphdrtbl b on a.docno=b.docno left join venmasttbl c on a.venno=c.venno " &
								  "where a.depstat is null and b.transdate between '" & Format(CDate(dpTransDate1.Text), "yyyy-MM-dd") & "' " &
								  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and " &
								  "a.acctno = '" & lblFormName.Text.Substring(0, 7) & "' and a.dramt > 0 and " &
								  "b.status <> 'void' group by a.docno,a.acctno order by a.docno"

					Case "CA Liquidation"
						dt = GetDataTable("select * from expdettbl b left join exphdrtbl a on a.docno=b.docno where " &
										  "a.transdate between '" & Format(CDate(dpTransDate1.Text), "yyyy-MM-dd") & "' " &
										  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and " &
										  "b.acctno = '" & lblFormName.Text.Substring(0, 7) & "' " &
										  "and a.tc = '42' and ifnull(a.liqstat,'Open') = 'Open'")
						If Not CBool(dt.Rows.Count) Then
							dgvSum.DataSource = Nothing
							Exit Sub

						End If

						dt.Dispose()

						sqldata = "select b.docno,b.tc,b.transdate,concat(a.venno,space(1),c.venname) as vendor," &
								  "sum(a.dramt) as dramt,b.remarks,ifnull(b.cvno,'No CV') as replno from expdettbl a " &
								  "left join exphdrtbl b on a.docno=b.docno left join venmasttbl c on a.venno=c.venno " &
								  "where a.transdate between '" & Format(CDate(dpTransDate1.Text), "yyyy-MM-dd") & "' " &
								  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and " &
								  "a.acctno = '" & lblFormName.Text.Substring(0, 7) & "' and a.tc = '42' and " &
								  "ifnull(b.liqstat,'Open') = 'Open' group by a.docno,a.acctno order by a.docno"
				End Select

			Case Else
				Select Case tssNameMe.Text
					Case "Advances to Vendor"
						dt = GetDataTable("select * from expdettbl a left join exphdrtbl b on a.docno=b.docno where " &
										  "a.depstat = '" & cboStat.Text & "' and b.transdate between '" & Format(CDate(dpTransDate1.Text), "yyyy-MM-dd") & "' " &
										  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and " &
										  "a.acctno = '" & lblFormName.Text.Substring(0, 7) & "' and " &
										  "a.dramt > 0 and b.status <> 'void' group by a.depstat")
						If Not CBool(dt.Rows.Count) Then
							dgvSum.DataSource = Nothing
							Exit Sub

						End If

						dt.Dispose()

						sqldata = "select b.docno,b.tc,b.transdate,concat(a.venno,space(1),c.venname) as vendor,sum(a.dramt) as dramt," &
								  "b.remarks,a.pctrno from expdettbl a left join exphdrtbl b on a.docno=b.docno left join venmasttbl c on a.venno=c.venno " &
								  "where a.depstat = '" & cboStat.Text & "' and b.transdate between '" & Format(CDate(dpTransDate1.Text), "yyyy-MM-dd") & "' " &
								  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and " &
								  "a.acctno = '" & lblFormName.Text.Substring(0, 7) & "' and " &
								  "a.dramt > 0 and b.status <> 'void' group by a.docno,a.acctno order by a.docno"

					Case "CA Liquidation"
						dt = GetDataTable("select * from expdettbl b left join exphdrtbl a on a.docno=b.docno where " &
										  "a.transdate between '" & Format(CDate(dpTransDate1.Text), "yyyy-MM-dd") & "' " &
										  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and " &
										  "b.acctno = '" & lblFormName.Text.Substring(0, 7) & "' " &
										  "and a.tc = '42' and a.liqstat = '" & cboStat.Text & "'")
						If Not CBool(dt.Rows.Count) Then
							dgvSum.DataSource = Nothing
							Exit Sub

						End If

						dt.Dispose()

						sqldata = "select b.docno,b.tc,b.transdate,concat(a.venno,space(1),c.venname) as vendor,sum(a.dramt) as dramt," &
								  "b.remarks,b.replno from expdettbl a left join exphdrtbl b on a.docno=b.docno left join venmasttbl c on a.venno=c.venno " &
								  "where a.transdate between '" & Format(CDate(dpTransDate1.Text), "yyyy-MM-dd") & "' " &
								  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and " &
								  "a.acctno = '" & lblFormName.Text.Substring(0, 7) & "' " &
								  "and a.tc = '42' and b.liqstat = '" & cboStat.Text & "' group by a.docno,a.acctno order by a.docno"

				End Select

		End Select

		With dgvSum
			'.Columns(1).ItemStyle.Width


			Select Case UCase(cboStat.Text)
				Case "CLOSE"
					.Columns(7).HeaderText = "Ref No."
				Case "LIQUIDATED"
					.Columns(7).HeaderText = "JV No."
				Case Else
					.Columns(7).HeaderText = "CV No."
			End Select

		End With

		conn.Open()
		Dim command As New MySqlCommand(sqldata, conn)
		adapter.SelectCommand = command
		adapter.Fill(ds)
		adapter.Dispose()
		command.Dispose()
		conn.Close()
		dgvSum.DataSource = ds.Tables(0)
		dgvSum.DataBind()

		popVenList()

	End Sub

	Private Sub popVenList()
		cboVenName.Items.Clear()
		dt = GetDataTable("select concat(a.subacct,space(1),c.venname) from gltranstbl a left join venmasttbl c on a.subacct=c.venno " &
						  "where a.transdate <= '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
						  "and a.acctno = '" & gAcctNo & "' group by a.subacct order by c.venname")
		If Not CBool(dt.Rows.Count) Then
			Exit Sub
		Else
			cboVenName.Items.Add("")
			For Each dr As DataRow In dt.Rows
				cboVenName.Items.Add(dr.Item(0).ToString())
			Next
		End If

		dt.Dispose()

	End Sub

	Private Sub cboStat_TextChanged(sender As Object, e As EventArgs) Handles cboStat.TextChanged
		FillSumList()
	End Sub

	Private Sub dgvSum_SelectedIndexChanged(sender As Object, e As EventArgs) Handles dgvSum.SelectedIndexChanged

		FillGLentry()
		FillDataToClose()

	End Sub

	Private Sub FillGLentry()
		dt = GetDataTable("select sum(ifnull(dramt,0)),sum(ifnull(cramt,0)) from gltranstbl where " &
						  "docno = '" & dgvSum.SelectedRow.Cells(1).Text & "' and " &
						  "tc = '" & dgvSum.SelectedRow.Cells(2).Text & "' group by docno")
		If Not CBool(dt.Rows.Count) Then
			dgvGLentry.DataSource = Nothing
			dgvGLentry.DataBind()
			lblDRamtTot.Text = "0.00"
			lblCRamtTot.Text = "0.00"
			Exit Sub
		Else
			For Each dr As DataRow In dt.Rows
				lblDRamtTot.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0.00")
				lblCRamtTot.Text = Format(CDbl(dr.Item(1).ToString()), "#,##0.00")
			Next

		End If

		dt.Dispose()

		Dim adapter As New MySqlDataAdapter
		Dim ds As New DataSet()
		sqldata = Nothing

		sqldata = "select concat(a.acctno,space(1),b.acctdesc) as acctno,concat(a.ccno,space(1),c.ccdesc) as ccno," &
				  "ifnull(a.dramt,0) as dramt,ifnull(a.cramt,0) as cramt,a.pk,ifnull(a.subacct,'00000') as subacct " &
				  "from gltranstbl a left join acctcharttbl b on a.acctno=b.acctno left join cctrnotbl c on a.ccno=c.ccno " &
				  "where a.docno = '" & dgvSum.SelectedRow.Cells(1).Text & "' and " &
				  "a.tc = '" & dgvSum.SelectedRow.Cells(2).Text & "' group by a.id"

		conn.Open()
		Dim command As New MySqlCommand(sqldata, conn)
		adapter.SelectCommand = command
		adapter.Fill(ds)
		adapter.Dispose()
		command.Dispose()
		conn.Close()
		dgvGLentry.DataSource = ds.Tables(0)
		dgvGLentry.DataBind()

	End Sub

	Private Sub lbStat_Click(sender As Object, e As EventArgs) Handles lbStat.Click
		If dpTransDate1.Text = Nothing Then
			Exit Sub
		ElseIf dpTransDate2.Text = Nothing Then
			Exit Sub
		ElseIf Format(CDate(dpTransDate1.Text), "yyyy-MM-dd") > Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") Then
			Exit Sub
		End If

		popStatList()

	End Sub

	Private Sub CheckBox2_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox2.CheckedChanged
		If CheckBox2.Checked = True Then
			CheckBox2.Text = "Multi Vendor"
			Panel5.Visible = True
			lblPopUpLabel.Text = "Total CA Amount:"
			If txtREfNo.Text <> "" Then
				FilldgvVendorMulti()
			End If

		Else
			CheckBox2.Text = "Single Vendor"
			Panel5.Visible = False
		End If

	End Sub

	Private Sub FilldgvVendorMulti()
		dt = GetDataTable("select sum(ifnull(dramt,0)) from expdettbl where docno = '" & txtREfNo.Text & "' " &
						  "and acctno = '" & lblFormName.Text.Substring(0, 7) & "' group by acctno")
		If Not CBool(dt.Rows.Count) Then
			dgvMMRRlist.DataSource = Nothing
			txtAmtSelect.Text = "0.00"
			Exit Sub
		Else
			For Each dr As DataRow In dt.Rows
				txtAmtSelect.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0.00")
			Next

		End If

		dt.Dispose()

		Dim adapter As New MySqlDataAdapter
		Dim ds As New DataSet()
		sqldata = Nothing

		sqldata = "select a.venno,c.venname,sum(ifnull(a.dramt,0)) as dramt from expdettbl a " &
					"left join venmasttbl c on a.venno=c.venno where a.docno = '" & txtREfNo.Text & "' " &
					"and a.acctno = '" & lblFormName.Text.Substring(0, 7) & "' and a.tc = '42' " &
					"group by a.venno order by a.itemno"

		'With dgvMMRRlist
		'	.DataSource = MyDataSet.Tables("gkotbl")
		'	.Columns(0).HeaderText = "Vendor No."
		'	.Columns(1).HeaderText = "Vendor Name"
		'	.Columns(2).HeaderText = "Amount"

		'End With

		conn.Open()
		Dim command As New MySqlCommand(sqldata, conn)
		adapter.SelectCommand = command
		adapter.Fill(ds)
		adapter.Dispose()
		command.Dispose()
		conn.Close()
		dgvMMRRlist.DataSource = ds.Tables(0)
		dgvMMRRlist.DataBind()


		'dt = GetDataTable("select sum(ifnull(dramt,0)) from expdettbl where docno = '" & txtREfNo.Text & "' " &
		'				  "and acctno = '" & gAcctNo & "' group by acctno")
		'If Not CBool(dt.Rows.Count) Then
		'	dgvMMRRlist.DataSource = Nothing
		'	txtAmtSelect.Text = "0.00"
		'	Exit Sub
		'Else
		'	For Each dr As DataRow In dt.Rows
		'		txtAmtSelect.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0.00")
		'	Next

		'End If

		'dt.Dispose()

	End Sub

	Private Sub cboCCtr_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboCCtr.SelectedIndexChanged
		If cboCCtr.Text = "" Then
			Exit Sub
		End If

		popGLDesc()

	End Sub

	Private Sub popGLDesc()
		cboGLdesc.Items.Clear()
		dt = GetDataTable("select concat(a.acctno,space(1),a.acctdesc) from acctcharttbl a " &
						  "left join cctrnotbl b on a.acctnopre=b.acctnopre where " &
						  "b.ccno = '" & cboCCtr.Text.Substring(0, 5) & "' order by a.acctdesc") ' and a.baxchrgs = '" & "8888" & "'
		If Not CBool(dt.Rows.Count) Then
			tsErrMsg.Text = ("Chart of Acct Not found.")
			Exit Sub
		Else
			cboGLdesc.Items.Add("")
			For Each dr As DataRow In dt.Rows
				cboGLdesc.Items.Add(dr.Item(0).ToString())

			Next
		End If

		Call dt.Dispose()

	End Sub

	Private Sub txtDRamt_TextChanged(sender As Object, e As EventArgs) Handles txtDRamt.TextChanged
		Dim DRAmt As Double = CDbl(IIf(txtDRamt.Text = "", 0, txtDRamt.Text))
		txtDRamt.Text = Format(DRAmt, "#,##0.00")
		txtCRamt.Focus()
		getPKdr()

	End Sub

	Private Sub getPKdr()

		dt = GetDataTable("select subacct,upperacct from acctcharttbl where acctno = '" & cboGLdesc.Text.Substring(0, 7) & "'")
		If Not CBool(dt.Rows.Count) Then
			tsErrMsg.Text = "Chart of Acct Not found."
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

						Case Else
							lblPK.Text = "40"

					End Select

				Else
					Exit Sub

				End If

			Next
		End If

		Call dt.Dispose()

	End Sub

	Private Sub getPKcr()

		dt = GetDataTable("select subacct,upperacct from acctcharttbl where acctno = '" & cboGLdesc.Text.Substring(0, 7) & "'")
		If Not CBool(dt.Rows.Count) Then
			tsErrMsg.Text = "Chart of Acct Not found."
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

						Case Else
							lblPK.Text = "50"

					End Select

				Else
					Exit Sub

				End If

			Next

		End If

		Call dt.Dispose()

	End Sub

	Private Sub txtCRamt_TextChanged(sender As Object, e As EventArgs) Handles txtCRamt.TextChanged
		Dim CRAmt As Double = CDbl(IIf(txtCRamt.Text = "", 0, txtCRamt.Text))
		txtCRamt.Text = Format(CRAmt, "##,##0.00")

		If lblSubAcct.Text = "Y" Then
			txtSubAcct.Focus()
		Else

		End If

		getPKdr()

	End Sub

	Private Sub cboGLdesc_TextChanged(sender As Object, e As EventArgs) Handles cboGLdesc.TextChanged
		If cboGLdesc.Text = "" Then
			Exit Sub
		End If

		dt = GetDataTable("select subacct,cv from acctcharttbl where acctno = '" & cboGLdesc.Text.Substring(0, 7) & "'")
		If Not CBool(dt.Rows.Count) Then
			tsErrMsg.Text = "Chart of Acct Not found."
			Exit Sub
		End If

		For Each dr As DataRow In dt.Rows
			lblSubAcct.Text = dr.Item(0).ToString()
			strCV = dr.Item(1).ToString()

		Next

		Call dt.Dispose()

		txtDRamt.Focus()

		If lblSubAcct.Text = "N" Then
			txtSubAcct.Text = "20000"

		End If

	End Sub

	Private Sub btnAdd_Click(sender As Object, e As ImageClickEventArgs) Handles btnAdd.Click
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

		getPKdr()
		getPKcr()

		AddLineItemProc()
		BalanceCheck()
		JVlineItemClr()
		FillDGVjvdet()

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

	Protected Sub AddLineItemProc()
		If txtJVNo.Text = Nothing And lblLineItm.Text = "New" Then
			sql = "delete from tempgltranstbl where user = '" & lblUser.Text & "'"
			ExecuteNonQuery(sql)

		End If

		dt = GetDataTable("select * from tempgltranstbl where transyear = " & txtItm.Text & " and " &
						  "user = '" & lblUser.Text & "'")
		If Not CBool(dt.Rows.Count) Then 'strCV
			sql = "insert into tempgltranstbl(transyear,ccno,acctno,dramt,cramt,balamt,pk,subacct,cv,user)values" &
				  "(" & txtItm.Text & ",'" & cboCCtr.Text.Substring(0, 5) & "','" & cboGLdesc.Text.Substring(0, 7) & "'," &
				  CDbl(IIf(txtDRamt.Text = "", 0, txtDRamt.Text)) & "," & CDbl(IIf(txtCRamt.Text = "", 0, txtCRamt.Text)) & "," &
				  CDbl(IIf(txtDRamt.Text = "", 0, txtDRamt.Text)) & " - " & CDbl(IIf(txtCRamt.Text = "", 0, txtCRamt.Text)) & "," &
				  "'" & lblPK.Text & "','" & txtSubAcct.Text & "','" & strCV & "','" & lblUser.Text & "')"
			ExecuteNonQuery(sql)

		Else
			sql = "update tempgltranstbl set ccno = '" & cboCCtr.Text.Substring(0, 5) & "',acctno = '" & cboGLdesc.Text.Substring(0, 7) & "'," &
				  "dramt = " & CDbl(IIf(txtDRamt.Text = "", 0, txtDRamt.Text)) & ",cramt = " & CDbl(IIf(txtCRamt.Text = "", 0, txtCRamt.Text)) & "," &
				  "balamt = " & CDbl(IIf(txtDRamt.Text = "", 0, txtDRamt.Text)) & " - " & CDbl(IIf(txtCRamt.Text = "", 0, txtCRamt.Text)) & "," &
				  "pk = '" & lblPK.Text & "',subacct = '" & txtSubAcct.Text & "',cv = '" & strCV & "' where " &
				  "transyear = " & txtItm.Text & " and user = '" & lblUser.Text & "'"
			ExecuteNonQuery(sql)

		End If

		lblLineItm.Text = "On Process"

	End Sub

	Protected Sub BalanceCheck()
		Dim Bals As Double
		Bals = Math.Round(CDbl(IIf(lblDRtot.Text = "", 0, lblDRtot.Text)) - CDbl(IIf(lblCRtot.Text = "", 0, lblCRtot.Text)) - CDbl(IIf(txtVPamt.Text = "", 0, txtVPamt.Text)), 2)
		lblBalAmt.Text = Format(Bals, "#,##0.00")

		If Bals = 0 Then
			vThisFormCode = "023"
			If IsAllowed(lblGrpUser.Text, vThisFormCode, 3) = True Then ' 3 = Insert 
				lbSave.Enabled = True

			Else
				lbSave.Enabled = False

			End If

		End If

	End Sub

	Private Sub lbClose_Click(sender As Object, e As EventArgs) Handles lbClose.Click
		Response.Redirect("FinancialAccounting.aspx")
	End Sub
End Class