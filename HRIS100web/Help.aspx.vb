Public Class About
	Inherits System.Web.UI.Page

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		'GenInintPage(Me)

		If vLoggedUserID Is Nothing Then
			Response.Redirect("login.aspx")
			Exit Sub
		End If

		If Not Me.IsPostBack Then
			lblUser.Text = Session("UserID")
			lblGrpUser.Text = Session("UserGrp")

		End If


	End Sub

End Class