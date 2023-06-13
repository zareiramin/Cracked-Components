using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using TMW.SCL;
using TMW.SCL.MB;
using TMW.SCL.MB.Master;
using TMW.SCL.ProtocolAnalyzer;

namespace MMBmasterGUI
{
  public partial class MasterForm : Form
  {
    static TMWApplication pAppl;

    private readonly MMBSimDatabase db;
    private readonly MMBSession masterSesn;
    private readonly MBChannel masterChan;

    // Timer values
    private decimal readPollInterval;
    private decimal readPollCount;

    private delegate void UpdatePointDelegate(TMWSimPoint simPoint);

    /// <summary>
    /// Construct the main form for this application
    /// </summary>
    public MasterForm()
    {
      TMWApplicationBuilder applBuilder = new TMWApplicationBuilder();

      // This causes application to process received data and timers.
      pAppl = TMWApplicationBuilder.getAppl();
      pAppl.EnableEventProcessor = true;
      pAppl.InitEventProcessor();

      // This enables a Forms timer to process protocol data output.
      // For non Forms applications see MBMasterDatabaseEvents example for getting protocol data.
      protocolBuffer = TMWApplicationBuilder.getProtocolBuffer();
      protocolBuffer.ProtocolDataReadyEvent += new ProtocolBuffer.ProtocolDataReadyEventDelegate(ProtocolEvent);
      protocolBuffer.EnableCheckForDataTimer = true;

      InitializeComponent();

      // create, initialize and open channel
      masterChan = new MBChannel(TMW_CHANNEL_OR_SESSION_TYPE.MASTER);

      bool bSerial = false;
      if (bSerial)
      {
        masterChan.Type = WINIO_TYPE.RS232;
        masterChan.Win232comPortName = "COM3";
        masterChan.Win232baudRate = "9600";
        masterChan.Win232numDataBits = RS232_DATA_BITS.BITS_8;
        masterChan.Win232numStopBits = RS232_STOP_BITS.BITS_1;
        masterChan.Win232parity = RS232_PARITY.EVEN;
        masterChan.Win232portMode = RS232_PORT_MODE.NONE; 

        masterChan.LinkConfigType = MBChannel.LINK_TYPE.ASCII;
      }
      else
      {
        masterChan.Type = WINIO_TYPE.TCP;
        masterChan.WinTCPipAddress = "127.0.0.1";
        masterChan.WinTCPipPort = 502;
        masterChan.WinTCPmode = TCP_MODE.CLIENT;
      }

      masterChan.Name = ".NET Modbus Master";  /* name displayed in analyzer window */  
      masterChan.OpenChannel();

      // create, initialize and open session
      masterSesn = new MMBSession(masterChan);
      masterSesn.OpenSession();
      masterSesn.SessionStatisticsEvent += new TMWSession.SessionStatisticsEventDelegate(masterSesn_SessionStatisticsEvent);

      // initialize built in database
      db = (MMBSimDatabase)masterSesn.SimDatabase;
      // Register to receive notification of database changes
      db.UpdateDBEvent += new TMWSimDataBase.UpdateDBEventDelegate(UpdateDBEvent);
      customizeDatabase();

      // Set up poll timer to read data from slave
      readPollCount = 0;
      readPollInterval = 20; // every 2 seconds
      ReadProgressBar.Value = 0;
      ReadProgressBar.Maximum = (int)readPollInterval;
      ReadPollTimer.Start();
    }

    /// <summary>
    /// Handle statistics provided by the .NET Protocol Component
    /// </summary>
    /// <param name="session"></param>
    /// <returns></returns>
    void masterSesn_SessionStatisticsEvent(TMWSession session, TMWSessionStatData eventData)
    {
      protocolBuffer.Insert("STATISTIC: " + eventData.StatType.ToString() + " " + eventData.TypeId.ToString() + " " + eventData.PointIndex.ToString() + "\n"); 
    }


    /// <summary>
    /// Set up our database
    /// </summary>
    private void customizeDatabase()
    {
      ushort i;

      // This Clear is not required here since the database is empty,
      // but this would delete all points from the database.
      db.Clear();

      // Add points to database
      for (i = 0; i < 3; i++)
      {
        db.addCoil(i);
        db.addDisc(i);
        db.addHreg(i);
        db.addIreg(i);
      }

    }

    /// <summary>
    /// Process Discrete Input register change
    /// </summary>
    /// <param name="simPoint"></param>
    private void updateDiscreteInput(TMWSimPoint simPoint)
    {
      if (simPoint == null)
        return;

      string strVal = (simPoint as MMBSimDInput).Value ? "On" : "Off";
      Color textColor = (simPoint as MMBSimDInput).Value ? Color.ForestGreen : Color.Red;

      switch (simPoint.PointNumber)
      {
        case 0:
          DiscIn0.Text = strVal;
          DiscIn0.ForeColor = textColor;
          break;
        case 1:
          DiscIn1.Text = strVal;
          DiscIn1.ForeColor = textColor;
          break;
        case 2:
          DiscIn2.Text = strVal;
          DiscIn2.ForeColor = textColor;
          break;
        default:
          protocolBuffer.Insert("Received update for unexpected discrete input point: " +
                                simPoint.PointNumber.ToString());
          break;
      }
    }
    /// <summary>
    /// Process Coil change
    /// </summary>
    /// <param name="simPoint"></param>
    private void updateCoil(TMWSimPoint simPoint)
    {
      if (simPoint == null)
        return;

      string strVal = (simPoint as MMBSimCoil).Value ? "On" : "Off";
      Color textColor = (simPoint as MMBSimCoil).Value ? Color.ForestGreen : Color.Red;

      switch (simPoint.PointNumber)
      {
        case 0:
          Coil0.Text = strVal;
          Coil0.ForeColor = textColor;
          break;
        case 1:
          Coil1.Text = strVal;
          Coil1.ForeColor = textColor;
          break;
        case 2:
          Coil2.Text = strVal;
          Coil2.ForeColor = textColor;
          break;
        default:
          protocolBuffer.Insert("Received update for unexpected coil point: " + simPoint.PointNumber.ToString());
          break;
      }
    }
    /// <summary>
    /// Process Holding register change
    /// </summary>
    /// <param name="simPoint"></param>
    private void updateHoldingRegisterInput(TMWSimPoint simPoint)
    {
      if (simPoint == null)
        return;

      switch (simPoint.PointNumber)
      {
        case 0:
          HoldingRegister0.Text = (simPoint as MMBSimHReg).Value.ToString();
          break;
        case 1:
          HoldingRegister1.Text = (simPoint as MMBSimHReg).Value.ToString();
          break;
        case 2:
          HoldingRegister2.Text = (simPoint as MMBSimHReg).Value.ToString();
          break;
        default:
          protocolBuffer.Insert("Received update for unexpected holding register point: " +
                                simPoint.PointNumber.ToString());
          break;
      }
    }

    /// <summary>
    /// Process Input Register change
    /// </summary>
    /// <param name="simPoint"></param>
    private void updateInputRegister(TMWSimPoint simPoint)
    {
      if (simPoint == null)
        return;

      switch (simPoint.PointNumber)
      {
        case 0:
          InputRegister0.Text = (simPoint as MMBSimIReg).Value.ToString();
          break;
        case 1:
          InputRegister1.Text = (simPoint as MMBSimIReg).Value.ToString();
          break;
        case 2:
          InputRegister2.Text = (simPoint as MMBSimIReg).Value.ToString();
          break;
        default:
          protocolBuffer.Insert("Received update for unexpected input register point: " +
                                simPoint.PointNumber.ToString());
          break;
      }
    }

    /// <summary>
    /// Event Handler to deal with changes in the database
    /// </summary>
    /// <param name="simPoint"></param>
    private void UpdateDBEvent(TMWSimPoint simPoint)
    {
      if (simPoint == null)
        return;

      if (InvokeRequired)
        BeginInvoke(new UpdatePointDelegate(UpdateDBEvent), new object[] { simPoint });
      else
      {
        switch ((MBTYPE_ID)simPoint.PointType)
        {
          case MBTYPE_ID.DISCRETE_INPUT:
            // Binary Input
            updateDiscreteInput(simPoint);
            break;
          case MBTYPE_ID.COIL:
            // Binary Output (CROB)
            updateCoil(simPoint);
            break;
          case MBTYPE_ID.HOLDING_REGISTER:
            // Binary Counters
            updateHoldingRegisterInput(simPoint);
            break;
          case MBTYPE_ID.INPUT_REGISTER:
            // Analog Inputs
            updateInputRegister(simPoint);
            break;
          default:
            protocolBuffer.Insert("Unknown point type in database update routine");
            break;
        }
      }
    }

    private void PollTimer_Tick(object sender, EventArgs e)
    {
      if (readPollCount++ < readPollInterval)
      {
        ReadProgressBar.Increment(1);
      }
      else
      {
        // time to do a poll

        Poll_Click( sender, e);

        readPollCount = 0;
        ReadProgressBar.Value = 0;
        ReadProgressBar.Maximum = (int)readPollInterval;
        ReadPollTimer.Start();
      }
    }

    private void Poll_Click(object sender, EventArgs e)
    {
      protocolBuffer.Insert("\nPoll timer expired: Now Issuing read requests ... \n");
      MMBRequest request = new MMBRequest(masterSesn);

      // You can put something meaningful here to use when the response callback event occurs
      request.Tag = this;

      // Register to receive responses or timeout from request 
      request.RequestEvent += new MMBRequest.RequestEventDelegate(request_RequestEvent);

      // Change these calls to read other point ranges
      request.ReadCoils(0, 3);

      // The request object can be reused. 
      // However, if you want to receive response events for each, you should use separate request objects
      // or wait for the previous command to complete.
      request.ReadDiscs(0, 3);
      request.ReadHregs(0, 3); ;

      // Allocate a new request object 
       MMBRequest request1 = new MMBRequest(masterSesn);

      // Register to receive responses or timeout from request 
      request1.RequestEvent += new MMBRequest.RequestEventDelegate(request_RequestEvent1);
      request1.ReadIregs(0, 3);
    }

    // This will be called each time a response is received from the request or on timeout.
    void request_RequestEvent(MMBRequest request, MMBResponseParser response)
    {
      // Session property indicates what session this request was sent on if you want to use it.
      MMBSession session = request.Session;

      // You can look at the Tag if you put something meaningful there.
      if (request.Tag != this)
        protocolBuffer.Insert("\nTag did not match, this will not happen.\n");

      if (response.Status == MBChannel.RESPONSE_STATUS.SUCCESS)
      {
        // This means the request was successful
      }
      else if (response.Status == MBChannel.RESPONSE_STATUS.FAILURE)
      {
        Byte exception = response.ExceptionCode;
        protocolBuffer.Insert("\nCommand Failed, exception code = " + exception.ToString() +"\n");
      }
    }

    // This will be called each time a response is received from the request or on timeout.
    void request_RequestEvent1(MMBRequest request, MMBResponseParser response)
    {
      if (response.Status == MBChannel.RESPONSE_STATUS.SUCCESS)
      {
        // This means the request was successful
      }
    }


    private void CoilOn_Click(object sender, EventArgs e)
    {
      // Determine which point this request is for
      if (sender == null)
        return;

      ushort pointNumber = Convert.ToUInt16((sender as Control).Tag);
      MMBRequest request = new MMBRequest(masterSesn);
      request.WriteCoil(pointNumber, true);
    }

    private void CoilOff_Click(object sender, EventArgs e)
    {
      if (sender == null)
        return;

      // Determine which point this request is for
      ushort pointNumber = Convert.ToUInt16((sender as Control).Tag);
      MMBRequest request = new MMBRequest(masterSesn);
      request.WriteCoil(pointNumber, false);
    }

    private void WriteHoldingRegister_Click(object sender, EventArgs e)
    {
      // Determine which point this request is for
      if (sender == null)
        return;

      ushort pointNumber = Convert.ToUInt16((sender as Control).Tag);

      // Get the value to write for this point
      ushort val;
      switch (pointNumber)
      {
        case 0:
          val = (ushort)HoldingRegister0Val.Value;
          break;
        case 1:
          val = (ushort)HoldingRegister1Val.Value;
          break;
        case 2:
          val = (ushort)HoldingRegister2Val.Value;
          break;
        default:
          val = 0;
          protocolBuffer.Insert("Unexected analog output command for undefined point: " + pointNumber.ToString());
          break;
      }

      MMBRequest request = new MMBRequest(masterSesn);
      request.WriteHreg(pointNumber, val);
    }

    private void ChangePollPeriod_Click(object sender, EventArgs e)
    {
      ReadPollTimer.Stop();
      readPollInterval = PollIntervalMilliSecNumUpDown.Value;
      ReadProgressBar.Maximum = (int)readPollInterval;

      if (readPollInterval == 0)
      {
        PollEnableChkBox.Checked = false;
      }

      if (PollEnableChkBox.Checked)
      {
        ReadPollTimer.Start();
      }
      else
      {
        // if unchecked, reset the progress bar and current count
        ReadProgressBar.Value = 0;
        readPollCount = 0;
      }
    }

    #region Protocol Buffer Display

    /// <summary>
    /// Member Variables specific to displayin protocol buffer information
    /// </summary>
    private const int WM_VSCROLL = 0x115;
    private const int SB_BOTTOM = 7;
    private int _OldEventMask = 0;
    private const int WM_SETREDRAW = 0x000B;
    private const int EM_SETEVENTMASK = 0x0431;
    private TMW.SCL.ProtocolAnalyzer.ProtocolBuffer protocolBuffer;

    private bool pauseAnalyzer;

    [DllImport("user32", CharSet = CharSet.Auto)]
    private static extern int SendMessage(HandleRef hWnd, int msg, int wParam, int lParam);
     

    /// <summary>
    /// Goto bottom of the protocol text control
    /// </summary>
    private void ScrollToBottom()
    {
      SendMessage(new HandleRef(protocolAnalyzerTextCtrl, protocolAnalyzerTextCtrl.Handle), WM_VSCROLL, SB_BOTTOM, 0);
    }

    /// <summary>
    /// Helps cut down on flickering in the protocol text control
    /// </summary>
    private void BeginUpdate()
    {
      // Prevent the control from raising any events
      _OldEventMask = SendMessage(new HandleRef(protocolAnalyzerTextCtrl, protocolAnalyzerTextCtrl.Handle), EM_SETEVENTMASK, 0, 0);

      // Prevent the control from redrawing itself
      SendMessage(new HandleRef(protocolAnalyzerTextCtrl, protocolAnalyzerTextCtrl.Handle), WM_SETREDRAW, 0, 0);
    }

    /// <summary>
    /// Helps cut down on flickering in the protocol text control
    /// </summary>
    private void EndUpdate()
    {
      // Allow the control to redraw itself
      SendMessage(new HandleRef(protocolAnalyzerTextCtrl, protocolAnalyzerTextCtrl.Handle), WM_SETREDRAW, 1, 0);

      // Allow the control to raise event messages
      SendMessage(new HandleRef(protocolAnalyzerTextCtrl, protocolAnalyzerTextCtrl.Handle), EM_SETEVENTMASK, 0, _OldEventMask);
    }

    /// <summary>
    /// Remove some lines from the top of the protocol text control to keep it limited in size
    /// </summary>
    /// <param name="numLines"></param>
    private void RemoveTopLines(int numLines)
    {
      int lastLine = protocolAnalyzerTextCtrl.Lines.GetLength(0) - 1;
      if (numLines < 1)
      {
        return;
      }
      else if (numLines > lastLine)
      {
        numLines = lastLine;
      }

      int startChar = protocolAnalyzerTextCtrl.GetFirstCharIndexFromLine(0);
      int endChar = protocolAnalyzerTextCtrl.GetFirstCharIndexFromLine(numLines);

      bool b = protocolAnalyzerTextCtrl.ReadOnly;
      protocolAnalyzerTextCtrl.ReadOnly = false;
      protocolAnalyzerTextCtrl.Select(startChar, endChar - startChar);
      protocolAnalyzerTextCtrl.SelectedRtf = "";
      protocolAnalyzerTextCtrl.ReadOnly = b;
    }

    /// <summary>
    /// New protocol data is available
    /// </summary>
    /// <param name="buf"></param>
    private void ProtocolEvent(TMW.SCL.ProtocolAnalyzer.ProtocolBuffer buf)
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
            {
              protocolAnalyzerTextCtrl.AppendText(protocolBuffer.getPdoAtIndex(i).ProtocolText);
              SendMessage(new HandleRef(protocolAnalyzerTextCtrl, protocolAnalyzerTextCtrl.Handle), WM_VSCROLL, SB_BOTTOM, 0);
            }
          }
        }
        buf.UnLock();

        // remove lines from the text box
        if (protocolAnalyzerTextCtrl.Lines.Length > 1000)
        {
          BeginUpdate();
          RemoveTopLines(100);
          ScrollToBottom();
          EndUpdate();
        }
      }
    }
    /// <summary>
    /// User wants to pause the protocol scrolling
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void protocolAnalyzerTextCtrl_MouseDown(object sender, MouseEventArgs e)
    {
      if (e.Clicks == 2)
      {
        // double click toggles pausing
        if (pauseAnalyzer)
        {
          // it's paused, so unpause it
          pauseAnalyzer = false;
          protocolAnalyzerTextCtrl.BackColor = Color.Gainsboro;
        }
        else
        {
          // it's not paused, so pause it
          pauseAnalyzer = true;
          protocolAnalyzerTextCtrl.BackColor = Color.MistyRose;
        }
      }
    }

    #endregion
  }
}