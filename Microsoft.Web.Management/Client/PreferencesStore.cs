// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Web.Management.Client
{
    public sealed class PreferencesStore
    {
        public bool ContainsValue(
            string name
            )
        {
            return false;
        }

        public bool GetValue(
            string name,
            bool defaultValue
            )
        {
            return false;
        }

        public int GetValue(
            string name,
            int defaultValue
            )
        {
            return 0;
        }

        public string GetValue(
            string name,
            string defaultValue
            )
        {
            return null;
        }

        public void Reset() { }
        public void ResetValue(
            string name
            )
        { }

        public void SetValue(
            string name,
            bool value,
            bool defaultValue
            )
        { }

        public void SetValue(
            string name,
            int value,
            int defaultValue
            )
        { }

        public void SetValue(
            string name,
            string value,
            string defaultValue
            )
        { }

        public bool IsEmpty { get; }
    }
}