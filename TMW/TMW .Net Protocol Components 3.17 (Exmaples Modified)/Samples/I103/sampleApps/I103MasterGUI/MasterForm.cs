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
using TMW.SCL.IEC60870_5.I103.Master;

namespace I103masterGUI
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
    private M103SimDatabase db;

    private M103Session masterSesn103;
    private M103Sector masterSctr103;
    private FT12Channel masterChan103;

    private M103Request customASDURequest;

    private bool pauseAnalyzer;

    public MasterForm()
    {
      TMWApplicationBuilder applBuilder = new TMWApplicationBuilder();

      // This causes application to process received data and timers.
      pAppl = TMWApplicationBuilder.getAppl();
      pAppl.EnableEventProcessor = true;
      pAppl.InitEventProcessor();

      // This enables a Forms timer to process protocol data output.
      // For non Forms applications see I103MasterDatabaseEvents example for getting protocol data.
      protocolBuffer = TMWApplicationBuilder.getProtocolBuffer();
      protocolBuffer.ProtocolDataReadyEvent += new ProtocolBuffer.ProtocolDataReadyEventDelegate(ProtocolEvent);
      protocolBuffer.EnableCheckForDataTimer = true;

      InitializeComponent();

      masterChan103 = new FT12Channel(TMW_PROTOCOL.I103, TMW_CHANNEL_OR_SESSION_TYPE.MASTER);
      masterChan103.ChannelConnectStateEvent += new TMWChannel.ChannelConnectStateEventDelegate(masterChan_ChannelConnectStateEvent);
      
      // Even though IEC 60870-5-103 would normally be over serial link, use TCP since it makes Demo more friendly */
      bool bSerial = false;
      if (bSerial)
      {
        masterChan103.Type = WINIO_TYPE.RS232;
        masterChan103.Win232comPortName = "COM3";
        masterChan103.Win232baudRate = "9600";
        masterChan103.Win232numDataBits = RS232_DATA_BITS.BITS_8;
        masterChan103.Win232numStopBits = RS232_STOP_BITS.BITS_1;
        masterChan103.Win232parity = RS232_PARITY.EVEN;
        masterChan103.Win232portMode = RS232_PORT_MODE.NONE;
      }
      else
      {
        masterChan103.Type = WINIO_TYPE.TCP;
        masterChan103.WinTCPipAddress = "127.0.0.1";
        masterChan103.WinTCPipPort = 2404;
        masterChan103.WinTCPmode = TCP_MODE.CLIENT;
      }

      masterChan103.Name = ".NET I103 Master";  /* name displayed in analyzer window */
      masterChan103.OpenChannel();

      masterSesn103 = new M103Session(masterChan103);
      masterSesn103.SessionOnlineStateEvent += new TMWSession.SessionOnlineStateEventDelegate(masterSesn_SessionOnlineStateEvent);

      // Process Custom ASDUs that the library does not implement
      // If this method is registered, it will be called for every application layer response.
      // This is very rarely required.  
      //masterSesn103.ProcessCustomASDUEvent += new M103Session.ProcessCustomASDUDelegate(masterSesn103_ProcessCustomASDUEvent);
      //customASDURequest = null;

      masterSesn103.OpenSession();

      masterSctr103 = new M103Sector(masterSesn103);

      masterSctr103.OpenSector();
      db = (M103SimDatabase)masterSctr103.SimDatabase;


      // Register to receive notification of database changes
      db.UpdateDBEvent += new TMWSimDataBase.UpdateDBEventDelegate(UpdateDBEvent);

      customizeDatabase();
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

    private delegate void UpdatePointDelegate(TMWSimPoint simPoint);
    private void UpdateDBEvent(TMWSimPoint simPoint)
    {
      if (this.InvokeRequired)

        this.BeginInvoke(new UpdatePointDelegate(UpdateDBEvent), new object[] { simPoint });

      else
      {
        if (simPoint.Tag != null)
        {
          if (simPoint is M103SimDpi)
          {
            (simPoint.Tag as Label).Text = (simPoint as M103SimDpi).Value.ToString();
          }
          else if (simPoint is M103SimMsrnd)
          {
            (simPoint.Tag as Label).Text = (simPoint as M103SimMsrnd).Value.ToString();
          }
          else
          {
            protocolBuffer.Insert("Unknown point type in database update routine");
          }
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

    private System.Windows.Forms.Label[] DPIs;
    private System.Windows.Forms.Label[] MSRNDs;

    private void customizeDatabase()
    {
      Byte i;

      DPIs = new System.Windows.Forms.Label[3] { dpi1, dpi2, dpi3 };
      MSRNDs = new System.Windows.Forms.Label[3] { msrnd1, msrnd2, msrnd3 };

      // This Clear is not required here since the database is empty,
      // but this would delete all points from the database.
      db.Clear();

      TMWSimPoint pt;
      for (i = 0; i < 3; i++)
      {
        if (db.Add103Point((short)Defs.TYPE_ID.DPI, 128, (Byte)(i + 22), 0))
        {
          pt = db.LookupPoint(Defs.TYPE_ID.DPI, 128, (Byte)(i + 22), 0);
          pt.Tag = DPIs[i];
          DPIs[i].Tag = pt;
        }

        if (db.Add103Point((short)Defs.TYPE_ID.MSRND, 160, (Byte)(i + 144), 0))
        {
          pt = db.LookupPoint(Defs.TYPE_ID.MSRND, 160, (Byte)(i + 144), 0);
          pt.Tag = MSRNDs[i];
          MSRNDs[i].Tag = pt;
        }
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

        // remove lines from the text box
        if (protocolAnalyzer.Lines.Length > 1000)
        {
          BeginUpdate();
          RemoveTopLines(100);
          ScrollToBottom();
          EndUpdate();
        }
        buf.UnLock();
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

    private void interogationPB_Click(object sender, EventArgs e)
    {
      M103Request req = null;
      req = new M103Request(masterSctr103);

      // You can put something meaningful here to use when the response callback event occurs
      req.Tag = this;

      // Register to receive responses or timeout from request 
      req.RequestEvent += new M103Request.RequestEventDelegate(req_RequestEvent);

      req.gi();

      // Send custom ASDU
      //customASDUPB_Click(sender, e);
    }

    // This will be called each time a response is received from the request or on timeout.
    void req_RequestEvent(M103Request request, M103ResponseParser response)
    {
      // Sector property indicates what sector this request was sent on if you want to use it.
      M103Sector sector = request.Sector;

      // You can look at the Tag if you put something meaningful there.
      if (request.Tag != this)
        protocolBuffer.Insert("\nTag did not match, this will not happen.\n");

      if (response.Last)
      {
        // this is the end of the request
        if (response.status == I870Channel.RESP_STATUS.SUCCESS)
        {
          // This means the request was successful
        }
      }
    }

    // Send Custom ASDU that the library does not implement
    // This is very rarely required.  
    //private void customASDUPB_Click(object sender, EventArgs e)
    //{
    //  customASDURequest = new M103Request(masterSctr103);
    //  customASDURequest.ResponseTimeout = 5000;

    //  Boolean dataUnitIdInData = false;
    //  Byte typeId = 99;
    //  Byte vsq = 1;
    //  Byte cot = 5;
    //  Byte[] dataArray = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };
    //  customASDURequest.CustomAsdu(dataUnitIdInData, typeId, vsq, cot, dataArray);
    //}

    //// Process Custom ASDUs that the library does not implement
    //// If this method is registered, it will be called for every application layer response.
    //// This is very rarely required.  
    //bool masterSesn103_ProcessCustomASDUEvent(M103Session m103Session, M103Sector m103Sector, byte typeId, byte[] rcvData)
    //{
    //  if (typeId == 99)
    //  {
    //    // Process the response.
    //    int x = rcvData[2];

    //    // The custom ASDU command is complete, remove it from the lbirary queue.
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
      this.protocolBuffer.SaveAsText("103MasterGUI.log", "create", "begin", "end");
    }
  }
}