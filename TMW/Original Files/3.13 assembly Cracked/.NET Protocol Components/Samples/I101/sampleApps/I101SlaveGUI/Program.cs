using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace I101slaveGUI
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
        Application.Run(new SlaveForm());
      }
      catch (Exception e)
      {
        MessageBox.Show(e.ToString());
      }
    }
  }
}