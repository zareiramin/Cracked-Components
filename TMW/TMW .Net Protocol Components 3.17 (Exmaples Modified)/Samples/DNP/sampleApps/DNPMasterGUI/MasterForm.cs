using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using TMW;
using TMW.SCL;
using TMW.SCL.ProtocolAnalyzer;

using TMW.SCL.DNP;
using TMW.SCL.DNP.Master;

namespace DNPmasterGUI
{
  public partial class MasterForm : Form
  {
    private const int WM_VSCROLL = 0x115;
    private const int SB_BOTTOM = 7;
    private int _OldEventMask = 0;
    private const int WM_SETREDRAW = 0x000B;
    private const int EM_SETEVENTMASK = 0x0431;

    [DllImport("user32", CharSet = CharSet.Auto)]
    private static extern int SendMessage(HandleRef hWnd, int msg, int wParam, int lParam);

    static TMWApplication pAppl;

    private ProtocolBuffer protocolBuffer;
    private MDNPSimDatabase db;
    private MDNPSession masterSesn;
    private DNPChannel masterChan;
    private bool pauseAnalyzer;

    // Timer values
    private decimal integrityPollInterval;
    private decimal integrityPollCount;
    private decimal eventPollInterval;
    private decimal eventPollCount;

    public MasterForm()
    {
        InitializeComponent();


    }

    private delegate void UpdateStateDelegate(bool channel, bool state);
    private void UpdateState(bool channel, bool state)
    {
      if (InvokeRequired)
      {
        BeginInvoke(new UpdateStateDelegate(UpdateState), new object[] { channel, state });
      }
      else
      {
        if (channel)
        {
          if (state)
          {
            Connected_LB.Text = "Connected";
            Connected_LB.ForeColor = Color.Black;
          }
          else
          {
            Connected_LB.Text = "Disconnected";
            Connected_LB.ForeColor = Color.Crimson;
          }
        }
        else
        {
          if (state)
          {
            Online_LB.Text = "Online";
            Online_LB.ForeColor = Color.Black;
          }
          else
          {
            Online_LB.Text = "Offline";
            Online_LB.ForeColor = Color.Crimson;
          }
        }
      }
    }

    void masterChan_ChannelConnectStateEvent(TMWChannel channel, bool state)
    {
      // state == true indicates channel is now connected
      UpdateState(true, state);
    }

    void masterSesn_SessionOnlineStateEvent(TMWSession session, bool state)
    {
      // state == true indicates session is now online 
      UpdateState(false, state);
    }

    void masterChan_ChannelStatisticsEvent(TMWChannel channel, TMWChannelStatData statData)
    {
      if (statData.StatType == TMWChannelStatData.STAT_TYPE.ERROR)
        protocolBuffer.Insert("CHANNEL STATISTIC: " + statData.StatType.ToString() + " " + statData.ErrorCode.ToString() + "\n");
      else
        protocolBuffer.Insert("CHANNEL STATISTIC: " + statData.StatType.ToString() + "\n");
    }

    void masterSesn_SessionStatisticsEvent(TMWSession session, TMWSessionStatData statData)
    {
      protocolBuffer.Insert("SESSION STATISTIC: " + statData.StatType.ToString() + " " + statData.TypeId.ToString() + " " + statData.PointIndex.ToString() + "\n");
    }

    private void customizeDatabase()
    {
      ushort i;

      // This Clear is not required here since the database is empty,
      // but this would delete all points from the database.
      db.Clear();

      /* Add 3 of each of the following types:
       *  Binary Input
       *    Value = False, Flags = onLine, class = 1
       *  Binary Output
       *    Value = False, Flags = onLine, class = 3, ControlMask = 0x3ff (allow all control operations)
       *  Analog Input
       *    Value = 0, Flags = 0, classMask = 2, Deadband = 5
       *  Analog Output
       *    Value = 0, Flags = onLine; classMask = 3
       *  Binary Counter
       *    Value = 0, Flags = onLIne, classMask = 2, frozenClassMask = 2
       */
      for (i = 0; i < 3; i++)
      {
        db.AddBinIn(i);
        db.AddBinOut(i);
        db.AddAnlgIn(i);
        db.AddAnlgOut(i);
        db.AddBinCntr(i);
      }

      /* Don't add any of the following data types:
       *   Double Bit Input
       *   String
       *   Vterm
       *   etc
       */
    }

    private void updateBinaryInput(TMWSimPoint simPoint)
    {
      string strVal = (simPoint as MDNPSimBinIn).Value ? "On" : "Off";
      Color textColor = (simPoint as MDNPSimBinIn).Value ? Color.ForestGreen : Color.Red;

      switch (simPoint.PointNumber)
      {
        case 0:
          BinIn0.Text = strVal;
          BinIn0.ForeColor = textColor;
          break;
        case 1:
          BinIn1.Text = strVal;
          BinIn1.ForeColor = textColor;
          break;
        case 2:
          BinIn2.Text = strVal;
          BinIn2.ForeColor = textColor;
          break;
        default:
          //protocolBuffer.Insert("Received update for unexpected binary input point: " + simPoint.PointNumber.ToString());
          break;
      }
    }

    private void updateBinaryOutput(TMWSimPoint simPoint)
    {
      string strVal = (simPoint as MDNPSimBinOut).Value ? "On" : "Off";
      Color textColor = (simPoint as MDNPSimBinOut).Value ? Color.ForestGreen : Color.Red;

      switch (simPoint.PointNumber)
      {
        case 0:
          BinOut0Feedback.Text = strVal;
          BinOut0Feedback.ForeColor = textColor;
          break;
        case 1:
          BinOut1Feedback.Text = strVal;
          BinOut1Feedback.ForeColor = textColor;
          break;
        case 2:
          BinOut2Feedback.Text = strVal;
          BinOut2Feedback.ForeColor = textColor;
          break;
        default:
          //protocolBuffer.Insert("Received update for unexpected binary output point: " + simPoint.PointNumber.ToString());
          break;
      }
    }

    private void updateBinCntr(TMWSimPoint simPoint)
    {
      switch (simPoint.PointNumber)
      {
        case 0:
          BinCntr0.Text = (simPoint as MDNPSimBinCntr).Value.ToString();
          break;
        case 1:
          BinCntr1.Text = (simPoint as MDNPSimBinCntr).Value.ToString();
          break;
        case 2:
          BinCntr2.Text = (simPoint as MDNPSimBinCntr).Value.ToString();
          break;
        default:
          //protocolBuffer.Insert("Received update for unexpected analog input point: " + simPoint.PointNumber.ToString());
          break;
      }
    }

    private void updateAnalogInput(TMWSimPoint simPoint)
    {
      switch (simPoint.PointNumber)
      {
        case 0:
          AnlgIn0.Text = (simPoint as MDNPSimAnlgIn).Value.ToString();
          break;
        case 1:
          AnlgIn1.Text = (simPoint as MDNPSimAnlgIn).Value.ToString();
          break;
        case 2:
          AnlgIn2.Text = (simPoint as MDNPSimAnlgIn).Value.ToString();
          break;
        default:
          //protocolBuffer.Insert("Received update for unexpected analog input point: " + simPoint.PointNumber.ToString());
          break;
      }
    }

    private void updateAnalogOutput(TMWSimPoint simPoint)
    {
      switch (simPoint.PointNumber)
      {
        case 0:
          AnlgOut0Feedback.Text = (simPoint as MDNPSimAnlgOut).Value.ToString();
          break;
        case 1:
          AnlgOut1Feedback.Text = (simPoint as MDNPSimAnlgOut).Value.ToString();
          break;
        case 2:
          AnlgOut2Feedback.Text = (simPoint as MDNPSimAnlgOut).Value.ToString();
          break;
        default:
          //protocolBuffer.Insert("Received update for unexpected analog output point: " + simPoint.PointNumber.ToString());
          break;
      }
    }


    private delegate void UpdatePointDelegate(TMWSimPoint simPoint);
    private void UpdateDBEvent(TMWSimPoint simPoint)
    {
      if (InvokeRequired)
        BeginInvoke(new UpdatePointDelegate(UpdateDBEvent), new object[] { simPoint });
      else
      {
        switch (simPoint.PointType)
        {
          case 1:
            // Binary Input
            updateBinaryInput(simPoint);
            break;
          case 10:
            // Binary Output (CROB)
            updateBinaryOutput(simPoint);
            break;
          case 20:
            // Binary Counters
            updateBinCntr(simPoint);
            break;
          case 30:
            // Analog Inputs
            updateAnalogInput(simPoint);
            break;
          case 40:
            // Analog Output
            updateAnalogOutput(simPoint);
            break;
          default:
            //protocolBuffer.Insert("Unknown point type in database update routine");
            break;
        }
      }
    }

    private void IntegrityPollTimer_Tick(object sender, EventArgs e)
    {
      if (integrityPollCount++ < integrityPollInterval)
      {
        IntegrityProgressBar.Increment(1);
      }
      else
      {
        // time to do a poll
        Integrity_Click( sender, e);

        integrityPollCount = 0;
        IntegrityProgressBar.Value = 0;
        IntegrityProgressBar.Maximum = (int)integrityPollInterval;
        IntegrityPollTimer.Start();
      }
    }

    private void EventPollTimer_Tick(object sender, EventArgs e)
    {
      if (eventPollCount++ < eventPollInterval)
      {
        EventProgressBar.Increment(1);
      }
      else
      {
        // time to do a poll
        Event_Click(sender, e);

        eventPollCount = 0;
        EventProgressBar.Value = 0;
        EventProgressBar.Maximum = (int)eventPollInterval;
      }
    }

    private void Integrity_Click(object sender, EventArgs e)
    {
      protocolBuffer.Insert("\nRequested Integrity Poll\n");
      MDNPRequest request = new MDNPRequest(masterSesn);

      // You can put something meaningful here to use when the response callback event occurs
      request.Tag = this;

      // Register to receive responses or timeout from request
      request.RequestEvent += new MDNPRequest.RequestEventDelegate(IntegrityRequestEvent);
      request.IntegrityPoll(true);
    }

    // This will be called each time a response is received from the integrity request or on timeout.
    void IntegrityRequestEvent(MDNPRequest request, MDNPResponseParser response)
    {
      // Session property indicates what session this request was sent on if you want to use it.
      MDNPSession session = request.Session;

      // You can look at the Tag if you put something meaningful there.
      if (request.Tag != this)
        protocolBuffer.Insert("\nTag did not match, this will not happen.\n");

      if (response.Last)
      {
        // this is the end of the integrity poll
        if (response.Status == DNPChannel.RESPONSE_STATUS.SUCCESS)
        {
          // This means the request was successful
        }
      }
    }

    private void Event_Click(object sender, EventArgs e)
    {
      protocolBuffer.Insert("\nRequested Event Poll\n");

      MDNPRequest request = new MDNPRequest(masterSesn);
      request.ReadClass(MDNPRequest.DNP_QUALIFIER.Q_ALL_POINTS, 0, false, true, true, true);
    }

    private void AnlgOut2Val_ValueChanged(object sender, EventArgs e)
    {

    }

    private void BinOutOn_Click(object sender, EventArgs e)
    {
      // Determine which point this request is for
      ushort pointNumber = Convert.ToUInt16((sender as Control).Tag);

      // Build and send the request
      CROBInfo crobData = new CROBInfo(pointNumber, CROBInfo.CROB_CTRL.LON, 1, 0, 0);
      CROBInfo[] crobArray = { crobData };

      MDNPSimBinOut point = db.LookupBinOut(pointNumber);
      MDNPRequest request = new MDNPRequest(masterSesn);
      request.BinaryCommand(MDNPRequest.DNP_FUNCTION_CODE.SELECT, true, true, 100, MDNPRequest.DNP_QUALIFIER.Q_8BIT_INDEX, crobArray);
      // request.BinaryOutWrite((byte)DNP_QUALIFIER.Q_8BIT_START_STOP, pointNumber, pointNumber, val);
    }

    private void BinOutOff_Click(object sender, EventArgs e)
    {
      // Determine which point this request is for
      ushort pointNumber = Convert.ToUInt16((sender as Control).Tag);

      // Build and send the request
      CROBInfo crobData = new CROBInfo(pointNumber, CROBInfo.CROB_CTRL.LOFF, 1, 0, 0);
      CROBInfo[] crobArray = { crobData };

      MDNPSimBinOut point = db.LookupBinOut(pointNumber);
      MDNPRequest request = new MDNPRequest(masterSesn);
      request.BinaryCommand(MDNPRequest.DNP_FUNCTION_CODE.SELECT, true, true, 100, MDNPRequest.DNP_QUALIFIER.Q_8BIT_INDEX, crobArray);
      // request.BinaryOutWrite((byte)DNP_QUALIFIER.Q_8BIT_START_STOP, pointNumber, pointNumber, val);
    }

    private void AnlgOutSend_Click(object sender, EventArgs e)
    {
      double val;
      // Determine which point this request is for
      ushort pointNumber = Convert.ToUInt16((sender as Control).Tag);

      // Get the value to write for this point
      switch (pointNumber)
      {
        case 0:
          val = (double)AnlgOut0Val.Value;
          break;
        case 1:
          val = (double)AnlgOut1Val.Value;
          break;
        case 2:
          val = (double)AnlgOut2Val.Value;
          break;
        default:
          val = 0;
          protocolBuffer.Insert("Unexected analog output command for undefined point: " + pointNumber.ToString());
          break;
      }

      // Build and send the request
      AnalogInfo anlgData = new AnalogInfo(pointNumber, val);
      AnalogInfo[] anlgArray = { anlgData };

      MDNPSimAnlgOut point = db.LookupAnlgOut(pointNumber);
      MDNPRequest request = new MDNPRequest(masterSesn);
      request.AnalogCommand(MDNPRequest.DNP_FUNCTION_CODE.SELECT, true, true, 100, MDNPRequest.DNP_QUALIFIER.Q_8BIT_INDEX, 2, anlgArray);
    }

    private void IntegrityInterval_ValueChanged(object sender, EventArgs e)
    {
      IntegrityPollTimer.Stop();
      integrityPollInterval = (IntegrityIntervalHr.Value * 3600) + (IntegrityIntervalMin.Value * 60) + (IntegrityIntervalSec.Value);
      IntegrityProgressBar.Maximum = (int)integrityPollInterval;

      if (integrityPollInterval == 0)
      {
        IntegrityEnable.Checked = false;
      }

      if (IntegrityEnable.Checked)
      {
        IntegrityPollTimer.Start();
      }
      else
      {
        // if unchecked, reset the progress bar and current count
        IntegrityProgressBar.Value = 0;
        integrityPollCount = 0;
      }
    }

    private void EventInterval_ValueChanged(object sender, EventArgs e)
    {
      EventPollTimer.Stop();
      eventPollInterval = (EventIntervalHr.Value * 3600) + (EventIntervalMin.Value * 60) + (EventIntervalSec.Value);
      EventProgressBar.Maximum = (int)eventPollInterval;

      if (eventPollInterval == 0)
      {
        EventEnable.Checked = false;
      }

      if (EventEnable.Checked)
      {
        EventPollTimer.Start();
      }
      else
      {
        // if unchecked, reset the progress bar and current count to 0
        EventProgressBar.Value = 0;
        eventPollCount = 0;
      }
    }

// Protocol Analyzer Display Code
#region ProtocolAnalyzerCode
    private void ScrollToBottom()
    {
      SendMessage(new HandleRef(protocolAnalyzer, protocolAnalyzer.Handle), WM_VSCROLL, SB_BOTTOM, 0);
    }

    private void BeginUpdate()
    {
      // Prevent the control from raising any events
      _OldEventMask = SendMessage(new HandleRef(protocolAnalyzer, protocolAnalyzer.Handle), EM_SETEVENTMASK, 0, 0);

      // Prevent the control from redrawing itself
      SendMessage(new HandleRef(protocolAnalyzer, protocolAnalyzer.Handle), WM_SETREDRAW, 0, 0);
    }

    private void EndUpdate()
    {
      // Allow the control to redraw itself
      SendMessage(new HandleRef(protocolAnalyzer, protocolAnalyzer.Handle), WM_SETREDRAW, 1, 0);

      // Allow the control to raise event messages
      SendMessage(new HandleRef(protocolAnalyzer, protocolAnalyzer.Handle), EM_SETEVENTMASK, 0, _OldEventMask);
    }

    private void RemoveTopLines(int numLines)
    {
      int lastLine = protocolAnalyzer.Lines.GetLength(0) - 1;
      if (numLines < 1)
      {
        return;
      }
      else if (numLines > lastLine)
      {
        numLines = lastLine;
      }

      int startChar = protocolAnalyzer.GetFirstCharIndexFromLine(0);
      int endChar = protocolAnalyzer.GetFirstCharIndexFromLine(numLines);

      bool b = protocolAnalyzer.ReadOnly;
      protocolAnalyzer.ReadOnly = false;
      protocolAnalyzer.Select(startChar, endChar - startChar);
      protocolAnalyzer.SelectedRtf = "";
      protocolAnalyzer.ReadOnly = b;
    }

    private void ProtocolEvent(ProtocolBuffer buf)
    {
      if (!pauseAnalyzer)
      {
        buf.Lock();
        for (int i = buf.LastProvidedIndex; i < buf.LastAddedIndex; i++)
        {
          // Don't display physical and target layer trace
          //if ((pdo.SourceIDasUint & ((UInt32)SCLDIAG_ID.PHYS | (UInt32)SCLDIAG_ID.TARGET)) == 0)  
          if (protocolBuffer.getPdoAtIndex(i).Time != null)
            protocolAnalyzer.AppendText(protocolBuffer.getPdoAtIndex(i).Time.ToLogString());

          protocolAnalyzer.AppendText(protocolBuffer.getPdoAtIndex(i).ProtocolText);
          SendMessage(new HandleRef(protocolAnalyzer, protocolAnalyzer.Handle), WM_VSCROLL, SB_BOTTOM, 0);
        }
        buf.UnLock();

        // remove lines from the text box
        if (protocolAnalyzer.Lines.Length > 1000)
        {
          BeginUpdate();
          RemoveTopLines(100);
          ScrollToBottom();
          EndUpdate();
        }
      }
    }

    private void protocolAnalyzer_MouseDown(object sender, MouseEventArgs e)
    {
      if (e.Clicks == 2)
      {
        // double click toggles pausing
        if (pauseAnalyzer)
        {
          // it's paused, so unpause it
          pauseAnalyzer = false;
          protocolAnalyzer.BackColor = Color.Gainsboro;
        }
        else
        {
          // it's not paused, so pause it
          pauseAnalyzer = true;
          protocolAnalyzer.BackColor = Color.MistyRose;
        }
      }
    }

    // This will save the protocol log to a file
    private void SaveLog_Click(object sender, EventArgs e)
    {
      this.protocolBuffer.UpdateDiagIdMask(SCLDIAG_ID.ALL_BUT_TARGET, true);
      this.protocolBuffer.UpdateDiagIdMask(SCLDIAG_ID.PHYS, false);
      this.protocolBuffer.SaveAsText("DNPMasterGUI.log", "create", "begin", "end");
    }
#endregion ProtocolAnalyzerCode

    private void button2_Click(object sender, EventArgs e)
    {
        // Change this to true to connect over COM ports
        bool bSerialMode = false;

        TMWApplicationBuilder applBuilder = new TMWApplicationBuilder();

        // This causes application to process received data and timers.
        pAppl = TMWApplicationBuilder.getAppl();
        pAppl.EnableEventProcessor = true;
        pAppl.InitEventProcessor();

        // This enables a Forms timer to process protocol data output.
        // For non Forms applications see DNPMasterDatabaseEvents example for getting protocol data.
        protocolBuffer = TMWApplicationBuilder.getProtocolBuffer();
        protocolBuffer.ProtocolDataReadyEvent += new ProtocolBuffer.ProtocolDataReadyEventDelegate(ProtocolEvent);
        protocolBuffer.EnableCheckForDataTimer = true;


        masterChan = new DNPChannel(TMW_CHANNEL_OR_SESSION_TYPE.MASTER);
        masterChan.ChannelConnectStateEvent += new TMWChannel.ChannelConnectStateEventDelegate(masterChan_ChannelConnectStateEvent);

        if (bSerialMode)
        {
            masterChan.Type = WINIO_TYPE.RS232;
            masterChan.Win232comPortName = "COM3";
            masterChan.Win232baudRate = "9600";
            masterChan.Win232numDataBits = RS232_DATA_BITS.BITS_8;
            masterChan.Win232numStopBits = RS232_STOP_BITS.BITS_1;
            masterChan.Win232portMode = RS232_PORT_MODE.NONE;
        }
        else
        {
            masterChan.Type = WINIO_TYPE.TCP; // equivalent to UDP_TCP
            masterChan.Name = ".NET DNP Master";  /* name displayed in analyzer window */
            masterChan.WinTCPipAddress = txtIP.Text;
            masterChan.WinTCPipPort = (ushort) numericUpDownPort.Value;
            masterChan.WinTCPmode = TCP_MODE.CLIENT;

            // UDP ONLY, no TCP connection.
            // Using UDP only is not recommended, but some customers require it
            //masterChan.LocalUDPPort = DNPChannel.UDP_PORT_ANY; // or choose a specific port like 20001.
            //masterChan.DestUDPPort = 20000;
            //masterChan.WinTCPmode = TCP_MODE.UDP;
            //masterChan.NetworkType = DNPChannel.NETWORK_TYPE.UDP_ONLY;
        }

        // Register to receive channel statistics
        //masterChan.ChannelStatisticsEvent += new TMWChannel.ChannelStatisticsEventDelegate(masterChan_ChannelStatisticsEvent);
        masterChan.OpenChannel();

        masterSesn = new MDNPSession(masterChan);

        // If using TCP the DNP Spec requires keep alives to be configured in order to detect disconnects.
        if (!bSerialMode)
            masterSesn.LinkStatusPeriod = 30000;

        // For examples of using DNP Secure Authentication, see the DNPMasterSA example
        masterSesn.AuthenticationEnabled = false;

        // Register to receive online/offline events 
        masterSesn.SessionOnlineStateEvent += new TMWSession.SessionOnlineStateEventDelegate(masterSesn_SessionOnlineStateEvent);

        // Register to receive session statistics
        //masterSesn.SessionStatisticsEvent += new TMWSession.SessionStatisticsEventDelegate(masterSesn_SessionStatisticsEvent);
        masterSesn.OpenSession();

        db = (MDNPSimDatabase)masterSesn.SimDatabase;
        // Register to receive notification of database changes
        db.UpdateDBEvent += new TMWSimDataBase.UpdateDBEventDelegate(UpdateDBEvent);

        customizeDatabase();

        // Set up integrity poll timer
        integrityPollCount = 0;
        integrityPollInterval = 3600;  // Once per hour
        IntegrityProgressBar.Value = 0;
        IntegrityProgressBar.Maximum = (int)integrityPollInterval;
        IntegrityPollTimer.Start();

        // Set up event poll timer
        eventPollCount = 0;
        eventPollInterval = 5;
        EventProgressBar.Value = 0;
        EventProgressBar.Maximum = (int)eventPollInterval;
        EventPollTimer.Start();

    }

  }
}