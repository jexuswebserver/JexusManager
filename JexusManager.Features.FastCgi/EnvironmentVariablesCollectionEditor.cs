// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.FastCgi
{
    using System;
    using System.ComponentModel.Design;

    internal class EnvironmentVariablesCollectionEditor : CollectionEditor
    {
        public EnvironmentVariablesCollectionEditor(Type type)
            : base(type)
        {
        }

        protected override string GetDisplayText(object value)
        {
            EnvironmentVariables item = (EnvironmentVariables)value;
            return item.Name;
        }
    }
}