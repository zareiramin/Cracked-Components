using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Collections.Generic;
using Graybox.OPC.ServerToolkit.CLRWrapper;

namespace Clr2LifeTime
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        /// <remarks>
        /// In .NET CF we can only use the EventHandler delegate
        /// to communicate with the main thread of the application.
        /// And we can simply call methods of the Form class from the
        /// OPC event handler, because OPC events are raised on the
        /// random RPC thread, not the main application thread, and
        /// Form methods are non threadsafe.
        /// </remarks>
        public void LogMessages(object sender, EventArgs e)
        {
            lock (Program.SyncRoot)
            {
                foreach (string s in Program.Messages)
                {
                    listBoxLog.Items.Insert(0, s);
                }
                Program.Messages.Clear();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Program.OPCServer.Suspend();
            listBoxLog.Items.Insert(0, "Suspended by UI");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Program.OPCServer.Resume();
            listBoxLog.Items.Insert(0, "Resumed by UI");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Program.OPCServer.Shutdown("UI request");
            listBoxLog.Items.Insert(0, "Shutdown UI request");
        }
    }
}