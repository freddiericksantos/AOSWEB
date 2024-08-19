Imports MySql.Data.MySqlClient

Public Class LabMonitoring
    Inherits System.Web.UI.Page
    Dim dt As DataTable
    Dim sql As String
    Dim sqldata As String
    Dim qty As Double = 0
    Dim wt As Double = 0
    Dim amt As Double = 0
    Dim sqty As Double = 0
    Dim swt As Double = 0
    Dim samt As Double = 0
    Dim venno As Integer = 0
    Dim rowIndex As Integer = 1
    Dim strVenName As String
    Dim TotRowIndex As Integer = 1

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If lblUser.Text Is Nothing Then
            Response.Redirect("login.aspx")

        End If

        If Not Me.IsPostBack Then
            lblUser.Text = Session("UserID")
            lblGrpUser.Text = Session("UserGrp")

            lblTitle.Text = TabPanel1.HeaderText

        End If

        'select Case distinct a.mmrrno,b.mfgdate,a.codeno,a.expdate,round(ifnull(TactWt,0) - ifnull(twt,0),3) As wt from invdettbl a
        'Left Join invhdrtbl b on a.mmrrno=b.mmrrno where b.tc = '20' and b.dsrstat = 'finished';

    End Sub

    Protected Sub lbNew_Click(sender As Object, e As EventArgs)

    End Sub

    Protected Sub lbClose_Click(sender As Object, e As EventArgs)
        Response.Redirect("MaterialManagement.aspx")

    End Sub

    Protected Sub dgvLab_RowDataBound(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Protected Sub dgvLab_RowCreated(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Private Sub dpFrom_TextChanged(sender As Object, e As EventArgs) Handles dpFrom.TextChanged, dpTo.TextChanged
        lblMsg.Text = "Message Box"
        If dpFrom.Text = Nothing Then
            Exit Sub
        ElseIf dpTo.Text = Nothing Then
            Exit Sub

        End If

        If Format(CDate(dpFrom.Text), "yyyy-MM-dd") > Format(CDate(dpTo.Text), "yyyy-MM-dd") Then
            Exit Sub
        End If

        dgvLab.DataSource = Nothing
        dgvLab.DataBind()

        RefreshDataBelow()
        GetInvtryPlnt()

    End Sub

    Private Sub RefreshDataBelow()
        cboPlnt.Text = Nothing
        cboPlnt.Items.Clear()
        cboFG.Text = Nothing
        cboFG.Items.Clear()
        cboMMgrp.Text = Nothing
        cboMMgrp.Items.Clear()

        dgvLab.DataSource = Nothing
        dgvLab.DataBind()


    End Sub

    Private Sub GetInvtryPlnt()
        cboPlnt.Items.Clear()
        dt = GetDataTable("select distinct concat(a.plntno,space(1),b.plntname) from invhdrtbl a inner join plnttbl b " &
                          "on a.plntno=b.plntno where b.ownertype <> 'Company' and a.status <> 'void' and " &
                          "a.transdate between '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and " &
                          "'" & Format(CDate(dpTo.Text), "yyyy-MM-dd") & "' and a.tc ='20' and a.mov = '801'")
        If Not CBool(dt.Rows.Count) Then
            lblMsg.Text = "No Plant found."
            Exit Sub
        Else
            cboPlnt.Items.Add("")
            For Each dr As DataRow In dt.Rows
                cboPlnt.Items.Add(dr.Item(0).ToString())

            Next

        End If

        Call dt.Dispose()

    End Sub

    Private Sub cboPlnt_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboPlnt.SelectedIndexChanged
        lblMsg.Text = "Message Box"
        If dpFrom.Text = Nothing Then
            Exit Sub
        ElseIf dpTo.Text = Nothing Then
            Exit Sub

        End If

        If Format(CDate(dpFrom.Text), "yyyy-MM-dd") > Format(CDate(dpTo.Text), "yyyy-MM-dd") Then
            Exit Sub
        End If

        dgvLab.DataSource = Nothing
        dgvLab.DataBind()

        cboFG.Text = Nothing
        cboFG.Items.Clear()
        cboMMgrp.Text = Nothing
        cboMMgrp.Items.Clear()

        getMMgrp()

    End Sub

    Private Sub getMMgrp()
        'cboMMgrp
        dt = GetDataTable("select distinct b.mmgrp from invhdrtbl a left join mmasttbl b on a.pono=b.codeno " &
                          "where a.status <> 'void' and a.tc ='20' and a.mov = '801' and " &
                          "a.transdate between '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and " &
                          "'" & Format(CDate(dpTo.Text), "yyyy-MM-dd") & "'")
        If Not CBool(dt.Rows.Count) Then
            lblMsg.Text = "No Plant found."
            Exit Sub
        Else
            cboMMgrp.Items.Add("")
            cboMMgrp.Items.Add("ALL")
            For Each dr As DataRow In dt.Rows
                cboMMgrp.Items.Add(dr.Item(0).ToString())

            Next

        End If

        Call dt.Dispose()

    End Sub

    Private Sub getFG()
        Select Case cboMMgrp.Text
            Case "ALL"
                dt = GetDataTable("select distinct b.mmdesc from invhdrtbl a left join mmasttbl b on a.pono=b.codeno " &
                                  "where a.status <> 'void' and a.tc ='20' and a.mov = '801' and " &
                                  "a.transdate between '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and " &
                                  "'" & Format(CDate(dpTo.Text), "yyyy-MM-dd") & "' order by b.mmdesc")
            Case Else
                dt = GetDataTable("select distinct b.mmdesc from invhdrtbl a left join mmasttbl b on a.pono=b.codeno " &
                                  "where a.status <> 'void' and a.tc ='20' and a.mov = '801' and " &
                                  "a.transdate between '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and " &
                                  "'" & Format(CDate(dpTo.Text), "yyyy-MM-dd") & "' and b.mmgrp = '" & cboMMgrp.Text & "' " &
                                  "order by b.mmdesc")
        End Select

        If Not CBool(dt.Rows.Count) Then
            lblMsg.Text = "No Plant found."
            Exit Sub
        Else
            cboFG.Items.Add("")
            cboFG.Items.Add("ALL")
            For Each dr As DataRow In dt.Rows
                cboFG.Items.Add(dr.Item(0).ToString())

            Next

        End If

        Call dt.Dispose()
    End Sub

    Private Sub cboMMgrp_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboMMgrp.SelectedIndexChanged
        lblMsg.Text = "Message Box"
        If dpFrom.Text = Nothing Then
            Exit Sub
        ElseIf dpTo.Text = Nothing Then
            Exit Sub

        End If

        If Format(CDate(dpFrom.Text), "yyyy-MM-dd") > Format(CDate(dpTo.Text), "yyyy-MM-dd") Then
            Exit Sub
        End If

        dgvLab.DataSource = Nothing
        dgvLab.DataBind()

        cboFG.Text = Nothing
        cboFG.Items.Clear()

        getFG()

    End Sub

    Private Sub cboFG_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboFG.SelectedIndexChanged
        lblMsg.Text = "Message Box"
        If dpFrom.Text = Nothing Then
            Exit Sub
        ElseIf dpTo.Text = Nothing Then
            Exit Sub

        End If

        If Format(CDate(dpFrom.Text), "yyyy-MM-dd") > Format(CDate(dpTo.Text), "yyyy-MM-dd") Then
            Exit Sub
        End If

        Select Case cboFG.Text
            Case "ALL"
                Select Case cboMMgrp.Text
                    Case "ALL"
                        lblMsg.Text = "Select MM Group"
                        Exit Sub
                End Select

        End Select

        dgvLab.DataSource = Nothing
        dgvLab.DataBind()

        filldgvFG()


    End Sub

    Private Sub filldgvFG()
        Select Case cboFG.Text
            Case "ALL"
                lblCodeNo.Text = ""
            Case Else
                dt = GetDataTable("select codeno from mmasttbl where mmdesc = '" & cboFG.Text & "'")
                If Not CBool(dt.Rows.Count) Then
                    lblMsg.Text = "Code No not found."
                    Exit Sub
                Else
                    For Each dr As DataRow In dt.Rows
                        lblCodeNo.Text = dr.Item(0).ToString()

                    Next

                End If

                dt.Dispose()

        End Select

        Select Case cboFG.Text
            Case "ALL"
                Select Case cboMMgrp.Text
                    Case "ALL"
                        Exit Sub
                    Case Else
                        dt = GetDataTable("select distinct b.mmdesc from invhdrtbl a left join mmasttbl b on a.pono=b.codeno " &
                                          "where a.status <> 'void' and a.tc ='20' and a.mov = '801' and " &
                                          "a.transdate between '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and " &
                                          "'" & Format(CDate(dpTo.Text), "yyyy-MM-dd") & "' and b.mmgrp = '" & cboMMgrp.Text & "' limit 1")
                End Select

            Case Else
                dt = GetDataTable("select distinct b.mmdesc from invhdrtbl a left join mmasttbl b on a.pono=b.codeno " &
                                  "where a.status <> 'void' and a.tc ='20' and a.mov = '801' and " &
                                  "a.transdate between '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and " &
                                  "'" & Format(CDate(dpTo.Text), "yyyy-MM-dd") & "' and a.pono = '" & lblCodeNo.Text & "' limit 1")


        End Select

        If Not CBool(dt.Rows.Count) Then
            lblMsg.Text = "Error: No Inventory Found"
            dgvLab.DataSource = Nothing
            Exit Sub

        End If

        dt.Dispose()

        Dim adapter As New MySqlDataAdapter
        Dim ds As New DataSet()

        sqldata = Nothing

        Select Case cboFG.Text
            Case "ALL"
                Select Case cboMMgrp.Text
                    Case "ALL"
                        Exit Sub
                    Case Else
                        sqldata = "select a.mmrrno,ifnull(b.mfgdate,b.transdate) as mfgdate,a.expdate,a.codeno," &
                                  "ifnull(c.codename,c.mmdesc) as mmdesc,b.dsrno,b.batchno,(ifnull(b.tactwt,0) - ifnull(b.twt,0)) * 1000 as wt," &
                                  "ifnull(b.labstat,'On Stock') as labstat from invdettbl a left join invhdrtbl b " &
                                  "on a.mmrrno=b.mmrrno left join mmasttbl c on a.codeno=c.codeno where b.status <> 'void' and b.tc ='20' " &
                                  "and b.mov = '801' and b.transdate between '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and " &
                                  "'" & Format(CDate(dpTo.Text), "yyyy-MM-dd") & "' and c.mmgrp = '" & cboMMgrp.Text & "' " &
                                  "and (ifnull(b.tactwt,0) - ifnull(b.twt,0)) <> 0 group by a.mmrrno order by b.transdate,a.codeno,b.batchno"

                End Select

            Case Else
                sqldata = "select a.mmrrno,ifnull(b.mfgdate,b.transdate) as mfgdate,a.expdate,a.codeno," &
                          "ifnull(c.codename,c.mmdesc) as mmdesc,b.dsrno,b.batchno,(ifnull(b.tactwt,0) - ifnull(b.twt,0)) * 1000 as wt," &
                          "ifnull(b.labstat,'On Stock') as labstat from invdettbl a left join invhdrtbl b " &
                          "on a.mmrrno=b.mmrrno left join mmasttbl c on a.codeno=c.codeno where b.status <> 'void' and b.tc ='20' " &
                          "and b.mov = '801' and b.transdate between '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and " &
                          "'" & Format(CDate(dpTo.Text), "yyyy-MM-dd") & "' and b.pono = '" & lblCodeNo.Text & "' " &
                          "and (ifnull(b.tactwt,0) - ifnull(b.twt,0)) <> 0 group by a.mmrrno order by b.transdate,a.codeno,b.batchno"

        End Select

        conn.Open()
        Dim command As New MySqlCommand(sqldata, conn)
        adapter.SelectCommand = command
        adapter.Fill(ds)
        adapter.Dispose()
        command.Dispose()
        conn.Close()
        dgvLab.DataSource = ds.Tables(0)
        dgvLab.DataBind()

    End Sub

    Private Sub TabContainer1_ActiveTabChanged(sender As Object, e As EventArgs) Handles TabContainer1.ActiveTabChanged
        Select Case TabContainer1.ActiveTabIndex
            Case 0
                lblTitle.Text = TabPanel1.HeaderText
            Case 1
                lblTitle.Text = TabPanel2.HeaderText
        End Select


    End Sub

    Protected Sub dgvT2List_RowDataBound(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Protected Sub dgvT2List_RowCreated(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Private Sub btnGenerate_Click(sender As Object, e As EventArgs) Handles btnGenerate.Click
        lblMsg.Text = "Message Box"
        If dpDateT21.Text = Nothing Then
            Exit Sub
        ElseIf dpDateT22.Text = Nothing Then
            Exit Sub

        ElseIf cboMMgrpT2.Text = Nothing Or cboMMgrpT2.Text = "" Then
            lblMsg.Text = "Select MM Group"
            Exit Sub
        End If

        If Format(CDate(dpDateT21.Text), "yyyy-MM-dd") > Format(CDate(dpDateT22.Text), "yyyy-MM-dd") Then
            Exit Sub
        End If

        filldgvT2List()

    End Sub

    Private Sub filldgvT2List()
        Select Case cboMMgrpT2.Text
            Case "ALL"
                dt = GetDataTable("select distinct * from invhdrtbl where status <> 'void' and tc ='20' and mov = '801' and " &
                                  "transdate between '" & Format(CDate(dpDateT21.Text), "yyyy-MM-dd") & "' and " &
                                  "'" & Format(CDate(dpDateT22.Text), "yyyy-MM-dd") & "' and dsrstat = 'Finished' limit 1")
            Case Else
                dt = GetDataTable("select distinct c.mmgrp from invdettbl a left join invhdrtbl b on a.mmrrno = b.mmrrno " &
                                  "left join mmasttbl c on a.codeno = c.codeno where b.status <> 'void' and b.tc ='20' and " &
                                  "b.mov = '801' and b.transdate between '" & Format(CDate(dpDateT21.Text), "yyyy-MM-dd") & "' and " &
                                  "'" & Format(CDate(dpDateT22.Text), "yyyy-MM-dd") & "' and b.dsrstat = 'Finished' and " &
                                  "c.mmgrp = '" & cboMMgrpT2.Text & "' limit 1")
        End Select

        If Not CBool(dt.Rows.Count) Then
            lblMsg.Text = "Error: No Inventory Found"
            dgvT2List.DataSource = Nothing
            dgvT2List.DataBind()

            Exit Sub

        End If

        dt.Dispose()

        Dim adapter As New MySqlDataAdapter
        Dim ds As New DataSet()

        sqldata = Nothing

        Select Case cboMMgrpT2.Text
            Case "ALL"
                sqldata = "select a.mmrrno,ifnull(b.mfgdate,b.transdate) as mfgdate,a.expdate,a.codeno," &
                          "ifnull(c.codename,c.mmdesc) as mmdesc,b.dsrno,b.batchno,(ifnull(b.tactwt,0) - ifnull(b.twt,0)) * 1000 as wt," &
                          "ifnull(b.labstat,'On Stock') as labstat from invdettbl a left join invhdrtbl b " &
                          "on a.mmrrno=b.mmrrno left join mmasttbl c on a.codeno=c.codeno where b.status <> 'void' and b.tc ='20' " &
                          "and b.mov = '801' and b.transdate between '" & Format(CDate(dpDateT21.Text), "yyyy-MM-dd") & "' and " &
                          "'" & Format(CDate(dpDateT22.Text), "yyyy-MM-dd") & "' and b.dsrstat = 'Finished' " &
                          "and (ifnull(b.tactwt,0) - ifnull(b.twt,0)) <> 0 group by a.mmrrno order by b.transdate,a.codeno,b.batchno"
            Case Else
                sqldata = "select a.mmrrno,ifnull(b.mfgdate,b.transdate) as mfgdate,a.expdate,a.codeno," &
                          "ifnull(c.codename,c.mmdesc) as mmdesc,b.dsrno,b.batchno,(ifnull(b.tactwt,0) - ifnull(b.twt,0)) * 1000 as wt," &
                          "ifnull(b.labstat,'On Stock') as labstat from invdettbl a left join invhdrtbl b " &
                          "on a.mmrrno=b.mmrrno left join mmasttbl c on a.codeno=c.codeno where b.status <> 'void' and b.tc ='20' " &
                          "and b.mov = '801' and b.transdate between '" & Format(CDate(dpDateT21.Text), "yyyy-MM-dd") & "' and " &
                          "'" & Format(CDate(dpDateT22.Text), "yyyy-MM-dd") & "' and b.dsrstat = 'Finished' " &
                          "and (ifnull(b.tactwt,0) - ifnull(b.twt,0)) <> 0 and c.mmgrp = '" & cboMMgrpT2.Text & "' " &
                          "group by a.mmrrno order by b.transdate,a.codeno,b.batchno"
        End Select


        conn.Open()
        Dim command As New MySqlCommand(sqldata, conn)
        adapter.SelectCommand = command
        adapter.Fill(ds)
        adapter.Dispose()
        command.Dispose()
        conn.Close()
        dgvT2List.DataSource = ds.Tables(0)
        dgvT2List.DataBind()

        lbSave.Enabled = True

    End Sub

    Private Sub CheckBox2_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox2.CheckedChanged
        If dgvT2List.Rows.Count = 0 Then
            Exit Sub
        End If

        If CheckBox2.Checked = True Then
            CheckBox2.Text = "UnSelect All"
        Else
            CheckBox2.Text = "Select All"
        End If

        GetCheckBoxValues()

    End Sub

    Protected Sub GetCheckBoxValues()
        For Each row As GridViewRow In dgvT2List.Rows
            ' Find the CheckBox in the row
            Dim checkBox As CheckBox = TryCast(row.FindControl("CheckBox1"), CheckBox)
            If CheckBox2.Checked = True Then
                checkBox.Checked = True
            Else
                checkBox.Checked = False
            End If

            'If checkBox IsNot Nothing AndAlso checkBox.Checked Then
            '    ' Get the value from the row where the CheckBox is checked
            '    ' For example, you can access other column values in the same row:
            '    Dim otherColumnValue As String = row.Cells(1).Text ' Replace '1' with the appropriate column index
            '    ' Do something with the checked checkbox and other column values
            'End If
        Next

    End Sub

    Private Sub lbSave_Click(sender As Object, e As EventArgs) Handles lbSave.Click
        Select Case TabContainer1.ActiveTabIndex
            Case 0

            Case 1
                lblMsg.Text = ""
                SaveDispSampleProc()

        End Select

    End Sub

    Private Sub SaveDispSampleProc()
        If dgvT2List.Rows.Count = 0 Then
            lblMsg.Text = "No Listed Yet"
            Exit Sub

        ElseIf dpDispDate.Text = Nothing Then
            lblMsg.Text = "Set Date of Disposal"
            Exit Sub
        End If

        Dim admCnt As Integer = CLng(0)

        For Each row As GridViewRow In dgvT2List.Rows
            Dim checkBox As CheckBox = TryCast(row.FindControl("CheckBox1"), CheckBox)

            If checkBox.Checked = True Then
                admCnt += 1

            End If

        Next

        If admCnt = CLng(0) Then
            lblMsg.Text = "No Item Selected"
            Exit Sub
        End If

        SaveDispSample()

    End Sub

    Protected Sub SaveDispSample()
        For i As Integer = 0 To dgvT2List.Rows.Count - 1
            Dim checkBox As CheckBox = TryCast(dgvT2List.Rows(i).FindControl("CheckBox1"), CheckBox)

            If checkBox.Checked = True Then
                sql = "update invhdrtbl set labstat = 'Disposed',disposby = '" & lblUser.Text & "'," &
                      "disposdate = '" & Format(CDate(Now()), "yyyy-MM-dd HH:mm:ss") & "' where " &
                      "mmrrno = '" & dgvT2List.Rows(i).Cells(0).Text & "'"
                ExecuteNonQuery(sql)

            End If
        Next

        filldgvT2List()

    End Sub

    Private Sub dpDateT21_TextChanged(sender As Object, e As EventArgs) Handles dpDateT21.TextChanged, dpDateT22.TextChanged
        lblMsg.Text = "Message Box"
        If dpDateT21.Text = Nothing Then
            Exit Sub
        ElseIf dpDateT22.Text = Nothing Then
            Exit Sub

        End If

        If Format(CDate(dpDateT21.Text), "yyyy-MM-dd") > Format(CDate(dpDateT22.Text), "yyyy-MM-dd") Then
            Exit Sub
        End If

        dgvT2List.DataSource = Nothing
        dgvT2List.DataBind()

        getMMgrpT2List()

    End Sub

    Protected Sub getMMgrpT2List()
        dt = GetDataTable("select distinct b.mmgrp from invhdrtbl a left join mmasttbl b on a.pono=b.codeno " &
                          "where a.status <> 'void' and a.tc ='20' and a.mov = '801' and " &
                          "a.transdate between '" & Format(CDate(dpDateT21.Text), "yyyy-MM-dd") & "' and " &
                          "'" & Format(CDate(dpDateT22.Text), "yyyy-MM-dd") & "'")
        If Not CBool(dt.Rows.Count) Then
            lblMsg.Text = "No Plant found."
            Exit Sub
        Else
            cboMMgrpT2.Items.Add("")
            cboMMgrpT2.Items.Add("ALL")
            For Each dr As DataRow In dt.Rows
                cboMMgrpT2.Items.Add(dr.Item(0).ToString())

            Next

        End If

        Call dt.Dispose()

    End Sub

    Private Sub cboMMgrpT2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboMMgrpT2.SelectedIndexChanged
        dgvT2List.DataSource = Nothing
        dgvT2List.DataBind()


    End Sub
End Class