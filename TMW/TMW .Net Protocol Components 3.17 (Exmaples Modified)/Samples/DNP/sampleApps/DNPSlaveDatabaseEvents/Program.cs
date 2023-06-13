using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using TMW.SCL;
using TMW.SCL.DNP;
using TMW.SCL.DNP.Slave;
using TMW.SCL.ProtocolAnalyzer;

namespace DNPDatabaseEvents
{
  class Program
  {
    static private bool bSerialMode = false;
    static private bool bProtocolMode = false;

    static private SDNPDatabase sdb;
    static private SDNPSession slaveSesn;
    static private DNPChannel slaveChan;
    
    // Since this device attribute is writable, create an object to hold the value.
    static private DNPDataDeviceAttrValue DevAttr246 = new DNPDataDeviceAttrValue(DNP_DEVICE_ATTR_TYPE.VSTR, 1, "Device 100");

    // For displaying Protocol Analyzer data
    static ProtocolBuffer protocolBuffer;
    static System.Threading.Timer protocolTimer;

    static void Main(string[] args)
    {
      Console.WriteLine("This program will start a slave (outstation) and wait for a connection.");
      Console.WriteLine("The sample source code shows an example of replacing the built-in database.\n");
      Console.WriteLine("\nPress 'Y' to communicate on Serial port COM3(for slave).");
      Console.WriteLine("Press any other key to use TCP");
      ConsoleKeyInfo key = Console.ReadKey();
      if (key.Key == ConsoleKey.Y)
      {
        bSerialMode = true;
      }

      Console.WriteLine("\nPress 'Y' to show Protocol Analyzer data");
      Console.WriteLine("Press any other key to continue");

      key = Console.ReadKey();
      if (key.Key == ConsoleKey.Y)
      {
        bProtocolMode = true;
      }

      Console.WriteLine("");
      Console.WriteLine("\nPress any key to exit");
      Console.WriteLine("");
      Thread.Sleep(3000);

      try
      {
        if (bProtocolMode)
        {
          // This creates the application with internal protocol logging enabled.
          // The internal Protocol Timer is a Windows.Forms.Timer 
          // Create our own timer to drive the output since this is not a Forms application
          new TMWApplicationBuilder();
          protocolTimer = new System.Threading.Timer(OnUpdateProtocolBufferTimer, null, 500, 500);
          protocolBuffer = TMWApplicationBuilder.getProtocolBuffer();
          protocolBuffer.ProtocolDataReadyEvent += OnNewProtocolData;
        }
        else
        {
          // This creates the application without protocol logging.
          new TMWApplicationBuilder(false);
        }

        TMWApplication pAppl = TMWApplicationBuilder.getAppl();
        pAppl.EnableEventProcessor = true;
        pAppl.InitEventProcessor();
         
        OpenSlave();

        Thread.Sleep(1000);

        // This is an example of how to add an analog input event for Anlog Input point 0
        TMWTime time = slaveSesn.GetTimeStamp();
        slaveSesn.AnlgInAddEvent(0, 10, DNPDatabaseFlags.ON_LINE, time);

      }
      catch (Exception e)
      {
        Console.WriteLine(e.ToString());
      }
      Console.ReadKey();
    }

    /// <summary>
    /// Called by the SCL when database is first opened
    /// <param name="db">database object that called the event</param>
    static void SlaveInitEvent(SDNPDatabase db)
    {
      // Set database Tag to something meaningful, like a pointer or reference
      // It will be available to all database Event routines. 
      db.Tag = 0x5555;

      // Perform other initialization processing here.
    }

    /// <summary>
    /// Called by SCL to get the number of points
    /// </summary>
    /// <param name="db">database object that called the event</param>
    /// <returns>number of points</returns>
    static ushort SlaveBinInQuantityEvent(SDNPDatabase db)
    {
      Console.WriteLine("Slave:BinInQuantity: return 2 points");
      // db.Tag contains whatever has been put in it at when session was opened

      return 2;
    }

    /// <summary>
    /// Called by SCL to get a handle for the given point number
    /// </summary>
    /// <param name="db">database that called this event</param>
    /// <param name="pointNum">point number</param>
    /// <returns>handle that identifies point</returns>
    static int SlaveBinInGetPointEvent(SDNPDatabase db, ushort pointNum)
    {
      int handle = 0;
      if (pointNum < 2)
      {
        handle = pointNum + 100;
      }
      Console.WriteLine("Slave:BinInGetPoint: Return {0} as handle for Point Number {1}", handle, pointNum);
      return handle;
    }

    /// <summary>
    /// Called by SCL to get value
    /// </summary>
    /// <param name="pointId">handle of the point to access</param>
    /// <param name="value">value to be returned</param>
    static void SlaveBinInReadEvent(int pointId, ref Boolean value, DNPDatabaseFlags flags)
    {
      if (pointId == 100)
      {
        Console.WriteLine("Slave:BinInRead: Point ID: {0} Set value = true", pointId);
        value = true;
        flags.Value = DNPDatabaseFlags.ON_LINE;
      }
      else if (pointId == 101)
      {
        Console.WriteLine("Slave:BinInRead: Point ID: {0} Set value = false", pointId);
        value = false;
        flags.Value = DNPDatabaseFlags.OFF_LINE;
      }
      else
        throw new Exception("Unknown point id");
    }

    /// <summary>
    /// Called by SCL to determine what event class this point is in
    /// </summary>
    /// <param name="pointId">handle of the point to access</param> 
    static TMW_CLASS_MASK SlaveBinInEventClassEvent(int pointId)
    {
      return TMW_CLASS_MASK.ONE;
    }

    /// <summary>
    /// Called by SCL to get the number of points
    /// </summary>
    /// <param name="db">database object that called the event</param>
    /// <returns>number of points</returns>
    static UInt16 SlaveBinOutQuantityEvent(SDNPDatabase db)
    {
      Console.WriteLine("Slave:BinOutQuantity: Set Bin Out to 2 points");
      return 2;
    }

    /// <summary>
    /// Called by SCL to get a handle for the given point number
    /// </summary>
    /// <param name="db">database that called this event</param>
    /// <param name="pointNum">point number</param>
    /// <returns>handle that identifies point</returns>
    static int SlaveBinOutGetPointEvent(SDNPDatabase db, ushort pointNum)
    {
      int handle = pointNum + 300;
      Console.WriteLine("Slave:BinOutGetPoint: Return {0} as handle for Point Number {1}", handle, pointNum);
      return handle;
    }

    /// <summary>
    /// Called by SCL to get value
    /// </summary>
    /// <param name="pointId">handle of the point to access</param>
    /// <param name="value">value to be returned</param>
    static void SlaveBinOutReadEvent(int pointId, ref Boolean value, DNPDatabaseFlags flags)
    {
      if (pointId == 300)
      {
        Console.WriteLine("Slave:BinOutRead: Point ID: {0} Set value = true", pointId);
        value = true;
        flags.Value = DNPDatabaseFlags.ON_LINE;
      }
      else if (pointId == 301)
      {
        Console.WriteLine("Slave:BinOutRead: Point ID: {0} Set value = false", pointId);
        value = false;
        flags.Value = DNPDatabaseFlags.OFF_LINE;
      }
      else
        throw new Exception("Unknown point id");
    }

    /// <summary>
    /// Called by SCL to perform select
    /// </summary>
    /// <param name="pointId">handle of the point to control, returned by SlaveBinOutGetPointEvent</param>
    /// <param name="controlCode">control code of type DNP_CROB_CTRL. Some values may be or'ed together.</param> 
    /// <param name="count">the number of times that the control operation should be
    ///   performed in succession. If count is 0, do not execute the control</param> 
    /// <param name="onTime">amount of time (in ms) the digital output is to be turned on 
    ///   (may not apply to all control types)</param> 
    /// <param name="offTime">amount of time (in ms) the digital output is to be turned off
    ///   (may not apply to all control types)</param> 
    static DNP_CROB_ST SDNPDatabase_BinOutSelectEvent(Int32 pointId, Byte controlCode,
      Byte count, UInt32 onTime, UInt32 offTime)
    {
      Console.WriteLine("Slave:BinOutSelect: Point Number {0}", pointId);
      if ((controlCode & (Byte)DNP_CROB_CTRL.PULSE_ON)  !=0 )
      {
        Console.WriteLine("Slave:BinOutSelect: Pulse ON");
      }
      else if ((controlCode &= (Byte)DNP_CROB_CTRL.QUEUE) != 0)
      {
        return DNP_CROB_ST.NOT_SUPPORTED;
      }
      else
      {
      }
      {
        Console.WriteLine("Slave:BinOutSelect: Other control");
      }

      return DNP_CROB_ST.SUCCESS;
    }

    // <summary>
    /// Called by SCL to perform operate
    /// </summary>
    /// <param name="pointId">handle of the point to control, returned by SlaveBinOutGetPointEvent</param>
    /// <param name="controlCode">control code</param> 
    /// <param name="count">the number of times that the control operation should be
    ///   performed in succession. If count is 0, do not execute the control</param> 
    /// <param name="onTime">amount of time (in ms) the digital output is to be turned on 
    ///   (may not apply to all control types)</param> 
    /// <param name="offTime">amount of time (in ms) the digital output is to be turned off
    ///   (may not apply to all control types)</param> 
    static DNP_CROB_ST SDNPDatabase_BinOutOperateEvent(Int32 pointId, Byte controlCode,
      Byte count, UInt32 onTime, UInt32 offTime)
    {
      Console.WriteLine("Slave:BinOutOperate: Point Number {0}", pointId);
      if (controlCode == (Byte)DNP_CROB_CTRL.PULSE_ON)
      {
        Console.WriteLine("Slave:BinOutOperate: Pulse ON");
      }
      
      return DNP_CROB_ST.SUCCESS;
    }

    /// <summary>
    /// Called by SCL to get the number of points
    /// </summary>
    /// <param name="db">database object that called the event</param>
    /// <returns>number of points</returns>
    static UInt16 SlaveAnlgOutQuantityEvent(SDNPDatabase db)
    {
      Console.WriteLine("Slave:AnlgOutQuantity: return 2 points");
      return 2;
    }

    /// <summary>
    /// Called by SCL to get a handle for the given point number
    /// </summary>
    /// <param name="db">database that called this event</param>
    /// <param name="pointNum">point number</param>
    /// <returns>handle that identifies point</returns>
    static int SlaveAnlgOutGetPointEvent(SDNPDatabase db, ushort pointNum)
    {
      int handle = pointNum + 400;
      Console.WriteLine("Slave:AnlgOutGetPoint: Return {0} as handle for Point Number {1}", handle, pointNum);
      return handle;
    }

    /// <summary>
    /// Called by SCL to get value
    /// </summary>
    /// <param name="pointId">handle of the point to access, returned by SlaveAnlgOutGetPointEvent</param>
    /// <param name="value">value to be returned</param>
    /// <param name="Flags">flags</param>
    static void SlaveAnlgOutReadEvent(int pointId, TMWAnalogVal pValue, DNPDatabaseFlags flags)
    {
      if (pointId == 400)
      {
        Console.WriteLine("Slave:AnlgOutRead: Point ID: {0} Set value = 15", pointId);
        pValue.SetShortValue(15);
        flags.Value = DNPDatabaseFlags.ON_LINE;
      }
      else if (pointId == 401)
      {
        Console.WriteLine("Slave:AnlgOutRead: Point ID: {0} Set value = 16", pointId);
        pValue.SetShortValue(16);
        flags.Value = DNPDatabaseFlags.ON_LINE;
      }
      else
        throw new Exception("Unknown point id");
    }

    /// <summary>
    /// Called by SCL to perform select
    /// </summary>
    /// <param name="pointId">handle of the point to control, returned by SlaveAnlgOutGetPointEvent</param>
    /// <param name="value">value to be set</param> 
    static DNP_CROB_ST SDNPDatabase_AnlgOutSelectEvent(Int32 pointId, TMWAnalogVal value)
    {
      Console.WriteLine("Slave:AnlgOutSelect: Point Number {0}", pointId);
      return DNP_CROB_ST.SUCCESS;
    }

    /// <summary>
    /// Called by SCL to perform operate
    /// </summary>
    /// <param name="pointId">handle of the point to control, returned by SlaveAnlgOutGetPointEvent</param>
    /// <param name="value">value to be set</param> 
    static DNP_CROB_ST SDNPDatabase_AnlgOutOperateEvent(Int32 pointId, TMWAnalogVal value)
    {
      Console.WriteLine("Slave:AnlgOutOperate: Point Number {0}", pointId);
      return DNP_CROB_ST.SUCCESS;
    }

    /// <summary>
    /// Called by SCL to get the number of points
    /// </summary>
    /// <param name="db">database object that called the event</param>
    /// <returns>number of points</returns>
    static UInt16 SlaveAnlgInQuantityEvent(SDNPDatabase db)
    {
      Console.WriteLine("Slave:AnlgInQuantity: return 2 points");
      return 2;
    }

    /// <summary>
    /// Called by SCL to get a handle for the given point number
    /// </summary>
    /// <param name="db">database that called this event</param>
    /// <param name="pointNum">point number</param>
    /// <returns>handle that identifies point to other database event methods</returns>
    static int SlaveAnlgInGetPointEvent(SDNPDatabase db, ushort pointNum)
    {
      int handle = pointNum + 500;
      Console.WriteLine("Slave:AnlgInGetPoint: Return {0} for Point Number {1}", handle, pointNum);
      return handle;
    }

    /// <summary>
    /// Called by SCL to get value
    /// </summary>
    /// <param name="pointId">handle of the point to access</param>
    /// <param name="value">value to be returned</param>
    /// <param name="Flags">flags</param>
    static void SlaveAnlgInReadEvent(int pointId, TMWAnalogVal pValue, DNPDatabaseFlags flags)
    {
      if (pointId == 500)
      {
        Console.WriteLine("Slave:AnlgInRead: Point ID: {0} Set value = 25", pointId);
        pValue.SetShortValue(25);
        flags.Value = DNPDatabaseFlags.ON_LINE;
      }
      else if (pointId == 501)
      {
        Console.WriteLine("Slave:AnlgInRead: Point ID: {0} Set value = 26", pointId);
        pValue.SetShortValue(26);
        flags.Value = (Byte)(DNPDatabaseFlags.LOCAL_FORCED | DNPDatabaseFlags.ON_LINE);
      }
      else
        throw new Exception("Unknown point id");
    }

    /// <summary>
    /// Called by SCL to determine what event class this point is in
    /// </summary>
    /// <param name="pointId">handle of the point to access</param> 
    static TMW_CLASS_MASK SlaveAnlgInEventClassEvent(int pointId)
    {
      return TMW_CLASS_MASK.THREE;
    }


    /// <summary>
    /// Called by SCL to determine how many sets of device attributes exist
    /// </summary>
    /// <param name="db">database that called this event</param>
    static UInt16 SlaveDeviceAttrQuantityEvent(SDNPDatabase db)
    {
      // We are only supporting the single set of device attributes defined in the DNP spec
      // This would be point index 0. 
      return 1;
    }

    /// <summary>
    /// Called by SCL to get handle for the set of device attributes specified by pointNum
    /// </summary>
    /// <param name="db">database that called this event</param>
    static int SlaveDeviceAttrGetPointEvent(SDNPDatabase db, ushort pointNum)
    {
      // We are only supporting the set of device attributes defined in the DNP spec
      // Return something meaningful and non zero to be used in other methods
      if (pointNum == 0)
        return 0xffff;
      else
        return 0;
    }

    /// <summary>
    /// Called by SCL to get the variation of the next device attribute in the database and whether or not it is writable 
    /// </summary>
    /// <param name="pointId">handle of the set of device attributes to access</param> 
    /// <param name="variation">variation of device attribute</param> 
    /// <param name="nextVariation">returns variation of next device attribute in the database if there is one </param> 
    /// <param name="property">returns property for this attribute, read only or read/write</param> 
    /// <returns>true if nextVariation has been filled in, or false if there are no more device attributes</returns>
    static bool SDNPDatabase_DeviceAttrNextEvent(int pointId, byte variation, ref byte nextVariation, ref byte property)
    {
      if (pointId == 0xffff)
      {
        // This sample supports  
        // 213 number of outstation defined data set prototypes
        // 215 number of outstation defined data sets
        // 242 Device manufacturer’s software version
        // 243 Device manufacturer’s hardware version 
        // 246 User-assigned ID code/number 
        // 248 Device serial number 
        // 250 Device manufacturer’s product name and model
        // 252 Device manufacturer’s name
        if (variation == 0)
        {
          property = 0;
          nextVariation = 213;
        }
        else if (variation == 213)
        {
          property = 0;
          nextVariation = 215;
        }
        else if (variation == 215)
        {
          property = 0;
          nextVariation = 242;
        }
        else if (variation == 242)
        {
          property = 0;
          nextVariation = 243;
        }
        else if (variation == 243)
        { 
          property = 1; // the next device attribute is writable
          nextVariation = 246;
        }
        else if (variation == 246)
        {
          property = 0;
          nextVariation = 248;
        }
        else if (variation == 248)
        {
          property = 0;
          nextVariation = 250;
        }
        else if (variation == 250)
        {
          property = 0;
          nextVariation = 252;
        }
        else
          return false;

        return true;
      }
      return false;

    }

    /// <summary>
    /// Called by SCL to get a handle for the specified device attribute 
    /// </summary>
    /// <param name="pointId">handle of the SET of device attributes to access</param> 
    /// <param name="variation">variation of device attribute</param> 
    /// <returns>handle for this device attribute that will be passed to SDNPDatabase_DeviceAttrReadEvent and SDNPDatabase_DeviceAttrWriteEvent</returns>
    static int SDNPDatabase_DeviceAttrGetVarEvent(int pointId, byte variation)
    {
      if (pointId == 0xffff)
      {
        // return a nonzero value that will be meaningful to SDNPDatabase_DeviceAttrReadEvent and SDNPDatabase_DeviceAttrWriteEvent
        // In our case just return the variation. This could be a pointer, index or reference that is meaningful.
        return variation;
      }
      return 0;
    }

    /// <summary>
    /// Called by SCL to read the specified device attribute
    /// </summary>
    /// <param name="varId">handle of the specific device attributes to read, returned by SDNPDatabase_DeviceAttrGetVarEvent</param> 
    /// <param name="data">data to be filled in</param> 
    /// <returns>true if the read is successful</returns>
    static bool SDNPDatabase_DeviceAttrReadEvent(int varId, ref DNPDataDeviceAttrValue data)
    {
      // pointId indicates id returned from SDNPDatabase_DeviceAttrGetVarEvent 
      switch (varId)
      {
        case 213:
          data = new DNPDataDeviceAttrValue(DNP_DEVICE_ATTR_TYPE.UINT, 4, 0);
          break;
        case 215:
          // This method will convert "0" to an unsigned integer
          data = new DNPDataDeviceAttrValue(DNP_DEVICE_ATTR_TYPE.UINT, 4, "0");
          break;
        case 242:
          // Note: length doesn't really matter for type VSTR. Length of string will be used.
          data = new DNPDataDeviceAttrValue(DNP_DEVICE_ATTR_TYPE.VSTR, 0, "3.0009.000");
          break;
        case 243:
          data = new DNPDataDeviceAttrValue(DNP_DEVICE_ATTR_TYPE.VSTR, 0, "Hardware Revision 2.00");
          break;
        case 246:
          // Use the static one since it may have been written to.
          data = DevAttr246;
          break;
        case 248:
          data = new DNPDataDeviceAttrValue(DNP_DEVICE_ATTR_TYPE.VSTR, 0, "101020304050");
          break;
        case 250:
          data = new DNPDataDeviceAttrValue(DNP_DEVICE_ATTR_TYPE.VSTR, 0, "SDNP .NET Component");
          break;
        case 252:
          data = new DNPDataDeviceAttrValue(DNP_DEVICE_ATTR_TYPE.VSTR, 0, "Triangle MicroWorks Inc.");
          break;
        default:
          return false;
      }
      return true;
    }

    /// <summary>
    /// Called by SCL to write the specified device attribute
    /// </summary>
    /// <param name="varId">handle of the specific device attributes to write, returned by SDNPDatabase_DeviceAttrGetVarEvent</param> 
    /// <param name="data">data to store</param> 
    /// <returns>true if the write is successful</returns>
    static bool SDNPDatabase_DeviceAttrWriteEvent(int varId, DNPDataDeviceAttrValue data)
    {
      if (varId == 246)
      {
        DevAttr246 = data;
        return true;
      }
      // The other variations are not writable.
      return false;
    }


    private static void OpenSlave()
    {
      slaveChan = new DNPChannel(TMW_CHANNEL_OR_SESSION_TYPE.SLAVE);

      if (bSerialMode == true)
      {
        slaveChan.Type = WINIO_TYPE.RS232;
        slaveChan.Name = ".NET DNP Slave";  /* name displayed in analyzer window */
        slaveChan.Win232comPortName = "COM3";
        slaveChan.Win232baudRate = "9600";
        slaveChan.Win232numDataBits = RS232_DATA_BITS.BITS_8;
        slaveChan.Win232numStopBits = RS232_STOP_BITS.BITS_1;
        slaveChan.Win232portMode = RS232_PORT_MODE.NONE;
      }
      else
      {
        slaveChan.Type = WINIO_TYPE.TCP;
        slaveChan.Name = ".NET DNP Slave";  /* name displayed in analyzer window */
        slaveChan.WinTCPipAddress = "127.0.0.1";
        slaveChan.WinTCPipPort = 20000;
        slaveChan.WinTCPmode = TCP_MODE.SERVER;
      }
      slaveChan.OpenChannel();

      // Inputs and other database interface can be selected/implemented separately from Control Output database interface, 
      // file functionality interface and Secure Authentication interface.
      // You may want to use the default database for input value storage, but implement the output interface so that a .NET event is 
      // generated when a control is received from the master. The built-in File transfer database will read and write Windows files, so you
      // may also choose to use the default file transfer database instead of implementing your own.
      // For Secure Authentication, you can choose to use the built-in functionality instad of implementing your own.

      // .Net Event interface will be used for Inputs and other general database functionality.
      SDNPDatabase.UseSimDatabase = false;

      // .Net Event interface will be used for Control Output database functionality
      SDNPDatabase.UseSimControlDatabase = false;

      // .Net Event interface will be used for Device Attribute database functionality
      SDNPDatabase.UseSimDevAttrDatabase = false;

      // This is not supported 
      // SDNPDatabase.UseSimDatasetDatabase = false;

      //SDNPDatabase.UseSimFileDatabase = false;
      //SDNPDatabase.UseSimAuthDatabase = false; 

      SDNPDatabase.InitEvent += new SDNPDatabase.InitDelegate(SlaveInitEvent);

      slaveSesn = new SDNPSession(slaveChan);

      // If using TCP the DNP Spec requires keep alives to be configured in order to detect disconnects.
      if (!bSerialMode)
        slaveSesn.LinkStatusPeriod = 30000;

      slaveSesn.UnsolAllowed = true;
      slaveSesn.OpenSession();


      sdb = (SDNPDatabase)slaveSesn.SimDatabase;

      // Can set database Tag here, or in SlaveInitEvent to something meaningful
      // It will be available to all database Event routines.
      //sdb.Tag = 0x1111;

      // binary inputs, because UseSimDatabase was set to false
      sdb.BinInQuantityEvent += new SDNPDatabase.BinInQuantityDelegate(SlaveBinInQuantityEvent);
      sdb.BinInGetPointEvent += new SDNPDatabase.BinInGetPointDelegate(SlaveBinInGetPointEvent);
      SDNPDatabase.BinInReadEvent += new SDNPDatabase.BinInReadDelegate(SlaveBinInReadEvent);
      SDNPDatabase.BinInEventClassEvent += new SDNPDatabase.BinInEventClassDelegate(SlaveBinInEventClassEvent);
      // Register BinInAssignClassEvent to support Assign Class for Binary Input Points

      // analog inputs, because UseSimDatabase was set to false
      sdb.AnlgInQuantityEvent += new SDNPDatabase.AnlgInQuantityDelegate(SlaveAnlgInQuantityEvent);
      sdb.AnlgInGetPointEvent += new SDNPDatabase.AnlgInGetPointDelegate(SlaveAnlgInGetPointEvent);
      SDNPDatabase.AnlgInReadEvent += new SDNPDatabase.AnlgInReadDelegate(SlaveAnlgInReadEvent);
      SDNPDatabase.AnlgInEventClassEvent += new SDNPDatabase.AnlgInEventClassDelegate(SlaveAnlgInEventClassEvent);
      // Register AnlgInAssignClassEvent to support Assign Class for Analog Input Points

      // Also, register DblInxxxEvent, BinCntrxxxEvent, FrznCntrxxxEvent, StrxxxEvent, VtermxxxEvent if you want to support these objects.


      // binary outputs, because UseSimControlDatabase was set to false
      sdb.BinOutQuantityEvent += new SDNPDatabase.BinOutQuantityDelegate(SlaveBinOutQuantityEvent);
      sdb.BinOutGetPointEvent += new SDNPDatabase.BinOutGetPointDelegate(SlaveBinOutGetPointEvent);
      SDNPDatabase.BinOutReadEvent += new SDNPDatabase.BinOutReadDelegate(SlaveBinOutReadEvent);
      SDNPDatabase.BinOutSelectEvent += new SDNPDatabase.BinOutSelectDelegate(SDNPDatabase_BinOutSelectEvent);
      SDNPDatabase.BinOutOperateEvent += new SDNPDatabase.BinOutOperateDelegate(SDNPDatabase_BinOutOperateEvent);

      // analog outputs, because UseSimControlDatabase was set to false
      sdb.AnlgOutQuantityEvent += new SDNPDatabase.AnlgOutQuantityDelegate(SlaveAnlgOutQuantityEvent);
      sdb.AnlgOutGetPointEvent += new SDNPDatabase.AnlgOutGetPointDelegate(SlaveAnlgOutGetPointEvent);
      SDNPDatabase.AnlgOutReadEvent += new SDNPDatabase.AnlgOutReadDelegate(SlaveAnlgOutReadEvent);
      SDNPDatabase.AnlgOutSelectEvent += new SDNPDatabase.AnlgOutSelectDelegate(SDNPDatabase_AnlgOutSelectEvent);
      SDNPDatabase.AnlgOutOperateEvent += new SDNPDatabase.AnlgOutOperateDelegate(SDNPDatabase_AnlgOutOperateEvent);

      // Also, register ActivateConfigEvent, ColdRestartEvent and WarmRestartEvent
      // if you want to support those, because UseSimControlDatabase was set to false.


      // Device attributes, because UseSimDevAttrDatabase was set to false
      sdb.DeviceAttrGetPointEvent += new SDNPDatabase.DeviceAttrGetPointDelegate(SlaveDeviceAttrGetPointEvent);
      sdb.DeviceAttrQuantityEvent += new SDNPDatabase.DeviceAttrQuantityDelegate(SlaveDeviceAttrQuantityEvent);

      SDNPDatabase.DeviceAttrGetVarEvent += new SDNPDatabase.DeviceAttrGetVarDelegate(SDNPDatabase_DeviceAttrGetVarEvent);
      SDNPDatabase.DeviceAttrNextEvent += new SDNPDatabase.DeviceAttrNextDelegate(SDNPDatabase_DeviceAttrNextEvent);
      SDNPDatabase.DeviceAttrReadEvent += new SDNPDatabase.DeviceAttrReadDelegate(SDNPDatabase_DeviceAttrReadEvent);
      SDNPDatabase.DeviceAttrWriteEvent += new SDNPDatabase.DeviceAttrWriteDelegate(SDNPDatabase_DeviceAttrWriteEvent);
    }
     


    // Periodic Protocol Analyzer Buffer Timer
    static void OnUpdateProtocolBufferTimer(object obj)
    {
      protocolTimer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
      protocolBuffer.DoForceUpdate();
      protocolTimer.Change(500, 500);
    }

    // Write Protocol Analyzer data
    static void OnNewProtocolData(ProtocolBuffer buf)
    {
      buf.Lock();
      for (int i = buf.LastProvidedIndex; i < buf.LastAddedIndex; i++)
      {
        string text = protocolBuffer.getPdoAtIndex(i).ProtocolText;
        Console.Write(string.Format(">>>Protocol: {0}", text));
      }
      buf.UnLock();
    }
  }
}
