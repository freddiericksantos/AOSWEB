
Public Class SalesAndDist
	Inherits System.Web.UI.Page
	Dim dt As DataTable

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		If vLoggedUserID Is Nothing Then
			Response.Redirect("login.aspx")

		End If

		If Not Me.IsPostBack Then
			lblUser.Text = Session("UserID")
			lblGrpUser.Text = Session("UserGrp")

		End If

	End Sub

	Private Sub lbSalesOrder_Click(sender As Object, e As EventArgs) Handles lbSalesOrder.Click
		If IsAllowed(lblGrpUser.Text, "017", 1) = True Then
			Response.Redirect("SalesOrder.aspx")

		Else
			Response.Redirect("SalesAndDist.aspx")

		End If
	End Sub

	Private Sub lbSOapprov_Click(sender As Object, e As EventArgs) Handles lbSOapprov.Click
		If IsAllowed(lblGrpUser.Text, "018", 1) = True Then
			Response.Redirect("SOapp.aspx")

		Else
			Response.Redirect("SalesAndDist.aspx")

		End If
	End Sub

	Private Sub lbMgrAccess_Click(sender As Object, e As EventArgs) Handles lbMgrAccess.Click
		If IsAllowed(lblGrpUser.Text, "077", 1) = True Then
			vCurrForm = "Mgr"
			Session("CForm") = vCurrForm
			Response.Redirect("SalesmanModule.aspx")

		Else
			Response.Redirect("SalesAndDist.aspx")
		End If
	End Sub

	Private Sub lbSmnAccess_Click(sender As Object, e As EventArgs) Handles lbSmnAccess.Click
		If IsAllowed(lblGrpUser.Text, "076", 1) = True Then
			dt = GetDataTable("select ifnull(a.smnno,'ADM'),b.fullname from sys_userrecords a " &
							  "left join smnmtbl b on a.smnno=b.smnno " &
							  "where a.username = '" & lblUser.Text & "'")
			If Not CBool(dt.Rows.Count) Then

			Else
				For Each dr As DataRow In dt.Rows
					Select Case dr.Item(0).ToString()
						Case "ADM"
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
			Response.Redirect("SalesAndDist.aspx")
		End If

	End Sub

	Private Sub lbSDReport_Click(sender As Object, e As EventArgs) Handles lbSDReport.Click
		If IsAllowed(lblGrpUser.Text, "017", 1) = True Then
			Response.Redirect("SDReports.aspx")

		Else
			Response.Redirect("SalesAndDist.aspx")

		End If
	End Sub

	Private Sub lbMgrAccessM_Click(sender As Object, e As EventArgs) Handles lbMgrAccessM.Click
		If IsAllowed(lblGrpUser.Text, "077", 1) = True Then
			vCurrForm = "Mgr"
			Session("CForm") = vCurrForm
			Response.Redirect("SmnAccessMobile.aspx")

		Else
			Response.Redirect("Home.aspx")
		End If
	End Sub

	Private Sub lbSmnAccessM_Click(sender As Object, e As EventArgs) Handles lbSmnAccessM.Click
		If IsAllowed(lblGrpUser.Text, "076", 1) = True Then
			dt = GetDataTable("select ifnull(a.smnno,'ADM'),b.fullname from sys_userrecords a " &
							  "left join smnmtbl b on a.smnno=b.smnno " &
							  "where a.username = '" & lblUser.Text & "'")
			If Not CBool(dt.Rows.Count) Then

			Else
				For Each dr As DataRow In dt.Rows
					Select Case dr.Item(0).ToString()
						Case "ADM"
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
			Response.Redirect("Home.aspx")
		End If

	End Sub
End Class