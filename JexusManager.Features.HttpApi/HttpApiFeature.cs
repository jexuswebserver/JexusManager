// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Web.Management.Client;
using System;

namespace JexusManager.Features.HttpApi
{
    internal abstract class HttpApiFeature<T> : FeatureBase<T>, IHttpApiFeature
        where T : class, IItem<T>
    {
        protected HttpApiFeature(Module module)
            : base(module)
        {
        }

        protected static readonly Version FxVersion10 = new Version("1.0");
        protected static readonly Version FxVersion11 = new Version("1.1");
        protected static readonly Version FxVersion20 = new Version("2.0");
        protected static readonly Version FxVersionNotRequired = new Version();

        public abstract TaskList GetTaskList();

        public abstract void Load();

        protected override void OnSettingsSaved()
        {
            this.HttpApiSettingsUpdate?.Invoke();
        }

        public abstract bool ShowHelp();

        public HttpApiSettingsSavedEventHandler HttpApiSettingsUpdate { get; set; }

        public string Description { get; }

        public virtual Version MinimumFrameworkVersion
        {
            get { return FxVersionNotRequired; }
        }

        public abstract string Name { get; }
    }
}
