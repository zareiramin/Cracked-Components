using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using TMW.SCL;
using TMW.SCL.MB;
using TMW.SCL.MB.Slave;
using TMW.SCL.ProtocolAnalyzer;

namespace SMBSimulator
{
  public partial class SlaveForm : Form
  {

    private delegate void setLabelDelegate(TMWSimPoint pt);

    private readonly TMWApplication pAppl;
    private readonly MBChannel slaveChan;
    private readonly SMBSession slaveSesn;
    
    /// <summary>
    /// Construct the main form for this application
    /// </summary>
    public SlaveForm()
    {
      TMWApplicationBuilder applBuilder = new TMWApplicationBuilder();
      pAppl = TMWApplicationBuilder.getAppl();
      pAppl.EnableEventProcessor = true;
      pAppl.InitEventProcessor();

      InitProtocolBufferSupport();
      InitializeComponent();

      // create, initialize and open channel
      slaveChan = new MBChannel(TMW_CHANNEL_OR_SESSION_TYPE.SLAVE);

      bool bSerial = false;
      if (bSerial)
      {
        slaveChan.Type = WINIO_TYPE.RS232;
        slaveChan.Win232comPortName = "COM3";
        slaveChan.Win232baudRate = "9600";
        slaveChan.Win232numDataBits = RS232_DATA_BITS.BITS_8;
        slaveChan.Win232numStopBits = RS232_STOP_BITS.BITS_1;
        slaveChan.Win232parity = RS232_PARITY.EVEN;
        slaveChan.Win232portMode = RS232_PORT_MODE.NONE;

        slaveChan.LinkConfigType = MBChannel.LINK_TYPE.ASCII;
      }
      else
      {
        slaveChan.Type = WINIO_TYPE.TCP;
        slaveChan.WinTCPipAddress = "127.0.0.1";
        slaveChan.WinTCPipPort = 502;
        slaveChan.WinTCPmode = TCP_MODE.SERVER;
      }

      slaveChan.Name = ".NET Modbus Slave";  // name displayed in analyzer window 
      slaveChan.OpenChannel();

      // create, initialize and open session
      slaveSesn = new SMBSession(slaveChan);
      slaveSesn.OpenSession();

      // initialize built in database
      customizeDatabase();
    }

    /// <summary>
    /// Set up our database
    /// </summary>
    private void customizeDatabase()
    {
      SMBSimDatabase db = slaveSesn.SimDatabase as SMBSimDatabase;
      slaveSesn.SimDatabase.UpdateDBEvent += new TMWSimDataBase.UpdateDBEventDelegate(SimDatabase_UpdateDBEvent);

      // This Clear is not required here since the database is empty,
      // but this would delete all points from the database.
      db.Clear();

      // discrete inputs
      disc0.Tag = db.addDisc(0, false);
      disc0.Checked = false;

      disc1.Tag = db.addDisc(1, false);
      disc1.Checked = false;

      disc2.Tag = db.addDisc(2, false);
      disc2.Checked = false;

      // input registers
      SMBSimIReg iregPt = db.addIreg(0, 0);
      ireg0.Tag = iregPt;
      ireg0.Value = 0;
      iregPt.Tag = ireg0Value;
      setIreg(iregPt);

      iregPt = db.addIreg(1, 0);
      ireg1.Tag = iregPt;
      ireg1.Value = 0;
      iregPt.Tag = ireg1Value;
      setIreg(iregPt);

      iregPt = db.addIreg(2, 0);
      ireg2.Tag = iregPt;
      ireg2.Value = 0;
      iregPt.Tag = ireg2Value;
      setIreg(iregPt);

      // holding registers
      SMBSimHReg hregPt = db.addHreg(0, 0);
      hregPt.Tag = hreg0;
      setHreg(hregPt);

      hregPt = db.addHreg(1, 0);
      hregPt.Tag = hreg1;
      setHreg(hregPt);

      hregPt = db.addHreg(2, 0);
      hregPt.Tag = hreg2;
      setHreg(hregPt);

      hregPt = db.addHreg(3, 0);
      hregPt.Tag = hreg3;
      setHreg(hregPt);

      hregPt = db.addHreg(4, 0);
      hregPt.Tag = hreg4;
      setHreg(hregPt);

      hregPt = db.addHreg(5, 0);
      hregPt.Tag = hreg5;
      setHreg(hregPt);

      hregPt = db.addHreg(6, 0);
      hregPt.Tag = hreg6;
      setHreg(hregPt);

      hregPt = db.addHreg(7, 0);
      hregPt.Tag = hreg7;
      setHreg(hregPt);

      hregPt = db.addHreg(8, 0);
      hregPt.Tag = hreg8;
      setHreg(hregPt);

      hregPt = db.addHreg(9, 0);
      hregPt.Tag = hreg9;
      setHreg(hregPt);


      // coils
      SMBSimCoil coilPt = db.addCoil(0, false);
      coilPt.Tag = coil0;
      setCoil(coilPt);

      coilPt = db.addCoil(1, false);
      coilPt.Tag = coil1;
      setCoil(coilPt);

      coilPt = db.addCoil(2, false);
      coilPt.Tag = coil2;
      setCoil(coilPt);

      coilPt = db.addCoil(3, false);
      coilPt.Tag = coil3;
      setCoil(coilPt);

      coilPt = db.addCoil(4, false);
      coilPt.Tag = coil4;
      setCoil(coilPt);

      coilPt = db.addCoil(5, false);
      coilPt.Tag = coil5;
      setCoil(coilPt);

      coilPt = db.addCoil(6, false);
      coilPt.Tag = coil6;
      setCoil(coilPt);

      coilPt = db.addCoil(7, false);
      coilPt.Tag = coil7;
      setCoil(coilPt);

      coilPt = db.addCoil(8, false);
      coilPt.Tag = coil8;
      setCoil(coilPt);

      coilPt = db.addCoil(9, false);
      coilPt.Tag = coil9;
      setCoil(coilPt);
    }

    private void setIreg(TMWSimPoint pt)
    {
      if (InvokeRequired)
      {
        BeginInvoke(new setLabelDelegate(setIreg), new object[] { pt });
      }

      Label ctrl = (pt.Tag as Label);
      ctrl.Text = string.Format("Input Register {0}: {1}", pt.PointNumber.ToString(), (pt as SMBSimIReg).Value.ToString());
    }

    private void setHreg(TMWSimPoint pt)
    {
      if (InvokeRequired)
      {
        BeginInvoke(new setLabelDelegate(setHreg), new object[] { pt });
      }
      Label ctrl = (pt.Tag as Label);
      ctrl.Text = string.Format("Holding Register {0}: {1}", pt.PointNumber.ToString(), (pt as SMBSimHReg).Value.ToString());
    }

    private void setCoil(TMWSimPoint pt)
    {
      if (InvokeRequired)
      {
        BeginInvoke(new setLabelDelegate(setCoil), new object[] { pt });
      }
      Label ctrl = (pt.Tag as Label);
      ctrl.Text = string.Format("Coil {0}: {1}", pt.PointNumber.ToString(), (pt as SMBSimCoil).Value.ToString());
    }

    void SimDatabase_UpdateDBEvent(TMWSimPoint simPoint)
    {
      if (simPoint == null || simPoint.Tag == null)
        return;

      if (simPoint is SMBSimHReg)
      {
        setHreg(simPoint);
      }

      if (simPoint is SMBSimCoil)
      {
        setCoil(simPoint);
      }
    }

    private void closePB_Click(object sender, EventArgs e)
    {
      slaveSesn.CloseSession();
      slaveChan.CloseChannel();

      Close();
    }

    private void disc_CheckedChanged(object sender, EventArgs e)
    {
      CheckBox ctrl = sender as CheckBox;
      SMBSimDInput pt = (ctrl.Tag as SMBSimDInput);
      if (pt != null)
        pt.Value = ctrl.Checked;
    }

    private void ireg_ValueChanged(object sender, EventArgs e)
    {
      TrackBar ctrl = sender as TrackBar;
      SMBSimIReg pt = (ctrl.Tag as SMBSimIReg);
      if (pt != null)
      {
        pt.Value = (ushort)ctrl.Value;
        setIreg(pt);
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
    /// Initialize the protocol buffer for use by this application
    /// </summary>
    private void InitProtocolBufferSupport()
    {
      protocolBuffer = TMWApplicationBuilder.getProtocolBuffer();
      protocolBuffer.ProtocolDataReadyEvent += new TMW.SCL.ProtocolAnalyzer.ProtocolBuffer.ProtocolDataReadyEventDelegate(ProtocolEvent);
      protocolBuffer.EnableCheckForDataTimer = true;
    }

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