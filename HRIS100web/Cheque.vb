Public Class Cheque
	Private Name As String
	Private UserID As String
	Private UserGrp As String
	Private TransMon As DateTime
	Private TransYr As DateTime
	Private Title As String
	Private CVdate As DateTime
	Private ChDate As DateTime

	Public Property RepTitle As String
		Get
			Return Me.Title
		End Get
		Set(value As String)
			Me.Title = value

		End Set

	End Property

	Public Property PayeeName As String
		Get
			Return Me.Title
		End Get
		Set(value As String)
			Me.Title = value

		End Set

	End Property

	Public Property chDate1 As String
		Get
			Return Me.Title
		End Get
		Set(value As String)
			Me.Title = value

		End Set

	End Property

	Public Property chAmount As String
		Get
			Return Me.Title
		End Get
		Set(value As String)
			Me.Title = value

		End Set

	End Property

	Public Property chAmountWord As String
		Get
			Return Me.Title
		End Get
		Set(value As String)
			Me.Title = value

		End Set

	End Property

End Class
