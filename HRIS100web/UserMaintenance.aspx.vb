Option Explicit On
Imports MySql.Data.MySqlClient

Public Class UserMaintenance
    Inherits System.Web.UI.Page

    Dim dt As DataTable
    Dim MyDA_conn As New MySqlDataAdapter
    Dim MyDA_com_sections As New MySqlDataAdapter
    Dim MyDataSet As New DataSet
    Dim Answer As String
    Dim MySqlScript As String
    Dim sql As String
    Dim vResponse As String
    Dim admWFH As String
    Dim admLocDef As String
    Dim sqldata As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If lblUser.Text Is Nothing Then
            Response.Redirect("login.aspx")
            Exit Sub

        End If

        If Not Me.IsPostBack Then
            lblUser.Text = Session("UserID")
            lblGrpUser.Text = Session("UserGrp")

        End If

        vThisFormCode = "001"


        'TextBox3.Text = Nothing
        'TextBox2.Text = Nothing


    End Sub

    Private Sub ClearFields()
        vEditMode = False
        txtUserID.Text = ""

        txtUserFullname.Text = ""
        txtUserPass.Text = "123"
        lblPrevil.Text = ""

        txtUserEmailAdd.Text = ""
        txtUserTelNo.Text = ""
        txtUserNotes.Text = ""
        txtUserID.ReadOnly = False
        txtEmpNo.Text = ""

        dgvSideListView.SelectedIndex = -1

        dgvGrantedAccess.DataSource = Nothing
        dgvGrantedAccess.DataBind()

        CheckBox1.Checked = False
        CheckBox2.Checked = False
        RadioButton1.Checked = True

        popAutorization()
        fillDGVuser()

    End Sub

    Private Sub popAutorization()

        cboUserGroup.Items.Clear()
        dt = GetDataTable("select ug_code from sys_usergroup order by ug_code")
        If Not CBool(dt.Rows.Count) Then
            Exit Sub

        Else
            cboUserGroup.Items.Add("")
            For Each dr As DataRow In dt.Rows
                cboUserGroup.Items.Add(dr.Item(0).ToString())

            Next

        End If

        dt.Dispose()

    End Sub

    Protected Sub lbClose_Click(sender As Object, e As EventArgs)
        Response.Redirect("Administrator.aspx")

    End Sub

    Protected Sub lbNew_Click(sender As Object, e As EventArgs)
        ClearFields()

    End Sub

    Protected Sub dgvSideListView_RowDataBound(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Protected Sub dgvSideListView_RowCreated(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Private Sub UserMgt_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete


    End Sub

    Private Sub fillDGVuser()
        dgvSideListView.DataSource = Nothing
        dgvSideListView.DataBind()

        dt = GetDataTable("select username from sys_userrecords")
        If Not CBool(dt.Rows.Count) Then
            dgvSideListView.DataSource = Nothing
            dgvSideListView.DataBind()
            Exit Sub

        End If

        dt.Dispose()

        Dim adapter As New MySqlDataAdapter
        Dim ds As New DataSet()

        sqldata = Nothing

        sqldata = "select username from sys_userrecords order by username"

        conn.Open()
        Dim command As New MySqlCommand(sqldata, conn)
        adapter.SelectCommand = command
        adapter.Fill(ds)
        adapter.Dispose()
        command.Dispose()
        conn.Close()
        dgvSideListView.DataSource = ds.Tables(0)
        dgvSideListView.DataBind()


    End Sub

    Private Sub dgvSideListView_SelectedIndexChanged(sender As Object, e As EventArgs) Handles dgvSideListView.SelectedIndexChanged

        txtUserID.ReadOnly = False

        txtUserID.Text = dgvSideListView.SelectedRow.Cells(1).Text
        LoadUserDetails()

    End Sub

    Private Sub LoadUserDetails()

        dt = GetDataTable("select usergroup,userfullname,useremailaddress,usertelno,usernotes,userloc,smnno,empno,ifnull(wfh,'No')," &
                          "username from sys_userrecords where username = '" & dgvSideListView.SelectedRow.Cells(1).Text & "'")
        If Not CBool(dt.Rows.Count) Then
            Exit Sub
        Else
            For Each dr As DataRow In dt.Rows
                cboUserGroup.Text = dr.Item(0).ToString() & ""
                lblPrevil.Text = dr.Item(0).ToString() & ""
                txtUserFullname.Text = dr.Item(1).ToString() & ""

                txtUserEmailAdd.Text = dr.Item(2).ToString() & ""
                txtUserTelNo.Text = dr.Item(3).ToString() & ""
                txtUserNotes.Text = dr.Item(4).ToString() & ""

                Select Case dr.Item(5).ToString() & ""
                    Case "IN"
                        RadioButton1.Checked = True
                    Case "OUT"
                        RadioButton2.Checked = True
                End Select

                txtSmnNo.Text = dr.Item(6).ToString() & ""

                If txtSmnNo.Text = "" Then
                    CheckBox1.Checked = False
                Else
                    CheckBox1.Checked = True
                End If

                txtEmpNo.Text = dr.Item(7).ToString() & ""

                Select Case dr.Item(8).ToString() & ""
                    Case "Yes"
                        CheckBox2.Checked = True
                    Case Else
                        CheckBox2.Checked = False

                End Select

            Next

        End If

        dt.Dispose()

        txtUserID.ReadOnly = True

        fill_dgvGrantedAccess()

        txtUserPass.ReadOnly = False
        txtUserPass.Text = "***********"
        txtUserPass.ReadOnly = True

    End Sub

    Private Sub fill_dgvGrantedAccess()

        dgvGrantedAccess.DataSource = Nothing
        dgvGrantedAccess.DataBind()

        dt = GetDataTable("select * from sys_usergrouprights where gr_groupcode = '" & cboUserGroup.Text & "'")
        If Not CBool(dt.Rows.Count) Then
            dgvGrantedAccess.DataSource = Nothing
            dgvGrantedAccess.DataBind()
            Exit Sub

        End If

        dt.Dispose()

        Dim adapter As New MySqlDataAdapter
        Dim ds As New DataSet()

        sqldata = Nothing

        sqldata = "select distinct concat(a.gr_formcode,space(1),b.description) as formcode from sys_usergrouprights a " &
                  "left join sys_forms b on a.gr_formcode=b.code where a.gr_groupcode = '" & cboUserGroup.Text & "' order by b.description"

        conn.Open()
        Dim command As New MySqlCommand(sqldata, conn)
        adapter.SelectCommand = command
        adapter.Fill(ds)
        adapter.Dispose()
        command.Dispose()
        conn.Close()
        dgvGrantedAccess.DataSource = ds.Tables(0)
        dgvGrantedAccess.DataBind()

    End Sub

    Protected Sub dgvGrantedAccess_RowDataBound(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Protected Sub dgvGrantedAccess_RowCreated(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Protected Sub OnConfirm(sender As Object, e As EventArgs)
        Dim confirmValue As String = Request.Form("confirm_value")
        If confirmValue = "Yes" Then
            Dim admResPass = "123"

            sql = "update sys_userrecords set userpassword = '" & base64Encode(admResPass) & "' " &
                  "where username = '" & txtUserID.Text & "'"
            ExecuteNonQuery(sql)

            lblMsg.Text = "User ID: " & txtUserID.Text & " Password RESET"

        Else
            lblMsg.Text = "Action Aborted"
        End If

        'admCurrID = Nothing

    End Sub

    Private Sub lbSave_Click(sender As Object, e As EventArgs) Handles lbSave.Click
        If RadioButton1.Checked = False And RadioButton2.Checked = False Then
            lblMsg.Text = ("Select User Location Default")
            Exit Sub
        End If

        If Trim(txtUserID.Text) = "" Then
            lblMsg.Text = ("USERNAME Field is REQUIRED")
            Exit Sub
        ElseIf txtUserNotes.Text = "" Then
            lblMsg.Text = ("Designation is Blank")
            Exit Sub
        ElseIf cboUserGroup.Text = "" Then
            lblMsg.Text = ("Assign User Group Access")
            Exit Sub

        End If

        If CheckBox1.Checked = True Then
            If txtSmnNo.Text = "" Then
                lblMsg.Text = ("Salesman No. is Blank")
                Exit Sub
            End If
        End If

        SaveUserProc()

    End Sub

    Protected Sub OnConfirm2(sender As Object, e As EventArgs)

        Dim confirmValue As String = Request.Form("confirm_value")
        If confirmValue = "Yes" Then
            lblMsg.Text = "here"

        Else
            lblMsg.Text = "Action Aborted"
        End If
    End Sub


    Private Sub SaveUserProc()
        Select Case True
            Case RadioButton1.Checked
                admLocDef = "IN"
            Case RadioButton2.Checked
                admLocDef = "OUT"
        End Select

        If CheckBox2.Checked = True Then
            admWFH = "Yes"
        Else
            admWFH = "No"
        End If

        dt = GetDataTable("select userfullname from sys_userrecords where username = '" & txtUserID.Text & "'")
        If Not CBool(dt.Rows.Count) Then
            SaveUser()
            lblMsg.Text = ("New User Added")
        Else
            UpdateUser()
            lblMsg.Text = ("User UPDATE SAVED")
        End If

        dt.Dispose()

        fillDGVuser()

    End Sub

    Private Sub SaveUser()
        sql = "insert into sys_userrecords(username,userfullname,userpassword,usergroup,useremailaddress,usertelno," &
              "usernotes,userloc,stat,smnno,empno,wfh,user,pdate)values('" & txtUserID.Text & "','" & txtUserFullname.Text & "'," &
              "'" & base64Encode("123") & "','" & cboUserGroup.Text & "','" & txtUserEmailAdd.Text & "','" & txtUserTelNo.Text & "'," &
              "'" & txtUserNotes.Text & "','" & admLocDef & "','Active','" & txtSmnNo.Text & "','" & txtEmpNo.Text & "'," &
              "'" & admWFH & "','" & vLoggedUserID & "','" & Format(CDate(Now()), "yyyy-MM-dd HH:mm:ss") & "')"
        ExecuteNonQuery(sql)

    End Sub

    Private Sub UpdateUser()
        sql = "update sys_userrecords set userfullname = '" & txtUserFullname.Text & "',usergroup = '" & cboUserGroup.Text & "'," &
              "useremailaddress = '" & txtUserEmailAdd.Text & "',usertelno = '" & txtUserTelNo.Text & "'," &
              "usernotes = '" & txtUserNotes.Text & "',userloc = '" & admLocDef & "',stat = 'Active',smnno = '" & txtSmnNo.Text & "'," &
              "empno = '" & txtEmpNo.Text & "',wfh = '" & admWFH & "',user = '" & vLoggedUserID & "'," &
              "pdate = '" & Format(CDate(Now()), "yyyy-MM-dd HH:mm:ss") & "' where username = '" & txtUserID.Text & "'"
        ExecuteNonQuery(sql)

    End Sub

End Class