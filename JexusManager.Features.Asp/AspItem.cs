// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Asp
{
    using System.ComponentModel;

    using System;

    [Serializable]
    public class AspItem
    {
        [Browsable(true)]
        [Category("Behavior")]
        [Description("Specifies the default character set for an application. For example, code page 1252 supports a Latin character set used in American English and many European alphabets.")]
        [DisplayName("Code Page")]
        [DefaultValue(0)]
        public uint CodePage { get; set; }

        [Browsable(true)]
        [Category("Behavior")]
        [Description("Specifies whether output from an ASP application will be buffered.")]
        [DisplayName("Enable Buffering")]
        [DefaultValue(true)]
        public bool BufferingOn { get; set; }

        [Browsable(true)]
        [Category("Behavior")]
        [Description("Specifies whether HTTP 1.1 chunked transfer encoding is enabled for the World Wide Web Publishing Service (WWW service).")]
        [DisplayName("Enable Chunked Encoding")]
        [DefaultValue(true)]
        public bool EnableChunkedEncoding { get; set; }

        [Browsable(true)]
        [Category("Behavior")]
        [Description("Controls the behavior of ASP when a new request is to be rejected due to a full request queue. A true value for this property will cause an .htm file with a similar name as the requested .asp file, if it exists, to be sent instead of the .asp file. The naming convention for the .htm file is the name of the .asp file with _asp appended. For example, if the .asp file is hello.asp, then the .htm file should be hello_asp.htm")]
        [DisplayName("Enable HTML Fallback")]
        [DefaultValue(true)]
        public bool EnableAspHtmlFallback { get; set; }

        [Browsable(true)]
        [Category("Behavior")]
        [Description("Specifies whether an ASP page allows paths relative to the current directory (using the ..\\ notation) or above the current directory.")]
        [DisplayName("Enable Parent Paths")]
        [DefaultValue(false)]
        public bool EnableParentPaths { get; set; }

        [Browsable(true)]
        [Category("Behavior")]
        [Description("Properties related to ASP limits.")]
        [DisplayName("Limits Properties")]
        public LimitsProperties LimitsProperties { get; set; }

        [Browsable(true)]
        [Category("Behavior")]
        [Description("This is the default locale identifier (LCID) for an application. LCIDs define how dates, times, and currencies are formatted.")]
        [DisplayName("Locale ID")]
        [DefaultValue(0)]
        public uint Lcid { get; set; }

        [Browsable(true)]
        [Category("Behavior")]
        [Description("Specifies whether an ASP application is automatically restarted.")]
        [DisplayName("Restart On Config Change")]
        [DefaultValue(true)]
        public bool EnableApplicationRestart { get; set; }

        [Browsable(true)]
        [Category("Compilation")]
        [Description("Properties related to debug settings.")]
        [DisplayName("Debugging Properties")]
        public DebuggingProperties DebuggingProperties { get; set; }

        [Browsable(true)]
        [Category("Compilation")]
        [Description("Specifies the default script language for all ASP applications running on the Web server.")]
        [DisplayName("Script Language")]
        [DefaultValue("VBScript")]
        public string ScriptLanguage { get; set; }

        [Browsable(true)]
        [Category("Services")]
        [Description("Properties related to ASP cache.")]
        [DisplayName("Caching Properties")]
        public CachingProperties CachingProperties { get; set; }

        [Browsable(true)]
        [Category("Services")]
        [Description("Properties related to ASP COM Plus.")]
        [DisplayName("COM Plus Properties")]
        public ComPlusProperties ComPlusProperties { get; set; }

        [Browsable(true)]
        [Category("Services")]
        [Description("Properties related to ASP session.")]
        [DisplayName("Session Properties")]
        public SessionProperties SessionProperties { get; set; }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    [Serializable]
    public class SessionProperties
    {
        [Browsable(true)]
        [Description("Enables session state persistence for the ASP application.")]
        [DisplayName("Enable Session State")]
        [DefaultValue(true)]
        public bool AllowSessionState { get; set; }

        [Browsable(true)]
        [Description("Specifies the maximum number of concurrent sessions that IIS allows.")]
        [DisplayName("Maximum Sessions")]
        [DefaultValue(4294967295)]
        public uint Max { get; set; }

        [Browsable(true)]
        [Description("When enabled, ensures that a SessionID is sent as a secure cookie if assigned over a secure channel.")]
        [DisplayName("New ID On Secure Connection")]
        [DefaultValue(true)]
        public bool KeepSessionIdSecure { get; set; }

        [Browsable(true)]
        [Description("Specifies the default amount of time that a session object is maintained after the last request associated with the object is made.")]
        [DisplayName("Time-out")]
        [DefaultValue(typeof(TimeSpan), "00:20:00")]
        public TimeSpan Timeout { get; set; }

        public override string ToString()
        {
            return string.Empty;
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    [Serializable]
    public class ComPlusProperties
    {
        [Browsable(true)]
        [Description("A value of true enables COM+ side-by-side assemblies, which allow ASP applications to specify which version of a system DLL or classic COM component to use, such as MDAC, MFS, MSVCRT, MSXML, and so on.")]
        [DisplayName("Enable Side by Side Component")]
        [DefaultValue(false)]
        public bool EnableSxS { get; set; }

        [Browsable(true)]
        [Description("A value of true enables COM+ tracker, which allows administrators or developers to debug ASP applications.")]
        [DisplayName("Enable Tracker")]
        [DefaultValue(false)]
        public bool EnableTracker { get; set; }

        [Browsable(true)]
        [Description("Specifies whether ASP should run in a multithreaded environment.")]
        [DisplayName("Execute In MTA")]
        [DefaultValue(false)]
        public bool ExecuteInMta { get; set; }

        [Browsable(true)]
        [Description("Specifies whether IIS checks the threading model of any components that your application creates.")]
        [DisplayName("Honor Component Threading Model")]
        [DefaultValue(false)]
        public bool TrackThreadingModel { get; set; }

        [Browsable(true)]
        [Description("Set this property to the Globally Unique Identifier (GUID) of the COM+ partition. Also, set the Use Partition flag to true.")]
        [DisplayName("Partition ID")]
        [DefaultValue("00000000-0000-0000-0000-000000000000")]
        public string PartitionId { get; set; }

        [Browsable(true)]
        [Description("Set this property to the name of the COM+ application. Also, set the enable side-by-side flag to true.")]
        [DisplayName("Side By Side Component")]
        [DefaultValue("")]
        public string SxsName { get; set; }

        [Browsable(true)]
        [Description("A value of true enables COM+ partitioning, which can be used to isolate applications into their own COM+ partitions. COM+ partitions can hold different versions of your own custom COM components. If you set this flag, also set the Partition ID property.")]
        [DisplayName("Use Partition")]
        [DefaultValue(false)]
        public bool UsePartition { get; set; }

        public override string ToString()
        {
            return string.Empty;
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    [Serializable]
    public class CachingProperties
    {
        [Browsable(true)]
        [Description("Contains the name of the directory that ASP uses to store compiled ASP templates to disk after overflow of the in-memory cache.")]
        [DisplayName("Cache Directory Path")]
        [DefaultValue("%SystemDrive%\\inetpub\\temp\\ASP Compiled Templates")]
        public string DiskTemplateCacheDirectory { get; set; }

        [Browsable(true)]
        [Description("Specifies whether type libraries are cached on the server.")]
        [DisplayName("Enable Type Library Caching")]
        [DefaultValue(true)]
        public bool EnableTypelibCache { get; set; }

        [Browsable(true)]
        [Description("Specifies the maximum number of compiled ASP templates that can be stored.")]
        [DisplayName("Maximum Disk Cached Files")]
        [DefaultValue(2000)]
        public uint MaxDiskTemplateCacheFiles { get; set; }

        [Browsable(true)]
        [Description("Specifies the number of precompiled script files to cache. If set to 0, no script files are cached. If set to 4294967295, all script files requested are cached. This property is used to tune performance, depending on the amount of available memory and the amount of script traffic.")]
        [DisplayName("Maximum Memory Cached Files")]
        [DefaultValue(500)]
        public uint ScriptFileCacheSize { get; set; }

        [Browsable(true)]
        [Description("Specifies the maximum number of scripting engines that ASP pages will keep cached in memory.")]
        [DisplayName("Maximum Script Engines Cached")]
        [DefaultValue(250)]
        public uint ScriptEngineCacheMax { get; set; }

        public override string ToString()
        {
            return string.Empty;
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    [Serializable]
    public class DebuggingProperties
    {
        [Browsable(true)]
        [Description("Specifies whether ASP calculates and stores the line number of each executed line of code to provide the number in an error report.")]
        [DisplayName("Calculate Line Numbers")]
        [DefaultValue(true)]
        public bool CalcLineNumber { get; set; }

        [Browsable(true)]
        [Description("Specifies whether ASP pages trap exceptions thrown by components. If set to false (or, disabled), the Microsoft Script Debugger tool does not catch exceptions sent by the component that you are debugging.")]
        [DisplayName("Catch COM Component Exceptions")]
        [DefaultValue(true)]
        public bool ExceptionCatchEnable { get; set; }

        [Browsable(true)]
        [Description("Specifies whether ASP client-side debugging is enabled.")]
        [DisplayName("Enable Client-side Debugging")]
        [DefaultValue(false)]
        public bool AppAllowClientDebug { get; set; }

        [Browsable(true)]
        [Description("Controls whether the Web server writes ASP errors to the application section of the Windows event log. ASP errors are written to the client browser and to the IIS log files by default.")]
        [DisplayName("Enable Log Error Requests")]
        [DefaultValue(true)]
        public bool LogErrorRequests { get; set; }

        [Browsable(true)]
        [Description("Specifies whether ASP debugging is enabled on the server.")]
        [DisplayName("Enable Server-side Debugging")]
        [DefaultValue(false)]
        public bool AppAllowDebugging { get; set; }

        [Browsable(true)]
        [Description("Specifies which ASP errors are written to the Windows event log. ASP errors are written to the client browser and to the IIS log files by default.")]
        [DisplayName("Log Errors to NT Log")]
        [DefaultValue(false)]
        public bool ErrorsToNTLog { get; set; }

        [Browsable(true)]
        [Description("Specifies whether the SessionOnEnd and ApplicationOnEnd global ASP functions should be run as the anonymous user.")]
        [DisplayName("Run On End Functions Anonymously")]
        [DefaultValue(true)]
        public bool RunOnEndAnonymously { get; set; }

        [Browsable(true)]
        [Description("Specifies the error message to send to the browser if specific debugging errors are not sent to the client.")]
        [DisplayName("Script Error Message")]
        [DefaultValue("An error occurred on the server when processing the URL. Please contact the system administrator. &lt;p/> If you are the system administrator please click &lt;a href=&quot;http://go.microsoft.com/fwlink/?LinkID=82731&quot;>here&lt;/a> to find out more about this error.")]
        public string ScriptErrorMessage { get; set; }

        [Browsable(true)]
        [Description("Specifies whether the Web server writes debugging specifics (file name, error, line number, description) to the client browser, in addition to logging them to the Windows Event Log.")]
        [DisplayName("Send Errors To Browser")]
        [DefaultValue(false)]
        public bool ScriptErrorSentToBrowser { get; set; }

        public override string ToString()
        {
            return string.Empty;
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    [Serializable]
    public class LimitsProperties
    {
        [Browsable(true)]
        [Description("When a request is queued longer than the time specified in this property, ASP checks whether the client is still connected before executing the request. If the client is no longer connected, the request is not processed and it is deleted from the queue.")]
        [DisplayName("Client Connection Test Interval")]
        [DefaultValue(typeof(TimeSpan), "00:00:03")]
        public TimeSpan QueueConnectionTestTime { get; set; }

        [Browsable(true)]
        [Description("Specifies the maximum number of bytes allowed in the entity body of an ASP request.")]
        [DisplayName("Maximum Requesting Entity Body Limit")]
        [DefaultValue(200000)]
        public uint MaxRequestEntityAllowed { get; set; }

        [Browsable(true)]
        [Description("Specifies the maximum number of concurrent ASP requests that are permitted into the queue.")]
        [DisplayName("Queue Length")]
        [DefaultValue(3000)]
        public uint RequestQueueMax { get; set; }

        [Browsable(true)]
        [Description("Specifies the amount of time that an ASP script request is allowed to wait in the queue.")]
        [DisplayName("Request Queue Time-out")]
        [DefaultValue(typeof(TimeSpan), "00:00:00")]
        public TimeSpan QueueTimeout { get; set; }

        [Browsable(true)]
        [Description("Sets the maximum size of the ASP buffer. When response buffering is enabled, this property controls the maximum number of bytes that an ASP page can write to the response buffer before a flush occurs.")]
        [DisplayName("Response Buffering Limit")]
        [DefaultValue(4194304)]
        public uint BufferingLimit { get; set; }

        [Browsable(true)]
        [Description("Specifies the default length of time that ASP pages allow a script to run before terminating the script and writing an event to the Windows Event Log.")]
        [DisplayName("Script Time-out")]
        [DefaultValue(typeof(TimeSpan), "00:01:30")]
        public TimeSpan ScriptTimeout { get; set; }

        [Browsable(true)]
        [Description("Specifies the maximum number of worker threads per processor that IIS may create.")]
        [DisplayName("Threads Per Processor Limit")]
        [DefaultValue(25)]
        public uint ProcessorThreadMax { get; set; }

        public override string ToString()
        {
            return string.Empty;
        }
    }
}
