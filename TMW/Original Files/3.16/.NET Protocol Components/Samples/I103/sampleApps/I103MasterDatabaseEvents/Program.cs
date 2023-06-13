using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using TMW.SCL;
using TMW.SCL.ProtocolAnalyzer;
using TMW.SCL.IEC60870_5;
using TMW.SCL.IEC60870_5.I103;
using TMW.SCL.IEC60870_5.I103.Master;


namespace I103MasterDatabaseEvents
{
  class Program
  {
    static private M103Session masterSesn103;
    static private M103Sector masterSctr103;
    static private FT12Channel masterChan103;
    static private M103Database masterDB;

    // For displaying Protocol Analyzer data
    static private bool bProtocolMode = false;
    static ProtocolBuffer protocolBuffer;
    static System.Threading.Timer protocolTimer;

    static void Main(string[] args)
    {
      Console.WriteLine("\nThis program will start a master and try to connect to a separate slave.");
      Console.WriteLine("It will send a General Interrogation, General Command, and Generic Read Headings command to the slave.");
      Console.WriteLine("The sample source code shows an example of replacing the built-in database.\n");

      Console.WriteLine("\nPress 'Y' to show Protocol Analyzer data");
      Console.WriteLine("Press any other key to continue");

      ConsoleKeyInfo key = Console.ReadKey();
      if (key.Key == ConsoleKey.Y)
      {
        bProtocolMode = true;
      }

      Console.WriteLine("\nPress any key to end test\n");
      Thread.Sleep(1000);

      try
      {
        TMWApplicationBuilder applBuilder = new TMWApplicationBuilder();
        TMWApplication pAppl = TMWApplicationBuilder.getAppl();
        pAppl.EnableEventProcessor = true;
        pAppl.InitEventProcessor();
        
        if (bProtocolMode)
        {
          // Internal Protocol Timer is a Windows.Forms.Timer 
          // Create our own timer since this is not a Forms application
          protocolTimer = new System.Threading.Timer(OnUpdateProtocolBufferTimer, null, 500, 500);
          protocolBuffer = TMWApplicationBuilder.getProtocolBuffer();
          protocolBuffer.ProtocolDataReadyEvent += OnNewProtocolData;
        }

        OpenMaster();

        //Thread.Sleep(30000);

        // register for the double point store event on master
        masterDB.DpiStoreEvent += new M103Database.DpiStoreDelegate(masterDB_DpiStoreEvent);


        // make a General Interrogation request
        M103Request req = new M103Request(masterSctr103);
        req.gi();

        // Send a general command
        req.gnrl(128, 16, Defs.DIQ_ON);

        // Send a generic data read headings request
        req.gnrcReadHeadings();

      }
      catch (Exception e)
      {
        Console.WriteLine(e.ToString());
      }
      // wait for key input to finish
      Console.ReadKey();
    }

    static void MasterInitEvent(M103Database db)
    {
      // Set database Tag to something meaningful, like a pointer or reference
      // It will be available to all database Event routines. 
      db.Tag = 0x1111;

      // Perform any database initialization here.
    }

    static bool masterDB_DpiStoreEvent(M103Database db, byte functionType, byte informationNumber, byte cot, byte dpi, TMWTime dateTime)
    {
      // db.Tag contains whatever has been put in it at when session was opened

      Console.WriteLine("Master:DPIStore: Function Type: " + functionType.ToString() + " Information Number: " + informationNumber.ToString() + " Cot: " + cot.ToString() + " DPI: " + dpi.ToString());
      return true;
    }
  
    static void OpenMaster()
    {
      masterChan103 = new FT12Channel(TMW_PROTOCOL.I103, TMW_CHANNEL_OR_SESSION_TYPE.MASTER);

      masterChan103.Type = WINIO_TYPE.TCP;
      masterChan103.Name = ".NET I103 Master";  /* name displayed in analyzer window */
      masterChan103.WinTCPipAddress = "127.0.0.1";
      masterChan103.WinTCPipPort = 2404;
      masterChan103.WinTCPmode = TCP_MODE.CLIENT;
      masterChan103.OpenChannel();

      masterSesn103 = new M103Session(masterChan103);
      masterSesn103.OpenSession();

      M103Database.UseSimDatabase = false;
      M103Database.InitEvent += new M103Database.InitDelegate(MasterInitEvent);

      masterSctr103 = new M103Sector(masterSesn103);
      masterSctr103.OpenSector();

      masterDB = (M103Database)masterSctr103.SimDatabase;

      // Can set database Tag here or in MasterInitEvent to something meaningful
      // It will then be available to all database Event routines.
      masterDB.Tag = 0x5555;
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

