//////////////////////////////////////////////////////////
//
//  This file is a part of Graybox OPC Server Toolkit.
//  Copyright (C) 2008 Graybox Software
//
//  This example demonstrates the minimal OPC server.
//
//  A simple OPC server written in C#. This sample creates
//  an OPC server with 10 tags. Two properties are added
//  for each tag: LoLimit (set to 0) and HiLimit (set to 100).
//  The values of tags are incremented constantly.
//  WriteItems event handler allows OPC clients to set a
//  new tags data value if this value is in the range from
//  0 to 100. Otherwise, an error is reported to the calling
//  client. 
//
//  To register the OPC server type:
//    clrminopc -r
//  To remove the OPC server registration type:
//    clrminopc -u
//
//////////////////////////////////////////////////////////

using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Graybox.OPC.ServerToolkit.CLRWrapper;

// Assembly attributes.
[assembly: AssemblyTitle("ClrMinOpc")]
[assembly: AssemblyDescription("Sample .NET OPC Server")]
[assembly: AssemblyCompany("Graybox Software")]
[assembly: AssemblyProduct("Graybox OPC Server Toolkit")]
[assembly: AssemblyCopyright("Copyright © Graybox Software 2008-2012")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]
[assembly: AssemblyVersion("1.0.12.314")]
#if !WindowsCE && !PocketPC
[assembly: AssemblyFileVersion("1.0.12.314")]
#endif

namespace ClrMinOpc
{
    static class Program
    {
        // This int will be set to 1 when it's time to exit.
        static int stop = 0;
        // Count of OPC tags to create.
        static int tag_count = 10;
        // The TagId identifiers of OPC tags.
        static int[] tag_ids = new int[tag_count];
        // The process entry point. Set [MTAThread] to enable free threading.
        // Free threading is required by the OPC Toolkit.
        [MTAThread]
        static void Main(string[] args)
        {
            // This will be the CLSID and the AppID of our COM-object.
            Guid srv_guid = new Guid("EA1370BF-AC53-41f2-940D-3A834208BBFB");
            // Parse the command line args
            if (args.Length > 0)
            {
                try
                {
                    // Register the OPC server and return.
                    if (args[0].IndexOf("-r") != -1)
                    {
                        OPCDAServer.RegisterServer(
                            srv_guid,
                            "Graybox Software",
                            "ClrMinOpc",
                            "Graybox.Sample.ClrMinOpc",
                            "1.0");
                        return;
                    }
                    // Remove the OPC server registration and return.
                    if (args[0].IndexOf("-u") != -1)
                    {
                        OPCDAServer.UnregisterServer(srv_guid);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return;
                }
            }
            // Create an object of the OPC Server class.
            OPCDAServer srv = new OPCDAServer();
            // Advise for the OPC Toolkit events.
            srv.Events.WriteItems += new WriteItemsEventHandler(Events_WriteItems);
            srv.Events.ServerReleased += new ServerReleasedEventHandler(Events_ServerReleased);
            // Initialize the OPC server object and the OPC Toolkit.
            srv.Initialize(srv_guid, 50, 50, ServerOptions.NoAccessPaths, '.', 100);
            // Create the OPC tags.
            for (int i = 0; i < tag_count; i++)
            {
                // Create a tag.
                tag_ids[i] = srv.CreateTag(i, "Folder.Tag" + i.ToString(), AccessRights.readWritable, i);
                // Add a couple of standard OPC properties.
                srv.AddProperty(tag_ids[i], StandardProperties.LoLimit, (double)0);
                srv.AddProperty(tag_ids[i], StandardProperties.HiLimit, (double)100);
            }
            // Mark the OPC server COM object as running.
            srv.RegisterClassObject();
            // Wait until the OPC server is released by the clients.
            // Periodically update tags values while the OPC server is not released.
            while (System.Threading.Interlocked.CompareExchange(ref stop, 1, 1) == 0)
            {
                System.Threading.Thread.Sleep(200);
                // Begin the update of the OPC server cache.
                srv.BeginUpdate();
                // Get current values of the tags.
                object[] values = null;
                srv.GetTags(tag_ids, ref values);
                // Calculate new values and place them into the OPC server cache.
                for (int i = 0; i < tag_count; i++)
                {
                    srv.SetTag(tag_ids[i], ((int)values[i] + 1) % 100, Quality.Good, FileTime.UtcNow);
                }
                // Finish the update of the OPC server cache. We pass false,
                // because its unnecessary for this update to be synchronous.
                srv.EndUpdate(false);
            }
            // Mark the OPC server COM object as stopped.
            srv.RevokeClassObject();
        }

        /// <summary>
        /// A handler for the WriteItems event of the OPCDAServer object.
        /// We do not update the OPC server cache here.
        /// </summary>
        static void Events_WriteItems(object sender, WriteItemsArgs e)
        {
            for (int i = 0; i < e.Count; i++)
            {
                if (e.ItemIds[i].TagId == 0) continue;
                try
                {
                    int v = Convert.ToInt32(e.Values[i], System.Globalization.CultureInfo.InvariantCulture);
                    if (v < 0 || v > 100) throw new ArgumentOutOfRangeException();
                }
                catch (Exception ex)
                {
                    e.Errors[i] = (ErrorCodes)System.Runtime.InteropServices.Marshal.GetHRForException(ex);
                    e.ItemIds[i].TagId = 0;
                    e.MasterError = ErrorCodes.False;
                }
            }
        }

        /// <summary>
        /// A handler for the ServerReleased event of the OPCDAServer object.
        /// </summary>
        static void Events_ServerReleased(object sender, ServerReleasedArgs e)
        {
            // Make the OPC server object 'suspended'.
            // No new OPC server instances can be created by the clients
            // from this moment.
            e.Suspend = true;
            // Signal the main thread, that it's time to exit.
            System.Threading.Interlocked.Exchange(ref stop, 1);
        }
    }
}
