Imports MySql.Data.MySqlClient
Imports System.IO
Imports System.Data.OleDb
Imports System.Data
Imports System.Data.DataTable
Imports System.Data.SqlTypes
Imports System
Imports System.Web
Imports System.Net.Http
Public Class MaterialManagement
	Inherits System.Web.UI.Page
	Dim dt As DataTable
	Dim sql As String
	Private vThisFormCode As String
	Dim MyDA_conn As New MySqlDataAdapter
	Dim MyDataSet As New DataSet
	Dim Answer As String
	Dim MySqlScript As String
	Dim admRepNo As String
	Dim admDocNo As String
	Dim admPreNo As String
	Dim MyDA_com_sections As New MySqlDataAdapter
	Dim admAdjDocNo As String
	Dim admCCno As String
	Dim admExcelFile As String
	Dim admBook1 As String = "C:\updater\Book1.xlsx"
	Dim sqldata As String
	Dim admDesc As String
	Dim qtyIss As Long
	Dim wtIss As Double
	Dim admMMtype As String

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		If vLoggedUserID Is Nothing Then
			Response.Redirect("login.aspx")

		End If

		If Not Me.IsPostBack Then
			lblUser.Text = Session("UserID")
			lblGrpUser.Text = Session("UserGrp")

		End If


	End Sub

	Private Sub lbInvReport_Click(sender As Object, e As EventArgs) Handles lbInvReport.Click
		If vLoggedUserID Is Nothing Then
			Response.Redirect("login.aspx")

		End If

		If IsAllowed(lblGrpUser.Text, "034", 1) = True Then
			Response.Redirect("InventoryReports.aspx")
		Else

			Response.Redirect("MaterialManagement.aspx")
		End If


	End Sub

	Private Sub lbIssuance_Click(sender As Object, e As EventArgs) Handles lbIssuance.Click
		If vLoggedUserID Is Nothing Then
			Response.Redirect("login.aspx")


		End If

		If IsAllowed(lblGrpUser.Text, "007", 1) = True Then
			Response.Redirect("DeliveryOrder.aspx")
		Else
			Response.Redirect("MaterialManagement.aspx")
		End If

	End Sub

	Private Sub lblMMregister_Click(sender As Object, e As EventArgs) Handles lblMMregister.Click
		If vLoggedUserID Is Nothing Then
			Response.Redirect("login.aspx")

		End If

		If IsAllowed(lblGrpUser.Text, "034", 1) = True Then
			Response.Redirect("MMReports.aspx")
		Else
			Response.Redirect("MaterialManagement.aspx")
		End If

	End Sub



	Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged,
			RadioButton2.CheckedChanged, RadioButton3.CheckedChanged, CheckBox1.CheckedChanged

		If RadioButton1.Checked = False And RadioButton2.Checked = False And RadioButton3.Checked = False Then
			Exit Sub

		End If

		Select Case True
			Case RadioButton1.Checked
				CheckBox1.Visible = False

			Case RadioButton2.Checked
				CheckBox1.Visible = False

			Case RadioButton3.Checked
				Select Case vLoggedBussArea
					Case "8300"
						CheckBox1.Visible = True

				End Select
		End Select

		fillInvAlert()

	End Sub

	Protected Sub fillInvAlert()
		sql = "delete from tempinvtbl where user = '" & lblUser.Text & "'"
		ExecuteNonQuery(sql)

		Select Case True
			Case RadioButton1.Checked 'RM
				admMMtype = RadioButton1.Text

			Case RadioButton2.Checked 'Pack
				admMMtype = RadioButton2.Text

			Case RadioButton3.Checked 'FG
				admMMtype = RadioButton3.Text

		End Select

		If CheckBox1.Checked = True Then

		Else
			sql = "insert into tempinvtbl(codeno,qty_in,wt_in,user) select a.codeno,ifnull(sum(a.qty),0)," &
				  "ifnull(sum(a.wt),0),'" & lblUser.Text & "' from invdettbl a left join invhdrtbl " &
					"b on a.mmrrno=b.mmrrno left join mmasttbl c on a.codeno=c.codeno where b.status <> 'void' and " &
				  "b.transdate <= '" & Format(CDate(Now()), "yyyy-MM-dd") & "' and c.mmtype = '" & admMMtype & "' group by a.codeno"
			ExecuteNonQuery(sql)

			sql = "insert into tempinvtbl(codeno,qty_in,wt_in,user) select a.codeno,ifnull(sum(a.qty),0)," &
				  "ifnull(sum(a.wt),0),'" & lblUser.Text & "' from wrrdettbl a left join wrrhdrtbl b on a.wrrno=b.wrrno " &
				  "left join mmasttbl c on a.codeno=c.codeno where c.mmtype = '" & admMMtype & "' and b.status <> 'void' and " &
				  "b.transdate <= '" & Format(CDate(Now()), "yyyy-MM-dd") & "' group by a.codeno"
			ExecuteNonQuery(sql)

			'insert issuance
			sql = "insert into tempinvtbl(codeno,qty_out,wt_out,user) select a.codeno,ifnull(sum(a.qty),0)," &
				  "ifnull(sum(a.wt),0),'" & lblUser.Text & "' from issdettbl a left join isshdrtbl b on a.dono=b.dono " &
				  "left join mmasttbl c on a.codeno=c.codeno where c.mmtype = '" & admMMtype & "' and b.status <> 'void' " &
				  "and b.transdate <= '" & Format(CDate(Now()), "yyyy-MM-dd") & "' group by a.codeno"
			ExecuteNonQuery(sql)

			''adjtable in
			sql = "insert into tempinvtbl(codeno,qty_in,wt_in,user,docno) select a.codeno,sum(ifnull(a.stdqty,0))," &
				  "sum(ifnull(a.stdwt,0)),'" & lblUser.Text & "','in' from invadjdettbl a left join invadjhdrtbl b on a.dono=b.dono " &
				  "left join mmasttbl c on a.codeno=c.codeno where c.mmtype = '" & admMMtype & "' and " &
				  "a.pono = 'PARK' and b.transdate <= '" & Format(CDate(Now()), "yyyy-MM-dd") & "' and a.tc = '10' group by a.codeno"
			ExecuteNonQuery(sql)
			'adj out
			sql = "insert into tempinvtbl(codeno,qty_out,wt_out,user,docno) select a.codeno,sum(ifnull(a.qty,0))," &
				  "sum(ifnull(a.wt,0)),'" & lblUser.Text & "','out' from invadjdettbl a left join invadjhdrtbl b on a.dono=b.dono " &
				  "left join mmasttbl c on a.codeno=c.codeno where c.mmtype = '" & admMMtype & "' and a.pono = 'PARK' and " &
				  "b.transdate <= '" & Format(CDate(Now()), "yyyy-MM-dd") & "' and a.tc = '30' group by a.codeno"
			ExecuteNonQuery(sql)

			dt = GetDataTable("select * from information_schema.tables where table_schema =  '" & vDbase & "' and " &
							  "table_name = 'tempinvtbl2'")
			If Not CBool(dt.Rows.Count) Then
				sql = "CREATE TABLE tempinvtbl2 LIKE tempinvtbl"
				ExecuteNonQuery(sql)

			Else
				sql = "delete from tempinvtbl2 where user = '" & lblUser.Text & "'"
				ExecuteNonQuery(sql)

			End If

			dt.Dispose()

			sql = "insert into tempinvtbl2(codeno,qty_in,wt_in,qty_out,wt_out,user) select codeno,sum(ifnull(qty_in,0))," &
				  "sum(ifnull(wt_in,0)),sum(ifnull(qty_out,0)),sum(ifnull(wt_out,0)),'" & lblUser.Text & "' from tempinvtbl " &
				  "where user = '" & lblUser.Text & "' group by codeno"
			ExecuteNonQuery(sql)

			sql = "update tempinvtbl2 set qty_bal = round(ifnull(qty_in,0)-ifnull(qty_out,0),0)," &
				  "wt_bal = round(ifnull(wt_in,0)-ifnull(wt_out,0),2) where user = '" & lblUser.Text & "'"
			ExecuteNonQuery(sql)

			sql = "update tempinvtbl2 a,(select codeno,ifnull(invlevel,0) as invlevel from mmasttbl where " &
				  "mmtype = '" & admMMtype & "') as b set a.uc = b.invlevel where a.codeno = b.codeno and " &
				  "a.user = '" & lblUser.Text & "'"
			ExecuteNonQuery(sql)

		End If

		dt = GetDataTable("select * from tempinvtbl2 where user = '" & lblUser.Text & "'")
		If Not CBool(dt.Rows.Count) Then
			'lblMsg.Text = "Error: No Inventory Found"
			dgvMMinv.DataSource = Nothing
			Exit Sub

		End If

		dt.Dispose()

		Dim adapter As New MySqlDataAdapter
		Dim ds As New DataSet()
		Dim i As Integer = 0
		sqldata = Nothing

		Select Case True
			Case RadioButton1.Checked 'RM
				sqldata = "select a.codeno,ifnull(b.codename,b.mmdesc) as mmdesc,a.qty_bal,a.wt_bal,ifnull(b.invlevel,0) as invlevel " &
						  "from tempinvtbl2 a left join mmasttbl b on a.codeno=b.codeno where a.user = '" & lblUser.Text & "' and " &
						  "ifnull(a.wt_bal,0) < ifnull(a.uc,0) order by ifnull(b.codename,b.mmdesc)"
			Case RadioButton2.Checked 'Pack
				sqldata = "select a.codeno,ifnull(b.codename,b.mmdesc) as mmdesc,a.qty_bal,a.wt_bal,ifnull(b.invlevel,0) as invlevel " &
						  "from tempinvtbl2 a left join mmasttbl b on a.codeno=b.codeno where a.user = '" & lblUser.Text & "' and " &
						  "ifnull(a.qty_bal,0) < ifnull(a.uc,0) order by ifnull(b.codename,b.mmdesc)"
			Case RadioButton3.Checked 'FG
				sqldata = "select a.codeno,ifnull(b.codename,b.mmdesc) as mmdesc,a.wt_bal,a.qty_bal,ifnull(b.invlevel,0) as invlevel " &
						  "from tempinvtbl2 a left join mmasttbl b on a.codeno=b.codeno where a.user = '" & lblUser.Text & "' and " &
						  "ifnull(a.wt_bal,0) < ifnull(a.uc,0) order by ifnull(b.codename,b.mmdesc)"
		End Select

		Select Case True
			Case RadioButton1.Checked
				dgvMMinv.Columns(2).Visible = False
				dgvMMinv.Columns(3).Visible = True

			Case RadioButton2.Checked
				dgvMMinv.Columns(2).Visible = True
				dgvMMinv.Columns(3).Visible = False

			Case RadioButton3.Checked
				dgvMMinv.Columns(2).Visible = True
				dgvMMinv.Columns(3).Visible = True

		End Select

		conn.Open()
		Dim command As New MySqlCommand(sqldata, conn)
		adapter.SelectCommand = command
		adapter.Fill(ds)
		adapter.Dispose()
		command.Dispose()
		conn.Close()
		dgvMMinv.DataSource = ds.Tables(0)
		dgvMMinv.DataBind()

	End Sub

	Protected Sub dgvMMinv_RowDataBound(sender As Object, e As GridViewRowEventArgs)

	End Sub

	Protected Sub dgvMMinv_RowCreated(sender As Object, e As GridViewRowEventArgs)

	End Sub
End Class