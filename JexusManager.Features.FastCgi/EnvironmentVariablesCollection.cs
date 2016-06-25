// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.FastCgi
{
    using System.Collections;

    internal class EnvironmentVariablesCollection : CollectionBase
    {
        public EnvironmentVariables this[int index]
        {
            get
            {
                return (EnvironmentVariables)this.List[index];
            }
        }

        public void Add(EnvironmentVariables variable)
        {
            this.List.Add(variable);
        }

        public void Remove(EnvironmentVariables variable)
        {
            this.List.Remove(variable);
        }
    }
}
