Imports MySql.Data.MySqlClient
Imports System.IO
Imports System.Data.OleDb
Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Data.DataTable
Imports System.Data.SqlTypes
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Configuration

Public Class SalesmanModule
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
			Exit Sub

		End If


		If Not Me.IsPostBack Then
			lblUser.Text = Session("UserID")
			lblGrpUser.Text = Session("UserGrp")
			vCurrForm = Session("CForm")
			lblCform.Text = Session("CForm")

			Select Case vCurrForm
				Case "Smn"
					lblSmnName.Visible = True
					cboSmname.Visible = False
					lblSmnName.Text = vLogSmn
					lblAccess.Text = "Salesman's Access"
					TabPanel5.Visible = True
					GetSmnDetails()

				Case "Mgr"
					cboSmname.Visible = True
					lblSmnName.Visible = False
					lblAccess.Text = "Sales Manager's Access"
					GetSmnList()
					TabPanel5.Visible = False
			End Select

		End If

	End Sub

	Private Sub GetPCforSO()
		cboPClass.Items.Clear()
		dt = GetDataTable("select concat(pc,space(1),pclass) from pctrtbl where tradetype = 'trade'")
		If Not CBool(dt.Rows.Count) Then
			Exit Sub

		Else
			cboPClass.Items.Add("")
			For Each dr As DataRow In dt.Rows
				cboPClass.Items.Add(dr.Item(0).ToString() & "")

			Next

		End If

		Call dt.Dispose()


	End Sub

	Private Sub GetSmnList()
		cboSmname.Items.Clear()
		dt = GetDataTable("select concat(smnno,space(1),fullname) from smnmtbl where status = 'active' order by fullname")
		If Not CBool(dt.Rows.Count) Then
			Exit Sub

		Else
			cboSmname.Items.Add("")
			cboSmname.Items.Add("ALL")
			For Each dr As DataRow In dt.Rows
				cboSmname.Items.Add(dr.Item(0).ToString() & "")

			Next

		End If

		Call dt.Dispose()

	End Sub

	Private Sub GetSmnDetailsForSO()

	End Sub

	Protected Sub GetSmnDetails()
		Select Case vCurrForm
			Case "Smn"
				lblSmnNo.Visible = True
				cboSmnName.Visible = False
				lblSmnNo.Text = lblSmnName.Text
				GetCustForSO()

				dt = GetDataTable("select concat(a.areano,space(1),b.areaname),b.covarea from smnmtbl a " &
							 	  "left join areatbl b on a.areano = b.areano where " &
								  "a.smnno = '" & lblSmnName.Text.Substring(0, 3) & "'")
			Case "Mgr"
				If cboSmname.Text = "" Then
					Exit Sub
				End If

				Select Case cboSmname.Text
					Case "ALL"
						cboSmnName.Visible = False
						'pop SO Smn
						lblArea.Text = "All Areas"
						lblAreaCov.Text = "All"
						Exit Sub

					Case Else
						lblSmnNo.Text = cboSmname.Text
						GetCustForSO()
						dt = GetDataTable("select concat(a.areano,space(1),b.areaname),b.covarea from smnmtbl a " &
									 	  "left join areatbl b on a.areano = b.areano where " &
										  "a.smnno = '" & cboSmname.Text.Substring(0, 3) & "'")
				End Select

				lblSmnNo.Visible = False
				cboSmnName.Visible = True

		End Select

		If Not CBool(dt.Rows.Count) Then
			lblArea.Text = ""
			lblAreaCov.Text = ""
			Exit Sub

		Else
			For Each dr As DataRow In dt.Rows
				lblArea.Text = dr.Item(0).ToString() & ""
				lblAreaCov.Text = dr.Item(1).ToString() & ""

			Next

		End If

		Call dt.Dispose()

	End Sub

	Private Sub GetCustForSO()

		cboCustName.Items.Clear()

		Select Case vCurrForm
			Case "Smn"
				admSmnNo = lblSmnName.Text.Substring(0, 3)

			Case "Mgr"
				admSmnNo = cboSmname.Text.Substring(0, 3)

		End Select

		dt = GetDataTable("select concat(custno,space(1),bussname) from custmasttbl where " &
						  "smnno = '" & admSmnNo & "' and accttype ='main' order by bussname")
		If Not CBool(dt.Rows.Count) Then
			Exit Sub
		Else
			cboCustName.Items.Add("")
			For Each dr As DataRow In dt.Rows
				cboCustName.Items.Add(dr.Item(0).ToString())

			Next
		End If

		dt.Dispose()

	End Sub

	Protected Sub lbClose_Click(sender As Object, e As EventArgs)
		Response.Redirect("SalesAndDist.aspx")
	End Sub

	Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged,
		RadioButton2.CheckedChanged, RadioButton3.CheckedChanged, RadioButton4.CheckedChanged
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

		FillSOmon()

	End Sub

	Protected Sub dgvSOmon_RowDataBound(sender As Object, e As GridViewRowEventArgs)

	End Sub

	Protected Sub dgvSOmon_RowCreated(sender As Object, e As GridViewRowEventArgs)

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
				admOpt = "DR SAVED"
			Case RadioButton4.Checked
				admOpt = "SERVED"
		End Select

		PopCustList()

	End Sub

	Private Sub PopCustList()
		cboCustomer.Items.Clear()

		Select Case vCurrForm
			Case "Smn"
				admSmnNo = lblSmnName.Text.Substring(0, 3)

				Select Case admOpt
					Case "ALL"
						dt = GetDataTable("select concat(a.custno,space(1),b.bussname) from sohdrtbl a " &
										  "left join custmasttbl b on a.custno=b.custno where a.smnno = '" & admSmnNo & "' " &
										  "and a.transdate between '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and  " &
										  "'" & Format(CDate(dpTo.Text), "yyyy-MM-dd") & "' group by a.custno order by b.bussname")
					Case Else
						dt = GetDataTable("select concat(a.custno,space(1),b.bussname) from sohdrtbl a " &
										  "left join custmasttbl b on a.custno=b.custno where a.smnno = '" & admSmnNo & "' " &
										  "and a.transdate between '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and  " &
										  "'" & Format(CDate(dpTo.Text), "yyyy-MM-dd") & "' and a.delstat = '" & admOpt & "' " &
										  "group by a.custno order by b.bussname")
				End Select

			Case "Mgr"
				admSmnNo = cboSmname.Text.Substring(0, 3)
				Select Case cboSmname.Text
					Case "ALL"
						Select Case UCase(admOpt)
							Case "ALL"
								dt = GetDataTable("select concat(a.custno,space(1),b.bussname) from sohdrtbl a " &
												  "left join custmasttbl b on a.custno=b.custno where " &
												  "a.transdate between '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and  " &
												  "'" & Format(CDate(dpTo.Text), "yyyy-MM-dd") & "' group by a.custno order by b.bussname")

							Case Else
								dt = GetDataTable("select concat(a.custno,space(1),b.bussname) from sohdrtbl a " &
												  "left join custmasttbl b on a.custno=b.custno where a.delstat = '" & admOpt & "' " &
												  "and a.transdate between '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and  " &
												   "'" & Format(CDate(dpTo.Text), "yyyy-MM-dd") & "' group by a.custno order by b.bussname")
						End Select

					Case Else
						Select Case UCase(admOpt)
							Case "ALL"
								dt = GetDataTable("select concat(a.custno,space(1),b.bussname) from sohdrtbl a " &
												  "left join custmasttbl b on a.custno=b.custno where a.smnno = '" & admSmnNo & "' " &
												  "and a.transdate between '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and  " &
												  "'" & Format(CDate(dpTo.Text), "yyyy-MM-dd") & "' group by a.custno order by b.bussname")

							Case Else
								dt = GetDataTable("select concat(a.custno,space(1),b.bussname) from sohdrtbl a " &
												  "left join custmasttbl b on a.custno=b.custno where a.smnno = '" & lblSmnName.Text.Substring(0, 3) & "' " &
												   "and a.transdate between '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and  " &
												   "'" & Format(CDate(dpTo.Text), "yyyy-MM-dd") & "' and a.delstat = '" & admOpt & "' " &
												  "group by a.custno order by b.bussname")
						End Select

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

	Private Sub cboCustomer_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboCustomer.SelectedIndexChanged

		Select Case vCurrForm
			Case "Mgr"
				If cboCustomer.Text = "" Then
					Exit Sub

				End If
			Case "Smn"
				If lblSmnName.Text = Nothing Or lblSmnName.Text = "" Then
					Exit Sub

				End If

		End Select

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

		FillSOmon()
		PopMMdesc()
		dgvSOdet.DataSource = Nothing
		dgvSOdet.DataBind()

	End Sub

	Private Sub PopMMdesc()
		'cboMMdesc.Items.Clear()
		'txtCodeNo.Text = ""
		'dt = GetDataTable("select ifnull(c.codename,c.mmdesc) from tempinvbals a left join mmasttbl c on a.codeno = c.codeno " &
		'				  "where a.user = '" & lblUser.Text & "' group by a.codeno order by ifnull(c.codename,c.mmdesc)")
		'If Not CBool(dt.Rows.Count) Then
		'	Exit Sub
		'Else
		'	For Each dr As DataRow In dt.Rows
		'		cboMMdesc.Items.Add(dr.Item(0).ToString())

		'	Next

		'End If

		'dt.Dispose()

	End Sub

	Private Sub SOtempTable()
		Select Case vCurrForm
			Case "Smn"
				admSmnNo = lblSmnName.Text.Substring(0, 3)

			Case "Mgr"
				admSmnNo = cboSmname.Text.Substring(0, 3)
		End Select

		sql = "delete from tempinvbals where user ='" & lblUser.Text & "'"
		ExecuteNonQuery(sql)

		Select Case UCase(admOpt)
			Case "ALL"
				Select Case cboCustomer.Text
					Case "ALL Customers"
						sql = "insert into tempinvbals(pono,venno,transdate,codeno,recqty,recwt,uc,lotno,mmdesc,user) " &
							  "select a.sono,b.custno,b.transdate,a.codeno,sum(a.qty),sum(a.wt),a.sp,b.status,b.delstat," &
							  "'" & lblUser.Text & "' from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
							  "where b.transdate between '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and " &
							  "'" & Format(CDate(dpTo.Text), "yyyy-MM-dd") & "' and b.smnno = '" & admSmnNo & "' " &
							  "group by a.sono,a.codeno"
						ExecuteNonQuery(sql)
					Case Else
						sql = "insert into tempinvbals(pono,venno,transdate,codeno,recqty,recwt,uc,lotno,mmdesc,user) " &
							  "select a.sono,b.custno,b.transdate,a.codeno,sum(a.qty),sum(a.wt),a.sp,b.status,b.delstat," &
							  "'" & lblUser.Text & "' from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
							  "where b.transdate between '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and " &
							  "'" & Format(CDate(dpTo.Text), "yyyy-MM-dd") & "' and b.custno = '" & cboCustomer.Text.Substring(0, 5) & "' group by a.sono,a.codeno"
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
							  "'" & Format(CDate(dpTo.Text), "yyyy-MM-dd") & "' and b.custno = '" & cboCustomer.Text.Substring(0, 5) & "' group by a.sono,a.codeno"
						ExecuteNonQuery(sql)
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

	Private Sub FillSOmon()

		SOtempTable()

		Dim adapter As New MySqlDataAdapter
		Dim ds As New DataSet()
		'Dim i As Integer = 0
		sqldata = Nothing

		Select Case vCurrForm
			Case "Smn"
				admSmnNo = lblSmnName.Text.Substring(0, 3)

			Case "Mgr"
				admSmnNo = cboSmname.Text.Substring(0, 3)
		End Select

		Select Case UCase(admOpt)
			Case "ALL"
				Select Case cboCustomer.Text
					Case "ALL Customers"
						dt = GetDataTable("select * from sohdrtbl a left join custmasttbl b on a.custno=b.custno " &
										  "left join custmasttbl c on a.shipto=c.custno where a.smnno = '" & admSmnNo & "' " &
										  "and a.transdate between '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and " &
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
						dt = GetDataTable("select * from sohdrtbl a left join custmasttbl b on a.custno=b.custno " &
										  "left join custmasttbl c on a.shipto=c.custno where a.smnno = '" & admSmnNo & "' " &
										  "and a.transdate between '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and " &
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
								  "'" & Format(CDate(dpTo.Text), "yyyy-MM-dd") & "' and a.custno = '" & cboCustomer.Text.Substring(0, 5) & "'"
				End Select

			Case Else
				Select Case cboCustomer.Text
					Case "ALL Customers"
						dt = GetDataTable("select * from sohdrtbl a left join custmasttbl b on a.custno=b.custno " &
										  "left join custmasttbl c on a.shipto=c.custno where a.smnno = '" & admSmnNo & "' " &
										  "and a.transdate between '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and " &
										  "'" & Format(CDate(dpTo.Text), "yyyy-MM-dd") & "' and a.delstat = '" & admOpt & "' limit 1")
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
						dt = GetDataTable("select * from sohdrtbl a left join custmasttbl b on a.custno=b.custno " &
										  "left join custmasttbl c on a.shipto=c.custno where a.smnno = '" & admSmnNo & "' " &
										  "and a.transdate between '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and " &
										  "'" & Format(CDate(dpTo.Text), "yyyy-MM-dd") & "' and a.delstat = '" & admOpt & "' limit 1")
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

	Protected Sub dgvSOdet_RowDataBound(sender As Object, e As GridViewRowEventArgs)

	End Sub

	Protected Sub dgvSOdet_RowCreated(sender As Object, e As GridViewRowEventArgs)

	End Sub

	Private Sub dpSalesDate1_TextChanged(sender As Object, e As EventArgs) Handles dpSalesDate1.TextChanged, dpSalesDate2.TextChanged
		Select Case vCurrForm
			Case "Smn"

			Case "Mgr"
				If cboSmname.Text = "" Then
					Exit Sub
				End If

		End Select

		If dpSalesDate1.Text = Nothing Then
			Exit Sub
		ElseIf dpSalesDate2.Text = Nothing Then
			Exit Sub
		End If

		If Format(CDate(dpSalesDate1.Text), "yyyy-MM-dd") > Format(CDate(dpSalesDate2.Text), "yyyy-MM-dd") Then
			Exit Sub

		End If

		PopCustSales()

	End Sub

	Private Sub PopCustSales()
		Select Case vCurrForm
			Case "Smn"
				admSmnNo = lblSmnName.Text.Substring(0, 3)

			Case "Mgr"
				admSmnNo = cboSmname.Text.Substring(0, 3)
		End Select

		cboSalesCust.Items.Clear()

		Select Case cboSmname.Text
			Case "ALL"
				dt = GetDataTable("select concat(a.custno,space(1),b.bussname) from saleshdrtbl a left join custmasttbl b on " &
								  "a.custno = b.custno where a.transdate between '" & Format(CDate(dpSalesDate1.Text), "yyyy-MM-dd") & "' " &
								 "and '" & Format(CDate(dpSalesDate2.Text), "yyyy-MM-dd") & "' " &
								 "and a.status <> 'void' group by a.custno order by b.bussname")
			Case Else
				dt = GetDataTable("select concat(a.custno,space(1),b.bussname) from saleshdrtbl a left join custmasttbl b on " &
								  "a.custno = b.custno where a.transdate between '" & Format(CDate(dpSalesDate1.Text), "yyyy-MM-dd") & "' " &
								 "and '" & Format(CDate(dpSalesDate2.Text), "yyyy-MM-dd") & "' and a.smnno = '" & admSmnNo & "' " &
								 "and a.status <> 'void' group by a.custno order by b.bussname")
		End Select

		If Not CBool(dt.Rows.Count) Then
			Exit Sub
		Else
			cboSalesCust.Items.Add("")
			cboSalesCust.Items.Add("ALL")
			For Each dr As DataRow In dt.Rows
				cboSalesCust.Items.Add(dr.Item(0).ToString())

			Next

		End If

		dt.Dispose()

	End Sub

	Protected Sub dgvInvoice_RowDataBound(sender As Object, e As GridViewRowEventArgs)

	End Sub

	Protected Sub dgvInvoice_RowCreated(sender As Object, e As GridViewRowEventArgs)

	End Sub

	Private Sub cboSalesCust_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboSalesCust.SelectedIndexChanged
		If cboSalesCust.Text = "" Then
			Exit Sub
		End If

		Select Case TabContainer2.ActiveTabIndex
			Case 0
				FillDgvSalesInv()
			Case 1
				FillDgvSalesASP()
			Case 2

		End Select



	End Sub

	Private Sub GetASPSales()
		Select Case vCurrForm
			Case "Smn"
				admSmnNo = lblSmnName.Text.Substring(0, 3)

			Case "Mgr"
				admSmnNo = cboSmname.Text.Substring(0, 3)
		End Select

		sql = "delete from tempasp where user = '" & lblUser.Text & "'"
		ExecuteNonQuery(sql)

		Select Case cboSalesCust.Text
			Case "ALL"
				sql = "insert into tempasp(codeno,mmdesc,qty,wt,amt,billref,pc,pclass,user) select a.codeno,b.mmdesc,sum(a.qty)," &
					   "sum(a.wt),sum(a.itmamt),b.billref,b.pc,d.pclass,'" & lblUser.Text & "' from salesdettbl a " &
					  "left join mmasttbl b on a.codeno=b.codeno left join saleshdrtbl c on a.invno=c.invno " &
					  "left join pctrtbl d on b.pc=d.pc where c.transdate between '" & Format(CDate(dpSalesDate1.Text), "yyyy-MM-dd") & "' and " &
					  "'" & Format(CDate(dpSalesDate2.Text), "yyyy-MM-dd") & "' and c.status <> 'void' and " &
					  "c.smnno = '" & admSmnNo & "' and a.sp <> 0 and c.user <> 'Admin'  group by a.codeno"
				ExecuteNonQuery(sql)

			Case Else
				sql = "insert into tempasp(codeno,mmdesc,qty,wt,amt,billref,pc,pclass,user) select a.codeno,b.mmdesc,sum(a.qty)," &
					   "sum(a.wt),sum(a.itmamt),b.billref,b.pc,d.pclass,'" & lblUser.Text & "' from salesdettbl a " &
					  "left join mmasttbl b on a.codeno=b.codeno left join saleshdrtbl c on a.invno=c.invno " &
					  "left join pctrtbl d on b.pc=d.pc where c.transdate between '" & Format(CDate(dpSalesDate1.Text), "yyyy-MM-dd") & "' and " &
					  "'" & Format(CDate(dpSalesDate2.Text), "yyyy-MM-dd") & "' and c.status <> 'void' and " &
					  "c.smnno = '" & admSmnNo & "' and c.custno =  '" & cboSalesCust.Text.Substring(0, 5) & "' " &
					  "and a.sp <> 0 And c.user <> 'Admin' group by a.codeno"
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
	End Sub

	Private Sub FillDgvSalesASP()
		GetASPSales()

		Dim adapter As New MySqlDataAdapter
		Dim ds As New DataSet()
		sqldata = Nothing

		dt = GetDataTable("select sum(ifnull(qty,0)),sum(ifnull(wt,0)),sum(ifnull(amt,0)) from tempasp " &
						  "where user = '" & lblUser.Text & "' group by user")
		If Not CBool(dt.Rows.Count) Then
			dgvSalesSum.DataSource = Nothing
			dgvSalesSum.DataBind()
			Exit Sub

		Else
			For Each dr As DataRow In dt.Rows
				lblASPqty.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0")
				lblASPwt.Text = Format(CDbl(dr.Item(1).ToString()), "#,##0.00")
				lblASPamt.Text = Format(CDbl(dr.Item(2).ToString()), "#,##0.00")

			Next

		End If

		dt.Dispose()

		Try
			sqldata = "select a.codeno,ifnull(b.codename,b.mmdesc) as mmdesc,a.qty,a.wt,a.amt,a.asp " &
					  "from tempasp a left join mmasttbl b on a.codeno=b.codeno where a.user = '" & lblUser.Text & "'"

		Catch ex As Exception
			lblMsg.Text = ErrorToString()

		End Try

		With dgvSalesSum
			.Columns(0).HeaderText = "Code No."
			.Columns(1).HeaderText = "Product/SKU"
			.Columns(2).HeaderText = "Qty"
			.Columns(3).HeaderText = "Wt"
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
		dgvSalesSum.DataSource = ds.Tables(0)
		dgvSalesSum.DataBind()


	End Sub

	Private Sub FillDgvSalesInv()
		Select Case vCurrForm
			Case "Smn"
				admSmnNo = lblSmnName.Text.Substring(0, 3)
			Case "Mgr"
				admSmnNo = cboSmname.Text.Substring(0, 3)
		End Select

		Dim adapter As New MySqlDataAdapter
		Dim ds As New DataSet()
		sqldata = Nothing

		Select Case cboSalesCust.Text
			Case "ALL"
				dt = GetDataTable("select sum(ifnull(netamt,0)) from saleshdrtbl where smnno = '" & admSmnNo & "' and status <> 'void' " &
								  "and transdate between '" & Format(CDate(dpSalesDate1.Text), "yyyy-MM-dd") & "' and " &
								  "'" & Format(CDate(dpSalesDate2.Text), "yyyy-MM-dd") & "' group by smnno")
				If Not CBool(dt.Rows.Count) Then
					dgvInvoice.DataSource = Nothing
					dgvInvoice.DataBind()
					Exit Sub

				Else
					For Each dr As DataRow In dt.Rows
						lblSalesTotal.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0.00")

					Next

				End If

				dt.Dispose()

				Try
					sqldata = "select a.invno,a.tc,a.docno,a.transdate,concat(a.custno,space(1),b.bussname) as custno," &
							  "concat(a.shipto,space(1),c.bussname) as shipto,ifnull(a.netamt,0) as amt,a.status as stat " &
							  "from saleshdrtbl a left join custmasttbl b on a.custno=b.custno left join custmasttbl c " &
							  "on a.shipto=c.custno where a.smnno = '" & admSmnNo & "' and a.status <> 'void' and " &
							   "a.transdate between '" & Format(CDate(dpSalesDate1.Text), "yyyy-MM-dd") & "' and " &
							  "'" & Format(CDate(dpSalesDate2.Text), "yyyy-MM-dd") & "'"

				Catch ex As Exception
					lblMsg.Text = ErrorToString()

				End Try

			Case Else
				dt = GetDataTable("select sum(ifnull(netamt,0)) from saleshdrtbl where smnno = '" & admSmnNo & "' and " &
								  "status <> 'void' and custno = '" & cboSalesCust.Text.Substring(0, 5) & "' and " &
								  "transdate between '" & Format(CDate(dpSalesDate1.Text), "yyyy-MM-dd") & "' and " &
								  "'" & Format(CDate(dpSalesDate2.Text), "yyyy-MM-dd") & "'  group by smnno")
				If Not CBool(dt.Rows.Count) Then
					dgvInvoice.DataSource = Nothing
					dgvInvoice.DataBind()
					Exit Sub

				Else
					For Each dr As DataRow In dt.Rows
						lblSalesTotal.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0.00")

					Next

				End If

				dt.Dispose()

				Try
					sqldata = "select a.invno,a.tc,a.docno,a.transdate,concat(a.custno,space(1),b.bussname) as custno," &
							  "concat(a.shipto,space(1),c.bussname) as shipto,ifnull(a.netamt,0) as amt,a.status as stat " &
							  "from saleshdrtbl a left join custmasttbl b on a.custno=b.custno left join custmasttbl c " &
							  "on a.shipto=c.custno where a.smnno = '" & admSmnNo & "' and a.status <> 'void' and " &
							  "a.custno = '" & cboSalesCust.Text.Substring(0, 5) & "' and " &
							  "a.transdate between '" & Format(CDate(dpSalesDate1.Text), "yyyy-MM-dd") & "' and " &
							  "'" & Format(CDate(dpSalesDate2.Text), "yyyy-MM-dd") & "'"

				Catch ex As Exception
					lblMsg.Text = ErrorToString()

				End Try
		End Select

		With dgvInvoice
			.Columns(0).HeaderText = "Inv No."
			.Columns(1).HeaderText = "TC"
			.Columns(2).HeaderText = "Doc No."
			.Columns(3).HeaderText = "Posting Date"
			.Columns(4).HeaderText = "Customer/Sold To"
			.Columns(5).HeaderText = "Ship To"
			.Columns(6).HeaderText = "Amount"
			.Columns(7).HeaderText = "Status"

		End With


		conn.Open()
		Dim command As New MySqlCommand(sqldata, conn)
		adapter.SelectCommand = command
		adapter.Fill(ds)
		adapter.Dispose()
		command.Dispose()
		conn.Close()
		dgvInvoice.DataSource = ds.Tables(0)
		dgvInvoice.DataBind()

	End Sub

	Private Sub cboSmname_TextChanged(sender As Object, e As EventArgs) Handles cboSmname.TextChanged
		GetSmnDetails()
		ClrAllData()

	End Sub

	Private Sub ClrAllData()
		dgvInvoice.DataSource = Nothing
		dgvInvoice.DataBind()
		lblSalesTotal.Text = "0.00"

		dgvSOdet.DataSource = Nothing
		dgvSOdet.DataBind()

		dgvSOmon.DataSource = Nothing
		dgvSOmon.DataBind()

		cboCustomer.Text = Nothing
		cboSalesCust.Text = Nothing

		Select Case TabContainer1.ActiveTabIndex
			Case 0

			Case 1

			Case 2

			Case 3
				Select Case TabContainer3.ActiveTabIndex
					Case 0

					Case 1

				End Select

			Case 4

			Case 5

		End Select


	End Sub

	Private Sub lbNew_Click(sender As Object, e As EventArgs) Handles lbNew.Click
		Select Case vCurrForm
			Case "Smn"
				lblSmnName.Visible = True
				cboSmname.Visible = False
				lblSmnName.Text = vLogSmn
				lblAccess.Text = "Salesman's Access"
				GetSmnDetails()

			Case "Mgr"
				cboSmname.Visible = True
				lblSmnName.Visible = False
				lblAccess.Text = "Sales Manager's Access"
				GetSmnList()

		End Select

		ClrAllData()

	End Sub

	Protected Sub dgvCollection_RowDataBound(sender As Object, e As GridViewRowEventArgs)

	End Sub

	Protected Sub dgvCollection_RowCreated(sender As Object, e As GridViewRowEventArgs)

	End Sub

	Private Sub cboColCust_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboColCust.SelectedIndexChanged
		If cboColCust.Text = "" Then
			Exit Sub
		End If

		FillDgvColl()

	End Sub

	Private Sub FillDgvColl()
		Select Case vCurrForm
			Case "Smn"
				admSmnNo = lblSmnName.Text.Substring(0, 3)

			Case "Mgr"
				admSmnNo = cboSmname.Text.Substring(0, 3)
		End Select

		Dim adapter As New MySqlDataAdapter
		Dim ds As New DataSet()
		sqldata = Nothing

		Select Case cboColCust.Text
			Case "ALL"
				dt = GetDataTable("select sum(ifnull(colamt,0)) from colhdrtbl where smnno = '" & admSmnNo & "' and status <> 'void' " &
								  "and transdate between '" & Format(CDate(dpColDate1.Text), "yyyy-MM-dd") & "' and " &
								  "'" & Format(CDate(dpColDate2.Text), "yyyy-MM-dd") & "' group by smnno")
				If Not CBool(dt.Rows.Count) Then
					dgvCollection.DataSource = Nothing
					dgvCollection.DataBind()
					Exit Sub

				Else
					For Each dr As DataRow In dt.Rows
						lblTotCol.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0.00")

					Next

				End If

				dt.Dispose()

				Try
					sqldata = "select a.orno,b.doctype,b.docno,b.transdate,concat(b.custno,space(1),c.bussname) as custno," &
							  "a.refno as invno,a.tc,a.invno as sino,ifnull(a.oramt,0) as amt,b.coltype from coldettbl a " &
							  "left join colhdrtbl b on a.orno=b.orno left join custmasttbl c on b.custno=c.custno " &
							  "where b.smnno = '" & admSmnNo & "' and b.status <> 'void' and b.transdate " &
							  "between '" & Format(CDate(dpColDate1.Text), "yyyy-MM-dd") & "' and " &
							  "'" & Format(CDate(dpColDate2.Text), "yyyy-MM-dd") & "' and a.oramt <> 0 group by a.transid"

				Catch ex As Exception
					lblMsg.Text = ErrorToString()

				End Try

			Case Else
				dt = GetDataTable("select sum(ifnull(colamt,0)) from colhdrtbl where smnno = '" & admSmnNo & "' and " &
								  "status <> 'void' and custno = '" & cboColCust.Text.Substring(0, 5) & "' and " &
								  "transdate between '" & Format(CDate(dpColDate1.Text), "yyyy-MM-dd") & "' and " &
								  "'" & Format(CDate(dpColDate2.Text), "yyyy-MM-dd") & "'  group by smnno")
				If Not CBool(dt.Rows.Count) Then
					dgvCollection.DataSource = Nothing
					dgvCollection.DataBind()
					Exit Sub

				Else
					For Each dr As DataRow In dt.Rows
						lblTotCol.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0.00")

					Next

				End If

				dt.Dispose()

				Try
					sqldata = "select a.orno,b.doctype,b.docno,b.transdate,concat(b.custno,space(1),c.bussname) as custno," &
							  "a.refno as invno,a.tc,a.invno as sino,ifnull(a.oramt,0) as amt,b.coltype from coldettbl a " &
							  "left join colhdrtbl b on a.orno=b.orno left join custmasttbl c on b.custno=c.custno " &
							  "where b.smnno = '" & admSmnNo & "' and b.status <> 'void' and b.transdate " &
							  "between '" & Format(CDate(dpColDate1.Text), "yyyy-MM-dd") & "' and " &
							  "'" & Format(CDate(dpColDate2.Text), "yyyy-MM-dd") & "' and a.oramt <> 0 " &
							  "and b.custno = '" & cboColCust.Text.Substring(0, 5) & "' group by a.transid"

				Catch ex As Exception
					lblMsg.Text = ErrorToString()

				End Try
		End Select

		With dgvCollection
			.Columns(0).HeaderText = "Or No."
			.Columns(1).HeaderText = "Doc Type"
			.Columns(2).HeaderText = "Doc No."
			.Columns(3).HeaderText = "Posting Date"
			.Columns(4).HeaderText = "Customer"
			.Columns(5).HeaderText = "Inv No."
			.Columns(6).HeaderText = "TC"
			.Columns(7).HeaderText = "SI/DR No."
			.Columns(8).HeaderText = "Amount"
			.Columns(9).HeaderText = "Col Type"

		End With

		conn.Open()
		Dim command As New MySqlCommand(sqldata, conn)
		adapter.SelectCommand = command
		adapter.Fill(ds)
		adapter.Dispose()
		command.Dispose()
		conn.Close()
		dgvCollection.DataSource = ds.Tables(0)
		dgvCollection.DataBind()

	End Sub

	Private Sub dpColDate1_TextChanged(sender As Object, e As EventArgs) Handles dpColDate1.TextChanged, dpColDate2.TextChanged
		Select Case vCurrForm
			Case "Smn"

			Case "Mgr"
				If cboSmname.Text = "" Then
					Exit Sub
				End If

		End Select

		If dpColDate1.Text = Nothing Then
			Exit Sub
		ElseIf dpColDate2.Text = Nothing Then
			Exit Sub
		End If

		ClrColTab()
		GetCustColl()

	End Sub

	Private Sub GetCustColl()
		Select Case vCurrForm
			Case "Smn"
				admSmnNo = lblSmnName.Text.Substring(0, 3)

			Case "Mgr"
				admSmnNo = cboSmname.Text.Substring(0, 3)
		End Select

		cboColCust.Items.Clear()
		Select Case cboSmname.Text
			Case "ALL"
				dt = GetDataTable("Select concat(a.custno,space(1),b.bussname) from colhdrtbl a left join custmasttbl b On " &
								  "a.custno = b.custno where a.transdate between '" & Format(CDate(dpColDate1.Text), "yyyy-MM-dd") & "' " &
								  "and '" & Format(CDate(dpColDate2.Text), "yyyy-MM-dd") & "' " &
								  "and a.status <> 'void' group by a.custno order by b.bussname")
			Case Else
				dt = GetDataTable("Select concat(a.custno,space(1),b.bussname) from colhdrtbl a left join custmasttbl b On " &
								  "a.custno = b.custno where a.transdate between '" & Format(CDate(dpColDate1.Text), "yyyy-MM-dd") & "' " &
								  "and '" & Format(CDate(dpColDate2.Text), "yyyy-MM-dd") & "' and a.smnno = '" & admSmnNo & "' " &
								  "and a.status <> 'void' group by a.custno order by b.bussname")
		End Select

		If Not CBool(dt.Rows.Count) Then

			Exit Sub
		Else
			cboColCust.Items.Add("")
			cboColCust.Items.Add("ALL")
			For Each dr As DataRow In dt.Rows
				cboColCust.Items.Add(dr.Item(0).ToString())

			Next

		End If

		dt.Dispose()

	End Sub

	Private Sub ClrColTab()
		cboColCust.Items.Clear()
		cboColCust.Text = Nothing
		dgvCollection.DataSource = Nothing
		dgvCollection.DataBind()
		lblTotCol.Text = "0.00"

	End Sub

	Protected Sub dgvSalesSum_RowDataBound(sender As Object, e As GridViewRowEventArgs)

	End Sub

	Protected Sub dgvSalesSum_RowCreated(sender As Object, e As GridViewRowEventArgs)

	End Sub

	Protected Sub dgvProdSum_RowDataBound(sender As Object, e As GridViewRowEventArgs)

	End Sub

	Protected Sub dgvProdSum_RowCreated(sender As Object, e As GridViewRowEventArgs)

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

		Select Case cboARcust.Text
			Case "ALL"
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

			Case Else
				If (shipto > 0) AndAlso (DataBinder.Eval(e.Row.DataItem, "shipto") IsNot Nothing) Then
					If shipto <> Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "shipto").ToString()) Then
						newRow = True
					End If
				End If

				If (shipto > 0) AndAlso (DataBinder.Eval(e.Row.DataItem, "shipto") Is Nothing) Then
					newRow = True
					rowIndex = 0
				End If

				If newRow Then
					dgvARreport = DirectCast(sender, GridView)
					Dim NewTotalRow2 As New GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert)
					NewTotalRow2.Font.Bold = True
					NewTotalRow2.BackColor = System.Drawing.Color.Gray
					NewTotalRow2.ForeColor = System.Drawing.Color.White

					Dim HeaderCell2 As New TableCell()
					HeaderCell2.Text = "Ship To: " & shipto & Space(1) & strShipName & " Total"
					HeaderCell2.HorizontalAlign = HorizontalAlign.Center
					HeaderCell2.Font.Size = 8
					HeaderCell2.ColumnSpan = 8
					NewTotalRow2.Cells.Add(HeaderCell2)

					HeaderCell2 = New TableCell()
					HeaderCell2.HorizontalAlign = HorizontalAlign.Right
					HeaderCell2.Text = Format(CDbl(sBal.ToString()), "#,##0.00 ; (#,##0.00)")
					NewTotalRow2.Cells.Add(HeaderCell2)

					HeaderCell2 = New TableCell()
					HeaderCell2.HorizontalAlign = HorizontalAlign.Right
					HeaderCell2.Text = Format(CDbl(sd30.ToString()), "#,##0.00 ; (#,##0.00)")
					NewTotalRow2.Cells.Add(HeaderCell2)

					HeaderCell2 = New TableCell()
					HeaderCell2.HorizontalAlign = HorizontalAlign.Right
					HeaderCell2.Text = Format(CDbl(sd60.ToString()), "#,##0.00 ; (#,##0.00)")
					NewTotalRow2.Cells.Add(HeaderCell2)

					HeaderCell2 = New TableCell()
					HeaderCell2.HorizontalAlign = HorizontalAlign.Right
					HeaderCell2.Text = Format(CDbl(sd90.ToString()), "#,##0.00 ; (#,##0.00)")
					NewTotalRow2.Cells.Add(HeaderCell2)

					HeaderCell2 = New TableCell()
					HeaderCell2.HorizontalAlign = HorizontalAlign.Right
					HeaderCell2.Text = Format(CDbl(sd120.ToString()), "#,##0.00 ; (#,##0.00)")
					NewTotalRow2.Cells.Add(HeaderCell2)

					HeaderCell2 = New TableCell()
					HeaderCell2.HorizontalAlign = HorizontalAlign.Right
					HeaderCell2.Text = Format(CDbl(sd150.ToString()), "#,##0.00 ; (#,##0.00)")
					NewTotalRow2.Cells.Add(HeaderCell2)

					HeaderCell2 = New TableCell()
					HeaderCell2.HorizontalAlign = HorizontalAlign.Right
					HeaderCell2.Text = Format(CDbl(sd180.ToString()), "#,##0.00 ; (#,##0.00)")
					NewTotalRow2.Cells.Add(HeaderCell2)

					HeaderCell2 = New TableCell()
					HeaderCell2.HorizontalAlign = HorizontalAlign.Right
					HeaderCell2.Text = Format(CDbl(sd121.ToString()), "#,##0.00 ; (#,##0.00)")
					NewTotalRow2.Cells.Add(HeaderCell2)

					HeaderCell2 = New TableCell()
					HeaderCell2.HorizontalAlign = HorizontalAlign.Right
					HeaderCell2.Text = Format(CDbl(sd91.ToString()), "#,##0.00 ; (#,##0.00)")
					NewTotalRow2.Cells.Add(HeaderCell2)

					dgvARreport.Controls(0).Controls.AddAt(e.Row.RowIndex + rowIndex, NewTotalRow2)
					rowIndex += 1
					TotRowIndex += 1

					sDR = 0
					sCR = 0
					sBal = 0
					scurr = 0
					sd30 = 0
					sd60 = 0
					sd90 = 0
					sd120 = 0
					sd150 = 0
					sd180 = 0
					sd121 = 0
					sd91 = 0
					spdc = 0
					snet = 0

				End If

		End Select

	End Sub

	Private Sub ARproc()
		If cboARcust.Text = "" Then
			Exit Sub

			'ElseIf cboSmname.Text = "" Then
			'	Exit Sub

			'ElseIf RadioButton5.Checked = False And RadioButton6.Checked = False Then
			'	AdmMsgBox("Select A/R Option")
			'	'lblMsg.Text = "Select A/R Option"
			'	Exit Sub
		End If

		getSetUpVer2()

		Select Case TabContainer3.ActiveTabIndex
			Case 0
				FillDgvAR()
			Case 1
				FillDgvARlist()
			Case 2
				FillDgvARsumAll()
		End Select

		'Select Case True
		'	Case RadioButton5.Checked
		'		Panel6.Visible = True
		'		Panel7.Visible = False
		'		dgvARreport.Visible = True
		'		dgvARsummary.Visible = False
		'		FillDgvAR()
		'	Case RadioButton6.Checked
		'		Panel7.Visible = True
		'		Panel6.Visible = False
		'		dgvARreport.Visible = False
		'		dgvARsummary.Visible = True
		'		FillDgvARlist()
		'End Select
	End Sub

	Private Sub cboARcust_TextChanged(sender As Object, e As EventArgs) Handles cboARcust.TextChanged


	End Sub

	Private Sub getSetUpVer2()
		Select Case vCurrForm
			Case "Smn"
				admSmnNo = lblSmnName.Text.Substring(0, 3)

			Case "Mgr"
				admSmnNo = cboSmname.Text.Substring(0, 3)
		End Select

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

		Select Case cboSmname.Text
			Case "ALL"
				Select Case cboARcust.Text
					Case "ALL"
						sql = "insert into tempaging_" & lblUser.Text & "(invno,transdate,originvamt,tc,custno,smnno,shipto,pc,areano,term," &
							  "custname,shiptoname,dsrno) select a.invno,a.transdate,round(a.netamt,2),a.tc,a.custno,a.smnno,ifnull(a.shipto,a.custno)," &
							  "a.pc,b.areano,b.term,b.bussname,c.bussname,a.docno from saleshdrtbl a left join custmasttbl b on a.custno=b.custno " &
							  "left join custmasttbl c on a.shipto=c.custno where a.transdate <= '" & Format(CDate(dpARdate.Text), "yyyy-MM-dd") & "' " &
							  "and a.status <> 'void'" ' and a.arflag is null
						ExecuteNonQuery(sql)

					Case Else
						sql = "insert into tempaging_" & lblUser.Text & "(invno,transdate,originvamt,tc,custno,smnno,shipto,pc,areano,term," &
							  "custname,shiptoname) select a.invno,a.transdate,round(a.netamt,2),a.tc,a.custno,a.smnno,ifnull(a.shipto,a.custno)," &
							  "a.pc,b.areano,b.term,b.bussname,c.bussname from saleshdrtbl a left join custmasttbl b on a.custno=b.custno " &
							  "left join custmasttbl c on a.shipto=c.custno where a.transdate <= '" & Format(CDate(dpARdate.Text), "yyyy-MM-dd") & "' " &
							  "and a.status <> 'void' and a.custno = '" & cboARcust.Text.Substring(0, 5) & "'" 'and a.arflag is null 
						ExecuteNonQuery(sql)

				End Select

			Case Else
				Select Case cboARcust.Text
					Case "ALL"
						sql = "insert into tempaging_" & lblUser.Text & "(invno,transdate,originvamt,tc,custno,smnno,shipto,pc,areano,term," &
							  "custname,shiptoname) select a.invno,a.transdate,round(a.netamt,2),a.tc,a.custno,a.smnno,ifnull(a.shipto,a.custno)," &
							  "a.pc,b.areano,b.term,b.bussname,c.bussname from saleshdrtbl a left join custmasttbl b on a.custno=b.custno " &
							  "left join custmasttbl c on a.shipto=c.custno where a.transdate <= '" & Format(CDate(dpARdate.Text), "yyyy-MM-dd") & "' " &
							  "and a.smnno = '" & admSmnNo & "' and a.status <> 'void'" ' and a.arflag is null
						ExecuteNonQuery(sql)

					Case Else
						sql = "insert into tempaging_" & lblUser.Text & "(invno,transdate,originvamt,tc,custno,smnno,shipto,pc,areano,term," &
							  "custname,shiptoname) select a.invno,a.transdate,round(a.netamt,2),a.tc,a.custno,a.smnno,ifnull(a.shipto,a.custno)," &
							  "a.pc,b.areano,b.term,b.bussname,c.bussname from saleshdrtbl a left join custmasttbl b on a.custno=b.custno " &
							  "left join custmasttbl c on a.shipto=c.custno where a.transdate <= '" & Format(CDate(dpARdate.Text), "yyyy-MM-dd") & "' " &
							  "and a.status <> 'void' and a.custno = '" & cboARcust.Text.Substring(0, 5) & "' " &
							  "and a.smnno = '" & admSmnNo & "'" ' and a.arflag is null 
						ExecuteNonQuery(sql)

				End Select
		End Select

		'2 get DM setup w/ DM # ref

		Select Case cboSmname.Text
			Case "ALL"
				Select Case cboARcust.Text
					Case "ALL"
						sql = "insert into tempaging_" & lblUser.Text & "(invno,transdate,originvamt,tc,custno,smnno,shipto,pc,areano,term," &
							  "custname,shiptoname) select a.dmcmno,a.transdate,ifnull(a.totamt,0),a.tc,a.custno,a.smnno,a.custno,a.pc," &
							  "b.areano,b.term,b.bussname,b.bussname from custdmcmhdrtbl a left join custmasttbl b on a.custno=b.custno " &
							  "where a.transdate <= '" & Format(CDate(dpARdate.Text), "yyyy-MM-dd") & "' and a.tc = '92' and a.status <> 'void'"
						ExecuteNonQuery(sql) 'new code

					Case Else
						sql = "insert into tempaging_" & lblUser.Text & "(invno,transdate,originvamt,tc,custno,smnno,shipto,pc,areano,term," &
							  "custname,shiptoname) select a.dmcmno,a.transdate,ifnull(a.totamt,0),a.tc,a.custno,a.smnno,a.custno,a.pc," &
							  "b.areano,b.term,b.bussname,b.bussname from custdmcmhdrtbl a left join custmasttbl b on a.custno=b.custno " &
							  "where a.transdate <= '" & Format(CDate(dpARdate.Text), "yyyy-MM-dd") & "' and a.tc = '92' and a.status <> 'void' " &
							  "and a.custno = '" & cboARcust.Text.Substring(0, 5) & "'"
						ExecuteNonQuery(sql)

				End Select
			Case Else
				Select Case cboARcust.Text
					Case "ALL"
						sql = "insert into tempaging_" & lblUser.Text & "(invno,transdate,originvamt,tc,custno,smnno,shipto,pc,areano,term," &
							  "custname,shiptoname) select a.dmcmno,a.transdate,ifnull(a.totamt,0),a.tc,a.custno,a.smnno,a.custno,a.pc," &
							  "b.areano,b.term,b.bussname,b.bussname from custdmcmhdrtbl a left join custmasttbl b on a.custno=b.custno " &
							  "where a.transdate <= '" & Format(CDate(dpARdate.Text), "yyyy-MM-dd") & "' and a.tc = '92' and a.status <> 'void' " &
							  "and a.smnno = '" & admSmnNo & "'"
						ExecuteNonQuery(sql) 'new code

					Case Else
						sql = "insert into tempaging_" & lblUser.Text & "(invno,transdate,originvamt,tc,custno,smnno,shipto,pc,areano,term," &
							  "custname,shiptoname) select a.dmcmno,a.transdate,ifnull(a.totamt,0),a.tc,a.custno,a.smnno,a.custno,a.pc," &
							  "b.areano,b.term,b.bussname,b.bussname from custdmcmhdrtbl a left join custmasttbl b on a.custno=b.custno " &
							  "where a.transdate <= '" & Format(CDate(dpARdate.Text), "yyyy-MM-dd") & "' and a.tc = '92' and a.status <> 'void' " &
							  "and a.custno = '" & cboARcust.Text.Substring(0, 5) & "'"
						ExecuteNonQuery(sql)

				End Select
		End Select


		'3. get payment w/ ref no
		sql = "update tempaging_" & lblUser.Text & " a,(select a.refno,a.tc,round(sum(a.oramt),2) as oramt,c.custno from coldettbl a " &
			  "left join colhdrtbl c on a.orno=c.orno where a.tc <> '60' and a.oramt <> 0 and c.status = 'open' and " &
			  "c.transdate <= '" & Format(CDate(dpARdate.Text), "yyyy-MM-dd") & "' group by a.refno) as colref " &
			  "set a.origoramt = colref.oramt where a.invno = colref.refno and a.tc = colref.tc and a.custno = colref.custno" 'and c.branch = '" & vLoggedBranch & "' 
		ExecuteNonQuery(sql)

		'4. get CM

		sql = "update tempaging_" & lblUser.Text & " a,(select a.refno,a.reftc,round(sum(a.amt),2) as cmamt,c.custno from custdmcmdettbl a " &
			  "left join custdmcmhdrtbl c on a.dmcmno=c.dmcmno where c.tc = '90' and a.amt <> 0 " &
			  "and c.status = 'posted' and c.transdate <= '" & Format(CDate(dpARdate.Text), "yyyy-MM-dd") & "' " &
			  "group by a.refno,a.reftc) as cmref set a.cmamt = cmref.cmamt " &
			  "where a.invno = cmref.refno and a.tc = cmref.reftc and a.custno = cmref.custno" 'and c.branch = '" & vLoggedBranch & "' 
		ExecuteNonQuery(sql)

		'insert CM pending approval
		'Select Case cboSmname.Text
		'	Case "ALL"
		'		Select Case cboARcust.Text
		'			Case "ALL"
		'				sql = "insert into tempaging_" & lblUser.Text & "(invno,transdate,origoramt,tc,custno,smnno,shipto,pc,areano,term," &
		'					  "custname,shiptoname,branch,docno,dsrno) select c.dmcmno,c.transdate,round(sum(a.amt),2) as amt,a.tc,c.custno,c.smnno," &
		'					  "c.custno,c.pc,b.areano,b.term,b.bussname,b.bussname,'" & vLoggedBranch & "',a.docno,a.docno " &
		'					  "from custdmcmdettbl a left join custdmcmhdrtbl c on a.dmcmno=c.dmcmno left join custmasttbl b on c.custno=b.custno " &
		'					  "where a.tc = '90' and a.amt <> 0 and c.status = 'parked' and c.transdate <= '" & Format(CDate(dpARdate.Text), "yyyy-MM-dd") & "' " &
		'					  "group by a.refno"
		'				ExecuteNonQuery(sql)

		'			Case Else
		'				sql = "insert into tempaging_" & lblUser.Text & "(invno,transdate,origoramt,tc,custno,smnno,shipto,pc,areano,term," &
		'					  "custname,shiptoname,branch,docno,dsrno) select c.dmcmno,c.transdate,round(sum(a.amt),2) as amt,a.tc,c.custno,c.smnno," &
		'					  "c.custno,c.pc,b.areano,b.term,b.bussname,b.bussname,'" & vLoggedBranch & "',a.docno,a.docno " &
		'					  "from custdmcmdettbl a left join custdmcmhdrtbl c on a.dmcmno=c.dmcmno left join custmasttbl b on c.custno=b.custno " &
		'					  "where a.tc = '90' and a.amt <> 0 and c.status = 'parked' and c.transdate <= '" & Format(CDate(dpARdate.Text), "yyyy-MM-dd") & "' " &
		'					  "and c.custno = '" & cboARcust.Text.Substring(0, 5) & "' group by a.refno"
		'				ExecuteNonQuery(sql)


		'		End Select
		'	Case Else
		'		Select Case cboARcust.Text
		'			Case "ALL"
		'				sql = "insert into tempaging_" & lblUser.Text & "(invno,transdate,origoramt,tc,custno,smnno,shipto,pc,areano,term," &
		'					  "custname,shiptoname,branch,docno,dsrno) select c.dmcmno,c.transdate,round(sum(a.amt),2) as amt,a.tc,c.custno,c.smnno," &
		'					  "c.custno,c.pc,b.areano,b.term,b.bussname,b.bussname,'" & vLoggedBranch & "',a.docno,a.docno " &
		'					  "from custdmcmdettbl a left join custdmcmhdrtbl c on a.dmcmno=c.dmcmno left join custmasttbl b on c.custno=b.custno " &
		'					  "where a.tc = '90' and a.amt <> 0 and c.status = 'parked' and c.transdate <= '" & Format(CDate(dpARdate.Text), "yyyy-MM-dd") & "' " &
		'					  "and c.smnno = '" & admSmnNo & "' group by a.refno"
		'				ExecuteNonQuery(sql)

		'			Case Else
		'				sql = "insert into tempaging_" & lblUser.Text & "(invno,transdate,origoramt,tc,custno,smnno,shipto,pc,areano,term," &
		'					  "custname,shiptoname,branch,docno,dsrno) select c.dmcmno,c.transdate,round(sum(a.amt),2) as amt,a.tc,c.custno,c.smnno," &
		'					  "c.custno,c.pc,b.areano,b.term,b.bussname,b.bussname,'" & vLoggedBranch & "',a.docno,a.docno " &
		'					  "from custdmcmdettbl a left join custdmcmhdrtbl c on a.dmcmno=c.dmcmno left join custmasttbl b on c.custno=b.custno " &
		'					  "where a.tc = '90' and a.amt <> 0 and c.status = 'parked' and c.transdate <= '" & Format(CDate(dpARdate.Text), "yyyy-MM-dd") & "' " &
		'					  "and c.custno = '" & cboARcust.Text.Substring(0, 5) & "' group by a.refno"
		'				ExecuteNonQuery(sql)


		'		End Select
		'End Select

		'5. PDC w/ ref

		sql = "update tempaging_" & lblUser.Text & " a,(select a.refno,a.tc,round(sum(a.oramt),2) as oramt,c.custno from coldettbl a " &
			  "left join colhdrtbl c on a.orno=c.orno where a.tc <> '60' and a.oramt <> 0 " &
			  "and c.status = 'open' and c.transdate > '" & Format(CDate(dpARdate.Text), "yyyy-MM-dd") & "' " &
			  "and c.smnno = '" & admSmnNo & "' group by a.refno) as colref set a.pdc = colref.oramt " &
			  "where a.invno = colref.refno and a.tc = colref.tc and a.custno = colref.custno"
		ExecuteNonQuery(sql)


		'6. deposits
		Select Case cboSmname.Text
			Case "ALL"
				Select Case cboARcust.Text
					Case "ALL"
						sql = "insert into tempaging_" & lblUser.Text & "(invno,transdate,origoramt,tc,custno,smnno,shipto,pc,areano,term," &
							  "custname,shiptoname,dsrno) select c.orno,c.transdate,round(sum(a.oramt),2) as oramt,a.tc,c.custno,c.smnno,c.custno,c.pc, " &
							  "b.areano,b.term,b.bussname,b.bussname,concat(c.doctype,c.docno) from coldettbl a left join colhdrtbl c on a.orno=c.orno " &
							  "left join custmasttbl b on c.custno=b.custno where a.tc = '60' and a.oramt <> 0 and c.status = 'open' " &
							  "and c.transdate <= '" & Format(CDate(dpARdate.Text), "yyyy-MM-dd") & "' and a.coltype = 'trade' " &
							  "group by a.refno" 'and c.branch = '" & vLoggedBranch & "' 
						ExecuteNonQuery(sql)

					Case Else
						sql = "insert into tempaging_" & lblUser.Text & "(invno,transdate,origoramt,tc,custno,smnno,shipto,pc,areano,term," &
							  "custname,shiptoname,dsrno) select c.orno,c.transdate,round(sum(a.oramt),2) as oramt,a.tc,c.custno,c.smnno,c.custno,c.pc, " &
							  "b.areano,b.term,b.bussname,b.bussname,concat(c.doctype,c.docno) from coldettbl a left join colhdrtbl c on a.orno=c.orno " &
							  "left join custmasttbl b on c.custno=b.custno where a.tc = '60' and a.oramt <> 0 and c.status = 'open' " &
							  "and c.transdate <= '" & Format(CDate(dpARdate.Text), "yyyy-MM-dd") & "' and a.coltype = 'trade' " &
							  "and c.custno = '" & cboARcust.Text.Substring(0, 5) & "' group by a.refno" 'and c.branch = '" & vLoggedBranch & "' 
						ExecuteNonQuery(sql)

				End Select

			Case Else
				Select Case cboARcust.Text
					Case "ALL"
						sql = "insert into tempaging_" & lblUser.Text & "(invno,transdate,origoramt,tc,custno,smnno,shipto,pc,areano,term," &
							  "custname,shiptoname,dsrno) select c.orno,c.transdate,round(sum(a.oramt),2) as oramt,a.tc,c.custno,c.smnno,c.custno,c.pc, " &
							  "b.areano,b.term,b.bussname,b.bussname,concat(c.doctype,c.docno) from coldettbl a left join colhdrtbl c on a.orno=c.orno " &
							  "left join custmasttbl b on c.custno=b.custno where a.tc = '60' and a.oramt <> 0 and c.status = 'open' " &
							  "and c.transdate <= '" & Format(CDate(dpARdate.Text), "yyyy-MM-dd") & "' and a.coltype = 'trade' " &
							  "and c.smnno = '" & admSmnNo & "' group by a.refno" 'and c.branch = '" & vLoggedBranch & "' 
						ExecuteNonQuery(sql)

					Case Else
						sql = "insert into tempaging_" & lblUser.Text & "(invno,transdate,origoramt,tc,custno,smnno,shipto,pc,areano,term," &
							  "custname,shiptoname,dsrno) select c.orno,c.transdate,round(sum(a.oramt),2) as oramt,a.tc,c.custno,c.smnno,c.custno,c.pc, " &
							  "b.areano,b.term,b.bussname,b.bussname,concat(c.doctype,c.docno) from coldettbl a left join colhdrtbl c on a.orno=c.orno " &
							  "left join custmasttbl b on c.custno=b.custno where a.tc = '60' and a.oramt <> 0 and c.status = 'open' " &
							  "and c.transdate <= '" & Format(CDate(dpARdate.Text), "yyyy-MM-dd") & "' and a.coltype = 'trade' " &
							  "and c.custno = '" & cboARcust.Text.Substring(0, 5) & "' group by a.refno" 'and c.branch = '" & vLoggedBranch & "' 
						ExecuteNonQuery(sql)

				End Select
		End Select

		FinalUpdateAR() 'ok

		Select Case TabContainer3.ActiveTabIndex
			Case 0
				GetAgingAllver2() '

		End Select

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

		sql = "update tempaging_" & lblUser.Text & " set days = datediff('" & Format(CDate(dpARdate.Text), "yyyy-MM-dd") & "', transdate)" '" & _  - term
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

	Private Sub SumAllARdata()


	End Sub

	Private Sub FillDgvARsumAll()
		Dim adapter As New MySqlDataAdapter
		Dim ds As New DataSet()
		'Dim i As Integer = 0
		sqldata = Nothing

		dt = GetDataTable("select sum(ifnull(invamt,0)),sum(ifnull(dep,0)),sum(ifnull(net,0)) from " &
						  "tempaging_" & lblUser.Text)
		If Not CBool(dt.Rows.Count) Then
			dgvARsum.DataSource = Nothing
			dgvARsum.DataBind()
			Exit Sub

		Else
			For Each dr As DataRow In dt.Rows
				lblSumARdr.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0.00 ; (#,##0.00)")
				lblSumARcr.Text = Format(CDbl(dr.Item(1).ToString()), "#,##0.00 ; (#,##0.00)")
				lblSumARbal.Text = Format(CDbl(dr.Item(2).ToString()), "#,##0.00 ; (#,##0.00)")
			Next

		End If

		dt.Dispose()

		sqldata = "select concat(custno,space(1),custname) as custno,concat(shipto,space(1),shiptoname) as shipto," &
				  "sum(ifnull(invamt,0)) as invamt,sum(ifnull(dep,0)) as dep,sum(ifnull(net,0)) as net from " &
				  "tempaging_" & lblUser.Text & " group by shipto order by custname"

		With dgvARsum
			.Columns(0).HeaderText = "Customer/Sold To"
			.Columns(1).HeaderText = "Branch/Ship To"
			.Columns(2).HeaderText = "DR Amt"
			.Columns(3).HeaderText = "CR Amt"
			.Columns(4).HeaderText = "Bal Amt"

		End With

		conn.Open()
		Dim command As New MySqlCommand(sqldata, conn)
		adapter.SelectCommand = command
		adapter.Fill(ds)
		adapter.Dispose()
		command.Dispose()
		conn.Close()
		dgvARsum.DataSource = ds.Tables(0)
		dgvARsum.DataBind()

	End Sub

	Private Sub FillDgvARlist()
		Dim adapter As New MySqlDataAdapter
		Dim ds As New DataSet()
		'Dim i As Integer = 0
		sqldata = Nothing

		dt = GetDataTable("select sum(ifnull(invamt,0)),sum(ifnull(dep,0)),sum(ifnull(net,0)) from " &
						  "tempaging_" & lblUser.Text)
		If Not CBool(dt.Rows.Count) Then
			dgvARsummary.DataSource = Nothing
			dgvARsummary.DataBind()
			Exit Sub

		Else
			For Each dr As DataRow In dt.Rows
				lblARdrAmt.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0.00")
				lblARcrAmt.Text = Format(CDbl(dr.Item(1).ToString()), "#,##0.00")
				lblARbalAmt.Text = Format(CDbl(dr.Item(2).ToString()), "#,##0.00 ; (#,##0.00)")
			Next

		End If

		dt.Dispose()

		sqldata = "select concat(custno,space(1),custname) as custno,concat(shipto,space(1),shiptoname) as shipto," &
				  "invno,tc,dsrno,transdate,term,datedue,ifnull(invamt,0) as invamt,ifnull(dep,0) as dep," &
				  "ifnull(net,0) as net from tempaging_" & lblUser.Text & " order by custname,shiptoname,tc,transdate"

		With dgvARsummary
			.Columns(0).HeaderText = "Customer/Sold To"
			.Columns(1).HeaderText = "Branch/Ship To"
			.Columns(2).HeaderText = "Doc No."
			.Columns(3).HeaderText = "TC"
			.Columns(4).HeaderText = "Ref No."
			.Columns(5).HeaderText = "Posting Date"
			.Columns(6).HeaderText = "Term"
			.Columns(7).HeaderText = "Due Date"
			.Columns(8).HeaderText = "DR Amt"
			.Columns(9).HeaderText = "CR Amt"
			.Columns(10).HeaderText = "Bal Amt"

		End With

		conn.Open()
		Dim command As New MySqlCommand(sqldata, conn)
		adapter.SelectCommand = command
		adapter.Fill(ds)
		adapter.Dispose()
		command.Dispose()
		conn.Close()
		dgvARsummary.DataSource = ds.Tables(0)
		dgvARsummary.DataBind()

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

	Private Sub dpARdate_TextChanged(sender As Object, e As EventArgs) Handles dpARdate.TextChanged
		If dpARdate.Text = Nothing Then
			Exit Sub

		End If

		Select Case True

			Case cboSmname.Visible
				If cboSmname.Text = "" Then
					Exit Sub
				End If
		End Select

		GetCustForAR()

	End Sub

	Private Sub GetCustForAR()
		cboARcust.Items.Clear()

		Select Case vCurrForm
			Case "Smn"
				admSmnNo = lblSmnName.Text.Substring(0, 3)
				dt = GetDataTable("Select concat(a.custno,space(1),b.bussname) from saleshdrtbl a left join custmasttbl b on " &
								  "a.custno = b.custno where a.transdate <= '" & Format(CDate(dpARdate.Text), "yyyy-MM-dd") & "' " &
								  "and a.smnno = '" & admSmnNo & "' and a.status <> 'void' group by a.custno order by b.bussname")

			Case "Mgr"
				admSmnNo = cboSmname.Text.Substring(0, 3)
				Select Case cboSmname.Text
					Case "ALL"
						dt = GetDataTable("Select concat(a.custno,space(1),b.bussname) from saleshdrtbl a left join custmasttbl b on " &
										  "a.custno = b.custno where a.transdate <= '" & Format(CDate(dpARdate.Text), "yyyy-MM-dd") & "' " &
										  "and a.status <> 'void' group by a.custno order by b.bussname")
					Case Else
						dt = GetDataTable("Select concat(a.custno,space(1),b.bussname) from saleshdrtbl a left join custmasttbl b on " &
										  "a.custno = b.custno where a.transdate <= '" & Format(CDate(dpARdate.Text), "yyyy-MM-dd") & "' " &
										   "and a.smnno = '" & admSmnNo & "' and a.status <> 'void' group by a.custno order by b.bussname")
				End Select

		End Select

		If Not CBool(dt.Rows.Count) Then
			Exit Sub
		Else
			cboARcust.Items.Add("")
			cboARcust.Items.Add("ALL")
			For Each dr As DataRow In dt.Rows
				cboARcust.Items.Add(dr.Item(0).ToString())

			Next

		End If

		dt.Dispose()

	End Sub

	Protected Sub dgvARsummary_RowDataBound(sender As Object, e As GridViewRowEventArgs)
		'For i As Integer = dgvARsummary.Rows.Count - 1 To 1 Step -1
		'	Dim row As GridViewRow = dgvARsummary.Rows(i)
		'	Dim previousRow As GridViewRow = dgvARsummary.Rows(i - 1)
		'	For j As Integer = 0 To row.Cells.Count - 1
		'		If row.Cells(j).Text = previousRow.Cells(j).Text Then
		'			If previousRow.Cells(j).RowSpan = 0 Then
		'				If row.Cells(j).RowSpan = 0 Then
		'					previousRow.Cells(j).RowSpan += 2
		'				Else
		'					previousRow.Cells(j).RowSpan = row.Cells(0).RowSpan + 1
		'				End If

		'				row.Cells(j).Visible = False

		'			End If

		'		End If

		'	Next

		'Next

	End Sub

	Protected Sub dgvARsummary_RowCreated(sender As Object, e As GridViewRowEventArgs)

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


	Protected Sub dgvSOreqDet_RowDataBound(sender As Object, e As GridViewRowEventArgs)

	End Sub

	Protected Sub dgvSOreqDet_RowCreated(sender As Object, e As GridViewRowEventArgs)

	End Sub

	Private Sub btnAdd_Click(sender As Object, e As ImageClickEventArgs) Handles btnAdd.Click
		lblErrMsg.Text = lblUser.Text
		AddNewLineItem()
		FillDgvSOdetSO()
		cboPClass.Enabled = False
		lbSave.Enabled = True

	End Sub

	Private Sub FillDgvSOdetSO()
		Dim adapter As New MySqlDataAdapter

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
		dgvSOreqDet.DataSource = ds.Tables(0)
		dgvSOreqDet.DataBind()

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

	Private Sub SalesmanModule_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
		GetPClass()
	End Sub

	Protected Sub ClrErrMsg()
		lblErrMsg.Text = "No Error"
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
	Protected Sub PopPClass()
		ClrErrMsg()
		dt = GetDataTable("select concat(pc,space(1),pclass) from pctrtbl where tradetype = 'trade'")
		If Not CBool(dt.Rows.Count) Then
			lblErrMsg.Text = "PC Not found."
			Exit Sub
		Else
			cboPClass.Items.Add("")
			For Each dr As DataRow In dt.Rows
				cboPClass.Items.Add(dr.Item(0).ToString())

			Next

		End If

		Call dt.Dispose()

	End Sub

	Private Sub cboCustName_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboCustName.SelectedIndexChanged

		CreateNewTable()
		ClrErrMsg()

		If dpTransDate.Text = Nothing Then
			dpTransDate.Text = Format(CDate(Now()), "yyyy-MM-dd")
			dpDelDate.Text = Format(CDate(Now()), "yyyy-MM-dd")
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

				lblCurrCustNo.Text = cboCustName.Text.Substring(0, 5)

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
						'Call ChkNewBals()
						Call getSetUpAllVer2()
						Call GetSOstatnew()
					End If

					Call GetMMastCust()

					cboShipTo.Focus()

				End If

		End Select

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
			  "custname,shiptoname) select a.invno,a.transdate,round(a.netamt,2),a.tc,a.custno,a.smnno,ifnull(a.shipto,a.custno)," &
			  "a.pc,b.areano,b.term,b.bussname,c.bussname from saleshdrtbl a left join custmasttbl b on a.custno=b.custno " &
			  "left join custmasttbl c on a.shipto=c.custno where a.transdate <= '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
			  "and a.status <> 'void' and a.arflag is null and a.custno = '" & lblCurrCustNo.Text & "'"
		ExecuteNonQuery(sql)

		'2 get DM setup w/ DM # ref
		sql = "insert into tempaging_" & lblUser.Text & "(invno,transdate,originvamt,tc,custno,smnno,shipto,pc,areano,term," &
			  "custname,shiptoname) select a.dmcmno,a.transdate,ifnull(a.totamt,0),a.tc,a.custno,a.smnno,a.custno,a.pc," &
			  "b.areano,b.term,b.bussname,b.bussname from custdmcmhdrtbl a left join custmasttbl b on a.custno=b.custno " &
			  "where a.transdate <= '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and a.tc = '92' and a.status <> 'void' and " &
			  "a.custno = '" & lblCurrCustNo.Text & "'"
		ExecuteNonQuery(sql) 'new code

		'3. get payment w/ ref no
		sql = "update tempaging_" & lblUser.Text & " a,(select a.refno,a.tc,round(sum(a.oramt),2) as oramt,c.custno from coldettbl a " &
			  "left join colhdrtbl c on a.orno=c.orno where a.tc <> '60' and a.oramt <> 0 and c.status <> 'void' and " &
			  "c.transdate <= '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and c.custno = '" & lblCurrCustNo.Text & "' group by a.refno) as colref " &
			  "set a.origoramt = colref.oramt where a.invno = colref.refno and a.tc = colref.tc and a.custno = colref.custno" 'and c.branch = '" & vLoggedBranch & "' 
		ExecuteNonQuery(sql)

		'4. get CM
		sql = "update tempaging_" & lblUser.Text & " a,(select a.refno,a.reftc,round(sum(a.amt),2) as cmamt,c.custno from custdmcmdettbl a " &
			  "left join custdmcmhdrtbl c on a.dmcmno=c.dmcmno where c.tc = '90' and a.amt <> 0 and c.custno = '" & lblCurrCustNo.Text & "'" &
			  "and c.status = 'posted' and c.transdate <= '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
			  "group by a.refno,a.reftc) as cmref set a.cmamt = cmref.cmamt " &
			  "where a.invno = cmref.refno and a.tc = cmref.reftc and a.custno = cmref.custno" 'and c.branch = '" & vLoggedBranch & "' 
		ExecuteNonQuery(sql)

		'5. PDC w/ ref
		sql = "update tempaging_" & lblUser.Text & " a,(select a.refno,a.tc,round(sum(a.oramt),2) as oramt,c.custno from coldettbl a " &
			  "left join colhdrtbl c on a.orno=c.orno where a.tc <> '60' and a.oramt <> 0 and c.custno = '" & lblCurrCustNo.Text & "' " &
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
			  "c.custno = '" & lblCurrCustNo.Text & "' group by a.refno"
		ExecuteNonQuery(sql)

		'7. deposits PDC
		sql = "insert into tempaging_" & lblUser.Text & "(invno,transdate,origoramt,tc,custno,smnno,shipto,pc,areano,term," &
			  "custname,shiptoname) select c.orno,c.transdate,round(sum(a.oramt),2) as oramt,a.tc,c.custno,c.smnno,c.custno,c.pc, " &
			  "b.areano,b.term,b.bussname,b.bussname from coldettbl a left join colhdrtbl c on a.orno=c.orno " &
			  "left join custmasttbl b on c.custno=b.custno where a.tc = '60' and a.oramt <> 0 and c.status <> 'void' and " &
			  "c.transdate > '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and a.coltype = 'trade' and " &
			  "c.custno = '" & lblCurrCustNo.Text & "' group by a.refno"
		ExecuteNonQuery(sql)

		'FinalUpdateAR() 'ok

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

		sql = "update tempaging_" & lblUser.Text & " set days = datediff('" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "', transdate)" '" & _  - term
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
						  "c.custno = '" & lblCurrCustNo.Text & "' group by c.custno")
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
						  "and c.custno = '" & lblCurrCustNo.Text & "' group by c.custno")
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

	End Sub

	Private Sub ChkNewBals()
		Try
			'Call getSetUpAllVer2()

			'====1 to 30 ===
			'dt = GetDataTable("select sum(ifnull(totalar,0)) from tempaging_" & lblUser.Text & " where days between 1 and 30 and tc <> '60' group by custno")
			'If Not CBool(dt.Rows.Count) Then
			'	lbl30days.Text = "0.00"

			'Else
			'	For Each dr As DataRow In dt.Rows
			'		lbl30days.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0.00 ; (#,##0.00)")
			'	Next

			'	Call dt.Dispose()

			'End If

			''====31 to 60 ===
			'dt = GetDataTable("select sum(ifnull(totalar,0)) from tempaging_" & lblUser.Text & " where days between 31 and 60 and tc <> '60' group by custno")
			'If Not CBool(dt.Rows.Count) Then
			'	lbl60days.Text = "0.00"

			'Else
			'	For Each dr As DataRow In dt.Rows
			'		lbl60days.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0.00 ; (#,##0.00)")
			'	Next

			'	Call dt.Dispose()

			'End If

			''====61 to 90 ===
			'dt = GetDataTable("select sum(ifnull(totalar,0)) from tempaging_" & lblUser.Text & " where days between 61 and 90 and tc <> '60' group by custno")
			'If Not CBool(dt.Rows.Count) Then
			'	lbl90days.Text = "0.00"

			'Else
			'	For Each dr As DataRow In dt.Rows
			'		lbl90days.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0.00 ; (#,##0.00)")
			'	Next

			'	Call dt.Dispose()

			'End If


			''====91-120 ===
			'dt = GetDataTable("select sum(ifnull(totalar,0)) from tempaging_" & lblUser.Text & " where days between 91 and 120 and tc <> '60' group by custno")
			'If Not CBool(dt.Rows.Count) Then
			'	lbl91over.Text = "0.00"

			'Else
			'	For Each dr As DataRow In dt.Rows
			'		lbl91over.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0.00 ; (#,##0.00)")
			'	Next

			'	Call dt.Dispose()

			'End If

			''====121 days over===

			'dt = GetDataTable("select sum(ifnull(totalar,0)) from tempaging_" & lblUser.Text & " where days > 121 and tc <> '60' group by custno")
			'If Not CBool(dt.Rows.Count) Then
			'	lbl121over.Text = "0.00"

			'Else
			'	For Each dr As DataRow In dt.Rows
			'		lbl121over.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0.00 ; (#,##0.00)")

			'	Next

			'	Call dt.Dispose()

			'End If

			''===add over due acct
			'Dim overdue As Double = CDbl(IIf(lbl30days.Text = "", 0, lbl30days.Text)) + CDbl(IIf(lbl60days.Text = "", 0, lbl60days.Text)) +
			'						CDbl(IIf(lbl90days.Text = "", 0, lbl90days.Text)) + CDbl(IIf(lbl91over.Text = "", 0, lbl91over.Text)) +
			'						CDbl(IIf(lbl121over.Text = "", 0, lbl121over.Text))


			''lblOverDue.Text = Format(overdue, "#,##0.00 ; (#,##0.00)")

			'''===get deposits
			'dt = GetDataTable("select round(sum(a.oramt),2) as oramt from coldettbl a left join colhdrtbl c on a.orno=c.orno " &
			'				  "where a.tc = '60' and c.status <> 'void' and a.coltype = 'trade' " &
			'				  "and c.transdate <= '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
			'				  "c.custno = '" & lblCurrCustNo.text & "' group by c.custno")
			'If Not CBool(dt.Rows.Count) Then
			'	lblDepAmt.Text = "0.00"
			'Else
			'	For Each dr As DataRow In dt.Rows
			'		lblDepAmt.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0.00 ; (#,##0.00)")
			'	Next

			'	Call dt.Dispose()

			'End If

			''====get PDC
			'dt = GetDataTable("select round(sum(a.oramt),2) as oramt from coldettbl a left join colhdrtbl c on a.orno=c.orno " &
			'				  "where c.status <> 'void' and a.coltype = 'trade' and c.transdate > '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "'" &
			'				  "and c.custno = '" & lblCurrCustNo.text & "' group by c.custno")
			'If Not CBool(dt.Rows.Count) Then
			'	lblPDC.Text = "0.00"
			'Else
			'	For Each dr As DataRow In dt.Rows
			'		lblPDC.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0.00 ; (#,##0.00)")
			'	Next

			'	Call dt.Dispose()

			'End If

			''total A/R
			'dt = GetDataTable("select sum(ifnull(totalar,0)) from tempaging_" & lblUser.Text & " group by custno")
			'If Not CBool(dt.Rows.Count) Then
			'	lblAcctBal.Text = "0.00"

			'Else
			'	For Each dr As DataRow In dt.Rows
			'		lblAcctBal.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0.00 ; (#,##0.00)")

			'	Next

			'	Call dt.Dispose()

			'End If

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

	Protected Sub GetCustListALL()
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

	Private Sub GetCustListSmn()
		ClrErrMsg()
		cboCustName.Items.Clear()
		dt = GetDataTable("select concat(custno,space(1),bussname )from custmasttbl where smnno = '" & lblSmnNo.Text.Substring(0, 3) & "' and " &
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

	Private Sub CreateNewTable()
		dt = GetDataTable("select * from information_schema.tables where " &
						  "table_name = 'reqsohdrtbl'")
		If Not CBool(dt.Rows.Count) Then
			sql = "CREATE TABLE reqsohdrtbl LIKE sohdrtbl"
			ExecuteNonQuery(sql)

		End If

		dt.Dispose()

		dt = GetDataTable("select * from information_schema.tables where " &
						  "table_name = 'reqsodettbl'")
		If Not CBool(dt.Rows.Count) Then
			sql = "CREATE TABLE reqsodettbl LIKE sodettbl"
			ExecuteNonQuery(sql)

		End If

		dt.Dispose()

	End Sub

	Private Sub CboShipTo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboShipTo.SelectedIndexChanged
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

	End Sub

	Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
		Select Case vLoggedBussArea
			Case "8300"
				If cboPClass.Text = "" Then
					AdmMsgBox("Select Product Class")
					Exit Sub
				End If
		End Select

		GetMMastSO()

	End Sub

	Protected Sub GetMMastSO()
		ClrErrMsg()
		cboMMdesc.Items.Clear()
		Select Case vLoggedBussArea
			Case "8300"
				dt = GetDataTable("select ifnull(codename,mmdesc),mmtype from mmasttbl where trade = 'Y' and " &
								  "pc = '" & cboPClass.Text.Substring(0, 1) & "' " &
								  "and status = 'In Used' order by ifnull(codename,mmdesc)")

			Case Else
				dt = GetDataTable("select ifnull(codename,mmdesc),mmtype from mmasttbl where trade = 'Y' and " &
								  "status = 'In Used' order by ifnull(codename,mmdesc)")
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

	Private Sub GetMMastAvailSO()
		ClrErrMsg()
		'get inventory
		CreateNewInvBeg()

		cboMMdesc.Items.Clear()

		dt = GetDataTable("select ifnull(b.codename,b.mmdesc) from tempinvbalsnew_" & lblUser.Text & " a " &
						  "left join mmasttbl b on a.codeno = b.codeno where b.pc = '" & cboPClass.Text.Substring(0, 1) & "' " &
						  "and b.status = 'In Used' group by a.codeno order by ifnull(b.codename,b.mmdesc)")
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

	Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
		Select Case vLoggedBussArea
			Case "8300"
				If cboPClass.Text = "" Then
					AdmMsgBox("Select Product Class")
					Exit Sub
				End If
		End Select

		If dpTransDate.Text = Nothing Then
			AdmMsgBox("Please Select Date")
			Exit Sub
		End If

		GetMMastAvailSO()

	End Sub

	Private Sub CreateNewInvBeg()
		sql = "delete from tempinvbals where user = '" & lblUser.Text & "'"
		ExecuteNonQuery(sql)
		'mmrr
		sql = "insert into tempinvbals(codeno,lotno,user,mmdesc) select a.codeno,a.lotno,'" & lblUser.Text & "',c.mmdesc " &
			  "from invdettbl a left join invhdrtbl b on a.mmrrno=b.mmrrno left join mmasttbl c on a.codeno=c.codeno where " &
			  "b.status <> 'void' and b.transdate <= '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
			  "c.trade = 'Y' group by a.codeno,a.lotno"
		ExecuteNonQuery(sql)
		'do
		sql = "insert into tempinvbals(codeno,lotno,user,mmdesc) select a.codeno,a.lotno,'" & lblUser.Text & "',c.mmdesc " &
			  "from issdettbl a left join isshdrtbl b on a.dono=b.dono left join mmasttbl c on a.codeno=c.codeno where " &
			  "b.status <> 'void' and b.transdate <= '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
			  "c.trade = 'Y' group by a.codeno,a.lotno"
		ExecuteNonQuery(sql)
		'wrr
		sql = "insert into tempinvbals(codeno,lotno,user,mmdesc) select a.codeno,a.lotno,'" & lblUser.Text & "',c.mmdesc " &
			  "from wrrdettbl a left join wrrhdrtbl b on a.wrrno=b.wrrno left join mmasttbl c on a.codeno=c.codeno where " &
			  "b.status <> 'void' and b.transdate <= '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
			  "c.trade = 'Y' group by a.codeno,a.lotno"
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

		sql = "insert into tempinvbalsnew_" & lblUser.Text & "(codeno,mmdesc,lotno,user) select codeno,mmdesc,lotno,user " &
			  "from tempinvbals where user = '" & lblUser.Text & "' group by codeno,lotno"
		ExecuteNonQuery(sql)

		'prodn
		sql = "update tempinvbalsnew_" & lblUser.Text & " a,(select a.codeno,sum(ifnull(a.qty,0)) as qty,sum(ifnull(a.wt,0)) as wt,a.lotno " &
			  "from invdettbl a left join invhdrtbl b on a.mmrrno=b.mmrrno where b.mov = '801' and b.status <> 'void' and " &
			  "b.transdate < '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' group by a.codeno,a.lotno) as b set " &
			  "a.prodqtybeg = b.qty,a.prodwtbeg = b.wt where a.user = '" & lblUser.Text & "' and " &
			  "a.codeno = b.codeno and a.lotno=b.lotno"
		ExecuteNonQuery(sql)

		'mmrr
		sql = "update tempinvbalsnew_" & lblUser.Text & " a,(select a.codeno,sum(ifnull(a.qty,0)) as qty,sum(ifnull(a.wt,0)) as wt,a.lotno " &
			  "from invdettbl a left join invhdrtbl b on a.mmrrno=b.mmrrno where b.tc = '10' and (b.mov <> '701' and b.mov <> '511') and " &
			  "b.status <> 'void' and b.transdate < '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
			  "group by a.codeno,a.lotno) as b set a.recqtybeg = b.qty,a.recwtbeg = b.wt where a.user = '" & lblUser.Text & "' and " &
			  "a.codeno = b.codeno and a.lotno=b.lotno"
		ExecuteNonQuery(sql)

		'mdiMain.tsPbar.Value = 15

		'return
		sql = "update tempinvbalsnew_" & lblUser.Text & " a,(select a.codeno,sum(ifnull(a.qty,0)) as qty,sum(ifnull(a.wt,0)) as wt,a.lotno from " &
			  "wrrdettbl a left join wrrhdrtbl b on a.wrrno=b.wrrno where b.status <> 'void' and " &
			  "b.transdate < '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' group by a.codeno,a.lotno) as b set " &
			  "a.wrrqtybeg = b.qty,a.wrrwtbeg = b.wt where a.user = '" & lblUser.Text & "' and " &
			  "a.codeno = b.codeno and a.lotno=b.lotno"
		ExecuteNonQuery(sql)

		'mdiMain.tsPbar.Value = 20

		'issuance
		sql = "update tempinvbalsnew_" & lblUser.Text & " a,(select a.codeno,sum(ifnull(a.qty,0)) as qty,sum(ifnull(a.wt,0)) as wt,a.lotno from " &
			  "issdettbl a left join isshdrtbl b on a.dono=b.dono where b.tc = '30' and b.mov <> '701' " &
			  "and b.status <> 'void' and b.transdate < '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' group by a.codeno,a.lotno) as b set " &
			  "a.issqtybeg = b.qty,a.isswtbeg = b.wt where a.user = '" & lblUser.Text & "' and a.codeno = b.codeno and a.lotno=b.lotno"
		ExecuteNonQuery(sql)

		'mdiMain.tsPbar.Value = 25

		'adjustment in
		sql = "update tempinvbalsnew_" & lblUser.Text & " a,(select a.codeno,sum(ifnull(a.qty,0)) as qty,sum(ifnull(a.wt,0)) as wt,a.lotno from " &
			  "invdettbl a left join invhdrtbl b on a.mmrrno=b.mmrrno where b.tc = '10' and b.mov = '701' " &
			  "and b.status <> 'void' and b.transdate < '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' group by a.codeno,a.lotno) as b set " &
			  "a.inadjtqtybeg = b.qty,a.inadjtwtbeg = b.wt where a.user = '" & lblUser.Text & "' and a.codeno = b.codeno and a.lotno=b.lotno"
		ExecuteNonQuery(sql)

		'mdiMain.tsPbar.Value = 50

		'adjustment out
		sql = "update tempinvbalsnew_" & lblUser.Text & " a,(select a.codeno,sum(ifnull(a.qty,0)) as qty,sum(ifnull(a.wt,0)) as wt,a.lotno from " &
			  "issdettbl a left join isshdrtbl b on a.dono=b.dono where b.tc = '30' and b.mov = '701' " &
			  "and b.status <> 'void' and b.transdate < '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' group by a.codeno,a.lotno) as b set " &
			  "a.outadjtqtybeg = b.qty,a.outadjtwtbeg = b.wt where a.user = '" & lblUser.Text & "' and a.codeno = b.codeno and a.lotno=b.lotno"
		ExecuteNonQuery(sql)

		'mdiMain.tsPbar.Value = 70

		'initial stock
		sql = "update tempinvbalsnew_" & lblUser.Text & " a,(select a.codeno,sum(ifnull(a.qty,0)) as qty,sum(ifnull(a.wt,0)) as wt,a.lotno from " &
			  "invdettbl a left join invhdrtbl b on a.mmrrno=b.mmrrno where b.tc = '10' and b.mov = '511' " &
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

	Private Sub cboMMdesc_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboMMdesc.SelectedIndexChanged
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

	Protected Sub MMdescProc()
		ClrErrMsg()
		If cboCustName.Text = "" Then
			lblErrMsg.Text = "Select Customer First"
			Exit Sub
		End If

		dt = GetDataTable("select codeno,billref,qtpk,ifnull(wtfr,0),precodeno from mmasttbl where " &
						  "ifnull(codename,mmdesc) = '" & cboMMdesc.Text & "'")
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

	Protected Sub GetSP()
		ClrErrMsg()
		If CheckBox2.Checked = True Then
			getLastPrice()

		Else
			Select Case lblSPtype.Text
				Case "Area"
					dt = GetDataTable("select currprc from plsttbl where codeno = '" & txtCodeNo.Text & "' and " &
									  "areano = '" & lblArea.Text & "' and '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
									  "between effdate and prvdate")
					If Not CBool(dt.Rows.Count) Then
						getLastPrice()
					Else
						For Each dr As DataRow In dt.Rows
							SP = dr.Item(0).ToString()
							txtSP.Text = Format(SP, "#,##0.0000")

						Next

					End If

					Call dt.Dispose()

				Case "Customer"
					dt = GetDataTable("select currprc,ifnull(drate,0) from plsttbl where codeno = '" & txtCodeNo.Text & "' and " &
									  "custno = '" & lblCurrCustNo.Text & "' and '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
									  "between effdate and prvdate")
					If Not CBool(dt.Rows.Count) Then
						getLastPrice()

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


		End If

		txtDiscAmt.Focus()


		'Catch ex As Exception

		'End Try
	End Sub

	Private Sub txtQty_TextChanged(sender As Object, e As EventArgs) Handles txtQty.TextChanged
		QtyProc()
		SpProc()
		txtWt.Focus()

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

	Protected Sub OnConfirm(sender As Object, e As EventArgs)
		Select Case TabContainer1.ActiveTabIndex
			Case 4
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
						dgvSOdet.DataSource = Nothing
						dgvSOdet.DataBind()
						ds.Clear()

						strReport = "Reset Button"
						'Call SaveLogs()
						txtItm.Text = "1"
						lblLineItm.Text = "New"

					Else
						lblErrMsg.Text = "Action Aborted"

					End If
				End If
		End Select

	End Sub

	Protected Sub OnConfirm2(sender As Object, e As EventArgs)
		Dim confirmValue As String = Request.Form("confirm_value")

		Select Case TabContainer1.ActiveTabIndex
			Case 4
				If confirmValue = "Yes" Then
					RemoveLineItem()
				Else
					lblErrMsg.Text = "Delete Cancelled"
				End If
		End Select


	End Sub

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
			FillDgvSOdet()
		Else
			sql = "delete from reqsodettbl where itmno = " & CLng(txtItm.Text) & " and sono = '" & lblUser.Text & "'"
			ExecuteNonQuery(sql)

			sql = "delete from tempsodettbl where itmno = " & CLng(txtItm.Text) & " and user = '" & lblUser.Text & "'"
			ExecuteNonQuery(sql)

			FillDgvSOdet()
			'UpdateSOHdr()
			ClrLines()

		End If



	End Sub

	Protected Sub NewLoadProc()
		tssDocNo.Text = Nothing
		vThisFormCode = "017"
		'Call CheckGroupRights()
		txtSOno.ReadOnly = False

		tssDocStat.Text = "New"
		'MsgBox(vLoggedBussArea)

		strReport = "Load Form"
		'Call SaveLogs()

		If Not Me.IsPostBack Then
			Call GetCustList()
			Call GetSmnList()
			Call GetPClass()
		End If

		'cboPlnt.Items.Clear()
		'dt = GetDataTable("select concat(plntno,space(1),plntname) from plnttbl where status = 'active' and invset = 'On'")
		'If Not CBool(dt.Rows.Count) Then
		'	lblErrMsg.Text = "Not found."
		'	Exit Sub
		'Else
		'	For Each dr As DataRow In dt.Rows
		'		cboPlnt.Items.Add(dr.Item(0).ToString())

		'	Next
		'End If

		'Call dt.Dispose()

		txtSOno.MaxLength = 8

		'Select Case vLoggedBussArea
		'	Case "8100"
		'		txtSOno.MaxLength = 8

		'	Case Else
		'		txtSOno.MaxLength = 8

		'End Select

		dpTransDate.Text = Format(CDate(Now()), "yyyy-MM-dd")

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

	Protected Sub lbSave_Click(sender As Object, e As EventArgs)
		ClrErrMsg()
		Select Case TabContainer1.ActiveTabIndex
			Case 4
				lblMsg.Text = "SO Request"
				txtSOno.ReadOnly = False
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

				ElseIf lblSmnNo.Text = "" Then
					lblErrMsg.Text = "Salesman No is Blank"
					Exit Sub

				ElseIf dgvSOreqDet.Rows.Count = 0 Then
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

				'lblSmnNo
				If Len(txtSOno.Text) <> 8 Then
					lblErrMsg.Text = "SO No. should be 8 Digits"
					Exit Sub
				End If

				'Call chkNewBals()
				'gRepTbox(txtRemarks)
				If txtSOno.Text = "" Then
					lblMsg.Text = "Req No. is Blank"
					Exit Sub
				End If

				Call SaveHdrProc()
				'ToolStripMenuItem3.Enabled = True

				'mdiMain.tslblLastDoc2.Text = "SO No.: " & txtSOno.Text

				'gRepTboxUndo(txtRemarks)
				'lbPrint.Enabled = True
				txtSOno.ReadOnly = True

		End Select

	End Sub

	Private Sub GetSONo()
		ClrErrMsg()

		Dim admStart As String = Format(CDate(Now()), "yy") & "000001"
		Dim admEnd As String = Format(CDate(Now()), "yy") & "999999"

		dt = GetDataTable("select ifnull(sono,0) from reqsohdrtbl where sono between '" & admStart & "' " &
						  "and '" & admEnd & "' order by sono")
		If Not CBool(dt.Rows.Count) Then
			txtSOno.Text = admStart

		Else
			For Each dr As DataRow In dt.Rows
				Dim SONo As Long = CLng(dr.Item(0).ToString()) + 1
				txtSOno.Text = Format(SONo, "#00000000")

			Next

		End If

		Call dt.Dispose()

	End Sub

	Private Sub SaveHdrProc()
		ClrErrMsg()
		dt = GetDataTable("select delstat from reqsohdrtbl where sono = '" & txtSOno.Text & "'")
		If Not CBool(dt.Rows.Count) Then
			Call SaveSOHdr()

			'AdmMsgBox("SO No. " & txtSOno.Text & " saved")
			strReport = "save so request"
			'Call SaveLogs()

		Else
			For Each dr As DataRow In dt.Rows
				Select Case UCase(dr.Item(0).ToString())
					Case "SERVED"
						'AdmMsgBox("SO No.: " & txtSOno.Text & " Already SERVED, channges not SAVED")
						strReport = "Update SO, Not SAVED"
						'Call SaveLogs()
						Exit Sub

				End Select
			Next

			UpdateSOHdr()

			'AdmMsgBox("SO No. " & txtSOno.Text & " Update SAVED")
			strReport = "Update SO req"
			'Call SaveLogs()

		End If

		Call dt.Dispose()

		Call SaveDetProc()

		'mdiMain.tslblLastDoc2.Text = "SO No.:" & txtSOno.Text

	End Sub

	Private Sub SaveSOHdr()
		sql = "insert into reqsohdrtbl(sono,transdate,custno,shipto,smnno,pono,delstat,user,30days,60days,90days,120days," &
			  "soamt,status,totar,deldate,apprvdby,remarks,91over,deposit,pdc,pdate)values" &
			  "('" & txtSOno.Text & "','" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "'," &
			  "'" & cboCustName.Text.Substring(0, 5) & "','" & cboShipTo.Text.Substring(0, 5) & "','" & lblSmnNo.Text.Substring(0, 3) & "'," &
			  "'" & txtPONo.Text & "','OPEN','" & lblUser.Text & "'," & CDbl(IIf(lbl30days.Text = "", 0, lbl30days.Text)) & "," &
			  CDbl(IIf(lbl60days.Text = "", 0, lbl60days.Text)) & "," & CDbl(IIf(lbl90days.Text = "", 0, lbl90days.Text)) & "," &
			  CDbl(IIf(lbl91over.Text = "", 0, lbl91over.Text)) & "," & CDbl(IIf(lblTotNetAmt.Text = "", 0, lblTotNetAmt.Text)) & "," &
			  "'" & lblSOStat.Text & "'," & CDbl(IIf(lblAcctBal.Text = "", 0, lblAcctBal.Text)) & "," &
			  "'" & Format(CDate(dpDelDate.Text), "yyyy-MM-dd") & "',null,'" & txtRemarks.Text & "'," &
			  CDbl(IIf(lbl121over.Text = "", 0, lbl121over.Text)) & "," &
			  CDbl(IIf(lblDepAmt.Text = "", 0, lblDepAmt.Text)) & "," & CDbl(IIf(lblDepAmt.Text = "", 0, lblDepAmt.Text)) & "," &
			  "'" & Format(CDate(Now()), "yyyy-MM-dd HH:mm:ss") & "')"
		ExecuteNonQuery(sql)

		'vLastAct = Me.Text & " SAVE SO No. " & txtSOno.Text & Space(1) & txtCustNo.Text & Space(1) & txtSmnNo.Text & Space(1) & txtShipTo.Text
		'WriteToLogs(vLastAct)

	End Sub

	Private Sub UpdateSOHdr()
		sql = "update reqsohdrtbl set transdate='" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "'," &
			  "custno='" & cboCustName.Text.Substring(0, 5) & "',shipto='" & cboShipTo.Text.Substring(0, 5) & "'," &
			  "smnno='" & lblSmnNo.Text.Substring(0, 3) & "',pono='" & txtPONo.Text & "',delstat='OPEN'," &
			  "user='" & lblUser.Text & "',30days=" & CDbl(IIf(lbl30days.Text = "", 0, lbl30days.Text)) & "," &
			  "60days=" & CDbl(IIf(lbl60days.Text = "", 0, lbl60days.Text)) & ",90days= " & CDbl(IIf(lbl90days.Text = "", 0, lbl90days.Text)) & "," &
			  "120days=" & CDbl(IIf(lbl91over.Text = "", 0, lbl91over.Text)) & "," &
			  "soamt=" & CDbl(IIf(lblTotNetAmt.Text = "", 0, lblTotNetAmt.Text)) & ",status='" & lblSOStat.Text & "'," &
			  "totar=" & CDbl(IIf(lblAcctBal.Text = "", 0, lblAcctBal.Text)) & ",deldate='" & Format(CDate(dpDelDate.Text), "yyyy-MM-dd") & "'," &
			  "apprvdby=null,remarks='" & txtRemarks.Text & "',91over = " & CDbl(IIf(lbl121over.Text = "", 0, lbl121over.Text)) & "," &
			  "deposit = " & CDbl(IIf(lblDepAmt.Text = "", 0, lblDepAmt.Text)) & "," &
			  "pdc = " & CDbl(IIf(lblDepAmt.Text = "", 0, lblDepAmt.Text)) & ",pdate = '" & Format(CDate(Now()), "yyyy-MM-dd HH:mm:ss") & "' " &
			  "where sono='" & txtSOno.Text & "'"
		ExecuteNonQuery(sql)

		'vLastAct = Me.Text & " UPDATE SO No. " & txtSOno.Text & Space(1) & txtCustNo.Text & Space(1) & txtSmnNo.Text & Space(1) & txtShipTo.Text
		'WriteToLogs(vLastAct)

	End Sub

	Private Sub SaveDetProc()
		ClrErrMsg()
		dt = GetDataTable("select * from reqsodettbl where sono = '" & txtSOno.Text & "'")
		If Not CBool(dt.Rows.Count) Then
			Call SaveSODet()

			'strReport = "save item no.:" & txtItm.Text
			'Call SaveLogs()

		Else
			sql = "delete from reqsodettbl where sono = '" & txtSOno.Text & "'"
			ExecuteNonQuery(sql)

			Call SaveSODet()

			'strReport = "Update Item No.:" & txtItm.Text
			'Call SaveLogs()

		End If

		txtSOno.ReadOnly = False

		Call dt.Dispose()

	End Sub

	Private Sub SaveSODet()
		sql = "insert into reqsodettbl(sono,codeno,qty,wt,sp,itmamt,itmno,um,qtbal,wtbal,prodreq,discamt,netamt,status,sloc) " &
			  "select '" & txtSOno.Text & "',codeno,qty,wt,sp,itmamt,itmno,um,qtbal,wtbal,prodreq,discamt,netamt,status,sloc " &
			  "from tempsodettbl where user = '" & lblUser.Text & "'"
		ExecuteNonQuery(sql)
	End Sub

	Private Sub txtSOno_TextChanged(sender As Object, e As EventArgs) Handles txtSOno.TextChanged
		If txtSOno.Text = Nothing Then
			Exit Sub
		End If

		ExecBtnProc()

	End Sub

	Private Sub ExecBtnProc()
		If txtSOno.Text <> "" Then
			dgvSOreqDet.DataSource = Nothing
			dgvSOreqDet.DataBind()
			ds.Clear()

			Call ClrHdrDetails()

			dt = GetDataTable("select delstat from reqsohdrtbl where sono = '" & txtSOno.Text & "'")
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
							'CheckGroupRights()

						Case Else
							lbSave.Enabled = False
							lbDelete.Enabled = False
							btnDel.Enabled = False
							lblErrMsg.Text = "SO No. " & txtSOno.Text & " cannot be edited due to Status: " & strDelStat

					End Select

					Call reLoadSo()

				Next

			End If

			dt.Dispose()

		End If

		FillDgvSOdetSO()

		If CheckBox1.Checked = True Then

		End If
		'Call remItmReload()

	End Sub

	Private Sub ReLoadSo()
		Try
			dt = GetDataTable("select ifnull(sysver,'2.1041077') from reqsohdrtbl where sono = '" & txtSOno.Text & "'")
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
							  "concat(a.plntno,space(1),e.plntname),concat(a.pc,space(1),f.pclass),a.deldate,b.custtype from reqsohdrtbl a " &
							  "left join custmasttbl b on a.custno=b.custno left join custmasttbl c on a.shipto=c.custno " &
							  "left join smnmtbl d on a.smnno=d.smnno left join plnttbl e on a.plntno=e.plntno " &
							  "left join pctrtbl f on a.pc=f.pc where a.sono = '" & txtSOno.Text & "'")
			If Not CBool(dt.Rows.Count) Then
				lblErrMsg.Text = "Request for SO Not found."
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
					'cboPlnt.Text = dr.Item(20).ToString() & ""
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
				  "from reqsodettbl where sono = '" & txtSOno.Text & "'"
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

	Protected Sub dgvSOreqList_RowDataBound(sender As Object, e As GridViewRowEventArgs)

	End Sub

	Protected Sub dgvSOreqList_RowCreated(sender As Object, e As GridViewRowEventArgs)

	End Sub

	Protected Sub dgvARsum_RowDataBound(sender As Object, e As GridViewRowEventArgs)

	End Sub

	Protected Sub dgvARsum_RowCreated(sender As Object, e As GridViewRowEventArgs)

	End Sub

	Private Sub dpSOreqFr_TextChanged(sender As Object, e As EventArgs) Handles dpSOreqFr.TextChanged, dpSOreqTo.TextChanged
		If dpSOreqFr.Text = Nothing Then
			Exit Sub
		ElseIf dpSOreqTo.Text = Nothing Then
			Exit Sub
		End If

		If Format(CDate(dpSOreqFr.Text), "yyyy-MM-dd") > Format(CDate(dpSOreqTo.Text), "yyyy-MM-dd") Then
			Exit Sub

		End If


	End Sub

	'Private Sub Pop()

	Private Sub cboARcust_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboARcust.SelectedIndexChanged
		ARproc()

	End Sub

	Private Sub cboSOreqStat_TextChanged(sender As Object, e As EventArgs) Handles cboSOreqStat.TextChanged
		If cboSOreqStat.Text = "" Then
			Exit Sub

		End If


	End Sub

	Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged

	End Sub

	Protected Sub ExportToExcelAR(ByVal sender As Object, ByVal e As EventArgs)

		Dim admGrid As New GridView

		Response.ClearContent()

		Select Case TabContainer3.ActiveTabIndex
			Case 0
				If dgvARreport.Rows.Count = 0 Then
					lblErrMsg.Text = "No A/R Aging Listed"
					Exit Sub
				End If

				admGrid = dgvARreport
				Response.AddHeader("content-disposition", "attachment; filename=" & cboARcust.Text & "_ARaging.xls")

			Case 1
				If dgvARsummary.Rows.Count = 0 Then
					lblErrMsg.Text = "No A/R Summary Listed"
					Exit Sub
				End If

				admGrid = dgvARsummary
				Response.AddHeader("content-disposition", "attachment; filename=" & cboARcust.Text & "_ARSummary.xls")

			Case 2
				If dgvARsum.Rows.Count = 0 Then
					lblErrMsg.Text = "No A/R Sum Listed"
					Exit Sub
				End If

				admGrid = dgvARsum
				Response.AddHeader("content-disposition", "attachment; filename=" & cboARcust.Text & "_ARSum.xls")

		End Select

		Response.ContentType = "application/excel"
		Dim sw As New System.IO.StringWriter()
		Dim htw As New HtmlTextWriter(sw)
		admGrid.RenderControl(htw)

		Response.Write(sw.ToString())
		Response.[End]()

	End Sub

	Private Sub BindGrid()

	End Sub

	Public Overloads Overrides Sub VerifyRenderingInServerForm(ByVal c As Control)

	End Sub


	Protected Sub dgvSOmonSum_RowDataBound(sender As Object, e As GridViewRowEventArgs)

	End Sub

	Protected Sub dgvSOmonSum_RowCreated(sender As Object, e As GridViewRowEventArgs)

	End Sub

    Protected Sub dgvPerSO_RowDataBound(sender As Object, e As GridViewRowEventArgs)

    End Sub

	Protected Sub dgvPerSO_RowCreated(sender As Object, e As GridViewRowEventArgs)

	End Sub

	Private Sub btnRefr_Click(sender As Object, e As EventArgs) Handles btnRefr.Click

		Select Case TabContainer5.ActiveTabIndex
			Case 0

			Case 1

		End Select


	End Sub

	Private Sub cboSmnCust_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboSmnCust.SelectedIndexChanged
		If cboSmnCust.Text = "" Then
			Exit Sub
		End If

		'TabContainer2
		Select Case TabContainer5.ActiveTabIndex
			Case 0
				FillDgvSObal()
			Case 1
				FillDgvPerSO()
		End Select

	End Sub

	Private Sub FillDgvPerSO()

		dgvPerSO.DataSource = Nothing
		dgvPerSO.DataBind()

		Dim adapter As New MySqlDataAdapter
		Dim ds As New DataSet()
		sqldata = Nothing

		Select Case cboSmnCust.Text
			Case "ALL"
				If CheckBox5.Checked = True Then
					Select Case cboMMGrpSO.Text
						Case "ALL"
							Select Case lblCform.Text
								Case "Smn"
									admSmnNo = lblSmnName.Text.Substring(0, 3)
									dt = GetDataTable("select sum(ifnull(a.qtbal,0)),sum(ifnull(a.wtbal,0)) from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
													  "left join mmasttbl c on a.codeno=c.codeno where c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
													  "b.transdate <= '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' and " &
													  "(a.qtbal > 0 or a.wtbal > 0) and b.delstat <> 'Served' and b.status = 'approved' " &
													  "and b.smnno = '" & admSmnNo & "'")
								Case "Mgr"
									dt = GetDataTable("select sum(ifnull(a.qtbal,0)),sum(ifnull(a.wtbal,0)) from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
													  "left join mmasttbl c on a.codeno=c.codeno where c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
													  "b.transdate <= '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' and " &
													  "(a.qtbal > 0 or a.wtbal > 0) and b.delstat <> 'Served' and b.status = 'approved'")
							End Select

							If Not CBool(dt.Rows.Count) Then
								lblPerSOqty.Text = "0"
								lblPerSOwt.Text = "0.00"
								Exit Sub

							Else
								For Each dr As DataRow In dt.Rows
									lblPerSOqty.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0")
									lblPerSOwt.Text = Format(CDbl(dr.Item(1).ToString()), "#,##0.00")

								Next

							End If

							Call dt.Dispose()

							Select Case lblCform.Text
								Case "Smn"
									admSmnNo = lblSmnName.Text.Substring(0, 3)
									sqldata = "select a.sono,b.transdate,concat(b.custno,space(1),d.bussname) as custno,a.codeno," &
											  "ifnull(c.codename,c.mmdesc) as mmdesc,sum(ifnull(a.qtbal,0)) as qtbal,sum(ifnull(a.wtbal,0)) as wtbal " &
											  "from sodettbl a left join sohdrtbl b on a.sono=b.sono left join mmasttbl c on a.codeno=c.codeno " &
											  "left join custmasttbl d on b.custno=d.custno where c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' " &
											  "and b.transdate <= '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' and " &
											  "(a.qtbal > 0 or a.wtbal > 0) and b.delstat <> 'Served' and b.status = 'approved' " &
											  "and b.smnno = '" & admSmnNo & "' group by a.transid order by d.bussname,b.transdate"
								Case "Mgr"
									sqldata = "select a.sono,b.transdate,concat(b.custno,space(1),d.bussname) as custno,a.codeno," &
											  "ifnull(c.codename,c.mmdesc) as mmdesc,sum(ifnull(a.qtbal,0)) as qtbal,sum(ifnull(a.wtbal,0)) as wtbal " &
											  "from sodettbl a left join sohdrtbl b on a.sono=b.sono left join mmasttbl c on a.codeno=c.codeno " &
											  "left join custmasttbl d on b.custno=d.custno where c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' " &
											  "and b.transdate <= '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' and " &
											  "(a.qtbal > 0 or a.wtbal > 0) and b.delstat <> 'Served' and b.status = 'approved' " &
											  "group by a.transid order by d.bussname,b.transdate"
							End Select

						Case Else
							Select Case lblCform.Text
								Case "Smn"
									admSmnNo = lblSmnName.Text.Substring(0, 3)
									dt = GetDataTable("select sum(ifnull(a.qtbal,0)),sum(ifnull(a.wtbal,0)) from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
													  "left join mmasttbl c on a.codeno=c.codeno where c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
													  "b.transdate <= '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' and " &
													  "(a.qtbal > 0 or a.wtbal > 0) and b.delstat <> 'Served' and b.status = 'approved' " &
													  "and c.mmgrp = '" & cboMMGrpSO.Text & "' and b.smnno = '" & admSmnNo & "' group by c.mmgrp")
								Case "Mgr"
									dt = GetDataTable("select sum(ifnull(a.qtbal,0)),sum(ifnull(a.wtbal,0)) from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
													  "left join mmasttbl c on a.codeno=c.codeno where c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
													  "b.transdate <= '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' and " &
													  "(a.qtbal > 0 or a.wtbal > 0) and b.delstat <> 'Served' and b.status = 'approved' " &
													  "and c.mmgrp = '" & cboMMGrpSO.Text & "' group by c.mmgrp")
							End Select

							If Not CBool(dt.Rows.Count) Then
								lblPerSOqty.Text = "0"
								lblPerSOwt.Text = "0.00"
								Exit Sub

							Else
								For Each dr As DataRow In dt.Rows
									lblPerSOqty.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0")
									lblPerSOwt.Text = Format(CDbl(dr.Item(1).ToString()), "#,##0.00")

								Next

							End If

							Call dt.Dispose()

							Select Case lblCform.Text
								Case "Smn"
									admSmnNo = lblSmnName.Text.Substring(0, 3)
									sqldata = "select a.sono,b.transdate,concat(b.custno,space(1),d.bussname) as custno,a.codeno," &
											  "ifnull(c.codename,c.mmdesc) as mmdesc,sum(ifnull(a.qtbal,0)) as qtbal,sum(ifnull(a.wtbal,0)) as wtbal " &
											  "from sodettbl a left join sohdrtbl b on a.sono=b.sono left join mmasttbl c on a.codeno=c.codeno " &
											  "left join custmasttbl d on b.custno=d.custno where c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
											  "b.transdate <= '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' and " &
											  "(a.qtbal > 0 or a.wtbal > 0) and b.delstat <> 'Served' and b.status = 'approved' " &
											  "and c.mmgrp = '" & cboMMGrpSO.Text & "' and b.smnno = '" & admSmnNo & "' group by a.transid order by d.bussname,b.transdate"
								Case "Mgr"
									sqldata = "select a.sono,b.transdate,concat(b.custno,space(1),d.bussname) as custno,a.codeno," &
											  "ifnull(c.codename,c.mmdesc) as mmdesc,sum(ifnull(a.qtbal,0)) as qtbal,sum(ifnull(a.wtbal,0)) as wtbal " &
											  "from sodettbl a left join sohdrtbl b on a.sono=b.sono left join mmasttbl c on a.codeno=c.codeno " &
											  "left join custmasttbl d on b.custno=d.custno where c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
											  "b.transdate <= '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' and " &
											  "(a.qtbal > 0 or a.wtbal > 0) and b.delstat <> 'Served' and b.status = 'approved' " &
											  "and c.mmgrp = '" & cboMMGrpSO.Text & "' group by a.transid order by d.bussname,b.transdate"
							End Select
					End Select
				Else
					Select Case cboMMGrpSO.Text
						Case "ALL"
							Select Case lblCform.Text
								Case "Smn"
									admSmnNo = lblSmnName.Text.Substring(0, 3) '
									dt = GetDataTable("select sum(ifnull(a.qtbal,0)),sum(ifnull(a.wtbal,0)) from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
													  "left join mmasttbl c on a.codeno=c.codeno where c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
													  "b.transdate between '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' and " &
													  "'" & Format(CDate(dpSOdate2.Text), "yyyy-MM-dd") & "' and (a.qtbal > 0 or a.wtbal > 0) " &
													  "and b.delstat <> 'Served' and b.status = 'approved' and b.smnno = '" & admSmnNo & "'")
								Case "Mgr"
									dt = GetDataTable("select sum(ifnull(a.qtbal,0)),sum(ifnull(a.wtbal,0)) from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
													   "left join mmasttbl c on a.codeno=c.codeno where c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
													  "b.transdate between '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' and " &
													  "'" & Format(CDate(dpSOdate2.Text), "yyyy-MM-dd") & "' and (a.qtbal > 0 or a.wtbal > 0) " &
													  "and b.delstat <> 'Served' and b.status = 'approved'")
							End Select

							If Not CBool(dt.Rows.Count) Then
								lblPerSOqty.Text = "0"
								lblPerSOwt.Text = "0.00"
								Exit Sub

							Else
								For Each dr As DataRow In dt.Rows
									lblPerSOqty.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0")
									lblPerSOwt.Text = Format(CDbl(dr.Item(1).ToString()), "#,##0.00")

								Next

							End If

							Call dt.Dispose()

							Select Case lblCform.Text
								Case "Smn"
									admSmnNo = lblSmnName.Text.Substring(0, 3) ' 
									sqldata = "select a.sono,b.transdate,concat(b.custno,space(1),d.bussname) as custno,a.codeno," &
											  "ifnull(c.codename,c.mmdesc) as mmdesc,sum(ifnull(a.qtbal,0)) as qtbal,sum(ifnull(a.wtbal,0)) as wtbal " &
											  "from sodettbl a left join sohdrtbl b on a.sono=b.sono left join mmasttbl c on a.codeno=c.codeno " &
											  "left join custmasttbl d on b.custno=d.custno where c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
											  "b.transdate between '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' and " &
											  "'" & Format(CDate(dpSOdate2.Text), "yyyy-MM-dd") & "' and (a.qtbal > 0 or a.wtbal > 0) " &
											  "and b.delstat <> 'Served' and b.status = 'approved' and b.smnno = '" & admSmnNo & "' group by a.codeno order by wtbal desc"
								Case "Mgr"
									sqldata = "select a.sono,b.transdate,concat(b.custno,space(1),d.bussname) as custno,a.codeno," &
											  "ifnull(c.codename,c.mmdesc) as mmdesc,sum(ifnull(a.qtbal,0)) as qtbal,sum(ifnull(a.wtbal,0)) as wtbal " &
											  "from sodettbl a left join sohdrtbl b on a.sono=b.sono left join mmasttbl c on a.codeno=c.codeno " &
											  "left join custmasttbl d on b.custno=d.custno where c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
											  "b.transdate between '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' and " &
											  "'" & Format(CDate(dpSOdate2.Text), "yyyy-MM-dd") & "' and (a.qtbal > 0 or a.wtbal > 0) " &
											  "and b.delstat <> 'Served' and b.status = 'approved' group by a.codeno order by wtbal desc"
							End Select

						Case Else
							Select Case lblCform.Text
								Case "Smn"
									admSmnNo = lblSmnName.Text.Substring(0, 3)
									dt = GetDataTable("select sum(ifnull(a.qtbal,0)),sum(ifnull(a.wtbal,0)) from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
													  "left join mmasttbl c on a.codeno=c.codeno where c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
													  "b.transdate between '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' and " &
													  "'" & Format(CDate(dpSOdate2.Text), "yyyy-MM-dd") & "' and (a.qtbal > 0 or a.wtbal > 0) " &
													  "and b.delstat <> 'Served' and b.status = 'approved' and c.mmgrp = '" & cboMMGrpSO.Text & "' " &
													  "and b.smnno = '" & admSmnNo & "' group by c.mmgrp")
								Case "Mgr"
									dt = GetDataTable("select sum(ifnull(a.qtbal,0)),sum(ifnull(a.wtbal,0)) from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
													  "left join mmasttbl c on a.codeno=c.codeno where c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
													  "b.transdate between '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' and " &
													  "'" & Format(CDate(dpSOdate2.Text), "yyyy-MM-dd") & "' and (a.qtbal > 0 or a.wtbal > 0) " &
													  "and b.delstat <> 'Served' and b.status = 'approved' and c.mmgrp = '" & cboMMGrpSO.Text & "' group by c.mmgrp")
							End Select

							If Not CBool(dt.Rows.Count) Then
								lblPerSOqty.Text = "0"
								lblPerSOwt.Text = "0.00"
								Exit Sub

							Else
								For Each dr As DataRow In dt.Rows
									lblPerSOqty.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0")
									lblPerSOwt.Text = Format(CDbl(dr.Item(1).ToString()), "#,##0.00")

								Next

							End If

							Call dt.Dispose()

							Select Case lblCform.Text
								Case "Smn"
									admSmnNo = lblSmnName.Text.Substring(0, 3)
									sqldata = "select a.sono,b.transdate,concat(b.custno,space(1),d.bussname) as custno,a.codeno," &
											  "ifnull(c.codename,c.mmdesc) as mmdesc,sum(ifnull(a.qtbal,0)) as qtbal,sum(ifnull(a.wtbal,0)) as wtbal " &
											  "from sodettbl a left join sohdrtbl b on a.sono=b.sono left join mmasttbl c on a.codeno=c.codeno " &
											  "left join custmasttbl d on b.custno=d.custno where c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
											  "b.transdate between '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' and " &
											  "'" & Format(CDate(dpSOdate2.Text), "yyyy-MM-dd") & "' and (a.qtbal > 0 or a.wtbal > 0) " &
											  "and b.delstat <> 'Served' and b.status = 'approved' " &
											  "and c.mmgrp = '" & cboMMGrpSO.Text & "' and b.smnno = '" & admSmnNo & "' group by a.codeno order by wtbal desc"
								Case "Mgr"
									sqldata = "select a.sono,b.transdate,concat(b.custno,space(1),d.bussname) as custno,a.codeno," &
											  "ifnull(c.codename,c.mmdesc) as mmdesc,sum(ifnull(a.qtbal,0)) as qtbal,sum(ifnull(a.wtbal,0)) as wtbal " &
											  "from sodettbl a left join sohdrtbl b on a.sono=b.sono left join mmasttbl c on a.codeno=c.codeno " &
											  "left join custmasttbl d on b.custno=d.custno where c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
											  "b.transdate between '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' and " &
											  "'" & Format(CDate(dpSOdate2.Text), "yyyy-MM-dd") & "' and (a.qtbal > 0 or a.wtbal > 0) " &
											  "and b.delstat <> 'Served' and b.status = 'approved' " &
											  "and c.mmgrp = '" & cboMMGrpSO.Text & "' group by a.codeno order by wtbal desc"
							End Select

					End Select
				End If
			Case Else
				Select Case True
					Case rbMon1.Checked
						If CheckBox5.Checked = True Then
							Select Case cboMMGrpSO.Text
								Case "ALL"
									dt = GetDataTable("select sum(ifnull(a.qtbal,0)),sum(ifnull(a.wtbal,0)) from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
													  "left join mmasttbl c on a.codeno=c.codeno where c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
													  "b.transdate <= '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' and " &
													  "(a.qtbal > 0 or a.wtbal > 0) and b.delstat <> 'Served' and b.status = 'approved' " &
													  "and b.smnno = '" & cboSmnCust.Text.Substring(0, 3) & "' group by b.smnno")
									If Not CBool(dt.Rows.Count) Then
										lblPerSOqty.Text = "0"
										lblPerSOwt.Text = "0.00"
										Exit Sub

									Else
										For Each dr As DataRow In dt.Rows
											lblPerSOqty.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0")
											lblPerSOwt.Text = Format(CDbl(dr.Item(1).ToString()), "#,##0.00")

										Next

									End If

									Call dt.Dispose()

									sqldata = "select a.sono,b.transdate,concat(b.custno,space(1),d.bussname) as custno,a.codeno," &
											  "ifnull(c.codename,c.mmdesc) as mmdesc,sum(ifnull(a.qtbal,0)) as qtbal,sum(ifnull(a.wtbal,0)) as wtbal " &
											  "from sodettbl a left join sohdrtbl b on a.sono=b.sono left join mmasttbl c on a.codeno=c.codeno " &
											  "left join custmasttbl d on b.custno=d.custno where c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
											  "b.transdate <= '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' and " &
											  "(a.qtbal > 0 or a.wtbal > 0) and b.delstat <> 'Served' and b.status = 'approved' " &
											  "and b.smnno = '" & cboSmnCust.Text.Substring(0, 3) & "' group by a.codeno order by d.bussname"
								Case Else
									dt = GetDataTable("select sum(ifnull(a.qtbal,0)),sum(ifnull(a.wtbal,0)) from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
													  "left join mmasttbl c on a.codeno=c.codeno where c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
													  "b.transdate <= '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' and " &
													  "(a.qtbal > 0 or a.wtbal > 0) and b.delstat <> 'Served' and b.status = 'approved' " &
													  "and c.mmgrp = '" & cboMMGrpSO.Text & "' and b.smnno = '" & cboSmnCust.Text.Substring(0, 3) & "' " &
													  "group by b.smnno")
									If Not CBool(dt.Rows.Count) Then
										lblPerSOqty.Text = "0"
										lblPerSOwt.Text = "0.00"
										Exit Sub

									Else
										For Each dr As DataRow In dt.Rows
											lblPerSOqty.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0")
											lblPerSOwt.Text = Format(CDbl(dr.Item(1).ToString()), "#,##0.00")

										Next

									End If

									Call dt.Dispose()

									sqldata = "select a.sono,b.transdate,concat(b.custno,space(1),d.bussname) as custno,a.codeno," &
											  "ifnull(c.codename,c.mmdesc) as mmdesc,sum(ifnull(a.qtbal,0)) as qtbal,sum(ifnull(a.wtbal,0)) as wtbal " &
											  "from sodettbl a left join sohdrtbl b on a.sono=b.sono left join mmasttbl c on a.codeno=c.codeno " &
											  "left join custmasttbl d on b.custno=d.custno where c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
											  "b.transdate <= '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' and " &
											  "(a.qtbal > 0 or a.wtbal > 0) and b.delstat <> 'Served' and b.status = 'approved' " &
											  "and c.mmgrp = '" & cboMMGrpSO.Text & "' and b.smnno = '" & cboSmnCust.Text.Substring(0, 3) & "' " &
											  "group by a.codeno order by d.bussname"
							End Select
						Else
							Select Case cboMMGrpSO.Text
								Case "ALL"
									dt = GetDataTable("select sum(ifnull(a.qtbal,0)),sum(ifnull(a.wtbal,0)) from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
													  "left join mmasttbl c on a.codeno=c.codeno where c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
													  "b.transdate between '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' and " &
													  "'" & Format(CDate(dpSOdate2.Text), "yyyy-MM-dd") & "' and (a.qtbal > 0 or a.wtbal > 0) " &
													  "and b.delstat <> 'Served' and b.status = 'approved' and b.smnno = '" & cboSmnCust.Text.Substring(0, 3) & "' " &
													  "group by b.smnno")
									If Not CBool(dt.Rows.Count) Then
										lblPerSOqty.Text = "0"
										lblPerSOwt.Text = "0.00"
										Exit Sub

									Else
										For Each dr As DataRow In dt.Rows
											lblPerSOqty.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0")
											lblPerSOwt.Text = Format(CDbl(dr.Item(1).ToString()), "#,##0.00")

										Next

									End If

									Call dt.Dispose()

									sqldata = "select a.sono,b.transdate,concat(b.custno,space(1),d.bussname) as custno,a.codeno," &
											  "ifnull(c.codename,c.mmdesc) as mmdesc,sum(ifnull(a.qtbal,0)) as qtbal,sum(ifnull(a.wtbal,0)) as wtbal " &
											  "from sodettbl a left join sohdrtbl b on a.sono=b.sono left join mmasttbl c on a.codeno=c.codeno " &
											  "left join custmasttbl d on b.custno=d.custno where c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
											  "b.transdate between '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' and " &
											  "'" & Format(CDate(dpSOdate2.Text), "yyyy-MM-dd") & "' and (a.qtbal > 0 or a.wtbal > 0) " &
											  "and b.delstat <> 'Served' and b.status = 'approved' and b.smnno = '" & cboSmnCust.Text.Substring(0, 3) & "' " &
											  "group by a.codeno order by d.bussname"
								Case Else
									dt = GetDataTable("select sum(ifnull(a.qtbal,0)),sum(ifnull(a.wtbal,0)) from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
													  "left join mmasttbl c on a.codeno=c.codeno where c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
													  "b.transdate between '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' and " &
													  "'" & Format(CDate(dpSOdate2.Text), "yyyy-MM-dd") & "' and (a.qtbal > 0 or a.wtbal > 0) " &
													  "and b.delstat <> 'Served' and b.status = 'approved' and c.mmgrp = '" & cboMMGrpSO.Text & "' " &
													  "and b.smnno = '" & cboSmnCust.Text.Substring(0, 3) & "' group by b.smnno")
									If Not CBool(dt.Rows.Count) Then
										lblPerSOqty.Text = "0"
										lblPerSOwt.Text = "0.00"
										Exit Sub

									Else
										For Each dr As DataRow In dt.Rows
											lblPerSOqty.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0")
											lblPerSOwt.Text = Format(CDbl(dr.Item(1).ToString()), "#,##0.00")

										Next

									End If

									Call dt.Dispose()

									sqldata = "select a.sono,b.transdate,concat(b.custno,space(1),d.bussname) as custno,a.codeno," &
											  "ifnull(c.codename,c.mmdesc) as mmdesc,sum(ifnull(a.qtbal,0)) as qtbal,sum(ifnull(a.wtbal,0)) as wtbal " &
											  "from sodettbl a left join sohdrtbl b on a.sono=b.sono left join mmasttbl c on a.codeno=c.codeno " &
											  "left join custmasttbl d on b.custno=d.custno where c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
											  "b.transdate between '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' and " &
											  "'" & Format(CDate(dpSOdate2.Text), "yyyy-MM-dd") & "' and (a.qtbal > 0 or a.wtbal > 0) " &
											  "and b.delstat <> 'Served' and b.status = 'approved' and c.mmgrp = '" & cboMMGrpSO.Text & "' " &
											  "and b.smnno = '" & cboSmnCust.Text.Substring(0, 3) & "' " &
											  "group by a.codeno order by d.bussname"
							End Select
						End If

					Case rbMon2.Checked
						If CheckBox5.Checked = True Then
							Select Case cboMMGrpSO.Text
								Case "ALL"
									dt = GetDataTable("select sum(ifnull(a.qtbal,0)),sum(ifnull(a.wtbal,0)) from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
													  "left join mmasttbl c on a.codeno=c.codeno where c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
													  "b.transdate <= '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' and " &
													  "(a.qtbal > 0 or a.wtbal > 0) and b.delstat <> 'Served' and b.status = 'approved' " &
													  "and b.custno = '" & cboSmnCust.Text.Substring(0, 5) & "' group by b.smnno")
									If Not CBool(dt.Rows.Count) Then
										lblPerSOqty.Text = "0"
										lblPerSOwt.Text = "0.00"
										Exit Sub

									Else
										For Each dr As DataRow In dt.Rows
											lblPerSOqty.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0")
											lblPerSOwt.Text = Format(CDbl(dr.Item(1).ToString()), "#,##0.00")

										Next

									End If

									Call dt.Dispose()

									sqldata = "select a.sono,b.transdate,concat(b.custno,space(1),d.bussname) as custno,a.codeno," &
											  "ifnull(c.codename,c.mmdesc) as mmdesc,sum(ifnull(a.qtbal,0)) as qtbal,sum(ifnull(a.wtbal,0)) as wtbal " &
											  "from sodettbl a left join sohdrtbl b on a.sono=b.sono left join mmasttbl c on a.codeno=c.codeno " &
											  "left join custmasttbl d on b.custno=d.custno where c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
											  "b.transdate <= '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' and " &
											  "(a.qtbal > 0 or a.wtbal > 0) and b.delstat <> 'Served' and b.status = 'approved' " &
											  "and b.custno = '" & cboSmnCust.Text.Substring(0, 5) & "' group by a.codeno order by d.bussname"
								Case Else
									dt = GetDataTable("select sum(ifnull(a.qtbal,0)),sum(ifnull(a.wtbal,0)) from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
													  "left join mmasttbl c on a.codeno=c.codeno where c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
													  "b.transdate <= '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' and " &
													  "(a.qtbal > 0 or a.wtbal > 0) and b.delstat <> 'Served' and b.status = 'approved' " &
													  "and c.mmgrp = '" & cboMMGrpSO.Text & "' and b.custno = '" & cboSmnCust.Text.Substring(0, 5) & "' " &
													  "group by b.smnno")
									If Not CBool(dt.Rows.Count) Then
										lblPerSOqty.Text = "0"
										lblPerSOwt.Text = "0.00"
										Exit Sub

									Else
										For Each dr As DataRow In dt.Rows
											lblPerSOqty.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0")
											lblPerSOwt.Text = Format(CDbl(dr.Item(1).ToString()), "#,##0.00")

										Next

									End If

									Call dt.Dispose()

									sqldata = "select a.sono,b.transdate,concat(b.custno,space(1),d.bussname) as custno,a.codeno," &
											  "ifnull(c.codename,c.mmdesc) as mmdesc,sum(ifnull(a.qtbal,0)) as qtbal,sum(ifnull(a.wtbal,0)) as wtbal " &
											  "from sodettbl a left join sohdrtbl b on a.sono=b.sono left join mmasttbl c on a.codeno=c.codeno " &
											  "left join custmasttbl d on b.custno=d.custno where c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
											  "b.transdate <= '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' and " &
											  "(a.qtbal > 0 or a.wtbal > 0) and b.delstat <> 'Served' and b.status = 'approved' " &
											  "and c.mmgrp = '" & cboMMGrpSO.Text & "' and b.custno = '" & cboSmnCust.Text.Substring(0, 5) & "' " &
											  "group by a.codeno order by wtbal desc"
							End Select
						Else
							Select Case cboMMGrpSO.Text
								Case "ALL"
									dt = GetDataTable("select sum(ifnull(a.qtbal,0)),sum(ifnull(a.wtbal,0)) from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
													  "left join mmasttbl c on a.codeno=c.codeno where c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
													  "b.transdate between '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' and " &
													  "'" & Format(CDate(dpSOdate2.Text), "yyyy-MM-dd") & "' and (a.qtbal > 0 or a.wtbal > 0) " &
													  "and b.delstat <> 'Served' and b.status = 'approved' and b.custno = '" & cboSmnCust.Text.Substring(0, 5) & "' " &
													  "group by a.smnno")
									If Not CBool(dt.Rows.Count) Then
										lblPerSOqty.Text = "0"
										lblPerSOwt.Text = "0.00"
										Exit Sub

									Else
										For Each dr As DataRow In dt.Rows
											lblPerSOqty.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0")
											lblPerSOwt.Text = Format(CDbl(dr.Item(1).ToString()), "#,##0.00")

										Next

									End If

									Call dt.Dispose()

									sqldata = "select a.sono,b.transdate,concat(b.custno,space(1),d.bussname) as custno,a.codeno," &
											  "ifnull(c.codename,c.mmdesc) as mmdesc,sum(ifnull(a.qtbal,0)) as qtbal,sum(ifnull(a.wtbal,0)) as wtbal " &
											  "from sodettbl a left join sohdrtbl b on a.sono=b.sono left join mmasttbl c on a.codeno=c.codeno " &
											  "left join custmasttbl d on b.custno=d.custno where c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
											  "b.transdate between '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' and " &
											  "'" & Format(CDate(dpSOdate2.Text), "yyyy-MM-dd") & "' and (a.qtbal > 0 or a.wtbal > 0) " &
											  "and b.delstat <> 'Served' and b.status = 'approved' and b.custno = '" & cboSmnCust.Text.Substring(0, 5) & "' " &
											  "group by a.codeno order by wtbal desc"
								Case Else
									dt = GetDataTable("select sum(ifnull(a.qtbal,0)),sum(ifnull(a.wtbal,0)) from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
													  "left join mmasttbl c on a.codeno=c.codeno where c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
													  "b.transdate between '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' and " &
													  "'" & Format(CDate(dpSOdate2.Text), "yyyy-MM-dd") & "' and (a.qtbal > 0 or a.wtbal > 0) " &
													  "and b.delstat <> 'Served' and b.status = 'approved' and c.mmgrp = '" & cboMMGrpSO.Text & "' " &
													  "and b.custno = '" & cboSmnCust.Text.Substring(0, 5) & "' group by b.smnno")
									If Not CBool(dt.Rows.Count) Then
										lblPerSOqty.Text = "0"
										lblPerSOwt.Text = "0.00"
										Exit Sub

									Else
										For Each dr As DataRow In dt.Rows
											lblPerSOqty.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0")
											lblPerSOwt.Text = Format(CDbl(dr.Item(1).ToString()), "#,##0.00")

										Next

									End If

									Call dt.Dispose()

									sqldata = "select a.sono,b.transdate,concat(b.custno,space(1),d.bussname) as custno,a.codeno," &
											  "ifnull(c.codename,c.mmdesc) as mmdesc,sum(ifnull(a.qtbal,0)) as qtbal,sum(ifnull(a.wtbal,0)) as wtbal " &
											  "from sodettbl a left join sohdrtbl b on a.sono=b.sono left join mmasttbl c on a.codeno=c.codeno " &
											  "left join custmasttbl d on b.custno=d.custno where c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
											  "b.transdate between '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' and " &
											  "'" & Format(CDate(dpSOdate2.Text), "yyyy-MM-dd") & "' and (a.qtbal > 0 or a.wtbal > 0) " &
											  "and b.delstat <> 'Served' and b.status = 'approved' and c.mmgrp = '" & cboMMGrpSO.Text & "' " &
											  "and b.custno = '" & cboSmnCust.Text.Substring(0, 5) & "' group by a.codeno order by wtbal desc"
							End Select
						End If

				End Select

		End Select


		conn.Open()
		Dim command As New MySqlCommand(sqldata, conn)
		adapter.SelectCommand = command
		adapter.Fill(ds)
		adapter.Dispose()
		command.Dispose()
		conn.Close()
		dgvPerSO.DataSource = ds.Tables(0)
		dgvPerSO.DataBind()

	End Sub

	Private Sub FillDgvSObal()

		dgvSOmonSum.DataSource = Nothing
		dgvSOmonSum.DataBind()

		Dim adapter As New MySqlDataAdapter
		Dim ds As New DataSet()

		sqldata = Nothing

		Select Case cboSmnCust.Text
			Case "ALL"
				If CheckBox5.Checked = True Then
					Select Case cboMMGrpSO.Text
						Case "ALL"
							Select Case lblCform.Text
								Case "Smn"
									admSmnNo = lblSmnName.Text.Substring(0, 3)
									dt = GetDataTable("select sum(ifnull(a.qtbal,0)),sum(ifnull(a.wtbal,0)) from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
													  "left join mmasttbl c on a.codeno=c.codeno where c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
													  "b.transdate <= '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' and " &
													  "(a.qtbal > 0 or a.wtbal > 0) and b.delstat <> 'Served' and b.status = 'approved' and " &
													  "b.smnno = '" & admSmnNo & "'")
								Case "Mgr"
									dt = GetDataTable("select sum(ifnull(a.qtbal,0)),sum(ifnull(a.wtbal,0)) from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
													  "left join mmasttbl c on a.codeno=c.codeno where c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
													  "b.transdate <= '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' and " &
													  "(a.qtbal > 0 or a.wtbal > 0) and b.delstat <> 'Served' and b.status = 'approved'")
							End Select

							If Not CBool(dt.Rows.Count) Then
								lblSOqtyBal.Text = "0"
								lblSOwtBal.Text = "0.00"
								Exit Sub

							Else
								For Each dr As DataRow In dt.Rows
									lblSOqtyBal.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0")
									lblSOwtBal.Text = Format(CDbl(dr.Item(1).ToString()), "#,##0.00")

								Next

							End If

							Call dt.Dispose()

							Select Case lblCform.Text
								Case "Smn"
									admSmnNo = lblSmnName.Text.Substring(0, 3)
									sqldata = "select a.codeno,ifnull(c.codename,c.mmdesc) as mmdesc,sum(ifnull(a.qtbal,0)) as qtbal,sum(ifnull(a.wtbal,0)) as wtbal " &
											  "from sodettbl a left join sohdrtbl b on a.sono=b.sono left join mmasttbl c on a.codeno=c.codeno " &
											  "where c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
											  "b.transdate <= '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' and " &
											  "(a.qtbal > 0 or a.wtbal > 0) and b.delstat <> 'Served' and b.status = 'approved' " &
											  "and b.smnno = '" & admSmnNo & "' group by a.codeno order by wtbal desc"

								Case "Mgr"
									sqldata = "select a.codeno,ifnull(c.codename,c.mmdesc) as mmdesc,sum(ifnull(a.qtbal,0)) as qtbal,sum(ifnull(a.wtbal,0)) as wtbal " &
											  "from sodettbl a left join sohdrtbl b on a.sono=b.sono left join mmasttbl c on a.codeno=c.codeno " &
											  "where c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
											  "b.transdate <= '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' and " &
											  "(a.qtbal > 0 or a.wtbal > 0) and b.delstat <> 'Served' and b.status = 'approved' " &
											  "group by a.codeno order by wtbal desc"
							End Select


						Case Else
							Select Case lblCform.Text
								Case "Smn"
									admSmnNo = lblSmnName.Text.Substring(0, 3)
									dt = GetDataTable("select sum(ifnull(a.qtbal,0)),sum(ifnull(a.wtbal,0)) from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
													  "left join mmasttbl c on a.codeno=c.codeno where c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
													  "b.transdate <= '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' and " &
													  "(a.qtbal > 0 or a.wtbal > 0) and b.delstat <> 'Served' and b.status = 'approved' " &
													  "and c.mmgrp = '" & cboMMGrpSO.Text & "' and b.smnno = '" & admSmnNo & "' group by c.mmgrp")
								Case "Mgr"
									dt = GetDataTable("select sum(ifnull(a.qtbal,0)),sum(ifnull(a.wtbal,0)) from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
													  "left join mmasttbl c on a.codeno=c.codeno where c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
													  "b.transdate <= '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' and " &
													  "(a.qtbal > 0 or a.wtbal > 0) and b.delstat <> 'Served' and b.status = 'approved' " &
													  "and c.mmgrp = '" & cboMMGrpSO.Text & "' group by c.mmgrp")
							End Select

							If Not CBool(dt.Rows.Count) Then
								lblSOqtyBal.Text = "0"
								lblSOwtBal.Text = "0.00"
								Exit Sub

							Else
								For Each dr As DataRow In dt.Rows
									lblSOqtyBal.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0")
									lblSOwtBal.Text = Format(CDbl(dr.Item(1).ToString()), "#,##0.00")

								Next

							End If

							Call dt.Dispose()

							Select Case lblCform.Text
								Case "Smn"
									admSmnNo = lblSmnName.Text.Substring(0, 3)
									sqldata = "select a.codeno,ifnull(c.codename,c.mmdesc) as mmdesc,sum(ifnull(a.qtbal,0)) as qtbal,sum(ifnull(a.wtbal,0)) as wtbal " &
											  "from sodettbl a left join sohdrtbl b on a.sono=b.sono left join mmasttbl c on a.codeno=c.codeno " &
											  "where c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
											  "b.transdate <= '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' and " &
											  "(a.qtbal > 0 or a.wtbal > 0) and b.delstat <> 'Served' and b.status = 'approved' " &
											  "and c.mmgrp = '" & cboMMGrpSO.Text & "' and b.smnno = '" & admSmnNo & "' group by a.codeno order by wtbal desc"

								Case "Mgr"
									sqldata = "select a.codeno,ifnull(c.codename,c.mmdesc) as mmdesc,sum(ifnull(a.qtbal,0)) as qtbal,sum(ifnull(a.wtbal,0)) as wtbal " &
											  "from sodettbl a left join sohdrtbl b on a.sono=b.sono left join mmasttbl c on a.codeno=c.codeno " &
											  "where c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
											  "b.transdate <= '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' and " &
											  "(a.qtbal > 0 or a.wtbal > 0) and b.delstat <> 'Served' and b.status = 'approved' " &
											  "and c.mmgrp = '" & cboMMGrpSO.Text & "' group by a.codeno order by wtbal desc"
							End Select
					End Select
				Else
					Select Case cboMMGrpSO.Text
						Case "ALL"
							Select Case lblCform.Text
								Case "Smn"
									admSmnNo = lblSmnName.Text.Substring(0, 3)
									dt = GetDataTable("select sum(ifnull(a.qtbal,0)),sum(ifnull(a.wtbal,0)) from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
													  "left join mmasttbl c on a.codeno=c.codeno where c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
													  "b.transdate between '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' and " &
													  "'" & Format(CDate(dpSOdate2.Text), "yyyy-MM-dd") & "' and (a.qtbal > 0 or a.wtbal > 0) " &
													  "and b.delstat <> 'Served' and b.status = 'approved' and b.smnno = '" & admSmnNo & "'")
								Case "Mgr"
									dt = GetDataTable("select sum(ifnull(a.qtbal,0)),sum(ifnull(a.wtbal,0)) from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
													 "left join mmasttbl c on a.codeno=c.codeno where c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
													 "b.transdate between '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' and " &
													 "'" & Format(CDate(dpSOdate2.Text), "yyyy-MM-dd") & "' and (a.qtbal > 0 or a.wtbal > 0) " &
													 "and b.delstat <> 'Served' and b.status = 'approved'")
							End Select

							If Not CBool(dt.Rows.Count) Then
								lblSOqtyBal.Text = "0"
								lblSOwtBal.Text = "0.00"
								Exit Sub

							Else
								For Each dr As DataRow In dt.Rows
									lblSOqtyBal.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0")
									lblSOwtBal.Text = Format(CDbl(dr.Item(1).ToString()), "#,##0.00")

								Next

							End If

							Call dt.Dispose()

							Select Case lblCform.Text
								Case "Smn"
									admSmnNo = lblSmnName.Text.Substring(0, 3)
									sqldata = "select a.codeno,ifnull(c.codename,c.mmdesc) as mmdesc,sum(ifnull(a.qtbal,0)) as qtbal,sum(ifnull(a.wtbal,0)) as wtbal " &
											  "from sodettbl a left join sohdrtbl b on a.sono=b.sono left join mmasttbl c on a.codeno=c.codeno " &
											  "where c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
											  "b.transdate between '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' and " &
											  "'" & Format(CDate(dpSOdate2.Text), "yyyy-MM-dd") & "' and (a.qtbal > 0 or a.wtbal > 0) " &
											  "and b.delstat <> 'Served' and b.status = 'approved'  and b.smnno = '" & admSmnNo & "' group by a.codeno order by wtbal desc"

								Case "Mgr"
									sqldata = "select a.codeno,ifnull(c.codename,c.mmdesc) as mmdesc,sum(ifnull(a.qtbal,0)) as qtbal,sum(ifnull(a.wtbal,0)) as wtbal " &
											  "from sodettbl a left join sohdrtbl b on a.sono=b.sono left join mmasttbl c on a.codeno=c.codeno " &
											  "where c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
											  "b.transdate between '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' and " &
											  "'" & Format(CDate(dpSOdate2.Text), "yyyy-MM-dd") & "' and (a.qtbal > 0 or a.wtbal > 0) " &
											  "and b.delstat <> 'Served' and b.status = 'approved' group by a.codeno order by wtbal desc"
							End Select

						Case Else
							Select Case lblCform.Text
								Case "Smn"
									admSmnNo = lblSmnName.Text.Substring(0, 3)
									dt = GetDataTable("select sum(ifnull(a.qtbal,0)),sum(ifnull(a.wtbal,0)) from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
													  "left join mmasttbl c on a.codeno=c.codeno where c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
													  "b.transdate between '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' and " &
													  "'" & Format(CDate(dpSOdate2.Text), "yyyy-MM-dd") & "' and (a.qtbal > 0 or a.wtbal > 0) " &
													  "and b.delstat <> 'Served' and b.status = 'approved' and c.mmgrp = '" & cboMMGrpSO.Text & "' " &
													  " and b.smnno = '" & admSmnNo & "' group by c.mmgrp")
								Case "Mgr"
									dt = GetDataTable("select sum(ifnull(a.qtbal,0)),sum(ifnull(a.wtbal,0)) from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
													  "left join mmasttbl c on a.codeno=c.codeno where c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
													  "b.transdate between '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' and " &
													  "'" & Format(CDate(dpSOdate2.Text), "yyyy-MM-dd") & "' and (a.qtbal > 0 or a.wtbal > 0) " &
													  "and b.delstat <> 'Served' and b.status = 'approved' and c.mmgrp = '" & cboMMGrpSO.Text & "' group by c.mmgrp")
							End Select

							If Not CBool(dt.Rows.Count) Then
								lblSOqtyBal.Text = "0"
								lblSOwtBal.Text = "0.00"
								Exit Sub

							Else
								For Each dr As DataRow In dt.Rows
									lblSOqtyBal.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0")
									lblSOwtBal.Text = Format(CDbl(dr.Item(1).ToString()), "#,##0.00")

								Next

							End If

							Call dt.Dispose()

							Select Case lblCform.Text
								Case "Smn"
									admSmnNo = lblSmnName.Text.Substring(0, 3)
									sqldata = "select a.codeno,ifnull(c.codename,c.mmdesc) as mmdesc,sum(ifnull(a.qtbal,0)) as qtbal,sum(ifnull(a.wtbal,0)) as wtbal " &
												 "from sodettbl a left join sohdrtbl b on a.sono=b.sono left join mmasttbl c on a.codeno=c.codeno " &
											  "where c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
											  "b.transdate between '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' and " &
											  "'" & Format(CDate(dpSOdate2.Text), "yyyy-MM-dd") & "' and (a.qtbal > 0 or a.wtbal > 0) " &
											  "and b.delstat <> 'Served' and b.status = 'approved' " &
											  "and c.mmgrp = '" & cboMMGrpSO.Text & "'  and b.smnno = '" & admSmnNo & "' group by a.codeno order by wtbal desc"
								Case "Mgr"
									sqldata = "select a.codeno,ifnull(c.codename,c.mmdesc) as mmdesc,sum(ifnull(a.qtbal,0)) as qtbal,sum(ifnull(a.wtbal,0)) as wtbal " &
												 "from sodettbl a left join sohdrtbl b on a.sono=b.sono left join mmasttbl c on a.codeno=c.codeno " &
											  "where c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
											  "b.transdate between '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' and " &
											  "'" & Format(CDate(dpSOdate2.Text), "yyyy-MM-dd") & "' and (a.qtbal > 0 or a.wtbal > 0) " &
											  "and b.delstat <> 'Served' and b.status = 'approved' " &
											  "and c.mmgrp = '" & cboMMGrpSO.Text & "' group by a.codeno order by wtbal desc"
							End Select

					End Select
				End If
			Case Else
				Select Case True
					Case rbMon1.Checked
						If CheckBox5.Checked = True Then
							Select Case cboMMGrpSO.Text
								Case "ALL"
									dt = GetDataTable("select sum(ifnull(a.qtbal,0)),sum(ifnull(a.wtbal,0)) from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
													  "left join mmasttbl c on a.codeno=c.codeno where c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
													  "b.transdate <= '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' and " &
													  "(a.qtbal > 0 or a.wtbal > 0) and b.delstat <> 'Served' and b.status = 'approved' " &
													  "and b.smnno = '" & cboSmnCust.Text.Substring(0, 3) & "' group by b.smnno")
									If Not CBool(dt.Rows.Count) Then
										lblSOqtyBal.Text = "0"
										lblSOwtBal.Text = "0.00"
										Exit Sub

									Else
										For Each dr As DataRow In dt.Rows
											lblSOqtyBal.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0")
											lblSOwtBal.Text = Format(CDbl(dr.Item(1).ToString()), "#,##0.00")

										Next

									End If

									Call dt.Dispose()

									sqldata = "select a.codeno,ifnull(c.codename,c.mmdesc) as mmdesc,sum(ifnull(a.qtbal,0)) as qtbal,sum(ifnull(a.wtbal,0)) as wtbal " &
											  "from sodettbl a left join sohdrtbl b on a.sono=b.sono left join mmasttbl c on a.codeno=c.codeno " &
											  "where c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
											  "b.transdate <= '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' and " &
											  "(a.qtbal > 0 or a.wtbal > 0) and b.delstat <> 'Served' and b.status = 'approved' " &
											  "and b.smnno = '" & cboSmnCust.Text.Substring(0, 3) & "' group by a.codeno order by wtbal desc"
								Case Else
									dt = GetDataTable("select sum(ifnull(a.qtbal,0)),sum(ifnull(a.wtbal,0)) from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
													  "left join mmasttbl c on a.codeno=c.codeno where c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
													  "b.transdate <= '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' and " &
													  "(a.qtbal > 0 or a.wtbal > 0) and b.delstat <> 'Served' and b.status = 'approved' " &
													  "and c.mmgrp = '" & cboMMGrpSO.Text & "' and b.smnno = '" & cboSmnCust.Text.Substring(0, 3) & "' " &
													  "group by b.smnno")
									If Not CBool(dt.Rows.Count) Then
										lblSOqtyBal.Text = "0"
										lblSOwtBal.Text = "0.00"
										Exit Sub

									Else
										For Each dr As DataRow In dt.Rows
											lblSOqtyBal.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0")
											lblSOwtBal.Text = Format(CDbl(dr.Item(1).ToString()), "#,##0.00")

										Next

									End If

									Call dt.Dispose()

									sqldata = "select a.codeno,ifnull(c.codename,c.mmdesc) as mmdesc,sum(ifnull(a.qtbal,0)) as qtbal,sum(ifnull(a.wtbal,0)) as wtbal " &
											  "from sodettbl a left join sohdrtbl b on a.sono=b.sono left join mmasttbl c on a.codeno=c.codeno " &
											  "where c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
											  "b.transdate <= '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' and " &
											  "(a.qtbal > 0 or a.wtbal > 0) and b.delstat <> 'Served' and b.status = 'approved' " &
											  "and c.mmgrp = '" & cboMMGrpSO.Text & "' and b.smnno = '" & cboSmnCust.Text.Substring(0, 3) & "' " &
											  "group by a.codeno order by wtbal desc"
							End Select
						Else
							Select Case cboMMGrpSO.Text
								Case "ALL"
									dt = GetDataTable("select sum(ifnull(a.qtbal,0)),sum(ifnull(a.wtbal,0)) from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
													  "left join mmasttbl c on a.codeno=c.codeno where c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
													  "b.transdate between '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' and " &
													  "'" & Format(CDate(dpSOdate2.Text), "yyyy-MM-dd") & "' and (a.qtbal > 0 or a.wtbal > 0) " &
													  "and b.delstat <> 'Served' and b.status = 'approved' and b.smnno = '" & cboSmnCust.Text.Substring(0, 3) & "' " &
													  "group by b.smnno")
									If Not CBool(dt.Rows.Count) Then
										lblSOqtyBal.Text = "0"
										lblSOwtBal.Text = "0.00"
										Exit Sub

									Else
										For Each dr As DataRow In dt.Rows
											lblSOqtyBal.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0")
											lblSOwtBal.Text = Format(CDbl(dr.Item(1).ToString()), "#,##0.00")

										Next

									End If

									Call dt.Dispose()

									sqldata = "select a.codeno,ifnull(c.codename,c.mmdesc) as mmdesc,sum(ifnull(a.qtbal,0)) as qtbal,sum(ifnull(a.wtbal,0)) as wtbal " &
											  "from sodettbl a left join sohdrtbl b on a.sono=b.sono left join mmasttbl c on a.codeno=c.codeno " &
											  "where c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
											  "b.transdate between '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' and " &
											  "'" & Format(CDate(dpSOdate2.Text), "yyyy-MM-dd") & "' and (a.qtbal > 0 or a.wtbal > 0) " &
											  "and b.delstat <> 'Served' and b.status = 'approved' and b.smnno = '" & cboSmnCust.Text.Substring(0, 3) & "' " &
											  "group by a.codeno order by wtbal desc"
								Case Else
									dt = GetDataTable("select sum(ifnull(a.qtbal,0)),sum(ifnull(a.wtbal,0)) from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
													  "left join mmasttbl c on a.codeno=c.codeno where c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
													  "b.transdate between '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' and " &
													  "'" & Format(CDate(dpSOdate2.Text), "yyyy-MM-dd") & "' and (a.qtbal > 0 or a.wtbal > 0) " &
													  "and b.delstat <> 'Served' and b.status = 'approved' and c.mmgrp = '" & cboMMGrpSO.Text & "' " &
													  "and b.smnno = '" & cboSmnCust.Text.Substring(0, 3) & "' group by b.smnno")
									If Not CBool(dt.Rows.Count) Then
										lblSOqtyBal.Text = "0"
										lblSOwtBal.Text = "0.00"
										Exit Sub

									Else
										For Each dr As DataRow In dt.Rows
											lblSOqtyBal.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0")
											lblSOwtBal.Text = Format(CDbl(dr.Item(1).ToString()), "#,##0.00")

										Next

									End If

									Call dt.Dispose()

									sqldata = "select a.codeno,ifnull(c.codename,c.mmdesc) as mmdesc,sum(ifnull(a.qtbal,0)) as qtbal,sum(ifnull(a.wtbal,0)) as wtbal " &
											  "from sodettbl a left join sohdrtbl b on a.sono=b.sono left join mmasttbl c on a.codeno=c.codeno " &
											  "where c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
											  "b.transdate between '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' and " &
											  "'" & Format(CDate(dpSOdate2.Text), "yyyy-MM-dd") & "' and (a.qtbal > 0 or a.wtbal > 0) " &
											  "and b.delstat <> 'Served' and b.status = 'approved' and c.mmgrp = '" & cboMMGrpSO.Text & "' " &
											  "and b.smnno = '" & cboSmnCust.Text.Substring(0, 3) & "' " &
											  "group by a.codeno order by wtbal desc"
							End Select
						End If

					Case rbMon2.Checked
						If CheckBox5.Checked = True Then
							Select Case cboMMGrpSO.Text
								Case "ALL"
									dt = GetDataTable("select sum(ifnull(a.qtbal,0)),sum(ifnull(a.wtbal,0)) from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
													  "left join mmasttbl c on a.codeno=c.codeno where c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
													  "b.transdate <= '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' and " &
													  "(a.qtbal > 0 or a.wtbal > 0) and b.delstat <> 'Served' and b.status = 'approved' " &
													  "and b.custno = '" & cboSmnCust.Text.Substring(0, 5) & "' group by b.smnno")
									If Not CBool(dt.Rows.Count) Then
										lblSOqtyBal.Text = "0"
										lblSOwtBal.Text = "0.00"
										Exit Sub

									Else
										For Each dr As DataRow In dt.Rows
											lblSOqtyBal.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0")
											lblSOwtBal.Text = Format(CDbl(dr.Item(1).ToString()), "#,##0.00")

										Next

									End If

									Call dt.Dispose()

									sqldata = "select a.codeno,ifnull(c.codename,c.mmdesc) as mmdesc,sum(ifnull(a.qtbal,0)) as qtbal,sum(ifnull(a.wtbal,0)) as wtbal " &
											  "from sodettbl a left join sohdrtbl b on a.sono=b.sono left join mmasttbl c on a.codeno=c.codeno " &
											  "where c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
											  "b.transdate <= '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' and " &
											  "(a.qtbal > 0 or a.wtbal > 0) and b.delstat <> 'Served' and b.status = 'approved' " &
											  "and b.custno = '" & cboSmnCust.Text.Substring(0, 5) & "' group by a.codeno order by wtbal desc"
								Case Else
									dt = GetDataTable("select sum(ifnull(a.qtbal,0)),sum(ifnull(a.wtbal,0)) from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
													  "left join mmasttbl c on a.codeno=c.codeno where c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
													  "b.transdate <= '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' and " &
													  "(a.qtbal > 0 or a.wtbal > 0) and b.delstat <> 'Served' and b.status = 'approved' " &
													  "and c.mmgrp = '" & cboMMGrpSO.Text & "' and b.custno = '" & cboSmnCust.Text.Substring(0, 5) & "' " &
													  "group by b.smnno")
									If Not CBool(dt.Rows.Count) Then
										lblSOqtyBal.Text = "0"
										lblSOwtBal.Text = "0.00"
										Exit Sub

									Else
										For Each dr As DataRow In dt.Rows
											lblSOqtyBal.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0")
											lblSOwtBal.Text = Format(CDbl(dr.Item(1).ToString()), "#,##0.00")

										Next

									End If

									Call dt.Dispose()

									sqldata = "select a.codeno,ifnull(c.codename,c.mmdesc) as mmdesc,sum(ifnull(a.qtbal,0)) as qtbal,sum(ifnull(a.wtbal,0)) as wtbal " &
											  "from sodettbl a left join sohdrtbl b on a.sono=b.sono left join mmasttbl c on a.codeno=c.codeno " &
											  "where c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
											  "b.transdate <= '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' and " &
											  "(a.qtbal > 0 or a.wtbal > 0) and b.delstat <> 'Served' and b.status = 'approved' " &
											  "and c.mmgrp = '" & cboMMGrpSO.Text & "' and b.custno = '" & cboSmnCust.Text.Substring(0, 5) & "' " &
											  "group by a.codeno order by wtbal desc"
							End Select
						Else
							Select Case cboMMGrpSO.Text
								Case "ALL"
									dt = GetDataTable("select sum(ifnull(a.qtbal,0)),sum(ifnull(a.wtbal,0)) from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
													  "left join mmasttbl c on a.codeno=c.codeno where c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
													  "b.transdate between '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' and " &
													  "'" & Format(CDate(dpSOdate2.Text), "yyyy-MM-dd") & "' and (a.qtbal > 0 or a.wtbal > 0) " &
													  "and b.delstat <> 'Served' and b.status = 'approved' and b.custno = '" & cboSmnCust.Text.Substring(0, 5) & "' " &
													  "group by b.smnno")
									If Not CBool(dt.Rows.Count) Then
										lblSOqtyBal.Text = "0"
										lblSOwtBal.Text = "0.00"
										Exit Sub

									Else
										For Each dr As DataRow In dt.Rows
											lblSOqtyBal.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0")
											lblSOwtBal.Text = Format(CDbl(dr.Item(1).ToString()), "#,##0.00")

										Next

									End If

									Call dt.Dispose()

									sqldata = "select a.codeno,ifnull(c.codename,c.mmdesc) as mmdesc,sum(ifnull(a.qtbal,0)) as qtbal,sum(ifnull(a.wtbal,0)) as wtbal " &
											  "from sodettbl a left join sohdrtbl b on a.sono=b.sono left join mmasttbl c on a.codeno=c.codeno " &
											  "where c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
											  "b.transdate between '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' and " &
											  "'" & Format(CDate(dpSOdate2.Text), "yyyy-MM-dd") & "' and (a.qtbal > 0 or a.wtbal > 0) " &
											  "and b.delstat <> 'Served' and b.status = 'approved' and b.custno = '" & cboSmnCust.Text.Substring(0, 5) & "' " &
											  "group by a.codeno order by wtbal desc"
								Case Else
									dt = GetDataTable("select sum(ifnull(a.qtbal,0)),sum(ifnull(a.wtbal,0)) from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
													  "left join mmasttbl c on a.codeno=c.codeno where c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
													  "b.transdate between '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' and " &
													  "'" & Format(CDate(dpSOdate2.Text), "yyyy-MM-dd") & "' and (a.qtbal > 0 or a.wtbal > 0) " &
													  "and b.delstat <> 'Served' and b.status = 'approved' and c.mmgrp = '" & cboMMGrpSO.Text & "' " &
													  "and b.custno = '" & cboSmnCust.Text.Substring(0, 5) & "' group by b.smnno")
									If Not CBool(dt.Rows.Count) Then
										lblSOqtyBal.Text = "0"
										lblSOwtBal.Text = "0.00"
										Exit Sub

									Else
										For Each dr As DataRow In dt.Rows
											lblSOqtyBal.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0")
											lblSOwtBal.Text = Format(CDbl(dr.Item(1).ToString()), "#,##0.00")

										Next

									End If

									Call dt.Dispose()

									sqldata = "select a.codeno,ifnull(c.codename,c.mmdesc) as mmdesc,sum(ifnull(a.qtbal,0)) as qtbal,sum(ifnull(a.wtbal,0)) as wtbal " &
											  "from sodettbl a left join sohdrtbl b on a.sono=b.sono left join mmasttbl c on a.codeno=c.codeno " &
											  "where c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
											  "b.transdate between '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' and " &
											  "'" & Format(CDate(dpSOdate2.Text), "yyyy-MM-dd") & "' and (a.qtbal > 0 or a.wtbal > 0) " &
											  "and b.delstat <> 'Served' and b.status = 'approved' and c.mmgrp = '" & cboMMGrpSO.Text & "' " &
											  "and b.custno = '" & cboSmnCust.Text.Substring(0, 5) & "' group by a.codeno order by wtbal desc"
							End Select
						End If

				End Select

		End Select


		conn.Open()
		Dim command As New MySqlCommand(sqldata, conn)
		adapter.SelectCommand = command
		adapter.Fill(ds)
		adapter.Dispose()
		command.Dispose()
		conn.Close()
		dgvSOmonSum.DataSource = ds.Tables(0)
		dgvSOmonSum.DataBind()

	End Sub

	Private Sub rbMon1_CheckedChanged(sender As Object, e As EventArgs) Handles rbMon1.CheckedChanged, rbMon2.CheckedChanged
		If cboMMGrpSO.Text = "" Then
			Exit Sub
		End If

		Select Case TabContainer2.ActiveTabIndex
			Case 0
				dgvSOmonSum.DataSource = Nothing
				dgvSOmonSum.DataBind()
				lblSOqtyBal.Text = "0"
				lblSOwtBal.Text = "0.00"

			Case 1
				dgvPerSO.DataSource = Nothing
				dgvPerSO.DataBind()
				lblPerSOqty.Text = "0"
				lblPerSOwt.Text = "0.00"

		End Select

		Select Case lblCform.Text
			Case "Mgr"
				GetSmnCustList()
			Case "Smn"
				GetSmnCustListSmn()
		End Select

	End Sub

	Private Sub GetSmnCustListSmn()

		Select Case lblCform.Text
			Case "Smn"
				admSmnNo = lblSmnName.Text.Substring(0, 3)
			Case "Mgr"
				admSmnNo = cboSmname.Text.Substring(0, 3)
		End Select

		cboSmnCust.Items.Clear()

		If CheckBox5.Checked = True Then
			Select Case True
				Case rbMon1.Checked
					Select Case cboMMGrpSO.Text
						Case "ALL"
							dt = GetDataTable("select concat(b.smnno,space(1),d.fullname) from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
											  "left join mmasttbl c on a.codeno=c.codeno left join smnmtbl d on b.smnno=d.smnno where " &
											  "c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
											  "b.transdate <= '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' and " &
											  "(a.qtbal > 0 or a.wtbal > 0) and b.delstat <> 'Served' and b.status = 'approved' " &
											  "and b.smnno = '" & admSmnNo & "' group by b.smnno")
						Case Else
							dt = GetDataTable("select concat(b.smnno,space(1),d.fullname) from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
											  "left join mmasttbl c on a.codeno=c.codeno left join smnmtbl d on b.smnno=d.smnno where " &
											  "c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
											  "b.transdate <= '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' " &
											  "and (a.qtbal > 0 or a.wtbal > 0) and b.delstat <> 'Served' and b.status = 'approved' " &
											  "and c.mmgrp = '" & cboMMGrpSO.Text & "' and b.smnno = '" & admSmnNo & "' group by b.smnno")
					End Select

				Case rbMon2.Checked
					Select Case cboMMGrpSO.Text
						Case "ALL"
							dt = GetDataTable("select concat(b.custno,space(1),d.bussname) from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
											  "left join mmasttbl c on a.codeno=c.codeno left join custmasttbl d on b.custno=d.custno where " &
											  "c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
											  "b.transdate <= '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' " &
											  "and (a.qtbal > 0 or a.wtbal > 0) and b.delstat <> 'Served' and b.status = 'approved' " &
											  "and b.smnno = '" & admSmnNo & "' group by b.custno order by d.bussname")
						Case Else
							dt = GetDataTable("select concat(b.custno,space(1),d.bussname)  from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
											  "left join mmasttbl c on a.codeno=c.codeno left join custmasttbl d on b.custno=d.custno where " &
											  "c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
											  "b.transdate <= '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' " &
											  "and (a.qtbal > 0 or a.wtbal > 0) and b.delstat <> 'Served' and b.status = 'approved' " &
											  "and c.mmgrp = '" & cboMMGrpSO.Text & "' and b.smnno = '" & admSmnNo & "' group by b.custno order by d.bussname")
					End Select
			End Select
		Else
			Select Case True
				Case rbMon1.Checked
					Select Case cboMMGrpSO.Text
						Case "ALL"
							dt = GetDataTable("select concat(b.smnno,space(1),d.fullname) from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
											  "left join mmasttbl c on a.codeno=c.codeno left join smnmtbl d on b.smnno=d.smnno where " &
											  "c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
											  "b.transdate between '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' " &
											  "and '" & Format(CDate(dpSOdate2.Text), "yyyy-MM-dd") & "' and (a.qtbal > 0 or a.wtbal > 0) " &
											  "and b.delstat <> 'Served' and b.status = 'approved' and " &
											  "b.smnno = '" & admSmnNo & "' group by b.smnno")
						Case Else
							dt = GetDataTable("select concat(b.smnno,space(1),d.fullname) from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
											  "left join mmasttbl c on a.codeno=c.codeno left join smnmtbl d on b.smnno=d.smnno where " &
											  "c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
											  "b.transdate between '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' " &
											  "and '" & Format(CDate(dpSOdate2.Text), "yyyy-MM-dd") & "' and (a.qtbal > 0 or a.wtbal > 0) " &
											  "and b.delstat <> 'Served' and b.status = 'approved' and c.mmgrp = '" & cboMMGrpSO.Text & "' " &
											  "and b.smnno = '" & admSmnNo & "' group by b.smnno")
					End Select

				Case rbMon2.Checked
					Select Case cboMMGrpSO.Text
						Case "ALL"
							dt = GetDataTable("select concat(b.custno,space(1),d.bussname) from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
											  "left join mmasttbl c on a.codeno=c.codeno left join custmasttbl d on b.custno=d.custno where " &
											  "c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
											  "b.transdate between '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' " &
											  "and '" & Format(CDate(dpSOdate2.Text), "yyyy-MM-dd") & "' and (a.qtbal > 0 or a.wtbal > 0) " &
											  "and b.delstat <> 'Served' and b.status = 'approved' " &
											  "and b.smnno = '" & admSmnNo & "' group by b.custno order by d.bussname")
						Case Else
							dt = GetDataTable("select concat(b.custno,space(1),d.bussname)  from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
											  "left join mmasttbl c on a.codeno=c.codeno left join custmasttbl d on b.custno=d.custno where " &
											  "c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
											  "b.transdate between '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' " &
											  "and '" & Format(CDate(dpSOdate2.Text), "yyyy-MM-dd") & "' and (a.qtbal > 0 or a.wtbal > 0) " &
											  "and b.delstat <> 'Served' and b.status = 'approved' and c.mmgrp = '" & cboMMGrpSO.Text & "' " &
											  "and b.smnno = '" & admSmnNo & "' group by b.custno order by d.bussname")
					End Select
			End Select
		End If

		If Not CBool(dt.Rows.Count) Then
			Exit Sub

		Else
			cboSmnCust.Items.Add("")
			cboSmnCust.Items.Add("ALL")
			For Each dr As DataRow In dt.Rows
				cboSmnCust.Items.Add(dr.Item(0).ToString() & "")

			Next

		End If

		Call dt.Dispose()

	End Sub

	Private Sub GetSmnCustList()

		Select Case lblCform.Text
			Case "Smn"
				admSmnNo = lblSmnName.Text.Substring(0, 3)
			Case "Mgr"
				admSmnNo = cboSmname.Text.Substring(0, 3)
		End Select

		cboSmnCust.Items.Clear()

		Select Case cboSmname.Text
			Case "ALL"
				If CheckBox5.Checked = True Then
					Select Case True
						Case rbMon1.Checked
							Select Case cboMMGrpSO.Text
								Case "ALL"
									dt = GetDataTable("select concat(b.smnno,space(1),d.fullname) from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
													  "left join mmasttbl c on a.codeno=c.codeno left join smnmtbl d on b.smnno=d.smnno where " &
													  "c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
													  "b.transdate <= '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' and " &
													  "(a.qtbal > 0 or a.wtbal > 0) and b.delstat <> 'Served' and b.status = 'approved' group by b.smnno")
								Case Else
									dt = GetDataTable("select concat(b.smnno,space(1),d.fullname) from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
													  "left join mmasttbl c on a.codeno=c.codeno left join smnmtbl d on b.smnno=d.smnno where " &
													  "c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
													  "b.transdate <= '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' " &
													  "and (a.qtbal > 0 or a.wtbal > 0) and b.delstat <> 'Served' and b.status = 'approved' " &
													  "and c.mmgrp = '" & cboMMGrpSO.Text & "' group by b.smnno")
							End Select

						Case rbMon2.Checked
							Select Case cboMMGrpSO.Text
								Case "ALL"
									dt = GetDataTable("select concat(b.custno,space(1),d.bussname) from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
													  "left join mmasttbl c on a.codeno=c.codeno left join custmasttbl d on b.custno=d.custno where " &
													  "c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
													  "b.transdate <= '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' " &
													  "and (a.qtbal > 0 or a.wtbal > 0) and b.delstat <> 'Served' and b.status = 'approved' group by b.custno order by d.bussname")
								Case Else
									dt = GetDataTable("select concat(b.custno,space(1),d.bussname)  from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
													  "left join mmasttbl c on a.codeno=c.codeno left join custmasttbl d on b.custno=d.custno where " &
													  "c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
													  "b.transdate <= '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' " &
													  "and (a.qtbal > 0 or a.wtbal > 0) and b.delstat <> 'Served' and b.status = 'approved' " &
													  "and c.mmgrp = '" & cboMMGrpSO.Text & "' group by b.custno order by d.bussname")
							End Select
					End Select
				Else
					Select Case True
						Case rbMon1.Checked
							Select Case cboMMGrpSO.Text
								Case "ALL"
									dt = GetDataTable("select concat(b.smnno,space(1),d.fullname) from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
													  "left join mmasttbl c on a.codeno=c.codeno left join smnmtbl d on b.smnno=d.smnno where " &
													  "c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
													  "b.transdate between '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' " &
													  "and '" & Format(CDate(dpSOdate2.Text), "yyyy-MM-dd") & "' and (a.qtbal > 0 or a.wtbal > 0) " &
													  "and b.delstat <> 'Served' and b.status = 'approved' group by b.smnno")
								Case Else
									dt = GetDataTable("select concat(b.smnno,space(1),d.fullname) from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
													  "left join mmasttbl c on a.codeno=c.codeno left join smnmtbl d on b.smnno=d.smnno where " &
													  "c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
													  "b.transdate between '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' " &
													  "and '" & Format(CDate(dpSOdate2.Text), "yyyy-MM-dd") & "' and (a.qtbal > 0 or a.wtbal > 0) " &
													  "and b.delstat <> 'Served' and b.status = 'approved' and c.mmgrp = '" & cboMMGrpSO.Text & "' " &
													  "group by b.smnno")
							End Select

						Case rbMon2.Checked
							Select Case cboMMGrpSO.Text
								Case "ALL"
									dt = GetDataTable("select concat(b.custno,space(1),d.bussname) from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
													  "left join mmasttbl c on a.codeno=c.codeno left join custmasttbl d on b.custno=d.custno where " &
													  "c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
													  "b.transdate between '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' " &
													  "and '" & Format(CDate(dpSOdate2.Text), "yyyy-MM-dd") & "' and (a.qtbal > 0 or a.wtbal > 0) " &
													  "and b.delstat <> 'Served' and b.status = 'approved' group by b.custno order by d.bussname")
								Case Else
									dt = GetDataTable("select concat(b.custno,space(1),d.bussname)  from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
													  "left join mmasttbl c on a.codeno=c.codeno left join custmasttbl d on b.custno=d.custno where " &
													  "c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
													  "b.transdate between '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' " &
													  "and '" & Format(CDate(dpSOdate2.Text), "yyyy-MM-dd") & "' and (a.qtbal > 0 or a.wtbal > 0) " &
													  "and b.delstat <> 'Served' and b.status = 'approved' and c.mmgrp = '" & cboMMGrpSO.Text & "' " &
													  "group by b.custno order by d.bussname")
							End Select
					End Select
				End If

			Case Else
				If CheckBox5.Checked = True Then
					Select Case True
						Case rbMon1.Checked
							Select Case cboMMGrpSO.Text
								Case "ALL"
									dt = GetDataTable("select concat(b.smnno,space(1),d.fullname) from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
													  "left join mmasttbl c on a.codeno=c.codeno left join smnmtbl d on b.smnno=d.smnno where " &
													  "c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
													  "b.transdate <= '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' and " &
													  "(a.qtbal > 0 or a.wtbal > 0) and b.delstat <> 'Served' and b.status = 'approved' " &
													  "and b.smnno = '" & admSmnNo & "' group by b.smnno")
								Case Else
									dt = GetDataTable("select concat(b.smnno,space(1),d.fullname) from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
													  "left join mmasttbl c on a.codeno=c.codeno left join smnmtbl d on b.smnno=d.smnno where " &
													  "c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
													  "b.transdate <= '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' " &
													  "and (a.qtbal > 0 or a.wtbal > 0) and b.delstat <> 'Served' and b.status = 'approved' " &
													  "and c.mmgrp = '" & cboMMGrpSO.Text & "' and b.smnno = '" & admSmnNo & "' group by b.smnno")
							End Select

						Case rbMon2.Checked
							Select Case cboMMGrpSO.Text
								Case "ALL"
									dt = GetDataTable("select concat(b.custno,space(1),d.bussname) from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
													  "left join mmasttbl c on a.codeno=c.codeno left join custmasttbl d on b.custno=d.custno where " &
													  "c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
													  "b.transdate <= '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' " &
													  "and (a.qtbal > 0 or a.wtbal > 0) and b.delstat <> 'Served' and b.status = 'approved' " &
													  "and b.smnno = '" & admSmnNo & "' group by b.custno order by d.bussname")
								Case Else
									dt = GetDataTable("select concat(b.custno,space(1),d.bussname)  from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
													  "left join mmasttbl c on a.codeno=c.codeno left join custmasttbl d on b.custno=d.custno where " &
													  "c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
													  "b.transdate <= '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' " &
													  "and (a.qtbal > 0 or a.wtbal > 0) and b.delstat <> 'Served' and b.status = 'approved' " &
													  "and c.mmgrp = '" & cboMMGrpSO.Text & "' and b.smnno = '" & admSmnNo & "' group by b.custno order by d.bussname")
							End Select
					End Select
				Else
					Select Case True
						Case rbMon1.Checked
							Select Case cboMMGrpSO.Text
								Case "ALL"
									dt = GetDataTable("select concat(b.smnno,space(1),d.fullname) from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
													  "left join mmasttbl c on a.codeno=c.codeno left join smnmtbl d on b.smnno=d.smnno where " &
													  "c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
													  "b.transdate between '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' " &
													  "and '" & Format(CDate(dpSOdate2.Text), "yyyy-MM-dd") & "' and (a.qtbal > 0 or a.wtbal > 0) " &
													  "and b.delstat <> 'Served' and b.status = 'approved' and " &
													  "b.smnno = '" & admSmnNo & "' group by b.smnno")
								Case Else
									dt = GetDataTable("select concat(b.smnno,space(1),d.fullname) from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
													  "left join mmasttbl c on a.codeno=c.codeno left join smnmtbl d on b.smnno=d.smnno where " &
													  "c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
													  "b.transdate between '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' " &
													  "and '" & Format(CDate(dpSOdate2.Text), "yyyy-MM-dd") & "' and (a.qtbal > 0 or a.wtbal > 0) " &
													  "and b.delstat <> 'Served' and b.status = 'approved' and c.mmgrp = '" & cboMMGrpSO.Text & "' " &
													  "and b.smnno = '" & admSmnNo & "' group by b.smnno")
							End Select

						Case rbMon2.Checked
							Select Case cboMMGrpSO.Text
								Case "ALL"
									dt = GetDataTable("select concat(b.custno,space(1),d.bussname) from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
													  "left join mmasttbl c on a.codeno=c.codeno left join custmasttbl d on b.custno=d.custno where " &
													  "c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
													  "b.transdate between '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' " &
													  "and '" & Format(CDate(dpSOdate2.Text), "yyyy-MM-dd") & "' and (a.qtbal > 0 or a.wtbal > 0) " &
													  "and b.delstat <> 'Served' and b.status = 'approved' " &
													  "and b.smnno = '" & admSmnNo & "' group by b.custno order by d.bussname")
								Case Else
									dt = GetDataTable("select concat(b.custno,space(1),d.bussname)  from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
													  "left join mmasttbl c on a.codeno=c.codeno left join custmasttbl d on b.custno=d.custno where " &
													  "c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
													  "b.transdate between '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' " &
													  "and '" & Format(CDate(dpSOdate2.Text), "yyyy-MM-dd") & "' and (a.qtbal > 0 or a.wtbal > 0) " &
													  "and b.delstat <> 'Served' and b.status = 'approved' and c.mmgrp = '" & cboMMGrpSO.Text & "' " &
													  "and b.smnno = '" & admSmnNo & "' group by b.custno order by d.bussname")
							End Select
					End Select
				End If
		End Select

		If Not CBool(dt.Rows.Count) Then
			Exit Sub

		Else
			cboSmnCust.Items.Add("")
			cboSmnCust.Items.Add("ALL")
			For Each dr As DataRow In dt.Rows
				cboSmnCust.Items.Add(dr.Item(0).ToString() & "")

			Next

		End If

		Call dt.Dispose()

	End Sub

	Private Sub dpSOdate1_TextChanged(sender As Object, e As EventArgs) Handles dpSOdate1.TextChanged, dpSOdate2.TextChanged
		Select Case lblCform.Text
			Case "Smn"

			Case "Mgr"
				If cboSmname.Text = "" Then
					Exit Sub
				Else

				End If
		End Select

		If dpSOdate1.Text = Nothing Then
			Exit Sub
		ElseIf dpSOdate2.Text = Nothing Then
			If CheckBox5.Checked = True Then

			Else
				Exit Sub
			End If

		ElseIf Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") > Format(CDate(dpSOdate2.Text), "yyyy-MM-dd") Then
			If CheckBox5.Checked = True Then

			Else
				Exit Sub
			End If

		End If

		GetPClassSO()

	End Sub

	Private Sub GetPClassSO()
		cboPClassSO.Items.Clear()
		dt = GetDataTable("select concat(pc,space(1),pclass) from pctrtbl where tradetype = 'trade'")
		If Not CBool(dt.Rows.Count) Then
			Exit Sub

		Else
			cboPClassSO.Items.Add("")
			For Each dr As DataRow In dt.Rows
				cboPClassSO.Items.Add(dr.Item(0).ToString() & "")

			Next

		End If

		Call dt.Dispose()

	End Sub

	Private Sub CheckBox5_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox5.CheckedChanged
		If CheckBox5.Checked = True Then
			lblSODateLabel.Text = "As Of:"
			dpSOdate2.Enabled = False

		Else
			lblSODateLabel.Text = "Date From:"
			dpSOdate2.Enabled = True

		End If

		dgvSOmonSum.DataSource = Nothing
		dgvSOmonSum.DataBind()
		cboMMGrpSO.Items.Clear()
		cboMMGrpSO.Text = Nothing
		rbMon1.Checked = False
		rbMon2.Checked = False
		cboSmnCust.Items.Clear()
		cboSmnCust.Text = Nothing

	End Sub

	Private Sub cboPClassSO_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboPClassSO.SelectedIndexChanged
		If cboPClassSO.Text = "" Then
			Exit Sub

		End If

		GetMMgrpSO()

	End Sub

	Private Sub GetMMgrpSO()
		Select Case lblCform.Text
			Case "Smn"
				admSmnNo = lblSmnName.Text.Substring(0, 3)
			Case "Mgr"
				admSmnNo = cboSmname.Text.Substring(0, 3)
		End Select

		cboMMGrpSO.Items.Clear()

		If CheckBox5.Checked = True Then
			Select Case cboSmname.Text
				Case "ALL"
					dt = GetDataTable("select c.mmgrp from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
									  "left join mmasttbl c on a.codeno=c.codeno where c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' " &
									  "and b.transdate <= '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' and " &
									  "(a.qtbal > 0 or a.wtbal > 0) and b.delstat <> 'Served' and b.status = 'approved' group by c.mmgrp")
				Case Else
					'lblSmnName


					dt = GetDataTable("select c.mmgrp from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
									  "left join mmasttbl c on a.codeno=c.codeno where c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' " &
									  "and b.transdate <= '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' and " &
									  "(a.qtbal > 0 or a.wtbal > 0) and b.delstat <> 'Served' and b.status = 'approved' " &
									  "and b.smnno = '" & admSmnNo & "' group by c.mmgrp")
			End Select

		Else
			Select Case cboSmname.Text
				Case "ALL"
					dt = GetDataTable("select c.mmgrp from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
									   "left join mmasttbl c on a.codeno=c.codeno where c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' " &
									  "and b.transdate between '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' " &
									  "and '" & Format(CDate(dpSOdate2.Text), "yyyy-MM-dd") & "' and (a.qtbal > 0 or a.wtbal > 0) " &
									  "and b.delstat <> 'Served' and b.status = 'approved' group by c.mmgrp")
				Case Else
					dt = GetDataTable("select c.mmgrp from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
									  "left join mmasttbl c on a.codeno=c.codeno where c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' " &
									  "and b.transdate between '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' " &
									  "and '" & Format(CDate(dpSOdate2.Text), "yyyy-MM-dd") & "' and (a.qtbal > 0 or a.wtbal > 0) " &
									  "and b.delstat <> 'Served' and b.status = 'approved' and " &
									  "b.smnno = '" & admSmnNo & "' group by c.mmgrp")
			End Select


		End If

		If Not CBool(dt.Rows.Count) Then
			Exit Sub

		Else
			cboMMGrpSO.Items.Add("")
			cboMMGrpSO.Items.Add("ALL")
			For Each dr As DataRow In dt.Rows
				cboMMGrpSO.Items.Add(dr.Item(0).ToString() & "")

			Next

		End If

		Call dt.Dispose()

	End Sub

	Private Sub cboMMGrpSO_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboMMGrpSO.SelectedIndexChanged

	End Sub

	Private Sub dpSOdate1_PreRender(sender As Object, e As EventArgs) Handles dpSOdate1.PreRender

	End Sub

	'Private Sub TabContainer1_ActiveTabChanged(sender As Object, e As EventArgs) Handles TabContainer1.ActiveTabChanged
	'	Select Case TabContainer1.ActiveTabIndex
	'		Case 0
	'			lblMsg.Text = "    " & TabPanel1.HeaderText
	'		Case 1
	'			lblMsg.Text = "    " & TabPanel2.HeaderText
	'		Case 2
	'			lblMsg.Text = "    " & TabPanel3.HeaderText
	'		Case 3
	'			lblMsg.Text = "    " & TabPanel4.HeaderText
	'		Case 4
	'			lblMsg.Text = "    " & TabPanel5.HeaderText
	'		Case 5
	'			lblMsg.Text = "    " & TabPanel6.HeaderText
	'	End Select
	'End Sub

End Class