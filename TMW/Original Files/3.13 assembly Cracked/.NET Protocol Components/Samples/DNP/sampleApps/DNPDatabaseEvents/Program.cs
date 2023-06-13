using System;
using TMW.SCL;
using TMW.SCL.DNP;
using TMW.SCL.DNP.Master;
using TMW.SCL.DNP.Slave;
using TMW.SCL.ProtocolAnalyzer;


namespace DNPDatabaseEvents
{
  class Program
  {
    static private bool bSerialMode;
    static private bool bProtocolMode;

    static private MDNPDatabase mdb;
    static private MDNPSession masterSesn;
    static private DNPChannel masterChan;

    static private SDNPDatabase sdb;
    static private SDNPSession slaveSesn;
    static private DNPChannel slaveChan;

    static ProtocolBuffer protocolBuffer;
    static System.Threading.Timer protocolTimer;


    static void OnUpdateProtocolBufferTimer(object obj)
    {
      protocolTimer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
      protocolBuffer.DoForceUpdate();
      protocolTimer.Change(500, 500);
    }

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

    static void Main()
    {
      Console.WriteLine("This program will open both a master and a outstation.");
      Console.WriteLine("The master will send one Integrity Poll to the outstation.");
      Console.WriteLine("The sample source code shows an example of replacing the built-in database.\n");
      Console.WriteLine("Press 'Y' to communicate on Serial ports COM3(for slave) and COM4(for master), press any other key to continue");
      ConsoleKeyInfo key = Console.ReadKey();
      if (key.Key == ConsoleKey.Y)
      {
        bSerialMode = true;
      }

      Console.WriteLine("Press 'Y' to show protocol data");
      key = Console.ReadKey();
      if (key.Key == ConsoleKey.Y)
      {
        bProtocolMode = true;
      }

      Console.WriteLine("Press Enter to end test\n");

      try
      {
        new TMWApplicationBuilder();
        TMWApplication pAppl = TMWApplicationBuilder.getAppl();
        pAppl.InitEventProcessor();
        pAppl.EnableEventProcessor = true;

        if (bProtocolMode)
        {
          protocolTimer = new System.Threading.Timer(OnUpdateProtocolBufferTimer, null, 500, 500);
          protocolBuffer = TMWApplicationBuilder.getProtocolBuffer();
          protocolBuffer.ProtocolDataReadyEvent += OnNewProtocolData;
        }

        OpenSlave();
        OpenMaster();

        System.Threading.Thread.Sleep(1000);

        // Issue an integrity poll
        MDNPRequest request = new MDNPRequest(masterSesn);
        request.IntegrityPoll(true);
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
      int handle = pointNum + 100;
      Console.WriteLine("Slave:BinInGetPoint: Return {0} as handle for Point Number {1}", handle, pointNum);
      return handle;
    }

    /// <summary>
    /// Called by SCL to get value
    /// </summary>
    /// <param name="pointId">handle of the point to access</param>
    /// <param name="value">value to be returned</param>
    /// <param name="flags"></param>
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
    /// Called by the SCL when database is first opened
    /// <param name="db">database object that called the event</param>
    static void MasterInitEvent(MDNPDatabase db)
    {
      // Set database Tag to something meaningful, like a pointer or reference
      // It will be available to all database Event routines. 
      db.Tag = 0x5555;

      // Perform any database initialization here.
    }

    /// <summary>
    /// Called by the SCL to store a binary input
    /// </summary>
    /// <param name="db">database object that called the event</param>
    /// <param name="pointNumber">point number</param>
    /// <param name="value">value</param>
    /// <param name="flags">flags</param>
    /// <param name="isEvent">event</param>
    /// <param name="pTimeStamp">time stamp</param>
    /// <returns>success/failure</returns>
    static bool Master_StoreBinaryInputEvent(MDNPDatabase db, ushort pointNumber, bool value, DNPDatabaseFlags flags, bool isEvent, TMWTime pTimeStamp)
    {
      // Set database Tag to something meaningful, like a pointer or reference
      Console.WriteLine("Master:StoreBinaryInput: Point Number {0} Value {1} Flags {2}", pointNumber, value, flags);

      // db.Tag contains whatever has been put in it at when session was opened

      return true;
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
    /// <param name="flags"></param>
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
    /// <param name="controlCode">control code</param> 
    /// <param name="count">the number of times that the control operation should be
    ///   performed in succession. If count is 0, do not execute the control</param> 
    /// <param name="onTime">amount of time (in ms) the digital output is to be turned on 
    ///   (may not apply to all control types)</param> 
    /// <param name="offTime">amount of time (in ms) the digital output is to be turned off
    ///   (may not apply to all control types)</param> 
    static DNP_CROB_ST SlaveBinOutSelectEvent(Int32 pointId, Byte controlCode,
      Byte count, UInt32 onTime, UInt32 offTime)
    {
      Console.WriteLine("Slave:BinOutSelect: Point ID {0}", pointId);
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
    static DNP_CROB_ST SlaveBinOutOperateEvent(Int32 pointId, Byte controlCode,
      Byte count, UInt32 onTime, UInt32 offTime)
    {
      Console.WriteLine("Slave:BinOutOperate: Point ID {0}", pointId);
      return DNP_CROB_ST.SUCCESS;
    }

    /// <summary>
    /// Called by the SCL to store a binary output
    /// </summary>
    /// <param name="db">database object that called the event</param>
    /// <param name="pointNumber">point number</param>
    /// <param name="value">value</param>
    /// <param name="flags">flags</param>
    /// <param name="isEvent">event</param>
    /// <param name="pTimeStamp">time stamp</param>
    /// <returns>success/failure</returns>
    static bool Master_StoreBinaryOutputEvent(MDNPDatabase db, ushort pointNumber, bool value, DNPDatabaseFlags flags, bool isEvent, TMWTime pTimeStamp)
    {
      Console.WriteLine("Master:StoreBinaryOutput: Point Number {0} Value {1} Flags {2}", pointNumber, value, flags);
      return true;
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
    /// <param name="pointId">handle of the point to access</param>
    /// <param name="value">value to be returned</param>
    /// <param name="flags">flags</param>
    static void SlaveAnlgOutReadEvent(int pointId, TMWAnalogVal value, DNPDatabaseFlags flags)
    {
      if (pointId == 400)
      {
        Console.WriteLine("Slave:AnlgOutRead: Point ID: {0} Set value = 15", pointId);
        value.SetShortValue(15);
        flags.Value = DNPDatabaseFlags.ON_LINE;
      }
      else if (pointId == 401)
      {
        Console.WriteLine("Slave:AnlgOutRead: Point ID: {0} Set value = 16", pointId);
        value.SetShortValue(16);
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
    static DNP_CROB_ST SlaveAnlgOutSelectEvent(Int32 pointId, TMWAnalogVal value)
    {
      Console.WriteLine("Slave:AnlgOutSelect: Point ID {0}", pointId);
      return DNP_CROB_ST.SUCCESS;
    }

    /// <summary>
    /// Called by SCL to perform operate
    /// </summary>
    /// <param name="pointId">handle of the point to control, returned by SlaveAnlgOutGetPointEvent</param>
    /// <param name="value">value to be set</param> 
    static DNP_CROB_ST SlaveAnlgOutOperateEvent(Int32 pointId, TMWAnalogVal value)
    {
      Console.WriteLine("Slave:AnlgOutOperate: Point ID {0}", pointId);
      return DNP_CROB_ST.SUCCESS;
    } 

    /// <summary>
    /// Called by the SCL to store a analog output
    /// </summary>
    /// <param name="db">database object that called the event</param>
    /// <param name="pointNumber">point number</param>
    /// <param name="value">value</param>
    /// <param name="flags">flags</param>
    /// <param name="isEvent">event</param>
    /// <param name="pTimeStamp">time stamp</param>
    /// <returns>success/failure</returns>
    static bool Master_StoreAnalogOutputEvent(MDNPDatabase db, ushort pointNumber, TMWAnalogVal value, DNPDatabaseFlags flags, bool isEvent, TMWTime pTimeStamp)
    {
      Console.WriteLine("Master:StoreAnalogOutput: Point Number {0} Value {1} Flags {2}", pointNumber, value, flags);
      return true;
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
    /// <returns>handle that identifies point</returns>
    static int SlaveAnlgInGetPointEvent(SDNPDatabase db, ushort pointNum)
    {
      int handle = pointNum + 500;
      Console.WriteLine("Slave:AnlgInGetPoint: Return {0} as handle for Point Number {1}", handle, pointNum);
      return handle;
    }

    /// <summary>
    /// Called by SCL to get value
    /// </summary>
    /// <param name="pointId">handle of the point to access</param>
    /// <param name="value">value to be returned</param>
    /// <param name="flags">flags</param>
    static void SlaveAnlgInReadEvent(int pointId, TMWAnalogVal value, DNPDatabaseFlags flags)
    {
      if (pointId == 500)
      {
        Console.WriteLine("Slave:AnlgInRead: Point ID: {0} Set value = 25", pointId);
        value.SetShortValue(25);
        flags.Value = DNPDatabaseFlags.ON_LINE;
      }
      else if (pointId == 501)
      {
        Console.WriteLine("Slave:AnlgInRead: Point ID: {0} Set value = 26", pointId);
        value.SetShortValue(26);
        flags.Value = (Byte)(DNPDatabaseFlags.LOCAL_FORCED | DNPDatabaseFlags.ON_LINE);
      }
      else
        throw new Exception("Unknown point id");
    }

    /// <summary>
    /// Called by the SCL to store a analog output
    /// </summary>
    /// <param name="db">database object that called the event</param>
    /// <param name="pointNumber">point number</param>
    /// <param name="value">value</param>
    /// <param name="flags">flags</param>
    /// <param name="isEvent">event</param>
    /// <param name="pTimeStamp">time stamp</param>
    /// <returns>success/failure</returns>
    static bool Master_StoreAnalogInputEvent(MDNPDatabase db, ushort pointNumber, TMWAnalogVal value, DNPDatabaseFlags flags, bool isEvent, TMWTime pTimeStamp)
    {
      Console.WriteLine("Master:StoreAnalogInput: Point Number {0} Value {1} Flags {2}", pointNumber, value, flags);
      return true;
    }


    private static void OpenSlave()
    {
      slaveChan = new DNPChannel(TMW_CHANNEL_OR_SESSION_TYPE.SLAVE);

      if (bSerialMode)
      {
        slaveChan.Type = WINIO_TYPE.RS232;
        slaveChan.Name = ".NET DNP Slave";  /* name displayed in analyzer window */
        slaveChan.Win232comPortName = "COM3";
        slaveChan.Win232baudRate = "9600";
        slaveChan.Win232numDataBits = RS232_DATA_BITS.BITS_8;
        slaveChan.Win232numStopBits = RS232_STOP_BITS.BITS_1;
        slaveChan.Win232portMode = RS232_PORT_MODE.NONE;
        //        slaveChan.Win232bSyncMode = true;
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

      slaveSesn = new SDNPSession(slaveChan);
      slaveSesn.UnsolAllowed = true;

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

      //SDNPDatabase.UseSimFileDatabase = false;
      //SDNPDatabase.UseSimAuthDatabase = false; 

      SDNPDatabase.InitEvent += SlaveInitEvent;

      slaveSesn.OpenSession();

      sdb = (SDNPDatabase)slaveSesn.SimDatabase;

      // Can set database Tag here, or in SlaveInitEvent to something meaningful
      // It will be available to all database Event routines.
      //sdb.Tag = 0x1111;

      // binary inputs, because UseSimDatabase was set to false
      sdb.BinInQuantityEvent += new SDNPDatabase.BinInQuantityDelegate(SlaveBinInQuantityEvent);
      sdb.BinInGetPointEvent += new SDNPDatabase.BinInGetPointDelegate(SlaveBinInGetPointEvent);
      SDNPDatabase.BinInReadEvent += new SDNPDatabase.BinInReadDelegate(SlaveBinInReadEvent);

      // analog inputs, because UseSimDatabase was set to false
      sdb.AnlgInQuantityEvent += new SDNPDatabase.AnlgInQuantityDelegate(SlaveAnlgInQuantityEvent);
      sdb.AnlgInGetPointEvent += new SDNPDatabase.AnlgInGetPointDelegate(SlaveAnlgInGetPointEvent);
      SDNPDatabase.AnlgInReadEvent += new SDNPDatabase.AnlgInReadDelegate(SlaveAnlgInReadEvent);

      // Also, register DblInxxxEvent, BinCntrxxxEvent, FrznCntrxxxEvent, StrxxxEvent, VtermxxxEvent if you want to support these objects.

      // binary outputs, because UseSimControlDatabase was set to false
      sdb.BinOutQuantityEvent += new SDNPDatabase.BinOutQuantityDelegate(SlaveBinOutQuantityEvent);
      sdb.BinOutGetPointEvent += new SDNPDatabase.BinOutGetPointDelegate(SlaveBinOutGetPointEvent);
      SDNPDatabase.BinOutReadEvent += new SDNPDatabase.BinOutReadDelegate(SlaveBinOutReadEvent);
      SDNPDatabase.BinOutSelectEvent += new SDNPDatabase.BinOutSelectDelegate(SlaveBinOutSelectEvent);
      SDNPDatabase.BinOutOperateEvent += new SDNPDatabase.BinOutOperateDelegate(SlaveBinOutOperateEvent);

      // analog outputs, because UseSimControlDatabase was set to false
      sdb.AnlgOutQuantityEvent += new SDNPDatabase.AnlgOutQuantityDelegate(SlaveAnlgOutQuantityEvent);
      sdb.AnlgOutGetPointEvent += new SDNPDatabase.AnlgOutGetPointDelegate(SlaveAnlgOutGetPointEvent);
      SDNPDatabase.AnlgOutReadEvent += new SDNPDatabase.AnlgOutReadDelegate(SlaveAnlgOutReadEvent);
      SDNPDatabase.AnlgOutSelectEvent += new SDNPDatabase.AnlgOutSelectDelegate(SlaveAnlgOutSelectEvent);
      SDNPDatabase.AnlgOutOperateEvent += new SDNPDatabase.AnlgOutOperateDelegate(SlaveAnlgOutOperateEvent);

      // Also, register ActivateConfigEvent, ColdRestartEvent and WarmRestartEvent
      // if you want to support those, because UseSimControlDatabase was set to false.
    }

    private static void OpenMaster()
    {
      masterChan = new DNPChannel(TMW_CHANNEL_OR_SESSION_TYPE.MASTER);

      if (bSerialMode)
      {
        masterChan.Type = WINIO_TYPE.RS232;
        masterChan.Name = ".NET DNP Master";  /* name displayed in analyzer window */
        masterChan.Win232comPortName = "COM4";
        masterChan.Win232baudRate = "9600";
        masterChan.Win232numDataBits = RS232_DATA_BITS.BITS_8;
        masterChan.Win232numStopBits = RS232_STOP_BITS.BITS_1;
        masterChan.Win232portMode = RS232_PORT_MODE.NONE;
        //        masterChan.Win232bSyncMode = true;
      }
      else
      {
        masterChan.Type = WINIO_TYPE.TCP;
        masterChan.Name = ".NET DNP Master";  /* name displayed in analyzer window */
        masterChan.WinTCPipAddress = "127.0.0.1";
        masterChan.WinTCPipPort = 20000;
        masterChan.WinTCPmode = TCP_MODE.CLIENT;
      }
      masterChan.OpenChannel();

      masterSesn = new MDNPSession(masterChan);
      masterSesn.AutoClassPollIIN = true;
      masterSesn.AutoEnableUnsolClass1 = true;
      masterSesn.AutoEnableUnsolClass2 = true;
      masterSesn.AutoEnableUnsolClass3 = true;

      MDNPDatabase.UseSimDatabase = false;
      MDNPDatabase.InitEvent += MasterInitEvent;

      masterSesn.OpenSession();

      mdb = (MDNPDatabase)masterSesn.SimDatabase;

      // Can set database Tag here, or in MasterInitEvent to something meaningful
      // It will be available to all database Event routines.
      mdb.Tag = 0x5555;

      // register events on Master to store data
      mdb.StoreBinaryInputEvent += Master_StoreBinaryInputEvent;
      mdb.StoreBinaryOutputEvent += Master_StoreBinaryOutputEvent;
      mdb.StoreAnalogInputEvent += Master_StoreAnalogInputEvent;
      mdb.StoreAnalogOutputEvent += Master_StoreAnalogOutputEvent;

    }
  }
}
