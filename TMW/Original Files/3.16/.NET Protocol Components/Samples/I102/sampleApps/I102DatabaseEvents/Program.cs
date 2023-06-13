using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using TMW.SCL;
using TMW.SCL.ProtocolAnalyzer;
using TMW.SCL.IEC60870_5;
using TMW.SCL.IEC60870_5.I102;

using TMW.SCL.IEC60870_5.I102.Master;
using TMW.SCL.IEC60870_5.I102.Slave;


namespace I102DatabaseEvents
{
  class Program
  {
  
    static private M102Session masterSesn102;
    static private M102Sector masterSctr102;
    static private FT12Channel masterChan102;

    static private M102Database masterDB;
    static private S102Database slaveDB;

    static private S102Session slaveSesn102;
    static private S102Sector slaveSctr102;
    static private FT12Channel slaveChan102;



    static void Main(string[] args)
    {
      Console.WriteLine("This program will open both a master and a slave.");
      Console.WriteLine("The master will send one General Interrogation to the slave.");
      Console.WriteLine("The sample source code shows an example of replacing the built-in database.\n");
     
      Console.WriteLine("Press Enter to end test");

      try
      {
        TMWApplicationBuilder applBuilder = new TMWApplicationBuilder();
        TMWApplication pAppl = TMWApplicationBuilder.getAppl();
        pAppl.EnableEventProcessor = true;
        pAppl.InitEventProcessor();

        OpenSlave();
        OpenMaster();

        // register for slave events - we will only deal with oldest integrated totals
        slaveDB.HasOldestIntegratedTotalEvent += new S102Database.HasOldestIntegratedTotalDelegate(slaveDB_HasOldestIntegratedTotalEvent);
        slaveDB.GetOldestIntegratedTotalEvent += new S102Database.GetOldestIntegratedTotalDelegate(slaveDB_GetOldestIntegratedTotalEvent);

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

    static void SlaveInitEvent(S102Database db)
    {
      // Set database Tag to something meaningful, like a pointer or reference
      // It will be available to all database Event routines. 
      db.Tag = 0x1111;

      // Perform any database initialization here.
    }

    static S2_IT_TOTAL_COT slaveDB_HasOldestIntegratedTotalEvent(S102Database db, byte asduTypeId, byte numOctets, byte recordAddress, TMWTime oldestTime)
    {
      // db.Tag contains whatever has been put in it at when session was opened

      if (asduTypeId == (Byte)I2TYPE_ID.CCINA && numOctets == 4 && recordAddress == 11)
      {
        Console.WriteLine("Slave:HasOldestIntegratedTotal: Type Id " + asduTypeId.ToString() + " NumOctets: " + numOctets.ToString() + " Record Address: " + recordAddress.ToString());
        
        DateTime now;
        now = DateTime.Now;
        oldestTime.Hour = (Byte)now.Hour;
        oldestTime.Minute = (Byte)now.Minute;
        oldestTime.Second = (UInt16)now.Second;
        oldestTime.Day = (Byte)now.Day;
        oldestTime.Month = (Byte)now.Month;
        oldestTime.Year = (UInt16)now.Year;

        return S2_IT_TOTAL_COT.Found;
      }

      return S2_IT_TOTAL_COT.NoDataForRecord;
    }

    static bool slaveDB_GetOldestIntegratedTotalEvent(S102Database db, byte asduTypeId, byte numOctets, byte recordAddress, TMWTime oldestTime, ref ushort recordIndex, ref byte ioa, ref int itValue, ref byte sequence)
    {
      // just return 2 records
      if (recordIndex == 2)
        return false;

      ioa = (Byte)(100 + recordIndex);
      itValue = 333 + recordIndex;
      sequence = (Byte)(12 + recordIndex);

      recordIndex++;

      Console.WriteLine("Slave:GetOldestIntegratedTotal: Type Id " + asduTypeId.ToString() + " NumOctets: " + numOctets.ToString() + " Record Address: " + recordAddress.ToString());
      Console.WriteLine("\tRecordIndex: " + recordIndex.ToString() + " Value: " + itValue.ToString() + " Time: " + oldestTime.ToString());

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

    static void OpenSlave()
    {
      slaveChan102 = new FT12Channel(TMW_PROTOCOL.I102, TMW_CHANNEL_OR_SESSION_TYPE.SLAVE);

      slaveChan102.Type = WINIO_TYPE.TCP;
      slaveChan102.Name = ".NET I102 Slave";  /* name displayed in analyzer window */
      slaveChan102.WinTCPipAddress = "127.0.0.1";
      slaveChan102.WinTCPipPort = 2404;
      slaveChan102.WinTCPmode = TCP_MODE.SERVER;
      slaveChan102.OpenChannel();

      slaveSesn102 = new S102Session(slaveChan102);
      slaveSesn102.OpenSession();

      S102Database.UseSimDatabase = false;
      S102Database.InitEvent += new S102Database.InitDelegate(SlaveInitEvent);

      slaveSctr102 = new S102Sector(slaveSesn102);
      slaveSctr102.OpenSector();

      slaveDB = (S102Database)slaveSctr102.SimDatabase;

      // Can set database Tag here or in SlaveInitEvent to something meaningful
      // It will then be available to all database Event routines.
      slaveDB.Tag = 0x5555;
    }
  }
}

