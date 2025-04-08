// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.ComponentModel;
using Microsoft.Web.Management.Client.Win32;

namespace Microsoft.Web.Management.Client
{
    public abstract class PropertyGridObject : ICustomTypeDescriptor
    {
        protected PropertyGridObject(
            ModulePropertiesPage page
            ) : this(page, false)
        { }

        protected PropertyGridObject(
            ModulePropertiesPage page,
            bool readOnly
            )
        {
            Page = page;
            ReadOnly = readOnly;
        }

        protected DisplayNameAttribute GetDisplayNameAttribute(
            string friendlyName,
            string configPropertyName
            )
        {
            return null;
        }

        protected virtual PropertyDescriptorCollection GetProperties(
            Attribute[] attributes
            )
        {
            return null;
        }

        AttributeCollection ICustomTypeDescriptor.GetAttributes()
        {
            throw new NotImplementedException();
        }

        string ICustomTypeDescriptor.GetClassName()
        {
            throw new NotImplementedException();
        }

        string ICustomTypeDescriptor.GetComponentName()
        {
            throw new NotImplementedException();
        }

        TypeConverter ICustomTypeDescriptor.GetConverter()
        {
            throw new NotImplementedException();
        }

        EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
        {
            throw new NotImplementedException();
        }

        PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
        {
            throw new NotImplementedException();
        }

        object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
        {
            throw new NotImplementedException();
        }

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
        {
            throw new NotImplementedException();
        }

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
        {
            throw new NotImplementedException();
        }

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
        {
            throw new NotImplementedException();
        }

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
        {
            throw new NotImplementedException();
        }

        object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
        {
            throw new NotImplementedException();
        }

        protected ModulePropertiesPage Page { get; }

        protected bool ReadOnly { get; }
    }
}
