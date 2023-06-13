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

namespace DNPmasterModem
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
      TMWApplicationBuilder applBuilder = new TMWApplicationBuilder();
      pAppl = TMWApplicationBuilder.getAppl();
      pAppl.EnableEventProcessor = true;
      pAppl.InitEventProcessor();

      protocolBuffer = TMWApplicationBuilder.getProtocolBuffer();
      protocolBuffer.ProtocolDataReadyEvent += new ProtocolBuffer.ProtocolDataReadyEventDelegate(ProtocolEvent);
      protocolBuffer.EnableCheckForDataTimer = true;
      InitializeComponent();
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

    private void ProtocolEvent(ProtocolBuffer buf)
    {
      if (!pauseAnalyzer)
      {
        buf.Lock();
        for (int i = buf.LastProvidedIndex; i < buf.LastAddedIndex; i++)
        {
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

    // See if it is time to send another integrity poll.
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

    // See if it is time to send another event poll
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

    // Integrity poll button pressed
    private void Integrity_Click(object sender, EventArgs e)
    {
      if (masterSesn != null)
      {
        protocolBuffer.Insert("\nRequested Integrity Poll\n");
        MDNPRequest request = new MDNPRequest(masterSesn);
        request.IntegrityPoll(true);
      }
    }

    // Event poll button pressed
    private void Event_Click(object sender, EventArgs e)
    {
      if (masterSesn != null)
      {
        protocolBuffer.Insert("\nRequested Event Poll\n");
        MDNPRequest request = new MDNPRequest(masterSesn);
        request.ReadClass(MDNPRequest.DNP_QUALIFIER.Q_ALL_POINTS, 0, false, true, true, true);
      }
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
     
    // Open master channel and session
    private void openMaster()
    {
      if (masterSesn == null)
      {
        masterChan = new DNPChannel(TMW_CHANNEL_OR_SESSION_TYPE.MASTER);
        masterChan.Name = "MDNP";
 
        masterChan.Type = WINIO_TYPE.RS232; 
        masterChan.Win232comPortName = PortTB.Text;
        masterChan.Win232baudRate = BaudTB.Text;
        masterChan.Win232numDataBits = RS232_DATA_BITS.BITS_8;
        masterChan.Win232numStopBits = RS232_STOP_BITS.BITS_1;
        if(HardwareCB.Checked)
          masterChan.Win232portMode = RS232_PORT_MODE.HARDWARE;
        //masterChan.Win232portRtsMode = RS232_RTS_MODE.ENABLE;

        // If the disabled check box is selected disable the channel so that the master session does not see the 
        // connection or send and receive data.
        masterChan.Win232Disabled = DisabledCB.Checked;

        // Open the channel as usual
        masterChan.OpenChannel();

        // Register to receive the received data including any responses from the modem.
        if(masterChan.Win232Disabled)
          masterChan.rxPhysCallbackEvent += new TMWChannel.RxPhysCallbackEventDelegate(rxCallbackFunc);

        masterSesn = new MDNPSession(masterChan);
        masterSesn.OpenSession();

        // Set up integrity poll timer
        integrityPollCount = 0;
        integrityPollInterval = 3600;  // Once per hour
        IntegrityProgressBar.Value = 0;
        IntegrityProgressBar.Maximum = (int)integrityPollInterval; 

        // Set up event poll timer
        eventPollCount = 0;
        eventPollInterval = 5;
        EventProgressBar.Value = 0;
        EventProgressBar.Maximum = (int)eventPollInterval; 
      }
    }

    // Close master session and channel
    private void closeMaster()
    {
      if (masterSesn != null)
      {
        if (masterChan.Win232Disabled)
          // Unregister so no more received data events will be generated.
          masterChan.rxPhysCallbackEvent -= new TMWChannel.RxPhysCallbackEventDelegate(rxCallbackFunc);

        masterSesn.CloseSession();
        masterChan.CloseChannel();
        masterSesn = null;
        masterChan = null;

        EventPollTimer.Stop();
        EventProgressBar.Value = 0;
        IntegrityPollTimer.Stop();
        IntegrityProgressBar.Value = 0;
      } 
    }

    // Add received data to output window.
    private delegate void OutDataDelegate(String text);
    public void OutData(String text)
    {
      if (InvokeRequired)
        BeginInvoke(new OutDataDelegate(OutData), new object[] { text });
      else
      {
        OutDataTB.Text += text;
      }
    }

    // Received Data event
    private void rxCallbackFunc(TMWChannel channel, RxCallbackData callbackData)
    {
      Byte[] data = callbackData.GetRxArray();
      String text = "" ;
      //for(int i = 0; i< callbackData.NumBytes; i++)
      //{
      //  text += String.Format("{0:x2} ", data[i]);
      //} 
      for (int i = 0; i < callbackData.NumBytes; i++)
      {
        text += (char)data[i];
      }

      OutData(text);
    }

    // Send data to the COM port bypassing the DNP3 Protocol Layers
    private void sendData(String text)
    {
      // clear the data from the output window
      OutDataTB.Text = "";

      if (masterChan != null)
      {
        int i;
        byte[] dataToSend = new byte[text.Length + 1];
        for (i = 0; i < text.Length; i++)
          dataToSend[i] = (Byte)text[i];
        // append carriage return
        dataToSend[i] = 0x0d;

        // Send carriage return terminated modem request 
        masterChan.SendDataBytes(dataToSend);
      }
    }

    // Don't let DNP Protocol library send or receive data, 
    // Channel will appear disconnected.
    private void DisableData(bool value)
    {
      if (masterChan != null)
      {
        masterChan.Win232Disabled = value;

        if (value)
          masterChan.rxPhysCallbackEvent += new TMWChannel.RxPhysCallbackEventDelegate(rxCallbackFunc);
        else
          masterChan.rxPhysCallbackEvent -= new TMWChannel.RxPhysCallbackEventDelegate(rxCallbackFunc);
      }
    }

    // Open button pressed
    private void OpenBT_Click(object sender, EventArgs e)
    {
      openMaster();
    }

    // Close button pressed
    private void CloseBT_Click(object sender, EventArgs e)
    {
      closeMaster();
    }

    // Disabled Data Transfer button pressed
    private void DisabledCB_CheckedChanged(object sender, EventArgs e)
    {
      DisableData(DisabledCB.Checked);
    }

    // Init Modem button pressed
    private void InitBT_Click(object sender, EventArgs e)
    {
      sendData(this.InitTB.Text); 
    }

    // Connect button pressed
    private void ConnectBT_Click(object sender, EventArgs e)
    {
      sendData(this.ConnectTB.Text);
    }

    // Disconnect button pressed
    private void DisconnectBT_Click(object sender, EventArgs e)
    {
      sendData(this.DisconnectTB.Text); 
    }

  }
}