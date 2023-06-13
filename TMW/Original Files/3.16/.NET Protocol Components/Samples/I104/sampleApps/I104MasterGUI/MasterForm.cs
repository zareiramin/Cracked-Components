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
using TMW.SCL.IEC60870_5.I14.Master;

using TMW.SCL.IEC60870_5.I104;
using TMW.SCL.IEC60870_5.I104.Master;

namespace I104masterGUI
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
    private M14SimDatabase db;

    private M104Session masterSesn104;
    private M104Sector masterSctr104;
    private I104Channel masterChan104;
    private bool pauseAnalyzer;

    private M14Request customASDURequest = null;

    public MasterForm()
    {
      TMWApplicationBuilder applBuilder = new TMWApplicationBuilder();
      pAppl = TMWApplicationBuilder.getAppl();
      pAppl.EnableEventProcessor = true;
      pAppl.InitEventProcessor();

      protocolBuffer = TMWApplicationBuilder.getProtocolBuffer();
      protocolBuffer.ProtocolDataReadyEvent += new ProtocolBuffer.ProtocolDataReadyEventDelegate(ProtocolEvent);
      protocolBuffer.EnableCheckForDataTimer = true;

      InitializeComponent();

      closePB.Enabled = false;
      integrityPollPB.Enabled = false;
    }

    private delegate void UpdatePointDelegate(TMWSimPoint simPoint);
    private void UpdateDBEvent(TMWSimPoint simPoint)
    {
      if (this.InvokeRequired)

        this.BeginInvoke(new UpdatePointDelegate(UpdateDBEvent), new object[] { simPoint });

      else
      {
        if (simPoint is M14SimMspna)
        {
          if(simPoint.Tag != null)
            (simPoint.Tag as CheckBox).Checked = (simPoint as M14SimMspna).Value;
        }
        if (simPoint is M14SimMmenc)
        {
          if (simPoint.Tag != null)
            (simPoint.Tag as NumericUpDown).Value = (decimal)(simPoint as M14SimMmenc).Value;
        }
        if (simPoint is M14SimMitna)
        {
          if (simPoint.Tag != null)
            (simPoint.Tag as Label).Text = (simPoint as M14SimMitna).Value.ToString();
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

    private System.Windows.Forms.CheckBox[] BinaryInputs;
    private System.Windows.Forms.NumericUpDown[] AnalogInputs;
    private System.Windows.Forms.Label[] Counters;

    private void customizeDatabase()
    {
      uint i;

      BinaryInputs = new System.Windows.Forms.CheckBox[3] { BinaryInput0, BinaryInput1, BinaryInput2 };
      AnalogInputs = new System.Windows.Forms.NumericUpDown[3] { AnalogInput0, AnalogInput1, AnalogInput2 };
      Counters = new System.Windows.Forms.Label[3] { Counter0, Counter1, Counter2 };

      // This Clear is not required here since the database is empty,
      // but this would delete all points from the database.
      db.Clear();

      TMWSimPoint pt;
      for (i = 0; i < 3; i++)
      {
        pt = db.AddPoint(Defs.TYPE_ID.MSPNA, 100 + i);
        pt.Tag = BinaryInputs[i];
        BinaryInputs[i].Tag = pt;

        pt = db.AddPoint(Defs.TYPE_ID.MMENC, 700 + i);
        pt.Tag = AnalogInputs[i];
        AnalogInputs[i].Tag = pt;

        pt = db.AddPoint(Defs.TYPE_ID.MITNA, 800 + i);
        pt.Tag = Counters[i];
        Counters[i].Tag = pt;

      }
    }

    private void BinaryInput_CheckedChanged(object sender, EventArgs e)
    {
      M14SimMspna pt = ((sender as Control).Tag) as M14SimMspna;

      M14Request req = null;
      req = new M14Request(masterSctr104);

      // You can put something meaningful here to use when the response callback event occurs
      req.Tag = "This tag is meaningful to me";

      // Register to receive responses or timeout from request 
      req.RequestEvent += new M14Request.RequestEventDelegate(cscnaRequestEvent);
     
      req.cscna(M14Request.CTRL_MODE.AUTO, pt.IOA + 2000, M14Request.QOC_QU.USE_DEFAULT, (byte)((sender as CheckBox).Checked == true ? Defs.SCS_ON : Defs.SCS_OFF));
    }

    // This will be called each time a response is received from the request or on timeout.
    void cscnaRequestEvent(M14Request request, M14ResponseParser response)
    {
      // Sector property indicates what sector this request was sent on if you want to use it.
      M14Sector sector = request.Sector;

      // You can look at the Tag if you put something meaningful there.
      if ((string)request.Tag != "This tag is meaningful to me")
        protocolBuffer.Insert("\nTag did not match, this will not happen.\n");

      if (response.Last)
      {
        // this is the end of the request
        if (response.Status == I870Channel.RESP_STATUS.SUCCESS)
        {
          // This means the request was successful
        }
      }
    }

    private void AnalogInput_ValueChanged(object sender, EventArgs e)
    {
      M14SimMmenc pt = ((sender as Control).Tag) as M14SimMmenc;

      M14Request req = null;
      req = new M14Request(masterSctr104);

      req.csenc(M14Request.CTRL_MODE.AUTO, pt.IOA + 2000, (float)(sender as NumericUpDown).Value, Defs.QOS_QL_USE_DEFAULT);
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

    private void openPB_Click(object sender, EventArgs e)
    {

      masterChan104 = new I104Channel(TMW_CHANNEL_OR_SESSION_TYPE.MASTER);
      masterChan104.ChannelConnectStateEvent += new TMWChannel.ChannelConnectStateEventDelegate(masterChan_ChannelConnectStateEvent);

      masterChan104.Type = WINIO_TYPE.TCP;
      masterChan104.Name = ".NET I104 Master";  /* name displayed in analyzer window */
      masterChan104.WinTCPipAddress = "127.0.0.1";
      masterChan104.WinTCPipPort = 2404;
      masterChan104.WinTCPmode = TCP_MODE.CLIENT;
     
      // Register to receive channel statistics
      //masterChan104.ChannelStatisticsEvent += new TMWChannel.ChannelStatisticsEventDelegate(masterChan_ChannelStatisticsEvent);
      masterChan104.OpenChannel();

      masterSesn104 = new M104Session(masterChan104);
      masterSesn104.SessionOnlineStateEvent += new TMWSession.SessionOnlineStateEventDelegate(masterSesn_SessionOnlineStateEvent);

      // Process Custom ASDUs that the library does not implement
      // If this method is registered, it will be called for every application layer response.
      // This is very rarely required.  
      //masterSesn104.ProcessCustomASDUEvent += new M14Session.ProcessCustomASDUDelegate(masterSesn104_ProcessCustomASDUEvent);
      //customASDURequest = null;
      
      // Register to receive session statistics
      //masterSesn104.SessionStatisticsEvent += new TMWSession.SessionStatisticsEventDelegate(masterSesn_SessionStatisticsEvent);
      masterSesn104.OpenSession();

      masterSctr104 = new M104Sector(masterSesn104);

      // Register to receive sector statistics
      //masterSctr104.SectorStatisticsEvent += new TMWSector.SectorStatisticsEventDelegate(masterSctr_SectorStatisticsEvent);
      masterSctr104.OpenSector();
      db = (M14SimDatabase)masterSctr104.SimDatabase;
      

      // Register to receive notification of database changes
      db.UpdateDBEvent += new TMWSimDataBase.UpdateDBEventDelegate(UpdateDBEvent);

      customizeDatabase();
      openPB.Enabled = false;
      closePB.Enabled = true;
      integrityPollPB.Enabled = true;
    }

    private void closePB_Click(object sender, EventArgs e)
    {
      masterSctr104.CloseSector();
      masterSesn104.CloseSession();
      masterChan104.CloseChannel();
     
      openPB.Enabled = true;
      closePB.Enabled = false;
      integrityPollPB.Enabled = false;
    }

    private void integrityPollPB_Click(object sender, EventArgs e)
    {
      M14Request req = null;
      req = new M14Request(masterSctr104);
     
      req.cicna(M14Request.QOI.GLOBAL, true);

      // Send custom ASDU
      //customASDUPB_Click(sender, e);
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

    void masterSctr_SectorStatisticsEvent(TMWSector sector, TMWSectorStatData statData)
    {
      protocolBuffer.Insert("SECTOR STATISTIC: " + statData.StatType.ToString() + " " + statData.TypeId.ToString() + " " + statData.IOA.ToString() + "\n");
    }
    
    // Send Custom ASDU that the library does not implement
    // This is very rarely required.  
    //private void customASDUPB_Click(object sender, EventArgs e)
    //{
    //  // Send a custom ASDU request to the slave.
    //  if (customASDURequest == null)
    //  {
    //    customASDURequest = new M14Request(masterSctr104);

    //    Boolean dataUnitIdInData = false;
    //    Byte typeId = 99;
    //    Byte vsq = 1;
    //    Byte cot = 5;
    //    Byte[] dataArray = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };

    //    customASDURequest.CustomAsdu(dataUnitIdInData, typeId, vsq, cot, dataArray);
    //  }
    //}

    //// Process Custom ASDUs that the library does not implement
    //// If this method is registered, it will be called for every application layer response.
    //// This is very rarely required.  
    //bool masterSesn104_ProcessCustomASDUEvent(M14Session m14Session, M14Sector m14Sector, byte typeId, byte[] rcvData)
    //{
    //  if (typeId == 99)
    //  {
    //    // Process the response.
    //    int x = rcvData[2];

    //    // The custom ASDU command is complete, remove it from the library queue.
    //    if (customASDURequest != null)
    //    {
    //      customASDURequest.CustomAsduRemove();
    //      customASDURequest = null;
    //    }
    //    // Tell the library we have processed this response.
    //    return true;
    //  }

    //  // Tell the library that we have not processed this response.
    //  return false;
    //}

    // This will save the protocol log to a file
    private void SaveLog_Click(object sender, EventArgs e)
    {
      this.protocolBuffer.UpdateDiagIdMask(SCLDIAG_ID.ALL_BUT_TARGET, true);
      this.protocolBuffer.UpdateDiagIdMask(SCLDIAG_ID.PHYS, false);
      this.protocolBuffer.SaveAsText("104MasterGUI.log", "create", "begin", "end");
    }
  }
}