Imports MySql.Data.MySqlClient
Imports System
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports MySql.Data

Public Class JVPosting
	Inherits System.Web.UI.Page
	Dim dt As DataTable
	Dim sql As String
	Dim sqldata As String
	Dim MyDA_conn As New MySqlDataAdapter
	Dim MyDataSet As New DataSet
	Dim MySqlScript As String
	Dim MySQLtbl As String

	Protected Sub CheckGroupRights()
		If IsAllowed(lblGrpUser.Text, vThisFormCode, 2) = True Then 'edit all
			cboStat.Items.Clear()
			cboStat.Items.Add("Post")

		Else

		End If

		If IsAllowed(lblGrpUser.Text, vThisFormCode, 3) = True Then ' 3 = Insert 
			cboStat.Items.Add("Park")

		Else

		End If


		If IsAllowed(lblGrpUser.Text, vThisFormCode, 4) = True Then ' 4 = Delete
			'cboStat.Items.Add("Void")

		Else

		End If

		If IsAllowed(lblGrpUser.Text, vThisFormCode, 5) = True Then ' 4 = Delete
			CheckBox1.Visible = True

		Else
			CheckBox1.Visible = False

		End If

	End Sub

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		If lblUser.Text Is Nothing Then
			Response.Redirect("login.aspx")
			Exit Sub

		End If

		If Not Me.IsPostBack Then
			lblUser.Text = Session("UserID")
			lblGrpUser.Text = Session("UserGrp")

		End If

		vThisFormCode = "038"
		Call CheckGroupRights()
		GetCurrOpenMon()

	End Sub

	Protected Sub DgvJVdet_RowDataBound(sender As Object, e As GridViewRowEventArgs)

	End Sub

	Protected Sub DgvJVdet_RowCreated(sender As Object, e As GridViewRowEventArgs)

	End Sub

	Protected Sub DgvJVhdr_RowDataBound(sender As Object, e As GridViewRowEventArgs)

	End Sub

	Protected Sub DgvJVhdr_RowCreated(sender As Object, e As GridViewRowEventArgs)

	End Sub

	Private Sub cboStatList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboStatList.SelectedIndexChanged
		If cboStatList.Text = "" Then
			Exit Sub
		ElseIf dpFrom.Text = Nothing Then
			Exit Sub
		ElseIf dpTo.Text = Nothing Then
			Exit Sub
		End If

		FillLVdet()

	End Sub

	Protected Sub FillLVdet()

		DgvJVhdr.DataSource = Nothing
		DgvJVhdr.DataBind()

		dt = GetDataTable("select * from gljvhdrtbl where transdate between '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "'  and " &
						  "'" & Format(CDate(dpTo.Text), "yyyy-MM-dd") & "' and status = '" & cboStatList.Text & "'")
		If Not CBool(dt.Rows.Count) Then
			DgvJVhdr.DataSource = Nothing
			DgvJVhdr.DataBind()
			Exit Sub

		End If

		dt.Dispose()

		Dim adapter As New MySqlDataAdapter
		Dim ds As New DataSet()
		'Dim i As Integer = 0
		sqldata = Nothing

		sqldata = "select jvno,transdate,jvtype,sourcedoc,ifnull(dramt,0) as dramt,ifnull(cramt,0) as cramt,remarks," &
				  "user,status from gljvhdrtbl where transdate between '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' " &
				  "and '" & Format(CDate(dpTo.Text), "yyyy-MM-dd") & "' and status = '" & cboStatList.Text & "' " &
				  "order by jvno"

		conn.Open()
		Dim command As New MySqlCommand(sqldata, conn)
		adapter.SelectCommand = command
		adapter.Fill(ds)
		adapter.Dispose()
		command.Dispose()
		conn.Close()
		DgvJVhdr.DataSource = ds.Tables(0)
		DgvJVhdr.DataBind()

	End Sub

	Private Sub DgvJVhdr_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DgvJVhdr.SelectedIndexChanged
		txtJVNo.Text = DgvJVhdr.SelectedRow.Cells(1).Text
		dpPostDate.Text = Format(CDate(DgvJVhdr.SelectedRow.Cells(2).Text), "yyyy-MM-dd")
		lblDatePrep.Text = DgvJVhdr.SelectedRow.Cells(2).Text
		lblJVtype2.Text = DgvJVhdr.SelectedRow.Cells(3).Text
		lblJVtype.Text = DgvJVhdr.SelectedRow.Cells(4).Text
		lblRem.Text = DgvJVhdr.SelectedRow.Cells(7).Text
		lblPrepBy.Text = DgvJVhdr.SelectedRow.Cells(8).Text

		FillDGVjvDet()

		lblDateFr.Text = Format(CDate(FirstDayOfMonth(dpPostDate.Text)), "yyyy-MM-dd")
		lblDateTo.Text = Format(CDate(LastDayOfMonth(dpPostDate.Text)), "yyyy-MM-dd")
		DgvJVhdr.SelectedIndex = -1

	End Sub

	Protected Sub FillDGVjvDet()
		dt = GetDataTable("select * from gljvhdrtbl where jvno = '" & txtJVNo.Text & "'")
		If Not CBool(dt.Rows.Count) Then
			DgvJVdet.DataSource = Nothing
			DgvJVdet.DataBind()
			Exit Sub
		End If

		dt.Dispose()

		Dim adapter As New MySqlDataAdapter
		Dim ds As New DataSet()

		sqldata = Nothing

		sqldata = "select a.itmno,concat(a.ccno,space(1),b.ccdesc) as ccno,concat(a.acctno,space(1),c.acctdesc) as acctno," &
				  "ifnull(a.dramt,0) as dramt,ifnull(a.cramt,0) as cramt,a.pk,a.subacct from gljvdettbl a left join cctrnotbl b " &
				  "on a.ccno=b.ccno left join acctcharttbl c on a.acctno=c.acctno where jvno = '" & txtJVNo.Text & "' " &
				  "group by a.id order by a.itmno"

		conn.Open()
		Dim command As New MySqlCommand(sqldata, conn)
		adapter.SelectCommand = command
		adapter.Fill(ds)
		adapter.Dispose()
		command.Dispose()
		conn.Close()
		DgvJVdet.DataSource = ds.Tables(0)
		DgvJVdet.DataBind()

	End Sub

	Protected Sub ClrFields()
		txtJVNo.Text = ""
		lblPrepBy.Text = ""
		lblDatePrep.Text = ""
		lblRem.Text = ""
		cboStat.Text = Nothing
		'DgvJVhdr.SelectedRow.RowIndex = Nothing

	End Sub

	Private Sub dpFrom_TextChanged(sender As Object, e As EventArgs) Handles dpFrom.TextChanged
		Dim firstDay As Date = FirstDayOfMonth(CDate(dpFrom.Text))
		Dim lastDay As Date = LastDayOfMonth(CDate(dpFrom.Text))

		dpFrom.Text = Format(firstDay, "yyyy-MM-dd")
		dpTo.Text = Format(lastDay, "yyyy-MM-dd")
		dpPostDate.Text = Format(lastDay, "yyyy-MM-dd")

		lblDateFr.Text = Format(CDate(FirstDayOfMonth(dpPostDate.Text)), "yyyy-MM-dd")
		lblDateTo.Text = Format(CDate(LastDayOfMonth(dpPostDate.Text)), "yyyy-MM-dd")

	End Sub

	Protected Sub SaveDetProc()
		dt = GetDataTable("select * from glmaintranstbl where jvno = '" & txtJVNo.Text & "' and glstatus = '" & "open" & "'")
		If Not CBool(dt.Rows.Count) Then
			Call SaveDet()

		Else
			sql = "delete from glmaintranstbl where jvno = '" & txtJVNo.Text & "' and glstatus = '" & "open" & "'"
			ExecuteNonQuery(sql)

			SaveDet()

		End If

		Select Case UCase(lblJVtype.Text)
			Case "COSTING", "COSTING PC1", "COSTING PC2"
				sql = "update coshdrtbl set glstat = 'Processed' where jvno = '" & txtJVNo.Text & "'"
				ExecuteNonQuery(sql)

		End Select


	End Sub

	Protected Sub RevVoidDet()
		sql = "update gljvhdrtbl set status = '" & cboStat.Text & "' where jvno = '" & txtJVNo.Text & "'"
		ExecuteNonQuery(sql)

		sql = "delete from glmaintranstbl where jvno = '" & txtJVNo.Text & "' and glstatus = '" & "open" & "'"
		ExecuteNonQuery(sql)

		Select Case UCase(lblJVtype.Text)
			Case "COSTING", "COSTING PC1", "COSTING PC2"
				sql = "update coshdrtbl set glstat = null where jvno = '" & txtJVNo.Text & "'"
				ExecuteNonQuery(sql)

		End Select

	End Sub

	Protected Sub SaveDet()

		Dim strStat As String = "Open"
		'update gltrans
		sql = "update gltranstbl set transdate = '" & Format(CDate(dpPostDate.Text), "yyyy-MM-dd") & "'," &
			  "transyear = '" & Format(CDate(dpPostDate.Text), "yyyy") & "',transmon = '" & Format(CDate(dpPostDate.Text), "MM") & "' " &
			  "where jvno = '" & txtJVNo.Text & "'"
		ExecuteNonQuery(sql)

		sql = "update gljvdettbl set transyear = '" & Format(CDate(dpPostDate.Text), "yyyy") & "'," &
			  "transmon = '" & Format(CDate(dpPostDate.Text), "MM") & "' where jvno = '" & txtJVNo.Text & "'"
		ExecuteNonQuery(sql)

		sql = "update gljvhdrtbl set transdate = '" & Format(CDate(dpPostDate.Text), "yyyy-MM-dd") & "'," &
			  "transyear = '" & Format(CDate(dpPostDate.Text), "yyyy") & "',transmon = '" & Format(CDate(dpPostDate.Text), "MM") & "'," &
			  "dfrom = '" & Format(CDate(lblDateFr.Text), "yyyy-MM-dd") & "',dto = '" & Format(CDate(lblDateTo.Text), "yyyy-MM-dd") & "' " &
			  "where jvno = '" & txtJVNo.Text & "'"
		ExecuteNonQuery(sql)
		'new code end here 2021-02-26

		sql = "insert into glmaintranstbl(jvno,itmno,ccno,acctno,dramt,cramt,pk,subacct,transyear,transmon,user,jvsource," &
			  "yearend,glstatus,monstat,yearstat,dateposted,postedby,balamt,ba,branch,dfrom,dto,entrytype,jvtype,cos,pc," &
			  "prioryradjt) select a.jvno,a.itmno,a.ccno,a.acctno,a.dramt,a.cramt,a.pk,a.subacct,a.transyear,a.transmon," &
			  "a.user,a.sourcedoc,a.yearend,'" & strStat & "','" & strStat & "','" & strStat & "','" & Format(CDate(dpPostDate.Text), "yyyy-MM-dd") & "'," &
			  "'" & lblUser.Text & "',(ifnull(a.dramt,0)-ifnull(a.cramt,0)),'" & vLoggedBussArea & "','" & vLoggedBranch & "'," &
			  "b.dfrom,b.dto,a.entrytype,b.jvtype,a.cos,a.pc,a.prioryradjt from gljvdettbl a left join gljvhdrtbl b on a.jvno=b.jvno " &
			  "where a.jvno = '" & txtJVNo.Text & "' group by a.id"
		ExecuteNonQuery(sql)

		sql = "update glmaintranstbl a left join acctcharttbl b on a.acctno=b.acctno set a.upperacct=b.upperacct," &
			  "a.motheracct=b.motheracct,a.maingroup=b.maingroup,a.grpacct=b.grpacct where a.jvno = '" & txtJVNo.Text & "'"
		ExecuteNonQuery(sql)

		sql = "update glmaintranstbl a left join cctrnotbl b on a.ccno=b.ccno set a.pctrno=b.pctrno,a.ccno2=b.ccno2 where a.jvno = '" & txtJVNo.Text & "' " &
			  "and a.ccno <> '88888'"
		ExecuteNonQuery(sql)

		sql = "update gljvhdrtbl set status = '" & cboStat.Text & "',postby = '" & lblUser.Text & "'," &
			  "apppostdate = '" & Format(CDate(Now()), "yyyy-MM-dd HH:mm:ss") & "' where jvno = '" & txtJVNo.Text & "'"
		ExecuteNonQuery(sql)

		Select Case lblJVtype.Text
			Case "Manual JV" '
				sql = "update gltranstbl set glstatus = 'processed' where jvno = '" & txtJVNo.Text & "'"
				ExecuteNonQuery(sql)

		End Select

		sql = "update glmaintranstbl a,(select jvno,transdate from gljvhdrtbl where jvno = '" & txtJVNo.Text & "') as b " &
			  "set a.transdate = b.transdate where a.jvno = b.jvno and a.transdate is null"
		ExecuteNonQuery(sql)

		tssErrorMsg.Text = "JV No.: " & txtJVNo.Text & " Posted"

	End Sub

	Protected Sub GetCurrOpenMon()
		dt = GetDataTable("select dto from gltransmonstatus where yearstat = 'open' and monstat = 'open'")
		If Not CBool(dt.Rows.Count) Then
			Exit Sub
		Else
			For Each dr As DataRow In dt.Rows
				dpPostDate.Text = Format(CDate(dr.Item(0).ToString()), "yyyy-MM-dd")
			Next

		End If

		dt.Dispose()

		lblDateFr.Text = Format(CDate(FirstDayOfMonth(dpPostDate.Text)), "yyyy-MM-dd")
		lblDateTo.Text = Format(CDate(LastDayOfMonth(dpPostDate.Text)), "yyyy-MM-dd")

	End Sub

	Protected Sub OnConfirm(sender As Object, e As EventArgs)
		Dim confirmValue As String = Request.Form("confirm_value")
		If confirmValue = "Yes" Then
			MenuSaveProc()
		Else
			tssErrorMsg.Text = "Action Aborted"
		End If
	End Sub

	Protected Sub MenuSaveProc()
		If txtJVNo.Text = "" Then
			tssErrorMsg.Text = "Document No. is Blank"
			Exit Sub

		ElseIf cboStat.Text = "" Then
			cboStat.Focus()
			tssErrorMsg.Text = "Select Status"
			Exit Sub

		End If

		Select Case UCase(cboStat.Text)
			Case "POST"
				SaveDetProc()

			Case Else
				RevVoidDet()

				tssErrorMsg.Text = "JV No.: " & txtJVNo.Text & " Successfully " & UCase(cboStat.Text)

		End Select

		ClrFields()
		FillLVdet()

		DgvJVdet.DataSource = Nothing
		DgvJVdet.DataBind()


	End Sub

	Private Sub lbClose_Click(sender As Object, e As EventArgs) Handles lbClose.Click
		ClrFields()
		Response.Redirect("FinancialAccounting.aspx")

	End Sub

	Private Sub lbNew_Click(sender As Object, e As EventArgs) Handles lbNew.Click
		ClrFields()
		DgvJVdet.DataSource = Nothing
		DgvJVdet.DataBind()

		vThisFormCode = "038"
		Call CheckGroupRights()
	End Sub

	Private Sub dpPostDate_TextChanged(sender As Object, e As EventArgs) Handles dpPostDate.TextChanged
		If CheckBox1.Checked = True Then

		Else
			If CDate(dpPostDate.Text) < CDate(vTransMon) Then
				tssErrorMsg.Text = Format(CDate(dpPostDate.Text), "MMM yyyy") & " is Already CLOSED"
				dpPostDate.Text = Format(CDate(Now()), "yyyy-MM-dd")
				Exit Sub

			End If
		End If

		lblDateFr.Text = Format(CDate(FirstDayOfMonth(dpPostDate.Text)), "yyyy-MM-dd")
		lblDateTo.Text = Format(CDate(LastDayOfMonth(dpPostDate.Text)), "yyyy-MM-dd")

	End Sub

	Private Sub cboStat_TextChanged(sender As Object, e As EventArgs) Handles cboStat.TextChanged
		lbSave.Text = cboStat.Text
	End Sub

	Private Sub lbStatList_Click(sender As Object, e As EventArgs) Handles lbStatList.Click
		If cboStatList.Text = "" Then
			Exit Sub
		ElseIf dpFrom.Text = Nothing Then
			Exit Sub
		ElseIf dpTo.Text = Nothing Then
			Exit Sub
		End If

		FillLVdet()

	End Sub
End Class