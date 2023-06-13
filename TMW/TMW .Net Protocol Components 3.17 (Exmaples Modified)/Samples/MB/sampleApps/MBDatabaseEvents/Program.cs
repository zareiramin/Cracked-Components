using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using TMW.SCL;
using TMW.SCL.ProtocolAnalyzer;
using TMW.SCL.MB;
using TMW.SCL.MB.Master;
using TMW.SCL.MB.Slave;


namespace MBDatabaseEvents
{
  class Program
  {

    static private MMBSession masterSesn;
    static private MBChannel  masterChan;
    static private MMBDatabase masterDB;

    static private SMBSession slaveSesn;
    static private MBChannel slaveChan;
    static private SMBDatabase slaveDB;


    static void Main(string[] args)
    {
      Console.WriteLine("This program will open both a master and a slave.");
      Console.WriteLine("The master will send one Read Coils Request to the slave.");
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

        // register slave events
        slaveDB.CoilsValidateRangeEvent += new SMBDatabase.CoilsValidateRangeDelegate(slaveDB_CoilsValidateRangeEvent);
        slaveDB.GetCoilsEvent += new SMBDatabase.GetCoilsDelegate(slaveDB_GetCoilsEvent);
slaveDB.StoreHoldingRegistersEvent+=SlaveDbOnStoreHoldingRegistersEvent;
        // register event to store coils 
        masterDB.StoreCoilsEvent += new MMBDatabase.StoreCoilsDelegate(masterDB_StoreCoilsEvent);


        // make a General Interrogation request
        MMBRequest req = new MMBRequest(masterSesn);
        req.ReadCoils(10, 10 );
      }
      catch (Exception e)
      {
        Console.WriteLine(e.ToString());
      }
      // wait for key input to finish
      Console.ReadKey();
    }

      private static bool SlaveDbOnStoreHoldingRegistersEvent(SMBDatabase db, ushort startAddr, ushort[] values)
      {
          return true;
      }

   

      /// <summary>
    /// Called by the SCL when database is first opened
    /// <param name="db">database object that called the event</param>
    static void SlaveInitEvent(SMBDatabase db)
    {
      // Set database Tag to something meaningful, like a pointer or reference
      // It will be available to all database Event routines. 
      db.Tag = 0x5555;

      // Perform any database initialization here.
    }

    static bool slaveDB_CoilsValidateRangeEvent(SMBDatabase db, ushort startAddr, ushort quantity)
    {
      // db.Tag contains whatever has been put in it at when session was opened

      Console.WriteLine("Slave::CoilsValidateRange: Start Adddress: " + startAddr.ToString() + " Quantity " + quantity.ToString());
      return true; 
    }

    static bool slaveDB_GetCoilsEvent(SMBDatabase db, ushort startAddr, bool[] values)
    {
      values[0] = true;
      values[1] = false;
      values[2] = true;
      values[3] = false;
      values[4] = true;
      values[5] = false;
      values[6] = true;
      values[7] = false;
      values[8] = true;
      values[9] = false;
      Console.WriteLine("Slave::GetCoils: Start Adddress: " + startAddr.ToString());
      Console.WriteLine("\tValues:");
      for (int i = 0; i < values.Length; i++)
        Console.WriteLine("\t" + values[i].ToString());
      return true;
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

    static void OpenSlave()
    {      
      slaveChan = new MBChannel(TMW_CHANNEL_OR_SESSION_TYPE.SLAVE);

      slaveChan.Type = WINIO_TYPE.TCP;
      slaveChan.Name = ".NET MB Slave";  /* name displayed in analyzer window */
      slaveChan.WinTCPipAddress = "127.0.0.1";
      slaveChan.WinTCPipPort = 502;
      slaveChan.WinTCPmode = TCP_MODE.SERVER;
      slaveChan.OpenChannel();

      SMBDatabase.UseSimDatabase = false;
      SMBDatabase.InitEvent += new SMBDatabase.InitDelegate(SlaveInitEvent);

      slaveSesn = new SMBSession(slaveChan);
      slaveSesn.OpenSession();

      slaveDB = (SMBDatabase)slaveSesn.SimDatabase;

      // Can set database Tag here or in SlaveInitEvent to something meaningful
      // It will then be available to all database Event routines.
      slaveDB.Tag = 0x5555;
    }
  }
}

