//////////////////////////////////////////////////////////
//
//  This file is a part of Graybox OPC Server Toolkit.
//  Copyright (C) 2008 Graybox Software
//
//  This example demonstrates the basic OPC server for
//  a simulated generic automation device.
//
//  The simulated device has a number of holding registers,
//  which can be read or preset, and a number of input
//  registers, which are read only.
//  A saw waves are simulated on the device inputs.
//  New values of the input registers are generated each
//  100 ms.
//  The device simulator occasionally produces the
//  communication errors during the simulated reading and
//  writing of its registers.
//
//  The OPC server provides an OPC tag for each device
//  register. Device polling starts with 500 ms updaterate,
//  when the OPC clients requests the OPC tags.
//
//  To register the OPC server type:
//    clrtagpolling -r
//  To remove the OPC server registration type:
//    clrtagpolling -u
//
//////////////////////////////////////////////////////////

using System;
using System.Threading;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Graybox.OPC.ServerToolkit.CLRWrapper;

// Assembly attributes.
[assembly: AssemblyTitle("ClrTagPolling")]
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

namespace ClrTagPolling
{
    /// <summary>
    /// This class is simulating a physical device from which
    /// the OPC server reads the data.
    /// The simulated device is assumed to have a number floating point registers.
    /// The device have 5 holding register, which can be read
    /// with ReadInputRegister or preset with WriteHoldingRegister.
    /// And there are 5 input register, which can be read with ReadInputRegister.
    /// Input registers have random values.
    /// Each of these functions simulate a failure of reading or writing in
    /// the random moments of time, simulating communication errors.
    /// Values of the input registers are constantly incremented by 0.1 units to
    /// simulated the saw wave.
    /// </summary>
    public class Device
    {
        /// <summary>
        /// This structure represent a pair on value of the device register and
        /// the flag that indicates the quality of this value.
        /// </summary>
        public struct DeviceRegValue
        {
            public double Value;
            public bool ValueIsOk;
            public DeviceRegValue(double value, bool valueIsOk)
            {
                Value = value;
                ValueIsOk = valueIsOk;
            }
        }
        /// <summary>
        /// Random numbers generator used in the simulation.
        /// </summary>
        private static Random m_random = new Random();
        /// <summary>
        /// The array that simulates the input registers.
        /// </summary>
        private double[] m_inputRegisters = new double[5];
        /// <summary>
        /// The array that simulates the holding registers.
        /// </summary>
        private double[] m_holdingRegisters = new double[5];
        /// <summary>
        /// Timer used to generate the values of the input registers.
        /// </summary>
        private Timer m_simTimer;
        /// <summary>
        /// Generate new values of the input registers. This function simulates
        /// the internal activity of the device.
        /// </summary>
        /// <param name="param">The Device object.</param>
        private void Simulate(object param)
        {
            Device thisDevice = (Device)param;
            lock (thisDevice)
            {
                for (int i = 0; i < 5; i++)
                {
                    m_inputRegisters[i] += 0.1;
                    if (m_inputRegisters[i] > 100.0) m_inputRegisters[i] = 0.0;
                }
            }
        }
        /// <summary>
        /// Initializes a new object of the Device calss.
        /// </summary>
        public Device()
        {
            // Initialize input register with the random values.
            for (int i = 0; i < 5; i++)
            {
                m_inputRegisters[i] = m_random.NextDouble() * 100;
            }
            // Create a timer that will call the Simulate function
            // to simulate the saw wave on each input register.
            m_simTimer = new Timer(new TimerCallback(Simulate), this, 0, 100);
        }
        /// <summary>
        /// Releases all resources used by the Device object.
        /// </summary>
        ~Device()
        {
            m_simTimer.Dispose();
        }
        /// <summary>
        /// Simulates the reading of the input register.
        /// </summary>
        /// <param name="registerIndex">The index of the input register to read.</param>
        /// <returns>A DeviceRegValue structure.</returns>
        public DeviceRegValue ReadInputRegister(int registerIndex)
        {
            // Simulate the communication failure with the 20% probability.
            if (m_random.Next(0, 99) > 80) return new DeviceRegValue(0, false);
            // Lock this Device object to prevent the simultaneous access
            // to the m_inputRegisters array from the multiple threads.
            lock (this)
            {
                // Return the current value of the input register and mark
                // this value as 'good' (no read error).
                return new DeviceRegValue(m_inputRegisters[registerIndex], true);
            }
        }
        /// <summary>
        /// Simulates the reading of the holding register.
        /// </summary>
        /// <param name="registerIndex">The index of the holding register to read.</param>
        /// <returns>A DeviceRegValue structure.</returns>
        public DeviceRegValue ReadHoldingRegister(int registerIndex)
        {
            // Simulate the communication failure with the 20% probability.
            if (m_random.Next(0, 99) > 80) return new DeviceRegValue(0, false);
            // Lock this Device object to prevent the simultaneous access
            // to the m_holdingRegisters array from the multiple threads.
            lock (this)
            {
                // Return the current value of the holding register and mark
                // this value as 'good' (no read error).
                return new DeviceRegValue(m_holdingRegisters[registerIndex], true);
            }
        }
        /// <summary>
        /// Simulates the writing of the holding register.
        /// </summary>
        /// <param name="registerIndex">The index of the holding register to write.</param>
        /// <param name="value">A new value of the register.</param>
        /// <returns><c>false</c> if there was a communication error with the device.</returns>
        public bool WriteHoldingRegister(int registerIndex, double value)
        {
            // Simulate the communication failure with the 20% probability.
            if (m_random.Next(0, 99) > 80) return false;
            // Lock this Device object to prevent the simultaneous access
            // to the m_holdingRegisters array from the multiple threads.
            lock (this)
            {
                // Write a new value into the holding register.
                m_holdingRegisters[registerIndex] = value;
            }
            // Report that writing has succeeded.
            return true;
        }
    }

    /// <summary>
    /// Register types enumeration.
    /// </summary>
    public enum RegisterType
    {
        /// <summary>
        /// A holding register. Used to store an arbitrary floating point value.
        /// This register is available for reading and writing.
        /// </summary>
        Holding,
        /// <summary>
        /// An input register. This register is read only.
        /// </summary>
        Input
    }

    /// <summary>
    /// Describes a tag of the OPC server.
    /// </summary>
    public class TagDescription
    {
        /// <summary>
        /// Initializes a new instance of the TagDescription class.
        /// </summary>
        /// <param name="registerType">Type of the register that corresponds to the OPC tag,
        /// described by this TagDescription object.</param>
        /// <param name="registerIndex">An index of the register of an underlying physical device.
        /// OPC tag, described by this object of the TagDescription class, represents that register.</param>
        /// <param name="tagId">A TagId identifier of the OPC tag,
        /// described by this TagDescription object.</param>
        public TagDescription(RegisterType registerType, int registerIndex, int tagId)
        {
            m_registerType = registerType;
            m_registerIndex = registerIndex;
            m_tagId = tagId;
            m_isActive = 0;
        }
        RegisterType m_registerType;
        int m_registerIndex;
        int m_tagId;
        int m_isActive;
        /// <summary>
        /// Gets the index of the register of an underlying physical device.
        /// OPC tag, described by this object of the TagDescription class, represents that register.
        /// </summary>
        public int RegisterIndex { get { return m_registerIndex; } }
        /// <summary>
        /// Gets the register type of an underlying physical device.
        /// </summary>
        public RegisterType RegisterType { get { return m_registerType; } }
        /// <summary>
        /// A TagId identifier of the OPC tag. This id is return by the <c>OPCDAServer.CreateTag</c> function.
        /// </summary>
        public int TagId { get { return m_tagId; } }
        /// <summary>
        /// Gets or sets the value indicating whether the OPC tag is active.
        /// If tag is active then it should be periodically polled from the underlying physical device.
        /// </summary>
        public bool Active
        {
            // Use interlocked access to the m_isActive member to workaround the multithreading.
            get { return ((Interlocked.CompareExchange(ref m_isActive, 0, 0) != 0)); }
            set { Interlocked.Exchange(ref m_isActive, value ? 1 : 0); }
        }
    }

    /// <summary>
    /// A class that contains the application entry point.
    /// </summary>
    static class Program
    {
        // The Device object representing the physical device.
        static Device device = new Device();
        // The OPC server object used to provide the OPC interface for the
        // physical device.
        static OPCDAServer srv = new OPCDAServer();
        // This event will be signaled, when the OPC server is release by the clients.
        static AutoResetEvent eventStop = new AutoResetEvent(false);
        // A list containig the descriptions of the OPC tags.
        static List<TagDescription> tagDescrs = new List<TagDescription>();
        // The process entry point. Set [MTAThread] to enable free threading.
        // Free threading is required by the OPC Toolkit.
        [MTAThread]
        static void Main(string[] args)
        {
            // This will be the CLSID and the AppID of our COM-object.
            Guid srvGuid = new Guid("F041E2EB-067B-4aa0-8557-5C5B2A6BAFDA");
            // Parse the command line args.
            if (args.Length > 0)
            {
                try
                {
                    // Register the OPC server and exit.
                    if (args[0].IndexOf("-r") != -1)
                    {
                        OPCDAServer.RegisterServer(
                            srvGuid,
                            "Graybox Software",
                            "ClrTagPolling",
                            "Graybox.Sample.ClrTagPolling",
                            "1.0");
                        return;
                    }
                    // Unregister the OPC server and exit.
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
            // Advise for the OPC Toolkit events.
            srv.Events.ServerReleased += new ServerReleasedEventHandler(Events_ServerReleased);
            srv.Events.ReadItems += new ReadItemsEventHandler(Events_ReadItems);
            srv.Events.WriteItems += new WriteItemsEventHandler(Events_WriteItems);
            srv.Events.ActivateItems += new ActivateItemsEventHandler(Events_ActivateItems);
            srv.Events.DeactivateItems += new DeactivateItemsEventHandler(Events_DeactivateItems);
            // Initialize the OPC server object and the OPC Toolkit.
            srv.Initialize(srvGuid, 50, 50, ServerOptions.NoAccessPaths, '.', 100);
            // Now we create the OPC tags and fill in the tagDescrs list.
            // We place the index of the TagDescription object in the tagDescrs container
            // into userId of the tag, that is described by that TagDescription object.
            // Also we place a TagId identifier of each tag into the corresponding TagDescription object.
            for (int i = 0; i < 5; i++)
            {
                int tagId;
                // Create an OPC tag for the holding register.
                tagId = srv.CreateTag(
                    tagDescrs.Count,
                    "Device.HoldingRegisters.Reg" + i.ToString(),
                    AccessRights.readWritable,
                    (double)0);
                // Add a description of the created tag to the tagDescrs.
                tagDescrs.Add(new TagDescription(RegisterType.Holding, i, tagId));
                // Create an OPC tag for the input register.
                tagId = srv.CreateTag(
                    tagDescrs.Count,
                    "Device.InputRegisters.Reg" + i.ToString(),
                    AccessRights.readable,
                    (double)0);
                // Add a description of the created tag to the tagDescrs.
                tagDescrs.Add(new TagDescription(RegisterType.Input, i, tagId));
            }
            // Mark the OPC server COM object as running.
            srv.RegisterClassObject();
            // Wait until the OPC server is released.
            // While it is not released, poll the device registers each 500 ms.
            // We only poll the registers that is requested by the OPC clients.
            while (!eventStop.WaitOne(500, false))
            {
                // Begin the update of the OPC server cache.
                srv.BeginUpdate();
                foreach (TagDescription tagDescr in tagDescrs)
                {
                    // Poll the device. Read the device register if the
                    // corresponding OPC tag is requseted by the OPC clients.
                    if (tagDescr.Active) ReadDevice(tagDescr);
                }
                // Finish the update of the OPC server cache. We pass false,
                // because its unnecessary for this update to be synchronous.
                srv.EndUpdate(false);
            }
            // Mark the OPC server COM object as stopped.
            srv.RevokeClassObject();
        }

        /// <summary>
        /// Polls the device.
        /// </summary>
        /// <param name="tagDescr">Contains information about the OPC tag and the
        /// corresponding device register.</param>
        static void ReadDevice(TagDescription tagDescr)
        {
            Device.DeviceRegValue deviceValue;
            // Perform the device reading.
            switch (tagDescr.RegisterType)
            {
                case RegisterType.Holding:
                    deviceValue = device.ReadHoldingRegister(tagDescr.RegisterIndex);
                    break;
                case RegisterType.Input:
                    deviceValue = device.ReadInputRegister(tagDescr.RegisterIndex);
                    break;
                default:
                    throw new NotSupportedException();
            }
            if (deviceValue.ValueIsOk)
            {
                // Place the read value into the OPC server cache,
                // since the device reading completed successfully.
                srv.SetTag(tagDescr.TagId, deviceValue.Value);
            }
            else
            {
                // There was a communication failure.
                // Set the quality of the OPC tag to indicate that. Do not update
                // the tag data value.
                srv.SetTag(tagDescr.TagId, new Quality(QualityBits.badCommFailure));
            }
        }

        /// <summary>
        /// Modifies the Active property of the TagDescription objects
        /// in the tagDescrs list.
        /// </summary>
        /// <param name="items">An array of an OPC item identifiers.</param>
        /// <param name="active">New value of the Active property.</param>
        static void ChangeTagActivation(ItemId[] items, bool active)
        {
            for (int i = 0; i < items.Length; i++)
            {
                // Skip items with the zero TagId.
                if (items[i].TagId == 0) continue;
                // Use the UserId to retrieve a TagDescription object, that
                // describes the OPC tag, that has became active (or inactive).
                TagDescription tagDescr = tagDescrs[items[i].UserId];
                // Write the Active property.
                tagDescr.Active = active;
            }
        }

        /// <summary>
        /// A handler for the DeactivateItems event of the OPCDAServer object.
        /// </summary>
        static void Events_DeactivateItems(object sender, DeactivateItemsArgs e)
        {
            ChangeTagActivation(e.ItemIds, false);
        }

        /// <summary>
        /// A handler for the ActivateItems event of the OPCDAServer object.
        /// </summary>
        static void Events_ActivateItems(object sender, ActivateItemsArgs e)
        {
            ChangeTagActivation(e.ItemIds, true);
        }

        /// <summary>
        /// A handler for the WriteItems event of the OPCDAServer object.
        /// We do not update the OPC server cache here.
        /// </summary>
        static void Events_WriteItems(object sender, WriteItemsArgs e)
        {
            try
            {
                // Say that we don't the OPC Toolkit to copy the tag values,
                // contained in the WriteItemsArgs, to the OPC server cache.
                e.CopyToCache = false;
                // Iterate through the requested items.
                for (int i = 0; i < e.Count; i++)
                {
                    // Skip items with the zero TagId.
                    if (e.ItemIds[i].TagId == 0) continue;
                    try
                    {
                        // Use the UserId to retrieve a TagDescription object, that
                        // describes the OPC tag, that has became active (or inactive).
                        TagDescription tagDescr = tagDescrs[e.ItemIds[i].UserId];
                        bool writtenOk;
                        // Try to convert the values being written to the data type, that
                        // is acceptable by the device.
                        double newValue = ((IConvertible)e.Values[i]).ToDouble(e.Culture);
                        // Write a new value to the device.
                        writtenOk = device.WriteHoldingRegister(tagDescr.RegisterIndex, newValue);
                        // Throw an exception if there was a communicaiton error.
                        if (!writtenOk) throw new Exception();
                    }
                    catch (Exception ex)
                    {
                        // Indicate that error has occured during the tag writing.
                        e.Errors[i] = ErrorCodes.Fail;
                        throw ex;
                    }
                }
            }
            catch
            {
                // Indicate that there was some error during the tags writing operation.
                e.MasterError = ErrorCodes.False;
            }
        }

        /// <summary>
        /// A handler for the ReadItems event of the OPCDAServer object.
        /// </summary>
        static void Events_ReadItems(object sender, ReadItemsArgs e)
        {
            try
            {
                // Let the Toolkit know that we don't return tag values
                // in the ReadItemsArgs.
                e.ValuesReturned = false;
                // Start the trnsaction of the OPC server cache update.
                srv.BeginUpdate();
                // Iterate through the requested items.
                for (int i = 0; i < e.Count; i++)
                {
                    // Skip items with the zero TagId.
                    if (e.ItemIds[i].TagId == 0) continue;
                    // Use the UserId to retrieve a TagDescription object, that
                    // describes the OPC tag, that has became active (or inactive).
                    TagDescription tagDescr = tagDescrs[e.ItemIds[i].UserId];
                    // Read the device and update the OPC tag value.
                    ReadDevice(tagDescr);
                }
            }
            finally
            {
                // Finish the OPC server cache update transaction. We pass true,
                // because it is necessary to wait until this transaction completes,
                // before the control returns to the calling client.
                // If refuse to do so, then the client will possibly recieve the old
                // tag values, because the new ones will not be placed in the OPC server
                // cache yet.
                srv.EndUpdate(true);
            }
        }

        /// <summary>
        /// A handler for the ServerReleased event of the OPCDAServer object.
        /// </summary>
        static void Events_ServerReleased(object sender, ServerReleasedArgs e)
        {
            // Signal to the main thread, that it's time to exit.
            eventStop.Set();
        }
    }
}
