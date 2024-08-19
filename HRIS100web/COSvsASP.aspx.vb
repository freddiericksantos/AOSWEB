Imports MySql.Data.MySqlClient

Public Class cOSvsASP
    Inherits System.Web.UI.Page
    Dim sql As String
    Dim dt As DataTable
    Dim sqldata As String
    Dim admMatlCost As Double
    Dim admVarAmt As Double
    Dim admMatlWt As Double
    Dim admVarAmtFin As Double

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
            LoadToShow()
            GetPClass()
        End If
    End Sub

    Private Sub LoadToShow()
        Select Case vLoggedBussArea
            Case "8100"
                RadioButton1.Enabled = True
                lblPCtype.Text = "Produce"
                lblFGlabelA.Text = "Total Act Wt.:"
                txtQtyTotFG.Visible = False
            Case "8200"
                RadioButton1.Checked = False
                RadioButton1.Enabled = False
                txtQtyTotFG.Visible = False
            Case "8300"
                RadioButton1.Enabled = True
                lblFGlabelA.Text = ""
                lblPCtype.Text = "Produce"
                txtQtyTotFG.Visible = True
        End Select

    End Sub

    Private Sub GetPClass()
        cboPClass.Items.Clear()
        dt = GetDataTable("select concat(pc,space(1),pclass) from pctrtbl where pc='1'")
        If Not CBool(dt.Rows.Count) Then
            Exit Sub
        Else
            cboPClass.Items.Add("")
            For Each dr As DataRow In dt.Rows
                cboPClass.Items.Add(dr.Item(0).ToString())
            Next

        End If

        Call dt.Dispose()

        cboPClass2.Items.Clear()
        dt = GetDataTable("select concat(pc,space(1),pclass) from pctrtbl where tradetype = 'Trade'")
        If Not CBool(dt.Rows.Count) Then
            Exit Sub
        Else
            cboPClass2.Items.Add("")
            For Each dr As DataRow In dt.Rows
                cboPClass2.Items.Add(dr.Item(0).ToString())
            Next

        End If

        Call dt.Dispose()

    End Sub

    Private Sub ResetTabPage1()
        dgvTab1L.DataSource = Nothing
        dgvTab1L.DataBind()
        dgvTab1R.DataSource = Nothing
        dgvTab1R.DataBind()
        cboMMgrp.Items.Clear()
        cboMMgrp.Text = Nothing
        cboMMdesc.Items.Clear()
        cboMMdesc.Text = Nothing
        txtCodeNo.Text = ""
        txtCodeNo2.Text = ""

    End Sub

    Private Sub GetCCenterPlant()
        dt = GetDataTable("select concat(a.ccno,space(1),b.ccdesc),concat(ifnull(a.ccnoprod,a.ccprod),space(1),c.ccdesc) from pctrtbl a " &
                          "left join cctrnotbl b on a.ccno = b.ccno left join cctrnotbl c on ifnull(a.ccnoprod,a.ccprod) = c.ccno " &
                          "where a.pc = '" & cboPClass.Text.Substring(0, 1) & "'")
        If Not CBool(dt.Rows.Count) Then
            lblCCenter.Text = ""
            lblProdnCCNo.Text = ""
            Exit Sub
        Else
            For Each dr As DataRow In dt.Rows
                lblCCenter.Text = dr.Item(0).ToString()
                lblProdnCCNo.Text = dr.Item(1).ToString()

                'Select Case True
                '    Case RadioButton1.Checked
                '        lblCCenter.Text = dr.Item(0).ToString()
                '        lblProdnCCNo.Text = dr.Item(1).ToString()
                '    Case RadioButton2.Checked
                '        lblCCenter.Text = dr.Item(0).ToString()
                '        lblProdnCCNo.Text = ""
                'End Select

            Next

        End If

        Call dt.Dispose()

    End Sub

    Protected Sub lbClose_Click(sender As Object, e As EventArgs)
        Response.Redirect("FinancialAccounting.aspx")
    End Sub

    Protected Sub lbNew_Click(sender As Object, e As EventArgs)

    End Sub

    Protected Sub dgvRMlist_RowDataBound(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Protected Sub dgvRMlist_RowCreated(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Private Sub cboPClass_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboPClass.SelectedIndexChanged
        If cboPClass.Text = "" Then
            Exit Sub
        End If

        Select Case TabContainer1.ActiveTabIndex
            Case 0
                RadioButton2.Checked = False
                RadioButton1.Checked = True
            Case 1
                RadioButton1.Checked = False
                RadioButton2.Checked = True
        End Select

        ResetTabPage1()

        GetCCenterPlant()
        CosProcQuery()

    End Sub

    Private Sub CosProcQuery() 'Produce
        If cboPClass.Text = "" Then
            AdmMsgBox("Select Product Class")
            Exit Sub

            'ElseIf RadioButton1.Checked = False And RadioButton2.Checked = False Then
            '    MsgBox("Select PC Type")
            '    Exit Sub
        End If

        getOHamt()

        Select Case vLoggedBussArea
            Case "8100"
                getTotalProdnWt()
                getRMcost()
                fillFGaveCost()

            Case "8300"
                getTotalProdnWt()
                GetRMcostMnlProd()
                fillRMlistMnlProd()
                CosFGproc()

        End Select

    End Sub

    Private Sub getRMcost()
        'RM
        sql = "delete from tempdodet where user = '" & lblUser.Text & "'"
        ExecuteNonQuery(sql)

        sql = "insert into tempdodet(dono,codeno,wt,lotno,user,transdate) select a.dono,a.codeno,a.wt,a.lotno,'" & lblUser.Text & "'," &
              "b.transdate from issdettbl a left join isshdrtbl b on a.dono=b.dono left join mmasttbl c on a.codeno=c.codeno " &
              "where b.mov = '201' and b.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
              "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and c.mmtype <> 'packaging' and b.status <> 'void' group by a.idno"
        ExecuteNonQuery(sql)

        'get uc
        sql = "update tempdodet a,(select a.codeno,a.lotno,a.sp from invdettbl a left join invhdrtbl b on a.mmrrno=b.mmrrno " &
              "where b.transdate <= '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and b.status <> 'void' and " &
              "(a.sp <> 0 and a.sp is not null) group by a.codeno,a.lotno) as b set a.uc=b.sp where a.user = '" & lblUser.Text & "' " &
              "and a.codeno=b.codeno and a.lotno=b.lotno"
        ExecuteNonQuery(sql)

        sql = "update tempdodet set amt = wt * ifnull(uc,0) where user = '" & lblUser.Text & "'"
        ExecuteNonQuery(sql)

        FillRMlist()

        'FG
        sql = "delete from tempcostingtbl where user = '" & lblUser.Text & "'"
        ExecuteNonQuery(sql)

        Select Case vLoggedBussArea
            Case "8100"
                sql = "insert into tempcostingtbl(pwno,transdate,codeno,wt,lotno,user,cosno) select wipno,transdate,codeno," &
                      "ifnull(wt,0),source,'" & lblUser.Text & "',liqrepno from wipdettbl where fgtype = 'Matl Input' " &
                      "and transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                      "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and tc = '20' and " &
                      "status <> 'void' and liqrepno is not null group by transid" 'plntno = '" & cboPlnt.Text.Substring(0, 3) & "' and 
                ExecuteNonQuery(sql)

            Case "8300"
                sql = "insert into tempcostingtbl(pwno,transdate,codeno,wt,lotno,user,cosno) select wipno,transdate,codeno," &
                      "ifnull(wt,0),source,'" & lblUser.Text & "',liqrepno from wipdettbl where fgtype = 'Matl Input' " &
                      "and transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                      "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and " &
                      "status <> 'void' group by transid" 'plntno = '" & cboPlnt.Text.Substring(0, 3) & "' and and tc = '20' and liqrepno is not null
                ExecuteNonQuery(sql)
        End Select

        sql = "delete from tempcostingtbl where user = '" & lblUser.Text & "' and cosno is null"
        ExecuteNonQuery(sql)

        'update unit cost
        sql = "update tempcostingtbl a,(select codeno,uc,lotno from tempdodet where user = '" & lblUser.Text & "') as b " &
              "set a.uc = b.uc where a.codeno = b.codeno and a.lotno = b.lotno and a.user = '" & lblUser.Text & "'"
        ExecuteNonQuery(sql)

        sql = "update tempcostingtbl set matlcost = ifnull(wt,0) * ifnull(uc,0) where user = '" & lblUser.Text & "'"
        ExecuteNonQuery(sql)

        sql = "delete from tempcostingtbl2 where user = '" & lblUser.Text & "'"
        ExecuteNonQuery(sql)

        sql = "insert into tempcostingtbl2(codeno,wt,wiptotwt,user) select cosno,round(sum(wt),2),round(sum(matlcost),2),'" & lblUser.Text & "' " &
              "from tempcostingtbl where user = '" & lblUser.Text & "' group by cosno"
        ExecuteNonQuery(sql)

        'mat'l var.
        dt = GetDataTable("select ifnull(sum(wiptotwt),0),ifnull(sum(wt),0) from tempcostingtbl2 where user = '" & lblUser.Text & "'")
        If Not CBool(dt.Rows.Count) Then

        Else
            For Each dr As DataRow In dt.Rows
                admMatlCost = CDbl(dr.Item(0).ToString())
                admMatlWt = CDbl(dr.Item(1).ToString())
            Next

        End If

        dt.Dispose()

        admVarAmt = CDbl(IIf(txtRMtotAmt.Text = "", 0, txtRMtotAmt.Text)) - admMatlCost
        lblVarAmt.Text = Format(CDbl(admVarAmt), "#,##0.00 ; (#,##0.00)")

        sql = "update tempcostingtbl2 set wtvar = " & admVarAmt & " * wt / " & admMatlWt & " where user = '" & lblUser.Text & "'"
        ExecuteNonQuery(sql)

        sql = "update tempcostingtbl2 set matlcost = wiptotwt + wtvar where user = '" & lblUser.Text & "'"
        ExecuteNonQuery(sql)

        sql = "update tempcostingtbl2 set ohcost = " & CDbl(IIf(txtOHamt.Text = "", 0, txtOHamt.Text)) & " * ifnull(wt,0) / " &
              CDbl(IIf(txtProdnWtTot.Text = "", 0, txtProdnWtTot.Text)) & " where user = '" & lblUser.Text & "'"
        ExecuteNonQuery(sql)

        sql = "update tempcostingtbl2 set totcost = ohcost + matlcost where user = '" & lblUser.Text & "'"
        ExecuteNonQuery(sql)

        sql = "update tempcostingtbl2 set avecost = totcost / wt where user = '" & lblUser.Text & "'"
        ExecuteNonQuery(sql)

        'get Lab wt total
        dt = GetDataTable("select ifnull(sum(tactwt),0) - ifnull(sum(twt),0) from invhdrtbl where mov = '801' and " &
                          "status <> 'void' and transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                          "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and tc = '20'")
        If Not CBool(dt.Rows.Count) Then

        Else
            For Each dr As DataRow In dt.Rows
                txtWtLab.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0.00")
            Next

        End If

        dt.Dispose()


    End Sub

    Private Sub FillRMlist()
        dt = GetDataTable("select ifnull(sum(wt),0),ifnull(sum(amt),0) from tempdodet where user = '" & lblUser.Text & "' " &
                          "and codeno is not null group by user")
        If Not CBool(dt.Rows.Count) Then
            dgvRMlist.DataSource = Nothing
            txtTotRMwt.Text = "0.00"
            txtRMtotAmt.Text = "0.00"
            Exit Sub
        Else
            For Each dr As DataRow In dt.Rows
                txtTotRMwt.Text = Format(CDbl(dr.Item(0).ToString()), "##,##0.00") & " "
                txtRMtotAmt.Text = Format(CDbl(dr.Item(1).ToString()), "##,##0.00") & " "
            Next
        End If

        dt.Dispose()

        Dim adapter As New MySqlDataAdapter
        Dim ds As New DataSet()
        sqldata = Nothing

        sqldata = "select concat(a.codeno,space(1),ifnull(b.codename,b.mmdesc)) as mmdesc,ifnull(sum(a.amt),0) / ifnull(sum(a.wt),0) as auc," &
                  "ifnull(sum(a.wt),0) as awt,ifnull(sum(a.amt),0) as amt from tempdodet a left join mmasttbl b on a.codeno=b.codeno " &
                  "where a.user = '" & vLoggedUserID & "' and a.codeno is not null group by a.codeno"

        With dgvRMlist

            .Columns(0).HeaderText = "Material"
            .Columns(1).HeaderText = "UC"
            .Columns(2).HeaderText = "Wt"
            .Columns(3).HeaderText = "Amount"

        End With

        conn.Open()
        Dim command As New MySqlCommand(sqldata, conn)
        adapter.SelectCommand = command
        adapter.Fill(ds)
        adapter.Dispose()
        command.Dispose()
        conn.Close()
        dgvRMlist.DataSource = ds.Tables(0)
        dgvRMlist.DataBind()

        'dt = GetDataTable("select ifnull(sum(wt),0),ifnull(sum(amt),0) from tempdodet where user = '" & vLoggedUserID & "' " & _
        '                  "and codeno is not null group by user")
        'If Not CBool(dt.Rows.Count) Then

        'Else
        '    For Each dr As DataRow In dt.Rows
        '        txtTotRMwt.Text = Format(CDbl(dr.Item(0).ToString()), "##,##0.00") & " "
        '        txtRMtotAmt.Text = Format(CDbl(dr.Item(1).ToString()), "##,##0.00") & " "
        '    Next

        'End If


    End Sub

    Private Sub fillFGaveCost()
        dt = GetDataTable("select ifnull(sum(wt),0),ifnull(sum(matlcost),0),ifnull(sum(ohcost),0),ifnull(sum(totcost),0) " &
                          "from tempcostingtbl2 where user = '" & lblUser.Text & "'")
        If Not CBool(dt.Rows.Count) Then
            dgvFGlist.DataSource = Nothing
            dgvFGlist.DataBind()
            Exit Sub
        Else
            For Each dr As DataRow In dt.Rows
                txtTotWtFG.Text = Format(CDbl(dr.Item(0).ToString()), "##,##0.00") & " "
                txtTotRMamtFG.Text = Format(CDbl(dr.Item(1).ToString()), "##,##0.00") & " "
                txtTotCostAmtFG.Text = Format(CDbl(dr.Item(2).ToString()), "##,##0.00") & " "
                txtTotCostAmt.Text = Format(CDbl(dr.Item(3).ToString()), "##,##0.00") & " "

            Next

        End If

        dt.Dispose()

        Dim adapter As New MySqlDataAdapter
        Dim ds As New DataSet()
        sqldata = Nothing

        sqldata = "select a.codeno,ifnull(b.codename,b.mmdesc) as mmdesc,a.lotno,ifnull(a.qty,0) as qty,ifnull(a.wt,0) as wt,ifnull(a.matlcost,0) as rmcost," &
                  "ifnull(a.ohcost,0) as ohcos,ifnull(a.totcost,0) as totmrcos,ifnull(a.avecost,0) as avecos from tempcostingtbl2 a " &
                  "left join mmasttbl b on a.codeno=b.codeno where a.user = '" & lblUser.Text & "' group by a.codeno"

        With dgvFGlist
            '.Columns(2).Visible = False
            .Columns(0).HeaderText = "Code No."
            .Columns(1).HeaderText = "Description"
            .Columns(2).HeaderText = "Lot No."
            .Columns(3).HeaderText = "Total Qty"
            .Columns(4).HeaderText = "Total Wt"
            .Columns(5).HeaderText = "Matl Cost"
            .Columns(6).HeaderText = "OH Cost"
            .Columns(7).HeaderText = "Total Cost"
            .Columns(8).HeaderText = "Ave. Cost"

        End With

        conn.Open()
        Dim command As New MySqlCommand(sqldata, conn)
        adapter.SelectCommand = command
        adapter.Fill(ds)
        adapter.Dispose()
        command.Dispose()
        conn.Close()
        dgvFGlist.DataSource = ds.Tables(0)
        dgvFGlist.DataBind()



        admVarAmtFin = CDbl(IIf(txtRMtotAmt.Text = "", 0, txtRMtotAmt.Text)) - CDbl(IIf(txtTotRMamtFG.Text = "", 0, txtTotRMamtFG.Text))
        lblVarAmt.Text = Format(CDbl(admVarAmtFin), "#,##0.00 ; (#,##0.00)")

        'If admVarAmtFin < 0 Then
        '    lblVarAmt.ForeColor =
        'Else
        '    lblVarAmt.ForeColor = Color.Black
        'End If

        lblProcStatus.Text = "Processed"

        PopcboMMgrpAvail()

        cboMMgrp.Items.Clear()
        dt = GetDataTable("select  distinct b.mmgrp from tempcostingtbl2 a " &
                          "left join mmasttbl b on a.codeno=b.codeno where a.user = '" & lblUser.Text & "'")
        If Not CBool(dt.Rows.Count) Then
            lblProcStatus.Text = "Not Yet Processed"
            Exit Sub
        Else
            cboMMgrp.Items.Add("")
            For Each dr As DataRow In dt.Rows
                cboMMgrp.Items.Add(dr.Item(0).ToString())
                cboMMgrp.Items.Remove("")
            Next

        End If

        dt.Dispose()

    End Sub

    Private Sub CosFGproc()
        sql = "delete from tempcostingtbl where user = '" & lblUser.Text & "'"
        ExecuteNonQuery(sql)

        sql = "insert into tempcostingtbl(codeno,qty,wt,lotno,user) select a.codeno,ifnull(sum(a.qty),0),ifnull(sum(a.wt),0)," &
              "a.lotno,'" & lblUser.Text & "' from invdettbl a left join invhdrtbl c on c.mmrrno=a.mmrrno " &
              "left join mmasttbl b on a.codeno=b.codeno where c.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
              "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and c.tc = '10' and c.mov = '803' and " &
              "c.status <> 'void' and b.mmtype = 'Finished Goods' and b.pc = '1' and " &
              "b.produce = 'Yes' group by a.codeno,a.lotno" 'c.plntno = '" & cboPlnt.Text.Substring(0, 3) & "' and 
        ExecuteNonQuery(sql)

        'get asp
        sql = "update tempcostingtbl a,(select a.codeno,sum(a.wt) as wt,sum(a.itmamt) as amt from salesdettbl a left join saleshdrtbl b " &
              "on a.invno=b.invno left join mmasttbl c on a.codeno=c.codeno where b.transdate <= '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' " &
              "and b.status <> 'Void' and c.pc = '1' group by a.codeno) as b " &
              "set a.asp = b.amt / b.wt where a.codeno = b.codeno and a.user = '" & lblUser.Text & "'"
        ExecuteNonQuery(sql)

        sql = "update tempcostingtbl set uc = wt * asp where user = '" & lblUser.Text & "'"
        ExecuteNonQuery(sql)

        sql = "update tempcostingtbl a,(select sum(uc) as uc from tempcostingtbl where user = '" & lblUser.Text & "' group by user) as b " &
              "set a.totfgwt = b.uc where a.user = '" & lblUser.Text & "'"
        ExecuteNonQuery(sql)

        'rm cost

        sql = "update tempcostingtbl set totrmamt = " & CDbl(IIf(txtRMtotAmt.Text = "", 0, txtRMtotAmt.Text)) & " " &
              "where user = '" & lblUser.Text & "'"
        ExecuteNonQuery(sql)

        sql = "update tempcostingtbl set matlcost = totrmamt * uc / totfgwt where user = '" & lblUser.Text & "'"
        ExecuteNonQuery(sql)

        'oh 
        sql = "update tempcostingtbl set totohamt = " & CDbl(IIf(txtOHamt.Text = "", 0, txtOHamt.Text)) & " " &
              "where user = '" & lblUser.Text & "'"
        ExecuteNonQuery(sql)

        sql = "update tempcostingtbl set ohcost = totohamt * uc / totfgwt where user = '" & lblUser.Text & "'"
        ExecuteNonQuery(sql)

        sql = "update tempcostingtbl set totcost = ifnull( matlcost,0) + ifnull(dlcost,0) + ifnull(ohcost,0) " &
              "where user = '" & lblUser.Text & "'"
        ExecuteNonQuery(sql)

        sql = "update tempcostingtbl set avecost = totcost / wt where user = '" & lblUser.Text & "'"
        ExecuteNonQuery(sql)

        'update invdet

        sql = "update invdettbl a left join invhdrtbl c on a.mmrrno=c.mmrrno left join mmasttbl d on a.codeno=d.codeno," &
              "(select codeno,avecost from tempcostingtbl where user = '" & lblUser.Text & "') as b set a.uc = b.avecost " &
              "where a.codeno=b.codeno and c.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
              "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and c.tc = '10' and c.mov = '803' and " &
              "c.status <> 'void' and d.mmtype = 'Finished Goods' and " &
              "d.produce = 'Yes' and d.pc = '1'"
        ExecuteNonQuery(sql)

        FilldgvFGsum()

        lblProcStatus.Text = "Processed"

        PopcboMMgrpAvail()

    End Sub

    Private Sub PopcboMMgrpAvail()
        cboMMgrp.Items.Clear()

        Select Case vLoggedBussArea
            Case "8100"
                dt = GetDataTable("select b.mmgrp from tempcostingtbl2 a left join mmasttbl b on a.codeno=b.codeno " &
                                  "where a.user = '" & lblGrpUser.Text & "' group by b.mmgrp order by b.mmgrp")
            Case "8300"
                dt = GetDataTable("select b.mmgrp from tempcostingtbl a left join mmasttbl b on a.codeno=b.codeno " &
                                  "where a.user = '" & lblGrpUser.Text & "' group by b.mmgrp order by b.mmgrp")
        End Select

        If Not CBool(dt.Rows.Count) Then
            Exit Sub
        Else
            cboMMgrp.Items.Add("")
            For Each dr As DataRow In dt.Rows
                cboMMgrp.Items.Add(dr.Item(0).ToString())

            Next

        End If

        Call dt.Dispose()

    End Sub


    Private Sub FilldgvFGsum()
        Dim adapter As New MySqlDataAdapter
        Dim ds As New DataSet()
        sqldata = Nothing

        Select Case cboPClass.Text.Substring(0, 1)
            Case "1"
                dt = GetDataTable("select sum(ifnull(qty,0)),sum(ifnull(wt,0)),sum(ifnull(matlcost,0)),sum(ifnull(ohcost,0))," &
                                  "sum(ifnull(totcost,0)) from tempcostingtbl where user = '" & lblUser.Text & "' group by user")
                If Not CBool(dt.Rows.Count) Then
                    dgvFGlist.DataSource = Nothing
                    dgvFGlist.DataBind()
                    Exit Sub

                Else
                    For Each dr As DataRow In dt.Rows
                        txtQtyTotFG.Text = Format(CDbl(dr.Item(0).ToString()), "##,##0.00") & " "
                        txtTotWtFG.Text = Format(CDbl(dr.Item(1).ToString()), "##,##0.00") & " "
                        txtTotRMamtFG.Text = Format(CDbl(dr.Item(2).ToString()), "##,##0.00") & " "
                        txtTotCostAmtFG.Text = Format(CDbl(dr.Item(3).ToString()), "##,##0.00") & " "
                        txtTotCostAmt.Text = Format(CDbl(dr.Item(4).ToString()), "##,##0.00") & " "

                    Next

                End If

                dt.Dispose()

                sqldata = "select a.codeno,ifnull(b.codename,b.mmdesc) as mmdesc,a.lotno,a.qty,a.wt,ifnull(a.matlcost,0) as rmcost," &
                          "ifnull(a.ohcost,0) as ohcos,ifnull(a.totcost,0) as totmrcos,ifnull(a.avecost,0) as avecos from " &
                          "tempcostingtbl a left join mmasttbl b on a.codeno=b.codeno where a.user = '" & lblUser.Text & "'"

        End Select

        With dgvFGlist
            .Columns(0).HeaderText = "Code No."
            .Columns(1).HeaderText = "Finished Goods"
            .Columns(2).HeaderText = "Lot No."
            .Columns(3).HeaderText = "Qty"
            .Columns(4).HeaderText = "Wt"
            .Columns(5).HeaderText = "RM Cost"
            .Columns(6).HeaderText = "OHead"
            .Columns(7).HeaderText = "Total MC"
            .Columns(8).HeaderText = "Ave Cost"

        End With

        conn.Open()
        Dim command As New MySqlCommand(sqldata, conn)
        adapter.SelectCommand = command
        adapter.Fill(ds)
        adapter.Dispose()
        command.Dispose()
        conn.Close()
        dgvFGlist.DataSource = ds.Tables(0)
        dgvFGlist.DataBind()



    End Sub
    Private Sub getOHamt()
        dt = GetDataTable("select ifnull(sum(dramt),0)-ifnull(sum(cramt),0) from gltranstbl " &
                          "where transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                          "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and acctno between '2560000' and '2569999' " &
                          "and ccno = '" & lblProdnCCNo.Text.Substring(0, 5) & "' group by ccno")
        If Not CBool(dt.Rows.Count) Then
            txtOHamt.Text = "0.00"
            Exit Sub
        Else
            For Each dr As DataRow In dt.Rows
                txtOHamt.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0.00")

            Next
        End If

        Call dt.Dispose()

    End Sub

    Private Sub getTotalProdnWt()
        Select Case vLoggedBussArea
            Case "8100"
                dt = GetDataTable("select sum(ifnull(twt,0)) as twt,sum(ifnull(tactwt,0)) as acttwt from invhdrtbl where mov = '801' " &
                                  "and transdate between  '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                                  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and status <> 'void' and " &
                                  "tc ='20' group by mov") 'plntno = '" & cboPlnt.Text.Substring(0, 3) & "' and 
            Case "8300" 'revise this for manual costing
                dt = GetDataTable("select sum(ifnull(a.qty,0)) as twt,sum(ifnull(a.wt,0)) as acttwt from invdettbl a left join invhdrtbl b on a.mmrrno=b.mmrrno " &
                                  "where b.mov between '801' and '803' and b.transdate between  '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                                  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and b.status <> 'void'")
        End Select

        If Not CBool(dt.Rows.Count) Then
            txtPronWt.Text = "0.00"
            txtProdnWtTot.Text = "0.00"
            Exit Sub
        Else
            For Each dr As DataRow In dt.Rows

                Select Case vLoggedBussArea
                    Case "8100"
                        txtPronWt.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0.00")
                        txtProdnWtTot.Text = Format(CDbl(dr.Item(1).ToString()), "#,##0.00")
                    Case "8300"
                        txtPronWt.Text = Format(CDbl(dr.Item(1).ToString()), "#,##0.00")

                End Select

            Next
        End If

        Call dt.Dispose()

        Dim aveOHamt As Double = CDbl(IIf(txtOHamt.Text = "", 0, txtOHamt.Text)) / CDbl(IIf(txtPronWt.Text = "", 0, txtPronWt.Text))
        txtOHperWt.Text = Format(aveOHamt, "#,##0.00")

    End Sub

    Private Sub GetRMcostMnlProd()
        Select Case cboPClass.Text.Substring(0, 1)
            Case "1"
                sql = "update wipdettbl set sp = null where sp = 0 and transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                      "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and fgtype = 'Matl Input'"
                ExecuteNonQuery(sql)

                sql = "update wipdettbl a,(select a.codeno,sum(a.wt) as wt,sum(a.itmamt) as itmamt,sum(a.itmamt) / sum(a.wt) as auc from invdettbl a " &
                      "left join invhdrtbl b on a.mmrrno=b.mmrrno where b.transdate <= '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and " &
                      "b.status <> 'Void' and b.tc = '10' and ifnull(a.sp,0) <> 0 group by a.codeno) as b set a.sp=b.auc " &
                      "where a.codeno=b.codeno and a.sp is null and a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                      "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and fgtype ='Matl Input'" 'b.plntno = '" & cboPlnt.Text.Substring(0, 3) & "' and
                ExecuteNonQuery(sql)

                sql = "update wipdettbl set stdcosamt = ifnull(wt,0) * ifnull(sp,0) where fgtype ='Matl Input' and tc = '20' and " &
                      "transdate between  '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                      "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "'" 'plntno = '" & cboPlnt.Text.Substring(0, 3) & "' and 
                ExecuteNonQuery(sql)

                sql = "update wipdettbl set source = '' where fgtype ='Matl Input' and tc = '20' " &
                      "and transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                      "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and source is null"
                ExecuteNonQuery(sql)

            Case "2"



        End Select

    End Sub

    Private Sub fillRMlistMnlProd()
        Dim adapter As New MySqlDataAdapter
        Dim ds As New DataSet()
        sqldata = Nothing

        Select Case cboPClass.Text.Substring(0, 1)
            Case "1"
                dt = GetDataTable("select ifnull(sum(a.wt),0),ifnull(sum(a.stdcosamt),0) from wipdettbl a left join isshdrtbl c " &
                                  "on a.dono=c.dono left join mmasttbl b on a.codeno = b.codeno " &
                                  "where a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                                  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and a.fgtype = 'Matl Input' and " &
                                  "c.status <> 'Void' and c.mov = '201' and b.pc='1' group by a.fgtype")
                If Not CBool(dt.Rows.Count) Then
                    dgvRMlist.DataSource = Nothing

                    Exit Sub
                Else
                    For Each dr As DataRow In dt.Rows
                        txtTotRMwt.Text = Format(CDbl(dr.Item(0).ToString()), "##,##0.00")
                        txtRMtotAmt.Text = Format(CDbl(dr.Item(1).ToString()), "##,##0.00")
                    Next
                End If

                dt.Dispose()

                sqldata = "select concat(a.codeno,space(1),ifnull(b.codename,b.mmdesc)) as mmdesc,ifnull((sum(a.stdcosamt)/sum(a.wt)),0) as auc," &
                          "ifnull(sum(a.wt),0) as awt,ifnull(sum(a.stdcosamt),0) as amt from wipdettbl a left join isshdrtbl c on a.dono=c.dono " &
                          "left join mmasttbl b on a.codeno=b.codeno where a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                          "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and a.fgtype = 'Matl Input' and " &
                          "c.status <> 'Void' and c.mov = '201' and b.pc = '1' group by a.codeno"
            Case "2"

        End Select

        With dgvRMlist
            .Columns(0).HeaderText = "Materials"
            .Columns(1).HeaderText = "Ave. UC"
            .Columns(2).HeaderText = "Wt"
            .Columns(3).HeaderText = "Amount"

        End With

        conn.Open()
        Dim command As New MySqlCommand(sqldata, conn)
        adapter.SelectCommand = command
        adapter.Fill(ds)
        adapter.Dispose()
        command.Dispose()
        conn.Close()
        dgvRMlist.DataSource = ds.Tables(0)
        dgvRMlist.DataBind()

    End Sub

    Private Sub dpTransDate_TextChanged(sender As Object, e As EventArgs) Handles dpTransDate.TextChanged, dpTransDate2.TextChanged
        dpTransDate.Text = Format(FirstDayOfMonth(CDate(dpTransDate.Text)), "yyyy-MM-dd")

        If CheckBox2.Checked = True Then

        Else
            dpTransDate2.Text = Format(LastDayOfMonth(CDate(dpTransDate.Text)), "yyyy-MM-dd")
        End If

        'dpSalesDate1.Value = dpTransDate.text
        'dpSalesDate2.Value = dpTransDate2.text

    End Sub

    Protected Sub dgvFGlist_RowDataBound(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Protected Sub dgvFGlist_RowCreated(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Protected Sub dgvRMlist2_RowDataBound(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Protected Sub dgvRMlist2_RowCreated(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Private Sub cboPClass2_TextChanged(sender As Object, e As EventArgs) Handles cboPClass2.TextChanged


    End Sub

    Private Sub GetCCenterPlant2()
        dt = GetDataTable("select concat(a.ccno,space(1),b.ccdesc),concat(ifnull(a.ccnoprod,a.ccprod),space(1),c.ccdesc) from pctrtbl a " &
                          "left join cctrnotbl b on a.ccno = b.ccno left join cctrnotbl c on ifnull(a.ccnoprod,a.ccprod) = c.ccno " &
                          "where a.pc = '" & cboPClass2.Text.Substring(0, 1) & "'")
        If Not CBool(dt.Rows.Count) Then
            lblCCenter2.Text = ""
            lblProdnCCNo2.Text = ""
            Exit Sub
        Else
            For Each dr As DataRow In dt.Rows
                lblCCenter2.Text = dr.Item(0).ToString()
                lblProdnCCNo2.Text = dr.Item(1).ToString()

            Next

        End If

        Call dt.Dispose()

    End Sub

    Private Sub CosProcQuery2()
        If cboPClass2.Text = "" Then
            Exit Sub

        End If

        GetTradingCost()
        COSpc2Proc()
        lblProcStatus2.Text = "Processed"

    End Sub

    Private Sub COSpc2Proc()
        'tempinvdettbl 1)get beg bal, 2. get purchases
        sql = "delete from tempinvdettbl where user = '" & lblUser.Text & "'"
        ExecuteNonQuery(sql)

        Select Case vLoggedBussArea
            Case "8200"
                sql = "insert into tempinvdettbl(codeno,qty,wt,itmamt,billref,user) select b.basecodeno,sum(a.qty),sum(a.wt),sum(a.detgrossamt) + ifnull(sum(a.foh),0)," &
                      "ifnull(b.cosbillref,b.billref),'" & lblUser.Text & "' FROM invdettbl a left join invhdrtbl c on a.mmrrno=c.mmrrno left join mmasttbl b on a.codeno=b.codeno " &
                      "where c.transdate <= '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and c.status <> 'void' and b.pc ='" & cboPClass2.Text.Substring(0, 1) & "' " &
                      "and b.tradeonly = 'Y' and (c.mov between '101' and '103' or c.mov = '511') group by b.basecodeno"
                ExecuteNonQuery(sql)

                sql = "update tempinvdettbl set sp = round(itmamt / wt,2) where billref = 'wt' and user = '" & lblUser.Text & "'"
                ExecuteNonQuery(sql)

                sql = "update tempinvdettbl set sp = round(itmamt / qty,2) where billref = 'qty' and user = '" & lblUser.Text & "'"
                ExecuteNonQuery(sql)

            Case "8300"
                sql = "insert into tempinvdettbl(codeno,qty,wt,itmamt,billref,user) select b.bcodenogrp,sum(a.qty),sum(a.wt),sum(a.detgrossamt) + ifnull(sum(a.foh),0)," &
                      "ifnull(b.cosbillref,b.billref),'" & lblUser.Text & "' FROM invdettbl a left join invhdrtbl c on a.mmrrno=c.mmrrno left join mmasttbl b on a.codeno=b.codeno " &
                      "where c.transdate <= '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and c.status <> 'void' and b.pc ='" & cboPClass2.Text.Substring(0, 1) & "' " &
                      "and b.tradeonly = 'Y' and (c.mov between '101' and '103' or c.mov = '511') group by b.bcodenogrp" 'change to bcodenogrp
                ExecuteNonQuery(sql)

                sql = "update tempinvdettbl set sp = round(itmamt / wt,2) where billref = 'wt' and user = '" & lblUser.Text & "'"
                ExecuteNonQuery(sql)

                sql = "update tempinvdettbl set sp = round(itmamt / qty,2) where billref = 'qty' and user = '" & lblUser.Text & "'"
                ExecuteNonQuery(sql)

            Case Else
                sql = "insert into tempinvdettbl(codeno,qty,wt,itmamt,billref,user) select b.basecodeno,sum(a.qty),sum(a.wt),sum(a.detgrossamt) + ifnull(sum(a.foh),0)," &
                      "ifnull(b.cosbillref,b.billref),'" & lblUser.Text & "' FROM invdettbl a left join invhdrtbl c on a.mmrrno=c.mmrrno left join mmasttbl b on a.codeno=b.codeno " &
                      "where c.transdate <= '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and c.status <> 'void' and b.pc ='" & cboPClass2.Text.Substring(0, 1) & "' " &
                      "and b.tradeonly = 'Y' and (c.mov between '101' and '103' or c.mov = '511') group by b.basecodeno"
                ExecuteNonQuery(sql)


        End Select

        sql = "update tempinvdettbl set sp = round(itmamt / wt,2) where user = '" & lblUser.Text & "' and billref = 'wt'"
        ExecuteNonQuery(sql)

        sql = "update tempinvdettbl set sp = round(itmamt / qty,2) where user = '" & lblUser.Text & "' and billref = 'qty'"
        ExecuteNonQuery(sql)

        'salesdettbl
        sql = "delete from tempfgcostingtbl where user = '" & lblUser.Text & "'"
        ExecuteNonQuery(sql)

        Select Case cboPClass2.Text.Substring(0, 1)
            Case "1"
                Select Case vLoggedBussArea
                    Case "8100", "8200", "8300"
                        sql = "insert into tempfgcostingtbl(codeno,qty,wt,user,um,lotno,matlcost) select a.codeno,ifnull(sum(a.qty),0),ifnull(sum(a.wt),0)," &
                              "'" & lblUser.Text & "',c.basecodeno,ifnull(c.cosbillref,c.billref),ifnull(sum(a.itmamt),0) from salesdettbl a " &
                              "left join saleshdrtbl b on a.invno=b.invno left join mmasttbl c on a.codeno=c.codeno where " &
                              "b.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' " &
                              "and b.status <> 'void' and c.pc = '" & cboPClass2.Text.Substring(0, 1) & "' and c.tradeonly = 'Y' group by a.codeno"
                        ExecuteNonQuery(sql)

                        sql = "update tempfgcostingtbl set asp = round(matlcost / qty,2) where user = '" & lblUser.Text & "' and lotno = 'qty'"
                        ExecuteNonQuery(sql)

                        sql = "update tempfgcostingtbl set asp = round(matlcost / wt,2) where user = '" & lblUser.Text & "' and lotno = 'wt'"
                        ExecuteNonQuery(sql)

                        'update cost tempinvdettbl update uc / totcost
                        sql = "update tempfgcostingtbl a,(select codeno,sp from tempinvdettbl where user = '" & lblUser.Text & "') as b " &
                              "set a.uc = b.sp where a.um = b.codeno and a.user = '" & lblUser.Text & "'"
                        ExecuteNonQuery(sql)

                        sql = "update tempfgcostingtbl set uc = null where uc = 0 and user = '" & lblUser.Text & "'"
                        ExecuteNonQuery(sql)

                        sql = "update tempfgcostingtbl a,(select codeno,ifnull(sp,0) * .8 as asp from salesdettbl a left join saleshdrtbl b on a.invno=b.invno " &
                              "where b.status <> 'void' and b.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                              "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "') as c set a.uc = c.asp,a.pwno = 'Estimated Only' where " &
                              "a.codeno=c.codeno and a.user = '" & lblUser.Text & "' and a.uc is null"
                        ExecuteNonQuery(sql)

                        sql = "update tempfgcostingtbl set totcost = ifnull(uc,0) * wt where user = '" & lblUser.Text & "' and lotno = 'wt'"
                        ExecuteNonQuery(sql)

                        sql = "update tempfgcostingtbl set totcost =  ifnull(uc,0) * qty where user = '" & lblUser.Text & "' and lotno = 'qty'"
                        ExecuteNonQuery(sql)

                        sql = "update tempfgcostingtbl set avecost = (ifnull(matlcost,0) - ifnull(totcost,0)) / ifnull(matlcost,0)  where " &
                              "user = '" & lblUser.Text & "'"
                        ExecuteNonQuery(sql)

                    Case Else
                        sql = "insert into tempfgcostingtbl(codeno,qty,wt,user,um,lotno) select a.codeno,sum(a.qty),sum(a.wt),'" & lblUser.Text & "',c.basecodeno," &
                              "ifnull(c.cosbillref,c.billref) from salesdettbl a left join saleshdrtbl b on a.invno=b.invno left join mmasttbl c on a.codeno=c.codeno where " &
                              "b.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' " &
                              "and b.status <> 'void' and c.pc = '" & cboPClass2.Text.Substring(0, 1) & "' and c.mmtype = 'Raw Materials' group by a.codeno"
                        ExecuteNonQuery(sql)

                        sql = "update tempfgcostingtbl a,(select a.codeno,a.sp,b.basecodeno from tempinvdettbl a left join mmasttbl b on a.codeno=b.codeno " &
                              "where a.user = '" & lblUser.Text & "') as b set a.uc = b.sp where a.codeno = b.codeno and a.user = '" & lblUser.Text & "'"
                        ExecuteNonQuery(sql)

                        sql = "update tempfgcostingtbl a,(select a.codeno,a.sp from salesdettbl a left join saleshdrtbl c on a.invno = c.invno " &
                              "left join mmasttbl b on a.codeno=b.codeno where c.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                              "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and b.mmtype = 'Raw Materials' and c.status <> 'void') as b " &
                              "set a.uc = ifnull(b.sp,0) * .8 where a.codeno = b.codeno and a.uc is null"
                        ExecuteNonQuery(sql)

                End Select

            Case "2"
                Select Case vLoggedBussArea
                    Case "8300"
                        sql = "insert into tempfgcostingtbl(codeno,qty,wt,user,um,lotno,matlcost) select a.codeno,ifnull(sum(a.qty),0),ifnull(sum(a.wt),0)," &
                              "'" & lblUser.Text & "',c.bcodenogrp,c.billref,ifnull(sum(a.itmamt),0) from salesdettbl a " &
                              "left join saleshdrtbl b on a.invno=b.invno left join mmasttbl c on a.codeno=c.codeno where " &
                              "b.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' " &
                              "and b.status <> 'void' and c.pc = '" & cboPClass2.Text.Substring(0, 1) & "' group by a.codeno"
                        ExecuteNonQuery(sql)

                        sql = "update tempfgcostingtbl set asp = matlcost / qty where user = '" & lblUser.Text & "' and lotno = 'qty'"
                        ExecuteNonQuery(sql)

                        sql = "update tempfgcostingtbl set asp = matlcost / wt where user = '" & lblUser.Text & "' and lotno = 'wt'"
                        ExecuteNonQuery(sql)

                        'update cost tempinvdettbl update uc / totcost
                        sql = "update tempfgcostingtbl a,(select a.codeno,a.sp from tempinvdettbl a " &
                              "left join mmasttbl b on a.codeno=b.codeno where a.user = '" & lblUser.Text & "') as b " &
                              "set a.uc = b.sp where a.um = b.codeno and a.user = '" & lblUser.Text & "'"
                        ExecuteNonQuery(sql)

                        sql = "update tempfgcostingtbl set uc = null where uc = 0 and user = '" & lblUser.Text & "'"
                        ExecuteNonQuery(sql)

                        sql = "update tempfgcostingtbl a,(select codeno,ifnull(sp,0) * .8 as asp from salesdettbl a left join saleshdrtbl b on a.invno=b.invno " &
                              "where b.status <> 'void' and b.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                              "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "') as c set a.uc = c.asp,a.pwno = 'Estimated Only' where " &
                              "a.codeno=c.codeno and a.user = '" & lblUser.Text & "' and a.uc is null"
                        ExecuteNonQuery(sql)

                        sql = "update tempfgcostingtbl a left join mmasttbl b on a.codeno=b.codeno set a.totcost = ifnull(a.uc,0) * a.wt where " &
                              "a.user = '" & lblUser.Text & "' and ifnull(b.cosbillref,b.billref) = 'wt'"
                        ExecuteNonQuery(sql)

                        sql = "update tempfgcostingtbl a left join mmasttbl b on a.codeno=b.codeno set a.totcost = ifnull(a.uc,0) * a.qty where " &
                              "a.user = '" & lblUser.Text & "' and ifnull(b.cosbillref,b.billref) = 'qty'"
                        ExecuteNonQuery(sql)

                        sql = "update tempfgcostingtbl set wtvar = round(ifnull(totcost,0) / wt,2) where user = '" & lblUser.Text & "' and lotno = 'wt'"
                        ExecuteNonQuery(sql)

                        sql = "update tempfgcostingtbl set wtvar =  round(ifnull(totcost,0) / qty,2) where user = '" & lblUser.Text & "' and lotno = 'qty'"
                        ExecuteNonQuery(sql)

                        sql = "update tempfgcostingtbl set avecost = (ifnull(matlcost,0) - ifnull(totcost,0)) / ifnull(matlcost,0)  where " &
                             "user = '" & lblUser.Text & "'"
                        ExecuteNonQuery(sql)

                    Case Else
                        sql = "insert into tempfgcostingtbl(codeno,qty,wt,user,um,lotno) select a.codeno,sum(a.qty),sum(a.wt),'" & lblUser.Text & "',c.basecodeno," &
                              "ifnull(c.cosbillref,c.billref) from salesdettbl a left join saleshdrtbl b on a.invno=b.invno left join mmasttbl c on a.codeno=c.codeno where " &
                              "b.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' " &
                              "and b.status <> 'void' and c.pc = '" & cboPClass2.Text.Substring(0, 1) & "' group by a.codeno"
                        ExecuteNonQuery(sql)

                        sql = "update tempfgcostingtbl a,(select a.codeno,a.sp,b.basecodeno from tempinvdettbl a left join mmasttbl b on a.codeno=b.codeno " &
                              "where a.user = '" & lblUser.Text & "') as b set a.uc = b.sp where a.um = b.basecodeno and a.user = '" & lblUser.Text & "'"
                        ExecuteNonQuery(sql)

                End Select


        End Select

        Select Case vLoggedBussArea
            Case "8200"
                'update temp cos
                sql = "update salesdettbl a left join saleshdrtbl b on a.invno=b.invno left join mmasttbl c on a.codeno=c.codeno," &
                      "(select codeno,uc from tempfgcostingtbl where user = '" & lblUser.Text & "') as e set a.tempcos=e.uc where " &
                      "c.basecodeno=e.codeno and b.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                      "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "'"
                ExecuteNonQuery(sql)

            Case "8300"
                Select Case cboPClass2.Text.Substring(0, 1)
                    Case "1"
                        sql = "update salesdettbl a left join saleshdrtbl b on a.invno=b.invno left join mmasttbl c on a.codeno=c.codeno," &
                              "(select codeno,uc from tempfgcostingtbl where user = '" & lblUser.Text & "') as e set a.tempcos=e.uc where " &
                              "c.basecodeno=e.codeno and b.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                              "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "'"
                        ExecuteNonQuery(sql)

                    Case "2"
                        sql = "update salesdettbl a left join saleshdrtbl b on a.invno=b.invno left join mmasttbl c on a.codeno=c.codeno," &
                              "(select a.codeno,a.uc,ifnull(b.qtpk,'1') as qtpk from tempfgcostingtbl a left join mmasttbl b on a.codeno=b.codeno " &
                              "where a.user = '" & lblUser.Text & "') as e set a.tempcos=e.uc * e.qtpk where " &
                              "c.basecodeno=e.codeno and b.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                              "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "'"
                        ExecuteNonQuery(sql)

                End Select


            Case Else
                sql = "update tempfgcostingtbl set matlcost = wt * uc where user = '" & lblUser.Text & "' and lotno = 'wt'"
                ExecuteNonQuery(sql)

                sql = "update tempfgcostingtbl set matlcost = qty * uc where user = '" & lblUser.Text & "' and lotno = 'qty'"
                ExecuteNonQuery(sql)

                sql = "update salesdettbl a left join saleshdrtbl c on a.invno = c.invno,(select a.codeno,a.uc from tempfgcostingtbl a " &
                      "left join mmasttbl c on a.codeno=c.codeno where a.user = '" & lblUser.Text & "' and c. pc = '" & cboPClass2.Text.Substring(0, 1) & "') as e " &
                      "set a.ucos = ifnull(e.uc,0) where a.codeno=e.codeno " &
                      "and c.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                      "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "'"
                ExecuteNonQuery(sql)

                sql = "update salesdettbl a left join saleshdrtbl c on a.invno = c.invno set a.ucos = a.sp * .8 " &
                      "where c.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                      "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and (a.ucos is null or a.ucos = 0)"
                ExecuteNonQuery(sql)

        End Select


        FilldgvCOSvsASP_Tr()

        getMMgrpDbox()

    End Sub

    Private Sub FilldgvCOSvsASP_Tr()
        Dim adapter As New MySqlDataAdapter
        Dim ds As New DataSet()
        sqldata = Nothing

        dt = GetDataTable("select * from tempfgcostingtbl where user = '" & lblUser.Text & "'")
        If Not CBool(dt.Rows.Count) Then
            dgvFGlist2.DataSource = Nothing
            dgvFGlist2.DataBind()
            Exit Sub
        End If

        sqldata = "select a.codeno,ifnull(b.codename,b.mmdesc) as mmdesc,ifnull(a.pwno,'Actual') as wsno,a.qty,a.wt," &
                  "ifnull(a.matlcost,0) as rmcost,a.asp,ifnull(a.totcost,0) as tcost,ifnull(a.uc,0) as uc,ifnull(a.wtvar,0) as wtvar," &
                  "ifnull(avecost,0) * 100 as perc from tempfgcostingtbl a left join mmasttbl b on a.codeno=b.codeno " &
                  "where a.user = '" & lblUser.Text & "' and b.trade = 'Y' order by ifnull(b.codename,b.mmdesc)"

        With dgvFGlist2
            Select Case vLoggedBussArea
                Case "8200", "8300"
                    .Columns(0).HeaderText = "Code No."
                    .Columns(1).HeaderText = "Finished Goods"
                    .Columns(2).HeaderText = "Remarks"
                    .Columns(3).HeaderText = "Qty"
                    .Columns(4).HeaderText = "Wt"
                    .Columns(5).HeaderText = "Sales Amt"
                    .Columns(6).HeaderText = "ASP"
                    .Columns(7).HeaderText = "Cost Amt"
                    .Columns(8).HeaderText = "UC"
                    .Columns(9).HeaderText = "Per Pack"
                    .Columns(10).HeaderText = "%"

                Case Else
                    .Columns(0).HeaderText = "Code No."
                    .Columns(1).HeaderText = "Finished Goods"
                    .Columns(2).HeaderText = "AUC"
                    .Columns(3).HeaderText = "Qty"
                    .Columns(4).HeaderText = "Wt"
                    .Columns(5).HeaderText = "RM Cost"

            End Select


        End With

        conn.Open()
        Dim command As New MySqlCommand(sqldata, conn)
        adapter.SelectCommand = command
        adapter.Fill(ds)
        adapter.Dispose()
        command.Dispose()
        conn.Close()
        dgvFGlist2.DataSource = ds.Tables(0)
        dgvFGlist2.DataBind()


        Dim TCostWt As Double = 0
        Dim TCostRM As Double = 0
        Dim TCostOH As Double = 0
        Dim TCostAmt As Double = 0

        For i As Integer = 0 To dgvFGlist2.Rows.Count - 1
            TCostWt += CDbl(dgvFGlist2.Rows(i).Cells(3).Text)
            TCostRM += CDbl(dgvFGlist2.Rows(i).Cells(4).Text)
            TCostAmt += CDbl(dgvFGlist2.Rows(i).Cells(5).Text)
            TCostOH += CDbl(dgvFGlist2.Rows(i).Cells(7).Text)
        Next

        txtTotWtFG2.Text = Format(CDbl(TCostWt), "##,##0")
        txtTotRMamtFG2.Text = Format(CDbl(TCostRM), "##,##0.00")
        txtTotCostAmtFG2.Text = Format(CDbl(TCostAmt), "##,##0.00")
        txtTotCostAmt2.Text = Format(CDbl(TCostOH), "##,##0.00")


    End Sub

    Private Sub GetTradingCost()
        sql = "delete from tempdodet where user = '" & lblUser.Text & "'"
        ExecuteNonQuery(sql)

        Select Case cboPClass2.Text.Substring(0, 1)
            Case "1"
                txtOHamt.Text = "0.00"
                sql = "insert into tempdodet(dono,codeno,qty,wt,amt,user,lotno) select a.mmrrno,a.codeno,a.qty,a.wt,a.itmamt,'" & lblUser.Text & "'," &
                      "a.lotno from invdettbl a left join invhdrtbl b on a.mmrrno=b.mmrrno left join mmasttbl c on a.codeno=c.codeno where " &
                      "b.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                      "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and b.status <> 'Void' and c.mmtype = 'Finished Goods' and " &
                      "c.pc = '" & cboPClass2.Text.Substring(0, 1) & "' and b.mov between '101' and '103' group by a.transid"
                ExecuteNonQuery(sql)

            Case "2"
                sql = "insert into tempdodet(dono,codeno,qty,wt,amt,user,lotno) select a.mmrrno,a.codeno,a.qty,a.wt,a.itmamt,'" & lblUser.Text & "'," &
                      "a.lotno from invdettbl a left join invhdrtbl b on a.mmrrno=b.mmrrno left join mmasttbl c on a.codeno=c.codeno where " &
                      "b.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                      "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and b.status <> 'Void' and ifnull(c.tradeonly,'N') = 'Y' and " &
                      "c.pc = '" & cboPClass2.Text.Substring(0, 1) & "' and b.mov between '101' and '103' group by a.transid"
                ExecuteNonQuery(sql)

        End Select

        sql = "update tempdodet a,(select sum(wt) as wt from tempdodet where user = '" & lblUser.Text & "' group by user) as b " &
              "set a.stdwt = b.wt where a.user = '" & lblUser.Text & "'"
        ExecuteNonQuery(sql)

        sql = "update tempdodet set stdvar = stdqty * wt / stdwt where user = '" & lblUser.Text & "'"
        ExecuteNonQuery(sql)

        'insert oh to invdettbl
        sql = "update invdettbl a,(select dono,codeno,stdvar from tempdodet where user = '" & lblUser.Text & "') as b " &
              "set a.foh = b.stdvar where a.mmrrno=b.dono and a.codeno=b.codeno"
        ExecuteNonQuery(sql)

        sql = "update tempdodet set totcost = ifnull(amt,0) + ifnull(stdvar,0) where user = '" & lblUser.Text & "'"
        ExecuteNonQuery(sql)

        sql = "update tempdodet a left join mmasttbl b on a.codeno=b.codeno and a.user = '" & lblUser.Text & "' " &
              "set a.pono = ifnull(b.cosbillref,b.billref) where a.user = '" & lblUser.Text & "'"
        ExecuteNonQuery(sql)

        sql = "update tempdodet set uc = ifnull(totcost,0) / ifnull(wt,0) where pono = 'wt' and user = '" & lblUser.Text & "'"
        ExecuteNonQuery(sql)

        sql = "update tempdodet set uc = ifnull(totcost,0) / ifnull(qty,0) where pono = 'qty' and user = '" & lblUser.Text & "'"
        ExecuteNonQuery(sql)

        'create new table
        dt = GetDataTable("select * from information_schema.tables where table_schema =  '" & vDbase & "' and " &
                          "table_name = 'tempdodet2'")
        If Not CBool(dt.Rows.Count) Then
            sql = "CREATE TABLE tempdodet2 LIKE tempdodet"
            ExecuteNonQuery(sql)
        Else
            sql = "delete from tempdodet2 where user = '" & lblUser.Text & "'"
            ExecuteNonQuery(sql)
        End If

        dt.Dispose()

        sql = "insert into tempdodet2(codeno,qty,wt,amt,uc,pono,user) select codeno,ifnull(sum(qty),0),ifnull(sum(wt),0)," &
              "ifnull(sum(totcost),0),ifnull(sum(totcost),0) / ifnull(sum(wt),0),pono,'" & lblUser.Text & "' from " &
              "tempdodet where user = '" & lblUser.Text & "' and pono = 'wt' group by codeno"
        ExecuteNonQuery(sql)

        sql = "insert into tempdodet2(codeno,qty,wt,amt,uc,lotno,user) select codeno,ifnull(sum(qty),0),ifnull(sum(wt),0)," &
              "ifnull(sum(totcost),0),ifnull(sum(totcost),0) / ifnull(sum(qty),0),pono,'" & lblUser.Text & "' from " &
              "tempdodet where user = '" & lblUser.Text & "' and pono = 'qty' group by codeno"
        ExecuteNonQuery(sql)

        FillPurTr()
        getMMgrpDbox()

    End Sub

    Private Sub getMMgrpDbox()
        cboMMgrp.Items.Clear()

        Select Case vLoggedBussArea
            Case "8200"
                dt = GetDataTable("select b.mmgrp from tempdodet2 a left join mmasttbl b on a.codeno=b.codeno " &
                                  "where b.pc = '" & cboPClass2.Text.Substring(0, 1) & "' and a.user = '" & lblUser.Text & "'  group by b.mmgrp order by b.mmgrp")
                If Not CBool(dt.Rows.Count) Then
                    Exit Sub
                Else
                    cboMMgrp.Items.Add("")
                    cboMMgrp.Items.Add("ALL")
                    For Each dr As DataRow In dt.Rows
                        cboMMgrp.Items.Add(dr.Item(0).ToString())
                    Next
                End If

            Case "8100"
                Select Case True
                    Case RadioButton1.Checked
                        dt = GetDataTable("select b.mmgrp from tempcostingtbl2 a left join mmasttbl b on a.codeno=b.codeno where " &
                                          "a.user = '" & lblUser.Text & "' and b.pc = '" & cboPClass.Text.Substring(0, 1) & "' " &
                                          "and produce = 'Yes' group by b.mmgrp order by b.mmgrp")
                    Case RadioButton2.Checked
                        dt = GetDataTable("select b.mmgrp from tempfgcostingtbl a left join mmasttbl b on a.codeno=b.codeno where " &
                                          "a.user = '" & lblUser.Text & "' and b.pc = '" & cboPClass2.Text.Substring(0, 1) & "' and " &
                                          "b.tradeonly = 'Y' group by b.mmgrp order by b.mmgrp")
                        If Not CBool(dt.Rows.Count) Then
                            Exit Sub
                        Else
                            cboMMgrp.Items.Add("")
                            cboMMgrp.Items.Add("ALL")
                            For Each dr As DataRow In dt.Rows
                                cboMMgrp.Items.Add(dr.Item(0).ToString())
                            Next

                        End If
                End Select

            Case "8300"
                Select Case True
                    Case RadioButton1.Checked '
                        dt = GetDataTable("select b.mmgrp from tempcostingtbl a left join mmasttbl b on a.codeno=b.codeno where " &
                                          "a.user = '" & lblUser.Text & "' and b.pc = '" & cboPClass.Text.Substring(0, 1) & "' " &
                                          "and produce = 'Yes' group by b.mmgrp order by b.mmgrp")
                    Case RadioButton2.Checked
                        dt = GetDataTable("select b.mmgrp from tempfgcostingtbl a left join mmasttbl b on a.codeno=b.codeno where " &
                                          "a.user = '" & lblUser.Text & "' and b.pc = '" & cboPClass2.Text.Substring(0, 1) & "' and " &
                                          "b.tradeonly = 'Y' group by b.mmgrp order by b.mmgrp")
                End Select

                If Not CBool(dt.Rows.Count) Then
                    Exit Sub
                Else
                    cboMMgrp.Items.Add("")
                    cboMMgrp.Items.Add("ALL")
                    For Each dr As DataRow In dt.Rows
                        cboMMgrp.Items.Add(dr.Item(0).ToString())
                    Next

                End If

        End Select

        dt.Dispose()

    End Sub

    Private Sub FillPurTr()
        Dim adapter As New MySqlDataAdapter
        Dim ds As New DataSet()
        sqldata = Nothing

        dt = GetDataTable("select ifnull(sum(qty),0),ifnull(sum(wt),0),ifnull(sum(amt),0) from tempdodet2 " &
                          "where user = '" & lblUser.Text & "' group by user")
        If Not CBool(dt.Rows.Count) Then
            dgvRMlist2.DataSource = Nothing
            Exit Sub
        Else
            For Each dr As DataRow In dt.Rows
                txtTotRMqty2.Text = Format(CDbl(dr.Item(0).ToString()), "##,##0")
                txtTotRMwt2.Text = Format(CDbl(dr.Item(1).ToString()), "##,##0.00")
                txtRMtotAmt2.Text = Format(CDbl(dr.Item(2).ToString()), "##,##0.00")
            Next

        End If

        dt.Dispose()

        sqldata = "select concat(a.codeno,space(1),ifnull(b.codename,b.mmdesc)) as mmdesc,ifnull(a.uc,0) as asp," &
                  "ifnull(sum(a.qty),0) as aqty,ifnull(sum(a.wt),0) as awt,ifnull(sum(a.amt),0) as amt from tempdodet2 a " &
                  "left join mmasttbl b on a.codeno=b.codeno where a.user = '" & lblUser.Text & "' group by b.codeno order by ifnull(b.codename,b.mmdesc)"

        conn.Open()
        Dim command As New MySqlCommand(sqldata, conn)
        adapter.SelectCommand = command
        adapter.Fill(ds)
        adapter.Dispose()
        command.Dispose()
        conn.Close()
        dgvRMlist2.DataSource = ds.Tables(0)
        dgvRMlist2.DataBind()

        With dgvRMlist2
            .Columns(0).HeaderText = "Trading Goods"
            .Columns(1).HeaderText = "UC"
            .Columns(2).HeaderText = "Qty"
            .Columns(3).HeaderText = "Wt"
            .Columns(4).HeaderText = "Amount"

        End With

    End Sub

    Protected Sub dgvFGlist2_RowDataBound(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Protected Sub dgvFGlist2_RowCreated(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Private Sub CheckBox3_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox3.CheckedChanged
        If CheckBox3.Checked = True Then
            CheckBox3.Text = "Include Free/Deal"
        Else
            CheckBox3.Text = "Exclude Free/Deal"

        End If

    End Sub

    Protected Sub dgvTab1L_RowDataBound(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Protected Sub dgvTab1L_RowCreated(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Protected Sub dgvTab1R_RowDataBound(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Protected Sub dgvTab1R_RowCreated(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Private Sub cboPClass2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboPClass2.SelectedIndexChanged
        If cboPClass2.Text = "" Or cboPClass2.Text = Nothing Then
            Exit Sub
        End If

        Select Case TabContainer1.ActiveTabIndex
            Case 0
                RadioButton2.Checked = False
                RadioButton1.Checked = True
            Case 1
                RadioButton1.Checked = False
                RadioButton2.Checked = True
        End Select

        GetCCenterPlant2()
        CosProcQuery2()

    End Sub

    Private Sub cboMMgrp_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboMMgrp.SelectedIndexChanged
        If cboMMgrp.Text = "" Then
            Exit Sub

        End If

        cboMMdesc.Items.Clear()
        cboMMdesc.Text = Nothing
        dgvTab1L.DataSource = Nothing
        dgvTab1L.DataBind()
        dgvTab1R.DataSource = Nothing
        dgvTab1R.DataBind()

        Select Case True
            Case RadioButton1.Checked
                If lblProcStatus.Text <> "Processed" Then
                    AdmMsgBox("Production Realtime Costing Not Yet Processed")
                    TabContainer1.ActiveTabIndex = 0
                    Exit Sub
                End If

                PopMMdesc()

            Case RadioButton2.Checked
                If lblProcStatus2.Text <> "Processed" Then
                    AdmMsgBox("Trading Realtime Costing Not Yet Processed")
                    TabContainer1.ActiveTabIndex = 1
                    Exit Sub

                End If

                PopMMdescTrade()

        End Select


    End Sub

    Private Sub PopMMdescTrade()
        cboMMdesc.Items.Clear()

        Select Case vLoggedBussArea
            Case "8200"
                Select Case cboMMgrp.Text
                    Case "ALL"
                        dt = GetDataTable("select ifnull(b.codename,b.mmdesc) from tempdodet2 a left join mmasttbl b on a.codeno=b.codeno " &
                                          "where a.user = '" & lblUser.Text & "' group by a.codeno order by ifnull(b.codename,b.mmdesc)")
                        If Not CBool(dt.Rows.Count) Then
                            AdmMsgBox("Finished Goods Not found")
                            Exit Sub
                        Else
                            cboMMdesc.Items.Add("")
                            cboMMdesc.Items.Add("ALL")
                            For Each dr As DataRow In dt.Rows
                                cboMMdesc.Items.Add(dr.Item(0).ToString())

                            Next

                        End If
                    Case Else
                        dt = GetDataTable("select ifnull(b.codename,b.mmdesc) from tempdodet2 a left join mmasttbl b on a.codeno=b.codeno " &
                                          "where a.user = '" & lblUser.Text & "' and b.mmgrp = '" & cboMMgrp.Text & "' " &
                                          "group by a.codeno order by ifnull(b.codename,b.mmdesc)")
                        If Not CBool(dt.Rows.Count) Then
                            AdmMsgBox(cboMMgrp.Text & " Not found")
                            Exit Sub
                        Else
                            cboMMdesc.Items.Add("")
                            cboMMdesc.Items.Add("ALL")
                            For Each dr As DataRow In dt.Rows
                                cboMMdesc.Items.Add(dr.Item(0).ToString())

                            Next

                        End If

                End Select

                dt.Dispose()

            Case Else
                dt = GetDataTable("select ifnull(b.codename,b.mmdesc) from tempdodet2 a left join mmasttbl b on a.codeno=b.codeno " &
                                  "where a.user = '" & lblUser.Text & "' and b.mmgrp = '" & cboMMgrp.Text & "' and " &
                                  "b.pc = '" & cboPClass2.Text.Substring(0, 1) & "' group by a.codeno order by ifnull(b.codename,b.mmdesc)")
                If Not CBool(dt.Rows.Count) Then
                    AdmMsgBox("Finished Goods Not found")
                    Exit Sub
                Else
                    cboMMdesc.Items.Add("")
                    cboMMdesc.Items.Add("ALL")
                    For Each dr As DataRow In dt.Rows
                        cboMMdesc.Items.Add(dr.Item(0).ToString())

                    Next

                End If

        End Select

        Call dt.Dispose()

    End Sub

    Private Sub PopMMdesc()
        cboMMdesc.Items.Clear()
        cboMMdesc.Items.Add("ALL")

        Select Case vLoggedBussArea
            Case "8300"
                dt = GetDataTable("select ifnull(b.codename,b.mmdesc) from tempcostingtbl a left join mmasttbl b on a.codeno=b.codeno " &
                                  "where a.user = '" & lblUser.Text & "' and b.mmgrp = '" & cboMMgrp.Text & "' group by a.codeno order by b.mmdesc")
            Case Else
                dt = GetDataTable("select ifnull(b.codename,b.mmdesc) from tempcostingtbl2 a left join mmasttbl b on a.codeno=b.codeno " &
                                  "where a.user = '" & lblUser.Text & "' and b.mmgrp = '" & cboMMgrp.Text & "' group by a.codeno order by b.mmdesc")

        End Select

        If Not CBool(dt.Rows.Count) Then
            AdmMsgBox("Finished Goods Not found")
            Exit Sub
        Else
            For Each dr As DataRow In dt.Rows
                cboMMdesc.Items.Add(dr.Item(0).ToString())

            Next

        End If

        Call dt.Dispose()

    End Sub

    Private Sub cboMMdesc_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboMMdesc.SelectedIndexChanged
        If cboMMdesc.Text = "" Or cboMMdesc.Text = Nothing Then
            Exit Sub
        End If

        GetMMcodeno()


    End Sub

    Private Sub GetMMcodeno()

        Select Case cboMMdesc.Text
            Case "ALL"
                txtCodeNo.Text = ""
                txtCodeNo2.Text = ""

            Case Else
                dgvTab1R.DataSource = Nothing
                dt = GetDataTable("select codeno,basecodeno from mmasttbl where mmdesc = '" & cboMMdesc.Text & "'")
                If Not CBool(dt.Rows.Count) Then
                    Exit Sub
                Else
                    For Each dr As DataRow In dt.Rows
                        txtCodeNo.Text = dr.Item(0).ToString()
                        txtCodeNo2.Text = dr.Item(1).ToString()
                    Next

                End If

                Call dt.Dispose()

        End Select

        FilldgvTab1L()
        FillLeftPanelAll()


    End Sub

    Private Sub FilldgvTab1L()
        Dim adapter As New MySqlDataAdapter
        Dim ds As New DataSet()
        sqldata = Nothing
        dgvTab1L.DataSource = Nothing
        dgvTab1L.DataBind()

        Select Case True
            Case RadioButton1.Checked
                Select Case vLoggedBussArea
                    Case "8100"
                        Select Case cboMMdesc.Text
                            Case "ALL"
                                dt = GetDataTable("select * from tempcostingtbl2 a left join mmasttbl b on a.codeno=b.codeno " &
                                                  "where a.user = '" & lblUser.Text & "' and b.mmgrp = '" & cboMMgrp.Text & "'")
                                If Not CBool(dt.Rows.Count) Then
                                    Exit Sub
                                End If

                                dt.Dispose()

                                sqldata = "select a.codeno,b.mmdesc,ifnull(a.avecost,0) as avecos from tempcostingtbl2 a " &
                                          "left join mmasttbl b on a.codeno=b.codeno where a.user = '" & lblUser.Text & "' " &
                                          "and b.mmgrp = '" & cboMMgrp.Text & "' group by a.codeno order by b.mmdesc"
                            Case Else
                                dt = GetDataTable("select * from tempcostingtbl2 where user = '" & lblUser.Text & "' and " &
                                                  "codeno = '" & txtCodeNo.Text & "'")
                                If Not CBool(dt.Rows.Count) Then
                                    Exit Sub
                                End If

                                dt.Dispose()

                                sqldata = "select a.codeno,b.mmdesc,ifnull(a.avecost,0) as avecos from tempcostingtbl2 a " &
                                          "left join mmasttbl b on a.codeno=b.codeno where a.user = '" & lblUser.Text & "' " &
                                          "and a.codeno = '" & txtCodeNo.Text & "' group by a.codeno order by b.mmdesc"

                        End Select

                    Case "8200"
                        Exit Sub

                    Case "8300"
                        Select Case cboMMdesc.Text
                            Case "ALL"
                                dt = GetDataTable("select * from tempcostingtbl a left join mmasttbl b on a.codeno=b.codeno " &
                                                  "where a.user = '" & lblUser.Text & "' and b.mmgrp = '" & cboMMgrp.Text & "'")
                                If Not CBool(dt.Rows.Count) Then
                                    Exit Sub
                                End If

                                dt.Dispose()

                                sqldata = "select a.codeno,b.mmdesc,ifnull(a.avecost,0) as avecos from tempcostingtbl a " &
                                          "left join mmasttbl b on a.codeno=b.codeno where a.user = '" & lblUser.Text & "' " &
                                           "and b.mmgrp = '" & cboMMgrp.Text & "' group by a.codeno order by b.mmdesc"
                            Case Else
                                dt = GetDataTable("select * from tempcostingtbl where user = '" & lblUser.Text & "' and " &
                                                  "codeno = '" & txtCodeNo.Text & "'")
                                If Not CBool(dt.Rows.Count) Then
                                    Exit Sub
                                End If

                                dt.Dispose()

                                sqldata = "select a.codeno,b.mmdesc,ifnull(a.avecost,0) as avecos from tempcostingtbl a " &
                                          "left join mmasttbl b on a.codeno=b.codeno where a.user = '" & lblUser.Text & "' " &
                                          "and a.codeno = '" & txtCodeNo.Text & "' group by a.codeno order by b.mmdesc"

                        End Select
                End Select

            Case RadioButton2.Checked
                Select Case vLoggedBussArea
                    Case "8200"
                        Select Case cboMMgrp.Text
                            Case "ALL"
                                Select Case cboMMdesc.Text
                                    Case "ALL"
                                        dt = GetDataTable("select * from tempdodet2 a left join mmasttbl b on a.codeno=b.codeno " &
                                                          "where a.user = '" & lblUser.Text & "'")
                                        If Not CBool(dt.Rows.Count) Then
                                            Exit Sub
                                        End If

                                        dt.Dispose()

                                        sqldata = "select a.codeno,b.mmdesc,ifnull(a.uc,0) as avecos from tempdodet2 a " &
                                                  "left join mmasttbl b on a.codeno=b.codeno where a.user = '" & lblUser.Text & "' " &
                                                  "group by a.codeno order by b.mmdesc"
                                    Case Else
                                        dt = GetDataTable("select * from tempdodet2 a left join mmasttbl b on a.codeno=b.codeno " &
                                                          "where a.user = '" & lblUser.Text & "' and b.basecodeno = '" & txtCodeNo2.Text & "'")
                                        If Not CBool(dt.Rows.Count) Then
                                            Exit Sub
                                        End If

                                        dt.Dispose()

                                        sqldata = "select a.codeno,b.mmdesc,ifnull(a.uc,0) as avecos from tempdodet2 a " &
                                                  "left join mmasttbl b on a.codeno=b.codeno where a.user = '" & lblUser.Text & "' " &
                                                  "and b.basecodeno = '" & txtCodeNo2.Text & "' group by a.codeno order by b.mmdesc"

                                End Select

                            Case Else
                                Select Case cboMMdesc.Text
                                    Case "ALL"
                                        dt = GetDataTable("select * from tempdodet2 a left join mmasttbl b on a.codeno=b.codeno " &
                                                          "where a.user = '" & lblUser.Text & "' and b.mmgrp = '" & cboMMgrp.Text & "'")
                                        If Not CBool(dt.Rows.Count) Then
                                            Exit Sub
                                        End If

                                        dt.Dispose()

                                        sqldata = "select a.codeno,b.mmdesc,ifnull(a.uc,0) as avecos from tempdodet2 a " &
                                                  "left join mmasttbl b on a.codeno=b.codeno where a.user = '" & lblUser.Text & "' " &
                                                  "and b.mmgrp = '" & cboMMgrp.Text & "' group by a.codeno order by b.mmdesc"
                                    Case Else
                                        dt = GetDataTable("select * from tempdodet2 a left join mmasttbl b on a.codeno=b.codeno " &
                                                          "where a.user = '" & lblUser.Text & "' and b.basecodeno = '" & txtCodeNo2.Text & "'")
                                        If Not CBool(dt.Rows.Count) Then
                                            Exit Sub
                                        End If

                                        dt.Dispose()

                                        sqldata = "select a.codeno,b.mmdesc,ifnull(a.uc,0) as avecos from tempdodet2 a " &
                                                  "left join mmasttbl b on a.codeno=b.codeno where a.user = '" & lblUser.Text & "' " &
                                                  "and b.basecodeno = '" & txtCodeNo2.Text & "' group by a.codeno order by b.mmdesc"
                                End Select

                        End Select

                    Case Else
                        Select Case cboMMdesc.Text
                            Case "ALL"
                                dt = GetDataTable("select * from tempdodet2 a left join mmasttbl b on a.codeno=b.codeno " &
                                                  "where a.user = '" & lblUser.Text & "' and b.mmgrp = '" & cboMMgrp.Text & "'")
                                If Not CBool(dt.Rows.Count) Then
                                    Exit Sub
                                End If

                                dt.Dispose()

                                sqldata = "select a.codeno,b.mmdesc,ifnull(a.uc,0) as avecos from tempdodet2 a " &
                                          "left join mmasttbl b on a.codeno=b.codeno where a.user = '" & lblUser.Text & "' " &
                                          "and b.mmgrp = '" & cboMMgrp.Text & "' group by a.codeno order by b.mmdesc"
                            Case Else
                                dt = GetDataTable("select * from tempdodet2 a left join mmasttbl b on a.codeno=b.codeno " &
                                                  "where a.user = '" & lblUser.Text & "' and b.basecodeno = '" & txtCodeNo2.Text & "'")
                                If Not CBool(dt.Rows.Count) Then
                                    Exit Sub
                                End If

                                dt.Dispose()

                                sqldata = "select a.codeno,b.mmdesc,ifnull(a.uc,0) as avecos from tempdodet2 a " &
                                          "left join mmasttbl b on a.codeno=b.codeno where a.user = '" & lblUser.Text & "' " &
                                          "and b.basecodeno = '" & txtCodeNo2.Text & "' group by a.codeno order by b.mmdesc"
                        End Select
                End Select
        End Select

        With dgvTab1L
            .Columns(0).HeaderText = "Code No."
            .Columns(1).HeaderText = "Material"
            .Columns(2).HeaderText = "Ave. Cost"

        End With

        conn.Open()
        Dim command As New MySqlCommand(sqldata, conn)
        adapter.SelectCommand = command
        adapter.Fill(ds)
        adapter.Dispose()
        command.Dispose()
        conn.Close()
        dgvTab1L.DataSource = ds.Tables(0)
        dgvTab1L.DataBind()


    End Sub

    Private Sub FillLeftPanelAll()
        sql = "delete from tempsalesdettbl where user ='" & lblUser.Text & "'"
        ExecuteNonQuery(sql)

        Select Case True
            Case RadioButton1.Checked
                Select Case cboMMdesc.Text
                    Case "ALL"
                        If CheckBox3.Checked Then
                            sql = "insert into tempsalesdettbl(dsrno,codeno,qty,wt,itmamt,billref,nv,user) select b.custno,a.codeno,ifnull(sum(a.qty),0),ifnull(sum(a.wt),0)," &
                                  "ifnull(sum(a.itmamt),0),c.billref,c.qtpk,'" & lblUser.Text & "' from salesdettbl a left join saleshdrtbl b on a.invno=b.invno " &
                                  "left join mmasttbl c on a.codeno=c.codeno where b.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                                  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and b.status <> 'void' and c.pc = '" & cboPClass.Text.Substring(0, 1) & "' " &
                                  "and a.sp > 0 and c.mmgrp = '" & cboMMgrp.Text & "' group by b.custno,a.codeno"
                            ExecuteNonQuery(sql)

                        Else
                            sql = "insert into tempsalesdettbl(dsrno,codeno,qty,wt,itmamt,billref,nv,user) select b.custno,a.codeno,ifnull(sum(a.qty),0),ifnull(sum(a.wt),0)," &
                                  "ifnull(sum(a.itmamt),0),c.billref,c.qtpk,'" & lblUser.Text & "' from salesdettbl a left join saleshdrtbl b on a.invno=b.invno " &
                                  "left join mmasttbl c on a.codeno=c.codeno where b.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                                  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and b.status <> 'void' and c.pc = '" & cboPClass.Text.Substring(0, 1) & "' " &
                                  "and c.mmgrp = '" & cboMMgrp.Text & "' group by b.custno,a.codeno"
                            ExecuteNonQuery(sql)


                        End If
                    Case Else
                        Select Case vLoggedBussArea
                            Case "8100"
                                If CheckBox3.Checked Then
                                    sql = "insert into tempsalesdettbl(dsrno,codeno,qty,wt,itmamt,billref,itemno,user) select b.custno,a.codeno,ifnull(sum(a.qty),0),ifnull(sum(a.wt),0)," &
                                          "ifnull(sum(a.itmamt),0),c.billref,c.qtpk,'" & lblUser.Text & "' from salesdettbl a left join saleshdrtbl b on a.invno=b.invno " &
                                          "left join mmasttbl c on a.codeno=c.codeno where b.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                                          "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and b.status <> 'void' and c.basecodeno = '" & txtCodeNo2.Text & "' " &
                                          "and c.pc = '" & cboPClass.Text.Substring(0, 1) & "' and a.sp > 0 and c.mmgrp = '" & cboMMgrp.Text & "' group by b.custno,a.codeno"
                                    ExecuteNonQuery(sql)

                                Else
                                    sql = "insert into tempsalesdettbl(dsrno,codeno,qty,wt,itmamt,billref,itemno,user) select b.custno,a.codeno,ifnull(sum(a.qty),0),ifnull(sum(a.wt),0)," &
                                          "ifnull(sum(a.itmamt),0),c.billref,c.qtpk,'" & lblUser.Text & "' from salesdettbl a left join saleshdrtbl b on a.invno=b.invno " &
                                          "left join mmasttbl c on a.codeno=c.codeno where b.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                                          "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and b.status <> 'void' and c.basecodeno = '" & txtCodeNo2.Text & "' " &
                                          "and c.pc = '" & cboPClass.Text.Substring(0, 1) & "' and c.mmgrp = '" & cboMMgrp.Text & "' group by b.custno,a.codeno"
                                    ExecuteNonQuery(sql)

                                End If
                            Case "8300"
                                If CheckBox3.Checked Then
                                    sql = "insert into tempsalesdettbl(dsrno,codeno,qty,wt,itmamt,billref,itemno,user) select b.custno,a.codeno,ifnull(sum(a.qty),0),ifnull(sum(a.wt),0)," &
                                          "ifnull(sum(a.itmamt),0),c.billref,c.qtpk,'" & lblUser.Text & "' from salesdettbl a left join saleshdrtbl b on a.invno=b.invno " &
                                          "left join mmasttbl c on a.codeno=c.codeno where b.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                                          "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and b.status <> 'void' and a.codeno = '" & txtCodeNo.Text & "' " &
                                          "and c.pc = '" & cboPClass.Text.Substring(0, 1) & "' and a.sp > 0 and c.mmgrp = '" & cboMMgrp.Text & "' group by b.custno,a.codeno"
                                    ExecuteNonQuery(sql)

                                Else
                                    sql = "insert into tempsalesdettbl(dsrno,codeno,qty,wt,itmamt,billref,itemno,user) select b.custno,a.codeno,ifnull(sum(a.qty),0),ifnull(sum(a.wt),0)," &
                                          "ifnull(sum(a.itmamt),0),c.billref,c.qtpk,'" & lblUser.Text & "' from salesdettbl a left join saleshdrtbl b on a.invno=b.invno " &
                                          "left join mmasttbl c on a.codeno=c.codeno where b.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                                          "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and b.status <> 'void' and a.codeno = '" & txtCodeNo.Text & "' " &
                                          "and c.pc = '" & cboPClass.Text.Substring(0, 1) & "' and c.mmgrp = '" & cboMMgrp.Text & "' group by b.custno,a.codeno"
                                    ExecuteNonQuery(sql)

                                End If
                        End Select

                End Select

            Case RadioButton2.Checked
                Select Case cboMMdesc.Text
                    Case "ALL"
                        If CheckBox3.Checked Then
                            sql = "insert into tempsalesdettbl(dsrno,codeno,qty,wt,itmamt,billref,nv,user) select b.custno,a.codeno,ifnull(sum(a.qty),0),ifnull(sum(a.wt),0)," &
                                  "ifnull(sum(a.itmamt),0),c.billref,c.qtpk,'" & lblUser.Text & "' from salesdettbl a left join saleshdrtbl b on a.invno=b.invno " &
                                  "left join mmasttbl c on a.codeno=c.codeno where b.transdate between  '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                                  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and b.status <> 'void' and c.pc = '" & cboPClass2.Text.Substring(0, 1) & "' " &
                                  "and c.mmgrp = '" & cboMMgrp.Text & "' and a.sp > 0 group by b.custno,a.codeno"
                            ExecuteNonQuery(sql)

                        Else
                            sql = "insert into tempsalesdettbl(dsrno,codeno,qty,wt,itmamt,billref,nv,user) select b.custno,a.codeno,ifnull(sum(a.qty),0),ifnull(sum(a.wt),0)," &
                                  "ifnull(sum(a.itmamt),0),c.billref,c.qtpk,'" & lblUser.Text & "' from salesdettbl a left join saleshdrtbl b on a.invno=b.invno " &
                                  "left join mmasttbl c on a.codeno=c.codeno where b.transdate between  '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                                  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and b.status <> 'void' and c.pc = '" & cboPClass2.Text.Substring(0, 1) & "' " &
                                  "and c.mmgrp = '" & cboMMgrp.Text & "' group by b.custno,a.codeno"
                            ExecuteNonQuery(sql)


                        End If
                    Case Else
                        If CheckBox3.Checked Then
                            sql = "insert into tempsalesdettbl(dsrno,codeno,qty,wt,itmamt,billref,itemno,user) select b.custno,a.codeno,ifnull(sum(a.qty),0),ifnull(sum(a.wt),0)," &
                                  "ifnull(sum(a.itmamt),0),c.billref,c.qtpk,'" & lblUser.Text & "' from salesdettbl a left join saleshdrtbl b on a.invno=b.invno " &
                                  "left join mmasttbl c on a.codeno=c.codeno where b.transdate between  '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                                  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and b.status <> 'void' and c.basecodeno = '" & txtCodeNo2.Text & "' " &
                                  "and c.pc = '" & cboPClass2.Text.Substring(0, 1) & "' and c.mmgrp = '" & cboMMgrp.Text & "' and a.sp > 0 group by b.custno,a.codeno"
                            ExecuteNonQuery(sql)

                        Else
                            sql = "insert into tempsalesdettbl(dsrno,codeno,qty,wt,itmamt,billref,itemno,user) select b.custno,a.codeno,ifnull(sum(a.qty),0),ifnull(sum(a.wt),0)," &
                                  "ifnull(sum(a.itmamt),0),c.billref,c.qtpk,'" & lblUser.Text & "' from salesdettbl a left join saleshdrtbl b on a.invno=b.invno " &
                                  "left join mmasttbl c on a.codeno=c.codeno where b.transdate between  '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                                  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and b.status <> 'void' and c.basecodeno = '" & txtCodeNo2.Text & "' " &
                                  "and c.pc = '" & cboPClass2.Text.Substring(0, 1) & "' and c.mmgrp = '" & cboMMgrp.Text & "' group by b.custno,a.codeno"
                            ExecuteNonQuery(sql)

                        End If

                End Select

        End Select
        'End If

        sql = "update tempsalesdettbl set sp = ifnull(itmamt,0) / ifnull(wt,0) where billref = 'wt' and user = '" & lblUser.Text & "'"
        ExecuteNonQuery(sql)

        sql = "update tempsalesdettbl set sp = ifnull(itmamt,0) / ifnull(qty,0) where billref = 'qty' and user = '" & lblUser.Text & "'"
        ExecuteNonQuery(sql) 'detamt
        Select Case vLoggedBussArea
            Case "8100", "8200"
                sql = "update tempsalesdettbl set detamt = ifnull(sp,0) / ifnull(itemno,0) where billref = 'qty' and user = '" & lblUser.Text & "'"
                ExecuteNonQuery(sql)

                sql = "update tempsalesdettbl set detamt = ifnull(itmamt,0) / ifnull(wt,0) where billref = 'wt' and user = '" & lblUser.Text & "'"
                ExecuteNonQuery(sql) '

            Case "8300"
                sql = "update tempsalesdettbl set detamt = ifnull(itmamt,0) / ifnull(wt,0) where user = '" & lblUser.Text & "'"
                ExecuteNonQuery(sql)

        End Select

        Dim adapter As New MySqlDataAdapter
        Dim ds As New DataSet()
        sqldata = Nothing
        dgvTab1R.DataSource = Nothing
        dgvTab1R.DataBind()

        cboCustomer.Items.Clear()

        dt = GetDataTable("select distinct concat(a.dsrno,space(1),d.bussname) from tempsalesdettbl a " &
                          "left join custmasttbl d on a.dsrno=d.custno where a.user ='" & lblUser.Text & "' order by d.bussname")
        If Not CBool(dt.Rows.Count) Then
            Exit Sub
        Else
            cboCustomer.Items.Add("")
            For Each dr As DataRow In dt.Rows
                cboCustomer.Items.Add(dr.Item(0).ToString())

            Next
        End If

        dt.Dispose()

        sqldata = "select concat(a.dsrno,space(1),d.bussname) as cust,a.codeno,ifnull(c.codename,c.mmdesc) as mm," &
                  "ifnull(a.qty,0) as qty,ifnull(a.wt,0) as wt,ifnull(a.itmamt,0) as amt,ifnull(a.sp,0) as asp," &
                  "ifnull(a.detamt,a.sp) as perkg from tempsalesdettbl a left join mmasttbl c on a.codeno=c.codeno " &
                  "left join custmasttbl d on a.dsrno=d.custno where a.user ='" & lblUser.Text & "'"

        With dgvTab1R
            .Columns(0).HeaderText = "Customer"
            .Columns(1).HeaderText = "Code No."
            .Columns(2).HeaderText = "Description"
            .Columns(3).HeaderText = "Qty"
            .Columns(4).HeaderText = "Wt/Vol"
            .Columns(5).HeaderText = "Total Amt"
            .Columns(6).HeaderText = "ASP"
            .Columns(7).HeaderText = "ASP/Wt/Vol"

        End With

        conn.Open()
        Dim command As New MySqlCommand(sqldata, conn)
        adapter.SelectCommand = command
        adapter.Fill(ds)
        adapter.Dispose()
        command.Dispose()
        conn.Close()
        dgvTab1R.DataSource = ds.Tables(0)
        dgvTab1R.DataBind()

    End Sub

    Private Sub cboCustomer_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboCustomer.SelectedIndexChanged
        If cboCustomer.Text = "" Or cboCustomer.Text = Nothing Then
            Exit Sub
        End If

        FillCustList()

    End Sub

    Private Sub FillCustList()
        Dim adapter As New MySqlDataAdapter
        Dim ds As New DataSet()
        sqldata = Nothing
        dgvListPerCust.DataSource = Nothing
        dgvListPerCust.DataBind()

        Select Case True
            Case RadioButton1.Checked
                AdmMsgBox("Not Yet Available")
                Exit Sub

            Case RadioButton2.Checked
                dt = GetDataTable("select * from salesdettbl a left join saleshdrtbl b on a.invno=b.invno where " &
                                  "b.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                                  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and b.status <> 'void' " &
                                  "and b.custno = '" & cboCustomer.Text.Substring(0, 5) & "'")
                If Not CBool(dt.Rows.Count) Then
                    Exit Sub
                Else
                    TempdataGMperCust()

                End If

                sqldata = "select concat(a.codeno,space(1),ifnull(b.codename,b.mmdesc)) as mmdesc,a.transdate,a.dsrno," &
                          "a.qty,a.wt,a.detamt,a.detdiscamt,a.sp,a.itmamt,a.detgrossamt,a.detvat from tempsalesdettbl a " &
                          "left join mmasttbl b on a.codeno=b.codeno where a.user = '" & lblUser.Text & "'"

                With dgvListPerCust
                    .Columns(0).HeaderText = "PRODUCT"
                    .Columns(1).HeaderText = "SI DATE"
                    .Columns(2).HeaderText = "REF NO."
                    .Columns(3).HeaderText = "QTY"
                    .Columns(4).HeaderText = "WT/VOL"
                    .Columns(5).HeaderText = "UC"
                    .Columns(6).HeaderText = "COST AMT"
                    .Columns(7).HeaderText = "SP"
                    .Columns(8).HeaderText = "SALES AMT"
                    .Columns(9).HeaderText = "GROSS MARGIN"
                    .Columns(10).HeaderText = "%GM"

                End With

                conn.Open()
                Dim command As New MySqlCommand(sqldata, conn)
                adapter.SelectCommand = command
                adapter.Fill(ds)
                adapter.Dispose()
                command.Dispose()
                conn.Close()
                dgvListPerCust.DataSource = ds.Tables(0)
                dgvListPerCust.DataBind()

        End Select

    End Sub

    Private Sub TempdataGMperCust()
        sql = "delete from tempsalesdettbl where user = '" & lblUser.Text & "'"
        ExecuteNonQuery(sql)

        sql = "insert into tempsalesdettbl(invno,transdate,dsrno,codeno,qty,wt,sp,itmamt,detamt,user) select a.invno,b.transdate,b.docno," &
              "a.codeno,a.qty,a.wt,a.sp,a.itmamt,Ifnull(a.tempcos,0),'" & lblUser.Text & "' from salesdettbl a left join saleshdrtbl b " &
              "on a.invno=b.invno where b.status <> 'void' and b.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
              "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and b.status <> 'void' and a.sp > 0 and " &
              "b.custno = '" & cboCustomer.Text.Substring(0, 5) & "'"
        ExecuteNonQuery(sql)

        sql = "update tempsalesdettbl a,(select codeno,billref from mmasttbl) as b set a.billref=b.billref where a.codeno=b.codeno and " &
              "a.user = '" & lblUser.Text & "'"
        ExecuteNonQuery(sql)

        Select Case vLoggedBussArea
            Case "8200"
                sql = "update tempsalesdettbl set detdiscamt = ifnull(detamt,0) * ifnull(wt,0) where user = '" & lblUser.Text & "' " &
                      "and ifnull(billref,'wt') = 'wt'"
                ExecuteNonQuery(sql)

                sql = "update tempsalesdettbl set detdiscamt = ifnull(detamt,0) * ifnull(qty,0) where user = '" & lblUser.Text & "' " &
                      "and ifnull(billref,'wt') = 'qty'"
                ExecuteNonQuery(sql)

            Case "8300"
                sql = "update tempsalesdettbl a,(select codeno,billref from mmasttbl) as b set a.billref=b.billref " &
                      "where a.codeno=b.codeno and a.user = '" & lblUser.Text & "'"
                ExecuteNonQuery(sql)

                Select Case True
                    Case RadioButton1.Checked
                        Select Case cboPClass.Text.Substring(0, 1)
                            Case "1"
                                sql = "update tempsalesdettbl set detdiscamt = ifnull(detamt,0) * ifnull(wt,0) where user = '" & lblUser.Text & "' " &
                                      "and ifnull(billref,'wt') = 'wt'"
                                ExecuteNonQuery(sql)

                                sql = "update tempsalesdettbl set detdiscamt = ifnull(detamt,0) * ifnull(qty,0) where user = '" & lblUser.Text & "' " &
                                      "and ifnull(billref,'wt') = 'qty'"
                                ExecuteNonQuery(sql)

                            Case "2"
                                sql = "update tempsalesdettbl set detdiscamt = ifnull(detamt,0) * ifnull(wt,0) where user = '" & lblUser.Text & "' " &
                                      "and ifnull(billref,'wt') = 'wt'"
                                ExecuteNonQuery(sql)

                                sql = "update tempsalesdettbl set detdiscamt = ifnull(detamt,0) * ifnull(qty,0) where user = '" & lblUser.Text & "' " &
                                      "and ifnull(billref,'wt') = 'qty'"
                                ExecuteNonQuery(sql)

                        End Select
                    Case RadioButton2.Checked
                        Select Case cboPClass2.Text.Substring(0, 1)
                            Case "1"
                                sql = "update tempsalesdettbl set detdiscamt = ifnull(detamt,0) * ifnull(wt,0) where user = '" & lblUser.Text & "' " &
                                      "and ifnull(billref,'wt') = 'wt'"
                                ExecuteNonQuery(sql)

                                sql = "update tempsalesdettbl set detdiscamt = ifnull(detamt,0) * ifnull(qty,0) where user = '" & lblUser.Text & "' " &
                                      "and ifnull(billref,'wt') = 'qty'"
                                ExecuteNonQuery(sql)

                            Case "2"
                                sql = "update tempsalesdettbl set detdiscamt = ifnull(detamt,0) * ifnull(wt,0) where user = '" & lblUser.Text & "' " &
                                      "and ifnull(billref,'wt') = 'wt'"
                                ExecuteNonQuery(sql)

                                sql = "update tempsalesdettbl set detdiscamt = ifnull(detamt,0) * ifnull(qty,0) where user = '" & lblUser.Text & "' " &
                                      "and ifnull(billref,'wt') = 'qty'"
                                ExecuteNonQuery(sql)

                        End Select
                End Select

        End Select

        sql = "update tempsalesdettbl set detgrossamt = ifnull(itmamt,0) - ifnull(detdiscamt,0) where user = '" & lblUser.Text & "'"
        ExecuteNonQuery(sql)

        sql = "update tempsalesdettbl set detvat = ifnull(detgrossamt,0) / ifnull(itmamt,0) * 100 where user = '" & lblUser.Text & "'"
        ExecuteNonQuery(sql)


    End Sub

    Protected Sub dgvListPerCust_RowDataBound(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Protected Sub dgvListPerCust_RowCreated(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged, RadioButton2.CheckedChanged
        If sender.Equals(RadioButton1) Then
            If RadioButton1.Checked = True Then
                RadioButton2.Checked = False
            Else
                RadioButton2.Checked = True
            End If
        End If

        If sender.Equals(RadioButton2) Then
            If RadioButton2.Checked = True Then
                RadioButton1.Checked = False
            Else
                RadioButton1.Checked = True
            End If
        End If

    End Sub
End Class