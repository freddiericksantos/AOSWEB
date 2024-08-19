Public Class Admin
	Inherits System.Web.UI.MasterPage

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		If lblUserID.Text Is Nothing Then
			Response.Redirect("login.aspx")
			Exit Sub
		End If

		If Not Me.IsPostBack Then
			lblUserID.Text = Session("UserID")
			lblGrp.Text = Session("UserGrp")

		End If



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

	Protected Sub AdmMsgBox(ByVal sMessage As String)
		Dim msg As String
		msg = "<script language='javascript'>"
		msg += "alert('" & sMessage & "');"
		msg += "<" & "/script>"
		Response.Write(msg)
	End Sub


	Private Sub tvwMenuItems_SelectedNodeChanged(sender As Object, e As EventArgs) Handles tvwMenuItems.SelectedNodeChanged
		Session("UserID") = lblUserID.Text
		Session("UserGrp") = lblGrp.Text

		Select Case tvwMenuItems.SelectedNode.Text
			Case "Void Approval"
				If IsAllowed(lblGrp.Text, "003", 1) = True Then
					Response.Redirect("VoidApproval.aspx")
				Else
					AdmMsgBox("You're not allowed to this Module")
					Response.Redirect("Administrator.aspx")
				End If

			Case "User Manager"
				If IsAllowed(lblGrp.Text, "001", 1) = True Then
					Response.Redirect("UserMaintenance.aspx")
				Else
					AdmMsgBox("You're not allowed to this Module")
					Response.Redirect("Administrator.aspx")
				End If

			Case "Document Setup"

			Case "Clinic Monitoring"
				If IsAllowed(lblGrp.Text, "088", 1) = True Then
					Response.Redirect("ClinicMonitoring.aspx")
				Else
					AdmMsgBox("You're not allowed to this Module")
					Response.Redirect("Administrator.aspx")
				End If

			Case Else
				AdmMsgBox("Not Yet Available")
				Response.Redirect("Administrator.aspx")

		End Select
	End Sub
End Class