// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.FastCgi
{
    using System.ComponentModel;

    using Microsoft.Web.Administration;

    internal class FastCgiItem : IItem<FastCgiItem>
    {
        public FastCgiItem(ConfigurationElement element)
        {
            Element = element;
            Flag = element == null || element.IsLocallyStored ? "Local" : "Inhertied";
            EnvironmentVariables = new EnvironmentVariablesCollection();
            AdvancedSettings = new AdvancedSettings();
            if (element == null)
            {
                Path = Arguments = MonitorChangesTo = string.Empty;
                MaxInstances = 4U;
                InstanceMaxRequests = 200U;
                ActivityTimeout = 30U;
                IdleTimeout = 300U;
                QueueLength = 1000U;
                RapidFailsPerMinute = 10U;
                RequestTimeout = 90U;
                return;
            }

            Reset();
        }

        public void Reset()
        {
            Path = (string)Element["fullPath"];
            Arguments = (string)Element["arguments"];

            MonitorChangesTo = (string)Element["monitorChangesTo"];
            ErrorMode = (ErrorMode)Element["stderrMode"];
            MaxInstances = (uint)Element["maxInstances"];
            IdleTimeout = (uint)Element["idleTimeout"];
            ActivityTimeout = (uint)Element["activityTimeout"];
            RequestTimeout = (uint)Element["requestTimeout"];
            InstanceMaxRequests = (uint)Element["instanceMaxRequests"];
            SignalBeforeTerminateSeconds = (uint)Element["signalBeforeTerminateSeconds"];
            AdvancedSettings.Protocol = (Protocol)Element["protocol"];
            QueueLength = (uint)Element["queueLength"];
            AdvancedSettings.FlushNamedPipe = (bool)Element["flushNamedPipe"];
            RapidFailsPerMinute = (uint)Element["rapidFailsPerMinute"];

            foreach (ConfigurationElement child in Element.GetCollection("environmentVariables"))
            {
                EnvironmentVariables.Add(
                    new EnvironmentVariables { Name = (string)child["name"], Value = (string)child["value"] });
            }
        }

        [Browsable(false)]
        public string Arguments { get; set; }

        [Browsable(false)]
        public string Path { get; set; }

        [Browsable(false)]
        public ConfigurationElement Element { get; set; }

        [Browsable(false)]
        public string Flag { get; set; }

        public bool Equals(FastCgiItem other)
        {
            // all properties
            return Match(other);
        }

        public void Apply()
        {
            Element["fullPath"] = Path;
            Element["arguments"] = Arguments;

            Element["monitorChangesTo"] = MonitorChangesTo;
            Element["stderrMode"] = ErrorMode;
            Element["maxInstances"] = MaxInstances;
            Element["idleTimeout"] = IdleTimeout;
            Element["activityTimeout"] = ActivityTimeout;
            Element["requestTimeout"] = RequestTimeout;
            Element["instanceMaxRequests"] = InstanceMaxRequests;
            Element["signalBeforeTerminateSeconds"] = SignalBeforeTerminateSeconds;
            Element["protocol"] = AdvancedSettings.Protocol;
            Element["queueLength"] = QueueLength;
            Element["flushNamedPipe"] = AdvancedSettings.FlushNamedPipe;
            Element["rapidFailsPerMinute"] = RapidFailsPerMinute;

            var collection = Element.GetCollection("environmentVariables");
            collection.Clear();
            foreach (EnvironmentVariables item in EnvironmentVariables)
            {
                var newElement = collection.CreateElement();
                newElement["name"] = item.Name;
                newElement["value"] = item.Value;
                collection.Add(newElement);
            }
        }

        public bool Match(FastCgiItem other)
        {
            // match combined keys.
            return other != null && other.Arguments == Arguments && other.Path == Path;
        }

        [Browsable(true)]
        [Category("General")]
        [Description("Specifies optional environment variables that will be set in the FastCGI executable.")]
        [DisplayName("Environment Variables")]
        [Editor(typeof(EnvironmentVariablesCollectionEditor),
            typeof(System.Drawing.Design.UITypeEditor))]
        public EnvironmentVariablesCollection EnvironmentVariables { get; set; }

        [Browsable(true)]
        [Category("General")]
        [Description("Specifies the number of requests a FastCGI process for this application is allowed to handle.")]
        [DisplayName("Instance MaxRequests")]
        [DefaultValue(200U)]
        public uint InstanceMaxRequests { get; set; }

        [Browsable(true)]
        [Category("General")]
        [Description("Specifies the maximum number of FastCGI process that are allowed in the application's process pool.")]
        [DisplayName("Max Instances")]
        [DefaultValue(4U)]
        public uint MaxInstances { get; set; }

        [Browsable(true)]
        [Category("General")]
        [Description("Specifies path to a file changes to which will trigger recycle of FastCGI processes.")]
        [DisplayName("Monitor changes to file")]
        [DefaultValue("")]
        [Editor(typeof(System.Windows.Forms.Design.FileNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string MonitorChangesTo { get; set; }

        [Browsable(true)]
        [Category("General")]
        [Description("Specifies how FastCGI module should behave when FastCGI process sends text on standard error stream.")]
        [DisplayName("Standard error mode")]
        [TypeConverter(typeof(DescriptionConverter))]
        public ErrorMode ErrorMode { get; set; }

        [Browsable(true)]
        [Category("Process Model")]
        [Description("Specifies the time, in seconds, that a FastCGI process for this application is allowed to run without communicating with IIS.")]
        [DisplayName("Activity Timeout")]
        [DefaultValue(30U)]
        public uint ActivityTimeout { get; set; }

        [Browsable(true)]
        [Category("Process Model")]
        [DisplayName("Advanced Settings")]
        public AdvancedSettings AdvancedSettings { get; set; }

        [Browsable(true)]
        [Category("Process Model")]
        [Description("Specifies the time, in seconds, that a FastCGI process for this application is allowed to remain idle.")]
        [DisplayName("Idle Timeout")]
        [DefaultValue(300U)]
        public uint IdleTimeout { get; set; }

        [Browsable(true)]
        [Category("Process Model")]
        [Description("Maximum number of requests that are permitted into FastCGI handler queue.")]
        [DisplayName("Queue Length")]
        [DefaultValue(1000U)]
        public uint QueueLength { get; set; }

        [Browsable(true)]
        [Category("Process Model")]
        [Description("Specifies the number of FastCGI process failures allowed in a single minute before the FastCGI handler takes it off line.")]
        [DisplayName("Rapid Fails PerMinute")]
        [DefaultValue(10U)]
        public uint RapidFailsPerMinute { get; set; }

        [Browsable(true)]
        [Category("Process Model")]
        [Description("Specifies the maximum allowed time, in seconds, for request processing.")]
        [DisplayName("Request Timeout")]
        [DefaultValue(90U)]
        public uint RequestTimeout { get; set; }

        [Browsable(true)]
        [Category("Process Model")]
        [Description("Specifies the amount of time, in seconds, that IIS will wait after IIS signals a FastCGI application that it needs to shut down.")]
        [DisplayName("Signal Before Terminate")]
        [DefaultValue(300U)]
        public uint SignalBeforeTerminateSeconds { get; set; }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    internal class AdvancedSettings
    {
        [Browsable(true)]
        [Description("Specifies whether or not the named pipe between FastCGI processes for this application is flushed before shutting down the application.")]
        [DisplayName("FlushNamedpipe")]
        public bool FlushNamedPipe { get; set; }

        [Browsable(true)]
        [Description("Specifies protocol to be used to communicate with FastCGI process.")]
        [DisplayName("Protocol")]
        [TypeConverter(typeof(DescriptionConverter))]
        public Protocol Protocol { get; set; }

        public override string ToString()
        {
            return string.Empty;
        }
    }
}
