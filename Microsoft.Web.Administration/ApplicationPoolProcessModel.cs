// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Web.Administration
{
    public sealed class ApplicationPoolProcessModel : ConfigurationElement
    {
        internal ApplicationPoolProcessModel(ConfigurationElement parent)
            : this(null, parent)
        {
        }

        internal ApplicationPoolProcessModel(ConfigurationElement element, ConfigurationElement parent)
            : base(element, "processModel", null, parent, null, null)
        {
        }

        public ProcessModelIdentityType IdentityType
        {
            get { return (ProcessModelIdentityType)Enum.ToObject(typeof(ProcessModelIdentityType), this["identityType"]); }
            set { this["identityType"] = (long)value; }
        }

        public TimeSpan IdleTimeout
        {
            get { return (TimeSpan)this["idleTimeout"]; }
            set { this["idleTimeout"] = value; }
        }

        public IdleTimeoutAction IdleTimeoutAction
        {
            get { return (IdleTimeoutAction)Enum.ToObject(typeof(IdleTimeoutAction), this["idleTimeoutAction"]); }
            set { this["idleTimeoutAction"] = (long)value; }
        }

        public bool LoadUserProfile
        {
            get { return (bool)this["loadUserProfile"]; }
            set { this["loadUserProfile"] = value; }
        }

        public ProcessModelLogEventOnProcessModel LogEventOnProcessModel
        {
            get { return (ProcessModelLogEventOnProcessModel)Enum.ToObject(typeof(ProcessModelLogEventOnProcessModel), this["logEventOnProcessModel"]); }
            set { this["logEventOnProcessModel"] = (long)value; }
        }

        public long MaxProcesses
        {
            get { return Convert.ToInt64(this["maxProcesses"]); }
            set { this["maxProcesses"] = Convert.ToUInt32(value); }
        }

        public string Password
        {
            get { return (string)this["password"]; }
            set { this["password"] = value; }
        }

        public bool PingingEnabled
        {
            get { return (bool)this["pingingEnabled"]; }
            set { this["pingingEnabled"] = value; }
        }

        public TimeSpan PingInterval
        {
            get { return (TimeSpan)this["pingInterval"]; }
            set { this["pingInterval"] = value; }
        }

        public TimeSpan PingResponseTime
        {
            get { return (TimeSpan)this["pingResponseTime"]; }
            set { this["pingResponseTime"] = value; }
        }

        public TimeSpan ShutdownTimeLimit
        {
            get { return (TimeSpan)this["shutdownTimeLimit"]; }
            set { this["shutdownTimeLimit"] = value; }
        }

        public TimeSpan StartupTimeLimit
        {
            get { return (TimeSpan)this["startupTimeLimit"]; }
            set { this["startupTimeLimit"] = value; }
        }

        public string UserName
        {
            get { return this["userName"].ObjectToString(); }
            set { this["userName"] = value; }
        }

        public override string ToString()
        {
            switch (IdentityType)
            {
                case ProcessModelIdentityType.LocalSystem:
                case ProcessModelIdentityType.LocalService:
                case ProcessModelIdentityType.NetworkService:
                    return IdentityType.ToString();
                case ProcessModelIdentityType.ApplicationPoolIdentity:
                    return $"IIS AppPool\\{ParentElement["name"]}";
                case ProcessModelIdentityType.SpecificUser:
                    return this.UserName;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
