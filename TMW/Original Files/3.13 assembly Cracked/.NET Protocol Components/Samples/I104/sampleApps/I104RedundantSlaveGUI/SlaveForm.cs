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

namespace I104RedundantSlaveGUI
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
    private I104RedundancyGroup slaveRdcyGroup104;
    private I104RedundantChannel slaveRdnt1Chan104;
    private I104RedundantChannel slaveRdnt2Chan104;
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
          (simPoint.Tag as CheckBox).Checked = (simPoint as S14SimMspna).Value;
        }
        if (simPoint is S14SimMmenc)
        {
          (simPoint.Tag as NumericUpDown).Value = (decimal)(simPoint as S14SimMmenc).Value;
        }
        else
        {
          protocolBuffer.Insert("Unknown point type in database update routine");
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

        uint pacncIOA = 1200 + i; //pacnaIOA
        groupMask = TMW_GROUP_MASK.MASK_CYCLIC;
        pt = db.AddMmencPoint(700 + i, pacncIOA, pmencIOAs, groupMask, 0, 0);
        pt.Tag = AnalogInputs[i];
        AnalogInputs[i].Tag = pt;

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
      pt.AddEvent();
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
      slaveRdcyGroup104 = new I104RedundancyGroup(TMW_CHANNEL_OR_SESSION_TYPE.SLAVE);
      slaveRdcyGroup104.SetName("rdcyGroup");

      if (!slaveRdcyGroup104.InitGroup())
        // failed 
        return;
       
      slaveRdnt1Chan104 = new I104RedundantChannel(TMW_CHANNEL_OR_SESSION_TYPE.SLAVE);
      slaveRdnt1Chan104.ChannelConnectStateEvent += new TMWChannel.ChannelConnectStateEventDelegate(slaveChan_ChannelConnectStateEvent);

      slaveRdnt1Chan104.Type = WINIO_TYPE.TCP;
      slaveRdnt1Chan104.Name = "S104_1";  /* name displayed in analyzer window */
      slaveRdnt1Chan104.WinTCPipAddress = "127.0.0.1";
      // If you want to use the same IP Port as the second channel, you must set
      // the following to prevent disconnecting when the second connect request arrives.
      //slaveRdnt1Chan104.DisconnectOnNewSyn = false;
      slaveRdnt1Chan104.WinTCPipPort = 2404;
      slaveRdnt1Chan104.WinTCPmode = TCP_MODE.SERVER;

      if (!slaveRdnt1Chan104.OpenChannel(slaveRdcyGroup104))
        //failed
        return;
      
      slaveRdnt2Chan104 = new I104RedundantChannel(TMW_CHANNEL_OR_SESSION_TYPE.SLAVE);
      slaveRdnt2Chan104.ChannelConnectStateEvent += new TMWChannel.ChannelConnectStateEventDelegate(slaveChan_ChannelConnectStateEvent);

      slaveRdnt2Chan104.Type = WINIO_TYPE.TCP;
      slaveRdnt2Chan104.Name = "S104_2";  /* name displayed in analyzer window */
      slaveRdnt2Chan104.WinTCPipAddress = "127.0.0.1";
      // If you want to use the same IP Port as the first channel, you must set
      // the following to prevent disconnecting when the second connect request arrives.
      //slaveRdnt2Chan104.DisconnectOnNewSyn = false;
      slaveRdnt2Chan104.WinTCPipPort = 2405;
      slaveRdnt2Chan104.WinTCPmode = TCP_MODE.SERVER;

      if(!slaveRdnt2Chan104.OpenChannel(slaveRdcyGroup104))
        //failed
        return;

      slaveSesn104 = new S104Session(slaveRdcyGroup104);
      slaveSesn104.SessionOnlineStateEvent += new TMWSession.SessionOnlineStateEventDelegate(slaveSesn_SessionOnlineStateEvent);

      if (!slaveSesn104.OpenSession())
        //failed
        return;

      slaveSctr104 = new S104Sector(slaveSesn104);

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
      slaveRdnt1Chan104.CloseChannel();
      slaveRdnt2Chan104.CloseChannel();
      slaveRdcyGroup104.DeleteGroup(); 
     
      openPB.Enabled = true;
      closePB.Enabled = false;
    }
    private delegate void UpdateStateDelegate(bool channel, int channelNumber, bool state);
    private void UpdateState(bool channel, int channelNumber, bool state)
    {
      if (InvokeRequired)
      {
        BeginInvoke(new UpdateStateDelegate(UpdateState), new object[] { channel, channelNumber, state });
      }
      else
      {
        if (channel)
        {
          if (channelNumber == 1)
          {
            if (state)
            {
              Connected1_LB.Text = "Connected";
              Connected1_LB.ForeColor = Color.Black;
            }
            else
            {
              Connected1_LB.Text = "Disconnected";
              Connected1_LB.ForeColor = Color.Crimson;
            }
          }
          else
          {
            if (state)
            {
              Connected2_LB.Text = "Connected";
              Connected2_LB.ForeColor = Color.Black;
            }
            else
            {
              Connected2_LB.Text = "Disconnected";
              Connected2_LB.ForeColor = Color.Crimson;
            }
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
      if (channel == slaveRdnt1Chan104)
        UpdateState(true, 1, state);
      else
        UpdateState(true, 2, state);
    }

    void slaveSesn_SessionOnlineStateEvent(TMWSession session, bool state)
    {
      // state == true indicates session is now online 
      UpdateState(false, 0, state);
    }

  }
}