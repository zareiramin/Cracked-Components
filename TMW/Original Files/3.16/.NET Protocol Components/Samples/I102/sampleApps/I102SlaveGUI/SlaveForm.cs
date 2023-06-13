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
using TMW.SCL.IEC60870_5.I102;
using TMW.SCL.IEC60870_5.I102.Slave;

namespace I102slaveGUI
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
    private S102SimDatabase db;

    private S102Session slaveSesn102;
    private S102Sector slaveSctr102;
    private FT12Channel slaveChan102;

    private bool pauseAnalyzer;

    public SlaveForm()
    {
      TMWApplicationBuilder applBuilder = new TMWApplicationBuilder();
      pAppl = TMWApplicationBuilder.getAppl();
      pAppl.EnableEventProcessor = true;
      pAppl.InitEventProcessor();

      protocolBuffer = TMWApplicationBuilder.getProtocolBuffer();
      protocolBuffer.ProtocolDataReadyEvent += new ProtocolBuffer.ProtocolDataReadyEventDelegate(ProtocolEvent);
      protocolBuffer.EnableCheckForDataTimer = true;

      InitializeComponent();

      slaveChan102 = new FT12Channel(TMW_PROTOCOL.I102, TMW_CHANNEL_OR_SESSION_TYPE.SLAVE);
      slaveChan102.ChannelConnectStateEvent += new TMWChannel.ChannelConnectStateEventDelegate(slaveChan_ChannelConnectStateEvent);

      // Even though IEC 60870-5-102 would normally be over serial link, use TCP since it makes Demo more friendly */
      bool bSerial = false;
      if (bSerial)
      {
        slaveChan102.Type = WINIO_TYPE.RS232;
        slaveChan102.Win232comPortName = "COM4";
        slaveChan102.Win232baudRate = "9600";
        slaveChan102.Win232numDataBits = RS232_DATA_BITS.BITS_8;
        slaveChan102.Win232numStopBits = RS232_STOP_BITS.BITS_1;
        slaveChan102.Win232parity = RS232_PARITY.EVEN;
        slaveChan102.Win232portMode = RS232_PORT_MODE.NONE;
      }
      else
      {
        slaveChan102.Type = WINIO_TYPE.TCP;
        slaveChan102.Name = ".NET I101 Slave";  /* name displayed in analyzer window */
        slaveChan102.WinTCPipAddress = "127.0.0.1";
        slaveChan102.WinTCPipPort = 2404;
        slaveChan102.WinTCPmode = TCP_MODE.SERVER;
      } 

      slaveChan102.Name = ".NET I102 Slave";  /* name displayed in analyzer window */
      
      // Register to receive channel statistics
      //slaveChan102.ChannelStatisticsEvent += new TMWChannel.ChannelStatisticsEventDelegate(slaveChan_ChannelStatisticsEvent); 
      slaveChan102.OpenChannel();

      slaveSesn102 = new S102Session(slaveChan102);
      slaveSesn102.SessionOnlineStateEvent += new TMWSession.SessionOnlineStateEventDelegate(slaveSesn_SessionOnlineStateEvent);
      
      // Register to receive session statistics
      //slaveSesn102.SessionStatisticsEvent += new TMWSession.SessionStatisticsEventDelegate(slaveSesn_SessionStatisticsEvent);
      
      // Register to process Private Custom ASDUs that the library does not implement
      // This is very rarely required.  
      //slaveSesn102.ProcessCustomASDUReqEvent += new S102Session.ProcessCustomASDUReqDelegate(slaveSesn102_ProcessCustomASDUReqEvent);
      //slaveSesn102.BuildCustomASDURespEvent += new S102Session.BuildCustomASDURespDelegate(slaveSesn102_BuildCustomASDURespEvent);

      slaveSesn102.OpenSession();

      slaveSctr102 = new S102Sector(slaveSesn102);

      // Register to receive sector statistics
      //slaveSctr102.SectorStatisticsEvent += new TMWSector.SectorStatisticsEventDelegate(slaveSctr_SectorStatisticsEvent);
      
      slaveSctr102.OpenSector();
      db = (S102SimDatabase)slaveSctr102.SimDatabase;

      // Register to receive notification of database changes
      db.UpdateDBEvent += new TMWSimDataBase.UpdateDBEventDelegate(UpdateDBEvent);

      customizeDatabase();
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

    private delegate void UpdatePointDelegate(TMWSimPoint simPoint);
    private void UpdateDBEvent(TMWSimPoint simPoint)
    {
      if (this.InvokeRequired)

        this.BeginInvoke(new UpdatePointDelegate(UpdateDBEvent), new object[] { simPoint });

      else
      {
        if (simPoint is S102SimMspta)
        {
          (simPoint.Tag as CheckBox).Checked = (simPoint as S102SimMspta).Value;
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
    private System.Windows.Forms.Button[] Counters;

    private void customizeDatabase()
    {
      Byte i;

      BinaryInputs = new System.Windows.Forms.CheckBox[3] { BinaryInput0, BinaryInput1, BinaryInput2 };
      Counters = new System.Windows.Forms.Button[3] { Counter0, Counter1, Counter2 };

      // This Clear is not required here since the database is empty,
      // but this would delete all points from the database.
      db.Clear();

      TMWSimPoint pt;
      for (i = 0; i < 3; i++)
      {
        pt = db.AddMsptaPoint(51, i, false, 0);
        pt.Tag = BinaryInputs[i];
        BinaryInputs[i].Tag = pt;

        pt = db.AddMittaPoint(11, i, 0, 0);
        pt.Tag = Counters[i];
        Counters[i].Tag = pt;

      }

    }

    private void BinaryInput_CheckedChanged(object sender, EventArgs e)
    {
      S102SimMspta pt = ((sender as Control).Tag) as S102SimMspta;
      pt.Value = (sender as CheckBox).Checked;
      pt.AddEvent();
    }

    private void Counter_Click(object sender, EventArgs e)
    {
      S102SimMitta pt = ((sender as Control).Tag) as S102SimMitta;
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
            if ((pdo.SourceIDasUint & ((UInt32)SCLDIAG_ID.PHYS | (UInt32)SCLDIAG_ID.TARGET)) == 0)
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
    // The following two methods are used for Custom ASDU processing and responses
    // This is very rarely required.
    //bool responseToSend;
    //bool slaveSesn102_ProcessCustomASDUReqEvent(S102Session s102Session, S102Sector s102Sector, byte typeId, byte[] rcvData)
    //{
    //  if (typeId == 99)
    //  {
    //    responseToSend = true;
    //    return true;
    //  }
    //  return false;
    //}

    //bool slaveSesn102_BuildCustomASDURespEvent(S102Session s102Session, S102Sector s102Sector, bool buildResponse)
    //{
    //  if (responseToSend)
    //  {
    //    if (buildResponse)
    //    { 
    //      Byte typeId = 99;
    //      Byte vsq = 1;
    //      Byte cot = 5;
    //      UInt16 asduAddress = 3;
    //      Byte recordAddress = 10;
    //      Byte[] dataArray = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };
    //      s102Sector.SendCustomASDUResponse(false, typeId, vsq, cot, asduAddress, recordAddress, dataArray); 
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
      this.protocolBuffer.SaveAsText("102SlaveGUI.log", "create", "begin", "end");
    }
  }
}