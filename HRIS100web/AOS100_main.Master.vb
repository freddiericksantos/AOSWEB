Imports System
Imports System.Linq

Public Class aOS100_main
	Inherits System.Web.UI.MasterPage

	Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
		Response.Cache.SetExpires(DateTime.Now.AddMinutes(-1))
		Response.Cache.SetCacheability(HttpCacheability.NoCache)
		Response.Cache.SetNoStore()

	End Sub

	Protected Sub Application_BeginRequest()
		Response.Cache.SetCacheability(HttpCacheability.NoCache)
		Response.Cache.SetExpires(DateTime.Now.AddHours(-1))
		Response.Cache.SetNoStore()
	End Sub

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

		If Not Me.IsPostBack Then
			lblCoName.Text = vLoggedCompanyID
			'Dim UserDetails As UserDetails
			'UserDetails = Session("UserDetails")

			'lblUserName.Text = UserDetails.Fullname
			'lblUserID.Text = UserDetails.UserName
			'lblGrp.Text = UserDetails.GrpName
			'vTransMon = UserDetails.MonOpen
			'vTransYear = UserDetails.YrOpen

			lblUserName.Text = Session("Name")
			lblUserID.Text = Session("UserID")
			lblGrp.Text = Session("UserGrp")

		End If

		vLoggedUserID = lblUserID.Text
		vLoggedUserGroupID = lblGrp.Text

	End Sub

	Private Sub btnSCut_Click(sender As Object, e As EventArgs) Handles btnSCut.Click
		ShtCutProc()

	End Sub

	Protected Sub ShtCutProc()

		If vLoggedUserID Is Nothing Then
			Response.Redirect("Login.aspx")
			Exit Sub
		End If

		Select Case LCase(txtShtCut.Text)
			Case "me"
				txtShtCut.Text = vLoggedUserID

			Case "mygroup"
				txtShtCut.Text = vLoggedUserGroupID
				'Exit Sub
			Case "myco"
				txtShtCut.Text = vLoggedCompanyID

			Case "myba"
				txtShtCut.Text = vLoggedBussArea

			Case "tyear"
				txtShtCut.Text = Format(CDate(vTransYear), "yyyy-MM-dd")

			Case Else
				txtShtCut.Text = "Not Found..."

		End Select
	End Sub


	Private Sub TxtShtCut_Unload(sender As Object, e As EventArgs) Handles txtShtCut.Unload
		If Me.IsPostBack Then

		End If
	End Sub

	Private Sub Form1_Init(sender As Object, e As EventArgs) Handles form1.Init
		Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1))
		Response.Cache.SetCacheability(HttpCacheability.NoCache)
		Response.Cache.SetNoStore()
	End Sub

	Private Sub LblUserName_Unload(sender As Object, e As EventArgs) Handles lblUserName.Unload

	End Sub

	Private Sub txtShtCut_Load(sender As Object, e As EventArgs) Handles txtShtCut.Load

	End Sub

	Private Sub ImgLogo_Click(sender As Object, e As ImageClickEventArgs) Handles ImgLogo.Click
		Response.Redirect("Home.aspx")
	End Sub

	Private Sub txtShtCut_TextChanged(sender As Object, e As EventArgs) Handles txtShtCut.TextChanged
		ShtCutProc()

	End Sub

	Private Sub AOS100_main_Unload(sender As Object, e As EventArgs) Handles Me.Unload
		'HttpContext.Current.Session.Clear()
		'HttpContext.Current.Session.Abandon()
	End Sub
End Class