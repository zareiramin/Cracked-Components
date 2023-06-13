'-----------------------------------------------------------------------------
'                                                                            |
'                   Softing Industrial Automation GmbH                       |
'                        Richard-Reitzner-Allee 6                            |
'                           85540 Haar, Germany                              |
'                                                                            |
'                 This is a part of the Softing OPC Toolbox                  |
'       Copyright (c) 1998 - 2012 Softing Industrial Automation GmbH         |
'                           All Rights Reserved                              |
'                                                                            |
'-----------------------------------------------------------------------------
'-----------------------------------------------------------------------------
'                             OPC Toolbox NET                                |
'                                                                            |
'  Filename    : MySession.vb                                                |
'  Version     : 4.31                                                        |
'  Date        : 01-August-2012                                              |
'                                                                            |
'  Description : User's specific DataAccess OPC server                       |
'                DaSession definition                                        |
'                                                                            |
'-----------------------------------------------------------------------------
Imports System
Imports System.Collections
Imports System.Text
Imports Softing.OPCToolbox
Imports Softing.OPCToolbox.Server
Namespace Session
	Class MySession
		Inherits DaSession

#Region "Fields"
		Dim m_qualityGood As EnumQuality
#End Region

		Public Shared s_controlingSession As MySession = Nothing
		Private Shared s_clientCount As Int32 = 0
		Private m_clientSpecValue As ValueQT = Nothing

		Public Sub New(ByVal aType As EnumSessionType, ByVal aHandle As System.IntPtr)
			MyBase.New(aType, aHandle)
			m_qualityGood = [Enum].ToObject(GetType(EnumQuality), EnumQuality.GOOD)
			m_clientSpecValue = New ValueQT(s_clientCount, m_qualityGood, DateTime.Now)
			System.Math.Max(System.Threading.Interlocked.Increment(s_clientCount), s_clientCount - 1)
		End Sub

		Public ReadOnly Property Handle() As IntPtr
			Get
				Return MyBase.ObjectHandle
			End Get
		End Property

		Public Overloads Overrides Sub ConnectSession()

			MyBase.ConnectSession()

			Dim sessions As ArrayList = ArrayList.Synchronized(Console.m_sessions)
			sessions.Add(Me)
			Console.m_clientChanged.[Set]()

		End Sub

		Public Overloads Overrides Sub DisconnectSession()

			MyBase.ConnectSession()

			Dim sessions As ArrayList = ArrayList.Synchronized(Console.m_sessions)
			sessions.Remove(Me)
			Console.m_clientChanged.[Set]()

		End Sub

		Public Overloads Overrides Function Logoff() As Integer

			If Me.Equals(s_controlingSession) Then
				s_controlingSession = Nothing
			End If

			Console.m_clientChanged.[Set]()
			Return EnumResultCode.S_OK
		End Function

		Public Overloads Overrides Function Logon(ByVal UserName As String, ByVal Password As String) As Integer

			Dim ret As Integer = EnumResultCode.E_ACCESSDENIED

			If UserName = "OPC" AndAlso Password = "opc" Then

				If Me.Equals(s_controlingSession) OrElse s_controlingSession Is Nothing Then
					s_controlingSession = Me
					ret = EnumResultCode.S_OK
				End If
			End If

			Console.m_clientChanged.[Set]()
			Return ret
		End Function

		Public Function GetCacheValue(ByRef aCacheValue As ValueQT) As Integer

			aCacheValue = New ValueQT(m_clientSpecValue.Data, m_clientSpecValue.Quality, m_clientSpecValue.TimeStamp)
			Return EnumResultCode.S_OK
		End Function

	End Class
End Namespace

