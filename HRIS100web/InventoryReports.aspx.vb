Imports MySql.Data.MySqlClient
Imports System.IO
Imports System.Data.OleDb
Imports System.Data
Imports System.Data.DataTable
Imports System.Data.SqlTypes
Imports System
Imports System.Web
Public Class InventoryReports
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
    Dim qtyIss As Long
    Dim wtIss As Double

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If lblUser.Text Is Nothing Then
            Response.Redirect("login.aspx")

        End If

        If Not Me.IsPostBack Then
            lblUser.Text = Session("UserID")
            lblGrpUser.Text = Session("UserGrp")

            lblTitle.Text = TabPanel1.HeaderText

            'lblMsg.Text = Request.Browser.ScreenPixelsWidth.ToString

            'Select Case TabContainer1.ActiveTabIndex
            '    Case 0
            '        lblTitle.Text = TabPanel1.HeaderText
            '    Case 1
            '        lblTitle.Text = TabPanel2.HeaderText
            '    Case 2
            '        lblTitle.Text = TabPanel3.HeaderText
            '    Case 3
            '        lblTitle.Text = TabPanel4.HeaderText
            '    Case 4
            '        lblTitle.Text = TabPanel7.HeaderText
            'End Select

            cboRepFormatPA.Items.Clear()
            cboRepFormatPA.Items.Add("")
            cboRepFormatPA.Items.Add("RM vs FG Manual Entry")

            Select Case vLoggedBussArea
                Case "8100"
                    RadioButton9.Checked = True
                Case "8300"
                    RadioButton8.Checked = True
            End Select

        End If

        'lblMsg.Text = "UNDER CONSTRUCTION"


    End Sub

    Protected Sub lbClose_Click(sender As Object, e As EventArgs)
        Response.Redirect("MaterialManagement.aspx")

    End Sub

    Protected Sub dgvMMinv_RowDataBound(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Protected Sub dgvMMinv_RowCreated(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Private Sub CheckBox10_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox10.CheckedChanged
        If CheckBox10.Checked = True Then
            dpTo.Visible = False
            Label19.Visible = False
            Label18.Text = "As Of:"
        Else
            dpTo.Visible = True
            Label19.Visible = True
            Label18.Text = "Date From:"

        End If

    End Sub

    Private Sub dpFrom_TextChanged(sender As Object, e As EventArgs) Handles dpFrom.TextChanged, dpTo.TextChanged
        lblMsg.Text = "Message Box"
        If dpFrom.Text = Nothing Then
            Exit Sub
        ElseIf dpTo.Text = Nothing Then
            If CheckBox10.Checked = True Then

            Else
                lblMsg.Text = "Error: Select Date To"
                Exit Sub
            End If

        End If

        If CheckBox10.Checked = True Then

        Else
            If Format(CDate(dpFrom.Text), "yyyy-MM-dd") > Format(CDate(dpTo.Text), "yyyy-MM-dd") Then
                Exit Sub
            End If

        End If

        If RadioButton1.Checked = False And RadioButton2.Checked = False Then
            lblMsg.Text = "Error: Select Option"
            Exit Sub
        End If

        dgvMMinv.DataSource = Nothing
        dgvMMinv.DataBind()

        RefreshDataBelow()
        GetInvtryPlnt()

    End Sub

    Private Sub RefreshDataBelow()
        cboPlnt.Text = Nothing
        cboPlnt.Items.Clear()
        cboProdType.Text = Nothing
        cboProdType.Items.Clear()

        dgvMMinv.DataSource = Nothing

    End Sub

    Private Sub GetInvtryPlnt()
        cboPlnt.Items.Clear()
        If CheckBox10.Checked = True Then
            dt = GetDataTable("select concat(a.plntno,space(1),b.plntname) from invdettbl a inner join plnttbl b " &
                              "on a.plntno=b.plntno where b.ownertype <> 'Company' and a.transdate <= " &
                              "'" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' group by a.plntno")
        Else
            dt = GetDataTable("select concat(a.plntno,space(1),b.plntname) from invdettbl a inner join plnttbl b " &
                              "on a.plntno=b.plntno where b.ownertype <> 'Company' and a.transdate between " &
                              "'" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and " &
                              "'" & Format(CDate(dpTo.Text), "yyyy-MM-dd") & "' group by a.plntno")
        End If

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
        If cboPlnt.Text = "" Then
            Exit Sub

        ElseIf RadioButton1.Checked = False And RadioButton2.Checked = False Then
            lblMsg.Text = "Error: Select Option"
            Exit Sub

        End If

        dgvMMinv.DataSource = Nothing
        dgvMMinv.DataBind()

        Popmainprod()

        lbSave.Enabled = False

    End Sub

    Private Sub Popmainprod()
        cboProdType.Items.Clear()

        If CheckBox10.Checked = True Then
            dt = GetDataTable("select b.mmtype from invdettbl a left join invhdrtbl c on a.mmrrno=c.mmrrno " &
                              "left join mmasttbl b on a.codeno = b.codeno " &
                              "where c.transdate <= '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' group by b.mmtype")
        Else
            dt = GetDataTable("select b.mmtype from invdettbl a left join invhdrtbl c on a.mmrrno=c.mmrrno " &
                              "left join mmasttbl b on a.codeno = b.codeno " &
                              "where c.transdate between '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' " &
                              "and '" & Format(CDate(dpTo.Text), "yyyy-MM-dd") & "' group by b.mmtype")
        End If

        If Not CBool(dt.Rows.Count) Then
            lblMsg.Text = "Error: Not found."
            Exit Sub

        Else
            cboProdType.Items.Add("")
            For Each dr As DataRow In dt.Rows
                cboProdType.Items.Add(dr.Item(0).ToString())

            Next

        End If

        Call dt.Dispose()

    End Sub

    Private Sub cboProdType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboProdType.SelectedIndexChanged
        If RadioButton1.Checked = False And RadioButton2.Checked = False Then
            lblMsg.Text = "Error: Select Inventory Option"
            Exit Sub

        ElseIf cboPlnt.Text = "" Then
            lblMsg.Text = "Error: Select Plant"
            Exit Sub
        End If

        dgvMMinv.DataSource = Nothing

        Select Case LCase(cboProdType.Text)
            Case "finished goods"
                cboMMgrpT2.Visible = True
                Label108.Visible = True
                LbMMgrp.Visible = True
                cboMMgrpT2.Items.Clear()
                If CheckBox10.Checked = True Then
                    dt = GetDataTable("select c.mmgrp from invdettbl a left join invhdrtbl b on a.mmrrno=b.mmrrno " &
                                      "left join mmasttbl c on a.codeno=c.codeno where " &
                                      "b.transdate <= '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and " &
                                      "c.mmtype = '" & cboProdType.Text & "' and b.plntno = '" & cboPlnt.Text.Substring(0, 3) & "' " &
                                      "and b.status <> 'void' group by c.mmgrp")
                Else
                    dt = GetDataTable("select c.mmgrp from invdettbl a left join invhdrtbl b on a.mmrrno=b.mmrrno " &
                                      "left join mmasttbl c on a.codeno=c.codeno where " &
                                      "c.mmtype = '" & cboProdType.Text & "' and b.transdate between " &
                                      "'" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and " &
                                      "'" & Format(CDate(dpTo.Text), "yyyy-MM-dd") & "' and b.plntno = '" & cboPlnt.Text.Substring(0, 3) & "' " &
                                      "and b.status <> 'void' group by c.mmgrp")
                End If

                If Not CBool(dt.Rows.Count) Then
                    Exit Sub

                Else
                    cboMMgrpT2.Items.Add("")
                    For Each dr As DataRow In dt.Rows
                        cboMMgrpT2.Items.Add(dr.Item(0).ToString())

                    Next

                End If

                Call dt.Dispose()

            Case Else
                cboMMgrpT2.Visible = False
                Label108.Visible = False
                cboMMgrpT2.Items.Clear()
                LbMMgrp.Visible = False
                CrtNewDataInvAll()

        End Select

    End Sub

    Private Sub CrtNewDataInvAll()
        sql = "delete from tempinvtbl where user = '" & lblUser.Text & "'"
        ExecuteNonQuery(sql)

        Select Case True
            Case RadioButton1.Checked 'per lot no
                If CheckBox10.Checked = True Then
                    'insert inventory in
                    Select Case LCase(cboProdType.Text)
                        Case "finished goods"
                            Select Case vLoggedBussArea
                                Case "8100"
                                    sql = "insert into tempinvtbl(codeno,lotno,qty_in,wt_in,user) select a.codeno,a.lotno,ifnull(sum(a.qty),0)," &
                                          "ifnull(sum(a.wt),0),'" & lblUser.Text & "' from invdettbl a left join invhdrtbl b on a.mmrrno=b.mmrrno " &
                                          "left join mmasttbl c on a.codeno=c.codeno where b.status <> 'void' and b.plntno = '" & cboPlnt.Text.Substring(0, 3) & "' and " &
                                          "b.transdate <= '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and b.mov = '801' and b.dsrstat = 'Finished' " &
                                          "and c.mmtype = '" & cboProdType.Text & "' and c.mmgrp = '" & cboMMgrpT2.Text & "' group by a.codeno,a.lotno"
                                    ExecuteNonQuery(sql)

                                    sql = "insert into tempinvtbl(codeno,lotno,qty_in,wt_in,user) select a.codeno,a.lotno,sum(ifnull(a.qty,0))," &
                                          "sum(ifnull(a.wt,0)),'" & lblUser.Text & "' from invdettbl a left join invhdrtbl b on a.mmrrno=b.mmrrno " &
                                          "left join mmasttbl c on a.codeno=c.codeno where b.status <> 'void' and b.plntno = '" & cboPlnt.Text.Substring(0, 3) & "' and " &
                                          "b.transdate <= '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and b.mov <> '801' and c.mmtype = '" & cboProdType.Text & "' " &
                                          "and c.mmgrp = '" & cboMMgrpT2.Text & "' group by a.codeno,a.lotno"
                                    ExecuteNonQuery(sql)

                                Case Else
                                    sql = "insert into tempinvtbl(codeno,lotno,qty_in,wt_in,user) select a.codeno,a.lotno,ifnull(sum(a.qty),0),ifnull(sum(a.wt),0)," &
                                          "'" & lblUser.Text & "' from invdettbl a left join invhdrtbl b on a.mmrrno=b.mmrrno left join mmasttbl c on a.codeno=c.codeno " &
                                          "where b.status <> 'void' and b.plntno = '" & cboPlnt.Text.Substring(0, 3) & "' " &
                                          "and b.transdate <= '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and c.mmtype = '" & cboProdType.Text & "' " &
                                          "and c.mmgrp = '" & cboMMgrpT2.Text & "' group by a.codeno,a.lotno"
                                    ExecuteNonQuery(sql)

                            End Select

                            'insert wrr
                            sql = "insert into tempinvtbl(codeno,lotno,qty_in,wt_in,user) select a.codeno,a.lotno,ifnull(sum(a.qty),0),ifnull(sum(a.wt),0)," &
                                  "'" & lblUser.Text & "' from wrrdettbl a left join wrrhdrtbl b on a.wrrno=b.wrrno left join mmasttbl c on a.codeno=c.codeno " &
                                  "where c.mmtype = '" & cboProdType.Text & "' and b.status <> 'void' and b.plntno = '" & cboPlnt.Text.Substring(0, 3) & "' and b.transdate <= " &
                                  "'" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and c.mmgrp = '" & cboMMgrpT2.Text & "' group by a.codeno,a.lotno"
                            ExecuteNonQuery(sql)

                            'insert issuance
                            Select Case vLoggedBussArea
                                Case "8100"
                                    sql = "insert into tempinvtbl(codeno,lotno,qty_out,wt_out,user) select a.codeno,ifnull(a.lotno,'On Prodn')," &
                                          "ifnull(sum(a.qty),0),ifnull(sum(a.wt),0),'" & lblUser.Text & "' from issdettbl a left join isshdrtbl b on a.dono=b.dono " &
                                          "left join mmasttbl c on a.codeno=c.codeno where c.mmtype = '" & cboProdType.Text & "' and b.status <> 'void' and " &
                                          "b.plntno = '" & cboPlnt.Text.Substring(0, 3) & "' and b.transdate <= " &
                                          "'" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and c.mmgrp = '" & cboMMgrpT2.Text & "' group by a.codeno,a.lotno"
                                    ExecuteNonQuery(sql)

                                Case Else
                                    sql = "insert into tempinvtbl(codeno,lotno,qty_out,wt_out,user) select a.codeno,a.lotno,ifnull(sum(a.qty),0),ifnull(sum(a.wt),0)," &
                                          "'" & lblUser.Text & "' from issdettbl a left join isshdrtbl b on a.dono=b.dono left join mmasttbl c on a.codeno=c.codeno " &
                                          "where c.mmtype = '" & cboProdType.Text & "' and b.status <> 'void' and b.plntno = '" & cboPlnt.Text.Substring(0, 3) & "' " &
                                          "and b.transdate <= '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and c.mmgrp = '" & cboMMgrpT2.Text & "' " &
                                          "group by a.codeno,a.lotno"
                                    ExecuteNonQuery(sql)

                            End Select

                            ''adjtable in
                            sql = "insert into tempinvtbl(codeno,lotno,qty_in,wt_in,user,docno) select a.codeno,a.lotno,sum(ifnull(a.stdqty,0))," &
                                  "sum(ifnull(a.stdwt,0)),'" & lblUser.Text & "','in' from invadjdettbl a left join invadjhdrtbl b on a.dono=b.dono " &
                                  "left join mmasttbl c on a.codeno=c.codeno where c.mmtype = '" & cboProdType.Text & "' and " &
                                  "a.pono = 'PARK' and b.plntno = '" & cboPlnt.Text.Substring(0, 3) & "' " &
                                  "and b.transdate <= '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and a.tc = '10' " &
                                  "and c.mmgrp = '" & cboMMgrpT2.Text & "' group by a.codeno,a.lotno"
                            ExecuteNonQuery(sql)
                            'adj out
                            sql = "insert into tempinvtbl(codeno,lotno,qty_out,wt_out,user,docno) select a.codeno,a.lotno,sum(ifnull(a.qty,0))," &
                                  "sum(ifnull(a.wt,0)),'" & lblUser.Text & "','out' from invadjdettbl a " &
                                  "left join invadjhdrtbl b on a.dono=b.dono left join mmasttbl c on a.codeno=c.codeno where c.mmtype = '" & cboProdType.Text & "' and " &
                                  "a.pono = 'PARK' and b.plntno = '" & cboPlnt.Text.Substring(0, 3) & "' " &
                                  "and b.transdate <= '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and a.tc = '30' " &
                                  "and c.mmgrp = '" & cboMMgrpT2.Text & "' group by a.codeno,a.lotno"
                            ExecuteNonQuery(sql)

                        Case Else
                            Select Case vLoggedBussArea
                                Case "8100"
                                    sql = "insert into tempinvtbl(codeno,lotno,qty_in,wt_in,user) select a.codeno,a.lotno,ifnull(sum(a.qty),0)," &
                                          "ifnull(sum(a.wt),0),'" & lblUser.Text & "' from invdettbl a left join invhdrtbl b on a.mmrrno=b.mmrrno " &
                                          "left join mmasttbl c on a.codeno=c.codeno where b.status <> 'void' and b.plntno = '" & cboPlnt.Text.Substring(0, 3) & "' and " &
                                          "b.transdate <= '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and b.mov = '801' and b.dsrstat = 'Finished' " &
                                          "and c.mmtype = '" & cboProdType.Text & "' group by a.codeno,a.lotno"
                                    ExecuteNonQuery(sql)

                                    sql = "insert into tempinvtbl(codeno,lotno,qty_in,wt_in,user) select a.codeno,a.lotno,sum(ifnull(a.qty,0))," &
                                          "sum(ifnull(a.wt,0)),'" & lblUser.Text & "' from invdettbl a left join invhdrtbl b on a.mmrrno=b.mmrrno " &
                                          "left join mmasttbl c on a.codeno=c.codeno where b.status <> 'void' and b.plntno = '" & cboPlnt.Text.Substring(0, 3) & "' and " &
                                          "b.transdate <= '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and b.mov <> '801' and c.mmtype = '" & cboProdType.Text & "' " &
                                          "group by a.codeno,a.lotno"
                                    ExecuteNonQuery(sql)

                                Case Else
                                    sql = "insert into tempinvtbl(codeno,lotno,qty_in,wt_in,user) select a.codeno,a.lotno,ifnull(sum(a.qty),0),ifnull(sum(a.wt),0)," &
                                          "'" & lblUser.Text & "' from invdettbl a left join invhdrtbl b on a.mmrrno=b.mmrrno left join mmasttbl c on a.codeno=c.codeno " &
                                          "where b.status <> 'void' and b.plntno = '" & cboPlnt.Text.Substring(0, 3) & "' " &
                                          "and b.transdate <= '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and c.mmtype = '" & cboProdType.Text & "' group by a.codeno,a.lotno"
                                    ExecuteNonQuery(sql)

                            End Select

                            'insert wrr
                            sql = "insert into tempinvtbl(codeno,lotno,qty_in,wt_in,user) select a.codeno,a.lotno,ifnull(sum(a.qty),0),ifnull(sum(a.wt),0)," &
                                  "'" & lblUser.Text & "' from wrrdettbl a left join wrrhdrtbl b on a.wrrno=b.wrrno left join mmasttbl c on a.codeno=c.codeno " &
                                  "where c.mmtype = '" & cboProdType.Text & "' and b.status <> 'void' and b.plntno = '" & cboPlnt.Text.Substring(0, 3) & "' and b.transdate <= " &
                                  "'" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' group by a.codeno,a.lotno"
                            ExecuteNonQuery(sql)

                            'insert issuance
                            Select Case vLoggedBussArea
                                Case "8100"
                                    sql = "insert into tempinvtbl(codeno,lotno,qty_out,wt_out,user) select a.codeno,ifnull(a.lotno,'On Prodn')," &
                                          "ifnull(sum(a.qty),0),ifnull(sum(a.wt),0),'" & lblUser.Text & "' from issdettbl a left join isshdrtbl b on a.dono=b.dono " &
                                          "left join mmasttbl c on a.codeno=c.codeno where c.mmtype = '" & cboProdType.Text & "' and b.status <> 'void' and " &
                                          "b.plntno = '" & cboPlnt.Text.Substring(0, 3) & "' and b.transdate <= " &
                                          "'" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' group by a.codeno,a.lotno"
                                    ExecuteNonQuery(sql)

                                Case Else
                                    sql = "insert into tempinvtbl(codeno,lotno,qty_out,wt_out,user) select a.codeno,a.lotno,ifnull(sum(a.qty),0),ifnull(sum(a.wt),0)," &
                                          "'" & lblUser.Text & "' from issdettbl a left join isshdrtbl b on a.dono=b.dono left join mmasttbl c on a.codeno=c.codeno " &
                                          "where c.mmtype = '" & cboProdType.Text & "' and b.status <> 'void' and b.plntno = '" & cboPlnt.Text.Substring(0, 3) & "' " &
                                          "and b.transdate <= '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' group by a.codeno,a.lotno"
                                    ExecuteNonQuery(sql)

                            End Select

                            ''adjtable in
                            sql = "insert into tempinvtbl(codeno,lotno,qty_in,wt_in,user,docno) select a.codeno,a.lotno,sum(ifnull(a.stdqty,0))," &
                                  "sum(ifnull(a.stdwt,0)),'" & lblUser.Text & "','in' from invadjdettbl a " &
                                  "left join invadjhdrtbl b on a.dono=b.dono left join mmasttbl c on a.codeno=c.codeno where c.mmtype = '" & cboProdType.Text & "' and " &
                                  "a.pono = 'PARK' and b.plntno = '" & cboPlnt.Text.Substring(0, 3) & "' " &
                                  "and b.transdate <= '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and a.tc = '10' group by a.codeno,a.lotno"
                            ExecuteNonQuery(sql)
                            'adj out
                            sql = "insert into tempinvtbl(codeno,lotno,qty_out,wt_out,user,docno) select a.codeno,a.lotno,sum(ifnull(a.qty,0))," &
                                  "sum(ifnull(a.wt,0)),'" & lblUser.Text & "','out' from invadjdettbl a " &
                                  "left join invadjhdrtbl b on a.dono=b.dono left join mmasttbl c on a.codeno=c.codeno where c.mmtype = '" & cboProdType.Text & "' and " &
                                  "a.pono = 'PARK' and b.plntno = '" & cboPlnt.Text.Substring(0, 3) & "' " &
                                  "and b.transdate <= '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and a.tc = '30' group by a.codeno,a.lotno"
                            ExecuteNonQuery(sql)

                    End Select

                Else 'period inventory
                    Select Case LCase(cboProdType.Text)
                        Case "finished goods"
                            Select Case vLoggedBussArea
                                Case "8100"
                                    sql = "insert into tempinvtbl(codeno,lotno,qty_in,wt_in,user) select a.codeno,a.lotno,ifnull(sum(a.qty),0)," &
                                          "ifnull(sum(a.wt),0),'" & lblUser.Text & "' from invdettbl a left join invhdrtbl b on a.mmrrno=b.mmrrno " &
                                          "left join mmasttbl c on a.codeno=c.codeno where b.status <> 'void' and b.plntno = '" & cboPlnt.Text.Substring(0, 3) & "' and " &
                                          "b.transdate between '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and '" & Format(CDate(dpTo.Text), "yyyy-MM-dd") & "' " &
                                          "and b.mov = '801' and b.dsrstat = 'Finished' and c.mmtype = '" & cboProdType.Text & "' and c.mmgrp = '" & cboMMgrpT2.Text & "' " &
                                          "group by a.codeno,a.lotno"
                                    ExecuteNonQuery(sql)

                                    sql = "insert into tempinvtbl(codeno,lotno,qty_in,wt_in,user) select a.codeno,a.lotno,sum(ifnull(a.qty,0))," &
                                          "sum(ifnull(a.wt,0)),'" & lblUser.Text & "' from invdettbl a left join invhdrtbl b on a.mmrrno=b.mmrrno " &
                                          "left join mmasttbl c on a.codeno=c.codeno where b.status <> 'void' and b.plntno = '" & cboPlnt.Text.Substring(0, 3) & "' and " &
                                          "b.transdate <= '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and b.mov <> '801' and c.mmtype = '" & cboProdType.Text & "' " &
                                          "and c.mmgrp = '" & cboMMgrpT2.Text & "' group by a.codeno,a.lotno"
                                    ExecuteNonQuery(sql)

                                Case Else
                                    sql = "insert into tempinvtbl(codeno,lotno,qty_in,wt_in,user) select a.codeno,a.lotno,ifnull(sum(a.qty),0),ifnull(sum(a.wt),0)," &
                                          "'" & lblUser.Text & "' from invdettbl a left join invhdrtbl b on a.mmrrno=b.mmrrno left join mmasttbl c on a.codeno=c.codeno " &
                                          "where b.status <> 'void' and b.plntno = '" & cboPlnt.Text.Substring(0, 3) & "' " &
                                          "and b.transdate between '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and '" & Format(CDate(dpTo.Text), "yyyy-MM-dd") & "' " &
                                          "and c.mmtype = '" & cboProdType.Text & "' and c.mmgrp = '" & cboMMgrpT2.Text & "' group by a.codeno,a.lotno"
                                    ExecuteNonQuery(sql)

                            End Select

                            'insert wrr
                            sql = "insert into tempinvtbl(codeno,lotno,qty_in,wt_in,user) select a.codeno,a.lotno,ifnull(sum(a.qty),0),ifnull(sum(a.wt),0)," &
                                  "'" & lblUser.Text & "' from wrrdettbl a left join wrrhdrtbl b on a.wrrno=b.wrrno left join mmasttbl c on a.codeno=c.codeno " &
                                  "where c.mmtype = '" & cboProdType.Text & "' and b.status <> 'void' and b.plntno = '" & cboPlnt.Text.Substring(0, 3) & "' " &
                                  "and b.transdate between '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and '" & Format(CDate(dpTo.Text), "yyyy-MM-dd") & "' " &
                                  "and c.mmgrp = '" & cboMMgrpT2.Text & "' group by a.codeno,a.lotno"
                            ExecuteNonQuery(sql)

                            'insert issuance
                            Select Case vLoggedBussArea
                                Case "8100"
                                    sql = "insert into tempinvtbl(codeno,lotno,qty_out,wt_out,user) select a.codeno,ifnull(a.lotno,'On Prodn')," &
                                          "ifnull(sum(a.qty),0),ifnull(sum(a.wt),0),'" & lblUser.Text & "' from issdettbl a left join isshdrtbl b on a.dono=b.dono " &
                                          "left join mmasttbl c on a.codeno=c.codeno where c.mmtype = '" & cboProdType.Text & "' and b.status <> 'void' and " &
                                          "b.plntno = '" & cboPlnt.Text.Substring(0, 3) & "' and b.transdate between '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' " &
                                          "and '" & Format(CDate(dpTo.Text), "yyyy-MM-dd") & "' and c.mmgrp = '" & cboMMgrpT2.Text & "' group by a.codeno,a.lotno"
                                    ExecuteNonQuery(sql)

                                Case Else
                                    sql = "insert into tempinvtbl(codeno,lotno,qty_out,wt_out,user) select a.codeno,a.lotno,ifnull(sum(a.qty),0),ifnull(sum(a.wt),0)," &
                                          "'" & lblUser.Text & "' from issdettbl a left join isshdrtbl b on a.dono=b.dono left join mmasttbl c on a.codeno=c.codeno " &
                                          "where c.mmtype = '" & cboProdType.Text & "' and b.status <> 'void' and b.plntno = '" & cboPlnt.Text.Substring(0, 3) & "' " &
                                          "and b.transdate between '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and '" & Format(CDate(dpTo.Text), "yyyy-MM-dd") & "' " &
                                          "and c.mmgrp = '" & cboMMgrpT2.Text & "' group by a.codeno,a.lotno"
                                    ExecuteNonQuery(sql)

                            End Select

                            ''adjtable in
                            sql = "insert into tempinvtbl(codeno,lotno,qty_in,wt_in,user,docno) select a.codeno,a.lotno,sum(ifnull(a.stdqty,0))," &
                                  "sum(ifnull(a.stdwt,0)),'" & lblUser.Text & "','in' from invadjdettbl a left join invadjhdrtbl b on a.dono=b.dono " &
                                  "left join mmasttbl c on a.codeno=c.codeno where c.mmtype = '" & cboProdType.Text & "' and " &
                                  "a.pono = 'PARK' and b.plntno = '" & cboPlnt.Text.Substring(0, 3) & "' " &
                                  "and b.transdate between '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and '" & Format(CDate(dpTo.Text), "yyyy-MM-dd") & "' " &
                                  "and a.tc = '10' and c.mmgrp = '" & cboMMgrpT2.Text & "' group by a.codeno,a.lotno"
                            ExecuteNonQuery(sql)
                            'adj out
                            sql = "insert into tempinvtbl(codeno,lotno,qty_out,wt_out,user,docno) select a.codeno,a.lotno,sum(ifnull(a.qty,0))," &
                                  "sum(ifnull(a.wt,0)),'" & lblUser.Text & "','out' from invadjdettbl a " &
                                  "left join invadjhdrtbl b on a.dono=b.dono left join mmasttbl c on a.codeno=c.codeno where c.mmtype = '" & cboProdType.Text & "' and " &
                                  "a.pono = 'PARK' and b.plntno = '" & cboPlnt.Text.Substring(0, 3) & "' " &
                                  "and b.transdate between '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and '" & Format(CDate(dpTo.Text), "yyyy-MM-dd") & "' " &
                                  "and a.tc = '30' and c.mmgrp = '" & cboMMgrpT2.Text & "' group by a.codeno,a.lotno"
                            ExecuteNonQuery(sql)

                        Case Else
                            Select Case vLoggedBussArea
                                Case "8100"
                                    sql = "insert into tempinvtbl(codeno,lotno,qty_in,wt_in,user) select a.codeno,a.lotno,ifnull(sum(a.qty),0)," &
                                          "ifnull(sum(a.wt),0),'" & lblUser.Text & "' from invdettbl a left join invhdrtbl b on a.mmrrno=b.mmrrno " &
                                          "left join mmasttbl c on a.codeno=c.codeno where b.status <> 'void' and b.plntno = '" & cboPlnt.Text.Substring(0, 3) & "' and " &
                                          "b.transdate between '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and '" & Format(CDate(dpTo.Text), "yyyy-MM-dd") & "' " &
                                          "and b.mov = '801' and b.dsrstat = 'Finished' and c.mmtype = '" & cboProdType.Text & "' group by a.codeno,a.lotno"
                                    ExecuteNonQuery(sql)

                                    sql = "insert into tempinvtbl(codeno,lotno,qty_in,wt_in,user) select a.codeno,a.lotno,sum(ifnull(a.qty,0))," &
                                          "sum(ifnull(a.wt,0)),'" & lblUser.Text & "' from invdettbl a left join invhdrtbl b on a.mmrrno=b.mmrrno " &
                                          "left join mmasttbl c on a.codeno=c.codeno where b.status <> 'void' and b.plntno = '" & cboPlnt.Text.Substring(0, 3) & "' and " &
                                          "b.transdate between '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and '" & Format(CDate(dpTo.Text), "yyyy-MM-dd") & "' " &
                                          "and b.mov <> '801' and c.mmtype = '" & cboProdType.Text & "' group by a.codeno,a.lotno"
                                    ExecuteNonQuery(sql)

                                Case Else
                                    sql = "insert into tempinvtbl(codeno,lotno,qty_in,wt_in,user) select a.codeno,a.lotno,ifnull(sum(a.qty),0),ifnull(sum(a.wt),0)," &
                                          "'" & lblUser.Text & "' from invdettbl a left join invhdrtbl b on a.mmrrno=b.mmrrno left join mmasttbl c on a.codeno=c.codeno " &
                                          "where b.status <> 'void' and b.plntno = '" & cboPlnt.Text.Substring(0, 3) & "' " &
                                          "and b.transdate between '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and '" & Format(CDate(dpTo.Text), "yyyy-MM-dd") & "' " &
                                          "and c.mmtype = '" & cboProdType.Text & "' group by a.codeno,a.lotno"
                                    ExecuteNonQuery(sql)

                            End Select

                            'insert wrr
                            sql = "insert into tempinvtbl(codeno,lotno,qty_in,wt_in,user) select a.codeno,a.lotno,ifnull(sum(a.qty),0),ifnull(sum(a.wt),0)," &
                                  "'" & lblUser.Text & "' from wrrdettbl a left join wrrhdrtbl b on a.wrrno=b.wrrno left join mmasttbl c on a.codeno=c.codeno " &
                                  "where c.mmtype = '" & cboProdType.Text & "' and b.status <> 'void' and b.plntno = '" & cboPlnt.Text.Substring(0, 3) & "' " &
                                  "and b.transdate between '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and '" & Format(CDate(dpTo.Text), "yyyy-MM-dd") & "' " &
                                  "group by a.codeno,a.lotno"
                            ExecuteNonQuery(sql)

                            'insert issuance
                            Select Case vLoggedBussArea
                                Case "8100"
                                    sql = "insert into tempinvtbl(codeno,lotno,qty_out,wt_out,user) select a.codeno,ifnull(a.lotno,'On Prodn')," &
                                          "ifnull(sum(a.qty),0),ifnull(sum(a.wt),0),'" & lblUser.Text & "' from issdettbl a left join isshdrtbl b on a.dono=b.dono " &
                                          "left join mmasttbl c on a.codeno=c.codeno where c.mmtype = '" & cboProdType.Text & "' and b.status <> 'void' and " &
                                          "b.plntno = '" & cboPlnt.Text.Substring(0, 3) & "' and b.transdate between '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' " &
                                          "and '" & Format(CDate(dpTo.Text), "yyyy-MM-dd") & "' group by a.codeno,a.lotno"
                                    ExecuteNonQuery(sql)

                                Case Else
                                    sql = "insert into tempinvtbl(codeno,lotno,qty_out,wt_out,user) select a.codeno,a.lotno,ifnull(sum(a.qty),0),ifnull(sum(a.wt),0)," &
                                          "'" & lblUser.Text & "' from issdettbl a left join isshdrtbl b on a.dono=b.dono left join mmasttbl c on a.codeno=c.codeno " &
                                          "where c.mmtype = '" & cboProdType.Text & "' and b.status <> 'void' and b.plntno = '" & cboPlnt.Text.Substring(0, 3) & "' " &
                                          "and b.transdate between '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and '" & Format(CDate(dpTo.Text), "yyyy-MM-dd") & "' " &
                                          "group by a.codeno,a.lotno"
                                    ExecuteNonQuery(sql)

                            End Select

                            ''adjtable in
                            sql = "insert into tempinvtbl(codeno,lotno,qty_in,wt_in,user,docno) select a.codeno,a.lotno,sum(ifnull(a.stdqty,0))," &
                                  "sum(ifnull(a.stdwt,0)),'" & lblUser.Text & "','in' from invadjdettbl a " &
                                  "left join invadjhdrtbl b on a.dono=b.dono left join mmasttbl c on a.codeno=c.codeno where c.mmtype = '" & cboProdType.Text & "' and " &
                                  "a.pono = 'PARK' and b.plntno = '" & cboPlnt.Text.Substring(0, 3) & "' " &
                                  "and b.transdate between '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and '" & Format(CDate(dpTo.Text), "yyyy-MM-dd") & "' " &
                                  "and a.tc = '10' group by a.codeno,a.lotno"
                            ExecuteNonQuery(sql)
                            'adj out
                            sql = "insert into tempinvtbl(codeno,lotno,qty_out,wt_out,user,docno) select a.codeno,a.lotno,sum(ifnull(a.qty,0))," &
                                  "sum(ifnull(a.wt,0)),'" & lblUser.Text & "','out' from invadjdettbl a " &
                                  "left join invadjhdrtbl b on a.dono=b.dono left join mmasttbl c on a.codeno=c.codeno where c.mmtype = '" & cboProdType.Text & "' and " &
                                  "a.pono = 'PARK' and b.plntno = '" & cboPlnt.Text.Substring(0, 3) & "' " &
                                  "and b.transdate between '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and '" & Format(CDate(dpTo.Text), "yyyy-MM-dd") & "' " &
                                  "and a.tc = '30' group by a.codeno,a.lotno"
                            ExecuteNonQuery(sql)

                    End Select

                End If

            Case RadioButton2.Checked 'all lot no
                If CheckBox10.Checked = True Then
                    'insert inventory in
                    Select Case LCase(cboProdType.Text)
                        Case "finished goods"
                            Select Case vLoggedBussArea
                                Case "8100"
                                    sql = "insert into tempinvtbl(codeno,qty_in,wt_in,user) select a.codeno,ifnull(sum(a.qty),0)," &
                                          "ifnull(sum(a.wt),0),'" & lblUser.Text & "' from invdettbl a left join invhdrtbl b on a.mmrrno=b.mmrrno " &
                                          "left join mmasttbl c on a.codeno=c.codeno where b.status <> 'void' and b.plntno = '" & cboPlnt.Text.Substring(0, 3) & "' and " &
                                          "b.transdate <= '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and b.mov = '801' and b.dsrstat = 'Finished' " &
                                          "and c.mmtype = '" & cboProdType.Text & "' and c.mmgrp = '" & cboMMgrpT2.Text & "' group by a.codeno"
                                    ExecuteNonQuery(sql)

                                    sql = "insert into tempinvtbl(codeno,qty_in,wt_in,user) select a.codeno,sum(ifnull(a.qty,0))," &
                                          "sum(ifnull(a.wt,0)),'" & lblUser.Text & "' from invdettbl a left join invhdrtbl b on a.mmrrno=b.mmrrno " &
                                          "left join mmasttbl c on a.codeno=c.codeno where b.status <> 'void' and b.plntno = '" & cboPlnt.Text.Substring(0, 3) & "' and " &
                                          "b.transdate <= '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and b.mov <> '801' and c.mmtype = '" & cboProdType.Text & "' " &
                                          "and c.mmgrp = '" & cboMMgrpT2.Text & "' group by a.codeno"
                                    ExecuteNonQuery(sql)

                                Case Else
                                    sql = "insert into tempinvtbl(codeno,qty_in,wt_in,user) select a.codeno,ifnull(sum(a.qty),0),ifnull(sum(a.wt),0)," &
                                          "'" & lblUser.Text & "' from invdettbl a left join invhdrtbl b on a.mmrrno=b.mmrrno left join mmasttbl c on a.codeno=c.codeno " &
                                          "where b.status <> 'void' and b.plntno = '" & cboPlnt.Text.Substring(0, 3) & "' " &
                                          "and b.transdate <= '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and c.mmtype = '" & cboProdType.Text & "' " &
                                          "and c.mmgrp = '" & cboMMgrpT2.Text & "' group by a.codeno"
                                    ExecuteNonQuery(sql)

                            End Select

                            'insert wrr
                            sql = "insert into tempinvtbl(codeno,qty_in,wt_in,user) select a.codeno,ifnull(sum(a.qty),0),ifnull(sum(a.wt),0)," &
                                  "'" & lblUser.Text & "' from wrrdettbl a left join wrrhdrtbl b on a.wrrno=b.wrrno left join mmasttbl c on a.codeno=c.codeno " &
                                  "where c.mmtype = '" & cboProdType.Text & "' and b.status <> 'void' and b.plntno = '" & cboPlnt.Text.Substring(0, 3) & "' and b.transdate <= " &
                                  "'" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and c.mmgrp = '" & cboMMgrpT2.Text & "' group by a.codeno"
                            ExecuteNonQuery(sql)

                            'insert issuance
                            sql = "insert into tempinvtbl(codeno,qty_out,wt_out,user) select a.codeno,ifnull(sum(a.qty),0),ifnull(sum(a.wt),0)," &
                                  "'" & lblUser.Text & "' from issdettbl a left join isshdrtbl b on a.dono=b.dono left join mmasttbl c on a.codeno=c.codeno " &
                                  "where c.mmtype = '" & cboProdType.Text & "' and b.status <> 'void' and b.plntno = '" & cboPlnt.Text.Substring(0, 3) & "' " &
                                 "and b.transdate <= '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and c.mmgrp = '" & cboMMgrpT2.Text & "' group by a.codeno"
                            ExecuteNonQuery(sql)

                            ''adjtable in
                            sql = "insert into tempinvtbl(codeno,qty_in,wt_in,user,docno) select a.codeno,sum(ifnull(a.stdqty,0))," &
                                  "sum(ifnull(a.stdwt,0)),'" & lblUser.Text & "','in' from invadjdettbl a " &
                                  "left join invadjhdrtbl b on a.dono=b.dono left join mmasttbl c on a.codeno=c.codeno where c.mmtype = '" & cboProdType.Text & "' and " &
                                  "a.pono = 'PARK' and b.plntno = '" & cboPlnt.Text.Substring(0, 3) & "' " &
                                  "and b.transdate <= '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and a.tc = '10' " &
                                  "and c.mmgrp = '" & cboMMgrpT2.Text & "' group by a.codeno"
                            ExecuteNonQuery(sql)
                            'adj out
                            sql = "insert into tempinvtbl(codeno,qty_out,wt_out,user,docno) select a.codeno,sum(ifnull(a.qty,0))," &
                                  "sum(ifnull(a.wt,0)),'" & lblUser.Text & "','out' from invadjdettbl a " &
                                  "left join invadjhdrtbl b on a.dono=b.dono left join mmasttbl c on a.codeno=c.codeno where c.mmtype = '" & cboProdType.Text & "' and " &
                                  "a.pono = 'PARK' and b.plntno = '" & cboPlnt.Text.Substring(0, 3) & "' " &
                                  "and b.transdate <= '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and a.tc = '30' " &
                                  "and c.mmgrp = '" & cboMMgrpT2.Text & "' group by a.codeno"
                            ExecuteNonQuery(sql)

                        Case Else
                            Select Case vLoggedBussArea
                                Case "8100"
                                    sql = "insert into tempinvtbl(codeno,qty_in,wt_in,user) select a.codeno,ifnull(sum(a.qty),0)," &
                                          "ifnull(sum(a.wt),0),'" & lblUser.Text & "' from invdettbl a left join invhdrtbl b on a.mmrrno=b.mmrrno " &
                                          "left join mmasttbl c on a.codeno=c.codeno where b.status <> 'void' and b.plntno = '" & cboPlnt.Text.Substring(0, 3) & "' and " &
                                          "b.transdate <= '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and b.mov = '801' and b.dsrstat = 'Finished' " &
                                          "and c.mmtype = '" & cboProdType.Text & "' group by a.codeno"
                                    ExecuteNonQuery(sql)

                                    sql = "insert into tempinvtbl(codeno,qty_in,wt_in,user) select a.codeno,sum(ifnull(a.qty,0))," &
                                          "sum(ifnull(a.wt,0)),'" & lblUser.Text & "' from invdettbl a left join invhdrtbl b on a.mmrrno=b.mmrrno " &
                                          "left join mmasttbl c on a.codeno=c.codeno where b.status <> 'void' and b.plntno = '" & cboPlnt.Text.Substring(0, 3) & "' and " &
                                          "b.transdate <= '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and b.mov <> '801' and c.mmtype = '" & cboProdType.Text & "' " &
                                          "group by a.codeno"
                                    ExecuteNonQuery(sql)

                                Case Else
                                    sql = "insert into tempinvtbl(codeno,qty_in,wt_in,user) select a.codeno,ifnull(sum(a.qty),0),ifnull(sum(a.wt),0)," &
                                          "'" & lblUser.Text & "' from invdettbl a left join invhdrtbl b on a.mmrrno=b.mmrrno left join mmasttbl c on a.codeno=c.codeno " &
                                          "where b.status <> 'void' and b.plntno = '" & cboPlnt.Text.Substring(0, 3) & "' " &
                                          "and b.transdate <= '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and " &
                                          "c.mmtype = '" & cboProdType.Text & "' group by a.codeno"
                                    ExecuteNonQuery(sql)

                            End Select

                            'insert wrr
                            sql = "insert into tempinvtbl(codeno,qty_in,wt_in,user) select a.codeno,ifnull(sum(a.qty),0),ifnull(sum(a.wt),0)," &
                                  "'" & lblUser.Text & "' from wrrdettbl a left join wrrhdrtbl b on a.wrrno=b.wrrno left join mmasttbl c on a.codeno=c.codeno " &
                                  "where c.mmtype = '" & cboProdType.Text & "' and b.status <> 'void' and b.plntno = '" & cboPlnt.Text.Substring(0, 3) & "' and b.transdate <= " &
                                  "'" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' group by a.codeno"
                            ExecuteNonQuery(sql)

                            'insert issuance
                            Select Case vLoggedBussArea
                                Case "8100"
                                    sql = "insert into tempinvtbl(codeno,qty_out,wt_out,user) select a.codeno,ifnull(sum(a.qty),0),ifnull(sum(a.wt),0)," &
                                          "'" & lblUser.Text & "' from issdettbl a left join isshdrtbl b on a.dono=b.dono left join mmasttbl c on a.codeno=c.codeno " &
                                          "where c.mmtype = '" & cboProdType.Text & "' and b.status <> 'void' and b.plntno = '" & cboPlnt.Text.Substring(0, 3) & "' " &
                                          "and b.transdate <= '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' group by a.codeno"
                                    ExecuteNonQuery(sql)

                                Case Else
                                    sql = "insert into tempinvtbl(codeno,qty_out,wt_out,user) select a.codeno,ifnull(sum(a.qty),0),ifnull(sum(a.wt),0)," &
                                          "'" & lblUser.Text & "' from issdettbl a left join isshdrtbl b on a.dono=b.dono left join mmasttbl c on a.codeno=c.codeno " &
                                          "where c.mmtype = '" & cboProdType.Text & "' and b.status <> 'void' and b.plntno = '" & cboPlnt.Text.Substring(0, 3) & "' " &
                                          "and b.transdate <= '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' group by a.codeno"
                                    ExecuteNonQuery(sql)

                            End Select

                            ''adjtable in
                            sql = "insert into tempinvtbl(codeno,qty_in,wt_in,user,docno) select a.codeno,sum(ifnull(a.stdqty,0))," &
                                  "sum(ifnull(a.stdwt,0)),'" & lblUser.Text & "','in' from invadjdettbl a " &
                                  "left join invadjhdrtbl b on a.dono=b.dono left join mmasttbl c on a.codeno=c.codeno where c.mmtype = '" & cboProdType.Text & "' and " &
                                  "a.pono = 'PARK' and b.plntno = '" & cboPlnt.Text.Substring(0, 3) & "' " &
                                  "and b.transdate <= '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and a.tc = '10' group by a.codeno"
                            ExecuteNonQuery(sql)
                            'adj out
                            sql = "insert into tempinvtbl(codeno,qty_out,wt_out,user,docno) select a.codeno,sum(ifnull(a.qty,0))," &
                                  "sum(ifnull(a.wt,0)),'" & lblUser.Text & "','out' from invadjdettbl a " &
                                  "left join invadjhdrtbl b on a.dono=b.dono left join mmasttbl c on a.codeno=c.codeno where c.mmtype = '" & cboProdType.Text & "' and " &
                                  "a.pono = 'PARK' and b.plntno = '" & cboPlnt.Text.Substring(0, 3) & "' " &
                                  "and b.transdate <= '" & Format(CDate(dpFrom.Text), "yyyy-MM-dd") & "' and a.tc = '30' group by a.codeno"
                            ExecuteNonQuery(sql)

                    End Select

                Else
                    lblMsg.Text = "Error: Not Yet Available"
                    dgvMMinv.DataSource = Nothing
                    Exit Sub

                End If

        End Select

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

        sql = "insert into tempinvtbl2(codeno,lotno,qty_in,wt_in,qty_out,wt_out,user) select codeno,lotno,sum(ifnull(qty_in,0)),sum(ifnull(wt_in,0))," &
              "sum(ifnull(qty_out,0)),sum(ifnull(wt_out,0)),'" & lblUser.Text & "' from tempinvtbl where user = '" & lblUser.Text & "' group by codeno,lotno"
        ExecuteNonQuery(sql)

        'round off base on no of digit s
        Select Case LCase(cboProdType.Text)
            Case "raw materials"
                sql = "update tempinvtbl2 a,(select codeno,ifnull(admdigit,2) as adm from mmasttbl where mmtype = 'raw materials') as b " &
                      "set a.qty_bal = round(ifnull(a.qty_in,0)-ifnull(a.qty_out,0),0)," &
                      "a.wt_bal = round(ifnull(a.wt_in,0)-ifnull(a.wt_out,0),b.adm) where a.user = '" & lblUser.Text & "'"
                ExecuteNonQuery(sql)

            Case Else
                sql = "update tempinvtbl2 set qty_bal = round(ifnull(qty_in,0)-ifnull(qty_out,0),0)," &
                      "wt_bal = round(ifnull(wt_in,0)-ifnull(wt_out,0),2) where user = '" & lblUser.Text & "'"
                ExecuteNonQuery(sql)

        End Select


        sql = "delete from tempinvtbl2 where user = '" & lblUser.Text & "' and qty_bal = 0 and wt_bal = 0"
        ExecuteNonQuery(sql)

        FillDGViewInvT2()


    End Sub

    Private Sub FillDGViewInvT2()
        dt = GetDataTable("select * from tempinvtbl2 where user = '" & lblUser.Text & "'")
        If Not CBool(dt.Rows.Count) Then
            lblMsg.Text = "Error: No Inventory Found"
            dgvMMinv.DataSource = Nothing
            Exit Sub

        End If

        dt.Dispose()

        Dim adapter As New MySqlDataAdapter
        Dim ds As New DataSet()
        Dim i As Integer = 0
        sqldata = Nothing

        Select Case True
            Case RadioButton1.Checked
                sqldata = "select a.codeno,ifnull(b.codename,b.mmdesc) as mmdesc,a.lotno,a.qty_in,a.wt_in,a.qty_out,a.wt_out," &
                          "a.qty_bal,a.wt_bal  from tempinvtbl2 a left join mmasttbl b on a.codeno=b.codeno where " &
                          "a.user = '" & lblUser.Text & "' order by ifnull(b.codename,b.mmdesc)"
            Case RadioButton2.Checked
                sqldata = "select a.codeno,ifnull(b.codename,b.mmdesc) as mmdesc,'' as lotno,sum(a.qty_in) as qty_in," &
                          "sum(a.wt_in) as wt_in,sum(a.qty_out) as qty_out,sum(a.wt_out) as wt_out,sum(a.qty_bal) as qty_bal," &
                          "sum(a.wt_bal) as wt_bal  from tempinvtbl2 a left join mmasttbl b on a.codeno=b.codeno where " &
                          "a.user = '" & lblUser.Text & "' group by a.codeno order by ifnull(b.codename,b.mmdesc)"
        End Select

        Select Case True
            Case RadioButton1.Checked
                dgvMMinv.Columns(2).Visible = True
            Case RadioButton2.Checked
                dgvMMinv.Columns(2).Visible = False
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

    Private Sub LbMMgrp_Click(sender As Object, e As EventArgs) Handles LbMMgrp.Click
        FillDGViewInvT2()

    End Sub

    Private Sub cboMMgrpT2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboMMgrpT2.SelectedIndexChanged
        If cboMMgrpT2.Text = "" Then
            Exit Sub

        ElseIf RadioButton1.Checked = False And RadioButton2.Checked = False Then
            lblMsg.Text = "Error: Select Inventory Option"
            Exit Sub

        ElseIf cboPlnt.Text = "" Then
            lblMsg.Text = "Error: Select Plant"
            Exit Sub

        End If

        CrtNewDataInvAll()

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
                lblTitle.Text = TabPanel4.HeaderText
            Case 4
                lblTitle.Text = TabPanel7.HeaderText

                If Not Me.IsPostBack Then
                    cboRepFormatPA.Items.Add("")
                    cboRepFormatPA.Items.Add("RM vs FG Manual Entry")

                    Select Case vLoggedBussArea
                        Case "8100"
                            RadioButton9.Checked = True
                        Case "8300"
                            RadioButton8.Checked = True
                    End Select

                End If

        End Select

    End Sub

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged, RadioButton2.CheckedChanged

        dgvMMinv.DataSource = Nothing
        dgvMMinv.DataBind()
        RefreshDataBelow()
        cboMMgrpT2.Items.Clear()
        cboMMgrpT2.Text = Nothing

        If CheckBox10.Checked = True Then
            If dpFrom.Text = Nothing Then
                Exit Sub
            Else

            End If
        Else
            If dpFrom.Text = Nothing Or dpTo.Text = Nothing Then
                Exit Sub
            Else

            End If
        End If

        GetInvtryPlnt()

    End Sub

    Private Sub cboMMtype_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboMMtype.SelectedIndexChanged
        If cboMMtype.Text = "" Then
            Exit Sub
        End If

        ResetInvAnalysis()

        Select Case LCase(cboMMtype.Text)
            Case "finished goods"
                CheckBox13.Visible = True
                CheckBox13.Checked = False
                Select Case vLoggedBussArea
                    Case "8300"
                        cboPCinvAna.Visible = True
                        lblPClabel.Visible = True
                        If cboPCinvAna.Text = "" Then
                            lblMsg.Text = "Select Product Class"
                            Exit Sub
                        End If

                        lblPClabel.Visible = False
                        cboPCinvAna.Visible = False

                End Select

            Case "raw materials"
                Select Case vLoggedBussArea
                    Case "8100"
                        CheckBox13.Visible = True
                        CheckBox13.Checked = False

                End Select

            Case Else
                lblPClabel.Visible = False
                cboPCinvAna.Visible = False
                CheckBox13.Checked = False
                CheckBox13.Visible = False

        End Select

        GetMMdesc()

    End Sub

    Private Sub GetMMdesc()
        cboMMdesc.Items.Clear()

        Select Case LCase(cboMMtype.Text)
            Case "finished goods"
                Select Case vLoggedBussArea
                    Case "8300"
                        dt = GetDataTable("select ifnull(b.codename,b.mmdesc) from tempdodet a left join mmasttbl b on a.codeno=b.codeno where " &
                                          "b.mmtype = '" & cboMMtype.Text & "' and a.transdate <= '" & Format(CDate(dpInvDate.Text), "yyyy-MM-dd") & "' " &
                                          "and a.user = '" & lblUser.Text & "' and b.pc = '" & cboPCinvAna.Text & "' group by a.codeno order by b.mmdesc")
                    Case Else
                        dt = GetDataTable("select ifnull(b.codename,b.mmdesc) from tempdodet a left join mmasttbl b on a.codeno=b.codeno where " &
                                          "b.mmtype = '" & cboMMtype.Text & "' and a.transdate <= '" & Format(CDate(dpInvDate.Text), "yyyy-MM-dd") & "' " &
                                          "and a.user = '" & lblUser.Text & "' group by a.codeno order by b.mmdesc")
                End Select

                'Case "raw materials"
                '    Select Case vLoggedBussArea
                '        Case "8100"

                '    End Select

            Case Else
                dt = GetDataTable("select ifnull(b.codename,b.mmdesc) from tempdodet a left join mmasttbl b on a.codeno=b.codeno where " &
                                  "b.mmtype = '" & cboMMtype.Text & "' and a.transdate <= '" & Format(CDate(dpInvDate.Text), "yyyy-MM-dd") & "' " &
                                  "and a.user = '" & lblUser.Text & "' group by a.codeno order by b.mmdesc")
        End Select

        If Not CBool(dt.Rows.Count) Then
            cboMMdesc.Items.Clear()
            cboMMdesc.Text = Nothing
            lblCodeNo.Text = ""
            cboLotNo.Items.Clear()
            lblMsg.Text = "No Lot No. Created yet for " & cboMMtype.Text & " Please Select Plant Again"
            Exit Sub

        Else
            cboMMdesc.Items.Add("")
            For Each dr As DataRow In dt.Rows
                cboMMdesc.Items.Add(dr.Item(0).ToString())

            Next
        End If

        Call dt.Dispose()

    End Sub

    Private Sub dpInvDate_TextChanged(sender As Object, e As EventArgs) Handles dpInvDate.TextChanged
        GetPlnt()

        ResetInvAnalysis()

    End Sub

    Private Sub GetPlnt()

        cboPlnt2.Items.Clear()

        dt = GetDataTable("select concat(a.plntno,space(1),c.plntname) from invhdrtbl a " &
                          "left join plnttbl c on a.plntno=c.plntno where c.ownertype <> 'Company' and " &
                          "a.transdate <= '" & Format(CDate(dpInvDate.Text), "yyyy-MM-dd") & "' " &
                          "group by a.plntno")
        If Not CBool(dt.Rows.Count) Then
            lblMsg.Text = "Not found."
            Exit Sub
        Else
            cboPlnt2.Items.Add("")
            For Each dr As DataRow In dt.Rows
                cboPlnt2.Items.Add(dr.Item(0).ToString())
            Next
        End If

        Call dt.Dispose()

    End Sub

    Protected Sub lvInv_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowIndex >= 0 Then
            Select Case lblAdmDig.Text
                Case "2"
                    e.Row.Cells(3).Text = Format(Convert.ToDecimal(e.Row.Cells(3).Text), "#,##0.00")
                Case "3"
                    e.Row.Cells(3).Text = Format(Convert.ToDecimal(e.Row.Cells(3).Text), "#,##0.000")
                Case "4"
                    e.Row.Cells(3).Text = Format(Convert.ToDecimal(e.Row.Cells(3).Text), "#,##0.0000")
                Case "5"
                    e.Row.Cells(3).Text = Format(Convert.ToDecimal(e.Row.Cells(3).Text), "#,##0.00000")
                Case "6"
                    e.Row.Cells(3).Text = Format(Convert.ToDecimal(e.Row.Cells(3).Text), "#,##0.000000")
                Case Else
                    e.Row.Cells(3).Text = Format(Convert.ToDecimal(e.Row.Cells(3).Text), "#,##0.00")
            End Select

        End If


    End Sub

    Protected Sub lvInv_RowCreated(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Protected Sub lvIss_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowIndex >= 0 Then
            Select Case lblAdmDig.Text
                Case "2"
                    e.Row.Cells(3).Text = Format(Convert.ToDecimal(e.Row.Cells(3).Text), "#,##0.00")
                Case "3"
                    e.Row.Cells(3).Text = Format(Convert.ToDecimal(e.Row.Cells(3).Text), "#,##0.000")
                Case "4"
                    e.Row.Cells(3).Text = Format(Convert.ToDecimal(e.Row.Cells(3).Text), "#,##0.0000")
                Case "5"
                    e.Row.Cells(3).Text = Format(Convert.ToDecimal(e.Row.Cells(3).Text), "#,##0.00000")
                Case "6"
                    e.Row.Cells(3).Text = Format(Convert.ToDecimal(e.Row.Cells(3).Text), "#,##0.000000")
            End Select

        End If


    End Sub

    Protected Sub lvIss_RowCreated(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Private Sub cboPlnt2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboPlnt2.SelectedIndexChanged
        ResetInvAnalysis()

        PopMMtype2()

        lbSave.Enabled = False

        cboPCinvAna.Items.Clear()

        Select Case vLoggedBussArea
            Case "8300"
                cboPCinvAna.Items.Add("")
                cboPCinvAna.Items.Add("1")
                cboPCinvAna.Items.Add("2")

        End Select

    End Sub

    Protected Sub ResetInvAnalysis()
        lvInv.DataSource = Nothing
        lvInv.DataBind()

        lvIss.DataSource = Nothing
        lvIss.DataBind()

        lblInvQty.Text = "0"
        lblInvWt.Text = "0.00"
        lblIssQty.Text = "0"
        lblIssWt.Text = "0.00"
        lblBalQty.Text = "0"
        lblBalWt.Text = "0.00"

    End Sub

    Private Sub PopMMtype2()

        sql = "delete from tempdodet where user = '" & lblUser.Text & "'"
        ExecuteNonQuery(sql)

        sql = "insert into tempdodet(dono,transdate,codeno,user) select a.mmrrno,c.transdate,a.codeno," &
              "'" & lblUser.Text & "' from invdettbl a left join invhdrtbl c on a.mmrrno=c.mmrrno where " &
              "c.plntno = '" & cboPlnt2.Text.Substring(0, 3) & "' and " &
              "c.transdate <= '" & Format(CDate(dpInvDate.Text), "yyyy-MM-dd") & "' and " &
              "c.status <> 'void' group by a.codeno"
        ExecuteNonQuery(sql)

        sql = "insert into tempdodet(dono,transdate,codeno,user) select a.wrrno,c.transdate,a.codeno," &
              "'" & lblUser.Text & "' from wrrdettbl a left join wrrhdrtbl c on a.wrrno=c.wrrno where " &
              "c.plntno = '" & cboPlnt2.Text.Substring(0, 3) & "' and " &
              "c.transdate <= '" & Format(CDate(dpInvDate.Text), "yyyy-MM-dd") & "' and " &
              "c.status <> 'void' group by a.codeno"
        ExecuteNonQuery(sql)

        cboMMtype.Items.Clear()
        dt = GetDataTable("select b.mmtype from tempdodet a left join mmasttbl b on a.codeno=b.codeno where " &
                          "a.user = '" & lblUser.Text & "' group by b.mmtype")
        If Not CBool(dt.Rows.Count) Then
            lblMsg.Text = "Error: Not found."
            Exit Sub

        Else
            cboMMtype.Items.Add("")
            For Each dr As DataRow In dt.Rows
                cboMMtype.Items.Add(dr.Item(0).ToString())

            Next
        End If

        Call dt.Dispose()

    End Sub

    Private Sub cboMMdesc_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboMMdesc.SelectedIndexChanged
        If cboPlnt2.Text = "" Then
            cboPlnt2.Focus()
            lblMsg.Text = "Error: Select Plant"
            Exit Sub

        ElseIf cboMMdesc.Text = "" Then
            cboMMdesc.Focus()
            lblMsg.Text = "Error: Select Material"
            Exit Sub

        End If

        dt = GetDataTable("select codeno,ifnull(admdigit,'2') from mmasttbl where " &
                          "ifnull(codename,mmdesc) = '" & cboMMdesc.Text & "'")
        If Not CBool(dt.Rows.Count) Then
            lblMsg.Text = "Error: Not found."
            Exit Sub
        Else
            For Each dr As DataRow In dt.Rows
                lblCodeNo.Text = dr.Item(0).ToString()
                lblAdmDig.Text = dr.Item(1).ToString()
            Next
        End If

        Call dt.Dispose()

        ResetInvAnalysis()

        GetLotNoInv()

    End Sub

    Private Sub GetLotNoInv()
        sql = "delete from tempdodet where user ='" & lblUser.Text & "'"
        ExecuteNonQuery(sql)

        'purchases
        sql = "insert into tempdodet(lotno,user,stdqty,stdwt) select a.lotno,'" & lblUser.Text & "'," &
              "ifnull(sum(a.qty),0),ifnull(sum(a.wt),0) from invdettbl a left join invhdrtbl b on a.mmrrno=b.mmrrno " &
              "where b.plntno = '" & cboPlnt2.Text.Substring(0, 3) & "' and b.status <> 'void' " &
              "and a.codeno = '" & lblCodeNo.Text & "' and b.transdate <= '" & Format(CDate(dpInvDate.Text), "yyyy-MM-dd") & "' " &
              "and b.tc = '10' group by a.lotno"
        ExecuteNonQuery(sql)

        'prodn
        If CheckBox13.Checked = True Then
            sql = "insert into tempdodet(lotno,user,stdqty,stdwt) select a.lotno,'" & lblUser.Text & "'," &
                  "ifnull(sum(a.qty),0),ifnull(sum(a.wt),0) from invdettbl a left join invhdrtbl b on a.mmrrno=b.mmrrno " &
                  "where b.plntno = '" & cboPlnt2.Text.Substring(0, 3) & "' and b.status <> 'void' " &
                  "and a.codeno = '" & lblCodeNo.Text & "' and b.transdate <= '" & Format(CDate(dpInvDate.Text), "yyyy-MM-dd") & "' " &
                  "and b.tc = '20' and b.dsrstat = 'Finished' group by a.lotno"
            ExecuteNonQuery(sql)
        Else
            sql = "insert into tempdodet(lotno,user,stdqty,stdwt) select a.lotno,'" & lblUser.Text & "'," &
                  "ifnull(sum(a.qty),0),ifnull(sum(a.wt),0) from invdettbl a left join invhdrtbl b on a.mmrrno=b.mmrrno " &
                  "where b.plntno = '" & cboPlnt2.Text.Substring(0, 3) & "' and b.status <> 'void' " &
                  "and a.codeno = '" & lblCodeNo.Text & "' and b.transdate <= '" & Format(CDate(dpInvDate.Text), "yyyy-MM-dd") & "' " &
                  "and b.tc = '20' group by a.lotno"
            ExecuteNonQuery(sql)
        End If



        sql = "insert into tempdodet(lotno,user,qty,wt) select a.lotno,'" & lblUser.Text & "',ifnull(sum(a.qty),0)," &
              "ifnull(sum(a.wt),0) from issdettbl a left join isshdrtbl b on a.dono=b.dono where " &
              "b.plntno = '" & cboPlnt2.Text.Substring(0, 3) & "' and a.codeno = '" & lblCodeNo.Text & "' " &
              "and b.status <> 'void' and b.transdate <= '" & Format(CDate(dpInvDate.Text), "yyyy-MM-dd") & "' group by a.lotno"
        ExecuteNonQuery(sql)

        sql = "insert into tempdodet(lotno,user,stdqty,stdwt) select a.lotno,'" & lblUser.Text & "'," &
              "ifnull(sum(a.qty),0),ifnull(sum(a.wt),0) from wrrdettbl a left join wrrhdrtbl b on a.wrrno=b.wrrno " &
              "where b.plntno = '" & cboPlnt2.Text.Substring(0, 3) & "' and a.codeno = '" & lblCodeNo.Text & "' " &
              "and b.status <> 'void' and b.transdate <= '" & Format(CDate(dpInvDate.Text), "yyyy-MM-dd") & "' group by a.lotno"
        ExecuteNonQuery(sql)

        cboLotNo.Items.Clear()

        dt = GetDataTable("select lotno from tempdodet where user = '" & lblUser.Text & "' group by lotno order by lotno")
        If Not CBool(dt.Rows.Count) Then
            lblMsg.Text = "No Lot No. Created yet for " & cboMMdesc.Text
            Exit Sub
        Else
            cboLotNo.Items.Add("")
            For Each dr As DataRow In dt.Rows
                cboLotNo.Items.Add(dr.Item(0).ToString())

            Next

        End If

        Call dt.Dispose()

    End Sub

    Private Sub cboLotNo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboLotNo.SelectedIndexChanged
        ResetInvAnalysis()

        If cboPlnt2.Text = "" Then
            cboPlnt2.Focus()
            lblMsg.Text = "Error: Select Plant"
            Exit Sub

        ElseIf cboMMdesc.Text = "" Then
            cboMMdesc.Focus()
            lblMsg.Text = "Error: Select Material"
            Exit Sub

        End If

        lbPrint.Enabled = True

        GetRecGoods()
        GetIssGoods()
        getBalanceAll()

    End Sub

    Private Sub getBalanceAll()
        Dim QtyRec As Long = CLng(IIf(lblInvQty.Text = "", 0, lblInvQty.Text))
        Dim WtRec As Double = CDbl(IIf(lblInvWt.Text = "", 0, lblInvWt.Text))
        Dim QtyIss As Long = CLng(IIf(lblIssQty.Text = "", 0, lblIssQty.Text))
        Dim WtIss As Double = CDbl(IIf(lblIssWt.Text = "", 0, lblIssWt.Text))

        lblBalQty.Text = Format(QtyRec - QtyIss, "#,##0 ; (#,##0)")

        Select Case lblAdmDig.Text
            Case "2"
                lblBalWt.Text = Format(WtRec - WtIss, "#,##0.00 ; (#,##0.00)")
            Case "3"
                lblBalWt.Text = Format(WtRec - WtIss, "#,##0.000 ;(#,##0.000)")
            Case "4"
                lblBalWt.Text = Format(WtRec - WtIss, "#,##0.0000 ; (#,##0.0000)")
            Case "5"
                lblBalWt.Text = Format(WtRec - WtIss, "#,##0.00000 ; (#,##0.00000)")
            Case "6"
                lblBalWt.Text = Format(WtRec - WtIss, "#,##0.000000 ; (#,##0.000000)")
            Case Else
                lblBalWt.Text = Format(WtRec - WtIss, "#,##0.00 ; (#,##0.00)")
        End Select
    End Sub

    Private Sub GetRecGoods()

        InsertRecToTemp()

        Dim wtRec As Double

        lvInv.DataSource = Nothing
        lvInv.DataBind()

        dt = GetDataTable("select sum(ifnull(qty,0)),sum(ifnull(wt,0)) from tempdodet " &
                          "where user = '" & lblUser.Text & "' group by user")
        If Not CBool(dt.Rows.Count) Then
            lblInvQty.Text = "0"
            lblInvWt.Text = "0.00"
            Exit Sub
        Else
            For Each dr As DataRow In dt.Rows
                lblInvQty.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0")
                wtRec = CDbl(dr.Item(1).ToString())

                Select Case lblAdmDig.Text
                    Case "2"
                        lblInvWt.Text = Format(wtRec, "#,##0.00")
                    Case "3"
                        lblInvWt.Text = Format(wtRec, "#,##0.000")
                    Case "4"
                        lblInvWt.Text = Format(wtRec, "#,##0.0000")
                    Case "5"
                        lblInvWt.Text = Format(wtRec, "#,##0.00000")
                    Case "6"
                        lblInvWt.Text = Format(wtRec, "#,##0.000000")
                    Case Else
                        lblInvWt.Text = Format(wtRec, "#,##0.00")
                End Select

            Next

        End If

        dt.Dispose()

        Dim adapter As New MySqlDataAdapter
        Dim ds As New DataSet()

        sqldata = Nothing

        sqldata = "select dono,transdate,qty,wt,tc,ifnull(uc,0) as uc,pono  from tempdodet " &
                  "where user = '" & lblUser.Text & "' order by dono"

        conn.Open()
        Dim command As New MySqlCommand(sqldata, conn)
        adapter.SelectCommand = command
        adapter.Fill(ds)
        adapter.Dispose()
        command.Dispose()
        conn.Close()
        lvInv.DataSource = ds.Tables(0)
        lvInv.DataBind()

    End Sub

    Private Sub GetIssGoods()
        lblIssRem.Text = ""

        lvIss.DataSource = Nothing
        lvIss.DataBind()

        PopIssDetAll()

        'Select Case UCase(cboMMtype.Text)
        '    Case "RAW MATERIALS", "PACKAGING"
        '        dt = GetDataTable("select sum(ifnull(a.qty,0)),sum(ifnull(a.wt,0)) from issdettbl a " &
        '                          "left join tempisshdrtbl b on a.dono=b.dono and b.user = '" & lblUser.Text & "' " &
        '                          "left join wipdettbl d on a.dono=d.dono left join custmasttbl c on b.shipto=c.custno " &
        '                          "where a.lotno = '" & cboLotNo.Text & "' and a.codeno = '" & lblCodeNo.Text & "' and " &
        '                          "b.plntno = '" & cboPlnt2.Text.Substring(0, 3) & "' and b.status <> 'void' and " &
        '                          "b.transdate <= '" & Format(CDate(dpInvDate.Text), "yyyy-MM-dd") & "' " &
        '                          "and b.user = '" & lblUser.Text & "' group by a.codeno,a.lotno")
        '    Case Else
        '        dt = GetDataTable("select sum(ifnull(a.qty,0)),sum(ifnull(a.wt,0)) from issdettbl a " &
        '                          "left join tempisshdrtbl b on a.dono=b.dono and b.user = '" & lblUser.Text & "' " &
        '                          "left join custmasttbl c on b.shipto=c.custno " &
        '                          "where a.lotno = '" & cboLotNo.Text & "' and a.codeno = '" & lblCodeNo.Text & "' and " &
        '                          "b.plntno = '" & cboPlnt2.Text.Substring(0, 3) & "' and b.status <> 'void' and " &
        '                          "b.transdate <= '" & Format(CDate(dpInvDate.Text), "yyyy-MM-dd") & "' " &
        '                          "and b.user = '" & lblUser.Text & "' group by a.codeno,a.lotno")
        'End Select

        'If Not CBool(dt.Rows.Count) Then
        '    lblIssQty.Text = "0"
        '    lblIssWt.Text = "0.00"
        '    lvIss.DataSource = Nothing
        '    lvIss.DataBind()

        'Else
        '    For Each dr As DataRow In dt.Rows
        '        lblIssQty.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0")
        '        wtIss = CDbl(dr.Item(1).ToString())

        '        Select Case lblAdmDig.Text
        '            Case "2"
        '                lblIssWt.Text = Format(wtIss, "#,##0.00")
        '            Case "3"
        '                lblIssWt.Text = Format(wtIss, "#,##0.000")
        '            Case "4"
        '                lblIssWt.Text = Format(wtIss, "#,##0.0000")
        '            Case "5"
        '                lblIssWt.Text = Format(wtIss, "#,##0.00000")
        '            Case "6"
        '                lblIssWt.Text = Format(wtIss, "#,##0.000000")
        '            Case Else
        '                lblIssWt.Text = Format(wtIss, "#,##0.00")
        '        End Select
        '    Next

        'End If

        Dim adapter As New MySqlDataAdapter
        Dim ds As New DataSet()

        sqldata = Nothing

        Select Case UCase(cboMMtype.Text)
            Case "RAW MATERIALS", "PACKAGING"
                sqldata = "select a.dono,b.transdate,ifnull(a.qty,0) as qty,ifnull(a.wt,0) as wt,b.tc," &
                          "concat(ifnull(d.wipno,''),' / ',b.remarks) as rem,b.mov from issdettbl a " &
                          "left join tempisshdrtbl b on a.dono=b.dono and b.user = '" & lblUser.Text & "' " &
                          "left join wipdettbl d on a.dono=d.dono left join custmasttbl c on b.shipto=c.custno " &
                          "where a.lotno = '" & cboLotNo.Text & "' and a.codeno = '" & lblCodeNo.Text & "' and " &
                          "b.plntno = '" & cboPlnt2.Text.Substring(0, 3) & "' and b.status <> 'void' and " &
                          "b.transdate <= '" & Format(CDate(dpInvDate.Text), "yyyy-MM-dd") & "' " &
                          "and b.user = '" & lblUser.Text & "' group by a.idno order by a.dono"
                lvIss.Columns(5).HeaderText = "PW No. / Finished Product"
            Case Else
                sqldata = "select a.dono,b.transdate,ifnull(a.qty,0) as qty,ifnull(a.wt,0) as wt,b.tc,b.remarks as rem,b.mov " &
                          "from issdettbl a left join tempisshdrtbl b on a.dono=b.dono And b.user = '" & lblUser.Text & "' " &
                          "left join custmasttbl c on b.shipto=c.custno where a.lotno = '" & cboLotNo.Text & "' and " &
                          "a.codeno = '" & lblCodeNo.Text & "' and b.plntno = '" & cboPlnt2.Text.Substring(0, 3) & "' " &
                          "and b.status <> 'void' and b.transdate <= '" & Format(CDate(dpInvDate.Text), "yyyy-MM-dd") & "' " &
                          "and b.user = '" & lblUser.Text & "' group by a.idno order by a.dono"
                lvIss.Columns(5).HeaderText = "Ref No./Customer/Plant/Remarks"
        End Select

        conn.Open()
        Dim command As New MySqlCommand(sqldata, conn)
        adapter.SelectCommand = command
        adapter.Fill(ds)
        adapter.Dispose()
        command.Dispose()
        conn.Close()
        lvIss.DataSource = ds.Tables(0)
        lvIss.DataBind()

        For i As Integer = 0 To lvIss.Rows.Count - 1
            qtyIss = qtyIss + CLng(lvIss.Rows(i).Cells(2).Text)
            wtIss = wtIss + CDbl(lvIss.Rows(i).Cells(3).Text)
        Next

        lblIssQty.Text = Format(CDbl(qtyIss), "#,##0")

        Select Case lblAdmDig.Text
            Case "2"
                lblIssWt.Text = Format(wtIss, "#,##0.00")
            Case "3"
                lblIssWt.Text = Format(wtIss, "#,##0.000")
            Case "4"
                lblIssWt.Text = Format(wtIss, "#,##0.0000")
            Case "5"
                lblIssWt.Text = Format(wtIss, "#,##0.00000")
            Case "6"
                lblIssWt.Text = Format(wtIss, "#,##0.000000")
            Case Else
                lblIssWt.Text = Format(wtIss, "#,##0.00")
        End Select

    End Sub

    Private Sub InsertRecToTemp()

        sql = "delete from tempdodet"
        ExecuteNonQuery(sql)

        'purchases
        sql = "insert into tempdodet(dono,transdate,qty,wt,tc,uc,user) select a.mmrrno,b.transdate,ifnull(a.qty,0)," &
              "ifnull(a.wt,0),b.tc,a.sp,'" & lblUser.Text & "' from invdettbl a left join invhdrtbl b on a.mmrrno=b.mmrrno " &
              "where a.lotno = '" & cboLotNo.Text & "' and a.codeno = '" & lblCodeNo.Text & "' and " &
              "b.plntno = '" & cboPlnt2.Text.Substring(0, 3) & "' and b.status <> 'void' and " &
              "b.transdate <= '" & Format(CDate(dpInvDate.Text), "yyyy-MM-dd") & "' and " &
              "b.tc='10' group by a.transid"
        ExecuteNonQuery(sql)

        'prodn
        Select Case vLoggedBussArea
            Case "8100"
                If CheckBox13.Checked = True Then
                    sql = "insert into tempdodet(dono,transdate,qty,wt,tc,uc,user,pono) select a.mmrrno,b.transdate,ifnull(a.qty,0)," &
                          "ifnull(a.wt,0),b.tc,a.sp,'" & lblUser.Text & "',ifnull(b.formno,'01') from invdettbl a " &
                          "left join invhdrtbl b on a.mmrrno=b.mmrrno where a.lotno = '" & cboLotNo.Text & "' and " &
                          "a.codeno = '" & lblCodeNo.Text & "' and b.plntno = '" & cboPlnt2.Text.Substring(0, 3) & "' " &
                          "and b.status <> 'void' and b.transdate <= '" & Format(CDate(dpInvDate.Text), "yyyy-MM-dd") & "' " &
                          "and b.tc='20' and b.dsrstat = 'Finished' group by a.transid"
                    ExecuteNonQuery(sql)

                Else
                    sql = "insert into tempdodet(dono,transdate,qty,wt,tc,uc,user,pono) select a.mmrrno,b.transdate,ifnull(a.qty,0)," &
                          "ifnull(a.wt,0),b.tc,a.sp,'" & lblUser.Text & "',ifnull(b.formno,'01') from invdettbl a " &
                          "left join invhdrtbl b on a.mmrrno=b.mmrrno where a.lotno = '" & cboLotNo.Text & "' and " &
                          "a.codeno = '" & lblCodeNo.Text & "' and b.plntno = '" & cboPlnt2.Text.Substring(0, 3) & "' " &
                          "and b.status <> 'void' and b.transdate <= '" & Format(CDate(dpInvDate.Text), "yyyy-MM-dd") & "' " &
                          "and b.tc='20' group by a.transid"
                    ExecuteNonQuery(sql)
                End If
            Case Else
                If CheckBox13.Checked = True Then
                    sql = "insert into tempdodet(dono,transdate,qty,wt,tc,uc,user,pono) select a.mmrrno,b.transdate,ifnull(a.qty,0)," &
                          "ifnull(a.wt,0),b.tc,a.sp,'" & lblUser.Text & "','' from invdettbl a left join invhdrtbl b on a.mmrrno=b.mmrrno " &
                          "where a.lotno = '" & cboLotNo.Text & "' and a.codeno = '" & lblCodeNo.Text & "' and b.plntno = '" & cboPlnt2.Text.Substring(0, 3) & "' " &
                          "and b.status <> 'void' and b.transdate <= '" & Format(CDate(dpInvDate.Text), "yyyy-MM-dd") & "' and " &
                          "b.tc='20' and b.dsrstat = 'Finished' group by a.transid"
                    ExecuteNonQuery(sql)

                Else
                    sql = "insert into tempdodet(dono,transdate,qty,wt,tc,uc,user,pono) select a.mmrrno,b.transdate,ifnull(a.qty,0)," &
                          "ifnull(a.wt,0),b.tc,a.sp,'" & lblUser.Text & "','' from invdettbl a left join invhdrtbl b on a.mmrrno=b.mmrrno " &
                          "where a.lotno = '" & cboLotNo.Text & "' and a.codeno = '" & lblCodeNo.Text & "' and b.plntno = '" & cboPlnt2.Text.Substring(0, 3) & "' " &
                          "and b.status <> 'void' and b.transdate <= '" & Format(CDate(dpInvDate.Text), "yyyy-MM-dd") & "' and " &
                          "b.tc='20' group by a.transid"
                    ExecuteNonQuery(sql)
                End If

        End Select


        sql = "insert into tempdodet(dono,transdate,qty,wt,tc,user) select a.wrrno,b.transdate,ifnull(a.qty,0)," &
              "ifnull(a.wt,0),b.tc,'" & lblUser.Text & "' from wrrdettbl a left join wrrhdrtbl b on a.wrrno=b.wrrno " &
              "where a.lotno = '" & cboLotNo.Text & "' and a.codeno = '" & lblCodeNo.Text & "' and b.plntno = '" & cboPlnt2.Text.Substring(0, 3) & "' " &
              "and b.status <> 'void' and b.transdate < '" & Format(CDate(dpInvDate.Text), "yyyy-MM-dd") & "' group by a.transid"
        ExecuteNonQuery(sql)

    End Sub

    Private Sub PopIssDetAll()
        dt = GetDataTable("select * from information_schema.tables where table_schema =  '" & vDbase & "' and " &
                          "table_name = 'tempisshdrtbl'")
        If Not CBool(dt.Rows.Count) Then
            sql = "CREATE TABLE tempisshdrtbl LIKE isshdrtbl"
            ExecuteNonQuery(sql)
        Else
            sql = "delete from tempisshdrtbl where user = '" & lblUser.Text & "'"
            ExecuteNonQuery(sql)
        End If

        dt.Dispose()

        dt = GetDataTable("select b.mov from issdettbl a left join isshdrtbl b on a.dono=b.dono where b.plntno =  '" & cboPlnt2.Text.Substring(0, 3) & "' " &
                          "and b.status <> 'void' and b.transdate <= '" & Format(CDate(dpInvDate.Text), "yyyy-MM-dd") & "' and " &
                          "a.codeno = '" & lblCodeNo.Text & "' and a.lotno = '" & cboLotNo.Text & "' group by b.mov")
        If Not CBool(dt.Rows.Count) Then
            lvIss.DataSource = Nothing
            lvIss.DataBind()
            Exit Sub
        Else
            For Each dr As DataRow In dt.Rows
                Select Case dr.Item(0).ToString()
                    Case "201"
                        Select Case vLoggedBussArea
                            Case "8100"
                                If CheckBox13.Checked = True Then
                                    sql = "insert into tempisshdrtbl(dono,transdate,tc,mov,remarks,plntno,status,user,pono) select a.dono,a.transdate,a.tc,a.mov,ifnull(c.codename,c.mmdesc)," &
                                          "a.plntno,a.status,'" & lblUser.Text & "',b.dsrno from isshdrtbl a left join issdettbl d on a.dono=d.dono " &
                                          "left join invhdrtbl b on a.dsrno=b.mmrrno left join mmasttbl c on b.pono=c.codeno left join wipdettbl e on d.dono=e.dono " &
                                          "where d.lotno = '" & cboLotNo.Text & "' and d.codeno = '" & lblCodeNo.Text & "' and " &
                                          "a.plntno = '" & cboPlnt2.Text.Substring(0, 3) & "' and a.status <> 'void' and a.mov = '201' and " &
                                          "a.transdate <= '" & Format(CDate(dpInvDate.Text), "yyyy-MM-dd") & "' " &
                                          "and b.dsrstat = 'Finished' and e.remarks is null  group by d.idno order by d.dono"
                                    ExecuteNonQuery(sql)
                                    'Manual
                                    sql = "insert into tempisshdrtbl(dono,transdate,tc,mov,remarks,plntno,status,user,pono) select a.dono,a.transdate,a.tc,a.mov,'Manual Entry'," &
                                          "a.plntno,a.status,'" & lblUser.Text & "',b.wipno from isshdrtbl a left join issdettbl d on a.dono=d.dono " &
                                          "left join wipdettbl b on a.dono=b.dono where d.lotno = '" & cboLotNo.Text & "' " &
                                          "and d.codeno = '" & lblCodeNo.Text & "' and a.plntno = '" & cboPlnt2.Text.Substring(0, 3) & "' and a.status <> 'void' " &
                                          "and a.mov = '201' and a.transdate <= '" & Format(CDate(dpInvDate.Text), "yyyy-MM-dd") & "' and " &
                                          "b.remarks = 'Manual Entry' group by d.idno order by d.dono"
                                    ExecuteNonQuery(sql)
                                Else
                                    sql = "insert into tempisshdrtbl(dono,transdate,tc,mov,remarks,plntno,status,user,pono) select a.dono,a.transdate,a.tc,a.mov,ifnull(c.codename,c.mmdesc)," &
                                          "a.plntno,a.status,'" & lblUser.Text & "',b.dsrno from isshdrtbl a left join issdettbl d on a.dono=d.dono " &
                                          "left join invhdrtbl b on a.dsrno=b.mmrrno left join mmasttbl c on b.pono=c.codeno left join wipdettbl e on d.dono=e.dono " &
                                          "where d.lotno = '" & cboLotNo.Text & "' " &
                                          "and d.codeno = '" & lblCodeNo.Text & "' and a.plntno = '" & cboPlnt2.Text.Substring(0, 3) & "' and a.status <> 'void' " &
                                          "and a.mov = '201' and a.transdate <= '" & Format(CDate(dpInvDate.Text), "yyyy-MM-dd") & "' " &
                                          "and e.remarks is null  group by d.idno order by d.dono"
                                    ExecuteNonQuery(sql)
                                    'Manual
                                    sql = "insert into tempisshdrtbl(dono,transdate,tc,mov,remarks,plntno,status,user,pono) select a.dono,a.transdate,a.tc,a.mov,'Manual Entry'," &
                                          "a.plntno,a.status,'" & lblUser.Text & "',b.wipno from isshdrtbl a left join issdettbl d on a.dono=d.dono " &
                                          "left join wipdettbl b on a.dono=b.dono where d.lotno = '" & cboLotNo.Text & "' " &
                                          "and d.codeno = '" & lblCodeNo.Text & "' and a.plntno = '" & cboPlnt2.Text.Substring(0, 3) & "' and a.status <> 'void' " &
                                          "and a.mov = '201' and a.transdate <= '" & Format(CDate(dpInvDate.Text), "yyyy-MM-dd") & "' and " &
                                          "b.remarks = 'Manual Entry' group by d.idno order by d.dono"
                                    ExecuteNonQuery(sql)
                                End If

                            Case Else
                                sql = "insert into tempisshdrtbl(dono,transdate,tc,mov,remarks,plntno,status,user,pono) select a.dono,a.transdate,a.tc,a.mov,c.mmdesc," &
                                      "a.plntno,a.status,'" & lblUser.Text & "',b.dsrno from isshdrtbl a left join issdettbl d on a.dono=d.dono " &
                                      "left join invhdrtbl b on a.dsrno=b.mmrrno left join mmasttbl c on b.pono=c.codeno where d.lotno = '" & cboLotNo.Text & "' " &
                                      "and d.codeno = '" & lblCodeNo.Text & "' and a.plntno = '" & cboPlnt2.Text.Substring(0, 3) & "' and a.status <> 'void' " &
                                      "and a.mov = '201' and a.transdate <= '" & Format(CDate(dpInvDate.Text), "yyyy-MM-dd") & "' group by d.idno order by d.dono"
                                ExecuteNonQuery(sql)

                        End Select

                    Case "301"
                        sql = "insert into tempisshdrtbl(dono,transdate,tc,mov,remarks,plntno,status,user) select a.dono,a.transdate,a.tc,a.mov," &
                              "concat(a.refdoc,' / ',a.plntdest,space(1),c.plntname),a.plntno,a.status,'" & lblUser.Text & "' from isshdrtbl a left join issdettbl d on a.dono=d.dono " &
                              "left join plnttbl c on a.plntdest = c.plntno where d.lotno = '" & cboLotNo.Text & "' and d.codeno = '" & lblCodeNo.Text & "' and " &
                              "a.plntno = '" & cboPlnt2.Text.Substring(0, 3) & "' and a.status <> 'void' and a.mov = '301' and " &
                              "a.transdate <= '" & Format(CDate(dpInvDate.Text), "yyyy-MM-dd") & "' group by d.idno order by d.dono"
                        ExecuteNonQuery(sql)

                    Case "309"
                        sql = "insert into tempisshdrtbl(dono,transdate,tc,mov,remarks,plntno,status,user) select a.dono,a.transdate,a.tc,a.mov," &
                              "ifnull(a.remarks,'Material to Material'),a.plntno,a.status,'" & lblUser.Text & "' from isshdrtbl a " &
                              "left join issdettbl d on a.dono=d.dono  where d.lotno = '" & cboLotNo.Text & "' and d.codeno = '" & lblCodeNo.Text & "' and " &
                              "a.plntno = '" & cboPlnt2.Text.Substring(0, 3) & "' and a.status <> 'void' and a.mov = '309' and " &
                              "a.transdate <= '" & Format(CDate(dpInvDate.Text), "yyyy-MM-dd") & "' group by d.idno order by d.dono"
                        ExecuteNonQuery(sql)

                    Case "601"
                        sql = "insert into tempisshdrtbl(dono,transdate,tc,mov,remarks,plntno,status,user) select a.dono," &
                              "a.transdate,a.tc,a.mov,concat(a.refdoc,' / ',c.bussname),a.plntno,a.status,'" & lblUser.Text & "' " &
                              "from isshdrtbl a left join issdettbl d on a.dono=d.dono " &
                              "left join custmasttbl c on ifnull(a.shipto,a.custno)=c.custno where " &
                              "d.lotno = '" & cboLotNo.Text & "' and d.codeno = '" & lblCodeNo.Text & "' and " &
                              "a.plntno = '" & cboPlnt2.Text.Substring(0, 3) & "' and a.status <> 'void' and a.mov = '601' and " &
                              "a.transdate <= '" & Format(CDate(dpInvDate.Text), "yyyy-MM-dd") & "' group by d.idno order by d.dono"
                        ExecuteNonQuery(sql)

                    Case "611"
                        sql = "insert into tempisshdrtbl(dono,transdate,tc,mov,remarks,plntno,status,user) select a.dono,a.transdate,a.tc,a.mov,c.bussname," &
                              "a.plntno,a.status,'" & lblUser.Text & "' from isshdrtbl a left join issdettbl d on a.dono=d.dono " &
                              "left join custmasttbl c on ifnull(a.shipto,a.custno)=c.custno where " &
                              "d.lotno = '" & cboLotNo.Text & "' and d.codeno = '" & lblCodeNo.Text & "' and " &
                              "a.plntno = '" & cboPlnt2.Text.Substring(0, 3) & "' and a.status <> 'void' and a.mov = '611' and " &
                              "a.transdate <= '" & Format(CDate(dpInvDate.Text), "yyyy-MM-dd") & "' group by d.idno order by d.dono"
                        ExecuteNonQuery(sql)

                    Case Else
                        sql = "insert into tempisshdrtbl(dono,transdate,tc,mov,remarks,plntno,status,user) select a.dono,a.transdate,a.tc,a.mov,a.remarks," &
                              "a.plntno,a.status,'" & lblUser.Text & "' from isshdrtbl a left join issdettbl d on a.dono=d.dono where " &
                              "d.lotno = '" & cboLotNo.Text & "' and d.codeno = '" & lblCodeNo.Text & "' and a.plntno = '" & cboPlnt2.Text.Substring(0, 3) & "' and " &
                              "a.status <> 'void' and a.transdate <= '" & Format(CDate(dpInvDate.Text), "yyyy-MM-dd") & "' and " &
                              "(a.mov <> '201' and a.mov <> '301' and a.mov <> '309' and a.mov <> '601' and a.mov <> '611') group by d.idno order by d.dono"
                        ExecuteNonQuery(sql)

                End Select

            Next

        End If

        Call dt.Dispose()

    End Sub

    Private Sub InventoryReports_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete


    End Sub

    Protected Sub dgvSOmonSum_RowDataBound(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Protected Sub dgvSOmonSum_RowCreated(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Private Sub dpSOdate1_TextChanged(sender As Object, e As EventArgs) Handles dpSOdate1.TextChanged, dpSOdate2.TextChanged
        If dpSOdate1.Text = Nothing Then
            Exit Sub
        ElseIf dpSOdate2.Text = Nothing Then
            If CheckBox1.Checked = True Then

            Else
                Exit Sub
            End If

        ElseIf Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") > Format(CDate(dpSOdate2.Text), "yyyy-MM-dd") Then
            If CheckBox1.Checked = True Then

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

    Private Sub GetMMgrpSO()
        cboMMGrpSO.Items.Clear()
        If CheckBox1.Checked = True Then
            dt = GetDataTable("select c.mmgrp from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
                              "left join mmasttbl c on a.codeno=c.codeno where c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' " &
                              "and b.transdate <= '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' and " &
                              "(a.qtbal > 0 or a.wtbal > 0) and b.delstat <> 'Served' and b.status = 'approved' group by c.mmgrp")
        Else
            dt = GetDataTable("select c.mmgrp from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
                              "left join mmasttbl c on a.codeno=c.codeno where c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' " &
                              "and b.transdate between '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' " &
                              "and '" & Format(CDate(dpSOdate2.Text), "yyyy-MM-dd") & "' and (a.qtbal > 0 or a.wtbal > 0) " &
                              "and b.delstat <> 'Served' and b.status = 'approved' group by c.mmgrp")
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

    Private Sub cboPClassSO_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboPClassSO.SelectedIndexChanged
        If cboPClassSO.Text = "" Then
            Exit Sub

        End If

        GetMMgrpSO()

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

        GetSmnCustList()

    End Sub

    Private Sub GetSmnCustList()

        cboSmnCust.Items.Clear()

        If CheckBox1.Checked = True Then
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

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked = True Then
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

    Private Sub cboSmnCust_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboSmnCust.SelectedIndexChanged
        If cboSmnCust.Text = "" Then
            Exit Sub
        End If

        'TabContainer2
        Select Case TabContainer2.ActiveTabIndex
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
                If CheckBox1.Checked = True Then
                    Select Case cboMMGrpSO.Text
                        Case "ALL"
                            dt = GetDataTable("select sum(ifnull(a.qtbal,0)),sum(ifnull(a.wtbal,0)) from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
                                              "left join mmasttbl c on a.codeno=c.codeno where c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
                                              "b.transdate <= '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' and " &
                                              "(a.qtbal > 0 or a.wtbal > 0) and b.delstat <> 'Served' and b.status = 'approved'")
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
                                      "left join custmasttbl d on b.custno=d.custno where c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' " &
                                      "and b.transdate <= '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' and " &
                                      "(a.qtbal > 0 or a.wtbal > 0) and b.delstat <> 'Served' and b.status = 'approved' " &
                                      "group by a.transid order by d.bussname,b.transdate"
                        Case Else
                            dt = GetDataTable("select sum(ifnull(a.qtbal,0)),sum(ifnull(a.wtbal,0)) from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
                                              "left join mmasttbl c on a.codeno=c.codeno where c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
                                              "b.transdate <= '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' and " &
                                              "(a.qtbal > 0 or a.wtbal > 0) and b.delstat <> 'Served' and b.status = 'approved' " &
                                              "and c.mmgrp = '" & cboMMGrpSO.Text & "' group by c.mmgrp")
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
                                      "and c.mmgrp = '" & cboMMGrpSO.Text & "' group by a.transid order by d.bussname,b.transdate"
                    End Select
                Else
                    Select Case cboMMGrpSO.Text
                        Case "ALL"
                            dt = GetDataTable("select sum(ifnull(a.qtbal,0)),sum(ifnull(a.wtbal,0)) from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
                                              "left join mmasttbl c on a.codeno=c.codeno where c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
                                              "b.transdate between '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' and " &
                                              "'" & Format(CDate(dpSOdate2.Text), "yyyy-MM-dd") & "' and (a.qtbal > 0 or a.wtbal > 0) " &
                                              "and b.delstat <> 'Served' and b.status = 'approved'")
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
                                      "and b.delstat <> 'Served' and b.status = 'approved' group by a.codeno order by wtbal desc"
                        Case Else
                            dt = GetDataTable("select sum(ifnull(a.qtbal,0)),sum(ifnull(a.wtbal,0)) from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
                                              "left join mmasttbl c on a.codeno=c.codeno where c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
                                              "b.transdate between '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' and " &
                                              "'" & Format(CDate(dpSOdate2.Text), "yyyy-MM-dd") & "' and (a.qtbal > 0 or a.wtbal > 0) " &
                                              "and b.delstat <> 'Served' and b.status = 'approved' and c.mmgrp = '" & cboMMGrpSO.Text & "' group by c.mmgrp")
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
                                      "and b.delstat <> 'Served' and b.status = 'approved' " &
                                      "and c.mmgrp = '" & cboMMGrpSO.Text & "' group by a.codeno order by wtbal desc"
                    End Select
                End If
            Case Else
                Select Case True
                    Case rbMon1.Checked
                        If CheckBox1.Checked = True Then
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
                        If CheckBox1.Checked = True Then
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
                If CheckBox1.Checked = True Then
                    Select Case cboMMGrpSO.Text
                        Case "ALL"
                            dt = GetDataTable("select sum(ifnull(a.qtbal,0)),sum(ifnull(a.wtbal,0)) from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
                                              "left join mmasttbl c on a.codeno=c.codeno where c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
                                              "b.transdate <= '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' and " &
                                              "(a.qtbal > 0 or a.wtbal > 0) and b.delstat <> 'Served' and b.status = 'approved'")
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
                                      "group by a.codeno order by wtbal desc"
                        Case Else
                            dt = GetDataTable("select sum(ifnull(a.qtbal,0)),sum(ifnull(a.wtbal,0)) from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
                                              "left join mmasttbl c on a.codeno=c.codeno where c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
                                              "b.transdate <= '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' and " &
                                              "(a.qtbal > 0 or a.wtbal > 0) and b.delstat <> 'Served' and b.status = 'approved' " &
                                              "and c.mmgrp = '" & cboMMGrpSO.Text & "' group by c.mmgrp")
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
                                      "and c.mmgrp = '" & cboMMGrpSO.Text & "' group by a.codeno order by wtbal desc"
                    End Select
                Else
                    Select Case cboMMGrpSO.Text
                        Case "ALL"
                            dt = GetDataTable("select sum(ifnull(a.qtbal,0)),sum(ifnull(a.wtbal,0)) from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
                                              "left join mmasttbl c on a.codeno=c.codeno where c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
                                              "b.transdate between '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' and " &
                                              "'" & Format(CDate(dpSOdate2.Text), "yyyy-MM-dd") & "' and (a.qtbal > 0 or a.wtbal > 0) " &
                                              "and b.delstat <> 'Served' and b.status = 'approved'")
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
                                      "and b.delstat <> 'Served' and b.status = 'approved' group by a.codeno order by wtbal desc"
                        Case Else
                            dt = GetDataTable("select sum(ifnull(a.qtbal,0)),sum(ifnull(a.wtbal,0)) from sodettbl a left join sohdrtbl b on a.sono=b.sono " &
                                              "left join mmasttbl c on a.codeno=c.codeno where c.pc = '" & cboPClassSO.Text.Substring(0, 1) & "' and " &
                                              "b.transdate between '" & Format(CDate(dpSOdate1.Text), "yyyy-MM-dd") & "' and " &
                                              "'" & Format(CDate(dpSOdate2.Text), "yyyy-MM-dd") & "' and (a.qtbal > 0 or a.wtbal > 0) " &
                                              "and b.delstat <> 'Served' and b.status = 'approved' and c.mmgrp = '" & cboMMGrpSO.Text & "' group by c.mmgrp")
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
                                      "and b.delstat <> 'Served' and b.status = 'approved' " &
                                      "and c.mmgrp = '" & cboMMGrpSO.Text & "' group by a.codeno order by wtbal desc"
                    End Select
                End If
            Case Else
                Select Case True
                    Case rbMon1.Checked
                        If CheckBox1.Checked = True Then
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
                        If CheckBox1.Checked = True Then
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

    Protected Sub dgvPerSO_RowDataBound(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Protected Sub dgvPerSO_RowCreated(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Private Sub TabContainer2_ActiveTabChanged(sender As Object, e As EventArgs) Handles TabContainer2.ActiveTabChanged
        Select Case TabContainer2.ActiveTabIndex
            Case 0

            Case 1

        End Select
    End Sub

    Protected Sub lbExcelSO_Click(sender As Object, e As EventArgs)
        Dim admGrid As New GridView
        Response.ClearContent()

        Select Case TabContainer2.ActiveTabIndex
            Case 0
                admGrid = dgvSOmonSum

                If admGrid.Rows.Count = 0 Then
                    lblMsg.Text = "No Summary Per SKU Listed"
                    Exit Sub

                End If

            Case 1
                admGrid = dgvPerSO

                If admGrid.Rows.Count = 0 Then
                    lblMsg.Text = "No Per SO Listed"
                    Exit Sub
                End If

        End Select

        If CheckBox1.Checked = True Then
            Select Case True
                Case rbMon1.Checked
                    admDesc = "Smn_as_of " & Format(CDate(dpSOdate1.Text), "MMMddyyyy") & "_" & cboSmnCust.Text
                Case rbMon2.Checked
                    admDesc = "Cust_as_of " & Format(CDate(dpSOdate1.Text), "MMMddyyyy") & "_" & cboSmnCust.Text
            End Select
        Else
            Select Case True
                Case rbMon1.Checked
                    admDesc = "Smn_Dated " & Format(CDate(dpSOdate1.Text), "MMMddyyyy") & "_" & Format(CDate(dpSOdate2.Text), "MMMddyyyy") & "_" & cboSmnCust.Text
                Case rbMon2.Checked
                    admDesc = "Cust_Dated " & Format(CDate(dpSOdate1.Text), "MMMddyyyy") & "_" & Format(CDate(dpSOdate2.Text), "MMMddyyyy") & "_" & cboSmnCust.Text
            End Select

        End If

        Response.AddHeader("content-disposition", "attachment; filename=" & admDesc & ".xls")

        Response.ContentType = "application/excel"
        Dim sw As New System.IO.StringWriter()
        Dim htw As New HtmlTextWriter(sw)
        admGrid.RenderControl(htw)

        Response.Write(sw.ToString())
        Response.[End]()

    End Sub

    Public Overloads Overrides Sub VerifyRenderingInServerForm(ByVal c As Control)

    End Sub

    Private Sub cboPCinvAna_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboPCinvAna.SelectedIndexChanged
        If cboMMtype.Text = "" Then
            Exit Sub
        ElseIf cboPCinvAna.Text = "" Then
            Exit Sub
        End If

        ResetInvAnalysis()
        GetMMdesc()

    End Sub

    Protected Sub lbExcelT2_Click(sender As Object, e As EventArgs)
        If rbLeft.Checked = False And rbRight.Checked = False Then
            lblMsg.Text = "Please Select Data to be Exported"
            Exit Sub

        End If

        Select Case True
            Case rbRight.Checked
                If lvInv.Rows.Count = 0 Then
                    lblMsg.Text = "No Incoming Inventory to be Exported"
                    Exit Sub
                End If

            Case rbLeft.Checked
                If lvIss.Rows.Count = 0 Then
                    lblMsg.Text = "No Issuance Data to be Exported"
                    Exit Sub
                End If

        End Select

        ExportToExcelAna()

    End Sub

    Private Sub ExportToExcelAna()
        Dim admGrid As New GridView
        Response.ClearContent()

        Select Case True
            Case rbLeft.Checked
                admGrid = lvInv
                admDesc = "Recd_" & lblCodeNo.Text & "_LotNo_" & cboLotNo.Text

            Case rbRight.Checked
                admGrid = lvIss
                admDesc = "Iss_" & lblCodeNo.Text & "_LotNo_" & cboLotNo.Text
        End Select

        Response.AddHeader("content-disposition", "attachment; filename=" & admDesc & ".xls")

        Response.ContentType = "application/excel"
        Dim sw As New System.IO.StringWriter()
        Dim htw As New HtmlTextWriter(sw)
        admGrid.RenderControl(htw)

        Response.Write(sw.ToString())
        Response.[End]()

    End Sub

    Private Sub cboMMGrpSO_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboMMGrpSO.SelectedIndexChanged

    End Sub

    Private Sub cboRepFormatPA_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboRepFormatPA.SelectedIndexChanged
        If dpDatePA1.Text = Nothing Then
            Exit Sub
        ElseIf dpDatePA2.Text = Nothing Then
            Exit Sub
        End If

        If CDate(dpDatePA1.Text) > CDate(dpDatePA2.Text) Then
            lblMsg.Text = ("Select Receiving Type")
            Exit Sub
        ElseIf RadioButton8.Checked = False And RadioButton9.Checked = False Then
            lblMsg.Text = ("Select Receiving Type")
            Exit Sub
        End If

        GetPlantFGana()

    End Sub

    Private Sub GetPlantFGana()

        Select Case cboRepFormatPA.Text
            Case "RM vs FG Manual Entry"
                'get plant
                dt = GetDataTable("select * from information_schema.tables where table_schema =  '" & vDbase & "' and " &
                                  "table_name = 'tempplnttbl'")
                If Not CBool(dt.Rows.Count) Then
                    sql = "CREATE TABLE tempplnttbl LIKE tempinvtbl"
                    ExecuteNonQuery(sql)

                    sql = "ALTER TABLE tempplnttbl CHANGE lotno plntno VARCHAR(15)"
                    ExecuteNonQuery(sql)

                Else
                    sql = "delete from tempplnttbl where user = '" & lblUser.Text & "'"
                    ExecuteNonQuery(sql)

                End If

                dt.Dispose()

                sql = "insert into tempplnttbl(plntno,user) select plntno,'" & lblUser.Text & "' from isshdrtbl where " &
                      "transdate between '" & Format(CDate(dpDatePA1.Text), "yyyy-MM-dd") & "' " &
                      "and '" & Format(CDate(dpDatePA2.Text), "yyyy-MM-dd") & "' and mov = '201' and " &
                      "status <> 'void'"
                ExecuteNonQuery(sql)

                sql = "insert into tempplnttbl(plntno,user) select plntno,'" & lblUser.Text & "' from invhdrtbl where " &
                      "transdate between '" & Format(CDate(dpDatePA1.Text), "yyyy-MM-dd") & "' " &
                      "and '" & Format(CDate(dpDatePA2.Text), "yyyy-MM-dd") & "' and mov between '801' and '803' and " &
                      "status <> 'void'"
                ExecuteNonQuery(sql)

                cboPlntPA.Items.Clear()
                dt = GetDataTable("select distinct concat(a.plntno,space(1),b.plntname) from tempplnttbl a left join plnttbl b on a.plntno=b.plntno " &
                                  "where a.user = '" & lblUser.Text & "'")
                If Not CBool(dt.Rows.Count) Then
                    lblMsg.Text = "No Plant Found"
                    Exit Sub
                Else
                    cboPlntPA.Items.Add("")
                    For Each dr As DataRow In dt.Rows
                        cboPlntPA.Items.Add(dr.Item(0).ToString() & "")

                    Next

                End If

                dt.Dispose()

        End Select


    End Sub

    Private Sub InitTab4()
        dgvRmFg.DataSource = Nothing
        dgvRmFg.DataBind()
        txtTotRMwt.Text = "0.00"
        txtTotFGwt.Text = "0.00"
        txtTotVarWt.Text = "0.00"
        dgvRMdet.DataSource = Nothing
        dgvRMdet.DataBind()
        txtTotRMpa.Text = "0.0000000"
        dgvFGdet.DataSource = Nothing
        dgvFGdet.DataBind()
        txtTotFGpaQty.Text = "0"
        txtTotFGpaWt.Text = "0.00"

    End Sub

    Private Sub cboPlntPA_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboPlntPA.SelectedIndexChanged
        If cboRepFormatPA.Text = "" Then
            Exit Sub
        ElseIf cboPlntPA.Text = "" Then
            Exit Sub

        End If

        FilldgvRmFGproc()

    End Sub

    Private Sub FilldgvRmFGproc()
        Select Case cboRepFormatPA.Text
            Case "RM vs FG Manual Entry"
                FilldgvOthrRepRMFG()

        End Select


    End Sub

    Private Sub FilldgvOthrRepRMFG()
        sql = "delete from tempinvtbl where user = '" & lblUser.Text & "'"
        ExecuteNonQuery(sql)

        Select Case True
            Case RadioButton8.Checked
                sql = "insert into tempinvtbl(transdate,Wt_In,user) select b.transdate,sum(ifnull(a.wt,0))," &
                      "'" & lblUser.Text & "' from issdettbl a left join isshdrtbl b on a.dono=b.dono " &
                      "left join mmasttbl c on a.codeno=c.codeno where c.mmtype <> 'Packaging' and " &
                      "b.transdate between '" & Format(CDate(dpDatePA1.Text), "yyyy-MM-dd") & "' and " &
                      "'" & Format(CDate(dpDatePA2.Text), "yyyy-MM-dd") & "' and b.mov='201' and " &
                      "b.status <> 'void' and b.plntno = '" & cboPlntPA.Text.Substring(0, 3) & "' group by b.transdate"
                ExecuteNonQuery(sql)

                sql = "insert into tempinvtbl(transdate,Wt_out,user) select b.transdate,sum(ifnull(a.wt,0))," &
                      "'" & lblUser.Text & "' from invdettbl a left join invhdrtbl b on a.mmrrno=b.mmrrno " &
                      "left join mmasttbl c on a.codeno=c.codeno where " &
                      "b.transdate between '" & Format(CDate(dpDatePA1.Text), "yyyy-MM-dd") & "' and " &
                      "'" & Format(CDate(dpDatePA2.Text), "yyyy-MM-dd") & "' and b.mov='803' and " &
                      "b.status <> 'void' and b.plntno = '" & cboPlntPA.Text.Substring(0, 3) & "' group by b.transdate" 'c.produce = 'Yes' and 
                ExecuteNonQuery(sql)

            Case RadioButton9.Checked
                sql = "insert into tempinvtbl(transdate,Wt_In,user) select b.transdate,sum(ifnull(a.wt,0))," &
                      "'" & lblUser.Text & "' from issdettbl a left join isshdrtbl b on a.dono=b.dono " &
                      "left join invhdrtbl c on b.dsrno = c.mmrrno where " &
                      "b.transdate between '" & Format(CDate(dpDatePA1.Text), "yyyy-MM-dd") & "' and " &
                      "'" & Format(CDate(dpDatePA2.Text), "yyyy-MM-dd") & "' and b.mov='201' and " &
                      "b.status <> 'void' and b.plntno = '" & cboPlntPA.Text.Substring(0, 3) & "' and " &
                      "c.dsrstat = 'Finished' group by b.transdate"
                ExecuteNonQuery(sql)

                sql = "insert into tempinvtbl(transdate,Wt_out,user) select b.transdate,sum(ifnull(a.wt,0))," &
                      "'" & lblUser.Text & "' from invdettbl a left join invhdrtbl b on a.mmrrno=b.mmrrno where " &
                      "b.transdate between '" & Format(CDate(dpDatePA1.Text), "yyyy-MM-dd") & "' and " &
                      "'" & Format(CDate(dpDatePA2.Text), "yyyy-MM-dd") & "' and b.mov='801' and " &
                      "b.status <> 'void' and b.plntno = '" & cboPlntPA.Text.Substring(0, 3) & "' and " &
                      "b.dsrstat = 'Finished' group by b.transdate"
                ExecuteNonQuery(sql)

        End Select

        sql = "delete from tempinvtbl2 where user = '" & lblUser.Text & "'"
        ExecuteNonQuery(sql)

        sql = "insert into tempinvtbl2(transdate,wt_in,Wt_out,Wt_Bal,user) select transdate,sum(ifnull(wt_in,0))," &
              "sum(ifnull(wt_out,0)),sum(ifnull(wt_in,0))-sum(ifnull(wt_out,0)),'" & lblUser.Text & "' from " &
              "tempinvtbl where user = '" & lblUser.Text & "' group by transdate"
        ExecuteNonQuery(sql)

        dt = GetDataTable("select sum(wt_in),sum(Wt_out),sum(Wt_Bal) from tempinvtbl2 where " &
                          "user = '" & lblUser.Text & "' group by user")
        If Not CBool(dt.Rows.Count) Then
            lblMsg.Text = ("No Record Found...")
            dgvRmFg.DataSource = Nothing
            dgvRmFg.DataBind()
            txtTotRMwt.Text = "0.00"
            txtTotFGwt.Text = "0.00"
            txtTotVarWt.Text = "0.00"
            Exit Sub
        Else
            For Each dr As DataRow In dt.Rows
                txtTotRMwt.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0.00 ; (#,##0.00)")
                txtTotFGwt.Text = Format(CDbl(dr.Item(1).ToString()), "#,##0.00 ; (#,##0.00)")
                txtTotVarWt.Text = Format(CDbl(dr.Item(2).ToString()), "#,##0.00 ; (#,##0.00)")

            Next

        End If

        dt.Dispose()

        Dim adapter As New MySqlDataAdapter
        Dim ds As New DataSet()

        sqldata = Nothing

        sqldata = "select transdate,wt_in,Wt_out,Wt_Bal,ifnull(Wt_Bal,0) / ifnull(wt_in,0) as varper " &
                  "from tempinvtbl2 where user = '" & lblUser.Text & "'"

        conn.Open()
        Dim command As New MySqlCommand(sqldata, conn)
        adapter.SelectCommand = command
        adapter.Fill(ds)
        adapter.Dispose()
        command.Dispose()
        conn.Close()
        dgvRmFg.DataSource = ds.Tables(0)
        dgvRmFg.DataBind()

    End Sub

    Protected Sub dgvRmFg_RowDataBound(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Protected Sub dgvRmFg_RowCreated(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Protected Sub lbNew_Click(sender As Object, e As EventArgs)

        Select Case TabContainer1.ActiveTabIndex
            Case 0

            Case 1

            Case 2

            Case 3

            Case 4
                cboRepFormatPA.Items.Clear()
                cboRepFormatPA.Items.Add("")
                cboRepFormatPA.Items.Add("RM vs FG Manual Entry")

                Select Case vLoggedBussArea
                    Case "8100"
                        RadioButton9.Checked = True
                    Case "8300"
                        RadioButton8.Checked = True
                End Select

                InitTab4()

        End Select

    End Sub

    Protected Sub dgvRMdet_RowDataBound(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Protected Sub dgvRMdet_RowCreated(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Private Sub dgvRmFg_SelectedIndexChanged(sender As Object, e As EventArgs) Handles dgvRmFg.SelectedIndexChanged
        If dgvRmFg.Rows.Count = 0 Then
            Exit Sub
        End If

        FilldgvRMpa()
        FilldgvFGpa()

    End Sub

    Private Sub FilldgvRMpa()
        Dim adapter As New MySqlDataAdapter
        Dim ds As New DataSet()
        sqldata = Nothing

        Select Case True
            Case RadioButton8.Checked
                dt = GetDataTable("select sum(ifnull(a.wt,0)) from issdettbl a left join isshdrtbl b on a.dono=b.dono " &
                                  "left join mmasttbl c on a.codeno=c.codeno where c.mmtype <> 'Packaging' and " &
                                  "b.transdate =  '" & Format(CDate(dgvRmFg.SelectedRow.Cells(1).Text), "yyyy-MM-dd") & "' " &
                                  "and b.mov='201' and b.plntno = '" & cboPlntPA.Text.Substring(0, 3) & "' " &
                                  "and b.status <> 'void' group by b.transdate")
                If Not CBool(dt.Rows.Count) Then
                    lblMsg.Text = "No Record Found..."
                    dgvRMdet.DataSource = Nothing
                    dgvRMdet.DataBind()
                    txtTotRMpa.Text = "0.0000000"
                    Exit Sub
                Else
                    For Each dr As DataRow In dt.Rows
                        txtTotRMpa.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0.000000 ; (#,##0.000000)")

                    Next

                End If

                dt.Dispose()

                sqldata = "select a.dono,ifnull(c.codename,c.mmdesc) as mmdesc,b.user,a.lotno,sum(a.wt) as wt " &
                          "from issdettbl a left join isshdrtbl b on a.dono=b.dono left join mmasttbl c on a.codeno=c.codeno " &
                          "where b.transdate = '" & Format(CDate(dgvRmFg.SelectedRow.Cells(1).Text), "yyyy-MM-dd") & "' " &
                          "and b.mov='201' and b.plntno = '" & cboPlntPA.Text.Substring(0, 3) & "' and b.status <> 'void' " &
                          "and c.mmtype <> 'Packaging' group by a.dono,a.codeno,a.lotno order by wt desc"

            Case RadioButton9.Checked
                dt = GetDataTable("select sum(ifnull(a.wt,0)) from issdettbl a left join isshdrtbl b on a.dono=b.dono " &
                                  "left join invhdrtbl c on b.dsrno = c.mmrrno where " &
                                  "b.transdate = '" & Format(CDate(dgvRmFg.SelectedRow.Cells(1).Text), "yyyy-MM-dd") & "' " &
                                  "and b.mov='201' and b.plntno = '" & cboPlntPA.Text.Substring(0, 3) & "' " &
                                  "and b.status <> 'void' and c.dsrstat = 'Finished' group by b.transdate")
                If Not CBool(dt.Rows.Count) Then
                    lblMsg.Text = "No Record Found..."
                    dgvRMdet.DataSource = Nothing
                    dgvRMdet.DataBind()
                    txtTotRMpa.Text = "0.0000000"
                    Exit Sub
                Else
                    For Each dr As DataRow In dt.Rows
                        txtTotRMpa.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0.000000 ; (#,##0.000000)")

                    Next

                End If

                dt.Dispose()

                sqldata = "select a.dono,ifnull(c.codename,c.mmdesc) as mmdesc,b.user,a.lotno,sum(ifnull(a.wt,0)) as wt " &
                          "from issdettbl a left join isshdrtbl b on a.dono=b.dono left join mmasttbl c on a.codeno=c.codeno " &
                          "left join invhdrtbl d on b.dsrno =d.mmrrno where b.transdate = '" & Format(CDate(dgvRmFg.SelectedRow.Cells(1).Text), "yyyy-MM-dd") & "' " &
                          "and b.mov='201' and b.plntno = '" & cboPlntPA.Text.Substring(0, 3) & "' and b.status <> 'void' " &
                          "and d.dsrstat = 'Finished' group by a.dono,a.codeno,a.lotno order by wt desc"
        End Select

        conn.Open()
        Dim command As New MySqlCommand(sqldata, conn)
        adapter.SelectCommand = command
        adapter.Fill(ds)
        adapter.Dispose()
        command.Dispose()
        conn.Close()
        dgvRMdet.DataSource = ds.Tables(0)
        dgvRMdet.DataBind()


    End Sub

    Private Sub FilldgvFGpa()
        Dim adapter As New MySqlDataAdapter
        Dim ds As New DataSet()
        sqldata = Nothing

        Select Case True
            Case RadioButton8.Checked
                dt = GetDataTable("select sum(ifnull(a.qty,0)),sum(ifnull(a.wt,0)) from invdettbl a left join invhdrtbl b " &
                                  "on a.mmrrno=b.mmrrno where b.plntno = '" & cboPlntPA.Text.Substring(0, 3) & "' and " &
                                  "b.transdate =  '" & Format(CDate(dgvRmFg.SelectedRow.Cells(1).Text), "yyyy-MM-dd") & "' " &
                                  "and b.mov='803' and b.status <> 'void' group by b.transdate")
                If Not CBool(dt.Rows.Count) Then
                    lblMsg.Text = "No Record Found..."
                    dgvFGdet.DataSource = Nothing
                    dgvFGdet.DataBind()
                    txtTotFGpaQty.Text = "0"
                    txtTotFGpaWt.Text = "0.00"
                    Exit Sub
                Else
                    For Each dr As DataRow In dt.Rows
                        txtTotFGpaQty.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0 ; (#,##0)")
                        txtTotFGpaWt.Text = Format(CDbl(dr.Item(1).ToString()), "#,##0.00 ; (#,##0.00)")
                    Next

                End If

                dt.Dispose()

                sqldata = "select a.mmrrno,ifnull(c.codename,c.mmdesc) as mmdesc," &
                          "b.user,a.lotno,sum(a.qty) as qty,sum(a.wt) as wt from invdettbl a left join invhdrtbl b on a.mmrrno=b.mmrrno " &
                          "left join mmasttbl c on a.codeno=c.codeno where b.mov='803' and " &
                          "b.transdate = '" & Format(CDate(dgvRmFg.SelectedRow.Cells(1).Text), "yyyy-MM-dd") & "' " &
                          "and b.plntno = '" & cboPlntPA.Text.Substring(0, 3) & "' and b.status <> 'void' " &
                          "group by a.mmrrno,a.codeno,a.lotno"

            Case RadioButton9.Checked
                dt = GetDataTable("select sum(ifnull(a.qty,0)),sum(ifnull(a.wt,0)) from invdettbl a left join invhdrtbl b " &
                                  "on a.mmrrno=b.mmrrno where b.plntno = '" & cboPlntPA.Text.Substring(0, 3) & "' and " &
                                  "b.transdate =  '" & Format(CDate(dgvRmFg.SelectedRow.Cells(1).Text), "yyyy-MM-dd") & "' " &
                                  "and b.mov='801' and b.status <> 'void' and b.dsrstat = 'Finished' group by b.transdate")
                If Not CBool(dt.Rows.Count) Then
                    lblMsg.Text = "No Record Found..."
                    dgvFGdet.DataSource = Nothing
                    dgvFGdet.DataBind()
                    txtTotFGpaQty.Text = "0"
                    txtTotFGpaWt.Text = "0.00"
                    Exit Sub
                Else
                    For Each dr As DataRow In dt.Rows
                        txtTotFGpaQty.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0 ; (#,##0)")
                        txtTotFGpaWt.Text = Format(CDbl(dr.Item(1).ToString()), "#,##0.00 ; (#,##0.00)")
                    Next

                End If

                dt.Dispose()

                dt = GetDataTable("select sum(ifnull(TactWt,0))- sum(ifnull(twt,0)) from invhdrtbl where " &
                                  "plntno = '" & cboPlntPA.Text.Substring(0, 3) & "' and mov='801' and " &
                                  "transdate =  '" & Format(CDate(dgvRmFg.SelectedRow.Cells(1).Text), "yyyy-MM-dd") & "' " &
                                  "and status <> 'void' and dsrstat = 'Finished' group by transdate")
                If Not CBool(dt.Rows.Count) Then
                    lblLabTotal.Text = "0.00"
                Else
                    For Each dr As DataRow In dt.Rows
                        lblLabTotal.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0.00 ; (#,##0.00)")
                    Next

                End If

                dt.Dispose()

                lblNetVar.Text = Format(CDbl(IIf(txtTotRMpa.Text = "", 0, txtTotRMpa.Text)) - CDbl(IIf(txtTotFGpaWt.Text = "", 0, txtTotFGpaWt.Text)) _
                                 - CDbl(IIf(lblLabTotal.Text = "", 0, lblLabTotal.Text)), "#,##0.00 ; (#,##0.00)")

                sqldata = "select a.mmrrno,ifnull(c.codename,c.mmdesc) as mmdesc,b.user,a.lotno,sum(a.qty) as qty," &
                          "sum(a.wt) as wt from invdettbl a left join invhdrtbl b on a.mmrrno=b.mmrrno " &
                          "left join mmasttbl c on a.codeno=c.codeno where b.mov='801' and " &
                          "b.transdate = '" & Format(CDate(dgvRmFg.SelectedRow.Cells(1).Text), "yyyy-MM-dd") & "' " &
                          "and b.plntno = '" & cboPlntPA.Text.Substring(0, 3) & "' and b.status <> 'void' " &
                          "and b.dsrstat = 'Finished' group by a.mmrrno,a.codeno,a.lotno"
        End Select

        conn.Open()
        Dim command As New MySqlCommand(sqldata, conn)
        adapter.SelectCommand = command
        adapter.Fill(ds)
        adapter.Dispose()
        command.Dispose()
        conn.Close()
        dgvFGdet.DataSource = ds.Tables(0)
        dgvFGdet.DataBind()

    End Sub

    Protected Sub dgvFGdet_RowDataBound(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Protected Sub dgvFGdet_RowCreated(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Private Sub dpDatePA1_TextChanged(sender As Object, e As EventArgs) Handles dpDatePA1.TextChanged, dpDatePA2.TextChanged
        InitTab4()
    End Sub

    Private Sub CheckBox13_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox13.CheckedChanged

        If CheckBox13.Checked = True Then
            CheckBox13.Text = "Finished Only"
        Else
            CheckBox13.Text = "Real Time"
        End If

        ResetInvAnalysis()

        If cboLotNo.Text <> "" And cboMMdesc.Text <> "" And cboMMtype.Text <> "" Then
            GetRecGoods()
            GetIssGoods()
            getBalanceAll()
        End If


    End Sub



End Class
