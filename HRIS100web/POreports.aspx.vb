Imports MySql.Data.MySqlClient

Public Class POreports
    Inherits System.Web.UI.Page
    Dim dt As DataTable
    Dim sql As String
    Dim strMov As String
    Dim MyDA_conn As New MySqlDataAdapter
    Dim MyDataSet As New DataSet
    Dim MySqlScript As String
    Dim admQty As Double
    Dim admWt As Double

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If lblUser.Text Is Nothing Then
            Response.Redirect("login.aspx")

        End If

        If Not Me.IsPostBack Then
            lblUser.Text = Session("UserID")
            lblGrpUser.Text = Session("UserGrp")

            'lblTitle.Text = TabPanel1.HeaderText

        End If
    End Sub

    Protected Sub lbNew_Click(sender As Object, e As EventArgs)

    End Sub

    Protected Sub lbClose_Click(sender As Object, e As EventArgs)

    End Sub
End Class