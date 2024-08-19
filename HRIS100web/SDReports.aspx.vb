Imports MySql.Data.MySqlClient
Public Class SDReports
	Inherits System.Web.UI.Page
	Dim dt As DataTable
	Dim sql As String
	Dim sqldata As String
	Dim admOpt As String
	Dim admSmnNo As String
	Dim admCustNo As String
	Dim grossAmt As Double = 0
	Dim vatAmt As Double = 0
	Dim fhAmt As Double = 0
	Dim discAmt As Double = 0
	Dim netAmt As Double = 0
	Dim sgrossAmt As Double = 0
	Dim svatAmt As Double = 0
	Dim sfhAmt As Double = 0
	Dim sdiscAmt As Double = 0
	Dim snetAmt As Double = 0
	Dim custno As Integer = 0
	Dim smnno As Integer = 0
	Dim rowIndex As Integer = 1
	Dim strCustName As String
	Dim TotRowIndex As Integer = 1

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
			PopReportT3()

		End If

	End Sub

	Private Sub PopCboReport()
		cboFormat.Items.Clear()
		cboFormat.Items.Add("")
		cboFormat.Items.Add("Sales Order Register - Summary")
		cboFormat.Items.Add("Sales Order Register - Detailed")
		cboFormat.Items.Add("Sales Order Materials Summary")
		cboFormat.Items.Add("Sales Invoice Register - Summary")
		cboFormat.Items.Add("Sales Invoice Register - Detailed")
		cboFormat.Items.Add("Sales Invoice Register - Detailed per Invoice")
		cboFormat.Items.Add("Free(Deal) - Detailed per Invoice")
		cboFormat.Items.Add("Sales Summary per Material")
		cboFormat.Items.Add("Sales Summary with ASP")
		cboFormat.Items.Add("Sales Summary with ASP with VAT")
		cboFormat.Items.Add("Sales Volume Customer")
		cboFormat.Items.Add("Sales Total per TSR")
		cboFormat.Items.Add("Sales Total Per Category")
		cboFormat.Items.Add("Sales Total Per Product per Category")
		cboFormat.Items.Add("Sales per Material")
		cboFormat.Items.Add("Monthly Reports")

	End Sub

	Private Sub PopReportT3()
		cboReportT3.Items.Clear()
		cboReportT3.Items.Add("")
		cboReportT3.Items.Add("Sales Invoice Summary")
		cboReportT3.Items.Add("Sales Summary Per SKU")

	End Sub

	Protected Sub lbClose_Click(sender As Object, e As EventArgs)
		Response.Redirect("SalesAndDist.aspx")

	End Sub

	Protected Sub dgvSOmon_RowDataBound(sender As Object, e As GridViewRowEventArgs)

	End Sub

	Protected Sub dgvSOmon_RowCreated(sender As Object, e As GridViewRowEventArgs)

	End Sub

	Protected Sub dgvSOdet_RowDataBound(sender As Object, e As GridViewRowEventArgs)

	End Sub

	Protected Sub dgvSOdet_RowCreated(sender As Object, e As GridViewRowEventArgs)

	End Sub

	Private Sub dpFrom_TextChanged(sender As Object, e As EventArgs) Handles dpFrom.TextChanged, dpTo.TextChanged
		If dpFrom.Text = Nothing Then
			Exit Sub
		ElseIf dpTo.Text = Nothing Then
			Exit Sub
		End If

		If Format(CDate(dpFrom.Text), "yyyy-MM-dd") > Format(CDate(dpTo.Text), "yyyy-MM-dd") Then
			Exit Sub
		End If

		If RadioButton1.Checked = False And RadioButton2.Checked = False And RadioButton3.Checked = False And RadioButton4.Checked = False Then
			RadioButton1.Checked = True
			admOpt = "ALL"
		End If

		Select Case True
			Case RadioButton1.Checked
				admOpt = "ALL"
			Case RadioButton2.Checked
				admOpt = "OPEN"
			Case RadioButton3.Checked
				admOpt = "DR Saved"
			Case RadioButton4.Checked
				admOpt = "Served"
		End Select

		PopSmnList()

	End Sub

	Private Sub PopSmnList()
		dgvSOmon.DataSource = Nothing
		dgvSOmon.DataBind()
		dgvSOdet.DataSource = Nothing
		dgvSOdet.DataBind()

		cboSalesman.Items.Clear()

		Select Case admOpt
			Case "ALL"
				dt = GetDataTable("select distinct concat(a.smnno,space(1),b.fullname) from sohdrtbl a " &
								  "left join smnmtbl b on a.smnno=b.smnno where a.status <> 'void' and " &
								  "a.transdate between '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' " &
								  "and '" & Format(CDate(dpTo.Text), "yyyy-MM-dd") & "' and b.status = 'active' order by b.fullname")
			Case Else
				dt = GetDataTable("select distinct concat(a.smnno,space(1),b.fullname) from sohdrtbl a " &
								  "left join smnmtbl b on a.smnno=b.smnno where a.status <> 'void' and " &
								  "a.transdate between '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' " &
								  "and '" & Format(CDate(dpTo.Text), "yyyy-MM-dd") & "' and b.status = 'active' " &
								  "and a.delstat = '" & admOpt & "' order by b.fullname")
		End Select

		If Not CBool(dt.Rows.Count) Then
			Exit Sub

		Else
			cboSalesman.Items.Add("")
			cboSalesman.Items.Add("ALL")
			For Each dr As DataRow In dt.Rows
				cboSalesman.Items.Add(dr.Item(0).ToString() & "")

			Next

		End If

		Call dt.Dispose()

	End Sub

	Private Sub cboSalesman_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboSalesman.SelectedIndexChanged
		If cboSalesman.Text = "" Then
			Exit Sub
		End If

		If dpFrom.Text = Nothing Then
			Exit Sub
		ElseIf dpTo.Text = Nothing Then
			Exit Sub
		End If

		If Format(CDate(dpFrom.Text), "yyyy-MM-dd") > Format(CDate(dpTo.Text), "yyyy-MM-dd") Then
			Exit Sub
		End If

		If RadioButton1.Checked = False And RadioButton2.Checked = False And RadioButton3.Checked = False And RadioButton4.Checked = False Then
			RadioButton1.Checked = True
			admOpt = "ALL"
		End If

		Select Case True
			Case RadioButton1.Checked
				admOpt = "ALL"
			Case RadioButton2.Checked
				admOpt = "OPEN"
			Case RadioButton3.Checked
				admOpt = "DR Saved"
			Case RadioButton4.Checked
				admOpt = "Served"
		End Select

		PopCustList()

	End Sub

	Private Sub PopCustList()

		dgvSOmon.DataSource = Nothing
		dgvSOmon.DataBind()
		dgvSOdet.DataSource = Nothing
		dgvSOdet.DataBind()

		admSmnNo = cboSalesman.Text.Substring(0, 3)
		cboCustomer.Items.Clear()

		Select Case cboSalesman.Text
			Case "ALL"
				Select Case UCase(admOpt)
					Case "ALL"
						dt = GetDataTable("select distinct concat(a.custno,space(1),b.bussname) from sohdrtbl a " &
										  "left join custmasttbl b on a.custno=b.custno where " &
										  "a.transdate between '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and  " &
										  "'" & Format(CDate(dpTo.Text), "yyyy-MM-dd") & "' order by b.bussname")

					Case Else
						dt = GetDataTable("select distinct  concat(a.custno,space(1),b.bussname) from sohdrtbl a " &
										  "left join custmasttbl b on a.custno=b.custno where a.delstat = '" & admOpt & "' " &
										  "and a.transdate between '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and  " &
										   "'" & Format(CDate(dpTo.Text), "yyyy-MM-dd") & "' order by b.bussname")
				End Select

			Case Else
				Select Case UCase(admOpt)
					Case "ALL"
						dt = GetDataTable("select distinct  concat(a.custno,space(1),b.bussname) from sohdrtbl a " &
										  "left join custmasttbl b on a.custno=b.custno where a.smnno = '" & admSmnNo & "' " &
										  "and a.transdate between '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and  " &
										  "'" & Format(CDate(dpTo.Text), "yyyy-MM-dd") & "' order by b.bussname")

					Case Else
						dt = GetDataTable("select distinct concat(a.custno,space(1),b.bussname) from sohdrtbl a " &
										  "left join custmasttbl b on a.custno=b.custno where a.smnno = '" & admSmnNo & "' " &
										   "and a.transdate between '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and  " &
										   "'" & Format(CDate(dpTo.Text), "yyyy-MM-dd") & "' and a.delstat = '" & admOpt & "' " &
										  "order by b.bussname")
				End Select

		End Select

		If Not CBool(dt.Rows.Count) Then
			Exit Sub
		Else
			cboCustomer.Items.Add("")
			cboCustomer.Items.Add("ALL Customers")
			For Each dr As DataRow In dt.Rows
				cboCustomer.Items.Add(dr.Item(0).ToString())

			Next
		End If

		dt.Dispose()

	End Sub

	Private Sub FillSOmon()
		dgvSOmon.DataSource = Nothing
		dgvSOmon.DataBind()
		dgvSOdet.DataSource = Nothing
		dgvSOdet.DataBind()

		SOtempTable()

		Dim adapter As New MySqlDataAdapter
		Dim ds As New DataSet()
		'Dim i As Integer = 0
		sqldata = Nothing

		admSmnNo = cboSalesman.Text.Substring(0, 3)

		Select Case UCase(admOpt)
			Case "ALL"
				Select Case cboSalesman.Text
					Case "ALL"
						Select Case cboCustomer.Text
							Case "ALL Customers"
								dt = GetDataTable("select * from sohdrtbl where transdate between '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' " &
												  "and '" & Format(CDate(dpTo.Text), "yyyy-MM-dd") & "' limit 1")
								If Not CBool(dt.Rows.Count) Then
									dgvSOmon.DataSource = Nothing
									Exit Sub

								End If

								dt.Dispose()

								sqldata = "select a.sono,a.transdate,concat(a.custno,space(1),b.bussname) as custno," &
										  "concat(a.shipto,space(1),c.bussname) as shipto,a.soamt as amt,a.status as stat," &
										  "a.delstat from sohdrtbl a left join custmasttbl b on a.custno=b.custno " &
										  "left join custmasttbl c on a.shipto=c.custno where " &
										  "a.transdate between '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and " &
										  "'" & Format(CDate(dpTo.Text), "yyyy-MM-dd") & "'"
							Case Else
								dt = GetDataTable("select * from sohdrtbl where transdate between '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and " &
												  "'" & Format(CDate(dpTo.Text), "yyyy-MM-dd") & "' and " &
												  "custno = '" & cboCustomer.Text.Substring(0, 5) & "' limit 1")
								If Not CBool(dt.Rows.Count) Then
									dgvSOmon.DataSource = Nothing
									Exit Sub

								End If

								dt.Dispose()

								sqldata = "select a.sono,a.transdate,concat(a.custno,space(1),b.bussname) as custno," &
										  "concat(a.shipto,space(1),c.bussname) as shipto,a.soamt as amt,a.status as stat," &
										  "a.delstat from sohdrtbl a left join custmasttbl b on a.custno=b.custno " &
										  "left join custmasttbl c on a.shipto=c.custno where " &
										  "a.transdate between '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and " &
										  "'" & Format(CDate(dpTo.Text), "yyyy-MM-dd") & "' and " &
										  "a.custno = '" & cboCustomer.Text.Substring(0, 5) & "'"
						End Select
					Case Else
						Select Case cboCustomer.Text
							Case "ALL Customers"
								dt = GetDataTable("select * from sohdrtbl where smnno = '" & admSmnNo & "' " &
												  "and transdate between '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and " &
												  "'" & Format(CDate(dpTo.Text), "yyyy-MM-dd") & "' limit 1")
								If Not CBool(dt.Rows.Count) Then
									dgvSOmon.DataSource = Nothing
									Exit Sub

								End If

								dt.Dispose()

								sqldata = "select a.sono,a.transdate,concat(a.custno,space(1),b.bussname) as custno," &
										  "concat(a.shipto,space(1),c.bussname) as shipto,a.soamt as amt,a.status as stat," &
										  "a.delstat from sohdrtbl a left join custmasttbl b on a.custno=b.custno " &
										  "left join custmasttbl c on a.shipto=c.custno where a.smnno = '" & admSmnNo & "' " &
										  "and a.transdate between '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and " &
										  "'" & Format(CDate(dpTo.Text), "yyyy-MM-dd") & "'"
							Case Else
								dt = GetDataTable("select * from sohdrtbl where smnno = '" & admSmnNo & "' " &
												  "and transdate between '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and " &
												  "'" & Format(CDate(dpTo.Text), "yyyy-MM-dd") & "' and " &
												  "custno = '" & cboCustomer.Text.Substring(0, 5) & "' limit 1")
								If Not CBool(dt.Rows.Count) Then
									dgvSOmon.DataSource = Nothing
									Exit Sub

								End If

								dt.Dispose()

								sqldata = "select a.sono,a.transdate,concat(a.custno,space(1),b.bussname) as custno," &
										  "concat(a.shipto,space(1),c.bussname) as shipto,a.soamt as amt,a.status as stat," &
										  "a.delstat from sohdrtbl a left join custmasttbl b on a.custno=b.custno " &
										  "left join custmasttbl c on a.shipto=c.custno where a.smnno = '" & admSmnNo & "' " &
										  "and a.transdate between '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and " &
										  "'" & Format(CDate(dpTo.Text), "yyyy-MM-dd") & "' and a.custno = '" & cboCustomer.Text.Substring(0, 5) & "'"
						End Select
				End Select

			Case Else
				Select Case cboSalesman.Text
					Case "ALL"
						Select Case cboCustomer.Text
							Case "ALL Customers"
								dt = GetDataTable("select * from sohdrtbl where transdate between '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' " &
												  "and '" & Format(CDate(dpTo.Text), "yyyy-MM-dd") & "' and delstat = '" & admOpt & "' limit 1")
								If Not CBool(dt.Rows.Count) Then
									dgvSOmon.DataSource = Nothing
									Exit Sub

								End If

								dt.Dispose()

								sqldata = "select a.sono,a.transdate,concat(a.custno,space(1),b.bussname) as custno," &
										  "concat(a.shipto,space(1),c.bussname) as shipto,a.soamt as amt,a.status as stat," &
										  "a.delstat from sohdrtbl a left join custmasttbl b on a.custno=b.custno " &
										  "left join custmasttbl c on a.shipto=c.custno where a.delstat = '" & admOpt & "' " &
										  "and a.transdate between '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and " &
										  "'" & Format(CDate(dpTo.Text), "yyyy-MM-dd") & "'"
							Case Else
								dt = GetDataTable("select * from sohdrtbl where custno = '" & cboCustomer.Text.Substring(0, 5) & "' " &
												  "and transdate between '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and " &
												  "'" & Format(CDate(dpTo.Text), "yyyy-MM-dd") & "' and delstat = '" & admOpt & "' limit 1")
								If Not CBool(dt.Rows.Count) Then
									dgvSOmon.DataSource = Nothing
									Exit Sub

								End If

								dt.Dispose()

								sqldata = "select a.sono,a.transdate,concat(a.custno,space(1),b.bussname) as custno," &
										  "concat(a.shipto,space(1),c.bussname) as shipto,a.soamt as amt,a.status as stat," &
										  "a.delstat from sohdrtbl a left join custmasttbl b on a.custno=b.custno " &
										  "left join custmasttbl c on a.shipto=c.custno where a.delstat = '" & admOpt & "'  " &
										  "and a.transdate between '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and " &
										  "'" & Format(CDate(dpTo.Text), "yyyy-MM-dd") & "' and " &
										  "a.custno = '" & cboCustomer.Text.Substring(0, 5) & "'"
						End Select
					Case Else
						Select Case cboCustomer.Text
							Case "ALL Customers"
								dt = GetDataTable("select * from sohdrtbl where smnno = '" & admSmnNo & "' " &
												  "and transdate between '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and " &
												  "'" & Format(CDate(dpTo.Text), "yyyy-MM-dd") & "' and delstat = '" & admOpt & "' limit 1")
								If Not CBool(dt.Rows.Count) Then
									dgvSOmon.DataSource = Nothing
									Exit Sub

								End If

								dt.Dispose()

								sqldata = "select a.sono,a.transdate,concat(a.custno,space(1),b.bussname) as custno," &
										  "concat(a.shipto,space(1),c.bussname) as shipto,a.soamt as amt,a.status as stat," &
										  "a.delstat from sohdrtbl a left join custmasttbl b on a.custno=b.custno " &
										  "left join custmasttbl c on a.shipto=c.custno where a.smnno = '" & admSmnNo & "' " &
										  "and a.transdate between '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and " &
										  "'" & Format(CDate(dpTo.Text), "yyyy-MM-dd") & "' and a.delstat = '" & admOpt & "'"
							Case Else
								dt = GetDataTable("select * from sohdrtbl where smnno = '" & admSmnNo & "' " &
												  "and transdate between '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and " &
												  "'" & Format(CDate(dpTo.Text), "yyyy-MM-dd") & "' and delstat = '" & admOpt & "' " &
												  "and custno = '" & cboCustomer.Text.Substring(0, 5) & "' limit 1")
								If Not CBool(dt.Rows.Count) Then
									dgvSOmon.DataSource = Nothing
									Exit Sub

								End If

								dt.Dispose()

								sqldata = "select a.sono,a.transdate,concat(a.custno,space(1),b.bussname) as custno," &
										  "concat(a.shipto,space(1),c.bussname) as shipto,a.soamt as amt,a.status as stat," &
										  "a.delstat from sohdrtbl a left join custmasttbl b on a.custno=b.custno " &
										  "left join custmasttbl c on a.shipto=c.custno where a.smnno = '" & admSmnNo & "' " &
										  "and a.transdate between '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and " &
										  "'" & Format(CDate(dpTo.Text), "yyyy-MM-dd") & "' and a.delstat = '" & admOpt & "' " &
										  "and a.custno = '" & cboCustomer.Text.Substring(0, 5) & "'"
						End Select
				End Select

		End Select

		With dgvSOmon
			.Columns(0).HeaderText = "Sel"
			.Columns(1).HeaderText = "SO No."
			.Columns(2).HeaderText = "SO Date"
			.Columns(3).HeaderText = "Customer"
			.Columns(4).HeaderText = "Ship To"
			.Columns(5).HeaderText = "SO Amount"
			.Columns(6).HeaderText = "SO Status"
			.Columns(7).HeaderText = "DO Status"

		End With

		conn.Open()
		Dim command As New MySqlCommand(sqldata, conn)
		adapter.SelectCommand = command
		adapter.Fill(ds)
		adapter.Dispose()
		command.Dispose()
		conn.Close()
		dgvSOmon.DataSource = ds.Tables(0)
		dgvSOmon.DataBind()

	End Sub

	Private Sub SOtempTable()

		admSmnNo = cboSalesman.Text.Substring(0, 3)

		sql = "delete from tempinvbals where user ='" & lblUser.Text & "'"
		ExecuteNonQuery(sql)

		Select Case UCase(admOpt)
			Case "ALL"
				Select Case cboSalesman.Text
					Case "ALL"
						Select Case cboCustomer.Text
							Case "ALL Customers"
								sql = "insert into tempinvbals(pono,venno,transdate,codeno,recqty,recwt,uc,lotno,mmdesc,user) " &
									  "select a.sono,b.custno,b.transdate,a.codeno,sum(a.qty),sum(a.wt),a.sp,b.status,b.delstat," &
									  "'" & lblUser.Text & "' from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
									  "where b.transdate between '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and " &
									  "'" & Format(CDate(dpTo.Text), "yyyy-MM-dd") & "' group by a.sono,a.codeno"
								ExecuteNonQuery(sql)
							Case Else
								sql = "insert into tempinvbals(pono,venno,transdate,codeno,recqty,recwt,uc,lotno,mmdesc,user) " &
									  "select a.sono,b.custno,b.transdate,a.codeno,sum(a.qty),sum(a.wt),a.sp,b.status,b.delstat," &
									  "'" & lblUser.Text & "' from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
									  "where b.transdate between '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and " &
									  "'" & Format(CDate(dpTo.Text), "yyyy-MM-dd") & "' and " &
									  "b.custno = '" & cboCustomer.Text.Substring(0, 5) & "' group by a.sono,a.codeno"
								ExecuteNonQuery(sql)
						End Select
					Case Else
						Select Case cboCustomer.Text
							Case "ALL Customers"
								sql = "insert into tempinvbals(pono,venno,transdate,codeno,recqty,recwt,uc,lotno,mmdesc,user) " &
									  "select a.sono,b.custno,b.transdate,a.codeno,sum(a.qty),sum(a.wt),a.sp,b.status,b.delstat," &
									  "'" & lblUser.Text & "' from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
									  "where b.transdate between '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and " &
									  "'" & Format(CDate(dpTo.Text), "yyyy-MM-dd") & "' and b.smnno = '" & admSmnNo & "' " &
									  "and b.custno = '" & cboCustomer.Text.Substring(0, 5) & "' group by a.sono,a.codeno"
								ExecuteNonQuery(sql)
							Case Else
								sql = "insert into tempinvbals(pono,venno,transdate,codeno,recqty,recwt,uc,lotno,mmdesc,user) " &
									  "select a.sono,b.custno,b.transdate,a.codeno,sum(a.qty),sum(a.wt),a.sp,b.status,b.delstat," &
									  "'" & lblUser.Text & "' from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
									  "where b.transdate between '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and " &
									  "'" & Format(CDate(dpTo.Text), "yyyy-MM-dd") & "' and b.smnno = '" & admSmnNo & "' and " &
									  "b.custno = '" & cboCustomer.Text.Substring(0, 5) & "' group by a.sono,a.codeno"
								ExecuteNonQuery(sql)
						End Select
				End Select

			Case Else
				Select Case cboSalesman.Text
					Case "ALL"
						Select Case cboCustomer.Text
							Case "ALL Customers"
								sql = "insert into tempinvbals(pono,venno,transdate,codeno,recqty,recwt,uc,lotno,mmdesc,user) " &
									  "select a.sono,b.custno,b.transdate,a.codeno,sum(a.qty),sum(a.wt),a.sp,b.status,b.delstat," &
									  "'" & lblUser.Text & "' from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
									  "where b.delstat = '" & admOpt & "' and b.transdate between '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and " &
									  "'" & Format(CDate(dpTo.Text), "yyyy-MM-dd") & "' group by a.sono,a.codeno"
								ExecuteNonQuery(sql)
							Case Else
								sql = "insert into tempinvbals(pono,venno,transdate,codeno,recqty,recwt,uc,lotno,mmdesc,user) " &
									  "select a.sono,b.custno,b.transdate,a.codeno,sum(a.qty),sum(a.wt),a.sp,b.status,b.delstat," &
									  "'" & lblUser.Text & "' from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
									  "where b.delstat = '" & admOpt & "' and b.transdate between '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and " &
									  "'" & Format(CDate(dpTo.Text), "yyyy-MM-dd") & "' and b.smnno = '" & admSmnNo & "' " &
									  "and b.custno = '" & cboCustomer.Text.Substring(0, 5) & "' group by a.sono,a.codeno"
								ExecuteNonQuery(sql)
						End Select
					Case Else
						Select Case cboCustomer.Text
							Case "ALL Customers"
								sql = "insert into tempinvbals(pono,venno,transdate,codeno,recqty,recwt,uc,lotno,mmdesc,user) " &
									  "select a.sono,b.custno,b.transdate,a.codeno,sum(a.qty),sum(a.wt),a.sp,b.status,b.delstat," &
									  "'" & lblUser.Text & "' from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
									  "where b.delstat = '" & admOpt & "' and b.transdate between '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and " &
									  "'" & Format(CDate(dpTo.Text), "yyyy-MM-dd") & "' and b.smnno = '" & admSmnNo & "' " &
									  "group by a.sono,a.codeno"
								ExecuteNonQuery(sql)
							Case Else
								sql = "insert into tempinvbals(pono,venno,transdate,codeno,recqty,recwt,uc,lotno,mmdesc,user) " &
									  "select a.sono,b.custno,b.transdate,a.codeno,sum(a.qty),sum(a.wt),a.sp,b.status,b.delstat," &
									  "'" & lblUser.Text & "' from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
									  "where b.delstat = '" & admOpt & "' and b.transdate between '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and " &
									  "'" & Format(CDate(dpTo.Text), "yyyy-MM-dd") & "' and b.smnno = '" & admSmnNo & "' " &
									  "and b.custno = '" & cboCustomer.Text.Substring(0, 5) & "' group by a.sono,a.codeno"
								ExecuteNonQuery(sql)
						End Select
				End Select

		End Select

		sql = "update tempinvbals a,(select b.sono,a.codeno,ifnull(sum(a.qty),0) as qty,ifnull(sum(a.wt),0) as wt from issdettbl a " &
			  "left join isshdrtbl b on a.dono=b.dono where b.transdate <= '" & Format(CDate(dpTo.Text), "yyyy-MM-dd") & "' and " &
			  "b.status <> 'void' group by b.sono,a.codeno) as b set a.issqty = b.qty,a.isswt = b.wt where a.pono = b.sono and " &
			  "a.codeno=b.codeno and a.user = '" & lblUser.Text & "'"
		ExecuteNonQuery(sql)

		sql = "update tempinvbals set qtybal = ifnull(recqty,0) - ifnull(issqty,0),wtbal = ifnull(recwt,0) - ifnull(isswt,0) where user = '" & lblUser.Text & "'"
		ExecuteNonQuery(sql)

	End Sub

	Private Sub cboCustomer_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboCustomer.SelectedIndexChanged

		If cboCustomer.Text = "" Then
			Exit Sub

		End If

		Select Case True
			Case RadioButton1.Checked
				admOpt = "ALL"
			Case RadioButton2.Checked
				admOpt = "OPEN"
			Case RadioButton3.Checked
				admOpt = "DR SAVED"
			Case RadioButton4.Checked
				admOpt = "SERVED"
		End Select

		'FillSOmon()
		dgvSOdet.DataSource = Nothing
		dgvSOdet.DataBind()

		FillSOmon()

	End Sub

	Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged, RadioButton2.CheckedChanged,
			RadioButton3.CheckedChanged, RadioButton4.CheckedChanged

		dgvSOmon.DataSource = Nothing
		dgvSOmon.DataBind()
		dgvSOdet.DataSource = Nothing
		dgvSOdet.DataBind()
		cboSalesman.Items.Clear()
		cboCustomer.Items.Clear()

	End Sub

	Private Sub dgvSOmon_SelectedIndexChanged(sender As Object, e As EventArgs) Handles dgvSOmon.SelectedIndexChanged
		FillDgvSOdet()

	End Sub

	Private Sub FillDgvSOdet()
		dt = GetDataTable("select * from tempinvbals where user = '" & lblUser.Text & "' and " &
						  "pono = '" & dgvSOmon.SelectedRow.Cells(1).Text & "'")
		If Not CBool(dt.Rows.Count) Then
			dgvSOdet.DataSource = Nothing
			dgvSOdet.DataBind()
			Exit Sub

		End If

		dt.Dispose()

		Dim adapter As New MySqlDataAdapter
		Dim ds As New DataSet()

		sqldata = Nothing

		sqldata = "select a.codeno,ifnull(c.codename,c.mmdesc) as mmdesc,ifnull(a.recqty,0) as qty," &
				  "ifnull(a.recwt,0) as wt,ifnull(a.issqty,0) as doqty,ifnull(a.isswt,0) as dowt," &
				  "ifnull(a.qtybal,0) as qtybal,ifnull(a.wtbal,0) as wtbal from tempinvbals a  " &
				  "left join mmasttbl c on a.codeno = c.codeno where a.user = '" & lblUser.Text & "' " &
				  "and a.pono = '" & dgvSOmon.SelectedRow.Cells(1).Text & "'"

		With dgvSOdet
			.Columns(0).HeaderText = "Code No."
			.Columns(1).HeaderText = "Product"
			.Columns(2).HeaderText = "SO Qty"
			.Columns(3).HeaderText = "SO Wt"
			.Columns(4).HeaderText = "DO Qty"
			.Columns(5).HeaderText = "DO Wt/Vol"
			.Columns(6).HeaderText = "Qty Bal"
			.Columns(7).HeaderText = "Wt/Vol Bal"

		End With


		conn.Open()
		Dim command As New MySqlCommand(sqldata, conn)
		adapter.SelectCommand = command
		adapter.Fill(ds)
		adapter.Dispose()
		command.Dispose()
		conn.Close()
		dgvSOdet.DataSource = ds.Tables(0)
		dgvSOdet.DataBind()

	End Sub

	Protected Sub dgvSalesSum_RowDataBound(sender As Object, e As GridViewRowEventArgs)

	End Sub

	Protected Sub dgvSalesSum_RowCreated(sender As Object, e As GridViewRowEventArgs)

	End Sub

	Protected Sub dgvSalesDet_RowDataBound(sender As Object, e As GridViewRowEventArgs)

	End Sub

	Protected Sub dgvSalesDet_RowCreated(sender As Object, e As GridViewRowEventArgs)

	End Sub

	Private Sub dpT3DFrom_TextChanged(sender As Object, e As EventArgs) Handles dpT3DFrom.TextChanged, dpT3DTo.TextChanged
		If cboReportT3.Text = "" Then
			lblMsg.Text = "Select Report Option First"
			Exit Sub
		ElseIf dpT3DFrom.Text = Nothing Then
			lblMsg.Text = "Set Date From"
			Exit Sub
		ElseIf dpT3DTo.Text = Nothing Then
			lblMsg.Text = "Set Date To"
			Exit Sub
		End If

		If Format(CDate(dpT3DFrom.Text), "yyyy-MM-dd") > Format(CDate(dpT3DTo.Text), "yyyy-MM-dd") Then
			lblMsg.Text = "Invalid Date"
			Exit Sub
		End If

		PopSmnT3()

	End Sub

	Private Sub PopSmnT3()
		cboSmnT3.Items.Clear()
		dt = GetDataTable("select distinct concat(a.smnno,space(1),b.fullname) from saleshdrtbl a " &
						  "left join smnmtbl b on a.smnno=b.smnno where a.status <> 'void' and " &
						  "a.transdate between '" & Format(CDate(dpT3DFrom.Text), "yyyy-MM-dd") & "' " &
						  "and '" & Format(CDate(dpT3DTo.Text), "yyyy-MM-dd") & "' order by b.fullname")

		If Not CBool(dt.Rows.Count) Then
			Exit Sub

		Else
			cboSmnT3.Items.Add("")
			cboSmnT3.Items.Add("ALL")
			For Each dr As DataRow In dt.Rows
				cboSmnT3.Items.Add(dr.Item(0).ToString() & "")

			Next

		End If

		Call dt.Dispose()

		InitT3()

		Select Case cboReportT3.Text
			Case "Sales Invoice Summary"
				dgvSalesSum.DataSource = Nothing
				dgvSalesSum.DataBind()
			Case "Sales Summary Per SKU"
				dgvSalesDet.DataSource = Nothing
				dgvSalesDet.DataBind()
		End Select

	End Sub

	Private Sub cboSmnT3_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboSmnT3.SelectedIndexChanged
		If cboReportT3.Text = "" Then
			lblMsg.Text = "Select Report Option First"
			Exit Sub
		ElseIf dpT3DFrom.Text = Nothing Then
			lblMsg.Text = "Set Date From"
			Exit Sub
		ElseIf dpT3DTo.Text = Nothing Then
			lblMsg.Text = "Set Date To"
			Exit Sub
		ElseIf cboSmnT3.Text = "" Then
			lblMsg.Text = "Select Salesman"
			Exit Sub
		End If

		If Format(CDate(dpT3DFrom.Text), "yyyy-MM-dd") > Format(CDate(dpT3DTo.Text), "yyyy-MM-dd") Then
			lblMsg.Text = "Invalid Date"
			Exit Sub
		End If

		PopCustT3()

	End Sub

	Private Sub PopCustT3()
		cboCustT3.Items.Clear()
		Select Case cboSmnT3.Text
			Case "ALL"
				dt = GetDataTable("select distinct concat(a.custno,space(1),b.bussname) from saleshdrtbl a " &
								  "left join custmasttbl b on a.custno=b.custno where a.status <> 'void' and " &
								  "a.transdate between '" & Format(CDate(dpT3DFrom.Text), "yyyy-MM-dd") & "' " &
								  "and '" & Format(CDate(dpT3DTo.Text), "yyyy-MM-dd") & "' order by b.bussname")
			Case Else
				admSmnNo = cboSmnT3.Text.Substring(0, 3)
				dt = GetDataTable("select distinct concat(a.custno,space(1),b.bussname) from saleshdrtbl a " &
								  "left join custmasttbl b on a.custno=b.custno where a.status <> 'void' and " &
								  "a.transdate between '" & Format(CDate(dpT3DFrom.Text), "yyyy-MM-dd") & "' " &
								  "and '" & Format(CDate(dpT3DTo.Text), "yyyy-MM-dd") & "' and " &
								  "a.smnno = '" & admSmnNo & "' order by b.bussname")
		End Select

		If Not CBool(dt.Rows.Count) Then
			Exit Sub

		Else
			cboCustT3.Items.Add("")
			cboCustT3.Items.Add("ALL")
			For Each dr As DataRow In dt.Rows
				cboCustT3.Items.Add(dr.Item(0).ToString() & "")

			Next

		End If

		Call dt.Dispose()

		Select Case cboReportT3.Text
			Case "Sales Invoice Summary"
				dgvSalesSum.DataSource = Nothing
				dgvSalesSum.DataBind()
			Case "Sales Summary Per SKU"
				dgvSalesDet.DataSource = Nothing
				dgvSalesDet.DataBind()
		End Select

		InitT3()

	End Sub

	Private Sub cboCustT3_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboCustT3.SelectedIndexChanged
		If cboReportT3.Text = "" Then
			lblMsg.Text = "Select Report Option First"
			Exit Sub
		ElseIf dpT3DFrom.Text = Nothing Then
			lblMsg.Text = "Set Date From"
			Exit Sub
		ElseIf dpT3DTo.Text = Nothing Then
			lblMsg.Text = "Set Date To"
			Exit Sub
		ElseIf cboSmnT3.Text = "" Then
			lblMsg.Text = "Select Salesman"
			Exit Sub
		ElseIf cboCustT3.Text = "" Then
			lblMsg.Text = "Select Customer"
			Exit Sub
		End If

		If Format(CDate(dpT3DFrom.Text), "yyyy-MM-dd") > Format(CDate(dpT3DTo.Text), "yyyy-MM-dd") Then
			lblMsg.Text = "Invalid Date"
			Exit Sub
		End If

		Select Case cboReportT3.Text
			Case "Sales Invoice Summary"
				Panel4.Visible = False
				Panel3.Visible = True
				FilldgvSalesSummary()
			Case "Sales Summary Per SKU"
				Panel3.Visible = False
				Panel4.Visible = True
				FilldgvSalesDet()
		End Select

	End Sub

	Private Sub FilldgvSalesDet()
		InitT3()

		sql = "delete from tempasp where user = '" & lblUser.Text & "'"
		ExecuteNonQuery(sql)

		Select Case cboSmnT3.Text
			Case "ALL"
				Select Case cboCustT3.Text
					Case "ALL"
						sql = "insert into tempasp(codeno,mmdesc,qty,wt,amt,billref,pc,pclass,user) select a.codeno,b.mmdesc,sum(a.qty)," &
							  "sum(a.wt),sum(a.itmamt),b.billref,b.pc,d.pclass,'" & lblUser.Text & "' from salesdettbl a " &
							  "left join mmasttbl b on a.codeno=b.codeno left join saleshdrtbl c on a.invno=c.invno " &
							  "left join pctrtbl d on b.pc=d.pc where c.transdate between '" & Format(CDate(dpT3DFrom.Text), "yyyy-MM-dd") & "' and " &
							  "'" & Format(CDate(dpT3DTo.Text), "yyyy-MM-dd") & "' and c.status <> 'void' " &
							  "and a.sp <> 0 and c.user <> 'Admin' group by a.codeno"
						ExecuteNonQuery(sql)

					Case Else
						admCustNo = cboCustT3.Text.Substring(0, 5)
						sql = "insert into tempasp(codeno,mmdesc,qty,wt,amt,billref,pc,pclass,user) select a.codeno,b.mmdesc,sum(a.qty)," &
							  "sum(a.wt),sum(a.itmamt),b.billref,b.pc,d.pclass,'" & lblUser.Text & "' from salesdettbl a " &
							  "left join mmasttbl b on a.codeno=b.codeno left join saleshdrtbl c on a.invno=c.invno " &
							  "left join pctrtbl d on b.pc=d.pc where c.transdate between '" & Format(CDate(dpT3DFrom.Text), "yyyy-MM-dd") & "' and " &
							  "'" & Format(CDate(dpT3DTo.Text), "yyyy-MM-dd") & "' and c.status <> 'void' " &
							  "and a.sp <> 0 and c.user <> 'Admin' and c.custno = '" & admCustNo & "' group by a.codeno"
						ExecuteNonQuery(sql)
				End Select

			Case Else
				admSmnNo = cboSmnT3.Text.Substring(0, 3)
				Select Case cboCustT3.Text
					Case "ALL"
						sql = "insert into tempasp(codeno,mmdesc,qty,wt,amt,billref,pc,pclass,user) select a.codeno,b.mmdesc,sum(a.qty)," &
							  "sum(a.wt),sum(a.itmamt),b.billref,b.pc,d.pclass,'" & lblUser.Text & "' from salesdettbl a " &
							  "left join mmasttbl b on a.codeno=b.codeno left join saleshdrtbl c on a.invno=c.invno " &
							  "left join pctrtbl d on b.pc=d.pc where c.transdate between '" & Format(CDate(dpT3DFrom.Text), "yyyy-MM-dd") & "' and " &
							  "'" & Format(CDate(dpT3DTo.Text), "yyyy-MM-dd") & "' and c.status <> 'void' " &
							  "and a.sp <> 0 and c.user <> 'Admin' and c.smnno = '" & admSmnNo & "' group by a.codeno"
						ExecuteNonQuery(sql)

					Case Else
						admCustNo = cboCustT3.Text.Substring(0, 5)
						sql = "insert into tempasp(codeno,mmdesc,qty,wt,amt,billref,pc,pclass,user) select a.codeno,b.mmdesc,sum(a.qty)," &
							  "sum(a.wt),sum(a.itmamt),b.billref,b.pc,d.pclass,'" & lblUser.Text & "' from salesdettbl a " &
							  "left join mmasttbl b on a.codeno=b.codeno left join saleshdrtbl c on a.invno=c.invno " &
							  "left join pctrtbl d on b.pc=d.pc where c.transdate between '" & Format(CDate(dpT3DFrom.Text), "yyyy-MM-dd") & "' and " &
							  "'" & Format(CDate(dpT3DTo.Text), "yyyy-MM-dd") & "' and c.status <> 'void' " &
							  "and a.sp <> 0 and c.user <> 'Admin' and c.smnno = '" & admSmnNo & "' and " &
							  "c.custno = '" & admCustNo & "' group by a.codeno"
						ExecuteNonQuery(sql)
				End Select
		End Select

		sql = "update tempasp set asp = round((amt/qty),2) where billref = 'qty' and user = '" & lblUser.Text & "'"
		ExecuteNonQuery(sql)

		sql = "update tempasp set asp = round((amt/wt),2) where billref = 'wt' and user = '" & lblUser.Text & "'"
		ExecuteNonQuery(sql)

		sql = "update tempasp a left join mmasttbl b on a.codeno=b.codeno set a.mmgrpdesc = b.mmgrp," &
			  "a.pc=b.pc where a.user = '" & lblUser.Text & "'"
		ExecuteNonQuery(sql)

		sql = "update tempasp a left join mmasttbl b on a.codeno=b.codeno set a.mmgrp=b.mmgrp where a.user = '" & lblUser.Text & "'"
		ExecuteNonQuery(sql)

		Dim adapter As New MySqlDataAdapter
		Dim ds As New DataSet()
		'Dim i As Integer = 0
		sqldata = Nothing

		dt = GetDataTable("select sum(ifnull(qty,0)),sum(ifnull(wt,0)),sum(ifnull(amt,0)) from tempasp " &
						  "where user = '" & lblUser.Text & "'")
		If Not CBool(dt.Rows.Count) Then
			dgvSalesDet.DataSource = Nothing
			dgvSalesDet.DataBind()
			lblGrossAmt.Text = "0.00"
			lblDiscAmt.Text = "0.00"
			lblVatAmt.Text = "0.00"
			lblNetAmt.Text = ""
			Exit Sub

		Else
			For Each dr As DataRow In dt.Rows
				lblGrossAmt.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0.00 ; (#,##0.00)")
				lblDiscAmt.Text = Format(CDbl(dr.Item(1).ToString()), "#,##0.00 ; (#,##0.00)")
				lblVatAmt.Text = Format(CDbl(dr.Item(2).ToString()), "#,##0.00 ; (#,##0.00)")
				lblNetAmt.Text = ""
			Next

		End If

		Call dt.Dispose()

		sqldata = "Select codeno,mmdesc,qty,wt,amt,asp from tempasp where user = '" & lblUser.Text & "'"

		With dgvSalesDet
			.Columns(0).HeaderText = "Code No."
			.Columns(1).HeaderText = "Description"
			.Columns(2).HeaderText = "Qty"
			.Columns(3).HeaderText = "Wt/Vol."
			.Columns(4).HeaderText = "Amount"
			.Columns(5).HeaderText = "ASP"

		End With

		conn.Open()
		Dim command As New MySqlCommand(sqldata, conn)
		adapter.SelectCommand = command
		adapter.Fill(ds)
		adapter.Dispose()
		command.Dispose()
		conn.Close()
		dgvSalesDet.DataSource = ds.Tables(0)
		dgvSalesDet.DataBind()

	End Sub

	Private Sub FilldgvSalesSummary()
		InitT3()

		Dim adapter As New MySqlDataAdapter
		Dim ds As New DataSet()
		'Dim i As Integer = 0
		sqldata = Nothing

		Select Case cboSmnT3.Text
			Case "ALL"
				Select Case cboCustT3.Text
					Case "ALL"
						dt = GetDataTable("select sum(ifnull(grossamt,0)),sum(ifnull(discamt,0)),sum(ifnull(vat,0))," &
										  "sum(ifnull(netamt,0)) from saleshdrtbl where status <> 'void' and " &
										  "transdate between '" & Format(CDate(dpT3DFrom.Text), "yyyy-MM-dd") & "' and " &
										  "'" & Format(CDate(dpT3DTo.Text), "yyyy-MM-dd") & "'")
						If Not CBool(dt.Rows.Count) Then
							lblGrossAmt.Text = "0.00"
							lblDiscAmt.Text = "0.00"
							lblVatAmt.Text = "0.00"
							lblNetAmt.Text = "0.00"
							Exit Sub

						Else
							For Each dr As DataRow In dt.Rows
								lblGrossAmt.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0.00 ; (#,##0.00)")
								lblDiscAmt.Text = Format(CDbl(dr.Item(1).ToString()), "#,##0.00 ; (#,##0.00)")
								lblVatAmt.Text = Format(CDbl(dr.Item(2).ToString()), "#,##0.00 ; (#,##0.00)")
								lblNetAmt.Text = Format(CDbl(dr.Item(3).ToString()), "#,##0.00 ; (#,##0.00)")

							Next

						End If

						Call dt.Dispose()

						sqldata = "Select a.invno,a.tc,a.transdate,a.docno,concat(a.custno,space(1),b.bussname) As custno," &
								  "concat(a.shipto,space(1),c.bussname) As shipto,a.grossamt,a.discamt," &
								  "a.vat,a.netamt from saleshdrtbl a left join custmasttbl b On a.custno=b.custno " &
								  "left join custmasttbl c On a.shipto=c.custno where a.status <> 'void' and " &
								  "a.transdate between '" & Format(CDate(dpT3DFrom.Text), "yyyy-MM-dd") & "' and " &
								  "'" & Format(CDate(dpT3DTo.Text), "yyyy-MM-dd") & "'"
					Case Else
						admCustNo = cboCustT3.Text.Substring(0, 5)
						dt = GetDataTable("select sum(ifnull(grossamt,0)),sum(ifnull(discamt,0)),sum(ifnull(vat,0))," &
										  "sum(ifnull(netamt,0)) from saleshdrtbl where status <> 'void' and " &
										  "transdate between '" & Format(CDate(dpT3DFrom.Text), "yyyy-MM-dd") & "' and " &
										  "'" & Format(CDate(dpT3DTo.Text), "yyyy-MM-dd") & "' and " &
										  "custno = '" & admCustNo & "'")

						If Not CBool(dt.Rows.Count) Then
							lblGrossAmt.Text = "0.00"
							lblDiscAmt.Text = "0.00"
							lblVatAmt.Text = "0.00"
							lblNetAmt.Text = "0.00"
							Exit Sub

						Else
							For Each dr As DataRow In dt.Rows
								lblGrossAmt.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0.00 ; (#,##0.00)")
								lblDiscAmt.Text = Format(CDbl(dr.Item(1).ToString()), "#,##0.00 ; (#,##0.00)")
								lblVatAmt.Text = Format(CDbl(dr.Item(2).ToString()), "#,##0.00 ; (#,##0.00)")
								lblNetAmt.Text = Format(CDbl(dr.Item(3).ToString()), "#,##0.00 ; (#,##0.00)")

							Next

						End If

						Call dt.Dispose()

						sqldata = "Select a.invno,a.tc,a.transdate,a.docno,concat(a.custno,space(1),b.bussname) As custno," &
								  "concat(a.shipto,space(1),c.bussname) As shipto,a.grossamt,a.discamt," &
								  "a.vat,a.netamt from saleshdrtbl a left join custmasttbl b On a.custno=b.custno " &
								  "left join custmasttbl c On a.shipto=c.custno where a.status <> 'void' and " &
								  "a.transdate between '" & Format(CDate(dpT3DFrom.Text), "yyyy-MM-dd") & "' and " &
								  "'" & Format(CDate(dpT3DTo.Text), "yyyy-MM-dd") & "' and a.custno = '" & admCustNo & "'"
				End Select
			Case Else
				admSmnNo = cboSmnT3.Text.Substring(0, 3)
				Select Case cboCustT3.Text
					Case "ALL"
						dt = GetDataTable("select sum(ifnull(grossamt,0)),sum(ifnull(discamt,0)),sum(ifnull(vat,0))," &
										  "sum(ifnull(netamt,0)) from saleshdrtbl where status <> 'void' and " &
										  "transdate between '" & Format(CDate(dpT3DFrom.Text), "yyyy-MM-dd") & "' and " &
										  "'" & Format(CDate(dpT3DTo.Text), "yyyy-MM-dd") & "' " &
										  "and smnno = '" & admSmnNo & "'")
						If Not CBool(dt.Rows.Count) Then
							lblGrossAmt.Text = "0.00"
							lblDiscAmt.Text = "0.00"
							lblVatAmt.Text = "0.00"
							lblNetAmt.Text = "0.00"
							Exit Sub

						Else
							For Each dr As DataRow In dt.Rows
								lblGrossAmt.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0.00 ; (#,##0.00)")
								lblDiscAmt.Text = Format(CDbl(dr.Item(1).ToString()), "#,##0.00 ; (#,##0.00)")
								lblVatAmt.Text = Format(CDbl(dr.Item(2).ToString()), "#,##0.00 ; (#,##0.00)")
								lblNetAmt.Text = Format(CDbl(dr.Item(3).ToString()), "#,##0.00 ; (#,##0.00)")

							Next

						End If

						Call dt.Dispose()

						sqldata = "Select a.invno,a.tc,a.transdate,a.docno,concat(a.custno,space(1),b.bussname) As custno," &
								  "concat(a.shipto,space(1),c.bussname) As shipto,a.grossamt,a.discamt," &
								  "a.vat,a.netamt from saleshdrtbl a left join custmasttbl b On a.custno=b.custno " &
								  "left join custmasttbl c On a.shipto=c.custno where a.status <> 'void' and " &
								  "a.transdate between '" & Format(CDate(dpT3DFrom.Text), "yyyy-MM-dd") & "' and " &
								  "'" & Format(CDate(dpT3DTo.Text), "yyyy-MM-dd") & "'and a.smnno = '" & admSmnNo & "'"
					Case Else
						admCustNo = cboCustT3.Text.Substring(0, 5)

						dt = GetDataTable("select sum(ifnull(grossamt,0)),sum(ifnull(discamt,0)),sum(ifnull(vat,0))," &
										  "sum(ifnull(netamt,0)) from saleshdrtbl where status <> 'void' and " &
										  "transdate between '" & Format(CDate(dpT3DFrom.Text), "yyyy-MM-dd") & "' and " &
										  "'" & Format(CDate(dpT3DTo.Text), "yyyy-MM-dd") & "' " &
										  "and smnno = '" & admSmnNo & "' and custno = '" & admCustNo & "'")

						If Not CBool(dt.Rows.Count) Then
							lblGrossAmt.Text = "0.00"
							lblDiscAmt.Text = "0.00"
							lblVatAmt.Text = "0.00"
							lblNetAmt.Text = "0.00"
							Exit Sub

						Else
							For Each dr As DataRow In dt.Rows
								lblGrossAmt.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0.00 ; (#,##0.00)")
								lblDiscAmt.Text = Format(CDbl(dr.Item(1).ToString()), "#,##0.00 ; (#,##0.00)")
								lblVatAmt.Text = Format(CDbl(dr.Item(2).ToString()), "#,##0.00 ; (#,##0.00)")
								lblNetAmt.Text = Format(CDbl(dr.Item(3).ToString()), "#,##0.00 ; (#,##0.00)")

							Next

						End If

						Call dt.Dispose()

						sqldata = "Select a.invno,a.tc,a.transdate,a.docno,concat(a.custno,space(1),b.bussname) As custno," &
								  "concat(a.shipto,space(1),c.bussname) As shipto,a.grossamt,a.discamt," &
								  "a.vat,a.netamt from saleshdrtbl a left join custmasttbl b On a.custno=b.custno " &
								  "left join custmasttbl c On a.shipto=c.custno where a.status <> 'void' and " &
								  "a.transdate between '" & Format(CDate(dpT3DFrom.Text), "yyyy-MM-dd") & "' and " &
								  "'" & Format(CDate(dpT3DTo.Text), "yyyy-MM-dd") & "'and a.smnno = '" & admSmnNo & "' " &
								  "and a.custno = '" & admCustNo & "'"
				End Select

		End Select

		With dgvSalesSum
			.Columns(0).HeaderText = "Inv No."
			.Columns(1).HeaderText = "TC"
			.Columns(2).HeaderText = "Date"
			.Columns(3).HeaderText = "Doc No."
			.Columns(4).HeaderText = "Sold To"
			.Columns(5).HeaderText = "Ship To"
			.Columns(6).HeaderText = "Gross Amt"
			.Columns(7).HeaderText = "Disc Amt"
			.Columns(8).HeaderText = "VAT Amt"
			.Columns(9).HeaderText = "Net Amt"

		End With

		conn.Open()
		Dim command As New MySqlCommand(sqldata, conn)
		adapter.SelectCommand = command
		adapter.Fill(ds)
		adapter.Dispose()
		command.Dispose()
		conn.Close()
		dgvSalesSum.DataSource = ds.Tables(0)
		dgvSalesSum.DataBind()

	End Sub

	Private Sub InitT3()
		lblGrossAmt.Text = "0.00"
		lblDiscAmt.Text = "0.00"
		lblVatAmt.Text = "0.00"
		lblNetAmt.Text = "0.00"
		dgvSalesSum.DataSource = Nothing
		dgvSalesSum.DataBind()
		dgvSalesDet.DataSource = Nothing
		dgvSalesDet.DataBind()

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
				lblTitle.Text = TabPanel4.HeaderText'TabPanel4.HeaderText
			Case 4
				lblTitle.Text = "Not Yet Available" 'TabPanel5.HeaderText
			Case 5
				lblTitle.Text = TabPanel6.HeaderText
		End Select


	End Sub

	Private Sub dpPOdate1_TextChanged(sender As Object, e As EventArgs) Handles dpPOdate1.TextChanged, dpPOdate2.TextChanged
		If dpPOdate1.Text = Nothing Then
			Exit Sub
		ElseIf dpPOdate2.Text = Nothing Then
			Exit Sub
		End If

		If Format(CDate(dpPOdate1.Text), "yyyy-MM-dd") > Format(CDate(dpPOdate2.Text), "yyyy-MM-dd") Then
			Exit Sub
		End If

		GetCustPOlist()

	End Sub

	Private Sub GetCustPOlist()
		cboCustPO.Items.Clear()
		dt = GetDataTable("select distinct concat(a.custno,space(1),b.bussname) from sohdrtbl a left join custmasttbl b on a.custno=b.custno " &
						  "where a.transdate between '" & Format(CDate(dpPOdate1.Text), "yyyy-MM-dd") & "' and " &
						  "'" & Format(CDate(dpPOdate2.Text), "yyyy-MM-dd") & "' and a.status <> 'void' and " &
						  "length(a.pono) > 3 order by b.bussname")
		If Not CBool(dt.Rows.Count) Then
			lblMsg.Text = ("No Customer Found from the Dates Selected")
			Exit Sub
		Else
			cboCustPO.Items.Add("")
			For Each dr As DataRow In dt.Rows
				cboCustPO.Items.Add(dr.Item(0).ToString())
			Next

		End If

		dt.Dispose()

	End Sub

	Private Sub cboCustPO_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboCustPO.SelectedIndexChanged
		dgvPODet.DataSource = Nothing
		dgvPODet.DataBind()
		dgvPOsum.DataSource = Nothing
		dgvPOsum.DataBind()

		If dpPOdate1.Text = Nothing Then
			Exit Sub
		ElseIf dpPOdate2.Text = Nothing Then
			Exit Sub
		End If

		If Format(CDate(dpPOdate1.Text), "yyyy-MM-dd") > Format(CDate(dpPOdate2.Text), "yyyy-MM-dd") Then
			Exit Sub
		End If

		If cboCustPO.Text = "" Then
			Exit Sub
		End If

		'If Not Me.IsPostBack Then
		'	FillPOlist()
		'End If

		FillPOlist()

	End Sub

	Private Sub FillPOlist()
		tvPOlist.Nodes.Clear()

		dt = GetDataTable("select distinct pono from sohdrtbl where transdate between '" & Format(CDate(dpPOdate1.Text), "yyyy-MM-dd") & "' and " &
						  "'" & Format(CDate(dpPOdate2.Text), "yyyy-MM-dd") & "' and custno = '" & cboCustPO.Text.Substring(0, 5) & "' " &
						  "and status <> 'void' order by transdate")
		If Not CBool(dt.Rows.Count) Then
			lblMsg.Text = ("No PO Found")
			Exit Sub

		Else
			Dim root1 = New TreeNode("PO List")
			tvPOlist.Nodes.Add(root1)
			For Each dr As DataRow In dt.Rows
				tvPOlist.Nodes.Add(New TreeNode(dr.Item(0).ToString()))

			Next

		End If

		tvPOlist.Nodes(0).Expand()

		dt.Dispose()

	End Sub

	Private Sub tvPOlist_SelectedNodeChanged(sender As Object, e As EventArgs) Handles tvPOlist.SelectedNodeChanged
		If tvPOlist.SelectedNode.Text = "PO List" Then
			Exit Sub
		Else
			lblMsg.Text = "PO No.: " & tvPOlist.SelectedNode.Text
			dgvPODet.DataSource = Nothing
			dgvPODet.DataBind()
			dgvPOsum.DataSource = Nothing
			dgvPOsum.DataBind()
			FillSOwithPO()

		End If

	End Sub

	Private Sub FillSOwithPO()
		dt = GetDataTable("select c.invno from sohdrtbl a left join isshdrtbl b on a.sono=b.sono left join saleshdrtbl c on b.dono=c.dono " &
						  "where a.pono = '" & tvPOlist.SelectedNode.Text & "' and c.custno = '" & cboCustPO.Text.Substring(0, 5) & "'")
		If Not CBool(dt.Rows.Count) Then
			dgvPODet.DataSource = Nothing
			dgvPODet.DataBind()
			lblMsg.Text = "No Invoice found for PO No. " & tvPOlist.SelectedNode.Text
			Exit Sub

		End If

		dt.Dispose()

		Dim adapter As New MySqlDataAdapter
		Dim ds As New DataSet()

		sqldata = Nothing

		sqldata = "select a.invno,b.transdate,b.docno,a.codeno,ifnull(e.codename,e.mmdesc) as mmdesc,a.qty,a.wt,a.sp,a.itmamt from " &
				  "salesdettbl a left join saleshdrtbl b on a.invno=b.invno left join isshdrtbl c on b.dono=c.dono " &
				  "left join sohdrtbl d on d.sono=c.sono left join mmasttbl e on a.codeno=e.codeno where " &
				  "d.pono = '" & tvPOlist.SelectedNode.Text & "' and b.custno = '" & cboCustPO.Text.Substring(0, 5) & "' " &
				  "order by b.transdate,b.invno"

		With dgvPODet
			.Columns(0).HeaderText = "Doc No."
			.Columns(1).HeaderText = "Date"
			.Columns(2).HeaderText = "SI/DR"
			.Columns(3).HeaderText = "Code No"
			.Columns(4).HeaderText = "Description"
			.Columns(5).HeaderText = "Qty"
			.Columns(6).HeaderText = "Wt/Vol"
			.Columns(7).HeaderText = "SP"
			.Columns(8).HeaderText = "Amount"


		End With

		conn.Open()
		Dim command As New MySqlCommand(sqldata, conn)
		adapter.SelectCommand = command
		adapter.Fill(ds)
		adapter.Dispose()
		command.Dispose()
		conn.Close()
		dgvPODet.DataSource = ds.Tables(0)
		dgvPODet.DataBind()

		FillPOsum()

	End Sub

	Private Sub FillPOsum()
		dt = GetDataTable("select c.invno from sohdrtbl a left join isshdrtbl b on a.sono=b.sono left join saleshdrtbl c on b.dono=c.dono " &
						  "where a.pono = '" & tvPOlist.SelectedNode.Text & "' and c.custno = '" & cboCustPO.Text.Substring(0, 5) & "'")
		If Not CBool(dt.Rows.Count) Then
			dgvPOsum.DataSource = Nothing
			dgvPOsum.DataBind()
			Exit Sub

		End If

		dt.Dispose()

		Dim adapter As New MySqlDataAdapter
		Dim ds As New DataSet()

		sqldata = Nothing

		sqldata = "select a.codeno,ifnull(e.codename,e.mmdesc) as mmdesc,sum(a.qty) as qty,sum(a.wt) as wt,sum(a.itmamt) as amt " &
				  "from salesdettbl a left join saleshdrtbl b on a.invno=b.invno left join isshdrtbl c on b.dono=c.dono " &
				  "left join sohdrtbl d on d.sono=c.sono left join mmasttbl e on a.codeno=e.codeno where " &
				  "d.pono = '" & tvPOlist.SelectedNode.Text & "' and b.custno = '" & cboCustPO.Text.Substring(0, 5) & "' " &
				  "group by a.codeno"

		With dgvPOsum
			.Columns(0).HeaderText = "Code No."
			.Columns(1).HeaderText = "Description"
			.Columns(2).HeaderText = "Qty"
			.Columns(3).HeaderText = "Wt/Vol"
			.Columns(4).HeaderText = "Amount"

		End With

		conn.Open()
		Dim command As New MySqlCommand(sqldata, conn)
		adapter.SelectCommand = command
		adapter.Fill(ds)
		adapter.Dispose()
		command.Dispose()
		conn.Close()
		dgvPOsum.DataSource = ds.Tables(0)
		dgvPOsum.DataBind()

	End Sub

	Protected Sub dgvPODet_RowDataBound(sender As Object, e As GridViewRowEventArgs)

	End Sub

	Protected Sub dgvPODet_RowCreated(sender As Object, e As GridViewRowEventArgs)

	End Sub

	Protected Sub dgvPOsum_RowDataBound(sender As Object, e As GridViewRowEventArgs)

	End Sub

	Protected Sub dgvPOsum_RowCreated(sender As Object, e As GridViewRowEventArgs)

	End Sub

	Private Sub cboReportT3_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboReportT3.SelectedIndexChanged
		InitT3()

	End Sub

	Private Sub dpTransDate_TextChanged(sender As Object, e As EventArgs) Handles dpTransDate.TextChanged, dpTransDate2.TextChanged
		If dpTransDate.Text = Nothing Then
			Exit Sub
		ElseIf dpTransDate2.Text = Nothing Then
			Exit Sub
		ElseIf CDate(dpTransDate.Text) > CDate(dpTransDate2.Text) Then
			Exit Sub

		End If

		cboFilter1.Items.Clear()
		cboFilter2.Items.Clear()
		cboFilter3.Items.Clear()
		txtText1.Text = ""
		txtText2.Text = ""

		PopCboReport()

	End Sub

	Private Sub cboFormat_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboFormat.SelectedIndexChanged
		dgvRegSum.DataSource = Nothing
		dgvRegSum.DataBind()
		cboFilter1.Items.Clear()

		If dpTransDate.Text = Nothing Then
			Exit Sub
		ElseIf dpTransDate2.Text = Nothing Then
			Exit Sub
		ElseIf CDate(dpTransDate.Text) > CDate(dpTransDate2.Text) Then
			Exit Sub
		ElseIf cboFormat.Text = "" Then
			Exit Sub
		End If

		Select Case cboFormat.Text
			Case "Sales Order Register - Detailed"
				dpTransDate2.Visible = True
				lblDate2.Visible = True
				lblFilter1.Visible = True
				cboFilter1.Visible = True
				popFilter1()

			Case "Sales Order Register - Summary"
				dpTransDate2.Visible = True
				lblDate2.Visible = True
				lblFilter1.Visible = True
				cboFilter1.Visible = True
				popFilter1()

			Case "Sales Invoice Register - Summary", "Sales Summary per Material"
				dpTransDate2.Visible = True
				lblDate2.Visible = True
				lblFilter1.Visible = True
				cboFilter1.Visible = True
				popFilter1()

			Case "Sales Invoice Register - Detailed", "Sales Invoice Register - Detailed per Invoice", "Free(Deal) - Detailed per Invoice"
				dpTransDate2.Visible = True
				lblDate2.Visible = True
				lblFilter1.Visible = True
				cboFilter1.Visible = True
				popFilter1()

			Case "Sales Order Materials Summary"
				dpTransDate2.Visible = True
				lblDate2.Visible = True
				lblFilter1.Visible = False
				cboFilter1.Visible = False
				lblFilter1.Text = ""
				cboFilter1.Text = ""
				popFilter1()

			Case "Sales Summary with ASP", "Sales Summary with ASP with VAT"
				dpTransDate2.Visible = True
				lblDate2.Visible = True
				lblFilter1.Visible = True
				cboFilter1.Visible = True
				popFilter1()

			Case "Sales Volume Customer"
				dpTransDate2.Visible = True
				lblDate2.Visible = True
				lblFilter1.Visible = True
				cboFilter1.Visible = True
				cboFilter1.Items.Clear()
				cboFilter1.Items.Add("ALL")
				cboFilter1.Items.Add("AREA")
				'cboFilter1.Items.Add("Salesman")

			Case "Sales Total per TSR"
				dpTransDate2.Visible = True
				lblDate2.Visible = True
				lblFilter1.Visible = True
				cboFilter1.Visible = True
				cboFilter1.Items.Clear()
				cboFilter1.Items.Add("ALL")

				dt = GetDataTable("select distinct concat(a.smnno,space(1),b.fullname) from saleshdrtbl a left join smnmtbl b on a.smnno=b.smnno where " &
								  "a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
								  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' group by a.smnno order by b.fullname")
				If Not CBool(dt.Rows.Count) Then
					Exit Sub
				Else
					For Each dr As DataRow In dt.Rows
						cboFilter1.Items.Add(dr.Item(0).ToString())

					Next
				End If

				Call dt.Dispose()

			Case "Sales Total Per Category"
				dpTransDate2.Visible = True
				lblDate2.Visible = True
				lblFilter1.Visible = True
				cboFilter1.Visible = True
				cboFilter1.Items.Clear()
				cboFilter1.Items.Add("ALL")

			Case "Sales Total Per Product per Category"
				dpTransDate2.Visible = True
				lblDate2.Visible = True
				lblFilter1.Visible = True
				cboFilter1.Visible = True
				cboFilter1.Items.Clear()
				cboFilter1.Items.Add("ALL")

				dt = GetDataTable("select distinct c.mmgrp from salesdettbl a left join saleshdrtbl b on a.invno=b.invno " &
								  "left join mmasttbl c on a.codeno=c.codeno where b.transdate between " &
								  "'" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
								  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and " &
								  "b.status <> 'void' order by c.mmgrp")
				If Not CBool(dt.Rows.Count) Then
					Exit Sub
				Else
					For Each dr As DataRow In dt.Rows
						cboFilter1.Items.Add(dr.Item(0).ToString())

					Next
				End If

				Call dt.Dispose()


			Case "Sales per Material"
				dpTransDate2.Visible = True
				lblDate2.Visible = True
				lblFilter1.Visible = True
				cboFilter1.Visible = True
				cboFilter1.Items.Clear()
				'cboFilter1.Items.Add("All Customers")

				'If cboFilter1.Items.Count > 0 Then
				'    cboFilter1.SelectedIndex = 0
				'End If

				dt = GetDataTable("select distinct ifnull(b.codename,b.mmdesc) from salesdettbl c left join saleshdrtbl a on a.invno=c.invno " &
								  "left join mmasttbl b on c.codeno=b.codeno where a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
								  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and a.status <> 'void' order by ifnull(b.codename,b.mmdesc)")
				If Not CBool(dt.Rows.Count) Then
					Exit Sub
				Else
					For Each dr As DataRow In dt.Rows
						cboFilter1.Items.Add(dr.Item(0).ToString())

					Next
				End If

				Call dt.Dispose()

			Case "Monthly Reports"
				dpTransDate2.Visible = True
				lblDate2.Visible = True
				lblFilter1.Visible = True
				cboFilter1.Visible = True
				cboFilter1.Text = ""
				popFilter1MRrep()

		End Select

	End Sub

	Private Sub popFilter1MRrep()
		cboFilter1.Items.Clear()
		cboFilter1.Items.Add("")
		cboFilter1.Items.Add("Top 20 Sales-Product")
		cboFilter1.Items.Add("Top 20 Sales-Product Group")
		cboFilter1.Items.Add("Top 20 Sales-Client")
		cboFilter1.Items.Add("Top 20 Sales-Product per TSR")
		cboFilter1.Items.Add("Top 20 Sales-Client per TSR")
		cboFilter1.Items.Add("Top Client With Products")
		cboFilter1.Items.Add("Sales Vs COS per SKU")
		cboFilter1.Items.Add("VAT Register")

	End Sub

	Private Sub popFilter1()
		cboFilter1.Items.Clear()
		cboFilter1.Items.Add("")
		cboFilter1.Items.Add("ALL")
		cboFilter1.Items.Add("AREA")
		cboFilter1.Items.Add("Salesman")
		cboFilter1.Items.Add("Customer")

	End Sub

	Private Sub cboFilter1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboFilter1.SelectedIndexChanged
		dgvRegSum.DataSource = Nothing
		dgvRegSum.DataBind()
		cboFilter2.Items.Clear()

		If dpTransDate.Text = Nothing Then
			Exit Sub
		ElseIf dpTransDate2.Text = Nothing Then
			Exit Sub
		ElseIf CDate(dpTransDate.Text) > CDate(dpTransDate2.Text) Then
			Exit Sub
		ElseIf cboFormat.Text = "" Then
			Exit Sub
		ElseIf cboFilter1.Text = "" Then
			Exit Sub
		End If

		Select Case cboFilter1.Text
			Case "ALL"
				cboFilter2.Items.Clear()
				cboFilter3.Items.Clear()
				txtText1.Text = ""
				txtText2.Text = ""
				cboFilter2.Visible = False
				cboFilter3.Visible = False
				lblFilter2.Visible = False
				lblFilter3.Visible = False
				txtText1.Visible = False
				txtText2.Visible = False

			Case "AREA"
				Call Vis1()

				cboFilter2.Items.Clear()
				cboFilter2.Items.Add("ALL")

				Select Case cboFormat.Text
					Case "Sales Order Register - Detailed", "Sales Order Register - Summary", "Sales Order Materials Summary"
						dt = GetDataTable("select distinct concat(b.areano,space(1),c.areaname) from sohdrtbl a " &
										  "left join custmasttbl b on a.custno=b.custno left join areatbl c on b.areano=c.areano " &
										  "where a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
										  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' group by b.areano order by c.areaname")
						If Not CBool(dt.Rows.Count) Then Exit Sub

					Case "Sales Invoice Register - Summary", "Sales Invoice Register - Detailed", "Sales Summary with ASP",
						 "Sales Invoice Register - Detailed per Invoice", "Sales Summary per Material", "Sales Summary with ASP with VAT"
						dt = GetDataTable("select distinct concat(b.areano,space(1),c.areaname) from saleshdrtbl a " &
										  "left join custmasttbl b on a.custno=b.custno left join areatbl c on b.areano=c.areano " &
										  "where a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
										  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' group by b.areano order by c.areaname")
						If Not CBool(dt.Rows.Count) Then Exit Sub

					Case "Free(Deal) - Detailed per Invoice"
						dt = GetDataTable("select distinct concat(b.areano,space(1),c.areaname) from saleshdrtbl a " &
										  "left join custmasttbl b on a.custno=b.custno left join areatbl c on b.areano=c.areano " &
										  "left join salesdettbl d on a.invno=d.invno where a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
										  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and d.sp=0 group by b.areano order by c.areaname")
						If Not CBool(dt.Rows.Count) Then Exit Sub
				End Select


				For Each dr As DataRow In dt.Rows
					cboFilter2.Items.Add(dr.Item(0).ToString())

				Next

				Call dt.Dispose()


			Case "Salesman"
				Call Vis1()

				cboFilter2.Items.Clear()

				Select Case cboFormat.Text
					Case "Sales Order Register - Detailed", "Sales Order Register - Summary", "Sales Order Materials Summary"
						dt = GetDataTable("select distinct concat(a.smnno,space(1),b.fullname) from sohdrtbl a left join smnmtbl b on a.smnno=b.smnno  " &
										  "where a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
										  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' order by b.fullname")

					Case "Sales Invoice Register - Summary", "Sales Invoice Register - Detailed", "Sales Summary with ASP", "Sales Invoice Register - Detailed per Invoice",
						 "Sales Summary per Material", "Sales Summary with ASP with VAT"
						dt = GetDataTable("select distinct concat(a.smnno,space(1),b.fullname) from saleshdrtbl a left join smnmtbl b on a.smnno=b.smnno  " &
										  "where a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
										  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' order by b.fullname")

					Case "Free(Deal) - Detailed per Invoice"
						dt = GetDataTable("select distinct concat(a.smnno,space(1),b.fullname) from saleshdrtbl a left join smnmtbl b on a.smnno=b.smnno " &
										  "left join salesdettbl d on a.invno=d.invno where a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
										  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and d.sp=0 order by c.areaname")

				End Select

				If Not CBool(dt.Rows.Count) Then
					Exit Sub
				Else
					cboFilter2.Items.Add("")
					cboFilter2.Items.Add("ALL")
					For Each dr As DataRow In dt.Rows
						cboFilter2.Items.Add(dr.Item(0).ToString())

					Next

				End If

				Call dt.Dispose()


			Case "Customer"
				Call Vis1()

				cboFilter2.Items.Clear()
				cboFilter2.Items.Add("ALL")

				Select Case cboFormat.Text
					Case "Sales Order Register - Detailed", "Sales Order Register - Summary", "Sales Order Materials Summary"
						dt = GetDataTable("select distinct concat(a.custno,space(1),b.bussname) from sohdrtbl a " &
										  "left join custmasttbl b on a.custno=b.custno where " &
										  "a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
										  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' order by b.bussname")

					Case "Sales Invoice Register - Summary", "Sales Invoice Register - Detailed", "Sales Summary with ASP", "Sales Invoice Register - Detailed per Invoice",
						 "Sales Summary per Material", "Sales Summary with ASP with VAT"
						dt = GetDataTable("select distinct concat(a.custno,space(1),b.bussname) from saleshdrtbl a " &
										  "left join custmasttbl b on a.custno=b.custno where " &
										  "a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
										  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' order by b.bussname")

					Case "Free(Deal) - Detailed per Invoice"
						dt = GetDataTable("select distinct concat(a.custno,space(1),b.bussname) from saleshdrtbl a " &
										  "left join custmasttbl b on a.custno=b.custno  " &
										  "left join salesdettbl d on a.invno=d.invno where a.transdate between " &
										  "'" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
										  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and d.sp=0 order by b.bussname")
				End Select

				If Not CBool(dt.Rows.Count) Then
					Exit Sub
				Else
					For Each dr As DataRow In dt.Rows
						cboFilter2.Items.Add(dr.Item(0).ToString())

					Next
				End If

				Call dt.Dispose()

			Case "Top 20 Sales-Product"
				Vis1()
				popFilter2ProdCat()

			Case "Top 20 Sales-Product Group"
				Vis1()
				cboFilter2.Items.Clear()
				cboFilter2.Items.Add("ALL")
				cboFilter2.Items.Add("Per Salesman")
				'cboFilter2.Visible = False
				'lblFiler2.Visible = False

			Case "Top 20 Sales-Client"
				cboFilter2.Visible = False
				lblFilter2.Visible = False

			Case "Top 20 Sales-Product per TSR"
				cboFilter2.Visible = False
				lblFilter2.Visible = False

			Case "Top 20 Sales-Client per TSR"
				cboFilter2.Visible = False
				lblFilter2.Visible = False

			Case "Top Client With Products"
				cboFilter2.Visible = False
				lblFilter2.Visible = False

			Case "Sales Vs COS per SKU"
				cboFilter2.Visible = True
				lblFilter2.Visible = True

				cboFilter2.Items.Add("ALL")
				dt = GetDataTable("select distinct concat(a.custno,space(1),b.bussname) from saleshdrtbl a left join custmasttbl b on a.custno=b.custno  " &
								  "where a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
								  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' order by b.bussname")
				If Not CBool(dt.Rows.Count) Then
					Exit Sub
				Else
					For Each dr As DataRow In dt.Rows
						cboFilter2.Items.Add(dr.Item(0).ToString())

					Next
				End If

				Call dt.Dispose()

			Case Else
				Select Case cboFormat.Text
					Case "Sales per Material"
						dt = GetDataTable("select codeno from mmasttbl where ifnull(codename,mmdesc) = '" & cboFilter1.Text & "'")
						If Not CBool(dt.Rows.Count) Then
							Exit Sub
						Else
							For Each dr As DataRow In dt.Rows
								txtText1.Text = dr.Item(0).ToString() & ""

							Next

						End If

						Call dt.Dispose()

				End Select

		End Select

		btnGenerate.Enabled = True

	End Sub

	Private Sub Vis1()
		cboFilter2.Visible = True
		lblFilter2.Visible = True
		'txtText1.Visible = True

	End Sub

	Private Sub Vis2()
		cboFilter3.Visible = True
		lblFilter3.Visible = True
		txtText2.Visible = True

	End Sub

	Private Sub popFilter2ProdCat()
		cboFilter2.Items.Clear()


		dt = GetDataTable("select distinct c.mmgrp from salesdettbl a left join saleshdrtbl b on a.invno=b.invno " &
						  "left join mmasttbl c on a.codeno=c.codeno where b.transdate between " &
						  "'" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
						  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "'")
		If Not CBool(dt.Rows.Count) Then
			Exit Sub
		Else
			cboFilter2.Items.Add("")
			cboFilter2.Items.Add("ALL")
			For Each dr As DataRow In dt.Rows
				cboFilter2.Items.Add(dr.Item(0).ToString())

			Next
		End If

		Call dt.Dispose()

	End Sub

	Private Sub cboFilter2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboFilter2.SelectedIndexChanged
		dgvRegSum.DataSource = Nothing
		dgvRegSum.DataBind()

		If dpTransDate.Text = Nothing Then
			Exit Sub
		ElseIf dpTransDate2.Text = Nothing Then
			Exit Sub
		ElseIf CDate(dpTransDate.Text) > CDate(dpTransDate2.Text) Then
			Exit Sub
		ElseIf cboFormat.Text = "" Then
			Exit Sub
		ElseIf cboFilter1.Text = "" Then
			Exit Sub
		ElseIf cboFilter2.Text = "" Then
			Exit Sub
		End If


		Select Case cboFilter2.Text
			Case "ALL"
				txtText1.Visible = False
				cboFilter3.Visible = False

			Case "Per Salesman"
				Select Case cboFilter1.Text
					Case "Top 20 Sales-Product Group"
						Vis2()
						cboFilter3.Items.Clear()

						dt = GetDataTable("select distinct concat(a.smnno,space(1),b.fullname) from saleshdrtbl a left join smnmtbl b " &
										  "on a.smnno=b.smnno where a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
										  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and a.status <> 'void'")
						If Not CBool(dt.Rows.Count) Then
							Exit Sub

						Else
							cboFilter3.Items.Add("")
							cboFilter3.Items.Add("ALL")
							For Each dr As DataRow In dt.Rows
								cboFilter3.Items.Add(dr.Item(0).ToString())

							Next
						End If

						Call dt.Dispose()

				End Select

		End Select
	End Sub

	Private Sub btnGenerate_Click(sender As Object, e As EventArgs) Handles btnGenerate.Click
		If dpTransDate.Text = Nothing Then
			Exit Sub
		ElseIf dpTransDate2.Text = Nothing Then
			Exit Sub
		ElseIf CDate(dpTransDate.Text) > CDate(dpTransDate2.Text) Then
			Exit Sub
		ElseIf cboFormat.Text = "" Then
			Exit Sub
		ElseIf cboFilter1.Text = "" Then
			Exit Sub
		End If

		lblMsg.Text = ""

		Select Case cboFormat.Text
			Case "Sales Invoice Register - Summary"
				Select Case cboFilter1.Text
					Case "ALL"
						SDsumAll()

					Case "Customer"
						Select Case cboFilter2.Text
							Case "ALL"
								SDsumCustGrp()


							Case Else
								SDsumAll()

						End Select

					Case "AREA"
						Select Case cboFilter2.Text
							Case "ALL"
								SDsumAreaGrp()

							Case Else
								SDsumAll()

						End Select

					Case "Salesman"
						Select Case cboFilter2.Text
							Case "ALL"
								SDsumSmnGrp()

							Case Else
								SDsumAll()


						End Select

				End Select

			Case "Sales Invoice Register - Detailed", "Free(Deal) - Detailed per Invoice"
				Select Case cboFilter1.Text
					Case "ALL"
						SDsumAlldet()

					Case "Customer"
						Select Case cboFilter2.Text
							Case "ALL"
								SDregGrpdet()

							Case Else
								SDsumAlldet()

						End Select

					Case "AREA"
						Select Case cboFilter2.Text
							Case "ALL"
								SDregGrpdet()

							Case Else
								SDsumAlldet()

						End Select

					Case "Salesman"
						Select Case cboFilter2.Text
							Case "ALL"
								SDregGrpdet()

							Case Else
								SDsumAlldet()

						End Select

				End Select

			Case "Sales Summary per Material"
				Select Case cboFilter1.Text
					Case "ALL"
						SDsumAllMatl2()

					Case "Customer"
						Select Case cboFilter2.Text
							Case "ALL"
								lblMsg.Text = ("Select Customer")
								Exit Sub

							Case Else
								SDsumAllMatl2()

						End Select

					Case "AREA"
						Select Case cboFilter2.Text
							Case "ALL"
								lblMsg.Text = ("Select Area")
								Exit Sub

							Case Else
								SDsumAllMatl2()

						End Select

					Case "Salesman"
						Select Case cboFilter2.Text
							Case "ALL"
								lblMsg.Text = ("Select Salesman")
								Exit Sub

							Case Else
								SDsumAllMatl2()

						End Select

				End Select

			Case "Sales Invoice Register - Detailed per Invoice"
				Select Case cboFilter1.Text
					Case "ALL"
						SDsumAlldet2()

					Case "Customer"
						Select Case cboFilter2.Text
							Case "ALL"
								SDregGrpdet2()

							Case Else
								SDsumAlldet2()

						End Select

					Case "AREA"
						Select Case cboFilter2.Text
							Case "ALL"
								SDregGrpdet2()

							Case Else
								SDsumAlldet2()

						End Select

					Case "Salesman"
						Select Case cboFilter2.Text
							Case "ALL"
								SDregGrpdet2()

							Case Else
								SDsumAlldet2()

						End Select

				End Select

			Case "Sales Order Register - Summary"
				Select Case cboFilter1.Text
					Case "ALL"
						SD_SOsumAll()

					Case "Customer"
						Select Case cboFilter2.Text
							Case "ALL"
								SD_SOsumGrpCust()

							Case Else
								SD_SOsumAll()

						End Select

					Case "AREA"
						Select Case cboFilter2.Text
							Case "ALL"
								SD_SOsumGrpArea()

							Case Else
								SD_SOsumAll()

						End Select

					Case "Salesman"
						Select Case cboFilter2.Text
							Case "ALL"
								SD_SOsumGrpSmn()

							Case Else
								SD_SOsumAll()

						End Select

				End Select

			Case "Sales Order Register - Detailed"
				Select Case cboFilter1.Text
					Case "ALL"
						SD_SORegdet()

					Case "Customer"
						Select Case cboFilter2.Text
							Case "ALL"
								SD_SORegdetGrpCust()

							Case Else
								SD_SORegdet()

						End Select

					Case "AREA"
						Select Case cboFilter2.Text
							Case "ALL"
								SD_SORegdetGrpArea()

							Case Else
								SD_SORegdet()

						End Select

					Case "Salesman"
						Select Case cboFilter2.Text
							Case "ALL"
								SD_SORegdetGrpSmn()

							Case Else
								SD_SORegdet()

						End Select

				End Select

			Case "Sales Order Materials Summary"
				SD_SOmatlSum()

			Case "Sales Summary with ASP"
				procASPall()
				SalesASP()

			Case "Sales Summary with ASP with VAT"
				procASPallVAT()
				SalesASP()

			Case "Sales Total Per Category"
				sql = "delete from tempasp where user = '" & lblUser.Text & "'"
				ExecuteNonQuery(sql)

				sql = "insert into tempasp(mmgrp,codeno,mmdesc,qty,wt,amt,user) select distinct " &
					  "c.mmgrp as smnno,a.codeno,ifnull(c.codename,c.mmdesc) as mmdesc," &
					  "sum(ifnull(a.qty,0)) as qty,sum(ifnull(a.wt,0)) as wt,sum(ifnull(a.detgrossamt,0)) as amt," &
					  "'" & lblUser.Text & "' from salesdettbl a left join saleshdrtbl b on a.invno=b.invno " &
					  "left join mmasttbl c on a.codeno=c.codeno where b.transdate between " &
					  "'" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
					  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and " &
					  "b.status <> 'void' order by amt desc"
				ExecuteNonQuery(sql)

				sql = "update tempasp a left join mmasttbl b on a.codeno=b.codeno " &
					  "set a.mmgrp = 'Trading Goods' where b.mmtype <> 'Finished Goods' " &
					  "and a.user = '" & lblUser.Text & "'"
				ExecuteNonQuery(sql)

				SalesCateg()

			Case "Sales Total Per Product per Category"
				sql = "delete from tempasp where user = '" & lblUser.Text & "'"
				ExecuteNonQuery(sql)

				Select Case cboFilter1.Text
					Case "ALL"
						sql = "insert into tempasp(mmgrp,codeno,mmdesc,qty,wt,amt,user) select " &
							  "c.mmgrp as smnno,a.codeno,ifnull(c.codename,c.mmdesc) as mmdesc," &
							  "sum(ifnull(a.qty,0)) as qty,sum(ifnull(a.wt,0)) as wt,sum(ifnull(a.detgrossamt,0)) as amt," &
							  "'" & lblUser.Text & "' from salesdettbl a left join saleshdrtbl b on a.invno=b.invno " &
							  "left join mmasttbl c on a.codeno=c.codeno where b.transdate between " &
							  "'" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
							  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and " &
							  "b.status <> 'void' group by a.codeno order by amt desc"
						ExecuteNonQuery(sql)

					Case Else
						sql = "insert into tempasp(mmgrp,codeno,mmdesc,qty,wt,amt,user) select " &
							  "c.mmgrp as smnno,a.codeno,ifnull(c.codename,c.mmdesc) as mmdesc," &
							  "sum(ifnull(a.qty,0)) as qty,sum(ifnull(a.wt,0)) as wt,sum(ifnull(a.detgrossamt,0)) as amt," &
							  "'" & lblUser.Text & "' from salesdettbl a left join saleshdrtbl b on a.invno=b.invno " &
							  "left join mmasttbl c on a.codeno=c.codeno where b.transdate between " &
							  "'" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
							  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and " &
							  "b.status <> 'void' and c.mmgrp = '" & cboFilter1.Text & "' group by a.codeno " &
							  "order by amt desc"
						ExecuteNonQuery(sql)

				End Select

				sql = "update tempasp a left join mmasttbl b on a.codeno=b.codeno " &
					  "set a.mmgrp = 'Trading Goods' where b.mmtype <> 'Finished Goods' " &
					  "and a.user = '" & lblUser.Text & "'"
				ExecuteNonQuery(sql)

				SalesCategDet()

			Case "Sales Volume Customer"
				SalesVolCust()

			Case "Sales Total per TSR"
				'createSmnSum()
				SalesTotSmn()

			Case "Sales per Material"
				If cboFilter1.Text = "" Then
					lblMsg.Text = ("Select Material")
					Exit Sub
				End If


				'lblMsg.Text = ("Not Yet Available / " & txtText1.Text)
				'Exit Sub

				SalesMatlSmn()

			Case "Monthly Reports"
				Select Case cboFilter1.Text
					Case "Top 20 Sales-Product" 'with ASP
						createTempSalesSum()
						Top20ProdSales()

					Case "Top 20 Sales-Product Group"
						ProdGrpMoreOptn()

					Case "Top 20 Sales-Client"
						Top20ClientSales()

					Case "Top 20 Sales-Product per TSR"
						'add temp table
						createTempDataTSR()
						TopProdTSR()

					Case "Top 20 Sales-Client per TSR"
						TopClienTSR()

					Case "Top Client With Products"
						popTopCust()
						TopClienProd()

					Case "VAT Register"
						VATregister()

					Case "Sales Vs COS per SKU"
						'check if cos is available
						dt = GetDataTable("select * from coshdrtbl where transdate = '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "'")
						If Not CBool(dt.Rows.Count) Then
							lblMsg.Text = ("Cost of Sales Not Yet Processed for the Month of " & Format(CDate(dpTransDate2.Text), "MMM yyyy"))
							Exit Sub

						End If

						UpdateCOSsku()
						SalesASPcos()

				End Select


		End Select
	End Sub

	Private Sub SalesASPcos()
		lblMsg.Text = "Not Yet Available"

	End Sub

	Private Sub VATregister()
		lblMsg.Text = "Not Yet Available"

	End Sub

	Private Sub TopClienProd()
		lblMsg.Text = "Not Yet Available"

	End Sub

	Private Sub TopClienTSR()
		lblMsg.Text = "Not Yet Available"

	End Sub

	Private Sub TopProdTSR()
		lblMsg.Text = "Not Yet Available"

	End Sub

	Private Sub Top20ClientSales()
		lblMsg.Text = "Not Yet Available"

	End Sub

	Private Sub Top20ProdSales()
		lblMsg.Text = "Not Yet Available"

	End Sub

	Private Sub SalesMatlSmn()
		lblMsg.Text = "Not Yet Available"

	End Sub

	Private Sub SalesTotSmn()
		lblMsg.Text = "Not Yet Available"

	End Sub

	Private Sub SalesVolCust()
		lblMsg.Text = "Not Yet Available"

	End Sub

	Private Sub SalesCategDet()
		lblMsg.Text = "Not Yet Available"
	End Sub

	Private Sub SalesCateg()
		lblMsg.Text = "Not Yet Available"

	End Sub

	Private Sub SalesASP()
		lblMsg.Text = "Not Yet Available"
	End Sub

	Private Sub SD_SOmatlSum()
		lblMsg.Text = "Not Yet Available"
	End Sub

	Private Sub SD_SORegdetGrpSmn()
		lblMsg.Text = "Not Yet Available"

	End Sub

	Private Sub SD_SORegdetGrpArea()
		lblMsg.Text = "Not Yet Available"
	End Sub

	Private Sub SD_SORegdetGrpCust()
		lblMsg.Text = "Not Yet Available"
	End Sub

	Private Sub SD_SORegdet()
		lblMsg.Text = "Not Yet Available"
	End Sub

	Private Sub SD_SOsumGrpSmn()
		lblMsg.Text = "Not Yet Available"

	End Sub

	Private Sub SD_SOsumGrpArea()
		lblMsg.Text = "Not Yet Available"

	End Sub

	Private Sub SD_SOsumGrpCust()
		lblMsg.Text = "Not Yet Available"

	End Sub

	Private Sub SD_SOsumAll()
		lblMsg.Text = "Not Yet Available"
	End Sub

	Private Sub SDregGrpdet2()
		lblMsg.Text = "Not Yet Available"

	End Sub

	Private Sub SDsumAlldet2()
		lblMsg.Text = "Not Yet Available"
	End Sub

	Private Sub SDsumAllMatl2()
		lblMsg.Text = "Not Yet Available"
	End Sub

	Private Sub SDregGrpdet()
		lblMsg.Text = "Not Yet Available"
	End Sub

	Private Sub SDsumAlldet()
		lblMsg.Text = "Not Yet Available"
	End Sub

	Private Sub SDsumSmnGrp()
		PregSum.Visible = False
		PregSumCust.Visible = False
		PregSumSmn.Visible = True


		dt = GetDataTable("select * from saleshdrtbl where transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
						  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and status <> 'void'")
		If Not CBool(dt.Rows.Count) Then
			dgvRegSumSmnGrp.DataSource = Nothing
			dgvRegSumSmnGrp.DataBind()
			lblMsg.Text = "No Record Found for " & cboFormat.Text & " for ALL " & cboFilter1.Text & "s, from Dates selected"
			Exit Sub

		End If

		Call dt.Dispose()

		Dim adapter As New MySqlDataAdapter
		Dim ds As New DataSet()

		sqldata = Nothing

		sqldata = "select distinct a.smnno,c.fullname,a.invno,a.tc,a.transdate,a.dono,a.docno," &
				  "concat(a.custno,space(1),b.bussname) as custno,a.grossamt,a.vat,a.fhamt,a.discamt,a.netamt " &
				  "from saleshdrtbl a left join custmasttbl b on a.custno=b.custno left join smnmtbl c on a.smnno=c.smnno " &
				  "where a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
				  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and a.status <> 'void' order by a.smnno,b.bussname,a.docno"

		With dgvRegSumSmnGrp
			.Columns(0).HeaderText = "Smn No."
			.Columns(1).HeaderText = "Salesman"
			.Columns(2).HeaderText = "Inv No."
			.Columns(3).HeaderText = "TC."
			.Columns(4).HeaderText = "Date"
			.Columns(5).HeaderText = "DO No."
			.Columns(6).HeaderText = "Ref No."
			.Columns(7).HeaderText = "Customer"
			.Columns(8).HeaderText = "Gross Amt"
			.Columns(9).HeaderText = "VAT"
			.Columns(10).HeaderText = "F & H"
			.Columns(11).HeaderText = "Disc Amt"
			.Columns(12).HeaderText = "Net Amt"

		End With

		conn.Open()
		Dim command As New MySqlCommand(sqldata, conn)
		adapter.SelectCommand = command
		adapter.Fill(ds)
		adapter.Dispose()
		command.Dispose()
		conn.Close()
		dgvRegSumSmnGrp.DataSource = ds.Tables(0)
		dgvRegSumSmnGrp.DataBind()

	End Sub

	Private Sub SDsumAreaGrp()
		lblMsg.Text = "Not Yet Available"
	End Sub

	Private Sub SDsumCustGrp()
		PregSum.Visible = False
		PregSumSmn.Visible = False
		PregSumCust.Visible = True

		dt = GetDataTable("select * from saleshdrtbl where transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
						  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and status <> 'void'")
		If Not CBool(dt.Rows.Count) Then
			dgvRegSumCustGrp.DataSource = Nothing
			dgvRegSumCustGrp.DataBind()
			lblMsg.Text = "No Record Found for " & cboFormat.Text & " for ALL " & cboFilter1.Text & "s, from Dates selected"
			Exit Sub

		End If

		Call dt.Dispose()

		Dim adapter As New MySqlDataAdapter
		Dim ds As New DataSet()

		sqldata = Nothing

		sqldata = "select distinct a.custno,b.bussname,a.invno,a.tc,a.transdate,a.dono,a.docno,a.smnno," &
				  "concat(a.shipto,space(1),c.bussname) as shiptoname,a.grossamt,a.vat,a.fhamt,a.discamt,a.netamt " &
				  "from saleshdrtbl a left join custmasttbl b on a.custno=b.custno left join custmasttbl c on a.shipto=c.custno " &
				  "where a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
				  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and a.status <> 'void' order by b.bussname,a.docno"

		With dgvRegSumCustGrp
			.Columns(0).HeaderText = "Cust No."
			.Columns(1).HeaderText = "Sold To"
			.Columns(2).HeaderText = "Inv No."
			.Columns(3).HeaderText = "TC."
			.Columns(4).HeaderText = "Date"
			.Columns(5).HeaderText = "DO No."
			.Columns(6).HeaderText = "Ref No."
			.Columns(7).HeaderText = "Smn No"
			.Columns(8).HeaderText = "Ship To"
			.Columns(9).HeaderText = "Gross Amt"
			.Columns(10).HeaderText = "VAT"
			.Columns(11).HeaderText = "F & H"
			.Columns(12).HeaderText = "Disc Amt"
			.Columns(13).HeaderText = "Net Amt"

		End With

		conn.Open()
		Dim command As New MySqlCommand(sqldata, conn)
		adapter.SelectCommand = command
		adapter.Fill(ds)
		adapter.Dispose()
		command.Dispose()
		conn.Close()
		dgvRegSumCustGrp.DataSource = ds.Tables(0)
		dgvRegSumCustGrp.DataBind()




	End Sub


	Private Sub SDsumAll()
		PregSumCust.Visible = False
		PregSum.Visible = True
		Select Case cboFormat.Text
			Case "Sales Invoice Register - Summary"
				Select Case cboFilter1.Text
					Case "ALL"
						dt = GetDataTable("select * from saleshdrtbl where transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
										  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and status <> 'void'")
					Case "Customer"
						dt = GetDataTable("select * from saleshdrtbl where transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
										  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and status <> 'void' and " &
										  "custno = '" & cboFilter2.Text.Substring(0, 5) & "'")
					Case "AREA"
						dt = GetDataTable("select * from saleshdrtbl a left join custmasttbl b on a.custno=b.custno where " &
										  "a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
										  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and a.status <> 'void' and " &
										  "b.areano = '" & cboFilter2.Text.Substring(0, 3) & "'")
					Case "Salesman"
						dt = GetDataTable("select * from saleshdrtbl where transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
										  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and status <> 'void' and " &
										  "smnno = '" & cboFilter2.Text.Substring(0, 3) & "'")
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

		Select Case cboFormat.Text
			Case "Sales Invoice Register - Summary"
				Select Case cboFilter1.Text
					Case "ALL"
						sqldata = "select distinct a.invno,a.tc,a.transdate,a.dono,a.docno,a.smnno,concat(a.custno,space(1),b.bussname) as custno," &
								  "concat(a.shipto,space(1),c.bussname) as shiptoname,a.grossamt,a.vat,a.fhamt,a.discamt,a.netamt " &
								  "from saleshdrtbl a left join custmasttbl b on a.custno=b.custno left join custmasttbl c on a.shipto=c.custno " &
								  "where a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
								  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and a.status <> 'void'"
					Case "Customer"
						sqldata = "select distinct  a.invno,a.tc,a.transdate,a.dono,a.docno,a.smnno,concat(a.custno,space(1),b.bussname) as custno," &
								  "concat(a.shipto,space(1),c.bussname) as shiptoname,a.grossamt,a.vat,a.fhamt,a.discamt,a.netamt " &
								  "from saleshdrtbl a left join custmasttbl b on a.custno=b.custno left join custmasttbl c on a.shipto=c.custno " &
								  "where a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
								  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and a.status <> 'void' " &
								  "and a.custno = '" & cboFilter2.Text.Substring(0, 5) & "'"
					Case "AREA"
						sqldata = "select distinct  a.invno,a.tc,a.transdate,a.dono,a.docno,a.smnno,concat(a.custno,space(1),b.bussname) as custno," &
								  "concat(a.shipto,space(1),c.bussname) as shiptoname,a.grossamt,a.vat,a.fhamt,a.discamt,a.netamt " &
								  "from saleshdrtbl a left join custmasttbl b on a.custno=b.custno left join custmasttbl c on a.shipto=c.custno " &
								  "where a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
								  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and a.status <> 'void' " &
								  "and c.areano = '" & cboFilter2.Text.Substring(0, 3) & "'"
					Case "Salesman"
						sqldata = "select distinct a.invno,a.tc,a.transdate,a.dono,a.docno,a.smnno,concat(a.custno,space(1),b.bussname) as custno," &
								  "concat(a.shipto,space(1),c.bussname) as shiptoname,a.grossamt,a.vat,a.fhamt,a.discamt,a.netamt " &
								  "from saleshdrtbl a left join custmasttbl b on a.custno=b.custno left join custmasttbl c on a.shipto=c.custno " &
								  "where a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
								  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and a.status <> 'void' " &
								  "and a.smnno = '" & cboFilter2.Text.Substring(0, 3) & "'"
				End Select
		End Select

		With dgvRegSum
			.Columns(0).HeaderText = "Inv No."
			.Columns(1).HeaderText = "TC."
			.Columns(2).HeaderText = "Date"
			.Columns(3).HeaderText = "DO No."
			.Columns(4).HeaderText = "Ref No."
			.Columns(5).HeaderText = "Smn No"
			.Columns(6).HeaderText = "Sold To"
			.Columns(7).HeaderText = "Ship To"
			.Columns(8).HeaderText = "Gross Amt"
			.Columns(9).HeaderText = "VAT"
			.Columns(10).HeaderText = "F & H"
			.Columns(11).HeaderText = "Disc Amt"
			.Columns(12).HeaderText = "Net Amt"

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

	Private Sub createTempDataTSR()
		'Select Case concat(b.smnno, Space(1), d.fullname) As smnno,a.codeno As custno,c.mmdesc As bussname,sum(a.qty) As qty,sum(a.wt) As wt," & _
		'                        "ifnull(sum(a.itmamt),0) As amt from salesdettbl a left join saleshdrtbl b On a.invno=b.invno " & _
		'                        "left join mmasttbl c On c.codeno=a.codeno left join smnmtbl d On b.smnno=d.smnno where b.transdate " & _
		'                        "between '" & Format(CDate(frmSDreports.dpTransDate.text), "yyyy-MM-dd") & "' and " & _
		'                        "'" & Format(CDate(frmSDreports.dpTransDate2.text), "yyyy-MM-dd") & "' and b.status <> 'void' " & _
		'                        "group by a.codeno,b.smnno order by amt desc


	End Sub

	Private Sub ProdGrpMoreOptn()

		Select Case cboFilter2.Text
			Case "ALL"
				createTempSalesSum()
				DeliveryToPrint = "Top20ProdSalesOpn"

			Case "Per Salesman"
				sql = "delete from tempdodetprt where user = '" & lblUser.Text & "'"
				ExecuteNonQuery(sql)

				Select Case cboFilter3.Text
					Case "ALL"
						dt = GetDataTable("select smnno from saleshdrtbl where transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
										  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and status <> 'void' " &
										  "group by smnno")
						If Not CBool(dt.Rows.Count) Then
							Exit Sub

						Else
							For Each dr As DataRow In dt.Rows
								If dr.Item(0).ToString() <> "" Then
									sql = "insert into tempdodetprt(codeno,qty,wt,amount,user,um) select c.basecodeno,ifnull(sum(a.qty),0)," &
										  "ifnull(sum(a.wt),0),ifnull(sum(a.itmamt),0)-sum(a.detvat) as net,'" & lblUser.Text & "'," &
										  "'" & dr.Item(0).ToString() & "' from salesdettbl a left join saleshdrtbl b on a.invno=b.invno " &
										  "left join mmasttbl c on a.codeno=c.codeno where b.status <> 'Void' and " &
										  "b.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
										  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and  " &
										  "a.sp <> 0 and b.smnno = '" & dr.Item(0).ToString() & "' group by c.basecodeno order by net desc limit 20"
									ExecuteNonQuery(sql)
								End If
							Next
						End If

						Call dt.Dispose()

					Case Else
						sql = "insert into tempdodetprt(codeno,qty,wt,amount,user,um) select c.basecodeno,ifnull(sum(a.qty),0)," &
							  "ifnull(sum(a.wt),0),ifnull(sum(a.itmamt),0)-sum(a.detvat) as net,'" & lblUser.Text & "'," &
							  "'" & cboFilter3.Text.Substring(0, 3) & "' from salesdettbl a left join saleshdrtbl b on a.invno=b.invno " &
							  "left join mmasttbl c on a.codeno=c.codeno where b.status <> 'Void' and " &
							  "b.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
							  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and  " &
							  "a.sp <> 0 and b.smnno = '" & cboFilter3.Text.Substring(0, 3) & "' group by c.basecodeno order by net desc limit 20"
						ExecuteNonQuery(sql)

				End Select

				sql = "update tempdodetprt a left join mmasttbl b on a.codeno=b.basecodeno set a.mmdesc = b.basematl,a.lotno=b.billref " &
					  "where a.user = '" & lblUser.Text & "'"
				ExecuteNonQuery(sql)

				DeliveryToPrint = "Top20ProdSalesOpn"

			Case Else

		End Select



	End Sub

	Private Sub createSmnSum()
		sql = "delete from tempcustbal where user = '" & lblUser.Text & "'"
		ExecuteNonQuery(sql)

		Select Case cboFilter1.Text
			Case "ALL"
				sql = "insert into tempcustbal(smnno,custno,invamt,user) select smnno,custno,sum(netamt),'" & lblUser.Text & "' " &
					  "from saleshdrtbl where transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
					  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and status <> 'void' and user <> 'Admin' group by smnno"
				ExecuteNonQuery(sql)

				sql = "update tempcustbal,(select b.smnno,sum(a.qty) as qty,sum(a.wt) as wt from salesdettbl a " &
					  "left join saleshdrtbl b on a.invno=b.invno where b.transdate between " &
					  "'" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
					  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and " &
					  "b.status <> 'void' and b.user <> 'Admin' group by b.smnno,b.transdate) as smntot set " &
					  "tempcustbal.cmamt = smntot.qty,tempcustbal.dmamt = smntot.wt where tempcustbal.user = '" & lblUser.Text & "' " &
					  "and tempcustbal.smnno = smntot.smnno and tempcustbal.transdate = smntot.transdate"
				ExecuteNonQuery(sql)

			Case Else
				sql = "insert into tempcustbal(smnno,custno,invamt,user,transdate) select " &
					  "b.smnno,c.fullname,sum(b.netamt),'" & lblUser.Text & "',b.transdate from " &
					  "saleshdrtbl b left join smnmtbl c on b.smnno = c.smnno " &
					  "where b.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
					  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and " &
					  "b.status <> 'void' and b.smnno = '" & txtText1.Text & "' and b.user <> 'Admin' " &
					  "group by b.smnno,b.transdate"
				ExecuteNonQuery(sql)

				sql = "update tempcustbal,(select b.transdate,b.smnno,sum(a.qty) as qty,sum(a.wt) as wt from salesdettbl a " &
					  "left join saleshdrtbl b on a.invno=b.invno where b.transdate between " &
					  "'" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
					  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and " &
					  "b.status <> 'void' and b.user <> 'Admin' group by b.smnno,b.transdate) as smntot set " &
					  "tempcustbal.cmamt = smntot.qty,tempcustbal.dmamt = smntot.wt where tempcustbal.user = '" & lblUser.Text & "' " &
					  "and tempcustbal.smnno = smntot.smnno and tempcustbal.transdate = smntot.transdate"
				ExecuteNonQuery(sql)
		End Select

	End Sub

	Private Sub UpdateCOSsku()
		sql = "delete from tempasp where user = '" & lblUser.Text & "'"
		ExecuteNonQuery(sql)
		Select Case cboFilter2.Text
			Case "ALL"
				sql = "insert into tempasp(codeno,mmdesc,qty,wt,amt,billref,pc,pclass,user,qtpk) select a.codeno,b.mmdesc,sum(a.qty)," &
					  "sum(a.wt),sum(a.itmamt)-sum(a.detvat),b.billref,b.pc,d.pclass,'" & lblUser.Text & "',ifnull(b.qtpk,1) from salesdettbl a " &
					  "left join mmasttbl b on a.codeno=b.codeno left join saleshdrtbl c on a.invno=c.invno " &
					  "left join pctrtbl d on b.pc=d.pc where c.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
					  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and c.status <> 'void' " &
					  "and a.sp <> 0 group by a.codeno" ' and b.pc <> '10'
				ExecuteNonQuery(sql)
			Case Else
				sql = "insert into tempasp(codeno,mmdesc,qty,wt,amt,billref,pc,pclass,user,qtpk) select a.codeno,b.mmdesc,sum(a.qty)," &
					  "sum(a.wt),sum(a.itmamt)-sum(a.detvat),b.billref,b.pc,d.pclass,'" & lblUser.Text & "',ifnull(b.qtpk,1) from salesdettbl a " &
					  "left join mmasttbl b on a.codeno=b.codeno left join saleshdrtbl c on a.invno=c.invno " &
					  "left join pctrtbl d on b.pc=d.pc where c.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
					  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and c.status <> 'void' " &
					  "and a.sp <> 0 and c.custno = '" & cboFilter2.Text.Substring(0, 5) & "' group by a.codeno" ' and b.pc <> '10'
				ExecuteNonQuery(sql)

		End Select

		sql = "update tempasp set asp = round((amt/qty),2) where billref = 'qty' and user = '" & lblUser.Text & "'"
		ExecuteNonQuery(sql)

		sql = "update tempasp set asp = round((amt/wt),2) where billref = 'wt' and user = '" & lblUser.Text & "'"
		ExecuteNonQuery(sql)

		sql = "update tempasp a left join mmasttbl b on a.codeno=b.codeno set a.mmgrpdesc = b.mmgrp," &
			  "a.pc=b.pc where a.user = '" & lblUser.Text & "'"
		ExecuteNonQuery(sql)

		sql = "update tempasp a left join mmasttbl b on a.codeno=b.codeno set a.mmgrp=b.mmgrp where a.user = '" & lblUser.Text & "'"
		ExecuteNonQuery(sql)

		'verify costing
		'dt = GetDataTable("select * from cosinvtbl where transdate between '" & Format(CDate(dpTransDate.text), "yyyy-MM-dd") & "' " & _
		'                  "and '" & Format(CDate(dpTransDate2.text), "yyyy-MM-dd") & "'")
		'If Not CBool(dt.Rows.Count) Then
		'    lblMsg.Text = ("No Costing Yet")
		'    Exit Sub
		'End If

		sql = "update tempasp a,(select codeno,sum(itmamt) as amt,sum(qty) as qty,sum(wt) as wt from cosinvtbl where transdate between " &
			  "'" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' " &
			  "group by codeno) as b set a.amtcos = b.amt,a.qtycos = b.qty,a.wtcos = b.wt where a.codeno=b.codeno and a.user = '" & lblUser.Text & "'"
		ExecuteNonQuery(sql)

		sql = "update tempasp set uccos = round((amtcos/qtycos) * qtpk,2) where billref = 'qty' and user = '" & lblUser.Text & "'"
		ExecuteNonQuery(sql)

		sql = "update tempasp set uccos = round((amtcos/wtcos),2) where billref = 'wt' and user = '" & lblUser.Text & "'"
		ExecuteNonQuery(sql)

		sql = "update tempasp set avecos = uccos * qty where billref = 'qty' and user = '" & lblUser.Text & "'"
		ExecuteNonQuery(sql)

		sql = "update tempasp set avecos = uccos * wt where billref = 'wt' and user = '" & lblUser.Text & "'"
		ExecuteNonQuery(sql)

		sql = "update tempasp set varamt = round(ifnull(asp,0) - ifnull(uccos,0),2) where user = '" & lblUser.Text & "'"
		ExecuteNonQuery(sql)

	End Sub

	Private Sub procASPallVAT()
		sql = "delete from tempasp where user = '" & lblUser.Text & "'"
		ExecuteNonQuery(sql)

		Select Case cboFilter1.Text
			Case "ALL"
				sql = "insert into tempasp(codeno,mmdesc,qty,wt,amt,billref,pc,pclass,user) select a.codeno,b.mmdesc,sum(a.qty)," &
					  "sum(a.wt),sum(a.itmamt),b.billref,b.pc,d.pclass,'" & lblUser.Text & "' from salesdettbl a " &
					  "left join mmasttbl b on a.codeno=b.codeno left join saleshdrtbl c on a.invno=c.invno " &
					  "left join pctrtbl d on b.pc=d.pc where c.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
					  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and c.status <> 'void' " &
					  "and a.sp <> 0 and c.user <> 'Admin' group by a.codeno" ' and b.pc <> '10'
				ExecuteNonQuery(sql)

			Case "Customer"
				Select Case cboFilter2.Text
					Case "ALL"
						lblMsg.Text = ("Select " & cboFilter1.Text)
						Exit Sub

					Case Else
						sql = "insert into tempasp(codeno,mmdesc,qty,wt,amt,billref,pc,pclass,user) select a.codeno,b.mmdesc,sum(a.qty)," &
							  "sum(a.wt),sum(a.itmamt),b.billref,b.pc,d.pclass,'" & lblUser.Text & "' from salesdettbl a " &
							  "left join mmasttbl b on a.codeno=b.codeno left join saleshdrtbl c on a.invno=c.invno " &
							  "left join pctrtbl d on b.pc=d.pc where c.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
							  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and c.status <> 'void' and " &
							  "c.shipto = '" & txtText1.Text & "' and a.sp <> 0 and c.user <> 'Admin' group by a.codeno" ' and b.pc <> '10'
						ExecuteNonQuery(sql)

				End Select

			Case "Salesman"
				Select Case cboFilter2.Text
					Case "ALL"
						lblMsg.Text = ("Select " & cboFilter1.Text)
						Exit Sub

					Case Else
						sql = "insert into tempasp(codeno,mmdesc,qty,wt,amt,billref,pc,pclass,user) select a.codeno,b.mmdesc,sum(a.qty)," &
							  "sum(a.wt),sum(a.itmamt),b.billref,b.pc,d.pclass,'" & lblUser.Text & "' from salesdettbl a " &
							  "left join mmasttbl b on a.codeno=b.codeno left join saleshdrtbl c on a.invno=c.invno " &
							  "left join pctrtbl d on b.pc=d.pc where c.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
							  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and c.status <> 'void' and " &
							  "c.smnno = '" & txtText1.Text & "' and a.sp <> 0 and c.user <> 'Admin'  group by a.codeno" 'and b.pc <> '10'
						ExecuteNonQuery(sql)

				End Select

			Case "AREA"
				Select Case cboFilter2.Text
					Case "ALL"
						lblMsg.Text = ("Select " & cboFilter1.Text)
						Exit Sub

					Case Else
						sql = "insert into tempasp(codeno,mmdesc,qty,wt,amt,billref,pc,pclass,user) select a.codeno,b.mmdesc,sum(a.qty)," &
							  "sum(a.wt),sum(a.itmamt),b.billref,b.pc,d.pclass,'" & lblUser.Text & "' from salesdettbl a " &
							  "left join mmasttbl b on a.codeno=b.codeno left join saleshdrtbl c on a.invno=c.invno " &
							  "left join pctrtbl d on b.pc=d.pc left join custmasttbl e on c.shipto = e.custno " &
							  "where c.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
							  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and c.status <> 'void' and " &
							  "e.areano = '" & txtText1.Text & "' and a.sp <> 0 and c.user <> 'Admin' group by a.codeno" ' and b.pc <> '10'
						ExecuteNonQuery(sql)

				End Select

		End Select

		sql = "update tempasp set asp = round((amt/qty),2) where billref = 'qty' and user = '" & lblUser.Text & "'"
		ExecuteNonQuery(sql)

		sql = "update tempasp set asp = round((amt/wt),2) where billref = 'wt' and user = '" & lblUser.Text & "'"
		ExecuteNonQuery(sql)

		sql = "update tempasp a left join mmasttbl b on a.codeno=b.codeno set a.mmgrpdesc = b.mmgrp," &
			  "a.pc=b.pc where a.user = '" & lblUser.Text & "'"
		ExecuteNonQuery(sql)

		sql = "update tempasp a left join mmasttbl b on a.codeno=b.codeno set a.mmgrp=b.mmgrp where a.user = '" & lblUser.Text & "'"
		ExecuteNonQuery(sql)

	End Sub

	Private Sub procASPall()

		sql = "delete from tempasp where user = '" & lblUser.Text & "'"
		ExecuteNonQuery(sql)

		Select Case cboFilter1.Text
			Case "ALL"
				sql = "insert into tempasp(codeno,mmdesc,qty,wt,amt,billref,pc,pclass,user) select a.codeno,b.mmdesc,sum(a.qty)," &
					  "sum(a.wt),sum(a.itmamt)-sum(a.detvat),b.billref,b.pc,d.pclass,'" & lblUser.Text & "' from salesdettbl a " &
					  "left join mmasttbl b on a.codeno=b.codeno left join saleshdrtbl c on a.invno=c.invno " &
					  "left join pctrtbl d on b.pc=d.pc where c.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
					  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and c.status <> 'void' " &
					  "and a.sp <> 0 and c.user <> 'Admin' group by a.codeno" ' and b.pc <> '10'
				ExecuteNonQuery(sql)

			Case "Customer"
				Select Case cboFilter2.Text
					Case "ALL"
						lblMsg.Text = ("Select " & cboFilter1.Text)
						Exit Sub

					Case Else
						sql = "insert into tempasp(codeno,mmdesc,qty,wt,amt,billref,pc,pclass,user) select a.codeno,b.mmdesc,sum(a.qty)," &
							  "sum(a.wt),sum(a.itmamt)-sum(a.detvat),b.billref,b.pc,d.pclass,'" & lblUser.Text & "' from salesdettbl a " &
							  "left join mmasttbl b on a.codeno=b.codeno left join saleshdrtbl c on a.invno=c.invno " &
							  "left join pctrtbl d on b.pc=d.pc where c.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
							  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and c.status <> 'void' and " &
							  "c.shipto = '" & txtText1.Text & "' and a.sp <> 0 and c.user <> 'Admin' group by a.codeno" ' and b.pc <> '10'
						ExecuteNonQuery(sql)

				End Select

			Case "Salesman"
				Select Case cboFilter2.Text
					Case "ALL"
						lblMsg.Text = ("Select " & cboFilter1.Text)
						Exit Sub

					Case Else
						sql = "insert into tempasp(codeno,mmdesc,qty,wt,amt,billref,pc,pclass,user) select a.codeno,b.mmdesc,sum(a.qty)," &
							  "sum(a.wt),sum(a.itmamt)-sum(a.detvat),b.billref,b.pc,d.pclass,'" & lblUser.Text & "' from salesdettbl a " &
							  "left join mmasttbl b on a.codeno=b.codeno left join saleshdrtbl c on a.invno=c.invno " &
							  "left join pctrtbl d on b.pc=d.pc where c.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
							  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and c.status <> 'void' and " &
							  "c.smnno = '" & txtText1.Text & "' and a.sp <> 0 and c.user <> 'Admin'  group by a.codeno" 'and b.pc <> '10'
						ExecuteNonQuery(sql)

				End Select

			Case "AREA"
				Select Case cboFilter2.Text
					Case "ALL"
						lblMsg.Text = ("Select " & cboFilter1.Text)
						Exit Sub

					Case Else
						sql = "insert into tempasp(codeno,mmdesc,qty,wt,amt,billref,pc,pclass,user) select a.codeno,b.mmdesc,sum(a.qty)," &
							  "sum(a.wt),sum(a.itmamt)-sum(a.detvat),b.billref,b.pc,d.pclass,'" & lblUser.Text & "' from salesdettbl a " &
							  "left join mmasttbl b on a.codeno=b.codeno left join saleshdrtbl c on a.invno=c.invno " &
							  "left join pctrtbl d on b.pc=d.pc left join custmasttbl e on c.shipto = e.custno " &
							  "where c.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
							  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and c.status <> 'void' and " &
							  "e.areano = '" & txtText1.Text & "' and a.sp <> 0 and c.user <> 'Admin' group by a.codeno" ' and b.pc <> '10'
						ExecuteNonQuery(sql)

				End Select

		End Select

		sql = "update tempasp set asp = round((amt/qty),2) where billref = 'qty' and user = '" & lblUser.Text & "'"
		ExecuteNonQuery(sql)

		sql = "update tempasp set asp = round((amt/wt),2) where billref = 'wt' and user = '" & lblUser.Text & "'"
		ExecuteNonQuery(sql)

		sql = "update tempasp a left join mmasttbl b on a.codeno=b.codeno set a.mmgrpdesc = b.mmgrp," &
			  "a.pc=b.pc where a.user = '" & lblUser.Text & "'"
		ExecuteNonQuery(sql)

		sql = "update tempasp a left join mmasttbl b on a.codeno=b.codeno set a.mmgrp=b.mmgrp where a.user = '" & lblUser.Text & "'"
		ExecuteNonQuery(sql)

	End Sub

	Private Sub popTopCust()
		'dt = GetDataTable("select custno,sum(netamt) as amt from saleshdrtbl where " &
		'				  "transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and  " &
		'				  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' group by custno order by amt desc limit 20")
		'If Not CBool(dt.Rows.Count) Then Exit Sub

		'Call lvCust.Items.Clear()
		'For i As Integer = 0 To dt.Rows.Count - 1
		'	lvCust.Items.Add(dt.Rows(i).Item(0).ToString())
		'	lvCust.Items(i).SubItems.Add(dt.Rows(i).Item(1).ToString())
		'	lvCust.Items(i).SubItems.Add(i + 1.ToString())

		'Next

		'dt.Dispose()

		'sql = "delete from tempdodetprt where user = '" & lblUser.Text & "'"
		'ExecuteNonQuery(sql)

		'For i As Integer = 0 To lvCust.Items.Count - 1
		'	sql = "insert into tempdodetprt(codeno,wt,amount,user,lotno,qty) select c.codeno,ifnull(sum(a.wt),0),ifnull(sum(a.itmamt),0)-sum(a.detvat)," &
		'		  "'" & lbluser.text & "',b.custno," & lvCust.Items(i).SubItems(2).Text & " from salesdettbl a left join saleshdrtbl b on a.invno=b.invno " &
		'		  "where b.branch = '" & vLoggedBranch & "' and b.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
		'		  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and b.status <> 'Void' and b.custno = '" & lvCust.Items(i).SubItems(0).Text & "' group by a.codeno"
		'	ExecuteNonQuery(sql)

		'Next

	End Sub

	Private Sub createTempSalesSum()

		sql = "delete from tempdodetprt where user = '" & lblUser.Text & "'"
		ExecuteNonQuery(sql)

		Select Case cboFilter1.Text
			Case "Top 20 Sales-Product Group"
				sql = "insert into tempdodetprt(codeno,qty,wt,amount,user,um) select c.basecodeno,ifnull(sum(a.qty),0)," &
					  "ifnull(sum(a.wt),0),ifnull(sum(a.itmamt),0)-sum(a.detvat) as net,'" & lblUser.Text & "','ALL' " &
					  "from salesdettbl a left join saleshdrtbl b on a.invno=b.invno left join mmasttbl c on a.codeno=c.codeno where " &
					  "b.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
					  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and b.status <> 'Void' " &
					  "and a.sp <> 0 group by c.basecodeno order by net desc limit 20"
				ExecuteNonQuery(sql)

				sql = "update tempdodetprt a left join mmasttbl b on a.codeno=b.basecodeno set a.mmdesc = b.basematl,a.lotno=b.billref " &
					  "where a.user = '" & lblUser.Text & "'"
				ExecuteNonQuery(sql)

			Case "Top 20 Sales-Product"
				Select Case cboFilter2.Text
					Case "ALL"
						sql = "insert into tempdodetprt(codeno,qty,wt,amount,user,um) select a.codeno,ifnull(sum(a.qty),0)," &
							  "ifnull(sum(a.wt),0),ifnull(sum(a.itmamt),0)-sum(a.detvat) as net,'" & lblUser.Text & "','ALL' " &
							  "from salesdettbl a left join saleshdrtbl b on a.invno=b.invno where " &
							  "b.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
							  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and b.status <> 'Void' " &
							  "and a.sp <> 0 group by a.codeno order by net desc limit 20"
						ExecuteNonQuery(sql)
						'sum(a.itmamt)-sum(a.detvat)
					Case Else
						sql = "insert into tempdodetprt(codeno,qty,wt,amount,user,um) select a.codeno,ifnull(sum(a.qty),0)," &
							  "ifnull(sum(a.wt),0),ifnull(sum(a.itmamt),0)-sum(a.detvat) as net,'" & lblUser.Text & "',c.mmgrp " &
							  "from salesdettbl a left join saleshdrtbl b on a.invno=b.invno left join mmasttbl c on a.codeno=c.codeno " &
							  "where a.sp <> 0 And b.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
							  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and b.status <> 'Void' and " &
							  "c.mmgrp = '" & cboFilter2.Text & "' group by a.codeno order by net desc limit 20" '
						ExecuteNonQuery(sql)

				End Select

				sql = "update tempdodetprt a left join mmasttbl b on a.codeno=b.codeno set a.mmdesc = b.mmdesc,a.lotno=b.billref " &
					  "where a.user = '" & lblUser.Text & "'"
				ExecuteNonQuery(sql)

		End Select

		dt = GetDataTable("select * from tempdodetprt where user = '" & lblUser.Text & "'")
		If Not CBool(dt.Rows.Count) Then
			lblMsg.Text = ("No Record found")
			Exit Sub

		End If

		Call dt.Dispose()

		sql = "update tempdodetprt set sp = ifnull(amount,0) / ifnull(qty,0) where lotno = 'qty' and user = '" & lblUser.Text & "'"
		ExecuteNonQuery(sql)

		sql = "update tempdodetprt set sp = ifnull(amount,0) / ifnull(wt,0) where lotno = 'wt' and user = '" & lblUser.Text & "'"
		ExecuteNonQuery(sql)


	End Sub

	Protected Sub dgvRegSum_RowDataBound(sender As Object, e As GridViewRowEventArgs)

		If e.Row.RowType = DataControlRowType.DataRow Then
			Dim tgrossAmt As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "grossamt").ToString())
			grossAmt += tgrossAmt

			Dim tvatAmt As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "vat").ToString())
			vatAmt += tvatAmt

			Dim tfhAmt As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fhamt").ToString())
			fhAmt += tfhAmt

			Dim tdiscAmt As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "discamt").ToString())
			discAmt += tdiscAmt

			Dim tnetAmt As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "netamt").ToString())
			netAmt += tnetAmt

		End If

		If e.Row.RowType = DataControlRowType.Footer Then
			Dim lblGTotal As Label = DirectCast(e.Row.FindControl("lblGTotal"), Label)
			lblGTotal.Text = "GRAND TOTAL"

			Dim lblTotalGross As Label = DirectCast(e.Row.FindControl("lblGross"), Label)
			lblTotalGross.Text = Format(CDbl(grossAmt.ToString()), "#,##0.00 ; (#,##0.00)")
			grossAmt = 0

			Dim lblTotalVat As Label = DirectCast(e.Row.FindControl("lblVat"), Label)
			lblTotalVat.Text = Format(CDbl(vatAmt.ToString()), "#,##0.00 ; (#,##0.00)")
			vatAmt = 0

			Dim lblTotalFh As Label = DirectCast(e.Row.FindControl("lblFH"), Label)
			lblTotalFh.Text = Format(CDbl(fhAmt.ToString()), "#,##0.00 ; (#,##0.00)")
			fhAmt = 0

			Dim lblTotalDisc As Label = DirectCast(e.Row.FindControl("lblDisc"), Label)
			lblTotalDisc.Text = Format(CDbl(discAmt.ToString()), "#,##0.00 ; (#,##0.00)")
			discAmt = 0

			Dim lblTotalNet As Label = DirectCast(e.Row.FindControl("lblNet"), Label)
			lblTotalNet.Text = Format(CDbl(netAmt.ToString()), "#,##0.00 ; (#,##0.00)")
			netAmt = 0

		End If

	End Sub

	Protected Sub dgvRegSum_RowCreated(sender As Object, e As GridViewRowEventArgs)

	End Sub

	Protected Sub dgvRegSumCustGrp_RowDataBound(sender As Object, e As GridViewRowEventArgs)
		If e.Row.RowType = DataControlRowType.DataRow Then
			Dim tgrossAmt As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "grossamt").ToString())
			grossAmt += tgrossAmt

			Dim tvatAmt As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "vat").ToString())
			vatAmt += tvatAmt

			Dim tfhAmt As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fhamt").ToString())
			fhAmt += tfhAmt

			Dim tdiscAmt As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "discamt").ToString())
			discAmt += tdiscAmt

			Dim tnetAmt As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "netamt").ToString())
			netAmt += tnetAmt

			Dim tgrossAmt2 As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "grossamt").ToString())
			sgrossAmt += tgrossAmt2

			Dim tvatAmt2 As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "vat").ToString())
			svatAmt += tvatAmt2

			Dim tfhAmt2 As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fhamt").ToString())
			sfhAmt += tfhAmt2

			Dim tdiscAmt2 As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "discamt").ToString())
			sdiscAmt += tdiscAmt2

			Dim tnetAmt2 As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "netamt").ToString())
			snetAmt += tnetAmt2

			custno = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "custno").ToString())
			strCustName = DataBinder.Eval(e.Row.DataItem, "bussname").ToString()

		End If

		If e.Row.RowType = DataControlRowType.Footer Then
			Dim lblGTotal As Label = DirectCast(e.Row.FindControl("lblGTotal"), Label)
			lblGTotal.Text = "GRAND TOTAL"

			Dim lblTotalGross As Label = DirectCast(e.Row.FindControl("lblGross"), Label)
			lblTotalGross.Text = Format(CDbl(grossAmt.ToString()), "#,##0.00 ; (#,##0.00)")
			grossAmt = 0

			Dim lblTotalVat As Label = DirectCast(e.Row.FindControl("lblVat"), Label)
			lblTotalVat.Text = Format(CDbl(vatAmt.ToString()), "#,##0.00 ; (#,##0.00)")
			vatAmt = 0

			Dim lblTotalFh As Label = DirectCast(e.Row.FindControl("lblFH"), Label)
			lblTotalFh.Text = Format(CDbl(fhAmt.ToString()), "#,##0.00 ; (#,##0.00)")
			fhAmt = 0

			Dim lblTotalDisc As Label = DirectCast(e.Row.FindControl("lblDisc"), Label)
			lblTotalDisc.Text = Format(CDbl(discAmt.ToString()), "#,##0.00 ; (#,##0.00)")
			discAmt = 0

			Dim lblTotalNet As Label = DirectCast(e.Row.FindControl("lblNet"), Label)
			lblTotalNet.Text = Format(CDbl(netAmt.ToString()), "#,##0.00 ; (#,##0.00)")
			netAmt = 0

		End If


	End Sub

	Protected Sub dgvRegSumCustGrp_RowCreated(sender As Object, e As GridViewRowEventArgs)
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
			Dim dgvRegSumCustGrp As GridView = DirectCast(sender, GridView)
			Dim NewTotalRow As New GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert)
			NewTotalRow.Font.Bold = True
			NewTotalRow.BackColor = System.Drawing.Color.Gray
			NewTotalRow.ForeColor = System.Drawing.Color.White

			Dim HeaderCell As New TableCell()
			HeaderCell.Text = Format(custno, "#00000") & Space(1) & strCustName & " Total"
			HeaderCell.HorizontalAlign = HorizontalAlign.Center
			HeaderCell.Font.Size = 10
			HeaderCell.ColumnSpan = 9
			NewTotalRow.Cells.Add(HeaderCell)

			HeaderCell = New TableCell()
			HeaderCell.HorizontalAlign = HorizontalAlign.Right
			HeaderCell.Text = Format(CDbl(sgrossAmt.ToString()), "#,##0.00 ; (#,##0.00)")
			NewTotalRow.Cells.Add(HeaderCell)

			HeaderCell = New TableCell()
			HeaderCell.HorizontalAlign = HorizontalAlign.Right
			HeaderCell.Text = Format(CDbl(svatAmt.ToString()), "#,##0.00 ; (#,##0.00)")
			NewTotalRow.Cells.Add(HeaderCell)

			HeaderCell = New TableCell()
			HeaderCell.HorizontalAlign = HorizontalAlign.Right
			HeaderCell.Text = Format(CDbl(sfhAmt.ToString()), "#,##0.00 ; (#,##0.00)")
			NewTotalRow.Cells.Add(HeaderCell)

			HeaderCell = New TableCell()
			HeaderCell.HorizontalAlign = HorizontalAlign.Right
			HeaderCell.Text = Format(CDbl(sdiscAmt.ToString()), "#,##0.00 ; (#,##0.00)")
			NewTotalRow.Cells.Add(HeaderCell)

			HeaderCell = New TableCell()
			HeaderCell.HorizontalAlign = HorizontalAlign.Right
			HeaderCell.Text = Format(CDbl(snetAmt.ToString()), "#,##0.00 ; (#,##0.00)")
			NewTotalRow.Cells.Add(HeaderCell)

			dgvRegSumCustGrp.Controls(0).Controls.AddAt(e.Row.RowIndex + rowIndex, NewTotalRow)
			rowIndex += 1
			TotRowIndex += 1

			sgrossAmt = 0
			svatAmt = 0
			sfhAmt = 0
			sdiscAmt = 0
			snetAmt = 0

		End If


	End Sub

	Protected Sub dgvRegSumSmnGrp_RowDataBound(sender As Object, e As GridViewRowEventArgs)
		If e.Row.RowType = DataControlRowType.DataRow Then
			Dim tgrossAmt As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "grossamt").ToString())
			grossAmt += tgrossAmt

			Dim tvatAmt As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "vat").ToString())
			vatAmt += tvatAmt

			Dim tfhAmt As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fhamt").ToString())
			fhAmt += tfhAmt

			Dim tdiscAmt As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "discamt").ToString())
			discAmt += tdiscAmt

			Dim tnetAmt As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "netamt").ToString())
			netAmt += tnetAmt

			Dim tgrossAmt2 As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "grossamt").ToString())
			sgrossAmt += tgrossAmt2

			Dim tvatAmt2 As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "vat").ToString())
			svatAmt += tvatAmt2

			Dim tfhAmt2 As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fhamt").ToString())
			sfhAmt += tfhAmt2

			Dim tdiscAmt2 As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "discamt").ToString())
			sdiscAmt += tdiscAmt2

			Dim tnetAmt2 As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "netamt").ToString())
			snetAmt += tnetAmt2

			smnno = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "smnno").ToString())
			strCustName = DataBinder.Eval(e.Row.DataItem, "fullname").ToString()

		End If

		If e.Row.RowType = DataControlRowType.Footer Then
			Dim lblGTotal As Label = DirectCast(e.Row.FindControl("lblGTotal"), Label)
			lblGTotal.Text = "GRAND TOTAL"

			Dim lblTotalGross As Label = DirectCast(e.Row.FindControl("lblGross"), Label)
			lblTotalGross.Text = Format(CDbl(grossAmt.ToString()), "#,##0.00 ; (#,##0.00)")
			grossAmt = 0

			Dim lblTotalVat As Label = DirectCast(e.Row.FindControl("lblVat"), Label)
			lblTotalVat.Text = Format(CDbl(vatAmt.ToString()), "#,##0.00 ; (#,##0.00)")
			vatAmt = 0

			Dim lblTotalFh As Label = DirectCast(e.Row.FindControl("lblFH"), Label)
			lblTotalFh.Text = Format(CDbl(fhAmt.ToString()), "#,##0.00 ; (#,##0.00)")
			fhAmt = 0

			Dim lblTotalDisc As Label = DirectCast(e.Row.FindControl("lblDisc"), Label)
			lblTotalDisc.Text = Format(CDbl(discAmt.ToString()), "#,##0.00 ; (#,##0.00)")
			discAmt = 0

			Dim lblTotalNet As Label = DirectCast(e.Row.FindControl("lblNet"), Label)
			lblTotalNet.Text = Format(CDbl(netAmt.ToString()), "#,##0.00 ; (#,##0.00)")
			netAmt = 0

		End If
	End Sub

	Protected Sub dgvRegSumSmnGrp_RowCreated(sender As Object, e As GridViewRowEventArgs)
		Dim newRow As Boolean = False

		If (smnno > 0) AndAlso (DataBinder.Eval(e.Row.DataItem, "smnno") IsNot Nothing) Then
			If smnno <> Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "smnno").ToString()) Then
				newRow = True
			End If
		End If
		If (smnno > 0) AndAlso (DataBinder.Eval(e.Row.DataItem, "smnno") Is Nothing) Then
			newRow = True
			rowIndex = 0
		End If

		If newRow Then
			Dim dgvRegSumSmnGrp As GridView = DirectCast(sender, GridView)
			Dim NewTotalRow As New GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert)
			NewTotalRow.Font.Bold = True
			NewTotalRow.BackColor = System.Drawing.Color.Gray
			NewTotalRow.ForeColor = System.Drawing.Color.White

			Dim HeaderCell As New TableCell()
			HeaderCell.Text = Format(smnno, "#000") & Space(1) & strCustName & " Total"
			HeaderCell.HorizontalAlign = HorizontalAlign.Center
			HeaderCell.Font.Size = 10
			HeaderCell.ColumnSpan = 8
			NewTotalRow.Cells.Add(HeaderCell)

			HeaderCell = New TableCell()
			HeaderCell.HorizontalAlign = HorizontalAlign.Right
			HeaderCell.Text = Format(CDbl(sgrossAmt.ToString()), "#,##0.00 ; (#,##0.00)")
			NewTotalRow.Cells.Add(HeaderCell)

			HeaderCell = New TableCell()
			HeaderCell.HorizontalAlign = HorizontalAlign.Right
			HeaderCell.Text = Format(CDbl(svatAmt.ToString()), "#,##0.00 ; (#,##0.00)")
			NewTotalRow.Cells.Add(HeaderCell)

			HeaderCell = New TableCell()
			HeaderCell.HorizontalAlign = HorizontalAlign.Right
			HeaderCell.Text = Format(CDbl(sfhAmt.ToString()), "#,##0.00 ; (#,##0.00)")
			NewTotalRow.Cells.Add(HeaderCell)

			HeaderCell = New TableCell()
			HeaderCell.HorizontalAlign = HorizontalAlign.Right
			HeaderCell.Text = Format(CDbl(sdiscAmt.ToString()), "#,##0.00 ; (#,##0.00)")
			NewTotalRow.Cells.Add(HeaderCell)

			HeaderCell = New TableCell()
			HeaderCell.HorizontalAlign = HorizontalAlign.Right
			HeaderCell.Text = Format(CDbl(snetAmt.ToString()), "#,##0.00 ; (#,##0.00)")
			NewTotalRow.Cells.Add(HeaderCell)

			dgvRegSumSmnGrp.Controls(0).Controls.AddAt(e.Row.RowIndex + rowIndex, NewTotalRow)
			rowIndex += 1
			TotRowIndex += 1

			sgrossAmt = 0
			svatAmt = 0
			sfhAmt = 0
			sdiscAmt = 0
			snetAmt = 0

		End If
	End Sub

	Protected Sub dgvSalesProd_RowDataBound(sender As Object, e As GridViewRowEventArgs)

	End Sub

	Protected Sub dgvSalesProd_RowCreated(sender As Object, e As GridViewRowEventArgs)

	End Sub

	Private Sub dpSalesDate1_TextChanged(sender As Object, e As EventArgs) Handles dpSalesDate1.TextChanged, dpSalesDate2.TextChanged
		If dpSalesDate1.Text = Nothing Then
			Exit Sub
		ElseIf dpSalesDate2.Text = Nothing Then
			Exit Sub
		ElseIf CDate(dpSalesDate1.Text) > CDate(dpSalesDate2.Text) Then
			Exit Sub
		End If

		popCustSales()

	End Sub

	Private Sub popCustSales()
		dgvSalesProd.DataSource = Nothing
		dgvSalesProd.DataBind()

		cboSalesCust.Items.Clear()
		dt = GetDataTable("select distinct concat(a.custno,space(1),b.bussname) from saleshdrtbl a left join custmasttbl b on a.custno=b.custno " &
						  "where a.status <> 'void' and a.transdate between '" & Format(CDate(dpSalesDate1.Text), "yyyy-MM-dd") & "' and  " &
						  "'" & Format(CDate(dpSalesDate2.Text), "yyyy-MM-dd") & "' order by b.bussname")
		If Not CBool(dt.Rows.Count) Then
			Exit Sub
		Else
			cboSalesCust.Items.Add("")
			For Each dr As DataRow In dt.Rows
				cboSalesCust.Items.Add(dr.Item(0).ToString())

			Next

		End If

		dt.Dispose()


	End Sub

	Private Sub cboSalesCust_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboSalesCust.SelectedIndexChanged
		If dpSalesDate1.Text = Nothing Then
			Exit Sub
		ElseIf dpSalesDate2.Text = Nothing Then
			Exit Sub
		ElseIf CDate(dpSalesDate1.Text) > CDate(dpSalesDate2.Text) Then
			Exit Sub
		ElseIf cboSalesCust.Text = "" Then
			Exit Sub

		End If

		popCustProdSales()

	End Sub

	Private Sub popCustProdSales()
		dgvSalesProd.DataSource = Nothing
		dgvSalesProd.DataBind()

		cboSalesProd.Items.Clear()
		dt = GetDataTable("select distinct ifnull(c.codename,c.mmdesc) as mmdesc from salesdettbl a left join saleshdrtbl b " &
						  "on a.invno=b.invno left join mmasttbl c on a.codeno=c.codeno where b.status <> 'void' and " &
						  "b.transdate between '" & Format(CDate(dpSalesDate1.Text), "yyyy-MM-dd") & "' and  " &
						   "'" & Format(CDate(dpSalesDate2.Text), "yyyy-MM-dd") & "' and " &
						  "b.custno = '" & cboSalesCust.Text.Substring(0, 5) & "' order by ifnull(c.codename,c.mmdesc)")
		If Not CBool(dt.Rows.Count) Then
			Exit Sub
		Else
			cboSalesProd.Items.Add("")
			cboSalesProd.Items.Add("ALL")
			For Each dr As DataRow In dt.Rows
				cboSalesProd.Items.Add(dr.Item(0).ToString())

			Next

		End If

		dt.Dispose()

	End Sub

	Private Sub cboSalesProd_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboSalesProd.SelectedIndexChanged
		If dpSalesDate1.Text = Nothing Then
			Exit Sub
		ElseIf dpSalesDate2.Text = Nothing Then
			Exit Sub
		ElseIf CDate(dpSalesDate1.Text) > CDate(dpSalesDate2.Text) Then
			Exit Sub
		ElseIf cboSalesCust.Text = "" Then
			Exit Sub
		ElseIf cboSalesProd.Text = "" Then
			Exit Sub
		End If

		fillDGVsalesProd()

	End Sub

	Private Sub fillDGVsalesProd()
		dgvSalesProd.DataSource = Nothing
		dgvSalesProd.DataBind()

		Select Case cboSalesProd.Text
			Case "ALL"
				dt = GetDataTable("select * from saleshdrtbl where status <> 'void' and custno = '" & cboSalesCust.Text.Substring(0, 5) & "' " &
								  "and transdate between '" & Format(CDate(dpSalesDate1.Text), "yyyy-MM-dd") & "' and  " &
								  "'" & Format(CDate(dpSalesDate2.Text), "yyyy-MM-dd") & "' limit 1")
			Case Else
				dt = GetDataTable("select codeno from mmasttbl where ifnull(codename,mmdesc) = '" & cboSalesProd.Text & "'")
				If Not CBool(dt.Rows.Count) Then
					txtSalesCodeNo.Text = ""
					Exit Sub
				Else
					For Each dr As DataRow In dt.Rows
						txtSalesCodeNo.Text = dr.Item(0).ToString() & ""

					Next
				End If

				dt.Dispose()

				If txtSalesCodeNo.Text = "" Then
					Exit Sub
				End If

				dt = GetDataTable("select * from salesdettbl a left join saleshdrtbl b on a.invno=b.invno where " &
								  "b.status <> 'void' and b.custno = '" & cboSalesCust.Text.Substring(0, 5) & "' " &
								  "and b.transdate between '" & Format(CDate(dpSalesDate1.Text), "yyyy-MM-dd") & "' and  " &
								  "'" & Format(CDate(dpSalesDate2.Text), "yyyy-MM-dd") & "' and " &
								  "a.codeno = '" & txtSalesCodeNo.Text & "' limit 1")
		End Select

		If Not CBool(dt.Rows.Count) Then
			MsgBox("No Sales Found")
			dgvSalesProd.DataSource = Nothing
			Exit Sub
		End If

		Dim adapter As New MySqlDataAdapter
		Dim ds As New DataSet()

		sqldata = Nothing

		Select Case cboSalesProd.Text
			Case "ALL"
				sqldata = "select a.invno,b.docno,b.transdate,concat(b.shipto,space(1),c.bussname) as cust,a.codeno," &
							  "ifnull(d.codename,d.mmdesc) as mm,ifnull(a.qty,0) as qty,ifnull(a.wt,0) as wt,a.sp,a.itmamt," &
							  "b.dono,e.sono from salesdettbl a left join saleshdrtbl b on a.invno=b.invno left join custmasttbl c " &
							  "on b.shipto=c.custno left join mmasttbl d on a.codeno=d.codeno left join isshdrtbl e on b.dono=e.dono " &
							  "where b.status <> 'void' and b.custno = '" & cboSalesCust.Text.Substring(0, 5) & "' " &
							  "and b.transdate between '" & Format(CDate(dpSalesDate1.Text), "yyyy-MM-dd") & "' and  " &
							  "'" & Format(CDate(dpSalesDate2.Text), "yyyy-MM-dd") & "' group by a.transid order by b.transdate"
			Case Else
				sqldata = "select a.invno,b.docno,b.transdate,concat(b.shipto,space(1),c.bussname) as cust,a.codeno," &
							  "ifnull(d.codename,d.mmdesc) as mm,ifnull(a.qty,0) as qty,ifnull(a.wt,0) as wt,a.sp,a.itmamt," &
							  "b.dono,e.sono from salesdettbl a left join saleshdrtbl b on a.invno=b.invno left join custmasttbl c " &
							  "on b.shipto=c.custno left join mmasttbl d on a.codeno=d.codeno left join isshdrtbl e on b.dono=e.dono " &
							  "where b.status <> 'void' and b.custno = '" & cboSalesCust.Text.Substring(0, 5) & "' " &
							  "and b.transdate between '" & Format(CDate(dpSalesDate1.Text), "yyyy-MM-dd") & "' and  " &
							  "'" & Format(CDate(dpSalesDate2.Text), "yyyy-MM-dd") & "' and " &
							  "a.codeno = '" & txtSalesCodeNo.Text & "' group by a.transid order by b.transdate"

		End Select

		With dgvSalesProd
			.Columns(0).HeaderText = "Inv No"
			.Columns(1).HeaderText = "Doc No."
			.Columns(2).HeaderText = "Date"
			.Columns(3).HeaderText = "Ship To"
			.Columns(4).HeaderText = "Code No"
			.Columns(5).HeaderText = "Description"
			.Columns(6).HeaderText = "Qty"
			.Columns(7).HeaderText = "Wt"
			.Columns(8).HeaderText = "SP"
			.Columns(9).HeaderText = "Amount"
			.Columns(10).HeaderText = "DO No."
			.Columns(11).HeaderText = "SO No."

		End With

		conn.Open()
		Dim command As New MySqlCommand(sqldata, conn)
		adapter.SelectCommand = command
		adapter.Fill(ds)
		adapter.Dispose()
		command.Dispose()
		conn.Close()
		dgvSalesProd.DataSource = ds.Tables(0)
		dgvSalesProd.DataBind()

	End Sub
End Class