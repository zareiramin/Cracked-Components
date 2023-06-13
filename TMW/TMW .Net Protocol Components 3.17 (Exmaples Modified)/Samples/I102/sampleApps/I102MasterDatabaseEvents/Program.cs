using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using TMW.SCL;
using TMW.SCL.ProtocolAnalyzer;
using TMW.SCL.IEC60870_5;
using TMW.SCL.IEC60870_5.I102;
using TMW.SCL.IEC60870_5.I102.Master;


namespace I102DatabaseEvents
{
  class Program
  {
    static private M102Session masterSesn102;
    static private M102Sector masterSctr102;
    static private FT12Channel masterChan102;
    static private M102Database masterDB;

    // For displaying Protocol Analyzer data
    static private bool bProtocolMode = true;
    static ProtocolBuffer protocolBuffer;
    static System.Threading.Timer protocolTimer;

    static void Main(string[] args)
    {
      Console.WriteLine("\nThis program will start a master and try to connect to a separate slave.");
      Console.WriteLine("It will send one General Interrogation to the slave.");
      Console.WriteLine("The sample source code shows an example of replacing the built-in database.\n");
      Console.WriteLine("Press Enter to end test");

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

        // register for master event to store data
        masterDB.StoreITvalueEvent += new M102Database.StoreITvalueDelegate(masterDB_StoreITvalueEvent);

        // make a General Interrogation request
        M102Request req = new M102Request(masterSctr102);
        req.ccina(11, false);
      }
      catch (Exception e)
      {
        Console.WriteLine(e.ToString());
      }
      // wait for key input to finish
      Console.ReadKey();
    }

    static void MasterInitEvent(M102Database db)
    {
      // Set database Tag to something meaningful, like a pointer or reference
      // It will be available to all database Event routines. 
      db.Tag = 0x5555;

      // Perform any database initialization here.
    }

    static bool masterDB_StoreITvalueEvent(byte typeId, byte recordAddress, byte ioa, byte cot, int itValue, byte sequence, byte signature)
    {
      // db.Tag contains whatever has been put in it at when session was opened

      Console.WriteLine("Master::StoreITValue: Type id " + typeId.ToString() + " RecordAddress: " + recordAddress.ToString() + " IOA: " + ioa.ToString() + " COT: " + cot.ToString() + " Value: " + itValue.ToString());
      return true; 
    }
  
    static void OpenMaster()
    {
      masterChan102 = new FT12Channel(TMW_PROTOCOL.I102, TMW_CHANNEL_OR_SESSION_TYPE.MASTER);

      masterChan102.Type = WINIO_TYPE.TCP;
      masterChan102.Name = ".NET I102 Master";  /* name displayed in analyzer window */
      masterChan102.WinTCPipAddress = "127.0.0.1";
      masterChan102.WinTCPipPort = 2404;
      masterChan102.WinTCPmode = TCP_MODE.CLIENT;
      masterChan102.OpenChannel();

      masterSesn102 = new M102Session(masterChan102);
      masterSesn102.OpenSession();

      M102Database.UseSimDatabase = false;
      M102Database.InitEvent += new M102Database.InitDelegate(MasterInitEvent);

      masterSctr102 = new M102Sector(masterSesn102);

      masterSctr102.OpenSector();
      masterDB = (M102Database)masterSctr102.SimDatabase;

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

