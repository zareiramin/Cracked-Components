//////////////////////////////////////////////////////////
//
//  This file is a part of Graybox OPC Server Toolkit.
//  Copyright (C) 2008 Graybox Software
//
//  This example demonstrates the usage of the OPC tags
//  creation methods, including the dynamic tags creation.
//
//  To register the OPC server type:
//    clrcreatetags -r
//  To remove the OPC server registration type:
//    clrcreatetags -u
//
//////////////////////////////////////////////////////////

using System;
using System.Threading;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Graybox.OPC.ServerToolkit.CLRWrapper;

// Assembly attributes.
[assembly: AssemblyTitle("ClrCreateTags")]
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

namespace ClrCreateTags
{
    static class Program
    {
        // The OPC Server object.
        static OPCDAServer srv;
        // This event will be signaled, when the OPC server is release by the clients.
        static AutoResetEvent eventStop = new AutoResetEvent(false);
        // The process entry point. Set [MTAThread] to enable free threading.
        // Free threading is required by the OPC Toolkit.
        [MTAThread]
        static void Main(string[] args)
        {
            // This will be the CLSID and the AppID of our COM-object.
            Guid srvGuid = new Guid("25F33FF2-D15D-4a00-81F7-7D2E453B9D95");
            // Parse the command line args
            if (args.Length > 0)
            {
                try
                {
                    // Register the OPC server and return.
                    if (args[0].IndexOf("-r") != -1)
                    {
                        OPCDAServer.RegisterServer(
                            srvGuid,
                            "Graybox Software",
                            "ClrCreateTags",
                            "Graybox.Sample.ClrCreateTags",
                            "1.0");
                        return;
                    }
                    // Unregister the OPC server and return.
                    if (args[0].IndexOf("-u") != -1)
                    {
                        OPCDAServer.UnregisterServer(srvGuid);
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
            srv = new OPCDAServer();
            // Advise for the events.
            //   Advise for the event that is triggered when the last instance
            //   of the OPC server is released.
            srv.Events.ServerReleased += new ServerReleasedEventHandler(Events_ServerReleased);
            //   Advise for the event, that is used to create the tags "on demand".
            srv.Events.QueryItem += new QueryItemEventHandler(Events_QueryItem);
            // Initialize the OPC server object and the OPC Toolkit.
            srv.Initialize(srvGuid, 50, 50, ServerOptions.NoAccessPaths, '.', 50000);
            // Now we create the OPC tags.
            // Here we ignore userId of tags, because we won't use it, but in the real
            // application they could be used to identify our tags in the event handlers.
            // Also we ignore the returned TagIds. If we were to use our tags later (set or get
            // their values, add properties and so on), then we would store the returned
            // TagIds somewhere.
            //   Create a readable string tag.

            srv.CreateTag(0, "Readable.1", AccessRights.readable, "default value51");
            srv.CreateTag(0, "Readable.2", AccessRights.readable, "default value51");
            srv.CreateTag(0, "Readable.3", AccessRights.readable, "default value51");
            srv.CreateTag(0, "Readable.4", AccessRights.readable, "default value51");

            for (int i = 5; i <= 50; i++)
            {
                srv.CreateTag(0, "Readable.String" + i, AccessRights.readable, "default value" + i);
            }

            srv.CreateTag(0, "Readable.51", AccessRights.readable, "default value51");
            srv.CreateTag(0, "Readable.52", AccessRights.readable, "default value52");
            srv.CreateTag(0, "Readable.53", AccessRights.readable, "default value53");



            for (int i = 54; i <= 70; i++)
            {
                srv.CreateTag(0, "Readable.String" + i, AccessRights.readable, "default value" + i);
            }
          
            
            srv.CreateTag(0, "Readable.71", AccessRights.readable, "default value71");
            srv.CreateTag(0, "Readable.72", AccessRights.readable, "default value72");
            srv.CreateTag(0, "Readable.73", AccessRights.readable, "default value73");



            for (int i = 74; i <= 400; i++)
            {
                srv.CreateTag(0, "Readable.String" + i, AccessRights.readable, "default value" + i);
            }
            srv.CreateTag(0, "Readable.100", AccessRights.readable, "default value99");
            srv.CreateTag(0, "Readable.101", AccessRights.readable, "default value101");
            srv.CreateTag(0, "Readable.102", AccessRights.readable, "default value102");



            //   Create a writeonly tag with the data type of double (VT_R8) and preset its
            //   value to 10.23.
            srv.CreateTag(0, "Writeable.Double", AccessRights.writable, (double)10.23);
            //   Create a tag that can't be read or set (actually, it'll be useless).
            srv.CreateTag(0, "NoAccess.Int", AccessRights.noAccess, 0);
            //   Create an int tag. Allow to read and write this tag.
            //   It will be possible for the client to request a value
            //   of this tag in its canonical data type only. If a client will ask the
            //   OPC server to return a value of this tag in string data type (or in any type
            //   other then int), it will recieve an error.
            srv.CreateTag(0, "Readwriteable.IntOnly", AccessRights.readWritable,
                TagOptions.CanonicalOnly, 123);
            //   Another int tag. TagOptions.DontCompareValues flag indicates, that a value of
            //   this tag will be sent to OPC Clients via IOPCDataCallback interface every time
            //   its timestamp changes, even if its value has not changed.
            srv.CreateTag(0, "Readwriteable.IntUnusual", AccessRights.readWritable,
                TagOptions.DontCompareValues, 123);
            //   Create tag with the analog engineering units. This tag will have lo and hi limits
            //   set to 10.0 and 40.0 respectively.
            srv.CreateTag(0, "Readwriteable.Analog", AccessRights.readWritable, 10.0, 40.0, 25.6);
            //   Create tag with the enumerated engineering units. It will be an int tag, that
            //   have string descriptions of its possible value. A value of zero will
            //   correspond to "Open", one - to "Close" and two - to "Malfunction".
            //   A default value of a new tag is preset to 1 ("Close" state).
            srv.CreateTag(0, "Readwriteable.Enum", AccessRights.readWritable,
                new string[] { "Open", "Close", "Malfunction" }, 1);
            //  Create an another enumerated tag. Use other overload.
            srv.CreateTag(0, "Readable.EUTag", AccessRights.readable,
                TagOptions.Default, EUType.enumerated,
                new string[] { "Operational", "Alarm" }, 0);
            // Mark the OPC server COM object as running.
            srv.RegisterClassObject();
            // Wait until the OPC server is released.
            eventStop.WaitOne();
            // Mark the OPC server COM object as stopped.
            srv.RevokeClassObject();
        }

        static void Events_QueryItem(object sender, QueryItemArgs e)
        {
            // For clarity we only process the situations where
            // the creation of a new tag is expected.
            if (!e.CreateTag) return;
            AccessRights rights = AccessRights.noAccess;
            // Determine which rights to assign to a new tag.
            // If the name of the requested tag starts with "Readable", then the tag
            // is created read only, and so on.
            if (e.TagName.StartsWith("Readable.")) rights = AccessRights.readable;
            else if (e.TagName.StartsWith("Writeable.")) rights = AccessRights.writable;
            else if (e.TagName.StartsWith("Readwriteable.")) rights = AccessRights.readWritable;
            else if (e.TagName.StartsWith("NoAccess.")) rights = AccessRights.noAccess;
            else
            {
                // Report error. Unsupported tag naming syntax.
                e.EventHandlingError = ErrorCodes.UnknownItemId;
                return;
            }
            // Create a new integer tag with the name as being queried.
            // Return its TagId to the calling client.
            e.TagId = srv.CreateTag(0, e.TagName, rights, 0);
        }

        static void Events_ServerReleased(object sender, ServerReleasedArgs e)
        {
            // Signal to the main thread, that it's time to exit.
            eventStop.Set();
        }
    }
}
