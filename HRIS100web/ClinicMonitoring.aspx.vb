Public Class ClinicMonitoring
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If vLoggedUserID Is Nothing Then
            Response.Redirect("login.aspx")

        End If

        If Not Me.IsPostBack Then
            vLoggedUserID = Session("UserID")
            vLoggedUserGroupID = Session("UserGrp")
            lblTitle.Text = TabPanel1.HeaderText
            lblMstrTab.Text = TabPanel5.HeaderText

        End If



    End Sub

    Protected Sub lbNew_Click(sender As Object, e As EventArgs)

    End Sub

    Protected Sub lbClose_Click(sender As Object, e As EventArgs)
        Response.Redirect("Administrator.aspx")

    End Sub

    Private Sub TabContainer1_ActiveTabChanged(sender As Object, e As EventArgs) Handles TabContainer1.ActiveTabChanged

        Select Case TabContainer1.ActiveTabIndex
            Case 0
                lblTitle.Text = TabPanel1.HeaderText

            Case 1
                lblTitle.Text = TabPanel2.HeaderText

            Case 2
                lblTitle.Text = TabPanel3.HeaderText

            Case 3
                lblTitle.Text = TabPanel4.HeaderText

        End Select

    End Sub

    Private Sub TabContainer2_ActiveTabChanged(sender As Object, e As EventArgs) Handles TabContainer2.ActiveTabChanged
        Select Case TabContainer2.ActiveTabIndex
            Case 0
                lblMstrTab.Text = TabPanel5.HeaderText

            Case 1
                lblMstrTab.Text = TabPanel6.HeaderText

        End Select


    End Sub
End Class