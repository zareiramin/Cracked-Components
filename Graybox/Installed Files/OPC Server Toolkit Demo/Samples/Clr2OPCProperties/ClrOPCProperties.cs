//////////////////////////////////////////////////////////
//
//  This file is a part of Graybox OPC Server Toolkit.
//  Copyright (C) 2008 Graybox Software
//
//  This example demonstrates the usage the
//  OPC properties.
//
//  To register the OPC server type:
//    clropcproperties -r
//  To remove the OPC server registration type:
//    clropcproperties -u
//
//////////////////////////////////////////////////////////

using System;
using System.Threading;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Graybox.OPC.ServerToolkit.CLRWrapper;

// Assembly attributes.
[assembly: AssemblyTitle("ClrOPCProperties")]
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

namespace ClrOPCProperties
{
    static class Program
    {
        // The OPC Server object.
        static OPCDAServer srv;
        // Here we will place the TagId of the OPC tags.
        static int idTag;
        static int idTagNominal;
        static int idTagStatus;
        // This event will be signaled, when the OPC server is release by the clients.
        static AutoResetEvent eventStop = new AutoResetEvent(false);
        // The process entry point. Set [MTAThread] to enable free threading.
        // Free threading is required by the OPC Toolkit.
        [MTAThread]
        static void Main(string[] args)
        {
            // This will be the CLSID and the AppID of our COM-object.
            Guid srvGuid = new Guid("32468097-D88C-414f-8868-C1403DB496CE");
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
                            "ClrOPCProperties",
                            "Graybox.Sample.ClrOPCProperties",
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
            //   Advise for the event that is called when the client reads the OPC properties.
            srv.Events.ReadProperties += new ReadPropertiesEventHandler(Events_ReadProperties);
            //   Adive for the event taht will handle tag reads (from DEVICE, see OPC DA Spec).
            srv.Events.ReadItems += new ReadItemsEventHandler(Events_ReadItems);
            //   Adive for the event taht will handle tag writes.
            srv.Events.WriteItems += new WriteItemsEventHandler(Events_WriteItems);
            // Initialize the OPC server object and the OPC Toolkit.
            srv.Initialize(srvGuid, 50, 50, ServerOptions.NoAccessPaths, '.', 100);
            // Create an OPC tag. Assume that this tag corresponds to some physical
            // controlled variable, that has its current value, its lo and hi limits and
            // some nominal value.
            // Here we ignore userId of a tag, because we won't use it. The default value of
            // a tag is set to zero and its canonical data type is set to double (VT_R8).
            idTag = srv.CreateTag(0, "ControlledVariable", AccessRights.readWritable, (double)0);
            // Now add a tag that will serve as a nominal value of ControlledVariable.
            // Assume that nominal value is 50.0.
            idTagNominal = srv.CreateTag(0, "ControlledVariableNominal", AccessRights.readWritable, (double)50.0);
            // Add a tag that will contain some textual description of a controlled variable.
            // It'll be available for the OPC clients to modify and read this status.
            idTagStatus = srv.CreateTag(0, "ControlledVariableStatus", AccessRights.readWritable, "normal");
            // Now we add the standard OPC properties.
            //   Add lo limit property with the value of 10.0.
            srv.AddProperty(idTag, StandardProperties.LoLimit, (double)10);
            //   or we could do it like this
            //      srv.AddProperty(idTag, 309, (double)10);
            //   Add hi limit of 90.0.
            srv.AddProperty(idTag, StandardProperties.HiLimit, (double)90);
            //   Add a property, that will describe the OPC tags.
            srv.AddProperty(idTag, StandardProperties.ItemDescription, "This is a current value of a controlled variable");
            srv.AddProperty(idTagNominal, StandardProperties.ItemDescription, "This is a nominal value of a controlled variable");
            // Now we a custom OPC property. It will be a nominal value of a variable.
            // We will assign a PropID of 5000 to our custom property and provide its textual description.
            // Also we report, that there is a tag, that could be read to recieve a value of a property.
            srv.AddProperty(idTag, 5000, (double)0, "A nominal value of a controlled variable", "ControlledVariableNominal");
            // Add another custom property - a status of a controlled variable.
            // Bind "ControlledVariableStatus" tag to this property.
            srv.AddProperty(idTag, 5001, "", "A textual description of a controlled variable status",
                "ControlledVariableStatus");
            // This dummy OPC server cache update transaction will move
            // the default tag values to the OPC server cache. Tag qualities
            // will be set to OPC_BAD_WAITING_FOR_INITIAL_DATA.
            srv.BeginUpdate();
            srv.EndUpdate(true);
            // Mark the OPC server COM object as running.
            srv.RegisterClassObject();
            // Wait until the OPC server is released.
            eventStop.WaitOne();
            // Mark the OPC server COM object as stopped.
            srv.RevokeClassObject();
        }

        // When the "ControlledVariableStatus" tag value is being written
        // by the OPC client, we place its value to the property 5001 of the
        // "ControlledVariable" tag.
        static void Events_WriteItems(object sender, WriteItemsArgs e)
        {
            // Iterate through the requested items.
            for (int i = 0; i < e.Count; i++)
            {
                // Skip items with the zero TagId.
                if (e.ItemIds[i].TagId == 0) continue;
                // Process only the "ControlledVariableStatus" tag.
                if (e.ItemIds[i].TagId == idTagStatus)
                {
                    // Modify a value of the property 5001 of the
                    // "ControlledVariable" tag.
                    srv.SetProperty(idTag, 5001, e.Values[i]);
                }
            }
        }

        // When the client asks the OPC server to return a device value
        // of the "ControlledVariableStatus" tag, a value of the property 5001
        // of the "ControlledVariable" tag will be returned.
        static void Events_ReadItems(object sender, ReadItemsArgs e)
        {
            // Iterate through the requested items.
            for (int i = 0; i < e.Count; i++)
            {
                // Skip items with the zero TagId.
                if (e.ItemIds[i].TagId == 0) continue;
                // Process only the "ControlledVariableStatus" tag.
                if (e.ItemIds[i].TagId == idTagStatus)
                {
                    // Read a value of a property and place its value to the tag.
                    srv.UpdateTags(
                        new int[] { idTagStatus },
                        new object[] { srv.GetProperty(idTag, 5001) },
                        true);
                }
            }
        }

        // This handler returns the values of the properties.
        static void Events_ReadProperties(object sender, ReadPropertiesArgs e)
        {
            // Iterate through the requested properties.
            for (int i = 0; i < e.Count; i++)
            {
                // If property is our custom property 5000 ("A nominal value of a controlled variable")
                if (e.Properties[i] == 5000)
                {
                    // If property 5000 is requested for the "ControlledVariable" tag we
                    // will return a value of the "ControlledVariableNominal" tag.
                    if (e.Item.TagId == idTag)
                    {
                        // Read a value of the "ControlledVariableNominal" tag. This tag in bound to
                        // a property 5000 of the "ControlledVariable" tag.
                        object[] vals = null;
                        srv.GetTags(new int[] { idTagNominal }, ref vals);
                        // Return a value of a property.
                        e.Values[i] = vals[0];
                        // Set the result of a property reading operation.
                        e.Errors[i] = ErrorCodes.Ok;
                    }
                    // For all other tags we will report error.
                    else
                    {
                        // Say that we can't return a nominal value, because the property
                        // is not supported for this tag.
                        e.Errors[i] = ErrorCodes.InvalidPropertyId;
                        // Set the event handling error to False (there was a number of errors,
                        // while reading sertain properties).
                        e.EventHandlingError = ErrorCodes.False;
                    }
                }
            }
            // Do not process the properties of any other tags. Leave it for the Toolkit.
        }

        // Occures when the OPC server is released.
        static void Events_ServerReleased(object sender, ServerReleasedArgs e)
        {
            // Signal to the main thread, that it's time to exit.
            eventStop.Set();
        }
    }
}
