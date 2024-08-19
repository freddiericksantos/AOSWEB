Imports MySql.Data.MySqlClient

Public Class FS_JCS
    Inherits System.Web.UI.Page
    Dim Defaultdate As String
    Dim strTC As String
    Dim strDate As String
    Dim dt As DataTable
    Dim sql As String
    Dim strSales As String
    Dim strMIamt As String
    Dim strTolSales As Double
    Dim strDisc As String
    Dim admRet As String
    Dim admReb As String
    Dim strNetSales As Double
    Dim strCOS As String
    Dim strCOSrmInv As String
    Dim strGrossProfit As Double
    Dim strSelExp As String
    Dim strAdmExp As String
    Dim strOpexSum As String
    Dim strNetInc As Double
    Dim strProdCC As String
    Dim admRMinv As String
    Dim admRMcos As String
    Dim admFGinv As String
    Dim admFGinv2 As String
    Dim admFGrec As String
    Dim admFGtrans As String
    Dim strCOSpnl As String
    Dim strCOS2Pnl As String
    Dim strFGnchange As String
    Dim strMIsales As Double
    Dim strCCNoAdmin As String
    Dim strCCNoSales As String
    Dim strCCnoProdn As String
    Dim admCCtech As String
    Dim admCCLogis As String
    Dim admCCLogis2 As String
    Dim admCCLogis3 As String
    Dim strDocSource As String
    Dim strTransMon As String
    Dim strTransYear As String
    Dim strBIRmonth As String
    Dim strDateFr As String
    Dim strDateTo As String
    Dim admAmt As Double
    Dim admCCnoexp As String
    Dim admRMusedAmt As Double
    Dim admDLamt As Double
    Dim admOHamt As Double
    Dim admFGrecAmt As Double
    Dim admFGrecAmt2 As Double
    Dim admFGinvbeg As Double
    Dim admFGinvbeg2 As Double
    Dim admFGinvend As Double
    Dim admFGinvend2 As Double
    Dim admRMbeg As Double
    Dim admRMend As Double
    Dim admIOTprodn As Double
    Dim admIOTsales As Double
    Dim admTechExp As Double
    Dim admLogisExp As Double
    Dim MonValue As String
    Dim admSourceRep As String
    Dim admRepFilter As String
    Dim admProdCCno As String
    Dim admCosAmt As Double
    Dim admPriorYr As Double
    Dim admFinalInc As Double
    Dim vDate1 As Date
    Dim vDate2 As Date
    Dim vDateY1 As Date
    Dim admCCNo As String
    Dim MyDA_conn As New MySqlDataAdapter
    Dim MyDataSet As New DataSet
    Dim sqldata As String
    Dim Dramt As Double = 0
    Dim Cramt As Double = 0
    Dim strTotMan As Double
    Dim admTotFG As Double
    Dim admIOTprodnSales As Double
    Dim admRMnetChg As Double
    Dim admRMsold As Double
    Dim packAmt As Double

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
        dt = GetDataTable("select bussname from batbl where ba = '" & vLoggedBussArea & "'")
        If Not CBool(dt.Rows.Count) Then

        Else
            For Each dr As DataRow In dt.Rows
                txtCoName.Text = UCase(dr.Item(0).ToString())
                txtCoName2.Text = UCase(dr.Item(0).ToString())
            Next

        End If

        Call dt.Dispose()

        lblnewTitle.Text = "STATEMENT OF COMPREHENSIVE INCOME"
        lblnewTitle2.Text = "COST OF SALES"

        cboFSmonYear.Items.Clear()
        cboFSmonYear2.Items.Clear()
        cboFSmonYear3.Items.Clear()
        dt = GetDataTable("select distinct transdate from coshdrtbl order by transdate desc")
        If Not CBool(dt.Rows.Count) Then
            MsgBox("No Vendor Found")
            Exit Sub
        Else
            cboFSmonYear.Items.Add("")
            cboFSmonYear2.Items.Add("")
            cboFSmonYear3.Items.Add("")
            For Each dr As DataRow In dt.Rows
                cboFSmonYear.Items.Add(Format(CDate(dr.Item(0).ToString()), "MMM yyyy"))
                cboFSmonYear2.Items.Add(Format(CDate(dr.Item(0).ToString()), "MMM yyyy"))
                cboFSmonYear3.Items.Add(Format(CDate(dr.Item(0).ToString()), "MMM yyyy"))
            Next

        End If

        Call dt.Dispose()

    End Sub

    Protected Sub lbClose_Click(sender As Object, e As EventArgs)
        Response.Redirect("FinancialAccounting.aspx")

    End Sub

    Private Sub cboFSmonYear_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboFSmonYear.SelectedIndexChanged
        If cboFSmonYear.Text = "" Then
            Exit Sub
        End If

        lblFSdate1.Text = Format(CDate(FirstDayOfMonth(cboFSmonYear.Text)), "yyyy-MM-dd")
        lblFSdate2.Text = Format(CDate(LastDayOfMonth(cboFSmonYear.Text)), "yyyy-MM-dd")
        lblFirtDayYr.Text = Format(CDate(cboFSmonYear.Text), "yyyy") & "-01-01"
        lblTransDate.Text = "For the Month Ended " & Format(CDate(lblFSdate2.Text), "MMMM dd, yyyy")

        GetFS()

    End Sub

    Private Sub GetFS()
        vDate1 = Format(CDate(lblFSdate1.Text), "yyyy-MM-dd")
        vDate2 = Format(CDate(lblFSdate2.Text), "yyyy-MM-dd")
        vDateY1 = Format(CDate(lblFirtDayYr.Text), "yyyy-MM-dd")

        dt = GetDataTable("Select ccadmin,ccsales,ccprod,cctech,cclogis from batbl")
        If Not CBool(dt.Rows.Count) Then
            'admMsgBox("GL Configuration not found.")
            Exit Sub

        Else
            For Each dr As DataRow In dt.Rows
                strCCNoAdmin = dr.Item(0).ToString()
                strCCNoSales = dr.Item(1).ToString()
                strCCnoProdn = dr.Item(2).ToString()
                admCCtech = dr.Item(3).ToString()
                admCCLogis = dr.Item(4).ToString()
            Next

            Call dt.Dispose()

        End If

        strSales = CDbl(0)
        strDisc = CDbl(0)
        admRet = CDbl(0)
        admReb = CDbl(0)

        'txtSales

        dt = GetDataTable("select ifnull(sum(balamt),0)*- 1 as balamt from glmaintranstbl " &
                          "where dto between '" & Format(CDate(vDate1), "yyyy-MM-dd") & "' " &
                          "and '" & Format(CDate(vDate2), "yyyy-MM-dd") & "' and " &
                          "acctno = '1401000' and entrytype='Regular' group by acctno")
        If Not CBool(dt.Rows.Count) Then

        Else
            For Each dr As DataRow In dt.Rows
                strSales = CDbl(dr.Item(0).ToString())
                txtSales.Text = Format(CDbl(strSales), "#,##0.00")
            Next
        End If

        Call dt.Dispose()

        'txtDisc

        dt = GetDataTable("select ifnull(sum(balamt),0) As balamt,ccno from glmaintranstbl " &
                          "where ccno = '" & strCCNoSales & "' and dto between '" & Format(CDate(vDate1), "yyyy-MM-dd") & "' " &
                          "and '" & Format(CDate(vDate2), "yyyy-MM-dd") & "' and " &
                          "acctno = '1495000' and entrytype='Regular' group by ccno")
        If Not CBool(dt.Rows.Count) Then

        Else
            For Each dr As DataRow In dt.Rows
                strDisc = CDbl(dr.Item(0).ToString())
                txtDisc.Text = Format(CDbl(strDisc), "#,##0.00 ; (#,##0.00)")

            Next

        End If

        Call dt.Dispose()

        dt = GetDataTable("Select ifnull(sum(balamt),0)as balamt,ccno from glmaintranstbl where ccno = '" & strCCNoSales & "' and " &
                          "dto between '" & Format(CDate(vDate1), "yyyy-MM-dd") & "' and '" & Format(CDate(vDate2), "yyyy-MM-dd") & "' " &
                          "and acctno between '1496000' and '1497000' and entrytype='Regular' group by ccno")
        If Not CBool(dt.Rows.Count) Then

        Else
            For Each dr As DataRow In dt.Rows
                admRet = CDbl(dr.Item(0).ToString())
                txtSRA.Text = Format(CDbl(admRet), "#,##0.00 ; (#,##0.00)")

            Next

        End If

        Call dt.Dispose()

        'rebate txtReb
        dt = GetDataTable("Select ifnull(sum(balamt),0) as balamt,ccno from glmaintranstbl where ccno = '" & strCCNoSales & "' and " &
                          "dto between '" & Format(CDate(vDate1), "yyyy-MM-dd") & "' and '" & Format(CDate(vDate2), "yyyy-MM-dd") & "' " &
                          "and acctno = '1498000' and entrytype='Regular' group by ccno")
        If Not CBool(dt.Rows.Count) Then

        Else
            For Each dr As DataRow In dt.Rows
                admReb = CDbl(dr.Item(0).ToString())
                txtReb.Text = Format(CDbl(admReb), "#,##0.00 ; (#,##0.00)")

            Next
        End If

        Call dt.Dispose()

        'net sales
        strNetSales = CDbl(strSales) - CDbl(strDisc) - CDbl(admRet) - CDbl(admReb)
        txtNetSales.Text = Format(CDbl(strNetSales), "#,##0.00 ; (#,##0.00)")

        'cost of sales
        'txtCOS
        dt = GetDataTable("select ifnull(sum(a.balamt),0) from glmaintranstbl a left join cctrnotbl b on a.ccno=b.ccno " &
                          "where b.ccno2 = '" & strCCnoProdn & "' and a.dto between '" & Format(CDate(vDate1), "yyyy-MM-dd") & "' " &
                          "and '" & Format(CDate(vDate2), "yyyy-MM-dd") & "' and " &
                          "a.maingroup = '2000000' and a.entrytype='Regular' group by b.ccno2")
        If Not CBool(dt.Rows.Count) Then

        Else
            For Each dr As DataRow In dt.Rows
                strCOS = CDbl(dr.Item(0).ToString())
                txtCOS.Text = Format(CDbl(strCOS), "#,##0.00 ; (#,##0.00)")
            Next

        End If

        Call dt.Dispose()

        'txtGProfit
        strGrossProfit = CDbl(strNetSales) - CDbl(strCOS)
        txtGProfit.Text = Format(CDbl(strGrossProfit), "#,##0.00 ; (#,##0.00)")

        'GP
        admAmt = CDbl(strSales) / CDbl(strSales) * 100
        txtSalesGP.Text = Format(CDbl(admAmt), "#,##0.00 ; (#,##0.00)")

        If strDisc > 0 Then
            admAmt = CDbl(strDisc) / CDbl(strSales) * 100
        Else
            admAmt = CDbl(0)
        End If

        txtDiscGP.Text = Format(CDbl(admAmt), "#,##0.00 ; (#,##0.00)")

        If admRet > 0 Then
            admAmt = CDbl(admRet) / CDbl(strSales) * 100
        Else
            admAmt = CDbl(0)
        End If

        txtSRAgp.Text = Format(CDbl(admAmt), "#,##0.00 ; (#,##0.00)")

        If admReb > 0 Then
            admAmt = CDbl(admReb) / CDbl(strSales) * 100
        Else
            admAmt = CDbl(0)
        End If

        txtRebgp.Text = Format(CDbl(admAmt), "#,##0.00 ; (#,##0.00)")


        admAmt = CDbl(strNetSales) / CDbl(strSales) * 100
        txtNetSalesGP.Text = Format(CDbl(admAmt), "#,##0.00 ; (#,##0.00)")

        admAmt = CDbl(strCOS) / CDbl(strSales) * 100
        txtCOSgp.Text = Format(CDbl(admAmt), "#,##0.00 ; (#,##0.00)")

        admAmt = CDbl(strGrossProfit) / CDbl(strSales) * 100
        txtGrossProGP.Text = Format(CDbl(admAmt), "#,##0.00 ; (#,##0.00)")

        'Admin
        dt = GetDataTable("select ifnull(sum(a.balamt),0) as balamt from glmaintranstbl a left join cctrnotbl b on a.ccno=b.ccno where  " &
                          "a.dto between '" & Format(CDate(vDate1), "yyyy-MM-dd") & "' and '" & Format(CDate(vDate2), "yyyy-MM-dd") & "' " &
                          "and a.entrytype='Regular' and b.ccno2 = '" & strCCNoAdmin & "' and (a.motheracct = '1570000' or a.motheracct = '1580000') " &
                          "group by a.maingroup")
        If Not CBool(dt.Rows.Count) Then

        Else
            For Each dr As DataRow In dt.Rows
                strAdmExp = CDbl(dr.Item(0).ToString())
                txtAdmExpsTot.Text = Format(CDbl(strAdmExp), "#,##0.00 ; (#,##0.00)")

            Next
        End If

        Call dt.Dispose()

        'selling exps
        dt = GetDataTable("select ifnull(sum(a.balamt),0) as balamt,a.ccno from glmaintranstbl a left join cctrnotbl b on a.ccno=b.ccno where " &
                          "a.ccno = '" & strCCNoSales & "' and a.motheracct = '1570000' and a.dto between '" & Format(CDate(vDate1), "yyyy-MM-dd") & "' " &
                          "and '" & Format(CDate(vDate2), "yyyy-MM-dd") & "' and a.entrytype='Regular' and " &
                          "a.prioryradjt is null group by a.motheracct")
        If Not CBool(dt.Rows.Count) Then

        Else
            For Each dr As DataRow In dt.Rows
                strSelExp = CDbl(dr.Item(0).ToString())
                txtSelExpTot.Text = Format(CDbl(strSelExp), "#,##0.00 ; (#,##0.00)")

            Next
        End If

        Call dt.Dispose()

        'technical
        dt = GetDataTable("select ifnull(sum(a.balamt),0) as balamt from glmaintranstbl a left join cctrnotbl b on a.ccno=b.ccno where " &
                          "a.ccno = '" & admCCtech & "' and a.motheracct = '1570000' and a.dto between '" & Format(CDate(vDate1), "yyyy-MM-dd") & "' " &
                          "and '" & Format(CDate(vDate2), "yyyy-MM-dd") & "' and a.entrytype='Regular' and " &
                          "a.prioryradjt is null group by  a.motheracct")
        If Not CBool(dt.Rows.Count) Then

        Else
            For Each dr As DataRow In dt.Rows
                admTechExp = CDbl(dr.Item(0).ToString())
                txtTechTot.Text = Format(CDbl(admTechExp), "#,##0.00 ; (#,##0.00)")

            Next
        End If

        Call dt.Dispose()

        'Logistic
        dt = GetDataTable("select ifnull(sum(a.balamt),0) as balamt from glmaintranstbl a left join cctrnotbl b on a.ccno=b.ccno where " &
                          "b.ccno2 = '" & admCCLogis & "' and a.motheracct = '1570000' and a.dto between '" & Format(CDate(vDate1), "yyyy-MM-dd") & "' " &
                          "and '" & Format(CDate(vDate2), "yyyy-MM-dd") & "' and a.entrytype='Regular' and " &
                          "a.prioryradjt is null group by  a.motheracct")
        If Not CBool(dt.Rows.Count) Then

        Else
            For Each dr As DataRow In dt.Rows
                admLogisExp = CDbl(dr.Item(0).ToString())
                txtLogisTot.Text = Format(CDbl(admLogisExp), "#,##0.00 ; (#,##0.00)")

            Next
        End If

        Call dt.Dispose()

        'txtOpex
        strOpexSum = CDbl(strSelExp) + CDbl(strAdmExp) + CDbl(admTechExp) + CDbl(admLogisExp)
        txtOpexTot.Text = Format(strOpexSum, "Standard")

        dt = GetDataTable("select ifnull(sum(balamt),0)*- 1 as balamt from glmaintranstbl " &
                          "where dto between '" & Format(CDate(vDate1), "yyyy-MM-dd") & "' and '" & Format(CDate(vDate2), "yyyy-MM-dd") & "' and " &
                          "upperacct = '1810000' and entrytype='Regular' group by upperacct")
        If Not CBool(dt.Rows.Count) Then

        Else
            For Each dr As DataRow In dt.Rows
                strMIsales = CDbl(dr.Item(0).ToString())
                txtMIothersTot.Text = Format(CDbl(strMIsales), "#,##0.00 ;(#,##0.00)")
            Next
        End If

        Call dt.Dispose()

        'net inc
        'strNetInc = CDbl(strGrossProfit) - CDbl(strSelExp) - CDbl(strAdmExp) + CDbl(strMIsales)
        strNetInc = CDbl(strGrossProfit) - CDbl(strOpexSum) + CDbl(strMIsales)
        txtNetIncTot.Text = Format(CDbl(strNetInc), "#,##0.00 ; (#,##0.00)")

        'txtIncTaxExp    
        'tax exp

        'prior year adjt
        dt = GetDataTable("Select ifnull(sum(a.balamt),0) as balamt from glmaintranstbl a left join cctrnotbl b on a.ccno=b.ccno " &
                          "where a.motheracct between '1570000' and '1580000' and " &
                          "a.dto between '" & Format(CDate(vDate1), "yyyy-MM-dd") & "' " &
                          "and '" & Format(CDate(vDate2), "yyyy-MM-dd") & "' and a.entrytype='Regular' " &
                          "and a.prioryradjt is not null group by a.motheracct")
        If Not CBool(dt.Rows.Count) Then

        Else
            For Each dr As DataRow In dt.Rows
                admPriorYr = CDbl(dr.Item(0).ToString())
                txtAdjt.Text = Format(CDbl(admPriorYr), "#,##0.00 ; (#,##0.00)")

            Next
        End If

        Call dt.Dispose()

        'net inc

        admFinalInc = CDbl(strNetInc) - CDbl(admPriorYr)
        txtNetIncFinal.Text = Format(CDbl(admFinalInc), "#,##0.00 ; (#,##0.00)")

        'GP
        admAmt = CDbl(strSales) / CDbl(strSales) * 100
        txtSalesGP.Text = Format(CDbl(admAmt), "#,##0.00 ; (#,##0.00)")

        admAmt = CDbl(strDisc) / CDbl(strSales) * 100
        txtDiscGP.Text = Format(CDbl(admAmt), "#,##0.00 ; (#,##0.00)")

        'txtSRAtotGP
        admAmt = CDbl(admRet) / CDbl(strSales) * 100
        txtSRAgp.Text = Format(CDbl(admAmt), "#,##0.00 ; (#,##0.00)")

        'txtRebTotGP
        admAmt = CDbl(admReb) / CDbl(strSales) * 100
        txtRebgp.Text = Format(CDbl(admAmt), "#,##0.00 ; (#,##0.00)")

        admAmt = CDbl(strNetSales) / CDbl(strSales) * 100
        txtNetSalesGP.Text = Format(CDbl(admAmt), "#,##0.00 ; (#,##0.00)")

        admAmt = CDbl(strCOS) / CDbl(strSales) * 100
        txtCOSgp.Text = Format(CDbl(admAmt), "#,##0.00 ; (#,##0.00)")

        admAmt = CDbl(strGrossProfit) / CDbl(strSales) * 100
        txtGrossProGP.Text = Format(CDbl(admAmt), "#,##0.00 ; (#,##0.00)")

        admAmt = CDbl(strAdmExp) / CDbl(strSales) * 100
        txtAdmGPtot.Text = Format(CDbl(admAmt), "#,##0.00 ; (#,##0.00)")

        admAmt = CDbl(strSelExp) / CDbl(strSales) * 100
        txtSelGPtot.Text = Format(CDbl(admAmt), "#,##0.00 ; (#,##0.00)")

        admAmt = CDbl(admTechExp) / CDbl(strSales) * 100
        txtTechGPtot.Text = Format(CDbl(admAmt), "#,##0.00 ; (#,##0.00)")

        admAmt = CDbl(admLogisExp) / CDbl(strSales) * 100
        txtLogisGPtot.Text = Format(CDbl(admAmt), "#,##0.00 ; (#,##0.00)")

        admAmt = CDbl(strOpexSum) / CDbl(strSales) * 100
        txtOpExpGLtot.Text = Format(CDbl(admAmt), "#,##0.00 ; (#,##0.00)")

        admAmt = CDbl(strMIsales) / CDbl(strSales) * 100
        txtMIgpTot.Text = Format(CDbl(admAmt), "#,##0.00 ; (#,##0.00)")

        admAmt = CDbl(strNetInc) / CDbl(strSales) * 100
        txtNIncGPtot.Text = Format(CDbl(admAmt), "#,##0.00 ; (#,##0.00)")

        'txtTaxGP
        '
        'txtAdjtGP   admPriorYr
        admAmt = CDbl(admPriorYr) / CDbl(strSales) * 100
        txtAdjtGP.Text = Format(CDbl(admAmt), "#,##0.00 ; (#,##0.00)")

        admAmt = CDbl(admFinalInc) / CDbl(strSales) * 100
        txtFinalIncGP.Text = Format(CDbl(admAmt), "#,##0.00 ; (#,##0.00)")

        '===== YTD PC 1 ======
        'txtSalesDC
        strSales = CDbl(0)
        strDisc = CDbl(0)
        admRet = CDbl(0)
        admReb = CDbl(0)

        dt = GetDataTable("Select ifnull(sum(balamt),0)*- 1 as balamt from glmaintranstbl where dto between '" & Format(CDate(vDateY1), "yyyy-MM-dd") & "' " &
                          "and '" & Format(CDate(vDate2), "yyyy-MM-dd") & "' and acctno = '1401000' and entrytype='Regular' " &
                          "and ccno = '" & strCCNoSales & "' group by acctno")
        If Not CBool(dt.Rows.Count) Then

        Else
            For Each dr As DataRow In dt.Rows
                strSales = CDbl(dr.Item(0).ToString())
                txtSalesYTD.Text = Format(CDbl(strSales), "#,##0.00")
            Next
        End If

        Call dt.Dispose()

        'txtDisc

        dt = GetDataTable("Select ifnull(sum(balamt),0) as balamt from glmaintranstbl where ccno = '" & strCCNoSales & "' and " &
                          "dto between '" & Format(CDate(vDateY1), "yyyy-MM-dd") & "' and '" & Format(CDate(vDate2), "yyyy-MM-dd") & "' and " &
                          "acctno = '1495000' and entrytype='Regular' group by ccno")
        If Not CBool(dt.Rows.Count) Then

        Else
            For Each dr As DataRow In dt.Rows
                strDisc = CDbl(dr.Item(0).ToString())
                txtDiscYTD.Text = Format(CDbl(strDisc), "#,##0.00 ; (#,##0.00)")

            Next
        End If

        Call dt.Dispose()

        'return and allow txtSRA
        dt = GetDataTable("Select ifnull(sum(balamt),0) as balamt from glmaintranstbl where ccno = '" & strCCNoSales & "' and " &
                          "dto between '" & Format(CDate(vDateY1), "yyyy-MM-dd") & "' and '" & Format(CDate(vDate2), "yyyy-MM-dd") & "' " &
                          "and acctno between '1496000' and '1497000' and entrytype='Regular' group by ccno")
        If Not CBool(dt.Rows.Count) Then

        Else
            For Each dr As DataRow In dt.Rows
                admRet = CDbl(dr.Item(0).ToString())
                txtSRAytd.Text = Format(CDbl(admRet), "#,##0.00 ; (#,##0.00)")

            Next

        End If

        Call dt.Dispose()


        'rebate txtReb
        dt = GetDataTable("Select ifnull(sum(balamt),0) as balamt from glmaintranstbl where ccno = '" & strCCNoSales & "' and " &
                          "dto between '" & Format(CDate(vDateY1), "yyyy-MM-dd") & "' and '" & Format(CDate(vDate2), "yyyy-MM-dd") & "' " &
                          "and acctno = '1498000' and entrytype='Regular' group by ccno")
        If Not CBool(dt.Rows.Count) Then

        Else
            For Each dr As DataRow In dt.Rows
                admReb = CDbl(dr.Item(0).ToString())
                txtRebYTD.Text = Format(CDbl(admReb), "#,##0.00 ; (#,##0.00)")

            Next
        End If

        Call dt.Dispose()


        'net sales
        strNetSales = CDbl(strSales) - CDbl(strDisc) - CDbl(admRet) - CDbl(admReb)
        txtNetSalesYTD.Text = Format(CDbl(strNetSales), "#,##0.00 ; (#,##0.00)")

        'cost of sales
        'txtCOSdc
        'Dim strCOSpnl As String
        'Dim strCOS2Pnl As String
        dt = GetDataTable("select ifnull(sum(a.balamt),0) from glmaintranstbl a left join cctrnotbl b on a.ccno=b.ccno " &
                          "where b.ccno2 = '" & strCCnoProdn & "' and a.dto between '" & Format(CDate(vDateY1), "yyyy-MM-dd") & "' " &
                          "and '" & Format(CDate(vDate2), "yyyy-MM-dd") & "' and " &
                          "a.maingroup = '2000000' and a.entrytype='Regular' group by b.ccno2")
        If Not CBool(dt.Rows.Count) Then

        Else
            For Each dr As DataRow In dt.Rows
                strCOS = CDbl(dr.Item(0).ToString())
                txtCOSYTD.Text = Format(CDbl(strCOS), "#,##0.00 ; (#,##0.00)")
            Next
        End If

        'txtGProfit

        Call dt.Dispose()

        strGrossProfit = CDbl(strNetSales) - CDbl(strCOS)
        txtGProfitYTD.Text = Format(CDbl(strGrossProfit), "#,##0.00 ; (#,##0.00)")


        'GP YTD PC1

        admAmt = CDbl(strSales) / CDbl(strSales) * 100
        txtSalesGPytd.Text = Format(CDbl(admAmt), "#,##0.00 ; (#,##0.00)")

        If strDisc > 0 Then
            admAmt = CDbl(strDisc) / CDbl(strSales) * 100
        Else
            admAmt = CDbl(0)
        End If

        txtDiscGPytd.Text = Format(CDbl(admAmt), "#,##0.00 ; (#,##0.00)")

        'ret
        If admRet > 0 Then
            admAmt = CDbl(admRet) / CDbl(strSales) * 100
        Else
            admAmt = CDbl(0)
        End If

        txtSRAytdGP.Text = Format(CDbl(admAmt), "#,##0.00 ; (#,##0.00)")

        'reb
        If admReb > 0 Then
            admAmt = CDbl(admReb) / CDbl(strSales) * 100
        Else
            admAmt = CDbl(0)
        End If

        txtRebYTDgp.Text = Format(CDbl(admAmt), "#,##0.00 ; (#,##0.00)")

        admAmt = CDbl(strNetSales) / CDbl(strSales) * 100
        txtNetSalesGPytd.Text = Format(CDbl(admAmt), "#,##0.00 ; (#,##0.00)")

        admAmt = CDbl(strCOS) / CDbl(strSales) * 100
        txtCOSgpYTD.Text = Format(CDbl(admAmt), "#,##0.00 ; (#,##0.00)")

        admAmt = CDbl(strGrossProfit) / CDbl(strSales) * 100
        txtGrossProGPytd.Text = Format(CDbl(admAmt), "#,##0.00 ; (#,##0.00)")


        dt = GetDataTable("select ifnull(sum(a.balamt),0) as balamt from glmaintranstbl a left join cctrnotbl b on a.ccno=b.ccno where  " &
                          "a.dto between '" & Format(CDate(vDateY1), "yyyy-MM-dd") & "' and '" & Format(CDate(vDate2), "yyyy-MM-dd") & "' " &
                          "and a.entrytype='Regular' and b.ccno2 = '" & strCCNoAdmin & "' and (a.motheracct = '1570000' or a.motheracct = '1580000') " &
                          "and a.prioryradjt is null group by a.maingroup")
        If Not CBool(dt.Rows.Count) Then

        Else
            For Each dr As DataRow In dt.Rows
                strAdmExp = CDbl(dr.Item(0).ToString())
                txtAdmExpsYTDtot.Text = Format(CDbl(strAdmExp), "#,##0.00 ; (#,##0.00)")

            Next
        End If

        Call dt.Dispose()

        'selling exps
        dt = GetDataTable("select ifnull(sum(a.balamt),0) as balamt from glmaintranstbl a left join cctrnotbl b on a.ccno=b.ccno where " &
                          "a.ccno = '" & strCCNoSales & "' and a.motheracct = '1570000' and a.dto between '" & Format(CDate(vDateY1), "yyyy-MM-dd") & "' " &
                          "and '" & Format(CDate(vDate2), "yyyy-MM-dd") & "' and a.entrytype='Regular' and " &
                          "a.prioryradjt is null group by a.motheracct")
        If Not CBool(dt.Rows.Count) Then

        Else
            For Each dr As DataRow In dt.Rows
                strSelExp = CDbl(dr.Item(0).ToString())
                txtSelExpYTDtot.Text = Format(CDbl(strSelExp), "#,##0.00 ; (#,##0.00)")

            Next

        End If

        Call dt.Dispose()

        'technical
        dt = GetDataTable("select ifnull(sum(a.balamt),0) as balamt from glmaintranstbl a left join cctrnotbl b on a.ccno=b.ccno where " &
                          "a.ccno = '" & admCCtech & "' and a.motheracct = '1570000' and a.dto between '" & Format(CDate(vDateY1), "yyyy-MM-dd") & "' " &
                          "and '" & Format(CDate(vDate2), "yyyy-MM-dd") & "' and a.entrytype='Regular' and " &
                          "a.prioryradjt is null group by a.motheracct")
        If Not CBool(dt.Rows.Count) Then

        Else
            For Each dr As DataRow In dt.Rows
                admTechExp = CDbl(dr.Item(0).ToString())
                txtTechYTDtot.Text = Format(CDbl(admTechExp), "#,##0.00 ; (#,##0.00)")

            Next
        End If

        Call dt.Dispose()

        'Logistic
        dt = GetDataTable("select ifnull(sum(a.balamt),0) as balamt from glmaintranstbl a left join cctrnotbl b on a.ccno=b.ccno where " &
                          "b.ccno2 = '" & admCCLogis & "' and a.motheracct = '1570000' and a.dto between '" & Format(CDate(vDateY1), "yyyy-MM-dd") & "' " &
                          "and '" & Format(CDate(vDate2), "yyyy-MM-dd") & "' and a.entrytype='Regular' and " &
                          "a.prioryradjt is null group by a.motheracct")
        If Not CBool(dt.Rows.Count) Then

        Else
            For Each dr As DataRow In dt.Rows
                admLogisExp = CDbl(dr.Item(0).ToString())
                txtLogisYTDtot.Text = Format(CDbl(admLogisExp), "#,##0.00 ; (#,##0.00)")

            Next

        End If

        Call dt.Dispose()

        'txtOpex
        strOpexSum = CDbl(strSelExp) + CDbl(strAdmExp) + CDbl(admTechExp) + CDbl(admLogisExp)
        txtOpexYTDtot.Text = Format(strOpexSum, "Standard")

        dt = GetDataTable("Select ifnull(sum(balamt),0)*- 1 as balamt from glmaintranstbl where dto between '" & Format(CDate(vDateY1), "yyyy-MM-dd") & "' " &
                          "and '" & Format(CDate(vDate2), "yyyy-MM-dd") & "' and upperacct = '1810000' and entrytype='Regular' " &
                          " group by ccno")
        If Not CBool(dt.Rows.Count) Then

        Else
            For Each dr As DataRow In dt.Rows
                strMIsales = CDbl(dr.Item(0).ToString())
                txtMIothersYTDtot.Text = Format(CDbl(strMIsales), "#,##0.00 ;(#,##0.00)")
            Next
        End If

        Call dt.Dispose()



        'net inc
        'strNetInc = CDbl(strGrossProfit) - CDbl(strSelExp) - CDbl(strAdmExp) + CDbl(strMIsales)
        strNetInc = CDbl(strGrossProfit) - CDbl(strOpexSum) + CDbl(strMIsales)
        txtNetIncYTDtot.Text = Format(CDbl(strNetInc), "#,##0.00 ; (#,##0.00)")

        'add/deduct prior year adjustment txtAdjtYTD
        dt = GetDataTable("Select ifnull(sum(a.balamt),0) as balamt from glmaintranstbl a left join cctrnotbl b on a.ccno=b.ccno " &
                          "where a.motheracct between '1570000' and '1580000' and " &
                          "a.dto between '" & Format(CDate(vDateY1), "yyyy-MM-dd") & "' " &
                          "and '" & Format(CDate(vDate2), "yyyy-MM-dd") & "' and a.entrytype='Regular' " &
                          "and a.prioryradjt is not null group by a.motheracct")
        If Not CBool(dt.Rows.Count) Then

        Else
            For Each dr As DataRow In dt.Rows
                admPriorYr = CDbl(dr.Item(0).ToString())
                txtAdjtYTD.Text = Format(CDbl(admPriorYr), "#,##0.00 ; (#,##0.00)")

            Next
        End If

        Call dt.Dispose()

        'txtAdjtYTD txtIncTaxExpYTD  
        admFinalInc = CDbl(strNetInc) - CDbl(admPriorYr)
        txtNetIncFinalYTD.Text = Format(CDbl(admFinalInc), "#,##0.00 ; (#,##0.00)")


        'GP

        admAmt = CDbl(strAdmExp) / CDbl(strSales) * 100
        txtAdmGPytdTot.Text = Format(CDbl(admAmt), "#,##0.00 ; (#,##0.00)")

        admAmt = CDbl(strSelExp) / CDbl(strSales) * 100
        txtSelGPytdTot.Text = Format(CDbl(admAmt), "#,##0.00 ; (#,##0.00)")

        admAmt = CDbl(admTechExp) / CDbl(strSales) * 100
        txtTechGPytdTot.Text = Format(CDbl(admAmt), "#,##0.00 ; (#,##0.00)")

        admAmt = CDbl(admLogisExp) / CDbl(strSales) * 100
        txtLogisGPytdTot.Text = Format(CDbl(admAmt), "#,##0.00 ; (#,##0.00)")

        admAmt = CDbl(strOpexSum) / CDbl(strSales) * 100
        txtOpExpGLytdTot.Text = Format(CDbl(admAmt), "#,##0.00 ; (#,##0.00)")

        admAmt = CDbl(strMIsales) / CDbl(strSales) * 100
        txtMIgpYTDtot.Text = Format(CDbl(admAmt), "#,##0.00 ; (#,##0.00)")

        admAmt = CDbl(strNetInc) / CDbl(strSales) * 100
        txtNIncGPytdTot.Text = Format(CDbl(admAmt), "#,##0.00 ; (#,##0.00)")


        'txtTaxGPytd
        'txtAdjtGPytd 

        admAmt = CDbl(admPriorYr) / CDbl(strSales) * 100
        txtAdjtGPytd.Text = Format(CDbl(admAmt), "#,##0.00 ; (#,##0.00)")

        admAmt = CDbl(admFinalInc) / CDbl(strSales) * 100
        txtFinalIncGPytd.Text = Format(CDbl(admAmt), "#,##0.00 ; (#,##0.00)")


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

    Private Sub cboFSmonYear2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboFSmonYear2.SelectedIndexChanged
        If cboFSmonYear2.Text = "" Then
            Exit Sub
        End If

        lblFSdateT21.Text = Format(CDate(FirstDayOfMonth(cboFSmonYear2.Text)), "yyyy-MM-dd")
        lblFSdateT22.Text = Format(CDate(LastDayOfMonth(cboFSmonYear2.Text)), "yyyy-MM-dd")
        lblFirtDayYr2.Text = Format(CDate(cboFSmonYear2.Text), "yyyy") & "-01-01"
        lblTransDate2.Text = "For the Month of " & Format(CDate(lblFSdateT22.Text), "MMMM dd, yyyy")

        lblMsg.Text = "UNDER CONSTRUCTION"
        GetCOS()

    End Sub

    Private Sub GetCOS()
        vDate1 = Format(CDate(lblFSdateT21.Text), "yyyy-MM-dd")
        vDate2 = Format(CDate(lblFSdateT22.Text), "yyyy-MM-dd")
        vDateY1 = Format(CDate(lblFirtDayYr2.Text), "yyyy-MM-dd")

        dt = GetDataTable("Select nonexps from batbl where ba = '" & vLoggedBussArea & "'")
        If Not CBool(dt.Rows.Count) Then
            lblMsg.Text = ("Non-Expense CC Not Found")
            Exit Sub
        Else
            For Each dr As DataRow In dt.Rows
                admCCnoexp = dr.Item(0).ToString()
            Next
        End If

        dt.Dispose()

        dt = GetDataTable("select a.ccno,b.rminv,b.rmcos,b.fginv,b.fgrec,b.fgtrans,a.prodgrp from cctrnotbl a " &
                          "left join plnttbl b on a.ccno=b.ccno and b.prodnplnt ='Yes' " &
                          "where a.ba = '" & vLoggedBussArea & "' and a.cosexp = 'Yes'")
        If Not CBool(dt.Rows.Count) Then
            lblMsg.Text = "GL Configuration not found."
            Exit Sub

        Else
            For Each dr As DataRow In dt.Rows
                strProdCC = dr.Item(0).ToString()
                admRMinv = dr.Item(1).ToString()
                admRMcos = dr.Item(2).ToString()
                admFGinv = dr.Item(3).ToString()
                admFGrec = dr.Item(4).ToString()
                admFGtrans = dr.Item(5).ToString()
                admProdCCno = dr.Item(6).ToString()
            Next

            Call dt.Dispose()

        End If

        dt.Dispose()

        admRMusedAmt = CDbl(0)
        dt = GetDataTable("select ifnull(sum(balamt),0) from glmaintranstbl where ccno = '" & admCCnoexp & "' " &
                          "and dto between '" & Format(CDate(vDate1), "yyyy-MM-dd") & "' " &
                          "and '" & Format(CDate(vDate2), "yyyy-MM-dd") & "' and " &
                          "acctno = '" & admRMinv & "' and cos = 'Y' and pk <> '88' group by acctno") '
        If Not CBool(dt.Rows.Count) Then

        Else
            For Each dr As DataRow In dt.Rows
                admRMusedAmt = CDbl(dr.Item(0).ToString())  'f/g used as rm
            Next
        End If

        Call dt.Dispose()

        txtFGusedAsRMtot.Text = Format(CDbl(admRMusedAmt), "#,##0.00 ; (#,##0.00)")

        'RM beg Inv 
        admRMbeg = CDbl(0)
        dt = GetDataTable("select ifnull(sum(balamt),0) from glmaintranstbl where dfrom < '" & Format(CDate(vDate1), "yyyy-MM-dd") & "' " &
                          "and acctno = '" & admRMinv & "' and entrytype = 'Regular' group by acctno")
        If Not CBool(dt.Rows.Count) Then

        Else
            For Each dr As DataRow In dt.Rows
                admRMbeg = CDbl(dr.Item(0).ToString()) 'f/g used as rm

            Next
        End If

        dt.Dispose()

        txtRMbegTot.Text = Format(CDbl(admRMbeg), "#,##0.00 ; (#,##0.00)")

        'RM ending Inv
        admRMend = CDbl(0)
        dt = GetDataTable("select sum(ifnull(balamt,0)) from glmaintranstbl where dto <= '" & Format(CDate(vDate2), "yyyy-MM-dd") & "' " &
                          "and acctno = '" & admRMinv & "' and entrytype = 'Regular' group by acctno")
        If Not CBool(dt.Rows.Count) Then

        Else
            For Each dr As DataRow In dt.Rows
                admRMend = CDbl(dr.Item(0).ToString()) 'f/g used as rm
            Next
        End If

        dt.Dispose()

        txtRMendTot.Text = Format(CDbl(admRMend), "#,##0.00 ; (#,##0.00)")

        admIOTprodn = CDbl(0)

        admIOTprodnSales = CDbl(0)
        admRMsold = CDbl(0)

        dt = GetDataTable("select ifnull(sum(balamt),0) from glmaintranstbl where dto between '" & Format(CDate(vDate1), "yyyy-MM-dd") & "' " &
                          "and '" & Format(CDate(vDate2), "yyyy-MM-dd") & "' and " &
                          "acctno = '" & admRMinv & "' and entrytype = 'Regular' and pk = '88' group by acctno")
        If Not CBool(dt.Rows.Count) Then

        Else
            For Each dr As DataRow In dt.Rows
                admIOTprodnSales = CDbl(dr.Item(0).ToString()) * -1
                admRMsold = CDbl(dr.Item(0).ToString()) * -1
            Next

        End If

        dt.Dispose()

        txtIOTprodn.Text = Format(CDbl(admIOTprodnSales), "#,##0.00 ; (#,##0.00)")


        admRMusedAmt = CDbl(0)
        dt = GetDataTable("select ifnull(sum(balamt),0) from glmaintranstbl where ccno = '" & strProdCC & "' " &
                          "and dto between '" & Format(CDate(vDate1), "yyyy-MM-dd") & "' " &
                          "and '" & Format(CDate(vDate2), "yyyy-MM-dd") & "' and " &
                          "acctno = '" & admRMcos & "' and entrytype = 'Regular' group by acctno")
        If Not CBool(dt.Rows.Count) Then

        Else
            For Each dr As DataRow In dt.Rows
                admRMusedAmt = CDbl(dr.Item(0).ToString()) '- CDbl(admIOTprodn)  '- f/g used as rm

            Next
        End If

        Call dt.Dispose()

        txtRMusedTot.Text = Format(CDbl(admRMusedAmt), "#,##0.00 ; (#,##0.00)")

        'balancing figure
        admDLamt = CDbl(0)
        dt = GetDataTable("select ifnull(sum(a.balamt),0) from glmaintranstbl a left join cctrnotbl b on a.ccno=b.ccno " &
                          "where b.prodgrp = '" & admProdCCno & "' and a.dto between '" & Format(CDate(vDate1), "yyyy-MM-dd") & "' " &
                          "and '" & Format(CDate(vDate2), "yyyy-MM-dd") & "' and " &
                          "a.acctno = '2560000' and a.entrytype = 'Regular' group by a.acctno")
        If Not CBool(dt.Rows.Count) Then

        Else
            For Each dr As DataRow In dt.Rows
                admDLamt = CDbl(dr.Item(0).ToString()) 'DL

            Next

        End If

        Call dt.Dispose()

        txtDLtot.Text = Format(CDbl(admDLamt), "#,##0.00 ; (#,##0.00)")

        admOHamt = CDbl(0)
        dt = GetDataTable("select ifnull(sum(a.balamt),0) from glmaintranstbl a left join cctrnotbl b on a.ccno=b.ccno " &
                          "where b.prodgrp = '" & admProdCCno & "' and a.dto between '" & Format(CDate(vDate1), "yyyy-MM-dd") & "' " &
                          "and '" & Format(CDate(vDate2), "yyyy-MM-dd") & "' and " &
                          "a.motheracct = '2560000' and a.entrytype = 'Regular' group by a.motheracct")
        If Not CBool(dt.Rows.Count) Then

        Else
            For Each dr As DataRow In dt.Rows
                admOHamt = CDbl(dr.Item(0).ToString()) 'foh

            Next
        End If

        Call dt.Dispose()

        txtOHtot.Text = Format(CDbl(admOHamt), "#,##0.00 ; (#,##0.00)")

        strTotMan = CDbl(admRMusedAmt) + CDbl(admDLamt) + CDbl(admOHamt)
        txtTotManufTot.Text = Format(CDbl(strTotMan), "#,##0.00 ; (#,##0.00)")

        'fg rec
        admFGrecAmt2 = CDbl(0)
        admFGrecAmt = CDbl(0)

        admTotFG = CDbl(0)
        admFGrecAmt = CDbl(0)
        dt = GetDataTable("select ifnull(sum(balamt),0) from glmaintranstbl where " &
                          "dto between '" & Format(CDate(vDate1), "yyyy-MM-dd") & "' " &
                          "and '" & Format(CDate(vDate2), "yyyy-MM-dd") & "' and " &
                          "acctno = '" & admFGinv & "' and cos = 'Y' group by acctno")
        If Not CBool(dt.Rows.Count) Then

        Else
            For Each dr As DataRow In dt.Rows
                admFGrecAmt = CDbl(dr.Item(0).ToString()) '+ CDbl(admFGrecAmt2)

            Next

        End If

        Call dt.Dispose()

        admTotFG = admFGrecAmt
        txtFGnchangeTot.Text = Format(CDbl(admTotFG), "#,##0.00 ; (#,##0.00)")

        admFGinvbeg = CDbl(0)
        dt = GetDataTable("select sum(ifnull(balamt,0)) from glmaintranstbl where ccno = '" & admCCnoexp & "' " &
                          "and dfrom < '" & Format(CDate(vDate1), "yyyy-MM-dd") & "' " &
                          "and acctno = '" & admFGinv & "' group by acctno")
        If Not CBool(dt.Rows.Count) Then

        Else
            For Each dr As DataRow In dt.Rows
                admFGinvbeg = CDbl(dr.Item(0).ToString()) + admFGinvbeg2
                txtFGbegtot.Text = Format(CDbl(admFGinvbeg), "#,##0.00 ; (#,##0.00)")

            Next

        End If

        dt.Dispose()


        admFGinvend = CDbl(0)
        dt = GetDataTable("select sum(ifnull(balamt,0)) from glmaintranstbl where ccno = '" & admCCnoexp & "' " &
                          "and dto <= '" & Format(CDate(vDate2), "yyyy-MM-dd") & "' " &
                          "and acctno = '" & admFGinv & "' group by acctno")
        If Not CBool(dt.Rows.Count) Then

        Else
            For Each dr As DataRow In dt.Rows
                admFGinvend = CDbl(dr.Item(0).ToString())
                txtFGendTot.Text = Format(CDbl(admFGinvend), "#,##0.00 ; (#,##0.00)")

            Next

        End If

        dt.Dispose()

        admAmt = CDbl(0)
        admAmt = CDbl(admFGinvbeg) - CDbl(admFGinvend)
        txtFGnetChangeTot.Text = Format(CDbl(admAmt), "#,##0.00 ; (#,##0.00)")

        admIOTprodn = CDbl(0)
        dt = GetDataTable("select ifnull(sum(balamt),0) * -1 from glmaintranstbl where ccno = '" & strProdCC & "' " &
                          "and dto between '" & Format(CDate(vDate1), "yyyy-MM-dd") & "' " &
                          "and '" & Format(CDate(vDate2), "yyyy-MM-dd") & "' " &
                          "and acctno = '2512100' and entrytype = 'Regular' group by acctno")
        If Not CBool(dt.Rows.Count) Then

        Else
            For Each dr As DataRow In dt.Rows
                admIOTprodn = CDbl(dr.Item(0).ToString()) 'f/g used as rm
                txtRMcosFinalTot.Text = Format(CDbl(admIOTprodn), "#,##0.00 ; (#,##0.00)")

            Next

        End If

        dt.Dispose()

        packAmt = CDbl(0)
        dt = GetDataTable("select ifnull(sum(balamt),0) from glmaintranstbl where ccno = '" & admCCnoexp & "' " &
                          "and dto between '" & Format(CDate(vDate1), "yyyy-MM-dd") & "' " &
                          "and '" & Format(CDate(vDate2), "yyyy-MM-dd") & "' and " &
                          "acctno = '1131130' and pk = '88' group by acctno")
        If Not CBool(dt.Rows.Count) Then

        Else
            For Each dr As DataRow In dt.Rows
                packAmt = CDbl(dr.Item(0).ToString()) * -1

            Next

        End If

        dt.Dispose()

        txtRMcosSold.Text = Format(CDbl(admRMsold) + CDbl(packAmt), "#,##0.00 ; (#,##0.00)")

        admCosAmt = CDbl(0)
        admCosAmt = CDbl(admAmt) + CDbl(strTotMan) + CDbl(admTotFG) - CDbl(admIOTprodn) + CDbl(admRMsold) + CDbl(packAmt)
        txtNewCOStot.Text = Format(CDbl(admCosAmt), "#,##0.00 ; (#,##0.00)")

        '###### YTD ######

        admRMusedAmt = CDbl(0)
        dt = GetDataTable("select ifnull(sum(balamt),0) from glmaintranstbl where ccno = '" & admCCnoexp & "' " &
                          "and dto between '" & Format(CDate(vDateY1), "yyyy-MM-dd") & "' " &
                          "and '" & Format(CDate(vDate2), "yyyy-MM-dd") & "' and " &
                          "acctno = '" & admRMinv & "' and cos = 'Y' and pk <> '88' group by acctno") '
        If Not CBool(dt.Rows.Count) Then

        Else
            For Each dr As DataRow In dt.Rows
                admRMusedAmt = CDbl(dr.Item(0).ToString())  'f/g used as rm
            Next
        End If

        Call dt.Dispose()

        txtFGusedAsRMytd.Text = Format(CDbl(admRMusedAmt), "#,##0.00 ; (#,##0.00)")

        'RM beg Inv 
        admRMbeg = CDbl(0)
        dt = GetDataTable("select ifnull(sum(balamt),0) from glmaintranstbl where dfrom < '" & Format(CDate(vDateY1), "yyyy-MM-dd") & "' " &
                          "and acctno = '" & admRMinv & "' and entrytype = 'Regular' group by acctno")
        If Not CBool(dt.Rows.Count) Then

        Else
            For Each dr As DataRow In dt.Rows
                admRMbeg = CDbl(dr.Item(0).ToString()) 'f/g used as rm

            Next
        End If

        dt.Dispose()

        txtRMbegYtd.Text = Format(CDbl(admRMbeg), "#,##0.00 ; (#,##0.00)")

        'RM ending Inv
        admRMend = CDbl(0)
        dt = GetDataTable("select sum(ifnull(balamt,0)) from glmaintranstbl where dto <= '" & Format(CDate(vDate2), "yyyy-MM-dd") & "' " &
                          "and acctno = '" & admRMinv & "' and entrytype = 'Regular' group by acctno")
        If Not CBool(dt.Rows.Count) Then

        Else
            For Each dr As DataRow In dt.Rows
                admRMend = CDbl(dr.Item(0).ToString()) 'f/g used as rm
            Next
        End If

        dt.Dispose()

        txtRMendYtd.Text = Format(CDbl(admRMend), "#,##0.00 ; (#,##0.00)")

        admIOTprodn = CDbl(0)

        admIOTprodnSales = CDbl(0)
        admRMsold = CDbl(0)

        dt = GetDataTable("select ifnull(sum(balamt),0) from glmaintranstbl where dto between '" & Format(CDate(vDateY1), "yyyy-MM-dd") & "' " &
                          "and '" & Format(CDate(vDate2), "yyyy-MM-dd") & "' and " &
                          "acctno = '" & admRMinv & "' and entrytype = 'Regular' and pk = '88' group by acctno")
        If Not CBool(dt.Rows.Count) Then

        Else
            For Each dr As DataRow In dt.Rows
                admIOTprodnSales = CDbl(dr.Item(0).ToString()) * -1
                admRMsold = CDbl(dr.Item(0).ToString()) * -1
            Next

        End If

        dt.Dispose()

        txtIOTprodnYTD.Text = Format(CDbl(admIOTprodnSales), "#,##0.00 ; (#,##0.00)")


        admRMusedAmt = CDbl(0)
        dt = GetDataTable("select ifnull(sum(balamt),0) from glmaintranstbl where ccno = '" & strProdCC & "' " &
                          "and dto between '" & Format(CDate(vDateY1), "yyyy-MM-dd") & "' " &
                          "and '" & Format(CDate(vDate2), "yyyy-MM-dd") & "' and " &
                          "acctno = '" & admRMcos & "' and entrytype = 'Regular' group by acctno")
        If Not CBool(dt.Rows.Count) Then

        Else
            For Each dr As DataRow In dt.Rows
                admRMusedAmt = CDbl(dr.Item(0).ToString()) '- CDbl(admIOTprodn)  '- f/g used as rm

            Next
        End If

        Call dt.Dispose()

        txtRMusedYtd.Text = Format(CDbl(admRMusedAmt), "#,##0.00 ; (#,##0.00)")

        'balancing figure
        admDLamt = CDbl(0)
        dt = GetDataTable("select ifnull(sum(a.balamt),0) from glmaintranstbl a left join cctrnotbl b on a.ccno=b.ccno " &
                          "where b.prodgrp = '" & admProdCCno & "' and a.dto between '" & Format(CDate(vDateY1), "yyyy-MM-dd") & "' " &
                          "and '" & Format(CDate(vDate2), "yyyy-MM-dd") & "' and " &
                          "a.acctno = '2560000' and a.entrytype = 'Regular' group by a.acctno")
        If Not CBool(dt.Rows.Count) Then

        Else
            For Each dr As DataRow In dt.Rows
                admDLamt = CDbl(dr.Item(0).ToString()) 'DL

            Next

        End If

        Call dt.Dispose()

        txtDLytd.Text = Format(CDbl(admDLamt), "#,##0.00 ; (#,##0.00)")

        admOHamt = CDbl(0)
        dt = GetDataTable("select ifnull(sum(a.balamt),0) from glmaintranstbl a left join cctrnotbl b on a.ccno=b.ccno " &
                          "where b.prodgrp = '" & admProdCCno & "' and a.dto between '" & Format(CDate(vDateY1), "yyyy-MM-dd") & "' " &
                          "and '" & Format(CDate(vDate2), "yyyy-MM-dd") & "' and " &
                          "a.motheracct = '2560000' and a.entrytype = 'Regular' group by a.motheracct")
        If Not CBool(dt.Rows.Count) Then

        Else
            For Each dr As DataRow In dt.Rows
                admOHamt = CDbl(dr.Item(0).ToString()) 'foh

            Next
        End If

        Call dt.Dispose()

        txtOHytd.Text = Format(CDbl(admOHamt), "#,##0.00 ; (#,##0.00)")

        strTotMan = CDbl(admRMusedAmt) + CDbl(admDLamt) + CDbl(admOHamt)
        txtTotManufYtd.Text = Format(CDbl(strTotMan), "#,##0.00 ; (#,##0.00)")

        'fg rec
        admFGrecAmt2 = CDbl(0)
        admFGrecAmt = CDbl(0)

        admTotFG = CDbl(0)
        admFGrecAmt = CDbl(0)
        dt = GetDataTable("select ifnull(sum(balamt),0) from glmaintranstbl where " &
                          "dto between '" & Format(CDate(vDateY1), "yyyy-MM-dd") & "' " &
                          "and '" & Format(CDate(vDate2), "yyyy-MM-dd") & "' and " &
                          "acctno = '" & admFGinv & "' and cos = 'Y' group by acctno")
        If Not CBool(dt.Rows.Count) Then

        Else
            For Each dr As DataRow In dt.Rows
                admFGrecAmt = CDbl(dr.Item(0).ToString()) '+ CDbl(admFGrecAmt2)

            Next

        End If

        Call dt.Dispose()

        admTotFG = admFGrecAmt
        txtFGnchangeYtd.Text = Format(CDbl(admTotFG), "#,##0.00 ; (#,##0.00)")

        admFGinvbeg = CDbl(0)
        dt = GetDataTable("select sum(ifnull(balamt,0)) from glmaintranstbl where ccno = '" & admCCnoexp & "' " &
                          "and dfrom < '" & Format(CDate(vDateY1), "yyyy-MM-dd") & "' " &
                          "and acctno = '" & admFGinv & "' group by acctno")
        If Not CBool(dt.Rows.Count) Then

        Else
            For Each dr As DataRow In dt.Rows
                admFGinvbeg = CDbl(dr.Item(0).ToString()) + admFGinvbeg2
                txtFGbegYtd.Text = Format(CDbl(admFGinvbeg), "#,##0.00 ; (#,##0.00)")

            Next

        End If

        dt.Dispose()


        admFGinvend = CDbl(0)
        dt = GetDataTable("select sum(ifnull(balamt,0)) from glmaintranstbl where ccno = '" & admCCnoexp & "' " &
                          "and dto <= '" & Format(CDate(vDate2), "yyyy-MM-dd") & "' " &
                          "and acctno = '" & admFGinv & "' group by acctno")
        If Not CBool(dt.Rows.Count) Then

        Else
            For Each dr As DataRow In dt.Rows
                admFGinvend = CDbl(dr.Item(0).ToString())
                txtFGendYtd.Text = Format(CDbl(admFGinvend), "#,##0.00 ; (#,##0.00)")

            Next

        End If

        dt.Dispose()

        admAmt = CDbl(0)
        admAmt = CDbl(admFGinvbeg) - CDbl(admFGinvend)
        txtFGnetChangeYtd.Text = Format(CDbl(admAmt), "#,##0.00 ; (#,##0.00)")

        admIOTprodn = CDbl(0)
        dt = GetDataTable("select ifnull(sum(balamt),0) * -1 from glmaintranstbl where ccno = '" & strProdCC & "' " &
                          "and dto between '" & Format(CDate(vDateY1), "yyyy-MM-dd") & "' " &
                          "and '" & Format(CDate(vDate2), "yyyy-MM-dd") & "' " &
                          "and acctno = '2512100' and entrytype = 'Regular' group by acctno")
        If Not CBool(dt.Rows.Count) Then

        Else
            For Each dr As DataRow In dt.Rows
                admIOTprodn = CDbl(dr.Item(0).ToString()) 'f/g used as rm
                txtRMcosFinalYtd.Text = Format(CDbl(admIOTprodn), "#,##0.00 ; (#,##0.00)")

            Next

        End If

        dt.Dispose()

        packAmt = CDbl(0)
        dt = GetDataTable("select ifnull(sum(balamt),0) from glmaintranstbl where ccno = '" & admCCnoexp & "' " &
                          "and dto between '" & Format(CDate(vDateY1), "yyyy-MM-dd") & "' " &
                          "and '" & Format(CDate(vDate2), "yyyy-MM-dd") & "' and " &
                          "acctno = '1131130' and pk = '88' group by acctno")
        If Not CBool(dt.Rows.Count) Then

        Else
            For Each dr As DataRow In dt.Rows
                packAmt = CDbl(dr.Item(0).ToString()) * -1

            Next

        End If

        dt.Dispose()

        txtRMcosSoldYTD.Text = Format(CDbl(admRMsold) + CDbl(packAmt), "#,##0.00 ; (#,##0.00)")

        admCosAmt = CDbl(0)
        admCosAmt = CDbl(admAmt) + CDbl(strTotMan) + CDbl(admTotFG) - CDbl(admIOTprodn) + CDbl(admRMsold) + CDbl(packAmt)
        txtNewCOSytd.Text = Format(CDbl(admCosAmt), "#,##0.00 ; (#,##0.00)")

    End Sub

    Private Sub cboFormat_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboFormat.SelectedIndexChanged
        If cboFormat.Text = "" Then
            Exit Sub
        End If

        dgvSched.DataSource = Nothing
        dgvSched.DataBind()

        FillSched()

    End Sub

    Private Sub FillSched()

        vDate1 = Format(CDate(lblFSdateT31.Text), "yyyy-MM-dd")
        vDate2 = Format(CDate(lblFSdateT32.Text), "yyyy-MM-dd")
        vDateY1 = Format(CDate(lblFirtDayYr3.Text), "yyyy-MM-dd")

        dt = GetDataTable("Select ccadmin,ccsales,ccprod,cctech,cclogis from batbl where ba = '" & vLoggedBussArea & "'")
        If Not CBool(dt.Rows.Count) Then
            lblMsg.Text = ("GL Configuration not found.")
            Exit Sub

        Else
            For Each dr As DataRow In dt.Rows
                strCCNoAdmin = dr.Item(0).ToString()
                strCCNoSales = dr.Item(1).ToString()
                strCCnoProdn = dr.Item(2).ToString()
                admCCtech = dr.Item(3).ToString()
                admCCLogis = dr.Item(4).ToString()
            Next

        End If

        Call dt.Dispose()

        sql = "delete from tempexpdettbl where user = '" & vLoggedUserID & "'"
        ExecuteNonQuery(sql)

        Select Case cboFormat.Text
            Case "Admin Expenses"
                sql = "insert into tempexpdettbl(acctno,dramt,user) select a.acctno,sum(ifnull(a.balamt,0)),'" & vLoggedUserID & "' " &
                      "from glmaintranstbl a left join acctcharttbl b on a.acctno=b.acctno left join cctrnotbl c on a.ccno=c.ccno where " &
                      "c.ccno2 = '" & strCCNoAdmin & "' and a.dto between '" & Format(CDate(vDate1), "yyyy-MM-dd") & "' and " &
                      "'" & Format(CDate(vDate2), "yyyy-MM-dd") & "' and a.motheracct between '1570000' and '1580000' and " &
                      "a.prioryradjt is null and a.entrytype = 'Regular' group by a.acctno"
                ExecuteNonQuery(sql)

                sql = "insert into tempexpdettbl(acctno,cramt,user) select a.acctno,sum(ifnull(a.balamt,0)),'" & vLoggedUserID & "' " &
                      "from glmaintranstbl a left join acctcharttbl b on a.acctno=b.acctno left join cctrnotbl c on a.ccno=c.ccno where " &
                      "c.ccno2 = '" & strCCNoAdmin & "' and a.dto between '" & Format(CDate(vDateY1), "yyyy-MM-dd") & "' and " &
                      "'" & Format(CDate(vDate2), "yyyy-MM-dd") & "' and a.motheracct between '1570000' and '1580000' and " &
                      "a.prioryradjt is null and a.entrytype = 'Regular' group by a.acctno"
                ExecuteNonQuery(sql)

            Case "Selling Expenses"
                sql = "insert into tempexpdettbl(acctno,dramt,user) select a.acctno,sum(ifnull(a.balamt,0)),'" & vLoggedUserID & "' " &
                      "from glmaintranstbl a left join acctcharttbl b on a.acctno=b.acctno left join cctrnotbl c on a.ccno=c.ccno " &
                      "where a.ccno = '" & strCCNoSales & "' and a.dto between '" & Format(CDate(vDate1), "yyyy-MM-dd") & "' " &
                      "and '" & Format(CDate(vDate2), "yyyy-MM-dd") & "' and a.motheracct = '1570000' and a.entrytype = 'Regular' and " &
                      "a.prioryradjt is null group by a.acctno"
                ExecuteNonQuery(sql)

                sql = "insert into tempexpdettbl(acctno,cramt,user) select a.acctno,sum(ifnull(a.balamt,0)),'" & vLoggedUserID & "' " &
                      "from glmaintranstbl a left join acctcharttbl b on a.acctno=b.acctno left join cctrnotbl c on a.ccno=c.ccno " &
                      "where a.ccno = '" & strCCNoSales & "' and a.dto between '" & Format(CDate(vDateY1), "yyyy-MM-dd") & "' " &
                      "and '" & Format(CDate(vDate2), "yyyy-MM-dd") & "' and a.motheracct = '1570000' and a.entrytype = 'Regular' and " &
                      "a.prioryradjt is null group by a.acctno"
                ExecuteNonQuery(sql)

            Case "Technical Expenses"
                sql = "insert into tempexpdettbl(acctno,dramt,user) select a.acctno,sum(ifnull(a.balamt,0)),'" & vLoggedUserID & "' " &
                      "from glmaintranstbl a left join acctcharttbl b on a.acctno=b.acctno left join cctrnotbl c on a.ccno=c.ccno " &
                      "where a.ccno = '" & admCCtech & "' and a.dto between '" & Format(CDate(vDate1), "yyyy-MM-dd") & "' " &
                      "and '" & Format(CDate(vDate2), "yyyy-MM-dd") & "' and a.motheracct = '1570000' and a.entrytype = 'Regular' and " &
                      "a.prioryradjt is null group by a.acctno"
                ExecuteNonQuery(sql)

                sql = "insert into tempexpdettbl(acctno,cramt,user) select a.acctno,sum(ifnull(a.balamt,0)),'" & vLoggedUserID & "' " &
                      "from glmaintranstbl a left join acctcharttbl b on a.acctno=b.acctno left join cctrnotbl c on a.ccno=c.ccno " &
                      "where a.ccno = '" & admCCtech & "' and a.dto between '" & Format(CDate(vDateY1), "yyyy-MM-dd") & "' " &
                      "and '" & Format(CDate(vDate2), "yyyy-MM-dd") & "' and a.motheracct = '1570000' and a.entrytype = 'Regular' and " &
                      "a.prioryradjt is null group by a.acctno"
                ExecuteNonQuery(sql)

            Case "Logistic Expenses"
                sql = "insert into tempexpdettbl(acctno,dramt,user) select a.acctno,sum(ifnull(a.balamt,0)),'" & vLoggedUserID & "' " &
                      "from glmaintranstbl a left join acctcharttbl b on a.acctno=b.acctno left join cctrnotbl c on a.ccno=c.ccno " &
                      "where c.ccno2 = '" & admCCLogis & "' and a.dto between '" & Format(CDate(vDate1), "yyyy-MM-dd") & "' " &
                      "and '" & Format(CDate(vDate2), "yyyy-MM-dd") & "' and a.motheracct = '1570000' and a.entrytype = 'Regular' and " &
                      "a.prioryradjt is null group by a.acctno"
                ExecuteNonQuery(sql)

                sql = "insert into tempexpdettbl(acctno,cramt,user) select a.acctno,sum(ifnull(a.balamt,0)),'" & vLoggedUserID & "' " &
                      "from glmaintranstbl a left join acctcharttbl b on a.acctno=b.acctno left join cctrnotbl c on a.ccno=c.ccno " &
                      "where c.ccno2 = '" & admCCLogis & "' and a.dto between '" & Format(CDate(vDateY1), "yyyy-MM-dd") & "' " &
                      "and '" & Format(CDate(vDate2), "yyyy-MM-dd") & "' and a.motheracct = '1570000' and a.entrytype = 'Regular' and " &
                      "a.prioryradjt is null group by a.acctno"
                ExecuteNonQuery(sql)

            Case "Overhead Expenses and Direct Labor"
                sql = "insert into tempexpdettbl(acctno,dramt,user) select a.acctno,sum(ifnull(a.balamt,0)),'" & vLoggedUserID & "' " &
                      "from glmaintranstbl a left join acctcharttbl b on a.acctno=b.acctno left join cctrnotbl c on a.ccno=c.ccno " &
                      "where c.ccno2 = '" & strCCnoProdn & "' and a.dto between '" & Format(CDate(vDate1), "yyyy-MM-dd") & "' " &
                      "and '" & Format(CDate(vDate2), "yyyy-MM-dd") & "' and a.acctno between '2560000' and '2569999' and a.entrytype = 'Regular' " &
                      "and a.prioryradjt is null group by a.acctno"
                ExecuteNonQuery(sql)

                sql = "insert into tempexpdettbl(acctno,cramt,user) select a.acctno,sum(ifnull(a.balamt,0)),'" & vLoggedUserID & "' " &
                      "from glmaintranstbl a left join acctcharttbl b on a.acctno=b.acctno left join cctrnotbl c on a.ccno=c.ccno " &
                      "where c.ccno2 = '" & strCCnoProdn & "' and a.dto between '" & Format(CDate(vDateY1), "yyyy-MM-dd") & "' " &
                      "and '" & Format(CDate(vDate2), "yyyy-MM-dd") & "' and a.acctno between '2560000' and '2569999' and a.entrytype = 'Regular' " &
                      "and a.prioryradjt is null group by a.acctno"
                ExecuteNonQuery(sql)

            Case "Prior Years Adjustment"
                sql = "insert into tempexpdettbl(acctno,dramt,user) select a.acctno,sum(ifnull(a.balamt,0)),'" & vLoggedUserID & "' " &
                      "from glmaintranstbl a left join acctcharttbl b on a.acctno=b.acctno left join cctrnotbl c on a.ccno=c.ccno " &
                      "where a.dto between '" & Format(CDate(vDate1), "yyyy-MM-dd") & "' " &
                      "and '" & Format(CDate(vDate2), "yyyy-MM-dd") & "' and a.entrytype = 'Regular' and " &
                      "a.motheracct between '1570000' and '1580000' and a.prioryradjt is not null group by a.acctno"
                ExecuteNonQuery(sql)

                sql = "insert into tempexpdettbl(acctno,cramt,user) select a.acctno,sum(ifnull(a.balamt,0)),'" & vLoggedUserID & "' " &
                      "from glmaintranstbl a left join acctcharttbl b on a.acctno=b.acctno left join cctrnotbl c on a.ccno=c.ccno " &
                      "where a.dto between '" & Format(CDate(vDateY1), "yyyy-MM-dd") & "' " &
                      "and '" & Format(CDate(vDate2), "yyyy-MM-dd") & "' and a.entrytype = 'Regular' and " &
                      "a.motheracct between '1570000' and '1580000' and a.prioryradjt is not null group by a.acctno"
                ExecuteNonQuery(sql)

        End Select


        dt = GetDataTable("select sum(dramt),sum(cramt) from tempexpdettbl where user = '" & lblUser.Text & "'")
        If Not CBool(dt.Rows.Count) Then
            lblMsg.Text = "Error: " & cboFormat.Text & " Found"
            dgvSched.DataSource = Nothing
            'lblDRamt.Text = "0.00"
            'lblCRamt.Text = "0.00"
            Exit Sub
        Else
            'For Each dr As DataRow In dt.Rows
            '    lblDRamt.Text = Format(CDbl(dr.Item(0).ToString()), "#,##0.00 ; (#,##0.00)")
            '    lblCRamt.Text = Format(CDbl(dr.Item(1).ToString()), "#,##0.00 ; (#,##0.00)")
            'Next

        End If

        dt.Dispose()

        Dim adapter As New MySqlDataAdapter
        Dim ds As New DataSet()
        Dim i As Integer = 0
        sqldata = Nothing

        sqldata = "select concat(a.acctno,space(1),b.acctdesc) as gldesc,sum(ifnull(a.dramt,0)) as dramt,sum(ifnull(a.cramt,0)) as cramt " &
                  "from tempexpdettbl a left join acctcharttbl b on a.acctno=b.acctno where a.user = '" & lblUser.Text & "' group by a.acctno " &
                  "order by dramt desc"

        conn.Open()
        Dim command As New MySqlCommand(sqldata, conn)
        adapter.SelectCommand = command
        adapter.Fill(ds)
        adapter.Dispose()
        command.Dispose()
        conn.Close()
        dgvSched.DataSource = ds.Tables(0)
        dgvSched.DataBind()

    End Sub

    Protected Sub dgvSched_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim tDrAmt As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "dramt").ToString())
            Dramt += tDrAmt

            Dim tCrAmt As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "cramt").ToString())
            Cramt += tCrAmt

        End If

        If e.Row.RowType = DataControlRowType.Footer Then
            Dim lblGTotal As Label = DirectCast(e.Row.FindControl("lblGTotal"), Label)
            lblGTotal.Text = "TOTAL " & UCase(cboFormat.Text)

            Dim lblTotalDr As Label = DirectCast(e.Row.FindControl("lblDr"), Label)
            lblTotalDr.Text = Format(CDbl(Dramt.ToString()), "#,##0.00 ; (#,##0.00)")
            Dramt = 0

            Dim lblTotalCr As Label = DirectCast(e.Row.FindControl("lblCr"), Label)
            lblTotalCr.Text = Format(CDbl(Cramt.ToString()), "#,##0.00 ; (#,##0.00)")
            Cramt = 0

        End If

    End Sub

    Protected Sub dgvSched_RowCreated(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Private Sub cboFSmonYear3_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboFSmonYear3.SelectedIndexChanged
        If cboFSmonYear3.Text = "" Then
            Exit Sub
        End If

        dgvSched.DataSource = Nothing
        dgvSched.DataBind()
        'lblDRamt.Text = "0.00"
        'lblCRamt.Text = "0.00"

        cboFormat.Items.Clear()
        cboFormat.Items.Add("")
        cboFormat.Items.Add("Admin Expenses")
        cboFormat.Items.Add("Selling Expenses")
        cboFormat.Items.Add("Technical Expenses")
        cboFormat.Items.Add("Logistic Expenses")
        cboFormat.Items.Add("Overhead Expenses and Direct Labor")
        cboFormat.Items.Add("Prior Years Adjustment")

        lblFSdateT31.Text = Format(CDate(FirstDayOfMonth(cboFSmonYear3.Text)), "yyyy-MM-dd")
        lblFSdateT32.Text = Format(CDate(LastDayOfMonth(cboFSmonYear3.Text)), "yyyy-MM-dd")
        lblFirtDayYr3.Text = Format(CDate(cboFSmonYear3.Text), "yyyy") & "-01-01"

    End Sub
End Class