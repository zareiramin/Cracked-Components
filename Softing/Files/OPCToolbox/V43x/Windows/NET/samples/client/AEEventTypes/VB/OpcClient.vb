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
'  Filename    : OpcClient.vb	                                             |
'  Version     : 4.31                                                        |
'  Date        : 01-August-2012                                              |
'                                                                            |
'  Description : OPC Client template class definition                        |
'                                                                            |
'-----------------------------------------------------------------------------
Imports System
Imports System.Collections
Imports Softing.OPCToolbox.Client
Imports Softing.OPCToolbox
Imports System.Threading
Imports Microsoft.VisualBasic
Namespace AEEventTypes
	Public Class OpcClient

#Region "Constructor"
		'-----------------

		Public Sub New()
		End Sub

		'--
#End Region

#Region "Private Members"
		'---------------------

#Region "Private Attributes"
		'------------------------


		'--
#End Region

#Region "Private Methods"
		'---------------------


		'--
#End Region

		'--
#End Region

#Region "Public Methods"
		'---------------------

#Region "Public Attributes"
		'------------------------

		Public m_aeSession As MyAeSession = Nothing

		'--
#End Region

		Public Function GetApplication() As Application
			Return Application.Instance
		End Function

		Public Function Initialize() As Integer
			Dim traceGroupClient As EnumTraceGroup
			Dim traceGroupAll As EnumTraceGroup
			traceGroupClient = [Enum].ToObject(GetType(EnumTraceGroup), EnumTraceGroup.CLIENT)
			traceGroupAll = [Enum].ToObject(GetType(EnumTraceGroup), EnumTraceGroup.ALL)


			Dim result As Integer = EnumResultCode.S_OK
			'  TODO - design time license activation
			'  Fill in your design time license activation keys here
			'
			'  NOTE: you can activate one or all of the features at the same time

			'  activate the COM-AE Client Feature
			'  result = Application.Instance.Activate(EnumFeature.AE_CLIENT, "XXXX-XXXX-XXXX-XXXX-XXXX");
			If Not ResultCode.SUCCEEDED(result) Then
				Return result
			End If
			'  END TODO - design time license activation

			'  proceed with the OPC Toolbox core initialization
			result = GetApplication().Initialize()

			If ResultCode.SUCCEEDED(result) Then
				'  enable toolbox internal initialization
				GetApplication().EnableTracing(traceGroupAll, traceGroupAll, traceGroupClient, traceGroupClient, "Trace.txt", Convert.ToUInt32(1000000), _
		Convert.ToUInt32(0))
			End If
			Return result
		End Function

		Public Function ProcessCommandLine(ByVal commandLine As String) As Integer
			'  forward the command line arguments to the OPC Toolbox core internals
			Return Application.Instance.ProcessCommandLine(commandLine)
		End Function


		Public Sub Terminate()

			If m_aeSession.CurrentState <> EnumObjectState.DISCONNECTED Then
				m_aeSession.Disconnect(New ExecutionOptions)
			End If

			GetApplication().RemoveAeSession(m_aeSession)

			GetApplication().Terminate()
			m_aeSession = Nothing
		End Sub


		Public Function InitializeAeObjects() As Integer
			Dim connectResult As Integer = EnumResultCode.E_FAIL

			Try

				m_aeSession = New MyAeSession("opcae:///Softing.OPCToolboxDemo_ServerAE.1/{2E565243-B238-11D3-842D-0008C779D775}")

				connectResult = m_aeSession.Connect(True, True, Nothing)

			Catch exc As Exception
				Dim traceGroupUser As EnumTraceGroup
				traceGroupUser = [Enum].ToObject(GetType(EnumTraceGroup), EnumTraceGroup.USER)
				GetApplication().Trace(EnumTraceLevel.ERR, traceGroupUser, "OpcClient::InitializeAeObjects", exc.ToString())
			End Try

			Return connectResult
		End Function

		Public Sub Trace(ByVal traceLevel As EnumTraceLevel, ByVal traceGroup As EnumTraceGroup, ByVal objectID As String, ByVal message As String)
			Application.Instance.Trace(traceLevel, traceGroup, objectID, message)
		End Sub

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

	End Class

End Namespace

