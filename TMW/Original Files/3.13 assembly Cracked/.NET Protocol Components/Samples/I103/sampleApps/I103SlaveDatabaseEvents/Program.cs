using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using TMW.SCL;
using TMW.SCL.ProtocolAnalyzer;
using TMW.SCL.IEC60870_5;
using TMW.SCL.IEC60870_5.I103;

using TMW.SCL.IEC60870_5.I103.Slave;


namespace I103SlaveDatabaseEvents
{
  class Program
  {
    static private S103Database slaveDB;
    static private S103Session slaveSesn103;
    static private S103Sector slaveSctr103;
    static private FT12Channel slaveChan103;
    static private bool m_powerOn = true;

    // For displaying Protocol Analyzer data
    static private bool bProtocolMode = false;
    static ProtocolBuffer protocolBuffer;
    static System.Threading.Timer protocolTimer;

    static void Main(string[] args)
    {
      Console.WriteLine("\nThis program will start a slave and wait for a connection.");
      Console.WriteLine("The sample source code shows an example of replacing the built-in database.");

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
        pAppl.InitEventProcessor();
        pAppl.EnableEventProcessor = true;

        if (bProtocolMode)
        {
          // Internal Protocol Timer is a Windows.Forms.Timer 
          // Create our own timer since this is not a Forms application
          protocolTimer = new System.Threading.Timer(OnUpdateProtocolBufferTimer, null, 500, 500);
          protocolBuffer = TMWApplicationBuilder.getProtocolBuffer();
          protocolBuffer.ProtocolDataReadyEvent += OnNewProtocolData;
        }

        OpenSlave();
 
        // register for double point events on slave, because UseSimDatabase was set to false
        slaveDB.DpiGetPointEvent += new S103Database.DpiGetPointDelegate(slaveDB_DpiGetPointEvent);
        S103Database.DpiGetFunctionTypeEvent += new S103Database.DpiGetFunctionTypeDelegate(S103Database_DpiGetFunctionTypeEvent);
        S103Database.DpiGetInformationNumberEvent += new S103Database.DpiGetInformationNumberDelegate(S103Database_DpiGetInformationNumberEvent);
        S103Database.DpiGetResponseModeEvent += new S103Database.DpiGetResponseModeDelegate(S103Database_DpiGetResponseModeEvent);
        S103Database.DpiGetValueEvent += new S103Database.DpiGetValueDelegate(S103Database_DpiGetValueEvent);

        // register for general command events on slave, because UseSimControlDatabase was set to false
        slaveDB.GnrlCmdLookupPointEvent += new S103Database.GnrlCmdLookupPointDelegate(slaveDB_GnrlCmdLookupPointEvent);
        S103Database.GnrlCmdSetValueEvent += new S103Database.GnrlCmdSetValueDelegate(S103Database_GnrlCmdSetValueEvent);
        slaveDB.GnrlCmdSendChangeEvent += new S103Database.GnrlCmdSendChangeEventDelegate(slaveDB_GnrlCmdSendChangeEvent);

        // Add a monitored single point event 
        // This will get the current time for this sector. 
        // This will end up calling .CcsnaGetTimeEvent if it is registered, so you could just set the timeStamp here instead of calling GetTimeStamp().
        TMWTime timestamp;
        Thread.Sleep(2000);
        timestamp = slaveSctr103.GetTimeStamp();
        slaveSctr103.DpiRelAddEvent(128, 16, true, 100, 200, timestamp, TMW_CHANGE_REASON.SPONTANEOUS);

      }
      catch (Exception e)
      {
        Console.WriteLine(e.ToString());
      }
      // wait for key input to finish
      Console.ReadKey();
    }

    static void SlaveInitEvent(S103Database db)
    {
      // Set database Tag to something meaningful, like a pointer or reference
      // It will be available to all database Event routines. 
      db.Tag = 0x1111;

      // Perform any database initialization here.
    }

    static byte slaveDB_GetResetCOTEvent(S103Database db)
    {
      if (m_powerOn)
      {
        // Indicate that COT PowerOn should be sent
        m_powerOn = false;
        return (byte)Defs.COT.POWERON;
      }
      else
        // Indicate that COT Restart should be sent
        return (byte)Defs.COT.RESTART;
    }

    static void S103Database_GetAsciiIdEvent(byte[] buf)
    {
      // Identification Message Ascii Id
      buf[0] = (byte)'1';
      buf[1] = (byte)'2';
      buf[2] = (byte)'3';
      buf[3] = (byte)'4';
      buf[4] = (byte)'5';
      buf[5] = (byte)'6';
      buf[6] = (byte)'7';
      buf[7] = (byte)'8';
    }

    static void S103Database_GetInternalIdEvent(ref uint internalId)
    {
      // Identification Message Internal Software Id.
      internalId = 0x11223344;
    }

    static int slaveDB_DpiGetPointEvent(S103Database db, ushort index)
    {
      // return 2 points
      int pointId = index + 1;
      if (pointId < 3)
      {
        Console.WriteLine("Slave:DPIGetPoint: Index: " + index.ToString() + " PointId: " + pointId.ToString());

        // Return Unique id for this point, to be passed as pointId to other events
        return pointId;
      }

      // no more points
      return 0;
    }

    static void S103Database_DpiGetValueEvent(int pointId, ref byte dpi)
    {
      // pointId was returned by slaveDB_DpiGetPointEvent
      if (pointId == 1)
        // In this sample first point is on
        dpi = Defs.DIQ_ON;
      else
        // other point is off
        dpi = Defs.DIQ_OFF;

      Console.WriteLine("Slave:DPIGetValue: Point number: " + pointId.ToString() + " DPI = " + dpi.ToString());
    }

    static byte S103Database_DpiGetResponseModeEvent(int pointId)
    {
      // pointId was returned by slaveDB_DpiGetPointEvent
      Console.WriteLine("Slave:DPIGetResponseMode");

      // return that this point is in a response to GI mode requests
      return (Byte)Defs.RESPONSE_MODE.GI;
    }

    static byte S103Database_DpiGetInformationNumberEvent(int pointId)
    {
      // pointId was returned by slaveDB_DpiGetPointEvent
      int ioa = pointId + 100;
      Console.WriteLine("Slave:DPIGetInformationNumber: Point number: " + pointId.ToString() + " return " + ioa.ToString());
      return (Byte)ioa;
    }

    static byte S103Database_DpiGetFunctionTypeEvent(int pointId)
    {
      // pointId was returned by slaveDB_DpiGetPointEvent
      int ftype = 128;
      Console.WriteLine("Slave:DPIGetFunctionType: Point number: " + pointId.ToString() + " return " + ftype.ToString());
      return (Byte)ftype;
    }

    static int slaveDB_GnrlCmdLookupPointEvent(S103Database db, byte functionType, byte informationNumber)
    {
      // one control point
      if (functionType ==128 && informationNumber == 101)
      {
        Console.WriteLine("Slave:CnrlCmdLookupPoint: informationNumber: " + informationNumber.ToString() + " Point Found");
        return 1;
      }

      // point does not exist
      Console.WriteLine("Slave:CnrlCmdLookupPoint: informationNumber: " + informationNumber.ToString() + " Point does not exist");
      return 0;
    }

    static bool S103Database_GnrlCmdSetValueEvent(int pointId, byte dpi)
    {
      // pointId was returned by slaveDB_GnrlCmdLookupPointEvent
      if (pointId == 1)
      {
        if (dpi == Defs.DIQ_ON)
          Console.WriteLine("Slave:GnrlCmdSetValueEvent: point ID: " + pointId.ToString() + " ON");
         else
          Console.WriteLine("Slave:GnrlCmdSetValueEvent: point ID: " + pointId.ToString() + " OFF");
        return true;
      }
      else
        return false;
    }

    static bool slaveDB_GnrlCmdSendChangeEvent(S103Database db, byte functionType, byte informationNumber, ref byte dpi, TMWTime time)
    {
      // pointId was returned by slaveDB_GnrlCmdLookupPointEvent

      // Indicate that the change event with COT 12 Remote Operation should be sent automatically by library.
      // time and dpi values can be modified if you have a reason to.
      if (functionType == 128 && informationNumber == 101)
        return true;
      else
        return false;
    }
     
    static void OpenSlave()
    {
      slaveChan103 = new FT12Channel(TMW_PROTOCOL.I103, TMW_CHANNEL_OR_SESSION_TYPE.SLAVE);

      slaveChan103.Type = WINIO_TYPE.TCP;
      slaveChan103.Name = ".NET I103 Slave";  /* name displayed in analyzer window */
      slaveChan103.WinTCPipAddress = "127.0.0.1";
      slaveChan103.WinTCPipPort = 2404;
      slaveChan103.WinTCPmode = TCP_MODE.SERVER;
      slaveChan103.OpenChannel();

      slaveSesn103 = new S103Session(slaveChan103);
      slaveSesn103.OpenSession();

      // control operation events
      S103Database.UseSimControlDatabase = true;

      // monitor data and other database events
      S103Database.UseSimDatabase = true;

      S103Database.InitEvent += new S103Database.InitDelegate(SlaveInitEvent);

      // The following three events control what is sent in TypeId 5 Identification message
      S103Database.GetResetCOTEvent += new S103Database.GetResetCOTDelegate(slaveDB_GetResetCOTEvent);
      S103Database.GetAsciiIdEvent += new S103Database.GetAsciiIdDelegate(S103Database_GetAsciiIdEvent);
      S103Database.GetInternalIdEvent += new S103Database.GetInternalIdDelegate(S103Database_GetInternalIdEvent);


      slaveSctr103 = new S103Sector(slaveSesn103);
      slaveSctr103.RbeScanPeriod = 1000;
      slaveSctr103.OpenSector();

      slaveDB = (S103Database)slaveSctr103.SimDatabase;
 
      // Can set database Tag here or in SlaveInitEvent to something meaningful
      // It will then be available to all database Event routines.
      slaveDB.Tag = 0x5555;
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

