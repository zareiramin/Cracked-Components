using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using TMW.SCL;
using TMW.SCL.ProtocolAnalyzer;
using TMW.SCL.MB;
using TMW.SCL.MB.Slave;
 

namespace MBDatabaseEvents
{
  class Program
  {
    static private SMBSession slaveSesn;
    static private MBChannel slaveChan;
    static private SMBDatabase slaveDB;
 
    static private bool []m_values;

    static void Main(string[] args)
    {
      Console.WriteLine("This program will start a slave and wait for a connection.");
      Console.WriteLine("The sample source code shows an example of replacing the built-in database.\n");
      Console.WriteLine("Press Enter to end test\n");

      m_values = new bool[100];
      for (int i = 0; i < 100; i += 2)
      {
        m_values[i] = false;
        m_values[i + 1] = true;
      }

      try
      {
        TMWApplicationBuilder applBuilder = new TMWApplicationBuilder();
        TMWApplication pAppl = TMWApplicationBuilder.getAppl();
        pAppl.InitEventProcessor();
        pAppl.EnableEventProcessor = true;

        OpenSlave();

        // register slave events
        slaveDB.CoilsValidateRangeEvent += new SMBDatabase.CoilsValidateRangeDelegate(slaveDB_CoilsValidateRangeEvent);
        slaveDB.GetCoilsEvent += new SMBDatabase.GetCoilsDelegate(slaveDB_GetCoilsEvent);
     
      }
      catch (Exception e)
      {
        Console.WriteLine(e.ToString());
      }
      // wait for key input to finish
      Console.ReadKey();
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

      if ((startAddr + quantity) <= 100)
        return true;

      return false; 
    }

    static bool slaveDB_GetCoilsEvent(SMBDatabase db, ushort startAddr, bool[] values)
    { 

      // quantity of coils to read is specified by values.Length 
      for(int i=0; i<values.Length; i++)
        values[i] = m_values[startAddr+i];

        values[0] = values[1] = values[2] = true;

      Console.WriteLine("Slave::GetCoils: Start Adddress: " + startAddr.ToString());
      Console.WriteLine("\tValues:");
      for (int i = 0; i < values.Length; i++)
        Console.WriteLine("\t" + values[i].ToString());
      return true;
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

