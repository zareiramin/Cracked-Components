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
//                             OPC Toolbox C++                                |
//                                                                            |
//  Filename    : OpcServer.cs		                                          |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description : OPC Server class                                            |
//                                                                            |
//-----------------------------------------------------------------------------
using System;
using System.Configuration;
using System.IO;
using System.Collections;
using System.Runtime.InteropServices;
using System.Threading;
using Softing.OPCToolbox;
using Softing.OPCToolbox.Server;

namespace Softing.OPCToolbox.DemoServer
{
	public class OpcServer
	{
		#region Constructor
		//-----------------
		public OpcServer()
		{
			
		}	//	end constructor
		//--
		#endregion

        #region Protected Attributes

        private Byte m_second = 0;
        private Byte m_minute = 0;

        #endregion

        #region Private Attributes

        private DemoDaAddressSpaceElement m_keyElement = null;
		
		private ArrayList m_elemetList = new ArrayList();

        #endregion

        #region Protected Attributes
        //--------------------------

		protected ShutdownHandler m_shutdownRequest = null;
        protected AeCategory m_timeSlotCategory     = null;

		//--
		#endregion

		#region Public Properties
		//-----------------------

		public string ServiceName
		{
			get
			{
				return Application.Instance.ServiceName;
			}
			set
			{
				Application.Instance.ServiceName = value;
			}
		}	//	end SetServiceName

		//--
		#endregion

		#region Public Methods
		//-----------------------

		public static int Shutdown()
		{
			return (int)EnumResultCode.S_OK;
		}	//	end Shutdown

		public int Initialize()
		{
			try
			{
                string clsidDa = ConfigurationManager.AppSettings["ClsidDa"];
                string clsidAe = ConfigurationManager.AppSettings["ClsidAe"];
                string description = ConfigurationManager.AppSettings["Description"];

				Application.Instance.VersionOtb = 431;
				Application.Instance.AppType = EnumApplicationType.EXECUTABLE;

                Application.Instance.ClsidDa = clsidDa;
				
				Application.Instance.ProgIdDa = "56Softing.OPCToolbox.NET.CS.Sample.DemoServer.DA.1";
				Application.Instance.VerIndProgIdDa = "56Softing.OPCToolbox.NET.CS.Sample.DemoServer.DA";
                Application.Instance.ClsidAe = clsidAe;
				Application.Instance.ProgIdAe = "56Softing.OPCToolbox.NET.CS.Sample.DemoServer.AE.1";
				Application.Instance.VerIndProgIdAe = "56Softing.OPCToolbox.NET.CS.Sample.DemoServer.AE";
                Application.Instance.Description = description;
				Application.Instance.MajorVersion = 1;
				Application.Instance.MinorVersion = 0;
				Application.Instance.BuildNumber = 0;
				Application.Instance.VendorInfo = "56Softing Industrial Automation GmbH";
				Application.Instance.MinUpdateRateDa = 100;
				Application.Instance.ClientCheckPeriod = 30000;
				Application.Instance.AddressSpaceDelimiter = '.';
				Application.Instance.PropertyDelimiter = '/';
				Application.Instance.SupportDisableConditions = true;
				
				Application.Instance.IpPortHTTP = 8079;
				Application.Instance.UrlDa = "/OPC/DA";
				Application.Instance.ShutdownRequest += new Softing.OPCToolbox.Server.ShutdownHandler(Shutdown);
			}
			catch(Exception exc)
			{
				Trace(
					EnumTraceLevel.ERR,
					EnumTraceGroup.USER1,
					"OpcServer::Initialize",
					exc.ToString());

				return (int)EnumResultCode.E_FAIL;
			}	//	end try...catch
			return (int)EnumResultCode.S_OK;
		}	//	end Initialize

		public int Start()
		{
			return Application.Instance.Start();
		}	//	end Start

		public int Stop()
		{
			return Application.Instance.Stop();
		}	//	end Stop

		public int Ready()
		{
			return Application.Instance.Ready();
		}	//	end Ready

		public int Terminate()
		{
			return Application.Instance.Terminate();
		}	//	end Terminate

		public int Prepare(DemoCreator aMyCreator)
		{
            Application.Instance.Activate(EnumFeature.DA_CLIENT, "a");
            Application.Instance.Activate(EnumFeature.DA_SERVER, "a");
            Application.Instance.Activate(EnumFeature.AE_CLIENT, "a");
            Application.Instance.Activate(EnumFeature.AE_SERVER, "a");
            int result = Application.Instance.Initialize(aMyCreator);
		    System.Console.WriteLine(result);
			if (ResultCode.SUCCEEDED(result))
			{
				Application.Instance.EnableTracing(
					EnumTraceGroup.ALL,
					EnumTraceGroup.ALL,
					EnumTraceGroup.SERVER,
					EnumTraceGroup.SERVER,
					"Trace.txt",
					1000000,
					0);
			}	//	end if

			return result;
		}	//	end Prepare

		public int ProcessCommandLine(string commandLine)
		{
			return Application.Instance.ProcessCommandLine(commandLine);
		}	//	end ProcessCommandLine

        DemoDaAddressSpaceElement dynamicElements;
        public int BuildDAAddressSpace()
        {
            try
            {
                DemoCreator creator = (DemoCreator)Application.Instance.Creator;
                DaAddressSpaceRoot daRoot = Application.Instance.DaAddressSpaceRoot;
                DemoDaAddressSpaceElement allDataTypesElement = creator.CreateBasicElement("all data types");
                allDataTypesElement.HasChildren = true;
                if (!daRoot.AddChild(allDataTypesElement))
                {
                    return (int)EnumResultCode.E_FAIL;
                }
                dynamicElements = creator.CreateBasicElement("dynamic");
                dynamicElements.HasChildren = true;
                if (!allDataTypesElement.AddChild(dynamicElements))
                {
                    return (int)EnumResultCode.E_FAIL;
                }
                #region Child Variables for dynamic
                DemoDaAddressSpaceElement tag = null;


                tag = creator.CreateBasicDynamicElement(typeof(System.Int64));
                tag.Name = "Tag int64";
                if (!dynamicElements.AddChild(tag))
                {
                    return (int)EnumResultCode.E_FAIL;
                }
                tag.Init();
                m_elemetList.Add(tag);

                tag = creator.CreateBasicDynamicElement(typeof(System.Int32));
                tag.Name = "Tag int32";
                if (!dynamicElements.AddChild(tag))
                {
                    return (int)EnumResultCode.E_FAIL;
                }
                tag.Init();
                m_elemetList.Add(tag);

                tag = creator.CreateBasicDynamicElement(typeof(System.Int16));
                tag.Name = "Tag int16";
                if (!dynamicElements.AddChild(tag))
                {
                    return (int)EnumResultCode.E_FAIL;
                }
                tag.Init();
                m_elemetList.Add(tag);

                #endregion

            }
            catch (Exception exc)
            {
                Trace(
                    EnumTraceLevel.ERR,
                    EnumTraceGroup.USER1,
                    "OpcServer:;BuildAddressSpace",
                    exc.ToString());
                return (int)EnumResultCode.E_FAIL;
            }	//	end try...catch

            return (int)EnumResultCode.S_OK;
        }

        public int BuildAEAddressSpace()
        {
            int result = (int)EnumResultCode.S_OK;
            
            //AE
            DemoCreator creator = (DemoCreator)Application.Instance.Creator;
            AeAddressSpaceRoot aeRoot = Application.Instance.AeAddressSpaceRoot;

            AeAddressSpaceElement keyboardElement = creator.CreateDemoAeAddressSpaceElement();
            keyboardElement.Name = "keyboard";
            keyboardElement.HasChildren = false;
            if (!aeRoot.AddChild(keyboardElement))
            {
                return (int)EnumResultCode.E_FAIL;
            }

            AeAddressSpaceElement mouseElement = creator.CreateDemoAeAddressSpaceElement();
            mouseElement.Name = "mouse";
            mouseElement.HasChildren = true;
            if(!aeRoot.AddChild(mouseElement))
            {
                return (int)EnumResultCode.E_FAIL;
            }

            AeAddressSpaceElement rightButtonElement = creator.CreateDemoAeAddressSpaceElement();
            rightButtonElement.Name = "right button";
            rightButtonElement.HasChildren = false;
            if(!mouseElement.AddChild(rightButtonElement))
            {
                return (int)EnumResultCode.E_FAIL;
            }

            AeAddressSpaceElement leftButtonElement = creator.CreateDemoAeAddressSpaceElement();
            leftButtonElement.Name = "left button";
            leftButtonElement.HasChildren = false;
            if(!mouseElement.AddChild(leftButtonElement))
            {
                return (int)EnumResultCode.E_FAIL;
            }

            AeAddressSpaceElement timeElement = creator.CreateDemoAeAddressSpaceElement();
            timeElement.Name = "time";
            timeElement.HasChildren = true;
            if(!aeRoot.AddChild(timeElement))
            {
                return (int)EnumResultCode.E_FAIL;
            }

            AeAddressSpaceElement localElement = creator.CreateDemoAeAddressSpaceElement();
            localElement.Name = "local";
            localElement.HasChildren = true;
            if(!timeElement.AddChild(localElement))
            {
                return (int)EnumResultCode.E_FAIL;
            }

            AeAddressSpaceElement secondElement = creator.CreateDemoAeAddressSpaceElement();
            secondElement.Name = "second";
            secondElement.HasChildren = false;
            if(!localElement.AddChild(secondElement))
            {
                return (int)EnumResultCode.E_FAIL;
            }

            AeAddressSpaceElement minuteElement = creator.CreateDemoAeAddressSpaceElement();
            minuteElement.Name = "minute";
            minuteElement.HasChildren = false;
            if(!localElement.AddChild(minuteElement))
            {
                return (int)EnumResultCode.E_FAIL;
            }

            return result;
        }

        public int BuildAEEventCategories()
        {
            try
            {
                int result = (int)EnumResultCode.E_FAIL;
                // time tick
                AeCategory timeTickCategory = new AeCategory(
                    EnumEventType.SIMPLE,
                    Console.CATEGORY_ID_TIME_TICK,
                    "time tick");
                result = Application.Instance.AddAeCategory(timeTickCategory);
                if (result == (uint)EnumResultCode.S_OK)
                {
                    timeTickCategory.AddAttribute(1, "hour",    typeof(System.UInt16));
                    timeTickCategory.AddAttribute(2, "minute",  typeof(System.UInt16));
                    timeTickCategory.AddAttribute(3, "second",  typeof(System.UInt16));
                }
                // key hit
                AeCategory keyHitCategory = new AeCategory(
                    EnumEventType.TRACKING,
                    Console.CATEGORY_ID_KEY_HIT,
                    "key hit");
                result = Application.Instance.AddAeCategory(keyHitCategory);
                if (result == (uint)EnumResultCode.S_OK)
                {
                    keyHitCategory.AddAttribute(1, "key", typeof(System.UInt16));
                }
                // time slot
                m_timeSlotCategory = new AeCategory(
                    EnumEventType.CONDITION,
                    Console.CATEGORY_ID_TIME_SLOT,
                    "time slot");
                result = Application.Instance.AddAeCategory(m_timeSlotCategory);
                if (result == (uint)EnumResultCode.S_OK)
                {
                    m_timeSlotCategory.AddAttribute(1, "second", typeof(System.UInt16));
                    m_timeSlotCategory.AddConditionDefinition(Console.COND_NAME_BETWEEN_SINGLE);
                    m_timeSlotCategory.AddConditionDefinition(Console.COND_NAME_BETWEEN_MULTIPLE);
                    m_timeSlotCategory.AddSubConditionDefinition(Console.COND_NAME_BETWEEN_MULTIPLE, "[10,30]");
                    AeSubConditionDefinition subConditionDefinition = m_timeSlotCategory.GetSubConditionDefinition(
                        Console.COND_NAME_BETWEEN_MULTIPLE, "[10,30]");
                    if (subConditionDefinition != null)
                    {
                        subConditionDefinition.Description = "[10,30]";
                        subConditionDefinition.Severity = 100;
                    }
                    m_timeSlotCategory.AddSubConditionDefinition(Console.COND_NAME_BETWEEN_MULTIPLE, "[30,50]");
                    subConditionDefinition = m_timeSlotCategory.GetSubConditionDefinition(
                        Console.COND_NAME_BETWEEN_MULTIPLE, "[30,50]");
                    if (subConditionDefinition != null)
                    {
                        subConditionDefinition.Description = "[30,50]";
                        subConditionDefinition.Severity = 200;
                    }
                }
                //alarm simulation simple
                AeCategory alarmSimpleCategory = new AeCategory(
                    EnumEventType.SIMPLE,
                    Console.CATEGORY_ID_DA_WRITE_SIMPLE,
                    "alarm simulation simple");
                result = Application.Instance.AddAeCategory(alarmSimpleCategory);
                //alarm simulation tracking
                AeCategory alarmTrackingCategory = new AeCategory(
                    EnumEventType.TRACKING,
                    Console.CATEGORY_ID_DA_WRITE_TRACKING,
                    "alarm simulation simple");
                result = Application.Instance.AddAeCategory(alarmTrackingCategory);
            }
            catch (Exception exc)
            {
                Trace(
                    EnumTraceLevel.ERR,
                    EnumTraceGroup.USER1,
                    "OpcServer::BuildEventCategories",
                    exc.ToString());
                return (int)EnumResultCode.E_FAIL;
            }	//	end try...catch

            return (int)EnumResultCode.S_OK;

        }	//	end BuildAEEventCategories

		public int BuildAddressSpace()
		{
            int result = (int)EnumResultCode.E_FAIL;
            result = BuildDAAddressSpace();
            if (result != (uint)EnumResultCode.S_OK)
            {
                return result;
            }
            result = BuildAEAddressSpace();
            if (result != (uint)EnumResultCode.S_OK)
            {
                return result;
            }
            result = BuildAEEventCategories();
            return result;
		}	//	end BuildAddressSpace

		public void FireSimulationEvents()
		{
            String message = null;

            AeCondition condition1 = new AeCondition();
            condition1.Category = m_timeSlotCategory;
            condition1.Name = Console.COND_NAME_BETWEEN_SINGLE;
            condition1.Definition = m_timeSlotCategory.GetConditionDefinition(Console.COND_NAME_BETWEEN_SINGLE);
            condition1.SourcePath = "clock.time slot 1";
            condition1.Quality = EnumQuality.GOOD;
            condition1.UserData = 1;

            AeCondition condition2 = new AeCondition();
            condition2.Category = m_timeSlotCategory;
            condition2.Name = Console.COND_NAME_BETWEEN_MULTIPLE;
            condition2.Definition = m_timeSlotCategory.GetConditionDefinition(Console.COND_NAME_BETWEEN_MULTIPLE);
            condition2.SourcePath = "clock.time slot 2";
            condition2.Quality = EnumQuality.GOOD;
            condition2.UserData = 1;

            if (m_second != DateTime.Now.Second)
            {
                ArrayList conditionList = new ArrayList();
                ArrayList eventAttributes = new ArrayList();

                switch (DateTime.Now.Second)
                {
                    case 10:
                        eventAttributes.Add(DateTime.Now.Second);
                        condition1.Severity = 510;
                        message = "between [10,50]";
                        condition1.Message = message;
                        condition1.AckRequired = true;
                        condition1.EventAttributes = eventAttributes;
                        condition1.ChangeMask = (Byte)(EnumConditionChange.ACTIVE_STATE |
                            EnumConditionChange.SEVERITY |
                            EnumConditionChange.MESSAGE |
                            EnumConditionChange.ATTRIBUTE);
                        condition1.StateChange = EnumConditionState.ACTIVE;
                        conditionList.Add(condition1);

                        condition2.Severity = 500;
                        message = "between [10,30]";
                        condition2.Message = message;
                        condition2.AckRequired = true;
                        condition2.ActiveSubConditionName = "[10,30]";
                        condition2.EventAttributes = eventAttributes;
                        condition2.ChangeMask = (Byte)(EnumConditionChange.ACTIVE_STATE |
                            EnumConditionChange.SEVERITY |
                            EnumConditionChange.MESSAGE |
                            EnumConditionChange.SUBCONDITION |
                            EnumConditionChange.ATTRIBUTE);
                        condition2.StateChange = EnumConditionState.ACTIVE;
                        conditionList.Add(condition2);
                        break;
                    case 30:
                        eventAttributes.Add(DateTime.Now.Second);
                        condition2.Severity = 540;
                        message = "between [30,50]";
                        condition2.Message = message;
                        condition2.AckRequired = true;
                        condition2.ActiveSubConditionName = "[30,50]";
                        condition2.EventAttributes = eventAttributes;
                        condition2.ChangeMask = (Byte)(EnumConditionChange.ACTIVE_STATE |
                            EnumConditionChange.SEVERITY |
                            EnumConditionChange.MESSAGE |
                            EnumConditionChange.SUBCONDITION |
                            EnumConditionChange.ATTRIBUTE);
                        condition2.StateChange = EnumConditionState.ACTIVE;
                        conditionList.Add(condition2);
                        break;
                    case 50:
                        eventAttributes.Add(DateTime.Now.Second);
                        condition1.Severity = 1;
                        message = "not between [10,50]";
                        condition1.Message = message;
                        condition1.AckRequired = false;
                        condition1.EventAttributes = eventAttributes;
                        condition1.ChangeMask = (Byte)(EnumConditionChange.ACTIVE_STATE |
                            EnumConditionChange.SEVERITY |
                            EnumConditionChange.MESSAGE |
                            EnumConditionChange.ATTRIBUTE);
                        condition1.StateChange = EnumConditionState.NO;
                        conditionList.Add(condition1);

                        condition2.Severity = 1;
                        message = "not between [10,50]";
                        condition2.Message = message;
                        condition2.AckRequired = false;
                        condition2.EventAttributes = eventAttributes;
                        condition2.ChangeMask = (Byte)(EnumConditionChange.ACTIVE_STATE |
                            EnumConditionChange.SEVERITY |
                            EnumConditionChange.MESSAGE |
                            EnumConditionChange.SUBCONDITION |
                            EnumConditionChange.ATTRIBUTE);
                        condition2.StateChange = EnumConditionState.NO;
                        conditionList.Add(condition2);
                        break;
                    default:
                        break;
                }   //  end switch
                if (conditionList.Count > 0)
                {
                    Application.Instance.ChangeConditions(conditionList);
                }   //  end if

                //  simple events
                if (m_second != DateTime.Now.Second)
                {
                    AeEvent secondEvent = new AeEvent(EnumEventType.SIMPLE, "time.local", Console.CATEGORY_ID_TIME_TICK);
                    ArrayList attributeValues = new ArrayList();
                    attributeValues.Add(DateTime.Now.Hour);
                    attributeValues.Add(DateTime.Now.Minute);
                    attributeValues.Add(DateTime.Now.Second);
                    m_second = (Byte)DateTime.Now.Second;
                    secondEvent.Severity = 50;
                    secondEvent.ActorId = "opcUser";
                    message = "Second Changed";
                    secondEvent.Message = message;
                    secondEvent.AttributeValueList = attributeValues;
                    secondEvent.Fire();
                }
                if (m_minute != DateTime.Now.Minute)
                {
                    AeEvent minuteEvent = new AeEvent(EnumEventType.SIMPLE, "time.local", Console.CATEGORY_ID_TIME_TICK);
                    ArrayList attributeValues = new ArrayList(3);
                    attributeValues.Add(DateTime.Now.Hour);
                    attributeValues.Add(DateTime.Now.Minute);
                    attributeValues.Add(DateTime.Now.Second);
                    m_minute = (Byte)DateTime.Now.Minute;
                    minuteEvent.Severity = 900;
                    minuteEvent.ActorId = "opcUser";
                    message = "Minute Changed";
                    minuteEvent.Message = message;
                    minuteEvent.AttributeValueList = attributeValues;
                    minuteEvent.Fire();
                }
            }
            if (System.Console.KeyAvailable)
            {
                ArrayList attributeValues = new ArrayList(1);
                ConsoleKeyInfo keyInfo = System.Console.ReadKey();
                attributeValues.Add((Byte)keyInfo.KeyChar);
                char charPressed = keyInfo.KeyChar;
                AeEvent keyEvent = new AeEvent(EnumEventType.TRACKING, "keyboard.key", Console.CATEGORY_ID_KEY_HIT);
                keyEvent.Severity = 600;
                message = "key hit : ";
                message += charPressed.ToString();
                keyEvent.Message = message;
                keyEvent.AttributeValueList = attributeValues;
                keyEvent.Fire();
                //writing to keyboard.key DA Variable
                ValueQT value = new ValueQT((Byte)keyInfo.KeyChar, EnumQuality.GOOD, DateTime.Now);
                if (m_keyElement != null)
                {
                    m_keyElement.ValueChanged(value);
                }
            }
		}	//	end FireEvents
		public void ChangeSimulationValues()
		{
            foreach (DemoDaAddressSpaceElement demoElement in m_elemetList)
            {
                demoElement.Simulation();
            }
		}	//	end ChangeSimulationValues
		public void Trace(
			EnumTraceLevel traceLevel,
			EnumTraceGroup traceGroup,
			string objectID,
			string message)
		{
			Application.Instance.Trace(
				traceLevel,
				traceGroup,
				objectID,
				message);
		}	//	end Trace


		//--
		#endregion

	}	//	end OpcServer

}	//	end namespace
