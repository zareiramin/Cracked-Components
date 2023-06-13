using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using TMW.SCL;
using TMW.SCL.ProtocolAnalyzer;
using TMW.SCL.IEC60870_5;
using TMW.SCL.IEC60870_5.I14;

using TMW.SCL.IEC60870_5.I14.Slave;
using TMW.SCL.IEC60870_5.I101.Slave;

namespace I14DatabaseEvents
{
  class Program
  {
    
    static private S14Database slaveDB;
    static private S101Session slaveSesn101;
    static private S101Sector slaveSctr101;
    static private FT12Channel slaveChan101;
    static private byte[] mspValue;

    static private byte m_sectorResetInfo = (byte)((byte)Defs.COI.POWER_ON | (byte)Defs.COI.LOCAL_PARAMS_CHANGED);

    // For displaying Protocol Analyzer data
    static private bool bProtocolMode = false;
    static ProtocolBuffer protocolBuffer;
    static System.Threading.Timer protocolTimer;

    static void Main(string[] args)
    {
      // only two Monitored Single Points in this example database
      mspValue = new byte[2];

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

        // This causes application to process received data and timers.
        TMWApplication pAppl = TMWApplicationBuilder.getAppl();
        pAppl.EnableEventProcessor = true;
        pAppl.InitEventProcessor();

        // Change Simulated (default) to System time, but do not allow clock sync to modify system time.
        //pAppl.TimeMode = TIME_MODE.SYSTEM_NO_SETTIME;
   

        OpenSlave();

        // register events to tell the library when to send an End Of Init MEINA
        // If UseSimDatabase==true and these are not registered, the library will not send MEINA at startup
        slaveDB.HasSectorResetEvent += new S14Database.HasSectorResetDelegate(slaveDB_HasSectorResetEvent);
        slaveDB.ClearSectorResetEvent += new S14Database.ClearSectorResetDelegate(slaveDB_ClearSectorResetEvent);

        // Register for clock sync events. (UseSimDatabase was set to false)
        // Depending on pAppl.TimeMode the simulated or system time will also be set.
        // These two events do not have to be registered unless you want to modify the normal simulated or system clock behavior.
        slaveDB.CcsnaGetTimeEvent += new S14Database.CcsnaGetTimeDelegate(slaveDB_CcsnaGetTimeEvent);
        slaveDB.CcsnaSetTimeEvent += new S14Database.CcsnaSetTimeDelegate(slaveDB_CcsnaSetTimeEvent);
        
        // register events on Slave to handle monitored single point data    (UseSimDatabase was set to false)
        slaveDB.MspGetPointEvent += new S14Database.MspGetPointDelegate(slaveDB_MspGetPointEvent);
        S14Database.MspGetGroupMaskEvent += new S14Database.MspGetGroupMaskDelegate(S14Database_MspGetGroupMaskEvent);
        S14Database.MspGetFlagsAndValueEvent += new S14Database.MspGetFlagsAndValueDelegate(S14Database_MspGetFlagsAndValueEvent);
        S14Database.MspGetIndexedEvent += new S14Database.MspGetIndexedDelegate(S14Database_MspGetIndexedEvent);
        S14Database.MspGetInfoObjAddrEvent += new S14Database.MspGetInfoObjAddrDelegate(S14Database_MspGetInfoObjAddrEvent);
        S14Database.MspGetFlagsValueTimeEvent += new S14Database.MspGetFlagsValueTimeDelegate(S14Database_MspGetFlagsValueTimeEvent);

        // register events on Slave to handle normalized value point data    (UseSimDatabase was set to false)
        slaveDB.MmenaGetPointEvent += new S14Database.MmenaGetPointDelegate(slaveDB_MmenaGetPointEvent);
        S14Database.MmenaGetGroupMaskEvent += new S14Database.MmenaGetGroupMaskDelegate(S14Database_MmenaGetGroupMaskEvent);
        S14Database.MmenaGetFlagsEvent += new S14Database.MmenaGetFlagsDelegate(S14Database_MmenaGetFlagsEvent);
        S14Database.MmenaGetValueEvent += new S14Database.MmenaGetValueDelegate(S14Database_MmenaGetValueEvent);
        S14Database.MmenaGetIndexedEvent += new S14Database.MmenaGetIndexedDelegate(S14Database_MmenaGetIndexedEvent);
        S14Database.MmenaGetInfoObjAddrEvent += new S14Database.MmenaGetInfoObjAddrDelegate(S14Database_MmenaGetInfoObjAddrEvent);
        S14Database.MmenaGetValueFlagsTimeEvent += new S14Database.MmenaGetValueFlagsTimeDelegate(S14Database_MmenaGetValueFlagsTimeEvent);

        // register for other monitored database points if desired, MdpGetPointEvent, MstGetPointEvent, PmenaGetPointEvent etc.
        // The xxxGetPointEvent events are used for monitored data that is read from the database using indexes,

        // register events on Slave to handle single point control operations. (UseSimControlDatabase was set to false)
        slaveDB.CscLookupPointEvent += new S14Database.CscLookupPointDelegate(slaveDB_CscLookupPointEvent);
        S14Database.CscSelectRequiredEvent += new S14Database.CscSelectRequiredDelegate(S14Database_CscSelectRequiredEvent);
        S14Database.CscSelectEvent += new S14Database.CscSelectDelegate(S14Database_CscSelectEvent);
        S14Database.CscExecuteEvent += new S14Database.CscExecuteDelegate(S14Database_CscExecuteEvent);
        S14Database.CscGetMonitoredPointEvent += new S14Database.CscGetMonitoredPointDelegate(S14Database_CscGetMonitoredPointEvent);
        S14Database.CscStatusEvent += new S14Database.CscStatusDelegate(S14Database_CscStatusEvent);

        // register for other control operations if desired, CdcLookupPointEvent, CrcGetPointEvent, CsenaLookupPointEvent, PmenaLookupPointEvent, etc
        // The xxxLookupPointEvent events are used for control data that is "written" to the database using Information Object Addresses.

         
        // Add a single point event 
        // This will get the current time for this sector. 
        // This will end up calling .CcsnaGetTimeEvent if it is registered, so you could just set the timeStamp here instead of calling GetTimeStamp().
        TMWTime timestamp;
        timestamp = slaveSctr101.GetTimeStamp();
        slaveSctr101.MspAddEvent(100, true, 1, timestamp, TMW_CHANGE_REASON.SPONTANEOUS);

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
    static void SlaveInitEvent(S14Database db)
    {
      // Set database Tag to something meaningful, like a pointer or reference
      // It will be available to all database Event routines. 
      db.Tag = 0x5555;

      // Perform other initialization processing here.
    }


    // If this is not registered the SIMULATED or SYSTEM time will be used according to TMWApplication::TimeMode
    static void slaveDB_CcsnaGetTimeEvent(S14Database db, ref TMWTime time)
    { 
      // This returns the system time.
      time.SetToNow();
    }

    static Boolean slaveDB_CcsnaSetTimeEvent(S14Database db, TMWTime time)
    {
      // Add your code here if you want to change the time for this sector.
      return true;
    }
 
    static void slaveDB_ClearSectorResetEvent(S14Database db)
    {
      // Clear this so sample no longer tells library to send MEINA.
      // If CRPNA is received you could set this to Defs.COI.REMOTE_RESET 
      m_sectorResetInfo = 0;
    }

    static bool slaveDB_HasSectorResetEvent(S14Database db, ref byte resetCOI)
    {
      // Tell the library that the sector has reset so it sends the MEINA.
      if (m_sectorResetInfo != 0)
      {
        resetCOI = m_sectorResetInfo;
        return true;
      }
      return false;
    }


    static int slaveDB_MspGetPointEvent(S14Database db, ushort index)
    {
      // db.Tag contains whatever has been put in it at when session was opened
      // it is being ignored here because there is only a single database.

      Console.WriteLine("Slave:MSPGetPoint: Index " + index.ToString());

      // In this example there are two Monitored Single Points, 
      // they return point identifier 1 and 2 which will be passed into the other event methods as pointId.
      if (index < 2)
        return index + 1;

      return 0;
    }

    static uint S14Database_MspGetInfoObjAddrEvent(int pointId)
    {
      // In this example the two monitored points have IOAs 84 and 85
      if (pointId == 1)
        Console.WriteLine("Slave:MspGetInfoObjAddr Point Id " + pointId.ToString() + " Return: 84");
      else
        Console.WriteLine("Slave:MspGetInfoObjAddr Point Id " + pointId.ToString() + " Return: 85");

      return (uint)(pointId + 83);
    }

    static bool S14Database_MspGetIndexedEvent(int pointId)
    {
      // In this example tell the library to send all of the MSP points as indexed not sequential
      Console.WriteLine("Slave:MspGetIndexed Point Id " + pointId.ToString());
      return true;
    }

    static byte S14Database_MspGetFlagsAndValueEvent(int pointId)
    {
      byte value = 0;
      // Return values and flags for the MSP point.
      // returns single point information with quality descriptor as defined in 7.2.6.1
      //          The return value contains a status indication and the current state of the point.
      //          The following values (or OR'd combinations) are valid for this type:
      //   Defs.SIQ_OFF    - Single Point Information value (Off)
      //   Defs.SIQ_ON     - Single Point Information value (On)
      //   Defs.QUALITY.BL - Blocked
      //   Defs.QUALITY.SB - Substituted
      //   Defs.QUALITY.NT - Topical
      //   Defs.QUALITY.IV - Invalid
      if (pointId == 1)
      { 
        //value = Defs.SIQ_ON ;
        value = mspValue[0];
      }
      else
      {
        value = (byte)((int)Defs.SIQ_OFF | (int)Defs.QUALITY.IV);
      }
        
      Console.WriteLine("Slave:MspFlagsAndValue Point Id " + pointId.ToString() + " Return: " + value.ToString());
      return (value);
    }

    static TMW_GROUP_MASK S14Database_MspGetGroupMaskEvent(int pointId)
    {
      Console.WriteLine("Slave:MspGetGroupMask: PointID " + pointId.ToString());

      // Respond to general interrogation, CICNA global
      return TMW_GROUP_MASK.GENERAL;
    }

    static byte S14Database_MspGetFlagsValueTimeEvent(int pointId, ref TMWTime time)
    {
      if (pointId == 1)
      {
        // This will end up calling the registered event slaveDB_CcsnaGetTimeEvent 
        // so you could just do the same thing and call time.SetToNow() here.
        time = slaveSctr101.GetTimeStamp();

        // return (Defs.SIQ_ON);
        return(mspValue[0]);
      }
       return 0;
    }

    static int slaveDB_MmenaGetPointEvent(S14Database db, ushort index)
    {
      // In this example is one Normalized Measurand Point, 
      // it returns point identifier 10 which will be passed into the other mmena event methods as pointId.
      if (index < 1)
        return (index + 10);

      return 0;
    }

    static TMW_GROUP_MASK S14Database_MmenaGetGroupMaskEvent(int pointId)
    {
      // In this example measurands are included in cyclic data which is send periodically.
      return TMW_GROUP_MASK.MASK_CYCLIC;
    }

    static uint S14Database_MmenaGetValueEvent(int pointId)
    {
      if (pointId == 10)
        return 111;

      return 0;
    }

    static byte S14Database_MmenaGetFlagsEvent(int pointId)
    {
      // This example point has invalid quality
      if (pointId == 10)
        return (byte)Defs.QUALITY.IV;

      return 0;
    }

    static uint S14Database_MmenaGetInfoObjAddrEvent(int pointId)
    {
      // Measurand has Information Object address 700 
      if (pointId == 10)
        return 700;
      else
        return 0;
    }

    static bool S14Database_MmenaGetIndexedEvent(int pointId)
    {
      // In this example tell the library to send all of the MMENA points as indexed not sequential
      return true;
    }


    static Int16 S14Database_MmenaGetValueFlagsTimeEvent(int pointId, ref byte flags, ref TMWTime time)
    {
      // If CRDNA was received for the mmena point and it was configured to return the with time type
      if (pointId == 10)
      {
        // This will end up calling the registered event slaveDB_CcsnaGetTimeEvent 
        // so you could just do the same thing and call time.SetToNow() here.

        time = slaveSctr101.GetTimeStamp();

        // Say value overflowed
        flags = (byte)Defs.QUALITY.OV;
        return 111;
      }
      return 0;
    }

      
  /* 
   * Command Processing
   * Details are presented here using Single Point Commands (CSC) as an example.
   *  The exact same description applies to 
   *  Double Point Commands(CDC), and
   *  Regulating Step Commands (CRC). 
   *
   * SINGLE POINT COMMANDS
   * When a single point direct execute command is received from the master 
   * CscSelectRequiredEvent() will be called to verify that a select is not 
   * required before the execute. This should return false if a select 
   * is not required before the execute.
   *
   * CscExecuteEvent() will be called to perform the execute.
   * If the command can be performed immediately, the function should return 
   * SUCCESS or if it fails, it should return 
   * FAILED. If the command takes time to complete, it should 
   * return EXECUTING.
   * 
   * If it returned EXECUTING, the function CscStatusEvent() will then be 
   * called periodically. When the execute completes, this status function 
   * should return SUCCESS or FAILED.
   * 
   * If the execute command or status command while executing returns FAILED an 
   * error (ACT CON Negative) will be sent back to the master.
   *
   * If the execute or status function returns SUCCESS, the library will then call
   * CscGetMonitoredPointEvent to determine if it should go to MONITORING mode. 
   * If MONITORING, CscStatusEvent() will be called periodically until it returns 
   * anything other than MONITORING, at which time the library will
   * read the monitored feedback point to generate a change event. 
   * A failure while monitoring will need to be returned via the flag bits in the 
   * monitored data. The protocol does not allow for an ACT TERM Negative to be sent.
   *
   * Note:
   * If select before execute is required, a call to CscSelectEvent() will 
   * precede the call to CscExecuteEvent() This can return 
   * SELECTING, FAILED or SUCCESS. 
   * If it returned SELECTING, CscStatusEvent() will be called until it returns 
   * TSUCCESS or FAILED. If it returned SUCCESS, 
   * CscExecuteEvent() will be called.
   */
    static int slaveDB_CscLookupPointEvent(S14Database db, UInt32 ioa)
    {
      // In this example the only control point is ioa 2100 
      if (ioa == 2100)
        return 20;
      else
        // return 0 if lookup fails
        return 0;
    }
 
    static bool S14Database_CscSelectRequiredEvent(int pointId)
    {
      // In this example all single points require select before operate
      return true;
    }

    static TMW_COMMAND_STATUS S14Database_CscSelectEvent(int pointId, byte cot, byte sco)
    {
      // Could have returned SELECTING if the command is slow, 
      // FAILED if it was unsuccessful
      // or SUCCESS if the select was successful

      // Since there is only a single control point, these functions will only be called with pointId returned by slaveDB_CscLookupPointEvent
      // But for completeness it does not hurt to verify pointId.
      if(pointId == 20)
        return TMW_COMMAND_STATUS.SUCCESS;

      return TMW_COMMAND_STATUS.FAILED;
    }

    static TMW_COMMAND_STATUS S14Database_CscExecuteEvent(int pointId, byte cot, byte sco)
    {
      //  sco contains Defs.SCS_OFF|Defs.SCS_ON 
      //  QOC QU values in bits Defs.QOC_QU_MASK are: 
      //   Defs.QOC_QU_USE_DEFAULT - No additional definition
      //   Defs.QOC_QU_SHORT_PULSE - Short pulse duration (circuit-breaker)
      //   Defs.QOC_QU_LONG_PULSE  - Long pulse duration
      //   Defs.QOC_QU_PERSISTENT  - Persistent output
      if ((sco & Defs.SCS_ON) == Defs.SCS_ON)
      {
        mspValue[0] = Defs.SCS_ON;
        Console.WriteLine("Slave:CSCExecute: ON Index " + pointId.ToString());
      }
      else
      {
        mspValue[0] = Defs.SCS_OFF;
        Console.WriteLine("Slave:CSCExecute: OFF Index " + pointId.ToString());
      }
 
      // Could have returned EXECUTING if the command is slow, 
      // FAILED if the command was unsuccessful
      // or SUCCESS if the command was successful
      return TMW_COMMAND_STATUS.SUCCESS;
    }

    static Int32 S14Database_CscGetMonitoredPointEvent(Int32 pointId)
    {
      // Return same handle as slaveDB_MspGetPointEvent
      // if you want the library to send a M_SP with COT 11 RETURN_REMOTE
      if (pointId == 20)
        return 1;
      else
        return 0;
    }
       
    static TMW_COMMAND_STATUS S14Database_CscStatusEvent(Int32 pointId)
    {
      if (pointId == 20)
        return TMW_COMMAND_STATUS.SUCCESS;
      else
        return TMW_COMMAND_STATUS.FAILED;

      // Could have returned SELECTING if the select is still in progress, 
      // Could have returned EXECUTING if the execute is still in progress,
      // Could have returned MONITORING if the monitor is still in progress,
      // FAILED if the command was unsuccessful
      // or SUCCESS if the command was successful
    }
     
    static void OpenSlave()
    {
      // Even though IEC 60870-5-101 would normally be over serial link, use TCP since it makes Demo more friendly */
      bool bSerial = false;

      slaveChan101 = new FT12Channel(TMW_PROTOCOL.I101, TMW_CHANNEL_OR_SESSION_TYPE.SLAVE);

      if (bSerial)
      {
        slaveChan101.Type = WINIO_TYPE.RS232;
        slaveChan101.Win232comPortName = "COM4";
        slaveChan101.Win232baudRate = "9600";
        slaveChan101.Win232numDataBits = RS232_DATA_BITS.BITS_8;
        slaveChan101.Win232numStopBits = RS232_STOP_BITS.BITS_1;
        slaveChan101.Win232parity = RS232_PARITY.EVEN;
        slaveChan101.Win232portMode = RS232_PORT_MODE.NONE;
      }
      else
      {
        slaveChan101.Type = WINIO_TYPE.TCP;
        slaveChan101.Name = ".NET I101 Slave";  /* name displayed in analyzer window */
        slaveChan101.WinTCPipAddress = "127.0.0.1";
        slaveChan101.WinTCPipPort = 2404;
        slaveChan101.WinTCPmode = TCP_MODE.SERVER;
      }
    
      slaveChan101.OpenChannel();
  
      slaveSesn101 = new S101Session(slaveChan101);
      slaveSesn101.OpenSession();

      S14Database.InitEvent += new S14Database.InitDelegate(SlaveInitEvent);

      // register our own control operation events
      S14Database.UseSimControlDatabase = false;

      // register our own monitor data and other database events
      S14Database.UseSimDatabase = false;

      slaveSctr101 = new S101Sector(slaveSesn101);

      // Uncommenting these would allow the xxxGetValueFlagsTimeEvent methods 
      // above to be called when a CRDNA was received
      //slaveSctr101.ReadTimeFormat = IEC_TIME_FORMAT.TIME_56;
      //slaveSctr101.ReadMsrndTimeFormat = IEC_TIME_FORMAT.TIME_56;

      slaveSctr101.OpenSector();

      slaveDB = (S14Database)slaveSctr101.SimDatabase; 
      
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
