using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace MMBWebService
{
  public partial class MMBReadHoldingRegisters : System.Web.UI.Page
  {
    private MMBService serv = new MMBService();

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void ReadRegistersPB_Click(object sender, EventArgs e)
    {
      if (ReadRegistersPB.Enabled == false)
      {
        return;
      }
      ReadRegistersPB.Enabled = false;
      ResultList.Items.Clear();
      ushort[] res;
      res = serv.MMBReadHoldingRegisters("127.0.0.1", 502, 0, 10);
      if (res == null)
      {
        ResultList.Items.Add("read failed");
      }
      else
      {
        int index = 0;
        foreach (ushort v in res)
        {
          ResultList.Items.Add(index.ToString() + " = " + v.ToString());
          index++;
        }
      }
      ReadRegistersPB.Enabled = true;
    }

    protected void WriteHoldingRegisterPB_Click(object sender, EventArgs e)
    {
      if (WriteHoldingRegisterPB.Enabled == false)
      {
        return;
      }
      WriteHoldingRegisterPB.Enabled = false;
      ResultList.Items.Clear();

      UInt16 register;
      UInt16 value;
      try
      {
        register = Convert.ToUInt16(RegisterNumberTB.Text);
        value = Convert.ToUInt16(HoldingRegisterValueTB.Text);
      }
      catch
      {
        register = 0;
        value = 0;
      }

      bool res = serv.MMBWriteHoldingRegister("127.0.0.1", 502, register, value);
      if (!res)
      {
        ResultList.Items.Add("write failed");
      }
      
      WriteHoldingRegisterPB.Enabled = true;
    }
  }
}
