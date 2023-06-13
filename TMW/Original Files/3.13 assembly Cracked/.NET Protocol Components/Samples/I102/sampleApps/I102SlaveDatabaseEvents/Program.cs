using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using TMW.SCL;
using TMW.SCL.ProtocolAnalyzer;
using TMW.SCL.IEC60870_5;
using TMW.SCL.IEC60870_5.I102;
using TMW.SCL.IEC60870_5.I102.Slave;


namespace I102DatabaseEvents
{
  class Program
  {
    static private S102Database slaveDB;
    static private S102Session slaveSesn102;
    static private S102Sector slaveSctr102;
    static private FT12Channel slaveChan102;


    static void Main(string[] args)
    {
      Console.WriteLine("This program will start a slave and wait for a connection.");
      Console.WriteLine("The sample source code shows an example of replacing the built-in database.\n");
      Console.WriteLine("Press Enter to end test\n");

      try
      {
        TMWApplicationBuilder applBuilder = new TMWApplicationBuilder();
        TMWApplication pAppl = TMWApplicationBuilder.getAppl();
        pAppl.InitEventProcessor();
        pAppl.EnableEventProcessor = true;

        OpenSlave();

        // register for slave events - we will only deal with oldest integrated totals
        slaveDB.HasOldestIntegratedTotalEvent += new S102Database.HasOldestIntegratedTotalDelegate(slaveDB_HasOldestIntegratedTotalEvent);
        slaveDB.GetOldestIntegratedTotalEvent += new S102Database.GetOldestIntegratedTotalDelegate(slaveDB_GetOldestIntegratedTotalEvent);
      }
      catch (Exception e)
      {
        Console.WriteLine(e.ToString());
      }
      // wait for key input to finish
      Console.ReadKey();
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

