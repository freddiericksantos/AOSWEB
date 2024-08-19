
Public Class ChangePassword
	Inherits System.Web.UI.Page
	Dim dt As DataTable
	Dim sql As String
	Dim PwordCount As Integer

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		If vLoggedUserID Is Nothing Then
			Response.Redirect("login.aspx")

		End If

		If Not Me.IsPostBack Then
			vLoggedUserID = Session("UserID")
			vLoggedUserGroupID = Session("UserGrp")

		End If
	End Sub

	Protected Sub AdmMsgBox(ByVal sMessage As String)
		Dim msg As String
		msg = "<script language='javascript'>"
		msg += "alert('" & sMessage & "');"
		msg += "<" & "/script>"
		Response.Write(msg)
	End Sub

	Private Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
		lblLog_Error.Text = ""

		dt = GetDataTable("select userpassword,pwhint from sys_userrecords where username = '" & vLoggedUserID & "'")
		If Not CBool(dt.Rows.Count) Then
			lblLog_Error.Text = "Invalid User"
			Exit Sub
		Else
			For Each dr As DataRow In dt.Rows
				If dr.Item(0).ToString() <> base64Encode(txtCurrPass.Text) Then
					lblLog_Error.Text = "Current Password Not match, Hint: " & dr.Item(1).ToString() & ""
					Exit Sub

				ElseIf dr.Item(0).ToString() = base64Encode(txtPass1.Text) Then
					lblLog_Error.Text = "Password already Used, assign New Password"
					Exit Sub
				End If
			Next

		End If

		dt.Dispose()

		If txtPass1.Text <> txtPass2.Text Then
			lblLog_Error.Text = "Password not Match"
			Exit Sub
		ElseIf Len(txtPass2.Text) < 6 Then
			lblLog_Error.Text = "Password Must be at least 6 Characters or Numbers"
			Exit Sub

		End If

		SaveNewPass()
		SaveLogs()
		AdmMsgBox("New Password Set")
		Response.Redirect("Home.aspx")

	End Sub

	Protected Sub SaveLogs()

		sql = "INSERT INTO translog(trans,form,datetimelog,user,docno,tc)VALUES" &
			  "('Password Update','ChangePassword','" & Format(CDate(Now), "yyyy-MM-dd HH:mm:ss") & "'," &
			  "'" & vLoggedUserID & "','Set New Password','Password')"
		ExecuteNonQuery(sql)

	End Sub

	Protected Sub SaveNewPass()
		sql = "update sys_userrecords set userpassword  = '" & base64Encode(txtPass2.Text) & "'," &
			  "pwhint = '" & txtPWhint.Text & "' where username = '" & vLoggedUserID & "'"
		ExecuteNonQuery(sql)

	End Sub
End Class