Imports MySql.Data.MySqlClient
Public Class MMReports
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

    End Sub

    Protected Sub lbClose_Click(sender As Object, e As EventArgs)
        Response.Redirect("MaterialManagement.aspx")

    End Sub

    Protected Sub lbNew_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub MMReports_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete

    End Sub

    Private Sub Filter4Show()
        lblFilter4.Visible = True
        cboFilter4.Visible = True
        txtText4.Visible = True
    End Sub

    Private Sub Filter4Hide()
        lblFilter4.Visible = False
        cboFilter4.Visible = False
        txtText4.Visible = False

    End Sub

    Private Sub MovTypeInv()

        cboFilter3.Items.Clear()

        dt = GetDataTable("select distinct concat(a.mov,space(1),b.movdesc) from invhdrtbl a left join movtbl b on a.mov=b.mov " &
                          "where a.mov between '801' and '803' and a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                          "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' order by b.movdesc")
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

    End Sub

    Private Sub MovTypeIss()

        cboFilter3.Items.Clear()
        dt = GetDataTable("select distinct concat(a.mov,space(1),b.movdesc) from isshdrtbl a left join movtbl b on a.mov=b.mov " &
                          "where a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                          "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' order by b.movdesc")
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

    End Sub

    Private Sub MovTypeMMRR()
        cboFilter3.Items.Clear()

        dt = GetDataTable("select distinct concat(a.mov,space(1),b.movdesc) from invhdrtbl a left join movtbl b on a.mov = b.mov " &
                          "where a.mov between '101' and '107' and a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                          "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' order by b.movdesc")
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

    End Sub

    Private Sub cboFormat_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboFormat.SelectedIndexChanged
        clrGridSum()

        If dpTransDate.Text = Nothing Then
            Exit Sub
        ElseIf dpTransDate2.Text = Nothing Then
            Exit Sub
        ElseIf Format(CDate(dpTransDate.Text), "yyyy-MM-dd") > Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") Then
            Exit Sub
        ElseIf cboMain.Text = "" Then
            Exit Sub
        End If

        Select Case cboFormat.Text
            Case "DR Register - Summary", "DR Register - Detailed"
                MovTypeIss()
                Filter4Hide()
                CheckBox3.Visible = False

            Case "RM Summary per Finished Goods"
                Filter4Hide()
                CheckBox3.Visible = False
                'Case "Issuance Summary per Material"
                '    MovTypeIss()
                '    Filter4Hide()

            Case "Production Register - Detailed", "Production Register - VOID"
                Filter4Show()
                MovTypeInv()
                CheckBox3.Visible = False
            Case "Material Issued to Production", "Material Issued To Prodn per Material"
                dgvIssProdMatl.DataSource = Nothing
                dgvIssProdMatl.DataBind()
                CheckBox3.Visible = False
                Filter4Show()
                cboFilter4.Items.Clear()
                cboFilter3.Items.Clear()
                dt = GetDataTable("select distinct concat(a.mov,space(1),b.movdesc) from isshdrtbl a left join movtbl b on a.mov=b.mov " &
                                  "where a.mov = '201' and a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                                  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' group by a.mov order by b.movdesc")
                If Not CBool(dt.Rows.Count) Then
                    Exit Sub
                Else
                    For Each dr As DataRow In dt.Rows
                        cboFilter3.Items.Add(dr.Item(0).ToString())
                    Next
                End If

                Call dt.Dispose()

            Case "Issuance Summary per Material"
                CheckBox3.Visible = False
                Filter4Show()
                cboFilter4.Items.Clear()
                cboFilter3.Items.Clear()
                dt = GetDataTable("select distinct concat(a.mov,space(1),b.movdesc) from isshdrtbl a left join movtbl b on a.mov=b.mov " &
                                  "where a.status <> 'void' and a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' group by a.mov order by b.movdesc") 'a.mov = '201' and 
                If Not CBool(dt.Rows.Count) Then
                    Exit Sub
                Else
                    For Each dr As DataRow In dt.Rows
                        cboFilter3.Items.Add(dr.Item(0).ToString())
                    Next

                End If

                Call dt.Dispose()

            Case "WRR per Customer per Material", "WRR Register - Detailed", "WRR Register - Summary"
                MovTypeIss()
                Filter4Hide()
                CheckBox3.Visible = False
                cboFilter3.Items.Clear()

            'Case "Material Issued To Prodn per Material"
            '    CheckBox3.Visible = False
            '    Filter4Show()
            '    cboFilter4.Items.Clear()
            '    cboFilter3.Items.Clear()

            '    dt = GetDataTable("select distinct concat(a.mov,space(1),b.movdesc) from isshdrtbl a left join movtbl b on a.mov=b.mov " &
            '                      "where a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
            '                      "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and a.mov = '201' order by b.movdesc")
            '    If Not CBool(dt.Rows.Count) Then
            '        Exit Sub
            '    Else

            '        For Each dr As DataRow In dt.Rows
            '            cboFilter3.Items.Add(dr.Item(0).ToString())
            '        Next
            '    End If

            '    Call dt.Dispose()

            Case "Production Summary - Per Material"
                Filter4Show()
                MovTypeInv()
                CheckBox3.Visible = False

            Case "MMRR Register - Summary", "MMRR Register - Detailed", "MMRR Register per Material - Detailed"
                MovTypeMMRR()
                Filter4Hide()
                CheckBox3.Visible = True

            Case "MMRR Register per Vendor - Detailed"
                CheckBox3.Visible = False

            Case "Daily Production Summary"
                Filter4Hide()
                CheckBox3.Visible = False

            Case "Beg. Inventory Setup - Detailed"
                cboFilter3.Items.Clear()
                dt = GetDataTable("select distinct concat(a.mov,space(1),b.movdesc) from invhdrtbl a left join movtbl b on a.mov = b.mov " &
                                  "where a.mov = '511' and a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                                  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' order by b.movdesc")
                If Not CBool(dt.Rows.Count) Then
                    Exit Sub
                Else
                    For Each dr As DataRow In dt.Rows
                        cboFilter3.Items.Add(dr.Item(0).ToString())

                    Next
                End If

                Call dt.Dispose()

                Filter4Hide()
                CheckBox3.Visible = False

        End Select

        cboFilter1.Items.Clear()
        cboFilter1.Items.Add("")
        cboFilter1.Items.Add("Plant")

    End Sub

    Private Sub dpTransDate_TextChanged(sender As Object, e As EventArgs) Handles dpTransDate.TextChanged, dpTransDate2.TextChanged
        clrGridSum()

        If dpTransDate.Text = Nothing Then
            Exit Sub
        ElseIf dpTransDate2.Text = Nothing Then
            Exit Sub
        ElseIf Format(CDate(dpTransDate.Text), "yyyy-MM-dd") > Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") Then
            Exit Sub
        End If

        cboMain.Items.Clear()
        cboMain.Items.Add("")
        'verify receiving
        dt = GetDataTable("select *  from invhdrtbl where status <> 'void' and " &
                          "transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                          "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' limit 1")
        If Not CBool(dt.Rows.Count) Then
            cboMain.Items.Remove("Receiving")

        Else
            cboMain.Items.Add("Receiving")
        End If

        Call dt.Dispose()

        'verify issuance
        dt = GetDataTable("select *  from isshdrtbl where status <> 'void' and " &
                          "transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                          "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' limit 1")
        If Not CBool(dt.Rows.Count) Then
            cboMain.Items.Remove("Issuance")

        Else
            cboMain.Items.Add("Issuance")
        End If

        Call dt.Dispose()


        'others
        'dt = GetDataTable("select *  from invhdrtbl where status <> 'void' and " &
        '                  "transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
        '                  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' limit 1")
        'If Not CBool(dt.Rows.Count) Then
        '    cboMain.Items.Remove("Other Reports")

        'Else
        '    cboMain.Items.Add("Other Reports")
        'End If

        'Call dt.Dispose()



    End Sub

    Private Sub cboMain_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboMain.SelectedIndexChanged
        clrGridSum()

        If dpTransDate.Text = Nothing Then
            Exit Sub
        ElseIf dpTransDate2.Text = Nothing Then
            Exit Sub
        ElseIf Format(CDate(dpTransDate.Text), "yyyy-MM-dd") > Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") Then
            Exit Sub
        End If

        cboFormat.Items.Clear()
        cboFormat.Items.Add("")

        Select Case cboMain.Text
            Case "Receiving"
                dt = GetDataTable("select *  from invhdrtbl where status <> 'void' and mov between '801' and '803' and " &
                                  "transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' limit 1")
                If Not CBool(dt.Rows.Count) Then
                    cboFormat.Items.Remove("MMRR Register - Summary")
                    cboFormat.Items.Remove("MMRR Register - Detailed")
                    cboFormat.Items.Remove("MMRR Register per Material - Detailed")
                    cboFormat.Items.Remove("MMRR Register per Vendor - Detailed")
                    cboFormat.Items.Remove("Production Register - Detailed")
                    cboFormat.Items.Remove("Daily Production Summary")
                    cboFormat.Items.Remove("Production Summary - Per Material")
                Else
                    cboFormat.Items.Add("MMRR Register - Summary")
                    cboFormat.Items.Add("MMRR Register - Detailed")
                    cboFormat.Items.Add("MMRR Register per Material - Detailed")
                    cboFormat.Items.Add("MMRR Register per Vendor - Detailed")
                    cboFormat.Items.Add("Production Register - Detailed")
                    cboFormat.Items.Add("Daily Production Summary")
                    cboFormat.Items.Add("Production Summary - Per Material")
                End If

                dt.Dispose()

                dt = GetDataTable("select *  from invhdrtbl where status = 'void' and mov between '801' and '803' and " &
                                  "transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' limit 1")
                If Not CBool(dt.Rows.Count) Then
                    cboFormat.Items.Remove("Production Register - VOID")
                Else
                    cboFormat.Items.Add("Production Register - VOID")
                End If

                dt.Dispose()

                'verify WRR
                dt = GetDataTable("select *  from wrrhdrtbl where status <> 'void' and " &
                                  "transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' limit 1")
                If Not CBool(dt.Rows.Count) Then
                    cboFormat.Items.Remove("WRR Register - Summary")
                    cboFormat.Items.Remove("WRR Register - Detailed")
                    cboFormat.Items.Remove("WRR per Customer per Material")
                Else
                    cboFormat.Items.Add("WRR Register - Summary")
                    cboFormat.Items.Add("WRR Register - Detailed")
                    cboFormat.Items.Add("WRR per Customer per Material")
                End If

                Call dt.Dispose()

                'verify initial inventory
                dt = GetDataTable("select *  from invhdrtbl where status <> 'void' and mov = '511' and " &
                                  "transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and " &
                                  "begjvstat is not null limit 1")
                If Not CBool(dt.Rows.Count) Then
                    cboMain.Items.Remove("Beg. Inventory Setup - Detailed")
                Else
                    cboFormat.Items.Add("Beg. Inventory Setup - Detailed")
                End If

                Call dt.Dispose()

            Case "Issuance"
                dt = GetDataTable("select *  from isshdrtbl where status <> 'void' and " &
                                  "transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' limit 1")
                If Not CBool(dt.Rows.Count) Then
                    cboFormat.Items.Remove("DR Register - Summary")
                    cboFormat.Items.Remove("DR Register - Detailed")
                    cboFormat.Items.Remove("Issuance Summary per Material")
                Else
                    cboFormat.Items.Add("DR Register - Summary")
                    cboFormat.Items.Add("DR Register - Detailed")
                    cboFormat.Items.Add("Issuance Summary per Material")
                End If

                dt.Dispose()

                dt = GetDataTable("select *  from isshdrtbl where status <> 'void' and mov ='201' and " &
                                  "transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' limit 1")
                If Not CBool(dt.Rows.Count) Then
                    cboFormat.Items.Remove("Material Issued to Production")
                    Select Case vLoggedBussArea
                        Case "8100"
                            cboFormat.Items.Remove("Material Issued To Prodn per Material") 'Material Issued to Production per Material
                            cboFormat.Items.Remove("RM Summary per Finished Goods")
                    End Select

                Else
                    cboFormat.Items.Add("Material Issued to Production")
                    Select Case vLoggedBussArea
                        Case "8100"
                            cboFormat.Items.Add("Material Issued To Prodn per Material") 'Material Issued to Production per Material
                            cboFormat.Items.Add("RM Summary per Finished Goods")
                    End Select


                End If

                Call dt.Dispose()

            Case "Other Reports"
                dt = GetDataTable("select *  from prodreqhdrtbl where stat <> 'void' and " &
                                  "transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' limit 1")
                If Not CBool(dt.Rows.Count) Then
                    cboFormat.Items.Remove("Request for Production Reports")
                Else
                    cboFormat.Items.Add("Request for Production Reports")
                End If

                Call dt.Dispose()

                dt = GetDataTable("select *  from isshdrtbl where status <> 'void' and " &
                                  "transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' limit 1")
                If Not CBool(dt.Rows.Count) Then
                    cboMain.Items.Remove("Daily Delivery and Product Monitoring")

                Else
                    cboFormat.Items.Add("Daily Delivery and Product Monitoring")
                End If

                Call dt.Dispose()

        End Select

    End Sub

    Private Sub cboFilter1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboFilter1.SelectedIndexChanged
        clrGridSum()
        If dpTransDate.Text = Nothing Then
            Exit Sub
        ElseIf dpTransDate2.Text = Nothing Then
            Exit Sub
        ElseIf Format(CDate(dpTransDate.Text), "yyyy-MM-dd") > Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") Then
            Exit Sub
        ElseIf cboMain.Text = "" Then
            Exit Sub
        ElseIf cboFormat.Text = "" Then
            Exit Sub
        ElseIf cboFilter1.Text = "" Then
            Exit Sub
        End If

        cboFilter2.Items.Clear()
        cboFilter2.Items.Add("")
        'group by Plant
        Select Case cboFilter1.Text
            Case "Plant"
                Select Case cboMain.Text
                    Case "Receiving"
                        Select Case cboFormat.Text
                            Case "Daily Production Summary", "Production Register - Detailed"
                                dt = GetDataTable("select distinct concat(a.plntno,space(1),b.plntname) from invhdrtbl a " &
                                                  "left join plnttbl b on a.plntno=b.plntno where a.status <> 'void' and " &
                                                  "a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                                                  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and a.mov between '801' and '803' order by b.plntname")

                            Case "WRR per Customer per Material", "WRR Register - Detailed", "WRR Register - Summary"
                                dt = GetDataTable("select distinct concat(a.plntno,space(1),b.plntname) from wrrhdrtbl a " &
                                                  "left join plnttbl b on a.plntno=b.plntno where a.status <> 'void' and " &
                                                  "a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                                                  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' order by b.plntname")

                            Case Else
                                dt = GetDataTable("select distinct concat(a.plntno,space(1),b.plntname) from invhdrtbl a left join plnttbl b on a.plntno=b.plntno " &
                                                  "where b.status = '" & "active" & "' and b.ba = '" & vLoggedBussArea & "' " &
                                                  "and a.status <> 'void' and a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                                                  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' order by b.plntname")
                        End Select

                    Case "Issuance"
                        Select Case cboFormat.Text
                            Case "DR Register - Summary", "DR Register - Detailed", "Issuance Summary per Material"
                                dt = GetDataTable("select distinct concat(a.plntno,space(1),b.plntname) from isshdrtbl a " &
                                                  "left join plnttbl b on a.plntno=b.plntno where a.status <> 'void' and " &
                                                  "a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                                                  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' order by b.plntname")
                            Case "Material Issued to Production", "Material Issued To Prodn per Material", "RM Summary per Finished Goods"
                                dt = GetDataTable("select distinct concat(a.plntno,space(1),b.plntname) from isshdrtbl a " &
                                                  "left join plnttbl b on a.plntno=b.plntno where a.status <> 'void' and " &
                                                  "a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                                                  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and a. mov ='201' " &
                                                  "order by b.plntname")

                        End Select

                    Case Else
                        dt = GetDataTable("select distinct concat(plntno,space(1),plntname) from plnttbl where status = 'active' order by plntname")

                End Select

                If Not CBool(dt.Rows.Count) Then
                    Exit Sub
                Else

                    cboFilter2.Items.Add("ALL")

                    For Each dr As DataRow In dt.Rows
                        cboFilter2.Items.Add(dr.Item(0).ToString())

                    Next
                End If


                Call dt.Dispose()

                cboFilter2.Visible = True
                lblFilter2.Visible = True

        End Select

        btnGenerate.Enabled = True

    End Sub

    Private Sub cboFilter2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboFilter2.SelectedIndexChanged
        clrGridSum()

        If dpTransDate.Text = Nothing Then
            Exit Sub
        ElseIf dpTransDate2.Text = Nothing Then
            Exit Sub
        ElseIf Format(CDate(dpTransDate.Text), "yyyy-MM-dd") > Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") Then
            Exit Sub
        ElseIf cboMain.Text = "" Then
            Exit Sub
        ElseIf cboFormat.Text = "" Then
            Exit Sub
        ElseIf cboFilter1.Text = "" Then
            Exit Sub
        ElseIf cboFilter2.Text = "" Then
            Exit Sub
        End If

        Select Case cboMain.Text
            Case "Receiving"
                Select Case cboFilter1.Text
                    Case "Plant"
                        txtText2.Visible = False

                        Select Case cboFormat.Text
                            Case "Daily Production Summary"
                                Exit Sub

                            Case "MMRR Register per Vendor - Detailed"
                                dgvPRpVD.DataSource = Nothing
                                dgvPRpVD.DataBind()

                                lblFilter3.Visible = True
                                txtText3.Visible = False
                                cboFilter3.Visible = True
                                lblFilter4.Visible = False
                                cboFilter4.Visible = False
                                txtText4.Visible = False

                                lblFilter3.Text = "Vendor:"
                                cboFilter3.Items.Clear()
                                cboFilter3.Items.Add("")
                                cboFilter3.Items.Add("ALL")

                                Select Case cboFilter2.Text
                                    Case "ALL"
                                        dt = GetDataTable("select distinct concat(a.venno,space(1),b.venname) from invhdrtbl a left join venmasttbl b on a.venno=b.venno " &
                                                          "where a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                                                          "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' group by a.venno order by b.venname")

                                    Case Else
                                        dt = GetDataTable("select distinct concat(a.venno,space(1),b.venname) from invhdrtbl a " &
                                                          "left join venmasttbl b on a.venno=b.venno where a.plntno = '" & cboFilter2.Text.Substring(0, 3) & "' " &
                                                          "and a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                                                          "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' order by b.venname")

                                End Select

                                If Not CBool(dt.Rows.Count) Then
                                    Exit Sub
                                Else
                                    For Each dr As DataRow In dt.Rows
                                        cboFilter3.Items.Add(dr.Item(0).ToString())

                                    Next
                                End If

                                Call dt.Dispose()

                                Exit Sub

                            Case "WRR per Customer per Material", "WRR Register - Summary", "WRR Register - Detailed"
                                lblFilter3.Visible = True
                                txtText3.Visible = False
                                cboFilter3.Visible = True
                                lblFilter4.Visible = False
                                cboFilter4.Visible = False
                                txtText4.Visible = False

                                lblFilter3.Text = "Customer:"
                                cboFilter3.Items.Clear()
                                cboFilter3.Items.Add("")
                                cboFilter3.Items.Add("ALL")

                                Select Case cboFilter2.Text
                                    Case "ALL"
                                        dt = GetDataTable("select distinct concat(a.custno,space(1),b.bussname) from wrrhdrtbl a left join custmasttbl b on a.custno=b.custno " &
                                                          "where a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                                                          "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' order by b.bussname")

                                    Case Else
                                        dt = GetDataTable("select distinct concat(a.custno,space(1),b.bussname) from wrrhdrtbl a left join custmasttbl b on a.custno=b.custno " &
                                                          "where a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                                                          "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and a.plntno = '" & cboFilter2.Text.Substring(0, 3) & "' order by b.bussname")

                                End Select

                                If Not CBool(dt.Rows.Count) Then
                                    Exit Sub
                                Else
                                    For Each dr As DataRow In dt.Rows
                                        cboFilter3.Items.Add(dr.Item(0).ToString())

                                    Next

                                End If

                                Call dt.Dispose()

                                Exit Sub

                        End Select

                End Select

            Case "Issuance"
                Select Case cboFilter1.Text
                    Case "Plant"
                        txtText2.Visible = False

                        Select Case cboFormat.Text
                            Case "RM Summary per Finished Goods"
                                popFGlistSum()

                            Case "Material Issued to Production", "Issuance Summary per Material", "Material Issued To Prodn per Material"
                                dgvIssProdMatl.DataSource = Nothing
                                dgvIssProdMatl.DataBind()
                                popIssProdType()

                        End Select

                End Select

        End Select


        Select Case cboMain.Text
            Case "Other Reports"
                cboFilter3.Visible = False
                txtText3.Visible = False
                lblFilter3.Visible = False
                lblFilter3.Text = ""

                Select Case cboFormat.Text
                    Case "Daily Delivery and Product Monitoring"
                        'GroupBox1.Visible = True

                        'dt = GetDataTable("select b.areano,c.areaname from isshdrtbl a left join smnmtbl b on a.smnno=b.smnno " &
                        '                  "left join areatbl c on b.areano=c.areano where a.transdate between '" & Format(CDate(dpTransDate.text), "yyyy-MM-dd") & "' " &
                        '                  "and '" & Format(CDate(dpTransDate2.text), "yyyy-MM-dd") & "' and a.status <> 'void' and a.mov between '601' and '610' group by b.areano")
                        'If Not CBool(dt.Rows.Count) Then Call MessageBox.Show("Not found.", "MM Reports", MessageBoxButtons.OK, MessageBoxIcon.Warning) : Exit Sub

                        'lvBussArea.Items.Clear()

                        'For i As Integer = 0 To dt.Rows.Count - 1
                        '    lvBussArea.Items.Add(dt.Rows(i).Item(0).ToString())
                        '    lvBussArea.Items(i).SubItems.Add(dt.Rows(i).Item(1).ToString())

                        'Next

                        'Call dt.Dispose()

                        'txtText2.Visible = True
                        'dt = GetDataTable("select plntno from plnttbl where plntname = '" & cboFilter2.Text & "'")
                        'If Not CBool(dt.Rows.Count) Then Call MessageBox.Show("Not found.", "MM Reports", MessageBoxButtons.OK, MessageBoxIcon.Warning) : Exit Sub

                        'For Each dr As DataRow In dt.Rows
                        '    txtText2.Text = dr.Item(0).ToString()

                        'Next

                        'Call dt.Dispose()

                End Select

            Case Else
                cboFilter3.Visible = True
                txtText3.Visible = True
                lblFilter3.Visible = True

                Select Case cboFormat.Text
                    Case "RM Summary per Finished Goods"
                        lblFilter3.Text = "Finished Goods"
                        Filter4Hide()
                    Case Else

                        lblFilter3.Text = "Movement Type"
                        'GroupBox1.Visible = False
                End Select
        End Select


    End Sub

    Private Sub popFGlistSum()
        cboFilter3.Items.Clear()

        dt = GetDataTable("select b.mmdesc from invhdrtbl a left join mmasttbl b on a.pono = b.codeno where a.mov = '801' " &
                          "and a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                          "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and a.dsrstat = 'Finished' group by a.pono order by b.mmdesc")
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

    End Sub

    Private Sub popIssProdType()
        cboFilter4.Items.Clear()
        Select Case cboFormat.Text
            Case "Material Issued to Production", "Material Issued To Prodn per Material"
                Select Case cboFilter2.Text
                    Case "ALL"
                        dt = GetDataTable("select distinct b.mmtype from issdettbl a left join mmasttbl b on a.codeno=b.codeno " &
                                          "left join isshdrtbl c on a.dono=c.dono where c.status <> 'void' and " &
                                          "c.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                          "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and " &
                                          "c.mov = '201'")
                    Case Else
                        dt = GetDataTable("select distinct b.mmtype from issdettbl a left join mmasttbl b on a.codeno=b.codeno " &
                                          "left join isshdrtbl c on a.dono=c.dono where c.status <> 'void' and " &
                                          "c.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                          "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and " &
                                          "c.mov = '201' and c.plntno = '" & cboFilter2.Text.Substring(0, 3) & "'")
                End Select

            Case "Issuance Summary per Material"
                Select Case cboFilter2.Text
                    Case "ALL"
                        dt = GetDataTable("select distinct b.mmtype from issdettbl a left join mmasttbl b on a.codeno=b.codeno " &
                                          "left join isshdrtbl c on a.dono=c.dono where c.status <> 'void' and " &
                                          "c.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                          "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and " &
                                          "c.mov = '" & cboFilter3.Text.Substring(0, 3) & "'")
                    Case Else
                        dt = GetDataTable("select distinct b.mmtype from issdettbl a left join mmasttbl b on a.codeno=b.codeno " &
                                          "left join isshdrtbl c on a.dono=c.dono where c.status <> 'void' and " &
                                          "c.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                          "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and " &
                                          "c.mov = '" & cboFilter3.Text.Substring(0, 3) & "' and " &
                                          "c.plntno = '" & cboFilter2.Text.Substring(0, 3) & "'")
                End Select

        End Select

        If Not CBool(dt.Rows.Count) Then
            Exit Sub
        Else
            cboFilter4.Items.Add("")

            For Each dr As DataRow In dt.Rows
                cboFilter4.Items.Add(dr.Item(0).ToString())
            Next

        End If

        Call dt.Dispose()

    End Sub

    Private Sub clrGridSum()
        dgvRegSum.DataSource = Nothing
        dgvRegSum.DataBind()

        dgvProdRegDet.DataSource = Nothing
        dgvProdRegDet.DataBind()

        dgvProdRegDet.DataSource = Nothing
        dgvProdRegDet.DataBind()

        dgvRegSumIss.DataSource = Nothing
        dgvRegSumIss.DataBind()

        dgvMMperMatl.DataSource = Nothing
        dgvMMperMatl.DataBind()

        dgvIssProdMatl.DataSource = Nothing
        dgvIssProdMatl.DataBind()

        dgvIssProdPerMatl.DataSource = Nothing
        dgvIssProdPerMatl.DataBind()

        dgvISpM.DataSource = Nothing
        dgvISpM.DataBind()




    End Sub

    Private Sub cboFilter3_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboFilter3.SelectedIndexChanged
        clrGridSum()

        If dpTransDate.Text = Nothing Then
            Exit Sub
        ElseIf dpTransDate2.Text = Nothing Then
            Exit Sub
        ElseIf Format(CDate(dpTransDate.Text), "yyyy-MM-dd") > Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") Then
            Exit Sub
        ElseIf cboMain.Text = "" Then
            Exit Sub
        ElseIf cboFormat.Text = "" Then
            Exit Sub
        ElseIf cboFilter3.Text = "" Then
            Exit Sub
        End If

        Select Case cboFormat.Text
            Case "MMRR Register per Vendor - Detailed"
                dgvPRpVD.DataSource = Nothing
                dgvPRpVD.DataBind()
                cboFilter5.Items.Clear()

                Select Case cboFilter2.Text
                    Case "ALL"
                        Select Case cboFilter3.Text
                            Case "ALL"
                                dt = GetDataTable("select distinct c.mmtype from invdettbl a left join invhdrtbl b on a.mmrrno=b.mmrrno " &
                                                  "left join mmasttbl c on a.codeno=c.codeno where b.mov between '101' and '104' and " &
                                                  "b.status <> 'void' and b.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                                  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "'")
                            Case Else
                                dt = GetDataTable("select distinct c.mmtype from invdettbl a left join invhdrtbl b on a.mmrrno=b.mmrrno " &
                                                  "left join mmasttbl c on a.codeno=c.codeno where b.mov between '101' and '104' and " &
                                                  "b.status <> 'void' and b.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                                  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and " &
                                                  "b.venno = '" & cboFilter3.Text.Substring(0, 5) & "'")
                        End Select
                    Case Else
                        Select Case cboFilter3.Text
                            Case "ALL"
                                dt = GetDataTable("select distinct c.mmtype from invdettbl a left join invhdrtbl b on a.mmrrno=b.mmrrno " &
                                                  "left join mmasttbl c on a.codeno=c.codeno where b.mov between '101' and '104' and " &
                                                  "b.status <> 'void' and b.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                                  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and " &
                                                  "b.plntno = '" & cboFilter2.Text.Substring(0, 3) & "'")
                            Case Else
                                dt = GetDataTable("select distinct c.mmtype from invdettbl a left join invhdrtbl b on a.mmrrno=b.mmrrno " &
                                                  "left join mmasttbl c on a.codeno=c.codeno where b.mov between '101' and '104' and " &
                                                  "b.status <> 'void' and b.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                                  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and " &
                                                  "b.venno = '" & cboFilter3.Text.Substring(0, 5) & "' and b.plntno = '" & cboFilter2.Text.Substring(0, 3) & "'")
                        End Select
                End Select

                If Not CBool(dt.Rows.Count) Then
                    Exit Sub

                Else
                    cboFilter5.Items.Add("ALL")
                    For Each dr As DataRow In dt.Rows
                        cboFilter5.Items.Add(dr.Item(0).ToString())

                    Next

                End If

                Call dt.Dispose()

            Case Else
                Select Case lblFilter3.Text
                    Case "Movement Type"
                        If cboFilter3.Text = "ALL" Then
                            cboFilter4.Items.Clear()
                            cboFilter4.Items.Add("ALL")
                            'Exit Sub

                        End If

                        Select Case cboMain.Text
                            Case "Receiving"
                                Select Case cboFormat.Text
                                    Case "MMRR Register per Material - Detailed"
                                        cboFilter5.Items.Clear()
                                        Select Case cboFilter3.Text
                                            Case "ALL"
                                                dt = GetDataTable("select distinct concat(c.glacct,space(1),d.acctdesc) from invdettbl a left join invhdrtbl b on a.mmrrno=b.mmrrno " &
                                                                  "left join mmasttbl c on a.codeno=c.codeno left join acctcharttbl d on c.glacct = d.acctno " &
                                                                  "where b.status <> 'void' and b.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                                                                  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and b.tc = '10'")
                                            Case Else
                                                dt = GetDataTable("select distinct concat(c.glacct,space(1),d.acctdesc) from invdettbl a left join invhdrtbl b on a.mmrrno=b.mmrrno " &
                                                                  "left join mmasttbl c on a.codeno=c.codeno left join acctcharttbl d on c.glacct = d.acctno " &
                                                                  "where b.status <> 'void' and b.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                                                                  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and " &
                                                                  "b.tc = '10' and b.mov = '" & cboFilter3.Text.Substring(0, 3) & "'")
                                        End Select

                                        If Not CBool(dt.Rows.Count) Then
                                            lblMsg.Text = "No Material Found"
                                            Exit Sub

                                        Else
                                            cboFilter5.Items.Add("")
                                            cboFilter5.Items.Add("ALL")
                                            For Each dr As DataRow In dt.Rows
                                                cboFilter5.Items.Add(dr.Item(0).ToString())

                                            Next

                                        End If

                                        Call dt.Dispose()

                                    Case Else
                                        cboFilter4.Items.Clear()

                                        dt = GetDataTable("select distinct dsrstat from invhdrtbl where mov = '" & cboFilter3.Text.Substring(0, 3) & "' " &
                                                          "and transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                                                          "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and dsrstat <> 'void'")
                                        If Not CBool(dt.Rows.Count) Then
                                            Exit Sub

                                        Else
                                            cboFilter4.Items.Add("")
                                            cboFilter4.Items.Add("ALL")
                                            For Each dr As DataRow In dt.Rows
                                                cboFilter4.Items.Add(dr.Item(0).ToString())

                                            Next

                                        End If

                                        Call dt.Dispose()


                                End Select

                            Case "Issuance"
                                cboFilter5.Items.Clear()
                                Select Case cboFormat.Text
                                    Case "DR Register - Summary", "DR Register - Detailed", "Issuance Summary per Material"
                                        dt = GetDataTable("select distinct status from isshdrtbl where mov = '" & cboFilter3.Text.Substring(0, 3) & "' " &
                                                          "and transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                                                          "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "'")
                                End Select

                                If Not CBool(dt.Rows.Count) Then
                                    Exit Sub

                                Else
                                    cboFilter5.Items.Add("")
                                    cboFilter5.Items.Add("ALL")
                                    For Each dr As DataRow In dt.Rows
                                        cboFilter5.Items.Add(dr.Item(0).ToString())

                                    Next

                                End If

                                Call dt.Dispose()

                        End Select

                    Case "Finished Goods"
                        Select Case cboFilter3.Text
                            Case "ALL"
                                txtText3.Text = ""
                                txtText3.Visible = False
                                Exit Sub
                            Case Else
                                dt = GetDataTable("select codeno from mmasttbl where mmdesc = '" & cboFilter3.Text & "'")
                                If Not CBool(dt.Rows.Count) Then
                                    txtText3.Text = ""
                                    Exit Sub

                                Else
                                    txtText3.Visible = True
                                    For Each dr As DataRow In dt.Rows
                                        txtText3.Text = dr.Item(0).ToString() & ""

                                    Next

                                End If

                                Call dt.Dispose()

                        End Select

                End Select
        End Select


    End Sub

    Private Sub cboFilter4_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboFilter4.SelectedIndexChanged
        If dpTransDate.Text = Nothing Then
            Exit Sub
        ElseIf dpTransDate2.Text = Nothing Then
            Exit Sub
        ElseIf Format(CDate(dpTransDate.Text), "yyyy-MM-dd") > Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") Then
            Exit Sub
        ElseIf cboMain.Text = "" Then
            Exit Sub
        ElseIf cboFormat.Text = "" Then
            Exit Sub
        ElseIf cboFilter3.Text = "" Then
            Exit Sub
        ElseIf cboFilter4.Text = "" Then
            Exit Sub
        End If

        Select Case cboFormat.Text
            Case "Material Issued to Production"
                dgvIssProdMatl.DataSource = Nothing
                dgvIssProdMatl.DataBind()
                filter4HideOff()

            Case "Material Issued To Prodn per Material"
                dgvIssProdPerMatl.DataSource = Nothing
                dgvIssProdPerMatl.DataBind()
                popIssToProdnDet()
                filter4HideOn()
                GrpBox3VisOn()

                'Select Case cboFilter4.Text
                '    Case "Raw Materials"

                '    Case Else
                '        filter4HideOff()
                'End Select
            Case "Issuance Summary per Material"
                dgvISpM.DataSource = Nothing
                dgvISpM.DataBind()
                'filter4HideOn()

            Case Else
                filter4HideOff()

        End Select
    End Sub

    Private Sub filter4HideOn()
        lblFilter5.Visible = True
        cboFilter5.Visible = True
        txtText5.Visible = True

    End Sub

    Private Sub filter4HideOff()
        lblFilter5.Visible = False
        cboFilter5.Visible = False
        txtText5.Visible = False
    End Sub

    Private Sub GrpBox3VisOn()
        lblOptLabel.Visible = True
        RadioButton12.Visible = True
        RadioButton13.Visible = True
        RadioButton14.Visible = True

    End Sub

    Private Sub GrpBox3VisOff()
        lblOptLabel.Visible = False
        RadioButton12.Visible = False
        RadioButton13.Visible = False
        RadioButton14.Visible = False

    End Sub

    Private Sub popIssToProdnDet()
        cboFilter5.Items.Clear()

        Select Case cboFilter2.Text 'plnt
            Case "ALL"
                dt = GetDataTable("select distinct ifnull(b.codename,b.mmdesc) from issdettbl a left join isshdrtbl c on a.dono=c.dono " &
                                  "left join mmasttbl b on a.codeno=b.codeno where c.status <> 'void' and c.mov = '201' and " &
                                  "c.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                                  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and  " &
                                  "b.mmtype = '" & cboFilter4.Text & "' order by ifnull(b.codename,b.mmdesc)")
            Case Else
                dt = GetDataTable("select distinct ifnull(b.codename,b.mmdesc) from issdettbl a left join isshdrtbl c on a.dono=c.dono " &
                                  "left join mmasttbl b on a.codeno=b.codeno where c.status <> 'void' and c.mov = '201' and " &
                                  "c.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                                  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and b.mmtype = '" & cboFilter4.Text & "' " &
                                  "and c.plntno = '" & cboFilter2.Text.Substring(0, 3) & "' order by ifnull(b.codename,b.mmdesc)")

        End Select

        If Not CBool(dt.Rows.Count) Then
            Exit Sub
        Else
            For Each dr As DataRow In dt.Rows
                cboFilter5.Items.Add(dr.Item(0).ToString())
                cboFilter5.Items.Remove("")
            Next

        End If

        Call dt.Dispose()

    End Sub

    Private Sub cboFilter5_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboFilter5.SelectedIndexChanged
        If dpTransDate.Text = Nothing Then
            Exit Sub
        ElseIf dpTransDate2.Text = Nothing Then
            Exit Sub
        ElseIf Format(CDate(dpTransDate.Text), "yyyy-MM-dd") > Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") Then
            Exit Sub
        ElseIf cboMain.Text = "" Then
            Exit Sub
        ElseIf cboFormat.Text = "" Then
            Exit Sub
        ElseIf cboFilter3.Text = "" Then
            lblMsg.Text = "Select Movement Type"
            Exit Sub
        ElseIf cboFilter5.Text = "" Then
            Exit Sub
        End If

        Select Case cboFormat.Text
            Case "MMRR Register per Vendor - Detailed"
                dgvPRpVD.DataSource = Nothing
                dgvPRpVD.DataBind()

            Case "MMRR Register per Material - Detailed"
                dgvMMperMatl.DataSource = Nothing
                dgvMMperMatl.DataBind()

        End Select

        dt = GetDataTable("select codeno from mmasttbl where ifnull(codename,mmdesc) = '" & cboFilter5.Text & "'")
        If Not CBool(dt.Rows.Count) Then
            lblMsg.Text = "Material Not Found..."
            Exit Sub
        Else
            For Each dr As DataRow In dt.Rows
                txtText5.Text = dr.Item(0).ToString() & ""

            Next
        End If

        Call dt.Dispose()

        Select Case cboFilter3.Text.Substring(0, 3)
            Case "201"
                dt = GetDataTable("select distinct ifnull(b.dsrstat,'Open') from isshdrtbl a left join issdettbl d on a.dono=d.dono " &
                                  "left join invhdrtbl b on a.dsrno=b.mmrrno left join mmasttbl c on b.pono=c.codeno where " &
                                  "d.codeno = '" & txtText5.Text & "' and a.status <> 'void' and a.mov = '201' and " &
                                  "a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                                  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "'")
                If Not CBool(dt.Rows.Count) Then
                    Exit Sub
                Else
                    RadioButton13.Enabled = False
                    RadioButton14.Enabled = False
                    RadioButton12.Checked = True
                    For Each dr As DataRow In dt.Rows
                        Select Case dr.Item(0).ToString()
                            Case "On Process"
                                RadioButton13.Enabled = True
                            Case "Finished"
                                RadioButton14.Enabled = True

                        End Select

                    Next

                End If

                Call dt.Dispose()

        End Select



    End Sub

    Private Sub btnGenerate_Click(sender As Object, e As EventArgs) Handles btnGenerate.Click
        If dpTransDate.Text = Nothing Then
            Exit Sub
        ElseIf dpTransDate2.Text = Nothing Then
            Exit Sub
        ElseIf Format(CDate(dpTransDate.Text), "yyyy-MM-dd") > Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") Then
            Exit Sub
        ElseIf cboMain.Text = "" Then
            Exit Sub
        ElseIf cboFormat.Text = "" Then
            Exit Sub

        End If

        Select Case cboMain.Text
            Case "Receiving"
                Select Case cboFormat.Text
                    Case "MMRR Register - Summary" 'done
                        Select Case cboFilter1.Text
                            Case "Plant"
                                rptMMRR01()

                        End Select

                    Case "MMRR Register - Detailed"
                        Select Case cboFilter1.Text
                            Case "Plant"
                                rptMMRR02()

                        End Select

                    Case "MMRR Register per Material - Detailed"
                        If cboFilter5.Text = "" Or cboFilter5.Text = Nothing Then
                            lblMsg.Text = "Select MM Type"
                            Exit Sub
                        End If

                        rptMMRRperMatlDet()

                    Case "MMRR Register per Vendor - Detailed"
                        If cboFilter5.Text = "" Or cboFilter5.Text = Nothing Then
                            lblMsg.Text = "Select MM Type"
                            Exit Sub
                        End If

                        MMRRregVendor()

                    Case "Production Register - Detailed", "Production Register - VOID" 'done
                        prodnReg01()

                    Case "Production Summary - Per Material"
                        prodnSumMatl()

                    Case "Beg. Inventory Setup - Detailed"
                        rptMMRR02()

                    Case "Daily Production Summary"
                        rptDPRadm()

                    Case "WRR Register - Summary"
                        WRR_Reg_Sum()

                    Case "WRR Register - Detailed"
                        WRR_Reg_Det()

                    Case "WRR per Customer per Material"
                        WRR_custperMatl()

                End Select

            Case "Issuance"
                Select Case cboFilter1.Text
                    Case "Plant"
                        Select Case cboFormat.Text
                            Case "DR Register - Summary" 'done
                                If cboFilter5.Text = "" Or cboFilter5.Text = Nothing Then
                                    lblMsg.Text = "Select Status (Filter 5)"
                                    Exit Sub
                                End If

                                rptMMDO01()

                            Case "DR Register - Detailed"
                                If cboFilter5.Text = "" Or cboFilter5.Text = Nothing Then
                                    lblMsg.Text = "Select Status (Filter 5)"
                                    Exit Sub
                                End If

                                rptMMDO02()

                            Case "Issuance Summary per Material"
                                If cboFilter5.Text = "" Or cboFilter5.Text = Nothing Then
                                    lblMsg.Text = "Select Status (Filter 5)"
                                    Exit Sub
                                End If

                                rptMM_Iss01()

                            Case "Material Issued to Production" 'done
                                rptMM_Iss_Prodn()

                            Case "Material Issued To Prodn per Material" 'done
                                If cboFilter5.Text = "" Then
                                    lblMsg.Text = "Select Material"
                                    Exit Sub
                                End If

                                Select Case cboFilter4.Text
                                    Case "Raw Materials"
                                        If RadioButton12.Checked = False And RadioButton13.Checked = False And RadioButton14.Checked = False Then
                                            lblMsg.Text = "Select Option"
                                            Exit Sub
                                        End If
                                End Select

                                popIssDetAll_MMrep()

                            Case "RM Summary per Finished Goods"
                                rptMM_Iss02()

                        End Select

                End Select
        End Select


    End Sub

    Private Sub rptMMRRperMatlDet()
        Panel3.Visible = True

        Select Case cboFilter5.Text 'mmtype
            Case "ALL"
                Select Case cboFilter2.Text 'plant
                    Case "ALL"
                        Select Case cboFilter3.Text 'mov type
                            Case "ALL"
                                dt = GetDataTable("select * from invhdrtbl where tc ='10' and status <> 'void' " &
                                                  "and transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                                                  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' limit 1")
                            Case Else
                                dt = GetDataTable("select * from invhdrtbl where mov = '" & cboFilter3.Text.Substring(0, 3) & "' " &
                                                  "and status <> 'void' and transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                                  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and tc = '10' limit 1")
                        End Select
                    Case Else
                        'per plant
                        Select Case cboFilter3.Text
                            'mov type
                            Case "ALL"
                                dt = GetDataTable("select * from invhdrtbl where status <> 'void' " &
                                                  "and transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                                                  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and " &
                                                  "plntno = '" & cboFilter2.Text.Substring(0, 3) & "' and tc = '10' limit 1")
                            Case Else
                                dt = GetDataTable("select * from invhdrtbl where mov = '" & cboFilter3.Text.Substring(0, 3) & "' and status <> 'void' " &
                                                  "and transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                                                  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and " &
                                                  "plntno = '" & cboFilter2.Text.Substring(0, 3) & "' and tc = '10' limit 1")
                        End Select

                End Select
            Case Else
                Select Case cboFilter2.Text 'plant
                    Case "ALL"
                        Select Case cboFilter3.Text 'mov type
                            Case "ALL"
                                dt = GetDataTable("select * from invdettbl a left join invhdrtbl b on a.mmrrno=b.mmrrno left join mmasttbl c " &
                                                  "on a.codeno=c.codeno where b.tc ='10' and b.status <> 'void' " &
                                                  "and b.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                                                  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and " &
                                                  "c.glacct = '" & cboFilter5.Text.Substring(0, 7) & "' limit 1")
                            Case Else
                                dt = GetDataTable("select * from invdettbl a left join invhdrtbl b on a.mmrrno=b.mmrrno left join mmasttbl c " &
                                                  "on a.codeno=c.codeno where b.tc ='10' and b.status <> 'void' " &
                                                  "and b.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                                                  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and " &
                                                  "c.glacct = '" & cboFilter5.Text.Substring(0, 7) & "' and b.mov = '" & cboFilter3.Text.Substring(0, 3) & "' " &
                                                  " limit 1")
                        End Select
                    Case Else
                        'per plant
                        Select Case cboFilter3.Text 'mov type
                            Case "ALL"
                                dt = GetDataTable("select * from invdettbl a left join invhdrtbl b on a.mmrrno=b.mmrrno left join mmasttbl c " &
                                                  "on a.codeno=c.codeno where b.tc ='10' and b.status <> 'void' " &
                                                  "and b.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                                                  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and " &
                                                  "c.glacct = '" & cboFilter5.Text.Substring(0, 7) & "' and b.plntno = '" & cboFilter2.Text.Substring(0, 3) & "' limit 1")
                            Case Else
                                dt = GetDataTable("select * from invdettbl a left join invhdrtbl b on a.mmrrno=b.mmrrno left join mmasttbl c " &
                                                  "on a.codeno=c.codeno where b.tc ='10' and b.status <> 'void' " &
                                                  "and b.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                                                  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and " &
                                                  "c.glacct = '" & cboFilter5.Text.Substring(0, 7) & "' and b.mov = '" & cboFilter3.Text.Substring(0, 3) & "' " &
                                                  "and b.plntno = '" & cboFilter2.Text.Substring(0, 3) & "' limit 1")
                        End Select

                End Select
        End Select


        If Not CBool(dt.Rows.Count) Then
            dgvMMperMatl.DataSource = Nothing
            dgvMMperMatl.DataBind()
            lblMsg.Text = "No " & cboFormat.Text & " Found from Dates selected"
            Exit Sub

        End If

        Call dt.Dispose()

        Dim adapter As New MySqlDataAdapter
        Dim ds As New DataSet()
        sqldata = Nothing

        Select Case cboFilter5.Text 'mm type
            Case "ALL"
                Select Case cboFilter2.Text
                    Case "ALL"
                        Select Case cboFilter3.Text
                            Case "ALL"
                                sqldata = "select distinct b.venno,d.venname,b.mmrrno,b.transdate,a.codeno," &
                                          "ifnull(c.codename,c.mmdesc) as mmdesc,a.qty,a.wt,a.sp,a.itmamt as amt,a.lotno,b.mov,b.plntno " &
                                          "from invdettbl a left join invhdrtbl b on a.mmrrno=b.mmrrno left join mmasttbl c on a.codeno=c.codeno " &
                                          "left join venmasttbl d on b.venno=d.venno where b.status <> 'void' " &
                                          "and b.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                                          "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and b.tc = '10' order by b.transdate,b.mmrrno"
                            Case Else
                                sqldata = "select distinct b.venno,d.venname,b.mmrrno,b.transdate,a.codeno," &
                                          "ifnull(c.codename,c.mmdesc) as mmdesc,a.qty,a.wt,a.sp,a.itmamt as amt,a.lotno,b.mov,b.plntno " &
                                          "from invdettbl a left join invhdrtbl b on a.mmrrno=b.mmrrno left join mmasttbl c on a.codeno=c.codeno " &
                                          "left join venmasttbl d on b.venno=d.venno where b.mov = '" & cboFilter3.Text.Substring(0, 3) & "' " &
                                          "and b.status <> 'void' and b.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                                          "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and b.tc = '10' order by b.transdate,b.mmrrno"
                        End Select
                    Case Else
                        'per plant
                        Select Case cboFilter3.Text
                            Case "ALL"
                                sqldata = "select distinct b.venno,d.venname,b.mmrrno,b.transdate,a.codeno," &
                                          "ifnull(c.codename,c.mmdesc) as mmdesc,a.qty,a.wt,a.sp,a.itmamt as amt,a.lotno,b.mov,b.plntno " &
                                          "from invdettbl a left join invhdrtbl b on a.mmrrno=b.mmrrno left join mmasttbl c on a.codeno=c.codeno " &
                                          "left join venmasttbl d on b.venno=d.venno where b.tc = '10' and b.status <> 'void' " &
                                          "and b.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                                          "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and b.plntno = '" & cboFilter2.Text.Substring(0, 3) & "' " &
                                          "order by b.transdate,b.mmrrno"
                            Case Else
                                sqldata = "select distinct b.venno,d.venname,b.mmrrno,b.transdate,a.codeno," &
                                          "ifnull(c.codename,c.mmdesc) as mmdesc,a.qty,a.wt,a.sp,a.itmamt as amt,a.lotno,b.mov,b.plntno " &
                                          "from invdettbl a left join invhdrtbl b on a.mmrrno=b.mmrrno left join mmasttbl c on a.codeno=c.codeno " &
                                          "left join venmasttbl d on b.venno=d.venno where b.mov = '" & cboFilter3.Text.Substring(0, 3) & "' " &
                                          "and b.status <> 'void' and tc = '10' " &
                                          "and b.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                                          "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "'and " &
                                          " and b.plntno = '" & cboFilter2.Text.Substring(0, 3) & "' order by b.transdate,b.mmrrno"
                        End Select

                End Select
            Case Else
                Select Case cboFilter2.Text
                    Case "ALL"
                        Select Case cboFilter3.Text
                            Case "ALL"
                                sqldata = "select distinct b.venno,d.venname,b.mmrrno,b.transdate,a.codeno," &
                                          "ifnull(c.codename,c.mmdesc) as mmdesc,a.qty,a.wt,a.sp,a.itmamt as amt,a.lotno,b.mov,b.plntno " &
                                          "from invdettbl a left join invhdrtbl b on a.mmrrno=b.mmrrno left join mmasttbl c on a.codeno=c.codeno " &
                                          "left join venmasttbl d on b.venno=d.venno where c.glacct = '" & cboFilter5.Text.Substring(0, 7) & "' and b.status <> 'void' " &
                                          "and b.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                                          "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and b.tc = '10' order by b.transdate,b.mmrrno"
                            Case Else
                                sqldata = "select distinct b.venno,d.venname,b.mmrrno,b.transdate,a.codeno," &
                                          "ifnull(c.codename,c.mmdesc) as mmdesc,a.qty,a.wt,a.sp,a.itmamt as amt,a.lotno,b.mov,b.plntno " &
                                          "from invdettbl a left join invhdrtbl b on a.mmrrno=b.mmrrno left join mmasttbl c on a.codeno=c.codeno " &
                                          "left join venmasttbl d on b.venno=d.venno where c.glacct = '" & cboFilter5.Text.Substring(0, 7) & "' and " &
                                          "b.mov = '" & cboFilter3.Text.Substring(0, 3) & "' and b.status <> 'void' and " &
                                          "b.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                                          "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and b.tc = '10' order by b.transdate,b.mmrrno"
                        End Select
                    Case Else
                        'per plant
                        Select Case cboFilter3.Text
                            Case "ALL"
                                sqldata = "select distinct b.venno,d.venname,b.mmrrno,b.transdate,a.codeno," &
                                          "ifnull(c.codename,c.mmdesc) as mmdesc,a.qty,a.wt,a.sp,a.itmamt as amt,a.lotno,b.mov,b.plntno " &
                                          "from invdettbl a left join invhdrtbl b on a.mmrrno=b.mmrrno left join mmasttbl c on a.codeno=c.codeno " &
                                          "left join venmasttbl d on b.venno=d.venno where c.glacct = '" & cboFilter5.Text.Substring(0, 7) & "' and b.tc = '10' and b.status <> 'void' " &
                                          "and b.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                                          "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and b.plntno = '" & cboFilter2.Text.Substring(0, 3) & "' " &
                                          "order by b.transdate,b.mmrrno"
                            Case Else
                                sqldata = "select distinct b.venno,d.venname,b.mmrrno,b.transdate,a.codeno," &
                                          "ifnull(c.codename,c.mmdesc) as mmdesc,a.qty,a.wt,a.sp,a.itmamt as amt,a.lotno,b.mov,b.plntno " &
                                          "from invdettbl a left join invhdrtbl b on a.mmrrno=b.mmrrno left join mmasttbl c on a.codeno=c.codeno " &
                                          "left join venmasttbl d on b.venno=d.venno where c.glacct = '" & cboFilter5.Text.Substring(0, 7) & "' " &
                                          "and b.mov = '" & cboFilter3.Text.Substring(0, 3) & "' " &
                                          "and b.status <> 'void' and tc = '10' " &
                                          "and b.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                                          "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "'and " &
                                          " and b.plntno = '" & cboFilter2.Text.Substring(0, 3) & "' order by b.transdate,b.mmrrno"
                        End Select

                End Select
        End Select

        With dgvMMperMatl
            .Columns(0).HeaderText = "Ven No"
            .Columns(1).HeaderText = "Vendor"
            .Columns(2).HeaderText = "Doc No."
            .Columns(3).HeaderText = "Date"
            .Columns(4).HeaderText = "Code No."
            .Columns(5).HeaderText = "Description"
            .Columns(6).HeaderText = "Qty"
            .Columns(7).HeaderText = "Wt/Vol"
            .Columns(8).HeaderText = "UC"
            .Columns(9).HeaderText = "Amount"
            .Columns(10).HeaderText = "Lot No."
            .Columns(11).HeaderText = "Mov"
            .Columns(12).HeaderText = "Plnt No."

            Select Case cboFilter5.Text.Substring(0, 7)
                Case "1131100"
                    .Columns(6).Visible = False
                    .Columns(7).Visible = True
                Case "1131130"
                    .Columns(6).Visible = True
                    .Columns(7).Visible = False
                Case "1131120", "1131170", "1131180"
                    .Columns(6).Visible = True
                    .Columns(7).Visible = True
                Case Else
                    .Columns(6).Visible = True
                    .Columns(7).Visible = True
            End Select

        End With

        conn.Open()
        Dim command As New MySqlCommand(sqldata, conn)
        adapter.SelectCommand = command
        adapter.Fill(ds)
        adapter.Dispose()
        command.Dispose()
        conn.Close()
        dgvMMperMatl.DataSource = ds.Tables(0)
        dgvMMperMatl.DataBind()

    End Sub

    Private Sub rptMMRR01()
        PPRegDet.Visible = False
        PregSum.Visible = True

        Select Case cboFormat.Text
            Case "MMRR Register - Summary"
                Select Case cboFilter2.Text 'plant
                    Case "ALL"
                        Select Case cboFilter3.Text 'mov type
                            Case "ALL"
                                If CheckBox3.Checked = True Then
                                    dt = GetDataTable("select * from invhdrtbl where status = 'void' and tc ='10' and " &
                                                      "transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                                      "and  '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and " &
                                                      "mov between '101' and '107' limit 1")
                                Else
                                    dt = GetDataTable("select * from invhdrtbl where status <> 'void' and tc ='10' and " &
                                                      "transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                                      "and  '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and " &
                                                      "mov between '101' and '107' limit 1")
                                End If

                            Case Else
                                'per mov type
                                If CheckBox3.Checked = True Then
                                    dt = GetDataTable("select * from invhdrtbl where status = 'void' and tc ='10' and " &
                                                      "transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                                      "and  '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and " &
                                                      "mov = '" & cboFilter3.Text.Substring(0, 3) & "' and " &
                                                      "mov between '101' and '107' limit 1")
                                Else
                                    dt = GetDataTable("select * from invhdrtbl where status <> 'void' and tc ='10' and " &
                                                      "transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                                      "and  '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and " &
                                                      "mov = '" & cboFilter3.Text.Substring(0, 3) & "' limit 1")
                                End If
                        End Select

                    Case Else
                        Select Case cboFilter3.Text
                            Case "ALL"
                                If CheckBox3.Checked = True Then
                                    dt = GetDataTable("select * from invhdrtbl where status = 'void' and tc ='10' and " &
                                                      "transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                                      "and  '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and " &
                                                      "mov = '" & cboFilter3.Text.Substring(0, 3) & "' and " &
                                                      "plntno = '" & cboFilter2.Text.Substring(0, 3) & "' and " &
                                                      "mov between '101' and '107' limit 1")
                                Else
                                    dt = GetDataTable("select * from invhdrtbl where status <> 'void' and tc ='10' and " &
                                                      "transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                                      "and  '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and " &
                                                      "mov between '101' and '107' and " &
                                                      "plntno = '" & cboFilter2.Text.Substring(0, 3) & "' limit 1")
                                End If

                            Case Else
                                'per mov type
                                If CheckBox3.Checked = True Then
                                    dt = GetDataTable("select * from invhdrtbl where status = 'void' and tc ='10' and " &
                                                      "transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                                      "and  '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and " &
                                                      "mov = '" & cboFilter3.Text.Substring(0, 3) & "' and " &
                                                      "plntno = '" & cboFilter2.Text.Substring(0, 3) & "' and " &
                                                      "mov = '" & cboFilter3.Text.Substring(0, 3) & "' limit 1")
                                Else
                                    dt = GetDataTable("select * from invhdrtbl where status <> 'void' and tc ='10' and " &
                                                      "transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                                      "and  '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and " &
                                                      "mov = '" & cboFilter3.Text.Substring(0, 3) & "' and " &
                                                      "plntno = '" & cboFilter2.Text.Substring(0, 3) & "' and " &
                                                      "mov = '" & cboFilter3.Text.Substring(0, 3) & "' limit 1")
                                End If
                        End Select
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
            Case "MMRR Register - Summary"
                Select Case cboFilter2.Text 'plant
                    Case "ALL"
                        Select Case cboFilter3.Text 'mov type
                            Case "ALL"
                                If CheckBox3.Checked = True Then
                                    sqldata = "select c.mmrrno as docno,c.tc,c.transdate,concat(c.venno,space(1),d.venname) as venno," &
                                              "sum(ifnull(e.qty,0)) as qty,sum(ifnull(e.wt,0)) as wt,sum(ifnull(e.itmamt,0)) as amt," &
                                              "concat(c.plntno,space(1),b.plntname) as plntno,concat(c.mov,space(1),a.movdesc) as mov from invdettbl e " &
                                              "left join invhdrtbl c on e.mmrrno=c.mmrrno left join venmasttbl d on c.venno=d.venno " &
                                              "left join plnttbl b on c.plntno = b.plntno left join movtbl a on a.mov = c.mov where " &
                                              "c.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                                              "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and c.status = 'void' " &
                                              "and c.tc ='10' and c.mov between '101' and '107' group by e.mmrrno order by c.mmrrno"
                                Else
                                    sqldata = "select c.mmrrno as docno,c.tc,c.transdate,concat(c.venno,space(1),d.venname) as venno," &
                                              "sum(ifnull(e.qty,0)) as qty,sum(ifnull(e.wt,0)) as wt,sum(ifnull(e.itmamt,0)) as amt," &
                                              "concat(c.plntno,space(1),b.plntname) as plntno,concat(c.mov,space(1),a.movdesc) as mov from invdettbl e " &
                                              "left join invhdrtbl c on e.mmrrno=c.mmrrno left join venmasttbl d on c.venno=d.venno " &
                                              "left join plnttbl b on c.plntno = b.plntno left join movtbl a on a.mov = c.mov where c.transdate " &
                                              "between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                                              "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and c.status <> 'void' " &
                                              "and c.tc ='10' and c.mov between '101' and '107' group by e.mmrrno order by c.mmrrno"
                                End If

                            Case Else
                                'per mov type
                                If CheckBox3.Checked = True Then
                                    sqldata = "select c.mmrrno as docno,c.tc,c.transdate,concat(c.venno,space(1),d.venname) as venno," &
                                              "sum(ifnull(e.qty,0)) as qty,sum(ifnull(e.wt,0)) as wt,sum(ifnull(e.itmamt,0)) as amt," &
                                              "concat(c.plntno,space(1),b.plntname) as plntno,concat(c.mov,space(1),a.movdesc) as mov from invdettbl e " &
                                              "left join invhdrtbl c on e.mmrrno=c.mmrrno left join venmasttbl d on c.venno=d.venno " &
                                              "left join plnttbl b on c.plntno = b.plntno left join movtbl a on a.mov = c.mov where c.transdate " &
                                              "between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and  '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' " &
                                              "and c.status = 'void' and c.tc ='10' and c.mov = '" & cboFilter3.Text.Substring(0, 3) & "' group by e.mmrrno order by c.mmrrno"
                                Else
                                    sqldata = "select c.mmrrno as docno,c.tc,c.transdate,concat(c.venno,space(1),d.venname) as venno," &
                                              "sum(ifnull(e.qty,0)) as qty,sum(ifnull(e.wt,0)) as wt,sum(ifnull(e.itmamt,0)) as amt," &
                                              "concat(c.plntno,space(1),b.plntname) as plntno,concat(c.mov,space(1),a.movdesc) as mov from invdettbl e " &
                                              "left join invhdrtbl c on e.mmrrno=c.mmrrno left join venmasttbl d on c.venno=d.venno " &
                                              "left join plnttbl b on c.plntno = b.plntno left join movtbl a on a.mov = c.mov where c.transdate " &
                                              "between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and  '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' " &
                                              "and c.status <> 'void' and c.tc ='10' and c.mov = '" & cboFilter3.Text.Substring(0, 3) & "' group by e.mmrrno order by c.mmrrno"
                                End If
                        End Select

                    Case Else
                        'per plnt no
                        Select Case cboFilter3.Text
                            Case "ALL"
                                If CheckBox3.Checked = True Then
                                    sqldata = "select c.mmrrno as docno,c.tc,c.transdate,concat(c.venno,space(1),d.venname) as venno," &
                                              "sum(ifnull(e.qty,0)) as qty,sum(ifnull(e.wt,0)) as wt,sum(ifnull(e.itmamt,0)) as amt," &
                                              "concat(c.plntno,space(1),b.plntname) as plntno,concat(c.mov,space(1),a.movdesc) as mov from invdettbl e " &
                                              "left join invhdrtbl c on e.mmrrno=c.mmrrno left join venmasttbl d on c.venno=d.venno " &
                                              "left join plnttbl b on c.plntno = b.plntno left join movtbl a on a.mov = c.mov where c.transdate " &
                                              "between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and  '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' " &
                                              "and c.status = 'void' and c.tc ='10' and c.plntno = '" & cboFilter2.Text.Substring(0, 3) & "' " &
                                              "and c.mov between '101' and '107' group by e.mmrrno order by c.mmrrno"
                                Else
                                    sqldata = "select c.mmrrno as docno,c.tc,c.transdate,concat(c.venno,space(1),d.venname) as venno," &
                                              "sum(ifnull(e.qty,0)) as qty,sum(ifnull(e.wt,0)) as wt,sum(ifnull(e.itmamt,0)) as amt," &
                                              "concat(c.plntno,space(1),b.plntname) as plntno,concat(c.mov,space(1),a.movdesc) as mov from invdettbl e " &
                                              "left join invhdrtbl c on e.mmrrno=c.mmrrno left join venmasttbl d on c.venno=d.venno " &
                                              "left join plnttbl b on c.plntno = b.plntno left join movtbl a on a.mov = c.mov where c.transdate " &
                                              "between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and  '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' " &
                                              "and c.status <> 'void' and c.tc ='10' and c.plntno = '" & cboFilter2.Text.Substring(0, 3) & "' " &
                                              "and c.mov between '101' and '107' group by e.mmrrno order by c.mmrrno"
                                End If

                            Case Else
                                'per mov type
                                If CheckBox3.Checked = True Then
                                    sqldata = "select c.mmrrno as docno,c.tc,c.transdate,concat(c.venno,space(1),d.venname) as venno," &
                                              "sum(ifnull(e.qty,0)) as qty,sum(ifnull(e.wt,0)) as wt,sum(ifnull(e.itmamt,0)) as amt," &
                                              "concat(c.plntno,space(1),b.plntname) as plntno,concat(c.mov,space(1),a.movdesc) as mov from invdettbl e " &
                                              "left join invhdrtbl c on e.mmrrno=c.mmrrno left join venmasttbl d on c.venno=d.venno " &
                                              "left join plnttbl b on c.plntno = b.plntno left join movtbl a on a.mov = c.mov where c.transdate " &
                                              "between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and  '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' " &
                                              "and c.status = 'void' and c.tc ='10' and c.plntno = '" & cboFilter2.Text.Substring(0, 3) & "' and " &
                                              "c.mov = '" & cboFilter3.Text.Substring(0, 3) & "' group by e.mmrrno order by c.mmrrno"
                                Else
                                    sqldata = "select c.mmrrno as docno,c.tc,c.transdate,concat(c.venno,space(1),d.venname) as venno," &
                                              "sum(ifnull(e.qty,0)) as qty,sum(ifnull(e.wt,0)) as wt,sum(ifnull(e.itmamt,0)) as amt," &
                                              "concat(c.plntno,space(1),b.plntname) as plntno,concat(c.mov,space(1),a.movdesc) as mov from invdettbl e " &
                                              "left join invhdrtbl c on e.mmrrno=c.mmrrno left join venmasttbl d on c.venno=d.venno " &
                                              "left join plnttbl b on c.plntno = b.plntno left join movtbl a on a.mov = c.mov where c.transdate " &
                                              "between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and  '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' " &
                                              "and c.status <> 'void' and c.tc ='10' and c.plntno = '" & cboFilter2.Text.Substring(0, 3) & "' and " &
                                              "c.mov = '" & cboFilter3.Text.Substring(0, 3) & "' group by e.mmrrno order by c.mmrrno"
                                End If
                        End Select
                End Select
        End Select

        With dgvRegSum
            .Columns(0).HeaderText = "MMRR No."
            .Columns(1).HeaderText = "TC."
            .Columns(2).HeaderText = "Date"
            .Columns(3).HeaderText = "Vendor"
            .Columns(4).HeaderText = "Qty"
            .Columns(5).HeaderText = "Wt/Vol"
            .Columns(6).HeaderText = "Amount"
            .Columns(7).HeaderText = "Plant"
            .Columns(8).HeaderText = "Movement Type"

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

    Private Sub rptMMRR02()
        lblMsg.Text = "Not yet available"



    End Sub

    Private Sub MMRRregVendor()
        PRpVD.Visible = True

        Select Case cboFilter5.Text
            Case "ALL"
                Select Case cboFilter2.Text
                    Case "ALL"
                        Select Case cboFilter3.Text
                            Case "ALL"
                                dt = GetDataTable("select * from invhdrtbl where mov between '101' and '104' and status <> 'void' " &
                                                  "and transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                                                  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' limit 1")
                            Case Else
                                dt = GetDataTable("select * from invhdrtbl where mov between '101' and '104' and status <> 'void' " &
                                                  "and transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                                                  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and " &
                                                  "venno = '" & cboFilter3.Text.Substring(0, 5) & "' limit 1")
                        End Select
                    Case Else
                        'per plant
                        Select Case cboFilter3.Text
                            Case "ALL"
                                dt = GetDataTable("select * from invhdrtbl where mov between '101' and '104' and status <> 'void' " &
                                                  "and transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                                                  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and " &
                                                  "plntno = '" & cboFilter2.Text.Substring(0, 3) & "' limit 1")
                            Case Else
                                dt = GetDataTable("select * from invhdrtbl where mov between '101' and '104' and status <> 'void' " &
                                                  "and transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                                                  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and " &
                                                  "venno = '" & cboFilter3.Text.Substring(0, 5) & "'and " &
                                                  "plntno = '" & cboFilter2.Text.Substring(0, 3) & "' limit 1")
                        End Select

                End Select
            Case Else
                Select Case cboFilter2.Text
                    Case "ALL"
                        Select Case cboFilter3.Text
                            Case "ALL"
                                dt = GetDataTable("select * from invdettbl a left join invhdrtbl b on a.mmrrno=b.mmrrno " &
                                                  "left join mmasttbl c on a.codeno=c.codeno where b.mov between '101' and '104' and b.status <> 'void' " &
                                                  "and b.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                                                  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and c.mmtype = '" & cboFilter5.Text & "' limit 1")
                            Case Else
                                dt = GetDataTable("select * from invdettbl a left join invhdrtbl b on a.mmrrno=b.mmrrno " &
                                                  "left join mmasttbl c on a.codeno=c.codeno where b.mov between '101' and '104' and b.status <> 'void' " &
                                                  "and b.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                                                  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and " &
                                                  "b.venno = '" & cboFilter3.Text.Substring(0, 5) & "' and c.mmtype = '" & cboFilter5.Text & "' limit 1")
                        End Select
                    Case Else
                        'per plant
                        Select Case cboFilter3.Text
                            Case "ALL"
                                dt = GetDataTable("select * from invdettbl a left join invhdrtbl b on a.mmrrno=b.mmrrno " &
                                                  "left join mmasttbl c on a.codeno=c.codeno where b.mov between '101' and '104' and b.status <> 'void' " &
                                                  "and b.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                                                  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and c.mmtype = '" & cboFilter5.Text & "' and " &
                                                  "b.plntno = '" & cboFilter2.Text.Substring(0, 3) & "' limit 1")
                            Case Else
                                dt = GetDataTable("select * from invdettbl a left join invhdrtbl b on a.mmrrno=b.mmrrno " &
                                                  "left join mmasttbl c on a.codeno=c.codeno where b.mov between '101' and '104' and b.status <> 'void' " &
                                                  "and b.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                                                  "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and " &
                                                  "b.venno = '" & cboFilter3.Text.Substring(0, 5) & "' and c.mmtype = '" & cboFilter5.Text & "' and " &
                                                  "b.plntno = '" & cboFilter2.Text.Substring(0, 3) & "' limit 1")
                        End Select

                End Select
        End Select


        If Not CBool(dt.Rows.Count) Then
            dgvPRpVD.DataSource = Nothing
            dgvPRpVD.DataBind()
            lblMsg.Text = "No " & cboFormat.Text & " Found from Dates selected"
            Exit Sub

        End If

        Call dt.Dispose()

        Dim adapter As New MySqlDataAdapter
        Dim ds As New DataSet()
        sqldata = Nothing

        Select Case cboFilter5.Text
            Case "ALL"
                Select Case cboFilter2.Text
                    Case "ALL"
                        Select Case cboFilter3.Text
                            Case "ALL"
                                sqldata = "select distinct b.venno,d.venname,b.mmrrno,b.transdate,a.codeno," &
                                          "ifnull(c.codename,c.mmdesc) as mmdesc,a.qty,a.wt,a.sp,a.itmamt as amt,a.lotno,b.mov,b.plntno " &
                                          "from invdettbl a left join invhdrtbl b on a.mmrrno=b.mmrrno left join mmasttbl c on a.codeno=c.codeno " &
                                          "left join venmasttbl d on b.venno=d.venno where b.mov between '101' and '104' and b.status <> 'void' " &
                                          "and b.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                                          "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' order by b.venno,b.transdate,b.mmrrno"
                            Case Else
                                sqldata = "select distinct b.venno,d.venname,b.mmrrno,b.transdate,a.codeno," &
                                          "ifnull(c.codename,c.mmdesc) as mmdesc,a.qty,a.wt,a.sp,a.itmamt as amt,a.lotno,b.mov,b.plntno " &
                                          "from invdettbl a left join invhdrtbl b on a.mmrrno=b.mmrrno left join mmasttbl c on a.codeno=c.codeno " &
                                          "left join venmasttbl d on b.venno=d.venno where b.mov between '101' and '104' and b.status <> 'void' " &
                                          "and b.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                                          "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "'  and b.venno = '" & cboFilter3.Text.Substring(0, 5) & "' " &
                                          "order by b.transdate,b.mmrrno"
                        End Select
                    Case Else
                        'per plant
                        Select Case cboFilter3.Text
                            Case "ALL"
                                sqldata = "select distinct b.venno,d.venname,b.mmrrno,b.transdate,a.codeno," &
                                          "ifnull(c.codename,c.mmdesc) as mmdesc,a.qty,a.wt,a.sp,a.itmamt as amt,a.lotno,b.mov,b.plntno " &
                                          "from invdettbl a left join invhdrtbl b on a.mmrrno=b.mmrrno left join mmasttbl c on a.codeno=c.codeno " &
                                          "left join venmasttbl d on b.venno=d.venno where b.mov between '101' and '104' and b.status <> 'void' " &
                                          "and b.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                                          "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and b.plntno = '" & cboFilter2.Text.Substring(0, 3) & "' " &
                                          "order by b.venno,b.transdate,b.mmrrno"
                            Case Else
                                sqldata = "select distinct b.venno,d.venname,b.mmrrno,b.transdate,a.codeno," &
                                          "ifnull(c.codename,c.mmdesc) as mmdesc,a.qty,a.wt,a.sp,a.itmamt as amt,a.lotno,b.mov,b.plntno " &
                                          "from invdettbl a left join invhdrtbl b on a.mmrrno=b.mmrrno left join mmasttbl c on a.codeno=c.codeno " &
                                          "left join venmasttbl d on b.venno=d.venno where b.mov between '101' and '104' and b.status <> 'void' " &
                                          "and b.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                                          "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "'and b.venno = '" & cboFilter3.Text.Substring(0, 5) & "' " &
                                          " and b.plntno = '" & cboFilter2.Text.Substring(0, 3) & "' order by b.transdate,b.mmrrno"
                        End Select

                End Select
            Case Else
                Select Case cboFilter2.Text
                    Case "ALL"
                        Select Case cboFilter3.Text
                            Case "ALL"
                                sqldata = "select distinct b.venno,d.venname,b.mmrrno,b.transdate,a.codeno," &
                                          "ifnull(c.codename,c.mmdesc) as mmdesc,a.qty,a.wt,a.sp,a.itmamt as amt,a.lotno,b.mov,b.plntno " &
                                          "from invdettbl a left join invhdrtbl b on a.mmrrno=b.mmrrno left join mmasttbl c on a.codeno=c.codeno " &
                                          "left join venmasttbl d on b.venno=d.venno where b.mov between '101' and '104' and b.status <> 'void' " &
                                          "and b.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                                          "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and c.mmtype = '" & cboFilter5.Text & "' " &
                                          "order by b.venno,b.transdate,b.mmrrno"
                            Case Else
                                sqldata = "select distinct b.venno,d.venname,b.mmrrno,b.transdate,a.codeno," &
                                          "ifnull(c.codename,c.mmdesc) as mmdesc,a.qty,a.wt,a.sp,a.itmamt as amt,a.lotno,b.mov,b.plntno " &
                                          "from invdettbl a left join invhdrtbl b on a.mmrrno=b.mmrrno left join mmasttbl c on a.codeno=c.codeno " &
                                          "left join venmasttbl d on b.venno=d.venno where b.mov between '101' and '104' and b.status <> 'void' " &
                                          "and b.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                                          "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and b.venno = '" & cboFilter3.Text.Substring(0, 5) & "' " &
                                          "and c.mmtype = '" & cboFilter5.Text & "' order by b.transdate,b.mmrrno"
                        End Select
                    Case Else
                        'per plant
                        Select Case cboFilter3.Text
                            Case "ALL"
                                sqldata = "select distinct b.venno,d.venname,b.mmrrno,b.transdate,a.codeno," &
                                          "ifnull(c.codename,c.mmdesc) as mmdesc,a.qty,a.wt,a.sp,a.itmamt as amt,a.lotno,b.mov,b.plntno " &
                                          "from invdettbl a left join invhdrtbl b on a.mmrrno=b.mmrrno left join mmasttbl c on a.codeno=c.codeno " &
                                          "left join venmasttbl d on b.venno=d.venno where b.mov between '101' and '104' and b.status <> 'void' " &
                                          "and b.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                                          "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and b.plntno = '" & cboFilter2.Text.Substring(0, 3) & "' " &
                                          "and c.mmtype = '" & cboFilter5.Text & "' order by b.venno,b.transdate,b.mmrrno"
                            Case Else
                                sqldata = "select distinct b.venno,d.venname,b.mmrrno,b.transdate,a.codeno," &
                                          "ifnull(c.codename,c.mmdesc) as mmdesc,a.qty,a.wt,a.sp,a.itmamt as amt,a.lotno,b.mov,b.plntno " &
                                          "from invdettbl a left join invhdrtbl b on a.mmrrno=b.mmrrno left join mmasttbl c on a.codeno=c.codeno " &
                                          "left join venmasttbl d on b.venno=d.venno where b.mov between '101' and '104' and b.status <> 'void' " &
                                          "and b.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                                          "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "'and b.venno = '" & cboFilter3.Text.Substring(0, 5) & "' " &
                                          " and b.plntno = '" & cboFilter2.Text.Substring(0, 3) & "' and c.mmtype = '" & cboFilter5.Text & "' order by b.transdate,b.mmrrno"
                        End Select
                End Select
        End Select

        With dgvPRpVD
            .Columns(0).HeaderText = "Ven No"
            .Columns(1).HeaderText = "Vendor"
            .Columns(2).HeaderText = "Doc No."
            .Columns(3).HeaderText = "Date"
            .Columns(4).HeaderText = "Code No."
            .Columns(5).HeaderText = "Description"
            .Columns(6).HeaderText = "Qty"
            .Columns(7).HeaderText = "Wt/Vol"
            .Columns(8).HeaderText = "UC"
            .Columns(9).HeaderText = "Amount"
            .Columns(10).HeaderText = "Lot No."
            .Columns(11).HeaderText = "Mov"
            .Columns(12).HeaderText = "Plnt No."

            Select Case cboFilter5.Text
                Case "Raw Materials"
                    .Columns(6).Visible = False
                    .Columns(7).Visible = True
                Case "Packaging"
                    .Columns(6).Visible = True
                    .Columns(7).Visible = False
                Case "Finished Goods"
                    .Columns(6).Visible = True
                    .Columns(7).Visible = True
                Case Else
                    .Columns(6).Visible = True
                    .Columns(7).Visible = True
            End Select

        End With

        conn.Open()
        Dim command As New MySqlCommand(sqldata, conn)
        adapter.SelectCommand = command
        adapter.Fill(ds)
        adapter.Dispose()
        command.Dispose()
        conn.Close()
        dgvPRpVD.DataSource = ds.Tables(0)
        dgvPRpVD.DataBind()


    End Sub

    Private Sub prodnReg01()
        PregSum.Visible = False
        PPRegDet.Visible = True

        Select Case cboFilter4.Text
            Case "ALL"
                Select Case cboFormat.Text
                    Case "Production Register - Detailed"
                        Select Case cboFilter2.Text
                            Case "ALL"
                                Select Case cboFilter3.Text
                                    Case "ALL"
                                        dt = GetDataTable("select * from invhdrtbl where mov between '801' and '803' and " &
                                                          "transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                                          "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and  " &
                                                          "dsrstat <> 'void'  limit 1")
                                    Case Else
                                        'per mov type
                                        dt = GetDataTable("select * from invhdrtbl where mov = '" & cboFilter3.Text.Substring(0, 3) & "' and " &
                                                          "transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                                          "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and  " &
                                                          "dsrstat <> 'void' limit 1")
                                End Select

                            Case Else
                                'per plnt no
                                Select Case cboFilter3.Text
                                    Case "ALL"
                                        dt = GetDataTable("select * from invhdrtbl where mov between '801' and '803' and " &
                                                          "transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                                          "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and  " &
                                                          "dsrstat <> 'void' and plntno = '" & cboFilter2.Text.Substring(0, 3) & "' limit 1")

                                    Case Else
                                        'per mov type
                                        dt = GetDataTable("select * from invhdrtbl where mov = '" & cboFilter3.Text.Substring(0, 3) & "' and " &
                                                          "transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                                          "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and  " &
                                                          "dsrstat <> 'void' and plntno = '" & cboFilter2.Text.Substring(0, 3) & "' limit 1")
                                End Select

                        End Select

                    Case "Production Register - VOID"
                        Select Case cboFilter2.Text
                            Case "ALL"
                                Select Case cboFilter3.Text
                                    Case "ALL"
                                    Case "ALL"
                                        dt = GetDataTable("select * from invhdrtbl where mov between '801' and '803' and " &
                                                          "transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                                          "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and  " &
                                                          "dsrstat = 'void'  limit 1")
                                    Case Else
                                        'per mov type
                                        dt = GetDataTable("select * from invhdrtbl where mov = '" & cboFilter3.Text.Substring(0, 3) & "' and " &
                                                          "transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                                          "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and  " &
                                                          "dsrstat = 'void' limit 1")
                                End Select

                            Case Else
                                'per plnt no
                                Select Case cboFilter3.Text
                                    Case "ALL"
                                        dt = GetDataTable("select * from invhdrtbl where mov between '801' and '803' and " &
                                                          "transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                                          "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and  " &
                                                          "dsrstat = 'void' and plntno = '" & cboFilter2.Text.Substring(0, 3) & "' limit 1")

                                    Case Else
                                        'per mov type
                                        dt = GetDataTable("select * from invhdrtbl where mov = '" & cboFilter3.Text.Substring(0, 3) & "' and " &
                                                          "transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                                          "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and  " &
                                                          "dsrstat = 'void' and plntno = '" & cboFilter2.Text.Substring(0, 3) & "' limit 1")
                                End Select

                        End Select
                End Select

            Case Else
                Select Case cboFormat.Text
                    Case "Production Register - Detailed"
                        Select Case cboFilter2.Text
                            Case "ALL"
                                Select Case cboFilter3.Text
                                    Case "ALL"
                                        dt = GetDataTable("select * from invhdrtbl where mov between '801' and '803' and " &
                                                          "transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                                          "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and  " &
                                                          "dsrstat = '" & cboFilter4.Text & "' limit 1")
                                    Case Else
                                        'per mov type
                                        dt = GetDataTable("select * from invhdrtbl where mov = '" & cboFilter3.Text.Substring(0, 3) & "' and " &
                                                          "transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                                          "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and  " &
                                                          "dsrstat = '" & cboFilter4.Text & "' limit 1")
                                End Select

                            Case Else
                                'per plnt no
                                Select Case cboFilter3.Text
                                    Case "ALL"
                                        dt = GetDataTable("select * from invhdrtbl where mov between '801' and '803' and " &
                                                          "transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                                          "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and  " &
                                                          "plntno = '" & cboFilter2.Text.Substring(0, 3) & "' " &
                                                          "and dsrstat = '" & cboFilter4.Text & "' limit 1")

                                    Case Else
                                        'per mov type
                                        dt = GetDataTable("select * from invhdrtbl where mov = '" & cboFilter3.Text.Substring(0, 3) & "' and " &
                                                          "transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                                          "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and  " &
                                                          "plntno = '" & cboFilter2.Text.Substring(0, 3) & "' " &
                                                          "and dsrstat = '" & cboFilter4.Text & "' limit 1")
                                End Select

                        End Select

                    Case "Production Register - VOID"
                        Select Case cboFilter2.Text
                            Case "ALL"
                                Select Case cboFilter3.Text
                                    Case "ALL"
                                        dt = GetDataTable("select * from invhdrtbl where mov between '801' and '803' and " &
                                                          "transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                                          "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and  " &
                                                          "dsrstat = '" & cboFilter4.Text & "' limit 1")
                                    Case Else
                                        'per mov type
                                        dt = GetDataTable("select * from invhdrtbl where mov = '" & cboFilter3.Text.Substring(0, 3) & "' and " &
                                                          "transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                                          "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and  " &
                                                          "dsrstat = '" & cboFilter4.Text & "' limit 1")
                                End Select

                            Case Else
                                'per plnt no
                                Select Case cboFilter3.Text
                                    Case "ALL"
                                        dt = GetDataTable("select * from invhdrtbl where mov between '801' and '803' and " &
                                                          "transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                                          "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and  " &
                                                          "plntno = '" & cboFilter2.Text.Substring(0, 3) & "' " &
                                                          "and dsrstat = '" & cboFilter4.Text & "' limit 1")

                                    Case Else
                                        'per mov type
                                        dt = GetDataTable("select * from invhdrtbl where mov = '" & cboFilter3.Text.Substring(0, 3) & "' and " &
                                                          "transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                                          "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and  " &
                                                          "plntno = '" & cboFilter2.Text.Substring(0, 3) & "' " &
                                                          "and dsrstat = '" & cboFilter4.Text & "' limit 1")
                                End Select
                        End Select
                End Select
        End Select

        If Not CBool(dt.Rows.Count) Then
            dgvProdRegDet.DataSource = Nothing
            dgvProdRegDet.DataBind()
            lblMsg.Text = "No " & cboFormat.Text & " Found from Dates selected"
            Exit Sub

        End If

        Call dt.Dispose()

        Dim adapter As New MySqlDataAdapter
        Dim ds As New DataSet()

        sqldata = Nothing

        Select Case cboFilter4.Text
            Case "ALL"
                Select Case cboFormat.Text
                    Case "Production Register - Detailed"
                        Select Case cboFilter2.Text
                            Case "ALL"
                                Select Case cboFilter3.Text
                                    Case "ALL"
                                        sqldata = "select distinct concat(c.plntno,space(1),d.plntname) as plntno,c.mmrrno,c.transdate,a.codeno," &
                                                  "b.mmdesc,a.qty,a.wt,a.lotno,c.batchno,c.dsrstat from invdettbl a " &
                                                  "left join mmasttbl b on a.codeno=b.codeno left join invhdrtbl c on a.mmrrno=c.mmrrno " &
                                                  "left join plnttbl d on c.plntno=d.plntno where c.mov between '801' and '803' and " &
                                                  "c.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                                  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and  " &
                                                  "c.dsrstat <> 'void' order by c.mmrrno,c.batchno"
                                    Case Else
                                        'per mov type
                                        sqldata = "select distinct concat(c.plntno,space(1),d.plntname) as plntno,c.mmrrno,c.transdate,a.codeno," &
                                                  "b.mmdesc,a.qty,a.wt,a.lotno,c.batchno,c.dsrstat from invdettbl a " &
                                                  "left join mmasttbl b on a.codeno=b.codeno left join invhdrtbl c on a.mmrrno=c.mmrrno " &
                                                  "left join plnttbl d on c.plntno=d.plntno where c.mov = '" & cboFilter3.Text.Substring(0, 3) & "' " &
                                                  "and c.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                                  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and  " &
                                                  "c.dsrstat <> 'void' order by c.mmrrno,c.batchno"

                                End Select

                            Case Else
                                'per plnt no
                                Select Case cboFilter3.Text
                                    Case "ALL"
                                        sqldata = "select distinct concat(c.plntno,space(1),d.plntname) as plntno,c.mmrrno,c.transdate,a.codeno," &
                                                  "b.mmdesc,a.qty,a.wt,a.lotno,c.batchno,c.dsrstat from invdettbl a " &
                                                  "left join mmasttbl b on a.codeno=b.codeno left join invhdrtbl c on a.mmrrno=c.mmrrno " &
                                                  "left join plnttbl d on c.plntno=d.plntno where c.mov between '801' and '803' and " &
                                                  "c.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                                  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and  " &
                                                  "c.dsrstat <> 'void' and c.plntno = '" & cboFilter2.Text.Substring(0, 3) & "' order by c.mmrrno,c.batchno"
                                    Case Else
                                        'per mov type
                                        sqldata = "select distinct concat(c.plntno,space(1),d.plntname) as plntno,c.mmrrno,c.transdate,a.codeno," &
                                                  "b.mmdesc,a.qty,a.wt,a.lotno,c.batchno,c.dsrstat from invdettbl a " &
                                                  "left join mmasttbl b on a.codeno=b.codeno left join invhdrtbl c on a.mmrrno=c.mmrrno " &
                                                  "left join plnttbl d on c.plntno=d.plntno where c.mov = '" & cboFilter3.Text.Substring(0, 3) & "' " &
                                                  "and c.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                                  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and  " &
                                                  "c.dsrstat <> 'void' and c.plntno = '" & cboFilter2.Text.Substring(0, 3) & "' order by c.mmrrno,c.batchno"
                                End Select

                        End Select

                    Case "Production Register - VOID"
                        Select Case cboFilter2.Text
                            Case "ALL"
                                Select Case cboFilter3.Text
                                    Case "ALL"
                                        sqldata = "select distinct concat(c.plntno,space(1),d.plntname) as plntno,c.mmrrno,c.transdate,a.codeno," &
                                                  "b.mmdesc,a.qty,a.wt,a.lotno,c.batchno,c.dsrstat from invdettbl a " &
                                                  "left join mmasttbl b on a.codeno=b.codeno left join invhdrtbl c on a.mmrrno=c.mmrrno " &
                                                  "left join plnttbl d on c.plntno=d.plntno where c.mov between '801' and '803' and " &
                                                  "c.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                                  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and  " &
                                                  "c.dsrstat = 'void' order by c.mmrrno,c.batchno"
                                    Case Else
                                        'per mov type
                                        sqldata = "select distinct concat(c.plntno,space(1),d.plntname) as plntno,c.mmrrno,c.transdate,a.codeno," &
                                                  "b.mmdesc,a.qty,a.wt,a.lotno,c.batchno,c.dsrstat from invdettbl a " &
                                                  "left join mmasttbl b on a.codeno=b.codeno left join invhdrtbl c on a.mmrrno=c.mmrrno " &
                                                  "left join plnttbl d on c.plntno=d.plntno where c.mov = '" & cboFilter3.Text.Substring(0, 3) & "' " &
                                                  "and c.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                                  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and  " &
                                                  "c.dsrstat = 'void' order by c.mmrrno,c.batchno"

                                End Select

                            Case Else
                                'per plnt no
                                Select Case cboFilter3.Text
                                    Case "ALL"
                                        sqldata = "select distinct concat(c.plntno,space(1),d.plntname) as plntno,c.mmrrno,c.transdate,a.codeno," &
                                                  "b.mmdesc,a.qty,a.wt,a.lotno,c.batchno,c.dsrstat from invdettbl a " &
                                                  "left join mmasttbl b on a.codeno=b.codeno left join invhdrtbl c on a.mmrrno=c.mmrrno " &
                                                  "left join plnttbl d on c.plntno=d.plntno where c.mov between '801' and '803' and " &
                                                  "c.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                                  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and  " &
                                                  "c.dsrstat = 'void' and c.plntno = '" & cboFilter2.Text.Substring(0, 3) & "' order by c.mmrrno,c.batchno"
                                    Case Else
                                        'per mov type
                                        sqldata = "select distinct concat(c.plntno,space(1),d.plntname) as plntno,c.mmrrno,c.transdate,a.codeno," &
                                                  "b.mmdesc,a.qty,a.wt,a.lotno,c.batchno,c.dsrstat from invdettbl a " &
                                                  "left join mmasttbl b on a.codeno=b.codeno left join invhdrtbl c on a.mmrrno=c.mmrrno " &
                                                  "left join plnttbl d on c.plntno=d.plntno where c.mov = '" & cboFilter3.Text.Substring(0, 3) & "' " &
                                                  "and c.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                                  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and  " &
                                                  "c.dsrstat = 'void' and c.plntno = '" & cboFilter2.Text.Substring(0, 3) & "' order by c.mmrrno,c.batchno"
                                End Select
                        End Select
                End Select

            Case Else
                Select Case cboFormat.Text
                    Case "Production Register - Detailed"
                        Select Case cboFilter2.Text
                            Case "ALL"
                                Select Case cboFilter3.Text
                                    Case "ALL"
                                        sqldata = "select distinct concat(c.plntno,space(1),d.plntname) as plntno,c.mmrrno,c.transdate,a.codeno," &
                                                  "b.mmdesc,a.qty,a.wt,a.lotno,c.batchno,c.dsrstat from invdettbl a " &
                                                  "left join mmasttbl b on a.codeno=b.codeno left join invhdrtbl c on a.mmrrno=c.mmrrno " &
                                                  "left join plnttbl d on c.plntno=d.plntno where c.mov between '801' and '803' and " &
                                                  "c.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                                  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and  " &
                                                  "c.dsrstat = '" & cboFilter4.Text & "' order by c.mmrrno,c.batchno"
                                    Case Else
                                        'per mov type
                                        sqldata = "select distinct concat(c.plntno,space(1),d.plntname) as plntno,c.mmrrno,c.transdate,a.codeno," &
                                                  "b.mmdesc,a.qty,a.wt,a.lotno,c.batchno,c.dsrstat from invdettbl a " &
                                                  "left join mmasttbl b on a.codeno=b.codeno left join invhdrtbl c on a.mmrrno=c.mmrrno " &
                                                  "left join plnttbl d on c.plntno=d.plntno where c.mov = '" & cboFilter3.Text.Substring(0, 3) & "' " &
                                                  "and c.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                                  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and  " &
                                                  "c.dsrstat = '" & cboFilter4.Text & "' order by c.mmrrno,c.batchno"
                                End Select

                            Case Else
                                'per plnt no
                                Select Case cboFilter3.Text
                                    Case "ALL"
                                        sqldata = "select distinct concat(c.plntno,space(1),d.plntname) as plntno,c.mmrrno,c.transdate,a.codeno," &
                                                  "b.mmdesc,a.qty,a.wt,a.lotno,c.batchno,c.dsrstat from invdettbl a " &
                                                  "left join mmasttbl b on a.codeno=b.codeno left join invhdrtbl c on a.mmrrno=c.mmrrno " &
                                                  "left join plnttbl d on c.plntno=d.plntno where c.mov between '801' and '803' and " &
                                                  "c.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                                  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and  " &
                                                  "c.dsrstat = '" & cboFilter4.Text & "' and c.plntno = '" & cboFilter2.Text.Substring(0, 3) & "' order by c.mmrrno,c.batchno"
                                    Case Else
                                        'per mov type
                                        sqldata = "select distinct concat(c.plntno,space(1),d.plntname) as plntno,c.mmrrno,c.transdate,a.codeno," &
                                                  "b.mmdesc,a.qty,a.wt,a.lotno,c.batchno,c.dsrstat from invdettbl a " &
                                                  "left join mmasttbl b on a.codeno=b.codeno left join invhdrtbl c on a.mmrrno=c.mmrrno " &
                                                  "left join plnttbl d on c.plntno=d.plntno where c.mov = '" & cboFilter3.Text.Substring(0, 3) & "' " &
                                                  "and c.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                                  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and  " &
                                                  "c.dsrstat = '" & cboFilter4.Text & "' and c.plntno = '" & cboFilter2.Text.Substring(0, 3) & "' order by c.mmrrno,c.batchno"
                                End Select

                        End Select


                    Case "Production Register - VOID"
                        Select Case cboFilter2.Text
                            Case "ALL"
                                Select Case cboFilter3.Text
                                    Case "ALL"
                                        sqldata = "select distinct concat(c.plntno,space(1),d.plntname) as plntno,c.mmrrno,c.transdate,a.codeno," &
                                                  "b.mmdesc,a.qty,a.wt,a.lotno,c.batchno,c.dsrstat from invdettbl a " &
                                                  "left join mmasttbl b on a.codeno=b.codeno left join invhdrtbl c on a.mmrrno=c.mmrrno " &
                                                  "left join plnttbl d on c.plntno=d.plntno where c.mov between '801' and '803' and " &
                                                  "c.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                                  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and  " &
                                                  "c.dsrstat = 'void' order by c.mmrrno,c.batchno"
                                    Case Else
                                        'per mov type
                                        sqldata = "select distinct concat(c.plntno,space(1),d.plntname) as plntno,c.mmrrno,c.transdate,a.codeno," &
                                                  "b.mmdesc,a.qty,a.wt,a.lotno,c.batchno,c.dsrstat from invdettbl a " &
                                                  "left join mmasttbl b on a.codeno=b.codeno left join invhdrtbl c on a.mmrrno=c.mmrrno " &
                                                  "left join plnttbl d on c.plntno=d.plntno where c.mov = '" & cboFilter3.Text.Substring(0, 3) & "' " &
                                                  "and c.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                                  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and  " &
                                                  "c.dsrstat = 'void' order by c.mmrrno,c.batchno"
                                End Select

                            Case Else
                                'per plnt no
                                Select Case cboFilter3.Text
                                    Case "ALL"
                                        sqldata = "select distinct concat(c.plntno,space(1),d.plntname) as plntno,c.mmrrno,c.transdate,a.codeno," &
                                                  "b.mmdesc,a.qty,a.wt,a.lotno,c.batchno,c.dsrstat from invdettbl a " &
                                                  "left join mmasttbl b on a.codeno=b.codeno left join invhdrtbl c on a.mmrrno=c.mmrrno " &
                                                  "left join plnttbl d on c.plntno=d.plntno where c.mov between '801' and '803' and " &
                                                  "c.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                                  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and  " &
                                                  "c.dsrstat = 'void' and c.plntno = '" & cboFilter2.Text.Substring(0, 3) & "' order by c.mmrrno,c.batchno"
                                    Case Else
                                        'per mov type
                                        sqldata = "select distinct concat(c.plntno,space(1),d.plntname) as plntno,c.mmrrno,c.transdate,a.codeno," &
                                                  "b.mmdesc,a.qty,a.wt,a.lotno,c.batchno,c.dsrstat from invdettbl a " &
                                                  "left join mmasttbl b on a.codeno=b.codeno left join invhdrtbl c on a.mmrrno=c.mmrrno " &
                                                  "left join plnttbl d on c.plntno=d.plntno where c.mov = '" & cboFilter3.Text.Substring(0, 3) & "' " &
                                                  "and c.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                                  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and  " &
                                                  "c.dsrstat = 'void' and c.plntno = '" & cboFilter2.Text.Substring(0, 3) & "' order by c.mmrrno,c.batchno"
                                End Select

                        End Select
                End Select
        End Select

        With dgvProdRegDet
            .Columns(0).HeaderText = "Plant"
            .Columns(1).HeaderText = "Doc No."
            .Columns(2).HeaderText = "Date"
            .Columns(3).HeaderText = "Code No."
            .Columns(4).HeaderText = "Product Description"
            .Columns(5).HeaderText = "Qty"
            .Columns(6).HeaderText = "Wt/Vol"
            .Columns(7).HeaderText = "Lot No."
            .Columns(8).HeaderText = "Batch No."
            .Columns(9).HeaderText = "Status"

        End With

        conn.Open()
        Dim command As New MySqlCommand(sqldata, conn)
        Adapter.SelectCommand = command
        Adapter.Fill(ds)
        Adapter.Dispose()
        command.Dispose()
        conn.Close()
        dgvProdRegDet.DataSource = ds.Tables(0)
        dgvProdRegDet.DataBind()


    End Sub

    Private Sub prodnSumMatl()
        lblMsg.Text = "Not yet available"

    End Sub

    Private Sub rptDPRadm()
        lblMsg.Text = "Not yet available"

    End Sub
    Private Sub WRR_Reg_Sum()
        lblMsg.Text = "Not yet available"

    End Sub

    Private Sub WRR_Reg_Det()
        lblMsg.Text = "Not yet available"

    End Sub

    Private Sub WRR_custperMatl()
        lblMsg.Text = "Not yet available"

    End Sub

    Private Sub rptMMDO01()
        PregSum.Visible = False
        PPRegDet.Visible = False
        PRegSumIss.Visible = True

        Select Case cboFilter5.Text
            Case "ALL"
                Select Case cboFormat.Text
                    Case "DR Register - Summary"
                        Select Case cboFilter2.Text
                            Case "ALL"
                                Select Case cboFilter3.Text
                                    Case "ALL"
                                        dt = GetDataTable("select * from isshdrtbl where transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                                          "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' limit 1")
                                    Case Else
                                        'per mov type
                                        dt = GetDataTable("select * from isshdrtbl where transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                                          "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and " &
                                                          "mov = '" & cboFilter3.Text.Substring(0, 3) & "' limit 1")
                                End Select

                            Case Else
                                'per plnt no
                                Select Case cboFilter3.Text
                                    Case "ALL"
                                        dt = GetDataTable("select * from isshdrtbl where transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                                          "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and " &
                                                          "plntno = '" & cboFilter2.Text.Substring(0, 3) & "' limit 1")
                                    Case Else
                                        'per mov type
                                        dt = GetDataTable("select * from isshdrtbl where transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                                          "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and " &
                                                          "plntno = '" & cboFilter2.Text.Substring(0, 3) & "' and " &
                                                          "mov = '" & cboFilter3.Text.Substring(0, 3) & "' limit 1")
                                End Select
                        End Select
                End Select
            Case Else
                Select Case cboFormat.Text
                    Case "DR Register - Summary"
                        Select Case cboFilter2.Text
                            Case "ALL"
                                Select Case cboFilter3.Text
                                    Case "ALL"
                                        dt = GetDataTable("select * from isshdrtbl where status = '" & cboFilter5.Text & "' and " &
                                                          "transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                                          "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' limit 1")
                                    Case Else
                                        'per mov type
                                        dt = GetDataTable("select * from isshdrtbl where status = '" & cboFilter5.Text & "' and " &
                                                          "transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                                          "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and " &
                                                          "mov = '" & cboFilter3.Text.Substring(0, 3) & "' limit 1")
                                End Select

                            Case Else
                                'per plnt no
                                Select Case cboFilter3.Text
                                    Case "ALL"
                                        dt = GetDataTable("select * from isshdrtbl where status = '" & cboFilter5.Text & "' and " &
                                                          "transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                                          "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and " &
                                                          "plntno = '" & cboFilter2.Text.Substring(0, 3) & "' limit 1")
                                    Case Else
                                        'per mov type
                                        dt = GetDataTable("select * from isshdrtbl where status = '" & cboFilter5.Text & "' and " &
                                                          "transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                                          "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and " &
                                                          "plntno = '" & cboFilter2.Text.Substring(0, 3) & "' and " &
                                                          "mov = '" & cboFilter3.Text.Substring(0, 3) & "' limit 1")
                                End Select
                        End Select
                End Select
        End Select

        If Not CBool(dt.Rows.Count) Then
            dgvRegSumIss.DataSource = Nothing
            dgvRegSumIss.DataBind()
            lblMsg.Text = "No " & cboFormat.Text & " Found from Dates selected"
            Exit Sub

        End If

        Call dt.Dispose()

        Dim adapter As New MySqlDataAdapter
        Dim ds As New DataSet()

        sqldata = Nothing

        Select Case cboFilter5.Text
            Case "ALL"
                Select Case cboFormat.Text
                    Case "DR Register - Summary"
                        Select Case cboFilter2.Text
                            Case "ALL"
                                Select Case cboFilter3.Text
                                    Case "ALL"
                                        sqldata = "select distinct a.dono,a.refdoc,a.transdate,concat(a.custno,space(1),e.bussname) as custno," &
                                                  "concat(a.shipto,space(1),f.bussname) as shipto,a.smnno,a.pono,a.sono,a.plntno,a.mov,a.dsrstat " &
                                                  "from isshdrtbl a left join custmasttbl e on a.custno=e.custno left join custmasttbl f on a.shipto=f.custno " &
                                                  "where a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                                  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' order by a.transdate,a.dono"
                                    Case Else
                                        'per mov type
                                        sqldata = "select distinct a.dono,a.refdoc,a.transdate,concat(a.custno,space(1),e.bussname) as custno," &
                                                  "concat(a.shipto,space(1),f.bussname) as shipto,a.smnno,a.pono,a.sono,a.plntno,a.mov,a.dsrstat " &
                                                  "from isshdrtbl a left join custmasttbl e on a.custno=e.custno left join custmasttbl f on a.shipto=f.custno " &
                                                  "where a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                                  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and " &
                                                  "a.mov = '" & cboFilter3.Text.Substring(0, 3) & "' order by a.transdate,a.dono"
                                End Select

                            Case Else
                                'per plnt no
                                Select Case cboFilter3.Text
                                    Case "ALL"
                                        sqldata = "select distinct a.dono,a.refdoc,a.transdate,concat(a.custno,space(1),e.bussname) as custno," &
                                                  "concat(a.shipto,space(1),f.bussname) as shipto,a.smnno,a.pono,a.sono,a.plntno,a.mov,a.dsrstat " &
                                                  "from isshdrtbl a left join custmasttbl e on a.custno=e.custno left join custmasttbl f on a.shipto=f.custno " &
                                                  "where a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                                  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and " &
                                                  "a.plntno = '" & cboFilter2.Text.Substring(0, 3) & "'"
                                    Case Else
                                        'per mov type
                                        sqldata = "select distinct a.dono,a.refdoc,a.transdate,concat(a.custno,space(1),e.bussname) as custno," &
                                                  "concat(a.shipto,space(1),f.bussname) as shipto,a.smnno,a.pono,a.sono,a.plntno,a.mov,a.dsrstat " &
                                                  "from isshdrtbl a left join custmasttbl e on a.custno=e.custno left join custmasttbl f on a.shipto=f.custno " &
                                                  "where a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                                  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and " &
                                                  "a.plntno = '" & cboFilter2.Text.Substring(0, 3) & "' and a.mov = '" & cboFilter3.Text.Substring(0, 3) & "'"
                                End Select
                        End Select
                End Select
            Case Else
                Select Case cboFormat.Text
                    Case "DR Register - Summary"
                        Select Case cboFilter2.Text
                            Case "ALL"
                                Select Case cboFilter3.Text
                                    Case "ALL"
                                        sqldata = "select distinct a.dono,a.refdoc,a.transdate,concat(a.custno,space(1),e.bussname) as custno," &
                                                  "concat(a.shipto,space(1),f.bussname) as shipto,a.smnno,a.pono,a.sono,a.plntno,a.mov,a.dsrstat " &
                                                  "from isshdrtbl a left join custmasttbl e on a.custno=e.custno left join custmasttbl f on a.shipto=f.custno " &
                                                  "where a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                                  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and a.status = '" & cboFilter5.Text & "' " &
                                                  "order by a.transdate,a.dono"
                                    Case Else
                                        'per mov type
                                        sqldata = "select distinct a.dono,a.refdoc,a.transdate,concat(a.custno,space(1),e.bussname) as custno," &
                                                  "concat(a.shipto,space(1),f.bussname) as shipto,a.smnno,a.pono,a.sono,a.plntno,a.mov,a.dsrstat " &
                                                  "from isshdrtbl a left join custmasttbl e on a.custno=e.custno left join custmasttbl f on a.shipto=f.custno " &
                                                  "where a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                                  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and " &
                                                  "a.mov = '" & cboFilter3.Text.Substring(0, 3) & "' and a.status = '" & cboFilter5.Text & "' " &
                                                  "order by a.transdate,a.dono"
                                End Select

                            Case Else
                                'per plnt no
                                Select Case cboFilter3.Text
                                    Case "ALL"
                                        sqldata = "select distinct a.dono,a.refdoc,a.transdate,concat(a.custno,space(1),e.bussname) as custno," &
                                                  "concat(a.shipto,space(1),f.bussname) as shipto,a.smnno,a.pono,a.sono,a.plntno,a.mov,a.dsrstat " &
                                                  "from isshdrtbl a left join custmasttbl e on a.custno=e.custno left join custmasttbl f on a.shipto=f.custno " &
                                                  "where a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                                  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and " &
                                                  "a.plntno = '" & cboFilter2.Text.Substring(0, 3) & "'and a.status = '" & cboFilter5.Text & "' " &
                                                  "order by a.transdate,a.dono"
                                    Case Else
                                        'per mov type
                                        sqldata = "select distinct a.dono,a.refdoc,a.transdate,concat(a.custno,space(1),e.bussname) as custno," &
                                                  "concat(a.shipto,space(1),f.bussname) as shipto,a.smnno,a.pono,a.sono,a.plntno,a.mov,a.dsrstat " &
                                                  "from isshdrtbl a left join custmasttbl e on a.custno=e.custno left join custmasttbl f on a.shipto=f.custno " &
                                                  "where a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                                  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and " &
                                                  "a.plntno = '" & cboFilter2.Text.Substring(0, 3) & "' and a.mov = '" & cboFilter3.Text.Substring(0, 3) & "' " &
                                                  "and a.status = '" & cboFilter5.Text & "' order by a.transdate,a.dono"

                                End Select
                        End Select
                End Select
        End Select

        With dgvRegSumIss
            .Columns(0).HeaderText = "DO No."
            .Columns(1).HeaderText = "Ref No."
            .Columns(2).HeaderText = "Date"
            .Columns(3).HeaderText = "Sold To"
            .Columns(4).HeaderText = "Ship To"
            .Columns(5).HeaderText = "Smn No."
            .Columns(6).HeaderText = "PO No."
            .Columns(7).HeaderText = "SO No."
            .Columns(8).HeaderText = "Plant No."
            .Columns(9).HeaderText = "Mov"
            .Columns(10).HeaderText = "Status"

        End With

        conn.Open()
        Dim command As New MySqlCommand(sqldata, conn)
        adapter.SelectCommand = command
        adapter.Fill(ds)
        adapter.Dispose()
        command.Dispose()
        conn.Close()
        dgvRegSumIss.DataSource = ds.Tables(0)
        dgvRegSumIss.DataBind()


    End Sub

    Private Sub rptMMDO02()
        lblMsg.Text = "Not yet available"

    End Sub

    Private Sub rptMM_Iss01()
        Pispm.Visible = True

        Select Case cboFilter5.Text
            Case "ALL"
                Select Case cboFilter2.Text
                    Case "ALL"
                        dt = GetDataTable("select b.dono from issdettbl a left join isshdrtbl b on a.dono=b.dono left join mmasttbl c on a.codeno=c.codeno " &
                                          "where b.mov = '" & cboFilter3.Text.Substring(0, 3) & "' and c.mmtype = '" & cboFilter4.Text & "' " &
                                          "and b.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                          "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' limit 1")
                    Case Else
                        'per plant
                        dt = GetDataTable("select b.dono from issdettbl a left join isshdrtbl b on a.dono=b.dono left join mmasttbl c on a.codeno=c.codeno " &
                                          "where b.plntno = '" & cboFilter2.Text.Substring(0, 3) & "' and b.mov = '" & cboFilter3.Text.Substring(0, 3) & "' " &
                                          "and b.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                          "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and " &
                                          "c.mmtype = '" & cboFilter4.Text & "'limit 1")
                End Select
            Case Else
                Select Case cboFilter2.Text
                    Case "ALL"
                        dt = GetDataTable("select b.dono from issdettbl a left join isshdrtbl b on a.dono=b.dono left join mmasttbl c on a.codeno=c.codeno " &
                                          "where b.mov = '" & cboFilter3.Text.Substring(0, 3) & "' and c.mmtype = '" & cboFilter4.Text & "' " &
                                          "and b.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                          "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and b.status = '" & cboFilter5.Text & "' limit 1")
                    Case Else
                        'per plant
                        dt = GetDataTable("select b.dono from issdettbl a left join isshdrtbl b on a.dono=b.dono left join mmasttbl c on a.codeno=c.codeno " &
                                          "where b.plntno = '" & cboFilter2.Text.Substring(0, 3) & "' and b.mov = '" & cboFilter3.Text.Substring(0, 3) & "' " &
                                          "and b.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                          "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and " &
                                          "c.mmtype = '" & cboFilter4.Text & "' and b.status = '" & cboFilter5.Text & "' limit 1")
                End Select

        End Select



        If Not CBool(dt.Rows.Count) Then
            dgvISpM.DataSource = Nothing
            dgvISpM.DataBind()
            lblMsg.Text = "No " & cboFormat.Text & " Found from Dates selected"
            Exit Sub

        End If

        Call dt.Dispose()


        Dim adapter As New MySqlDataAdapter
        Dim ds As New DataSet()

        sqldata = Nothing

        Select Case cboFilter5.Text
            Case "ALL"
                Select Case cboFilter2.Text
                    Case "ALL"
                        sqldata = "select a.codeno,ifnull(c.codename,c.mmdesc) as mmdesc,sum(ifnull(a.qty,0)) as qty,sum(ifnull(a.wt,0)) as wt " &
                                  "from issdettbl a left join isshdrtbl b on a.dono=b.dono left join mmasttbl c on a.codeno=c.codeno where " &
                                  "b.mov = '" & cboFilter3.Text.Substring(0, 3) & "' and c.mmtype = '" & cboFilter4.Text & "' " &
                                  "and b.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' group by a.codeno order by ifnull(c.codename,c.mmdesc)"
                    Case Else
                        'per plant
                        sqldata = "select a.codeno,ifnull(c.codename,c.mmdesc) as mmdesc,sum(ifnull(a.qty,0)) as qty,sum(ifnull(a.wt,0)) as wt " &
                                  "from issdettbl a left join isshdrtbl b on a.dono=b.dono left join mmasttbl c on a.codeno=c.codeno where " &
                                  "b.plntno = '" & cboFilter2.Text.Substring(0, 3) & "' and b.mov = '" & cboFilter3.Text.Substring(0, 3) & "' " &
                                  "and b.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and " &
                                  "c.mmtype = '" & cboFilter4.Text & "' group by a.codeno order by ifnull(c.codename,c.mmdesc)"
                End Select
            Case Else
                Select Case cboFilter2.Text
                    Case "ALL"
                        sqldata = "select a.codeno,ifnull(c.codename,c.mmdesc) as mmdesc,sum(ifnull(a.qty,0)) as qty,sum(ifnull(a.wt,0)) as wt " &
                                  "from issdettbl a left join isshdrtbl b on a.dono=b.dono left join mmasttbl c on a.codeno=c.codeno where " &
                                  "b.mov = '" & cboFilter3.Text.Substring(0, 3) & "' and b.status = '" & cboFilter5.Text & "' and " &
                                  "c.mmtype = '" & cboFilter4.Text & "' and b.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' group by a.codeno order by ifnull(c.codename,c.mmdesc)"
                    Case Else
                        'per plant
                        sqldata = "select a.codeno,ifnull(c.codename,c.mmdesc) as mmdesc,sum(ifnull(a.qty,0)) as qty,sum(ifnull(a.wt,0)) as wt " &
                                  "from issdettbl a left join isshdrtbl b on a.dono=b.dono left join mmasttbl c on a.codeno=c.codeno where " &
                                  "b.plntno = '" & cboFilter2.Text.Substring(0, 3) & "' and b.mov = '" & cboFilter3.Text.Substring(0, 3) & "' " &
                                  "and b.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and b.status = '" & cboFilter5.Text & "' " &
                                  "and c.mmtype = '" & cboFilter4.Text & "' group by a.codeno order by ifnull(c.codename,c.mmdesc)"
                End Select
        End Select

        With dgvISpM
            .Columns(0).HeaderText = "Code No"
            .Columns(1).HeaderText = "Material Description"
            .Columns(2).HeaderText = "Qty"
            .Columns(3).HeaderText = "Wt/Vol"

            Select Case cboFilter4.Text
                Case "Packaging"
                    .Columns(2).Visible = True
                    .Columns(3).Visible = False
                Case "Finished Goods"
                    .Columns(2).Visible = True
                    .Columns(3).Visible = True
                Case Else
                    .Columns(2).Visible = False
                    .Columns(3).Visible = True
            End Select

        End With

        conn.Open()
        Dim command As New MySqlCommand(sqldata, conn)
        adapter.SelectCommand = command
        adapter.Fill(ds)
        adapter.Dispose()
        command.Dispose()
        conn.Close()
        dgvISpM.DataSource = ds.Tables(0)
        dgvISpM.DataBind()


    End Sub

    Private Sub rptMM_Iss02()
        'rptMM_Iss01 same dgv


    End Sub

    Private Sub rptMM_Iss_Prodn()
        If cboFilter4.Text = "" Then
            Exit Sub
        End If

        PIssToProd.Visible = True

        Select Case cboFilter2.Text 'Plant
            Case "ALL"
                dt = GetDataTable("select * from issdettbl a left join mmasttbl b on a.codeno=b.codeno " &
                                  "left join isshdrtbl c on a.dono=c.dono where c.status <> 'void' and " &
                                  "c.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and " &
                                  "c.mov = '201' and b.mmtype = '" & cboFilter4.Text & "' limit 1")

            Case Else
                'per plnt no
                dt = GetDataTable("select * from issdettbl a left join mmasttbl b on a.codeno=b.codeno " &
                                  "left join isshdrtbl c on a.dono=c.dono where c.status <> 'void' and " &
                                  "c.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                                  "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and " &
                                  "c.plntno = '" & cboFilter2.Text.Substring(0, 3) & "' and c.mov = '201' and " &
                                  "b.mmtype = '" & cboFilter4.Text & "' limit 1")
        End Select

        If Not CBool(dt.Rows.Count) Then
            dgvIssProdMatl.DataSource = Nothing
            dgvIssProdMatl.DataBind()
            lblMsg.Text = "No " & cboFormat.Text & " Found for " & cboFilter4.Text & " from Dates selected"
            Exit Sub

        End If

        dt.Dispose()

        Dim adapter As New MySqlDataAdapter
        Dim ds As New DataSet()

        sqldata = Nothing

        Select Case cboFilter2.Text 'Plant
            Case "ALL"
                sqldata = "select distinct a.dono,c.refdoc,c.transdate,a.codeno,ifnull(b.codename,b.mmdesc) as mmdesc,a.qty,a.wt," &
                          "a.lotno from issdettbl a left join mmasttbl b on a.codeno=b.codeno " &
                          "left join isshdrtbl c on a.dono=c.dono where c.status <> 'void' and " &
                          "c.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                          "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and " &
                          "c.mov = '201' and b.mmtype = '" & cboFilter4.Text & "'"

            Case Else
                'per plnt no
                sqldata = "select distinct a.dono,c.refdoc,c.transdate,a.codeno,ifnull(b.codename,b.mmdesc) as mmdesc,a.qty,a.wt," &
                          "a.lotno from issdettbl a left join mmasttbl b on a.codeno=b.codeno " &
                          "left join isshdrtbl c on a.dono=c.dono where c.status <> 'void' and " &
                          "c.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' " &
                          "and '" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and " &
                          "c.plntno = '" & cboFilter2.Text.Substring(0, 3) & "'  and c.mov = '201' and " &
                          "b.mmtype = '" & cboFilter4.Text & "'"
        End Select

        With dgvIssProdMatl
            .Columns(0).HeaderText = "DO No."
            .Columns(1).HeaderText = "Ref No."
            .Columns(2).HeaderText = "Date"
            .Columns(3).HeaderText = "Code No"
            .Columns(4).HeaderText = "Material Description"
            .Columns(5).HeaderText = "Qty"
            .Columns(6).HeaderText = "Wt/Vol"
            .Columns(7).HeaderText = "Lot No."

            Select Case cboFilter4.Text
                Case "Raw Materials"
                    .Columns(5).Visible = False
                    .Columns(6).Visible = True
                Case "Packaging"
                    .Columns(5).Visible = True
                    .Columns(6).Visible = False
                Case Else
                    .Columns(5).Visible = False
                    .Columns(6).Visible = True
            End Select

        End With

        conn.Open()
        Dim command As New MySqlCommand(sqldata, conn)
        adapter.SelectCommand = command
        adapter.Fill(ds)
        adapter.Dispose()
        command.Dispose()
        conn.Close()
        dgvIssProdMatl.DataSource = ds.Tables(0)
        dgvIssProdMatl.DataBind()

    End Sub

    Private Sub rptMM_Iss_Prodn_perMatl()
        PIssProdMatl.Visible = True

        Select Case cboFilter2.Text
            Case "ALL"
                dt = GetDataTable("select * from tempisshdrtbl where user = '" & lblUser.Text & "' limit 1")
            Case Else
                dt = GetDataTable("select * from tempisshdrtbl where user = '" & lblUser.Text & "' and " &
                                  "plntno = '" & cboFilter2.Text.Substring(0, 3) & "' limit 1")
        End Select

        If Not CBool(dt.Rows.Count) Then
            dgvIssProdPerMatl.DataSource = Nothing
            dgvIssProdPerMatl.DataBind()
            lblMsg.Text = "No " & cboFormat.Text & " Found for " & cboFilter4.Text & " from Dates selected"
            Exit Sub

        End If

        dt.Dispose()

        Dim adapter As New MySqlDataAdapter
        Dim ds As New DataSet()

        sqldata = Nothing

        Select Case cboFilter2.Text
            Case "ALL"
                sqldata = "select a.dono,b.transdate,b.pono as lotno,b.remarks as dsrno,ifnull(a.wt,0) as wt from issdettbl a " &
                          "left join tempisshdrtbl b on a.dono=b.dono  where a.codeno = '" & txtText5.Text & "' and b.status <> 'void' " &
                          "and b.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                          "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' " &
                          "and b.user = '" & lblUser.Text & "' group by a.idno order by a.dono"
            Case Else
                sqldata = "select a.dono,b.transdate,b.pono as lotno,b.remarks as dsrno,ifnull(a.wt,0) as wt from issdettbl a " &
                          "left join tempisshdrtbl b on a.dono=b.dono where a.codeno = '" & txtText5.Text & "' and b.status <> 'void' " &
                          "and b.plntno = '" & cboFilter2.Text.Substring(0, 3) & "' and  " &
                          "b.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                          "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' " &
                          "and b.user = '" & lblUser.Text & "' group by a.idno order by a.dono"
        End Select

        With dgvIssProdPerMatl
            .Columns(0).HeaderText = "Doc No."
            .Columns(1).HeaderText = "Date"
            .Columns(2).HeaderText = "Lot No."
            .Columns(3).HeaderText = "Material Description"
            .Columns(4).HeaderText = "Wt/Vol."

        End With

        conn.Open()
        Dim command As New MySqlCommand(sqldata, conn)
        adapter.SelectCommand = command
        adapter.Fill(ds)
        adapter.Dispose()
        command.Dispose()
        conn.Close()
        dgvIssProdPerMatl.DataSource = ds.Tables(0)
        dgvIssProdPerMatl.DataBind()


    End Sub

    Private Sub popIssDetAll_MMrep()
        dt = GetDataTable("select * from information_schema.tables where table_schema =  '" & vDbase & "' and " &
                          "table_name = 'tempisshdrtbl'")
        If Not CBool(dt.Rows.Count) Then
            sql = "CREATE TABLE tempisshdrtbl LIKE isshdrtbl"
            ExecuteNonQuery(sql)
        Else
            sql = "delete from tempisshdrtbl where user = '" & lblUser.Text & "'"
            ExecuteNonQuery(sql)
        End If

        Select Case cboFilter2.Text 'plant
            Case "ALL"
                Select Case cboFilter3.Text.Substring(0, 3) 'mov
                    Case "ALL"
                        sql = "insert into tempisshdrtbl(dono,transdate,tc,mov,remarks,plntno,status,user,pono) " &
                              "select a.dono,a.transdate,a.tc,a.mov,ifnull(c.codename,c.mmdesc),a.plntno,a.status," &
                              "'" & lblUser.Text & "',b.dsrno from isshdrtbl a left join issdettbl d on a.dono=d.dono " &
                              "left join invhdrtbl b on a.dsrno=b.mmrrno left join mmasttbl c on b.pono=c.codeno where " &
                              "d.codeno = '" & txtText5.Text & "' and a.status <> 'void' and a.mov = '201' and " &
                              "a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                              "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' group by d.idno order by d.dono"
                        ExecuteNonQuery(sql)

                        sql = "insert into tempisshdrtbl(dono,transdate,tc,mov,remarks,plntno,status,user) select a.dono,a.transdate,a.tc,a.mov,c.bussname," &
                              "a.plntno,a.status,'" & lblUser.Text & "' from isshdrtbl a left join issdettbl d on a.dono=d.dono " &
                              "left join custmasttbl c on a.shipto=c.custno where d.codeno = '" & txtText5.Text & "' and " &
                              "a.status <> 'void' and a.mov <> '201' and " &
                              "a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                              "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' group by d.idno order by d.dono"
                        ExecuteNonQuery(sql)

                    Case "201"
                        Select Case cboFilter4.Text
                            Case "Raw Materials"
                                Select Case True
                                    Case RadioButton12.Checked
                                        sql = "insert into tempisshdrtbl(dono,transdate,tc,mov,remarks,plntno,status,user,pono,dsrstat) select a.dono,a.transdate," &
                                              "a.tc,a.mov,ifnull(c.codename,c.mmdesc),a.plntno,a.status,'" & lblUser.Text & "',b.dsrno,b.dsrstat from isshdrtbl a " &
                                              "left join issdettbl d on a.dono=d.dono left join invhdrtbl b on a.dsrno=b.mmrrno left join mmasttbl c " &
                                              "on b.pono=c.codeno where d.codeno = '" & txtText5.Text & "' and a.status <> 'void' " &
                                              "and a.mov = '201' and a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                                              "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' group by d.idno order by d.dono"
                                        ExecuteNonQuery(sql)
                                    Case RadioButton13.Checked
                                        sql = "insert into tempisshdrtbl(dono,transdate,tc,mov,remarks,plntno,status,user,pono,dsrstat) select a.dono,a.transdate," &
                                              "a.tc,a.mov,ifnull(c.codename,c.mmdesc),a.plntno,a.status,'" & lblUser.Text & "',b.dsrno,b.dsrstat from isshdrtbl a " &
                                              "left join issdettbl d on a.dono=d.dono left join invhdrtbl b on a.dsrno=b.mmrrno left join mmasttbl c " &
                                              "on b.pono=c.codeno where d.codeno = '" & txtText5.Text & "' and a.status <> 'void' " &
                                              "and a.mov = '201' and a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                                              "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and b.dsrstat = 'On Process' group by d.idno order by d.dono"
                                        ExecuteNonQuery(sql)
                                    Case RadioButton14.Checked
                                        sql = "insert into tempisshdrtbl(dono,transdate,tc,mov,remarks,plntno,status,user,pono,dsrstat) select a.dono,a.transdate," &
                                              "a.tc,a.mov,ifnull(c.codename,c.mmdesc),a.plntno,a.status,'" & lblUser.Text & "',b.dsrno,b.dsrstat from isshdrtbl a " &
                                              "left join issdettbl d on a.dono=d.dono left join invhdrtbl b on a.dsrno=b.mmrrno left join mmasttbl c " &
                                              "on b.pono=c.codeno where d.codeno = '" & txtText5.Text & "' and a.status <> 'void' " &
                                              "and a.mov = '201' and a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                                              "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and b.dsrstat = 'Finished' group by d.idno order by d.dono"
                                        ExecuteNonQuery(sql)

                                End Select
                            Case Else
                                sql = "insert into tempisshdrtbl(dono,transdate,tc,mov,remarks,plntno,status,user,pono) select a.dono,a.transdate," &
                                      "a.tc,a.mov,ifnull(c.codename,c.mmdesc),a.plntno,a.status,'" & lblUser.Text & "',b.dsrno from isshdrtbl a " &
                                      "left join issdettbl d on a.dono=d.dono left join invhdrtbl b on a.dsrno=b.mmrrno left join mmasttbl c " &
                                      "on b.pono=c.codeno where d.codeno = '" & txtText5.Text & "' and a.status <> 'void' " &
                                      "and a.mov = '201' and a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                                      "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' group by d.idno order by d.dono"
                                ExecuteNonQuery(sql)
                        End Select


                    Case Else
                        sql = "insert into tempisshdrtbl(dono,transdate,tc,mov,remarks,plntno,status,user) select a.dono,a.transdate,a.tc,a.mov,c.bussname," &
                              "a.plntno,a.status,'" & lblUser.Text & "' from isshdrtbl a left join issdettbl d on a.dono=d.dono " &
                              "left join custmasttbl c on a.shipto=c.custno where d.codeno = '" & txtText5.Text & "' and " &
                              "a.status <> 'void' and a.mov = '" & cboFilter3.Text.Substring(0, 3) & "' and " &
                              "a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                              "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' group by d.idno order by d.dono"
                        ExecuteNonQuery(sql)

                End Select
            Case Else
                Select Case cboFilter3.Text.Substring(0, 3) 'mov
                    Case "ALL"
                        sql = "insert into tempisshdrtbl(dono,transdate,tc,mov,remarks,plntno,status,user,pono) select a.dono,a.transdate,a.tc,a.mov,ifnull(c.codename,c.mmdesc)," &
                              "a.plntno,a.status,'" & lblUser.Text & "',b.dsrno from isshdrtbl a left join issdettbl d on a.dono=d.dono " &
                              "left join invhdrtbl b on a.dsrno=b.mmrrno left join mmasttbl c on b.pono=c.codeno where " &
                              "d.codeno = '" & txtText5.Text & "' and a.plntno = '" & cboFilter2.Text.Substring(0, 3) & "' and a.status <> 'void' " &
                              "and a.mov = '201' and a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                              "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' group by d.idno order by d.dono"
                        ExecuteNonQuery(sql)

                        sql = "insert into tempisshdrtbl(dono,transdate,tc,mov,remarks,plntno,status,user) select a.dono,a.transdate,a.tc,a.mov,c.bussname," &
                              "a.plntno,a.status,'" & lblUser.Text & "' from isshdrtbl a left join issdettbl d on a.dono=d.dono " &
                              "left join custmasttbl c on a.shipto=c.custno where d.codeno = '" & txtText5.Text & "' and " &
                              "a.plntno = '" & cboFilter2.Text.Substring(0, 3) & "' and a.status <> 'void' and a.mov <> '201' and " &
                              "a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                              "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' group by d.idno order by d.dono"
                        ExecuteNonQuery(sql)

                    Case "201"
                        Select Case cboFilter4.Text
                            Case "Raw Materials"
                                Select Case True
                                    Case RadioButton12.Checked
                                        sql = "insert into tempisshdrtbl(dono,transdate,tc,mov,remarks,plntno,status,user,pono,dsrstat) select a.dono," &
                                              "a.transdate,a.tc,a.mov,ifnull(c.codename,c.mmdesc),a.plntno,a.status,'" & lblUser.Text & "'," &
                                              "b.dsrno,b.dsrstat from isshdrtbl a left join issdettbl d on a.dono=d.dono left join invhdrtbl b on a.dsrno=b.mmrrno " &
                                              "left join mmasttbl c on b.pono=c.codeno where d.codeno = '" & txtText5.Text & "' and " &
                                              "a.plntno = '" & cboFilter2.Text.Substring(0, 3) & "' and a.status <> 'void' and a.mov = '201' and " &
                                              "a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                                              "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' group by d.idno order by d.dono"
                                        ExecuteNonQuery(sql)
                                    Case RadioButton13.Checked '
                                        sql = "insert into tempisshdrtbl(dono,transdate,tc,mov,remarks,plntno,status,user,pono,dsrstat) select a.dono," &
                                              "a.transdate,a.tc,a.mov,ifnull(c.codename,c.mmdesc),a.plntno,a.status,'" & lblUser.Text & "'," &
                                              "b.dsrno,b.dsrstat from isshdrtbl a left join issdettbl d on a.dono=d.dono left join invhdrtbl b on a.dsrno=b.mmrrno " &
                                              "left join mmasttbl c on b.pono=c.codeno where d.codeno = '" & txtText5.Text & "' and " &
                                              "a.plntno = '" & cboFilter2.Text.Substring(0, 3) & "' and a.status <> 'void' and a.mov = '201' and " &
                                              "a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                                              "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and b.dsrstat = 'On Process' group by d.idno order by d.dono"
                                        ExecuteNonQuery(sql)
                                    Case RadioButton14.Checked
                                        sql = "insert into tempisshdrtbl(dono,transdate,tc,mov,remarks,plntno,status,user,pono,dsrstat) select a.dono," &
                                              "a.transdate,a.tc,a.mov,ifnull(c.codename,c.mmdesc),a.plntno,a.status,'" & lblUser.Text & "'," &
                                              "b.dsrno,b.dsrstat from isshdrtbl a left join issdettbl d on a.dono=d.dono left join invhdrtbl b on a.dsrno=b.mmrrno " &
                                              "left join mmasttbl c on b.pono=c.codeno where d.codeno = '" & txtText5.Text & "' and " &
                                              "a.plntno = '" & cboFilter2.Text.Substring(0, 3) & "' and a.status <> 'void' and a.mov = '201' and " &
                                              "a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                                              "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' and b.dsrstat = 'Finished' group by d.idno order by d.dono"
                                        ExecuteNonQuery(sql)
                                End Select

                            Case Else
                                sql = "insert into tempisshdrtbl(dono,transdate,tc,mov,remarks,plntno,status,user,pono) select a.dono,a.transdate,a.tc,a.mov,ifnull(c.codename,c.mmdesc)," &
                                      "a.plntno,a.status,'" & lblUser.Text & "',b.dsrno from isshdrtbl a left join issdettbl d on a.dono=d.dono " &
                                      "left join invhdrtbl b on a.dsrno=b.mmrrno left join mmasttbl c on b.pono=c.codeno where " &
                                      "d.codeno = '" & txtText5.Text & "' and a.plntno = '" & cboFilter2.Text.Substring(0, 3) & "' and a.status <> 'void' " &
                                      "and a.mov = '201' and a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                                      "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' group by d.idno order by d.dono"
                                ExecuteNonQuery(sql)
                        End Select

                    Case Else
                        sql = "insert into tempisshdrtbl(dono,transdate,tc,mov,remarks,plntno,status,user) select a.dono,a.transdate,a.tc,a.mov,c.bussname," &
                              "a.plntno,a.status,'" & lblUser.Text & "' from isshdrtbl a left join issdettbl d on a.dono=d.dono " &
                              "left join custmasttbl c on a.shipto=c.custno where d.codeno = '" & txtText5.Text & "' and " &
                              "a.plntno = '" & cboFilter2.Text.Substring(0, 3) & "' and a.status <> 'void' and a.mov = '" & cboFilter3.Text.Substring(0, 3) & "' and " &
                              "a.transdate between '" & Format(CDate(dpTransDate.Text), "yyyy-MM-dd") & "' and " &
                              "'" & Format(CDate(dpTransDate2.Text), "yyyy-MM-dd") & "' group by d.idno order by d.dono"
                        ExecuteNonQuery(sql)
                End Select

        End Select

        rptMM_Iss_Prodn_perMatl()

    End Sub

    Protected Sub dgvRegSum_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim tQty As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "qty").ToString())
            qty += tQty

            Dim tWt As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "wt").ToString())
            wt += tWt

            Dim tAmt As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "amt").ToString())
            amt += tAmt

        End If

        If e.Row.RowType = DataControlRowType.Footer Then
            Dim lblGTotal As Label = DirectCast(e.Row.FindControl("lblGTotal"), Label)
            lblGTotal.Text = "GRAND TOTAL"

            Dim lblTotalGross As Label = DirectCast(e.Row.FindControl("lblQty"), Label)
            lblTotalGross.Text = Format(CDbl(qty.ToString()), "#,##0 ; (#,##0)")
            qty = 0

            Dim lblTotalVat As Label = DirectCast(e.Row.FindControl("lblWt"), Label)
            lblTotalVat.Text = Format(CDbl(wt.ToString()), "#,##0.00 ; (#,##0.00)")
            wt = 0

            Dim lblTotalFh As Label = DirectCast(e.Row.FindControl("lblAmt"), Label)
            lblTotalFh.Text = Format(CDbl(amt.ToString()), "#,##0.00 ; (#,##0.00)")
            amt = 0

        End If

    End Sub

    Protected Sub dgvRegSum_RowCreated(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Protected Sub dgvProdRegDet_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim tQty As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "qty").ToString())
            qty += tQty

            Dim tWt As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "wt").ToString())
            wt += tWt

        End If

        If e.Row.RowType = DataControlRowType.Footer Then
            Dim lblGTotal As Label = DirectCast(e.Row.FindControl("lblGTotal"), Label)
            lblGTotal.Text = "GRAND TOTAL"

            Dim lblTotalGross As Label = DirectCast(e.Row.FindControl("lblQty"), Label)
            lblTotalGross.Text = Format(CDbl(qty.ToString()), "#,##0 ; (#,##0)")
            qty = 0

            Dim lblTotalVat As Label = DirectCast(e.Row.FindControl("lblWt"), Label)
            lblTotalVat.Text = Format(CDbl(wt.ToString()), "#,##0.00 ; (#,##0.00)")
            wt = 0

        End If

    End Sub

    Protected Sub dgvProdRegDet_RowCreated(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Private Sub dpMtoMdate1_TextChanged(sender As Object, e As EventArgs) Handles dpMtoMdate1.TextChanged, dpMtoMdate2.TextChanged
        If dpMtoMdate1.Text = Nothing Then
            Exit Sub
        ElseIf dpMtoMdate2.Text = Nothing Then
            Exit Sub
        ElseIf CDate(dpMtoMdate1.text) > CDate(dpMtoMdate2.text) Then
            Exit Sub
        End If

        popPlntMtoM()

    End Sub

    Private Sub popPlntMtoM()
        cboPlntMtoM.Items.Clear()
        dt = GetDataTable("select concat(a.plntno,space(1),b.plntname) from isshdrtbl a left join plnttbl b on a.plntno=b.plntno " &
                          "where a.mov = '309' and a.transdate between '" & Format(CDate(dpMtoMdate1.Text), "yyyy-MM-dd") & "' and " &
                          "'" & Format(CDate(dpMtoMdate2.Text), "yyyy-MM-dd") & "' and a.status <> 'void' group by a.plntno")
        If Not CBool(dt.Rows.Count) Then
            lblMsg.Text = "No Plant for Plant To Plant Transfer Found."
            Exit Sub
        Else
            cboPlntMtoM.Items.Add("")
            For Each dr As DataRow In dt.Rows
                cboPlntMtoM.Items.Add(dr.Item(0).ToString())
            Next

        End If

        Call dt.Dispose()

        dt = GetDataTable("select concat(mov,space(1),movdesc) from movtbl where mov = '309'")
        If Not CBool(dt.Rows.Count) Then
            lblMsg.Text = "Movement Type Not Found."
            Exit Sub
        Else
            For Each dr As DataRow In dt.Rows
                lblMovMtoM.Text = dr.Item(0).ToString() & ""
            Next

        End If

        Call dt.Dispose()

    End Sub

    Private Sub cboPlntMtoM_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboPlntMtoM.SelectedIndexChanged
        If dpMtoMdate1.Text = Nothing Then
            Exit Sub
        ElseIf dpMtoMdate2.Text = Nothing Then
            Exit Sub
        ElseIf CDate(dpMtoMdate1.Text) > CDate(dpMtoMdate2.Text) Then
            Exit Sub
        ElseIf cboPlntMtoM.Text = "" Then
            Exit Sub
        End If

        filldgvMtoMList()

    End Sub

    Private Sub filldgvMtoMList()
        dgvMtoMlist.DataSource = Nothing
        dgvMtoMlist.DataBind()
        dgvMtoMin.DataSource = Nothing
        dgvMtoMin.DataBind()
        dgvMtoMout.DataSource = Nothing
        dgvMtoMout.DataBind()
        'lblMtoMoutQty.Text = "0"
        'lblMtoMoutWt.Text = "0.00"
        'lblMtoMinQty.Text = "0"
        'lblMtoMinWt.Text = "0.00"

        dt = GetDataTable("select * from invhdrtbl where mov = '309' and transdate between '" & Format(CDate(dpMtoMdate1.Text), "yyyy-MM-dd") & "' " &
                          "and '" & Format(CDate(dpMtoMdate2.Text), "yyyy-MM-dd") & "' and plntno = '" & cboPlntMtoM.Text.Substring(0, 3) & "' " &
                          "and status is not null")
        If Not CBool(dt.Rows.Count) Then
            lblMsg.Text = "No Material to Material Transfer "
            Exit Sub

        End If

        dt.Dispose()

        Dim adapter As New MySqlDataAdapter
        Dim ds As New DataSet()

        sqldata = Nothing

        sqldata = "select mmrrno,transdate,tc,pc,ifnull(status,'PARK') as stat,user from invhdrtbl where mov = '309' and " &
                  "transdate between '" & Format(CDate(dpMtoMdate1.Text), "yyyy-MM-dd") & "' and '" & Format(CDate(dpMtoMdate2.Text), "yyyy-MM-dd") & "' " &
                  "and plntno = '" & cboPlntMtoM.Text.Substring(0, 3) & "' and status is not null order by mmrrno"

        With dgvMtoMlist

            .Columns(1).HeaderText = "Doc. No"
            .Columns(2).HeaderText = "Date"
            .Columns(3).HeaderText = "TC"
            .Columns(4).HeaderText = "Material Type"
            .Columns(5).HeaderText = "Status"
            .Columns(6).HeaderText = "Requested By"

        End With

        conn.Open()
        Dim command As New MySqlCommand(sqldata, conn)
        adapter.SelectCommand = command
        adapter.Fill(ds)
        adapter.Dispose()
        command.Dispose()
        conn.Close()
        dgvMtoMlist.DataSource = ds.Tables(0)
        dgvMtoMlist.DataBind()

    End Sub

    Private Sub filldgvMtoMinput()

        dt = GetDataTable("select * from issdettbl where dono = '" & dgvMtoMlist.SelectedRow.Cells(1).Text & "' limit 1")
        If Not CBool(dt.Rows.Count) Then
            dgvMtoMin.DataSource = Nothing
            dgvMtoMin.DataBind()
            Exit Sub

        End If

        dt.Dispose()

        Dim adapter As New MySqlDataAdapter
        Dim ds As New DataSet()

        sqldata = Nothing

        sqldata = "select a.codeno,ifnull(b.codename,b.mmdesc) as mmdesc,a.lotno,ifnull(a.qty,0) as qty,ifnull(a.wt,0) as wt " &
                  "from issdettbl a left join mmasttbl b on a.codeno=b.codeno where a.dono = '" & dgvMtoMlist.SelectedRow.Cells(1).Text & "'"

        With dgvMtoMin
            .Columns(0).HeaderText = "Code No"
            .Columns(1).HeaderText = "Material Description"
            .Columns(2).HeaderText = "Lot No."
            .Columns(3).HeaderText = "Qty"
            .Columns(4).HeaderText = "Wt/Vol"

        End With

        conn.Open()
        Dim command As New MySqlCommand(sqldata, conn)
        adapter.SelectCommand = command
        adapter.Fill(ds)
        adapter.Dispose()
        command.Dispose()
        conn.Close()
        dgvMtoMin.DataSource = ds.Tables(0)
        dgvMtoMin.DataBind()

    End Sub

    Private Sub filldgvMtoMoutput()
        dt = GetDataTable("select * from invdettbl where mmrrno = '" & dgvMtoMlist.SelectedRow.Cells(1).Text & "' limit 1")
        If Not CBool(dt.Rows.Count) Then
            dgvMtoMout.DataSource = Nothing
            dgvMtoMout.DataBind()
            Exit Sub

        End If

        dt.Dispose()

        Dim adapter As New MySqlDataAdapter
        Dim ds As New DataSet()

        sqldata = Nothing

        sqldata = "select a.codeno,ifnull(b.codename,b.mmdesc) as mmdesc,a.lotno,ifnull(a.qty,0) as qty,ifnull(a.wt,0) as wt " &
                  "from invdettbl a left join mmasttbl b on a.codeno=b.codeno where a.mmrrno = '" & dgvMtoMlist.SelectedRow.Cells(1).Text & "'"

        With dgvMtoMout
            .Columns(0).HeaderText = "Code No"
            .Columns(1).HeaderText = "Material Description"
            .Columns(2).HeaderText = "Lot No."
            .Columns(3).HeaderText = "Qty"
            .Columns(4).HeaderText = "Wt/Vol"

        End With

        conn.Open()
        Dim command As New MySqlCommand(sqldata, conn)
        Adapter.SelectCommand = command
        Adapter.Fill(ds)
        Adapter.Dispose()
        command.Dispose()
        conn.Close()
        dgvMtoMout.DataSource = ds.Tables(0)
        dgvMtoMout.DataBind()

    End Sub

    Protected Sub dgvMtoMlist_RowDataBound(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Protected Sub dgvMtoMlist_RowCreated(sender As Object, e As GridViewRowEventArgs)

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

    Private Sub dgvMtoMlist_SelectedIndexChanged(sender As Object, e As EventArgs) Handles dgvMtoMlist.SelectedIndexChanged
        If dpMtoMdate1.Text = Nothing Then
            Exit Sub
        ElseIf dpMtoMdate2.Text = Nothing Then
            Exit Sub
        ElseIf CDate(dpMtoMdate1.Text) > CDate(dpMtoMdate2.Text) Then
            Exit Sub
        End If

        If dgvMtoMlist.Rows.Count = 0 Then
            Exit Sub
        End If

        dgvMtoMin.DataSource = Nothing
        dgvMtoMin.DataBind()
        dgvMtoMout.DataSource = Nothing
        dgvMtoMout.DataBind()


        filldgvMtoMinput()
        filldgvMtoMoutput()

    End Sub

    Protected Sub dgvMtoMin_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim tQty As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "qty").ToString())
            qty += tQty

            Dim tWt As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "wt").ToString())
            wt += tWt

        End If

        If e.Row.RowType = DataControlRowType.Footer Then
            Dim lblGTotal As Label = DirectCast(e.Row.FindControl("lblGTotal"), Label)
            lblGTotal.Text = "TOTAL"

            Dim lblTotalGross As Label = DirectCast(e.Row.FindControl("lblQty"), Label)
            lblTotalGross.Text = Format(CDbl(qty.ToString()), "#,##0 ; (#,##0)")
            qty = 0

            Dim lblTotalVat As Label = DirectCast(e.Row.FindControl("lblWt"), Label)
            lblTotalVat.Text = Format(CDbl(wt.ToString()), "#,##0.00 ; (#,##0.00)")
            wt = 0


        End If
    End Sub

    Protected Sub dgvMtoMin_RowCreated(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Protected Sub dgvMtoMout_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim tQty As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "qty").ToString())
            qty += tQty

            Dim tWt As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "wt").ToString())
            wt += tWt

        End If

        If e.Row.RowType = DataControlRowType.Footer Then
            Dim lblGTotal As Label = DirectCast(e.Row.FindControl("lblGTotal"), Label)
            lblGTotal.Text = "TOTAL"

            Dim lblTotalGross As Label = DirectCast(e.Row.FindControl("lblQty"), Label)
            lblTotalGross.Text = Format(CDbl(qty.ToString()), "#,##0 ; (#,##0)")
            qty = 0

            Dim lblTotalVat As Label = DirectCast(e.Row.FindControl("lblWt"), Label)
            lblTotalVat.Text = Format(CDbl(wt.ToString()), "#,##0.00 ; (#,##0.00)")
            wt = 0


        End If
    End Sub

    Protected Sub dgvMtoMout_RowCreated(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Protected Sub dgvRegSumIss_RowDataBound(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Protected Sub dgvRegSumIss_RowCreated(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Protected Sub dgvIssProdMatl_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim tQty As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "qty").ToString())
            qty += tQty

            Dim tWt As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "wt").ToString())
            wt += tWt

        End If

        If e.Row.RowType = DataControlRowType.Footer Then
            Dim lblGTotal As Label = DirectCast(e.Row.FindControl("lblGTotal"), Label)
            lblGTotal.Text = "TOTAL"

            Dim lblTotalGross As Label = DirectCast(e.Row.FindControl("lblQty"), Label)
            lblTotalGross.Text = Format(CDbl(qty.ToString()), "#,##0 ; (#,##0)")
            qty = 0

            Dim lblTotalVat As Label = DirectCast(e.Row.FindControl("lblWt"), Label)
            lblTotalVat.Text = Format(CDbl(wt.ToString()), "#,##0.00 ; (#,##0.00)")
            wt = 0

        End If

    End Sub

    Protected Sub dgvIssProdMatl_RowCreated(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Protected Sub dgvIssProdPerMatl_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then

            Dim tWt As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "wt").ToString())
            wt += tWt

        End If

        If e.Row.RowType = DataControlRowType.Footer Then
            Dim lblGTotal As Label = DirectCast(e.Row.FindControl("lblGTotal"), Label)
            lblGTotal.Text = "GRAND TOTAL"

            Dim lblTotalVat As Label = DirectCast(e.Row.FindControl("lblWt"), Label)
            lblTotalVat.Text = Format(CDbl(wt.ToString()), "#,##0.00000 ; (#,##0.00000)")
            wt = 0

        End If


    End Sub

    Protected Sub dgvIssProdPerMatl_RowCreated(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Protected Sub dgvISpM_RowDataBound(sender As Object, e As GridViewRowEventArgs)

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim decimalFormat As String

            Select Case cboFilter4.Text
                Case "Raw Materials"
                    decimalFormat = "{0:N5}"

                Case Else
                    decimalFormat = "{0:N2}"
            End Select

            Dim priceCell As TableCell = e.Row.Cells(3)
            Dim priceValue As Decimal = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "wt").ToString())
            priceCell.Text = String.Format(decimalFormat, priceValue)

            Dim tQty As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "qty").ToString())
            qty += tQty

            Dim tWt As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "wt").ToString())
            wt += tWt

        End If

        If e.Row.RowType = DataControlRowType.Footer Then
            Dim lblGTotal As Label = DirectCast(e.Row.FindControl("lblGTotal"), Label)
            lblGTotal.Text = "TOTAL"

            Dim lblTotalGross As Label = DirectCast(e.Row.FindControl("lblQty"), Label)
            lblTotalGross.Text = Format(CDbl(qty.ToString()), "#,##0 ; (#,##0)")
            qty = 0

            Dim lblTotalVat As Label = DirectCast(e.Row.FindControl("lblWt"), Label)
            Select Case cboFilter4.Text
                Case "Raw Materials"
                    lblTotalVat.Text = Format(CDbl(wt.ToString()), "#,##0.00000 ; (#,##0.00000)")
                Case Else
                    lblTotalVat.Text = Format(CDbl(wt.ToString()), "#,##0.00 ; (#,##0.00)")
            End Select

            wt = 0


        End If


    End Sub

    Protected Sub dgvISpM_RowCreated(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Protected Sub dgvPRpVD_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim tQty As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "qty").ToString())
            qty += tQty

            Dim tWt As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "wt").ToString())
            wt += tWt

            Dim tAmt As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "amt").ToString())
            amt += tAmt

            Dim tQty2 As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "qty").ToString())
            sqty += tQty2

            Dim tWt2 As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "wt").ToString())
            swt += tWt2

            Dim tAmt2 As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "amt").ToString())
            samt += tAmt2

            venno = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "venno").ToString())
            strVenName = DataBinder.Eval(e.Row.DataItem, "venname").ToString()

        End If

        If e.Row.RowType = DataControlRowType.Footer Then
            Dim lblGTotal As Label = DirectCast(e.Row.FindControl("lblGTotal"), Label)
            lblGTotal.Text = "GRAND TOTAL"

            Dim lblTotalQty As Label = DirectCast(e.Row.FindControl("lblQty"), Label)
            lblTotalQty.Text = Format(CDbl(qty.ToString()), "#,##0 ; (#,##0)")
            qty = 0

            Dim lblTotalWt As Label = DirectCast(e.Row.FindControl("lblWt"), Label)
            lblTotalWt.Text = Format(CDbl(wt.ToString()), "#,##0.00 ; (#,##0.00)")
            wt = 0

            Dim lblTotalAmt As Label = DirectCast(e.Row.FindControl("lblAmt"), Label)
            lblTotalAmt.Text = Format(CDbl(amt.ToString()), "#,##0.00 ; (#,##0.00)")
            wt = 0

        End If
    End Sub

    Protected Sub dgvPRpVD_RowCreated(sender As Object, e As GridViewRowEventArgs)
        Dim newRow As Boolean = False

        If (venno > 0) AndAlso (DataBinder.Eval(e.Row.DataItem, "venno") IsNot Nothing) Then
            If venno <> Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "venno").ToString()) Then
                newRow = True
            End If
        End If
        If (venno > 0) AndAlso (DataBinder.Eval(e.Row.DataItem, "venno") Is Nothing) Then
            newRow = True
            rowIndex = 0
        End If

        If newRow Then
            Dim dgvPRpVD As GridView = DirectCast(sender, GridView)
            Dim NewTotalRow As New GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert)
            NewTotalRow.Font.Bold = True
            NewTotalRow.BackColor = System.Drawing.Color.Gray
            NewTotalRow.ForeColor = System.Drawing.Color.White

            Dim HeaderCell As New TableCell()
            HeaderCell.Text = Format(venno, "#00000") & Space(1) & strVenName & " Total"
            HeaderCell.HorizontalAlign = HorizontalAlign.Center
            HeaderCell.Font.Size = 10

            HeaderCell.ColumnSpan = 6
            NewTotalRow.Cells.Add(HeaderCell)

            Select Case cboFilter5.Text
                Case "ALL"
                    HeaderCell = New TableCell()
                    HeaderCell.HorizontalAlign = HorizontalAlign.Right
                    HeaderCell.Text = Format(CDbl(sqty.ToString()), "#,##0 ; (#,##0)")
                    NewTotalRow.Cells.Add(HeaderCell)

                    HeaderCell = New TableCell()
                    HeaderCell.HorizontalAlign = HorizontalAlign.Right
                    HeaderCell.Text = Format(CDbl(swt.ToString()), "#,##0.00 ; (#,##0.00)")
                    NewTotalRow.Cells.Add(HeaderCell)

                Case "Raw Materials"
                    HeaderCell = New TableCell()
                    HeaderCell.HorizontalAlign = HorizontalAlign.Right
                    HeaderCell.Text = Format(CDbl(swt.ToString()), "#,##0.00 ; (#,##0.00)")
                    NewTotalRow.Cells.Add(HeaderCell)

                Case "Packaginng"
                    HeaderCell = New TableCell()
                    HeaderCell.HorizontalAlign = HorizontalAlign.Right
                    HeaderCell.Text = Format(CDbl(sqty.ToString()), "#,##0 ; (#,##0)")
                    NewTotalRow.Cells.Add(HeaderCell)

                Case "Finished Goods"
                    HeaderCell = New TableCell()
                    HeaderCell.HorizontalAlign = HorizontalAlign.Right
                    HeaderCell.Text = Format(CDbl(sqty.ToString()), "#,##0 ; (#,##0)")
                    NewTotalRow.Cells.Add(HeaderCell)

                    HeaderCell = New TableCell()
                    HeaderCell.HorizontalAlign = HorizontalAlign.Right
                    HeaderCell.Text = Format(CDbl(swt.ToString()), "#,##0.00 ; (#,##0.00)")
                    NewTotalRow.Cells.Add(HeaderCell)

                Case Else
                    HeaderCell = New TableCell()
                    HeaderCell.HorizontalAlign = HorizontalAlign.Right
                    HeaderCell.Text = Format(CDbl(sqty.ToString()), "#,##0 ; (#,##0)")
                    NewTotalRow.Cells.Add(HeaderCell)

                    HeaderCell = New TableCell()
                    HeaderCell.HorizontalAlign = HorizontalAlign.Right
                    HeaderCell.Text = Format(CDbl(swt.ToString()), "#,##0.00 ; (#,##0.00)")
                    NewTotalRow.Cells.Add(HeaderCell)

            End Select

            HeaderCell = New TableCell()
            HeaderCell.HorizontalAlign = HorizontalAlign.Right
            HeaderCell.Text = ""
            NewTotalRow.Cells.Add(HeaderCell)

            HeaderCell = New TableCell()
            HeaderCell.HorizontalAlign = HorizontalAlign.Right
            HeaderCell.Text = Format(CDbl(samt.ToString()), "#,##0.00 ; (#,##0.00)")
            NewTotalRow.Cells.Add(HeaderCell)

            HeaderCell = New TableCell()
            HeaderCell.HorizontalAlign = HorizontalAlign.Right
            HeaderCell.Text = ""
            NewTotalRow.Cells.Add(HeaderCell)

            HeaderCell = New TableCell()
            HeaderCell.HorizontalAlign = HorizontalAlign.Right
            HeaderCell.Text = ""
            NewTotalRow.Cells.Add(HeaderCell)

            HeaderCell = New TableCell()
            HeaderCell.HorizontalAlign = HorizontalAlign.Right
            HeaderCell.Text = ""
            NewTotalRow.Cells.Add(HeaderCell)

            dgvPRpVD.Controls(0).Controls.AddAt(e.Row.RowIndex + rowIndex, NewTotalRow)
            rowIndex += 1
            TotRowIndex += 1

            sqty = 0
            swt = 0
            samt = 0


        End If

    End Sub

    Protected Sub dgvMMperMatl_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim tQty As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "qty").ToString())
            qty += tQty

            Dim tWt As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "wt").ToString())
            wt += tWt

            Dim tAmt As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "amt").ToString())
            amt += tAmt

            Dim tQty2 As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "qty").ToString())
            sqty += tQty2

            Dim tWt2 As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "wt").ToString())
            swt += tWt2

            Dim tAmt2 As Double = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "amt").ToString())
            samt += tAmt2

            venno = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "venno").ToString())
            strVenName = DataBinder.Eval(e.Row.DataItem, "venname").ToString()

        End If

        If e.Row.RowType = DataControlRowType.Footer Then
            Dim lblGTotal As Label = DirectCast(e.Row.FindControl("lblGTotal"), Label)
            lblGTotal.Text = "GRAND TOTAL"

            Dim lblTotalQty As Label = DirectCast(e.Row.FindControl("lblQty"), Label)
            lblTotalQty.Text = Format(CDbl(qty.ToString()), "#,##0 ; (#,##0)")
            qty = 0

            Dim lblTotalWt As Label = DirectCast(e.Row.FindControl("lblWt"), Label)
            lblTotalWt.Text = Format(CDbl(wt.ToString()), "#,##0.00 ; (#,##0.00)")
            wt = 0

            Dim lblTotalAmt As Label = DirectCast(e.Row.FindControl("lblAmt"), Label)
            lblTotalAmt.Text = Format(CDbl(amt.ToString()), "#,##0.00 ; (#,##0.00)")
            wt = 0

        End If
    End Sub

    Protected Sub dgvMMperMatl_RowCreated(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Private Sub btnGenerate_Command(sender As Object, e As CommandEventArgs) Handles btnGenerate.Command

    End Sub
End Class