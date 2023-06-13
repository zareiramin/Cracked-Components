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
'  Filename    : MyAeSubscription.vb	                                     |
'  Version     : 4.31                                                        |
'  Date        : 01-August-2012                                              |
'                                                                            |
'  Description : OPC AE Subscription template class definition               |
'                                                                            |
'-----------------------------------------------------------------------------
Imports System
Imports System.Collections
Imports Softing.OPCToolbox.Client
Imports Softing.OPCToolbox
Imports System.Threading
Namespace AEConsole
	Public Class MyAeSubscription
		Inherits AeSubscription

#Region "Constructor"
		'-----------------

		Public Sub New(ByVal parentSession As MyAeSession)
			MyBase.New(parentSession)
			AddHandler PerformStateTransitionCompleted, AddressOf HandlePerformObjectStateTransitionCompleted
			AddHandler StateChangeCompleted, AddressOf HandleStateChangeCompleted
			AddHandler AeEventsReceived, AddressOf HandleEventsReceived
			AddHandler AeConditionsChanged, AddressOf HandleConditionsChanged
		End Sub

		'--
#End Region

#Region "Private Members"
		'---------------------

#Region "Private Attributes"
		'------------------------


		'--
#End Region


		'--
#End Region

#Region "Public Methods"
		'---------------------

		' creates the string representation of the AeCondition.State property
		Public Shared Function GetState(ByVal state As EnumConditionState) As String
			Dim stateToString As String = String.Empty
			If (state And EnumConditionState.ACKED) = EnumConditionState.ACKED Then
				stateToString &= "ACKED "
			End If
			If (state And EnumConditionState.ACTIVE) = EnumConditionState.ACTIVE Then
				stateToString &= "ACTIVE "
			End If
			If (state And EnumConditionState.ENABLED) = EnumConditionState.ENABLED Then
				stateToString &= "ENABLED "
			End If
			Return stateToString
		End Function

		' creates the string representation of the AeCondition.ChangeMask property
		Public Shared Function GetWhatChanged(ByVal change As EnumConditionChange) As String
			Dim changeToString As String = String.Empty
			If (Convert.ToInt16(change) And EnumConditionChange.ACK_DATA) = EnumConditionChange.ACK_DATA Then
				changeToString &= "ACK_DATA "
			End If
			If (Convert.ToInt16(change) And EnumConditionChange.ACK_STATE) = EnumConditionChange.ACK_STATE Then
				changeToString &= "ACK_STATE "
			End If
			If (Convert.ToInt16(change) And EnumConditionChange.ACTIVE_STATE) = EnumConditionChange.ACTIVE_STATE Then
				changeToString &= "ACTIVE_STATE "
			End If
			If (Convert.ToInt16(change) And EnumConditionChange.ATTRIBUTE) = EnumConditionChange.ATTRIBUTE Then
				changeToString &= "ATTRIBUTE "
			End If
			If (Convert.ToInt16(change) And EnumConditionChange.ENABLE_STATE) = EnumConditionChange.ENABLE_STATE Then
				changeToString &= "ENABLE_STATE "
			End If
			If (Convert.ToInt16(change) And EnumConditionChange.MESSAGE) = EnumConditionChange.MESSAGE Then
				changeToString &= "MESSAGE "
			End If
			If (Convert.ToInt16(change) And EnumConditionChange.QUALITY) = EnumConditionChange.QUALITY Then
				changeToString &= "QUALITY "
			End If
			If (Convert.ToInt16(change) And EnumConditionChange.SEVERITY) = EnumConditionChange.SEVERITY Then
				changeToString &= "SEVERITY "
			End If
			If (Convert.ToInt16(change) And EnumConditionChange.STATE) = EnumConditionChange.STATE Then
				changeToString &= "STATE "
			End If
			If (Convert.ToInt16(change) And EnumConditionChange.SUBCONDITION) = EnumConditionChange.SUBCONDITION Then
				changeToString &= "SUBCONDITION "
			End If
			Return changeToString
		End Function

		'--
#End Region

#Region "Public Properties"
		'-----------------------


		'--
#End Region

#Region "Handles"

		' method that handles the completion of performing the ObjectSpaceElement state transition
		Public Shared Sub HandlePerformObjectStateTransitionCompleted(ByVal obj As ObjectSpaceElement, ByVal executionContext As UInt32, ByVal result As Integer)
			If ResultCode.SUCCEEDED(result) Then
				System.Console.WriteLine(obj.ToString() + " Performed state transition " + executionContext.ToString())
			Else
				System.Console.WriteLine(obj.ToString() + " Performed state transition failed")
			End If
		End Sub

		' method that handles the completion of state changing of an ObjectSpaceElement
		Public Shared Sub HandleStateChangeCompleted(ByVal obj As ObjectSpaceElement, ByVal state As EnumObjectState)
			System.Console.WriteLine(obj.ToString() + " State Changed - " + state.ToString())
		End Sub

		' method that handles the AeSubscription.AeEventsReceived event; it displays on the console the received events
		Public Shared Sub HandleEventsReceived(ByVal anAeSubscription As AeSubscription, ByVal refresh As Boolean, ByVal lastRefresh As Boolean, ByVal events As AeEvent())
			System.Console.WriteLine()
			System.Console.WriteLine(anAeSubscription.ToString() + "  Events Received:" + events.Length.ToString())
			Dim i As Integer = 0
			While i < events.Length
				If (events(i).EventType = EnumEventType.SIMPLE) OrElse (events(i).EventType = EnumEventType.TRACKING) Then
					System.Console.WriteLine("  Event type: " + events(i).EventType.ToString())
					System.Console.WriteLine("  Event category: " + events(i).Category.ToString())
					System.Console.WriteLine("  Source path: " + events(i).SourcePath)
					System.Console.WriteLine("  Message: " + events(i).Message)
					System.Console.WriteLine("  Occurence time: " + events(i).OccurenceTime.ToString())
					System.Console.WriteLine("  Severity: " + events(i).Severity.ToString())
					System.Console.WriteLine("  Actor id: " + events(i).ActorId)
					Dim attributes As ArrayList = events(i).Attributes
					System.Console.WriteLine("     Attributes: " + attributes.Count.ToString())
					Dim j As Integer = 0
					While i < events(i).Attributes.Count
						System.Console.WriteLine(events(i).Attributes(j).ToString())
						System.Math.Max(System.Threading.Interlocked.Increment(j), j - 1)
					End While
				End If
				If events(i).EventType = EnumEventType.CONDITION Then
					System.Console.WriteLine("  Event type: " + events(i).EventType.ToString())
					System.Console.WriteLine("  Event category: " + events(i).Category.ToString())
					System.Console.WriteLine("  Source path: " + events(i).SourcePath)
					System.Console.WriteLine("  Message: " + events(i).Message)
					System.Console.WriteLine("  Occurence time: " + events(i).OccurenceTime.ToString())
					System.Console.WriteLine("  Severity: " + events(i).Severity.ToString())
					System.Console.WriteLine("  Actor id: " + events(i).ActorId)
					Dim attributes As ArrayList = events(i).Attributes
					System.Console.WriteLine("     Attributes: " + attributes.Count.ToString())
					Dim j As Integer = 0
					While i < events(i).Attributes.Count
						System.Console.WriteLine(events(i).Attributes(j).ToString())
						System.Math.Max(System.Threading.Interlocked.Increment(j), j - 1)
					End While
					System.Console.WriteLine("  Change mask: " + GetWhatChanged((DirectCast(events(i), AeCondition)).ChangeMask).ToString())
					Dim state As String = [String].Empty
					System.Console.WriteLine("  New state: " + GetState((DirectCast(events(i), AeCondition)).State).ToString())
					System.Console.WriteLine("  Ack Required: " + (DirectCast(events(i), AeCondition)).AckRequired.ToString())
					System.Console.WriteLine("  Quality: " + (DirectCast(events(i), AeCondition)).Quality.ToString())
					System.Console.WriteLine("  Condition name: " + (DirectCast(events(i), AeCondition)).ConditionName)
					System.Console.WriteLine("  Sub condition name: " + (DirectCast(events(i), AeCondition)).SubConditionName)
					System.Console.WriteLine("  Active time: " + (DirectCast(events(i), AeCondition)).ActiveTime.ToString())
				End If
				System.Math.Max(System.Threading.Interlocked.Increment(i), i - 1)
			End While
		End Sub

		' method that handles the AeSubscription.AeConditionsChanged event
		Public Shared Sub HandleConditionsChanged(ByVal anAeSubscription As AeSubscription, ByVal conditions As AeCondition())
			System.Diagnostics.Debug.WriteLine("Conditions changed")
		End Sub

		'--
#End Region

	End Class

End Namespace

