// Copyright (c) Lex Li. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Web.Administration;
using Microsoft.Web.Management.Server;

namespace JexusManager.Features.Asp
{
    internal sealed class AspService : ModuleService
    {
        private const string SectionPath = "system.webServer/asp";

        [ModuleServiceMethod]
        public AspItem GetSettings()
        {
            var element = GetSection();
            var session = element.ChildElements["session"];
            var comPlus = element.ChildElements["comPlus"];
            var cache = element.ChildElements["cache"];
            var limits = element.ChildElements["limits"];
            var flags = (long)comPlus["appServiceFlags"];
            return new AspItem
            {
                CodePage = (uint)element["codePage"],
                BufferingOn = (bool)element["bufferingOn"],
                EnableChunkedEncoding = (bool)element["enableChunkedEncoding"],
                EnableAspHtmlFallback = (bool)element["enableAspHtmlFallback"],
                EnableParentPaths = (bool)element["enableParentPaths"],
                Lcid = (uint)element["lcid"],
                EnableApplicationRestart = (bool)element["enableApplicationRestart"],
                ScriptLanguage = (string)element["scriptLanguage"],
                SessionProperties = new SessionProperties
                {
                    AllowSessionState = (bool)session["allowSessionState"],
                    KeepSessionIdSecure = (bool)session["keepSessionIdSecure"],
                    Max = (uint)session["max"],
                    Timeout = (TimeSpan)session["timeout"]
                },
                ComPlusProperties = new ComPlusProperties
                {
                    EnableSxS = (flags & 2L) == 2L,
                    EnableTracker = (flags & 1L) == 1L,
                    UsePartition = (flags & 4L) == 4L,
                    SxsName = (string)comPlus["sxsName"],
                    PartitionId = (string)comPlus["partitionId"],
                    TrackThreadingModel = (bool)comPlus["trackThreadingModel"],
                    ExecuteInMta = (bool)comPlus["executeInMta"]
                },
                CachingProperties = new CachingProperties
                {
                    DiskTemplateCacheDirectory = (string)cache["diskTemplateCacheDirectory"],
                    EnableTypelibCache = (bool)cache["enableTypelibCache"],
                    MaxDiskTemplateCacheFiles = (uint)cache["maxDiskTemplateCacheFiles"],
                    ScriptFileCacheSize = (uint)cache["scriptFileCacheSize"],
                    ScriptEngineCacheMax = (uint)cache["scriptEngineCacheMax"]
                },
                DebuggingProperties = new DebuggingProperties
                {
                    CalcLineNumber = (bool)element["calcLineNumber"],
                    ExceptionCatchEnable = (bool)element["exceptionCatchEnable"],
                    AppAllowClientDebug = (bool)element["appAllowClientDebug"],
                    LogErrorRequests = (bool)element["logErrorRequests"],
                    AppAllowDebugging = (bool)element["appAllowDebugging"],
                    ErrorsToNTLog = (bool)element["errorsToNTLog"],
                    RunOnEndAnonymously = (bool)element["runOnEndAnonymously"],
                    ScriptErrorMessage = (string)element["scriptErrorMessage"],
                    ScriptErrorSentToBrowser = (bool)element["scriptErrorSentToBrowser"]
                },
                LimitsProperties = new LimitsProperties
                {
                    QueueConnectionTestTime = (TimeSpan)limits["queueConnectionTestTime"],
                    MaxRequestEntityAllowed = (uint)limits["maxRequestEntityAllowed"],
                    RequestQueueMax = (uint)limits["requestQueueMax"],
                    QueueTimeout = (TimeSpan)limits["queueTimeout"],
                    BufferingLimit = (uint)limits["bufferingLimit"],
                    ScriptTimeout = (TimeSpan)limits["scriptTimeout"],
                    ProcessorThreadMax = (uint)limits["processorThreadMax"]
                }
            };
        }

        [ModuleServiceMethod]
        public void Apply(AspItem settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            var element = GetSection();
            element["codePage"] = settings.CodePage;
            element["bufferingOn"] = settings.BufferingOn;
            element["enableChunkedEncoding"] = settings.EnableChunkedEncoding;
            element["enableAspHtmlFallback"] = settings.EnableAspHtmlFallback;
            element["enableParentPaths"] = settings.EnableParentPaths;
            element["lcid"] = settings.Lcid;
            element["enableApplicationRestart"] = settings.EnableApplicationRestart;
            element["scriptLanguage"] = settings.ScriptLanguage;

            var session = element.ChildElements["session"];
            var sessionSettings = settings.SessionProperties ?? throw new ArgumentException("Session settings are required.", nameof(settings));
            session["allowSessionState"] = sessionSettings.AllowSessionState;
            session["keepSessionIdSecure"] = sessionSettings.KeepSessionIdSecure;
            session["max"] = sessionSettings.Max;
            session["timeout"] = sessionSettings.Timeout;

            var comPlus = element.ChildElements["comPlus"];
            var comPlusSettings = settings.ComPlusProperties ?? throw new ArgumentException("COM+ settings are required.", nameof(settings));
            long flags = 0;
            if (comPlusSettings.EnableSxS)
            {
                flags |= 2L;
            }

            if (comPlusSettings.EnableTracker)
            {
                flags |= 1L;
            }

            if (comPlusSettings.UsePartition)
            {
                flags |= 4L;
            }

            comPlus["appServiceFlags"] = flags;
            var sxsName = comPlus.GetAttribute("sxsName");
            if (string.IsNullOrWhiteSpace(comPlusSettings.SxsName))
            {
                sxsName.Delete();
            }
            else
            {
                sxsName.Value = comPlusSettings.SxsName;
            }

            comPlus["partitionId"] = comPlusSettings.PartitionId;
            comPlus["trackThreadingModel"] = comPlusSettings.TrackThreadingModel;
            comPlus["executeInMta"] = comPlusSettings.ExecuteInMta;

            var cache = element.ChildElements["cache"];
            var cacheSettings = settings.CachingProperties ?? throw new ArgumentException("Cache settings are required.", nameof(settings));
            cache["diskTemplateCacheDirectory"] = cacheSettings.DiskTemplateCacheDirectory;
            cache["enableTypelibCache"] = cacheSettings.EnableTypelibCache;
            cache["maxDiskTemplateCacheFiles"] = cacheSettings.MaxDiskTemplateCacheFiles;
            cache["scriptFileCacheSize"] = cacheSettings.ScriptFileCacheSize;
            cache["scriptEngineCacheMax"] = cacheSettings.ScriptEngineCacheMax;

            var debuggingSettings = settings.DebuggingProperties ?? throw new ArgumentException("Debugging settings are required.", nameof(settings));
            element["calcLineNumber"] = debuggingSettings.CalcLineNumber;
            element["exceptionCatchEnable"] = debuggingSettings.ExceptionCatchEnable;
            element["appAllowClientDebug"] = debuggingSettings.AppAllowClientDebug;
            element["logErrorRequests"] = debuggingSettings.LogErrorRequests;
            element["appAllowDebugging"] = debuggingSettings.AppAllowDebugging;
            element["errorsToNTLog"] = debuggingSettings.ErrorsToNTLog;
            element["runOnEndAnonymously"] = debuggingSettings.RunOnEndAnonymously;
            element["scriptErrorMessage"] = debuggingSettings.ScriptErrorMessage;
            element["scriptErrorSentToBrowser"] = debuggingSettings.ScriptErrorSentToBrowser;

            var limits = element.ChildElements["limits"];
            var limitSettings = settings.LimitsProperties ?? throw new ArgumentException("Limit settings are required.", nameof(settings));
            limits["queueConnectionTestTime"] = limitSettings.QueueConnectionTestTime;
            limits["maxRequestEntityAllowed"] = limitSettings.MaxRequestEntityAllowed;
            limits["requestQueueMax"] = limitSettings.RequestQueueMax;
            limits["queueTimeout"] = limitSettings.QueueTimeout;
            limits["bufferingLimit"] = limitSettings.BufferingLimit;
            limits["scriptTimeout"] = limitSettings.ScriptTimeout;
            limits["processorThreadMax"] = limitSettings.ProcessorThreadMax;
            ManagementUnit.Update();
        }

        private ConfigurationSection GetSection()
        {
            return ManagementUnit.Configuration.GetSection(SectionPath);
        }
    }
}
