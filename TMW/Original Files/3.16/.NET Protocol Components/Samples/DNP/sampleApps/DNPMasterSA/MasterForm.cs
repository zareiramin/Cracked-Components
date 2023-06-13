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


namespace DNPMasterSA
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
    private MDNPSimDatabase db;
    private MDNPSession masterSesn;
    private DNPChannel masterChan;
    private bool pauseAnalyzer;

    public MasterForm()
    {
      // Change this to true to connect over COM ports
      bool bSerialMode = false;

      // Add Multiple Secure Authentication users, in addition to the Common user
      bool configureAdditionalUsers = true; 

      TMWApplicationBuilder applBuilder = new TMWApplicationBuilder();
      pAppl = TMWApplicationBuilder.getAppl();
      pAppl.EnableEventProcessor = true;
      pAppl.InitEventProcessor(); 

      protocolBuffer = TMWApplicationBuilder.getProtocolBuffer();
      protocolBuffer.ProtocolDataReadyEvent += new ProtocolBuffer.ProtocolDataReadyEventDelegate(ProtocolEvent);
      protocolBuffer.EnableCheckForDataTimer = true;
      InitializeComponent();

      masterChan = new DNPChannel(TMW_CHANNEL_OR_SESSION_TYPE.MASTER);
      masterChan.ChannelConnectStateEvent += new TMWChannel.ChannelConnectStateEventDelegate(masterChan_ChannelConnectStateEvent);
      
      if (bSerialMode)
      {
        masterChan.Type = WINIO_TYPE.RS232;
        masterChan.Win232comPortName = "COM3";
        masterChan.Win232baudRate = "9600";
        masterChan.Win232numDataBits = RS232_DATA_BITS.BITS_8;
        masterChan.Win232numStopBits = RS232_STOP_BITS.BITS_1;
        masterChan.Win232portMode = RS232_PORT_MODE.NONE;
      }
      else
      {
        masterChan.Type = WINIO_TYPE.TCP; // equivalent to UDP_TCP
        masterChan.Name = ".NET DNP Master";  /* name displayed in analyzer window */
        masterChan.WinTCPipAddress = "127.0.0.1";
        masterChan.WinTCPipPort = 20000;
        masterChan.WinTCPmode = TCP_MODE.CLIENT;

        // if SSL/TLS should be enabled, uncomment the following lines 
        // requires ssleay32.dll and libeay32.dll from OpenSSL 
        // Also requires Diffie-Helman dh1024.pem file and other files
        // which may be specified here.

        // SSL/TLS Enabled on this channel.
        //masterChan.SslTlsEnabled = true;

        // file containing root Certificate Authority certificate 
        //masterChan.SslTlsCertAuthFile = "root.pem";

        // file containing server certificate
        //masterChan.SslTlsCredentialsFile = "key.pem";

        // password for private key in credentials file
        //masterChan.SslTlsPassword = "yourPassword";

        // If false, even if SSL Credentials are not valid connection will be allowed.
        // If true, if SSL Credentials are not valid if SslTlsCheckCertificateEvent 
        // is registered it will be fired. If this event is not registered or the event
        // returns false the connection will be broken.
        //masterChan.SslTlsVerifyCert = true;

        // event to be fired if Credentials are not valid.
        //masterChan.SslTlsCheckCertificateEvent += new TMWChannel.SslTlsCheckCertificateEventDelegate(masterChan_SslTlsCheckCertificateEvent);
      }

      if(!masterChan.OpenChannel())
      {
        // failed  
        protocolBuffer.Insert("Error: Failed to open master channel\n");
        return;
      }

      masterSesn = new MDNPSession(masterChan);

      // Set this to true to enable DNP Secure Authentication 
      masterSesn.AuthenticationEnabled = true; 

      // This configures a common user with an update key that matches the default one in the test harness.
      // DO NOT USE THIS UPDATE KEY other than for testing with the test harness using it's default test key. 
      // Change this to a secret key and provision the outstation to use the same key.
      // Or with SAv5 the Authority can direct the master to add a User and Update Key on the outstation over DNP.
      TMWCryptoDataBase cryptoDb = masterSesn.AuthCryptoDb;
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
      cryptoDb.AuthOSAsymPubKey = "TMWTestOSAsymPubKey.pem";
      cryptoDb.AuthOSAsymPrvKey = "TMWTestOSAsymPrvKey.pem";
      cryptoDb.AuthorityAsymPubKey = "TMWTestRsa2048PubKey.pem";
      cryptoDb.AuthorityAsymPrvKey = "TMWTestRsa2048PrvKey.pem"; 


      // The following are defaults, but are here to show some parameters that could be configured
      masterSesn.AuthAggressiveMode = true;
      
      // Number of Authentication Messages before Session Keys must be updated.
      masterSesn.AuthMaxKeyChangeCount = 1000;

      // 15 minutes till Session Keys must be updated.
      masterSesn.AuthKeyChangeInterval = 15 * 60 * 1000;

      // 2 seconds for an Authentication Request to timeout.
      masterSesn.AuthResponseTimeout = 2 * 1000;

      // If SAv2 AND SAv5 are supported, you can choose which to use on an association.
      // SAv5 is an additional component that may be purchased and licensed separately.
      // If configure to use SAv5 and you do not have a license for it, the session will use SAv2.
      // SAv2 does NOT support adding Users over DNP. (It cannot send an Update User Status g120v10 request)
      masterSesn.AuthOperateInV2Mode = false;
   
      // Register to receive Session online/offline events 
      masterSesn.SessionOnlineStateEvent += new TMWSession.SessionOnlineStateEventDelegate(masterSesn_SessionOnlineStateEvent);
 
      if (!masterSesn.OpenSession())
      {
        // failed  
        protocolBuffer.Insert("Error: Failed to open master session\n");
        return;
      }

      // Register to receive notification of database changes
      db = (MDNPSimDatabase)masterSesn.SimDatabase;
      db.UpdateDBEvent += new TMWSimDataBase.UpdateDBEventDelegate(UpdateDBEvent);
              
      customizeDatabase();
    }

    // ssl - pointer to OpenSSL SSL structure (requires header file from www.openssl.org).
    // error - openSSL error returned from function SSL_get_verify_result()
    //         such as X509_V_ERR_SELF_SIGNED_CERT_IN_CHAIN in x509_vfy.h  
    private bool masterChan_SslTlsCheckCertificateEvent(TMWChannel channel, IntPtr ssl, int error)
    {
      // Check credentials and either return true-meaning keep connection
      // or false-meaning disconnect

      // You may want to decide about whether to allow the connection
      // based on the value of error, or call OpenSSL functions to analyze the SSL structure
      return true;
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
     
    private void customizeDatabase()
    {
      ushort i;
      // Points will be automatically added on the master as data about them is 
      // received in responses from the Outstation.
      // It is also possible to add them as this sample does so that the GUI 
      // can reference them before receiving them from the outstation
      //
      // Add 3 of each of the following types since that is what the GUI references:
      //  Binary Output 
      //  Analog Output 
      for (i = 0; i < 3; i++)
      {
        db.AddBinOut(i);
        db.AddAnlgOut(i);
      }
    }

    private void updateBinaryOutput(TMWSimPoint simPoint)
    {
      string strVal = (simPoint as MDNPSimBinOut).Value ? "On" : "Off";
      Color textColor = (simPoint as MDNPSimBinOut).Value ? Color.ForestGreen : Color.Red;

      switch (simPoint.PointNumber)
      {
        case 0:
          BinOut0Feedback.Text = strVal;
          BinOut0Feedback.ForeColor = textColor;
          break;
        case 1:
          BinOut1Feedback.Text = strVal;
          BinOut1Feedback.ForeColor = textColor;
          break;
        case 2:
          BinOut2Feedback.Text = strVal;
          BinOut2Feedback.ForeColor = textColor;
          break;
        default:
          //protocolBuffer.Insert("Received update for unexpected binary output point: " + simPoint.PointNumber.ToString());
          break;
      }
    }

    private void updateAnalogOutput(TMWSimPoint simPoint)
    {
      switch (simPoint.PointNumber)
      {
        case 0:
          AnlgOut0Feedback.Text = (simPoint as MDNPSimAnlgOut).Value.ToString();
          break;
        case 1:
          AnlgOut1Feedback.Text = (simPoint as MDNPSimAnlgOut).Value.ToString();
          break;
        case 2:
          AnlgOut2Feedback.Text = (simPoint as MDNPSimAnlgOut).Value.ToString();
          break;
        default:
          //protocolBuffer.Insert("Received update for unexpected analog output point: " + simPoint.PointNumber.ToString());
          break;
      }
    }

    private delegate void UpdatePointDelegate(TMWSimPoint simPoint);
    private void UpdateDBEvent(TMWSimPoint simPoint)
    {
      if (InvokeRequired)
        BeginInvoke(new UpdatePointDelegate(UpdateDBEvent), new object[] { simPoint });
      else
      {
        switch (simPoint.PointType)
        {
          case 10:
            // Binary Output (CROB)
            updateBinaryOutput(simPoint);
            break;
          case 40:
            // Analog Output
            updateAnalogOutput(simPoint);
            break;
          default:
            //protocolBuffer.Insert("Unknown point type in database update routine");
            break;
        }
      }
    }

    private void BinOutOn_Click(object sender, EventArgs e)
    {
      // Determine which point this request is for
      ushort pointNumber = Convert.ToUInt16((sender as Control).Tag);

      // Build and send the request
      CROBInfo crobData = new CROBInfo(pointNumber, CROBInfo.CROB_CTRL.LON, 1, 0, 0);
      CROBInfo[] crobArray = { crobData };

      MDNPSimBinOut point = db.LookupBinOut(pointNumber);
      MDNPRequest request = new MDNPRequest(masterSesn);
      request.BinaryCommand(MDNPRequest.DNP_FUNCTION_CODE.SELECT, true, true, 100, MDNPRequest.DNP_QUALIFIER.Q_8BIT_INDEX, crobArray);
      // request.BinaryOutWrite((byte)DNP_QUALIFIER.Q_8BIT_START_STOP, pointNumber, pointNumber, val);
    }

    private void BinOutOff_Click(object sender, EventArgs e)
    {
      // Determine which point this request is for
      ushort pointNumber = Convert.ToUInt16((sender as Control).Tag);

      // Build and send the request
      CROBInfo crobData = new CROBInfo(pointNumber, CROBInfo.CROB_CTRL.LOFF, 1, 0, 0);
      CROBInfo[] crobArray = { crobData };

      MDNPSimBinOut point = db.LookupBinOut(pointNumber);
      MDNPRequest request = new MDNPRequest(masterSesn);
      request.BinaryCommand(MDNPRequest.DNP_FUNCTION_CODE.SELECT, true, true, 100, MDNPRequest.DNP_QUALIFIER.Q_8BIT_INDEX, crobArray);
      // request.BinaryOutWrite((byte)DNP_QUALIFIER.Q_8BIT_START_STOP, pointNumber, pointNumber, val);
    }

    private void AnlgOutSend_Click(object sender, EventArgs e)
    {
      double val;
      // Determine which point this request is for
      ushort pointNumber = Convert.ToUInt16((sender as Control).Tag);

      // Get the value to write for this point
      switch (pointNumber)
      {
        case 0:
          val = (double)AnlgOut0Val.Value;
          break;
        case 1:
          val = (double)AnlgOut1Val.Value;
          break;
        case 2:
          val = (double)AnlgOut2Val.Value;
          break;
        default:
          val = 0;
          protocolBuffer.Insert("Unexected analog output command for undefined point: " + pointNumber.ToString());
          break;
      }

      // Build and send the request
      AnalogInfo anlgData = new AnalogInfo(pointNumber, val);
      AnalogInfo[] anlgArray = { anlgData };

      MDNPSimAnlgOut point = db.LookupAnlgOut(pointNumber);
      MDNPRequest request = new MDNPRequest(masterSesn);
      request.AnalogCommand(MDNPRequest.DNP_FUNCTION_CODE.SELECT, true, true, 100, MDNPRequest.DNP_QUALIFIER.Q_8BIT_INDEX, 2, anlgArray);
    }

#region TrustedAuthorityCode
    // Trusted Authority Code
    // This only applies if SAv5 is licensed.

    // A trusted third party, the Authority, should direct the Master to Add, Modify, or Delete an Authentication User
    private void ActAsAuthority_Click(object sender, EventArgs e)
    {
      TMWCrypto.ALGORITHM algorithm;
      TMWAUTH_OPERATION operation;
      String userName;  
      TMWAUTH_USERROLE role;
      TMWAUTH_KEYCHANGE_METHOD keyChangeMethod;
      UInt16 expiry;
      UInt32 sequence;
      UInt16 updateKeyLength;

      // Clear display text box
      UserInfoTB.Text = "";

      if (masterSesn.AuthOperateInV2Mode)
      {
        UserInfoTB.AppendText("Master is operating in SAv2 mode, it cannot send Update User Status (g120v10) to Outstation.\nDNP SA must be licensed to use Remote Key Change Methods.");
        return;
      }

      operation = TMWAUTH_OPERATION.ADD;
      if(OperationCB.Text == "Delete")
        operation = TMWAUTH_OPERATION.DELETE;
      else if(OperationCB.Text == "Change")
        operation = TMWAUTH_OPERATION.CHANGE;

      if (UserNameTB.Text == "")
      {
        UserInfoTB.AppendText("There is no User Name\n");
        return;
      }

      userName = UserNameTB.Text;

      role = TMWAUTH_USERROLE.SINGLEUSER;
      if(UserRoleCB.Text == "Viewer")
      {
        role = TMWAUTH_USERROLE.VIEWER;
      }
      else if(UserRoleCB.Text == "Operator")
      {
        role = TMWAUTH_USERROLE.OPERATOR;
      }
      else if(UserRoleCB.Text == "Engineer")
      {
        role = TMWAUTH_USERROLE.ENGINEER;
      }
      else if(UserRoleCB.Text == "Installer")
      {
        role = TMWAUTH_USERROLE.INSTALLER;
      }
      else if(UserRoleCB.Text == "SecAdm")
      {
        role = TMWAUTH_USERROLE.SECADM;
      }
      else if(UserRoleCB.Text == "SecAud")
      {
        role = TMWAUTH_USERROLE.SECAUD;
      }
      else if(UserRoleCB.Text == "RBACmnt")
      {
        role = TMWAUTH_USERROLE.RBACMNT;
      }
          
   
      // key change method
      // AES-GMAC needs to be added.
      keyChangeMethod = TMWAUTH_KEYCHANGE_METHOD.AES128;
      if(KeyChangeMethodCB.Text == "AES-256 / SHA-256-HMAC")
        keyChangeMethod = TMWAUTH_KEYCHANGE_METHOD.AES256;
      else if(KeyChangeMethodCB.Text == "RSA-1024 / SHA-1-HMAC")
        keyChangeMethod = TMWAUTH_KEYCHANGE_METHOD.RSA1024SHA1;
      else if (KeyChangeMethodCB.Text == "RSA-2048 / SHA-256-HMAC")
        keyChangeMethod = TMWAUTH_KEYCHANGE_METHOD.RSA2048SHA256;
      else if (KeyChangeMethodCB.Text == "RSA-3072 / SHA-256-HMAC")
        keyChangeMethod = TMWAUTH_KEYCHANGE_METHOD.RSA3072SHA256;


      expiry = 30;

      // This number must increment or outstation will reject the request  
      // wraps at 0xffffffff
      if (StatusChangeSequenceUpDown.Value == 0xffffffff)
        StatusChangeSequenceUpDown.Value = 1;
      else 
        StatusChangeSequenceUpDown.Value++;
      sequence = (UInt32) StatusChangeSequenceUpDown.Value;

      TMWAuthUser newUser = new TMWAuthUser();
      newUser.UserName = userName;
      newUser.Operation = operation;
      newUser.KeyChangeMethod = keyChangeMethod;
      newUser.UserRole = role;
      newUser.UserRoleExpiryInterval = expiry;

      // Create certification data as an Authority would
      UInt16 certLength;
      Byte[] plainTextArray;
      plainTextArray = new Byte[4000];
 
      plainTextArray[0] = (Byte)((Byte)operation & 0xff); 

      plainTextArray[1] = (Byte)(sequence & 0xff);
      plainTextArray[2] = (Byte)((sequence>>8) & 0xff);
      plainTextArray[3] = (Byte)((sequence>>16) & 0xff);
      plainTextArray[4] = (Byte)((sequence>>24) & 0xff); 

      certLength = 5;
 
      plainTextArray[certLength++] = (Byte)((UInt16)role & 0xff);
      plainTextArray[certLength++] = (Byte)(((UInt16)role>>8) & 0xff);

      plainTextArray[certLength++] = (Byte)(expiry & 0xff);
      plainTextArray[certLength++] = (Byte)((expiry >> 8) & 0xff);  

      Byte[] userNameArray = System.Text.Encoding.ASCII.GetBytes(userName);
      plainTextArray[certLength++] = (Byte)(userNameArray.Length & 0xff);
      plainTextArray[certLength++] = (Byte)((userNameArray.Length>>8) & 0xff);

      if((int)keyChangeMethod < 64)
      {
        // Symmetric key change method
        userNameArray.CopyTo(plainTextArray, certLength);
        certLength += (UInt16)userNameArray.Length;
      }
      else
      {
        // If Asymmetric Key Change Method, User Public Key is part of the certification data
        newUser.AsymPubKey = "TMWTestDsa2048PrvKey.pem";
        // The private key file also contains the public key
        newUser.AsymPrvKey = "TMWTestDsa2048PrvKey.pem";

        Byte[] userPublicKeyArray;

        // get the actual public key from the key file 
        userPublicKeyArray = TMWCrypto.GetPubKeyData(newUser.AsymPubKey);
        if (userPublicKeyArray == null)
        {
          // failed
          UserInfoTB.AppendText("Failed to read User Public Key data from " + newUser.AsymPubKey + "\n");
          return;
        }

        plainTextArray[certLength++] = (Byte)(userPublicKeyArray.Length & 0xff);
        plainTextArray[certLength++] = (Byte)((userPublicKeyArray.Length >> 8) & 0xff);

        userNameArray.CopyTo(plainTextArray, certLength);
        certLength += (UInt16)userNameArray.Length;

        userPublicKeyArray.CopyTo(plainTextArray, certLength);
        certLength += (UInt16)userPublicKeyArray.Length;
      }

      System.Array.Resize(ref plainTextArray, certLength);
      

      updateKeyLength = 16;
      if ((int)keyChangeMethod < 64)
      {
        algorithm = TMWCrypto.ALGORITHM.SHA1MAC; 
        if (keyChangeMethod == TMWAUTH_KEYCHANGE_METHOD.AES256)
        {
          algorithm = TMWCrypto.ALGORITHM.SHA256MAC;
          updateKeyLength = 32;
        }
        else if (keyChangeMethod == TMWAUTH_KEYCHANGE_METHOD.AES256GMAC)
        {
          // Add example code for AES GMAC.
          return;
        }
        Byte[] AuthoritySymCertKey = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06 };  
        newUser.CertificationData = TMWCrypto.MACValue(algorithm, plainTextArray, null,  AuthoritySymCertKey);
      }
      else
      {   
        // Authority should digitally sign this with Central Authority private key, 
        // and then verified by the outstation using public key.
        Byte[] authorityPrivateKeyArray = System.Text.Encoding.ASCII.GetBytes("TMWTestRsa2048PrvKey.pem");

        if ((int)keyChangeMethod < (int)TMWAUTH_KEYCHANGE_METHOD.RSA2048SHA256)
        {
          algorithm = TMWCrypto.ALGORITHM.DSA_SHA1_SIGN;
        }
        else
        {
          algorithm = TMWCrypto.ALGORITHM.DSA_SHA256_SIGN;
          updateKeyLength = 32;
        }

        // Sign this with authority private key.
        newUser.CertificationData = TMWCrypto.SignData(algorithm, plainTextArray, authorityPrivateKeyArray);
      }
      
      if (newUser.CertificationData.Length == 0)
      {
        // failed
        UserInfoTB.AppendText("Creating the certification data failed\n");
        return;
      }

      // generate a random User Update Key
      newUser.UpdateKey = TMWCrypto.GenerateUserUpdateKey(updateKeyLength);
      newUser.SequenceNumber = sequence;
      newUser.UserRole = role;
      newUser.UserRoleExpiryInterval = expiry;
        
      MDNPRequest request = new MDNPRequest(masterSesn);
      
      // Register to receive responses or timeout from this request
      request.RequestEvent += new MDNPRequest.RequestEventDelegate(AuthUserStatusChangeRequestEvent);

      // perform complete sequence, g120v10, g120v11, g120v13, g120v14/15
      bool sendUpdateKey = true;

      // Send the request
      request.AuthUserStatusChange(newUser, operation, sendUpdateKey);

      // Update display
      UserInfoTB.AppendText("Sent User Status Request\n");
    }
#endregion TrustedAuthorityCode


    // This will be called each time a response is received from the AuthUserStatusChange request or on timeout.
    void AuthUserStatusChangeRequestEvent(MDNPRequest request, MDNPResponseParser response)
    { 
      if (response.Last)
      {
        // this is the end of the request sequence
        UpdateUsers(response.Status);
      }
    }

    private delegate void UpdateUsersDelegate(DNPChannel.RESPONSE_STATUS status);
    private void UpdateUsers(DNPChannel.RESPONSE_STATUS status)
    {
      if (InvokeRequired)
      {
        BeginInvoke(new UpdateUsersDelegate(UpdateUsers), new object[] {status});
      }
      else
      {
        if(status == DNPChannel.RESPONSE_STATUS.SUCCESS)
        {
          UserInfoTB.AppendText("Success\n");
          foreach (TMWAuthUser userEntry in db.AuthCryptoDb.AuthUsers)
          {
            UserInfoTB.AppendText(userEntry.UserName + "   ");
            UserInfoTB.AppendText(userEntry.UserNumber.ToString() + "\n");
          }
        }
        else
        {
          UserInfoTB.AppendText(status.ToString() + "\n");
        }
      }
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
          // Don't display physical and target layer trace
          //if ((pdo.SourceIDasUint & ((UInt32)SCLDIAG_ID.PHYS | (UInt32)SCLDIAG_ID.TARGET)) == 0)  
          if (protocolBuffer.getPdoAtIndex(i).Time != null)
            protocolAnalyzer.AppendText(protocolBuffer.getPdoAtIndex(i).Time.ToLogString());

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

    // This will save the protocol log to a file
    private void SaveLog_Click(object sender, EventArgs e)
    {
      this.protocolBuffer.UpdateDiagIdMask(SCLDIAG_ID.ALL_BUT_TARGET, true);
      this.protocolBuffer.UpdateDiagIdMask(SCLDIAG_ID.PHYS, false);
      this.protocolBuffer.SaveAsText("DNPMasterSA.log", "create", "begin", "end");
    }
#endregion ProtocolAnalyzerCode
  
  }
}