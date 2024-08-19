Imports MySql.Data.MySqlClient
Imports System.Net

Module mdlDatabase
	Public cnnDBCnn As New ADODB.Connection
	Public strConnection As String
	Public strCCNo As String
	Public vLoggedUserID As String
	Public vLoggedUserFullName As String
	Public vLoggedUserGroupID As String
	Public vLoggedUserType As String
	Public vLoggedCompanyID As String
	Public vLoggedBussArea As String
	Public vLoggedBranch As String
	Public vLoggedBrCode As String
	Public vLoggedServer As String
	Public vLoggedMainCo As String
	Public vLoggedIn As Boolean
	Public vLogSmn As String
	Public vTransMon As Date
	Public vTransYear As Date
	Public vYearEnd As Date
	Public vFormOpen As String
	Public vJVsource As String
	Public vThisFormCode As String
	Public vThisMenuCode As String
	Public vDepartment As String
	Public vUpdatePopUp As String
	Public vLastAct As String
	Public admCurrID As String
	Public vEditMode As String
	Public vOpenFormCount As Integer
	Public vSysCurrVer As String
	Public vCurrForm As String
	Public vDbaseBA As String
	Public vActiveDebugLevel As String
	Public vEmpPickerForm As String
	Public vLoggedBA As String
	Public conn As MySqlConnection = New MySqlConnection()
	Public strConn As String
	Public strPass As String
	Public vDbase As String
	Public vSave As String
	Public vMonYear As String
	Public db_user As String
	Public db_port As String

	Public vDBaseDef As String
	Public vMenu As String
	Public vExitClose As String

	Public ReportToPrint As String
	Public vDocFormToPrint As String
	Public prtOption As String
	Public vReportStat As String = "This Module is not yet Available"
	Public vModulestat As String = "You're not Allowed in Module"
	Public vDocStat As String
	Public vDocNo As String
	Public vItmCnt As String
	Public vPC As String
	Public vTC As String

	Public strHostName As String = System.Net.Dns.GetHostName()
	Public strIPAddress As String = GetIpV4()
	Public strIPhost As String = strHostName & " / " & strIPAddress
	Public Answer As String
	Dim vLogDir As String
	Dim vLogFile As String
	Dim vlogFilePath As String
	Dim vLogText As String
	Dim admStatus As Label
	Public DeliveryToPrint As String

	Public Function Connect() As Boolean 'common connection
		Try
			strPass = "a1o0s0"
			'strConn = "89.117.139.204"
			conn.ConnectionString = "Server='" & strConn & "'; User = root; Password='" & strPass & "'; database='" & vDbase & "';"
			conn.Open()
			Return True

		Catch ex As MySqlException
			MsgBox(ErrorToString, MsgBoxStyle.Critical)
			Call DisConnect()
			Return False
		End Try

	End Function

	Public Sub DisConnect()
		If conn.State = ConnectionState.Open Then
			conn.Close()
		End If

	End Sub

	Public Function ExecuteNonQuery(ByVal sql As String) As Boolean
		If Not Connect() Then Return False
		Try
			Dim cmd As MySqlCommand = New MySqlCommand(sql, conn)
			cmd.CommandType = CommandType.Text
			Call cmd.ExecuteNonQuery()
		Catch ex As MySqlException
			MsgBox(ErrorToString, MsgBoxStyle.Critical)
			Return False
		Finally
			Call DisConnect()
		End Try


	End Function

	Public Function GetDataSet(ByVal sql As String) As DataSet
		If Not Connect() Then Return Nothing
		Try
			Dim cmd As MySqlCommand = New MySqlCommand(sql, conn)
			cmd.CommandType = CommandType.Text
			Dim da As MySqlDataAdapter = New MySqlDataAdapter(cmd)
			Dim ds As DataSet = New DataSet()
			Call da.Fill(ds)
			Call da.Dispose()
			Return ds
		Catch ex As MySqlException
			MsgBox(ErrorToString, MsgBoxStyle.Critical)
			Return Nothing
		Finally
			Call DisConnect()
		End Try

	End Function

	Public Function GetDataTable(ByVal sql As String) As DataTable
		If Not Connect() Then Return Nothing
		Try
			Dim cmd As MySqlCommand = New MySqlCommand(sql, conn)
			cmd.CommandType = CommandType.Text
			Dim da As MySqlDataAdapter = New MySqlDataAdapter(cmd)
			Dim dt As DataTable = New DataTable()
			Call da.Fill(dt)
			Call da.Dispose()
			Return dt
		Catch ex As MySqlException
			MsgBox(ErrorToString, MsgBoxStyle.Critical)
			Return Nothing
		Finally
			Call DisConnect()
		End Try
	End Function

	Public Function IsAllowed(ByVal vGroupCode As String, ByVal vFormCode As String, ByVal vAction As Integer) As Boolean
		Dim MyDA_sys_usergrouprights As New MySqlDataAdapter
		Dim MyDataTable As New DataTable
		Dim MySqlScript As String
		MySqlScript = "select * from sys_usergrouprights where gr_formcode = '" & vFormCode & "' and gr_groupcode = '" & vGroupCode & "'"
		Try
			MyDA_sys_usergrouprights = New MySqlDataAdapter(MySqlScript, conn)
			MyDA_sys_usergrouprights.Fill(MyDataTable)
		Catch ex As Exception
			MsgBox(ex.Message)
		End Try

		If MyDataTable.Rows.Count > 0 Then
			Select Case vAction
				'vaction note: 1 = view, 2 = edit, 3 = insert, 4 = delete, 5 = release, 6 = autosave, 7 = remember preference
				Case 1
					If MyDataTable(0)("gr_viewrights").ToString() = True Then
						IsAllowed = True
					Else
						IsAllowed = False
					End If
				Case 2
					If MyDataTable(0)("gr_editrights").ToString() = True Then
						IsAllowed = True
					Else
						IsAllowed = False
					End If
				Case 3
					If MyDataTable(0)("gr_insertrights").ToString() = True Then
						IsAllowed = True
					Else
						IsAllowed = False
					End If
				Case 4
					If MyDataTable(0)("gr_deleterights").ToString() = True Then
						IsAllowed = True
					Else
						IsAllowed = False
					End If
				Case 5
					If MyDataTable(0)("gr_releaserights").ToString() = True Then
						IsAllowed = True
					Else
						IsAllowed = False
					End If
				Case 6
					If MyDataTable(0)("gr_autosave").ToString() = True Then
						IsAllowed = True
					Else
						IsAllowed = False
					End If
				Case 7
					If MyDataTable(0)("gr_remember").ToString() = True Then
						IsAllowed = True
					Else
						IsAllowed = False
					End If
				Case Else
					IsAllowed = False
			End Select

		Else
			IsAllowed = False

		End If

	End Function

	Public Function gRepTbox(ByVal gText As TextBox) As String
		Try
			Select Case True
				Case gText.Text.Contains("'")
					gText.Text = gText.Text.Replace("'", "\'")

			End Select

		Catch ex As Exception

		End Try

	End Function

	Public Function gRepTboxUndo(ByVal gText As TextBox) As String
		Try
			Select Case True
				Case gText.Text.Contains("\'")
					gText.Text = gText.Text.Replace("\'", "'")

			End Select

		Catch ex As Exception

		End Try

	End Function

	Public Function base64Encode(ByVal sData As String) As String
		Try
			Dim encData_byte As Byte() = New Byte(sData.Length - 1) {}
			encData_byte = System.Text.Encoding.UTF8.GetBytes(sData)
			Dim encodedData As String = Convert.ToBase64String(encData_byte)
			Return (encodedData)
		Catch ex As Exception
			Throw (New Exception("Error in base64Encode" & ex.Message))
		End Try

	End Function

	Public Function base64Decode(ByVal sData As String) As String
		Dim encoder As New System.Text.UTF8Encoding()
		Dim utf8Decode As System.Text.Decoder = encoder.GetDecoder()
		Dim todecode_byte As Byte() = Convert.FromBase64String(sData)
		Dim charCount As Integer = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length)
		Dim decoded_char As Char() = New Char(charCount - 1) {}
		utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0)
		Dim result As String = New [String](decoded_char)
		Return result
	End Function

	Public Function DBConnect() As Boolean
		Try

			db_user = "root"
			db_port = "3306"
			strConnection = "Provider=MSDASQL.1;Persist Security Info=True;User ID=root;Driver={MySQL ODBC 3.51 Driver};DESC=;DATABASE=" & vDbase & ";SERVER=" & strConn & ";UID=" & db_user & ";PASSWORD=" & strPass & ";PORT=" & db_port & ""

			cnnDBCnn = New ADODB.Connection
			With cnnDBCnn
				.ConnectionString = strConnection
				.CursorLocation = ADODB.CursorLocationEnum.adUseClient
				.CommandTimeout = 180
				.Open()
			End With
			DBConnect = True
			Exit Function

		Catch ex As Exception
			MsgBox("Error in connection.", vbInformation, "Connection")
			DBConnect = False

		End Try

	End Function

	Public Function GetRecordSet(ByVal strQuery As String) As Object
		Dim cmdDBCmd As New ADODB.Command
		Dim rstDBRst As New ADODB.Recordset
		On Error GoTo ErrorHandler

		cmdDBCmd = New ADODB.Command
		rstDBRst = New ADODB.Recordset
		With cmdDBCmd
			.ActiveConnection = cnnDBCnn
			.CommandText = strQuery
		End With
		Call rstDBRst.Open(cmdDBCmd)

		GetRecordSet = rstDBRst

		rstDBRst = Nothing
		Exit Function
ErrorHandler:
		Call MsgBox("Error in recordset." & vbCrLf & Err.Description, vbInformation, "Recordset")
		GetRecordSet = Nothing
		rstDBRst = Nothing
		cmdDBCmd = Nothing

	End Function

	Public Function ExecQry(ByVal strQuery As String) As Boolean
		On Error GoTo ErrorHandler
		Call cnnDBCnn.Execute(strQuery)
		ExecQry = True
		Exit Function
ErrorHandler:
		Call MsgBox("Error in executing." & vbCrLf & Err.Description, vbInformation, "Execute")
		ExecQry = False
	End Function

	Public Function FirstDayOfMonth(ByVal sourceDate As DateTime) As DateTime
		Return New DateTime(sourceDate.Year, sourceDate.Month, 1)
	End Function

	Public Function LastDayOfMonth(ByVal sourceDate As DateTime) As DateTime
		Dim lastDay As DateTime = New DateTime(sourceDate.Year, sourceDate.Month, 1)
		Return lastDay.AddMonths(1).AddDays(-1)
	End Function

	Public Function FirstDayOfYear(ByVal sourceDate As DateTime) As DateTime
		Return New DateTime(sourceDate.Year, 1, 1)

	End Function

	Public Function LastDayOfYear(ByVal sourceDate As DateTime) As DateTime
		Dim lastDay As DateTime = New DateTime(sourceDate.Year, 1, 1)
		Return lastDay.AddYears(1).AddDays(-1)

	End Function

	Public Function FirstDayOf1stQrt(ByVal sourceDate As DateTime) As DateTime
		Return New DateTime(sourceDate.Year, 1, 1)

	End Function

	Public Function LastDayOf1stQrt(ByVal sourceDate As DateTime) As DateTime
		Dim lastDay As DateTime = New DateTime(sourceDate.Year, 1, 1)
		Return lastDay.AddMonths(3).AddDays(-1)

	End Function

	Public Function FirstDayOf2ndQrt(ByVal sourceDate As DateTime) As DateTime
		Return New DateTime(sourceDate.Year, 4, 1)

	End Function

	Public Function LastDayOf2ndQrt(ByVal sourceDate As DateTime) As DateTime
		Dim lastDay As DateTime = New DateTime(sourceDate.Year, 4, 1)
		Return lastDay.AddMonths(3).AddDays(-1)

	End Function

	Public Function FirstDayOf3rdQrt(ByVal sourceDate As DateTime) As DateTime
		Return New DateTime(sourceDate.Year, 7, 1)

	End Function

	Public Function LastDayOf3rdQrt(ByVal sourceDate As DateTime) As DateTime
		Dim lastDay As DateTime = New DateTime(sourceDate.Year, 7, 1)
		Return lastDay.AddMonths(3).AddDays(-1)

	End Function

	Public Function FirstDayOf4thQrt(ByVal sourceDate As DateTime) As DateTime
		Return New DateTime(sourceDate.Year, 10, 1)

	End Function

	Public Function LastDayOf4thQrt(ByVal sourceDate As DateTime) As DateTime
		Dim lastDay As DateTime = New DateTime(sourceDate.Year, 10, 1)
		Return lastDay.AddMonths(3).AddDays(-1)

	End Function

	Public Function GetIpV4() As String

		Dim myHost As String = Dns.GetHostName
		Dim ipEntry As IPHostEntry = Dns.GetHostEntry(myHost)
		Dim ip As String = ""

		For Each tmpIpAddress As IPAddress In ipEntry.AddressList
			If tmpIpAddress.AddressFamily = Sockets.AddressFamily.InterNetwork Then
				Dim ipAddress As String = tmpIpAddress.ToString
				ip = ipAddress
				Exit For
			End If
		Next

		If ip = "" Then
			Throw New Exception("No 10. IP found!")
		End If

		Return ip

	End Function

	Public Function GetLocalIP() As String
		Dim IPList As System.Net.IPHostEntry = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName)

		For Each IPaddress In IPList.AddressList
			'Only return IPv4 routable IPs
			If (IPaddress.AddressFamily = Sockets.AddressFamily.InterNetwork) AndAlso (Not IsPrivateIP(IPaddress.ToString)) Then
				Return IPaddress.ToString
			End If
		Next
		Return ""

	End Function

	Public Function IsPrivateIP(ByVal CheckIP As String) As Boolean
		Dim Quad1, Quad2 As Integer

		Quad1 = CInt(CheckIP.Substring(0, CheckIP.IndexOf(".")))
		Quad2 = CInt(CheckIP.Substring(CheckIP.IndexOf(".") + 1).Substring(0, CheckIP.IndexOf(".")))
		Select Case Quad1
			Case 10
				Return True
			Case 172
				If Quad2 >= 16 And Quad2 <= 31 Then Return True
			Case 192
				If Quad2 = 168 Then Return True
		End Select
		Return False

	End Function

	'Public Function RetrieveImageDB(ByRef img As Byte()) As Image
	'    Dim msRet As New System.IO.MemoryStream(img)
	'    RetrieveImageDB = System.Drawing.Image.FromStream(msRet)
	'    msRet.Close()

	'End Function

	Public Sub LogThisEventLevel(ByVal vEventDebugLevel As Byte, ByVal vEvent As String, ByVal vNotes As String)
		If vActiveDebugLevel = 0 Then Exit Sub
		If vEventDebugLevel <= vActiveDebugLevel Then
			Dim file As System.IO.StreamWriter
			Try
				file = My.Computer.FileSystem.OpenTextFileWriter(vLogDir & "\MyAppLog.log", True)
				file.WriteLine(Now().ToString("MM/dd/yyyy hh:mm:ss.fff tt") & ", " & vEvent & ", " & vNotes)
				file.Close()
			Catch ex As Exception
				file = My.Computer.FileSystem.OpenTextFileWriter(vLogDir & "\MyAppLog.log", True)
				file.WriteLine(Now().ToString("MM/dd/yyyy hh:mm:ss.fff tt") & ", " & ex.Message)
				file.Close()
			End Try
		End If
	End Sub

	Public Sub LogThisEvent(ByVal vEvent As String, ByVal vNotes As String)
		Dim file As System.IO.StreamWriter
		Try
			file = My.Computer.FileSystem.OpenTextFileWriter(vLogDir & "\MyAppLog.log", True)
			file.WriteLine(Now().ToString("MM/dd/yyyy hh:mm:ss.fff tt") & ", " & vEvent & ", " & vNotes)
			file.Close()
		Catch ex As Exception
			file = My.Computer.FileSystem.OpenTextFileWriter(vLogDir & "\MyAppLog.log", True)
			file.WriteLine(Now().ToString("MM/dd/yyyy hh:mm:ss.fff tt") & ", " & ex.Message)
			file.Close()
		End Try
	End Sub

	Public Sub WriteToLogs(ByVal WriteToLogs As String)
		vLogDir = "c:\updater\logs"
		vLogFile = "c:\updater\logs\mylogs.txt"
		vlogFilePath = "c:\updater\mylogs.txt"
		vLogText = DateTime.Now & Space(1) & vLoggedBranch & "(" & vLoggedBussArea & ")" & Space(1) & " Web Version" & Space(1) &
			vLoggedUserID & Space(1) & GetIpV4() & Space(1) & vLastAct

		Dim dir As New IO.DirectoryInfo(vLogDir)
		If dir.Exists Then

		Else
			IO.Directory.CreateDirectory(vLogDir)

		End If

		IO.File.SetAttributes(vLogDir, IO.FileAttributes.Hidden)

		Dim filePathLog As String = String.Format(vLogFile, DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss"))
		Using writer As New IO.StreamWriter(filePathLog, True)
			If IO.File.Exists(filePathLog) Then
				writer.WriteLine(vLogText)
			Else
				writer.WriteLine(vLogText)
			End If

		End Using

		Dim filePath As String = String.Format(vlogFilePath, DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss"))
		Using writer As New IO.StreamWriter(filePath, True)
			If IO.File.Exists(filePath) Then
				writer.WriteLine(vLogText)
			Else
				writer.WriteLine(vLogText)
			End If

		End Using

	End Sub

	Public Sub GenInintPage(ByVal admPage As Page) 'for single master

		admStatus = TryCast(admPage.Master.FindControl("lblUserName"), Label)
		admStatus.Text = UCase(vLoggedUserFullName)

		'admStatus = TryCast(admPage.Master.FindControl("lblBA"), Label)
		'admStatus.Text = UCase(vLoggedBussArea)

		admStatus = TryCast(admPage.Master.FindControl("lblCoName"), Label)
		admStatus.Text = UCase(vLoggedCompanyID)

	End Sub

	Public Sub admLogout(ByVal admPage As Page)
		admStatus = TryCast(admPage.Master.FindControl("lblUserName"), Label)
		admStatus.Text = "Log Out"

		'admStatus = TryCast(admPage.Master.FindControl("lblBA"), Label)
		'admStatus.Text = UCase(vLoggedBussArea)

		admStatus = TryCast(admPage.Master.FindControl("lblCoName"), Label)
		admStatus.Text = "My Company"

	End Sub
	Public Sub GenInintPageNest(ByVal admPage As Page) 'for nested

		'admStatus = TryCast(AOS100_main.Master.FindControl("lblUserName"), Label)
		'admStatus.Text = UCase(vLoggedUserFullName)

		'admStatus = TryCast(admPage.Master.FindControl("lblBA"), Label)
		'admStatus.Text = UCase(vLoggedBussArea)

		'admStatus = TryCast(admPage.Master.FindControl("lblCoName"), Label)
		'admStatus.Text = UCase(vLoggedCompanyID)

	End Sub


End Module