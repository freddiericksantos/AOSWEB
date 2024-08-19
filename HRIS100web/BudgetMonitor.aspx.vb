Imports MySql.Data.MySqlClient
Imports System.IO
Imports System.Data.OleDb
Imports System.Data
Imports System.Data.DataTable
Imports System.Data.SqlTypes

Public Class BudgetMonitor
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
    Dim admDesc As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If lblUser.Text Is Nothing Then
            Response.Redirect("login.aspx")

        End If

        If Not Me.IsPostBack Then
            lblUser.Text = Session("UserID")
            lblGrpUser.Text = Session("UserGrp")
            GetYearTrans()
        End If


    End Sub

    Private Sub GetYearTrans()
        cboYear.Items.Clear()
        cboYear.Items.Add("")

        Select Case vLoggedBussArea
            Case "8100"
                cboYear.Items.Add("2016")
                cboYear.Items.Add("2017")
                cboYear.Items.Add("2018")
                cboYear.Items.Add("2019")
                cboYear.Items.Add("2020")
                cboYear.Items.Add("2021")
                cboYear.Items.Add("2022")
                cboYear.Items.Add("2023")

            Case "8200"
                cboYear.Items.Add("2022")
                cboYear.Items.Add("2023")

            Case "8300"
                cboYear.Items.Add("2020")
                cboYear.Items.Add("2021")
                cboYear.Items.Add("2022")
                cboYear.Items.Add("2023")

        End Select

    End Sub
    Protected Sub OnConfirm(sender As Object, e As EventArgs)

    End Sub

    Protected Sub lbSave_Click(sender As Object, e As EventArgs)

    End Sub

    Protected Sub lbClose_Click(sender As Object, e As EventArgs)
        Response.Redirect("FinancialAccounting.aspx")

    End Sub

    Private Sub cboYear_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboYear.SelectedIndexChanged
        If cboYear.Text = "" Then
            Exit Sub
        End If

        GetBudMonAvailable()

    End Sub

    Private Sub GetBudMonAvailable()
        cboBudMon.Items.Clear()
        dt = GetDataTable("select datefr from buddettbl where year = '" & cboYear.Text & "' group by datefr")
        If Not CBool(dt.Rows.Count) Then
            'lblMsg.Text = "No Plant found."
            Exit Sub
        Else
            cboBudMon.Items.Add("")
            For Each dr As DataRow In dt.Rows
                cboBudMon.Items.Add(Format(CDate(dr.Item(0).ToString()), "MMM yyyy"))

            Next
        End If

        Call dt.Dispose()

    End Sub

    Private Sub cboBudMon_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboBudMon.SelectedIndexChanged
        lblDateBM1.Text = Format(CDate(FirstDayOfMonth(cboBudMon.Text)), "yyyy-MM-dd")
        lblDateBM2.Text = Format(CDate(LastDayOfMonth(cboBudMon.Text)), "yyyy-MM-dd")

        GetCostCenterBM()

    End Sub

    Private Sub GetCostCenterBM()
        cboCCenterBM.Items.Clear()
        dt = GetDataTable("select concat(a.ccno,space(1),c.ccdesc2) from buddettbl a left join cctrnotbl c on a.ccno=c.ccno " &
                          "where a.datefr between '" & Format(CDate(lblDateBM1.Text), "yyyy-MM-dd") & "' and " &
                          "'" & Format(CDate(lblDateBM2.Text), "yyyy-MM-dd") & "' group by a.ccno")
        If Not CBool(dt.Rows.Count) Then
            GetCCenterMastBM()
            Exit Sub
        Else
            cboCCenterBM.Items.Add("")
            For Each dr As DataRow In dt.Rows
                cboCCenterBM.Items.Add(dr.Item(0).ToString())
            Next

        End If

        dt.Dispose()

    End Sub

    Private Sub GetCCenterMastBM()
        dt = GetDataTable("select concat(budccno,space(1),ccdesc2) from cctrnotbl")
        If Not CBool(dt.Rows.Count) Then
            Exit Sub
        Else
            cboCCenterBM.Items.Add("")
            For Each dr As DataRow In dt.Rows
                cboCCenterBM.Items.Add(dr.Item(0).ToString())
            Next

        End If

        dt.Dispose()

    End Sub

    Private Sub ResetFieldsTab()
        dgvBudMon.DataSource = Nothing
        dgvBudMon.DataBind()
        'dgvActDet.DataSource = Nothing
        lblTotBudStd.Text = "0.00 "
        lblTotBudAct.Text = "0.00 "
        lblTotBudVar.Text = "0.00 "
        lblActAmtDet.Text = "0.00 "

    End Sub

    Private Sub CreateTempDataBud()
        sql = "delete from tempexpdettbl where user = '" & vLoggedUserID & "'"
        ExecuteNonQuery(sql)

        sql = "insert into tempexpdettbl(acctno,ccno,dramt,user) select acctno,ccno,sum(ifnull(budamt,0)),'" & vLoggedUserID & "' " &
              "from buddettbl where ccno = '" & cboCCenterBM.Text.Substring(0, 5) & "' and " &
              "datefr between '" & Format(CDate(lblDateBM1.Text), "yyyy-MM-dd") & "' and " &
              "'" & Format(CDate(lblDateBM2.Text), "yyyy-MM-dd") & "' group by acctno"
        ExecuteNonQuery(sql)

        sql = "insert into tempexpdettbl(acctno,ccno,cramt,user) select a.acctno,a.ccno,sum(ifnull(a.dramt,0))-sum(ifnull(a.cramt,0))," &
              "'" & vLoggedUserID & "' from gltranstbl a left join cctrnotbl c on a.ccno = c.ccno " &
              "where a.tc between '40' and '48' and a.transdate between '" & Format(CDate(lblDateBM1.Text), "yyyy-MM-dd") & "' and " &
              "'" & Format(CDate(lblDateBM2.Text), "yyyy-MM-dd") & "' and c.budccno = '" & cboCCenterBM.Text.Substring(0, 5) & "' " &
              "group by a.acctno,a.ccno"
        ExecuteNonQuery(sql)

        'update current status
        sql = "update tempexpdettbl a,(select ccno,acctno,ifnull(currstat,'Open') as currstat from buddettbl where " &
              "datefr = '" & Format(CDate(lblDateBM1.Text), "yyyy-MM-dd") & "' and " &
              "dateto = '" & Format(CDate(lblDateBM2.Text), "yyyy-MM-dd") & "' and ccno = '" & cboCCenterBM.Text.Substring(0, 5) & "') as b " &
              "set a.glstat = b.currstat where a.ccno = b.ccno and a.acctno = b.acctno and a.user = '" & vLoggedUserID & "' "
        ExecuteNonQuery(sql)

        dt = GetDataTable("select sum(ifnull(dramt,0)),sum(ifnull(cramt,0)),sum(ifnull(dramt,0))-sum(ifnull(cramt,0)) " &
                          "from tempexpdettbl where user = '" & vLoggedUserID & "'")
        If Not CBool(dt.Rows.Count) Then
            MsgBox("No Detailed Found")
            dgvBudMon.DataSource = Nothing
            lblTotBudStd.Text = "0.00 "
            lblTotBudAct.Text = "0.00 "
            lblTotBudVar.Text = "0.00 "
            Exit Sub
        Else
            For Each dr As DataRow In dt.Rows
                lblTotBudStd.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0.00 ; (#,##0.00)") & " "
                lblTotBudAct.Text = Format(CDbl(dr.Item(1).ToString()), "#,##0.00 ; (#,##0.00)") & " "
                lblTotBudVar.Text = Format(CDbl(dr.Item(2).ToString()), "#,##0.00 ; (#,##0.00)") & " "
            Next

        End If

        dt.Dispose()

        Dim adapter As New MySqlDataAdapter
        Dim ds As New DataSet()
        Dim i As Integer = 0
        sqldata = Nothing

        sqldata = "select concat(a.acctno,space(1),b.acctdesc) as acct,sum(ifnull(a.dramt,0)) as budamt,sum(ifnull(a.cramt,0)) as actamt," &
                  "sum(ifnull(a.dramt,0))-sum(ifnull(a.cramt,0)) as varamt from tempexpdettbl a " &
                  "left join acctcharttbl b on a.acctno=b.acctno where a.user = '" & vLoggedUserID & "' group by a.acctno"

        With dgvBudMon
            .Columns(1).HeaderText = "Account Description"
            .Columns(2).HeaderText = "Budget Amt"
            .Columns(3).HeaderText = "Actual Amt"
            .Columns(4).HeaderText = "Balance(Short)"
        End With

        conn.Open()
        Dim command As New MySqlCommand(sqldata, conn)
        adapter.SelectCommand = command
        adapter.Fill(ds)
        adapter.Dispose()
        command.Dispose()
        conn.Close()
        dgvBudMon.DataSource = ds.Tables(0)
        dgvBudMon.DataBind()

        For x As Integer = 0 To dgvBudMon.Rows.Count - 1
            If CDbl(dgvBudMon.Rows(x).Cells(4).Text.ToString) < 0 Then
                dgvBudMon.Rows(x).ForeColor = System.Drawing.Color.Red

            Else
                dgvBudMon.Rows(x).ForeColor = System.Drawing.Color.Black
            End If
        Next

    End Sub

    Protected Sub dgvBudMon_RowDataBound(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Protected Sub dgvBudMon_RowCreated(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Private Sub cboCCenterBM_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboCCenterBM.SelectedIndexChanged
        If cboCCenterBM.Text = "" Then
            Exit Sub

        End If

        ResetFieldsTab()
        CreateTempDataBud()

    End Sub

    Protected Sub dgvActDet_RowDataBound(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Protected Sub dgvActDet_RowCreated(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Private Sub dgvBudMon_SelectedIndexChanged(sender As Object, e As EventArgs) Handles dgvBudMon.SelectedIndexChanged

        If CDbl(dgvBudMon.SelectedRow.Cells(3).Text.ToString) <> 0 Then
            FillDetExpsActual()
        Else
            dgvActDet.DataSource = Nothing
            dgvActDet.DataBind()
            lblActAmtDet.Text = "0.00 "
            Exit Sub
        End If

    End Sub

    Private Sub GetTempDataExps()
        sql = "delete from tempexpsumtbl where user = '" & vLoggedUserID & "'"
        ExecuteNonQuery(sql)

        dt = GetDataTable("select distinct a.tc from gltranstbl a left join cctrnotbl c on a.ccno = c.ccno where " &
                          "a.transdate between '" & Format(CDate(lblDateBM1.Text), "yyyy-MM-dd") & "' " &
                          "and '" & Format(CDate(lblDateBM2.Text), "yyyy-MM-dd") & "' and a.tc between '40' and '46' and " &
                          "c.budccno = '" & cboCCenterBM.Text.Substring(0, 5) & "' and (a.tc <> '' or a.tc is not null) order by a.tc")
        If Not CBool(dt.Rows.Count) Then
            Exit Sub
        Else
            For Each dr As DataRow In dt.Rows
                Select Case dr.Item(0).ToString()
                    Case "43"

                    Case "45"
                        sql = "insert into tempexpsumtbl(docno,tc,datechange,venno,expl,dramt,user) select a.docno,a.tc,a.transdate," &
                              "'Payroll','System',sum(ifnull(a.dramt,0))-sum(ifnull(a.cramt,0)),'" & vLoggedUserID & "' from gltranstbl a " &
                              "left join cctrnotbl c on a.ccno = c.ccno where a.acctno = '" & dgvBudMon.SelectedRow.Cells(1).Text.ToString.Substring(0, 7) & "' " &
                              "and a.tc = '45' and a.transdate between '" & Format(CDate(lblDateBM1.Text), "yyyy-MM-dd") & "' and " &
                              "'" & Format(CDate(lblDateBM2.Text), "yyyy-MM-dd") & "' and c.budccno = '" & cboCCenterBM.Text.Substring(0, 5) & "' group by a.docno"
                        ExecuteNonQuery(sql)

                    Case Else
                        sql = "insert into tempexpsumtbl(docno,tc,datechange,venno,expl,dramt,user) select b.docno,b.tc,b.transdate," &
                              "b.venno,c.venname,sum(ifnull(a.dramt,0))-sum(ifnull(a.cramt,0)),'" & vLoggedUserID & "' from expdettbl a " &
                              "left join exphdrtbl b on a.docno=b.docno left join venmasttbl c on b.venno = c.venno " &
                              "left join cctrnotbl d on a.ccno = d.ccno where b.status <> 'void' and " &
                              "a.acctno = '" & dgvBudMon.SelectedRow.Cells(1).Text.ToString.Substring(0, 7) & "' and  " &
                              "b.transdate between '" & Format(CDate(lblDateBM1.Text), "yyyy-MM-dd") & "' and " &
                              "'" & Format(CDate(lblDateBM2.Text), "yyyy-MM-dd") & "' and a.tc = '" & dr.Item(0).ToString() & "' " &
                              "and d.budccno = '" & cboCCenterBM.Text.Substring(0, 5) & "' group by a.docno,a.acctno"
                        ExecuteNonQuery(sql)

                End Select
            Next

        End If

        dt.Dispose()

    End Sub

    Private Sub FillDetExpsActual()

        GetTempDataExps()

        dt = GetDataTable("select sum(ifnull(dramt,0)) from tempexpsumtbl where user = '" & vLoggedUserID & "' group by user")
        If Not CBool(dt.Rows.Count) Then
            MsgBox("No Detailed Found")
            dgvActDet.DataSource = Nothing
            dgvActDet.DataBind()
            lblActAmtDet.Text = "0.00 "
            Exit Sub
        Else
            For Each dr As DataRow In dt.Rows
                lblActAmtDet.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0.00 ; (#,##0.00)") & " "

            Next

        End If

        dt.Dispose()

        Dim adapter As New MySqlDataAdapter
        Dim ds As New DataSet()
        Dim i As Integer = 0
        sqldata = Nothing

        sqldata = "select docno,tc,datechange,concat(venno,space(1),expl) as ven,ifnull(dramt,0) as amt " &
                  "from tempexpsumtbl where user = '" & vLoggedUserID & "' order by datechange"

        With dgvActDet
            .Columns(0).HeaderText = "Doc No."
            .Columns(1).HeaderText = "TC"
            .Columns(2).HeaderText = "Date"
            .Columns(3).HeaderText = "Payee"
            .Columns(4).HeaderText = "Amount"

        End With

        conn.Open()
        Dim command As New MySqlCommand(sqldata, conn)
        adapter.SelectCommand = command
        adapter.Fill(ds)
        adapter.Dispose()
        command.Dispose()
        conn.Close()
        dgvActDet.DataSource = ds.Tables(0)
        dgvActDet.DataBind()

    End Sub

End Class