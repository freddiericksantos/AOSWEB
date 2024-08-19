Imports MySql.Data.MySqlClient
Imports System.IO
Imports System.Data.OleDb
Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing

Public Class aRreports
	Inherits System.Web.UI.Page
	Dim dt As DataTable
	Dim sql As String
	Dim sqldata As String
	Dim DR As Double = 0
	Dim CR As Double = 0
	Dim Bal As Double = 0
	Dim curr As Double = 0
	Dim d30 As Double = 0
	Dim d60 As Double = 0
	Dim d90 As Double = 0
	Dim d120 As Double = 0
	Dim d150 As Double = 0
	Dim d180 As Double = 0
	Dim d121 As Double = 0
	Dim d91 As Double = 0
	Dim pdc As Double = 0
	Dim net As Double = 0
	Dim custno As Integer = 0
	Dim sDR As Double = 0
	Dim sCR As Double = 0
	Dim sBal As Double = 0
	Dim scurr As Double = 0
	Dim sd30 As Double = 0
	Dim sd60 As Double = 0
	Dim sd90 As Double = 0
	Dim sd120 As Double = 0
	Dim sd150 As Double = 0
	Dim sd180 As Double = 0
	Dim sd121 As Double = 0
	Dim sd91 As Double = 0
	Dim spdc As Double = 0
	Dim snet As Double = 0
	Dim shipto As Integer = 0
	Dim storid As Integer = 0
	Dim rowIndex As Integer = 1
	Dim TotRowIndex As Integer = 1
	Dim strCustName As String
	Dim strShipName As String
	Dim tDR As Double = 0
	Dim tCR As Double = 0
	Dim tBal As Double = 0
	Dim tcurr As Double = 0
	Dim td30 As Double = 0
	Dim td60 As Double = 0
	Dim td90 As Double = 0
	Dim td120 As Double = 0
	Dim td150 As Double = 0
	Dim td180 As Double = 0
	Dim td121 As Double = 0
	Dim td91 As Double = 0
	Dim tpdc As Double = 0
	Dim tnet As Double = 0
	Dim SP As Double
	Dim Amt As Double
	Dim NetAmt As Double
	Dim DiscAmt As Double
	Dim DiscRate As Double
	Dim GrossAmt As Double
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

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		If lblUser.Text Is Nothing Then
			Response.Redirect("login.aspx")

		End If

		If Not Me.IsPostBack Then
			lblUser.Text = Session("UserID")
			lblGrpUser.Text = Session("UserGrp")
			PopFormat()
		End If

	End Sub

	Private Sub PopFormat()
		cboFormat.Items.Clear()
		cboFormat.Items.Add("")
		cboFormat.Items.Add("ALL")
		cboFormat.Items.Add("Customer")
		cboFormat.Items.Add("Salesman")

	End Sub

	Protected Sub lbHome_Click(sender As Object, e As EventArgs)
		Response.Redirect("Home.aspx")
	End Sub

	Protected Sub lbReset_Click(sender As Object, e As EventArgs)
		'FillDGVreports1()

	End Sub

	Protected Sub lbPrint_Click(sender As Object, e As EventArgs)

		AdmMsgBox("Not Yet Available")

	End Sub

	Protected Sub lbExcel_Click(sender As Object, e As EventArgs)
		AdmMsgBox("Not Yet Available")

	End Sub

	Protected Sub lbClose_Click(sender As Object, e As EventArgs)
		Response.Redirect("FinancialAccounting.aspx")
	End Sub

	'Protected Sub OnItemDataBound(ByVal sender As Object, ByVal e As RepeaterItemEventArgs)

	'End Sub

	'Protected Sub OnItemDataBound1(ByVal sender As Object, ByVal e As RepeaterItemEventArgs)

	'End Sub

	'Protected Sub OnRowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)

	'End Sub

	'Protected Sub OnRowCreated(ByVal sender As Object, ByVal e As GridViewRowEventArgs)

	'End Sub

	'Protected Sub OnDataBound(ByVal sender As Object, ByVal e As EventArgs)

	'End Sub

	'Private Sub AddTotalRow(ByVal GridView1 As GridView, ByVal labelText As String, ByVal value As String)

	'End Sub

	'Private Shared Function GetData(ByVal query As String) As DataTable
	'	Dim constr As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString
	'	Using con As SqlConnection = New SqlConnection(constr)
	'		Using cmd As SqlCommand = New SqlCommand()
	'			cmd.CommandText = query
	'			Using sda As SqlDataAdapter = New SqlDataAdapter()
	'				cmd.Connection = con
	'				sda.SelectCommand = cmd
	'				Using ds As DataSet = New DataSet()
	'					Dim dt As DataTable = New DataTable()
	'					sda.Fill(dt)
	'					Return dt
	'				End Using
	'			End Using
	'		End Using
	'	End Using
	'End Function

	Protected Sub FillDGVreports1()
		'Dim adapter As New MySqlDataAdapter
		'Dim ds As New DataSet()
		''Dim i As Integer = 0
		'MySqlScript = Nothing

		'MySqlScript = "select a.codeno,ifnull(c.codename,c.mmdesc) as mmdesc,sum(ifnull(a.qty,0)) as qty,sum(ifnull(a.wt,0)) as wt " &
		'              "from issdettbl a inner join isshdrtbl b on a.dono=b.dono inner join mmasttbl c on a.codeno=c.codeno " &
		'              "where b.mov = '601' and b.status <> 'void' and b.transdate between '2022-01-01' and " &
		'              "'2022-01-31' group by a.codeno order by wt desc"

		'conn.Open()
		'Dim command As New MySqlCommand(MySqlScript, conn)
		'adapter.SelectCommand = command
		'adapter.Fill(ds)
		'adapter.Dispose()
		'command.Dispose()
		'conn.Close()
		''dgvARreports.DataSource = ds.Tables(0)
		''dgvARreports.DataBind()

		'With dgvARreports
		'    .DataSource = ds.Tables(0)
		'    .DataBind()


		'End With



	End Sub

	Protected Sub dgvARreport_RowDataBound(sender As Object, e As GridViewRowEventArgs)
		If e.Row.RowType = DataControlRowType.DataRow Then
			'***Ship to
			shipto = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "shipto").ToString())
			strShipName = DataBinder.Eval(e.Row.DataItem, "shiptoname").ToString()

			Dim stBalc As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "totalar").ToString())
			sBal += stBalc
			Dim std30c As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "30d").ToString())
			sd30 += std30c
			Dim std60c As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "60d").ToString())
			sd60 += std60c
			Dim std90c As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "90d").ToString())
			sd90 += std90c
			Dim std120c As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "120d").ToString())
			sd120 += std120c
			Dim std150c As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "150d").ToString())
			sd150 += std150c
			Dim std180c As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "180d").ToString())
			sd180 += std180c
			Dim std121c As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "121d").ToString())
			sd121 += std121c
			Dim std91c As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "91d").ToString())
			sd91 += std91c

			'***customer

			Dim tBalc2 As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "totalar").ToString())
			tBal += tBalc2
			Dim td30c2 As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "30d").ToString())
			td30 += td30c2
			Dim td60c2 As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "60d").ToString())
			td60 += td60c2
			Dim td90c2 As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "90d").ToString())
			td90 += td90c2
			Dim td120c2 As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "120d").ToString())
			td120 += td120c2
			Dim td150c2 As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "150d").ToString())
			td150 += td150c2
			Dim td180c2 As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "180d").ToString())
			td180 += td180c2
			Dim td121c2 As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "121d").ToString())
			td121 += td121c2
			Dim td91c2 As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "91d").ToString())
			td91 += td91c2

			custno = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "custno").ToString())
			strCustName = DataBinder.Eval(e.Row.DataItem, "custname").ToString()

			Dim tBalc As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "totalar").ToString())
			Bal += tBalc
			Dim td30c As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "30d").ToString())
			d30 += td30c
			Dim td60c As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "60d").ToString())
			d60 += td60c
			Dim td90c As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "90d").ToString())
			d90 += td90c
			Dim td120c As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "120d").ToString())
			d120 += td120c
			Dim td150c As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "150d").ToString())
			d150 += td150c
			Dim td180c As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "180d").ToString())
			d180 += td180c
			Dim td121c As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "121d").ToString())
			d121 += td121c
			Dim td91c As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "91d").ToString())
			d91 += td91c



		End If

		Dim newRow As Boolean = False
		If e.Row.RowType = DataControlRowType.Footer Then
			Dim lblGTotal As Label = DirectCast(e.Row.FindControl("lblGTotal"), Label)
			lblGTotal.Text = "GRAND TOTAL"

			Dim lblTotalBal As Label = DirectCast(e.Row.FindControl("lblTotalNet"), Label)
			lblTotalBal.Text = Format(CDbl(tBal.ToString()), "#,##0.00 ; (#,##0.00)")
			tBal = 0

			Dim lblTotal30d As Label = DirectCast(e.Row.FindControl("lblTotal30d"), Label)
			lblTotal30d.Text = Format(CDbl(td30.ToString()), "#,##0.00 ; (#,##0.00)")
			td30 = 0

			Dim lblTotal60d As Label = DirectCast(e.Row.FindControl("lblTotal60d"), Label)
			lblTotal60d.Text = Format(CDbl(td60.ToString()), "#,##0.00 ; (#,##0.00)")
			td60 = 0

			Dim lblTotal90d As Label = DirectCast(e.Row.FindControl("lblTotal90d"), Label)
			lblTotal90d.Text = Format(CDbl(td90.ToString()), "#,##0.00 ; (#,##0.00)")
			td90 = 0

			Dim lblTotal120d As Label = DirectCast(e.Row.FindControl("lblTotal120d"), Label)
			lblTotal120d.Text = Format(CDbl(td120.ToString()), "#,##0.00 ; (#,##0.00)")
			td120 = 0

			Dim lblTotal150d As Label = DirectCast(e.Row.FindControl("lblTotal150d"), Label)
			lblTotal150d.Text = Format(CDbl(td150.ToString()), "#,##0.00 ; (#,##0.00)")
			td150 = 0

			Dim lblTotal180d As Label = DirectCast(e.Row.FindControl("lblTotal180d"), Label)
			lblTotal180d.Text = Format(CDbl(td180.ToString()), "#,##0.00 ; (#,##0.00)")
			td180 = 0

			Dim lblTotal121d As Label = DirectCast(e.Row.FindControl("lblTotal121d"), Label)
			lblTotal121d.Text = Format(CDbl(td121.ToString()), "#,##0.00 ; (#,##0.00)")
			td121 = 0

			Dim lblTotal91d As Label = DirectCast(e.Row.FindControl("lblTotal91d"), Label)
			lblTotal91d.Text = Format(CDbl(td91.ToString()), "#,##0.00 ; (#,##0.00)")
			td91 = 0

		End If
	End Sub

	Protected Sub dgvARreport_RowCreated(sender As Object, e As GridViewRowEventArgs)

		Dim newRow As Boolean = False

		If (custno > 0) AndAlso (DataBinder.Eval(e.Row.DataItem, "custno") IsNot Nothing) Then
			If custno <> Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "custno").ToString()) Then
				newRow = True
			End If
		End If
		If (custno > 0) AndAlso (DataBinder.Eval(e.Row.DataItem, "custno") Is Nothing) Then
			newRow = True
			rowIndex = 0
		End If

		If newRow Then
			Dim dgvARreport As GridView = DirectCast(sender, GridView)
			Dim NewTotalRow As New GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert)
			NewTotalRow.Font.Bold = True
			NewTotalRow.BackColor = System.Drawing.Color.Gray
			NewTotalRow.ForeColor = System.Drawing.Color.White

			Dim HeaderCell As New TableCell()
			HeaderCell.Text = custno & Space(1) & strCustName & " Total"
			HeaderCell.HorizontalAlign = HorizontalAlign.Center
			HeaderCell.Font.Size = 8
			HeaderCell.ColumnSpan = 8
			NewTotalRow.Cells.Add(HeaderCell)

			HeaderCell = New TableCell()
			HeaderCell.HorizontalAlign = HorizontalAlign.Right
			HeaderCell.Text = Format(CDbl(Bal.ToString()), "#,##0.00 ; (#,##0.00)")
			NewTotalRow.Cells.Add(HeaderCell)

			HeaderCell = New TableCell()
			HeaderCell.HorizontalAlign = HorizontalAlign.Right
			HeaderCell.Text = Format(CDbl(d30.ToString()), "#,##0.00 ; (#,##0.00)")
			NewTotalRow.Cells.Add(HeaderCell)

			HeaderCell = New TableCell()
			HeaderCell.HorizontalAlign = HorizontalAlign.Right
			HeaderCell.Text = Format(CDbl(d60.ToString()), "#,##0.00 ; (#,##0.00)")
			NewTotalRow.Cells.Add(HeaderCell)

			HeaderCell = New TableCell()
			HeaderCell.HorizontalAlign = HorizontalAlign.Right
			HeaderCell.Text = Format(CDbl(d90.ToString()), "#,##0.00 ; (#,##0.00)")
			NewTotalRow.Cells.Add(HeaderCell)

			HeaderCell = New TableCell()
			HeaderCell.HorizontalAlign = HorizontalAlign.Right
			HeaderCell.Text = Format(CDbl(d120.ToString()), "#,##0.00 ; (#,##0.00)")
			NewTotalRow.Cells.Add(HeaderCell)

			HeaderCell = New TableCell()
			HeaderCell.HorizontalAlign = HorizontalAlign.Right
			HeaderCell.Text = Format(CDbl(d150.ToString()), "#,##0.00 ; (#,##0.00)")
			NewTotalRow.Cells.Add(HeaderCell)

			HeaderCell = New TableCell()
			HeaderCell.HorizontalAlign = HorizontalAlign.Right
			HeaderCell.Text = Format(CDbl(d180.ToString()), "#,##0.00 ; (#,##0.00)")
			NewTotalRow.Cells.Add(HeaderCell)

			HeaderCell = New TableCell()
			HeaderCell.HorizontalAlign = HorizontalAlign.Right
			HeaderCell.Text = Format(CDbl(d121.ToString()), "#,##0.00 ; (#,##0.00)")
			NewTotalRow.Cells.Add(HeaderCell)

			HeaderCell = New TableCell()
			HeaderCell.HorizontalAlign = HorizontalAlign.Right
			HeaderCell.Text = Format(CDbl(d91.ToString()), "#,##0.00 ; (#,##0.00)")
			NewTotalRow.Cells.Add(HeaderCell)

			dgvARreport.Controls(0).Controls.AddAt(e.Row.RowIndex + rowIndex, NewTotalRow)
			rowIndex += 1
			TotRowIndex += 1

			DR = 0
			CR = 0
			Bal = 0
			curr = 0
			d30 = 0
			d60 = 0
			d90 = 0
			d120 = 0
			d150 = 0
			d180 = 0
			d121 = 0
			d91 = 0
			pdc = 0
			net = 0

		End If
	End Sub

	Private Sub getSetUpVer2()
		dt = GetDataTable("select * from information_schema.tables where table_schema =  '" & vDbase & "' and " &
						  "table_name = '" & "tempaging_" & lblUser.Text & "'")
		If Not CBool(dt.Rows.Count) Then
			sql = "CREATE TABLE tempaging_" & lblUser.Text & " LIKE tempaging"
			ExecuteNonQuery(sql)
		Else
			sql = "truncate table tempaging_" & lblUser.Text
			ExecuteNonQuery(sql)
		End If

		dt.Dispose()

		'1 get invoice setup

		Select Case cboFormat.Text
			Case "ALL"
				sql = "insert into tempaging_" & lblUser.Text & "(invno,transdate,originvamt,tc,custno,smnno,shipto,pc,areano,term," &
					  "custname,shiptoname,dsrno) select a.invno,a.transdate,round(a.netamt,2),a.tc,a.custno,a.smnno,ifnull(a.shipto,a.custno)," &
					  "a.pc,b.areano,b.term,b.bussname,c.bussname,a.docno from saleshdrtbl a left join custmasttbl b on a.custno=b.custno " &
					  "left join custmasttbl c on a.shipto=c.custno where a.transdate <= '" & Format(CDate(dpTransdate.Text), "yyyy-MM-dd") & "' " &
					  "and a.status <> 'void'"
				ExecuteNonQuery(sql)

				sql = "insert into tempaging_" & lblUser.Text & "(invno,transdate,originvamt,tc,custno,smnno,shipto,pc,areano,term," &
					  "custname,shiptoname) select a.dmcmno,a.transdate,ifnull(a.totamt,0),a.tc,a.custno,a.smnno,a.custno,a.pc," &
					  "b.areano,b.term,b.bussname,b.bussname from custdmcmhdrtbl a left join custmasttbl b on a.custno=b.custno " &
					  "where a.transdate <= '" & Format(CDate(dpTransdate.Text), "yyyy-MM-dd") & "' and a.tc = '92' and a.status <> 'void'"
				ExecuteNonQuery(sql) 'new code

				'6. deposits
				sql = "insert into tempaging_" & lblUser.Text & "(invno,transdate,origoramt,tc,custno,smnno,shipto,pc,areano,term," &
					  "custname,shiptoname,dsrno) select c.orno,c.transdate,round(sum(a.oramt),2) as oramt,a.tc,c.custno,c.smnno,c.custno,c.pc, " &
					  "b.areano,b.term,b.bussname,b.bussname,concat(c.doctype,c.docno) from coldettbl a left join colhdrtbl c on a.orno=c.orno " &
					  "left join custmasttbl b on c.custno=b.custno where a.tc = '60' and a.oramt <> 0 and c.status = 'open' " &
					  "and c.transdate <= '" & Format(CDate(dpTransdate.Text), "yyyy-MM-dd") & "' and a.coltype = 'trade' " &
					  "group by a.refno"
				ExecuteNonQuery(sql)

			Case "Customer"
				sql = "insert into tempaging_" & lblUser.Text & "(invno,transdate,originvamt,tc,custno,smnno,shipto,pc,areano,term," &
					  "custname,shiptoname) select a.invno,a.transdate,round(a.netamt,2),a.tc,a.custno,a.smnno,ifnull(a.shipto,a.custno)," &
					  "a.pc,b.areano,b.term,b.bussname,c.bussname from saleshdrtbl a left join custmasttbl b on a.custno=b.custno " &
					  "left join custmasttbl c on a.shipto=c.custno where a.transdate <= '" & Format(CDate(dpTransdate.Text), "yyyy-MM-dd") & "' " &
					  "and a.custno = '" & cboARcust.Text.Substring(0, 5) & "' and a.status <> 'void'"
				ExecuteNonQuery(sql)

				sql = "insert into tempaging_" & lblUser.Text & "(invno,transdate,originvamt,tc,custno,smnno,shipto,pc,areano,term," &
							  "custname,shiptoname) select a.dmcmno,a.transdate,ifnull(a.totamt,0),a.tc,a.custno,a.smnno,a.custno,a.pc," &
							  "b.areano,b.term,b.bussname,b.bussname from custdmcmhdrtbl a left join custmasttbl b on a.custno=b.custno " &
							  "where a.transdate <= '" & Format(CDate(dpTransdate.Text), "yyyy-MM-dd") & "' and a.tc = '92' and a.status <> 'void' " &
							  "and a.custno = '" & cboARcust.Text.Substring(0, 5) & "'"
				ExecuteNonQuery(sql)

				'6. deposits
				sql = "insert into tempaging_" & lblUser.Text & "(invno,transdate,origoramt,tc,custno,smnno,shipto,pc,areano,term," &
					  "custname,shiptoname,dsrno) select c.orno,c.transdate,round(sum(a.oramt),2) as oramt,a.tc,c.custno,c.smnno,c.custno,c.pc, " &
					  "b.areano,b.term,b.bussname,b.bussname,concat(c.doctype,c.docno) from coldettbl a left join colhdrtbl c on a.orno=c.orno " &
					  "left join custmasttbl b on c.custno=b.custno where a.tc = '60' and a.oramt <> 0 and c.status = 'open' " &
					  "and c.transdate <= '" & Format(CDate(dpTransdate.Text), "yyyy-MM-dd") & "' and a.coltype = 'trade' " &
					  "and c.custno = '" & cboARcust.Text.Substring(0, 5) & "' group by a.refno" 'and c.branch = '" & vLoggedBranch & "' 
				ExecuteNonQuery(sql)

			Case "Salesman"
				sql = "insert into tempaging_" & lblUser.Text & "(invno,transdate,originvamt,tc,custno,smnno,shipto,pc,areano,term," &
					  "custname,shiptoname) select a.invno,a.transdate,round(a.netamt,2),a.tc,a.custno,a.smnno,ifnull(a.shipto,a.custno)," &
					  "a.pc,b.areano,b.term,b.bussname,c.bussname from saleshdrtbl a left join custmasttbl b on a.custno=b.custno " &
					  "left join custmasttbl c on a.shipto=c.custno where a.transdate <= '" & Format(CDate(dpTransdate.Text), "yyyy-MM-dd") & "' " &
					  "and a.smnno = '" & cboARcust.Text.Substring(0, 3) & "' and a.status <> 'void'"
				ExecuteNonQuery(sql)

				sql = "insert into tempaging_" & lblUser.Text & "(invno,transdate,originvamt,tc,custno,smnno,shipto,pc,areano,term," &
					  "custname,shiptoname) select a.dmcmno,a.transdate,ifnull(a.totamt,0),a.tc,a.custno,a.smnno,a.custno,a.pc," &
					  "b.areano,b.term,b.bussname,b.bussname from custdmcmhdrtbl a left join custmasttbl b on a.custno=b.custno " &
					  "where a.transdate <= '" & Format(CDate(dpTransdate.Text), "yyyy-MM-dd") & "' and a.tc = '92' and a.status <> 'void' " &
					  "and a.smnno = '" & cboARcust.Text.Substring(0, 3) & "'"
				ExecuteNonQuery(sql)

				dt = GetDataTable("select distinct custno from tempaging_" & lblUser.Text)
				If Not CBool(dt.Rows.Count) Then
					MsgBox("No " & cboFormat.Text & " Found")
					Exit Sub
				Else
					For Each dr As DataRow In dt.Rows
						sql = "insert into tempaging_" & lblUser.Text & "(invno,transdate,origoramt,tc,custno,smnno,shipto,pc,areano,term," &
							  "custname,shiptoname,dsrno) select c.orno,c.transdate,round(sum(a.oramt),2) as oramt,a.tc,c.custno,c.smnno,c.custno,c.pc, " &
							  "b.areano,b.term,b.bussname,b.bussname,concat(c.doctype,c.docno) from coldettbl a left join colhdrtbl c on a.orno=c.orno " &
							  "left join custmasttbl b on c.custno=b.custno where a.tc = '60' and a.oramt <> 0 and c.status = 'open' " &
							  "and c.transdate <= '" & Format(CDate(dpTransdate.Text), "yyyy-MM-dd") & "' and a.coltype = 'trade' " &
							  "and c.custno = '" & dr.Item(0).ToString() & "' group by a.refno" 'and c.branch = '" & vLoggedBranch & "' 
						ExecuteNonQuery(sql)

					Next

				End If

				Call dt.Dispose()

		End Select

		'3. get payment w/ ref no
		sql = "update tempaging_" & lblUser.Text & " a,(select a.refno,a.tc,round(sum(a.oramt),2) as oramt,c.custno from coldettbl a " &
			  "left join colhdrtbl c on a.orno=c.orno where a.tc <> '60' and a.oramt <> 0 and c.status = 'open' and " &
			  "c.transdate <= '" & Format(CDate(dpTransdate.Text), "yyyy-MM-dd") & "' group by a.refno) as colref " &
			  "set a.origoramt = colref.oramt where a.invno = colref.refno and a.tc = colref.tc and a.custno = colref.custno" 'and c.branch = '" & vLoggedBranch & "' 
		ExecuteNonQuery(sql)

		'4. get CM

		sql = "update tempaging_" & lblUser.Text & " a,(select a.refno,a.reftc,round(sum(a.amt),2) as cmamt,c.custno from custdmcmdettbl a " &
			  "left join custdmcmhdrtbl c on a.dmcmno=c.dmcmno where c.tc = '90' and a.amt <> 0 " &
			  "and c.status = 'posted' and c.transdate <= '" & Format(CDate(dpTransdate.Text), "yyyy-MM-dd") & "' " &
			  "group by a.refno,a.reftc) as cmref set a.cmamt = cmref.cmamt " &
			  "where a.invno = cmref.refno and a.tc = cmref.reftc and a.custno = cmref.custno" 'and c.branch = '" & vLoggedBranch & "' 
		ExecuteNonQuery(sql)

		''5. PDC w/ ref

		sql = "update tempaging_" & lblUser.Text & " a,(select a.refno,a.tc,round(sum(a.oramt),2) as oramt,c.custno from coldettbl a " &
			  "left join colhdrtbl c on a.orno=c.orno where a.tc <> '60' and a.oramt <> 0 " &
			  "and c.status = 'open' and c.transdate > '" & Format(CDate(dpTransdate.Text), "yyyy-MM-dd") & "' " &
			  "group by a.refno) as colref set a.pdc = colref.oramt " &
			  "where a.invno = colref.refno and a.tc = colref.tc and a.custno = colref.custno"
		ExecuteNonQuery(sql)

		FinalUpdateAR() 'ok
		GetAgingAllver2()
		FillDgvAR()

	End Sub

	Private Sub FillDgvAR()
		Dim adapter As New MySqlDataAdapter
		Dim ds As New DataSet()
		'Dim i As Integer = 0
		sqldata = Nothing

		sqldata = "select custno,custname,shipto,shiptoname,invno,tc,transdate,term,ifnull(30d,0) as 30d," &
				  "ifnull(60d,0) as 60d,ifnull(90d,0) as 90d,ifnull(120d,0) as 120d,ifnull(150d,0) as 150d," &
				  "ifnull(180d,0) as 180d,ifnull(121d,0) as 121d,ifnull(91d,0) as 91d," &
				  "ifnull(totalar,0) as totalar from tempaging_" & lblUser.Text & " " &
				  "order by custno,shipto,tc,transdate"

		conn.Open()
		Dim command As New MySqlCommand(sqldata, conn)
		adapter.SelectCommand = command
		adapter.Fill(ds)
		adapter.Dispose()
		command.Dispose()
		conn.Close()
		dgvARreport.DataSource = ds.Tables(0)
		dgvARreport.DataBind()

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

		'update term
		sql = "update tempaging_" & lblUser.Text & " a left join custmasttbl b on a.custno=b.custno set a.term = ifnull(b.term,0)"
		ExecuteNonQuery(sql)

		sql = "update tempaging_" & lblUser.Text & " set days = datediff('" & Format(CDate(dpTransdate.Text), "yyyy-MM-dd") & "', transdate)" '" & _  - term
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

		'update Smn init
		sql = "update tempaging_" & lblUser.Text & " a,(select smnno,shrtname FROM smnmtbl) as b set a.smnno = concat(b.smnno,'-',b.shrtname) " &
			  "where a.smnno=b.smnno"
		ExecuteNonQuery(sql)


	End Sub

	Private Sub GetAgingAllver2()

		sql = "update tempaging_" & lblUser.Text & " set 30d = case when days between 0 and 30 then totalar else 0 end," &
			  "60d = case when days between 31 and 60 then totalar else 0 end," &
			  "90d = case when days between 61 and 90 then totalar else 0 end," &
			  "120d = case when days between 91 and 120 then totalar else 0 end," &
			  "150d = case when days between 121 and 150 then totalar else 0 end," &
			  "180d = case when days between 151 and 180 then totalar else 0 end," &
			  "121d = case when days between 181 and 360 then totalar else 0 end," &
			  "91d = case when days > 360 then totalar else 0 end " &
			  "where invno = invno"
		ExecuteNonQuery(sql)

		'sql = "update tempaging_or set curr = null where curr = 0 and user = '" & lblUser.Text & "'"
		'ExecuteNonQuery(sql)

		'sql = "update tempaging_or set 30d = null where 30d = 0 and user = '" & lblUser.Text & "'"
		'ExecuteNonQuery(sql)

	End Sub

	Private Sub cboFormat_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboFormat.SelectedIndexChanged
		If cboFormat.Text = "" Then
			Exit Sub
		ElseIf dpTransdate.Text = Nothing Then
			Exit Sub
		End If

		PopARcust()

	End Sub

	Private Sub PopARcust()

		dgvARreport.DataSource = Nothing
		dgvARreport.DataBind()

		cboARcust.Items.Clear()

		Select Case cboFormat.Text
			Case "ALL"
				cboARcust.Visible = False
				lblFilter2.Visible = False
				getSetUpVer2()
				Exit Sub
			Case "Customer"
				cboARcust.Visible = True
				lblFilter2.Visible = True
				dt = GetDataTable("select distinct concat(a.custno,space(1),b.bussname) from saleshdrtbl a left join custmasttbl b " &
								  "on a.custno=b.custno where a.transdate <= '" & Format(CDate(dpTransdate.Text), "yyyy-MM-dd") & "' and " &
								  "a.status <> 'void' order by b.bussname")
			Case "Salesman"
				cboARcust.Visible = True
				lblFilter2.Visible = True
				dt = GetDataTable("select distinct concat(a.smnno,space(1),b.fullname) from saleshdrtbl a left join smnmtbl b " &
								  "on a.smnno=b.smnno where a.transdate <= '" & Format(CDate(dpTransdate.Text), "yyyy-MM-dd") & "' and " &
								  "a.status <> 'void' order by b.fullname")
		End Select

		If Not CBool(dt.Rows.Count) Then
			MsgBox("No " & cboFormat.Text & " Found")
			Exit Sub
		Else
			cboARcust.Items.Add("")

			For Each dr As DataRow In dt.Rows
				cboARcust.Items.Add(dr.Item(0).ToString())

			Next

		End If

		Call dt.Dispose()


	End Sub

	Private Sub dpTransdate_TextChanged(sender As Object, e As EventArgs) Handles dpTransdate.TextChanged
		If cboFormat.Text = "" Then
			Exit Sub
		ElseIf dpTransdate.Text = Nothing Then
			Exit Sub
		End If

		PopARcust()

	End Sub

	Private Sub cboARcust_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboARcust.SelectedIndexChanged
		If cboARcust.Text = "" Then
			Exit Sub
		End If

		getSetUpVer2()

	End Sub
End Class