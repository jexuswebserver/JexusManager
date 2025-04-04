// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

/*
 * Created by SharpDevelop.
 * User: lextm
 * Time: 11:06 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

namespace JexusManager.Features.RequestFiltering
{
    using System;
    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client;

    /// <summary>
    /// Description of DefaultDocumentFeature.
    /// </summary>
    internal abstract class RequestFilteringFeature<T> : FeatureBase<T>, IRequestFilteringFeature
        where T : class, IItem<T>
    {
        protected RequestFilteringFeature(Module module)
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
            this.RequestFilteringSettingsUpdate?.Invoke();
        }

        public abstract bool ShowHelp();

        public RequestFilteringSettingsSavedEventHandler RequestFilteringSettingsUpdate { get; set; }

        public string Description { get; }

        public virtual Version MinimumFrameworkVersion
        {
            get { return FxVersionNotRequired; }
        }

        public abstract string Name { get; }
    }
}
