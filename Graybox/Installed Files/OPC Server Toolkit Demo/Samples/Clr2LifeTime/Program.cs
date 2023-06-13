//////////////////////////////////////////////////////////
//
//  This file is a part of Graybox OPC Server Toolkit.
//  Copyright (C) 2008 Graybox Software
//
//  An OPC server written in C#. This OPC server contains
//  no tags. 
//
//  Demonstarates the usage of dotNET Wrapper for Graybox
//  OPC Server Toolkit and the usage of the following events:
//  Unlock, Lock, DestroyInstance, CreateInstance,
//  BeforeCreateInstance and ServerReleased. 
//
//  To register the OPC server type:
//    clrlifetime -r
//  To remove the OPC server registration type:
//    clrlifetime -u
//
//////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;
using Graybox.OPC.ServerToolkit.CLRWrapper;

namespace Clr2LifeTime
{
    static class Program
    {
        public static int m_init = 0;
        private static int m_new_client_id = 0;
        private static List<string> m_messages = new List<string>();
        private static Guid m_guid = new Guid("AA4E7383-D1AB-4f1c-AA5E-A820DB3BB9E5");
        private static OPCDAServer m_srv = new OPCDAServer();
        private static Form1 m_form = null;
        private static void AddLogMessage(string s)
        {
            lock (SyncRoot) { m_messages.Add(s); }
            if (Interlocked.CompareExchange(ref m_init, 1, 1) == 1)
                m_form.Invoke(new EventHandler(m_form.LogMessages), m_srv, EventArgs.Empty);
        }
        public static object SyncRoot
        {
            get { return m_messages; }
        }
        public static List<string> Messages
        {
            get { return m_messages; }
        }
        public static OPCDAServer OPCServer
        {
            get { return m_srv; }
        }

        [MTAThread]
        static void Main(string[] args)
        {
            // Parse the command line args
            if (args.Length > 0)
            {
                try
                {
                    // Register the OPC server
                    if (args[0].IndexOf("-r") != -1)
                    {
                        OPCDAServer.RegisterServer(
                            m_guid,
                            "Graybox Software",
                            "ClrLifeTime",
                            "Graybox.Sample.ClrLifeTime",
                            "1.0");
                        return;
                    }
                    // Unregister the OPC server
                    if (args[0].IndexOf("-u") != -1)
                    {
                        OPCDAServer.UnregisterServer(m_guid);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return;
                }
            }

            // Initialize the OPC server object and the OPC Toolkit
            m_srv.Initialize(Program.m_guid, 100, 100, ServerOptions.NoAccessPaths, '.', 100);
            m_srv.Events.ServerReleased += new ServerReleasedEventHandler(Events_ServerReleased);
            m_srv.Events.BeforeCreateInstance += new BeforeCreateInstanceEventHandler(Events_BeforeCreateInstance);
            m_srv.Events.CreateInstance += new CreateInstanceEventHandler(Events_CreateInstance);
            m_srv.Events.DestroyInstance += new DestroyInstanceEventHandler(Events_DestroyInstance);
            m_srv.Events.Lock += new LockEventHandler(Events_Lock);
            m_srv.Events.Unlock += new UnlockEventHandler(Events_Unlock);
            // Mark the OPC server COM object as running
            m_srv.RegisterClassObject();

            m_form = new Form1();
            Interlocked.Increment(ref m_init);

            // Run the application
            Application.Run(m_form);

            m_srv.Shutdown("Server is terminating");
            m_srv.RevokeClassObject();
        }

        static public void Events_Unlock(object sender, UnlockArgs e)
        {
            AddLogMessage("Lock counter has been decremented");
        }

        static public void Events_Lock(object sender, LockArgs e)
        {
            AddLogMessage("Lock counter has been incremented");
        }

        static public void Events_DestroyInstance(object sender, DestroyInstanceArgs e)
        {
            AddLogMessage("[ClientId " + e.ClientId.ToString() + "] Client has disconnected");
        }

        static public void Events_CreateInstance(object sender, CreateInstanceArgs e)
        {
            AddLogMessage("[ClientId " + e.ClientId.ToString() + "] Client has connected");
        }

        static public void Events_BeforeCreateInstance(object sender, BeforeCreateInstanceArgs e)
        {
            AddLogMessage("Connection request");
            DialogResult dr = MessageBox.Show(
                "A client is requesting a connection.\n\n" +
                "Allow the client to connect to ClrLifeTime?",
                "ClrLifeTime",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1);
            // Restrict creation of a new OPC server instance
            if (dr == DialogResult.No)
            {
                e.EventHandlingError = ErrorCodes.AccessDenied;
                return;
            }
            e.ClientId = Interlocked.Increment(ref m_new_client_id);
            AddLogMessage("Connection request accepted [ClientId " + e.ClientId.ToString() + "]");
        }

        static public void Events_ServerReleased(object sender, ServerReleasedArgs e)
        {
            AddLogMessage("OPC server has been released");
            DialogResult dr = MessageBox.Show(
                "Last instance of ClrLifeTime OPC server\n" +
                "has been released.\n\n" +
                "Allow clients to connect to ClrLifeTime in the furute?",
                "ClrLifeTime",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1);
            e.Suspend = (dr == DialogResult.No);
        }

    }
}