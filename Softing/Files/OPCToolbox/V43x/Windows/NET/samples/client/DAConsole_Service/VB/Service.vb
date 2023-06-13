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
'  Filename    : Service.vb                                                  |
'  Version     : 4.31                                                        |
'  Date        : 01-August-2012                                              |
'                                                                            |
'  Description : OPC Service main class implementation                       |
'                                                                            |
'-----------------------------------------------------------------------------
Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.ServiceProcess
Imports System.Threading
Imports System.IO
Imports Softing.OPCToolbox
Imports Softing.OPCToolbox.Client
Namespace DaConsole_Service
  Public Class OpcService
    Inherits System.ServiceProcess.ServiceBase

#Region "Private Attributes"
    '-------------------------

    Private Shared m_opcClient As OpcClient = Nothing
    '  The following constant holds the name of the Windows NT service that 
    '  runs the OPC application
    '    TODO : change your service name here    
		Private Const defaultServiceName As String = "DaConsole_Service OpcService"

    '--
#End Region

#Region "Public Properties"
    '------------------------

    Public ReadOnly Property OpcClient() As OpcClient
      Get
        Return m_opcClient
      End Get
    End Property

    '--
#End Region

#Region "Public Methods"
    '--------------------

    Public Sub CreateOpcClient()
      If m_opcClient Is Nothing Then
        m_opcClient = New OpcClient()
      End If
    End Sub
    '--
#End Region

    Public Sub New()
    End Sub

    ' The main entry point for the process
        Shared Sub Main()

            Dim result As Integer = EnumResultCode.S_OK
            Dim commandLineProcessed As Boolean = False
            Dim commandline As String = Environment.CommandLine

            Dim ServicesToRun As System.ServiceProcess.ServiceBase()
            Dim opcService As New OpcService
            ServicesToRun = New System.ServiceProcess.ServiceBase() {opcService}

            Dim traceGroupUser1 As EnumTraceGroup
            traceGroupUser1 = [Enum].ToObject(GetType(EnumTraceGroup), EnumTraceGroup.USER1)


            opcService.CreateOpcClient()
            m_opcClient.ServiceName = defaultServiceName
            '  initialize the client instance
            If Not ResultCode.SUCCEEDED(m_opcClient.Initialize()) Then
                m_opcClient = Nothing
                Return
            End If

            If Not commandLineProcessed Then
                result = m_opcClient.ProcessCommandLine(commandline)
                commandLineProcessed = True
                If result <> EnumResultCode.S_OK Then
                    If result = EnumResultCode.S_FALSE Then
                        'registration operation succesful
                        m_opcClient.Trace(EnumTraceLevel.INF, traceGroupUser1, "Service::Main", "Registration succeeded")
                    Else
                        m_opcClient.Trace(EnumTraceLevel.INF, traceGroupUser1, "Service::Main", "Registration failed")
                    End If

                    '  no matter what close the application if
                    'processCommandLine returned something different of S_OK        
                    m_opcClient.Terminate()
                    m_opcClient = Nothing

                    Return
                End If
            End If
            System.ServiceProcess.ServiceBase.Run(ServicesToRun)
        End Sub

        ''' <summary>
        ''' Clean up any resources being used.
        ''' </summary>
        Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
            MyBase.Dispose(disposing)
        End Sub

        ''' <summary>
        ''' Set things in motion so your service can do its work.
        ''' </summary>
        Protected Overloads Overrides Sub OnStart(ByVal args As String())
            Dim result As Integer = EnumResultCode.S_OK
            '  initialize the DA client simulation
            result = result Or m_opcClient.InitializeDaObjects()

        End Sub

        ''' <summary>
        ''' Stop this service.
        ''' </summary>
        Protected Overloads Overrides Sub OnStop()

            Try
                m_opcClient.Terminate()
                m_opcClient = Nothing
            Catch exc As Exception
                Dim fs As New FileStream("C:\ClientService.txt", FileMode.OpenOrCreate, FileAccess.Write)
                Dim streamWriter As New StreamWriter(fs)
                streamWriter.BaseStream.Seek(0, SeekOrigin.[End])
                streamWriter.WriteLine()
                streamWriter.WriteLine(exc.Message.ToString() + exc.Source.ToString() + exc.StackTrace.ToString() + exc.TargetSite.ToString())
                streamWriter.Flush()
                streamWriter.Close()
            End Try
        End Sub
    End Class
End Namespace

