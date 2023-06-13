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
//                             OPC Toolbox .NET                               |
//                                                                            |
//  Filename    : ClientValueComparer.cs                                      |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description : Comparer class to compare two ControlDAItem objects.        |
//                                                                            |
//-----------------------------------------------------------------------------

using System.Collections; 
using System.Collections.Specialized; 
using System.ComponentModel; 
using System;
using System.Data;
using System.Diagnostics;
using Softing.OPCToolbox.Client;

namespace Softing.OPCToolbox.Client.Control
{
	/// <summary>
	/// Comparer class for comparing two ControlDaItem objects.
	/// </summary>
	internal class ValueComparer: IComparer { 
		
		#region //Private Attributes
		//--------------------------

		private PropertyDescriptor m_SortProperty; 
		private ListSortDirection m_SortDirection; 
        
		//-
		#endregion

		#region //Public Methods
		//----------------------

		public ValueComparer( PropertyDescriptor SortProperty, ListSortDirection direction ) { 
			m_SortProperty = SortProperty; 
			m_SortDirection = direction; 
		}    
		
		
		public int Compare( object x, object y ) { 
			if ( ( !( x is ControlDaItem ) ) ) { 
				throw new ArgumentException( "Unexpected Argument.  Arguments must be of Type ControlDaItem", "x" ); 
			} 
			if ( ( !( y is ControlDaItem ) ) ) { 
				throw new ArgumentException( "Unexpected Argument.  Arguments must be of Type ControlDaItem", "y" ); 
			} 
			int result = CompareProperties( ( ( ControlDaItem )( x ) ), ( ( ControlDaItem )( y ) ) ); 
			return result; 
		} 
       
	
		//-
		#endregion

		#region //PrivateMethods
		//----------------------

		private int CompareProperties( ControlDaItem x, ControlDaItem y ) { 
			int result = 0; 
			int directionModifier = 0; 
			if ( ( m_SortDirection == ListSortDirection.Ascending ) ) { 
				directionModifier = 1; 
			} 
			else { 
				directionModifier = -1; 
			} 
			if ( ( x == null ) ) { 
				result = -1 * directionModifier; 
			} 
			else if ( ( y == null ) ) { 
				result = 1 * directionModifier; 
			} 
			else if ( ( System.Convert.ToDouble( m_SortProperty.GetValue( x ) ) < System.Convert.ToDouble( m_SortProperty.GetValue( y ) ) ) ) { 
				result = -1 * directionModifier; 
			} 
			else if ( ( System.Convert.ToDouble( m_SortProperty.GetValue( x ) ) > System.Convert.ToDouble( m_SortProperty.GetValue( y ) ) ) ) { 
				result = 1 * directionModifier; 
			} 
			else { 
				result = 0; 
			} 
			return result; 
		} 
        
		
		int System.Collections.IComparer.Compare( object x, object y ) { 
			return Compare( x, y );
		}

		//-
		#endregion

	} 
} 
