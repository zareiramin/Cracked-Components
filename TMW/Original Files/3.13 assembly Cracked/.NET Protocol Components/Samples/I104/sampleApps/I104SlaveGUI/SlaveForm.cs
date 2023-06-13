using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using TMW.SCL;
using TMW.SCL.ProtocolAnalyzer;
using TMW.SCL.IEC60870_5;
using TMW.SCL.IEC60870_5.I14;
using TMW.SCL.IEC60870_5.I14.Slave;
using TMW.SCL.IEC60870_5.I104;
using TMW.SCL.IEC60870_5.I104.Slave;

namespace I104slaveGUI
{
  public partial class SlaveForm : Form
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
    private S14SimDatabase db;

    private S104Session slaveSesn104;
    private S104Sector slaveSctr104;
    private I104Channel slaveChan104;
    private bool pauseAnalyzer;

    public SlaveForm()
    {
      TMWApplicationBuilder applBuilder = new TMWApplicationBuilder();
      pAppl = TMWApplicationBuilder.getAppl();
      pAppl.InitEventProcessor();
      pAppl.EnableEventProcessor = true;

      protocolBuffer = TMWApplicationBuilder.getProtocolBuffer();
      protocolBuffer.ProtocolDataReadyEvent += new ProtocolBuffer.ProtocolDataReadyEventDelegate(ProtocolEvent);
      protocolBuffer.EnableCheckForDataTimer = true;

      InitializeComponent();

      closePB.Enabled = false;

    }

    private delegate void UpdatePointDelegate(TMWSimPoint simPoint);
    private void UpdateDBEvent(TMWSimPoint simPoint)
    {
      if (this.InvokeRequired)

        this.BeginInvoke(new UpdatePointDelegate(UpdateDBEvent), new object[] { simPoint });

      else
      {
        if (simPoint is S14SimMspna)
        {
          if (simPoint.Tag != null)
            (simPoint.Tag as CheckBox).Checked = (simPoint as S14SimMspna).Value;
        }
        if (simPoint is S14SimMmenc)
        {
          if (simPoint.Tag != null)
            (simPoint.Tag as NumericUpDown).Value = (decimal)(simPoint as S14SimMmenc).Value;
        }
        else
        {
          //protocolBuffer.Insert("Unhandled point type in database update routine");
        }
      }

    }

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

    private  System.Windows.Forms.CheckBox[] BinaryInputs;
    private System.Windows.Forms.NumericUpDown[] AnalogInputs;
    private System.Windows.Forms.Button[] Counters;

    private void customizeDatabase()
    {
      uint i;

      BinaryInputs = new System.Windows.Forms.CheckBox[3] { BinaryInput0, BinaryInput1, BinaryInput2 };
      AnalogInputs = new System.Windows.Forms.NumericUpDown[3] { AnalogInput0, AnalogInput1, AnalogInput2 };
      Counters = new System.Windows.Forms.Button[3] { Counter0, Counter1, Counter2 };

      // This Clear is not required here since the database is empty,
      // but this would delete all points from the database.
      db.Clear();

      TMW_GROUP_MASK groupMask;
      uint[] pmencIOAs = new uint[] { 0,0,0,0 };
      TMWSimPoint pt;
      for (i = 0; i < 3; i++)
      {
        groupMask = TMW_GROUP_MASK.GENERAL;
        pt = db.AddMspPoint(100 + i, groupMask, 0, 0);
        pt.Tag = BinaryInputs[i];
        BinaryInputs[i].Tag = pt;


        pmencIOAs[0] = 1100 + (i * 4);      //lowLimitIOA
        pmencIOAs[1] = 1100 + (i * 4) + 1;  //highLimitIOA
        pmencIOAs[2] = 1100 + (i * 4) + 2;  //thresholdIOA
        pmencIOAs[3] = 1100 + (i * 4) + 3;  //smoothingIOA

        S14SimPoint simPoint;
        uint pacncIOA = 1200 + i; //pacnaIOA
        groupMask = TMW_GROUP_MASK.MASK_CYCLIC;
        simPoint = db.AddMmencPoint(700 + i, pacncIOA, pmencIOAs, groupMask, 0, 0);

        if(i == 1)
          simPoint.TimeFormat = (uint)IEC_TIME_FORMAT.TIME_NONE;
        else
          simPoint.TimeFormat = (uint)IEC_TIME_FORMAT.TIME_56;

        simPoint.Tag = AnalogInputs[i];
        AnalogInputs[i].Tag = simPoint;

        groupMask = TMW_GROUP_MASK.GENERAL;
        pt = db.AddMitnaPoint(800 + i, groupMask, 0, 0);
        pt.Tag = Counters[i];
        Counters[i].Tag = pt;

      }

    }

    private void BinaryInput_CheckedChanged(object sender, EventArgs e)
    {
      S14SimMspna pt = ((sender as Control).Tag) as S14SimMspna;
      if (pt == null)
        return;

      pt.Value = (sender as CheckBox).Checked;
      pt.AddEvent();

      /* The following example uses the sector add event method to specify the time and value for an event 
       * instead of using the simple AddEvent method on the point itself.
       * TMWTime timeStamp = new TMWTime(); ;
       * timeStamp.Hour = 12;
       * timeStamp.Minute = 22;
       * timeStamp.Second = 0;
       * timeStamp.Millisecond = 0;
       * timeStamp.Day = 1;
       * timeStamp.Month = 3;
       * timeStamp.Invalid = false;
       * slaveSctr104.MspAddEvent(100, false, 0xff, timeStamp, 0); 
       */
    }

    private void AnalogInput_ValueChanged(object sender, EventArgs e)
    {
      S14SimMmenc pt = ((sender as Control).Tag) as S14SimMmenc;
      if (pt == null)
        return;

      pt.Value = (float)(sender as NumericUpDown).Value;
      //pt.AddEvent();

      TMWTime timeStamp = new TMWTime(); ;
      timeStamp.Hour = 12;
      timeStamp.Minute = 22;
      timeStamp.Second = 0;
      timeStamp.Millisecond = 0;
      timeStamp.Day = 1;
      timeStamp.Month = 3;
      timeStamp.Invalid = false;

      if(pt.IOA==701)
        slaveSctr104.MmencAddEvent(701, (float)(sender as NumericUpDown).Value, pt.Quality.Flags, null);
      else
        slaveSctr104.MmencAddEvent(pt.IOA, (float)(sender as NumericUpDown).Value, pt.Quality.Flags, timeStamp);


    }

    private void Counter_Click(object sender, EventArgs e)
    {
      S14SimMitna pt = ((sender as Control).Tag) as S14SimMitna;
      if (pt == null)
        return;

      pt.Value += 1;
      (sender as Button).Text = pt.Value.ToString();
      pt.AddEvent();
    }

    private void ProtocolEvent(ProtocolBuffer buf)
    {
      if (!pauseAnalyzer)
      {
        buf.Lock();
        for (int i = buf.LastProvidedIndex; i < buf.LastAddedIndex; i++)
        {
          ProtocolDataObject pdo = protocolBuffer.getPdoAtIndex(i);
          if (pdo != null)
          {
            // Don't display physical and target layer trace
            //if ((pdo.SourceIDasUint & ((UInt32)SCLDIAG_ID.PHYS | (UInt32)SCLDIAG_ID.TARGET)) == 0)
            protocolAnalyzer.AppendText(pdo.ProtocolText);
          }
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
          this.protocolAnalyzer.BackColor = Color.Gainsboro;
        }
        else
        {
          // it's not paused, so pause it
          pauseAnalyzer = true;
          this.protocolAnalyzer.BackColor = Color.MistyRose;
        }
      }
    }

    private void SlaveForm_Load(object sender, EventArgs e)
    {

    }

    private void openPB_Click(object sender, EventArgs e)
    {
      slaveChan104 = new I104Channel(TMW_CHANNEL_OR_SESSION_TYPE.SLAVE);
      slaveChan104.ChannelConnectStateEvent += new TMWChannel.ChannelConnectStateEventDelegate(slaveChan_ChannelConnectStateEvent);

      slaveChan104.Type = WINIO_TYPE.TCP;
      slaveChan104.Name = ".NET I104 Slave";  /* name displayed in analyzer window */
      slaveChan104.WinTCPipAddress = "127.0.0.1";
      slaveChan104.WinTCPipPort = 2404;
      slaveChan104.WinTCPmode = TCP_MODE.SERVER;

      // Register to receive channel statistics
      //slaveChan104.ChannelStatisticsEvent += new TMWChannel.ChannelStatisticsEventDelegate(slaveChan_ChannelStatisticsEvent);
      slaveChan104.OpenChannel();

      slaveSesn104 = new S104Session(slaveChan104);
      slaveSesn104.SessionOnlineStateEvent += new TMWSession.SessionOnlineStateEventDelegate(slaveSesn_SessionOnlineStateEvent);

      // Register to process Private Custom ASDUs that the library does not implement
      // This is very rarely required.
      //slaveSesn104.ProcessCustomASDUReqEvent += new S104Session.ProcessCustomASDUReqDelegate(slaveSesn104_ProcessCustomASDUReqEvent);
      //slaveSesn104.BuildCustomASDURespEvent += new S104Session.BuildCustomASDURespDelegate(slaveSesn104_BuildCustomASDURespEvent);

      // Register to receive session statistics
      //slaveSesn104.SessionStatisticsEvent += new TMWSession.SessionStatisticsEventDelegate(slaveSesn_SessionStatisticsEvent);
      slaveSesn104.OpenSession();

      slaveSctr104 = new S104Sector(slaveSesn104);

      slaveSctr104.MmencTimeFormat = IEC_TIME_FORMAT.TIME_56;
      slaveSctr104.CicnaTimeFormat = IEC_TIME_FORMAT.TIME_56;

      // Register to receive sector statistics
      //slaveSctr104.SectorStatisticsEvent += new TMWSector.SectorStatisticsEventDelegate(slaveSctr_SectorStatisticsEvent);
      slaveSctr104.OpenSector();
      db = (S14SimDatabase)slaveSctr104.SimDatabase;

      // Register to receive notification of database changes
      db.UpdateDBEvent += new TMWSimDataBase.UpdateDBEventDelegate(UpdateDBEvent);

      customizeDatabase();
      openPB.Enabled = false;
      closePB.Enabled = true;
    }

    private void closePB_Click(object sender, EventArgs e)
    {
      slaveSctr104.CloseSector();
      slaveSesn104.CloseSession();
      slaveChan104.CloseChannel();
     
      openPB.Enabled = true;
      closePB.Enabled = false;
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

    void slaveChan_ChannelConnectStateEvent(TMWChannel channel, bool state)
    {
      // state == true indicates channel is now connected
      UpdateState(true, state);
    }

    void slaveSesn_SessionOnlineStateEvent(TMWSession session, bool state)
    {
      // state == true indicates session is now online 
      UpdateState(false, state);
    }

    void slaveChan_ChannelStatisticsEvent(TMWChannel channel, TMWChannelStatData statData)
    {
      if (statData.StatType == TMWChannelStatData.STAT_TYPE.ERROR)
        protocolBuffer.Insert("CHANNEL STATISTIC: " + statData.StatType.ToString() + " " + statData.ErrorCode.ToString() + "\n");
      else
        protocolBuffer.Insert("CHANNEL STATISTIC: " + statData.StatType.ToString() + "\n");
    }

    void slaveSesn_SessionStatisticsEvent(TMWSession session, TMWSessionStatData statData)
    {
      protocolBuffer.Insert("SESSION STATISTIC: " + statData.StatType.ToString() + " " + statData.TypeId.ToString() + " " + statData.PointIndex.ToString() + "\n");
    }

    void slaveSctr_SectorStatisticsEvent(TMWSector sector, TMWSectorStatData statData)
    {
      protocolBuffer.Insert("SECTOR STATISTIC: " + statData.StatType.ToString() + " " + statData.TypeId.ToString() + " " + statData.IOA.ToString() + "\n");
    }

    // The following two methods are used for Custom ASDU processing and responses
    // This is very rarely required.
    //bool responseToSend;
    //bool slaveSesn104_ProcessCustomASDUReqEvent(S14Session s14Session, S14Sector s14Sector, byte typeId, byte[] rcvData)
    //{
    //  if (typeId == 99)
    //  {
    //    // Add processing of rcvData here.
    //    responseToSend = true;
    //    return true;
    //  }
    //  return false;
    //}

    //bool slaveSesn104_BuildCustomASDURespEvent(S14Session s14Session, S14Sector s14Sector, bool buildResponse)
    //{
    //  if (responseToSend)
    //  {
    //    if (buildResponse)
    //    {
    //      Boolean dataUnitIdInData = false;
    //      Byte typeId = 99;
    //      Byte vsq = 1;
    //      Byte cot = 5;
    //      Byte[] dataArray = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };
    //      s14Sector.SendCustomASDUResponse(dataUnitIdInData, typeId, vsq, cot, dataArray);
    //      responseToSend = false;
    //    }
    //    return true;
    //  }
    //  return false;
    //}

    // This will save the protocol log to a file
    private void SaveLog_Click(object sender, EventArgs e)
    {
      this.protocolBuffer.UpdateDiagIdMask(SCLDIAG_ID.ALL_BUT_TARGET, true);
      this.protocolBuffer.UpdateDiagIdMask(SCLDIAG_ID.PHYS, false);
      this.protocolBuffer.SaveAsText("104SlaveGUI.log", "create", "begin", "end");
    }
  }
}