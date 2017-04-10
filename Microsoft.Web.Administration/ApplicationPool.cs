// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Web.Administration
{
    using System.Collections.ObjectModel;
#if !__MonoCS__
    using System.Management.Automation;
#endif
    using System.Threading.Tasks;

    public sealed class ApplicationPool : ConfigurationElement
    {
        private ApplicationPoolProcessModel _processModel;
        private ApplicationPoolRecycling _recycling;
        private ApplicationPoolFailure _failure;
        private ApplicationPoolCpu _cpu;

        private ObjectState? _state;

        internal ApplicationPool(ConfigurationElement element, ApplicationPoolCollection parent)
            : base(element, "add", null, parent, null, null)
        {
            Parent = parent;
        }

        internal ApplicationPoolCollection Parent { get; private set; }

        public ObjectState Recycle()
        {
            State = ObjectState.Started;
            return State;
        }

        public ObjectState Start()
        {
            State = ObjectState.Started;
            return State;
        }

        public ObjectState Stop()
        {
            State = ObjectState.Stopped;
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
                if (_state == null)
                {
                    var result = AsyncHelper.RunSync(GetStateAsync);
                    _state = result ? ObjectState.Started : ObjectState.Stopped;
                }

                return _state.Value;
            }

            private set
            {
                _state = value;
            }
        }

        private async Task<bool> GetStateAsync()
        {
            return await Parent.Parent.GetPoolStateAsync(this);
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
