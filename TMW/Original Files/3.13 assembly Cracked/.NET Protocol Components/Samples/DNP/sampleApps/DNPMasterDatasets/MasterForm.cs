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

namespace DNPMasterDatasets
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
      bool bSerialMode = false;
      TMWApplicationBuilder applBuilder = new TMWApplicationBuilder();
      pAppl = TMWApplicationBuilder.getAppl();
      pAppl.InitEventProcessor();
      pAppl.EnableEventProcessor = true;

      protocolBuffer = TMWApplicationBuilder.getProtocolBuffer();
      protocolBuffer.ProtocolDataReadyEvent += new ProtocolBuffer.ProtocolDataReadyEventDelegate(ProtocolEvent);
      protocolBuffer.EnableCheckForDataTimer = true;
      InitializeComponent();

      masterChan = new DNPChannel(TMW_CHANNEL_OR_SESSION_TYPE.MASTER);
      masterChan.Name = "DNPMaster";  /* name displayed in analyzer window */

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
        masterChan.Type = WINIO_TYPE.TCP;
        masterChan.WinTCPipAddress = "127.0.0.1";
        masterChan.WinTCPipPort = 20000;
        masterChan.WinTCPmode = TCP_MODE.CLIENT;
      }

      masterChan.OpenChannel();

      // This property does not control data sets or device attributes.
      // They are separate from the rest of the data point database.
      // Only the simulated database implementation is implemented for those.
      MDNPDatabase.UseSimDatabase = false;

      masterSesn = new MDNPSession(masterChan);
      masterSesn.AuthenticationEnabled = false;

      masterSesn.AutoDataSetRestart = true;
      // could also use MDNPRequest.DatasetExchange(); to exchange data set objects on demand.
  
      masterSesn.SessionOnlineStateEvent += new TMWSession.SessionOnlineStateEventDelegate(masterSesn_SessionOnlineStateEvent);
 
      masterSesn.OpenSession();

      db = (MDNPSimDatabase)masterSesn.SimDatabase;
      customizeDatabase();

      // Register to receive notification of database changes
      db.UpdateDBEvent += new TMWSimDataBase.UpdateDBEventDelegate(UpdateDBEvent);

      ReadTypeCB.Items.Add("Prototype");
      ReadTypeCB.Items.Add("Descriptor");
      ReadTypeCB.Items.Add("Characteristics");
      ReadTypeCB.Items.Add("Point Index Attributes");
      ReadTypeCB.Items.Add("Data Set Value");
      ReadTypeCB.Items.Add("Data Set Event");
      ReadTypeCB.SelectedIndex = 1;

      SendTypeCB.Items.Add("Write");
      SendTypeCB.Items.Add("Select");
      SendTypeCB.Items.Add("Operate");
      SendTypeCB.Items.Add("Select/Operate");
      SendTypeCB.Items.Add("DirectOperate");
      SendTypeCB.SelectedIndex = 0;
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
      db.Clear();

      CreateDatasetPrototypes();
      CreateDatasetDescriptors();
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

    private delegate void UpdatePointDelegate(TMWSimPoint simPoint);
    private void UpdateDBEvent(TMWSimPoint simPoint)
    {
      if (InvokeRequired)
      {
        BeginInvoke(new UpdatePointDelegate(UpdateDBEvent), new object[] { simPoint });
      }
      else
      {
        switch (simPoint.PointType)
        {
          case 87:
            protocolBuffer.Insert("Data set value updated for Data Set Id " + simPoint.PointNumber.ToString() + "\n");
            break;
          case 88:
            protocolBuffer.Insert("Data set event updated for Data Set Id " + simPoint.PointNumber.ToString() + "\n");
            break;
          default:
            break;
        }
      }
    }

    // create data set prototypes in master database
    void CreateDatasetPrototypes()
    {
      MDNPSimDatasetProto point;

      Byte[] uuidArray ={ 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5, 6 };

      point = db.LookupDatasetPrototype(uuidArray);
      if (point != null)
      {
        //A Prototype with this UUID already exists in database 
        return;
      }

      point = db.AddDatasetPrototype(uuidArray);
      if (point == null)
      {
        //Failed to add prototype 
        return;
      }

      DNPDatasetElem elem;
      DNP_DATASET_DATA_TYPE dataType;
      DNP_DATASET_DESCR_TYPE descrType;
      Byte maxLength;
      String myString;
      
      // Namespace and Name are optional, but if they exist they must be the first two elements.
      descrType = DNP_DATASET_DESCR_TYPE.NSPC;
      dataType = DNP_DATASET_DATA_TYPE.NONE;
      myString = "NewNamespace";
      elem = new DNPDatasetElem(descrType, dataType, 0, myString);
      point.AddDescrElem(elem);

      descrType = DNP_DATASET_DESCR_TYPE.NAME;
      dataType = DNP_DATASET_DATA_TYPE.NONE;
      myString = "MasterPrototype1";
      elem = new DNPDatasetElem(descrType, dataType, 0, myString);
      point.AddDescrElem(elem);
   
      dataType = DNP_DATASET_DATA_TYPE.INT;
      descrType = DNP_DATASET_DESCR_TYPE.DAEL;
      maxLength = 4;
      myString = "elem1";
      elem = new DNPDatasetElem(descrType, dataType, maxLength, myString);
      point.AddDescrElem(elem);
       
      dataType = DNP_DATASET_DATA_TYPE.INT;
      descrType = DNP_DATASET_DESCR_TYPE.DAEL;
      maxLength = 4;
      myString = "elem2";
      elem = new DNPDatasetElem(descrType, dataType, maxLength, myString);
      point.AddDescrElem(elem);

      dataType = DNP_DATASET_DATA_TYPE.FLT;
      descrType = DNP_DATASET_DESCR_TYPE.DAEL;
      maxLength = 8;
      myString = "elem3";
      elem = new DNPDatasetElem(descrType, dataType, maxLength, myString);
      point.AddDescrElem(elem);

      dataType = DNP_DATASET_DATA_TYPE.INT;
      descrType = DNP_DATASET_DESCR_TYPE.DAEL;
      maxLength = 4;
      myString = "elem4";
      elem = new DNPDatasetElem(descrType, dataType, maxLength, myString);
      point.AddDescrElem(elem);

      dataType = DNP_DATASET_DATA_TYPE.INT;
      descrType = DNP_DATASET_DESCR_TYPE.DAEL;
      maxLength = 4;
      myString = "elem5";
      elem = new DNPDatasetElem(descrType, dataType, maxLength, myString);
      point.AddDescrElem(elem);

      dataType = DNP_DATASET_DATA_TYPE.FLT;
      descrType = DNP_DATASET_DESCR_TYPE.DAEL;
      maxLength = 8;
      myString = "elem6";
      elem = new DNPDatasetElem(descrType, dataType, maxLength, myString);
      point.AddDescrElem(elem);

      dataType = DNP_DATASET_DATA_TYPE.INT;
      descrType = DNP_DATASET_DESCR_TYPE.DAEL;
      maxLength = 4;
      myString = "elem7";
      elem = new DNPDatasetElem(descrType, dataType, maxLength, myString);
      point.AddDescrElem(elem);

      dataType = DNP_DATASET_DATA_TYPE.INT;
      descrType = DNP_DATASET_DESCR_TYPE.DAEL;
      maxLength = 4;
      myString = "elem8";
      elem = new DNPDatasetElem(descrType, dataType, maxLength, myString);
      point.AddDescrElem(elem);

      dataType = DNP_DATASET_DATA_TYPE.FLT;
      descrType = DNP_DATASET_DESCR_TYPE.DAEL;
      maxLength = 8;
      myString = "elem9";
      elem = new DNPDatasetElem(descrType, dataType, maxLength, myString);
      point.AddDescrElem(elem);

      dataType = DNP_DATASET_DATA_TYPE.INT;
      descrType = DNP_DATASET_DESCR_TYPE.DAEL;
      maxLength = 4;
      myString = "elem10";
      elem = new DNPDatasetElem(descrType, dataType, maxLength, myString);
      point.AddDescrElem(elem);


      Byte[] uuidArray2 = { 2, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5, 6 };

      point = db.LookupDatasetPrototype(uuidArray2);
      if (point != null)
      {
        //A Prototype with this UUID already exists in database 
        return;
      }

      point = db.AddDatasetPrototype(uuidArray2);
      if (point == null)
      {
        //Failed to add prototype 
        return;
      }

      // Namespace and Name are optional, but if they exist they must be the first two elements.
       /*
      descrType = DNP_DATASET_DESCR_TYPE.NSPC;
      dataType = DNP_DATASET_DATA_TYPE.NONE;
      myString = "NewNamespace";
      elem = new DNPDatasetElem(descrType, dataType, 0, myString);
      point.AddDescrElem(elem);

      descrType = DNP_DATASET_DESCR_TYPE.NAME;
      dataType = DNP_DATASET_DATA_TYPE.NONE;
      myString = "MasterPrototype2";
      elem = new DNPDatasetElem(descrType, dataType, 0, myString);
      point.AddDescrElem(elem);
      */

      dataType = DNP_DATASET_DATA_TYPE.INT;
      descrType = DNP_DATASET_DESCR_TYPE.DAEL;
      maxLength = 4;
      myString = "elem1";
      elem = new DNPDatasetElem(descrType, dataType, maxLength, myString);
      point.AddDescrElem(elem);

      dataType = DNP_DATASET_DATA_TYPE.INT;
      descrType = DNP_DATASET_DESCR_TYPE.DAEL;
      maxLength = 4;
      myString = "elem2";
      elem = new DNPDatasetElem(descrType, dataType, maxLength, myString);
      point.AddDescrElem(elem);

      dataType = DNP_DATASET_DATA_TYPE.FLT;
      descrType = DNP_DATASET_DESCR_TYPE.DAEL;
      maxLength = 8;
      myString = "elem3";
      elem = new DNPDatasetElem(descrType, dataType, maxLength, myString);
      point.AddDescrElem(elem);

      dataType = DNP_DATASET_DATA_TYPE.INT;
      descrType = DNP_DATASET_DESCR_TYPE.DAEL;
      maxLength = 4;
      myString = "elem4";
      elem = new DNPDatasetElem(descrType, dataType, maxLength, myString);
      point.AddDescrElem(elem);

      dataType = DNP_DATASET_DATA_TYPE.INT;
      descrType = DNP_DATASET_DESCR_TYPE.DAEL;
      maxLength = 4;
      myString = "elem5";
      elem = new DNPDatasetElem(descrType, dataType, maxLength, myString);
      point.AddDescrElem(elem);

      dataType = DNP_DATASET_DATA_TYPE.FLT;
      descrType = DNP_DATASET_DESCR_TYPE.DAEL;
      maxLength = 8;
      myString = "elem6";
      elem = new DNPDatasetElem(descrType, dataType, maxLength, myString);
      point.AddDescrElem(elem);

      dataType = DNP_DATASET_DATA_TYPE.INT;
      descrType = DNP_DATASET_DESCR_TYPE.DAEL;
      maxLength = 4;
      myString = "elem7";
      elem = new DNPDatasetElem(descrType, dataType, maxLength, myString);
      point.AddDescrElem(elem);

      dataType = DNP_DATASET_DATA_TYPE.INT;
      descrType = DNP_DATASET_DESCR_TYPE.DAEL;
      maxLength = 4;
      myString = "elem8";
      elem = new DNPDatasetElem(descrType, dataType, maxLength, myString);
      point.AddDescrElem(elem);

      dataType = DNP_DATASET_DATA_TYPE.FLT;
      descrType = DNP_DATASET_DESCR_TYPE.DAEL;
      maxLength = 8;
      myString = "elem9";
      elem = new DNPDatasetElem(descrType, dataType, maxLength, myString);
      point.AddDescrElem(elem);

      dataType = DNP_DATASET_DATA_TYPE.INT;
      descrType = DNP_DATASET_DESCR_TYPE.DAEL;
      maxLength = 4;
      myString = "elem10";
      elem = new DNPDatasetElem(descrType, dataType, maxLength, myString);
      point.AddDescrElem(elem);

    }

    // create data set descriptors in master database
    void CreateDatasetDescriptors()
    { 
      // Add dataset descriptor (and dataset) to database

      DNP_DATASET_CHAR characteristics = (DNP_DATASET_CHAR.MASTER|DNP_DATASET_CHAR.READABLE
                                 |DNP_DATASET_CHAR.WRITABLE|DNP_DATASET_CHAR.STATIC); 
       
      MDNPSimDatasetDescr point = db.AddDatasetDescriptor(characteristics);
      if(point == null)
      {
        //Failed to add descriptor 
        return;
      }
      
      DNPDatasetElem elem;
      DNP_DATASET_DATA_TYPE dataType;
      DNP_DATASET_DESCR_TYPE descrType;
      Byte maxLength;
      String myString;
      Byte[] uuidArray ={ 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5, 6 };
 
      descrType = DNP_DATASET_DESCR_TYPE.NAME; 
      dataType = DNP_DATASET_DATA_TYPE.NONE;
      myString = "MasterDescriptor1";
      elem = new DNPDatasetElem(descrType, dataType, 0, myString);
      point.AddDescrElem(elem, null);

      dataType = DNP_DATASET_DATA_TYPE.NONE;
      myString = "Prototype1";
      elem = new DNPDatasetElem(dataType, 0, uuidArray, myString);
      point.AddDescrElem(elem, null);

      Byte[] uuidArray2 = { 2, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5, 6 };

      dataType = DNP_DATASET_DATA_TYPE.NONE;
      myString = "Prototype2";
      elem = new DNPDatasetElem(dataType, 0, uuidArray2, myString);
      point.AddDescrElem(elem, null);

      dataType = DNP_DATASET_DATA_TYPE.INT;
      descrType = DNP_DATASET_DESCR_TYPE.CTLS;
      maxLength = 1;
      myString = "Ctls1";
      elem = new DNPDatasetElem(descrType, dataType, maxLength, myString);
      point.AddDescrElem(elem, null);
       
      dataType = DNP_DATASET_DATA_TYPE.INT;
      descrType = DNP_DATASET_DESCR_TYPE.CTLV;
      maxLength = 4;
      myString = "Ctlv1";
      elem = new DNPDatasetElem(descrType, dataType, maxLength, myString);
      point.AddDescrElem(elem, null);

    }
        
    // Response event from data set write/control request
    void  WriteDataset_RequestEvent(MDNPRequest request, MDNPResponseParser response)
    { 
      if(response.Status == DNPChannel.RESPONSE_STATUS.SUCCESS)
        protocolBuffer.Insert("Data Set Write/Control Request succeeded");
      else if (response.Status == DNPChannel.RESPONSE_STATUS.INTERMEDIATE)
        protocolBuffer.Insert("Data Set Write/Control Request intermediate response");
      else
        protocolBuffer.Insert("Data Set Write/Control Request failed");
    }

    // Response event from data set read request
    void  ReadDatasetEvent_RequestEvent(MDNPRequest request, MDNPResponseParser response)
    {  
      // Just as an example, look at the dataset data in the database
      // This data may or may not be the result of the Dataset Events Read command
      MDNPSimDataset dataset = db.LookupDataset(3);
      if (dataset != null)
      {
        for (int i = 1; i < dataset.NumberElems; i++)
        {
          DNPDatasetValue datasetValue;
          datasetValue = dataset.GetDatasetElem((byte)i);
          protocolBuffer.Insert("Data Set elem " + i.ToString() + " value " + datasetValue.Value + "\n");
        }
      }
    }

    // convert text to string
    private UInt32 ToUInt32 ( String text )
    {
      UInt32 result = 0;
      try
      {
        result = Convert.ToUInt32(text);
      }
      catch
      {
        Output_TB.Text = text + " is not a valid Unsigned 32 bit value\n"; 
      }
      return result;
    }

    // send button was selected
    private void Send_BT_Click(object sender, EventArgs e)
    {
      bool retValue = false;

      // send a data set write/control request
      ushort[] pointsArray = { (UInt16)ToUInt32(SendId_TB.Text) };
      MDNPRequest request = new MDNPRequest(masterSesn);
      request.RequestEvent += new MDNPRequest.RequestEventDelegate(WriteDataset_RequestEvent);


      //SendTypeCB.Items.Add("Write");
      //SendTypeCB.Items.Add("Select");
      //SendTypeCB.Items.Add("Operate");
      //SendTypeCB.Items.Add("Select/Operate");
      //SendTypeCB.Items.Add("DirectOperate");
      switch (SendTypeCB.SelectedIndex)
      {
        case 0:
          retValue = request.WriteDataset(pointsArray);
          break;
        case 1:
          retValue = request.DatasetControl(MDNPRequest.DNP_FUNCTION_CODE.SELECT, false, pointsArray);
          break;
        case 2:
          retValue = request.DatasetControl(MDNPRequest.DNP_FUNCTION_CODE.OPERATE, false, pointsArray);
          break;
        case 3:
          retValue = request.DatasetControl(MDNPRequest.DNP_FUNCTION_CODE.SELECT, true, pointsArray);
          break;
        case 4:
          retValue = request.DatasetControl(MDNPRequest.DNP_FUNCTION_CODE.DIRECT_OP, false, pointsArray);
          break;
      }

      if (!retValue)
      {
        Output_TB.Text = "Data Set write/control was not sent, verify data set exists in master database";
      }
    }

    // read button was selected
    private void Read_BT_Click(object sender, EventArgs e)
    {
      // read a data set from the outstation
      MDNPRequest request2 = new MDNPRequest(masterSesn);
      UInt16 start = (UInt16)ToUInt32(ReadId_TB.Text);
      request2.RequestEvent += new MDNPRequest.RequestEventDelegate(ReadDatasetEvent_RequestEvent);


      //ReadTypeCB.Items.Add("Prototype");
      //ReadTypeCB.Items.Add("Descriptor");
      //ReadTypeCB.Items.Add("Characteristics");
      //ReadTypeCB.Items.Add("Point Index Attributes");
      //ReadTypeCB.Items.Add("Data Set Value");
      //ReadTypeCB.Items.Add("Data Set Event"); 
      Byte group = 87;
      Byte variation = 0;
      switch (ReadTypeCB.SelectedIndex)
      {
        case 0:
          group = 85;
          variation = 1;
          break;
        case 1:
          group = 86;
          variation = 1;
          break;
        case 2:
          group = 86;
          variation = 2;
          break;
        case 3:
          group = 86;
          variation = 3;
          break;
        case 4:
          group = 87;
          variation = 1;
          break;
        case 5: 
          request2.ReadGroup(88, 1, MDNPRequest.DNP_QUALIFIER.Q_ALL_POINTS, 0, 0);
          return; //don't call ReadGroup below
      }
      request2.ReadGroup(group, variation, MDNPRequest.DNP_QUALIFIER.Q_16BIT_START_STOP, start, start);
    }

    // display the selected dataset
    private void DisplayDataset(UInt16 datasetId)
    {
      MDNPSimDataset dataset = db.LookupDataset(datasetId);
      MDNPSimDatasetDescr descr = db.LookupDatasetDescriptor(datasetId);
      Output_TB.Text = " ";
      if (dataset != null)
      {
        Output_TB.Text = "Data Set " + datasetId + "\n";
        for (int i = 1; i <= dataset.NumberElems; i++)
        {
          DNPDatasetElem descrElem;
          DNPDatasetValue datasetValue;
          DNPDatasetPointIndexAttr pointIndexAttr;
          String typeMaxLengthText = " ";

          descrElem = descr.GetExpandedDescrElem((byte)(i - 1));
          pointIndexAttr = dataset.GetAssociatedPointIndexAttrElem((byte)(i - 1));
          datasetValue = dataset.GetDatasetElem((byte)i);
          
          // Convert the types and max length to a string to display
          if (i == 1)
          {
            // mandatory time element
            Output_TB.Text += "Element " + i.ToString() + " Time " + datasetValue.Value + "\n";
          }
          else
          {

            if (descrElem != null)
            {
              typeMaxLengthText += descrElem.DescrType.ToString() + ":" +
                              descrElem.DataType.ToString() + "(" +
                              descrElem.MaxLength.ToString() + ")";
            }

            Output_TB.Text += "Element " + i.ToString() + typeMaxLengthText + " value " + datasetValue.Value;

            // If point index attributes exist for this data set element display them 
            if (pointIndexAttr != null)
            {
              Output_TB.Text += " PointIndexAttr " + pointIndexAttr.PointType.ToString() + ":" + pointIndexAttr.PointIndex.ToString() + "\n";
            }
            else
            {
              Output_TB.Text += "\n";
            }
          }
        }
      }
      else
      {
        Output_TB.Text = "Data Set " + datasetId + " not found";
      }
    }


    // display button was selected
    private void Display_BT_Click(object sender, EventArgs e)
    {
      UInt16 datasetId = (UInt16)ToUInt32(DisplayId_TB.Text);
      DisplayDataset(datasetId);
    }

    // modify button was selected
    private void Modify_BT_Click(object sender, EventArgs e)
    {
      UInt16 datasetId = (UInt16)ToUInt32(ModifyId_TB.Text);
      MDNPSimDataset dataset = db.LookupDataset(datasetId);
      Output_TB.Text = " ";
      if (dataset != null)
      {
        Output_TB.Text += "Data Set " + datasetId + "\n";
        UInt16 elemIndex = (UInt16)ToUInt32(ModifyIndex_TB.Text);

        if (elemIndex == 1)
        {
          // Mandatory time for dataset 
          if (!dataset.SetDatasetTime(ModifyValue_TB.Text))
          {
            Output_TB.Text = "SetDatasetTime could not set mandatory time element, try format 17Sep09 14:22:22.443 or September 17, 2009 14:22:22.443 or any valid System.DateTime format";
          }
          else
          {
            DisplayDataset(datasetId);
          }
        }
        else if (dataset.NumberElems >= elemIndex)
        {
          byte length = 4;
          if (!dataset.SetDatasetElem((byte)elemIndex, length, ModifyValue_TB.Text))
          {
            Output_TB.Text = "SetDatasetElem failed, does the value make sense for this type of element?";
          }
          else
          {
            DisplayDataset(datasetId);
          }
        }
        else
        {
          Output_TB.Text = "Data Set " + datasetId + " Element Index " + elemIndex.ToString() + " not found";
        }
      }
      else
      {
        Output_TB.Text = "Data Set " + datasetId + " not found";
      }
    }

    // This will save the protocol log to a file
    private void SaveLogBt_Click(object sender, EventArgs e)
    {
      this.protocolBuffer.UpdateDiagIdMask(SCLDIAG_ID.ALL_BUT_TARGET, true);
      this.protocolBuffer.UpdateDiagIdMask(SCLDIAG_ID.PHYS, false);
      this.protocolBuffer.SaveAsText("DNPMasterDatasets.log", "create", "begin", "end"); 
    }
  }
}