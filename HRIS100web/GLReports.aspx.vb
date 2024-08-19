Imports MySql.Data.MySqlClient
Public Class GLReportsNew
    Inherits System.Web.UI.Page
    Dim dt As DataTable
    Dim sql As String
    Dim admCCno As String
    Dim admExcelFile As String
    Dim admBook1 As String = "C:\updater\Book1.xlsx"
    Dim sqldata As String
    Dim admDesc As String
    Dim vDate1 As Date
    Dim vDate2 As Date
    Dim vDateY1 As Date
    Dim begAmt As Double = 0
    Dim drAmt As Double = 0
    Dim crAmt As Double = 0
    Dim balAmt As Double = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If lblUser.Text Is Nothing Then
            Response.Redirect("login.aspx")

        End If

        If Not Me.IsPostBack Then
            lblUser.Text = Session("UserID")
            lblGrpUser.Text = Session("UserGrp")
            lblTitle.Text = TabPanel1.HeaderText
            GetTransMon()
        End If


    End Sub

    Private Sub GetTransMon()

        cboFSmonYear.Items.Clear()
        'cboFSmonYear2.Items.Clear()
        'cboFSmonYear3.Items.Clear()
        dt = GetDataTable("select distinct transdate from coshdrtbl order by transdate desc")
        If Not CBool(dt.Rows.Count) Then
            MsgBox("No Vendor Found")
            Exit Sub
        Else
            cboFSmonYear.Items.Add("")
            cboFSmonYear2.Items.Add("")
            'cboFSmonYear3.Items.Add("")
            For Each dr As DataRow In dt.Rows
                cboFSmonYear.Items.Add(Format(CDate(dr.Item(0).ToString()), "MMM yyyy"))
                cboFSmonYear2.Items.Add(Format(CDate(dr.Item(0).ToString()), "MMM yyyy"))
                'cboFSmonYear3.Items.Add(Format(CDate(dr.Item(0).ToString()), "MMM yyyy"))
            Next

        End If

        Call dt.Dispose()

    End Sub

    Protected Sub lbClose_Click(sender As Object, e As EventArgs)
        Response.Redirect("FinancialAccounting.aspx")
    End Sub

    Private Sub TabContainer1_ActiveTabChanged(sender As Object, e As EventArgs) Handles TabContainer1.ActiveTabChanged

        Select Case TabContainer1.ActiveTabIndex
            Case 0
                lblTitle.Text = TabPanel1.HeaderText
            Case 1
                lblTitle.Text = TabPanel2.HeaderText
            Case 2
                lblTitle.Text = TabPanel3.HeaderText
        End Select

    End Sub

    Private Sub cboFSmonYear_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboFSmonYear.SelectedIndexChanged
        If cboFSmonYear.Text = "" Then
            Exit Sub
        ElseIf rbMon.Checked = False And rbYr.Checked = False Then
            Exit Sub
        End If

        lblTBdate1.Text = Format(CDate(FirstDayOfMonth(cboFSmonYear.Text)), "yyyy-MM-dd")
        lblTBdate2.Text = Format(CDate(LastDayOfMonth(cboFSmonYear.Text)), "yyyy-MM-dd")
        lblTBFirtDay.Text = Format(CDate(cboFSmonYear.Text), "yyyy") & "-01-01"

        FillTBals()


    End Sub

    Private Sub rbMon_CheckedChanged(sender As Object, e As EventArgs) Handles rbMon.CheckedChanged, rbYr.CheckedChanged
        If cboFSmonYear.Text = "" Then
            Exit Sub
        ElseIf rbMon.Checked = False And rbYr.Checked = False Then
            Exit Sub
        End If

        lblTBdate1.Text = Format(CDate(FirstDayOfMonth(cboFSmonYear.Text)), "yyyy-MM-dd")
        lblTBdate2.Text = Format(CDate(LastDayOfMonth(cboFSmonYear.Text)), "yyyy-MM-dd")
        lblTBFirtDay.Text = Format(CDate(cboFSmonYear.Text), "yyyy") & "-01-01"

        FillTBals()

    End Sub

    Private Sub FillTBals()
        InitTab1()

        vDate1 = Format(CDate(lblTBdate1.Text), "yyyy-MM-dd")
        vDate2 = Format(CDate(lblTBdate2.Text), "yyyy-MM-dd")
        vDateY1 = Format(CDate(lblTBFirtDay.Text), "yyyy-MM-dd")

        sql = "delete from tempglmaintbl where user = '" & vLoggedUserID & "'"
        ExecuteNonQuery(sql)

        Select Case True
            Case rbMon.Checked


            Case rbYr.Checked
                'insert beg bals
                sql = "insert into tempglmaintbl(acctno,acctdesc,begamt,user) select a.acctno,b.acctdesc," &
                      "ifnull((sum(a.dramt)-sum(a.cramt)),0),'" & vLoggedUserID & "' FROM glmaintranstbl a " &
                      "left join acctcharttbl b on a.acctno=b.acctno where (a.dramt <> 0 or a.cramt <> 0) and " &
                      "a.dfrom < '" & Format(CDate(vDate1), "yyyy-MM-dd") & "' group by a.acctno,a.ccno"
                ExecuteNonQuery(sql) 'a.monstat = '" & "close" & "'and

        End Select

        sql = "insert into tempglmaintbl(acctno,acctdesc,dramt,cramt,user) " &
              "SELECT a.acctno,b.acctdesc,ifnull(sum(a.dramt),0) as dramt,ifnull(sum(a.cramt),0) as cramt,'" & vLoggedUserID & "' " &
              "FROM glmaintranstbl a left join acctcharttbl b on a.acctno=b.acctno where a.branch = '" & vLoggedBranch & "' " &
              "and a.dfrom = '" & Format(CDate(vDate1), "yyyy-MM-dd") & "' " &
              "and a.dto = '" & Format(CDate(vDate2), "yyyy-MM-dd") & "' group by a.acctno order by a.acctno"
        ExecuteNonQuery(sql)

        sql = "update tempglmaintbl set balamt = ifnull(begamt,0) + ifnull(dramt,0) - ifnull(cramt,0) where user = '" & vLoggedUserID & "'"
        ExecuteNonQuery(sql)

        dt = GetDataTable("select * from tempglmaintbl where user = '" & lblUser.Text & "'")
        If Not CBool(dt.Rows.Count) Then
            lblMsg.Text = "Error: No Record Found"
            InitTab1()

        End If

        dt.Dispose()

        Dim adapter As New MySqlDataAdapter
        Dim ds As New DataSet()

        sqldata = Nothing

        sqldata = "select concat(acctno,space(1),acctdesc) as acctdesc,sum(ifnull(begamt,0)) as begamt," &
                  "sum(ifnull(dramt,0)) as dramt,sum(ifnull(cramt,0)) as cramt,sum(ifnull(balamt,0)) as balamt " &
                  "from tempglmaintbl where user = '" & lblUser.Text & "' group by acctno order by acctno"

        conn.Open()
        Dim command As New MySqlCommand(sqldata, conn)
        adapter.SelectCommand = command
        adapter.Fill(ds)
        adapter.Dispose()
        command.Dispose()
        conn.Close()
        dgvTbals.DataSource = ds.Tables(0)
        dgvTbals.DataBind()


    End Sub

    Private Sub InitTab1()
        dgvTbals.DataSource = Nothing
        dgvTbals.DataBind()



    End Sub

    Protected Sub dgvTbals_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim tBegAmt As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "begamt").ToString())
            begAmt += tBegAmt

            Dim tDrAmt As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "dramt").ToString())
            drAmt += tDrAmt

            Dim tCrAmt As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "cramt").ToString())
            crAmt += tCrAmt

            Dim tBalAmt As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "balamt").ToString())
            balAmt += tBalAmt

        End If

        If e.Row.RowType = DataControlRowType.Footer Then
            Dim lblGTotal As Label = DirectCast(e.Row.FindControl("lblGTotal"), Label)
            lblGTotal.Text = "GRAND TOTAL "

            Dim lblTotalBeg As Label = DirectCast(e.Row.FindControl("lblBeg"), Label)
            lblTotalBeg.Text = Format(CDbl(begAmt.ToString()), "#,##0.00 ; (#,##0.00)")
            begAmt = 0

            Dim lblTotalDr As Label = DirectCast(e.Row.FindControl("lblDr"), Label)
            lblTotalDr.Text = Format(CDbl(Dramt.ToString()), "#,##0.00 ; (#,##0.00)")
            Dramt = 0

            Dim lblTotalCr As Label = DirectCast(e.Row.FindControl("lblCr"), Label)
            lblTotalCr.Text = Format(CDbl(Cramt.ToString()), "#,##0.00 ; (#,##0.00)")
            crAmt = 0

            Dim lblTotalEnd As Label = DirectCast(e.Row.FindControl("lblEnd"), Label)
            lblTotalEnd.Text = Format(CDbl(begAmt.ToString()), "#,##0.00 ; (#,##0.00)")
            balAmt = 0

        End If



    End Sub

    Protected Sub dgvTbals_RowCreated(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Protected Sub dgvGL_RowDataBound(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Protected Sub dgvGL_RowCreated(sender As Object, e As GridViewRowEventArgs)

    End Sub
End Class