Option Explicit On
Imports System.IO
Imports MySql.Data.MySqlClient
Imports System.Net
Imports System
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration

Public Class login
	Inherits System.Web.UI.Page
	Dim sql As String
	Dim dt As DataTable
	Dim MyDA_sys_userrecords As New MySqlDataAdapter
	Dim MyDataSet As New DataSet
	Dim MySqlScript As String
	Dim Addrs As String
	Dim admStatus As Label

	Protected Sub GenInintPageLo(ByVal admPageLo As Page) 'for single master
		admStatus = Nothing

		admStatus = TryCast(admPageLo.Master.FindControl("lblUserName"), Label)
		admStatus.Text = UCase("logout")

		'admStatus = TryCast(admPageLo.Master.FindControl("lblCoName"), Label)
		'admStatus.Text = UCase("Company Name")

	End Sub

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1))
		Response.Cache.SetCacheability(HttpCacheability.NoCache)
		Response.Cache.SetNoStore()
		'HttpContext.Current.Session.Clear()
		'HttpContext.Current.Session.Abandon()
		vLoggedUserID = Nothing
		GenInintPageLo(Me)
		'admLogout(Me)
		strConn = "89.117.139.204"
		strPass = "P@55w0rd2024"
		vLoggedUserID = Nothing
		lblLog_Error.Visible = False
		lblStrike.Visible = False

		If Not Me.IsPostBack Then
			'getIPadd()
			cboCompany.Items.Clear()
			GetCompany()

		End If

		txtPassWord.TextMode = TextBoxMode.Password

	End Sub

	Protected Sub GetIPadd()
		Try
			'cboDbase.Items.Clear()
			'Dim sr As StreamReader = New StreamReader(Server.MapPath("\App_Data\ServerAddresses.ini"))
			'Do
			'    Addrs = sr.ReadLine()
			'    If Trim(Addrs) <> "" Then
			'        cboDbase.Items.Add(Addrs & "")

			'    End If

			'Loop Until Addrs Is Nothing
			'sr.Close()


		Catch Ex As Exception
			' Let the user know what went wrong.
			' MsgBox("The file could not be read:")
			MsgBox(Ex.Message())

		End Try
	End Sub

	Protected Sub GetCompany()
		Try
			Dim sr As New StreamReader(Server.MapPath("\App_Data\Companies.ini"))
			Do
				Addrs = sr.ReadLine()
				If Trim(Addrs) <> "" Then
					cboCompany.Items.Add(Addrs & "")
				End If
			Loop Until Addrs Is Nothing
			sr.Close()


		Catch E As Exception
			' Let the user know what went wrong.
			' MsgBox("The file could not be read:")
			MsgBox(E.Message())
		End Try

	End Sub

	Protected Sub GetDbase()
		Try
			Dim sr As New StreamReader(Server.MapPath("\App_Data\DBase_" & cboCompany.SelectedItem.Value & ".ini"))
			Do
				Addrs = sr.ReadLine()
				If Trim(Addrs) <> "" Then
					vDbase = (Addrs & "")

				End If

			Loop Until Addrs Is Nothing
			sr.Close()

		Catch Ex As Exception
			' Let the user know what went wrong.
			' MsgBox("The file could not be read:")
			MsgBox(Ex.Message())

		End Try


	End Sub

	Public Sub GenInintPage(ByVal admPage As Page) 'for single master

		admStatus = TryCast(admPage.Master.FindControl("lblUserName"), Label)
		admStatus.Text = UCase(txtUserID.Text)

		admStatus = TryCast(admPage.Master.FindControl("lblBA"), Label)
		admStatus.Text = UCase(vLoggedBussArea)

		admStatus = TryCast(admPage.Master.FindControl("lblCoName"), Label)
		admStatus.Text = UCase(vLoggedCompanyID)

	End Sub

	Protected Sub BtnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click

		'strPass = "P@55w0rd2024"
		'strConn = "89.117.139.204"
		'strPass = "P@55w0rd2024"
		strConn = "localhost"

		'getDbase()

		Select Case cboCompany.Text.Substring(0, 4)
			Case "8100"
				vDbase = "bfi"

			Case "8200"
				vDbase = "bfi_bicol"

			Case "8300"
				vDbase = "bfi_rbt"

		End Select

		If txtUserID.Text = "" Then
			lblLog_Error.Text = "User ID is Blank"
			Exit Sub

		ElseIf txtPassWord.Text = "" Then
			lblLog_Error.Text = "Password is Blank"
			Exit Sub

		End If

		Connect()

		SyncUserRecords()

		DisConnect()

		Try
			Dim vRowToView As DataRow = MyDataSet.Tables("sys_userrecords").Rows.Find(txtUserID.Text)
			If vRowToView IsNot Nothing Then
				If Trim(txtPassWord.Text) = base64Decode(vRowToView("userpassword").ToString()) Then
					vLoggedUserID = vRowToView("username").ToString()
					vLoggedUserFullName = vRowToView("userfullname").ToString()
					vLoggedUserGroupID = vRowToView("usergroup").ToString()
					vLoggedServer = strConn

					Dim UserDetails As New UserDetails
					UserDetails.Fullname = vLoggedUserFullName
					UserDetails.UserName = vLoggedUserID
					UserDetails.GrpName = vLoggedUserGroupID
					UserDetails.MonOpen = vTransMon
					UserDetails.YrOpen = vTransYear
					Session("UserDetails") = UserDetails

					Session("Name") = vLoggedUserFullName
					Session("UserID") = vLoggedUserID
					Session("UserGrp") = vLoggedUserGroupID

					'vUserType = vRowToView("usertype").ToString()
					LogginCompany()

					GenInintPage(Me)

					txtPassWord.Text = ""
					txtUserID.Text = ""

					Response.Redirect("Home.aspx")
					strIPAddress = GetIpV4()

					vMenu = "ADM"

				Else

					DisConnect()
					lblStrike.Text = CLng(lblStrike.Text) + 1
					vLoggedIn = False

					If CLng(lblStrike.Text) > 2 Then
						lblLog_Error.Visible = True
						AdmMsgBox("3 Attemps Exceeded, your User ID is now LOCKED, please contact your System Admin to reset your password")
						'vLastAct = " 3 Attemps - ACCESS DENIED " & UCase(txtUserID.Text)
						'WriteToLogs(vLastAct)
						lblStrike.Text = "0"
						UserIDlock()
						DisConnect()
						Response.Redirect("http://rothmanagoncillo.webs.com")
						btnLogin.Enabled = False
					Else
						'vLastAct = " Try # " & lblStrike.Text & " - ACCESS DENIED " & UCase(txtUserID.Text)
						'WriteToLogs(vLastAct)
						lblLog_Error.Visible = True
						lblLog_Error.Text = "InvalidPassword, Try Again, Strike: " & lblStrike.Text
						txtPassWord.Text = ""
						txtPassWord.Focus()

						Exit Sub

					End If
				End If

			Else
				lblLog_Error.Visible = True
				lblLog_Error.Text = "Invalid Username"
				Exit Sub
			End If

		Catch ex As Exception

		End Try



	End Sub

	Private Sub AdmMsgBox(ByVal sMessage As String)
		Dim msg As String
		msg = "<script language='javascript'>"
		msg += "alert('" & sMessage & "');"
		msg += "<" & "/script>"
		Response.Write(msg)
	End Sub

	Protected Sub UserIDlock()
		sql = "update sys_userrecords set userpassword = '" & base64Encode("admLock") & "',idstat = 'Lock' " &
			  "where username = '" & txtUserID.Text & "'"
		ExecuteNonQuery(sql)

	End Sub

	Protected Sub LogginCompany()

		DisConnect()
		dt = GetDataTable("select ba,bussname,branch from batbl where shrtname = '" & vDbase & "'")
		If Not CBool(dt.Rows.Count) Then Exit Sub

		For Each dr As DataRow In dt.Rows
			vLoggedBussArea = dr.Item(0).ToString() & ""
			Session("BA") = vLoggedBussArea
			vLoggedCompanyID = cboCompany.Text
			vLoggedBranch = dr.Item(2).ToString() & ""
			vLoggedBA = vDbase

		Next

		dt.Dispose()

		Dim strGLStat As String
		strGLStat = "OPEN"

		dt = GetDataTable("select dfrom from gltransmonstatus where yearstat = '" & strGLStat & "' " &
						  "and monstat = '" & strGLStat & "'")
		If Not CBool(dt.Rows.Count) Then MsgBox("Not found") : Exit Sub

		For Each dr As DataRow In dt.Rows
			vTransMon = Format(CDate(dr.Item(0).ToString()), "yyyy-MM-dd")

		Next

		Call dt.Dispose()

		dt = GetDataTable("select dto from gltransmonstatus where yearstat = 'OPEN' " &
						  "and monstat = 'CLOSE' and yearendcltype <> 'Final' and transmon = '12'")
		If Not CBool(dt.Rows.Count) Then
			vTransYear = LastDayOfMonth(vTransMon)
		Else
			For Each dr As DataRow In dt.Rows
				vTransYear = Format(CDate(dr.Item(0).ToString()), "yyyy-MM-dd")

			Next
		End If

		Call dt.Dispose()

	End Sub

	Private Sub SyncUserRecords()
		Try
			MyDataSet.Tables.Remove("sys_userrecords")
		Catch ex As Exception

		End Try

		MySqlScript = "select * From sys_userrecords"

		Try
			MyDA_sys_userrecords = New MySqlDataAdapter(MySqlScript, conn)
			MyDA_sys_userrecords.Fill(MyDataSet, "sys_userrecords")

		Catch ex As Exception
			MsgBox(ex.Message, MsgBoxStyle.Critical)
			Exit Sub
		End Try

		Dim vPrimaryCol(1) As DataColumn
		vPrimaryCol(0) = MyDataSet.Tables("sys_userrecords").Columns("username")
		MyDataSet.Tables("sys_userrecords").PrimaryKey = vPrimaryCol

	End Sub

	Private Sub Login_Unload(sender As Object, e As EventArgs) Handles Me.Unload
		DisConnect()
		'HttpContext.Current.Session.Clear()
		'HttpContext.Current.Session.Abandon()
	End Sub
End Class