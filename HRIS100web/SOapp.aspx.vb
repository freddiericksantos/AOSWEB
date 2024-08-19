Imports MySql.Data.MySqlClient
Imports System
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports MySql.Data

Public Class SOapp
	Inherits System.Web.UI.Page
	Dim dt As DataTable
	Dim sql As String
	Dim sqldata As String
	'Dim Answer As String
	'Dim admDocType As String
	'Dim strReport As String
	'Dim strStat As String

	Protected Sub AdmMsgBox(ByVal sMessage As String)
		Dim msg As String
		msg = "<script language='javascript'>"
		msg += "alert('" & sMessage & "');"
		msg += "<" & "/script>"
		Response.Write(msg)
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

		lblTC.Text = "05"

		cboStat.Items.Clear()
		dt = GetDataTable("select appstat from authtbl where userid = '" & lblUser.Text & "' and form = 'soapp'")
		If Not CBool(dt.Rows.Count) Then
			Call AdmMsgBox("No Authorization found.")
			Exit Sub
		Else
			For Each dr As DataRow In dt.Rows
				cboStat.Items.Add(dr.Item(0).ToString())

			Next
		End If

		Call dt.Dispose()

		dpPostDate.Text = Format(CDate(Now()), "yyyy-MM-dd")

		If Not Me.IsPostBack Then
			PopLVdet()
		End If

	End Sub

	Protected Sub PopLVdet()

		dt = GetDataTable("select * from sohdrtbl a left join custmasttbl b on a.custno=b.custno where a.delstat = 'open' and " &
						  "a.status <> 'void' and a.apprvdby is null group by a.delstat")
		If Not CBool(dt.Rows.Count) Then
			DgvSOdet.DataSource = Nothing
			Exit Sub

		End If

		dt.Dispose()

		Dim adapter As New MySqlDataAdapter
		Dim ds As New DataSet()
		'Dim i As Integer = 0
		sqldata = Nothing

		sqldata = "select a.sono,a.transdate,concat(a.custno,space(1),b.bussname) as custno,a.soamt,a.status,a.totar,a.currbal,a.30days,a.60days,a.90days," &
				  "a.120days,a.91over from sohdrtbl a left join custmasttbl b on a.custno=b.custno where a.delstat = 'open' and " &
				  "a.status <> 'void' and a.apprvdby is null order by a.transdate desc"

		conn.Open()
		Dim command As New MySqlCommand(sqldata, conn)
		adapter.SelectCommand = command
		adapter.Fill(ds)
		adapter.Dispose()
		command.Dispose()
		conn.Close()
		DgvSOdet.DataSource = ds.Tables(0)
		DgvSOdet.DataBind()


	End Sub

	Protected Sub lbNew_Click(sender As Object, e As EventArgs)
		ClrFields()
		PopLVdet()

	End Sub

	Protected Sub ClrFields()
		txtSOno.Text = ""
		lblCust.Text = ""
		lblCRlimit.Text = "0.00"
		lblSOamt.Text = "0.00"
		lblTerm.Text = ""

	End Sub

	Protected Sub OnConfirm(sender As Object, e As EventArgs)
		If cboStat.Text = "" Then
			cboStat.Focus()
			AdmMsgBox("Select Status")
			Exit Sub

		ElseIf txtSOno.Text = "" Then
			AdmMsgBox("Select SO to Approve")
			Exit Sub

		End If

		Dim confirmValue As String = Request.Form("confirm_value")
		If confirmValue = "Yes" Then
			UpdateSave()
			PopLVdet()
			ClrFields()

		Else
			AdmMsgBox("Action Aborted....")
		End If

	End Sub

	'Protected Sub lbSave_Click(sender As Object, e As EventArgs)
	'	If cboStat.Text = "" Then
	'		cboStat.Focus()
	'		AdmMsgBox("Select Status")
	'		Exit Sub

	'	ElseIf txtSONo.Text = "" Then
	'		AdmMsgBox("Select SO to Approve")
	'		Exit Sub

	'	End If

	'	Answer = MsgBox("Are you Sure to UPDATE Status to " & cboStat.Text & "?", vbExclamation + vbYesNo)
	'	If Answer = vbYes Then
	'		UpdateSave()
	'		PopLVdet()
	'		ClrFields()

	'	Else
	'		Exit Sub

	'	End If


	'End Sub

	Protected Sub UpdateSave()
		sql = "update sohdrtbl set status = '" & cboStat.Text & "',apprvdby = '" & lblUser.Text & "'," &
			  "apprdate = '" & Format(CDate(Now()), "yyyy-MM-dd HH:mm:ss") & "' where sono = '" & txtSOno.Text & "'"
		ExecuteNonQuery(sql)

	End Sub

	Protected Sub lbPrint_Click(sender As Object, e As EventArgs)

	End Sub

	Protected Sub lbDelete_Click(sender As Object, e As EventArgs)

	End Sub

	Protected Sub lbClose_Click(sender As Object, e As EventArgs)
		Response.Redirect("SalesAndDist.aspx")

	End Sub

	Protected Sub DgvSOdet_RowDataBound(sender As Object, e As GridViewRowEventArgs)

	End Sub

	Protected Sub DgvSOdet_RowCreated(sender As Object, e As GridViewRowEventArgs)

	End Sub

	Private Sub DgvSOdet_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DgvSOdet.SelectedIndexChanged
		txtSOno.Text = DgvSOdet.SelectedRow.Cells(1).Text
		lblCust.Text = DgvSOdet.SelectedRow.Cells(3).Text
		lblSOamt.Text = Format(CDbl(DgvSOdet.SelectedRow.Cells(4).Text), "#,##0.00")
		GetCustDetails()

	End Sub

	Protected Sub GetCustDetails()
		dt = GetDataTable("select ifnull(crlimit,0),ifnull(term,'0') from custmasttbl where custno = '" & lblCust.Text.Substring(0, 5) & "'")
		If Not CBool(dt.Rows.Count) Then
			Call MsgBox("Customer Not found.")
			Exit Sub
		Else
			For Each dr As DataRow In dt.Rows
				lblCRlimit.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0.00")
				lblTerm.Text = dr.Item(1).ToString() & ""

			Next

		End If

		Call dt.Dispose()

	End Sub

End Class