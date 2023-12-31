
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
using TMW.SCL.DNP;
using TMW.SCL.DNP.Slave;

namespace DNPSlaveSA
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
    private SDNPSimDatabase db;
    private SDNPSession slaveSesn;
    private DNPChannel slaveChan;
    private bool pauseAnalyzer;

    public SlaveForm()
    {
      // Change this to true to connect over COM ports
      bool bSerialMode = false;

      // Add Multiple Secure Authentication users, in addition to the Common user
      bool configureAdditionalUsers = true; 

      TMWApplicationBuilder applBuilder = new TMWApplicationBuilder();

      // This causes application to process received data and timers.
      pAppl = TMWApplicationBuilder.getAppl();
      pAppl.EnableEventProcessor = true;
      pAppl.InitEventProcessor();

      // This enables a Forms timer to process protocol data output.
      // For non Forms applications see DNPSlaveDatabaseEvents example for getting protocol data.
      protocolBuffer = TMWApplicationBuilder.getProtocolBuffer();
      protocolBuffer.ProtocolDataReadyEvent += new ProtocolBuffer.ProtocolDataReadyEventDelegate(ProtocolEvent);
      protocolBuffer.EnableCheckForDataTimer = true;

      InitializeComponent();

      this.timer1.Interval = 3000;
      this.timer1.Tick += new System.EventHandler(UpdateAuthUsers);
      this.timer1.Enabled = true;
      this.timer1.Start();

      slaveChan = new DNPChannel(TMW_CHANNEL_OR_SESSION_TYPE.SLAVE);

      slaveChan.ChannelConnectStateEvent += new TMWChannel.ChannelConnectStateEventDelegate(slaveChan_ChannelConnectStateEvent);
      
      if (bSerialMode)
      {
        slaveChan.Type = WINIO_TYPE.RS232;
        slaveChan.Win232comPortName = "COM4";
        slaveChan.Win232baudRate = "9600";
        slaveChan.Win232numDataBits = RS232_DATA_BITS.BITS_8;
        slaveChan.Win232numStopBits = RS232_STOP_BITS.BITS_1;
        slaveChan.Win232portMode = RS232_PORT_MODE.NONE;
      }
      else
      {
        slaveChan.Type = WINIO_TYPE.TCP; // is equivalent to UDP_TCP
        slaveChan.WinTCPipAddress = "127.0.0.1";
        slaveChan.WinTCPipPort = 20000;
        slaveChan.WinTCPmode = TCP_MODE.SERVER;
        
        // if TLS should be enabled, uncomment the following lines 
        // requires ssleay32.dll and libeay32.dll from OpenSSL 
        // Also requires Diffie-Helman ie dh1024.pem file and other files
        // which may be specified here.

        // SSL/TLS Enabled on this channel.
        //slaveChan.TlsEnabled = true;

        // TLS Common Name 
        //slaveChan.TlsCommonName = "TLS";

        // NOTE: Then following use the path to directory where the .NET Components Installer puts the default certificates.
        // For the Source Code Library version or if you want to use certificates from a different location you should modify the following.

        // file containing Certificate Authority Certificates 
        //slaveChan.CaFile = "C:\\Users\\Public\\Documents\\Triangle MicroWorks\\TMWCertificates\\ca_public\\tmw_sample_ca_rsa_public_certificate.pem";

        // file containing revocation list
        //slaveChan.CaCrlFile = "C:\\Users\\Public\\Documents\\Triangle MicroWorks\\TMWCertificates\\ca_public\\tmw_sample_ca_certificate_revocation_list.pem";

        // file containing Diffie-Hellman parameters for TLS cipher suites
        //slaveChan.DhFileName = "C:\\Users\\Public\\Documents\\Triangle MicroWorks\\TMWCertificates\\DH\\dh_params.pem";

        // Max PDUs before forcing cipher renegotiation 
        //slaveChan.TlsRenegotiationCount = 5000;
        // Max seconds before forcing cipher renegotiation    
        //slaveChan.TlsRenegotiationSeconds = 600000;

        //slaveChan.TlsRsaCertificateFile = "C:\\Users\\Public\\Documents\\Triangle MicroWorks\\TMWCertificates\\server_user\\tmw_sample_tls_rsa_public_cert.pem";
        //slaveChan.TlsRsaPrivateKeyFile = "C:\\Users\\Public\\Documents\\Triangle MicroWorks\\TMWCertificates\\server_user\\tmw_sample_tls_rsa_private_key.pem";
        //slaveChan.TlsRsaPrivateKeyPassPhrase = "triangle";
      }

      slaveChan.Name = ".NET DNP Slave";  /* name displayed in analyzer window */

      // Register to receive channel statistics
      //slaveChan.ChannelStatisticsEvent += new TMWChannel.ChannelStatisticsEventDelegate(slaveChan_ChannelStatisticsEvent);
      slaveChan.OpenChannel();

      slaveSesn = new SDNPSession(slaveChan);

      // If using TCP the DNP Spec requires keep alives to be configured in order to detect disconnects.
      if (!bSerialMode)
        slaveSesn.LinkStatusPeriod = 30000;

      // Set this to true to enable DNP Secure Authentication 
      slaveSesn.AuthenticationEnabled = true;

      // This configures a single user with an update key that matches the default one in the test harness.
      // DO NOT USE THIS UPDATE KEY other than testing with the test harness using it's default key. 
      // Change this to a secret key and provision the master (including the test harness) to use the same key.
      // Or with SAv5 the Authority can direct the master to add a User and Update Key on the outstation over DNP.

      TMWCryptoDataBase cryptoDb = slaveSesn.AuthCryptoDb;
      TMWAuthUser authUser = new TMWAuthUser();
      authUser.UserName = "Common";
      authUser.UserRole = TMWAUTH_USERROLE.SINGLEUSER;
      authUser.UserNumber = 1;
      // Default update key value the test harness uses for user 1 */
      authUser.UpdateKey = TMWTypeConverter.ConvertStringToByteArrayHex("49 C8 7D 5D 90 21 7A AF EC 80 74 eb 71 52 fd b5");
      cryptoDb.AuthUserAdd(authUser);

      // Add more users, choosing the user number and key, Outstation must be configured to match. */
      if (configureAdditionalUsers)
      {
        authUser.UserName = "JohnSmith";
        authUser.UserRole = TMWAUTH_USERROLE.VIEWER;
        authUser.UserNumber = 20;
        // Default update key value the test harness uses */
        authUser.UpdateKey = TMWTypeConverter.ConvertStringToByteArrayHex("00 11 22 33 44 55 66 77 88 99 aa bb cc dd ee ff");
        cryptoDb.AuthUserAdd(authUser);

        authUser.UserName = "DavidJohnson";
        authUser.UserRole = TMWAUTH_USERROLE.ENGINEER;
        authUser.UserNumber = 300;
        // Should not use the same update key here, but this is the default value the test harness uses */
        authUser.UpdateKey = TMWTypeConverter.ConvertStringToByteArrayHex("00 11 22 33 44 55 66 77 88 99 aa bb cc dd ee ff");
        cryptoDb.AuthUserAdd(authUser);
      }

      // Configure other values to the same values that the Test Harness Outstation uses by default
      cryptoDb.AuthOSName = "SDNP Outstation";
      cryptoDb.AuthoritySymCertKey = TMWTypeConverter.ConvertStringToByteArrayHex("01 02 03 04 05 06 07 08 09 00 01 02 03 04 05 06 01 02 03 04 05 06 07 08 09 00 01 02 03 04 05 06");
      cryptoDb.AuthOSAsymPrvKey = "TMWTestOSAsymPrvKey.pem";
      cryptoDb.AuthorityAsymPubKey = "TMWTestRsa2048PubKey.pem";

      // The following are defaults, but are here to show some things that could be configured
      slaveSesn.AuthAggressiveMode = true;

      // Number of Authentication Messages before Session Keys are considered invalid.
      slaveSesn.AuthMaxKeyChangeCount = 2000;

      // 30 minutes till Session Keys are considered invalid
      slaveSesn.AuthKeyChangeInterval = 30*60*1000;

      // 2 seconds for an Authentication Request to timeout.
      slaveSesn.AuthReplyTimeout = 2 * 1000;

      // If SAv2 AND SAv5 are supported, you can choose which to use on an association.
      // SAv5 is an additional component that may be purchased and licensed separately.
      // If configure to use SAv5 and you do not have a license for it, the session will use SAv2.
      // SAv2 does NOT support adding Users over DNP. (It cannot receive an Update User Status g120v10 request)
      slaveSesn.AuthOperateInV2Mode = false;

      slaveSesn.BinaryInputScanPeriod = 500;
      slaveSesn.AnalogInputScanPeriod = 500;
      slaveSesn.BinaryCounterScanPeriod = 500;

      // Register to receive online/offline events 
      slaveSesn.SessionOnlineStateEvent += new TMWSession.SessionOnlineStateEventDelegate(slaveSesn_SessionOnlineStateEvent);

      // Register to receive session statistics
      slaveSesn.SessionStatisticsEvent += new TMWSession.SessionStatisticsEventDelegate(slaveSesn_SessionStatisticsEvent);
     
      if (!slaveSesn.OpenSession())
      {
        // failed  
        protocolBuffer.Insert("Error: Failed to open slave session\n");
        return;
      }

      // Register to receive notification of database changes
      db = (SDNPSimDatabase)slaveSesn.SimDatabase;
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

    private void updateCROB(TMWSimPoint simPoint)
    {
      bool crobVal = (simPoint as SDNPSimBinOut).Value;
      string strVal = crobVal ? "On" : "Off";
      Color textColor = crobVal ? Color.ForestGreen : Color.Red;

      switch (simPoint.PointNumber)
      {
        case 0:
          this.DO0Val.Text = strVal;
          this.DO0Val.ForeColor = textColor;
          break;
        case 1:
          this.DO1Val.Text = strVal;
          this.DO1Val.ForeColor = textColor;
          break;
        case 2:
          this.DO2Val.Text = strVal;
          this.DO2Val.ForeColor = textColor;
          break;
        default:
          protocolBuffer.Insert("Got update for uknown Binary Output point");
          break;
      }
    }

    private void UpdateAnlgOut(TMWSimPoint simPoint)
    {
      switch (simPoint.PointNumber)
      {
        case 0:
          this.AO0Val.Text = (simPoint as SDNPSimAnlgOut).Value.ToString();
          break;
        case 1:
          this.AO1Val.Text = (simPoint as SDNPSimAnlgOut).Value.ToString();
          break;
        case 2:
          this.AO2Val.Text = (simPoint as SDNPSimAnlgOut).Value.ToString();
          break;
        default:
          protocolBuffer.Insert("Got update for unknown Analog Output point");
          break;
      }
    }

    private delegate void UpdatePointDelegate(TMWSimPoint simPoint);
    private void UpdateDBEvent(TMWSimPoint simPoint)
    {
      if (this.InvokeRequired)

        this.BeginInvoke(new UpdatePointDelegate(UpdateDBEvent), new object[] { simPoint });

      else
      {
        switch (simPoint.PointType)
        {
          case 10:
            // Binary Output (CROB)
            updateCROB(simPoint);
            break;
          case 40:
            // Analog Output
            UpdateAnlgOut(simPoint);
            break;
          case 121:
            // Security Statistic 
            break;
          default:
            break;
        }
      }
    }

    private void customizeDatabase()
    {
      int i;

      // This Clear is not required here since the database is empty,
      // but this would delete all points from the database.
      db.Clear();

      /* Add 3 of each of the following types:
       *  Binary Input
       *    Value = False, Flags = onLine, class = 1
       *  Binary Output
       *    Value = False, Flags = onLine, class = 3, ControlMask = 0x3ff (allow all control operations)
       *  Analog Input
       *    Value = 0, Flags = 0, classMask = 2, Deadband = 5
       *  Analog Output
       *    Value = 0, Flags = onLine; classMask = 3
       *  Binary Counter
       *    Value = 0, Flags = onLIne, classMask = 2, frozenClassMask = 2
       */
      for (i = 0; i < 3; i++)
      {
        db.AddBinIn(false, 1, TMW_CLASS_MASK.ONE);
        db.AddBinOut(false, 1, TMW_CLASS_MASK.THREE, 0x3f);
        db.AddAnlgIn(0, 1, TMW_CLASS_MASK.TWO, 5);
        db.AddAnlgOut(0, 1, TMW_CLASS_MASK.THREE);
        db.AddBinCntr(0, 1, TMW_CLASS_MASK.TWO, TMW_CLASS_MASK.TWO);
        db.AddString("testString", TMW_CLASS_MASK.ONE);
      }

      // Point index and variation are used to specify particular device attributes
      // Point index 0 is used to specify the standardized set of device attributes
      // Variation 255 is used to request a list of the attribute variation numbers supported by an outstation.
      // Variation 254 is used to request all of the attribute objects from an outstation in a single response.

      // This sample supports the following device attribute variations
      // 213 number of outstation defined data set prototypes
      // 215 number of outstation defined data sets
      // 242 Device manufacturer�s software version
      // 243 Device manufacturer�s hardware version 
      // 246 User-assigned ID code/number 
      // 248 Device serial number 
      // 250 Device manufacturer�s product name and model
      // 252 Device manufacturer�s name

      Byte propertyValue = 0x00;  // all of these are read only device attributes, 0x01 is writable
      UInt16 pointIndex = 0;      // all of these will be added to point index 0, the standardized set

      DNPDataDeviceAttrValue value = new DNPDataDeviceAttrValue(DNP_DEVICE_ATTR_TYPE.UINT, 4, 0);
      db.AddDeviceAttr(pointIndex, (byte)DNP_DEVICE_ATTR_VAR.OUTSTATION_PROTOS, propertyValue, value); 
      // If you add 5 outstation data set protoypes you could change this device attribute value later as follows
      // value = new DNPDataDeviceAttrValue(DNP_DEVICE_ATTR_TYPE.INT, 4, 5);
      // db.SetDeviceAttr(0, 213, value);

      // This overloaded method will try to convert the string "0" into an unsigned integer.
      value = new DNPDataDeviceAttrValue(DNP_DEVICE_ATTR_TYPE.UINT, 4, "0");
      db.AddDeviceAttr(pointIndex, 215, propertyValue, value); 

      // Note: length doesn't really matter for type VSTR. Length of string will be used.
      value = new DNPDataDeviceAttrValue(DNP_DEVICE_ATTR_TYPE.VSTR, 0, "3.000.0010");
      db.AddDeviceAttr(pointIndex, 242, propertyValue, value);

      value = new DNPDataDeviceAttrValue(DNP_DEVICE_ATTR_TYPE.VSTR, 0, "HardwareRevison2.00");
      db.AddDeviceAttr(pointIndex, 243, propertyValue, value);

      value = new DNPDataDeviceAttrValue(DNP_DEVICE_ATTR_TYPE.VSTR, 1, "Device 100");
      db.AddDeviceAttr(pointIndex, 246, propertyValue, value);

      value = new DNPDataDeviceAttrValue(DNP_DEVICE_ATTR_TYPE.VSTR, 0, "101020304050");
      db.AddDeviceAttr(pointIndex, 248, propertyValue, value);

      value = new DNPDataDeviceAttrValue(DNP_DEVICE_ATTR_TYPE.VSTR, 0, "SDNP .NET Component");
      db.AddDeviceAttr(pointIndex, 250, propertyValue, value);

      value = new DNPDataDeviceAttrValue(DNP_DEVICE_ATTR_TYPE.VSTR, 0, "Triangle MicroWorks Inc.");
      db.AddDeviceAttr(pointIndex, 252, propertyValue, value);

      /* The following data types are not added:
       *   Double Bit Input
       *   String
       *   Vterm
       *   etc
       */
    }

    private void BinaryInput_CheckedChanged(object sender, EventArgs e)
    {
      ushort pointNumber = Convert.ToUInt16((sender as Control).Tag);
      SDNPSimBinIn point = db.LookupBinIn(pointNumber);
      BoolPointValue val = (BoolPointValue)point.PointValue;
      val.Value = (sender as CheckBox).Checked;

      // This will fail to add an event if BinaryInputScanPeriod != 0
      // but serves as an example if scanning was not enabled.
      point.AddEvent();

      // Or you could use the session add event method, which lets you specify the values
       TMWTime time = slaveSesn.GetTimeStamp();
       slaveSesn.BinInAddEvent(pointNumber, val.Value, DNPDatabaseFlags.ON_LINE, time);
      // This will also fail to add an event if BinaryInputScanPeriod != 0

       
       
       // Or you could use the session add event method, which lets you specify the values 
       slaveSesn.StringAddEvent(1, "test", time);
 
       slaveSesn.StringAddEvent(1, "", time);
    }

    private void AnalogInput_ValueChanged(object sender, EventArgs e)
    {
      ushort pointNumber = Convert.ToUInt16((sender as Control).Tag);
      SDNPSimAnlgIn point = db.LookupAnlgIn(pointNumber);
      DoublePointValue val = (DoublePointValue)point.PointValue;
      val.Value = Convert.ToDouble((sender as NumericUpDown).Value);

      Decimal product;
      Double dproduct;
      Double temp;

      temp = 1039.11000d;
      dproduct = temp * 1.00000e2d;

      temp = 1039.11;
      product = (Decimal)(temp * 100);

      temp = 1039.01;
      product = (Decimal)(temp * 100);

      temp = 1039.21;
      product = (Decimal)(temp * 100); 

      temp = 1039.02;
      product = (Decimal)(temp * 100); 

      point.Value = (Double)product;

      // This will fail to add an event if AnalogInputScanPeriod != 0
      // but serves as an example if scanning was not enabled.
      point.AddEvent();
    }

    private void Counter_Click(object sender, EventArgs e)
    {
      ushort pointNumber = Convert.ToUInt16((sender as Control).Tag);
      SDNPSimBinCntr point = db.LookupBinCntr(pointNumber);
      uint val = ++point.Value;
      switch (pointNumber)
      {
        case 0:
          Counter0.Text = val.ToString();
          break;
        case 1:
          Counter1.Text = val.ToString();
          break;
        case 2:
          Counter2.Text = val.ToString();
          break;
        default:
          break;
      }
      // This will fail to add an event if BinaryCounterScanPeriod != 0
      // but serves as an example if scanning was not enabled.
      point.AddEvent();
    }

// Protocol Analyzer Display Code
#region ProtocolAnalyzerCode

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

    // This will save the protocol log to a file
    private void SaveLog_Click(object sender, EventArgs e)
    {
      this.protocolBuffer.UpdateDiagIdMask(SCLDIAG_ID.ALL_BUT_TARGET, true);
      this.protocolBuffer.UpdateDiagIdMask(SCLDIAG_ID.PHYS, false);
      this.protocolBuffer.SaveAsText("DNPSlaveSA.log", "create", "begin", "end");
    }

#endregion ProtocolAnalyzerCode

    private void updateUserWindow()
    {
      UsersTextBox.Clear();

      TMWCryptoDataBase cryptoDb = slaveSesn.AuthCryptoDb;
      foreach (TMWAuthUser authUser in cryptoDb.AuthUsers)
      {
        UsersTextBox.AppendText(authUser.UserName + "  ");
        UsersTextBox.AppendText(authUser.UserRole.ToString() + "\n");
      }
    }

    private void UpdateAuthUsers(object sender, EventArgs e)
    {
      updateUserWindow();
    }
    private void ShowUsersBt_Click(object sender, EventArgs e)
    {
      updateUserWindow();
    }

  }
}