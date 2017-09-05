// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Web.Administration
{
    public sealed class ApplicationPoolDefaults : ConfigurationElement
    {
        private ApplicationPoolRecycling _recycling;
        private ApplicationPoolProcessModel _model;
        private ApplicationPoolFailure _failure;
        private ApplicationPoolCpu _cpu;

        internal ApplicationPoolDefaults(ConfigurationElement element, ConfigurationElement parent)
            : base(element, "applicationPoolDefaults", null, parent, null, null)
        {
        }

        public bool AutoStart
        {
            get { return (bool)this["autoStart"]; }
            set { this["autoStart"] = value; }
        }

        public ApplicationPoolCpu Cpu
        {
            get { return _cpu ?? (_cpu = new ApplicationPoolCpu(ChildElements["cpu"], this)); }
        }

        public bool Enable32BitAppOnWin64
        {
            get { return (bool)this["enable32BitAppOnWin64"]; }
            set { this["enable32BitAppOnWin64"] = value; }
        }

        public ApplicationPoolFailure Failure
        {
            get { return _failure ?? (_failure = new ApplicationPoolFailure(ChildElements["failure"], this)); }
        }

        public ManagedPipelineMode ManagedPipelineMode
        {
            get { return (ManagedPipelineMode)Enum.ToObject(typeof(ManagedPipelineMode), this["managedPipelineMode"]); }
            set { this["managedPipelineMode"] = (long)value; }
        }

        public string ManagedRuntimeVersion
        {
            get { return this["managedRuntimeVersion"].ToString(); }
            set { this["managedRuntimeVersion"] = value; }
        }

        public ApplicationPoolProcessModel ProcessModel
        {
            get { return _model ?? (_model = new ApplicationPoolProcessModel(ChildElements["processModel"], this)); }
        }

        public long QueueLength
        {
            get { return Convert.ToInt64(this["queueLength"]); }
            set { this["queueLength"] = Convert.ToUInt32(value); }
        }

        public ApplicationPoolRecycling Recycling
        {
            get { return _recycling ?? (_recycling = new ApplicationPoolRecycling(ChildElements["recycling"], this)); }
        }

        public StartMode StartMode
        {
            get { return (StartMode)Enum.ToObject(typeof(StartMode), this["startMode"]); }
            set { this["startMode"] = (long)value; }
        }
    }
}
