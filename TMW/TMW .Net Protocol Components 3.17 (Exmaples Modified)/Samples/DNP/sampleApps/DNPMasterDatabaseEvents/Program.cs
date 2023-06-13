using System;
using System.Threading;


using TMW.SCL;
using TMW.SCL.DNP;
using TMW.SCL.DNP.Master;
using TMW.SCL.ProtocolAnalyzer;


namespace DNPMasterDatabaseEvents
{
  class Program
  {
    static private bool bSerialMode = false;
    static private bool bProtocolMode = false;

    static private MDNPDatabase mdb;
    static private MDNPSession masterSesn;
    static private DNPChannel masterChan;

    // For displaying Protocol Analyzer data
    static ProtocolBuffer protocolBuffer;
    static System.Threading.Timer protocolTimer;

    static void Main()
    {
      Console.WriteLine("\nThis program will start a master and try to connect");
      Console.WriteLine("to an external outstation (DNPSlaveDatabaseEvents.exe).");
      Console.WriteLine("It will loop 5 times sending one Integrity poll to the outstation.\n");
      Console.WriteLine("The sample source code shows an example of replacing the built-in database.\n");

      Console.WriteLine("\nPress 'Y' to communicate on Serial port COM4(for master).");
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

        // This causes application to process received data and timers.
        TMWApplication pAppl = TMWApplicationBuilder.getAppl();
        pAppl.EnableEventProcessor = true;
        pAppl.InitEventProcessor();


        masterChan = new DNPChannel(TMW_CHANNEL_OR_SESSION_TYPE.MASTER);
        masterSesn = new MDNPSession(masterChan);

        for (int i = 0; i < 5; i++)
        {

          Console.WriteLine("\nOpen Master\n");
          OpenMaster();

          Thread.Sleep(500);

          // Issue an integrity poll
          MDNPRequest request = new MDNPRequest(masterSesn);
          request.IntegrityPoll(true);

          // Read All Device Attributes
          request = new MDNPRequest(masterSesn);
          request.ReadGroup(0, 254, MDNPRequest.DNP_QUALIFIER.Q_ALL_POINTS, 0, 0);

          Thread.Sleep(5000);

          Console.WriteLine("\nClose Master\n");
          CloseMaster();

          Thread.Sleep(5000);
        }
      }
      catch (Exception e)
      {
        Console.WriteLine(e.ToString());
      }
      Console.WriteLine("Done: Press any key to exit");
      Console.ReadKey();
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
      DateTime dt = (DateTime)pTimeStamp.ToDateTime();
      Console.WriteLine("Master:StoreBinaryInput: Point Number {0} Value {1} Flags {2} Date {3}", pointNumber, value, flags, dt);

      // db.Tag contains whatever has been put in it at when session was opened

      return true;
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

    static void Master_StoreDeviceAttrPropertyEvent(MDNPDatabase db, ushort pointNumber, byte variation, byte property)
    {
      Console.WriteLine("Master:StoreDeviceAttrProperty: Point Number {0} variation {1} property {2}", pointNumber, variation, property);
    }

    static void Master_StoreDeviceAttributeEvent(MDNPDatabase db, ushort pointNumber, byte variation, DNPDataDeviceAttrValue data)
    {
      Console.WriteLine("Master:StoreDeviceAttribute: Point Number {0} variation {1} dataType {2} value {3}", pointNumber, variation, data.DataType, data.ToString());
    }

    private static void CloseMaster()
    {
      MDNPDatabase.InitEvent -= new MDNPDatabase.InitDelegate(MasterInitEvent);
      mdb.StoreBinaryInputEvent -= new MDNPDatabase.StoreBinaryInputDelegate(Master_StoreBinaryInputEvent);
      mdb.StoreBinaryOutputEvent -= new MDNPDatabase.StoreBinaryOutputDelegate(Master_StoreBinaryOutputEvent);
      mdb.StoreAnalogInputEvent -= new MDNPDatabase.StoreAnalogInputDelegate(Master_StoreAnalogInputEvent);
      mdb.StoreAnalogOutputEvent -= new MDNPDatabase.StoreAnalogOutputDelegate(Master_StoreAnalogOutputEvent);

      masterSesn.CloseSession();
      masterChan.CloseChannel();
    }

    private static void OpenMaster()
    {

      if (bSerialMode)
      {
        masterChan.Type = WINIO_TYPE.RS232;
        masterChan.Name = ".NET DNP Master";  /* name displayed in analyzer window */
        masterChan.Win232comPortName = "COM4";
        masterChan.Win232baudRate = "9600";
        masterChan.Win232numDataBits = RS232_DATA_BITS.BITS_8;
        masterChan.Win232numStopBits = RS232_STOP_BITS.BITS_1;
        masterChan.Win232portMode = RS232_PORT_MODE.NONE;
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

      masterSesn.AutoClassPollIIN = true;
      masterSesn.AutoEnableUnsolClass1 = true;
      masterSesn.AutoEnableUnsolClass2 = true;
      masterSesn.AutoEnableUnsolClass3 = true;

      MDNPDatabase.UseSimDatabase = false;
      MDNPDatabase.UseSimDevAttrDatabase = false;
      MDNPDatabase.InitEvent += new MDNPDatabase.InitDelegate(MasterInitEvent);

      masterSesn.OpenSession();

      // If using TCP the DNP Spec requires keep alives to be configured in order to detect disconnects.
      if (!bSerialMode)
        masterSesn.LinkStatusPeriod = 30000;

      mdb = (MDNPDatabase)masterSesn.SimDatabase;

      // Can set database Tag here or in MasterInitEvent to something meaningful
      // It will then be available to all database Event routines.
      mdb.Tag = 0x5555;

      // register events on Master to store data, because UseSimDatabase was set to false
      mdb.StoreBinaryInputEvent += new MDNPDatabase.StoreBinaryInputDelegate(Master_StoreBinaryInputEvent);
      mdb.StoreBinaryOutputEvent += new MDNPDatabase.StoreBinaryOutputDelegate(Master_StoreBinaryOutputEvent);
      mdb.StoreAnalogInputEvent += new MDNPDatabase.StoreAnalogInputDelegate(Master_StoreAnalogInputEvent);
      mdb.StoreAnalogOutputEvent += new MDNPDatabase.StoreAnalogOutputDelegate(Master_StoreAnalogOutputEvent);

      // Register device attributes, because UseSimDevAttrDatabase was set to false
      mdb.StoreDeviceAttributeEvent += new MDNPDatabase.StoreDeviceAttributeDelegate(Master_StoreDeviceAttributeEvent); 
      mdb.StoreDeviceAttrPropertyEvent += new MDNPDatabase.StoreDeviceAttrPropertyDelegate(Master_StoreDeviceAttrPropertyEvent);
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