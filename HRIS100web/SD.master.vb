Public Class SD
	Inherits System.Web.UI.MasterPage
	Dim dt As DataTable
	Dim admAuthor As String
	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		If vLoggedUserID Is Nothing Then
			Response.Redirect("login.aspx")
			Exit Sub
		End If

		If Not Me.IsPostBack Then
			lblUserID.Text = Session("UserID")
			lblGrp.Text = Session("UserGrp")

			'Dim UserDetails As UserDetails
			'UserDetails = Session("UserDetails")
			'lblUserID.Text = UserDetails.UserName
			'vLoggedUserID = UserDetails.UserName
			'lblGrp.Text = UserDetails.GrpName
			'vLoggedUserGroupID = UserDetails.GrpName
			'vTransMon = UserDetails.MonOpen
			'vTransYear = UserDetails.YrOpen

		End If

		vLoggedUserID = lblUserID.Text
		vLoggedUserGroupID = lblGrp.Text

		LoadCurrMon()

	End Sub

	Protected Sub AdmMsgBox(ByVal sMessage As String)
		Dim msg As String
		msg = "<script language='javascript'>"
		msg += "alert('" & sMessage & "');"
		msg += "<" & "/script>"
		Response.Write(msg)
	End Sub

	Private Sub LoadCurrMon()

		Dim strGLStat As String
		strGLStat = "OPEN"

		dt = GetDataTable("select dfrom from gltransmonstatus where yearstat = '" & strGLStat & "' " &
						  "and monstat = '" & strGLStat & "'")
		If Not CBool(dt.Rows.Count) Then
			Exit Sub
		Else
			For Each dr As DataRow In dt.Rows
				vTransMon = dr.Item(0).ToString()

			Next
		End If

		Call dt.Dispose()

		dt = GetDataTable("select dto from gltransmonstatus where yearstat = 'OPEN' " &
						  "and monstat = 'CLOSE' and yearendcltype <> 'Final' and transmon = '12'")
		If Not CBool(dt.Rows.Count) Then
			vTransYear = LastDayOfMonth(vTransMon)
		Else
			For Each dr As DataRow In dt.Rows
				vTransYear = dr.Item(0).ToString()

			Next
		End If

		Call dt.Dispose()

	End Sub

	Private Sub tvwMenuItems_SelectedNodeChanged(sender As Object, e As EventArgs) Handles tvwMenuItems.SelectedNodeChanged
		Session("UserID") = lblUserID.Text
		Session("UserGrp") = lblGrp.Text

		Select Case tvwMenuItems.SelectedNode.Text
			Case "SO Approval"
				If IsAllowed(lblGrp.Text, "018", 1) = True Then
					Response.Redirect("SOapp.aspx")
				Else
					AdmMsgBox("You're not allowed to this Module")
					Response.Redirect("SalesAndDist.aspx")
				End If

			Case "Sales Order"
				If IsAllowed(lblGrp.Text, "017", 1) = True Then
					Response.Redirect("SalesOrder.aspx")

				Else
					AdmMsgBox("You're not allowed to this Module")
					Response.Redirect("SalesAndDist.aspx")

				End If

			Case "Sales Invoice"
				If IsAllowed(lblGrp.Text, "019", 1) = True Then
					Response.Redirect("SalesInvoice.aspx")

				Else
					AdmMsgBox("You're not allowed to this Module")
					Response.Redirect("SalesAndDist.aspx")

				End If

			Case "Salesman's Access"
				If IsAllowed(lblGrp.Text, "076", 1) = True Then
					dt = GetDataTable("select ifnull(a.smnno,'ADM'),b.fullname from sys_userrecords a " &
									  "left join smnmtbl b on a.smnno=b.smnno " &
									  "where a.username = '" & lblUserID.Text & "'")
					If Not CBool(dt.Rows.Count) Then

					Else
						For Each dr As DataRow In dt.Rows
							Select Case dr.Item(0).ToString()
								Case "ADM"
									AdmMsgBox("For Salesman Used Only, Please Contact System Admin")
									Exit Sub
								Case Else
									vLogSmn = dr.Item(0).ToString() & Space(1) & dr.Item(1).ToString()
									vCurrForm = "Smn"
									Session("CForm") = vCurrForm
									Response.Redirect("SalesmanModule.aspx")

							End Select

						Next

					End If

					Call dt.Dispose()

				Else
					AdmMsgBox("You're not allowed to this Module")
					Response.Redirect("SalesAndDist.aspx")
				End If

			Case "Salesman's Access (Mobile)"
				If IsAllowed(lblGrp.Text, "076", 1) = True Then
					dt = GetDataTable("select ifnull(a.smnno,'ADM'),b.fullname from sys_userrecords a " &
									  "left join smnmtbl b on a.smnno=b.smnno " &
									  "where a.username = '" & lblUserID.Text & "'")
					If Not CBool(dt.Rows.Count) Then

					Else
						For Each dr As DataRow In dt.Rows
							Select Case dr.Item(0).ToString()
								Case "ADM"
									AdmMsgBox("For Salesman Used Only, Please Contact System Admin")
									Exit Sub
								Case Else
									vLogSmn = dr.Item(0).ToString() & Space(1) & dr.Item(1).ToString()
									vCurrForm = "Smn"
									Session("CForm") = vCurrForm
									Response.Redirect("SmnAccessMobile.aspx")

							End Select

						Next

					End If

					Call dt.Dispose()

				Else
					AdmMsgBox("You're not allowed to this Module")
					Response.Redirect("Home.aspx")
				End If


			Case "Sales Manager's Access"
				If IsAllowed(lblGrp.Text, "077", 1) = True Then
					vCurrForm = "Mgr"
					Session("CForm") = vCurrForm
					Response.Redirect("SalesmanModule.aspx")

				Else
					AdmMsgBox("You're not allowed to this Module")
					Response.Redirect("SalesAndDist.aspx")
				End If

			Case "Sales Manager's Access (Mobile)"
				If IsAllowed(lblGrp.Text, "077", 1) = True Then
					vCurrForm = "Mgr"
					Session("CForm") = vCurrForm
					Response.Redirect("SmnAccessMobile.aspx")

				Else
					AdmMsgBox("You're not allowed to this Module")
					Response.Redirect("Home.aspx")
				End If

			Case "SD Reports"
				If IsAllowed(lblGrp.Text, "017", 1) = True Then
					Response.Redirect("SDReports.aspx")

				Else
					AdmMsgBox("You're not allowed to this Module")
					Response.Redirect("SalesAndDist.aspx")

				End If

			Case Else
				AdmMsgBox("Not Yet Available")
				Response.Redirect("SalesAndDist.aspx")

		End Select
	End Sub

	Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
		If CheckBox1.Checked = True Then
			CheckBox1.Text = "Show Side Menu"
			Panel1.Visible = False
		Else
			CheckBox1.Text = "Hide Side Menu"
			Panel1.Visible = True
		End If
	End Sub
End Class