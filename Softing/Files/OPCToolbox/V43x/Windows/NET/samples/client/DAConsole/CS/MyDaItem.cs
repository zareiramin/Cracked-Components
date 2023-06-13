//-----------------------------------------------------------------------------
//                                                                            |
//                   Softing Industrial Automation GmbH                       |
//                        Richard-Reitzner-Allee 6                            |
//                           85540 Haar, Germany                              |
//                                                                            |
//                 This is a part of the Softing OPC Toolbox                  |
//       Copyright (c) 1998 - 2012 Softing Industrial Automation GmbH         |
//                           All Rights Reserved                              |
//                                                                            |
//-----------------------------------------------------------------------------
//-----------------------------------------------------------------------------
//                             OPC Toolbox C#                                 |
//                                                                            |
//  Filename    : MyDaItem.cs		                                          |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description : OPC DA Item template class definition                       |
//                                                                            |
//-----------------------------------------------------------------------------
using System;
using System.Collections;
using Softing.OPCToolbox.Client;
using Softing.OPCToolbox;
using System.Threading;

namespace DaConsole
{
	public class MyDaItem : DaItem 
	{
		
		#region Constructor
		//-----------------

		public MyDaItem (string itemId, MyDaSubscription parentSubscription)  : base (itemId, parentSubscription) 
		{
			ValueChanged += new ValueChangedEventHandler(HandleValueChanged);
			StateChangeCompleted += new StateChangeEventHandler(HandleStateChanged);	
			PerformStateTransitionCompleted += new PerformStateTransitionEventHandler(HandlePerformStateTransition);
		}

		//--
		#endregion

		#region Private Members
		//---------------------

		#region Private Attributes
		//------------------------


		//--
		#endregion

		//--
		#endregion

		#region Public Methods
		//---------------------

		//--
		#endregion

		#region Public Properties
		//-----------------------


		//--
		#endregion

		#region Handles

		public static void HandleStateChanged(ObjectSpaceElement sender, EnumObjectState state)
		{
			MyDaItem item = sender as MyDaItem;				
			System.Console.WriteLine( item + " " + item.Id + " State Changed - " + state);
		} // end HandleStateChanged


		public static void HandleValueChanged(DaItem aDaItem, ValueQT aValue)
		{
			if (aDaItem.Id == "maths.sin")
			{
				System.Console.WriteLine("Value changed!");
				System.Console.WriteLine( String.Format("{0,-19} {1} {2,-50} ",aDaItem.Id,"-", aValue.ToString()));		
			}			
		} // end HandleValueChanged


		public static void HandlePerformStateTransition(
			ObjectSpaceElement sender, 
			uint executionContext, 
			int result)
		{			
			if(ResultCode.SUCCEEDED(result))
			{
				MyDaItem item = sender as MyDaItem;                    
				System.Console.WriteLine( sender + " " + item.Id + " Performed state transition - "  + executionContext );
			}
			else
			{
				System.Console.WriteLine(sender + "  Performed state transition failed! Result: " + result);
			}
		} // end HandlePerformStateTransition


		//--
		#endregion

	}	// end class MyDaItem


}	//	end namespace
