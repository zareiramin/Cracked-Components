using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using TMW.SCL;
using TMW.SCL.ProtocolAnalyzer;
using TMW.SCL.IEC60870_5;
using TMW.SCL.IEC60870_5.I14;

using TMW.SCL.IEC60870_5.I14.Master;
using TMW.SCL.IEC60870_5.I101.Master;

namespace I14DatabaseEvents
{
  class Program
  {

    static private M101Session masterSesn101;
    static private M101Sector masterSctr101;
    static private FT12Channel masterChan101;

    static private M14Database masterDB;

    // For displaying Protocol Analyzer data
    static private bool bProtocolMode = false;
    static ProtocolBuffer protocolBuffer;
    static System.Threading.Timer protocolTimer;

    static void Main(string[] args)
    {
      Console.WriteLine("\nThis program will start a master and try to connect to a separate slave."); 
      Console.WriteLine("It will send a General Interrogation, CSCNA, and CRDNA to the slave.");
      Console.WriteLine("The sample source code shows an example of replacing the built-in database.");

      Console.WriteLine("\nPress 'Y' to show Protocol Analyzer data");
      Console.WriteLine("Press any other key to continue");

      ConsoleKeyInfo key = Console.ReadKey();
      if (key.Key == ConsoleKey.Y)
      {
        bProtocolMode = true;
      }

      Console.WriteLine("\nPress Enter to end test");

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

        // register event on Master to store single point data
        masterDB.MspStoreEvent += new M14Database.MspStoreDelegate(masterDB_MspStoreEvent);

        // make a General Interrogation request
        Thread.Sleep(1000);
        M101Request req = new M101Request(masterSctr101);
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
      // Even though IEC 60870-5-101 would normally be over serial link, use TCP since it makes Demo more friendly */
      bool bSerial = false;

      masterChan101 = new FT12Channel(TMW_PROTOCOL.I101, TMW_CHANNEL_OR_SESSION_TYPE.MASTER);

      if (bSerial)
      {
        masterChan101.Type = WINIO_TYPE.RS232;
        masterChan101.Win232comPortName = "COM3";
        masterChan101.Win232baudRate = "9600";
        masterChan101.Win232numDataBits = RS232_DATA_BITS.BITS_8;
        masterChan101.Win232numStopBits = RS232_STOP_BITS.BITS_1;
        masterChan101.Win232parity = RS232_PARITY.EVEN;
        masterChan101.Win232portMode = RS232_PORT_MODE.NONE;
      }
      else
      {
        masterChan101.Type = WINIO_TYPE.TCP;
        masterChan101.WinTCPipAddress = "127.0.0.1";
        masterChan101.WinTCPipPort = 2404;
        masterChan101.WinTCPmode = TCP_MODE.CLIENT;
      }

      masterChan101.Name = ".NET I101 Master";  /* name displayed in analyzer window */

      masterChan101.OpenChannel();

      masterSesn101 = new M101Session(masterChan101);
      masterSesn101.OpenSession();

      M14Database.UseSimDatabase = false;
      M14Database.InitEvent += new M14Database.InitDelegate(MasterInitEvent);

      masterSctr101 = new M101Sector(masterSesn101);
      masterSctr101.OpenSector();

      masterDB = (M14Database)masterSctr101.SimDatabase;
      
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
