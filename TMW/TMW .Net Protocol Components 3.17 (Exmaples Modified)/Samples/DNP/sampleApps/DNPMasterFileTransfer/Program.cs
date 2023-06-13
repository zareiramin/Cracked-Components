using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DNPmasterFileTransfer
{
  static class Program
  {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      try
      {
        Application.Run(new MasterForm());
      }
      catch (Exception e)
      {
        MessageBox.Show(e.ToString());
      }
    }
  }
}