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
    private I104RedundancyGroup masterRdcyGroup104;
    private I104RedundantChannel masterRdnt1Chan104;
    private I104RedundantChannel masterRdnt2Chan104; 
    private bool pauseAnalyzer;

    public MasterForm()
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
        else if (simPoint is M14SimMmenc)
        {
          if (simPoint.Tag != null)
            (simPoint.Tag as NumericUpDown).Value = (decimal)(simPoint as M14SimMmenc).Value;
        }
        else if (simPoint is M14SimMitna)
        {
          if (simPoint.Tag != null)
            (simPoint.Tag as Label).Text = (simPoint as M14SimMitna).Value.ToString();
        }
        else
        {
          // protocolBuffer.Insert("Unknown point type in database update routine");
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

      // Register to receive responses or timeout from request 
      req.RequestEvent += new M14Request.RequestEventDelegate(cscnaRequestEvent);
     
      req.cscna(M14Request.CTRL_MODE.AUTO, pt.IOA + 2000, M14Request.QOC_QU.USE_DEFAULT, (byte)((sender as CheckBox).Checked == true ? Defs.SCS_ON : Defs.SCS_OFF));
    }

    // This will be called each time a response is received from the request or on timeout.
    void cscnaRequestEvent(M14Request request, M14ResponseParser response)
    {
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
      masterRdcyGroup104 = new I104RedundancyGroup(TMW_CHANNEL_OR_SESSION_TYPE.MASTER);
      masterRdcyGroup104.SetName("rdcyGroup");

      if (!masterRdcyGroup104.InitGroup())
        // failed 
        return;

      masterRdnt1Chan104 = new I104RedundantChannel(TMW_CHANNEL_OR_SESSION_TYPE.MASTER);
      masterRdnt1Chan104.ChannelConnectStateEvent += new TMWChannel.ChannelConnectStateEventDelegate(masterChan_ChannelConnectStateEvent);

      masterRdnt1Chan104.Type = WINIO_TYPE.TCP;
      masterRdnt1Chan104.Name = "M104_1";  /* name displayed in analyzer window */
      masterRdnt1Chan104.WinTCPipAddress = "127.0.0.1";
      masterRdnt1Chan104.WinTCPipPort = 2404;
      masterRdnt1Chan104.WinTCPmode = TCP_MODE.CLIENT;
      if (!masterRdnt1Chan104.OpenChannel(masterRdcyGroup104))
        //failed
        return;

      masterRdnt2Chan104 = new I104RedundantChannel(TMW_CHANNEL_OR_SESSION_TYPE.MASTER);
      masterRdnt2Chan104.ChannelConnectStateEvent += new TMWChannel.ChannelConnectStateEventDelegate(masterChan_ChannelConnectStateEvent);

      masterRdnt2Chan104.Type = WINIO_TYPE.TCP;
      masterRdnt2Chan104.Name = "M104_2";  /* name displayed in analyzer window */
      masterRdnt2Chan104.WinTCPipAddress = "127.0.0.1";
      masterRdnt2Chan104.WinTCPipPort = 2405;
      masterRdnt2Chan104.WinTCPmode = TCP_MODE.CLIENT;
      if (!masterRdnt2Chan104.OpenChannel(masterRdcyGroup104))
        //failed
        return;

      masterSesn104 = new M104Session(masterRdcyGroup104);
      masterSesn104.SessionOnlineStateEvent += new TMWSession.SessionOnlineStateEventDelegate(masterSesn_SessionOnlineStateEvent);

      masterSesn104.OpenSession();

      masterSctr104 = new M104Sector(masterSesn104);

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
      masterRdnt1Chan104.CloseChannel();
      masterRdnt2Chan104.CloseChannel();
      masterRdcyGroup104.DeleteGroup();
     
      openPB.Enabled = true;
      closePB.Enabled = false;
      integrityPollPB.Enabled = false;
    }

    private void integrityPollPB_Click(object sender, EventArgs e)
    {
      M14Request req = null;
      req = new M14Request(masterSctr104);
     
      req.cicna(M14Request.QOI.GLOBAL, true);
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

    void masterChan_ChannelConnectStateEvent(TMWChannel channel, bool state)
    {
      // state == true indicates channel is now connected
      if (channel == masterRdnt1Chan104)
        UpdateState(true, 1, state);
      else
        UpdateState(true, 2, state);
    }

    void masterSesn_SessionOnlineStateEvent(TMWSession session, bool state)
    {
      // state == true indicates session is now online 
      UpdateState(false, 0, state);
    }

  }
}