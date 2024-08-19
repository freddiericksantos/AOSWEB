Public Class UserDetails

	Private Name As String
	Private UserID As String
	Private UserGrp As String
	Private TransMon As DateTime
	Private TransYr As DateTime
	Private Title As String
	Private DateFr As DateTime
	Private DateTo As DateTime

	Public Property RepTitle As String
		Get
			Return Me.Title
		End Get
		Set(value As String)
			Me.Title = value

		End Set

	End Property

	Public Property Fullname As String
		Get
			Return Me.Name
		End Get
		Set(value As String)
			Me.Name = value

		End Set

	End Property

	Public Property UserName As String
		Get
			Return Me.UserID
		End Get
		Set(value As String)
			Me.UserID = value

		End Set

	End Property

	Public Property GrpName As String
		Get
			Return Me.UserGrp
		End Get
		Set(value As String)
			Me.UserGrp = value

		End Set

	End Property

	Public Property MonOpen() As String
		Get
			Return Me.TransMon
		End Get
		Set(value As String)
			Me.TransMon = value

		End Set

	End Property

	Public Property YrOpen() As String
		Get
			Return Me.TransYr
		End Get
		Set(value As String)
			Me.TransYr = value

		End Set

	End Property

	Public Property TransDateFr() As String
		Get
			Return Me.DateFr
		End Get
		Set(value As String)
			Me.DateFr = value

		End Set

	End Property


	Public Property TransDateTo() As String
		Get
			Return Me.DateTo
		End Get
		Set(value As String)
			Me.DateTo = value

		End Set

	End Property


End Class
