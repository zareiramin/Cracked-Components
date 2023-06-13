using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using TMW.SCL;
using TMW.SCL.ProtocolAnalyzer;
using TMW.SCL.IEC60870_5;
using TMW.SCL.IEC60870_5.I14;
using TMW.SCL.IEC60870_5.I104;

using TMW.SCL.IEC60870_5.I14.Master;
using TMW.SCL.IEC60870_5.I104.Master;

namespace I14DatabaseEvents
{
  class Program
  {
    static private M104Session masterSesn104;
    static private M104Sector masterSctr104;
    static private I104Channel masterChan104;

    static private M14Database masterDB;

    // For displaying Protocol Analyzer data
    static private bool bProtocolMode = false;
    static ProtocolBuffer protocolBuffer;
    static System.Threading.Timer protocolTimer;

    static void Main(string[] args)
    {
      Console.WriteLine("\nThis program will start a master and try to connect to a separate slave.");
      Console.WriteLine("It will send a General Interrogation, CSCNA, and CRDNA to the slave.");
      Console.WriteLine("The sample source code shows an example of replacing the built-in database.\n");

      Console.WriteLine("\nPress 'Y' to show Protocol Analyzer data");
      Console.WriteLine("Press any other key to continue");

      ConsoleKeyInfo key = Console.ReadKey();
      if (key.Key == ConsoleKey.Y)
      {
        bProtocolMode = true;
      }
      
      Console.WriteLine("\nPress any key to end test");

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

        OpenMaster();


        // register event on Master to store single point data
        masterDB.MspStoreEvent += new M14Database.MspStoreDelegate(masterDB_MspStoreEvent);


        // make a General Interrogation request
        Thread.Sleep(1000);
        M104Request req = new M104Request(masterSctr104);
        req.cicna(M14Request.QOI.GLOBAL, true);

        // Wait 5 seconds then send Single Point Control to IOA 210
        Thread.Sleep(5000);
        req.cscna(M14Request.CTRL_MODE.AUTO, 2100, M14Request.QOC_QU.PERSISTENT, 1);

        // Wait 5 seconds then send Read Command to IOA 700
        Thread.Sleep(5000);
        req.crdna(700);
      
      }
      catch (Exception e)
      {
        Console.WriteLine(e.ToString());
      }
      // wait for key input to finish
      Console.ReadKey();
    }

    static void MasterInitEvent(M14Database db)
    {
      // Set database Tag to something meaningful, like a pointer or reference
      // It will be available to all database Event routines. 
      db.Tag = 0x5555;

      // Perform any database initialization here.
    }

    static bool masterDB_MspStoreEvent(M14Database db, uint ioa, byte cot, byte flags, TMWTime timeStamp)
    {
      // db.Tag contains whatever has been put in it at when session was opened

      Console.WriteLine("Master:MspStore: IOA " + ioa.ToString() + " Cot: " + cot.ToString() + " Flags: " + flags.ToString());
      return true;
    }

    static void OpenMaster()
    {
      masterChan104 = new I104Channel(TMW_CHANNEL_OR_SESSION_TYPE.MASTER);

      masterChan104.Type = WINIO_TYPE.TCP;
      masterChan104.Name = ".NET I104 Master";  /* name displayed in analyzer window */
      masterChan104.WinTCPipAddress = "127.0.0.1";
      masterChan104.WinTCPipPort = 2404;
      masterChan104.WinTCPmode = TCP_MODE.CLIENT;
      masterChan104.OpenChannel();

      masterSesn104 = new M104Session(masterChan104);
      masterSesn104.OpenSession();

      M14Database.UseSimDatabase = false;
      M14Database.InitEvent += new M14Database.InitDelegate(MasterInitEvent);

      masterSctr104 = new M104Sector(masterSesn104);
      masterSctr104.OpenSector();

      masterDB = (M14Database)masterSctr104.SimDatabase;

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
