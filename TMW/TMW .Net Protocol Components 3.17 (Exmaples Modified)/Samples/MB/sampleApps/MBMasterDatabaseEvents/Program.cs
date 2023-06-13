using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using TMW.SCL;
using TMW.SCL.ProtocolAnalyzer;
using TMW.SCL.MB;
using TMW.SCL.MB.Master;


namespace MBDatabaseEvents
{
  class Program
  {
    static private MMBSession masterSesn;
    static private MBChannel  masterChan;
    static private MMBDatabase masterDB;

    // For displaying Protocol Analyzer data
    static private bool bProtocolMode = false;
    static ProtocolBuffer protocolBuffer;
    static System.Threading.Timer protocolTimer;

    static void Main(string[] args)
    { 
      Console.WriteLine("\nThis program will start a master and try to connect to a separate slave.");
      Console.WriteLine("It will send one Read Coils Command to the slave.");
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

        // register event to store coils 
        masterDB.StoreCoilsEvent += new MMBDatabase.StoreCoilsDelegate(masterDB_StoreCoilsEvent);


        // make a General Interrogation request
        MMBRequest req = new MMBRequest(masterSesn);
        req.ReadCoils(10, 10);
      }
      catch (Exception e)
      {
        Console.WriteLine(e.ToString());
      }
      // wait for key input to finish
      Console.ReadKey();
    }

    static void MasterInitEvent(MMBDatabase db)
    {
      // Set database Tag to something meaningful, like a pointer or reference
      // It will be available to all database Event routines. 
      db.Tag = 0x5555;

      // Perform any database initialization here.
    }

    static bool masterDB_StoreCoilsEvent(MMBDatabase db, ushort startAddr, bool[] values)
    {
      // db.Tag contains whatever has been put in it at when session was opened

      Console.WriteLine("Master::StoreCoilsEvent: Start Adddress: " + startAddr.ToString());
      Console.WriteLine("\tValues:");
      for (int i = 0; i < values.Length; i++)
        Console.WriteLine("\t" + values[i].ToString());
      return true; 
    }
  
    static void OpenMaster()
    {
      masterChan = new MBChannel(TMW_CHANNEL_OR_SESSION_TYPE.MASTER);

      masterChan.Type = WINIO_TYPE.TCP;
      masterChan.Name = ".NET MB Master";  /* name displayed in analyzer window */
      masterChan.WinTCPipAddress = "127.0.0.1";
      masterChan.WinTCPipPort = 502;
      masterChan.WinTCPmode = TCP_MODE.CLIENT;
      masterChan.OpenChannel();

      MMBDatabase.UseSimDatabase = false;
      MMBDatabase.InitEvent += new MMBDatabase.InitDelegate(MasterInitEvent);

      masterSesn = new MMBSession(masterChan);
      masterSesn.OpenSession();

      masterDB = (MMBDatabase)masterSesn.SimDatabase;

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

