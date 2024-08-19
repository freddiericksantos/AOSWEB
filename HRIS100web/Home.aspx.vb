

Public Class Home
	Inherits System.Web.UI.Page

	Private Sub Home_Load(sender As Object, e As EventArgs) Handles Me.Load
		If vLoggedUserID Is Nothing Then
			Response.Redirect("login.aspx")
			Exit Sub
		End If

		If Not Me.IsPostBack Then
			vLoggedUserID = Session("UserID")
			vLoggedUserGroupID = Session("UserGrp")

		End If

	End Sub
End Class