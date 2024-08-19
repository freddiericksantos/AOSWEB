Imports MySql.Data.MySqlClient

Public Class VoidApproval
    Inherits System.Web.UI.Page
    Dim dt As DataTable
    Dim sql As String
    Dim MyDA_com_sections As New MySqlDataAdapter
    Dim MyDataSet As New DataSet
    Dim MySqlScript As String
    Dim gFormNo As String
    Dim Answer As String
    Dim admCnld As String
    Dim sqldata As String
    Dim cellIndex As Integer
    Dim rowIndex As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If lblUser.Text Is Nothing Then
            Response.Redirect("login.aspx")

        End If

        If Not Me.IsPostBack Then
            lblUser.Text = Session("UserID")
            lblGrpUser.Text = Session("UserGrp")

            lblTitle.Text = TabPanel1.HeaderText
            popDocTypeTC()

        End If

    End Sub

    Protected Sub AdmMsgBox(ByVal sMessage As String)
        Dim msg As String
        msg = "<script language='javascript'>"
        msg += "alert('" & sMessage & "');"
        msg += "<" & "/script>"
        Response.Write(msg)
    End Sub

    Private Sub popDocTypeTC()
        cboDocTypeStat.Items.Clear()
        dt = GetDataTable("select concat(c.tc,space(1),d.document) from sys_usergrouprights a left join sys_forms b on a.gr_formcode=b.code " &
                          "left join voidreqtbl c on b.tc=c.tc left join tccodestbl d on b.tc=d.tc where a.gr_groupcode = '" & lblGrpUser.Text & "' and " &
                          "a.gr_releaserights = " & CLng(1) & " and b.tc is not null and c.reqstat = 'Park' group by c.tc")
        If Not CBool(dt.Rows.Count) Then
            lblMsg.Text = "No Park Documment or Authorization Found"
            Exit Sub

        Else
            cboDocTypeStat.Items.Add("")
            For Each dr As DataRow In dt.Rows
                cboDocTypeStat.Items.Add(dr.Item(0).ToString())

            Next

        End If

        dt.Dispose()


    End Sub

    Protected Sub lbNew_Click(sender As Object, e As EventArgs)

        Select Case TabContainer1.ActiveTabIndex
            Case 0
                dgvAppList.SelectedIndex = -1

            Case 1

        End Select

    End Sub

    Protected Sub lbSave_Click(sender As Object, e As EventArgs)

    End Sub

    Protected Sub lbDelete_Click(sender As Object, e As EventArgs)

    End Sub

    Protected Sub lbPrint_Click(sender As Object, e As EventArgs)

    End Sub

    Protected Sub lbClose_Click(sender As Object, e As EventArgs)
        Response.Redirect("Administrator.aspx")

    End Sub

    Protected Sub dgvAppList_RowCreated(sender As Object, e As GridViewRowEventArgs)

    End Sub



    Private Sub cboDocTypeStat_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboDocTypeStat.SelectedIndexChanged
        If cboDocTypeStat.Text = "" Then
            Exit Sub
        End If

        filldgvParkList()


    End Sub

    Private Sub filldgvParkList()

        dt = GetDataTable("select * from voidreqtbl where tc = '" & cboDocTypeStat.Text.Substring(0, 2) & "' and " &
                          "reqstat = 'PARK' order by vdocno limit 1")
        If Not CBool(dt.Rows.Count) Then
            dgvAppList.DataSource = Nothing
            dgvAppList.DataBind()
            Exit Sub

        End If

        Dim adapter As New MySqlDataAdapter
        Dim ds As New DataSet()

        sqldata = Nothing

        sqldata = "select a.vdocno,a.docno,a.tc,a.transdate,a.user,a.remarks,a.reqstat,ifnull(a.appby,'Not Yet Approved') as appby,b.module " &
                  "from voidreqtbl a left join tccodestbl b on a.tc=b.tc where a.tc = '" & cboDocTypeStat.Text.Substring(0, 2) & "' " &
                  "and a.reqstat = 'PARK' order by vdocno"

        With dgvAppList
            .Columns(2).HeaderText = "Reqt No."
            .Columns(3).HeaderText = "Doc. No."
            .Columns(4).HeaderText = "TC"
            .Columns(5).HeaderText = "Date Reqt"
            .Columns(6).HeaderText = "Reqt By"
            .Columns(7).HeaderText = "Reason"
            .Columns(8).HeaderText = "Status"
            .Columns(9).HeaderText = "Appr By"
            .Columns(10).HeaderText = "Module"

        End With

        conn.Open()
        Dim command As New MySqlCommand(sqldata, conn)
        adapter.SelectCommand = command
        adapter.Fill(ds)
        adapter.Dispose()
        command.Dispose()
        conn.Close()
        dgvAppList.DataSource = ds.Tables(0)
        dgvAppList.DataBind()

    End Sub

    Private Sub dgvAppList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles dgvAppList.SelectedIndexChanged

        'lblMsg.Text = dgvAppList.Rows(rowIndex).Cells(2).Text

    End Sub

    Private Sub dgvAppList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgvAppList.RowCommand

        Select Case e.CommandName
            Case "Select1"
                rowIndex = Convert.ToInt32(e.CommandArgument)
                Dim selectedRow As GridViewRow = dgvAppList.Rows(rowIndex)
                cellIndex = GetCellIndex(selectedRow, DirectCast(e.CommandSource, Control))
                PostVoidProc()

                'Case "Select2"
                '    rowIndex = Convert.ToInt32(e.CommandArgument)
                '    Dim selectedRow As GridViewRow = dgvAppList.Rows(rowIndex)
                '    cellIndex = GetCellIndex(selectedRow, DirectCast(e.CommandSource, Control))
                '    CancelVoidProc()

        End Select

    End Sub

    Private Sub CancelVoidProc()

        sql = "update voidreqtbl set reqstat = 'CANCEL',appdate = '" & Format(CDate(Now()), "yyyy-MM-dd HH:mm:ss") & "'," &
              "appby = '" & lblUser.Text & "' where vdocno = '" & dgvAppList.Rows(rowIndex).Cells(2).Text & "'"
        ExecuteNonQuery(sql)

        lblMsg.Text = "Void Req No. " & dgvAppList.Rows(rowIndex).Cells(2).Text & " CANCELLED "

        filldgvParkList()

    End Sub


    Private Function GetCellIndex(ByVal row As GridViewRow, ByVal control As Control) As Integer
        For i As Integer = 0 To row.Cells.Count - 1
            If row.Cells(i).Controls.Contains(control) Then
                Return i
            End If
        Next

        Return -1

    End Function

    Private Sub PostVoidProc()
        dt = GetDataTable("select ifnull(jvno,'No JV') as adm from gltranstbl where docno = '" & dgvAppList.Rows(rowIndex).Cells(3).Text & "' " &
                          "and tc = '" & dgvAppList.Rows(rowIndex).Cells(4).Text & "' group by adm")
        If Not CBool(dt.Rows.Count) Then

        Else
            For Each dr As DataRow In dt.Rows
                Select Case dr.Item(0).ToString()
                    Case "No JV"

                    Case Else
                        AdmMsgBox("VOID Not Possible, due to GL Transaction already Process: JV No. " & dr.Item(0).ToString())
                        Exit Sub
                End Select

            Next

        End If

        dt.Dispose()

        Select Case dgvAppList.Rows(rowIndex).Cells(4).Text
            Case "10"
                vJVsource = "MMRR"
                PostVoidTC10()

            Case "42"
                vJVsource = "Voucher Payables"
                PostVoidTC42()

            Case "43"
                vJVsource = "Check Voucher"
                PostVoidTC43()

            Case "53"
                vJVsource = "Sales"
                PostVoidTC53()

        End Select

        If admCnld = "Stop" Then
            AdmMsgBox("VOID Reqt No. " & dgvAppList.Rows(rowIndex).Cells(2).Text & " Action ABORTED")
            Exit Sub

        Else
            sql = "update voidreqtbl set reqstat = 'POSTED',appdate = '" & Format(CDate(Now()), "yyyy-MM-dd HH:mm:ss") & "'," &
                  "appby = '" & lblUser.Text & "' where vdocno = '" & dgvAppList.Rows(rowIndex).Cells(2).Text & "'"
            ExecuteNonQuery(sql)

            lblMsg.Text = "Void Req No. " & dgvAppList.Rows(rowIndex).Cells(2).Text & " POSTED "

        End If

        filldgvParkList()

    End Sub

    Private Sub PostVoidTC10()
        dt = GetDataTable("select ifnull(invstat,'OPEN'),ifnull(vpno,'NoVP'),mov from invhdrtbl where " &
                          "mmrrno = '" & dgvAppList.Rows(rowIndex).Cells(3).Text & "' and tc = '10'")
        If Not CBool(dt.Rows.Count) Then
            admCnld = "Stop"
            Exit Sub
        Else
            For Each dr As DataRow In dt.Rows
                Select Case dr.Item(0).ToString()
                    Case "OPEN"
                        'Answer = MessageBox.Show("Are you sure to Void MMRR No.:" & dgvAppList.Rows(rowIndex).Cells(3).Text & "?", lblFormName.Text, MessageBoxButtons.YesNo)
                        'If Answer = vbYes Then

                        sql = "update invhdrtbl set status = 'void',dsrstat = 'open',invstat = 'VOID' where mmrrno = '" & dgvAppList.Rows(rowIndex).Cells(3).Text & "'"
                        ExecuteNonQuery(sql)

                        Select Case dr.Item(1).ToString() ' Vendors Deposit
                            Case "NoVP"

                            Case Else
                                sql = "update exphdrtbl set mmrrno = null where docno = '" & dr.Item(1).ToString() & "' and tc = '42'"
                                ExecuteNonQuery(sql)

                                sql = "update expdettbl set depstat = 'open' where docno ='" & dr.Item(1).ToString() & "' and acctno ='1141300' " &
                                          "and mmrrno = '" & dgvAppList.Rows(rowIndex).Cells(3).Text & "'"
                                ExecuteNonQuery(sql)

                        End Select

                        Select Case dr.Item(2).ToString()
                            Case "101"
                                sql = "update invdettbl set pono = null where mmrrno = '" & dgvAppList.Rows(rowIndex).Cells(3).Text & "'"
                                ExecuteNonQuery(sql)

                        End Select

                        'void GL entry
                        sql = "delete from gltranstbl where docno = '" & dgvAppList.Rows(rowIndex).Cells(3).Text & "' and tc = '10'  and " &
                              "jvsource = '" & vJVsource & "'"
                        ExecuteNonQuery(sql)

                        'vLastAct = Me.Text & Space(1) & " VOID MMRR No." & dgvAppList.Rows(rowIndex).Cells(3).Text
                        'WriteToLogs(vLastAct)

                        AdmMsgBox("MMRR No.:" & dgvAppList.Rows(rowIndex).Cells(3).Text & " SUCCESSFULLY VOID")

                        admCnld = "Go"

                        'Else
                        '    admCnld = "Stop"
                        '    Exit Sub
                        'End If

                End Select
            Next

        End If

        dt.Dispose()

    End Sub

    Private Sub PostVoidTC53()
        'Answer = MessageBox.Show("Are you sure to Void Invoice No.:" & dgvAppList.Rows(rowIndex).Cells(3).Text & "?", lblFormName.Text, MessageBoxButtons.YesNo)
        'If Answer = vbYes Then
        'VOID INVHDR
        sql = "update saleshdrtbl set status = 'void' where invno = '" & dgvAppList.Rows(rowIndex).Cells(3).Text & "'"
        ExecuteNonQuery(sql)


        'void invdet
        sql = "update salesdettbl set status = 'void' where invno = '" & dgvAppList.Rows(rowIndex).Cells(3).Text & "'"
        ExecuteNonQuery(sql)


        'reset or ref
        sql = "update coldettbl set refno = orno, tc = '60' where refno = '" & dgvAppList.Rows(rowIndex).Cells(3).Text & "' and " &
              "tc = '53'"
        ExecuteNonQuery(sql)


        'reset do
        sql = "update isshdrtbl set dsrno = '00000', dsrstat = 'open' where dsrno = '" & dgvAppList.Rows(rowIndex).Cells(3).Text & "'"
        ExecuteNonQuery(sql)


        'GL ENTRY
        sql = "delete from gltranstbl where docno = '" & dgvAppList.Rows(rowIndex).Cells(3).Text & "' and " &
              "tc = '53' and jvsource = '" & vJVsource & "'"
        ExecuteNonQuery(sql)

        admCnld = "Go"



        'Else
        '    admCnld = "Stop"
        '    Exit Sub
        'End If

    End Sub

    Private Sub PostVoidTC43()
        dt = GetDataTable("select * from cvhdrtbl where docno = '" & dgvAppList.Rows(rowIndex).Cells(3).Text & "' and tc = '43' and " &
                          "glstat is null")
        If Not CBool(dt.Rows.Count) Then
            AdmMsgBox("Doc. No. " & dgvAppList.Rows(rowIndex).Cells(3).Text & " cannot VOID due to: GL Period is alredy Close")
            admCnld = "Stop"
            Exit Sub

        Else
            'Answer = MessageBox.Show("Are you sure to VOID CV No. " & dgvAppList.Rows(rowIndex).Cells(3).Text & "?", lblFormName.Text, MessageBoxButtons.YesNo)
            'If Answer = vbYes Then

            sql = "update cvhdrtbl set status = 'void' where docno = '" & dgvAppList.Rows(rowIndex).Cells(3).Text & "' and tc = '43'"
            ExecuteNonQuery(sql)

            sql = "update cvdettbl set status = 'void' where docno = '" & dgvAppList.Rows(rowIndex).Cells(3).Text & "' and tc = '43'"
            ExecuteNonQuery(sql)

            sql = "update exphdrtbl a,(select vpno,tcref from cvdettbl where docno = '" & dgvAppList.Rows(rowIndex).Cells(3).Text & "' " &
                  "and tc='43' and entryfr ='det') as b set a.cvstat = null,a.cvno = null,a.vpbals=a.docamt where " &
                  "a.docno = b.vpno and a.tc = b.tcref"
            ExecuteNonQuery(sql)

            sql = "delete from gltranstbl where docno = '" & dgvAppList.Rows(rowIndex).Cells(3).Text & "' " &
                  "and tc = '43' and jvsource = '" & vJVsource & "'"
            ExecuteNonQuery(sql)

            admCnld = "Go"

            'vLastAct = Me.Text & " VOID CV No." & dgvAppList.Rows(rowIndex).Cells(3).Text & Space(1)
            'WriteToLogs(vLastAct)

            'Else
            '    admCnld = "Stop"
            '    Exit Sub

            'End If

            dt.Dispose()

        End If

    End Sub

    Private Sub PostVoidTC42()
        dt = GetDataTable("select * from exphdrtbl where docno = '" & dgvAppList.Rows(rowIndex).Cells(3).Text & "' " &
                          "and tc = '" & dgvAppList.Rows(rowIndex).Cells(2).Text & "' and cvstat is null")
        If Not CBool(dt.Rows.Count) Then
            AdmMsgBox("Doc. No. " & dgvAppList.Rows(rowIndex).Cells(3).Text & " cannot be VOIDED due to: payment already made")
            admCnld = "Stop"
            Exit Sub

        Else
            'Answer = MessageBox.Show("Are you sure to VOID Doc No. " & dgvAppList.Rows(rowIndex).Cells(3).Text & "?", lblFormName.Text, MessageBoxButtons.YesNo)
            'If Answer = vbYes Then
            sql = "update exphdrtbl set status = 'void' where docno = '" & dgvAppList.Rows(rowIndex).Cells(3).Text & "' " &
                  "and tc = '" & dgvAppList.Rows(rowIndex).Cells(4).Text & "'"
            ExecuteNonQuery(sql)


            sql = "update expdettbl set status = 'void' where docno = '" & dgvAppList.Rows(rowIndex).Cells(3).Text & "' " &
                  "and tc = '" & dgvAppList.Rows(rowIndex).Cells(4).Text & "'"
            ExecuteNonQuery(sql)


            sql = "update expdettbl set depstat = 'void' where docno = '" & dgvAppList.Rows(rowIndex).Cells(3).Text & "' and " &
                  "tc = '" & dgvAppList.Rows(rowIndex).Cells(4).Text & "' and acctno = '1145000'"
            ExecuteNonQuery(sql)


            sql = "delete from gltranstbl where docno = '" & dgvAppList.Rows(rowIndex).Cells(3).Text & "' and " &
                  "tc = '" & dgvAppList.Rows(rowIndex).Cells(4).Text & "'"
            ExecuteNonQuery(sql)


            dt = GetDataTable("select reflabel,mmrrno from exphdrtbl where docno = '" & dgvAppList.Rows(rowIndex).Cells(3).Text & "' " &
                              "and tc = '" & dgvAppList.Rows(rowIndex).Cells(4).Text & "'")
            If Not CBool(dt.Rows.Count) Then

            Else
                For Each dr As DataRow In dt.Rows
                    Select Case dr.Item(0).ToString()
                        Case "MMRR"
                            sql = "update invhdrtbl set invstat = null where mmrrno = '" & dr.Item(1).ToString() & "'"
                            ExecuteNonQuery(sql)

                        Case "Fund Repl"
                            sql = "update fundrepltbl set vpno = null,status = 'unpaid',changeby = '" & lblUser.Text & "'," &
                                  "postingdate = '" & Format(CDate(Now), "yyyy-MM-dd") & "' where replno = '" & dr.Item(1).ToString() & "'"
                            ExecuteNonQuery(sql)

                    End Select
                Next

            End If

            admCnld = "Go"

            AdmMsgBox("Doc. No. " & dgvAppList.Rows(rowIndex).Cells(3).Text & " VOID")

            'vLastAct = Me.Text & " VOID Doc No." & dgvAppList.Rows(rowIndex).Cells(3).Text & Space(1) & dgvAppList.Rows(rowIndex).Cells(4).Text & Space(1)
            'WriteToLogs(vLastAct)

            'Else
            '    admCnld = "Stop"
            '    Exit Sub

            'End If

            dt.Dispose()

        End If

    End Sub

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged, RadioButton2.CheckedChanged, RadioButton3.CheckedChanged
        If dpTransDate.Text = Nothing Then
            Exit Sub
        ElseIf dpTransDate2.Text = Nothing Then
            Exit Sub
        ElseIf CDate(dpTransDate.Text) > CDate(dpTransDate2.Text) Then
            Exit Sub
        End If

        Select Case True
            Case RadioButton1.Checked
                popDocTypeFI()

            Case RadioButton2.Checked
                popDocTypeMM()

            Case RadioButton3.Checked
                popDocTypeSI()

        End Select


    End Sub

    Private Sub popDocTypeFIinitialLoad()
        cboDocTypeT4.Items.Clear()

        dt = GetDataTable("select concat(c.tc,space(1),d.document) from sys_usergrouprights a left join sys_forms b on a.gr_formcode=b.code " &
                          "left join voidreqtbl c on c.tc=b.tc left join tccodestbl d on c.tc=d.tc where left(c.pdate,10) between " &
                          "'" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                          "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and " &
                          "a.gr_deleterights = " & CLng(1) & " group by c.tc order by d.document")
        If Not CBool(dt.Rows.Count) Then

        Else
            cboDocTypeT4.Items.Add("")
            cboDocTypeT4.Items.Add("ALL")
            For Each dr As DataRow In dt.Rows
                cboDocTypeT4.Items.Add(dr.Item(0).ToString())

            Next

        End If

        Call dt.Dispose()

    End Sub

    Private Sub popDocTypeMM()


        cboDocTypeT4.Items.Clear()
        dt = GetDataTable("select distinct tc from voidreqtbl where left(pdate,10) between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                           "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and tc = '10' limit 1")
        If Not CBool(dt.Rows.Count) Then
            cboDocTypeT4.Items.Remove("10 MMRR")
        Else
            cboDocTypeT4.Items.Add("")
            For Each dr As DataRow In dt.Rows
                cboDocTypeT4.Items.Add("10 MMRR")
            Next
        End If

        dt.Dispose()

    End Sub

    Private Sub popDocTypeSI()

        cboDocTypeT4.Items.Clear()
        dt = GetDataTable("select distinct tc from voidreqtbl where left(pdate,10) between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                           "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and tc = '53' limit 1")
        If Not CBool(dt.Rows.Count) Then
            cboDocTypeT4.Items.Remove("53 Sales Invoice")
        Else
            cboDocTypeT4.Items.Add("")
            For Each dr As DataRow In dt.Rows
                cboDocTypeT4.Items.Add("53 Sales Invoice")
            Next
        End If

        dt.Dispose()

    End Sub

    Private Sub popDocTypeFI()
        cboDocTypeT4.Items.Clear()
        dt = GetDataTable("select tc from voidreqtbl where left(pdate,10) between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                           "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' group by tc")
        If Not CBool(dt.Rows.Count) Then
            Exit Sub
        Else
            cboDocTypeT4.Items.Add("")
            For Each dr As DataRow In dt.Rows
                Select Case dr.Item(0).ToString()
                    Case "40"
                        cboDocTypeT4.Items.Add("40 Petty Cash Voucher")
                    Case "41"
                        cboDocTypeT4.Items.Add("41 Weekly Expense Report")
                    Case "42"
                        cboDocTypeT4.Items.Add("42 Voucher Payable")
                    Case "43"
                        cboDocTypeT4.Items.Add("43 Check Voucher")
                    Case "44"
                        cboDocTypeT4.Items.Add("44 Revolving Fund Voucher")
                    Case "48"
                        cboDocTypeT4.Items.Add("48 Gas PO Payment")
                    Case "49"
                        cboDocTypeT4.Items.Add("49 Fund Replenishment")

                    Case "60"
                        cboDocTypeT4.Items.Add("60 Collection")
                    Case "65"
                        cboDocTypeT4.Items.Add("65 CDCR")
                    Case "90"
                        cboDocTypeT4.Items.Add("90 Cust Credit Memo")
                    Case "92"
                        cboDocTypeT4.Items.Add("92 Cust Debit Memo")

                End Select

            Next

        End If

        Call dt.Dispose()


    End Sub

    Private Sub dpTransDate_TextChanged(sender As Object, e As EventArgs) Handles dpTransDate.TextChanged, dpTransDate2.TextChanged
        If dpTransDate.Text = Nothing Then
            Exit Sub
        ElseIf dpTransDate2.Text = Nothing Then
            Exit Sub
        ElseIf CDate(dpTransDate.Text) > CDate(dpTransDate2.Text) Then
            Exit Sub
        End If

        popDocTypeFIinitialLoad()

    End Sub

    Private Sub cboDocTypeT4_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboDocTypeT4.SelectedIndexChanged

        If dpTransDate.Text = Nothing Then
            Exit Sub
        ElseIf dpTransDate2.Text = Nothing Then
            Exit Sub
        ElseIf CDate(dpTransDate.Text) > CDate(dpTransDate2.Text) Then
            Exit Sub
        ElseIf cboDocTypeT4.Text = "" Then
            Exit Sub
        End If

        filldgvStatList()

    End Sub

    Private Sub filldgvStatList()
        Dim adapter As New MySqlDataAdapter
        Dim ds As New DataSet()

        sqldata = Nothing

        Select Case cboDocTypeT4.Text
            Case "ALL"
                dt = GetDataTable("select * from voidreqtbl where left(pdate,10) between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "'")
                If Not CBool(dt.Rows.Count) Then
                    dgvStatList.DataSource = Nothing
                    dgvStatList.DataBind()
                    Exit Sub

                End If

                sqldata = "select a.vdocno,a.docno,a.tc,a.transdate,a.user,a.remarks,a.reqstat,ifnull(a.appby,'Not Yet Approved') as appby,b.module " &
                          "from voidreqtbl a left join tccodestbl b on a.tc=b.tc where left(a.pdate,10) between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                          "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' order by a.vdocno"

            Case Else
                dt = GetDataTable("select * from voidreqtbl where left(pdate,10) between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and tc = '" & cboDocTypeT4.Text.Substring(0, 2) & "'")
                If Not CBool(dt.Rows.Count) Then
                    dgvStatList.DataSource = Nothing
                    dgvStatList.DataBind()
                    Exit Sub

                End If

                sqldata = "select a.vdocno,a.docno,a.tc,a.transdate,a.user,a.remarks,a.reqstat,ifnull(a.appby,'Not Yet Approved') as appby,b.module " &
                          "from voidreqtbl a left join tccodestbl b on a.tc=b.tc where left(a.pdate,10) between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                          "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and a.tc = '" & cboDocTypeT4.Text.Substring(0, 2) & "' order by a.vdocno"
        End Select

        With dgvStatList
            .Columns(0).HeaderText = "Reqt No."
            .Columns(1).HeaderText = "Doc. No."
            .Columns(2).HeaderText = "TC"
            .Columns(3).HeaderText = "Date Reqt"
            .Columns(4).HeaderText = "Reqt By"
            .Columns(5).HeaderText = "Reason"
            .Columns(6).HeaderText = "Status"
            .Columns(7).HeaderText = "Appr By"
            .Columns(8).HeaderText = "Module"

        End With

        conn.Open()
        Dim command As New MySqlCommand(sqldata, conn)
        adapter.SelectCommand = command
        adapter.Fill(ds)
        adapter.Dispose()
        command.Dispose()
        conn.Close()
        dgvStatList.DataSource = ds.Tables(0)
        dgvStatList.DataBind()

    End Sub

    Protected Sub dgvStatList_RowDataBound(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Protected Sub dgvStatList_RowCreated(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Private Sub TabContainer1_ActiveTabChanged(sender As Object, e As EventArgs) Handles TabContainer1.ActiveTabChanged

        Select Case TabContainer1.ActiveTabIndex
            Case 0
                lblTitle.Text = TabPanel1.HeaderText
            Case 1
                lblTitle.Text = TabPanel2.HeaderText

        End Select
    End Sub

    Protected Sub dgvAppList_RowDeleting(sender As Object, e As GridViewDeleteEventArgs)

    End Sub

    Protected Sub OnRowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim item As String = e.Row.Cells(2).Text
            For Each button As Button In e.Row.Cells(1).Controls.OfType(Of Button)()
                If button.CommandName = "Delete" Then
                    button.Attributes("onclick") = "if(!confirm('Are you sure to CANCEL Req. No. " + item + "?')){ return false; };"
                End If
            Next
        End If
    End Sub

    Protected Sub OnRowDeleting(sender As Object, e As GridViewDeleteEventArgs)

        CancelVoidProc()

    End Sub

End Class