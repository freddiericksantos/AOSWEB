Option Explicit On
Imports MySql.Data.MySqlClient

Public Class DailySalesReport
    Inherits System.Web.UI.Page
    Dim strReport As String
    Dim sql As String
    Dim dt As DataTable
    Dim dt2 As DataTable
    Dim sortColumn As Integer = -1
    Dim MyDA_conn As New MySqlDataAdapter
    Dim MyDataSet As New DataSet
    Dim MySqlScript As String
    Dim strTC As String
    Dim strUnit As String
    Dim Answer As String
    Dim vDateIndex As Integer
    Dim strAMsckDSR As String
    Dim admMMdesc As String
    Dim admAmt As Double
    Dim sqldata As String

    Private Sub SaveLogs()

        Dim strForm As String = "DSR"
        sql = "insert into translog(trans,form,datetimelog,user,docno,tc)values" &
              "('" & strReport & "','" & strForm & "','" & Format(CDate(Now), "yyyy-MM-dd hh:mm:ss") & "'," &
              "'" & lblUser.Text & "','" & txtDSRNo.Text & "','" & "dsr" & "')"
        ExecuteNonQuery(sql)

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If lblUser.Text Is Nothing Then
            Response.Redirect("login.aspx")

        End If

        If Not Me.IsPostBack Then
            lblUser.Text = Session("UserID")
            lblGrpUser.Text = Session("UserGrp")

            vThisFormCode = "046"
            Call CheckGroupRights()

            vJVsource = "Sales"

            txtDSRNo.ReadOnly = False

            cboStat.Items.Clear()
            cboStat.Items.Add("")
            cboStat.Items.Add("OPEN")
            cboStat.Items.Add("COMPLETED")

            cboShrUnit.Items.Clear()
            Select Case vLoggedBussArea
                Case "8100", "8200"
                    cboShrUnit.Items.Add("Heads")
                    cboShrUnit.Items.Add("Kilos")
                    Label9.Text = "Salesman:"

                Case Else
                    cboShrUnit.Items.Add("Packs")
                    cboShrUnit.Items.Add("Pcs")
                    Label9.Text = "Salesman:"

            End Select

            cboStatAll.Items.Clear()
            cboStatAll.Items.Add("")
            cboStatAll.Items.Add("ALL")
            cboStatAll.Items.Add("OPEN")
            cboStatAll.Items.Add("COMPLETED")

            cboInv.Items.Clear()
            cboInv.Items.Add("")
            cboInv.Items.Add("84 ETCSI")
            cboInv.Items.Add("85 ETCI")

            popPC()

            cboSmnName.Items.Clear()
            dt = GetDataTable("select concat(smnno,space(1),fullname) from smnmtbl where status = 'active' order by fullname")
            If Not CBool(dt.Rows.Count) Then
                Exit Sub

            Else
                cboSmnName.Items.Add("")
                For Each dr As DataRow In dt.Rows
                    cboSmnName.Items.Add(dr.Item(0).ToString())

                Next
            End If

            Call dt.Dispose()

        End If

    End Sub

    Private Sub popPC()

        cboPC.Items.Clear()

        dt = GetDataTable("select pclass from pctrtbl where stat = 'Active' and tradetype = 'trade' and dsr = 'yes'")
        If Not CBool(dt.Rows.Count) Then
            Exit Sub

        Else
            For Each dr As DataRow In dt.Rows
                cboPC.Items.Add(dr.Item(0).ToString())

            Next

        End If

        dt.Dispose()

    End Sub

    Protected Sub OnConfirm2(sender As Object, e As EventArgs)
        Dim confirmValue As String = Request.Form("confirm_value")
        If confirmValue = "Yes" Then
            'RemoveLineItem()
        Else
            'lblErrMsg.Text = "Delete Cancelled"
        End If

    End Sub

    Private Sub CheckGroupRights()
        If IsAllowed(vLoggedUserGroupID, vThisFormCode, 3) = True Then ' 3 = Insert 
            cmdAddDO.Enabled = True
            'cmdAddSI.Enabled = True
            lbSave.Enabled = True
            Button3.Enabled = True
            'cmdAddItm.Enabled = True 'temp comment
            'ToolStripMenuItem2.Enabled = True

        Else
            cmdAddDO.Enabled = False
            'cmdAddSI.Enabled = False
            lbSave.Enabled = False
            Button3.Enabled = False
            'cmdAddItm.Enabled = False


        End If


        'If IsAllowed(vLoggedUserGroupID, vThisFormCode, 5) = True Then ' 5 post
        '    txtDSRamReload.ReadOnly = False
        '    btnAMreload.Enabled = True

        'Else
        '    txtDSRamReload.ReadOnly = True
        '    btnAMreload.Enabled = False

        'End If


        'If IsAllowed(vLoggedUserGroupID, vThisFormCode, 4) = True Then ' 4 = Delete
        '    VoidToolStripMenuItem.Enabled = True
        '    cmdRemDO.Enabled = True
        '    Button1.Enabled = True
        '    cmdDetItm.Enabled = True
        '    VoidToolStripMenuItem.Enabled = True

        'Else
        '    VoidToolStripMenuItem.Enabled = False
        '    cmdRemDO.Enabled = False
        '    Button1.Enabled = False
        '    cmdDetItm.Enabled = False
        '    VoidToolStripMenuItem.Enabled = False

        'End If


    End Sub

    Private Sub LoadExistingDSR()

        Call RRsalesAmt()
        Call popDONo()
        Call popWRRNo()
        Call fillWRRsum()
        Call ListDOsmn()
        Call FillLvPMstk()
        Call getMMstocks()
        Call UpdateStocks()
        Call ChkStockAvail()
        Call CalcRR()

        vLastAct = "DSR" & " Reload DSR No." & txtDSRNo.Text & Space(1) & cboSmnName.Text.Substring(0, 3)
        WriteToLogs(vLastAct)

    End Sub

    Private Sub CalcRR()
        Try

            If txtDSRNo.Text = "" Then
                'txtDSRNo.Focus()
                MsgBox("DSR No. is Blank", vbCritical)

                Exit Sub

            Else

                dt = GetDataTable("select ifnull(sum(netamt),0) from saleshdrtbl where " &
                                  "dsrno = '" & txtDSRNo.Text & "' and tc = '" & "84" & "' and " &
                                  "status <> '" & "void" & "'")
                If Not CBool(dt.Rows.Count) Then
                    'Call MessageBox.Show("No Cash Sales found.", "DSR System", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Exit Sub
                End If

                For Each dr As DataRow In dt.Rows
                    lblRRtot84.Text = Format(dr.Item(0), "##,##0.00".ToString())
                    lblTot84.Text = Format(dr.Item(0), "##,##0.00".ToString())

                Next

                Call dt.Dispose()

                dt = GetDataTable("select ifnull(sum(netamt),0) from saleshdrtbl where dsrno = '" & txtDSRNo.Text & "' " &
                                  "and tc = '" & "85" & "' and status <> '" & "void" & "'")
                If Not CBool(dt.Rows.Count) Then
                    'Call MessageBox.Show("No Charge Sales found.", "DSR System", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Exit Sub
                End If

                For Each dr As DataRow In dt.Rows
                    lblTot85.Text = Format(dr.Item(0), "##,##0.00".ToString())

                Next

                Call dt.Dispose()

                Dim RRVar As Double = CDbl(IIf(lblRRtot84.Text = "", 0, lblRRtot84.Text)) - CDbl(IIf(txtRRAmt.Text = "", 0, txtRRAmt.Text))
                lblVarRR.Text = Format(RRVar, "##,##0.00")

                'If lblVarRR.Text < 0 Then
                '    lblVarRR.ForeColor = Color.red
                'Else
                '    lblVarRR.ForeColor = Color.Black
                'End If

                Dim TotSales As Double = CDbl(IIf(lblTot84.Text = "", 0, lblTot84.Text)) + CDbl(IIf(lblTot85.Text = "", 0, lblTot85.Text))
                lblTotalSales.Text = Format(TotSales, "##,##0.00")


            End If

        Catch ex As Exception
            'mdiMain.tsErrMsg.Text = ErrorToString()

        End Try

    End Sub

    Private Sub ChkStockAvail()
        dt = GetDataTable("select ifnull(availablestck,0),ifnull(sales,0),ifnull(wrr,0),ifnull(pmstock,0),ifnull(qty,0),ifnull(qty2,0)," &
                          "ifnull(qty3,0),ifnull(qty4,0) from tempdsreport where codeno = '" & txtCodeNo.Text & "' and user = '" & vLoggedUserID & "'")
        If Not CBool(dt.Rows.Count) Then
            Exit Sub

        Else
            For Each dr As DataRow In dt.Rows
                Dim Avai As Double = CDbl(dr.Item(0).ToString() - dr.Item(1).ToString() - dr.Item(2).ToString() - dr.Item(3).ToString())
                Dim Heads As Long = CLng(dr.Item(4).ToString() - dr.Item(5).ToString() - dr.Item(6).ToString() - dr.Item(7).ToString())

                'put modal here
                tssErrorMsg.Text = "Available for " & txtCodeNo.Text & ":" & Space(3) & Format(Heads, "#,##0Hds") & Space(3) & Format(Avai, "#,##0.00kgs")

            Next

        End If

        Call dt.Dispose()

    End Sub

    Private Sub clrLines()

        txtCodeNo.Text = ""
        cboMMdesc.Text = Nothing
        txtQty.Text = ""
        txtWt.Text = ""
        txtCodeNo.Focus()

    End Sub

    Private Sub FillLvPMstk()
        dt = GetDataTable("select sum(ifnull(a.qty,0)),sum(ifnull(a.wt,0)) from pmstocktbl where " &
                          "dsrno = '" & txtDSRNo.Text & "' and smnno = '" & cboSmnName.Text.Substring(0, 3) & "'")
        If Not CBool(dt.Rows.Count) Then
            dgvPMstock.DataSource = Nothing
            dgvPMstock.DataBind()
            Exit Sub '
        Else
            For Each dr As DataRow In dt.Rows
                lblPMqtyTot.Text = Format(CLng(dr.Item(0).ToString()), "#,##0 ")
                lblPMwtTot.Text = Format(CLng(dr.Item(1).ToString()), "#,##0.00 ")

            Next

            txtItm.Text = dgvPMstock.Rows.Count + 1


        End If

        Call dt.Dispose()

        Dim adapter As New MySqlDataAdapter
        Dim ds As New DataSet()
        Dim i As Integer = 0
        sqldata = Nothing

        sqldata = "select a.itmno,a.codeno,b.mmdesc,ifnull(a.qty,0) as qty,ifnull(a.wt,0) as wt from pmstocktbl a " &
                  "left join mmasttbl b a.codeno=b.codeno on where a.dsrno = '" & txtDSRNo.Text & "' " &
                  "and a.smnno = '" & cboSmnName.Text.Substring(0, 3) & "' order by a.itmno"

        conn.Open()
        Dim command As New MySqlCommand(sqldata, conn)
        adapter.SelectCommand = command
        adapter.Fill(ds)
        adapter.Dispose()
        command.Dispose()
        conn.Close()

        dgvPMstock.DataSource = ds.Tables(0)
        dgvPMstock.DataBind()

    End Sub

    Private Sub ListDOsmn()

        dt = GetDataTable("select dono from isshdrtbl where dsrno = '" & txtDSRNo.Text & "' and " &
                          "smnno = '" & cboSmnName.Text.Substring(0, 3) & "' and mov = '603' limit 1")
        If Not CBool(dt.Rows.Count) Then
            dgvDOdsr.DataSource = Nothing
            dgvDOdsr.DataBind()
            Exit Sub 'Call MessageBox.Show("No DO Selected yet.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning) :
        End If

        Call dt.Dispose()

        Dim adapter As New MySqlDataAdapter
        Dim ds As New DataSet()
        Dim i As Integer = 0
        sqldata = Nothing

        sqldata = "select dono,transdate,totqty,totwt,plntno from isshdrtbl where dsrno = '" & txtDSRNo.Text & "' " &
                  "and smnno = '" & cboSmnName.Text.Substring(0, 3) & "' and mov = '603'"

        conn.Open()
        Dim command As New MySqlCommand(sqldata, conn)
        adapter.SelectCommand = command
        adapter.Fill(ds)
        adapter.Dispose()
        command.Dispose()
        conn.Close()

        dgvDOdsr.DataSource = ds.Tables(0)
        dgvDOdsr.DataBind()

    End Sub

    Private Sub fillWRRsum()
        dt = GetDataTable("select wrrno from wrrhdrtbl where dsrno = '" & txtDSRNo.Text & "' and mov = '" & "409" & "'")
        If Not CBool(dt.Rows.Count) Then
            'lblMsg.Text = "Error: No Inventory Found"
            dgvWRRhdr.DataSource = Nothing
            dgvWRRhdr.DataBind()
            Exit Sub

        End If

        dt.Dispose()

        Dim adapter As New MySqlDataAdapter
        Dim ds As New DataSet()
        Dim i As Integer = 0
        sqldata = Nothing

        sqldata = "select wrrno,transdate,totqty,totwt from wrrhdrtbl " &
                  "where dsrno = '" & txtDSRNo.Text & "' and mov = '" & "409" & "'"

        conn.Open()
        Dim command As New MySqlCommand(sqldata, conn)
        adapter.SelectCommand = command
        adapter.Fill(ds)
        adapter.Dispose()
        command.Dispose()
        conn.Close()
        dgvWRRhdr.DataSource = ds.Tables(0)
        dgvWRRhdr.DataBind()

    End Sub

    Private Sub popWRRNo() '

        Call cboWRRNo.Items.Clear()
        cboWRRNo.Items.Add("<Clear>")
        dt = GetDataTable("select wrrno from wrrhdrtbl where smnno = '" & cboSmnName.Text.Substring(0, 3) & "' and " &
                          "custno = '" & lblCustNo.Text & "' and dsrno is null " &
                          "and mov = '" & "409" & "' order by wrrno")
        If Not CBool(dt.Rows.Count) Then
            Exit Sub

        Else
            cboWRRNo.Items.Add("")
            For Each dr As DataRow In dt.Rows
                cboWRRNo.Items.Add(dr.Item(0).ToString())

            Next

        End If


        Call dt.Dispose()

    End Sub

    Private Sub popDONo()

        Call cboDONo.Items.Clear()
        cboDONo.Items.Add("<Clear>")
        dt = GetDataTable("select dono from isshdrtbl where smnno = '" & cboSmnName.Text.Substring(0, 3) & "' and " &
                          "custno = '" & lblCustNo.Text & "' and dsrno = '" & "00000" & "' " &
                          "and mov = '603' and status <> 'void' order by dono")
        If Not CBool(dt.Rows.Count) Then
            Exit Sub

        Else
            cboDONo.Items.Add("")
            For Each dr As DataRow In dt.Rows
                cboDONo.Items.Add(dr.Item(0).ToString())

            Next

        End If

        Call dt.Dispose()

    End Sub

    Private Sub RRsalesAmt()

        Dim RRamt As Double

        dt = GetDataTable("select smnno,dsrno,ifnull(tc84amt,0),transdate,tc85amt,status from dsrnotbl " &
                          "where smnno = '" & cboSmnName.Text.Substring(0, 3) & "' and status = '" & "OPEN" & "' " &
                          "and dsrno = '" & Trim(txtDSRNo.Text) & "'")
        If Not CBool(dt.Rows.Count) Then
            'Call MessageBox.Show("RR Amount not yet Checked", "DSR System", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub

        Else

            For Each dr As DataRow In dt.Rows
                If dr.Item(5).ToString() = "OPEN" Then
                    lbSave.Enabled = True

                Else
                    lbSave.Enabled = False

                End If

                lblRRtot84.Text = Format(dr.Item(2), "##,##0.00".ToString())
                lblTot84.Text = Format(dr.Item(2), "##,##0.00".ToString())
                RRamt = CDbl(dr.Item(2).ToString())
                txtRRAmt.Text = Format(RRamt, "#,##0.00")
                dpTransDate.Text = dr.Item(3).ToString()
                lblTot85.Text = Format(dr.Item(4), "##,##0.00".ToString())
                cboStat.Text = dr.Item(5).ToString()

            Next

            RRamt = CDbl(IIf(txtRRAmt.Text = "", 0, txtRRAmt.Text))

            If RRamt <> CDbl(0) Then
                tssDocStat.Text = "Saved"

            Else
                tssDocStat.Text = "Not Yet Saved"

            End If

            Dim TSales As Double = CDbl(IIf(lblTot84.Text = "", 0, lblTot84.Text)) + CDbl(IIf(lblTot85.Text = "", 0, lblTot85.Text))
            lblTotalSales.Text = Format(TSales, "#,##0.00")

            Call UpdateStocks()

        End If

        Call dt.Dispose()

    End Sub

    Private Sub UpdateStocks()

        sql = "delete from tempdsrstck where user = '" & lblUser.Text & "'"
        ExecuteNonQuery(sql)

        'do
        sql = "insert into tempdsrstck(codeno,mmdesc,wt,qty,user) " &
              "select a.codeno,b.mmdesc,ifnull(sum(a.wt),0),ifnull(sum(qty),0),'" & lblUser.Text & "' " &
              "from issdettbl a, mmasttbl b, isshdrtbl c where a.codeno=b.codeno and c.dono=a.dono and " &
              "c.dsrno = '" & txtDSRNo.Text & "' and c.smnno = '" & cboSmnName.Text.Substring(0, 3) & "' and " &
              "c.mov between '" & "603" & "' and '" & "604" & "' group by a.codeno"
        ExecuteNonQuery(sql)

        '==== am stock
        sql = "insert into tempdsrstck(codeno,mmdesc,wt,qty,user) " &
              "select a.codeno,b.mmdesc,ifnull(sum(a.wt),0),ifnull(sum(a.qty),0),'" & lblUser.Text & "' from " &
              "amstocktbl a, mmasttbl b where a.codeno=b.codeno and " &
              "a.smnno = '" & cboSmnName.Text.Substring(0, 3) & "' group by a.codeno"
        ExecuteNonQuery(sql)

        'updating available stock

        '=== sales 
        sql = "insert into tempdsrstck(codeno,mmdesc,wt,qty,user) " &
              "select a.codeno,b.mmdesc,ifnull(sum(a.wt),0)*-1,ifnull(sum(a.qty),0)*-1,'" & lblUser.Text & "' " &
              "from salesdettbl a, mmasttbl b where a.codeno=b.codeno and a.dsrno = '" & txtDSRNo.Text & "' group by codeno"
        ExecuteNonQuery(sql)

        '=== wrr 
        sql = "insert into tempdsrstck(codeno,mmdesc,wt,qty,user) " &
              "select a.codeno,b.mmdesc,ifnull(sum(a.wt),0)*-1,ifnull(sum(a.qty),0)*-1,'" & lblUser.Text & "' " &
              "from wrrdettbl a, mmasttbl b,wrrhdrtbl c where a.codeno=b.codeno and c.wrrno=a.wrrno and " &
              "a.dsrno = '" & txtDSRNo.Text & "' and c.smnno = '" & cboSmnName.Text.Substring(0, 3) & "' and a.mov " &
              "between '" & "409" & "' and '" & "410" & "' and c.tc = '" & "88" & "' group by codeno"
        ExecuteNonQuery(sql)

        '=== pmstock


        '=== get available
        sql = "delete from tempdsreport where user = '" & lblUser.Text & "'"
        ExecuteNonQuery(sql)

        sql = "insert into tempdsreport(codeno,description,availablestck,qty,user) " &
              "select codeno,mmdesc,ifnull(sum(wt),0),ifnull(sum(qty),0),'" & lblUser.Text & "' from " &
              "tempdsrstck where user = '" & lblUser.Text & "' group by codeno"
        ExecuteNonQuery(sql)


    End Sub

    Protected Sub lbNew_Click(sender As Object, e As EventArgs)

    End Sub

    Protected Sub lbClose_Click(sender As Object, e As EventArgs)
        Response.Redirect("FinancialAccounting.aspx")

    End Sub

    Protected Sub DgvDOdet_RowDataBound(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Protected Sub DgvDOdet_RowCreated(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Protected Sub DgvSalesdet_RowDataBound(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Protected Sub DgvSalesdet_RowCreated(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Protected Sub dgvWRR_RowDataBound(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Protected Sub dgvWRR_RowCreated(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Protected Sub dgvTC8485_RowDataBound(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Protected Sub dgvTC8485_RowCreated(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Protected Sub dgvDOdsr_RowDataBound(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Protected Sub dgvDOdsr_RowCreated(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Protected Sub dgvWRRhdr_RowDataBound(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Protected Sub dgvWRRhdr_RowCreated(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Protected Sub dgvPMstock_RowDataBound(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Protected Sub dgvPMstock_RowCreated(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Protected Sub dgvShrink_RowDataBound(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Protected Sub dgvShrink_RowCreated(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Protected Sub dgvDSRlist_RowDataBound(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Protected Sub dgvDSRlist_RowCreated(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Protected Sub dgvExcep_RowDataBound(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Protected Sub dgvExcep_RowCreated(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Private Sub getMMstocks()

        cboMMdesc.Items.Clear()

        dt = GetDataTable("select description from tempdsreport where user = '" & lblUser.Text & "'")
        If Not CBool(dt.Rows.Count) Then
            Exit Sub

        Else
            cboMMdesc.Items.Add("")
            For Each dr As DataRow In dt.Rows
                cboMMdesc.Items.Add(dr.Item(0).ToString())

            Next

        End If

        Call dt.Dispose()

    End Sub

    Private Sub cboSmnName_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboSmnName.SelectedIndexChanged
        'txtSmnNo.Text = ""
        lblCustNo.Text = "00000"
        lblAreaNo.Text = "000"
        txtDSRNo.Text = ""
        'tssDocNo.Text = "00000000"
        'tssDSRstat.Text = "New"
        txtRRAmt.Text = ""

        dt = GetDataTable("select custno,areano from smnmtbl where smnno = '" & cboSmnName.Text.Substring(0, 3) & "'")
        If Not CBool(dt.Rows.Count) Then
            'mdiMain.tsErrMsg.Text = "Salesman Not found."
            Exit Sub

        Else
            For Each dr As DataRow In dt.Rows
                lblCustNo.Text = dr.Item(0).ToString()
                lblAreaNo.Text = dr.Item(1).ToString()
                lblArea.Text = lblAreaNo.Text

            Next

        End If

        Call dt.Dispose()

        lblBAto.Text = vLoggedBussArea
        lblBranchTo.Text = vLoggedBranch

        filldgvAMstocKSmn()

        'check for open DSR Temp Disable
        dt = GetDataTable("select status,dsrno,transdate,ifnull(tc84amt,0) from dsrnotbl where smnno = '" & cboSmnName.Text.Substring(0, 3) & "' " &
                          "and status = 'OPEN' order by transdate desc") ' 
        If Not CBool(dt.Rows.Count) Then
            Exit Sub

        Else
            For Each dr As DataRow In dt.Rows
                'place modal here
                'MsgBox("Salesman No. " & txtSmnNo.Text & " have OPEN DSR No. " & dr.Item(1).ToString())
                txtDSRNo.Text = dr.Item(1).ToString() & ""
                tssDocNo.Text = dr.Item(1).ToString() & ""

                tssDSRstat.Text = "Edit"

                If dr.Item(1).ToString() > 0 Then
                    tssDocStat.Text = "Saved"

                End If

            Next

            lblDSRNo.Text = txtDSRNo.Text

        End If

        If txtDSRNo.Text = "" Then
            Exit Sub

        End If

        cboSalesSmn.Items.Clear()
        cboSalesSmn.Items.Add(cboSmnName.Text)


        btnDisAbleFalse()
        'LoadExistingDSR()
        'popDSRinvoices()
        'fillExcepList()
        'clrExcepRep()
    End Sub

    Private Sub btnDisAbleFalse()
        cmdAddDO.Enabled = True
        'cmdAddItm.Enabled = True
        'cmdAddSI.Enabled = True
        'cmdDetItm.Enabled = True
        cmdRemDO.Enabled = True
        lbSave.Enabled = True
        Button1.Enabled = True
        Button3.Enabled = True


    End Sub

    Private Sub filldgvAMstocKSmn()
        dt = GetDataTable("select * from amstocktbl where smnno = '" & cboSmnName.Text.Substring(0, 3) & "'")
        If Not CBool(dt.Rows.Count) Then
            dgvAMstock.DataSource = Nothing
            dgvAMstock.DataBind()
            Exit Sub

        End If


        Dim adapter As New MySqlDataAdapter
        Dim ds As New DataSet()
        Dim i As Integer = 0
        sqldata = Nothing

        sqldata = "select a.codeno,b.mmdesc,a.qty,a.wt from amstocktbl a left join mmasttbl b on a.codeno = b.codeno " &
                  "where a.smnno = '" & cboSmnName.Text.Substring(0, 3) & "'"

        conn.Open()
        Dim command As New MySqlCommand(sqldata, conn)
        adapter.SelectCommand = command
        adapter.Fill(ds)
        adapter.Dispose()
        command.Dispose()
        conn.Close()
        dgvAMstock.DataSource = ds.Tables(0)
        dgvAMstock.DataBind()

        With dgvAMstock
            .Columns(0).HeaderText = "Code No."
            .Columns(1).HeaderText = "Description"
            .Columns(2).HeaderText = "Qty"
            .Columns(3).HeaderText = "Wt"

        End With


    End Sub

    Protected Sub dgvAMstock_RowDataBound(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Protected Sub dgvAMstock_RowCreated(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Protected Sub dgvInvListDSR_RowCreated(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Protected Sub dgvInvListDSR_RowDataBound(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Private Sub getCustListSmn()

        cboCustName.Items.Clear()

        Select Case cboInv.Text.Substring(0, 2)
            Case "84"
                dt = GetDataTable("select concat(custno,space(1),bussname) from custmasttbl where custno = '" & lblCustNo.Text & "'")

            Case "85"
                dt = GetDataTable("select concat(custno,space(1),bussname) from custmasttbl where accttype = 'Main' and " &
                                  "smnno = '" & cboSmnName.Text.Substring(0, 3) & "' and custtype <> 'SMN' order by bussname")
        End Select


        If Not CBool(dt.Rows.Count) Then
            'Call MessageBox.Show("Customer Not found.", "DSR System", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub

        Else

            For Each dr As DataRow In dt.Rows
                cboCustName.Items.Add(dr.Item(0).ToString())

            Next


        End If

        Call dt.Dispose()

        cboShipTo.Items.Clear()

        dt = GetDataTable("select concat(custno,space(1),bussname) from custmasttbl where " &
                          "moacctno = '" & cboCustName.Text.Substring(0, 5) & "' order by bussname")
        If Not CBool(dt.Rows.Count) Then
            'Call MessageBox.Show("Customer Not found.", "DSR System", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub

        Else

            For Each dr As DataRow In dt.Rows
                cboShipTo.Items.Add(dr.Item(0).ToString())

            Next


        End If

        Call dt.Dispose()

    End Sub

    Private Sub cboInv_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboInv.SelectedIndexChanged
        If cboSalesSmn.Text = Nothing Then
            Exit Sub

        ElseIf cboInv.Text = Nothing Then
            Exit Sub

        End If

        getCustListSmn()

    End Sub

    Private Sub cboSalesSmn_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboSalesSmn.SelectedIndexChanged


    End Sub

    Private Sub txtDSRNo_TextChanged(sender As Object, e As EventArgs) Handles txtDSRNo.TextChanged
        If txtDSRNo.Text = Nothing Then
            Exit Sub

        End If

        lblDSRNo.Text = txtDSRNo.Text

    End Sub

    Private Sub cboShipTo_TextChanged(sender As Object, e As EventArgs) Handles cboShipTo.TextChanged
        If cboCustName.Text = Nothing Then
            Exit Sub

        End If

        popShipToAll()

    End Sub

    Private Sub popShipToAll()
        cboShipTo.Items.Clear()

        dt = GetDataTable("select concat(custno,space(1),bussname) from custmasttbl where " &
                          "moacctno = '" & cboCustName.Text.Substring(0, 5) & "' order by bussname")
        If Not CBool(dt.Rows.Count) Then
            'Call MessageBox.Show("Customer Not found.", "DSR System", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub

        Else

            For Each dr As DataRow In dt.Rows
                cboShipTo.Items.Add(dr.Item(0).ToString())

            Next


        End If

        Call dt.Dispose()

    End Sub
End Class