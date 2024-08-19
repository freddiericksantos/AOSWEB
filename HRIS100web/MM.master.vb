Public Class MM
	Inherits System.Web.UI.MasterPage
	Dim dt As DataTable

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


		If vLoggedUserID Is Nothing Then
			Response.Redirect("login.aspx")
			Exit Sub
		End If

		If Not Me.IsPostBack Then
			lblUserID.Text = Session("UserID")
			lblGrp.Text = Session("UserGrp")

		End If

		'vLoggedUserID = lblUserID.Text
		'vLoggedUserGroupID = lblGrp.Text

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
			Case "Issuance (DO)"
				If IsAllowed(lblGrp.Text, "007", 1) = True Then
					Response.Redirect("DeliveryOrder.aspx")
				Else
					AdmMsgBox("You're not allowed to this Module")
					Response.Redirect("MaterialManagement.aspx")
				End If

			Case "Inventory Reports"
				If IsAllowed(lblGrp.Text, "034", 1) = True Then
					Response.Redirect("InventoryReports.aspx")
				Else
					AdmMsgBox("You're not allowed to this Module")
					Response.Redirect("MaterialManagement.aspx")
				End If

			Case "Registers"
				If IsAllowed(lblGrp.Text, "034", 1) = True Then
					Response.Redirect("MMReports.aspx")
				Else
					AdmMsgBox("You're not allowed to this Module")
					Response.Redirect("MaterialManagement.aspx")
				End If

			Case "PO Reports"
				If IsAllowed(lblGrp.Text, "054", 1) = True Then
					Response.Redirect("POreports.aspx")
				Else
					AdmMsgBox("You're not allowed to this Module")
					Response.Redirect("MaterialManagement.aspx")
				End If

			Case "Lab Monitoring"
				If IsAllowed(lblGrp.Text, "087", 1) = True Then
					Response.Redirect("LabMonitoring.aspx")
				Else
					AdmMsgBox("You're not allowed to this Module")
					Response.Redirect("MaterialManagement.aspx")
				End If

			Case Else
				AdmMsgBox("Not Yet Available")
				Response.Redirect("MaterialManagement.aspx")

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