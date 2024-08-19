Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared

Public Class ReportViewer
	Inherits System.Web.UI.Page
	Private myTextObjectOnReport As CrystalDecisions.CrystalReports.Engine.TextObject
	Dim TransDate As DateTime
	Dim TransDate2 As DateTime
	Dim Title2 As String
	Dim TextDate As String



	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

		Dim UserDetails As UserDetails
		UserDetails = Session("UserDetails")

		Title2 = UserDetails.RepTitle
		TransDate = Format(CDate(UserDetails.TransDateFr), "yyyy-MM-dd")
		TransDate2 = Format(CDate(UserDetails.TransDateTo), "yyyy-MM-dd")
		TextDate = "For the Period " & Format(CDate(TransDate), "MMM dd, yyyy") & " to " & Format(CDate(TransDate2), "MMM dd, yyyy")

		Dim myReport As New ReportDocument()
		myReport.Load(Server.MapPath("~/Reports/Testing123.rpt.rpt"))

		'Set the values of the report parameters
		myReport.SetParameterValue("txtTitle", Title2)
		myReport.SetParameterValue("txtDate", TextDate)
		myReport.SetParameterValue("txtCoName", vLoggedCompanyID)

		CrystalReportViewer1.ReportSource = myReport
		'CrystalReportViewer1.DataBind()

	End Sub

End Class