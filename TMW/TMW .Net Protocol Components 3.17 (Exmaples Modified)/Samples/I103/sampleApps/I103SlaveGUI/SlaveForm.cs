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
using TMW.SCL.IEC60870_5.I103;
using TMW.SCL.IEC60870_5.I103.Slave;

namespace I103slaveGUI
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
    private S103SimDatabase db;

    private S103Session slaveSesn103;
    private S103Sector slaveSctr103;
    private FT12Channel slaveChan103;

    private bool pauseAnalyzer;

    public SlaveForm()
    {
      TMWApplicationBuilder applBuilder = new TMWApplicationBuilder();

      // This causes application to process received data and timers.
      pAppl = TMWApplicationBuilder.getAppl();
      pAppl.EnableEventProcessor = true;
      pAppl.InitEventProcessor();

      // This enables a Forms timer to process protocol data output.
      // For non Forms applications see I103SlaveDatabaseEvents example for getting protocol data.
      protocolBuffer = TMWApplicationBuilder.getProtocolBuffer();
      protocolBuffer.ProtocolDataReadyEvent += new ProtocolBuffer.ProtocolDataReadyEventDelegate(ProtocolEvent);
      protocolBuffer.EnableCheckForDataTimer = true;

      InitializeComponent();

      slaveChan103 = new FT12Channel(TMW_PROTOCOL.I103, TMW_CHANNEL_OR_SESSION_TYPE.SLAVE);
      slaveChan103.ChannelConnectStateEvent += new TMWChannel.ChannelConnectStateEventDelegate(slaveChan_ChannelConnectStateEvent);

      // Even though IEC 60870-5-103 would normally be over serial link, use TCP since it makes Demo more friendly */
      bool bSerial = false;
      if (bSerial)
      {
        slaveChan103.Type = WINIO_TYPE.RS232;
        slaveChan103.Win232comPortName = "COM4";
        slaveChan103.Win232baudRate = "9600";
        slaveChan103.Win232numDataBits = RS232_DATA_BITS.BITS_8;
        slaveChan103.Win232numStopBits = RS232_STOP_BITS.BITS_1;
        slaveChan103.Win232parity = RS232_PARITY.EVEN;
        slaveChan103.Win232portMode = RS232_PORT_MODE.NONE;
      }
      else
      {
        slaveChan103.Type = WINIO_TYPE.TCP;
        slaveChan103.WinTCPipAddress = "127.0.0.1";
        slaveChan103.WinTCPipPort = 2404;
        slaveChan103.WinTCPmode = TCP_MODE.SERVER;
      } 
 
      slaveChan103.Name = ".NET I103 Slave";  /* name displayed in analyzer window */

      // Register to receive channel statistics
      //slaveChan103.ChannelStatisticsEvent += new TMWChannel.ChannelStatisticsEventDelegate(slaveChan_ChannelStatisticsEvent);
      slaveChan103.OpenChannel();

      slaveSesn103 = new S103Session(slaveChan103);
      slaveSesn103.SessionOnlineStateEvent += new TMWSession.SessionOnlineStateEventDelegate(slaveSesn_SessionOnlineStateEvent);

      // Register to receive session statistics
      //slaveSesn103.SessionStatisticsEvent += new TMWSession.SessionStatisticsEventDelegate(slaveSesn_SessionStatisticsEvent);

      // Register to process Private Custom ASDUs that the library does not implement
      // This is very rarely required.  
      //slaveSesn103.ProcessCustomASDUReqEvent += new S103Session.ProcessCustomASDUReqDelegate(slaveSesn103_ProcessCustomASDUReqEvent);
      //slaveSesn103.BuildCustomASDURespEvent += new S103Session.BuildCustomASDURespDelegate(slaveSesn103_BuildCustomASDURespEvent);
      
      slaveSesn103.OpenSession();

      slaveSctr103 = new S103Sector(slaveSesn103);
      slaveSctr103.CyclicPeriod = 30000; // 30 seconds

      // Register to receive sector statistics
      //slaveSctr103.SectorStatisticsEvent += new TMWSector.SectorStatisticsEventDelegate(slaveSctr_SectorStatisticsEvent);
      slaveSctr103.OpenSector();

      db = (S103SimDatabase)slaveSctr103.SimDatabase;

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
        if (simPoint is S103SimDpi)
        {
          (simPoint.Tag as CheckBox).Checked = (simPoint as S103SimDpi).Value;
        }

        else if (simPoint is S103SimMsrnd)
        {
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

    private  System.Windows.Forms.CheckBox[] DPIs;
    private System.Windows.Forms.TextBox[] MSRNDs;

    private void customizeDatabase()
    {
      Byte i;

      DPIs = new System.Windows.Forms.CheckBox[3] { dpi1, dpi2, dpi3 };
      MSRNDs = new System.Windows.Forms.TextBox[3] { msrnd1, msrnd2, msrnd3 };

      // This Clear is not required here since the database is empty,
      // but this would delete all points from the database.
      db.Clear();

      TMWSimPoint pt;
      for (i = 0; i < 3; i++)
      {
        // Response Mode should be either 0, GI, or CYCLIC, but cannot be both GI AND CYCLIC
        pt = db.AddDpiPoint(128, (Byte)(i + 22), Defs.RESPONSE_MODE.GI, 0);
        pt.Tag = DPIs[i];
        DPIs[i].Tag = pt;

        // Simulated database creates general command points for function code 128, 
        // information numbers 16-19 and 23-26 as shown in the 103 spec
        // Add general command point for first dpi as an example
        // A general command point is required if ASDU Type 20 General Command is to be supported for this point.
        if(i == 0)
          db.AddGnrlCmdPoint(128, 22); 

        pt = db.AddMsrndPoint(160, (Byte)(i + 144), Defs.RESPONSE_MODE.CYCLIC, 0, 0);
        pt.Tag = MSRNDs[i];
        MSRNDs[i].Tag = pt;
      }
    }

    private void DPI_CheckChanged(object sender, EventArgs e)
    {
      S103SimDpi pt = ((sender as Control).Tag) as S103SimDpi;
      pt.Value = (sender as CheckBox).Checked;
      pt.AddEvent();
    }

    private void MSRND_TextChanged(object sender, EventArgs e)
    {
      S103SimMsrnd pt = ((sender as Control).Tag) as S103SimMsrnd;
      try
      {
        pt.Value = Convert.ToUInt16((sender as TextBox).Text);
      }
      catch(Exception except)
      {
        MessageBox.Show("invalid value, must be less than 65536:  " + except.Message);
      }
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

    // The following two methods are used for Custom ASDU processing and responses
    // This is very rarely required.
    //bool responseToSend;
    //bool slaveSesn103_ProcessCustomASDUReqEvent(S103Session s103Session, S103Sector s103Sector, byte typeId, byte[] rcvData)
    //{
    //  if (typeId == 99)
    //  {
    //    responseToSend = true;
    //    return true;
    //  }
    //  return false;
    //}

    //bool slaveSesn103_BuildCustomASDURespEvent(S103Session s103Session, S103Sector s103Sector, bool buildResponse)
    //{
    //  if (responseToSend)
    //  {
    //    if (buildResponse)
    //    {
    //      Byte[] dataArray = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };
    //      s103Sector.SendCustomASDUResponse(false, 99, 1, 5, dataArray);
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
      this.protocolBuffer.SaveAsText("103SlaveGUI.log", "create", "begin", "end");
    }

  }
}