using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using TMW.SCL;
using TMW.SCL.ProtocolAnalyzer;
using TMW.SCL.IEC60870_5;
using TMW.SCL.IEC60870_5.I103;

using TMW.SCL.IEC60870_5.I103.Master;
using TMW.SCL.IEC60870_5.I103.Slave;


namespace I103DatabaseEvents
{
  class Program
  {
    
    static private M103Session masterSesn103;
    static private M103Sector masterSctr103;
    static private FT12Channel masterChan103;

    static private M103Database masterDB;
    static private S103Database slaveDB;

    static private S103Session slaveSesn103;
    static private S103Sector slaveSctr103;
    static private FT12Channel slaveChan103;
    static private bool m_powerOn = true;


    static void Main(string[] args)
    {
      Console.WriteLine("This program will open both a master and a slave connection.");
      Console.WriteLine("The master will send one General Interrogation to the slave.");
      Console.WriteLine("The sample source code shows an example of replacing the built-in database.\n");
      Console.WriteLine("Press Enter to end test\n");

      try
      {
        TMWApplicationBuilder applBuilder = new TMWApplicationBuilder();
        TMWApplication pAppl = TMWApplicationBuilder.getAppl();
        pAppl.InitEventProcessor();
        pAppl.EnableEventProcessor = true;

        OpenSlave();
        OpenMaster();

        //Thread.Sleep(30000);
 
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
       

        // register for the double point store event on master
        masterDB.DpiStoreEvent += new M103Database.DpiStoreDelegate(masterDB_DpiStoreEvent);


        // make a General Interrogation request
        M103Request req = new M103Request(masterSctr103);
        req.gi();
      }
      catch (Exception e)
      {
        Console.WriteLine(e.ToString());
      }
      // wait for key input to finish
      Console.ReadKey();
    }

    static bool masterDB_DpiStoreEvent(M103Database db, byte functionType, byte informationNumber, byte cot, byte dpi, TMWTime dateTime)
    {
      Console.WriteLine("Master:DPIStore: Function Type: " + functionType.ToString() + " Information Number: " + informationNumber.ToString() + " Cot: " + cot.ToString() + " DPI: " + dpi.ToString());
      return true;
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
        dpi = Defs.DIQ_ON;
      else
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
      Console.WriteLine("Slave:DPIGetFunctionType: Point number:" + pointId.ToString() + " return " + ftype.ToString());
      return (Byte)ftype;
    }

    static int slaveDB_GnrlCmdLookupPointEvent(S103Database db, byte functionType, byte informationNumber)
    {
      // one control point
      if (functionType == 128 && informationNumber == 101)
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

      masterSctr103 = new M103Sector(masterSesn103);

      masterSctr103.OpenSector();
      masterDB = (M103Database)masterSctr103.SimDatabase;

      M103Database.UseSimDatabase = false;
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
      S103Database.UseSimControlDatabase = false;

      // monitor data and other database events
      S103Database.UseSimDatabase = false;

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
  }
}

