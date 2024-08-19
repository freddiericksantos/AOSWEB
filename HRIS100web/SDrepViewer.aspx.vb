Imports System.Data.SqlClient
Imports System.Data
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Configuration
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Collections
Imports CrystalDecisions.[Shared]


Public Class SDrepViewer
	Inherits System.Web.UI.Page
	Dim rsOrders As New ADODB.Recordset
	Dim rsInvoice As New ADODB.Recordset
	'Private myTextObjectOnReport As CrystalDecisions.CrystalReports.Engine.TextObject
	Dim myTextObjectOnReport As TextObject
	Private sqlda As SqlDataAdapter = New SqlDataAdapter()
	Private com As SqlCommand = New SqlCommand()
	Private dt As DataTable
	'Private ds As PrintDataSet1 = New PrintDataSet1()
	'Private rptDoc As ReportDocument = New ReportDocument()

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		Select Case DeliveryToPrint
			Case "SO"
				prtSO()
		End Select

	End Sub

	Public Sub bindCrystal()
		'Dim conn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("con1").ConnectionString)
		'dt = New DataTable()
		'dt.TableName = "Crystal Report Example"
		'com.Connection = conn
		'Dim sql As String = "SELECT * from Table1 "
		'com.CommandText = sql
		'sqlda = New SqlDataAdapter(com)
		'sqlda.Fill(dt)
		'ds.Tables(0).Merge(dt)
		'rptDoc.Load(Server.MapPath("SD_prtForm_SO.rpt"))
		'rptDoc.SetDatabaseLogon("Username", "password", "IPADDRESS\SERVERR2", "DB_DatabaseName")
		'rptDoc.Load(Server.MapPath("SD_prtForm_SO.rpt"))
		'CrystalReportViewer1.ReportSource = rptDoc
	End Sub

	Public Sub prtSO()
		Try



			'DBConnect()
			'Dim reportDocument As New ReportDocument()
			'reportDocument.Load(Server.MapPath("SD_prtForm_SO.rpt"))

			'myTextObjectOnReport = reportDocument.ReportDefinition.ReportObjects("txtDocNo")
			'myTextObjectOnReport.Text = vDocNo
			''Dim textObject As TextObject = ReportDocument.ReportDefinition.ReportObjects("TextObjectName")

			'rsOrders = GetRecordSet("select b.um,concat(a.codeno,space(1),b.mmdesc) as mmdesc,a.qty,a.wt,a.sp,a.itmamt as amt1 from sodettbl a " &
			'						"left join mmasttbl b on a.codeno=b.codeno where sono = '" & vDocNo & "' group by a.transid")
			'If rsOrders.RecordCount > 0 Then
			'	reportDocument.SetDataSource(rsOrders)
			'	CrystalReportViewer1.ReportSource = reportDocument
			'	'CrystalReportViewer1.RefreshReport()

			'Else
			'	MsgBox("No record Found")
			'	CrystalReportViewer1.ReportSource = Nothing
			'	Exit Sub

			'End If

			'reportDocument.SetDataSource(rsOrders)

			'' Bind the report to the CrystalReportViewer
			'CrystalReportViewer1.ReportSource = reportDocument
			If Not IsPostBack Then
				' Assuming you have a Cheque class with properties like PayeeName, Amount, Date, etc.
				Dim chequeData As New Cheque()
				chequeData.PayeeName = "John Doe"
				chequeData.chAmount = 1000.0
				chequeData.chDate1 = DateTime.Now.ToString("dd/MM/yyyy")

				' Create an instance of the Crystal Report
				Dim report As New ReportDocument()
				report.Load(Server.MapPath("~/Reports/ChequeReport.rpt")) ' Provide the correct path

				' Set the data source for the report
				report.SetParameterValue("PayeeName", chequeData.PayeeName)
				report.SetParameterValue("Amount", chequeData.chAmount)
				report.SetParameterValue("Date", chequeData.chDate1)

				' Set the CrystalReportViewer to display the report
				CrystalReportViewer1.ReportSource = report
			End If

		Catch ex As Exception

		End Try
	End Sub
End Class