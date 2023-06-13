using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.Diagnostics;

using SCLLicenseManager;

using TMW.SCL;
using TMW.SCL.MB;
using TMW.SCL.MB.Master;

namespace MMBWebService
{
  /// <summary>
  /// This wraps Master Modbus as a Web Service
  /// </summary>
  [WebService(Namespace = "http://tempuri.com/")]
  [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
  [ToolboxItem(false)]
  public class MMBService : System.Web.Services.WebService
  {
    static private TMWApplication pAppl = null;
    static bool bBusy = false;
    private MMBSession masterSesn = null;
    private MBChannel masterChan = null;
    private MMBDatabase Master;
    private ushort[] hrValues;

    private void UnInit()
    {
      pAppl.Close();
    }

    private void Init()
    {
      TMWApplicationBuilder applBuilder = new TMWApplicationBuilder();
      pAppl = TMWApplicationBuilder.getAppl();
      pAppl.InitEventProcessor();
      pAppl.EnableEventProcessor = true;
    }

    private void CloseMaster()
    {
      if (masterSesn != null)
      {
        masterSesn.CloseSession();
        masterSesn = null;
      }
      if (masterChan != null)
      {
        masterChan.CloseChannel();
        masterChan = null;
      }
    }
    private bool OpenMaster(String ipAddress, ushort ipPort)
    {
      if (masterChan == null)
      {
        masterChan = new MBChannel(TMW_CHANNEL_OR_SESSION_TYPE.MASTER);

        masterChan.Type = WINIO_TYPE.TCP;
        masterChan.Name = ".NET MB Master Web Service";  /* name displayed in analyzer window */
        masterChan.WinTCPipAddress = ipAddress;
        masterChan.WinTCPipPort = ipPort;
        masterChan.WinTCPmode = TCP_MODE.CLIENT;
        masterChan.OpenChannel();

        masterSesn = new MMBSession(masterChan);

        masterSesn.OpenSession();

        Master = (MMBDatabase)masterSesn.SimDatabase;
        MMBDatabase.UseSimDatabase = false;
        Master.StoreHoldingRegistersEvent += new MMBDatabase.StoreHoldingRegistersDelegate(Master_StoreHoldingRegistersEvent);
        return true;
      }
      return false;
    }

    

    bool Master_StoreHoldingRegistersEvent(MMBDatabase db, ushort startAddr, ushort[] values)
    {
      hrValues = values;
      bReadDone = true;
      return true;
    }

    [WebMethod]
    public string Name()
    {
      return "TMW .NET Protocol Component Modbus Master Web Service";
    }

    [WebMethod]
    public ushort[] MMBReadHoldingRegisters(String ipAddress, ushort ipPort, ushort startAddress, ushort count)
    {
      if (bBusy == false)
      {
        Debug.WriteLine("MMBReadHoldingRegisters");
        bBusy = true;
        Init();
        if (OpenMaster(ipAddress, ipPort) == true)
        {
          MMBRequest req = new MMBRequest(masterSesn);
          req.RequestEvent += new MMBRequest.RequestEventDelegate(readReq_RequestEvent);
          req.ReadHregs(startAddress, count);

          bReadDone = false;
          while (bReadDone == false)
          {
            System.Threading.Thread.Sleep(100);
          }

          CloseMaster();
          UnInit();

          bBusy = false;
          return hrValues;
        }
      }
      return null;
    }

    [WebMethod]
    public bool MMBWriteHoldingRegister(String ipAddress, ushort ipPort, ushort addr, ushort value)
    {
      if (bBusy == false)
      {
        bBusy = true;
        Init();
        if (OpenMaster(ipAddress, ipPort) == true)
        {
          MMBRequest req = new MMBRequest(masterSesn);
          req.RequestEvent += new MMBRequest.RequestEventDelegate(writeReq_RequestEvent);
          bool bOK = req.WriteHreg(addr, value);

          bWriteDone = false;
          while (bWriteDone == false)
          {
            System.Threading.Thread.Sleep(100);
          }

          CloseMaster();
          UnInit();

          bBusy = false;
          return bOK;
        }
      }
      return false;
    }

    private bool bReadDone = false;
    private bool bWriteDone = false;

    void readReq_RequestEvent(MMBRequest request, MMBResponseParser response)
    {
      bReadDone = true;
    }

    void writeReq_RequestEvent(MMBRequest request, MMBResponseParser response)
    {
      bWriteDone = true;
    }

  }
}
