Imports MySql.Data.MySqlClient

Public Class cashierReports
	Inherits System.Web.UI.Page
	Private strReport As String
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
	Dim admOpt As String
	Dim admSmnNo As String
	Dim admCustNo As String

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
			PopCollReport()
			PopDepReport()
		End If
	End Sub

	Private Sub PopCollReport()
		cboColReport.Items.Clear()
		cboColReport.Items.Add("")
		cboColReport.Items.Add("Collection Register")
		cboColReport.Items.Add("Collection Summary")


	End Sub

	Private Sub PopDepReport()
		cboDepReport.Items.Clear()
		cboDepReport.Items.Add("")
		cboDepReport.Items.Add("Deposit Register")
		cboDepReport.Items.Add("Deposit Summary")

	End Sub

	Protected Sub lbClose_Click(sender As Object, e As EventArgs)
		Response.Redirect("FinancialAccounting.aspx")
	End Sub

	Protected Sub dgvColReg_RowDataBound(sender As Object, e As GridViewRowEventArgs)

	End Sub

	Protected Sub dgvColReg_RowCreated(sender As Object, e As GridViewRowEventArgs)

	End Sub

	Protected Sub dgvColSum_RowDataBound(sender As Object, e As GridViewRowEventArgs)

	End Sub

	Protected Sub dgvColSum_RowCreated(sender As Object, e As GridViewRowEventArgs)

	End Sub

	Private Sub dpColDFrom_TextChanged(sender As Object, e As EventArgs) Handles dpColDFrom.TextChanged, dpColDTo.TextChanged
		If cboColReport.Text = "" Then
			Exit Sub
		ElseIf dpColDFrom.Text = Nothing Then
			Exit Sub
		ElseIf dpColDTo.Text = Nothing Then
			Exit Sub
		ElseIf CDate(dpColDFrom.Text) > CDate(dpColDTo.Text) Then
			Exit Sub
		End If

		InitColl()
		PopSmnColl()

	End Sub

	Private Sub PopSmnColl()
		cboColCust.Items.Clear()
		cboColSmn.Items.Clear()
		dt = GetDataTable("select distinct concat(a.smnno,space(1),b.fullname) from colhdrtbl a " &
						  "left join smnmtbl b on a.smnno=b.smnno where a.status <> 'void' and " &
						  "a.transdate between '" & Format(CDate(dpColDFrom.Text), "yyyy-MM-dd") & "' " &
						  "and '" & Format(CDate(dpColDTo.Text), "yyyy-MM-dd") & "' and " &
						  "b.status = 'active' order by b.fullname")
		If Not CBool(dt.Rows.Count) Then
			Exit Sub

		Else
			cboColSmn.Items.Add("")
			cboColSmn.Items.Add("ALL")
			For Each dr As DataRow In dt.Rows
				cboColSmn.Items.Add(dr.Item(0).ToString() & "")

			Next

		End If

		Call dt.Dispose()

	End Sub

	Private Sub cboColSmn_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboColSmn.SelectedIndexChanged
		InitColl()
		If cboColReport.Text = "" Then
			Exit Sub
		ElseIf dpColDFrom.Text = Nothing Then
			Exit Sub
		ElseIf dpColDTo.Text = Nothing Then
			Exit Sub
		ElseIf CDate(dpColDFrom.Text) > CDate(dpColDTo.Text) Then
			Exit Sub
		ElseIf cboColSmn.Text = "" Then
			Exit Sub
		End If

		PopCollCust()

	End Sub

	Private Sub PopCollCust()

		cboColCust.Items.Clear()
		cboColCust.Items.Add("")

		Select Case cboColReport.Text
			Case "Collection Register"
				Panel4.Visible = False
				Panel3.Visible = True
				lblLabelCust.Visible = True
				cboColCust.Visible = True

				Select Case cboColSmn.Text
					Case "ALL"
						FilldgvCollReg()
						dt = GetDataTable("select distinct concat(a.custno,space(1),b.bussname) from colhdrtbl a " &
										  "left join custmasttbl b on a.custno=b.custno where a.status <> 'void' and " &
										  "a.transdate between '" & Format(CDate(dpColDFrom.Text), "yyyy-MM-dd") & "' " &
										  "and '" & Format(CDate(dpColDTo.Text), "yyyy-MM-dd") & "' and " &
										  "b.status = 'active' order by b.bussname")
					Case Else
						cboColCust.Items.Add("ALL")
						dt = GetDataTable("select distinct concat(a.custno,space(1),b.bussname) from colhdrtbl a " &
										  "left join custmasttbl b on a.custno=b.custno where a.status <> 'void' and " &
										  "a.transdate between '" & Format(CDate(dpColDFrom.Text), "yyyy-MM-dd") & "' " &
										  "and '" & Format(CDate(dpColDTo.Text), "yyyy-MM-dd") & "' and " &
										  "b.status = 'active' and a.smnno = '" & cboColSmn.Text.Substring(0, 3) & "' " &
										  "order by b.bussname")
				End Select

				If Not CBool(dt.Rows.Count) Then
					Exit Sub
				Else
					For Each dr As DataRow In dt.Rows
						cboColCust.Items.Add(dr.Item(0).ToString() & "")
					Next

				End If

				Call dt.Dispose()

			Case "Collection Summary"
				Panel3.Visible = False
				Panel4.Visible = True
				lblLabelCust.Visible = False
				cboColCust.Visible = False

				FillColSum()

				lblMsg.Text = "Not Yet Available"
				Exit Sub

		End Select

	End Sub

	Private Sub InitColl()

		dgvColReg.DataSource = Nothing
		dgvColReg.DataBind()
		dgvColSum.DataSource = Nothing
		dgvColSum.DataBind()
		lblColAmt.Text = "0.00"
		lblEwtAmt.Text = "0.00"
		lblNetAmt.Text = "0.00"


	End Sub

	Private Sub FilldgvCollReg()
		Dim adapter As New MySqlDataAdapter
		Dim ds As New DataSet()

		Select Case cboColSmn.Text
			Case "ALL"
				dt = GetDataTable("select sum(ifnull(a.oramt,0)),sum(ifnull(a.ewt,0)),sum(ifnull(a.netcolamt,0)) from coldettbl a " &
								  "left join colhdrtbl b on a.orno=b.orno where b.status <> 'void' and " &
								  "b.transdate between '" & Format(CDate(dpColDFrom.Text), "yyyy-MM-dd") & "' " &
								  "and '" & Format(CDate(dpColDTo.Text), "yyyy-MM-dd") & "'")
				If Not CBool(dt.Rows.Count) Then
					lblColAmt.Text = "0.00"
					lblEwtAmt.Text = "0.00"
					lblNetAmt.Text = "0.00"
					dgvColReg.DataSource = Nothing
					dgvColReg.DataBind()
					lblMsg.Text = "No Collection from Selected Dates"
					Exit Sub
				Else
					For Each dr As DataRow In dt.Rows
						lblColAmt.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0.00")
						lblEwtAmt.Text = Format(CDbl(dr.Item(1).ToString()), "#,##0.00")
						lblNetAmt.Text = Format(CDbl(dr.Item(2).ToString()), "#,##0.00")
					Next

				End If

				dt.Dispose()

				sqldata = Nothing

				sqldata = "select a.orno,a.transdate,concat(a.doctype,a.docno) as docno,concat(a.smnno,space(1),b.fullname) as smnno," &
						  "concat(a.custno,space(1),c.bussname) as custno,a.coltype,a.cdcrno,sum(ifnull(d.oramt,0)) as colamt," &
						  "sum(ifnull(d.ewt,0)) as ewtamt,sum(ifnull(d.netcolamt,0)) as netamt from coldettbl d left join colhdrtbl a on a.orno=d.orno " &
						  "left join smnmtbl b on a.smnno=b.smnno left join custmasttbl c on a.custno=c.custno where " &
						  "a.transdate between '" & Format(CDate(dpColDFrom.Text), "yyyy-MM-dd") & "' " &
						  "and '" & Format(CDate(dpColDTo.Text), "yyyy-MM-dd") & "' and a.status <> 'void' group by a.orno order by a.orno"

			Case Else
				Select Case cboColCust.Text
					Case "ALL"
						dt = GetDataTable("select sum(ifnull(a.oramt,0)),sum(ifnull(a.ewt,0)),sum(ifnull(a.netcolamt,0)) from coldettbl a " &
										  "left join colhdrtbl b on a.orno=b.orno where b.status <> 'void' and " &
										  "b.transdate between '" & Format(CDate(dpColDFrom.Text), "yyyy-MM-dd") & "' " &
										  "and '" & Format(CDate(dpColDTo.Text), "yyyy-MM-dd") & "' and " &
										  "b.smnno = '" & cboColSmn.Text.Substring(0, 3) & "' group by b.smnno")
						If Not CBool(dt.Rows.Count) Then
							lblColAmt.Text = "0.00"
							lblEwtAmt.Text = "0.00"
							lblNetAmt.Text = "0.00"
							dgvColReg.DataSource = Nothing
							dgvColReg.DataBind()
							lblMsg.Text = "No Collection from Selected Dates"
							Exit Sub
						Else
							For Each dr As DataRow In dt.Rows
								lblColAmt.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0.00")
								lblEwtAmt.Text = Format(CDbl(dr.Item(1).ToString()), "#,##0.00")
								lblNetAmt.Text = Format(CDbl(dr.Item(2).ToString()), "#,##0.00")
							Next

						End If

						dt.Dispose()

						sqldata = Nothing

						sqldata = "select a.orno,a.transdate,concat(a.doctype,a.docno) as docno,concat(a.smnno,space(1),b.fullname) as smnno," &
								  "concat(a.custno,space(1),c.bussname) as custno,a.coltype,a.cdcrno,sum(ifnull(d.oramt,0)) as colamt," &
								  "sum(ifnull(d.ewt,0)) as ewtamt,sum(ifnull(d.netcolamt,0)) as netamt from coldettbl d left join colhdrtbl a " &
								  "on a.orno=d.orno left join smnmtbl b on a.smnno=b.smnno left join custmasttbl c on a.custno=c.custno where " &
								  "a.transdate between '" & Format(CDate(dpColDFrom.Text), "yyyy-MM-dd") & "' " &
								  "and '" & Format(CDate(dpColDTo.Text), "yyyy-MM-dd") & "' and a.status <> 'void' and " &
								  "a.smnno = '" & cboColSmn.Text.Substring(0, 3) & "' group by a.orno order by a.orno"
					Case Else
						dt = GetDataTable("select sum(ifnull(a.oramt,0)),sum(ifnull(a.ewt,0)),sum(ifnull(a.netcolamt,0)) from coldettbl a " &
										  "left join colhdrtbl b on a.orno=b.orno where b.status <> 'void' and " &
										  "b.transdate between '" & Format(CDate(dpColDFrom.Text), "yyyy-MM-dd") & "' " &
										  "and '" & Format(CDate(dpColDTo.Text), "yyyy-MM-dd") & "' and " &
										  "b.custno = '" & cboColCust.Text.Substring(0, 5) & "' group by b.custno")
						If Not CBool(dt.Rows.Count) Then
							lblColAmt.Text = "0.00"
							lblEwtAmt.Text = "0.00"
							lblNetAmt.Text = "0.00"
							dgvColReg.DataSource = Nothing
							dgvColReg.DataBind()
							lblMsg.Text = "No Collection from Selected Dates"
							Exit Sub
						Else
							For Each dr As DataRow In dt.Rows
								lblColAmt.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0.00")
								lblEwtAmt.Text = Format(CDbl(dr.Item(1).ToString()), "#,##0.00")
								lblNetAmt.Text = Format(CDbl(dr.Item(2).ToString()), "#,##0.00")
							Next

						End If

						dt.Dispose()

						sqldata = Nothing

						sqldata = "select a.orno,a.transdate,concat(a.doctype,a.docno) as docno,concat(a.smnno,space(1),b.fullname) as smnno," &
								  "concat(a.custno,space(1),c.bussname) as custno,a.coltype,a.cdcrno,sum(ifnull(d.oramt,0)) as colamt," &
								  "sum(ifnull(d.ewt,0)) as ewtamt,sum(ifnull(d.netcolamt,0)) as netamt from coldettbl d left join colhdrtbl a " &
								  "on a.orno=d.orno left join smnmtbl b on a.smnno=b.smnno left join custmasttbl c on a.custno=c.custno where " &
								  "a.transdate between '" & Format(CDate(dpColDFrom.Text), "yyyy-MM-dd") & "' " &
								  "and '" & Format(CDate(dpColDTo.Text), "yyyy-MM-dd") & "' and a.status <> 'void' and " &
								  "a.custno = '" & cboColCust.Text.Substring(0, 5) & "' group by a.orno order by a.orno"
				End Select
		End Select

		With dgvColReg
			.DataSource = MyDataSet.Tables("colhdrtbl")

			.Columns(0).HeaderText = "OR No."
			.Columns(1).HeaderText = "Date"
			.Columns(2).HeaderText = "Doc No"
			.Columns(3).HeaderText = "Salesman"
			.Columns(4).HeaderText = "Customer"
			.Columns(5).HeaderText = "Col Type"
			.Columns(6).HeaderText = "CDCR No."
			.Columns(7).HeaderText = "Col Amt"
			.Columns(8).HeaderText = "EWT Amt"
			.Columns(9).HeaderText = "Net Amt"

		End With

		conn.Open()
		Dim command As New MySqlCommand(sqldata, conn)
		adapter.SelectCommand = command
		adapter.Fill(ds)
		adapter.Dispose()
		command.Dispose()
		conn.Close()
		dgvColReg.DataSource = ds.Tables(0)
		dgvColReg.DataBind()


	End Sub

	Private Sub FillDGVregCustOnly()

		Dim adapter As New MySqlDataAdapter
		Dim ds As New DataSet()

		Select Case cboColCust.Text
			Case "ALL"
				Exit Sub
			Case Else

				dt = GetDataTable("select sum(ifnull(a.oramt,0)),sum(ifnull(a.ewt,0)),sum(ifnull(a.netcolamt,0)) from coldettbl a " &
								  "left join colhdrtbl b on a.orno=b.orno where b.status <> 'void' and " &
								  "b.transdate between '" & Format(CDate(dpColDFrom.Text), "yyyy-MM-dd") & "' " &
								  "and '" & Format(CDate(dpColDTo.Text), "yyyy-MM-dd") & "' and " &
								  "b.custno = '" & cboColCust.Text.Substring(0, 5) & "' group by b.custno")
				If Not CBool(dt.Rows.Count) Then
					lblColAmt.Text = "0.00"
					lblEwtAmt.Text = "0.00"
					lblNetAmt.Text = "0.00"
					dgvColReg.DataSource = Nothing
					dgvColReg.DataBind()
					lblMsg.Text = "No Collection from Selected Dates"
					Exit Sub
				Else
					For Each dr As DataRow In dt.Rows
						lblColAmt.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0.00")
						lblEwtAmt.Text = Format(CDbl(dr.Item(1).ToString()), "#,##0.00")
						lblNetAmt.Text = Format(CDbl(dr.Item(2).ToString()), "#,##0.00")
					Next

				End If

				dt.Dispose()

				sqldata = Nothing

				sqldata = "select a.orno,a.transdate,concat(a.doctype,a.docno) as docno,concat(a.smnno,space(1),b.fullname) as smnno," &
						  "concat(a.custno,space(1),c.bussname) as custno,a.coltype,a.cdcrno,sum(ifnull(d.oramt,0)) as colamt," &
						  "sum(ifnull(d.ewt,0)) as ewtamt,sum(ifnull(d.netcolamt,0)) as netamt from coldettbl d left join colhdrtbl a " &
						  "on a.orno=d.orno left join smnmtbl b on a.smnno=b.smnno left join custmasttbl c on a.custno=c.custno where " &
						  "a.transdate between '" & Format(CDate(dpColDFrom.Text), "yyyy-MM-dd") & "' " &
						  "and '" & Format(CDate(dpColDTo.Text), "yyyy-MM-dd") & "' and a.status <> 'void' and " &
						  "a.custno = '" & cboColCust.Text.Substring(0, 5) & "' group by a.orno order by a.orno"
		End Select

		With dgvColReg
			.DataSource = MyDataSet.Tables("colhdrtbl")

			.Columns(0).HeaderText = "OR No."
			.Columns(1).HeaderText = "Date"
			.Columns(2).HeaderText = "Doc No"
			.Columns(3).HeaderText = "Salesman"
			.Columns(4).HeaderText = "Customer"
			.Columns(5).HeaderText = "Col Type"
			.Columns(6).HeaderText = "CDCR No."
			.Columns(7).HeaderText = "Col Amt"
			.Columns(8).HeaderText = "EWT Amt"
			.Columns(9).HeaderText = "Net Amt"

		End With

		conn.Open()
		Dim command As New MySqlCommand(sqldata, conn)
		adapter.SelectCommand = command
		adapter.Fill(ds)
		adapter.Dispose()
		command.Dispose()
		conn.Close()
		dgvColReg.DataSource = ds.Tables(0)
		dgvColReg.DataBind()

	End Sub

	Private Sub cboColCust_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboColCust.SelectedIndexChanged
		InitColl()
		If cboColReport.Text = "" Then
			Exit Sub
		ElseIf dpColDFrom.Text = Nothing Then
			Exit Sub
		ElseIf dpColDTo.Text = Nothing Then
			Exit Sub
		ElseIf CDate(dpColDFrom.Text) > CDate(dpColDTo.Text) Then
			Exit Sub
		ElseIf cboColSmn.Text = "" Then
			Exit Sub
		ElseIf cboColCust.Text = "" Or cboColCust.Text = Nothing Then
			Exit Sub
		End If

		Select Case cboColSmn.Text
			Case "ALL"
				Select Case cboColCust.Text
					Case "ALL"

					Case Else
						FillDGVregCustOnly()
				End Select
			Case Else
				Select Case cboColCust.Text
					Case "ALL"
						FilldgvCollReg()
					Case Else
						FillDGVregCustOnly()
				End Select

		End Select

	End Sub

	Private Sub cboColReport_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboColReport.SelectedIndexChanged
		InitColl()

	End Sub

	Private Sub FillColSum()

		Dim adapter As New MySqlDataAdapter
		Dim ds As New DataSet()
		Select Case cboColSmn.Text
			Case "ALL"
				dt = GetDataTable("select sum(ifnull(a.oramt,0)),sum(ifnull(a.ewt,0)),sum(ifnull(a.netcolamt,0)) from coldettbl a " &
								  "left join colhdrtbl b on a.orno=b.orno where b.status <> 'void' and " &
								  "b.transdate between '" & Format(CDate(dpColDFrom.Text), "yyyy-MM-dd") & "' " &
								  "and '" & Format(CDate(dpColDTo.Text), "yyyy-MM-dd") & "'")
				If Not CBool(dt.Rows.Count) Then
					lblColAmt.Text = "0.00"
					lblEwtAmt.Text = "0.00"
					lblNetAmt.Text = "0.00"
					dgvColSum.DataSource = Nothing
					dgvColSum.DataBind()
					lblMsg.Text = "No Collection from Selected Dates"
					Exit Sub
				Else
					For Each dr As DataRow In dt.Rows
						lblColAmt.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0.00")
						lblEwtAmt.Text = Format(CDbl(dr.Item(1).ToString()), "#,##0.00")
						lblNetAmt.Text = Format(CDbl(dr.Item(2).ToString()), "#,##0.00")
					Next

				End If

				dt.Dispose()

				sqldata = Nothing

				sqldata = "select concat(a.smnno,space(1),b.fullname) as smnno,concat(a.custno,space(1),c.bussname) as custno," &
						  "sum(ifnull(d.oramt,0)) as colamt,sum(ifnull(d.ewt,0)) as ewtamt,sum(ifnull(d.netcolamt,0)) as netamt " &
						  "from coldettbl d left join colhdrtbl a on a.orno=d.orno left join smnmtbl b on a.smnno=b.smnno " &
						  "left join custmasttbl c on a.custno=c.custno where a.status <> 'void' and " &
						  "a.transdate between '" & Format(CDate(dpColDFrom.Text), "yyyy-MM-dd") & "' " &
						  "and '" & Format(CDate(dpColDTo.Text), "yyyy-MM-dd") & "' group by a.custno order by netamt desc"
			Case Else
				dt = GetDataTable("select sum(ifnull(a.oramt,0)),sum(ifnull(a.ewt,0)),sum(ifnull(a.netcolamt,0)) from coldettbl a " &
								  "left join colhdrtbl b on a.orno=b.orno where b.status <> 'void' and " &
								  "b.transdate between '" & Format(CDate(dpColDFrom.Text), "yyyy-MM-dd") & "' " &
								  "and '" & Format(CDate(dpColDTo.Text), "yyyy-MM-dd") & "' and " &
								  "b.smnno = '" & cboColSmn.Text.Substring(0, 3) & "' group by b.smnno")
				If Not CBool(dt.Rows.Count) Then
					lblColAmt.Text = "0.00"
					lblEwtAmt.Text = "0.00"
					lblNetAmt.Text = "0.00"
					dgvColSum.DataSource = Nothing
					dgvColSum.DataBind()
					lblMsg.Text = "No Collection from Selected Dates"
					Exit Sub
				Else
					For Each dr As DataRow In dt.Rows
						lblColAmt.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0.00")
						lblEwtAmt.Text = Format(CDbl(dr.Item(1).ToString()), "#,##0.00")
						lblNetAmt.Text = Format(CDbl(dr.Item(2).ToString()), "#,##0.00")
					Next

				End If

				dt.Dispose()

				sqldata = Nothing

				sqldata = "select concat(a.smnno,space(1),b.fullname) as smnno,concat(a.custno,space(1),c.bussname) as custno," &
						  "sum(ifnull(d.oramt,0)) as colamt,sum(ifnull(d.ewt,0)) as ewtamt,sum(ifnull(d.netcolamt,0)) as netamt " &
						  "from coldettbl d left join colhdrtbl a on a.orno=d.orno left join smnmtbl b on a.smnno=b.smnno " &
						  "left join custmasttbl c on a.custno=c.custno where a.status <> 'void' and " &
						  "a.transdate between '" & Format(CDate(dpColDFrom.Text), "yyyy-MM-dd") & "' " &
						  "and '" & Format(CDate(dpColDTo.Text), "yyyy-MM-dd") & "' and a.smnno = '" & cboColSmn.Text.Substring(0, 3) & "' " &
						  "group by a.custno order by netamt desc"

		End Select

		With dgvColSum
			.DataSource = MyDataSet.Tables("colhdrtbl")

			.Columns(0).HeaderText = "Salesman"
			.Columns(1).HeaderText = "Customer"
			.Columns(2).HeaderText = "Col Amt"
			.Columns(3).HeaderText = "EWT Amt"
			.Columns(4).HeaderText = "Net Amt"

		End With

		conn.Open()
		Dim command As New MySqlCommand(sqldata, conn)
		adapter.SelectCommand = command
		adapter.Fill(ds)
		adapter.Dispose()
		command.Dispose()
		conn.Close()
		dgvColSum.DataSource = ds.Tables(0)
		dgvColSum.DataBind()

	End Sub

	Protected Sub dgvDepReg_RowDataBound(sender As Object, e As GridViewRowEventArgs)

	End Sub

	Protected Sub dgvDepReg_RowCreated(sender As Object, e As GridViewRowEventArgs)

	End Sub

	Protected Sub dgvDepSum_RowDataBound(sender As Object, e As GridViewRowEventArgs)

	End Sub

	Protected Sub dgvDepSum_RowCreated(sender As Object, e As GridViewRowEventArgs)

	End Sub

	Private Sub dpDepDate1_TextChanged(sender As Object, e As EventArgs) Handles dpDepDate1.TextChanged, dpDepDate2.TextChanged
		If cboDepReport.Text = "" Then
			Exit Sub
		ElseIf dpDepDate1.Text = Nothing Then
			Exit Sub
		ElseIf dpDepDate2.Text = Nothing Then
			Exit Sub
		ElseIf CDate(dpDepDate1.Text) > CDate(dpDepDate2.Text) Then
			Exit Sub
		End If

		Select Case cboDepReport.Text
			Case "Deposit Register"
				Panel2.Visible = False
				Panel1.Visible = True
				FillDepositReg()
			Case "Deposit Summary"
				Panel1.Visible = False
				Panel2.Visible = True

		End Select

		PopBank()

	End Sub

	Private Sub FillDepositReg()
		Dim adapter As New MySqlDataAdapter
		Dim ds As New DataSet()

		dt = GetDataTable("select sum(ifnull(a.cashamt,0)),sum(ifnull(a.checkamt,0)),sum(ifnull(a.cashamt,0)) + sum(ifnull(a.checkamt,0)) " &
						  "from bankdepdettbl a join bankdephdrtbl c on a.depno=c.depno where " &
						  "c.transdate between '" & Format(CDate(dpDepDate1.Text), "yyyy-MM-dd") & "' and " &
						  "'" & Format(CDate(dpDepDate2.Text), "yyyy-MM-dd") & "'")
		If Not CBool(dt.Rows.Count) Then
			lblCashAmt.Text = "0.00"
			lblCheckAmt.Text = "0.00"
			lblTotAmt.Text = "0.00"
			dgvDepReg.DataSource = Nothing
			dgvDepReg.DataBind()
			lblMsg.Text = "No Deposit from Selected Dates"
			Exit Sub
		Else
			For Each dr As DataRow In dt.Rows
				lblCashAmt.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0.00")
				lblCheckAmt.Text = Format(CDbl(dr.Item(1).ToString()), "#,##0.00")
				lblTotAmt.Text = Format(CDbl(dr.Item(2).ToString()), "#,##0.00")
			Next

		End If

		dt.Dispose()

		sqldata = Nothing

		sqldata = "select a.depno,c.transdate,a.cdcrno,concat(a.acctno,space(1),b.acctdesc) as acctno,ifnull(a.cashamt,0) as cashamt," &
				  "ifnull(a.checkamt,0) as checkamt,ifnull(a.cashamt,0) + ifnull(a.checkamt,0) as totamt from bankdepdettbl a " &
				  "left join acctcharttbl b on a.acctno=b.acctno left join bankdephdrtbl c on a.depno=c.depno where " &
				  "c.transdate between '" & Format(CDate(dpDepDate1.Text), "yyyy-MM-dd") & "' and " &
				  "'" & Format(CDate(dpDepDate2.Text), "yyyy-MM-dd") & "' group by a.transid"

		With dgvDepReg
			.DataSource = MyDataSet.Tables("colhdrtbl")
			.Columns(0).HeaderText = "Dep No."
			.Columns(1).HeaderText = "Date"
			.Columns(2).HeaderText = "CDCR No"
			.Columns(3).HeaderText = "Account/Bank"
			.Columns(4).HeaderText = "Cash"
			.Columns(5).HeaderText = "Cheque"
			.Columns(6).HeaderText = "Total"

		End With

		conn.Open()
		Dim command As New MySqlCommand(sqldata, conn)
		adapter.SelectCommand = command
		adapter.Fill(ds)
		adapter.Dispose()
		command.Dispose()
		conn.Close()
		dgvDepReg.DataSource = ds.Tables(0)
		dgvDepReg.DataBind()

	End Sub

	Private Sub FillDepositRegBank()
		Dim adapter As New MySqlDataAdapter
		Dim ds As New DataSet()

		dt = GetDataTable("select sum(ifnull(a.cashamt,0)),sum(ifnull(a.checkamt,0)),sum(ifnull(a.cashamt,0)) + sum(ifnull(a.checkamt,0)) " &
						  "from bankdepdettbl a join bankdephdrtbl c on a.depno=c.depno where a.acctno = '" & cboBank.Text.Substring(0, 7) & "' " &
						  "and c.transdate between '" & Format(CDate(dpDepDate1.Text), "yyyy-MM-dd") & "' and " &
						  "'" & Format(CDate(dpDepDate2.Text), "yyyy-MM-dd") & "'")
		If Not CBool(dt.Rows.Count) Then
			lblCashAmt.Text = "0.00"
			lblCheckAmt.Text = "0.00"
			lblTotAmt.Text = "0.00"
			dgvDepReg.DataSource = Nothing
			dgvDepReg.DataBind()
			lblMsg.Text = "No Deposit from Selected Dates"
			Exit Sub
		Else
			For Each dr As DataRow In dt.Rows
				lblCashAmt.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0.00")
				lblCheckAmt.Text = Format(CDbl(dr.Item(1).ToString()), "#,##0.00")
				lblTotAmt.Text = Format(CDbl(dr.Item(2).ToString()), "#,##0.00")
			Next

		End If

		dt.Dispose()

		sqldata = Nothing

		sqldata = "select a.depno,c.transdate,a.cdcrno,concat(a.acctno,space(1),b.acctdesc) as acctno,ifnull(a.cashamt,0) as cashamt," &
				  "ifnull(a.checkamt,0) as checkamt,ifnull(a.cashamt,0) + ifnull(a.checkamt,0) as totamt from bankdepdettbl a " &
				  "left join acctcharttbl b on a.acctno=b.acctno left join bankdephdrtbl c on a.depno=c.depno where " &
				  "a.acctno = '" & cboBank.Text.Substring(0, 7) & "' and c.transdate between '" & Format(CDate(dpDepDate1.Text), "yyyy-MM-dd") & "' and " &
				  "'" & Format(CDate(dpDepDate2.Text), "yyyy-MM-dd") & "' group by a.transid"

		With dgvDepReg
			.DataSource = MyDataSet.Tables("colhdrtbl")
			.Columns(0).HeaderText = "Dep No."
			.Columns(1).HeaderText = "Date"
			.Columns(2).HeaderText = "CDCR No"
			.Columns(3).HeaderText = "Account/Bank"
			.Columns(4).HeaderText = "Cash"
			.Columns(5).HeaderText = "Cheque"
			.Columns(6).HeaderText = "Total"

		End With

		conn.Open()
		Dim command As New MySqlCommand(sqldata, conn)
		adapter.SelectCommand = command
		adapter.Fill(ds)
		adapter.Dispose()
		command.Dispose()
		conn.Close()
		dgvDepReg.DataSource = ds.Tables(0)
		dgvDepReg.DataBind()

	End Sub

	Private Sub PopBank()

		cboBank.Items.Clear()
		dt = GetDataTable("select distinct concat(a.acctno,space(1),b.acctdesc) from bankdepdettbl a " &
						  "left join acctcharttbl b on a.acctno=b.acctno left join bankdephdrtbl c on a.depno=c.depno where " &
						  "c.transdate between '" & Format(CDate(dpDepDate1.Text), "yyyy-MM-dd") & "' and " &
						  "'" & Format(CDate(dpDepDate2.Text), "yyyy-MM-dd") & "'")
		If Not CBool(dt.Rows.Count) Then
			Exit Sub
		Else
			cboBank.Items.Add("")
			cboBank.Items.Add("ALL")
			For Each dr As DataRow In dt.Rows
				cboBank.Items.Add(dr.Item(0).ToString() & "")
			Next

		End If

		Call dt.Dispose()

	End Sub

	Private Sub cboBank_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboBank.SelectedIndexChanged
		If cboDepReport.Text = "" Then
			Exit Sub
		ElseIf dpDepDate1.Text = Nothing Then
			Exit Sub
		ElseIf dpDepDate2.Text = Nothing Then
			Exit Sub
		ElseIf CDate(dpDepDate1.Text) > CDate(dpDepDate2.Text) Then
			Exit Sub
		ElseIf cboBank.Text = "" Or cboBank.Text = Nothing Then
			Exit Sub
		End If

		Select Case cboDepReport.Text
			Case "Deposit Register"
				Select Case cboBank.Text
					Case "ALL"
						FillDepositReg()
					Case Else
						FillDepositRegBank()
				End Select

			Case "Deposit Summary"


		End Select
	End Sub
End Class