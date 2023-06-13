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

namespace DNPmasterFileTransfer
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
    private MDNPDatabase mdb;
    private MDNPSession masterSesn;
    private DNPChannel masterChan;
    private bool pauseAnalyzer;

    // Timer values
    private decimal integrityPollInterval;
    private decimal integrityPollCount;
    private decimal eventPollInterval;
    private decimal eventPollCount;

    private RemoteFileForm remoteBrowseWindow;

    public MasterForm()
    {
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

      InitializeComponent();

      masterChan = new DNPChannel(TMW_CHANNEL_OR_SESSION_TYPE.MASTER);

      bool bSerialMode = false;
      if (bSerialMode)
      {
        masterChan.Type = WINIO_TYPE.RS232;
        masterChan.Win232comPortName = "COM3";
        masterChan.Win232baudRate = "56000";
        masterChan.Win232numDataBits = RS232_DATA_BITS.BITS_8;
        masterChan.Win232numStopBits = RS232_STOP_BITS.BITS_1;
        masterChan.Win232portMode = RS232_PORT_MODE.NONE;
      }
      else
      {
        masterChan.Type = WINIO_TYPE.TCP;
        masterChan.Name = ".NET DNP Master";  /* name displayed in analyzer window */
        masterChan.WinTCPipAddress = "127.0.0.1";
        masterChan.WinTCPipPort = 20000;
        masterChan.WinTCPmode = TCP_MODE.CLIENT;
      }

      masterChan.OpenChannel();

      masterSesn = new MDNPSession(masterChan);

      // If using TCP the DNP Spec requires keep alives to be configured in order to detect disconnects.
      if (!bSerialMode)
        masterSesn.LinkStatusPeriod = 30000;

      // For examples of using DNP Secure Authentication, see the DNPSlaveSA example
      masterSesn.AuthenticationEnabled = false;

      // Do not use the built in simulated database functionality. 
      // The user application will implement their own database functionality instead.
      // For example, StoreBinaryInputEvent below.
      MDNPDatabase.UseSimDatabase = false;

      // Leave UseSimFileDatabase==true
      // By default this is true, if set to false the built in Windows file open, close, 
      //   read, write, and file authentication functionality will not be used.
      //  This allows spitting the Windows File access and File authentication functionality  
      // from the rest of the user implemented database functionality. If you want to implement your own
      // file open/close read/write authentication functionality this should be set to false.
      // This property controls whether or not the following events will be generated.
      //  StoreFileAuthKeyEvent , StoreFileDataEvent, GetFileAuthKeyEvent, OpenLocalFileEvent, CloseLocalFileEvent, ReadLocalFileEvent
      // The following three events are NOT controlled by this property. They are controlled by UseSimDatabase instead.
      // StoreFileStatusEvent, StoreFileDataStatusEvent, StoreFileInfoEvent. The first two are used to get the
      // status returned from the outstation and would probably not need to be implemented.
      // StoreFileInfoEvent provides the results of a directory read request.
      //MDNPDatabase.UseSimFileDatabase = false;

      MDNPDatabase.InitEvent += new MDNPDatabase.InitDelegate(MasterInitEvent);

      masterSesn.OpenSession();
      masterSesn.SessionStatisticsEvent += new TMWSession.SessionStatisticsEventDelegate(masterSesn_SessionStatisticsEvent);

      mdb = (MDNPDatabase)masterSesn.SimDatabase;

      // register events on Master to store data
      mdb.StoreBinaryInputEvent += new MDNPDatabase.StoreBinaryInputDelegate(Master_StoreBinaryInputEvent);
      mdb.StoreBinaryOutputEvent += new MDNPDatabase.StoreBinaryOutputDelegate(Master_StoreBinaryOutputEvent);

      // This event is used to return the results from a Read Directory Request.
      mdb.StoreFileInfoEvent += new MDNPDatabase.StoreFileInfoDelegate(Master_StoreFileInfoEvent);


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

      remoteBrowseWindow = new RemoteFileForm();
      remoteBrowseWindow.Session = masterSesn;
      remoteBrowseWindow.Directory = "c:/temp";
    }
    
    // This is the result of a directory read from the outstation.
    bool Master_StoreFileInfoEvent(MDNPDatabase db, DNP_FILE_TYPE fileType, uint fileSize, TMWTime fileCreationTime, DNP_FILE_PERMISSIONS permissions, string fileName)
    {
      remoteBrowseWindow.AddFileName(fileName, fileType, fileSize, fileCreationTime, permissions);
      return true;
    }
     

    // Add received data to output window.
    private delegate void FileReadRequestEventDelegate(MDNPRequest request, MDNPResponseParser response);

    // event generated when each response to a file request is received.
    void request_FileRequestEvent(MDNPRequest request, MDNPResponseParser response)
    {
      // This is required to write back into the main form from the Library thread
      if (InvokeRequired)
        BeginInvoke(new FileReadRequestEventDelegate(request_FileRequestEvent), new object[] { request, response });
      else
      {
        if (response.Last)
        {
          FileProgress.Value = FileProgress.Maximum;

          if (response.Status != DNPChannel.RESPONSE_STATUS.SUCCESS)
          {
          }
        }

        else
        {
          if (FileProgress.Value < FileProgress.Maximum)
            FileProgress.Value += 1;
          // start over with progress bar 
          if (FileProgress.Value == FileProgress.Maximum)
            FileProgress.Value = 0;
        }
      }
    }

    void masterSesn_SessionStatisticsEvent(TMWSession session, TMWSessionStatData eventData)
    {
      protocolBuffer.Insert("STATISTIC: " + eventData.StatType.ToString() + " " + eventData.TypeId.ToString() + " " + eventData.PointIndex.ToString() + "\n"); 
    }

    /// <summary>
    /// Called by the SCL when database is first opened
    /// <param name="db">database object that called the event</param>
    static void MasterInitEvent(MDNPDatabase db)
    {
      // Set database Tag to something meaningful, like a pointer or reference
      // It will be available to all database Event routines. 
      db.Tag = 0x5555;

      // Perform any database initialization here.
    }

    /// <summary>
    /// Called by the SCL to store a binary input
    /// </summary>
    /// <param name="db">database object that called the event</param>
    /// <param name="pointNumber">point number</param>
    /// <param name="value">value</param>
    /// <param name="flags">flags</param>
    /// <param name="isEvent">event</param>
    /// <param name="pTimeStamp">time stamp</param>
    /// <returns>success/failure</returns>
    static bool Master_StoreBinaryInputEvent(MDNPDatabase db, ushort pointNumber, bool value, DNPDatabaseFlags flags, bool isEvent, TMWTime pTimeStamp)
    {
      DateTime dt = (DateTime)pTimeStamp.ToDateTime();
      Console.WriteLine("Master:StoreBinaryInput: Point Number {0} Value {1} Flags {2} Date {3}", pointNumber, value, flags, dt);

      // db.Tag contains whatever has been put in it at when session was opened

      return true;
    }

    /// <summary>
    /// Called by the SCL to store a binary output
    /// </summary>
    /// <param name="db">database object that called the event</param>
    /// <param name="pointNumber">point number</param>
    /// <param name="value">value</param>
    /// <param name="flags">flags</param>
    /// <param name="isEvent">event</param>
    /// <param name="pTimeStamp">time stamp</param>
    /// <returns>success/failure</returns>
    static bool Master_StoreBinaryOutputEvent(MDNPDatabase db, ushort pointNumber, bool value, DNPDatabaseFlags flags, bool isEvent, TMWTime pTimeStamp)
    {
      Console.WriteLine("Master:StoreBinaryOutput: Point Number {0} Value {1} Flags {2}", pointNumber, value, flags);
      return true;
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
          ProtocolDataObject pdo = protocolBuffer.getPdoAtIndex(i);
          if (pdo != null)
          {
            // Don't display physical and target layer trace
            if ((pdo.SourceIDasUint & ((UInt32)SCLDIAG_ID.PHYS | (UInt32)SCLDIAG_ID.TARGET)) == 0)
              protocolAnalyzer.AppendText(pdo.ProtocolText);
            SendMessage(new HandleRef(protocolAnalyzer, protocolAnalyzer.Handle), WM_VSCROLL, SB_BOTTOM, 0);
          }
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
      
    private void IntegrityPollTimer_Tick(object sender, EventArgs e)
    {
      if (integrityPollCount++ < integrityPollInterval)
      {
        IntegrityProgressBar.Increment(1);
      }
      else
      {
        // time to do a poll

        Integrity_Click(sender, e);

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
      request.IntegrityPoll(true);
    }

    private void Event_Click(object sender, EventArgs e)
    {
      protocolBuffer.Insert("\nRequested Event Poll\n");

      MDNPRequest request = new MDNPRequest(masterSesn);
      request.ReadClass(MDNPRequest.DNP_QUALIFIER.Q_ALL_POINTS, 0, false, true, true, true);
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

    // Browse local file directory
    private void BrowseLocalBT_Click(object sender, EventArgs e)
    { 
      if (this.openFileDialog1.ShowDialog() != DialogResult.OK) return;
      LocalFileNameTB.Text = this.openFileDialog1.FileName;
    }

    // Browse remote (outstation) file directory 
    private void BrowseRemoteBT_Click(object sender, EventArgs e)
    {
      remoteBrowseWindow.UserName = UserNameTB.Text;
      remoteBrowseWindow.Password = PasswordTB.Text;
      remoteBrowseWindow.Clear();
      remoteBrowseWindow.RefreshDirectory();

      if (remoteBrowseWindow.ShowDialog() != DialogResult.OK) return;

      RemoteFileNameTB.Text = remoteBrowseWindow.FileName;
    }

    // Read File from remote device (outstation)
    private void ReadBt_Click(object sender, EventArgs e)
    {
      MDNPRequest request = new MDNPRequest(masterSesn);
      // Register to receive event when each response is received for this request, or the request times out.
      request.RequestEvent += new MDNPRequest.RequestEventDelegate(request_FileRequestEvent);

      // Set up to show progress
      FileProgress.Value = 0;
      FileProgress.Maximum = 50;

      // Send request
      request.FileCopyFromRemote(LocalFileNameTB.Text, RemoteFileNameTB.Text, UserNameTB.Text, PasswordTB.Text);
    }

    // Write file to remote device (outstation)
    private void WriteBt_Click(object sender, EventArgs e)
    {
      MDNPRequest request = new MDNPRequest(masterSesn);

      // Register to receive event when each response is received for this request, or the request times out.
      request.RequestEvent += new MDNPRequest.RequestEventDelegate(request_FileRequestEvent);

      // Set up to show progress
      FileProgress.Value = 0;
      FileProgress.Maximum = 50;
      bool result = request.FileCopyToRemote(LocalFileNameTB.Text, RemoteFileNameTB.Text, UserNameTB.Text, PasswordTB.Text, 0x1ff);
      
      if (result == false)
      {
        protocolBuffer.Insert("ERROR copying file\n"); 
      }
    }
  }
}