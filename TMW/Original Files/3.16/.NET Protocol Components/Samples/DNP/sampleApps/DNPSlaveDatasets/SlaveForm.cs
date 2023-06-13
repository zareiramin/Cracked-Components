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
using TMW.SCL.DNP.Slave;

namespace DNPSlaveDatasets
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
      bool bSerialMode = false;

      TMWApplicationBuilder applBuilder = new TMWApplicationBuilder();
      pAppl = TMWApplicationBuilder.getAppl();
      pAppl.EnableEventProcessor = true;
      pAppl.InitEventProcessor();

      protocolBuffer = TMWApplicationBuilder.getProtocolBuffer();
      protocolBuffer.ProtocolDataReadyEvent += new ProtocolBuffer.ProtocolDataReadyEventDelegate(ProtocolEvent);
      protocolBuffer.EnableCheckForDataTimer = true;
      InitializeComponent();

      slaveChan = new DNPChannel(TMW_CHANNEL_OR_SESSION_TYPE.SLAVE);
      slaveChan.Name = ".NET DNP Slave Datasets";  /* name displayed in analyzer window */

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
        slaveChan.Type = WINIO_TYPE.TCP;
        slaveChan.WinTCPipAddress = "127.0.0.1";
        slaveChan.WinTCPipPort = 20000;
        slaveChan.WinTCPmode = TCP_MODE.SERVER;
      }

      slaveChan.OpenChannel();

      slaveSesn = new SDNPSession(slaveChan);
      slaveSesn.AuthenticationEnabled = false;

      slaveSesn.SessionOnlineStateEvent += new TMWSession.SessionOnlineStateEventDelegate(slaveSesn_SessionOnlineStateEvent);
 
      slaveSesn.OpenSession(); 

      db = (SDNPSimDatabase)slaveSesn.SimDatabase;
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

    private delegate void UpdatePointDelegate(TMWSimPoint simPoint);

    private void customizeDatabase()
    {
      // This Clear is not required here since the database is empty,
      // but this would delete all points from the database.
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
 
    private void UpdateDBEvent(TMWSimPoint simPoint)
    {
      if (InvokeRequired)
        BeginInvoke(new UpdatePointDelegate(UpdateDBEvent), new object[] { simPoint });
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

    // create data set prototypes in slave database
    void CreateDatasetPrototypes()
    {
      SDNPSimDatasetProto point;

      Byte[] uuidArray ={ 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 1, 1, 1, 1, 1 };

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
       
      descrType = DNP_DATASET_DESCR_TYPE.NSPC;
      dataType = DNP_DATASET_DATA_TYPE.NONE;
      myString = "SlaveNamespace";
      elem = new DNPDatasetElem(descrType, dataType, 0, myString);
      point.AddDescrElem(elem);

      descrType = DNP_DATASET_DESCR_TYPE.NAME;
      dataType = DNP_DATASET_DATA_TYPE.NONE;
      myString = "SlavePrototype1";
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
    }

    // create data set descriptors in slave database
    void CreateDatasetDescriptors()
    { 
      // Add dataset descriptor (and dataset) to database

      DNP_DATASET_CHAR characteristics = (DNP_DATASET_CHAR.READABLE|DNP_DATASET_CHAR.EVENT
                                 |DNP_DATASET_CHAR.WRITABLE|DNP_DATASET_CHAR.STATIC);

      TMW_CLASS_MASK classMask = TMW_CLASS_MASK.THREE;
      SDNPSimDatasetDescr point = db.AddDatasetDescriptor(classMask, characteristics);
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
      myString = "SlaveDescriptor1";
      elem = new DNPDatasetElem(descrType, dataType, 0, myString);
      point.AddDescrElem(elem, null);

      dataType = DNP_DATASET_DATA_TYPE.NONE;
      myString = "Prototype1";
      elem = new DNPDatasetElem(dataType, 0, uuidArray, myString);
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
        String errorText = text + " is not a valid Unsigned 32 bit value\n";
        protocolBuffer.Insert(errorText);
      }
      return result;
    }

    // display the selected dataset
    private void DisplayDataset(UInt16 datasetId)
    {
      SDNPSimDataset dataset = db.LookupDataset(datasetId);
      SDNPSimDatasetDescr descr = db.LookupDatasetDescriptor(datasetId);
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

            Output_TB.Text += "Element " + i.ToString() + typeMaxLengthText + " value " + datasetValue.Value.ToString();

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
      SDNPSimDataset dataset = db.LookupDataset(datasetId);
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

    // generate data set event button was selected
    private void DatasetEvent_BT_Click(object sender, EventArgs e)
    {
      UInt16 datasetId = (UInt16)ToUInt32(GenerateId_TB.Text); 
      
      SDNPSimDataset dataset = db.LookupDataset(datasetId);
      if (dataset != null)
      {
        if (!dataset.AddEvent())
        {
          Output_TB.Text = "Data Set " + datasetId + ", add event failed, does this Data Set allow events?";
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
      this.protocolBuffer.SaveAsText("DNPSlaveDatasets.log", "create", "begin", "end");
    }

  }
}