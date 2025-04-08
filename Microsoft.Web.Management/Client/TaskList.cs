// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;

namespace Microsoft.Web.Management.Client
{
    public abstract class TaskList
    {
        public virtual object GetPropertyValue(string propertyName)
        {
            return null;
        }

        public abstract ICollection GetTaskItems();

        public virtual object InvokeMethod(string methodName, object userData)
        {
            var type = GetType();
            dynamic method = type.GetMethod(methodName);
            return method == null ? null : method.Invoke(this, userData == null ? null : new[] { userData });
        }

        public virtual void SetPropertyValue(string propertyName, object value)
        { }

        [Obsolete]
        public virtual bool IsDirty { get; }
    }
}
