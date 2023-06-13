using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using TMW.SCL;
using TMW.SCL.IEC60870_5.I104;
using TMW.SCL.IEC60870_5.I104.Slave;
using TMW.SCL.IEC60870_5.I14.Slave;
using TMW.SCL.ProtocolAnalyzer;

namespace I104slaveGUI
{
    public partial class SlaveForm : Form
    {
        private const int WmVscroll = 0x115;
        private const int SbBottom = 7;
        private const int WmSetredraw = 0x000B;
        private const int EmSeteventmask = 0x0431;
        private static TMWApplication _pAppl;
        private readonly ProtocolBuffer _protocolBuffer;
        private int _oldEventMask;
        private NumericUpDown[] _analogInputs;
        private CheckBox[] _binaryInputs;
        private Button[] _counters;
        private S14SimDatabase _db;
        private bool _pauseAnalyzer;
        private I104Channel _slaveChan104;
        private S104Sector _slaveSctr104;
        private S104Session _slaveSesn104;

        public SlaveForm()
        {
            var applBuilder = new TMWApplicationBuilder();

            // This causes application to process received data and timers.
            _pAppl = TMWApplicationBuilder.getAppl();
            _pAppl.EnableEventProcessor = true;
            _pAppl.InitEventProcessor();

            // This enables a Forms timer to process protocol data output.
            // For non Forms applications see I104SlaverDatabaseEvents example for getting protocol data.
            _protocolBuffer = TMWApplicationBuilder.getProtocolBuffer();
            _protocolBuffer.ProtocolDataReadyEvent += ProtocolEvent;
            _protocolBuffer.EnableCheckForDataTimer = true;

            InitializeComponent();

            closePB.Enabled = false;
        }

        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern int SendMessage(HandleRef hWnd, int msg, int wParam, int lParam);

        private void UpdateDbEvent(TMWSimPoint simPoint)
        {
            if (InvokeRequired)

                BeginInvoke(new UpdatePointDelegate(UpdateDbEvent), simPoint);

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
            }
        }

        private void ScrollToBottom()
        {
            SendMessage(new HandleRef(protocolAnalyzer, protocolAnalyzer.Handle), WmVscroll, SbBottom, 0);
        }

        private void BeginUpdate()
        {
            // Prevent the control from raising any events
            _oldEventMask = SendMessage(new HandleRef(protocolAnalyzer, protocolAnalyzer.Handle), EmSeteventmask, 0, 0);

            // Prevent the control from redrawing itself
            SendMessage(new HandleRef(protocolAnalyzer, protocolAnalyzer.Handle), WmSetredraw, 0, 0);
        }

        private void EndUpdate()
        {
            // Allow the control to redraw itself
            SendMessage(new HandleRef(protocolAnalyzer, protocolAnalyzer.Handle), WmSetredraw, 1, 0);

            // Allow the control to raise event messages
            SendMessage(new HandleRef(protocolAnalyzer, protocolAnalyzer.Handle), EmSeteventmask, 0, _oldEventMask);
        }

        private void RemoveTopLines(int numLines)
        {
            var lastLine = protocolAnalyzer.Lines.GetLength(0) - 1;
            if (numLines < 1)
            {
                return;
            }
            if (numLines > lastLine)
            {
                numLines = lastLine;
            }

            var startChar = protocolAnalyzer.GetFirstCharIndexFromLine(0);
            var endChar = protocolAnalyzer.GetFirstCharIndexFromLine(numLines);

            var b = protocolAnalyzer.ReadOnly;
            protocolAnalyzer.ReadOnly = false;
            protocolAnalyzer.Select(startChar, endChar - startChar);
            protocolAnalyzer.SelectedRtf = "";
            protocolAnalyzer.ReadOnly = b;
        }

        private void CustomizeDatabase()
        {
            uint i;

            _binaryInputs = new CheckBox[3] { BinaryInput0, BinaryInput1, BinaryInput2 };
            _analogInputs = new NumericUpDown[3] { AnalogInput0, AnalogInput1, AnalogInput2 };
            _counters = new Button[3] { Counter0, Counter1, Counter2 };

            // This Clear is not required here since the database is empty,
            // but this would delete all points from the database.
            _db.Clear();

            TMW_GROUP_MASK groupMask;
            uint[] pmencIoAs = { 0, 0, 0, 0 };
            TMWSimPoint pt;
            for (i = 0; i < 3; i++)
            {
                groupMask = TMW_GROUP_MASK.GENERAL;
                pt = _db.AddMspPoint(100 + i, groupMask, 0, 0);
                pt.Tag = _binaryInputs[i];
                _binaryInputs[i].Tag = pt;


                pmencIoAs[0] = 1100 + (i * 4); //lowLimitIOA
                pmencIoAs[1] = 1100 + (i * 4) + 1; //highLimitIOA
                pmencIoAs[2] = 1100 + (i * 4) + 2; //thresholdIOA
                pmencIoAs[3] = 1100 + (i * 4) + 3; //smoothingIOA

                S14SimPoint simPoint;
                var pacncIoa = 1200 + i; //pacnaIOA
                groupMask = TMW_GROUP_MASK.MASK_CYCLIC;
                simPoint = _db.AddMmencPoint(700 + i, pacncIoa, pmencIoAs, groupMask, 0, 0);

                if (i == 1)
                    simPoint.TimeFormat = (uint)IEC_TIME_FORMAT.TIME_NONE;
                else
                    simPoint.TimeFormat = (uint)IEC_TIME_FORMAT.TIME_56;

                simPoint.Tag = _analogInputs[i];
                _analogInputs[i].Tag = simPoint;

                groupMask = TMW_GROUP_MASK.GENERAL;
                pt = _db.AddMitnaPoint(800 + i, groupMask, 0, 0);
                pt.Tag = _counters[i];
                _counters[i].Tag = pt;
            }

            _db.AddMmencPoint(300, 400, new uint[] { 10, 20, 50, 700 }, TMW_GROUP_MASK.MASK_2,0, 55.55f);

        }

        private void BinaryInput_CheckedChanged(object sender, EventArgs e)
        {
            var pt = ((sender as Control).Tag) as S14SimMspna;
            if (pt == null)
                return;

            pt.Value = (sender as CheckBox).Checked;
            // pt.Flags = 0; // set flags to valid
            // pt.Flags = (byte)Defs.QUALITY.IV; // set flags to invalid

            // This would add an event with the value, flags and timestamp in the database.
            // pt.AddEvent();

            // This example uses the sector add event method to specify a specific value, flags, time stamp and reason for an event 
            // instead of using the AddEvent method which uses the values in the database
            // TMWTime timeStamp = new TMWTime();
            // timeStamp.Hour = 12;
            // timeStamp.Minute = 22;
            // timeStamp.Second = 0;
            // timeStamp.Millisecond = 0;
            // timeStamp.Day = 1;
            // timeStamp.Month = 3;
            // timeStamp.Invalid = false;
            // slaveSctr104.MspAddEvent(100, false, (byte)Defs.QUALITY.IV, timeStamp, TMW_CHANGE_REASON.SPONTANEOUS); 

            // This example uses the sector add event method.
            // It allows you to specify a value, flags and reason, and in this example uses the current sector time.
            // The sector time will reflect time syncs and the whether the time is valid or not
            // You do NOT need to use the value and flags from the point in the database, though that often makes sense.
            var timeStamp = _slaveSctr104.GetTimeStamp();
            _slaveSctr104.MspAddEvent(pt.IOA, pt.Value, pt.Flags, timeStamp, TMW_CHANGE_REASON.SPONTANEOUS);
        }

        private void AnalogInput_ValueChanged(object sender, EventArgs e)
        {
            var pt = ((sender as Control).Tag) as S14SimMmenc;
            if (pt == null)
                return;

            pt.Value = (float)(sender as NumericUpDown).Value;
            pt.AddEvent();
        }

        private void Counter_Click(object sender, EventArgs e)
        {
            var pt = ((sender as Control).Tag) as S14SimMitna;
            if (pt == null)
                return;

            pt.Value += 1;
            (sender as Button).Text = pt.Value.ToString();
            pt.AddEvent();
        }

        private void ProtocolEvent(ProtocolBuffer buf)
        {
            if (!_pauseAnalyzer)
            {
                buf.Lock();
                for (var i = buf.LastProvidedIndex; i < buf.LastAddedIndex; i++)
                {
                    var pdo = _protocolBuffer.getPdoAtIndex(i);
                    if (pdo != null)
                    {
                        // Don't display physical and target layer trace
                        //if ((pdo.SourceIDasUint & ((UInt32)SCLDIAG_ID.PHYS | (UInt32)SCLDIAG_ID.TARGET)) == 0)
                        protocolAnalyzer.AppendText(pdo.ProtocolText);
                    }
                    SendMessage(new HandleRef(protocolAnalyzer, protocolAnalyzer.Handle), WmVscroll, SbBottom, 0);
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
                if (_pauseAnalyzer)
                {
                    // it's paused, so unpause it
                    _pauseAnalyzer = false;
                    protocolAnalyzer.BackColor = Color.Gainsboro;
                }
                else
                {
                    // it's not paused, so pause it
                    _pauseAnalyzer = true;
                    protocolAnalyzer.BackColor = Color.MistyRose;
                }
            }
        }

        private void SlaveForm_Load(object sender, EventArgs e)
        {
        }

        private void openPB_Click(object sender, EventArgs e)
        {
            _slaveChan104 = new I104Channel(TMW_CHANNEL_OR_SESSION_TYPE.SLAVE);
            _slaveChan104.ChannelConnectStateEvent += slaveChan_ChannelConnectStateEvent;

            _slaveChan104.Type = WINIO_TYPE.TCP;
            _slaveChan104.Name = ".NET I104 Slave"; /* name displayed in analyzer window */
            _slaveChan104.WinTCPipAddress = "*.*.*.*";
            _slaveChan104.WinTCPipPort = 2404;
            _slaveChan104.WinTCPmode = TCP_MODE.DUAL_ENDPOINT;
            _slaveChan104.OpenMasterMonitorSession = true;
            _slaveChan104.Active = true;
            _slaveChan104.T0ConnectPeriod = 100000;
            _slaveChan104.T1AckPeriod = 50000;
            _slaveChan104.T2SFramePeriod = 20000;
            _slaveChan104.T3TestPeriod = 80000;
            // Register to receive channel statistics
            //slaveChan104.ChannelStatisticsEvent += new TMWChannel.ChannelStatisticsEventDelegate(slaveChan_ChannelStatisticsEvent);
            _slaveChan104.OpenChannel();

            _slaveSesn104 = new S104Session(_slaveChan104);
            _slaveSesn104.SessionOnlineStateEvent += slaveSesn_SessionOnlineStateEvent;

            // Register to process Private Custom ASDUs that the library does not implement
            // This is very rarely required.
            //slaveSesn104.ProcessCustomASDUReqEvent += new S104Session.ProcessCustomASDUReqDelegate(slaveSesn104_ProcessCustomASDUReqEvent);
            //slaveSesn104.BuildCustomASDURespEvent += new S104Session.BuildCustomASDURespDelegate(slaveSesn104_BuildCustomASDURespEvent);

            // Register to receive session statistics
            //slaveSesn104.SessionStatisticsEvent += new TMWSession.SessionStatisticsEventDelegate(slaveSesn_SessionStatisticsEvent);
            _slaveSesn104.OpenSession();

            _slaveSctr104 = new S104Sector(_slaveSesn104);
            _slaveSctr104.AsduAddress = 550;
            _slaveSctr104.MmencTimeFormat = IEC_TIME_FORMAT.TIME_56;
            _slaveSctr104.CicnaTimeFormat = IEC_TIME_FORMAT.TIME_56;

            // Register to receive sector statistics
            //slaveSctr104.SectorStatisticsEvent += new TMWSector.SectorStatisticsEventDelegate(slaveSctr_SectorStatisticsEvent);
            _slaveSctr104.OpenSector();
            _db = (S14SimDatabase)_slaveSctr104.SimDatabase;
            //_db.AddMmencPoint(100, 1,new uint[]{22,11} , TMW_GROUP_MASK.GENERAL, 0,1000);
            // Register to receive notification of database changes
            _db.UpdateDBEvent += UpdateDbEvent;

            CustomizeDatabase();
            openPB.Enabled = false;
            closePB.Enabled = true;
        }


        private void closePB_Click(object sender, EventArgs e)
        {
            _slaveSctr104.CloseSector();
            _slaveSesn104.CloseSession();
            _slaveChan104.CloseChannel();

            openPB.Enabled = true;
            closePB.Enabled = false;
        }

        private void UpdateState(bool channel, bool state)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new UpdateStateDelegate(UpdateState), channel, state);
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

        private void slaveChan_ChannelConnectStateEvent(TMWChannel channel, bool state)
        {
            // state == true indicates channel is now connected
            UpdateState(true, state);
        }

        private void slaveSesn_SessionOnlineStateEvent(TMWSession session, bool state)
        {
            // state == true indicates session is now online 
            UpdateState(false, state);
        }

        // This will save the protocol log to a file
        private void SaveLog_Click(object sender, EventArgs e)
        {
            _protocolBuffer.UpdateDiagIdMask(SCLDIAG_ID.ALL_BUT_TARGET, true);
            _protocolBuffer.UpdateDiagIdMask(SCLDIAG_ID.PHYS, false);
            _protocolBuffer.SaveAsText("104SlaveGUI.log", "create", "begin", "end");
        }

        private delegate void UpdatePointDelegate(TMWSimPoint simPoint);

        private delegate void UpdateStateDelegate(bool channel, bool state);

        private void button2_Click(object sender, EventArgs e)
        {
            protocolAnalyzer.Clear();
        }
    }
}