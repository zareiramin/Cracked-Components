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
'  Filename    : OpcServer.vb                                                |
'  Version     : 4.31                                                        |
'  Date        : 01-August-2012                                              |
'                                                                            |
'  Description : OPC Server class                                            |
'                                                                            |
'-----------------------------------------------------------------------------
Imports System
Imports System.Collections
Imports System.Runtime.InteropServices
Imports System.Threading
Imports Softing.OPCToolbox
Imports Softing.OPCToolbox.Server
Namespace Session
	Public Class OpcServer

#Region "Fields"
		Dim m_qualityGood As EnumQuality
		Dim m_traceGroupUser1 As EnumTraceGroup
#End Region

#Region "Constructor"
		'-----------------

		Public Sub New()
			m_qualityGood = [Enum].ToObject(GetType(EnumQuality), EnumQuality.GOOD)
			m_traceGroupUser1 = [Enum].ToObject(GetType(EnumTraceGroup), EnumTraceGroup.USER1)
		End Sub

		'--
#End Region

#Region "Protected Attributes"
		'--------------------------

		Protected m_shutdownRequest As ShutdownHandler = Nothing
		Protected m_daSimulationElement1 As MyDaAddressSpaceElement = Nothing
		Protected m_daSimulationElement2 As MyDaAddressSpaceElement = Nothing
		Protected m_random As New Random(1)
		'--
#End Region

#Region "Public Properties"
		'-----------------------

		Public Property ServiceName() As String
			Get
				Return Application.Instance.ServiceName
			End Get
			Set(ByVal Value As String)
				Application.Instance.ServiceName = Value
			End Set
		End Property

		'--
#End Region

#Region "Public Methods"
		'-----------------------

		Public Shared Function Shutdown() As Integer
			Return EnumResultCode.S_OK
		End Function

		Public Function Initialize() As Integer
			Try
				Application.Instance.VersionOtb = 431
				Application.Instance.AppType = EnumApplicationType.EXECUTABLE

				Application.Instance.ClsidDa = "{9FEAA5EF-B361-4D7C-A28C-519F073F61A7}"
				Application.Instance.ProgIdDa = "Softing.Session.DA.1"
				Application.Instance.VerIndProgIdDa = "Softing.Session.DA"
				Application.Instance.Description = "Softing Session OPC Server"
				Application.Instance.MajorVersion = 4
				Application.Instance.MinorVersion = 31
				Application.Instance.BuildNumber = 0
				Application.Instance.VendorInfo = "Softing Industrial Automation GmbH"
				Application.Instance.MinUpdateRateDa = 1000
				Application.Instance.ClientCheckPeriod = 30000
				Application.Instance.AddressSpaceDelimiter = "."c
				Application.Instance.PropertyDelimiter = "/"c

				Application.Instance.IpPortHTTP = 8070
				Application.Instance.UrlDa = "/OPC/DA"
				AddHandler Application.Instance.ShutdownRequest, AddressOf Shutdown

			Catch exc As Exception
				Trace(EnumTraceLevel.ERR, m_traceGroupUser1, "OpcServer::Initialize", exc.ToString())

				Return EnumResultCode.E_FAIL
			End Try
			Return EnumResultCode.S_OK
		End Function

		Public Function Start() As Integer
			Return Application.Instance.Start()
		End Function

		Public Function [Stop]() As Integer
			Return Application.Instance.[Stop]()
		End Function

		Public Function Ready() As Integer
			Return Application.Instance.Ready()
		End Function

		Public Function Terminate() As Integer
			Return Application.Instance.Terminate()
		End Function

		Public Function Prepare(ByVal aMyCreator As MyCreator) As Integer

			Dim traceGroupServer As EnumTraceGroup
			Dim traceGroupAll As EnumTraceGroup
			traceGroupServer = [Enum].ToObject(GetType(EnumTraceGroup), EnumTraceGroup.SERVER)
			traceGroupAll = [Enum].ToObject(GetType(EnumTraceGroup), EnumTraceGroup.ALL)


			Dim result As Integer = EnumResultCode.S_OK
			'  TODO - design time license activation
			'  Fill in your design time license activation keys here
			'
			'  NOTE: you can activate one or all of the features at the same time

			'  activate the COM-DA Server feature
			'  result = Application.Instance.Activate(EnumFeature.DA_SERVER, "XXXX-XXXX-XXXX-XXXX-XXXX");
			If Not ResultCode.SUCCEEDED(result) Then
				Return result
			End If

			'  activate the XML-DA Server Feature
			'  result = Application.Instance.Activate(EnumFeature.XMLDA_SERVER, "XXXX-XXXX-XXXX-XXXX-XXXX");
			If Not ResultCode.SUCCEEDED(result) Then
				Return result
			End If
			'  END TODO - design time license activation

			result = Application.Instance.Initialize(aMyCreator)

			If ResultCode.SUCCEEDED(result) Then
				Application.Instance.EnableTracing(traceGroupAll, traceGroupAll, traceGroupServer, traceGroupServer, "Trace.txt", Convert.ToUInt32(1000000), _
				  Convert.ToUInt32(0))
			End If

			Return result
		End Function

		Public Function ProcessCommandLine(ByVal commandLine As String) As Integer
			Return Application.Instance.ProcessCommandLine(commandLine)
		End Function

		Public Function BuildAddressSpace() As Integer
			Try
				Dim creator As MyCreator = DirectCast(Application.Instance.Creator, MyCreator)

				'  DA        
				Dim daRoot As MyDaAddressSpaceRoot = DirectCast(Application.Instance.DaAddressSpaceRoot, MyDaAddressSpaceRoot)

				m_daSimulationElement1 = DirectCast(creator.CreateMyDaAddressSpaceElement(), MyDaAddressSpaceElement)
				m_daSimulationElement1.Name = "client specific"
				m_daSimulationElement1.AccessRights = EnumAccessRights.READABLE
				m_daSimulationElement1.Datatype = GetType(Integer)
				m_daSimulationElement1.IoMode = EnumIoMode.POLL_OWNCACHE
				daRoot.AddChild(m_daSimulationElement1)

				m_daSimulationElement2 = DirectCast(creator.CreateMyDaAddressSpaceElement(), MyDaAddressSpaceElement)
				m_daSimulationElement2.Name = "secured write"
				m_daSimulationElement2.AccessRights = EnumAccessRights.READWRITEABLE
				m_daSimulationElement2.Datatype = GetType(Integer)
				m_daSimulationElement2.IoMode = EnumIoMode.POLL
				daRoot.AddChild(m_daSimulationElement2)
				m_daSimulationElement2.ValueChanged(New ValueQT(2, m_qualityGood, DateTime.Now))

				m_daSimulationElement2.ValueChanged(New ValueQT(2, m_qualityGood, DateTime.Now))
			Catch exc As Exception
				Trace(EnumTraceLevel.ERR, m_traceGroupUser1, "OpcServer:BuildAddressSpace", exc.ToString())
				Return EnumResultCode.E_FAIL
			End Try

			Return EnumResultCode.S_OK
		End Function

		Public Sub ChangeSimulationValues()
			If Not m_daSimulationElement2 Is Nothing Then
				m_daSimulationElement2.ValueChanged(New ValueQT(m_random.[Next](5000), m_qualityGood, DateTime.Now))

			End If
		End Sub
		Public Sub Trace(ByVal traceLevel As EnumTraceLevel, ByVal traceGroup As EnumTraceGroup, ByVal objectID As String, ByVal message As String)
			Application.Instance.Trace(traceLevel, traceGroup, objectID, message)
		End Sub


		'--
#End Region

	End Class
End Namespace

