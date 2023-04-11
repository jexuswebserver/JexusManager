// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Web.Administration
{
    public sealed class ApplicationPool : ConfigurationElement
    {
        private ApplicationPoolProcessModel _processModel;
        private ApplicationPoolRecycling _recycling;
        private ApplicationPoolFailure _failure;
        private ApplicationPoolCpu _cpu;

        private ObjectState? _state;
        internal const string ManagedRuntimeVersion40 = "v4.0";
        internal const string ManagedRuntimeVersion20 = "v2.0";
        internal readonly static string ManagedRuntimeVersionNone = string.Empty;

        internal ApplicationPool(ConfigurationElement element, ApplicationPoolCollection parent)
            : base(element, "add", null, parent, null, null)
        {
            Parent = parent;
        }

        internal ApplicationPoolCollection Parent { get; }
        internal ServerManager Server => Parent.Parent;

        public ObjectState Recycle()
        {
            State = ObjectState.Started;
            Server.Recycle(this);
            return State;
        }

        public ObjectState Start()
        {
            // TODO: add timeout.
            State = ObjectState.Starting;
            Server.Start(this);
            return State;
        }

        public ObjectState Stop()
        {
            // TODO: add timeout.
            State = ObjectState.Starting;
            Server.Stop(this);
            return State;
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

        public bool EnableEmulationOnWinArm64
        {
            get { return (bool)this["enableEmulationOnWinArm64"]; }
            set { this["enableEmulationOnWinArm64"] = value; }
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

        public string Name
        {
            get { return this["name"].ToString(); }
            set { this["name"] = value; }
        }

        public ApplicationPoolProcessModel ProcessModel
        {
            get { return _processModel ?? (_processModel = new ApplicationPoolProcessModel(ChildElements["processModel"], this)); }
        }

        public long QueueLength
        {
            get { return Convert.ToInt64((uint)this["queueLength"]); }
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

        public ObjectState State
        {
            get
            {
                if (_state == null || _state == ObjectState.Starting || _state == ObjectState.Stopping)
                {
                    var result = GetState();
                    _state = result ? ObjectState.Started : ObjectState.Stopped;
                }

                return _state.Value;
            }

            private set
            {
                _state = value;
            }
        }

        private bool GetState()
        {
            return Server.GetPoolState(this);
        }

        public WorkerProcessCollection WorkerProcesses
        {
            get { return new WorkerProcessCollection(); }
        }

        public override string ToString()
        {
            return Name;
        }

        internal int ApplicationCount { get; set; }
    }
}
